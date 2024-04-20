using System;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;
using MS.Internal.Resources;

namespace MS.Internal.AppModel
{
	// Token: 0x0200026B RID: 619
	internal static class AppModelKnownContentFactory
	{
		// Token: 0x060017F8 RID: 6136 RVA: 0x0015FCEC File Offset: 0x0015ECEC
		internal static object BamlConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (!BaseUriHelper.IsPackApplicationUri(baseUri))
			{
				throw new InvalidOperationException(SR.Get("BamlIsNotSupportedOutsideOfApplicationResources"));
			}
			string partName;
			string text;
			string text2;
			string text3;
			BaseUriHelper.GetAssemblyNameAndPart(PackUriHelper.GetPartUri(baseUri), out partName, out text, out text2, out text3);
			if (ContentFileHelper.IsContentFile(partName))
			{
				throw new InvalidOperationException(SR.Get("BamlIsNotSupportedOutsideOfApplicationResources"));
			}
			return Application.LoadBamlStreamWithSyncInfo(stream, new ParserContext
			{
				BaseUri = baseUri,
				SkipJournaledProperties = isJournalNavigation
			});
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0015FD60 File Offset: 0x0015ED60
		internal static object XamlConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (sandboxExternalContent)
			{
				if (SecurityHelper.AreStringTypesEqual(baseUri.Scheme, BaseUriHelper.PackAppBaseUri.Scheme))
				{
					baseUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(baseUri);
				}
				stream.Close();
				return new WebBrowser
				{
					Source = baseUri
				};
			}
			ParserContext parserContext = new ParserContext();
			parserContext.BaseUri = baseUri;
			parserContext.SkipJournaledProperties = isJournalNavigation;
			if (allowAsync)
			{
				XamlReader xamlReader = new XamlReader();
				asyncObjectConverter = xamlReader;
				xamlReader.LoadCompleted += AppModelKnownContentFactory.OnParserComplete;
				return xamlReader.LoadAsync(stream, parserContext);
			}
			return XamlReader.Load(stream, parserContext);
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x0015FDEB File Offset: 0x0015EDEB
		private static void OnParserComplete(object sender, AsyncCompletedEventArgs args)
		{
			if (!args.Cancelled && args.Error != null)
			{
				throw args.Error;
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x0015FE04 File Offset: 0x0015EE04
		internal static object HtmlXappConverter(Stream stream, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			asyncObjectConverter = null;
			if (canUseTopLevelBrowser)
			{
				return null;
			}
			if (SecurityHelper.AreStringTypesEqual(baseUri.Scheme, BaseUriHelper.PackAppBaseUri.Scheme))
			{
				baseUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(baseUri);
			}
			stream.Close();
			return new WebBrowser
			{
				Source = baseUri
			};
		}
	}
}
