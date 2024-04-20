using System;
using System.Printing;
using System.Windows.Xps.Serialization;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006EF RID: 1775
	public class WritingPrintTicketRequiredEventArgs : EventArgs
	{
		// Token: 0x06005D48 RID: 23880 RVA: 0x0028C838 File Offset: 0x0028B838
		public WritingPrintTicketRequiredEventArgs(PrintTicketLevel printTicketLevel, int sequence)
		{
			this._printTicketLevel = printTicketLevel;
			this._sequence = sequence;
		}

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x06005D49 RID: 23881 RVA: 0x0028C84E File Offset: 0x0028B84E
		public PrintTicketLevel CurrentPrintTicketLevel
		{
			get
			{
				return this._printTicketLevel;
			}
		}

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x06005D4A RID: 23882 RVA: 0x0028C856 File Offset: 0x0028B856
		public int Sequence
		{
			get
			{
				return this._sequence;
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x06005D4C RID: 23884 RVA: 0x0028C867 File Offset: 0x0028B867
		// (set) Token: 0x06005D4B RID: 23883 RVA: 0x0028C85E File Offset: 0x0028B85E
		public PrintTicket CurrentPrintTicket
		{
			get
			{
				return this._printTicket;
			}
			set
			{
				this._printTicket = value;
			}
		}

		// Token: 0x04003155 RID: 12629
		private PrintTicketLevel _printTicketLevel;

		// Token: 0x04003156 RID: 12630
		private int _sequence;

		// Token: 0x04003157 RID: 12631
		private PrintTicket _printTicket;
	}
}
