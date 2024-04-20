using System;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Shell
{
	// Token: 0x020003F2 RID: 1010
	public sealed class ThumbButtonInfo : Freezable, ICommandSource
	{
		// Token: 0x06002B5A RID: 11098 RVA: 0x001A23DB File Offset: 0x001A13DB
		protected override Freezable CreateInstanceCore()
		{
			return new ThumbButtonInfo();
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x001A23E2 File Offset: 0x001A13E2
		// (set) Token: 0x06002B5C RID: 11100 RVA: 0x001A23F4 File Offset: 0x001A13F4
		public Visibility Visibility
		{
			get
			{
				return (Visibility)base.GetValue(ThumbButtonInfo.VisibilityProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.VisibilityProperty, value);
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x001A2407 File Offset: 0x001A1407
		// (set) Token: 0x06002B5E RID: 11102 RVA: 0x001A2419 File Offset: 0x001A1419
		public bool DismissWhenClicked
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.DismissWhenClickedProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.DismissWhenClickedProperty, value);
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x001A2427 File Offset: 0x001A1427
		// (set) Token: 0x06002B60 RID: 11104 RVA: 0x001A2439 File Offset: 0x001A1439
		public ImageSource ImageSource
		{
			get
			{
				return (ImageSource)base.GetValue(ThumbButtonInfo.ImageSourceProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.ImageSourceProperty, value);
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x001A2447 File Offset: 0x001A1447
		// (set) Token: 0x06002B62 RID: 11106 RVA: 0x001A2459 File Offset: 0x001A1459
		public bool IsBackgroundVisible
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsBackgroundVisibleProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsBackgroundVisibleProperty, value);
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x001A2467 File Offset: 0x001A1467
		// (set) Token: 0x06002B64 RID: 11108 RVA: 0x001A2479 File Offset: 0x001A1479
		public string Description
		{
			get
			{
				return (string)base.GetValue(ThumbButtonInfo.DescriptionProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.DescriptionProperty, value);
			}
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x001A2488 File Offset: 0x001A1488
		private static object CoerceDescription(DependencyObject d, object value)
		{
			string text = (string)value;
			if (text != null && text.Length >= 260)
			{
				text = text.Substring(0, 259);
			}
			return text;
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x001A24BA File Offset: 0x001A14BA
		private object CoerceIsEnabledValue(object value)
		{
			return (bool)value && this.CanExecute;
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x001A24D2 File Offset: 0x001A14D2
		// (set) Token: 0x06002B68 RID: 11112 RVA: 0x001A24E4 File Offset: 0x001A14E4
		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsEnabledProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsEnabledProperty, value);
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x001A24F2 File Offset: 0x001A14F2
		// (set) Token: 0x06002B6A RID: 11114 RVA: 0x001A2504 File Offset: 0x001A1504
		public bool IsInteractive
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsInteractiveProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsInteractiveProperty, value);
			}
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x001A2514 File Offset: 0x001A1514
		private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
		{
			ICommand command = (ICommand)e.OldValue;
			ICommand command2 = (ICommand)e.NewValue;
			if (command == command2)
			{
				return;
			}
			if (command != null)
			{
				this.UnhookCommand(command);
			}
			if (command2 != null)
			{
				this.HookCommand(command2);
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06002B6C RID: 11116 RVA: 0x001A2554 File Offset: 0x001A1554
		// (set) Token: 0x06002B6D RID: 11117 RVA: 0x001A2566 File Offset: 0x001A1566
		private bool CanExecute
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo._CanExecuteProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo._CanExecuteProperty, value);
			}
		}

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06002B6E RID: 11118 RVA: 0x001A2574 File Offset: 0x001A1574
		// (remove) Token: 0x06002B6F RID: 11119 RVA: 0x001A25AC File Offset: 0x001A15AC
		public event EventHandler Click;

		// Token: 0x06002B70 RID: 11120 RVA: 0x001A25E4 File Offset: 0x001A15E4
		internal void InvokeClick()
		{
			EventHandler click = this.Click;
			if (click != null)
			{
				click(this, EventArgs.Empty);
			}
			this._InvokeCommand();
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x001A2610 File Offset: 0x001A1610
		private void _InvokeCommand()
		{
			ICommand command = this.Command;
			if (command != null)
			{
				object commandParameter = this.CommandParameter;
				IInputElement commandTarget = this.CommandTarget;
				RoutedCommand routedCommand = command as RoutedCommand;
				if (routedCommand != null)
				{
					if (routedCommand.CanExecute(commandParameter, commandTarget))
					{
						routedCommand.Execute(commandParameter, commandTarget);
						return;
					}
				}
				else if (command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
				}
			}
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x001A2662 File Offset: 0x001A1662
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x001A267C File Offset: 0x001A167C
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x001A2696 File Offset: 0x001A1696
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x001A26A0 File Offset: 0x001A16A0
		private void UpdateCanExecute()
		{
			if (this.Command == null)
			{
				this.CanExecute = true;
				return;
			}
			object commandParameter = this.CommandParameter;
			IInputElement commandTarget = this.CommandTarget;
			RoutedCommand routedCommand = this.Command as RoutedCommand;
			if (routedCommand != null)
			{
				this.CanExecute = routedCommand.CanExecute(commandParameter, commandTarget);
				return;
			}
			this.CanExecute = this.Command.CanExecute(commandParameter);
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06002B76 RID: 11126 RVA: 0x001A26FB File Offset: 0x001A16FB
		// (set) Token: 0x06002B77 RID: 11127 RVA: 0x001A270D File Offset: 0x001A170D
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ThumbButtonInfo.CommandProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandProperty, value);
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06002B78 RID: 11128 RVA: 0x001A271B File Offset: 0x001A171B
		// (set) Token: 0x06002B79 RID: 11129 RVA: 0x001A2728 File Offset: 0x001A1728
		public object CommandParameter
		{
			get
			{
				return base.GetValue(ThumbButtonInfo.CommandParameterProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandParameterProperty, value);
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06002B7A RID: 11130 RVA: 0x001A2736 File Offset: 0x001A1736
		// (set) Token: 0x06002B7B RID: 11131 RVA: 0x001A2748 File Offset: 0x001A1748
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(ThumbButtonInfo.CommandTargetProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandTargetProperty, value);
			}
		}

		// Token: 0x04001ABE RID: 6846
		public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(ThumbButtonInfo), new PropertyMetadata(Visibility.Visible));

		// Token: 0x04001ABF RID: 6847
		public static readonly DependencyProperty DismissWhenClickedProperty = DependencyProperty.Register("DismissWhenClicked", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(false));

		// Token: 0x04001AC0 RID: 6848
		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ThumbButtonInfo), new PropertyMetadata(null));

		// Token: 0x04001AC1 RID: 6849
		public static readonly DependencyProperty IsBackgroundVisibleProperty = DependencyProperty.Register("IsBackgroundVisible", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));

		// Token: 0x04001AC2 RID: 6850
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ThumbButtonInfo), new PropertyMetadata(string.Empty, null, new CoerceValueCallback(ThumbButtonInfo.CoerceDescription)));

		// Token: 0x04001AC3 RID: 6851
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, null, (DependencyObject d, object e) => ((ThumbButtonInfo)d).CoerceIsEnabledValue(e)));

		// Token: 0x04001AC4 RID: 6852
		public static readonly DependencyProperty IsInteractiveProperty = DependencyProperty.Register("IsInteractive", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));

		// Token: 0x04001AC5 RID: 6853
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d).OnCommandChanged(e);
		}));

		// Token: 0x04001AC6 RID: 6854
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d).UpdateCanExecute();
		}));

		// Token: 0x04001AC7 RID: 6855
		public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d).UpdateCanExecute();
		}));

		// Token: 0x04001AC8 RID: 6856
		private static readonly DependencyProperty _CanExecuteProperty = DependencyProperty.Register("_CanExecute", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(ThumbButtonInfo.IsEnabledProperty);
		}));
	}
}
