using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Utility;

namespace System.Windows.Data
{
	// Token: 0x02000452 RID: 1106
	public class CollectionContainer : DependencyObject, INotifyCollectionChanged, IWeakEventListener
	{
		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060037C5 RID: 14277 RVA: 0x001E6DD7 File Offset: 0x001E5DD7
		// (set) Token: 0x060037C6 RID: 14278 RVA: 0x001E6DE9 File Offset: 0x001E5DE9
		public IEnumerable Collection
		{
			get
			{
				return (IEnumerable)base.GetValue(CollectionContainer.CollectionProperty);
			}
			set
			{
				base.SetValue(CollectionContainer.CollectionProperty, value);
			}
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x001E6DF8 File Offset: 0x001E5DF8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCollection()
		{
			if (this.Collection == null)
			{
				return false;
			}
			ICollection collection = this.Collection as ICollection;
			if (collection != null && collection.Count == 0)
			{
				return false;
			}
			IEnumerator enumerator = this.Collection.GetEnumerator();
			bool result = enumerator.MoveNext();
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return result;
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060037C8 RID: 14280 RVA: 0x001E6E4A File Offset: 0x001E5E4A
		internal ICollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060037C9 RID: 14281 RVA: 0x001E6E54 File Offset: 0x001E5E54
		internal int ViewCount
		{
			get
			{
				if (this.View == null)
				{
					return 0;
				}
				CollectionView collectionView = this.View as CollectionView;
				if (collectionView != null)
				{
					return collectionView.Count;
				}
				ICollection collection = this.View as ICollection;
				if (collection != null)
				{
					return collection.Count;
				}
				if (this.ViewList != null)
				{
					return this.ViewList.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060037CA RID: 14282 RVA: 0x001E6EAC File Offset: 0x001E5EAC
		internal bool ViewIsEmpty
		{
			get
			{
				if (this.View == null)
				{
					return true;
				}
				ICollectionView view = this.View;
				if (view != null)
				{
					return view.IsEmpty;
				}
				ICollection collection = this.View as ICollection;
				if (collection != null)
				{
					return collection.Count == 0;
				}
				if (this.ViewList == null)
				{
					return true;
				}
				IndexedEnumerable viewList = this.ViewList;
				if (viewList != null)
				{
					return viewList.IsEmpty;
				}
				return this.ViewList.Count == 0;
			}
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x001E6F18 File Offset: 0x001E5F18
		internal object ViewItem(int index)
		{
			Invariant.Assert(index >= 0 && this.View != null);
			CollectionView collectionView = this.View as CollectionView;
			if (collectionView != null)
			{
				return collectionView.GetItemAt(index);
			}
			if (this.ViewList != null)
			{
				return this.ViewList[index];
			}
			return null;
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x001E6F68 File Offset: 0x001E5F68
		internal int ViewIndexOf(object item)
		{
			if (this.View == null)
			{
				return -1;
			}
			CollectionView collectionView = this.View as CollectionView;
			if (collectionView != null)
			{
				return collectionView.IndexOf(item);
			}
			if (this.ViewList != null)
			{
				return this.ViewList.IndexOf(item);
			}
			return -1;
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x001E6FAC File Offset: 0x001E5FAC
		internal void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._view != null)
			{
				CollectionView collectionView = this._view as CollectionView;
				if (collectionView != null)
				{
					collectionView.GetCollectionChangedSources(level + 1, format, sources);
					return;
				}
				format(level + 1, this._view, new bool?(true), sources);
			}
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x060037CE RID: 14286 RVA: 0x001E7001 File Offset: 0x001E6001
		// (remove) Token: 0x060037CF RID: 14287 RVA: 0x001E700A File Offset: 0x001E600A
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add
			{
				this.CollectionChanged += value;
			}
			remove
			{
				this.CollectionChanged -= value;
			}
		}

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x060037D0 RID: 14288 RVA: 0x001E7014 File Offset: 0x001E6014
		// (remove) Token: 0x060037D1 RID: 14289 RVA: 0x001E704C File Offset: 0x001E604C
		protected virtual event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060037D2 RID: 14290 RVA: 0x001E7081 File Offset: 0x001E6081
		protected virtual void OnContainedCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x001E7098 File Offset: 0x001E6098
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060037D5 RID: 14293 RVA: 0x001E70A3 File Offset: 0x001E60A3
		private IndexedEnumerable ViewList
		{
			get
			{
				if (this._viewList == null && this.View != null)
				{
					this._viewList = new IndexedEnumerable(this.View);
				}
				return this._viewList;
			}
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x001E70CC File Offset: 0x001E60CC
		private static object OnGetCollection(DependencyObject d)
		{
			return ((CollectionContainer)d).Collection;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x001E70D9 File Offset: 0x001E60D9
		private static void OnCollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((CollectionContainer)d).HookUpToCollection((IEnumerable)e.NewValue, true);
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x001E70F4 File Offset: 0x001E60F4
		private void HookUpToCollection(IEnumerable newCollection, bool shouldRaiseChangeEvent)
		{
			this._viewList = null;
			if (this.View != null)
			{
				CollectionChangedEventManager.RemoveHandler(this.View, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				if (this._traceLog != null)
				{
					this._traceLog.Add("Unsubscribe to CollectionChange from {0}", new object[]
					{
						TraceLog.IdFor(this.View)
					});
				}
			}
			if (newCollection != null)
			{
				this._view = CollectionViewSource.GetDefaultCollectionView(newCollection, this, null);
			}
			else
			{
				this._view = null;
			}
			if (this.View != null)
			{
				CollectionChangedEventManager.AddHandler(this.View, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				if (this._traceLog != null)
				{
					this._traceLog.Add("Subscribe to CollectionChange from {0}", new object[]
					{
						TraceLog.IdFor(this.View)
					});
				}
			}
			if (shouldRaiseChangeEvent)
			{
				this.OnContainedCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x001E71C7 File Offset: 0x001E61C7
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnContainedCollectionChanged(e);
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x001E71D0 File Offset: 0x001E61D0
		private void InitializeTraceLog()
		{
			this._traceLog = new TraceLog(20);
		}

		// Token: 0x04001CF9 RID: 7417
		public static readonly DependencyProperty CollectionProperty = DependencyProperty.Register("Collection", typeof(IEnumerable), typeof(CollectionContainer), new FrameworkPropertyMetadata(new PropertyChangedCallback(CollectionContainer.OnCollectionPropertyChanged)));

		// Token: 0x04001CFB RID: 7419
		private TraceLog _traceLog;

		// Token: 0x04001CFC RID: 7420
		private ICollectionView _view;

		// Token: 0x04001CFD RID: 7421
		private IndexedEnumerable _viewList;
	}
}
