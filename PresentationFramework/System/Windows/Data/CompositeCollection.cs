using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000459 RID: 1113
	[Localizability(LocalizationCategory.Ignore)]
	public class CompositeCollection : IList, ICollection, IEnumerable, INotifyCollectionChanged, ICollectionViewFactory, IWeakEventListener
	{
		// Token: 0x060038A3 RID: 14499 RVA: 0x001E9A8D File Offset: 0x001E8A8D
		public CompositeCollection()
		{
			this.Initialize(new ArrayList());
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x001E9AA0 File Offset: 0x001E8AA0
		public CompositeCollection(int capacity)
		{
			this.Initialize(new ArrayList(capacity));
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x001E9AB4 File Offset: 0x001E8AB4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x001E9AC1 File Offset: 0x001E8AC1
		public void CopyTo(Array array, int index)
		{
			this.InternalList.CopyTo(array, index);
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x001E9AD0 File Offset: 0x001E8AD0
		public int Add(object newItem)
		{
			CollectionContainer collectionContainer = newItem as CollectionContainer;
			if (collectionContainer != null)
			{
				this.AddCollectionContainer(collectionContainer);
			}
			int num = this.InternalList.Add(newItem);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, newItem, num);
			return num;
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x001E9B08 File Offset: 0x001E8B08
		public void Clear()
		{
			int i = 0;
			int count = this.InternalList.Count;
			while (i < count)
			{
				CollectionContainer collectionContainer = this[i] as CollectionContainer;
				if (collectionContainer != null)
				{
					this.RemoveCollectionContainer(collectionContainer);
				}
				i++;
			}
			this.InternalList.Clear();
			this.OnCollectionChanged(NotifyCollectionChangedAction.Reset);
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x001E9B56 File Offset: 0x001E8B56
		public bool Contains(object containItem)
		{
			return this.InternalList.Contains(containItem);
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x001E9B64 File Offset: 0x001E8B64
		public int IndexOf(object indexItem)
		{
			return this.InternalList.IndexOf(indexItem);
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x001E9B74 File Offset: 0x001E8B74
		public void Insert(int insertIndex, object insertItem)
		{
			CollectionContainer collectionContainer = insertItem as CollectionContainer;
			if (collectionContainer != null)
			{
				this.AddCollectionContainer(collectionContainer);
			}
			this.InternalList.Insert(insertIndex, insertItem);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, insertItem, insertIndex);
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x001E9BA8 File Offset: 0x001E8BA8
		public void Remove(object removeItem)
		{
			int num = this.InternalList.IndexOf(removeItem);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x001E9BD0 File Offset: 0x001E8BD0
		public void RemoveAt(int removeIndex)
		{
			if (0 <= removeIndex && removeIndex < this.Count)
			{
				object obj = this[removeIndex];
				CollectionContainer collectionContainer = obj as CollectionContainer;
				if (collectionContainer != null)
				{
					this.RemoveCollectionContainer(collectionContainer);
				}
				this.InternalList.RemoveAt(removeIndex);
				this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, obj, removeIndex);
				return;
			}
			throw new ArgumentOutOfRangeException("removeIndex", SR.Get("ItemCollectionRemoveArgumentOutOfRange"));
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x001E9C2D File Offset: 0x001E8C2D
		ICollectionView ICollectionViewFactory.CreateView()
		{
			return new CompositeCollectionView(this);
		}

		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060038AF RID: 14511 RVA: 0x001E9C35 File Offset: 0x001E8C35
		public int Count
		{
			get
			{
				return this.InternalList.Count;
			}
		}

		// Token: 0x17000C2B RID: 3115
		public object this[int itemIndex]
		{
			get
			{
				return this.InternalList[itemIndex];
			}
			set
			{
				object obj = this.InternalList[itemIndex];
				CollectionContainer cc;
				if ((cc = (obj as CollectionContainer)) != null)
				{
					this.RemoveCollectionContainer(cc);
				}
				if ((cc = (value as CollectionContainer)) != null)
				{
					this.AddCollectionContainer(cc);
				}
				this.InternalList[itemIndex] = value;
				this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, obj, value, itemIndex);
			}
		}

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x060038B2 RID: 14514 RVA: 0x001E9CA3 File Offset: 0x001E8CA3
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InternalList.IsSynchronized;
			}
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x060038B3 RID: 14515 RVA: 0x001E9CB0 File Offset: 0x001E8CB0
		object ICollection.SyncRoot
		{
			get
			{
				return this.InternalList.SyncRoot;
			}
		}

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060038B4 RID: 14516 RVA: 0x001E9CBD File Offset: 0x001E8CBD
		bool IList.IsFixedSize
		{
			get
			{
				return this.InternalList.IsFixedSize;
			}
		}

		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x060038B5 RID: 14517 RVA: 0x001E9CCA File Offset: 0x001E8CCA
		bool IList.IsReadOnly
		{
			get
			{
				return this.InternalList.IsReadOnly;
			}
		}

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x060038B6 RID: 14518 RVA: 0x001E9CD7 File Offset: 0x001E8CD7
		// (remove) Token: 0x060038B7 RID: 14519 RVA: 0x001E9CE0 File Offset: 0x001E8CE0
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

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x060038B8 RID: 14520 RVA: 0x001E9CEC File Offset: 0x001E8CEC
		// (remove) Token: 0x060038B9 RID: 14521 RVA: 0x001E9D24 File Offset: 0x001E8D24
		protected event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060038BA RID: 14522 RVA: 0x001E9D59 File Offset: 0x001E8D59
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x060038BC RID: 14524 RVA: 0x001E9D64 File Offset: 0x001E8D64
		// (remove) Token: 0x060038BD RID: 14525 RVA: 0x001E9D9C File Offset: 0x001E8D9C
		internal event NotifyCollectionChangedEventHandler ContainedCollectionChanged;

		// Token: 0x060038BE RID: 14526 RVA: 0x001E9DD1 File Offset: 0x001E8DD1
		private void OnContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.ContainedCollectionChanged != null)
			{
				this.ContainedCollectionChanged(sender, e);
			}
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x001E9DE8 File Offset: 0x001E8DE8
		private void Initialize(ArrayList internalList)
		{
			this._internalList = internalList;
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060038C0 RID: 14528 RVA: 0x001E9DF1 File Offset: 0x001E8DF1
		private ArrayList InternalList
		{
			get
			{
				return this._internalList;
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x001E9DF9 File Offset: 0x001E8DF9
		private void AddCollectionContainer(CollectionContainer cc)
		{
			if (this.InternalList.Contains(cc))
			{
				throw new ArgumentException(SR.Get("CollectionContainerMustBeUniqueForComposite"), "cc");
			}
			CollectionChangedEventManager.AddHandler(cc, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnContainedCollectionChanged));
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x001E9E30 File Offset: 0x001E8E30
		private void RemoveCollectionContainer(CollectionContainer cc)
		{
			CollectionChangedEventManager.RemoveHandler(cc, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnContainedCollectionChanged));
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x001E9E44 File Offset: 0x001E8E44
		private void OnCollectionChanged(NotifyCollectionChangedAction action)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
			}
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x001E9E60 File Offset: 0x001E8E60
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
			}
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x001E9E7E File Offset: 0x001E8E7E
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
			}
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x001E9EA0 File Offset: 0x001E8EA0
		internal void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			foreach (object obj in this.InternalList)
			{
				CollectionContainer collectionContainer = obj as CollectionContainer;
				if (collectionContainer != null)
				{
					collectionContainer.GetCollectionChangedSources(level + 1, format, sources);
				}
			}
		}

		// Token: 0x04001D4D RID: 7501
		private ArrayList _internalList;
	}
}
