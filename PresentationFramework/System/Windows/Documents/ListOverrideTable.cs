using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x0200067B RID: 1659
	internal class ListOverrideTable : ArrayList
	{
		// Token: 0x060051B0 RID: 20912 RVA: 0x0024BDD2 File Offset: 0x0024ADD2
		internal ListOverrideTable() : base(20)
		{
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x0024FF82 File Offset: 0x0024EF82
		internal ListOverride EntryAt(int index)
		{
			return (ListOverride)this[index];
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x0024FF90 File Offset: 0x0024EF90
		internal ListOverride FindEntry(int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				ListOverride listOverride = this.EntryAt(i);
				if (listOverride.Index == (long)index)
				{
					return listOverride;
				}
			}
			return null;
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x0024FFC4 File Offset: 0x0024EFC4
		internal ListOverride AddEntry()
		{
			ListOverride listOverride = new ListOverride();
			this.Add(listOverride);
			return listOverride;
		}

		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x060051B4 RID: 20916 RVA: 0x0024FFE0 File Offset: 0x0024EFE0
		internal ListOverride CurrentEntry
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this.EntryAt(this.Count - 1);
			}
		}
	}
}
