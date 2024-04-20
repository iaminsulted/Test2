using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000AF RID: 175
	internal class MarshalInterfaceHelper<T>
	{
		// Token: 0x060002AC RID: 684 RVA: 0x000FBCF0 File Offset: 0x000FACF0
		public unsafe static MarshalInterfaceHelper<T>.MarshalerArray CreateMarshalerArray(T[] array, Func<T, IObjectReference> createMarshaler)
		{
			MarshalInterfaceHelper<T>.MarshalerArray m = default(MarshalInterfaceHelper<T>.MarshalerArray);
			if (array == null)
			{
				return m;
			}
			Func<bool> func = delegate()
			{
				m.Dispose();
				return false;
			};
			MarshalInterfaceHelper<T>.MarshalerArray result;
			try
			{
				int num = array.Length;
				int cb = num * IntPtr.Size;
				m._array = Marshal.AllocCoTaskMem(cb);
				m._marshalers = new IObjectReference[num];
				IntPtr* ptr = (IntPtr*)m._array.ToPointer();
				for (int i = 0; i < num; i++)
				{
					m._marshalers[i] = createMarshaler(array[i]);
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalInterfaceHelper<T>.GetAbi(m._marshalers[i]);
				}
				result = m;
			}
			catch (Exception obj) when (func())
			{
				result = default(MarshalInterfaceHelper<T>.MarshalerArray);
			}
			return result;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000FBDF8 File Offset: 0x000FADF8
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalInterfaceHelper<T>.MarshalerArray marshalerArray = (MarshalInterfaceHelper<T>.MarshalerArray)box;
			IObjectReference[] marshalers = marshalerArray._marshalers;
			return new ValueTuple<int, IntPtr>((marshalers != null) ? marshalers.Length : 0, marshalerArray._array);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000FBE28 File Offset: 0x000FAE28
		public unsafe static T[] FromAbiArray(object box, Func<IntPtr, T> fromAbi)
		{
			if (box == null)
			{
				return null;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			T[] array = new T[valueTuple.Item1];
			IntPtr* ptr = (IntPtr*)valueTuple.Item2.ToPointer();
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				array[i] = fromAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
			return array;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000FBE88 File Offset: 0x000FAE88
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public unsafe static ValueTuple<int, IntPtr> FromManagedArray(T[] array, Func<T, IntPtr> fromManaged)
		{
			if (array == null)
			{
				return new ValueTuple<int, IntPtr>(0, IntPtr.Zero);
			}
			IntPtr data = IntPtr.Zero;
			int i = 0;
			Func<bool> func = delegate()
			{
				MarshalInterfaceHelper<T>.DisposeAbiArray(new ValueTuple<int, IntPtr>(i, data));
				i = 0;
				data = IntPtr.Zero;
				return false;
			};
			try
			{
				int num = array.Length;
				int cb = num * IntPtr.Size;
				data = Marshal.AllocCoTaskMem(cb);
				IntPtr* ptr = (IntPtr*)data.ToPointer();
				int j;
				for (i = 0; i < num; i = j + 1)
				{
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = fromManaged(array[i]);
					j = i;
				}
			}
			catch (Exception obj) when (func())
			{
				return default(ValueTuple<int, IntPtr>);
			}
			return new ValueTuple<int, IntPtr>(i, data);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000FBF84 File Offset: 0x000FAF84
		public unsafe static void CopyManagedArray(T[] array, IntPtr data, Action<T, IntPtr> copyManaged)
		{
			if (array == null)
			{
				return;
			}
			MarshalInterfaceHelper<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(array.Length, data));
			int i = 0;
			Func<bool> func = delegate()
			{
				MarshalInterfaceHelper<T>.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(i, data));
				return false;
			};
			try
			{
				int num = array.Length;
				int size = IntPtr.Size;
				byte* ptr = (byte*)data.ToPointer();
				int j;
				for (i = 0; i < num; i = j + 1)
				{
					copyManaged(array[i], (IntPtr)((void*)ptr));
					ptr += IntPtr.Size;
					j = i;
				}
			}
			catch (Exception obj) when (func())
			{
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000FC050 File Offset: 0x000FB050
		public static void DisposeMarshalerArray(object box)
		{
			((MarshalInterfaceHelper<T>.MarshalerArray)box).Dispose();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000FC06C File Offset: 0x000FB06C
		public unsafe static void DisposeAbiArrayElements([TupleElementNames(new string[]
		{
			"length",
			"data"
		})] ValueTuple<int, IntPtr> abi)
		{
			IntPtr* ptr = (IntPtr*)abi.Item2.ToPointer();
			for (int i = 0; i < abi.Item1; i++)
			{
				MarshalInterfaceHelper<T>.DisposeAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000FC0A8 File Offset: 0x000FB0A8
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
			MarshalInterfaceHelper<T>.DisposeAbiArrayElements(valueTuple);
			Marshal.FreeCoTaskMem(valueTuple.Item2);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000FC0E4 File Offset: 0x000FB0E4
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000FC0F5 File Offset: 0x000FB0F5
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000FC100 File Offset: 0x000FB100
		public static void DisposeAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return;
			}
			ObjectReference<IUnknownVftbl>.Attach(ref ptr).Dispose();
		}

		// Token: 0x0200089A RID: 2202
		internal struct MarshalerArray
		{
			// Token: 0x0600804F RID: 32847 RVA: 0x0032205C File Offset: 0x0032105C
			public void Dispose()
			{
				if (this._marshalers != null)
				{
					IObjectReference[] marshalers = this._marshalers;
					for (int i = 0; i < marshalers.Length; i++)
					{
						MarshalInterfaceHelper<T>.DisposeMarshaler(marshalers[i]);
					}
				}
				if (this._array != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(this._array);
				}
			}

			// Token: 0x04003BF2 RID: 15346
			public IntPtr _array;

			// Token: 0x04003BF3 RID: 15347
			public IObjectReference[] _marshalers;
		}
	}
}
