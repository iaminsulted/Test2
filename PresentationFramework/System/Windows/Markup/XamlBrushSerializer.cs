using System;
using System.IO;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x020004DB RID: 1243
	internal class XamlBrushSerializer : XamlSerializer
	{
		// Token: 0x06003F72 RID: 16242 RVA: 0x002118F9 File Offset: 0x002108F9
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return SolidColorBrush.SerializeOn(writer, stringValue.Trim());
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x00211907 File Offset: 0x00210907
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return SolidColorBrush.DeserializeFrom(reader);
		}
	}
}
