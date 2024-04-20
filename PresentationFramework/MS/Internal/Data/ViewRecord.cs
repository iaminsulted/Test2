using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x02000247 RID: 583
	internal class ViewRecord
	{
		// Token: 0x0600167A RID: 5754 RVA: 0x0015AC84 File Offset: 0x00159C84
		internal ViewRecord(ICollectionView view)
		{
			this._view = view;
			this._version = -1;
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x0015AC9A File Offset: 0x00159C9A
		internal ICollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x0015ACA2 File Offset: 0x00159CA2
		// (set) Token: 0x0600167D RID: 5757 RVA: 0x0015ACAA File Offset: 0x00159CAA
		internal int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600167E RID: 5758 RVA: 0x0015ACB3 File Offset: 0x00159CB3
		internal bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x0015ACBB File Offset: 0x00159CBB
		internal void InitializeView()
		{
			this._view.MoveCurrentToFirst();
			this._isInitialized = true;
		}

		// Token: 0x04000C5F RID: 3167
		private ICollectionView _view;

		// Token: 0x04000C60 RID: 3168
		private int _version;

		// Token: 0x04000C61 RID: 3169
		private bool _isInitialized;
	}
}
