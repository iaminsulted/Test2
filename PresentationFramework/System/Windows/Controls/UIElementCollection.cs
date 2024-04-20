using System;
using System.Collections;
using System.Windows.Media;

namespace System.Windows.Controls
{
	// Token: 0x020007F6 RID: 2038
	public class UIElementCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x0600768D RID: 30349 RVA: 0x002EFEA4 File Offset: 0x002EEEA4
		public UIElementCollection(UIElement visualParent, FrameworkElement logicalParent)
		{
			if (visualParent == null)
			{
				throw new ArgumentNullException(SR.Get("Panel_NoNullVisualParent", new object[]
				{
					"visualParent",
					base.GetType()
				}));
			}
			this._visualChildren = new VisualCollection(visualParent);
			this._visualParent = visualParent;
			this._logicalParent = logicalParent;
		}

		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x0600768E RID: 30350 RVA: 0x002EFEFB File Offset: 0x002EEEFB
		public virtual int Count
		{
			get
			{
				return this._visualChildren.Count;
			}
		}

		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x0600768F RID: 30351 RVA: 0x002EFF08 File Offset: 0x002EEF08
		public virtual bool IsSynchronized
		{
			get
			{
				return this._visualChildren.IsSynchronized;
			}
		}

		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x06007690 RID: 30352 RVA: 0x002EFF15 File Offset: 0x002EEF15
		public virtual object SyncRoot
		{
			get
			{
				return this._visualChildren.SyncRoot;
			}
		}

		// Token: 0x06007691 RID: 30353 RVA: 0x002EFF22 File Offset: 0x002EEF22
		public virtual void CopyTo(Array array, int index)
		{
			this._visualChildren.CopyTo(array, index);
		}

		// Token: 0x06007692 RID: 30354 RVA: 0x002EFF34 File Offset: 0x002EEF34
		public virtual void CopyTo(UIElement[] array, int index)
		{
			this._visualChildren.CopyTo(array, index);
		}

		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x06007693 RID: 30355 RVA: 0x002EFF50 File Offset: 0x002EEF50
		// (set) Token: 0x06007694 RID: 30356 RVA: 0x002EFF5D File Offset: 0x002EEF5D
		public virtual int Capacity
		{
			get
			{
				return this._visualChildren.Capacity;
			}
			set
			{
				this.VerifyWriteAccess();
				this._visualChildren.Capacity = value;
			}
		}

		// Token: 0x17001B8C RID: 7052
		public virtual UIElement this[int index]
		{
			get
			{
				return this._visualChildren[index] as UIElement;
			}
			set
			{
				this.VerifyWriteAccess();
				this.ValidateElement(value);
				VisualCollection visualChildren = this._visualChildren;
				if (visualChildren[index] != value)
				{
					UIElement uielement = visualChildren[index] as UIElement;
					if (uielement != null)
					{
						this.ClearLogicalParent(uielement);
					}
					visualChildren[index] = value;
					this.SetLogicalParent(value);
					this._visualParent.InvalidateMeasure();
				}
			}
		}

		// Token: 0x06007697 RID: 30359 RVA: 0x002EFFE0 File Offset: 0x002EEFE0
		internal void SetInternal(int index, UIElement item)
		{
			this.ValidateElement(item);
			VisualCollection visualChildren = this._visualChildren;
			if (visualChildren[index] != item)
			{
				visualChildren[index] = null;
				visualChildren[index] = item;
				this._visualParent.InvalidateMeasure();
			}
		}

		// Token: 0x06007698 RID: 30360 RVA: 0x002F0020 File Offset: 0x002EF020
		public virtual int Add(UIElement element)
		{
			this.VerifyWriteAccess();
			return this.AddInternal(element);
		}

		// Token: 0x06007699 RID: 30361 RVA: 0x002F002F File Offset: 0x002EF02F
		internal int AddInternal(UIElement element)
		{
			this.ValidateElement(element);
			this.SetLogicalParent(element);
			int result = this._visualChildren.Add(element);
			this._visualParent.InvalidateMeasure();
			return result;
		}

