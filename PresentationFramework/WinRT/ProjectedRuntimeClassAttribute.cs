using System;
using System.ComponentModel;

namespace WinRT
{
	// Token: 0x0200009E RID: 158
	[EditorBrowsable(EditorBrowsableState.Never)]
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	internal sealed class ProjectedRuntimeClassAttribute : Attribute
	{
		// Token: 0x0600023E RID: 574 RVA: 0x000F99AD File Offset: 0x000F89AD
		public ProjectedRuntimeClassAttribute(string defaultInterfaceProp)
		{
			this.DefaultInterfaceProperty = defaultInterfaceProp;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000F99BC File Offset: 0x000F89BC
		public string DefaultInterfaceProperty { get; }
	}
}
