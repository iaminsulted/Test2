using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007EC RID: 2028
	[TemplatePart(Name = "PART_ToolBarOverflowPanel", Type = typeof(ToolBarOverflowPanel))]
	[TemplatePart(Name = "PART_ToolBarPanel", Type = typeof(ToolBarPanel))]
	public class ToolBar : HeaderedItemsControl
	{
		// Token: 0x06007564 RID: 30052 RVA: 0x002EB9CC File Offset: 0x002EA9CC
		static ToolBar()
		{
			ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ToolBar.CoerceToolTipIsEnabled)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(typeof(ToolBar)));
			ToolBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToolBar));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.FocusableProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			EventManager.RegisterClassHandler(typeof(ToolBar), Mouse.MouseDownEvent, new MouseButtonEventHandler(ToolBar.OnMouseButtonDown), true);
			EventManager.RegisterClassHandler(typeof(ToolBar), ButtonBase.ClickEvent, new RoutedEventHandler(ToolBar._OnClick));
			ControlsTraceLogger.AddControl(TelemetryControls.ToolBar);
		}

		// Token: 0x06007566 RID: 30054 RVA: 0x002EBCEC File Offset: 0x002EACEC
		private static object CoerceOrientation(DependencyObject d, object value)
		{
			ToolBarTray toolBarTray = ((ToolBar)d).ToolBarTray;
			if (toolBarTray == null)
			{
				return value;
			}
			return toolBarTray.Orientation;
		}

		// Token: 0x17001B42 RID: 6978
		// (get) Token: 0x06007567 RID: 30055 RVA: 0x002EBD15 File Offset: 0x002EAD15
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ToolBar.OrientationProperty);
			}
		}

		// Token: 0x17001B43 RID: 6979
		// (get) Token: 0x06007568 RID: 30056 RVA: 0x002EBD27 File Offset: 0x002EAD27
		// (set) Token: 0x06007569 RID: 30057 RVA: 0x002EBD39 File Offset: 0x002EAD39
		public int Band
		{
			get
			{
				return (int)base.GetValue(ToolBar.BandProperty);
			}
			set
			{
				base.SetValue(ToolBar.BandProperty, value);
			}
		}

		// Token: 0x17001B44 RID: 6980
		// (get) Token: 0x0600756A RID: 30058 RVA: 0x002EBD4C File Offset: 0x002EAD4C
		// (set) Token: 0x0600756B RID: 30059 RVA: 0x002EBD5E File Offset: 0x002EAD5E
		public int BandIndex
		{
			get
			{
				return (int)base.GetValue(ToolBar.BandIndexProperty);
			}
			set
			{
				base.SetValue(ToolBar.BandIndexProperty, value);
			}
		}

		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x0600756C RID: 30060 RVA: 0x002EBD71 File Offset: 0x002EAD71
		// (set) Token: 0x0600756D RID: 30061 RVA: 0x002EBD83 File Offset: 0x002EAD83
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOverflowOpen
		{
			get
			{
				return (bool)base.GetValue(ToolBar.IsOverflowOpenProperty);
			}
			set
			{
				base.SetValue(ToolBar.IsOverflowOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x0600756E RID: 30062 RVA: 0x002EBD98 File Offset: 0x002EAD98
		private static object CoerceIsOverflowOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				ToolBar toolBar = (ToolBar)d;
				if (!toolBar.IsLoaded)
				{
					toolBar.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x0600756F RID: 30063 RVA: 0x002EBDC9 File Offset: 0x002EADC9
		private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
		{
			if (!((ToolBar)d).IsOverflowOpen)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x06007570 RID: 30064 RVA: 0x002EBDDF File Offset: 0x002EADDF
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x06007571 RID: 30065 RVA: 0x002EBDF3 File Offset: 0x002EADF3
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(ToolBar.IsOverflowOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x06007572 RID: 30066 RVA: 0x002EBE10 File Offset: 0x002EAE10
		private static void OnOverflowOpenChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			ToolBar toolBar = (ToolBar)element;
			if ((bool)e.NewValue)
			{
				Mouse.Capture(toolBar, CaptureMode.SubTree);
				toolBar.SetFocusOnToolBarOverflowPanel();
			}
			else
			{
				ToolBarOverflowPanel toolBarOverflowPanel = toolBar.ToolBarOverflowPanel;
				if (toolBarOverflowPanel != null && toolBarOverflowPanel.IsKeyboardFocusWithin)
				{
					Keyboard.Focus(null);
				}
				if (Mouse.Captured == toolBar)
				{
					Mouse.Capture(null);
				}
			}
			toolBar.CoerceValue(ToolTipService.IsEnabledProperty);
		}

		// Token: 0x06007573 RID: 30067 RVA: 0x002EBE75 File Offset: 0x002EAE75
		private void SetFocusOnToolBarOverflowPanel()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				if (this.ToolBarOverflowPanel != null)
				{
					if (KeyboardNavigation.IsKeyboardMostRecentInputDevice())
					{
						this.ToolBarOverflowPanel.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
					}
					else
					{
						this.ToolBarOverflowPanel.Focus();
					}
				}
				return null;
			}), null);
		}

		// Token: 0x17001B46 RID: 6982
		// (get) Token: 0x06007574 RID: 30068 RVA: 0x002EBE91 File Offset: 0x002EAE91
		public bool HasOverflowItems
		{
			get
			{
				return (bool)base.GetValue(ToolBar.HasOverflowItemsProperty);
			}
		}

		// Token: 0x06007575 RID: 30069 RVA: 0x002EBEA3 File Offset: 0x002EAEA3
		internal static void SetIsOverflowItem(DependencyObject element, object value)
		{
			element.SetValue(ToolBar.IsOverflowItemPropertyKey, value);
		}

		// Token: 0x06007576 RID: 30070 RVA: 0x002EBEB1 File Offset: 0x002EAEB1
		public static bool GetIsOverflowItem(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolBar.IsOverflowItemProperty);
		}

		// Token: 0x06007577 RID: 30071 RVA: 0x002EBED4 File Offset: 0x002EAED4
		private static void OnOverflowModeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			ToolBar toolBar = ItemsControl.ItemsControlFromItemContainer(element) as ToolBar;
			if (toolBar != null)
			{
				toolBar.InvalidateLayout();
			}
		}

		// Token: 0x06007578 RID: 30072 RVA: 0x002EBEF8 File Offset: 0x002EAEF8
		private void InvalidateLayout()
		{
			this._minLength = 0.0;
			this._maxLength = 0.0;
			base.InvalidateMeasure();
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				toolBarPanel.InvalidateMeasure();
			}
		}

		// Token: 0x06007579 RID: 30073 RVA: 0x002EBF3C File Offset: 0x002EAF3C
		private static bool IsValidOverflowMode(object o)
		{
			OverflowMode overflowMode = (OverflowMode)o;
			return overflowMode == OverflowMode.AsNeeded || overflowMode == OverflowMode.Always || overflowMode == OverflowMode.Never;
		}

		// Token: 0x0600757A RID: 30074 RVA: 0x002EBF5D File Offset: 0x002EAF5D
		public static void SetOverflowMode(DependencyObject element, OverflowMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolBar.OverflowModeProperty, mode);
		}

		// Token: 0x0600757B RID: 30075 RVA: 0x002EBF7E File Offset: 0x002EAF7E
		[AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
		public static OverflowMode GetOverflowMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (OverflowMode)element.GetValue(ToolBar.OverflowModeProperty);
		}

		// Token: 0x0600757C RID: 30076 RVA: 0x002EBF9E File Offset: 0x002EAF9E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToolBarAutomationPeer(this);
		}

		// Token: 0x0600757D RID: 30077 RVA: 0x002EBFA8 File Offset: 0x002EAFA8
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			FrameworkElement frameworkElement = element as FrameworkElement;
			if (frameworkElement != null)
			{
				Type type = frameworkElement.GetType();
				ResourceKey resourceKey = null;
				if (type == typeof(Button))
				{
					resourceKey = ToolBar.ButtonStyleKey;
				}
				else if (type == typeof(ToggleButton))
				{
					resourceKey = ToolBar.ToggleButtonStyleKey;
				}
				else if (type == typeof(Separator))
				{
					resourceKey = ToolBar.SeparatorStyleKey;
				}
				else if (type == typeof(CheckBox))
				{
					resourceKey = ToolBar.CheckBoxStyleKey;
				}
				else if (type == typeof(RadioButton))
				{
					resourceKey = ToolBar.RadioButtonStyleKey;
				}
				else if (type == typeof(ComboBox))
				{
					resourceKey = ToolBar.ComboBoxStyleKey;
				}
				else if (type == typeof(TextBox))
				{
					resourceKey = ToolBar.TextBoxStyleKey;
				}
				else if (type == typeof(Menu))
				{
					resourceKey = ToolBar.MenuStyleKey;
				}
				if (resourceKey != null)
				{
					bool flag;
					if (frameworkElement.GetValueSource(FrameworkElement.StyleProperty, null, out flag) <= BaseValueSourceInternal.ImplicitReference)
					{
						frameworkElement.SetResourceReference(FrameworkElement.StyleProperty, resourceKey);
					}
					frameworkElement.DefaultStyleKey = resourceKey;
				}
			}
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x002EC0D1 File Offset: 0x002EB0D1
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this._toolBarPanel = null;
			this._toolBarOverflowPanel = null;
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x002EC0E9 File Offset: 0x002EB0E9
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			this.InvalidateLayout();
			base.OnItemsChanged(e);
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x002EC0F8 File Offset: 0x002EB0F8
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = base.MeasureOverride(constraint);
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				Thickness margin = toolBarPanel.Margin;
				double num;
				if (toolBarPanel.Orientation == Orientation.Horizontal)
				{
					num = Math.Max(0.0, result.Width - toolBarPanel.DesiredSize.Width + margin.Left + margin.Right);
				}
				else
				{
					num = Math.Max(0.0, result.Height - toolBarPanel.DesiredSize.Height + margin.Top + margin.Bottom);
				}
				this._minLength = toolBarPanel.MinLength + num;
				this._maxLength = toolBarPanel.MaxLength + num;
			}
			return result;
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x002EC1B4 File Offset: 0x002EB1B4
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (Mouse.Captured == null)
			{
				this.Close();
			}
		}

		// Token: 0x17001B47 RID: 6983
		// (get) Token: 0x06007582 RID: 30082 RVA: 0x002EC1CA File Offset: 0x002EB1CA
		internal ToolBarPanel ToolBarPanel
		{
			get
			{
				if (this._toolBarPanel == null)
				{
					this._toolBarPanel = this.FindToolBarPanel();
				}
				return this._toolBarPanel;
			}
		}

		// Token: 0x06007583 RID: 30083 RVA: 0x002EC1E8 File Offset: 0x002EB1E8
		private ToolBarPanel FindToolBarPanel()
		{
			DependencyObject templateChild = base.GetTemplateChild("PART_ToolBarPanel");
			ToolBarPanel toolBarPanel = templateChild as ToolBarPanel;
			if (templateChild != null && toolBarPanel == null)
			{
				throw new NotSupportedException(SR.Get("ToolBar_InvalidStyle_ToolBarPanel", new object[]
				{
					templateChild.GetType()
				}));
			}
			return toolBarPanel;
		}

		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x06007584 RID: 30084 RVA: 0x002EC22E File Offset: 0x002EB22E
		internal ToolBarOverflowPanel ToolBarOverflowPanel
		{
			get
			{
				if (this._toolBarOverflowPanel == null)
				{
					this._toolBarOverflowPanel = this.FindToolBarOverflowPanel();
				}
				return this._toolBarOverflowPanel;
			}
		}

		// Token: 0x06007585 RID: 30085 RVA: 0x002EC24C File Offset: 0x002EB24C
		private ToolBarOverflowPanel FindToolBarOverflowPanel()
		{
			DependencyObject templateChild = base.GetTemplateChild("PART_ToolBarOverflowPanel");
			ToolBarOverflowPanel toolBarOverflowPanel = templateChild as ToolBarOverflowPanel;
			if (templateChild != null && toolBarOverflowPanel == null)
			{
				throw new NotSupportedException(SR.Get("ToolBar_InvalidStyle_ToolBarOverflowPanel", new object[]
				{
					templateChild.GetType()
				}));
			}
			return toolBarOverflowPanel;
		}

		// Token: 0x06007586 RID: 30086 RVA: 0x002EC294 File Offset: 0x002EB294
		protected override void OnKeyDown(KeyEventArgs e)
		{
			UIElement uielement = null;
			UIElement uielement2 = e.Source as UIElement;
			if (uielement2 != null && ItemsControl.ItemsControlFromItemContainer(uielement2) == this)
			{
				Panel panel = VisualTreeHelper.GetParent(uielement2) as Panel;
				if (panel != null)
				{
					Key key = e.Key;
					if (key != Key.Escape)
					{
						if (key != Key.End)
						{
							if (key == Key.Home)
							{
								uielement = (VisualTreeHelper.GetChild(panel, 0) as UIElement);
							}
						}
						else
						{
							uielement = (VisualTreeHelper.GetChild(panel, VisualTreeHelper.GetChildrenCount(panel) - 1) as UIElement);
						}
					}
					else
					{
						ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
						if (toolBarOverflowPanel != null && toolBarOverflowPanel.IsKeyboardFocusWithin)
						{
							this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Last));
						}
						else
						{
							Keyboard.Focus(null);
						}
						this.Close();
					}
					if (uielement != null && uielement.Focus())
					{
						e.Handled = true;
					}
				}
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x06007587 RID: 30087 RVA: 0x002EC360 File Offset: 0x002EB360
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			ToolBar toolBar = (ToolBar)sender;
			if (!e.Handled)
			{
				toolBar.Close();
				e.Handled = true;
			}
		}

		// Token: 0x06007588 RID: 30088 RVA: 0x002EC38C File Offset: 0x002EB38C
		private static void _OnClick(object e, RoutedEventArgs args)
		{
			ToolBar toolBar = (ToolBar)e;
			ButtonBase buttonBase = args.OriginalSource as ButtonBase;
			if (toolBar.IsOverflowOpen && buttonBase != null && buttonBase.Parent == toolBar)
			{
				toolBar.Close();
			}
		}

		// Token: 0x06007589 RID: 30089 RVA: 0x002EC3C6 File Offset: 0x002EB3C6
		internal override void OnAncestorChanged()
		{
			base.CoerceValue(ToolBar.OrientationProperty);
		}

		// Token: 0x0600758A RID: 30090 RVA: 0x002EC3D3 File Offset: 0x002EB3D3
		private void Close()
		{
			base.SetCurrentValueInternal(ToolBar.IsOverflowOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x0600758B RID: 30091 RVA: 0x002EC3E5 File Offset: 0x002EB3E5
		private ToolBarTray ToolBarTray
		{
			get
			{
				return base.Parent as ToolBarTray;
			}
		}

		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x0600758C RID: 30092 RVA: 0x002EC3F2 File Offset: 0x002EB3F2
		internal double MinLength
		{
			get
			{
				return this._minLength;
			}
		}

		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x0600758D RID: 30093 RVA: 0x002EC3FA File Offset: 0x002EB3FA
		internal double MaxLength
		{
			get
			{
				return this._maxLength;
			}
		}

		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x0600758E RID: 30094 RVA: 0x002EC402 File Offset: 0x002EB402
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ToolBar._dType;
			}
		}

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x0600758F RID: 30095 RVA: 0x002EC409 File Offset: 0x002EB409
		public static ResourceKey ButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarButtonStyleKey;
			}
		}

		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x06007590 RID: 30096 RVA: 0x002EC410 File Offset: 0x002EB410
		public static ResourceKey ToggleButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarToggleButtonStyleKey;
			}
		}

		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x06007591 RID: 30097 RVA: 0x002EC417 File Offset: 0x002EB417
		public static ResourceKey SeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarSeparatorStyleKey;
			}
		}

		// Token: 0x17001B50 RID: 6992
		// (get) Token: 0x06007592 RID: 30098 RVA: 0x002EC41E File Offset: 0x002EB41E
		public static ResourceKey CheckBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarCheckBoxStyleKey;
			}
		}

		// Token: 0x17001B51 RID: 6993
		// (get) Token: 0x06007593 RID: 30099 RVA: 0x002EC425 File Offset: 0x002EB425
		public static ResourceKey RadioButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarRadioButtonStyleKey;
			}
		}

		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x06007594 RID: 30100 RVA: 0x002EC42C File Offset: 0x002EB42C
		public static ResourceKey ComboBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarComboBoxStyleKey;
			}
		}

		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x06007595 RID: 30101 RVA: 0x002EC433 File Offset: 0x002EB433
		public static ResourceKey TextBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarTextBoxStyleKey;
			}
		}

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x06007596 RID: 30102 RVA: 0x002EC43A File Offset: 0x002EB43A
		public static ResourceKey MenuStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarMenuStyleKey;
			}
		}

		// Token: 0x0400384E RID: 14414
		private static readonly DependencyPropertyKey OrientationPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Orientation", typeof(Orientation), typeof(ToolBar), new FrameworkPropertyMetadata(Orientation.Horizontal, null, new CoerceValueCallback(ToolBar.CoerceOrientation)));

		// Token: 0x0400384F RID: 14415
		public static readonly DependencyProperty OrientationProperty = ToolBar.OrientationPropertyKey.DependencyProperty;

		// Token: 0x04003850 RID: 14416
		public static readonly DependencyProperty BandProperty = DependencyProperty.Register("Band", typeof(int), typeof(ToolBar), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		// Token: 0x04003851 RID: 14417
		public static readonly DependencyProperty BandIndexProperty = DependencyProperty.Register("BandIndex", typeof(int), typeof(ToolBar), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		// Token: 0x04003852 RID: 14418
		public static readonly DependencyProperty IsOverflowOpenProperty = DependencyProperty.Register("IsOverflowOpen", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ToolBar.OnOverflowOpenChanged), new CoerceValueCallback(ToolBar.CoerceIsOverflowOpen)));

		// Token: 0x04003853 RID: 14419
		internal static readonly DependencyPropertyKey HasOverflowItemsPropertyKey = DependencyProperty.RegisterReadOnly("HasOverflowItems", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003854 RID: 14420
		public static readonly DependencyProperty HasOverflowItemsProperty = ToolBar.HasOverflowItemsPropertyKey.DependencyProperty;

		// Token: 0x04003855 RID: 14421
		internal static readonly DependencyPropertyKey IsOverflowItemPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsOverflowItem", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003856 RID: 14422
		public static readonly DependencyProperty IsOverflowItemProperty = ToolBar.IsOverflowItemPropertyKey.DependencyProperty;

		// Token: 0x04003857 RID: 14423
		public static readonly DependencyProperty OverflowModeProperty = DependencyProperty.RegisterAttached("OverflowMode", typeof(OverflowMode), typeof(ToolBar), new FrameworkPropertyMetadata(OverflowMode.AsNeeded, new PropertyChangedCallback(ToolBar.OnOverflowModeChanged)), new ValidateValueCallback(ToolBar.IsValidOverflowMode));

		// Token: 0x04003858 RID: 14424
		private ToolBarPanel _toolBarPanel;

		// Token: 0x04003859 RID: 14425
		private ToolBarOverflowPanel _toolBarOverflowPanel;

		// Token: 0x0400385A RID: 14426
		private const string ToolBarPanelTemplateName = "PART_ToolBarPanel";

		// Token: 0x0400385B RID: 14427
		private const string ToolBarOverflowPanelTemplateName = "PART_ToolBarOverflowPanel";

		// Token: 0x0400385C RID: 14428
		private double _minLength;

		// Token: 0x0400385D RID: 14429
		private double _maxLength;

		// Token: 0x0400385E RID: 14430
		private static DependencyObjectType _dType;
	}
}
