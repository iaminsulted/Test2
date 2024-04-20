using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000706 RID: 1798
	public class FigureStructure : SemanticBasicElement, IAddChild, IEnumerable<NamedElement>, IEnumerable
	{
		// Token: 0x06005DE0 RID: 24032 RVA: 0x0028E01B File Offset: 0x0028D01B
		public FigureStructure()
		{
			this._elementType = FixedElement.ElementType.Figure;
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x0028E02C File Offset: 0x0028D02C
		void IAddChild.AddChild(object value)
		{
			if (value is NamedElement)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(NamedElement)
			}), "value");
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x0028DEF4 File Offset: 0x0028CEF4
		public void Add(NamedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<NamedElement> IEnumerable<NamedElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005DE5 RID: 24037 RVA: 0x0028E013 File Offset: 0x0028D013
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedElement>)this).GetEnumerator();
		}
	}
}
