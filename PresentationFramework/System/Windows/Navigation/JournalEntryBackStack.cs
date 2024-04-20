using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020005A5 RID: 1445
	internal class JournalEntryBackStack : JournalEntryStack
	{
		// Token: 0x0600462B RID: 17963 RVA: 0x002258A9 File Offset: 0x002248A9
		public JournalEntryBackStack(Journal journal) : base(journal)
		{
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x002258B2 File Offset: 0x002248B2
		public override IEnumerator GetEnumerator()
		{
			return new JournalEntryStackEnumerator(this._journal, this._journal.CurrentIndex - 1, -1, base.Filter);
		}
	}
}
