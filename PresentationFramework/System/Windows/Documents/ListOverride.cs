using System;

namespace System.Windows.Documents
{
	// Token: 0x0200067A RID: 1658
	internal class ListOverride
	{
		// Token: 0x060051A7 RID: 20903 RVA: 0x0024FF17 File Offset: 0x0024EF17
		internal ListOverride()
		{
			this._id = 0L;
			this._index = 0L;
			this._levels = null;
			this._nStartIndex = -1L;
		}

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x060051A8 RID: 20904 RVA: 0x0024FF3E File Offset: 0x0024EF3E
		// (set) Token: 0x060051A9 RID: 20905 RVA: 0x0024FF46 File Offset: 0x0024EF46
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

		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x060051AA RID: 20906 RVA: 0x0024FF4F File Offset: 0x0024EF4F
		// (set) Token: 0x060051AB RID: 20907 RVA: 0x0024FF57 File Offset: 0x0024EF57
		internal long Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x060051AC RID: 20908 RVA: 0x0024FF60 File Offset: 0x0024EF60
		// (set) Token: 0x060051AD RID: 20909 RVA: 0x0024FF68 File Offset: 0x0024EF68
		internal ListLevelTable Levels
		{
			get
			{
				return this._levels;
			}
			set
			{
				this._levels = value;
			}
		}

		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x060051AE RID: 20910 RVA: 0x0024FF71 File Offset: 0x0024EF71
		// (set) Token: 0x060051AF RID: 20911 RVA: 0x0024FF79 File Offset: 0x0024EF79
		internal long StartIndex
		{
			get
			{
				return this._nStartIndex;
			}
			set
			{
				this._nStartIndex = value;
			}
		}

		// Token: 0x04002E77 RID: 11895
		private long _id;

		// Token: 0x04002E78 RID: 11896
		private long _index;

		// Token: 0x04002E79 RID: 11897
		private long _nStartIndex;

		// Token: 0x04002E7A RID: 11898
		private ListLevelTable _levels;
	}
}
