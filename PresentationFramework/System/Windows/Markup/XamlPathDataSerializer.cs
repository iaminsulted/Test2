using System;
using System.IO;
using MS.Internal;

namespace System.Windows.Markup
{
	// Token: 0x0200050A RID: 1290
	internal class XamlPathDataSerializer : XamlSerializer
	{
		// Token: 0x06004025 RID: 16421 RVA: 0x00212FC8 File Offset: 0x00211FC8
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			Parsers.PathMinilanguageToBinary(writer, stringValue);
			return true;
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x00212FD2 File Offset: 0x00211FD2
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Parsers.DeserializeStreamGeometry(reader);
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x00212FDA File Offset: 0x00211FDA
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Parsers.DeserializeStreamGeometry(reader);
		}
	}
}
