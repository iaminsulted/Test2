﻿using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MS.Internal.Globalization;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x020006E5 RID: 1765
	internal class XamlToRtfWriter
	{
		// Token: 0x06005CB2 RID: 23730 RVA: 0x00288278 File Offset: 0x00287278
		internal XamlToRtfWriter(string xaml)
		{
			this._xaml = xaml;
			this._rtfBuilder = new StringBuilder();
			this._xamlIn = new XamlToRtfWriter.XamlIn(this, xaml);
			this._converterState = new ConverterState();
			ColorTable colorTable = this._converterState.ColorTable;
			colorTable.AddColor(Color.FromArgb(byte.MaxValue, 0, 0, 0));
			colorTable.AddColor(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			FontTableEntry fontTableEntry = this._converterState.FontTable.DefineEntry(0);
			fontTableEntry.Name = "Times New Roman";
			fontTableEntry.ComputePreferredCodePage();
		}

		// Token: 0x06005CB3 RID: 23731 RVA: 0x00288314 File Offset: 0x00287314
		internal XamlToRtfError Process()
		{
			XamlToRtfError result = this._xamlIn.Parse();
			XamlToRtfWriter.XamlParserHelper.EnsureParagraphClosed(this._converterState);
			this._converterState.DocumentNodeArray.EstablishTreeRelationships();
			this.WriteOutput();
			return result;
		}

		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x06005CB4 RID: 23732 RVA: 0x00288342 File Offset: 0x00287342
		internal string Output
		{
			get
			{
				return this._rtfBuilder.ToString();
			}
		}

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x06005CB5 RID: 23733 RVA: 0x0028834F File Offset: 0x0028734F
		internal bool GenerateListTables
		{
			get
			{
				return this._xamlIn.GenerateListTables;
			}
		}

		// Token: 0x17001595 RID: 5525
		// (set) Token: 0x06005CB6 RID: 23734 RVA: 0x0028835C File Offset: 0x0028735C
		internal WpfPayload WpfPayload
		{
			set
			{
				this._wpfPayload = value;
			}
		}

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x06005CB7 RID: 23735 RVA: 0x00288365 File Offset: 0x00287365
		internal ConverterState ConverterState
		{
			get
			{
				return this._converterState;
			}
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x00288370 File Offset: 0x00287370
		private void BuildListTable()
		{
			ListLevelTable[] array = new ListLevelTable[9];
			for (int i = 0; i < 9; i++)
			{
				array[i] = new ListLevelTable();
			}
			ArrayList openLists = new ArrayList();
			int num = this.BuildListStyles(array, openLists);
			ListOverrideTable listOverrideTable = this._converterState.ListOverrideTable;
			for (int i = 0; i < num; i++)
			{
				ListOverride listOverride = listOverrideTable.AddEntry();
				listOverride.ID = (long)(i + 1);
				listOverride.Index = (long)(i + 1);
			}
			ListTable listTable = this._converterState.ListTable;
			for (int i = 0; i < num; i++)
			{
				ListTableEntry listTableEntry = listTable.AddEntry();
				listTableEntry.ID = (long)(i + 1);
				ListLevelTable levels = listTableEntry.Levels;
				for (int j = 0; j < 9; j++)
				{
					ListLevel listLevel = levels.AddEntry();
					ListLevelTable listLevelTable = array[j];
					if (listLevelTable.Count > i)
					{
						ListLevel listLevel2 = listLevelTable.EntryAt(i);
						listLevel.Marker = listLevel2.Marker;
						listLevel.StartIndex = listLevel2.StartIndex;
					}
				}
			}
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x0028845C File Offset: 0x0028745C
		private int BuildListStyles(ListLevelTable[] levels, ArrayList openLists)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			int num = -1;
			bool flag = false;
			int num2 = 0;
			for (int i = 0; i < documentNodeArray.Count; i++)
			{
				while (i == num)
				{
					if (openLists.Count > 0)
					{
						openLists.RemoveRange(openLists.Count - 1, 1);
						if (openLists.Count > 0)
						{
							DocumentNode documentNode = (DocumentNode)openLists[openLists.Count - 1];
							num = documentNode.Index + documentNode.ChildCount + 1;
						}
						else
						{
							num = -1;
						}
					}
					else
					{
						num = -1;
					}
				}
				DocumentNode documentNode2 = documentNodeArray.EntryAt(i);
				DocumentNodeType type = documentNode2.Type;
				if (type != DocumentNodeType.dnParagraph)
				{
					if (type != DocumentNodeType.dnList)
					{
						if (type == DocumentNodeType.dnListItem)
						{
							flag = true;
						}
					}
					else
					{
						openLists.Add(documentNode2);
						num = documentNode2.Index + documentNode2.ChildCount + 1;
						flag = true;
					}
				}
				else if (flag && openLists.Count > 0)
				{
					flag = false;
					DocumentNode documentNode3 = (DocumentNode)openLists[openLists.Count - 1];
					int num3 = openLists.Count;
					MarkerStyle marker = documentNode3.FormatState.Marker;
					long num4 = documentNode3.FormatState.StartIndex;
					if (num4 < 0L)
					{
						num4 = 1L;
					}
					if (num3 > 9)
					{
						num3 = 9;
					}
					ListLevelTable listLevelTable = levels[num3 - 1];
					int j;
					for (j = 0; j < listLevelTable.Count; j++)
					{
						ListLevel listLevel = listLevelTable.EntryAt(j);
						if (listLevel.Marker == marker && listLevel.StartIndex == num4)
						{
							break;
						}
					}
					if (j == listLevelTable.Count)
					{
						ListLevel listLevel = listLevelTable.AddEntry();
						listLevel.Marker = marker;
						listLevel.StartIndex = num4;
						if (listLevelTable.Count > num2)
						{
							num2 = listLevelTable.Count;
						}
					}
					if (num3 > 1)
					{
						documentNode2.FormatState.ILVL = (long)(num3 - 1);
					}
					documentNode2.FormatState.ILS = (long)(j + 1);
					for (j = 0; j < openLists.Count; j++)
					{
						documentNode3 = (DocumentNode)openLists[j];
						if (documentNode3.FormatState.PNLVL < (long)num3)
						{
							documentNode3.FormatState.PNLVL = (long)num3;
						}
						if (documentNode3.FormatState.ILS == -1L)
						{
							documentNode3.FormatState.ILS = documentNode2.FormatState.ILS;
						}
						else if (documentNode3.FormatState.ILS != documentNode2.FormatState.ILS)
						{
							documentNode3.FormatState.ILS = 0L;
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x002886D0 File Offset: 0x002876D0
		private void MergeParagraphMargins()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			for (int i = 0; i < documentNodeArray.Count; i++)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnParagraph)
				{
					long num = documentNode.FormatState.LI;
					long num2 = documentNode.FormatState.RI;
					DocumentNode parent = documentNode.Parent;
					while (parent != null && parent.Type != DocumentNodeType.dnCell)
					{
						if (parent.Type == DocumentNodeType.dnListItem || parent.Type == DocumentNodeType.dnList)
						{
							num += parent.FormatState.LI;
							num2 += parent.FormatState.RI;
						}
						parent = parent.Parent;
					}
					documentNode.FormatState.LI = num;
					documentNode.FormatState.RI = num2;
				}
			}
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x002887A0 File Offset: 0x002877A0
		private void GenerateListLabels()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			ArrayList arrayList = new ArrayList();
			long[] array = new long[documentNodeArray.Count];
			long[] array2 = new long[documentNodeArray.Count];
			int num = -1;
			for (int i = 0; i < documentNodeArray.Count; i++)
			{
				while (i == num)
				{
					if (arrayList.Count > 0)
					{
						arrayList.RemoveRange(arrayList.Count - 1, 1);
						if (arrayList.Count > 0)
						{
							DocumentNode documentNode = (DocumentNode)arrayList[arrayList.Count - 1];
							num = documentNode.Index + documentNode.ChildCount + 1;
						}
						else
						{
							num = -1;
						}
					}
					else
					{
						num = -1;
					}
				}
				DocumentNode documentNode2 = documentNodeArray.EntryAt(i);
				DocumentNodeType type = documentNode2.Type;
				if (type != DocumentNodeType.dnParagraph)
				{
					if (type != DocumentNodeType.dnList)
					{
						if (type == DocumentNodeType.dnListItem)
						{
							if (arrayList.Count > 0)
							{
								array[arrayList.Count - 1] = array[arrayList.Count - 1] + 1L;
							}
						}
					}
					else
					{
						arrayList.Add(documentNode2);
						array[arrayList.Count - 1] = documentNode2.FormatState.StartIndex - 1L;
						array2[arrayList.Count - 1] = documentNode2.FormatState.StartIndex;
						num = documentNode2.Index + documentNode2.ChildCount + 1;
					}
				}
				else if (documentNode2.FormatState.ListLevel > 0L && arrayList.Count > 0)
				{
					DocumentNode documentNode3 = (DocumentNode)arrayList[arrayList.Count - 1];
					long nCount = array[arrayList.Count - 1];
					long startIndex = array2[arrayList.Count - 1];
					documentNode2.FormatState.StartIndex = startIndex;
					documentNode2.ListLabel = Converters.MarkerCountToString(documentNode3.FormatState.Marker, nCount);
				}
			}
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x00288960 File Offset: 0x00287960
		private void SetParagraphStructureProperties()
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			for (int i = 0; i < documentNodeArray.Count; i++)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnParagraph)
				{
					long num = 0L;
					for (DocumentNode parent = documentNode.Parent; parent != null; parent = parent.Parent)
					{
						if (parent.Type == DocumentNodeType.dnCell)
						{
							num += 1L;
						}
					}
					if (num > 1L)
					{
						documentNode.FormatState.ITAP = num;
					}
					if (num != 0L)
					{
						documentNode.FormatState.IsInTable = true;
					}
				}
			}
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x002889E8 File Offset: 0x002879E8
		private void WriteProlog()
		{
			this._rtfBuilder.Append("{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp");
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			for (int i = 0; i < documentNodeArray.Count; i++)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (documentNode.FormatState.Font >= 0L)
				{
					this._rtfBuilder.Append("\\deff");
					this._rtfBuilder.Append(documentNode.FormatState.Font.ToString(CultureInfo.InvariantCulture));
					return;
				}
			}
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x00288A70 File Offset: 0x00287A70
		private void WriteHeaderTables()
		{
			this.WriteFontTable();
			this.WriteColorTable();
			if (this.GenerateListTables)
			{
				this.WriteListTable();
			}
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x00288A8C File Offset: 0x00287A8C
		private void WriteFontTable()
		{
			FontTable fontTable = this._converterState.FontTable;
			this._rtfBuilder.Append("{\\fonttbl");
			for (int i = 0; i < fontTable.Count; i++)
			{
				FontTableEntry fontTableEntry = fontTable.EntryAt(i);
				this._rtfBuilder.Append("{");
				this._rtfBuilder.Append("\\f");
				this._rtfBuilder.Append(fontTableEntry.Index.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append("\\fcharset");
				this._rtfBuilder.Append(fontTableEntry.CharSet.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append(" ");
				XamlToRtfWriter.XamlParserHelper.AppendRTFText(this._rtfBuilder, fontTableEntry.Name, fontTableEntry.CodePage);
				this._rtfBuilder.Append(";}");
			}
			this._rtfBuilder.Append("}");
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x00288B90 File Offset: 0x00287B90
		private void WriteColorTable()
		{
			ColorTable colorTable = this._converterState.ColorTable;
			this._rtfBuilder.Append("{\\colortbl");
			for (int i = 0; i < colorTable.Count; i++)
			{
				Color color = colorTable.ColorAt(i);
				this._rtfBuilder.Append("\\red");
				this._rtfBuilder.Append(color.R.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append("\\green");
				this._rtfBuilder.Append(color.G.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append("\\blue");
				this._rtfBuilder.Append(color.B.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append(";");
			}
			this._rtfBuilder.Append("}");
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x00288C90 File Offset: 0x00287C90
		private void WriteListTable()
		{
			ListTable listTable = this._converterState.ListTable;
			if (listTable.Count > 0)
			{
				this._rtfBuilder.Append("\r\n{\\*\\listtable");
				int num = 5;
				for (int i = 0; i < listTable.Count; i++)
				{
					ListTableEntry listTableEntry = listTable.EntryAt(i);
					this._rtfBuilder.Append("\r\n{\\list");
					this._rtfBuilder.Append("\\listtemplateid");
					this._rtfBuilder.Append(listTableEntry.ID.ToString(CultureInfo.InvariantCulture));
					this._rtfBuilder.Append("\\listhybrid");
					ListLevelTable levels = listTableEntry.Levels;
					for (int j = 0; j < levels.Count; j++)
					{
						ListLevel listLevel = levels.EntryAt(j);
						long num2 = (long)listLevel.Marker;
						this._rtfBuilder.Append("\r\n{\\listlevel");
						this._rtfBuilder.Append("\\levelnfc");
						this._rtfBuilder.Append(num2.ToString(CultureInfo.InvariantCulture));
						this._rtfBuilder.Append("\\levelnfcn");
						this._rtfBuilder.Append(num2.ToString(CultureInfo.InvariantCulture));
						this._rtfBuilder.Append("\\leveljc0");
						this._rtfBuilder.Append("\\leveljcn0");
						this._rtfBuilder.Append("\\levelfollow0");
						this._rtfBuilder.Append("\\levelstartat");
						this._rtfBuilder.Append(listLevel.StartIndex);
						this._rtfBuilder.Append("\\levelspace0");
						this._rtfBuilder.Append("\\levelindent0");
						this._rtfBuilder.Append("{\\leveltext");
						this._rtfBuilder.Append("\\leveltemplateid");
						this._rtfBuilder.Append(num.ToString(CultureInfo.InvariantCulture));
						num++;
						if (listLevel.Marker == MarkerStyle.MarkerBullet)
						{
							this._rtfBuilder.Append("\\'01\\'b7}");
							this._rtfBuilder.Append("{\\levelnumbers;}");
						}
						else
						{
							this._rtfBuilder.Append("\\'02\\'0");
							this._rtfBuilder.Append(j.ToString(CultureInfo.InvariantCulture));
							this._rtfBuilder.Append(".;}");
							this._rtfBuilder.Append("{\\levelnumbers\\'01;}");
						}
						this._rtfBuilder.Append("\\fi-360");
						this._rtfBuilder.Append("\\li");
						string value = ((j + 1) * 720).ToString(CultureInfo.InvariantCulture);
						this._rtfBuilder.Append(value);
						this._rtfBuilder.Append("\\lin");
						this._rtfBuilder.Append(value);
						this._rtfBuilder.Append("\\jclisttab\\tx");
						this._rtfBuilder.Append(value);
						this._rtfBuilder.Append("}");
					}
					this._rtfBuilder.Append("\r\n{\\listname ;}");
					this._rtfBuilder.Append("\\listid");
					this._rtfBuilder.Append(listTableEntry.ID.ToString(CultureInfo.InvariantCulture));
					this._rtfBuilder.Append("}");
				}
				this._rtfBuilder.Append("}\r\n");
			}
			ListOverrideTable listOverrideTable = this._converterState.ListOverrideTable;
			if (listOverrideTable.Count > 0)
			{
				this._rtfBuilder.Append("{\\*\\listoverridetable");
				for (int k = 0; k < listOverrideTable.Count; k++)
				{
					ListOverride listOverride = listOverrideTable.EntryAt(k);
					this._rtfBuilder.Append("\r\n{\\listoverride");
					this._rtfBuilder.Append("\\listid");
					this._rtfBuilder.Append(listOverride.ID.ToString(CultureInfo.InvariantCulture));
					this._rtfBuilder.Append("\\listoverridecount0");
					if (listOverride.StartIndex > 0L)
					{
						this._rtfBuilder.Append("\\levelstartat");
						this._rtfBuilder.Append(listOverride.StartIndex.ToString(CultureInfo.InvariantCulture));
					}
					this._rtfBuilder.Append("\\ls");
					this._rtfBuilder.Append(listOverride.Index.ToString(CultureInfo.InvariantCulture));
					this._rtfBuilder.Append("}");
				}
				this._rtfBuilder.Append("\r\n}\r\n");
			}
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x0028913B File Offset: 0x0028813B
		private void WriteEmptyChild(DocumentNode documentNode)
		{
			if (documentNode.Type == DocumentNodeType.dnLineBreak)
			{
				this._rtfBuilder.Append("\\line ");
			}
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x00289158 File Offset: 0x00288158
		private void WriteInlineChild(DocumentNode documentNode)
		{
			if (documentNode.IsEmptyNode)
			{
				this.WriteEmptyChild(documentNode);
				return;
			}
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			FormatState formatState = documentNode.FormatState;
			FormatState formatState2 = (documentNode.Parent != null) ? documentNode.Parent.FormatState : FormatState.EmptyFormatState;
			bool flag = formatState.Font != formatState2.Font;
			bool flag2 = formatState.Bold != formatState2.Bold;
			bool flag3 = formatState.Italic != formatState2.Italic;
			bool flag4 = formatState.UL != formatState2.UL;
			bool flag5 = formatState.FontSize != formatState2.FontSize;
			bool flag6 = formatState.CF != formatState2.CF;
			bool flag7 = formatState.CB != formatState2.CB;
			bool flag8 = formatState.Strike != formatState2.Strike;
			bool flag9 = formatState.Super != formatState2.Super;
			bool flag10 = formatState.Sub != formatState2.Sub;
			bool flag11 = formatState.Lang != formatState2.Lang && formatState.Lang > 0L;
			bool flag12 = formatState.DirChar != DirState.DirDefault && (documentNode.Parent == null || !documentNode.Parent.IsInline || formatState.Lang != formatState2.Lang);
			bool flag13 = flag || flag2 || flag3 || flag4 || flag11 || flag12 || flag5 || flag6 || flag7 || flag8 || flag9 || flag10;
			if (flag13)
			{
				this._rtfBuilder.Append("{");
			}
			if (flag11)
			{
				this._rtfBuilder.Append("\\lang");
				this._rtfBuilder.Append(formatState.Lang.ToString(CultureInfo.InvariantCulture));
			}
			if (flag)
			{
				this._rtfBuilder.Append("\\loch");
				this._rtfBuilder.Append("\\f");
				this._rtfBuilder.Append(formatState.Font.ToString(CultureInfo.InvariantCulture));
			}
			if (flag2)
			{
				if (formatState.Bold)
				{
					this._rtfBuilder.Append("\\b");
				}
				else
				{
					this._rtfBuilder.Append("\\b0");
				}
			}
			if (flag3)
			{
				if (formatState.Italic)
				{
					this._rtfBuilder.Append("\\i");
				}
				else
				{
					this._rtfBuilder.Append("\\i0");
				}
			}
			if (flag4)
			{
				if (formatState.UL != ULState.ULNone)
				{
					this._rtfBuilder.Append("\\ul");
				}
				else
				{
					this._rtfBuilder.Append("\\ul0");
				}
			}
			if (flag8)
			{
				if (formatState.Strike != StrikeState.StrikeNone)
				{
					this._rtfBuilder.Append("\\strike");
				}
				else
				{
					this._rtfBuilder.Append("\\strike0");
				}
			}
			if (flag5)
			{
				this._rtfBuilder.Append("\\fs");
				this._rtfBuilder.Append(formatState.FontSize.ToString(CultureInfo.InvariantCulture));
			}
			if (flag6)
			{
				this._rtfBuilder.Append("\\cf");
				this._rtfBuilder.Append(formatState.CF.ToString(CultureInfo.InvariantCulture));
			}
			if (flag7)
			{
				this._rtfBuilder.Append("\\highlight");
				this._rtfBuilder.Append(formatState.CB.ToString(CultureInfo.InvariantCulture));
			}
			if (flag9)
			{
				if (formatState.Super)
				{
					this._rtfBuilder.Append("\\super");
				}
				else
				{
					this._rtfBuilder.Append("\\super0");
				}
			}
			if (flag10)
			{
				if (formatState.Sub)
				{
					this._rtfBuilder.Append("\\sub");
				}
				else
				{
					this._rtfBuilder.Append("\\sub0");
				}
			}
			if (flag12)
			{
				if (formatState.DirChar == DirState.DirLTR)
				{
					this._rtfBuilder.Append("\\ltrch");
				}
				else
				{
					this._rtfBuilder.Append("\\rtlch");
				}
			}
			if (flag13)
			{
				this._rtfBuilder.Append(" ");
			}
			if (documentNode.Type == DocumentNodeType.dnHyperlink && !string.IsNullOrEmpty(documentNode.NavigateUri))
			{
				this._rtfBuilder.Append("{\\field{\\*\\fldinst { HYPERLINK \"");
				documentNode.NavigateUri = BamlResourceContentUtil.UnescapeString(documentNode.NavigateUri);
				for (int i = 0; i < documentNode.NavigateUri.Length; i++)
				{
					if (documentNode.NavigateUri[i] == '\\')
					{
						this._rtfBuilder.Append("\\\\");
					}
					else
					{
						this._rtfBuilder.Append(documentNode.NavigateUri[i]);
					}
				}
				this._rtfBuilder.Append("\" }}{\\fldrslt {");
			}
			else
			{
				this._rtfBuilder.Append(documentNode.Content);
			}
			if (documentNode.Type == DocumentNodeType.dnImage)
			{
				this.WriteImage(documentNode);
			}
			int index = documentNode.Index;
			for (int j = index + 1; j <= index + documentNode.ChildCount; j++)
			{
				DocumentNode documentNode2 = documentNodeArray.EntryAt(j);
				if (documentNode2.Parent == documentNode)
				{
					this.WriteInlineChild(documentNode2);
				}
			}
			if (documentNode.Type == DocumentNodeType.dnHyperlink && !string.IsNullOrEmpty(documentNode.NavigateUri))
			{
				this._rtfBuilder.Append("}}}");
			}
			if (flag13)
			{
				this._rtfBuilder.Append("}");
			}
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x002896AC File Offset: 0x002886AC
		private void WriteUIContainerChild(DocumentNode documentNode)
		{
			this._rtfBuilder.Append("{");
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			int index = documentNode.Index;
			for (int i = index + 1; i <= index + documentNode.ChildCount; i++)
			{
				DocumentNode documentNode2 = documentNodeArray.EntryAt(i);
				if (documentNode2.Parent == documentNode && documentNode2.Type == DocumentNodeType.dnImage)
				{
					this.WriteImage(documentNode2);
				}
			}
			if (documentNode.Type == DocumentNodeType.dnBlockUIContainer)
			{
				this._rtfBuilder.Append("\\par");
			}
			this._rtfBuilder.Append("}");
			this._rtfBuilder.Append("\r\n");
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x00289750 File Offset: 0x00288750
		private void WriteSection(DocumentNode dnThis)
		{
			int index = dnThis.Index;
			int num = index + 1;
			FormatState formatState = dnThis.FormatState;
			FormatState formatState2 = (dnThis.Parent != null) ? dnThis.Parent.FormatState : FormatState.EmptyFormatState;
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			this._rtfBuilder.Append("{");
			if (formatState.Lang != formatState2.Lang && formatState.Lang > 0L)
			{
				this._rtfBuilder.Append("\\lang");
				this._rtfBuilder.Append(formatState.Lang.ToString(CultureInfo.InvariantCulture));
			}
			if (formatState.DirPara == DirState.DirRTL)
			{
				this._rtfBuilder.Append("\\rtlpar");
			}
			if (this.WriteParagraphFontInfo(dnThis, formatState, formatState2))
			{
				this._rtfBuilder.Append(" ");
			}
			if (formatState.CF != formatState2.CF)
			{
				this._rtfBuilder.Append("\\cf");
				this._rtfBuilder.Append(formatState.CF.ToString(CultureInfo.InvariantCulture));
			}
			switch (formatState.HAlign)
			{
			case HAlign.AlignLeft:
				if (formatState.DirPara != DirState.DirRTL)
				{
					this._rtfBuilder.Append("\\ql");
				}
				else
				{
					this._rtfBuilder.Append("\\qr");
				}
				break;
			case HAlign.AlignRight:
				if (formatState.DirPara != DirState.DirRTL)
				{
					this._rtfBuilder.Append("\\qr");
				}
				else
				{
					this._rtfBuilder.Append("\\ql");
				}
				break;
			case HAlign.AlignCenter:
				this._rtfBuilder.Append("\\qc");
				break;
			case HAlign.AlignJustify:
				this._rtfBuilder.Append("\\qj");
				break;
			}
			if (formatState.SL != 0L)
			{
				this._rtfBuilder.Append("\\sl");
				this._rtfBuilder.Append(formatState.SL.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append("\\slmult0");
			}
			for (int i = num; i <= index + dnThis.ChildCount; i++)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (documentNode.Parent == dnThis)
				{
					this.WriteStructure(documentNode);
				}
			}
			this._rtfBuilder.Append("}");
			this._rtfBuilder.Append("\r\n");
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x002899AC File Offset: 0x002889AC
		private void WriteParagraph(DocumentNode dnThis)
		{
			int index = dnThis.Index;
			int num = index + 1;
			FormatState formatState = dnThis.FormatState;
			FormatState fsParent = (dnThis.Parent != null) ? dnThis.Parent.FormatState : FormatState.EmptyFormatState;
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			this._rtfBuilder.Append("{");
			bool flag = this.WriteParagraphFontInfo(dnThis, formatState, fsParent);
			if (formatState.IsInTable)
			{
				this._rtfBuilder.Append("\\intbl");
				flag = true;
			}
			if (flag)
			{
				this._rtfBuilder.Append(" ");
			}
			flag = this.WriteParagraphListInfo(dnThis, formatState);
			if (flag)
			{
				this._rtfBuilder.Append(" ");
			}
			if (formatState.DirPara == DirState.DirRTL)
			{
				this._rtfBuilder.Append("\\rtlpar");
			}
			for (int i = num; i <= index + dnThis.ChildCount; i++)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (documentNode.Parent == dnThis)
				{
					this.WriteInlineChild(documentNode);
				}
			}
			if (formatState.ITAP > 1L)
			{
				this._rtfBuilder.Append("\\itap");
				this._rtfBuilder.Append(formatState.ITAP.ToString(CultureInfo.InvariantCulture));
			}
			this._rtfBuilder.Append("\\li");
			this._rtfBuilder.Append(formatState.LI.ToString(CultureInfo.InvariantCulture));
			this._rtfBuilder.Append("\\ri");
			this._rtfBuilder.Append(formatState.RI.ToString(CultureInfo.InvariantCulture));
			this._rtfBuilder.Append("\\sa");
			this._rtfBuilder.Append(formatState.SA.ToString(CultureInfo.InvariantCulture));
			this._rtfBuilder.Append("\\sb");
			this._rtfBuilder.Append(formatState.SB.ToString(CultureInfo.InvariantCulture));
			if (formatState.HasParaBorder)
			{
				this._rtfBuilder.Append(formatState.ParaBorder.RTFEncoding);
			}
			if (dnThis.ListLabel != null)
			{
				this._rtfBuilder.Append("\\jclisttab\\tx");
				this._rtfBuilder.Append(formatState.LI.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append("\\fi-360");
			}
			else
			{
				this._rtfBuilder.Append("\\fi");
				this._rtfBuilder.Append(formatState.FI.ToString(CultureInfo.InvariantCulture));
			}
			switch (formatState.HAlign)
			{
			case HAlign.AlignLeft:
				if (formatState.DirPara != DirState.DirRTL)
				{
					this._rtfBuilder.Append("\\ql");
				}
				else
				{
					this._rtfBuilder.Append("\\qr");
				}
				break;
			case HAlign.AlignRight:
				if (formatState.DirPara != DirState.DirRTL)
				{
					this._rtfBuilder.Append("\\qr");
				}
				else
				{
					this._rtfBuilder.Append("\\ql");
				}
				break;
			case HAlign.AlignCenter:
				this._rtfBuilder.Append("\\qc");
				break;
			case HAlign.AlignJustify:
				this._rtfBuilder.Append("\\qj");
				break;
			}
			if (formatState.CBPara >= 0L)
			{
				this._rtfBuilder.Append("\\cbpat");
				this._rtfBuilder.Append(formatState.CBPara.ToString(CultureInfo.InvariantCulture));
			}
			if (formatState.SL != 0L)
			{
				this._rtfBuilder.Append("\\sl");
				this._rtfBuilder.Append(formatState.SL.ToString(CultureInfo.InvariantCulture));
				this._rtfBuilder.Append("\\slmult0");
			}
			if (dnThis.IsLastParagraphInCell())
			{
				dnThis.GetParentOfType(DocumentNodeType.dnCell).IsTerminated = true;
				if (formatState.ITAP > 1L)
				{
					this._rtfBuilder.Append("\\nestcell");
					this._rtfBuilder.Append("{\\nonesttables\\par}");
				}
				else
				{
					this._rtfBuilder.Append("\\cell");
				}
				this._rtfBuilder.Append("\r\n");
			}
			else
			{
				this._rtfBuilder.Append("\\par");
			}
			this._rtfBuilder.Append("}");
			this._rtfBuilder.Append("\r\n");
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x00289E08 File Offset: 0x00288E08
		private bool WriteParagraphFontInfo(DocumentNode dnThis, FormatState fsThis, FormatState fsParent)
		{
			int index = dnThis.Index;
			int num = index + 1;
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			bool result = false;
			long num2 = -2L;
			long num3 = -2L;
			for (int i = num; i <= index + dnThis.ChildCount; i++)
			{
				DocumentNode documentNode = documentNodeArray.EntryAt(i);
				if (documentNode.Parent == dnThis)
				{
					if (num2 == -2L)
					{
						num2 = documentNode.FormatState.FontSize;
					}
					else if (num2 != documentNode.FormatState.FontSize)
					{
						num2 = -3L;
					}
					if (num3 == -2L)
					{
						num3 = documentNode.FormatState.Font;
					}
					else if (num3 != documentNode.FormatState.Font)
					{
						num3 = -3L;
					}
				}
			}
			if (num2 >= 0L)
			{
				fsThis.FontSize = num2;
			}
			if (num3 >= 0L)
			{
				fsThis.Font = num3;
			}
			bool flag = dnThis.Type == DocumentNodeType.dnParagraph && dnThis.Parent != null && dnThis.Parent.Type == DocumentNodeType.dnSection && dnThis.Parent.Parent == null;
			if (fsThis.FontSize != fsParent.FontSize)
			{
				this._rtfBuilder.Append("\\fs");
				this._rtfBuilder.Append(fsThis.FontSize.ToString(CultureInfo.InvariantCulture));
				result = true;
			}
			if (fsThis.Font != fsParent.Font || flag)
			{
				this._rtfBuilder.Append("\\f");
				this._rtfBuilder.Append(fsThis.Font.ToString(CultureInfo.InvariantCulture));
				result = true;
			}
			if (fsThis.Bold != fsParent.Bold)
			{
				this._rtfBuilder.Append("\\b");
				result = true;
			}
			if (fsThis.Italic != fsParent.Italic)
			{
				this._rtfBuilder.Append("\\i");
				result = true;
			}
			if (fsThis.UL != fsParent.UL)
			{
				this._rtfBuilder.Append("\\ul");
				result = true;
			}
			if (fsThis.Strike != fsParent.Strike)
			{
				this._rtfBuilder.Append("\\strike");
				result = true;
			}
			if (fsThis.CF != fsParent.CF)
			{
				this._rtfBuilder.Append("\\cf");
				this._rtfBuilder.Append(fsThis.CF.ToString(CultureInfo.InvariantCulture));
				result = true;
			}
			return result;
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x0028A054 File Offset: 0x00289054
		private bool WriteParagraphListInfo(DocumentNode dnThis, FormatState fsThis)
		{
			bool result = false;
			bool flag = this.GenerateListTables;
			if (dnThis.ListLabel != null)
			{
				DocumentNode parentOfType = dnThis.GetParentOfType(DocumentNodeType.dnList);
				if (parentOfType != null)
				{
					if (flag && parentOfType.FormatState.PNLVL == 1L)
					{
						flag = false;
					}
					if (flag)
					{
						this._rtfBuilder.Append("{\\listtext ");
						this._rtfBuilder.Append(dnThis.ListLabel);
						if (parentOfType.FormatState.Marker != MarkerStyle.MarkerBullet && parentOfType.FormatState.Marker != MarkerStyle.MarkerNone)
						{
							this._rtfBuilder.Append(".");
						}
						this._rtfBuilder.Append("\\tab}");
						if (fsThis.ILS > 0L)
						{
							this._rtfBuilder.Append("\\ls");
							this._rtfBuilder.Append(fsThis.ILS.ToString(CultureInfo.InvariantCulture));
							result = true;
						}
						if (fsThis.ILVL > 0L)
						{
							this._rtfBuilder.Append("\\ilvl");
							this._rtfBuilder.Append(fsThis.ILVL.ToString(CultureInfo.InvariantCulture));
							result = true;
						}
					}
					else
					{
						this._rtfBuilder.Append("{\\pntext ");
						this._rtfBuilder.Append(dnThis.ListLabel);
						if (parentOfType.FormatState.Marker != MarkerStyle.MarkerBullet && parentOfType.FormatState.Marker != MarkerStyle.MarkerNone)
						{
							this._rtfBuilder.Append(".");
						}
						this._rtfBuilder.Append("\\tab}{\\*\\pn");
						this._rtfBuilder.Append(Converters.MarkerStyleToOldRTFString(parentOfType.FormatState.Marker));
						if (fsThis.ListLevel > 0L && parentOfType.FormatState.PNLVL > 1L)
						{
							this._rtfBuilder.Append("\\pnlvl");
							this._rtfBuilder.Append(fsThis.ListLevel.ToString(CultureInfo.InvariantCulture));
						}
						if (fsThis.FI > 0L)
						{
							this._rtfBuilder.Append("\\pnhang");
						}
						if (fsThis.StartIndex >= 0L)
						{
							this._rtfBuilder.Append("\\pnstart");
							this._rtfBuilder.Append(fsThis.StartIndex.ToString(CultureInfo.InvariantCulture));
						}
						if (parentOfType.FormatState.Marker == MarkerStyle.MarkerBullet)
						{
							this._rtfBuilder.Append("{\\pntxtb\\'B7}}");
						}
						else if (parentOfType.FormatState.Marker == MarkerStyle.MarkerNone)
						{
							this._rtfBuilder.Append("{\\pntxta }{\\pntxtb }}");
						}
						else
						{
							this._rtfBuilder.Append("{\\pntxta .}}");
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x0028A2F4 File Offset: 0x002892F4
		private void WriteRow(DocumentNode dnRow)
		{
			int tableDepth = dnRow.GetTableDepth();
			this._rtfBuilder.Append("\r\n");
			this._rtfBuilder.Append("{");
			if (tableDepth == 1)
			{
				this.WriteRowStart(dnRow);
				this.WriteRowSettings(dnRow);
				this.WriteRowsCellProperties(dnRow);
			}
			else if (tableDepth > 1)
			{
				this._rtfBuilder.Append("\\intbl\\itap");
				this._rtfBuilder.Append(tableDepth.ToString(CultureInfo.InvariantCulture));
			}
			this.WriteRowsCellContents(dnRow);
			if (tableDepth > 1)
			{
				this._rtfBuilder.Append("\\intbl\\itap");
				this._rtfBuilder.Append(tableDepth.ToString(CultureInfo.InvariantCulture));
			}
			this._rtfBuilder.Append("{");
			if (tableDepth > 1)
			{
				this._rtfBuilder.Append("\\*\\nesttableprops");
			}
			this.WriteRowStart(dnRow);
			this.WriteRowSettings(dnRow);
			this.WriteRowsCellProperties(dnRow);
			if (tableDepth > 1)
			{
				this._rtfBuilder.Append("\\nestrow");
			}
			else
			{
				this._rtfBuilder.Append("\\row");
			}
			this._rtfBuilder.Append("}}");
			this._rtfBuilder.Append("\r\n");
		}

		// Token: 0x06005CCA RID: 23754 RVA: 0x0028A42B File Offset: 0x0028942B
		private void WriteRowStart(DocumentNode dnRow)
		{
			this._rtfBuilder.Append("\\trowd");
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x0028A440 File Offset: 0x00289440
		private void WriteRowSettings(DocumentNode dnRow)
		{
			DocumentNode parentOfType = dnRow.GetParentOfType(DocumentNodeType.dnTable);
			int num = (int)((parentOfType != null) ? parentOfType.XamlDir : DirState.DirLTR);
			DirState dirState = (parentOfType != null) ? parentOfType.ParentXamlDir : DirState.DirLTR;
			if (parentOfType != null)
			{
				string value = ((dirState == DirState.DirLTR) ? parentOfType.FormatState.LI : parentOfType.FormatState.RI).ToString(CultureInfo.InvariantCulture);
				this._rtfBuilder.Append("\\trleft");
				this._rtfBuilder.Append(value);
				this._rtfBuilder.Append("\\trgaph-");
				this._rtfBuilder.Append(value);
			}
			else
			{
				this._rtfBuilder.Append("\\trgaph0");
				this._rtfBuilder.Append("\\trleft0");
			}
			this.WriteRowBorders(dnRow);
			this.WriteRowDimensions(dnRow);
			this.WriteRowPadding(dnRow);
			this._rtfBuilder.Append("\\trql");
			if (num == 2)
			{
				this._rtfBuilder.Append("\\rtlrow");
				return;
			}
			this._rtfBuilder.Append("\\ltrrow");
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x0028A548 File Offset: 0x00289548
		private void WriteRowBorders(DocumentNode dnRow)
		{
			DocumentNodeArray rowsCells = dnRow.GetRowsCells();
			if (rowsCells.Count > 0)
			{
				DocumentNode documentNode = rowsCells.EntryAt(0);
				if (documentNode.FormatState.HasRowFormat)
				{
					CellFormat rowCellFormat = documentNode.FormatState.RowFormat.RowCellFormat;
					this.WriteBorder("\\trbrdrt", rowCellFormat.BorderTop);
					this.WriteBorder("\\trbrdrb", rowCellFormat.BorderBottom);
					this.WriteBorder("\\trbrdrr", rowCellFormat.BorderRight);
					this.WriteBorder("\\trbrdrl", rowCellFormat.BorderLeft);
					this.WriteBorder("\\trbrdrv", rowCellFormat.BorderLeft);
					this.WriteBorder("\\trbrdrh", rowCellFormat.BorderTop);
				}
			}
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x0028A5F4 File Offset: 0x002895F4
		private void WriteRowDimensions(DocumentNode dnRow)
		{
			this._rtfBuilder.Append("\\trftsWidth1");
			this._rtfBuilder.Append("\\trftsWidthB3");
		}

		// Token: 0x06005CCE RID: 23758 RVA: 0x0028A618 File Offset: 0x00289618
		private void WriteRowPadding(DocumentNode dnRow)
		{
			this._rtfBuilder.Append("\\trpaddl10");
			this._rtfBuilder.Append("\\trpaddr10");
			this._rtfBuilder.Append("\\trpaddb10");
			this._rtfBuilder.Append("\\trpaddt10");
			this._rtfBuilder.Append("\\trpaddfl3");
			this._rtfBuilder.Append("\\trpaddfr3");
			this._rtfBuilder.Append("\\trpaddft3");
			this._rtfBuilder.Append("\\trpaddfb3");
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x0028A6B0 File Offset: 0x002896B0
		private void WriteRowsCellProperties(DocumentNode dnRow)
		{
			DocumentNodeArray rowsCells = dnRow.GetRowsCells();
			int num = 0;
			long lastCellX = 0L;
			for (int i = 0; i < rowsCells.Count; i++)
			{
				DocumentNode documentNode = rowsCells.EntryAt(i);
				lastCellX = this.WriteCellProperties(documentNode, num, lastCellX);
				num += documentNode.ColSpan;
			}
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x0028A6F8 File Offset: 0x002896F8
		private void WriteRowsCellContents(DocumentNode dnRow)
		{
			DocumentNodeArray rowsCells = dnRow.GetRowsCells();
			this._rtfBuilder.Append("{");
			for (int i = 0; i < rowsCells.Count; i++)
			{
				DocumentNode dnThis = rowsCells.EntryAt(i);
				this.WriteStructure(dnThis);
			}
			this._rtfBuilder.Append("}");
		}

		// Token: 0x06005CD1 RID: 23761 RVA: 0x0028A750 File Offset: 0x00289750
		private long WriteCellProperties(DocumentNode dnCell, int nCol, long lastCellX)
		{
			this.WriteCellColor(dnCell);
			if (dnCell.FormatState.HasRowFormat)
			{
				if (dnCell.FormatState.RowFormat.RowCellFormat.IsVMergeFirst)
				{
					this._rtfBuilder.Append("\\clvmgf");
				}
				else if (dnCell.FormatState.RowFormat.RowCellFormat.IsVMerge)
				{
					this._rtfBuilder.Append("\\clvmrg");
				}
			}
			this.WriteCellVAlignment(dnCell);
			this.WriteCellBorders(dnCell);
			this.WriteCellPadding(dnCell);
			return this.WriteCellDimensions(dnCell, nCol, lastCellX);
		}

		// Token: 0x06005CD2 RID: 23762 RVA: 0x0028A7E1 File Offset: 0x002897E1
		private void WriteCellVAlignment(DocumentNode dnCell)
		{
			this._rtfBuilder.Append("\\clvertalt");
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x0028A7F4 File Offset: 0x002897F4
		private void WriteCellBorders(DocumentNode dnCell)
		{
			if (dnCell.FormatState.HasRowFormat)
			{
				CellFormat rowCellFormat = dnCell.FormatState.RowFormat.RowCellFormat;
				this.WriteBorder("\\clbrdrt", rowCellFormat.BorderTop);
				this.WriteBorder("\\clbrdrl", rowCellFormat.BorderLeft);
				this.WriteBorder("\\clbrdrb", rowCellFormat.BorderBottom);
				this.WriteBorder("\\clbrdrr", rowCellFormat.BorderRight);
				return;
			}
			this.WriteBorder("\\clbrdrt", BorderFormat.EmptyBorderFormat);
			this.WriteBorder("\\clbrdrl", BorderFormat.EmptyBorderFormat);
			this.WriteBorder("\\clbrdrb", BorderFormat.EmptyBorderFormat);
			this.WriteBorder("\\clbrdrr", BorderFormat.EmptyBorderFormat);
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private void WriteCellPadding(DocumentNode dnCell)
		{
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x0028A8A4 File Offset: 0x002898A4
		private void WriteCellColor(DocumentNode dnCell)
		{
			FormatState formatState = null;
			if (dnCell.FormatState.CBPara >= 0L)
			{
				formatState = dnCell.FormatState;
			}
			else if (dnCell.Parent != null && dnCell.Parent.FormatState.CBPara >= 0L)
			{
				formatState = dnCell.Parent.FormatState;
			}
			if (formatState != null)
			{
				this._rtfBuilder.Append("\\clcbpat");
				this._rtfBuilder.Append(formatState.CBPara.ToString(CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x0028A928 File Offset: 0x00289928
		private long WriteCellDimensions(DocumentNode dnCell, int nCol, long lastCellX)
		{
			DocumentNode parentOfType = dnCell.GetParentOfType(DocumentNodeType.dnTable);
			if (parentOfType.FormatState.HasRowFormat)
			{
				RowFormat rowFormat = parentOfType.FormatState.RowFormat;
				CellFormat cellFormat = rowFormat.NthCellFormat(nCol);
				if (dnCell.ColSpan > 1)
				{
					CellFormat cellFormat2 = new CellFormat(cellFormat);
					for (int i = 1; i < dnCell.ColSpan; i++)
					{
						cellFormat = rowFormat.NthCellFormat(nCol + i);
						cellFormat2.Width.Value += cellFormat.Width.Value;
						cellFormat2.CellX = cellFormat.CellX;
					}
					if (cellFormat2.CellX == -1L || rowFormat.CellCount == 0)
					{
						cellFormat2.CellX = lastCellX + (long)(dnCell.ColSpan * 1440) + this.GetDefaultAllTablesWidthFromCell(dnCell);
					}
					this._rtfBuilder.Append(cellFormat2.RTFEncodingForWidth);
					lastCellX = cellFormat2.CellX;
				}
				else
				{
					if (cellFormat.CellX == -1L || rowFormat.CellCount == 0)
					{
						cellFormat.CellX = lastCellX + 1440L + this.GetDefaultAllTablesWidthFromCell(dnCell);
					}
					this._rtfBuilder.Append(cellFormat.RTFEncodingForWidth);
					lastCellX = cellFormat.CellX;
				}
			}
			else
			{
				this._rtfBuilder.Append("\\clftsWidth1");
				this._rtfBuilder.Append("\\cellx");
				long num = lastCellX + (long)(dnCell.ColSpan * 1440);
				this._rtfBuilder.Append(num.ToString(CultureInfo.InvariantCulture));
				lastCellX = num;
			}
			return lastCellX;
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x0028AAA0 File Offset: 0x00289AA0
		private long GetDefaultAllTablesWidthFromCell(DocumentNode dnCell)
		{
			long num = 0L;
			for (int i = dnCell.Index + 1; i <= dnCell.Index + dnCell.ChildCount; i++)
			{
				DocumentNode documentNode = this._converterState.DocumentNodeArray.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnTable)
				{
					num += this.CalculateDefaultTableWidth(documentNode);
				}
			}
			return num;
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x0028AAF8 File Offset: 0x00289AF8
		private long CalculateDefaultTableWidth(DocumentNode dnTable)
		{
			long num = 0L;
			long num2 = 0L;
			for (int i = dnTable.Index + 1; i <= dnTable.Index + dnTable.ChildCount; i++)
			{
				DocumentNode documentNode = this._converterState.DocumentNodeArray.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnRow)
				{
					num = 0L;
					DocumentNodeArray rowsCells = documentNode.GetRowsCells();
					for (int j = 0; j < rowsCells.Count; j++)
					{
						DocumentNode documentNode2 = rowsCells.EntryAt(j);
						num += (long)(documentNode2.ColSpan * 1440);
					}
				}
				else if (documentNode.Type == DocumentNodeType.dnTable)
				{
					i += documentNode.ChildCount;
				}
				num2 = Math.Max(num2, num);
			}
			return num2;
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x0028ABA4 File Offset: 0x00289BA4
		private void WriteBorder(string borderControlWord, BorderFormat bf)
		{
			this._rtfBuilder.Append(borderControlWord);
			this._rtfBuilder.Append(bf.RTFEncoding);
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x0028ABC8 File Offset: 0x00289BC8
		private void PatchVerticallyMergedCells(DocumentNode dnThis)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			DocumentNodeArray tableRows = dnThis.GetTableRows();
			DocumentNodeArray documentNodeArray2 = new DocumentNodeArray();
			ArrayList arrayList = new ArrayList();
			int num = 0;
			for (int i = 0; i < tableRows.Count; i++)
			{
				DocumentNode documentNode = tableRows.EntryAt(i);
				DocumentNodeArray rowsCells = documentNode.GetRowsCells();
				int j = 0;
				int l;
				for (int k = 0; k < rowsCells.Count; k++)
				{
					DocumentNode documentNode2 = rowsCells.EntryAt(k);
					DocumentNode documentNode4;
					for (l = j; l < arrayList.Count; l += documentNode4.ColSpan)
					{
						if ((int)arrayList[l] <= 0)
						{
							break;
						}
						DocumentNode documentNode3 = documentNodeArray2.EntryAt(l);
						documentNode4 = new DocumentNode(DocumentNodeType.dnCell);
						documentNodeArray.InsertChildAt(documentNode, documentNode4, documentNode2.Index, 0);
						documentNode4.FormatState = new FormatState(documentNode3.FormatState);
						if (documentNode3.FormatState.HasRowFormat)
						{
							documentNode4.FormatState.RowFormat = new RowFormat(documentNode3.FormatState.RowFormat);
						}
						documentNode4.FormatState.RowFormat.RowCellFormat.IsVMergeFirst = false;
						documentNode4.FormatState.RowFormat.RowCellFormat.IsVMerge = true;
						documentNode4.ColSpan = documentNode3.ColSpan;
					}
					while (j < arrayList.Count && (int)arrayList[j] > 0)
					{
						arrayList[j] = (int)arrayList[j] - 1;
						if ((int)arrayList[j] == 0)
						{
							documentNodeArray2[j] = null;
						}
						j++;
					}
					for (int m = 0; m < documentNode2.ColSpan; m++)
					{
						if (j < arrayList.Count)
						{
							arrayList[j] = documentNode2.RowSpan - 1;
							documentNodeArray2[j] = ((documentNode2.RowSpan > 1) ? documentNode2 : null);
						}
						else
						{
							arrayList.Add(documentNode2.RowSpan - 1);
							documentNodeArray2.Add((documentNode2.RowSpan > 1) ? documentNode2 : null);
						}
						j++;
					}
					if (documentNode2.RowSpan > 1)
					{
						documentNode2.FormatState.RowFormat.RowCellFormat.IsVMergeFirst = true;
					}
				}
				l = j;
				while (l < arrayList.Count)
				{
					if ((int)arrayList[l] > 0)
					{
						DocumentNode documentNode5 = documentNodeArray2.EntryAt(l);
						DocumentNode documentNode6 = new DocumentNode(DocumentNodeType.dnCell);
						documentNodeArray.InsertChildAt(documentNode, documentNode6, documentNode.Index + documentNode.ChildCount + 1, 0);
						documentNode6.FormatState = new FormatState(documentNode5.FormatState);
						if (documentNode5.FormatState.HasRowFormat)
						{
							documentNode6.FormatState.RowFormat = new RowFormat(documentNode5.FormatState.RowFormat);
						}
						documentNode6.FormatState.RowFormat.RowCellFormat.IsVMergeFirst = false;
						documentNode6.FormatState.RowFormat.RowCellFormat.IsVMerge = true;
						documentNode6.ColSpan = documentNode5.ColSpan;
						l += documentNode6.ColSpan;
					}
					else
					{
						l++;
					}
				}
				while (j < arrayList.Count)
				{
					if ((int)arrayList[j] > 0)
					{
						arrayList[j] = (int)arrayList[j] - 1;
						if ((int)arrayList[j] == 0)
						{
							documentNodeArray2[j] = null;
						}
					}
					j++;
				}
				if (j > num)
				{
					num = j;
				}
			}
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x0028AF60 File Offset: 0x00289F60
		private void WriteStructure(DocumentNode dnThis)
		{
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			bool flag = dnThis.GetParentOfType(DocumentNodeType.dnCell) != null;
			switch (dnThis.Type)
			{
			case DocumentNodeType.dnInline:
				this.WriteInlineChild(dnThis);
				return;
			case DocumentNodeType.dnLineBreak:
			case DocumentNodeType.dnHyperlink:
			case DocumentNodeType.dnImage:
				return;
			case DocumentNodeType.dnParagraph:
				this.WriteParagraph(dnThis);
				return;
			case DocumentNodeType.dnInlineUIContainer:
			case DocumentNodeType.dnBlockUIContainer:
				this.WriteUIContainerChild(dnThis);
				return;
			case DocumentNodeType.dnList:
			case DocumentNodeType.dnListItem:
			case DocumentNodeType.dnTableBody:
			case DocumentNodeType.dnCell:
				break;
			case DocumentNodeType.dnTable:
				if (dnThis.FormatState.HasRowFormat)
				{
					dnThis.FormatState.RowFormat.Trleft = dnThis.FormatState.LI;
					dnThis.FormatState.RowFormat.CanonicalizeWidthsFromXaml();
				}
				this.PatchVerticallyMergedCells(dnThis);
				break;
			case DocumentNodeType.dnRow:
				this.WriteRow(dnThis);
				break;
			case DocumentNodeType.dnSection:
				this.WriteSection(dnThis);
				return;
			default:
				return;
			}
			if (dnThis.Type != DocumentNodeType.dnRow)
			{
				int index = dnThis.Index;
				for (int i = index + 1; i <= index + dnThis.ChildCount; i++)
				{
					DocumentNode documentNode = documentNodeArray.EntryAt(i);
					if (documentNode.Parent == dnThis)
					{
						this.WriteStructure(documentNode);
					}
				}
			}
			switch (dnThis.Type)
			{
			case DocumentNodeType.dnList:
			case DocumentNodeType.dnListItem:
			case DocumentNodeType.dnTable:
			case DocumentNodeType.dnTableBody:
			case DocumentNodeType.dnRow:
				break;
			case DocumentNodeType.dnCell:
				if (!dnThis.IsTerminated)
				{
					this._rtfBuilder.Append(flag ? "\\nestcell" : "\\cell");
					this._rtfBuilder.Append("\r\n");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x0028B0DC File Offset: 0x0028A0DC
		private void WriteDocumentContents()
		{
			this._rtfBuilder.Append("\\loch\\hich\\dbch\\pard\\plain\\ltrpar\\itap0");
			DocumentNodeArray documentNodeArray = this._converterState.DocumentNodeArray;
			DocumentNode documentNode;
			for (int i = 0; i < documentNodeArray.Count; i += documentNode.ChildCount + 1)
			{
				documentNode = documentNodeArray.EntryAt(i);
				this.WriteStructure(documentNode);
			}
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x0028B12D File Offset: 0x0028A12D
		private void WriteEpilog()
		{
			this._rtfBuilder.Append("}");
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x0028B140 File Offset: 0x0028A140
		private void WriteOutput()
		{
			this.BuildListTable();
			this.SetParagraphStructureProperties();
			this.MergeParagraphMargins();
			this.GenerateListLabels();
			this.WriteProlog();
			this.WriteHeaderTables();
			this.WriteDocumentContents();
			this.WriteEpilog();
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x0028B174 File Offset: 0x0028A174
		private void WriteImage(DocumentNode documentNode)
		{
			if (this._wpfPayload == null)
			{
				return;
			}
			using (Stream imageStream = this._wpfPayload.GetImageStream(documentNode.FormatState.ImageSource))
			{
				RtfImageFormat imageFormatFromImageSourceName = this.GetImageFormatFromImageSourceName(documentNode.FormatState.ImageSource);
				this.WriteShapeImage(documentNode, imageStream, imageFormatFromImageSourceName);
			}
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x0028B1D8 File Offset: 0x0028A1D8
		private void WriteShapeImage(DocumentNode documentNode, Stream imageStream, RtfImageFormat imageFormat)
		{
			this._rtfBuilder.Append("{\\*\\shppict{\\pict");
			Size availableSize = new Size(documentNode.FormatState.ImageWidth, documentNode.FormatState.ImageHeight);
			BitmapSource bitmapSource = BitmapFrame.Create(imageStream);
			Size contentSize;
			if (bitmapSource != null)
			{
				contentSize = new Size(bitmapSource.Width, bitmapSource.Height);
			}
			else
			{
				contentSize = new Size(availableSize.Width, availableSize.Height);
			}
			Stretch imageStretch = this.GetImageStretch(documentNode.FormatState.ImageStretch);
			StretchDirection imageStretchDirection = this.GetImageStretchDirection(documentNode.FormatState.ImageStretchDirection);
			if (availableSize.Width == 0.0)
			{
				if (availableSize.Height == 0.0)
				{
					availableSize.Width = contentSize.Width;
				}
				else
				{
					availableSize.Width = contentSize.Width * (availableSize.Height / contentSize.Height);
				}
			}
			if (availableSize.Height == 0.0)
			{
				if (availableSize.Width == 0.0)
				{
					availableSize.Height = contentSize.Height;
				}
				else
				{
					availableSize.Height = contentSize.Height * (availableSize.Width / contentSize.Width);
				}
			}
			Size size = Viewbox.ComputeScaleFactor(availableSize, contentSize, imageStretch, imageStretchDirection);
			if (documentNode.FormatState.IncludeImageBaselineOffset)
			{
				this._rtfBuilder.Append("\\dn");
				this._rtfBuilder.Append(Converters.PxToHalfPointRounded(contentSize.Height * size.Height - documentNode.FormatState.ImageBaselineOffset));
			}
			this._rtfBuilder.Append("\\picwgoal");
			this._rtfBuilder.Append(Converters.PxToTwipRounded(contentSize.Width * size.Width).ToString(CultureInfo.InvariantCulture));
			this._rtfBuilder.Append("\\pichgoal");
			this._rtfBuilder.Append(Converters.PxToTwipRounded(contentSize.Height * size.Height).ToString(CultureInfo.InvariantCulture));
			switch (imageFormat)
			{
			case RtfImageFormat.Bmp:
			case RtfImageFormat.Dib:
			case RtfImageFormat.Gif:
			case RtfImageFormat.Png:
			case RtfImageFormat.Tif:
				this._rtfBuilder.Append("\\pngblip");
				break;
			case RtfImageFormat.Jpeg:
				this._rtfBuilder.Append("\\jpegblip");
				break;
			}
			this._rtfBuilder.Append("\r\n");
			if (imageFormat != RtfImageFormat.Unknown)
			{
				string value = this.ConvertToImageHexDataString(imageStream);
				this._rtfBuilder.Append(value);
			}
			this._rtfBuilder.Append("}}");
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x0028B470 File Offset: 0x0028A470
		private string ConvertToImageHexDataString(Stream imageStream)
		{
			byte[] array = new byte[imageStream.Length * 2L];
			imageStream.Position = 0L;
			int num = 0;
			while ((long)num < imageStream.Length)
			{
				Converters.ByteToHex((byte)imageStream.ReadByte(), out array[num * 2], out array[num * 2 + 1]);
				num++;
			}
			return InternalEncoding.GetEncoding(1252).GetString(array);
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x0028B4D8 File Offset: 0x0028A4D8
		private RtfImageFormat GetImageFormatFromImageSourceName(string imageName)
		{
			RtfImageFormat result = RtfImageFormat.Unknown;
			int num = imageName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
			if (num >= 0)
			{
				string strB = imageName.Substring(num);
				if (string.Compare(".png", strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = RtfImageFormat.Png;
				}
				if (string.Compare(".jpeg", strB, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(".jpg", strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = RtfImageFormat.Jpeg;
				}
				if (string.Compare(".gif", strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = RtfImageFormat.Gif;
				}
				if (string.Compare(".tif", strB, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(".tiff", strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = RtfImageFormat.Tif;
				}
				if (string.Compare(".bmp", strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = RtfImageFormat.Bmp;
				}
				if (string.Compare(".dib", strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = RtfImageFormat.Dib;
				}
			}
			return result;
		}

		// Token: 0x06005CE3 RID: 23779 RVA: 0x0028B580 File Offset: 0x0028A580
		private Stretch GetImageStretch(string imageStretch)
		{
			if (string.Compare("Fill", imageStretch, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return Stretch.Fill;
			}
			if (string.Compare("UniformToFill", imageStretch, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return Stretch.UniformToFill;
			}
			return Stretch.Uniform;
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x0028B5A3 File Offset: 0x0028A5A3
		private StretchDirection GetImageStretchDirection(string imageStretchDirection)
		{
			if (string.Compare("UpOnly", imageStretchDirection, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return StretchDirection.UpOnly;
			}
			if (string.Compare("DownOnly", imageStretchDirection, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return StretchDirection.DownOnly;
			}
			return StretchDirection.Both;
		}

		// Token: 0x0400312D RID: 12589
		private string _xaml;

		// Token: 0x0400312E RID: 12590
		private StringBuilder _rtfBuilder;

		// Token: 0x0400312F RID: 12591
		private ConverterState _converterState;

		// Token: 0x04003130 RID: 12592
		private XamlToRtfWriter.XamlIn _xamlIn;

		// Token: 0x04003131 RID: 12593
		private WpfPayload _wpfPayload;

		// Token: 0x04003132 RID: 12594
		private const int DefaultCellXAsTwips = 1440;

		// Token: 0x02000B89 RID: 2953
		internal enum XamlTag
		{
			// Token: 0x0400492D RID: 18733
			XTUnknown,
			// Token: 0x0400492E RID: 18734
			XTBold,
			// Token: 0x0400492F RID: 18735
			XTItalic,
			// Token: 0x04004930 RID: 18736
			XTUnderline,
			// Token: 0x04004931 RID: 18737
			XTHyperlink,
			// Token: 0x04004932 RID: 18738
			XTInline,
			// Token: 0x04004933 RID: 18739
			XTLineBreak,
			// Token: 0x04004934 RID: 18740
			XTParagraph,
			// Token: 0x04004935 RID: 18741
			XTInlineUIContainer,
			// Token: 0x04004936 RID: 18742
			XTBlockUIContainer,
			// Token: 0x04004937 RID: 18743
			XTImage,
			// Token: 0x04004938 RID: 18744
			XTBitmapImage,
			// Token: 0x04004939 RID: 18745
			XTList,
			// Token: 0x0400493A RID: 18746
			XTListItem,
			// Token: 0x0400493B RID: 18747
			XTTable,
			// Token: 0x0400493C RID: 18748
			XTTableBody,
			// Token: 0x0400493D RID: 18749
			XTTableRow,
			// Token: 0x0400493E RID: 18750
			XTTableCell,
			// Token: 0x0400493F RID: 18751
			XTTableColumn,
			// Token: 0x04004940 RID: 18752
			XTSection,
			// Token: 0x04004941 RID: 18753
			XTFloater,
			// Token: 0x04004942 RID: 18754
			XTFigure,
			// Token: 0x04004943 RID: 18755
			XTTextDecoration
		}

		// Token: 0x02000B8A RID: 2954
		internal class XamlIn : IXamlContentHandler, IXamlErrorHandler
		{
			// Token: 0x06008E6B RID: 36459 RVA: 0x00341352 File Offset: 0x00340352
			internal XamlIn(XamlToRtfWriter writer, string xaml)
			{
				this._writer = writer;
				this._xaml = xaml;
				this._parser = new XamlToRtfParser(this._xaml);
				this._parser.SetCallbacks(this, this);
				this._bGenListTables = true;
			}

			// Token: 0x17001F2A RID: 7978
			// (get) Token: 0x06008E6C RID: 36460 RVA: 0x0034138D File Offset: 0x0034038D
			internal bool GenerateListTables
			{
				get
				{
					return this._bGenListTables;
				}
			}

			// Token: 0x06008E6D RID: 36461 RVA: 0x00341395 File Offset: 0x00340395
			internal XamlToRtfError Parse()
			{
				return this._parser.Parse();
			}

			// Token: 0x06008E6E RID: 36462 RVA: 0x003413A4 File Offset: 0x003403A4
			XamlToRtfError IXamlContentHandler.Characters(string characters)
			{
				XamlToRtfError xamlToRtfError = XamlToRtfError.None;
				ConverterState converterState = this._writer.ConverterState;
				DocumentNodeArray documentNodeArray = converterState.DocumentNodeArray;
				DocumentNode documentNode = documentNodeArray.TopPending();
				int num = 0;
				while (xamlToRtfError == XamlToRtfError.None && num < characters.Length)
				{
					while (num < characters.Length && this.IsNewLine(characters[num]))
					{
						num++;
					}
					int num2 = num;
					while (num2 < characters.Length && !this.IsNewLine(characters[num2]))
					{
						num2++;
					}
					if (num != num2)
					{
						string s = characters.Substring(num, num2 - num);
						DocumentNode documentNode2 = new DocumentNode(DocumentNodeType.dnText);
						if (documentNode != null)
						{
							documentNode2.InheritFormatState(documentNode.FormatState);
						}
						documentNodeArray.Push(documentNode2);
						documentNode2.IsPending = false;
						if (xamlToRtfError == XamlToRtfError.None)
						{
							FontTableEntry fontTableEntry = converterState.FontTable.FindEntryByIndex((int)documentNode2.FormatState.Font);
							int cp = (fontTableEntry == null) ? 1252 : fontTableEntry.CodePage;
							XamlToRtfWriter.XamlParserHelper.AppendRTFText(documentNode2.Content, s, cp);
						}
					}
					num = num2;
				}
				return xamlToRtfError;
			}

			// Token: 0x06008E6F RID: 36463 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlContentHandler.StartDocument()
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E70 RID: 36464 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlContentHandler.EndDocument()
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E71 RID: 36465 RVA: 0x003414B0 File Offset: 0x003404B0
			XamlToRtfError IXamlContentHandler.StartElement(string nameSpaceUri, string localName, string qName, IXamlAttributes attributes)
			{
				XamlToRtfError xamlToRtfError = XamlToRtfError.None;
				ConverterState converterState = this._writer.ConverterState;
				DocumentNodeArray documentNodeArray = converterState.DocumentNodeArray;
				DocumentNodeType documentNodeType = DocumentNodeType.dnUnknown;
				DocumentNode documentNode = documentNodeArray.TopPending();
				DocumentNode documentNode2 = null;
				XamlToRtfWriter.XamlTag xamlTag = XamlToRtfWriter.XamlTag.XTUnknown;
				bool flag = true;
				if (!XamlToRtfWriter.XamlParserHelper.ConvertToTag(converterState, localName, ref xamlTag))
				{
					return xamlToRtfError;
				}
				if (xamlTag == XamlToRtfWriter.XamlTag.XTTextDecoration || xamlTag == XamlToRtfWriter.XamlTag.XTTableColumn || xamlTag == XamlToRtfWriter.XamlTag.XTBitmapImage)
				{
					if (documentNode == null)
					{
						return xamlToRtfError;
					}
					documentNode2 = documentNode;
					flag = false;
				}
				if (flag)
				{
					if (!XamlToRtfWriter.XamlParserHelper.ConvertTagToNodeType(xamlTag, ref documentNodeType))
					{
						return xamlToRtfError;
					}
					documentNode2 = this.CreateDocumentNode(converterState, documentNodeType, documentNode, xamlTag);
				}
				if (attributes != null && documentNode2 != null)
				{
					xamlToRtfError = this.HandleAttributes(converterState, attributes, documentNode2, xamlTag, documentNodeArray);
				}
				if (xamlToRtfError == XamlToRtfError.None && documentNode2 != null && flag)
				{
					if (!documentNode2.IsInline)
					{
						XamlToRtfWriter.XamlParserHelper.EnsureParagraphClosed(converterState);
					}
					documentNodeArray.Push(documentNode2);
				}
				return xamlToRtfError;
			}

			// Token: 0x06008E72 RID: 36466 RVA: 0x00341570 File Offset: 0x00340570
			XamlToRtfError IXamlContentHandler.EndElement(string nameSpaceUri, string localName, string qName)
			{
				XamlToRtfError result = XamlToRtfError.None;
				ConverterState converterState = this._writer.ConverterState;
				XamlToRtfWriter.XamlTag xamlTag = XamlToRtfWriter.XamlTag.XTUnknown;
				if (!XamlToRtfWriter.XamlParserHelper.ConvertToTag(converterState, localName, ref xamlTag))
				{
					return result;
				}
				DocumentNodeType documentNodeType = DocumentNodeType.dnUnknown;
				if (!XamlToRtfWriter.XamlParserHelper.ConvertTagToNodeType(xamlTag, ref documentNodeType))
				{
					return result;
				}
				DocumentNodeArray documentNodeArray = converterState.DocumentNodeArray;
				int num = documentNodeArray.FindPending(documentNodeType);
				if (num >= 0)
				{
					DocumentNode documentNode = documentNodeArray.EntryAt(num);
					if (documentNodeType != DocumentNodeType.dnParagraph && !documentNode.IsInline)
					{
						XamlToRtfWriter.XamlParserHelper.EnsureParagraphClosed(converterState);
					}
					documentNodeArray.CloseAt(num);
				}
				return result;
			}

			// Token: 0x06008E73 RID: 36467 RVA: 0x003415E8 File Offset: 0x003405E8
			XamlToRtfError IXamlContentHandler.IgnorableWhitespace(string xaml)
			{
				XamlToRtfError xamlToRtfError = XamlToRtfError.None;
				ConverterState converterState = this._writer.ConverterState;
				if (converterState.DocumentNodeArray.FindPending(DocumentNodeType.dnParagraph) >= 0 || converterState.DocumentNodeArray.FindPending(DocumentNodeType.dnInline) >= 0)
				{
					int j;
					int num2;
					for (int i = 0; i < xaml.Length; i = ((j == xaml.Length) ? j : num2))
					{
						int num = i;
						j = i;
						num2 = -1;
						while (j < xaml.Length)
						{
							if (xaml[j] == '\r' || xaml[j] == '\n')
							{
								if (xaml[j] == '\r' && j + 1 < xaml.Length && xaml[j + 1] == '\n')
								{
									num2 = j + 2;
								}
								else
								{
									num2 = j + 1;
								}
							}
							j++;
						}
						if (num == 0 && j == xaml.Length)
						{
							return ((IXamlContentHandler)this).Characters(xaml);
						}
						if (j != num)
						{
							string characters = xaml.Substring(num, j - num);
							xamlToRtfError = ((IXamlContentHandler)this).Characters(characters);
							if (xamlToRtfError != XamlToRtfError.None)
							{
								return xamlToRtfError;
							}
						}
						xamlToRtfError = ((IXamlContentHandler)this).StartElement(null, "LineBreak", null, null);
						if (xamlToRtfError != XamlToRtfError.None)
						{
							return xamlToRtfError;
						}
						xamlToRtfError = ((IXamlContentHandler)this).EndElement(null, "LineBreak", null);
						if (xamlToRtfError != XamlToRtfError.None)
						{
							return xamlToRtfError;
						}
					}
					return ((IXamlContentHandler)this).Characters(xaml);
				}
				return xamlToRtfError;
			}

			// Token: 0x06008E74 RID: 36468 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlContentHandler.StartPrefixMapping(string prefix, string uri)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E75 RID: 36469 RVA: 0x00105F35 File Offset: 0x00104F35
			XamlToRtfError IXamlContentHandler.ProcessingInstruction(string target, string data)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008E76 RID: 36470 RVA: 0x00341714 File Offset: 0x00340714
			XamlToRtfError IXamlContentHandler.SkippedEntity(string name)
			{
				XamlToRtfError result = XamlToRtfError.None;
				if (string.Compare(name, "&gt;", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return ((IXamlContentHandler)this).Characters(">");
				}
				if (string.Compare(name, "&lt;", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return ((IXamlContentHandler)this).Characters("<");
				}
				if (string.Compare(name, "&amp;", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return ((IXamlContentHandler)this).Characters("&");
				}
				if (name.IndexOf("&#x", StringComparison.OrdinalIgnoreCase) == 0)
				{
					result = XamlToRtfError.InvalidFormat;
					if (name.Length >= 5)
					{
						string s = name.Substring(3, name.Length - 4);
						int num = 0;
						Converters.HexStringToInt(s, ref num);
						if (num >= 0 && num <= 65535)
						{
							string characters = new string(new char[]
							{
								(char)num
							});
							return ((IXamlContentHandler)this).Characters(characters);
						}
					}
				}
				else if (name.IndexOf("&#", StringComparison.OrdinalIgnoreCase) == 0 && name.Length >= 4)
				{
					string s2 = name.Substring(2, name.Length - 3);
					int num2 = 0;
					Converters.StringToInt(s2, ref num2);
					if (num2 >= 0 && num2 <= 65535)
					{
						string characters2 = new string(new char[]
						{
							(char)num2
						});
						return ((IXamlContentHandler)this).Characters(characters2);
					}
				}
				return result;
			}

			// Token: 0x06008E77 RID: 36471 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			void IXamlErrorHandler.Error(string message, XamlToRtfError xamlToRtfError)
			{
			}

			// Token: 0x06008E78 RID: 36472 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			void IXamlErrorHandler.FatalError(string message, XamlToRtfError xamlToRtfError)
			{
			}

			// Token: 0x06008E79 RID: 36473 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			void IXamlErrorHandler.IgnorableWarning(string message, XamlToRtfError xamlToRtfError)
			{
			}

			// Token: 0x06008E7A RID: 36474 RVA: 0x00341823 File Offset: 0x00340823
			private bool IsNewLine(char character)
			{
				return character == '\r' || character == '\n';
			}

			// Token: 0x06008E7B RID: 36475 RVA: 0x00341834 File Offset: 0x00340834
			private DocumentNode CreateDocumentNode(ConverterState converterState, DocumentNodeType documentNodeType, DocumentNode dnTop, XamlToRtfWriter.XamlTag xamlTag)
			{
				DocumentNode documentNode = new DocumentNode(documentNodeType);
				if (dnTop != null)
				{
					documentNode.InheritFormatState(dnTop.FormatState);
				}
				switch (xamlTag)
				{
				case XamlToRtfWriter.XamlTag.XTBold:
					documentNode.FormatState.Bold = true;
					break;
				case XamlToRtfWriter.XamlTag.XTItalic:
					documentNode.FormatState.Italic = true;
					break;
				case XamlToRtfWriter.XamlTag.XTUnderline:
					documentNode.FormatState.UL = ULState.ULNormal;
					break;
				case XamlToRtfWriter.XamlTag.XTHyperlink:
				{
					long cf = 0L;
					documentNode.FormatState.UL = ULState.ULNormal;
					if (XamlToRtfWriter.XamlParserHelper.ConvertToColor(converterState, "#FF0000FF", ref cf))
					{
						documentNode.FormatState.CF = cf;
					}
					break;
				}
				default:
					if (xamlTag == XamlToRtfWriter.XamlTag.XTList)
					{
						documentNode.FormatState.Marker = MarkerStyle.MarkerBullet;
						documentNode.FormatState.StartIndex = 1L;
						documentNode.FormatState.LI = 720L;
					}
					break;
				}
				return documentNode;
			}

			// Token: 0x06008E7C RID: 36476 RVA: 0x00341900 File Offset: 0x00340900
			private XamlToRtfError HandleAttributes(ConverterState converterState, IXamlAttributes attributes, DocumentNode documentNode, XamlToRtfWriter.XamlTag xamlTag, DocumentNodeArray dna)
			{
				int num = 0;
				XamlToRtfError xamlToRtfError = attributes.GetLength(ref num);
				if (xamlToRtfError == XamlToRtfError.None)
				{
					string empty = string.Empty;
					string empty2 = string.Empty;
					string empty3 = string.Empty;
					string empty4 = string.Empty;
					FormatState formatState = documentNode.FormatState;
					XamlAttribute xamlAttribute = XamlAttribute.XAUnknown;
					long num2 = 0L;
					int num3 = 0;
					while (xamlToRtfError == XamlToRtfError.None && num3 < num)
					{
						xamlToRtfError = attributes.GetName(num3, ref empty, ref empty2, ref empty3);
						if (xamlToRtfError == XamlToRtfError.None)
						{
							xamlToRtfError = attributes.GetValue(num3, ref empty4);
							if (xamlToRtfError == XamlToRtfError.None && XamlToRtfWriter.XamlParserHelper.ConvertToAttribute(converterState, empty2, ref xamlAttribute))
							{
								switch (xamlAttribute)
								{
								case XamlAttribute.XAUnknown:
								case XamlAttribute.XAMarkerOffset:
								case XamlAttribute.XAKeepTogether:
								case XamlAttribute.XAKeepWithNext:
								case XamlAttribute.XABaselineAlignment:
								case XamlAttribute.XATargetName:
									goto IL_993;
								case XamlAttribute.XAFontWeight:
									if (string.Compare(empty4, "Normal", StringComparison.OrdinalIgnoreCase) == 0)
									{
										formatState.Bold = false;
										goto IL_993;
									}
									if (string.Compare(empty4, "Bold", StringComparison.OrdinalIgnoreCase) == 0)
									{
										formatState.Bold = true;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XAFontSize:
								{
									double a = 0.0;
									if (XamlToRtfWriter.XamlParserHelper.ConvertToFontSize(converterState, empty4, ref a))
									{
										formatState.FontSize = (long)Math.Round(a);
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XAFontStyle:
									if (string.Compare(empty4, "Italic", StringComparison.OrdinalIgnoreCase) == 0)
									{
										formatState.Italic = true;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XAFontFamily:
									if (XamlToRtfWriter.XamlParserHelper.ConvertToFont(converterState, empty4, ref num2))
									{
										formatState.Font = num2;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XAFontStretch:
									if (XamlToRtfWriter.XamlParserHelper.ConvertToFontStretch(converterState, empty4, ref num2))
									{
										formatState.Expand = num2;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XABackground:
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToColor(converterState, empty4, ref num2))
									{
										goto IL_993;
									}
									if (documentNode.IsInline)
									{
										formatState.CB = num2;
										goto IL_993;
									}
									formatState.CBPara = num2;
									goto IL_993;
								case XamlAttribute.XAForeground:
									if (XamlToRtfWriter.XamlParserHelper.ConvertToColor(converterState, empty4, ref num2))
									{
										formatState.CF = num2;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XAFlowDirection:
								{
									DirState dirState = DirState.DirDefault;
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToDir(converterState, empty4, ref dirState))
									{
										goto IL_993;
									}
									if (documentNode.IsInline)
									{
										formatState.DirChar = dirState;
									}
									else if (documentNode.Type == DocumentNodeType.dnTable)
									{
										formatState.RowFormat.Dir = dirState;
									}
									else
									{
										formatState.DirPara = dirState;
										formatState.DirChar = dirState;
									}
									if (documentNode.Type == DocumentNodeType.dnList && formatState.DirPara == DirState.DirRTL)
									{
										formatState.LI = 0L;
										formatState.RI = 720L;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XATextDecorations:
								{
									ULState ulstate = ULState.ULNormal;
									StrikeState strikeState = StrikeState.StrikeNormal;
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToDecoration(converterState, empty4, ref ulstate, ref strikeState))
									{
										goto IL_993;
									}
									if (ulstate != ULState.ULNone)
									{
										formatState.UL = ulstate;
									}
									if (strikeState != StrikeState.StrikeNone)
									{
										formatState.Strike = strikeState;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XATextAlignment:
									break;
								case XamlAttribute.XAMarkerStyle:
								{
									MarkerStyle marker = MarkerStyle.MarkerBullet;
									if (XamlToRtfWriter.XamlParserHelper.ConvertToMarkerStyle(converterState, empty4, ref marker))
									{
										formatState.Marker = marker;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XATextIndent:
								{
									double px = 0.0;
									if (XamlToRtfWriter.XamlParserHelper.ConvertToTextIndent(converterState, empty4, ref px))
									{
										formatState.FI = Converters.PxToTwipRounded(px);
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XAColumnSpan:
								{
									int colSpan = 0;
									if (Converters.StringToInt(empty4, ref colSpan) && documentNode.Type == DocumentNodeType.dnCell)
									{
										documentNode.ColSpan = colSpan;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XARowSpan:
								{
									int rowSpan = 0;
									if (Converters.StringToInt(empty4, ref rowSpan) && documentNode.Type == DocumentNodeType.dnCell)
									{
										documentNode.RowSpan = rowSpan;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XAStartIndex:
								{
									int num4 = 0;
									if (XamlToRtfWriter.XamlParserHelper.ConvertToStartIndex(converterState, empty4, ref num4))
									{
										formatState.StartIndex = (long)num4;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XABorderThickness:
								{
									XamlToRtfWriter.XamlThickness xamlThickness = new XamlToRtfWriter.XamlThickness(0f, 0f, 0f, 0f);
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToThickness(converterState, empty4, ref xamlThickness))
									{
										goto IL_993;
									}
									if (xamlTag == XamlToRtfWriter.XamlTag.XTParagraph)
									{
										ParaBorder paraBorder = formatState.ParaBorder;
										paraBorder.BorderLeft.Type = BorderType.BorderSingle;
										paraBorder.BorderLeft.Width = Converters.PxToTwipRounded((double)xamlThickness.Left);
										paraBorder.BorderRight.Type = BorderType.BorderSingle;
										paraBorder.BorderRight.Width = Converters.PxToTwipRounded((double)xamlThickness.Right);
										paraBorder.BorderTop.Type = BorderType.BorderSingle;
										paraBorder.BorderTop.Width = Converters.PxToTwipRounded((double)xamlThickness.Top);
										paraBorder.BorderBottom.Type = BorderType.BorderSingle;
										paraBorder.BorderBottom.Width = Converters.PxToTwipRounded((double)xamlThickness.Bottom);
										goto IL_993;
									}
									CellFormat rowCellFormat = formatState.RowFormat.RowCellFormat;
									rowCellFormat.BorderLeft.Type = BorderType.BorderSingle;
									rowCellFormat.BorderLeft.Width = Converters.PxToTwipRounded((double)xamlThickness.Left);
									rowCellFormat.BorderRight.Type = BorderType.BorderSingle;
									rowCellFormat.BorderRight.Width = Converters.PxToTwipRounded((double)xamlThickness.Right);
									rowCellFormat.BorderTop.Type = BorderType.BorderSingle;
									rowCellFormat.BorderTop.Width = Converters.PxToTwipRounded((double)xamlThickness.Top);
									rowCellFormat.BorderBottom.Type = BorderType.BorderSingle;
									rowCellFormat.BorderBottom.Width = Converters.PxToTwipRounded((double)xamlThickness.Bottom);
									goto IL_993;
								}
								case XamlAttribute.XABorderBrush:
								{
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToColor(converterState, empty4, ref num2))
									{
										goto IL_993;
									}
									if (xamlTag == XamlToRtfWriter.XamlTag.XTParagraph)
									{
										formatState.ParaBorder.CF = num2;
										goto IL_993;
									}
									CellFormat rowCellFormat2 = formatState.RowFormat.RowCellFormat;
									rowCellFormat2.BorderLeft.CF = num2;
									rowCellFormat2.BorderRight.CF = num2;
									rowCellFormat2.BorderTop.CF = num2;
									rowCellFormat2.BorderBottom.CF = num2;
									goto IL_993;
								}
								case XamlAttribute.XAPadding:
								{
									XamlToRtfWriter.XamlThickness xamlThickness2 = new XamlToRtfWriter.XamlThickness(0f, 0f, 0f, 0f);
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToThickness(converterState, empty4, ref xamlThickness2))
									{
										goto IL_993;
									}
									if (xamlTag == XamlToRtfWriter.XamlTag.XTParagraph)
									{
										formatState.ParaBorder.Spacing = Converters.PxToTwipRounded((double)xamlThickness2.Left);
										goto IL_993;
									}
									CellFormat rowCellFormat3 = formatState.RowFormat.RowCellFormat;
									rowCellFormat3.PaddingLeft = Converters.PxToTwipRounded((double)xamlThickness2.Left);
									rowCellFormat3.PaddingRight = Converters.PxToTwipRounded((double)xamlThickness2.Right);
									rowCellFormat3.PaddingTop = Converters.PxToTwipRounded((double)xamlThickness2.Top);
									rowCellFormat3.PaddingBottom = Converters.PxToTwipRounded((double)xamlThickness2.Bottom);
									goto IL_993;
								}
								case XamlAttribute.XAMargin:
								{
									XamlToRtfWriter.XamlThickness xamlThickness3 = new XamlToRtfWriter.XamlThickness(0f, 0f, 0f, 0f);
									if (XamlToRtfWriter.XamlParserHelper.ConvertToThickness(converterState, empty4, ref xamlThickness3))
									{
										formatState.LI = Converters.PxToTwipRounded((double)xamlThickness3.Left);
										formatState.RI = Converters.PxToTwipRounded((double)xamlThickness3.Right);
										formatState.SB = Converters.PxToTwipRounded((double)xamlThickness3.Top);
										formatState.SA = Converters.PxToTwipRounded((double)xamlThickness3.Bottom);
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XABaselineOffset:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTImage)
									{
										double imageBaselineOffset = 0.0;
										Converters.StringToDouble(empty4, ref imageBaselineOffset);
										documentNode.FormatState.ImageBaselineOffset = imageBaselineOffset;
										documentNode.FormatState.IncludeImageBaselineOffset = true;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XANavigateUri:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTHyperlink && empty4.Length > 0)
									{
										StringBuilder stringBuilder = new StringBuilder();
										XamlToRtfWriter.XamlParserHelper.AppendRTFText(stringBuilder, empty4, 0);
										documentNode.NavigateUri = stringBuilder.ToString();
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XALineHeight:
								{
									double px2 = 0.0;
									if (XamlToRtfWriter.XamlParserHelper.ConvertToLineHeight(converterState, empty4, ref px2))
									{
										formatState.SL = Converters.PxToTwipRounded(px2);
										formatState.SLMult = false;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XALocation:
								{
									ULState ulstate2 = ULState.ULNormal;
									StrikeState strikeState2 = StrikeState.StrikeNormal;
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToDecoration(converterState, empty4, ref ulstate2, ref strikeState2))
									{
										goto IL_993;
									}
									if (ulstate2 != ULState.ULNone)
									{
										formatState.UL = ulstate2;
									}
									if (strikeState2 != StrikeState.StrikeNone)
									{
										formatState.Strike = strikeState2;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XAWidth:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTTableColumn)
									{
										double px3 = 0.0;
										if (!Converters.StringToDouble(empty4, ref px3))
										{
											goto IL_993;
										}
										int num5 = dna.FindPending(DocumentNodeType.dnTable);
										if (num5 >= 0)
										{
											CellFormat cellFormat = dna.EntryAt(num5).FormatState.RowFormat.NextCellFormat();
											cellFormat.Width.Type = WidthType.WidthTwips;
											cellFormat.Width.Value = Converters.PxToTwipRounded(px3);
											goto IL_993;
										}
										goto IL_993;
									}
									else
									{
										if (xamlTag == XamlToRtfWriter.XamlTag.XTImage)
										{
											double imageWidth = 0.0;
											Converters.StringToDouble(empty4, ref imageWidth);
											documentNode.FormatState.ImageWidth = imageWidth;
											goto IL_993;
										}
										goto IL_993;
									}
									break;
								case XamlAttribute.XAHeight:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTImage)
									{
										double imageHeight = 0.0;
										Converters.StringToDouble(empty4, ref imageHeight);
										documentNode.FormatState.ImageHeight = imageHeight;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XASource:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTImage)
									{
										documentNode.FormatState.ImageSource = empty4;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XAUriSource:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTBitmapImage)
									{
										documentNode.FormatState.ImageSource = empty4;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XAStretch:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTImage)
									{
										documentNode.FormatState.ImageStretch = empty4;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XAStretchDirection:
									if (xamlTag == XamlToRtfWriter.XamlTag.XTImage)
									{
										documentNode.FormatState.ImageStretchDirection = empty4;
										goto IL_993;
									}
									goto IL_993;
								case XamlAttribute.XACellSpacing:
								{
									double px4 = 0.0;
									if (Converters.StringToDouble(empty4, ref px4) && documentNode.Type == DocumentNodeType.dnTable)
									{
										formatState.RowFormat.Trgaph = Converters.PxToTwipRounded(px4);
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XATypographyVariants:
								{
									RtfSuperSubscript rtfSuperSubscript = RtfSuperSubscript.None;
									if (!XamlToRtfWriter.XamlParserHelper.ConvertToSuperSub(converterState, empty4, ref rtfSuperSubscript))
									{
										goto IL_993;
									}
									if (rtfSuperSubscript == RtfSuperSubscript.Super)
									{
										formatState.Super = true;
										goto IL_993;
									}
									if (rtfSuperSubscript == RtfSuperSubscript.Sub)
									{
										formatState.Sub = true;
										goto IL_993;
									}
									if (rtfSuperSubscript == RtfSuperSubscript.Normal)
									{
										formatState.Sub = false;
										formatState.Super = false;
										goto IL_993;
									}
									goto IL_993;
								}
								case XamlAttribute.XALang:
									try
									{
										CultureInfo cultureInfo = new CultureInfo(empty4);
										if (cultureInfo.LCID > 0)
										{
											formatState.Lang = (long)((ulong)((ushort)cultureInfo.LCID));
										}
										goto IL_993;
									}
									catch (ArgumentException)
									{
										goto IL_993;
									}
									break;
								default:
									goto IL_993;
								}
								HAlign halign = HAlign.AlignDefault;
								if (XamlToRtfWriter.XamlParserHelper.ConvertToHAlign(converterState, empty4, ref halign))
								{
									formatState.HAlign = halign;
								}
							}
						}
						IL_993:
						num3++;
					}
				}
				return xamlToRtfError;
			}

			// Token: 0x04004944 RID: 18756
			private string _xaml;

			// Token: 0x04004945 RID: 18757
			private XamlToRtfWriter _writer;

			// Token: 0x04004946 RID: 18758
			private XamlToRtfParser _parser;

			// Token: 0x04004947 RID: 18759
			private bool _bGenListTables;
		}

		// Token: 0x02000B8B RID: 2955
		internal static class XamlParserHelper
		{
			// Token: 0x06008E7D RID: 36477 RVA: 0x003422C4 File Offset: 0x003412C4
			internal static int BasicLookup(XamlToRtfWriter.XamlParserHelper.LookupTableEntry[] entries, string name)
			{
				for (int i = 0; i < entries.Length; i++)
				{
					if (string.Compare(entries[i].Name, name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return entries[i].Value;
					}
				}
				return 0;
			}

			// Token: 0x06008E7E RID: 36478 RVA: 0x00342302 File Offset: 0x00341302
			internal static bool ConvertToTag(ConverterState converterState, string tagName, ref XamlToRtfWriter.XamlTag xamlTag)
			{
				if (tagName.Length == 0)
				{
					return false;
				}
				xamlTag = (XamlToRtfWriter.XamlTag)XamlToRtfWriter.XamlParserHelper.BasicLookup(XamlToRtfWriter.XamlParserHelper.TagTable, tagName);
				return xamlTag > XamlToRtfWriter.XamlTag.XTUnknown;
			}

			// Token: 0x06008E7F RID: 36479 RVA: 0x00342320 File Offset: 0x00341320
			internal static bool ConvertToSuperSub(ConverterState converterState, string s, ref RtfSuperSubscript ss)
			{
				if (s.Length == 0)
				{
					return false;
				}
				ss = (RtfSuperSubscript)XamlToRtfWriter.XamlParserHelper.BasicLookup(XamlToRtfWriter.XamlParserHelper.TypographyVariantsTable, s);
				return ss > RtfSuperSubscript.None;
			}

			// Token: 0x06008E80 RID: 36480 RVA: 0x0034233E File Offset: 0x0034133E
			internal static bool ConvertToAttribute(ConverterState converterState, string attributeName, ref XamlAttribute xamlAttribute)
			{
				if (attributeName.Length == 0)
				{
					return false;
				}
				xamlAttribute = (XamlAttribute)XamlToRtfWriter.XamlParserHelper.BasicLookup(XamlToRtfWriter.XamlParserHelper.AttributeTable, attributeName);
				return xamlAttribute > XamlAttribute.XAUnknown;
			}

			// Token: 0x06008E81 RID: 36481 RVA: 0x0034235C File Offset: 0x0034135C
			internal static bool ConvertToFont(ConverterState converterState, string attributeName, ref long fontIndex)
			{
				if (attributeName.Length == 0)
				{
					return false;
				}
				FontTable fontTable = converterState.FontTable;
				FontTableEntry fontTableEntry = fontTable.FindEntryByName(attributeName);
				if (fontTableEntry == null)
				{
					fontTableEntry = fontTable.DefineEntry(fontTable.Count + 1);
					if (fontTableEntry != null)
					{
						fontTableEntry.Name = attributeName;
						fontTableEntry.ComputePreferredCodePage();
					}
				}
				if (fontTableEntry == null)
				{
					return false;
				}
				fontIndex = (long)fontTableEntry.Index;
				return true;
			}

			// Token: 0x06008E82 RID: 36482 RVA: 0x003423B4 File Offset: 0x003413B4
			internal static bool ConvertToFontSize(ConverterState converterState, string s, ref double d)
			{
				if (s.Length == 0)
				{
					return false;
				}
				int num = s.Length - 1;
				while (num >= 0 && (s[num] < '0' || s[num] > '9') && s[num] != '.')
				{
					num--;
				}
				string text = null;
				if (num < s.Length - 1)
				{
					text = s.Substring(num + 1);
					s = s.Substring(0, num + 1);
				}
				bool flag = Converters.StringToDouble(s, ref d);
				if (flag)
				{
					if (text == null || text.Length == 0)
					{
						d = Converters.PxToPt(d);
					}
					d *= 2.0;
				}
				return flag && d > 0.0;
			}

			// Token: 0x06008E83 RID: 36483 RVA: 0x0034245F File Offset: 0x0034145F
			internal static bool ConvertToTextIndent(ConverterState converterState, string s, ref double d)
			{
				return Converters.StringToDouble(s, ref d);
			}

			// Token: 0x06008E84 RID: 36484 RVA: 0x0034245F File Offset: 0x0034145F
			internal static bool ConvertToLineHeight(ConverterState converterState, string s, ref double d)
			{
				return Converters.StringToDouble(s, ref d);
			}

			// Token: 0x06008E85 RID: 36485 RVA: 0x00342468 File Offset: 0x00341468
			internal static bool ConvertToColor(ConverterState converterState, string brush, ref long colorIndex)
			{
				if (brush.Length == 0)
				{
					return false;
				}
				ColorTable colorTable = converterState.ColorTable;
				if (brush[0] == '#')
				{
					int num = 1;
					uint num2 = 0U;
					while (num < brush.Length && num < 9)
					{
						char c = brush[num];
						if (c >= '0' && c <= '9')
						{
							num2 = (uint)((ulong)((ulong)num2 << 4) + (ulong)((long)(c - '0')));
						}
						else if (c >= 'A' && c <= 'F')
						{
							num2 = (uint)((ulong)((ulong)num2 << 4) + (ulong)((long)(c - 'A' + '\n')));
						}
						else
						{
							if (c < 'a' || c > 'f')
							{
								break;
							}
							num2 = (uint)((ulong)((ulong)num2 << 4) + (ulong)((long)(c - 'a' + '\n')));
						}
						num++;
					}
					Color color = Color.FromRgb((byte)((num2 & 16711680U) >> 16), (byte)((num2 & 65280U) >> 8), (byte)(num2 & 255U));
					colorIndex = (long)colorTable.AddColor(color);
					return colorIndex >= 0L;
				}
				bool result;
				try
				{
					Color color2 = (Color)ColorConverter.ConvertFromString(brush);
					colorIndex = (long)colorTable.AddColor(color2);
					result = (colorIndex >= 0L);
				}
				catch (NotSupportedException)
				{
					result = false;
				}
				catch (FormatException)
				{
					result = false;
				}
				return result;
			}

			// Token: 0x06008E86 RID: 36486 RVA: 0x00342594 File Offset: 0x00341594
			internal static bool ConvertToDecoration(ConverterState converterState, string decoration, ref ULState ulState, ref StrikeState strikeState)
			{
				ulState = ULState.ULNone;
				strikeState = StrikeState.StrikeNone;
				if (decoration.IndexOf("Underline", StringComparison.OrdinalIgnoreCase) != -1)
				{
					ulState = ULState.ULNormal;
				}
				if (decoration.IndexOf("Strikethrough", StringComparison.OrdinalIgnoreCase) != -1)
				{
					strikeState = StrikeState.StrikeNormal;
				}
				return ulState != ULState.ULNone || strikeState > StrikeState.StrikeNone;
			}

			// Token: 0x06008E87 RID: 36487 RVA: 0x003425CB File Offset: 0x003415CB
			internal static bool ConvertToDir(ConverterState converterState, string dirName, ref DirState dirState)
			{
				if (dirName.Length == 0)
				{
					return false;
				}
				if (string.Compare("RightToLeft", dirName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					dirState = DirState.DirRTL;
					return true;
				}
				if (string.Compare("LeftToRight", dirName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					dirState = DirState.DirLTR;
					return true;
				}
				return false;
			}

			// Token: 0x06008E88 RID: 36488 RVA: 0x00342600 File Offset: 0x00341600
			internal static bool ConvertTagToNodeType(XamlToRtfWriter.XamlTag xamlTag, ref DocumentNodeType documentNodeType)
			{
				documentNodeType = DocumentNodeType.dnUnknown;
				switch (xamlTag)
				{
				default:
					return false;
				case XamlToRtfWriter.XamlTag.XTBold:
				case XamlToRtfWriter.XamlTag.XTItalic:
				case XamlToRtfWriter.XamlTag.XTUnderline:
				case XamlToRtfWriter.XamlTag.XTInline:
					documentNodeType = DocumentNodeType.dnInline;
					break;
				case XamlToRtfWriter.XamlTag.XTHyperlink:
					documentNodeType = DocumentNodeType.dnHyperlink;
					break;
				case XamlToRtfWriter.XamlTag.XTLineBreak:
					documentNodeType = DocumentNodeType.dnLineBreak;
					break;
				case XamlToRtfWriter.XamlTag.XTParagraph:
					documentNodeType = DocumentNodeType.dnParagraph;
					break;
				case XamlToRtfWriter.XamlTag.XTInlineUIContainer:
					documentNodeType = DocumentNodeType.dnInlineUIContainer;
					break;
				case XamlToRtfWriter.XamlTag.XTBlockUIContainer:
					documentNodeType = DocumentNodeType.dnBlockUIContainer;
					break;
				case XamlToRtfWriter.XamlTag.XTImage:
					documentNodeType = DocumentNodeType.dnImage;
					break;
				case XamlToRtfWriter.XamlTag.XTList:
					documentNodeType = DocumentNodeType.dnList;
					break;
				case XamlToRtfWriter.XamlTag.XTListItem:
					documentNodeType = DocumentNodeType.dnListItem;
					break;
				case XamlToRtfWriter.XamlTag.XTTable:
					documentNodeType = DocumentNodeType.dnTable;
					break;
				case XamlToRtfWriter.XamlTag.XTTableBody:
					documentNodeType = DocumentNodeType.dnTableBody;
					break;
				case XamlToRtfWriter.XamlTag.XTTableRow:
					documentNodeType = DocumentNodeType.dnRow;
					break;
				case XamlToRtfWriter.XamlTag.XTTableCell:
					documentNodeType = DocumentNodeType.dnCell;
					break;
				case XamlToRtfWriter.XamlTag.XTSection:
					documentNodeType = DocumentNodeType.dnSection;
					break;
				}
				return true;
			}

			// Token: 0x06008E89 RID: 36489 RVA: 0x003426B4 File Offset: 0x003416B4
			internal static bool ConvertToMarkerStyle(ConverterState converterState, string styleName, ref MarkerStyle ms)
			{
				ms = MarkerStyle.MarkerBullet;
				if (styleName.Length == 0)
				{
					return false;
				}
				ms = (MarkerStyle)XamlToRtfWriter.XamlParserHelper.BasicLookup(XamlToRtfWriter.XamlParserHelper.MarkerStyleTable, styleName);
				return true;
			}

			// Token: 0x06008E8A RID: 36490 RVA: 0x003426D4 File Offset: 0x003416D4
			internal static bool ConvertToStartIndex(ConverterState converterState, string s, ref int i)
			{
				bool result = true;
				try
				{
					i = Convert.ToInt32(s, CultureInfo.InvariantCulture);
				}
				catch (OverflowException)
				{
					result = false;
				}
				catch (FormatException)
				{
					result = false;
				}
				return result;
			}

			// Token: 0x06008E8B RID: 36491 RVA: 0x0034271C File Offset: 0x0034171C
			internal static bool ConvertToThickness(ConverterState converterState, string thickness, ref XamlToRtfWriter.XamlThickness xthickness)
			{
				int num = 0;
				int num2;
				for (int i = 0; i < thickness.Length; i = num2 + 1)
				{
					num2 = i;
					while (num2 < thickness.Length && thickness[num2] != ',')
					{
						num2++;
					}
					string text = thickness.Substring(i, num2 - i);
					if (text.Length > 0)
					{
						double num3 = 0.0;
						if (!Converters.StringToDouble(text, ref num3))
						{
							return false;
						}
						switch (num)
						{
						case 0:
							xthickness.Left = (float)num3;
							break;
						case 1:
							xthickness.Top = (float)num3;
							break;
						case 2:
							xthickness.Right = (float)num3;
							break;
						case 3:
							xthickness.Bottom = (float)num3;
							break;
						default:
							return false;
						}
						num++;
					}
				}
				if (num == 1)
				{
					xthickness.Top = xthickness.Left;
					xthickness.Right = xthickness.Left;
					xthickness.Bottom = xthickness.Left;
					num = 4;
				}
				return num == 4;
			}

			// Token: 0x06008E8C RID: 36492 RVA: 0x00342801 File Offset: 0x00341801
			internal static bool ConvertToHAlign(ConverterState converterState, string alignName, ref HAlign align)
			{
				if (alignName.Length == 0)
				{
					return false;
				}
				align = (HAlign)XamlToRtfWriter.XamlParserHelper.BasicLookup(XamlToRtfWriter.XamlParserHelper.HAlignTable, alignName);
				return true;
			}

			// Token: 0x06008E8D RID: 36493 RVA: 0x0034281B File Offset: 0x0034181B
			internal static bool ConvertToFontStretch(ConverterState converterState, string stretchName, ref long twips)
			{
				if (stretchName.Length == 0)
				{
					return false;
				}
				twips = (long)XamlToRtfWriter.XamlParserHelper.BasicLookup(XamlToRtfWriter.XamlParserHelper.HAlignTable, stretchName);
				return true;
			}

			// Token: 0x06008E8E RID: 36494 RVA: 0x00342838 File Offset: 0x00341838
			internal static void AppendRTFText(StringBuilder sb, string s, int cp)
			{
				if (cp <= 0)
				{
					cp = 1252;
				}
				Encoding encoding = null;
				byte[] rgAnsi = new byte[20];
				char[] rgChar = new char[20];
				for (int i = 0; i < s.Length; i++)
				{
					XamlToRtfWriter.XamlParserHelper.AppendRtfChar(sb, s[i], cp, ref encoding, rgAnsi, rgChar);
				}
			}

			// Token: 0x06008E8F RID: 36495 RVA: 0x00342888 File Offset: 0x00341888
			internal static void EnsureParagraphClosed(ConverterState converterState)
			{
				DocumentNodeArray documentNodeArray = converterState.DocumentNodeArray;
				int num = documentNodeArray.FindPending(DocumentNodeType.dnParagraph);
				if (num >= 0)
				{
					documentNodeArray.EntryAt(num);
					documentNodeArray.CloseAt(num);
				}
			}

			// Token: 0x06008E90 RID: 36496 RVA: 0x003428B8 File Offset: 0x003418B8
			private static void AppendRtfChar(StringBuilder sb, char c, int cp, ref Encoding e, byte[] rgAnsi, char[] rgChar)
			{
				if (c == '{' || c == '}' || c == '\\')
				{
					sb.Append('\\');
				}
				if (c == '\t')
				{
					sb.Append("\\tab ");
					return;
				}
				if (c == '\f')
				{
					sb.Append("\\page ");
					return;
				}
				if (c < '\u0080')
				{
					sb.Append(c);
					return;
				}
				if (c <= '—')
				{
					if (c == '\u00a0')
					{
						sb.Append("\\~");
						return;
					}
					switch (c)
					{
					case '\u2002':
						sb.Append("\\enspace ");
						return;
					case '\u2003':
						sb.Append("\\emspace ");
						return;
					case '\u2005':
						sb.Append("\\qmspace ");
						return;
					case '‌':
						sb.Append("\\zwnj ");
						return;
					case '‍':
						sb.Append("\\zwj ");
						return;
					case '‎':
						sb.Append("\\ltrmark ");
						return;
					case '‏':
						sb.Append("\\rtlmark ");
						return;
					case '‑':
						sb.Append("\\_");
						return;
					case '–':
						sb.Append("\\endash ");
						return;
					case '—':
						sb.Append("\\emdash ");
						return;
					}
				}
				else
				{
					switch (c)
					{
					case '‘':
						sb.Append("\\lquote ");
						return;
					case '’':
						sb.Append("\\rquote ");
						return;
					case '‚':
					case '‛':
						break;
					case '“':
						sb.Append("\\ldblquote ");
						return;
					case '”':
						sb.Append("\\rdblquote ");
						return;
					default:
						if (c == '•')
						{
							sb.Append("\\bullet ");
							return;
						}
						break;
					}
				}
				XamlToRtfWriter.XamlParserHelper.AppendRtfUnicodeChar(sb, c, cp, ref e, rgAnsi, rgChar);
			}

			// Token: 0x06008E91 RID: 36497 RVA: 0x00342A90 File Offset: 0x00341A90
			private static void AppendRtfUnicodeChar(StringBuilder sb, char c, int cp, ref Encoding e, byte[] rgAnsi, char[] rgChar)
			{
				if (e == null)
				{
					e = InternalEncoding.GetEncoding(cp);
				}
				int bytes = e.GetBytes(new char[]
				{
					c
				}, 0, 1, rgAnsi, 0);
				if (e.GetChars(rgAnsi, 0, bytes, rgChar, 0) == 1 && rgChar[0] == c)
				{
					for (int i = 0; i < bytes; i++)
					{
						sb.Append("\\'");
						sb.Append(rgAnsi[i].ToString("x", CultureInfo.InvariantCulture));
					}
					return;
				}
				sb.Append("\\u");
				sb.Append(((short)c).ToString(CultureInfo.InvariantCulture));
				sb.Append("?");
			}

			// Token: 0x04004948 RID: 18760
			internal static XamlToRtfWriter.XamlParserHelper.LookupTableEntry[] TagTable = new XamlToRtfWriter.XamlParserHelper.LookupTableEntry[]
			{
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("", 0),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("", 0),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Bold", 1),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Italic", 2),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Underline", 3),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Hyperlink", 4),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Span", 5),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Run", 5),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("LineBreak", 6),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Paragraph", 7),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("InlineUIContainer", 5),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("BlockUIContainer", 9),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Image", 10),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("BitmapImage", 11),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("List", 12),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("ListItem", 13),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Table", 14),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TableRowGroup", 15),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TableRow", 16),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TableCell", 17),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TableColumn", 18),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Section", 19),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Figure", 21),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Floater", 20),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TextDecoration", 22)
			};

			// Token: 0x04004949 RID: 18761
			internal static XamlToRtfWriter.XamlParserHelper.LookupTableEntry[] AttributeTable = new XamlToRtfWriter.XamlParserHelper.LookupTableEntry[]
			{
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("", 0),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("FontWeight", 1),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("FontSize", 2),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("FontStyle", 3),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("FontFamily", 4),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Background", 6),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Foreground", 7),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("FlowDirection", 8),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TextDecorations", 9),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TextAlignment", 10),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("MarkerStyle", 11),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TextIndent", 12),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("ColumnSpan", 13),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("RowSpan", 14),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("StartIndex", 15),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("MarkerOffset", 16),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("BorderThickness", 17),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("BorderBrush", 18),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Padding", 19),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Margin", 20),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("KeepTogether", 21),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("KeepWithNext", 22),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("BaselineAlignment", 23),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("BaselineOffset", 24),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("NavigateUri", 25),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("TargetName", 26),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("LineHeight", 27),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("xml:lang", 37),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Height", 30),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Source", 31),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("UriSource", 32),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Stretch", 33),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("StretchDirection", 34),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Location", 28),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Width", 29),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Typography.Variants", 36)
			};

			// Token: 0x0400494A RID: 18762
			internal static XamlToRtfWriter.XamlParserHelper.LookupTableEntry[] MarkerStyleTable = new XamlToRtfWriter.XamlParserHelper.LookupTableEntry[]
			{
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("", 23),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("None", -1),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Decimal", 0),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("UpperRoman", 1),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("LowerRoman", 2),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("UpperLatin", 3),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("LowerLatin", 4),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Ordinal", 5),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Decimal", 6),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Disc", 23),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Box", 23),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Circle", 23),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Square", 23)
			};

			// Token: 0x0400494B RID: 18763
			internal static XamlToRtfWriter.XamlParserHelper.LookupTableEntry[] HAlignTable = new XamlToRtfWriter.XamlParserHelper.LookupTableEntry[]
			{
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("", 4),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Left", 0),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Right", 1),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Center", 2),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Justify", 3)
			};

			// Token: 0x0400494C RID: 18764
			internal static XamlToRtfWriter.XamlParserHelper.LookupTableEntry[] FontStretchTable = new XamlToRtfWriter.XamlParserHelper.LookupTableEntry[]
			{
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("", 0),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Normal", 0),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("UltraCondensed", -80),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("ExtraCondensed", -60),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Condensed", -40),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("SemiCondensed", -20),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("SemiExpanded", 20),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Expanded", 40),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("ExtraExpanded", 60),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("UltraExpanded", 80)
			};

			// Token: 0x0400494D RID: 18765
			internal static XamlToRtfWriter.XamlParserHelper.LookupTableEntry[] TypographyVariantsTable = new XamlToRtfWriter.XamlParserHelper.LookupTableEntry[]
			{
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Normal", 1),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Superscript", 2),
				new XamlToRtfWriter.XamlParserHelper.LookupTableEntry("Subscript", 3)
			};

			// Token: 0x02000C92 RID: 3218
			internal struct LookupTableEntry
			{
				// Token: 0x06009578 RID: 38264 RVA: 0x0034DFEA File Offset: 0x0034CFEA
				internal LookupTableEntry(string name, int value)
				{
					this._name = name;
					this._value = value;
				}

				// Token: 0x1700200D RID: 8205
				// (get) Token: 0x06009579 RID: 38265 RVA: 0x0034DFFA File Offset: 0x0034CFFA
				internal string Name
				{
					get
					{
						return this._name;
					}
				}

				// Token: 0x1700200E RID: 8206
				// (get) Token: 0x0600957A RID: 38266 RVA: 0x0034E002 File Offset: 0x0034D002
				internal int Value
				{
					get
					{
						return this._value;
					}
				}

				// Token: 0x04004FD0 RID: 20432
				private string _name;

				// Token: 0x04004FD1 RID: 20433
				private int _value;
			}
		}

		// Token: 0x02000B8C RID: 2956
		internal struct XamlThickness
		{
			// Token: 0x06008E93 RID: 36499 RVA: 0x00343272 File Offset: 0x00342272
			internal XamlThickness(float l, float t, float r, float b)
			{
				this._left = l;
				this._top = t;
				this._right = r;
				this._bottom = b;
			}

			// Token: 0x17001F2B RID: 7979
			// (get) Token: 0x06008E94 RID: 36500 RVA: 0x00343291 File Offset: 0x00342291
			// (set) Token: 0x06008E95 RID: 36501 RVA: 0x00343299 File Offset: 0x00342299
			internal float Left
			{
				get
				{
					return this._left;
				}
				set
				{
					this._left = value;
				}
			}

			// Token: 0x17001F2C RID: 7980
			// (get) Token: 0x06008E96 RID: 36502 RVA: 0x003432A2 File Offset: 0x003422A2
			// (set) Token: 0x06008E97 RID: 36503 RVA: 0x003432AA File Offset: 0x003422AA
			internal float Top
			{
				get
				{
					return this._top;
				}
				set
				{
					this._top = value;
				}
			}

			// Token: 0x17001F2D RID: 7981
			// (get) Token: 0x06008E98 RID: 36504 RVA: 0x003432B3 File Offset: 0x003422B3
			// (set) Token: 0x06008E99 RID: 36505 RVA: 0x003432BB File Offset: 0x003422BB
			internal float Right
			{
				get
				{
					return this._right;
				}
				set
				{
					this._right = value;
				}
			}

			// Token: 0x17001F2E RID: 7982
			// (get) Token: 0x06008E9A RID: 36506 RVA: 0x003432C4 File Offset: 0x003422C4
			// (set) Token: 0x06008E9B RID: 36507 RVA: 0x003432CC File Offset: 0x003422CC
			internal float Bottom
			{
				get
				{
					return this._bottom;
				}
				set
				{
					this._bottom = value;
				}
			}

			// Token: 0x0400494E RID: 18766
			private float _left;

			// Token: 0x0400494F RID: 18767
			private float _top;

			// Token: 0x04004950 RID: 18768
			private float _right;

			// Token: 0x04004951 RID: 18769
			private float _bottom;
		}
	}
}
