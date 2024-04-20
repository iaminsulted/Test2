using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace WebSocketSharp.Net
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	public class CookieException : FormatException, ISerializable
	{
		// Token: 0x06000201 RID: 513 RVA: 0x0000D602 File Offset: 0x0000B802
		internal CookieException(string message) : base(message)
		{
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000D60D File Offset: 0x0000B80D
		internal CookieException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000D619 File Offset: 0x0000B819
		protected CookieException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000D625 File Offset: 0x0000B825
		public CookieException()
		{
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000D62F File Offset: 0x0000B82F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000D62F File Offset: 0x0000B82F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}
	}
}
