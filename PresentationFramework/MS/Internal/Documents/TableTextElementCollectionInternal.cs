using System;
using System.Collections;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001F0 RID: 496
	internal class TableTextElementCollectionInternal<TParent, TElementType> : ContentElementCollection<TParent, TElementType> where TParent : TextElement, IAcceptInsertion where TElementType : TextElement, IIndexedChild<TParent>
	{
		// Token: 0x060011A0 RID: 4512 RVA: 0x001449F6 File Offset: 0x001439F6
		internal TableTextElementCollectionInternal(TParent owner) : base(owner)
		{
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00144A00 File Offset: 0x00143A00
		public override void Add(TElementType item)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Parent != null)
			{
				throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
			}
			base.Owner.InsertionIndex = base.Size;
			item.RepositionWithContent(base.Owner.ContentEnd);
			base.Owner.InsertionIndex = -1;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00144A90 File Offset: 0x00143A90
		public override void Clear()
		{
			int version = base.Version;
			base.Version = version + 1;
			for (int i = base.Size - 1; i >= 0; i--)
			{
				this.Remove(base.Items[i]);
			}
			base.Size = 0;
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00144ADC File Offset: 0x00143ADC
		public override void Insert(int index, TElementType item)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index > base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Parent != null)
			{
				throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
			}
			base.Owner.InsertionIndex = index;
			if (index == base.Size)
			{
				item.RepositionWithContent(base.Owner.ContentEnd);
			}
			else
			{
				TextPointer textPosition = new TextPointer(base.Items[index].ContentStart, -1);
				item.RepositionWithContent(textPosition);
			}
			base.Owner.InsertionIndex = -1;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00144BB8 File Offset: 0x00143BB8
		public override bool Remove(TElementType item)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (!base.BelongsToOwner(item))
			{
				return false;
			}
			TextPointer startPosition = new TextPointer(item.TextContainer, item.TextElementNode, ElementEdge.BeforeStart, LogicalDirection.Backward);
			TextPointer endPosition = new TextPointer(item.TextContainer, item.TextElementNode, ElementEdge.AfterEnd, LogicalDirection.Backward);
			base.Owner.TextContainer.BeginChange();
			try
			{
				base.Owner.TextContainer.DeleteContentInternal(startPosition, endPosition);
			}
			finally
			{
				base.Owner.TextContainer.EndChange();
			}
			return true;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00144C84 File Offset: 0x00143C84
		public override void RemoveAt(int index)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index >= base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.Remove(base.Items[index]);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00144CD4 File Offset: 0x00143CD4
		public override void RemoveRange(int index, int count)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index >= base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionCountNeedNonNegNum"));
			}
			if (base.Size - index < count)
			{
				throw new ArgumentException(SR.Get("TableCollectionRangeOutOfRange"));
			}
			if (count > 0)
			{
				for (int i = index + count - 1; i >= index; i--)
				{
					this.Remove(base.Items[i]);
				}
			}
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00144D64 File Offset: 0x00143D64
		internal override void PrivateConnectChild(int index, TElementType item)
		{
			if (item.Parent is ContentElementCollection<TParent, TElementType>.DummyProxy && LogicalTreeHelper.GetParent(item.Parent) != base.Owner)
			{
				throw new ArgumentException(SR.Get("TableCollectionWrongProxyParent"));
			}
			base.Items[index] = item;
			item.Index = index;
			item.OnEnterParentTree();
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00144DD4 File Offset: 0x00143DD4
		internal override void PrivateDisconnectChild(TElementType item)
		{
			int index = item.Index;
			item.OnExitParentTree();
			base.Items[item.Index] = default(TElementType);
			item.Index = -1;
			int size = base.Size - 1;
			base.Size = size;
			for (int i = index; i < base.Size; i++)
			{
				base.Items[i] = base.Items[i + 1];
				base.Items[i].Index = i;
			}
			base.Items[base.Size] = default(TElementType);
			item.OnAfterExitParentTree(base.Owner);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00144EA0 File Offset: 0x00143EA0
		internal int FindInsertionIndex(TElementType item)
		{
			int num = 0;
			object obj = item;
			if (item.Parent is ContentElementCollection<TParent, TElementType>.DummyProxy)
			{
				obj = item.Parent;
			}
			IEnumerator enumerator = base.Owner.IsEmpty ? new RangeContentEnumerator(null, null) : new RangeContentEnumerator(base.Owner.ContentStart, base.Owner.ContentEnd);
			while (enumerator.MoveNext())
			{
				if (obj == enumerator.Current)
				{
					return num;
				}
				if (enumerator.Current is TElementType || enumerator.Current is ContentElementCollection<TParent, TElementType>.DummyProxy)
				{
					num++;
				}
			}
			Invariant.Assert(false);
			return -1;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00144F50 File Offset: 0x00143F50
		internal void InternalAdd(TElementType item)
		{
			if (base.Size == base.Items.Length)
			{
				base.EnsureCapacity(base.Size + 1);
			}
			int num = base.Owner.InsertionIndex;
			if (num == -1)
			{
				num = this.FindInsertionIndex(item);
			}
			for (int i = base.Size - 1; i >= num; i--)
			{
				base.Items[i + 1] = base.Items[i];
				base.Items[i].Index = i + 1;
			}
			base.Items[num] = default(TElementType);
			int size = base.Size;
			base.Size = size + 1;
			this.PrivateConnectChild(num, item);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0014500B File Offset: 0x0014400B
		internal void InternalRemove(TElementType item)
		{
			this.PrivateDisconnectChild(item);
		}

		// Token: 0x1700033F RID: 831
		public override TElementType this[int index]
		{
			get
			{
				if (index < 0 || index >= base.Size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return base.Items[index];
			}
			set
			{
				if (index < 0 || index >= base.Size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Parent != null)
				{
					throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
				}
				this.RemoveAt(index);
				this.Insert(index, value);
			}
		}
	}
}
