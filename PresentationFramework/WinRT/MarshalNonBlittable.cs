using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x020000AE RID: 174
	internal class MarshalNonBlittable<T> : MarshalGeneric<T>
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x000FB818 File Offset: 0x000FA818
		public unsafe static MarshalNonBlittable<T>.MarshalerArray CreateMarshalerArray(T[] array)
		{
			MarshalNonBlittable<T>.MarshalerArray m = default(MarshalNonBlittable<T>.MarshalerArray);
			if (array == null)
			{
				return m;
			}
			Func<bool> func = delegate()
			{
				m.Dispose();
				return false;
			};
			MarshalNonBlittable<T>.MarshalerArray result;
			try
			{
				int num = array.Length;
				int num2 = Marshal.SizeOf(MarshalGeneric<T>.HelperType);
				int cb = num * num2;
				m._array = Marshal.AllocCoTaskMem(cb);
				m._marshalers = new object[num];
				byte* ptr = (byte*)m._array.ToPointer();
				for (int i = 0; i < num; i++)
				{
					m._marshalers[i] = Marshaler<T>.CreateMarshaler(array[i]);
					Marshaler<T>.CopyAbi(m._marshalers[i], (IntPtr)((void*)ptr));
					ptr += num2;
				}
				result = m;
			}
			catch (Exception obj) when (func())
			{
				result = default(MarshalNonBlittable<T>.MarshalerArray);
			}
			return result;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000FB930 File Offset: 0x000FA930
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalNonBlittable<T>.MarshalerArray marshalerArray = (MarshalNonBlittable<T>.MarshalerArray)box;
			object[] marshalers = marshalerArray._marshalers;
			return new ValueTuple<int, IntPtr>((marshalers != null) ? marshalers.Length : 0, marshalerArray._array);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000FB960 File Offset: 0x000FA960
		public unsafe static T[] FromAbiArray(object box)
		{
			if (box == null)
			{
				return null;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			T[] array = new T[valueTuple.Item1];
			byte* ptr = (byte*)valueTuple.Item2.ToPointer();
			int num = Marshal.SizeOf(MarshalGeneric<T>.HelperType);
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				object arg = Marshal.PtrToStructure((IntPtr)((void*)ptr), MarshalGeneric<T>.HelperType);
				array[i] = Marshaler<T>.FromAbi(arg);
				ptr += num;
			}
			return array;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000FB9E0 File Offset: 0x000FA9E0
		public unsafe static void CopyAbiArray(T[] array, object box)
		{
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			if (valueTuple.Item2 == IntPtr.Zero)
			{
				return;
			}
			byte* ptr = (byte*)valueTuple.Item2.ToPointer();
			int num = Marshal.SizeOf(MarshalGeneric<T>.HelperType);
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				object arg = Marshal.PtrToStructure((IntPtr)((void*)ptr), MarshalGeneric<T>.HelperType);
				array[i] = Marshaler<T>.FromAbi(arg);
				ptr += num;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000FBA5C File Offset: 0x000FAA5C
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public unsafe static ValueTuple<int, IntPtr> FromManagedArray(T[] array)
		{
			if (array == null)
			{
				return new ValueTuple<int, IntPtr>(0, IntPtr.Zero);
			}
			IntPtr data = IntPtr.Zero;
			int i = 0;
			Func<bool> func = delegate()
			{
				MarshalNonBlittable<T>.DisposeAbiArray(new ValueTuple<int, IntPtr>(i, data));
				i = 0;
				data = IntPtr.Zero;
				return false;
			};
			try
			{
				int num = array.Length;
				int num2 = Marshal.SizeOf(MarshalGeneric<T>.HelperType);
				int cb = num * num2;
				data = Marshal.AllocCoTaskMem(cb);
				byte* ptr = (byte*)data.ToPointer();
				int j;
				for (i = 0; i < num; i = j + 1)
				{
					Marshaler<T>.CopyManaged(array[i], (IntPtr)((void*)ptr));
					ptr += num2;
					j = i;
				}
			}
			catch (Exception obj) when (func())
			{
				return default(ValueTuple<int, IntPtr>);
			}
			return new ValueTuple<int, IntPtr>(i, data);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000FBB60 File Offset: 0x000FAB60
		public unsafe static void CopyManagedArray(T[] array, IntPtr data)
		{
			if (array == null)
			{
				return;
			}
			MarshalNonBlittable<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(array.Length, data));
			int i = 0;
			Func<bool> func = delegate()
			{
				MarshalNonBlittable<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(i, data));
				return false;
			};
			try
			{
				int num = array.Length;
				int num2 = Marshal.SizeOf(MarshalGeneric<T>.HelperType);
				byte* ptr = (byte*)data.ToPointer();
				int j;
				for (i = 0; i < num; i = j + 1)
				{
					Marshaler<T>.CopyManaged(array[i], (IntPtr)((void*)ptr));
					ptr += num2;
					j = i;
				}
			}
			catch (Exception obj) when (func())
			{
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000FBC38 File Offset: 0x000FAC38
		public static void DisposeMarshalerArray(object box)
		{
			((MarshalNonBlittable<T>.MarshalerArray)box).Dispose();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000FBC54 File Offset: 0x000FAC54
		public unsafe static void DisposeAbiArrayElements([TupleElementNames(new string[]
		{
			"length",
			"data"
		})] ValueTuple<int, IntPtr> abi)
		{
			byte* ptr = (byte*)abi.Item2.ToPointer();
			int num = Marshal.SizeOf(MarshalGeneric<T>.HelperType);
			for (int i = 0; i < abi.Item1; i++)
			{
				object obj = Marshal.PtrToStructure((IntPtr)((void*)ptr), MarshalGeneric<T>.HelperType);
				Marshaler<T>.DisposeAbi(obj);
				ptr += num;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000FBCAC File Offset: 0x000FACAC
		public static void DisposeAbiArray(object box)
		{
			if (box == null)
			{
				return;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			if (valueTuple.Item2 == IntPtr.Zero)
			{
				return;
			}
			MarshalNonBlittable<T>.DisposeAbiArrayElements(valueTuple);
			Marshal.FreeCoTaskMem(valueTuple.Item2);
		}

		// Token: 0x02000896 RID: 2198
		internal struct MarshalerArray
		{
			// Token: 0x06008048 RID: 32840 RVA: 0x00321FAC File Offset: 0x00320FAC
			public void Dispose()
			{
				if (this._marshalers != null)
				{
					foreach (object obj in this._marshalers)
					{
						Marshaler<T>.DisposeMarshaler(obj);
					}
				}
				if (this._array != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(this._array);
				}
			}

			// Token: 0x04003BEB RID: 15339
			public IntPtr _array;

			// Token: 0x04003BEC RID: 15340
			public object[] _marshalers;
		}
	}
}
