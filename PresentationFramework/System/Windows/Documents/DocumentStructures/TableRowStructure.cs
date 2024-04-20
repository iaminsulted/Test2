using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x0200070B RID: 1803
	public class TableRowStructure : SemanticBasicElement, IAddChild, IEnumerable<TableCellStructure>, IEnumerable
	{
		// Token: 0x06005E00 RID: 24064 RVA: 0x0028E2D3 File Offset: 0x0028D2D3
		public TableRowStructure()
		{
			this._elementType = FixedElement.ElementType.TableRow;
		}

		// Token: 0x06005E01 RID: 24065 RVA: 0x0028E2E3 File Offset: 0x0028D2E3
		public void Add(TableCellStructure tableCell)
		{
			if (tableCell == null)
			{
				throw new ArgumentNullException("tableCell");
			}
			((IAddChild)this).AddChild(tableCell);
		}

		// Token: 0x06005E02 RID: 24066 RVA: 0x0028E2FC File Offset: 0x0028D2FC
		void IAddChild.AddChild(object value)
		{
			if (value is TableCellStructure)
			{
				this._elementList.Add((TableCellStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableCellStructure)
			}), "value");
		}

		// Token: 0x06005E03 RID: 24067 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005E04 RID: 24068 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<TableCellStructure> IEnumerable<TableCellStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005E05 RID: 24069 RVA: 0x0028E353 File Offset: 0x0028D353
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TableCellStructure>)this).GetEnumerator();
		}
	}
}
