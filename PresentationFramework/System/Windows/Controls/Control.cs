using System;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x0200073A RID: 1850
	public class Control : FrameworkElement
	{
		// Token: 0x060061E7 RID: 25063 RVA: 0x0029E264 File Offset: 0x0029D264
		static Control()
		{
			Control.PreviewMouseDoubleClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseDoubleClick", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(Control));
			Control.MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(Control));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Control), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			EventManager.RegisterClassHandler(typeof(Control), UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			EventManager.RegisterClassHandler(typeof(Control), UIElement.MouseRightButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			UIElement.IsKeyboardFocusedPropertyKey.OverrideMetadata(typeof(Control), new PropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x0029E614 File Offset: 0x0029D614
		public Control()
		{
			PropertyMetadata metadata = Control.TemplateProperty.GetMetadata(base.DependencyObjectType);
			ControlTemplate controlTemplate = (ControlTemplate)metadata.DefaultValue;
			if (controlTemplate != null)
			{
				Control.OnTemplateChanged(this, new DependencyPropertyChangedEventArgs(Control.TemplateProperty, metadata, null, controlTemplate));
			}
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x060061E9 RID: 25065 RVA: 0x0029E65A File Offset: 0x0029D65A
		// (set) Token: 0x060061EA RID: 25066 RVA: 0x0029E66C File Offset: 0x0029D66C
		[Category("Appearance")]
		[Bindable(true)]
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(Control.BorderBrushProperty);
			}
			set
			{
				base.SetValue(Control.BorderBrushProperty, value);
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x060061EB RID: 25067 RVA: 0x0029E67A File Offset: 0x0029D67A
		// (set) Token: 0x060061EC RID: 25068 RVA: 0x0029E68C File Offset: 0x0029D68C
		[Bindable(true)]
		[Category("Appearance")]
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(Control.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(Control.BorderThicknessProperty, value);
			}
		}

		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x060061ED RID: 25069 RVA: 0x0029E69F File Offset: 0x0029D69F
		// (set) Token: 0x060061EE RID: 25070 RVA: 0x0029E6B1 File Offset: 0x0029D6B1
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Control.BackgroundProperty);
			}
			set
			{
				base.SetValue(Control.BackgroundProperty, value);
			}
		}

		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x060061EF RID: 25071 RVA: 0x0029E6BF File Offset: 0x0029D6BF
		// (set) Token: 0x060061F0 RID: 25072 RVA: 0x0029E6D1 File Offset: 0x0029D6D1
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(Control.ForegroundProperty);
			}
			set
			{
				base.SetValue(Control.ForegroundProperty, value);
			}
		}

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x060061F1 RID: 25073 RVA: 0x0029E6DF File Offset: 0x0029D6DF
		// (set) Token: 0x060061F2 RID: 25074 RVA: 0x0029E6F1 File Offset: 0x0029D6F1
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.Font)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(Control.FontFamilyProperty);
			}
			set
			{
				base.SetValue(Control.FontFamilyProperty, value);
			}
		}

		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x060061F3 RID: 25075 RVA: 0x0029E6FF File Offset: 0x0029D6FF
		// (set) Token: 0x060061F4 RID: 25076 RVA: 0x0029E711 File Offset: 0x0029D711
		[Bindable(true)]
		[TypeConverter(typeof(FontSizeConverter))]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(Control.FontSizeProperty);
			}
			set
			{
				base.SetValue(Control.FontSizeProperty, value);
			}
		}

		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x060061F5 RID: 25077 RVA: 0x0029E724 File Offset: 0x0029D724
		// (set) Token: 0x060061F6 RID: 25078 RVA: 0x0029E736 File Offset: 0x0029D736
		[Bindable(true)]
		[Category("Appearance")]
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(Control.FontStretchProperty);
			}
			set
			{
				base.SetValue(Control.FontStretchProperty, value);
			}
		}

		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x060061F7 RID: 25079 RVA: 0x0029E749 File Offset: 0x0029D749
		// (set) Token: 0x060061F8 RID: 25080 RVA: 0x0029E75B File Offset: 0x0029D75B
		[Bindable(true)]
		[Category("Appearance")]
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(Control.FontStyleProperty);
			}
			set
			{
				base.SetValue(Control.FontStyleProperty, value);
			}
		}

		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x060061F9 RID: 25081 RVA: 0x0029E76E File Offset: 0x0029D76E
		// (set) Token: 0x060061FA RID: 25082 RVA: 0x0029E780 File Offset: 0x0029D780
		[Category("Appearance")]
		[Bindable(true)]
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(Control.FontWeightProperty);
			}
			set
			{
				base.SetValue(Control.FontWeightProperty, value);
			}
		}

		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x060061FB RID: 25083 RVA: 0x0029E793 File Offset: 0x0029D793
		// (set) Token: 0x060061FC RID: 25084 RVA: 0x0029E7A5 File Offset: 0x0029D7A5
		[Bindable(true)]
		[Category("Layout")]
		public HorizontalAlignment HorizontalContentAlignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(Control.HorizontalContentAlignmentProperty);
			}
			set
			{
				base.SetValue(Control.HorizontalContentAlignmentProperty, value);
			}
		}

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x060061FD RID: 25085 RVA: 0x0029E7B8 File Offset: 0x0029D7B8
		// (set) Token: 0x060061FE RID: 25086 RVA: 0x0029E7CA File Offset: 0x0029D7CA
		[Category("Layout")]
		[Bindable(true)]
		public VerticalAlignment VerticalContentAlignment
		{
			get
			{
				return (VerticalAlignment)base.GetValue(Control.VerticalContentAlignmentProperty);
			}
			set
			{
				base.SetValue(Control.VerticalContentAlignmentProperty, value);
			}
		}

		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x060061FF RID: 25087 RVA: 0x0029E7DD File Offset: 0x0029D7DD
		// (set) Token: 0x06006200 RID: 25088 RVA: 0x0029E7EF File Offset: 0x0029D7EF
		[Bindable(true)]
		[Category("Behavior")]
		public int TabIndex
		{
			get
			{
				return (int)base.GetValue(Control.TabIndexProperty);
			}
			set
			{
				base.SetValue(Control.TabIndexProperty, value);
			}
		}

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06006201 RID: 25089 RVA: 0x0029E802 File Offset: 0x0029D802
		// (set) Token: 0x06006202 RID: 25090 RVA: 0x0029E814 File Offset: 0x0029D814
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsTabStop
		{
			get
			{
				return (bool)base.GetValue(Control.IsTabStopProperty);
			}
			set
			{
				base.SetValue(Control.IsTabStopProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x0029E828 File Offset: 0x0029D828
		private static bool IsMarginValid(object value)
		{
			Thickness thickness = (Thickness)value;
			return thickness.Left >= 0.0 && thickness.Right >= 0.0 && thickness.Top >= 0.0 && thickness.Bottom >= 0.0;
		}

		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06006204 RID: 25092 RVA: 0x0029E889 File Offset: 0x0029D889
		// (set) Token: 0x06006205 RID: 25093 RVA: 0x0029E89B File Offset: 0x0029D89B
		[Bindable(true)]
		[Category("Layout")]
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(Control.PaddingProperty);
			}
			set
			{
				base.SetValue(Control.PaddingProperty, value);
			}
		}

		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x06006206 RID: 25094 RVA: 0x0029E8AE File Offset: 0x0029D8AE
		// (set) Token: 0x06006207 RID: 25095 RVA: 0x0029E8B6 File Offset: 0x0029D8B6
		public ControlTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(Control.TemplateProperty, value);
			}
		}

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x06006208 RID: 25096 RVA: 0x0029E8C4 File Offset: 0x0029D8C4
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06006209 RID: 25097 RVA: 0x0029E8AE File Offset: 0x0029D8AE
		// (set) Token: 0x0600620A RID: 25098 RVA: 0x0029E8CC File Offset: 0x0029D8CC
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (ControlTemplate)value;
			}
		}

		// Token: 0x0600620B RID: 25099 RVA: 0x0029E8DA File Offset: 0x0029D8DA
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((ControlTemplate)oldTemplate, (ControlTemplate)newTemplate);
		}

		// Token: 0x0600620C RID: 25100 RVA: 0x0029E8EE File Offset: 0x0029D8EE
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StyleHelper.UpdateTemplateCache((Control)d, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, Control.TemplateProperty);
		}

		// Token: 0x0600620D RID: 25101 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
		}

		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x0600620E RID: 25102 RVA: 0x00105F35 File Offset: 0x00104F35
		protected internal virtual bool HandlesScrolling
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x0600620F RID: 25103 RVA: 0x0029E918 File Offset: 0x0029D918
		// (set) Token: 0x06006210 RID: 25104 RVA: 0x0029E925 File Offset: 0x0029D925
		internal bool VisualStateChangeSuspended
		{
			get
			{
				return this.ReadControlFlag(Control.ControlBoolFlags.VisualStateChangeSuspended);
			}
			set
			{
				this.WriteControlFlag(Control.ControlBoolFlags.VisualStateChangeSuspended, value);
			}
		}

		// Token: 0x06006211 RID: 25105 RVA: 0x0029E934 File Offset: 0x0029D934
		public override string ToString()
		{
			string text;
			if (base.CheckAccess())
			{
				text = this.GetPlainText();
			}
			else
			{
				text = (string)base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback((object o) => this.GetPlainText()), null);
			}
			if (!string.IsNullOrEmpty(text))
			{
				return SR.Get("ToStringFormatString_Control", new object[]
				{
					base.ToString(),
					text
				});
			}
			return base.ToString();
		}

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x06006212 RID: 25106 RVA: 0x0029E9AB File Offset: 0x0029D9AB
		// (remove) Token: 0x06006213 RID: 25107 RVA: 0x0029E9B9 File Offset: 0x0029D9B9
		public event MouseButtonEventHandler PreviewMouseDoubleClick
		{
			add
			{
				base.AddHandler(Control.PreviewMouseDoubleClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(Control.PreviewMouseDoubleClickEvent, value);
			}
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x06006215 RID: 25109 RVA: 0x0029E9C7 File Offset: 0x0029D9C7
		// (remove) Token: 0x06006216 RID: 25110 RVA: 0x0029E9D5 File Offset: 0x0029D9D5
		public event MouseButtonEventHandler MouseDoubleClick
		{
			add
			{
				base.AddHandler(Control.MouseDoubleClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(Control.MouseDoubleClickEvent, value);
			}
		}

		// Token: 0x06006217 RID: 25111 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		protected virtual void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06006218 RID: 25112 RVA: 0x0029E9E4 File Offset: 0x0029D9E4
		private static void HandleDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				Control control = (Control)sender;
				MouseButtonEventArgs mouseButtonEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice);
				if (e.RoutedEvent == UIElement.PreviewMouseLeftButtonDownEvent || e.RoutedEvent == UIElement.PreviewMouseRightButtonDownEvent)
				{
					mouseButtonEventArgs.RoutedEvent = Control.PreviewMouseDoubleClickEvent;
					mouseButtonEventArgs.Source = e.OriginalSource;
					mouseButtonEventArgs.OverrideSource(e.Source);
					control.OnPreviewMouseDoubleClick(mouseButtonEventArgs);
				}
				else
				{
					mouseButtonEventArgs.RoutedEvent = Control.MouseDoubleClickEvent;
					mouseButtonEventArgs.Source = e.OriginalSource;
					mouseButtonEventArgs.OverrideSource(e.Source);
					control.OnMouseDoubleClick(mouseButtonEventArgs);
				}
				if (mouseButtonEventArgs.Handled)
				{
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006219 RID: 25113 RVA: 0x0029EAA1 File Offset: 0x0029DAA1
		internal override void OnPreApplyTemplate()
		{
			this.VisualStateChangeSuspended = true;
			base.OnPreApplyTemplate();
		}

		// Token: 0x0600621A RID: 25114 RVA: 0x0029EAB0 File Offset: 0x0029DAB0
		internal override void OnPostApplyTemplate()
		{
			base.OnPostApplyTemplate();
			this.VisualStateChangeSuspended = false;
			this.UpdateVisualState(false);
		}

		// Token: 0x0600621B RID: 25115 RVA: 0x0029EAC6 File Offset: 0x0029DAC6
		internal void UpdateVisualState()
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x0600621C RID: 25116 RVA: 0x0029EACF File Offset: 0x0029DACF
		internal void UpdateVisualState(bool useTransitions)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Info, EventTrace.Event.UpdateVisualStateStart);
			if (!this.VisualStateChangeSuspended)
			{
				this.ChangeVisualState(useTransitions);
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Info, EventTrace.Event.UpdateVisualStateEnd);
		}

		// Token: 0x0600621D RID: 25117 RVA: 0x0029EAF1 File Offset: 0x0029DAF1
		internal virtual void ChangeVisualState(bool useTransitions)
		{
			this.ChangeValidationVisualState(useTransitions);
		}

		// Token: 0x0600621E RID: 25118 RVA: 0x0029EAFA File Offset: 0x0029DAFA
		internal void ChangeValidationVisualState(bool useTransitions)
		{
			if (!Validation.GetHasError(this))
			{
				VisualStateManager.GoToState(this, "Valid", useTransitions);
				return;
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "InvalidFocused", useTransitions);
				return;
			}
			VisualStateManager.GoToState(this, "InvalidUnfocused", useTransitions);
		}

		// Token: 0x0600621F RID: 25119 RVA: 0x0029EB38 File Offset: 0x0029DB38
		internal static void OnVisualStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control control = d as Control;
			if (control != null)
			{
				control.UpdateVisualState();
			}
		}

		// Token: 0x06006220 RID: 25120 RVA: 0x0029EB58 File Offset: 0x0029DB58
		protected override Size MeasureOverride(Size constraint)
		{
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = (UIElement)this.GetVisualChild(0);
				if (uielement != null)
				{
					uielement.Measure(constraint);
					return uielement.DesiredSize;
				}
			}
			return new Size(0.0, 0.0);
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x0029EBA4 File Offset: 0x0029DBA4
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			if (this.VisualChildrenCount > 0)
			{
				UIElement uielement = (UIElement)this.GetVisualChild(0);
				if (uielement != null)
				{
					uielement.Arrange(new Rect(arrangeBounds));
				}
			}
			return arrangeBounds;
		}

		// Token: 0x06006222 RID: 25122 RVA: 0x0029EBD7 File Offset: 0x0029DBD7
		internal bool ReadControlFlag(Control.ControlBoolFlags reqFlag)
		{
			return (this._controlBoolField & reqFlag) > (Control.ControlBoolFlags)0;
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x0029EBE4 File Offset: 0x0029DBE4
		internal void WriteControlFlag(Control.ControlBoolFlags reqFlag, bool set)
		{
			if (set)
			{
				this._controlBoolField |= reqFlag;
				return;
			}
			this._controlBoolField &= ~reqFlag;
		}

		// Token: 0x0400329C RID: 12956
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderBrushProperty = Border.BorderBrushProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));

		// Token: 0x0400329D RID: 12957
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderThicknessProperty = Border.BorderThicknessProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(Border.BorderThicknessProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));

		// Token: 0x0400329E RID: 12958
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(Panel.BackgroundProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));

		// Token: 0x0400329F RID: 12959
		[CommonDependencyProperty]
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040032A0 RID: 12960
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040032A1 RID: 12961
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040032A2 RID: 12962
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(TextElement.FontStretchProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040032A3 RID: 12963
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040032A4 RID: 12964
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x040032A5 RID: 12965
		[CommonDependencyProperty]
		public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(Control), new FrameworkPropertyMetadata(HorizontalAlignment.Left), new ValidateValueCallback(FrameworkElement.ValidateHorizontalAlignmentValue));

		// Token: 0x040032A6 RID: 12966
		[CommonDependencyProperty]
		public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(Control), new FrameworkPropertyMetadata(VerticalAlignment.Top), new ValidateValueCallback(FrameworkElement.ValidateVerticalAlignmentValue));

		// Token: 0x040032A7 RID: 12967
		[CommonDependencyProperty]
		public static readonly DependencyProperty TabIndexProperty = KeyboardNavigation.TabIndexProperty.AddOwner(typeof(Control));

		// Token: 0x040032A8 RID: 12968
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsTabStopProperty = KeyboardNavigation.IsTabStopProperty.AddOwner(typeof(Control));

		// Token: 0x040032A9 RID: 12969
		[CommonDependencyProperty]
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Control), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		// Token: 0x040032AA RID: 12970
		[CommonDependencyProperty]
		public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(Control), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(Control.OnTemplateChanged)));

		// Token: 0x040032AD RID: 12973
		private ControlTemplate _templateCache;

		// Token: 0x040032AE RID: 12974
		internal Control.ControlBoolFlags _controlBoolField;

		// Token: 0x02000BC7 RID: 3015
		internal enum ControlBoolFlags : ushort
		{
			// Token: 0x040049DB RID: 18907
			ContentIsNotLogical = 1,
			// Token: 0x040049DC RID: 18908
			IsSpaceKeyDown,
			// Token: 0x040049DD RID: 18909
			HeaderIsNotLogical = 4,
			// Token: 0x040049DE RID: 18910
			CommandDisabled = 8,
			// Token: 0x040049DF RID: 18911
			ContentIsItem = 16,
			// Token: 0x040049E0 RID: 18912
			HeaderIsItem = 32,
			// Token: 0x040049E1 RID: 18913
			ScrollHostValid = 64,
			// Token: 0x040049E2 RID: 18914
			ContainsSelection = 128,
			// Token: 0x040049E3 RID: 18915
			VisualStateChangeSuspended = 256
		}
	}
}
