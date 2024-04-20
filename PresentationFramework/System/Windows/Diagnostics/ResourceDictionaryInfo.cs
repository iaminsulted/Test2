using System;
using System.Diagnostics;
using System.Reflection;

namespace System.Windows.Diagnostics
{
	// Token: 0x02000444 RID: 1092
	[DebuggerDisplay("Assembly = {Assembly?.GetName()?.Name}, ResourceDictionary SourceUri = {SourceUri?.AbsoluteUri}")]
	public class ResourceDictionaryInfo
	{
		// Token: 0x06003537 RID: 13623 RVA: 0x001DD777 File Offset: 0x001DC777
		internal ResourceDictionaryInfo(Assembly assembly, Assembly resourceDictionaryAssembly, ResourceDictionary resourceDictionary, Uri sourceUri)
		{
			this.Assembly = assembly;
			this.ResourceDictionaryAssembly = resourceDictionaryAssembly;
			this.ResourceDictionary = resourceDictionary;
			this.SourceUri = sourceUri;
		}

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06003538 RID: 13624 RVA: 0x001DD79C File Offset: 0x001DC79C
		// (set) Token: 0x06003539 RID: 13625 RVA: 0x001DD7A4 File Offset: 0x001DC7A4
		public Assembly Assembly { get; private set; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x0600353A RID: 13626 RVA: 0x001DD7AD File Offset: 0x001DC7AD
		// (set) Token: 0x0600353B RID: 13627 RVA: 0x001DD7B5 File Offset: 0x001DC7B5
		public Assembly ResourceDictionaryAssembly { get; private set; }

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x001DD7BE File Offset: 0x001DC7BE
		// (set) Token: 0x0600353D RID: 13629 RVA: 0x001DD7C6 File Offset: 0x001DC7C6
		public ResourceDictionary ResourceDictionary { get; private set; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600353E RID: 13630 RVA: 0x001DD7CF File Offset: 0x001DC7CF
		// (set) Token: 0x0600353F RID: 13631 RVA: 0x001DD7D7 File Offset: 0x001DC7D7
		public Uri SourceUri { get; private set; }
	}
}
