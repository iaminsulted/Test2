using System;
using MS.Internal.PresentationFramework;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000431 RID: 1073
	public class LinearThicknessKeyFrame : ThicknessKeyFrame
	{
		// Token: 0x0600341B RID: 13339 RVA: 0x001D9F78 File Offset: 0x001D8F78
		public LinearThicknessKeyFrame()
		{
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x001D9F80 File Offset: 0x001D8F80
		public LinearThicknessKeyFrame(Thickness value) : base(value)
		{
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x001D9F89 File Offset: 0x001D8F89
		public LinearThicknessKeyFrame(Thickness value, KeyTime keyTime) : base(value, keyTime)
		{
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x001DA1AF File Offset: 0x001D91AF
		protected override Freezable CreateInstanceCore()
		{
			return new LinearThicknessKeyFrame();
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x001DA1B6 File Offset: 0x001D91B6
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
			return AnimatedTypeHelpers.InterpolateThickness(baseValue, base.Value, keyFrameProgress);
		}
	}
}
