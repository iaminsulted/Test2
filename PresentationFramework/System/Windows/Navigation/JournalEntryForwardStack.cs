using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020005A6 RID: 1446
	internal class JournalEntryForwardStack : JournalEntryStack
	{
		// Token: 0x0600462D RID: 17965 RVA: 0x002258A9 File Offset: 0x002248A9
		public JournalEntryForwardStack(Journal journal) : base(journal)
		{
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x002258D3 File Offset: 0x002248D3
		public override IEnumerator GetEnumerator()
		{
			return new JournalEntryStackEnumerator(this._journal, this._journal.CurrentIndex + 1, 1, base.Filter);
		}
	}
}
