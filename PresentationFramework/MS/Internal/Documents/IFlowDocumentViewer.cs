using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001CC RID: 460
	internal interface IFlowDocumentViewer
	{
		// Token: 0x06000FF5 RID: 4085
		void PreviousPage();

		// Token: 0x06000FF6 RID: 4086
		void NextPage();

		// Token: 0x06000FF7 RID: 4087
		void FirstPage();

		// Token: 0x06000FF8 RID: 4088
		void LastPage();

		// Token: 0x06000FF9 RID: 4089
		void Print();

		// Token: 0x06000FFA RID: 4090
		void CancelPrint();

		// Token: 0x06000FFB RID: 4091
		void ShowFindResult(ITextRange findResult);

		// Token: 0x06000FFC RID: 4092
		bool CanGoToPage(int pageNumber);

		// Token: 0x06000FFD RID: 4093
		void GoToPage(int pageNumber);

		// Token: 0x06000FFE RID: 4094
		void SetDocument(FlowDocument document);

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000FFF RID: 4095
		// (set) Token: 0x06001000 RID: 4096
		ContentPosition ContentPosition { get; set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001001 RID: 4097
		// (set) Token: 0x06001002 RID: 4098
		ITextSelection TextSelection { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001003 RID: 4099
		bool CanGoToPreviousPage { get; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001004 RID: 4100
		bool CanGoToNextPage { get; }

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001005 RID: 4101
		int PageNumber { get; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001006 RID: 4102
		int PageCount { get; }

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06001007 RID: 4103
		// (remove) Token: 0x06001008 RID: 4104
		event EventHandler PageNumberChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06001009 RID: 4105
		// (remove) Token: 0x0600100A RID: 4106
		event EventHandler PageCountChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600100B RID: 4107
		// (remove) Token: 0x0600100C RID: 4108
		event EventHandler PrintStarted;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600100D RID: 4109
		// (remove) Token: 0x0600100E RID: 4110
		event EventHandler PrintCompleted;
	}
}