		// Token: 0x0600769A RID: 30362 RVA: 0x002F0056 File Offset: 0x002EF056
		public virtual int IndexOf(UIElement element)
		{
			return this._visualChildren.IndexOf(element);
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x002F0064 File Offset: 0x002EF064
		public virtual void Remove(UIElement element)
		{
			this.VerifyWriteAccess();
			this.RemoveInternal(element);
		}

		// Token: 0x0600769C RID: 30364 RVA: 0x002F0073 File Offset: 0x002EF073
		internal void RemoveInternal(UIElement element)
		{
			this._visualChildren.Remove(element);
			this.ClearLogicalParent(element);
			this._visualParent.InvalidateMeasure();
		}

		// Token: 0x0600769D RID: 30365 RVA: 0x002F0093 File Offset: 0x002EF093
		internal virtual void RemoveNoVerify(UIElement element)
		{
			this._visualChildren.Remove(element);
		}

		// Token: 0x0600769E RID: 30366 RVA: 0x002F00A1 File Offset: 0x002EF0A1
		public virtual bool Contains(UIElement element)
		{
			return this._visualChildren.Contains(element);
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x002F00AF File Offset: 0x002EF0AF
		public virtual void Clear()
		{
			this.VerifyWriteAccess();
			this.ClearInternal();
		}

		// Token: 0x060076A0 RID: 30368 RVA: 0x002F00C0 File Offset: 0x002EF0C0
		internal void ClearInternal()
		{
			VisualCollection visualChildren = this._visualChildren;
			int count = visualChildren.Count;
			if (count > 0)
			{
				Visual[] array = new Visual[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = visualChildren[i];
				}
				visualChildren.Clear();
				for (int j = 0; j < count; j++)
				{
					UIElement uielement = array[j] as UIElement;
					if (uielement != null)
					{
						this.ClearLogicalParent(uielement);
					}
				}
				this._visualParent.InvalidateMeasure();
			}
		}

		// Token: 0x060076A1 RID: 30369 RVA: 0x002F0134 File Offset: 0x002EF134
		public virtual void Insert(int index, UIElement element)
		{
			this.VerifyWriteAccess();
			this.InsertInternal(index, element);
		}

		// Token: 0x060076A2 RID: 30370 RVA: 0x002F0144 File Offset: 0x002EF144
		internal void InsertInternal(int index, UIElement element)
		{
			this.ValidateElement(element);
			this.SetLogicalParent(element);
			this._visualChildren.Insert(index, element);
			this._visualParent.InvalidateMeasure();
		}

		// Token: 0x060076A3 RID: 30371 RVA: 0x002F016C File Offset: 0x002EF16C
		public virtual void RemoveAt(int index)
		{
			this.VerifyWriteAccess();
			VisualCollection visualChildren = this._visualChildren;
			UIElement uielement = visualChildren[index] as UIElement;
			visualChildren.RemoveAt(index);
			if (uielement != null)
			{
				this.ClearLogicalParent(uielement);
			}
			this._visualParent.InvalidateMeasure();
		}

		// Token: 0x060076A4 RID: 30372 RVA: 0x002F01AD File Offset: 0x002EF1AD
		public virtual void RemoveRange(int index, int count)
		{
			this.VerifyWriteAccess();
			this.RemoveRangeInternal(index, count);
		}

		// Token: 0x060076A5 RID: 30373 RVA: 0x002F01C0 File Offset: 0x002EF1C0
		internal void RemoveRangeInternal(int index, int count)
		{
			VisualCollection visualChildren = this._visualChildren;
			int count2 = visualChildren.Count;
			if (count > count2 - index)
			{
				count = count2 - index;
			}
			if (count > 0)
			{
				Visual[] array = new Visual[count];
				int i = index;
				for (int j = 0; j < count; j++)
				{
					array[j] = visualChildren[i];
					i++;
				}
				visualChildren.RemoveRange(index, count);
				for (i = 0; i < count; i++)
				{
					UIElement uielement = array[i] as UIElement;
					if (uielement != null)
					{
						this.ClearLogicalParent(uielement);
					}
				}
				this._visualParent.InvalidateMeasure();
			}
		}

		// Token: 0x060076A6 RID: 30374 RVA: 0x002F0247 File Offset: 0x002EF247
		internal void MoveVisualChild(Visual visual, Visual destination)
		{
			this._visualChildren.Move(visual, destination);
		}

		// Token: 0x060076A7 RID: 30375 RVA: 0x002F0258 File Offset: 0x002EF258
		private UIElement Cast(object value)
		{
			if (value == null)
			{
				throw new ArgumentException(SR.Get("Collection_NoNull", new object[]
				{
					"UIElementCollection"
				}));
			}
			UIElement uielement = value as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("Collection_BadType", new object[]
				{
					"UIElementCollection",
					value.GetType().Name,
					"UIElement"
				}));
			}
			return uielement;
		}

