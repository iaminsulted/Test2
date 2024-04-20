using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200072D RID: 1837
	[DefaultEvent("CheckStateChanged")]
	[Localizability(LocalizationCategory.CheckBox)]
	public class CheckBox : ToggleButton
	{
		// Token: 0x0600609E RID: 24734 RVA: 0x0029A248 File Offset: 0x00299248
		static CheckBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBox), new FrameworkPropertyMetadata(typeof(CheckBox)));
			CheckBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(CheckBox));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(CheckBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.CheckBox);
		}

		// Token: 0x060060A0 RID: 24736 RVA: 0x0029A2BA File Offset: 0x002992BA
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CheckBoxAutomationPeer(this);
		}

		// Token: 0x060060A1 RID: 24737 RVA: 0x0029A2C4 File Offset: 0x002992C4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!base.IsThreeState)
			{
				if (e.Key == Key.OemPlus || e.Key == Key.Add)
				{
					e.Handled = true;
					base.ClearValue(ButtonBase.IsPressedPropertyKey);
					base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.TrueBox);
					return;
				}
				if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
				{
					e.Handled = true;
					base.ClearValue(ButtonBase.IsPressedPropertyKey);
					base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.FalseBox);
				}
			}
		}

		// Token: 0x060060A2 RID: 24738 RVA: 0x0029A353 File Offset: 0x00299353
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			if (!base.IsKeyboardFocused)
			{
				base.Focus();
			}
			base.OnAccessKey(e);
		}

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x060060A3 RID: 24739 RVA: 0x0029A36B File Offset: 0x0029936B
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return CheckBox._dType;
			}
		}

		// Token: 0x0400323A RID: 12858
		private static DependencyObjectType _dType;
	}
}
