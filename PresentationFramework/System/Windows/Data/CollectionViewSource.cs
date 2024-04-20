using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000458 RID: 1112
	public class CollectionViewSource : DependencyObject, ISupportInitialize, IWeakEventListener
	{
		// Token: 0x06003859 RID: 14425 RVA: 0x001E8C04 File Offset: 0x001E7C04
		public CollectionViewSource()
		{
			this._sort = new SortDescriptionCollection();
			((INotifyCollectionChanged)this._sort).CollectionChanged += this.OnForwardedCollectionChanged;
			this._groupBy = new ObservableCollection<GroupDescription>();
			((INotifyCollectionChanged)this._groupBy).CollectionChanged += this.OnForwardedCollectionChanged;
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x0600385A RID: 14426 RVA: 0x001E8C5B File Offset: 0x001E7C5B
		[ReadOnly(true)]
		public ICollectionView View
		{
			get
			{
				return CollectionViewSource.GetOriginalView(this.CollectionView);
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x0600385B RID: 14427 RVA: 0x001E8C68 File Offset: 0x001E7C68
		// (set) Token: 0x0600385C RID: 14428 RVA: 0x001E8C75 File Offset: 0x001E7C75
		public object Source
		{
			get
			{
				return base.GetValue(CollectionViewSource.SourceProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.SourceProperty, value);
			}
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x001E8C83 File Offset: 0x001E7C83
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionViewSource collectionViewSource = (CollectionViewSource)d;
			collectionViewSource.OnSourceChanged(e.OldValue, e.NewValue);
			collectionViewSource.EnsureView();
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnSourceChanged(object oldSource, object newSource)
		{
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x001E8CA4 File Offset: 0x001E7CA4
		private static bool IsSourceValid(object o)
		{
			return (o == null || o is IEnumerable || o is IListSource || o is DataSourceProvider) && !(o is ICollectionView);
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x001E8CCF File Offset: 0x001E7CCF
		private static bool IsValidSourceForView(object o)
		{
			return o is IEnumerable || o is IListSource;
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06003861 RID: 14433 RVA: 0x001E8CE4 File Offset: 0x001E7CE4
		// (set) Token: 0x06003862 RID: 14434 RVA: 0x001E8CF6 File Offset: 0x001E7CF6
		public Type CollectionViewType
		{
			get
			{
				return (Type)base.GetValue(CollectionViewSource.CollectionViewTypeProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.CollectionViewTypeProperty, value);
			}
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x001E8D04 File Offset: 0x001E7D04
		private static void OnCollectionViewTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionViewSource collectionViewSource = (CollectionViewSource)d;
			Type oldCollectionViewType = (Type)e.OldValue;
			Type newCollectionViewType = (Type)e.NewValue;
			if (!collectionViewSource._isInitializing)
			{
				throw new InvalidOperationException(SR.Get("CollectionViewTypeIsInitOnly"));
			}
			collectionViewSource.OnCollectionViewTypeChanged(oldCollectionViewType, newCollectionViewType);
			collectionViewSource.EnsureView();
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnCollectionViewTypeChanged(Type oldCollectionViewType, Type newCollectionViewType)
		{
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x001E8D58 File Offset: 0x001E7D58
		private static bool IsCollectionViewTypeValid(object o)
		{
			Type type = (Type)o;
			return type == null || typeof(ICollectionView).IsAssignableFrom(type);
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06003866 RID: 14438 RVA: 0x001E8D87 File Offset: 0x001E7D87
		// (set) Token: 0x06003867 RID: 14439 RVA: 0x001E8D8F File Offset: 0x001E7D8F
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				this._culture = value;
				this.OnForwardedPropertyChanged();
			}
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06003868 RID: 14440 RVA: 0x001E8D9E File Offset: 0x001E7D9E
		public SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this._sort;
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06003869 RID: 14441 RVA: 0x001E8DA6 File Offset: 0x001E7DA6
		public ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._groupBy;
			}
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x0600386A RID: 14442 RVA: 0x001E8DAE File Offset: 0x001E7DAE
		// (set) Token: 0x0600386B RID: 14443 RVA: 0x001E8DC0 File Offset: 0x001E7DC0
		[ReadOnly(true)]
		public bool CanChangeLiveSorting
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.CanChangeLiveSortingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.CanChangeLiveSortingPropertyKey, value);
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x0600386C RID: 14444 RVA: 0x001E8DCE File Offset: 0x001E7DCE
		// (set) Token: 0x0600386D RID: 14445 RVA: 0x001E8DE0 File Offset: 0x001E7DE0
		public bool IsLiveSortingRequested
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.IsLiveSortingRequestedProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.IsLiveSortingRequestedProperty, value);
			}
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x001E8DEE File Offset: 0x001E7DEE
		private static void OnIsLiveSortingRequestedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CollectionViewSource)d).OnForwardedPropertyChanged();
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x0600386F RID: 14447 RVA: 0x001E8DFB File Offset: 0x001E7DFB
		// (set) Token: 0x06003870 RID: 14448 RVA: 0x001E8E0D File Offset: 0x001E7E0D
		[ReadOnly(true)]
		public bool? IsLiveSorting
		{
			get
			{
				return (bool?)base.GetValue(CollectionViewSource.IsLiveSortingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.IsLiveSortingPropertyKey, value);
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06003871 RID: 14449 RVA: 0x001E8E20 File Offset: 0x001E7E20
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
					((INotifyCollectionChanged)this._liveSortingProperties).CollectionChanged += this.OnForwardedCollectionChanged;
				}
				return this._liveSortingProperties;
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06003872 RID: 14450 RVA: 0x001E8E52 File Offset: 0x001E7E52
		// (set) Token: 0x06003873 RID: 14451 RVA: 0x001E8E64 File Offset: 0x001E7E64
		[ReadOnly(true)]
		public bool CanChangeLiveFiltering
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.CanChangeLiveFilteringProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.CanChangeLiveFilteringPropertyKey, value);
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06003874 RID: 14452 RVA: 0x001E8E72 File Offset: 0x001E7E72
		// (set) Token: 0x06003875 RID: 14453 RVA: 0x001E8E84 File Offset: 0x001E7E84
		public bool IsLiveFilteringRequested
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.IsLiveFilteringRequestedProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.IsLiveFilteringRequestedProperty, value);
			}
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x001E8DEE File Offset: 0x001E7DEE
		private static void OnIsLiveFilteringRequestedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CollectionViewSource)d).OnForwardedPropertyChanged();
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06003877 RID: 14455 RVA: 0x001E8E92 File Offset: 0x001E7E92
		// (set) Token: 0x06003878 RID: 14456 RVA: 0x001E8EA4 File Offset: 0x001E7EA4
		[ReadOnly(true)]
		public bool? IsLiveFiltering
		{
			get
			{
				return (bool?)base.GetValue(CollectionViewSource.IsLiveFilteringProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.IsLiveFilteringPropertyKey, value);
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06003879 RID: 14457 RVA: 0x001E8EB7 File Offset: 0x001E7EB7
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
					((INotifyCollectionChanged)this._liveFilteringProperties).CollectionChanged += this.OnForwardedCollectionChanged;
				}
				return this._liveFilteringProperties;
			}
		}

		// Token: 0x17000C20 RID: 3104
		// (get) Token: 0x0600387A RID: 14458 RVA: 0x001E8EE9 File Offset: 0x001E7EE9
		// (set) Token: 0x0600387B RID: 14459 RVA: 0x001E8EFB File Offset: 0x001E7EFB
		[ReadOnly(true)]
		public bool CanChangeLiveGrouping
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.CanChangeLiveGroupingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.CanChangeLiveGroupingPropertyKey, value);
			}
		}

		// Token: 0x17000C21 RID: 3105
		// (get) Token: 0x0600387C RID: 14460 RVA: 0x001E8F09 File Offset: 0x001E7F09
		// (set) Token: 0x0600387D RID: 14461 RVA: 0x001E8F1B File Offset: 0x001E7F1B
		public bool IsLiveGroupingRequested
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.IsLiveGroupingRequestedProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.IsLiveGroupingRequestedProperty, value);
			}
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x001E8DEE File Offset: 0x001E7DEE
		private static void OnIsLiveGroupingRequestedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CollectionViewSource)d).OnForwardedPropertyChanged();
		}

		// Token: 0x17000C22 RID: 3106
		// (get) Token: 0x0600387F RID: 14463 RVA: 0x001E8F29 File Offset: 0x001E7F29
		// (set) Token: 0x06003880 RID: 14464 RVA: 0x001E8F3B File Offset: 0x001E7F3B
		[ReadOnly(true)]
		public bool? IsLiveGrouping
		{
			get
			{
				return (bool?)base.GetValue(CollectionViewSource.IsLiveGroupingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.IsLiveGroupingPropertyKey, value);
			}
		}

		// Token: 0x17000C23 RID: 3107
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x001E8F4E File Offset: 0x001E7F4E
		public ObservableCollection<string> LiveGroupingProperties
		{
			get
			{
				if (this._liveGroupingProperties == null)
				{
					this._liveGroupingProperties = new ObservableCollection<string>();
					((INotifyCollectionChanged)this._liveGroupingProperties).CollectionChanged += this.OnForwardedCollectionChanged;
				}
				return this._liveGroupingProperties;
			}
		}

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06003882 RID: 14466 RVA: 0x001E8F80 File Offset: 0x001E7F80
		// (remove) Token: 0x06003883 RID: 14467 RVA: 0x001E8FC0 File Offset: 0x001E7FC0
		public event FilterEventHandler Filter
		{
			add
			{
				FilterEventHandler filterEventHandler = CollectionViewSource.FilterHandlersField.GetValue(this);
				if (filterEventHandler != null)
				{
					filterEventHandler = (FilterEventHandler)Delegate.Combine(filterEventHandler, value);
				}
				else
				{
					filterEventHandler = value;
				}
				CollectionViewSource.FilterHandlersField.SetValue(this, filterEventHandler);
				this.OnForwardedPropertyChanged();
			}
			remove
			{
				FilterEventHandler filterEventHandler = CollectionViewSource.FilterHandlersField.GetValue(this);
				if (filterEventHandler != null)
				{
					filterEventHandler = (FilterEventHandler)Delegate.Remove(filterEventHandler, value);
					if (filterEventHandler == null)
					{
						CollectionViewSource.FilterHandlersField.ClearValue(this);
					}
					else
					{
						CollectionViewSource.FilterHandlersField.SetValue(this, filterEventHandler);
					}
				}
				this.OnForwardedPropertyChanged();
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x001E900B File Offset: 0x001E800B
		public static ICollectionView GetDefaultView(object source)
		{
			return CollectionViewSource.GetOriginalView(CollectionViewSource.GetDefaultCollectionView(source, true, null));
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x001E901A File Offset: 0x001E801A
		private static ICollectionView LazyGetDefaultView(object source)
		{
			return CollectionViewSource.GetOriginalView(CollectionViewSource.GetDefaultCollectionView(source, false, null));
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x001E902C File Offset: 0x001E802C
		public static bool IsDefaultView(ICollectionView view)
		{
			if (view != null)
			{
				object sourceCollection = view.SourceCollection;
				return CollectionViewSource.GetOriginalView(view) == CollectionViewSource.LazyGetDefaultView(sourceCollection);
			}
			return true;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x001E9053 File Offset: 0x001E8053
		public IDisposable DeferRefresh()
		{
			return new CollectionViewSource.DeferHelper(this);
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x001E905B File Offset: 0x001E805B
		void ISupportInitialize.BeginInit()
		{
			this._isInitializing = true;
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x001E9064 File Offset: 0x001E8064
		void ISupportInitialize.EndInit()
		{
			this._isInitializing = false;
			this.EnsureView();
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x001E9073 File Offset: 0x001E8073
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x17000C24 RID: 3108
		// (get) Token: 0x0600388C RID: 14476 RVA: 0x001E9080 File Offset: 0x001E8080
		internal CollectionView CollectionView
		{
			get
			{
				ICollectionView collectionView = (ICollectionView)base.GetValue(CollectionViewSource.ViewProperty);
				if (collectionView != null && !this._isViewInitialized)
				{
					object obj = this.Source;
					DataSourceProvider dataSourceProvider = obj as DataSourceProvider;
					if (dataSourceProvider != null)
					{
						obj = dataSourceProvider.Data;
					}
					if (obj != null)
					{
						ViewRecord viewRecord = DataBindEngine.CurrentDataBindEngine.GetViewRecord(obj, this, this.CollectionViewType, true, null);
						if (viewRecord != null)
						{
							viewRecord.InitializeView();
							this._isViewInitialized = true;
						}
					}
				}
				return (CollectionView)collectionView;
			}
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x0600388D RID: 14477 RVA: 0x001E90ED File Offset: 0x001E80ED
		internal DependencyProperty PropertyForInheritanceContext
		{
			get
			{
				return this._propertyForInheritanceContext;
			}
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x001E90F8 File Offset: 0x001E80F8
		internal static CollectionView GetDefaultCollectionView(object source, bool createView, Func<object, object> GetSourceItem = null)
		{
			if (!CollectionViewSource.IsValidSourceForView(source))
			{
				return null;
			}
			ViewRecord viewRecord = DataBindEngine.CurrentDataBindEngine.GetViewRecord(source, CollectionViewSource.DefaultSource, null, createView, GetSourceItem);
			if (viewRecord == null)
			{
				return null;
			}
			return (CollectionView)viewRecord.View;
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x001E9134 File Offset: 0x001E8134
		internal static CollectionView GetDefaultCollectionView(object source, DependencyObject d, Func<object, object> GetSourceItem = null)
		{
			CollectionView defaultCollectionView = CollectionViewSource.GetDefaultCollectionView(source, true, GetSourceItem);
			if (defaultCollectionView != null && defaultCollectionView.Culture == null)
			{
				XmlLanguage xmlLanguage = (d != null) ? ((XmlLanguage)d.GetValue(FrameworkElement.LanguageProperty)) : null;
				if (xmlLanguage != null)
				{
					try
					{
						defaultCollectionView.Culture = xmlLanguage.GetSpecificCulture();
					}
					catch (InvalidOperationException)
					{
					}
				}
			}
			return defaultCollectionView;
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06003890 RID: 14480 RVA: 0x001E9194 File Offset: 0x001E8194
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x001E919C File Offset: 0x001E819C
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (!this._hasMultipleInheritanceContexts && this._inheritanceContext == null)
			{
				this._propertyForInheritanceContext = property;
			}
			else
			{
				this._propertyForInheritanceContext = null;
			}
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x001E91D1 File Offset: 0x001E81D1
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
			this._propertyForInheritanceContext = null;
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06003893 RID: 14483 RVA: 0x001E91ED File Offset: 0x001E81ED
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x00105F35 File Offset: 0x00104F35
		internal bool IsShareableInTemplate()
		{
			return false;
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x001E91F5 File Offset: 0x001E81F5
		private void EnsureView()
		{
			this.EnsureView(this.Source, this.CollectionViewType);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x001E920C File Offset: 0x001E820C
		private void EnsureView(object source, Type collectionViewType)
		{
			if (this._isInitializing || this._deferLevel > 0)
			{
				return;
			}
			DataSourceProvider dataSourceProvider = source as DataSourceProvider;
			if (dataSourceProvider != this._dataProvider)
			{
				if (this._dataProvider != null)
				{
					DataChangedEventManager.RemoveHandler(this._dataProvider, new EventHandler<EventArgs>(this.OnDataChanged));
				}
				this._dataProvider = dataSourceProvider;
				if (this._dataProvider != null)
				{
					DataChangedEventManager.AddHandler(this._dataProvider, new EventHandler<EventArgs>(this.OnDataChanged));
					this._dataProvider.InitialLoad();
				}
			}
			if (dataSourceProvider != null)
			{
				source = dataSourceProvider.Data;
			}
			ICollectionView collectionView = null;
			if (source != null)
			{
				ViewRecord viewRecord = DataBindEngine.CurrentDataBindEngine.GetViewRecord(source, this, collectionViewType, true, delegate(object x)
				{
					BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(this, CollectionViewSource.SourceProperty);
					if (bindingExpressionBase == null)
					{
						return null;
					}
					return bindingExpressionBase.GetSourceItem(x);
				});
				if (viewRecord != null)
				{
					collectionView = viewRecord.View;
					this._isViewInitialized = viewRecord.IsInitialized;
					if (this._version != viewRecord.Version)
					{
						this.ApplyPropertiesToView(collectionView);
						viewRecord.Version = this._version;
					}
				}
			}
			base.SetValue(CollectionViewSource.ViewPropertyKey, collectionView);
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x001E92FC File Offset: 0x001E82FC
		private void ApplyPropertiesToView(ICollectionView view)
		{
			if (view == null || this._deferLevel > 0)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = view as ICollectionViewLiveShaping;
			using (view.DeferRefresh())
			{
				if (this.Culture != null)
				{
					view.Culture = this.Culture;
				}
				if (view.CanSort)
				{
					view.SortDescriptions.Clear();
					int i = 0;
					int count = this.SortDescriptions.Count;
					while (i < count)
					{
						view.SortDescriptions.Add(this.SortDescriptions[i]);
						i++;
					}
				}
				else if (this.SortDescriptions.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("CannotSortView", new object[]
					{
						view
					}));
				}
				Predicate<object> predicate;
				if (CollectionViewSource.FilterHandlersField.GetValue(this) != null)
				{
					predicate = this.FilterWrapper;
				}
				else
				{
					predicate = null;
				}
				if (view.CanFilter)
				{
					view.Filter = predicate;
				}
				else if (predicate != null)
				{
					throw new InvalidOperationException(SR.Get("CannotFilterView", new object[]
					{
						view
					}));
				}
				if (view.CanGroup)
				{
					view.GroupDescriptions.Clear();
					int i = 0;
					int count = this.GroupDescriptions.Count;
					while (i < count)
					{
						view.GroupDescriptions.Add(this.GroupDescriptions[i]);
						i++;
					}
				}
				else if (this.GroupDescriptions.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("CannotGroupView", new object[]
					{
						view
					}));
				}
				if (collectionViewLiveShaping != null)
				{
					if (collectionViewLiveShaping.CanChangeLiveSorting)
					{
						collectionViewLiveShaping.IsLiveSorting = new bool?(this.IsLiveSortingRequested);
						ObservableCollection<string> observableCollection = collectionViewLiveShaping.LiveSortingProperties;
						observableCollection.Clear();
						if (this.IsLiveSortingRequested)
						{
							foreach (string item in this.LiveSortingProperties)
							{
								observableCollection.Add(item);
							}
						}
					}
					this.CanChangeLiveSorting = collectionViewLiveShaping.CanChangeLiveSorting;
					this.IsLiveSorting = collectionViewLiveShaping.IsLiveSorting;
					if (collectionViewLiveShaping.CanChangeLiveFiltering)
					{
						collectionViewLiveShaping.IsLiveFiltering = new bool?(this.IsLiveFilteringRequested);
						ObservableCollection<string> observableCollection = collectionViewLiveShaping.LiveFilteringProperties;
						observableCollection.Clear();
						if (this.IsLiveFilteringRequested)
						{
							foreach (string item2 in this.LiveFilteringProperties)
							{
								observableCollection.Add(item2);
							}
						}
					}
					this.CanChangeLiveFiltering = collectionViewLiveShaping.CanChangeLiveFiltering;
					this.IsLiveFiltering = collectionViewLiveShaping.IsLiveFiltering;
					if (collectionViewLiveShaping.CanChangeLiveGrouping)
					{
						collectionViewLiveShaping.IsLiveGrouping = new bool?(this.IsLiveGroupingRequested);
						ObservableCollection<string> observableCollection = collectionViewLiveShaping.LiveGroupingProperties;
						observableCollection.Clear();
						if (this.IsLiveGroupingRequested)
						{
							foreach (string item3 in this.LiveGroupingProperties)
							{
								observableCollection.Add(item3);
							}
						}
					}
					this.CanChangeLiveGrouping = collectionViewLiveShaping.CanChangeLiveGrouping;
					this.IsLiveGrouping = collectionViewLiveShaping.IsLiveGrouping;
				}
				else
				{
					this.CanChangeLiveSorting = false;
					this.IsLiveSorting = null;
					this.CanChangeLiveFiltering = false;
					this.IsLiveFiltering = null;
					this.CanChangeLiveGrouping = false;
					this.IsLiveGrouping = null;
				}
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x001E9698 File Offset: 0x001E8698
		private static ICollectionView GetOriginalView(ICollectionView view)
		{
			for (CollectionViewProxy collectionViewProxy = view as CollectionViewProxy; collectionViewProxy != null; collectionViewProxy = (view as CollectionViewProxy))
			{
				view = collectionViewProxy.ProxiedView;
			}
			return view;
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06003899 RID: 14489 RVA: 0x001E96C1 File Offset: 0x001E86C1
		private Predicate<object> FilterWrapper
		{
			get
			{
				if (this._filterStub == null)
				{
					this._filterStub = new CollectionViewSource.FilterStub(this);
				}
				return this._filterStub.FilterWrapper;
			}
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x001E96E4 File Offset: 0x001E86E4
		private bool WrapFilter(object item)
		{
			FilterEventArgs filterEventArgs = new FilterEventArgs(item);
			FilterEventHandler value = CollectionViewSource.FilterHandlersField.GetValue(this);
			if (value != null)
			{
				value(this, filterEventArgs);
			}
			return filterEventArgs.Accepted;
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x001E9715 File Offset: 0x001E8715
		private void OnDataChanged(object sender, EventArgs e)
		{
			this.EnsureView();
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x001E971D File Offset: 0x001E871D
		private void OnForwardedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnForwardedPropertyChanged();
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x001E9725 File Offset: 0x001E8725
		private void OnForwardedPropertyChanged()
		{
			this._version++;
			this.ApplyPropertiesToView(this.View);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x001E9741 File Offset: 0x001E8741
		private void BeginDefer()
		{
			this._deferLevel++;
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x001E9754 File Offset: 0x001E8754
		private void EndDefer()
		{
			int num = this._deferLevel - 1;
			this._deferLevel = num;
			if (num == 0)
			{
				this.EnsureView();
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x060038A0 RID: 14496 RVA: 0x001E977A File Offset: 0x001E877A
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x04001D27 RID: 7463
		private static readonly DependencyPropertyKey ViewPropertyKey = DependencyProperty.RegisterReadOnly("View", typeof(ICollectionView), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		// Token: 0x04001D28 RID: 7464
		public static readonly DependencyProperty ViewProperty = CollectionViewSource.ViewPropertyKey.DependencyProperty;

		// Token: 0x04001D29 RID: 7465
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(CollectionViewSource.OnSourceChanged)), new ValidateValueCallback(CollectionViewSource.IsSourceValid));

		// Token: 0x04001D2A RID: 7466
		public static readonly DependencyProperty CollectionViewTypeProperty = DependencyProperty.Register("CollectionViewType", typeof(Type), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(CollectionViewSource.OnCollectionViewTypeChanged)), new ValidateValueCallback(CollectionViewSource.IsCollectionViewTypeValid));

		// Token: 0x04001D2B RID: 7467
		private static readonly DependencyPropertyKey CanChangeLiveSortingPropertyKey = DependencyProperty.RegisterReadOnly("CanChangeLiveSorting", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false));

		// Token: 0x04001D2C RID: 7468
		public static readonly DependencyProperty CanChangeLiveSortingProperty = CollectionViewSource.CanChangeLiveSortingPropertyKey.DependencyProperty;

		// Token: 0x04001D2D RID: 7469
		public static readonly DependencyProperty IsLiveSortingRequestedProperty = DependencyProperty.Register("IsLiveSortingRequested", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(CollectionViewSource.OnIsLiveSortingRequestedChanged)));

		// Token: 0x04001D2E RID: 7470
		private static readonly DependencyPropertyKey IsLiveSortingPropertyKey = DependencyProperty.RegisterReadOnly("IsLiveSorting", typeof(bool?), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		// Token: 0x04001D2F RID: 7471
		public static readonly DependencyProperty IsLiveSortingProperty = CollectionViewSource.IsLiveSortingPropertyKey.DependencyProperty;

		// Token: 0x04001D30 RID: 7472
		private static readonly DependencyPropertyKey CanChangeLiveFilteringPropertyKey = DependencyProperty.RegisterReadOnly("CanChangeLiveFiltering", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false));

		// Token: 0x04001D31 RID: 7473
		public static readonly DependencyProperty CanChangeLiveFilteringProperty = CollectionViewSource.CanChangeLiveFilteringPropertyKey.DependencyProperty;

		// Token: 0x04001D32 RID: 7474
		public static readonly DependencyProperty IsLiveFilteringRequestedProperty = DependencyProperty.Register("IsLiveFilteringRequested", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(CollectionViewSource.OnIsLiveFilteringRequestedChanged)));

		// Token: 0x04001D33 RID: 7475
		private static readonly DependencyPropertyKey IsLiveFilteringPropertyKey = DependencyProperty.RegisterReadOnly("IsLiveFiltering", typeof(bool?), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		// Token: 0x04001D34 RID: 7476
		public static readonly DependencyProperty IsLiveFilteringProperty = CollectionViewSource.IsLiveFilteringPropertyKey.DependencyProperty;

		// Token: 0x04001D35 RID: 7477
		private static readonly DependencyPropertyKey CanChangeLiveGroupingPropertyKey = DependencyProperty.RegisterReadOnly("CanChangeLiveGrouping", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false));

		// Token: 0x04001D36 RID: 7478
		public static readonly DependencyProperty CanChangeLiveGroupingProperty = CollectionViewSource.CanChangeLiveGroupingPropertyKey.DependencyProperty;

		// Token: 0x04001D37 RID: 7479
		public static readonly DependencyProperty IsLiveGroupingRequestedProperty = DependencyProperty.Register("IsLiveGroupingRequested", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(CollectionViewSource.OnIsLiveGroupingRequestedChanged)));

		// Token: 0x04001D38 RID: 7480
		private static readonly DependencyPropertyKey IsLiveGroupingPropertyKey = DependencyProperty.RegisterReadOnly("IsLiveGrouping", typeof(bool?), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		// Token: 0x04001D39 RID: 7481
		public static readonly DependencyProperty IsLiveGroupingProperty = CollectionViewSource.IsLiveGroupingPropertyKey.DependencyProperty;

		// Token: 0x04001D3A RID: 7482
		private CultureInfo _culture;

		// Token: 0x04001D3B RID: 7483
		private SortDescriptionCollection _sort;

		// Token: 0x04001D3C RID: 7484
		private ObservableCollection<GroupDescription> _groupBy;

		// Token: 0x04001D3D RID: 7485
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x04001D3E RID: 7486
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x04001D3F RID: 7487
		private ObservableCollection<string> _liveGroupingProperties;

		// Token: 0x04001D40 RID: 7488
		private bool _isInitializing;

		// Token: 0x04001D41 RID: 7489
		private bool _isViewInitialized;

		// Token: 0x04001D42 RID: 7490
		private int _version;

		// Token: 0x04001D43 RID: 7491
		private int _deferLevel;

		// Token: 0x04001D44 RID: 7492
		private DataSourceProvider _dataProvider;

		// Token: 0x04001D45 RID: 7493
		private CollectionViewSource.FilterStub _filterStub;

		// Token: 0x04001D46 RID: 7494
		private DependencyObject _inheritanceContext;

		// Token: 0x04001D47 RID: 7495
		private bool _hasMultipleInheritanceContexts;

		// Token: 0x04001D48 RID: 7496
		private DependencyProperty _propertyForInheritanceContext;

		// Token: 0x04001D49 RID: 7497
		internal static readonly CollectionViewSource DefaultSource = new CollectionViewSource();

		// Token: 0x04001D4A RID: 7498
		private static readonly UncommonField<FilterEventHandler> FilterHandlersField = new UncommonField<FilterEventHandler>();

		// Token: 0x02000AE4 RID: 2788
		private class DeferHelper : IDisposable
		{
			// Token: 0x06008B47 RID: 35655 RVA: 0x00339960 File Offset: 0x00338960
			public DeferHelper(CollectionViewSource target)
			{
				this._target = target;
				this._target.BeginDefer();
			}

			// Token: 0x06008B48 RID: 35656 RVA: 0x0033997A File Offset: 0x0033897A
			public void Dispose()
			{
				if (this._target != null)
				{
					CollectionViewSource target = this._target;
					this._target = null;
					target.EndDefer();
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x0400471A RID: 18202
			private CollectionViewSource _target;
		}

		// Token: 0x02000AE5 RID: 2789
		private class FilterStub
		{
			// Token: 0x06008B49 RID: 35657 RVA: 0x0033999C File Offset: 0x0033899C
			public FilterStub(CollectionViewSource parent)
			{
				this._parent = new WeakReference(parent);
				this._filterWrapper = new Predicate<object>(this.WrapFilter);
			}

			// Token: 0x17001E80 RID: 7808
			// (get) Token: 0x06008B4A RID: 35658 RVA: 0x003399C2 File Offset: 0x003389C2
			public Predicate<object> FilterWrapper
			{
				get
				{
					return this._filterWrapper;
				}
			}

			// Token: 0x06008B4B RID: 35659 RVA: 0x003399CC File Offset: 0x003389CC
			private bool WrapFilter(object item)
			{
				CollectionViewSource collectionViewSource = (CollectionViewSource)this._parent.Target;
				return collectionViewSource == null || collectionViewSource.WrapFilter(item);
			}

			// Token: 0x0400471B RID: 18203
			private WeakReference _parent;

			// Token: 0x0400471C RID: 18204
			private Predicate<object> _filterWrapper;
		}
	}
}
