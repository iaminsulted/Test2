using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x0200034D RID: 845
	[TypeConverter(typeof(CornerRadiusConverter))]
	public struct CornerRadius : IEquatable<CornerRadius>
	{
		// Token: 0x06002011 RID: 8209 RVA: 0x00174458 File Offset: 0x00173458
		public CornerRadius(double uniformRadius)
		{
			this._bottomRight = uniformRadius;
			this._bottomLeft = uniformRadius;
			this._topRight = uniformRadius;
			this._topLeft = uniformRadius;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x00174487 File Offset: 0x00173487
		public CornerRadius(double topLeft, double topRight, double bottomRight, double bottomLeft)
		{
			this._topLeft = topLeft;
			this._topRight = topRight;
			this._bottomRight = bottomRight;
			this._bottomLeft = bottomLeft;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x001744A8 File Offset: 0x001734A8
		public override bool Equals(object obj)
		{
			if (obj is CornerRadius)
			{
				CornerRadius cr = (CornerRadius)obj;
				return this == cr;
			}
			return false;
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x001744D2 File Offset: 0x001734D2
		public bool Equals(CornerRadius cornerRadius)
		{
			return this == cornerRadius;
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x001744E0 File Offset: 0x001734E0
		public override int GetHashCode()
		{
			return this._topLeft.GetHashCode() ^ this._topRight.GetHashCode() ^ this._bottomLeft.GetHashCode() ^ this._bottomRight.GetHashCode();
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x00174511 File Offset: 0x00173511
		public override string ToString()
		{
			return CornerRadiusConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x00174524 File Offset: 0x00173524
		public static bool operator ==(CornerRadius cr1, CornerRadius cr2)
		{
			return (cr1._topLeft == cr2._topLeft || (DoubleUtil.IsNaN(cr1._topLeft) && DoubleUtil.IsNaN(cr2._topLeft))) && (cr1._topRight == cr2._topRight || (DoubleUtil.IsNaN(cr1._topRight) && DoubleUtil.IsNaN(cr2._topRight))) && (cr1._bottomRight == cr2._bottomRight || (DoubleUtil.IsNaN(cr1._bottomRight) && DoubleUtil.IsNaN(cr2._bottomRight))) && (cr1._bottomLeft == cr2._bottomLeft || (DoubleUtil.IsNaN(cr1._bottomLeft) && DoubleUtil.IsNaN(cr2._bottomLeft)));
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x001745D8 File Offset: 0x001735D8
		public static bool operator !=(CornerRadius cr1, CornerRadius cr2)
		{
			return !(cr1 == cr2);
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x001745E4 File Offset: 0x001735E4
		// (set) Token: 0x0600201A RID: 8218 RVA: 0x001745EC File Offset: 0x001735EC
		public double TopLeft
		{
			get
			{
				return this._topLeft;
			}
			set
			{
				this._topLeft = value;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600201B RID: 8219 RVA: 0x001745F5 File Offset: 0x001735F5
		// (set) Token: 0x0600201C RID: 8220 RVA: 0x001745FD File Offset: 0x001735FD
		public double TopRight
		{
			get
			{
				return this._topRight;
			}
			set
			{
				this._topRight = value;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x00174606 File Offset: 0x00173606
		// (set) Token: 0x0600201E RID: 8222 RVA: 0x0017460E File Offset: 0x0017360E
		public double BottomRight
		{
			get
			{
				return this._bottomRight;
			}
			set
			{
				this._bottomRight = value;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x00174617 File Offset: 0x00173617
		// (set) Token: 0x06002020 RID: 8224 RVA: 0x0017461F File Offset: 0x0017361F
		public double BottomLeft
		{
			get
			{
				return this._bottomLeft;
			}
			set
			{
				this._bottomLeft = value;
			}
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x00174628 File Offset: 0x00173628
		internal bool IsValid(bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
		{
			return (allowNegative || (this._topLeft >= 0.0 && this._topRight >= 0.0 && this._bottomLeft >= 0.0 && this._bottomRight >= 0.0)) && (allowNaN || (!DoubleUtil.IsNaN(this._topLeft) && !DoubleUtil.IsNaN(this._topRight) && !DoubleUtil.IsNaN(this._bottomLeft) && !DoubleUtil.IsNaN(this._bottomRight))) && (allowPositiveInfinity || (!double.IsPositiveInfinity(this._topLeft) && !double.IsPositiveInfinity(this._topRight) && !double.IsPositiveInfinity(this._bottomLeft) && !double.IsPositiveInfinity(this._bottomRight))) && (allowNegativeInfinity || (!double.IsNegativeInfinity(this._topLeft) && !double.IsNegativeInfinity(this._topRight) && !double.IsNegativeInfinity(this._bottomLeft) && !double.IsNegativeInfinity(this._bottomRight)));
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002022 RID: 8226 RVA: 0x0017472B File Offset: 0x0017372B
		internal bool IsZero
		{
			get
			{
				return DoubleUtil.IsZero(this._topLeft) && DoubleUtil.IsZero(this._topRight) && DoubleUtil.IsZero(this._bottomRight) && DoubleUtil.IsZero(this._bottomLeft);
			}
		}

		// Token: 0x04000FBA RID: 4026
		private double _topLeft;

		// Token: 0x04000FBB RID: 4027
		private double _topRight;

		// Token: 0x04000FBC RID: 4028
		private double _bottomLeft;

		// Token: 0x04000FBD RID: 4029
		private double _bottomRight;
	}
}
