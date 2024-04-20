using System;
using System.Collections.Specialized;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Win32;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000845 RID: 2117
	[Localizability(LocalizationCategory.Menu)]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
	public abstract class MenuBase : ItemsControl
	{
		// Token: 0x06007BD8 RID: 31704 RVA: 0x0030C8D0 File Offset: 0x0030B8D0
		static MenuBase()
		{
			EventManager.RegisterClassHandler(typeof(MenuBase), MenuItem.PreviewClickEvent, new RoutedEventHandler(MenuBase.OnMenuItemPreviewClick));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseDownEvent, new MouseButtonEventHandler(MenuBase.OnMouseButtonDown));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseUpEvent, new MouseButtonEventHandler(MenuBase.OnMouseButtonUp));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.LostMouseCaptureEvent, new MouseEventHandler(MenuBase.OnLostMouseCapture));
			EventManager.RegisterClassHandler(typeof(MenuBase), MenuBase.IsSelectedChangedEvent, new RoutedPropertyChangedEventHandler<bool>(MenuBase.OnIsSelectedChanged));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseDownEvent, new MouseButtonEventHandler(MenuBase.OnPromotedMouseButton));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseUpEvent, new MouseButtonEventHandler(MenuBase.OnPromotedMouseButton));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(MenuBase.OnClickThroughThunk));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.PreviewMouseUpOutsideCapturedElementEvent, new MouseButtonEventHandler(MenuBase.OnClickThroughThunk));
			EventManager.RegisterClassHandler(typeof(MenuBase), Keyboard.PreviewKeyboardInputProviderAcquireFocusEvent, new KeyboardInputProviderAcquireFocusEventHandler(MenuBase.OnPreviewKeyboardInputProviderAcquireFocus), true);
			EventManager.RegisterClassHandler(typeof(MenuBase), Keyboard.KeyboardInputProviderAcquireFocusEvent, new KeyboardInputProviderAcquireFocusEventHandler(MenuBase.OnKeyboardInputProviderAcquireFocus), true);
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(MenuBase), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			InputMethod.IsInputMethodSuspendedProperty.OverrideMetadata(typeof(MenuBase), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
		}

		// Token: 0x17001CA4 RID: 7332
		// (get) Token: 0x06007BD9 RID: 31705 RVA: 0x0030CAFB File Offset: 0x0030BAFB
		// (set) Token: 0x06007BDA RID: 31706 RVA: 0x0030CB0D File Offset: 0x0030BB0D
		public ItemContainerTemplateSelector ItemContainerTemplateSelector
		{
			get
			{
				return (ItemContainerTemplateSelector)base.GetValue(MenuBase.ItemContainerTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(MenuBase.ItemContainerTemplateSelectorProperty, value);
			}
		}

		// Token: 0x17001CA5 RID: 7333
		// (get) Token: 0x06007BDB RID: 31707 RVA: 0x0030CB1B File Offset: 0x0030BB1B
		// (set) Token: 0x06007BDC RID: 31708 RVA: 0x0030CB2D File Offset: 0x0030BB2D
		public bool UsesItemContainerTemplate
		{
			get
			{
				return (bool)base.GetValue(MenuBase.UsesItemContainerTemplateProperty);
			}
			set
			{
				base.SetValue(MenuBase.UsesItemContainerTemplateProperty, value);
			}
		}

		// Token: 0x06007BDD RID: 31709 RVA: 0x0030CB3B File Offset: 0x0030BB3B
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			((MenuBase)sender).HandleMouseButton(e);
		}

		// Token: 0x06007BDE RID: 31710 RVA: 0x0030CB3B File Offset: 0x0030BB3B
		private static void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			((MenuBase)sender).HandleMouseButton(e);
		}

		// Token: 0x06007BDF RID: 31711 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void HandleMouseButton(MouseButtonEventArgs e)
		{
		}

		// Token: 0x06007BE0 RID: 31712 RVA: 0x0030CB49 File Offset: 0x0030BB49
		private static void OnClickThroughThunk(object sender, MouseButtonEventArgs e)
		{
			((MenuBase)sender).OnClickThrough(e);
		}

		// Token: 0x06007BE1 RID: 31713 RVA: 0x0030CB58 File Offset: 0x0030BB58
		private void OnClickThrough(MouseButtonEventArgs e)
		{
			if ((e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right) && this.HasCapture)
			{
				bool flag = true;
				if (e.ButtonState == MouseButtonState.Released)
				{
					if (e.ChangedButton == MouseButton.Left && this.IgnoreNextLeftRelease)
					{
						this.IgnoreNextLeftRelease = false;
						flag = false;
					}
					else if (e.ChangedButton == MouseButton.Right && this.IgnoreNextRightRelease)
					{
						this.IgnoreNextRightRelease = false;
						flag = false;
					}
				}
				if (flag)
				{
					this.IsMenuMode = false;
				}
			}
		}

		// Token: 0x06007BE2 RID: 31714 RVA: 0x0030CBC7 File Offset: 0x0030BBC7
		private static void OnPromotedMouseButton(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06007BE3 RID: 31715 RVA: 0x0030CBD8 File Offset: 0x0030BBD8
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (!this.HasCapture && !base.IsMouseOver && this.CurrentSelection != null && !this.CurrentSelection.IsKeyboardFocused && !this.CurrentSelection.IsSubmenuOpen)
			{
				this.CurrentSelection = null;
			}
		}

		// Token: 0x06007BE4 RID: 31716 RVA: 0x0030CC28 File Offset: 0x0030BC28
		private static void OnPreviewKeyboardInputProviderAcquireFocus(object sender, KeyboardInputProviderAcquireFocusEventArgs e)
		{
			MenuBase menuBase = (MenuBase)sender;
			if (!menuBase.IsKeyboardFocusWithin && !menuBase.HasPushedMenuMode)
			{
				menuBase.PushMenuMode(true);
			}
		}

		// Token: 0x06007BE5 RID: 31717 RVA: 0x0030CC54 File Offset: 0x0030BC54
		private static void OnKeyboardInputProviderAcquireFocus(object sender, KeyboardInputProviderAcquireFocusEventArgs e)
		{
			MenuBase menuBase = (MenuBase)sender;
			if (!menuBase.IsKeyboardFocusWithin && !e.FocusAcquired && menuBase.IsAcquireFocusMenuMode)
			{
				menuBase.PopMenuMode();
			}
		}

		// Token: 0x06007BE6 RID: 31718 RVA: 0x0030CC88 File Offset: 0x0030BC88
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (base.IsKeyboardFocusWithin)
			{
				if (!this.IsMenuMode)
				{
					this.IsMenuMode = true;
					this.OpenOnMouseEnter = false;
				}
				if (KeyboardNavigation.IsKeyboardMostRecentInputDevice())
				{
					KeyboardNavigation.EnableKeyboardCues(this, true);
				}
			}
			else
			{
				KeyboardNavigation.EnableKeyboardCues(this, false);
				if (this.IsMenuMode)
				{
					if (this.HasCapture)
					{
						this.IsMenuMode = false;
					}
				}
				else if (this.CurrentSelection != null)
				{
					this.CurrentSelection = null;
				}
			}
			this.InvokeMenuOpenedClosedAutomationEvent(base.IsKeyboardFocusWithin);
		}

		// Token: 0x06007BE7 RID: 31719 RVA: 0x0030CD08 File Offset: 0x0030BD08
		private void InvokeMenuOpenedClosedAutomationEvent(bool open)
		{
			AutomationEvents automationEvent = open ? AutomationEvents.MenuOpened : AutomationEvents.MenuClosed;
			if (AutomationPeer.ListenerExists(automationEvent))
			{
				AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (peer != null)
				{
					if (open)
					{
						base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
						{
							peer.RaiseAutomationEvent(automationEvent);
							return null;
						}), null);
						return;
					}
					peer.RaiseAutomationEvent(automationEvent);
				}
			}
		}

		// Token: 0x06007BE8 RID: 31720 RVA: 0x0030CD78 File Offset: 0x0030BD78
		private static void OnIsSelectedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
		{
			MenuItem menuItem = e.OriginalSource as MenuItem;
			if (menuItem != null)
			{
				MenuBase menuBase = (MenuBase)sender;
				if (e.NewValue)
				{
					if (menuBase.CurrentSelection != menuItem && menuItem.LogicalParent == menuBase)
					{
						bool flag = false;
						if (menuBase.CurrentSelection != null)
						{
							flag = menuBase.CurrentSelection.IsSubmenuOpen;
							menuBase.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
						}
						menuBase.CurrentSelection = menuItem;
						if (menuBase.CurrentSelection != null && flag)
						{
							MenuItemRole role = menuBase.CurrentSelection.Role;
							if ((role == MenuItemRole.SubmenuHeader || role == MenuItemRole.TopLevelHeader) && menuBase.CurrentSelection.IsSubmenuOpen != flag)
							{
								menuBase.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.Box(flag));
							}
						}
					}
				}
				else if (menuBase.CurrentSelection == menuItem)
				{
					menuBase.CurrentSelection = null;
				}
				e.Handled = true;
			}
		}

		// Token: 0x06007BE9 RID: 31721 RVA: 0x0030CE51 File Offset: 0x0030BE51
		private bool IsDescendant(DependencyObject node)
		{
			return MenuBase.IsDescendant(this, node);
		}

		// Token: 0x06007BEA RID: 31722 RVA: 0x0030CE5C File Offset: 0x0030BE5C
		internal static bool IsDescendant(DependencyObject reference, DependencyObject node)
		{
			bool result = false;
			DependencyObject dependencyObject = node;
			while (dependencyObject != null)
			{
				if (dependencyObject == reference)
				{
					result = true;
					break;
				}
				PopupRoot popupRoot = dependencyObject as PopupRoot;
				if (popupRoot != null)
				{
					Popup popup = popupRoot.Parent as Popup;
					dependencyObject = popup;
					if (popup != null)
					{
						dependencyObject = popup.Parent;
						if (dependencyObject == null)
						{
							dependencyObject = popup.PlacementTarget;
						}
					}
				}
				else
				{
					dependencyObject = PopupControlService.FindParent(dependencyObject);
				}
			}
			return result;
		}

		// Token: 0x06007BEB RID: 31723 RVA: 0x0030CEB0 File Offset: 0x0030BEB0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			Key key = e.Key;
			if (key != Key.Escape)
			{
				if (key != Key.System)
				{
					return;
				}
				if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F10)
				{
					this.KeyboardLeaveMenuMode();
					e.Handled = true;
				}
				return;
			}
			else
			{
				if (this.CurrentSelection != null && this.CurrentSelection.IsSubmenuOpen)
				{
					this.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
					this.OpenOnMouseEnter = false;
					e.Handled = true;
					return;
				}
				this.KeyboardLeaveMenuMode();
				e.Handled = true;
				return;
			}
		}

		// Token: 0x06007BEC RID: 31724 RVA: 0x0030CF4B File Offset: 0x0030BF4B
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			bool flag = item is MenuItem || item is Separator;
			if (!flag)
			{
				this._currentItem = item;
			}
			return flag;
		}

		// Token: 0x06007BED RID: 31725 RVA: 0x0030CF6C File Offset: 0x0030BF6C
		protected override DependencyObject GetContainerForItemOverride()
		{
			object currentItem = this._currentItem;
			this._currentItem = null;
			if (this.UsesItemContainerTemplate)
			{
				DataTemplate dataTemplate = this.ItemContainerTemplateSelector.SelectTemplate(currentItem, this);
				if (dataTemplate != null)
				{
					object obj = dataTemplate.LoadContent();
					if (obj is MenuItem || obj is Separator)
					{
						return obj as DependencyObject;
					}
					throw new InvalidOperationException(SR.Get("InvalidItemContainer", new object[]
					{
						base.GetType().Name,
						typeof(MenuItem).Name,
						typeof(Separator).Name,
						obj
					}));
				}
			}
			return new MenuItem();
		}

		// Token: 0x06007BEE RID: 31726 RVA: 0x0030D010 File Offset: 0x0030C010
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			MenuBase menuBase = sender as MenuBase;
			if (Mouse.Captured != menuBase)
			{
				if (e.OriginalSource == menuBase)
				{
					if (Mouse.Captured == null || !MenuBase.IsDescendant(menuBase, Mouse.Captured as DependencyObject))
					{
						menuBase.IsMenuMode = false;
						return;
					}
				}
				else if (MenuBase.IsDescendant(menuBase, e.OriginalSource as DependencyObject))
				{
					if (menuBase.IsMenuMode && Mouse.Captured == null && SafeNativeMethods.GetCapture() == IntPtr.Zero)
					{
						Mouse.Capture(menuBase, CaptureMode.SubTree);
						e.Handled = true;
						return;
					}
				}
				else
				{
					menuBase.IsMenuMode = false;
				}
			}
		}

		// Token: 0x06007BEF RID: 31727 RVA: 0x0030D0A0 File Offset: 0x0030C0A0
		private static void OnMenuItemPreviewClick(object sender, RoutedEventArgs e)
		{
			MenuBase menuBase = (MenuBase)sender;
			MenuItem menuItem = e.OriginalSource as MenuItem;
			if (menuItem != null && !menuItem.StaysOpenOnClick)
			{
				MenuItemRole role = menuItem.Role;
				if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem)
				{
					menuBase.IsMenuMode = false;
					e.Handled = true;
				}
			}
		}

		// Token: 0x14000155 RID: 341
		// (add) Token: 0x06007BF0 RID: 31728 RVA: 0x0030D0E7 File Offset: 0x0030C0E7
		// (remove) Token: 0x06007BF1 RID: 31729 RVA: 0x0030D0F5 File Offset: 0x0030C0F5
		internal event EventHandler InternalMenuModeChanged
		{
			add
			{
				base.EventHandlersStoreAdd(MenuBase.InternalMenuModeChangedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(MenuBase.InternalMenuModeChangedKey, value);
			}
		}

		// Token: 0x06007BF2 RID: 31730 RVA: 0x0030D104 File Offset: 0x0030C104
		private void RestorePreviousFocus()
		{
			if (base.IsKeyboardFocusWithin)
			{
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				if (((focus != IntPtr.Zero) ? HwndSource.CriticalFromHwnd(focus) : null) != null)
				{
					Keyboard.Focus(null);
					return;
				}
				Keyboard.ClearFocus();
			}
		}

		// Token: 0x06007BF3 RID: 31731 RVA: 0x0030D144 File Offset: 0x0030C144
		internal static void SetSuspendingPopupAnimation(ItemsControl menu, MenuItem ignore, bool suspend)
		{
			if (menu != null)
			{
				int count = menu.Items.Count;
				for (int i = 0; i < count; i++)
				{
					MenuItem menuItem = menu.ItemContainerGenerator.ContainerFromIndex(i) as MenuItem;
					if (menuItem != null && menuItem != ignore && menuItem.IsSuspendingPopupAnimation != suspend)
					{
						menuItem.IsSuspendingPopupAnimation = suspend;
						if (!suspend)
						{
							MenuBase.SetSuspendingPopupAnimation(menuItem, null, suspend);
						}
					}
				}
			}
		}

		// Token: 0x06007BF4 RID: 31732 RVA: 0x0030D1A0 File Offset: 0x0030C1A0
		internal void KeyboardLeaveMenuMode()
		{
			if (this.IsMenuMode)
			{
				this.IsMenuMode = false;
				return;
			}
			this.CurrentSelection = null;
			this.RestorePreviousFocus();
		}

		// Token: 0x17001CA6 RID: 7334
		// (get) Token: 0x06007BF5 RID: 31733 RVA: 0x0030D1BF File Offset: 0x0030C1BF
		// (set) Token: 0x06007BF6 RID: 31734 RVA: 0x0030D1C8 File Offset: 0x0030C1C8
		internal MenuItem CurrentSelection
		{
			get
			{
				return this._currentSelection;
			}
			set
			{
				bool flag = false;
				if (this._currentSelection != null)
				{
					flag = this._currentSelection.IsKeyboardFocused;
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
				}
				this._currentSelection = value;
				if (this._currentSelection != null)
				{
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
					if (flag)
					{
						this._currentSelection.Focus();
					}
				}
			}
		}

		// Token: 0x17001CA7 RID: 7335
		// (get) Token: 0x06007BF7 RID: 31735 RVA: 0x0029C100 File Offset: 0x0029B100
		internal bool HasCapture
		{
			get
			{
				return Mouse.Captured == this;
			}
		}

		// Token: 0x17001CA8 RID: 7336
		// (get) Token: 0x06007BF8 RID: 31736 RVA: 0x0030D233 File Offset: 0x0030C233
		// (set) Token: 0x06007BF9 RID: 31737 RVA: 0x0030D241 File Offset: 0x0030C241
		internal bool IgnoreNextLeftRelease
		{
			get
			{
				return this._bitFlags[1];
			}
			set
			{
				this._bitFlags[1] = value;
			}
		}

		// Token: 0x17001CA9 RID: 7337
		// (get) Token: 0x06007BFA RID: 31738 RVA: 0x0030D250 File Offset: 0x0030C250
		// (set) Token: 0x06007BFB RID: 31739 RVA: 0x0030D25E File Offset: 0x0030C25E
		internal bool IgnoreNextRightRelease
		{
			get
			{
				return this._bitFlags[2];
			}
			set
			{
				this._bitFlags[2] = value;
			}
		}

		// Token: 0x17001CAA RID: 7338
		// (get) Token: 0x06007BFC RID: 31740 RVA: 0x0030D26D File Offset: 0x0030C26D
		// (set) Token: 0x06007BFD RID: 31741 RVA: 0x0030D27C File Offset: 0x0030C27C
		internal bool IsMenuMode
		{
			get
			{
				return this._bitFlags[4];
			}
			set
			{
				bool flag = this._bitFlags[4];
				if (flag != value)
				{
					this._bitFlags[4] = value;
					flag = value;
					if (flag)
					{
						if (!MenuBase.IsDescendant(this, Mouse.Captured as Visual) && !Mouse.Capture(this, CaptureMode.SubTree))
						{
							flag = (this._bitFlags[4] = false);
						}
						else
						{
							if (!this.HasPushedMenuMode)
							{
								this.PushMenuMode(false);
							}
							base.RaiseClrEvent(MenuBase.InternalMenuModeChangedKey, EventArgs.Empty);
						}
					}
					if (!flag)
					{
						if (this.CurrentSelection != null)
						{
							bool isSubmenuOpen = this.CurrentSelection.IsSubmenuOpen;
							this.CurrentSelection.IsSubmenuOpen = false;
							this.CurrentSelection = null;
						}
						if (this.HasPushedMenuMode)
						{
							this.PopMenuMode();
						}
						if (!value)
						{
							base.RaiseClrEvent(MenuBase.InternalMenuModeChangedKey, EventArgs.Empty);
						}
						MenuBase.SetSuspendingPopupAnimation(this, null, false);
						if (this.HasCapture)
						{
							Mouse.Capture(null);
						}
						this.RestorePreviousFocus();
					}
					this.OpenOnMouseEnter = flag;
				}
			}
		}

		// Token: 0x17001CAB RID: 7339
		// (get) Token: 0x06007BFE RID: 31742 RVA: 0x0030D36D File Offset: 0x0030C36D
		// (set) Token: 0x06007BFF RID: 31743 RVA: 0x0030D37B File Offset: 0x0030C37B
		internal bool OpenOnMouseEnter
		{
			get
			{
				return this._bitFlags[8];
			}
			set
			{
				this._bitFlags[8] = value;
			}
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x0030D38A File Offset: 0x0030C38A
		private void PushMenuMode(bool isAcquireFocusMenuMode)
		{
			this._pushedMenuMode = PresentationSource.CriticalFromVisual(this);
			this.IsAcquireFocusMenuMode = isAcquireFocusMenuMode;
			InputManager.UnsecureCurrent.PushMenuMode(this._pushedMenuMode);
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x0030D3B0 File Offset: 0x0030C3B0
		private void PopMenuMode()
		{
			PresentationSource pushedMenuMode = this._pushedMenuMode;
			this._pushedMenuMode = null;
			this.IsAcquireFocusMenuMode = false;
			InputManager.UnsecureCurrent.PopMenuMode(pushedMenuMode);
		}

		// Token: 0x17001CAC RID: 7340
		// (get) Token: 0x06007C02 RID: 31746 RVA: 0x0030D3DD File Offset: 0x0030C3DD
		private bool HasPushedMenuMode
		{
			get
			{
				return this._pushedMenuMode != null;
			}
		}

		// Token: 0x17001CAD RID: 7341
		// (get) Token: 0x06007C03 RID: 31747 RVA: 0x0030D3E8 File Offset: 0x0030C3E8
		// (set) Token: 0x06007C04 RID: 31748 RVA: 0x0030D3F7 File Offset: 0x0030C3F7
		private bool IsAcquireFocusMenuMode
		{
			get
			{
				return this._bitFlags[16];
			}
			set
			{
				this._bitFlags[16] = value;
			}
		}

		// Token: 0x04003A5C RID: 14940
		public static readonly DependencyProperty ItemContainerTemplateSelectorProperty = DependencyProperty.Register("ItemContainerTemplateSelector", typeof(ItemContainerTemplateSelector), typeof(MenuBase), new FrameworkPropertyMetadata(new DefaultItemContainerTemplateSelector()));

		// Token: 0x04003A5D RID: 14941
		public static readonly DependencyProperty UsesItemContainerTemplateProperty = DependencyProperty.Register("UsesItemContainerTemplate", typeof(bool), typeof(MenuBase));

		// Token: 0x04003A5E RID: 14942
		internal static readonly RoutedEvent IsSelectedChangedEvent = EventManager.RegisterRoutedEvent("IsSelectedChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(MenuBase));

		// Token: 0x04003A5F RID: 14943
		private object _currentItem;

		// Token: 0x04003A60 RID: 14944
		private static readonly EventPrivateKey InternalMenuModeChangedKey = new EventPrivateKey();

		// Token: 0x04003A61 RID: 14945
		private PresentationSource _pushedMenuMode;

		// Token: 0x04003A62 RID: 14946
		private MenuItem _currentSelection;

		// Token: 0x04003A63 RID: 14947
		private BitVector32 _bitFlags = new BitVector32(0);

		// Token: 0x02000C49 RID: 3145
		private enum MenuBaseFlags
		{
			// Token: 0x04004C2C RID: 19500
			IgnoreNextLeftRelease = 1,
			// Token: 0x04004C2D RID: 19501
			IgnoreNextRightRelease,
			// Token: 0x04004C2E RID: 19502
			IsMenuMode = 4,
			// Token: 0x04004C2F RID: 19503
			OpenOnMouseEnter = 8,
			// Token: 0x04004C30 RID: 19504
			IsAcquireFocusMenuMode = 16
		}
	}
}
