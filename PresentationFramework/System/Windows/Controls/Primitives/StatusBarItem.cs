using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000857 RID: 2135
	[Localizability(LocalizationCategory.Inherit)]
	public class StatusBarItem : ContentControl
	{
		// Token: 0x06007D8E RID: 32142 RVA: 0x00314614 File Offset: 0x00313614
		static StatusBarItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(typeof(StatusBarItem)));
			StatusBarItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(StatusBarItem));
			Control.IsTabStopProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x06007D8F RID: 32143 RVA: 0x00314695 File Offset: 0x00313695
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new StatusBarItemAutomationPeer(this);
		}

		// Token: 0x17001CF6 RID: 7414
		// (get) Token: 0x06007D90 RID: 32144 RVA: 0x0031469D File Offset: 0x0031369D
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return StatusBarItem._dType;
			}
		}

		// Token: 0x04003AF5 RID: 15093
		private static DependencyObjectType _dType;
	}
}
