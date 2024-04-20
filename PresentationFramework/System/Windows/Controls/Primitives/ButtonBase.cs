using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000823 RID: 2083
	[DefaultEvent("Click")]
	[Localizability(LocalizationCategory.Button)]
	public abstract class ButtonBase : ContentControl, ICommandSource
	{
		// Token: 0x0600796D RID: 31085 RVA: 0x003032F0 File Offset: 0x003022F0
		static ButtonBase()
		{
			ButtonBase.ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ButtonBase));
			ButtonBase.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonBase), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ButtonBase.OnCommandChanged)));
			ButtonBase.CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ButtonBase), new FrameworkPropertyMetadata(null));
			ButtonBase.CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ButtonBase), new FrameworkPropertyMetadata(null));
			ButtonBase.IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof(bool), typeof(ButtonBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(ButtonBase.OnIsPressedChanged)));
			ButtonBase.IsPressedProperty = ButtonBase.IsPressedPropertyKey.DependencyProperty;
			ButtonBase.ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(ButtonBase), new FrameworkPropertyMetadata(ClickMode.Release), new ValidateValueCallback(ButtonBase.IsValidClickMode));
			EventManager.RegisterClassHandler(typeof(ButtonBase), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(ButtonBase.OnAccessKeyPressed));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(ButtonBase), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof(ButtonBase), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(ButtonBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(ButtonBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x0600796F RID: 31087 RVA: 0x003034D4 File Offset: 0x003024D4
		protected virtual void OnClick()
		{
			RoutedEventArgs e = new RoutedEventArgs(ButtonBase.ClickEvent, this);
			base.RaiseEvent(e);
			CommandHelpers.ExecuteCommandSource(this);
		}

		// Token: 0x06007970 RID: 31088 RVA: 0x003034FA File Offset: 0x003024FA
		protected virtual void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
		{
			Control.OnVisualStatePropertyChanged(this, e);
		}

		// Token: 0x17001C22 RID: 7202
		// (get) Token: 0x06007971 RID: 31089 RVA: 0x00303504 File Offset: 0x00302504
		private bool IsInMainFocusScope
		{
			get
			{
				Visual visual = FocusManager.GetFocusScope(this) as Visual;
				return visual == null || VisualTreeHelper.GetParent(visual) == null;
			}
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x0030352B File Offset: 0x0030252B
		internal void AutomationButtonBaseClick()
		{
			this.OnClick();
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x00303534 File Offset: 0x00302534
		private static bool IsValidClickMode(object o)
		{
			ClickMode clickMode = (ClickMode)o;
			return clickMode == ClickMode.Press || clickMode == ClickMode.Release || clickMode == ClickMode.Hover;
		}

		// Token: 0x06007974 RID: 31092 RVA: 0x00303555 File Offset: 0x00302555
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			if (base.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed && !this.IsSpaceKeyDown)
			{
				this.UpdateIsPressed();
			}
		}

		// Token: 0x06007975 RID: 31093 RVA: 0x00303581 File Offset: 0x00302581
		private static void OnIsPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ButtonBase)d).OnIsPressedChanged(e);
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x001D3103 File Offset: 0x001D2103
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!e.Handled && e.Scope == null && e.Target == null)
			{
				e.Target = (UIElement)sender;
			}
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x00303590 File Offset: 0x00302590
		private void UpdateIsPressed()
		{
			Point position = Mouse.PrimaryDevice.GetPosition(this);
			if (position.X >= 0.0 && position.X <= base.ActualWidth && position.Y >= 0.0 && position.Y <= base.ActualHeight)
			{
				if (!this.IsPressed)
				{
					this.SetIsPressed(true);
					return;
				}
			}
			else if (this.IsPressed)
			{
				this.SetIsPressed(false);
			}
		}

		// Token: 0x14000151 RID: 337
		// (add) Token: 0x06007978 RID: 31096 RVA: 0x0030360A File Offset: 0x0030260A
		// (remove) Token: 0x06007979 RID: 31097 RVA: 0x00303618 File Offset: 0x00302618
		[Category("Behavior")]
		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(ButtonBase.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(ButtonBase.ClickEvent, value);
			}
		}

		// Token: 0x17001C23 RID: 7203
		// (get) Token: 0x0600797A RID: 31098 RVA: 0x00303626 File Offset: 0x00302626
		// (set) Token: 0x0600797B RID: 31099 RVA: 0x00303638 File Offset: 0x00302638
		[Browsable(false)]
		[Category("Appearance")]
		[ReadOnly(true)]
		public bool IsPressed
		{
			get
			{
				return (bool)base.GetValue(ButtonBase.IsPressedProperty);
			}
			protected set
			{
				base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x0600797C RID: 31100 RVA: 0x0030364B File Offset: 0x0030264B
		private void SetIsPressed(bool pressed)
		{
			if (pressed)
			{
				base.SetValue(ButtonBase.IsPressedPropertyKey, BooleanBoxes.Box(pressed));
				return;
			}
			base.ClearValue(ButtonBase.IsPressedPropertyKey);
		}

		// Token: 0x17001C24 RID: 7204
		// (get) Token: 0x0600797D RID: 31101 RVA: 0x0030366D File Offset: 0x0030266D
		// (set) Token: 0x0600797E RID: 31102 RVA: 0x0030367F File Offset: 0x0030267F
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ButtonBase.CommandProperty);
			}
			set
			{
				base.SetValue(ButtonBase.CommandProperty, value);
			}
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x0030368D File Offset: 0x0030268D
		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ButtonBase)d).OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		// Token: 0x06007980 RID: 31104 RVA: 0x003036B2 File Offset: 0x003026B2
		private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				this.UnhookCommand(oldCommand);
			}
			if (newCommand != null)
			{
				this.HookCommand(newCommand);
			}
		}

		// Token: 0x06007981 RID: 31105 RVA: 0x003036C8 File Offset: 0x003026C8
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06007982 RID: 31106 RVA: 0x003036E2 File Offset: 0x003026E2
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06007983 RID: 31107 RVA: 0x003036FC File Offset: 0x003026FC
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x06007984 RID: 31108 RVA: 0x00303704 File Offset: 0x00302704
		private void UpdateCanExecute()
		{
			if (this.Command != null)
			{
				this.CanExecute = CommandHelpers.CanExecuteCommandSource(this);
				return;
			}
			this.CanExecute = true;
		}

		// Token: 0x17001C25 RID: 7205
		// (get) Token: 0x06007985 RID: 31109 RVA: 0x00303722 File Offset: 0x00302722
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this.CanExecute;
			}
		}

		// Token: 0x17001C26 RID: 7206
		// (get) Token: 0x06007986 RID: 31110 RVA: 0x00303734 File Offset: 0x00302734
		// (set) Token: 0x06007987 RID: 31111 RVA: 0x00303741 File Offset: 0x00302741
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object CommandParameter
		{
			get
			{
				return base.GetValue(ButtonBase.CommandParameterProperty);
			}
			set
			{
				base.SetValue(ButtonBase.CommandParameterProperty, value);
			}
		}

		// Token: 0x17001C27 RID: 7207
		// (get) Token: 0x06007988 RID: 31112 RVA: 0x0030374F File Offset: 0x0030274F
		// (set) Token: 0x06007989 RID: 31113 RVA: 0x00303761 File Offset: 0x00302761
		[Bindable(true)]
		[Category("Action")]
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(ButtonBase.CommandTargetProperty);
			}
			set
			{
				base.SetValue(ButtonBase.CommandTargetProperty, value);
			}
		}

		// Token: 0x17001C28 RID: 7208
		// (get) Token: 0x0600798A RID: 31114 RVA: 0x0030376F File Offset: 0x0030276F
		// (set) Token: 0x0600798B RID: 31115 RVA: 0x00303781 File Offset: 0x00302781
		[Bindable(true)]
		[Category("Behavior")]
		public ClickMode ClickMode
		{
			get
			{
				return (ClickMode)base.GetValue(ButtonBase.ClickModeProperty);
			}
			set
			{
				base.SetValue(ButtonBase.ClickModeProperty, value);
			}
		}

		// Token: 0x0600798C RID: 31116 RVA: 0x00303794 File Offset: 0x00302794
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (this.ClickMode != ClickMode.Hover)
			{
				e.Handled = true;
				base.Focus();
				if (e.ButtonState == MouseButtonState.Pressed)
				{
					base.CaptureMouse();
					if (base.IsMouseCaptured)
					{
						if (e.ButtonState == MouseButtonState.Pressed)
						{
							if (!this.IsPressed)
							{
								this.SetIsPressed(true);
							}
						}
						else
						{
							base.ReleaseMouseCapture();
						}
					}
				}
				if (this.ClickMode == ClickMode.Press)
				{
					bool flag = true;
					try
					{
						this.OnClick();
						flag = false;
					}
					finally
					{
						if (flag)
						{
							this.SetIsPressed(false);
							base.ReleaseMouseCapture();
						}
					}
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x0600798D RID: 31117 RVA: 0x00303830 File Offset: 0x00302830
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (this.ClickMode != ClickMode.Hover)
			{
				e.Handled = true;
				bool flag = !this.IsSpaceKeyDown && this.IsPressed && this.ClickMode == ClickMode.Release;
				if (base.IsMouseCaptured && !this.IsSpaceKeyDown)
				{
					base.ReleaseMouseCapture();
				}
				if (flag)
				{
					this.OnClick();
				}
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x0600798E RID: 31118 RVA: 0x0030388E File Offset: 0x0030288E
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.ClickMode != ClickMode.Hover && base.IsMouseCaptured && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed && !this.IsSpaceKeyDown)
			{
				this.UpdateIsPressed();
				e.Handled = true;
			}
		}

		// Token: 0x0600798F RID: 31119 RVA: 0x003038CC File Offset: 0x003028CC
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (e.OriginalSource == this && this.ClickMode != ClickMode.Hover && !this.IsSpaceKeyDown)
			{
				if (base.IsKeyboardFocused && !this.IsInMainFocusScope)
				{
					Keyboard.Focus(null);
				}
				this.SetIsPressed(false);
			}
		}

		// Token: 0x06007990 RID: 31120 RVA: 0x00303918 File Offset: 0x00302918
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06007991 RID: 31121 RVA: 0x00303930 File Offset: 0x00302930
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06007992 RID: 31122 RVA: 0x00303948 File Offset: 0x00302948
		private bool HandleIsMouseOverChanged()
		{
			if (this.ClickMode == ClickMode.Hover)
			{
				if (base.IsMouseOver)
				{
					this.SetIsPressed(true);
					this.OnClick();
				}
				else
				{
					this.SetIsPressed(false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06007993 RID: 31123 RVA: 0x00303974 File Offset: 0x00302974
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.ClickMode == ClickMode.Hover)
			{
				return;
			}
			if (e.Key == Key.Space)
			{
				if ((Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) != ModifierKeys.Alt && !base.IsMouseCaptured && e.OriginalSource == this)
				{
					this.IsSpaceKeyDown = true;
					this.SetIsPressed(true);
					base.CaptureMouse();
					if (this.ClickMode == ClickMode.Press)
					{
						this.OnClick();
					}
					e.Handled = true;
					return;
				}
			}
			else if (e.Key == Key.Return && (bool)base.GetValue(KeyboardNavigation.AcceptsReturnProperty))
			{
				if (e.OriginalSource == this)
				{
					this.IsSpaceKeyDown = false;
					this.SetIsPressed(false);
					if (base.IsMouseCaptured)
					{
						base.ReleaseMouseCapture();
					}
					this.OnClick();
					e.Handled = true;
					return;
				}
			}
			else if (this.IsSpaceKeyDown)
			{
				this.SetIsPressed(false);
				this.IsSpaceKeyDown = false;
				if (base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
			}
		}

		// Token: 0x06007994 RID: 31124 RVA: 0x00303A60 File Offset: 0x00302A60
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (this.ClickMode == ClickMode.Hover)
			{
				return;
			}
			if (e.Key == Key.Space && this.IsSpaceKeyDown && (Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) != ModifierKeys.Alt)
			{
				this.IsSpaceKeyDown = false;
				if (this.GetMouseLeftButtonReleased())
				{
					bool flag = this.IsPressed && this.ClickMode == ClickMode.Release;
					if (base.IsMouseCaptured)
					{
						base.ReleaseMouseCapture();
					}
					if (flag)
					{
						this.OnClick();
					}
				}
				else if (base.IsMouseCaptured)
				{
					this.UpdateIsPressed();
				}
				e.Handled = true;
			}
		}

		// Token: 0x06007995 RID: 31125 RVA: 0x00303AEA File Offset: 0x00302AEA
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (this.ClickMode == ClickMode.Hover)
			{
				return;
			}
			if (e.OriginalSource == this)
			{
				if (this.IsPressed)
				{
					this.SetIsPressed(false);
				}
				if (base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
				this.IsSpaceKeyDown = false;
			}
		}

		// Token: 0x06007996 RID: 31126 RVA: 0x00303B2A File Offset: 0x00302B2A
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			if (e.IsMultiple)
			{
				base.OnAccessKey(e);
				return;
			}
			this.OnClick();
		}

		// Token: 0x06007997 RID: 31127 RVA: 0x00303B42 File Offset: 0x00302B42
		private bool GetMouseLeftButtonReleased()
		{
			return InputManager.Current.PrimaryMouseDevice.LeftButton == MouseButtonState.Released;
		}

		// Token: 0x17001C29 RID: 7209
		// (get) Token: 0x06007998 RID: 31128 RVA: 0x00303B56 File Offset: 0x00302B56
		// (set) Token: 0x06007999 RID: 31129 RVA: 0x00303B5F File Offset: 0x00302B5F
		private bool IsSpaceKeyDown
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.IsSpaceKeyDown);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.IsSpaceKeyDown, value);
			}
		}

		// Token: 0x17001C2A RID: 7210
		// (get) Token: 0x0600799A RID: 31130 RVA: 0x002D6172 File Offset: 0x002D5172
		// (set) Token: 0x0600799B RID: 31131 RVA: 0x00303B69 File Offset: 0x00302B69
		private bool CanExecute
		{
			get
			{
				return !base.ReadControlFlag(Control.ControlBoolFlags.CommandDisabled);
			}
			set
			{
				if (value != this.CanExecute)
				{
					base.WriteControlFlag(Control.ControlBoolFlags.CommandDisabled, !value);
					base.CoerceValue(UIElement.IsEnabledProperty);
				}
			}
		}

		// Token: 0x0600799C RID: 31132 RVA: 0x00303B8C File Offset: 0x00302B8C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (this.IsPressed)
			{
				VisualStateManager.GoToState(this, "Pressed", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x040039A9 RID: 14761
		[CommonDependencyProperty]
		public static readonly DependencyProperty CommandProperty;

		// Token: 0x040039AA RID: 14762
		[CommonDependencyProperty]
		public static readonly DependencyProperty CommandParameterProperty;

		// Token: 0x040039AB RID: 14763
		[CommonDependencyProperty]
		public static readonly DependencyProperty CommandTargetProperty;

		// Token: 0x040039AC RID: 14764
		internal static readonly DependencyPropertyKey IsPressedPropertyKey;

		// Token: 0x040039AD RID: 14765
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsPressedProperty;

		// Token: 0x040039AE RID: 14766
		public static readonly DependencyProperty ClickModeProperty;
	}
}
