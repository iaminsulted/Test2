using System;
using System.Reflection;

namespace WinRT
{
	// Token: 0x020000B7 RID: 183
	internal static class TypeExtensions
	{
		// Token: 0x06000305 RID: 773 RVA: 0x000FD630 File Offset: 0x000FC630
		public static Type FindHelperType(this Type type)
		{
			if (typeof(Exception).IsAssignableFrom(type))
			{
				type = typeof(Exception);
			}
			Type type2 = Projections.FindCustomHelperTypeMapping(type);
			if (type2 != null)
			{
				return type2;
			}
			string typeName = "ABI." + type.FullName;
			string typeName2 = "MS.Internal.WindowsRuntime.ABI." + type.FullName;
			if (type.FullName.StartsWith("MS.Internal.WindowsRuntime."))
			{
				typeName = "MS.Internal.WindowsRuntime.ABI." + TypeExtensions.RemoveNamespacePrefix(type.FullName);
			}
			return Type.GetType(typeName) ?? Type.GetType(typeName2);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x000FD6C4 File Offset: 0x000FC6C4
		public static Type GetHelperType(this Type type)
		{
			Type type2 = type.FindHelperType();
			if (type2 != null)
			{
				return type2;
			}
			throw new InvalidOperationException("Target type is not a projected type: " + type.FullName + ".");
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000FD6F7 File Offset: 0x000FC6F7
		public static Type GetGuidType(this Type type)
		{
			if (!type.IsDelegate())
			{
				return type;
			}
			return type.GetHelperType();
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000FD70C File Offset: 0x000FC70C
		public static Type FindVftblType(this Type helperType)
		{
			Type type = helperType.GetNestedType("Vftbl");
			if (type == null)
			{
				return null;
			}
			if (helperType.IsGenericType && type != null)
			{
				type = type.MakeGenericType(helperType.GetGenericArguments());
			}
			return type;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x000FD743 File Offset: 0x000FC743
		public static Type GetAbiType(this Type type)
		{
			return type.GetHelperType().GetMethod("GetAbi", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).ReturnType;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000FD75C File Offset: 0x000FC75C
		public static Type GetMarshalerType(this Type type)
		{
			return type.GetHelperType().GetMethod("CreateMarshaler", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).ReturnType;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x000FD775 File Offset: 0x000FC775
		public static bool IsDelegate(this Type type)
		{
			return typeof(Delegate).IsAssignableFrom(type);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000FD787 File Offset: 0x000FC787
		public static string RemoveNamespacePrefix(string ns)
		{
			if (ns.StartsWith("MS.Internal.WindowsRuntime."))
			{
				return ns.Substring("MS.Internal.WindowsRuntime.".Length);
			}
			return ns;
		}
	}
}
