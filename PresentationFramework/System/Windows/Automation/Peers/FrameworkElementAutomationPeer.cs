using System;
using System.Windows.Data;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000564 RID: 1380
	public class FrameworkElementAutomationPeer : UIElementAutomationPeer
	{
		// Token: 0x06004419 RID: 17433 RVA: 0x002202CA File Offset: 0x0021F2CA
		public FrameworkElementAutomationPeer(FrameworkElement owner) : base(owner)
		{
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x002202D4 File Offset: 0x0021F2D4
		protected override string GetAutomationIdCore()
		{
			string text = base.GetAutomationIdCore();
			if (string.IsNullOrEmpty(text))
			{
				FrameworkElement frameworkElement = (FrameworkElement)base.Owner;
				text = base.Owner.Uid;
				if (string.IsNullOrEmpty(text))
				{
					text = frameworkElement.Name;
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x00220324 File Offset: 0x0021F324
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			if (string.IsNullOrEmpty(text))
			{
				AutomationPeer labeledByCore = this.GetLabeledByCore();
				if (labeledByCore != null)
				{
					text = labeledByCore.GetName();
				}
				if (string.IsNullOrEmpty(text))
				{
					text = ((FrameworkElement)base.Owner).GetPlainText();
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x00220374 File Offset: 0x0021F374
		protected override string GetHelpTextCore()
		{
			string text = base.GetHelpTextCore();
			if (string.IsNullOrEmpty(text))
			{
				object toolTip = ((FrameworkElement)base.Owner).ToolTip;
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

		// Token: 0x0600441D RID: 17437 RVA: 0x002203D0 File Offset: 0x0021F3D0
		internal override bool IgnoreUpdatePeer()
		{
			DependencyObject owner = base.Owner;
			return owner == null || owner.GetValue(FrameworkElement.DataContextProperty) == BindingOperations.DisconnectedSource || base.IgnoreUpdatePeer();
		}
	}
}
