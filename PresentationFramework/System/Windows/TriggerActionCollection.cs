using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003D9 RID: 985
	public sealed class TriggerActionCollection : IList, ICollection, IEnumerable, IList<TriggerAction>, ICollection<TriggerAction>, IEnumerable<TriggerAction>
	{
		// Token: 0x06002947 RID: 10567 RVA: 0x001995D6 File Offset: 0x001985D6
		public TriggerActionCollection()
		{
			this._rawList = new List<TriggerAction>();
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x001995E9 File Offset: 0x001985E9
		public TriggerActionCollection(int initialSize)
		{
			this._rawList = new List<TriggerAction>(initialSize);
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06002949 RID: 10569 RVA: 0x001995FD File Offset: 0x001985FD
		public int Count
		{
			get
			{
				return this._rawList.Count;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x0019960A File Offset: 0x0019860A
		public bool IsReadOnly
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x00199614 File Offset: 0x00198614
		public void Clear()
		{
			this.CheckSealed();
			for (int i = this._rawList.Count - 1; i >= 0; i--)
			{
				InheritanceContextHelper.RemoveContextFromObject(this._owner, this._rawList[i]);
			}
			this._rawList.Clear();
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x00199664 File Offset: 0x00198664
		public void RemoveAt(int index)
		{
			this.CheckSealed();
			TriggerAction oldValue = this._rawList[index];
			InheritanceContextHelper.RemoveContextFromObject(this._owner, oldValue);
			this._rawList.RemoveAt(index);
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x0019969C File Offset: 0x0019869C
		public void Add(TriggerAction value)
		{
			this.CheckSealed();
			InheritanceContextHelper.ProvideContextForObject(this._owner, value);
			this._rawList.Add(value);
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x001996BC File Offset: 0x001986BC
		public bool Contains(TriggerAction value)
		{
			return this._rawList.Contains(value);
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x001996CA File Offset: 0x001986CA
		public void CopyTo(TriggerAction[] array, int index)
		{
			this._rawList.CopyTo(array, index);
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x001996D9 File Offset: 0x001986D9
		public int IndexOf(TriggerAction value)
		{
			return this._rawList.IndexOf(value);
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x001996E7 File Offset: 0x001986E7
		public void Insert(int index, TriggerAction value)
		{
			this.CheckSealed();
			InheritanceContextHelper.ProvideContextForObject(this._owner, value);
			this._rawList.Insert(index, value);
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x00199708 File Offset: 0x00198708
		public bool Remove(TriggerAction value)
		{
			this.CheckSealed();
			InheritanceContextHelper.RemoveContextFromObject(this._owner, value);
			return this._rawList.Remove(value);
		}

		// Token: 0x170009A9 RID: 2473
		public TriggerAction this[int index]
		{
			get
			{
				return this._rawList[index];
			}
			set
			{
				this.CheckSealed();
				object obj = this._rawList[index];
				InheritanceContextHelper.RemoveContextFromObject(this.Owner, obj as DependencyObject);
				this._rawList[index] = value;
			}
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x00199776 File Offset: 0x00198776
		[CLSCompliant(false)]
		public IEnumerator<TriggerAction> GetEnumerator()
		{
			return this._rawList.GetEnumerator();
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x00199788 File Offset: 0x00198788
		int IList.Add(object value)
		{
			this.CheckSealed();
			InheritanceContextHelper.ProvideContextForObject(this._owner, value as DependencyObject);
			return ((IList)this._rawList).Add(this.VerifyIsTriggerAction(value));
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x001997B3 File Offset: 0x001987B3
		bool IList.Contains(object value)
		{
			return this._rawList.Contains(this.VerifyIsTriggerAction(value));
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x001997C7 File Offset: 0x001987C7
		int IList.IndexOf(object value)
		{
			return this._rawList.IndexOf(this.VerifyIsTriggerAction(value));
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x001997DB File Offset: 0x001987DB
		void IList.Insert(int index, object value)
		{
			this.Insert(index, this.VerifyIsTriggerAction(value));
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x0600295A RID: 10586 RVA: 0x0019960A File Offset: 0x0019860A
		bool IList.IsFixedSize
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x001997EB File Offset: 0x001987EB
		void IList.Remove(object value)
		{
			this.Remove(this.VerifyIsTriggerAction(value));
		}

		// Token: 0x170009AB RID: 2475
		object IList.this[int index]
		{
			get
			{
				return this._rawList[index];
			}
			set
			{
				this[index] = this.VerifyIsTriggerAction(value);
			}
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0019980B File Offset: 0x0019880B
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._rawList).CopyTo(array, index);
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x0600295F RID: 10591 RVA: 0x000F93D3 File Offset: 0x000F83D3
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06002960 RID: 10592 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x0019981A File Offset: 0x0019881A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this._rawList).GetEnumerator();
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x00199828 File Offset: 0x00198828
		internal void Seal(TriggerBase containingTrigger)
		{
			for (int i = 0; i < this._rawList.Count; i++)
			{
				this._rawList[i].Seal(containingTrigger);
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06002963 RID: 10595 RVA: 0x0019985D File Offset: 0x0019885D
		// (set) Token: 0x06002964 RID: 10596 RVA: 0x00199865 File Offset: 0x00198865
		internal DependencyObject Owner
		{
			get
			{
				return this._owner;
			}
			set
			{
				this._owner = value;
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0019986E File Offset: 0x0019886E
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"TriggerActionCollection"
				}));
			}
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x00199896 File Offset: 0x00198896
		private TriggerAction VerifyIsTriggerAction(object value)
		{
			TriggerAction triggerAction = value as TriggerAction;
			if (triggerAction != null)
			{
				return triggerAction;
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			throw new ArgumentException(SR.Get("MustBeTriggerAction"));
		}

		// Token: 0x040014E9 RID: 5353
		private List<TriggerAction> _rawList;

		// Token: 0x040014EA RID: 5354
		private bool _sealed;

		// Token: 0x040014EB RID: 5355
		private DependencyObject _owner;
	}
}
