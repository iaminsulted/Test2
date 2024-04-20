using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.KnownBoxes;
using MS.Win32;

namespace System.Windows
{
	// Token: 0x020003E1 RID: 993
	[Localizability(LocalizationCategory.Ignore)]
	public class Window : ContentControl, IWindowService
	{
		// Token: 0x060029D0 RID: 10704 RVA: 0x0019ADA8 File Offset: 0x00199DA8
		static Window()
		{
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnHeightChanged)));
			FrameworkElement.MinHeightProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMinHeightChanged)));
			FrameworkElement.MaxHeightProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMaxHeightChanged)));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnWidthChanged)));
			FrameworkElement.MinWidthProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMinWidthChanged)));
			FrameworkElement.MaxWidthProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnMaxWidthChanged)));
			UIElement.VisibilityProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(Window._OnVisibilityChanged), new CoerceValueCallback(Window.CoerceVisibility)));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
			Window._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Window));
			FrameworkElement.FlowDirectionProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnFlowDirectionChanged)));
			UIElement.RenderTransformProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(Transform.Identity, new PropertyChangedCallback(Window._OnRenderTransformChanged), new CoerceValueCallback(Window.CoerceRenderTransform)));
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Window._OnClipToBoundsChanged), new CoerceValueCallback(Window.CoerceClipToBounds)));
			Window.WM_TASKBARBUTTONCREATED = UnsafeNativeMethods.RegisterWindowMessage("TaskbarButtonCreated");
			Window.WM_APPLYTASKBARITEMINFO = UnsafeNativeMethods.RegisterWindowMessage("WPF_ApplyTaskbarItemInfo");
			EventManager.RegisterClassHandler(typeof(Window), UIElement.ManipulationCompletedEvent, new EventHandler<ManipulationCompletedEventArgs>(Window.OnStaticManipulationCompleted), true);
			EventManager.RegisterClassHandler(typeof(Window), UIElement.ManipulationInertiaStartingEvent, new EventHandler<ManipulationInertiaStartingEventArgs>(Window.OnStaticManipulationInertiaStarting), true);
			Window.DpiChangedEvent = EventManager.RegisterRoutedEvent("DpiChanged", RoutingStrategy.Bubble, typeof(DpiChangedEventHandler), typeof(Window));
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x0019B528 File Offset: 0x0019A528
		public Window()
		{
			this._inTrustedSubWindow = false;
			this.Initialize();
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x0019B5E4 File Offset: 0x0019A5E4
		internal Window(bool inRbw)
		{
			if (inRbw)
			{
				this._inTrustedSubWindow = true;
			}
			else
			{
				this._inTrustedSubWindow = false;
			}
			this.Initialize();
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x0019B6AB File Offset: 0x0019A6AB
		public void Show()
		{
			this.VerifyContextAndObjectState();
			this.VerifyCanShow();
			this.VerifyNotClosing();
			this.VerifyConsistencyWithAllowsTransparency();
			this.UpdateVisibilityProperty(Visibility.Visible);
			this.ShowHelper(BooleanBoxes.TrueBox);
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x0019B6D8 File Offset: 0x0019A6D8
		public void Hide()
		{
			this.VerifyContextAndObjectState();
			if (this._disposed)
			{
				return;
			}
			this.UpdateVisibilityProperty(Visibility.Hidden);
			this.ShowHelper(BooleanBoxes.FalseBox);
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x0019B6FC File Offset: 0x0019A6FC
		public void Close()
		{
			this.VerifyApiSupported();
			this.VerifyContextAndObjectState();
			this.InternalClose(false, false);
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0019B714 File Offset: 0x0019A714
		public void DragMove()
		{
			this.VerifyApiSupported();
			this.VerifyContextAndObjectState();
			this.VerifyHwndCreateShowState();
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return;
			}
			if (Mouse.LeftButton != MouseButtonState.Pressed)
			{
				throw new InvalidOperationException(SR.Get("DragMoveFail"));
			}
			if (this.WindowState == WindowState.Normal)
			{
				UnsafeNativeMethods.SendMessage(this.CriticalHandle, WindowMessage.WM_SYSCOMMAND, (IntPtr)61458, IntPtr.Zero);
				UnsafeNativeMethods.SendMessage(this.CriticalHandle, WindowMessage.WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
				return;
			}
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x0019B7A0 File Offset: 0x0019A7A0
		public bool? ShowDialog()
		{
			this.VerifyApiSupported();
			this.VerifyContextAndObjectState();
			this.VerifyCanShow();
			this.VerifyNotClosing();
			this.VerifyConsistencyWithAllowsTransparency();
			if (this._isVisible)
			{
				throw new InvalidOperationException(SR.Get("ShowDialogOnVisible"));
			}
			if (this._showingAsDialog)
			{
				throw new InvalidOperationException(SR.Get("ShowDialogOnModal"));
			}
			this._dialogOwnerHandle = this._ownerHandle;
			if (!UnsafeNativeMethods.IsWindow(new HandleRef(null, this._dialogOwnerHandle)))
			{
				this._dialogOwnerHandle = IntPtr.Zero;
			}
			this._dialogPreviousActiveHandle = UnsafeNativeMethods.GetActiveWindow();
			if (this._dialogOwnerHandle == IntPtr.Zero)
			{
				this._dialogOwnerHandle = this._dialogPreviousActiveHandle;
			}
			if (this._dialogOwnerHandle != IntPtr.Zero && this._dialogOwnerHandle == UnsafeNativeMethods.GetDesktopWindow())
			{
				this._dialogOwnerHandle = IntPtr.Zero;
			}
			if (this._dialogOwnerHandle != IntPtr.Zero)
			{
				while (this._dialogOwnerHandle != IntPtr.Zero && (UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this._dialogOwnerHandle), -16) & 1073741824) == 1073741824)
				{
					this._dialogOwnerHandle = UnsafeNativeMethods.GetParent(new HandleRef(null, this._dialogOwnerHandle));
				}
			}
			this._threadWindowHandles = new ArrayList();
			UnsafeNativeMethods.EnumThreadWindows(SafeNativeMethods.GetCurrentThreadId(), new NativeMethods.EnumThreadWindowsCallback(this.ThreadWindowsCallback), NativeMethods.NullHandleRef);
			this.EnableThreadWindows(false);
			if (SafeNativeMethods.GetCapture() != IntPtr.Zero)
			{
				SafeNativeMethods.ReleaseCapture();
			}
			this.EnsureDialogCommand();
			try
			{
				this._showingAsDialog = true;
				this.Show();
			}
			catch
			{
				if (this._threadWindowHandles != null)
				{
					this.EnableThreadWindows(true);
				}
				if (this._dialogPreviousActiveHandle != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this._dialogPreviousActiveHandle)))
				{
					UnsafeNativeMethods.TrySetFocus(new HandleRef(null, this._dialogPreviousActiveHandle), ref this._dialogPreviousActiveHandle);
				}
				this.ClearShowKeyboardCueState();
				this._showingAsDialog = false;
				throw;
			}
			finally
			{
				this._showingAsDialog = false;
			}
			return this._dialogResult;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x0019B9BC File Offset: 0x0019A9BC
		public bool Activate()
		{
			this.VerifyApiSupported();
			this.VerifyContextAndObjectState();
			this.VerifyHwndCreateShowState();
			return !this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && UnsafeNativeMethods.SetForegroundWindow(new HandleRef(null, this.CriticalHandle));
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x0019B9F3 File Offset: 0x0019A9F3
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new SingleChildEnumerator(base.Content);
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x0019BA00 File Offset: 0x0019AA00
		public static Window GetWindow(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return dependencyObject.GetValue(Window.IWindowServiceProperty) as Window;
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x0019BA20 File Offset: 0x0019AA20
		// (set) Token: 0x060029DC RID: 10716 RVA: 0x0019BA38 File Offset: 0x0019AA38
		public TaskbarItemInfo TaskbarItemInfo
		{
			get
			{
				this.VerifyContextAndObjectState();
				return (TaskbarItemInfo)base.GetValue(Window.TaskbarItemInfoProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				base.SetValue(Window.TaskbarItemInfoProperty, value);
			}
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x0019BA4C File Offset: 0x0019AA4C
		private void OnTaskbarItemInfoChanged(DependencyPropertyChangedEventArgs e)
		{
			TaskbarItemInfo taskbarItemInfo = (TaskbarItemInfo)e.OldValue;
			TaskbarItemInfo taskbarItemInfo2 = (TaskbarItemInfo)e.NewValue;
			if (!Utilities.IsOSWindows7OrNewer)
			{
				return;
			}
			if (!e.IsASubPropertyChange)
			{
				if (taskbarItemInfo != null)
				{
					taskbarItemInfo.PropertyChanged -= this.OnTaskbarItemInfoSubPropertyChanged;
				}
				if (taskbarItemInfo2 != null)
				{
					taskbarItemInfo2.PropertyChanged += this.OnTaskbarItemInfoSubPropertyChanged;
				}
				this.ApplyTaskbarItemInfo();
			}
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x0019BAB4 File Offset: 0x0019AAB4
		private void HandleTaskbarListError(HRESULT hr)
		{
			if (hr.Failed)
			{
				if (hr == (HRESULT)Win32Error.ERROR_TIMEOUT)
				{
					if (TraceShell.IsEnabled)
					{
						TraceShell.Trace(TraceEventType.Error, TraceShell.ExplorerTaskbarTimeout);
						TraceShell.Trace(TraceEventType.Warning, TraceShell.ExplorerTaskbarRetrying);
					}
					this._taskbarRetryTimer.Start();
					return;
				}
				if (hr == (HRESULT)Win32Error.ERROR_INVALID_WINDOW_HANDLE)
				{
					if (TraceShell.IsEnabled)
					{
						TraceShell.Trace(TraceEventType.Warning, TraceShell.ExplorerTaskbarNotRunning);
					}
					Utilities.SafeRelease<ITaskbarList3>(ref this._taskbarList);
					return;
				}
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Error, TraceShell.NativeTaskbarError(new object[]
					{
						hr.ToString()
					}));
				}
			}
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x0019BB60 File Offset: 0x0019AB60
		private void OnTaskbarItemInfoSubPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (sender != this.TaskbarItemInfo)
			{
				return;
			}
			if (this._taskbarList == null)
			{
				return;
			}
			if (this._taskbarRetryTimer != null && this._taskbarRetryTimer.IsEnabled)
			{
				return;
			}
			DependencyProperty property = e.Property;
			HRESULT hr = HRESULT.S_OK;
			if (property == TaskbarItemInfo.ProgressStateProperty)
			{
				hr = this.UpdateTaskbarProgressState();
			}
			else if (property == TaskbarItemInfo.ProgressValueProperty)
			{
				hr = this.UpdateTaskbarProgressValue();
			}
			else if (property == TaskbarItemInfo.OverlayProperty)
			{
				hr = this.UpdateTaskbarOverlay();
			}
			else if (property == TaskbarItemInfo.DescriptionProperty)
			{
				hr = this.UpdateTaskbarDescription();
			}
			else if (property == TaskbarItemInfo.ThumbnailClipMarginProperty)
			{
				hr = this.UpdateTaskbarThumbnailClipping();
			}
			else if (property == TaskbarItemInfo.ThumbButtonInfosProperty)
			{
				hr = this.UpdateTaskbarThumbButtons();
			}
			this.HandleTaskbarListError(hr);
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x0019BC0F File Offset: 0x0019AC0F
		// (set) Token: 0x060029E1 RID: 10721 RVA: 0x0019BC21 File Offset: 0x0019AC21
		public bool AllowsTransparency
		{
			get
			{
				return (bool)base.GetValue(Window.AllowsTransparencyProperty);
			}
			set
			{
				base.SetValue(Window.AllowsTransparencyProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnAllowsTransparencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x0019BC34 File Offset: 0x0019AC34
		private static object CoerceAllowsTransparency(DependencyObject d, object value)
		{
			value = Window.VerifyAccessCoercion(d, value);
			if (!((Window)d).IsSourceWindowNull)
			{
				throw new InvalidOperationException(SR.Get("ChangeNotAllowedAfterShow"));
			}
			return value;
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x0019BC5D File Offset: 0x0019AC5D
		// (set) Token: 0x060029E5 RID: 10725 RVA: 0x0019BC75 File Offset: 0x0019AC75
		[Localizability(LocalizationCategory.Title)]
		public string Title
		{
			get
			{
				this.VerifyContextAndObjectState();
				return (string)base.GetValue(Window.TitleProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				base.SetValue(Window.TitleProperty, value);
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x0019BC89 File Offset: 0x0019AC89
		// (set) Token: 0x060029E7 RID: 10727 RVA: 0x0019BCA7 File Offset: 0x0019ACA7
		public ImageSource Icon
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (ImageSource)base.GetValue(Window.IconProperty);
			}
			set
			{
				this.VerifyApiSupported();
				this.VerifyContextAndObjectState();
				base.SetValue(Window.IconProperty, value);
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x0019BCC1 File Offset: 0x0019ACC1
		// (set) Token: 0x060029E9 RID: 10729 RVA: 0x0019BCDF File Offset: 0x0019ACDF
		public SizeToContent SizeToContent
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (SizeToContent)base.GetValue(Window.SizeToContentProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.SizeToContentProperty, value);
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060029EA RID: 10730 RVA: 0x0019BCFE File Offset: 0x0019ACFE
		// (set) Token: 0x060029EB RID: 10731 RVA: 0x0019BD1C File Offset: 0x0019AD1C
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double Top
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (double)base.GetValue(Window.TopProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.TopProperty, value);
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060029EC RID: 10732 RVA: 0x0019BD3B File Offset: 0x0019AD3B
		// (set) Token: 0x060029ED RID: 10733 RVA: 0x0019BD59 File Offset: 0x0019AD59
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double Left
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (double)base.GetValue(Window.LeftProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.LeftProperty, value);
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x0019BD78 File Offset: 0x0019AD78
		public Rect RestoreBounds
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
				{
					return Rect.Empty;
				}
				return this.GetNormalRectLogicalUnits(this.CriticalHandle);
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x0019BDA8 File Offset: 0x0019ADA8
		// (set) Token: 0x060029F0 RID: 10736 RVA: 0x0019BDBC File Offset: 0x0019ADBC
		[DefaultValue(WindowStartupLocation.Manual)]
		public WindowStartupLocation WindowStartupLocation
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return this._windowStartupLocation;
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				if (!Window.IsValidWindowStartupLocation(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(WindowStartupLocation));
				}
				this._windowStartupLocation = value;
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x0019BDEF File Offset: 0x0019ADEF
		// (set) Token: 0x060029F2 RID: 10738 RVA: 0x0019BE0D File Offset: 0x0019AE0D
		public bool ShowInTaskbar
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (bool)base.GetValue(Window.ShowInTaskbarProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.ShowInTaskbarProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x0019BE2C File Offset: 0x0019AE2C
		public bool IsActive
		{
			get
			{
				this.VerifyContextAndObjectState();
				return (bool)base.GetValue(Window.IsActiveProperty);
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060029F4 RID: 10740 RVA: 0x0019BE44 File Offset: 0x0019AE44
		// (set) Token: 0x060029F5 RID: 10741 RVA: 0x0019BE58 File Offset: 0x0019AE58
		[DefaultValue(null)]
		public Window Owner
		{
			get
			{
				this.VerifyApiSupported();
				this.VerifyContextAndObjectState();
				return this._ownerWindow;
			}
			set
			{
				this.VerifyApiSupported();
				this.VerifyContextAndObjectState();
				if (value == this)
				{
					throw new ArgumentException(SR.Get("CannotSetOwnerToItself"));
				}
				if (this._showingAsDialog)
				{
					throw new InvalidOperationException(SR.Get("CantSetOwnerAfterDialogIsShown"));
				}
				if (value != null && value.IsSourceWindowNull)
				{
					if (value._disposed)
					{
						throw new InvalidOperationException(SR.Get("CantSetOwnerToClosedWindow"));
					}
					throw new InvalidOperationException(SR.Get("CantSetOwnerWhosHwndIsNotCreated"));
				}
				else
				{
					if (this._ownerWindow == value)
					{
						return;
					}
					if (!this._disposed)
					{
						if (value != null)
						{
							WindowCollection ownedWindows = this.OwnedWindows;
							for (int i = 0; i < ownedWindows.Count; i++)
							{
								if (ownedWindows[i] == value)
								{
									throw new ArgumentException(SR.Get("CircularOwnerChild", new object[]
									{
										value,
										this
									}));
								}
							}
						}
						if (this._ownerWindow != null)
						{
							this._ownerWindow.OwnedWindowsInternal.Remove(this);
						}
					}
					this._ownerWindow = value;
					if (this._disposed)
					{
						return;
					}
					this.SetOwnerHandle((this._ownerWindow != null) ? this._ownerWindow.CriticalHandle : IntPtr.Zero);
					if (this._ownerWindow != null)
					{
						this._ownerWindow.OwnedWindowsInternal.Add(this);
					}
					return;
				}
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x0019BF88 File Offset: 0x0019AF88
		private bool IsOwnerNull
		{
			get
			{
				return this._ownerWindow == null;
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x0019BF93 File Offset: 0x0019AF93
		public WindowCollection OwnedWindows
		{
			get
			{
				this.VerifyContextAndObjectState();
				return this.OwnedWindowsInternal.Clone();
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x0019BFA6 File Offset: 0x0019AFA6
		// (set) Token: 0x060029F9 RID: 10745 RVA: 0x0019BFBC File Offset: 0x0019AFBC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[TypeConverter(typeof(DialogResultConverter))]
		public bool? DialogResult
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return this._dialogResult;
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				if (this._showingAsDialog)
				{
					bool? dialogResult = this._dialogResult;
					bool? flag = value;
					if (!(dialogResult.GetValueOrDefault() == flag.GetValueOrDefault() & dialogResult != null == (flag != null)))
					{
						this._dialogResult = value;
						if (!this._isClosing)
						{
							this.Close();
							return;
						}
					}
					return;
				}
				throw new InvalidOperationException(SR.Get("DialogResultMustBeSetAfterShowDialog"));
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060029FA RID: 10746 RVA: 0x0019C02F File Offset: 0x0019B02F
		// (set) Token: 0x060029FB RID: 10747 RVA: 0x0019C04D File Offset: 0x0019B04D
		public WindowStyle WindowStyle
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (WindowStyle)base.GetValue(Window.WindowStyleProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.WindowStyleProperty, value);
			}
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x0019C06C File Offset: 0x0019B06C
		private static object CoerceWindowStyle(DependencyObject d, object value)
		{
			value = Window.VerifyAccessCoercion(d, value);
			if (!((Window)d).IsSourceWindowNull)
			{
				((Window)d).VerifyConsistencyWithAllowsTransparency((WindowStyle)value);
			}
			return value;
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060029FD RID: 10749 RVA: 0x0019C096 File Offset: 0x0019B096
		// (set) Token: 0x060029FE RID: 10750 RVA: 0x0019C0B4 File Offset: 0x0019B0B4
		public WindowState WindowState
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (WindowState)base.GetValue(Window.WindowStateProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.WindowStateProperty, value);
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x0019C0D3 File Offset: 0x0019B0D3
		// (set) Token: 0x06002A00 RID: 10752 RVA: 0x0019C0F1 File Offset: 0x0019B0F1
		public ResizeMode ResizeMode
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (ResizeMode)base.GetValue(Window.ResizeModeProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.ResizeModeProperty, value);
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06002A01 RID: 10753 RVA: 0x0019C110 File Offset: 0x0019B110
		// (set) Token: 0x06002A02 RID: 10754 RVA: 0x0019C12E File Offset: 0x0019B12E
		public bool Topmost
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (bool)base.GetValue(Window.TopmostProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.TopmostProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002A03 RID: 10755 RVA: 0x0019C14D File Offset: 0x0019B14D
		// (set) Token: 0x06002A04 RID: 10756 RVA: 0x0019C16B File Offset: 0x0019B16B
		public bool ShowActivated
		{
			get
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				return (bool)base.GetValue(Window.ShowActivatedProperty);
			}
			set
			{
				this.VerifyContextAndObjectState();
				this.VerifyApiSupported();
				base.SetValue(Window.ShowActivatedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06002A05 RID: 10757 RVA: 0x0019C18A File Offset: 0x0019B18A
		// (remove) Token: 0x06002A06 RID: 10758 RVA: 0x0019C19D File Offset: 0x0019B19D
		public event EventHandler SourceInitialized
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_SOURCEINITIALIZED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_SOURCEINITIALIZED, value);
			}
		}

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06002A07 RID: 10759 RVA: 0x0019C1B0 File Offset: 0x0019B1B0
		// (remove) Token: 0x06002A08 RID: 10760 RVA: 0x0019C1BE File Offset: 0x0019B1BE
		public event DpiChangedEventHandler DpiChanged
		{
			add
			{
				base.AddHandler(Window.DpiChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Window.DpiChangedEvent, value);
			}
		}

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06002A09 RID: 10761 RVA: 0x0019C1CC File Offset: 0x0019B1CC
		// (remove) Token: 0x06002A0A RID: 10762 RVA: 0x0019C1DF File Offset: 0x0019B1DF
		public event EventHandler Activated
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_ACTIVATED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_ACTIVATED, value);
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06002A0B RID: 10763 RVA: 0x0019C1F2 File Offset: 0x0019B1F2
		// (remove) Token: 0x06002A0C RID: 10764 RVA: 0x0019C205 File Offset: 0x0019B205
		public event EventHandler Deactivated
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_DEACTIVATED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_DEACTIVATED, value);
			}
		}

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06002A0D RID: 10765 RVA: 0x0019C218 File Offset: 0x0019B218
		// (remove) Token: 0x06002A0E RID: 10766 RVA: 0x0019C22B File Offset: 0x0019B22B
		public event EventHandler StateChanged
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_STATECHANGED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_STATECHANGED, value);
			}
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06002A0F RID: 10767 RVA: 0x0019C23E File Offset: 0x0019B23E
		// (remove) Token: 0x06002A10 RID: 10768 RVA: 0x0019C251 File Offset: 0x0019B251
		public event EventHandler LocationChanged
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_LOCATIONCHANGED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_LOCATIONCHANGED, value);
			}
		}

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06002A11 RID: 10769 RVA: 0x0019C264 File Offset: 0x0019B264
		// (remove) Token: 0x06002A12 RID: 10770 RVA: 0x0019C277 File Offset: 0x0019B277
		public event CancelEventHandler Closing
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_CLOSING, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_CLOSING, value);
			}
		}

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06002A13 RID: 10771 RVA: 0x0019C28A File Offset: 0x0019B28A
		// (remove) Token: 0x06002A14 RID: 10772 RVA: 0x0019C29D File Offset: 0x0019B29D
		public event EventHandler Closed
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_CLOSED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_CLOSED, value);
			}
		}

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06002A15 RID: 10773 RVA: 0x0019C2B0 File Offset: 0x0019B2B0
		// (remove) Token: 0x06002A16 RID: 10774 RVA: 0x0019C2C3 File Offset: 0x0019B2C3
		public event EventHandler ContentRendered
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_CONTENTRENDERED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_CONTENTRENDERED, value);
			}
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06002A17 RID: 10775 RVA: 0x0019C2D6 File Offset: 0x0019B2D6
		// (remove) Token: 0x06002A18 RID: 10776 RVA: 0x0019C2E9 File Offset: 0x0019B2E9
		internal event EventHandler<EventArgs> VisualChildrenChanged
		{
			add
			{
				this.Events.AddHandler(Window.EVENT_VISUALCHILDRENCHANGED, value);
			}
			remove
			{
				this.Events.RemoveHandler(Window.EVENT_VISUALCHILDRENCHANGED, value);
			}
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x0019C2FC File Offset: 0x0019B2FC
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new WindowAutomationPeer(this);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x0019C304 File Offset: 0x0019B304
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			base.RaiseEvent(new DpiChangedEventArgs(oldDpi, newDpi, Window.DpiChangedEvent, this));
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x0019C319 File Offset: 0x0019B319
		protected internal sealed override void OnVisualParentChanged(DependencyObject oldParent)
		{
			this.VerifyContextAndObjectState();
			base.OnVisualParentChanged(oldParent);
			if (VisualTreeHelper.GetParent(this) != null)
			{
				throw new InvalidOperationException(SR.Get("WindowMustBeRoot"));
			}
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x0019C340 File Offset: 0x0019B340
		protected override Size MeasureOverride(Size availableSize)
		{
			this.VerifyContextAndObjectState();
			Size constraint = new Size(availableSize.Width, availableSize.Height);
			Window.WindowMinMax windowMinMax = this.GetWindowMinMax();
			constraint.Width = Math.Max(windowMinMax.minWidth, Math.Min(constraint.Width, windowMinMax.maxWidth));
			constraint.Height = Math.Max(windowMinMax.minHeight, Math.Min(constraint.Height, windowMinMax.maxHeight));
			Size result = this.MeasureOverrideHelper(constraint);
			result = new Size(Math.Max(result.Width, windowMinMax.minWidth), Math.Max(result.Height, windowMinMax.minHeight));
			return result;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x0019C3EC File Offset: 0x0019B3EC
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			this.VerifyContextAndObjectState();
			Window.WindowMinMax windowMinMax = this.GetWindowMinMax();
			arrangeBounds.Width = Math.Max(windowMinMax.minWidth, Math.Min(arrangeBounds.Width, windowMinMax.maxWidth));
			arrangeBounds.Height = Math.Max(windowMinMax.minHeight, Math.Min(arrangeBounds.Height, windowMinMax.maxHeight));
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return arrangeBounds;
			}
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					Size hwndNonClientAreaSizeInMeasureUnits = this.GetHwndNonClientAreaSizeInMeasureUnits();
					Size size = new Size
					{
						Width = Math.Max(0.0, arrangeBounds.Width - hwndNonClientAreaSizeInMeasureUnits.Width),
						Height = Math.Max(0.0, arrangeBounds.Height - hwndNonClientAreaSizeInMeasureUnits.Height)
					};
					uielement.Arrange(new Rect(size));
					if (base.FlowDirection == FlowDirection.RightToLeft)
					{
						FrameworkElement.InternalSetLayoutTransform(uielement, new MatrixTransform(-1.0, 0.0, 0.0, 1.0, size.Width, 0.0));
					}
				}
			}
			return arrangeBounds;
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x0019C52B File Offset: 0x0019B52B
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			this.SetIWindowService();
			if (base.IsLoaded)
			{
				this.PostContentRendered();
				return;
			}
			if (!this._postContentRenderedFromLoadedHandler)
			{
				base.Loaded += this.LoadedHandler;
				this._postContentRenderedFromLoadedHandler = true;
			}
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x0019C56C File Offset: 0x0019B56C
		protected virtual void OnSourceInitialized(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_SOURCEINITIALIZED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x0019C5A0 File Offset: 0x0019B5A0
		protected virtual void OnActivated(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_ACTIVATED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x0019C5D4 File Offset: 0x0019B5D4
		protected virtual void OnDeactivated(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_DEACTIVATED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x0019C608 File Offset: 0x0019B608
		protected virtual void OnStateChanged(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_STATECHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x0019C63C File Offset: 0x0019B63C
		protected virtual void OnLocationChanged(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_LOCATIONCHANGED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x0019C670 File Offset: 0x0019B670
		protected virtual void OnClosing(CancelEventArgs e)
		{
			this.VerifyContextAndObjectState();
			CancelEventHandler cancelEventHandler = (CancelEventHandler)this.Events[Window.EVENT_CLOSING];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x0019C6A4 File Offset: 0x0019B6A4
		protected virtual void OnClosed(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_CLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x0019C6D8 File Offset: 0x0019B6D8
		protected virtual void OnContentRendered(EventArgs e)
		{
			this.VerifyContextAndObjectState();
			DependencyObject dependencyObject = base.Content as DependencyObject;
			if (dependencyObject != null)
			{
				IInputElement focusedElement = FocusManager.GetFocusedElement(dependencyObject);
				if (focusedElement != null)
				{
					focusedElement.Focus();
				}
			}
			EventHandler eventHandler = (EventHandler)this.Events[Window.EVENT_CONTENTRENDERED];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x0019C72C File Offset: 0x0019B72C
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			this.VerifyContextAndObjectState();
			EventHandler<EventArgs> eventHandler = this.Events[Window.EVENT_VISUALCHILDRENCHANGED] as EventHandler<EventArgs>;
			if (eventHandler == null)
			{
				return;
			}
			eventHandler(this, new EventArgs());
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x0019C75C File Offset: 0x0019B75C
		internal Point DeviceToLogicalUnits(Point ptDeviceUnits)
		{
			Invariant.Assert(!this.IsCompositionTargetInvalid, "IsCompositionTargetInvalid is supposed to be false here");
			return this._swh.CompositionTarget.TransformFromDevice.Transform(ptDeviceUnits);
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x0019C798 File Offset: 0x0019B798
		internal Point LogicalToDeviceUnits(Point ptLogicalUnits)
		{
			Invariant.Assert(!this.IsCompositionTargetInvalid, "IsCompositionTargetInvalid is supposed to be false here");
			return this._swh.CompositionTarget.TransformToDevice.Transform(ptLogicalUnits);
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x0019C7D1 File Offset: 0x0019B7D1
		internal static bool VisibilityToBool(Visibility v)
		{
			return v == Visibility.Visible || (v - Visibility.Hidden > 1 && false);
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x0019C7E3 File Offset: 0x0019B7E3
		internal virtual void SetResizeGripControl(Control ctrl)
		{
			this._resizeGripControl = ctrl;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x0019C7EC File Offset: 0x0019B7EC
		internal virtual void ClearResizeGripControl(Control oldCtrl)
		{
			if (oldCtrl == this._resizeGripControl)
			{
				this._resizeGripControl = null;
			}
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x0019C7FE File Offset: 0x0019B7FE
		internal virtual void TryClearingMainWindow()
		{
			if (this.IsInsideApp && this == this.App.MainWindow)
			{
				this.App.MainWindow = null;
			}
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x0019C824 File Offset: 0x0019B824
		internal void InternalClose(bool shutdown, bool ignoreCancel)
		{
			this.VerifyNotClosing();
			if (this._disposed)
			{
				return;
			}
			this._appShuttingDown = shutdown;
			this._ignoreCancel = ignoreCancel;
			if (!this.IsSourceWindowNull)
			{
				UnsafeNativeMethods.UnsafeSendMessage(this.CriticalHandle, WindowMessage.WM_CLOSE, (IntPtr)0, (IntPtr)0);
				return;
			}
			this._isClosing = true;
			CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
			try
			{
				this.OnClosing(cancelEventArgs);
			}
			catch
			{
				this.CloseWindowBeforeShow();
				throw;
			}
			if (this.ShouldCloseWindow(cancelEventArgs.Cancel))
			{
				this.CloseWindowBeforeShow();
				return;
			}
			this._isClosing = false;
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x0019C8B8 File Offset: 0x0019B8B8
		private void CloseWindowBeforeShow()
		{
			this.InternalDispose();
			this.OnClosed(EventArgs.Empty);
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06002A30 RID: 10800 RVA: 0x0019C8CB File Offset: 0x0019B8CB
		internal bool IsSourceWindowNull
		{
			get
			{
				return this._swh == null || this._swh.IsSourceWindowNull;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06002A31 RID: 10801 RVA: 0x0019C8E2 File Offset: 0x0019B8E2
		internal bool IsCompositionTargetInvalid
		{
			get
			{
				return this._swh == null || this._swh.IsCompositionTargetInvalid;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x0019C8F9 File Offset: 0x0019B8F9
		internal NativeMethods.RECT WorkAreaBoundsForNearestMonitor
		{
			get
			{
				return this._swh.WorkAreaBoundsForNearestMonitor;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x0019C906 File Offset: 0x0019B906
		internal Size WindowSize
		{
			get
			{
				return this._swh.WindowSize;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x0019C913 File Offset: 0x0019B913
		internal HwndSource HwndSourceWindow
		{
			get
			{
				if (this._swh != null)
				{
					return this._swh.HwndSourceWindow;
				}
				return null;
			}
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x0019C92C File Offset: 0x0019B92C
		private void InternalDispose()
		{
			this._disposed = true;
			this.UpdateWindowListsOnClose();
			if (this._taskbarRetryTimer != null)
			{
				this._taskbarRetryTimer.Stop();
				this._taskbarRetryTimer = null;
			}
			try
			{
				this.ClearSourceWindow();
				Utilities.SafeDispose<HwndWrapper>(ref this._hiddenWindow);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._defaultLargeIconHandle);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._defaultSmallIconHandle);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._currentLargeIconHandle);
				Utilities.SafeDispose<NativeMethods.IconHandle>(ref this._currentSmallIconHandle);
				Utilities.SafeRelease<ITaskbarList3>(ref this._taskbarList);
			}
			finally
			{
				this._isClosing = false;
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x0019C9C4 File Offset: 0x0019B9C4
		internal override void OnAncestorChanged()
		{
			base.OnAncestorChanged();
			if (base.Parent != null)
			{
				throw new InvalidOperationException(SR.Get("WindowMustBeRoot"));
			}
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x0019C9E4 File Offset: 0x0019B9E4
		internal virtual void CreateAllStyle()
		{
			this._Style = 34078720;
			this._StyleEx = 0;
			this.CreateWindowStyle();
			this.CreateWindowState();
			if (this._isVisible)
			{
				this._Style |= 268435456;
			}
			this.SetTaskbarStatus();
			this.CreateTopmost();
			this.CreateResizibility();
			this.CreateRtl();
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x0019CA41 File Offset: 0x0019BA41
		internal virtual void CreateSourceWindowDuringShow()
		{
			this.CreateSourceWindow(true);
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x0019CA4C File Offset: 0x0019BA4C
		internal void CreateSourceWindow(bool duringShow)
		{
			this.VerifyContextAndObjectState();
			this.VerifyCanShow();
			this.VerifyNotClosing();
			this.VerifyConsistencyWithAllowsTransparency();
			if (!duringShow)
			{
				this.VerifyApiSupported();
			}
			double num = 0.0;
			double num2 = 0.0;
			double requestedWidth = 0.0;
			double requestedHeight = 0.0;
			this.GetRequestedDimensions(ref num2, ref num, ref requestedWidth, ref requestedHeight);
			Window.WindowStartupTopLeftPointHelper windowStartupTopLeftPointHelper = new Window.WindowStartupTopLeftPointHelper(new Point(num2, num));
			using (Window.HwndStyleManager hwndStyleManager = Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
			{
				this.CreateAllStyle();
				HwndSourceParameters parameters = this.CreateHwndSourceParameters();
				if (windowStartupTopLeftPointHelper.ScreenTopLeft != null)
				{
					Point value = windowStartupTopLeftPointHelper.ScreenTopLeft.Value;
					parameters.SetPosition((int)value.X, (int)value.Y);
				}
				HwndSource hwndSource = new HwndSource(parameters);
				this._swh = new Window.SourceWindowHelper(hwndSource);
				hwndSource.SizeToContentChanged += this.OnSourceSizeToContentChanged;
				hwndStyleManager.Dirty = false;
				this.CorrectStyleForBorderlessWindowCase();
			}
			this._swh.AddDisposedHandler(new EventHandler(this.OnSourceWindowDisposed));
			this._hwndCreatedButNotShown = !duringShow;
			if (Utilities.IsOSWindows7OrNewer)
			{
				MSGFLTINFO msgfltinfo;
				UnsafeNativeMethods.ChangeWindowMessageFilterEx(this._swh.CriticalHandle, Window.WM_TASKBARBUTTONCREATED, MSGFLT.ALLOW, out msgfltinfo);
				UnsafeNativeMethods.ChangeWindowMessageFilterEx(this._swh.CriticalHandle, WindowMessage.WM_COMMAND, MSGFLT.ALLOW, out msgfltinfo);
			}
			this.SetupInitialState(num, num2, requestedWidth, requestedHeight);
			this.OnSourceInitialized(EventArgs.Empty);
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x0019CBE0 File Offset: 0x0019BBE0
		internal virtual HwndSourceParameters CreateHwndSourceParameters()
		{
			return new HwndSourceParameters(this.Title, int.MinValue, int.MinValue)
			{
				UsesPerPixelOpacity = this.AllowsTransparency,
				WindowStyle = this._Style,
				ExtendedWindowStyle = this._StyleEx,
				ParentWindow = this._ownerHandle,
				AdjustSizingForNonClientArea = true,
				HwndSourceHook = new HwndSourceHook(this.WindowFilterMessage)
			};
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x0019CC54 File Offset: 0x0019BC54
		private void OnSourceSizeToContentChanged(object sender, EventArgs args)
		{
			this.SizeToContent = this.HwndSourceSizeToContent;
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x0019CC64 File Offset: 0x0019BC64
		internal virtual void CorrectStyleForBorderlessWindowCase()
		{
			using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
			{
				if (this.WindowStyle == WindowStyle.None)
				{
					this._Style = this._swh.StyleFromHwnd;
					this._Style &= -12582913;
				}
			}
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x0019CCCC File Offset: 0x0019BCCC
		internal virtual void GetRequestedDimensions(ref double requestedLeft, ref double requestedTop, ref double requestedWidth, ref double requestedHeight)
		{
			requestedTop = this.Top;
			requestedLeft = this.Left;
			requestedWidth = base.Width;
			requestedHeight = base.Height;
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x0019CCF0 File Offset: 0x0019BCF0
		internal virtual void SetupInitialState(double requestedTop, double requestedLeft, double requestedWidth, double requestedHeight)
		{
			this.HwndSourceSizeToContent = (SizeToContent)base.GetValue(Window.SizeToContentProperty);
			this.UpdateIcon();
			NativeMethods.RECT windowBounds = this.WindowBounds;
			Size currentSizeDeviceUnits = new Size((double)(windowBounds.right - windowBounds.left), (double)(windowBounds.bottom - windowBounds.top));
			double num = (double)windowBounds.left;
			double num2 = (double)windowBounds.top;
			bool flag = false;
			Point point = this.DeviceToLogicalUnits(new Point(num, num2));
			this._actualLeft = point.X;
			this._actualTop = point.Y;
			try
			{
				this._updateHwndLocation = false;
				base.CoerceValue(Window.TopProperty);
				base.CoerceValue(Window.LeftProperty);
			}
			finally
			{
				this._updateHwndLocation = true;
			}
			Point point2 = this.LogicalToDeviceUnits(new Point(requestedWidth, requestedHeight));
			Point point3 = this.LogicalToDeviceUnits(new Point(requestedLeft, requestedTop));
			if (!DoubleUtil.IsNaN(requestedWidth) && !DoubleUtil.AreClose(currentSizeDeviceUnits.Width, point2.X))
			{
				flag = true;
				currentSizeDeviceUnits.Width = point2.X;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedWidth, Window.BoundsSpecified.Width);
				}
			}
			if (!DoubleUtil.IsNaN(requestedHeight) && !DoubleUtil.AreClose(currentSizeDeviceUnits.Height, point2.Y))
			{
				flag = true;
				currentSizeDeviceUnits.Height = point2.Y;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedHeight, Window.BoundsSpecified.Height);
				}
			}
			if (!DoubleUtil.IsNaN(requestedLeft) && !DoubleUtil.AreClose(num, point3.X))
			{
				flag = true;
				num = point3.X;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedLeft, Window.BoundsSpecified.Left);
				}
			}
			if (!DoubleUtil.IsNaN(requestedTop) && !DoubleUtil.AreClose(num2, point3.Y))
			{
				flag = true;
				num2 = point3.Y;
				if (this.WindowState != WindowState.Normal)
				{
					this.UpdateHwndRestoreBounds(requestedTop, Window.BoundsSpecified.Top);
				}
			}
			Point point4 = this.LogicalToDeviceUnits(new Point(base.MinWidth, base.MinHeight));
			Point point5 = this.LogicalToDeviceUnits(new Point(base.MaxWidth, base.MaxHeight));
			if (!double.IsPositiveInfinity(point5.X) && currentSizeDeviceUnits.Width > point5.X)
			{
				flag = true;
				currentSizeDeviceUnits.Width = point5.X;
			}
			if (!double.IsPositiveInfinity(point4.Y) && currentSizeDeviceUnits.Height > point5.Y)
			{
				flag = true;
				currentSizeDeviceUnits.Height = point5.Y;
			}
			if (currentSizeDeviceUnits.Width < point4.X)
			{
				flag = true;
				currentSizeDeviceUnits.Width = point4.X;
			}
			if (currentSizeDeviceUnits.Height < point4.Y)
			{
				flag = true;
				currentSizeDeviceUnits.Height = point4.Y;
			}
			flag = (this.CalculateWindowLocation(ref num, ref num2, currentSizeDeviceUnits) || flag);
			if (flag && this.WindowState == WindowState.Normal)
			{
				UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), DoubleUtil.DoubleToInt(num), DoubleUtil.DoubleToInt(num2), DoubleUtil.DoubleToInt(currentSizeDeviceUnits.Width), DoubleUtil.DoubleToInt(currentSizeDeviceUnits.Height), 20);
				try
				{
					this._updateHwndLocation = false;
					this._updateStartupLocation = true;
					base.CoerceValue(Window.TopProperty);
					base.CoerceValue(Window.LeftProperty);
				}
				finally
				{
					this._updateHwndLocation = true;
					this._updateStartupLocation = false;
				}
			}
			if (!this.HwndCreatedButNotShown)
			{
				this.SetRootVisualAndUpdateSTC();
			}
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x0019D040 File Offset: 0x0019C040
		internal void SetRootVisual()
		{
			this.SetIWindowService();
			if (!this.IsSourceWindowNull)
			{
				this._swh.RootVisual = this;
			}
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x0019D05C File Offset: 0x0019C05C
		internal void SetRootVisualAndUpdateSTC()
		{
			this.SetRootVisual();
			if (!this.IsSourceWindowNull && (this.SizeToContent != SizeToContent.Manual || this.HwndCreatedButNotShown))
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				double val = (double)windowBounds.left;
				double val2 = (double)windowBounds.top;
				Point point = this.LogicalToDeviceUnits(new Point(base.ActualWidth, base.ActualHeight));
				if (this.CalculateWindowLocation(ref val, ref val2, new Size(point.X, point.Y)) && this.WindowState == WindowState.Normal)
				{
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), DoubleUtil.DoubleToInt(val), DoubleUtil.DoubleToInt(val2), 0, 0, 21);
					try
					{
						this._updateHwndLocation = false;
						this._updateStartupLocation = true;
						base.CoerceValue(Window.TopProperty);
						base.CoerceValue(Window.LeftProperty);
					}
					finally
					{
						this._updateHwndLocation = true;
						this._updateStartupLocation = false;
					}
				}
			}
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x0019D154 File Offset: 0x0019C154
		private void CreateWindowStyle()
		{
			this._Style &= -12582913;
			this._StyleEx &= -641;
			switch (this.WindowStyle)
			{
			case WindowStyle.None:
				this._Style &= -12582913;
				return;
			case WindowStyle.SingleBorderWindow:
				this._Style |= 12582912;
				return;
			case WindowStyle.ThreeDBorderWindow:
				this._Style |= 12582912;
				this._StyleEx |= 512;
				return;
			case WindowStyle.ToolWindow:
				this._Style |= 12582912;
				this._StyleEx |= 128;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x0019D212 File Offset: 0x0019C212
		internal virtual void UpdateTitle(string title)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				UnsafeNativeMethods.SetWindowText(new HandleRef(this, this.CriticalHandle), title);
			}
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x0019D238 File Offset: 0x0019C238
		private void UpdateHwndSizeOnWidthHeightChange(double widthLogicalUnits, double heightLogicalUnits)
		{
			bool inTrustedSubWindow = this._inTrustedSubWindow;
			Point point = this.LogicalToDeviceUnits(new Point(widthLogicalUnits, heightLogicalUnits));
			UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), 0, 0, DoubleUtil.DoubleToInt(point.X), DoubleUtil.DoubleToInt(point.Y), 22);
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x0019D294 File Offset: 0x0019C294
		internal void HandleActivate(bool windowActivated)
		{
			if (windowActivated && !this.IsActive)
			{
				base.SetValue(Window.IsActivePropertyKey, BooleanBoxes.TrueBox);
				this.OnActivated(EventArgs.Empty);
				return;
			}
			if (!windowActivated && this.IsActive)
			{
				base.SetValue(Window.IsActivePropertyKey, BooleanBoxes.FalseBox);
				this.OnDeactivated(EventArgs.Empty);
			}
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x0019D2F0 File Offset: 0x0019C2F0
		internal virtual void UpdateHeight(double newHeight)
		{
			if (this.WindowState == WindowState.Normal)
			{
				this.UpdateHwndSizeOnWidthHeightChange(this.DeviceToLogicalUnits(new Point((double)this.WindowBounds.Width, 0.0)).X, newHeight);
				return;
			}
			this.UpdateHwndRestoreBounds(newHeight, Window.BoundsSpecified.Height);
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x0019D340 File Offset: 0x0019C340
		internal virtual void UpdateWidth(double newWidth)
		{
			if (this.WindowState == WindowState.Normal)
			{
				this.UpdateHwndSizeOnWidthHeightChange(newWidth, this.DeviceToLogicalUnits(new Point(0.0, (double)this.WindowBounds.Height)).Y);
				return;
			}
			this.UpdateHwndRestoreBounds(newWidth, Window.BoundsSpecified.Width);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void VerifyApiSupported()
		{
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x0019D390 File Offset: 0x0019C390
		internal bool HwndCreatedButNotShown
		{
			get
			{
				return this._hwndCreatedButNotShown;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06002A49 RID: 10825 RVA: 0x0019D398 File Offset: 0x0019C398
		internal bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x0019D3A0 File Offset: 0x0019C3A0
		internal bool IsVisibilitySet
		{
			get
			{
				this.VerifyContextAndObjectState();
				return this._isVisibilitySet;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06002A4B RID: 10827 RVA: 0x0019D3AE File Offset: 0x0019C3AE
		internal IntPtr CriticalHandle
		{
			get
			{
				this.VerifyContextAndObjectState();
				if (this._swh != null)
				{
					return this._swh.CriticalHandle;
				}
				return IntPtr.Zero;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x0019D3CF File Offset: 0x0019C3CF
		// (set) Token: 0x06002A4D RID: 10829 RVA: 0x0019D3DD File Offset: 0x0019C3DD
		internal IntPtr OwnerHandle
		{
			get
			{
				this.VerifyContextAndObjectState();
				return this._ownerHandle;
			}
			set
			{
				this.VerifyContextAndObjectState();
				if (this._showingAsDialog)
				{
					throw new InvalidOperationException(SR.Get("CantSetOwnerAfterDialogIsShown"));
				}
				this.SetOwnerHandle(value);
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06002A4E RID: 10830 RVA: 0x0019D404 File Offset: 0x0019C404
		// (set) Token: 0x06002A4F RID: 10831 RVA: 0x0019D412 File Offset: 0x0019C412
		internal int Win32Style
		{
			get
			{
				this.VerifyContextAndObjectState();
				return this._Style;
			}
			set
			{
				this.VerifyContextAndObjectState();
				this._Style = value;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06002A50 RID: 10832 RVA: 0x0019D421 File Offset: 0x0019C421
		// (set) Token: 0x06002A51 RID: 10833 RVA: 0x0019D456 File Offset: 0x0019C456
		internal int _Style
		{
			get
			{
				if (this.Manager != null)
				{
					return this._styleDoNotUse.Value;
				}
				if (this.IsSourceWindowNull)
				{
					return this._styleDoNotUse.Value;
				}
				return this._swh.StyleFromHwnd;
			}
			set
			{
				this._styleDoNotUse = new SecurityCriticalDataForSet<int>(value);
				this.Manager.Dirty = true;
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x0019D470 File Offset: 0x0019C470
		// (set) Token: 0x06002A53 RID: 10835 RVA: 0x0019D4A5 File Offset: 0x0019C4A5
		internal int _StyleEx
		{
			get
			{
				if (this.Manager != null)
				{
					return this._styleExDoNotUse.Value;
				}
				if (this.IsSourceWindowNull)
				{
					return this._styleExDoNotUse.Value;
				}
				return this._swh.StyleExFromHwnd;
			}
			set
			{
				this._styleExDoNotUse = new SecurityCriticalDataForSet<int>(value);
				this.Manager.Dirty = true;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x0019D4BF File Offset: 0x0019C4BF
		// (set) Token: 0x06002A55 RID: 10837 RVA: 0x0019D4C7 File Offset: 0x0019C4C7
		internal Window.HwndStyleManager Manager
		{
			get
			{
				return this._manager;
			}
			set
			{
				this._manager = value;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWindowService.UserResized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x0019D4D0 File Offset: 0x0019C4D0
		private Size MeasureOverrideHelper(Size constraint)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return new Size(0.0, 0.0);
			}
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = this.GetVisualChild(0) as UIElement;
				if (uielement != null)
				{
					Size hwndNonClientAreaSizeInMeasureUnits = this.GetHwndNonClientAreaSizeInMeasureUnits();
					uielement.Measure(new Size
					{
						Width = ((constraint.Width == double.PositiveInfinity) ? double.PositiveInfinity : Math.Max(0.0, constraint.Width - hwndNonClientAreaSizeInMeasureUnits.Width)),
						Height = ((constraint.Height == double.PositiveInfinity) ? double.PositiveInfinity : Math.Max(0.0, constraint.Height - hwndNonClientAreaSizeInMeasureUnits.Height))
					});
					Size desiredSize = uielement.DesiredSize;
					return new Size(desiredSize.Width + hwndNonClientAreaSizeInMeasureUnits.Width, desiredSize.Height + hwndNonClientAreaSizeInMeasureUnits.Height);
				}
			}
			return this._swh.GetSizeFromHwndInMeasureUnits();
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x0019D5F4 File Offset: 0x0019C5F4
		internal virtual Window.WindowMinMax GetWindowMinMax()
		{
			Window.WindowMinMax result = default(Window.WindowMinMax);
			Invariant.Assert(!this.IsCompositionTargetInvalid, "IsCompositionTargetInvalid is supposed to be false here");
			double x = this._trackMaxWidthDeviceUnits;
			double y = this._trackMaxHeightDeviceUnits;
			if (this.WindowState == WindowState.Maximized)
			{
				x = Math.Max(this._trackMaxWidthDeviceUnits, this._windowMaxWidthDeviceUnits);
				y = Math.Max(this._trackMaxHeightDeviceUnits, this._windowMaxHeightDeviceUnits);
			}
			Point point = this.DeviceToLogicalUnits(new Point(x, y));
			Point point2 = this.DeviceToLogicalUnits(new Point(this._trackMinWidthDeviceUnits, this._trackMinHeightDeviceUnits));
			result.minWidth = Math.Max(base.MinWidth, point2.X);
			if (base.MinWidth > base.MaxWidth)
			{
				result.maxWidth = Math.Min(base.MinWidth, point.X);
			}
			else if (!double.IsPositiveInfinity(base.MaxWidth))
			{
				result.maxWidth = Math.Min(base.MaxWidth, point.X);
			}
			else
			{
				result.maxWidth = point.X;
			}
			result.minHeight = Math.Max(base.MinHeight, point2.Y);
			if (base.MinHeight > base.MaxHeight)
			{
				result.maxHeight = Math.Min(base.MinHeight, point.Y);
			}
			else if (!double.IsPositiveInfinity(base.MaxHeight))
			{
				result.maxHeight = Math.Min(base.MaxHeight, point.Y);
			}
			else
			{
				result.maxHeight = point.Y;
			}
			return result;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x0019D76F File Offset: 0x0019C76F
		private void LoadedHandler(object sender, RoutedEventArgs e)
		{
			if (this._postContentRenderedFromLoadedHandler)
			{
				this.PostContentRendered();
				this._postContentRenderedFromLoadedHandler = false;
				base.Loaded -= this.LoadedHandler;
			}
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x0019D798 File Offset: 0x0019C798
		private void PostContentRendered()
		{
			if (this._contentRenderedCallback != null)
			{
				this._contentRenderedCallback.Abort();
			}
			this._contentRenderedCallback = base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object unused)
			{
				this._contentRenderedCallback = null;
				this.OnContentRendered(EventArgs.Empty);
				return null;
			}), this);
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x0019D7D0 File Offset: 0x0019C7D0
		private void EnsureDialogCommand()
		{
			if (!Window._dialogCommandAdded)
			{
				CommandBinding commandBinding = new CommandBinding(Window.DialogCancelCommand);
				commandBinding.Executed += Window.OnDialogCommand;
				CommandManager.RegisterClassCommandBinding(typeof(Window), commandBinding);
				Window._dialogCommandAdded = true;
			}
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x0019D817 File Offset: 0x0019C817
		private static void OnDialogCommand(object target, ExecutedRoutedEventArgs e)
		{
			(target as Window).OnDialogCancelCommand();
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x0019D824 File Offset: 0x0019C824
		private void OnDialogCancelCommand()
		{
			if (this._showingAsDialog)
			{
				this.DialogResult = new bool?(false);
			}
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x0019D83A File Offset: 0x0019C83A
		private bool ThreadWindowsCallback(IntPtr hWnd, IntPtr lparam)
		{
			if (SafeNativeMethods.IsWindowVisible(new HandleRef(null, hWnd)) && SafeNativeMethods.IsWindowEnabled(new HandleRef(null, hWnd)))
			{
				this._threadWindowHandles.Add(hWnd);
			}
			return true;
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x0019D86C File Offset: 0x0019C86C
		private void EnableThreadWindows(bool state)
		{
			for (int i = 0; i < this._threadWindowHandles.Count; i++)
			{
				IntPtr handle = (IntPtr)this._threadWindowHandles[i];
				if (UnsafeNativeMethods.IsWindow(new HandleRef(null, handle)))
				{
					UnsafeNativeMethods.EnableWindowNoThrow(new HandleRef(null, handle), state);
				}
			}
			if (state)
			{
				this._threadWindowHandles = null;
			}
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x0019D8C8 File Offset: 0x0019C8C8
		private void Initialize()
		{
			base.BypassLayoutPolicies = true;
			if (this.IsInsideApp)
			{
				if (Application.Current.Dispatcher.Thread == Dispatcher.CurrentDispatcher.Thread)
				{
					this.App.WindowsInternal.Add(this);
					if (this.App.MainWindow == null)
					{
						this.App.MainWindow = this;
						return;
					}
				}
				else
				{
					this.App.NonAppWindowsInternal.Add(this);
				}
			}
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x0019D93D File Offset: 0x0019C93D
		internal void VerifyContextAndObjectState()
		{
			base.VerifyAccess();
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x0019D945 File Offset: 0x0019C945
		private void VerifyCanShow()
		{
			if (this._disposed)
			{
				throw new InvalidOperationException(SR.Get("ReshowNotAllowed"));
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x0019D95F File Offset: 0x0019C95F
		private void VerifyNotClosing()
		{
			if (this._isClosing)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperationDuringClosing"));
			}
			if (!this.IsSourceWindowNull && this.IsCompositionTargetInvalid)
			{
				throw new InvalidOperationException(SR.Get("InvalidCompositionTarget"));
			}
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x0019D999 File Offset: 0x0019C999
		private void VerifyHwndCreateShowState()
		{
			if (this.HwndCreatedButNotShown)
			{
				throw new InvalidOperationException(SR.Get("NotAllowedBeforeShow"));
			}
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x0019D9B3 File Offset: 0x0019C9B3
		private void SetIWindowService()
		{
			if (base.GetValue(Window.IWindowServiceProperty) == null)
			{
				base.SetValue(Window.IWindowServiceProperty, this);
			}
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x0019D9D0 File Offset: 0x0019C9D0
		private IntPtr GetCurrentMonitorFromMousePosition()
		{
			NativeMethods.POINT point = new NativeMethods.POINT();
			UnsafeNativeMethods.TryGetCursorPos(point);
			return SafeNativeMethods.MonitorFromPoint(new NativeMethods.POINTSTRUCT(point.x, point.y), 2);
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x0019DA04 File Offset: 0x0019CA04
		private bool CalculateWindowLocation(ref double leftDeviceUnits, ref double topDeviceUnits, Size currentSizeDeviceUnits)
		{
			double value = leftDeviceUnits;
			double value2 = topDeviceUnits;
			switch (this._windowStartupLocation)
			{
			case WindowStartupLocation.Manual:
				goto IL_259;
			case WindowStartupLocation.CenterScreen:
				break;
			case WindowStartupLocation.CenterOwner:
			{
				Rect rect = Rect.Empty;
				if (this.CanCenterOverWPFOwner)
				{
					if (this.Owner.WindowState == WindowState.Maximized || this.Owner.WindowState == WindowState.Minimized)
					{
						break;
					}
					Point point;
					if (this.Owner.CriticalHandle == IntPtr.Zero)
					{
						point = this.Owner.LogicalToDeviceUnits(new Point(this.Owner.Width, this.Owner.Height));
					}
					else
					{
						Size windowSize = this.Owner.WindowSize;
						point = new Point(windowSize.Width, windowSize.Height);
					}
					Point point2 = this.Owner.LogicalToDeviceUnits(new Point(this.Owner.Left, this.Owner.Top));
					rect = new Rect(point2.X, point2.Y, point.X, point.Y);
				}
				else if (this._ownerHandle != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(null, this._ownerHandle)))
				{
					rect = this.GetNormalRectDeviceUnits(this._ownerHandle);
				}
				if (!rect.IsEmpty)
				{
					leftDeviceUnits = rect.X + (rect.Width - currentSizeDeviceUnits.Width) / 2.0;
					topDeviceUnits = rect.Y + (rect.Height - currentSizeDeviceUnits.Height) / 2.0;
					NativeMethods.RECT rect2 = Window.WorkAreaBoundsForHwnd(this._ownerHandle);
					leftDeviceUnits = Math.Min(leftDeviceUnits, (double)rect2.right - currentSizeDeviceUnits.Width);
					leftDeviceUnits = Math.Max(leftDeviceUnits, (double)rect2.left);
					topDeviceUnits = Math.Min(topDeviceUnits, (double)rect2.bottom - currentSizeDeviceUnits.Height);
					topDeviceUnits = Math.Max(topDeviceUnits, (double)rect2.top);
					goto IL_259;
				}
				goto IL_259;
			}
			default:
				goto IL_259;
			}
			IntPtr intPtr = IntPtr.Zero;
			if (this._ownerHandle == IntPtr.Zero || (this._hiddenWindow != null && this._hiddenWindow.Handle == this._ownerHandle))
			{
				intPtr = this.GetCurrentMonitorFromMousePosition();
			}
			else
			{
				intPtr = Window.MonitorFromWindow(this._ownerHandle);
			}
			if (intPtr != IntPtr.Zero)
			{
				Window.CalculateCenterScreenPosition(intPtr, currentSizeDeviceUnits, ref leftDeviceUnits, ref topDeviceUnits);
			}
			IL_259:
			return !DoubleUtil.AreClose(value, leftDeviceUnits) || !DoubleUtil.AreClose(value2, topDeviceUnits);
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x0019DC81 File Offset: 0x0019CC81
		private static NativeMethods.RECT WorkAreaBoundsForHwnd(IntPtr hwnd)
		{
			return Window.WorkAreaBoundsForMointor(Window.MonitorFromWindow(hwnd));
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x0019DC90 File Offset: 0x0019CC90
		private static NativeMethods.RECT WorkAreaBoundsForMointor(IntPtr hMonitor)
		{
			NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
			SafeNativeMethods.GetMonitorInfo(new HandleRef(null, hMonitor), monitorinfoex);
			return monitorinfoex.rcWork;
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x0019DCB6 File Offset: 0x0019CCB6
		private static IntPtr MonitorFromWindow(IntPtr hwnd)
		{
			IntPtr intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(null, hwnd), 2);
			if (intPtr == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			return intPtr;
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x0019DCD8 File Offset: 0x0019CCD8
		internal static void CalculateCenterScreenPosition(IntPtr hMonitor, Size currentSizeDeviceUnits, ref double leftDeviceUnits, ref double topDeviceUnits)
		{
			NativeMethods.RECT rect = Window.WorkAreaBoundsForMointor(hMonitor);
			double num = (double)(rect.right - rect.left);
			double num2 = (double)(rect.bottom - rect.top);
			leftDeviceUnits = (double)rect.left + (num - currentSizeDeviceUnits.Width) / 2.0;
			topDeviceUnits = (double)rect.top + (num2 - currentSizeDeviceUnits.Height) / 2.0;
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06002A6C RID: 10860 RVA: 0x0019DD44 File Offset: 0x0019CD44
		private bool CanCenterOverWPFOwner
		{
			get
			{
				return this.Owner != null && (!this.Owner.IsSourceWindowNull || (!DoubleUtil.IsNaN(this.Owner.Width) && !DoubleUtil.IsNaN(this.Owner.Height))) && !DoubleUtil.IsNaN(this.Owner.Left) && !DoubleUtil.IsNaN(this.Owner.Top);
			}
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x0019DDB8 File Offset: 0x0019CDB8
		private Rect GetNormalRectDeviceUnits(IntPtr hwndHandle)
		{
			bool windowLong = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, hwndHandle), -20) != 0;
			NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
			windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
			UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, hwndHandle), ref windowplacement);
			Point pt = new Point((double)windowplacement.rcNormalPosition_left, (double)windowplacement.rcNormalPosition_top);
			if (((windowLong ? 1 : 0) & 128) == 0)
			{
				pt = this.TransformWorkAreaScreenArea(pt, Window.TransformType.WorkAreaToScreenArea);
			}
			Point point = new Point((double)(windowplacement.rcNormalPosition_right - windowplacement.rcNormalPosition_left), (double)(windowplacement.rcNormalPosition_bottom - windowplacement.rcNormalPosition_top));
			return new Rect(pt.X, pt.Y, point.X, point.Y);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x0019DE6C File Offset: 0x0019CE6C
		private Rect GetNormalRectLogicalUnits(IntPtr hwndHandle)
		{
			Rect normalRectDeviceUnits = this.GetNormalRectDeviceUnits(hwndHandle);
			Point point = this.DeviceToLogicalUnits(new Point(normalRectDeviceUnits.Width, normalRectDeviceUnits.Height));
			Point point2 = this.DeviceToLogicalUnits(new Point(normalRectDeviceUnits.X, normalRectDeviceUnits.Y));
			return new Rect(point2.X, point2.Y, point.X, point.Y);
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x0019DED8 File Offset: 0x0019CED8
		private void CreateWindowState()
		{
			switch (this.WindowState)
			{
			case WindowState.Normal:
				break;
			case WindowState.Minimized:
				this._Style |= 536870912;
				break;
			case WindowState.Maximized:
				this._Style |= 16777216;
				return;
			default:
				return;
			}
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x0019DF24 File Offset: 0x0019CF24
		private void CreateTopmost()
		{
			if (this.Topmost)
			{
				this._StyleEx |= 8;
				return;
			}
			this._StyleEx &= -9;
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x0019DF4C File Offset: 0x0019CF4C
		private void CreateResizibility()
		{
			this._Style &= -458753;
			switch (this.ResizeMode)
			{
			case ResizeMode.NoResize:
				break;
			case ResizeMode.CanMinimize:
				this._Style |= 131072;
				return;
			case ResizeMode.CanResize:
			case ResizeMode.CanResizeWithGrip:
				this._Style |= 458752;
				break;
			default:
				return;
			}
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x0019DFB0 File Offset: 0x0019CFB0
		private void UpdateIcon()
		{
			NativeMethods.IconHandle defaultLargeIconHandle;
			NativeMethods.IconHandle defaultSmallIconHandle;
			if (this._icon != null)
			{
				IconHelper.GetIconHandlesFromImageSource(this._icon, out defaultLargeIconHandle, out defaultSmallIconHandle);
			}
			else if (this._defaultLargeIconHandle == null && this._defaultSmallIconHandle == null)
			{
				IconHelper.GetDefaultIconHandles(out defaultLargeIconHandle, out defaultSmallIconHandle);
				this._defaultLargeIconHandle = defaultLargeIconHandle;
				this._defaultSmallIconHandle = defaultSmallIconHandle;
			}
			else
			{
				defaultLargeIconHandle = this._defaultLargeIconHandle;
				defaultSmallIconHandle = this._defaultSmallIconHandle;
			}
			HandleRef[] array = new HandleRef[2];
			array[0] = new HandleRef(this, this.CriticalHandle);
			HandleRef[] array2 = array;
			int num = 1;
			if (this._hiddenWindow != null)
			{
				array2[1] = new HandleRef(this._hiddenWindow, this._hiddenWindow.Handle);
				num++;
			}
			for (int i = 0; i < num; i++)
			{
				HandleRef hWnd = array2[i];
				UnsafeNativeMethods.SendMessage(hWnd, WindowMessage.WM_SETICON, (IntPtr)1, defaultLargeIconHandle);
				UnsafeNativeMethods.SendMessage(hWnd, WindowMessage.WM_SETICON, (IntPtr)0, defaultSmallIconHandle);
			}
			if (this._currentLargeIconHandle != null && this._currentLargeIconHandle != this._defaultLargeIconHandle)
			{
				this._currentLargeIconHandle.Dispose();
			}
			if (this._currentSmallIconHandle != null && this._currentSmallIconHandle != this._defaultSmallIconHandle)
			{
				this._currentSmallIconHandle.Dispose();
			}
			this._currentLargeIconHandle = defaultLargeIconHandle;
			this._currentSmallIconHandle = defaultSmallIconHandle;
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x0019E0E0 File Offset: 0x0019D0E0
		private void SetOwnerHandle(IntPtr ownerHandle)
		{
			if (this._ownerHandle == ownerHandle && this._ownerHandle == IntPtr.Zero)
			{
				return;
			}
			this._ownerHandle = ((IntPtr.Zero == ownerHandle && !this.ShowInTaskbar) ? this.EnsureHiddenWindow().Handle : ownerHandle);
			if (!this.IsSourceWindowNull)
			{
				UnsafeNativeMethods.SetWindowLong(new HandleRef(null, this.CriticalHandle), -8, this._ownerHandle);
				if (this._ownerWindow != null && this._ownerWindow.CriticalHandle != this._ownerHandle)
				{
					this._ownerWindow.OwnedWindowsInternal.Remove(this);
					this._ownerWindow = null;
				}
			}
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x0019E191 File Offset: 0x0019D191
		private void OnSourceWindowDisposed(object sender, EventArgs e)
		{
			if (!this._disposed)
			{
				this.InternalDispose();
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x0019E1A4 File Offset: 0x0019D1A4
		private IntPtr WindowFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr zero = IntPtr.Zero;
			if (msg != 5)
			{
				if (msg == 36)
				{
					handled = this.WmGetMinMaxInfo(lParam);
				}
			}
			else
			{
				handled = this.WmSizeChanged(wParam);
			}
			if (this._swh != null && this._swh.CompositionTarget != null)
			{
				if (msg == (int)Window.WM_TASKBARBUTTONCREATED || msg == (int)Window.WM_APPLYTASKBARITEMINFO)
				{
					if (this._taskbarRetryTimer != null)
					{
						this._taskbarRetryTimer.Stop();
					}
					this.ApplyTaskbarItemInfo();
				}
				else
				{
					if (msg <= 16)
					{
						switch (msg)
						{
						case 2:
							handled = this.WmDestroy();
							return zero;
						case 3:
							handled = this.WmMoveChanged();
							return zero;
						case 4:
						case 5:
							break;
						case 6:
							handled = this.WmActivate(wParam);
							return zero;
						default:
							if (msg == 16)
							{
								handled = this.WmClose();
								return zero;
							}
							break;
						}
					}
					else
					{
						if (msg == 24)
						{
							handled = this.WmShowWindow(wParam, lParam);
							return zero;
						}
						if (msg == 132)
						{
							handled = this.WmNcHitTest(lParam, ref zero);
							return zero;
						}
						if (msg == 273)
						{
							handled = this.WmCommand(wParam, lParam);
							return zero;
						}
					}
					handled = false;
				}
			}
			return zero;
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x0019E2C0 File Offset: 0x0019D2C0
		private bool WmCommand(IntPtr wParam, IntPtr lParam)
		{
			if (NativeMethods.SignedHIWORD(wParam.ToInt32()) == 6144)
			{
				TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
				if (taskbarItemInfo != null)
				{
					int num = NativeMethods.SignedLOWORD(wParam.ToInt32());
					if (num >= 0 && num < taskbarItemInfo.ThumbButtonInfos.Count)
					{
						taskbarItemInfo.ThumbButtonInfos[num].InvokeClick();
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x0019E320 File Offset: 0x0019D320
		private bool WmClose()
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			this._isClosing = true;
			CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
			try
			{
				this.OnClosing(cancelEventArgs);
			}
			catch
			{
				this.CloseWindowFromWmClose();
				throw;
			}
			if (this.ShouldCloseWindow(cancelEventArgs.Cancel))
			{
				this.CloseWindowFromWmClose();
				return false;
			}
			this._isClosing = false;
			this._dialogResult = null;
			return true;
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x0019E39C File Offset: 0x0019D39C
		private void CloseWindowFromWmClose()
		{
			if (this._showingAsDialog)
			{
				this.DoDialogHide();
			}
			this.ClearRootVisual();
			this.ClearHiddenWindowIfAny();
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x0019E3B8 File Offset: 0x0019D3B8
		private bool ShouldCloseWindow(bool cancelled)
		{
			return !cancelled || this._appShuttingDown || this._ignoreCancel;
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x0019E3D0 File Offset: 0x0019D3D0
		private void DoDialogHide()
		{
			if (this._dispatcherFrame != null)
			{
				this._dispatcherFrame.Continue = false;
				this._dispatcherFrame = null;
			}
			if (this._dialogResult == null)
			{
				this._dialogResult = new bool?(false);
			}
			this._showingAsDialog = false;
			bool isActiveWindow = this._swh.IsActiveWindow;
			this.EnableThreadWindows(true);
			if (isActiveWindow && this._dialogPreviousActiveHandle != IntPtr.Zero && UnsafeNativeMethods.IsWindow(new HandleRef(this, this._dialogPreviousActiveHandle)))
			{
				UnsafeNativeMethods.SetActiveWindow(new HandleRef(this, this._dialogPreviousActiveHandle));
			}
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x0019E464 File Offset: 0x0019D464
		private void UpdateWindowListsOnClose()
		{
			WindowCollection ownedWindowsInternal = this.OwnedWindowsInternal;
			while (ownedWindowsInternal.Count > 0)
			{
				ownedWindowsInternal[0].InternalClose(false, true);
			}
			if (!this.IsOwnerNull)
			{
				this.Owner.OwnedWindowsInternal.Remove(this);
			}
			if (this.IsInsideApp)
			{
				if (Application.Current.Dispatcher.Thread == Dispatcher.CurrentDispatcher.Thread)
				{
					this.App.WindowsInternal.Remove(this);
					if (!this._appShuttingDown && ((this.App.Windows.Count == 0 && this.App.ShutdownMode == ShutdownMode.OnLastWindowClose) || (this.App.MainWindow == this && this.App.ShutdownMode == ShutdownMode.OnMainWindowClose)))
					{
						this.App.CriticalShutdown(0);
					}
					this.TryClearingMainWindow();
					return;
				}
				this.App.NonAppWindowsInternal.Remove(this);
			}
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x0019E548 File Offset: 0x0019D548
		private bool WmDestroy()
		{
			if (this.IsSourceWindowNull)
			{
				return false;
			}
			if (!this._disposed)
			{
				this.InternalDispose();
			}
			this.OnClosed(EventArgs.Empty);
			return false;
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x0019E570 File Offset: 0x0019D570
		private bool WmActivate(IntPtr wParam)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			bool windowActivated = NativeMethods.SignedLOWORD(wParam) != 0;
			this.HandleActivate(windowActivated);
			return false;
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x0019E5A8 File Offset: 0x0019D5A8
		private void UpdateDimensionsToRestoreBounds()
		{
			Rect restoreBounds = this.RestoreBounds;
			base.SetValue(Window.LeftProperty, restoreBounds.Left);
			base.SetValue(Window.TopProperty, restoreBounds.Top);
			base.SetValue(FrameworkElement.WidthProperty, restoreBounds.Width);
			base.SetValue(FrameworkElement.HeightProperty, restoreBounds.Height);
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x0019E618 File Offset: 0x0019D618
		private bool WmSizeChanged(IntPtr wParam)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			NativeMethods.RECT windowBounds = this.WindowBounds;
			Point ptDeviceUnits = new Point((double)(windowBounds.right - windowBounds.left), (double)(windowBounds.bottom - windowBounds.top));
			Point point = this.DeviceToLogicalUnits(ptDeviceUnits);
			try
			{
				this._updateHwndSize = false;
				base.SetValue(FrameworkElement.WidthProperty, point.X);
				base.SetValue(FrameworkElement.HeightProperty, point.Y);
			}
			finally
			{
				this._updateHwndSize = true;
			}
			this.UpdateTaskbarThumbnailClipping();
			switch ((int)wParam)
			{
			case 0:
				if (this._previousWindowState != WindowState.Normal)
				{
					if (this.WindowState != WindowState.Normal)
					{
						this.WindowState = WindowState.Normal;
						this.WmMoveChangedHelper();
					}
					this._previousWindowState = WindowState.Normal;
					this.OnStateChanged(EventArgs.Empty);
				}
				break;
			case 1:
				if (this._previousWindowState != WindowState.Minimized)
				{
					if (this.WindowState != WindowState.Minimized)
					{
						try
						{
							this._updateHwndSize = false;
							this._updateHwndLocation = false;
							this.UpdateDimensionsToRestoreBounds();
						}
						finally
						{
							this._updateHwndSize = true;
							this._updateHwndLocation = true;
						}
						this.WindowState = WindowState.Minimized;
					}
					this._previousWindowState = WindowState.Minimized;
					this.OnStateChanged(EventArgs.Empty);
				}
				break;
			case 2:
				if (this._previousWindowState != WindowState.Maximized)
				{
					if (this.WindowState != WindowState.Maximized)
					{
						try
						{
							this._updateHwndLocation = false;
							this._updateHwndSize = false;
							this.UpdateDimensionsToRestoreBounds();
						}
						finally
						{
							this._updateHwndSize = true;
							this._updateHwndLocation = true;
						}
						this.WindowState = WindowState.Maximized;
					}
					this._windowMaxWidthDeviceUnits = Math.Max(this._windowMaxWidthDeviceUnits, ptDeviceUnits.X);
					this._windowMaxHeightDeviceUnits = Math.Max(this._windowMaxHeightDeviceUnits, ptDeviceUnits.Y);
					this._previousWindowState = WindowState.Maximized;
					this.OnStateChanged(EventArgs.Empty);
				}
				break;
			}
			return false;
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x0019E800 File Offset: 0x0019D800
		private bool WmMoveChanged()
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			NativeMethods.RECT windowBounds = this.WindowBounds;
			Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.left, (double)windowBounds.top));
			if (!DoubleUtil.AreClose(this._actualLeft, point.X) || !DoubleUtil.AreClose(this._actualTop, point.Y))
			{
				this._actualLeft = point.X;
				this._actualTop = point.Y;
				this.WmMoveChangedHelper();
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this);
				if (automationPeer != null)
				{
					automationPeer.InvalidatePeer();
				}
			}
			return false;
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x0019E89C File Offset: 0x0019D89C
		internal virtual void WmMoveChangedHelper()
		{
			if (this.WindowState == WindowState.Normal)
			{
				try
				{
					this._updateHwndLocation = false;
					base.SetValue(Window.LeftProperty, this._actualLeft);
					base.SetValue(Window.TopProperty, this._actualTop);
				}
				finally
				{
					this._updateHwndLocation = true;
				}
				this.OnLocationChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x0019E90C File Offset: 0x0019D90C
		private bool WmGetMinMaxInfo(IntPtr lParam)
		{
			NativeMethods.MINMAXINFO minmaxinfo = (NativeMethods.MINMAXINFO)UnsafeNativeMethods.PtrToStructure(lParam, typeof(NativeMethods.MINMAXINFO));
			this._trackMinWidthDeviceUnits = (double)minmaxinfo.ptMinTrackSize.x;
			this._trackMinHeightDeviceUnits = (double)minmaxinfo.ptMinTrackSize.y;
			this._trackMaxWidthDeviceUnits = (double)minmaxinfo.ptMaxTrackSize.x;
			this._trackMaxHeightDeviceUnits = (double)minmaxinfo.ptMaxTrackSize.y;
			this._windowMaxWidthDeviceUnits = (double)minmaxinfo.ptMaxSize.x;
			this._windowMaxHeightDeviceUnits = (double)minmaxinfo.ptMaxSize.y;
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				Window.WindowMinMax windowMinMax = this.GetWindowMinMax();
				Point point = this.LogicalToDeviceUnits(new Point(windowMinMax.minWidth, windowMinMax.minHeight));
				Point point2 = this.LogicalToDeviceUnits(new Point(windowMinMax.maxWidth, windowMinMax.maxHeight));
				minmaxinfo.ptMinTrackSize.x = DoubleUtil.DoubleToInt(point.X);
				minmaxinfo.ptMinTrackSize.y = DoubleUtil.DoubleToInt(point.Y);
				minmaxinfo.ptMaxTrackSize.x = DoubleUtil.DoubleToInt(point2.X);
				minmaxinfo.ptMaxTrackSize.y = DoubleUtil.DoubleToInt(point2.Y);
				Marshal.StructureToPtr<NativeMethods.MINMAXINFO>(minmaxinfo, lParam, true);
			}
			return true;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x0019EA4D File Offset: 0x0019DA4D
		private bool WmNcHitTest(IntPtr lParam, ref IntPtr refInt)
		{
			return !this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && this.HandleWmNcHitTestMsg(lParam, ref refInt);
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x0019EA6C File Offset: 0x0019DA6C
		internal virtual bool HandleWmNcHitTestMsg(IntPtr lParam, ref IntPtr refInt)
		{
			if (this._resizeGripControl == null || this.ResizeMode != ResizeMode.CanResizeWithGrip)
			{
				return false;
			}
			int x = NativeMethods.SignedLOWORD(lParam);
			int y = NativeMethods.SignedHIWORD(lParam);
			NativeMethods.POINT pointRelativeToWindow = this.GetPointRelativeToWindow(x, y);
			Point point = this.DeviceToLogicalUnits(new Point((double)pointRelativeToWindow.x, (double)pointRelativeToWindow.y));
			GeneralTransform generalTransform = base.TransformToDescendant(this._resizeGripControl);
			Point point2 = point;
			if (generalTransform == null || !generalTransform.TryTransform(point, out point2))
			{
				return false;
			}
			if (point2.X < 0.0 || point2.Y < 0.0 || point2.X > this._resizeGripControl.RenderSize.Width || point2.Y > this._resizeGripControl.RenderSize.Height)
			{
				return false;
			}
			if (base.FlowDirection == FlowDirection.RightToLeft)
			{
				refInt = new IntPtr(16);
			}
			else
			{
				refInt = new IntPtr(17);
			}
			return true;
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x0019EB60 File Offset: 0x0019DB60
		private bool WmShowWindow(IntPtr wParam, IntPtr lParam)
		{
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return false;
			}
			int num = NativeMethods.IntPtrToInt32(lParam);
			if (num != 1)
			{
				if (num == 3)
				{
					this._isVisible = true;
					this.UpdateVisibilityProperty(Visibility.Visible);
				}
			}
			else
			{
				this._isVisible = false;
				this.UpdateVisibilityProperty(Visibility.Hidden);
			}
			return false;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x0019EBAF File Offset: 0x0019DBAF
		private static void _OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnIconChanged(e.NewValue as ImageSource);
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x0019EBC8 File Offset: 0x0019DBC8
		private void OnIconChanged(ImageSource newIcon)
		{
			this._icon = newIcon;
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				this.UpdateIcon();
			}
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x0019EBE7 File Offset: 0x0019DBE7
		private static void _OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnTitleChanged();
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x0019EBF4 File Offset: 0x0019DBF4
		private static bool _ValidateText(object value)
		{
			return value != null;
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x0019EBFA File Offset: 0x0019DBFA
		private void OnTitleChanged()
		{
			this.UpdateTitle(this.Title);
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x0019EC08 File Offset: 0x0019DC08
		private static void _OnShowInTaskbarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnShowInTaskbarChanged();
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x0019EC18 File Offset: 0x0019DC18
		private void OnShowInTaskbarChanged()
		{
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				bool flag = false;
				if (this._isVisible)
				{
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 1175);
					flag = true;
				}
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.SetTaskbarStatus();
				}
				if (flag)
				{
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 1111);
				}
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x0019ECC0 File Offset: 0x0019DCC0
		private static bool _ValidateWindowStateCallback(object value)
		{
			return Window.IsValidWindowState((WindowState)value);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x0019ECCD File Offset: 0x0019DCCD
		private static void _OnWindowStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnWindowStateChanged((WindowState)e.NewValue);
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x0019ECE8 File Offset: 0x0019DCE8
		private void OnWindowStateChanged(WindowState windowState)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				if (this._isVisible)
				{
					HandleRef hWnd = new HandleRef(this, this.CriticalHandle);
					int style = this._Style;
					switch (windowState)
					{
					case WindowState.Normal:
						if ((style & 16777216) == 16777216)
						{
							if (this.ShowActivated || this.IsActive)
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 9);
							}
							else
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 4);
							}
						}
						else if ((style & 536870912) == 536870912)
						{
							NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
							windowplacement.length = Marshal.SizeOf<NativeMethods.WINDOWPLACEMENT>(windowplacement);
							UnsafeNativeMethods.GetWindowPlacement(hWnd, ref windowplacement);
							if ((windowplacement.flags & 2) == 2)
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 9);
							}
							else if (this.ShowActivated)
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 9);
							}
							else
							{
								UnsafeNativeMethods.ShowWindow(hWnd, 4);
							}
						}
						break;
					case WindowState.Minimized:
						if ((style & 536870912) != 536870912)
						{
							UnsafeNativeMethods.ShowWindow(hWnd, 6);
						}
						break;
					case WindowState.Maximized:
						if ((style & 16777216) != 16777216)
						{
							UnsafeNativeMethods.ShowWindow(hWnd, 3);
						}
						break;
					}
				}
			}
			else
			{
				this._previousWindowState = windowState;
			}
			try
			{
				this._updateHwndLocation = false;
				base.CoerceValue(Window.TopProperty);
				base.CoerceValue(Window.LeftProperty);
			}
			finally
			{
				this._updateHwndLocation = true;
			}
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x0019EE4C File Offset: 0x0019DE4C
		private static bool _ValidateWindowStyleCallback(object value)
		{
			return Window.IsValidWindowStyle((WindowStyle)value);
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x0019EE59 File Offset: 0x0019DE59
		private static void _OnWindowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnWindowStyleChanged((WindowStyle)e.NewValue);
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x0019EE74 File Offset: 0x0019DE74
		private void OnWindowStyleChanged(WindowStyle windowStyle)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.CreateWindowStyle();
				}
			}
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x0019EEC8 File Offset: 0x0019DEC8
		private static void _OnTopmostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnTopmostChanged((bool)e.NewValue);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x0019EEE4 File Offset: 0x0019DEE4
		private void OnTopmostChanged(bool topmost)
		{
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				HandleRef hWndInsertAfter = topmost ? NativeMethods.HWND_TOPMOST : NativeMethods.HWND_NOTOPMOST;
				UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.CriticalHandle), hWndInsertAfter, 0, 0, 0, 0, 19);
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x0019EF30 File Offset: 0x0019DF30
		private static object CoerceVisibility(DependencyObject d, object value)
		{
			Window window = (Window)d;
			if ((Visibility)value == Visibility.Visible)
			{
				window.VerifyCanShow();
				window.VerifyConsistencyWithAllowsTransparency();
				window.VerifyNotClosing();
				window.VerifyConsistencyWithShowActivated();
			}
			return value;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x0019EF68 File Offset: 0x0019DF68
		private static void _OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = (Window)d;
			window._isVisibilitySet = true;
			if (window._visibilitySetInternally)
			{
				return;
			}
			bool flag = Window.VisibilityToBool((Visibility)e.NewValue);
			window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(window.ShowHelper), flag ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x0019EFC7 File Offset: 0x0019DFC7
		private void SafeCreateWindowDuringShow()
		{
			if (this.IsSourceWindowNull)
			{
				this.CreateSourceWindowDuringShow();
				return;
			}
			if (this.HwndCreatedButNotShown)
			{
				this.SetRootVisualAndUpdateSTC();
				this._hwndCreatedButNotShown = false;
			}
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x0019EFED File Offset: 0x0019DFED
		private void SetShowKeyboardCueState()
		{
			if (KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				this._previousKeyboardCuesProperty = (bool)base.GetValue(KeyboardNavigation.ShowKeyboardCuesProperty);
				base.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, BooleanBoxes.TrueBox);
				this._resetKeyboardCuesProperty = true;
			}
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x0019F023 File Offset: 0x0019E023
		private void ClearShowKeyboardCueState()
		{
			if (this._resetKeyboardCuesProperty)
			{
				this._resetKeyboardCuesProperty = false;
				base.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, BooleanBoxes.Box(this._previousKeyboardCuesProperty));
			}
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x0019F04C File Offset: 0x0019E04C
		private void UpdateVisibilityProperty(Visibility value)
		{
			try
			{
				this._visibilitySetInternally = true;
				base.SetValue(UIElement.VisibilityProperty, value);
			}
			finally
			{
				this._visibilitySetInternally = false;
			}
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x0019F08C File Offset: 0x0019E08C
		private object ShowHelper(object booleanBox)
		{
			if (this._disposed)
			{
				return null;
			}
			bool flag = (bool)booleanBox;
			this._isClosing = false;
			if (this._isVisible == flag)
			{
				return null;
			}
			if (flag)
			{
				if (Application.IsShuttingDown)
				{
					return null;
				}
				this.SetShowKeyboardCueState();
				this.SafeCreateWindowDuringShow();
				this._isVisible = true;
			}
			else
			{
				this.ClearShowKeyboardCueState();
				if (this._showingAsDialog)
				{
					this.DoDialogHide();
				}
				this._isVisible = false;
			}
			if (!this.IsSourceWindowNull)
			{
				int num;
				if (flag)
				{
					num = this.nCmdForShow();
				}
				else
				{
					num = 0;
				}
				if ((bool)base.GetValue(Window.TopmostProperty) && FrameworkCompatibilityPreferences.GetUseSetWindowPosForTopmostWindows() && (num == 5 || num == 8))
				{
					int num2 = (num == 8) ? 16 : 0;
					UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, num2 | 2 | 1 | 512 | 64);
				}
				else
				{
					UnsafeNativeMethods.ShowWindow(new HandleRef(this, this.CriticalHandle), num);
				}
				this.SafeStyleSetter();
			}
			if (this._showingAsDialog && this._isVisible)
			{
				try
				{
					ComponentDispatcher.PushModal();
					this._dispatcherFrame = new DispatcherFrame();
					Dispatcher.PushFrame(this._dispatcherFrame);
				}
				finally
				{
					ComponentDispatcher.PopModal();
				}
			}
			return null;
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x0019F1C4 File Offset: 0x0019E1C4
		internal virtual int nCmdForShow()
		{
			WindowState windowState = this.WindowState;
			int result;
			if (windowState != WindowState.Minimized)
			{
				if (windowState == WindowState.Maximized)
				{
					result = 3;
				}
				else
				{
					result = (this.ShowActivated ? 5 : 8);
				}
			}
			else
			{
				result = (this.ShowActivated ? 2 : 7);
			}
			return result;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x0019F204 File Offset: 0x0019E204
		private void SafeStyleSetter()
		{
			using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
			{
				this._Style = (this._isVisible ? (this._Style | 268435456) : this._Style);
			}
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x0019F264 File Offset: 0x0019E264
		private static bool _ValidateSizeToContentCallback(object value)
		{
			return Window.IsValidSizeToContent((SizeToContent)value);
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x0019F271 File Offset: 0x0019E271
		private static object _SizeToContentGetValueOverride(DependencyObject d)
		{
			return (d as Window).SizeToContent;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x0019F283 File Offset: 0x0019E283
		private static void _OnSizeToContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Window).OnSizeToContentChanged((SizeToContent)e.NewValue);
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x0019F29C File Offset: 0x0019E29C
		private void OnSizeToContentChanged(SizeToContent sizeToContent)
		{
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				this.HwndSourceSizeToContent = sizeToContent;
			}
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x0019F2BC File Offset: 0x0019E2BC
		private static void ValidateLengthForHeightWidth(double l)
		{
			if (!double.IsPositiveInfinity(l) && !DoubleUtil.IsNaN(l) && (l > 2147483647.0 || l < -2147483648.0))
			{
				throw new ArgumentException(SR.Get("ValueNotBetweenInt32MinMax", new object[]
				{
					l
				}));
			}
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x0019F310 File Offset: 0x0019E310
		private static void ValidateTopLeft(double length)
		{
			if (double.IsPositiveInfinity(length) || double.IsNegativeInfinity(length))
			{
				throw new ArgumentException(SR.Get("InvalidValueForTopLeft", new object[]
				{
					length
				}));
			}
			if (length > 2147483647.0 || length < -2147483648.0)
			{
				throw new ArgumentException(SR.Get("ValueNotBetweenInt32MinMax", new object[]
				{
					length
				}));
			}
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x0019F384 File Offset: 0x0019E384
		private static void _OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndSize)
			{
				window.OnHeightChanged((double)e.NewValue);
			}
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x0019F3B2 File Offset: 0x0019E3B2
		private void OnHeightChanged(double height)
		{
			Window.ValidateLengthForHeightWidth(height);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && !DoubleUtil.IsNaN(height))
			{
				this.UpdateHeight(height);
			}
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x0019F3D9 File Offset: 0x0019E3D9
		private static void _OnMinHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Window).OnMinHeightChanged((double)e.NewValue);
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x0019F3F4 File Offset: 0x0019E3F4
		private void OnMinHeightChanged(double minHeight)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(minHeight);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (minHeight > point.Y && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(point.X, minHeight);
				}
			}
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x0019F460 File Offset: 0x0019E460
		private static void _OnMaxHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Window).OnMaxHeightChanged((double)e.NewValue);
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x0019F47C File Offset: 0x0019E47C
		private void OnMaxHeightChanged(double maxHeight)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(base.MaxHeight);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (maxHeight < point.Y && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(point.X, maxHeight);
				}
			}
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x0019F4F0 File Offset: 0x0019E4F0
		private static void _OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndSize)
			{
				window.OnWidthChanged((double)e.NewValue);
			}
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x0019F51E File Offset: 0x0019E51E
		private void OnWidthChanged(double width)
		{
			Window.ValidateLengthForHeightWidth(width);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid && !DoubleUtil.IsNaN(width))
			{
				this.UpdateWidth(width);
			}
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x0019F545 File Offset: 0x0019E545
		private static void _OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Window).OnMinWidthChanged((double)e.NewValue);
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x0019F560 File Offset: 0x0019E560
		private void OnMinWidthChanged(double minWidth)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(minWidth);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (minWidth > point.X && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(minWidth, point.Y);
				}
			}
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x0019F5CC File Offset: 0x0019E5CC
		private static void _OnMaxWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Window).OnMaxWidthChanged((double)e.NewValue);
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x0019F5E8 File Offset: 0x0019E5E8
		private void OnMaxWidthChanged(double maxWidth)
		{
			this.VerifyApiSupported();
			Window.ValidateLengthForHeightWidth(maxWidth);
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = this.DeviceToLogicalUnits(new Point((double)windowBounds.Width, (double)windowBounds.Height));
				if (maxWidth < point.X && this.WindowState == WindowState.Normal)
				{
					this.UpdateHwndSizeOnWidthHeightChange(maxWidth, point.Y);
				}
			}
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x0019F654 File Offset: 0x0019E654
		private void UpdateHwndRestoreBounds(double newValue, Window.BoundsSpecified specifiedRestoreBounds)
		{
			NativeMethods.WINDOWPLACEMENT windowplacement = default(NativeMethods.WINDOWPLACEMENT);
			windowplacement.length = Marshal.SizeOf(typeof(NativeMethods.WINDOWPLACEMENT));
			UnsafeNativeMethods.GetWindowPlacement(new HandleRef(this, this.CriticalHandle), ref windowplacement);
			double x = this.LogicalToDeviceUnits(new Point(newValue, 0.0)).X;
			switch (specifiedRestoreBounds)
			{
			case Window.BoundsSpecified.Height:
				windowplacement.rcNormalPosition_bottom = windowplacement.rcNormalPosition_top + DoubleUtil.DoubleToInt(x);
				break;
			case Window.BoundsSpecified.Width:
				windowplacement.rcNormalPosition_right = windowplacement.rcNormalPosition_left + DoubleUtil.DoubleToInt(x);
				break;
			case Window.BoundsSpecified.Top:
			{
				double num = newValue;
				if ((this.StyleExFromHwnd & 128) == 0)
				{
					num = this.TransformWorkAreaScreenArea(new Point(0.0, num), Window.TransformType.ScreenAreaToWorkArea).Y;
				}
				num = this.LogicalToDeviceUnits(new Point(0.0, num)).Y;
				int num2 = windowplacement.rcNormalPosition_bottom - windowplacement.rcNormalPosition_top;
				windowplacement.rcNormalPosition_top = DoubleUtil.DoubleToInt(num);
				windowplacement.rcNormalPosition_bottom = windowplacement.rcNormalPosition_top + num2;
				break;
			}
			case Window.BoundsSpecified.Left:
			{
				double num3 = newValue;
				if ((this.StyleExFromHwnd & 128) == 0)
				{
					num3 = this.TransformWorkAreaScreenArea(new Point(num3, 0.0), Window.TransformType.ScreenAreaToWorkArea).X;
				}
				num3 = this.LogicalToDeviceUnits(new Point(num3, 0.0)).X;
				int num4 = windowplacement.rcNormalPosition_right - windowplacement.rcNormalPosition_left;
				windowplacement.rcNormalPosition_left = DoubleUtil.DoubleToInt(num3);
				windowplacement.rcNormalPosition_right = windowplacement.rcNormalPosition_left + num4;
				break;
			}
			}
			if (!this._isVisible)
			{
				windowplacement.showCmd = 0;
			}
			UnsafeNativeMethods.SetWindowPlacement(new HandleRef(this, this.CriticalHandle), ref windowplacement);
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x0019F81C File Offset: 0x0019E81C
		private Point TransformWorkAreaScreenArea(Point pt, Window.TransformType transformType)
		{
			int num = 0;
			int num2 = 0;
			IntPtr intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(this, this.CriticalHandle), 0);
			if (intPtr != IntPtr.Zero)
			{
				NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
				monitorinfoex.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFOEX));
				SafeNativeMethods.GetMonitorInfo(new HandleRef(this, intPtr), monitorinfoex);
				NativeMethods.RECT rcWork = monitorinfoex.rcWork;
				NativeMethods.RECT rcMonitor = monitorinfoex.rcMonitor;
				num = rcWork.left - rcMonitor.left;
				num2 = rcWork.top - rcMonitor.top;
			}
			Point result;
			if (transformType == Window.TransformType.WorkAreaToScreenArea)
			{
				result = new Point(pt.X + (double)num, pt.Y + (double)num2);
			}
			else
			{
				result = new Point(pt.X - (double)num, pt.Y - (double)num2);
			}
			return result;
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x0019F8E4 File Offset: 0x0019E8E4
		private static object CoerceTop(DependencyObject d, object value)
		{
			Window window = d as Window;
			window.VerifyApiSupported();
			double num = (double)value;
			Window.ValidateTopLeft(num);
			if (window.IsSourceWindowNull || window.IsCompositionTargetInvalid)
			{
				return value;
			}
			if (double.IsNaN(num))
			{
				return window._actualTop;
			}
			if (window.WindowState != WindowState.Normal)
			{
				return value;
			}
			if (window._updateStartupLocation && window.WindowStartupLocation != WindowStartupLocation.Manual)
			{
				return window._actualTop;
			}
			return value;
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x0019F958 File Offset: 0x0019E958
		private static void _OnTopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndLocation)
			{
				window.OnTopChanged((double)e.NewValue);
			}
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x0019F988 File Offset: 0x0019E988
		private void OnTopChanged(double newTop)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				if (!DoubleUtil.IsNaN(newTop))
				{
					if (this.WindowState == WindowState.Normal)
					{
						Invariant.Assert(!double.IsNaN(this._actualLeft), "_actualLeft cannot be NaN after show");
						this.UpdateHwndPositionOnTopLeftChange(double.IsNaN(this.Left) ? this._actualLeft : this.Left, newTop);
						return;
					}
					this.UpdateHwndRestoreBounds(newTop, Window.BoundsSpecified.Top);
					return;
				}
			}
			else
			{
				this._actualTop = newTop;
			}
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x0019FA00 File Offset: 0x0019EA00
		private static object CoerceLeft(DependencyObject d, object value)
		{
			Window window = d as Window;
			window.VerifyApiSupported();
			double num = (double)value;
			Window.ValidateTopLeft(num);
			if (window.IsSourceWindowNull || window.IsCompositionTargetInvalid)
			{
				return value;
			}
			if (double.IsNaN(num))
			{
				return window._actualLeft;
			}
			if (window.WindowState != WindowState.Normal)
			{
				return value;
			}
			if (window._updateStartupLocation && window.WindowStartupLocation != WindowStartupLocation.Manual)
			{
				return window._actualLeft;
			}
			return value;
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x0019FA74 File Offset: 0x0019EA74
		private static void _OnLeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window._updateHwndLocation)
			{
				window.OnLeftChanged((double)e.NewValue);
			}
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x0019FAA4 File Offset: 0x0019EAA4
		private void OnLeftChanged(double newLeft)
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				if (!DoubleUtil.IsNaN(newLeft))
				{
					if (this.WindowState == WindowState.Normal)
					{
						Invariant.Assert(!double.IsNaN(this._actualTop), "_actualTop cannot be NaN after show");
						this.UpdateHwndPositionOnTopLeftChange(newLeft, double.IsNaN(this.Top) ? this._actualTop : this.Top);
						return;
					}
					this.UpdateHwndRestoreBounds(newLeft, Window.BoundsSpecified.Left);
					return;
				}
			}
			else
			{
				this._actualLeft = newLeft;
			}
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x0019FB1C File Offset: 0x0019EB1C
		private void UpdateHwndPositionOnTopLeftChange(double leftLogicalUnits, double topLogicalUnits)
		{
			Point point = this.LogicalToDeviceUnits(new Point(leftLogicalUnits, topLogicalUnits));
			UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), new HandleRef(null, IntPtr.Zero), DoubleUtil.DoubleToInt(point.X), DoubleUtil.DoubleToInt(point.Y), 0, 0, 21);
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x0019FB70 File Offset: 0x0019EB70
		private static bool _ValidateResizeModeCallback(object value)
		{
			return Window.IsValidResizeMode((ResizeMode)value);
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x0019FB7D File Offset: 0x0019EB7D
		private static void _OnResizeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Window).OnResizeModeChanged();
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x0019FB8C File Offset: 0x0019EB8C
		private void OnResizeModeChanged()
		{
			this.VerifyApiSupported();
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.CreateResizibility();
				}
			}
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x0019FBE4 File Offset: 0x0019EBE4
		private static object VerifyAccessCoercion(DependencyObject d, object value)
		{
			((Window)d).VerifyApiSupported();
			return value;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x0019FBF2 File Offset: 0x0019EBF2
		private static void _OnFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as Window).OnFlowDirectionChanged();
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x0019FC00 File Offset: 0x0019EC00
		private void OnFlowDirectionChanged()
		{
			if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
			{
				using (Window.HwndStyleManager.StartManaging(this, this.StyleFromHwnd, this.StyleExFromHwnd))
				{
					this.CreateRtl();
				}
			}
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x0019FC54 File Offset: 0x0019EC54
		private static object CoerceRenderTransform(DependencyObject d, object value)
		{
			Transform transform = (Transform)value;
			if (value != null && (transform == null || !transform.Value.IsIdentity))
			{
				throw new InvalidOperationException(SR.Get("TransformNotSupported"));
			}
			return value;
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void _OnRenderTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x0019FC8F File Offset: 0x0019EC8F
		private static object CoerceClipToBounds(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				throw new InvalidOperationException(SR.Get("ClipToBoundsNotSupported"));
			}
			return value;
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void _OnClipToBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x0019FCAC File Offset: 0x0019ECAC
		private HwndWrapper EnsureHiddenWindow()
		{
			if (this._hiddenWindow == null)
			{
				this._hiddenWindow = new HwndWrapper(0, 13565952, 0, int.MinValue, int.MinValue, int.MinValue, int.MinValue, "Hidden Window", IntPtr.Zero, null);
			}
			return this._hiddenWindow;
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x0019FCF8 File Offset: 0x0019ECF8
		private void SetTaskbarStatus()
		{
			if (!this.ShowInTaskbar)
			{
				this.EnsureHiddenWindow();
				if (this._ownerHandle == IntPtr.Zero)
				{
					this.SetOwnerHandle(this._hiddenWindow.Handle);
					if (!this.IsSourceWindowNull && !this.IsCompositionTargetInvalid)
					{
						this.UpdateIcon();
					}
				}
				this._StyleEx &= -262145;
				return;
			}
			this._StyleEx |= 262144;
			if (!this.IsSourceWindowNull && this._hiddenWindow != null && this._ownerHandle == this._hiddenWindow.Handle)
			{
				this.SetOwnerHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x0019FDA5 File Offset: 0x0019EDA5
		private void OnTaskbarRetryTimerTick(object sender, EventArgs e)
		{
			UnsafeNativeMethods.PostMessage(new HandleRef(this, this.CriticalHandle), Window.WM_APPLYTASKBARITEMINFO, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x0019FDC8 File Offset: 0x0019EDC8
		private void ApplyTaskbarItemInfo()
		{
			if (!Utilities.IsOSWindows7OrNewer)
			{
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Warning, TraceShell.NotOnWindows7);
				}
				return;
			}
			if (this.IsSourceWindowNull || this.IsCompositionTargetInvalid)
			{
				return;
			}
			if (this._taskbarRetryTimer != null && this._taskbarRetryTimer.IsEnabled)
			{
				return;
			}
			if (this._taskbarList == null)
			{
				if (this.TaskbarItemInfo == null)
				{
					return;
				}
				ITaskbarList taskbarList = null;
				try
				{
					taskbarList = (ITaskbarList)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("56FDF344-FD6D-11d0-958A-006097C9A090")));
					taskbarList.HrInit();
					this._taskbarList = (ITaskbarList3)taskbarList;
					taskbarList = null;
				}
				finally
				{
					Utilities.SafeRelease<ITaskbarList>(ref taskbarList);
				}
				this._overlaySize = new Size((double)UnsafeNativeMethods.GetSystemMetrics(SM.CXSMICON), (double)UnsafeNativeMethods.GetSystemMetrics(SM.CYSMICON));
				if (this._taskbarRetryTimer == null)
				{
					this._taskbarRetryTimer = new DispatcherTimer
					{
						Interval = new TimeSpan(0, 1, 0)
					};
					this._taskbarRetryTimer.Tick += this.OnTaskbarRetryTimerTick;
				}
			}
			HRESULT hr = HRESULT.S_OK;
			hr = this.RegisterTaskbarThumbButtons();
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarProgressState();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarOverlay();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarDescription();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarThumbnailClipping();
			}
			if (hr.Succeeded)
			{
				hr = this.UpdateTaskbarThumbButtons();
			}
			this.HandleTaskbarListError(hr);
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0019FF2C File Offset: 0x0019EF2C
		private HRESULT UpdateTaskbarProgressState()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			TBPF tbpFlags = TBPF.NOPROGRESS;
			if (taskbarItemInfo != null)
			{
				switch (taskbarItemInfo.ProgressState)
				{
				case TaskbarItemProgressState.None:
					tbpFlags = TBPF.NOPROGRESS;
					break;
				case TaskbarItemProgressState.Indeterminate:
					tbpFlags = TBPF.INDETERMINATE;
					break;
				case TaskbarItemProgressState.Normal:
					tbpFlags = TBPF.NORMAL;
					break;
				case TaskbarItemProgressState.Error:
					tbpFlags = TBPF.ERROR;
					break;
				case TaskbarItemProgressState.Paused:
					tbpFlags = TBPF.PAUSED;
					break;
				default:
					tbpFlags = TBPF.NOPROGRESS;
					break;
				}
			}
			HRESULT result = this._taskbarList.SetProgressState(this.CriticalHandle, tbpFlags);
			if (result.Succeeded)
			{
				result = this.UpdateTaskbarProgressValue();
			}
			return result;
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0019FFA4 File Offset: 0x0019EFA4
		private HRESULT UpdateTaskbarProgressValue()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			if (taskbarItemInfo == null || taskbarItemInfo.ProgressState == TaskbarItemProgressState.None || taskbarItemInfo.ProgressState == TaskbarItemProgressState.Indeterminate)
			{
				return HRESULT.S_OK;
			}
			ulong ullCompleted = (ulong)(taskbarItemInfo.ProgressValue * 1000.0);
			return this._taskbarList.SetProgressValue(this.CriticalHandle, ullCompleted, 1000UL);
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x0019FFFC File Offset: 0x0019EFFC
		private HRESULT UpdateTaskbarOverlay()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			NativeMethods.IconHandle iconHandle = NativeMethods.IconHandle.GetInvalidIcon();
			HRESULT result;
			try
			{
				if (taskbarItemInfo != null && taskbarItemInfo.Overlay != null)
				{
					iconHandle = IconHelper.CreateIconHandleFromImageSource(taskbarItemInfo.Overlay, this._overlaySize);
				}
				result = this._taskbarList.SetOverlayIcon(this.CriticalHandle, iconHandle, null);
			}
			finally
			{
				iconHandle.Dispose();
			}
			return result;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x001A0064 File Offset: 0x0019F064
		private HRESULT UpdateTaskbarDescription()
		{
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			string pszTip = "";
			if (taskbarItemInfo != null)
			{
				pszTip = (taskbarItemInfo.Description ?? "");
			}
			return this._taskbarList.SetThumbnailTooltip(this.CriticalHandle, pszTip);
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x001A00A4 File Offset: 0x0019F0A4
		private HRESULT UpdateTaskbarThumbnailClipping()
		{
			if (this._taskbarList == null)
			{
				return HRESULT.S_OK;
			}
			if (this._taskbarRetryTimer != null && this._taskbarRetryTimer.IsEnabled)
			{
				return HRESULT.S_FALSE;
			}
			if (UnsafeNativeMethods.IsIconic(this.CriticalHandle))
			{
				return HRESULT.S_FALSE;
			}
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			NativeMethods.RefRECT prcClip = null;
			if (taskbarItemInfo != null && !taskbarItemInfo.ThumbnailClipMargin.IsZero)
			{
				Thickness thumbnailClipMargin = taskbarItemInfo.ThumbnailClipMargin;
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetClientRect(new HandleRef(this, this.CriticalHandle), ref rect);
				Rect rect2 = new Rect(this.DeviceToLogicalUnits(new Point((double)rect.left, (double)rect.top)), this.DeviceToLogicalUnits(new Point((double)rect.right, (double)rect.bottom)));
				if (thumbnailClipMargin.Left + thumbnailClipMargin.Right >= rect2.Width || thumbnailClipMargin.Top + thumbnailClipMargin.Bottom >= rect2.Height)
				{
					prcClip = new NativeMethods.RefRECT(0, 0, 0, 0);
				}
				else
				{
					Rect rect3 = new Rect(this.LogicalToDeviceUnits(new Point(thumbnailClipMargin.Left, thumbnailClipMargin.Top)), this.LogicalToDeviceUnits(new Point(rect2.Width - thumbnailClipMargin.Right, rect2.Height - thumbnailClipMargin.Bottom)));
					prcClip = new NativeMethods.RefRECT((int)rect3.Left, (int)rect3.Top, (int)rect3.Right, (int)rect3.Bottom);
				}
			}
			return this._taskbarList.SetThumbnailClip(this.CriticalHandle, prcClip);
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x001A022C File Offset: 0x0019F22C
		private HRESULT RegisterTaskbarThumbButtons()
		{
			THUMBBUTTON[] array = new THUMBBUTTON[7];
			for (int i = 0; i < 7; i++)
			{
				array[i] = new THUMBBUTTON
				{
					iId = (uint)i,
					dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN | THBF.NONINTERACTIVE),
					dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
				};
			}
			HRESULT hresult = this._taskbarList.ThumbBarAddButtons(this.CriticalHandle, (uint)array.Length, array);
			if (hresult == HRESULT.E_INVALIDARG)
			{
				hresult = HRESULT.S_FALSE;
			}
			return hresult;
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x001A02A0 File Offset: 0x0019F2A0
		private HRESULT UpdateTaskbarThumbButtons()
		{
			THUMBBUTTON[] array = new THUMBBUTTON[7];
			TaskbarItemInfo taskbarItemInfo = this.TaskbarItemInfo;
			ThumbButtonInfoCollection thumbButtonInfoCollection = null;
			if (taskbarItemInfo != null)
			{
				thumbButtonInfoCollection = taskbarItemInfo.ThumbButtonInfos;
			}
			List<NativeMethods.IconHandle> list = new List<NativeMethods.IconHandle>();
			HRESULT result;
			try
			{
				uint num = 0U;
				if (thumbButtonInfoCollection == null)
				{
					goto IL_1B4;
				}
				using (FreezableCollection<ThumbButtonInfo>.Enumerator enumerator = thumbButtonInfoCollection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThumbButtonInfo thumbButtonInfo = enumerator.Current;
						THUMBBUTTON thumbbutton = new THUMBBUTTON
						{
							iId = num,
							dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
						};
						switch (thumbButtonInfo.Visibility)
						{
						case Visibility.Visible:
							goto IL_A1;
						case Visibility.Hidden:
							thumbbutton.dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND);
							thumbbutton.hIcon = IntPtr.Zero;
							break;
						case Visibility.Collapsed:
							thumbbutton.dwFlags = THBF.HIDDEN;
							break;
						default:
							goto IL_A1;
						}
						IL_14E:
						array[(int)num] = thumbbutton;
						num += 1U;
						if (num == 7U)
						{
							break;
						}
						continue;
						IL_A1:
						thumbbutton.szTip = (thumbButtonInfo.Description ?? "");
						if (thumbButtonInfo.ImageSource != null)
						{
							NativeMethods.IconHandle iconHandle = IconHelper.CreateIconHandleFromImageSource(thumbButtonInfo.ImageSource, this._overlaySize);
							thumbbutton.hIcon = iconHandle.CriticalGetHandle();
							list.Add(iconHandle);
						}
						if (!thumbButtonInfo.IsBackgroundVisible)
						{
							thumbbutton.dwFlags |= THBF.NOBACKGROUND;
						}
						if (!thumbButtonInfo.IsEnabled)
						{
							thumbbutton.dwFlags |= THBF.DISABLED;
						}
						else
						{
							thumbbutton.dwFlags |= THBF.ENABLED;
						}
						if (!thumbButtonInfo.IsInteractive)
						{
							thumbbutton.dwFlags |= THBF.NONINTERACTIVE;
						}
						if (thumbButtonInfo.DismissWhenClicked)
						{
							thumbbutton.dwFlags |= THBF.DISMISSONCLICK;
							goto IL_14E;
						}
						goto IL_14E;
					}
					goto IL_1B4;
				}
				IL_181:
				array[(int)num] = new THUMBBUTTON
				{
					iId = num,
					dwFlags = (THBF.DISABLED | THBF.NOBACKGROUND | THBF.HIDDEN),
					dwMask = (THB.ICON | THB.TOOLTIP | THB.FLAGS)
				};
				num += 1U;
				IL_1B4:
				if (num < 7U)
				{
					goto IL_181;
				}
				result = this._taskbarList.ThumbBarUpdateButtons(this.CriticalHandle, (uint)array.Length, array);
			}
			finally
			{
				foreach (NativeMethods.IconHandle iconHandle2 in list)
				{
					iconHandle2.Dispose();
				}
			}
			return result;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x001A0500 File Offset: 0x0019F500
		private void CreateRtl()
		{
			if (base.FlowDirection == FlowDirection.LeftToRight)
			{
				this._StyleEx &= -4194305;
				return;
			}
			if (base.FlowDirection == FlowDirection.RightToLeft)
			{
				this._StyleEx |= 4194304;
				return;
			}
			throw new InvalidOperationException(SR.Get("IncorrectFlowDirection"));
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x001A0554 File Offset: 0x0019F554
		internal void Flush()
		{
			Window.HwndStyleManager manager = this.Manager;
			if (manager.Dirty && this.CriticalHandle != IntPtr.Zero)
			{
				UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, this.CriticalHandle), -16, (IntPtr)this._styleDoNotUse.Value);
				UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, this.CriticalHandle), -20, (IntPtr)this._styleExDoNotUse.Value);
				UnsafeNativeMethods.SetWindowPos(new HandleRef(this, this.CriticalHandle), NativeMethods.NullHandleRef, 0, 0, 0, 0, 55);
				manager.Dirty = false;
			}
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x001A05EE File Offset: 0x0019F5EE
		private void ClearRootVisual()
		{
			if (this._swh != null)
			{
				this._swh.ClearRootVisual();
			}
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x001A0603 File Offset: 0x0019F603
		private NativeMethods.POINT GetPointRelativeToWindow(int x, int y)
		{
			return this._swh.GetPointRelativeToWindow(x, y, base.FlowDirection);
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x001A0618 File Offset: 0x0019F618
		private Size GetHwndNonClientAreaSizeInMeasureUnits()
		{
			if (!this.AllowsTransparency)
			{
				return this._swh.GetHwndNonClientAreaSizeInMeasureUnits();
			}
			return new Size(0.0, 0.0);
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x001A0648 File Offset: 0x0019F648
		private void ClearSourceWindow()
		{
			if (this._swh != null)
			{
				try
				{
					this._swh.RemoveDisposedHandler(new EventHandler(this.OnSourceWindowDisposed));
				}
				finally
				{
					HwndSource hwndSourceWindow = this._swh.HwndSourceWindow;
					this._swh = null;
					if (hwndSourceWindow != null)
					{
						hwndSourceWindow.SizeToContentChanged -= this.OnSourceSizeToContentChanged;
					}
				}
			}
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x001A06B0 File Offset: 0x0019F6B0
		private void ClearHiddenWindowIfAny()
		{
			if (this._hiddenWindow != null && this._hiddenWindow.Handle == this._ownerHandle)
			{
				this.SetOwnerHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x001A06DD File Offset: 0x0019F6DD
		private void VerifyConsistencyWithAllowsTransparency()
		{
			if (this.AllowsTransparency)
			{
				this.VerifyConsistencyWithAllowsTransparency(this.WindowStyle);
			}
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x001A06F3 File Offset: 0x0019F6F3
		private void VerifyConsistencyWithAllowsTransparency(WindowStyle style)
		{
			if (this.AllowsTransparency && style != WindowStyle.None)
			{
				throw new InvalidOperationException(SR.Get("MustUseWindowStyleNone"));
			}
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x001A0710 File Offset: 0x0019F710
		private void VerifyConsistencyWithShowActivated()
		{
			if (!this._inTrustedSubWindow && this.WindowState == WindowState.Maximized && !this.ShowActivated)
			{
				throw new InvalidOperationException(SR.Get("ShowNonActivatedAndMaximized"));
			}
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x001A073B File Offset: 0x0019F73B
		private static bool IsValidSizeToContent(SizeToContent value)
		{
			return value == SizeToContent.Manual || value == SizeToContent.Width || value == SizeToContent.Height || value == SizeToContent.WidthAndHeight;
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x001A073B File Offset: 0x0019F73B
		private static bool IsValidResizeMode(ResizeMode value)
		{
			return value == ResizeMode.NoResize || value == ResizeMode.CanMinimize || value == ResizeMode.CanResize || value == ResizeMode.CanResizeWithGrip;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x001A074E File Offset: 0x0019F74E
		private static bool IsValidWindowStartupLocation(WindowStartupLocation value)
		{
			return value == WindowStartupLocation.CenterOwner || value == WindowStartupLocation.CenterScreen || value == WindowStartupLocation.Manual;
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x001A074E File Offset: 0x0019F74E
		private static bool IsValidWindowState(WindowState value)
		{
			return value == WindowState.Maximized || value == WindowState.Minimized || value == WindowState.Normal;
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x001A073B File Offset: 0x0019F73B
		private static bool IsValidWindowStyle(WindowStyle value)
		{
			return value == WindowStyle.None || value == WindowStyle.SingleBorderWindow || value == WindowStyle.ThreeDBorderWindow || value == WindowStyle.ToolWindow;
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x001A0760 File Offset: 0x0019F760
		protected override void OnManipulationBoundaryFeedback(ManipulationBoundaryFeedbackEventArgs e)
		{
			base.OnManipulationBoundaryFeedback(e);
			if (!PresentationSource.UnderSamePresentationSource(new DependencyObject[]
			{
				e.OriginalSource as DependencyObject,
				this
			}))
			{
				return;
			}
			if (!e.Handled)
			{
				if ((this._currentPanningTarget == null || !this._currentPanningTarget.IsAlive || this._currentPanningTarget.Target != e.OriginalSource) && this._swh != null)
				{
					NativeMethods.RECT windowBounds = this.WindowBounds;
					this._prePanningLocation = this.DeviceToLogicalUnits(new Point((double)windowBounds.left, (double)windowBounds.top));
				}
				ManipulationDelta boundaryFeedback = e.BoundaryFeedback;
				this.UpdatePanningFeedback(boundaryFeedback.Translation, e.OriginalSource);
				e.CompensateForBoundaryFeedback = new Func<Point, Point>(this.CompensateForPanningFeedback);
			}
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x001A0820 File Offset: 0x0019F820
		private static void OnStaticManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				window.EndPanningFeedback(true);
			}
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x001A0840 File Offset: 0x0019F840
		private static void OnStaticManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
			Window window = sender as Window;
			if (window != null)
			{
				window.EndPanningFeedback(false);
			}
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x001A0860 File Offset: 0x0019F860
		private void UpdatePanningFeedback(Vector totalOverpanOffset, object originalSource)
		{
			if (this._currentPanningTarget != null && !this._currentPanningTarget.IsAlive)
			{
				this._currentPanningTarget = null;
				this.EndPanningFeedback(false);
			}
			if (this._swh != null)
			{
				if (this._currentPanningTarget == null)
				{
					this._currentPanningTarget = new WeakReference(originalSource);
				}
				if (originalSource == this._currentPanningTarget.Target)
				{
					this._swh.UpdatePanningFeedback(totalOverpanOffset, false);
				}
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x001A08C7 File Offset: 0x0019F8C7
		private void EndPanningFeedback(bool animateBack)
		{
			if (this._swh != null)
			{
				this._swh.EndPanningFeedback(animateBack);
			}
			this._currentPanningTarget = null;
			this._prePanningLocation = new Point(double.NaN, double.NaN);
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x001A0904 File Offset: 0x0019F904
		private Point CompensateForPanningFeedback(Point point)
		{
			if (!double.IsNaN(this._prePanningLocation.X) && !double.IsNaN(this._prePanningLocation.Y) && this._swh != null)
			{
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point2 = this.DeviceToLogicalUnits(new Point((double)windowBounds.left, (double)windowBounds.top));
				return new Point(point.X - (this._prePanningLocation.X - point2.X), point.Y - (this._prePanningLocation.Y - point2.Y));
			}
			return point;
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x001A099B File Offset: 0x0019F99B
		// (set) Token: 0x06002AE4 RID: 10980 RVA: 0x001A09A8 File Offset: 0x0019F9A8
		private SizeToContent HwndSourceSizeToContent
		{
			get
			{
				return this._swh.HwndSourceSizeToContent;
			}
			set
			{
				this._swh.HwndSourceSizeToContent = value;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x001A09B6 File Offset: 0x0019F9B6
		private NativeMethods.RECT WindowBounds
		{
			get
			{
				return this._swh.WindowBounds;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x001A09C3 File Offset: 0x0019F9C3
		private int StyleFromHwnd
		{
			get
			{
				if (this._swh == null)
				{
					return 0;
				}
				return this._swh.StyleFromHwnd;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x001A09DA File Offset: 0x0019F9DA
		private int StyleExFromHwnd
		{
			get
			{
				if (this._swh == null)
				{
					return 0;
				}
				return this._swh.StyleExFromHwnd;
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x001A09F1 File Offset: 0x0019F9F1
		private WindowCollection OwnedWindowsInternal
		{
			get
			{
				if (this._ownedWindows == null)
				{
					this._ownedWindows = new WindowCollection();
				}
				return this._ownedWindows;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x001A0A0C File Offset: 0x0019FA0C
		private Application App
		{
			get
			{
				return Application.Current;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06002AEA RID: 10986 RVA: 0x001A0A13 File Offset: 0x0019FA13
		private bool IsInsideApp
		{
			get
			{
				return Application.Current != null;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06002AEB RID: 10987 RVA: 0x001A0A1D File Offset: 0x0019FA1D
		private EventHandlerList Events
		{
			get
			{
				if (this._events == null)
				{
					this._events = new EventHandlerList();
				}
				return this._events;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06002AEC RID: 10988 RVA: 0x001A0A38 File Offset: 0x0019FA38
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Window._dType;
			}
		}

		// Token: 0x0400150F RID: 5391
		public static readonly DependencyProperty TaskbarItemInfoProperty = DependencyProperty.Register("TaskbarItemInfo", typeof(TaskbarItemInfo), typeof(Window), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Window)d).OnTaskbarItemInfoChanged(e);
		}, new CoerceValueCallback(Window.VerifyAccessCoercion)));

		// Token: 0x04001511 RID: 5393
		public static readonly DependencyProperty AllowsTransparencyProperty = DependencyProperty.Register("AllowsTransparency", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Window.OnAllowsTransparencyChanged), new CoerceValueCallback(Window.CoerceAllowsTransparency)));

		// Token: 0x04001512 RID: 5394
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Window), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(Window._OnTitleChanged)), new ValidateValueCallback(Window._ValidateText));

		// Token: 0x04001513 RID: 5395
		public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(Window), new FrameworkPropertyMetadata(new PropertyChangedCallback(Window._OnIconChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)));

		// Token: 0x04001514 RID: 5396
		public static readonly DependencyProperty SizeToContentProperty = DependencyProperty.Register("SizeToContent", typeof(SizeToContent), typeof(Window), new FrameworkPropertyMetadata(SizeToContent.Manual, new PropertyChangedCallback(Window._OnSizeToContentChanged)), new ValidateValueCallback(Window._ValidateSizeToContentCallback));

		// Token: 0x04001515 RID: 5397
		public static readonly DependencyProperty TopProperty = Canvas.TopProperty.AddOwner(typeof(Window), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Window._OnTopChanged), new CoerceValueCallback(Window.CoerceTop)));

		// Token: 0x04001516 RID: 5398
		public static readonly DependencyProperty LeftProperty = Canvas.LeftProperty.AddOwner(typeof(Window), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(Window._OnLeftChanged), new CoerceValueCallback(Window.CoerceLeft)));

		// Token: 0x04001517 RID: 5399
		public static readonly DependencyProperty ShowInTaskbarProperty = DependencyProperty.Register("ShowInTaskbar", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(Window._OnShowInTaskbarChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)));

		// Token: 0x04001518 RID: 5400
		private static readonly DependencyPropertyKey IsActivePropertyKey = DependencyProperty.RegisterReadOnly("IsActive", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04001519 RID: 5401
		public static readonly DependencyProperty IsActiveProperty = Window.IsActivePropertyKey.DependencyProperty;

		// Token: 0x0400151A RID: 5402
		public static readonly DependencyProperty WindowStyleProperty = DependencyProperty.Register("WindowStyle", typeof(WindowStyle), typeof(Window), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow, new PropertyChangedCallback(Window._OnWindowStyleChanged), new CoerceValueCallback(Window.CoerceWindowStyle)), new ValidateValueCallback(Window._ValidateWindowStyleCallback));

		// Token: 0x0400151B RID: 5403
		public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register("WindowState", typeof(WindowState), typeof(Window), new FrameworkPropertyMetadata(WindowState.Normal, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Window._OnWindowStateChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)), new ValidateValueCallback(Window._ValidateWindowStateCallback));

		// Token: 0x0400151C RID: 5404
		public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register("ResizeMode", typeof(ResizeMode), typeof(Window), new FrameworkPropertyMetadata(ResizeMode.CanResize, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(Window._OnResizeModeChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)), new ValidateValueCallback(Window._ValidateResizeModeCallback));

		// Token: 0x0400151D RID: 5405
		public static readonly DependencyProperty TopmostProperty = DependencyProperty.Register("Topmost", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Window._OnTopmostChanged), new CoerceValueCallback(Window.VerifyAccessCoercion)));

		// Token: 0x0400151E RID: 5406
		public static readonly DependencyProperty ShowActivatedProperty = DependencyProperty.Register("ShowActivated", typeof(bool), typeof(Window), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, null, new CoerceValueCallback(Window.VerifyAccessCoercion)));

		// Token: 0x0400151F RID: 5407
		internal static readonly RoutedCommand DialogCancelCommand = new RoutedCommand("DialogCancel", typeof(Window));

		// Token: 0x04001520 RID: 5408
		private Window.SourceWindowHelper _swh;

		// Token: 0x04001521 RID: 5409
		private Window _ownerWindow;

		// Token: 0x04001522 RID: 5410
		private IntPtr _ownerHandle = IntPtr.Zero;

		// Token: 0x04001523 RID: 5411
		private WindowCollection _ownedWindows;

		// Token: 0x04001524 RID: 5412
		private ArrayList _threadWindowHandles;

		// Token: 0x04001525 RID: 5413
		private bool _updateHwndSize = true;

		// Token: 0x04001526 RID: 5414
		private bool _updateHwndLocation = true;

		// Token: 0x04001527 RID: 5415
		private bool _updateStartupLocation;

		// Token: 0x04001528 RID: 5416
		private bool _isVisible;

		// Token: 0x04001529 RID: 5417
		private bool _isVisibilitySet;

		// Token: 0x0400152A RID: 5418
		private bool _resetKeyboardCuesProperty;

		// Token: 0x0400152B RID: 5419
		private bool _previousKeyboardCuesProperty;

		// Token: 0x0400152C RID: 5420
		private static bool _dialogCommandAdded;

		// Token: 0x0400152D RID: 5421
		private bool _postContentRenderedFromLoadedHandler;

		// Token: 0x0400152E RID: 5422
		private bool _disposed;

		// Token: 0x0400152F RID: 5423
		private bool _appShuttingDown;

		// Token: 0x04001530 RID: 5424
		private bool _ignoreCancel;

		// Token: 0x04001531 RID: 5425
		private bool _showingAsDialog;

		// Token: 0x04001532 RID: 5426
		private bool _isClosing;

		// Token: 0x04001533 RID: 5427
		private bool _visibilitySetInternally;

		// Token: 0x04001534 RID: 5428
		private bool _hwndCreatedButNotShown;

		// Token: 0x04001535 RID: 5429
		private double _trackMinWidthDeviceUnits;

		// Token: 0x04001536 RID: 5430
		private double _trackMinHeightDeviceUnits;

		// Token: 0x04001537 RID: 5431
		private double _trackMaxWidthDeviceUnits = double.PositiveInfinity;

		// Token: 0x04001538 RID: 5432
		private double _trackMaxHeightDeviceUnits = double.PositiveInfinity;

		// Token: 0x04001539 RID: 5433
		private double _windowMaxWidthDeviceUnits = double.PositiveInfinity;

		// Token: 0x0400153A RID: 5434
		private double _windowMaxHeightDeviceUnits = double.PositiveInfinity;

		// Token: 0x0400153B RID: 5435
		private double _actualTop = double.NaN;

		// Token: 0x0400153C RID: 5436
		private double _actualLeft = double.NaN;

		// Token: 0x0400153D RID: 5437
		private bool _inTrustedSubWindow;

		// Token: 0x0400153E RID: 5438
		private ImageSource _icon;

		// Token: 0x0400153F RID: 5439
		private NativeMethods.IconHandle _defaultLargeIconHandle;

		// Token: 0x04001540 RID: 5440
		private NativeMethods.IconHandle _defaultSmallIconHandle;

		// Token: 0x04001541 RID: 5441
		private NativeMethods.IconHandle _currentLargeIconHandle;

		// Token: 0x04001542 RID: 5442
		private NativeMethods.IconHandle _currentSmallIconHandle;

		// Token: 0x04001543 RID: 5443
		private bool? _dialogResult;

		// Token: 0x04001544 RID: 5444
		private IntPtr _dialogOwnerHandle = IntPtr.Zero;

		// Token: 0x04001545 RID: 5445
		private IntPtr _dialogPreviousActiveHandle;

		// Token: 0x04001546 RID: 5446
		private DispatcherFrame _dispatcherFrame;

		// Token: 0x04001547 RID: 5447
		private WindowStartupLocation _windowStartupLocation;

		// Token: 0x04001548 RID: 5448
		private WindowState _previousWindowState;

		// Token: 0x04001549 RID: 5449
		private HwndWrapper _hiddenWindow;

		// Token: 0x0400154A RID: 5450
		private EventHandlerList _events;

		// Token: 0x0400154B RID: 5451
		private SecurityCriticalDataForSet<int> _styleDoNotUse;

		// Token: 0x0400154C RID: 5452
		private SecurityCriticalDataForSet<int> _styleExDoNotUse;

		// Token: 0x0400154D RID: 5453
		private Window.HwndStyleManager _manager;

		// Token: 0x0400154E RID: 5454
		private Control _resizeGripControl;

		// Token: 0x0400154F RID: 5455
		private Point _prePanningLocation = new Point(double.NaN, double.NaN);

		// Token: 0x04001550 RID: 5456
		private static readonly object EVENT_SOURCEINITIALIZED = new object();

		// Token: 0x04001551 RID: 5457
		private static readonly object EVENT_CLOSING = new object();

		// Token: 0x04001552 RID: 5458
		private static readonly object EVENT_CLOSED = new object();

		// Token: 0x04001553 RID: 5459
		private static readonly object EVENT_ACTIVATED = new object();

		// Token: 0x04001554 RID: 5460
		private static readonly object EVENT_DEACTIVATED = new object();

		// Token: 0x04001555 RID: 5461
		private static readonly object EVENT_STATECHANGED = new object();

		// Token: 0x04001556 RID: 5462
		private static readonly object EVENT_LOCATIONCHANGED = new object();

		// Token: 0x04001557 RID: 5463
		private static readonly object EVENT_CONTENTRENDERED = new object();

		// Token: 0x04001558 RID: 5464
		private static readonly object EVENT_VISUALCHILDRENCHANGED = new object();

		// Token: 0x04001559 RID: 5465
		private static readonly WindowMessage WM_TASKBARBUTTONCREATED;

		// Token: 0x0400155A RID: 5466
		private static readonly WindowMessage WM_APPLYTASKBARITEMINFO;

		// Token: 0x0400155B RID: 5467
		private const int c_MaximumThumbButtons = 7;

		// Token: 0x0400155C RID: 5468
		private ITaskbarList3 _taskbarList;

		// Token: 0x0400155D RID: 5469
		private DispatcherTimer _taskbarRetryTimer;

		// Token: 0x0400155E RID: 5470
		private Size _overlaySize;

		// Token: 0x0400155F RID: 5471
		internal static readonly DependencyProperty IWindowServiceProperty = DependencyProperty.RegisterAttached("IWindowService", typeof(IWindowService), typeof(Window), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior));

		// Token: 0x04001560 RID: 5472
		private DispatcherOperation _contentRenderedCallback;

		// Token: 0x04001561 RID: 5473
		private WeakReference _currentPanningTarget;

		// Token: 0x04001562 RID: 5474
		private static DependencyObjectType _dType;

		// Token: 0x02000A9A RID: 2714
		internal struct WindowMinMax
		{
			// Token: 0x060086D6 RID: 34518 RVA: 0x0032B9A9 File Offset: 0x0032A9A9
			internal WindowMinMax(double minSize, double maxSize)
			{
				this.minWidth = minSize;
				this.maxWidth = maxSize;
				this.minHeight = minSize;
				this.maxHeight = maxSize;
			}

			// Token: 0x0400428E RID: 17038
			internal double minWidth;

			// Token: 0x0400428F RID: 17039
			internal double maxWidth;

			// Token: 0x04004290 RID: 17040
			internal double minHeight;

			// Token: 0x04004291 RID: 17041
			internal double maxHeight;
		}

		// Token: 0x02000A9B RID: 2715
		internal class SourceWindowHelper
		{
			// Token: 0x060086D7 RID: 34519 RVA: 0x0032B9C7 File Offset: 0x0032A9C7
			internal SourceWindowHelper(HwndSource sourceWindow)
			{
				this._sourceWindow = sourceWindow;
			}

			// Token: 0x17001E40 RID: 7744
			// (get) Token: 0x060086D8 RID: 34520 RVA: 0x0032B9D6 File Offset: 0x0032A9D6
			internal bool IsSourceWindowNull
			{
				get
				{
					return this._sourceWindow == null;
				}
			}

			// Token: 0x17001E41 RID: 7745
			// (get) Token: 0x060086D9 RID: 34521 RVA: 0x0032B9E1 File Offset: 0x0032A9E1
			internal bool IsCompositionTargetInvalid
			{
				get
				{
					return this.CompositionTarget == null;
				}
			}

			// Token: 0x17001E42 RID: 7746
			// (get) Token: 0x060086DA RID: 34522 RVA: 0x0032B9EC File Offset: 0x0032A9EC
			internal IntPtr CriticalHandle
			{
				get
				{
					if (this._sourceWindow != null)
					{
						return this._sourceWindow.CriticalHandle;
					}
					return IntPtr.Zero;
				}
			}

			// Token: 0x17001E43 RID: 7747
			// (get) Token: 0x060086DB RID: 34523 RVA: 0x0032BA08 File Offset: 0x0032AA08
			internal NativeMethods.RECT WorkAreaBoundsForNearestMonitor
			{
				get
				{
					NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
					monitorinfoex.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFOEX));
					IntPtr intPtr = SafeNativeMethods.MonitorFromWindow(new HandleRef(this, this.CriticalHandle), 2);
					if (intPtr != IntPtr.Zero)
					{
						SafeNativeMethods.GetMonitorInfo(new HandleRef(this, intPtr), monitorinfoex);
					}
					return monitorinfoex.rcWork;
				}
			}

			// Token: 0x17001E44 RID: 7748
			// (get) Token: 0x060086DC RID: 34524 RVA: 0x0032BA64 File Offset: 0x0032AA64
			private NativeMethods.RECT ClientBounds
			{
				get
				{
					NativeMethods.RECT result = new NativeMethods.RECT(0, 0, 0, 0);
					SafeNativeMethods.GetClientRect(new HandleRef(this, this.CriticalHandle), ref result);
					return result;
				}
			}

			// Token: 0x17001E45 RID: 7749
			// (get) Token: 0x060086DD RID: 34525 RVA: 0x0032BA90 File Offset: 0x0032AA90
			internal NativeMethods.RECT WindowBounds
			{
				get
				{
					NativeMethods.RECT result = new NativeMethods.RECT(0, 0, 0, 0);
					SafeNativeMethods.GetWindowRect(new HandleRef(this, this.CriticalHandle), ref result);
					return result;
				}
			}

			// Token: 0x060086DE RID: 34526 RVA: 0x0032BABC File Offset: 0x0032AABC
			private NativeMethods.POINT GetWindowScreenLocation(FlowDirection flowDirection)
			{
				NativeMethods.POINT point = new NativeMethods.POINT(0, 0);
				if (flowDirection == FlowDirection.RightToLeft)
				{
					NativeMethods.RECT rect = new NativeMethods.RECT(0, 0, 0, 0);
					SafeNativeMethods.GetClientRect(new HandleRef(this, this.CriticalHandle), ref rect);
					point = new NativeMethods.POINT(rect.right, rect.top);
				}
				UnsafeNativeMethods.ClientToScreen(new HandleRef(this, this._sourceWindow.CriticalHandle), point);
				return point;
			}

			// Token: 0x17001E46 RID: 7750
			// (get) Token: 0x060086DF RID: 34527 RVA: 0x0032BB1D File Offset: 0x0032AB1D
			// (set) Token: 0x060086E0 RID: 34528 RVA: 0x0032BB2A File Offset: 0x0032AB2A
			internal SizeToContent HwndSourceSizeToContent
			{
				get
				{
					return this._sourceWindow.SizeToContent;
				}
				set
				{
					this._sourceWindow.SizeToContent = value;
				}
			}

			// Token: 0x17001E47 RID: 7751
			// (set) Token: 0x060086E1 RID: 34529 RVA: 0x0032BB38 File Offset: 0x0032AB38
			internal Visual RootVisual
			{
				set
				{
					this._sourceWindow.RootVisual = value;
				}
			}

			// Token: 0x17001E48 RID: 7752
			// (get) Token: 0x060086E2 RID: 34530 RVA: 0x0032BB46 File Offset: 0x0032AB46
			internal bool IsActiveWindow
			{
				get
				{
					return this._sourceWindow.CriticalHandle == UnsafeNativeMethods.GetActiveWindow();
				}
			}

			// Token: 0x17001E49 RID: 7753
			// (get) Token: 0x060086E3 RID: 34531 RVA: 0x0032BB5D File Offset: 0x0032AB5D
			internal HwndSource HwndSourceWindow
			{
				get
				{
					return this._sourceWindow;
				}
			}

			// Token: 0x17001E4A RID: 7754
			// (get) Token: 0x060086E4 RID: 34532 RVA: 0x0032BB68 File Offset: 0x0032AB68
			internal HwndTarget CompositionTarget
			{
				get
				{
					if (this._sourceWindow != null)
					{
						HwndTarget compositionTarget = this._sourceWindow.CompositionTarget;
						if (compositionTarget != null && !compositionTarget.IsDisposed)
						{
							return compositionTarget;
						}
					}
					return null;
				}
			}

			// Token: 0x17001E4B RID: 7755
			// (get) Token: 0x060086E5 RID: 34533 RVA: 0x0032BB98 File Offset: 0x0032AB98
			internal Size WindowSize
			{
				get
				{
					NativeMethods.RECT windowBounds = this.WindowBounds;
					return new Size((double)(windowBounds.right - windowBounds.left), (double)(windowBounds.bottom - windowBounds.top));
				}
			}

			// Token: 0x17001E4C RID: 7756
			// (get) Token: 0x060086E6 RID: 34534 RVA: 0x0032BBCD File Offset: 0x0032ABCD
			internal int StyleExFromHwnd
			{
				get
				{
					return UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.CriticalHandle), -20);
				}
			}

			// Token: 0x17001E4D RID: 7757
			// (get) Token: 0x060086E7 RID: 34535 RVA: 0x0032BBE2 File Offset: 0x0032ABE2
			internal int StyleFromHwnd
			{
				get
				{
					return UnsafeNativeMethods.GetWindowLong(new HandleRef(this, this.CriticalHandle), -16);
				}
			}

			// Token: 0x060086E8 RID: 34536 RVA: 0x0032BBF8 File Offset: 0x0032ABF8
			internal NativeMethods.POINT GetPointRelativeToWindow(int x, int y, FlowDirection flowDirection)
			{
				NativeMethods.POINT windowScreenLocation = this.GetWindowScreenLocation(flowDirection);
				return new NativeMethods.POINT(x - windowScreenLocation.x, y - windowScreenLocation.y);
			}

			// Token: 0x060086E9 RID: 34537 RVA: 0x0032BC24 File Offset: 0x0032AC24
			internal Size GetSizeFromHwndInMeasureUnits()
			{
				Point point = new Point(0.0, 0.0);
				NativeMethods.RECT windowBounds = this.WindowBounds;
				point.X = (double)(windowBounds.right - windowBounds.left);
				point.Y = (double)(windowBounds.bottom - windowBounds.top);
				point = this._sourceWindow.CompositionTarget.TransformFromDevice.Transform(point);
				return new Size(point.X, point.Y);
			}

			// Token: 0x060086EA RID: 34538 RVA: 0x0032BCA8 File Offset: 0x0032ACA8
			internal Size GetHwndNonClientAreaSizeInMeasureUnits()
			{
				NativeMethods.RECT clientBounds = this.ClientBounds;
				NativeMethods.RECT windowBounds = this.WindowBounds;
				Point point = new Point((double)(windowBounds.right - windowBounds.left - (clientBounds.right - clientBounds.left)), (double)(windowBounds.bottom - windowBounds.top - (clientBounds.bottom - clientBounds.top)));
				point = this._sourceWindow.CompositionTarget.TransformFromDevice.Transform(point);
				return new Size(Math.Max(0.0, point.X), Math.Max(0.0, point.Y));
			}

			// Token: 0x060086EB RID: 34539 RVA: 0x0032BD4B File Offset: 0x0032AD4B
			internal void ClearRootVisual()
			{
				if (this._sourceWindow.RootVisual != null)
				{
					this._sourceWindow.RootVisual = null;
				}
			}

			// Token: 0x060086EC RID: 34540 RVA: 0x0032BD66 File Offset: 0x0032AD66
			internal void AddDisposedHandler(EventHandler theHandler)
			{
				if (this._sourceWindow != null)
				{
					this._sourceWindow.Disposed += theHandler;
				}
			}

			// Token: 0x060086ED RID: 34541 RVA: 0x0032BD7C File Offset: 0x0032AD7C
			internal void RemoveDisposedHandler(EventHandler theHandler)
			{
				if (this._sourceWindow != null)
				{
					this._sourceWindow.Disposed -= theHandler;
				}
			}

			// Token: 0x060086EE RID: 34542 RVA: 0x0032BD92 File Offset: 0x0032AD92
			internal void UpdatePanningFeedback(Vector totalOverpanOffset, bool animate)
			{
				if (this._panningFeedback == null && this._sourceWindow != null)
				{
					this._panningFeedback = new HwndPanningFeedback(this._sourceWindow);
				}
				if (this._panningFeedback != null)
				{
					this._panningFeedback.UpdatePanningFeedback(totalOverpanOffset, animate);
				}
			}

			// Token: 0x060086EF RID: 34543 RVA: 0x0032BDCA File Offset: 0x0032ADCA
			internal void EndPanningFeedback(bool animateBack)
			{
				if (this._panningFeedback != null)
				{
					this._panningFeedback.EndPanningFeedback(animateBack);
					this._panningFeedback = null;
				}
			}

			// Token: 0x04004292 RID: 17042
			private HwndSource _sourceWindow;

			// Token: 0x04004293 RID: 17043
			private HwndPanningFeedback _panningFeedback;
		}

		// Token: 0x02000A9C RID: 2716
		internal class HwndStyleManager : IDisposable
		{
			// Token: 0x060086F0 RID: 34544 RVA: 0x0032BDE7 File Offset: 0x0032ADE7
			internal static Window.HwndStyleManager StartManaging(Window w, int Style, int StyleEx)
			{
				if (w.Manager == null)
				{
					return new Window.HwndStyleManager(w, Style, StyleEx);
				}
				w.Manager._refCount++;
				return w.Manager;
			}

			// Token: 0x060086F1 RID: 34545 RVA: 0x0032BE14 File Offset: 0x0032AE14
			private HwndStyleManager(Window w, int Style, int StyleEx)
			{
				this._window = w;
				this._window.Manager = this;
				if (!w.IsSourceWindowNull)
				{
					this._window._Style = Style;
					this._window._StyleEx = StyleEx;
					this.Dirty = false;
				}
				this._refCount = 1;
			}

			// Token: 0x060086F2 RID: 34546 RVA: 0x0032BE68 File Offset: 0x0032AE68
			void IDisposable.Dispose()
			{
				this._refCount--;
				if (this._refCount == 0)
				{
					this._window.Flush();
					if (this._window.Manager == this)
					{
						this._window.Manager = null;
					}
				}
			}

			// Token: 0x17001E4E RID: 7758
			// (get) Token: 0x060086F3 RID: 34547 RVA: 0x0032BEA5 File Offset: 0x0032AEA5
			// (set) Token: 0x060086F4 RID: 34548 RVA: 0x0032BEAD File Offset: 0x0032AEAD
			internal bool Dirty
			{
				get
				{
					return this._fDirty;
				}
				set
				{
					this._fDirty = value;
				}
			}

			// Token: 0x04004294 RID: 17044
			private Window _window;

			// Token: 0x04004295 RID: 17045
			private int _refCount;

			// Token: 0x04004296 RID: 17046
			private bool _fDirty;
		}

		// Token: 0x02000A9D RID: 2717
		private class WindowStartupTopLeftPointHelper
		{
			// Token: 0x17001E4F RID: 7759
			// (get) Token: 0x060086F5 RID: 34549 RVA: 0x0032BEB6 File Offset: 0x0032AEB6
			internal Point LogicalTopLeft { get; }

			// Token: 0x17001E50 RID: 7760
			// (get) Token: 0x060086F6 RID: 34550 RVA: 0x0032BEBE File Offset: 0x0032AEBE
			// (set) Token: 0x060086F7 RID: 34551 RVA: 0x0032BEC6 File Offset: 0x0032AEC6
			internal Point? ScreenTopLeft { get; private set; }

			// Token: 0x060086F8 RID: 34552 RVA: 0x0032BECF File Offset: 0x0032AECF
			internal WindowStartupTopLeftPointHelper(Point topLeft)
			{
				this.LogicalTopLeft = topLeft;
				if (this.IsHelperNeeded)
				{
					this.IdentifyScreenTopLeft();
				}
			}

			// Token: 0x17001E51 RID: 7761
			// (get) Token: 0x060086F9 RID: 34553 RVA: 0x0032BEEC File Offset: 0x0032AEEC
			private bool IsHelperNeeded
			{
				get
				{
					if (CoreAppContextSwitches.DoNotUsePresentationDpiCapabilityTier2OrGreater)
					{
						return false;
					}
					if (!HwndTarget.IsPerMonitorDpiScalingEnabled)
					{
						return false;
					}
					if (HwndTarget.IsProcessPerMonitorDpiAware != null)
					{
						return HwndTarget.IsProcessPerMonitorDpiAware.Value;
					}
					return DpiUtil.GetProcessDpiAwareness(IntPtr.Zero) == NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE;
				}
			}

			// Token: 0x060086FA RID: 34554 RVA: 0x0032BF38 File Offset: 0x0032AF38
			private void IdentifyScreenTopLeft()
			{
				HandleRef hWnd = new HandleRef(null, IntPtr.Zero);
				IntPtr dc = UnsafeNativeMethods.GetDC(hWnd);
				UnsafeNativeMethods.EnumDisplayMonitors(dc, IntPtr.Zero, new NativeMethods.MonitorEnumProc(this.MonitorEnumProc), IntPtr.Zero);
				UnsafeNativeMethods.ReleaseDC(hWnd, new HandleRef(null, dc));
			}

			// Token: 0x060086FB RID: 34555 RVA: 0x0032BF84 File Offset: 0x0032AF84
			private bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref NativeMethods.RECT lprcMonitor, IntPtr dwData)
			{
				bool result = true;
				uint num;
				uint num2;
				if (UnsafeNativeMethods.GetDpiForMonitor(new HandleRef(null, hMonitor), NativeMethods.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out num, out num2) == 0U)
				{
					double num3 = num * 1.0 / 96.0;
					double num4 = num2 * 1.0 / 96.0;
					if (new Rect
					{
						X = (double)lprcMonitor.left / num3,
						Y = (double)lprcMonitor.top / num4,
						Width = (double)(lprcMonitor.right - lprcMonitor.left) / num3,
						Height = (double)(lprcMonitor.bottom - lprcMonitor.top) / num4
					}.Contains(this.LogicalTopLeft))
					{
						this.ScreenTopLeft = new Point?(new Point
						{
							X = this.LogicalTopLeft.X * num3,
							Y = this.LogicalTopLeft.Y * num4
						});
						result = false;
					}
				}
				return result;
			}
		}

		// Token: 0x02000A9E RID: 2718
		private enum TransformType
		{
			// Token: 0x0400429A RID: 17050
			WorkAreaToScreenArea,
			// Token: 0x0400429B RID: 17051
			ScreenAreaToWorkArea
		}

		// Token: 0x02000A9F RID: 2719
		private enum BoundsSpecified
		{
			// Token: 0x0400429D RID: 17053
			Height,
			// Token: 0x0400429E RID: 17054
			Width,
			// Token: 0x0400429F RID: 17055
			Top,
			// Token: 0x040042A0 RID: 17056
			Left
		}
	}
}
