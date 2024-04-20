using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200075B RID: 1883
	[TypeConverter(typeof(DataGridLengthConverter))]
	public struct DataGridLength : IEquatable<DataGridLength>
	{
		// Token: 0x0600666B RID: 26219 RVA: 0x002B19CB File Offset: 0x002B09CB
		public DataGridLength(double pixels)
		{
			this = new DataGridLength(pixels, DataGridLengthUnitType.Pixel);
		}

		// Token: 0x0600666C RID: 26220 RVA: 0x002B19D5 File Offset: 0x002B09D5
		public DataGridLength(double value, DataGridLengthUnitType type)
		{
			this = new DataGridLength(value, type, (type == DataGridLengthUnitType.Pixel) ? value : double.NaN, (type == DataGridLengthUnitType.Pixel) ? value : double.NaN);
		}

		// Token: 0x0600666D RID: 26221 RVA: 0x002B1A00 File Offset: 0x002B0A00
		public DataGridLength(double value, DataGridLengthUnitType type, double desiredValue, double displayValue)
		{
			if (DoubleUtil.IsNaN(value) || double.IsInfinity(value))
			{
				throw new ArgumentException(SR.Get("DataGridLength_Infinity"), "value");
			}
			if (type != DataGridLengthUnitType.Auto && type != DataGridLengthUnitType.Pixel && type != DataGridLengthUnitType.Star && type != DataGridLengthUnitType.SizeToCells && type != DataGridLengthUnitType.SizeToHeader)
			{
				throw new ArgumentException(SR.Get("DataGridLength_InvalidType"), "type");
			}
			if (double.IsInfinity(desiredValue))
			{
				throw new ArgumentException(SR.Get("DataGridLength_Infinity"), "desiredValue");
			}
			if (double.IsInfinity(displayValue))
			{
				throw new ArgumentException(SR.Get("DataGridLength_Infinity"), "displayValue");
			}
			this._unitValue = ((type == DataGridLengthUnitType.Auto) ? 1.0 : value);
			this._unitType = type;
			this._desiredValue = desiredValue;
			this._displayValue = displayValue;
		}

		// Token: 0x0600666E RID: 26222 RVA: 0x002B1AC0 File Offset: 0x002B0AC0
		public static bool operator ==(DataGridLength gl1, DataGridLength gl2)
		{
			return gl1.UnitType == gl2.UnitType && gl1.Value == gl2.Value && (gl1.DesiredValue == gl2.DesiredValue || (DoubleUtil.IsNaN(gl1.DesiredValue) && DoubleUtil.IsNaN(gl2.DesiredValue))) && (gl1.DisplayValue == gl2.DisplayValue || (DoubleUtil.IsNaN(gl1.DisplayValue) && DoubleUtil.IsNaN(gl2.DisplayValue)));
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x002B1B4C File Offset: 0x002B0B4C
		public static bool operator !=(DataGridLength gl1, DataGridLength gl2)
		{
			return gl1.UnitType != gl2.UnitType || gl1.Value != gl2.Value || (gl1.DesiredValue != gl2.DesiredValue && (!DoubleUtil.IsNaN(gl1.DesiredValue) || !DoubleUtil.IsNaN(gl2.DesiredValue))) || (gl1.DisplayValue != gl2.DisplayValue && (!DoubleUtil.IsNaN(gl1.DisplayValue) || !DoubleUtil.IsNaN(gl2.DisplayValue)));
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x002B1BD8 File Offset: 0x002B0BD8
		public override bool Equals(object obj)
		{
			if (obj is DataGridLength)
			{
				DataGridLength gl = (DataGridLength)obj;
				return this == gl;
			}
			return false;
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x002B1C02 File Offset: 0x002B0C02
		public bool Equals(DataGridLength other)
		{
			return this == other;
		}

		// Token: 0x06006672 RID: 26226 RVA: 0x002B1C10 File Offset: 0x002B0C10
		public override int GetHashCode()
		{
			return (int)((int)this._unitValue + this._unitType + (int)this._desiredValue + (int)this._displayValue);
		}

		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x06006673 RID: 26227 RVA: 0x002B1C30 File Offset: 0x002B0C30
		public bool IsAbsolute
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.Pixel;
			}
		}

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x06006674 RID: 26228 RVA: 0x002B1C3B File Offset: 0x002B0C3B
		public bool IsAuto
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.Auto;
			}
		}

		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x06006675 RID: 26229 RVA: 0x002B1C46 File Offset: 0x002B0C46
		public bool IsStar
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.Star;
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x06006676 RID: 26230 RVA: 0x002B1C51 File Offset: 0x002B0C51
		public bool IsSizeToCells
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.SizeToCells;
			}
		}

		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x06006677 RID: 26231 RVA: 0x002B1C5C File Offset: 0x002B0C5C
		public bool IsSizeToHeader
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.SizeToHeader;
			}
		}

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06006678 RID: 26232 RVA: 0x002B1C67 File Offset: 0x002B0C67
		public double Value
		{
			get
			{
				if (this._unitType != DataGridLengthUnitType.Auto)
				{
					return this._unitValue;
				}
				return 1.0;
			}
		}

		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x06006679 RID: 26233 RVA: 0x002B1C81 File Offset: 0x002B0C81
		public DataGridLengthUnitType UnitType
		{
			get
			{
				return this._unitType;
			}
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x0600667A RID: 26234 RVA: 0x002B1C89 File Offset: 0x002B0C89
		public double DesiredValue
		{
			get
			{
				return this._desiredValue;
			}
		}

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x0600667B RID: 26235 RVA: 0x002B1C91 File Offset: 0x002B0C91
		public double DisplayValue
		{
			get
			{
				return this._displayValue;
			}
		}

		// Token: 0x0600667C RID: 26236 RVA: 0x002B1C99 File Offset: 0x002B0C99
		public override string ToString()
		{
			return DataGridLengthConverter.ConvertToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x0600667D RID: 26237 RVA: 0x002B1CAB File Offset: 0x002B0CAB
		public static DataGridLength Auto
		{
			get
			{
				return DataGridLength._auto;
			}
		}

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x0600667E RID: 26238 RVA: 0x002B1CB2 File Offset: 0x002B0CB2
		public static DataGridLength SizeToCells
		{
			get
			{
				return DataGridLength._sizeToCells;
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x0600667F RID: 26239 RVA: 0x002B1CB9 File Offset: 0x002B0CB9
		public static DataGridLength SizeToHeader
		{
			get
			{
				return DataGridLength._sizeToHeader;
			}
		}

		// Token: 0x06006680 RID: 26240 RVA: 0x002B1CC0 File Offset: 0x002B0CC0
		public static implicit operator DataGridLength(double value)
		{
			return new DataGridLength(value);
		}

		// Token: 0x040033C0 RID: 13248
		private double _unitValue;

		// Token: 0x040033C1 RID: 13249
		private DataGridLengthUnitType _unitType;

		// Token: 0x040033C2 RID: 13250
		private double _desiredValue;

		// Token: 0x040033C3 RID: 13251
		private double _displayValue;

		// Token: 0x040033C4 RID: 13252
		private const double AutoValue = 1.0;

		// Token: 0x040033C5 RID: 13253
		private static readonly DataGridLength _auto = new DataGridLength(1.0, DataGridLengthUnitType.Auto, 0.0, 0.0);

		// Token: 0x040033C6 RID: 13254
		private static readonly DataGridLength _sizeToCells = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells, 0.0, 0.0);

		// Token: 0x040033C7 RID: 13255
		private static readonly DataGridLength _sizeToHeader = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader, 0.0, 0.0);
	}
}
