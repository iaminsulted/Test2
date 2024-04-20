using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x0200071D RID: 1821
	[Localizability(LocalizationCategory.NeverLocalize)]
	public sealed class BooleanToVisibilityConverter : IValueConverter
	{
		// Token: 0x06005FB8 RID: 24504 RVA: 0x00296028 File Offset: 0x00295028
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (value is bool)
			{
				flag = (bool)value;
			}
			else if (value is bool?)
			{
				bool? flag2 = (bool?)value;
				flag = (flag2 != null && flag2.Value);
			}
			return flag ? Visibility.Visible : Visibility.Collapsed;
		}

		// Token: 0x06005FB9 RID: 24505 RVA: 0x00296077 File Offset: 0x00295077
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility)
			{
				return (Visibility)value == Visibility.Visible;
			}
			return false;
		}
	}
}
