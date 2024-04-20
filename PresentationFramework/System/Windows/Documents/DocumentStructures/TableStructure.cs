using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000709 RID: 1801
	public class TableStructure : SemanticBasicElement, IAddChild, IEnumerable<TableRowGroupStructure>, IEnumerable
	{
		// Token: 0x06005DF4 RID: 24052 RVA: 0x0028E1C3 File Offset: 0x0028D1C3
		public TableStructure()
		{
			this._elementType = FixedElement.ElementType.Table;
		}

		// Token: 0x06005DF5 RID: 24053 RVA: 0x0028E1D3 File Offset: 0x0028D1D3
		public void Add(TableRowGroupStructure tableRowGroup)
		{
			if (tableRowGroup == null)
			{
				throw new ArgumentNullException("tableRowGroup");
			}
			((IAddChild)this).AddChild(tableRowGroup);
		}

		// Token: 0x06005DF6 RID: 24054 RVA: 0x0028E1EC File Offset: 0x0028D1EC
		void IAddChild.AddChild(object value)
		{
			if (value is TableRowGroupStructure)
			{
				this._elementList.Add((TableRowGroupStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRowGroupStructure)
			}), "value");
		}

		// Token: 0x06005DF7 RID: 24055 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<TableRowGroupStructure> IEnumerable<TableRowGroupStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005DF9 RID: 24057 RVA: 0x0028E243 File Offset: 0x0028D243
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TableRowGroupStructure>)this).GetEnumerator();
		}
	}
}
