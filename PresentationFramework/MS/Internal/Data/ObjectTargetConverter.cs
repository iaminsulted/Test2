using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200021B RID: 539
	internal class ObjectTargetConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x0600145C RID: 5212 RVA: 0x00151DDB File Offset: 0x00150DDB
		public ObjectTargetConverter(Type sourceType, DataBindEngine engine) : base(null, sourceType, typeof(object), true, false, engine)
		{
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x001136C4 File Offset: 0x001126C4
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return o;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x00151DF4 File Offset: 0x00150DF4
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			if (o == null && !this._sourceType.IsValueType)
			{
				return o;
			}
			if (o != null && this._sourceType.IsAssignableFrom(o.GetType()))
			{
				return o;
			}
			if (this._sourceType == typeof(string))
			{
				return string.Format(culture, "{0}", o);
			}
			base.EnsureConverter(this._sourceType);
			return base.ConvertFrom(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
