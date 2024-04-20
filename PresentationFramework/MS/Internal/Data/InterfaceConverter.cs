using System;
using System.Globalization;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200021E RID: 542
	internal class InterfaceConverter : IValueConverter
	{
		// Token: 0x06001465 RID: 5221 RVA: 0x00151F25 File Offset: 0x00150F25
		internal InterfaceConverter(Type sourceType, Type targetType)
		{
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00151F3B File Offset: 0x00150F3B
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return this.ConvertTo(o, this._targetType);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00151F4A File Offset: 0x00150F4A
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return this.ConvertTo(o, this._sourceType);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00151F59 File Offset: 0x00150F59
		private object ConvertTo(object o, Type type)
		{
			if (!type.IsInstanceOfType(o))
			{
				return null;
			}
			return o;
		}

		// Token: 0x04000BC9 RID: 3017
		private Type _sourceType;

		// Token: 0x04000BCA RID: 3018
		private Type _targetType;
	}
}
