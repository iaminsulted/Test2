using System;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200078D RID: 1933
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class GroupBox : HeaderedContentControl
	{
		// Token: 0x06006B48 RID: 27464 RVA: 0x002C597C File Offset: 0x002C497C
		static GroupBox()
		{
			UIElement.FocusableProperty.OverrideMetadata(typeof(GroupBox), new FrameworkPropertyMetadata(false));
			Control.IsTabStopProperty.OverrideMetadata(typeof(GroupBox), new FrameworkPropertyMetadata(false));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupBox), new FrameworkPropertyMetadata(typeof(GroupBox)));
			EventManager.RegisterClassHandler(typeof(GroupBox), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(GroupBox.OnAccessKeyPressed));
			ControlsTraceLogger.AddControl(TelemetryControls.GroupBox);
		}

		// Token: 0x06006B49 RID: 27465 RVA: 0x002C5A15 File Offset: 0x002C4A15
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GroupBoxAutomationPeer(this);
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x002C5A1D File Offset: 0x002C4A1D
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x002C5A2C File Offset: 0x002C4A2C
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!e.Handled && e.Scope == null && e.Target == null)
			{
				e.Target = (sender as GroupBox);
			}
		}
	}
}
