using System;

namespace WebSocketSharp
{
	// Token: 0x0200000F RID: 15
	public class WebSocketException : Exception
	{
		// Token: 0x06000128 RID: 296 RVA: 0x000093DC File Offset: 0x000075DC
		internal WebSocketException() : this(CloseStatusCode.Abnormal, null, null)
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000093ED File Offset: 0x000075ED
		internal WebSocketException(Exception innerException) : this(CloseStatusCode.Abnormal, null, innerException)
		{
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000093FE File Offset: 0x000075FE
		internal WebSocketException(string message) : this(CloseStatusCode.Abnormal, message, null)
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000940F File Offset: 0x0000760F
		internal WebSocketException(CloseStatusCode code) : this(code, null, null)
		{
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000941C File Offset: 0x0000761C
		internal WebSocketException(string message, Exception innerException) : this(CloseStatusCode.Abnormal, message, innerException)
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000942D File Offset: 0x0000762D
		internal WebSocketException(CloseStatusCode code, Exception innerException) : this(code, null, innerException)
		{
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000943A File Offset: 0x0000763A
		internal WebSocketException(CloseStatusCode code, string message) : this(code, message, null)
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00009447 File Offset: 0x00007647
		internal WebSocketException(CloseStatusCode code, string message, Exception innerException) : base(message ?? code.GetMessage(), innerException)
		{
			this._code = code;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00009464 File Offset: 0x00007664
		public CloseStatusCode Code
		{
			get
			{
				return this._code;
			}
		}

		// Token: 0x0400006F RID: 111
		private CloseStatusCode _code;
	}
}
