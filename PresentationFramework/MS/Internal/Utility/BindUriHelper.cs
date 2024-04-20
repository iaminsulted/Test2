using System;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using MS.Internal.AppModel;

namespace MS.Internal.Utility
{
	// Token: 0x020002E0 RID: 736
	internal static class BindUriHelper
	{
		// Token: 0x06001BCB RID: 7115 RVA: 0x0016A568 File Offset: 0x00169568
		internal static string UriToString(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			return new StringBuilder(uri.GetComponents(uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString, UriFormat.SafeUnescaped), 2083).ToString();
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x0016A5A5 File Offset: 0x001695A5
		// (set) Token: 0x06001BCD RID: 7117 RVA: 0x0016A5AC File Offset: 0x001695AC
		internal static Uri BaseUri
		{
			get
			{
				return BaseUriHelper.BaseUri;
			}
			set
			{
				BaseUriHelper.BaseUri = BaseUriHelper.FixFileUri(value);
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0016A5B9 File Offset: 0x001695B9
		internal static bool DoSchemeAndHostMatch(Uri first, Uri second)
		{
			return SecurityHelper.AreStringTypesEqual(first.Scheme, second.Scheme) && first.Host.Equals(second.Host);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0016A5E4 File Offset: 0x001695E4
		internal static Uri GetResolvedUri(Uri baseUri, Uri orgUri)
		{
			Uri result;
			if (orgUri == null)
			{
				result = null;
			}
			else if (!orgUri.IsAbsoluteUri)
			{
				result = new Uri((baseUri == null) ? BindUriHelper.BaseUri : baseUri, orgUri);
			}
			else
			{
				result = BaseUriHelper.FixFileUri(orgUri);
			}
			return result;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0016A628 File Offset: 0x00169628
		internal static string GetReferer(Uri destinationUri)
		{
			string result = null;
			Uri browserSource = SiteOfOriginContainer.BrowserSource;
			if (browserSource != null)
			{
				int num = CustomCredentialPolicy.MapUrlToZone(browserSource);
				int num2 = CustomCredentialPolicy.MapUrlToZone(destinationUri);
				if (num == num2 && SecurityHelper.AreStringTypesEqual(browserSource.Scheme, destinationUri.Scheme))
				{
					result = browserSource.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped);
				}
			}
			return result;
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0016A674 File Offset: 0x00169674
		internal static Uri GetResolvedUri(Uri originalUri)
		{
			return BindUriHelper.GetResolvedUri(null, originalUri);
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0016A680 File Offset: 0x00169680
		internal static Uri GetUriToNavigate(DependencyObject element, Uri baseUri, Uri inputUri)
		{
			if (inputUri == null || inputUri.IsAbsoluteUri)
			{
				return inputUri;
			}
			if (BindUriHelper.StartWithFragment(inputUri))
			{
				baseUri = null;
			}
			Uri resolvedUri;
			if (baseUri != null)
			{
				if (!baseUri.IsAbsoluteUri)
				{
					resolvedUri = BindUriHelper.GetResolvedUri(BindUriHelper.GetResolvedUri(null, baseUri), inputUri);
				}
				else
				{
					resolvedUri = BindUriHelper.GetResolvedUri(baseUri, inputUri);
				}
			}
			else
			{
				Uri uri = null;
				if (element != null)
				{
					INavigator navigator = element as INavigator;
					if (navigator != null)
					{
						uri = navigator.CurrentSource;
					}
					else
					{
						NavigationService navigationService = element.GetValue(NavigationService.NavigationServiceProperty) as NavigationService;
						uri = ((navigationService == null) ? null : navigationService.CurrentSource);
					}
				}
				if (uri != null)
				{
					if (uri.IsAbsoluteUri)
					{
						resolvedUri = BindUriHelper.GetResolvedUri(uri, inputUri);
					}
					else
					{
						resolvedUri = BindUriHelper.GetResolvedUri(BindUriHelper.GetResolvedUri(null, uri), inputUri);
					}
				}
				else
				{
					resolvedUri = BindUriHelper.GetResolvedUri(null, inputUri);
				}
			}
			return resolvedUri;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0016A744 File Offset: 0x00169744
		internal static bool StartWithFragment(Uri uri)
		{
			return uri.OriginalString.StartsWith("#", StringComparison.Ordinal);
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0016A758 File Offset: 0x00169758
		internal static string GetFragment(Uri uri)
		{
			Uri uri2 = uri;
			string result = string.Empty;
			if (!uri.IsAbsoluteUri)
			{
				uri2 = new Uri(BindUriHelper.placeboBase, uri);
			}
			string fragment = uri2.Fragment;
			if (fragment != null && fragment.Length > 0)
			{
				result = fragment.Substring(1);
			}
			return result;
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0016A7A0 File Offset: 0x001697A0
		internal static Uri GetUriRelativeToPackAppBase(Uri original)
		{
			if (original == null)
			{
				return null;
			}
			Uri resolvedUri = BindUriHelper.GetResolvedUri(original);
			return BaseUriHelper.PackAppBaseUri.MakeRelativeUri(resolvedUri);
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0016A7CA File Offset: 0x001697CA
		internal static bool IsXamlMimeType(ContentType mimeType)
		{
			return MimeTypeMapper.XamlMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedDocumentSequenceMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedDocumentMime.AreTypeAndSubTypeEqual(mimeType) || MimeTypeMapper.FixedPageMime.AreTypeAndSubTypeEqual(mimeType);
		}

		// Token: 0x04000E61 RID: 3681
		private const int MAX_PATH_LENGTH = 2048;

		// Token: 0x04000E62 RID: 3682
		private const int MAX_SCHEME_LENGTH = 32;

		// Token: 0x04000E63 RID: 3683
		public const int MAX_URL_LENGTH = 2083;

		// Token: 0x04000E64 RID: 3684
		private const string PLACEBOURI = "http://microsoft.com/";

		// Token: 0x04000E65 RID: 3685
		private static Uri placeboBase = new Uri("http://microsoft.com/");

		// Token: 0x04000E66 RID: 3686
		private const string FRAGMENTMARKER = "#";
	}
}
