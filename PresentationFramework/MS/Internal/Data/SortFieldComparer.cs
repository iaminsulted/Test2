using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000240 RID: 576
	internal class SortFieldComparer : IComparer
	{
		// Token: 0x06001641 RID: 5697 RVA: 0x00159ECC File Offset: 0x00158ECC
		internal SortFieldComparer(SortDescriptionCollection sortFields, CultureInfo culture)
		{
			this._sortFields = sortFields;
			this._fields = this.CreatePropertyInfo(this._sortFields);
			this._comparer = ((culture == null || culture == CultureInfo.InvariantCulture) ? Comparer.DefaultInvariant : ((culture == CultureInfo.CurrentCulture) ? Comparer.Default : new Comparer(culture)));
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001642 RID: 5698 RVA: 0x00159F25 File Offset: 0x00158F25
		internal IComparer BaseComparer
		{
			get
			{
				return this._comparer;
			}
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00159F30 File Offset: 0x00158F30
		public int Compare(object o1, object o2)
		{
			int num = 0;
			for (int i = 0; i < this._fields.Length; i++)
			{
				object value = this._fields[i].GetValue(o1);
				object value2 = this._fields[i].GetValue(o2);
				num = this._comparer.Compare(value, value2);
				if (this._fields[i].descending)
				{
					num = -num;
				}
				if (num != 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x00159FA0 File Offset: 0x00158FA0
		internal static void SortHelper(ArrayList al, IComparer comparer)
		{
			SortFieldComparer sortFieldComparer = comparer as SortFieldComparer;
			if (sortFieldComparer == null)
			{
				al.Sort(comparer);
				return;
			}
			int count = al.Count;
			int nFields = sortFieldComparer._fields.Length;
			SortFieldComparer.CachedValueItem[] array = new SortFieldComparer.CachedValueItem[count];
			for (int i = 0; i < count; i++)
			{
				array[i].Initialize(al[i], nFields);
			}
			Array.Sort(array, sortFieldComparer);
			for (int j = 0; j < count; j++)
			{
				al[j] = array[j].OriginalItem;
			}
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0015A028 File Offset: 0x00159028
		private SortFieldComparer.SortPropertyInfo[] CreatePropertyInfo(SortDescriptionCollection sortFields)
		{
			SortFieldComparer.SortPropertyInfo[] array = new SortFieldComparer.SortPropertyInfo[sortFields.Count];
			for (int i = 0; i < sortFields.Count; i++)
			{
				PropertyPath info;
				if (string.IsNullOrEmpty(sortFields[i].PropertyName))
				{
					info = null;
				}
				else
				{
					info = new PropertyPath(sortFields[i].PropertyName, Array.Empty<object>());
				}
				array[i].index = i;
				array[i].info = info;
				array[i].descending = (sortFields[i].Direction == ListSortDirection.Descending);
			}
			return array;
		}

		// Token: 0x04000C54 RID: 3156
		private SortFieldComparer.SortPropertyInfo[] _fields;

		// Token: 0x04000C55 RID: 3157
		private SortDescriptionCollection _sortFields;

		// Token: 0x04000C56 RID: 3158
		private Comparer _comparer;

		// Token: 0x02000A01 RID: 2561
		private struct SortPropertyInfo
		{
			// Token: 0x0600847E RID: 33918 RVA: 0x00325D3B File Offset: 0x00324D3B
			internal object GetValue(object o)
			{
				if (o is SortFieldComparer.CachedValueItem)
				{
					return this.GetValueFromCVI((SortFieldComparer.CachedValueItem)o);
				}
				return this.GetValueCore(o);
			}

			// Token: 0x0600847F RID: 33919 RVA: 0x00325D5C File Offset: 0x00324D5C
			private object GetValueFromCVI(SortFieldComparer.CachedValueItem cvi)
			{
				object obj = cvi[this.index];
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = (cvi[this.index] = this.GetValueCore(cvi.OriginalItem));
				}
				return obj;
			}

			// Token: 0x06008480 RID: 33920 RVA: 0x00325DA0 File Offset: 0x00324DA0
			private object GetValueCore(object o)
			{
				object obj;
				if (this.info == null)
				{
					obj = o;
				}
				else
				{
					using (this.info.SetContext(o))
					{
						obj = this.info.GetValue();
					}
				}
				if (obj == DependencyProperty.UnsetValue || BindingExpressionBase.IsNullValue(obj))
				{
					obj = null;
				}
				return obj;
			}

			// Token: 0x0400404E RID: 16462
			internal int index;

			// Token: 0x0400404F RID: 16463
			internal PropertyPath info;

			// Token: 0x04004050 RID: 16464
			internal bool descending;
		}

		// Token: 0x02000A02 RID: 2562
		private struct CachedValueItem
		{
			// Token: 0x17001DC2 RID: 7618
			// (get) Token: 0x06008481 RID: 33921 RVA: 0x00325E04 File Offset: 0x00324E04
			public object OriginalItem
			{
				get
				{
					return this._item;
				}
			}

			// Token: 0x06008482 RID: 33922 RVA: 0x00325E0C File Offset: 0x00324E0C
			public void Initialize(object item, int nFields)
			{
				this._item = item;
				this._values = new object[nFields];
				this._values[0] = DependencyProperty.UnsetValue;
			}

			// Token: 0x17001DC3 RID: 7619
			public object this[int index]
			{
				get
				{
					return this._values[index];
				}
				set
				{
					this._values[index] = value;
					if (++index < this._values.Length)
					{
						this._values[index] = DependencyProperty.UnsetValue;
					}
				}
			}

			// Token: 0x04004051 RID: 16465
			private object _item;

			// Token: 0x04004052 RID: 16466
			private object[] _values;
		}
	}
}
