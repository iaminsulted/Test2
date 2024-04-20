using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using ABI.System;
using MS.Internal.WindowsRuntime.ABI.System.Collections.Generic;

namespace WinRT
{
	// Token: 0x020000B6 RID: 182
	internal static class Projections
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x000FD158 File Offset: 0x000FC158
		static Projections()
		{
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(bool), typeof(Boolean), "Boolean", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(char), typeof(Char), "Char", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IReadOnlyList<>), typeof(IReadOnlyList<>), "Windows.Foundation.Collections.IVectorView`1", false);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000FD200 File Offset: 0x000FC200
		private static void RegisterCustomAbiTypeMappingNoLock(Type publicType, Type abiType, string winrtTypeName, bool isRuntimeClass = false)
		{
			Projections.CustomTypeToHelperTypeMappings.Add(publicType, abiType);
			Projections.CustomAbiTypeToTypeMappings.Add(abiType, publicType);
			Projections.CustomTypeToAbiTypeNameMappings.Add(publicType, winrtTypeName);
			Projections.CustomAbiTypeNameToTypeMappings.Add(winrtTypeName, publicType);
			if (isRuntimeClass)
			{
				Projections.ProjectedRuntimeClassNames.Add(winrtTypeName);
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000FD24C File Offset: 0x000FC24C
		public static Type FindCustomHelperTypeMapping(Type publicType)
		{
			Projections.rwlock.EnterReadLock();
			Type result;
			try
			{
				if (publicType.IsGenericType)
				{
					Type type;
					result = (Projections.CustomTypeToHelperTypeMappings.TryGetValue(publicType.GetGenericTypeDefinition(), out type) ? type.MakeGenericType(publicType.GetGenericArguments()) : null);
				}
				else
				{
					Type type2;
					result = (Projections.CustomTypeToHelperTypeMappings.TryGetValue(publicType, out type2) ? type2 : null);
				}
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x000FD2C4 File Offset: 0x000FC2C4
		public static Type FindCustomPublicTypeForAbiType(Type abiType)
		{
			Projections.rwlock.EnterReadLock();
			Type result;
			try
			{
				if (abiType.IsGenericType)
				{
					Type type;
					result = (Projections.CustomAbiTypeToTypeMappings.TryGetValue(abiType.GetGenericTypeDefinition(), out type) ? type.MakeGenericType(abiType.GetGenericArguments()) : null);
				}
				else
				{
					Type type2;
					result = (Projections.CustomAbiTypeToTypeMappings.TryGetValue(abiType, out type2) ? type2 : null);
				}
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000FD33C File Offset: 0x000FC33C
		public static Type FindCustomTypeForAbiTypeName(string abiTypeName)
		{
			Projections.rwlock.EnterReadLock();
			Type result;
			try
			{
				Type type;
				result = (Projections.CustomAbiTypeNameToTypeMappings.TryGetValue(abiTypeName, out type) ? type : null);
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000FD388 File Offset: 0x000FC388
		public static string FindCustomAbiTypeNameForType(Type type)
		{
			Projections.rwlock.EnterReadLock();
			string result;
			try
			{
				string text;
				result = (Projections.CustomTypeToAbiTypeNameMappings.TryGetValue(type, out text) ? text : null);
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x000FD3D4 File Offset: 0x000FC3D4
		public static bool IsTypeWindowsRuntimeType(Type type)
		{
			Type type2 = type;
			if (type2.IsArray)
			{
				type2 = type2.GetElementType();
			}
			return Projections.IsTypeWindowsRuntimeTypeNoArray(type2);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000FD3F8 File Offset: 0x000FC3F8
		private static bool IsTypeWindowsRuntimeTypeNoArray(Type type)
		{
			if (!type.IsConstructedGenericType)
			{
				return Projections.CustomTypeToAbiTypeNameMappings.ContainsKey(type) || type.IsPrimitive || type == typeof(string) || type == typeof(Guid) || type == typeof(object) || type.GetCustomAttribute<WindowsRuntimeTypeAttribute>() != null;
			}
			if (Projections.IsTypeWindowsRuntimeTypeNoArray(type.GetGenericTypeDefinition()))
			{
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (!Projections.IsTypeWindowsRuntimeTypeNoArray(genericArguments[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000FD498 File Offset: 0x000FC498
		public static bool TryGetCompatibleWindowsRuntimeTypeForVariantType(Type type, out Type compatibleType)
		{
			compatibleType = null;
			if (!type.IsConstructedGenericType)
			{
				throw new ArgumentException("type");
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (!Projections.IsTypeWindowsRuntimeTypeNoArray(genericTypeDefinition))
			{
				return false;
			}
			Type[] genericArguments = genericTypeDefinition.GetGenericArguments();
			Type[] genericArguments2 = type.GetGenericArguments();
			Type[] array = new Type[genericArguments2.Length];
			for (int i = 0; i < genericArguments2.Length; i++)
			{
				if (!Projections.IsTypeWindowsRuntimeTypeNoArray(genericArguments2[i]))
				{
					if ((genericArguments[i].GenericParameterAttributes & GenericParameterAttributes.VarianceMask) != GenericParameterAttributes.Covariant || genericArguments2[i].IsValueType)
					{
						return false;
					}
					array[i] = typeof(object);
				}
				else
				{
					array[i] = genericArguments2[i];
				}
			}
			compatibleType = genericTypeDefinition.MakeGenericType(array);
			return true;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000FD540 File Offset: 0x000FC540
		internal static bool TryGetDefaultInterfaceTypeForRuntimeClassType(Type runtimeClass, out Type defaultInterface)
		{
			defaultInterface = null;
			ProjectedRuntimeClassAttribute customAttribute = runtimeClass.GetCustomAttribute<ProjectedRuntimeClassAttribute>();
			if (customAttribute == null)
			{
				return false;
			}
			defaultInterface = runtimeClass.GetProperty(customAttribute.DefaultInterfaceProperty, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).PropertyType;
			return true;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x000FD574 File Offset: 0x000FC574
		internal static Type GetDefaultInterfaceTypeForRuntimeClassType(Type runtimeClass)
		{
			Type result;
			if (!Projections.TryGetDefaultInterfaceTypeForRuntimeClassType(runtimeClass, out result))
			{
				throw new ArgumentException("The provided type '" + runtimeClass.FullName + "' is not a WinRT projected runtime class.", "runtimeClass");
			}
			return result;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000FD5AC File Offset: 0x000FC5AC
		internal static bool TryGetMarshalerTypeForProjectedRuntimeClass(IObjectReference objectReference, out Type type)
		{
			ObjectReference<IInspectable.Vftbl> objectReference2;
			if (objectReference.TryAs<IInspectable.Vftbl>(out objectReference2) == 0)
			{
				Projections.rwlock.EnterReadLock();
				try
				{
					string runtimeClassName = objectReference2.GetRuntimeClassName(true);
					if (runtimeClassName != null && Projections.ProjectedRuntimeClassNames.Contains(runtimeClassName))
					{
						type = Projections.CustomTypeToHelperTypeMappings[Projections.CustomAbiTypeNameToTypeMappings[runtimeClassName]];
						return true;
					}
				}
				finally
				{
					objectReference2.Dispose();
					Projections.rwlock.ExitReadLock();
				}
			}
			type = null;
			return false;
		}

		// Token: 0x040005E4 RID: 1508
		private static readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();

		// Token: 0x040005E5 RID: 1509
		private static readonly Dictionary<Type, Type> CustomTypeToHelperTypeMappings = new Dictionary<Type, Type>();

		// Token: 0x040005E6 RID: 1510
		private static readonly Dictionary<Type, Type> CustomAbiTypeToTypeMappings = new Dictionary<Type, Type>();

		// Token: 0x040005E7 RID: 1511
		private static readonly Dictionary<string, Type> CustomAbiTypeNameToTypeMappings = new Dictionary<string, Type>();

		// Token: 0x040005E8 RID: 1512
		private static readonly Dictionary<Type, string> CustomTypeToAbiTypeNameMappings = new Dictionary<Type, string>();

		// Token: 0x040005E9 RID: 1513
		private static readonly HashSet<string> ProjectedRuntimeClassNames = new HashSet<string>();
	}
}
