using System;

namespace System.Windows
{
	// Token: 0x02000392 RID: 914
	internal class ResourcesChangedEventArgs : EventArgs
	{
		// Token: 0x0600250F RID: 9487 RVA: 0x00185905 File Offset: 0x00184905
		internal ResourcesChangedEventArgs(ResourcesChangeInfo info)
		{
			this._info = info;
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x00185914 File Offset: 0x00184914
		internal ResourcesChangeInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x04001173 RID: 4467
		private ResourcesChangeInfo _info;
	}
}
