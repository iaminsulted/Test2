using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x02000671 RID: 1649
	internal class MarkerList : ArrayList
	{
		// Token: 0x06005163 RID: 20835 RVA: 0x0024F245 File Offset: 0x0024E245
		internal MarkerList() : base(5)
		{
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x0024F24E File Offset: 0x0024E24E
		internal MarkerListEntry EntryAt(int index)
		{
			return (MarkerListEntry)this[index];
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x0024F25C File Offset: 0x0024E25C
		internal void AddEntry(MarkerStyle m, long nILS, long nStartIndexOverride, long nStartIndexDefault, long nLevel)
		{
			this.Add(new MarkerListEntry
			{
				Marker = m,
				StartIndexOverride = nStartIndexOverride,
				StartIndexDefault = nStartIndexDefault,
				VirtualListLevel = nLevel,
				ILS = nILS
			});
		}
	}
}
