using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Windows.Navigation
{
	// Token: 0x020005A4 RID: 1444
	internal abstract class JournalEntryStack : IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x06004623 RID: 17955 RVA: 0x002257E4 File Offset: 0x002247E4
		internal JournalEntryStack(Journal journal)
		{
			this._journal = journal;
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x002257F3 File Offset: 0x002247F3
		internal void OnCollectionChanged()
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x0022580F File Offset: 0x0022480F
		// (set) Token: 0x06004626 RID: 17958 RVA: 0x00225817 File Offset: 0x00224817
		internal JournalEntryFilter Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x00225820 File Offset: 0x00224820
		internal IEnumerable GetLimitedJournalEntryStackEnumerable()
		{
			if (this._ljese == null)
			{
				this._ljese = new LimitedJournalEntryStackEnumerable(this);
			}
			return this._ljese;
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06004628 RID: 17960 RVA: 0x0022583C File Offset: 0x0022483C
		// (remove) Token: 0x06004629 RID: 17961 RVA: 0x00225874 File Offset: 0x00224874
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x0600462A RID: 17962
		public abstract IEnumerator GetEnumerator();

		// Token: 0x04002547 RID: 9543
		private LimitedJournalEntryStackEnumerable _ljese;

		// Token: 0x04002548 RID: 9544
		protected JournalEntryFilter _filter;

		// Token: 0x0400254A RID: 9546
		protected Journal _journal;
	}
}
