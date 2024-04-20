using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020001EF RID: 495
	internal class TableColumnCollectionInternal : ContentElementCollection<Table, TableColumn>
	{
		// Token: 0x06001195 RID: 4501 RVA: 0x001445FD File Offset: 0x001435FD
		internal TableColumnCollectionInternal(Table owner) : base(owner)
		{
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00144608 File Offset: 0x00143608
		public override void Add(TableColumn item)
		{
			int num = base.Version;
			base.Version = num + 1;
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (base.Size == base.Items.Length)
			{
				base.EnsureCapacity(base.Size + 1);
			}
			num = base.Size;
			base.Size = num + 1;
			int index = num;
			this.PrivateConnectChild(index, item);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0014466C File Offset: 0x0014366C
		public override void Clear()
		{
			int version = base.Version;
			base.Version = version + 1;
			for (int i = 0; i < base.Size; i++)
			{
				this.PrivateDisconnectChild(base.Items[i]);
				base.Items[i] = null;
			}
			base.Size = 0;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x001446B8 File Offset: 0x001436B8
		public override void Insert(int index, TableColumn item)
		{
			int num = base.Version;
			base.Version = num + 1;
			if (index < 0 || index > base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (base.Size == base.Items.Length)
			{
				base.EnsureCapacity(base.Size + 1);
			}
			for (int i = base.Size - 1; i >= index; i--)
			{
				base.Items[i + 1] = base.Items[i];
				base.Items[i].Index = i + 1;
			}
			base.Items[index] = null;
			num = base.Size;
			base.Size = num + 1;
			this.PrivateConnectChild(index, item);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00144774 File Offset: 0x00143774
		internal override void PrivateConnectChild(int index, TableColumn item)
		{
			if (item.Parent is ContentElementCollection<Table, TableColumn>.DummyProxy)
			{
				if (LogicalTreeHelper.GetParent(item.Parent) != base.Owner)
				{
					throw new ArgumentException(SR.Get("TableCollectionWrongProxyParent"));
				}
			}
			else
			{
				if (item.Parent != null)
				{
					throw new ArgumentException(SR.Get("TableCollectionInOtherCollection"));
				}
				base.Owner.AddLogicalChild(item);
			}
			base.Items[index] = item;
			item.Index = index;
			item.OnEnterParentTree();
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x001447EB File Offset: 0x001437EB
		internal override void PrivateDisconnectChild(TableColumn item)
		{
			item.OnExitParentTree();
			base.Items[item.Index] = null;
			item.Index = -1;
			if (!(item.Parent is ContentElementCollection<Table, TableColumn>.DummyProxy))
			{
				base.Owner.RemoveLogicalChild(item);
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00144824 File Offset: 0x00143824
		public override bool Remove(TableColumn item)
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
			base.PrivateRemove(item);
			return true;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00144864 File Offset: 0x00143864
		public override void RemoveAt(int index)
		{
			int version = base.Version;
			base.Version = version + 1;
			if (index < 0 || index >= base.Size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			base.PrivateRemove(base.Items[index]);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x001448AC File Offset: 0x001438AC
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
					this.PrivateDisconnectChild(base.Items[i]);
				}
				base.Size -= count;
				for (int j = index; j < base.Size; j++)
				{
					base.Items[j] = base.Items[j + count];
					base.Items[j].Index = j;
					base.Items[j + count] = null;
				}
			}
		}

		// Token: 0x1700033E RID: 830
		public override TableColumn this[int index]
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
				this.PrivateDisconnectChild(base.Items[index]);
				this.PrivateConnectChild(index, value);
			}
		}
	}
}
