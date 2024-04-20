using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Annotations
{
	// Token: 0x020002B9 RID: 697
	internal class AnnotationObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged2, IOwnedObject
	{
		// Token: 0x06001A15 RID: 6677 RVA: 0x00162FD8 File Offset: 0x00161FD8
		public AnnotationObservableCollection()
		{
			this._listener = new PropertyChangedEventHandler(this.OnItemPropertyChanged);
		}

		// Token: 0x06001A16 RID: 6678 RVA: 0x00163009 File Offset: 0x00162009
		public AnnotationObservableCollection(List<T> list) : base(list)
		{
			this._listener = new PropertyChangedEventHandler(this.OnItemPropertyChanged);
		}

		// Token: 0x06001A17 RID: 6679 RVA: 0x0016303C File Offset: 0x0016203C
		protected override void ClearItems()
		{
			foreach (!0 ! in this)
			{
				INotifyPropertyChanged2 item = !;
				this.SetOwned(item, false);
			}
			this.ProtectedClearItems();
		}

		// Token: 0x06001A18 RID: 6680 RVA: 0x00163090 File Offset: 0x00162090
		protected override void RemoveItem(int index)
		{
			T t = base[index];
			this.SetOwned(t, false);
			base.RemoveItem(index);
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x001630B9 File Offset: 0x001620B9
		protected override void InsertItem(int index, T item)
		{
			if (this.ItemOwned(item))
			{
				throw new ArgumentException(SR.Get("AlreadyHasParent"));
			}
			base.InsertItem(index, item);
			this.SetOwned(item, true);
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x001630F0 File Offset: 0x001620F0
		protected override void SetItem(int index, T item)
		{
			if (this.ItemOwned(item))
			{
				throw new ArgumentException(SR.Get("AlreadyHasParent"));
			}
			T t = base[index];
			this.SetOwned(t, false);
			this.ProtectedSetItem(index, item);
			this.SetOwned(item, true);
		}

		// Token: 0x06001A1B RID: 6683 RVA: 0x00163145 File Offset: 0x00162145
		protected virtual void ProtectedClearItems()
		{
			base.ClearItems();
		}

		// Token: 0x06001A1C RID: 6684 RVA: 0x0016314D File Offset: 0x0016214D
		protected virtual void ProtectedSetItem(int index, T item)
		{
			base.Items[index] = item;
			this.OnPropertyChanged(new PropertyChangedEventArgs(this.CountString));
			this.OnPropertyChanged(new PropertyChangedEventArgs(this.IndexerName));
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x0016318A File Offset: 0x0016218A
		protected void ObservableCollectionSetItem(int index, T item)
		{
			base.SetItem(index, item);
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00163194 File Offset: 0x00162194
		protected virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x001631A2 File Offset: 0x001621A2
		private bool ItemOwned(object item)
		{
			return item != null && (item as IOwnedObject).Owned;
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x001631B4 File Offset: 0x001621B4
		private void SetOwned(object item, bool owned)
		{
			if (item != null)
			{
				(item as IOwnedObject).Owned = owned;
				if (owned)
				{
					((INotifyPropertyChanged2)item).PropertyChanged += this._listener;
					return;
				}
				((INotifyPropertyChanged2)item).PropertyChanged -= this._listener;
			}
		}

		// Token: 0x04000D9A RID: 3482
		private readonly PropertyChangedEventHandler _listener;

		// Token: 0x04000D9B RID: 3483
		internal readonly string CountString = "Count";

		// Token: 0x04000D9C RID: 3484
		internal readonly string IndexerName = "Item[]";
	}
}
