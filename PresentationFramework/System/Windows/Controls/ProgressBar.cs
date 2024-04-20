using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007C5 RID: 1989
	[TemplatePart(Name = "PART_Track", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_Indicator", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_GlowRect", Type = typeof(FrameworkElement))]
	public class ProgressBar : RangeBase
	{
		// Token: 0x060071DC RID: 29148 RVA: 0x002DBDE8 File Offset: 0x002DADE8
		static ProgressBar()
		{
			UIElement.FocusableProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
			ProgressBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ProgressBar));
			RangeBase.MaximumProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(100.0));
			Control.ForegroundProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(ProgressBar.OnForegroundChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.ProgressBar);
		}

		// Token: 0x060071DD RID: 29149 RVA: 0x002DBF22 File Offset: 0x002DAF22
		public ProgressBar()
		{
			base.IsVisibleChanged += delegate(object s, DependencyPropertyChangedEventArgs e)
			{
				this.UpdateAnimation();
			};
		}

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x060071DE RID: 29150 RVA: 0x002DBF3C File Offset: 0x002DAF3C
		// (set) Token: 0x060071DF RID: 29151 RVA: 0x002DBF4E File Offset: 0x002DAF4E
		public bool IsIndeterminate
		{
			get
			{
				return (bool)base.GetValue(ProgressBar.IsIndeterminateProperty);
			}
			set
			{
				base.SetValue(ProgressBar.IsIndeterminateProperty, value);
			}
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x002DBF5C File Offset: 0x002DAF5C
		private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProgressBar progressBar = (ProgressBar)d;
			ProgressBarAutomationPeer progressBarAutomationPeer = UIElementAutomationPeer.FromElement(progressBar) as ProgressBarAutomationPeer;
			if (progressBarAutomationPeer != null)
			{
				progressBarAutomationPeer.InvalidatePeer();
			}
			progressBar.SetProgressBarGlowElementBrush();
			progressBar.SetProgressBarIndicatorLength();
			progressBar.UpdateVisualState();
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x002DBF95 File Offset: 0x002DAF95
		private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ProgressBar)d).SetProgressBarGlowElementBrush();
		}

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x060071E2 RID: 29154 RVA: 0x002DBFA2 File Offset: 0x002DAFA2
		// (set) Token: 0x060071E3 RID: 29155 RVA: 0x002DBFB4 File Offset: 0x002DAFB4
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ProgressBar.OrientationProperty);
			}
			set
			{
				base.SetValue(ProgressBar.OrientationProperty, value);
			}
		}

		// Token: 0x060071E4 RID: 29156 RVA: 0x002DBFC8 File Offset: 0x002DAFC8
		internal static bool IsValidOrientation(object o)
		{
			Orientation orientation = (Orientation)o;
			return orientation == Orientation.Horizontal || orientation == Orientation.Vertical;
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x002DBFE5 File Offset: 0x002DAFE5
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ProgressBar)d).SetProgressBarIndicatorLength();
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x002DBFF4 File Offset: 0x002DAFF4
		private void SetProgressBarIndicatorLength()
		{
			if (this._track != null && this._indicator != null)
			{
				double minimum = base.Minimum;
				double maximum = base.Maximum;
				double value = base.Value;
				double num = (this.IsIndeterminate || maximum <= minimum) ? 1.0 : ((value - minimum) / (maximum - minimum));
				this._indicator.Width = num * this._track.ActualWidth;
				this.UpdateAnimation();
			}
		}

		// Token: 0x060071E7 RID: 29159 RVA: 0x002DC064 File Offset: 0x002DB064
		private void SetProgressBarGlowElementBrush()
		{
			if (this._glow == null)
			{
				return;
			}
			this._glow.InvalidateProperty(UIElement.OpacityMaskProperty);
			this._glow.InvalidateProperty(Shape.FillProperty);
			if (this.IsIndeterminate)
			{
				if (base.Foreground is SolidColorBrush)
				{
					Color color = ((SolidColorBrush)base.Foreground).Color;
					LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
					linearGradientBrush.StartPoint = new Point(0.0, 0.0);
					linearGradientBrush.EndPoint = new Point(1.0, 0.0);
					linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.0));
					linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.4));
					linearGradientBrush.GradientStops.Add(new GradientStop(color, 0.6));
					linearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
					this._glow.SetCurrentValue(Shape.FillProperty, linearGradientBrush);
					return;
				}
				LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
				linearGradientBrush2.StartPoint = new Point(0.0, 0.0);
				linearGradientBrush2.EndPoint = new Point(1.0, 0.0);
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Transparent, 0.0));
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Black, 0.4));
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Black, 0.6));
				linearGradientBrush2.GradientStops.Add(new GradientStop(Colors.Transparent, 1.0));
				this._glow.SetCurrentValue(UIElement.OpacityMaskProperty, linearGradientBrush2);
				this._glow.SetCurrentValue(Shape.FillProperty, base.Foreground);
			}
		}

		// Token: 0x060071E8 RID: 29160 RVA: 0x002DC268 File Offset: 0x002DB268
		private void UpdateAnimation()
		{
			if (this._glow != null)
			{
				if (base.IsVisible && this._glow.Width > 0.0 && this._indicator.Width > 0.0)
				{
					double num = this._indicator.Width + this._glow.Width;
					double num2 = -1.0 * this._glow.Width;
					TimeSpan timeSpan = TimeSpan.FromSeconds((double)((int)(num - num2)) / 200.0);
					TimeSpan t = TimeSpan.FromSeconds(1.0);
					TimeSpan value;
					if (DoubleUtil.GreaterThan(this._glow.Margin.Left, num2) && DoubleUtil.LessThan(this._glow.Margin.Left, num - 1.0))
					{
						value = TimeSpan.FromSeconds(-1.0 * (this._glow.Margin.Left - num2) / 200.0);
					}
					else
					{
						value = TimeSpan.Zero;
					}
					ThicknessAnimationUsingKeyFrames thicknessAnimationUsingKeyFrames = new ThicknessAnimationUsingKeyFrames();
					thicknessAnimationUsingKeyFrames.BeginTime = new TimeSpan?(value);
					thicknessAnimationUsingKeyFrames.Duration = new Duration(timeSpan + t);
					thicknessAnimationUsingKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
					thicknessAnimationUsingKeyFrames.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(num2, 0.0, 0.0, 0.0), TimeSpan.FromSeconds(0.0)));
					thicknessAnimationUsingKeyFrames.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(num, 0.0, 0.0, 0.0), timeSpan));
					this._glow.BeginAnimation(FrameworkElement.MarginProperty, thicknessAnimationUsingKeyFrames);
					return;
				}
				this._glow.BeginAnimation(FrameworkElement.MarginProperty, null);
			}
		}

		// Token: 0x060071E9 RID: 29161 RVA: 0x002DC462 File Offset: 0x002DB462
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!this.IsIndeterminate)
			{
				VisualStateManager.GoToState(this, "Determinate", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Indeterminate", useTransitions);
			}
			base.ChangeValidationVisualState(useTransitions);
		}

		// Token: 0x060071EA RID: 29162 RVA: 0x002DC48F File Offset: 0x002DB48F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ProgressBarAutomationPeer(this);
		}

		// Token: 0x060071EB RID: 29163 RVA: 0x002DC497 File Offset: 0x002DB497
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			base.OnMinimumChanged(oldMinimum, newMinimum);
			this.SetProgressBarIndicatorLength();
		}

		// Token: 0x060071EC RID: 29164 RVA: 0x002DC4A7 File Offset: 0x002DB4A7
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			base.OnMaximumChanged(oldMaximum, newMaximum);
			this.SetProgressBarIndicatorLength();
		}

		// Token: 0x060071ED RID: 29165 RVA: 0x002DC4B7 File Offset: 0x002DB4B7
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			this.SetProgressBarIndicatorLength();
		}

		// Token: 0x060071EE RID: 29166 RVA: 0x002DC4C8 File Offset: 0x002DB4C8
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._track != null)
			{
				this._track.SizeChanged -= this.OnTrackSizeChanged;
			}
			this._track = (base.GetTemplateChild("PART_Track") as FrameworkElement);
			this._indicator = (base.GetTemplateChild("PART_Indicator") as FrameworkElement);
			this._glow = (base.GetTemplateChild("PART_GlowRect") as FrameworkElement);
			if (this._track != null)
			{
				this._track.SizeChanged += this.OnTrackSizeChanged;
			}
			if (this.IsIndeterminate)
			{
				this.SetProgressBarGlowElementBrush();
			}
		}

		// Token: 0x060071EF RID: 29167 RVA: 0x002DC569 File Offset: 0x002DB569
		private void OnTrackSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.SetProgressBarIndicatorLength();
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x060071F0 RID: 29168 RVA: 0x002DC571 File Offset: 0x002DB571
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ProgressBar._dType;
			}
		}

		// Token: 0x04003741 RID: 14145
		public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(ProgressBar), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(ProgressBar.OnIsIndeterminateChanged)));

		// Token: 0x04003742 RID: 14146
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ProgressBar), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ProgressBar.OnOrientationChanged)), new ValidateValueCallback(ProgressBar.IsValidOrientation));

		// Token: 0x04003743 RID: 14147
		private const string TrackTemplateName = "PART_Track";

		// Token: 0x04003744 RID: 14148
		private const string IndicatorTemplateName = "PART_Indicator";

		// Token: 0x04003745 RID: 14149
		private const string GlowingRectTemplateName = "PART_GlowRect";

		// Token: 0x04003746 RID: 14150
		private FrameworkElement _track;

		// Token: 0x04003747 RID: 14151
		private FrameworkElement _indicator;

		// Token: 0x04003748 RID: 14152
		private FrameworkElement _glow;

		// Token: 0x04003749 RID: 14153
		private static DependencyObjectType _dType;
	}
}
