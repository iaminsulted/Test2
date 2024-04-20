using System;

namespace System.Windows
{
	// Token: 0x020003CA RID: 970
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class TemplatePartAttribute : Attribute
	{
		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x00197766 File Offset: 0x00196766
		// (set) Token: 0x060028D9 RID: 10457 RVA: 0x0019776E File Offset: 0x0019676E
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x00197777 File Offset: 0x00196777
		// (set) Token: 0x060028DB RID: 10459 RVA: 0x0019777F File Offset: 0x0019677F
		public Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x040014A9 RID: 5289
		private string _name;

		// Token: 0x040014AA RID: 5290
		private Type _type;
	}
}
