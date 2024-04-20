using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000602 RID: 1538
	internal class XpsS0Schema : XpsSchema
	{
		// Token: 0x06004B11 RID: 19217 RVA: 0x00235788 File Offset: 0x00234788
		protected XpsS0Schema()
		{
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x00235790 File Offset: 0x00234790
		public override XmlReaderSettings GetXmlReaderSettings()
		{
			if (XpsS0Schema._xmlReaderSettings == null)
			{
				XpsS0Schema._xmlReaderSettings = new XmlReaderSettings();
				XpsS0Schema._xmlReaderSettings.ValidationFlags = (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints);
				MemoryStream input = new MemoryStream(XpsS0Schema.S0SchemaBytes);
				MemoryStream input2 = new MemoryStream(XpsS0Schema.DictionarySchemaBytes);
				XmlResolver xmlResolver = new XmlUrlResolver();
				XpsS0Schema._xmlReaderSettings.ValidationType = ValidationType.Schema;
				XpsS0Schema._xmlReaderSettings.Schemas.XmlResolver = xmlResolver;
				XpsS0Schema._xmlReaderSettings.Schemas.Add("http://schemas.microsoft.com/xps/2005/06", new XmlTextReader(input));
				XpsS0Schema._xmlReaderSettings.Schemas.Add(null, new XmlTextReader(input2));
			}
			return XpsS0Schema._xmlReaderSettings;
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x00235828 File Offset: 0x00234828
		public override bool HasRequiredResources(ContentType mimeType)
		{
			return XpsS0Schema._fixedPageContentType.AreTypeAndSubTypeEqual(mimeType);
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool HasUriAttributes(ContentType mimeType)
		{
			return true;
		}

		// Token: 0x06004B15 RID: 19221 RVA: 0x0023583A File Offset: 0x0023483A
		public override bool AllowsMultipleReferencesToSameUri(ContentType mimeType)
		{
			return !XpsS0Schema._fixedDocumentSequenceContentType.AreTypeAndSubTypeEqual(mimeType) && !XpsS0Schema._fixedDocumentContentType.AreTypeAndSubTypeEqual(mimeType);
		}

		// Token: 0x06004B16 RID: 19222 RVA: 0x00235859 File Offset: 0x00234859
		public override bool IsValidRootNamespaceUri(string namespaceUri)
		{
			return namespaceUri.Equals("http://schemas.microsoft.com/xps/2005/06", StringComparison.Ordinal);
		}

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x06004B17 RID: 19223 RVA: 0x00235867 File Offset: 0x00234867
		public override string RootNamespaceUri
		{
			get
			{
				return "http://schemas.microsoft.com/xps/2005/06";
			}
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x00235870 File Offset: 0x00234870
		public override string[] ExtractUriFromAttr(string attrName, string attrValue)
		{
			if (attrName.Equals("Source", StringComparison.Ordinal) || attrName.Equals("FontUri", StringComparison.Ordinal))
			{
				return new string[]
				{
					attrValue
				};
			}
			if (!attrName.Equals("ImageSource", StringComparison.Ordinal))
			{
				if (attrName.Equals("Color", StringComparison.Ordinal) || attrName.Equals("Fill", StringComparison.Ordinal) || attrName.Equals("Stroke", StringComparison.Ordinal))
				{
					attrValue = attrValue.Trim();
					if (attrValue.StartsWith("ContextColor ", StringComparison.Ordinal))
					{
						attrValue = attrValue.Substring("ContextColor ".Length);
						attrValue = attrValue.Trim();
						string[] array = attrValue.Split(new char[]
						{
							' '
						});
						if (array.GetLength(0) >= 1)
						{
							return new string[]
							{
								array[0]
							};
						}
					}
				}
				return null;
			}
			if (attrValue.StartsWith("{ColorConvertedBitmap ", StringComparison.Ordinal))
			{
				attrValue = attrValue.Substring("{ColorConvertedBitmap ".Length);
				return attrValue.Split(new char[]
				{
					' ',
					'}'
				});
			}
			return new string[]
			{
				attrValue
			};
		}

		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x00235978 File Offset: 0x00234978
		private static byte[] S0SchemaBytes
		{
			get
			{
				return (byte[])new ResourceManager("Schemas_S0", Assembly.GetAssembly(typeof(XpsS0Schema))).GetObject("s0schema.xsd");
			}
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x06004B1A RID: 19226 RVA: 0x002359A2 File Offset: 0x002349A2
		private static byte[] DictionarySchemaBytes
		{
			get
			{
				return (byte[])new ResourceManager("Schemas_S0", Assembly.GetAssembly(typeof(XpsS0Schema))).GetObject("rdkey.xsd");
			}
		}

		// Token: 0x04002751 RID: 10065
		protected static ContentType _fontContentType = new ContentType("application/vnd.ms-opentype");

		// Token: 0x04002752 RID: 10066
		protected static ContentType _colorContextContentType = new ContentType("application/vnd.ms-color.iccprofile");

		// Token: 0x04002753 RID: 10067
		protected static ContentType _obfuscatedContentType = new ContentType("application/vnd.ms-package.obfuscated-opentype");

		// Token: 0x04002754 RID: 10068
		protected static ContentType _jpgContentType = new ContentType("image/jpeg");

		// Token: 0x04002755 RID: 10069
		protected static ContentType _pngContentType = new ContentType("image/png");

		// Token: 0x04002756 RID: 10070
		protected static ContentType _tifContentType = new ContentType("image/tiff");

		// Token: 0x04002757 RID: 10071
		protected static ContentType _wmpContentType = new ContentType("image/vnd.ms-photo");

		// Token: 0x04002758 RID: 10072
		protected static ContentType _fixedDocumentSequenceContentType = new ContentType("application/vnd.ms-package.xps-fixeddocumentsequence+xml");

		// Token: 0x04002759 RID: 10073
		protected static ContentType _fixedDocumentContentType = new ContentType("application/vnd.ms-package.xps-fixeddocument+xml");

		// Token: 0x0400275A RID: 10074
		protected static ContentType _fixedPageContentType = new ContentType("application/vnd.ms-package.xps-fixedpage+xml");

		// Token: 0x0400275B RID: 10075
		protected static ContentType _resourceDictionaryContentType = new ContentType("application/vnd.ms-package.xps-resourcedictionary+xml");

		// Token: 0x0400275C RID: 10076
		protected static ContentType _printTicketContentType = new ContentType("application/vnd.ms-printing.printticket+xml");

		// Token: 0x0400275D RID: 10077
		protected static ContentType _discardControlContentType = new ContentType("application/vnd.ms-package.xps-discard-control+xml");

		// Token: 0x0400275E RID: 10078
		private const string _xpsS0SchemaNamespace = "http://schemas.microsoft.com/xps/2005/06";

		// Token: 0x0400275F RID: 10079
		private const string _contextColor = "ContextColor ";

		// Token: 0x04002760 RID: 10080
		private const string _colorConvertedBitmap = "{ColorConvertedBitmap ";

		// Token: 0x04002761 RID: 10081
		private static XmlReaderSettings _xmlReaderSettings;
	}
}