		// Token: 0x060076A8 RID: 30376 RVA: 0x002F02C5 File Offset: 0x002EF2C5
		int IList.Add(object value)
		{
			return this.Add(this.Cast(value));
		}

		// Token: 0x060076A9 RID: 30377 RVA: 0x002F02D4 File Offset: 0x002EF2D4
		bool IList.Contains(object value)
		{
			return this.Contains(value as UIElement);
		}

		// Token: 0x060076AA RID: 30378 RVA: 0x002F02E2 File Offset: 0x002EF2E2
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as UIElement);
		}

		// Token: 0x060076AB RID: 30379 RVA: 0x002F02F0 File Offset: 0x002EF2F0
		void IList.Insert(int index, object value)
		{
			this.Insert(index, this.Cast(value));
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x060076AC RID: 30380 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x060076AD RID: 30381 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060076AE RID: 30382 RVA: 0x002F0300 File Offset: 0x002EF300
		void IList.Remove(object value)
		{
			this.Remove(value as UIElement);
		}

		// Token: 0x17001B8F RID: 7055
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = this.Cast(value);
			}
		}

		// Token: 0x060076B1 RID: 30385 RVA: 0x002F0327 File Offset: 0x002EF327
		public virtual IEnumerator GetEnumerator()
		{
			return this._visualChildren.GetEnumerator();
		}

		// Token: 0x060076B2 RID: 30386 RVA: 0x002F0339 File Offset: 0x002EF339
		protected void SetLogicalParent(UIElement element)
		{
			if (this._logicalParent != null)
			{
				this._logicalParent.AddLogicalChild(element);
			}
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x002F034F File Offset: 0x002EF34F
		protected void ClearLogicalParent(UIElement element)
		{
			if (this._logicalParent != null)
			{
				this._logicalParent.RemoveLogicalChild(element);
			}
		}

		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x060076B4 RID: 30388 RVA: 0x002F0365 File Offset: 0x002EF365
		internal UIElement VisualParent
		{
			get
			{
				return this._visualParent;
			}
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x002F036D File Offset: 0x002EF36D
		private void ValidateElement(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException(SR.Get("Panel_NoNullChildren", new object[]
				{
					base.GetType()
				}));
			}
		}

		// Token: 0x060076B6 RID: 30390 RVA: 0x002F0394 File Offset: 0x002EF394
		private void VerifyWriteAccess()
		{
			Panel panel = this._visualParent as Panel;
			if (panel != null && panel.IsDataBound)
			{
				throw new InvalidOperationException(SR.Get("Panel_BoundPanel_NoChildren"));
			}
		}

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x060076B7 RID: 30391 RVA: 0x002F03C8 File Offset: 0x002EF3C8
		internal FrameworkElement LogicalParent
		{
			get
			{
				return this._logicalParent;
			}
		}

		// Token: 0x0400389D RID: 14493
		private readonly VisualCollection _visualChildren;

		// Token: 0x0400389E RID: 14494
		private readonly UIElement _visualParent;

		// Token: 0x0400389F RID: 14495
		private readonly FrameworkElement _logicalParent;
	}
}
