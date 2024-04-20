using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000615 RID: 1557
	internal sealed class FixedSOMTextRun : FixedSOMElement, IComparable
	{
		// Token: 0x06004BB2 RID: 19378 RVA: 0x0023938C File Offset: 0x0023838C
		private FixedSOMTextRun(Rect boundingRect, GeneralTransform trans, FixedNode fixedNode, int startIndex, int endIndex) : base(fixedNode, startIndex, endIndex, trans)
		{
			this._boundingRect = trans.TransformBounds(boundingRect);
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x002393A8 File Offset: 0x002383A8
		int IComparable.CompareTo(object comparedObj)
		{
			FixedSOMTextRun fixedSOMTextRun = comparedObj as FixedSOMTextRun;
			int result;
			if (this._fixedBlock.IsRTL)
			{
				Rect boundingRect = base.BoundingRect;
				Rect boundingRect2 = fixedSOMTextRun.BoundingRect;
				if (!base.Matrix.IsIdentity)
				{
					Matrix mat = this._mat;
					mat.Invert();
					boundingRect.Transform(mat);
					boundingRect.Offset(this._mat.OffsetX, this._mat.OffsetY);
					boundingRect2.Transform(mat);
					boundingRect2.Offset(this._mat.OffsetX, this._mat.OffsetY);
				}
				boundingRect.Offset(this._mat.OffsetX, this._mat.OffsetY);
				boundingRect2.Offset(fixedSOMTextRun.Matrix.OffsetX, fixedSOMTextRun.Matrix.OffsetY);
				if (FixedTextBuilder.IsSameLine(boundingRect2.Top - boundingRect.Top, boundingRect.Height, boundingRect2.Height))
				{
					result = ((boundingRect.Left < boundingRect2.Left) ? 1 : -1);
				}
				else
				{
					result = ((boundingRect.Top < boundingRect2.Top) ? -1 : 1);
				}
			}
			else
			{
				List<FixedNode> markupOrder = this.FixedBlock.FixedSOMPage.MarkupOrder;
				result = markupOrder.IndexOf(base.FixedNode) - markupOrder.IndexOf(fixedSOMTextRun.FixedNode);
			}
			return result;
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x0023950C File Offset: 0x0023850C
		public static FixedSOMTextRun Create(Rect boundingRect, GeneralTransform transform, Glyphs glyphs, FixedNode fixedNode, int startIndex, int endIndex, bool allowReverseGlyphs)
		{
			if (string.IsNullOrEmpty(glyphs.UnicodeString) || glyphs.FontRenderingEmSize <= 0.0)
			{
				return null;
			}
			FixedSOMTextRun fixedSOMTextRun = new FixedSOMTextRun(boundingRect, transform, fixedNode, startIndex, endIndex);
			fixedSOMTextRun._fontUri = glyphs.FontUri;
			fixedSOMTextRun._cultureInfo = glyphs.Language.GetCompatibleCulture();
			fixedSOMTextRun._bidiLevel = glyphs.BidiLevel;
			fixedSOMTextRun._isSideways = glyphs.IsSideways;
			fixedSOMTextRun._fontSize = glyphs.FontRenderingEmSize;
			GlyphRun glyphRun = glyphs.ToGlyphRun();
			GlyphTypeface glyphTypeface = glyphRun.GlyphTypeface;
			glyphTypeface.FamilyNames.TryGetValue(fixedSOMTextRun._cultureInfo, out fixedSOMTextRun._fontFamily);
			if (fixedSOMTextRun._fontFamily == null)
			{
				glyphTypeface.FamilyNames.TryGetValue(TypeConverterHelper.InvariantEnglishUS, out fixedSOMTextRun._fontFamily);
			}
			fixedSOMTextRun._fontStyle = glyphTypeface.Style;
			fixedSOMTextRun._fontWeight = glyphTypeface.Weight;
			fixedSOMTextRun._fontStretch = glyphTypeface.Stretch;
			fixedSOMTextRun._defaultCharWidth = ((glyphTypeface.XHeight > 0.0) ? (glyphTypeface.XHeight * glyphs.FontRenderingEmSize) : glyphRun.AdvanceWidths[startIndex]);
			Transform affineTransform = transform.AffineTransform;
			if (affineTransform != null && !affineTransform.Value.IsIdentity)
			{
				Matrix value = affineTransform.Value;
				double num = Math.Sqrt(value.M12 * value.M12 + value.M22 * value.M22);
				double num2 = Math.Sqrt(value.M11 * value.M11 + value.M21 * value.M21);
				fixedSOMTextRun._fontSize *= num;
				fixedSOMTextRun._defaultCharWidth *= num2;
			}
			fixedSOMTextRun._foreground = glyphs.Fill;
			string unicodeString = glyphs.UnicodeString;
			fixedSOMTextRun.Text = unicodeString.Substring(startIndex, endIndex - startIndex);
			if (allowReverseGlyphs && fixedSOMTextRun._bidiLevel == 0 && !fixedSOMTextRun._isSideways && startIndex == 0 && endIndex == unicodeString.Length && string.IsNullOrEmpty(glyphs.CaretStops) && FixedTextBuilder.MostlyRTL(unicodeString))
			{
				char[] array = new char[fixedSOMTextRun.Text.Length];
				for (int i = 0; i < fixedSOMTextRun.Text.Length; i++)
				{
					array[i] = fixedSOMTextRun.Text[fixedSOMTextRun.Text.Length - 1 - i];
				}
				fixedSOMTextRun._isReversed = true;
				fixedSOMTextRun.Text = new string(array);
			}
			if (unicodeString == "" && glyphs.Indices != null && glyphs.Indices.Length > 0)
			{
				fixedSOMTextRun._isWhiteSpace = false;
			}
			else
			{
				fixedSOMTextRun._isWhiteSpace = true;
				for (int j = 0; j < unicodeString.Length; j++)
				{
					if (!char.IsWhiteSpace(unicodeString[j]))
					{
						fixedSOMTextRun._isWhiteSpace = false;
						break;
					}
				}
			}
			return fixedSOMTextRun;
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x002397E4 File Offset: 0x002387E4
		public bool HasSameRichProperties(FixedSOMTextRun run)
		{
			if (run.FontRenderingEmSize == this.FontRenderingEmSize && run.CultureInfo.Equals(this.CultureInfo) && run.FontStyle.Equals(this.FontStyle) && run.FontStretch.Equals(this.FontStretch) && run.FontWeight.Equals(this.FontWeight) && run.FontFamily == this.FontFamily && run.IsRTL == this.IsRTL)
			{
				SolidColorBrush solidColorBrush = this.Foreground as SolidColorBrush;
				SolidColorBrush solidColorBrush2 = run.Foreground as SolidColorBrush;
				if ((run.Foreground == null && this.Foreground == null) || (solidColorBrush != null && solidColorBrush2 != null && solidColorBrush.Color == solidColorBrush2.Color && solidColorBrush.Opacity == solidColorBrush2.Opacity))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x002398D8 File Offset: 0x002388D8
		public override void SetRTFProperties(FixedElement element)
		{
			if (this._cultureInfo != null)
			{
				element.SetValue(FrameworkElement.LanguageProperty, XmlLanguage.GetLanguage(this._cultureInfo.IetfLanguageTag));
			}
			element.SetValue(TextElement.FontSizeProperty, this._fontSize);
			element.SetValue(TextElement.FontWeightProperty, this._fontWeight);
			element.SetValue(TextElement.FontStretchProperty, this._fontStretch);
			element.SetValue(TextElement.FontStyleProperty, this._fontStyle);
			if (this.IsRTL)
			{
				element.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
			}
			else
			{
				element.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
			}
			if (this._fontFamily != null)
			{
				element.SetValue(TextElement.FontFamilyProperty, new FontFamily(this._fontFamily));
			}
			element.SetValue(TextElement.ForegroundProperty, this._foreground);
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x002399BB File Offset: 0x002389BB
		public double DefaultCharWidth
		{
			get
			{
				return this._defaultCharWidth;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x06004BB8 RID: 19384 RVA: 0x002399C3 File Offset: 0x002389C3
		public bool IsSideways
		{
			get
			{
				return this._isSideways;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x06004BB9 RID: 19385 RVA: 0x002399CB File Offset: 0x002389CB
		public bool IsWhiteSpace
		{
			get
			{
				return this._isWhiteSpace;
			}
		}

		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x06004BBA RID: 19386 RVA: 0x002399D3 File Offset: 0x002389D3
		public CultureInfo CultureInfo
		{
			get
			{
				return this._cultureInfo;
			}
		}

		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x06004BBB RID: 19387 RVA: 0x002399DB File Offset: 0x002389DB
		public bool IsLTR
		{
			get
			{
				return (this._bidiLevel & 1) == 0 && !this._isReversed;
			}
		}

		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x06004BBC RID: 19388 RVA: 0x002399F2 File Offset: 0x002389F2
		public bool IsRTL
		{
			get
			{
				return !this.IsLTR;
			}
		}

		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x06004BBD RID: 19389 RVA: 0x002399FD File Offset: 0x002389FD
		// (set) Token: 0x06004BBE RID: 19390 RVA: 0x00239A05 File Offset: 0x00238A05
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x06004BBF RID: 19391 RVA: 0x00239A0E File Offset: 0x00238A0E
		// (set) Token: 0x06004BC0 RID: 19392 RVA: 0x00239A16 File Offset: 0x00238A16
		public FixedSOMFixedBlock FixedBlock
		{
			get
			{
				return this._fixedBlock;
			}
			set
			{
				this._fixedBlock = value;
			}
		}

		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x06004BC1 RID: 19393 RVA: 0x00239A1F File Offset: 0x00238A1F
		public string FontFamily
		{
			get
			{
				return this._fontFamily;
			}
		}

		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x00239A27 File Offset: 0x00238A27
		public FontStyle FontStyle
		{
			get
			{
				return this._fontStyle;
			}
		}

		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x06004BC3 RID: 19395 RVA: 0x00239A2F File Offset: 0x00238A2F
		public FontWeight FontWeight
		{
			get
			{
				return this._fontWeight;
			}
		}

		// Token: 0x17001170 RID: 4464
		// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x00239A37 File Offset: 0x00238A37
		public FontStretch FontStretch
		{
			get
			{
				return this._fontStretch;
			}
		}

		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x06004BC5 RID: 19397 RVA: 0x00239A3F File Offset: 0x00238A3F
		public double FontRenderingEmSize
		{
			get
			{
				return this._fontSize;
			}
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x06004BC6 RID: 19398 RVA: 0x00239A47 File Offset: 0x00238A47
		public Brush Foreground
		{
			get
			{
				return this._foreground;
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x06004BC7 RID: 19399 RVA: 0x00239A4F File Offset: 0x00238A4F
		public bool IsReversed
		{
			get
			{
				return this._isReversed;
			}
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x06004BC8 RID: 19400 RVA: 0x00239A57 File Offset: 0x00238A57
		// (set) Token: 0x06004BC9 RID: 19401 RVA: 0x00239A5F File Offset: 0x00238A5F
		internal int LineIndex
		{
			get
			{
				return this._lineIndex;
			}
			set
			{
				this._lineIndex = value;
			}
		}

		// Token: 0x040027A0 RID: 10144
		private double _defaultCharWidth;

		// Token: 0x040027A1 RID: 10145
		private Uri _fontUri;

		// Token: 0x040027A2 RID: 10146
		private CultureInfo _cultureInfo;

		// Token: 0x040027A3 RID: 10147
		private bool _isSideways;

		// Token: 0x040027A4 RID: 10148
		private int _bidiLevel;

		// Token: 0x040027A5 RID: 10149
		private bool _isWhiteSpace;

		// Token: 0x040027A6 RID: 10150
		private bool _isReversed;

		// Token: 0x040027A7 RID: 10151
		private FixedSOMFixedBlock _fixedBlock;

		// Token: 0x040027A8 RID: 10152
		private int _lineIndex;

		// Token: 0x040027A9 RID: 10153
		private string _text;

		// Token: 0x040027AA RID: 10154
		private Brush _foreground;

		// Token: 0x040027AB RID: 10155
		private double _fontSize;

		// Token: 0x040027AC RID: 10156
		private string _fontFamily;

		// Token: 0x040027AD RID: 10157
		private FontStyle _fontStyle;

		// Token: 0x040027AE RID: 10158
		private FontWeight _fontWeight;

		// Token: 0x040027AF RID: 10159
		private FontStretch _fontStretch;
	}
}
