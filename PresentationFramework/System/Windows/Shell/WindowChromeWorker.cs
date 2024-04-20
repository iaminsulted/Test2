using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Standard;

namespace System.Windows.Shell
{
	// Token: 0x020003F7 RID: 1015
	internal class WindowChromeWorker : DependencyObject
	{
		// Token: 0x06002B9E RID: 11166 RVA: 0x001A311C File Offset: 0x001A211C
		public WindowChromeWorker()
		{
			this._messageTable = new List<KeyValuePair<WM, MessageHandler>>
			{
				new KeyValuePair<WM, MessageHandler>(WM.SETTEXT, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.SETICON, new MessageHandler(this._HandleSetTextOrIcon)),
				new KeyValuePair<WM, MessageHandler>(WM.NCACTIVATE, new MessageHandler(this._HandleNCActivate)),
				new KeyValuePair<WM, MessageHandler>(WM.NCCALCSIZE, new MessageHandler(this._HandleNCCalcSize)),
				new KeyValuePair<WM, MessageHandler>(WM.NCHITTEST, new MessageHandler(this._HandleNCHitTest)),
				new KeyValuePair<WM, MessageHandler>(WM.NCRBUTTONUP, new MessageHandler(this._HandleNCRButtonUp)),
				new KeyValuePair<WM, MessageHandler>(WM.SIZE, new MessageHandler(this._HandleSize)),
				new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGED, new MessageHandler(this._HandleWindowPosChanged)),
				new KeyValuePair<WM, MessageHandler>(WM.DWMCOMPOSITIONCHANGED, new MessageHandler(this._HandleDwmCompositionChanged))
			};
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				this._messageTable.AddRange(new KeyValuePair<WM, MessageHandler>[]
				{
					new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, new MessageHandler(this._HandleSettingChange)),
					new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMove)),
					new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMove)),
					new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMove))
				});
			}
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x001A32B4 File Offset: 0x001A22B4
		public void SetWindowChrome(WindowChrome newChrome)
		{
			base.VerifyAccess();
			if (newChrome == this._chromeInfo)
			{
				return;
			}
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._chromeInfo = newChrome;
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint += this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._ApplyNewCustomChrome();
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x001A331C File Offset: 0x001A231C
		private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
		{
			this._UpdateFrameState(true);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x001A3328 File Offset: 0x001A2328
		private static void _OnChromeWorkerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			((WindowChromeWorker)e.NewValue)._SetWindow(window);
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x001A3350 File Offset: 0x001A2350
		private void _SetWindow(Window window)
		{
			this.UnsubscribeWindowEvents();
			this._window = window;
			this._hwnd = new WindowInteropHelper(this._window).Handle;
			Utility.AddDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			Utility.AddDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
			this._window.Closed += this._UnsetWindow;
			if (IntPtr.Zero != this._hwnd)
			{
				this._hwndSource = HwndSource.FromHwnd(this._hwnd);
				this._window.ApplyTemplate();
				if (this._chromeInfo != null)
				{
					this._ApplyNewCustomChrome();
					return;
				}
			}
			else
			{
				this._window.SourceInitialized += this._WindowSourceInitialized;
			}
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x001A3424 File Offset: 0x001A2424
		private void _WindowSourceInitialized(object sender, EventArgs e)
		{
			this._hwnd = new WindowInteropHelper(this._window).Handle;
			this._hwndSource = HwndSource.FromHwnd(this._hwnd);
			if (this._chromeInfo != null)
			{
				this._ApplyNewCustomChrome();
			}
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x001A345C File Offset: 0x001A245C
		private void UnsubscribeWindowEvents()
		{
			if (this._window != null)
			{
				Utility.RemoveDependencyPropertyChangeListener(this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				Utility.RemoveDependencyPropertyChangeListener(this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
				this._window.SourceInitialized -= this._WindowSourceInitialized;
				this._window.StateChanged -= this._FixupRestoreBounds;
			}
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x001A34D7 File Offset: 0x001A24D7
		private void _UnsetWindow(object sender, EventArgs e)
		{
			this.UnsubscribeWindowEvents();
			if (this._chromeInfo != null)
			{
				this._chromeInfo.PropertyChangedThatRequiresRepaint -= this._OnChromePropertyChangedThatRequiresRepaint;
			}
			this._RestoreStandardChromeState(true);
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x001A3505 File Offset: 0x001A2505
		public static WindowChromeWorker GetWindowChromeWorker(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			return (WindowChromeWorker)window.GetValue(WindowChromeWorker.WindowChromeWorkerProperty);
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x001A3522 File Offset: 0x001A2522
		public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
		{
			Verify.IsNotNull<Window>(window, "window");
			window.SetValue(WindowChromeWorker.WindowChromeWorkerProperty, chrome);
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x001A353B File Offset: 0x001A253B
		private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
		{
			if (this._chromeInfo != null && this._hwnd != IntPtr.Zero)
			{
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new WindowChromeWorker._Action(this._FixupTemplateIssues));
			}
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x001A3578 File Offset: 0x001A2578
		private void _ApplyNewCustomChrome()
		{
			if (this._hwnd == IntPtr.Zero || this._hwndSource.IsDisposed)
			{
				return;
			}
			if (this._chromeInfo == null)
			{
				this._RestoreStandardChromeState(false);
				return;
			}
			if (!this._isHooked)
			{
				this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
				this._isHooked = true;
			}
			this._FixupTemplateIssues();
			this._UpdateSystemMenu(new WindowState?(this._window.WindowState));
			this._UpdateFrameState(true);
			NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x001A3618 File Offset: 0x001A2618
		private void RetryFixupTemplateIssuesOnVisualChildrenAdded(object sender, EventArgs e)
		{
			if (sender == this._window)
			{
				this._window.VisualChildrenChanged -= this.RetryFixupTemplateIssuesOnVisualChildrenAdded;
				this._window.Dispatcher.BeginInvoke(DispatcherPriority.Render, new WindowChromeWorker._Action(this._FixupTemplateIssues));
			}
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x001A3658 File Offset: 0x001A2658
		private void _FixupTemplateIssues()
		{
			if (this._window.Template == null)
			{
				return;
			}
			if (VisualTreeHelper.GetChildrenCount(this._window) == 0)
			{
				this._window.VisualChildrenChanged += this.RetryFixupTemplateIssuesOnVisualChildrenAdded;
				return;
			}
			Thickness margin = default(Thickness);
			FrameworkElement frameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(this._window, 0);
			if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
			{
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					margin.Top -= SystemParameters.WindowResizeBorderThickness.Top;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					margin.Left -= SystemParameters.WindowResizeBorderThickness.Left;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					margin.Right -= SystemParameters.WindowResizeBorderThickness.Right;
				}
			}
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				DpiScale dpi = this._window.GetDpi();
				RECT windowRect = NativeMethods.GetWindowRect(this._hwnd);
				RECT rect = this._GetAdjustedWindowRect(windowRect);
				Rect rect2 = DpiHelper.DeviceRectToLogical(new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height), dpi.DpiScaleX, dpi.DpiScaleY);
				Rect rect3 = DpiHelper.DeviceRectToLogical(new Rect((double)rect.Left, (double)rect.Top, (double)rect.Width, (double)rect.Height), dpi.DpiScaleX, dpi.DpiScaleY);
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					margin.Right -= SystemParameters.WindowResizeBorderThickness.Left;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					margin.Right -= SystemParameters.WindowResizeBorderThickness.Right;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Top;
				}
				if (!Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					margin.Bottom -= SystemParameters.WindowResizeBorderThickness.Bottom;
				}
				margin.Bottom -= SystemParameters.WindowCaptionHeight;
				Transform renderTransform;
				if (this._window.FlowDirection == FlowDirection.RightToLeft)
				{
					Thickness thickness = new Thickness(rect2.Left - rect3.Left, rect2.Top - rect3.Top, rect3.Right - rect2.Right, rect3.Bottom - rect2.Bottom);
					renderTransform = new MatrixTransform(1.0, 0.0, 0.0, 1.0, -(thickness.Left + thickness.Right), 0.0);
				}
				else
				{
					renderTransform = null;
				}
				frameworkElement.RenderTransform = renderTransform;
			}
			frameworkElement.Margin = margin;
			if (Utility.IsPresentationFrameworkVersionLessThan4 && !this._isFixedUp)
			{
				this._hasUserMovedWindow = false;
				this._window.StateChanged += this._FixupRestoreBounds;
				this._isFixedUp = true;
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x001A39C8 File Offset: 0x001A29C8
		private void _FixupRestoreBounds(object sender, EventArgs e)
		{
			if ((this._window.WindowState == WindowState.Maximized || this._window.WindowState == WindowState.Minimized) && this._hasUserMovedWindow)
			{
				DpiScale dpi = this._window.GetDpi();
				this._hasUserMovedWindow = false;
				WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(this._hwnd);
				RECT rect = this._GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = DpiHelper.DevicePixelsToLogical(new Point((double)(windowPlacement.rcNormalPosition.Left - rect.Left), (double)(windowPlacement.rcNormalPosition.Top - rect.Top)), dpi.DpiScaleX, dpi.DpiScaleY);
				this._window.Top = point.Y;
				this._window.Left = point.X;
			}
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x001A3AA8 File Offset: 0x001A2AA8
		private RECT _GetAdjustedWindowRect(RECT rcWindow)
		{
			WS dwStyle = (WS)((int)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE));
			WS_EX dwExStyle = (WS_EX)((int)NativeMethods.GetWindowLongPtr(this._hwnd, GWL.EXSTYLE));
			return NativeMethods.AdjustWindowRectEx(rcWindow, dwStyle, false, dwExStyle);
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x001A3AE4 File Offset: 0x001A2AE4
		private bool _IsWindowDocked
		{
			get
			{
				if (this._window.WindowState != WindowState.Normal)
				{
					return false;
				}
				DpiScale dpi = this._window.GetDpi();
				RECT rect = this._GetAdjustedWindowRect(new RECT
				{
					Bottom = 100,
					Right = 100
				});
				Point point = new Point(this._window.Left, this._window.Top);
				point -= (Vector)DpiHelper.DevicePixelsToLogical(new Point((double)rect.Left, (double)rect.Top), dpi.DpiScaleX, dpi.DpiScaleY);
				return this._window.RestoreBounds.Location != point;
			}
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x001A3B9C File Offset: 0x001A2B9C
		private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			foreach (KeyValuePair<WM, MessageHandler> keyValuePair in this._messageTable)
			{
				if (keyValuePair.Key == (WM)msg)
				{
					return keyValuePair.Value((WM)msg, wParam, lParam, out handled);
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x001A3C10 File Offset: 0x001A2C10
		private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
			IntPtr result = NativeMethods.DefWindowProc(this._hwnd, uMsg, wParam, lParam);
			if (flag)
			{
				this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
			}
			handled = true;
			return result;
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x001A3C4C File Offset: 0x001A2C4C
		private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			IntPtr result = NativeMethods.DefWindowProc(this._hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
			handled = true;
			return result;
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x001A3C6C File Offset: 0x001A2C6C
		private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
			{
				DpiScale dpi = this._window.GetDpi();
				Thickness thickness = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness, dpi.DpiScaleX, dpi.DpiScaleY);
				RECT structure = (RECT)Marshal.PtrToStructure(lParam, typeof(RECT));
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					structure.Top += (int)thickness.Top;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					structure.Left += (int)thickness.Left;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					structure.Bottom -= (int)thickness.Bottom;
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					structure.Right -= (int)thickness.Right;
				}
				Marshal.StructureToPtr<RECT>(structure, lParam, false);
			}
			handled = true;
			IntPtr zero = IntPtr.Zero;
			if (wParam.ToInt32() != 0)
			{
				zero = new IntPtr(768);
			}
			return zero;
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x001A3D90 File Offset: 0x001A2D90
		private HT _GetHTFromResizeGripDirection(ResizeGripDirection direction)
		{
			bool flag = this._window.FlowDirection == FlowDirection.RightToLeft;
			switch (direction)
			{
			case ResizeGripDirection.TopLeft:
				if (!flag)
				{
					return HT.TOPLEFT;
				}
				return HT.TOPRIGHT;
			case ResizeGripDirection.Top:
				return HT.TOP;
			case ResizeGripDirection.TopRight:
				if (!flag)
				{
					return HT.TOPRIGHT;
				}
				return HT.TOPLEFT;
			case ResizeGripDirection.Right:
				if (!flag)
				{
					return HT.RIGHT;
				}
				return HT.LEFT;
			case ResizeGripDirection.BottomRight:
				if (!flag)
				{
					return HT.BOTTOMRIGHT;
				}
				return HT.BOTTOMLEFT;
			case ResizeGripDirection.Bottom:
				return HT.BOTTOM;
			case ResizeGripDirection.BottomLeft:
				if (!flag)
				{
					return HT.BOTTOMLEFT;
				}
				return HT.BOTTOMRIGHT;
			case ResizeGripDirection.Left:
				if (!flag)
				{
					return HT.LEFT;
				}
				return HT.RIGHT;
			default:
				return HT.NOWHERE;
			}
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x001A3E14 File Offset: 0x001A2E14
		private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			DpiScale dpi = this._window.GetDpi();
			Point point = new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam));
			Rect deviceRectangle = this._GetWindowRect();
			Point point2 = point;
			point2.Offset(-deviceRectangle.X, -deviceRectangle.Y);
			point2 = DpiHelper.DevicePixelsToLogical(point2, dpi.DpiScaleX, dpi.DpiScaleY);
			IInputElement inputElement = this._window.InputHitTest(point2);
			if (inputElement != null)
			{
				if (WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
				{
					handled = true;
					return new IntPtr(1);
				}
				ResizeGripDirection resizeGripDirection = WindowChrome.GetResizeGripDirection(inputElement);
				if (resizeGripDirection != ResizeGripDirection.None)
				{
					handled = true;
					return new IntPtr((int)this._GetHTFromResizeGripDirection(resizeGripDirection));
				}
			}
			if (this._chromeInfo.UseAeroCaptionButtons && Utility.IsOSVistaOrNewer && this._chromeInfo.GlassFrameThickness != default(Thickness) && this._isGlassEnabled)
			{
				IntPtr intPtr;
				handled = NativeMethods.DwmDefWindowProc(this._hwnd, uMsg, wParam, lParam, out intPtr);
				if (IntPtr.Zero != intPtr)
				{
					return intPtr;
				}
			}
			int value = (int)this._HitTestNca(DpiHelper.DeviceRectToLogical(deviceRectangle, dpi.DpiScaleX, dpi.DpiScaleY), DpiHelper.DevicePixelsToLogical(point, dpi.DpiScaleX, dpi.DpiScaleY));
			handled = true;
			return new IntPtr(value);
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x001A3F4F File Offset: 0x001A2F4F
		private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (2 == wParam.ToInt32())
			{
				SystemCommands.ShowSystemMenuPhysicalCoordinates(this._window, new Point((double)Utility.GET_X_LPARAM(lParam), (double)Utility.GET_Y_LPARAM(lParam)));
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x001A3F84 File Offset: 0x001A2F84
		private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WindowState? assumeState = null;
			if (wParam.ToInt32() == 2)
			{
				assumeState = new WindowState?(WindowState.Maximized);
			}
			this._UpdateSystemMenu(assumeState);
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x001A3FBC File Offset: 0x001A2FBC
		private IntPtr _HandleWindowPosChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			WINDOWPOS windowpos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
			if (!Utility.IsFlagSet(windowpos.flags, 1))
			{
				this._UpdateSystemMenu(null);
				if (!this._isGlassEnabled)
				{
					this._SetRoundingRegion(new WINDOWPOS?(windowpos));
				}
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x001A4019 File Offset: 0x001A3019
		private IntPtr _HandleDwmCompositionChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._UpdateFrameState(false);
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x001A402B File Offset: 0x001A302B
		private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._FixupTemplateIssues();
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x001A403C File Offset: 0x001A303C
		private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = true;
			if (this._window.WindowState != WindowState.Maximized && !this._IsWindowDocked)
			{
				this._windowPosAtStartOfUserMove = new Point(this._window.Left, this._window.Top);
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x001A4090 File Offset: 0x001A3090
		private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			this._isUserResizing = false;
			if (this._window.WindowState == WindowState.Maximized)
			{
				this._window.Top = this._windowPosAtStartOfUserMove.Y;
				this._window.Left = this._windowPosAtStartOfUserMove.X;
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x001A40E7 File Offset: 0x001A30E7
		private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
		{
			if (this._isUserResizing)
			{
				this._hasUserMovedWindow = true;
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x001A4104 File Offset: 0x001A3104
		private bool _ModifyStyle(WS removeStyle, WS addStyle)
		{
			int num = NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32();
			WS ws = (WS)((num & (int)(~(int)removeStyle)) | (int)addStyle);
			if (num == (int)ws)
			{
				return false;
			}
			NativeMethods.SetWindowLongPtr(this._hwnd, GWL.STYLE, new IntPtr((int)ws));
			return true;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x001A4148 File Offset: 0x001A3148
		private WindowState _GetHwndState()
		{
			SW showCmd = NativeMethods.GetWindowPlacement(this._hwnd).showCmd;
			if (showCmd == SW.SHOWMINIMIZED)
			{
				return WindowState.Minimized;
			}
			if (showCmd != SW.SHOWMAXIMIZED)
			{
				return WindowState.Normal;
			}
			return WindowState.Maximized;
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x001A4178 File Offset: 0x001A3178
		private Rect _GetWindowRect()
		{
			RECT windowRect = NativeMethods.GetWindowRect(this._hwnd);
			return new Rect((double)windowRect.Left, (double)windowRect.Top, (double)windowRect.Width, (double)windowRect.Height);
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x001A41B8 File Offset: 0x001A31B8
		private void _UpdateSystemMenu(WindowState? assumeState)
		{
			WindowState windowState = assumeState ?? this._GetHwndState();
			if (assumeState != null || this._lastMenuState != windowState)
			{
				this._lastMenuState = windowState;
				bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
				IntPtr systemMenu = NativeMethods.GetSystemMenu(this._hwnd, false);
				if (IntPtr.Zero != systemMenu)
				{
					int value = NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32();
					bool flag2 = Utility.IsFlagSet(value, 131072);
					bool flag3 = Utility.IsFlagSet(value, 65536);
					bool flag4 = Utility.IsFlagSet(value, 262144);
					if (windowState != WindowState.Minimized)
					{
						if (windowState == WindowState.Maximized)
						{
							NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
							NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.GRAYED | MF.DISABLED);
						}
						else
						{
							NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.GRAYED | MF.DISABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.ENABLED);
							NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, flag4 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
							NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
							NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
						}
					}
					else
					{
						NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.GRAYED | MF.DISABLED);
						NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : (MF.GRAYED | MF.DISABLED));
					}
				}
				if (flag)
				{
					this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
				}
			}
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x001A436C File Offset: 0x001A336C
		private void _UpdateFrameState(bool force)
		{
			if (IntPtr.Zero == this._hwnd || this._hwndSource.IsDisposed)
			{
				return;
			}
			bool flag = NativeMethods.DwmIsCompositionEnabled();
			if (force || flag != this._isGlassEnabled)
			{
				this._isGlassEnabled = (flag && this._chromeInfo.GlassFrameThickness != default(Thickness));
				if (!this._isGlassEnabled)
				{
					this._SetRoundingRegion(null);
				}
				else
				{
					this._ClearRoundingRegion();
					this._ExtendGlassFrame();
				}
				NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
			}
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x001A440F File Offset: 0x001A340F
		private void _ClearRoundingRegion()
		{
			NativeMethods.SetWindowRgn(this._hwnd, IntPtr.Zero, NativeMethods.IsWindowVisible(this._hwnd));
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x001A442C File Offset: 0x001A342C
		private void _SetRoundingRegion(WINDOWPOS? wp)
		{
			if (NativeMethods.GetWindowPlacement(this._hwnd).showCmd == SW.SHOWMAXIMIZED)
			{
				int num;
				int num2;
				if (wp != null)
				{
					num = wp.Value.x;
					num2 = wp.Value.y;
				}
				else
				{
					Rect rect = this._GetWindowRect();
					num = (int)rect.Left;
					num2 = (int)rect.Top;
				}
				RECT rcWork = NativeMethods.GetMonitorInfo(NativeMethods.MonitorFromWindow(this._hwnd, 2U)).rcWork;
				rcWork.Offset(-num, -num2);
				IntPtr hRgn = IntPtr.Zero;
				try
				{
					hRgn = NativeMethods.CreateRectRgnIndirect(rcWork);
					NativeMethods.SetWindowRgn(this._hwnd, hRgn, NativeMethods.IsWindowVisible(this._hwnd));
					hRgn = IntPtr.Zero;
					return;
				}
				finally
				{
					Utility.SafeDeleteObject(ref hRgn);
				}
			}
			Size size;
			if (wp != null && !Utility.IsFlagSet(wp.Value.flags, 1))
			{
				size = new Size((double)wp.Value.cx, (double)wp.Value.cy);
			}
			else
			{
				if (wp != null && this._lastRoundingState == this._window.WindowState)
				{
					return;
				}
				size = this._GetWindowRect().Size;
			}
			this._lastRoundingState = this._window.WindowState;
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				DpiScale dpi = this._window.GetDpi();
				double num3 = Math.Min(size.Width, size.Height);
				double num4 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopLeft, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
				num4 = Math.Min(num4, num3 / 2.0);
				if (WindowChromeWorker._IsUniform(this._chromeInfo.CornerRadius))
				{
					intPtr = WindowChromeWorker._CreateRoundRectRgn(new Rect(size), num4);
				}
				else
				{
					intPtr = WindowChromeWorker._CreateRoundRectRgn(new Rect(0.0, 0.0, size.Width / 2.0 + num4, size.Height / 2.0 + num4), num4);
					double num5 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopRight, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
					num5 = Math.Min(num5, num3 / 2.0);
					Rect region = new Rect(0.0, 0.0, size.Width / 2.0 + num5, size.Height / 2.0 + num5);
					region.Offset(size.Width / 2.0 - num5, 0.0);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region, num5);
					double num6 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomLeft, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
					num6 = Math.Min(num6, num3 / 2.0);
					Rect region2 = new Rect(0.0, 0.0, size.Width / 2.0 + num6, size.Height / 2.0 + num6);
					region2.Offset(0.0, size.Height / 2.0 - num6);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region2, num6);
					double num7 = DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomRight, 0.0), dpi.DpiScaleX, dpi.DpiScaleY).X;
					num7 = Math.Min(num7, num3 / 2.0);
					Rect region3 = new Rect(0.0, 0.0, size.Width / 2.0 + num7, size.Height / 2.0 + num7);
					region3.Offset(size.Width / 2.0 - num7, size.Height / 2.0 - num7);
					WindowChromeWorker._CreateAndCombineRoundRectRgn(intPtr, region3, num7);
				}
				NativeMethods.SetWindowRgn(this._hwnd, intPtr, NativeMethods.IsWindowVisible(this._hwnd));
				intPtr = IntPtr.Zero;
			}
			finally
			{
				Utility.SafeDeleteObject(ref intPtr);
			}
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x001A490C File Offset: 0x001A390C
		private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
		{
			if (DoubleUtilities.AreClose(0.0, radius))
			{
				return NativeMethods.CreateRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right), (int)Math.Ceiling(region.Bottom));
			}
			return NativeMethods.CreateRoundRectRgn((int)Math.Floor(region.Left), (int)Math.Floor(region.Top), (int)Math.Ceiling(region.Right) + 1, (int)Math.Ceiling(region.Bottom) + 1, (int)Math.Ceiling(radius), (int)Math.Ceiling(radius));
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x001A49B0 File Offset: 0x001A39B0
		private static void _CreateAndCombineRoundRectRgn(IntPtr hrgnSource, Rect region, double radius)
		{
			IntPtr hrgnSrc = IntPtr.Zero;
			try
			{
				hrgnSrc = WindowChromeWorker._CreateRoundRectRgn(region, radius);
				if (NativeMethods.CombineRgn(hrgnSource, hrgnSource, hrgnSrc, RGN.OR) == CombineRgnResult.ERROR)
				{
					throw new InvalidOperationException("Unable to combine two HRGNs.");
				}
			}
			catch
			{
				Utility.SafeDeleteObject(ref hrgnSrc);
				throw;
			}
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x001A4A00 File Offset: 0x001A3A00
		private static bool _IsUniform(CornerRadius cornerRadius)
		{
			return DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight) && DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight) && DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x001A4A54 File Offset: 0x001A3A54
		private void _ExtendGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer)
			{
				return;
			}
			if (IntPtr.Zero == this._hwnd)
			{
				return;
			}
			if (!NativeMethods.DwmIsCompositionEnabled())
			{
				this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
				return;
			}
			DpiScale dpi = this._window.GetDpi();
			this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
			Thickness thickness = DpiHelper.LogicalThicknessToDevice(this._chromeInfo.GlassFrameThickness, dpi.DpiScaleX, dpi.DpiScaleY);
			if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
			{
				Thickness thickness2 = DpiHelper.LogicalThicknessToDevice(SystemParameters.WindowResizeBorderThickness, dpi.DpiScaleX, dpi.DpiScaleY);
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 2))
				{
					thickness.Top -= thickness2.Top;
					thickness.Top = Math.Max(0.0, thickness.Top);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 1))
				{
					thickness.Left -= thickness2.Left;
					thickness.Left = Math.Max(0.0, thickness.Left);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 8))
				{
					thickness.Bottom -= thickness2.Bottom;
					thickness.Bottom = Math.Max(0.0, thickness.Bottom);
				}
				if (Utility.IsFlagSet((int)this._chromeInfo.NonClientFrameEdges, 4))
				{
					thickness.Right -= thickness2.Right;
					thickness.Right = Math.Max(0.0, thickness.Right);
				}
			}
			MARGINS margins = new MARGINS
			{
				cxLeftWidth = (int)Math.Ceiling(thickness.Left),
				cxRightWidth = (int)Math.Ceiling(thickness.Right),
				cyTopHeight = (int)Math.Ceiling(thickness.Top),
				cyBottomHeight = (int)Math.Ceiling(thickness.Bottom)
			};
			NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref margins);
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x001A4C7C File Offset: 0x001A3C7C
		private HT _HitTestNca(Rect windowPosition, Point mousePosition)
		{
			int num = 1;
			int num2 = 1;
			bool flag = false;
			if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top + this._chromeInfo.CaptionHeight)
			{
				flag = (mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top);
				num = 0;
			}
			else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double)((int)this._chromeInfo.ResizeBorderThickness.Bottom))
			{
				num = 2;
			}
			if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double)((int)this._chromeInfo.ResizeBorderThickness.Left))
			{
				num2 = 0;
			}
			else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - this._chromeInfo.ResizeBorderThickness.Right)
			{
				num2 = 2;
			}
			if (num == 0 && num2 != 1 && !flag)
			{
				num = 1;
			}
			HT ht = WindowChromeWorker._HitTestBorders[num, num2];
			if (ht == HT.TOP && !flag)
			{
				ht = HT.CAPTION;
			}
			return ht;
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x001A4DC9 File Offset: 0x001A3DC9
		private void _RestoreStandardChromeState(bool isClosing)
		{
			base.VerifyAccess();
			this._UnhookCustomChrome();
			if (!isClosing && !this._hwndSource.IsDisposed)
			{
				this._RestoreFrameworkIssueFixups();
				this._RestoreGlassFrame();
				this._RestoreHrgn();
				this._window.InvalidateMeasure();
			}
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x001A4E04 File Offset: 0x001A3E04
		private void _UnhookCustomChrome()
		{
			if (this._isHooked)
			{
				this._hwndSource.RemoveHook(new HwndSourceHook(this._WndProc));
				this._isHooked = false;
			}
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x001A4E2C File Offset: 0x001A3E2C
		private void _RestoreFrameworkIssueFixups()
		{
			((FrameworkElement)VisualTreeHelper.GetChild(this._window, 0)).Margin = default(Thickness);
			if (Utility.IsPresentationFrameworkVersionLessThan4)
			{
				this._window.StateChanged -= this._FixupRestoreBounds;
				this._isFixedUp = false;
			}
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x001A4E80 File Offset: 0x001A3E80
		private void _RestoreGlassFrame()
		{
			if (!Utility.IsOSVistaOrNewer || this._hwnd == IntPtr.Zero)
			{
				return;
			}
			this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
			if (NativeMethods.DwmIsCompositionEnabled())
			{
				MARGINS margins = default(MARGINS);
				NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref margins);
			}
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x001A4ED8 File Offset: 0x001A3ED8
		private void _RestoreHrgn()
		{
			this._ClearRoundingRegion();
			NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
		}

		// Token: 0x04001AE7 RID: 6887
		private const SWP _SwpFlags = SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER;

		// Token: 0x04001AE8 RID: 6888
		private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;

		// Token: 0x04001AE9 RID: 6889
		private Window _window;

		// Token: 0x04001AEA RID: 6890
		private IntPtr _hwnd;

		// Token: 0x04001AEB RID: 6891
		private HwndSource _hwndSource;

		// Token: 0x04001AEC RID: 6892
		private bool _isHooked;

		// Token: 0x04001AED RID: 6893
		private bool _isFixedUp;

		// Token: 0x04001AEE RID: 6894
		private bool _isUserResizing;

		// Token: 0x04001AEF RID: 6895
		private bool _hasUserMovedWindow;

		// Token: 0x04001AF0 RID: 6896
		private Point _windowPosAtStartOfUserMove;

		// Token: 0x04001AF1 RID: 6897
		private WindowChrome _chromeInfo;

		// Token: 0x04001AF2 RID: 6898
		private WindowState _lastRoundingState;

		// Token: 0x04001AF3 RID: 6899
		private WindowState _lastMenuState;

		// Token: 0x04001AF4 RID: 6900
		private bool _isGlassEnabled;

		// Token: 0x04001AF5 RID: 6901
		public static readonly DependencyProperty WindowChromeWorkerProperty = DependencyProperty.RegisterAttached("WindowChromeWorker", typeof(WindowChromeWorker), typeof(WindowChromeWorker), new PropertyMetadata(null, new PropertyChangedCallback(WindowChromeWorker._OnChromeWorkerChanged)));

		// Token: 0x04001AF6 RID: 6902
		private static readonly HT[,] _HitTestBorders = new HT[,]
		{
			{
				HT.TOPLEFT,
				HT.TOP,
				HT.TOPRIGHT
			},
			{
				HT.LEFT,
				HT.CLIENT,
				HT.RIGHT
			},
			{
				HT.BOTTOMLEFT,
				HT.BOTTOM,
				HT.BOTTOMRIGHT
			}
		};

		// Token: 0x02000AA7 RID: 2727
		// (Invoke) Token: 0x0600872B RID: 34603
		private delegate void _Action();
	}
}
