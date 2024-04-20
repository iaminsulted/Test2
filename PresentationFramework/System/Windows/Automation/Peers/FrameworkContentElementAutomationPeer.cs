using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000563 RID: 1379
	public class FrameworkContentElementAutomationPeer : ContentElementAutomationPeer
	{
		// Token: 0x06004415 RID: 17429 RVA: 0x002201F6 File Offset: 0x0021F1F6
		public FrameworkContentElementAutomationPeer(FrameworkContentElement owner) : base(owner)
		{
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x00220200 File Offset: 0x0021F200
		protected override string GetAutomationIdCore()
		{
			string text = base.GetAutomationIdCore();
			if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text))
			{
				text = ((FrameworkContentElement)base.Owner).Name;
			}
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x00220240 File Offset: 0x0021F240
		protected override string GetHelpTextCore()
		{
			string text = base.GetHelpTextCore();
			if (string.IsNullOrEmpty(text))
			{
				object toolTip = ((FrameworkContentElement)base.Owner).ToolTip;
				if (toolTip != null)
				{
					text = (toolTip as string);
					if (string.IsNullOrEmpty(text))
					{
						FrameworkElement frameworkElement = toolTip as FrameworkElement;
						if (frameworkElement != null)
						{
							text = frameworkElement.GetPlainText();
						}
					}
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x0022029C File Offset: 0x0021F29C
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer labeledByCore = base.GetLabeledByCore();
			if (labeledByCore == null)
			{
				Label labeledBy = Label.GetLabeledBy(base.Owner);
				if (labeledBy != null)
				{
					return labeledBy.GetAutomationPeer();
				}
			}
			return labeledByCore;
		}
	}
}
