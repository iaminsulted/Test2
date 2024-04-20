using System;
using MS.Internal.Media;

namespace System.Windows.Media
{
	// Token: 0x0200042B RID: 1067
	public static class TextOptions
	{
		// Token: 0x060033E5 RID: 13285 RVA: 0x001D9B34 File Offset: 0x001D8B34
		internal static bool IsTextFormattingModeValid(object valueObject)
		{
			TextFormattingMode textFormattingMode = (TextFormattingMode)valueObject;
			return textFormattingMode == TextFormattingMode.Ideal || textFormattingMode == TextFormattingMode.Display;
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x001D9B51 File Offset: 0x001D8B51
		public static void SetTextFormattingMode(DependencyObject element, TextFormattingMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextOptions.TextFormattingModeProperty, value);
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x001D9B72 File Offset: 0x001D8B72
		public static TextFormattingMode GetTextFormattingMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextFormattingMode)element.GetValue(TextOptions.TextFormattingModeProperty);
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x001D9B92 File Offset: 0x001D8B92
		public static void SetTextRenderingMode(DependencyObject element, TextRenderingMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextOptions.TextRenderingModeProperty, value);
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x001D9BB3 File Offset: 0x001D8BB3
		public static TextRenderingMode GetTextRenderingMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextRenderingMode)element.GetValue(TextOptions.TextRenderingModeProperty);
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x001D9BD3 File Offset: 0x001D8BD3
		public static void SetTextHintingMode(DependencyObject element, TextHintingMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextOptions.TextHintingModeProperty, value);
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x001D9BF4 File Offset: 0x001D8BF4
		public static TextHintingMode GetTextHintingMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (TextHintingMode)element.GetValue(TextOptions.TextHintingModeProperty);
		}

		// Token: 0x04001C43 RID: 7235
		public static readonly DependencyProperty TextFormattingModeProperty = DependencyProperty.RegisterAttached("TextFormattingMode", typeof(TextFormattingMode), typeof(TextOptions), new FrameworkPropertyMetadata(TextFormattingMode.Ideal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(TextOptions.IsTextFormattingModeValid));

		// Token: 0x04001C44 RID: 7236
		public static readonly DependencyProperty TextRenderingModeProperty = DependencyProperty.RegisterAttached("TextRenderingMode", typeof(TextRenderingMode), typeof(TextOptions), new FrameworkPropertyMetadata(TextRenderingMode.Auto, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(ValidateEnums.IsTextRenderingModeValid));

		// Token: 0x04001C45 RID: 7237
		public static readonly DependencyProperty TextHintingModeProperty = TextOptionsInternal.TextHintingModeProperty.AddOwner(typeof(TextOptions));
	}
}
