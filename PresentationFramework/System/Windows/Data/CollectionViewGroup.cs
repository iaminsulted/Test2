using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Data
{
	// Token: 0x02000456 RID: 1110
	public abstract class CollectionViewGroup : INotifyPropertyChanged
	{
		// Token: 0x0600384A RID: 14410 RVA: 0x001E8AF0 File Offset: 0x001E7AF0
		protected CollectionViewGroup(object name)
		{
			this._name = name;
			this._itemsRW = new ObservableCollection<object>();
			this._itemsRO = new ReadOnlyObservableCollection<object>(this._itemsRW);
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600384B RID: 14411 RVA: 0x001E8B1B File Offset: 0x001E7B1B
		public object Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x0600384C RID: 14412 RVA: 0x001E8B23 File Offset: 0x001E7B23
		public ReadOnlyObservableCollection<object> Items
		{
			get
			{
				return this._itemsRO;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600384D RID: 14413 RVA: 0x001E8B2B File Offset: 0x001E7B2B
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x0600384E RID: 14414
		public abstract bool IsBottomLevel { get; }

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x0600384F RID: 14415 RVA: 0x001E8B33 File Offset: 0x001E7B33
		// (remove) Token: 0x06003850 RID: 14416 RVA: 0x001E8B3C File Offset: 0x001E7B3C
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this.PropertyChanged += value;
			}
			remove
			{
				this.PropertyChanged -= value;
			}
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06003851 RID: 14417 RVA: 0x001E8B48 File Offset: 0x001E7B48
		// (remove) Token: 0x06003852 RID: 14418 RVA: 0x001E8B80 File Offset: 0x001E7B80
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06003853 RID: 14419 RVA: 0x001E8BB5 File Offset: 0x001E7BB5
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x001E8BCC File Offset: 0x001E7BCC
		protected ObservableCollection<object> ProtectedItems
		{
			get
			{
				return this._itemsRW;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06003855 RID: 14421 RVA: 0x001E8B2B File Offset: 0x001E7B2B
		// (set) Token: 0x06003856 RID: 14422 RVA: 0x001E8BD4 File Offset: 0x001E7BD4
		protected int ProtectedItemCount
		{
			get
			{
				return this._itemCount;
			}
			set
			{
				this._itemCount = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("ItemCount"));
			}
		}

		// Token: 0x04001D22 RID: 7458
		private object _name;

		// Token: 0x04001D23 RID: 7459
		private ObservableCollection<object> _itemsRW;

		// Token: 0x04001D24 RID: 7460
		private ReadOnlyObservableCollection<object> _itemsRO;

		// Token: 0x04001D25 RID: 7461
		private int _itemCount;
	}
}
