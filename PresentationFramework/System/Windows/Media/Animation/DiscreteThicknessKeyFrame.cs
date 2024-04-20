using System;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200042E RID: 1070
	public class DiscreteThicknessKeyFrame : ThicknessKeyFrame
	{
		// Token: 0x06003401 RID: 13313 RVA: 0x001D9F78 File Offset: 0x001D8F78
		public DiscreteThicknessKeyFrame()
		{
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x001D9F80 File Offset: 0x001D8F80
		public DiscreteThicknessKeyFrame(Thickness value) : base(value)
		{
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x001D9F89 File Offset: 0x001D8F89
		public DiscreteThicknessKeyFrame(Thickness value, KeyTime keyTime) : base(value, keyTime)
		{
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x001D9F93 File Offset: 0x001D8F93
		protected override Freezable CreateInstanceCore()
		{
			return new DiscreteThicknessKeyFrame();
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x001D9F9A File Offset: 0x001D8F9A
		protected override Thickness InterpolateValueCore(Thickness baseValue, double keyFrameProgress)
		{
			if (keyFrameProgress < 1.0)
			{
				return baseValue;
			}
			return base.Value;
		}
	}
}
