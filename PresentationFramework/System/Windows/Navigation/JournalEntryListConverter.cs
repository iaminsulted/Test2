using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Navigation
{
	// Token: 0x020005B2 RID: 1458
	public sealed class JournalEntryListConverter : IValueConverter
	{
		// Token: 0x0600468F RID: 18063 RVA: 0x00226704 File Offset: 0x00225704
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			return ((JournalEntryStack)value).GetLimitedJournalEntryStackEnumerable();
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x00226716 File Offset: 0x00225716
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}
}
