using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200021C RID: 540
	internal class ObjectSourceConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x0600145F RID: 5215 RVA: 0x00151E71 File Offset: 0x00150E71
		public ObjectSourceConverter(Type targetType, DataBindEngine engine) : base(null, typeof(object), targetType, true, false, engine)
		{
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00151E88 File Offset: 0x00150E88
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			if ((o != null && this._targetType.IsAssignableFrom(o.GetType())) || (o == null && !this._targetType.IsValueType))
			{
				return o;
			}
			if (this._targetType == typeof(string))
			{
				return string.Format(culture, "{0}", o);
			}
			base.EnsureConverter(this._targetType);
			return base.ConvertFrom(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x001136C4 File Offset: 0x001126C4
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return o;
		}
	}
}
