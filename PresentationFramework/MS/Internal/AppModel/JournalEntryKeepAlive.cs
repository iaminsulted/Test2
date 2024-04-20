using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000284 RID: 644
	internal class JournalEntryKeepAlive : JournalEntry
	{
		// Token: 0x0600186C RID: 6252 RVA: 0x00160B92 File Offset: 0x0015FB92
		internal JournalEntryKeepAlive(JournalEntryGroupState jeGroupState, Uri uri, object keepAliveRoot) : base(jeGroupState, uri)
		{
			Invariant.Assert(keepAliveRoot != null);
			this._keepAliveRoot = keepAliveRoot;
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600186D RID: 6253 RVA: 0x00160BAC File Offset: 0x0015FBAC
		internal object KeepAliveRoot
		{
			get
			{
				return this._keepAliveRoot;
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x00160BB4 File Offset: 0x0015FBB4
		internal override bool IsAlive()
		{
			return this.KeepAliveRoot != null;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00160BBF File Offset: 0x0015FBBF
		internal override void SaveState(object contentObject)
		{
			this._keepAliveRoot = contentObject;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00160BC8 File Offset: 0x0015FBC8
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			return navigator.Navigate(this.KeepAliveRoot, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x04000D50 RID: 3408
		private object _keepAliveRoot;
	}
}
