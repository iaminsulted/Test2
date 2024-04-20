using System;
using System.ComponentModel;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000820 RID: 2080
	public class RowDefinition : DefinitionBase
	{
		// Token: 0x06007954 RID: 31060 RVA: 0x00302BDD File Offset: 0x00301BDD
		public RowDefinition() : base(false)
		{
		}

		// Token: 0x17001C19 RID: 7193
		// (get) Token: 0x06007955 RID: 31061 RVA: 0x0030224E File Offset: 0x0030124E
		// (set) Token: 0x06007956 RID: 31062 RVA: 0x00302BE6 File Offset: 0x00301BE6
		public GridLength Height
		{
			get
			{
				return base.UserSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.HeightProperty, value);
			}
		}

		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x06007957 RID: 31063 RVA: 0x002B612C File Offset: 0x002B512C
		// (set) Token: 0x06007958 RID: 31064 RVA: 0x00302BF9 File Offset: 0x00301BF9
		[TypeConverter(typeof(LengthConverter))]
		public double MinHeight
		{
			get
			{
				return base.UserMinSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.MinHeightProperty, value);
			}
		}

		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x06007959 RID: 31065 RVA: 0x002B6134 File Offset: 0x002B5134
		// (set) Token: 0x0600795A RID: 31066 RVA: 0x00302C0C File Offset: 0x00301C0C
		[TypeConverter(typeof(LengthConverter))]
		public double MaxHeight
		{
			get
			{
				return base.UserMaxSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.MaxHeightProperty, value);
			}
		}

		// Token: 0x17001C1C RID: 7196
		// (get) Token: 0x0600795B RID: 31067 RVA: 0x00302C20 File Offset: 0x00301C20
		public double ActualHeight
		{
			get
			{
				double result = 0.0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalRowDefinitionHeight(base.Index);
				}
				return result;
			}
		}

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x0600795C RID: 31068 RVA: 0x003022C8 File Offset: 0x003012C8
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

		// Token: 0x0400399F RID: 14751
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(GridLength), typeof(RowDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

		// Token: 0x040039A0 RID: 14752
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

		// Token: 0x040039A1 RID: 14753
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
	}
}
