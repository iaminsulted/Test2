using System;

namespace System.Windows
{
	// Token: 0x0200036F RID: 879
	[Flags]
	public enum FrameworkPropertyMetadataOptions
	{
		// Token: 0x040010C2 RID: 4290
		None = 0,
		// Token: 0x040010C3 RID: 4291
		AffectsMeasure = 1,
		// Token: 0x040010C4 RID: 4292
		AffectsArrange = 2,
		// Token: 0x040010C5 RID: 4293
		AffectsParentMeasure = 4,
		// Token: 0x040010C6 RID: 4294
		AffectsParentArrange = 8,
		// Token: 0x040010C7 RID: 4295
		AffectsRender = 16,
		// Token: 0x040010C8 RID: 4296
		Inherits = 32,
		// Token: 0x040010C9 RID: 4297
		OverridesInheritanceBehavior = 64,
		// Token: 0x040010CA RID: 4298
		NotDataBindable = 128,
		// Token: 0x040010CB RID: 4299
		BindsTwoWayByDefault = 256,
		// Token: 0x040010CC RID: 4300
		Journal = 1024,
		// Token: 0x040010CD RID: 4301
		SubPropertiesDoNotAffectRender = 2048
	}
}
