using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Markup;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007A5 RID: 1957
	[Localizability(LocalizationCategory.Label)]
	public class Label : ContentControl
	{
		// Token: 0x06006E7B RID: 28283 RVA: 0x002D1C10 File Offset: 0x002D0C10
		static Label()
		{
			EventManager.RegisterClassHandler(typeof(Label), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(Label.OnAccessKeyPressed));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(typeof(Label)));
			Label._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Label));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Label), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.Label);
		}

		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x06006E7D RID: 28285 RVA: 0x002D1D19 File Offset: 0x002D0D19
		// (set) Token: 0x06006E7E RID: 28286 RVA: 0x002D1D2B File Offset: 0x002D0D2B
		[TypeConverter(typeof(NameReferenceConverter))]
		public UIElement Target
		{
			get
			{
				return (UIElement)base.GetValue(Label.TargetProperty);
			}
			set
			{
				base.SetValue(Label.TargetProperty, value);
			}
		}

		// Token: 0x06006E7F RID: 28287 RVA: 0x002D1D3C File Offset: 0x002D0D3C
		private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Label label = (Label)d;
			UIElement uielement = (UIElement)e.OldValue;
			UIElement uielement2 = (UIElement)e.NewValue;
			if (uielement != null && uielement.GetValue(Label.LabeledByProperty) == label)
			{
				uielement.ClearValue(Label.LabeledByProperty);
			}
			if (uielement2 != null)
			{
				uielement2.SetValue(Label.LabeledByProperty, label);
			}
		}

		// Token: 0x06006E80 RID: 28288 RVA: 0x002D1D95 File Offset: 0x002D0D95
		internal static Label GetLabeledBy(DependencyObject o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			return (Label)o.GetValue(Label.LabeledByProperty);
		}

		// Token: 0x06006E81 RID: 28289 RVA: 0x002D1DB5 File Offset: 0x002D0DB5
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new LabelAutomationPeer(this);
		}

		// Token: 0x06006E82 RID: 28290 RVA: 0x002D1DC0 File Offset: 0x002D0DC0
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			Label label = sender as Label;
			if (!e.Handled && e.Scope == null && (e.Target == null || e.Target == label))
			{
				e.Target = label.Target;
			}
		}

		// Token: 0x17001984 RID: 6532
		// (get) Token: 0x06006E83 RID: 28291 RVA: 0x002D1E01 File Offset: 0x002D0E01
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Label._dType;
			}
		}

		// Token: 0x04003658 RID: 13912
		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(Label), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Label.OnTargetChanged)));

		// Token: 0x04003659 RID: 13913
		private static readonly DependencyProperty LabeledByProperty = DependencyProperty.RegisterAttached("LabeledBy", typeof(Label), typeof(Label), new FrameworkPropertyMetadata(null));

		// Token: 0x0400365A RID: 13914
		private static DependencyObjectType _dType;
	}
}
