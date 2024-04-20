using System;

namespace System.Windows.Diagnostics
{
	// Token: 0x02000442 RID: 1090
	public class ResourceDictionaryLoadedEventArgs : EventArgs
	{
		// Token: 0x06003531 RID: 13617 RVA: 0x001DD737 File Offset: 0x001DC737
		internal ResourceDictionaryLoadedEventArgs(ResourceDictionaryInfo resourceDictionaryInfo)
		{
			this.ResourceDictionaryInfo = resourceDictionaryInfo;
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06003532 RID: 13618 RVA: 0x001DD746 File Offset: 0x001DC746
		// (set) Token: 0x06003533 RID: 13619 RVA: 0x001DD74E File Offset: 0x001DC74E
		public ResourceDictionaryInfo ResourceDictionaryInfo { get; private set; }
	}
}
