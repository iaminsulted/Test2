using System;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000432 RID: 1074
	public class SplineThicknessKeyFrame : ThicknessKeyFrame
	{
		// Token: 0x06003420 RID: 13344 RVA: 0x001D9F78 File Offset: 0x001D8F78
		public SplineThicknessKeyFrame()
		{
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x001DA1E6 File Offset: 0x001D91E6
		public SplineThicknessKeyFrame(Thickness value) : this()
		{
			base.Value = value;
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x001DA1F5 File Offset: 0x001D91F5
		public SplineThicknessKeyFrame(Thickness value, KeyTime keyTime) : this()
		{
			base.Value = value;
			base.KeyTime = keyTime;
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x001DA20B File Offset: 0x001D920B
		public SplineThicknessKeyFrame(Thickness value, KeyTime keyTime, KeySpline keySpline) : this()
		{
			if (keySpline == null)
			{
				throw new ArgumentNullException("keySpline");
			}
			base.Value = value;
			base.KeyTime = keyTime;
			this.KeySpline = keySpline;
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x001DA236 File Offset: 0x001D9236
		protected override Freezable CreateInstanceCore()
		{
			return new SplineThicknessKeyFrame();
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x001DA240 File Offset: 0x001D9240
		protected override Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress)
		{
			if (keyFrameProgress == 0.0)
			{
				return baseValue;
			}
			if (keyFrameProgress == 1.0)
			{
				return base.Value;
			}
			double splineProgress = this.KeySpline.GetSplineProgress(keyFrameProgress);
			return AnimatedTypeHelpers.InterpolateThickness(baseValue, base.Value, splineProgress);
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x001DA288 File Offset: 0x001D9288
		// (set) Token: 0x06003427 RID: 13351 RVA: 0x001DA29A File Offset: 0x001D929A
		public KeySpline KeySpline
		{
			get
			{
				return (KeySpline)base.GetValue(SplineThicknessKeyFrame.KeySplineProperty);
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.SetValue(SplineThicknessKeyFrame.KeySplineProperty, value);
			}
		}

		// Token: 0x04001C4D RID: 7245
		public static readonly DependencyProperty KeySplineProperty = DependencyProperty.Register("KeySpline", typeof(KeySpline), typeof(SplineThicknessKeyFrame), new PropertyMetadata(new KeySpline()));
	}
}
