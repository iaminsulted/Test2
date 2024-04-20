using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x02000677 RID: 1655
	internal class ListLevelTable : ArrayList
	{
		// Token: 0x06005198 RID: 20888 RVA: 0x0024FDDC File Offset: 0x0024EDDC
		internal ListLevelTable() : base(1)
		{
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x0024FDE5 File Offset: 0x0024EDE5
		internal ListLevel EntryAt(int index)
		{
			if (index > this.Count)
			{
				index = this.Count - 1;
			}
			return (ListLevel)((this.Count > index && index >= 0) ? this[index] : null);
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x0024FE18 File Offset: 0x0024EE18
		internal ListLevel AddEntry()
		{
			ListLevel listLevel = new ListLevel();
			this.Add(listLevel);
			return listLevel;
		}

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x0600519B RID: 20891 RVA: 0x0024FE34 File Offset: 0x0024EE34
		internal ListLevel CurrentEntry
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
