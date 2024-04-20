using System;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x02000474 RID: 1140
	internal class BamlBinaryReader : BinaryReader
	{
		// Token: 0x06003A84 RID: 14980 RVA: 0x001F0E3B File Offset: 0x001EFE3B
		public BamlBinaryReader(Stream stream, Encoding code) : base(stream, code)
		{
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x001AB549 File Offset: 0x001AA549
		public new int Read7BitEncodedInt()
		{
			return base.Read7BitEncodedInt();
		}
	}
}
