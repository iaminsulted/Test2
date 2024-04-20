using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007EE RID: 2030
	[DefaultEvent("Opened")]
	[Localizability(LocalizationCategory.ToolTip)]
	public class ToolTip : ContentControl
	{
		// Token: 0x060075BC RID: 30140 RVA: 0x002ED5D8 File Offset: 0x002EC5D8
		static ToolTip()
		{
			System.Windows.Controls.ToolTip.OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToolTip));
			System.Windows.Controls.ToolTip.ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToolTip));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(typeof(ToolTip)));
			System.Windows.Controls.ToolTip._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToolTip));
			Control.BackgroundProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(SystemColors.InfoBrush));
			UIElement.FocusableProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(false));
		}

		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x060075BE RID: 30142 RVA: 0x002ED842 File Offset: 0x002EC842
		// (set) Token: 0x060075BF RID: 30143 RVA: 0x002ED854 File Offset: 0x002EC854
		[Bindable(true)]
		[Category("Behavior")]
		internal bool FromKeyboard
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.FromKeyboardProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.FromKeyboardProperty, value);
			}
		}

		// Token: 0x17001B5D RID: 7005
		// (get) Token: 0x060075C0 RID: 30144 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool ShouldShowOnKeyboardFocus
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060075C1 RID: 30145 RVA: 0x002ED862 File Offset: 0x002EC862
		private static object CoerceHorizontalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.HorizontalOffsetProperty);
		}

		// Token: 0x17001B5E RID: 7006
		// (get) Token: 0x060075C2 RID: 30146 RVA: 0x002ED870 File Offset: 0x002EC870
		// (set) Token: 0x060075C3 RID: 30147 RVA: 0x002ED882 File Offset: 0x002EC882
		[Bindable(true)]
		[TypeConverter(typeof(LengthConverter))]
		[Category("Layout")]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(System.Windows.Controls.ToolTip.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x060075C4 RID: 30148 RVA: 0x002ED895 File Offset: 0x002EC895
		private static object CoerceVerticalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.VerticalOffsetProperty);
		}

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x060075C5 RID: 30149 RVA: 0x002ED8A3 File Offset: 0x002EC8A3
		// (set) Token: 0x060075C6 RID: 30150 RVA: 0x002ED8B5 File Offset: 0x002EC8B5
		[Category("Layout")]
		[Bindable(true)]
		[TypeConverter(typeof(LengthConverter))]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(System.Windows.Controls.ToolTip.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.VerticalOffsetProperty, value);
			}
		}

		// Token: 0x060075C7 RID: 30151 RVA: 0x002ED8C8 File Offset: 0x002EC8C8
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToolTip toolTip = (ToolTip)d;
			if ((bool)e.NewValue)
			{
				if (toolTip._parentPopup == null)
				{
					toolTip.HookupParentPopup();
				}
			}
			else if (AutomationPeer.ListenerExists(AutomationEvents.ToolTipClosed))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(toolTip);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.ToolTipClosed);
				}
			}
			Control.OnVisualStatePropertyChanged(d, e);
		}

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x060075C8 RID: 30152 RVA: 0x002ED91A File Offset: 0x002EC91A
		// (set) Token: 0x060075C9 RID: 30153 RVA: 0x002ED92C File Offset: 0x002EC92C
		[Browsable(false)]
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.IsOpenProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.IsOpenProperty, value);
			}
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x002ED93C File Offset: 0x002EC93C
		private static object CoerceHasDropShadow(DependencyObject d, object value)
		{
			ToolTip toolTip = (ToolTip)d;
			if (toolTip._parentPopup == null || !toolTip._parentPopup.AllowsTransparency || !SystemParameters.DropShadow)
			{
				return BooleanBoxes.FalseBox;
			}
			return PopupControlService.CoerceProperty(d, value, ToolTipService.HasDropShadowProperty);
		}

		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x060075CB RID: 30155 RVA: 0x002ED97E File Offset: 0x002EC97E
		// (set) Token: 0x060075CC RID: 30156 RVA: 0x002ED990 File Offset: 0x002EC990
		public bool HasDropShadow
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.HasDropShadowProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.HasDropShadowProperty, value);
			}
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x002ED99E File Offset: 0x002EC99E
		private static object CoercePlacementTarget(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.PlacementTargetProperty);
		}

		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x060075CE RID: 30158 RVA: 0x002ED9AC File Offset: 0x002EC9AC
		// (set) Token: 0x060075CF RID: 30159 RVA: 0x002ED9BE File Offset: 0x002EC9BE
		[Category("Layout")]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UIElement PlacementTarget
		{
			get
			{
				return (UIElement)base.GetValue(System.Windows.Controls.ToolTip.PlacementTargetProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.PlacementTargetProperty, value);
			}
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x002ED9CC File Offset: 0x002EC9CC
		private static object CoercePlacementRectangle(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.PlacementRectangleProperty);
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x060075D1 RID: 30161 RVA: 0x002ED9DA File Offset: 0x002EC9DA
		// (set) Token: 0x060075D2 RID: 30162 RVA: 0x002ED9EC File Offset: 0x002EC9EC
		[Bindable(true)]
		[Category("Layout")]
		public Rect PlacementRectangle
		{
			get
			{
				return (Rect)base.GetValue(System.Windows.Controls.ToolTip.PlacementRectangleProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.PlacementRectangleProperty, value);
			}
		}

		// Token: 0x060075D3 RID: 30163 RVA: 0x002ED9FF File Offset: 0x002EC9FF
		private static object CoercePlacement(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.PlacementProperty);
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x060075D4 RID: 30164 RVA: 0x002EDA0D File Offset: 0x002ECA0D
		// (set) Token: 0x060075D5 RID: 30165 RVA: 0x002EDA1F File Offset: 0x002ECA1F
		[Bindable(true)]
		[Category("Layout")]
		public PlacementMode Placement
		{
			get
			{
				return (PlacementMode)base.GetValue(System.Windows.Controls.ToolTip.PlacementProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.PlacementProperty, value);
			}
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x060075D6 RID: 30166 RVA: 0x002EDA32 File Offset: 0x002ECA32
		// (set) Token: 0x060075D7 RID: 30167 RVA: 0x002EDA44 File Offset: 0x002ECA44
		[Bindable(false)]
		[Category("Layout")]
		public CustomPopupPlacementCallback CustomPopupPlacementCallback
		{
			get
			{
				return (CustomPopupPlacementCallback)base.GetValue(System.Windows.Controls.ToolTip.CustomPopupPlacementCallbackProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.CustomPopupPlacementCallbackProperty, value);
			}
		}

		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x060075D8 RID: 30168 RVA: 0x002EDA52 File Offset: 0x002ECA52
		// (set) Token: 0x060075D9 RID: 30169 RVA: 0x002EDA64 File Offset: 0x002ECA64
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpen
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.StaysOpenProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.StaysOpenProperty, value);
			}
		}

		// Token: 0x14000147 RID: 327
		// (add) Token: 0x060075DA RID: 30170 RVA: 0x002EDA72 File Offset: 0x002ECA72
		// (remove) Token: 0x060075DB RID: 30171 RVA: 0x002EDA80 File Offset: 0x002ECA80
		public event RoutedEventHandler Opened
		{
			add
			{
				base.AddHandler(System.Windows.Controls.ToolTip.OpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(System.Windows.Controls.ToolTip.OpenedEvent, value);
			}
		}

		// Token: 0x060075DC RID: 30172 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnOpened(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x14000148 RID: 328
		// (add) Token: 0x060075DD RID: 30173 RVA: 0x002EDA8E File Offset: 0x002ECA8E
		// (remove) Token: 0x060075DE RID: 30174 RVA: 0x002EDA9C File Offset: 0x002ECA9C
		public event RoutedEventHandler Closed
		{
			add
			{
				base.AddHandler(System.Windows.Controls.ToolTip.ClosedEvent, value);
			}
			remove
			{
				base.RemoveHandler(System.Windows.Controls.ToolTip.ClosedEvent, value);
			}
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnClosed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x002EDAAA File Offset: 0x002ECAAA
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (this.IsOpen)
			{
				VisualStateManager.GoToState(this, "Open", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Closed", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x002EDAD7 File Offset: 0x002ECAD7
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToolTipAutomationPeer(this);
		}

		// Token: 0x060075E2 RID: 30178 RVA: 0x002EDADF File Offset: 0x002ECADF
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ToolTip"
				}));
			}
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x002EDB14 File Offset: 0x002ECB14
		internal override void OnAncestorChanged()
		{
			base.OnAncestorChanged();
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ToolTip"
				}));
			}
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x002EDB48 File Offset: 0x002ECB48
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			PopupControlService popupControlService = PopupControlService.Current;
			if (this == popupControlService.CurrentToolTip && (bool)base.GetValue(PopupControlService.ServiceOwnedProperty) && newContent is ToolTip)
			{
				popupControlService.OnRaiseToolTipClosingEvent(null, EventArgs.Empty);
				popupControlService.OnRaiseToolTipOpeningEvent(null, EventArgs.Empty);
				return;
			}
			base.OnContentChanged(oldContent, newContent);
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x002EDBA0 File Offset: 0x002ECBA0
		private void HookupParentPopup()
		{
			this._parentPopup = new Popup();
			this._parentPopup.AllowsTransparency = true;
			this._parentPopup.HitTestable = !this.StaysOpen;
			base.CoerceValue(System.Windows.Controls.ToolTip.HasDropShadowProperty);
			this._parentPopup.Opened += this.OnPopupOpened;
			this._parentPopup.Closed += this.OnPopupClosed;
			this._parentPopup.PopupCouldClose += this.OnPopupCouldClose;
			this._parentPopup.SetResourceReference(Popup.PopupAnimationProperty, SystemParameters.ToolTipPopupAnimationKey);
			Popup.CreateRootPopupInternal(this._parentPopup, this, true);
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x002EDC4A File Offset: 0x002ECC4A
		internal void ForceClose()
		{
			if (this._parentPopup != null)
			{
				this._parentPopup.ForceClose();
			}
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x002EDC5F File Offset: 0x002ECC5F
		private void OnPopupCouldClose(object sender, EventArgs e)
		{
			base.SetCurrentValueInternal(System.Windows.Controls.ToolTip.IsOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x002EDC74 File Offset: 0x002ECC74
		private void OnPopupOpened(object source, EventArgs e)
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.ToolTipOpened))
			{
				AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (peer != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
					{
						peer.RaiseAutomationEvent(AutomationEvents.ToolTipOpened);
						return null;
					}), null);
				}
			}
			this.OnOpened(new RoutedEventArgs(System.Windows.Controls.ToolTip.OpenedEvent, this));
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x002EDCCE File Offset: 0x002ECCCE
		private void OnPopupClosed(object source, EventArgs e)
		{
			this.OnClosed(new RoutedEventArgs(System.Windows.Controls.ToolTip.ClosedEvent, this));
		}

		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x060075EA RID: 30186 RVA: 0x002EDCE1 File Offset: 0x002ECCE1
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return System.Windows.Controls.ToolTip._dType;
			}
		}

		// Token: 0x04003866 RID: 14438
		internal static readonly DependencyProperty FromKeyboardProperty = DependencyProperty.Register("FromKeyboard", typeof(bool), typeof(ToolTip), new FrameworkPropertyMetadata(false));

		// Token: 0x04003867 RID: 14439
		public static readonly DependencyProperty HorizontalOffsetProperty = ToolTipService.HorizontalOffsetProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoerceHorizontalOffset)));

		// Token: 0x04003868 RID: 14440
		public static readonly DependencyProperty VerticalOffsetProperty = ToolTipService.VerticalOffsetProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoerceVerticalOffset)));

		// Token: 0x04003869 RID: 14441
		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ToolTip), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(System.Windows.Controls.ToolTip.OnIsOpenChanged)));

		// Token: 0x0400386A RID: 14442
		public static readonly DependencyProperty HasDropShadowProperty = ToolTipService.HasDropShadowProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoerceHasDropShadow)));

		// Token: 0x0400386B RID: 14443
		public static readonly DependencyProperty PlacementTargetProperty = ToolTipService.PlacementTargetProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoercePlacementTarget)));

		// Token: 0x0400386C RID: 14444
		public static readonly DependencyProperty PlacementRectangleProperty = ToolTipService.PlacementRectangleProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoercePlacementRectangle)));

		// Token: 0x0400386D RID: 14445
		public static readonly DependencyProperty PlacementProperty = ToolTipService.PlacementProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoercePlacement)));

		// Token: 0x0400386E RID: 14446
		public static readonly DependencyProperty CustomPopupPlacementCallbackProperty = Popup.CustomPopupPlacementCallbackProperty.AddOwner(typeof(ToolTip));

		// Token: 0x0400386F RID: 14447
		public static readonly DependencyProperty StaysOpenProperty = Popup.StaysOpenProperty.AddOwner(typeof(ToolTip));

		// Token: 0x04003872 RID: 14450
		private Popup _parentPopup;

		// Token: 0x04003873 RID: 14451
		private static DependencyObjectType _dType;

		// Token: 0x02000C28 RID: 3112
		internal enum ToolTipTrigger
		{
			// Token: 0x04004B51 RID: 19281
			Mouse,
			// Token: 0x04004B52 RID: 19282
			KeyboardFocus,
			// Token: 0x04004B53 RID: 19283
			KeyboardShortcut
		}
	}
}
