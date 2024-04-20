using System;
using System.IO;
using System.Windows.Media.Media3D;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x0200050B RID: 1291
	internal class XamlPoint3DCollectionSerializer : XamlSerializer
	{
		// Token: 0x06004029 RID: 16425 RVA: 0x00212FE2 File Offset: 0x00211FE2
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializePoint3D(writer, stringValue);
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00212FEB File Offset: 0x00211FEB
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Point3DCollection.DeserializeFrom(reader);
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x00212FF3 File Offset: 0x00211FF3
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Point3DCollection.DeserializeFrom(reader);
		}
	}
}
