using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace MS.Internal.Documents.Application
{
	// Token: 0x020001FC RID: 508
	[Serializable]
	internal sealed class DocumentApplicationJournalEntry : CustomContentState
	{
		// Token: 0x060012AF RID: 4783 RVA: 0x0014B8C2 File Offset: 0x0014A8C2
		public DocumentApplicationJournalEntry(object state, string name)
		{
			Invariant.Assert(state is DocumentApplicationState, "state should be of type DocumentApplicationState");
			this._state = state;
			this._displayName = name;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0014B8EB File Offset: 0x0014A8EB
		public DocumentApplicationJournalEntry(object state) : this(state, null)
		{
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0014B8F8 File Offset: 0x0014A8F8
		public override void Replay(NavigationService navigationService, NavigationMode mode)
		{
			ContentControl contentControl = (ContentControl)navigationService.INavigatorHost;
			contentControl.ApplyTemplate();
			DocumentApplicationDocumentViewer documentApplicationDocumentViewer = contentControl.Template.FindName("PUIDocumentApplicationDocumentViewer", contentControl) as DocumentApplicationDocumentViewer;
			if (documentApplicationDocumentViewer != null)
			{
				if (this._state is DocumentApplicationState)
				{
					documentApplicationDocumentViewer.StoredDocumentApplicationState = (DocumentApplicationState)this._state;
				}
				if (navigationService.Content != null)
				{
					IDocumentPaginatorSource documentPaginatorSource = navigationService.Content as IDocumentPaginatorSource;
					if (documentPaginatorSource != null && documentPaginatorSource.DocumentPaginator.IsPageCountValid)
					{
						documentApplicationDocumentViewer.SetUIToStoredState();
					}
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060012B2 RID: 4786 RVA: 0x0014B97A File Offset: 0x0014A97A
		public override string JournalEntryName
		{
			get
			{
				return this._displayName;
			}
		}

		// Token: 0x04000B49 RID: 2889
		private object _state;

		// Token: 0x04000B4A RID: 2890
		private string _displayName;
	}
}
