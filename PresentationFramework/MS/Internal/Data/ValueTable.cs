using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000244 RID: 580
	internal sealed class ValueTable : IWeakEventListener
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x0015A6EB File Offset: 0x001596EB
		internal static bool ShouldCache(object item, PropertyDescriptor pd)
		{
			return SystemDataHelper.IsDataSetCollectionProperty(pd) || SystemXmlLinqHelper.IsXLinqCollectionProperty(pd);
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0015A704 File Offset: 0x00159704
		internal object GetValue(object item, PropertyDescriptor pd, bool indexerIsNext)
		{
			if (!ValueTable.ShouldCache(item, pd))
			{
				return pd.GetValue(item);
			}
			if (this._table == null)
			{
				this._table = new HybridDictionary();
			}
			bool isXLinqCollectionProperty = SystemXmlLinqHelper.IsXLinqCollectionProperty(pd);
			ValueTable.ValueTableKey key = new ValueTable.ValueTableKey(item, pd);
			object value = this._table[key];
			Action action = delegate()
			{
				if (value == null)
				{
					if (SystemDataHelper.IsDataSetCollectionProperty(pd))
					{
						value = SystemDataHelper.GetValue(item, pd, !FrameworkAppContextSwitches.DoNotUseFollowParentWhenBindingToADODataRelation);
					}
					else if (isXLinqCollectionProperty)
					{
						value = new XDeferredAxisSource(item, pd);
					}
					else
					{
						value = pd.GetValue(item);
					}
					if (value == null)
					{
						value = ValueTable.CachedNull;
					}
					if (SystemDataHelper.IsDataSetCollectionProperty(pd))
					{
						value = new WeakReference(value);
					}
					this._table[key] = value;
				}
				if (SystemDataHelper.IsDataSetCollectionProperty(pd))
				{
					WeakReference weakReference = value as WeakReference;
					if (weakReference != null)
					{
						value = weakReference.Target;
					}
				}
			};
			action();
			if (value == null)
			{
				action();
			}
			if (value == ValueTable.CachedNull)
			{
				value = null;
			}
			else if (isXLinqCollectionProperty && !indexerIsNext)
			{
				XDeferredAxisSource xdeferredAxisSource = (XDeferredAxisSource)value;
				value = xdeferredAxisSource.FullCollection;
			}
			return value;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0015A804 File Offset: 0x00159804
		internal void RegisterForChanges(object item, PropertyDescriptor pd, DataBindEngine engine)
		{
			if (this._table == null)
			{
				this._table = new HybridDictionary();
			}
			ValueTable.ValueTableKey key = new ValueTable.ValueTableKey(item, pd);
			if (this._table[key] == null)
			{
				INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
				if (notifyPropertyChanged != null)
				{
					PropertyChangedEventManager.AddHandler(notifyPropertyChanged, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), pd.Name);
					return;
				}
				ValueChangedEventManager.AddHandler(item, new EventHandler<ValueChangedEventArgs>(this.OnValueChanged), pd);
			}
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0015A870 File Offset: 0x00159870
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string text = e.PropertyName;
			if (text == null)
			{
				text = string.Empty;
			}
			this.InvalidateCache(sender, text);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0015A895 File Offset: 0x00159895
		private void OnValueChanged(object sender, ValueChangedEventArgs e)
		{
			this.InvalidateCache(sender, e.PropertyDescriptor);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0015A8A4 File Offset: 0x001598A4
		private void InvalidateCache(object item, string name)
		{
			if (name == string.Empty)
			{
				foreach (PropertyDescriptor pd in this.GetPropertiesForItem(item))
				{
					this.InvalidateCache(item, pd);
				}
				return;
			}
			PropertyDescriptor pd2;
			if (item is ICustomTypeDescriptor)
			{
				pd2 = TypeDescriptor.GetProperties(item)[name];
			}
			else
			{
				pd2 = TypeDescriptor.GetProperties(item.GetType())[name];
			}
			this.InvalidateCache(item, pd2);
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0015A934 File Offset: 0x00159934
		private void InvalidateCache(object item, PropertyDescriptor pd)
		{
			if (SystemXmlLinqHelper.IsXLinqCollectionProperty(pd))
			{
				return;
			}
			ValueTable.ValueTableKey key = new ValueTable.ValueTableKey(item, pd);
			this._table.Remove(key);
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0015A960 File Offset: 0x00159960
		private IEnumerable<PropertyDescriptor> GetPropertiesForItem(object item)
		{
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			foreach (object obj in this._table)
			{
				ValueTable.ValueTableKey valueTableKey = (ValueTable.ValueTableKey)((DictionaryEntry)obj).Key;
				if (object.Equals(item, valueTableKey.Item))
				{
					list.Add(valueTableKey.PropertyDescriptor);
				}
			}
			return list;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0015A9E4 File Offset: 0x001599E4
		internal bool Purge()
		{
			if (this._table == null)
			{
				return false;
			}
			bool flag = false;
			ICollection keys = this._table.Keys;
			using (IEnumerator enumerator = keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((ValueTable.ValueTableKey)enumerator.Current).IsStale)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				ValueTable.ValueTableKey[] array = new ValueTable.ValueTableKey[keys.Count];
				keys.CopyTo(array, 0);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					if (array[i].IsStale)
					{
						this._table.Remove(array[i]);
					}
				}
			}
			return flag;
		}

		// Token: 0x04000C5B RID: 3163
		private HybridDictionary _table;

		// Token: 0x04000C5C RID: 3164
		private static object CachedNull = new object();

		// Token: 0x02000A06 RID: 2566
		private class ValueTableKey
		{
			// Token: 0x060084A8 RID: 33960 RVA: 0x0032689C File Offset: 0x0032589C
			public ValueTableKey(object item, PropertyDescriptor pd)
			{
				Invariant.Assert(item != null && pd != null);
				this._item = new WeakReference(item);
				this._descriptor = new WeakReference(pd);
				this._hashCode = item.GetHashCode() + pd.GetHashCode();
			}

			// Token: 0x17001DCD RID: 7629
			// (get) Token: 0x060084A9 RID: 33961 RVA: 0x003268E9 File Offset: 0x003258E9
			public object Item
			{
				get
				{
					return this._item.Target;
				}
			}

			// Token: 0x17001DCE RID: 7630
			// (get) Token: 0x060084AA RID: 33962 RVA: 0x003268F6 File Offset: 0x003258F6
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return (PropertyDescriptor)this._descriptor.Target;
				}
			}

			// Token: 0x17001DCF RID: 7631
			// (get) Token: 0x060084AB RID: 33963 RVA: 0x00326908 File Offset: 0x00325908
			public bool IsStale
			{
				get
				{
					return this.Item == null || this.PropertyDescriptor == null;
				}
			}

			// Token: 0x060084AC RID: 33964 RVA: 0x00326920 File Offset: 0x00325920
			public override bool Equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				ValueTable.ValueTableKey valueTableKey = o as ValueTable.ValueTableKey;
				if (valueTableKey != null)
				{
					object item = this.Item;
					PropertyDescriptor propertyDescriptor = this.PropertyDescriptor;
					return item != null && propertyDescriptor != null && (this._hashCode == valueTableKey._hashCode && object.Equals(item, valueTableKey.Item)) && object.Equals(propertyDescriptor, valueTableKey.PropertyDescriptor);
				}
				return false;
			}

			// Token: 0x060084AD RID: 33965 RVA: 0x0032697F File Offset: 0x0032597F
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04004061 RID: 16481
			private WeakReference _item;

			// Token: 0x04004062 RID: 16482
			private WeakReference _descriptor;

			// Token: 0x04004063 RID: 16483
			private int _hashCode;
		}
	}
}
