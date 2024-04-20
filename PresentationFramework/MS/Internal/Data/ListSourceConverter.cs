using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200021D RID: 541
	internal class ListSourceConverter : IValueConverter
	{
		// Token: 0x06001462 RID: 5218 RVA: 0x00151F04 File Offset: 0x00150F04
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			IList result = null;
			IListSource listSource = o as IListSource;
			if (listSource != null)
			{
				result = listSource.GetList();
			}
			return result;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00109403 File Offset: 0x00108403
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
