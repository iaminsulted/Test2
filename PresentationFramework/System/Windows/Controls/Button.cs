using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000720 RID: 1824
	public class Button : ButtonBase
	{
		// Token: 0x06005FE1 RID: 24545 RVA: 0x00297530 File Offset: 0x00296530
		static Button()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
			Button._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Button));
			if (ButtonBase.CommandProperty != null)
			{
				UIElement.IsEnabledProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(new PropertyChangedCallback(Button.OnIsEnabledChanged)));
			}
			ControlsTraceLogger.AddControl(TelemetryControls.Button);
		}

		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x06005FE3 RID: 24547 RVA: 0x00297667 File Offset: 0x00296667
		// (set) Token: 0x06005FE4 RID: 24548 RVA: 0x00297679 File Offset: 0x00296679
		public bool IsDefault
		{
			get
			{
				return (bool)base.GetValue(Button.IsDefaultProperty);
			}
			set
			{
				base.SetValue(Button.IsDefaultProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x0029768C File Offset: 0x0029668C
		private static void OnIsDefaultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = d as Button;
			KeyboardFocusChangedEventHandler keyboardFocusChangedEventHandler = Button.FocusChangedEventHandlerField.GetValue(button);
			if (keyboardFocusChangedEventHandler == null)
			{
				keyboardFocusChangedEventHandler = new KeyboardFocusChangedEventHandler(button.OnFocusChanged);
				Button.FocusChangedEventHandlerField.SetValue(button, keyboardFocusChangedEventHandler);
			}
			if ((bool)e.NewValue)
			{
				AccessKeyManager.Register("\r", button);
				KeyboardNavigation.Current.FocusChanged += keyboardFocusChangedEventHandler;
				button.UpdateIsDefaulted(Keyboard.FocusedElement);
				return;
			}
			AccessKeyManager.Unregister("\r", button);
			KeyboardNavigation.Current.FocusChanged -= keyboardFocusChangedEventHandler;
			button.UpdateIsDefaulted(null);
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x00297718 File Offset: 0x00296718
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button button = (Button)d;
			if (button.IsDefault)
			{
				button.UpdateIsDefaulted(Keyboard.FocusedElement);
			}
		}

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x06005FE7 RID: 24551 RVA: 0x0029773F File Offset: 0x0029673F
		// (set) Token: 0x06005FE8 RID: 24552 RVA: 0x00297751 File Offset: 0x00296751
		public bool IsCancel
		{
			get
			{
				return (bool)base.GetValue(Button.IsCancelProperty);
			}
			set
			{
				base.SetValue(Button.IsCancelProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x00297764 File Offset: 0x00296764
		private static void OnIsCancelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Button element = d as Button;
			if ((bool)e.NewValue)
			{
				AccessKeyManager.Register("\u001b", element);
				return;
			}
			AccessKeyManager.Unregister("\u001b", element);
		}

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x06005FEA RID: 24554 RVA: 0x0029779D File Offset: 0x0029679D
		public bool IsDefaulted
		{
			get
			{
				return (bool)base.GetValue(Button.IsDefaultedProperty);
			}
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x002977AF File Offset: 0x002967AF
		private void OnFocusChanged(object sender, KeyboardFocusChangedEventArgs e)
		{
			this.UpdateIsDefaulted(Keyboard.FocusedElement);
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x002977BC File Offset: 0x002967BC
		private void UpdateIsDefaulted(IInputElement focus)
		{
			if (!this.IsDefault || focus == null || !base.IsEnabled)
			{
				base.SetValue(Button.IsDefaultedPropertyKey, BooleanBoxes.FalseBox);
				return;
			}
			DependencyObject dependencyObject = focus as DependencyObject;
			object value = BooleanBoxes.FalseBox;
			try
			{
				AccessKeyPressedEventArgs accessKeyPressedEventArgs = new AccessKeyPressedEventArgs();
				focus.RaiseEvent(accessKeyPressedEventArgs);
				object scope = accessKeyPressedEventArgs.Scope;
				accessKeyPressedEventArgs = new AccessKeyPressedEventArgs();
				base.RaiseEvent(accessKeyPressedEventArgs);
				if (accessKeyPressedEventArgs.Scope == scope && (dependencyObject == null || !(bool)dependencyObject.GetValue(KeyboardNavigation.AcceptsReturnProperty)))
				{
					value = BooleanBoxes.TrueBox;
				}
			}
			finally
			{
				base.SetValue(Button.IsDefaultedPropertyKey, value);
			}
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x00297860 File Offset: 0x00296860
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ButtonAutomationPeer(this);
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x00297868 File Offset: 0x00296868
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
			try
			{
				base.OnClick();
			}
			finally
			{
				if (base.Command == null && this.IsCancel)
				{
					CommandHelpers.ExecuteCommand(Window.DialogCancelCommand, null, this);
				}
			}
		}

		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x06005FEF RID: 24559 RVA: 0x001FCA42 File Offset: 0x001FBA42
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06005FF0 RID: 24560 RVA: 0x002978C4 File Offset: 0x002968C4
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Button._dType;
			}
		}

		// Token: 0x040031F6 RID: 12790
		public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register("IsDefault", typeof(bool), typeof(Button), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Button.OnIsDefaultChanged)));

		// Token: 0x040031F7 RID: 12791
		public static readonly DependencyProperty IsCancelProperty = DependencyProperty.Register("IsCancel", typeof(bool), typeof(Button), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Button.OnIsCancelChanged)));

		// Token: 0x040031F8 RID: 12792
		private static readonly DependencyPropertyKey IsDefaultedPropertyKey = DependencyProperty.RegisterReadOnly("IsDefaulted", typeof(bool), typeof(Button), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x040031F9 RID: 12793
		public static readonly DependencyProperty IsDefaultedProperty = Button.IsDefaultedPropertyKey.DependencyProperty;

		// Token: 0x040031FA RID: 12794
		private static readonly UncommonField<KeyboardFocusChangedEventHandler> FocusChangedEventHandlerField = new UncommonField<KeyboardFocusChangedEventHandler>();

		// Token: 0x040031FB RID: 12795
		private static DependencyObjectType _dType;
	}
}
