using System;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	// Token: 0x0200075A RID: 1882
	internal class DataGridItemAttachedStorage
	{
		// Token: 0x06006663 RID: 26211 RVA: 0x002B18F2 File Offset: 0x002B08F2
		public void SetValue(object item, DependencyProperty property, object value)
		{
			this.EnsureItem(item)[property] = value;
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x002B1904 File Offset: 0x002B0904
		public bool TryGetValue(object item, DependencyProperty property, out object value)
		{
			value = null;
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			return this._itemStorageMap.TryGetValue(item, out dictionary) && dictionary.TryGetValue(property, out value);
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x002B1934 File Offset: 0x002B0934
		public void ClearValue(object item, DependencyProperty property)
		{
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			if (this._itemStorageMap.TryGetValue(item, out dictionary))
			{
				dictionary.Remove(property);
			}
		}

		// Token: 0x06006666 RID: 26214 RVA: 0x002B195F File Offset: 0x002B095F
		public void ClearItem(object item)
		{
			this.EnsureItemStorageMap();
			this._itemStorageMap.Remove(item);
		}

		// Token: 0x06006667 RID: 26215 RVA: 0x002B1974 File Offset: 0x002B0974
		public void Clear()
		{
			this._itemStorageMap = null;
		}

		// Token: 0x06006668 RID: 26216 RVA: 0x002B197D File Offset: 0x002B097D
		private void EnsureItemStorageMap()
		{
			if (this._itemStorageMap == null)
			{
				this._itemStorageMap = new Dictionary<object, Dictionary<DependencyProperty, object>>();
			}
		}

		// Token: 0x06006669 RID: 26217 RVA: 0x002B1994 File Offset: 0x002B0994
		private Dictionary<DependencyProperty, object> EnsureItem(object item)
		{
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			if (!this._itemStorageMap.TryGetValue(item, out dictionary))
			{
				dictionary = new Dictionary<DependencyProperty, object>();
				this._itemStorageMap[item] = dictionary;
			}
			return dictionary;
		}

		// Token: 0x040033BF RID: 13247
		private Dictionary<object, Dictionary<DependencyProperty, object>> _itemStorageMap;
	}
}
