using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x02000679 RID: 1657
	internal class ListTable : ArrayList
	{
		// Token: 0x060051A2 RID: 20898 RVA: 0x0024BDD2 File Offset: 0x0024ADD2
		internal ListTable() : base(20)
		{
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x0024FE9D File Offset: 0x0024EE9D
		internal ListTableEntry EntryAt(int index)
		{
			return (ListTableEntry)this[index];
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x0024FEAC File Offset: 0x0024EEAC
		internal ListTableEntry FindEntry(long id)
		{
			for (int i = 0; i < this.Count; i++)
			{
				ListTableEntry listTableEntry = this.EntryAt(i);
				if (listTableEntry.ID == id)
				{
					return listTableEntry;
				}
			}
			return null;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x0024FEE0 File Offset: 0x0024EEE0
		internal ListTableEntry AddEntry()
		{
			ListTableEntry listTableEntry = new ListTableEntry();
			this.Add(listTableEntry);
			return listTableEntry;
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x060051A6 RID: 20902 RVA: 0x0024FEFC File Offset: 0x0024EEFC
		internal ListTableEntry CurrentEntry
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
