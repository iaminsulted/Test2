using System;

namespace Windows.Kinect
{
	// Token: 0x0200006D RID: 109
	[Flags]
	public enum FrameSourceTypes : uint
	{
		// Token: 0x04000283 RID: 643
		None = 0U,
		// Token: 0x04000284 RID: 644
		Color = 1U,
		// Token: 0x04000285 RID: 645
		Infrared = 2U,
		// Token: 0x04000286 RID: 646
		LongExposureInfrared = 4U,
		// Token: 0x04000287 RID: 647
		Depth = 8U,
		// Token: 0x04000288 RID: 648
		BodyIndex = 16U,
		// Token: 0x04000289 RID: 649
		Body = 32U,
		// Token: 0x0400028A RID: 650
		Audio = 64U
	}
}
