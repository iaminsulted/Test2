using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007A6 RID: 1958
	[Localizability(LocalizationCategory.ListBox)]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListBoxItem))]
	public class ListBox : Selector
	{
		// Token: 0x06006E84 RID: 28292 RVA: 0x002D1E08 File Offset: 0x002D0E08
		public ListBox()
		{
			this.Initialize();
		}

		// Token: 0x06006E85 RID: 28293 RVA: 0x002D1E18 File Offset: 0x002D0E18
		private void Initialize()
		{
			SelectionMode mode = (SelectionMode)ListBox.SelectionModeProperty.GetDefaultValue(base.DependencyObjectType);
			this.ValidateSelectionMode(mode);
		}

		// Token: 0x06006E86 RID: 28294 RVA: 0x002D1E44 File Offset: 0x002D0E44
		static ListBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(typeof(ListBox)));
			ListBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ListBox));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			ItemsControl.IsTextSearchEnabledProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
			itemsPanelTemplate.Seal();
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(itemsPanelTemplate));
			EventManager.RegisterClassHandler(typeof(ListBox), Mouse.MouseUpEvent, new MouseButtonEventHandler(ListBox.OnMouseButtonUp), true);
			EventManager.RegisterClassHandler(typeof(ListBox), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ListBox.OnGotKeyboardFocus));
			CommandHelpers.RegisterCommandHandler(typeof(ListBox), ListBox.SelectAllCommand, new ExecutedRoutedEventHandler(ListBox.OnSelectAll), new CanExecuteRoutedEventHandler(ListBox.OnQueryStatusSelectAll), KeyGesture.CreateFromResourceStrings("Ctrl+A", SR.Get("ListBoxSelectAllKeyDisplayString")));
			ControlsTraceLogger.AddControl(TelemetryControls.ListBox);
		}

		// Token: 0x06006E87 RID: 28295 RVA: 0x002D2036 File Offset: 0x002D1036
		public void SelectAll()
		{
			if (base.CanSelectMultiple)
			{
				this.SelectAllImpl();
				return;
			}
			throw new NotSupportedException(SR.Get("ListBoxSelectAllSelectionMode"));
		}

		// Token: 0x06006E88 RID: 28296 RVA: 0x002D2056 File Offset: 0x002D1056
		public void UnselectAll()
		{
			this.UnselectAllImpl();
		}

		// Token: 0x06006E89 RID: 28297 RVA: 0x002D205E File Offset: 0x002D105E
		public void ScrollIntoView(object item)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				base.OnBringItemIntoView(item);
				return;
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(base.OnBringItemIntoView), item);
		}

		// Token: 0x17001985 RID: 6533
		// (get) Token: 0x06006E8A RID: 28298 RVA: 0x002D2091 File Offset: 0x002D1091
		// (set) Token: 0x06006E8B RID: 28299 RVA: 0x002D20A3 File Offset: 0x002D10A3
		public SelectionMode SelectionMode
		{
			get
			{
				return (SelectionMode)base.GetValue(ListBox.SelectionModeProperty);
			}
			set
			{
				base.SetValue(ListBox.SelectionModeProperty, value);
			}
		}

		// Token: 0x06006E8C RID: 28300 RVA: 0x002D20B6 File Offset: 0x002D10B6
		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBox listBox = (ListBox)d;
			listBox.ValidateSelectionMode(listBox.SelectionMode);
		}

		// Token: 0x06006E8D RID: 28301 RVA: 0x002D20C9 File Offset: 0x002D10C9
		private static object OnGetSelectionMode(DependencyObject d)
		{
			return ((ListBox)d).SelectionMode;
		}

		// Token: 0x06006E8E RID: 28302 RVA: 0x002D20DC File Offset: 0x002D10DC
		private static bool IsValidSelectionMode(object o)
		{
			SelectionMode selectionMode = (SelectionMode)o;
			return selectionMode == SelectionMode.Single || selectionMode == SelectionMode.Multiple || selectionMode == SelectionMode.Extended;
		}

		// Token: 0x06006E8F RID: 28303 RVA: 0x002D20FD File Offset: 0x002D10FD
		private void ValidateSelectionMode(SelectionMode mode)
		{
			base.CanSelectMultiple = (mode > SelectionMode.Single);
		}

		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x06006E90 RID: 28304 RVA: 0x002D2109 File Offset: 0x002D1109
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList SelectedItems
		{
			get
			{
				return base.SelectedItemsImpl;
			}
		}

		// Token: 0x06006E91 RID: 28305 RVA: 0x002D2111 File Offset: 0x002D1111
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ListBoxAutomationPeer(this);
		}

		// Token: 0x06006E92 RID: 28306 RVA: 0x002D2119 File Offset: 0x002D1119
		protected bool SetSelectedItems(IEnumerable selectedItems)
		{
			return base.SetSelectedItemsImpl(selectedItems);
		}

		// Token: 0x06006E93 RID: 28307 RVA: 0x0029B48A File Offset: 0x0029A48A
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (item is Separator)
			{
				Separator.PrepareContainer(element as Control);
			}
		}

		// Token: 0x06006E94 RID: 28308 RVA: 0x002D2122 File Offset: 0x002D1122
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			base.AdjustItemInfo(e, this._anchorItem);
			if (this._anchorItem != null && this._anchorItem.Index < 0)
			{
				this._anchorItem = null;
			}
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x06006E95 RID: 28309 RVA: 0x002D215B File Offset: 0x002D115B
		internal override void AdjustItemInfosAfterGeneratorChangeOverride()
		{
			base.AdjustItemInfoAfterGeneratorChange(this._anchorItem);
			base.AdjustItemInfosAfterGeneratorChangeOverride();
		}

		// Token: 0x06006E96 RID: 28310 RVA: 0x002D2170 File Offset: 0x002D1170
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			if (this.SelectionMode == SelectionMode.Single)
			{
				ItemsControl.ItemInfo internalSelectedInfo = base.InternalSelectedInfo;
				if (((internalSelectedInfo != null) ? (internalSelectedInfo.Container as ListBoxItem) : null) != null)
				{
					this.UpdateAnchorAndActionItem(internalSelectedInfo);
				}
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionPatternOnInvalidated) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				ListBoxAutomationPeer listBoxAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this) as ListBoxAutomationPeer;
				if (listBoxAutomationPeer != null)
				{
					listBoxAutomationPeer.RaiseSelectionEvents(e);
				}
			}
		}

		// Token: 0x06006E97 RID: 28311 RVA: 0x002D21EC File Offset: 0x002D11EC
		protected override void OnKeyDown(KeyEventArgs e)
		{
			bool flag = true;
			Key key = e.Key;
			if (key <= Key.Down)
			{
				if (key != Key.Return)
				{
					switch (key)
					{
					case Key.Space:
						break;
					case Key.Prior:
						base.NavigateByPage(FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.Next:
						base.NavigateByPage(FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.End:
						base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.Home:
						base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.Left:
					case Key.Up:
					case Key.Right:
					case Key.Down:
					{
						KeyboardNavigation.ShowFocusVisual();
						bool flag2 = base.ScrollHost != null;
						if (flag2)
						{
							flag2 = ((key == Key.Down && base.IsLogicalHorizontal && DoubleUtil.GreaterThan(base.ScrollHost.ScrollableHeight, base.ScrollHost.VerticalOffset)) || (key == Key.Up && base.IsLogicalHorizontal && DoubleUtil.GreaterThan(base.ScrollHost.VerticalOffset, 0.0)) || (key == Key.Right && base.IsLogicalVertical && DoubleUtil.GreaterThan(base.ScrollHost.ScrollableWidth, base.ScrollHost.HorizontalOffset)) || (key == Key.Left && base.IsLogicalVertical && DoubleUtil.GreaterThan(base.ScrollHost.HorizontalOffset, 0.0)));
						}
						if (flag2)
						{
							base.ScrollHost.ScrollInDirection(e);
							goto IL_379;
						}
						if ((base.ItemsHost == null || !base.ItemsHost.IsKeyboardFocusWithin) && !base.IsKeyboardFocused)
						{
							flag = false;
							goto IL_379;
						}
						if (!base.NavigateByLine(KeyboardNavigation.KeyToTraversalDirection(key), new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers)))
						{
							flag = false;
							goto IL_379;
						}
						goto IL_379;
					}
					default:
						goto IL_377;
					}
				}
				if (e.Key == Key.Return && !(bool)base.GetValue(KeyboardNavigation.AcceptsReturnProperty))
				{
					flag = false;
					goto IL_379;
				}
				ListBoxItem listBoxItem = e.OriginalSource as ListBoxItem;
				if ((Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) == ModifierKeys.Alt)
				{
					flag = false;
					goto IL_379;
				}
				if (base.IsTextSearchEnabled && Keyboard.Modifiers == ModifierKeys.None)
				{
					TextSearch textSearch = TextSearch.EnsureInstance(this);
					if (textSearch != null && textSearch.GetCurrentPrefix() != string.Empty)
					{
						flag = false;
						goto IL_379;
					}
				}
				if (listBoxItem == null || ItemsControl.ItemsControlFromItemContainer(listBoxItem) != this)
				{
					flag = false;
					goto IL_379;
				}
				switch (this.SelectionMode)
				{
				case SelectionMode.Single:
					if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
					{
						this.MakeToggleSelection(listBoxItem);
						goto IL_379;
					}
					this.MakeSingleSelection(listBoxItem);
					goto IL_379;
				case SelectionMode.Multiple:
					this.MakeToggleSelection(listBoxItem);
					goto IL_379;
				case SelectionMode.Extended:
					if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == ModifierKeys.Control)
					{
						this.MakeToggleSelection(listBoxItem);
						goto IL_379;
					}
					if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == ModifierKeys.Shift)
					{
						this.MakeAnchorSelection(listBoxItem, true);
						goto IL_379;
					}
					if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
					{
						this.MakeSingleSelection(listBoxItem);
						goto IL_379;
					}
					flag = false;
					goto IL_379;
				default:
					goto IL_379;
				}
			}
			else if (key != Key.Divide && key != Key.Oem2)
			{
				if (key == Key.Oem5)
				{
					if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control || this.SelectionMode != SelectionMode.Extended)
					{
						flag = false;
						goto IL_379;
					}
					ListBoxItem listBoxItem2 = (base.FocusedInfo != null) ? (base.FocusedInfo.Container as ListBoxItem) : null;
					if (listBoxItem2 != null)
					{
						this.MakeSingleSelection(listBoxItem2);
						goto IL_379;
					}
					goto IL_379;
				}
			}
			else
			{
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && this.SelectionMode == SelectionMode.Extended)
				{
					this.SelectAll();
					goto IL_379;
				}
				flag = false;
				goto IL_379;
			}
			IL_377:
			flag = false;
			IL_379:
			if (flag)
			{
				e.Handled = true;
				return;
			}
			base.OnKeyDown(e);
		}

		// Token: 0x06006E98 RID: 28312 RVA: 0x002D2584 File Offset: 0x002D1584
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.OriginalSource == this && Mouse.Captured == this)
			{
				if (Mouse.LeftButton == MouseButtonState.Pressed)
				{
					base.DoAutoScroll();
				}
				else
				{
					base.ReleaseMouseCapture();
					base.ResetLastMousePosition();
				}
			}
			base.OnMouseMove(e);
		}

		// Token: 0x06006E99 RID: 28313 RVA: 0x002D25BA File Offset: 0x002D15BA
		private static void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				ListBox listBox = (ListBox)sender;
				listBox.ReleaseMouseCapture();
				listBox.ResetLastMousePosition();
			}
		}

		// Token: 0x06006E9A RID: 28314 RVA: 0x002D25D8 File Offset: 0x002D15D8
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			if (!KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				return;
			}
			ListBoxItem listBoxItem = e.NewFocus as ListBoxItem;
			if (listBoxItem != null && ItemsControl.ItemsControlFromItemContainer(listBoxItem) == listBox)
			{
				DependencyObject dependencyObject = e.OldFocus as DependencyObject;
				Visual visual = dependencyObject as Visual;
				if (visual == null)
				{
					ContentElement contentElement = dependencyObject as ContentElement;
					if (contentElement != null)
					{
						visual = KeyboardNavigation.GetParentUIElementFromContentElement(contentElement);
					}
				}
				if ((visual != null && listBox.IsAncestorOf(visual)) || dependencyObject == listBox)
				{
					listBox.LastActionItem = listBoxItem;
					listBox.MakeKeyboardSelection(listBoxItem);
				}
			}
		}

		// Token: 0x06006E9B RID: 28315 RVA: 0x002D2654 File Offset: 0x002D1654
		protected override void OnIsMouseCapturedChanged(DependencyPropertyChangedEventArgs e)
		{
			if (base.IsMouseCaptured)
			{
				if (this._autoScrollTimer == null)
				{
					this._autoScrollTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
					this._autoScrollTimer.Interval = ItemsControl.AutoScrollTimeout;
					this._autoScrollTimer.Tick += this.OnAutoScrollTimeout;
					this._autoScrollTimer.Start();
				}
			}
			else if (this._autoScrollTimer != null)
			{
				this._autoScrollTimer.Stop();
				this._autoScrollTimer = null;
			}
			base.OnIsMouseCapturedChanged(e);
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x002D26D2 File Offset: 0x002D16D2
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListBoxItem;
		}

		// Token: 0x06006E9D RID: 28317 RVA: 0x002D26DD File Offset: 0x002D16DD
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListBoxItem();
		}

		// Token: 0x17001987 RID: 6535
		// (get) Token: 0x06006E9E RID: 28318 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006E9F RID: 28319 RVA: 0x002D26E4 File Offset: 0x002D16E4
		private static void OnQueryStatusSelectAll(object target, CanExecuteRoutedEventArgs args)
		{
			if ((target as ListBox).SelectionMode == SelectionMode.Extended)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x06006EA0 RID: 28320 RVA: 0x002D26FC File Offset: 0x002D16FC
		private static void OnSelectAll(object target, ExecutedRoutedEventArgs args)
		{
			ListBox listBox = target as ListBox;
			if (listBox.SelectionMode == SelectionMode.Extended)
			{
				listBox.SelectAll();
			}
		}

		// Token: 0x06006EA1 RID: 28321 RVA: 0x002D2720 File Offset: 0x002D1720
		internal void NotifyListItemClicked(ListBoxItem item, MouseButton mouseButton)
		{
			if (mouseButton == MouseButton.Left && Mouse.Captured != this)
			{
				Mouse.Capture(this, CaptureMode.SubTree);
				base.SetInitialMousePosition();
			}
			switch (this.SelectionMode)
			{
			case SelectionMode.Single:
				if (!item.IsSelected)
				{
					item.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.TrueBox);
				}
				else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					item.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.FalseBox);
				}
				this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
				return;
			case SelectionMode.Multiple:
				this.MakeToggleSelection(item);
				return;
			case SelectionMode.Extended:
				if (mouseButton != MouseButton.Left)
				{
					if (mouseButton == MouseButton.Right && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == ModifierKeys.None)
					{
						if (item.IsSelected)
						{
							this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
							return;
						}
						this.MakeSingleSelection(item);
					}
					return;
				}
				if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
				{
					this.MakeAnchorSelection(item, false);
					return;
				}
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					this.MakeToggleSelection(item);
					return;
				}
				if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					this.MakeAnchorSelection(item, true);
					return;
				}
				this.MakeSingleSelection(item);
				return;
			default:
				return;
			}
		}

		// Token: 0x06006EA2 RID: 28322 RVA: 0x002D2817 File Offset: 0x002D1817
		internal void NotifyListItemMouseDragged(ListBoxItem listItem)
		{
			if (Mouse.Captured == this && base.DidMouseMove())
			{
				base.NavigateToItem(base.ItemInfoFromContainer(listItem), new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers), false);
			}
		}

		// Token: 0x06006EA3 RID: 28323 RVA: 0x002D2848 File Offset: 0x002D1848
		private void UpdateAnchorAndActionItem(ItemsControl.ItemInfo info)
		{
			object item = info.Item;
			ListBoxItem listBoxItem = info.Container as ListBoxItem;
			if (item == DependencyProperty.UnsetValue)
			{
				this.AnchorItemInternal = null;
				this.LastActionItem = null;
			}
			else
			{
				this.AnchorItemInternal = info;
				this.LastActionItem = listBoxItem;
			}
			KeyboardNavigation.SetTabOnceActiveElement(this, listBoxItem);
		}

		// Token: 0x06006EA4 RID: 28324 RVA: 0x002D2894 File Offset: 0x002D1894
		private void MakeSingleSelection(ListBoxItem listItem)
		{
			if (ItemsControl.ItemsControlFromItemContainer(listItem) == this)
			{
				ItemsControl.ItemInfo info = base.ItemInfoFromContainer(listItem);
				base.SelectionChange.SelectJustThisItem(info, true);
				listItem.Focus();
				this.UpdateAnchorAndActionItem(info);
			}
		}

		// Token: 0x06006EA5 RID: 28325 RVA: 0x002D28D0 File Offset: 0x002D18D0
		private void MakeToggleSelection(ListBoxItem item)
		{
			bool value = !item.IsSelected;
			item.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.Box(value));
			this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
		}

		// Token: 0x06006EA6 RID: 28326 RVA: 0x002D2908 File Offset: 0x002D1908
		private void MakeAnchorSelection(ListBoxItem actionItem, bool clearCurrent)
		{
			ItemsControl.ItemInfo anchorItemInternal = this.AnchorItemInternal;
			if (anchorItemInternal == null)
			{
				if (this._selectedItems.Count > 0)
				{
					this.AnchorItemInternal = this._selectedItems[this._selectedItems.Count - 1];
				}
				else
				{
					this.AnchorItemInternal = base.NewItemInfo(base.Items[0], null, 0);
				}
				if ((anchorItemInternal = this.AnchorItemInternal) == null)
				{
					return;
				}
			}
			int num = this.ElementIndex(actionItem);
			int num2 = this.AnchorItemInternal.Index;
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			bool flag = false;
			if (!base.SelectionChange.IsActive)
			{
				flag = true;
				base.SelectionChange.Begin();
			}
			try
			{
				if (clearCurrent)
				{
					for (int i = 0; i < this._selectedItems.Count; i++)
					{
						ItemsControl.ItemInfo itemInfo = this._selectedItems[i];
						int index = itemInfo.Index;
						if (index < num || num2 < index)
						{
							base.SelectionChange.Unselect(itemInfo);
						}
					}
				}
				IEnumerator enumerator = ((IEnumerable)base.Items).GetEnumerator();
				for (int j = 0; j <= num2; j++)
				{
					enumerator.MoveNext();
					if (j >= num)
					{
						base.SelectionChange.Select(base.NewItemInfo(enumerator.Current, null, j), true);
					}
				}
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			finally
			{
				if (flag)
				{
					base.SelectionChange.End();
				}
			}
			this.LastActionItem = actionItem;
			GC.KeepAlive(anchorItemInternal);
		}

		// Token: 0x06006EA7 RID: 28327 RVA: 0x002D2A90 File Offset: 0x002D1A90
		private void MakeKeyboardSelection(ListBoxItem item)
		{
			if (item == null)
			{
				return;
			}
			switch (this.SelectionMode)
			{
			case SelectionMode.Single:
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					this.MakeSingleSelection(item);
					return;
				}
				break;
			case SelectionMode.Multiple:
				this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
				return;
			case SelectionMode.Extended:
				if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					bool clearCurrent = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None;
					this.MakeAnchorSelection(item, clearCurrent);
					return;
				}
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					this.MakeSingleSelection(item);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06006EA8 RID: 28328 RVA: 0x002D2B08 File Offset: 0x002D1B08
		private int ElementIndex(ListBoxItem listItem)
		{
			return base.ItemContainerGenerator.IndexFromContainer(listItem);
		}

		// Token: 0x06006EA9 RID: 28329 RVA: 0x002D2B16 File Offset: 0x002D1B16
		private ListBoxItem ElementAt(int index)
		{
			return base.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
		}

		// Token: 0x06006EAA RID: 28330 RVA: 0x002D2B29 File Offset: 0x002D1B29
		private object GetWeakReferenceTarget(ref WeakReference weakReference)
		{
			if (weakReference != null)
			{
				return weakReference.Target;
			}
			return null;
		}

		// Token: 0x06006EAB RID: 28331 RVA: 0x002D2B38 File Offset: 0x002D1B38
		private void OnAutoScrollTimeout(object sender, EventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				base.DoAutoScroll();
			}
		}

		// Token: 0x06006EAC RID: 28332 RVA: 0x002D2B48 File Offset: 0x002D1B48
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			bool result = base.FocusItem(info, itemNavigateArgs);
			ListBoxItem listBoxItem = info.Container as ListBoxItem;
			if (listBoxItem != null)
			{
				this.LastActionItem = listBoxItem;
				this.MakeKeyboardSelection(listBoxItem);
			}
			return result;
		}

		// Token: 0x17001988 RID: 6536
		// (get) Token: 0x06006EAD RID: 28333 RVA: 0x002D2B7A File Offset: 0x002D1B7A
		// (set) Token: 0x06006EAE RID: 28334 RVA: 0x002D2B84 File Offset: 0x002D1B84
		protected object AnchorItem
		{
			get
			{
				return this.AnchorItemInternal;
			}
			set
			{
				if (value == null || value == DependencyProperty.UnsetValue)
				{
					this.AnchorItemInternal = null;
					this.LastActionItem = null;
					return;
				}
				ItemsControl.ItemInfo itemInfo = base.NewItemInfo(value, null, -1);
				ListBoxItem listBoxItem = itemInfo.Container as ListBoxItem;
				if (listBoxItem == null)
				{
					throw new InvalidOperationException(SR.Get("ListBoxInvalidAnchorItem", new object[]
					{
						value
					}));
				}
				this.AnchorItemInternal = itemInfo;
				this.LastActionItem = listBoxItem;
			}
		}

		// Token: 0x17001989 RID: 6537
		// (get) Token: 0x06006EAF RID: 28335 RVA: 0x002D2BEC File Offset: 0x002D1BEC
		// (set) Token: 0x06006EB0 RID: 28336 RVA: 0x002D2BF4 File Offset: 0x002D1BF4
		internal ItemsControl.ItemInfo AnchorItemInternal
		{
			get
			{
				return this._anchorItem;
			}
			set
			{
				this._anchorItem = ((value != null) ? value.Clone() : null);
			}
		}

		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x06006EB1 RID: 28337 RVA: 0x002D2C0E File Offset: 0x002D1C0E
		// (set) Token: 0x06006EB2 RID: 28338 RVA: 0x002D2C21 File Offset: 0x002D1C21
		internal ListBoxItem LastActionItem
		{
			get
			{
				return this.GetWeakReferenceTarget(ref this._lastActionItem) as ListBoxItem;
			}
			set
			{
				this._lastActionItem = new WeakReference(value);
			}
		}

		// Token: 0x1700198B RID: 6539
		// (get) Token: 0x06006EB3 RID: 28339 RVA: 0x002D2C2F File Offset: 0x002D1C2F
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ListBox._dType;
			}
		}

		// Token: 0x0400365B RID: 13915
		internal const string ListBoxSelectAllKey = "Ctrl+A";

		// Token: 0x0400365C RID: 13916
		public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(ListBox), new FrameworkPropertyMetadata(SelectionMode.Single, new PropertyChangedCallback(ListBox.OnSelectionModeChanged)), new ValidateValueCallback(ListBox.IsValidSelectionMode));

		// Token: 0x0400365D RID: 13917
		public static readonly DependencyProperty SelectedItemsProperty = Selector.SelectedItemsImplProperty;

		// Token: 0x0400365E RID: 13918
		private ItemsControl.ItemInfo _anchorItem;

		// Token: 0x0400365F RID: 13919
		private WeakReference _lastActionItem;

		// Token: 0x04003660 RID: 13920
		private DispatcherTimer _autoScrollTimer;

		// Token: 0x04003661 RID: 13921
		private static RoutedUICommand SelectAllCommand = new RoutedUICommand(SR.Get("ListBoxSelectAllText"), "SelectAll", typeof(ListBox));

		// Token: 0x04003662 RID: 13922
		private static DependencyObjectType _dType;
	}
}
