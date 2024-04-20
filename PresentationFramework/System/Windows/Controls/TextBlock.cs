using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Text;

namespace System.Windows.Controls
{
	// Token: 0x020007E4 RID: 2020
	[ContentProperty("Inlines")]
	[Localizability(LocalizationCategory.Text)]
	public class TextBlock : FrameworkElement, IContentHost, IAddChildInternal, IAddChild, IServiceProvider
	{
		// Token: 0x0600743C RID: 29756 RVA: 0x002E5FC4 File Offset: 0x002E4FC4
		IInputElement IContentHost.InputHitTest(Point point)
		{
			return this.InputHitTestCore(point);
		}

		// Token: 0x0600743D RID: 29757 RVA: 0x002E5FCD File Offset: 0x002E4FCD
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			return this.GetRectanglesCore(child);
		}

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x0600743E RID: 29758 RVA: 0x002E5FD6 File Offset: 0x002E4FD6
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				return this.HostedElementsCore;
			}
		}

		// Token: 0x0600743F RID: 29759 RVA: 0x002E5FDE File Offset: 0x002E4FDE
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			this.OnChildDesiredSizeChangedCore(child);
		}

		// Token: 0x06007440 RID: 29760 RVA: 0x002E5FE8 File Offset: 0x002E4FE8
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.EnsureComplexContent();
			if (!(this._complexContent.TextContainer is TextContainer))
			{
				throw new ArgumentException(SR.Get("TextPanelIllegalParaTypeForIAddChild", new object[]
				{
					"value",
					value.GetType()
				}));
			}
			Type type = this._complexContent.TextContainer.Parent.GetType();
			Type type2 = value.GetType();
			if (!TextSchema.IsValidChildOfContainer(type, type2))
			{
				if (!(value is UIElement))
				{
					throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
					{
						type.Name,
						type2.Name
					}));
				}
				value = new InlineUIContainer((UIElement)value);
			}
			Invariant.Assert(value is Inline, "Schema validation helper must guarantee that invalid element is not passed here");
			TextContainer textContainer = (TextContainer)this._complexContent.TextContainer;
			textContainer.BeginChange();
			try
			{
				TextPointer end = textContainer.End;
				textContainer.InsertElementInternal(end, end, (Inline)value);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x06007441 RID: 29761 RVA: 0x002E6100 File Offset: 0x002E5100
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (this._complexContent == null)
			{
				this.Text += text;
				return;
			}
			TextContainer textContainer = (TextContainer)this._complexContent.TextContainer;
			textContainer.BeginChange();
			try
			{
				TextPointer end = textContainer.End;
				Run run = Inline.CreateImplicitRun(this);
				textContainer.InsertElementInternal(end, end, run);
				run.Text = text;
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x06007442 RID: 29762 RVA: 0x002E6184 File Offset: 0x002E5184
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.IsContentPresenterContainer)
				{
					return EmptyEnumerator.Instance;
				}
				if (this._complexContent == null)
				{
					return new TextBlock.SimpleContentEnumerator(this.Text);
				}
				if (!this._complexContent.ForeignTextContainer)
				{
					return new RangeContentEnumerator(this.ContentStart, this.ContentEnd);
				}
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x06007443 RID: 29763 RVA: 0x002E61D8 File Offset: 0x002E51D8
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				this.EnsureComplexContent();
				return this._complexContent.TextView;
			}
			if (serviceType == typeof(ITextContainer))
			{
				this.EnsureComplexContent();
				return this._complexContent.TextContainer;
			}
			if (serviceType == typeof(TextContainer))
			{
				this.EnsureComplexContent();
				return this._complexContent.TextContainer as TextContainer;
			}
			return null;
		}

		// Token: 0x06007444 RID: 29764 RVA: 0x002E626C File Offset: 0x002E526C
		static TextBlock()
		{
			TextBlock.BaselineOffsetProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(null, new CoerceValueCallback(TextBlock.CoerceBaselineOffset)));
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(TextBlock.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(propertyChangedCallback));
			}
			EventManager.RegisterClassHandler(typeof(TextBlock), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(TextBlock.OnRequestBringIntoView));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBlock), new FrameworkPropertyMetadata(typeof(TextBlock)));
			ControlsTraceLogger.AddControl(TelemetryControls.TextBlock);
		}

		// Token: 0x06007445 RID: 29765 RVA: 0x002E65D0 File Offset: 0x002E55D0
		public TextBlock()
		{
			this.Initialize();
		}

		// Token: 0x06007446 RID: 29766 RVA: 0x002E65DE File Offset: 0x002E55DE
		public TextBlock(Inline inline)
		{
			this.Initialize();
			if (inline == null)
			{
				throw new ArgumentNullException("inline");
			}
			this.Inlines.Add(inline);
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private void Initialize()
		{
		}

		// Token: 0x06007448 RID: 29768 RVA: 0x002E6608 File Offset: 0x002E5608
		public TextPointer GetPositionFromPoint(Point point, bool snapToText)
		{
			if (this.CheckFlags(TextBlock.Flags.ContentChangeInProgress))
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			this.EnsureComplexContent();
			TextPointer result;
			if (((ITextView)this._complexContent.TextView).Validate(point))
			{
				result = (TextPointer)this._complexContent.TextView.GetTextPositionFromPoint(point, snapToText);
			}
			else
			{
				result = (snapToText ? new TextPointer((TextPointer)this._complexContent.TextContainer.Start) : null);
			}
			return result;
		}

		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x06007449 RID: 29769 RVA: 0x002451C0 File Offset: 0x002441C0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public InlineCollection Inlines
		{
			get
			{
				return new InlineCollection(this, true);
			}
		}

		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x0600744A RID: 29770 RVA: 0x002E6684 File Offset: 0x002E5684
		public TextPointer ContentStart
		{
			get
			{
				this.EnsureComplexContent();
				return (TextPointer)this._complexContent.TextContainer.Start;
			}
		}

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x0600744B RID: 29771 RVA: 0x002E66A1 File Offset: 0x002E56A1
		public TextPointer ContentEnd
		{
			get
			{
				this.EnsureComplexContent();
				return (TextPointer)this._complexContent.TextContainer.End;
			}
		}

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x0600744C RID: 29772 RVA: 0x002E66BE File Offset: 0x002E56BE
		internal TextRange TextRange
		{
			get
			{
				return new TextRange(this.ContentStart, this.ContentEnd);
			}
		}

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x0600744D RID: 29773 RVA: 0x00105F35 File Offset: 0x00104F35
		public LineBreakCondition BreakBefore
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x0600744E RID: 29774 RVA: 0x00105F35 File Offset: 0x00104F35
		public LineBreakCondition BreakAfter
		{
			get
			{
				return LineBreakCondition.BreakDesired;
			}
		}

		// Token: 0x17001B01 RID: 6913
		// (get) Token: 0x0600744F RID: 29775 RVA: 0x0023D24E File Offset: 0x0023C24E
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x06007450 RID: 29776 RVA: 0x002E66D1 File Offset: 0x002E56D1
		// (set) Token: 0x06007451 RID: 29777 RVA: 0x002E66E3 File Offset: 0x002E56E3
		public double BaselineOffset
		{
			get
			{
				return (double)base.GetValue(TextBlock.BaselineOffsetProperty);
			}
			set
			{
				base.SetValue(TextBlock.BaselineOffsetProperty, value);
			}
		}

		// Token: 0x06007452 RID: 29778 RVA: 0x002E66F6 File Offset: 0x002E56F6
		public static void SetBaselineOffset(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.BaselineOffsetProperty, value);
		}

		// Token: 0x06007453 RID: 29779 RVA: 0x002E6717 File Offset: 0x002E5717
		public static double GetBaselineOffset(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextBlock.BaselineOffsetProperty);
		}

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x06007454 RID: 29780 RVA: 0x002E6737 File Offset: 0x002E5737
		// (set) Token: 0x06007455 RID: 29781 RVA: 0x002E6749 File Offset: 0x002E5749
		[Localizability(LocalizationCategory.Text)]
		public string Text
		{
			get
			{
				return (string)base.GetValue(TextBlock.TextProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextProperty, value);
			}
		}

		// Token: 0x06007456 RID: 29782 RVA: 0x002E6758 File Offset: 0x002E5758
		private static object CoerceText(DependencyObject d, object value)
		{
			TextBlock textBlock = (TextBlock)d;
			if (value == null)
			{
				value = string.Empty;
			}
			if (textBlock._complexContent != null && !textBlock.CheckFlags(TextBlock.Flags.TextContentChanging) && (string)value == (string)textBlock.GetValue(TextBlock.TextProperty))
			{
				TextBlock.OnTextChanged(d, (string)value);
			}
			return value;
		}

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x06007457 RID: 29783 RVA: 0x002E67B5 File Offset: 0x002E57B5
		// (set) Token: 0x06007458 RID: 29784 RVA: 0x002E67C7 File Offset: 0x002E57C7
		[Localizability(LocalizationCategory.Font)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(TextBlock.FontFamilyProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontFamilyProperty, value);
			}
		}

		// Token: 0x06007459 RID: 29785 RVA: 0x002E67D5 File Offset: 0x002E57D5
		public static void SetFontFamily(DependencyObject element, FontFamily value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontFamilyProperty, value);
		}

		// Token: 0x0600745A RID: 29786 RVA: 0x002E67F1 File Offset: 0x002E57F1
		public static FontFamily GetFontFamily(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontFamily)element.GetValue(TextBlock.FontFamilyProperty);
		}

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x0600745B RID: 29787 RVA: 0x002E6811 File Offset: 0x002E5811
		// (set) Token: 0x0600745C RID: 29788 RVA: 0x002E6823 File Offset: 0x002E5823
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(TextBlock.FontStyleProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontStyleProperty, value);
			}
		}

		// Token: 0x0600745D RID: 29789 RVA: 0x002E6836 File Offset: 0x002E5836
		public static void SetFontStyle(DependencyObject element, FontStyle value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontStyleProperty, value);
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x002E6857 File Offset: 0x002E5857
		public static FontStyle GetFontStyle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStyle)element.GetValue(TextBlock.FontStyleProperty);
		}

		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x0600745F RID: 29791 RVA: 0x002E6877 File Offset: 0x002E5877
		// (set) Token: 0x06007460 RID: 29792 RVA: 0x002E6889 File Offset: 0x002E5889
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(TextBlock.FontWeightProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontWeightProperty, value);
			}
		}

		// Token: 0x06007461 RID: 29793 RVA: 0x002E689C File Offset: 0x002E589C
		public static void SetFontWeight(DependencyObject element, FontWeight value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontWeightProperty, value);
		}

		// Token: 0x06007462 RID: 29794 RVA: 0x002E68BD File Offset: 0x002E58BD
		public static FontWeight GetFontWeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontWeight)element.GetValue(TextBlock.FontWeightProperty);
		}

		// Token: 0x17001B07 RID: 6919
		// (get) Token: 0x06007463 RID: 29795 RVA: 0x002E68DD File Offset: 0x002E58DD
		// (set) Token: 0x06007464 RID: 29796 RVA: 0x002E68EF File Offset: 0x002E58EF
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(TextBlock.FontStretchProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontStretchProperty, value);
			}
		}

		// Token: 0x06007465 RID: 29797 RVA: 0x002E6902 File Offset: 0x002E5902
		public static void SetFontStretch(DependencyObject element, FontStretch value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontStretchProperty, value);
		}

		// Token: 0x06007466 RID: 29798 RVA: 0x002E6923 File Offset: 0x002E5923
		public static FontStretch GetFontStretch(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStretch)element.GetValue(TextBlock.FontStretchProperty);
		}

		// Token: 0x17001B08 RID: 6920
		// (get) Token: 0x06007467 RID: 29799 RVA: 0x002E6943 File Offset: 0x002E5943
		// (set) Token: 0x06007468 RID: 29800 RVA: 0x002E6955 File Offset: 0x002E5955
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(TextBlock.FontSizeProperty);
			}
			set
			{
				base.SetValue(TextBlock.FontSizeProperty, value);
			}
		}

		// Token: 0x06007469 RID: 29801 RVA: 0x002E6968 File Offset: 0x002E5968
		public static void SetFontSize(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.FontSizeProperty, value);
		}

		// Token: 0x0600746A RID: 29802 RVA: 0x002E6989 File Offset: 0x002E5989
		[TypeConverter(typeof(FontSizeConverter))]
		public static double GetFontSize(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextBlock.FontSizeProperty);
		}

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x0600746B RID: 29803 RVA: 0x002E69A9 File Offset: 0x002E59A9
		// (set) Token: 0x0600746C RID: 29804 RVA: 0x002E69BB File Offset: 0x002E59BB
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(TextBlock.ForegroundProperty);
			}
			set
			{
				base.SetValue(TextBlock.ForegroundProperty, value);
			}
		}

		// Token: 0x0600746D RID: 29805 RVA: 0x002E69C9 File Offset: 0x002E59C9
		public static void SetForeground(DependencyObject element, Brush value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.ForegroundProperty, value);
		}

		// Token: 0x0600746E RID: 29806 RVA: 0x002E69E5 File Offset: 0x002E59E5
		public static Brush GetForeground(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Brush)element.GetValue(TextBlock.ForegroundProperty);
		}

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x0600746F RID: 29807 RVA: 0x002E6A05 File Offset: 0x002E5A05
		// (set) Token: 0x06007470 RID: 29808 RVA: 0x002E6A17 File Offset: 0x002E5A17
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(TextBlock.BackgroundProperty);
			}
			set
			{
				base.SetValue(TextBlock.BackgroundProperty, value);
			}
		}

		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x06007471 RID: 29809 RVA: 0x002E6A25 File Offset: 0x002E5A25
		// (set) Token: 0x06007472 RID: 29810 RVA: 0x002E6A37 File Offset: 0x002E5A37
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(TextBlock.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextDecorationsProperty, value);
			}
		}

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x06007473 RID: 29811 RVA: 0x002E6A45 File Offset: 0x002E5A45
		// (set) Token: 0x06007474 RID: 29812 RVA: 0x002E6A57 File Offset: 0x002E5A57
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(TextBlock.TextEffectsProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextEffectsProperty, value);
			}
		}

		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x06007475 RID: 29813 RVA: 0x002E6A65 File Offset: 0x002E5A65
		// (set) Token: 0x06007476 RID: 29814 RVA: 0x002E6A77 File Offset: 0x002E5A77
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(TextBlock.LineHeightProperty);
			}
			set
			{
				base.SetValue(TextBlock.LineHeightProperty, value);
			}
		}

		// Token: 0x06007477 RID: 29815 RVA: 0x002E6A8A File Offset: 0x002E5A8A
		public static void SetLineHeight(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.LineHeightProperty, value);
		}

		// Token: 0x06007478 RID: 29816 RVA: 0x002E6AAB File Offset: 0x002E5AAB
		[TypeConverter(typeof(LengthConverter))]
		public static double GetLineHeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextBlock.LineHeightProperty);
		}

		// Token: 0x17001B0E RID: 6926
		// (get) Token: 0x06007479 RID: 29817 RVA: 0x002E6ACB File Offset: 0x002E5ACB
		// (set) Token: 0x0600747A RID: 29818 RVA: 0x002E6ADD File Offset: 0x002E5ADD
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(TextBlock.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(TextBlock.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x0600747B RID: 29819 RVA: 0x002E6AF0 File Offset: 0x002E5AF0
		public static void SetLineStackingStrategy(DependencyObject element, LineStackingStrategy value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.LineStackingStrategyProperty, value);
		}

		// Token: 0x0600747C RID: 29820 RVA: 0x002E6B11 File Offset: 0x002E5B11
		public static LineStackingStrategy GetLineStackingStrategy(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (LineStackingStrategy)element.GetValue(TextBlock.LineStackingStrategyProperty);
		}

		// Token: 0x17001B0F RID: 6927
		// (get) Token: 0x0600747D RID: 29821 RVA: 0x002E6B31 File Offset: 0x002E5B31
		// (set) Token: 0x0600747E RID: 29822 RVA: 0x002E6B43 File Offset: 0x002E5B43
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(TextBlock.PaddingProperty);
			}
			set
			{
				base.SetValue(TextBlock.PaddingProperty, value);
			}
		}

		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x0600747F RID: 29823 RVA: 0x002E6B56 File Offset: 0x002E5B56
		// (set) Token: 0x06007480 RID: 29824 RVA: 0x002E6B68 File Offset: 0x002E5B68
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(TextBlock.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextAlignmentProperty, value);
			}
		}

		// Token: 0x06007481 RID: 29825 RVA: 0x002E6B7B File Offset: 0x002E5B7B
		public static void SetTextAlignment(DependencyObject element, TextAlignment value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextBlock.TextAlignmentProperty, value);
		}

		// Token: 0x06007482 RID: 29826 RVA: 0x002E6B9C File Offset: 0x002E5B9C
		public static TextAlignment GetTextAlignment(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextAlignment)element.GetValue(TextBlock.TextAlignmentProperty);
		}

		// Token: 0x17001B11 RID: 6929
		// (get) Token: 0x06007483 RID: 29827 RVA: 0x002E6BBC File Offset: 0x002E5BBC
		// (set) Token: 0x06007484 RID: 29828 RVA: 0x002E6BCE File Offset: 0x002E5BCE
		public TextTrimming TextTrimming
		{
			get
			{
				return (TextTrimming)base.GetValue(TextBlock.TextTrimmingProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextTrimmingProperty, value);
			}
		}

		// Token: 0x17001B12 RID: 6930
		// (get) Token: 0x06007485 RID: 29829 RVA: 0x002E6BE1 File Offset: 0x002E5BE1
		// (set) Token: 0x06007486 RID: 29830 RVA: 0x002E6BF3 File Offset: 0x002E5BF3
		public TextWrapping TextWrapping
		{
			get
			{
				return (TextWrapping)base.GetValue(TextBlock.TextWrappingProperty);
			}
			set
			{
				base.SetValue(TextBlock.TextWrappingProperty, value);
			}
		}

		// Token: 0x17001B13 RID: 6931
		// (get) Token: 0x06007487 RID: 29831 RVA: 0x002E6C06 File Offset: 0x002E5C06
		// (set) Token: 0x06007488 RID: 29832 RVA: 0x002E6C18 File Offset: 0x002E5C18
		public bool IsHyphenationEnabled
		{
			get
			{
				return (bool)base.GetValue(TextBlock.IsHyphenationEnabledProperty);
			}
			set
			{
				base.SetValue(TextBlock.IsHyphenationEnabledProperty, value);
			}
		}

		// Token: 0x17001B14 RID: 6932
		// (get) Token: 0x06007489 RID: 29833 RVA: 0x002E6C26 File Offset: 0x002E5C26
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._complexContent != null)
				{
					return this._complexContent.VisualChildren.Count;
				}
				return 0;
			}
		}

		// Token: 0x0600748A RID: 29834 RVA: 0x002E6C42 File Offset: 0x002E5C42
		protected override Visual GetVisualChild(int index)
		{
			if (this._complexContent == null)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._complexContent.VisualChildren[index];
		}

		// Token: 0x0600748B RID: 29835 RVA: 0x002E6C68 File Offset: 0x002E5C68
		protected sealed override Size MeasureOverride(Size constraint)
		{
			this.VerifyReentrancy();
			this._textBlockCache = null;
			this.EnsureTextBlockCache();
			LineProperties lineProperties = this._textBlockCache._lineProperties;
			if (this.CheckFlags(TextBlock.Flags.PendingTextContainerEventInit))
			{
				Invariant.Assert(this._complexContent != null);
				this.InitializeTextContainerListeners();
				this.SetFlags(false, TextBlock.Flags.PendingTextContainerEventInit);
			}
			int lineCount = this.LineCount;
			if (lineCount > 0 && base.IsMeasureValid && this.InlineObjects == null)
			{
				bool flag;
				if (lineProperties.TextTrimming == TextTrimming.None)
				{
					flag = (DoubleUtil.AreClose(constraint.Width, this._referenceSize.Width) || lineProperties.TextWrapping == TextWrapping.NoWrap);
				}
				else
				{
					flag = (DoubleUtil.AreClose(constraint.Width, this._referenceSize.Width) && lineProperties.TextWrapping == TextWrapping.NoWrap && (DoubleUtil.AreClose(constraint.Height, this._referenceSize.Height) || lineCount == 1));
				}
				if (flag)
				{
					this._referenceSize = constraint;
					return this._previousDesiredSize;
				}
			}
			this._referenceSize = constraint;
			this.CheckFlags(TextBlock.Flags.FormattedOnce);
			double baselineOffset = this._baselineOffset;
			this.InlineObjects = null;
			int capacity = (this._subsequentLines == null) ? 1 : this._subsequentLines.Count;
			this.ClearLineMetrics();
			if (this._complexContent != null)
			{
				this._complexContent.TextView.Invalidate();
			}
			lineProperties.IgnoreTextAlignment = true;
			this.SetFlags(true, TextBlock.Flags.RequiresAlignment);
			this.SetFlags(true, TextBlock.Flags.FormattedOnce);
			this.SetFlags(false, TextBlock.Flags.HasParagraphEllipses);
			this.SetFlags(true, TextBlock.Flags.MeasureInProgress | TextBlock.Flags.TreeInReadOnlyMode);
			Size size = default(Size);
			bool flag2 = true;
			try
			{
				Line line = this.CreateLine(lineProperties);
				bool flag3 = false;
				int num = 0;
				TextLineBreak textLineBreak = null;
				Thickness padding = this.Padding;
				Size size2 = new Size(Math.Max(0.0, constraint.Width - (padding.Left + padding.Right)), Math.Max(0.0, constraint.Height - (padding.Top + padding.Bottom)));
				TextDpi.EnsureValidLineWidth(ref size2);
				while (!flag3)
				{
					using (line)
					{
						line.Format(num, size2.Width, this.GetLineProperties(num == 0, lineProperties), textLineBreak, this._textBlockCache._textRunCache, false);
						double num2 = this.CalcLineAdvance(line.Height, lineProperties);
						LineMetrics lineMetrics = new LineMetrics(line.Length, line.Width, num2, line.BaselineOffset, line.HasInlineObjects(), textLineBreak);
						if (!this.CheckFlags(TextBlock.Flags.HasFirstLine))
						{
							this.SetFlags(true, TextBlock.Flags.HasFirstLine);
							this._firstLine = lineMetrics;
						}
						else
						{
							if (this._subsequentLines == null)
							{
								this._subsequentLines = new List<LineMetrics>(capacity);
							}
							this._subsequentLines.Add(lineMetrics);
						}
						size.Width = Math.Max(size.Width, line.GetCollapsedWidth());
						if (lineProperties.TextTrimming == TextTrimming.None || size2.Height >= size.Height + num2 || num == 0)
						{
							this._baselineOffset = size.Height + line.BaselineOffset;
							size.Height += num2;
						}
						else
						{
							this.SetFlags(true, TextBlock.Flags.HasParagraphEllipses);
						}
						textLineBreak = line.GetTextLineBreak();
						flag3 = line.EndOfParagraph;
						num += line.Length;
						if (!flag3 && lineProperties.TextWrapping == TextWrapping.NoWrap && line.Length == 9600)
						{
							flag3 = true;
						}
					}
				}
				size.Width += padding.Left + padding.Right;
				size.Height += padding.Top + padding.Bottom;
				Invariant.Assert(textLineBreak == null);
				flag2 = false;
			}
			finally
			{
				lineProperties.IgnoreTextAlignment = false;
				this.SetFlags(false, TextBlock.Flags.MeasureInProgress | TextBlock.Flags.TreeInReadOnlyMode);
				if (flag2)
				{
					this._textBlockCache._textRunCache = null;
					this.ClearLineMetrics();
				}
			}
			if (!DoubleUtil.AreClose(baselineOffset, this._baselineOffset))
			{
				base.CoerceValue(TextBlock.BaselineOffsetProperty);
			}
			this._previousDesiredSize = size;
			return size;
		}

		// Token: 0x0600748C RID: 29836 RVA: 0x002E7094 File Offset: 0x002E6094
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			this.VerifyReentrancy();
			if (this._complexContent != null)
			{
				this._complexContent.VisualChildren.Clear();
			}
			bool inlineObjects = this.InlineObjects != null;
			int lineCount = this.LineCount;
			if (inlineObjects && lineCount > 0)
			{
				bool flag = true;
				this.SetFlags(true, TextBlock.Flags.TreeInReadOnlyMode);
				this.SetFlags(true, TextBlock.Flags.ArrangeInProgress);
				try
				{
					this.EnsureTextBlockCache();
					LineProperties lineProperties = this._textBlockCache._lineProperties;
					double wrappingWidth = this.CalcWrappingWidth(arrangeSize.Width);
					Vector vector = this.CalcContentOffset(arrangeSize, wrappingWidth);
					Line line = this.CreateLine(lineProperties);
					int num = 0;
					Vector lineOffset = vector;
					for (int i = 0; i < lineCount; i++)
					{
						LineMetrics line2 = this.GetLine(i);
						if (line2.HasInlineObjects)
						{
							using (line)
							{
								bool ellipsis = this.ParagraphEllipsisShownOnLine(i, lineOffset.Y - vector.Y);
								this.Format(line, line2.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line2.TextLineBreak, this._textBlockCache._textRunCache, ellipsis);
								line.Arrange(this._complexContent.VisualChildren, lineOffset);
							}
						}
						lineOffset.Y += line2.Height;
						num += line2.Length;
					}
					flag = false;
				}
				finally
				{
					this.SetFlags(false, TextBlock.Flags.TreeInReadOnlyMode);
					this.SetFlags(false, TextBlock.Flags.ArrangeInProgress);
					if (flag)
					{
						this._textBlockCache._textRunCache = null;
						this.ClearLineMetrics();
					}
				}
			}
			if (this._complexContent != null)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnValidateTextView), EventArgs.Empty);
			}
			base.InvalidateVisual();
			return arrangeSize;
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x002E725C File Offset: 0x002E625C
		protected sealed override void OnRender(DrawingContext ctx)
		{
			this.VerifyReentrancy();
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (!this.IsLayoutDataValid)
			{
				return;
			}
			Brush background = this.Background;
			if (background != null)
			{
				ctx.DrawRectangle(background, null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
			}
			this.SetFlags(false, TextBlock.Flags.RequiresAlignment);
			this.SetFlags(true, TextBlock.Flags.TreeInReadOnlyMode);
			try
			{
				this.EnsureTextBlockCache();
				LineProperties lineProperties = this._textBlockCache._lineProperties;
				double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
				Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
				Point lineOffset = new Point(vector.X, vector.Y);
				Line line = this.CreateLine(lineProperties);
				int num = 0;
				bool flag = false;
				this.SetFlags(this.CheckFlags(TextBlock.Flags.HasParagraphEllipses), TextBlock.Flags.RequiresAlignment);
				int lineCount = this.LineCount;
				for (int i = 0; i < lineCount; i++)
				{
					LineMetrics metrics = this.GetLine(i);
					double value = Math.Max(0.0, base.RenderSize.Height - this.Padding.Bottom);
					if (this.CheckFlags(TextBlock.Flags.HasParagraphEllipses) && i + 1 < lineCount)
					{
						double value2 = this.GetLine(i + 1).Height + metrics.Height + lineOffset.Y;
						flag = (DoubleUtil.GreaterThan(value2, value) && !DoubleUtil.AreClose(value2, value));
					}
					if (!this.CheckFlags(TextBlock.Flags.HasParagraphEllipses) || DoubleUtil.LessThanOrClose(metrics.Height + lineOffset.Y, value) || i == 0)
					{
						using (line)
						{
							this.Format(line, metrics.Length, num, wrappingWidth, this.GetLineProperties(num == 0, flag, lineProperties), metrics.TextLineBreak, this._textBlockCache._textRunCache, flag);
							if (!this.CheckFlags(TextBlock.Flags.HasParagraphEllipses))
							{
								metrics = this.UpdateLine(i, metrics, line.Start, line.Width);
							}
							line.Render(ctx, lineOffset);
							lineOffset.Y += metrics.Height;
							num += metrics.Length;
						}
					}
				}
			}
			finally
			{
				this.SetFlags(false, TextBlock.Flags.TreeInReadOnlyMode);
				this._textBlockCache = null;
			}
		}

		// Token: 0x0600748E RID: 29838 RVA: 0x002E74F4 File Offset: 0x002E64F4
		protected sealed override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if ((e.IsAValueChange || e.IsASubPropertyChange) && this.CheckFlags(TextBlock.Flags.FormattedOnce))
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null)
				{
					bool flag = frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || flag)
					{
						this.VerifyTreeIsUnlocked();
						this._textBlockCache = null;
					}
				}
			}
		}

		// Token: 0x0600748F RID: 29839 RVA: 0x002E7578 File Offset: 0x002E6578
		protected sealed override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			this.VerifyReentrancy();
			if (hitTestParameters == null)
			{
				throw new ArgumentNullException("hitTestParameters");
			}
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(hitTestParameters.HitPoint))
			{
				return new PointHitTestResult(this, hitTestParameters.HitPoint);
			}
			return null;
		}

		// Token: 0x06007490 RID: 29840 RVA: 0x002E75CC File Offset: 0x002E65CC
		protected virtual IInputElement InputHitTestCore(Point point)
		{
			if (!this.IsLayoutDataValid)
			{
				return this;
			}
			LineProperties lineProperties = this.GetLineProperties();
			IInputElement inputElement = null;
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			point -= vector;
			if (point.X < 0.0 || point.Y < 0.0)
			{
				return this;
			}
			inputElement = null;
			int num = 0;
			double num2 = 0.0;
			TextRunCache textRunCache = new TextRunCache();
			int lineCount = this.LineCount;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line = this.GetLine(i);
				if (num2 + line.Height > point.Y)
				{
					Line line2 = this.CreateLine(lineProperties);
					using (line2)
					{
						bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num2);
						this.Format(line2, line.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line.TextLineBreak, textRunCache, ellipsis);
						if (line2.Start <= point.X && line2.Start + line2.Width >= point.X)
						{
							inputElement = line2.InputHitTest(point.X);
						}
						break;
					}
				}
				num += line.Length;
				num2 += line.Height;
			}
			if (inputElement == null)
			{
				return this;
			}
			return inputElement;
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x002E7750 File Offset: 0x002E6750
		protected virtual ReadOnlyCollection<Rect> GetRectanglesCore(ContentElement child)
		{
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (!this.IsLayoutDataValid)
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			LineProperties lineProperties = this.GetLineProperties();
			if (this._complexContent == null || !(this._complexContent.TextContainer is TextContainer))
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			TextPointer textPointer = this.FindElementPosition(child);
			if (textPointer == null)
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			TextPointer textPointer2 = null;
			if (child is TextElement)
			{
				textPointer2 = new TextPointer(((TextElement)child).ElementEnd);
			}
			else if (child is FrameworkContentElement)
			{
				textPointer2 = new TextPointer(textPointer);
				textPointer2.MoveByOffset(1);
			}
			if (textPointer2 == null)
			{
				return new ReadOnlyCollection<Rect>(new List<Rect>(0));
			}
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(textPointer);
			int offsetToPosition2 = this._complexContent.TextContainer.Start.GetOffsetToPosition(textPointer2);
			int num = 0;
			int num2 = 0;
			double num3 = 0.0;
			int lineCount = this.LineCount;
			while (offsetToPosition >= num2 + this.GetLine(num).Length && num < lineCount)
			{
				num2 += this.GetLine(num).Length;
				num++;
				num3 += this.GetLine(num).Height;
			}
			int num4 = num2;
			List<Rect> list = new List<Rect>();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			TextRunCache textRunCache = new TextRunCache();
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			do
			{
				LineMetrics line = this.GetLine(num);
				Line line2 = this.CreateLine(lineProperties);
				using (line2)
				{
					bool ellipsis = this.ParagraphEllipsisShownOnLine(num, (double)num2);
					this.Format(line2, line.Length, num4, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line.TextLineBreak, textRunCache, ellipsis);
					if (line.Length == line2.Length)
					{
						int num5 = (offsetToPosition >= num4) ? offsetToPosition : num4;
						int num6 = (offsetToPosition2 < num4 + line.Length) ? offsetToPosition2 : (num4 + line.Length);
						double x = vector.X;
						double yOffset = vector.Y + num3;
						List<Rect> rangeBounds = line2.GetRangeBounds(num5, num6 - num5, x, yOffset);
						list.AddRange(rangeBounds);
					}
				}
				num4 += line.Length;
				num3 += line.Height;
				num++;
			}
			while (offsetToPosition2 > num4);
			Invariant.Assert(list != null);
			return new ReadOnlyCollection<Rect>(list);
		}

		// Token: 0x17001B15 RID: 6933
		// (get) Token: 0x06007492 RID: 29842 RVA: 0x002E79E8 File Offset: 0x002E69E8
		protected virtual IEnumerator<IInputElement> HostedElementsCore
		{
			get
			{
				if (this.CheckFlags(TextBlock.Flags.ContentChangeInProgress))
				{
					throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
				}
				if (this._complexContent == null || !(this._complexContent.TextContainer is TextContainer))
				{
					return new HostedElements(new ReadOnlyCollection<TextSegment>(new List<TextSegment>(0)));
				}
				List<TextSegment> list = new List<TextSegment>(1);
				TextSegment item = new TextSegment(this._complexContent.TextContainer.Start, this._complexContent.TextContainer.End);
				list.Insert(0, item);
				return new HostedElements(new ReadOnlyCollection<TextSegment>(list));
			}
		}

		// Token: 0x06007493 RID: 29843 RVA: 0x002E7A79 File Offset: 0x002E6A79
		protected virtual void OnChildDesiredSizeChangedCore(UIElement child)
		{
			this.OnChildDesiredSizeChanged(child);
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x002E7A82 File Offset: 0x002E6A82
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TextBlockAutomationPeer(this);
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x002E7A8A File Offset: 0x002E6A8A
		internal void RemoveChild(Visual child)
		{
			if (this._complexContent != null)
			{
				this._complexContent.VisualChildren.Remove(child);
			}
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x002E7AA8 File Offset: 0x002E6AA8
		internal void SetTextContainer(ITextContainer textContainer)
		{
			if (this._complexContent != null)
			{
				this._complexContent.Detach(this);
				this._complexContent = null;
				this.SetFlags(false, TextBlock.Flags.PendingTextContainerEventInit);
			}
			if (textContainer != null)
			{
				this._complexContent = null;
				this.EnsureComplexContent(textContainer);
			}
			this.SetFlags(false, TextBlock.Flags.ContentChangeInProgress);
			base.InvalidateMeasure();
			base.InvalidateVisual();
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x002E7B04 File Offset: 0x002E6B04
		internal Size MeasureChild(InlineObject inlineObject)
		{
			Size desiredSize;
			if (this.CheckFlags(TextBlock.Flags.MeasureInProgress))
			{
				Thickness padding = this.Padding;
				Size availableSize = new Size(Math.Max(0.0, this._referenceSize.Width - (padding.Left + padding.Right)), Math.Max(0.0, this._referenceSize.Height - (padding.Top + padding.Bottom)));
				inlineObject.Element.Measure(availableSize);
				desiredSize = inlineObject.Element.DesiredSize;
				ArrayList arrayList = this.InlineObjects;
				bool flag = false;
				if (arrayList == null)
				{
					arrayList = (this.InlineObjects = new ArrayList(1));
				}
				else
				{
					for (int i = 0; i < arrayList.Count; i++)
					{
						if (((InlineObject)arrayList[i]).Dcp == inlineObject.Dcp)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					arrayList.Add(inlineObject);
				}
			}
			else
			{
				desiredSize = inlineObject.Element.DesiredSize;
			}
			return desiredSize;
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x002E7C04 File Offset: 0x002E6C04
		internal override string GetPlainText()
		{
			if (this._complexContent != null)
			{
				return TextRangeBase.GetTextInternal(this._complexContent.TextContainer.Start, this._complexContent.TextContainer.End);
			}
			if (this._contentCache != null)
			{
				return this._contentCache;
			}
			return string.Empty;
		}

		// Token: 0x06007499 RID: 29849 RVA: 0x002E7C54 File Offset: 0x002E6C54
		internal ReadOnlyCollection<LineResult> GetLineResults()
		{
			Invariant.Assert(this.IsLayoutDataValid);
			if (this.CheckFlags(TextBlock.Flags.RequiresAlignment))
			{
				this.AlignContent();
			}
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			int lineCount = this.LineCount;
			List<LineResult> list = new List<LineResult>(lineCount);
			int num = 0;
			double num2 = 0.0;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line = this.GetLine(i);
				Rect layoutBox = new Rect(vector.X + line.Start, vector.Y + num2, line.Width, line.Height);
				list.Add(new TextLineResult(this, num, line.Length, layoutBox, line.Baseline, i));
				num2 += line.Height;
				num += line.Length;
			}
			return new ReadOnlyCollection<LineResult>(list);
		}

		// Token: 0x0600749A RID: 29850 RVA: 0x002E7D44 File Offset: 0x002E6D44
		internal void GetLineDetails(int dcp, int index, double lineVOffset, out int cchContent, out int cchEllipses)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			Invariant.Assert(index >= 0 && index < this.LineCount);
			LineProperties lineProperties = this.GetLineProperties();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			TextRunCache textRunCache = new TextRunCache();
			LineMetrics line = this.GetLine(index);
			using (Line line2 = this.CreateLine(lineProperties))
			{
				TextLineBreak textLineBreak = this.GetLine(index).TextLineBreak;
				bool ellipsis = this.ParagraphEllipsisShownOnLine(index, lineVOffset);
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(dcp == 0, lineProperties), textLineBreak, textRunCache, ellipsis);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				cchContent = line2.ContentLength;
				cchEllipses = line2.GetEllipsesLength();
			}
		}

		// Token: 0x0600749B RID: 29851 RVA: 0x002E7E30 File Offset: 0x002E6E30
		internal ITextPointer GetTextPositionFromDistance(int dcp, double distance, double lineVOffset, int index)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			distance -= vector.X;
			lineVOffset -= vector.Y;
			TextRunCache textRunCache = new TextRunCache();
			LineMetrics line = this.GetLine(index);
			ITextPointer result;
			using (Line line2 = this.CreateLine(lineProperties))
			{
				Invariant.Assert(index >= 0 && index < this.LineCount);
				TextLineBreak textLineBreak = this.GetLine(index).TextLineBreak;
				bool ellipsis = this.ParagraphEllipsisShownOnLine(index, lineVOffset);
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(dcp == 0, lineProperties), textLineBreak, textRunCache, ellipsis);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				CharacterHit textPositionFromDistance = line2.GetTextPositionFromDistance(distance);
				LogicalDirection gravity = (textPositionFromDistance.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward;
				result = this._complexContent.TextContainer.Start.CreatePointer(textPositionFromDistance.FirstCharacterIndex + textPositionFromDistance.TrailingLength, gravity);
			}
			return result;
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x002E7F7C File Offset: 0x002E6F7C
		internal Rect GetRectangleFromTextPosition(ITextPointer orientedPosition)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			Invariant.Assert(orientedPosition != null);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int num = this._complexContent.TextContainer.Start.GetOffsetToPosition(orientedPosition);
			int num2 = num;
			if (orientedPosition.LogicalDirection == LogicalDirection.Backward && num > 0)
			{
				num--;
			}
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			double num3 = 0.0;
			int num4 = 0;
			TextRunCache textRunCache = new TextRunCache();
			Rect result = Rect.Empty;
			FlowDirection flowDirection = FlowDirection.LeftToRight;
			int lineCount = this.LineCount;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line = this.GetLine(i);
				if (num4 + line.Length > num || (num4 + line.Length == num && i == lineCount - 1))
				{
					using (Line line2 = this.CreateLine(lineProperties))
					{
						bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num3);
						this.Format(line2, line.Length, num4, wrappingWidth, this.GetLineProperties(num4 == 0, lineProperties), line.TextLineBreak, textRunCache, ellipsis);
						Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
						result = line2.GetBoundsFromTextPosition(num, out flowDirection);
						break;
					}
				}
				num4 += line.Length;
				num3 += line.Height;
			}
			if (!result.IsEmpty)
			{
				result.X += vector.X;
				result.Y += vector.Y + num3;
				if (lineProperties.FlowDirection != flowDirection)
				{
					if (orientedPosition.LogicalDirection == LogicalDirection.Forward || num2 == 0)
					{
						result.X = result.Right;
					}
				}
				else if (orientedPosition.LogicalDirection == LogicalDirection.Backward && num2 > 0)
				{
					result.X = result.Right;
				}
				result.Width = 0.0;
			}
			return result;
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x002E8184 File Offset: 0x002E7184
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			Invariant.Assert(startPosition != null);
			Invariant.Assert(endPosition != null);
			Invariant.Assert(startPosition.CompareTo(endPosition) <= 0);
			Geometry result = null;
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(startPosition);
			int offsetToPosition2 = this._complexContent.TextContainer.Start.GetOffsetToPosition(endPosition);
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			Vector vector = this.CalcContentOffset(base.RenderSize, wrappingWidth);
			TextRunCache textRunCache = new TextRunCache();
			Line line = this.CreateLine(lineProperties);
			int num = 0;
			ITextPointer textPointer = this._complexContent.TextContainer.Start.CreatePointer(0);
			double num2 = 0.0;
			int lineCount = this.LineCount;
			int i = 0;
			int num3 = lineCount;
			while (i < num3)
			{
				LineMetrics line2 = this.GetLine(i);
				if (offsetToPosition2 <= num)
				{
					break;
				}
				int num4 = num + line2.Length;
				textPointer.MoveByOffset(line2.Length);
				if (offsetToPosition < num4)
				{
					using (line)
					{
						bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num2);
						this.Format(line, line2.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line2.TextLineBreak, textRunCache, ellipsis);
						if (Invariant.Strict)
						{
							Invariant.Assert(this.GetLine(i).Length == line.Length, "Line length is out of sync");
						}
						int num5 = Math.Max(num, offsetToPosition);
						int num6 = Math.Min(num4, offsetToPosition2);
						if (num5 != num6)
						{
							IList<Rect> rangeBounds = line.GetRangeBounds(num5, num6 - num5, vector.X, vector.Y + num2);
							if (rangeBounds.Count > 0)
							{
								int num7 = 0;
								int count = rangeBounds.Count;
								do
								{
									Rect rect = rangeBounds[num7];
									if (num7 == count - 1 && offsetToPosition2 >= num4 && TextPointerBase.IsNextToAnyBreak(textPointer, LogicalDirection.Backward))
									{
										double num8 = this.FontSize * 0.5;
										rect.Width += num8;
									}
									RectangleGeometry addedGeometry = new RectangleGeometry(rect);
									CaretElement.AddGeometry(ref result, addedGeometry);
								}
								while (++num7 < count);
							}
						}
					}
				}
				num += line2.Length;
				num2 += line2.Height;
				i++;
			}
			return result;
		}

		// Token: 0x0600749E RID: 29854 RVA: 0x002E840C File Offset: 0x002E740C
		internal bool IsAtCaretUnitBoundary(ITextPointer position, int dcp, int lineIndex)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			TextRunCache textRunCache = new TextRunCache();
			bool result = false;
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(position);
			CharacterHit charHit = default(CharacterHit);
			if (position.LogicalDirection == LogicalDirection.Backward)
			{
				if (offsetToPosition <= dcp)
				{
					return false;
				}
				charHit = new CharacterHit(offsetToPosition - 1, 1);
			}
			else if (position.LogicalDirection == LogicalDirection.Forward)
			{
				charHit = new CharacterHit(offsetToPosition, 0);
			}
			LineMetrics line = this.GetLine(lineIndex);
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			using (Line line2 = this.CreateLine(lineProperties))
			{
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(lineIndex == 0, lineProperties), line.TextLineBreak, textRunCache, false);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				result = line2.IsAtCaretCharacterHit(charHit);
			}
			return result;
		}

		// Token: 0x0600749F RID: 29855 RVA: 0x002E8518 File Offset: 0x002E7518
		internal ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction, int dcp, int lineIndex)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(position);
			if (offsetToPosition == dcp && direction == LogicalDirection.Backward)
			{
				if (lineIndex == 0)
				{
					return position;
				}
				lineIndex--;
				dcp -= this.GetLine(lineIndex).Length;
			}
			else if (offsetToPosition == dcp + this.GetLine(lineIndex).Length && direction == LogicalDirection.Forward)
			{
				int lineCount = this.LineCount;
				if (lineIndex == lineCount - 1)
				{
					return position;
				}
				dcp += this.GetLine(lineIndex).Length;
				lineIndex++;
			}
			TextRunCache textRunCache = new TextRunCache();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			CharacterHit index = new CharacterHit(offsetToPosition, 0);
			LineMetrics line = this.GetLine(lineIndex);
			CharacterHit characterHit;
			using (Line line2 = this.CreateLine(lineProperties))
			{
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(lineIndex == 0, lineProperties), line.TextLineBreak, textRunCache, false);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				if (direction == LogicalDirection.Forward)
				{
					characterHit = line2.GetNextCaretCharacterHit(index);
				}
				else
				{
					characterHit = line2.GetPreviousCaretCharacterHit(index);
				}
			}
			LogicalDirection gravity;
			if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == dcp + this.GetLine(lineIndex).Length && direction == LogicalDirection.Forward)
			{
				if (lineIndex == this.LineCount - 1)
				{
					gravity = LogicalDirection.Backward;
				}
				else
				{
					gravity = LogicalDirection.Forward;
				}
			}
			else if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == dcp && direction == LogicalDirection.Backward)
			{
				if (dcp == 0)
				{
					gravity = LogicalDirection.Forward;
				}
				else
				{
					gravity = LogicalDirection.Backward;
				}
			}
			else
			{
				gravity = ((characterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			return this._complexContent.TextContainer.Start.CreatePointer(characterHit.FirstCharacterIndex + characterHit.TrailingLength, gravity);
		}

		// Token: 0x060074A0 RID: 29856 RVA: 0x002E8710 File Offset: 0x002E7710
		internal ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position, int dcp, int lineIndex)
		{
			Invariant.Assert(this.IsLayoutDataValid);
			LineProperties lineProperties = this.GetLineProperties();
			this.EnsureComplexContent();
			int offsetToPosition = this._complexContent.TextContainer.Start.GetOffsetToPosition(position);
			if (offsetToPosition == dcp)
			{
				if (lineIndex == 0)
				{
					return position;
				}
				lineIndex--;
				dcp -= this.GetLine(lineIndex).Length;
			}
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			CharacterHit index = new CharacterHit(offsetToPosition, 0);
			LineMetrics line = this.GetLine(lineIndex);
			TextRunCache textRunCache = new TextRunCache();
			CharacterHit backspaceCaretCharacterHit;
			using (Line line2 = this.CreateLine(lineProperties))
			{
				this.Format(line2, line.Length, dcp, wrappingWidth, this.GetLineProperties(lineIndex == 0, lineProperties), line.TextLineBreak, textRunCache, false);
				Invariant.Assert(line.Length == line2.Length, "Line length is out of sync");
				backspaceCaretCharacterHit = line2.GetBackspaceCaretCharacterHit(index);
			}
			LogicalDirection gravity;
			if (backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength == dcp)
			{
				if (dcp == 0)
				{
					gravity = LogicalDirection.Forward;
				}
				else
				{
					gravity = LogicalDirection.Backward;
				}
			}
			else
			{
				gravity = ((backspaceCaretCharacterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			return this._complexContent.TextContainer.Start.CreatePointer(backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength, gravity);
		}

		// Token: 0x17001B16 RID: 6934
		// (get) Token: 0x060074A1 RID: 29857 RVA: 0x002E8864 File Offset: 0x002E7864
		internal TextFormatter TextFormatter
		{
			get
			{
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(this);
				if (TextFormattingMode.Display == textFormattingMode)
				{
					if (this._textFormatterDisplay == null)
					{
						this._textFormatterDisplay = TextFormatter.FromCurrentDispatcher(textFormattingMode);
					}
					return this._textFormatterDisplay;
				}
				if (this._textFormatterIdeal == null)
				{
					this._textFormatterIdeal = TextFormatter.FromCurrentDispatcher(textFormattingMode);
				}
				return this._textFormatterIdeal;
			}
		}

		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x060074A2 RID: 29858 RVA: 0x002E88B1 File Offset: 0x002E78B1
		internal ITextContainer TextContainer
		{
			get
			{
				this.EnsureComplexContent();
				return this._complexContent.TextContainer;
			}
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x060074A3 RID: 29859 RVA: 0x002E88C4 File Offset: 0x002E78C4
		internal ITextView TextView
		{
			get
			{
				this.EnsureComplexContent();
				return this._complexContent.TextView;
			}
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x060074A4 RID: 29860 RVA: 0x002E88D7 File Offset: 0x002E78D7
		internal Highlights Highlights
		{
			get
			{
				this.EnsureComplexContent();
				return this._complexContent.Highlights;
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x060074A5 RID: 29861 RVA: 0x002E88EA File Offset: 0x002E78EA
		internal LineProperties ParagraphProperties
		{
			get
			{
				return this.GetLineProperties();
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x060074A6 RID: 29862 RVA: 0x002E88F4 File Offset: 0x002E78F4
		internal bool IsLayoutDataValid
		{
			get
			{
				return base.IsMeasureValid && base.IsArrangeValid && this.CheckFlags(TextBlock.Flags.HasFirstLine) && !this.CheckFlags(TextBlock.Flags.ContentChangeInProgress) && !this.CheckFlags(TextBlock.Flags.MeasureInProgress) && !this.CheckFlags(TextBlock.Flags.ArrangeInProgress);
			}
		}

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x060074A7 RID: 29863 RVA: 0x002E8941 File Offset: 0x002E7941
		internal bool HasComplexContent
		{
			get
			{
				return this._complexContent != null;
			}
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x060074A8 RID: 29864 RVA: 0x002E894C File Offset: 0x002E794C
		internal bool IsTypographyDefaultValue
		{
			get
			{
				return !this.CheckFlags(TextBlock.Flags.IsTypographySet);
			}
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x060074A9 RID: 29865 RVA: 0x002E895C File Offset: 0x002E795C
		// (set) Token: 0x060074AA RID: 29866 RVA: 0x002E8973 File Offset: 0x002E7973
		private ArrayList InlineObjects
		{
			get
			{
				if (this._complexContent != null)
				{
					return this._complexContent.InlineObjects;
				}
				return null;
			}
			set
			{
				if (this._complexContent != null)
				{
					this._complexContent.InlineObjects = value;
				}
			}
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x060074AB RID: 29867 RVA: 0x002E8989 File Offset: 0x002E7989
		// (set) Token: 0x060074AC RID: 29868 RVA: 0x002E8993 File Offset: 0x002E7993
		internal bool IsContentPresenterContainer
		{
			get
			{
				return this.CheckFlags(TextBlock.Flags.IsContentPresenterContainer);
			}
			set
			{
				this.SetFlags(value, TextBlock.Flags.IsContentPresenterContainer);
			}
		}

		// Token: 0x060074AD RID: 29869 RVA: 0x002E899E File Offset: 0x002E799E
		private static void OnTypographyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TextBlock)d).SetFlags(true, TextBlock.Flags.IsTypographySet);
		}

		// Token: 0x060074AE RID: 29870 RVA: 0x002E89B1 File Offset: 0x002E79B1
		private object OnValidateTextView(object arg)
		{
			if (this.IsLayoutDataValid && this._complexContent != null)
			{
				this._complexContent.TextView.OnUpdated();
			}
			return null;
		}

		// Token: 0x060074AF RID: 29871 RVA: 0x002E89D4 File Offset: 0x002E79D4
		private static void InsertTextRun(ITextPointer position, string text, bool whitespacesIgnorable)
		{
			if (!(position is TextPointer) || ((TextPointer)position).Parent == null || ((TextPointer)position).Parent is TextBox)
			{
				position.InsertTextInRun(text);
				return;
			}
			if (!whitespacesIgnorable || text.Trim().Length > 0)
			{
				Run run = Inline.CreateImplicitRun(((TextPointer)position).Parent);
				((TextPointer)position).InsertTextElement(run);
				run.Text = text;
			}
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x002E8A48 File Offset: 0x002E7A48
		private Line CreateLine(LineProperties lineProperties)
		{
			Line result;
			if (this._complexContent == null)
			{
				result = new SimpleLine(this, this.Text, lineProperties.DefaultTextRunProperties);
			}
			else
			{
				result = new ComplexLine(this);
			}
			return result;
		}

		// Token: 0x060074B1 RID: 29873 RVA: 0x002E8A7A File Offset: 0x002E7A7A
		private void EnsureComplexContent()
		{
			this.EnsureComplexContent(null);
		}

		// Token: 0x060074B2 RID: 29874 RVA: 0x002E8A84 File Offset: 0x002E7A84
		private void EnsureComplexContent(ITextContainer textContainer)
		{
			if (this._complexContent == null)
			{
				if (textContainer == null)
				{
					textContainer = new TextContainer(this.IsContentPresenterContainer ? null : this, false);
				}
				this._complexContent = new TextBlock.ComplexContent(this, textContainer, false, this.Text);
				this._contentCache = null;
				if (this.CheckFlags(TextBlock.Flags.FormattedOnce))
				{
					Invariant.Assert(!this.CheckFlags(TextBlock.Flags.PendingTextContainerEventInit));
					this.InitializeTextContainerListeners();
					bool flag = base.IsMeasureValid && base.IsArrangeValid;
					base.InvalidateMeasure();
					base.InvalidateVisual();
					if (flag)
					{
						base.UpdateLayout();
						return;
					}
				}
				else
				{
					this.SetFlags(true, TextBlock.Flags.PendingTextContainerEventInit);
				}
			}
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x002E8B22 File Offset: 0x002E7B22
		private void ClearComplexContent()
		{
			if (this._complexContent != null)
			{
				this._complexContent.Detach(this);
				this._complexContent = null;
				Invariant.Assert(this._contentCache == null, "Content cache should be null when complex content exists.");
			}
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x002E8B54 File Offset: 0x002E7B54
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			Invariant.Assert(args != null);
			Invariant.Assert(args.Ranges != null);
			Invariant.Assert(this.CheckFlags(TextBlock.Flags.FormattedOnce), "Unexpected Highlights.Changed callback before first format!");
			if (args.OwnerType != typeof(SpellerHighlightLayer))
			{
				return;
			}
			base.InvalidateVisual();
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x002E8BA7 File Offset: 0x002E7BA7
		private void OnTextContainerChanging(object sender, EventArgs args)
		{
			if (this.CheckFlags(TextBlock.Flags.FormattedOnce))
			{
				this.VerifyTreeIsUnlocked();
				this.SetFlags(true, TextBlock.Flags.ContentChangeInProgress);
			}
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x002E8BC4 File Offset: 0x002E7BC4
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			Invariant.Assert(args != null);
			if (this._complexContent == null)
			{
				return;
			}
			Invariant.Assert(sender == this._complexContent.TextContainer, "Received text change for foreign TextContainer.");
			if (args.Count == 0)
			{
				return;
			}
			if (this.CheckFlags(TextBlock.Flags.FormattedOnce))
			{
				this.VerifyTreeIsUnlocked();
				this.SetFlags(false, TextBlock.Flags.ContentChangeInProgress);
				base.InvalidateMeasure();
			}
			if (!this.CheckFlags(TextBlock.Flags.TextContentChanging) && args.TextChange != TextChangeType.PropertyModified)
			{
				this.SetFlags(true, TextBlock.Flags.TextContentChanging);
				try
				{
					base.SetDeferredValue(TextBlock.TextProperty, new DeferredTextReference(this.TextContainer));
				}
				finally
				{
					this.SetFlags(false, TextBlock.Flags.TextContentChanging);
				}
			}
		}

		// Token: 0x060074B7 RID: 29879 RVA: 0x002E8C7C File Offset: 0x002E7C7C
		private void EnsureTextBlockCache()
		{
			if (this._textBlockCache == null)
			{
				this._textBlockCache = new TextBlockCache();
				this._textBlockCache._lineProperties = this.GetLineProperties();
				this._textBlockCache._textRunCache = new TextRunCache();
			}
		}

		// Token: 0x060074B8 RID: 29880 RVA: 0x002E8CB4 File Offset: 0x002E7CB4
		private LineProperties GetLineProperties()
		{
			TextProperties defaultTextProperties = new TextProperties(this, this.IsTypographyDefaultValue);
			LineProperties lineProperties = new LineProperties(this, this, defaultTextProperties, null);
			if ((bool)base.GetValue(TextBlock.IsHyphenationEnabledProperty))
			{
				lineProperties.Hyphenator = this.EnsureHyphenator();
			}
			return lineProperties;
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x002E8CF7 File Offset: 0x002E7CF7
		private TextParagraphProperties GetLineProperties(bool firstLine, LineProperties lineProperties)
		{
			return this.GetLineProperties(firstLine, false, lineProperties);
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x002E8D02 File Offset: 0x002E7D02
		private TextParagraphProperties GetLineProperties(bool firstLine, bool showParagraphEllipsis, LineProperties lineProperties)
		{
			this.GetLineProperties();
			firstLine = (firstLine && lineProperties.HasFirstLineProperties);
			if (showParagraphEllipsis)
			{
				return lineProperties.GetParaEllipsisLineProps(firstLine);
			}
			if (!firstLine)
			{
				return lineProperties;
			}
			return lineProperties.FirstLineProps;
		}

		// Token: 0x060074BB RID: 29883 RVA: 0x002E8D2F File Offset: 0x002E7D2F
		private double CalcLineAdvance(double lineHeight, LineProperties lineProperties)
		{
			return lineProperties.CalcLineAdvance(lineHeight);
		}

		// Token: 0x060074BC RID: 29884 RVA: 0x002E8D38 File Offset: 0x002E7D38
		private Vector CalcContentOffset(Size computedSize, double wrappingWidth)
		{
			Vector result = default(Vector);
			Thickness padding = this.Padding;
			Size size = new Size(Math.Max(0.0, computedSize.Width - (padding.Left + padding.Right)), Math.Max(0.0, computedSize.Height - (padding.Top + padding.Bottom)));
			TextAlignment textAlignment = this.TextAlignment;
			if (textAlignment != TextAlignment.Right)
			{
				if (textAlignment == TextAlignment.Center)
				{
					result.X = (size.Width - wrappingWidth) / 2.0;
				}
			}
			else
			{
				result.X = size.Width - wrappingWidth;
			}
			result.X += padding.Left;
			result.Y += padding.Top;
			return result;
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x002E8E10 File Offset: 0x002E7E10
		private bool ParagraphEllipsisShownOnLine(int lineIndex, double lineVOffset)
		{
			if (lineIndex >= this.LineCount - 1)
			{
				return false;
			}
			if (!this.CheckFlags(TextBlock.Flags.HasParagraphEllipses))
			{
				return false;
			}
			double value = this.GetLine(lineIndex + 1).Height + this.GetLine(lineIndex).Height + lineVOffset;
			double value2 = Math.Max(0.0, base.RenderSize.Height - this.Padding.Bottom);
			return DoubleUtil.GreaterThan(value, value2) && !DoubleUtil.AreClose(value, value2);
		}

		// Token: 0x060074BE RID: 29886 RVA: 0x002E8E9C File Offset: 0x002E7E9C
		private double CalcWrappingWidth(double width)
		{
			if (width < this._previousDesiredSize.Width)
			{
				width = this._previousDesiredSize.Width;
			}
			if (width > this._referenceSize.Width)
			{
				width = this._referenceSize.Width;
			}
			bool flag = DoubleUtil.AreClose(width, this._referenceSize.Width);
			double num = this.Padding.Left + this.Padding.Right;
			width = Math.Max(0.0, width - num);
			if (!flag && width != 0.0)
			{
				if (TextOptions.GetTextFormattingMode(this) == TextFormattingMode.Display)
				{
					width += 0.5 / base.GetDpi().DpiScaleY;
				}
				if (num != 0.0)
				{
					width += 1E-11;
				}
			}
			TextDpi.EnsureValidLineWidth(ref width);
			return width;
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x002E8F78 File Offset: 0x002E7F78
		private void Format(Line line, int length, int dcp, double wrappingWidth, TextParagraphProperties paragraphProperties, TextLineBreak textLineBreak, TextRunCache textRunCache, bool ellipsis)
		{
			line.Format(dcp, wrappingWidth, paragraphProperties, textLineBreak, textRunCache, ellipsis);
			if (line.Length < length)
			{
				double num = this._referenceSize.Width;
				double num2 = wrappingWidth;
				TextDpi.EnsureValidLineWidth(ref num);
				double num3 = 0.01;
				double num4;
				for (;;)
				{
					num4 = num2 + num3;
					if (num4 > num)
					{
						goto IL_74;
					}
					line.Format(dcp, num4, paragraphProperties, textLineBreak, textRunCache, ellipsis);
					if (line.Length >= length)
					{
						break;
					}
					num2 = num4;
					num3 *= 2.0;
				}
				num = num4;
				IL_74:
				for (double num5 = (num - num2) / 2.0; num5 > 0.01; num5 /= 2.0)
				{
					double num6 = num2 + num5;
					line.Format(dcp, num6, paragraphProperties, textLineBreak, textRunCache, ellipsis);
					if (line.Length < length)
					{
						num2 = num6;
					}
					else
					{
						num = num6;
					}
				}
				line.Format(dcp, num, paragraphProperties, textLineBreak, textRunCache, ellipsis);
			}
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x002E905D File Offset: 0x002E805D
		private void VerifyTreeIsUnlocked()
		{
			if (this.CheckFlags(TextBlock.Flags.TreeInReadOnlyMode))
			{
				throw new InvalidOperationException(SR.Get("IllegalTreeChangeDetected"));
			}
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x002E9078 File Offset: 0x002E8078
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeText()
		{
			bool result = false;
			if (this._complexContent == null)
			{
				object obj = base.ReadLocalValue(TextBlock.TextProperty);
				if (obj != null && obj != DependencyProperty.UnsetValue && obj as string != string.Empty)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x002E90BB File Offset: 0x002E80BB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeInlines(XamlDesignerSerializationManager manager)
		{
			return this._complexContent != null && manager != null && manager.XmlWriter == null;
		}

		// Token: 0x060074C3 RID: 29891 RVA: 0x002E90D4 File Offset: 0x002E80D4
		private void AlignContent()
		{
			LineProperties lineProperties = this.GetLineProperties();
			double wrappingWidth = this.CalcWrappingWidth(base.RenderSize.Width);
			this.CalcContentOffset(base.RenderSize, wrappingWidth);
			Line line = this.CreateLine(lineProperties);
			TextRunCache textRunCache = new TextRunCache();
			int num = 0;
			double num2 = 0.0;
			int lineCount = this.LineCount;
			for (int i = 0; i < lineCount; i++)
			{
				LineMetrics line2 = this.GetLine(i);
				using (line)
				{
					bool ellipsis = this.ParagraphEllipsisShownOnLine(i, num2);
					this.Format(line, line2.Length, num, wrappingWidth, this.GetLineProperties(num == 0, lineProperties), line2.TextLineBreak, textRunCache, ellipsis);
					double num3 = this.CalcLineAdvance(line.Height, lineProperties);
					Invariant.Assert(line2.Length == line.Length, "Line length is out of sync");
					num += this.UpdateLine(i, line2, line.Start, line.Width).Length;
					num2 += num3;
				}
			}
			this.SetFlags(false, TextBlock.Flags.RequiresAlignment);
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x002E9200 File Offset: 0x002E8200
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			TextBlock textBlock = sender as TextBlock;
			ContentElement contentElement = args.TargetObject as ContentElement;
			if (textBlock != null && contentElement != null && TextBlock.ContainsContentElement(textBlock, contentElement))
			{
				args.Handled = true;
				ReadOnlyCollection<Rect> rectanglesCore = textBlock.GetRectanglesCore(contentElement);
				Invariant.Assert(rectanglesCore != null, "Rect collection cannot be null.");
				if (rectanglesCore.Count > 0)
				{
					textBlock.BringIntoView(rectanglesCore[0]);
					return;
				}
				textBlock.BringIntoView();
			}
		}

		// Token: 0x060074C5 RID: 29893 RVA: 0x002E926C File Offset: 0x002E826C
		private static bool ContainsContentElement(TextBlock textBlock, ContentElement element)
		{
			return textBlock._complexContent != null && textBlock._complexContent.TextContainer is TextContainer && element is TextElement && textBlock._complexContent.TextContainer == ((TextElement)element).TextContainer;
		}

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x060074C6 RID: 29894 RVA: 0x002E92BA File Offset: 0x002E82BA
		private int LineCount
		{
			get
			{
				if (!this.CheckFlags(TextBlock.Flags.HasFirstLine))
				{
					return 0;
				}
				if (this._subsequentLines != null)
				{
					return this._subsequentLines.Count + 1;
				}
				return 1;
			}
		}

		// Token: 0x060074C7 RID: 29895 RVA: 0x002E92E2 File Offset: 0x002E82E2
		private LineMetrics GetLine(int index)
		{
			if (index != 0)
			{
				return this._subsequentLines[index - 1];
			}
			return this._firstLine;
		}

		// Token: 0x060074C8 RID: 29896 RVA: 0x002E92FC File Offset: 0x002E82FC
		private LineMetrics UpdateLine(int index, LineMetrics metrics, double start, double width)
		{
			metrics = new LineMetrics(metrics, start, width);
			if (index == 0)
			{
				this._firstLine = metrics;
			}
			else
			{
				this._subsequentLines[index - 1] = metrics;
			}
			return metrics;
		}

		// Token: 0x060074C9 RID: 29897 RVA: 0x002E9325 File Offset: 0x002E8325
		private void SetFlags(bool value, TextBlock.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x060074CA RID: 29898 RVA: 0x002E9343 File Offset: 0x002E8343
		private bool CheckFlags(TextBlock.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x060074CB RID: 29899 RVA: 0x002E9350 File Offset: 0x002E8350
		private void VerifyReentrancy()
		{
			if (this.CheckFlags(TextBlock.Flags.MeasureInProgress))
			{
				throw new InvalidOperationException(SR.Get("MeasureReentrancyInvalid"));
			}
			if (this.CheckFlags(TextBlock.Flags.ArrangeInProgress))
			{
				throw new InvalidOperationException(SR.Get("ArrangeReentrancyInvalid"));
			}
			if (this.CheckFlags(TextBlock.Flags.ContentChangeInProgress))
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
		}

		// Token: 0x060074CC RID: 29900 RVA: 0x002E93B0 File Offset: 0x002E83B0
		private int GetLineIndexFromDcp(int dcpLine)
		{
			Invariant.Assert(dcpLine >= 0);
			int i = 0;
			int num = 0;
			int lineCount = this.LineCount;
			while (i < lineCount)
			{
				if (num == dcpLine)
				{
					return i;
				}
				num += this.GetLine(i).Length;
				i++;
			}
			Invariant.Assert(false, "Dcp passed is not at start of any line in TextBlock");
			return -1;
		}

		// Token: 0x060074CD RID: 29901 RVA: 0x002E9404 File Offset: 0x002E8404
		private TextPointer FindElementPosition(IInputElement e)
		{
			if (e is TextElement && (e as TextElement).TextContainer == this._complexContent.TextContainer)
			{
				return new TextPointer((e as TextElement).ElementStart);
			}
			TextPointer textPointer = new TextPointer((TextPointer)this._complexContent.TextContainer.Start);
			while (textPointer.CompareTo((TextPointer)this._complexContent.TextContainer.End) < 0)
			{
				if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement)
				{
					DependencyObject adjacentElement = textPointer.GetAdjacentElement(LogicalDirection.Forward);
					if ((adjacentElement is ContentElement || adjacentElement is UIElement) && (adjacentElement == e as ContentElement || adjacentElement == e as UIElement))
					{
						return textPointer;
					}
				}
				textPointer.MoveByOffset(1);
			}
			return null;
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x002E94BE File Offset: 0x002E84BE
		internal void OnChildBaselineOffsetChanged(DependencyObject source)
		{
			if (!this.CheckFlags(TextBlock.Flags.MeasureInProgress))
			{
				base.InvalidateMeasure();
				base.InvalidateVisual();
			}
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x002E94D8 File Offset: 0x002E84D8
		private static void OnBaselineOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextElement value = TextElement.ContainerTextElementField.GetValue(d);
			if (value != null)
			{
				DependencyObject parent = value.TextContainer.Parent;
				TextBlock textBlock = parent as TextBlock;
				if (textBlock != null)
				{
					textBlock.OnChildBaselineOffsetChanged(d);
					return;
				}
				FlowDocument flowDocument = parent as FlowDocument;
				if (flowDocument != null && d is UIElement)
				{
					flowDocument.OnChildDesiredSizeChanged((UIElement)d);
				}
			}
		}

		// Token: 0x060074D0 RID: 29904 RVA: 0x002E9530 File Offset: 0x002E8530
		private void InitializeTextContainerListeners()
		{
			this._complexContent.TextContainer.Changing += this.OnTextContainerChanging;
			this._complexContent.TextContainer.Change += this.OnTextContainerChange;
			this._complexContent.Highlights.Changed += this.OnHighlightChanged;
		}

		// Token: 0x060074D1 RID: 29905 RVA: 0x002E9594 File Offset: 0x002E8594
		private void ClearLineMetrics()
		{
			if (this.CheckFlags(TextBlock.Flags.HasFirstLine))
			{
				if (this._subsequentLines != null)
				{
					int count = this._subsequentLines.Count;
					for (int i = 0; i < count; i++)
					{
						this._subsequentLines[i].Dispose(false);
					}
					this._subsequentLines = null;
				}
				this._firstLine = this._firstLine.Dispose(true);
				this.SetFlags(false, TextBlock.Flags.HasFirstLine);
			}
		}

		// Token: 0x060074D2 RID: 29906 RVA: 0x002E960C File Offset: 0x002E860C
		private NaturalLanguageHyphenator EnsureHyphenator()
		{
			if (this.CheckFlags(TextBlock.Flags.IsHyphenatorSet))
			{
				return TextBlock.HyphenatorField.GetValue(this);
			}
			NaturalLanguageHyphenator naturalLanguageHyphenator = new NaturalLanguageHyphenator();
			TextBlock.HyphenatorField.SetValue(this, naturalLanguageHyphenator);
			this.SetFlags(true, TextBlock.Flags.IsHyphenatorSet);
			return naturalLanguageHyphenator;
		}

		// Token: 0x060074D3 RID: 29907 RVA: 0x002E9654 File Offset: 0x002E8654
		private static bool IsValidTextTrimming(object o)
		{
			TextTrimming textTrimming = (TextTrimming)o;
			return textTrimming == TextTrimming.CharacterEllipsis || textTrimming == TextTrimming.None || textTrimming == TextTrimming.WordEllipsis;
		}

		// Token: 0x060074D4 RID: 29908 RVA: 0x002E9678 File Offset: 0x002E8678
		private static bool IsValidTextWrap(object o)
		{
			TextWrapping textWrapping = (TextWrapping)o;
			return textWrapping == TextWrapping.Wrap || textWrapping == TextWrapping.NoWrap || textWrapping == TextWrapping.WrapWithOverflow;
		}

		// Token: 0x060074D5 RID: 29909 RVA: 0x002E969C File Offset: 0x002E869C
		private static object CoerceBaselineOffset(DependencyObject d, object value)
		{
			TextBlock textBlock = (TextBlock)d;
			if (DoubleUtil.IsNaN((double)value))
			{
				return textBlock._baselineOffset;
			}
			return value;
		}

		// Token: 0x060074D6 RID: 29910 RVA: 0x002E96CC File Offset: 0x002E86CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBaselineOffset()
		{
			object obj = base.ReadLocalValue(TextBlock.BaselineOffsetProperty);
			return obj != DependencyProperty.UnsetValue && !DoubleUtil.IsNaN((double)obj);
		}

		// Token: 0x060074D7 RID: 29911 RVA: 0x002E96FD File Offset: 0x002E86FD
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBlock.OnTextChanged(d, (string)e.NewValue);
		}

		// Token: 0x060074D8 RID: 29912 RVA: 0x002E9714 File Offset: 0x002E8714
		private static void OnTextChanged(DependencyObject d, string newText)
		{
			TextBlock textBlock = (TextBlock)d;
			if (textBlock.CheckFlags(TextBlock.Flags.TextContentChanging))
			{
				return;
			}
			if (textBlock._complexContent == null)
			{
				textBlock._contentCache = ((newText != null) ? newText : string.Empty);
				return;
			}
			textBlock.SetFlags(true, TextBlock.Flags.TextContentChanging);
			try
			{
				bool flag = true;
				Invariant.Assert(textBlock._contentCache == null, "Content cache should be null when complex content exists.");
				textBlock._complexContent.TextContainer.BeginChange();
				try
				{
					((TextContainer)textBlock._complexContent.TextContainer).DeleteContentInternal((TextPointer)textBlock._complexContent.TextContainer.Start, (TextPointer)textBlock._complexContent.TextContainer.End);
					TextBlock.InsertTextRun(textBlock._complexContent.TextContainer.End, newText, true);
					flag = false;
				}
				finally
				{
					textBlock._complexContent.TextContainer.EndChange();
					if (flag)
					{
						textBlock.ClearLineMetrics();
					}
				}
			}
			finally
			{
				textBlock.SetFlags(false, TextBlock.Flags.TextContentChanging);
			}
		}

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x060074D9 RID: 29913 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x04003805 RID: 14341
		public static readonly DependencyProperty BaselineOffsetProperty = DependencyProperty.RegisterAttached("BaselineOffset", typeof(double), typeof(TextBlock), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(TextBlock.OnBaselineOffsetChanged)));

		// Token: 0x04003806 RID: 14342
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBlock), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TextBlock.OnTextChanged), new CoerceValueCallback(TextBlock.CoerceText)));

		// Token: 0x04003807 RID: 14343
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(TextBlock));

		// Token: 0x04003808 RID: 14344
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(TextBlock));

		// Token: 0x04003809 RID: 14345
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(TextBlock));

		// Token: 0x0400380A RID: 14346
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(TextBlock));

		// Token: 0x0400380B RID: 14347
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(TextBlock));

		// Token: 0x0400380C RID: 14348
		[CommonDependencyProperty]
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(TextBlock));

		// Token: 0x0400380D RID: 14349
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x0400380E RID: 14350
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x0400380F RID: 14351
		public static readonly DependencyProperty TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04003810 RID: 14352
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(TextBlock));

		// Token: 0x04003811 RID: 14353
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(TextBlock));

		// Token: 0x04003812 RID: 14354
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(TextBlock), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04003813 RID: 14355
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextBlock));

		// Token: 0x04003814 RID: 14356
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(TextBlock), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(TextBlock.IsValidTextTrimming));

		// Token: 0x04003815 RID: 14357
		[CommonDependencyProperty]
		public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBlock), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), new ValidateValueCallback(TextBlock.IsValidTextWrap));

		// Token: 0x04003816 RID: 14358
		public static readonly DependencyProperty IsHyphenationEnabledProperty = Block.IsHyphenationEnabledProperty.AddOwner(typeof(TextBlock));

		// Token: 0x04003817 RID: 14359
		private TextBlockCache _textBlockCache;

		// Token: 0x04003818 RID: 14360
		private string _contentCache;

		// Token: 0x04003819 RID: 14361
		private TextBlock.ComplexContent _complexContent;

		// Token: 0x0400381A RID: 14362
		private TextFormatter _textFormatterIdeal;

		// Token: 0x0400381B RID: 14363
		private TextFormatter _textFormatterDisplay;

		// Token: 0x0400381C RID: 14364
		private Size _referenceSize;

		// Token: 0x0400381D RID: 14365
		private Size _previousDesiredSize;

		// Token: 0x0400381E RID: 14366
		private double _baselineOffset;

		// Token: 0x0400381F RID: 14367
		private static readonly UncommonField<NaturalLanguageHyphenator> HyphenatorField = new UncommonField<NaturalLanguageHyphenator>();

		// Token: 0x04003820 RID: 14368
		private LineMetrics _firstLine;

		// Token: 0x04003821 RID: 14369
		private List<LineMetrics> _subsequentLines;

		// Token: 0x04003822 RID: 14370
		private TextBlock.Flags _flags;

		// Token: 0x02000C23 RID: 3107
		[Flags]
		private enum Flags
		{
			// Token: 0x04004B38 RID: 19256
			FormattedOnce = 1,
			// Token: 0x04004B39 RID: 19257
			MeasureInProgress = 2,
			// Token: 0x04004B3A RID: 19258
			TreeInReadOnlyMode = 4,
			// Token: 0x04004B3B RID: 19259
			RequiresAlignment = 8,
			// Token: 0x04004B3C RID: 19260
			ContentChangeInProgress = 16,
			// Token: 0x04004B3D RID: 19261
			IsContentPresenterContainer = 32,
			// Token: 0x04004B3E RID: 19262
			HasParagraphEllipses = 64,
			// Token: 0x04004B3F RID: 19263
			PendingTextContainerEventInit = 128,
			// Token: 0x04004B40 RID: 19264
			ArrangeInProgress = 256,
			// Token: 0x04004B41 RID: 19265
			IsTypographySet = 512,
			// Token: 0x04004B42 RID: 19266
			TextContentChanging = 1024,
			// Token: 0x04004B43 RID: 19267
			IsHyphenatorSet = 2048,
			// Token: 0x04004B44 RID: 19268
			HasFirstLine = 4096
		}

		// Token: 0x02000C24 RID: 3108
		private class ComplexContent
		{
			// Token: 0x0600909F RID: 37023 RVA: 0x00346DAC File Offset: 0x00345DAC
			internal ComplexContent(TextBlock owner, ITextContainer textContainer, bool foreignTextContianer, string content)
			{
				this.VisualChildren = new VisualCollection(owner);
				this.TextContainer = textContainer;
				this.ForeignTextContainer = foreignTextContianer;
				if (content != null && content.Length > 0)
				{
					TextBlock.InsertTextRun(this.TextContainer.End, content, false);
				}
				this.TextView = new TextParagraphView(owner, this.TextContainer);
				this.TextContainer.TextView = this.TextView;
			}

			// Token: 0x060090A0 RID: 37024 RVA: 0x00346E20 File Offset: 0x00345E20
			internal void Detach(TextBlock owner)
			{
				this.Highlights.Changed -= owner.OnHighlightChanged;
				this.TextContainer.Changing -= owner.OnTextContainerChanging;
				this.TextContainer.Change -= owner.OnTextContainerChange;
			}

			// Token: 0x17001F9C RID: 8092
			// (get) Token: 0x060090A1 RID: 37025 RVA: 0x00346E72 File Offset: 0x00345E72
			internal Highlights Highlights
			{
				get
				{
					return this.TextContainer.Highlights;
				}
			}

			// Token: 0x04004B45 RID: 19269
			internal VisualCollection VisualChildren;

			// Token: 0x04004B46 RID: 19270
			internal readonly ITextContainer TextContainer;

			// Token: 0x04004B47 RID: 19271
			internal readonly bool ForeignTextContainer;

			// Token: 0x04004B48 RID: 19272
			internal readonly TextParagraphView TextView;

			// Token: 0x04004B49 RID: 19273
			internal ArrayList InlineObjects;
		}

		// Token: 0x02000C25 RID: 3109
		private class SimpleContentEnumerator : IEnumerator
		{
			// Token: 0x060090A2 RID: 37026 RVA: 0x00346E7F File Offset: 0x00345E7F
			internal SimpleContentEnumerator(string content)
			{
				this._content = content;
				this._initialized = false;
				this._invalidPosition = false;
			}

			// Token: 0x060090A3 RID: 37027 RVA: 0x00346E9C File Offset: 0x00345E9C
			void IEnumerator.Reset()
			{
				this._initialized = false;
				this._invalidPosition = false;
			}

			// Token: 0x060090A4 RID: 37028 RVA: 0x00346EAC File Offset: 0x00345EAC
			bool IEnumerator.MoveNext()
			{
				if (!this._initialized)
				{
					this._initialized = true;
					return true;
				}
				this._invalidPosition = true;
				return false;
			}

			// Token: 0x17001F9D RID: 8093
			// (get) Token: 0x060090A5 RID: 37029 RVA: 0x00346EC7 File Offset: 0x00345EC7
			object IEnumerator.Current
			{
				get
				{
					if (!this._initialized || this._invalidPosition)
					{
						throw new InvalidOperationException();
					}
					return this._content;
				}
			}

			// Token: 0x04004B4A RID: 19274
			private readonly string _content;

			// Token: 0x04004B4B RID: 19275
			private bool _initialized;

			// Token: 0x04004B4C RID: 19276
			private bool _invalidPosition;
		}
	}
}
