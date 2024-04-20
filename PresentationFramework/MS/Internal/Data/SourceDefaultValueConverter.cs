using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000218 RID: 536
	internal class SourceDefaultValueConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x06001450 RID: 5200 RVA: 0x00151A92 File Offset: 0x00150A92
		public SourceDefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine) : base(typeConverter, sourceType, targetType, shouldConvertFrom, shouldConvertTo, engine)
		{
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x00151AA3 File Offset: 0x00150AA3
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertTo(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00151ABA File Offset: 0x00150ABA
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertFrom(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
