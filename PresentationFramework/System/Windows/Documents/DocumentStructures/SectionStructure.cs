using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000704 RID: 1796
	public class SectionStructure : SemanticBasicElement, IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		// Token: 0x06005DD4 RID: 24020 RVA: 0x0028DEE4 File Offset: 0x0028CEE4
		public SectionStructure()
		{
			this._elementType = FixedElement.ElementType.Section;
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x0028DEF4 File Offset: 0x0028CEF4
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x0028DF0C File Offset: 0x0028CF0C
		void IAddChild.AddChild(object value)
		{
			if (value is ParagraphStructure || value is FigureStructure || value is ListStructure || value is TableStructure)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType4", new object[]
			{
				value.GetType(),
				typeof(ParagraphStructure),
				typeof(FigureStructure),
				typeof(ListStructure),
				typeof(TableStructure)
			}), "value");
		}

		// Token: 0x06005DD7 RID: 24023 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x0028DFA2 File Offset: 0x0028CFA2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}
	}
}
