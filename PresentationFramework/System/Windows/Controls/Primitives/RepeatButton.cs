using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Threading;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200084E RID: 2126
	public class RepeatButton : ButtonBase
	{
		// Token: 0x06007CC3 RID: 31939 RVA: 0x00310D2C File Offset: 0x0030FD2C
		static RepeatButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RepeatButton), new FrameworkPropertyMetadata(typeof(RepeatButton)));
			RepeatButton._dType = DependencyObjectType.FromSystemTypeInternal(typeof(RepeatButton));
			ButtonBase.ClickModeProperty.OverrideMetadata(typeof(RepeatButton), new FrameworkPropertyMetadata(ClickMode.Press));
		}

		// Token: 0x17001CD1 RID: 7377
		// (get) Token: 0x06007CC5 RID: 31941 RVA: 0x00310E0B File Offset: 0x0030FE0B
		// (set) Token: 0x06007CC6 RID: 31942 RVA: 0x00310E1D File Offset: 0x0030FE1D
		[Bindable(true)]
		[Category("Behavior")]
		public int Delay
		{
			get
			{
				return (int)base.GetValue(RepeatButton.DelayProperty);
			}
			set
			{
				base.SetValue(RepeatButton.DelayProperty, value);
			}
		}

		// Token: 0x17001CD2 RID: 7378
		// (get) Token: 0x06007CC7 RID: 31943 RVA: 0x00310E30 File Offset: 0x0030FE30
		// (set) Token: 0x06007CC8 RID: 31944 RVA: 0x00310E42 File Offset: 0x0030FE42
		[Bindable(true)]
		[Category("Behavior")]
		public int Interval
		{
			get
			{
				return (int)base.GetValue(RepeatButton.IntervalProperty);
			}
			set
			{
				base.SetValue(RepeatButton.IntervalProperty, value);
			}
		}

		// Token: 0x06007CC9 RID: 31945 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool IsDelayValid(object value)
		{
			return (int)value >= 0;
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x0024392A File Offset: 0x0024292A
		private static bool IsIntervalValid(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x00310E58 File Offset: 0x0030FE58
		private void StartTimer()
		{
			if (this._timer == null)
			{
				this._timer = new DispatcherTimer();
				this._timer.Tick += this.OnTimeout;
			}
			else if (this._timer.IsEnabled)
			{
				return;
			}
			this._timer.Interval = TimeSpan.FromMilliseconds((double)this.Delay);
			this._timer.Start();
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x00310EC1 File Offset: 0x0030FEC1
		private void StopTimer()
		{
			if (this._timer != null)
			{
				this._timer.Stop();
			}
		}

		// Token: 0x06007CCD RID: 31949 RVA: 0x00310ED8 File Offset: 0x0030FED8
		private void OnTimeout(object sender, EventArgs e)
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)this.Interval);
			if (this._timer.Interval != timeSpan)
			{
				this._timer.Interval = timeSpan;
			}
			if (base.IsPressed)
			{
				this.OnClick();
			}
		}

		// Token: 0x06007CCE RID: 31950 RVA: 0x00310F20 File Offset: 0x0030FF20
		internal static int GetKeyboardDelay()
		{
			int num = SystemParameters.KeyboardDelay;
			if (num < 0 || num > 3)
			{
				num = 0;
			}
			return (num + 1) * 250;
		}

		// Token: 0x06007CCF RID: 31951 RVA: 0x00310F48 File Offset: 0x0030FF48
		internal static int GetKeyboardSpeed()
		{
			int num = SystemParameters.KeyboardSpeed;
			if (num < 0 || num > 31)
			{
				num = 31;
			}
			return (31 - num) * 367 / 31 + 33;
		}

		// Token: 0x06007CD0 RID: 31952 RVA: 0x00310F77 File Offset: 0x0030FF77
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RepeatButtonAutomationPeer(this);
		}

		// Token: 0x06007CD1 RID: 31953 RVA: 0x002C33D4 File Offset: 0x002C23D4
		protected override void OnClick()
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			base.OnClick();
		}

		// Token: 0x06007CD2 RID: 31954 RVA: 0x00310F7F File Offset: 0x0030FF7F
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (base.IsPressed && base.ClickMode != ClickMode.Hover)
			{
				this.StartTimer();
			}
		}

		// Token: 0x06007CD3 RID: 31955 RVA: 0x00310F9F File Offset: 0x0030FF9F
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			if (base.ClickMode != ClickMode.Hover)
			{
				this.StopTimer();
			}
		}

		// Token: 0x06007CD4 RID: 31956 RVA: 0x00310FB7 File Offset: 0x0030FFB7
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			this.StopTimer();
		}

		// Token: 0x06007CD5 RID: 31957 RVA: 0x00310FC6 File Offset: 0x0030FFC6
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06007CD6 RID: 31958 RVA: 0x00310FDE File Offset: 0x0030FFDE
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06007CD7 RID: 31959 RVA: 0x00310FF6 File Offset: 0x0030FFF6
		private bool HandleIsMouseOverChanged()
		{
			if (base.ClickMode == ClickMode.Hover)
			{
				if (base.IsMouseOver)
				{
					this.StartTimer();
				}
				else
				{
					this.StopTimer();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06007CD8 RID: 31960 RVA: 0x0031101A File Offset: 0x0031001A
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Space && base.ClickMode != ClickMode.Hover)
			{
				this.StartTimer();
			}
		}

		// Token: 0x06007CD9 RID: 31961 RVA: 0x0031103C File Offset: 0x0031003C
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.Key == Key.Space && base.ClickMode != ClickMode.Hover)
			{
				this.StopTimer();
			}
			base.OnKeyUp(e);
		}

		// Token: 0x17001CD3 RID: 7379
		// (get) Token: 0x06007CDA RID: 31962 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x17001CD4 RID: 7380
		// (get) Token: 0x06007CDB RID: 31963 RVA: 0x0031105E File Offset: 0x0031005E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return RepeatButton._dType;
			}
		}

		// Token: 0x04003AA6 RID: 15014
		public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(RepeatButton), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardDelay()), new ValidateValueCallback(RepeatButton.IsDelayValid));

		// Token: 0x04003AA7 RID: 15015
		public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(RepeatButton), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardSpeed()), new ValidateValueCallback(RepeatButton.IsIntervalValid));

		// Token: 0x04003AA8 RID: 15016
		private DispatcherTimer _timer;

		// Token: 0x04003AA9 RID: 15017
		private static DependencyObjectType _dType;
	}
}
