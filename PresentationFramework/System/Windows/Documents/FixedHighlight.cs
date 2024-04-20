using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace System.Windows.Documents
{
	// Token: 0x020005F7 RID: 1527
	internal sealed class FixedHighlight
	{
		// Token: 0x06004A90 RID: 19088 RVA: 0x0023406F File Offset: 0x0023306F
		internal FixedHighlight(UIElement element, int beginOffset, int endOffset, FixedHighlightType t, Brush foreground, Brush background)
		{
			this._element = element;
			this._gBeginOffset = beginOffset;
			this._gEndOffset = endOffset;
			this._type = t;
			this._foregroundBrush = foreground;
			this._backgroundBrush = background;
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x002340A4 File Offset: 0x002330A4
		public override bool Equals(object oCompare)
		{
			FixedHighlight fixedHighlight = oCompare as FixedHighlight;
			return fixedHighlight != null && (fixedHighlight._element == this._element && fixedHighlight._gBeginOffset == this._gBeginOffset && fixedHighlight._gEndOffset == this._gEndOffset) && fixedHighlight._type == this._type;
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x002340F7 File Offset: 0x002330F7
		public override int GetHashCode()
		{
			if (this._element != null)
			{
				return (int)(this._element.GetHashCode() + this._gBeginOffset + this._gEndOffset + this._type);
			}
			return 0;
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x00234124 File Offset: 0x00233124
		internal Rect ComputeDesignRect()
		{
			Glyphs glyphs = this._element as Glyphs;
			if (glyphs == null)
			{
				Image image = this._element as Image;
				if (image != null && image.Source != null)
				{
					return new Rect(0.0, 0.0, image.Width, image.Height);
				}
				Path path = this._element as Path;
				if (path != null)
				{
					return path.Data.Bounds;
				}
				return Rect.Empty;
			}
			else
			{
				GlyphRun measurementGlyphRun = glyphs.MeasurementGlyphRun;
				if (measurementGlyphRun == null || this._gBeginOffset >= this._gEndOffset)
				{
					return Rect.Empty;
				}
				Rect result = measurementGlyphRun.ComputeAlignmentBox();
				result.Offset(glyphs.OriginX, glyphs.OriginY);
				int num = (measurementGlyphRun.Characters == null) ? 0 : measurementGlyphRun.Characters.Count;
				double num2 = measurementGlyphRun.GetDistanceFromCaretCharacterHit(new CharacterHit(this._gBeginOffset, 0));
				double num3;
				if (this._gEndOffset == num)
				{
					num3 = measurementGlyphRun.GetDistanceFromCaretCharacterHit(new CharacterHit(num - 1, 1));
				}
				else
				{
					num3 = measurementGlyphRun.GetDistanceFromCaretCharacterHit(new CharacterHit(this._gEndOffset, 0));
				}
				if (num3 < num2)
				{
					double num4 = num2;
					num2 = num3;
					num3 = num4;
				}
				double width = num3 - num2;
				if ((measurementGlyphRun.BidiLevel & 1) != 0)
				{
					result.X = glyphs.OriginX - num3;
				}
				else
				{
					result.X = glyphs.OriginX + num2;
				}
				result.Width = width;
				return result;
			}
		}

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06004A94 RID: 19092 RVA: 0x00234282 File Offset: 0x00233282
		internal FixedHighlightType HighlightType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06004A95 RID: 19093 RVA: 0x0023428A File Offset: 0x0023328A
		internal Glyphs Glyphs
		{
			get
			{
				return this._element as Glyphs;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06004A96 RID: 19094 RVA: 0x00234297 File Offset: 0x00233297
		internal UIElement Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x06004A97 RID: 19095 RVA: 0x0023429F File Offset: 0x0023329F
		internal Brush ForegroundBrush
		{
			get
			{
				return this._foregroundBrush;
			}
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x06004A98 RID: 19096 RVA: 0x002342A7 File Offset: 0x002332A7
		internal Brush BackgroundBrush
		{
			get
			{
				return this._backgroundBrush;
			}
		}

		// Token: 0x0400272A RID: 10026
		private readonly UIElement _element;

		// Token: 0x0400272B RID: 10027
		private readonly int _gBeginOffset;

		// Token: 0x0400272C RID: 10028
		private readonly int _gEndOffset;

		// Token: 0x0400272D RID: 10029
		private readonly FixedHighlightType _type;

		// Token: 0x0400272E RID: 10030
		private readonly Brush _backgroundBrush;

		// Token: 0x0400272F RID: 10031
		private readonly Brush _foregroundBrush;
	}
}
