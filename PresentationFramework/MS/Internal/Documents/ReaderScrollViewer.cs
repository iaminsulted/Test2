using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020001CD RID: 461
	internal class ReaderScrollViewer : FlowDocumentScrollViewer, IFlowDocumentViewer
	{
		// Token: 0x0600100F RID: 4111 RVA: 0x0013F292 File Offset: 0x0013E292
		protected override void OnPrintCompleted()
		{
			base.OnPrintCompleted();
			if (this._printCompleted != null)
			{
				this._printCompleted(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0013F2B3 File Offset: 0x0013E2B3
		protected override void OnPrintCommand()
		{
			base.OnPrintCommand();
			if (this._printStarted != null && base.IsPrinting)
			{
				this._printStarted(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0013F2DC File Offset: 0x0013E2DC
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == FlowDocumentScrollViewer.DocumentProperty)
			{
				if (this._pageNumberChanged != null)
				{
					this._pageNumberChanged(this, EventArgs.Empty);
				}
				if (this._pageCountChanged != null)
				{
					this._pageCountChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0013F330 File Offset: 0x0013E330
		private bool IsValidTextSelectionForDocument(ITextSelection textSelection, FlowDocument flowDocument)
		{
			return textSelection.Start != null && textSelection.Start.TextContainer == flowDocument.StructuralCache.TextContainer;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0013F358 File Offset: 0x0013E358
		private object SetTextSelection(object arg)
		{
			ITextSelection textSelection = arg as ITextSelection;
			if (textSelection != null && base.Document != null && this.IsValidTextSelectionForDocument(textSelection, base.Document))
			{
				ITextSelection textSelection2 = base.Document.StructuralCache.TextContainer.TextSelection;
				if (textSelection2 != null)
				{
					textSelection2.SetCaretToPosition(textSelection.AnchorPosition, textSelection.MovingPosition.LogicalDirection, true, true);
					textSelection2.ExtendToPosition(textSelection.MovingPosition);
				}
			}
			return null;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0013F3C5 File Offset: 0x0013E3C5
		void IFlowDocumentViewer.PreviousPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.PageUp();
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0013F3DA File Offset: 0x0013E3DA
		void IFlowDocumentViewer.NextPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.PageDown();
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0013F3EF File Offset: 0x0013E3EF
		void IFlowDocumentViewer.FirstPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0013F404 File Offset: 0x0013E404
		void IFlowDocumentViewer.LastPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToEnd();
			}
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0013F419 File Offset: 0x0013E419
		void IFlowDocumentViewer.Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0013F421 File Offset: 0x0013E421
		void IFlowDocumentViewer.CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IFlowDocumentViewer.ShowFindResult(ITextRange findResult)
		{
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0013F429 File Offset: 0x0013E429
		bool IFlowDocumentViewer.CanGoToPage(int pageNumber)
		{
			return pageNumber == 1;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0013F42F File Offset: 0x0013E42F
		void IFlowDocumentViewer.GoToPage(int pageNumber)
		{
			if (pageNumber == 1 && base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0013F448 File Offset: 0x0013E448
		void IFlowDocumentViewer.SetDocument(FlowDocument document)
		{
			base.Document = document;
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0013F451 File Offset: 0x0013E451
		// (set) Token: 0x0600101F RID: 4127 RVA: 0x0013F459 File Offset: 0x0013E459
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

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0013F480 File Offset: 0x0013E480
		// (set) Token: 0x06001021 RID: 4129 RVA: 0x0013F488 File Offset: 0x0013E488
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

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IFlowDocumentViewer.CanGoToPreviousPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IFlowDocumentViewer.CanGoToNextPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x0013F4AF File Offset: 0x0013E4AF
		int IFlowDocumentViewer.PageNumber
		{
			get
			{
				if (base.Document == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0013F4AF File Offset: 0x0013E4AF
		int IFlowDocumentViewer.PageCount
		{
			get
			{
				if (base.Document == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06001026 RID: 4134 RVA: 0x0013F4BC File Offset: 0x0013E4BC
		// (remove) Token: 0x06001027 RID: 4135 RVA: 0x0013F4D5 File Offset: 0x0013E4D5
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

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06001028 RID: 4136 RVA: 0x0013F4EE File Offset: 0x0013E4EE
		// (remove) Token: 0x06001029 RID: 4137 RVA: 0x0013F507 File Offset: 0x0013E507
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

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600102A RID: 4138 RVA: 0x0013F520 File Offset: 0x0013E520
		// (remove) Token: 0x0600102B RID: 4139 RVA: 0x0013F539 File Offset: 0x0013E539
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

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600102C RID: 4140 RVA: 0x0013F552 File Offset: 0x0013E552
		// (remove) Token: 0x0600102D RID: 4141 RVA: 0x0013F56B File Offset: 0x0013E56B
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

		// Token: 0x04000AB5 RID: 2741
		private EventHandler _pageNumberChanged;

		// Token: 0x04000AB6 RID: 2742
		private EventHandler _pageCountChanged;

		// Token: 0x04000AB7 RID: 2743
		private EventHandler _printCompleted;

		// Token: 0x04000AB8 RID: 2744
		private EventHandler _printStarted;
	}
}
