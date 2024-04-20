using System;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007D6 RID: 2006
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Separator : Control
	{
		// Token: 0x060072F5 RID: 29429 RVA: 0x002E115C File Offset: 0x002E015C
		static Separator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(typeof(Separator)));
			Separator._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Separator));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.Separator);
		}

		// Token: 0x060072F6 RID: 29430 RVA: 0x002E11C9 File Offset: 0x002E01C9
		internal static void PrepareContainer(Control container)
		{
			if (container != null)
			{
				container.IsEnabled = false;
				container.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			}
		}

		// Token: 0x060072F7 RID: 29431 RVA: 0x002E11DC File Offset: 0x002E01DC
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new SeparatorAutomationPeer(this);
		}

		// Token: 0x17001AA5 RID: 6821
		// (get) Token: 0x060072F8 RID: 29432 RVA: 0x002E11E4 File Offset: 0x002E01E4
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Separator._dType;
			}
		}

		// Token: 0x040037AD RID: 14253
		private static DependencyObjectType _dType;
	}
}
