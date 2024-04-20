using System;
using System.Runtime.Serialization;

namespace System.Windows.Controls
{
	// Token: 0x020007C4 RID: 1988
	[Serializable]
	public class PrintDialogException : Exception
	{
		// Token: 0x060071D8 RID: 29144 RVA: 0x0028DE8D File Offset: 0x0028CE8D
		public PrintDialogException()
		{
		}

		// Token: 0x060071D9 RID: 29145 RVA: 0x0028DE95 File Offset: 0x0028CE95
		public PrintDialogException(string message) : base(message)
		{
		}

		// Token: 0x060071DA RID: 29146 RVA: 0x0028DE9E File Offset: 0x0028CE9E
		public PrintDialogException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x060071DB RID: 29147 RVA: 0x002DBDDD File Offset: 0x002DADDD
		protected PrintDialogException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
