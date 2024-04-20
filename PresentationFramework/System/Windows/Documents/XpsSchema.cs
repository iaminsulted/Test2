using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Xml.Schema;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000601 RID: 1537
	internal class XpsSchema
	{
		// Token: 0x06004B03 RID: 19203 RVA: 0x0023564E File Offset: 0x0023464E
		protected XpsSchema()
		{
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x00235664 File Offset: 0x00234664
		protected static void RegisterSchema(XpsSchema schema, ContentType[] handledMimeTypes)
		{
			foreach (ContentType key in handledMimeTypes)
			{
				XpsSchema._schemas.Add(key, schema);
			}
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x00235694 File Offset: 0x00234694
		protected void RegisterRequiredResourceMimeTypes(ContentType[] requiredResourceMimeTypes)
		{
			if (requiredResourceMimeTypes != null)
			{
				foreach (ContentType key in requiredResourceMimeTypes)
				{
					this._requiredResourceMimeTypes.Add(key, true);
				}
			}
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x002356CA File Offset: 0x002346CA
		public virtual XmlReaderSettings GetXmlReaderSettings()
		{
			return new XmlReaderSettings
			{
				ValidationFlags = (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints)
			};
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public virtual void ValidateRelationships(SecurityCriticalData<Package> package, Uri packageUri, Uri partUri, ContentType mimeType)
		{
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x00105F35 File Offset: 0x00104F35
		public virtual bool HasRequiredResources(ContentType mimeType)
		{
			return false;
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x00105F35 File Offset: 0x00104F35
		public virtual bool HasUriAttributes(ContentType mimeType)
		{
			return false;
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public virtual bool AllowsMultipleReferencesToSameUri(ContentType mimeType)
		{
			return true;
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x00105F35 File Offset: 0x00104F35
		public virtual bool IsValidRootNamespaceUri(string namespaceUri)
		{
			return false;
		}

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x06004B0C RID: 19212 RVA: 0x002356D9 File Offset: 0x002346D9
		public virtual string RootNamespaceUri
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x002356E0 File Offset: 0x002346E0
		public bool IsValidRequiredResourceMimeType(ContentType mimeType)
		{
			using (IEnumerator enumerator = this._requiredResourceMimeTypes.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((ContentType)enumerator.Current).AreTypeAndSubTypeEqual(mimeType))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x00109403 File Offset: 0x00108403
		public virtual string[] ExtractUriFromAttr(string attrName, string attrValue)
		{
			return null;
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x00235748 File Offset: 0x00234748
		public static XpsSchema GetSchema(ContentType mimeType)
		{
			XpsSchema result = null;
			if (!XpsSchema._schemas.TryGetValue(mimeType, out result))
			{
				throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
			}
			return result;
		}

		// Token: 0x0400274F RID: 10063
		private static readonly Dictionary<ContentType, XpsSchema> _schemas = new Dictionary<ContentType, XpsSchema>(new ContentType.StrongComparer());

		// Token: 0x04002750 RID: 10064
		private Hashtable _requiredResourceMimeTypes = new Hashtable(11);
	}
}
