using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000282 RID: 642
	[Serializable]
	internal class JournalEntryGroupState
	{
		// Token: 0x0600185F RID: 6239 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal JournalEntryGroupState()
		{
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x00160B05 File Offset: 0x0015FB05
		internal JournalEntryGroupState(Guid navSvcId, uint contentId)
		{
			this._navigationServiceId = navSvcId;
			this._contentId = contentId;
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x00160B1B File Offset: 0x0015FB1B
		// (set) Token: 0x06001862 RID: 6242 RVA: 0x00160B23 File Offset: 0x0015FB23
		internal Guid NavigationServiceId
		{
			get
			{
				return this._navigationServiceId;
			}
			set
			{
				this._navigationServiceId = value;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x00160B2C File Offset: 0x0015FB2C
		// (set) Token: 0x06001864 RID: 6244 RVA: 0x00160B34 File Offset: 0x0015FB34
		internal uint ContentId
		{
			get
			{
				return this._contentId;
			}
			set
			{
				this._contentId = value;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001865 RID: 6245 RVA: 0x00160B3D File Offset: 0x0015FB3D
		// (set) Token: 0x06001866 RID: 6246 RVA: 0x00160B45 File Offset: 0x0015FB45
		internal DataStreams JournalDataStreams
		{
			get
			{
				return this._journalDataStreams;
			}
			set
			{
				this._journalDataStreams = value;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001867 RID: 6247 RVA: 0x00160B4E File Offset: 0x0015FB4E
		// (set) Token: 0x06001868 RID: 6248 RVA: 0x00160B56 File Offset: 0x0015FB56
		internal JournalEntry GroupExitEntry
		{
			get
			{
				return this._groupExitEntry;
			}
			set
			{
				this._groupExitEntry = value;
			}
		}

		// Token: 0x04000D4C RID: 3404
		private Guid _navigationServiceId;

		// Token: 0x04000D4D RID: 3405
		private uint _contentId;

		// Token: 0x04000D4E RID: 3406
		private DataStreams _journalDataStreams;

		// Token: 0x04000D4F RID: 3407
		private JournalEntry _groupExitEntry;
	}
}
