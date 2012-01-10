/*
 * Created with SharpDevelop 3.
 * User: F. Phoenix
 * Date: 08.01.2012
 * Time: 10:29
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace PO_Tool
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			cbFormatIDs.SelectedIndex = cbFormatStings.SelectedIndex = cbFormatLinks.SelectedIndex = 0;
			bSourceBrowse.Tag = cbSourceFile;
			bUpdateBrowse.Tag = cbUpdateFile;
			bDestBrowse.Tag = cbDestFile;
			
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
						else if ((category != "SOURCE MRU" && category != "UPDATE MRU") || File.Exists(str))
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
		}
		
		
		void bStart_Click(object sender, EventArgs e)
		{
			string file_src = cbSourceFile.Text, file_upd = cbUpdateFile.Text, file_dest = cbDestFile.Text;
			
			bool upd = (file_upd.Length > 0);
			short error = 0;
			if (!File.Exists(file_src)) error = 1;
			else if (upd && !File.Exists(file_upd)) error = 2;
			else if (file_dest.Length == 0) error = 3;
			
			Parser parser_src = new Parser(file_src), paser_upd = new Parser(file_upd);
			if (error == 0 && !parser_src.Parse()) error = 4;
			if (error == 0 && upd && !paser_upd.Parse()) error = 5;
			
			var data = new List<string>();
			
			#region Применение изменений
			bool wri_strings = !chRemStrings.Checked, wri_com = !chRemComments.Checked, wri_autocom = !chRemAutoComments.Checked, wri_links = !chRemLinks.Checked, wri_flags = !chRemFlags.Checked;
			bool upd_header = (upd && chUpdHeader.Checked), upd_strings = (upd && chUpdStrings.Checked), upd_com = (upd && chUpdComments.Checked), upd_autocom = (upd && chUpdAutoComments.Checked), upd_links = (upd && chUpdLinks.Checked), upd_flags = (upd && chUpdFlags.Checked);
			if (error == 0)
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
						if (wri_links)  block.AddRange(((upd_links && iupd >= 0 ? bupd : bsrc) as GettextBlock).FormatLinks(cbFormatLinks.SelectedIndex));
						if (wri_flags)  block.AddRange(((upd_flags && iupd >= 0 ? bupd : bsrc) as GettextBlock).Flags);
						block.AddRange(bsrc.FormatMsgID(cbFormatIDs.SelectedIndex));
						if (bsrc.IDPlural != null)
							block.AddRange(bsrc.FormatMsgIDPlural(cbFormatIDs.SelectedIndex));
						GettextBlock buse = (upd_strings && iupd >= 0 ? bupd : bsrc);
						if (buse.StrInd.Length == 0)
						{
							if (wri_strings)
								block.AddRange(buse.FormatMsgStr(cbFormatStings.SelectedIndex));
							else
								block.Add("msgstr \"\"");
						}
						else
						{
							for (int i = 0; i < buse.StrInd.Length; i ++)
							{
								if (wri_strings)
									block.AddRange(buse.FormatMsgStrInd(i, cbFormatStings.SelectedIndex));
								else
									block.Add("msgstr \"[" + i + "]\"");
							}
						}
						
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
					if (chRemRegions.Checked)
					{
						var rx = new Regex("^#(region|endregion|[1-9])");
						for (int i = data.Count - 1; i >= 0; i --)
							if (rx.IsMatch(data[i]))
								data.RemoveAt(i);
					}
				}
				catch
				{
					error = 6;
				}
			}
			#endregion
			
			#region Запись нового файла
			if (error == 0)
			{
				try
				{
					var sw = new StreamWriter(file_dest + ".tmp");
					foreach (string s in data)
						sw.WriteLine(s);
					sw.Close();
					// Net нормально понимает и просто реплейс для любых ситуаций. Mono - нифига.
					if (File.Exists(file_dest))
						File.Replace(file_dest + ".tmp", file_dest, file_dest + ".bak");
					else
						File.Move(file_dest + ".tmp", file_dest);
				}
				catch
				{
					error = 7;
				}
			}
			#endregion
			
			string result;
			switch (error)
			{
				case 1: result = "Неверно указан исходный файл!"; break;
				case 2: result = "Неверно указан файл обновления!"; break;
				case 3: result = "Не указан целевой файл!"; break;
				case 4: result = "Не удалось проанализировать исходный файл."; break;
				case 5: result = "Не удалось проанализировать файл обновления."; break;
				case 6: result = "Не удалось обработать файл."; break;
				case 7: result = "Не удалось записать целевой файл."; break;
				default:
					bool updated = false;
					if (upd)
						foreach (CheckBox c in gUpdate.Controls)
							if (c.Checked) updated = true;
					result = "Файл благополучно " + (updated ? "обновлен." : "отформатирован.");
					TextToItem(cbSourceFile);
					TextToItem(cbUpdateFile);
					TextToItem(cbDestFile);
					break;
			}
			MessageBox.Show(result, error == 0 ? "Готово" : "Ошибка");
		}
		
		
		void bBrowse_Click(object sender, EventArgs e)
		{
			ComboBox tag = (ComboBox)(((Button)sender).Tag);
			openFileDialog.InitialDirectory = GetDir(tag.Text);
			FileDialog fd = (sender == bDestBrowse ? (saveFileDialog as FileDialog) : (openFileDialog as FileDialog));
			if (fd.ShowDialog() == DialogResult.OK)
				tag.Text = fd.FileName;
		}
		
		void cbFiles_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
		}
		
		void cbFiles_DragDrop(object sender, DragEventArgs e)
        {
			((ComboBox)sender).Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
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
		}
		
		
		void TextToItem(ComboBox cb)
		{
			if (!cb.Items.Contains(cb.Text))
				cb.Items.Add(cb.Text);
		}
		
		string GetDir(string path)
		{
			int ind = path.LastIndexOfAny(new char[]{'\\','/'});
			return (ind < 0 ? "" : path.Remove(ind));
		}
	}
}
