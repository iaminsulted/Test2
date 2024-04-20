using System;

namespace MS.Internal.Data
{
	// Token: 0x02000223 RID: 547
	internal abstract class DynamicPropertyAccessor : DynamicObjectAccessor
	{
		// Token: 0x06001481 RID: 5249 RVA: 0x0015244C File Offset: 0x0015144C
		protected DynamicPropertyAccessor(Type ownerType, string propertyName) : base(ownerType, propertyName)
		{
		}

		// Token: 0x06001482 RID: 5250
		public abstract object GetValue(object component);

		// Token: 0x06001483 RID: 5251
		public abstract void SetValue(object component, object value);
	}
}
