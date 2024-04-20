using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200010D RID: 269
	internal sealed class BreakRecordTable
	{
		// Token: 0x060006C9 RID: 1737 RVA: 0x001096AE File Offset: 0x001086AE
		internal BreakRecordTable(FlowDocumentPaginator owner)
		{
			this._owner = owner;
			this._breakRecords = new List<BreakRecordTable.BreakRecordTableEntry>();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x001096C8 File Offset: 0x001086C8
		internal PageBreakRecord GetPageBreakRecord(int pageNumber)
		{
			PageBreakRecord pageBreakRecord = null;
			Invariant.Assert(pageNumber >= 0 && pageNumber <= this._breakRecords.Count, "Invalid PageNumber.");
			if (pageNumber > 0)
			{
				Invariant.Assert(this._breakRecords[pageNumber - 1] != null, "Invalid BreakRecordTable entry.");
				pageBreakRecord = this._breakRecords[pageNumber - 1].BreakRecord;
				Invariant.Assert(pageBreakRecord != null, "BreakRecord can be null only for the first page.");
			}
			return pageBreakRecord;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0010973C File Offset: 0x0010873C
		internal FlowDocumentPage GetCachedDocumentPage(int pageNumber)
		{
			FlowDocumentPage flowDocumentPage = null;
			if (pageNumber < this._breakRecords.Count)
			{
				Invariant.Assert(this._breakRecords[pageNumber] != null, "Invalid BreakRecordTable entry.");
				WeakReference documentPage = this._breakRecords[pageNumber].DocumentPage;
				if (documentPage != null)
				{
					flowDocumentPage = (documentPage.Target as FlowDocumentPage);
					if (flowDocumentPage != null && flowDocumentPage.IsDisposed)
					{
						flowDocumentPage = null;
					}
				}
			}
			return flowDocumentPage;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x001097A4 File Offset: 0x001087A4
		internal bool GetPageNumberForContentPosition(TextPointer contentPosition, ref int pageNumber)
		{
			bool result = false;
			Invariant.Assert(pageNumber >= 0 && pageNumber <= this._breakRecords.Count, "Invalid PageNumber.");
			while (pageNumber < this._breakRecords.Count)
			{
				Invariant.Assert(this._breakRecords[pageNumber] != null, "Invalid BreakRecordTable entry.");
				ReadOnlyCollection<TextSegment> textSegments = this._breakRecords[pageNumber].TextSegments;
				if (textSegments == null)
				{
					break;
				}
				if (TextDocumentView.Contains(contentPosition, textSegments))
				{
					result = true;
					break;
				}
				pageNumber++;
			}
			return result;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0010982C File Offset: 0x0010882C
		internal void OnInvalidateLayout()
		{
			if (this._breakRecords.Count > 0)
			{
				this.InvalidateBreakRecords(0, this._breakRecords.Count);
				this._owner.InitiateNextAsyncOperation();
				this._owner.OnPagesChanged(0, 1073741823);
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0010986C File Offset: 0x0010886C
		internal void OnInvalidateLayout(ITextPointer start, ITextPointer end)
		{
			if (this._breakRecords.Count > 0)
			{
				int num;
				int num2;
				this.GetAffectedPages(start, end, out num, out num2);
				num2 = this._breakRecords.Count - num;
				if (num2 > 0)
				{
					this.InvalidateBreakRecords(num, num2);
					this._owner.InitiateNextAsyncOperation();
					this._owner.OnPagesChanged(num, 1073741823);
				}
			}
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x001098C9 File Offset: 0x001088C9
		internal void OnInvalidateRender()
		{
			if (this._breakRecords.Count > 0)
			{
				this.DisposePages(0, this._breakRecords.Count);
				this._owner.OnPagesChanged(0, this._breakRecords.Count);
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00109904 File Offset: 0x00108904
		internal void OnInvalidateRender(ITextPointer start, ITextPointer end)
		{
			if (this._breakRecords.Count > 0)
			{
				int num;
				int num2;
				this.GetAffectedPages(start, end, out num, out num2);
				if (num2 > 0)
				{
					this.DisposePages(num, num2);
					this._owner.OnPagesChanged(num, num2);
				}
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00109944 File Offset: 0x00108944
		internal void UpdateEntry(int pageNumber, FlowDocumentPage page, PageBreakRecord brOut, TextPointer dependentMax)
		{
			Invariant.Assert(pageNumber >= 0 && pageNumber <= this._breakRecords.Count, "The previous BreakRecord does not exist.");
			Invariant.Assert(page != null && page != DocumentPage.Missing, "Cannot update BRT with an invalid document page.");
			ITextView textView = (ITextView)((IServiceProvider)page).GetService(typeof(ITextView));
			Invariant.Assert(textView != null, "Cannot access ITextView for FlowDocumentPage.");
			bool isClean = this.IsClean;
			BreakRecordTable.BreakRecordTableEntry breakRecordTableEntry = new BreakRecordTable.BreakRecordTableEntry();
			breakRecordTableEntry.BreakRecord = brOut;
			breakRecordTableEntry.DocumentPage = new WeakReference(page);
			breakRecordTableEntry.TextSegments = textView.TextSegments;
			breakRecordTableEntry.DependentMax = dependentMax;
			if (pageNumber == this._breakRecords.Count)
			{
				this._breakRecords.Add(breakRecordTableEntry);
				this._owner.OnPaginationProgress(pageNumber, 1);
			}
			else
			{
				if (this._breakRecords[pageNumber].BreakRecord != null && this._breakRecords[pageNumber].BreakRecord != breakRecordTableEntry.BreakRecord)
				{
					this._breakRecords[pageNumber].BreakRecord.Dispose();
				}
				if (this._breakRecords[pageNumber].DocumentPage != null && this._breakRecords[pageNumber].DocumentPage.Target != null && this._breakRecords[pageNumber].DocumentPage.Target != breakRecordTableEntry.DocumentPage.Target)
				{
					((FlowDocumentPage)this._breakRecords[pageNumber].DocumentPage.Target).Dispose();
				}
				this._breakRecords[pageNumber] = breakRecordTableEntry;
			}
			if (!isClean && this.IsClean)
			{
				this._owner.OnPaginationCompleted();
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00109AE4 File Offset: 0x00108AE4
		internal bool HasPageBreakRecord(int pageNumber)
		{
			Invariant.Assert(pageNumber >= 0, "Page number cannot be negative.");
			if (pageNumber == 0)
			{
				return true;
			}
			if (pageNumber > this._breakRecords.Count)
			{
				return false;
			}
			Invariant.Assert(this._breakRecords[pageNumber - 1] != null, "Invalid BreakRecordTable entry.");
			return this._breakRecords[pageNumber - 1].BreakRecord != null;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00109B48 File Offset: 0x00108B48
		internal int Count
		{
			get
			{
				return this._breakRecords.Count;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00109B58 File Offset: 0x00108B58
		internal bool IsClean
		{
			get
			{
				if (this._breakRecords.Count == 0)
				{
					return false;
				}
				Invariant.Assert(this._breakRecords[this._breakRecords.Count - 1] != null, "Invalid BreakRecordTable entry.");
				return this._breakRecords[this._breakRecords.Count - 1].BreakRecord == null;
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00109BBC File Offset: 0x00108BBC
		private void DisposePages(int start, int count)
		{
			int i = start + count - 1;
			Invariant.Assert(start >= 0 && start < this._breakRecords.Count, "Invalid starting index for BreakRecordTable invalidation.");
			Invariant.Assert(start + count <= this._breakRecords.Count, "Partial invalidation of BreakRecordTable is not allowed.");
			while (i >= start)
			{
				Invariant.Assert(this._breakRecords[i] != null, "Invalid BreakRecordTable entry.");
				WeakReference documentPage = this._breakRecords[i].DocumentPage;
				if (documentPage != null && documentPage.Target != null)
				{
					((FlowDocumentPage)documentPage.Target).Dispose();
				}
				this._breakRecords[i].DocumentPage = null;
				i--;
			}
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00109C70 File Offset: 0x00108C70
		private void InvalidateBreakRecords(int start, int count)
		{
			int i = start + count - 1;
			Invariant.Assert(start >= 0 && start < this._breakRecords.Count, "Invalid starting index for BreakRecordTable invalidation.");
			Invariant.Assert(start + count == this._breakRecords.Count, "Partial invalidation of BreakRecordTable is not allowed.");
			while (i >= start)
			{
				Invariant.Assert(this._breakRecords[i] != null, "Invalid BreakRecordTable entry.");
				WeakReference documentPage = this._breakRecords[i].DocumentPage;
				if (documentPage != null && documentPage.Target != null)
				{
					((FlowDocumentPage)documentPage.Target).Dispose();
				}
				if (this._breakRecords[i].BreakRecord != null)
				{
					this._breakRecords[i].BreakRecord.Dispose();
				}
				this._breakRecords.RemoveAt(i);
				i--;
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00109D44 File Offset: 0x00108D44
		private void GetAffectedPages(ITextPointer start, ITextPointer end, out int pageStart, out int pageCount)
		{
			for (pageStart = 0; pageStart < this._breakRecords.Count; pageStart++)
			{
				Invariant.Assert(this._breakRecords[pageStart] != null, "Invalid BreakRecordTable entry.");
				TextPointer dependentMax = this._breakRecords[pageStart].DependentMax;
				if (dependentMax != null && start.CompareTo(dependentMax) <= 0)
				{
					break;
				}
				ReadOnlyCollection<TextSegment> textSegments = this._breakRecords[pageStart].TextSegments;
				if (textSegments == null)
				{
					break;
				}
				bool flag = false;
				foreach (TextSegment textSegment in textSegments)
				{
					if (start.CompareTo(textSegment.End) <= 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			pageCount = this._breakRecords.Count - pageStart;
		}

		// Token: 0x0400072B RID: 1835
		private FlowDocumentPaginator _owner;

		// Token: 0x0400072C RID: 1836
		private List<BreakRecordTable.BreakRecordTableEntry> _breakRecords;

		// Token: 0x020008C0 RID: 2240
		private class BreakRecordTableEntry
		{
			// Token: 0x04003C2E RID: 15406
			public PageBreakRecord BreakRecord;

			// Token: 0x04003C2F RID: 15407
			public ReadOnlyCollection<TextSegment> TextSegments;

			// Token: 0x04003C30 RID: 15408
			public WeakReference DocumentPage;

			// Token: 0x04003C31 RID: 15409
			public TextPointer DependentMax;
		}
	}
}
