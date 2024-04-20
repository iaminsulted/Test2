using System;

namespace System.Windows.Documents
{
	// Token: 0x02000670 RID: 1648
	internal class MarkerListEntry
	{
		// Token: 0x06005157 RID: 20823 RVA: 0x0024F1A7 File Offset: 0x0024E1A7
		internal MarkerListEntry()
		{
			this._marker = MarkerStyle.MarkerBullet;
			this._nILS = -1L;
			this._nStartIndexOverride = -1L;
			this._nStartIndexDefault = -1L;
			this._nVirtualListLevel = -1L;
		}

		// Token: 0x17001321 RID: 4897
		// (get) Token: 0x06005158 RID: 20824 RVA: 0x0024F1D7 File Offset: 0x0024E1D7
		// (set) Token: 0x06005159 RID: 20825 RVA: 0x0024F1DF File Offset: 0x0024E1DF
		internal MarkerStyle Marker
		{
			get
			{
				return this._marker;
			}
			set
			{
				this._marker = value;
			}
		}

		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x0600515A RID: 20826 RVA: 0x0024F1E8 File Offset: 0x0024E1E8
		// (set) Token: 0x0600515B RID: 20827 RVA: 0x0024F1F0 File Offset: 0x0024E1F0
		internal long StartIndexOverride
		{
			get
			{
				return this._nStartIndexOverride;
			}
			set
			{
				this._nStartIndexOverride = value;
			}
		}

		// Token: 0x17001323 RID: 4899
		// (get) Token: 0x0600515C RID: 20828 RVA: 0x0024F1F9 File Offset: 0x0024E1F9
		// (set) Token: 0x0600515D RID: 20829 RVA: 0x0024F201 File Offset: 0x0024E201
		internal long StartIndexDefault
		{
			get
			{
				return this._nStartIndexDefault;
			}
			set
			{
				this._nStartIndexDefault = value;
			}
		}

		// Token: 0x17001324 RID: 4900
		// (get) Token: 0x0600515E RID: 20830 RVA: 0x0024F20A File Offset: 0x0024E20A
		// (set) Token: 0x0600515F RID: 20831 RVA: 0x0024F212 File Offset: 0x0024E212
		internal long VirtualListLevel
		{
			get
			{
				return this._nVirtualListLevel;
			}
			set
			{
				this._nVirtualListLevel = value;
			}
		}

		// Token: 0x17001325 RID: 4901
		// (get) Token: 0x06005160 RID: 20832 RVA: 0x0024F21B File Offset: 0x0024E21B
		internal long StartIndexToUse
		{
			get
			{
				if (this._nStartIndexOverride <= 0L)
				{
					return this._nStartIndexDefault;
				}
				return this._nStartIndexOverride;
			}
		}

		// Token: 0x17001326 RID: 4902
		// (get) Token: 0x06005161 RID: 20833 RVA: 0x0024F234 File Offset: 0x0024E234
		// (set) Token: 0x06005162 RID: 20834 RVA: 0x0024F23C File Offset: 0x0024E23C
		internal long ILS
		{
			get
			{
				return this._nILS;
			}
			set
			{
				this._nILS = value;
			}
		}

		// Token: 0x04002E61 RID: 11873
		private MarkerStyle _marker;

		// Token: 0x04002E62 RID: 11874
		private long _nStartIndexOverride;

		// Token: 0x04002E63 RID: 11875
		private long _nStartIndexDefault;

		// Token: 0x04002E64 RID: 11876
		private long _nVirtualListLevel;

		// Token: 0x04002E65 RID: 11877
		private long _nILS;
	}
}
