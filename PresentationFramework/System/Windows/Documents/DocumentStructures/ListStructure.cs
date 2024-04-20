using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	// Token: 0x02000707 RID: 1799
	public class ListStructure : SemanticBasicElement, IAddChild, IEnumerable<ListItemStructure>, IEnumerable
	{
		// Token: 0x06005DE6 RID: 24038 RVA: 0x0028E083 File Offset: 0x0028D083
		public ListStructure()
		{
			this._elementType = FixedElement.ElementType.List;
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x0028E093 File Offset: 0x0028D093
		public void Add(ListItemStructure listItem)
		{
			if (listItem == null)
			{
				throw new ArgumentNullException("listItem");
			}
			((IAddChild)this).AddChild(listItem);
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x0028E0AC File Offset: 0x0028D0AC
		void IAddChild.AddChild(object value)
		{
			if (value is ListItemStructure)
			{
				this._elementList.Add((ListItemStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(ListItemStructure)
			}), "value");
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x0012F160 File Offset: 0x0012E160
		IEnumerator<ListItemStructure> IEnumerable<ListItemStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005DEB RID: 24043 RVA: 0x0028E103 File Offset: 0x0028D103
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ListItemStructure>)this).GetEnumerator();
		}
	}
}
