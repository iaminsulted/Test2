using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x02000374 RID: 884
	[TypeConverter(typeof(GridLengthConverter))]
	public struct GridLength : IEquatable<GridLength>
	{
		// Token: 0x060023D4 RID: 9172 RVA: 0x001810A2 File Offset: 0x001800A2
		public GridLength(double pixels)
		{
			this = new GridLength(pixels, GridUnitType.Pixel);
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x001810AC File Offset: 0x001800AC
		public GridLength(double value, GridUnitType type)
		{
			if (DoubleUtil.IsNaN(value))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoNaN", new object[]
				{
					"value"
				}));
			}
			if (double.IsInfinity(value))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoInfinity", new object[]
				{
					"value"
				}));
			}
			if (type != GridUnitType.Auto && type != GridUnitType.Pixel && type != GridUnitType.Star)
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterUnknownGridUnitType", new object[]
				{
					"type"
				}));
			}
			this._unitValue = ((type == GridUnitType.Auto) ? 0.0 : value);
			this._unitType = type;
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0018114A File Offset: 0x0018014A
		public static bool operator ==(GridLength gl1, GridLength gl2)
		{
			return gl1.GridUnitType == gl2.GridUnitType && gl1.Value == gl2.Value;
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x0018116E File Offset: 0x0018016E
		public static bool operator !=(GridLength gl1, GridLength gl2)
		{
			return gl1.GridUnitType != gl2.GridUnitType || gl1.Value != gl2.Value;
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x00181198 File Offset: 0x00180198
		public override bool Equals(object oCompare)
		{
			if (oCompare is GridLength)
			{
				GridLength gl = (GridLength)oCompare;
				return this == gl;
			}
			return false;
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x001811C2 File Offset: 0x001801C2
		public bool Equals(GridLength gridLength)
		{
			return this == gridLength;
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x001811D0 File Offset: 0x001801D0
		public override int GetHashCode()
		{
			return (int)((int)this._unitValue + this._unitType);
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x060023DB RID: 9179 RVA: 0x001811E0 File Offset: 0x001801E0
		public bool IsAbsolute
		{
			get
			{
				return this._unitType == GridUnitType.Pixel;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x001811EB File Offset: 0x001801EB
		public bool IsAuto
		{
			get
			{
				return this._unitType == GridUnitType.Auto;
			}
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x060023DD RID: 9181 RVA: 0x001811F6 File Offset: 0x001801F6
		public bool IsStar
		{
			get
			{
				return this._unitType == GridUnitType.Star;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x00181201 File Offset: 0x00180201
		public double Value
		{
			get
			{
				if (this._unitType != GridUnitType.Auto)
				{
					return this._unitValue;
				}
				return 1.0;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060023DF RID: 9183 RVA: 0x0018121B File Offset: 0x0018021B
		public GridUnitType GridUnitType
		{
			get
			{
				return this._unitType;
			}
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x00181223 File Offset: 0x00180223
		public override string ToString()
		{
			return GridLengthConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060023E1 RID: 9185 RVA: 0x00181235 File Offset: 0x00180235
		public static GridLength Auto
		{
			get
			{
				return GridLength.s_auto;
			}
		}

		// Token: 0x040010F0 RID: 4336
		private double _unitValue;

		// Token: 0x040010F1 RID: 4337
		private GridUnitType _unitType;

		// Token: 0x040010F2 RID: 4338
		private static readonly GridLength s_auto = new GridLength(1.0, GridUnitType.Auto);
	}
}
