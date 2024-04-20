using System;

namespace System.Windows.Markup
{
	// Token: 0x0200047D RID: 1149
	internal enum ReaderFlags : ushort
	{
		// Token: 0x04001E17 RID: 7703
		Unknown,
		// Token: 0x04001E18 RID: 7704
		DependencyObject = 4096,
		// Token: 0x04001E19 RID: 7705
		ClrObject = 8192,
		// Token: 0x04001E1A RID: 7706
		PropertyComplexClr = 12288,
		// Token: 0x04001E1B RID: 7707
		PropertyComplexDP = 16384,
		// Token: 0x04001E1C RID: 7708
		PropertyArray = 20480,
		// Token: 0x04001E1D RID: 7709
		PropertyIList = 24576,
		// Token: 0x04001E1E RID: 7710
		PropertyIDictionary = 28672,
		// Token: 0x04001E1F RID: 7711
		PropertyIAddChild = 32768,
		// Token: 0x04001E20 RID: 7712
		RealizeDeferContent = 36864,
		// Token: 0x04001E21 RID: 7713
		ConstructorParams = 40960,
		// Token: 0x04001E22 RID: 7714
		ContextTypeMask = 61440,
		// Token: 0x04001E23 RID: 7715
		StyleObject = 256,
		// Token: 0x04001E24 RID: 7716
		FrameworkTemplateObject = 512,
		// Token: 0x04001E25 RID: 7717
		TableTemplateObject = 1024,
		// Token: 0x04001E26 RID: 7718
		SingletonConstructorParam = 2048,
		// Token: 0x04001E27 RID: 7719
		NeedToAddToTree = 1,
		// Token: 0x04001E28 RID: 7720
		AddedToTree,
		// Token: 0x04001E29 RID: 7721
		InjectedElement = 4,
		// Token: 0x04001E2A RID: 7722
		CollectionHolder = 8,
		// Token: 0x04001E2B RID: 7723
		IDictionary = 16,
		// Token: 0x04001E2C RID: 7724
		IList = 32,
		// Token: 0x04001E2D RID: 7725
		ArrayExt = 64,
		// Token: 0x04001E2E RID: 7726
		IAddChild = 128
	}
}
