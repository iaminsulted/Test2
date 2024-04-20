using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Navigation
{
	// Token: 0x020005B4 RID: 1460
	public sealed class JournalEntryUnifiedViewConverter : IMultiValueConverter
	{
		// Token: 0x06004692 RID: 18066 RVA: 0x0022671D File Offset: 0x0022571D
		public static JournalEntryPosition GetJournalEntryPosition(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (JournalEntryPosition)element.GetValue(JournalEntryUnifiedViewConverter.JournalEntryPositionProperty);
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x0022673D File Offset: 0x0022573D
		public static void SetJournalEntryPosition(DependencyObject element, JournalEntryPosition position)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(JournalEntryUnifiedViewConverter.JournalEntryPositionProperty, position);
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x00226760 File Offset: 0x00225760
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values != null && values.Length == 2)
			{
				JournalEntryStack journalEntryStack = values[0] as JournalEntryStack;
				JournalEntryStack journalEntryStack2 = values[1] as JournalEntryStack;
				if (journalEntryStack != null && journalEntryStack2 != null)
				{
					LimitedJournalEntryStackEnumerable backStack = (LimitedJournalEntryStackEnumerable)journalEntryStack.GetLimitedJournalEntryStackEnumerable();
					LimitedJournalEntryStackEnumerable forwardStack = (LimitedJournalEntryStackEnumerable)journalEntryStack2.GetLimitedJournalEntryStackEnumerable();
					return new UnifiedJournalEntryStackEnumerable(backStack, forwardStack);
				}
			}
			return null;
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x002267AD File Offset: 0x002257AD
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				Binding.DoNothing
			};
		}

		// Token: 0x04002578 RID: 9592
		public static readonly DependencyProperty JournalEntryPositionProperty = DependencyProperty.RegisterAttached("JournalEntryPosition", typeof(JournalEntryPosition), typeof(JournalEntryUnifiedViewConverter), new PropertyMetadata(JournalEntryPosition.Current));
	}
}
