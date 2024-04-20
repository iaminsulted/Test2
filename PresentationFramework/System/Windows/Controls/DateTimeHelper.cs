using System;
using System.Globalization;
using MS.Internal.Text;

namespace System.Windows.Controls
{
	// Token: 0x02000770 RID: 1904
	internal static class DateTimeHelper
	{
		// Token: 0x0600678B RID: 26507 RVA: 0x002B582C File Offset: 0x002B482C
		public static DateTime? AddDays(DateTime time, int days)
		{
			DateTime? result;
			try
			{
				result = new DateTime?(DateTimeHelper.cal.AddDays(time, days));
			}
			catch (ArgumentException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600678C RID: 26508 RVA: 0x002B586C File Offset: 0x002B486C
		public static DateTime? AddMonths(DateTime time, int months)
		{
			DateTime? result;
			try
			{
				result = new DateTime?(DateTimeHelper.cal.AddMonths(time, months));
			}
			catch (ArgumentException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x002B58AC File Offset: 0x002B48AC
		public static DateTime? AddYears(DateTime time, int years)
		{
			DateTime? result;
			try
			{
				result = new DateTime?(DateTimeHelper.cal.AddYears(time, years));
			}
			catch (ArgumentException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x002B58EC File Offset: 0x002B48EC
		public static DateTime? SetYear(DateTime date, int year)
		{
			return DateTimeHelper.AddYears(date, year - date.Year);
		}

		// Token: 0x0600678F RID: 26511 RVA: 0x002B5900 File Offset: 0x002B4900
		public static DateTime? SetYearMonth(DateTime date, DateTime yearMonth)
		{
			DateTime? result = DateTimeHelper.SetYear(date, yearMonth.Year);
			if (result != null)
			{
				result = DateTimeHelper.AddMonths(result.Value, yearMonth.Month - date.Month);
			}
			return result;
		}

		// Token: 0x06006790 RID: 26512 RVA: 0x002B5944 File Offset: 0x002B4944
		public static int CompareDays(DateTime dt1, DateTime dt2)
		{
			return DateTime.Compare(DateTimeHelper.DiscardTime(new DateTime?(dt1)).Value, DateTimeHelper.DiscardTime(new DateTime?(dt2)).Value);
		}

		// Token: 0x06006791 RID: 26513 RVA: 0x002B597C File Offset: 0x002B497C
		public static int CompareYearMonth(DateTime dt1, DateTime dt2)
		{
			return (dt1.Year - dt2.Year) * 12 + (dt1.Month - dt2.Month);
		}

		// Token: 0x06006792 RID: 26514 RVA: 0x002B59A0 File Offset: 0x002B49A0
		public static int DecadeOfDate(DateTime date)
		{
			return date.Year - date.Year % 10;
		}

		// Token: 0x06006793 RID: 26515 RVA: 0x002B59B4 File Offset: 0x002B49B4
		public static DateTime DiscardDayTime(DateTime d)
		{
			return new DateTime(d.Year, d.Month, 1, 0, 0, 0);
		}

		// Token: 0x06006794 RID: 26516 RVA: 0x002B59D0 File Offset: 0x002B49D0
		public static DateTime? DiscardTime(DateTime? d)
		{
			if (d == null)
			{
				return null;
			}
			return new DateTime?(d.Value.Date);
		}

		// Token: 0x06006795 RID: 26517 RVA: 0x002B5A04 File Offset: 0x002B4A04
		public static int EndOfDecade(DateTime date)
		{
			return DateTimeHelper.DecadeOfDate(date) + 9;
		}

		// Token: 0x06006796 RID: 26518 RVA: 0x002B5A0F File Offset: 0x002B4A0F
		public static DateTimeFormatInfo GetCurrentDateFormat()
		{
			return DateTimeHelper.GetDateFormat(CultureInfo.CurrentCulture);
		}

		// Token: 0x06006797 RID: 26519 RVA: 0x002B5A1C File Offset: 0x002B4A1C
		internal static CultureInfo GetCulture(FrameworkElement element)
		{
			bool flag;
			CultureInfo result;
			if (element.GetValueSource(FrameworkElement.LanguageProperty, null, out flag) != BaseValueSourceInternal.Default)
			{
				result = DynamicPropertyReader.GetCultureInfo(element);
			}
			else
			{
				result = CultureInfo.CurrentCulture;
			}
			return result;
		}

		// Token: 0x06006798 RID: 26520 RVA: 0x002B5A4C File Offset: 0x002B4A4C
		internal static DateTimeFormatInfo GetDateFormat(CultureInfo culture)
		{
			if (culture.Calendar is GregorianCalendar)
			{
				return culture.DateTimeFormat;
			}
			GregorianCalendar gregorianCalendar = null;
			foreach (Calendar calendar in culture.OptionalCalendars)
			{
				if (calendar is GregorianCalendar)
				{
					if (gregorianCalendar == null)
					{
						gregorianCalendar = (calendar as GregorianCalendar);
					}
					if (((GregorianCalendar)calendar).CalendarType == GregorianCalendarTypes.Localized)
					{
						gregorianCalendar = (calendar as GregorianCalendar);
						break;
					}
				}
			}
			DateTimeFormatInfo dateTimeFormat;
			if (gregorianCalendar == null)
			{
				dateTimeFormat = ((CultureInfo)CultureInfo.InvariantCulture.Clone()).DateTimeFormat;
				dateTimeFormat.Calendar = new GregorianCalendar();
			}
			else
			{
				dateTimeFormat = ((CultureInfo)culture.Clone()).DateTimeFormat;
				dateTimeFormat.Calendar = gregorianCalendar;
			}
			return dateTimeFormat;
		}

		// Token: 0x06006799 RID: 26521 RVA: 0x002B5AF6 File Offset: 0x002B4AF6
		public static bool InRange(DateTime date, CalendarDateRange range)
		{
			return DateTimeHelper.InRange(date, range.Start, range.End);
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x002B5B0A File Offset: 0x002B4B0A
		public static bool InRange(DateTime date, DateTime start, DateTime end)
		{
			return DateTimeHelper.CompareDays(date, start) > -1 && DateTimeHelper.CompareDays(date, end) < 1;
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x002B5B24 File Offset: 0x002B4B24
		public static string ToDayString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.Day.ToString(dateFormat);
			}
			return result;
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x002B5B64 File Offset: 0x002B4B64
		public static string ToDecadeRangeString(int decade, FrameworkElement fe)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(fe));
			if (dateFormat != null)
			{
				bool flag = fe.FlowDirection == FlowDirection.RightToLeft;
				int num = flag ? decade : (decade + 9);
				result = (flag ? (decade + 9) : decade).ToString(dateFormat) + "-" + num.ToString(dateFormat);
			}
			return result;
		}

		// Token: 0x0600679D RID: 26525 RVA: 0x002B5BC0 File Offset: 0x002B4BC0
		public static string ToYearMonthPatternString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.ToString(dateFormat.YearMonthPattern, dateFormat);
			}
			return result;
		}

		// Token: 0x0600679E RID: 26526 RVA: 0x002B5C00 File Offset: 0x002B4C00
		public static string ToYearString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.Year.ToString(dateFormat);
			}
			return result;
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x002B5C40 File Offset: 0x002B4C40
		public static string ToAbbreviatedMonthString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				string[] abbreviatedMonthNames = dateFormat.AbbreviatedMonthNames;
				if (abbreviatedMonthNames != null && abbreviatedMonthNames.Length != 0)
				{
					result = abbreviatedMonthNames[(date.Value.Month - 1) % abbreviatedMonthNames.Length];
				}
			}
			return result;
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x002B5C90 File Offset: 0x002B4C90
		public static string ToLongDateString(DateTime? date, CultureInfo culture)
		{
			string result = string.Empty;
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
			if (date != null && dateFormat != null)
			{
				result = date.Value.Date.ToString(dateFormat.LongDatePattern, dateFormat);
			}
			return result;
		}

		// Token: 0x0400344E RID: 13390
		private static Calendar cal = new GregorianCalendar();
	}
}
