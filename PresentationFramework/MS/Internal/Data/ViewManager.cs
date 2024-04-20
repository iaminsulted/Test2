﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200024A RID: 586
	internal class ViewManager : HybridDictionary
	{
		// Token: 0x1700045E RID: 1118
		public CollectionRecord this[object o]
		{
			get
			{
				WeakRefKey weakRefKey = new WeakRefKey(o);
				return (CollectionRecord)base[weakRefKey];
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x0015AEE2 File Offset: 0x00159EE2
		internal void Add(object collection, CollectionRecord cr)
		{
			base.Add(new WeakRefKey(collection), cr);
			DataBindEngine.CurrentDataBindEngine.ScheduleCleanup();
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0015AF00 File Offset: 0x00159F00
		internal ViewRecord GetViewRecord(object collection, CollectionViewSource cvs, Type collectionViewType, bool createView, Func<object, object> GetSourceItem)
		{
			ViewRecord viewRecord = this.GetExistingView(collection, cvs, collectionViewType, GetSourceItem);
			if (viewRecord != null || !createView)
			{
				return viewRecord;
			}
			IListSource listSource = collection as IListSource;
			IList list = null;
			if (listSource != null)
			{
				list = listSource.GetList();
				viewRecord = this.GetExistingView(list, cvs, collectionViewType, GetSourceItem);
				if (viewRecord != null)
				{
					return this.CacheView(collection, cvs, (CollectionView)viewRecord.View, viewRecord);
				}
			}
			ICollectionView collectionView = collection as ICollectionView;
			if (collectionView != null)
			{
				collectionView = new CollectionViewProxy(collectionView);
			}
			else if (collectionViewType == null)
			{
				ICollectionViewFactory collectionViewFactory = collection as ICollectionViewFactory;
				if (collectionViewFactory != null)
				{
					collectionView = collectionViewFactory.CreateView();
				}
				else
				{
					IList list2 = (list != null) ? list : (collection as IList);
					if (list2 != null)
					{
						IBindingList bindingList = list2 as IBindingList;
						if (bindingList != null)
						{
							collectionView = new BindingListCollectionView(bindingList);
						}
						else
						{
							collectionView = new ListCollectionView(list2);
						}
					}
					else
					{
						IEnumerable enumerable = collection as IEnumerable;
						if (enumerable != null)
						{
							collectionView = new EnumerableCollectionView(enumerable);
						}
					}
				}
			}
			else
			{
				if (!typeof(ICollectionView).IsAssignableFrom(collectionViewType))
				{
					throw new ArgumentException(SR.Get("CollectionView_WrongType", new object[]
					{
						collectionViewType.Name
					}));
				}
				object obj = (list != null) ? list : collection;
				try
				{
					collectionView = (Activator.CreateInstance(collectionViewType, BindingFlags.CreateInstance, null, new object[]
					{
						obj
					}, null) as ICollectionView);
				}
				catch (MissingMethodException innerException)
				{
					throw new ArgumentException(SR.Get("CollectionView_ViewTypeInsufficient", new object[]
					{
						collectionViewType.Name,
						collection.GetType()
					}), innerException);
				}
			}
			if (collectionView != null)
			{
				CollectionView collectionView2 = collectionView as CollectionView;
				if (collectionView2 == null)
				{
					collectionView2 = new CollectionViewProxy(collectionView);
				}
				if (list != null)
				{
					viewRecord = this.CacheView(list, cvs, collectionView2, null);
				}
				viewRecord = this.CacheView(collection, cvs, collectionView2, viewRecord);
				BindingOperations.OnCollectionViewRegistering(collectionView2);
			}
			return viewRecord;
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0015B0B8 File Offset: 0x0015A0B8
		private CollectionRecord EnsureCollectionRecord(object collection, Func<object, object> GetSourceItem = null)
		{
			CollectionRecord collectionRecord = this[collection];
			if (collectionRecord == null)
			{
				collectionRecord = new CollectionRecord();
				this.Add(collection, collectionRecord);
				object parent = (GetSourceItem != null) ? GetSourceItem(collection) : null;
				IEnumerable enumerable = collection as IEnumerable;
				if (enumerable != null)
				{
					BindingOperations.OnCollectionRegistering(enumerable, parent);
				}
			}
			return collectionRecord;
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0015B100 File Offset: 0x0015A100
		internal void RegisterCollectionSynchronizationCallback(IEnumerable collection, object context, CollectionSynchronizationCallback synchronizationCallback)
		{
			CollectionRecord collectionRecord = this.EnsureCollectionRecord(collection, null);
			collectionRecord.SynchronizationInfo = new SynchronizationInfo(context, synchronizationCallback);
			ViewTable viewTable = collectionRecord.ViewTable;
			if (viewTable != null)
			{
				bool isSynchronized = collectionRecord.SynchronizationInfo.IsSynchronized;
				foreach (object obj in viewTable)
				{
					CollectionView collectionView = ((ViewRecord)((DictionaryEntry)obj).Value).View as CollectionView;
					if (collectionView != null)
					{
						collectionView.SetAllowsCrossThreadChanges(isSynchronized);
					}
				}
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x0015B1A4 File Offset: 0x0015A1A4
		internal SynchronizationInfo GetSynchronizationInfo(IEnumerable collection)
		{
			CollectionRecord collectionRecord = this[collection];
			if (collectionRecord == null)
			{
				return SynchronizationInfo.None;
			}
			return collectionRecord.SynchronizationInfo;
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0015B1C8 File Offset: 0x0015A1C8
		public void AccessCollection(IEnumerable collection, Action accessMethod, bool writeAccess)
		{
			this.GetSynchronizationInfo(collection).AccessCollection(collection, accessMethod, writeAccess);
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x0015B1E7 File Offset: 0x0015A1E7
		internal static ViewManager Current
		{
			get
			{
				return DataBindEngine.CurrentDataBindEngine.ViewManager;
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0015B1F4 File Offset: 0x0015A1F4
		private ViewRecord GetExistingView(object collection, CollectionViewSource cvs, Type collectionViewType, Func<object, object> GetSourceItem)
		{
			CollectionView collectionView = collection as CollectionView;
			ViewRecord result;
			if (collectionView == null)
			{
				ViewTable viewTable = this.EnsureCollectionRecord(collection, GetSourceItem).ViewTable;
				if (viewTable != null)
				{
					ViewRecord viewRecord = viewTable[cvs];
					if (viewRecord != null)
					{
						collectionView = (CollectionView)viewRecord.View;
					}
					result = viewRecord;
					if (this._inactiveViewTables.Contains(viewTable))
					{
						this._inactiveViewTables[viewTable] = 2;
					}
				}
				else
				{
					result = null;
				}
			}
			else
			{
				result = new ViewRecord(collectionView);
			}
			if (collectionView != null)
			{
				this.ValidateViewType(collectionView, collectionViewType);
			}
			return result;
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0015B270 File Offset: 0x0015A270
		private ViewRecord CacheView(object collection, CollectionViewSource cvs, CollectionView cv, ViewRecord vr)
		{
			CollectionRecord collectionRecord = this[collection];
			ViewTable viewTable = collectionRecord.ViewTable;
			if (viewTable == null)
			{
				viewTable = new ViewTable();
				collectionRecord.ViewTable = viewTable;
				if (!(collection is INotifyCollectionChanged))
				{
					this._inactiveViewTables.Add(viewTable, 2);
				}
			}
			if (vr == null)
			{
				vr = new ViewRecord(cv);
			}
			else if (cv == null)
			{
				cv = (CollectionView)vr.View;
			}
			cv.SetViewManagerData(viewTable);
			viewTable[cvs] = vr;
			return vr;
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0015B2E8 File Offset: 0x0015A2E8
		internal bool Purge()
		{
			int count = this._inactiveViewTables.Count;
			if (count > 0)
			{
				ViewTable[] array = new ViewTable[count];
				this._inactiveViewTables.Keys.CopyTo(array, 0);
				for (int i = 0; i < count; i++)
				{
					ViewTable key = array[i];
					int num = (int)this._inactiveViewTables[key];
					if (--num > 0)
					{
						this._inactiveViewTables[key] = num;
					}
					else
					{
						this._inactiveViewTables.Remove(key);
					}
				}
			}
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			foreach (object obj in this)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				WeakRefKey weakRefKey = (WeakRefKey)dictionaryEntry.Key;
				CollectionRecord collectionRecord = (CollectionRecord)dictionaryEntry.Value;
				if (weakRefKey.Target == null || !collectionRecord.IsAlive)
				{
					arrayList.Add(weakRefKey);
				}
				else
				{
					ViewTable viewTable = collectionRecord.ViewTable;
					if (viewTable != null && viewTable.Purge())
					{
						flag = true;
						if (viewTable.Count == 0)
						{
							collectionRecord.ViewTable = null;
							if (!collectionRecord.IsAlive)
							{
								arrayList.Add(weakRefKey);
							}
						}
					}
				}
			}
			for (int j = 0; j < arrayList.Count; j++)
			{
				base.Remove(arrayList[j]);
			}
			return arrayList.Count > 0 || flag;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0015B478 File Offset: 0x0015A478
		private void ValidateViewType(CollectionView cv, Type collectionViewType)
		{
			if (collectionViewType != null)
			{
				CollectionViewProxy collectionViewProxy = cv as CollectionViewProxy;
				Type type = (collectionViewProxy == null) ? cv.GetType() : collectionViewProxy.ProxiedView.GetType();
				if (type != collectionViewType)
				{
					throw new ArgumentException(SR.Get("CollectionView_NameTypeDuplicity", new object[]
					{
						collectionViewType,
						type
					}));
				}
			}
		}

		// Token: 0x04000C68 RID: 3176
		private const int InactivityThreshold = 2;

		// Token: 0x04000C69 RID: 3177
		private HybridDictionary _inactiveViewTables = new HybridDictionary();

		// Token: 0x04000C6A RID: 3178
		private static object StaticObject = new object();

		// Token: 0x04000C6B RID: 3179
		internal static WeakReference StaticWeakRef = new WeakReference(ViewManager.StaticObject);

		// Token: 0x04000C6C RID: 3180
		internal static WeakReference NullWeakRef = new WeakReference(null);
	}
}
