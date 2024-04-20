using System;

namespace System.Windows
{
	// Token: 0x020003CB RID: 971
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class TemplateVisualStateAttribute : Attribute
	{
		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x00197788 File Offset: 0x00196788
		// (set) Token: 0x060028DD RID: 10461 RVA: 0x00197790 File Offset: 0x00196790
		public string Name { get; set; }

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060028DE RID: 10462 RVA: 0x00197799 File Offset: 0x00196799
		// (set) Token: 0x060028DF RID: 10463 RVA: 0x001977A1 File Offset: 0x001967A1
		public string GroupName { get; set; }
	}
}
