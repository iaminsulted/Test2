using System;

namespace Windows.Kinect
{
	// Token: 0x02000078 RID: 120
	[Flags]
	public enum KinectCapabilities : uint
	{
		// Token: 0x040002C2 RID: 706
		None = 0U,
		// Token: 0x040002C3 RID: 707
		Vision = 1U,
		// Token: 0x040002C4 RID: 708
		Audio = 2U,
		// Token: 0x040002C5 RID: 709
		Face = 4U,
		// Token: 0x040002C6 RID: 710
		Expressions = 8U,
		// Token: 0x040002C7 RID: 711
		Gamechat = 16U
	}
}
