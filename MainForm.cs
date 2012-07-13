/*
 * Created with SharpDevelop 3.
 * User: F. Phoenix
 * Date: 08.01.2012
 * Time: 10:29
 * 
 */

//#define DebugSourceParser

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

//TODO: Сделать опцию для чистки от ненужных комментариев типа "#. #-#-#-#-#  wesnoth.wml.pot (PACKAGE VERSION)  #-#-#-#-#"

namespace PO_Tool
{
	public partial class MainForm : Form
	{
		#region Переменные и константы
		bool Unix = System.Environment.OSVersion.Platform == PlatformID.Unix;
		
		List<GTFile> Files;
		bool FilesUpdated;
		int FilesError;
		bool Stop;
		
		// Опции
		public bool UpdHeader, UpdPrevIDs, UpdStrings, UpdAltStrings, UpdComments, UpdAutoComments, UpdLinks, UpdFlags;
		public bool RemPrevIDs, RemStrings, RemAltStrings, RemRegions, RemComments, RemAutoComments, RemLinks, RemFlags;
		public int FormatIDs, FormatStrings, FormatLinks;
		
		// Коды ошибок (EG - глобальные, EF - отдельного файла)
		// NOTE: Возможно, это удобно будет переделать на исключения
		const int EG_SrcEmpty = 1, EG_SrcBad = 2, EG_SrcNotExist = 3, EG_UpdBad = 4, EG_TrgEmpty = 5, EG_TrgBad = 6, EG_1Src_ManyUpd = 7, EG_1Src_ManyTrg = 8;
		const int EF_ParserSrc = 10, EF_ParserUpd = 11, EF_Process = 12, EF_Write = 13, EF_Canceled = 14;
		#endregion
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			cbFormatIDs.SelectedIndex = cbFormatStrings.SelectedIndex = cbFormatLinks.SelectedIndex = 0;
			bSourceBrowse.Tag = cbSourceFile;
			bUpdateBrowse.Tag = cbUpdateFile;
			bDestBrowse.Tag = cbDestFile;
			cbSourceFile.Tag = new[]{"Исходный путь", "Укажите папку, в которой находятся исходные файлы."};
			cbUpdateFile.Tag = new[]{"Путь обновления", "Укажите папку, в которой находятся файлы для обновления."};
			cbDestFile.Tag = new[]{"Целевой путь", "Укажите папку, в которую программа запишет обработанные файлы."};
			
			#region Загрузка конфигурации
			var config = Application.StartupPath + "/PO Tool.cfg";
			if (File.Exists(config))
			{
				try
				{
					var rx_cat = new Regex(@"^\[(?<category>[\w\d ]+)\]$");
					var rx_opt = new Regex(@"^(?<var>\w+)\s*=\s*(?<value>\w+)$");
					string category = string.Empty;
					var sr = new StreamReader(config);
					while (!sr.EndOfStream)
					{
						string str = sr.ReadLine().Trim();
						if (str.Length == 0) continue;
						
						var m = rx_cat.Match(str);
						if (m.Success)
							category = m.Groups["category"].Value.ToUpper();
						else //if ((category != "SOURCE MRU" && category != "UPDATE MRU") || File.Exists(str))
						{
							if (!category.Contains("MRU") && !(m = rx_opt.Match(str)).Success) break;
							bool bres;
							int ires;
							switch (category)
							{
								case "UPDATE":
									foreach (CheckBox c in gUpdate.Controls)
										if ((c.Tag as string) == m.Groups["var"].Value && bool.TryParse(m.Groups["value"].Value, out bres))
											c.Checked = bres;
									break;
								case "REMOVE":
									foreach (CheckBox c in gRemove.Controls)
										if ((c.Tag as string) == m.Groups["var"].Value && bool.TryParse(m.Groups["value"].Value, out bres))
											c.Checked = bres;
									break;
								case "FORMAT":
									foreach (ComboBox c in gFormat.Controls)
										if ((c.Tag as string) == m.Groups["var"].Value && int.TryParse(m.Groups["value"].Value, out ires) && ires > 0 && ires < c.Items.Count)
											c.SelectedIndex = ires;
									break;
								case "SOURCE MRU": cbSourceFile.Items.Add(str);	break;
								case "UPDATE MRU": cbUpdateFile.Items.Add(str); break;
								case "TARGET MRU": cbDestFile.Items.Add(str); break;
							}
						}
					}
					sr.Close();
				}
				catch {}
			}
			#endregion
		}
		
