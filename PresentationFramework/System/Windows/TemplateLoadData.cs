using System;
using System.Collections.Generic;
using System.Xaml;

namespace System.Windows
{
	// Token: 0x020003C6 RID: 966
	internal class TemplateLoadData
	{
		// Token: 0x060028B2 RID: 10418 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal TemplateLoadData()
		{
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x0019711B File Offset: 0x0019611B
		// (set) Token: 0x060028B4 RID: 10420 RVA: 0x00197123 File Offset: 0x00196123
		internal TemplateContent.StackOfFrames Stack { get; set; }

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x0019712C File Offset: 0x0019612C
		internal Dictionary<string, XamlType> NamedTypes
		{
			get
			{
				if (this._namedTypes == null)
				{
					this._namedTypes = new Dictionary<string, XamlType>();
				}
				return this._namedTypes;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x00197147 File Offset: 0x00196147
		// (set) Token: 0x060028B7 RID: 10423 RVA: 0x0019714F File Offset: 0x0019614F
		internal XamlReader Reader { get; set; }

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x060028B8 RID: 10424 RVA: 0x00197158 File Offset: 0x00196158
		// (set) Token: 0x060028B9 RID: 10425 RVA: 0x00197160 File Offset: 0x00196160
		internal string RootName { get; set; }

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x060028BA RID: 10426 RVA: 0x00197169 File Offset: 0x00196169
		// (set) Token: 0x060028BB RID: 10427 RVA: 0x00197171 File Offset: 0x00196171
		internal object RootObject { get; set; }

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060028BC RID: 10428 RVA: 0x0019717A File Offset: 0x0019617A
		// (set) Token: 0x060028BD RID: 10429 RVA: 0x00197182 File Offset: 0x00196182
		internal TemplateContent.ServiceProviderWrapper ServiceProviderWrapper { get; set; }

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060028BE RID: 10430 RVA: 0x0019718B File Offset: 0x0019618B
		// (set) Token: 0x060028BF RID: 10431 RVA: 0x00197193 File Offset: 0x00196193
		internal XamlObjectWriter ObjectWriter { get; set; }

		// Token: 0x04001499 RID: 5273
		internal Dictionary<string, XamlType> _namedTypes;
	}
}
