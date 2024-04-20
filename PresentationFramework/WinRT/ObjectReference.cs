using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000B4 RID: 180
	internal class ObjectReference<T> : IObjectReference
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060002EC RID: 748 RVA: 0x000FCF14 File Offset: 0x000FBF14
		protected override IUnknownVftbl VftblIUnknownUnsafe
		{
			get
			{
				return this._vftblIUnknown;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000FCF1C File Offset: 0x000FBF1C
		public static ObjectReference<T> Attach(ref IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			ObjectReference<T> result = new ObjectReference<T>(thisPtr);
			thisPtr = IntPtr.Zero;
			return result;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000FCF3C File Offset: 0x000FBF3C
		private ObjectReference(IntPtr thisPtr, IUnknownVftbl vftblIUnknown, T vftblT) : base(thisPtr)
		{
			this._vftblIUnknown = vftblIUnknown;
			this.Vftbl = vftblT;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000FCF53 File Offset: 0x000FBF53
		private protected ObjectReference(IntPtr thisPtr) : this(thisPtr, ObjectReference<T>.GetVtables(thisPtr))
		{
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000FCF62 File Offset: 0x000FBF62
		private ObjectReference(IntPtr thisPtr, [TupleElementNames(new string[]
		{
			"vftblIUnknown",
			"vftblT"
		})] ValueTuple<IUnknownVftbl, T> vtables) : this(thisPtr, vtables.Item1, vtables.Item2)
		{
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000FCF78 File Offset: 0x000FBF78
		public static ObjectReference<T> FromAbi(IntPtr thisPtr, IUnknownVftbl vftblIUnknown, T vftblT)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			ObjectReference<T> objectReference = new ObjectReference<T>(thisPtr, vftblIUnknown, vftblT);
			objectReference._vftblIUnknown.AddRef(objectReference.ThisPtr);
			return objectReference;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x000FCFB8 File Offset: 0x000FBFB8
		public static ObjectReference<T> FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			ValueTuple<IUnknownVftbl, T> vtables = ObjectReference<T>.GetVtables(thisPtr);
			IUnknownVftbl item = vtables.Item1;
			T item2 = vtables.Item2;
			return ObjectReference<T>.FromAbi(thisPtr, item, item2);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000FCFF0 File Offset: 0x000FBFF0
		[return: TupleElementNames(new string[]
		{
			"vftblIUnknown",
			"vftblT"
		})]
		private static ValueTuple<IUnknownVftbl, T> GetVtables(IntPtr thisPtr)
		{
			VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
			IUnknownVftbl item = Marshal.PtrToStructure<IUnknownVftbl>(vftblPtr.Vftbl);
			T item2;
			if (typeof(T).IsGenericType)
			{
				item2 = (T)((object)typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new Type[]
				{
					typeof(IntPtr)
				}, null).Invoke(new object[]
				{
					thisPtr
				}));
			}
			else
			{
				item2 = Marshal.PtrToStructure<T>(vftblPtr.Vftbl);
			}
			return new ValueTuple<IUnknownVftbl, T>(item, item2);
		}

		// Token: 0x040005E0 RID: 1504
		private readonly IUnknownVftbl _vftblIUnknown;

		// Token: 0x040005E1 RID: 1505
		public readonly T Vftbl;
	}
}
