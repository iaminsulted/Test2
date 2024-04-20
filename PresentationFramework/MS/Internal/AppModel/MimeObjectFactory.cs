using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x0200028D RID: 653
	internal static class MimeObjectFactory
	{
		// Token: 0x060018CE RID: 6350 RVA: 0x00161748 File Offset: 0x00160748
		internal static object GetObjectAndCloseStream(Stream s, ContentType contentType, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			object result = null;
			asyncObjectConverter = null;
			StreamToObjectFactoryDelegate streamToObjectFactoryDelegate;
			if (contentType != null && MimeObjectFactory._objectConverters.TryGetValue(contentType, out streamToObjectFactoryDelegate))
			{
				result = streamToObjectFactoryDelegate(s, baseUri, canUseTopLevelBrowser, sandboxExternalContent, allowAsync, isJournalNavigation, out asyncObjectConverter);
			}
			return result;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x00161780 File Offset: 0x00160780
		internal static void Register(ContentType contentType, StreamToObjectFactoryDelegate method)
		{
			MimeObjectFactory._objectConverters[contentType] = method;
		}

		// Token: 0x04000D63 RID: 3427
		private static readonly Dictionary<ContentType, StreamToObjectFactoryDelegate> _objectConverters = new Dictionary<ContentType, StreamToObjectFactoryDelegate>(5, new ContentType.WeakComparer());
	}
}
