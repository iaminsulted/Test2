using System;

namespace System.Windows.Documents
{
	// Token: 0x0200067D RID: 1661
	internal class ColumnState
	{
		// Token: 0x06005208 RID: 21000 RVA: 0x00251A6A File Offset: 0x00250A6A
		internal ColumnState()
		{
			this._nCellX = 0L;
			this._row = null;
			this._fFilled = false;
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06005209 RID: 21001 RVA: 0x00251A88 File Offset: 0x00250A88
		// (set) Token: 0x0600520A RID: 21002 RVA: 0x00251A90 File Offset: 0x00250A90
		internal long CellX
		{
			get
			{
				return this._nCellX;
			}
			set
			{
				this._nCellX = value;
			}
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x0600520B RID: 21003 RVA: 0x00251A99 File Offset: 0x00250A99
		// (set) Token: 0x0600520C RID: 21004 RVA: 0x00251AA1 File Offset: 0x00250AA1
		internal DocumentNode Row
		{
			get
			{
				return this._row;
			}
			set
			{
				this._row = value;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x0600520D RID: 21005 RVA: 0x00251AAA File Offset: 0x00250AAA
		// (set) Token: 0x0600520E RID: 21006 RVA: 0x00251AB2 File Offset: 0x00250AB2
		internal bool IsFilled
		{
			get
			{
				return this._fFilled;
			}
			set
			{
				this._fFilled = value;
			}
		}

		// Token: 0x04002E8F RID: 11919
		private long _nCellX;

		// Token: 0x04002E90 RID: 11920
		private DocumentNode _row;

		// Token: 0x04002E91 RID: 11921
		private bool _fFilled;
	}
}
