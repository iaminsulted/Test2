using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x02000614 RID: 1556
	internal sealed class FixedSOMTableRow : FixedSOMContainer
	{
		// Token: 0x06004BAF RID: 19375 RVA: 0x00236F00 File Offset: 0x00235F00
		public void AddCell(FixedSOMTableCell cell)
		{
			base.Add(cell);
		}

		// Token: 0x17001163 RID: 4451
		// (get) Token: 0x06004BB0 RID: 19376 RVA: 0x0023931D File Offset: 0x0023831D
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.TableRow
				};
			}
		}

		// Token: 0x17001164 RID: 4452
		// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x0023932C File Offset: 0x0023832C
		internal bool IsEmpty
		{
			get
			{
				using (List<FixedSOMSemanticBox>.Enumerator enumerator = base.SemanticBoxes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!((FixedSOMTableCell)enumerator.Current).IsEmpty)
						{
							return false;
						}
					}
				}
				return true;
			}
		}
	}
}
