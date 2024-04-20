using System;

namespace System.Windows
{
	// Token: 0x0200036C RID: 876
	internal enum InternalFlags : uint
	{
		// Token: 0x04001078 RID: 4216
		HasResourceReferences = 1U,
		// Token: 0x04001079 RID: 4217
		HasNumberSubstitutionChanged,
		// Token: 0x0400107A RID: 4218
		HasImplicitStyleFromResources = 4U,
		// Token: 0x0400107B RID: 4219
		InheritanceBehavior0 = 8U,
		// Token: 0x0400107C RID: 4220
		InheritanceBehavior1 = 16U,
		// Token: 0x0400107D RID: 4221
		InheritanceBehavior2 = 32U,
		// Token: 0x0400107E RID: 4222
		IsStyleUpdateInProgress = 64U,
		// Token: 0x0400107F RID: 4223
		IsThemeStyleUpdateInProgress = 128U,
		// Token: 0x04001080 RID: 4224
		StoresParentTemplateValues = 256U,
		// Token: 0x04001081 RID: 4225
		NeedsClipBounds = 1024U,
		// Token: 0x04001082 RID: 4226
		HasWidthEverChanged = 2048U,
		// Token: 0x04001083 RID: 4227
		HasHeightEverChanged = 4096U,
		// Token: 0x04001084 RID: 4228
		IsInitialized = 32768U,
		// Token: 0x04001085 RID: 4229
		InitPending = 65536U,
		// Token: 0x04001086 RID: 4230
		IsResourceParentValid = 131072U,
		// Token: 0x04001087 RID: 4231
		AncestorChangeInProgress = 524288U,
		// Token: 0x04001088 RID: 4232
		InVisibilityCollapsedTree = 1048576U,
		// Token: 0x04001089 RID: 4233
		HasStyleEverBeenFetched = 2097152U,
		// Token: 0x0400108A RID: 4234
		HasThemeStyleEverBeenFetched = 4194304U,
		// Token: 0x0400108B RID: 4235
		HasLocalStyle = 8388608U,
		// Token: 0x0400108C RID: 4236
		HasTemplateGeneratedSubTree = 16777216U,
		// Token: 0x0400108D RID: 4237
		HasLogicalChildren = 67108864U,
		// Token: 0x0400108E RID: 4238
		IsLogicalChildrenIterationInProgress = 134217728U,
		// Token: 0x0400108F RID: 4239
		CreatingRoot = 268435456U,
		// Token: 0x04001090 RID: 4240
		IsRightToLeft = 536870912U,
		// Token: 0x04001091 RID: 4241
		ShouldLookupImplicitStyles = 1073741824U,
		// Token: 0x04001092 RID: 4242
		PotentiallyHasMentees = 2147483648U
	}
}
