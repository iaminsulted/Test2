using System;
using System.Printing;
using System.Windows.Media;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006ED RID: 1773
	public abstract class SerializerWriterCollator
	{
		// Token: 0x06005D3D RID: 23869
		public abstract void BeginBatchWrite();

		// Token: 0x06005D3E RID: 23870
		public abstract void EndBatchWrite();

		// Token: 0x06005D3F RID: 23871
		public abstract void Write(Visual visual);

		// Token: 0x06005D40 RID: 23872
		public abstract void Write(Visual visual, PrintTicket printTicket);

		// Token: 0x06005D41 RID: 23873
		public abstract void WriteAsync(Visual visual);

		// Token: 0x06005D42 RID: 23874
		public abstract void WriteAsync(Visual visual, object userState);

		// Token: 0x06005D43 RID: 23875
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket);

		// Token: 0x06005D44 RID: 23876
		public abstract void WriteAsync(Visual visual, PrintTicket printTicket, object userState);

		// Token: 0x06005D45 RID: 23877
		public abstract void CancelAsync();

		// Token: 0x06005D46 RID: 23878
		public abstract void Cancel();
	}
}
