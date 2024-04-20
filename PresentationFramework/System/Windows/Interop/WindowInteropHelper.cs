using System;
using MS.Internal;

namespace System.Windows.Interop
{
	// Token: 0x02000425 RID: 1061
	public sealed class WindowInteropHelper
	{
		// Token: 0x06003355 RID: 13141 RVA: 0x001D4992 File Offset: 0x001D3992
		public WindowInteropHelper(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			this._window = window;
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003356 RID: 13142 RVA: 0x001D49AF File Offset: 0x001D39AF
		public IntPtr Handle
		{
			get
			{
				return this.CriticalHandle;
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06003357 RID: 13143 RVA: 0x001D49B7 File Offset: 0x001D39B7
		internal IntPtr CriticalHandle
		{
			get
			{
				Invariant.Assert(this._window != null, "Cannot be null since we verify in the constructor");
				return this._window.CriticalHandle;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003358 RID: 13144 RVA: 0x001D49D7 File Offset: 0x001D39D7
		// (set) Token: 0x06003359 RID: 13145 RVA: 0x001D49E4 File Offset: 0x001D39E4
		public IntPtr Owner
		{
			get
			{
				return this._window.OwnerHandle;
			}
			set
			{
				this._window.OwnerHandle = value;
			}
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x001D49F2 File Offset: 0x001D39F2
		public IntPtr EnsureHandle()
		{
			if (this.CriticalHandle == IntPtr.Zero)
			{
				this._window.CreateSourceWindow(false);
			}
			return this.CriticalHandle;
		}

		// Token: 0x04001C23 RID: 7203
		private Window _window;
	}
}
