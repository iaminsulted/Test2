using System;

namespace System.Windows
{
	// Token: 0x020003A7 RID: 935
	internal class BindingValueCache
	{
		// Token: 0x0600261C RID: 9756 RVA: 0x0018B3A7 File Offset: 0x0018A3A7
		internal BindingValueCache(Type bindingValueType, object valueAsBindingValueType)
		{
			this.BindingValueType = bindingValueType;
			this.ValueAsBindingValueType = valueAsBindingValueType;
		}

		// Token: 0x040011DA RID: 4570
		internal readonly Type BindingValueType;

		// Token: 0x040011DB RID: 4571
		internal readonly object ValueAsBindingValueType;
	}
}
