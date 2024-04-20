using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000278 RID: 632
	internal interface IContentContainer
	{
		// Token: 0x06001831 RID: 6193
		void OnContentReady(ContentType contentType, object content, Uri uri, object navState);

		// Token: 0x06001832 RID: 6194
		void OnNavigationProgress(Uri uri, long bytesRead, long maxBytes);

		// Token: 0x06001833 RID: 6195
		void OnStreamClosed(Uri uri);
	}
}
