using System;

namespace System.Windows.Controls
{
	// Token: 0x0200079E RID: 1950
	public class ItemContainerTemplateKey : TemplateKey
	{
		// Token: 0x06006D9B RID: 28059 RVA: 0x002CE3CF File Offset: 0x002CD3CF
		public ItemContainerTemplateKey() : base(TemplateKey.TemplateType.TableTemplate)
		{
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x002CE3D8 File Offset: 0x002CD3D8
		public ItemContainerTemplateKey(object dataType) : base(TemplateKey.TemplateType.TableTemplate, dataType)
		{
		}
	}
}
