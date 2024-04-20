using System;
using System.IO;
using System.Windows.Media;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x0200050C RID: 1292
	internal class XamlPointCollectionSerializer : XamlSerializer
	{
		// Token: 0x0600402D RID: 16429 RVA: 0x00212FFB File Offset: 0x00211FFB
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializePoint(writer, stringValue);
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x00213004 File Offset: 0x00212004
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return PointCollection.DeserializeFrom(reader);
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x0021300C File Offset: 0x0021200C
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return PointCollection.DeserializeFrom(reader);
		}
	}
}
