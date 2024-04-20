using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x0200070A RID: 1802
	public class TableRowGroupStructure : SemanticBasicElement, IAddChild, IEnumerable<TableRowStructure>, IEnumerable
	{
		// Token: 0x06005DFA RID: 24058 RVA: 0x0028E24B File Offset: 0x0028D24B
		public TableRowGroupStructure()
		{
			this._elementType = FixedElement.ElementType.TableRowGroup;
		}

		// Token: 0x06005DFB RID: 24059 RVA: 0x0028E25B File Offset: 0x0028D25B
		public void Add(TableRowStructure tableRow)
		{
			if (tableRow == null)
			{
				throw new ArgumentNullException("tableRow");
			}
			((IAddChild)this).AddChild(tableRow);
		}

		// Token: 0x06005DFC RID: 24060 RVA: 0x0028E274 File Offset: 0x0028D274
		void IAddChild.AddChild(object value)
		{
			if (value is TableRowStructure)
			{
				this._elementList.Add((TableRowStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRowStructure)
			}), "value");
		}

		// Token: 0x06005DFD RID: 24061 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005DFE RID: 24062 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<TableRowStructure> IEnumerable<TableRowStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005DFF RID: 24063 RVA: 0x0028E2CB File Offset: 0x0028D2CB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TableRowStructure>)this).GetEnumerator();
		}
	}
}
