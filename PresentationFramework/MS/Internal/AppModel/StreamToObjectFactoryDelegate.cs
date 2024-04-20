using System;
using System.IO;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x0200028C RID: 652
	// (Invoke) Token: 0x060018CB RID: 6347
	internal delegate object StreamToObjectFactoryDelegate(Stream s, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter);
}
