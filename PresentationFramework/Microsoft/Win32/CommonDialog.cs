using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using MS.Win32;

namespace Microsoft.Win32
{
	// Token: 0x020000E1 RID: 225
	public abstract class CommonDialog
	{
		// Token: 0x0600039C RID: 924
		public abstract void Reset();

		// Token: 0x0600039D RID: 925 RVA: 0x000FD85C File Offset: 0x000FC85C
		public virtual bool? ShowDialog()
		{
			this.CheckPermissionsToShowDialog();
			if (!Environment.UserInteractive)
			{
				throw new InvalidOperationException(SR.Get("CantShowModalOnNonInteractive"));
			}
			IntPtr intPtr = UnsafeNativeMethods.GetActiveWindow();
			if (intPtr == IntPtr.Zero && Application.Current != null)
			{
				intPtr = Application.Current.ParkingHwnd;
			}
			HwndWrapper hwndWrapper = null;
			bool? result;
			try
			{
				if (intPtr == IntPtr.Zero)
				{
					hwndWrapper = new HwndWrapper(0, 0, 0, 0, 0, 0, 0, "", IntPtr.Zero, null);
					intPtr = hwndWrapper.Handle;
				}
				this._hwndOwnerWindow = intPtr;
				try
				{
					ComponentDispatcher.CriticalPushModal();
					result = new bool?(this.RunDialog(intPtr));
				}
				finally
				{
					ComponentDispatcher.CriticalPopModal();
				}
			}
			finally
			{
				if (hwndWrapper != null)
				{
					hwndWrapper.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000FD924 File Offset: 0x000FC924
		public bool? ShowDialog(Window owner)
		{
			this.CheckPermissionsToShowDialog();
			if (owner == null)
			{
				return this.ShowDialog();
			}
			if (!Environment.UserInteractive)
			{
				throw new InvalidOperationException(SR.Get("CantShowModalOnNonInteractive"));
			}
			IntPtr criticalHandle = new WindowInteropHelper(owner).CriticalHandle;
			if (criticalHandle == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			this._hwndOwnerWindow = criticalHandle;
			bool? result;
			try
			{
				ComponentDispatcher.CriticalPushModal();
				result = new bool?(this.RunDialog(criticalHandle));
			}
			finally
			{
				ComponentDispatcher.CriticalPopModal();
			}
			return result;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600039F RID: 927 RVA: 0x000FD9AC File Offset: 0x000FC9AC
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x000FD9B4 File Offset: 0x000FC9B4
		public object Tag
		{
			get
			{
				return this._userData;
			}
			set
			{
				this._userData = value;
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000FD9BD File Offset: 0x000FC9BD
		protected virtual IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			if (msg == 272)
			{
				this.MoveToScreenCenter(new HandleRef(this, hwnd));
				return new IntPtr(1);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060003A2 RID: 930
		protected abstract bool RunDialog(IntPtr hwndOwner);

		// Token: 0x060003A3 RID: 931 RVA: 0x000FD9E0 File Offset: 0x000FC9E0
		protected virtual void CheckPermissionsToShowDialog()
		{
			if (this._thread != Thread.CurrentThread)
			{
				throw new InvalidOperationException(SR.Get("CantShowOnDifferentThread"));
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000FDA00 File Offset: 0x000FCA00
		internal void MoveToScreenCenter(HandleRef hWnd)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (this._hwndOwnerWindow != IntPtr.Zero)
			{
				intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(this, this._hwndOwnerWindow), 2);
				if (intPtr != IntPtr.Zero)
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					SafeNativeMethods.GetWindowRect(hWnd, ref rect);
					Size currentSizeDeviceUnits = new Size((double)(rect.right - rect.left), (double)(rect.bottom - rect.top));
					double a = 0.0;
					double a2 = 0.0;
					Window.CalculateCenterScreenPosition(intPtr, currentSizeDeviceUnits, ref a, ref a2);
					UnsafeNativeMethods.SetWindowPos(hWnd, NativeMethods.NullHandleRef, (int)Math.Round(a), (int)Math.Round(a2), 0, 0, 21);
				}
			}
		}

		// Token: 0x040005F7 RID: 1527
		private object _userData;

		// Token: 0x040005F8 RID: 1528
		private Thread _thread = Thread.CurrentThread;

		// Token: 0x040005F9 RID: 1529
		private IntPtr _hwndOwnerWindow;
	}
}
