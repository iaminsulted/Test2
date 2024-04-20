using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000322 RID: 802
	internal struct LineMetrics
	{
		// Token: 0x06001DDB RID: 7643 RVA: 0x0016E800 File Offset: 0x0016D800
		internal LineMetrics(int length, double width, double height, double baseline, bool hasInlineObjects, TextLineBreak textLineBreak)
		{
			this._start = 0.0;
			this._width = width;
			this._height = height;
			this._baseline = baseline;
			this._textLineBreak = textLineBreak;
			this._packedData = (uint)((length & (int)LineMetrics.LengthMask) | (int)(hasInlineObjects ? LineMetrics.HasInlineObjectsMask : 0U));
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0016E854 File Offset: 0x0016D854
		internal LineMetrics(LineMetrics source, double start, double width)
		{
			this._start = start;
			this._width = width;
			this._height = source.Height;
			this._baseline = source.Baseline;
			this._textLineBreak = source.TextLineBreak;
			this._packedData = (source._packedData | LineMetrics.HasBeenUpdatedMask);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0016E8A8 File Offset: 0x0016D8A8
		internal LineMetrics Dispose(bool returnUpdatedMetrics)
		{
			if (this._textLineBreak != null)
			{
				this._textLineBreak.Dispose();
				if (returnUpdatedMetrics)
				{
					return new LineMetrics(this.Length, this._width, this._height, this._baseline, this.HasInlineObjects, null);
				}
			}
			return this;
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0016E8F6 File Offset: 0x0016D8F6
		internal int Length
		{
			get
			{
				return (int)(this._packedData & LineMetrics.LengthMask);
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x0016E904 File Offset: 0x0016D904
		internal double Width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0016E90C File Offset: 0x0016D90C
		internal double Height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x0016E914 File Offset: 0x0016D914
		internal double Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0016E91C File Offset: 0x0016D91C
		internal double Baseline
		{
			get
			{
				return this._baseline;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x0016E924 File Offset: 0x0016D924
		internal bool HasInlineObjects
		{
			get
			{
				return (this._packedData & LineMetrics.HasInlineObjectsMask) > 0U;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x0016E935 File Offset: 0x0016D935
		internal TextLineBreak TextLineBreak
		{
			get
			{
				return this._textLineBreak;
			}
		}

		// Token: 0x04000ECB RID: 3787
		private uint _packedData;

		// Token: 0x04000ECC RID: 3788
		private double _width;

		// Token: 0x04000ECD RID: 3789
		private double _height;

		// Token: 0x04000ECE RID: 3790
		private double _start;

		// Token: 0x04000ECF RID: 3791
		private double _baseline;

		// Token: 0x04000ED0 RID: 3792
		private TextLineBreak _textLineBreak;

		// Token: 0x04000ED1 RID: 3793
		private static readonly uint HasBeenUpdatedMask = 1073741824U;

		// Token: 0x04000ED2 RID: 3794
		private static readonly uint LengthMask = 1073741823U;

		// Token: 0x04000ED3 RID: 3795
		private static readonly uint HasInlineObjectsMask = 2147483648U;
	}
}
