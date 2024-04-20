using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Controls;
using MS.Win32;

namespace System.Windows.Interop
{
	// Token: 0x0200041F RID: 1055
	public class ActiveXHost : HwndHost
	{
		// Token: 0x060032A6 RID: 12966 RVA: 0x001D2738 File Offset: 0x001D1738
		static ActiveXHost()
		{
			ActiveXHost.invalidatorMap[UIElement.VisibilityProperty] = new ActiveXHost.PropertyInvalidator(ActiveXHost.OnVisibilityInvalidated);
			ActiveXHost.invalidatorMap[UIElement.IsEnabledProperty] = new ActiveXHost.PropertyInvalidator(ActiveXHost.OnIsEnabledInvalidated);
			EventManager.RegisterClassHandler(typeof(ActiveXHost), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(ActiveXHost.OnAccessKeyPressed));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ActiveXHost), new FrameworkPropertyMetadata(true));
			UIElement.FocusableProperty.OverrideMetadata(typeof(ActiveXHost), new FrameworkPropertyMetadata(true));
			EventManager.RegisterClassHandler(typeof(ActiveXHost), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ActiveXHost.OnGotFocus));
			EventManager.RegisterClassHandler(typeof(ActiveXHost), Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ActiveXHost.OnLostFocus));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ActiveXHost), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x001D285C File Offset: 0x001D185C
		internal ActiveXHost(Guid clsid, bool fTrusted) : base(fTrusted)
		{
			if (Thread.CurrentThread.ApartmentState != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.Get("AxRequiresApartmentThread", new object[]
				{
					clsid.ToString()
				}));
			}
			this._clsid.Value = clsid;
			base.Initialized += this.OnInitialized;
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x001D290C File Offset: 0x001D190C
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.IsAValueChange || e.IsASubPropertyChange)
			{
				DependencyProperty property = e.Property;
				if (property != null && ActiveXHost.invalidatorMap.ContainsKey(property))
				{
					((ActiveXHost.PropertyInvalidator)ActiveXHost.invalidatorMap[property])(this);
				}
			}
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x001D2960 File Offset: 0x001D1960
		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			this.ParentHandle = hwndParent;
			this.TransitionUpTo(ActiveXHelper.ActiveXState.InPlaceActive);
			Invariant.Assert(this._axOleInPlaceActiveObject != null, "InPlace activation of ActiveX control failed");
			if (this.ControlHandle.Handle == IntPtr.Zero)
			{
				IntPtr zero = IntPtr.Zero;
				this._axOleInPlaceActiveObject.GetWindow(out zero);
				this.AttachWindow(zero);
			}
			return this._axWindow;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x001D29CC File Offset: 0x001D19CC
		protected override void OnWindowPositionChanged(Rect bounds)
		{
			this._boundRect = bounds;
			this._bounds.left = (int)bounds.X;
			this._bounds.top = (int)bounds.Y;
			this._bounds.right = (int)(bounds.Width + bounds.X);
			this._bounds.bottom = (int)(bounds.Height + bounds.Y);
			this.ActiveXSite.OnActiveXRectChange(this._bounds);
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void DestroyWindowCore(HandleRef hwnd)
		{
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x001D2A50 File Offset: 0x001D1A50
		protected override Size MeasureOverride(Size swConstraint)
		{
			base.MeasureOverride(swConstraint);
			double width;
			if (double.IsPositiveInfinity(swConstraint.Width))
			{
				width = 150.0;
			}
			else
			{
				width = swConstraint.Width;
			}
			double height;
			if (double.IsPositiveInfinity(swConstraint.Height))
			{
				height = 150.0;
			}
			else
			{
				height = swConstraint.Height;
			}
			return new Size(width, height);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void OnAccessKey(AccessKeyEventArgs args)
		{
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x001D2AB0 File Offset: 0x001D1AB0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && !this._isDisposed)
				{
					this.TransitionDownTo(ActiveXHelper.ActiveXState.Passive);
					this._isDisposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x001D2AF0 File Offset: 0x001D1AF0
		internal virtual ActiveXSite CreateActiveXSite()
		{
			return new ActiveXSite(this);
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x001D2AF8 File Offset: 0x001D1AF8
		internal virtual object CreateActiveXObject(Guid clsid)
		{
			return Activator.CreateInstance(Type.GetTypeFromCLSID(clsid));
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void AttachInterfaces(object nativeActiveXObject)
		{
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void DetachInterfaces()
		{
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void CreateSink()
		{
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void DetachSink()
		{
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnActiveXStateChange(int oldState, int newState)
		{
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060032B6 RID: 12982 RVA: 0x001D2B05 File Offset: 0x001D1B05
		protected bool IsDisposed
		{
			get
			{
				return this._isDisposed;
			}
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x001D2B0D File Offset: 0x001D1B0D
		internal void RegisterAccessKey(char key)
		{
			AccessKeyManager.Register(key.ToString(), this);
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060032B8 RID: 12984 RVA: 0x001D2B1C File Offset: 0x001D1B1C
		internal ActiveXSite ActiveXSite
		{
			get
			{
				if (this._axSite == null)
				{
					this._axSite = this.CreateActiveXSite();
				}
				return this._axSite;
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x001D2B38 File Offset: 0x001D1B38
		internal ActiveXContainer Container
		{
			get
			{
				if (this._axContainer == null)
				{
					this._axContainer = new ActiveXContainer(this);
				}
				return this._axContainer;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060032BA RID: 12986 RVA: 0x001D2B54 File Offset: 0x001D1B54
		// (set) Token: 0x060032BB RID: 12987 RVA: 0x001D2B5C File Offset: 0x001D1B5C
		internal ActiveXHelper.ActiveXState ActiveXState
		{
			get
			{
				return this._axState;
			}
			set
			{
				this._axState = value;
			}
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x001D2B65 File Offset: 0x001D1B65
		internal bool GetAxHostState(int mask)
		{
			return this._axHostState[mask];
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x001D2B73 File Offset: 0x001D1B73
		internal void SetAxHostState(int mask, bool value)
		{
			this._axHostState[mask] = value;
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x001D2B84 File Offset: 0x001D1B84
		internal void TransitionUpTo(ActiveXHelper.ActiveXState state)
		{
			if (!this.GetAxHostState(ActiveXHelper.inTransition))
			{
				this.SetAxHostState(ActiveXHelper.inTransition, true);
				try
				{
					while (state > this.ActiveXState)
					{
						ActiveXHelper.ActiveXState activeXState = this.ActiveXState;
						switch (this.ActiveXState)
						{
						case ActiveXHelper.ActiveXState.Passive:
							this.TransitionFromPassiveToLoaded();
							this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
							break;
						case ActiveXHelper.ActiveXState.Loaded:
							this.TransitionFromLoadedToRunning();
							this.ActiveXState = ActiveXHelper.ActiveXState.Running;
							break;
						case ActiveXHelper.ActiveXState.Running:
							this.TransitionFromRunningToInPlaceActive();
							this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
							break;
						case (ActiveXHelper.ActiveXState)3:
							goto IL_87;
						case ActiveXHelper.ActiveXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToUIActive();
							this.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
							break;
						default:
							goto IL_87;
						}
						IL_95:
						this.OnActiveXStateChange((int)activeXState, (int)this.ActiveXState);
						continue;
						IL_87:
						this.ActiveXState++;
						goto IL_95;
					}
				}
				finally
				{
					this.SetAxHostState(ActiveXHelper.inTransition, false);
				}
			}
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x001D2C60 File Offset: 0x001D1C60
		internal void TransitionDownTo(ActiveXHelper.ActiveXState state)
		{
			if (!this.GetAxHostState(ActiveXHelper.inTransition))
			{
				this.SetAxHostState(ActiveXHelper.inTransition, true);
				try
				{
					while (state < this.ActiveXState)
					{
						ActiveXHelper.ActiveXState activeXState = this.ActiveXState;
						ActiveXHelper.ActiveXState activeXState2 = this.ActiveXState;
						switch (activeXState2)
						{
						case ActiveXHelper.ActiveXState.Loaded:
							this.TransitionFromLoadedToPassive();
							this.ActiveXState = ActiveXHelper.ActiveXState.Passive;
							break;
						case ActiveXHelper.ActiveXState.Running:
							this.TransitionFromRunningToLoaded();
							this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
							break;
						case (ActiveXHelper.ActiveXState)3:
							goto IL_95;
						case ActiveXHelper.ActiveXState.InPlaceActive:
							this.TransitionFromInPlaceActiveToRunning();
							this.ActiveXState = ActiveXHelper.ActiveXState.Running;
							break;
						default:
							if (activeXState2 != ActiveXHelper.ActiveXState.UIActive)
							{
								if (activeXState2 != ActiveXHelper.ActiveXState.Open)
								{
									goto IL_95;
								}
								this.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
							}
							else
							{
								this.TransitionFromUIActiveToInPlaceActive();
								this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
							}
							break;
						}
						IL_A3:
						this.OnActiveXStateChange((int)activeXState, (int)this.ActiveXState);
						continue;
						IL_95:
						this.ActiveXState--;
						goto IL_A3;
					}
				}
				finally
				{
					this.SetAxHostState(ActiveXHelper.inTransition, false);
				}
			}
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x001D2D48 File Offset: 0x001D1D48
		internal bool DoVerb(int verb)
		{
			return this._axOleObject.DoVerb(verb, IntPtr.Zero, this.ActiveXSite, 0, this.ParentHandle.Handle, this._bounds) == 0;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x001D2D84 File Offset: 0x001D1D84
		internal void AttachWindow(IntPtr hwnd)
		{
			if (this._axWindow.Handle == hwnd)
			{
				return;
			}
			this._axWindow = new HandleRef(this, hwnd);
			if (this.ParentHandle.Handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.SetParent(this._axWindow, this.ParentHandle);
			}
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x001D2DDE File Offset: 0x001D1DDE
		private void StartEvents()
		{
			if (!this.GetAxHostState(ActiveXHelper.sinkAttached))
			{
				this.SetAxHostState(ActiveXHelper.sinkAttached, true);
				this.CreateSink();
			}
			this.ActiveXSite.StartEvents();
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x001D2E0A File Offset: 0x001D1E0A
		private void StopEvents()
		{
			if (this.GetAxHostState(ActiveXHelper.sinkAttached))
			{
				this.SetAxHostState(ActiveXHelper.sinkAttached, false);
				this.DetachSink();
			}
			this.ActiveXSite.StopEvents();
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x001D2E36 File Offset: 0x001D1E36
		private void TransitionFromPassiveToLoaded()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Passive)
			{
				this._axInstance = this.CreateActiveXObject(this._clsid.Value);
				this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
				this.AttachInterfacesInternal();
			}
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x001D2E64 File Offset: 0x001D1E64
		private void TransitionFromLoadedToPassive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Loaded)
			{
				if (this._axInstance != null)
				{
					this.DetachInterfacesInternal();
					Marshal.FinalReleaseComObject(this._axInstance);
					this._axInstance = null;
				}
				this.ActiveXState = ActiveXHelper.ActiveXState.Passive;
			}
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x001D2E98 File Offset: 0x001D1E98
		private void TransitionFromLoadedToRunning()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Loaded)
			{
				int num = 0;
				if (NativeMethods.Succeeded(this._axOleObject.GetMiscStatus(1, out num)) && (num & 131072) != 0)
				{
					this._axOleObject.SetClientSite(this.ActiveXSite);
				}
				this.StartEvents();
				this.ActiveXState = ActiveXHelper.ActiveXState.Running;
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x001D2EED File Offset: 0x001D1EED
		private void TransitionFromRunningToLoaded()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Running)
			{
				this.StopEvents();
				this._axOleObject.SetClientSite(null);
				this.ActiveXState = ActiveXHelper.ActiveXState.Loaded;
			}
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x001D2F14 File Offset: 0x001D1F14
		private void TransitionFromRunningToInPlaceActive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.Running)
			{
				try
				{
					this.DoVerb(-5);
				}
				catch (Exception ex)
				{
					if (CriticalExceptions.IsCriticalException(ex))
					{
						throw;
					}
					throw new TargetInvocationException(SR.Get("AXNohWnd", new object[]
					{
						base.GetType().Name
					}), ex);
				}
				this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
			}
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x001D2F7C File Offset: 0x001D1F7C
		private void TransitionFromInPlaceActiveToRunning()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.InPlaceActive)
			{
				this._axOleInPlaceObject.InPlaceDeactivate();
				this.ActiveXState = ActiveXHelper.ActiveXState.Running;
			}
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x001D2F99 File Offset: 0x001D1F99
		private void TransitionFromInPlaceActiveToUIActive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.InPlaceActive)
			{
				this.DoVerb(-4);
				this.ActiveXState = ActiveXHelper.ActiveXState.UIActive;
			}
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x001D2FB4 File Offset: 0x001D1FB4
		private void TransitionFromUIActiveToInPlaceActive()
		{
			if (this.ActiveXState == ActiveXHelper.ActiveXState.UIActive)
			{
				this._axOleInPlaceObject.UIDeactivate();
				this.ActiveXState = ActiveXHelper.ActiveXState.InPlaceActive;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x001D2FD2 File Offset: 0x001D1FD2
		// (set) Token: 0x060032CD RID: 13005 RVA: 0x001D2FE4 File Offset: 0x001D1FE4
		internal int TabIndex
		{
			get
			{
				return (int)base.GetValue(ActiveXHost.TabIndexProperty);
			}
			set
			{
				base.SetValue(ActiveXHost.TabIndexProperty, value);
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x001D2FF7 File Offset: 0x001D1FF7
		// (set) Token: 0x060032CF RID: 13007 RVA: 0x001D2FFF File Offset: 0x001D1FFF
		internal HandleRef ParentHandle
		{
			get
			{
				return this._hwndParent;
			}
			set
			{
				this._hwndParent = value;
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x060032D0 RID: 13008 RVA: 0x001D3008 File Offset: 0x001D2008
		// (set) Token: 0x060032D1 RID: 13009 RVA: 0x001D3010 File Offset: 0x001D2010
		internal NativeMethods.COMRECT Bounds
		{
			get
			{
				return this._bounds;
			}
			set
			{
				this._bounds = value;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x060032D2 RID: 13010 RVA: 0x001D3019 File Offset: 0x001D2019
		internal Rect BoundRect
		{
			get
			{
				return this._boundRect;
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x060032D3 RID: 13011 RVA: 0x001D3021 File Offset: 0x001D2021
		internal HandleRef ControlHandle
		{
			get
			{
				return this._axWindow;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x001D3029 File Offset: 0x001D2029
		internal object ActiveXInstance
		{
			get
			{
				return this._axInstance;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x001D3031 File Offset: 0x001D2031
		internal UnsafeNativeMethods.IOleInPlaceObject ActiveXInPlaceObject
		{
			get
			{
				return this._axOleInPlaceObject;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x001D3039 File Offset: 0x001D2039
		internal UnsafeNativeMethods.IOleInPlaceActiveObject ActiveXInPlaceActiveObject
		{
			get
			{
				return this._axOleInPlaceActiveObject;
			}
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x001D3041 File Offset: 0x001D2041
		private void OnInitialized(object sender, EventArgs e)
		{
			base.Initialized -= this.OnInitialized;
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnIsEnabledInvalidated(ActiveXHost axHost)
		{
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x001D3058 File Offset: 0x001D2058
		private static void OnVisibilityInvalidated(ActiveXHost axHost)
		{
			if (axHost != null)
			{
				switch (axHost.Visibility)
				{
				}
			}
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x001D3084 File Offset: 0x001D2084
		private static void OnGotFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ActiveXHost activeXHost = sender as ActiveXHost;
			if (activeXHost != null)
			{
				Invariant.Assert(activeXHost.ActiveXState >= ActiveXHelper.ActiveXState.InPlaceActive, "Should at least be InPlaceActive when getting focus");
				if (activeXHost.ActiveXState < ActiveXHelper.ActiveXState.UIActive)
				{
					activeXHost.TransitionUpTo(ActiveXHelper.ActiveXState.UIActive);
				}
			}
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x001D30C4 File Offset: 0x001D20C4
		private static void OnLostFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ActiveXHost activeXHost = sender as ActiveXHost;
			if (activeXHost != null)
			{
				Invariant.Assert(activeXHost.ActiveXState >= ActiveXHelper.ActiveXState.UIActive, "Should at least be UIActive when losing focus");
				if (!activeXHost.IsKeyboardFocusWithin)
				{
					activeXHost.TransitionDownTo(ActiveXHelper.ActiveXState.InPlaceActive);
				}
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x001D3103 File Offset: 0x001D2103
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs args)
		{
			if (!args.Handled && args.Scope == null && args.Target == null)
			{
				args.Target = (UIElement)sender;
			}
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x001D312C File Offset: 0x001D212C
		private void AttachInterfacesInternal()
		{
			this._axOleObject = (UnsafeNativeMethods.IOleObject)this._axInstance;
			this._axOleInPlaceObject = (UnsafeNativeMethods.IOleInPlaceObject)this._axInstance;
			this._axOleInPlaceActiveObject = (UnsafeNativeMethods.IOleInPlaceActiveObject)this._axInstance;
			this.AttachInterfaces(this._axInstance);
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x001D3178 File Offset: 0x001D2178
		private void DetachInterfacesInternal()
		{
			this._axOleObject = null;
			this._axOleInPlaceObject = null;
			this._axOleInPlaceActiveObject = null;
			this.DetachInterfaces();
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x001D3198 File Offset: 0x001D2198
		private NativeMethods.SIZE SetExtent(int width, int height)
		{
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			size.cx = width;
			size.cy = height;
			bool flag = false;
			try
			{
				this._axOleObject.SetExtent(1, size);
			}
			catch (COMException)
			{
				flag = true;
			}
			if (flag)
			{
				this._axOleObject.GetExtent(1, size);
				try
				{
					this._axOleObject.SetExtent(1, size);
				}
				catch (COMException)
				{
				}
			}
			return this.GetExtent();
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x001D3218 File Offset: 0x001D2218
		private NativeMethods.SIZE GetExtent()
		{
			NativeMethods.SIZE size = new NativeMethods.SIZE();
			this._axOleObject.GetExtent(1, size);
			return size;
		}

		// Token: 0x04001BFB RID: 7163
		internal static readonly DependencyProperty TabIndexProperty = Control.TabIndexProperty.AddOwner(typeof(ActiveXHost));

		// Token: 0x04001BFC RID: 7164
		private static Hashtable invalidatorMap = new Hashtable();

		// Token: 0x04001BFD RID: 7165
		private NativeMethods.COMRECT _bounds = new NativeMethods.COMRECT(0, 0, 0, 0);

		// Token: 0x04001BFE RID: 7166
		private Rect _boundRect = new Rect(0.0, 0.0, 0.0, 0.0);

		// Token: 0x04001BFF RID: 7167
		private Size _cachedSize = Size.Empty;

		// Token: 0x04001C00 RID: 7168
		private HandleRef _hwndParent;

		// Token: 0x04001C01 RID: 7169
		private bool _isDisposed;

		// Token: 0x04001C02 RID: 7170
		private SecurityCriticalDataForSet<Guid> _clsid;

		// Token: 0x04001C03 RID: 7171
		private HandleRef _axWindow;

		// Token: 0x04001C04 RID: 7172
		private BitVector32 _axHostState;

		// Token: 0x04001C05 RID: 7173
		private ActiveXHelper.ActiveXState _axState;

		// Token: 0x04001C06 RID: 7174
		private ActiveXSite _axSite;

		// Token: 0x04001C07 RID: 7175
		private ActiveXContainer _axContainer;

		// Token: 0x04001C08 RID: 7176
		private object _axInstance;

		// Token: 0x04001C09 RID: 7177
		private UnsafeNativeMethods.IOleObject _axOleObject;

		// Token: 0x04001C0A RID: 7178
		private UnsafeNativeMethods.IOleInPlaceObject _axOleInPlaceObject;

		// Token: 0x04001C0B RID: 7179
		private UnsafeNativeMethods.IOleInPlaceActiveObject _axOleInPlaceActiveObject;

		// Token: 0x02000AB8 RID: 2744
		// (Invoke) Token: 0x06008ABD RID: 35517
		private delegate void PropertyInvalidator(ActiveXHost axhost);
	}
}
