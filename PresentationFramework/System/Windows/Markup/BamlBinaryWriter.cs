using System;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x02000475 RID: 1141
	internal class BamlBinaryWriter : BinaryWriter
	{
		// Token: 0x06003A86 RID: 14982 RVA: 0x001F0E45 File Offset: 0x001EFE45
		public BamlBinaryWriter(Stream stream, Encoding code) : base(stream, code)
		{
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x001F0E4F File Offset: 0x001EFE4F
		public new void Write7BitEncodedInt(int value)
		{
			base.Write7BitEncodedInt(value);
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001F0E58 File Offset: 0x001EFE58
		public static int SizeOf7bitEncodedSize(int size)
		{
			if ((size & -128) == 0)
			{
				return 1;
			}
			if ((size & -16384) == 0)
			{
				return 2;
			}
			if ((size & -2097152) == 0)
			{
				return 3;
			}
			if ((size & -268435456) == 0)
			{
				return 4;
			}
			return 5;
		}
	}
}
