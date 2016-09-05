/****************************\
 * Создано в SharpDevelop.
 * Автор: F.Phoenix
 * Дата: 08.01.2012 11:02
\****************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PO_Tool
{
	public class Parser
	{
		string SourceFile;
		public GettextBlock Header {get; private set;}
		public int HeaderFirstLines {get; private set;}
		public GettextBlockList Data {get; private set;}
		
		public Parser(string file)
		{
			SourceFile = file;
			Header = new GettextBlock();
			HeaderFirstLines = 0;
			Data = new GettextBlockList();
		}
		
		public Parser(string file, out bool result) : this(file)
		{
			result = Parse();
		}
		
		
		public bool Parse()
		{
			if (!File.Exists(SourceFile)) return false;
			try
			{
				var rx_regions = new Regex("^#(region|endregion|[1-9]).*$");
				var rx_strings = new Regex("^(?<type>msgctxt|msgid|msgid_plural|msgstr|msgstr\\[(?<index>\\d)\\])\\s\"(?<string>.*)\"$");
				var rx_srings_continue = new Regex("^\"(?<string>.*)\"$");
				var rx_error = new Regex("^(msgid\\s\"|(#[,:.\\|]))");
				var block = new GettextBlock();
				string str = String.Empty;
				bool end = false, close_block = false, keep_line = false;
				bool header = true;
				Match m;
				
				var sr = new StreamReader(SourceFile);
				for (int line = 0; !end; line ++)
				{
					end = sr.EndOfStream;
					if (keep_line) line --;
					else str = (end ? String.Empty : sr.ReadLine().Trim());
					keep_line = false;

					if ((close_block = (str.Length == 0)) == false)
					{
						bool is_data = false;
						
						//UNDONE: Хреновенько, но пока хоть так, чтоб строки не удалял.
						if (block.ID != null && rx_error.IsMatch(str))
							close_block = keep_line = true;
						
						#region Комментарии
						else if (str.StartsWith("#"))
						{
							if (rx_regions.IsMatch(str))
								close_block = true;
							else
							{
								is_data = true;
								
								if (str.StartsWith("#,"))
									block.Flags.Add(str);
								else if (str.StartsWith("#:"))
									block.Links.Add(str.Substring(2));
								else if (str.StartsWith("#."))
									block.AutoComments.Add(str);
								else if (str.StartsWith("#|"))  // TODO: Сделать поддержку форматирования предыдущих ID
									block.PrevIDs.Add(str);
								else if (str.StartsWith("#*"))
									block.AltStrings.Add(str);
								else
									block.Comments.Add(str);
							}
						}
						#endregion
						
						#region Строки
						else if ((m = rx_strings.Match(str)).Success)
						{
							keep_line = true;
							if (block.StartLine < 0)
								block.StartLine = line;
							string gs = String.Empty;
							var breaks = new List<int>();
							bool add_break = false;
							Match gm = m;
							
							// Добавления всех последующих обрамленных кавычками строк
							do
							{
								if (add_break)
									breaks.Add(gs.Length);
								gs += gm.Groups["string"].Value;
								if ((end = sr.EndOfStream) == false)
									str = sr.ReadLine().Trim();
								line ++;
								add_break = true;
							}
							while(!end && (gm = rx_srings_continue.Match(str)).Success);
							close_block = end;
							
							switch (m.Groups["type"].Value)
							{
								case "msgctxt":
									block.Context = gs;
									break;
								case "msgid":
									block.ID = gs;
									block.ID_Breaks = breaks;
									break;
								case "msgid_plural":
									block.IDPlural = gs;
									block.IDPlural_Breaks = breaks;
									break;
								case "msgstr":
									block.Str = gs;
									block.Str_Breaks = breaks;
									break;
								default:
									int ind = int.Parse(m.Groups["index"].Value);
									if (block.StrInd.Length < ind + 1)
									{
										Array.Resize(ref block.StrInd, ind + 1);
										Array.Resize(ref block.StrInd_Breaks, ind + 1);
									}
									block.StrInd[ind] = gs;
									block.StrInd_Breaks[ind] = breaks;
									break;
							}
						}
						#endregion
						
						if (is_data && block.StartLine < 0)
							block.StartLine = line;
					}
					
					#region Завершение блока
					if (close_block)
					{
						if (block.ID != null)
						{
							block.LinesCount = line - block.StartLine;
							if (block.ID.Length == 0)
							{
								block.Comments.InsertRange(0, Header.Comments);
								Header = block;
								if (HeaderFirstLines > Header.StartLine)
									HeaderFirstLines = Header.StartLine;
							}
							else Data.Add(block);
							header = false;
						}
						else if (header)
						{
							Header.Comments.AddRange(block.Comments);
							if (str.Length == 0 && Header.ID == null)
								Header.Comments.Add(string.Empty);
							HeaderFirstLines = line + 1;
						}
						block = new GettextBlock();
					}
					#endregion
				}
				
				sr.Close();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
	
	
	public class GettextBlock
	{
		public GettextBlock()
		{
			StartLine = LinesCount = -1;
			Context = ID = IDPlural = null;
			Str = string.Empty;
			StrInd = new string[0];
			Flags = new List<string>();
			Comments = new List<string>();
			AutoComments = new List<string>();
			Links = new List<string>();
			PrevIDs = new List<string>();
			AltStrings = new List<string>();
			ID_Breaks = new List<int>();
			IDPlural_Breaks = new List<int>();
			Str_Breaks = new List<int>();
			StrInd_Breaks = new List<int>[0];
		}
		
		public int StartLine, LinesCount;
		public string Context, ID, IDPlural, Str;
		public string[] StrInd;
		public List<string> Flags, Comments, AutoComments, Links, PrevIDs, AltStrings;
		public List<int> ID_Breaks, IDPlural_Breaks, Str_Breaks;
		public List<int>[] StrInd_Breaks;
		
		// FIXME: При отсутствии ссылок их слияние добавляет пустую #:
		public string[] FormatLinks(int format)
		{
			string[] result;
			
			if (format >=1 && format <= 2)
			{
				var tmp = new List<string>();
				foreach (string s in Links)
					tmp.AddRange(s.Split(new char[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries));
				if (format == 1)
				{
					result = new string[1];
					result[0] = "#: " + String.Join(" ", tmp.ToArray());
				}
				else
				{
					result = new string[tmp.Count];
					for (int i = 0; i < tmp.Count; i ++)
						result[i] = "#: " + tmp[i];
				}
			}
			else
			{
				result = new string[Links.Count];
				for (int i = 0; i < Links.Count; i ++)
					result[i] = "#:" + Links[i];
			}
			
			return result;
		}
		
		public string FormatContext()
		{
			return string.Format("msgctxt \"{0}\"", Context);
		}
		
		public string[] FormatMsgID(int format)
		{
			string[] result = FormatString(ID, ID_Breaks, format);
			result[0] = "msgid " + result[0];
			return result;
		}
		
		public string[] FormatMsgIDPlural(int format)
		{
			string[] result = FormatString(IDPlural, IDPlural_Breaks, format);
			result[0] = "msgid_plural " + result[0];
			return result;
		}
		
		public string[] FormatMsgStr(int format)
		{
			string[] result = FormatString(Str, Str_Breaks, format);
			result[0] = "msgstr " + result[0];
			return result;
		}
		
		public string[] FormatMsgStrInd(int index, int format)
		{
			string[] result = FormatString(StrInd[index], StrInd_Breaks[index], format);
			result[0] = String.Format("msgstr[{0}] {1}", index, result[0]);
			return result;
		}
		
		
		string[] FormatString(string src, List<int> br, int format)
		{
			var lstr = new List<string>();
			switch (format)
			{
				case 0: // Сохранить исходные переносы
					if (br.Count == 0)
						lstr.Add(src);
					else
					{
						lstr.Add(src.Substring(0, br[0]));
						for (int i = 0; i < br.Count - 1; i ++)
							lstr.Add(src.Substring(br[i], br[i + 1] - br[i]));
						lstr.Add(src.Substring(br[br.Count - 1]));
					}
					for (int i = 0; i < lstr.Count; i ++)
						lstr[i] = "\"" + lstr[i] + "\"";
					break;
				case 1: // Слить все в одну строку
					lstr.Add("\"" + src + "\"");
					break;
				case 2: // Разделить строки по \n (строго)
				case 3: // Разделить строки по \n (сокращенно)
					lstr.AddRange(src.Split(new string[]{@"\n"}, StringSplitOptions.None));
					if (lstr.Count > 1)
					{
						if (format == 3) // оптимизация переносов
						{
							var start = String.Empty;
							while (lstr.Count > 1 && lstr[0].Length == 0)
							{
								start += "\\n";
								lstr.RemoveAt(0);
							}
							lstr[0] = start + lstr[0];
							for (int i = 0; i < lstr.Count - 1; i ++)
								if (lstr[i + 1].Length == 0)
								{
									lstr[i] += "\\n";
									lstr.RemoveAt(i + 1);
									i --;
								}
						}
						for (int i = 0; i < lstr.Count - 1; i ++) // добавление кавычек и переноса ко всем строкам, кроме последней
							lstr[i] = "\"" + lstr[i] + "\\n\"";
						if (lstr.Count == 2 && lstr[1].Length == 0)
							return new string[]{lstr[0]};
						if (lstr.Count > 1) lstr.Insert(0, "\"\""); // добавление первой пустой строки в многострочном тексте
					}
					if (lstr.Count > 1 && lstr[lstr.Count - 1].Length == 0)
						lstr.RemoveAt(lstr.Count - 1); // удаление последней пустой строки
					else lstr[lstr.Count - 1] = "\"" + lstr[lstr.Count - 1] + "\"";
					break;
			}
			return lstr.ToArray();;
		}
	}
	
	public class GettextBlockList : List<GettextBlock>
	{
		public int IndexOf(string id)
		{
			for (int i = 0; i < this.Count; i ++)
				if (this[i].ID == id)
					return i;
			return -1;
		}
	}
}