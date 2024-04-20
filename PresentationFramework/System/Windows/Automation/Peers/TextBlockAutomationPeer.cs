using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000597 RID: 1431
	public class TextBlockAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060045B5 RID: 17845 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		public TextBlockAutomationPeer(TextBlock owner) : base(owner)
		{
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x002246B4 File Offset: 0x002236B4
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> result = null;
			TextBlock textBlock = (TextBlock)base.Owner;
			if (textBlock.HasComplexContent)
			{
				result = TextContainerHelper.GetAutomationPeersFromRange(textBlock.TextContainer.Start, textBlock.TextContainer.End, null);
			}
			return result;
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x001FB806 File Offset: 0x001FA806
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Text;
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x002246F5 File Offset: 0x002236F5
		protected override string GetClassNameCore()
		{
			return "TextBlock";
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x002246FC File Offset: 0x002236FC
		protected override bool IsControlElementCore()
		{
			DependencyObject templatedParent = ((TextBlock)base.Owner).TemplatedParent;
			return (templatedParent == null || templatedParent is ContentPresenter) && base.IsControlElementCore();
		}
	}
}
