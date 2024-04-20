using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x02000736 RID: 1846
	[DefaultEvent("Opened")]
	public class ContextMenu : MenuBase
	{
		// Token: 0x0600618F RID: 24975 RVA: 0x0029D394 File Offset: 0x0029C394
		static ContextMenu()
		{
			ContextMenu.OpenedEvent = PopupControlService.ContextMenuOpenedEvent.AddOwner(typeof(ContextMenu));
			ContextMenu.ClosedEvent = PopupControlService.ContextMenuClosedEvent.AddOwner(typeof(ContextMenu));
			ContextMenu.InsideContextMenuProperty = MenuItem.InsideContextMenuProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			EventManager.RegisterClassHandler(typeof(ContextMenu), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(ContextMenu.OnAccessKeyPressed));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(typeof(ContextMenu)));
			ContextMenu._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ContextMenu));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(null));
		}

		// Token: 0x06006190 RID: 24976 RVA: 0x0029D64C File Offset: 0x0029C64C
		public ContextMenu()
		{
			this.Initialize();
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x0029D65A File Offset: 0x0029C65A
		private static object CoerceHorizontalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.HorizontalOffsetProperty);
		}

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x06006192 RID: 24978 RVA: 0x0029D668 File Offset: 0x0029C668
		// (set) Token: 0x06006193 RID: 24979 RVA: 0x0029D67A File Offset: 0x0029C67A
		[Bindable(true)]
		[TypeConverter(typeof(LengthConverter))]
		[Category("Layout")]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(ContextMenu.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(ContextMenu.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x06006194 RID: 24980 RVA: 0x0029D68D File Offset: 0x0029C68D
		private static object CoerceVerticalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.VerticalOffsetProperty);
		}

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x06006195 RID: 24981 RVA: 0x0029D69B File Offset: 0x0029C69B
		// (set) Token: 0x06006196 RID: 24982 RVA: 0x0029D6AD File Offset: 0x0029C6AD
		[TypeConverter(typeof(LengthConverter))]
		[Bindable(true)]
		[Category("Layout")]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(ContextMenu.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(ContextMenu.VerticalOffsetProperty, value);
			}
		}

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x06006197 RID: 24983 RVA: 0x0029D6C0 File Offset: 0x0029C6C0
		// (set) Token: 0x06006198 RID: 24984 RVA: 0x0029D6D2 File Offset: 0x0029C6D2
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.IsOpenProperty);
			}
			set
			{
				base.SetValue(ContextMenu.IsOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006199 RID: 24985 RVA: 0x0029D6E8 File Offset: 0x0029C6E8
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContextMenu contextMenu = (ContextMenu)d;
			if ((bool)e.NewValue)
			{
				if (contextMenu._parentPopup == null)
				{
					contextMenu.HookupParentPopup();
				}
				contextMenu._parentPopup.Unloaded += contextMenu.OnPopupUnloaded;
				contextMenu.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, KeyboardNavigation.IsKeyboardMostRecentInputDevice());
				return;
			}
			contextMenu.ClosingMenu();
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x0029D746 File Offset: 0x0029C746
		private static object CoercePlacementTarget(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.PlacementTargetProperty);
		}

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x0600619B RID: 24987 RVA: 0x0029D754 File Offset: 0x0029C754
		// (set) Token: 0x0600619C RID: 24988 RVA: 0x0029D766 File Offset: 0x0029C766
		[Bindable(true)]
		[Category("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UIElement PlacementTarget
		{
			get
			{
				return (UIElement)base.GetValue(ContextMenu.PlacementTargetProperty);
			}
			set
			{
				base.SetValue(ContextMenu.PlacementTargetProperty, value);
			}
		}

		// Token: 0x0600619D RID: 24989 RVA: 0x0029D774 File Offset: 0x0029C774
		private static object CoercePlacementRectangle(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.PlacementRectangleProperty);
		}

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x0600619E RID: 24990 RVA: 0x0029D782 File Offset: 0x0029C782
		// (set) Token: 0x0600619F RID: 24991 RVA: 0x0029D794 File Offset: 0x0029C794
		[Bindable(true)]
		[Category("Layout")]
		public Rect PlacementRectangle
		{
			get
			{
				return (Rect)base.GetValue(ContextMenu.PlacementRectangleProperty);
			}
			set
			{
				base.SetValue(ContextMenu.PlacementRectangleProperty, value);
			}
		}

		// Token: 0x060061A0 RID: 24992 RVA: 0x0029D7A7 File Offset: 0x0029C7A7
		private static object CoercePlacement(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.PlacementProperty);
		}

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x060061A1 RID: 24993 RVA: 0x0029D7B5 File Offset: 0x0029C7B5
		// (set) Token: 0x060061A2 RID: 24994 RVA: 0x0029D7C7 File Offset: 0x0029C7C7
		[Bindable(true)]
		[Category("Layout")]
		public PlacementMode Placement
		{
			get
			{
				return (PlacementMode)base.GetValue(ContextMenu.PlacementProperty);
			}
			set
			{
				base.SetValue(ContextMenu.PlacementProperty, value);
			}
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x0029D7DC File Offset: 0x0029C7DC
		private static object CoerceHasDropShadow(DependencyObject d, object value)
		{
			ContextMenu contextMenu = (ContextMenu)d;
			if (contextMenu._parentPopup == null || !contextMenu._parentPopup.AllowsTransparency || !SystemParameters.DropShadow)
			{
				return BooleanBoxes.FalseBox;
			}
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.HasDropShadowProperty);
		}

		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x060061A4 RID: 24996 RVA: 0x0029D81E File Offset: 0x0029C81E
		// (set) Token: 0x060061A5 RID: 24997 RVA: 0x0029D830 File Offset: 0x0029C830
		public bool HasDropShadow
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.HasDropShadowProperty);
			}
			set
			{
				base.SetValue(ContextMenu.HasDropShadowProperty, value);
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x060061A6 RID: 24998 RVA: 0x0029D83E File Offset: 0x0029C83E
		// (set) Token: 0x060061A7 RID: 24999 RVA: 0x0029D850 File Offset: 0x0029C850
		[Bindable(false)]
		[Category("Layout")]
		public CustomPopupPlacementCallback CustomPopupPlacementCallback
		{
			get
			{
				return (CustomPopupPlacementCallback)base.GetValue(ContextMenu.CustomPopupPlacementCallbackProperty);
			}
			set
			{
				base.SetValue(ContextMenu.CustomPopupPlacementCallbackProperty, value);
			}
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x060061A8 RID: 25000 RVA: 0x0029D85E File Offset: 0x0029C85E
		// (set) Token: 0x060061A9 RID: 25001 RVA: 0x0029D870 File Offset: 0x0029C870
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpen
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.StaysOpenProperty);
			}
			set
			{
				base.SetValue(ContextMenu.StaysOpenProperty, value);
			}
		}

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x060061AA RID: 25002 RVA: 0x0029D87E File Offset: 0x0029C87E
		// (remove) Token: 0x060061AB RID: 25003 RVA: 0x0029D88C File Offset: 0x0029C88C
		public event RoutedEventHandler Opened
		{
			add
			{
				base.AddHandler(ContextMenu.OpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ContextMenu.OpenedEvent, value);
			}
		}

		// Token: 0x060061AC RID: 25004 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnOpened(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x060061AD RID: 25005 RVA: 0x0029D89A File Offset: 0x0029C89A
		// (remove) Token: 0x060061AE RID: 25006 RVA: 0x0029D8A8 File Offset: 0x0029C8A8
		public event RoutedEventHandler Closed
		{
			add
			{
				base.AddHandler(ContextMenu.ClosedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ContextMenu.ClosedEvent, value);
			}
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnClosed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x0029D8B6 File Offset: 0x0029C8B6
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ContextMenuAutomationPeer(this);
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x0029D8BE File Offset: 0x0029C8BE
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			MenuItem.PrepareMenuItem(element, item);
		}

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x060061B2 RID: 25010 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x0029D8D0 File Offset: 0x0029C8D0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled || !this.IsOpen)
			{
				return;
			}
			Key key = e.Key;
			if (key != Key.Up)
			{
				if (key == Key.Down && base.CurrentSelection == null)
				{
					base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
					e.Handled = true;
					return;
				}
			}
			else if (base.CurrentSelection == null)
			{
				base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
				e.Handled = true;
			}
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x0029D951 File Offset: 0x0029C951
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (!e.Handled && this.IsOpen && e.Key == Key.Apps)
			{
				base.KeyboardLeaveMenuMode();
				e.Handled = true;
			}
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x0029D981 File Offset: 0x0029C981
		private void Initialize()
		{
			MenuItem.SetInsideContextMenuProperty(this, true);
			base.InternalMenuModeChanged += this.OnIsMenuModeChanged;
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x0029D99C File Offset: 0x0029C99C
		private void HookupParentPopup()
		{
			this._parentPopup = new Popup();
			this._parentPopup.AllowsTransparency = true;
			base.CoerceValue(ContextMenu.HasDropShadowProperty);
			this._parentPopup.DropOpposite = false;
			this._parentPopup.Opened += this.OnPopupOpened;
			this._parentPopup.Closed += this.OnPopupClosed;
			this._parentPopup.PopupCouldClose += this.OnPopupCouldClose;
			this._parentPopup.SetResourceReference(Popup.PopupAnimationProperty, SystemParameters.MenuPopupAnimationKey);
			Popup.CreateRootPopup(this._parentPopup, this);
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x0029DA3D File Offset: 0x0029CA3D
		private void OnPopupCouldClose(object sender, EventArgs e)
		{
			base.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x0029DA50 File Offset: 0x0029CA50
		private void OnPopupOpened(object source, EventArgs e)
		{
			if (base.CurrentSelection != null)
			{
				base.CurrentSelection = null;
			}
			base.IsMenuMode = true;
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				base.IgnoreNextLeftRelease = true;
			}
			if (Mouse.RightButton == MouseButtonState.Pressed)
			{
				base.IgnoreNextRightRelease = true;
			}
			this.OnOpened(new RoutedEventArgs(ContextMenu.OpenedEvent, this));
		}

		// Token: 0x060061B9 RID: 25017 RVA: 0x0029DAA2 File Offset: 0x0029CAA2
		private void OnPopupClosed(object source, EventArgs e)
		{
			base.IgnoreNextLeftRelease = false;
			base.IgnoreNextRightRelease = false;
			base.IsMenuMode = false;
			this.OnClosed(new RoutedEventArgs(ContextMenu.ClosedEvent, this));
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x0029DACC File Offset: 0x0029CACC
		private void ClosingMenu()
		{
			if (this._parentPopup != null)
			{
				this._parentPopup.Unloaded -= this.OnPopupUnloaded;
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object arg)
				{
					ContextMenu contextMenu = (ContextMenu)arg;
					if (!contextMenu.IsOpen)
					{
						FocusManager.SetFocusedElement(contextMenu, null);
					}
					return null;
				}), this);
			}
		}

		// Token: 0x060061BB RID: 25019 RVA: 0x0029DB26 File Offset: 0x0029CB26
		private void OnPopupUnloaded(object sender, RoutedEventArgs e)
		{
			if (this.IsOpen)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object arg)
				{
					ContextMenu contextMenu = (ContextMenu)arg;
					if (contextMenu.IsOpen)
					{
						contextMenu.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.FalseBox);
					}
					return null;
				}), this);
			}
		}

		// Token: 0x060061BC RID: 25020 RVA: 0x0029DB60 File Offset: 0x0029CB60
		private void OnIsMenuModeChanged(object sender, EventArgs e)
		{
			if (base.IsMenuMode)
			{
				if (Keyboard.FocusedElement != null)
				{
					this._weakRefToPreviousFocus = new WeakReference<IInputElement>(Keyboard.FocusedElement);
				}
				base.Focus();
				return;
			}
			base.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.FalseBox);
			if (this._weakRefToPreviousFocus != null)
			{
				IInputElement inputElement;
				if (this._weakRefToPreviousFocus.TryGetTarget(out inputElement))
				{
					inputElement.Focus();
				}
				this._weakRefToPreviousFocus = null;
			}
		}

		// Token: 0x060061BD RID: 25021 RVA: 0x0029DBC9 File Offset: 0x0029CBC9
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			if (!(bool)e.NewValue)
			{
				this._weakRefToPreviousFocus = null;
			}
			base.OnIsKeyboardFocusWithinChanged(e);
		}

		// Token: 0x060061BE RID: 25022 RVA: 0x0029DBE7 File Offset: 0x0029CBE7
		internal override bool IgnoreModelParentBuildRoute(RoutedEventArgs e)
		{
			return e is KeyEventArgs || e is FindToolTipEventArgs;
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x0029DBFC File Offset: 0x0029CBFC
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			e.Scope = sender;
			e.Handled = true;
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x0029DC0C File Offset: 0x0029CC0C
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ContextMenu"
				}));
			}
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x0029DC41 File Offset: 0x0029CC41
		internal override void OnAncestorChanged()
		{
			base.OnAncestorChanged();
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ContextMenu"
				}));
			}
		}

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x060061C2 RID: 25026 RVA: 0x0029DC75 File Offset: 0x0029CC75
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ContextMenu._dType;
			}
		}

		// Token: 0x0400327F RID: 12927
		public static readonly DependencyProperty HorizontalOffsetProperty = ContextMenuService.HorizontalOffsetProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoerceHorizontalOffset)));

		// Token: 0x04003280 RID: 12928
		public static readonly DependencyProperty VerticalOffsetProperty = ContextMenuService.VerticalOffsetProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoerceVerticalOffset)));

		// Token: 0x04003281 RID: 12929
		public static readonly DependencyProperty IsOpenProperty = Popup.IsOpenProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ContextMenu.OnIsOpenChanged)));

		// Token: 0x04003282 RID: 12930
		public static readonly DependencyProperty PlacementTargetProperty = ContextMenuService.PlacementTargetProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoercePlacementTarget)));

		// Token: 0x04003283 RID: 12931
		public static readonly DependencyProperty PlacementRectangleProperty = ContextMenuService.PlacementRectangleProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoercePlacementRectangle)));

		// Token: 0x04003284 RID: 12932
		public static readonly DependencyProperty PlacementProperty = ContextMenuService.PlacementProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoercePlacement)));

		// Token: 0x04003285 RID: 12933
		public static readonly DependencyProperty HasDropShadowProperty = ContextMenuService.HasDropShadowProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoerceHasDropShadow)));

		// Token: 0x04003286 RID: 12934
		public static readonly DependencyProperty CustomPopupPlacementCallbackProperty = Popup.CustomPopupPlacementCallbackProperty.AddOwner(typeof(ContextMenu));

		// Token: 0x04003287 RID: 12935
		public static readonly DependencyProperty StaysOpenProperty = Popup.StaysOpenProperty.AddOwner(typeof(ContextMenu));

		// Token: 0x0400328A RID: 12938
		private static readonly DependencyProperty InsideContextMenuProperty;

		// Token: 0x0400328B RID: 12939
		private Popup _parentPopup;

		// Token: 0x0400328C RID: 12940
		private WeakReference<IInputElement> _weakRefToPreviousFocus;

		// Token: 0x0400328D RID: 12941
		private static DependencyObjectType _dType;
	}
}
