using System;

namespace System.Windows
{
	// Token: 0x02000350 RID: 848
	public class DataTemplateKey : TemplateKey
	{
		// Token: 0x06002036 RID: 8246 RVA: 0x00174B97 File Offset: 0x00173B97
		public DataTemplateKey() : base(TemplateKey.TemplateType.DataTemplate)
		{
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00174BA0 File Offset: 0x00173BA0
		public DataTemplateKey(object dataType) : base(TemplateKey.TemplateType.DataTemplate, dataType)
		{
		}
	}
}
