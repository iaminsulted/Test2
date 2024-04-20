using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020002BB RID: 699
	internal struct AnnotationXmlConstants
	{
		// Token: 0x02000A11 RID: 2577
		internal struct Namespaces
		{
			// Token: 0x0400407D RID: 16509
			public const string CoreSchemaNamespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core";

			// Token: 0x0400407E RID: 16510
			public const string BaseSchemaNamespace = "http://schemas.microsoft.com/windows/annotations/2003/11/base";
		}

		// Token: 0x02000A12 RID: 2578
		internal struct Prefixes
		{
			// Token: 0x0400407F RID: 16511
			internal const string XmlPrefix = "xml";

			// Token: 0x04004080 RID: 16512
			internal const string XmlnsPrefix = "xmlns";

			// Token: 0x04004081 RID: 16513
			internal const string CoreSchemaPrefix = "anc";

			// Token: 0x04004082 RID: 16514
			internal const string BaseSchemaPrefix = "anb";
		}

		// Token: 0x02000A13 RID: 2579
		internal struct Elements
		{
			// Token: 0x04004083 RID: 16515
			internal const string Annotation = "Annotation";

			// Token: 0x04004084 RID: 16516
			internal const string Resource = "Resource";

			// Token: 0x04004085 RID: 16517
			internal const string ContentLocator = "ContentLocator";

			// Token: 0x04004086 RID: 16518
			internal const string ContentLocatorGroup = "ContentLocatorGroup";

			// Token: 0x04004087 RID: 16519
			internal const string AuthorCollection = "Authors";

			// Token: 0x04004088 RID: 16520
			internal const string AnchorCollection = "Anchors";

			// Token: 0x04004089 RID: 16521
			internal const string CargoCollection = "Cargos";

			// Token: 0x0400408A RID: 16522
			internal const string Item = "Item";

			// Token: 0x0400408B RID: 16523
			internal const string StringAuthor = "StringAuthor";
		}

		// Token: 0x02000A14 RID: 2580
		internal struct Attributes
		{
			// Token: 0x0400408C RID: 16524
			internal const string Id = "Id";

			// Token: 0x0400408D RID: 16525
			internal const string CreationTime = "CreationTime";

			// Token: 0x0400408E RID: 16526
			internal const string LastModificationTime = "LastModificationTime";

			// Token: 0x0400408F RID: 16527
			internal const string TypeName = "Type";

			// Token: 0x04004090 RID: 16528
			internal const string ResourceName = "Name";

			// Token: 0x04004091 RID: 16529
			internal const string ItemName = "Name";

			// Token: 0x04004092 RID: 16530
			internal const string ItemValue = "Value";
		}
	}
}
