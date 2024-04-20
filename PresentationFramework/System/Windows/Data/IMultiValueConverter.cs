using System;
using System.Globalization;

namespace System.Windows.Data
{
	// Token: 0x0200045E RID: 1118
	public interface IMultiValueConverter
	{
		// Token: 0x060038DD RID: 14557
		object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

		// Token: 0x060038DE RID: 14558
		object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
	}
}
