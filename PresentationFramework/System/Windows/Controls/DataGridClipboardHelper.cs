﻿using System;
using System.Globalization;
using System.IO;
using System.Text;
using MS.Internal.Text;

namespace System.Windows.Controls
{
	// Token: 0x0200074A RID: 1866
	internal static class DataGridClipboardHelper
	{
		// Token: 0x060064FB RID: 25851 RVA: 0x002AB798 File Offset: 0x002AA798
		internal static void FormatCell(object cellValue, bool firstCell, bool lastCell, StringBuilder sb, string format)
		{
			bool flag = string.Equals(format, DataFormats.CommaSeparatedValue, StringComparison.OrdinalIgnoreCase);
			if (!flag && !string.Equals(format, DataFormats.Text, StringComparison.OrdinalIgnoreCase) && !string.Equals(format, DataFormats.UnicodeText, StringComparison.OrdinalIgnoreCase))
			{
				if (string.Equals(format, DataFormats.Html, StringComparison.OrdinalIgnoreCase))
				{
					if (firstCell)
					{
						sb.Append("<TR>");
					}
					sb.Append("<TD>");
					if (cellValue != null)
					{
						DataGridClipboardHelper.FormatPlainTextAsHtml(cellValue.ToString(), new StringWriter(sb, CultureInfo.CurrentCulture));
					}
					else
					{
						sb.Append("&nbsp;");
					}
					sb.Append("</TD>");
					if (lastCell)
					{
						sb.Append("</TR>");
					}
				}
				return;
			}
			if (cellValue != null)
			{
				bool flag2 = false;
				int length = sb.Length;
				DataGridClipboardHelper.FormatPlainText(cellValue.ToString(), flag, new StringWriter(sb, CultureInfo.CurrentCulture), ref flag2);
				if (flag2)
				{
					sb.Insert(length, '"');
				}
			}
			if (lastCell)
			{
				sb.Append('\r');
				sb.Append('\n');
				return;
			}
			sb.Append(flag ? ',' : '\t');
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x002AB89C File Offset: 0x002AA89C
		internal static void GetClipboardContentForHtml(StringBuilder content)
		{
			content.Insert(0, "<TABLE>");
			content.Append("</TABLE>");
			byte[] bytes = Encoding.Unicode.GetBytes(content.ToString());
			byte[] array = InternalEncoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);
			int num = 135 + array.Length;
			int num2 = num + 36;
			string value = string.Format(CultureInfo.InvariantCulture, "Version:1.0\r\nStartHTML:00000097\r\nEndHTML:{0}\r\nStartFragment:00000133\r\nEndFragment:{1}\r\n", num2.ToString("00000000", CultureInfo.InvariantCulture), num.ToString("00000000", CultureInfo.InvariantCulture)) + "<HTML>\r\n<BODY>\r\n<!--StartFragment-->";
			content.Insert(0, value);
			content.Append("\r\n<!--EndFragment-->\r\n</BODY>\r\n</HTML>");
		}

		// Token: 0x060064FD RID: 25853 RVA: 0x002AB948 File Offset: 0x002AA948
		private static void FormatPlainText(string s, bool csv, TextWriter output, ref bool escapeApplied)
		{
			if (s != null)
			{
				int length = s.Length;
				for (int i = 0; i < length; i++)
				{
					char c = s[i];
					if (c != '\t')
					{
						if (c != '"')
						{
							if (c != ',')
							{
								output.Write(c);
							}
							else
							{
								if (csv)
								{
									escapeApplied = true;
								}
								output.Write(',');
							}
						}
						else if (csv)
						{
							output.Write("\"\"");
							escapeApplied = true;
						}
						else
						{
							output.Write('"');
						}
					}
					else if (!csv)
					{
						output.Write(' ');
					}
					else
					{
						output.Write('\t');
					}
				}
				if (escapeApplied)
				{
					output.Write('"');
				}
			}
		}

		// Token: 0x060064FE RID: 25854 RVA: 0x002AB9E0 File Offset: 0x002AA9E0
		private static void FormatPlainTextAsHtml(string s, TextWriter output)
		{
			if (s == null)
			{
				return;
			}
			int length = s.Length;
			char c = '\0';
			int i = 0;
			while (i < length)
			{
				char c2 = s[i];
				if (c2 <= ' ')
				{
					if (c2 != '\n')
					{
						if (c2 != '\r')
						{
							if (c2 != ' ')
							{
								goto IL_B7;
							}
							if (c == ' ')
							{
								output.Write("&nbsp;");
							}
							else
							{
								output.Write(c2);
							}
						}
					}
					else
					{
						output.Write("<br>");
					}
				}
				else if (c2 <= '&')
				{
					if (c2 != '"')
					{
						if (c2 != '&')
						{
							goto IL_B7;
						}
						output.Write("&amp;");
					}
					else
					{
						output.Write("&quot;");
					}
				}
				else if (c2 != '<')
				{
					if (c2 != '>')
					{
						goto IL_B7;
					}
					output.Write("&gt;");
				}
				else
				{
					output.Write("&lt;");
				}
				IL_F8:
				c = c2;
				i++;
				continue;
				IL_B7:
				if (c2 >= '\u00a0' && c2 < 'Ā')
				{
					output.Write("&#");
					int num = (int)c2;
					output.Write(num.ToString(NumberFormatInfo.InvariantInfo));
					output.Write(';');
					goto IL_F8;
				}
				output.Write(c2);
				goto IL_F8;
			}
		}

		// Token: 0x04003364 RID: 13156
		private const string DATAGRIDVIEW_htmlPrefix = "Version:1.0\r\nStartHTML:00000097\r\nEndHTML:{0}\r\nStartFragment:00000133\r\nEndFragment:{1}\r\n";

		// Token: 0x04003365 RID: 13157
		private const string DATAGRIDVIEW_htmlStartFragment = "<HTML>\r\n<BODY>\r\n<!--StartFragment-->";

		// Token: 0x04003366 RID: 13158
		private const string DATAGRIDVIEW_htmlEndFragment = "\r\n<!--EndFragment-->\r\n</BODY>\r\n</HTML>";
	}
}
