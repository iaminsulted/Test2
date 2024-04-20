using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x0200022C RID: 556
	internal struct LivePropertyInfo
	{
		// Token: 0x0600151E RID: 5406 RVA: 0x001541EF File Offset: 0x001531EF
		public LivePropertyInfo(string path, DependencyProperty dp)
		{
			this._path = path;
			this._dp = dp;
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x001541FF File Offset: 0x001531FF
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x00154207 File Offset: 0x00153207
		public DependencyProperty Property
		{
			get
			{
				return this._dp;
			}
		}

		// Token: 0x04000BF6 RID: 3062
		private string _path;

		// Token: 0x04000BF7 RID: 3063
		private DependencyProperty _dp;
	}
}
