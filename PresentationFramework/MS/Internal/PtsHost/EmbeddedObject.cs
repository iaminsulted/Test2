using System;
using System.Windows;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000116 RID: 278
	internal abstract class EmbeddedObject
	{
		// Token: 0x06000730 RID: 1840 RVA: 0x0010CBE6 File Offset: 0x0010BBE6
		protected EmbeddedObject(int dcp)
		{
			this.Dcp = dcp;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void Dispose()
		{
		}

		// Token: 0x06000732 RID: 1842
		internal abstract void Update(EmbeddedObject newObject);

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000733 RID: 1843
		internal abstract DependencyObject Element { get; }

		// Token: 0x04000746 RID: 1862
		internal int Dcp;
	}
}
