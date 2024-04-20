using System;
using System.Text;

namespace MS.Internal.Text
{
	// Token: 0x0200031D RID: 797
	internal static class InternalEncoding
	{
		// Token: 0x06001D8C RID: 7564 RVA: 0x0016D37F File Offset: 0x0016C37F
		static InternalEncoding()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x0016D38B File Offset: 0x0016C38B
		internal static Encoding GetEncoding(int codepage)
		{
			return Encoding.GetEncoding(codepage);
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x0016D393 File Offset: 0x0016C393
		internal static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes)
		{
			return Encoding.Convert(srcEncoding, dstEncoding, bytes);
		}
	}
}
