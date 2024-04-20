using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007F5 RID: 2037
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeViewItem))]
	[TemplatePart(Name = "ItemsHost", Type = typeof(ItemsPresenter))]
	[TemplatePart(Name = "PART_Header", Type = typeof(FrameworkElement))]
	public class TreeViewItem : HeaderedItemsControl, IHierarchicalVirtualizationAndScrollInfo
	{
		// Token: 0x06007647 RID: 30279 RVA: 0x002EEFFC File Offset: 0x002EDFFC
		static TreeViewItem()
		{
			TreeViewItem.ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			TreeViewItem.CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			TreeViewItem.SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			TreeViewItem.UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(typeof(TreeViewItem)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			TreeViewItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TreeViewItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			Control.IsTabStopProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(TreeViewItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TreeViewItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			EventManager.RegisterClassHandler(typeof(TreeViewItem), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(TreeViewItem.OnRequestBringIntoView));
			EventManager.RegisterClassHandler(typeof(TreeViewItem), Mouse.MouseDownEvent, new MouseButtonEventHandler(TreeViewItem.OnMouseButtonDown), true);
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x06007649 RID: 30281 RVA: 0x002EF2A9 File Offset: 0x002EE2A9
		// (set) Token: 0x0600764A RID: 30282 RVA: 0x002EF2BB File Offset: 0x002EE2BB
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(TreeViewItem.IsExpandedProperty);
			}
			set
			{
				base.SetValue(TreeViewItem.IsExpandedProperty, value);
			}
		}

		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x0600764B RID: 30283 RVA: 0x002CEA4A File Offset: 0x002CDA4A
		private bool CanExpand
		{
			get
			{
				return base.HasItems;
			}
		}

		// Token: 0x0600764C RID: 30284 RVA: 0x002EF2CC File Offset: 0x002EE2CC
		private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem treeViewItem = (TreeViewItem)d;
			bool flag = (bool)e.NewValue;
			TreeView parentTreeView = treeViewItem.ParentTreeView;
			if (parentTreeView != null && !flag)
			{
				parentTreeView.HandleSelectionAndCollapsed(treeViewItem);
			}
			ItemsPresenter itemsHostPresenter = treeViewItem.ItemsHostPresenter;
			if (itemsHostPresenter != null)
			{
				treeViewItem.InvalidateMeasure();
				Helper.InvalidateMeasureOnPath(itemsHostPresenter, treeViewItem, false);
			}
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = UIElementAutomationPeer.FromElement(treeViewItem) as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				treeViewItemAutomationPeer.RaiseExpandCollapseAutomationEvent((bool)e.OldValue, flag);
			}
			if (flag)
			{
				treeViewItem.OnExpanded(new RoutedEventArgs(TreeViewItem.ExpandedEvent, treeViewItem));
			}
			else
			{
				treeViewItem.OnCollapsed(new RoutedEventArgs(TreeViewItem.CollapsedEvent, treeViewItem));
			}
			treeViewItem.UpdateVisualState();
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x0600764D RID: 30285 RVA: 0x002EF36B File Offset: 0x002EE36B
		// (set) Token: 0x0600764E RID: 30286 RVA: 0x002EF37D File Offset: 0x002EE37D
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(TreeViewItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(TreeViewItem.IsSelectedProperty, value);
			}
		}

		// Token: 0x0600764F RID: 30287 RVA: 0x002EF38C File Offset: 0x002EE38C
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem treeViewItem = (TreeViewItem)d;
			bool flag = (bool)e.NewValue;
			treeViewItem.Select(flag);
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = UIElementAutomationPeer.FromElement(treeViewItem) as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				treeViewItemAutomationPeer.RaiseAutomationIsSelectedChanged(flag);
			}
			if (flag)
			{
				treeViewItem.OnSelected(new RoutedEventArgs(TreeViewItem.SelectedEvent, treeViewItem));
			}
			else
			{
				treeViewItem.OnUnselected(new RoutedEventArgs(TreeViewItem.UnselectedEvent, treeViewItem));
			}
			treeViewItem.UpdateVisualState();
		}

		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x06007650 RID: 30288 RVA: 0x002EF3F7 File Offset: 0x002EE3F7
		[ReadOnly(true)]
		[Category("Appearance")]
		[Browsable(false)]
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(TreeViewItem.IsSelectionActiveProperty);
			}
		}

		// Token: 0x1400014A RID: 330
		// (add) Token: 0x06007651 RID: 30289 RVA: 0x002EF409 File Offset: 0x002EE409
		// (remove) Token: 0x06007652 RID: 30290 RVA: 0x002EF417 File Offset: 0x002EE417
		[Category("Behavior")]
		public event RoutedEventHandler Expanded
		{
			add
			{
				base.AddHandler(TreeViewItem.ExpandedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.ExpandedEvent, value);
			}
		}

		// Token: 0x06007653 RID: 30291 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnExpanded(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x1400014B RID: 331
		// (add) Token: 0x06007654 RID: 30292 RVA: 0x002EF425 File Offset: 0x002EE425
		// (remove) Token: 0x06007655 RID: 30293 RVA: 0x002EF433 File Offset: 0x002EE433
		[Category("Behavior")]
		public event RoutedEventHandler Collapsed
		{
			add
			{
				base.AddHandler(TreeViewItem.CollapsedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.CollapsedEvent, value);
			}
		}

		// Token: 0x06007656 RID: 30294 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnCollapsed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x1400014C RID: 332
		// (add) Token: 0x06007657 RID: 30295 RVA: 0x002EF441 File Offset: 0x002EE441
		// (remove) Token: 0x06007658 RID: 30296 RVA: 0x002EF44F File Offset: 0x002EE44F
		[Category("Behavior")]
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(TreeViewItem.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.SelectedEvent, value);
			}
		}

		// Token: 0x06007659 RID: 30297 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x1400014D RID: 333
		// (add) Token: 0x0600765A RID: 30298 RVA: 0x002EF45D File Offset: 0x002EE45D
		// (remove) Token: 0x0600765B RID: 30299 RVA: 0x002EF46B File Offset: 0x002EE46B
		[Category("Behavior")]
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(TreeViewItem.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.UnselectedEvent, value);
			}
		}

		// Token: 0x0600765C RID: 30300 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x0600765D RID: 30301 RVA: 0x002EF479 File Offset: 0x002EE479
		public void ExpandSubtree()
		{
			TreeViewItem.ExpandRecursive(this);
		}

		// Token: 0x0600765E RID: 30302 RVA: 0x002EF481 File Offset: 0x002EE481
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			arrangeSize = base.ArrangeOverride(arrangeSize);
			Helper.ComputeCorrectionFactor(this.ParentTreeView, this, base.ItemsHost, this.HeaderElement);
			return arrangeSize;
		}

		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x0600765F RID: 30303 RVA: 0x002C5E8B File Offset: 0x002C4E8B
		// (set) Token: 0x06007660 RID: 30304 RVA: 0x002C5E98 File Offset: 0x002C4E98
		HierarchicalVirtualizationConstraints IHierarchicalVirtualizationAndScrollInfo.Constraints
		{
			get
			{
				return GroupItem.HierarchicalVirtualizationConstraintsField.GetValue(this);
			}
			set
			{
				if (value.CacheLengthUnit == VirtualizationCacheLengthUnit.Page)
				{
					throw new InvalidOperationException(SR.Get("PageCacheSizeNotAllowed"));
				}
				GroupItem.HierarchicalVirtualizationConstraintsField.SetValue(this, value);
			}
		}

		// Token: 0x17001B7A RID: 7034
		// (get) Token: 0x06007661 RID: 30305 RVA: 0x002EF4A8 File Offset: 0x002EE4A8
		HierarchicalVirtualizationHeaderDesiredSizes IHierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes
		{
			get
			{
				FrameworkElement headerElement = this.HeaderElement;
				Size pixelSize = (base.IsVisible && headerElement != null) ? headerElement.DesiredSize : default(Size);
				Helper.ApplyCorrectionFactorToPixelHeaderSize(this.ParentTreeView, this, base.ItemsHost, ref pixelSize);
				return new HierarchicalVirtualizationHeaderDesiredSizes(new Size((double)(DoubleUtil.GreaterThan(pixelSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(pixelSize.Height, 0.0) ? 1 : 0)), pixelSize);
			}
		}

		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x06007662 RID: 30306 RVA: 0x002EF530 File Offset: 0x002EE530
		// (set) Token: 0x06007663 RID: 30307 RVA: 0x002C5F53 File Offset: 0x002C4F53
		HierarchicalVirtualizationItemDesiredSizes IHierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes
		{
			get
			{
				return Helper.ApplyCorrectionFactorToItemDesiredSizes(this, base.ItemsHost);
			}
			set
			{
				GroupItem.HierarchicalVirtualizationItemDesiredSizesField.SetValue(this, value);
			}
		}

		// Token: 0x17001B7C RID: 7036
		// (get) Token: 0x06007664 RID: 30308 RVA: 0x002EF53E File Offset: 0x002EE53E
		Panel IHierarchicalVirtualizationAndScrollInfo.ItemsHost
		{
			get
			{
				return base.ItemsHost;
			}
		}

		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x06007665 RID: 30309 RVA: 0x002C5F69 File Offset: 0x002C4F69
		// (set) Token: 0x06007666 RID: 30310 RVA: 0x002C5F76 File Offset: 0x002C4F76
		bool IHierarchicalVirtualizationAndScrollInfo.MustDisableVirtualization
		{
			get
			{
				return GroupItem.MustDisableVirtualizationField.GetValue(this);
			}
			set
			{
				GroupItem.MustDisableVirtualizationField.SetValue(this, value);
			}
		}

		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x06007667 RID: 30311 RVA: 0x002C5F84 File Offset: 0x002C4F84
		// (set) Token: 0x06007668 RID: 30312 RVA: 0x002C5F91 File Offset: 0x002C4F91
		bool IHierarchicalVirtualizationAndScrollInfo.InBackgroundLayout
		{
			get
			{
				return GroupItem.InBackgroundLayoutField.GetValue(this);
			}
			set
			{
				GroupItem.InBackgroundLayoutField.SetValue(this, value);
			}
		}

		// Token: 0x17001B7F RID: 7039
		// (get) Token: 0x06007669 RID: 30313 RVA: 0x002EF548 File Offset: 0x002EE548
		internal TreeView ParentTreeView
		{
			get
			{
				for (ItemsControl itemsControl = this.ParentItemsControl; itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
				{
					TreeView treeView = itemsControl as TreeView;
					if (treeView != null)
					{
						return treeView;
					}
				}
				return null;
			}
		}

		// Token: 0x17001B80 RID: 7040
		// (get) Token: 0x0600766A RID: 30314 RVA: 0x002EF575 File Offset: 0x002EE575
		internal TreeViewItem ParentTreeViewItem
		{
			get
			{
				return this.ParentItemsControl as TreeViewItem;
			}
		}

		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x0600766B RID: 30315 RVA: 0x002EF582 File Offset: 0x002EE582
		internal ItemsControl ParentItemsControl
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this);
			}
		}

		// Token: 0x0600766C RID: 30316 RVA: 0x002EF58A File Offset: 0x002EE58A
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			if (VisualTreeHelper.GetParent(this) != null && this.IsSelected)
			{
				this.Select(true);
			}
			base.OnVisualParentChanged(oldParent);
		}

		// Token: 0x0600766D RID: 30317 RVA: 0x002EF5AC File Offset: 0x002EE5AC
		private void Select(bool selected)
		{
			TreeView parentTreeView = this.ParentTreeView;
			ItemsControl parentItemsControl = this.ParentItemsControl;
			if (parentTreeView != null && parentItemsControl != null && !parentTreeView.IsSelectionChangeActive)
			{
				object itemOrContainerFromContainer = parentItemsControl.GetItemOrContainerFromContainer(this);
				parentTreeView.ChangeSelection(itemOrContainerFromContainer, this, selected);
				if (selected && parentTreeView.IsKeyboardFocusWithin && !base.IsKeyboardFocusWithin)
				{
					base.Focus();
				}
			}
		}

		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x0600766E RID: 30318 RVA: 0x002EF600 File Offset: 0x002EE600
		// (set) Token: 0x0600766F RID: 30319 RVA: 0x002EF60D File Offset: 0x002EE60D
		private bool ContainsSelection
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.ContainsSelection);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.ContainsSelection, value);
			}
		}

		// Token: 0x06007670 RID: 30320 RVA: 0x002EF61C File Offset: 0x002EE61C
		internal void UpdateContainsSelection(bool selected)
		{
			for (TreeViewItem parentTreeViewItem = this.ParentTreeViewItem; parentTreeViewItem != null; parentTreeViewItem = parentTreeViewItem.ParentTreeViewItem)
			{
				parentTreeViewItem.ContainsSelection = selected;
			}
		}

		// Token: 0x06007671 RID: 30321 RVA: 0x002EF643 File Offset: 0x002EE643
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			this.Select(true);
			base.OnGotFocus(e);
		}

		// Token: 0x06007672 RID: 30322 RVA: 0x002EF654 File Offset: 0x002EE654
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled && base.IsEnabled)
			{
				bool isFocused = base.IsFocused;
				if (base.Focus())
				{
					if (isFocused && !this.IsSelected)
					{
						this.Select(true);
					}
					e.Handled = true;
				}
				if (e.ClickCount % 2 == 0)
				{
					base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.Box(!this.IsExpanded));
					e.Handled = true;
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x002EF6CC File Offset: 0x002EE6CC
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				Key key = e.Key;
				switch (key)
				{
				case Key.Left:
				case Key.Right:
					if (this.LogicalLeft(e.Key))
					{
						if (!TreeViewItem.IsControlKeyDown && this.CanExpandOnInput && this.IsExpanded)
						{
							if (base.IsFocused)
							{
								base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.FalseBox);
							}
							else
							{
								base.Focus();
							}
							e.Handled = true;
							return;
						}
					}
					else if (!TreeViewItem.IsControlKeyDown && this.CanExpandOnInput)
					{
						if (!this.IsExpanded)
						{
							base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
							e.Handled = true;
							return;
						}
						if (this.HandleDownKey(e))
						{
							e.Handled = true;
							return;
						}
					}
					break;
				case Key.Up:
					if (!TreeViewItem.IsControlKeyDown && this.HandleUpKey(e))
					{
						e.Handled = true;
					}
					break;
				case Key.Down:
					if (!TreeViewItem.IsControlKeyDown && this.HandleDownKey(e))
					{
						e.Handled = true;
						return;
					}
					break;
				default:
					if (key != Key.Add)
					{
						if (key != Key.Subtract)
						{
							return;
						}
						if (this.CanExpandOnInput && this.IsExpanded)
						{
							base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.FalseBox);
							e.Handled = true;
							return;
						}
					}
					else if (this.CanExpandOnInput && !this.IsExpanded)
					{
						base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
						e.Handled = true;
						return;
					}
					break;
				}
			}
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x002EF838 File Offset: 0x002EE838
		private bool LogicalLeft(Key key)
		{
			bool flag = base.FlowDirection == FlowDirection.RightToLeft;
			return (!flag && key == Key.Left) || (flag && key == Key.Right);
		}

		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x06007675 RID: 30325 RVA: 0x002A48BF File Offset: 0x002A38BF
		private static bool IsControlKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			}
		}

		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x06007676 RID: 30326 RVA: 0x002EF863 File Offset: 0x002EE863
		private bool CanExpandOnInput
		{
			get
			{
				return this.CanExpand && base.IsEnabled;
			}
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x002EF875 File Offset: 0x002EE875
		internal bool HandleUpKey(KeyEventArgs e)
		{
			return this.HandleUpDownKey(true, e);
		}

		// Token: 0x06007678 RID: 30328 RVA: 0x002EF87F File Offset: 0x002EE87F
		internal bool HandleDownKey(KeyEventArgs e)
		{
			return this.HandleUpDownKey(false, e);
		}

		// Token: 0x06007679 RID: 30329 RVA: 0x002EF88C File Offset: 0x002EE88C
		private bool HandleUpDownKey(bool up, KeyEventArgs e)
		{
			FocusNavigationDirection direction = up ? FocusNavigationDirection.Up : FocusNavigationDirection.Down;
			if (this.AllowHandleKeyEvent(direction))
			{
				TreeView parentTreeView = this.ParentTreeView;
				IInputElement focusedElement = Keyboard.FocusedElement;
				if (parentTreeView != null)
				{
					FrameworkElement frameworkElement = this.HeaderElement;
					if (frameworkElement == null)
					{
						frameworkElement = this;
					}
					ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
					ItemsControl.ItemInfo startingInfo = (itemsControl != null) ? itemsControl.ItemInfoFromContainer(this) : null;
					return parentTreeView.NavigateByLine(startingInfo, frameworkElement, direction, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
				}
			}
			return false;
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x002EF8F8 File Offset: 0x002EE8F8
		private bool AllowHandleKeyEvent(FocusNavigationDirection direction)
		{
			if (!this.IsSelected)
			{
				return false;
			}
			DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
			if (dependencyObject != null)
			{
				DependencyObject dependencyObject2 = UIElementHelper.PredictFocus(dependencyObject, direction);
				if (dependencyObject2 != dependencyObject)
				{
					while (dependencyObject2 != null)
					{
						TreeViewItem treeViewItem = dependencyObject2 as TreeViewItem;
						if (treeViewItem == this)
						{
							return false;
						}
						if (treeViewItem != null || dependencyObject2 is TreeView)
						{
							return true;
						}
						dependencyObject2 = KeyboardNavigation.GetParent(dependencyObject2);
					}
				}
			}
			return true;
		}

		// Token: 0x0600767B RID: 30331 RVA: 0x002EF950 File Offset: 0x002EE950
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			TreeView parentTreeView = ((TreeViewItem)sender).ParentTreeView;
			if (parentTreeView != null)
			{
				parentTreeView.HandleMouseButtonDown();
			}
		}

		// Token: 0x0600767C RID: 30332 RVA: 0x002EF972 File Offset: 0x002EE972
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			if (e.TargetObject == sender)
			{
				((TreeViewItem)sender).HandleBringIntoView(e);
			}
		}

		// Token: 0x0600767D RID: 30333 RVA: 0x002EF98C File Offset: 0x002EE98C
		private void HandleBringIntoView(RequestBringIntoViewEventArgs e)
		{
			for (TreeViewItem parentTreeViewItem = this.ParentTreeViewItem; parentTreeViewItem != null; parentTreeViewItem = parentTreeViewItem.ParentTreeViewItem)
			{
				if (!parentTreeViewItem.IsExpanded)
				{
					parentTreeViewItem.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
				}
			}
			if (e.TargetRect.IsEmpty)
			{
				FrameworkElement headerElement = this.HeaderElement;
				if (headerElement != null)
				{
					e.Handled = true;
					headerElement.BringIntoView();
					return;
				}
				base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.BringItemIntoView), null);
			}
		}

		// Token: 0x0600767E RID: 30334 RVA: 0x002EFA08 File Offset: 0x002EEA08
		private object BringItemIntoView(object args)
		{
			FrameworkElement headerElement = this.HeaderElement;
			if (headerElement != null)
			{
				headerElement.BringIntoView();
			}
			return null;
		}

		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x0600767F RID: 30335 RVA: 0x002EFA26 File Offset: 0x002EEA26
		internal FrameworkElement HeaderElement
		{
			get
			{
				return base.GetTemplateChild("PART_Header") as FrameworkElement;
			}
		}

		// Token: 0x06007680 RID: 30336 RVA: 0x002EFA38 File Offset: 0x002EEA38
		internal FrameworkElement TryGetHeaderElement()
		{
			FrameworkElement frameworkElement = this.HeaderElement;
			if (frameworkElement != null)
			{
				return frameworkElement;
			}
			FrameworkTemplate templateInternal = this.TemplateInternal;
			if (templateInternal == null)
			{
				return this;
			}
			int i = StyleHelper.QueryChildIndexFromChildName("PART_Header", templateInternal.ChildIndexFromChildName);
			if (i < 0)
			{
				ToggleButton toggleButton = Helper.FindTemplatedDescendant<ToggleButton>(this, this);
				if (toggleButton != null)
				{
					FrameworkElement frameworkElement2 = VisualTreeHelper.GetParent(toggleButton) as FrameworkElement;
					if (frameworkElement2 != null)
					{
						int childrenCount = VisualTreeHelper.GetChildrenCount(frameworkElement2);
						i = 0;
						while (i < childrenCount - 1)
						{
							if (VisualTreeHelper.GetChild(frameworkElement2, i) == toggleButton)
							{
								frameworkElement = (VisualTreeHelper.GetChild(frameworkElement2, i + 1) as FrameworkElement);
								if (frameworkElement != null)
								{
									return frameworkElement;
								}
								break;
							}
							else
							{
								i++;
							}
						}
					}
				}
			}
			return this;
		}

		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x06007681 RID: 30337 RVA: 0x002EFAC7 File Offset: 0x002EEAC7
		private ItemsPresenter ItemsHostPresenter
		{
			get
			{
				return base.GetTemplateChild("ItemsHost") as ItemsPresenter;
			}
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x002EEA81 File Offset: 0x002EDA81
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeViewItem;
		}

		// Token: 0x06007683 RID: 30339 RVA: 0x002EEA8C File Offset: 0x002EDA8C
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeViewItem();
		}

		// Token: 0x06007684 RID: 30340 RVA: 0x002EFAD9 File Offset: 0x002EEAD9
		internal void PrepareItemContainer(object item, ItemsControl parentItemsControl)
		{
			Helper.ClearVirtualizingElement(this);
			TreeViewItem.IsVirtualizingPropagationHelper(parentItemsControl, this);
			if (VirtualizingPanel.GetIsVirtualizing(parentItemsControl))
			{
				Helper.SetItemValuesOnContainer(parentItemsControl, this, item);
			}
		}

		// Token: 0x06007685 RID: 30341 RVA: 0x002EFAF8 File Offset: 0x002EEAF8
		internal void ClearItemContainer(object item, ItemsControl parentItemsControl)
		{
			if (VirtualizingPanel.GetIsVirtualizing(parentItemsControl))
			{
				Helper.StoreItemValues(parentItemsControl, this, item);
				VirtualizingPanel virtualizingPanel = base.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.OnClearChildrenInternal();
				}
				base.ItemContainerGenerator.RemoveAllInternal(true);
			}
			this.ContainsSelection = false;
		}

		// Token: 0x06007686 RID: 30342 RVA: 0x002EFB3D File Offset: 0x002EEB3D
		internal static void IsVirtualizingPropagationHelper(DependencyObject parent, DependencyObject element)
		{
			TreeViewItem.SynchronizeValue(VirtualizingPanel.IsVirtualizingProperty, parent, element);
			TreeViewItem.SynchronizeValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty, parent, element);
			TreeViewItem.SynchronizeValue(VirtualizingPanel.VirtualizationModeProperty, parent, element);
			TreeViewItem.SynchronizeValue(VirtualizingPanel.ScrollUnitProperty, parent, element);
		}

		// Token: 0x06007687 RID: 30343 RVA: 0x002EFB70 File Offset: 0x002EEB70
		internal static void SynchronizeValue(DependencyProperty dp, DependencyObject parent, DependencyObject child)
		{
			object value = parent.GetValue(dp);
			child.SetValue(dp, value);
		}

		// Token: 0x06007688 RID: 30344 RVA: 0x002EFB90 File Offset: 0x002EEB90
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Remove:
			case NotifyCollectionChangedAction.Reset:
				if (this.ContainsSelection)
				{
					TreeView parentTreeView = this.ParentTreeView;
					if (parentTreeView != null && !parentTreeView.IsSelectedContainerHookedUp)
					{
						this.ContainsSelection = false;
						this.Select(true);
						return;
					}
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this.ContainsSelection)
				{
					TreeView parentTreeView2 = this.ParentTreeView;
					if (parentTreeView2 != null)
					{
						object selectedItem = parentTreeView2.SelectedItem;
						if (selectedItem != null && selectedItem.Equals(e.OldItems[0]))
						{
							parentTreeView2.ChangeSelection(selectedItem, parentTreeView2.SelectedContainer, false);
							return;
						}
					}
				}
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x06007689 RID: 30345 RVA: 0x002EFC50 File Offset: 0x002EEC50
		private static void ExpandRecursive(TreeViewItem item)
		{
			if (item == null)
			{
				return;
			}
			if (!item.IsExpanded)
			{
				item.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
			}
			item.ApplyTemplate();
			ItemsPresenter itemsPresenter = (ItemsPresenter)item.Template.FindName("ItemsHost", item);
			if (itemsPresenter != null)
			{
				itemsPresenter.ApplyTemplate();
			}
			else
			{
				item.UpdateLayout();
			}
			VirtualizingPanel virtualizingPanel = item.ItemsHost as VirtualizingPanel;
			item.ItemsHost.EnsureGenerator();
			int i = 0;
			int count = item.Items.Count;
			while (i < count)
			{
				TreeViewItem treeViewItem;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.BringIndexIntoView(i);
					treeViewItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
				}
				else
				{
					treeViewItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
					treeViewItem.BringIntoView();
				}
				if (treeViewItem != null)
				{
					TreeViewItem.ExpandRecursive(treeViewItem);
				}
				i++;
			}
		}

		// Token: 0x0600768A RID: 30346 RVA: 0x002EFD1C File Offset: 0x002EED1C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TreeViewItemAutomationPeer(this);
		}

		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x0600768B RID: 30347 RVA: 0x002EFD24 File Offset: 0x002EED24
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TreeViewItem._dType;
			}
		}

		// Token: 0x0600768C RID: 30348 RVA: 0x002EFD2C File Offset: 0x002EED2C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Normal"
				});
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unfocused"
				});
			}
			if (this.IsExpanded)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Expanded"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Collapsed"
				});
			}
			if (base.HasItems)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"HasItems"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"NoItems"
				});
			}
			if (this.IsSelected)
			{
				if (this.IsSelectionActive)
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"Selected"
					});
				}
				else
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SelectedInactive",
						"Selected"
					});
				}
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unselected"
				});
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x04003893 RID: 14483
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(TreeViewItem.OnIsExpandedChanged)));

		// Token: 0x04003894 RID: 14484
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TreeViewItem.OnIsSelectedChanged)));

		// Token: 0x04003895 RID: 14485
		public static readonly DependencyProperty IsSelectionActiveProperty = Selector.IsSelectionActiveProperty.AddOwner(typeof(TreeViewItem));

		// Token: 0x0400389A RID: 14490
		private static DependencyObjectType _dType;

		// Token: 0x0400389B RID: 14491
		private const string HeaderPartName = "PART_Header";

		// Token: 0x0400389C RID: 14492
		private const string ItemsHostPartName = "ItemsHost";
	}
}
