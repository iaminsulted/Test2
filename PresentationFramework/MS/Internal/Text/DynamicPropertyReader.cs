using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace MS.Internal.Text
{
	// Token: 0x0200031F RID: 799
	internal static class DynamicPropertyReader
	{
		// Token: 0x06001D9C RID: 7580 RVA: 0x0016DB80 File Offset: 0x0016CB80
		internal static Typeface GetTypeface(DependencyObject element)
		{
			FontFamily fontFamily = (FontFamily)element.GetValue(TextElement.FontFamilyProperty);
			FontStyle style = (FontStyle)element.GetValue(TextElement.FontStyleProperty);
			FontWeight weight = (FontWeight)element.GetValue(TextElement.FontWeightProperty);
			FontStretch stretch = (FontStretch)element.GetValue(TextElement.FontStretchProperty);
			return new Typeface(fontFamily, style, weight, stretch);
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x0016DBD8 File Offset: 0x0016CBD8
		internal static Typeface GetModifiedTypeface(DependencyObject element, FontFamily fontFamily)
		{
			FontStyle style = (FontStyle)element.GetValue(TextElement.FontStyleProperty);
			FontWeight weight = (FontWeight)element.GetValue(TextElement.FontWeightProperty);
			FontStretch stretch = (FontStretch)element.GetValue(TextElement.FontStretchProperty);
			return new Typeface(fontFamily, style, weight, stretch);
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0016DC24 File Offset: 0x0016CC24
		internal static TextDecorationCollection GetTextDecorationsForInlineObject(DependencyObject element, TextDecorationCollection textDecorations)
		{
			DependencyObject parent = LogicalTreeHelper.GetParent(element);
			TextDecorationCollection textDecorationCollection = null;
			if (parent != null)
			{
				textDecorationCollection = DynamicPropertyReader.GetTextDecorations(parent);
			}
			if (!((textDecorations == null) ? (textDecorationCollection == null) : textDecorations.ValueEquals(textDecorationCollection)))
			{
				if (textDecorationCollection == null)
				{
					textDecorations = null;
				}
				else
				{
					textDecorations = new TextDecorationCollection();
					int count = textDecorationCollection.Count;
					for (int i = 0; i < count; i++)
					{
						textDecorations.Add(textDecorationCollection[i]);
					}
				}
			}
			return textDecorations;
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x0016DC86 File Offset: 0x0016CC86
		internal static TextDecorationCollection GetTextDecorations(DependencyObject element)
		{
			return DynamicPropertyReader.GetCollectionValue(element, Inline.TextDecorationsProperty) as TextDecorationCollection;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x0016DC98 File Offset: 0x0016CC98
		internal static TextEffectCollection GetTextEffects(DependencyObject element)
		{
			return DynamicPropertyReader.GetCollectionValue(element, TextElement.TextEffectsProperty) as TextEffectCollection;
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x0016DCAC File Offset: 0x0016CCAC
		private static object GetCollectionValue(DependencyObject element, DependencyProperty property)
		{
			bool flag;
			if (element.GetValueSource(property, null, out flag) != BaseValueSourceInternal.Default || flag)
			{
				return element.GetValue(property);
			}
			return null;
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x0016DCD8 File Offset: 0x0016CCD8
		internal static bool GetKeepTogether(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			return paragraph != null && paragraph.KeepTogether;
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x0016DCF8 File Offset: 0x0016CCF8
		internal static bool GetKeepWithNext(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			return paragraph != null && paragraph.KeepWithNext;
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x0016DD18 File Offset: 0x0016CD18
		internal static int GetMinWidowLines(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			if (paragraph == null)
			{
				return 0;
			}
			return paragraph.MinWidowLines;
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x0016DD38 File Offset: 0x0016CD38
		internal static int GetMinOrphanLines(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			if (paragraph == null)
			{
				return 0;
			}
			return paragraph.MinOrphanLines;
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x0016DD58 File Offset: 0x0016CD58
		internal static double GetLineHeightValue(DependencyObject d)
		{
			double num = (double)d.GetValue(Block.LineHeightProperty);
			if (DoubleUtil.IsNaN(num))
			{
				FontFamily fontFamily = (FontFamily)d.GetValue(TextElement.FontFamilyProperty);
				double num2 = (double)d.GetValue(TextElement.FontSizeProperty);
				num = fontFamily.LineSpacing * num2;
			}
			return Math.Max(TextDpi.MinWidth, Math.Min(TextDpi.MaxWidth, num));
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x0016DDBC File Offset: 0x0016CDBC
		internal static Brush GetBackgroundBrush(DependencyObject element)
		{
			Brush brush = null;
			while (brush == null && DynamicPropertyReader.CanApplyBackgroundBrush(element))
			{
				brush = (Brush)element.GetValue(TextElement.BackgroundProperty);
				Invariant.Assert(element is FrameworkContentElement);
				element = ((FrameworkContentElement)element).Parent;
			}
			return brush;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x0016DE08 File Offset: 0x0016CE08
		internal static Brush GetBackgroundBrushForInlineObject(StaticTextPointer position)
		{
			Brush result;
			if (position.TextContainer.Highlights.GetHighlightValue(position, LogicalDirection.Forward, typeof(TextSelection)) == DependencyProperty.UnsetValue)
			{
				result = (Brush)position.GetValue(TextElement.BackgroundProperty);
			}
			else
			{
				result = SelectionHighlightInfo.BackgroundBrush;
			}
			return result;
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0016DE54 File Offset: 0x0016CE54
		internal static BaselineAlignment GetBaselineAlignment(DependencyObject element)
		{
			Inline inline = element as Inline;
			BaselineAlignment result = (inline != null) ? inline.BaselineAlignment : BaselineAlignment.Baseline;
			while (inline != null && DynamicPropertyReader.BaselineAlignmentIsDefault(inline))
			{
				inline = (inline.Parent as Inline);
			}
			if (inline != null)
			{
				result = inline.BaselineAlignment;
			}
			return result;
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0016DE99 File Offset: 0x0016CE99
		internal static BaselineAlignment GetBaselineAlignmentForInlineObject(DependencyObject element)
		{
			return DynamicPropertyReader.GetBaselineAlignment(LogicalTreeHelper.GetParent(element));
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x0016DEA8 File Offset: 0x0016CEA8
		internal static CultureInfo GetCultureInfo(DependencyObject element)
		{
			XmlLanguage xmlLanguage = (XmlLanguage)element.GetValue(FrameworkElement.LanguageProperty);
			CultureInfo result;
			try
			{
				result = xmlLanguage.GetSpecificCulture();
			}
			catch (InvalidOperationException)
			{
				result = TypeConverterHelper.InvariantEnglishUS;
			}
			return result;
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x0016DEEC File Offset: 0x0016CEEC
		internal static NumberSubstitution GetNumberSubstitution(DependencyObject element)
		{
			return new NumberSubstitution
			{
				CultureSource = (NumberCultureSource)element.GetValue(NumberSubstitution.CultureSourceProperty),
				CultureOverride = (CultureInfo)element.GetValue(NumberSubstitution.CultureOverrideProperty),
				Substitution = (NumberSubstitutionMethod)element.GetValue(NumberSubstitution.SubstitutionProperty)
			};
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x0016DF40 File Offset: 0x0016CF40
		private static bool CanApplyBackgroundBrush(DependencyObject element)
		{
			return element is Inline && !(element is AnchoredBlock);
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0016DF58 File Offset: 0x0016CF58
		private static bool BaselineAlignmentIsDefault(DependencyObject element)
		{
			Invariant.Assert(element != null);
			bool flag;
			return element.GetValueSource(Inline.BaselineAlignmentProperty, null, out flag) == BaseValueSourceInternal.Default && !flag;
		}
	}
}
