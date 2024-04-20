using System;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Windows.Markup;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000600 RID: 1536
	internal class XpsSchemaValidator
	{
		// Token: 0x06004B00 RID: 19200 RVA: 0x00235580 File Offset: 0x00234580
		public XpsSchemaValidator(XpsValidatingLoader loader, XpsSchema schema, ContentType mimeType, Stream objectStream, Uri packageUri, Uri baseUri)
		{
			XmlReader xmlReader = new XpsSchemaValidator.XmlEncodingEnforcingTextReader(objectStream)
			{
				ProhibitDtd = true,
				Normalization = true
			};
			string[] array = XpsSchemaValidator._predefinedNamespaces;
			if (!string.IsNullOrEmpty(schema.RootNamespaceUri))
			{
				array = new string[XpsSchemaValidator._predefinedNamespaces.Length + 1];
				array[0] = schema.RootNamespaceUri;
				XpsSchemaValidator._predefinedNamespaces.CopyTo(array, 1);
			}
			xmlReader = new XmlCompatibilityReader(xmlReader, array);
			xmlReader = XmlReader.Create(xmlReader, schema.GetXmlReaderSettings());
			if (schema.HasUriAttributes(mimeType) && packageUri != null && baseUri != null)
			{
				xmlReader = new XpsSchemaValidator.RootXMLNSAndUriValidatingXmlReader(loader, schema, xmlReader, packageUri, baseUri);
			}
			else
			{
				xmlReader = new XpsSchemaValidator.RootXMLNSAndUriValidatingXmlReader(loader, schema, xmlReader);
			}
			this._compatReader = xmlReader;
		}

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x06004B01 RID: 19201 RVA: 0x00235631 File Offset: 0x00234631
		public XmlReader XmlReader
		{
			get
			{
				return this._compatReader;
			}
		}

		// Token: 0x0400274D RID: 10061
		private XmlReader _compatReader;

		// Token: 0x0400274E RID: 10062
		private static string[] _predefinedNamespaces = new string[]
		{
			"http://schemas.microsoft.com/xps/2005/06/resourcedictionary-key"
		};

		// Token: 0x02000B35 RID: 2869
		private class XmlEncodingEnforcingTextReader : XmlTextReader
		{
			// Token: 0x06008CA1 RID: 36001 RVA: 0x0033D3C2 File Offset: 0x0033C3C2
			public XmlEncodingEnforcingTextReader(Stream objectStream) : base(objectStream)
			{
			}

			// Token: 0x06008CA2 RID: 36002 RVA: 0x0033D3CC File Offset: 0x0033C3CC
			public override bool Read()
			{
				bool flag = base.Read();
				if (flag && !this._encodingChecked)
				{
					if (base.NodeType == XmlNodeType.XmlDeclaration)
					{
						string text = base["encoding"];
						if (text != null && !text.Equals(Encoding.Unicode.WebName, StringComparison.OrdinalIgnoreCase) && !text.Equals(Encoding.UTF8.WebName, StringComparison.OrdinalIgnoreCase))
						{
							throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedEncoding"));
						}
					}
					if (!(base.Encoding is UTF8Encoding) && !(base.Encoding is UnicodeEncoding))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedEncoding"));
					}
					this._encodingChecked = true;
				}
				return flag;
			}

			// Token: 0x0400481A RID: 18458
			private bool _encodingChecked;
		}

		// Token: 0x02000B36 RID: 2870
		private class RootXMLNSAndUriValidatingXmlReader : XmlWrappingReader
		{
			// Token: 0x06008CA3 RID: 36003 RVA: 0x0033D470 File Offset: 0x0033C470
			public RootXMLNSAndUriValidatingXmlReader(XpsValidatingLoader loader, XpsSchema schema, XmlReader xmlReader, Uri packageUri, Uri baseUri) : base(xmlReader)
			{
				this._loader = loader;
				this._schema = schema;
				this._packageUri = packageUri;
				this._baseUri = baseUri;
			}

			// Token: 0x06008CA4 RID: 36004 RVA: 0x0033D497 File Offset: 0x0033C497
			public RootXMLNSAndUriValidatingXmlReader(XpsValidatingLoader loader, XpsSchema schema, XmlReader xmlReader) : base(xmlReader)
			{
				this._loader = loader;
				this._schema = schema;
			}

			// Token: 0x06008CA5 RID: 36005 RVA: 0x0033D4AE File Offset: 0x0033C4AE
			private void CheckUri(string attr)
			{
				this.CheckUri(base.Reader.LocalName, attr);
			}

			// Token: 0x06008CA6 RID: 36006 RVA: 0x0033D4C4 File Offset: 0x0033C4C4
			private void CheckUri(string localName, string attr)
			{
				if (attr != this._lastAttr)
				{
					this._lastAttr = attr;
					string[] array = this._schema.ExtractUriFromAttr(localName, attr);
					if (array != null)
					{
						foreach (string text in array)
						{
							if (text.Length > 0)
							{
								Uri partUri = PackUriHelper.ResolvePartUri(this._baseUri, new Uri(text, UriKind.Relative));
								Uri uri = PackUriHelper.Create(this._packageUri, partUri);
								this._loader.UriHitHandler(this._node, uri);
							}
						}
					}
				}
			}

			// Token: 0x17001ECE RID: 7886
			// (get) Token: 0x06008CA7 RID: 36007 RVA: 0x0033D545 File Offset: 0x0033C545
			public override string Value
			{
				get
				{
					this.CheckUri(base.Reader.Value);
					return base.Reader.Value;
				}
			}

			// Token: 0x06008CA8 RID: 36008 RVA: 0x0033D564 File Offset: 0x0033C564
			public override string GetAttribute(string name)
			{
				string attribute = base.Reader.GetAttribute(name);
				this.CheckUri(name, attribute);
				return attribute;
			}

			// Token: 0x06008CA9 RID: 36009 RVA: 0x0033D588 File Offset: 0x0033C588
			public override string GetAttribute(string name, string namespaceURI)
			{
				string attribute = base.Reader.GetAttribute(name, namespaceURI);
				this.CheckUri(attribute);
				return attribute;
			}

			// Token: 0x06008CAA RID: 36010 RVA: 0x0033D5AC File Offset: 0x0033C5AC
			public override string GetAttribute(int i)
			{
				string attribute = base.Reader.GetAttribute(i);
				this.CheckUri(attribute);
				return attribute;
			}

			// Token: 0x06008CAB RID: 36011 RVA: 0x0033D5D0 File Offset: 0x0033C5D0
			public override bool Read()
			{
				this._node++;
				bool result = base.Reader.Read();
				if (base.Reader.NodeType == XmlNodeType.Element && !this._rootXMLNSChecked)
				{
					if (!this._schema.IsValidRootNamespaceUri(base.Reader.NamespaceURI))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedRootNamespaceUri"));
					}
					this._rootXMLNSChecked = true;
				}
				return result;
			}

			// Token: 0x0400481B RID: 18459
			private XpsValidatingLoader _loader;

			// Token: 0x0400481C RID: 18460
			private XpsSchema _schema;

			// Token: 0x0400481D RID: 18461
			private Uri _packageUri;

			// Token: 0x0400481E RID: 18462
			private Uri _baseUri;

			// Token: 0x0400481F RID: 18463
			private string _lastAttr;

			// Token: 0x04004820 RID: 18464
			private int _node;

			// Token: 0x04004821 RID: 18465
			private bool _rootXMLNSChecked;
		}
	}
}
