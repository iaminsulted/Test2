using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000605 RID: 1541
	internal sealed class XpsDocStructSchema : XpsSchema
	{
		// Token: 0x06004B20 RID: 19232 RVA: 0x00235F24 File Offset: 0x00234F24
		public XpsDocStructSchema()
		{
			XpsSchema.RegisterSchema(this, new ContentType[]
			{
				XpsDocStructSchema._documentStructureContentType,
				XpsDocStructSchema._storyFragmentsContentType
			});
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x00235F48 File Offset: 0x00234F48
		public override XmlReaderSettings GetXmlReaderSettings()
		{
			if (XpsDocStructSchema._xmlReaderSettings == null)
			{
				XpsDocStructSchema._xmlReaderSettings = new XmlReaderSettings();
				XpsDocStructSchema._xmlReaderSettings.ValidationFlags = (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints);
				MemoryStream input = new MemoryStream(XpsDocStructSchema.SchemaBytes);
				XmlResolver xmlResolver = new XmlUrlResolver();
				XpsDocStructSchema._xmlReaderSettings.ValidationType = ValidationType.Schema;
				XpsDocStructSchema._xmlReaderSettings.Schemas.XmlResolver = xmlResolver;
				XpsDocStructSchema._xmlReaderSettings.Schemas.Add("http://schemas.microsoft.com/xps/2005/06/documentstructure", new XmlTextReader(input));
			}
			return XpsDocStructSchema._xmlReaderSettings;
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x00235FBE File Offset: 0x00234FBE
		public override bool IsValidRootNamespaceUri(string namespaceUri)
		{
			return namespaceUri.Equals("http://schemas.microsoft.com/xps/2005/06/documentstructure", StringComparison.Ordinal);
		}

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x06004B23 RID: 19235 RVA: 0x00235FCC File Offset: 0x00234FCC
		public override string RootNamespaceUri
		{
			get
			{
				return "http://schemas.microsoft.com/xps/2005/06/documentstructure";
			}
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x00235FD3 File Offset: 0x00234FD3
		private static byte[] SchemaBytes
		{
			get
			{
				return (byte[])new ResourceManager("Schemas_DocStructure", Assembly.GetAssembly(typeof(XpsDocStructSchema))).GetObject("DocStructure.xsd");
			}
		}

		// Token: 0x04002766 RID: 10086
		private static ContentType _documentStructureContentType = new ContentType("application/vnd.ms-package.xps-documentstructure+xml");

		// Token: 0x04002767 RID: 10087
		private static ContentType _storyFragmentsContentType = new ContentType("application/vnd.ms-package.xps-storyfragments+xml");

		// Token: 0x04002768 RID: 10088
		private const string _xpsDocStructureSchemaNamespace = "http://schemas.microsoft.com/xps/2005/06/documentstructure";

		// Token: 0x04002769 RID: 10089
		private static XmlReaderSettings _xmlReaderSettings;
	}
}
