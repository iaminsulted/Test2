using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020005A7 RID: 1447
	internal class JournalEntryStackEnumerator : IEnumerator
	{
		// Token: 0x0600462F RID: 17967 RVA: 0x002258F4 File Offset: 0x002248F4
		public JournalEntryStackEnumerator(Journal journal, int start, int delta, JournalEntryFilter filter)
		{
			this._journal = journal;
			this._version = journal.Version;
			this._start = start;
			this._delta = delta;
			this._filter = filter;
			this.Reset();
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x0022592B File Offset: 0x0022492B
		public void Reset()
		{
			this._next = this._start;
			this._current = null;
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x00225940 File Offset: 0x00224940
		public bool MoveNext()
		{
			this.VerifyUnchanged();
			while (this._next >= 0 && this._next < this._journal.TotalCount)
			{
				this._current = this._journal[this._next];
				this._next += this._delta;
				if (this._filter == null || this._filter(this._current))
				{
					return true;
				}
			}
			this._current = null;
			return false;
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x002259C0 File Offset: 0x002249C0
		public object Current
		{
			get
			{
				return this._current;
			}
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x002259C8 File Offset: 0x002249C8
		protected void VerifyUnchanged()
		{
			if (this._version != this._journal.Version)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
		}

		// Token: 0x0400254B RID: 9547
		private Journal _journal;

		// Token: 0x0400254C RID: 9548
		private int _start;

		// Token: 0x0400254D RID: 9549
		private int _delta;

		// Token: 0x0400254E RID: 9550
		private int _next;

		// Token: 0x0400254F RID: 9551
		private JournalEntry _current;

		// Token: 0x04002550 RID: 9552
		private JournalEntryFilter _filter;

		// Token: 0x04002551 RID: 9553
		private int _version;
	}
}
