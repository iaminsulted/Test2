using System;
using System.Collections;
using System.Text;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200067F RID: 1663
	internal class DocumentNodeArray : ArrayList
	{
		// Token: 0x06005212 RID: 21010 RVA: 0x00251B32 File Offset: 0x00250B32
		internal DocumentNodeArray() : base(100)
		{
			this._fMain = false;
			this._dnaOpen = null;
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x00251B4A File Offset: 0x00250B4A
		internal DocumentNode EntryAt(int nAt)
		{
			return (DocumentNode)this[nAt];
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x00251B58 File Offset: 0x00250B58
		internal void Push(DocumentNode documentNode)
		{
			this.InsertNode(this.Count, documentNode);
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x00251B67 File Offset: 0x00250B67
		internal DocumentNode Pop()
		{
			DocumentNode top = this.Top;
			if (this.Count > 0)
			{
				this.Excise(this.Count - 1, 1);
			}
			return top;
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x00251B88 File Offset: 0x00250B88
		internal DocumentNode TopPending()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.IsPending)
				{
					return documentNode;
				}
			}
			return null;
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x00251BBB File Offset: 0x00250BBB
		internal bool TestTop(DocumentNodeType documentNodeType)
		{
			return this.Count > 0 && this.EntryAt(this.Count - 1).Type == documentNodeType;
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x00251BE0 File Offset: 0x00250BE0
		internal void PreCoalesceChildren(ConverterState converterState, int nStart, bool bChild)
		{
			DocumentNodeArray documentNodeArray = new DocumentNodeArray();
			bool flag = false;
			int num = this.EntryAt(nStart).ChildCount;
			if (nStart + num >= this.Count)
			{
				num = this.Count - nStart - 1;
			}
			int num2 = nStart + num;
			if (bChild)
			{
				nStart++;
			}
			for (int i = nStart; i <= num2; i++)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.IsInline && documentNode.RequiresXamlDir && documentNode.ClosedParent != null)
				{
					int j;
					for (j = i + 1; j <= num2; j++)
					{
						DocumentNode documentNode2 = this.EntryAt(j);
						if (!documentNode2.IsInline || documentNode2.Type == DocumentNodeType.dnHyperlink || documentNode2.FormatState.DirChar != documentNode.FormatState.DirChar || documentNode2.ClosedParent != documentNode.ClosedParent)
						{
							break;
						}
					}
					int num3 = j - i;
					if (num3 > 1)
					{
						DocumentNode documentNode3 = new DocumentNode(DocumentNodeType.dnInline);
						documentNode3.FormatState = new FormatState(documentNode.Parent.FormatState);
						documentNode3.FormatState.DirChar = documentNode.FormatState.DirChar;
						this.InsertChildAt(documentNode.ClosedParent, documentNode3, i, num3);
						num2++;
					}
				}
				else if (documentNode.Type == DocumentNodeType.dnListItem)
				{
					this.PreCoalesceListItem(documentNode);
				}
				else if (documentNode.Type == DocumentNodeType.dnList)
				{
					this.PreCoalesceList(documentNode);
				}
				else if (documentNode.Type == DocumentNodeType.dnTable)
				{
					documentNodeArray.Add(documentNode);
					num2 += this.PreCoalesceTable(documentNode);
				}
				else if (documentNode.Type == DocumentNodeType.dnRow)
				{
					this.PreCoalesceRow(documentNode, ref flag);
				}
			}
			if (flag)
			{
				this.ProcessTableRowSpan(documentNodeArray);
			}
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x00251D8C File Offset: 0x00250D8C
		internal void CoalesceChildren(ConverterState converterState, int nStart)
		{
			if (nStart >= this.Count || nStart < 0)
			{
				return;
			}
			this.PreCoalesceChildren(converterState, nStart, false);
			int num = this.EntryAt(nStart).ChildCount;
			if (nStart + num >= this.Count)
			{
				num = this.Count - nStart - 1;
			}
			int num2 = nStart + num;
			for (int i = num2; i >= nStart; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.ChildCount == 0)
				{
					documentNode.Terminate(converterState);
				}
				else
				{
					documentNode.AppendXamlPrefix(converterState);
					StringBuilder stringBuilder = new StringBuilder(documentNode.Xaml);
					int childCount = documentNode.ChildCount;
					int num3 = i + childCount;
					for (int j = i + 1; j <= num3; j++)
					{
						DocumentNode documentNode2 = this.EntryAt(j);
						stringBuilder.Append(documentNode2.Xaml);
					}
					documentNode.Xaml = stringBuilder.ToString();
					documentNode.AppendXamlPostfix(converterState);
					documentNode.IsTerminated = true;
					this.Excise(i + 1, childCount);
					num2 -= childCount;
					this.AssertTreeInvariants();
				}
				if (documentNode.ColSpan == 0)
				{
					documentNode.Xaml = string.Empty;
				}
			}
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x00251E94 File Offset: 0x00250E94
		internal void CoalesceOnlyChildren(ConverterState converterState, int nStart)
		{
			if (nStart >= this.Count || nStart < 0)
			{
				return;
			}
			this.PreCoalesceChildren(converterState, nStart, true);
			int num = this.EntryAt(nStart).ChildCount;
			if (nStart + num >= this.Count)
			{
				num = this.Count - nStart - 1;
			}
			int num2 = nStart + num;
			for (int i = num2; i >= nStart; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.ChildCount == 0 && i != nStart)
				{
					documentNode.Terminate(converterState);
				}
				else if (documentNode.ChildCount > 0)
				{
					if (i != nStart)
					{
						documentNode.AppendXamlPrefix(converterState);
					}
					StringBuilder stringBuilder = new StringBuilder(documentNode.Xaml);
					int childCount = documentNode.ChildCount;
					int num3 = i + childCount;
					for (int j = i + 1; j <= num3; j++)
					{
						DocumentNode documentNode2 = this.EntryAt(j);
						stringBuilder.Append(documentNode2.Xaml);
					}
					documentNode.Xaml = stringBuilder.ToString();
					if (i != nStart)
					{
						documentNode.AppendXamlPostfix(converterState);
						documentNode.IsTerminated = true;
					}
					this.Excise(i + 1, childCount);
					num2 -= childCount;
				}
			}
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x00251FA0 File Offset: 0x00250FA0
		internal void CoalesceAll(ConverterState converterState)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this.CoalesceChildren(converterState, i);
			}
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x00251FC8 File Offset: 0x00250FC8
		internal void CloseAtHelper(int index, int nChildCount)
		{
			if (index >= this.Count || index < 0 || index + nChildCount >= this.Count)
			{
				return;
			}
			DocumentNode documentNode = this.EntryAt(index);
			if (!documentNode.IsPending)
			{
				return;
			}
			documentNode.IsPending = false;
			documentNode.ChildCount = nChildCount;
			int i = index + 1;
			int num = index + documentNode.ChildCount;
			while (i <= num)
			{
				DocumentNode documentNode2 = this.EntryAt(i);
				documentNode2.Parent = documentNode;
				i += documentNode2.ChildCount + 1;
			}
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x0025203C File Offset: 0x0025103C
		internal void CloseAt(int index)
		{
			if (index >= this.Count || index < 0)
			{
				return;
			}
			if (!this.EntryAt(index).IsPending)
			{
				return;
			}
			this.AssertTreeInvariants();
			this.AssertTreeSemanticInvariants();
			for (int i = this.Count - 1; i > index; i--)
			{
				if (this.EntryAt(i).IsPending)
				{
					this.CloseAt(i);
				}
			}
			this.CloseAtHelper(index, this.Count - index - 1);
			this.AssertTreeInvariants();
			this.AssertTreeSemanticInvariants();
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x002520B8 File Offset: 0x002510B8
		internal void AssertTreeInvariants()
		{
			if (Invariant.Strict)
			{
				for (int i = 0; i < this.Count; i++)
				{
					DocumentNode documentNode = this.EntryAt(i);
					for (int j = i + 1; j <= documentNode.LastChildIndex; j++)
					{
					}
					for (DocumentNode parent = documentNode.Parent; parent != null; parent = parent.Parent)
					{
					}
				}
			}
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x0025210C File Offset: 0x0025110C
		internal void AssertTreeSemanticInvariants()
		{
			if (Invariant.Strict)
			{
				int i = 0;
				while (i < this.Count)
				{
					DocumentNode documentNode = this.EntryAt(i);
					DocumentNode parent = documentNode.Parent;
					switch (documentNode.Type)
					{
					default:
						i++;
						break;
					}
				}
			}
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x00252164 File Offset: 0x00251164
		internal void CloseAll()
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.EntryAt(i).IsPending)
				{
					this.CloseAt(i);
					return;
				}
			}
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x00252198 File Offset: 0x00251198
		internal int CountOpenNodes(DocumentNodeType documentNodeType)
		{
			int num = 0;
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending)
					{
						if (documentNode.Type == documentNodeType)
						{
							num++;
						}
						else if (documentNode.Type == DocumentNodeType.dnShape)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x00252201 File Offset: 0x00251201
		internal int CountOpenCells()
		{
			return this.CountOpenNodes(DocumentNodeType.dnCell);
		}

		// Token: 0x06005223 RID: 21027 RVA: 0x0025220C File Offset: 0x0025120C
		internal DocumentNode GetOpenParentWhileParsing(DocumentNode dn)
		{
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending && documentNode.Index < dn.Index)
					{
						return documentNode;
					}
				}
			}
			return null;
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x0025226C File Offset: 0x0025126C
		internal DocumentNodeType GetTableScope()
		{
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending)
					{
						if (documentNode.Type == DocumentNodeType.dnTable || documentNode.Type == DocumentNodeType.dnTableBody || documentNode.Type == DocumentNodeType.dnRow || documentNode.Type == DocumentNodeType.dnCell)
						{
							return documentNode.Type;
						}
						if (documentNode.Type == DocumentNodeType.dnShape)
						{
							return DocumentNodeType.dnParagraph;
						}
					}
				}
			}
			return DocumentNodeType.dnParagraph;
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x002522F8 File Offset: 0x002512F8
		internal MarkerList GetOpenMarkerStyles()
		{
			MarkerList markerList = new MarkerList();
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				int num = 0;
				for (int i = 0; i < this._dnaOpen.Count; i++)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.IsPending && documentNode.Type == DocumentNodeType.dnShape)
					{
						num = i + 1;
					}
				}
				for (int j = num; j < this._dnaOpen.Count; j++)
				{
					DocumentNode documentNode2 = this._dnaOpen.EntryAt(j);
					if (documentNode2.IsPending && documentNode2.Type == DocumentNodeType.dnList)
					{
						markerList.AddEntry(documentNode2.FormatState.Marker, documentNode2.FormatState.ILS, documentNode2.FormatState.StartIndex, documentNode2.FormatState.StartIndexDefault, documentNode2.VirtualListLevel);
					}
				}
			}
			return markerList;
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x002523D8 File Offset: 0x002513D8
		internal MarkerList GetLastMarkerStyles(MarkerList mlHave, MarkerList mlWant)
		{
			MarkerList markerList = new MarkerList();
			if (mlHave.Count > 0 || mlWant.Count == 0)
			{
				return markerList;
			}
			bool flag = true;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnCell || documentNode.Type == DocumentNodeType.dnTable)
				{
					break;
				}
				if (documentNode.Type == DocumentNodeType.dnListItem)
				{
					DocumentNode parentOfType = documentNode.GetParentOfType(DocumentNodeType.dnCell);
					if (parentOfType != null && !parentOfType.IsPending)
					{
						break;
					}
					DocumentNode parentOfType2 = documentNode.GetParentOfType(DocumentNodeType.dnShape);
					if (parentOfType2 == null || parentOfType2.IsPending)
					{
						for (DocumentNode parent = documentNode.Parent; parent != null; parent = parent.Parent)
						{
							if (parent.Type == DocumentNodeType.dnList)
							{
								MarkerListEntry markerListEntry = new MarkerListEntry();
								markerListEntry.Marker = parent.FormatState.Marker;
								markerListEntry.StartIndexOverride = parent.FormatState.StartIndex;
								markerListEntry.StartIndexDefault = parent.FormatState.StartIndexDefault;
								markerListEntry.VirtualListLevel = parent.VirtualListLevel;
								markerListEntry.ILS = parent.FormatState.ILS;
								markerList.Insert(0, markerListEntry);
								if (markerListEntry.Marker != MarkerStyle.MarkerBullet)
								{
									flag = false;
								}
							}
						}
						break;
					}
				}
			}
			if (markerList.Count == 1 && flag)
			{
				markerList.RemoveRange(0, 1);
			}
			return markerList;
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x00252534 File Offset: 0x00251534
		internal void OpenLastList()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnListItem)
				{
					DocumentNode parentOfType = documentNode.GetParentOfType(DocumentNodeType.dnShape);
					if (parentOfType == null || parentOfType.IsPending)
					{
						for (DocumentNode documentNode2 = documentNode; documentNode2 != null; documentNode2 = documentNode2.Parent)
						{
							if (documentNode2.Type == DocumentNodeType.dnList || documentNode2.Type == DocumentNodeType.dnListItem)
							{
								documentNode2.IsPending = true;
								this._dnaOpen.InsertOpenNode(documentNode2);
							}
						}
						return;
					}
				}
			}
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x002525B0 File Offset: 0x002515B0
		internal void OpenLastCell()
		{
			for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this._dnaOpen.EntryAt(i);
				if (documentNode.IsPending)
				{
					if (documentNode.Type == DocumentNodeType.dnCell)
					{
						return;
					}
					if (documentNode.Type == DocumentNodeType.dnTable || documentNode.Type == DocumentNodeType.dnTableBody || documentNode.Type == DocumentNodeType.dnRow)
					{
						for (int j = this.Count - 1; j >= 0; j--)
						{
							DocumentNode documentNode2 = this.EntryAt(j);
							if (documentNode2 == documentNode)
							{
								return;
							}
							if (documentNode2.Type == DocumentNodeType.dnCell && documentNode2.GetParentOfType(documentNode.Type) == documentNode)
							{
								DocumentNode documentNode3 = documentNode2;
								while (documentNode3 != null && documentNode3 != documentNode)
								{
									documentNode3.IsPending = true;
									this._dnaOpen.InsertOpenNode(documentNode3);
									documentNode3 = documentNode3.Parent;
								}
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x00252684 File Offset: 0x00251684
		internal int FindPendingFrom(DocumentNodeType documentNodeType, int nStart, int nLow)
		{
			if (this._dnaOpen != null)
			{
				this._dnaOpen.CullOpen();
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.Index <= nStart)
					{
						if (documentNode.Index <= nLow)
						{
							break;
						}
						if (documentNode.IsPending)
						{
							if (documentNode.Type == documentNodeType)
							{
								return documentNode.Index;
							}
							if (documentNode.Type == DocumentNodeType.dnShape)
							{
								break;
							}
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x002526FE File Offset: 0x002516FE
		internal int FindPending(DocumentNodeType documentNodeType, int nLow)
		{
			return this.FindPendingFrom(documentNodeType, this.Count - 1, nLow);
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x00252710 File Offset: 0x00251710
		internal int FindPending(DocumentNodeType documentNodeType)
		{
			return this.FindPending(documentNodeType, -1);
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x0025271C File Offset: 0x0025171C
		internal int FindUnmatched(DocumentNodeType dnType)
		{
			if (this._dnaOpen != null)
			{
				for (int i = this._dnaOpen.Count - 1; i >= 0; i--)
				{
					DocumentNode documentNode = this._dnaOpen.EntryAt(i);
					if (documentNode.Type == dnType && !documentNode.IsMatched)
					{
						return documentNode.Index;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x00252770 File Offset: 0x00251770
		internal void EstablishTreeRelationships()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this.EntryAt(i).Index = i;
			}
			for (int i = 1; i < this.Count; i++)
			{
				DocumentNode documentNode = this.EntryAt(i);
				DocumentNode documentNode2 = this.EntryAt(i - 1);
				if (documentNode2.ChildCount == 0)
				{
					documentNode2 = documentNode2.Parent;
					while (documentNode2 != null && !documentNode2.IsAncestorOf(documentNode))
					{
						documentNode2 = documentNode2.Parent;
					}
				}
				documentNode.Parent = documentNode2;
			}
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x002527EC File Offset: 0x002517EC
		internal void CullOpen()
		{
			int i;
			for (i = this.Count - 1; i >= 0; i--)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Index >= 0 && documentNode.IsTrackedAsOpen)
				{
					break;
				}
			}
			int num = this.Count - (i + 1);
			if (num > 0)
			{
				this.RemoveRange(i + 1, num);
			}
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x00252840 File Offset: 0x00251840
		internal void InsertOpenNode(DocumentNode dn)
		{
			this.CullOpen();
			int num = this.Count;
			while (num > 0 && dn.Index <= this.EntryAt(num - 1).Index)
			{
				num--;
			}
			this.Insert(num, dn);
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x00252884 File Offset: 0x00251884
		internal void InsertNode(int nAt, DocumentNode dn)
		{
			this.Insert(nAt, dn);
			if (this._fMain)
			{
				dn.Index = nAt;
				dn.DNA = this;
				for (nAt++; nAt < this.Count; nAt++)
				{
					this.EntryAt(nAt).Index = nAt;
				}
				if (dn.IsTrackedAsOpen)
				{
					if (this._dnaOpen == null)
					{
						this._dnaOpen = new DocumentNodeArray();
					}
					this._dnaOpen.InsertOpenNode(dn);
				}
			}
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x002528F8 File Offset: 0x002518F8
		internal void InsertChildAt(DocumentNode dnParent, DocumentNode dnNew, int nInsertAt, int nChild)
		{
			this.InsertNode(nInsertAt, dnNew);
			this.CloseAtHelper(nInsertAt, nChild);
			if (dnParent != null && dnParent.Parent == dnNew)
			{
				Invariant.Assert(false, "Parent's Parent node shouldn't be the child node!");
			}
			dnNew.Parent = dnParent;
			while (dnParent != null)
			{
				dnParent.ChildCount++;
				dnParent = dnParent.ClosedParent;
			}
			this.AssertTreeInvariants();
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x00252958 File Offset: 0x00251958
		internal void Excise(int nAt, int nExcise)
		{
			DocumentNode documentNode = this.EntryAt(nAt);
			if (this._fMain)
			{
				int num = nAt + nExcise;
				for (int i = nAt; i < num; i++)
				{
					DocumentNode documentNode2 = this.EntryAt(i);
					documentNode2.Index = -1;
					documentNode2.DNA = null;
				}
			}
			this.RemoveRange(nAt, nExcise);
			if (this._fMain)
			{
				for (DocumentNode parent = documentNode.Parent; parent != null; parent = parent.Parent)
				{
					if (!parent.IsPending)
					{
						parent.ChildCount -= nExcise;
					}
				}
				while (nAt < this.Count)
				{
					this.EntryAt(nAt).Index = nAt;
					nAt++;
				}
				this.AssertTreeInvariants();
			}
		}

		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06005233 RID: 21043 RVA: 0x002529F5 File Offset: 0x002519F5
		internal DocumentNode Top
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this.EntryAt(this.Count - 1);
			}
		}

		// Token: 0x1700136B RID: 4971
		// (set) Token: 0x06005234 RID: 21044 RVA: 0x00252A10 File Offset: 0x00251A10
		internal bool IsMain
		{
			set
			{
				this._fMain = value;
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x00252A1C File Offset: 0x00251A1C
		private void PreCoalesceListItem(DocumentNode dn)
		{
			int index = dn.Index;
			long num = -1L;
			int num2 = index + dn.ChildCount;
			for (int i = index + 1; i <= num2; i++)
			{
				DocumentNode documentNode = this.EntryAt(i);
				if (documentNode.Type == DocumentNodeType.dnParagraph)
				{
					if (num == -1L)
					{
						num = documentNode.NearMargin;
					}
					else if (documentNode.NearMargin < num && documentNode.IsNonEmpty)
					{
						num = documentNode.NearMargin;
					}
				}
			}
			dn.NearMargin = num;
			for (int j = index; j <= num2; j++)
			{
				DocumentNode documentNode2 = this.EntryAt(j);
				if (documentNode2.Type == DocumentNodeType.dnParagraph)
				{
					documentNode2.NearMargin -= num;
				}
			}
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x00252AC4 File Offset: 0x00251AC4
		private void PreCoalesceList(DocumentNode dn)
		{
			int index = dn.Index;
			bool flag = false;
			DirState dirState = DirState.DirDefault;
			int num = index + dn.ChildCount;
			int num2 = index + 1;
			while (!flag && num2 <= num)
			{
				DocumentNode documentNode = this.EntryAt(num2);
				if (documentNode.Type == DocumentNodeType.dnParagraph && documentNode.IsNonEmpty)
				{
					if (dirState == DirState.DirDefault)
					{
						dirState = documentNode.FormatState.DirPara;
					}
					else if (dirState != documentNode.FormatState.DirPara)
					{
						flag = true;
					}
				}
				num2++;
			}
			if (!flag && dirState != DirState.DirDefault)
			{
				for (int i = index; i <= num; i++)
				{
					DocumentNode documentNode2 = this.EntryAt(i);
					if (documentNode2.Type == DocumentNodeType.dnList || documentNode2.Type == DocumentNodeType.dnListItem)
					{
						documentNode2.FormatState.DirPara = dirState;
					}
				}
			}
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x00252B80 File Offset: 0x00251B80
		private int PreCoalesceTable(DocumentNode dn)
		{
			int result = 0;
			int index = dn.Index;
			ColumnStateArray columnStateArray = dn.ComputeColumns();
			int minUnfilledRowIndex = columnStateArray.GetMinUnfilledRowIndex();
			if (minUnfilledRowIndex > 0)
			{
				DocumentNode documentNode = new DocumentNode(DocumentNodeType.dnTable);
				DocumentNode documentNode2 = new DocumentNode(DocumentNodeType.dnTableBody);
				documentNode.FormatState = new FormatState(dn.FormatState);
				documentNode.FormatState.RowFormat = this.EntryAt(minUnfilledRowIndex).FormatState.RowFormat;
				int num = minUnfilledRowIndex - dn.Index - 1;
				int num2 = dn.ChildCount - num;
				dn.ChildCount = num;
				this.EntryAt(index + 1).ChildCount = num - 1;
				this.InsertNode(minUnfilledRowIndex, documentNode2);
				this.CloseAtHelper(minUnfilledRowIndex, num2);
				this.InsertNode(minUnfilledRowIndex, documentNode);
				this.CloseAtHelper(minUnfilledRowIndex, num2 + 1);
				documentNode2.Parent = documentNode;
				documentNode.Parent = dn.ClosedParent;
				for (DocumentNode closedParent = documentNode.ClosedParent; closedParent != null; closedParent = closedParent.ClosedParent)
				{
					closedParent.ChildCount += 2;
				}
				result = 2;
				dn.ColumnStateArray = dn.ComputeColumns();
			}
			else
			{
				dn.ColumnStateArray = columnStateArray;
			}
			return result;
		}

		// Token: 0x06005238 RID: 21048 RVA: 0x00252C9C File Offset: 0x00251C9C
		private void PreCoalesceRow(DocumentNode dn, ref bool fVMerged)
		{
			DocumentNodeArray rowsCells = dn.GetRowsCells();
			RowFormat rowFormat = dn.FormatState.RowFormat;
			DocumentNode parentOfType = dn.GetParentOfType(DocumentNodeType.dnTable);
			ColumnStateArray columnStateArray = (parentOfType != null) ? parentOfType.ColumnStateArray : null;
			int num = (rowsCells.Count < rowFormat.CellCount) ? rowsCells.Count : rowFormat.CellCount;
			int i = 0;
			int j = 0;
			while (j < num)
			{
				DocumentNode documentNode = rowsCells.EntryAt(j);
				CellFormat cellFormat = rowFormat.NthCellFormat(j);
				long cellX = cellFormat.CellX;
				if (cellFormat.IsVMerge)
				{
					fVMerged = true;
				}
				if (cellFormat.IsHMergeFirst)
				{
					for (j++; j < num; j++)
					{
						CellFormat cellFormat2 = rowFormat.NthCellFormat(j);
						if (cellFormat2.IsVMerge)
						{
							fVMerged = true;
						}
						if (cellFormat2.IsHMerge)
						{
							rowsCells.EntryAt(j).ColSpan = 0;
						}
					}
				}
				else
				{
					j++;
				}
				if (columnStateArray != null)
				{
					int num2 = i;
					while (i < columnStateArray.Count)
					{
						ColumnState columnState = columnStateArray.EntryAt(i);
						i++;
						if (columnState.CellX == cellX || columnState.CellX > cellX)
						{
							break;
						}
					}
					if (i - num2 > documentNode.ColSpan)
					{
						documentNode.ColSpan = i - num2;
					}
				}
			}
		}

		// Token: 0x06005239 RID: 21049 RVA: 0x00252DC8 File Offset: 0x00251DC8
		private void ProcessTableRowSpan(DocumentNodeArray dnaTables)
		{
			for (int i = 0; i < dnaTables.Count; i++)
			{
				DocumentNode documentNode = dnaTables.EntryAt(i);
				ColumnStateArray columnStateArray = documentNode.ColumnStateArray;
				if (columnStateArray != null && columnStateArray.Count != 0)
				{
					int count = columnStateArray.Count;
					DocumentNodeArray tableRows = documentNode.GetTableRows();
					DocumentNodeArray documentNodeArray = new DocumentNodeArray();
					for (int j = 0; j < count; j++)
					{
						documentNodeArray.Add(null);
					}
					for (int k = 0; k < tableRows.Count; k++)
					{
						DocumentNode documentNode2 = tableRows.EntryAt(k);
						RowFormat rowFormat = documentNode2.FormatState.RowFormat;
						DocumentNodeArray rowsCells = documentNode2.GetRowsCells();
						int num = count;
						if (rowFormat.CellCount < num)
						{
							num = rowFormat.CellCount;
						}
						if (rowsCells.Count < num)
						{
							num = rowsCells.Count;
						}
						int num2 = 0;
						int num3 = 0;
						while (num3 < num && num2 < documentNodeArray.Count)
						{
							DocumentNode documentNode3 = rowsCells.EntryAt(num3);
							CellFormat cellFormat = rowFormat.NthCellFormat(num3);
							if (cellFormat.IsVMerge)
							{
								DocumentNode documentNode4 = documentNodeArray.EntryAt(num2);
								if (documentNode4 != null)
								{
									documentNode4.RowSpan++;
								}
								num2 += documentNode3.ColSpan;
								documentNode3.ColSpan = 0;
							}
							else
							{
								if (cellFormat.IsVMergeFirst)
								{
									documentNode3.RowSpan = 1;
									documentNodeArray[num2] = documentNode3;
								}
								else
								{
									documentNodeArray[num2] = null;
								}
								for (int l = num2 + 1; l < num2 + documentNode3.ColSpan; l++)
								{
									documentNodeArray[l] = null;
								}
								num2 += documentNode3.ColSpan;
							}
							num3++;
						}
					}
				}
			}
		}

		// Token: 0x04002E92 RID: 11922
		private bool _fMain;

		// Token: 0x04002E93 RID: 11923
		private DocumentNodeArray _dnaOpen;
	}
}
