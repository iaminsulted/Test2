using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x020000AC RID: 172
	internal struct MarshalBlittable<T>
	{
		// Token: 0x06000292 RID: 658 RVA: 0x000FB34F File Offset: 0x000FA34F
		public static MarshalBlittable<T>.MarshalerArray CreateMarshalerArray(Array array)
		{
			return new MarshalBlittable<T>.MarshalerArray(array);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000FB358 File Offset: 0x000FA358
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> GetAbiArray(object box)
		{
			MarshalBlittable<T>.MarshalerArray marshalerArray = (MarshalBlittable<T>.MarshalerArray)box;
			return new ValueTuple<int, IntPtr>(((Array)marshalerArray._gchandle.Target).Length, marshalerArray._gchandle.AddrOfPinnedObject());
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000FB394 File Offset: 0x000FA394
		public static T[] FromAbiArray(object box)
		{
			if (box == null)
			{
				return null;
			}
			ValueTuple<int, IntPtr> valueTuple = (ValueTuple<int, IntPtr>)box;
			ReadOnlySpan<T> readOnlySpan = new ReadOnlySpan<T>(valueTuple.Item2.ToPointer(), valueTuple.Item1);
			return readOnlySpan.ToArray();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000FB3D0 File Offset: 0x000FA3D0
		[return: TupleElementNames(new string[]
		{
			"length",
			"data"
		})]
		public static ValueTuple<int, IntPtr> FromManagedArray(Array array)
		{
			if (array == null)
			{
				return new ValueTuple<int, IntPtr>(0, IntPtr.Zero);
			}
			int length = array.Length;
			IntPtr intPtr = Marshal.AllocCoTaskMem(length * Marshal.SizeOf<T>());
			MarshalBlittable<T>.CopyManagedArray(array, intPtr);
			return new ValueTuple<int, IntPtr>(length, intPtr);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000FB40C File Offset: 0x000FA40C
		public static void CopyManagedArray(Array array, IntPtr data)
		{
			if (array == null)
			{
				return;
			}
			int num = array.Length * Marshal.SizeOf<T>();
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			Buffer.MemoryCopy(gchandle.AddrOfPinnedObject().ToPointer(), data.ToPointer(), (long)num, (long)num);
			gchandle.Free();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000FB458 File Offset: 0x000FA458
		public static void DisposeMarshalerArray(object box)
		{
			if (box != null)
			{
				((MarshalBlittable<T>.MarshalerArray)box).Dispose();
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x000FB476 File Offset: 0x000FA476
		public static void DisposeAbiArray(object box)
		{
			if (box == null)
			{
				return;
			}
			Marshal.FreeCoTaskMem(((ValueTuple<int, IntPtr>)box).Item2);
		}

		// Token: 0x02000895 RID: 2197
		internal struct MarshalerArray
		{
			// Token: 0x06008046 RID: 32838 RVA: 0x00321F8E File Offset: 0x00320F8E
			public MarshalerArray(Array array)
			{
				this._gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			}

			// Token: 0x06008047 RID: 32839 RVA: 0x00321F9D File Offset: 0x00320F9D
			public void Dispose()
			{
				this._gchandle.Dispose();
			}

			// Token: 0x04003BEA RID: 15338
			public GCHandle _gchandle;
		}
	}
}
