using System;
using System.IO;

namespace System.Windows.Baml2006
{
	// Token: 0x0200040F RID: 1039
	internal class BamlBinaryReader : BinaryReader
	{
		// Token: 0x06002D31 RID: 11569 RVA: 0x001AB540 File Offset: 0x001AA540
		public BamlBinaryReader(Stream stream) : base(stream)
		{
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x001AB549 File Offset: 0x001AA549
		public new int Read7BitEncodedInt()
		{
			return base.Read7BitEncodedInt();
		}
	}
}
