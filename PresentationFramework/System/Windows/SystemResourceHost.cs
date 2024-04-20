using System;

namespace System.Windows
{
	// Token: 0x020003B8 RID: 952
	internal sealed class SystemResourceHost
	{
		// Token: 0x0600280F RID: 10255 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private SystemResourceHost()
		{
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x00192354 File Offset: 0x00191354
		internal static SystemResourceHost Instance
		{
			get
			{
				if (SystemResourceHost._instance == null)
				{
					SystemResourceHost._instance = new SystemResourceHost();
				}
				return SystemResourceHost._instance;
			}
		}

		// Token: 0x04001364 RID: 4964
		private static SystemResourceHost _instance;
	}
}
