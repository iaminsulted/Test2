using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000855 RID: 2133
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	[DefaultProperty("SelectedIndex")]
	[DefaultEvent("SelectionChanged")]
	public abstract class Selector : ItemsControl
	{
		// Token: 0x06007D29 RID: 32041 RVA: 0x0031243C File Offset: 0x0031143C
		protected Selector()
		{
			base.Items.CurrentChanged += this.OnCurrentChanged;
			base.ItemContainerGenerator.StatusChanged += this.OnGeneratorStatusChanged;
			this._focusEnterMainFocusScopeEventHandler = new EventHandler(this.OnFocusEnterMainFocusScope);
			KeyboardNavigation.Current.FocusEnterMainFocusScope += this._focusEnterMainFocusScopeEventHandler;
			ObservableCollection<object> observableCollection = new SelectedItemCollection(this);
			base.SetValue(Selector.SelectedItemsPropertyKey, observableCollection);
			observableCollection.CollectionChanged += this.OnSelectedItemsCollectionChanged;
			base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.FalseBox);
		}

		// Token: 0x06007D2A RID: 32042 RVA: 0x003124F4 File Offset: 0x003114F4
		static Selector()
		{
			Selector.SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(Selector));
			Selector.SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));
			Selector.UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Selector));
			Selector.IsSelectionActivePropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsSelectionActive", typeof(bool), typeof(Selector), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			Selector.IsSelectionActiveProperty = Selector.IsSelectionActivePropertyKey.DependencyProperty;
			Selector.IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(Selector), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
			Selector.IsSynchronizedWithCurrentItemProperty = DependencyProperty.Register("IsSynchronizedWithCurrentItem", typeof(bool?), typeof(Selector), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Selector.OnIsSynchronizedWithCurrentItemChanged)));
			Selector.SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Selector), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(Selector.OnSelectedIndexChanged), new CoerceValueCallback(Selector.CoerceSelectedIndex)), new ValidateValueCallback(Selector.ValidateSelectedIndex));
			Selector.SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(Selector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Selector.OnSelectedItemChanged), new CoerceValueCallback(Selector.CoerceSelectedItem)));
			Selector.SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(Selector), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Selector.OnSelectedValueChanged), new CoerceValueCallback(Selector.CoerceSelectedValue)));
			Selector.SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(Selector), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(Selector.OnSelectedValuePathChanged)));
			Selector.SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItems", typeof(IList), typeof(Selector), new FrameworkPropertyMetadata(null));
			Selector.SelectedItemsImplProperty = Selector.SelectedItemsPropertyKey.DependencyProperty;
			Selector.ItemValueBindingExpression = new BindingExpressionUncommonField();
			Selector.PendingSelectionByValueField = new UncommonField<ItemsControl.ItemInfo>();
			Selector.MatchExplicitEqualityComparer = new Selector.ItemInfoEqualityComparer(false);
			Selector.MatchUnresolvedEqualityComparer = new Selector.ItemInfoEqualityComparer(true);
			Selector.ChangeInfoField = new UncommonField<Selector.ChangeInfo>();
			EventManager.RegisterClassHandler(typeof(Selector), Selector.SelectedEvent, new RoutedEventHandler(Selector.OnSelected));
			EventManager.RegisterClassHandler(typeof(Selector), Selector.UnselectedEvent, new RoutedEventHandler(Selector.OnUnselected));
		}

		// Token: 0x1400015B RID: 347
		// (add) Token: 0x06007D2B RID: 32043 RVA: 0x003127DA File Offset: 0x003117DA
		// (remove) Token: 0x06007D2C RID: 32044 RVA: 0x003127E8 File Offset: 0x003117E8
		[Category("Behavior")]
		public event SelectionChangedEventHandler SelectionChanged
		{
			add
			{
				base.AddHandler(Selector.SelectionChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Selector.SelectionChangedEvent, value);
			}
		}

		// Token: 0x06007D2D RID: 32045 RVA: 0x003127F6 File Offset: 0x003117F6
		public static void AddSelectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.AddHandler(element, Selector.SelectedEvent, handler);
		}

		// Token: 0x06007D2E RID: 32046 RVA: 0x00312804 File Offset: 0x00311804
		public static void RemoveSelectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.RemoveHandler(element, Selector.SelectedEvent, handler);
		}

		// Token: 0x06007D2F RID: 32047 RVA: 0x00312812 File Offset: 0x00311812
		public static void AddUnselectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.AddHandler(element, Selector.UnselectedEvent, handler);
		}

		// Token: 0x06007D30 RID: 32048 RVA: 0x00312820 File Offset: 0x00311820
		public static void RemoveUnselectedHandler(DependencyObject element, RoutedEventHandler handler)
		{
			UIElement.RemoveHandler(element, Selector.UnselectedEvent, handler);
		}

		// Token: 0x06007D31 RID: 32049 RVA: 0x0031282E File Offset: 0x0031182E
		public static bool GetIsSelectionActive(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Selector.IsSelectionActiveProperty);
		}

		// Token: 0x06007D32 RID: 32050 RVA: 0x0031284E File Offset: 0x0031184E
		[AttachedPropertyBrowsableForChildren]
		public static bool GetIsSelected(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Selector.IsSelectedProperty);
		}

		// Token: 0x06007D33 RID: 32051 RVA: 0x0031286E File Offset: 0x0031186E
		public static void SetIsSelected(DependencyObject element, bool isSelected)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Selector.IsSelectedProperty, BooleanBoxes.Box(isSelected));
		}

		// Token: 0x17001CE4 RID: 7396
		// (get) Token: 0x06007D34 RID: 32052 RVA: 0x0031288F File Offset: 0x0031188F
		// (set) Token: 0x06007D35 RID: 32053 RVA: 0x003128A1 File Offset: 0x003118A1
		[Bindable(true)]
		[Category("Behavior")]
		[TypeConverter("System.Windows.NullableBoolConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public bool? IsSynchronizedWithCurrentItem
		{
			get
			{
				return (bool?)base.GetValue(Selector.IsSynchronizedWithCurrentItemProperty);
			}
			set
			{
				base.SetValue(Selector.IsSynchronizedWithCurrentItemProperty, value);
			}
		}

		// Token: 0x06007D36 RID: 32054 RVA: 0x003128B4 File Offset: 0x003118B4
		private static void OnIsSynchronizedWithCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Selector)d).SetSynchronizationWithCurrentItem();
		}

		// Token: 0x06007D37 RID: 32055 RVA: 0x003128C4 File Offset: 0x003118C4
		private void SetSynchronizationWithCurrentItem()
		{
			bool? isSynchronizedWithCurrentItem = this.IsSynchronizedWithCurrentItem;
			bool isSynchronizedWithCurrentItemPrivate = this.IsSynchronizedWithCurrentItemPrivate;
			bool flag;
			if (isSynchronizedWithCurrentItem != null)
			{
				flag = isSynchronizedWithCurrentItem.Value;
			}
			else
			{
				if (!base.IsInitialized)
				{
					return;
				}
				flag = ((SelectionMode)base.GetValue(ListBox.SelectionModeProperty) == SelectionMode.Single && !CollectionViewSource.IsDefaultView(base.Items.CollectionView));
			}
			this.IsSynchronizedWithCurrentItemPrivate = flag;
			if (!isSynchronizedWithCurrentItemPrivate && flag)
			{
				if (this.SelectedItem != null)
				{
					this.SetCurrentToSelected();
					return;
				}
				this.SetSelectedToCurrent();
			}
		}

		// Token: 0x17001CE5 RID: 7397
		// (get) Token: 0x06007D38 RID: 32056 RVA: 0x00312948 File Offset: 0x00311948
		// (set) Token: 0x06007D39 RID: 32057 RVA: 0x0031295A File Offset: 0x0031195A
		[Localizability(LocalizationCategory.NeverLocalize)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(true)]
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(Selector.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedIndexProperty, value);
			}
		}

		// Token: 0x06007D3A RID: 32058 RVA: 0x00312970 File Offset: 0x00311970
		private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)d;
			if (!selector.SelectionChange.IsActive)
			{
				int index = (int)e.NewValue;
				selector.SelectionChange.SelectJustThisItem(selector.ItemInfoFromIndex(index), true);
			}
		}

		// Token: 0x06007D3B RID: 32059 RVA: 0x003129B1 File Offset: 0x003119B1
		private static bool ValidateSelectedIndex(object o)
		{
			return (int)o >= -1;
		}

		// Token: 0x06007D3C RID: 32060 RVA: 0x003129C0 File Offset: 0x003119C0
		private static object CoerceSelectedIndex(DependencyObject d, object value)
		{
			Selector selector = (Selector)d;
			if (value is int && (int)value >= selector.Items.Count)
			{
				return DependencyProperty.UnsetValue;
			}
			return value;
		}

		// Token: 0x17001CE6 RID: 7398
		// (get) Token: 0x06007D3D RID: 32061 RVA: 0x003129F6 File Offset: 0x003119F6
		// (set) Token: 0x06007D3E RID: 32062 RVA: 0x00312A03 File Offset: 0x00311A03
		[Category("Appearance")]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				return base.GetValue(Selector.SelectedItemProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedItemProperty, value);
			}
		}

		// Token: 0x06007D3F RID: 32063 RVA: 0x00312A14 File Offset: 0x00311A14
		private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)d;
			if (!selector.SelectionChange.IsActive)
			{
				selector.SelectionChange.SelectJustThisItem(selector.NewItemInfo(e.NewValue, null, -1), false);
			}
		}

		// Token: 0x06007D40 RID: 32064 RVA: 0x00312A50 File Offset: 0x00311A50
		private static object CoerceSelectedItem(DependencyObject d, object value)
		{
			Selector selector = (Selector)d;
			if (value == null || selector.SkipCoerceSelectedItemCheck)
			{
				return value;
			}
			int selectedIndex = selector.SelectedIndex;
			if ((selectedIndex > -1 && selectedIndex < selector.Items.Count && selector.Items[selectedIndex] == value) || selector.Items.Contains(value))
			{
				return value;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x17001CE7 RID: 7399
		// (get) Token: 0x06007D41 RID: 32065 RVA: 0x00312AAE File Offset: 0x00311AAE
		// (set) Token: 0x06007D42 RID: 32066 RVA: 0x00312ABB File Offset: 0x00311ABB
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object SelectedValue
		{
			get
			{
				return base.GetValue(Selector.SelectedValueProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedValueProperty, value);
			}
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x00312ACC File Offset: 0x00311ACC
		private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
			{
				Selector selector = (Selector)d;
				ItemsControl.ItemInfo value = Selector.PendingSelectionByValueField.GetValue(selector);
				if (value != null)
				{
					try
					{
						if (!selector.SelectionChange.IsActive)
						{
							selector._cacheValid[16] = true;
							selector.SelectionChange.SelectJustThisItem(value, true);
						}
					}
					finally
					{
						selector._cacheValid[16] = false;
						Selector.PendingSelectionByValueField.ClearValue(selector);
					}
				}
			}
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x00312B50 File Offset: 0x00311B50
		private object SelectItemWithValue(object value, bool selectNow)
		{
			object obj;
			if (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
			{
				this._cacheValid[16] = true;
				if (base.HasItems)
				{
					int index;
					obj = this.FindItemWithValue(value, out index);
					this.SelectionChange.SelectJustThisItem(base.NewItemInfo(obj, null, index), true);
				}
				else
				{
					obj = DependencyProperty.UnsetValue;
					this._cacheValid[32] = true;
				}
				this._cacheValid[16] = false;
			}
			else if (base.HasItems)
			{
				int index2;
				obj = this.FindItemWithValue(value, out index2);
				ItemsControl.ItemInfo itemInfo = base.NewItemInfo(obj, null, index2);
				if (selectNow)
				{
					try
					{
						this._cacheValid[16] = true;
						this.SelectionChange.SelectJustThisItem(itemInfo, true);
						return obj;
					}
					finally
					{
						this._cacheValid[16] = false;
					}
				}
				Selector.PendingSelectionByValueField.SetValue(this, itemInfo);
			}
			else
			{
				obj = DependencyProperty.UnsetValue;
				this._cacheValid[32] = true;
			}
			return obj;
		}

		// Token: 0x06007D45 RID: 32069 RVA: 0x00312C40 File Offset: 0x00311C40
		private object FindItemWithValue(object value, out int index)
		{
			index = -1;
			if (!base.HasItems)
			{
				return DependencyProperty.UnsetValue;
			}
			BindingExpression bindingExpression = this.PrepareItemValueBinding(base.Items.GetRepresentativeItem());
			if (bindingExpression == null)
			{
				return DependencyProperty.UnsetValue;
			}
			if (!string.IsNullOrEmpty(this.SelectedValuePath))
			{
				Type knownType = (value != null) ? value.GetType() : null;
				DynamicValueConverter converter = new DynamicValueConverter(false);
				index = 0;
				foreach (object obj in ((IEnumerable)base.Items))
				{
					bindingExpression.Activate(obj);
					object value2 = bindingExpression.Value;
					if (this.VerifyEqual(value, knownType, value2, converter))
					{
						bindingExpression.Deactivate();
						return obj;
					}
					index++;
				}
				bindingExpression.Deactivate();
				index = -1;
				return DependencyProperty.UnsetValue;
			}
			if (!string.IsNullOrEmpty(bindingExpression.ParentBinding.Path.Path))
			{
				return SystemXmlHelper.FindXmlNodeWithInnerText(base.Items, value, out index);
			}
			index = base.Items.IndexOf(value);
			if (index >= 0)
			{
				return value;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06007D46 RID: 32070 RVA: 0x00312D64 File Offset: 0x00311D64
		private bool VerifyEqual(object knownValue, Type knownType, object itemValue, DynamicValueConverter converter)
		{
			object obj = knownValue;
			if (knownType != null && itemValue != null)
			{
				Type type = itemValue.GetType();
				if (!knownType.IsAssignableFrom(type))
				{
					obj = converter.Convert(knownValue, type);
					if (obj == DependencyProperty.UnsetValue)
					{
						obj = knownValue;
					}
				}
			}
			return object.Equals(obj, itemValue);
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x00312DAC File Offset: 0x00311DAC
		private static object CoerceSelectedValue(DependencyObject d, object value)
		{
			Selector selector = (Selector)d;
			if (selector.SelectionChange.IsActive)
			{
				selector._cacheValid[16] = false;
			}
			else if (selector.SelectItemWithValue(value, false) == DependencyProperty.UnsetValue && selector.HasItems)
			{
				value = null;
			}
			return value;
		}

		// Token: 0x17001CE8 RID: 7400
		// (get) Token: 0x06007D48 RID: 32072 RVA: 0x00312DF8 File Offset: 0x00311DF8
		// (set) Token: 0x06007D49 RID: 32073 RVA: 0x00312E0A File Offset: 0x00311E0A
		[Category("Appearance")]
		[Bindable(true)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public string SelectedValuePath
		{
			get
			{
				return (string)base.GetValue(Selector.SelectedValuePathProperty);
			}
			set
			{
				base.SetValue(Selector.SelectedValuePathProperty, value);
			}
		}

		// Token: 0x06007D4A RID: 32074 RVA: 0x00312E18 File Offset: 0x00311E18
		private static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Selector selector = (Selector)d;
			Selector.ItemValueBindingExpression.ClearValue(selector);
			if (selector.GetValueEntry(selector.LookupEntry(Selector.SelectedValueProperty.GlobalIndex), Selector.SelectedValueProperty, null, RequestFlags.RawEntry).IsCoerced || selector.SelectedValue != null)
			{
				selector.CoerceValue(Selector.SelectedValueProperty);
			}
		}

		// Token: 0x06007D4B RID: 32075 RVA: 0x00312E74 File Offset: 0x00311E74
		private BindingExpression PrepareItemValueBinding(object item)
		{
			if (item == null)
			{
				return null;
			}
			bool flag = SystemXmlHelper.IsXmlNode(item);
			BindingExpression bindingExpression = Selector.ItemValueBindingExpression.GetValue(this);
			if (bindingExpression != null)
			{
				Binding binding = bindingExpression.ParentBinding;
				bool flag2 = binding.XPath != null;
				if ((!flag2 && flag) || (flag2 && !flag))
				{
					Selector.ItemValueBindingExpression.ClearValue(this);
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
				Selector.ItemValueBindingExpression.SetValue(this, bindingExpression);
			}
			return bindingExpression;
		}

		// Token: 0x17001CE9 RID: 7401
		// (get) Token: 0x06007D4C RID: 32076 RVA: 0x00312F2E File Offset: 0x00311F2E
		internal IList SelectedItemsImpl
		{
			get
			{
				return (IList)base.GetValue(Selector.SelectedItemsImplProperty);
			}
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x00312F40 File Offset: 0x00311F40
		internal bool SetSelectedItemsImpl(IEnumerable selectedItems)
		{
			bool flag = false;
			if (!this.SelectionChange.IsActive)
			{
				this.SelectionChange.Begin();
				this.SelectionChange.CleanupDeferSelection();
				ObservableCollection<object> observableCollection = (ObservableCollection<object>)base.GetValue(Selector.SelectedItemsImplProperty);
				try
				{
					if (observableCollection != null)
					{
						foreach (object item in observableCollection)
						{
							this.SelectionChange.Unselect(base.NewUnresolvedItemInfo(item));
						}
					}
					if (selectedItems != null)
					{
						foreach (object item2 in selectedItems)
						{
							if (!this.SelectionChange.Select(base.NewUnresolvedItemInfo(item2), false))
							{
								this.SelectionChange.Cancel();
								return false;
							}
						}
					}
					this.SelectionChange.End();
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.SelectionChange.Cancel();
					}
				}
				return flag;
			}
			return flag;
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x00313068 File Offset: 0x00312068
		private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.SelectionChange.IsActive)
			{
				return;
			}
			if (!this.CanSelectMultiple)
			{
				throw new InvalidOperationException(SR.Get("ChangingCollectionNotSupported"));
			}
			this.SelectionChange.Begin();
			bool flag = false;
			try
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems.Count != 1)
					{
						throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
					}
					this.SelectionChange.Select(base.NewUnresolvedItemInfo(e.NewItems[0]), false);
					break;
				case NotifyCollectionChangedAction.Remove:
					if (e.OldItems.Count != 1)
					{
						throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
					}
					this.SelectionChange.Unselect(base.NewUnresolvedItemInfo(e.OldItems[0]));
					break;
				case NotifyCollectionChangedAction.Replace:
					if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
					{
						throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
					}
					this.SelectionChange.Unselect(base.NewUnresolvedItemInfo(e.OldItems[0]));
					this.SelectionChange.Select(base.NewUnresolvedItemInfo(e.NewItems[0]), false);
					break;
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Reset:
				{
					this.SelectionChange.CleanupDeferSelection();
					for (int i = 0; i < this._selectedItems.Count; i++)
					{
						this.SelectionChange.Unselect(this._selectedItems[i]);
					}
					ObservableCollection<object> observableCollection = (ObservableCollection<object>)sender;
					for (int j = 0; j < observableCollection.Count; j++)
					{
						this.SelectionChange.Select(base.NewUnresolvedItemInfo(observableCollection[j]), false);
					}
					break;
				}
				default:
					throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
					{
						e.Action
					}));
				}
				this.SelectionChange.End();
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					this.SelectionChange.Cancel();
				}
			}
		}

		// Token: 0x17001CEA RID: 7402
		// (get) Token: 0x06007D4F RID: 32079 RVA: 0x00313290 File Offset: 0x00312290
		// (set) Token: 0x06007D50 RID: 32080 RVA: 0x0031329E File Offset: 0x0031229E
		internal bool CanSelectMultiple
		{
			get
			{
				return this._cacheValid[2];
			}
			set
			{
				if (this._cacheValid[2] != value)
				{
					this._cacheValid[2] = value;
					if (!value && this._selectedItems.Count > 1)
					{
						this.SelectionChange.Validate();
					}
				}
			}
		}

		// Token: 0x06007D51 RID: 32081 RVA: 0x003132D8 File Offset: 0x003122D8
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			if (!((IGeneratorHost)this).IsItemItsOwnContainer(item))
			{
				try
				{
					this._clearingContainer = element;
					element.ClearValue(Selector.IsSelectedProperty);
				}
				finally
				{
					this._clearingContainer = null;
				}
			}
		}

		// Token: 0x06007D52 RID: 32082 RVA: 0x00313324 File Offset: 0x00312324
		internal void RaiseIsSelectedChangedAutomationEvent(DependencyObject container, bool isSelected)
		{
			SelectorAutomationPeer selectorAutomationPeer = UIElementAutomationPeer.FromElement(this) as SelectorAutomationPeer;
			if (selectorAutomationPeer != null && selectorAutomationPeer.ItemPeers != null)
			{
				object itemOrContainerFromContainer = base.GetItemOrContainerFromContainer(container);
				if (itemOrContainerFromContainer != null)
				{
					SelectorItemAutomationPeer selectorItemAutomationPeer = selectorAutomationPeer.ItemPeers[itemOrContainerFromContainer] as SelectorItemAutomationPeer;
					if (selectorItemAutomationPeer != null)
					{
						selectorItemAutomationPeer.RaiseAutomationIsSelectedChanged(isSelected);
					}
				}
			}
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x0031336F File Offset: 0x0031236F
		internal void SetInitialMousePosition()
		{
			this._lastMousePosition = Mouse.GetPosition(this);
		}

		// Token: 0x06007D54 RID: 32084 RVA: 0x00313380 File Offset: 0x00312380
		internal bool DidMouseMove()
		{
			Point position = Mouse.GetPosition(this);
			if (position != this._lastMousePosition)
			{
				this._lastMousePosition = position;
				return true;
			}
			return false;
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x003133AC File Offset: 0x003123AC
		internal void ResetLastMousePosition()
		{
			this._lastMousePosition = default(Point);
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x003133BC File Offset: 0x003123BC
		internal virtual void SelectAllImpl()
		{
			this.SelectionChange.Begin();
			this.SelectionChange.CleanupDeferSelection();
			try
			{
				int num = 0;
				foreach (object item in ((IEnumerable)base.Items))
				{
					ItemsControl.ItemInfo info = base.NewItemInfo(item, null, num++);
					this.SelectionChange.Select(info, true);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		// Token: 0x06007D57 RID: 32087 RVA: 0x00313458 File Offset: 0x00312458
		internal virtual void UnselectAllImpl()
		{
			this.SelectionChange.Begin();
			this.SelectionChange.CleanupDeferSelection();
			try
			{
				object internalSelectedItem = this.InternalSelectedItem;
				foreach (ItemsControl.ItemInfo info in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
				{
					this.SelectionChange.Unselect(info);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		// Token: 0x06007D58 RID: 32088 RVA: 0x003134E0 File Offset: 0x003124E0
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (base.DataContext == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			if (e.Action == NotifyCollectionChangedAction.Reset || (e.Action == NotifyCollectionChangedAction.Add && e.NewStartingIndex == 0))
			{
				this.ResetSelectedItemsAlgorithm();
			}
			EffectiveValueEntry valueEntry = base.GetValueEntry(base.LookupEntry(Selector.SelectedIndexProperty.GlobalIndex), Selector.SelectedIndexProperty, null, RequestFlags.DeferredReferences);
			if (!valueEntry.IsDeferredReference || !(valueEntry.Value is DeferredSelectedIndexReference))
			{
				base.CoerceValue(Selector.SelectedIndexProperty);
			}
			base.CoerceValue(Selector.SelectedItemProperty);
			if (this._cacheValid[32] && !object.Equals(this.SelectedValue, this.InternalSelectedValue))
			{
				this.SelectItemWithValue(this.SelectedValue, true);
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.SelectionChange.Begin();
				try
				{
					ItemsControl.ItemInfo info = base.NewItemInfo(e.NewItems[0], null, e.NewStartingIndex);
					if (this.InfoGetIsSelected(info))
					{
						this.SelectionChange.Select(info, true);
					}
					return;
				}
				finally
				{
					this.SelectionChange.End();
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveFromSelection(e);
				return;
			case NotifyCollectionChangedAction.Replace:
				break;
			case NotifyCollectionChangedAction.Move:
				this.AdjustNewContainers();
				this.SelectionChange.Validate();
				return;
			case NotifyCollectionChangedAction.Reset:
				if (base.Items.IsEmpty)
				{
					this.SelectionChange.CleanupDeferSelection();
				}
				if (base.Items.CurrentItem != null && this.IsSynchronizedWithCurrentItemPrivate)
				{
					this.SetSelectedToCurrent();
					return;
				}
				this.SelectionChange.Begin();
				try
				{
					this.LocateSelectedItems(null, true);
					if (base.ItemsSource == null)
					{
						for (int i = 0; i < base.Items.Count; i++)
						{
							ItemsControl.ItemInfo itemInfo = base.ItemInfoFromIndex(i);
							if (this.InfoGetIsSelected(itemInfo) && !this._selectedItems.Contains(itemInfo))
							{
								this.SelectionChange.Select(itemInfo, true);
							}
						}
					}
					return;
				}
				finally
				{
					this.SelectionChange.End();
				}
				goto IL_1FB;
			default:
				goto IL_1FB;
			}
			this.ItemSetIsSelected(base.ItemInfoFromIndex(e.NewStartingIndex), false);
			this.RemoveFromSelection(e);
			return;
			IL_1FB:
			throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
			{
				e.Action
			}));
		}

		// Token: 0x06007D59 RID: 32089 RVA: 0x00313728 File Offset: 0x00312728
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			base.AdjustItemInfos(e, this._selectedItems);
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x06007D5A RID: 32090 RVA: 0x00313740 File Offset: 0x00312740
		private void RemoveFromSelection(NotifyCollectionChangedEventArgs e)
		{
			this.SelectionChange.Begin();
			try
			{
				ItemsControl.ItemInfo itemInfo = base.NewItemInfo(e.OldItems[0], ItemsControl.ItemInfo.SentinelContainer, e.OldStartingIndex);
				itemInfo.Container = null;
				if (this._selectedItems.Contains(itemInfo))
				{
					this.SelectionChange.Unselect(itemInfo);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		// Token: 0x06007D5B RID: 32091 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06007D5C RID: 32092 RVA: 0x003137B8 File Offset: 0x003127B8
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			bool flag = false;
			if ((bool)e.NewValue)
			{
				flag = true;
			}
			else
			{
				DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
				if (dependencyObject != null)
				{
					UIElement uielement = KeyboardNavigation.GetVisualRoot(this) as UIElement;
					if (uielement != null && uielement.IsKeyboardFocusWithin && FocusManager.GetFocusScope(dependencyObject) != FocusManager.GetFocusScope(this))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.TrueBox);
				return;
			}
			base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.FalseBox);
		}

		// Token: 0x06007D5D RID: 32093 RVA: 0x002EEF88 File Offset: 0x002EDF88
		private void OnFocusEnterMainFocusScope(object sender, EventArgs e)
		{
			if (!base.IsKeyboardFocusWithin)
			{
				base.ClearValue(Selector.IsSelectionActivePropertyKey);
			}
		}

		// Token: 0x06007D5E RID: 32094 RVA: 0x00313839 File Offset: 0x00312839
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			this.SetSynchronizationWithCurrentItem();
		}

		// Token: 0x06007D5F RID: 32095 RVA: 0x00313841 File Offset: 0x00312841
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (item == this.SelectedItem)
			{
				KeyboardNavigation.Current.UpdateActiveElement(this, element);
			}
			this.OnNewContainer();
		}

		// Token: 0x06007D60 RID: 32096 RVA: 0x00313866 File Offset: 0x00312866
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.SetSynchronizationWithCurrentItem();
		}

		// Token: 0x17001CEB RID: 7403
		// (get) Token: 0x06007D61 RID: 32097 RVA: 0x00313875 File Offset: 0x00312875
		// (set) Token: 0x06007D62 RID: 32098 RVA: 0x00313883 File Offset: 0x00312883
		private bool IsSynchronizedWithCurrentItemPrivate
		{
			get
			{
				return this._cacheValid[4];
			}
			set
			{
				this._cacheValid[4] = value;
			}
		}

		// Token: 0x17001CEC RID: 7404
		// (get) Token: 0x06007D63 RID: 32099 RVA: 0x00313892 File Offset: 0x00312892
		// (set) Token: 0x06007D64 RID: 32100 RVA: 0x003138A0 File Offset: 0x003128A0
		private bool SkipCoerceSelectedItemCheck
		{
			get
			{
				return this._cacheValid[8];
			}
			set
			{
				this._cacheValid[8] = value;
			}
		}

		// Token: 0x06007D65 RID: 32101 RVA: 0x003138B0 File Offset: 0x003128B0
		private void SetSelectedHelper(object item, FrameworkElement UI, bool selected)
		{
			if (!Selector.ItemGetIsSelectable(item) && selected)
			{
				throw new InvalidOperationException(SR.Get("CannotSelectNotSelectableItem"));
			}
			this.SelectionChange.Begin();
			try
			{
				ItemsControl.ItemInfo info = base.NewItemInfo(item, UI, -1);
				if (selected)
				{
					this.SelectionChange.Select(info, true);
				}
				else
				{
					this.SelectionChange.Unselect(info);
				}
			}
			finally
			{
				this.SelectionChange.End();
			}
		}

		// Token: 0x06007D66 RID: 32102 RVA: 0x00313930 File Offset: 0x00312930
		private void OnCurrentChanged(object sender, EventArgs e)
		{
			if (this.IsSynchronizedWithCurrentItemPrivate)
			{
				this.SetSelectedToCurrent();
			}
		}

		// Token: 0x06007D67 RID: 32103 RVA: 0x00313940 File Offset: 0x00312940
		private void OnNewContainer()
		{
			if (this._cacheValid[64])
			{
				return;
			}
			this._cacheValid[64] = true;
			base.LayoutUpdated += this.OnLayoutUpdated;
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x00313972 File Offset: 0x00312972
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.AdjustNewContainers();
		}

		// Token: 0x06007D69 RID: 32105 RVA: 0x0031397A File Offset: 0x0031297A
		private void OnGeneratorStatusChanged(object sender, EventArgs e)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				this.AdjustNewContainers();
			}
		}

		// Token: 0x06007D6A RID: 32106 RVA: 0x00313990 File Offset: 0x00312990
		private void AdjustNewContainers()
		{
			if (this._cacheValid[64])
			{
				base.LayoutUpdated -= this.OnLayoutUpdated;
				this._cacheValid[64] = false;
			}
			this.AdjustItemInfosAfterGeneratorChangeOverride();
			if (base.HasItems)
			{
				this.SelectionChange.Begin();
				try
				{
					for (int i = 0; i < this._selectedItems.Count; i++)
					{
						this.ItemSetIsSelected(this._selectedItems[i], true);
					}
				}
				finally
				{
					this.SelectionChange.Cancel();
				}
			}
		}

		// Token: 0x06007D6B RID: 32107 RVA: 0x00313A2C File Offset: 0x00312A2C
		internal virtual void AdjustItemInfosAfterGeneratorChangeOverride()
		{
			base.AdjustItemInfosAfterGeneratorChange(this._selectedItems, true);
		}

		// Token: 0x06007D6C RID: 32108 RVA: 0x00313A3C File Offset: 0x00312A3C
		private void SetSelectedToCurrent()
		{
			if (!this._cacheValid[1])
			{
				this._cacheValid[1] = true;
				try
				{
					object currentItem = base.Items.CurrentItem;
					if (currentItem != null && Selector.ItemGetIsSelectable(currentItem))
					{
						this.SelectionChange.SelectJustThisItem(base.NewItemInfo(currentItem, null, base.Items.CurrentPosition), true);
					}
					else
					{
						this.SelectionChange.SelectJustThisItem(null, false);
					}
				}
				finally
				{
					this._cacheValid[1] = false;
				}
			}
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x00313ACC File Offset: 0x00312ACC
		private void SetCurrentToSelected()
		{
			if (!this._cacheValid[1])
			{
				this._cacheValid[1] = true;
				try
				{
					if (this._selectedItems.Count == 0)
					{
						base.Items.MoveCurrentToPosition(-1);
					}
					else
					{
						int index = this._selectedItems[0].Index;
						if (index >= 0)
						{
							base.Items.MoveCurrentToPosition(index);
						}
						else
						{
							base.Items.MoveCurrentTo(this.InternalSelectedItem);
						}
					}
				}
				finally
				{
					this._cacheValid[1] = false;
				}
			}
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x00313B68 File Offset: 0x00312B68
		private void UpdateSelectedItems()
		{
			SelectedItemCollection selectedItemCollection = (SelectedItemCollection)this.SelectedItemsImpl;
			if (selectedItemCollection != null)
			{
				Selector.InternalSelectedItemsStorage internalSelectedItemsStorage = new Selector.InternalSelectedItemsStorage(0, Selector.MatchExplicitEqualityComparer);
				Selector.InternalSelectedItemsStorage internalSelectedItemsStorage2 = new Selector.InternalSelectedItemsStorage(selectedItemCollection.Count, Selector.MatchExplicitEqualityComparer);
				internalSelectedItemsStorage.UsesItemHashCodes = this._selectedItems.UsesItemHashCodes;
				internalSelectedItemsStorage2.UsesItemHashCodes = this._selectedItems.UsesItemHashCodes;
				for (int i = 0; i < selectedItemCollection.Count; i++)
				{
					internalSelectedItemsStorage2.Add(selectedItemCollection[i], ItemsControl.ItemInfo.SentinelContainer, ~i);
				}
				using (internalSelectedItemsStorage2.DeferRemove())
				{
					ItemsControl.ItemInfo itemInfo = new ItemsControl.ItemInfo(null, null, -1);
					foreach (ItemsControl.ItemInfo itemInfo2 in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
					{
						itemInfo.Reset(itemInfo2.Item);
						if (internalSelectedItemsStorage2.Contains(itemInfo))
						{
							internalSelectedItemsStorage2.Remove(itemInfo);
						}
						else
						{
							internalSelectedItemsStorage.Add(itemInfo2);
						}
					}
				}
				if (internalSelectedItemsStorage.Count > 0 || internalSelectedItemsStorage2.Count > 0)
				{
					if (selectedItemCollection.IsChanging)
					{
						Selector.ChangeInfoField.SetValue(this, new Selector.ChangeInfo(internalSelectedItemsStorage, internalSelectedItemsStorage2));
						return;
					}
					this.UpdateSelectedItems(internalSelectedItemsStorage, internalSelectedItemsStorage2);
				}
			}
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x00313CB4 File Offset: 0x00312CB4
		internal void FinishSelectedItemsChange()
		{
			Selector.ChangeInfo value = Selector.ChangeInfoField.GetValue(this);
			if (value != null)
			{
				bool isActive = this.SelectionChange.IsActive;
				if (!isActive)
				{
					this.SelectionChange.Begin();
				}
				this.UpdateSelectedItems(value.ToAdd, value.ToRemove);
				if (!isActive)
				{
					this.SelectionChange.End();
				}
			}
		}

		// Token: 0x06007D70 RID: 32112 RVA: 0x00313D08 File Offset: 0x00312D08
		private void UpdateSelectedItems(Selector.InternalSelectedItemsStorage toAdd, Selector.InternalSelectedItemsStorage toRemove)
		{
			IList selectedItemsImpl = this.SelectedItemsImpl;
			Selector.ChangeInfoField.ClearValue(this);
			for (int i = 0; i < toAdd.Count; i++)
			{
				selectedItemsImpl.Add(toAdd[i].Item);
			}
			for (int j = toRemove.Count - 1; j >= 0; j--)
			{
				selectedItemsImpl.RemoveAt(~toRemove[j].Index);
			}
		}

		// Token: 0x06007D71 RID: 32113 RVA: 0x00313D74 File Offset: 0x00312D74
		internal void UpdatePublicSelectionProperties()
		{
			EffectiveValueEntry valueEntry = base.GetValueEntry(base.LookupEntry(Selector.SelectedIndexProperty.GlobalIndex), Selector.SelectedIndexProperty, null, RequestFlags.DeferredReferences);
			if (!valueEntry.IsDeferredReference)
			{
				int num = (int)valueEntry.Value;
				if (num > base.Items.Count - 1 || (num == -1 && this._selectedItems.Count > 0) || (num > -1 && (this._selectedItems.Count == 0 || num != this._selectedItems[0].Index)))
				{
					base.SetCurrentDeferredValue(Selector.SelectedIndexProperty, new DeferredSelectedIndexReference(this));
				}
			}
			if (this.SelectedItem != this.InternalSelectedItem)
			{
				try
				{
					this.SkipCoerceSelectedItemCheck = true;
					base.SetCurrentValueInternal(Selector.SelectedItemProperty, this.InternalSelectedItem);
				}
				finally
				{
					this.SkipCoerceSelectedItemCheck = false;
				}
			}
			if (this._selectedItems.Count > 0)
			{
				this._cacheValid[32] = false;
			}
			if (!this._cacheValid[16] && !this._cacheValid[32])
			{
				object obj = this.InternalSelectedValue;
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = null;
				}
				if (!object.Equals(this.SelectedValue, obj))
				{
					base.SetCurrentValueInternal(Selector.SelectedValueProperty, obj);
				}
			}
			this.UpdateSelectedItems();
		}

		// Token: 0x06007D72 RID: 32114 RVA: 0x00313EB8 File Offset: 0x00312EB8
		private void InvokeSelectionChanged(List<ItemsControl.ItemInfo> unselectedInfos, List<ItemsControl.ItemInfo> selectedInfos)
		{
			this.OnSelectionChanged(new SelectionChangedEventArgs(unselectedInfos, selectedInfos)
			{
				Source = this
			});
		}

		// Token: 0x06007D73 RID: 32115 RVA: 0x00313EDC File Offset: 0x00312EDC
		private bool InfoGetIsSelected(ItemsControl.ItemInfo info)
		{
			DependencyObject container = info.Container;
			if (container != null)
			{
				return (bool)container.GetValue(Selector.IsSelectedProperty);
			}
			if (this.IsItemItsOwnContainerOverride(info.Item))
			{
				DependencyObject dependencyObject = info.Item as DependencyObject;
				if (dependencyObject != null)
				{
					return (bool)dependencyObject.GetValue(Selector.IsSelectedProperty);
				}
			}
			return false;
		}

		// Token: 0x06007D74 RID: 32116 RVA: 0x00313F34 File Offset: 0x00312F34
		private void ItemSetIsSelected(ItemsControl.ItemInfo info, bool value)
		{
			if (info == null)
			{
				return;
			}
			DependencyObject container = info.Container;
			if (container != null && container != ItemsControl.ItemInfo.RemovedContainer)
			{
				if (Selector.GetIsSelected(container) != value)
				{
					container.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.Box(value));
					return;
				}
			}
			else
			{
				object item = info.Item;
				if (this.IsItemItsOwnContainerOverride(item))
				{
					DependencyObject dependencyObject = item as DependencyObject;
					if (dependencyObject != null && Selector.GetIsSelected(dependencyObject) != value)
					{
						dependencyObject.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.Box(value));
					}
				}
			}
		}

		// Token: 0x06007D75 RID: 32117 RVA: 0x00313FAC File Offset: 0x00312FAC
		internal static bool ItemGetIsSelectable(object item)
		{
			return item != null && !(item is Separator);
		}

		// Token: 0x06007D76 RID: 32118 RVA: 0x00313FC0 File Offset: 0x00312FC0
		internal static bool UiGetIsSelectable(DependencyObject o)
		{
			if (o != null)
			{
				if (!Selector.ItemGetIsSelectable(o))
				{
					return false;
				}
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(o);
				if (itemsControl != null)
				{
					object obj = itemsControl.ItemContainerGenerator.ItemFromContainer(o);
					return obj == o || Selector.ItemGetIsSelectable(obj);
				}
			}
			return false;
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x00313FFF File Offset: 0x00312FFF
		private static void OnSelected(object sender, RoutedEventArgs e)
		{
			((Selector)sender).NotifyIsSelectedChanged(e.OriginalSource as FrameworkElement, true, e);
		}

		// Token: 0x06007D78 RID: 32120 RVA: 0x00314019 File Offset: 0x00313019
		private static void OnUnselected(object sender, RoutedEventArgs e)
		{
			((Selector)sender).NotifyIsSelectedChanged(e.OriginalSource as FrameworkElement, false, e);
		}

		// Token: 0x06007D79 RID: 32121 RVA: 0x00314034 File Offset: 0x00313034
		internal void NotifyIsSelectedChanged(FrameworkElement container, bool selected, RoutedEventArgs e)
		{
			if (this.SelectionChange.IsActive || container == this._clearingContainer)
			{
				e.Handled = true;
				return;
			}
			if (container != null)
			{
				object itemOrContainerFromContainer = base.GetItemOrContainerFromContainer(container);
				if (itemOrContainerFromContainer != DependencyProperty.UnsetValue)
				{
					this.SetSelectedHelper(itemOrContainerFromContainer, container, selected);
					e.Handled = true;
				}
			}
		}

		// Token: 0x17001CED RID: 7405
		// (get) Token: 0x06007D7A RID: 32122 RVA: 0x00314082 File Offset: 0x00313082
		internal Selector.SelectionChanger SelectionChange
		{
			get
			{
				if (this._selectionChangeInstance == null)
				{
					this._selectionChangeInstance = new Selector.SelectionChanger(this);
				}
				return this._selectionChangeInstance;
			}
		}

		// Token: 0x06007D7B RID: 32123 RVA: 0x0031409E File Offset: 0x0031309E
		private void ResetSelectedItemsAlgorithm()
		{
			if (!base.Items.IsEmpty)
			{
				this._selectedItems.UsesItemHashCodes = base.Items.CollectionView.HasReliableHashCodes();
			}
		}

		// Token: 0x06007D7C RID: 32124 RVA: 0x003140C8 File Offset: 0x003130C8
		internal void LocateSelectedItems(List<Tuple<int, int>> ranges = null, bool deselectMissingItems = false)
		{
			List<int> list = new List<int>(this._selectedItems.Count);
			int num = 0;
			foreach (ItemsControl.ItemInfo itemInfo in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
			{
				if (itemInfo.Index < 0)
				{
					num++;
				}
				else
				{
					list.Add(itemInfo.Index);
				}
			}
			int count = list.Count;
			list.Sort();
			ItemsControl.ItemInfo itemInfo2 = new ItemsControl.ItemInfo(null, ItemsControl.ItemInfo.KeyContainer, -1);
			int num2 = 0;
			while (num > 0 && num2 < base.Items.Count)
			{
				if (list.BinarySearch(0, count, num2, null) < 0)
				{
					itemInfo2.Reset(base.Items[num2]);
					itemInfo2.Index = num2;
					ItemsControl.ItemInfo itemInfo3 = this._selectedItems.FindMatch(itemInfo2);
					if (itemInfo3 != null)
					{
						itemInfo3.Index = num2;
						list.Add(num2);
						num--;
					}
				}
				num2++;
			}
			if (ranges != null)
			{
				ranges.Clear();
				list.Sort();
				list.Add(-1);
				int num3 = -1;
				int num4 = -2;
				foreach (int num5 in list)
				{
					if (num5 == num4 + 1)
					{
						num4 = num5;
					}
					else
					{
						if (num3 >= 0)
						{
							ranges.Add(new Tuple<int, int>(num3, num4 - num3 + 1));
						}
						num4 = (num3 = num5);
					}
				}
			}
			if (deselectMissingItems)
			{
				foreach (ItemsControl.ItemInfo itemInfo4 in ((IEnumerable<ItemsControl.ItemInfo>)this._selectedItems))
				{
					if (itemInfo4.Index < 0)
					{
						itemInfo4.Container = ItemsControl.ItemInfo.RemovedContainer;
						this.SelectionChange.Unselect(itemInfo4);
					}
				}
			}
		}

		// Token: 0x17001CEE RID: 7406
		// (get) Token: 0x06007D7D RID: 32125 RVA: 0x003142B8 File Offset: 0x003132B8
		internal object InternalSelectedItem
		{
			get
			{
				if (this._selectedItems.Count != 0)
				{
					return this._selectedItems[0].Item;
				}
				return null;
			}
		}

		// Token: 0x17001CEF RID: 7407
		// (get) Token: 0x06007D7E RID: 32126 RVA: 0x003142DA File Offset: 0x003132DA
		internal ItemsControl.ItemInfo InternalSelectedInfo
		{
			get
			{
				if (this._selectedItems.Count != 0)
				{
					return this._selectedItems[0];
				}
				return null;
			}
		}

		// Token: 0x17001CF0 RID: 7408
		// (get) Token: 0x06007D7F RID: 32127 RVA: 0x003142F8 File Offset: 0x003132F8
		internal int InternalSelectedIndex
		{
			get
			{
				if (this._selectedItems.Count == 0)
				{
					return -1;
				}
				int num = this._selectedItems[0].Index;
				if (num < 0)
				{
					num = base.Items.IndexOf(this._selectedItems[0].Item);
					this._selectedItems[0].Index = num;
				}
				return num;
			}
		}

		// Token: 0x17001CF1 RID: 7409
		// (get) Token: 0x06007D80 RID: 32128 RVA: 0x0031435C File Offset: 0x0031335C
		private object InternalSelectedValue
		{
			get
			{
				object internalSelectedItem = this.InternalSelectedItem;
				object result;
				if (internalSelectedItem != null)
				{
					BindingExpression bindingExpression = this.PrepareItemValueBinding(internalSelectedItem);
					if (string.IsNullOrEmpty(this.SelectedValuePath))
					{
						if (string.IsNullOrEmpty(bindingExpression.ParentBinding.Path.Path))
						{
							result = internalSelectedItem;
						}
						else
						{
							result = SystemXmlHelper.GetInnerText(internalSelectedItem);
						}
					}
					else
					{
						bindingExpression.Activate(internalSelectedItem);
						result = bindingExpression.Value;
						bindingExpression.Deactivate();
					}
				}
				else
				{
					result = DependencyProperty.UnsetValue;
				}
				return result;
			}
		}

		// Token: 0x04003ADA RID: 15066
		public static readonly RoutedEvent SelectedEvent;

		// Token: 0x04003ADB RID: 15067
		public static readonly RoutedEvent UnselectedEvent;

		// Token: 0x04003ADC RID: 15068
		internal static readonly DependencyPropertyKey IsSelectionActivePropertyKey;

		// Token: 0x04003ADD RID: 15069
		public static readonly DependencyProperty IsSelectionActiveProperty;

		// Token: 0x04003ADE RID: 15070
		public static readonly DependencyProperty IsSelectedProperty;

		// Token: 0x04003ADF RID: 15071
		public static readonly DependencyProperty IsSynchronizedWithCurrentItemProperty;

		// Token: 0x04003AE0 RID: 15072
		public static readonly DependencyProperty SelectedIndexProperty;

		// Token: 0x04003AE1 RID: 15073
		public static readonly DependencyProperty SelectedItemProperty;

		// Token: 0x04003AE2 RID: 15074
		public static readonly DependencyProperty SelectedValueProperty;

		// Token: 0x04003AE3 RID: 15075
		public static readonly DependencyProperty SelectedValuePathProperty;

		// Token: 0x04003AE4 RID: 15076
		private static readonly DependencyPropertyKey SelectedItemsPropertyKey;

		// Token: 0x04003AE5 RID: 15077
		internal static readonly DependencyProperty SelectedItemsImplProperty;

		// Token: 0x04003AE6 RID: 15078
		private static readonly BindingExpressionUncommonField ItemValueBindingExpression;

		// Token: 0x04003AE7 RID: 15079
		internal Selector.InternalSelectedItemsStorage _selectedItems = new Selector.InternalSelectedItemsStorage(1, Selector.MatchExplicitEqualityComparer);

		// Token: 0x04003AE8 RID: 15080
		private Point _lastMousePosition;

		// Token: 0x04003AE9 RID: 15081
		private Selector.SelectionChanger _selectionChangeInstance;

		// Token: 0x04003AEA RID: 15082
		private BitVector32 _cacheValid = new BitVector32(2);

		// Token: 0x04003AEB RID: 15083
		private EventHandler _focusEnterMainFocusScopeEventHandler;

		// Token: 0x04003AEC RID: 15084
		private DependencyObject _clearingContainer;

		// Token: 0x04003AED RID: 15085
		private static readonly UncommonField<ItemsControl.ItemInfo> PendingSelectionByValueField;

		// Token: 0x04003AEE RID: 15086
		private static readonly Selector.ItemInfoEqualityComparer MatchExplicitEqualityComparer;

		// Token: 0x04003AEF RID: 15087
		private static readonly Selector.ItemInfoEqualityComparer MatchUnresolvedEqualityComparer;

		// Token: 0x04003AF0 RID: 15088
		private static readonly UncommonField<Selector.ChangeInfo> ChangeInfoField;

		// Token: 0x02000C53 RID: 3155
		[Flags]
		private enum CacheBits
		{
			// Token: 0x04004C55 RID: 19541
			SyncingSelectionAndCurrency = 1,
			// Token: 0x04004C56 RID: 19542
			CanSelectMultiple = 2,
			// Token: 0x04004C57 RID: 19543
			IsSynchronizedWithCurrentItem = 4,
			// Token: 0x04004C58 RID: 19544
			SkipCoerceSelectedItemCheck = 8,
			// Token: 0x04004C59 RID: 19545
			SelectedValueDrivesSelection = 16,
			// Token: 0x04004C5A RID: 19546
			SelectedValueWaitsForItems = 32,
			// Token: 0x04004C5B RID: 19547
			NewContainersArePending = 64
		}

		// Token: 0x02000C54 RID: 3156
		internal class SelectionChanger
		{
			// Token: 0x060091A8 RID: 37288 RVA: 0x0034A0B4 File Offset: 0x003490B4
			internal SelectionChanger(Selector s)
			{
				this._owner = s;
				this._active = false;
				this._toSelect = new Selector.InternalSelectedItemsStorage(1, Selector.MatchUnresolvedEqualityComparer);
				this._toUnselect = new Selector.InternalSelectedItemsStorage(1, Selector.MatchUnresolvedEqualityComparer);
				this._toDeferSelect = new Selector.InternalSelectedItemsStorage(1, Selector.MatchUnresolvedEqualityComparer);
			}

			// Token: 0x17001FE2 RID: 8162
			// (get) Token: 0x060091A9 RID: 37289 RVA: 0x0034A108 File Offset: 0x00349108
			internal bool IsActive
			{
				get
				{
					return this._active;
				}
			}

			// Token: 0x060091AA RID: 37290 RVA: 0x0034A110 File Offset: 0x00349110
			internal void Begin()
			{
				this._active = true;
				this._toSelect.Clear();
				this._toUnselect.Clear();
			}

			// Token: 0x060091AB RID: 37291 RVA: 0x0034A130 File Offset: 0x00349130
			internal void End()
			{
				List<ItemsControl.ItemInfo> list = new List<ItemsControl.ItemInfo>();
				List<ItemsControl.ItemInfo> list2 = new List<ItemsControl.ItemInfo>();
				try
				{
					this.ApplyCanSelectMultiple();
					this.CreateDeltaSelectionChange(list, list2);
					this._owner.UpdatePublicSelectionProperties();
				}
				finally
				{
					this.Cleanup();
				}
				if (list.Count > 0 || list2.Count > 0)
				{
					if (this._owner.IsSynchronizedWithCurrentItemPrivate)
					{
						this._owner.SetCurrentToSelected();
					}
					this._owner.InvokeSelectionChanged(list, list2);
				}
			}

			// Token: 0x060091AC RID: 37292 RVA: 0x0034A1B4 File Offset: 0x003491B4
			private void ApplyCanSelectMultiple()
			{
				if (!this._owner.CanSelectMultiple)
				{
					if (this._toSelect.Count == 1)
					{
						this._toUnselect = new Selector.InternalSelectedItemsStorage(this._owner._selectedItems, null);
						return;
					}
					if (this._owner._selectedItems.Count > 1 && this._owner._selectedItems.Count != this._toUnselect.Count + 1)
					{
						ItemsControl.ItemInfo info = this._owner._selectedItems[0];
						this._toUnselect.Clear();
						foreach (ItemsControl.ItemInfo itemInfo in ((IEnumerable<ItemsControl.ItemInfo>)this._owner._selectedItems))
						{
							if (itemInfo != info)
							{
								this._toUnselect.Add(itemInfo);
							}
						}
					}
				}
			}

			// Token: 0x060091AD RID: 37293 RVA: 0x0034A29C File Offset: 0x0034929C
			private void CreateDeltaSelectionChange(List<ItemsControl.ItemInfo> unselectedItems, List<ItemsControl.ItemInfo> selectedItems)
			{
				for (int i = 0; i < this._toDeferSelect.Count; i++)
				{
					ItemsControl.ItemInfo itemInfo = this._toDeferSelect[i];
					if (this._owner.Items.Contains(itemInfo.Item))
					{
						this._toSelect.Add(itemInfo);
						this._toDeferSelect.Remove(itemInfo);
						i--;
					}
				}
				if (this._toUnselect.Count > 0 || this._toSelect.Count > 0)
				{
					using (this._owner._selectedItems.DeferRemove())
					{
						if (this._toUnselect.ResolvedCount > 0)
						{
							foreach (ItemsControl.ItemInfo itemInfo2 in ((IEnumerable<ItemsControl.ItemInfo>)this._toUnselect))
							{
								if (itemInfo2.IsResolved)
								{
									this._owner.ItemSetIsSelected(itemInfo2, false);
									if (this._owner._selectedItems.Remove(itemInfo2))
									{
										unselectedItems.Add(itemInfo2);
									}
								}
							}
						}
						if (this._toUnselect.UnresolvedCount > 0)
						{
							foreach (ItemsControl.ItemInfo itemInfo3 in ((IEnumerable<ItemsControl.ItemInfo>)this._toUnselect))
							{
								if (!itemInfo3.IsResolved)
								{
									ItemsControl.ItemInfo itemInfo4 = this._owner._selectedItems.FindMatch(ItemsControl.ItemInfo.Key(itemInfo3));
									if (itemInfo4 != null)
									{
										this._owner.ItemSetIsSelected(itemInfo4, false);
										this._owner._selectedItems.Remove(itemInfo4);
										unselectedItems.Add(itemInfo4);
									}
								}
							}
						}
					}
					using (this._toSelect.DeferRemove())
					{
						if (this._toSelect.ResolvedCount > 0)
						{
							List<ItemsControl.ItemInfo> list = (this._toSelect.UnresolvedCount > 0) ? new List<ItemsControl.ItemInfo>() : null;
							foreach (ItemsControl.ItemInfo itemInfo5 in ((IEnumerable<ItemsControl.ItemInfo>)this._toSelect))
							{
								if (itemInfo5.IsResolved)
								{
									this._owner.ItemSetIsSelected(itemInfo5, true);
									if (!this._owner._selectedItems.Contains(itemInfo5))
									{
										this._owner._selectedItems.Add(itemInfo5);
										selectedItems.Add(itemInfo5);
									}
									if (list != null)
									{
										list.Add(itemInfo5);
									}
								}
							}
							if (list != null)
							{
								foreach (ItemsControl.ItemInfo e in list)
								{
									this._toSelect.Remove(e);
								}
							}
						}
						int num = 0;
						while (this._toSelect.UnresolvedCount > 0 && num < this._owner.Items.Count)
						{
							ItemsControl.ItemInfo itemInfo6 = this._owner.NewItemInfo(this._owner.Items[num], null, num);
							ItemsControl.ItemInfo e2 = new ItemsControl.ItemInfo(itemInfo6.Item, ItemsControl.ItemInfo.KeyContainer, -1);
							if (this._toSelect.Contains(e2) && !this._owner._selectedItems.Contains(itemInfo6))
							{
								this._owner.ItemSetIsSelected(itemInfo6, true);
								this._owner._selectedItems.Add(itemInfo6);
								selectedItems.Add(itemInfo6);
								this._toSelect.Remove(e2);
							}
							num++;
						}
					}
				}
			}

			// Token: 0x060091AE RID: 37294 RVA: 0x0034A688 File Offset: 0x00349688
			internal bool Select(ItemsControl.ItemInfo info, bool assumeInItemsCollection)
			{
				if (!Selector.ItemGetIsSelectable(info))
				{
					return false;
				}
				if (!assumeInItemsCollection && !this._owner.Items.Contains(info.Item))
				{
					if (!this._toDeferSelect.Contains(info))
					{
						this._toDeferSelect.Add(info);
					}
					return false;
				}
				ItemsControl.ItemInfo itemInfo = ItemsControl.ItemInfo.Key(info);
				if (this._toUnselect.Remove(itemInfo))
				{
					return true;
				}
				if (this._owner._selectedItems.Contains(info))
				{
					return false;
				}
				if (!itemInfo.IsKey && this._toSelect.Contains(itemInfo))
				{
					return false;
				}
				if (!this._owner.CanSelectMultiple && this._toSelect.Count > 0)
				{
					foreach (ItemsControl.ItemInfo info2 in ((IEnumerable<ItemsControl.ItemInfo>)this._toSelect))
					{
						this._owner.ItemSetIsSelected(info2, false);
					}
					this._toSelect.Clear();
				}
				this._toSelect.Add(info);
				return true;
			}

			// Token: 0x060091AF RID: 37295 RVA: 0x0034A794 File Offset: 0x00349794
			internal bool Unselect(ItemsControl.ItemInfo info)
			{
				ItemsControl.ItemInfo e = ItemsControl.ItemInfo.Key(info);
				this._toDeferSelect.Remove(info);
				if (this._toSelect.Remove(e))
				{
					return true;
				}
				if (!this._owner._selectedItems.Contains(e))
				{
					return false;
				}
				if (this._toUnselect.Contains(info))
				{
					return false;
				}
				this._toUnselect.Add(info);
				return true;
			}

			// Token: 0x060091B0 RID: 37296 RVA: 0x0034A7F7 File Offset: 0x003497F7
			internal void Validate()
			{
				this.Begin();
				this.End();
			}

			// Token: 0x060091B1 RID: 37297 RVA: 0x0034A805 File Offset: 0x00349805
			internal void Cancel()
			{
				this.Cleanup();
			}

			// Token: 0x060091B2 RID: 37298 RVA: 0x0034A80D File Offset: 0x0034980D
			internal void CleanupDeferSelection()
			{
				if (this._toDeferSelect.Count > 0)
				{
					this._toDeferSelect.Clear();
				}
			}

			// Token: 0x060091B3 RID: 37299 RVA: 0x0034A828 File Offset: 0x00349828
			internal void Cleanup()
			{
				this._active = false;
				if (this._toSelect.Count > 0)
				{
					this._toSelect.Clear();
				}
				if (this._toUnselect.Count > 0)
				{
					this._toUnselect.Clear();
				}
			}

			// Token: 0x060091B4 RID: 37300 RVA: 0x0034A864 File Offset: 0x00349864
			internal void SelectJustThisItem(ItemsControl.ItemInfo info, bool assumeInItemsCollection)
			{
				this.Begin();
				this.CleanupDeferSelection();
				try
				{
					bool flag = false;
					for (int i = this._owner._selectedItems.Count - 1; i >= 0; i--)
					{
						if (info != this._owner._selectedItems[i])
						{
							this.Unselect(this._owner._selectedItems[i]);
						}
						else
						{
							flag = true;
						}
					}
					if (!flag && info != null && info.Item != DependencyProperty.UnsetValue)
					{
						this.Select(info, assumeInItemsCollection);
					}
				}
				finally
				{
					this.End();
				}
			}

			// Token: 0x04004C5C RID: 19548
			private Selector _owner;

			// Token: 0x04004C5D RID: 19549
			private Selector.InternalSelectedItemsStorage _toSelect;

			// Token: 0x04004C5E RID: 19550
			private Selector.InternalSelectedItemsStorage _toUnselect;

			// Token: 0x04004C5F RID: 19551
			private Selector.InternalSelectedItemsStorage _toDeferSelect;

			// Token: 0x04004C60 RID: 19552
			private bool _active;
		}

		// Token: 0x02000C55 RID: 3157
		internal class InternalSelectedItemsStorage : IEnumerable<ItemsControl.ItemInfo>, IEnumerable
		{
			// Token: 0x060091B5 RID: 37301 RVA: 0x0034A90C File Offset: 0x0034990C
			internal InternalSelectedItemsStorage(int capacity, IEqualityComparer<ItemsControl.ItemInfo> equalityComparer)
			{
				this._equalityComparer = equalityComparer;
				this._list = new List<ItemsControl.ItemInfo>(capacity);
				this._set = new Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo>(capacity, equalityComparer);
			}

			// Token: 0x060091B6 RID: 37302 RVA: 0x0034A934 File Offset: 0x00349934
			internal InternalSelectedItemsStorage(Selector.InternalSelectedItemsStorage collection, IEqualityComparer<ItemsControl.ItemInfo> equalityComparer = null)
			{
				this._equalityComparer = (equalityComparer ?? collection._equalityComparer);
				this._list = new List<ItemsControl.ItemInfo>(collection._list);
				if (collection.UsesItemHashCodes)
				{
					this._set = new Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo>(collection._set, this._equalityComparer);
				}
				this._resolvedCount = collection._resolvedCount;
				this._unresolvedCount = collection._unresolvedCount;
			}

			// Token: 0x060091B7 RID: 37303 RVA: 0x0034A9A0 File Offset: 0x003499A0
			public void Add(object item, DependencyObject container, int index)
			{
				this.Add(new ItemsControl.ItemInfo(item, container, index));
			}

			// Token: 0x060091B8 RID: 37304 RVA: 0x0034A9B0 File Offset: 0x003499B0
			public void Add(ItemsControl.ItemInfo info)
			{
				if (this._set != null)
				{
					this._set.Add(info, info);
				}
				this._list.Add(info);
				if (info.IsResolved)
				{
					this._resolvedCount++;
					return;
				}
				this._unresolvedCount++;
			}

			// Token: 0x060091B9 RID: 37305 RVA: 0x0034AA04 File Offset: 0x00349A04
			public bool Remove(ItemsControl.ItemInfo e)
			{
				bool flag = false;
				bool flag2 = false;
				if (this._set != null)
				{
					ItemsControl.ItemInfo itemInfo;
					if (this._set.TryGetValue(e, out itemInfo))
					{
						flag = true;
						flag2 = itemInfo.IsResolved;
						this._set.Remove(e);
						if (this.RemoveIsDeferred)
						{
							itemInfo.Container = ItemsControl.ItemInfo.RemovedContainer;
							Selector.InternalSelectedItemsStorage.BatchRemoveHelper batchRemove = this._batchRemove;
							int removedCount = batchRemove.RemovedCount + 1;
							batchRemove.RemovedCount = removedCount;
						}
						else
						{
							this.RemoveFromList(e);
						}
					}
				}
				else
				{
					flag = this.RemoveFromList(e);
				}
				if (flag)
				{
					if (flag2)
					{
						this._resolvedCount--;
					}
					else
					{
						this._unresolvedCount--;
					}
				}
				return flag;
			}

			// Token: 0x060091BA RID: 37306 RVA: 0x0034AAA4 File Offset: 0x00349AA4
			private bool RemoveFromList(ItemsControl.ItemInfo e)
			{
				bool result = false;
				int num = this.LastIndexInList(e);
				if (num >= 0)
				{
					this._list.RemoveAt(num);
					result = true;
				}
				return result;
			}

			// Token: 0x060091BB RID: 37307 RVA: 0x0034AACE File Offset: 0x00349ACE
			public bool Contains(ItemsControl.ItemInfo e)
			{
				if (this._set != null)
				{
					return this._set.ContainsKey(e);
				}
				return this.IndexInList(e) >= 0;
			}

			// Token: 0x17001FE3 RID: 8163
			public ItemsControl.ItemInfo this[int index]
			{
				get
				{
					return this._list[index];
				}
			}

			// Token: 0x060091BD RID: 37309 RVA: 0x0034AB00 File Offset: 0x00349B00
			public void Clear()
			{
				this._list.Clear();
				if (this._set != null)
				{
					this._set.Clear();
				}
				this._resolvedCount = (this._unresolvedCount = 0);
			}

			// Token: 0x17001FE4 RID: 8164
			// (get) Token: 0x060091BE RID: 37310 RVA: 0x0034AB3B File Offset: 0x00349B3B
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x17001FE5 RID: 8165
			// (get) Token: 0x060091BF RID: 37311 RVA: 0x0034AB48 File Offset: 0x00349B48
			public bool RemoveIsDeferred
			{
				get
				{
					return this._batchRemove != null && this._batchRemove.IsActive;
				}
			}

			// Token: 0x060091C0 RID: 37312 RVA: 0x0034AB5F File Offset: 0x00349B5F
			public IDisposable DeferRemove()
			{
				if (this._batchRemove == null)
				{
					this._batchRemove = new Selector.InternalSelectedItemsStorage.BatchRemoveHelper(this);
				}
				this._batchRemove.Enter();
				return this._batchRemove;
			}

			// Token: 0x060091C1 RID: 37313 RVA: 0x0034AB88 File Offset: 0x00349B88
			private void DoBatchRemove()
			{
				int num = 0;
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					if (!this._list[i].IsRemoved)
					{
						if (num < i)
						{
							this._list[num] = this._list[i];
						}
						num++;
					}
				}
				this._list.RemoveRange(num, count - num);
			}

			// Token: 0x17001FE6 RID: 8166
			// (get) Token: 0x060091C2 RID: 37314 RVA: 0x0034ABF1 File Offset: 0x00349BF1
			public int ResolvedCount
			{
				get
				{
					return this._resolvedCount;
				}
			}

			// Token: 0x17001FE7 RID: 8167
			// (get) Token: 0x060091C3 RID: 37315 RVA: 0x0034ABF9 File Offset: 0x00349BF9
			public int UnresolvedCount
			{
				get
				{
					return this._unresolvedCount;
				}
			}

			// Token: 0x060091C4 RID: 37316 RVA: 0x0034AC01 File Offset: 0x00349C01
			IEnumerator<ItemsControl.ItemInfo> IEnumerable<ItemsControl.ItemInfo>.GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x060091C5 RID: 37317 RVA: 0x0034AC01 File Offset: 0x00349C01
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._list.GetEnumerator();
			}

			// Token: 0x17001FE8 RID: 8168
			// (get) Token: 0x060091C6 RID: 37318 RVA: 0x0034AC13 File Offset: 0x00349C13
			// (set) Token: 0x060091C7 RID: 37319 RVA: 0x0034AC20 File Offset: 0x00349C20
			public bool UsesItemHashCodes
			{
				get
				{
					return this._set != null;
				}
				set
				{
					if (value && this._set == null)
					{
						this._set = new Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo>(this._list.Count);
						for (int i = 0; i < this._list.Count; i++)
						{
							this._set.Add(this._list[i], this._list[i]);
						}
						return;
					}
					if (!value)
					{
						this._set = null;
					}
				}
			}

			// Token: 0x060091C8 RID: 37320 RVA: 0x0034AC94 File Offset: 0x00349C94
			public ItemsControl.ItemInfo FindMatch(ItemsControl.ItemInfo info)
			{
				ItemsControl.ItemInfo result;
				if (this._set != null)
				{
					if (!this._set.TryGetValue(info, out result))
					{
						result = null;
					}
				}
				else
				{
					int num = this.IndexInList(info);
					result = ((num < 0) ? null : this._list[num]);
				}
				return result;
			}

			// Token: 0x060091C9 RID: 37321 RVA: 0x0034ACDC File Offset: 0x00349CDC
			private int IndexInList(ItemsControl.ItemInfo info)
			{
				return this._list.FindIndex((ItemsControl.ItemInfo x) => this._equalityComparer.Equals(info, x));
			}

			// Token: 0x060091CA RID: 37322 RVA: 0x0034AD14 File Offset: 0x00349D14
			private int LastIndexInList(ItemsControl.ItemInfo info)
			{
				return this._list.FindLastIndex((ItemsControl.ItemInfo x) => this._equalityComparer.Equals(info, x));
			}

			// Token: 0x04004C61 RID: 19553
			private List<ItemsControl.ItemInfo> _list;

			// Token: 0x04004C62 RID: 19554
			private Dictionary<ItemsControl.ItemInfo, ItemsControl.ItemInfo> _set;

			// Token: 0x04004C63 RID: 19555
			private IEqualityComparer<ItemsControl.ItemInfo> _equalityComparer;

			// Token: 0x04004C64 RID: 19556
			private int _resolvedCount;

			// Token: 0x04004C65 RID: 19557
			private int _unresolvedCount;

			// Token: 0x04004C66 RID: 19558
			private Selector.InternalSelectedItemsStorage.BatchRemoveHelper _batchRemove;

			// Token: 0x02000C95 RID: 3221
			private class BatchRemoveHelper : IDisposable
			{
				// Token: 0x06009587 RID: 38279 RVA: 0x0034E163 File Offset: 0x0034D163
				public BatchRemoveHelper(Selector.InternalSelectedItemsStorage owner)
				{
					this._owner = owner;
				}

				// Token: 0x17002012 RID: 8210
				// (get) Token: 0x06009588 RID: 38280 RVA: 0x0034E172 File Offset: 0x0034D172
				public bool IsActive
				{
					get
					{
						return this._level > 0;
					}
				}

				// Token: 0x17002013 RID: 8211
				// (get) Token: 0x06009589 RID: 38281 RVA: 0x0034E17D File Offset: 0x0034D17D
				// (set) Token: 0x0600958A RID: 38282 RVA: 0x0034E185 File Offset: 0x0034D185
				public int RemovedCount { get; set; }

				// Token: 0x0600958B RID: 38283 RVA: 0x0034E18E File Offset: 0x0034D18E
				public void Enter()
				{
					this._level++;
				}

				// Token: 0x0600958C RID: 38284 RVA: 0x0034E1A0 File Offset: 0x0034D1A0
				public void Leave()
				{
					if (this._level > 0)
					{
						int num = this._level - 1;
						this._level = num;
						if (num == 0 && this.RemovedCount > 0)
						{
							this._owner.DoBatchRemove();
							this.RemovedCount = 0;
						}
					}
				}

				// Token: 0x0600958D RID: 38285 RVA: 0x0034E1E4 File Offset: 0x0034D1E4
				public void Dispose()
				{
					this.Leave();
				}

				// Token: 0x04004FD7 RID: 20439
				private Selector.InternalSelectedItemsStorage _owner;

				// Token: 0x04004FD8 RID: 20440
				private int _level;
			}
		}

		// Token: 0x02000C56 RID: 3158
		private class ItemInfoEqualityComparer : IEqualityComparer<ItemsControl.ItemInfo>
		{
			// Token: 0x060091CB RID: 37323 RVA: 0x0034AD4C File Offset: 0x00349D4C
			public ItemInfoEqualityComparer(bool matchUnresolved)
			{
				this._matchUnresolved = matchUnresolved;
			}

			// Token: 0x060091CC RID: 37324 RVA: 0x0034AD5B File Offset: 0x00349D5B
			bool IEqualityComparer<ItemsControl.ItemInfo>.Equals(ItemsControl.ItemInfo x, ItemsControl.ItemInfo y)
			{
				if (x == y)
				{
					return true;
				}
				if (!(x == null))
				{
					return x.Equals(y, this._matchUnresolved);
				}
				return y == null;
			}

			// Token: 0x060091CD RID: 37325 RVA: 0x00322958 File Offset: 0x00321958
			int IEqualityComparer<ItemsControl.ItemInfo>.GetHashCode(ItemsControl.ItemInfo x)
			{
				return x.GetHashCode();
			}

			// Token: 0x04004C67 RID: 19559
			private bool _matchUnresolved;
		}

		// Token: 0x02000C57 RID: 3159
		private class ChangeInfo
		{
			// Token: 0x060091CE RID: 37326 RVA: 0x0034AD81 File Offset: 0x00349D81
			public ChangeInfo(Selector.InternalSelectedItemsStorage toAdd, Selector.InternalSelectedItemsStorage toRemove)
			{
				this.ToAdd = toAdd;
				this.ToRemove = toRemove;
			}

			// Token: 0x17001FE9 RID: 8169
			// (get) Token: 0x060091CF RID: 37327 RVA: 0x0034AD97 File Offset: 0x00349D97
			// (set) Token: 0x060091D0 RID: 37328 RVA: 0x0034AD9F File Offset: 0x00349D9F
			public Selector.InternalSelectedItemsStorage ToAdd { get; private set; }

			// Token: 0x17001FEA RID: 8170
			// (get) Token: 0x060091D1 RID: 37329 RVA: 0x0034ADA8 File Offset: 0x00349DA8
			// (set) Token: 0x060091D2 RID: 37330 RVA: 0x0034ADB0 File Offset: 0x00349DB0
			public Selector.InternalSelectedItemsStorage ToRemove { get; private set; }
		}
	}
}
