using System;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007F7 RID: 2039
	public class UserControl : ContentControl
	{
		// Token: 0x060076B8 RID: 30392 RVA: 0x002F03D0 File Offset: 0x002EF3D0
		static UserControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(typeof(UserControl)));
			UserControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(UserControl));
			UIElement.FocusableProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch));
			Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof(UserControl), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x002F048E File Offset: 0x002EF48E
		internal override void AdjustBranchSource(RoutedEventArgs e)
		{
			e.Source = this;
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x002F0497 File Offset: 0x002EF497
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new UserControlAutomationPeer(this);
		}

		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x060076BC RID: 30396 RVA: 0x002A8831 File Offset: 0x002A7831
		internal override FrameworkElement StateGroupsRoot
		{
			get
			{
				return base.Content as FrameworkElement;
			}
		}

		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x060076BD RID: 30397 RVA: 0x002F049F File Offset: 0x002EF49F
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return UserControl._dType;
			}
		}

		// Token: 0x040038A0 RID: 14496
		private static DependencyObjectType _dType;
	}
}
