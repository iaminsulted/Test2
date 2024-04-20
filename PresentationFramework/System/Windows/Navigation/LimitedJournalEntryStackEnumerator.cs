using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020005A9 RID: 1449
	internal class LimitedJournalEntryStackEnumerator : IEnumerator
	{
		// Token: 0x06004639 RID: 17977 RVA: 0x00225AB9 File Offset: 0x00224AB9
		internal LimitedJournalEntryStackEnumerator(IEnumerable ieble, uint viewLimit)
		{
			this._ienum = ieble.GetEnumerator();
			this._viewLimit = viewLimit;
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x00225AD4 File Offset: 0x00224AD4
		public void Reset()
		{
			this._itemsReturned = 0U;
			this._ienum.Reset();
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x00225AE8 File Offset: 0x00224AE8
		public bool MoveNext()
		{
			bool flag;
			if (this._itemsReturned == this._viewLimit)
			{
				flag = false;
			}
			else
			{
				flag = this._ienum.MoveNext();
				if (flag)
				{
					this._itemsReturned += 1U;
				}
			}
			return flag;
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x0600463C RID: 17980 RVA: 0x00225B25 File Offset: 0x00224B25
		public object Current
		{
			get
			{
				return this._ienum.Current;
			}
		}

		// Token: 0x04002555 RID: 9557
		private uint _itemsReturned;

		// Token: 0x04002556 RID: 9558
		private uint _viewLimit;

		// Token: 0x04002557 RID: 9559
		private IEnumerator _ienum;
	}
}
