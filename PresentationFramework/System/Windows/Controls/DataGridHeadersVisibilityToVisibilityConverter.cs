using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x02000757 RID: 1879
	[Localizability(LocalizationCategory.NeverLocalize)]
	internal sealed class DataGridHeadersVisibilityToVisibilityConverter : IValueConverter
	{
		// Token: 0x06006624 RID: 26148 RVA: 0x002B0C9C File Offset: 0x002AFC9C
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool flag = false;
			if (value is DataGridHeadersVisibility && parameter is DataGridHeadersVisibility)
			{
				DataGridHeadersVisibility dataGridHeadersVisibility = (DataGridHeadersVisibility)value;
				DataGridHeadersVisibility dataGridHeadersVisibility2 = (DataGridHeadersVisibility)parameter;
				switch (dataGridHeadersVisibility)
				{
				case DataGridHeadersVisibility.Column:
					flag = (dataGridHeadersVisibility2 == DataGridHeadersVisibility.Column || dataGridHeadersVisibility2 == DataGridHeadersVisibility.None);
					break;
				case DataGridHeadersVisibility.Row:
					flag = (dataGridHeadersVisibility2 == DataGridHeadersVisibility.Row || dataGridHeadersVisibility2 == DataGridHeadersVisibility.None);
					break;
				case DataGridHeadersVisibility.All:
					flag = true;
					break;
				}
			}
			if (targetType == typeof(Visibility))
			{
				return flag ? Visibility.Visible : Visibility.Collapsed;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06006625 RID: 26149 RVA: 0x001056E1 File Offset: 0x001046E1
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
