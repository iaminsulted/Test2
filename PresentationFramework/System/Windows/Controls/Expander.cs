using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200077B RID: 1915
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Expander : HeaderedContentControl
	{
		// Token: 0x06006868 RID: 26728 RVA: 0x002B8820 File Offset: 0x002B7820
		static Expander()
		{
			Expander.ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Expander));
			Expander.CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Expander));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Expander), new FrameworkPropertyMetadata(typeof(Expander)));
			Expander._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Expander));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Expander), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(Expander), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Expander), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.Expander);
		}

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x06006869 RID: 26729 RVA: 0x002B89A8 File Offset: 0x002B79A8
		// (set) Token: 0x0600686A RID: 26730 RVA: 0x002B89BA File Offset: 0x002B79BA
		[Bindable(true)]
		[Category("Behavior")]
		public ExpandDirection ExpandDirection
		{
			get
			{
				return (ExpandDirection)base.GetValue(Expander.ExpandDirectionProperty);
			}
			set
			{
				base.SetValue(Expander.ExpandDirectionProperty, value);
			}
		}

		// Token: 0x0600686B RID: 26731 RVA: 0x002B89D0 File Offset: 0x002B79D0
		private static bool IsValidExpandDirection(object o)
		{
			ExpandDirection expandDirection = (ExpandDirection)o;
			return expandDirection == ExpandDirection.Down || expandDirection == ExpandDirection.Left || expandDirection == ExpandDirection.Right || expandDirection == ExpandDirection.Up;
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x002B89F8 File Offset: 0x002B79F8
		private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Expander expander = (Expander)d;
			bool flag = (bool)e.NewValue;
			ExpanderAutomationPeer expanderAutomationPeer = UIElementAutomationPeer.FromElement(expander) as ExpanderAutomationPeer;
			if (expanderAutomationPeer != null)
			{
				expanderAutomationPeer.RaiseExpandCollapseAutomationEvent(!flag, flag);
			}
			if (flag)
			{
				expander.OnExpanded();
			}
			else
			{
				expander.OnCollapsed();
			}
			expander.UpdateVisualState();
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x0600686D RID: 26733 RVA: 0x002B8A4A File Offset: 0x002B7A4A
		// (set) Token: 0x0600686E RID: 26734 RVA: 0x002B8A5C File Offset: 0x002B7A5C
		[Category("Appearance")]
		[Bindable(true)]
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(Expander.IsExpandedProperty);
			}
			set
			{
				base.SetValue(Expander.IsExpandedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x1400010E RID: 270
		// (add) Token: 0x0600686F RID: 26735 RVA: 0x002B8A6F File Offset: 0x002B7A6F
		// (remove) Token: 0x06006870 RID: 26736 RVA: 0x002B8A7D File Offset: 0x002B7A7D
		public event RoutedEventHandler Expanded
		{
			add
			{
				base.AddHandler(Expander.ExpandedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Expander.ExpandedEvent, value);
			}
		}

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x06006871 RID: 26737 RVA: 0x002B8A8B File Offset: 0x002B7A8B
		// (remove) Token: 0x06006872 RID: 26738 RVA: 0x002B8A99 File Offset: 0x002B7A99
		public event RoutedEventHandler Collapsed
		{
			add
			{
				base.AddHandler(Expander.CollapsedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Expander.CollapsedEvent, value);
			}
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x002B8AA8 File Offset: 0x002B7AA8
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
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Normal"
				});
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
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unfocused"
				});
			}
			if (this.IsExpanded)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Expanded"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Collapsed"
				});
			}
			switch (this.ExpandDirection)
			{
			case ExpandDirection.Down:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandDown"
				});
				break;
			case ExpandDirection.Up:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandUp"
				});
				break;
			case ExpandDirection.Left:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandLeft"
				});
				break;
			default:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandRight"
				});
				break;
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06006874 RID: 26740 RVA: 0x002B8C04 File Offset: 0x002B7C04
		protected virtual void OnExpanded()
		{
			base.RaiseEvent(new RoutedEventArgs
			{
				RoutedEvent = Expander.ExpandedEvent,
				Source = this
			});
		}

		// Token: 0x06006875 RID: 26741 RVA: 0x002B8C30 File Offset: 0x002B7C30
		protected virtual void OnCollapsed()
		{
			base.RaiseEvent(new RoutedEventArgs(Expander.CollapsedEvent, this));
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x002B8C43 File Offset: 0x002B7C43
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ExpanderAutomationPeer(this);
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x002B8C4B File Offset: 0x002B7C4B
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._expanderToggleButton = (base.GetTemplateChild("HeaderSite") as ToggleButton);
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x06006878 RID: 26744 RVA: 0x002B8C69 File Offset: 0x002B7C69
		internal bool IsExpanderToggleButtonFocused
		{
			get
			{
				ToggleButton expanderToggleButton = this._expanderToggleButton;
				return expanderToggleButton != null && expanderToggleButton.IsKeyboardFocused;
			}
		}

		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x06006879 RID: 26745 RVA: 0x002B8C7C File Offset: 0x002B7C7C
		internal ToggleButton ExpanderToggleButton
		{
			get
			{
				return this._expanderToggleButton;
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x0600687A RID: 26746 RVA: 0x002B8C84 File Offset: 0x002B7C84
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Expander._dType;
			}
		}

		// Token: 0x040034AB RID: 13483
		public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register("ExpandDirection", typeof(ExpandDirection), typeof(Expander), new FrameworkPropertyMetadata(ExpandDirection.Down, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)), new ValidateValueCallback(Expander.IsValidExpandDirection));

		// Token: 0x040034AC RID: 13484
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(Expander), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(Expander.OnIsExpandedChanged)));

		// Token: 0x040034AF RID: 13487
		private const string ExpanderToggleButtonTemplateName = "HeaderSite";

		// Token: 0x040034B0 RID: 13488
		private ToggleButton _expanderToggleButton;

		// Token: 0x040034B1 RID: 13489
		private static DependencyObjectType _dType;
	}
}
