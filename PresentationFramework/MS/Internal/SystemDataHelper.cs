using System;
using System.ComponentModel;
using System.Reflection;

namespace MS.Internal
{
	// Token: 0x020000FD RID: 253
	internal static class SystemDataHelper
	{
		// Token: 0x06000601 RID: 1537 RVA: 0x0010585C File Offset: 0x0010485C
		internal static bool IsDataView(IBindingList list)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataView(list);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0010587C File Offset: 0x0010487C
		internal static bool IsDataRowView(object item)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataRowView(item);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0010589C File Offset: 0x0010489C
		internal static bool IsSqlNull(object value)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsSqlNull(value);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x001058BC File Offset: 0x001048BC
		internal static bool IsSqlNullableType(Type type)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsSqlNullableType(type);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x001058DC File Offset: 0x001048DC
		internal static bool IsDataSetCollectionProperty(PropertyDescriptor pd)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataSetCollectionProperty(pd);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x001058FC File Offset: 0x001048FC
		internal static object GetValue(object item, PropertyDescriptor pd, bool useFollowParent)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			if (systemDataExtensionMethods == null)
			{
				return null;
			}
			return systemDataExtensionMethods.GetValue(item, pd, useFollowParent);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00105920 File Offset: 0x00104920
		internal static bool DetermineWhetherDBNullIsValid(object item, string columnName, object arg)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.DetermineWhetherDBNullIsValid(item, columnName, arg);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00105944 File Offset: 0x00104944
		internal static object NullValueForSqlNullableType(Type type)
		{
			FieldInfo field = type.GetField("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (field != null)
			{
				return field.GetValue(null);
			}
			PropertyInfo property = type.GetProperty("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (property != null)
			{
				return property.GetValue(null, null);
			}
			return null;
		}
	}
}
