using System;

namespace System.Windows.Documents
{
	// Token: 0x02000678 RID: 1656
	internal class ListTableEntry
	{
		// Token: 0x0600519C RID: 20892 RVA: 0x0024FE4F File Offset: 0x0024EE4F
		internal ListTableEntry()
		{
			this._id = 0L;
			this._templateID = 0L;
			this._levels = new ListLevelTable();
		}

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x0600519D RID: 20893 RVA: 0x0024FE72 File Offset: 0x0024EE72
		// (set) Token: 0x0600519E RID: 20894 RVA: 0x0024FE7A File Offset: 0x0024EE7A
		internal long ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x1700133D RID: 4925
		// (set) Token: 0x0600519F RID: 20895 RVA: 0x0024FE83 File Offset: 0x0024EE83
		internal long TemplateID
		{
			set
			{
				this._templateID = value;
			}
		}

		// Token: 0x1700133E RID: 4926
		// (set) Token: 0x060051A0 RID: 20896 RVA: 0x0024FE8C File Offset: 0x0024EE8C
		internal bool Simple
		{
			set
			{
				this._simple = value;
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x060051A1 RID: 20897 RVA: 0x0024FE95 File Offset: 0x0024EE95
		internal ListLevelTable Levels
		{
			get
			{
				return this._levels;
			}
		}

		// Token: 0x04002E73 RID: 11891
		private long _id;

		// Token: 0x04002E74 RID: 11892
		private long _templateID;

		// Token: 0x04002E75 RID: 11893
		private bool _simple;

		// Token: 0x04002E76 RID: 11894
		private ListLevelTable _levels;
	}
}
