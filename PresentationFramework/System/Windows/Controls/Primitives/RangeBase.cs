using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200084C RID: 2124
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	public abstract class RangeBase : Control
	{
		// Token: 0x06007CA9 RID: 31913 RVA: 0x00310750 File Offset: 0x0030F750
		static RangeBase()
		{
			RangeBase.ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(RangeBase));
			RangeBase.MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(RangeBase.OnMinimumChanged)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(1.0, new PropertyChangedCallback(RangeBase.OnMaximumChanged), new CoerceValueCallback(RangeBase.CoerceMaximum)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(RangeBase.OnValueChanged), new CoerceValueCallback(RangeBase.ConstrainToRange)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.LargeChangeProperty = DependencyProperty.Register("LargeChange", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(RangeBase.IsValidChange));
			RangeBase.SmallChangeProperty = DependencyProperty.Register("SmallChange", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.1), new ValidateValueCallback(RangeBase.IsValidChange));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(RangeBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(RangeBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x14000159 RID: 345
		// (add) Token: 0x06007CAB RID: 31915 RVA: 0x00310956 File Offset: 0x0030F956
		// (remove) Token: 0x06007CAC RID: 31916 RVA: 0x00310964 File Offset: 0x0030F964
		[Category("Behavior")]
		public event RoutedPropertyChangedEventHandler<double> ValueChanged
		{
			add
			{
				base.AddHandler(RangeBase.ValueChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeBase.ValueChangedEvent, value);
			}
		}

		// Token: 0x17001CCC RID: 7372
		// (get) Token: 0x06007CAD RID: 31917 RVA: 0x00310972 File Offset: 0x0030F972
		// (set) Token: 0x06007CAE RID: 31918 RVA: 0x00310984 File Offset: 0x0030F984
		[Bindable(true)]
		[Category("Behavior")]
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(RangeBase.MinimumProperty);
			}
			set
			{
				base.SetValue(RangeBase.MinimumProperty, value);
			}
		}

		// Token: 0x06007CAF RID: 31919 RVA: 0x00310998 File Offset: 0x0030F998
		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseMinimumPropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.CoerceValue(RangeBase.MaximumProperty);
			rangeBase.CoerceValue(RangeBase.ValueProperty);
			rangeBase.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x06007CB0 RID: 31920 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
		}

		// Token: 0x06007CB1 RID: 31921 RVA: 0x00310A0C File Offset: 0x0030FA0C
		private static object CoerceMaximum(DependencyObject d, object value)
		{
			double minimum = ((RangeBase)d).Minimum;
			if ((double)value < minimum)
			{
				return minimum;
			}
			return value;
		}

		// Token: 0x17001CCD RID: 7373
		// (get) Token: 0x06007CB2 RID: 31922 RVA: 0x00310A36 File Offset: 0x0030FA36
		// (set) Token: 0x06007CB3 RID: 31923 RVA: 0x00310A48 File Offset: 0x0030FA48
		[Bindable(true)]
		[Category("Behavior")]
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(RangeBase.MaximumProperty);
			}
			set
			{
				base.SetValue(RangeBase.MaximumProperty, value);
			}
		}

		// Token: 0x06007CB4 RID: 31924 RVA: 0x00310A5C File Offset: 0x0030FA5C
		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseMaximumPropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.CoerceValue(RangeBase.ValueProperty);
			rangeBase.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x06007CB5 RID: 31925 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
		}

		// Token: 0x06007CB6 RID: 31926 RVA: 0x00310AC4 File Offset: 0x0030FAC4
		internal static object ConstrainToRange(DependencyObject d, object value)
		{
			RangeBase rangeBase = (RangeBase)d;
			double minimum = rangeBase.Minimum;
			double num = (double)value;
			if (num < minimum)
			{
				return minimum;
			}
			double maximum = rangeBase.Maximum;
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		// Token: 0x17001CCE RID: 7374
		// (get) Token: 0x06007CB7 RID: 31927 RVA: 0x00310B04 File Offset: 0x0030FB04
		// (set) Token: 0x06007CB8 RID: 31928 RVA: 0x00310B16 File Offset: 0x0030FB16
		[Bindable(true)]
		[Category("Behavior")]
		public double Value
		{
			get
			{
				return (double)base.GetValue(RangeBase.ValueProperty);
			}
			set
			{
				base.SetValue(RangeBase.ValueProperty, value);
			}
		}

		// Token: 0x06007CB9 RID: 31929 RVA: 0x00310B2C File Offset: 0x0030FB2C
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseValuePropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.OnValueChanged((double)e.OldValue, (double)e.NewValue);
		}

		// Token: 0x06007CBA RID: 31930 RVA: 0x00310B8C File Offset: 0x0030FB8C
		protected virtual void OnValueChanged(double oldValue, double newValue)
		{
			base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue)
			{
				RoutedEvent = RangeBase.ValueChangedEvent
			});
		}

		// Token: 0x06007CBB RID: 31931 RVA: 0x002E3C88 File Offset: 0x002E2C88
		private static bool IsValidDoubleValue(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && !double.IsInfinity(num);
		}

		// Token: 0x06007CBC RID: 31932 RVA: 0x00310BB4 File Offset: 0x0030FBB4
		private static bool IsValidChange(object value)
		{
			double num = (double)value;
			return RangeBase.IsValidDoubleValue(value) && num >= 0.0;
		}

		// Token: 0x17001CCF RID: 7375
		// (get) Token: 0x06007CBD RID: 31933 RVA: 0x00310BE1 File Offset: 0x0030FBE1
		// (set) Token: 0x06007CBE RID: 31934 RVA: 0x00310BF3 File Offset: 0x0030FBF3
		[Bindable(true)]
		[Category("Behavior")]
		public double LargeChange
		{
			get
			{
				return (double)base.GetValue(RangeBase.LargeChangeProperty);
			}
			set
			{
				base.SetValue(RangeBase.LargeChangeProperty, value);
			}
		}

		// Token: 0x17001CD0 RID: 7376
		// (get) Token: 0x06007CBF RID: 31935 RVA: 0x00310C06 File Offset: 0x0030FC06
		// (set) Token: 0x06007CC0 RID: 31936 RVA: 0x00310C18 File Offset: 0x0030FC18
		[Bindable(true)]
		[Category("Behavior")]
		public double SmallChange
		{
			get
			{
				return (double)base.GetValue(RangeBase.SmallChangeProperty);
			}
			set
			{
				base.SetValue(RangeBase.SmallChangeProperty, value);
			}
		}

		// Token: 0x06007CC1 RID: 31937 RVA: 0x002D817C File Offset: 0x002D717C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06007CC2 RID: 31938 RVA: 0x00310C2C File Offset: 0x0030FC2C
		public override string ToString()
		{
			string text = base.GetType().ToString();
			double min = double.NaN;
			double max = double.NaN;
			double val = double.NaN;
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				min = this.Minimum;
				max = this.Maximum;
				val = this.Value;
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					min = this.Minimum;
					max = this.Maximum;
					val = this.Value;
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_RangeBase", new object[]
				{
					text,
					min,
					max,
					val
				});
			}
			return text;
		}

		// Token: 0x04003A9C RID: 15004
		public static readonly DependencyProperty MinimumProperty;

		// Token: 0x04003A9D RID: 15005
		public static readonly DependencyProperty MaximumProperty;

		// Token: 0x04003A9E RID: 15006
		public static readonly DependencyProperty ValueProperty;

		// Token: 0x04003A9F RID: 15007
		public static readonly DependencyProperty LargeChangeProperty;

		// Token: 0x04003AA0 RID: 15008
		public static readonly DependencyProperty SmallChangeProperty;
	}
}
