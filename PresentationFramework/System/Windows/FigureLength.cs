using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x02000362 RID: 866
	[TypeConverter(typeof(FigureLengthConverter))]
	public struct FigureLength : IEquatable<FigureLength>
	{
		// Token: 0x060020A2 RID: 8354 RVA: 0x00175E52 File Offset: 0x00174E52
		public FigureLength(double pixels)
		{
			this = new FigureLength(pixels, FigureUnitType.Pixel);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00175E5C File Offset: 0x00174E5C
		public FigureLength(double value, FigureUnitType type)
		{
			double num = 1000.0;
			double num2 = (double)Math.Min(1000000, 3500000);
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
			if (value < 0.0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("InvalidCtorParameterNoNegative", new object[]
				{
					"value"
				}));
			}
			if (type != FigureUnitType.Auto && type != FigureUnitType.Pixel && type != FigureUnitType.Column && type != FigureUnitType.Content && type != FigureUnitType.Page)
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterUnknownFigureUnitType", new object[]
				{
					"type"
				}));
			}
			if (value > 1.0 && (type == FigureUnitType.Content || type == FigureUnitType.Page))
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value > num && type == FigureUnitType.Column)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value > num2 && type == FigureUnitType.Pixel)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			this._unitValue = ((type == FigureUnitType.Auto) ? 0.0 : value);
			this._unitType = type;
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x00175F8C File Offset: 0x00174F8C
		public static bool operator ==(FigureLength fl1, FigureLength fl2)
		{
			return fl1.FigureUnitType == fl2.FigureUnitType && fl1.Value == fl2.Value;
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x00175FB0 File Offset: 0x00174FB0
		public static bool operator !=(FigureLength fl1, FigureLength fl2)
		{
			return fl1.FigureUnitType != fl2.FigureUnitType || fl1.Value != fl2.Value;
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x00175FD8 File Offset: 0x00174FD8
		public override bool Equals(object oCompare)
		{
			if (oCompare is FigureLength)
			{
				FigureLength fl = (FigureLength)oCompare;
				return this == fl;
			}
			return false;
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x00176002 File Offset: 0x00175002
		public bool Equals(FigureLength figureLength)
		{
			return this == figureLength;
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00176010 File Offset: 0x00175010
		public override int GetHashCode()
		{
			return (int)((int)this._unitValue + this._unitType);
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060020A9 RID: 8361 RVA: 0x00176020 File Offset: 0x00175020
		public bool IsAbsolute
		{
			get
			{
				return this._unitType == FigureUnitType.Pixel;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060020AA RID: 8362 RVA: 0x0017602B File Offset: 0x0017502B
		public bool IsAuto
		{
			get
			{
				return this._unitType == FigureUnitType.Auto;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x00176036 File Offset: 0x00175036
		public bool IsColumn
		{
			get
			{
				return this._unitType == FigureUnitType.Column;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x00176041 File Offset: 0x00175041
		public bool IsContent
		{
			get
			{
				return this._unitType == FigureUnitType.Content;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060020AD RID: 8365 RVA: 0x0017604C File Offset: 0x0017504C
		public bool IsPage
		{
			get
			{
				return this._unitType == FigureUnitType.Page;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060020AE RID: 8366 RVA: 0x00176057 File Offset: 0x00175057
		public double Value
		{
			get
			{
				if (this._unitType != FigureUnitType.Auto)
				{
					return this._unitValue;
				}
				return 1.0;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060020AF RID: 8367 RVA: 0x00176071 File Offset: 0x00175071
		public FigureUnitType FigureUnitType
		{
			get
			{
				return this._unitType;
			}
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x00176079 File Offset: 0x00175079
		public override string ToString()
		{
			return FigureLengthConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x04000FF6 RID: 4086
		private double _unitValue;

		// Token: 0x04000FF7 RID: 4087
		private FigureUnitType _unitType;
	}
}
