using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace MS.Internal.Data
{
	// Token: 0x0200024B RID: 587
	internal sealed class XDeferredAxisSource
	{
		// Token: 0x06001697 RID: 5783 RVA: 0x0015B50C File Offset: 0x0015A50C
		internal XDeferredAxisSource(object component, PropertyDescriptor pd)
		{
			this._component = new WeakReference(component);
			this._propertyDescriptor = pd;
			this._table = new HybridDictionary();
		}

		// Token: 0x17000460 RID: 1120
		public IEnumerable this[string name]
		{
			get
			{
				XDeferredAxisSource.Record record = (XDeferredAxisSource.Record)this._table[name];
				if (record == null)
				{
					object target = this._component.Target;
					if (target == null)
					{
						return null;
					}
					IEnumerable enumerable = this._propertyDescriptor.GetValue(target) as IEnumerable;
					if (enumerable != null && name != "%%FullCollection%%")
					{
						MemberInfo[] defaultMembers = enumerable.GetType().GetDefaultMembers();
						PropertyInfo propertyInfo = (defaultMembers.Length != 0) ? (defaultMembers[0] as PropertyInfo) : null;
						enumerable = ((propertyInfo == null) ? null : (propertyInfo.GetValue(enumerable, BindingFlags.GetProperty, null, new object[]
						{
							name
						}, CultureInfo.InvariantCulture) as IEnumerable));
					}
					record = new XDeferredAxisSource.Record(enumerable);
					this._table[name] = record;
				}
				else
				{
					record.DC.Update(record.XDA);
				}
				return record.Collection;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001699 RID: 5785 RVA: 0x0015B606 File Offset: 0x0015A606
		internal IEnumerable FullCollection
		{
			get
			{
				return this["%%FullCollection%%"];
			}
		}

		// Token: 0x04000C6D RID: 3181
		private WeakReference _component;

		// Token: 0x04000C6E RID: 3182
		private PropertyDescriptor _propertyDescriptor;

		// Token: 0x04000C6F RID: 3183
		private HybridDictionary _table;

		// Token: 0x04000C70 RID: 3184
		private const string FullCollectionKey = "%%FullCollection%%";

		// Token: 0x02000A08 RID: 2568
		private class Record
		{
			// Token: 0x060084B0 RID: 33968 RVA: 0x00326A7B File Offset: 0x00325A7B
			public Record(IEnumerable xda)
			{
				this._xda = xda;
				if (xda != null)
				{
					this._dc = new DifferencingCollection(xda);
					this._rooc = new ReadOnlyObservableCollection<object>(this._dc);
				}
			}

			// Token: 0x17001DD0 RID: 7632
			// (get) Token: 0x060084B1 RID: 33969 RVA: 0x00326AAA File Offset: 0x00325AAA
			public IEnumerable XDA
			{
				get
				{
					return this._xda;
				}
			}

			// Token: 0x17001DD1 RID: 7633
			// (get) Token: 0x060084B2 RID: 33970 RVA: 0x00326AB2 File Offset: 0x00325AB2
			public DifferencingCollection DC
			{
				get
				{
					return this._dc;
				}
			}

			// Token: 0x17001DD2 RID: 7634
			// (get) Token: 0x060084B3 RID: 33971 RVA: 0x00326ABA File Offset: 0x00325ABA
			public ReadOnlyObservableCollection<object> Collection
			{
				get
				{
					return this._rooc;
				}
			}

			// Token: 0x0400406A RID: 16490
			private IEnumerable _xda;

			// Token: 0x0400406B RID: 16491
			private DifferencingCollection _dc;

			// Token: 0x0400406C RID: 16492
			private ReadOnlyObservableCollection<object> _rooc;
		}
	}
}
