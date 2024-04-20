using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000430 RID: 1072
	public abstract class ThicknessKeyFrame : Freezable, IKeyFrame
	{
		// Token: 0x0600340F RID: 13327 RVA: 0x001A2211 File Offset: 0x001A1211
		protected ThicknessKeyFrame()
		{
		}

		// Token: 0x06003410 RID: 13328 RVA: 0x001DA08F File Offset: 0x001D908F
		protected ThicknessKeyFrame(Thickness value) : this()
		{
			this.Value = value;
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x001DA09E File Offset: 0x001D909E
		protected ThicknessKeyFrame(Thickness value, KeyTime keyTime) : this()
		{
			this.Value = value;
			this.KeyTime = keyTime;
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003412 RID: 13330 RVA: 0x001DA0B4 File Offset: 0x001D90B4
		// (set) Token: 0x06003413 RID: 13331 RVA: 0x001DA0C6 File Offset: 0x001D90C6
		public KeyTime KeyTime
		{
			get
			{
				return (KeyTime)base.GetValue(ThicknessKeyFrame.KeyTimeProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessKeyFrame.KeyTimeProperty, value);
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06003414 RID: 13332 RVA: 0x001DA0D9 File Offset: 0x001D90D9
		// (set) Token: 0x06003415 RID: 13333 RVA: 0x001DA0E6 File Offset: 0x001D90E6
		object IKeyFrame.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				this.Value = (Thickness)value;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06003416 RID: 13334 RVA: 0x001DA0F4 File Offset: 0x001D90F4
		// (set) Token: 0x06003417 RID: 13335 RVA: 0x001DA106 File Offset: 0x001D9106
		public Thickness Value
		{
			get
			{
				return (Thickness)base.GetValue(ThicknessKeyFrame.ValueProperty);
			}
			set
			{
				base.SetValueInternal(ThicknessKeyFrame.ValueProperty, value);
			}
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x001DA119 File Offset: 0x001D9119
		public Thickness InterpolateValue(Thickness baseValue, double keyFrameProgress)
		{
			if (keyFrameProgress < 0.0 || keyFrameProgress > 1.0)
			{
				throw new ArgumentOutOfRangeException("keyFrameProgress");
			}
			return this.InterpolateValueCore(baseValue, keyFrameProgress);
		}

		// Token: 0x06003419 RID: 13337
		protected abstract Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress);

		// Token: 0x04001C4B RID: 7243
		public static readonly DependencyProperty KeyTimeProperty = DependencyProperty.Register("KeyTime", typeof(KeyTime), typeof(ThicknessKeyFrame), new PropertyMetadata(KeyTime.Uniform));

		// Token: 0x04001C4C RID: 7244
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(Thickness), typeof(ThicknessKeyFrame), new PropertyMetadata());
	}
}
