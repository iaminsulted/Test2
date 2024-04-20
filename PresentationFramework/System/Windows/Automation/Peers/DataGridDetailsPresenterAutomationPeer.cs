using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000553 RID: 1363
	public sealed class DataGridDetailsPresenterAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06004360 RID: 17248 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public DataGridDetailsPresenterAutomationPeer(DataGridDetailsPresenter owner) : base(owner)
		{
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}
	}
}
