using System;
using System.IO;
using System.Windows.Media.Media3D;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x0200051C RID: 1308
	internal class XamlVector3DCollectionSerializer : XamlSerializer
	{
		// Token: 0x06004131 RID: 16689 RVA: 0x002118F1 File Offset: 0x002108F1
		internal XamlVector3DCollectionSerializer()
		{
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x00217465 File Offset: 0x00216465
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializeVector3D(writer, stringValue);
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x0021746E File Offset: 0x0021646E
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Vector3DCollection.DeserializeFrom(reader);
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x00217476 File Offset: 0x00216476
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Vector3DCollection.DeserializeFrom(reader);
		}
	}
}
