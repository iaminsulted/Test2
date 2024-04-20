using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007F4 RID: 2036
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeViewItem))]
	public class TreeView : ItemsControl
	{
		// Token: 0x0600761E RID: 30238 RVA: 0x002EE50C File Offset: 0x002ED50C
		static TreeView()
		{
			TreeView.SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(TreeView));
			TreeView.SelectedValuePathBindingExpression = new BindingExpressionUncommonField();
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(typeof(TreeView)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			TreeView._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TreeView));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			VirtualizingPanel.ScrollUnitProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(ScrollUnit.Pixel));
			ControlsTraceLogger.AddControl(TelemetryControls.TreeView);
		}

		// Token: 0x0600761F RID: 30239 RVA: 0x002EE6B0 File Offset: 0x002ED6B0
		public TreeView()
		{
			this._focusEnterMainFocusScopeEventHandler = new EventHandler(this.OnFocusEnterMainFocusScope);
			KeyboardNavigation.Current.FocusEnterMainFocusScope += this._focusEnterMainFocusScopeEventHandler;
		}

		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x06007620 RID: 30240 RVA: 0x002EE6E6 File Offset: 0x002ED6E6
		[Category("Appearance")]
		[Bindable(true)]
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				return base.GetValue(TreeView.SelectedItemProperty);
			}
		}

		// Token: 0x06007621 RID: 30241 RVA: 0x002EE6F3 File Offset: 0x002ED6F3
		private void SetSelectedItem(object data)
		{
			if (this.SelectedItem != data)
			{
				base.SetValue(TreeView.SelectedItemPropertyKey, data);
			}
		}

		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x06007622 RID: 30242 RVA: 0x002EE70A File Offset: 0x002ED70A
		[Bindable(true)]
		[Category("Appearance")]
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedValue
		{
			get
			{
				return base.GetValue(TreeView.SelectedValueProperty);
			}
		}

		// Token: 0x06007623 RID: 30243 RVA: 0x002EE717 File Offset: 0x002ED717
		private void SetSelectedValue(object data)
		{
			if (this.SelectedValue != data)
			{
				base.SetValue(TreeView.SelectedValuePropertyKey, data);
			}
		}

		// Token: 0x17001B6D RID: 7021
		// (get) Token: 0x06007624 RID: 30244 RVA: 0x002EE72E File Offset: 0x002ED72E
		// (set) Token: 0x06007625 RID: 30245 RVA: 0x002EE740 File Offset: 0x002ED740
		[Bindable(true)]
		[Category("Appearance")]
		public string SelectedValuePath
		{
			get
			{
				return (string)base.GetValue(TreeView.SelectedValuePathProperty);
			}
			set
			{
				base.SetValue(TreeView.SelectedValuePathProperty, value);
			}
		}

		// Token: 0x06007626 RID: 30246 RVA: 0x002EE750 File Offset: 0x002ED750
		private static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeView treeView = (TreeView)d;
			TreeView.SelectedValuePathBindingExpression.ClearValue(treeView);
			treeView.UpdateSelectedValue(treeView.SelectedItem);
		}

		// Token: 0x14000149 RID: 329
		// (add) Token: 0x06007627 RID: 30247 RVA: 0x002EE77B File Offset: 0x002ED77B
		// (remove) Token: 0x06007628 RID: 30248 RVA: 0x002EE789 File Offset: 0x002ED789
		[Category("Behavior")]
		public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged
		{
			add
			{
				base.AddHandler(TreeView.SelectedItemChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeView.SelectedItemChangedEvent, value);
			}
		}

		// Token: 0x06007629 RID: 30249 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x0600762A RID: 30250 RVA: 0x002EE798 File Offset: 0x002ED798
		internal void ChangeSelection(object data, TreeViewItem container, bool selected)
		{
			if (this.IsSelectionChangeActive)
			{
				return;
			}
			object oldValue = null;
			object newValue = null;
			bool flag = false;
			TreeViewItem selectedContainer = this._selectedContainer;
			this.IsSelectionChangeActive = true;
			try
			{
				if (selected)
				{
					if (container != this._selectedContainer)
					{
						oldValue = this.SelectedItem;
						newValue = data;
						if (this._selectedContainer != null)
						{
							this._selectedContainer.IsSelected = false;
							this._selectedContainer.UpdateContainsSelection(false);
						}
						this._selectedContainer = container;
						this._selectedContainer.UpdateContainsSelection(true);
						this.SetSelectedItem(data);
						this.UpdateSelectedValue(data);
						flag = true;
					}
				}
				else if (container == this._selectedContainer)
				{
					this._selectedContainer.UpdateContainsSelection(false);
					this._selectedContainer = null;
					this.SetSelectedItem(null);
					this.UpdateSelectedValue(null);
					oldValue = data;
					flag = true;
				}
				if (container.IsSelected != selected)
				{
					container.IsSelected = selected;
				}
			}
			finally
			{
				this.IsSelectionChangeActive = false;
			}
			if (flag)
			{
				if (this._selectedContainer != null && AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected))
				{
					TreeViewItemAutomationPeer treeViewItemAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this._selectedContainer) as TreeViewItemAutomationPeer;
					if (treeViewItemAutomationPeer != null)
					{
						treeViewItemAutomationPeer.RaiseAutomationSelectionEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					}
				}
				if (selectedContainer != null && AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					TreeViewItemAutomationPeer treeViewItemAutomationPeer2 = UIElementAutomationPeer.CreatePeerForElement(selectedContainer) as TreeViewItemAutomationPeer;
					if (treeViewItemAutomationPeer2 != null)
					{
						treeViewItemAutomationPeer2.RaiseAutomationSelectionEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
					}
				}
				RoutedPropertyChangedEventArgs<object> e = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, TreeView.SelectedItemChangedEvent);
				this.OnSelectedItemChanged(e);
			}
		}

		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x0600762B RID: 30251 RVA: 0x002EE8E0 File Offset: 0x002ED8E0
		// (set) Token: 0x0600762C RID: 30252 RVA: 0x002EE8EE File Offset: 0x002ED8EE
		internal bool IsSelectionChangeActive
		{
			get
			{
				return this._bits[1];
			}
			set
			{
				this._bits[1] = value;
			}
		}

		// Token: 0x0600762D RID: 30253 RVA: 0x002EE900 File Offset: 0x002ED900
		private void UpdateSelectedValue(object selectedItem)
		{
			BindingExpression bindingExpression = this.PrepareSelectedValuePathBindingExpression(selectedItem);
			if (bindingExpression != null)
			{
				bindingExpression.Activate(selectedItem);
				object value = bindingExpression.Value;
				bindingExpression.Deactivate();
				base.SetValue(TreeView.SelectedValuePropertyKey, value);
				return;
			}
			base.ClearValue(TreeView.SelectedValuePropertyKey);
		}

		// Token: 0x0600762E RID: 30254 RVA: 0x002EE944 File Offset: 0x002ED944
		private BindingExpression PrepareSelectedValuePathBindingExpression(object item)
		{
			if (item == null)
			{
				return null;
			}
			bool flag = SystemXmlHelper.IsXmlNode(item);
			BindingExpression bindingExpression = TreeView.SelectedValuePathBindingExpression.GetValue(this);
			if (bindingExpression != null)
			{
				Binding binding = bindingExpression.ParentBinding;
				if (binding.XPath != null != flag)
				{
					bindingExpression = null;
				}
			}
			if (bindingExpression == null)
			{
				Binding binding = new Binding();
				binding.Source = null;
				if (flag)
				{
					binding.XPath = this.SelectedValuePath;
					binding.Path = new PropertyPath("/InnerText", Array.Empty<object>());
				}
				else
				{
					binding.Path = new PropertyPath(this.SelectedValuePath, Array.Empty<object>());
				}
				bindingExpression = (BindingExpression)BindingExpressionBase.CreateUntargetedBindingExpression(this, binding);
				TreeView.SelectedValuePathBindingExpression.SetValue(this, bindingExpression);
			}
			return bindingExpression;
		}

		// Token: 0x0600762F RID: 30255 RVA: 0x002EE9E8 File Offset: 0x002ED9E8
		internal void HandleSelectionAndCollapsed(TreeViewItem collapsed)
		{
			if (this._selectedContainer != null && this._selectedContainer != collapsed)
			{
				TreeViewItem treeViewItem = this._selectedContainer;
				for (;;)
				{
					treeViewItem = treeViewItem.ParentTreeViewItem;
					if (treeViewItem == collapsed)
					{
						break;
					}
					if (treeViewItem == null)
					{
						return;
					}
				}
				UIElement selectedContainer = this._selectedContainer;
				this.ChangeSelection(collapsed.ParentItemsControl.ItemContainerGenerator.ItemFromContainer(collapsed), collapsed, true);
				if (selectedContainer.IsKeyboardFocusWithin)
				{
					this._selectedContainer.Focus();
					return;
				}
			}
		}

		// Token: 0x06007630 RID: 30256 RVA: 0x002EEA4E File Offset: 0x002EDA4E
		internal void HandleMouseButtonDown()
		{
			if (!base.IsKeyboardFocusWithin)
			{
				if (this._selectedContainer != null)
				{
					if (!this._selectedContainer.IsKeyboardFocused)
					{
						this._selectedContainer.Focus();
						return;
					}
				}
				else
				{
					base.Focus();
				}
			}
		}

		// Token: 0x06007631 RID: 30257 RVA: 0x002EEA81 File Offset: 0x002EDA81
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeViewItem;
		}

		// Token: 0x06007632 RID: 30258 RVA: 0x002EEA8C File Offset: 0x002EDA8C
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeViewItem();
		}

		// Token: 0x06007633 RID: 30259 RVA: 0x002EEA94 File Offset: 0x002EDA94
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Remove:
			case NotifyCollectionChangedAction.Reset:
				if (this.SelectedItem != null && !this.IsSelectedContainerHookedUp)
				{
					this.SelectFirstItem();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
			{
				object selectedItem = this.SelectedItem;
				if (selectedItem != null && selectedItem.Equals(e.OldItems[0]))
				{
					this.ChangeSelection(selectedItem, this._selectedContainer, false);
					return;
				}
				break;
			}
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x06007634 RID: 30260 RVA: 0x002EEB2C File Offset: 0x002EDB2C
		private void SelectFirstItem()
		{
			object selectedItem;
			TreeViewItem selectedContainer;
			bool firstItem = this.GetFirstItem(out selectedItem, out selectedContainer);
			if (!firstItem)
			{
				selectedItem = this.SelectedItem;
				selectedContainer = this._selectedContainer;
			}
			this.ChangeSelection(selectedItem, selectedContainer, firstItem);
		}

		// Token: 0x06007635 RID: 30261 RVA: 0x002EEB5E File Offset: 0x002EDB5E
		private bool GetFirstItem(out object item, out TreeViewItem container)
		{
			if (base.HasItems)
			{
				item = base.Items[0];
				container = (base.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem);
				return item != null && container != null;
			}
			item = null;
			container = null;
			return false;
		}

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x06007636 RID: 30262 RVA: 0x002EEB9C File Offset: 0x002EDB9C
		internal bool IsSelectedContainerHookedUp
		{
			get
			{
				return this._selectedContainer != null && this._selectedContainer.ParentTreeView == this;
			}
		}

		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x06007637 RID: 30263 RVA: 0x002EEBB6 File Offset: 0x002EDBB6
		internal TreeViewItem SelectedContainer
		{
			get
			{
				return this._selectedContainer;
			}
		}

		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x06007638 RID: 30264 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007639 RID: 30265 RVA: 0x002EEBC0 File Offset: 0x002EDBC0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				if (TreeView.IsControlKeyDown)
				{
					Key key = e.Key;
					if (key - Key.Prior <= 7 && this.HandleScrollKeys(e.Key))
					{
						e.Handled = true;
						return;
					}
				}
				else
				{
					Key key = e.Key;
					if (key != Key.Tab)
					{
						switch (key)
						{
						case Key.Prior:
						case Key.Next:
							if (this._selectedContainer == null)
							{
								if (this.FocusFirstItem())
								{
									e.Handled = true;
									return;
								}
							}
							else if (this.HandleScrollByPage(e))
							{
								e.Handled = true;
								return;
							}
							break;
						case Key.End:
							if (this.FocusLastItem())
							{
								e.Handled = true;
								return;
							}
							break;
						case Key.Home:
							if (this.FocusFirstItem())
							{
								e.Handled = true;
								return;
							}
							break;
						case Key.Left:
						case Key.Right:
							break;
						case Key.Up:
						case Key.Down:
							if (this._selectedContainer == null && this.FocusFirstItem())
							{
								e.Handled = true;
								return;
							}
							break;
						default:
							if (key != Key.Multiply)
							{
								return;
							}
							if (this.ExpandSubtree(this._selectedContainer))
							{
								e.Handled = true;
							}
							break;
						}
					}
					else if (TreeView.IsShiftKeyDown && base.IsKeyboardFocusWithin && this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous)))
					{
						e.Handled = true;
						return;
					}
				}
			}
		}

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600763A RID: 30266 RVA: 0x002A48BF File Offset: 0x002A38BF
		private static bool IsControlKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			}
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x0600763B RID: 30267 RVA: 0x002EECF1 File Offset: 0x002EDCF1
		private static bool IsShiftKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
			}
		}

		// Token: 0x0600763C RID: 30268 RVA: 0x002EED00 File Offset: 0x002EDD00
		private bool FocusFirstItem()
		{
			FrameworkElement frameworkElement;
			return base.NavigateToStartInternal(new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers), true, out frameworkElement);
		}

		// Token: 0x0600763D RID: 30269 RVA: 0x002EED28 File Offset: 0x002EDD28
		private bool FocusLastItem()
		{
			FrameworkElement frameworkElement;
			return base.NavigateToEndInternal(new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers), true, out frameworkElement);
		}

		// Token: 0x0600763E RID: 30270 RVA: 0x002EED50 File Offset: 0x002EDD50
		private bool HandleScrollKeys(Key key)
		{
			ScrollViewer scrollHost = base.ScrollHost;
			if (scrollHost != null)
			{
				bool flag = base.FlowDirection == FlowDirection.RightToLeft;
				switch (key)
				{
				case Key.Prior:
					if (DoubleUtil.GreaterThan(scrollHost.ExtentHeight, scrollHost.ViewportHeight))
					{
						scrollHost.PageUp();
					}
					else
					{
						scrollHost.PageLeft();
					}
					return true;
				case Key.Next:
					if (DoubleUtil.GreaterThan(scrollHost.ExtentHeight, scrollHost.ViewportHeight))
					{
						scrollHost.PageDown();
					}
					else
					{
						scrollHost.PageRight();
					}
					return true;
				case Key.End:
					scrollHost.ScrollToBottom();
					return true;
				case Key.Home:
					scrollHost.ScrollToTop();
					return true;
				case Key.Left:
					if (flag)
					{
						scrollHost.LineRight();
					}
					else
					{
						scrollHost.LineLeft();
					}
					return true;
				case Key.Up:
					scrollHost.LineUp();
					return true;
				case Key.Right:
					if (flag)
					{
						scrollHost.LineLeft();
					}
					else
					{
						scrollHost.LineRight();
					}
					return true;
				case Key.Down:
					scrollHost.LineDown();
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600763F RID: 30271 RVA: 0x002EEE30 File Offset: 0x002EDE30
		private bool HandleScrollByPage(KeyEventArgs e)
		{
			IInputElement focusedElement = Keyboard.FocusedElement;
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this._selectedContainer);
			ItemsControl.ItemInfo startingInfo = (itemsControl != null) ? itemsControl.ItemInfoFromContainer(this._selectedContainer) : null;
			FrameworkElement frameworkElement = this._selectedContainer.HeaderElement;
			if (frameworkElement == null)
			{
				frameworkElement = this._selectedContainer;
			}
			return base.NavigateByPage(startingInfo, frameworkElement, (e.Key == Key.Prior) ? FocusNavigationDirection.Up : FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x002EEE9E File Offset: 0x002EDE9E
		protected virtual bool ExpandSubtree(TreeViewItem container)
		{
			if (container != null)
			{
				container.ExpandSubtree();
				return true;
			}
			return false;
		}

		// Token: 0x06007641 RID: 30273 RVA: 0x002EEEAC File Offset: 0x002EDEAC
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			bool flag = false;
			bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
			if (isKeyboardFocusWithin)
			{
				flag = true;
			}
			else
			{
				DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
				if (dependencyObject != null)
				{
					UIElement uielement = KeyboardNavigation.GetVisualRoot(this) as UIElement;
					if (uielement != null && uielement.IsKeyboardFocusWithin && FocusManager.GetFocusScope(dependencyObject) != uielement)
					{
						flag = true;
					}
				}
			}
			if ((bool)base.GetValue(Selector.IsSelectionActiveProperty) != flag)
			{
				base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.Box(flag));
			}
			if (isKeyboardFocusWithin && base.IsKeyboardFocused && this._selectedContainer != null && !this._selectedContainer.IsKeyboardFocusWithin)
			{
				this._selectedContainer.Focus();
			}
		}

		// Token: 0x06007642 RID: 30274 RVA: 0x002EEF4E File Offset: 0x002EDF4E
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			if (base.IsKeyboardFocusWithin && base.IsKeyboardFocused && this._selectedContainer != null && !this._selectedContainer.IsKeyboardFocusWithin)
			{
				this._selectedContainer.Focus();
			}
		}

		// Token: 0x06007643 RID: 30275 RVA: 0x002EEF88 File Offset: 0x002EDF88
		private void OnFocusEnterMainFocusScope(object sender, EventArgs e)
		{
			if (!base.IsKeyboardFocusWithin)
			{
				base.ClearValue(Selector.IsSelectionActivePropertyKey);
			}
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x002EEFA0 File Offset: 0x002EDFA0
		private static DependencyObject FindParent(DependencyObject o)
		{
			Visual visual = o as Visual;
			ContentElement contentElement = (visual == null) ? (o as ContentElement) : null;
			if (contentElement != null)
			{
				o = ContentOperations.GetParent(contentElement);
				if (o != null)
				{
					return o;
				}
				FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					return frameworkContentElement.Parent;
				}
			}
			else if (visual != null)
			{
				return VisualTreeHelper.GetParent(visual);
			}
			return null;
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x002EEFED File Offset: 0x002EDFED
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TreeViewAutomationPeer(this);
		}

		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x06007646 RID: 30278 RVA: 0x002EEFF5 File Offset: 0x002EDFF5
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TreeView._dType;
			}
		}

		// Token: 0x04003888 RID: 14472
		private static readonly DependencyPropertyKey SelectedItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItem", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null));

		// Token: 0x04003889 RID: 14473
		public static readonly DependencyProperty SelectedItemProperty = TreeView.SelectedItemPropertyKey.DependencyProperty;

		// Token: 0x0400388A RID: 14474
		private static readonly DependencyPropertyKey SelectedValuePropertyKey = DependencyProperty.RegisterReadOnly("SelectedValue", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null));

		// Token: 0x0400388B RID: 14475
		public static readonly DependencyProperty SelectedValueProperty = TreeView.SelectedValuePropertyKey.DependencyProperty;

		// Token: 0x0400388C RID: 14476
		public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(TreeView), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(TreeView.OnSelectedValuePathChanged)));

		// Token: 0x0400388E RID: 14478
		private static DependencyObjectType _dType;

		// Token: 0x0400388F RID: 14479
		private BitVector32 _bits = new BitVector32(0);

		// Token: 0x04003890 RID: 14480
		private TreeViewItem _selectedContainer;

		// Token: 0x04003891 RID: 14481
		private static readonly BindingExpressionUncommonField SelectedValuePathBindingExpression;

		// Token: 0x04003892 RID: 14482
		private EventHandler _focusEnterMainFocusScopeEventHandler;

		// Token: 0x02000C2A RID: 3114
		private enum Bits
		{
			// Token: 0x04004B56 RID: 19286
			IsSelectionChangeActive = 1
		}
	}
}
