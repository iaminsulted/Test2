using System;
using System.Reflection;

namespace IngameDebugConsole
{
	// Token: 0x020001E8 RID: 488
	public class ConsoleMethodInfo
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x0002C79D File Offset: 0x0002A99D
		public ConsoleMethodInfo(MethodInfo method, Type[] parameterTypes, object instance, string signature)
		{
			this.method = method;
			this.parameterTypes = parameterTypes;
			this.instance = instance;
			this.signature = signature;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0002C7C2 File Offset: 0x0002A9C2
		public bool IsValid()
		{
			return this.method.IsStatic || (this.instance != null && !this.instance.Equals(null));
		}

		// Token: 0x04000AB5 RID: 2741
		public readonly MethodInfo method;

		// Token: 0x04000AB6 RID: 2742
		public readonly Type[] parameterTypes;

		// Token: 0x04000AB7 RID: 2743
		public readonly object instance;

		// Token: 0x04000AB8 RID: 2744
		public readonly string signature;
	}
}
