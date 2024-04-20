using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Interop
{
	// Token: 0x02000422 RID: 1058
	public abstract class HwndHost : FrameworkElement, IDisposable, IWin32Window, IKeyboardInputSink
	{
		// Token: 0x06003303 RID: 13059 RVA: 0x001D3AEC File Offset: 0x001D2AEC
		static HwndHost()
		{
			UIElement.FocusableProperty.OverrideMetadata(typeof(HwndHost), new FrameworkPropertyMetadata(true));
			HwndHost.DpiChangedEvent = Window.DpiChangedEvent.AddOwner(typeof(HwndHost));
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x001D3B26 File Offset: 0x001D2B26
		protected HwndHost()
		{
			this.Initialize(false);
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x001D3B35 File Offset: 0x001D2B35
		internal HwndHost(bool fTrusted)
		{
			this.Initialize(fTrusted);
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x001D3B44 File Offset: 0x001D2B44
		~HwndHost()
		{
			this.Dispose(false);
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x001D3B74 File Offset: 0x001D2B74
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06003308 RID: 13064 RVA: 0x001D3B83 File Offset: 0x001D2B83
		public IntPtr Handle
		{
			get
			{
				return this.CriticalHandle;
			}
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06003309 RID: 13065 RVA: 0x001D3B8B File Offset: 0x001D2B8B
		// (remove) Token: 0x0600330A RID: 13066 RVA: 0x001D3BAE File Offset: 0x001D2BAE
		public event HwndSourceHook MessageHook
		{
			add
			{
				if (this._hooks == null)
				{
					this._hooks = new ArrayList(8);
				}
				this._hooks.Add(value);
			}
			remove
			{
				if (this._hooks != null)
				{
					this._hooks.Remove(value);
					if (this._hooks.Count == 0)
					{
						this._hooks = null;
					}
				}
			}
		}

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x0600330B RID: 13067 RVA: 0x001D3BD8 File Offset: 0x001D2BD8
		// (remove) Token: 0x0600330C RID: 13068 RVA: 0x001D3BE6 File Offset: 0x001D2BE6
		public event DpiChangedEventHandler DpiChanged
		{
			add
			{
				base.AddHandler(HwndHost.DpiChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(HwndHost.DpiChangedEvent, value);
			}
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x001D3BF4 File Offset: 0x001D2BF4
		protected override void OnKeyUp(KeyEventArgs e)
		{
			MSG msg;
			if (this._fTrusted.Value)
			{
				msg = ComponentDispatcher.UnsecureCurrentKeyboardMessage;
			}
			else
			{
				msg = ComponentDispatcher.CurrentKeyboardMessage;
			}
			ModifierKeys systemModifierKeys = HwndKeyboardInputProvider.GetSystemModifierKeys();
			bool flag = ((IKeyboardInputSink)this).TranslateAccelerator(ref msg, systemModifierKeys);
			if (flag)
			{
				e.Handled = flag;
			}
			base.OnKeyUp(e);
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x001D3C3D File Offset: 0x001D2C3D
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.RaiseEvent(new DpiChangedEventArgs(oldDpi, newDpi, HwndHost.DpiChangedEvent, this));
			this.UpdateWindowPos();
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x001D3C58 File Offset: 0x001D2C58
		protected override void OnKeyDown(KeyEventArgs e)
		{
			MSG msg;
			if (this._fTrusted.Value)
			{
				msg = ComponentDispatcher.UnsecureCurrentKeyboardMessage;
			}
			else
			{
				msg = ComponentDispatcher.CurrentKeyboardMessage;
			}
			ModifierKeys systemModifierKeys = HwndKeyboardInputProvider.GetSystemModifierKeys();
			bool flag = ((IKeyboardInputSink)this).TranslateAccelerator(ref msg, systemModifierKeys);
			if (flag)
			{
				e.Handled = flag;
			}
			base.OnKeyDown(e);
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x001D3CA1 File Offset: 0x001D2CA1
		protected virtual IKeyboardInputSite RegisterKeyboardInputSinkCore(IKeyboardInputSink sink)
		{
			throw new InvalidOperationException(SR.Get("HwndHostDoesNotSupportChildKeyboardSinks"));
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x001D3CB2 File Offset: 0x001D2CB2
		IKeyboardInputSite IKeyboardInputSink.RegisterKeyboardInputSink(IKeyboardInputSink sink)
		{
			return this.RegisterKeyboardInputSinkCore(sink);
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool TranslateAcceleratorCore(ref MSG msg, ModifierKeys modifiers)
		{
			return false;
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x001D3CBB File Offset: 0x001D2CBB
		bool IKeyboardInputSink.TranslateAccelerator(ref MSG msg, ModifierKeys modifiers)
		{
			return this.TranslateAcceleratorCore(ref msg, modifiers);
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool TabIntoCore(TraversalRequest request)
		{
			return false;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x001D3CC5 File Offset: 0x001D2CC5
		bool IKeyboardInputSink.TabInto(TraversalRequest request)
		{
			return this.TabIntoCore(request);
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x001D3CCE File Offset: 0x001D2CCE
		// (set) Token: 0x06003317 RID: 13079 RVA: 0x001D3CD6 File Offset: 0x001D2CD6
		IKeyboardInputSite IKeyboardInputSink.KeyboardInputSite { get; set; }

		// Token: 0x06003318 RID: 13080 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool OnMnemonicCore(ref MSG msg, ModifierKeys modifiers)
		{
			return false;
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x001D3CDF File Offset: 0x001D2CDF
		bool IKeyboardInputSink.OnMnemonic(ref MSG msg, ModifierKeys modifiers)
		{
			return this.OnMnemonicCore(ref msg, modifiers);
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool TranslateCharCore(ref MSG msg, ModifierKeys modifiers)
		{
			return false;
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x001D3CE9 File Offset: 0x001D2CE9
		bool IKeyboardInputSink.TranslateChar(ref MSG msg, ModifierKeys modifiers)
		{
			return this.TranslateCharCore(ref msg, modifiers);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x001D3CF4 File Offset: 0x001D2CF4
		protected virtual bool HasFocusWithinCore()
		{
			HandleRef hwnd = new HandleRef(this, UnsafeNativeMethods.GetFocus());
			return this.Handle != IntPtr.Zero && (hwnd.Handle == this._hwnd.Handle || UnsafeNativeMethods.IsChild(this._hwnd, hwnd));
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x001D3D4A File Offset: 0x001D2D4A
		bool IKeyboardInputSink.HasFocusWithin()
		{
			return this.HasFocusWithinCore();
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x001D3D54 File Offset: 0x001D2D54
		public void UpdateWindowPos()
		{
			if (this._isDisposed)
			{
				return;
			}
			PresentationSource presentationSource = null;
			CompositionTarget compositionTarget = null;
			if (this.CriticalHandle != IntPtr.Zero && base.IsVisible)
			{
				presentationSource = PresentationSource.CriticalFromVisual(this, false);
				if (presentationSource != null)
				{
					compositionTarget = presentationSource.CompositionTarget;
				}
			}
			if (compositionTarget != null && compositionTarget.RootVisual != null)
			{
				Rect rcBoundingBox = PointUtil.ToRect(this.CalculateAssignedRC(presentationSource));
				this.OnWindowPositionChanged(rcBoundingBox);
				UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 5);
				return;
			}
			UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 0);
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x001D3DD4 File Offset: 0x001D2DD4
		private NativeMethods.RECT CalculateAssignedRC(PresentationSource source)
		{
			Rect rect = PointUtil.RootToClient(PointUtil.ElementToRoot(new Rect(base.RenderSize), this, source), source);
			IntPtr parent = UnsafeNativeMethods.GetParent(this._hwnd);
			NativeMethods.RECT rect2 = PointUtil.AdjustForRightToLeft(PointUtil.FromRect(rect), new HandleRef(null, parent));
			if (!CoreAppContextSwitches.DoNotUsePresentationDpiCapabilityTier2OrGreater)
			{
				rect2 = this.AdjustRectForDpi(rect2);
			}
			return rect2;
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003320 RID: 13088 RVA: 0x001D3E28 File Offset: 0x001D2E28
		private double DpiParentToChildRatio
		{
			get
			{
				if (!this._hasDpiAwarenessContextTransition)
				{
					return 1.0;
				}
				DpiScale2 windowDpi = DpiUtil.GetWindowDpi(this.Handle, false);
				DpiScale2 windowDpi2 = DpiUtil.GetWindowDpi(UnsafeNativeMethods.GetParent(this._hwnd), false);
				if (windowDpi == null || windowDpi2 == null)
				{
					return 1.0;
				}
				return windowDpi2.DpiScaleX / windowDpi.DpiScaleX;
			}
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x001D3E90 File Offset: 0x001D2E90
		private NativeMethods.RECT AdjustRectForDpi(NativeMethods.RECT rcRect)
		{
			if (this._hasDpiAwarenessContextTransition)
			{
				double dpiParentToChildRatio = this.DpiParentToChildRatio;
				rcRect.left = (int)((double)rcRect.left / dpiParentToChildRatio);
				rcRect.top = (int)((double)rcRect.top / dpiParentToChildRatio);
				rcRect.right = (int)((double)rcRect.right / dpiParentToChildRatio);
				rcRect.bottom = (int)((double)rcRect.bottom / dpiParentToChildRatio);
			}
			return rcRect;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x001D3EF4 File Offset: 0x001D2EF4
		protected virtual void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				return;
			}
			if (disposing)
			{
				base.VerifyAccess();
				if (this._hwndSubclass != null)
				{
					if (this._fTrusted.Value)
					{
						this._hwndSubclass.CriticalDetach(false);
					}
					else
					{
						this._hwndSubclass.RequestDetach(false);
					}
					this._hwndSubclass = null;
				}
				this._hooks = null;
				PresentationSource.RemoveSourceChangedHandler(this, new SourceChangedEventHandler(this.OnSourceChanged));
			}
			if (this._weakEventDispatcherShutdown != null)
			{
				this._weakEventDispatcherShutdown.Dispose();
				this._weakEventDispatcherShutdown = null;
			}
			this.DestroyWindow();
			this._isDisposed = true;
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x001D3F8A File Offset: 0x001D2F8A
		private void OnDispatcherShutdown(object sender, EventArgs e)
		{
			this.Dispose();
		}

		// Token: 0x06003324 RID: 13092
		protected abstract HandleRef BuildWindowCore(HandleRef hwndParent);

		// Token: 0x06003325 RID: 13093
		protected abstract void DestroyWindowCore(HandleRef hwnd);

		// Token: 0x06003326 RID: 13094 RVA: 0x001D3F94 File Offset: 0x001D2F94
		protected unsafe virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			this.DemandIfUntrusted();
			if (msg != 61)
			{
				if (msg != 70)
				{
					if (msg == 130)
					{
						this._hwnd = new HandleRef(null, IntPtr.Zero);
					}
				}
				else
				{
					PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this, false);
					if (presentationSource != null)
					{
						NativeMethods.RECT rect = this.CalculateAssignedRC(presentationSource);
						NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)lParam);
						ptr->cx = rect.right - rect.left;
						ptr->cy = rect.bottom - rect.top;
						ptr->flags &= -2;
						ptr->x = rect.left;
						ptr->y = rect.top;
						ptr->flags &= -3;
						ptr->flags |= 256;
					}
				}
				return IntPtr.Zero;
			}
			handled = true;
			return this.OnWmGetObject(wParam, lParam);
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x001D406F File Offset: 0x001D306F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new HwndHostAutomationPeer(this);
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x001D4078 File Offset: 0x001D3078
		private IntPtr OnWmGetObject(IntPtr wparam, IntPtr lparam)
		{
			IntPtr result = IntPtr.Zero;
			AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
			if (automationPeer != null)
			{
				IRawElementProviderSimple interopChild = automationPeer.GetInteropChild();
				result = AutomationInteropProvider.ReturnRawElementProvider(this.CriticalHandle, wparam, lparam, interopChild);
			}
			return result;
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x001D40AC File Offset: 0x001D30AC
		protected virtual void OnWindowPositionChanged(Rect rcBoundingBox)
		{
			if (this._isDisposed)
			{
				return;
			}
			UnsafeNativeMethods.SetWindowPos(this._hwnd, new HandleRef(null, IntPtr.Zero), (int)rcBoundingBox.X, (int)rcBoundingBox.Y, (int)rcBoundingBox.Width, (int)rcBoundingBox.Height, 16660);
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x001D4100 File Offset: 0x001D3100
		protected override Size MeasureOverride(Size constraint)
		{
			this.DemandIfUntrusted();
			Size result = new Size(0.0, 0.0);
			if (this.CriticalHandle != IntPtr.Zero)
			{
				result.Width = Math.Min(this._desiredSize.Width, constraint.Width);
				result.Height = Math.Min(this._desiredSize.Height, constraint.Height);
			}
			return result;
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x001D417B File Offset: 0x001D317B
		internal override DrawingGroup GetDrawing()
		{
			return this.GetDrawingHelper();
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x001D4183 File Offset: 0x001D3183
		internal override Rect GetContentBounds()
		{
			return new Rect(base.RenderSize);
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x001D4190 File Offset: 0x001D3190
		private DrawingGroup GetDrawingHelper()
		{
			DrawingGroup drawingGroup = null;
			if (this.Handle != IntPtr.Zero)
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetWindowRect(this._hwnd, ref rect);
				int num = rect.right - rect.left;
				int num2 = rect.bottom - rect.top;
				HandleRef hDC = new HandleRef(this, UnsafeNativeMethods.GetDC(new HandleRef(this, IntPtr.Zero)));
				if (hDC.Handle != IntPtr.Zero)
				{
					HandleRef handleRef = new HandleRef(this, IntPtr.Zero);
					HandleRef hObject = new HandleRef(this, IntPtr.Zero);
					try
					{
						handleRef = new HandleRef(this, UnsafeNativeMethods.CriticalCreateCompatibleDC(hDC));
						if (handleRef.Handle != IntPtr.Zero)
						{
							hObject = new HandleRef(this, UnsafeNativeMethods.CriticalCreateCompatibleBitmap(hDC, num, num2));
							if (hObject.Handle != IntPtr.Zero)
							{
								IntPtr obj = UnsafeNativeMethods.CriticalSelectObject(handleRef, hObject.Handle);
								try
								{
									NativeMethods.RECT rect2 = new NativeMethods.RECT(0, 0, num, num2);
									IntPtr brush = UnsafeNativeMethods.CriticalGetStockObject(0);
									UnsafeNativeMethods.CriticalFillRect(handleRef.Handle, ref rect2, brush);
									if (!UnsafeNativeMethods.CriticalPrintWindow(this._hwnd, handleRef, 0))
									{
										UnsafeNativeMethods.SendMessage(this._hwnd.Handle, WindowMessage.WM_PRINT, handleRef.Handle, (IntPtr)30);
									}
									else
									{
										UnsafeNativeMethods.CriticalRedrawWindow(this._hwnd, IntPtr.Zero, IntPtr.Zero, 129);
									}
									drawingGroup = new DrawingGroup();
									BitmapSource imageSource = Imaging.CriticalCreateBitmapSourceFromHBitmap(hObject.Handle, IntPtr.Zero, Int32Rect.Empty, null, WICBitmapAlphaChannelOption.WICBitmapIgnoreAlpha);
									Rect rect3 = new Rect(base.RenderSize);
									drawingGroup.Children.Add(new ImageDrawing(imageSource, rect3));
									drawingGroup.Freeze();
								}
								finally
								{
									UnsafeNativeMethods.CriticalSelectObject(handleRef, obj);
								}
							}
						}
					}
					finally
					{
						UnsafeNativeMethods.ReleaseDC(new HandleRef(this, IntPtr.Zero), hDC);
						hDC = new HandleRef(null, IntPtr.Zero);
						if (hObject.Handle != IntPtr.Zero)
						{
							UnsafeNativeMethods.DeleteObject(hObject);
							hObject = new HandleRef(this, IntPtr.Zero);
						}
						if (handleRef.Handle != IntPtr.Zero)
						{
							UnsafeNativeMethods.CriticalDeleteDC(handleRef);
							handleRef = new HandleRef(this, IntPtr.Zero);
						}
					}
				}
			}
			return drawingGroup;
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x001D43F8 File Offset: 0x001D33F8
		private void Initialize(bool fTrusted)
		{
			this._fTrusted = new SecurityCriticalDataForSet<bool>(fTrusted);
			this._hwndSubclassHook = new HwndWrapperHook(this.SubclassWndProc);
			this._handlerLayoutUpdated = new EventHandler(this.OnLayoutUpdated);
			this._handlerEnabledChanged = new DependencyPropertyChangedEventHandler(this.OnEnabledChanged);
			this._handlerVisibleChanged = new DependencyPropertyChangedEventHandler(this.OnVisibleChanged);
			PresentationSource.AddSourceChangedHandler(this, new SourceChangedEventHandler(this.OnSourceChanged));
			this._weakEventDispatcherShutdown = new HwndHost.WeakEventDispatcherShutdown(this, base.Dispatcher);
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x001D447D File Offset: 0x001D347D
		private void DemandIfUntrusted()
		{
			bool value = this._fTrusted.Value;
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x001D448C File Offset: 0x001D348C
		private void OnSourceChanged(object sender, SourceChangedEventArgs e)
		{
			IKeyboardInputSite keyboardInputSite = ((IKeyboardInputSink)this).KeyboardInputSite;
			if (keyboardInputSite != null)
			{
				((IKeyboardInputSink)this).KeyboardInputSite = null;
				keyboardInputSite.Unregister();
			}
			IKeyboardInputSink keyboardInputSink = PresentationSource.CriticalFromVisual(this, false) as IKeyboardInputSink;
			if (keyboardInputSink != null)
			{
				((IKeyboardInputSink)this).KeyboardInputSite = keyboardInputSink.RegisterKeyboardInputSink(this);
			}
			this.BuildOrReparentWindow();
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x001D44D3 File Offset: 0x001D34D3
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.UpdateWindowPos();
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x001D44DC File Offset: 0x001D34DC
		private void OnEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this._isDisposed)
			{
				return;
			}
			bool enable = (bool)e.NewValue;
			UnsafeNativeMethods.EnableWindow(this._hwnd, enable);
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x001D450C File Offset: 0x001D350C
		private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (this._isDisposed)
			{
				return;
			}
			if ((bool)e.NewValue)
			{
				UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 8);
				return;
			}
			UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 0);
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x001D4540 File Offset: 0x001D3540
		private void BuildOrReparentWindow()
		{
			this.DemandIfUntrusted();
			if (this._isBuildingWindow || this._isDisposed)
			{
				return;
			}
			this._isBuildingWindow = true;
			IntPtr intPtr = IntPtr.Zero;
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this, false);
			if (presentationSource != null)
			{
				HwndSource hwndSource = presentationSource as HwndSource;
				if (hwndSource != null)
				{
					intPtr = hwndSource.CriticalHandle;
				}
			}
			else if (PresentationSource.CriticalFromVisual(this, true) != null && TraceHwndHost.IsEnabled)
			{
				TraceHwndHost.Trace(TraceEventType.Warning, TraceHwndHost.HwndHostIn3D);
			}
			try
			{
				if (intPtr != IntPtr.Zero)
				{
					if (this._hwnd.Handle == IntPtr.Zero)
					{
						this.BuildWindow(new HandleRef(null, intPtr));
						base.LayoutUpdated += this._handlerLayoutUpdated;
						base.IsEnabledChanged += this._handlerEnabledChanged;
						base.IsVisibleChanged += this._handlerVisibleChanged;
					}
					else if (intPtr != UnsafeNativeMethods.GetParent(this._hwnd))
					{
						UnsafeNativeMethods.SetParent(this._hwnd, new HandleRef(null, intPtr));
					}
				}
				else if (this.Handle != IntPtr.Zero)
				{
					HwndWrapper dpiAwarenessCompatibleNotificationWindow = SystemResources.GetDpiAwarenessCompatibleNotificationWindow(this._hwnd);
					if (dpiAwarenessCompatibleNotificationWindow != null)
					{
						UnsafeNativeMethods.SetParent(this._hwnd, new HandleRef(null, dpiAwarenessCompatibleNotificationWindow.Handle));
						SystemResources.DelayHwndShutdown();
					}
					else
					{
						Trace.WriteLineIf(dpiAwarenessCompatibleNotificationWindow == null, "- Warning - Notification Window is null\n" + new StackTrace(true).ToString());
					}
				}
			}
			finally
			{
				this._isBuildingWindow = false;
			}
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x001D46A8 File Offset: 0x001D36A8
		private void BuildWindow(HandleRef hwndParent)
		{
			this.DemandIfUntrusted();
			this._hwnd = this.BuildWindowCore(hwndParent);
			if (this._hwnd.Handle == IntPtr.Zero || !UnsafeNativeMethods.IsWindow(this._hwnd))
			{
				throw new InvalidOperationException(SR.Get("ChildWindowNotCreated"));
			}
			if ((UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this._hwnd.Handle), -16) & 1073741824) == 0)
			{
				throw new InvalidOperationException(SR.Get("HostedWindowMustBeAChildWindow"));
			}
			if (hwndParent.Handle != UnsafeNativeMethods.GetParent(this._hwnd))
			{
				throw new InvalidOperationException(SR.Get("ChildWindowMustHaveCorrectParent"));
			}
			if (DpiUtil.GetDpiAwarenessContext(this._hwnd.Handle) != DpiUtil.GetDpiAwarenessContext(hwndParent.Handle))
			{
				this._hasDpiAwarenessContextTransition = true;
			}
			int num;
			if (UnsafeNativeMethods.GetWindowThreadProcessId(this._hwnd, out num) == SafeNativeMethods.GetCurrentThreadId() && num == SafeNativeMethods.GetCurrentProcessId())
			{
				this._hwndSubclass = new HwndSubclass(this._hwndSubclassHook);
				this._hwndSubclass.CriticalAttach(this._hwnd.Handle);
			}
			UnsafeNativeMethods.ShowWindowAsync(this._hwnd, 0);
			NativeMethods.RECT rect = default(NativeMethods.RECT);
			SafeNativeMethods.GetWindowRect(this._hwnd, ref rect);
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this, false);
			Point point = new Point((double)rect.left, (double)rect.top);
			Point point2 = new Point((double)rect.right, (double)rect.bottom);
			point = presentationSource.CompositionTarget.TransformFromDevice.Transform(point);
			point2 = presentationSource.CompositionTarget.TransformFromDevice.Transform(point2);
			this._desiredSize = new Size(point2.X - point.X, point2.Y - point.Y);
			base.InvalidateMeasure();
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x001D486C File Offset: 0x001D386C
		private void DestroyWindow()
		{
			if (this.CriticalHandle == IntPtr.Zero)
			{
				return;
			}
			if (!base.CheckAccess())
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.AsyncDestroyWindow), null);
				return;
			}
			HandleRef hwnd = this._hwnd;
			this._hwnd = new HandleRef(null, IntPtr.Zero);
			this.DestroyWindowCore(hwnd);
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x001D48CF File Offset: 0x001D38CF
		private object AsyncDestroyWindow(object arg)
		{
			this.DestroyWindow();
			return null;
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003338 RID: 13112 RVA: 0x001D48D8 File Offset: 0x001D38D8
		internal IntPtr CriticalHandle
		{
			get
			{
				if (this._hwnd.Handle != IntPtr.Zero && !UnsafeNativeMethods.IsWindow(this._hwnd))
				{
					this._hwnd = new HandleRef(null, IntPtr.Zero);
				}
				return this._hwnd.Handle;
			}
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x001D4928 File Offset: 0x001D3928
		private IntPtr SubclassWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr result = IntPtr.Zero;
			result = this.WndProc(hwnd, msg, wParam, lParam, ref handled);
			if (!handled && this._hooks != null)
			{
				int i = 0;
				int count = this._hooks.Count;
				while (i < count)
				{
					result = ((HwndSourceHook)this._hooks[i])(hwnd, msg, wParam, lParam, ref handled);
					if (handled)
					{
						break;
					}
					i++;
				}
			}
			return result;
		}

		// Token: 0x04001C16 RID: 7190
		private DependencyPropertyChangedEventHandler _handlerEnabledChanged;

		// Token: 0x04001C17 RID: 7191
		private DependencyPropertyChangedEventHandler _handlerVisibleChanged;

		// Token: 0x04001C18 RID: 7192
		private EventHandler _handlerLayoutUpdated;

		// Token: 0x04001C19 RID: 7193
		private HwndSubclass _hwndSubclass;

		// Token: 0x04001C1A RID: 7194
		private HwndWrapperHook _hwndSubclassHook;

		// Token: 0x04001C1B RID: 7195
		private HandleRef _hwnd;

		// Token: 0x04001C1C RID: 7196
		private ArrayList _hooks;

		// Token: 0x04001C1D RID: 7197
		private Size _desiredSize;

		// Token: 0x04001C1E RID: 7198
		private bool _hasDpiAwarenessContextTransition;

		// Token: 0x04001C1F RID: 7199
		private SecurityCriticalDataForSet<bool> _fTrusted;

		// Token: 0x04001C20 RID: 7200
		private bool _isBuildingWindow;

		// Token: 0x04001C21 RID: 7201
		private bool _isDisposed;

		// Token: 0x04001C22 RID: 7202
		private HwndHost.WeakEventDispatcherShutdown _weakEventDispatcherShutdown;

		// Token: 0x02000AB9 RID: 2745
		private class WeakEventDispatcherShutdown : WeakReference
		{
			// Token: 0x06008AC0 RID: 35520 RVA: 0x003382F9 File Offset: 0x003372F9
			public WeakEventDispatcherShutdown(HwndHost hwndHost, Dispatcher that) : base(hwndHost)
			{
				this._that = that;
				this._that.ShutdownFinished += this.OnShutdownFinished;
			}

			// Token: 0x06008AC1 RID: 35521 RVA: 0x00338320 File Offset: 0x00337320
			public void OnShutdownFinished(object sender, EventArgs e)
			{
				HwndHost hwndHost = this.Target as HwndHost;
				if (hwndHost != null)
				{
					hwndHost.OnDispatcherShutdown(sender, e);
					return;
				}
				this.Dispose();
			}

			// Token: 0x06008AC2 RID: 35522 RVA: 0x0033834B File Offset: 0x0033734B
			public void Dispose()
			{
				if (this._that != null)
				{
					this._that.ShutdownFinished -= this.OnShutdownFinished;
				}
			}

			// Token: 0x04004635 RID: 17973
			private Dispatcher _that;
		}
	}
}
