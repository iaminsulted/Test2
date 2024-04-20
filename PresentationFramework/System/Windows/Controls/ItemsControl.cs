using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Controls
{
	// Token: 0x020007A1 RID: 1953
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	[DefaultProperty("Items")]
	[ContentProperty("Items")]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(FrameworkElement))]
	[DefaultEvent("OnItemsChanged")]
	public class ItemsControl : Control, IAddChild, IGeneratorHost, IContainItemStorage
	{
		// Token: 0x06006DA1 RID: 28065 RVA: 0x002CE402 File Offset: 0x002CD402
		public ItemsControl()
		{
			ItemsControl.ShouldCoerceCacheSizeField.SetValue(this, true);
			base.CoerceValue(VirtualizingPanel.CacheLengthUnitProperty);
		}

		// Token: 0x06006DA2 RID: 28066 RVA: 0x002CE42C File Offset: 0x002CD42C
		static ItemsControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(typeof(ItemsControl)));
			ItemsControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ItemsControl));
			EventManager.RegisterClassHandler(typeof(ItemsControl), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ItemsControl.OnGotFocus));
			VirtualizingPanel.ScrollUnitProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsControl.OnScrollingModeChanged), new CoerceValueCallback(ItemsControl.CoerceScrollingMode)));
			VirtualizingPanel.CacheLengthProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsControl.OnCacheSizeChanged)));
			VirtualizingPanel.CacheLengthUnitProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsControl.OnCacheSizeChanged), new CoerceValueCallback(ItemsControl.CoerceVirtualizationCacheLengthUnit)));
		}

		// Token: 0x06006DA3 RID: 28067 RVA: 0x002CE8A6 File Offset: 0x002CD8A6
		private static void OnScrollingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl.ShouldCoerceScrollUnitField.SetValue(d, true);
			d.CoerceValue(VirtualizingPanel.ScrollUnitProperty);
		}

		// Token: 0x06006DA4 RID: 28068 RVA: 0x002CE8C0 File Offset: 0x002CD8C0
		private static object CoerceScrollingMode(DependencyObject d, object baseValue)
		{
			if (ItemsControl.ShouldCoerceScrollUnitField.GetValue(d))
			{
				ItemsControl.ShouldCoerceScrollUnitField.SetValue(d, false);
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(d, VirtualizingPanel.ScrollUnitProperty).BaseValueSource;
				if (((ItemsControl)d).IsGrouping && baseValueSource == BaseValueSource.Default)
				{
					return ScrollUnit.Pixel;
				}
			}
			return baseValue;
		}

		// Token: 0x06006DA5 RID: 28069 RVA: 0x002CE913 File Offset: 0x002CD913
		private static void OnCacheSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl.ShouldCoerceCacheSizeField.SetValue(d, true);
			d.CoerceValue(e.Property);
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x002CE930 File Offset: 0x002CD930
		private static object CoerceVirtualizationCacheLengthUnit(DependencyObject d, object baseValue)
		{
			if (ItemsControl.ShouldCoerceCacheSizeField.GetValue(d))
			{
				ItemsControl.ShouldCoerceCacheSizeField.SetValue(d, false);
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(d, VirtualizingPanel.CacheLengthUnitProperty).BaseValueSource;
				if (!((ItemsControl)d).IsGrouping && !(d is TreeView) && baseValueSource == BaseValueSource.Default)
				{
					return VirtualizationCacheLengthUnit.Item;
				}
			}
			return baseValue;
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x002CE98C File Offset: 0x002CD98C
		private void CreateItemCollectionAndGenerator()
		{
			this._items = new ItemCollection(this);
			((INotifyCollectionChanged)this._items).CollectionChanged += this.OnItemCollectionChanged1;
			this._itemContainerGenerator = new ItemContainerGenerator(this);
			this._itemContainerGenerator.ChangeAlternationCount();
			((INotifyCollectionChanged)this._items).CollectionChanged += this.OnItemCollectionChanged2;
			if (this.IsInitPending)
			{
				this._items.BeginInit();
			}
			else if (base.IsInitialized)
			{
				this._items.BeginInit();
				this._items.EndInit();
			}
			((INotifyCollectionChanged)this._groupStyle).CollectionChanged += this.OnGroupStyleChanged;
		}

		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x06006DA8 RID: 28072 RVA: 0x002CEA34 File Offset: 0x002CDA34
		[CustomCategory("Content")]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ItemCollection Items
		{
			get
			{
				if (this._items == null)
				{
					this.CreateItemCollectionAndGenerator();
				}
				return this._items;
			}
		}

		// Token: 0x06006DA9 RID: 28073 RVA: 0x002CEA4A File Offset: 0x002CDA4A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeItems()
		{
			return this.HasItems;
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x002CEA54 File Offset: 0x002CDA54
		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			IEnumerable oldValue = (IEnumerable)e.OldValue;
			IEnumerable enumerable = (IEnumerable)e.NewValue;
			((IContainItemStorage)itemsControl).Clear();
			BindingExpressionBase beb = BindingOperations.GetBindingExpressionBase(d, ItemsControl.ItemsSourceProperty);
			if (beb != null)
			{
				itemsControl.Items.SetItemsSource(enumerable, (object x) => beb.GetSourceItem(x));
			}
			else if (e.NewValue != null)
			{
				itemsControl.Items.SetItemsSource(enumerable, null);
			}
			else
			{
				itemsControl.Items.ClearItemsSource();
			}
			itemsControl.OnItemsSourceChanged(oldValue, enumerable);
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
		}

		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x06006DAC RID: 28076 RVA: 0x002CEAEC File Offset: 0x002CDAEC
		// (set) Token: 0x06006DAD RID: 28077 RVA: 0x002CEAF9 File Offset: 0x002CDAF9
		[Bindable(true)]
		[CustomCategory("Content")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable ItemsSource
		{
			get
			{
				return this.Items.ItemsSource;
			}
			set
			{
				if (value == null)
				{
					base.ClearValue(ItemsControl.ItemsSourceProperty);
					return;
				}
				base.SetValue(ItemsControl.ItemsSourceProperty, value);
			}
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x06006DAE RID: 28078 RVA: 0x002CEB16 File Offset: 0x002CDB16
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		[Bindable(false)]
		public ItemContainerGenerator ItemContainerGenerator
		{
			get
			{
				if (this._itemContainerGenerator == null)
				{
					this.CreateItemCollectionAndGenerator();
				}
				return this._itemContainerGenerator;
			}
		}

		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x06006DAF RID: 28079 RVA: 0x002CEB2C File Offset: 0x002CDB2C
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (!this.HasItems)
				{
					return EmptyEnumerator.Instance;
				}
				return this.Items.LogicalChildren;
			}
		}

		// Token: 0x06006DB0 RID: 28080 RVA: 0x002CEB47 File Offset: 0x002CDB47
		private void OnItemCollectionChanged1(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.AdjustItemInfoOverride(e);
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x002CEB50 File Offset: 0x002CDB50
		private void OnItemCollectionChanged2(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.SetValue(ItemsControl.HasItemsPropertyKey, this._items != null && !this._items.IsEmpty);
			if (this._focusedInfo != null && this._focusedInfo.Index < 0)
			{
				this._focusedInfo = null;
			}
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				((IContainItemStorage)this).Clear();
			}
			this.OnItemsChanged(e);
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x002CEBBA File Offset: 0x002CDBBA
		internal virtual void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			this.AdjustItemInfo(e, this._focusedInfo);
		}

		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x06006DB4 RID: 28084 RVA: 0x002CEBC9 File Offset: 0x002CDBC9
		[Bindable(false)]
		[Browsable(false)]
		public bool HasItems
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.HasItemsProperty);
			}
		}

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x06006DB5 RID: 28085 RVA: 0x002CEBDB File Offset: 0x002CDBDB
		// (set) Token: 0x06006DB6 RID: 28086 RVA: 0x002CEBED File Offset: 0x002CDBED
		[Bindable(true)]
		[CustomCategory("Content")]
		public string DisplayMemberPath
		{
			get
			{
				return (string)base.GetValue(ItemsControl.DisplayMemberPathProperty);
			}
			set
			{
				base.SetValue(ItemsControl.DisplayMemberPathProperty, value);
			}
		}

		// Token: 0x06006DB7 RID: 28087 RVA: 0x002CEBFB File Offset: 0x002CDBFB
		private static void OnDisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			itemsControl.OnDisplayMemberPathChanged((string)e.OldValue, (string)e.NewValue);
			itemsControl.UpdateDisplayMemberTemplateSelector();
		}

		// Token: 0x06006DB8 RID: 28088 RVA: 0x002CEC28 File Offset: 0x002CDC28
		private void UpdateDisplayMemberTemplateSelector()
		{
			string displayMemberPath = this.DisplayMemberPath;
			string itemStringFormat = this.ItemStringFormat;
			if (string.IsNullOrEmpty(displayMemberPath) && string.IsNullOrEmpty(itemStringFormat))
			{
				if (this.ItemTemplateSelector is DisplayMemberTemplateSelector)
				{
					base.ClearValue(ItemsControl.ItemTemplateSelectorProperty);
				}
				return;
			}
			DataTemplateSelector itemTemplateSelector = this.ItemTemplateSelector;
			if (itemTemplateSelector != null && !(itemTemplateSelector is DisplayMemberTemplateSelector) && (base.ReadLocalValue(ItemsControl.ItemTemplateSelectorProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(ItemsControl.DisplayMemberPathProperty) == DependencyProperty.UnsetValue))
			{
				throw new InvalidOperationException(SR.Get("DisplayMemberPathAndItemTemplateSelectorDefined"));
			}
			this.ItemTemplateSelector = new DisplayMemberTemplateSelector(this.DisplayMemberPath, this.ItemStringFormat);
		}

		// Token: 0x06006DB9 RID: 28089 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnDisplayMemberPathChanged(string oldDisplayMemberPath, string newDisplayMemberPath)
		{
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x06006DBA RID: 28090 RVA: 0x002CECC7 File Offset: 0x002CDCC7
		// (set) Token: 0x06006DBB RID: 28091 RVA: 0x002CECD9 File Offset: 0x002CDCD9
		[CustomCategory("Content")]
		[Bindable(true)]
		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ItemsControl.ItemTemplateProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemTemplateProperty, value);
			}
		}

		// Token: 0x06006DBC RID: 28092 RVA: 0x002CECE7 File Offset: 0x002CDCE7
		private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		// Token: 0x06006DBD RID: 28093 RVA: 0x002CED0C File Offset: 0x002CDD0C
		protected virtual void OnItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
		{
			this.CheckTemplateSource();
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x06006DBE RID: 28094 RVA: 0x002CED27 File Offset: 0x002CDD27
		// (set) Token: 0x06006DBF RID: 28095 RVA: 0x002CED39 File Offset: 0x002CDD39
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[CustomCategory("Content")]
		[Bindable(true)]
		public DataTemplateSelector ItemTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(ItemsControl.ItemTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006DC0 RID: 28096 RVA: 0x002CED47 File Offset: 0x002CDD47
		private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		// Token: 0x06006DC1 RID: 28097 RVA: 0x002CED6C File Offset: 0x002CDD6C
		protected virtual void OnItemTemplateSelectorChanged(DataTemplateSelector oldItemTemplateSelector, DataTemplateSelector newItemTemplateSelector)
		{
			this.CheckTemplateSource();
			if (this._itemContainerGenerator != null && this.ItemTemplate == null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x06006DC2 RID: 28098 RVA: 0x002CED8F File Offset: 0x002CDD8F
		// (set) Token: 0x06006DC3 RID: 28099 RVA: 0x002CEDA1 File Offset: 0x002CDDA1
		[Bindable(true)]
		[CustomCategory("Content")]
		public string ItemStringFormat
		{
			get
			{
				return (string)base.GetValue(ItemsControl.ItemStringFormatProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemStringFormatProperty, value);
			}
		}

		// Token: 0x06006DC4 RID: 28100 RVA: 0x002CEDAF File Offset: 0x002CDDAF
		private static void OnItemStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			itemsControl.OnItemStringFormatChanged((string)e.OldValue, (string)e.NewValue);
			itemsControl.UpdateDisplayMemberTemplateSelector();
		}

		// Token: 0x06006DC5 RID: 28101 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnItemStringFormatChanged(string oldItemStringFormat, string newItemStringFormat)
		{
		}

		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x06006DC6 RID: 28102 RVA: 0x002CEDDA File Offset: 0x002CDDDA
		// (set) Token: 0x06006DC7 RID: 28103 RVA: 0x002CEDEC File Offset: 0x002CDDEC
		[Bindable(true)]
		[CustomCategory("Content")]
		public BindingGroup ItemBindingGroup
		{
			get
			{
				return (BindingGroup)base.GetValue(ItemsControl.ItemBindingGroupProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemBindingGroupProperty, value);
			}
		}

		// Token: 0x06006DC8 RID: 28104 RVA: 0x002CEDFA File Offset: 0x002CDDFA
		private static void OnItemBindingGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemBindingGroupChanged((BindingGroup)e.OldValue, (BindingGroup)e.NewValue);
		}

		// Token: 0x06006DC9 RID: 28105 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnItemBindingGroupChanged(BindingGroup oldItemBindingGroup, BindingGroup newItemBindingGroup)
		{
		}

		// Token: 0x06006DCA RID: 28106 RVA: 0x002CEE20 File Offset: 0x002CDE20
		private void CheckTemplateSource()
		{
			if (string.IsNullOrEmpty(this.DisplayMemberPath))
			{
				Helper.CheckTemplateAndTemplateSelector("Item", ItemsControl.ItemTemplateProperty, ItemsControl.ItemTemplateSelectorProperty, this);
				return;
			}
			if (!(this.ItemTemplateSelector is DisplayMemberTemplateSelector))
			{
				throw new InvalidOperationException(SR.Get("ItemTemplateSelectorBreaksDisplayMemberPath"));
			}
			if (Helper.IsTemplateDefined(ItemsControl.ItemTemplateProperty, this))
			{
				throw new InvalidOperationException(SR.Get("DisplayMemberPathAndItemTemplateDefined"));
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x06006DCB RID: 28107 RVA: 0x002CEE8A File Offset: 0x002CDE8A
		// (set) Token: 0x06006DCC RID: 28108 RVA: 0x002CEE9C File Offset: 0x002CDE9C
		[Category("Content")]
		[Bindable(true)]
		public Style ItemContainerStyle
		{
			get
			{
				return (Style)base.GetValue(ItemsControl.ItemContainerStyleProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemContainerStyleProperty, value);
			}
		}

		// Token: 0x06006DCD RID: 28109 RVA: 0x002CEEAA File Offset: 0x002CDEAA
		private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemContainerStyleChanged((Style)e.OldValue, (Style)e.NewValue);
		}

		// Token: 0x06006DCE RID: 28110 RVA: 0x002CEECF File Offset: 0x002CDECF
		protected virtual void OnItemContainerStyleChanged(Style oldItemContainerStyle, Style newItemContainerStyle)
		{
			Helper.CheckStyleAndStyleSelector("ItemContainer", ItemsControl.ItemContainerStyleProperty, ItemsControl.ItemContainerStyleSelectorProperty, this);
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x06006DCF RID: 28111 RVA: 0x002CEEF9 File Offset: 0x002CDEF9
		// (set) Token: 0x06006DD0 RID: 28112 RVA: 0x002CEF0B File Offset: 0x002CDF0B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category("Content")]
		[Bindable(true)]
		public StyleSelector ItemContainerStyleSelector
		{
			get
			{
				return (StyleSelector)base.GetValue(ItemsControl.ItemContainerStyleSelectorProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemContainerStyleSelectorProperty, value);
			}
		}

		// Token: 0x06006DD1 RID: 28113 RVA: 0x002CEF19 File Offset: 0x002CDF19
		private static void OnItemContainerStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemContainerStyleSelectorChanged((StyleSelector)e.OldValue, (StyleSelector)e.NewValue);
		}

		// Token: 0x06006DD2 RID: 28114 RVA: 0x002CEF3E File Offset: 0x002CDF3E
		protected virtual void OnItemContainerStyleSelectorChanged(StyleSelector oldItemContainerStyleSelector, StyleSelector newItemContainerStyleSelector)
		{
			Helper.CheckStyleAndStyleSelector("ItemContainer", ItemsControl.ItemContainerStyleProperty, ItemsControl.ItemContainerStyleSelectorProperty, this);
			if (this._itemContainerGenerator != null && this.ItemContainerStyle == null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x002CEF70 File Offset: 0x002CDF70
		public static ItemsControl GetItemsOwner(DependencyObject element)
		{
			ItemsControl result = null;
			Panel panel = element as Panel;
			if (panel != null && panel.IsItemsHost)
			{
				ItemsPresenter itemsPresenter = ItemsPresenter.FromPanel(panel);
				if (itemsPresenter != null)
				{
					result = itemsPresenter.Owner;
				}
				else
				{
					result = (panel.TemplatedParent as ItemsControl);
				}
			}
			return result;
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x002CEFB4 File Offset: 0x002CDFB4
		internal static DependencyObject GetItemsOwnerInternal(DependencyObject element)
		{
			ItemsControl itemsControl;
			return ItemsControl.GetItemsOwnerInternal(element, out itemsControl);
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x002CEFCC File Offset: 0x002CDFCC
		internal static DependencyObject GetItemsOwnerInternal(DependencyObject element, out ItemsControl itemsControl)
		{
			DependencyObject dependencyObject = null;
			Panel panel = element as Panel;
			itemsControl = null;
			if (panel != null && panel.IsItemsHost)
			{
				ItemsPresenter itemsPresenter = ItemsPresenter.FromPanel(panel);
				if (itemsPresenter != null)
				{
					dependencyObject = itemsPresenter.TemplatedParent;
					itemsControl = itemsPresenter.Owner;
				}
				else
				{
					dependencyObject = panel.TemplatedParent;
					itemsControl = (dependencyObject as ItemsControl);
				}
			}
			return dependencyObject;
		}

		// Token: 0x06006DD6 RID: 28118 RVA: 0x002CF01B File Offset: 0x002CE01B
		private static ItemsPanelTemplate GetDefaultItemsPanelTemplate()
		{
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel)));
			itemsPanelTemplate.Seal();
			return itemsPanelTemplate;
		}

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x06006DD7 RID: 28119 RVA: 0x002CF037 File Offset: 0x002CE037
		// (set) Token: 0x06006DD8 RID: 28120 RVA: 0x002CF049 File Offset: 0x002CE049
		[Bindable(false)]
		public ItemsPanelTemplate ItemsPanel
		{
			get
			{
				return (ItemsPanelTemplate)base.GetValue(ItemsControl.ItemsPanelProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemsPanelProperty, value);
			}
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x002CF057 File Offset: 0x002CE057
		private static void OnItemsPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemsPanelChanged((ItemsPanelTemplate)e.OldValue, (ItemsPanelTemplate)e.NewValue);
		}

		// Token: 0x06006DDA RID: 28122 RVA: 0x002CF07C File Offset: 0x002CE07C
		protected virtual void OnItemsPanelChanged(ItemsPanelTemplate oldItemsPanel, ItemsPanelTemplate newItemsPanel)
		{
			this.ItemContainerGenerator.OnPanelChanged();
		}

		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x06006DDB RID: 28123 RVA: 0x002CF089 File Offset: 0x002CE089
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsGrouping
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.IsGroupingProperty);
			}
		}

		// Token: 0x06006DDC RID: 28124 RVA: 0x002CF09B File Offset: 0x002CE09B
		private static void OnIsGroupingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnIsGroupingChanged(e);
		}

		// Token: 0x06006DDD RID: 28125 RVA: 0x002CF0A9 File Offset: 0x002CE0A9
		internal virtual void OnIsGroupingChanged(DependencyPropertyChangedEventArgs e)
		{
			ItemsControl.ShouldCoerceScrollUnitField.SetValue(this, true);
			base.CoerceValue(VirtualizingPanel.ScrollUnitProperty);
			ItemsControl.ShouldCoerceCacheSizeField.SetValue(this, true);
			base.CoerceValue(VirtualizingPanel.CacheLengthUnitProperty);
			((IContainItemStorage)this).Clear();
		}

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x06006DDE RID: 28126 RVA: 0x002CF0DF File Offset: 0x002CE0DF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ObservableCollection<GroupStyle> GroupStyle
		{
			get
			{
				return this._groupStyle;
			}
		}

		// Token: 0x06006DDF RID: 28127 RVA: 0x002CF0E7 File Offset: 0x002CE0E7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeGroupStyle()
		{
			return this.GroupStyle.Count > 0;
		}

		// Token: 0x06006DE0 RID: 28128 RVA: 0x002CF0F7 File Offset: 0x002CE0F7
		private void OnGroupStyleChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x06006DE1 RID: 28129 RVA: 0x002CF10C File Offset: 0x002CE10C
		// (set) Token: 0x06006DE2 RID: 28130 RVA: 0x002CF11E File Offset: 0x002CE11E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(true)]
		[CustomCategory("Content")]
		public GroupStyleSelector GroupStyleSelector
		{
			get
			{
				return (GroupStyleSelector)base.GetValue(ItemsControl.GroupStyleSelectorProperty);
			}
			set
			{
				base.SetValue(ItemsControl.GroupStyleSelectorProperty, value);
			}
		}

		// Token: 0x06006DE3 RID: 28131 RVA: 0x002CF12C File Offset: 0x002CE12C
		private static void OnGroupStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnGroupStyleSelectorChanged((GroupStyleSelector)e.OldValue, (GroupStyleSelector)e.NewValue);
		}

		// Token: 0x06006DE4 RID: 28132 RVA: 0x002CF0F7 File Offset: 0x002CE0F7
		protected virtual void OnGroupStyleSelectorChanged(GroupStyleSelector oldGroupStyleSelector, GroupStyleSelector newGroupStyleSelector)
		{
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x06006DE5 RID: 28133 RVA: 0x002CF151 File Offset: 0x002CE151
		// (set) Token: 0x06006DE6 RID: 28134 RVA: 0x002CF163 File Offset: 0x002CE163
		[CustomCategory("Content")]
		[Bindable(true)]
		public int AlternationCount
		{
			get
			{
				return (int)base.GetValue(ItemsControl.AlternationCountProperty);
			}
			set
			{
				base.SetValue(ItemsControl.AlternationCountProperty, value);
			}
		}

		// Token: 0x06006DE7 RID: 28135 RVA: 0x002CF178 File Offset: 0x002CE178
		private static void OnAlternationCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			int oldAlternationCount = (int)e.OldValue;
			int newAlternationCount = (int)e.NewValue;
			itemsControl.OnAlternationCountChanged(oldAlternationCount, newAlternationCount);
		}

		// Token: 0x06006DE8 RID: 28136 RVA: 0x002CF1AC File Offset: 0x002CE1AC
		protected virtual void OnAlternationCountChanged(int oldAlternationCount, int newAlternationCount)
		{
			this.ItemContainerGenerator.ChangeAlternationCount();
		}

		// Token: 0x06006DE9 RID: 28137 RVA: 0x002CF1B9 File Offset: 0x002CE1B9
		public static int GetAlternationIndex(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ItemsControl.AlternationIndexProperty);
		}

		// Token: 0x06006DEA RID: 28138 RVA: 0x002CF1D9 File Offset: 0x002CE1D9
		internal static void SetAlternationIndex(DependencyObject d, int value)
		{
			d.SetValue(ItemsControl.AlternationIndexPropertyKey, value);
		}

		// Token: 0x06006DEB RID: 28139 RVA: 0x002CF1EC File Offset: 0x002CE1EC
		internal static void ClearAlternationIndex(DependencyObject d)
		{
			d.ClearValue(ItemsControl.AlternationIndexPropertyKey);
		}

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x06006DEC RID: 28140 RVA: 0x002CF1F9 File Offset: 0x002CE1F9
		// (set) Token: 0x06006DED RID: 28141 RVA: 0x002CF20B File Offset: 0x002CE20B
		public bool IsTextSearchEnabled
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.IsTextSearchEnabledProperty);
			}
			set
			{
				base.SetValue(ItemsControl.IsTextSearchEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x06006DEE RID: 28142 RVA: 0x002CF21E File Offset: 0x002CE21E
		// (set) Token: 0x06006DEF RID: 28143 RVA: 0x002CF230 File Offset: 0x002CE230
		public bool IsTextSearchCaseSensitive
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.IsTextSearchCaseSensitiveProperty);
			}
			set
			{
				base.SetValue(ItemsControl.IsTextSearchCaseSensitiveProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006DF0 RID: 28144 RVA: 0x002CF244 File Offset: 0x002CE244
		public static ItemsControl ItemsControlFromItemContainer(DependencyObject container)
		{
			UIElement uielement = container as UIElement;
			if (uielement == null)
			{
				return null;
			}
			ItemsControl itemsControl = LogicalTreeHelper.GetParent(uielement) as ItemsControl;
			if (itemsControl == null)
			{
				uielement = (VisualTreeHelper.GetParent(uielement) as UIElement);
				return ItemsControl.GetItemsOwner(uielement);
			}
			if (((IGeneratorHost)itemsControl).IsItemItsOwnContainer(uielement))
			{
				return itemsControl;
			}
			return null;
		}

		// Token: 0x06006DF1 RID: 28145 RVA: 0x002CF28C File Offset: 0x002CE28C
		public static DependencyObject ContainerFromElement(ItemsControl itemsControl, DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (ItemsControl.IsContainerForItemsControl(element, itemsControl))
			{
				return element;
			}
			FrameworkObject frameworkObject = new FrameworkObject(element);
			frameworkObject.Reset(frameworkObject.GetPreferVisualParent(true).DO);
			while (frameworkObject.DO != null && !ItemsControl.IsContainerForItemsControl(frameworkObject.DO, itemsControl))
			{
				frameworkObject.Reset(frameworkObject.PreferVisualParent.DO);
			}
			return frameworkObject.DO;
		}

		// Token: 0x06006DF2 RID: 28146 RVA: 0x002CF308 File Offset: 0x002CE308
		public DependencyObject ContainerFromElement(DependencyObject element)
		{
			return ItemsControl.ContainerFromElement(this, element);
		}

		// Token: 0x06006DF3 RID: 28147 RVA: 0x002CF311 File Offset: 0x002CE311
		private static bool IsContainerForItemsControl(DependencyObject element, ItemsControl itemsControl)
		{
			return element.ContainsValue(ItemContainerGenerator.ItemForItemContainerProperty) && (itemsControl == null || itemsControl == ItemsControl.ItemsControlFromItemContainer(element));
		}

		// Token: 0x06006DF4 RID: 28148 RVA: 0x002CF32F File Offset: 0x002CE32F
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		// Token: 0x06006DF5 RID: 28149 RVA: 0x002CF338 File Offset: 0x002CE338
		protected virtual void AddChild(object value)
		{
			this.Items.Add(value);
		}

		// Token: 0x06006DF6 RID: 28150 RVA: 0x002CF347 File Offset: 0x002CE347
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		// Token: 0x06006DF7 RID: 28151 RVA: 0x002CF338 File Offset: 0x002CE338
		protected virtual void AddText(string text)
		{
			this.Items.Add(text);
		}

		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x06006DF8 RID: 28152 RVA: 0x002A3094 File Offset: 0x002A2094
		ItemCollection IGeneratorHost.View
		{
			get
			{
				return this.Items;
			}
		}

		// Token: 0x06006DF9 RID: 28153 RVA: 0x002CF350 File Offset: 0x002CE350
		bool IGeneratorHost.IsItemItsOwnContainer(object item)
		{
			return this.IsItemItsOwnContainer(item);
		}

		// Token: 0x06006DFA RID: 28154 RVA: 0x002CF35C File Offset: 0x002CE35C
		DependencyObject IGeneratorHost.GetContainerForItem(object item)
		{
			DependencyObject dependencyObject;
			if (this.IsItemItsOwnContainerOverride(item))
			{
				dependencyObject = (item as DependencyObject);
			}
			else
			{
				dependencyObject = this.GetContainerForItemOverride();
			}
			Visual visual = dependencyObject as Visual;
			if (visual != null)
			{
				Visual visual2 = VisualTreeHelper.GetParent(visual) as Visual;
				if (visual2 != null)
				{
					Invariant.Assert(visual2 is FrameworkElement, SR.Get("ItemsControl_ParentNotFrameworkElement"));
					Panel panel = visual2 as Panel;
					if (panel != null && visual is UIElement)
					{
						panel.Children.RemoveNoVerify((UIElement)visual);
					}
					else
					{
						((FrameworkElement)visual2).TemplateChild = null;
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x06006DFB RID: 28155 RVA: 0x002CF3E8 File Offset: 0x002CE3E8
		void IGeneratorHost.PrepareItemContainer(DependencyObject container, object item)
		{
			GroupItem groupItem = container as GroupItem;
			if (groupItem != null)
			{
				groupItem.PrepareItemContainer(item, this);
				return;
			}
			if (this.ShouldApplyItemContainerStyle(container, item))
			{
				this.ApplyItemContainerStyle(container, item);
			}
			this.PrepareContainerForItemOverride(container, item);
			if (!Helper.HasUnmodifiedDefaultValue(this, ItemsControl.ItemBindingGroupProperty) && Helper.HasUnmodifiedDefaultOrInheritedValue(container, FrameworkElement.BindingGroupProperty))
			{
				BindingGroup itemBindingGroup = this.ItemBindingGroup;
				BindingGroup value = (itemBindingGroup != null) ? new BindingGroup(itemBindingGroup) : null;
				container.SetValue(FrameworkElement.BindingGroupProperty, value);
			}
			if (container == item && TraceData.IsEnabled && (this.ItemTemplate != null || this.ItemTemplateSelector != null))
			{
				TraceData.TraceAndNotify(TraceEventType.Error, TraceData.ItemTemplateForDirectItem, null, new object[]
				{
					AvTrace.TypeName(item)
				}, null);
			}
			TreeViewItem treeViewItem = container as TreeViewItem;
			if (treeViewItem != null)
			{
				treeViewItem.PrepareItemContainer(item, this);
			}
		}

		// Token: 0x06006DFC RID: 28156 RVA: 0x002CF4A8 File Offset: 0x002CE4A8
		void IGeneratorHost.ClearContainerForItem(DependencyObject container, object item)
		{
			GroupItem groupItem = container as GroupItem;
			if (groupItem == null)
			{
				this.ClearContainerForItemOverride(container, item);
				TreeViewItem treeViewItem = container as TreeViewItem;
				if (treeViewItem != null)
				{
					treeViewItem.ClearItemContainer(item, this);
					return;
				}
			}
			else
			{
				groupItem.ClearItemContainer(item, this);
			}
		}

		// Token: 0x06006DFD RID: 28157 RVA: 0x002CF4E4 File Offset: 0x002CE4E4
		bool IGeneratorHost.IsHostForItemContainer(DependencyObject container)
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
			if (itemsControl != null)
			{
				return itemsControl == this;
			}
			return LogicalTreeHelper.GetParent(container) == null && (this.IsItemItsOwnContainerOverride(container) && this.HasItems) && this.Items.Contains(container);
		}

		// Token: 0x06006DFE RID: 28158 RVA: 0x002CF52C File Offset: 0x002CE52C
		GroupStyle IGeneratorHost.GetGroupStyle(CollectionViewGroup group, int level)
		{
			GroupStyle groupStyle = null;
			if (this.GroupStyleSelector != null)
			{
				groupStyle = this.GroupStyleSelector(group, level);
			}
			if (groupStyle == null)
			{
				if (level >= this.GroupStyle.Count)
				{
					level = this.GroupStyle.Count - 1;
				}
				if (level >= 0)
				{
					groupStyle = this.GroupStyle[level];
				}
			}
			return groupStyle;
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x002CF583 File Offset: 0x002CE583
		void IGeneratorHost.SetIsGrouping(bool isGrouping)
		{
			base.SetValue(ItemsControl.IsGroupingPropertyKey, BooleanBoxes.Box(isGrouping));
		}

		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x06006E00 RID: 28160 RVA: 0x002CF596 File Offset: 0x002CE596
		int IGeneratorHost.AlternationCount
		{
			get
			{
				return this.AlternationCount;
			}
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x002CF59E File Offset: 0x002CE59E
		public override void BeginInit()
		{
			base.BeginInit();
			if (this._items != null)
			{
				this._items.BeginInit();
			}
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x002CF5B9 File Offset: 0x002CE5B9
		public override void EndInit()
		{
			if (this.IsInitPending)
			{
				if (this._items != null)
				{
					this._items.EndInit();
				}
				base.EndInit();
			}
		}

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x06006E03 RID: 28163 RVA: 0x002CF5DC File Offset: 0x002CE5DC
		private bool IsInitPending
		{
			get
			{
				return base.ReadInternalFlag(InternalFlags.InitPending);
			}
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x002CF5E9 File Offset: 0x002CE5E9
		public bool IsItemItsOwnContainer(object item)
		{
			return this.IsItemItsOwnContainerOverride(item);
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x002CF5F2 File Offset: 0x002CE5F2
		protected virtual bool IsItemItsOwnContainerOverride(object item)
		{
			return item is UIElement;
		}

		// Token: 0x06006E06 RID: 28166 RVA: 0x002CF5FD File Offset: 0x002CE5FD
		protected virtual DependencyObject GetContainerForItemOverride()
		{
			return new ContentPresenter();
		}

		// Token: 0x06006E07 RID: 28167 RVA: 0x002CF604 File Offset: 0x002CE604
		protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			HeaderedContentControl headeredContentControl;
			if ((headeredContentControl = (element as HeaderedContentControl)) != null)
			{
				headeredContentControl.PrepareHeaderedContentControl(item, this.ItemTemplate, this.ItemTemplateSelector, this.ItemStringFormat);
				return;
			}
			ContentControl contentControl;
			if ((contentControl = (element as ContentControl)) != null)
			{
				contentControl.PrepareContentControl(item, this.ItemTemplate, this.ItemTemplateSelector, this.ItemStringFormat);
				return;
			}
			ContentPresenter contentPresenter;
			if ((contentPresenter = (element as ContentPresenter)) != null)
			{
				contentPresenter.PrepareContentPresenter(item, this.ItemTemplate, this.ItemTemplateSelector, this.ItemStringFormat);
				return;
			}
			HeaderedItemsControl headeredItemsControl;
			if ((headeredItemsControl = (element as HeaderedItemsControl)) != null)
			{
				headeredItemsControl.PrepareHeaderedItemsControl(item, this);
				return;
			}
			ItemsControl itemsControl;
			if ((itemsControl = (element as ItemsControl)) != null && itemsControl != this)
			{
				itemsControl.PrepareItemsControl(item, this);
			}
		}

		// Token: 0x06006E08 RID: 28168 RVA: 0x002CF6A8 File Offset: 0x002CE6A8
		protected virtual void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			HeaderedContentControl headeredContentControl;
			if ((headeredContentControl = (element as HeaderedContentControl)) != null)
			{
				headeredContentControl.ClearHeaderedContentControl(item);
				return;
			}
			ContentControl contentControl;
			if ((contentControl = (element as ContentControl)) != null)
			{
				contentControl.ClearContentControl(item);
				return;
			}
			ContentPresenter contentPresenter;
			if ((contentPresenter = (element as ContentPresenter)) != null)
			{
				contentPresenter.ClearContentPresenter(item);
				return;
			}
			HeaderedItemsControl headeredItemsControl;
			if ((headeredItemsControl = (element as HeaderedItemsControl)) != null)
			{
				headeredItemsControl.ClearHeaderedItemsControl(item);
				return;
			}
			ItemsControl itemsControl;
			if ((itemsControl = (element as ItemsControl)) != null && itemsControl != this)
			{
				itemsControl.ClearItemsControl(item);
			}
		}

		// Token: 0x06006E09 RID: 28169 RVA: 0x002CF714 File Offset: 0x002CE714
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (!string.IsNullOrEmpty(e.Text) && this.IsTextSearchEnabled && (e.OriginalSource == this || ItemsControl.ItemsControlFromItemContainer(e.OriginalSource as DependencyObject) == this))
			{
				TextSearch textSearch = TextSearch.EnsureInstance(this);
				if (textSearch != null)
				{
					textSearch.DoSearch(e.Text);
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006E0A RID: 28170 RVA: 0x002CF778 File Offset: 0x002CE778
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.IsTextSearchEnabled && e.Key == Key.Back)
			{
				TextSearch textSearch = TextSearch.EnsureInstance(this);
				if (textSearch != null)
				{
					textSearch.DeleteLastCharacter();
				}
			}
		}

		// Token: 0x06006E0B RID: 28171 RVA: 0x002CF7AE File Offset: 0x002CE7AE
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this._itemsHost = null;
			this._scrollHost = null;
			base.WriteControlFlag(Control.ControlBoolFlags.ScrollHostValid, false);
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
		}

		// Token: 0x06006E0C RID: 28172 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected virtual bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
		{
			return true;
		}

		// Token: 0x06006E0D RID: 28173 RVA: 0x002CF7D0 File Offset: 0x002CE7D0
		internal void PrepareItemsControl(object item, ItemsControl parentItemsControl)
		{
			if (item != this)
			{
				DataTemplate itemTemplate = parentItemsControl.ItemTemplate;
				DataTemplateSelector itemTemplateSelector = parentItemsControl.ItemTemplateSelector;
				string itemStringFormat = parentItemsControl.ItemStringFormat;
				Style itemContainerStyle = parentItemsControl.ItemContainerStyle;
				StyleSelector itemContainerStyleSelector = parentItemsControl.ItemContainerStyleSelector;
				int alternationCount = parentItemsControl.AlternationCount;
				BindingGroup itemBindingGroup = parentItemsControl.ItemBindingGroup;
				if (itemTemplate != null)
				{
					base.SetValue(ItemsControl.ItemTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(ItemsControl.ItemTemplateSelectorProperty, itemTemplateSelector);
				}
				if (itemStringFormat != null && Helper.HasDefaultValue(this, ItemsControl.ItemStringFormatProperty))
				{
					base.SetValue(ItemsControl.ItemStringFormatProperty, itemStringFormat);
				}
				if (itemContainerStyle != null && Helper.HasDefaultValue(this, ItemsControl.ItemContainerStyleProperty))
				{
					base.SetValue(ItemsControl.ItemContainerStyleProperty, itemContainerStyle);
				}
				if (itemContainerStyleSelector != null && Helper.HasDefaultValue(this, ItemsControl.ItemContainerStyleSelectorProperty))
				{
					base.SetValue(ItemsControl.ItemContainerStyleSelectorProperty, itemContainerStyleSelector);
				}
				if (alternationCount != 0 && Helper.HasDefaultValue(this, ItemsControl.AlternationCountProperty))
				{
					base.SetValue(ItemsControl.AlternationCountProperty, alternationCount);
				}
				if (itemBindingGroup != null && Helper.HasDefaultValue(this, ItemsControl.ItemBindingGroupProperty))
				{
					base.SetValue(ItemsControl.ItemBindingGroupProperty, itemBindingGroup);
				}
			}
		}

		// Token: 0x06006E0E RID: 28174 RVA: 0x002CF8CD File Offset: 0x002CE8CD
		internal void ClearItemsControl(object item)
		{
		}

		// Token: 0x06006E0F RID: 28175 RVA: 0x002CF8D4 File Offset: 0x002CE8D4
		internal object OnBringItemIntoView(object arg)
		{
			ItemsControl.ItemInfo itemInfo = arg as ItemsControl.ItemInfo;
			if (itemInfo == null)
			{
				itemInfo = this.NewItemInfo(arg, null, -1);
			}
			return this.OnBringItemIntoView(itemInfo);
		}

		// Token: 0x06006E10 RID: 28176 RVA: 0x002CF904 File Offset: 0x002CE904
		internal object OnBringItemIntoView(ItemsControl.ItemInfo info)
		{
			FrameworkElement frameworkElement = info.Container as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.BringIntoView();
			}
			else if ((info = this.LeaseItemInfo(info, true)).Index >= 0)
			{
				if (!FrameworkCompatibilityPreferences.GetVSP45Compat())
				{
					base.UpdateLayout();
				}
				VirtualizingPanel virtualizingPanel = this.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.BringIndexIntoView(info.Index);
				}
			}
			return null;
		}

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x06006E11 RID: 28177 RVA: 0x002CF964 File Offset: 0x002CE964
		// (set) Token: 0x06006E12 RID: 28178 RVA: 0x002CF96C File Offset: 0x002CE96C
		internal Panel ItemsHost
		{
			get
			{
				return this._itemsHost;
			}
			set
			{
				this._itemsHost = value;
			}
		}

		// Token: 0x06006E13 RID: 28179 RVA: 0x002CF978 File Offset: 0x002CE978
		internal bool NavigateByLine(FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
			if (!FrameworkAppContextSwitches.KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement)
			{
				while (dependencyObject != null && !(dependencyObject is FrameworkElement))
				{
					dependencyObject = KeyboardNavigation.GetParent(dependencyObject);
				}
			}
			return this.NavigateByLine(this.FocusedInfo, dependencyObject as FrameworkElement, direction, itemNavigateArgs);
		}

		// Token: 0x06006E14 RID: 28180 RVA: 0x002CF9C0 File Offset: 0x002CE9C0
		internal void PrepareNavigateByLine(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost == null)
			{
				return;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			this.NavigateByLineInternal(startingItem, direction, startingElement, itemNavigateArgs, false, out container);
		}

		// Token: 0x06006E15 RID: 28181 RVA: 0x002CFA14 File Offset: 0x002CEA14
		internal bool NavigateByLine(ItemsControl.ItemInfo startingInfo, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			return this.NavigateByLine(startingInfo, null, direction, itemNavigateArgs);
		}

		// Token: 0x06006E16 RID: 28182 RVA: 0x002CFA20 File Offset: 0x002CEA20
		internal bool NavigateByLine(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			if (this.ItemsHost == null)
			{
				return false;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			FrameworkElement frameworkElement;
			return this.NavigateByLineInternal(startingItem, direction, startingElement, itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x06006E17 RID: 28183 RVA: 0x002CFA70 File Offset: 0x002CEA70
		private bool NavigateByLineInternal(object startingItem, FocusNavigationDirection direction, FrameworkElement startingElement, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (startingItem == null && (startingElement == null || startingElement == this))
			{
				return this.NavigateToStartInternal(itemNavigateArgs, shouldFocus, out container);
			}
			if (startingElement == null || !this.ItemsHost.IsAncestorOf(startingElement))
			{
				startingElement = this.ScrollHost;
			}
			else
			{
				DependencyObject parent = VisualTreeHelper.GetParent(startingElement);
				while (parent != null && parent != this.ItemsHost)
				{
					KeyboardNavigationMode directionalNavigation = KeyboardNavigation.GetDirectionalNavigation(parent);
					if (directionalNavigation == KeyboardNavigationMode.Contained || directionalNavigation == KeyboardNavigationMode.Cycle)
					{
						return false;
					}
					parent = VisualTreeHelper.GetParent(parent);
				}
			}
			bool flag = this.ItemsHost != null && this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
			bool treeViewNavigation = this is TreeView;
			FrameworkElement frameworkElement = KeyboardNavigation.Current.PredictFocusedElement(startingElement, direction, treeViewNavigation) as FrameworkElement;
			if (this.ScrollHost != null)
			{
				bool flag2 = false;
				FrameworkElement viewportElement = this.GetViewportElement();
				VirtualizingPanel virtualizingPanel = this.ItemsHost as VirtualizingPanel;
				bool flag3 = KeyboardNavigation.GetDirectionalNavigation(this) == KeyboardNavigationMode.Cycle;
				for (;;)
				{
					if (frameworkElement != null)
					{
						if (virtualizingPanel == null || !this.ScrollHost.CanContentScroll || !VirtualizingPanel.GetIsVirtualizing(this))
						{
							goto IL_27B;
						}
						Rect toRect;
						ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportElement, ItemsControl.TryGetTreeViewItemHeader(frameworkElement) as FrameworkElement, direction, false, out toRect);
						if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport || elementViewportPosition == ElementViewportPosition.PartiallyInViewport)
						{
							if (!flag3)
							{
								goto IL_27B;
							}
							Rect fromRect;
							ItemsControl.GetElementViewportPosition(viewportElement, startingElement, direction, false, out fromRect);
							if (this.IsInDirectionForLineNavigation(fromRect, toRect, direction, flag))
							{
								goto IL_27B;
							}
						}
						frameworkElement = null;
					}
					double horizontalOffset = this.ScrollHost.HorizontalOffset;
					double verticalOffset = this.ScrollHost.VerticalOffset;
					if (direction != FocusNavigationDirection.Up)
					{
						if (direction == FocusNavigationDirection.Down)
						{
							flag2 = true;
							if (flag)
							{
								this.ScrollHost.LineRight();
							}
							else
							{
								this.ScrollHost.LineDown();
							}
						}
					}
					else
					{
						flag2 = true;
						if (flag)
						{
							this.ScrollHost.LineLeft();
						}
						else
						{
							this.ScrollHost.LineUp();
						}
					}
					this.ScrollHost.UpdateLayout();
					if ((DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset)) || (direction == FocusNavigationDirection.Down && (this.ScrollHost.VerticalOffset > this.ScrollHost.ExtentHeight || this.ScrollHost.HorizontalOffset > this.ScrollHost.ExtentWidth)) || (direction == FocusNavigationDirection.Up && (this.ScrollHost.VerticalOffset < 0.0 || this.ScrollHost.HorizontalOffset < 0.0)))
					{
						break;
					}
					frameworkElement = (KeyboardNavigation.Current.PredictFocusedElement(startingElement, direction, treeViewNavigation) as FrameworkElement);
				}
				if (flag3)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						return this.NavigateToEndInternal(itemNavigateArgs, true, out container);
					}
					if (direction == FocusNavigationDirection.Down)
					{
						return this.NavigateToStartInternal(itemNavigateArgs, true, out container);
					}
				}
				IL_27B:
				if (flag2 && frameworkElement != null && this.ItemsHost.IsAncestorOf(frameworkElement))
				{
					this.AdjustOffsetToAlignWithEdge(frameworkElement, direction);
				}
			}
			if (frameworkElement != null && this.ItemsHost.IsAncestorOf(frameworkElement))
			{
				ItemsControl itemsControl = null;
				object encapsulatingItem = ItemsControl.GetEncapsulatingItem(frameworkElement, out container, out itemsControl);
				container = frameworkElement;
				if (!shouldFocus)
				{
					return false;
				}
				if (encapsulatingItem == DependencyProperty.UnsetValue || encapsulatingItem is CollectionViewGroupInternal)
				{
					return frameworkElement.Focus();
				}
				if (itemsControl != null)
				{
					return itemsControl.FocusItem(this.NewItemInfo(encapsulatingItem, container, -1), itemNavigateArgs);
				}
			}
			return false;
		}

		// Token: 0x06006E18 RID: 28184 RVA: 0x002CFD74 File Offset: 0x002CED74
		internal void PrepareToNavigateByPage(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost == null)
			{
				return;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			this.NavigateByPageInternal(startingItem, direction, startingElement, itemNavigateArgs, false, out container);
		}

		// Token: 0x06006E19 RID: 28185 RVA: 0x002CFDC8 File Offset: 0x002CEDC8
		internal bool NavigateByPage(FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			return this.NavigateByPage(this.FocusedInfo, Keyboard.FocusedElement as FrameworkElement, direction, itemNavigateArgs);
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x002CFDE2 File Offset: 0x002CEDE2
		internal bool NavigateByPage(ItemsControl.ItemInfo startingInfo, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			return this.NavigateByPage(startingInfo, null, direction, itemNavigateArgs);
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x002CFDF0 File Offset: 0x002CEDF0
		internal bool NavigateByPage(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			if (this.ItemsHost == null)
			{
				return false;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			FrameworkElement frameworkElement;
			return this.NavigateByPageInternal(startingItem, direction, startingElement, itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x06006E1C RID: 28188 RVA: 0x002CFE40 File Offset: 0x002CEE40
		private bool NavigateByPageInternal(object startingItem, FocusNavigationDirection direction, FrameworkElement startingElement, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (startingItem == null && (startingElement == null || startingElement == this))
			{
				return this.NavigateToFirstItemOnCurrentPage(startingItem, direction, itemNavigateArgs, shouldFocus, out container);
			}
			FrameworkElement frameworkElement;
			object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, direction, out frameworkElement);
			if ((object.Equals(startingItem, firstItemOnCurrentPage) || object.Equals(startingElement, frameworkElement)) && this.ScrollHost != null)
			{
				bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
				do
				{
					double horizontalOffset = this.ScrollHost.HorizontalOffset;
					double verticalOffset = this.ScrollHost.VerticalOffset;
					if (direction != FocusNavigationDirection.Up)
					{
						if (direction == FocusNavigationDirection.Down)
						{
							if (flag)
							{
								this.ScrollHost.PageRight();
							}
							else
							{
								this.ScrollHost.PageDown();
							}
						}
					}
					else if (flag)
					{
						this.ScrollHost.PageLeft();
					}
					else
					{
						this.ScrollHost.PageUp();
					}
					this.ScrollHost.UpdateLayout();
					if (DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset))
					{
						break;
					}
					firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, direction, out frameworkElement);
				}
				while (firstItemOnCurrentPage == DependencyProperty.UnsetValue);
			}
			container = frameworkElement;
			if (shouldFocus)
			{
				if (frameworkElement != null && (firstItemOnCurrentPage == DependencyProperty.UnsetValue || firstItemOnCurrentPage is CollectionViewGroupInternal))
				{
					return frameworkElement.Focus();
				}
				ItemsControl encapsulatingItemsControl = ItemsControl.GetEncapsulatingItemsControl(frameworkElement);
				if (encapsulatingItemsControl != null)
				{
					return encapsulatingItemsControl.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, frameworkElement, -1), itemNavigateArgs);
				}
			}
			return false;
		}

		// Token: 0x06006E1D RID: 28189 RVA: 0x002CFF9C File Offset: 0x002CEF9C
		internal void NavigateToStart(ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			FrameworkElement frameworkElement;
			this.NavigateToStartInternal(itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x002CFFB4 File Offset: 0x002CEFB4
		internal bool NavigateToStartInternal(ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost != null)
			{
				if (this.ScrollHost != null)
				{
					bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
					double horizontalOffset;
					double verticalOffset;
					do
					{
						horizontalOffset = this.ScrollHost.HorizontalOffset;
						verticalOffset = this.ScrollHost.VerticalOffset;
						if (flag)
						{
							this.ScrollHost.ScrollToLeftEnd();
						}
						else
						{
							this.ScrollHost.ScrollToTop();
						}
						this.ItemsHost.UpdateLayout();
					}
					while (!DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) || !DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset));
				}
				FrameworkElement startingElement = this.FindEndFocusableLeafContainer(this.ItemsHost, false);
				FrameworkElement frameworkElement;
				object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, FocusNavigationDirection.Up, out frameworkElement);
				container = frameworkElement;
				if (shouldFocus)
				{
					if (frameworkElement != null && (firstItemOnCurrentPage == DependencyProperty.UnsetValue || firstItemOnCurrentPage is CollectionViewGroupInternal))
					{
						return frameworkElement.Focus();
					}
					ItemsControl encapsulatingItemsControl = ItemsControl.GetEncapsulatingItemsControl(frameworkElement);
					if (encapsulatingItemsControl != null)
					{
						return encapsulatingItemsControl.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, frameworkElement, -1), itemNavigateArgs);
					}
				}
			}
			return false;
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x002D00D0 File Offset: 0x002CF0D0
		internal void NavigateToEnd(ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			FrameworkElement frameworkElement;
			this.NavigateToEndInternal(itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x06006E20 RID: 28192 RVA: 0x002D00E8 File Offset: 0x002CF0E8
		internal bool NavigateToEndInternal(ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost != null)
			{
				if (this.ScrollHost != null)
				{
					bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
					double horizontalOffset;
					double verticalOffset;
					do
					{
						horizontalOffset = this.ScrollHost.HorizontalOffset;
						verticalOffset = this.ScrollHost.VerticalOffset;
						if (flag)
						{
							this.ScrollHost.ScrollToRightEnd();
						}
						else
						{
							this.ScrollHost.ScrollToBottom();
						}
						this.ItemsHost.UpdateLayout();
					}
					while (!DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) || !DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset));
				}
				FrameworkElement startingElement = this.FindEndFocusableLeafContainer(this.ItemsHost, true);
				FrameworkElement frameworkElement;
				object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, FocusNavigationDirection.Down, out frameworkElement);
				container = frameworkElement;
				if (shouldFocus)
				{
					if (frameworkElement != null && (firstItemOnCurrentPage == DependencyProperty.UnsetValue || firstItemOnCurrentPage is CollectionViewGroupInternal))
					{
						return frameworkElement.Focus();
					}
					ItemsControl encapsulatingItemsControl = ItemsControl.GetEncapsulatingItemsControl(frameworkElement);
					if (encapsulatingItemsControl != null)
					{
						return encapsulatingItemsControl.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, frameworkElement, -1), itemNavigateArgs);
					}
				}
			}
			return false;
		}

		// Token: 0x06006E21 RID: 28193 RVA: 0x002D0204 File Offset: 0x002CF204
		private FrameworkElement FindEndFocusableLeafContainer(Panel itemsHost, bool last)
		{
			if (itemsHost == null)
			{
				return null;
			}
			UIElementCollection children = itemsHost.Children;
			if (children != null)
			{
				int count = children.Count;
				int num = last ? (count - 1) : 0;
				int num2 = last ? -1 : 1;
				while (num >= 0 && num < count)
				{
					FrameworkElement frameworkElement = children[num] as FrameworkElement;
					if (frameworkElement != null)
					{
						ItemsControl itemsControl = frameworkElement as ItemsControl;
						FrameworkElement frameworkElement2 = null;
						if (itemsControl != null)
						{
							if (itemsControl.ItemsHost != null)
							{
								frameworkElement2 = this.FindEndFocusableLeafContainer(itemsControl.ItemsHost, last);
							}
						}
						else
						{
							GroupItem groupItem = frameworkElement as GroupItem;
							if (groupItem != null && groupItem.ItemsHost != null)
							{
								frameworkElement2 = this.FindEndFocusableLeafContainer(groupItem.ItemsHost, last);
							}
						}
						if (frameworkElement2 != null)
						{
							return frameworkElement2;
						}
						if (FrameworkElement.KeyboardNavigation.IsFocusableInternal(frameworkElement))
						{
							return frameworkElement;
						}
					}
					num += num2;
				}
			}
			return null;
		}

		// Token: 0x06006E22 RID: 28194 RVA: 0x002D02C9 File Offset: 0x002CF2C9
		internal void NavigateToItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool alwaysAtTopOfViewport = false)
		{
			if (info != null)
			{
				this.NavigateToItem(info.Item, info.Index, itemNavigateArgs, alwaysAtTopOfViewport);
			}
		}

		// Token: 0x06006E23 RID: 28195 RVA: 0x002D02E8 File Offset: 0x002CF2E8
		internal void NavigateToItem(object item, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			this.NavigateToItem(item, -1, itemNavigateArgs, false);
		}

		// Token: 0x06006E24 RID: 28196 RVA: 0x002D02F4 File Offset: 0x002CF2F4
		internal void NavigateToItem(object item, int itemIndex, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			this.NavigateToItem(item, itemIndex, itemNavigateArgs, false);
		}

		// Token: 0x06006E25 RID: 28197 RVA: 0x002D0300 File Offset: 0x002CF300
		internal void NavigateToItem(object item, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool alwaysAtTopOfViewport)
		{
			this.NavigateToItem(item, -1, itemNavigateArgs, alwaysAtTopOfViewport);
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x002D030C File Offset: 0x002CF30C
		private void NavigateToItem(object item, int elementIndex, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool alwaysAtTopOfViewport)
		{
			if (item == DependencyProperty.UnsetValue)
			{
				return;
			}
			if (elementIndex == -1)
			{
				elementIndex = this.Items.IndexOf(item);
				if (elementIndex == -1)
				{
					return;
				}
			}
			bool flag = false;
			if (this.ItemsHost != null)
			{
				flag = (this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal);
			}
			FocusNavigationDirection direction = flag ? FocusNavigationDirection.Right : FocusNavigationDirection.Down;
			FrameworkElement container;
			this.MakeVisible(elementIndex, direction, alwaysAtTopOfViewport, out container);
			this.FocusItem(this.NewItemInfo(item, container, -1), itemNavigateArgs);
		}

		// Token: 0x06006E27 RID: 28199 RVA: 0x002D0388 File Offset: 0x002CF388
		private object FindFocusable(int startIndex, int direction, out int foundIndex, out FrameworkElement foundContainer)
		{
			if (this.HasItems)
			{
				int count = this.Items.Count;
				while (startIndex >= 0 && startIndex < count)
				{
					FrameworkElement frameworkElement = this.ItemContainerGenerator.ContainerFromIndex(startIndex) as FrameworkElement;
					if (frameworkElement == null || Keyboard.IsFocusable(frameworkElement))
					{
						foundIndex = startIndex;
						foundContainer = frameworkElement;
						return this.Items[startIndex];
					}
					startIndex += direction;
				}
			}
			foundIndex = -1;
			foundContainer = null;
			return null;
		}

		// Token: 0x06006E28 RID: 28200 RVA: 0x002D03F4 File Offset: 0x002CF3F4
		private void AdjustOffsetToAlignWithEdge(FrameworkElement element, FocusNavigationDirection direction)
		{
			if (VirtualizingPanel.GetScrollUnit(this) != ScrollUnit.Item)
			{
				ScrollViewer scrollHost = this.ScrollHost;
				FrameworkElement viewportElement = this.GetViewportElement();
				element = (ItemsControl.TryGetTreeViewItemHeader(element) as FrameworkElement);
				Rect rect = new Rect(default(Point), element.RenderSize);
				rect = element.TransformToAncestor(viewportElement).TransformBounds(rect);
				bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
				if (direction == FocusNavigationDirection.Down)
				{
					if (flag)
					{
						scrollHost.ScrollToHorizontalOffset(scrollHost.HorizontalOffset - scrollHost.ViewportWidth + rect.Right);
						return;
					}
					scrollHost.ScrollToVerticalOffset(scrollHost.VerticalOffset - scrollHost.ViewportHeight + rect.Bottom);
					return;
				}
				else if (direction == FocusNavigationDirection.Up)
				{
					if (flag)
					{
						scrollHost.ScrollToHorizontalOffset(scrollHost.HorizontalOffset + rect.Left);
						return;
					}
					scrollHost.ScrollToVerticalOffset(scrollHost.VerticalOffset + rect.Top);
				}
			}
		}

		// Token: 0x06006E29 RID: 28201 RVA: 0x002D04DC File Offset: 0x002CF4DC
		private void MakeVisible(int index, FocusNavigationDirection direction, bool alwaysAtTopOfViewport, out FrameworkElement container)
		{
			container = null;
			if (index >= 0)
			{
				container = (this.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement);
				if (container == null)
				{
					VirtualizingPanel virtualizingPanel = this.ItemsHost as VirtualizingPanel;
					if (virtualizingPanel != null)
					{
						virtualizingPanel.BringIndexIntoView(index);
						base.UpdateLayout();
						container = (this.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement);
					}
				}
				this.MakeVisible(container, direction, alwaysAtTopOfViewport);
			}
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x002D0545 File Offset: 0x002CF545
		private void MakeVisible(ItemsControl.ItemInfo info, FocusNavigationDirection direction, out FrameworkElement container)
		{
			if (info != null)
			{
				this.MakeVisible(info.Index, direction, false, out container);
				info.Container = container;
				return;
			}
			this.MakeVisible(-1, direction, false, out container);
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x002D0574 File Offset: 0x002CF574
		internal void MakeVisible(FrameworkElement container, FocusNavigationDirection direction, bool alwaysAtTopOfViewport)
		{
			if (this.ScrollHost != null && this.ItemsHost != null)
			{
				FrameworkElement viewportElement = this.GetViewportElement();
				while (container != null && !this.IsOnCurrentPage(viewportElement, container, direction, false))
				{
					double horizontalOffset = this.ScrollHost.HorizontalOffset;
					double verticalOffset = this.ScrollHost.VerticalOffset;
					container.BringIntoView();
					this.ItemsHost.UpdateLayout();
					if (DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset))
					{
						break;
					}
				}
				if (container != null && alwaysAtTopOfViewport)
				{
					bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
					FrameworkElement frameworkElement;
					this.GetFirstItemOnCurrentPage(container, FocusNavigationDirection.Up, out frameworkElement);
					while (frameworkElement != container)
					{
						double horizontalOffset2 = this.ScrollHost.HorizontalOffset;
						double verticalOffset = this.ScrollHost.VerticalOffset;
						if (flag)
						{
							this.ScrollHost.LineRight();
						}
						else
						{
							this.ScrollHost.LineDown();
						}
						this.ScrollHost.UpdateLayout();
						if (DoubleUtil.AreClose(horizontalOffset2, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset))
						{
							break;
						}
						this.GetFirstItemOnCurrentPage(container, FocusNavigationDirection.Up, out frameworkElement);
					}
				}
			}
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x002D06A4 File Offset: 0x002CF6A4
		private bool NavigateToFirstItemOnCurrentPage(object startingItem, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(this.ItemContainerGenerator.ContainerFromItem(startingItem) as FrameworkElement, direction, out container);
			return firstItemOnCurrentPage != DependencyProperty.UnsetValue && shouldFocus && this.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, container, -1), itemNavigateArgs);
		}

		// Token: 0x06006E2D RID: 28205 RVA: 0x002D06EC File Offset: 0x002CF6EC
		private object GetFirstItemOnCurrentPage(FrameworkElement startingElement, FocusNavigationDirection direction, out FrameworkElement firstElement)
		{
			bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
			bool flag2 = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Vertical;
			if (this.ScrollHost != null && this.ScrollHost.CanContentScroll && VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item && !(this is TreeView) && !this.IsGrouping)
			{
				int num = -1;
				if (flag2)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						return this.FindFocusable((int)this.ScrollHost.VerticalOffset, 1, out num, out firstElement);
					}
					return this.FindFocusable((int)(this.ScrollHost.VerticalOffset + Math.Max(this.ScrollHost.ViewportHeight - 1.0, 0.0)), -1, out num, out firstElement);
				}
				else if (flag)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						return this.FindFocusable((int)this.ScrollHost.HorizontalOffset, 1, out num, out firstElement);
					}
					return this.FindFocusable((int)(this.ScrollHost.HorizontalOffset + Math.Max(this.ScrollHost.ViewportWidth - 1.0, 0.0)), -1, out num, out firstElement);
				}
			}
			if (startingElement != null)
			{
				if (flag)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						direction = FocusNavigationDirection.Left;
					}
					else if (direction == FocusNavigationDirection.Down)
					{
						direction = FocusNavigationDirection.Right;
					}
				}
				FrameworkElement viewportElement = this.GetViewportElement();
				bool treeViewNavigation = this is TreeView;
				FrameworkElement frameworkElement = KeyboardNavigation.Current.PredictFocusedElementAtViewportEdge(startingElement, direction, treeViewNavigation, viewportElement, viewportElement) as FrameworkElement;
				object obj = null;
				firstElement = null;
				if (frameworkElement != null)
				{
					obj = ItemsControl.GetEncapsulatingItem(frameworkElement, out firstElement);
				}
				if (frameworkElement == null || obj == DependencyProperty.UnsetValue)
				{
					ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportElement, startingElement, direction, false);
					if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport || elementViewportPosition == ElementViewportPosition.PartiallyInViewport)
					{
						frameworkElement = startingElement;
						obj = ItemsControl.GetEncapsulatingItem(frameworkElement, out firstElement);
					}
				}
				if (obj != null && obj is CollectionViewGroupInternal)
				{
					firstElement = frameworkElement;
				}
				return obj;
			}
			firstElement = null;
			return null;
		}

		// Token: 0x06006E2E RID: 28206 RVA: 0x002D08C4 File Offset: 0x002CF8C4
		internal FrameworkElement GetViewportElement()
		{
			FrameworkElement frameworkElement = this.ScrollHost;
			if (frameworkElement == null)
			{
				frameworkElement = this.ItemsHost;
			}
			else
			{
				ScrollContentPresenter scrollContentPresenter = frameworkElement.GetTemplateChild("PART_ScrollContentPresenter") as ScrollContentPresenter;
				if (scrollContentPresenter != null)
				{
					frameworkElement = scrollContentPresenter;
				}
			}
			return frameworkElement;
		}

		// Token: 0x06006E2F RID: 28207 RVA: 0x002D08FC File Offset: 0x002CF8FC
		private bool IsOnCurrentPage(object item, FocusNavigationDirection axis)
		{
			FrameworkElement frameworkElement = this.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
			return frameworkElement != null && ItemsControl.GetElementViewportPosition(this.GetViewportElement(), frameworkElement, axis, false) == ElementViewportPosition.CompletelyInViewport;
		}

		// Token: 0x06006E30 RID: 28208 RVA: 0x002D0931 File Offset: 0x002CF931
		private bool IsOnCurrentPage(FrameworkElement element, FocusNavigationDirection axis)
		{
			return ItemsControl.GetElementViewportPosition(this.GetViewportElement(), element, axis, false) == ElementViewportPosition.CompletelyInViewport;
		}

		// Token: 0x06006E31 RID: 28209 RVA: 0x002D0944 File Offset: 0x002CF944
		private bool IsOnCurrentPage(FrameworkElement viewPort, FrameworkElement element, FocusNavigationDirection axis, bool fullyVisible)
		{
			return ItemsControl.GetElementViewportPosition(viewPort, element, axis, fullyVisible) == ElementViewportPosition.CompletelyInViewport;
		}

		// Token: 0x06006E32 RID: 28210 RVA: 0x002D0954 File Offset: 0x002CF954
		internal static ElementViewportPosition GetElementViewportPosition(FrameworkElement viewPort, UIElement element, FocusNavigationDirection axis, bool fullyVisible)
		{
			Rect rect;
			return ItemsControl.GetElementViewportPosition(viewPort, element, axis, fullyVisible, out rect);
		}

		// Token: 0x06006E33 RID: 28211 RVA: 0x002D096C File Offset: 0x002CF96C
		internal static ElementViewportPosition GetElementViewportPosition(FrameworkElement viewPort, UIElement element, FocusNavigationDirection axis, bool fullyVisible, out Rect elementRect)
		{
			return ItemsControl.GetElementViewportPosition(viewPort, element, axis, fullyVisible, false, out elementRect);
		}

		// Token: 0x06006E34 RID: 28212 RVA: 0x002D097C File Offset: 0x002CF97C
		internal static ElementViewportPosition GetElementViewportPosition(FrameworkElement viewPort, UIElement element, FocusNavigationDirection axis, bool fullyVisible, bool ignorePerpendicularAxis, out Rect elementRect)
		{
			elementRect = Rect.Empty;
			if (viewPort == null)
			{
				return ElementViewportPosition.None;
			}
			if (element == null || !viewPort.IsAncestorOf(element))
			{
				return ElementViewportPosition.None;
			}
			Rect viewportRect = new Rect(default(Point), viewPort.RenderSize);
			Rect rect = new Rect(default(Point), element.RenderSize);
			rect = ItemsControl.CorrectCatastrophicCancellation(element.TransformToAncestor(viewPort)).TransformBounds(rect);
			bool flag = axis == FocusNavigationDirection.Up || axis == FocusNavigationDirection.Down;
			bool flag2 = axis == FocusNavigationDirection.Left || axis == FocusNavigationDirection.Right;
			elementRect = rect;
			if (ignorePerpendicularAxis)
			{
				if (flag)
				{
					viewportRect = new Rect(double.NegativeInfinity, viewportRect.Top, double.PositiveInfinity, viewportRect.Height);
				}
				else if (flag2)
				{
					viewportRect = new Rect(viewportRect.Left, double.NegativeInfinity, viewportRect.Width, double.PositiveInfinity);
				}
			}
			if (fullyVisible)
			{
				if (viewportRect.Contains(rect))
				{
					return ElementViewportPosition.CompletelyInViewport;
				}
			}
			else if (flag)
			{
				if (DoubleUtil.LessThanOrClose(viewportRect.Top, rect.Top) && DoubleUtil.LessThanOrClose(rect.Bottom, viewportRect.Bottom))
				{
					return ElementViewportPosition.CompletelyInViewport;
				}
			}
			else if (flag2 && DoubleUtil.LessThanOrClose(viewportRect.Left, rect.Left) && DoubleUtil.LessThanOrClose(rect.Right, viewportRect.Right))
			{
				return ElementViewportPosition.CompletelyInViewport;
			}
			if (ItemsControl.ElementIntersectsViewport(viewportRect, rect))
			{
				return ElementViewportPosition.PartiallyInViewport;
			}
			if ((flag && DoubleUtil.LessThanOrClose(rect.Bottom, viewportRect.Top)) || (flag2 && DoubleUtil.LessThanOrClose(rect.Right, viewportRect.Left)))
			{
				return ElementViewportPosition.BeforeViewport;
			}
			if ((flag && DoubleUtil.LessThanOrClose(viewportRect.Bottom, rect.Top)) || (flag2 && DoubleUtil.LessThanOrClose(viewportRect.Right, rect.Left)))
			{
				return ElementViewportPosition.AfterViewport;
			}
			return ElementViewportPosition.None;
		}

		// Token: 0x06006E35 RID: 28213 RVA: 0x002D0B40 File Offset: 0x002CFB40
		private static GeneralTransform CorrectCatastrophicCancellation(GeneralTransform transform)
		{
			MatrixTransform matrixTransform = transform as MatrixTransform;
			if (matrixTransform != null)
			{
				bool flag = false;
				Matrix matrix = matrixTransform.Matrix;
				if (matrix.OffsetX != 0.0 && LayoutDoubleUtil.AreClose(matrix.OffsetX, 0.0))
				{
					matrix.OffsetX = 0.0;
					flag = true;
				}
				if (matrix.OffsetY != 0.0 && LayoutDoubleUtil.AreClose(matrix.OffsetY, 0.0))
				{
					matrix.OffsetY = 0.0;
					flag = true;
				}
				if (flag)
				{
					transform = new MatrixTransform(matrix);
				}
			}
			return transform;
		}

		// Token: 0x06006E36 RID: 28214 RVA: 0x002D0BE8 File Offset: 0x002CFBE8
		private static bool ElementIntersectsViewport(Rect viewportRect, Rect elementRect)
		{
			return !viewportRect.IsEmpty && !elementRect.IsEmpty && !DoubleUtil.LessThan(elementRect.Right, viewportRect.Left) && !LayoutDoubleUtil.AreClose(elementRect.Right, viewportRect.Left) && !DoubleUtil.GreaterThan(elementRect.Left, viewportRect.Right) && !LayoutDoubleUtil.AreClose(elementRect.Left, viewportRect.Right) && !DoubleUtil.LessThan(elementRect.Bottom, viewportRect.Top) && !LayoutDoubleUtil.AreClose(elementRect.Bottom, viewportRect.Top) && !DoubleUtil.GreaterThan(elementRect.Top, viewportRect.Bottom) && !LayoutDoubleUtil.AreClose(elementRect.Top, viewportRect.Bottom);
		}

		// Token: 0x06006E37 RID: 28215 RVA: 0x002D0CB8 File Offset: 0x002CFCB8
		private bool IsInDirectionForLineNavigation(Rect fromRect, Rect toRect, FocusNavigationDirection direction, bool isHorizontal)
		{
			if (direction == FocusNavigationDirection.Down)
			{
				if (isHorizontal)
				{
					return DoubleUtil.GreaterThanOrClose(toRect.Left, fromRect.Left);
				}
				return DoubleUtil.GreaterThanOrClose(toRect.Top, fromRect.Top);
			}
			else
			{
				if (direction != FocusNavigationDirection.Up)
				{
					return false;
				}
				if (isHorizontal)
				{
					return DoubleUtil.LessThanOrClose(toRect.Right, fromRect.Right);
				}
				return DoubleUtil.LessThanOrClose(toRect.Bottom, fromRect.Bottom);
			}
		}

		// Token: 0x06006E38 RID: 28216 RVA: 0x002D0D28 File Offset: 0x002CFD28
		private static void OnGotFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)sender;
			UIElement uielement = e.OriginalSource as UIElement;
			if (uielement != null && uielement != itemsControl)
			{
				object obj = itemsControl.ItemContainerGenerator.ItemFromContainer(uielement);
				if (obj != DependencyProperty.UnsetValue)
				{
					itemsControl._focusedInfo = itemsControl.NewItemInfo(obj, uielement, -1);
					return;
				}
				if (itemsControl._focusedInfo != null)
				{
					UIElement uielement2 = itemsControl._focusedInfo.Container as UIElement;
					if (uielement2 == null || !Helper.IsAnyAncestorOf(uielement2, uielement))
					{
						itemsControl._focusedInfo = null;
					}
				}
			}
		}

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x06006E39 RID: 28217 RVA: 0x002D0DA6 File Offset: 0x002CFDA6
		internal ItemsControl.ItemInfo FocusedInfo
		{
			get
			{
				return this._focusedInfo;
			}
		}

		// Token: 0x06006E3A RID: 28218 RVA: 0x002D0DB0 File Offset: 0x002CFDB0
		internal virtual bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			bool item = info.Item != null;
			bool result = false;
			if (item)
			{
				UIElement uielement = info.Container as UIElement;
				if (uielement != null)
				{
					result = uielement.Focus();
				}
			}
			if (itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				KeyboardNavigation.ShowFocusVisual();
			}
			return result;
		}

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x06006E3B RID: 28219 RVA: 0x002D0DF0 File Offset: 0x002CFDF0
		internal bool IsLogicalVertical
		{
			get
			{
				return this.ItemsHost != null && this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Vertical && this.ScrollHost != null && this.ScrollHost.CanContentScroll && VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item;
			}
		}

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x06006E3C RID: 28220 RVA: 0x002D0E40 File Offset: 0x002CFE40
		internal bool IsLogicalHorizontal
		{
			get
			{
				return this.ItemsHost != null && this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal && this.ScrollHost != null && this.ScrollHost.CanContentScroll && VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item;
			}
		}

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x06006E3D RID: 28221 RVA: 0x002D0E90 File Offset: 0x002CFE90
		internal ScrollViewer ScrollHost
		{
			get
			{
				if (!base.ReadControlFlag(Control.ControlBoolFlags.ScrollHostValid))
				{
					if (this._itemsHost == null)
					{
						return null;
					}
					DependencyObject dependencyObject = this._itemsHost;
					while (dependencyObject != this && dependencyObject != null)
					{
						ScrollViewer scrollViewer = dependencyObject as ScrollViewer;
						if (scrollViewer != null)
						{
							this._scrollHost = scrollViewer;
							break;
						}
						dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
					}
					base.WriteControlFlag(Control.ControlBoolFlags.ScrollHostValid, true);
				}
				return this._scrollHost;
			}
		}

		// Token: 0x1700197A RID: 6522
		// (get) Token: 0x06006E3E RID: 28222 RVA: 0x002D0EEA File Offset: 0x002CFEEA
		internal static TimeSpan AutoScrollTimeout
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)SafeNativeMethods.GetDoubleClickTime() * 0.8);
			}
		}

		// Token: 0x06006E3F RID: 28223 RVA: 0x002D0F01 File Offset: 0x002CFF01
		internal void DoAutoScroll()
		{
			this.DoAutoScroll(this.FocusedInfo);
		}

		// Token: 0x06006E40 RID: 28224 RVA: 0x002D0F10 File Offset: 0x002CFF10
		internal void DoAutoScroll(ItemsControl.ItemInfo startingInfo)
		{
			FrameworkElement frameworkElement = (this.ScrollHost != null) ? this.ScrollHost : this.ItemsHost;
			if (frameworkElement != null)
			{
				Point position = Mouse.GetPosition(frameworkElement);
				Rect rect = new Rect(default(Point), frameworkElement.RenderSize);
				bool flag = false;
				if (position.Y < rect.Top)
				{
					this.NavigateByLine(startingInfo, FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
					flag = (startingInfo != this.FocusedInfo);
				}
				else if (position.Y >= rect.Bottom)
				{
					this.NavigateByLine(startingInfo, FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
					flag = (startingInfo != this.FocusedInfo);
				}
				if (!flag)
				{
					if (position.X < rect.Left)
					{
						FocusNavigationDirection direction = FocusNavigationDirection.Left;
						if (this.IsRTL(frameworkElement))
						{
							direction = FocusNavigationDirection.Right;
						}
						this.NavigateByLine(startingInfo, direction, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
						return;
					}
					if (position.X >= rect.Right)
					{
						FocusNavigationDirection direction2 = FocusNavigationDirection.Right;
						if (this.IsRTL(frameworkElement))
						{
							direction2 = FocusNavigationDirection.Left;
						}
						this.NavigateByLine(startingInfo, direction2, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
					}
				}
			}
		}

		// Token: 0x06006E41 RID: 28225 RVA: 0x002D103A File Offset: 0x002D003A
		private bool IsRTL(FrameworkElement element)
		{
			return element.FlowDirection == FlowDirection.RightToLeft;
		}

		// Token: 0x06006E42 RID: 28226 RVA: 0x002D1048 File Offset: 0x002D0048
		private static ItemsControl GetEncapsulatingItemsControl(FrameworkElement element)
		{
			while (element != null)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(element);
				if (itemsControl != null)
				{
					return itemsControl;
				}
				element = (VisualTreeHelper.GetParent(element) as FrameworkElement);
			}
			return null;
		}

		// Token: 0x06006E43 RID: 28227 RVA: 0x002D1074 File Offset: 0x002D0074
		private static object GetEncapsulatingItem(FrameworkElement element, out FrameworkElement container)
		{
			ItemsControl itemsControl = null;
			return ItemsControl.GetEncapsulatingItem(element, out container, out itemsControl);
		}

		// Token: 0x06006E44 RID: 28228 RVA: 0x002D108C File Offset: 0x002D008C
		private static object GetEncapsulatingItem(FrameworkElement element, out FrameworkElement container, out ItemsControl itemsControl)
		{
			object obj = DependencyProperty.UnsetValue;
			itemsControl = null;
			while (element != null)
			{
				itemsControl = ItemsControl.ItemsControlFromItemContainer(element);
				if (itemsControl != null)
				{
					obj = itemsControl.ItemContainerGenerator.ItemFromContainer(element);
					if (obj != DependencyProperty.UnsetValue)
					{
						break;
					}
				}
				element = (VisualTreeHelper.GetParent(element) as FrameworkElement);
			}
			container = element;
			return obj;
		}

		// Token: 0x06006E45 RID: 28229 RVA: 0x002D10DC File Offset: 0x002D00DC
		internal static DependencyObject TryGetTreeViewItemHeader(DependencyObject element)
		{
			TreeViewItem treeViewItem = element as TreeViewItem;
			if (treeViewItem != null)
			{
				return treeViewItem.TryGetHeaderElement();
			}
			return element;
		}

		// Token: 0x06006E46 RID: 28230 RVA: 0x002D10FC File Offset: 0x002D00FC
		private void ApplyItemContainerStyle(DependencyObject container, object item)
		{
			FrameworkObject frameworkObject = new FrameworkObject(container);
			if (!frameworkObject.IsStyleSetFromGenerator && container.ReadLocalValue(FrameworkElement.StyleProperty) != DependencyProperty.UnsetValue)
			{
				return;
			}
			Style style = this.ItemContainerStyle;
			if (style == null && this.ItemContainerStyleSelector != null)
			{
				style = this.ItemContainerStyleSelector.SelectStyle(item, container);
			}
			if (style == null)
			{
				if (frameworkObject.IsStyleSetFromGenerator)
				{
					frameworkObject.IsStyleSetFromGenerator = false;
					container.ClearValue(FrameworkElement.StyleProperty);
				}
				return;
			}
			if (!style.TargetType.IsInstanceOfType(container))
			{
				throw new InvalidOperationException(SR.Get("StyleForWrongType", new object[]
				{
					style.TargetType.Name,
					container.GetType().Name
				}));
			}
			frameworkObject.Style = style;
			frameworkObject.IsStyleSetFromGenerator = true;
		}

		// Token: 0x06006E47 RID: 28231 RVA: 0x002D11C0 File Offset: 0x002D01C0
		private void RemoveItemContainerStyle(DependencyObject container)
		{
			FrameworkObject frameworkObject = new FrameworkObject(container);
			if (frameworkObject.IsStyleSetFromGenerator)
			{
				container.ClearValue(FrameworkElement.StyleProperty);
			}
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x002D11EC File Offset: 0x002D01EC
		internal object GetItemOrContainerFromContainer(DependencyObject container)
		{
			object obj = this.ItemContainerGenerator.ItemFromContainer(container);
			if (obj == DependencyProperty.UnsetValue && ItemsControl.ItemsControlFromItemContainer(container) == this && ((IGeneratorHost)this).IsItemItsOwnContainer(container))
			{
				obj = container;
			}
			return obj;
		}

		// Token: 0x06006E49 RID: 28233 RVA: 0x002D1224 File Offset: 0x002D0224
		internal static bool EqualsEx(object o1, object o2)
		{
			bool result;
			try
			{
				result = object.Equals(o1, o2);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06006E4A RID: 28234 RVA: 0x002D1254 File Offset: 0x002D0254
		internal ItemsControl.ItemInfo NewItemInfo(object item, DependencyObject container = null, int index = -1)
		{
			return new ItemsControl.ItemInfo(item, container, index).Refresh(this.ItemContainerGenerator);
		}

		// Token: 0x06006E4B RID: 28235 RVA: 0x002D1269 File Offset: 0x002D0269
		internal ItemsControl.ItemInfo ItemInfoFromContainer(DependencyObject container)
		{
			return this.NewItemInfo(this.ItemContainerGenerator.ItemFromContainer(container), container, this.ItemContainerGenerator.IndexFromContainer(container));
		}

		// Token: 0x06006E4C RID: 28236 RVA: 0x002D128A File Offset: 0x002D028A
		internal ItemsControl.ItemInfo ItemInfoFromIndex(int index)
		{
			if (index < 0)
			{
				return null;
			}
			return this.NewItemInfo(this.Items[index], this.ItemContainerGenerator.ContainerFromIndex(index), index);
		}

		// Token: 0x06006E4D RID: 28237 RVA: 0x002D12B1 File Offset: 0x002D02B1
		internal ItemsControl.ItemInfo NewUnresolvedItemInfo(object item)
		{
			return new ItemsControl.ItemInfo(item, ItemsControl.ItemInfo.UnresolvedContainer, -1);
		}

		// Token: 0x06006E4E RID: 28238 RVA: 0x002D12C0 File Offset: 0x002D02C0
		internal DependencyObject ContainerFromItemInfo(ItemsControl.ItemInfo info)
		{
			DependencyObject dependencyObject = info.Container;
			if (dependencyObject == null)
			{
				if (info.Index >= 0)
				{
					dependencyObject = this.ItemContainerGenerator.ContainerFromIndex(info.Index);
					info.Container = dependencyObject;
				}
				else
				{
					dependencyObject = this.ItemContainerGenerator.ContainerFromItem(info.Item);
				}
			}
			return dependencyObject;
		}

		// Token: 0x06006E4F RID: 28239 RVA: 0x002D1310 File Offset: 0x002D0310
		internal void AdjustItemInfoAfterGeneratorChange(ItemsControl.ItemInfo info)
		{
			if (info != null)
			{
				ItemsControl.ItemInfo[] list = new ItemsControl.ItemInfo[]
				{
					info
				};
				this.AdjustItemInfosAfterGeneratorChange(list, false);
			}
		}

		// Token: 0x06006E50 RID: 28240 RVA: 0x002D133C File Offset: 0x002D033C
		internal void AdjustItemInfosAfterGeneratorChange(IEnumerable<ItemsControl.ItemInfo> list, bool claimUniqueContainer)
		{
			bool flag = false;
			foreach (ItemsControl.ItemInfo itemInfo in list)
			{
				DependencyObject container = itemInfo.Container;
				if (container == null)
				{
					flag = true;
				}
				else if (itemInfo.IsRemoved || !ItemsControl.EqualsEx(itemInfo.Item, container.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty)))
				{
					itemInfo.Container = null;
					flag = true;
				}
			}
			if (flag)
			{
				List<DependencyObject> claimedContainers = new List<DependencyObject>();
				if (claimUniqueContainer)
				{
					foreach (ItemsControl.ItemInfo itemInfo2 in list)
					{
						DependencyObject container2 = itemInfo2.Container;
						if (container2 != null)
						{
							claimedContainers.Add(container2);
						}
					}
				}
				foreach (ItemsControl.ItemInfo itemInfo3 in list)
				{
					DependencyObject dependencyObject = itemInfo3.Container;
					if (dependencyObject == null)
					{
						int index = itemInfo3.Index;
						if (index >= 0)
						{
							dependencyObject = this.ItemContainerGenerator.ContainerFromIndex(index);
						}
						else
						{
							object item = itemInfo3.Item;
							this.ItemContainerGenerator.FindItem((object o, DependencyObject d) => ItemsControl.EqualsEx(o, item) && !claimedContainers.Contains(d), out dependencyObject, out index);
						}
						if (dependencyObject != null)
						{
							itemInfo3.Container = dependencyObject;
							itemInfo3.Index = index;
							if (claimUniqueContainer)
							{
								claimedContainers.Add(dependencyObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006E51 RID: 28241 RVA: 0x002D14E4 File Offset: 0x002D04E4
		internal void AdjustItemInfo(NotifyCollectionChangedEventArgs e, ItemsControl.ItemInfo info)
		{
			if (info != null)
			{
				ItemsControl.ItemInfo[] list = new ItemsControl.ItemInfo[]
				{
					info
				};
				this.AdjustItemInfos(e, list);
			}
		}

		// Token: 0x06006E52 RID: 28242 RVA: 0x002D1510 File Offset: 0x002D0510
		internal void AdjustItemInfos(NotifyCollectionChangedEventArgs e, IEnumerable<ItemsControl.ItemInfo> list)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				using (IEnumerator<ItemsControl.ItemInfo> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemsControl.ItemInfo itemInfo = enumerator.Current;
						int index = itemInfo.Index;
						if (index >= e.NewStartingIndex)
						{
							itemInfo.Index = index + 1;
						}
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				break;
			case NotifyCollectionChangedAction.Replace:
				return;
			case NotifyCollectionChangedAction.Move:
				goto IL_CC;
			case NotifyCollectionChangedAction.Reset:
				goto IL_161;
			default:
				return;
			}
			using (IEnumerator<ItemsControl.ItemInfo> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemsControl.ItemInfo itemInfo2 = enumerator.Current;
					int index2 = itemInfo2.Index;
					if (index2 > e.OldStartingIndex)
					{
						itemInfo2.Index = index2 - 1;
					}
					else if (index2 == e.OldStartingIndex)
					{
						itemInfo2.Index = -1;
					}
				}
				return;
			}
			IL_CC:
			int num;
			int num2;
			int num3;
			if (e.OldStartingIndex < e.NewStartingIndex)
			{
				num = e.OldStartingIndex + 1;
				num2 = e.NewStartingIndex;
				num3 = -1;
			}
			else
			{
				num = e.NewStartingIndex;
				num2 = e.OldStartingIndex - 1;
				num3 = 1;
			}
			using (IEnumerator<ItemsControl.ItemInfo> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemsControl.ItemInfo itemInfo3 = enumerator.Current;
					int index3 = itemInfo3.Index;
					if (index3 == e.OldStartingIndex)
					{
						itemInfo3.Index = e.NewStartingIndex;
					}
					else if (num <= index3 && index3 <= num2)
					{
						itemInfo3.Index = index3 + num3;
					}
				}
				return;
			}
			IL_161:
			foreach (ItemsControl.ItemInfo itemInfo4 in list)
			{
				itemInfo4.Index = -1;
				itemInfo4.Container = null;
			}
		}

		// Token: 0x06006E53 RID: 28243 RVA: 0x002D16E8 File Offset: 0x002D06E8
		internal ItemsControl.ItemInfo LeaseItemInfo(ItemsControl.ItemInfo info, bool ensureIndex = false)
		{
			if (info.Index < 0)
			{
				info = this.NewItemInfo(info.Item, null, -1);
				if (ensureIndex && info.Index < 0)
				{
					info.Index = this.Items.IndexOf(info.Item);
				}
			}
			return info;
		}

		// Token: 0x06006E54 RID: 28244 RVA: 0x002D1727 File Offset: 0x002D0727
		internal void RefreshItemInfo(ItemsControl.ItemInfo info)
		{
			if (info != null)
			{
				info.Refresh(this.ItemContainerGenerator);
			}
		}

		// Token: 0x06006E55 RID: 28245 RVA: 0x002C5F9F File Offset: 0x002C4F9F
		object IContainItemStorage.ReadItemValue(object item, DependencyProperty dp)
		{
			return Helper.ReadItemValue(this, item, dp.GlobalIndex);
		}

		// Token: 0x06006E56 RID: 28246 RVA: 0x002C5FAE File Offset: 0x002C4FAE
		void IContainItemStorage.StoreItemValue(object item, DependencyProperty dp, object value)
		{
			Helper.StoreItemValue(this, item, dp.GlobalIndex, value);
		}

		// Token: 0x06006E57 RID: 28247 RVA: 0x002C5FBE File Offset: 0x002C4FBE
		void IContainItemStorage.ClearItemValue(object item, DependencyProperty dp)
		{
			Helper.ClearItemValue(this, item, dp.GlobalIndex);
		}

		// Token: 0x06006E58 RID: 28248 RVA: 0x002C5FCD File Offset: 0x002C4FCD
		void IContainItemStorage.ClearValue(DependencyProperty dp)
		{
			Helper.ClearItemValueStorage(this, new int[]
			{
				dp.GlobalIndex
			});
		}

		// Token: 0x06006E59 RID: 28249 RVA: 0x002C5FE4 File Offset: 0x002C4FE4
		void IContainItemStorage.Clear()
		{
			Helper.ClearItemValueStorage(this);
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x002D1740 File Offset: 0x002D0740
		public override string ToString()
		{
			int num = this.HasItems ? this.Items.Count : 0;
			return SR.Get("ToStringFormatString_ItemsControl", new object[]
			{
				base.GetType(),
				num
			});
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x002D1786 File Offset: 0x002D0786
		internal override AutomationPeer OnCreateAutomationPeerInternal()
		{
			return new ItemsControlWrapperAutomationPeer(this);
		}

		// Token: 0x1700197B RID: 6523
		// (get) Token: 0x06006E5C RID: 28252 RVA: 0x002D178E File Offset: 0x002D078E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ItemsControl._dType;
			}
		}

		// Token: 0x04003632 RID: 13874
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemsSourceChanged)));

		// Token: 0x04003633 RID: 13875
		internal static readonly DependencyPropertyKey HasItemsPropertyKey = DependencyProperty.RegisterReadOnly("HasItems", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x04003634 RID: 13876
		public static readonly DependencyProperty HasItemsProperty = ItemsControl.HasItemsPropertyKey.DependencyProperty;

		// Token: 0x04003635 RID: 13877
		public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ItemsControl), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(ItemsControl.OnDisplayMemberPathChanged)));

		// Token: 0x04003636 RID: 13878
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemTemplateChanged)));

		// Token: 0x04003637 RID: 13879
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemTemplateSelectorChanged)));

		// Token: 0x04003638 RID: 13880
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemStringFormatProperty = DependencyProperty.Register("ItemStringFormat", typeof(string), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemStringFormatChanged)));

		// Token: 0x04003639 RID: 13881
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemBindingGroupProperty = DependencyProperty.Register("ItemBindingGroup", typeof(BindingGroup), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemBindingGroupChanged)));

		// Token: 0x0400363A RID: 13882
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemContainerStyleChanged)));

		// Token: 0x0400363B RID: 13883
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemContainerStyleSelectorProperty = DependencyProperty.Register("ItemContainerStyleSelector", typeof(StyleSelector), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemContainerStyleSelectorChanged)));

		// Token: 0x0400363C RID: 13884
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemsPanelProperty = DependencyProperty.Register("ItemsPanel", typeof(ItemsPanelTemplate), typeof(ItemsControl), new FrameworkPropertyMetadata(ItemsControl.GetDefaultItemsPanelTemplate(), new PropertyChangedCallback(ItemsControl.OnItemsPanelChanged)));

		// Token: 0x0400363D RID: 13885
		private static readonly DependencyPropertyKey IsGroupingPropertyKey = DependencyProperty.RegisterReadOnly("IsGrouping", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(ItemsControl.OnIsGroupingChanged)));

		// Token: 0x0400363E RID: 13886
		public static readonly DependencyProperty IsGroupingProperty = ItemsControl.IsGroupingPropertyKey.DependencyProperty;

		// Token: 0x0400363F RID: 13887
		public static readonly DependencyProperty GroupStyleSelectorProperty = DependencyProperty.Register("GroupStyleSelector", typeof(GroupStyleSelector), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnGroupStyleSelectorChanged)));

		// Token: 0x04003640 RID: 13888
		public static readonly DependencyProperty AlternationCountProperty = DependencyProperty.Register("AlternationCount", typeof(int), typeof(ItemsControl), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ItemsControl.OnAlternationCountChanged)));

		// Token: 0x04003641 RID: 13889
		private static readonly DependencyPropertyKey AlternationIndexPropertyKey = DependencyProperty.RegisterAttachedReadOnly("AlternationIndex", typeof(int), typeof(ItemsControl), new FrameworkPropertyMetadata(0));

		// Token: 0x04003642 RID: 13890
		public static readonly DependencyProperty AlternationIndexProperty = ItemsControl.AlternationIndexPropertyKey.DependencyProperty;

		// Token: 0x04003643 RID: 13891
		public static readonly DependencyProperty IsTextSearchEnabledProperty = DependencyProperty.Register("IsTextSearchEnabled", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003644 RID: 13892
		public static readonly DependencyProperty IsTextSearchCaseSensitiveProperty = DependencyProperty.Register("IsTextSearchCaseSensitive", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003645 RID: 13893
		private ItemsControl.ItemInfo _focusedInfo;

		// Token: 0x04003646 RID: 13894
		private ItemCollection _items;

		// Token: 0x04003647 RID: 13895
		private ItemContainerGenerator _itemContainerGenerator;

		// Token: 0x04003648 RID: 13896
		private Panel _itemsHost;

		// Token: 0x04003649 RID: 13897
		private ScrollViewer _scrollHost;

		// Token: 0x0400364A RID: 13898
		private ObservableCollection<GroupStyle> _groupStyle = new ObservableCollection<GroupStyle>();

		// Token: 0x0400364B RID: 13899
		private static readonly UncommonField<bool> ShouldCoerceScrollUnitField = new UncommonField<bool>();

		// Token: 0x0400364C RID: 13900
		private static readonly UncommonField<bool> ShouldCoerceCacheSizeField = new UncommonField<bool>();

		// Token: 0x0400364D RID: 13901
		private static DependencyObjectType _dType;

		// Token: 0x02000C07 RID: 3079
		internal class ItemNavigateArgs
		{
			// Token: 0x06009037 RID: 36919 RVA: 0x0034633C File Offset: 0x0034533C
			public ItemNavigateArgs(InputDevice deviceUsed, ModifierKeys modifierKeys)
			{
				this._deviceUsed = deviceUsed;
				this._modifierKeys = modifierKeys;
			}

			// Token: 0x17001F81 RID: 8065
			// (get) Token: 0x06009038 RID: 36920 RVA: 0x00346352 File Offset: 0x00345352
			public InputDevice DeviceUsed
			{
				get
				{
					return this._deviceUsed;
				}
			}

			// Token: 0x17001F82 RID: 8066
			// (get) Token: 0x06009039 RID: 36921 RVA: 0x0034635A File Offset: 0x0034535A
			public static ItemsControl.ItemNavigateArgs Empty
			{
				get
				{
					if (ItemsControl.ItemNavigateArgs._empty == null)
					{
						ItemsControl.ItemNavigateArgs._empty = new ItemsControl.ItemNavigateArgs(null, ModifierKeys.None);
					}
					return ItemsControl.ItemNavigateArgs._empty;
				}
			}

			// Token: 0x04004ABC RID: 19132
			private InputDevice _deviceUsed;

			// Token: 0x04004ABD RID: 19133
			private ModifierKeys _modifierKeys;

			// Token: 0x04004ABE RID: 19134
			private static ItemsControl.ItemNavigateArgs _empty;
		}

		// Token: 0x02000C08 RID: 3080
		[DebuggerDisplay("Index: {Index}  Item: {Item}")]
		internal class ItemInfo
		{
			// Token: 0x17001F83 RID: 8067
			// (get) Token: 0x0600903A RID: 36922 RVA: 0x00346374 File Offset: 0x00345374
			// (set) Token: 0x0600903B RID: 36923 RVA: 0x0034637C File Offset: 0x0034537C
			internal object Item { get; private set; }

			// Token: 0x17001F84 RID: 8068
			// (get) Token: 0x0600903C RID: 36924 RVA: 0x00346385 File Offset: 0x00345385
			// (set) Token: 0x0600903D RID: 36925 RVA: 0x0034638D File Offset: 0x0034538D
			internal DependencyObject Container { get; set; }

			// Token: 0x17001F85 RID: 8069
			// (get) Token: 0x0600903E RID: 36926 RVA: 0x00346396 File Offset: 0x00345396
			// (set) Token: 0x0600903F RID: 36927 RVA: 0x0034639E File Offset: 0x0034539E
			internal int Index { get; set; }

			// Token: 0x06009040 RID: 36928 RVA: 0x003463A8 File Offset: 0x003453A8
			static ItemInfo()
			{
				ItemsControl.ItemInfo.SentinelContainer.MakeSentinel();
				ItemsControl.ItemInfo.UnresolvedContainer.MakeSentinel();
				ItemsControl.ItemInfo.KeyContainer.MakeSentinel();
				ItemsControl.ItemInfo.RemovedContainer.MakeSentinel();
			}

			// Token: 0x06009041 RID: 36929 RVA: 0x00346405 File Offset: 0x00345405
			public ItemInfo(object item, DependencyObject container = null, int index = -1)
			{
				this.Item = item;
				this.Container = container;
				this.Index = index;
			}

			// Token: 0x17001F86 RID: 8070
			// (get) Token: 0x06009042 RID: 36930 RVA: 0x00346422 File Offset: 0x00345422
			internal bool IsResolved
			{
				get
				{
					return this.Container != ItemsControl.ItemInfo.UnresolvedContainer;
				}
			}

			// Token: 0x17001F87 RID: 8071
			// (get) Token: 0x06009043 RID: 36931 RVA: 0x00346434 File Offset: 0x00345434
			internal bool IsKey
			{
				get
				{
					return this.Container == ItemsControl.ItemInfo.KeyContainer;
				}
			}

			// Token: 0x17001F88 RID: 8072
			// (get) Token: 0x06009044 RID: 36932 RVA: 0x00346443 File Offset: 0x00345443
			internal bool IsRemoved
			{
				get
				{
					return this.Container == ItemsControl.ItemInfo.RemovedContainer;
				}
			}

			// Token: 0x06009045 RID: 36933 RVA: 0x00346452 File Offset: 0x00345452
			internal ItemsControl.ItemInfo Clone()
			{
				return new ItemsControl.ItemInfo(this.Item, this.Container, this.Index);
			}

			// Token: 0x06009046 RID: 36934 RVA: 0x0034646B File Offset: 0x0034546B
			internal static ItemsControl.ItemInfo Key(ItemsControl.ItemInfo info)
			{
				if (info.Container != ItemsControl.ItemInfo.UnresolvedContainer)
				{
					return info;
				}
				return new ItemsControl.ItemInfo(info.Item, ItemsControl.ItemInfo.KeyContainer, -1);
			}

			// Token: 0x06009047 RID: 36935 RVA: 0x0034648D File Offset: 0x0034548D
			public override int GetHashCode()
			{
				if (this.Item == null)
				{
					return 314159;
				}
				return this.Item.GetHashCode();
			}

			// Token: 0x06009048 RID: 36936 RVA: 0x003464A8 File Offset: 0x003454A8
			public override bool Equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				ItemsControl.ItemInfo itemInfo = o as ItemsControl.ItemInfo;
				return !(itemInfo == null) && this.Equals(itemInfo, false);
			}

			// Token: 0x06009049 RID: 36937 RVA: 0x003464D8 File Offset: 0x003454D8
			internal bool Equals(ItemsControl.ItemInfo that, bool matchUnresolved)
			{
				if (this.IsRemoved || that.IsRemoved)
				{
					return false;
				}
				if (!ItemsControl.EqualsEx(this.Item, that.Item))
				{
					return false;
				}
				if (this.Container == ItemsControl.ItemInfo.KeyContainer)
				{
					return matchUnresolved || that.Container != ItemsControl.ItemInfo.UnresolvedContainer;
				}
				if (that.Container == ItemsControl.ItemInfo.KeyContainer)
				{
					return matchUnresolved || this.Container != ItemsControl.ItemInfo.UnresolvedContainer;
				}
				if (this.Container == ItemsControl.ItemInfo.UnresolvedContainer || that.Container == ItemsControl.ItemInfo.UnresolvedContainer)
				{
					return false;
				}
				if (this.Container != that.Container)
				{
					return this.Container == ItemsControl.ItemInfo.SentinelContainer || that.Container == ItemsControl.ItemInfo.SentinelContainer || ((this.Container == null || that.Container == null) && (this.Index < 0 || that.Index < 0 || this.Index == that.Index));
				}
				if (this.Container != ItemsControl.ItemInfo.SentinelContainer)
				{
					return this.Index < 0 || that.Index < 0 || this.Index == that.Index;
				}
				return this.Index == that.Index;
			}

			// Token: 0x0600904A RID: 36938 RVA: 0x002F0D85 File Offset: 0x002EFD85
			public static bool operator ==(ItemsControl.ItemInfo info1, ItemsControl.ItemInfo info2)
			{
				return object.Equals(info1, info2);
			}

			// Token: 0x0600904B RID: 36939 RVA: 0x002F0D8E File Offset: 0x002EFD8E
			public static bool operator !=(ItemsControl.ItemInfo info1, ItemsControl.ItemInfo info2)
			{
				return !object.Equals(info1, info2);
			}

			// Token: 0x0600904C RID: 36940 RVA: 0x0034660C File Offset: 0x0034560C
			internal ItemsControl.ItemInfo Refresh(ItemContainerGenerator generator)
			{
				if (this.Container == null && this.Index < 0)
				{
					this.Container = generator.ContainerFromItem(this.Item);
				}
				if (this.Index < 0 && this.Container != null)
				{
					this.Index = generator.IndexFromContainer(this.Container);
				}
				if (this.Container == null && this.Index >= 0)
				{
					this.Container = generator.ContainerFromIndex(this.Index);
				}
				if (this.Container == ItemsControl.ItemInfo.SentinelContainer && this.Index >= 0)
				{
					this.Container = null;
				}
				return this;
			}

			// Token: 0x0600904D RID: 36941 RVA: 0x003466A0 File Offset: 0x003456A0
			internal void Reset(object item)
			{
				this.Item = item;
			}

			// Token: 0x04004AC2 RID: 19138
			internal static readonly DependencyObject SentinelContainer = new DependencyObject();

			// Token: 0x04004AC3 RID: 19139
			internal static readonly DependencyObject UnresolvedContainer = new DependencyObject();

			// Token: 0x04004AC4 RID: 19140
			internal static readonly DependencyObject KeyContainer = new DependencyObject();

			// Token: 0x04004AC5 RID: 19141
			internal static readonly DependencyObject RemovedContainer = new DependencyObject();
		}
	}
}
