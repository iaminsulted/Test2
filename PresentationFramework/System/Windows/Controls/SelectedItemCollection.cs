using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	// Token: 0x020007D2 RID: 2002
	internal class SelectedItemCollection : ObservableCollection<object>
	{
		// Token: 0x060072DC RID: 29404 RVA: 0x002E0C5D File Offset: 0x002DFC5D
		public SelectedItemCollection(Selector selector)
		{
			this._selector = selector;
			this._changer = new SelectedItemCollection.Changer(this);
		}

		// Token: 0x060072DD RID: 29405 RVA: 0x002E0C78 File Offset: 0x002DFC78
		protected override void ClearItems()
		{
			if (this._updatingSelectedItems)
			{
				using (IEnumerator<ItemsControl.ItemInfo> enumerator = ((IEnumerable<ItemsControl.ItemInfo>)this._selector._selectedItems).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemsControl.ItemInfo info = enumerator.Current;
						this._selector.SelectionChange.Unselect(info);
					}
					return;
				}
			}
			using (this.ChangeSelectedItems())
			{
				base.ClearItems();
			}
		}

		// Token: 0x060072DE RID: 29406 RVA: 0x002E0D04 File Offset: 0x002DFD04
		protected override void RemoveItem(int index)
		{
			if (this._updatingSelectedItems)
			{
				this._selector.SelectionChange.Unselect(this._selector.NewItemInfo(base[index], null, -1));
				return;
			}
			using (this.ChangeSelectedItems())
			{
				base.RemoveItem(index);
			}
		}

		// Token: 0x060072DF RID: 29407 RVA: 0x002E0D6C File Offset: 0x002DFD6C
		protected override void InsertItem(int index, object item)
		{
			if (!this._updatingSelectedItems)
			{
				using (this.ChangeSelectedItems())
				{
					base.InsertItem(index, item);
				}
				return;
			}
			if (index == base.Count)
			{
				this._selector.SelectionChange.Select(this._selector.NewItemInfo(item, null, -1), true);
				return;
			}
			throw new InvalidOperationException(SR.Get("InsertInDeferSelectionActive"));
		}

		// Token: 0x060072E0 RID: 29408 RVA: 0x002E0DE8 File Offset: 0x002DFDE8
		protected override void SetItem(int index, object item)
		{
			if (this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("SetInDeferSelectionActive"));
			}
			using (this.ChangeSelectedItems())
			{
				base.SetItem(index, item);
			}
		}

		// Token: 0x060072E1 RID: 29409 RVA: 0x002E0E38 File Offset: 0x002DFE38
		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				if (this._updatingSelectedItems)
				{
					throw new InvalidOperationException(SR.Get("MoveInDeferSelectionActive"));
				}
				using (this.ChangeSelectedItems())
				{
					base.MoveItem(oldIndex, newIndex);
				}
			}
		}

		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x060072E2 RID: 29410 RVA: 0x002E0E8C File Offset: 0x002DFE8C
		internal bool IsChanging
		{
			get
			{
				return this._changeCount > 0;
			}
		}

		// Token: 0x060072E3 RID: 29411 RVA: 0x002E0E97 File Offset: 0x002DFE97
		private IDisposable ChangeSelectedItems()
		{
			this._changeCount++;
			return this._changer;
		}

		// Token: 0x060072E4 RID: 29412 RVA: 0x002E0EB0 File Offset: 0x002DFEB0
		private void FinishChange()
		{
			int num = this._changeCount - 1;
			this._changeCount = num;
			if (num == 0)
			{
				this._selector.FinishSelectedItemsChange();
			}
		}

		// Token: 0x060072E5 RID: 29413 RVA: 0x002E0EDC File Offset: 0x002DFEDC
		internal void BeginUpdateSelectedItems()
		{
			if (this._selector.SelectionChange.IsActive || this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionActive"));
			}
			this._updatingSelectedItems = true;
			this._selector.SelectionChange.Begin();
		}

		// Token: 0x060072E6 RID: 29414 RVA: 0x002E0F2C File Offset: 0x002DFF2C
		internal void EndUpdateSelectedItems()
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._updatingSelectedItems = false;
			this._selector.SelectionChange.End();
		}

		// Token: 0x17001AA0 RID: 6816
		// (get) Token: 0x060072E7 RID: 29415 RVA: 0x002E0F7A File Offset: 0x002DFF7A
		internal bool IsUpdatingSelectedItems
		{
			get
			{
				return this._selector.SelectionChange.IsActive || this._updatingSelectedItems;
			}
		}

		// Token: 0x060072E8 RID: 29416 RVA: 0x002E0F96 File Offset: 0x002DFF96
		internal void Add(ItemsControl.ItemInfo info)
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._selector.SelectionChange.Select(info, true);
		}

		// Token: 0x060072E9 RID: 29417 RVA: 0x002E0FD5 File Offset: 0x002DFFD5
		internal void Remove(ItemsControl.ItemInfo info)
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._selector.SelectionChange.Unselect(info);
		}

		// Token: 0x040037A0 RID: 14240
		private int _changeCount;

		// Token: 0x040037A1 RID: 14241
		private SelectedItemCollection.Changer _changer;

		// Token: 0x040037A2 RID: 14242
		private Selector _selector;

		// Token: 0x040037A3 RID: 14243
		private bool _updatingSelectedItems;

		// Token: 0x02000C1C RID: 3100
		private class Changer : IDisposable
		{
			// Token: 0x06009087 RID: 36999 RVA: 0x00346C13 File Offset: 0x00345C13
			public Changer(SelectedItemCollection owner)
			{
				this._owner = owner;
			}

			// Token: 0x06009088 RID: 37000 RVA: 0x00346C22 File Offset: 0x00345C22
			public void Dispose()
			{
				this._owner.FinishChange();
			}

			// Token: 0x04004B24 RID: 19236
			private SelectedItemCollection _owner;
		}
	}
}
