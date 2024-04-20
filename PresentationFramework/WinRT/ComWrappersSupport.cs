using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000A1 RID: 161
	internal static class ComWrappersSupport
	{
		// Token: 0x06000243 RID: 579 RVA: 0x000F99E3 File Offset: 0x000F89E3
		static ComWrappersSupport()
		{
			ComWrappersSupport.PlatformSpecificInitialize();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000F9A17 File Offset: 0x000F8A17
		private static void PlatformSpecificInitialize()
		{
			ComWrappersSupport.IUnknownVftbl = WpfWinRTComWrappers.IUnknownVftbl;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000F9A24 File Offset: 0x000F8A24
		public static TReturn MarshalDelegateInvoke<TDelegate, TReturn>(IntPtr thisPtr, Func<TDelegate, TReturn> invoke) where TDelegate : class, Delegate
		{
			TDelegate tdelegate = ComWrappersSupport.FindObject<TDelegate>(thisPtr);
			if (tdelegate != null)
			{
				return invoke(tdelegate);
			}
			return default(TReturn);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000F9A58 File Offset: 0x000F8A58
		public static void MarshalDelegateInvoke<T>(IntPtr thisPtr, Action<T> invoke) where T : class, Delegate
		{
			T t = ComWrappersSupport.FindObject<T>(thisPtr);
			if (t != null)
			{
				invoke(t);
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000F9A84 File Offset: 0x000F8A84
		public static bool TryUnwrapObject(object o, out IObjectReference objRef)
		{
			Delegate @delegate = o as Delegate;
			if (@delegate != null)
			{
				return ComWrappersSupport.TryUnwrapObject(@delegate.Target, out objRef);
			}
			Type type = o.GetType();
			ObjectReferenceWrapperAttribute customAttribute = type.GetCustomAttribute<ObjectReferenceWrapperAttribute>();
			if (customAttribute != null)
			{
				objRef = (IObjectReference)type.GetField(customAttribute.ObjectReferenceField, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(o);
				return true;
			}
			ProjectedRuntimeClassAttribute customAttribute2 = type.GetCustomAttribute<ProjectedRuntimeClassAttribute>();
			if (customAttribute2 != null)
			{
				return ComWrappersSupport.TryUnwrapObject(type.GetProperty(customAttribute2.DefaultInterfaceProperty, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(o), out objRef);
			}
			objRef = null;
			return false;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000F9B00 File Offset: 0x000F8B00
		public static IObjectReference GetObjectReferenceForInterface(IntPtr externalComObject)
		{
			IObjectReference result;
			using (ObjectReference<IUnknownVftbl> objectReference = ObjectReference<IUnknownVftbl>.FromAbi(externalComObject))
			{
				ObjectReference<IUnknownVftbl> objectReference2;
				if (objectReference.TryAs<IUnknownVftbl>(ComWrappersSupport.IID_IAgileObject, out objectReference2) >= 0)
				{
					objectReference2.Dispose();
					result = objectReference.As<IUnknownVftbl>();
				}
				else
				{
					result = new ObjectReferenceWithContext<IUnknownVftbl>(objectReference.GetRef(), Context.GetContextCallback());
				}
			}
			return result;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000F9B64 File Offset: 0x000F8B64
		public static List<ComWrappers.ComInterfaceEntry> GetInterfaceTableEntries(object obj)
		{
			List<ComWrappers.ComInterfaceEntry> list = new List<ComWrappers.ComInterfaceEntry>();
			foreach (Type type in obj.GetType().GetInterfaces())
			{
				if (Projections.IsTypeWindowsRuntimeType(type))
				{
					Type type2 = type.FindHelperType();
					list.Add(new ComWrappers.ComInterfaceEntry
					{
						IID = GuidGenerator.GetIID(type2),
						Vtable = (IntPtr)type2.FindVftblType().GetField("AbiToProjectionVftablePtr", BindingFlags.Static | BindingFlags.Public).GetValue(null)
					});
				}
				Type type3;
				if (type.IsConstructedGenericType && Projections.TryGetCompatibleWindowsRuntimeTypeForVariantType(type, out type3))
				{
					Type type4 = type3.FindHelperType();
					list.Add(new ComWrappers.ComInterfaceEntry
					{
						IID = GuidGenerator.GetIID(type4),
						Vtable = (IntPtr)type4.FindVftblType().GetField("AbiToProjectionVftablePtr", BindingFlags.Static | BindingFlags.Public).GetValue(null)
					});
				}
			}
			if (obj is Delegate)
			{
				list.Add(new ComWrappers.ComInterfaceEntry
				{
					IID = GuidGenerator.GetIID(obj.GetType()),
					Vtable = (IntPtr)obj.GetType().GetHelperType().GetField("AbiToProjectionVftablePtr", BindingFlags.Static | BindingFlags.Public).GetValue(null)
				});
			}
			list.Add(new ComWrappers.ComInterfaceEntry
			{
				IID = ComWrappersSupport.IID_IAgileObject,
				Vtable = IUnknownVftbl.AbiToProjectionVftblPtr
			});
			return list;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000F9CC8 File Offset: 0x000F8CC8
		[return: TupleElementNames(new string[]
		{
			"inspectableInfo",
			"interfaceTableEntries"
		})]
		public static ValueTuple<ComWrappersSupport.InspectableInfo, List<ComWrappers.ComInterfaceEntry>> PregenerateNativeTypeInformation(object obj)
		{
			List<ComWrappers.ComInterfaceEntry> interfaceTableEntries = ComWrappersSupport.GetInterfaceTableEntries(obj);
			Guid[] array = new Guid[interfaceTableEntries.Count];
			for (int i = 0; i < interfaceTableEntries.Count; i++)
			{
				array[i] = interfaceTableEntries[i].IID;
			}
			Type type = obj.GetType();
			if (type.FullName.StartsWith("ABI."))
			{
				Type type2;
				if ((type2 = Projections.FindCustomPublicTypeForAbiType(type)) == null)
				{
					type2 = (type.Assembly.GetType(type.FullName.Substring("ABI.".Length)) ?? type);
				}
				type = type2;
			}
			return new ValueTuple<ComWrappersSupport.InspectableInfo, List<ComWrappers.ComInterfaceEntry>>(new ComWrappersSupport.InspectableInfo(type, array), interfaceTableEntries);
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000F9D62 File Offset: 0x000F8D62
		private static ComWrappers ComWrappers { get; } = new WpfWinRTComWrappers();

		// Token: 0x0600024C RID: 588 RVA: 0x000F9D6C File Offset: 0x000F8D6C
		public static ComWrappersSupport.InspectableInfo GetInspectableInfo(IntPtr pThis)
		{
			object key = ComWrappersSupport.FindObject<object>(pThis);
			return ComWrappersSupport.InspectableInfoTable.GetValue(key, (object o) => ComWrappersSupport.PregenerateNativeTypeInformation(o).Item1);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000F9DAA File Offset: 0x000F8DAA
		public static object CreateRcwForComObject(IntPtr ptr)
		{
			return ComWrappersSupport.ComWrappers.GetOrCreateObjectForComInstance(ptr, CreateObjectFlags.TrackerObject);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000F9DB8 File Offset: 0x000F8DB8
		public static void RegisterObjectForInterface(object obj, IntPtr thisPtr)
		{
			ComWrappersSupport.TryRegisterObjectForInterface(obj, thisPtr);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000F9DC2 File Offset: 0x000F8DC2
		public static object TryRegisterObjectForInterface(object obj, IntPtr thisPtr)
		{
			return ComWrappersSupport.ComWrappers.GetOrRegisterObjectForComInstance(thisPtr, CreateObjectFlags.TrackerObject, obj);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000F9DD4 File Offset: 0x000F8DD4
		public static IObjectReference CreateCCWForObject(object obj)
		{
			IntPtr orCreateComInterfaceForObject = ComWrappersSupport.ComWrappers.GetOrCreateComInterfaceForObject(obj, CreateComInterfaceFlags.TrackerSupport);
			return ObjectReference<IUnknownVftbl>.Attach(ref orCreateComInterfaceForObject);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000F9DF5 File Offset: 0x000F8DF5
		public unsafe static T FindObject<T>(IntPtr ptr) where T : class
		{
			return ComWrappers.ComInterfaceDispatch.GetInstance<T>((ComWrappers.ComInterfaceDispatch*)((void*)ptr));
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000F9E02 File Offset: 0x000F8E02
		private static T FindDelegate<T>(IntPtr thisPtr) where T : class, Delegate
		{
			return ComWrappersSupport.FindObject<T>(thisPtr);
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000253 RID: 595 RVA: 0x000F9E0A File Offset: 0x000F8E0A
		// (set) Token: 0x06000254 RID: 596 RVA: 0x000F9E11 File Offset: 0x000F8E11
		public static IUnknownVftbl IUnknownVftbl { get; private set; }

		// Token: 0x06000255 RID: 597 RVA: 0x000F9E19 File Offset: 0x000F8E19
		public static IntPtr AllocateVtableMemory(Type vtableType, int size)
		{
			return RuntimeHelpers.AllocateTypeAssociatedMemory(vtableType, size);
		}

		// Token: 0x0400058C RID: 1420
		private static readonly ConcurrentDictionary<string, Func<IInspectable, object>> TypedObjectFactoryCache = new ConcurrentDictionary<string, Func<IInspectable, object>>();

		// Token: 0x0400058D RID: 1421
		private static readonly Guid IID_IAgileObject = Guid.Parse("94ea2b94-e9cc-49e0-c0ff-ee64ca8f5b90");

		// Token: 0x0400058E RID: 1422
		public static readonly ConditionalWeakTable<object, ComWrappersSupport.InspectableInfo> InspectableInfoTable = new ConditionalWeakTable<object, ComWrappersSupport.InspectableInfo>();

		// Token: 0x02000885 RID: 2181
		internal class InspectableInfo
		{
			// Token: 0x17001D78 RID: 7544
			// (get) Token: 0x0600801A RID: 32794 RVA: 0x00321CF8 File Offset: 0x00320CF8
			public Guid[] IIDs { get; }

			// Token: 0x17001D79 RID: 7545
			// (get) Token: 0x0600801B RID: 32795 RVA: 0x002356D9 File Offset: 0x002346D9
			public string RuntimeClassName
			{
				get
				{
					return "";
				}
			}

			// Token: 0x0600801C RID: 32796 RVA: 0x00321D00 File Offset: 0x00320D00
			public InspectableInfo(Type type, Guid[] iids)
			{
				this.IIDs = iids;
			}
		}
	}
}
