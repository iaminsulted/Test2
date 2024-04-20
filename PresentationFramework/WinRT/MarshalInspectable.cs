using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000B1 RID: 177
	internal static class MarshalInspectable
	{
		// Token: 0x060002CA RID: 714 RVA: 0x000FC460 File Offset: 0x000FB460
		public static IObjectReference CreateMarshaler(object o, bool unwrapObject = true)
		{
			if (o == null)
			{
				return null;
			}
			IObjectReference objectReference;
			if (unwrapObject && ComWrappersSupport.TryUnwrapObject(o, out objectReference))
			{
				return objectReference.As<IInspectable.Vftbl>();
			}
			return ComWrappersSupport.CreateCCWForObject(o);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000FC48C File Offset: 0x000FB48C
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef != null)
			{
				return MarshalInterfaceHelper<object>.GetAbi(objRef);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000FC4A0 File Offset: 0x000FB4A0
		public static object FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			object result;
			using (ObjectReference<IUnknownVftbl> objectReference = ObjectReference<IUnknownVftbl>.FromAbi(ptr))
			{
				using (ObjectReference<IUnknownVftbl> objectReference2 = objectReference.As<IUnknownVftbl>())
				{
					Type type;
					if (objectReference2.IsReferenceToManagedObject)
					{
						result = ComWrappersSupport.FindObject<object>(objectReference2.ThisPtr);
					}
					else if (Projections.TryGetMarshalerTypeForProjectedRuntimeClass(objectReference, out type))
					{
						MethodInfo method = type.GetMethod("FromAbi", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (method == null)
						{
							throw new MissingMethodException();
						}
						result = method.Invoke(null, new object[]
						{
							ptr
						});
					}
					else
					{
						result = ComWrappersSupport.CreateRcwForComObject(ptr);
					}
				}
			}
			return result;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000FC554 File Offset: 0x000FB554
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			MarshalInterfaceHelper<object>.DisposeMarshaler(objRef);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000FC55C File Offset: 0x000FB55C
		public static void DisposeAbi(IntPtr ptr)
		{
			MarshalInterfaceHelper<object>.DisposeAbi(ptr);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000FC564 File Offset: 0x000FB564
		public static IntPtr FromManaged(object o, bool unwrapObject = true)
		{
			IObjectReference objectReference = MarshalInspectable.CreateMarshaler(o, unwrapObject);
			if (objectReference == null)
			{
				return IntPtr.Zero;
			}
			return objectReference.GetRef();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000FC57C File Offset: 0x000FB57C
		public unsafe static void CopyManaged(object o, IntPtr dest, bool unwrapObject = true)
		{
			IObjectReference objectReference = MarshalInspectable.CreateMarshaler(o, unwrapObject);
			*(IntPtr*)dest.ToPointer() = ((objectReference != null) ? objectReference.GetRef() : IntPtr.Zero);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000FC5A9 File Offset: 0x000FB5A9
		public static MarshalInterfaceHelper<object>.MarshalerArray CreateMarshalerArray(object[] array)
		{
			return MarshalInterfaceHelper<object>.CreateMarshalerArray(array, (object o) => MarshalInspectable.CreateMarshaler(o, true));
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000FC5D0 File Offset: 0x000FB5D0
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			return MarshalInterfaceHelper<object>.GetAbiArray(box);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000FC5D8 File Offset: 0x000FB5D8
		public static object[] FromAbiArray(object box)
		{
			return MarshalInterfaceHelper<object>.FromAbiArray(box, new Func<IntPtr, object>(MarshalInspectable.FromAbi));
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000FC5EC File Offset: 0x000FB5EC
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(object[] array)
		{
			return MarshalInterfaceHelper<object>.FromManagedArray(array, (object o) => MarshalInspectable.FromManaged(o, true));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000FC613 File Offset: 0x000FB613
		public static void CopyManagedArray(object[] array, IntPtr data)
		{
			MarshalInterfaceHelper<object>.CopyManagedArray(array, data, delegate(object o, IntPtr dest)
			{
				MarshalInspectable.CopyManaged(o, dest, true);
			});
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000FC63B File Offset: 0x000FB63B
		public static void DisposeMarshalerArray(object box)
		{
			MarshalInterfaceHelper<object>.DisposeMarshalerArray(box);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000FC643 File Offset: 0x000FB643
		public static void DisposeAbiArray(object box)
		{
			MarshalInterfaceHelper<object>.DisposeAbiArray(box);
		}
	}
}
