using System;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Shell
{
	// Token: 0x020003F1 RID: 1009
	public sealed class TaskbarItemInfo : Freezable
	{
		// Token: 0x06002B45 RID: 11077 RVA: 0x001A2041 File Offset: 0x001A1041
		protected override Freezable CreateInstanceCore()
		{
			return new TaskbarItemInfo();
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06002B46 RID: 11078 RVA: 0x001A2048 File Offset: 0x001A1048
		// (set) Token: 0x06002B47 RID: 11079 RVA: 0x001A205A File Offset: 0x001A105A
		public TaskbarItemProgressState ProgressState
		{
			get
			{
				return (TaskbarItemProgressState)base.GetValue(TaskbarItemInfo.ProgressStateProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ProgressStateProperty, value);
			}
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x001A206D File Offset: 0x001A106D
		private TaskbarItemProgressState CoerceProgressState(TaskbarItemProgressState value)
		{
			if (value > TaskbarItemProgressState.Paused)
			{
				value = TaskbarItemProgressState.None;
			}
			return value;
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06002B49 RID: 11081 RVA: 0x001A2077 File Offset: 0x001A1077
		// (set) Token: 0x06002B4A RID: 11082 RVA: 0x001A2089 File Offset: 0x001A1089
		public double ProgressValue
		{
			get
			{
				return (double)base.GetValue(TaskbarItemInfo.ProgressValueProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ProgressValueProperty, value);
			}
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x001A209C File Offset: 0x001A109C
		private static double CoerceProgressValue(double progressValue)
		{
			if (double.IsNaN(progressValue))
			{
				progressValue = 0.0;
			}
			else
			{
				progressValue = Math.Max(progressValue, 0.0);
				progressValue = Math.Min(1.0, progressValue);
			}
			return progressValue;
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06002B4C RID: 11084 RVA: 0x001A20D6 File Offset: 0x001A10D6
		// (set) Token: 0x06002B4D RID: 11085 RVA: 0x001A20E8 File Offset: 0x001A10E8
		public ImageSource Overlay
		{
			get
			{
				return (ImageSource)base.GetValue(TaskbarItemInfo.OverlayProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.OverlayProperty, value);
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06002B4E RID: 11086 RVA: 0x001A20F6 File Offset: 0x001A10F6
		// (set) Token: 0x06002B4F RID: 11087 RVA: 0x001A2108 File Offset: 0x001A1108
		public string Description
		{
			get
			{
				return (string)base.GetValue(TaskbarItemInfo.DescriptionProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.DescriptionProperty, value);
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06002B50 RID: 11088 RVA: 0x001A2116 File Offset: 0x001A1116
		// (set) Token: 0x06002B51 RID: 11089 RVA: 0x001A2128 File Offset: 0x001A1128
		public Thickness ThumbnailClipMargin
		{
			get
			{
				return (Thickness)base.GetValue(TaskbarItemInfo.ThumbnailClipMarginProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ThumbnailClipMarginProperty, value);
			}
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x001A213C File Offset: 0x001A113C
		private Thickness CoerceThumbnailClipMargin(Thickness margin)
		{
			if (!margin.IsValid(false, false, false, false))
			{
				return default(Thickness);
			}
			return margin;
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06002B53 RID: 11091 RVA: 0x001A2161 File Offset: 0x001A1161
		// (set) Token: 0x06002B54 RID: 11092 RVA: 0x001A2173 File Offset: 0x001A1173
		public ThumbButtonInfoCollection ThumbButtonInfos
		{
			get
			{
				return (ThumbButtonInfoCollection)base.GetValue(TaskbarItemInfo.ThumbButtonInfosProperty);
			}
			set
			{
				base.SetValue(TaskbarItemInfo.ThumbButtonInfosProperty, value);
			}
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x001A2184 File Offset: 0x001A1184
		private void NotifyDependencyPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyPropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, e);
			}
		}

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06002B56 RID: 11094 RVA: 0x001A21A4 File Offset: 0x001A11A4
		// (remove) Token: 0x06002B57 RID: 11095 RVA: 0x001A21DC File Offset: 0x001A11DC
		internal event DependencyPropertyChangedEventHandler PropertyChanged;

		// Token: 0x04001AB7 RID: 6839
		public static readonly DependencyProperty ProgressStateProperty = DependencyProperty.Register("ProgressState", typeof(TaskbarItemProgressState), typeof(TaskbarItemInfo), new PropertyMetadata(TaskbarItemProgressState.None, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}, (DependencyObject d, object baseValue) => ((TaskbarItemInfo)d).CoerceProgressState((TaskbarItemProgressState)baseValue)));

		// Token: 0x04001AB8 RID: 6840
		public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register("ProgressValue", typeof(double), typeof(TaskbarItemInfo), new PropertyMetadata(0.0, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}, (DependencyObject d, object baseValue) => TaskbarItemInfo.CoerceProgressValue((double)baseValue)));

		// Token: 0x04001AB9 RID: 6841
		public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register("Overlay", typeof(ImageSource), typeof(TaskbarItemInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}));

		// Token: 0x04001ABA RID: 6842
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(TaskbarItemInfo), new PropertyMetadata(string.Empty, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}));

		// Token: 0x04001ABB RID: 6843
		public static readonly DependencyProperty ThumbnailClipMarginProperty = DependencyProperty.Register("ThumbnailClipMargin", typeof(Thickness), typeof(TaskbarItemInfo), new PropertyMetadata(default(Thickness), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}, (DependencyObject d, object baseValue) => ((TaskbarItemInfo)d).CoerceThumbnailClipMargin((Thickness)baseValue)));

		// Token: 0x04001ABC RID: 6844
		public static readonly DependencyProperty ThumbButtonInfosProperty = DependencyProperty.Register("ThumbButtonInfos", typeof(ThumbButtonInfoCollection), typeof(TaskbarItemInfo), new PropertyMetadata(new FreezableDefaultValueFactory(ThumbButtonInfoCollection.Empty), delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TaskbarItemInfo)d).NotifyDependencyPropertyChanged(e);
		}));
	}
}
