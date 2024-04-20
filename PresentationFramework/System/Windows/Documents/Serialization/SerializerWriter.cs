using System;
using System.Printing;
using System.Windows.Media;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006EC RID: 1772
	public abstract class SerializerWriter
	{
		// Token: 0x06005D13 RID: 23827
		public abstract void Write(Visual visual);

		// Token: 0x06005D14 RID: 23828
		public abstract void Write(Visual visual, PrintTicket printTicket);

		// Token: 0x06005D15 RID: 23829
		public abstract void WriteAsync(Visual visual);

		// Token: 0x06005D16 RID: 23830
		public abstract void WriteAsync(Visual visual, object userState);

		// Token: 0x06005D17 RID: 23831
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket);

		// Token: 0x06005D18 RID: 23832
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket, object userState);

		// Token: 0x06005D19 RID: 23833
		public abstract void Write(DocumentPaginator documentPaginator);

		// Token: 0x06005D1A RID: 23834
		public abstract void Write(DocumentPaginator documentPaginator, PrintTicket printTicket);

		// Token: 0x06005D1B RID: 23835
		public abstract void WriteAsync(DocumentPaginator documentPaginator);

		// Token: 0x06005D1C RID: 23836
		public abstract void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket);

		// Token: 0x06005D1D RID: 23837
		public abstract void WriteAsync(DocumentPaginator documentPaginator, object userState);

		// Token: 0x06005D1E RID: 23838
		public abstract void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket, object userState);

		// Token: 0x06005D1F RID: 23839
		public abstract void Write(FixedPage fixedPage);

		// Token: 0x06005D20 RID: 23840
		public abstract void Write(FixedPage fixedPage, PrintTicket printTicket);

		// Token: 0x06005D21 RID: 23841
		public abstract void WriteAsync(FixedPage fixedPage);

		// Token: 0x06005D22 RID: 23842
		public abstract void WriteAsync(FixedPage fixedPage, PrintTicket printTicket);

		// Token: 0x06005D23 RID: 23843
		public abstract void WriteAsync(FixedPage fixedPage, object userState);

		// Token: 0x06005D24 RID: 23844
		public abstract void WriteAsync(FixedPage fixedPage, PrintTicket printTicket, object userState);

		// Token: 0x06005D25 RID: 23845
		public abstract void Write(FixedDocument fixedDocument);

		// Token: 0x06005D26 RID: 23846
		public abstract void Write(FixedDocument fixedDocument, PrintTicket printTicket);

		// Token: 0x06005D27 RID: 23847
		public abstract void WriteAsync(FixedDocument fixedDocument);

		// Token: 0x06005D28 RID: 23848
		public abstract void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket);

		// Token: 0x06005D29 RID: 23849
		public abstract void WriteAsync(FixedDocument fixedDocument, object userState);

		// Token: 0x06005D2A RID: 23850
		public abstract void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket, object userState);

		// Token: 0x06005D2B RID: 23851
		public abstract void Write(FixedDocumentSequence fixedDocumentSequence);

		// Token: 0x06005D2C RID: 23852
		public abstract void Write(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket);

		// Token: 0x06005D2D RID: 23853
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence);

		// Token: 0x06005D2E RID: 23854
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket);

		// Token: 0x06005D2F RID: 23855
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence, object userState);

		// Token: 0x06005D30 RID: 23856
		public abstract void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket, object userState);

		// Token: 0x06005D31 RID: 23857
		public abstract void CancelAsync();

		// Token: 0x06005D32 RID: 23858
		public abstract SerializerWriterCollator CreateVisualsCollator();

		// Token: 0x06005D33 RID: 23859
		public abstract SerializerWriterCollator CreateVisualsCollator(PrintTicket documentSequencePT, PrintTicket documentPT);

		// Token: 0x140000D2 RID: 210
		// (add) Token: 0x06005D34 RID: 23860
		// (remove) Token: 0x06005D35 RID: 23861
		public abstract event WritingPrintTicketRequiredEventHandler WritingPrintTicketRequired;

		// Token: 0x140000D3 RID: 211
		// (add) Token: 0x06005D36 RID: 23862
		// (remove) Token: 0x06005D37 RID: 23863
		public abstract event WritingProgressChangedEventHandler WritingProgressChanged;

		// Token: 0x140000D4 RID: 212
		// (add) Token: 0x06005D38 RID: 23864
		// (remove) Token: 0x06005D39 RID: 23865
		public abstract event WritingCompletedEventHandler WritingCompleted;

		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06005D3A RID: 23866
		// (remove) Token: 0x06005D3B RID: 23867
		public abstract event WritingCancelledEventHandler WritingCancelled;
	}
}