		void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			#region Сохранение конфигурации
			try
			{
				var sw = new StreamWriter(Application.StartupPath + "/PO Tool.cfg");
				
				sw.WriteLine("[UPDATE]");
				foreach (CheckBox c in gUpdate.Controls)
					sw.WriteLine((c.Tag as string) + " = " + c.Checked);
				
				sw.WriteLine("\r\n[REMOVE]");
				foreach (CheckBox c in gRemove.Controls)
					sw.WriteLine((c.Tag as string) + " = " + c.Checked);
				
				sw.WriteLine("\r\n[FORMAT]");
				foreach (ComboBox c in gFormat.Controls)
					sw.WriteLine((c.Tag as string) + " = " + c.SelectedIndex);
				
				sw.WriteLine("\r\n[SOURCE MRU]");
				foreach (object o in cbSourceFile.Items)
					sw.WriteLine(o as string);
				
				sw.WriteLine("\r\n[UPDATE MRU]");
				foreach (object o in cbUpdateFile.Items)
					sw.WriteLine(o as string);
				
				sw.WriteLine("\r\n[TARGET MRU]");
				foreach (object o in cbDestFile.Items)
					sw.WriteLine(o as string);
				
				sw.Close();
			}
			catch {}
			#endregion
		}
		
		
		#region Интерфейс
		void bBrowse_Click(object sender, EventArgs e)
		{
			ComboBox tag = (ComboBox)(((Button)sender).Tag);
			
			// Поиск папки
			string dir = tag.Text;
			while (dir.Length > 0 && !Directory.Exists(dir))
			{
				int ind = Unix ? dir.LastIndexOf('/') : dir.LastIndexOfAny(new char[]{'\\','/'});
				dir = (ind > 0 ? dir.Remove(ind) : String.Empty);
			}
			
			// Обзор папок
			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				folderBrowserDialog.Description = (tag.Tag as string[])[1];
				if (dir.Length > 0) folderBrowserDialog.SelectedPath = dir;
				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
					tag.Text = FormatDir(folderBrowserDialog.SelectedPath);
			}
			// Обзор файлов
			else
			{
				FileDialog fd = (sender == bDestBrowse ? (saveFileDialog as FileDialog) : (openFileDialog as FileDialog));
				fd.Title = (tag.Tag as string[])[0];
				if (File.Exists(tag.Text)) fd.FileName = tag.Text;
				else if (dir.Length > 0)
				{
					fd.FileName = String.Empty;
					fd.InitialDirectory = dir;
				}
				if (fd.ShowDialog() == DialogResult.OK)
					tag.Text = fd.FileName;
			}
		}
		
		void cbFiles_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
		}
		
		void cbFiles_DragDrop(object sender, DragEventArgs e)
        {
			((ComboBox)sender).Text = FormatDir(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
        }
		
		void cbUpdateFile_TextChanged(object sender, EventArgs e)
		{
			CheckState cs = (cbUpdateFile.Text.Length == 0 ? CheckState.Indeterminate : CheckState.Checked);
			foreach (CheckBox c in gUpdate.Controls)
				if (c.Checked) c.CheckState = cs;
		}
		
		void chUpdate_CheckedChanged(object sender, EventArgs e)
		{
			var ch = sender as CheckBox;
			if (ch.Checked && cbUpdateFile.Text.Length == 0)
				ch.CheckState = CheckState.Indeterminate;
			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				SetCheckBoxes(gUpdate.Controls, ch.CheckState);
		}
		
		void chRemove_CheckedChanged(object sender, EventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				SetCheckBoxes(gRemove.Controls, (sender as CheckBox).CheckState);
		}
		#endregion
		
		#region Процесс обработки
		//TODO: Непомешало бы это дело в отдельный класс вынести для консольного режима (а возможно и GTK-версии).
		void bStart_Click(object sender, EventArgs e)
		{
			if (backgroundWorker.IsBusy)
			{
				Stop = true;
				return;
			}
			
			cbSourceFile.Text = FormatDir(cbSourceFile.Text);
			cbUpdateFile.Text = FormatDir(cbUpdateFile.Text);
			cbDestFile.Text = FormatDir(cbDestFile.Text);
			
			FilesUpdated = false;
			FilesError = 0;
			Files = new List<GTFile>();
			
			#region Определение таблицы файлов
			// Если целевой путь не указан, то изменяются исходные файлы.
			if (cbSourceFile.Text.Length == 0) FilesError = EG_SrcEmpty;
			else
			{
				FileInfo[] src_files, upd_files;
				string[] src_filter, upd_filter;
				bool src_solo = GetFiles(cbSourceFile.Text, out src_files, out src_filter);
				bool upd_solo = GetFiles(cbUpdateFile.Text, out upd_files, out upd_filter);
				string dest = (cbDestFile.Text.Length == 0 ? cbSourceFile : cbDestFile).Text;
				if (src_files.Length == 0) FilesError = (src_solo ? EG_SrcBad : EG_SrcNotExist);
				else if (cbUpdateFile.Text.Length > 0 && upd_files.Length == 0) FilesError = EG_UpdBad;
				else if (src_solo && upd_filter[1].Contains("*")) FilesError = EG_1Src_ManyUpd;
				else if (src_solo && dest.Contains("*")) FilesError = EG_1Src_ManyTrg;
				else
				{
					//TODO: Сделать проверку на создаваемый файл/папку.
					var dest_filter = SplitPath(dest);
					if (dest_filter[0].Length == 0) dest_filter[0] = src_filter[0];
					var dest_ext = GetExt(dest_filter[1]);
					bool keep_ext = dest_ext.Contains("*");
					foreach (FileInfo fs in src_files)
					{
						string upd_file = string.Empty;
						if (upd_solo) upd_file = upd_files[0].FullName;
						else
						{
							bool any_ext = (src_filter != upd_filter && !GetExt(src_filter[1]).Contains("*") && !GetExt(upd_filter[1]).Contains("*"));
							foreach (FileInfo fu in upd_files)
								if ((any_ext && GetName(fs) == GetName(fu)) || fs.Name == fu.Name)
								{
									upd_file = fu.FullName;
									break;
								}
						}
						Files.Add(new GTFile(fs.FullName, upd_file, (src_solo ? dest : dest_filter[0] + "/" + (keep_ext ? fs.Name : GetName(fs) + dest_ext))));
					}
					if (Files.Count == 0) FilesError = EG_SrcNotExist;
				}
			}
			#endregion
			
			if (FilesError == 0)
			{
				var fields = this.GetType().GetFields();
				SetFields(gUpdate.Controls, fields);
				SetFields(gRemove.Controls, fields);
				SetFields(gFormat.Controls, fields);
				
				Stop = false;
				if (Files.Count > 1)
				{
					bStart.Text = "Стоп";
					progressBar.Value = 0;
					progressBar.Maximum = Files.Count;
					progressBar.Visible = true;
				}
			}
			backgroundWorker.RunWorkerAsync();
		}
		
		void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			#region Обработка файлов
			if (FilesError == 0)
			{
				for (int progress = 0; progress < Files.Count; progress ++)
				{
					var file = Files[progress];
					FilesError = (Stop ? EF_Canceled : 0);

					string file_src = file.Source, file_upd = file.Update, file_dest = file.Target;
					bool upd = (file_upd.Length > 0);
					FilesUpdated = (FilesUpdated || upd);
					Parser parser_src = new Parser(file_src), paser_upd = new Parser(file_upd);
					if (FilesError == 0 && !parser_src.Parse()) FilesError = EF_ParserSrc;
					if (FilesError == 0 && upd && !paser_upd.Parse()) FilesError = EF_ParserUpd;
					
					var data = new List<string>();
					
					#region Отладка парсера
					#if DebugSourceParser
					try
					{
						var sr = new StreamReader(file_src);
						data.AddRange(sr.ReadToEnd().Replace("\r\n", "\n").Split('\n'));
						sr.Close();
						Directory.CreateDirectory(SplitPath(file_dest)[0]);
						var sw = new StreamWriter(file_dest + "~dbg.txt");
						for (int bl = 0; bl < parser_src.Data.Count; bl ++)
						{
							var block = parser_src.Data[bl];
							sw.WriteLine(String.Format("——————————— Block {0} ———————————", bl));
							for (int ln = 0; ln < block.LinesCount; ln++)
								sw.WriteLine(data[block.StartLine + ln]);
						}
						sw.Close();
					}
					catch (Exception ex)
					{
						System.Diagnostics.Trace.WriteLine(ex.ToString());
					}
					data.Clear();
					#endif
					#endregion
					
					#region Применение изменений
					bool wri_prev = !RemPrevIDs, wri_strings = !RemStrings, wri_alt = !RemAltStrings, wri_com = !RemComments, wri_autocom = !RemAutoComments, wri_links = !RemLinks, wri_flags = !RemFlags;
					bool upd_header = (upd && UpdHeader), upd_prev = (upd && UpdPrevIDs), upd_strings = (upd && UpdStrings), upd_alt = (upd && UpdAltStrings), upd_com = (upd && UpdComments), upd_autocom = (upd && UpdAutoComments), upd_links = (upd && UpdLinks), upd_flags = (upd && UpdFlags);
					if (FilesError == 0)
					{
						try
						{
							var sr = new StreamReader(file_src);
							data.AddRange(sr.ReadToEnd().Replace("\r\n", "\n").Split('\n'));
							sr.Close();
							for (int isrc = parser_src.Data.Count - 1; isrc >= 0; isrc --)
							{
								var bsrc = parser_src.Data[isrc];
								int iupd = paser_upd.Data.IndexOf(bsrc.ID);
								var bupd = (iupd < 0 ? null : paser_upd.Data[iupd]);
								var block = new List<string>();
								if (wri_com)  block.AddRange(((upd_com && iupd >= 0 ? bupd : bsrc) as GettextBlock).Comments);
								if (wri_autocom)  block.AddRange(((upd_autocom && iupd >= 0 ? bupd : bsrc) as GettextBlock).AutoComments);
								if (wri_links)  block.AddRange(((upd_links && iupd >= 0 ? bupd : bsrc) as GettextBlock).FormatLinks(FormatLinks));
								if (wri_flags)  block.AddRange(((upd_flags && iupd >= 0 ? bupd : bsrc) as GettextBlock).Flags);
								if (wri_prev)	block.AddRange(((upd_prev && iupd >= 0 ? bupd : bsrc) as GettextBlock).PrevIDs);
								block.AddRange(bsrc.FormatMsgID(FormatIDs));
								if (bsrc.IDPlural != null)
									block.AddRange(bsrc.FormatMsgIDPlural(FormatIDs));
								GettextBlock buse = (upd_strings && iupd >= 0 ? bupd : bsrc);
								if (buse.StrInd.Length == 0)
								{
									if (wri_strings)
										block.AddRange(buse.FormatMsgStr(FormatStrings));
									else
										block.Add("msgstr \"\"");
								}
								else
								{
									for (int i = 0; i < buse.StrInd.Length; i ++)
									{
										if (wri_strings)
											block.AddRange(buse.FormatMsgStrInd(i, FormatStrings));
										else
											block.Add("msgstr \"[" + i + "]\"");
									}
								}
								if (wri_alt)  block.AddRange(((upd_alt && iupd >= 0 ? bupd : bsrc) as GettextBlock).AltStrings);
								
								data.RemoveRange(bsrc.StartLine, bsrc.LinesCount);
								data.InsertRange(bsrc.StartLine, block);
							}
							if (upd_header)
							{
								data.RemoveRange(parser_src.Header.StartLine, parser_src.Header.LinesCount);
								data.RemoveRange(0, parser_src.HeaderFirstLines);
								data.InsertRange(0, paser_upd.Header.Comments);
								int cur = paser_upd.Header.Comments.Count;
								data.Insert(cur++, "msgid \"\"");
								data.InsertRange(cur, paser_upd.Header.FormatMsgStr(2));
							}
							if (RemRegions)
							{
								var rx = new Regex("^#(region|endregion|[1-9])");
								for (int i = data.Count - 1; i >= 0; i --)
									if (rx.IsMatch(data[i]))
										data.RemoveAt(i);
							}
						}
						catch //(Exception ex)
						{
							FilesError = EF_Process;
							//System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
					#endregion
					
					#region Запись нового файла
					if (FilesError == 0)
					{
						try
						{
							Directory.CreateDirectory(SplitPath(file_dest)[0]);
							var sw = new StreamWriter(file_dest + ".tmp");
							sw.Write(String.Join("\r\n", data.ToArray()));
//							foreach (string s in data)
//								sw.WriteLine(s);
							sw.Close();
							// Net нормально понимает и просто реплейс для любых ситуаций. Mono - нифига.
							if (File.Exists(file_dest))
								File.Replace(file_dest + ".tmp", file_dest, file_dest + ".bak");
							else
								File.Move(file_dest + ".tmp", file_dest);
						}
						catch
						{
							FilesError = EF_Write;
						}
					}
					#endregion
					
					file.Error = FilesError;
					if (Files.Count > 1) FilesError = 0;
					backgroundWorker.ReportProgress(progress + 1);
				}
			}
			#endregion
		}
		
		void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			progressBar.Value = e.ProgressPercentage;
		}
		
		void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			progressBar.Visible = false;
			bStart.Text = "Поехали";
			
			#region Вывод результата
			string message;
			string[] prc_errors = new string[]
			{
				"Не удалось проанализировать исходный файл.",
				"Не удалось проанализировать файл обновления.",
				"Не удалось обработать файл.",
				"Не удалось записать целевой файл.",
				"Обработка файла отменена."
			};
			
			switch (FilesError)
			{
				case EG_SrcEmpty: message = "Не указан исходный файл!"; break;
				case EG_SrcBad: message = "Неверно указан исходный файл!"; break;
				case EG_SrcNotExist: message = "Исходные файлы не найдены!"; break;
				case EG_UpdBad: message = "Неверно указан файл обновления!"; break;
				case EG_TrgEmpty: message = "Не указан целевой файл!"; break;
				case EG_TrgBad: message = "Неверно указан целевой файл!"; break;
				case EG_1Src_ManyUpd: message = "Нельзя использовать несколько файлов обновлений для единственного исходного!"; break;
				case EG_1Src_ManyTrg: message = "Нельзя использовать несколько целевых файлов для единственного исходного!"; break;
				default:
					bool updated = false;
					if (FilesUpdated)
						foreach (CheckBox c in gUpdate.Controls)
							if (c.Checked) updated = true;
					if (Files.Count == 1)
					{
						message = (Files[0].Error == 0)
							? String.Format("Файл благополучно {0}.", (updated ? "обновлен" : "отформатирован"))
							: prc_errors[Files[0].Error - 10];
					}
					else
					{
						var sb = new System.Text.StringBuilder();
						foreach (GTFile file in Files)
							if (file.Error > 0)
								sb.AppendFormat("{0} - {1}\r\n", SplitPath(file.Source)[1], prc_errors[file.Error - 10]);
						message = (sb.Length == 0)
							? String.Format("Файлы благополучно {0}.", (updated ? "обновлены" : "отформатированы"))
							: String.Format("Не все файлы удалось {0}:\r\n\r\n{1}", (updated ? "обновить" : "отформатировать"), sb.ToString());
					}
					TextToItem(cbSourceFile);
					TextToItem(cbUpdateFile);
					TextToItem(cbDestFile);
					break;
			}

			MessageBox.Show(message, FilesError == 0 ? "Готово" : "Ошибка", MessageBoxButtons.OK, FilesError == 0 ? MessageBoxIcon.Asterisk : MessageBoxIcon.Error);
			#endregion
		}
		#endregion
		
		#region Вспомогательные функции
		//TODO: Тут много что можно заменить стандартными функциями Path.
		
		/// <summary> Добавляет текущий текст ComboBox к списку его объектов. </summary>
		void TextToItem(ComboBox cb)
		{
			if (!cb.Items.Contains(cb.Text))
				cb.Items.Add(cb.Text);
		}
		
		/// <summary> Разделяет путь на папку [0] и файл [1]. </summary>
		string[] SplitPath(string path)
		{
			if (Directory.Exists(path))
				return new string[]{path, ""};
			else
			{
				int ind = Unix ? path.LastIndexOf('/') : path.LastIndexOfAny(new char[]{'\\','/'});
				return (ind < 0)
					? new string[]{"", path}
					: new string[]{path.Remove(ind), path.Substring(ind + 1)};
			}
		}
		
		/// <summary> Если путь указывает на папку, то добавляет к нему "*.po". </summary>
		string FormatDir(string path)
		{
			if (path.Length > 0 && Directory.Exists(path))
			{
				if (!path.EndsWith("/") && (Unix || !path.EndsWith(@"\")))
					path += (Unix ? '/' : path[2]);
				path += "*.po";
			}
			return path;
		}
		
		/// <summary> Ищет файлы по указанному пути. </summary>
		/// <param name="path"> </param>
		/// <param name="files"> Список найденных файлов. </param>
		/// <param name="filter"> [0] - папка; [1] - файл (маска). </param>
		/// <returns> true, если запрошен и найден 1 файл; иначе false. </returns>
		bool GetFiles(string path, out FileInfo[] files, out string[] filter)
		{
			bool simple = File.Exists(path);
			filter = SplitPath(path);
			var sf = (simple
			          ? new[]{path} // если указан и найден 1 файл, то его и возвращаем
			          : (Directory.Exists(filter[0])
			             ? Directory.GetFiles(filter[0], filter[1]) // если нет, но существует папка, ищем в ней
			             : new string[]{})); // если и папки нет, то вернем пустой массив
			files = new FileInfo[sf.Length];
			for (int i = 0; i < sf.Length; i ++)
				files[i] = new FileInfo(sf[i]);
			return simple;
		}
		
		/// <summary> Возвращает имя файла без расширения. </summary>
		string GetName(FileInfo fi)
		{
			//NOTE: Это можно заменить на метод Path.GetFileNameWithoutExtension(string path)
			return fi.Name.Remove(fi.Name.Length - fi.Extension.Length);
		}
		
		/// <summary> Возвращает расширение файла. </summary>
		string GetExt(string filename)
		{
			//NOTE: Это можно заменить на метод Path.GetExtension(string path)
			int ind = filename.LastIndexOf('.');
			return (ind < 0 ? "" : filename.Substring(ind));
		}

		/// <summary> Сохраняет состояние контролов в соответствующих переменных. </summary>
		void SetFields(Control.ControlCollection controls, FieldInfo[] fields)
		{
			foreach (Control c in controls)
				foreach (FieldInfo f in fields)
					if (f.Name == c.Name.Substring(2))
					{
						if (c is CheckBox)
							f.SetValue(this, (c as CheckBox).Checked);
						else if (c is ComboBox)
							f.SetValue(this, (c as ComboBox).SelectedIndex);
					}
		}
		
		/// <summary> Устанавливает группе чекбоксов указанное состояние. </summary>
		void SetCheckBoxes(Control.ControlCollection controls, CheckState state)
		{
			foreach (Control c in controls)
				if (c is CheckBox) (c as CheckBox).CheckState = state;
		}
		#endregion
		
		class GTFile
		{
			public GTFile(string source, string update, string target)
			{
				Source = source;
				Update = update;
				Target = target;
				Error = 0;
			}
			
			public string Source, Update, Target;
			public int Error;
		}
	}
}
