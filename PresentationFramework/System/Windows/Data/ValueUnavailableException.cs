using System;
using System.Runtime.Serialization;

namespace System.Windows.Data
{
	// Token: 0x0200046B RID: 1131
	[Serializable]
	public class ValueUnavailableException : SystemException
	{
		// Token: 0x06003A2D RID: 14893 RVA: 0x001EFBCA File Offset: 0x001EEBCA
		public ValueUnavailableException()
		{
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x001EFBD2 File Offset: 0x001EEBD2
		public ValueUnavailableException(string message) : base(message)
		{
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x001EFBDB File Offset: 0x001EEBDB
		public ValueUnavailableException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x001EFBE5 File Offset: 0x001EEBE5
		protected ValueUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
