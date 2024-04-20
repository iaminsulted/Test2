using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000167 RID: 359
	internal interface IManagedFilter
	{
		// Token: 0x06000BFD RID: 3069
		IFILTER_FLAGS Init(IFILTER_INIT grfFlags, ManagedFullPropSpec[] aAttributes);

		// Token: 0x06000BFE RID: 3070
		ManagedChunk GetChunk();

		// Token: 0x06000BFF RID: 3071
		string GetText(int bufferCharacterCount);

		// Token: 0x06000C00 RID: 3072
		object GetValue();
	}
}
