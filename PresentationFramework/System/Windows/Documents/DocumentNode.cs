using System;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x0200067C RID: 1660
	internal class DocumentNode
	{
		// Token: 0x060051B5 RID: 20917 RVA: 0x0024FFFC File Offset: 0x0024EFFC
		internal DocumentNode(DocumentNodeType documentNodeType)
		{
			this._type = documentNodeType;
			this._bPending = true;
			this._childCount = 0;
			this._index = -1;
			this._dna = null;
			this._parent = null;
			this._bTerminated = false;
			this._bMatched = false;
			this._bHasMarkerContent = false;
			this._sCustom = null;
			this._nRowSpan = 1;
			this._nColSpan = 1;
			this._nVirtualListLevel = -1L;
			this._csa = null;
			this._formatState = new FormatState();
			this._contentBuilder = new StringBuilder();
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x00250088 File Offset: 0x0024F088
		internal void InheritFormatState(FormatState formatState)
		{
			this._formatState = new FormatState(formatState);
			this._formatState.LI = 0L;
			this._formatState.RI = 0L;
			this._formatState.SB = 0L;
			this._formatState.SA = 0L;
			this._formatState.FI = 0L;
			this._formatState.Marker = MarkerStyle.MarkerNone;
			this._formatState.CBPara = -1L;
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x002500FB File Offset: 0x0024F0FB
		internal string GetTagName()
		{
			return DocumentNode.XamlNames[(int)this.Type];
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x0025010C File Offset: 0x0024F10C
		internal DocumentNode GetParentOfType(DocumentNodeType parentType)
		{
			DocumentNode parent = this.Parent;
			while (parent != null && parent.Type != parentType)
			{
				parent = parent.Parent;
			}
			return parent;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x00250138 File Offset: 0x0024F138
		internal int GetTableDepth()
		{
			DocumentNode parent = this.Parent;
			int num = 0;
			while (parent != null)
			{
				if (parent.Type == DocumentNodeType.dnTable)
				{
					num++;
				}
				parent = parent.Parent;
			}
			return num;
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x0025016C File Offset: 0x0024F16C
		internal int GetListDepth()
		{
			DocumentNode parent = this.Parent;
			int num = 0;
			while (parent != null)
			{
				if (parent.Type == DocumentNodeType.dnList)
				{
					num++;
				}
				else if (parent.Type == DocumentNodeType.dnCell)
				{
					break;
				}
				parent = parent.Parent;
			}
			return num;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x002501AC File Offset: 0x0024F1AC
		internal void Terminate(ConverterState converterState)
		{
			if (!this.IsTerminated)
			{
				string value = this.StripInvalidChars(this.Xaml);
				this.AppendXamlPrefix(converterState);
				StringBuilder stringBuilder = new StringBuilder(this.Xaml);
				stringBuilder.Append(value);
				this.Xaml = stringBuilder.ToString();
				this.AppendXamlPostfix(converterState);
				this.IsTerminated = true;
			}
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x00250204 File Offset: 0x0024F204
		internal void ConstrainFontPropagation(FormatState fsOrig)
		{
			this.FormatState.SetCharDefaults();
			this.FormatState.Font = fsOrig.Font;
			this.FormatState.FontSize = fsOrig.FontSize;
			this.FormatState.Bold = fsOrig.Bold;
			this.FormatState.Italic = fsOrig.Italic;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x00250260 File Offset: 0x0024F260
		internal bool RequiresXamlFontProperties()
		{
			FormatState formatState = this.FormatState;
			FormatState parentFormatStateForFont = this.ParentFormatStateForFont;
			return formatState.Strike != parentFormatStateForFont.Strike || formatState.UL != parentFormatStateForFont.UL || (formatState.Font != parentFormatStateForFont.Font && formatState.Font >= 0L) || (formatState.FontSize != parentFormatStateForFont.FontSize && formatState.FontSize >= 0L) || formatState.CF != parentFormatStateForFont.CF || formatState.Bold != parentFormatStateForFont.Bold || formatState.Italic != parentFormatStateForFont.Italic || formatState.LangCur != parentFormatStateForFont.LangCur;
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x00250304 File Offset: 0x0024F304
		internal void AppendXamlFontProperties(ConverterState converterState, StringBuilder sb)
		{
			FormatState formatState = this.FormatState;
			FormatState parentFormatStateForFont = this.ParentFormatStateForFont;
			bool flag = formatState.Strike != parentFormatStateForFont.Strike;
			bool flag2 = formatState.UL != parentFormatStateForFont.UL;
			if (flag || flag2)
			{
				sb.Append(" TextDecorations=\"");
				if (flag2)
				{
					sb.Append("Underline");
				}
				if (flag2 && flag)
				{
					sb.Append(", ");
				}
				if (flag)
				{
					sb.Append("Strikethrough");
				}
				sb.Append("\"");
			}
			if (formatState.Font != parentFormatStateForFont.Font && formatState.Font >= 0L)
			{
				FontTableEntry fontTableEntry = converterState.FontTable.FindEntryByIndex((int)formatState.Font);
				if (fontTableEntry != null && fontTableEntry.Name != null && !fontTableEntry.Name.Equals(string.Empty))
				{
					sb.Append(" FontFamily=\"");
					if (fontTableEntry.Name.Length > 32)
					{
						sb.Append(fontTableEntry.Name, 0, 32);
					}
					else
					{
						sb.Append(fontTableEntry.Name);
					}
					sb.Append("\"");
				}
			}
			if (formatState.FontSize != parentFormatStateForFont.FontSize && formatState.FontSize >= 0L)
			{
				sb.Append(" FontSize=\"");
				double num = (double)formatState.FontSize;
				if (num <= 1.0)
				{
					num = 2.0;
				}
				sb.Append((num / 2.0).ToString(CultureInfo.InvariantCulture));
				sb.Append("pt\"");
			}
			if (formatState.Bold != parentFormatStateForFont.Bold)
			{
				if (formatState.Bold)
				{
					sb.Append(" FontWeight=\"Bold\"");
				}
				else
				{
					sb.Append(" FontWeight=\"Normal\"");
				}
			}
			if (formatState.Italic != parentFormatStateForFont.Italic)
			{
				if (formatState.Italic)
				{
					sb.Append(" FontStyle=\"Italic\"");
				}
				else
				{
					sb.Append(" FontStyle=\"Normal\"");
				}
			}
			if (formatState.CF != parentFormatStateForFont.CF)
			{
				ColorTableEntry colorTableEntry = converterState.ColorTable.EntryAt((int)formatState.CF);
				if (colorTableEntry != null && !colorTableEntry.IsAuto)
				{
					sb.Append(" Foreground=\"");
					sb.Append(colorTableEntry.Color.ToString());
					sb.Append("\"");
				}
			}
			if (formatState.LangCur != parentFormatStateForFont.LangCur && formatState.LangCur > 0L && formatState.LangCur != 1024L)
			{
				try
				{
					CultureInfo cultureInfo = new CultureInfo((int)formatState.LangCur);
					sb.Append(" xml:lang=\"");
					sb.Append(cultureInfo.Name);
					sb.Append("\"");
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x002505CC File Offset: 0x0024F5CC
		internal string StripInvalidChars(string text)
		{
			if (text == null || text.Length == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = null;
			for (int i = 0; i < text.Length; i++)
			{
				int num = i;
				while (i < text.Length)
				{
					if ((text[i] & '') == '\ud800')
					{
						if (i + 1 == text.Length || (text[i] & 'ﰀ') == '\udc00' || (text[i + 1] & 'ﰀ') != '\udc00')
						{
							break;
						}
						i++;
					}
					i++;
				}
				if (num != 0 || i != text.Length)
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder();
					}
					if (i != num)
					{
						stringBuilder.Append(text, num, i - num);
					}
				}
			}
			if (stringBuilder != null)
			{
				return stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x00250690 File Offset: 0x0024F690
		internal void AppendXamlEncoded(string text)
		{
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			int num;
			for (int i = 0; i < text.Length; i = num + 1)
			{
				num = i;
				while (num < text.Length && (text[num] >= ' ' || text[num] == '\t') && text[num] != '&' && text[num] != '>' && text[num] != '<' && text[num] != '\0')
				{
					num++;
				}
				if (num != i)
				{
					string value = text.Substring(i, num - i);
					stringBuilder.Append(value);
				}
				if (num < text.Length)
				{
					if (text[num] < ' ' && text[num] != '\t')
					{
						if (text[num] == '\f')
						{
							stringBuilder.Append("&#x");
							stringBuilder.Append(((int)text[num]).ToString("x", CultureInfo.InvariantCulture));
							stringBuilder.Append(";");
						}
					}
					else
					{
						char c = text[num];
						if (c <= '&')
						{
							if (c != '\0')
							{
								if (c == '&')
								{
									stringBuilder.Append("&amp;");
								}
							}
						}
						else if (c != '<')
						{
							if (c == '>')
							{
								stringBuilder.Append("&gt;");
							}
						}
						else
						{
							stringBuilder.Append("&lt;");
						}
					}
				}
			}
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x002507F0 File Offset: 0x0024F7F0
		internal void AppendXamlPrefix(ConverterState converterState)
		{
			DocumentNodeArray documentNodeArray = converterState.DocumentNodeArray;
			if (this.IsHidden)
			{
				return;
			}
			if (this.Type == DocumentNodeType.dnImage)
			{
				this.AppendImageXamlPrefix();
				return;
			}
			if (this.Type == DocumentNodeType.dnText || this.Type == DocumentNodeType.dnInline)
			{
				this.AppendInlineXamlPrefix(converterState);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsEmptyNode && this.RequiresXamlFontProperties())
			{
				stringBuilder.Append("<");
				stringBuilder.Append(DocumentNode.XamlNames[2]);
				this.AppendXamlFontProperties(converterState, stringBuilder);
				stringBuilder.Append(">");
			}
			stringBuilder.Append("<");
			stringBuilder.Append(this.GetTagName());
			switch (this.Type)
			{
			case DocumentNodeType.dnHyperlink:
				this.AppendXamlPrefixHyperlinkProperties(stringBuilder);
				break;
			case DocumentNodeType.dnParagraph:
				this.AppendXamlPrefixParagraphProperties(stringBuilder, converterState);
				break;
			case DocumentNodeType.dnList:
				this.AppendXamlPrefixListProperties(stringBuilder);
				break;
			case DocumentNodeType.dnListItem:
				this.AppendXamlPrefixListItemProperties(stringBuilder);
				break;
			case DocumentNodeType.dnTable:
				this.AppendXamlPrefixTableProperties(stringBuilder);
				break;
			case DocumentNodeType.dnCell:
				this.AppendXamlPrefixCellProperties(stringBuilder, documentNodeArray, converterState);
				break;
			}
			if (this.IsEmptyNode)
			{
				stringBuilder.Append(" /");
			}
			stringBuilder.Append(">");
			if (this.IsEmptyNode && this.RequiresXamlFontProperties())
			{
				stringBuilder.Append("</");
				stringBuilder.Append(DocumentNode.XamlNames[2]);
				stringBuilder.Append(">");
			}
			if (this.Type == DocumentNodeType.dnTable)
			{
				this.AppendXamlTableColumnsAfterStartTag(stringBuilder);
			}
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x0025097C File Offset: 0x0024F97C
		private void AppendXamlPrefixTableProperties(StringBuilder xamlStringBuilder)
		{
			if (this.FormatState.HasRowFormat)
			{
				if (this.FormatState.RowFormat.Dir == DirState.DirRTL)
				{
					xamlStringBuilder.Append(" FlowDirection=\"RightToLeft\"");
				}
				RowFormat rowFormat = this.FormatState.RowFormat;
				CellFormat rowCellFormat = rowFormat.RowCellFormat;
				xamlStringBuilder.Append(" CellSpacing=\"");
				xamlStringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)rowCellFormat.SpacingLeft));
				xamlStringBuilder.Append("\"");
				xamlStringBuilder.Append(" Margin=\"");
				xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)rowFormat.Trleft));
				xamlStringBuilder.Append(",0,0,0\"");
				return;
			}
			xamlStringBuilder.Append(" CellSpacing=\"0\" Margin=\"0,0,0,0\"");
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x00250A30 File Offset: 0x0024FA30
		private void AppendXamlPrefixCellProperties(StringBuilder xamlStringBuilder, DocumentNodeArray dna, ConverterState converterState)
		{
			Color color = Color.FromArgb(byte.MaxValue, 0, 0, 0);
			DocumentNode parentOfType = this.GetParentOfType(DocumentNodeType.dnRow);
			if (parentOfType != null && parentOfType.FormatState.HasRowFormat)
			{
				int cellColumn = this.GetCellColumn();
				CellFormat cellFormat = parentOfType.FormatState.RowFormat.NthCellFormat(cellColumn);
				if (Converters.ColorToUse(converterState, cellFormat.CB, cellFormat.CF, cellFormat.Shading, ref color))
				{
					xamlStringBuilder.Append(" Background=\"");
					xamlStringBuilder.Append(color.ToString(CultureInfo.InvariantCulture));
					xamlStringBuilder.Append("\"");
				}
				if (cellFormat.HasBorder)
				{
					xamlStringBuilder.Append(cellFormat.GetBorderAttributeString(converterState));
				}
				xamlStringBuilder.Append(cellFormat.GetPaddingAttributeString());
			}
			else
			{
				xamlStringBuilder.Append(" BorderBrush=\"#FF000000\" BorderThickness=\"1,1,1,1\"");
			}
			if (this.ColSpan > 1)
			{
				xamlStringBuilder.Append(" ColumnSpan=\"");
				xamlStringBuilder.Append(this.ColSpan.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
			if (this.RowSpan > 1)
			{
				xamlStringBuilder.Append(" RowSpan=\"");
				xamlStringBuilder.Append(this.RowSpan.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x00250B73 File Offset: 0x0024FB73
		private void AppendXamlDir(StringBuilder xamlStringBuilder)
		{
			if (this.RequiresXamlDir)
			{
				if (this.XamlDir == DirState.DirLTR)
				{
					xamlStringBuilder.Append(" FlowDirection=\"LeftToRight\"");
					return;
				}
				xamlStringBuilder.Append(" FlowDirection=\"RightToLeft\"");
			}
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x00250BA0 File Offset: 0x0024FBA0
		private void AppendXamlPrefixParagraphProperties(StringBuilder xamlStringBuilder, ConverterState converterState)
		{
			Color color = Color.FromArgb(byte.MaxValue, 0, 0, 0);
			FormatState formatState = this.FormatState;
			if (Converters.ColorToUse(converterState, formatState.CBPara, formatState.CFPara, formatState.ParaShading, ref color))
			{
				xamlStringBuilder.Append(" Background=\"");
				xamlStringBuilder.Append(color.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
			this.AppendXamlDir(xamlStringBuilder);
			xamlStringBuilder.Append(" Margin=\"");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)this.NearMargin));
			xamlStringBuilder.Append(",");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)formatState.SB));
			xamlStringBuilder.Append(",");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)this.FarMargin));
			xamlStringBuilder.Append(",");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)formatState.SA));
			xamlStringBuilder.Append("\"");
			this.AppendXamlFontProperties(converterState, xamlStringBuilder);
			if (formatState.FI != 0L)
			{
				xamlStringBuilder.Append(" TextIndent=\"");
				xamlStringBuilder.Append(Converters.TwipToPxString((double)formatState.FI));
				xamlStringBuilder.Append("\"");
			}
			if (formatState.HAlign != HAlign.AlignDefault)
			{
				xamlStringBuilder.Append(" TextAlignment=\"");
				xamlStringBuilder.Append(Converters.AlignmentToString(formatState.HAlign, formatState.DirPara));
				xamlStringBuilder.Append("\"");
			}
			if (formatState.HasParaBorder)
			{
				xamlStringBuilder.Append(formatState.GetBorderAttributeString(converterState));
			}
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x00250D24 File Offset: 0x0024FD24
		private void AppendXamlPrefixListItemProperties(StringBuilder xamlStringBuilder)
		{
			long num = this.NearMargin;
			if (num < 360L && this.GetListDepth() == 1)
			{
				DocumentNode parent = this.Parent;
				if (parent != null && parent.FormatState.Marker != MarkerStyle.MarkerHidden)
				{
					num = 360L;
				}
			}
			xamlStringBuilder.Append(" Margin=\"");
			xamlStringBuilder.Append(Converters.TwipToPositivePxString((double)num));
			xamlStringBuilder.Append(",0,0,0\"");
			this.AppendXamlDir(xamlStringBuilder);
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x00250D9C File Offset: 0x0024FD9C
		private void AppendXamlPrefixListProperties(StringBuilder xamlStringBuilder)
		{
			xamlStringBuilder.Append(" Margin=\"0,0,0,0\"");
			xamlStringBuilder.Append(" Padding=\"0,0,0,0\"");
			xamlStringBuilder.Append(" MarkerStyle=\"");
			xamlStringBuilder.Append(Converters.MarkerStyleToString(this.FormatState.Marker));
			xamlStringBuilder.Append("\"");
			if (this.FormatState.StartIndex > 0L && this.FormatState.StartIndex != 1L)
			{
				xamlStringBuilder.Append(" StartIndex=\"");
				xamlStringBuilder.Append(this.FormatState.StartIndex.ToString(CultureInfo.InvariantCulture));
				xamlStringBuilder.Append("\"");
			}
			this.AppendXamlDir(xamlStringBuilder);
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x00250E4C File Offset: 0x0024FE4C
		private void AppendXamlPrefixHyperlinkProperties(StringBuilder xamlStringBuilder)
		{
			if (this.NavigateUri != null && this.NavigateUri.Length > 0)
			{
				xamlStringBuilder.Append(" NavigateUri=\"");
				xamlStringBuilder.Append(Converters.StringToXMLAttribute(this.NavigateUri));
				xamlStringBuilder.Append("\"");
			}
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x00250E9C File Offset: 0x0024FE9C
		private void AppendXamlTableColumnsAfterStartTag(StringBuilder xamlStringBuilder)
		{
			if (this.ColumnStateArray != null && this.ColumnStateArray.Count > 0)
			{
				xamlStringBuilder.Append("<Table.Columns>");
				long num = 0L;
				if (this.FormatState.HasRowFormat)
				{
					num = this.FormatState.RowFormat.Trleft;
				}
				for (int i = 0; i < this.ColumnStateArray.Count; i++)
				{
					ColumnState columnState = this.ColumnStateArray.EntryAt(i);
					long num2 = columnState.CellX - num;
					if (num2 <= 0L)
					{
						num2 = 1L;
					}
					num = columnState.CellX;
					xamlStringBuilder.Append("<TableColumn Width=\"");
					xamlStringBuilder.Append(Converters.TwipToPxString((double)num2));
					xamlStringBuilder.Append("\" />");
				}
				xamlStringBuilder.Append("</Table.Columns>");
			}
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x00250F60 File Offset: 0x0024FF60
		internal void AppendXamlPostfix(ConverterState converterState)
		{
			if (this.IsHidden)
			{
				return;
			}
			if (this.IsEmptyNode)
			{
				return;
			}
			if (this.Type == DocumentNodeType.dnImage)
			{
				this.AppendImageXamlPostfix();
				return;
			}
			if (this.Type == DocumentNodeType.dnText || this.Type == DocumentNodeType.dnInline)
			{
				this.AppendInlineXamlPostfix(converterState);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			stringBuilder.Append("</");
			stringBuilder.Append(this.GetTagName());
			stringBuilder.Append(">");
			if (this.IsBlock)
			{
				stringBuilder.Append("\r\n");
			}
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x00250FFC File Offset: 0x0024FFFC
		internal void AppendInlineXamlPrefix(ConverterState converterState)
		{
			StringBuilder stringBuilder = new StringBuilder();
			FormatState formatState = this.FormatState;
			FormatState parentFormatStateForFont = this.ParentFormatStateForFont;
			stringBuilder.Append("<Span");
			this.AppendXamlDir(stringBuilder);
			if (formatState.CB != parentFormatStateForFont.CB)
			{
				ColorTableEntry colorTableEntry = converterState.ColorTable.EntryAt((int)formatState.CB);
				if (colorTableEntry != null && !colorTableEntry.IsAuto)
				{
					stringBuilder.Append(" Background=\"");
					stringBuilder.Append(colorTableEntry.Color.ToString());
					stringBuilder.Append("\"");
				}
			}
			this.AppendXamlFontProperties(converterState, stringBuilder);
			if (formatState.Super != parentFormatStateForFont.Super)
			{
				stringBuilder.Append(" Typography.Variants=\"Superscript\"");
			}
			if (formatState.Sub != parentFormatStateForFont.Sub)
			{
				stringBuilder.Append(" Typography.Variants=\"Subscript\"");
			}
			stringBuilder.Append(">");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x002510E4 File Offset: 0x002500E4
		internal void AppendInlineXamlPostfix(ConverterState converterState)
		{
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			stringBuilder.Append("</Span>");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x00251118 File Offset: 0x00250118
		internal void AppendImageXamlPrefix()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<InlineUIContainer>");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x00251144 File Offset: 0x00250144
		internal void AppendImageXamlPostfix()
		{
			StringBuilder stringBuilder = new StringBuilder(this.Xaml);
			stringBuilder.Append("</InlineUIContainer>");
			this.Xaml = stringBuilder.ToString();
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x00251178 File Offset: 0x00250178
		internal bool IsAncestorOf(DocumentNode documentNode)
		{
			int index = this.Index;
			int num = this.Index + this.ChildCount;
			return documentNode.Index > index && documentNode.Index <= num;
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x002511B4 File Offset: 0x002501B4
		internal bool IsLastParagraphInCell()
		{
			DocumentNodeArray dna = this.DNA;
			if (this.Type != DocumentNodeType.dnParagraph)
			{
				return false;
			}
			DocumentNode parentOfType = this.GetParentOfType(DocumentNodeType.dnCell);
			if (parentOfType == null)
			{
				return false;
			}
			int i = parentOfType.Index + 1;
			int num = parentOfType.Index + parentOfType.ChildCount;
			while (i <= num)
			{
				DocumentNode documentNode = dna.EntryAt(num);
				if (documentNode == this)
				{
					return true;
				}
				if (documentNode.IsBlock)
				{
					return false;
				}
				num--;
			}
			return false;
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x00251220 File Offset: 0x00250220
		internal DocumentNodeArray GetTableRows()
		{
			DocumentNodeArray dna = this.DNA;
			DocumentNodeArray documentNodeArray = new DocumentNodeArray();
			if (this.Type == DocumentNodeType.dnTable)
			{
				int i = this.Index + 1;
				int num = this.Index + this.ChildCount;
				while (i <= num)
				{
					DocumentNode documentNode = dna.EntryAt(i);
					if (documentNode.Type == DocumentNodeType.dnRow && this == documentNode.GetParentOfType(DocumentNodeType.dnTable))
					{
						documentNodeArray.Push(documentNode);
					}
					i++;
				}
			}
			return documentNodeArray;
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x00251290 File Offset: 0x00250290
		internal DocumentNodeArray GetRowsCells()
		{
			DocumentNodeArray dna = this.DNA;
			DocumentNodeArray documentNodeArray = new DocumentNodeArray();
			if (this.Type == DocumentNodeType.dnRow)
			{
				int i = this.Index + 1;
				int num = this.Index + this.ChildCount;
				while (i <= num)
				{
					DocumentNode documentNode = dna.EntryAt(i);
					if (documentNode.Type == DocumentNodeType.dnCell && this == documentNode.GetParentOfType(DocumentNodeType.dnRow))
					{
						documentNodeArray.Push(documentNode);
					}
					i++;
				}
			}
			return documentNodeArray;
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x00251300 File Offset: 0x00250300
		internal int GetCellColumn()
		{
			DocumentNodeArray dna = this.DNA;
			int num = 0;
			if (this.Type == DocumentNodeType.dnCell)
			{
				DocumentNode parentOfType = this.GetParentOfType(DocumentNodeType.dnRow);
				if (parentOfType != null)
				{
					int i = parentOfType.Index + 1;
					int num2 = parentOfType.Index + parentOfType.ChildCount;
					while (i <= num2)
					{
						DocumentNode documentNode = dna.EntryAt(i);
						if (documentNode == this)
						{
							break;
						}
						if (documentNode.Type == DocumentNodeType.dnCell && documentNode.GetParentOfType(DocumentNodeType.dnRow) == parentOfType)
						{
							num++;
						}
						i++;
					}
				}
			}
			return num;
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x0025137C File Offset: 0x0025037C
		internal ColumnStateArray ComputeColumns()
		{
			DocumentNodeArray dna = this.DNA;
			DocumentNodeArray tableRows = this.GetTableRows();
			ColumnStateArray columnStateArray = new ColumnStateArray();
			for (int i = 0; i < tableRows.Count; i++)
			{
				DocumentNode documentNode = tableRows.EntryAt(i);
				RowFormat rowFormat = documentNode.FormatState.RowFormat;
				long num = 0L;
				for (int j = 0; j < rowFormat.CellCount; j++)
				{
					CellFormat cellFormat = rowFormat.NthCellFormat(j);
					bool flag = false;
					long num2 = 0L;
					if (!cellFormat.IsHMerge)
					{
						for (int k = 0; k < columnStateArray.Count; k++)
						{
							ColumnState columnState = (ColumnState)columnStateArray[k];
							if (columnState.CellX == cellFormat.CellX)
							{
								if (!columnState.IsFilled && num2 == num)
								{
									columnState.IsFilled = true;
								}
								flag = true;
								break;
							}
							if (columnState.CellX > cellFormat.CellX)
							{
								columnStateArray.Insert(k, new ColumnState
								{
									Row = documentNode,
									CellX = cellFormat.CellX,
									IsFilled = (num2 == num)
								});
								flag = true;
								break;
							}
							num2 = columnState.CellX;
						}
						if (!flag)
						{
							columnStateArray.Add(new ColumnState
							{
								Row = documentNode,
								CellX = cellFormat.CellX,
								IsFilled = (num2 == num)
							});
						}
						num = cellFormat.CellX;
					}
				}
			}
			return columnStateArray;
		}

		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x060051D5 RID: 20949 RVA: 0x002514F0 File Offset: 0x002504F0
		internal bool IsInline
		{
			get
			{
				return this._type == DocumentNodeType.dnText || this._type == DocumentNodeType.dnInline || this._type == DocumentNodeType.dnImage || this._type == DocumentNodeType.dnLineBreak || this._type == DocumentNodeType.dnListText || this._type == DocumentNodeType.dnHyperlink;
			}
		}

		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x060051D6 RID: 20950 RVA: 0x0025152C File Offset: 0x0025052C
		internal bool IsBlock
		{
			get
			{
				return this._type == DocumentNodeType.dnParagraph || this._type == DocumentNodeType.dnList || this._type == DocumentNodeType.dnListItem || this._type == DocumentNodeType.dnTable || this._type == DocumentNodeType.dnTableBody || this._type == DocumentNodeType.dnRow || this._type == DocumentNodeType.dnCell || this._type == DocumentNodeType.dnSection || this._type == DocumentNodeType.dnFigure || this._type == DocumentNodeType.dnFloater;
			}
		}

		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x0025159E File Offset: 0x0025059E
		internal bool IsEmptyNode
		{
			get
			{
				return this._type == DocumentNodeType.dnLineBreak;
			}
		}

		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x002515A9 File Offset: 0x002505A9
		internal bool IsHidden
		{
			get
			{
				return this._type == DocumentNodeType.dnFieldBegin || this._type == DocumentNodeType.dnFieldEnd || this._type == DocumentNodeType.dnShape || this._type == DocumentNodeType.dnListText;
			}
		}

		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x002515D5 File Offset: 0x002505D5
		internal bool IsWhiteSpace
		{
			get
			{
				return !this.IsTerminated && this._type == DocumentNodeType.dnText && this.Xaml.Trim().Length == 0;
			}
		}

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x002515FF File Offset: 0x002505FF
		// (set) Token: 0x060051DB RID: 20955 RVA: 0x00251612 File Offset: 0x00250612
		internal bool IsPending
		{
			get
			{
				return this.Index >= 0 && this._bPending;
			}
			set
			{
				this._bPending = value;
			}
		}

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x060051DC RID: 20956 RVA: 0x0025161B File Offset: 0x0025061B
		// (set) Token: 0x060051DD RID: 20957 RVA: 0x00251623 File Offset: 0x00250623
		internal bool IsTerminated
		{
			get
			{
				return this._bTerminated;
			}
			set
			{
				this._bTerminated = value;
			}
		}

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x060051DE RID: 20958 RVA: 0x0025162C File Offset: 0x0025062C
		// (set) Token: 0x060051DF RID: 20959 RVA: 0x00251640 File Offset: 0x00250640
		internal bool IsMatched
		{
			get
			{
				return this.Type != DocumentNodeType.dnFieldBegin || this._bMatched;
			}
			set
			{
				this._bMatched = value;
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x060051E0 RID: 20960 RVA: 0x00251649 File Offset: 0x00250649
		internal bool IsTrackedAsOpen
		{
			get
			{
				return this.Index >= 0 && this.Type != DocumentNodeType.dnFieldEnd && ((this.IsPending && !this.IsTerminated) || !this.IsMatched);
			}
		}

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x060051E1 RID: 20961 RVA: 0x0025167F File Offset: 0x0025067F
		// (set) Token: 0x060051E2 RID: 20962 RVA: 0x00251687 File Offset: 0x00250687
		internal bool HasMarkerContent
		{
			get
			{
				return this._bHasMarkerContent;
			}
			set
			{
				this._bHasMarkerContent = value;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x00251690 File Offset: 0x00250690
		internal bool IsNonEmpty
		{
			get
			{
				return this.ChildCount > 0 || this.Xaml != null;
			}
		}

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x060051E4 RID: 20964 RVA: 0x002516A6 File Offset: 0x002506A6
		// (set) Token: 0x060051E5 RID: 20965 RVA: 0x002516AE File Offset: 0x002506AE
		internal string ListLabel
		{
			get
			{
				return this._sCustom;
			}
			set
			{
				this._sCustom = value;
			}
		}

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x060051E6 RID: 20966 RVA: 0x002516B7 File Offset: 0x002506B7
		// (set) Token: 0x060051E7 RID: 20967 RVA: 0x002516BF File Offset: 0x002506BF
		internal long VirtualListLevel
		{
			get
			{
				return this._nVirtualListLevel;
			}
			set
			{
				this._nVirtualListLevel = value;
			}
		}

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x060051E8 RID: 20968 RVA: 0x002516A6 File Offset: 0x002506A6
		// (set) Token: 0x060051E9 RID: 20969 RVA: 0x002516AE File Offset: 0x002506AE
		internal string NavigateUri
		{
			get
			{
				return this._sCustom;
			}
			set
			{
				this._sCustom = value;
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x060051EA RID: 20970 RVA: 0x002516C8 File Offset: 0x002506C8
		internal DocumentNodeType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x060051EB RID: 20971 RVA: 0x002516D0 File Offset: 0x002506D0
		// (set) Token: 0x060051EC RID: 20972 RVA: 0x002516D8 File Offset: 0x002506D8
		internal FormatState FormatState
		{
			get
			{
				return this._formatState;
			}
			set
			{
				this._formatState = value;
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x060051ED RID: 20973 RVA: 0x002516E4 File Offset: 0x002506E4
		internal FormatState ParentFormatStateForFont
		{
			get
			{
				DocumentNode parent = this.Parent;
				if (parent != null && parent.Type == DocumentNodeType.dnHyperlink)
				{
					parent = parent.Parent;
				}
				if (this.Type == DocumentNodeType.dnParagraph || parent == null)
				{
					return FormatState.EmptyFormatState;
				}
				return parent.FormatState;
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x060051EE RID: 20974 RVA: 0x00251723 File Offset: 0x00250723
		// (set) Token: 0x060051EF RID: 20975 RVA: 0x0025172B File Offset: 0x0025072B
		internal int ChildCount
		{
			get
			{
				return this._childCount;
			}
			set
			{
				if (value >= 0)
				{
					this._childCount = value;
				}
			}
		}

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x060051F0 RID: 20976 RVA: 0x00251738 File Offset: 0x00250738
		// (set) Token: 0x060051F1 RID: 20977 RVA: 0x00251740 File Offset: 0x00250740
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x060051F2 RID: 20978 RVA: 0x00251749 File Offset: 0x00250749
		// (set) Token: 0x060051F3 RID: 20979 RVA: 0x00251751 File Offset: 0x00250751
		internal DocumentNodeArray DNA
		{
			get
			{
				return this._dna;
			}
			set
			{
				this._dna = value;
			}
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x060051F4 RID: 20980 RVA: 0x0025175A File Offset: 0x0025075A
		internal int LastChildIndex
		{
			get
			{
				return this.Index + this.ChildCount;
			}
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x060051F5 RID: 20981 RVA: 0x00251769 File Offset: 0x00250769
		internal DocumentNode ClosedParent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x060051F6 RID: 20982 RVA: 0x00251771 File Offset: 0x00250771
		// (set) Token: 0x060051F7 RID: 20983 RVA: 0x00251796 File Offset: 0x00250796
		internal DocumentNode Parent
		{
			get
			{
				if (this._parent == null && this.DNA != null)
				{
					return this.DNA.GetOpenParentWhileParsing(this);
				}
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x060051F8 RID: 20984 RVA: 0x0025179F File Offset: 0x0025079F
		// (set) Token: 0x060051F9 RID: 20985 RVA: 0x002517A7 File Offset: 0x002507A7
		internal string Xaml
		{
			get
			{
				return this._xaml;
			}
			set
			{
				this._xaml = value;
			}
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x060051FA RID: 20986 RVA: 0x002517B0 File Offset: 0x002507B0
		internal StringBuilder Content
		{
			get
			{
				return this._contentBuilder;
			}
		}

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x060051FB RID: 20987 RVA: 0x002517B8 File Offset: 0x002507B8
		// (set) Token: 0x060051FC RID: 20988 RVA: 0x002517C0 File Offset: 0x002507C0
		internal int RowSpan
		{
			get
			{
				return this._nRowSpan;
			}
			set
			{
				this._nRowSpan = value;
			}
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x060051FD RID: 20989 RVA: 0x002517C9 File Offset: 0x002507C9
		// (set) Token: 0x060051FE RID: 20990 RVA: 0x002517D1 File Offset: 0x002507D1
		internal int ColSpan
		{
			get
			{
				return this._nColSpan;
			}
			set
			{
				this._nColSpan = value;
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x060051FF RID: 20991 RVA: 0x002517DA File Offset: 0x002507DA
		// (set) Token: 0x06005200 RID: 20992 RVA: 0x002517E2 File Offset: 0x002507E2
		internal ColumnStateArray ColumnStateArray
		{
			get
			{
				return this._csa;
			}
			set
			{
				this._csa = value;
			}
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06005201 RID: 20993 RVA: 0x002517EC File Offset: 0x002507EC
		internal DirState XamlDir
		{
			get
			{
				if (this.IsInline)
				{
					return this.FormatState.DirChar;
				}
				if (this.Type == DocumentNodeType.dnTable)
				{
					if (this.FormatState.HasRowFormat)
					{
						return this.FormatState.RowFormat.Dir;
					}
					return this.ParentXamlDir;
				}
				else
				{
					if (this.Type == DocumentNodeType.dnList || this.Type == DocumentNodeType.dnParagraph)
					{
						return this.FormatState.DirPara;
					}
					for (DocumentNode parent = this.Parent; parent != null; parent = parent.Parent)
					{
						DocumentNodeType type = parent.Type;
						if (type == DocumentNodeType.dnParagraph || type == DocumentNodeType.dnList || type == DocumentNodeType.dnTable)
						{
							return parent.XamlDir;
						}
					}
					return DirState.DirLTR;
				}
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06005202 RID: 20994 RVA: 0x0025188B File Offset: 0x0025088B
		internal DirState ParentXamlDir
		{
			get
			{
				if (this.Parent != null)
				{
					return this.Parent.XamlDir;
				}
				return DirState.DirLTR;
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06005203 RID: 20995 RVA: 0x002518A2 File Offset: 0x002508A2
		internal bool RequiresXamlDir
		{
			get
			{
				return this.XamlDir != this.ParentXamlDir;
			}
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06005204 RID: 20996 RVA: 0x002518B5 File Offset: 0x002508B5
		// (set) Token: 0x06005205 RID: 20997 RVA: 0x002518D7 File Offset: 0x002508D7
		internal long NearMargin
		{
			get
			{
				if (this.ParentXamlDir != DirState.DirLTR)
				{
					return this.FormatState.RI;
				}
				return this.FormatState.LI;
			}
			set
			{
				if (this.ParentXamlDir == DirState.DirLTR)
				{
					this.FormatState.LI = value;
					return;
				}
				this.FormatState.RI = value;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06005206 RID: 20998 RVA: 0x002518FB File Offset: 0x002508FB
		internal long FarMargin
		{
			get
			{
				if (this.ParentXamlDir != DirState.DirLTR)
				{
					return this.FormatState.LI;
				}
				return this.FormatState.RI;
			}
		}

		// Token: 0x04002E7B RID: 11899
		internal static string[] HtmlNames = new string[]
		{
			"",
			"",
			"span",
			"br",
			"a",
			"p",
			"ul",
			"li",
			"table",
			"tbody",
			"tr",
			"td"
		};

		// Token: 0x04002E7C RID: 11900
		internal static int[] HtmlLengths = new int[]
		{
			0,
			0,
			4,
			2,
			1,
			1,
			2,
			2,
			5,
			6,
			2,
			2
		};

		// Token: 0x04002E7D RID: 11901
		internal static string[] XamlNames = new string[]
		{
			"",
			"",
			"Span",
			"LineBreak",
			"Hyperlink",
			"Paragraph",
			"InlineUIContainer",
			"BlockUIContainer",
			"Image",
			"List",
			"ListItem",
			"Table",
			"TableRowGroup",
			"TableRow",
			"TableCell",
			"Section",
			"Figure",
			"Floater",
			"Field",
			"ListText"
		};

		// Token: 0x04002E7E RID: 11902
		private bool _bPending;

		// Token: 0x04002E7F RID: 11903
		private bool _bTerminated;

		// Token: 0x04002E80 RID: 11904
		private DocumentNodeType _type;

		// Token: 0x04002E81 RID: 11905
		private FormatState _formatState;

		// Token: 0x04002E82 RID: 11906
		private string _xaml;

		// Token: 0x04002E83 RID: 11907
		private StringBuilder _contentBuilder;

		// Token: 0x04002E84 RID: 11908
		private int _childCount;

		// Token: 0x04002E85 RID: 11909
		private int _index;

		// Token: 0x04002E86 RID: 11910
		private DocumentNode _parent;

		// Token: 0x04002E87 RID: 11911
		private DocumentNodeArray _dna;

		// Token: 0x04002E88 RID: 11912
		private ColumnStateArray _csa;

		// Token: 0x04002E89 RID: 11913
		private int _nRowSpan;

		// Token: 0x04002E8A RID: 11914
		private int _nColSpan;

		// Token: 0x04002E8B RID: 11915
		private string _sCustom;

		// Token: 0x04002E8C RID: 11916
		private long _nVirtualListLevel;

		// Token: 0x04002E8D RID: 11917
		private bool _bHasMarkerContent;

		// Token: 0x04002E8E RID: 11918
		private bool _bMatched;
	}
}
