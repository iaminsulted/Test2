using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005C8 RID: 1480
	internal class PageFunctionReturnInfo : NavigateInfo
	{
		// Token: 0x06004775 RID: 18293 RVA: 0x00229F3D File Offset: 0x00228F3D
		internal PageFunctionReturnInfo(PageFunctionBase finishingChildPageFunction, Uri source, NavigationMode navigationMode, JournalEntry journalEntry, object returnEventArgs) : base(source, navigationMode, journalEntry)
		{
			this._returnEventArgs = returnEventArgs;
			this._finishingChildPageFunction = finishingChildPageFunction;
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06004776 RID: 18294 RVA: 0x00229F58 File Offset: 0x00228F58
		internal object ReturnEventArgs
		{
			get
			{
				return this._returnEventArgs;
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06004777 RID: 18295 RVA: 0x00229F60 File Offset: 0x00228F60
		internal PageFunctionBase FinishingChildPageFunction
		{
			get
			{
				return this._finishingChildPageFunction;
			}
		}

		// Token: 0x040025D0 RID: 9680
		private object _returnEventArgs;

		// Token: 0x040025D1 RID: 9681
		private PageFunctionBase _finishingChildPageFunction;
	}
}
