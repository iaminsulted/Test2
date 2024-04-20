using System;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x02000403 RID: 1027
	internal class StaticResource
	{
		// Token: 0x06002C52 RID: 11346 RVA: 0x001A66C0 File Offset: 0x001A56C0
		public StaticResource(XamlType type, XamlSchemaContext schemaContext)
		{
			this.ResourceNodeList = new XamlNodeList(schemaContext, 8);
			this.ResourceNodeList.Writer.WriteStartObject(type);
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06002C53 RID: 11347 RVA: 0x001A66E6 File Offset: 0x001A56E6
		// (set) Token: 0x06002C54 RID: 11348 RVA: 0x001A66EE File Offset: 0x001A56EE
		public XamlNodeList ResourceNodeList { get; private set; }
	}
}
