using System;
using System.Runtime.Serialization;

namespace System.Windows
{
	// Token: 0x02000393 RID: 915
	[Serializable]
	public class ResourceReferenceKeyNotFoundException : InvalidOperationException
	{
		// Token: 0x06002511 RID: 9489 RVA: 0x0018591C File Offset: 0x0018491C
		public ResourceReferenceKeyNotFoundException()
		{
			this._resourceKey = null;
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x0018592B File Offset: 0x0018492B
		public ResourceReferenceKeyNotFoundException(string message, object resourceKey) : base(message)
		{
			this._resourceKey = resourceKey;
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x0018593B File Offset: 0x0018493B
		protected ResourceReferenceKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._resourceKey = info.GetValue("Key", typeof(object));
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002514 RID: 9492 RVA: 0x00185960 File Offset: 0x00184960
		public object Key
		{
			get
			{
				return this._resourceKey;
			}
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x00185968 File Offset: 0x00184968
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Key", this._resourceKey);
		}

		// Token: 0x04001174 RID: 4468
		private object _resourceKey;
	}
}
