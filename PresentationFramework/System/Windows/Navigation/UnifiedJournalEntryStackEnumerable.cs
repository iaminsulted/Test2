using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Windows.Navigation
{
	// Token: 0x020005B5 RID: 1461
	internal class UnifiedJournalEntryStackEnumerable : IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x06004698 RID: 18072 RVA: 0x002267F0 File Offset: 0x002257F0
		internal UnifiedJournalEntryStackEnumerable(LimitedJournalEntryStackEnumerable backStack, LimitedJournalEntryStackEnumerable forwardStack)
		{
			this._backStack = backStack;
			this._backStack.CollectionChanged += this.StacksChanged;
			this._forwardStack = forwardStack;
			this._forwardStack.CollectionChanged += this.StacksChanged;
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x00226840 File Offset: 0x00225840
		public IEnumerator GetEnumerator()
		{
			if (this._items == null)
			{
				this._items = new ArrayList(19);
				foreach (object obj in this._forwardStack)
				{
					JournalEntry journalEntry = (JournalEntry)obj;
					this._items.Insert(0, journalEntry);
					JournalEntryUnifiedViewConverter.SetJournalEntryPosition(journalEntry, JournalEntryPosition.Forward);
				}
				DependencyObject dependencyObject = new DependencyObject();
				dependencyObject.SetValue(JournalEntry.NameProperty, SR.Get("NavWindowMenuCurrentPage"));
				this._items.Add(dependencyObject);
				foreach (object obj2 in this._backStack)
				{
					JournalEntry journalEntry2 = (JournalEntry)obj2;
					this._items.Add(journalEntry2);
					JournalEntryUnifiedViewConverter.SetJournalEntryPosition(journalEntry2, JournalEntryPosition.Back);
				}
			}
			return this._items.GetEnumerator();
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0022694C File Offset: 0x0022594C
		internal void StacksChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this._items = null;
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x0600469B RID: 18075 RVA: 0x00226970 File Offset: 0x00225970
		// (remove) Token: 0x0600469C RID: 18076 RVA: 0x002269A8 File Offset: 0x002259A8
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x0400257A RID: 9594
		private LimitedJournalEntryStackEnumerable _backStack;

		// Token: 0x0400257B RID: 9595
		private LimitedJournalEntryStackEnumerable _forwardStack;

		// Token: 0x0400257C RID: 9596
		private ArrayList _items;
	}
}
