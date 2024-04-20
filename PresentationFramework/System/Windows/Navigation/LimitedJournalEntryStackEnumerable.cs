using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Windows.Navigation
{
	// Token: 0x020005A8 RID: 1448
	internal class LimitedJournalEntryStackEnumerable : IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x06004634 RID: 17972 RVA: 0x002259F0 File Offset: 0x002249F0
		internal LimitedJournalEntryStackEnumerable(IEnumerable ieble)
		{
			this._ieble = ieble;
			INotifyCollectionChanged notifyCollectionChanged = ieble as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged += this.PropogateCollectionChanged;
			}
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x00225A26 File Offset: 0x00224A26
		public IEnumerator GetEnumerator()
		{
			return new LimitedJournalEntryStackEnumerator(this._ieble, 9U);
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x00225A35 File Offset: 0x00224A35
		internal void PropogateCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, e);
			}
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06004637 RID: 17975 RVA: 0x00225A4C File Offset: 0x00224A4C
		// (remove) Token: 0x06004638 RID: 17976 RVA: 0x00225A84 File Offset: 0x00224A84
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x04002552 RID: 9554
		private const uint DefaultMaxMenuEntries = 9U;

		// Token: 0x04002554 RID: 9556
		private IEnumerable _ieble;
	}
}
