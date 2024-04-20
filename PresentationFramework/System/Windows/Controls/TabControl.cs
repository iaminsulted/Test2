using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007E1 RID: 2017
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TabItem))]
	[TemplatePart(Name = "PART_SelectedContentHost", Type = typeof(ContentPresenter))]
	public class TabControl : Selector
	{
		// Token: 0x06007400 RID: 29696 RVA: 0x002E5194 File Offset: 0x002E4194
		static TabControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
			TabControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TabControl));
			Control.IsTabStopProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TabControl), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.TabControl);
		}

		// Token: 0x06007402 RID: 29698 RVA: 0x002E53F4 File Offset: 0x002E43F4
		private static void OnTabStripPlacementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TabControl tabControl = (TabControl)d;
			ItemCollection items = tabControl.Items;
			for (int i = 0; i < items.Count; i++)
			{
				TabItem tabItem = tabControl.ItemContainerGenerator.ContainerFromIndex(i) as TabItem;
				if (tabItem != null)
				{
					tabItem.CoerceValue(TabItem.TabStripPlacementProperty);
				}
			}
		}

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x06007403 RID: 29699 RVA: 0x002E5440 File Offset: 0x002E4440
		// (set) Token: 0x06007404 RID: 29700 RVA: 0x002E5452 File Offset: 0x002E4452
		[Bindable(true)]
		[Category("Behavior")]
		public Dock TabStripPlacement
		{
			get
			{
				return (Dock)base.GetValue(TabControl.TabStripPlacementProperty);
			}
			set
			{
				base.SetValue(TabControl.TabStripPlacementProperty, value);
			}
		}

		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x06007405 RID: 29701 RVA: 0x002E5465 File Offset: 0x002E4465
		// (set) Token: 0x06007406 RID: 29702 RVA: 0x002E5472 File Offset: 0x002E4472
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedContent
		{
			get
			{
				return base.GetValue(TabControl.SelectedContentProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentPropertyKey, value);
			}
		}

		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x06007407 RID: 29703 RVA: 0x002E5480 File Offset: 0x002E4480
		// (set) Token: 0x06007408 RID: 29704 RVA: 0x002E5492 File Offset: 0x002E4492
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplate SelectedContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(TabControl.SelectedContentTemplateProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentTemplatePropertyKey, value);
			}
		}

		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x06007409 RID: 29705 RVA: 0x002E54A0 File Offset: 0x002E44A0
		// (set) Token: 0x0600740A RID: 29706 RVA: 0x002E54B2 File Offset: 0x002E44B2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector SelectedContentTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(TabControl.SelectedContentTemplateSelectorProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentTemplateSelectorPropertyKey, value);
			}
		}

		// Token: 0x17001AEF RID: 6895
		// (get) Token: 0x0600740B RID: 29707 RVA: 0x002E54C0 File Offset: 0x002E44C0
		// (set) Token: 0x0600740C RID: 29708 RVA: 0x002E54D2 File Offset: 0x002E44D2
		public string SelectedContentStringFormat
		{
			get
			{
				return (string)base.GetValue(TabControl.SelectedContentStringFormatProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentStringFormatPropertyKey, value);
			}
		}

		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x0600740D RID: 29709 RVA: 0x002E54E0 File Offset: 0x002E44E0
		// (set) Token: 0x0600740E RID: 29710 RVA: 0x002E54F2 File Offset: 0x002E44F2
		public DataTemplate ContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(TabControl.ContentTemplateProperty);
			}
			set
			{
				base.SetValue(TabControl.ContentTemplateProperty, value);
			}
		}

		// Token: 0x17001AF1 RID: 6897
		// (get) Token: 0x0600740F RID: 29711 RVA: 0x002E5500 File Offset: 0x002E4500
		// (set) Token: 0x06007410 RID: 29712 RVA: 0x002E5512 File Offset: 0x002E4512
		public DataTemplateSelector ContentTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(TabControl.ContentTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(TabControl.ContentTemplateSelectorProperty, value);
			}
		}

		// Token: 0x17001AF2 RID: 6898
		// (get) Token: 0x06007411 RID: 29713 RVA: 0x002E5520 File Offset: 0x002E4520
		// (set) Token: 0x06007412 RID: 29714 RVA: 0x002E5532 File Offset: 0x002E4532
		public string ContentStringFormat
		{
			get
			{
				return (string)base.GetValue(TabControl.ContentStringFormatProperty);
			}
			set
			{
				base.SetValue(TabControl.ContentStringFormatProperty, value);
			}
		}

		// Token: 0x06007413 RID: 29715 RVA: 0x002B4D7C File Offset: 0x002B3D7C
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
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06007414 RID: 29716 RVA: 0x002E5540 File Offset: 0x002E4540
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TabControlAutomationPeer(this);
		}

		// Token: 0x06007415 RID: 29717 RVA: 0x002E5548 File Offset: 0x002E4548
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			base.CanSelectMultiple = false;
			base.ItemContainerGenerator.StatusChanged += this.OnGeneratorStatusChanged;
		}

		// Token: 0x06007416 RID: 29718 RVA: 0x002E556F File Offset: 0x002E456F
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdateSelectedContent();
		}

		// Token: 0x06007417 RID: 29719 RVA: 0x002E5580 File Offset: 0x002E4580
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			if (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
			{
				base.OnSelectionChanged(e);
				if (base.IsKeyboardFocusWithin)
				{
					TabItem selectedTabItem = this.GetSelectedTabItem();
					if (selectedTabItem != null)
					{
						selectedTabItem.SetFocus();
					}
				}
				this.UpdateSelectedContent();
			}
			else
			{
				bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
				this.UpdateSelectedContent();
				if (isKeyboardFocusWithin)
				{
					TabItem selectedTabItem2 = this.GetSelectedTabItem();
					if (selectedTabItem2 != null)
					{
						selectedTabItem2.SetFocus();
					}
				}
				base.OnSelectionChanged(e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionPatternOnInvalidated) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				TabControlAutomationPeer tabControlAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this) as TabControlAutomationPeer;
				if (tabControlAutomationPeer != null)
				{
					tabControlAutomationPeer.RaiseSelectionEvents(e);
				}
			}
		}

		// Token: 0x06007418 RID: 29720 RVA: 0x002E561C File Offset: 0x002E461C
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Remove && base.SelectedIndex == -1)
			{
				int num = e.OldStartingIndex + 1;
				if (num > base.Items.Count)
				{
					num = 0;
				}
				TabItem tabItem = this.FindNextTabItem(num, -1);
				if (tabItem != null)
				{
					tabItem.SetCurrentValueInternal(TabItem.IsSelectedProperty, BooleanBoxes.TrueBox);
				}
			}
		}

		// Token: 0x06007419 RID: 29721 RVA: 0x002E5678 File Offset: 0x002E4678
		protected override void OnKeyDown(KeyEventArgs e)
		{
			int direction = 0;
			int startIndex = -1;
			Key key = e.Key;
			if (key != Key.Tab)
			{
				if (key != Key.End)
				{
					if (key == Key.Home)
					{
						direction = 1;
						startIndex = -1;
					}
				}
				else
				{
					direction = -1;
					startIndex = base.Items.Count;
				}
			}
			else if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				startIndex = base.ItemContainerGenerator.IndexFromContainer(base.ItemContainerGenerator.ContainerFromItem(base.SelectedItem));
				if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					direction = -1;
				}
				else
				{
					direction = 1;
				}
			}
			TabItem tabItem = this.FindNextTabItem(startIndex, direction);
			if (tabItem != null && tabItem != base.SelectedItem)
			{
				e.Handled = tabItem.SetFocus();
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x0600741A RID: 29722 RVA: 0x002E572C File Offset: 0x002E472C
		private TabItem FindNextTabItem(int startIndex, int direction)
		{
			TabItem result = null;
			if (direction != 0)
			{
				int num = startIndex;
				for (int i = 0; i < base.Items.Count; i++)
				{
					num += direction;
					if (num >= base.Items.Count)
					{
						num = 0;
					}
					else if (num < 0)
					{
						num = base.Items.Count - 1;
					}
					TabItem tabItem = base.ItemContainerGenerator.ContainerFromIndex(num) as TabItem;
					if (tabItem != null && tabItem.IsEnabled && tabItem.Visibility == Visibility.Visible)
					{
						result = tabItem;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600741B RID: 29723 RVA: 0x002E57A8 File Offset: 0x002E47A8
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TabItem;
		}

		// Token: 0x0600741C RID: 29724 RVA: 0x002E57B3 File Offset: 0x002E47B3
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TabItem();
		}

		// Token: 0x17001AF3 RID: 6899
		// (get) Token: 0x0600741D RID: 29725 RVA: 0x002E57BA File Offset: 0x002E47BA
		internal ContentPresenter SelectedContentPresenter
		{
			get
			{
				return base.GetTemplateChild("PART_SelectedContentHost") as ContentPresenter;
			}
		}

		// Token: 0x0600741E RID: 29726 RVA: 0x002E57CC File Offset: 0x002E47CC
		private void OnGeneratorStatusChanged(object sender, EventArgs e)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				if (base.HasItems && this._selectedItems.Count == 0)
				{
					base.SetCurrentValueInternal(Selector.SelectedIndexProperty, 0);
				}
				this.UpdateSelectedContent();
			}
		}

		// Token: 0x0600741F RID: 29727 RVA: 0x002E5808 File Offset: 0x002E4808
		private TabItem GetSelectedTabItem()
		{
			object selectedItem = base.SelectedItem;
			if (selectedItem != null)
			{
				TabItem tabItem = selectedItem as TabItem;
				if (tabItem == null)
				{
					tabItem = (base.ItemContainerGenerator.ContainerFromIndex(base.SelectedIndex) as TabItem);
					if (tabItem == null || !ItemsControl.EqualsEx(selectedItem, base.ItemContainerGenerator.ItemFromContainer(tabItem)))
					{
						tabItem = (base.ItemContainerGenerator.ContainerFromItem(selectedItem) as TabItem);
					}
				}
				return tabItem;
			}
			return null;
		}

		// Token: 0x06007420 RID: 29728 RVA: 0x002E586C File Offset: 0x002E486C
		private void UpdateSelectedContent()
		{
			if (base.SelectedIndex < 0)
			{
				this.SelectedContent = null;
				this.SelectedContentTemplate = null;
				this.SelectedContentTemplateSelector = null;
				this.SelectedContentStringFormat = null;
				return;
			}
			TabItem selectedTabItem = this.GetSelectedTabItem();
			if (selectedTabItem != null)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(selectedTabItem) as FrameworkElement;
				if (frameworkElement != null)
				{
					KeyboardNavigation.SetTabOnceActiveElement(frameworkElement, selectedTabItem);
					KeyboardNavigation.SetTabOnceActiveElement(this, frameworkElement);
				}
				this.SelectedContent = selectedTabItem.Content;
				ContentPresenter selectedContentPresenter = this.SelectedContentPresenter;
				if (selectedContentPresenter != null)
				{
					selectedContentPresenter.HorizontalAlignment = selectedTabItem.HorizontalContentAlignment;
					selectedContentPresenter.VerticalAlignment = selectedTabItem.VerticalContentAlignment;
				}
				if (selectedTabItem.ContentTemplate != null || selectedTabItem.ContentTemplateSelector != null || selectedTabItem.ContentStringFormat != null)
				{
					this.SelectedContentTemplate = selectedTabItem.ContentTemplate;
					this.SelectedContentTemplateSelector = selectedTabItem.ContentTemplateSelector;
					this.SelectedContentStringFormat = selectedTabItem.ContentStringFormat;
					return;
				}
				this.SelectedContentTemplate = this.ContentTemplate;
				this.SelectedContentTemplateSelector = this.ContentTemplateSelector;
				this.SelectedContentStringFormat = this.ContentStringFormat;
			}
		}

		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x06007421 RID: 29729 RVA: 0x002E5958 File Offset: 0x002E4958
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TabControl._dType;
			}
		}

		// Token: 0x040037F0 RID: 14320
		public static readonly DependencyProperty TabStripPlacementProperty = DependencyProperty.Register("TabStripPlacement", typeof(Dock), typeof(TabControl), new FrameworkPropertyMetadata(Dock.Top, new PropertyChangedCallback(TabControl.OnTabStripPlacementPropertyChanged)), new ValidateValueCallback(DockPanel.IsValidDock));

		// Token: 0x040037F1 RID: 14321
		private static readonly DependencyPropertyKey SelectedContentPropertyKey = DependencyProperty.RegisterReadOnly("SelectedContent", typeof(object), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x040037F2 RID: 14322
		public static readonly DependencyProperty SelectedContentProperty = TabControl.SelectedContentPropertyKey.DependencyProperty;

		// Token: 0x040037F3 RID: 14323
		private static readonly DependencyPropertyKey SelectedContentTemplatePropertyKey = DependencyProperty.RegisterReadOnly("SelectedContentTemplate", typeof(DataTemplate), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x040037F4 RID: 14324
		public static readonly DependencyProperty SelectedContentTemplateProperty = TabControl.SelectedContentTemplatePropertyKey.DependencyProperty;

		// Token: 0x040037F5 RID: 14325
		private static readonly DependencyPropertyKey SelectedContentTemplateSelectorPropertyKey = DependencyProperty.RegisterReadOnly("SelectedContentTemplateSelector", typeof(DataTemplateSelector), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x040037F6 RID: 14326
		public static readonly DependencyProperty SelectedContentTemplateSelectorProperty = TabControl.SelectedContentTemplateSelectorPropertyKey.DependencyProperty;

		// Token: 0x040037F7 RID: 14327
		private static readonly DependencyPropertyKey SelectedContentStringFormatPropertyKey = DependencyProperty.RegisterReadOnly("SelectedContentStringFormat", typeof(string), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x040037F8 RID: 14328
		public static readonly DependencyProperty SelectedContentStringFormatProperty = TabControl.SelectedContentStringFormatPropertyKey.DependencyProperty;

		// Token: 0x040037F9 RID: 14329
		public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x040037FA RID: 14330
		public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x040037FB RID: 14331
		public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x040037FC RID: 14332
		private const string SelectedContentHostTemplateName = "PART_SelectedContentHost";

		// Token: 0x040037FD RID: 14333
		private static DependencyObjectType _dType;
	}
}
