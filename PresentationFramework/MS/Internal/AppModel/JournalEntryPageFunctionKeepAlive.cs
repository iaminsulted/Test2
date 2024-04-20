using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000286 RID: 646
	internal class JournalEntryPageFunctionKeepAlive : JournalEntryPageFunction
	{
		// Token: 0x0600187C RID: 6268 RVA: 0x00160D2A File Offset: 0x0015FD2A
		internal JournalEntryPageFunctionKeepAlive(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
			this._keepAlivePageFunction = pageFunction;
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsPageFunction()
		{
			return true;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x00160D3B File Offset: 0x0015FD3B
		internal override bool IsAlive()
		{
			return this.KeepAlivePageFunction != null;
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x00160D46 File Offset: 0x0015FD46
		internal PageFunctionBase KeepAlivePageFunction
		{
			get
			{
				return this._keepAlivePageFunction;
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00160D4E File Offset: 0x0015FD4E
		internal override PageFunctionBase ResumePageFunction()
		{
			PageFunctionBase keepAlivePageFunction = this.KeepAlivePageFunction;
			keepAlivePageFunction._Resume = true;
			return keepAlivePageFunction;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00160D5D File Offset: 0x0015FD5D
		internal override void SaveState(object contentObject)
		{
			Invariant.Assert(this._keepAlivePageFunction == contentObject);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00160D70 File Offset: 0x0015FD70
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			PageFunctionBase content = (navigator.Content == this._keepAlivePageFunction) ? this._keepAlivePageFunction : this.ResumePageFunction();
			return navigator.Navigate(content, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x04000D54 RID: 3412
		private PageFunctionBase _keepAlivePageFunction;
	}
}
