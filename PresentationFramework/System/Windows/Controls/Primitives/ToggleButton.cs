using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200085E RID: 2142
	[DefaultEvent("Checked")]
	public class ToggleButton : ButtonBase
	{
		// Token: 0x06007E4F RID: 32335 RVA: 0x00317E4C File Offset: 0x00316E4C
		static ToggleButton()
		{
			ToggleButton.CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButton));
			ToggleButton.UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButton));
			ToggleButton.IndeterminateEvent = EventManager.RegisterRoutedEvent("Indeterminate", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButton));
			ToggleButton.IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool?), typeof(ToggleButton), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ToggleButton.OnIsCheckedChanged)));
			ToggleButton.IsThreeStateProperty = DependencyProperty.Register("IsThreeState", typeof(bool), typeof(ToggleButton), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButton), new FrameworkPropertyMetadata(typeof(ToggleButton)));
			ToggleButton._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToggleButton));
		}

		// Token: 0x14000161 RID: 353
		// (add) Token: 0x06007E51 RID: 32337 RVA: 0x00317F67 File Offset: 0x00316F67
		// (remove) Token: 0x06007E52 RID: 32338 RVA: 0x00317F75 File Offset: 0x00316F75
		[Category("Behavior")]
		public event RoutedEventHandler Checked
		{
			add
			{
				base.AddHandler(ToggleButton.CheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ToggleButton.CheckedEvent, value);
			}
		}

		// Token: 0x14000162 RID: 354
		// (add) Token: 0x06007E53 RID: 32339 RVA: 0x00317F83 File Offset: 0x00316F83
		// (remove) Token: 0x06007E54 RID: 32340 RVA: 0x00317F91 File Offset: 0x00316F91
		[Category("Behavior")]
		public event RoutedEventHandler Unchecked
		{
			add
			{
				base.AddHandler(ToggleButton.UncheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ToggleButton.UncheckedEvent, value);
			}
		}

		// Token: 0x14000163 RID: 355
		// (add) Token: 0x06007E55 RID: 32341 RVA: 0x00317F9F File Offset: 0x00316F9F
		// (remove) Token: 0x06007E56 RID: 32342 RVA: 0x00317FAD File Offset: 0x00316FAD
		[Category("Behavior")]
		public event RoutedEventHandler Indeterminate
		{
			add
			{
				base.AddHandler(ToggleButton.IndeterminateEvent, value);
			}
			remove
			{
				base.RemoveHandler(ToggleButton.IndeterminateEvent, value);
			}
		}

		// Token: 0x17001D26 RID: 7462
		// (get) Token: 0x06007E57 RID: 32343 RVA: 0x00317FBC File Offset: 0x00316FBC
		// (set) Token: 0x06007E58 RID: 32344 RVA: 0x00317FED File Offset: 0x00316FED
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[TypeConverter(typeof(NullableBoolConverter))]
		[Category("Appearance")]
		public bool? IsChecked
		{
			get
			{
				object value = base.GetValue(ToggleButton.IsCheckedProperty);
				if (value == null)
				{
					return null;
				}
				return new bool?((bool)value);
			}
			set
			{
				base.SetValue(ToggleButton.IsCheckedProperty, (value != null) ? BooleanBoxes.Box(value.Value) : null);
			}
		}

		// Token: 0x06007E59 RID: 32345 RVA: 0x00318012 File Offset: 0x00317012
		private static object OnGetIsChecked(DependencyObject d)
		{
			return ((ToggleButton)d).IsChecked;
		}

		// Token: 0x06007E5A RID: 32346 RVA: 0x00318024 File Offset: 0x00317024
		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleButton toggleButton = (ToggleButton)d;
			bool? oldValue = (bool?)e.OldValue;
			bool? flag = (bool?)e.NewValue;
			ToggleButtonAutomationPeer toggleButtonAutomationPeer = UIElementAutomationPeer.FromElement(toggleButton) as ToggleButtonAutomationPeer;
			if (toggleButtonAutomationPeer != null)
			{
				toggleButtonAutomationPeer.RaiseToggleStatePropertyChangedEvent(oldValue, flag);
			}
			bool? flag2 = flag;
			bool flag3 = true;
			if (flag2.GetValueOrDefault() == flag3 & flag2 != null)
			{
				toggleButton.OnChecked(new RoutedEventArgs(ToggleButton.CheckedEvent));
			}
			else
			{
				flag2 = flag;
				flag3 = false;
				if (flag2.GetValueOrDefault() == flag3 & flag2 != null)
				{
					toggleButton.OnUnchecked(new RoutedEventArgs(ToggleButton.UncheckedEvent));
				}
				else
				{
					toggleButton.OnIndeterminate(new RoutedEventArgs(ToggleButton.IndeterminateEvent));
				}
			}
			toggleButton.UpdateVisualState();
		}

		// Token: 0x06007E5B RID: 32347 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnChecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06007E5C RID: 32348 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnUnchecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06007E5D RID: 32349 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnIndeterminate(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x17001D27 RID: 7463
		// (get) Token: 0x06007E5E RID: 32350 RVA: 0x003180D9 File Offset: 0x003170D9
		// (set) Token: 0x06007E5F RID: 32351 RVA: 0x003180EB File Offset: 0x003170EB
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsThreeState
		{
			get
			{
				return (bool)base.GetValue(ToggleButton.IsThreeStateProperty);
			}
			set
			{
				base.SetValue(ToggleButton.IsThreeStateProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06007E60 RID: 32352 RVA: 0x003180FE File Offset: 0x003170FE
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToggleButtonAutomationPeer(this);
		}

		// Token: 0x06007E61 RID: 32353 RVA: 0x00318106 File Offset: 0x00317106
		protected override void OnClick()
		{
			this.OnToggle();
			base.OnClick();
		}

		// Token: 0x06007E62 RID: 32354 RVA: 0x00318114 File Offset: 0x00317114
		internal override void ChangeVisualState(bool useTransitions)
		{
			base.ChangeVisualState(useTransitions);
			bool? isChecked = this.IsChecked;
			bool? flag = isChecked;
			bool flag2 = true;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				VisualStateManager.GoToState(this, "Checked", useTransitions);
				return;
			}
			flag = isChecked;
			flag2 = false;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				VisualStateManager.GoToState(this, "Unchecked", useTransitions);
				return;
			}
			VisualStates.GoToState(this, useTransitions, new string[]
			{
				"Indeterminate",
				"Unchecked"
			});
		}

		// Token: 0x06007E63 RID: 32355 RVA: 0x00318198 File Offset: 0x00317198
		public override string ToString()
		{
			string text = base.GetType().ToString();
			string contentText = string.Empty;
			bool? isChecked = new bool?(false);
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				contentText = this.GetPlainText();
				isChecked = this.IsChecked;
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					contentText = this.GetPlainText();
					isChecked = this.IsChecked;
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_ToggleButton", new object[]
				{
					text,
					contentText,
					(isChecked != null) ? isChecked.Value.ToString() : "null"
				});
			}
			return text;
		}

		// Token: 0x06007E64 RID: 32356 RVA: 0x0031827C File Offset: 0x0031727C
		protected internal virtual void OnToggle()
		{
			bool? isChecked = this.IsChecked;
			bool flag = true;
			bool? flag2;
			if (isChecked.GetValueOrDefault() == flag & isChecked != null)
			{
				flag2 = (this.IsThreeState ? null : new bool?(false));
			}
			else
			{
				flag2 = new bool?(this.IsChecked != null);
			}
			base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, flag2);
		}

		// Token: 0x17001D28 RID: 7464
		// (get) Token: 0x06007E65 RID: 32357 RVA: 0x003182E8 File Offset: 0x003172E8
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ToggleButton._dType;
			}
		}

		// Token: 0x04003B37 RID: 15159
		public static readonly DependencyProperty IsCheckedProperty;

		// Token: 0x04003B38 RID: 15160
		public static readonly DependencyProperty IsThreeStateProperty;

		// Token: 0x04003B39 RID: 15161
		private static DependencyObjectType _dType;
	}
}
