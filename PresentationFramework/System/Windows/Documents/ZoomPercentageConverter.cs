using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Documents
{
	// Token: 0x020006E7 RID: 1767
	public sealed class ZoomPercentageConverter : IValueConverter
	{
		// Token: 0x06005CEE RID: 23790 RVA: 0x0028B974 File Offset: 0x0028A974
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (targetType == null)
			{
				return DependencyProperty.UnsetValue;
			}
			if (value != null && value is double)
			{
				double num = (double)value;
				if (targetType == typeof(string) || targetType == typeof(object))
				{
					if (double.IsNaN(num) || double.IsInfinity(num))
					{
						return DependencyProperty.UnsetValue;
					}
					return string.Format(CultureInfo.CurrentCulture, "{0:0.##}%", num);
				}
				else if (targetType == typeof(double))
				{
					return num;
				}
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06005CEF RID: 23791 RVA: 0x0028BA10 File Offset: 0x0028AA10
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(targetType == typeof(double)) || value == null)
			{
				return DependencyProperty.UnsetValue;
			}
			double num = 0.0;
			bool flag = false;
			if (value is int)
			{
				num = (double)((int)value);
				flag = true;
			}
			else if (value is double)
			{
				num = (double)value;
				flag = true;
			}
			else if (value is string)
			{
				try
				{
					string text = (string)value;
					if (culture != null && !string.IsNullOrEmpty(text))
					{
						text = ((string)value).Trim();
						if (!culture.IsNeutralCulture && text.Length > 0 && culture.NumberFormat != null)
						{
							int percentPositivePattern = culture.NumberFormat.PercentPositivePattern;
							if (percentPositivePattern > 1)
							{
								if (percentPositivePattern == 2)
								{
									if (text.IndexOf(culture.NumberFormat.PercentSymbol, StringComparison.CurrentCultureIgnoreCase) == 0)
									{
										text = text.Substring(1);
									}
								}
							}
							else if (text.Length - 1 == text.LastIndexOf(culture.NumberFormat.PercentSymbol, StringComparison.CurrentCultureIgnoreCase))
							{
								text = text.Substring(0, text.Length - 1);
							}
						}
						num = System.Convert.ToDouble(text, culture);
						flag = true;
					}
				}
				catch (ArgumentOutOfRangeException)
				{
				}
				catch (ArgumentNullException)
				{
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			if (!flag)
			{
				return DependencyProperty.UnsetValue;
			}
			return num;
		}

		// Token: 0x0400313A RID: 12602
		internal const string ZoomPercentageConverterStringFormat = "{0:0.##}%";
	}
}
