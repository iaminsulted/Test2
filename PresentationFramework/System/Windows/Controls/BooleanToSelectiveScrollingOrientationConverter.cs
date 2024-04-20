using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x0200071C RID: 1820
	[Localizability(LocalizationCategory.NeverLocalize)]
	internal sealed class BooleanToSelectiveScrollingOrientationConverter : IValueConverter
	{
		// Token: 0x06005FB5 RID: 24501 RVA: 0x00295FEC File Offset: 0x00294FEC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && parameter is SelectiveScrollingOrientation)
			{
				bool flag = (bool)value;
				SelectiveScrollingOrientation selectiveScrollingOrientation = (SelectiveScrollingOrientation)parameter;
				if (flag)
				{
					return selectiveScrollingOrientation;
				}
			}
			return SelectiveScrollingOrientation.Both;
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x001056E1 File Offset: 0x001046E1
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
