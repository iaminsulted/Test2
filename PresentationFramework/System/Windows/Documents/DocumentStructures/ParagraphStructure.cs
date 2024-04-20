using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000705 RID: 1797
	public class ParagraphStructure : SemanticBasicElement, IAddChild, IEnumerable<NamedElement>, IEnumerable
	{
		// Token: 0x06005DDA RID: 24026 RVA: 0x0028DFAA File Offset: 0x0028CFAA
		public ParagraphStructure()
		{
			this._elementType = FixedElement.ElementType.Paragraph;
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x0028DEF4 File Offset: 0x0028CEF4
		public void Add(NamedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x0028DFBC File Offset: 0x0028CFBC
		void IAddChild.AddChild(object value)
		{
			if (value is NamedElement)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType1", new object[]
			{
				value.GetType(),
				typeof(NamedElement)
			}), "value");
		}

		// Token: 0x06005DDD RID: 24029 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005DDE RID: 24030 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<NamedElement> IEnumerable<NamedElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005DDF RID: 24031 RVA: 0x0028E013 File Offset: 0x0028D013
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedElement>)this).GetEnumerator();
		}
	}
}
