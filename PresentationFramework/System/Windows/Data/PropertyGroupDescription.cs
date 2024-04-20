using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Data
{
	// Token: 0x02000467 RID: 1127
	public class PropertyGroupDescription : GroupDescription
	{
		// Token: 0x06003A02 RID: 14850 RVA: 0x001EF73D File Offset: 0x001EE73D
		public PropertyGroupDescription()
		{
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x001EF74C File Offset: 0x001EE74C
		public PropertyGroupDescription(string propertyName)
		{
			this.UpdatePropertyName(propertyName);
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x001EF762 File Offset: 0x001EE762
		public PropertyGroupDescription(string propertyName, IValueConverter converter)
		{
			this.UpdatePropertyName(propertyName);
			this._converter = converter;
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x001EF77F File Offset: 0x001EE77F
		public PropertyGroupDescription(string propertyName, IValueConverter converter, StringComparison stringComparison)
		{
			this.UpdatePropertyName(propertyName);
			this._converter = converter;
			this._stringComparison = stringComparison;
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06003A06 RID: 14854 RVA: 0x001EF7A3 File Offset: 0x001EE7A3
		// (set) Token: 0x06003A07 RID: 14855 RVA: 0x001EF7AB File Offset: 0x001EE7AB
		[DefaultValue(null)]
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
			set
			{
				this.UpdatePropertyName(value);
				this.OnPropertyChanged("PropertyName");
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x06003A08 RID: 14856 RVA: 0x001EF7BF File Offset: 0x001EE7BF
		// (set) Token: 0x06003A09 RID: 14857 RVA: 0x001EF7C7 File Offset: 0x001EE7C7
		[DefaultValue(null)]
		public IValueConverter Converter
		{
			get
			{
				return this._converter;
			}
			set
			{
				this._converter = value;
				this.OnPropertyChanged("Converter");
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06003A0A RID: 14858 RVA: 0x001EF7DB File Offset: 0x001EE7DB
		// (set) Token: 0x06003A0B RID: 14859 RVA: 0x001EF7E3 File Offset: 0x001EE7E3
		[DefaultValue(StringComparison.Ordinal)]
		public StringComparison StringComparison
		{
			get
			{
				return this._stringComparison;
			}
			set
			{
				this._stringComparison = value;
				this.OnPropertyChanged("StringComparison");
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06003A0C RID: 14860 RVA: 0x001EF7F7 File Offset: 0x001EE7F7
		public static IComparer CompareNameAscending
		{
			get
			{
				return PropertyGroupDescription._compareNameAscending;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x001EF7FE File Offset: 0x001EE7FE
		public static IComparer CompareNameDescending
		{
			get
			{
				return PropertyGroupDescription._compareNameDescending;
			}
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x001EF808 File Offset: 0x001EE808
		public override object GroupNameFromItem(object item, int level, CultureInfo culture)
		{
			object obj;
			object obj2;
			if (string.IsNullOrEmpty(this.PropertyName))
			{
				obj = item;
			}
			else if (SystemXmlHelper.TryGetValueFromXmlNode(item, this.PropertyName, out obj2))
			{
				obj = obj2;
			}
			else
			{
				if (item != null)
				{
					using (this._propertyPath.SetContext(item))
					{
						obj = this._propertyPath.GetValue();
						goto IL_4F;
					}
				}
				obj = null;
			}
			IL_4F:
			if (this.Converter != null)
			{
				obj = this.Converter.Convert(obj, typeof(object), level, culture);
			}
			return obj;
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x001EF89C File Offset: 0x001EE89C
		public override bool NamesMatch(object groupName, object itemName)
		{
			string text = groupName as string;
			string text2 = itemName as string;
			if (text != null && text2 != null)
			{
				return string.Equals(text, text2, this.StringComparison);
			}
			return object.Equals(groupName, itemName);
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x001EF8D2 File Offset: 0x001EE8D2
		private void UpdatePropertyName(string propertyName)
		{
			this._propertyName = propertyName;
			this._propertyPath = ((!string.IsNullOrEmpty(propertyName)) ? new PropertyPath(propertyName, Array.Empty<object>()) : null);
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x001EF8F7 File Offset: 0x001EE8F7
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x04001D86 RID: 7558
		private string _propertyName;

		// Token: 0x04001D87 RID: 7559
		private PropertyPath _propertyPath;

		// Token: 0x04001D88 RID: 7560
		private IValueConverter _converter;

		// Token: 0x04001D89 RID: 7561
		private StringComparison _stringComparison = StringComparison.Ordinal;

		// Token: 0x04001D8A RID: 7562
		private static readonly IComparer _compareNameAscending = new PropertyGroupDescription.NameComparer(ListSortDirection.Ascending);

		// Token: 0x04001D8B RID: 7563
		private static readonly IComparer _compareNameDescending = new PropertyGroupDescription.NameComparer(ListSortDirection.Descending);

		// Token: 0x02000AEA RID: 2794
		private class NameComparer : IComparer
		{
			// Token: 0x06008B52 RID: 35666 RVA: 0x00339C04 File Offset: 0x00338C04
			public NameComparer(ListSortDirection direction)
			{
				this._direction = direction;
			}

			// Token: 0x06008B53 RID: 35667 RVA: 0x00339C14 File Offset: 0x00338C14
			int IComparer.Compare(object x, object y)
			{
				CollectionViewGroup collectionViewGroup = x as CollectionViewGroup;
				object a = ((collectionViewGroup != null) ? collectionViewGroup.Name : null) ?? x;
				CollectionViewGroup collectionViewGroup2 = y as CollectionViewGroup;
				object b = ((collectionViewGroup2 != null) ? collectionViewGroup2.Name : null) ?? y;
				int num = Comparer.DefaultInvariant.Compare(a, b);
				if (this._direction != ListSortDirection.Ascending)
				{
					return -num;
				}
				return num;
			}

			// Token: 0x04004728 RID: 18216
			private ListSortDirection _direction;
		}
	}
}
