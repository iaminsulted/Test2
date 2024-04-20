using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x0200070C RID: 1804
	public class TableCellStructure : SemanticBasicElement, IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		// Token: 0x06005E06 RID: 24070 RVA: 0x0028E35B File Offset: 0x0028D35B
		public TableCellStructure()
		{
			this._elementType = FixedElement.ElementType.TableCell;
			this._rowSpan = 1;
			this._columnSpan = 1;
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x0028DEF4 File Offset: 0x0028CEF4
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x0028E11C File Offset: 0x0028D11C
		void IAddChild.AddChild(object value)
		{
			if (value is ParagraphStructure || value is TableStructure || value is ListStructure || value is FigureStructure)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType4", new object[]
			{
				value.GetType(),
				typeof(ParagraphStructure),
				typeof(TableStructure),
				typeof(ListStructure),
				typeof(FigureStructure)
			}), "value");
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005E0B RID: 24075 RVA: 0x0028DFA2 File Offset: 0x0028CFA2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x06005E0C RID: 24076 RVA: 0x0028E379 File Offset: 0x0028D379
		// (set) Token: 0x06005E0D RID: 24077 RVA: 0x0028E381 File Offset: 0x0028D381
		public int RowSpan
		{
			get
			{
				return this._rowSpan;
			}
			set
			{
				this._rowSpan = value;
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x06005E0E RID: 24078 RVA: 0x0028E38A File Offset: 0x0028D38A
		// (set) Token: 0x06005E0F RID: 24079 RVA: 0x0028E392 File Offset: 0x0028D392
		public int ColumnSpan
		{
			get
			{
				return this._columnSpan;
			}
			set
			{
				this._columnSpan = value;
			}
		}

		// Token: 0x0400316D RID: 12653
		private int _rowSpan;

		// Token: 0x0400316E RID: 12654
		private int _columnSpan;
	}
}
