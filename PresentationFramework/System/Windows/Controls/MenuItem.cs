using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007AF RID: 1967
	[DefaultEvent("Click")]
	[Localizability(LocalizationCategory.Menu)]
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
	public class MenuItem : HeaderedItemsControl, ICommandSource
	{
		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x06006F38 RID: 28472 RVA: 0x002D40F8 File Offset: 0x002D30F8
		public static ResourceKey TopLevelItemTemplateKey
		{
			get
			{
				if (MenuItem._topLevelItemTemplateKey == null)
				{
					MenuItem._topLevelItemTemplateKey = new ComponentResourceKey(typeof(MenuItem), "TopLevelItemTemplateKey");
				}
				return MenuItem._topLevelItemTemplateKey;
			}
		}

		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x06006F39 RID: 28473 RVA: 0x002D411F File Offset: 0x002D311F
		public static ResourceKey TopLevelHeaderTemplateKey
		{
			get
			{
				if (MenuItem._topLevelHeaderTemplateKey == null)
				{
					MenuItem._topLevelHeaderTemplateKey = new ComponentResourceKey(typeof(MenuItem), "TopLevelHeaderTemplateKey");
				}
				return MenuItem._topLevelHeaderTemplateKey;
			}
		}

		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x06006F3A RID: 28474 RVA: 0x002D4146 File Offset: 0x002D3146
		public static ResourceKey SubmenuItemTemplateKey
		{
			get
			{
				if (MenuItem._submenuItemTemplateKey == null)
				{
					MenuItem._submenuItemTemplateKey = new ComponentResourceKey(typeof(MenuItem), "SubmenuItemTemplateKey");
				}
				return MenuItem._submenuItemTemplateKey;
			}
		}

		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x06006F3B RID: 28475 RVA: 0x002D416D File Offset: 0x002D316D
		public static ResourceKey SubmenuHeaderTemplateKey
		{
			get
			{
				if (MenuItem._submenuHeaderTemplateKey == null)
				{
					MenuItem._submenuHeaderTemplateKey = new ComponentResourceKey(typeof(MenuItem), "SubmenuHeaderTemplateKey");
				}
				return MenuItem._submenuHeaderTemplateKey;
			}
		}

		// Token: 0x06006F3D RID: 28477 RVA: 0x002D419C File Offset: 0x002D319C
		static MenuItem()
		{
			MenuItem.ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.PreviewClickEvent = EventManager.RegisterRoutedEvent("PreviewClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.SubmenuOpenedEvent = EventManager.RegisterRoutedEvent("SubmenuOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.SubmenuClosedEvent = EventManager.RegisterRoutedEvent("SubmenuClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(MenuItem.OnCommandChanged)));
			MenuItem.CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(null));
			MenuItem.CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(null));
			MenuItem.IsSubmenuOpenProperty = DependencyProperty.Register("IsSubmenuOpen", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MenuItem.OnIsSubmenuOpenChanged), new CoerceValueCallback(MenuItem.CoerceIsSubmenuOpen)));
			MenuItem.RolePropertyKey = DependencyProperty.RegisterReadOnly("Role", typeof(MenuItemRole), typeof(MenuItem), new FrameworkPropertyMetadata(MenuItemRole.TopLevelItem));
			MenuItem.RoleProperty = MenuItem.RolePropertyKey.DependencyProperty;
			MenuItem.IsCheckableProperty = DependencyProperty.Register("IsCheckable", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(MenuItem.OnIsCheckableChanged)));
			MenuItem.IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsPressedProperty = MenuItem.IsPressedPropertyKey.DependencyProperty;
			MenuItem.IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsHighlightedProperty = MenuItem.IsHighlightedPropertyKey.DependencyProperty;
			MenuItem.IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(MenuItem.OnIsCheckedChanged)));
			MenuItem.StaysOpenOnClickProperty = DependencyProperty.Register("StaysOpenOnClick", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MenuItem.OnIsSelectedChanged)));
			MenuItem.InputGestureTextProperty = DependencyProperty.Register("InputGestureText", typeof(string), typeof(MenuItem), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(MenuItem.OnInputGestureTextChanged), new CoerceValueCallback(MenuItem.CoerceInputGestureText)));
			MenuItem.IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(MenuItem), new FrameworkPropertyMetadata(null));
			MenuItem.IsSuspendingPopupAnimationPropertyKey = DependencyProperty.RegisterReadOnly("IsSuspendingPopupAnimation", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsSuspendingPopupAnimationProperty = MenuItem.IsSuspendingPopupAnimationPropertyKey.DependencyProperty;
			MenuItem.ItemContainerTemplateSelectorProperty = MenuBase.ItemContainerTemplateSelectorProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(new DefaultItemContainerTemplateSelector()));
			MenuItem.UsesItemContainerTemplateProperty = MenuBase.UsesItemContainerTemplateProperty.AddOwner(typeof(MenuItem));
			MenuItem.InsideContextMenuProperty = DependencyProperty.RegisterAttached("InsideContextMenu", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			MenuItem.BooleanFieldStoreProperty = DependencyProperty.RegisterAttached("BooleanFieldStore", typeof(MenuItem.BoolField), typeof(MenuItem), new FrameworkPropertyMetadata((MenuItem.BoolField)0));
			HeaderedItemsControl.HeaderProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null, new CoerceValueCallback(MenuItem.CoerceHeader)));
			EventManager.RegisterClassHandler(typeof(MenuItem), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(MenuItem.OnAccessKeyPressed));
			EventManager.RegisterClassHandler(typeof(MenuItem), MenuBase.IsSelectedChangedEvent, new RoutedPropertyChangedEventHandler<bool>(MenuItem.OnIsSelectedChanged));
			Control.ForegroundProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemColors.MenuTextBrush));
			Control.FontFamilyProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily));
			Control.FontSizeProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize));
			Control.FontStyleProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle));
			Control.FontWeightProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight));
			ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null, new CoerceValueCallback(MenuItem.CoerceToolTipIsEnabled)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(typeof(MenuItem)));
			MenuItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(MenuItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null));
			InputMethod.IsInputMethodSuspendedProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x06006F3E RID: 28478 RVA: 0x002D47E4 File Offset: 0x002D37E4
		// (remove) Token: 0x06006F3F RID: 28479 RVA: 0x002D47F2 File Offset: 0x002D37F2
		[Category("Behavior")]
		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(MenuItem.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.ClickEvent, value);
			}
		}

		// Token: 0x1400013C RID: 316
		// (add) Token: 0x06006F40 RID: 28480 RVA: 0x002D4800 File Offset: 0x002D3800
		// (remove) Token: 0x06006F41 RID: 28481 RVA: 0x002D480E File Offset: 0x002D380E
		[Category("Behavior")]
		public event RoutedEventHandler Checked
		{
			add
			{
				base.AddHandler(MenuItem.CheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.CheckedEvent, value);
			}
		}

		// Token: 0x1400013D RID: 317
		// (add) Token: 0x06006F42 RID: 28482 RVA: 0x002D481C File Offset: 0x002D381C
		// (remove) Token: 0x06006F43 RID: 28483 RVA: 0x002D482A File Offset: 0x002D382A
		[Category("Behavior")]
		public event RoutedEventHandler Unchecked
		{
			add
			{
				base.AddHandler(MenuItem.UncheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.UncheckedEvent, value);
			}
		}

		// Token: 0x1400013E RID: 318
		// (add) Token: 0x06006F44 RID: 28484 RVA: 0x002D4838 File Offset: 0x002D3838
		// (remove) Token: 0x06006F45 RID: 28485 RVA: 0x002D4846 File Offset: 0x002D3846
		[Category("Behavior")]
		public event RoutedEventHandler SubmenuOpened
		{
			add
			{
				base.AddHandler(MenuItem.SubmenuOpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.SubmenuOpenedEvent, value);
			}
		}

		// Token: 0x1400013F RID: 319
		// (add) Token: 0x06006F46 RID: 28486 RVA: 0x002D4854 File Offset: 0x002D3854
		// (remove) Token: 0x06006F47 RID: 28487 RVA: 0x002D4862 File Offset: 0x002D3862
		[Category("Behavior")]
		public event RoutedEventHandler SubmenuClosed
		{
			add
			{
				base.AddHandler(MenuItem.SubmenuClosedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.SubmenuClosedEvent, value);
			}
		}

		// Token: 0x06006F48 RID: 28488 RVA: 0x002D4870 File Offset: 0x002D3870
		private static object CoerceHeader(DependencyObject d, object value)
		{
			MenuItem menuItem = (MenuItem)d;
			RoutedUICommand routedUICommand;
			if (value == null && !menuItem.HasNonDefaultValue(HeaderedItemsControl.HeaderProperty))
			{
				routedUICommand = (menuItem.Command as RoutedUICommand);
				if (routedUICommand != null)
				{
					value = routedUICommand.Text;
				}
				return value;
			}
			routedUICommand = (value as RoutedUICommand);
			if (routedUICommand != null)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(menuItem);
				if (itemsControl != null && itemsControl.ItemContainerGenerator.ItemFromContainer(menuItem) == value)
				{
					return routedUICommand.Text;
				}
			}
			return value;
		}

		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x06006F49 RID: 28489 RVA: 0x002D48D8 File Offset: 0x002D38D8
		// (set) Token: 0x06006F4A RID: 28490 RVA: 0x002D48EA File Offset: 0x002D38EA
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(MenuItem.CommandProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandProperty, value);
			}
		}

		// Token: 0x06006F4B RID: 28491 RVA: 0x002D48F8 File Offset: 0x002D38F8
		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((MenuItem)d).OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		// Token: 0x06006F4C RID: 28492 RVA: 0x002D491D File Offset: 0x002D391D
		private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				this.UnhookCommand(oldCommand);
			}
			if (newCommand != null)
			{
				this.HookCommand(newCommand);
			}
			base.CoerceValue(HeaderedItemsControl.HeaderProperty);
			base.CoerceValue(MenuItem.InputGestureTextProperty);
		}

		// Token: 0x06006F4D RID: 28493 RVA: 0x002D4949 File Offset: 0x002D3949
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06006F4E RID: 28494 RVA: 0x002D4963 File Offset: 0x002D3963
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06006F4F RID: 28495 RVA: 0x002D497D File Offset: 0x002D397D
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x06006F50 RID: 28496 RVA: 0x002D4988 File Offset: 0x002D3988
		private void UpdateCanExecute()
		{
			MenuItem.SetBoolField(this, MenuItem.BoolField.CanExecuteInvalid, false);
			if (this.Command == null)
			{
				this.CanExecute = true;
				return;
			}
			MenuItem menuItem = ItemsControl.ItemsControlFromItemContainer(this) as MenuItem;
			if (menuItem == null || menuItem.IsSubmenuOpen)
			{
				this.CanExecute = CommandHelpers.CanExecuteCommandSource(this);
				return;
			}
			this.CanExecute = true;
			MenuItem.SetBoolField(this, MenuItem.BoolField.CanExecuteInvalid, true);
		}

		// Token: 0x170019B0 RID: 6576
		// (get) Token: 0x06006F51 RID: 28497 RVA: 0x002D49E2 File Offset: 0x002D39E2
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this.CanExecute;
			}
		}

		// Token: 0x170019B1 RID: 6577
		// (get) Token: 0x06006F52 RID: 28498 RVA: 0x002D49F4 File Offset: 0x002D39F4
		// (set) Token: 0x06006F53 RID: 28499 RVA: 0x002D4A01 File Offset: 0x002D3A01
		[Localizability(LocalizationCategory.NeverLocalize)]
		[Category("Action")]
		[Bindable(true)]
		public object CommandParameter
		{
			get
			{
				return base.GetValue(MenuItem.CommandParameterProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandParameterProperty, value);
			}
		}

		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x06006F54 RID: 28500 RVA: 0x002D4A0F File Offset: 0x002D3A0F
		// (set) Token: 0x06006F55 RID: 28501 RVA: 0x002D4A21 File Offset: 0x002D3A21
		[Bindable(true)]
		[Category("Action")]
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(MenuItem.CommandTargetProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandTargetProperty, value);
			}
		}

		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x06006F56 RID: 28502 RVA: 0x002D4A2F File Offset: 0x002D3A2F
		// (set) Token: 0x06006F57 RID: 28503 RVA: 0x002D4A41 File Offset: 0x002D3A41
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSubmenuOpen
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsSubmenuOpenProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006F58 RID: 28504 RVA: 0x002D4A54 File Offset: 0x002D3A54
		private static object CoerceIsSubmenuOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				MenuItem menuItem = (MenuItem)d;
				if (!menuItem.IsLoaded)
				{
					menuItem.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x06006F59 RID: 28505 RVA: 0x002D4A85 File Offset: 0x002D3A85
		private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
		{
			if (!((MenuItem)d).IsSubmenuOpen)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x06006F5A RID: 28506 RVA: 0x002D4A9B File Offset: 0x002D3A9B
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x002D4AAF File Offset: 0x002D3AAF
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(MenuItem.IsSubmenuOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x002D4ACC File Offset: 0x002D3ACC
		private static void OnIsSubmenuOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MenuItem menuItem = (MenuItem)d;
			bool oldValue = (bool)e.OldValue;
			bool flag = (bool)e.NewValue;
			menuItem.StopTimer(ref menuItem._openHierarchyTimer);
			menuItem.StopTimer(ref menuItem._closeHierarchyTimer);
			MenuItemAutomationPeer menuItemAutomationPeer = UIElementAutomationPeer.FromElement(menuItem) as MenuItemAutomationPeer;
			if (menuItemAutomationPeer != null)
			{
				menuItemAutomationPeer.ResetChildrenCache();
				menuItemAutomationPeer.RaiseExpandCollapseAutomationEvent(oldValue, flag);
			}
			if (flag)
			{
				CommandManager.InvalidateRequerySuggested();
				menuItem.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
				if (menuItem.Role == MenuItemRole.TopLevelHeader)
				{
					menuItem.SetMenuMode(true);
				}
				menuItem.CurrentSelection = null;
				menuItem.NotifySiblingsToSuspendAnimation();
				for (int i = 0; i < menuItem.Items.Count; i++)
				{
					MenuItem menuItem2 = menuItem.ItemContainerGenerator.ContainerFromIndex(i) as MenuItem;
					if (menuItem2 != null && MenuItem.GetBoolField(menuItem2, MenuItem.BoolField.CanExecuteInvalid))
					{
						menuItem2.UpdateCanExecute();
					}
				}
				menuItem.OnSubmenuOpened(new RoutedEventArgs(MenuItem.SubmenuOpenedEvent, menuItem));
				MenuItem.SetBoolField(menuItem, MenuItem.BoolField.IgnoreMouseEvents, true);
				MenuItem.SetBoolField(menuItem, MenuItem.BoolField.MouseEnterOnMouseMove, false);
				menuItem.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(object param)
				{
					MenuItem.SetBoolField(menuItem, MenuItem.BoolField.IgnoreMouseEvents, false);
					return null;
				}), null);
			}
			else
			{
				if (menuItem.CurrentSelection != null)
				{
					if (menuItem.CurrentSelection.IsKeyboardFocusWithin)
					{
						menuItem.Focus();
					}
					if (menuItem.CurrentSelection.IsSubmenuOpen)
					{
						menuItem.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
					}
				}
				else if (menuItem.IsKeyboardFocusWithin)
				{
					menuItem.Focus();
				}
				menuItem.CurrentSelection = null;
				if (menuItem.IsMouseOver && menuItem.Role == MenuItemRole.SubmenuHeader)
				{
					MenuItem.SetBoolField(menuItem, MenuItem.BoolField.IgnoreNextMouseLeave, true);
				}
				menuItem.NotifyChildrenToResumeAnimation();
				if (menuItem._submenuPopup == null)
				{
					menuItem.OnSubmenuClosed(new RoutedEventArgs(MenuItem.SubmenuClosedEvent, menuItem));
				}
			}
			menuItem.CoerceValue(ToolTipService.IsEnabledProperty);
		}

		// Token: 0x06006F5D RID: 28509 RVA: 0x002D4D34 File Offset: 0x002D3D34
		private void OnPopupClosed(object source, EventArgs e)
		{
			this.OnSubmenuClosed(new RoutedEventArgs(MenuItem.SubmenuClosedEvent, this));
		}

		// Token: 0x06006F5E RID: 28510 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSubmenuOpened(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06006F5F RID: 28511 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSubmenuClosed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x06006F60 RID: 28512 RVA: 0x002D4D47 File Offset: 0x002D3D47
		[Category("Behavior")]
		public MenuItemRole Role
		{
			get
			{
				return (MenuItemRole)base.GetValue(MenuItem.RoleProperty);
			}
		}

		// Token: 0x06006F61 RID: 28513 RVA: 0x002D4D5C File Offset: 0x002D3D5C
		private void UpdateRole()
		{
			MenuItemRole menuItemRole;
			if (!this.IsCheckable && base.HasItems)
			{
				if (this.LogicalParent is Menu)
				{
					menuItemRole = MenuItemRole.TopLevelHeader;
				}
				else
				{
					menuItemRole = MenuItemRole.SubmenuHeader;
				}
			}
			else if (this.LogicalParent is Menu)
			{
				menuItemRole = MenuItemRole.TopLevelItem;
			}
			else
			{
				menuItemRole = MenuItemRole.SubmenuItem;
			}
			base.SetValue(MenuItem.RolePropertyKey, menuItemRole);
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x06006F62 RID: 28514 RVA: 0x002D4DB2 File Offset: 0x002D3DB2
		// (set) Token: 0x06006F63 RID: 28515 RVA: 0x002D4DC4 File Offset: 0x002D3DC4
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsCheckable
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsCheckableProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsCheckableProperty, value);
			}
		}

		// Token: 0x06006F64 RID: 28516 RVA: 0x002D4DD2 File Offset: 0x002D3DD2
		private static void OnIsCheckableChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			((MenuItem)target).UpdateRole();
		}

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x06006F65 RID: 28517 RVA: 0x002D4DDF File Offset: 0x002D3DDF
		// (set) Token: 0x06006F66 RID: 28518 RVA: 0x002D4DF1 File Offset: 0x002D3DF1
		[Browsable(false)]
		[Category("Appearance")]
		public bool IsPressed
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsPressedProperty);
			}
			protected set
			{
				base.SetValue(MenuItem.IsPressedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006F67 RID: 28519 RVA: 0x002D4E04 File Offset: 0x002D3E04
		private void UpdateIsPressed()
		{
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (Mouse.LeftButton == MouseButtonState.Pressed && base.IsMouseOver && rect.Contains(Mouse.GetPosition(this)))
			{
				this.IsPressed = true;
				return;
			}
			base.ClearValue(MenuItem.IsPressedPropertyKey);
		}

		// Token: 0x170019B7 RID: 6583
		// (get) Token: 0x06006F68 RID: 28520 RVA: 0x002D4E59 File Offset: 0x002D3E59
		// (set) Token: 0x06006F69 RID: 28521 RVA: 0x002D4E6B File Offset: 0x002D3E6B
		[Browsable(false)]
		[Category("Appearance")]
		public bool IsHighlighted
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsHighlightedProperty);
			}
			protected set
			{
				base.SetValue(MenuItem.IsHighlightedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x06006F6A RID: 28522 RVA: 0x002D4E7E File Offset: 0x002D3E7E
		// (set) Token: 0x06006F6B RID: 28523 RVA: 0x002D4E90 File Offset: 0x002D3E90
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsChecked
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsCheckedProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsCheckedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006F6C RID: 28524 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnChecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06006F6D RID: 28525 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnUnchecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06006F6E RID: 28526 RVA: 0x002D4EA4 File Offset: 0x002D3EA4
		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MenuItem menuItem = (MenuItem)d;
			if ((bool)e.NewValue)
			{
				menuItem.OnChecked(new RoutedEventArgs(MenuItem.CheckedEvent));
				return;
			}
			menuItem.OnUnchecked(new RoutedEventArgs(MenuItem.UncheckedEvent));
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x06006F6F RID: 28527 RVA: 0x002D4EE7 File Offset: 0x002D3EE7
		// (set) Token: 0x06006F70 RID: 28528 RVA: 0x002D4EF9 File Offset: 0x002D3EF9
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpenOnClick
		{
			get
			{
				return (bool)base.GetValue(MenuItem.StaysOpenOnClickProperty);
			}
			set
			{
				base.SetValue(MenuItem.StaysOpenOnClickProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x170019BA RID: 6586
		// (get) Token: 0x06006F71 RID: 28529 RVA: 0x002D4F0C File Offset: 0x002D3F0C
		// (set) Token: 0x06006F72 RID: 28530 RVA: 0x002D4F1E File Offset: 0x002D3F1E
		internal bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsSelectedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006F73 RID: 28531 RVA: 0x002D4F34 File Offset: 0x002D3F34
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MenuItem menuItem = (MenuItem)d;
			menuItem.SetValue(MenuItem.IsHighlightedPropertyKey, e.NewValue);
			if ((bool)e.OldValue)
			{
				if (menuItem.IsSubmenuOpen)
				{
					menuItem.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
				}
				menuItem.StopTimer(ref menuItem._openHierarchyTimer);
				menuItem.StopTimer(ref menuItem._closeHierarchyTimer);
			}
			menuItem.RaiseEvent(new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue, MenuBase.IsSelectedChangedEvent));
		}

		// Token: 0x06006F74 RID: 28532 RVA: 0x002D4FC0 File Offset: 0x002D3FC0
		private static void OnIsSelectedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
		{
			if (sender != e.OriginalSource)
			{
				MenuItem menuItem = (MenuItem)sender;
				MenuItem menuItem2 = e.OriginalSource as MenuItem;
				if (menuItem2 != null)
				{
					if (e.NewValue)
					{
						if (menuItem.CurrentSelection == menuItem2)
						{
							menuItem.StopTimer(ref menuItem._closeHierarchyTimer);
						}
						if (menuItem.CurrentSelection != menuItem2 && menuItem2.LogicalParent == menuItem)
						{
							if (menuItem.CurrentSelection != null && menuItem.CurrentSelection.IsSubmenuOpen)
							{
								menuItem.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
							}
							menuItem.CurrentSelection = menuItem2;
						}
					}
					else if (menuItem.CurrentSelection == menuItem2)
					{
						menuItem.CurrentSelection = null;
					}
					e.Handled = true;
				}
			}
		}

		// Token: 0x170019BB RID: 6587
		// (get) Token: 0x06006F75 RID: 28533 RVA: 0x002D5068 File Offset: 0x002D4068
		// (set) Token: 0x06006F76 RID: 28534 RVA: 0x002D507A File Offset: 0x002D407A
		[Bindable(true)]
		[CustomCategory("Content")]
		public string InputGestureText
		{
			get
			{
				return (string)base.GetValue(MenuItem.InputGestureTextProperty);
			}
			set
			{
				base.SetValue(MenuItem.InputGestureTextProperty, value);
			}
		}

		// Token: 0x06006F77 RID: 28535 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private static void OnInputGestureTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06006F78 RID: 28536 RVA: 0x002D5088 File Offset: 0x002D4088
		private static object CoerceInputGestureText(DependencyObject d, object value)
		{
			MenuItem menuItem = (MenuItem)d;
			RoutedCommand routedCommand;
			if (string.IsNullOrEmpty((string)value) && !menuItem.HasNonDefaultValue(MenuItem.InputGestureTextProperty) && (routedCommand = (menuItem.Command as RoutedCommand)) != null)
			{
				InputGestureCollection inputGestures = routedCommand.InputGestures;
				if (inputGestures != null && inputGestures.Count >= 1)
				{
					for (int i = 0; i < inputGestures.Count; i++)
					{
						KeyGesture keyGesture = ((IList)inputGestures)[i] as KeyGesture;
						if (keyGesture != null)
						{
							return keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
						}
					}
				}
			}
			return value;
		}

		// Token: 0x170019BC RID: 6588
		// (get) Token: 0x06006F79 RID: 28537 RVA: 0x002D5109 File Offset: 0x002D4109
		// (set) Token: 0x06006F7A RID: 28538 RVA: 0x002D5116 File Offset: 0x002D4116
		[Bindable(true)]
		[CustomCategory("Content")]
		public object Icon
		{
			get
			{
				return base.GetValue(MenuItem.IconProperty);
			}
			set
			{
				base.SetValue(MenuItem.IconProperty, value);
			}
		}

		// Token: 0x170019BD RID: 6589
		// (get) Token: 0x06006F7B RID: 28539 RVA: 0x002D5124 File Offset: 0x002D4124
		// (set) Token: 0x06006F7C RID: 28540 RVA: 0x002D5136 File Offset: 0x002D4136
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSuspendingPopupAnimation
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsSuspendingPopupAnimationProperty);
			}
			internal set
			{
				base.SetValue(MenuItem.IsSuspendingPopupAnimationPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006F7D RID: 28541 RVA: 0x002D514C File Offset: 0x002D414C
		private void NotifySiblingsToSuspendAnimation()
		{
			if (!this.IsSuspendingPopupAnimation)
			{
				bool boolField = MenuItem.GetBoolField(this, MenuItem.BoolField.OpenedWithKeyboard);
				MenuItem ignore = boolField ? null : this;
				MenuBase.SetSuspendingPopupAnimation(ItemsControl.ItemsControlFromItemContainer(this), ignore, true);
				if (!boolField)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object arg)
					{
						((MenuItem)arg).IsSuspendingPopupAnimation = true;
						return null;
					}), this);
					return;
				}
				MenuItem.SetBoolField(this, MenuItem.BoolField.OpenedWithKeyboard, false);
			}
		}

		// Token: 0x06006F7E RID: 28542 RVA: 0x002D51B5 File Offset: 0x002D41B5
		private void NotifyChildrenToResumeAnimation()
		{
			MenuBase.SetSuspendingPopupAnimation(this, null, false);
		}

		// Token: 0x170019BE RID: 6590
		// (get) Token: 0x06006F7F RID: 28543 RVA: 0x002D51BF File Offset: 0x002D41BF
		// (set) Token: 0x06006F80 RID: 28544 RVA: 0x002D51D1 File Offset: 0x002D41D1
		public ItemContainerTemplateSelector ItemContainerTemplateSelector
		{
			get
			{
				return (ItemContainerTemplateSelector)base.GetValue(MenuItem.ItemContainerTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(MenuItem.ItemContainerTemplateSelectorProperty, value);
			}
		}

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x06006F81 RID: 28545 RVA: 0x002D51DF File Offset: 0x002D41DF
		// (set) Token: 0x06006F82 RID: 28546 RVA: 0x002D51F1 File Offset: 0x002D41F1
		public bool UsesItemContainerTemplate
		{
			get
			{
				return (bool)base.GetValue(MenuItem.UsesItemContainerTemplateProperty);
			}
			set
			{
				base.SetValue(MenuItem.UsesItemContainerTemplateProperty, value);
			}
		}

		// Token: 0x06006F83 RID: 28547 RVA: 0x002D51FF File Offset: 0x002D41FF
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MenuItemAutomationPeer(this);
		}

		// Token: 0x06006F84 RID: 28548 RVA: 0x002D5207 File Offset: 0x002D4207
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.UpdateRole();
		}

		// Token: 0x06006F85 RID: 28549 RVA: 0x0029D8BE File Offset: 0x0029C8BE
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			MenuItem.PrepareMenuItem(element, item);
		}

		// Token: 0x06006F86 RID: 28550 RVA: 0x002D5218 File Offset: 0x002D4218
		internal static void PrepareMenuItem(DependencyObject element, object item)
		{
			MenuItem menuItem = element as MenuItem;
			if (menuItem != null)
			{
				ICommand command = item as ICommand;
				if (command != null && !menuItem.HasNonDefaultValue(MenuItem.CommandProperty))
				{
					menuItem.Command = command;
				}
				if (MenuItem.GetBoolField(menuItem, MenuItem.BoolField.CanExecuteInvalid))
				{
					menuItem.UpdateCanExecute();
					return;
				}
			}
			else
			{
				Separator separator = item as Separator;
				if (separator != null)
				{
					bool flag;
					if (separator.GetValueSource(FrameworkElement.StyleProperty, null, out flag) <= BaseValueSourceInternal.ImplicitReference)
					{
						separator.SetResourceReference(FrameworkElement.StyleProperty, MenuItem.SeparatorStyleKey);
					}
					separator.DefaultStyleKey = MenuItem.SeparatorStyleKey;
				}
			}
		}

		// Token: 0x06006F87 RID: 28551 RVA: 0x002D5294 File Offset: 0x002D4294
		protected virtual void OnClick()
		{
			this.OnClickImpl(false);
		}

		// Token: 0x06006F88 RID: 28552 RVA: 0x002D529D File Offset: 0x002D429D
		internal virtual void OnClickCore(bool userInitiated)
		{
			this.OnClick();
		}

		// Token: 0x06006F89 RID: 28553 RVA: 0x002D52A8 File Offset: 0x002D42A8
		internal void OnClickImpl(bool userInitiated)
		{
			if (this.IsCheckable)
			{
				base.SetCurrentValueInternal(MenuItem.IsCheckedProperty, BooleanBoxes.Box(!this.IsChecked));
			}
			if (!base.IsKeyboardFocusWithin)
			{
				this.FocusOrSelect();
			}
			base.RaiseEvent(new RoutedEventArgs(MenuItem.PreviewClickEvent, this));
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(this.InvokeClickAfterRender), userInitiated);
		}

		// Token: 0x06006F8A RID: 28554 RVA: 0x002D5330 File Offset: 0x002D4330
		private object InvokeClickAfterRender(object arg)
		{
			bool userInitiated = (bool)arg;
			base.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent, this));
			CommandHelpers.CriticalExecuteCommandSource(this, userInitiated);
			return null;
		}

		// Token: 0x06006F8B RID: 28555 RVA: 0x002D535D File Offset: 0x002D435D
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseDown(e);
				this.UpdateIsPressed();
				if (e.UserInitiated)
				{
					this._userInitiatedPress = true;
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06006F8C RID: 28556 RVA: 0x002D538A File Offset: 0x002D438A
		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseDown(e);
				if (e.UserInitiated)
				{
					this._userInitiatedPress = true;
				}
			}
			base.OnMouseRightButtonDown(e);
		}

		// Token: 0x06006F8D RID: 28557 RVA: 0x002D53B1 File Offset: 0x002D43B1
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseUp(e);
				this.UpdateIsPressed();
				this._userInitiatedPress = false;
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x06006F8E RID: 28558 RVA: 0x002D53D6 File Offset: 0x002D43D6
		protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseUp(e);
				this._userInitiatedPress = false;
			}
			base.OnMouseRightButtonUp(e);
		}

		// Token: 0x06006F8F RID: 28559 RVA: 0x002D53F8 File Offset: 0x002D43F8
		private void HandleMouseDown(MouseButtonEventArgs e)
		{
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(e.GetPosition(this)) && (e.ChangedButton == MouseButton.Left || (e.ChangedButton == MouseButton.Right && this.InsideContextMenu)))
			{
				MenuItemRole role = this.Role;
				if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader)
				{
					this.ClickHeader();
				}
			}
			e.Handled = true;
		}

		// Token: 0x06006F90 RID: 28560 RVA: 0x002D5460 File Offset: 0x002D4460
		private void HandleMouseUp(MouseButtonEventArgs e)
		{
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(e.GetPosition(this)) && (e.ChangedButton == MouseButton.Left || (e.ChangedButton == MouseButton.Right && this.InsideContextMenu)))
			{
				MenuItemRole role = this.Role;
				if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem)
				{
					if (this._userInitiatedPress)
					{
						this.ClickItem(e.UserInitiated);
					}
					else
					{
						this.ClickItem(false);
					}
				}
			}
			if (e.ChangedButton != MouseButton.Right || this.InsideContextMenu)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06006F91 RID: 28561 RVA: 0x002D54F0 File Offset: 0x002D44F0
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			bool flag = false;
			if (e.Target == null)
			{
				if (Mouse.Captured == null || Mouse.Captured is MenuBase)
				{
					e.Target = menuItem;
					if (e.OriginalSource == menuItem && menuItem.IsSubmenuOpen)
					{
						flag = true;
					}
				}
				else
				{
					e.Handled = true;
				}
			}
			else if (e.Scope == null)
			{
				if (e.Target != menuItem && e.Target is MenuItem)
				{
					flag = true;
				}
				else
				{
					DependencyObject dependencyObject = e.Source as DependencyObject;
					while (dependencyObject != null && dependencyObject != menuItem)
					{
						UIElement uielement = dependencyObject as UIElement;
						if (uielement != null && ItemsControl.ItemsControlFromItemContainer(uielement) == menuItem)
						{
							flag = true;
							break;
						}
						dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject);
					}
				}
			}
			if (flag)
			{
				e.Scope = menuItem;
				e.Handled = true;
			}
		}

		// Token: 0x06006F92 RID: 28562 RVA: 0x002D55AC File Offset: 0x002D45AC
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			MenuItemRole role = this.Role;
			if (((role == MenuItemRole.TopLevelHeader || role == MenuItemRole.TopLevelItem) && this.IsInMenuMode) || role == MenuItemRole.SubmenuHeader || role == MenuItemRole.SubmenuItem)
			{
				this.MouseLeaveInMenuMode(role);
			}
			else if (base.IsMouseOver != this.IsSelected)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.Box(base.IsMouseOver));
			}
			this.UpdateIsPressed();
		}

		// Token: 0x06006F93 RID: 28563 RVA: 0x002D5614 File Offset: 0x002D4614
		protected override void OnMouseMove(MouseEventArgs e)
		{
			MenuItem menuItem = ItemsControl.ItemsControlFromItemContainer(this) as MenuItem;
			if (menuItem != null && MenuItem.GetBoolField(menuItem, MenuItem.BoolField.MouseEnterOnMouseMove))
			{
				MenuItem.SetBoolField(menuItem, MenuItem.BoolField.MouseEnterOnMouseMove, false);
				this.MouseEnterHelper();
			}
		}

		// Token: 0x06006F94 RID: 28564 RVA: 0x002D5647 File Offset: 0x002D4647
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			this.MouseEnterHelper();
		}

		// Token: 0x06006F95 RID: 28565 RVA: 0x002D5658 File Offset: 0x002D4658
		private void MouseEnterHelper()
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
			if (itemsControl == null || !MenuItem.GetBoolField(itemsControl, MenuItem.BoolField.IgnoreMouseEvents))
			{
				MenuItemRole role = this.Role;
				if (((role == MenuItemRole.TopLevelHeader || role == MenuItemRole.TopLevelItem) && this.OpenOnMouseEnter) || role == MenuItemRole.SubmenuHeader || role == MenuItemRole.SubmenuItem)
				{
					this.MouseEnterInMenuMode(role);
				}
				else if (base.IsMouseOver != this.IsSelected)
				{
					base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.Box(base.IsMouseOver));
				}
				this.UpdateIsPressed();
				return;
			}
			if (itemsControl is MenuItem)
			{
				MenuItem.SetBoolField(itemsControl, MenuItem.BoolField.MouseEnterOnMouseMove, true);
			}
		}

		// Token: 0x06006F96 RID: 28566 RVA: 0x002D56DC File Offset: 0x002D46DC
		private void MouseEnterInMenuMode(MenuItemRole role)
		{
			if (role > MenuItemRole.TopLevelHeader)
			{
				if (role - MenuItemRole.SubmenuItem <= 1)
				{
					MenuItem currentSibling = this.CurrentSibling;
					if (currentSibling == null || !currentSibling.IsSubmenuOpen)
					{
						if (!this.IsSubmenuOpen)
						{
							this.FocusOrSelect();
						}
						else
						{
							this.IsHighlighted = true;
						}
					}
					else
					{
						currentSibling.IsHighlighted = false;
						this.IsHighlighted = true;
					}
					if (!this.IsSelected || !this.IsSubmenuOpen)
					{
						this.SetTimerToOpenHierarchy();
					}
				}
			}
			else if (!this.IsSubmenuOpen)
			{
				this.OpenHierarchy(role);
			}
			this.StopTimer(ref this._closeHierarchyTimer);
		}

		// Token: 0x06006F97 RID: 28567 RVA: 0x002D5764 File Offset: 0x002D4764
		private void MouseLeaveInMenuMode(MenuItemRole role)
		{
			if (role == MenuItemRole.SubmenuHeader || role == MenuItemRole.SubmenuItem)
			{
				if (MenuItem.GetBoolField(this, MenuItem.BoolField.IgnoreNextMouseLeave))
				{
					MenuItem.SetBoolField(this, MenuItem.BoolField.IgnoreNextMouseLeave, false);
				}
				else if (!this.IsSubmenuOpen)
				{
					if (this.IsSelected)
					{
						base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
					}
					else
					{
						this.IsHighlighted = false;
					}
					if (base.IsKeyboardFocusWithin)
					{
						ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
						if (itemsControl != null)
						{
							itemsControl.Focus();
						}
					}
				}
				else if (this.IsMouseOverSibling)
				{
					this.SetTimerToCloseHierarchy();
				}
			}
			this.StopTimer(ref this._openHierarchyTimer);
		}

		// Token: 0x06006F98 RID: 28568 RVA: 0x002D57EA File Offset: 0x002D47EA
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			if (!e.Handled && e.NewFocus == this)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
			}
		}

		// Token: 0x06006F99 RID: 28569 RVA: 0x002D5814 File Offset: 0x002D4814
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (base.IsKeyboardFocusWithin && !this.IsSelected)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
			}
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x06006F9A RID: 28570 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006F9B RID: 28571 RVA: 0x002D5840 File Offset: 0x002D4840
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = false;
			Key key = e.Key;
			MenuItemRole role = this.Role;
			if (base.FlowDirection == FlowDirection.RightToLeft)
			{
				if (key == Key.Right)
				{
					key = Key.Left;
				}
				else if (key == Key.Left)
				{
					key = Key.Right;
				}
			}
			if (key <= Key.Return)
			{
				if (key != Key.Tab)
				{
					if (key == Key.Return)
					{
						if (role == MenuItemRole.SubmenuItem || role == MenuItemRole.TopLevelItem)
						{
							this.ClickItem(e.UserInitiated);
							flag = true;
						}
						else if (role == MenuItemRole.TopLevelHeader)
						{
							this.OpenSubmenuWithKeyboard();
							flag = true;
						}
						else if (role == MenuItemRole.SubmenuHeader && !this.IsSubmenuOpen)
						{
							this.OpenSubmenuWithKeyboard();
							flag = true;
						}
					}
				}
				else if (role == MenuItemRole.SubmenuHeader && this.IsSubmenuOpen && this.CurrentSelection == null)
				{
					if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
					{
						base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
					}
					else
					{
						base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
					}
					flag = true;
				}
			}
			else
			{
				if (key != Key.Escape)
				{
					switch (key)
					{
					case Key.Left:
						break;
					case Key.Up:
						if (role == MenuItemRole.SubmenuHeader && this.IsSubmenuOpen && this.CurrentSelection == null)
						{
							base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_1AB;
						}
						goto IL_1AB;
					case Key.Right:
						if (role == MenuItemRole.SubmenuHeader && !this.IsSubmenuOpen)
						{
							this.OpenSubmenuWithKeyboard();
							flag = true;
							goto IL_1AB;
						}
						goto IL_1AB;
					case Key.Down:
						if (role == MenuItemRole.SubmenuHeader && this.IsSubmenuOpen && this.CurrentSelection == null)
						{
							base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_1AB;
						}
						goto IL_1AB;
					default:
						goto IL_1AB;
					}
				}
				if (role != MenuItemRole.TopLevelHeader && role != MenuItemRole.TopLevelItem && this.IsSubmenuOpen)
				{
					base.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
					flag = true;
				}
			}
			IL_1AB:
			if (!flag)
			{
				ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(this);
				if (parent != null && !MenuItem.GetBoolField(parent, MenuItem.BoolField.IgnoreMouseEvents))
				{
					MenuItem.SetBoolField(parent, MenuItem.BoolField.IgnoreMouseEvents, true);
					parent.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(object param)
					{
						MenuItem.SetBoolField(parent, MenuItem.BoolField.IgnoreMouseEvents, false);
						return null;
					}), null);
				}
				flag = this.MenuItemNavigate(e.Key, e.KeyboardDevice.Modifiers);
			}
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06006F9C RID: 28572 RVA: 0x002D5A74 File Offset: 0x002D4A74
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			base.OnAccessKey(e);
			if (!e.IsMultiple)
			{
				switch (this.Role)
				{
				case MenuItemRole.TopLevelItem:
				case MenuItemRole.SubmenuItem:
					this.ClickItem(e.UserInitiated);
					return;
				case MenuItemRole.TopLevelHeader:
				case MenuItemRole.SubmenuHeader:
					this.OpenSubmenuWithKeyboard();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06006F9D RID: 28573 RVA: 0x002D5AC1 File Offset: 0x002D4AC1
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			this.UpdateRole();
			base.OnItemsChanged(e);
		}

		// Token: 0x06006F9E RID: 28574 RVA: 0x002D5AD0 File Offset: 0x002D4AD0
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			bool flag = item is MenuItem || item is Separator;
			if (!flag)
			{
				this._currentItem = item;
			}
			return flag;
		}

		// Token: 0x06006F9F RID: 28575 RVA: 0x002D5AF0 File Offset: 0x002D4AF0
		protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
		{
			return !(item is Separator) && base.ShouldApplyItemContainerStyle(container, item);
		}

		// Token: 0x06006FA0 RID: 28576 RVA: 0x002D5B04 File Offset: 0x002D4B04
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

		// Token: 0x06006FA1 RID: 28577 RVA: 0x002D5BA8 File Offset: 0x002D4BA8
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			this.UpdateRole();
			DependencyObject parentInternal = VisualTreeHelper.GetParentInternal(this);
			if (base.Parent != null && parentInternal != null && base.Parent != parentInternal)
			{
				Binding binding = new Binding();
				binding.Path = new PropertyPath(DefinitionBase.PrivateSharedSizeScopeProperty);
				binding.Mode = BindingMode.OneWay;
				binding.Source = parentInternal;
				BindingOperations.SetBinding(this, DefinitionBase.PrivateSharedSizeScopeProperty, binding);
			}
			if (parentInternal == null)
			{
				BindingOperations.ClearBinding(this, DefinitionBase.PrivateSharedSizeScopeProperty);
			}
		}

		// Token: 0x06006FA2 RID: 28578 RVA: 0x002D5C1C File Offset: 0x002D4C1C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._submenuPopup != null)
			{
				this._submenuPopup.Closed -= this.OnPopupClosed;
			}
			this._submenuPopup = (base.GetTemplateChild("PART_Popup") as Popup);
			if (this._submenuPopup != null)
			{
				this._submenuPopup.Closed += this.OnPopupClosed;
			}
		}

		// Token: 0x06006FA3 RID: 28579 RVA: 0x002D5C84 File Offset: 0x002D4C84
		private void SetMenuMode(bool menuMode)
		{
			MenuBase menuBase = this.LogicalParent as MenuBase;
			if (menuBase != null && menuBase.IsMenuMode != menuMode)
			{
				menuBase.IsMenuMode = menuMode;
			}
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x06006FA4 RID: 28580 RVA: 0x002D5CB0 File Offset: 0x002D4CB0
		private bool IsInMenuMode
		{
			get
			{
				MenuBase menuBase = this.LogicalParent as MenuBase;
				return menuBase != null && menuBase.IsMenuMode;
			}
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x06006FA5 RID: 28581 RVA: 0x002D5CD4 File Offset: 0x002D4CD4
		private bool OpenOnMouseEnter
		{
			get
			{
				MenuBase menuBase = this.LogicalParent as MenuBase;
				return menuBase != null && menuBase.OpenOnMouseEnter;
			}
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x06006FA6 RID: 28582 RVA: 0x002D5CF8 File Offset: 0x002D4CF8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private bool InsideContextMenu
		{
			get
			{
				return (bool)base.GetValue(MenuItem.InsideContextMenuProperty);
			}
		}

		// Token: 0x06006FA7 RID: 28583 RVA: 0x002D5D0A File Offset: 0x002D4D0A
		internal static void SetInsideContextMenuProperty(UIElement element, bool value)
		{
			element.SetValue(MenuItem.InsideContextMenuProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x06006FA8 RID: 28584 RVA: 0x002D5D1D File Offset: 0x002D4D1D
		internal void ClickItem()
		{
			this.ClickItem(false);
		}

		// Token: 0x06006FA9 RID: 28585 RVA: 0x002D5D28 File Offset: 0x002D4D28
		private void ClickItem(bool userInitiated)
		{
			try
			{
				this.OnClickCore(userInitiated);
			}
			finally
			{
				if (this.Role == MenuItemRole.TopLevelItem && !this.StaysOpenOnClick)
				{
					this.SetMenuMode(false);
				}
			}
		}

		// Token: 0x06006FAA RID: 28586 RVA: 0x002D5D68 File Offset: 0x002D4D68
		internal void ClickHeader()
		{
			if (!base.IsKeyboardFocusWithin)
			{
				this.FocusOrSelect();
			}
			if (this.IsSubmenuOpen)
			{
				if (this.Role == MenuItemRole.TopLevelHeader)
				{
					this.SetMenuMode(false);
					return;
				}
			}
			else
			{
				this.OpenMenu();
			}
		}

		// Token: 0x06006FAB RID: 28587 RVA: 0x002D5D98 File Offset: 0x002D4D98
		internal bool OpenMenu()
		{
			if (!this.IsSubmenuOpen)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
				if (itemsControl == null)
				{
					itemsControl = (VisualTreeHelper.GetParent(this) as ItemsControl);
				}
				if (itemsControl != null && (itemsControl is MenuItem || itemsControl is MenuBase))
				{
					base.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.TrueBox);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006FAC RID: 28588 RVA: 0x002D5DE9 File Offset: 0x002D4DE9
		internal void OpenSubmenuWithKeyboard()
		{
			MenuItem.SetBoolField(this, MenuItem.BoolField.OpenedWithKeyboard, true);
			if (this.OpenMenu())
			{
				base.NavigateToStart(new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers));
			}
		}

		// Token: 0x06006FAD RID: 28589 RVA: 0x002D5E10 File Offset: 0x002D4E10
		private bool MenuItemNavigate(Key key, ModifierKeys modifiers)
		{
			if (key == Key.Left || key == Key.Right || key == Key.Up || key == Key.Down)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
				if (itemsControl != null)
				{
					if (!itemsControl.HasItems)
					{
						return false;
					}
					if (itemsControl.Items.Count == 1 && !(itemsControl is Menu) && key == Key.Up && key == Key.Down)
					{
						return true;
					}
					object focusedElement = Keyboard.FocusedElement;
					itemsControl.NavigateByLine(itemsControl.FocusedInfo, KeyboardNavigation.KeyToTraversalDirection(key), new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, modifiers));
					object focusedElement2 = Keyboard.FocusedElement;
					if (focusedElement2 != focusedElement && focusedElement2 != this)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x06006FAE RID: 28590 RVA: 0x002D5E9C File Offset: 0x002D4E9C
		internal object LogicalParent
		{
			get
			{
				if (base.Parent != null)
				{
					return base.Parent;
				}
				return ItemsControl.ItemsControlFromItemContainer(this);
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x06006FAF RID: 28591 RVA: 0x002D5EB4 File Offset: 0x002D4EB4
		private MenuItem CurrentSibling
		{
			get
			{
				object logicalParent = this.LogicalParent;
				MenuItem menuItem = logicalParent as MenuItem;
				MenuItem menuItem2 = null;
				if (menuItem != null)
				{
					menuItem2 = menuItem.CurrentSelection;
				}
				else
				{
					MenuBase menuBase = logicalParent as MenuBase;
					if (menuBase != null)
					{
						menuItem2 = menuBase.CurrentSelection;
					}
				}
				if (menuItem2 == this)
				{
					menuItem2 = null;
				}
				return menuItem2;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x06006FB0 RID: 28592 RVA: 0x002D5EF8 File Offset: 0x002D4EF8
		private bool IsMouseOverSibling
		{
			get
			{
				FrameworkElement frameworkElement = this.LogicalParent as FrameworkElement;
				return frameworkElement != null && MenuItem.IsMouseReallyOver(frameworkElement) && !base.IsMouseOver;
			}
		}

		// Token: 0x06006FB1 RID: 28593 RVA: 0x002D5F28 File Offset: 0x002D4F28
		private static bool IsMouseReallyOver(FrameworkElement elem)
		{
			bool isMouseOver = elem.IsMouseOver;
			return (!isMouseOver || Mouse.Captured != elem || Mouse.DirectlyOver != elem) && isMouseOver;
		}

		// Token: 0x06006FB2 RID: 28594 RVA: 0x002D5F52 File Offset: 0x002D4F52
		private void OpenHierarchy(MenuItemRole role)
		{
			this.FocusOrSelect();
			if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader)
			{
				this.OpenMenu();
			}
		}

		// Token: 0x06006FB3 RID: 28595 RVA: 0x002D5F69 File Offset: 0x002D4F69
		private void FocusOrSelect()
		{
			if (!base.IsKeyboardFocusWithin)
			{
				base.Focus();
			}
			if (!this.IsSelected)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
			}
			if (this.IsSelected && !this.IsHighlighted)
			{
				this.IsHighlighted = true;
			}
		}

		// Token: 0x06006FB4 RID: 28596 RVA: 0x002D5FAC File Offset: 0x002D4FAC
		private void SetTimerToOpenHierarchy()
		{
			if (this._openHierarchyTimer == null)
			{
				this._openHierarchyTimer = new DispatcherTimer(DispatcherPriority.Normal);
				this._openHierarchyTimer.Tick += delegate(object sender, EventArgs e)
				{
					this.OpenHierarchy(this.Role);
					this.StopTimer(ref this._openHierarchyTimer);
				};
			}
			else
			{
				this._openHierarchyTimer.Stop();
			}
			this.StartTimer(this._openHierarchyTimer);
		}

		// Token: 0x06006FB5 RID: 28597 RVA: 0x002D6000 File Offset: 0x002D5000
		private void SetTimerToCloseHierarchy()
		{
			if (this._closeHierarchyTimer == null)
			{
				this._closeHierarchyTimer = new DispatcherTimer(DispatcherPriority.Normal);
				this._closeHierarchyTimer.Tick += delegate(object sender, EventArgs e)
				{
					base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
					this.StopTimer(ref this._closeHierarchyTimer);
				};
			}
			else
			{
				this._closeHierarchyTimer.Stop();
			}
			this.StartTimer(this._closeHierarchyTimer);
		}

		// Token: 0x06006FB6 RID: 28598 RVA: 0x002D6052 File Offset: 0x002D5052
		private void StopTimer(ref DispatcherTimer timer)
		{
			if (timer != null)
			{
				timer.Stop();
				timer = null;
			}
		}

		// Token: 0x06006FB7 RID: 28599 RVA: 0x002D6062 File Offset: 0x002D5062
		private void StartTimer(DispatcherTimer timer)
		{
			timer.Interval = TimeSpan.FromMilliseconds((double)SystemParameters.MenuShowDelay);
			timer.Start();
		}

		// Token: 0x06006FB8 RID: 28600 RVA: 0x002D607C File Offset: 0x002D507C
		private static object OnCoerceAcceleratorKey(DependencyObject d, object value)
		{
			if (value == null)
			{
				string inputGestureText = ((MenuItem)d).InputGestureText;
				if (inputGestureText != string.Empty)
				{
					value = inputGestureText;
				}
			}
			return value;
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x06006FB9 RID: 28601 RVA: 0x002D60A9 File Offset: 0x002D50A9
		// (set) Token: 0x06006FBA RID: 28602 RVA: 0x002D60B4 File Offset: 0x002D50B4
		private MenuItem CurrentSelection
		{
			get
			{
				return this._currentSelection;
			}
			set
			{
				if (this._currentSelection != null)
				{
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
				}
				this._currentSelection = value;
				if (this._currentSelection != null)
				{
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
				}
			}
		}

		// Token: 0x06006FBB RID: 28603 RVA: 0x002D6102 File Offset: 0x002D5102
		private static bool GetBoolField(UIElement element, MenuItem.BoolField field)
		{
			return ((MenuItem.BoolField)element.GetValue(MenuItem.BooleanFieldStoreProperty) & field) > (MenuItem.BoolField)0;
		}

		// Token: 0x06006FBC RID: 28604 RVA: 0x002D611C File Offset: 0x002D511C
		private static void SetBoolField(UIElement element, MenuItem.BoolField field, bool value)
		{
			if (value)
			{
				element.SetValue(MenuItem.BooleanFieldStoreProperty, (MenuItem.BoolField)element.GetValue(MenuItem.BooleanFieldStoreProperty) | field);
				return;
			}
			element.SetValue(MenuItem.BooleanFieldStoreProperty, (MenuItem.BoolField)element.GetValue(MenuItem.BooleanFieldStoreProperty) & ~field);
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x06006FBD RID: 28605 RVA: 0x001FCA42 File Offset: 0x001FBA42
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x06006FBE RID: 28606 RVA: 0x002D6172 File Offset: 0x002D5172
		// (set) Token: 0x06006FBF RID: 28607 RVA: 0x002D617E File Offset: 0x002D517E
		private bool CanExecute
		{
			get
			{
				return !base.ReadControlFlag(Control.ControlBoolFlags.CommandDisabled);
			}
			set
			{
				if (value != this.CanExecute)
				{
					base.WriteControlFlag(Control.ControlBoolFlags.CommandDisabled, !value);
					base.CoerceValue(UIElement.IsEnabledProperty);
				}
			}
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x06006FC0 RID: 28608 RVA: 0x002D619F File Offset: 0x002D519F
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return MenuItem._dType;
			}
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x06006FC1 RID: 28609 RVA: 0x002D61A6 File Offset: 0x002D51A6
		public static ResourceKey SeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.MenuItemSeparatorStyleKey;
			}
		}

		// Token: 0x0400368C RID: 13964
		private static ComponentResourceKey _topLevelItemTemplateKey;

		// Token: 0x0400368D RID: 13965
		private static ComponentResourceKey _topLevelHeaderTemplateKey;

		// Token: 0x0400368E RID: 13966
		private static ComponentResourceKey _submenuItemTemplateKey;

		// Token: 0x0400368F RID: 13967
		private static ComponentResourceKey _submenuHeaderTemplateKey;

		// Token: 0x04003691 RID: 13969
		internal static readonly RoutedEvent PreviewClickEvent;

		// Token: 0x04003696 RID: 13974
		public static readonly DependencyProperty CommandProperty;

		// Token: 0x04003697 RID: 13975
		public static readonly DependencyProperty CommandParameterProperty;

		// Token: 0x04003698 RID: 13976
		public static readonly DependencyProperty CommandTargetProperty;

		// Token: 0x04003699 RID: 13977
		public static readonly DependencyProperty IsSubmenuOpenProperty;

		// Token: 0x0400369A RID: 13978
		private static readonly DependencyPropertyKey RolePropertyKey;

		// Token: 0x0400369B RID: 13979
		public static readonly DependencyProperty RoleProperty;

		// Token: 0x0400369C RID: 13980
		public static readonly DependencyProperty IsCheckableProperty;

		// Token: 0x0400369D RID: 13981
		private static readonly DependencyPropertyKey IsPressedPropertyKey;

		// Token: 0x0400369E RID: 13982
		public static readonly DependencyProperty IsPressedProperty;

		// Token: 0x0400369F RID: 13983
		private static readonly DependencyPropertyKey IsHighlightedPropertyKey;

		// Token: 0x040036A0 RID: 13984
		public static readonly DependencyProperty IsHighlightedProperty;

		// Token: 0x040036A1 RID: 13985
		public static readonly DependencyProperty IsCheckedProperty;

		// Token: 0x040036A2 RID: 13986
		public static readonly DependencyProperty StaysOpenOnClickProperty;

		// Token: 0x040036A3 RID: 13987
		internal static readonly DependencyProperty IsSelectedProperty;

		// Token: 0x040036A4 RID: 13988
		public static readonly DependencyProperty InputGestureTextProperty;

		// Token: 0x040036A5 RID: 13989
		public static readonly DependencyProperty IconProperty;

		// Token: 0x040036A6 RID: 13990
		private static readonly DependencyPropertyKey IsSuspendingPopupAnimationPropertyKey;

		// Token: 0x040036A7 RID: 13991
		public static readonly DependencyProperty IsSuspendingPopupAnimationProperty;

		// Token: 0x040036A8 RID: 13992
		public static readonly DependencyProperty ItemContainerTemplateSelectorProperty;

		// Token: 0x040036A9 RID: 13993
		public static readonly DependencyProperty UsesItemContainerTemplateProperty;

		// Token: 0x040036AA RID: 13994
		private object _currentItem;

		// Token: 0x040036AB RID: 13995
		internal static readonly DependencyProperty InsideContextMenuProperty;

		// Token: 0x040036AC RID: 13996
		private static readonly DependencyProperty BooleanFieldStoreProperty;

		// Token: 0x040036AD RID: 13997
		private const string PopupTemplateName = "PART_Popup";

		// Token: 0x040036AE RID: 13998
		private MenuItem _currentSelection;

		// Token: 0x040036AF RID: 13999
		private Popup _submenuPopup;

		// Token: 0x040036B0 RID: 14000
		private DispatcherTimer _openHierarchyTimer;

		// Token: 0x040036B1 RID: 14001
		private DispatcherTimer _closeHierarchyTimer;

		// Token: 0x040036B2 RID: 14002
		private bool _userInitiatedPress;

		// Token: 0x040036B3 RID: 14003
		private static DependencyObjectType _dType;

		// Token: 0x02000C0C RID: 3084
		[Flags]
		private enum BoolField
		{
			// Token: 0x04004ACB RID: 19147
			OpenedWithKeyboard = 1,
			// Token: 0x04004ACC RID: 19148
			IgnoreNextMouseLeave = 2,
			// Token: 0x04004ACD RID: 19149
			IgnoreMouseEvents = 4,
			// Token: 0x04004ACE RID: 19150
			MouseEnterOnMouseMove = 8,
			// Token: 0x04004ACF RID: 19151
			CanExecuteInvalid = 16
		}
	}
}
