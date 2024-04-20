using System;
using System.IO;
using System.IO.Packaging;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000603 RID: 1539
	internal sealed class XpsS0FixedPageSchema : XpsS0Schema
	{
		// Token: 0x06004B1C RID: 19228 RVA: 0x00235A9C File Offset: 0x00234A9C
		public XpsS0FixedPageSchema()
		{
			XpsSchema.RegisterSchema(this, new ContentType[]
			{
				XpsS0Schema._fixedDocumentSequenceContentType,
				XpsS0Schema._fixedDocumentContentType,
				XpsS0Schema._fixedPageContentType
			});
			base.RegisterRequiredResourceMimeTypes(new ContentType[]
			{
				XpsS0Schema._resourceDictionaryContentType,
				XpsS0Schema._fontContentType,
				XpsS0Schema._colorContextContentType,
				XpsS0Schema._obfuscatedContentType,
				XpsS0Schema._jpgContentType,
				XpsS0Schema._pngContentType,
				XpsS0Schema._tifContentType,
				XpsS0Schema._wmpContentType
			});
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x00235B20 File Offset: 0x00234B20
		public override void ValidateRelationships(SecurityCriticalData<Package> package, Uri packageUri, Uri partUri, ContentType mimeType)
		{
			PackagePart part = package.Value.GetPart(partUri);
			PackageRelationshipCollection relationshipsByType = part.GetRelationshipsByType("http://schemas.microsoft.com/xps/2005/06/printticket");
			int num = 0;
			foreach (PackageRelationship packageRelationship in relationshipsByType)
			{
				num++;
				if (num > 1)
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOnePrintTicketPart"));
				}
				Uri partUri2 = PackUriHelper.ResolvePartUri(partUri, packageRelationship.TargetUri);
				PackUriHelper.Create(packageUri, partUri2);
				PackagePart part2 = package.Value.GetPart(partUri2);
				if (!XpsS0Schema._printTicketContentType.AreTypeAndSubTypeEqual(new ContentType(part2.ContentType)))
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderPrintTicketHasIncorrectType"));
				}
			}
			PackageRelationshipCollection relationshipsByType2 = part.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
			num = 0;
			foreach (PackageRelationship packageRelationship2 in relationshipsByType2)
			{
				num++;
				if (num > 1)
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOneThumbnailPart"));
				}
				Uri partUri3 = PackUriHelper.ResolvePartUri(partUri, packageRelationship2.TargetUri);
				PackUriHelper.Create(packageUri, partUri3);
				PackagePart part3 = package.Value.GetPart(partUri3);
				if (!XpsS0Schema._jpgContentType.AreTypeAndSubTypeEqual(new ContentType(part3.ContentType)) && !XpsS0Schema._pngContentType.AreTypeAndSubTypeEqual(new ContentType(part3.ContentType)))
				{
					throw new FileFormatException(SR.Get("XpsValidatingLoaderThumbnailHasIncorrectType"));
				}
			}
			if (XpsS0Schema._fixedDocumentContentType.AreTypeAndSubTypeEqual(mimeType))
			{
				foreach (PackageRelationship packageRelationship3 in part.GetRelationshipsByType("http://schemas.microsoft.com/xps/2005/06/restricted-font"))
				{
					Uri partUri4 = PackUriHelper.ResolvePartUri(partUri, packageRelationship3.TargetUri);
					PackUriHelper.Create(packageUri, partUri4);
					PackagePart part4 = package.Value.GetPart(partUri4);
					if (!XpsS0Schema._fontContentType.AreTypeAndSubTypeEqual(new ContentType(part4.ContentType)) && !XpsS0Schema._obfuscatedContentType.AreTypeAndSubTypeEqual(new ContentType(part4.ContentType)))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderRestrictedFontHasIncorrectType"));
					}
				}
			}
			if (XpsS0Schema._fixedDocumentSequenceContentType.AreTypeAndSubTypeEqual(mimeType))
			{
				PackageRelationshipCollection relationshipsByType3 = package.Value.GetRelationshipsByType("http://schemas.microsoft.com/xps/2005/06/discard-control");
				num = 0;
				foreach (PackageRelationship packageRelationship4 in relationshipsByType3)
				{
					num++;
					if (num > 1)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOneDiscardControlInPackage"));
					}
					Uri partUri5 = PackUriHelper.ResolvePartUri(partUri, packageRelationship4.TargetUri);
					PackUriHelper.Create(packageUri, partUri5);
					PackagePart part5 = package.Value.GetPart(partUri5);
					if (!XpsS0Schema._discardControlContentType.AreTypeAndSubTypeEqual(new ContentType(part5.ContentType)))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderDiscardControlHasIncorrectType"));
					}
				}
				PackageRelationshipCollection relationshipsByType4 = package.Value.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
				num = 0;
				foreach (PackageRelationship packageRelationship5 in relationshipsByType4)
				{
					num++;
					if (num > 1)
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderMoreThanOneThumbnailInPackage"));
					}
					Uri partUri6 = PackUriHelper.ResolvePartUri(partUri, packageRelationship5.TargetUri);
					PackUriHelper.Create(packageUri, partUri6);
					PackagePart part6 = package.Value.GetPart(partUri6);
					if (!XpsS0Schema._jpgContentType.AreTypeAndSubTypeEqual(new ContentType(part6.ContentType)) && !XpsS0Schema._pngContentType.AreTypeAndSubTypeEqual(new ContentType(part6.ContentType)))
					{
						throw new FileFormatException(SR.Get("XpsValidatingLoaderThumbnailHasIncorrectType"));
					}
				}
			}
		}

		// Token: 0x04002762 RID: 10082
		private const string _printTicketRel = "http://schemas.microsoft.com/xps/2005/06/printticket";

		// Token: 0x04002763 RID: 10083
		private const string _discardControlRel = "http://schemas.microsoft.com/xps/2005/06/discard-control";

		// Token: 0x04002764 RID: 10084
		private const string _restrictedFontRel = "http://schemas.microsoft.com/xps/2005/06/restricted-font";

		// Token: 0x04002765 RID: 10085
		private const string _thumbnailRel = "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail";
	}
}
