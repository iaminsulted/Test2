using System;

namespace Windows.Kinect
{
	// Token: 0x0200006C RID: 108
	[Flags]
	public enum FrameEdges : uint
	{
		// Token: 0x0400027D RID: 637
		None = 0U,
		// Token: 0x0400027E RID: 638
		Right = 1U,
		// Token: 0x0400027F RID: 639
		Left = 2U,
		// Token: 0x04000280 RID: 640
		Top = 4U,
		// Token: 0x04000281 RID: 641
		Bottom = 8U
	}
}
