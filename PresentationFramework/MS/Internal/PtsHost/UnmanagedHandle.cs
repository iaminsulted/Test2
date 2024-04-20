using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000150 RID: 336
	internal class UnmanagedHandle : IDisposable
	{
		// Token: 0x06000AE9 RID: 2793 RVA: 0x0012BD1F File Offset: 0x0012AD1F
		protected UnmanagedHandle(PtsContext ptsContext)
		{
			this._ptsContext = ptsContext;
			this._handle = ptsContext.CreateHandle(this);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0012BD3C File Offset: 0x0012AD3C
		public virtual void Dispose()
		{
			try
			{
				this._ptsContext.ReleaseHandle(this._handle);
			}
			finally
			{
				this._handle = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0012BD80 File Offset: 0x0012AD80
		internal IntPtr Handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0012BD88 File Offset: 0x0012AD88
		internal PtsContext PtsContext
		{
			get
			{
				return this._ptsContext;
			}
		}

		// Token: 0x04000829 RID: 2089
		private IntPtr _handle;

		// Token: 0x0400082A RID: 2090
		private readonly PtsContext _ptsContext;
	}
}
