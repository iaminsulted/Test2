using System;

namespace System.Windows.Documents
{
	// Token: 0x02000676 RID: 1654
	internal class ListLevel
	{
		// Token: 0x06005192 RID: 20882 RVA: 0x0024FD9A File Offset: 0x0024ED9A
		internal ListLevel()
		{
			this._nStartIndex = 1L;
			this._numberType = MarkerStyle.MarkerArabic;
		}

		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x0024FDB1 File Offset: 0x0024EDB1
		// (set) Token: 0x06005194 RID: 20884 RVA: 0x0024FDB9 File Offset: 0x0024EDB9
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

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x0024FDC2 File Offset: 0x0024EDC2
		// (set) Token: 0x06005196 RID: 20886 RVA: 0x0024FDCA File Offset: 0x0024EDCA
		internal MarkerStyle Marker
		{
			get
			{
				return this._numberType;
			}
			set
			{
				this._numberType = value;
			}
		}

		// Token: 0x1700133A RID: 4922
		// (set) Token: 0x06005197 RID: 20887 RVA: 0x0024FDD3 File Offset: 0x0024EDD3
		internal FormatState FormatState
		{
			set
			{
				this._formatState = value;
			}
		}

		// Token: 0x04002E70 RID: 11888
		private long _nStartIndex;

		// Token: 0x04002E71 RID: 11889
		private MarkerStyle _numberType;

		// Token: 0x04002E72 RID: 11890
		private FormatState _formatState;
	}
}
