using System;

namespace MS.Internal
{
	// Token: 0x020000FC RID: 252
	internal static class SystemCoreHelper
	{
		// Token: 0x060005FE RID: 1534 RVA: 0x001057F8 File Offset: 0x001047F8
		internal static bool IsIDynamicMetaObjectProvider(object item)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			return systemCoreExtensionMethods != null && systemCoreExtensionMethods.IsIDynamicMetaObjectProvider(item);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00105818 File Offset: 0x00104818
		internal static object NewDynamicPropertyAccessor(Type ownerType, string propertyName)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			if (systemCoreExtensionMethods == null)
			{
				return null;
			}
			return systemCoreExtensionMethods.NewDynamicPropertyAccessor(ownerType, propertyName);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0010583C File Offset: 0x0010483C
		internal static object GetIndexerAccessor(int rank)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			if (systemCoreExtensionMethods == null)
			{
				return null;
			}
			return systemCoreExtensionMethods.GetIndexerAccessor(rank);
		}
	}
}
