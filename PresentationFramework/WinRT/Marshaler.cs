using System;
using System.Collections.Generic;
using System.Reflection;

namespace WinRT
{
	// Token: 0x020000B2 RID: 178
	internal class Marshaler<T>
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x000FC64C File Offset: 0x000FB64C
		static Marshaler()
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle.IsArray)
			{
				throw new InvalidOperationException("Arrays may not be marshaled generically.");
			}
			if (typeFromHandle == typeof(string))
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = ((T value) => MarshalString.CreateMarshaler((string)((object)value)));
				Marshaler<T>.GetAbi = ((object box) => MarshalString.GetAbi(box));
				Marshaler<T>.FromAbi = ((object value) => (T)((object)MarshalString.FromAbi((IntPtr)value)));
				Marshaler<T>.FromManaged = ((T value) => MarshalString.FromManaged((string)((object)value)));
				Marshaler<T>.DisposeMarshaler = delegate(object box)
				{
					MarshalString.DisposeMarshaler(box);
				};
				Marshaler<T>.DisposeAbi = delegate(object box)
				{
					MarshalString.DisposeAbi(box);
				};
				Marshaler<T>.CreateMarshalerArray = ((T[] array) => MarshalString.CreateMarshalerArray((string[])array));
				Marshaler<T>.GetAbiArray = ((object box) => MarshalString.GetAbiArray(box));
				Marshaler<T>.FromAbiArray = ((object box) => (T[])MarshalString.FromAbiArray(box));
				Marshaler<T>.FromManagedArray = ((T[] array) => MarshalString.FromManagedArray((string[])array));
				Marshaler<T>.CopyManagedArray = delegate(T[] array, IntPtr data)
				{
					MarshalString.CopyManagedArray((string[])array, data);
				};
				Marshaler<T>.DisposeMarshalerArray = delegate(object box)
				{
					MarshalString.DisposeMarshalerArray(box);
				};
				Marshaler<T>.DisposeAbiArray = delegate(object box)
				{
					MarshalString.DisposeAbiArray(box);
				};
			}
			else if (typeFromHandle.IsGenericType && typeFromHandle.GetGenericTypeDefinition() == typeof(KeyValuePair<, >))
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = MarshalGeneric<T>.CreateMarshaler;
				Marshaler<T>.GetAbi = MarshalGeneric<T>.GetAbi;
				Marshaler<T>.CopyAbi = MarshalGeneric<T>.CopyAbi;
				Marshaler<T>.FromAbi = MarshalGeneric<T>.FromAbi;
				Marshaler<T>.FromManaged = MarshalGeneric<T>.FromManaged;
				Marshaler<T>.CopyManaged = MarshalGeneric<T>.CopyManaged;
				Marshaler<T>.DisposeMarshaler = MarshalGeneric<T>.DisposeMarshaler;
				Marshaler<T>.DisposeAbi = delegate(object box)
				{
				};
			}
			else if (typeFromHandle.IsValueType || typeFromHandle == typeof(Type))
			{
				Marshaler<T>.AbiType = typeFromHandle.FindHelperType();
				if (Marshaler<T>.AbiType != null && Marshaler<T>.AbiType.GetMethod("FromAbi", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) == null)
				{
					Marshaler<T>.AbiType = null;
				}
				if (Marshaler<T>.AbiType == null)
				{
					Marshaler<T>.AbiType = typeFromHandle;
					Marshaler<T>.CreateMarshaler = ((T value) => value);
					Marshaler<T>.GetAbi = ((object box) => box);
					Marshaler<T>.FromAbi = ((object value) => (T)((object)value));
					Marshaler<T>.FromManaged = ((T value) => value);
					Marshaler<T>.DisposeMarshaler = delegate(object box)
					{
					};
					Marshaler<T>.DisposeAbi = delegate(object box)
					{
					};
					Marshaler<T>.CreateMarshalerArray = ((T[] array) => MarshalBlittable<T>.CreateMarshalerArray(array));
					Marshaler<T>.GetAbiArray = ((object box) => MarshalBlittable<T>.GetAbiArray(box));
					Marshaler<T>.FromAbiArray = ((object box) => MarshalBlittable<T>.FromAbiArray(box));
					Marshaler<T>.FromManagedArray = ((T[] array) => MarshalBlittable<T>.FromManagedArray(array));
					Marshaler<T>.CopyManagedArray = delegate(T[] array, IntPtr data)
					{
						MarshalBlittable<T>.CopyManagedArray(array, data);
					};
					Marshaler<T>.DisposeMarshalerArray = delegate(object box)
					{
						MarshalBlittable<T>.DisposeMarshalerArray(box);
					};
					Marshaler<T>.DisposeAbiArray = delegate(object box)
					{
						MarshalBlittable<T>.DisposeAbiArray(box);
					};
				}
				else
				{
					Marshaler<T>.CreateMarshaler = MarshalGeneric<T>.CreateMarshaler;
					Marshaler<T>.GetAbi = MarshalGeneric<T>.GetAbi;
					Marshaler<T>.CopyAbi = MarshalGeneric<T>.CopyAbi;
					Marshaler<T>.FromAbi = MarshalGeneric<T>.FromAbi;
					Marshaler<T>.FromManaged = MarshalGeneric<T>.FromManaged;
					Marshaler<T>.CopyManaged = MarshalGeneric<T>.CopyManaged;
					Marshaler<T>.DisposeMarshaler = MarshalGeneric<T>.DisposeMarshaler;
					Marshaler<T>.DisposeAbi = delegate(object box)
					{
					};
					Marshaler<T>.CreateMarshalerArray = ((T[] array) => MarshalNonBlittable<T>.CreateMarshalerArray(array));
					Marshaler<T>.GetAbiArray = ((object box) => MarshalNonBlittable<T>.GetAbiArray(box));
					Marshaler<T>.FromAbiArray = ((object box) => MarshalNonBlittable<T>.FromAbiArray(box));
					Marshaler<T>.FromManagedArray = ((T[] array) => MarshalNonBlittable<T>.FromManagedArray(array));
					Marshaler<T>.CopyManagedArray = delegate(T[] array, IntPtr data)
					{
						MarshalNonBlittable<T>.CopyManagedArray(array, data);
					};
					Marshaler<T>.DisposeMarshalerArray = delegate(object box)
					{
						MarshalNonBlittable<T>.DisposeMarshalerArray(box);
					};
					Marshaler<T>.DisposeAbiArray = delegate(object box)
					{
						MarshalNonBlittable<T>.DisposeAbiArray(box);
					};
				}
			}
			else if (typeFromHandle.IsInterface)
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = ((T value) => MarshalInterface<T>.CreateMarshaler(value));
				Marshaler<T>.GetAbi = ((object objRef) => MarshalInterface<T>.GetAbi((IObjectReference)objRef));
				Marshaler<T>.FromAbi = ((object value) => (T)((object)MarshalInterface<T>.FromAbi((IntPtr)value)));
				Marshaler<T>.FromManaged = ((T value) => ((IObjectReference)Marshaler<T>.CreateMarshaler(value)).GetRef());
				Marshaler<T>.DisposeMarshaler = delegate(object objRef)
				{
					MarshalInterface<T>.DisposeMarshaler((IObjectReference)objRef);
				};
				Marshaler<T>.DisposeAbi = delegate(object box)
				{
					MarshalInterface<T>.DisposeAbi((IntPtr)box);
				};
			}
			else if (typeof(T) == typeof(object))
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = ((T value) => MarshalInspectable.CreateMarshaler(value, true));
				Marshaler<T>.GetAbi = ((object objRef) => MarshalInspectable.GetAbi((IObjectReference)objRef));
				Marshaler<T>.FromAbi = ((object box) => (T)((object)MarshalInspectable.FromAbi((IntPtr)box)));
				Marshaler<T>.FromManaged = ((T value) => MarshalInspectable.FromManaged(value, true));
				Marshaler<T>.CopyManaged = delegate(T value, IntPtr dest)
				{
					MarshalInspectable.CopyManaged(value, dest, true);
				};
				Marshaler<T>.DisposeMarshaler = delegate(object objRef)
				{
					MarshalInspectable.DisposeMarshaler((IObjectReference)objRef);
				};
				Marshaler<T>.DisposeAbi = delegate(object box)
				{
					MarshalInspectable.DisposeAbi((IntPtr)box);
				};
			}
			else
			{
				Marshaler<T>.AbiType = typeof(IntPtr);
				Marshaler<T>.CreateMarshaler = MarshalGeneric<T>.CreateMarshaler;
				Marshaler<T>.GetAbi = MarshalGeneric<T>.GetAbi;
				Marshaler<T>.FromAbi = MarshalGeneric<T>.FromAbi;
				Marshaler<T>.FromManaged = MarshalGeneric<T>.FromManaged;
				Marshaler<T>.CopyManaged = MarshalGeneric<T>.CopyManaged;
				Marshaler<T>.DisposeMarshaler = MarshalGeneric<T>.DisposeMarshaler;
				Marshaler<T>.DisposeAbi = delegate(object box)
				{
				};
			}
			Marshaler<T>.RefAbiType = Marshaler<T>.AbiType.MakeByRefType();
		}

		// Token: 0x040005CC RID: 1484
		public static readonly Type AbiType;

		// Token: 0x040005CD RID: 1485
		public static readonly Type RefAbiType;

		// Token: 0x040005CE RID: 1486
		public static readonly Func<T, object> CreateMarshaler;

		// Token: 0x040005CF RID: 1487
		public static readonly Func<object, object> GetAbi;

		// Token: 0x040005D0 RID: 1488
		public static readonly Action<object, IntPtr> CopyAbi;

		// Token: 0x040005D1 RID: 1489
		public static readonly Func<object, T> FromAbi;

		// Token: 0x040005D2 RID: 1490
		public static readonly Func<T, object> FromManaged;

		// Token: 0x040005D3 RID: 1491
		public static readonly Action<T, IntPtr> CopyManaged;

		// Token: 0x040005D4 RID: 1492
		public static readonly Action<object> DisposeMarshaler;

		// Token: 0x040005D5 RID: 1493
		public static readonly Action<object> DisposeAbi;

		// Token: 0x040005D6 RID: 1494
		public static readonly Func<T[], object> CreateMarshalerArray;

		// Token: 0x040005D7 RID: 1495
		public static readonly Func<object, ValueTuple<int, IntPtr>> GetAbiArray;

		// Token: 0x040005D8 RID: 1496
		public static readonly Func<object, T[]> FromAbiArray;

		// Token: 0x040005D9 RID: 1497
		public static readonly Func<T[], ValueTuple<int, IntPtr>> FromManagedArray;

		// Token: 0x040005DA RID: 1498
		public static readonly Action<T[], IntPtr> CopyManagedArray;

		// Token: 0x040005DB RID: 1499
		public static readonly Action<object> DisposeMarshalerArray;

		// Token: 0x040005DC RID: 1500
		public static readonly Action<object> DisposeAbiArray;
	}
}
