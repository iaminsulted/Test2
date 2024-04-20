using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Windows.Markup;
using System.Xml;
using MS.Internal;
using MS.Internal.IO.Packaging;
using MS.Internal.IO.Packaging.Extensions;

namespace System.Windows.Documents
{
	// Token: 0x020006E6 RID: 1766
	internal class XpsValidatingLoader
	{
		// Token: 0x06005CE5 RID: 23781 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal XpsValidatingLoader()
		{
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x0028B5C6 File Offset: 0x0028A5C6
		internal object Load(Stream stream, Uri parentUri, ParserContext pc, ContentType mimeType)
		{
			return this.Load(stream, parentUri, pc, mimeType, null);
		}

		// Token: 0x06005CE7 RID: 23783 RVA: 0x0028B5D4 File Offset: 0x0028A5D4
		internal void Validate(Stream stream, Uri parentUri, ParserContext pc, ContentType mimeType, string rootElement)
		{
			this.Load(stream, parentUri, pc, mimeType, rootElement);
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x0028B5E4 File Offset: 0x0028A5E4
		private object Load(Stream stream, Uri parentUri, ParserContext pc, ContentType mimeType, string rootElement)
		{
			object result = null;
			List<Type> safeTypes = new List<Type>
			{
				typeof(ResourceDictionary)
			};
			if (!XpsValidatingLoader.DocumentMode)
			{
				if (rootElement == null)
				{
					result = XamlReader.Load(XmlReader.Create(stream, null, pc), pc, XamlParseMode.Synchronous, true, safeTypes);
					stream.Close();
				}
			}
			else
			{
				XpsSchema schema = XpsSchema.GetSchema(mimeType);
				Uri baseUri = pc.BaseUri;
				Uri packageUri = PackUriHelper.GetPackageUri(baseUri);
				Uri partUri = PackUriHelper.GetPartUri(baseUri);
				Package package = PreloadedPackages.GetPackage(packageUri);
				if (parentUri != null && !PackUriHelper.GetPackageUri(parentUri).Equals(packageUri))
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderUriNotInSamePackage"));
				}
				schema.ValidateRelationships(new SecurityCriticalData<Package>(package), packageUri, partUri, mimeType);
				if (schema.AllowsMultipleReferencesToSameUri(mimeType))
				{
					this._uniqueUriRef = null;
				}
				else
				{
					this._uniqueUriRef = new Hashtable(11);
				}
				Hashtable hashtable = (XpsValidatingLoader._validResources.Count > 0) ? XpsValidatingLoader._validResources.Peek() : null;
				if (schema.HasRequiredResources(mimeType))
				{
					hashtable = new Hashtable(11);
					foreach (PackageRelationship packageRelationship in package.GetPart(partUri).GetRelationshipsByType(XpsValidatingLoader._requiredResourceRel))
					{
						Uri partUri2 = PackUriHelper.ResolvePartUri(partUri, packageRelationship.TargetUri);
						Uri key = PackUriHelper.Create(packageUri, partUri2);
						PackagePart part = package.GetPart(partUri2);
						if (schema.IsValidRequiredResourceMimeType(part.ValidatedContentType()))
						{
							if (!hashtable.ContainsKey(key))
							{
								hashtable.Add(key, true);
							}
						}
						else if (!hashtable.ContainsKey(key))
						{
							hashtable.Add(key, false);
						}
					}
				}
				XpsSchemaValidator xpsSchemaValidator = new XpsSchemaValidator(this, schema, mimeType, stream, packageUri, partUri);
				XpsValidatingLoader._validResources.Push(hashtable);
				if (rootElement != null)
				{
					xpsSchemaValidator.XmlReader.MoveToContent();
					if (!rootElement.Equals(xpsSchemaValidator.XmlReader.Name))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
					}
					while (xpsSchemaValidator.XmlReader.Read())
					{
					}
				}
				else
				{
					result = XamlReader.Load(xpsSchemaValidator.XmlReader, pc, XamlParseMode.Synchronous, true, safeTypes);
				}
				XpsValidatingLoader._validResources.Pop();
			}
			return result;
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x06005CE9 RID: 23785 RVA: 0x0028B81C File Offset: 0x0028A81C
		internal static bool DocumentMode
		{
			get
			{
				return XpsValidatingLoader._documentMode;
			}
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x0028B823 File Offset: 0x0028A823
		internal static void AssertDocumentMode()
		{
			XpsValidatingLoader._documentMode = true;
		}

		// Token: 0x06005CEB RID: 23787 RVA: 0x0028B82C File Offset: 0x0028A82C
		internal void UriHitHandler(int node, Uri uri)
		{
			if (this._uniqueUriRef != null)
			{
				if (this._uniqueUriRef.Contains(uri))
				{
					if ((int)this._uniqueUriRef[uri] != node)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderDuplicateReference"));
					}
				}
				else
				{
					this._uniqueUriRef.Add(uri, node);
				}
			}
			Hashtable hashtable = XpsValidatingLoader._validResources.Peek();
			if (hashtable != null)
			{
				if (!hashtable.ContainsKey(uri))
				{
					bool flag = false;
					foreach (object obj in hashtable.Keys)
					{
						Uri uri2 = (Uri)obj;
						if (PackUriHelper.ComparePackUri(uri2, uri) == 0)
						{
							hashtable.Add(uri, hashtable[uri2]);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderUnlistedResource"));
					}
				}
				if (!(bool)hashtable[uri])
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
				}
			}
		}

		// Token: 0x04003133 RID: 12595
		private static Stack<Hashtable> _validResources = new Stack<Hashtable>();

		// Token: 0x04003134 RID: 12596
		private Hashtable _uniqueUriRef;

		// Token: 0x04003135 RID: 12597
		private static bool _documentMode = false;

		// Token: 0x04003136 RID: 12598
		private static string _requiredResourceRel = "http://schemas.microsoft.com/xps/2005/06/required-resource";

		// Token: 0x04003137 RID: 12599
		private static XpsS0FixedPageSchema xpsS0FixedPageSchema = new XpsS0FixedPageSchema();

		// Token: 0x04003138 RID: 12600
		private static XpsS0ResourceDictionarySchema xpsS0ResourceDictionarySchema = new XpsS0ResourceDictionarySchema();

		// Token: 0x04003139 RID: 12601
		private static XpsDocStructSchema xpsDocStructSchema = new XpsDocStructSchema();
	}
}
