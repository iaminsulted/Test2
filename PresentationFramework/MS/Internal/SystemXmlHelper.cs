using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020000FE RID: 254
	internal static class SystemXmlHelper
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00105994 File Offset: 0x00104994
		internal static bool IsXmlNode(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsXmlNode(item);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x001059B4 File Offset: 0x001049B4
		internal static bool IsXmlNamespaceManager(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsXmlNamespaceManager(item);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x001059D4 File Offset: 0x001049D4
		internal static bool TryGetValueFromXmlNode(object item, string name, out object value)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods != null)
			{
				return systemXmlExtensionMethods.TryGetValueFromXmlNode(item, name, out value);
			}
			value = null;
			return false;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x001059FC File Offset: 0x001049FC
		internal static IComparer PrepareXmlComparer(IEnumerable collection, SortDescriptionCollection sort, CultureInfo culture)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods != null)
			{
				return systemXmlExtensionMethods.PrepareXmlComparer(collection, sort, culture);
			}
			return null;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00105A20 File Offset: 0x00104A20
		internal static bool IsEmptyXmlDataCollection(object parent)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsEmptyXmlDataCollection(parent);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00105A40 File Offset: 0x00104A40
		internal static string GetXmlTagName(object item, DependencyObject target)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return null;
			}
			return systemXmlExtensionMethods.GetXmlTagName(item, target);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00105A64 File Offset: 0x00104A64
		internal static object FindXmlNodeWithInnerText(IEnumerable items, object innerText, out int index)
		{
			index = -1;
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return DependencyProperty.UnsetValue;
			}
			return systemXmlExtensionMethods.FindXmlNodeWithInnerText(items, innerText, out index);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00105A90 File Offset: 0x00104A90
		internal static object GetInnerText(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return null;
			}
			return systemXmlExtensionMethods.GetInnerText(item);
		}
	}
}
