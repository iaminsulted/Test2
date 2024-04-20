using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x020003D3 RID: 979
	[TypeConverter(typeof(ThicknessConverter))]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public struct Thickness : IEquatable<Thickness>
	{
		// Token: 0x060028EF RID: 10479 RVA: 0x00197B24 File Offset: 0x00196B24
		public Thickness(double uniformLength)
		{
			this._Bottom = uniformLength;
			this._Right = uniformLength;
			this._Top = uniformLength;
			this._Left = uniformLength;
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x00197B53 File Offset: 0x00196B53
		public Thickness(double left, double top, double right, double bottom)
		{
			this._Left = left;
			this._Top = top;
			this._Right = right;
			this._Bottom = bottom;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x00197B74 File Offset: 0x00196B74
		public override bool Equals(object obj)
		{
			if (obj is Thickness)
			{
				Thickness t = (Thickness)obj;
				return this == t;
			}
			return false;
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x00197B9E File Offset: 0x00196B9E
		public bool Equals(Thickness thickness)
		{
			return this == thickness;
		}

		// Token: 0x060028F3 RID: 10483 RVA: 0x00197BAC File Offset: 0x00196BAC
		public override int GetHashCode()
		{
			return this._Left.GetHashCode() ^ this._Top.GetHashCode() ^ this._Right.GetHashCode() ^ this._Bottom.GetHashCode();
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x00197BDD File Offset: 0x00196BDD
		public override string ToString()
		{
			return ThicknessConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x00197BEF File Offset: 0x00196BEF
		internal string ToString(CultureInfo cultureInfo)
		{
			return ThicknessConverter.ToString(this, cultureInfo);
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x00197BFD File Offset: 0x00196BFD
		internal bool IsZero
		{
			get
			{
				return DoubleUtil.IsZero(this.Left) && DoubleUtil.IsZero(this.Top) && DoubleUtil.IsZero(this.Right) && DoubleUtil.IsZero(this.Bottom);
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x00197C33 File Offset: 0x00196C33
		internal bool IsUniform
		{
			get
			{
				return DoubleUtil.AreClose(this.Left, this.Top) && DoubleUtil.AreClose(this.Left, this.Right) && DoubleUtil.AreClose(this.Left, this.Bottom);
			}
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x00197C70 File Offset: 0x00196C70
		internal bool IsValid(bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
		{
			return (allowNegative || (this.Left >= 0.0 && this.Right >= 0.0 && this.Top >= 0.0 && this.Bottom >= 0.0)) && (allowNaN || (!DoubleUtil.IsNaN(this.Left) && !DoubleUtil.IsNaN(this.Right) && !DoubleUtil.IsNaN(this.Top) && !DoubleUtil.IsNaN(this.Bottom))) && (allowPositiveInfinity || (!double.IsPositiveInfinity(this.Left) && !double.IsPositiveInfinity(this.Right) && !double.IsPositiveInfinity(this.Top) && !double.IsPositiveInfinity(this.Bottom))) && (allowNegativeInfinity || (!double.IsNegativeInfinity(this.Left) && !double.IsNegativeInfinity(this.Right) && !double.IsNegativeInfinity(this.Top) && !double.IsNegativeInfinity(this.Bottom)));
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x00197D74 File Offset: 0x00196D74
		internal bool IsClose(Thickness thickness)
		{
			return DoubleUtil.AreClose(this.Left, thickness.Left) && DoubleUtil.AreClose(this.Top, thickness.Top) && DoubleUtil.AreClose(this.Right, thickness.Right) && DoubleUtil.AreClose(this.Bottom, thickness.Bottom);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x00197DD1 File Offset: 0x00196DD1
		internal static bool AreClose(Thickness thickness0, Thickness thickness1)
		{
			return thickness0.IsClose(thickness1);
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x00197DDC File Offset: 0x00196DDC
		public static bool operator ==(Thickness t1, Thickness t2)
		{
			return (t1._Left == t2._Left || (DoubleUtil.IsNaN(t1._Left) && DoubleUtil.IsNaN(t2._Left))) && (t1._Top == t2._Top || (DoubleUtil.IsNaN(t1._Top) && DoubleUtil.IsNaN(t2._Top))) && (t1._Right == t2._Right || (DoubleUtil.IsNaN(t1._Right) && DoubleUtil.IsNaN(t2._Right))) && (t1._Bottom == t2._Bottom || (DoubleUtil.IsNaN(t1._Bottom) && DoubleUtil.IsNaN(t2._Bottom)));
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x00197E90 File Offset: 0x00196E90
		public static bool operator !=(Thickness t1, Thickness t2)
		{
			return !(t1 == t2);
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x00197E9C File Offset: 0x00196E9C
		// (set) Token: 0x060028FE RID: 10494 RVA: 0x00197EA4 File Offset: 0x00196EA4
		public double Left
		{
			get
			{
				return this._Left;
			}
			set
			{
				this._Left = value;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060028FF RID: 10495 RVA: 0x00197EAD File Offset: 0x00196EAD
		// (set) Token: 0x06002900 RID: 10496 RVA: 0x00197EB5 File Offset: 0x00196EB5
		public double Top
		{
			get
			{
				return this._Top;
			}
			set
			{
				this._Top = value;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06002901 RID: 10497 RVA: 0x00197EBE File Offset: 0x00196EBE
		// (set) Token: 0x06002902 RID: 10498 RVA: 0x00197EC6 File Offset: 0x00196EC6
		public double Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002903 RID: 10499 RVA: 0x00197ECF File Offset: 0x00196ECF
		// (set) Token: 0x06002904 RID: 10500 RVA: 0x00197ED7 File Offset: 0x00196ED7
		public double Bottom
		{
			get
			{
				return this._Bottom;
			}
			set
			{
				this._Bottom = value;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06002905 RID: 10501 RVA: 0x00197EE0 File Offset: 0x00196EE0
		internal Size Size
		{
			get
			{
				return new Size(this._Left + this._Right, this._Top + this._Bottom);
			}
		}

		// Token: 0x040014D0 RID: 5328
		private double _Left;

		// Token: 0x040014D1 RID: 5329
		private double _Top;

		// Token: 0x040014D2 RID: 5330
		private double _Right;

		// Token: 0x040014D3 RID: 5331
		private double _Bottom;
	}
}
