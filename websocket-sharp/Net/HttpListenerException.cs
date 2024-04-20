using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace WebSocketSharp.Net
{
	// Token: 0x02000022 RID: 34
	[Serializable]
	public class HttpListenerException : Win32Exception
	{
		// Token: 0x06000281 RID: 641 RVA: 0x000102A7 File Offset: 0x0000E4A7
		protected HttpListenerException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000102B3 File Offset: 0x0000E4B3
		public HttpListenerException()
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000102BD File Offset: 0x0000E4BD
		public HttpListenerException(int errorCode) : base(errorCode)
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000102C8 File Offset: 0x0000E4C8
		public HttpListenerException(int errorCode, string message) : base(errorCode, message)
		{
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000285 RID: 645 RVA: 0x000102D4 File Offset: 0x0000E4D4
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}
	}
}
