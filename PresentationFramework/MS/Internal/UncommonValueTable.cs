using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x02000101 RID: 257
	internal struct UncommonValueTable
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x00105B30 File Offset: 0x00104B30
		public bool HasValue(int id)
		{
			return (this._bitmask & 1U << id) > 0U;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00105B42 File Offset: 0x00104B42
		public object GetValue(int id)
		{
			return this.GetValue(id, DependencyProperty.UnsetValue);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00105B50 File Offset: 0x00104B50
		public object GetValue(int id, object defaultValue)
		{
			int num = this.IndexOf(id);
			if (num >= 0)
			{
				return this._table[num];
			}
			return defaultValue;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00105B74 File Offset: 0x00104B74
		public void SetValue(int id, object value)
		{
			int num = this.Find(id);
			if (num < 0)
			{
				if (this._table == null)
				{
					this._table = new object[1];
					num = 0;
				}
				else
				{
					int num2 = this._table.Length;
					object[] array = new object[num2 + 1];
					num = ~num;
					Array.Copy(this._table, 0, array, 0, num);
					Array.Copy(this._table, num, array, num + 1, num2 - num);
					this._table = array;
				}
				this._bitmask |= 1U << id;
			}
			this._table[num] = value;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00105C00 File Offset: 0x00104C00
		public void ClearValue(int id)
		{
			int num = this.Find(id);
			if (num >= 0)
			{
				int num2 = this._table.Length - 1;
				if (num2 == 0)
				{
					this._table = null;
				}
				else
				{
					object[] array = new object[num2];
					Array.Copy(this._table, 0, array, 0, num);
					Array.Copy(this._table, num + 1, array, num, num2 - num);
					this._table = array;
				}
				this._bitmask &= ~(1U << id);
			}
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00105C74 File Offset: 0x00104C74
		private int IndexOf(int id)
		{
			if (!this.HasValue(id))
			{
				return -1;
			}
			return this.GetIndex(id);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00105C88 File Offset: 0x00104C88
		private int Find(int id)
		{
			int num = this.GetIndex(id);
			if (!this.HasValue(id))
			{
				num = ~num;
			}
			return num;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00105CAC File Offset: 0x00104CAC
		private int GetIndex(int id)
		{
			uint num = this._bitmask << 31 - id << 1;
			num -= (num >> 1 & 1431655765U);
			num = (num & 858993459U) + (num >> 2 & 858993459U);
			num = (num + (num >> 4) & 252645135U);
			return (int)(num * 16843009U >> 24);
		}

		// Token: 0x040006F0 RID: 1776
		private object[] _table;

		// Token: 0x040006F1 RID: 1777
		private uint _bitmask;
	}
}
