using System;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200056E RID: 1390
	public class HyperlinkAutomationPeer : TextElementAutomationPeer, IInvokeProvider
	{
		// Token: 0x06004471 RID: 17521 RVA: 0x002213E3 File Offset: 0x002203E3
		public HyperlinkAutomationPeer(Hyperlink owner) : base(owner)
		{
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x002213EC File Offset: 0x002203EC
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x001FC136 File Offset: 0x001FB136
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Hyperlink;
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x002213FC File Offset: 0x002203FC
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (text == string.Empty)
			{
				text = ((Hyperlink)base.Owner).Text;
				if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x00221438 File Offset: 0x00220438
		protected override string GetClassNameCore()
		{
			return "Hyperlink";
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x00221440 File Offset: 0x00220440
		protected override bool IsControlElementCore()
		{
			if (!base.IncludeInvisibleElementsInControlView)
			{
				bool? isTextViewVisible = base.IsTextViewVisible;
				bool flag = true;
				return isTextViewVisible.GetValueOrDefault() == flag & isTextViewVisible != null;
			}
			return true;
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x00221472 File Offset: 0x00220472
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((Hyperlink)base.Owner).DoClick();
		}
	}
}
