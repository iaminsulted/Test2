using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Foundation.Collections;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.ABI.System.Collections.Generic
{
	// Token: 0x020002E8 RID: 744
	[Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
	internal class IReadOnlyList<T> : IReadOnlyList<T>, IEnumerable<!0>, IEnumerable, IReadOnlyCollection<T>
	{
		// Token: 0x06001C1F RID: 7199 RVA: 0x0016B14A File Offset: 0x0016A14A
		public static IObjectReference CreateMarshaler(IReadOnlyList<T> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject(obj).As<IReadOnlyList<T>.Vftbl>(GuidGenerator.GetIID(typeof(IReadOnlyList<T>)));
			}
			return null;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000FC0E4 File Offset: 0x000FB0E4
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x0016B16B File Offset: 0x0016A16B
		public static IReadOnlyList<T> FromAbi(IntPtr thisPtr)
		{
			if (!(thisPtr == IntPtr.Zero))
			{
				return new IReadOnlyList<T>(IReadOnlyList<T>.ObjRefFromAbi(thisPtr));
			}
			return null;
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0016B187 File Offset: 0x0016A187
		public static IntPtr FromManaged(IReadOnlyList<T> value)
		{
			if (value != null)
			{
				return IReadOnlyList<T>.CreateMarshaler(value).GetRef();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000FC0F5 File Offset: 0x000FB0F5
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0016B19D File Offset: 0x0016A19D
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IVectorView<T>>.DisposeAbi(abi);
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x0016B1A5 File Offset: 0x0016A1A5
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IReadOnlyList<T>));
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x0016B1B8 File Offset: 0x0016A1B8
		public static ObjectReference<IReadOnlyList<T>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IReadOnlyList<T>.Vftbl vftbl = new IReadOnlyList<T>.Vftbl(thisPtr);
			return ObjectReference<IReadOnlyList<T>.Vftbl>.FromAbi(thisPtr, vftbl.IInspectableVftbl.IUnknownVftbl, vftbl);
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x0016B1EE File Offset: 0x0016A1EE
		public static implicit operator IReadOnlyList<T>(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IReadOnlyList<T>(obj);
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0016B1FB File Offset: 0x0016A1FB
		public static implicit operator IReadOnlyList<T>(ObjectReference<IReadOnlyList<T>.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IReadOnlyList<T>(obj);
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x0016B208 File Offset: 0x0016A208
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x0016B210 File Offset: 0x0016A210
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0016B21D File Offset: 0x0016A21D
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0016B22A File Offset: 0x0016A22A
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0016B237 File Offset: 0x0016A237
		public IReadOnlyList(IObjectReference obj) : this(obj.As<IReadOnlyList<T>.Vftbl>())
		{
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x0016B245 File Offset: 0x0016A245
		public IReadOnlyList(ObjectReference<IReadOnlyList<T>.Vftbl> obj)
		{
			this._obj = obj;
			this._FromVectorView = new IReadOnlyList<T>.FromAbiHelper(this);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x0016B260 File Offset: 0x0016A260
		public T GetAt(uint index)
		{
			object[] array = new object[3];
			array[0] = this.ThisPtr;
			array[1] = index;
			object[] array2 = array;
			T result;
			try
			{
				this._obj.Vftbl.GetAt_0.DynamicInvokeAbi(array2);
				result = Marshaler<T>.FromAbi(array2[2]);
			}
			finally
			{
				Marshaler<T>.DisposeAbi(array2[2]);
			}
			return result;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0016B2D0 File Offset: 0x0016A2D0
		public bool IndexOf(T value, out uint index)
		{
			object obj = null;
			object[] array = new object[4];
			array[0] = this.ThisPtr;
			object[] array2 = array;
			bool result;
			try
			{
				obj = Marshaler<T>.CreateMarshaler(value);
				array2[1] = Marshaler<T>.GetAbi(obj);
				this._obj.Vftbl.IndexOf_2.DynamicInvokeAbi(array2);
				index = (uint)array2[2];
				result = ((byte)array2[3] > 0);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler(obj);
			}
			return result;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x0016B35C File Offset: 0x0016A35C
		public uint GetMany(uint startIndex, ref T[] items)
		{
			object obj = null;
			IntPtr intPtr = 0;
			uint num = 0U;
			uint result;
			try
			{
				obj = Marshaler<T>.CreateMarshalerArray(items);
				ValueTuple<int, IntPtr> valueTuple = Marshaler<T>.GetAbiArray(obj);
				int item = valueTuple.Item1;
				intPtr = valueTuple.Item2;
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetMany_3(this.ThisPtr, startIndex, item, intPtr, out num));
				items = Marshaler<T>.FromAbiArray(new ValueTuple<int, IntPtr>(item, intPtr));
				result = num;
			}
			finally
			{
				Marshaler<T>.DisposeMarshalerArray(obj);
			}
			return result;
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001C32 RID: 7218 RVA: 0x0016B3FC File Offset: 0x0016A3FC
		public uint Size
		{
			get
			{
				uint result = 0U;
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_Size_1(this.ThisPtr, out result));
				return result;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001C33 RID: 7219 RVA: 0x0016B42E File Offset: 0x0016A42E
		public int Count
		{
			get
			{
				return this._FromVectorView.Count;
			}
		}

		// Token: 0x17000523 RID: 1315
		public T this[int index]
		{
			get
			{
				return this._FromVectorView[index];
			}
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0016B449 File Offset: 0x0016A449
		public IEnumerator<T> GetEnumerator()
		{
			return this._FromVectorView.GetEnumerator();
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0016B456 File Offset: 0x0016A456
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000E71 RID: 3697
		public static Guid PIID = IReadOnlyList<T>.Vftbl.PIID;

		// Token: 0x04000E72 RID: 3698
		protected readonly ObjectReference<IReadOnlyList<T>.Vftbl> _obj;

		// Token: 0x04000E73 RID: 3699
		private IReadOnlyList<T>.FromAbiHelper _FromVectorView;

		// Token: 0x02000A2A RID: 2602
		public class FromAbiHelper : IReadOnlyList<T>, IEnumerable<!0>, IEnumerable, IReadOnlyCollection<T>
		{
			// Token: 0x0600852D RID: 34093 RVA: 0x003284C0 File Offset: 0x003274C0
			public FromAbiHelper(IObjectReference obj) : this(new IReadOnlyList<T>(obj))
			{
			}

			// Token: 0x0600852E RID: 34094 RVA: 0x003284CE File Offset: 0x003274CE
			public FromAbiHelper(IReadOnlyList<T> vectorView)
			{
				this._vectorView = vectorView;
				this._enumerable = new IEnumerable<T>(vectorView.ObjRef);
			}

			// Token: 0x17001DF0 RID: 7664
			// (get) Token: 0x0600852F RID: 34095 RVA: 0x003284F0 File Offset: 0x003274F0
			public int Count
			{
				get
				{
					uint size = this._vectorView.Size;
					if (2147483647U < size)
					{
						throw new InvalidOperationException(ErrorStrings.InvalidOperation_CollectionBackingListTooLarge);
					}
					return (int)size;
				}
			}

			// Token: 0x17001DF1 RID: 7665
			public T this[int index]
			{
				get
				{
					return this.Indexer_Get(index);
				}
			}

			// Token: 0x06008531 RID: 34097 RVA: 0x00328528 File Offset: 0x00327528
			private T Indexer_Get(int index)
			{
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				T at;
				try
				{
					at = this._vectorView.GetAt((uint)index);
				}
				catch (Exception ex)
				{
					if (-2147483637 == ex.HResult)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					throw;
				}
				return at;
			}

			// Token: 0x06008532 RID: 34098 RVA: 0x00328580 File Offset: 0x00327580
			public IEnumerator<T> GetEnumerator()
			{
				return this._enumerable.GetEnumerator();
			}

			// Token: 0x06008533 RID: 34099 RVA: 0x0032858D File Offset: 0x0032758D
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040040CC RID: 16588
			private readonly IReadOnlyList<T> _vectorView;

			// Token: 0x040040CD RID: 16589
			private readonly IEnumerable<T> _enumerable;
		}

		// Token: 0x02000A2B RID: 2603
		public sealed class ToAbiHelper : IVectorView<T>, IIterable<!0>
		{
			// Token: 0x06008534 RID: 34100 RVA: 0x00328595 File Offset: 0x00327595
			internal ToAbiHelper(IReadOnlyList<T> list)
			{
				this._list = list;
			}

			// Token: 0x06008535 RID: 34101 RVA: 0x003285A4 File Offset: 0x003275A4
			IIterator<T> IIterable<!0>.First()
			{
				return new IEnumerator<T>.ToAbiHelper(this._list.GetEnumerator());
			}

			// Token: 0x06008536 RID: 34102 RVA: 0x003285B6 File Offset: 0x003275B6
			private static void EnsureIndexInt32(uint index, int limit = 2147483647)
			{
				if (2147483647U <= index || index >= (uint)limit)
				{
					ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("index", ErrorStrings.ArgumentOutOfRange_IndexLargerThanMaxValue);
					ex.SetHResult(-2147483637);
					throw ex;
				}
			}

			// Token: 0x06008537 RID: 34103 RVA: 0x003285E0 File Offset: 0x003275E0
			public T GetAt(uint index)
			{
				IReadOnlyList<T>.ToAbiHelper.EnsureIndexInt32(index, this._list.Count);
				T result;
				try
				{
					result = this._list[(int)index];
				}
				catch (ArgumentOutOfRangeException ex)
				{
					ex.SetHResult(-2147483637);
					throw;
				}
				return result;
			}

			// Token: 0x17001DF2 RID: 7666
			// (get) Token: 0x06008538 RID: 34104 RVA: 0x0032862C File Offset: 0x0032762C
			public uint Size
			{
				get
				{
					return (uint)this._list.Count;
				}
			}

			// Token: 0x06008539 RID: 34105 RVA: 0x0032863C File Offset: 0x0032763C
			public bool IndexOf(T value, out uint index)
			{
				int num = -1;
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<T>.Default.Equals(value, this._list[i]))
					{
						num = i;
						break;
					}
				}
				if (-1 == num)
				{
					index = 0U;
					return false;
				}
				index = (uint)num;
				return true;
			}

			// Token: 0x0600853A RID: 34106 RVA: 0x00328690 File Offset: 0x00327690
			public uint GetMany(uint startIndex, ref T[] items)
			{
				if ((ulong)startIndex == (ulong)((long)this._list.Count))
				{
					return 0U;
				}
				IReadOnlyList<T>.ToAbiHelper.EnsureIndexInt32(startIndex, this._list.Count);
				if (items == null)
				{
					return 0U;
				}
				uint num = Math.Min((uint)items.Length, (uint)(this._list.Count - (int)startIndex));
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					items[(int)num2] = this._list[(int)(num2 + startIndex)];
				}
				if (typeof(T) == typeof(string))
				{
					string[] array = items as string[];
					uint num3 = num;
					while ((ulong)num3 < (ulong)((long)items.Length))
					{
						array[(int)num3] = string.Empty;
						num3 += 1U;
					}
				}
				return num;
			}

			// Token: 0x040040CE RID: 16590
			private readonly IReadOnlyList<T> _list;
		}

		// Token: 0x02000A2C RID: 2604
		[Guid("BBE1FA4C-B0E3-4583-BAEF-1F1B2E483E56")]
		public struct Vftbl
		{
			// Token: 0x0600853B RID: 34107 RVA: 0x0032873C File Offset: 0x0032773C
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.GetAt_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IReadOnlyList<T>.Vftbl.GetAt_0_Type);
				this.get_Size_1 = Marshal.GetDelegateForFunctionPointer<_get_PropertyAsUInt32>(ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
				this.IndexOf_2 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IReadOnlyList<T>.Vftbl.IndexOf_2_Type);
				this.GetMany_3 = Marshal.GetDelegateForFunctionPointer<IReadOnlyList_Delegates.GetMany_3>(ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}

			// Token: 0x0600853C RID: 34108 RVA: 0x003287D4 File Offset: 0x003277D4
			unsafe static Vftbl()
			{
				IReadOnlyList<T>.Vftbl.AbiToProjectionVftable = new IReadOnlyList<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					GetAt_0 = Delegate.CreateDelegate(IReadOnlyList<T>.Vftbl.GetAt_0_Type, typeof(IReadOnlyList<T>.Vftbl).GetMethod("Do_Abi_GetAt_0", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					get_Size_1 = new _get_PropertyAsUInt32(IReadOnlyList<T>.Vftbl.Do_Abi_get_Size_1),
					IndexOf_2 = Delegate.CreateDelegate(IReadOnlyList<T>.Vftbl.IndexOf_2_Type, typeof(IReadOnlyList<T>.Vftbl).GetMethod("Do_Abi_IndexOf_2", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					GetMany_3 = new IReadOnlyList_Delegates.GetMany_3(IReadOnlyList<T>.Vftbl.Do_Abi_GetMany_3)
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.GetAt_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<_get_PropertyAsUInt32>(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.get_Size_1);
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.IndexOf_2);
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<IReadOnlyList_Delegates.GetMany_3>(IReadOnlyList<T>.Vftbl.AbiToProjectionVftable.GetMany_3);
				IReadOnlyList<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600853D RID: 34109 RVA: 0x003289F4 File Offset: 0x003279F4
			private static IReadOnlyList<T>.ToAbiHelper FindAdapter(IntPtr thisPtr)
			{
				IReadOnlyList<T> key = ComWrappersSupport.FindObject<IReadOnlyList<T>>(thisPtr);
				return IReadOnlyList<T>.Vftbl._adapterTable.GetValue(key, (IReadOnlyList<T> list) => new IReadOnlyList<T>.ToAbiHelper(list));
			}

			// Token: 0x0600853E RID: 34110 RVA: 0x00328A34 File Offset: 0x00327A34
			private unsafe static int Do_Abi_GetAt_0<TAbi>(void* thisPtr, uint index, out TAbi __return_value__)
			{
				T arg = default(T);
				__return_value__ = default(TAbi);
				try
				{
					arg = IReadOnlyList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).GetAt(index);
					__return_value__ = (TAbi)((object)Marshaler<T>.FromManaged(arg));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0600853F RID: 34111 RVA: 0x00328A9C File Offset: 0x00327A9C
			private unsafe static int Do_Abi_IndexOf_2<TAbi>(void* thisPtr, TAbi value, out uint index, out byte __return_value__)
			{
				index = 0U;
				__return_value__ = 0;
				uint num = 0U;
				try
				{
					bool flag = IReadOnlyList<T>.Vftbl.FindAdapter(new IntPtr(thisPtr)).IndexOf(Marshaler<T>.FromAbi(value), out num);
					index = num;
					__return_value__ = (flag ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008540 RID: 34112 RVA: 0x00328B08 File Offset: 0x00327B08
			private static int Do_Abi_GetMany_3(IntPtr thisPtr, uint startIndex, int __itemsSize, IntPtr items, out uint __return_value__)
			{
				__return_value__ = 0U;
				T[] arg = Marshaler<T>.FromAbiArray(new ValueTuple<int, IntPtr>(__itemsSize, items));
				try
				{
					uint many = IReadOnlyList<T>.Vftbl.FindAdapter(thisPtr).GetMany(startIndex, ref arg);
					Marshaler<T>.CopyManagedArray(arg, items);
					__return_value__ = many;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008541 RID: 34113 RVA: 0x00328B74 File Offset: 0x00327B74
			private static int Do_Abi_get_Size_1(IntPtr thisPtr, out uint __return_value__)
			{
				__return_value__ = 0U;
				try
				{
					uint size = IReadOnlyList<T>.Vftbl.FindAdapter(thisPtr).Size;
					__return_value__ = size;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040CF RID: 16591
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040D0 RID: 16592
			public Delegate GetAt_0;

			// Token: 0x040040D1 RID: 16593
			internal _get_PropertyAsUInt32 get_Size_1;

			// Token: 0x040040D2 RID: 16594
			public Delegate IndexOf_2;

			// Token: 0x040040D3 RID: 16595
			public IReadOnlyList_Delegates.GetMany_3 GetMany_3;

			// Token: 0x040040D4 RID: 16596
			public static Guid PIID = GuidGenerator.CreateIID(typeof(IReadOnlyList<T>));

			// Token: 0x040040D5 RID: 16597
			private static readonly Type GetAt_0_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(uint),
				Marshaler<T>.AbiType.MakeByRefType(),
				typeof(int)
			});

			// Token: 0x040040D6 RID: 16598
			private static readonly Type IndexOf_2_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				Marshaler<T>.AbiType,
				typeof(uint).MakeByRefType(),
				typeof(byte).MakeByRefType(),
				typeof(int)
			});

			// Token: 0x040040D7 RID: 16599
			private static readonly IReadOnlyList<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x040040D8 RID: 16600
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040040D9 RID: 16601
			private static ConditionalWeakTable<IReadOnlyList<T>, IReadOnlyList<T>.ToAbiHelper> _adapterTable = new ConditionalWeakTable<IReadOnlyList<T>, IReadOnlyList<T>.ToAbiHelper>();
		}
	}
}
