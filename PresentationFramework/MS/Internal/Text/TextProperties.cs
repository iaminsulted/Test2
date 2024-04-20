using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000328 RID: 808
	internal sealed class TextProperties : TextRunProperties
	{
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001E22 RID: 7714 RVA: 0x0016F3A7 File Offset: 0x0016E3A7
		public override Typeface Typeface
		{
			get
			{
				return this._typeface;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x0016F3B0 File Offset: 0x0016E3B0
		public override double FontRenderingEmSize
		{
			get
			{
				double fontSize = this._fontSize;
				TextDpi.EnsureValidLineOffset(ref fontSize);
				return fontSize;
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001E24 RID: 7716 RVA: 0x0016F3CC File Offset: 0x0016E3CC
		public override double FontHintingEmSize
		{
			get
			{
				return 12.0;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x0016F3D7 File Offset: 0x0016E3D7
		public override TextDecorationCollection TextDecorations
		{
			get
			{
				return this._textDecorations;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x0016F3DF File Offset: 0x0016E3DF
		public override Brush ForegroundBrush
		{
			get
			{
				return this._foreground;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x0016F3E7 File Offset: 0x0016E3E7
		public override Brush BackgroundBrush
		{
			get
			{
				return this._backgroundBrush;
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06001E28 RID: 7720 RVA: 0x0016F3EF File Offset: 0x0016E3EF
		public override BaselineAlignment BaselineAlignment
		{
			get
			{
				return this._baselineAlignment;
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x0016F3F7 File Offset: 0x0016E3F7
		public override CultureInfo CultureInfo
		{
			get
			{
				return this._cultureInfo;
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001E2A RID: 7722 RVA: 0x0016F3FF File Offset: 0x0016E3FF
		public override NumberSubstitution NumberSubstitution
		{
			get
			{
				return this._numberSubstitution;
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x0016F407 File Offset: 0x0016E407
		public override TextRunTypographyProperties TypographyProperties
		{
			get
			{
				return this._typographyProperties;
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001E2C RID: 7724 RVA: 0x0016F40F File Offset: 0x0016E40F
		public override TextEffectCollection TextEffects
		{
			get
			{
				return this._textEffects;
			}
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0016F418 File Offset: 0x0016E418
		internal TextProperties(FrameworkElement target, bool isTypographyDefaultValue)
		{
			if (!target.HasNumberSubstitutionChanged)
			{
				this._numberSubstitution = FrameworkElement.DefaultNumberSubstitution;
			}
			base.PixelsPerDip = target.GetDpi().PixelsPerDip;
			this.InitCommon(target);
			if (!isTypographyDefaultValue)
			{
				this._typographyProperties = TextElement.GetTypographyProperties(target);
			}
			else
			{
				this._typographyProperties = Typography.Default;
			}
			this._baselineAlignment = BaselineAlignment.Baseline;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x0016F47C File Offset: 0x0016E47C
		internal TextProperties(DependencyObject target, StaticTextPointer position, bool inlineObjects, bool getBackground, double pixelsPerDip)
		{
			FrameworkContentElement frameworkContentElement = target as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				if (!frameworkContentElement.HasNumberSubstitutionChanged)
				{
					this._numberSubstitution = FrameworkContentElement.DefaultNumberSubstitution;
				}
			}
			else
			{
				FrameworkElement frameworkElement = target as FrameworkElement;
				if (frameworkElement != null && !frameworkElement.HasNumberSubstitutionChanged)
				{
					this._numberSubstitution = FrameworkElement.DefaultNumberSubstitution;
				}
			}
			base.PixelsPerDip = pixelsPerDip;
			this.InitCommon(target);
			this._typographyProperties = TextProperties.GetTypographyProperties(target);
			if (!inlineObjects)
			{
				this._baselineAlignment = DynamicPropertyReader.GetBaselineAlignment(target);
				if (!position.IsNull)
				{
					TextDecorationCollection highlightTextDecorations = TextProperties.GetHighlightTextDecorations(position);
					if (highlightTextDecorations != null)
					{
						this._textDecorations = highlightTextDecorations;
					}
				}
				if (getBackground)
				{
					this._backgroundBrush = DynamicPropertyReader.GetBackgroundBrush(target);
					return;
				}
			}
			else
			{
				this._baselineAlignment = DynamicPropertyReader.GetBaselineAlignmentForInlineObject(target);
				this._textDecorations = DynamicPropertyReader.GetTextDecorationsForInlineObject(target, this._textDecorations);
				if (getBackground)
				{
					this._backgroundBrush = DynamicPropertyReader.GetBackgroundBrushForInlineObject(position);
				}
			}
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0016F550 File Offset: 0x0016E550
		internal TextProperties(TextProperties source, TextDecorationCollection textDecorations)
		{
			this._backgroundBrush = source._backgroundBrush;
			this._typeface = source._typeface;
			this._fontSize = source._fontSize;
			this._foreground = source._foreground;
			this._textEffects = source._textEffects;
			this._cultureInfo = source._cultureInfo;
			this._numberSubstitution = source._numberSubstitution;
			this._typographyProperties = source._typographyProperties;
			this._baselineAlignment = source._baselineAlignment;
			base.PixelsPerDip = source.PixelsPerDip;
			this._textDecorations = textDecorations;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0016F5E4 File Offset: 0x0016E5E4
		private void InitCommon(DependencyObject target)
		{
			this._typeface = DynamicPropertyReader.GetTypeface(target);
			this._fontSize = (double)target.GetValue(TextElement.FontSizeProperty);
			this._foreground = (Brush)target.GetValue(TextElement.ForegroundProperty);
			this._textEffects = DynamicPropertyReader.GetTextEffects(target);
			this._cultureInfo = DynamicPropertyReader.GetCultureInfo(target);
			this._textDecorations = DynamicPropertyReader.GetTextDecorations(target);
			if (this._numberSubstitution == null)
			{
				this._numberSubstitution = DynamicPropertyReader.GetNumberSubstitution(target);
			}
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0016F664 File Offset: 0x0016E664
		private static TextDecorationCollection GetHighlightTextDecorations(StaticTextPointer highlightPosition)
		{
			TextDecorationCollection result = null;
			Highlights highlights = highlightPosition.TextContainer.Highlights;
			if (highlights == null)
			{
				return result;
			}
			return highlights.GetHighlightValue(highlightPosition, LogicalDirection.Forward, typeof(SpellerHighlightLayer)) as TextDecorationCollection;
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0016F6A0 File Offset: 0x0016E6A0
		private static TypographyProperties GetTypographyProperties(DependencyObject element)
		{
			TextBlock textBlock = element as TextBlock;
			if (textBlock != null)
			{
				if (!textBlock.IsTypographyDefaultValue)
				{
					return TextElement.GetTypographyProperties(element);
				}
				return Typography.Default;
			}
			else
			{
				TextBox textBox = element as TextBox;
				if (textBox != null)
				{
					if (!textBox.IsTypographyDefaultValue)
					{
						return TextElement.GetTypographyProperties(element);
					}
					return Typography.Default;
				}
				else
				{
					TextElement textElement = element as TextElement;
					if (textElement != null)
					{
						return textElement.TypographyPropertiesGroup;
					}
					FlowDocument flowDocument = element as FlowDocument;
					if (flowDocument != null)
					{
						return flowDocument.TypographyPropertiesGroup;
					}
					return Typography.Default;
				}
			}
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x0016F712 File Offset: 0x0016E712
		internal void SetBackgroundBrush(Brush backgroundBrush)
		{
			this._backgroundBrush = backgroundBrush;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0016F71B File Offset: 0x0016E71B
		internal void SetForegroundBrush(Brush foregroundBrush)
		{
			this._foreground = foregroundBrush;
		}

		// Token: 0x04000EF4 RID: 3828
		private Typeface _typeface;

		// Token: 0x04000EF5 RID: 3829
		private double _fontSize;

		// Token: 0x04000EF6 RID: 3830
		private Brush _foreground;

		// Token: 0x04000EF7 RID: 3831
		private TextEffectCollection _textEffects;

		// Token: 0x04000EF8 RID: 3832
		private TextDecorationCollection _textDecorations;

		// Token: 0x04000EF9 RID: 3833
		private BaselineAlignment _baselineAlignment;

		// Token: 0x04000EFA RID: 3834
		private Brush _backgroundBrush;

		// Token: 0x04000EFB RID: 3835
		private CultureInfo _cultureInfo;

		// Token: 0x04000EFC RID: 3836
		private NumberSubstitution _numberSubstitution;

		// Token: 0x04000EFD RID: 3837
		private TextRunTypographyProperties _typographyProperties;
	}
}
