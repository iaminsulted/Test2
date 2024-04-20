using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000219 RID: 537
	internal class TargetDefaultValueConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x00151A92 File Offset: 0x00150A92
		public TargetDefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine) : base(typeConverter, sourceType, targetType, shouldConvertFrom, shouldConvertTo, engine)
		{
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00151AD1 File Offset: 0x00150AD1
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertFrom(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x00151AE8 File Offset: 0x00150AE8
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertTo(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
