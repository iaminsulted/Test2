using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Win32;

namespace System.Windows.Navigation
{
	// Token: 0x020005AD RID: 1453
	[Serializable]
	internal sealed class Journal : ISerializable
	{
		// Token: 0x06004642 RID: 17986 RVA: 0x00225B69 File Offset: 0x00224B69
		internal Journal()
		{
			this._Initialize();
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x00225B8D File Offset: 0x00224B8D
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_journalEntryList", this._journalEntryList);
			info.AddValue("_currentEntryIndex", this._currentEntryIndex);
			info.AddValue("_journalEntryId", this._journalEntryId);
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x00225BC4 File Offset: 0x00224BC4
		private Journal(SerializationInfo info, StreamingContext context)
		{
			this._Initialize();
			this._journalEntryList = (List<JournalEntry>)info.GetValue("_journalEntryList", typeof(List<JournalEntry>));
			this._currentEntryIndex = info.GetInt32("_currentEntryIndex");
			this._uncommittedCurrentIndex = this._currentEntryIndex;
			this._journalEntryId = info.GetInt32("_journalEntryId");
		}

		// Token: 0x17000FAE RID: 4014
		internal JournalEntry this[int index]
		{
			get
			{
				return this._journalEntryList[index];
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06004646 RID: 17990 RVA: 0x00225C4F File Offset: 0x00224C4F
		internal int TotalCount
		{
			get
			{
				return this._journalEntryList.Count;
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06004647 RID: 17991 RVA: 0x00225C5C File Offset: 0x00224C5C
		internal int CurrentIndex
		{
			get
			{
				return this._currentEntryIndex;
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06004648 RID: 17992 RVA: 0x00225C64 File Offset: 0x00224C64
		internal JournalEntry CurrentEntry
		{
			get
			{
				if (this._currentEntryIndex >= 0 && this._currentEntryIndex < this.TotalCount)
				{
					return this._journalEntryList[this._currentEntryIndex];
				}
				return null;
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06004649 RID: 17993 RVA: 0x00225C90 File Offset: 0x00224C90
		internal bool HasUncommittedNavigation
		{
			get
			{
				return this._uncommittedCurrentIndex != this._currentEntryIndex;
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x00225CA3 File Offset: 0x00224CA3
		internal JournalEntryStack BackStack
		{
			get
			{
				return this._backStack;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x0600464B RID: 17995 RVA: 0x00225CAB File Offset: 0x00224CAB
		internal JournalEntryStack ForwardStack
		{
			get
			{
				return this._forwardStack;
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x00225CB3 File Offset: 0x00224CB3
		internal bool CanGoBack
		{
			get
			{
				return this.GetGoBackEntry() != null;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x0600464D RID: 17997 RVA: 0x00225CC0 File Offset: 0x00224CC0
		internal bool CanGoForward
		{
			get
			{
				int num;
				this.GetGoForwardEntryIndex(out num);
				return num != -1;
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x00225CDC File Offset: 0x00224CDC
		internal int Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x0600464F RID: 17999 RVA: 0x00225CE4 File Offset: 0x00224CE4
		// (set) Token: 0x06004650 RID: 18000 RVA: 0x00225CEC File Offset: 0x00224CEC
		internal JournalEntryFilter Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
				this.BackStack.Filter = this._filter;
				this.ForwardStack.Filter = this._filter;
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06004651 RID: 18001 RVA: 0x00225D17 File Offset: 0x00224D17
		// (remove) Token: 0x06004652 RID: 18002 RVA: 0x00225D30 File Offset: 0x00224D30
		internal event EventHandler BackForwardStateChange
		{
			add
			{
				this._backForwardStateChange = (EventHandler)Delegate.Combine(this._backForwardStateChange, value);
			}
			remove
			{
				this._backForwardStateChange = (EventHandler)Delegate.Remove(this._backForwardStateChange, value);
			}
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x00225D4C File Offset: 0x00224D4C
		internal JournalEntry RemoveBackEntry()
		{
			int num = this._currentEntryIndex;
			while (--num >= 0)
			{
				if (this.IsNavigable(this._journalEntryList[num]))
				{
					JournalEntry result = this.RemoveEntryInternal(num);
					this.UpdateView();
					return result;
				}
			}
			return null;
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x00225D8C File Offset: 0x00224D8C
		internal void UpdateCurrentEntry(JournalEntry journalEntry)
		{
			if (journalEntry == null)
			{
				throw new ArgumentNullException("journalEntry");
			}
			if (this._currentEntryIndex > -1 && this._currentEntryIndex < this.TotalCount)
			{
				JournalEntry journalEntry2 = this._journalEntryList[this._currentEntryIndex];
				journalEntry.Id = journalEntry2.Id;
				this._journalEntryList[this._currentEntryIndex] = journalEntry;
			}
			else
			{
				int num = this._journalEntryId + 1;
				this._journalEntryId = num;
				journalEntry.Id = num;
				this._journalEntryList.Add(journalEntry);
			}
			this._version++;
			journalEntry.JEGroupState.GroupExitEntry = journalEntry;
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x00225E2D File Offset: 0x00224E2D
		internal void RecordNewNavigation()
		{
			Invariant.Assert(this.ValidateIndexes());
			this._currentEntryIndex++;
			this._uncommittedCurrentIndex = this._currentEntryIndex;
			if (!this.ClearForwardStack())
			{
				this.UpdateView();
			}
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x00225E64 File Offset: 0x00224E64
		internal bool ClearForwardStack()
		{
			if (this._currentEntryIndex >= this.TotalCount)
			{
				return false;
			}
			if (this._uncommittedCurrentIndex > this._currentEntryIndex)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_CannotClearFwdStack"));
			}
			this._journalEntryList.RemoveRange(this._currentEntryIndex, this._journalEntryList.Count - this._currentEntryIndex);
			this.UpdateView();
			return true;
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00225EC9 File Offset: 0x00224EC9
		internal void CommitJournalNavigation(JournalEntry navigated)
		{
			this.NavigateTo(navigated);
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x00225ED2 File Offset: 0x00224ED2
		internal void AbortJournalNavigation()
		{
			this._uncommittedCurrentIndex = this._currentEntryIndex;
			this.UpdateView();
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00225EE8 File Offset: 0x00224EE8
		internal JournalEntry BeginBackNavigation()
		{
			Invariant.Assert(this.ValidateIndexes());
			int uncommittedCurrentIndex;
			JournalEntry goBackEntry = this.GetGoBackEntry(out uncommittedCurrentIndex);
			if (goBackEntry == null)
			{
				throw new InvalidOperationException(SR.Get("NoBackEntry"));
			}
			this._uncommittedCurrentIndex = uncommittedCurrentIndex;
			this.UpdateView();
			if (this._uncommittedCurrentIndex == this._currentEntryIndex)
			{
				return null;
			}
			return goBackEntry;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00225F3C File Offset: 0x00224F3C
		internal JournalEntry BeginForwardNavigation()
		{
			Invariant.Assert(this.ValidateIndexes());
			int num;
			this.GetGoForwardEntryIndex(out num);
			if (num == -1)
			{
				throw new InvalidOperationException(SR.Get("NoForwardEntry"));
			}
			this._uncommittedCurrentIndex = num;
			this.UpdateView();
			if (num == this._currentEntryIndex)
			{
				return null;
			}
			return this._journalEntryList[num];
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00225F94 File Offset: 0x00224F94
		internal NavigationMode GetNavigationMode(JournalEntry entry)
		{
			if (this._journalEntryList.IndexOf(entry) <= this._currentEntryIndex)
			{
				return NavigationMode.Back;
			}
			return NavigationMode.Forward;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00225FB0 File Offset: 0x00224FB0
		internal void NavigateTo(JournalEntry target)
		{
			int num = this._journalEntryList.IndexOf(target);
			if (num > -1)
			{
				this._currentEntryIndex = num;
				this._uncommittedCurrentIndex = this._currentEntryIndex;
				this.UpdateView();
			}
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00225FE8 File Offset: 0x00224FE8
		internal int FindIndexForEntryWithId(int id)
		{
			for (int i = 0; i < this.TotalCount; i++)
			{
				if (this[i].Id == id)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00226018 File Offset: 0x00225018
		internal void PruneKeepAliveEntries()
		{
			for (int i = this.TotalCount - 1; i >= 0; i--)
			{
				JournalEntry journalEntry = this._journalEntryList[i];
				if (journalEntry.IsAlive())
				{
					this.RemoveEntryInternal(i);
				}
				else
				{
					DataStreams journalDataStreams = journalEntry.JEGroupState.JournalDataStreams;
					if (journalDataStreams != null)
					{
						journalDataStreams.PrepareForSerialization();
					}
					if (journalEntry.RootViewerState != null)
					{
						journalEntry.RootViewerState.PrepareForSerialization();
					}
				}
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00226080 File Offset: 0x00225080
		internal JournalEntry RemoveEntryInternal(int index)
		{
			JournalEntry result = this._journalEntryList[index];
			this._version++;
			this._journalEntryList.RemoveAt(index);
			if (this._currentEntryIndex > index)
			{
				this._currentEntryIndex--;
			}
			if (this._uncommittedCurrentIndex > index)
			{
				this._uncommittedCurrentIndex--;
			}
			return result;
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x002260E4 File Offset: 0x002250E4
		internal void RemoveEntries(Guid navSvcId)
		{
			for (int i = this.TotalCount - 1; i >= 0; i--)
			{
				if (i != this._currentEntryIndex && this._journalEntryList[i].NavigationServiceId == navSvcId)
				{
					this.RemoveEntryInternal(i);
				}
			}
			this.UpdateView();
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00226134 File Offset: 0x00225134
		internal void UpdateView()
		{
			this.BackStack.OnCollectionChanged();
			this.ForwardStack.OnCollectionChanged();
			if (this._backForwardStateChange != null)
			{
				this._backForwardStateChange(this, EventArgs.Empty);
			}
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00226168 File Offset: 0x00225168
		internal JournalEntry GetGoBackEntry(out int index)
		{
			for (index = this._uncommittedCurrentIndex - 1; index >= 0; index--)
			{
				JournalEntry journalEntry = this._journalEntryList[index];
				if (this.IsNavigable(journalEntry))
				{
					return journalEntry;
				}
			}
			return null;
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x002261A8 File Offset: 0x002251A8
		internal JournalEntry GetGoBackEntry()
		{
			int num;
			return this.GetGoBackEntry(out num);
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x002261BD File Offset: 0x002251BD
		internal void GetGoForwardEntryIndex(out int index)
		{
			index = this._uncommittedCurrentIndex;
			for (;;)
			{
				index++;
				if (index == this._currentEntryIndex)
				{
					break;
				}
				if (index >= this.TotalCount)
				{
					goto Block_2;
				}
				if (this.IsNavigable(this._journalEntryList[index]))
				{
					return;
				}
			}
			return;
			Block_2:
			index = -1;
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x002261FB File Offset: 0x002251FB
		private bool ValidateIndexes()
		{
			return this._currentEntryIndex >= 0 && this._currentEntryIndex <= this.TotalCount && this._uncommittedCurrentIndex >= 0 && this._uncommittedCurrentIndex <= this.TotalCount;
		}

		// Token: 0x06004666 RID: 18022 RVA: 0x00226230 File Offset: 0x00225230
		private void _Initialize()
		{
			this._backStack = new JournalEntryBackStack(this);
			this._forwardStack = new JournalEntryForwardStack(this);
		}

		// Token: 0x06004667 RID: 18023 RVA: 0x0022624A File Offset: 0x0022524A
		internal bool IsNavigable(JournalEntry entry)
		{
			if (entry == null)
			{
				return false;
			}
			if (this.Filter == null)
			{
				return entry.IsNavigable();
			}
			return this.Filter(entry);
		}

		// Token: 0x04002563 RID: 9571
		[NonSerialized]
		private EventHandler _backForwardStateChange;

		// Token: 0x04002564 RID: 9572
		private JournalEntryFilter _filter;

		// Token: 0x04002565 RID: 9573
		private JournalEntryBackStack _backStack;

		// Token: 0x04002566 RID: 9574
		private JournalEntryForwardStack _forwardStack;

		// Token: 0x04002567 RID: 9575
		private int _journalEntryId = SafeNativeMethods.GetTickCount();

		// Token: 0x04002568 RID: 9576
		private List<JournalEntry> _journalEntryList = new List<JournalEntry>();

		// Token: 0x04002569 RID: 9577
		private int _currentEntryIndex;

		// Token: 0x0400256A RID: 9578
		private int _uncommittedCurrentIndex;

		// Token: 0x0400256B RID: 9579
		private int _version;
	}
}
