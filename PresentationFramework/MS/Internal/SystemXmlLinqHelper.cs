using System;
using System.ComponentModel;

namespace MS.Internal
{
	// Token: 0x020000FF RID: 255
	internal static class SystemXmlLinqHelper
	{
		// Token: 0x06000611 RID: 1553 RVA: 0x00105AB0 File Offset: 0x00104AB0
		internal static bool IsXElement(object item)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXElement(item);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00105AD0 File Offset: 0x00104AD0
		internal static string GetXElementTagName(object item)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			if (systemXmlLinqExtensionMethods == null)
			{
				return null;
			}
			return systemXmlLinqExtensionMethods.GetXElementTagName(item);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00105AF0 File Offset: 0x00104AF0
		internal static bool IsXLinqCollectionProperty(PropertyDescriptor pd)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXLinqCollectionProperty(pd);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00105B10 File Offset: 0x00104B10
		internal static bool IsXLinqNonIdempotentProperty(PropertyDescriptor pd)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXLinqNonIdempotentProperty(pd);
		}
	}
}
