using System;

namespace System.Windows.Data
{
	// Token: 0x02000457 RID: 1111
	public class CollectionViewRegisteringEventArgs : EventArgs
	{
		// Token: 0x06003857 RID: 14423 RVA: 0x001E8BED File Offset: 0x001E7BED
		internal CollectionViewRegisteringEventArgs(CollectionView view)
		{
			this._view = view;
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06003858 RID: 14424 RVA: 0x001E8BFC File Offset: 0x001E7BFC
		public CollectionView CollectionView
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x04001D26 RID: 7462
		private CollectionView _view;
	}
}
