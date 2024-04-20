using System;

namespace System.Windows.Diagnostics
{
	// Token: 0x02000443 RID: 1091
	public class ResourceDictionaryUnloadedEventArgs : EventArgs
	{
		// Token: 0x06003534 RID: 13620 RVA: 0x001DD757 File Offset: 0x001DC757
		internal ResourceDictionaryUnloadedEventArgs(ResourceDictionaryInfo resourceDictionaryInfo)
		{
			this.ResourceDictionaryInfo = resourceDictionaryInfo;
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003535 RID: 13621 RVA: 0x001DD766 File Offset: 0x001DC766
		// (set) Token: 0x06003536 RID: 13622 RVA: 0x001DD76E File Offset: 0x001DC76E
		public ResourceDictionaryInfo ResourceDictionaryInfo { get; private set; }
	}
}
