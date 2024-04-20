using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Utility;

namespace MS.Internal.AppModel
{
	// Token: 0x02000294 RID: 660
	internal sealed class RequestSetStatusBarEventArgs : RoutedEventArgs
	{
		// Token: 0x060018F5 RID: 6389 RVA: 0x00162227 File Offset: 0x00161227
		internal RequestSetStatusBarEventArgs(string text)
		{
			this._text.Value = text;
			base.RoutedEvent = Hyperlink.RequestSetStatusBarEvent;
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x00162246 File Offset: 0x00161246
		internal RequestSetStatusBarEventArgs(Uri targetUri)
		{
			if (targetUri == null)
			{
				this._text.Value = string.Empty;
			}
			else
			{
				this._text.Value = BindUriHelper.UriToString(targetUri);
			}
			base.RoutedEvent = Hyperlink.RequestSetStatusBarEvent;
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060018F7 RID: 6391 RVA: 0x00162285 File Offset: 0x00161285
		internal string Text
		{
			get
			{
				return this._text.Value;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x00162292 File Offset: 0x00161292
		internal static RequestSetStatusBarEventArgs Clear
		{
			get
			{
				return new RequestSetStatusBarEventArgs(string.Empty);
			}
		}

		// Token: 0x04000D72 RID: 3442
		private SecurityCriticalDataForSet<string> _text;
	}
}
