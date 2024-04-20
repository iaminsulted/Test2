using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	// Token: 0x020007D4 RID: 2004
	public class SelectionChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x060072EE RID: 29422 RVA: 0x002E1014 File Offset: 0x002E0014
		public SelectionChangedEventArgs(RoutedEvent id, IList removedItems, IList addedItems)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (removedItems == null)
			{
				throw new ArgumentNullException("removedItems");
			}
			if (addedItems == null)
			{
				throw new ArgumentNullException("addedItems");
			}
			base.RoutedEvent = id;
			this._removedItems = new object[removedItems.Count];
			removedItems.CopyTo(this._removedItems, 0);
			this._addedItems = new object[addedItems.Count];
			addedItems.CopyTo(this._addedItems, 0);
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x002E1094 File Offset: 0x002E0094
		internal SelectionChangedEventArgs(List<ItemsControl.ItemInfo> unselectedInfos, List<ItemsControl.ItemInfo> selectedInfos)
		{
			base.RoutedEvent = Selector.SelectionChangedEvent;
			this._removedItems = new object[unselectedInfos.Count];
			for (int i = 0; i < unselectedInfos.Count; i++)
			{
				this._removedItems[i] = unselectedInfos[i].Item;
			}
			this._addedItems = new object[selectedInfos.Count];
			for (int j = 0; j < selectedInfos.Count; j++)
			{
				this._addedItems[j] = selectedInfos[j].Item;
			}
			this._removedInfos = unselectedInfos;
			this._addedInfos = selectedInfos;
		}

		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x060072F0 RID: 29424 RVA: 0x002E112C File Offset: 0x002E012C
		public IList RemovedItems
		{
			get
			{
				return this._removedItems;
			}
		}

		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x060072F1 RID: 29425 RVA: 0x002E1134 File Offset: 0x002E0134
		public IList AddedItems
		{
			get
			{
				return this._addedItems;
			}
		}

		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x060072F2 RID: 29426 RVA: 0x002E113C File Offset: 0x002E013C
		internal List<ItemsControl.ItemInfo> RemovedInfos
		{
			get
			{
				return this._removedInfos;
			}
		}

		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x060072F3 RID: 29427 RVA: 0x002E1144 File Offset: 0x002E0144
		internal List<ItemsControl.ItemInfo> AddedInfos
		{
			get
			{
				return this._addedInfos;
			}
		}

		// Token: 0x060072F4 RID: 29428 RVA: 0x002E114C File Offset: 0x002E014C
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((SelectionChangedEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x040037A4 RID: 14244
		private object[] _addedItems;

		// Token: 0x040037A5 RID: 14245
		private object[] _removedItems;

		// Token: 0x040037A6 RID: 14246
		private List<ItemsControl.ItemInfo> _addedInfos;

		// Token: 0x040037A7 RID: 14247
		private List<ItemsControl.ItemInfo> _removedInfos;
	}
}
