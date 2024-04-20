using System;

namespace System.Windows
{
	// Token: 0x0200036D RID: 877
	[Flags]
	internal enum InternalFlags2 : uint
	{
		// Token: 0x04001094 RID: 4244
		R0 = 1U,
		// Token: 0x04001095 RID: 4245
		R1 = 2U,
		// Token: 0x04001096 RID: 4246
		R2 = 4U,
		// Token: 0x04001097 RID: 4247
		R3 = 8U,
		// Token: 0x04001098 RID: 4248
		R4 = 16U,
		// Token: 0x04001099 RID: 4249
		R5 = 32U,
		// Token: 0x0400109A RID: 4250
		R6 = 64U,
		// Token: 0x0400109B RID: 4251
		R7 = 128U,
		// Token: 0x0400109C RID: 4252
		R8 = 256U,
		// Token: 0x0400109D RID: 4253
		R9 = 512U,
		// Token: 0x0400109E RID: 4254
		RA = 1024U,
		// Token: 0x0400109F RID: 4255
		RB = 2048U,
		// Token: 0x040010A0 RID: 4256
		RC = 4096U,
		// Token: 0x040010A1 RID: 4257
		RD = 8192U,
		// Token: 0x040010A2 RID: 4258
		RE = 16384U,
		// Token: 0x040010A3 RID: 4259
		RF = 32768U,
		// Token: 0x040010A4 RID: 4260
		TreeHasLoadedChangeHandler = 1048576U,
		// Token: 0x040010A5 RID: 4261
		IsLoadedCache = 2097152U,
		// Token: 0x040010A6 RID: 4262
		IsStyleSetFromGenerator = 4194304U,
		// Token: 0x040010A7 RID: 4263
		IsParentAnFE = 8388608U,
		// Token: 0x040010A8 RID: 4264
		IsTemplatedParentAnFE = 16777216U,
		// Token: 0x040010A9 RID: 4265
		HasStyleChanged = 33554432U,
		// Token: 0x040010AA RID: 4266
		HasTemplateChanged = 67108864U,
		// Token: 0x040010AB RID: 4267
		HasStyleInvalidated = 134217728U,
		// Token: 0x040010AC RID: 4268
		IsRequestingExpression = 268435456U,
		// Token: 0x040010AD RID: 4269
		HasMultipleInheritanceContexts = 536870912U,
		// Token: 0x040010AE RID: 4270
		BypassLayoutPolicies = 2147483648U,
		// Token: 0x040010AF RID: 4271
		Default = 65535U
	}
}
