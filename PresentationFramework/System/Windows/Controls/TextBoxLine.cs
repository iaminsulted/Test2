using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal;
using MS.Internal.Text;

namespace System.Windows.Controls
{
	// Token: 0x02000712 RID: 1810
	internal class TextBoxLine : TextSource, IDisposable
	{
		// Token: 0x06005EA0 RID: 24224 RVA: 0x00290DD0 File Offset: 0x0028FDD0
		internal TextBoxLine(TextBoxView owner)
		{
			this._owner = owner;
			base.PixelsPerDip = this._owner.GetDpi().PixelsPerDip;
		}

		// Token: 0x06005EA1 RID: 24225 RVA: 0x00290E03 File Offset: 0x0028FE03
		public void Dispose()
		{
			if (this._line != null)
			{
				this._line.Dispose();
				this._line = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005EA2 RID: 24226 RVA: 0x00290E28 File Offset: 0x0028FE28
		public override TextRun GetTextRun(int dcp)
		{
			TextRun textRun = null;
			StaticTextPointer position = this._owner.Host.TextContainer.CreateStaticPointerAtOffset(dcp);
			switch (position.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				textRun = new TextEndOfParagraph(1);
				goto IL_5C;
			case TextPointerContext.Text:
				textRun = this.HandleText(position);
				goto IL_5C;
			}
			Invariant.Assert(false, "Unsupported position type.");
			IL_5C:
			Invariant.Assert(textRun != null, "TextRun has not been created.");
			Invariant.Assert(textRun.Length > 0, "TextRun has to have positive length.");
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06005EA3 RID: 24227 RVA: 0x00290ECC File Offset: 0x0028FECC
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				ITextPointer textPointer = this._owner.Host.TextContainer.CreatePointerAtOffset(dcp, LogicalDirection.Backward);
				int num = Math.Min(128, textPointer.GetTextRunLength(LogicalDirection.Backward));
				char[] array = new char[num];
				textPointer.GetTextInRun(LogicalDirection.Backward, array, 0, num);
				empty = new CharacterBufferRange(array, 0, num);
				culture = DynamicPropertyReader.GetCultureInfo((Control)this._owner.Host);
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(empty.Length, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x06005EA4 RID: 24228 RVA: 0x001136C4 File Offset: 0x001126C4
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return textSourceCharacterIndex;
		}

		// Token: 0x06005EA5 RID: 24229 RVA: 0x00290F58 File Offset: 0x0028FF58
		internal void Format(int dcp, double formatWidth, double paragraphWidth, LineProperties lineProperties, TextRunCache textRunCache, TextFormatter formatter)
		{
			this._lineProperties = lineProperties;
			this._dcp = dcp;
			this._paragraphWidth = paragraphWidth;
			lineProperties.IgnoreTextAlignment = (lineProperties.TextAlignment != TextAlignment.Justify);
			try
			{
				this._line = formatter.FormatLine(this, dcp, formatWidth, lineProperties, null, textRunCache);
			}
			finally
			{
				lineProperties.IgnoreTextAlignment = false;
			}
		}

		// Token: 0x06005EA6 RID: 24230 RVA: 0x00290FC0 File Offset: 0x0028FFC0
		internal TextBoxLineDrawingVisual CreateVisual(Geometry selectionGeometry)
		{
			TextBoxLineDrawingVisual textBoxLineDrawingVisual = new TextBoxLineDrawingVisual();
			double x = this.CalculateXOffsetShift();
			DrawingContext drawingContext = textBoxLineDrawingVisual.RenderOpen();
			if (selectionGeometry != null)
			{
				TextBoxView owner = this._owner;
				FrameworkElement frameworkElement;
				if (owner == null)
				{
					frameworkElement = null;
				}
				else
				{
					ITextBoxViewHost host = owner.Host;
					if (host == null)
					{
						frameworkElement = null;
					}
					else
					{
						ITextContainer textContainer = host.TextContainer;
						if (textContainer == null)
						{
							frameworkElement = null;
						}
						else
						{
							ITextSelection textSelection = textContainer.TextSelection;
							if (textSelection == null)
							{
								frameworkElement = null;
							}
							else
							{
								TextEditor textEditor = textSelection.TextEditor;
								frameworkElement = ((textEditor != null) ? textEditor.UiScope : null);
							}
						}
					}
				}
				FrameworkElement frameworkElement2 = frameworkElement;
				if (frameworkElement2 != null)
				{
					Brush brush = frameworkElement2.GetValue(TextBoxBase.SelectionBrushProperty) as Brush;
					if (brush != null)
					{
						double opacity = (double)frameworkElement2.GetValue(TextBoxBase.SelectionOpacityProperty);
						drawingContext.PushOpacity(opacity);
						drawingContext.DrawGeometry(brush, new Pen
						{
							Brush = brush
						}, selectionGeometry);
						drawingContext.Pop();
					}
				}
			}
			this._line.Draw(drawingContext, new Point(x, 0.0), (this._lineProperties.FlowDirection == FlowDirection.RightToLeft) ? InvertAxes.Horizontal : InvertAxes.None);
			drawingContext.Close();
			return textBoxLineDrawingVisual;
		}

		// Token: 0x06005EA7 RID: 24231 RVA: 0x002910A7 File Offset: 0x002900A7
		internal Rect GetBoundsFromTextPosition(int characterIndex, out FlowDirection flowDirection)
		{
			return this.GetBoundsFromPosition(characterIndex, 1, out flowDirection);
		}

		// Token: 0x06005EA8 RID: 24232 RVA: 0x002910B4 File Offset: 0x002900B4
		internal List<Rect> GetRangeBounds(int cp, int cch, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>();
			double num = this.CalculateXOffsetShift();
			double num2 = xOffset + num;
			IList<TextBounds> textBounds = this._line.GetTextBounds(cp, cch);
			Invariant.Assert(textBounds.Count > 0);
			for (int i = 0; i < textBounds.Count; i++)
			{
				Rect rectangle = textBounds[i].Rectangle;
				rectangle.X += num2;
				rectangle.Y += yOffset;
				list.Add(rectangle);
			}
			return list;
		}

		// Token: 0x06005EA9 RID: 24233 RVA: 0x0029113C File Offset: 0x0029013C
		internal CharacterHit GetTextPositionFromDistance(double distance)
		{
			double num = this.CalculateXOffsetShift();
			return this._line.GetCharacterHitFromDistance(distance - num);
		}

		// Token: 0x06005EAA RID: 24234 RVA: 0x0029115E File Offset: 0x0029015E
		internal CharacterHit GetNextCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetNextCaretCharacterHit(index);
		}

		// Token: 0x06005EAB RID: 24235 RVA: 0x0029116C File Offset: 0x0029016C
		internal CharacterHit GetPreviousCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetPreviousCaretCharacterHit(index);
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x0029117A File Offset: 0x0029017A
		internal CharacterHit GetBackspaceCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetBackspaceCaretCharacterHit(index);
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x00291188 File Offset: 0x00290188
		internal bool IsAtCaretCharacterHit(CharacterHit charHit)
		{
			return this._line.IsAtCaretCharacterHit(charHit, this._dcp);
		}

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x06005EAE RID: 24238 RVA: 0x0029119C File Offset: 0x0029019C
		internal double Width
		{
			get
			{
				if (this.IsWidthAdjusted)
				{
					return this._line.WidthIncludingTrailingWhitespace;
				}
				return this._line.Width;
			}
		}

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x06005EAF RID: 24239 RVA: 0x002911BD File Offset: 0x002901BD
		internal double Height
		{
			get
			{
				return this._line.Height;
			}
		}

		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x06005EB0 RID: 24240 RVA: 0x002911CA File Offset: 0x002901CA
		internal bool EndOfParagraph
		{
			get
			{
				if (this._line.NewlineLength == 0)
				{
					return false;
				}
				IList<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
				return textRunSpans[textRunSpans.Count - 1].Value is TextEndOfParagraph;
			}
		}

		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x00291200 File Offset: 0x00290200
		internal int Length
		{
			get
			{
				return this._line.Length - (this.EndOfParagraph ? 1 : 0);
			}
		}

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x06005EB2 RID: 24242 RVA: 0x0029121A File Offset: 0x0029021A
		internal int ContentLength
		{
			get
			{
				return this._line.Length - this._line.NewlineLength;
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x06005EB3 RID: 24243 RVA: 0x00291233 File Offset: 0x00290233
		internal bool HasLineBreak
		{
			get
			{
				return this._line.NewlineLength > 0;
			}
		}

		// Token: 0x06005EB4 RID: 24244 RVA: 0x00291244 File Offset: 0x00290244
		private TextRun HandleText(StaticTextPointer position)
		{
			StaticTextPointer position2 = this._owner.Host.TextContainer.Highlights.GetNextPropertyChangePosition(position, LogicalDirection.Forward);
			if (position.GetOffsetToPosition(position2) > 4096)
			{
				position2 = position.CreatePointer(4096);
			}
			Highlights highlights = position.TextContainer.Highlights;
			TextDecorationCollection textDecorationCollection = highlights.GetHighlightValue(position, LogicalDirection.Forward, typeof(SpellerHighlightLayer)) as TextDecorationCollection;
			TextRunProperties textRunProperties = this._lineProperties.DefaultTextRunProperties;
			if (textDecorationCollection != null)
			{
				if (this._spellerErrorProperties == null)
				{
					this._spellerErrorProperties = new TextProperties((TextProperties)textRunProperties, textDecorationCollection);
				}
				textRunProperties = this._spellerErrorProperties;
			}
			ITextSelection textSelection = position.TextContainer.TextSelection;
			TextEditor textEditor = (textSelection != null) ? textSelection.TextEditor : null;
			if (textEditor != null)
			{
				ITextView textView = textEditor.TextView;
				bool? flag = (textView != null) ? new bool?(textView.RendersOwnSelection) : null;
				bool flag2 = true;
				if ((flag.GetValueOrDefault() == flag2 & flag != null) && highlights.GetHighlightValue(position, LogicalDirection.Forward, typeof(TextSelection)) != DependencyProperty.UnsetValue)
				{
					TextProperties textProperties = new TextProperties((TextProperties)textRunProperties, textDecorationCollection);
					FrameworkElement frameworkElement = (textEditor != null) ? textEditor.UiScope : null;
					if (frameworkElement != null)
					{
						textProperties.SetBackgroundBrush(null);
						Brush brush = frameworkElement.GetValue(TextBoxBase.SelectionTextBrushProperty) as Brush;
						if (brush != null)
						{
							textProperties.SetForegroundBrush(brush);
						}
					}
					textRunProperties = textProperties;
				}
			}
			char[] array = new char[position.GetOffsetToPosition(position2)];
			int textInRun = position.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
			Invariant.Assert(textInRun == array.Length);
			return new TextCharacters(array, 0, textInRun, textRunProperties);
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x002913DC File Offset: 0x002903DC
		private Rect GetBoundsFromPosition(int cp, int cch, out FlowDirection flowDirection)
		{
			double num = this.CalculateXOffsetShift();
			IList<TextBounds> textBounds = this._line.GetTextBounds(cp, cch);
			Invariant.Assert(textBounds != null && textBounds.Count == 1, "Expecting exactly one TextBounds for a single text position.");
			IList<TextRunBounds> textRunBounds = textBounds[0].TextRunBounds;
			Rect rectangle;
			if (textRunBounds != null)
			{
				Invariant.Assert(textRunBounds.Count == 1, "Expecting exactly one TextRunBounds for a single text position.");
				rectangle = textRunBounds[0].Rectangle;
			}
			else
			{
				rectangle = textBounds[0].Rectangle;
			}
			rectangle.X += num;
			flowDirection = textBounds[0].FlowDirection;
			return rectangle;
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x00291474 File Offset: 0x00290474
		private double CalculateXOffsetShift()
		{
			double num = 0.0;
			if (this._lineProperties.TextAlignmentInternal == TextAlignment.Right)
			{
				num = this._paragraphWidth - this._line.Width;
			}
			else if (this._lineProperties.TextAlignmentInternal == TextAlignment.Center)
			{
				num = (this._paragraphWidth - this._line.Width) / 2.0;
			}
			if (this.IsXOffsetAdjusted)
			{
				if (this._lineProperties.TextAlignmentInternal == TextAlignment.Center)
				{
					num += (this._line.Width - this._line.WidthIncludingTrailingWhitespace) / 2.0;
				}
				else
				{
					num += this._line.Width - this._line.WidthIncludingTrailingWhitespace;
				}
			}
			return num;
		}

		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x06005EB7 RID: 24247 RVA: 0x00291530 File Offset: 0x00290530
		private bool IsXOffsetAdjusted
		{
			get
			{
				return (this._lineProperties.TextAlignmentInternal == TextAlignment.Right || this._lineProperties.TextAlignmentInternal == TextAlignment.Center) && this.IsWidthAdjusted;
			}
		}

		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x06005EB8 RID: 24248 RVA: 0x00291556 File Offset: 0x00290556
		private bool IsWidthAdjusted
		{
			get
			{
				return this.HasLineBreak || this.EndOfParagraph;
			}
		}

		// Token: 0x0400319E RID: 12702
		private readonly TextBoxView _owner;

		// Token: 0x0400319F RID: 12703
		private TextLine _line;

		// Token: 0x040031A0 RID: 12704
		private int _dcp;

		// Token: 0x040031A1 RID: 12705
		private LineProperties _lineProperties;

		// Token: 0x040031A2 RID: 12706
		private TextProperties _spellerErrorProperties;

		// Token: 0x040031A3 RID: 12707
		private double _paragraphWidth;

		// Token: 0x040031A4 RID: 12708
		private const int _syntheticCharacterLength = 1;
	}
}
