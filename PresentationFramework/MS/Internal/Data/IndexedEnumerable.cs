using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000228 RID: 552
	internal class IndexedEnumerable : IEnumerable, IWeakEventListener
	{
		// Token: 0x060014BA RID: 5306 RVA: 0x00152DAE File Offset: 0x00151DAE
		internal IndexedEnumerable(IEnumerable collection) : this(collection, null)
		{
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00152DB8 File Offset: 0x00151DB8
		internal IndexedEnumerable(IEnumerable collection, Predicate<object> filterCallback)
		{
			this._filterCallback = filterCallback;
			this.SetCollection(collection);
			if (this.List == null)
			{
				INotifyCollectionChanged notifyCollectionChanged = collection as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					CollectionChangedEventManager.AddHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				}
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00152E14 File Offset: 0x00151E14
		internal int IndexOf(object item)
		{
			int num;
			if (this.GetNativeIndexOf(item, out num))
			{
				return num;
			}
			if (this.EnsureCacheCurrent() && item == this._cachedItem)
			{
				return this._cachedIndex;
			}
			num = -1;
			if (this._cachedIndex >= 0)
			{
				this.UseNewEnumerator();
			}
			int num2 = 0;
			while (this._enumerator.MoveNext())
			{
				if (object.Equals(this._enumerator.Current, item))
				{
					num = num2;
					break;
				}
				num2++;
			}
			if (num >= 0)
			{
				this.CacheCurrentItem(num, this._enumerator.Current);
			}
			else
			{
				this.ClearAllCaches();
				this.DisposeEnumerator(ref this._enumerator);
			}
			return num;
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x00152EB0 File Offset: 0x00151EB0
		internal int Count
		{
			get
			{
				this.EnsureCacheCurrent();
				int num = 0;
				if (this.GetNativeCount(out num))
				{
					return num;
				}
				if (this._cachedCount >= 0)
				{
					return this._cachedCount;
				}
				num = 0;
				foreach (object obj in this)
				{
					num++;
				}
				this._cachedCount = num;
				this._cachedIsEmpty = new bool?(this._cachedCount == 0);
				return num;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x00152F40 File Offset: 0x00151F40
		internal bool IsEmpty
		{
			get
			{
				bool result;
				if (this.GetNativeIsEmpty(out result))
				{
					return result;
				}
				if (this._cachedIsEmpty != null)
				{
					return this._cachedIsEmpty.Value;
				}
				IEnumerator enumerator = this.GetEnumerator();
				this._cachedIsEmpty = new bool?(!enumerator.MoveNext());
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				if (this._cachedIsEmpty.Value)
				{
					this._cachedCount = 0;
				}
				return this._cachedIsEmpty.Value;
			}
		}

		// Token: 0x170003F9 RID: 1017
		internal object this[int index]
		{
			get
			{
				object result;
				if (this.GetNativeItemAt(index, out result))
				{
					return result;
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				int num = index - this._cachedIndex;
				if (num < 0)
				{
					this.UseNewEnumerator();
					num = index + 1;
				}
				if (this.EnsureCacheCurrent())
				{
					if (index == this._cachedIndex)
					{
						return this._cachedItem;
					}
				}
				else
				{
					num = index + 1;
				}
				while (num > 0 && this._enumerator.MoveNext())
				{
					num--;
				}
				if (num != 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this.CacheCurrentItem(index, this._enumerator.Current);
				return this._cachedItem;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x00153055 File Offset: 0x00152055
		internal IEnumerable Enumerable
		{
			get
			{
				return this._enumerable;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0015305D File Offset: 0x0015205D
		internal ICollection Collection
		{
			get
			{
				return this._collection;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x00153065 File Offset: 0x00152065
		internal IList List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060014C3 RID: 5315 RVA: 0x0015306D File Offset: 0x0015206D
		internal CollectionView CollectionView
		{
			get
			{
				return this._collectionView;
			}
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00153075 File Offset: 0x00152075
		public IEnumerator GetEnumerator()
		{
			return new IndexedEnumerable.FilteredEnumerator(this, this.Enumerable, this.FilterCallback);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0015308C File Offset: 0x0015208C
		internal static void CopyTo(IEnumerable collection, Array array, int index)
		{
			Invariant.Assert(collection != null, "collection is null");
			Invariant.Assert(array != null, "target array is null");
			Invariant.Assert(array.Rank == 1, "expected array of rank=1");
			Invariant.Assert(index >= 0, "index must be positive");
			ICollection collection2 = collection as ICollection;
			if (collection2 != null)
			{
				collection2.CopyTo(array, index);
				return;
			}
			foreach (object value in collection)
			{
				if (index >= array.Length)
				{
					throw new ArgumentException(SR.Get("CopyToNotEnoughSpace"), "index");
				}
				((IList)array)[index] = value;
				index++;
			}
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0015315C File Offset: 0x0015215C
		internal void Invalidate()
		{
			this.ClearAllCaches();
			if (this.List == null)
			{
				INotifyCollectionChanged notifyCollectionChanged = this.Enumerable as INotifyCollectionChanged;
				if (notifyCollectionChanged != null)
				{
					CollectionChangedEventManager.RemoveHandler(notifyCollectionChanged, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnCollectionChanged));
				}
			}
			this._enumerable = null;
			this.DisposeEnumerator(ref this._enumerator);
			this.DisposeEnumerator(ref this._changeTracker);
			this._collection = null;
			this._list = null;
			this._filterCallback = null;
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060014C7 RID: 5319 RVA: 0x001531CC File Offset: 0x001521CC
		private Predicate<object> FilterCallback
		{
			get
			{
				return this._filterCallback;
			}
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x001531D4 File Offset: 0x001521D4
		private void CacheCurrentItem(int index, object item)
		{
			this._cachedIndex = index;
			this._cachedItem = item;
			this._cachedVersion = this._enumeratorVersion;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x001531F0 File Offset: 0x001521F0
		private bool EnsureCacheCurrent()
		{
			int num = this.EnsureEnumerator();
			if (num != this._cachedVersion)
			{
				this.ClearAllCaches();
				this._cachedVersion = num;
			}
			return num == this._cachedVersion && this._cachedIndex >= 0;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00153234 File Offset: 0x00152234
		private int EnsureEnumerator()
		{
			if (this._enumerator == null)
			{
				this.UseNewEnumerator();
			}
			else
			{
				try
				{
					this._changeTracker.MoveNext();
				}
				catch (InvalidOperationException)
				{
					this.UseNewEnumerator();
				}
			}
			return this._enumeratorVersion;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00153280 File Offset: 0x00152280
		private void UseNewEnumerator()
		{
			this._enumeratorVersion++;
			this.DisposeEnumerator(ref this._changeTracker);
			this._changeTracker = this._enumerable.GetEnumerator();
			this.DisposeEnumerator(ref this._enumerator);
			this._enumerator = this.GetEnumerator();
			this._cachedIndex = -1;
			this._cachedItem = null;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x001532DE File Offset: 0x001522DE
		private void InvalidateEnumerator()
		{
			this._enumeratorVersion++;
			this.DisposeEnumerator(ref this._enumerator);
			this.ClearAllCaches();
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00153300 File Offset: 0x00152300
		private void DisposeEnumerator(ref IEnumerator ie)
		{
			IDisposable disposable = ie as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			ie = null;
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00153321 File Offset: 0x00152321
		private void ClearAllCaches()
		{
			this._cachedItem = null;
			this._cachedIndex = -1;
			this._cachedCount = -1;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00153338 File Offset: 0x00152338
		private void SetCollection(IEnumerable collection)
		{
			Invariant.Assert(collection != null);
			this._enumerable = collection;
			this._collection = (collection as ICollection);
			this._list = (collection as IList);
			this._collectionView = (collection as CollectionView);
			if (this.List == null && this.CollectionView == null)
			{
				Type type = collection.GetType();
				MethodInfo method = type.GetMethod("IndexOf", new Type[]
				{
					typeof(object)
				});
				if (method != null && method.ReturnType == typeof(int))
				{
					this._reflectedIndexOf = method;
				}
				MemberInfo[] defaultMembers = type.GetDefaultMembers();
				for (int i = 0; i <= defaultMembers.Length - 1; i++)
				{
					PropertyInfo propertyInfo = defaultMembers[i] as PropertyInfo;
					if (propertyInfo != null)
					{
						ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
						if (indexParameters.Length == 1 && indexParameters[0].ParameterType.IsAssignableFrom(typeof(int)))
						{
							this._reflectedItemAt = propertyInfo;
							break;
						}
					}
				}
				if (this.Collection == null)
				{
					PropertyInfo property = type.GetProperty("Count", typeof(int));
					if (property != null)
					{
						this._reflectedCount = property;
					}
				}
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0015346C File Offset: 0x0015246C
		private bool GetNativeCount(out int value)
		{
			bool result = false;
			value = -1;
			if (this.Collection != null)
			{
				value = this.Collection.Count;
				result = true;
			}
			else if (this.CollectionView != null)
			{
				value = this.CollectionView.Count;
				result = true;
			}
			else if (this._reflectedCount != null)
			{
				try
				{
					value = (int)this._reflectedCount.GetValue(this.Enumerable, null);
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedCount = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x001534F8 File Offset: 0x001524F8
		private bool GetNativeIsEmpty(out bool isEmpty)
		{
			bool result = false;
			isEmpty = true;
			if (this.Collection != null)
			{
				isEmpty = (this.Collection.Count == 0);
				result = true;
			}
			else if (this.CollectionView != null)
			{
				isEmpty = this.CollectionView.IsEmpty;
				result = true;
			}
			else if (this._reflectedCount != null)
			{
				try
				{
					isEmpty = ((int)this._reflectedCount.GetValue(this.Enumerable, null) == 0);
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedCount = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0015358C File Offset: 0x0015258C
		private bool GetNativeIndexOf(object item, out int value)
		{
			bool result = false;
			value = -1;
			if (this.List != null && this.FilterCallback == null)
			{
				value = this.List.IndexOf(item);
				result = true;
			}
			else if (this.CollectionView != null)
			{
				value = this.CollectionView.IndexOf(item);
				result = true;
			}
			else if (this._reflectedIndexOf != null)
			{
				try
				{
					value = (int)this._reflectedIndexOf.Invoke(this.Enumerable, new object[]
					{
						item
					});
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedIndexOf = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0015362C File Offset: 0x0015262C
		private bool GetNativeItemAt(int index, out object value)
		{
			bool result = false;
			value = null;
			if (this.List != null)
			{
				value = this.List[index];
				result = true;
			}
			else if (this.CollectionView != null)
			{
				value = this.CollectionView.GetItemAt(index);
				result = true;
			}
			else if (this._reflectedItemAt != null)
			{
				try
				{
					value = this._reflectedItemAt.GetValue(this.Enumerable, new object[]
					{
						index
					});
					result = true;
				}
				catch (MethodAccessException)
				{
					this._reflectedItemAt = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x001536C4 File Offset: 0x001526C4
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x001536CF File Offset: 0x001526CF
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.InvalidateEnumerator();
		}

		// Token: 0x04000BE0 RID: 3040
		private IEnumerable _enumerable;

		// Token: 0x04000BE1 RID: 3041
		private IEnumerator _enumerator;

		// Token: 0x04000BE2 RID: 3042
		private IEnumerator _changeTracker;

		// Token: 0x04000BE3 RID: 3043
		private ICollection _collection;

		// Token: 0x04000BE4 RID: 3044
		private IList _list;

		// Token: 0x04000BE5 RID: 3045
		private CollectionView _collectionView;

		// Token: 0x04000BE6 RID: 3046
		private int _enumeratorVersion;

		// Token: 0x04000BE7 RID: 3047
		private object _cachedItem;

		// Token: 0x04000BE8 RID: 3048
		private int _cachedIndex = -1;

		// Token: 0x04000BE9 RID: 3049
		private int _cachedVersion = -1;

		// Token: 0x04000BEA RID: 3050
		private int _cachedCount = -1;

		// Token: 0x04000BEB RID: 3051
		private bool? _cachedIsEmpty;

		// Token: 0x04000BEC RID: 3052
		private PropertyInfo _reflectedCount;

		// Token: 0x04000BED RID: 3053
		private PropertyInfo _reflectedItemAt;

		// Token: 0x04000BEE RID: 3054
		private MethodInfo _reflectedIndexOf;

		// Token: 0x04000BEF RID: 3055
		private Predicate<object> _filterCallback;

		// Token: 0x020009F2 RID: 2546
		private class FilteredEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x06008457 RID: 33879 RVA: 0x0032580C File Offset: 0x0032480C
			public FilteredEnumerator(IndexedEnumerable indexedEnumerable, IEnumerable enumerable, Predicate<object> filterCallback)
			{
				this._enumerable = enumerable;
				this._enumerator = this._enumerable.GetEnumerator();
				this._filterCallback = filterCallback;
				this._indexedEnumerable = indexedEnumerable;
			}

			// Token: 0x06008458 RID: 33880 RVA: 0x0032583A File Offset: 0x0032483A
			void IEnumerator.Reset()
			{
				if (this._indexedEnumerable._enumerable == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				this.Dispose();
				this._enumerator = this._enumerable.GetEnumerator();
			}

			// Token: 0x06008459 RID: 33881 RVA: 0x00325870 File Offset: 0x00324870
			bool IEnumerator.MoveNext()
			{
				if (this._indexedEnumerable._enumerable == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				bool result;
				if (this._filterCallback == null)
				{
					result = this._enumerator.MoveNext();
				}
				else
				{
					while ((result = this._enumerator.MoveNext()) && !this._filterCallback(this._enumerator.Current))
					{
					}
				}
				return result;
			}

			// Token: 0x17001DBB RID: 7611
			// (get) Token: 0x0600845A RID: 33882 RVA: 0x003258D8 File Offset: 0x003248D8
			object IEnumerator.Current
			{
				get
				{
					return this._enumerator.Current;
				}
			}

			// Token: 0x0600845B RID: 33883 RVA: 0x003258E8 File Offset: 0x003248E8
			public void Dispose()
			{
				IDisposable disposable = this._enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				this._enumerator = null;
			}

			// Token: 0x0400401D RID: 16413
			private IEnumerable _enumerable;

			// Token: 0x0400401E RID: 16414
			private IEnumerator _enumerator;

			// Token: 0x0400401F RID: 16415
			private IndexedEnumerable _indexedEnumerable;

			// Token: 0x04004020 RID: 16416
			private Predicate<object> _filterCallback;
		}
	}
}
