using System;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x020006FF RID: 1791
	internal class RetriesExhaustedException : Exception
	{
		// Token: 0x06005DC9 RID: 24009 RVA: 0x0028DE8D File Offset: 0x0028CE8D
		internal RetriesExhaustedException()
		{
		}

		// Token: 0x06005DCA RID: 24010 RVA: 0x0028DE95 File Offset: 0x0028CE95
		internal RetriesExhaustedException(string message) : base(message)
		{
		}

		// Token: 0x06005DCB RID: 24011 RVA: 0x0028DE9E File Offset: 0x0028CE9E
		internal RetriesExhaustedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
