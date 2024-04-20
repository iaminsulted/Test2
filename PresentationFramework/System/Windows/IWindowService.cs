using System;

namespace System.Windows
{
	// Token: 0x0200037A RID: 890
	internal interface IWindowService
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x0600240A RID: 9226
		// (set) Token: 0x0600240B RID: 9227
		string Title { get; set; }

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x0600240C RID: 9228
		// (set) Token: 0x0600240D RID: 9229
		double Height { get; set; }

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x0600240E RID: 9230
		// (set) Token: 0x0600240F RID: 9231
		double Width { get; set; }

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002410 RID: 9232
		bool UserResized { get; }
	}
}
