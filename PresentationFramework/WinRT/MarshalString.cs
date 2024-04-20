using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x020000AB RID: 171
	internal class MarshalString
	{
		// Token: 0x0600027E RID: 638 RVA: 0x000FAD9E File Offset: 0x000F9D9E
		public void Dispose()
		{
			this._gchandle.Dispose();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000FADAC File Offset: 0x000F9DAC
		public unsafe static MarshalString CreateMarshaler(string value)
		{
			if (value == null)
			{
				return null;
			}
			MarshalString m = new MarshalString();
			Func<bool> func = delegate()
			{
				m.Dispose();
				return false;
			};
			MarshalString result;
			try
			{
				m._gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
				try
				{
					char* sourceString;
					if (value == null)
					{
						sourceString = null;
					}
					else
					{
						fixed (char* ptr = value.GetPinnableReference())
						{
							sourceString = ptr;
						}
					}
					fixed (MarshalString.HStringHeader* ptr2 = &m._header)
					{
						void* hstring_header = (void*)ptr2;
						fixed (IntPtr* ptr3 = &m._handle)
						{
							void* hstring = (void*)ptr3;
							Marshal.ThrowExceptionForHR(Platform.WindowsCreateStringReference(sourceString, value.Length, (IntPtr*)hstring_header, (IntPtr*)hstring));
						}
					}
				}
				finally
				{
					char* ptr = null;
					MarshalString.HStringHeader* ptr2 = null;
					IntPtr* ptr3 = null;
				}
				result = m;
			}
			catch (Exception obj) when (func())
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000FAE8C File Offset: 0x000F9E8C
		public static IntPtr GetAbi(MarshalString m)
		{
			if (m != null)
			{
				return m._handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000FAE9D File Offset: 0x000F9E9D
		public static IntPtr GetAbi(object box)
		{
			if (box != null)
			{
				return ((MarshalString)box)._handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x000FAEB3 File Offset: 0x000F9EB3
		public static void DisposeMarshaler(MarshalString m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000FAEBE File Offset: 0x000F9EBE
		public static void DisposeMarshaler(object box)
		{
			if (box != null)
			{
				MarshalString.DisposeMarshaler((MarshalString)box);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000FAECE File Offset: 0x000F9ECE
		public static void DisposeAbi(IntPtr hstring)
		{
			if (hstring != IntPtr.Zero)
			{
				Platform.WindowsDeleteString(hstring);
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000FAEE4 File Offset: 0x000F9EE4
		public static void DisposeAbi(object abi)
		{
			if (abi != null)
			{
				MarshalString.DisposeAbi((IntPtr)abi);
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x000FAEF4 File Offset: 0x000F9EF4
		public unsafe static string FromAbi(IntPtr value)
		{
			if (value == IntPtr.Zero)
			{
				return "";
			}
			uint length;
			return new string(Platform.WindowsGetStringRawBuffer(value, &length), 0, (int)length);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x000FAF24 File Offset: 0x000F9F24
		public unsafe static IntPtr FromManaged(string value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			IntPtr result;
			Marshal.ThrowExceptionForHR(Platform.WindowsCreateString(value, value.Length, &result));
			return result;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000FAF50 File Offset: 0x000F9F50
		public unsafe static MarshalString.MarshalerArray CreateMarshalerArray(string[] array)
		{
			MarshalString.MarshalerArray m = default(MarshalString.MarshalerArray);
			if (array == null)
			{
				return m;
			}
			Func<bool> func = delegate()
			{
				m.Dispose();
				return false;
			};
			MarshalString.MarshalerArray result;
			try
			{
				int num = array.Length;
				m._array = Marshal.AllocCoTaskMem(num * Marshal.SizeOf<IntPtr>());
				m._marshalers = new MarshalString[num];
				IntPtr* ptr = (IntPtr*)m._array.ToPointer();
				for (int i = 0; i < num; i++)
				{
					m._marshalers[i] = MarshalString.CreateMarshaler(array[i]);
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalString.GetAbi(m._marshalers[i]);
				}
				result = m;
			}
			catch (Exception obj) when (func())
			{
				result = default(MarshalString.MarshalerArray);
			}
			return result;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000FB050 File Offset: 0x000FA050
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalString.MarshalerArray marshalerArray = (MarshalString.MarshalerArray)box;
			MarshalString[] marshalers = marshalerArray._marshalers;
			return new ValueTuple<int, IntPtr>((marshalers != null) ? marshalers.Length : 0, marshalerArray._array);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000FB080 File Offset: 0x000FA080
		public unsafe static string[] FromAbiArray(object box)
		{
			if (box == null)
			{
				return null;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			string[] array = new string[valueTuple.Item1];
			IntPtr* ptr = (IntPtr*)valueTuple.Item2.ToPointer();
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				array[i] = MarshalString.FromAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
			return array;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000FB0D8 File Offset: 0x000FA0D8
		public unsafe static void CopyAbiArray(string[] array, object box)
		{
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			IntPtr* ptr = (IntPtr*)valueTuple.Item2.ToPointer();
			for (int i = 0; i < valueTuple.Item1; i++)
			{
				array[i] = MarshalString.FromAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000FB120 File Offset: 0x000FA120
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public unsafe static ValueTuple<int, IntPtr> FromManagedArray(string[] array)
		{
			if (array == null)
			{
				return new ValueTuple<int, IntPtr>(0, IntPtr.Zero);
			}
			IntPtr data = IntPtr.Zero;
			int i = 0;
			Func<bool> func = delegate()
			{
				MarshalString.DisposeAbiArray(new ValueTuple<int, IntPtr>(i, data));
				i = 0;
				data = IntPtr.Zero;
				return false;
			};
			try
			{
				int num = array.Length;
				data = Marshal.AllocCoTaskMem(num * Marshal.SizeOf<IntPtr>());
				IntPtr* ptr = (IntPtr*)((void*)data);
				int j;
				for (i = 0; i < num; i = j + 1)
				{
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalString.FromManaged(array[i]);
					j = i;
				}
			}
			catch (Exception obj) when (func())
			{
				return default(ValueTuple<int, IntPtr>);
			}
			return new ValueTuple<int, IntPtr>(i, data);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x000FB214 File Offset: 0x000FA214
		public unsafe static void CopyManagedArray(string[] array, IntPtr data)
		{
			if (array == null)
			{
				return;
			}
			MarshalString.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(array.Length, data));
			int i = 0;
			Func<bool> func = delegate()
			{
				MarshalString.DisposeAbiArrayElements(new ValueTuple<int, IntPtr>(i, data));
				return false;
			};
			try
			{
				int num = array.Length;
				IntPtr* ptr = (IntPtr*)((void*)data);
				int j;
				for (i = 0; i < num; i = j + 1)
				{
					ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = MarshalString.FromManaged(array[i]);
					j = i;
				}
			}
			catch (Exception obj) when (func())
			{
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x000FB2D8 File Offset: 0x000FA2D8
		public static void DisposeMarshalerArray(object box)
		{
			if (box != null)
			{
				((MarshalString.MarshalerArray)box).Dispose();
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000FB2F8 File Offset: 0x000FA2F8
		public unsafe static void DisposeAbiArrayElements([TupleElementNames(new string[]
		{
			"length",
			"data"
		})] ValueTuple<int, IntPtr> abi)
		{
			IntPtr* ptr = (IntPtr*)((void*)abi.Item2);
			for (int i = 0; i < abi.Item1; i++)
			{
				MarshalString.DisposeAbi(ptr[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000FB333 File Offset: 0x000FA333
		public static void DisposeAbiArray(object box)
		{
			if (box == null)
			{
				return;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			MarshalString.DisposeAbiArrayElements(valueTuple);
			Marshal.FreeCoTaskMem(valueTuple.Item2);
		}

		// Token: 0x040005BB RID: 1467
		public MarshalString.HStringHeader _header;

		// Token: 0x040005BC RID: 1468
		public GCHandle _gchandle;

		// Token: 0x040005BD RID: 1469
		public IntPtr _handle;

		// Token: 0x0200088F RID: 2191
		public struct HStringHeader
		{
			// Token: 0x04003BE1 RID: 15329
			[FixedBuffer(typeof(byte), 24)]
			public MarshalString.HStringHeader.<Reserved>e__FixedBuffer Reserved;

			// Token: 0x02000C72 RID: 3186
			[UnsafeValueType]
			[CompilerGenerated]
			[StructLayout(LayoutKind.Sequential, Size = 24)]
			public struct <Reserved>e__FixedBuffer
			{
				// Token: 0x04004C6F RID: 19567
				public byte FixedElementField;
			}
		}

		// Token: 0x02000890 RID: 2192
		internal struct MarshalerArray
		{
			// Token: 0x0600803D RID: 32829 RVA: 0x00321ED4 File Offset: 0x00320ED4
			public void Dispose()
			{
				if (this._marshalers != null)
				{
					foreach (MarshalString marshalString in this._marshalers)
					{
						if (marshalString != null)
						{
							marshalString.Dispose();
						}
					}
				}
				if (this._array != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(this._array);
				}
			}

			// Token: 0x04003BE2 RID: 15330
			public IntPtr _array;

			// Token: 0x04003BE3 RID: 15331
			public MarshalString[] _marshalers;
		}
	}
}
