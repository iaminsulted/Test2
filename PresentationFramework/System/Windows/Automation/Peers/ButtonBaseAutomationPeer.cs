using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000545 RID: 1349
	public abstract class ButtonBaseAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x0600429F RID: 17055 RVA: 0x0021BBD9 File Offset: 0x0021ABD9
		protected ButtonBaseAutomationPeer(ButtonBase owner) : base(owner)
		{
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x0021BBE4 File Offset: 0x0021ABE4
		protected override string GetAcceleratorKeyCore()
		{
			string text = base.GetAcceleratorKeyCore();
			if (text == string.Empty)
			{
				RoutedUICommand routedUICommand = ((ButtonBase)base.Owner).Command as RoutedUICommand;
				if (routedUICommand != null && !string.IsNullOrEmpty(routedUICommand.Text))
				{
					text = routedUICommand.Text;
				}
			}
			return text;
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x0021BC34 File Offset: 0x0021AC34
		protected override string GetAutomationIdCore()
		{
			string text = base.GetAutomationIdCore();
			if (string.IsNullOrEmpty(text))
			{
				RoutedCommand routedCommand = ((ButtonBase)base.Owner).Command as RoutedCommand;
				if (routedCommand != null)
				{
					string name = routedCommand.Name;
					if (!string.IsNullOrEmpty(name))
					{
						text = name;
					}
				}
			}
			return text ?? string.Empty;
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x0021BC84 File Offset: 0x0021AC84
		protected override string GetNameCore()
		{
			string text = base.GetNameCore();
			ButtonBase buttonBase = (ButtonBase)base.Owner;
			if (!string.IsNullOrEmpty(text))
			{
				if (buttonBase.Content is string)
				{
					text = AccessText.RemoveAccessKeyMarker(text);
				}
			}
			else
			{
				RoutedUICommand routedUICommand = buttonBase.Command as RoutedUICommand;
				if (routedUICommand != null && !string.IsNullOrEmpty(routedUICommand.Text))
				{
					text = routedUICommand.Text;
				}
			}
			return text;
		}
	}
}
