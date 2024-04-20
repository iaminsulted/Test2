using System;
using System.Runtime.InteropServices;

namespace Helper
{
	// Token: 0x02000036 RID: 54
	public class SmartGCHandle : IDisposable
	{
		// Token: 0x060001EA RID: 490 RVA: 0x00016A59 File Offset: 0x00014C59
		public SmartGCHandle(GCHandle handle)
		{
			this.handle = handle;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00016A68 File Offset: 0x00014C68
		~SmartGCHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00016A98 File Offset: 0x00014C98
		public IntPtr AddrOfPinnedObject()
		{
			return this.handle.AddrOfPinnedObject();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00016AA5 File Offset: 0x00014CA5
		public virtual void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00016AAE File Offset: 0x00014CAE
		protected virtual void Dispose(bool disposing)
		{
			this.handle.Free();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00016ABB File Offset: 0x00014CBB
		public static implicit operator GCHandle(SmartGCHandle other)
		{
			return other.handle;
		}

		// Token: 0x040001F2 RID: 498
		private GCHandle handle;
	}
}
