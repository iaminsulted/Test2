using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000708 RID: 1800
	public class ListItemStructure : SemanticBasicElement, IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		// Token: 0x06005DEC RID: 24044 RVA: 0x0028E10B File Offset: 0x0028D10B
		public ListItemStructure()
		{
			this._elementType = FixedElement.ElementType.ListItem;
		}

		// Token: 0x06005DED RID: 24045 RVA: 0x0028DEF4 File Offset: 0x0028CEF4
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x0028E11C File Offset: 0x0028D11C
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

		// Token: 0x06005DEF RID: 24047 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005DF0 RID: 24048 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005DF1 RID: 24049 RVA: 0x0028DFA2 File Offset: 0x0028CFA2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x06005DF2 RID: 24050 RVA: 0x0028E1B2 File Offset: 0x0028D1B2
		// (set) Token: 0x06005DF3 RID: 24051 RVA: 0x0028E1BA File Offset: 0x0028D1BA
		public string Marker
		{
			get
			{
				return this._markerName;
			}
			set
			{
				this._markerName = value;
			}
		}

		// Token: 0x0400316C RID: 12652
		private string _markerName;
	}
}
