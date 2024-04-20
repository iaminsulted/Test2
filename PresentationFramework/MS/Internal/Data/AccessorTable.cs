using System;
using System.Collections;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x020001FE RID: 510
	internal sealed class AccessorTable
	{
		// Token: 0x060012B9 RID: 4793 RVA: 0x0014B9C8 File Offset: 0x0014A9C8
		internal AccessorTable()
		{
		}

		// Token: 0x17000370 RID: 880
		internal AccessorInfo this[SourceValueType sourceValueType, Type type, string name]
		{
			get
			{
				if (type == null || name == null)
				{
					return null;
				}
				AccessorInfo accessorInfo = (AccessorInfo)this._table[new AccessorTable.AccessorTableKey(sourceValueType, type, name)];
				if (accessorInfo != null)
				{
					accessorInfo.Generation = this._generation;
				}
				return accessorInfo;
			}
			set
			{
				if (type != null && name != null)
				{
					value.Generation = this._generation;
					this._table[new AccessorTable.AccessorTableKey(sourceValueType, type, name)] = value;
					if (!this._cleanupRequested)
					{
						this.RequestCleanup();
					}
				}
			}
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0014BA76 File Offset: 0x0014AA76
		private void RequestCleanup()
		{
			this._cleanupRequested = true;
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(this.CleanupOperation), null);
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0014BA98 File Offset: 0x0014AA98
		private object CleanupOperation(object arg)
		{
			object[] array = new object[this._table.Count];
			int num = 0;
			IDictionaryEnumerator enumerator = this._table.GetEnumerator();
			while (enumerator.MoveNext())
			{
				AccessorInfo accessorInfo = (AccessorInfo)enumerator.Value;
				if (this._generation - accessorInfo.Generation >= 10)
				{
					array[num++] = enumerator.Key;
				}
			}
			for (int i = 0; i < num; i++)
			{
				this._table.Remove(array[i]);
			}
			this._generation++;
			this._cleanupRequested = false;
			return null;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void PrintStats()
		{
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x0014BB2D File Offset: 0x0014AB2D
		// (set) Token: 0x060012C0 RID: 4800 RVA: 0x0014BB35 File Offset: 0x0014AB35
		internal bool TraceSize
		{
			get
			{
				return this._traceSize;
			}
			set
			{
				this._traceSize = value;
			}
		}

		// Token: 0x04000B4F RID: 2895
		private const int AgeLimit = 10;

		// Token: 0x04000B50 RID: 2896
		private Hashtable _table = new Hashtable();

		// Token: 0x04000B51 RID: 2897
		private int _generation;

		// Token: 0x04000B52 RID: 2898
		private bool _cleanupRequested;

		// Token: 0x04000B53 RID: 2899
		private bool _traceSize;

		// Token: 0x020009E4 RID: 2532
		private struct AccessorTableKey
		{
			// Token: 0x06008422 RID: 33826 RVA: 0x0032514C File Offset: 0x0032414C
			public AccessorTableKey(SourceValueType sourceValueType, Type type, string name)
			{
				Invariant.Assert(type != null && type != null);
				this._sourceValueType = sourceValueType;
				this._type = type;
				this._name = name;
			}

			// Token: 0x06008423 RID: 33827 RVA: 0x0032517B File Offset: 0x0032417B
			public override bool Equals(object o)
			{
				return o is AccessorTable.AccessorTableKey && this == (AccessorTable.AccessorTableKey)o;
			}

			// Token: 0x06008424 RID: 33828 RVA: 0x00325198 File Offset: 0x00324198
			public static bool operator ==(AccessorTable.AccessorTableKey k1, AccessorTable.AccessorTableKey k2)
			{
				return k1._sourceValueType == k2._sourceValueType && k1._type == k2._type && k1._name == k2._name;
			}

			// Token: 0x06008425 RID: 33829 RVA: 0x003251CE File Offset: 0x003241CE
			public static bool operator !=(AccessorTable.AccessorTableKey k1, AccessorTable.AccessorTableKey k2)
			{
				return !(k1 == k2);
			}

			// Token: 0x06008426 RID: 33830 RVA: 0x003251DA File Offset: 0x003241DA
			public override int GetHashCode()
			{
				return this._type.GetHashCode() + this._name.GetHashCode();
			}

			// Token: 0x04003FF3 RID: 16371
			private SourceValueType _sourceValueType;

			// Token: 0x04003FF4 RID: 16372
			private Type _type;

			// Token: 0x04003FF5 RID: 16373
			private string _name;
		}
	}
}
