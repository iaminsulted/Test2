using System;

namespace MS.Internal.Data
{
	// Token: 0x02000224 RID: 548
	internal abstract class DynamicIndexerAccessor : DynamicObjectAccessor
	{
		// Token: 0x06001484 RID: 5252 RVA: 0x0015244C File Offset: 0x0015144C
		protected DynamicIndexerAccessor(Type ownerType, string propertyName) : base(ownerType, propertyName)
		{
		}

		// Token: 0x06001485 RID: 5253
		public abstract object GetValue(object component, object[] args);

		// Token: 0x06001486 RID: 5254
		public abstract void SetValue(object component, object[] args, object value);
	}
}
