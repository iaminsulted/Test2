using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005AF RID: 1455
	[Serializable]
	public abstract class CustomContentState
	{
		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x0600466C RID: 18028 RVA: 0x00109403 File Offset: 0x00108403
		public virtual string JournalEntryName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600466D RID: 18029
		public abstract void Replay(NavigationService navigationService, NavigationMode mode);
	}
}
