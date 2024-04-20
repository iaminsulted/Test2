using System;
using System.ComponentModel;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x0200081E RID: 2078
	public class ColumnDefinition : DefinitionBase
	{
		// Token: 0x06007923 RID: 31011 RVA: 0x00302245 File Offset: 0x00301245
		public ColumnDefinition() : base(true)
		{
		}

		// Token: 0x17001C0B RID: 7179
		// (get) Token: 0x06007924 RID: 31012 RVA: 0x0030224E File Offset: 0x0030124E
		// (set) Token: 0x06007925 RID: 31013 RVA: 0x00302256 File Offset: 0x00301256
		public GridLength Width
		{
			get
			{
				return base.UserSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.WidthProperty, value);
			}
		}

		// Token: 0x17001C0C RID: 7180
		// (get) Token: 0x06007926 RID: 31014 RVA: 0x002B612C File Offset: 0x002B512C
		// (set) Token: 0x06007927 RID: 31015 RVA: 0x00302269 File Offset: 0x00301269
		[TypeConverter(typeof(LengthConverter))]
		public double MinWidth
		{
			get
			{
				return base.UserMinSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MinWidthProperty, value);
			}
		}

		// Token: 0x17001C0D RID: 7181
		// (get) Token: 0x06007928 RID: 31016 RVA: 0x002B6134 File Offset: 0x002B5134
		// (set) Token: 0x06007929 RID: 31017 RVA: 0x0030227C File Offset: 0x0030127C
		[TypeConverter(typeof(LengthConverter))]
		public double MaxWidth
		{
			get
			{
				return base.UserMaxSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MaxWidthProperty, value);
			}
		}

		// Token: 0x17001C0E RID: 7182
		// (get) Token: 0x0600792A RID: 31018 RVA: 0x00302290 File Offset: 0x00301290
		public double ActualWidth
		{
			get
			{
				double result = 0.0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalColumnDefinitionWidth(base.Index);
				}
				return result;
			}
		}

		// Token: 0x17001C0F RID: 7183
		// (get) Token: 0x0600792B RID: 31019 RVA: 0x003022C8 File Offset: 0x003012C8
		public double Offset
		{
			get
			{
				double result = 0.0;
				if (base.Index != 0)
				{
					result = base.FinalOffset;
				}
				return result;
			}
		}

		// Token: 0x04003997 RID: 14743
		[CommonDependencyProperty]
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(ColumnDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

		// Token: 0x04003998 RID: 14744
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

		// Token: 0x04003999 RID: 14745
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
	}
}
