using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace Standard
{
	// Token: 0x0200000C RID: 12
	internal sealed class MessageWindow : CriticalFinalizerObject
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000F753D File Offset: 0x000F653D
		// (set) Token: 0x0600004B RID: 75 RVA: 0x000F7545 File Offset: 0x000F6545
		public IntPtr Handle { get; private set; }

		// Token: 0x0600004C RID: 76 RVA: 0x000F7550 File Offset: 0x000F6550
		public MessageWindow(CS classStyle, WS style, WS_EX exStyle, Rect location, string name, WndProc callback)
		{
			this._wndProcCallback = callback;
			this._className = "MessageWindowClass+" + Guid.NewGuid().ToString();
			WNDCLASSEX wndclassex = new WNDCLASSEX
			{
				cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
				style = classStyle,
				lpfnWndProc = MessageWindow.s_WndProc,
				hInstance = NativeMethods.GetModuleHandle(null),
				hbrBackground = NativeMethods.GetStockObject(StockObject.NULL_BRUSH),
				lpszMenuName = "",
				lpszClassName = this._className
			};
			NativeMethods.RegisterClassEx(ref wndclassex);
			GCHandle value = default(GCHandle);
			try
			{
				value = GCHandle.Alloc(this);
				IntPtr lpParam = (IntPtr)value;
				this.Handle = NativeMethods.CreateWindowEx(exStyle, this._className, name, style, (int)location.X, (int)location.Y, (int)location.Width, (int)location.Height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, lpParam);
			}
			finally
			{
				value.Free();
			}
			this._dispatcher = Dispatcher.CurrentDispatcher;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000F7680 File Offset: 0x000F6680
		~MessageWindow()
		{
			this._Dispose(false, false);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000F76B0 File Offset: 0x000F66B0
		public void Release()
		{
			this._Dispose(true, false);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000F76C0 File Offset: 0x000F66C0
		private void _Dispose(bool disposing, bool isHwndBeingDestroyed)
		{
			if (this._isDisposed)
			{
				return;
			}
			this._isDisposed = true;
			IntPtr handle = this.Handle;
			string className = this._className;
			if (isHwndBeingDestroyed)
			{
				this._dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this._DestroyWindowCallback), new object[]
				{
					IntPtr.Zero,
					className
				});
			}
			else if (this.Handle != IntPtr.Zero)
			{
				if (this._dispatcher.CheckAccess())
				{
					MessageWindow._DestroyWindow(handle, className);
				}
				else
				{
					this._dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this._DestroyWindowCallback), new object[]
					{
						handle,
						className
					});
				}
			}
			MessageWindow.s_windowLookup.Remove(handle);
			this._className = null;
			this.Handle = IntPtr.Zero;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000F7794 File Offset: 0x000F6794
		private object _DestroyWindowCallback(object arg)
		{
			object[] array = (object[])arg;
			MessageWindow._DestroyWindow((IntPtr)array[0], (string)array[1]);
			return null;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000F77C0 File Offset: 0x000F67C0
		private static IntPtr _WndProc(IntPtr hwnd, WM msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr result = IntPtr.Zero;
			MessageWindow messageWindow = null;
			if (msg == WM.CREATE)
			{
				messageWindow = (MessageWindow)GCHandle.FromIntPtr(((CREATESTRUCT)Marshal.PtrToStructure(lParam, typeof(CREATESTRUCT))).lpCreateParams).Target;
				MessageWindow.s_windowLookup.Add(hwnd, messageWindow);
			}
			else if (!MessageWindow.s_windowLookup.TryGetValue(hwnd, out messageWindow))
			{
				return NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
			}
			WndProc wndProcCallback = messageWindow._wndProcCallback;
			if (wndProcCallback != null)
			{
				result = wndProcCallback(hwnd, msg, wParam, lParam);
			}
			else
			{
				result = NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam);
			}
			if (msg == WM.NCDESTROY)
			{
				messageWindow._Dispose(true, true);
				GC.SuppressFinalize(messageWindow);
			}
			return result;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000F7865 File Offset: 0x000F6865
		private static void _DestroyWindow(IntPtr hwnd, string className)
		{
			Utility.SafeDestroyWindow(ref hwnd);
			NativeMethods.UnregisterClass(className, NativeMethods.GetModuleHandle(null));
		}

		// Token: 0x04000059 RID: 89
		private static readonly WndProc s_WndProc = new WndProc(MessageWindow._WndProc);

		// Token: 0x0400005A RID: 90
		private static readonly Dictionary<IntPtr, MessageWindow> s_windowLookup = new Dictionary<IntPtr, MessageWindow>();

		// Token: 0x0400005B RID: 91
		private WndProc _wndProcCallback;

		// Token: 0x0400005C RID: 92
		private string _className;

		// Token: 0x0400005D RID: 93
		private bool _isDisposed;

		// Token: 0x0400005E RID: 94
		private Dispatcher _dispatcher;
	}
}
