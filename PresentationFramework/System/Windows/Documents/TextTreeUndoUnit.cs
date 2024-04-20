using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020006D7 RID: 1751
	internal abstract class TextTreeUndoUnit : IUndoUnit
	{
		// Token: 0x06005B9F RID: 23455 RVA: 0x00284784 File Offset: 0x00283784
		internal TextTreeUndoUnit(TextContainer tree, int symbolOffset)
		{
			this._tree = tree;
			this._symbolOffset = symbolOffset;
			this._treeContentHashCode = this._tree.GetContentHashCode();
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x002847AC File Offset: 0x002837AC
		public void Do()
		{
			this._tree.BeginChange();
			try
			{
				this.DoCore();
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		// Token: 0x06005BA1 RID: 23457
		public abstract void DoCore();

		// Token: 0x06005BA2 RID: 23458 RVA: 0x00142B2E File Offset: 0x00141B2E
		public bool Merge(IUndoUnit unit)
		{
			Invariant.Assert(unit != null);
			return false;
		}

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x06005BA3 RID: 23459 RVA: 0x002847E8 File Offset: 0x002837E8
		protected TextContainer TextContainer
		{
			get
			{
				return this._tree;
			}
		}

		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x06005BA4 RID: 23460 RVA: 0x002847F0 File Offset: 0x002837F0
		protected int SymbolOffset
		{
			get
			{
				return this._symbolOffset;
			}
		}

		// Token: 0x06005BA5 RID: 23461 RVA: 0x002847F8 File Offset: 0x002837F8
		internal void SetTreeHashCode()
		{
			this._treeContentHashCode = this._tree.GetContentHashCode();
		}

		// Token: 0x06005BA6 RID: 23462 RVA: 0x0028480B File Offset: 0x0028380B
		internal void VerifyTreeContentHashCode()
		{
			if (this._tree.GetContentHashCode() != this._treeContentHashCode)
			{
				Invariant.Assert(false, "Undo unit is out of sync with TextContainer!");
			}
		}

		// Token: 0x06005BA7 RID: 23463 RVA: 0x0028482C File Offset: 0x0028382C
		internal static PropertyRecord[] GetPropertyRecordArray(DependencyObject d)
		{
			LocalValueEnumerator localValueEnumerator = d.GetLocalValueEnumerator();
			PropertyRecord[] array = new PropertyRecord[localValueEnumerator.Count];
			int num = 0;
			localValueEnumerator.Reset();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				DependencyProperty property = localValueEntry.Property;
				if (!property.ReadOnly)
				{
					array[num].Property = property;
					array[num].Value = d.GetValue(property);
					num++;
				}
			}
			PropertyRecord[] array2;
			if (localValueEnumerator.Count != num)
			{
				array2 = new PropertyRecord[num];
				for (int i = 0; i < num; i++)
				{
					array2[i] = array[i];
				}
			}
			else
			{
				array2 = array;
			}
			return array2;
		}

		// Token: 0x06005BA8 RID: 23464 RVA: 0x002848D8 File Offset: 0x002838D8
		internal static LocalValueEnumerator ArrayToLocalValueEnumerator(PropertyRecord[] records)
		{
			DependencyObject dependencyObject = new DependencyObject();
			for (int i = 0; i < records.Length; i++)
			{
				dependencyObject.SetValue(records[i].Property, records[i].Value);
			}
			return dependencyObject.GetLocalValueEnumerator();
		}

		// Token: 0x04003094 RID: 12436
		private readonly TextContainer _tree;

		// Token: 0x04003095 RID: 12437
		private readonly int _symbolOffset;

		// Token: 0x04003096 RID: 12438
		private int _treeContentHashCode;
	}
}
