using System;
using System.Collections;

namespace System.Windows
{
	// Token: 0x020003BD RID: 957
	internal class DeferredAppResourceReference : DeferredResourceReference
	{
		// Token: 0x06002868 RID: 10344 RVA: 0x00195294 File Offset: 0x00194294
		internal DeferredAppResourceReference(ResourceDictionary dictionary, object resourceKey) : base(dictionary, resourceKey)
		{
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x001952A0 File Offset: 0x001942A0
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			object syncRoot = ((ICollection)Application.Current.Resources).SyncRoot;
			object value;
			lock (syncRoot)
			{
				value = base.GetValue(valueSource);
			}
			return value;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x001952EC File Offset: 0x001942EC
		internal override Type GetValueType()
		{
			object syncRoot = ((ICollection)Application.Current.Resources).SyncRoot;
			Type valueType;
			lock (syncRoot)
			{
				valueType = base.GetValueType();
			}
			return valueType;
		}
	}
}
