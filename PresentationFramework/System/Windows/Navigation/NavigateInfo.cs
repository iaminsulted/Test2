using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005C7 RID: 1479
	internal class NavigateInfo
	{
		// Token: 0x0600476E RID: 18286 RVA: 0x00229EC1 File Offset: 0x00228EC1
		internal NavigateInfo(Uri source)
		{
			this._source = source;
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x00229ED0 File Offset: 0x00228ED0
		internal NavigateInfo(Uri source, NavigationMode navigationMode)
		{
			this._source = source;
			this._navigationMode = navigationMode;
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x00229EE6 File Offset: 0x00228EE6
		internal NavigateInfo(Uri source, NavigationMode navigationMode, JournalEntry journalEntry)
		{
			this._source = source;
			this._navigationMode = navigationMode;
			this._journalEntry = journalEntry;
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x06004771 RID: 18289 RVA: 0x00229F03 File Offset: 0x00228F03
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06004772 RID: 18290 RVA: 0x00229F0B File Offset: 0x00228F0B
		internal NavigationMode NavigationMode
		{
			get
			{
				return this._navigationMode;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06004773 RID: 18291 RVA: 0x00229F13 File Offset: 0x00228F13
		internal JournalEntry JournalEntry
		{
			get
			{
				return this._journalEntry;
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06004774 RID: 18292 RVA: 0x00229F1B File Offset: 0x00228F1B
		internal bool IsConsistent
		{
			get
			{
				return (this._navigationMode == NavigationMode.New ^ this._journalEntry != null) || this._navigationMode == NavigationMode.Refresh;
			}
		}

		// Token: 0x040025CD RID: 9677
		private Uri _source;

		// Token: 0x040025CE RID: 9678
		private NavigationMode _navigationMode;

		// Token: 0x040025CF RID: 9679
		private JournalEntry _journalEntry;
	}
}
