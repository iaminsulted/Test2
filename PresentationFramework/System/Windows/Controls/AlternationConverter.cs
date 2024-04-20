using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows.Controls
{
	// Token: 0x02000719 RID: 1817
	[ContentProperty("Values")]
	public class AlternationConverter : IValueConverter
	{
		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x06005F91 RID: 24465 RVA: 0x002955F4 File Offset: 0x002945F4
		public IList Values
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x002955FC File Offset: 0x002945FC
		public object Convert(object o, Type targetType, object parameter, CultureInfo culture)
		{
			if (this._values.Count > 0 && o is int)
			{
				int num = (int)o % this._values.Count;
				if (num < 0)
				{
					num += this._values.Count;
				}
				return this._values[num];
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06005F93 RID: 24467 RVA: 0x00295656 File Offset: 0x00294656
		public object ConvertBack(object o, Type targetType, object parameter, CultureInfo culture)
		{
			return this._values.IndexOf(o);
		}

		// Token: 0x040031D6 RID: 12758
		private List<object> _values = new List<object>();
	}
}
