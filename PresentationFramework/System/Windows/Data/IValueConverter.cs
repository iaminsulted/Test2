using System;
using System.Globalization;

namespace System.Windows.Data
{
	// Token: 0x0200045F RID: 1119
	public interface IValueConverter
	{
		// Token: 0x060038DF RID: 14559
		object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		// Token: 0x060038E0 RID: 14560
		object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
	}
}
