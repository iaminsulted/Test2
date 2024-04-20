using System;
using System.Windows.Threading;

namespace System.Windows.Interop
{
	// Token: 0x02000423 RID: 1059
	public interface IErrorPage
	{
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600333A RID: 13114
		// (set) Token: 0x0600333B RID: 13115
		Uri DeploymentPath { get; set; }

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600333C RID: 13116
		// (set) Token: 0x0600333D RID: 13117
		string ErrorTitle { get; set; }

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600333E RID: 13118
		// (set) Token: 0x0600333F RID: 13119
		string ErrorText { get; set; }

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003340 RID: 13120
		// (set) Token: 0x06003341 RID: 13121
		bool ErrorFlag { get; set; }

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003342 RID: 13122
		// (set) Token: 0x06003343 RID: 13123
		string LogFilePath { get; set; }

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003344 RID: 13124
		// (set) Token: 0x06003345 RID: 13125
		Uri SupportUri { get; set; }

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003346 RID: 13126
		// (set) Token: 0x06003347 RID: 13127
		DispatcherOperationCallback RefreshCallback { get; set; }

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003348 RID: 13128
		// (set) Token: 0x06003349 RID: 13129
		DispatcherOperationCallback GetWinFxCallback { get; set; }
	}
}
