using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020001CE RID: 462
	internal class ReaderPageViewer : FlowDocumentPageViewer, IFlowDocumentViewer
	{
		// Token: 0x0600102F RID: 4143 RVA: 0x0013F58C File Offset: 0x0013E58C
		protected override void OnPrintCompleted()
		{
			base.OnPrintCompleted();
			if (this._printCompleted != null)
			{
				this._printCompleted(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x0013F5AD File Offset: 0x0013E5AD
		protected override void OnPrintCommand()
		{
			base.OnPrintCommand();
			if (this._printStarted != null && base.IsPrinting)
			{
				this._printStarted(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0013F5D8 File Offset: 0x0013E5D8
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DocumentViewerBase.PageCountProperty || e.Property == DocumentViewerBase.MasterPageNumberProperty || e.Property == DocumentViewerBase.CanGoToPreviousPageProperty || e.Property == DocumentViewerBase.CanGoToNextPageProperty)
			{
				if (!this._raisePageNumberChanged && !this._raisePageCountChanged)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.RaisePropertyChangedAsync), null);
				}
				if (e.Property == DocumentViewerBase.PageCountProperty)
				{
					this._raisePageCountChanged = true;
					base.CoerceValue(DocumentViewerBase.CanGoToNextPageProperty);
					return;
				}
				if (e.Property == DocumentViewerBase.MasterPageNumberProperty)
				{
					this._raisePageNumberChanged = true;
					base.CoerceValue(DocumentViewerBase.CanGoToNextPageProperty);
					return;
				}
				this._raisePageNumberChanged = true;
			}
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0013F698 File Offset: 0x0013E698
		private object SetTextSelection(object arg)
		{
			ITextSelection textSelection = arg as ITextSelection;
			FlowDocument flowDocument = base.Document as FlowDocument;
			if (textSelection != null && flowDocument != null && textSelection.AnchorPosition != null && textSelection.AnchorPosition.TextContainer == flowDocument.StructuralCache.TextContainer)
			{
				ITextSelection textSelection2 = flowDocument.StructuralCache.TextContainer.TextSelection;
				if (textSelection2 != null)
				{
					textSelection2.SetCaretToPosition(textSelection.AnchorPosition, textSelection.MovingPosition.LogicalDirection, true, true);
					textSelection2.ExtendToPosition(textSelection.MovingPosition);
				}
			}
			return null;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0013F718 File Offset: 0x0013E718
		private object RaisePropertyChangedAsync(object arg)
		{
			if (this._raisePageCountChanged)
			{
				if (this._pageCountChanged != null)
				{
					this._pageCountChanged(this, EventArgs.Empty);
				}
				this._raisePageCountChanged = false;
			}
			if (this._raisePageNumberChanged)
			{
				if (this._pageNumberChanged != null)
				{
					this._pageNumberChanged(this, EventArgs.Empty);
				}
				this._raisePageNumberChanged = false;
			}
			return null;
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0013F776 File Offset: 0x0013E776
		void IFlowDocumentViewer.PreviousPage()
		{
			this.OnPreviousPageCommand();
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0013F77E File Offset: 0x0013E77E
		void IFlowDocumentViewer.NextPage()
		{
			this.OnNextPageCommand();
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0013F786 File Offset: 0x0013E786
		void IFlowDocumentViewer.FirstPage()
		{
			this.OnFirstPageCommand();
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0013F78E File Offset: 0x0013E78E
		void IFlowDocumentViewer.LastPage()
		{
			this.OnLastPageCommand();
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0013F796 File Offset: 0x0013E796
		void IFlowDocumentViewer.Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0013F79E File Offset: 0x0013E79E
		void IFlowDocumentViewer.CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0013F7A6 File Offset: 0x0013E7A6
		void IFlowDocumentViewer.ShowFindResult(ITextRange findResult)
		{
			if (findResult.Start is ContentPosition)
			{
				base.BringContentPositionIntoView((ContentPosition)findResult.Start);
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0013F7C7 File Offset: 0x0013E7C7
		bool IFlowDocumentViewer.CanGoToPage(int pageNumber)
		{
			return this.CanGoToPage(pageNumber);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0013F7D0 File Offset: 0x0013E7D0
		void IFlowDocumentViewer.GoToPage(int pageNumber)
		{
			this.OnGoToPageCommand(pageNumber);
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0013F7D9 File Offset: 0x0013E7D9
		void IFlowDocumentViewer.SetDocument(FlowDocument document)
		{
			base.Document = document;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0013F7E2 File Offset: 0x0013E7E2
		// (set) Token: 0x0600103F RID: 4159 RVA: 0x0013F7EA File Offset: 0x0013E7EA
		ContentPosition IFlowDocumentViewer.ContentPosition
		{
			get
			{
				return base.ContentPosition;
			}
			set
			{
				if (value != null && base.Document != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(base.BringContentPositionIntoView), value);
				}
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0013F811 File Offset: 0x0013E811
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x0013F819 File Offset: 0x0013E819
		ITextSelection IFlowDocumentViewer.TextSelection
		{
			get
			{
				return base.Selection;
			}
			set
			{
				if (value != null && base.Document != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.SetTextSelection), value);
				}
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0013F840 File Offset: 0x0013E840
		bool IFlowDocumentViewer.CanGoToPreviousPage
		{
			get
			{
				return this.CanGoToPreviousPage;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0013F848 File Offset: 0x0013E848
		bool IFlowDocumentViewer.CanGoToNextPage
		{
			get
			{
				return this.CanGoToNextPage;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0013F850 File Offset: 0x0013E850
		int IFlowDocumentViewer.PageNumber
		{
			get
			{
				return this.MasterPageNumber;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0013F858 File Offset: 0x0013E858
		int IFlowDocumentViewer.PageCount
		{
			get
			{
				return base.PageCount;
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06001046 RID: 4166 RVA: 0x0013F860 File Offset: 0x0013E860
		// (remove) Token: 0x06001047 RID: 4167 RVA: 0x0013F879 File Offset: 0x0013E879
		event EventHandler IFlowDocumentViewer.PageNumberChanged
		{
			add
			{
				this._pageNumberChanged = (EventHandler)Delegate.Combine(this._pageNumberChanged, value);
			}
			remove
			{
				this._pageNumberChanged = (EventHandler)Delegate.Remove(this._pageNumberChanged, value);
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06001048 RID: 4168 RVA: 0x0013F892 File Offset: 0x0013E892
		// (remove) Token: 0x06001049 RID: 4169 RVA: 0x0013F8AB File Offset: 0x0013E8AB
		event EventHandler IFlowDocumentViewer.PageCountChanged
		{
			add
			{
				this._pageCountChanged = (EventHandler)Delegate.Combine(this._pageCountChanged, value);
			}
			remove
			{
				this._pageCountChanged = (EventHandler)Delegate.Remove(this._pageCountChanged, value);
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600104A RID: 4170 RVA: 0x0013F8C4 File Offset: 0x0013E8C4
		// (remove) Token: 0x0600104B RID: 4171 RVA: 0x0013F8DD File Offset: 0x0013E8DD
		event EventHandler IFlowDocumentViewer.PrintStarted
		{
			add
			{
				this._printStarted = (EventHandler)Delegate.Combine(this._printStarted, value);
			}
			remove
			{
				this._printStarted = (EventHandler)Delegate.Remove(this._printStarted, value);
			}
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600104C RID: 4172 RVA: 0x0013F8F6 File Offset: 0x0013E8F6
		// (remove) Token: 0x0600104D RID: 4173 RVA: 0x0013F90F File Offset: 0x0013E90F
		event EventHandler IFlowDocumentViewer.PrintCompleted
		{
			add
			{
				this._printCompleted = (EventHandler)Delegate.Combine(this._printCompleted, value);
			}
			remove
			{
				this._printCompleted = (EventHandler)Delegate.Remove(this._printCompleted, value);
			}
		}

		// Token: 0x04000AB9 RID: 2745
		private EventHandler _pageNumberChanged;

		// Token: 0x04000ABA RID: 2746
		private EventHandler _pageCountChanged;

		// Token: 0x04000ABB RID: 2747
		private EventHandler _printCompleted;

		// Token: 0x04000ABC RID: 2748
		private EventHandler _printStarted;

		// Token: 0x04000ABD RID: 2749
		private bool _raisePageNumberChanged;

		// Token: 0x04000ABE RID: 2750
		private bool _raisePageCountChanged;
	}
}
