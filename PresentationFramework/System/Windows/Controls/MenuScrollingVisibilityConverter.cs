using System;
using System.Globalization;
using System.Windows.Data;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020007B0 RID: 1968
	public sealed class MenuScrollingVisibilityConverter : IMultiValueConverter
	{
		// Token: 0x06006FC5 RID: 28613 RVA: 0x002D61F4 File Offset: 0x002D51F4
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			Type typeFromHandle = typeof(double);
			if (parameter == null || values == null || values.Length != 4 || values[0] == null || values[1] == null || values[2] == null || values[3] == null || !typeof(Visibility).IsAssignableFrom(values[0].GetType()) || !typeFromHandle.IsAssignableFrom(values[1].GetType()) || !typeFromHandle.IsAssignableFrom(values[2].GetType()) || !typeFromHandle.IsAssignableFrom(values[3].GetType()))
			{
				return DependencyProperty.UnsetValue;
			}
			Type type = parameter.GetType();
			if (!typeFromHandle.IsAssignableFrom(type) && !typeof(string).IsAssignableFrom(type))
			{
				return DependencyProperty.UnsetValue;
			}
			if ((Visibility)values[0] != Visibility.Visible)
			{
				return Visibility.Collapsed;
			}
			double value;
			if (parameter is string)
			{
				value = double.Parse((string)parameter, NumberFormatInfo.InvariantInfo);
			}
			else
			{
				value = (double)parameter;
			}
			double num = (double)values[1];
			double num2 = (double)values[2];
			double num3 = (double)values[3];
			if (num2 != num3 && DoubleUtil.AreClose(Math.Min(100.0, Math.Max(0.0, num * 100.0 / (num2 - num3))), value))
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		// Token: 0x06006FC6 RID: 28614 RVA: 0x002267AD File Offset: 0x002257AD
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				Binding.DoNothing
			};
		}
	}
}
