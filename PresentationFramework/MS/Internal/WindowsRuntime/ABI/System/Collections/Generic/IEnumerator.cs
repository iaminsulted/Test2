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
	// Token: 0x020002E6 RID: 742
	[Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
	internal class IEnumerator<T> : IEnumerator<!0>, IEnumerator, IDisposable, IIterator<T>
	{
		// Token: 0x06001C04 RID: 7172 RVA: 0x0016AE65 File Offset: 0x00169E65
		public static IObjectReference CreateMarshaler(IEnumerator<T> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject(obj).As<IEnumerator<T>.Vftbl>(GuidGenerator.GetIID(typeof(IEnumerator<T>)));
			}
			return null;
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x000FC0E4 File Offset: 0x000FB0E4
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x0016AE86 File Offset: 0x00169E86
		public static IEnumerator<T> FromAbi(IntPtr thisPtr)
		{
			if (!(thisPtr == IntPtr.Zero))
			{
				return new IEnumerator<T>(IEnumerator<T>.ObjRefFromAbi(thisPtr));
			}
			return null;
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x0016AEA2 File Offset: 0x00169EA2
		internal static IIterator<T> FromAbiInternal(IntPtr thisPtr)
		{
			return new IEnumerator<T>(IEnumerator<T>.ObjRefFromAbi(thisPtr));
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x0016AEAF File Offset: 0x00169EAF
		public static IntPtr FromManaged(IEnumerator<T> value)
		{
			if (value != null)
			{
				return IEnumerator<T>.CreateMarshaler(value).GetRef();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x000FC0F5 File Offset: 0x000FB0F5
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x0016AEC5 File Offset: 0x00169EC5
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IIterator<T>>.DisposeAbi(abi);
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x0016AECD File Offset: 0x00169ECD
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IEnumerator<T>));
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0016AEE0 File Offset: 0x00169EE0
		public static ObjectReference<IEnumerator<T>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IEnumerator<T>.Vftbl vftbl = new IEnumerator<T>.Vftbl(thisPtr);
			return ObjectReference<IEnumerator<T>.Vftbl>.FromAbi(thisPtr, vftbl.IInspectableVftbl.IUnknownVftbl, vftbl);
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x0016AF16 File Offset: 0x00169F16
		public static implicit operator IEnumerator<T>(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IEnumerator<T>(obj);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x0016AF23 File Offset: 0x00169F23
		public static implicit operator IEnumerator<T>(ObjectReference<IEnumerator<T>.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IEnumerator<T>(obj);
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001C0F RID: 7183 RVA: 0x0016AF30 File Offset: 0x00169F30
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x0016AF38 File Offset: 0x00169F38
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0016AF45 File Offset: 0x00169F45
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x0016AF52 File Offset: 0x00169F52
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0016AF5F File Offset: 0x00169F5F
		public IEnumerator(IObjectReference obj) : this(obj.As<IEnumerator<T>.Vftbl>())
		{
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0016AF6D File Offset: 0x00169F6D
		public IEnumerator(ObjectReference<IEnumerator<T>.Vftbl> obj)
		{
			this._obj = obj;
			this._FromIterator = new IEnumerator<T>.FromAbiHelper(this);
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0016AF88 File Offset: 0x00169F88
		public bool _MoveNext()
		{
			byte b = 0;
			ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.MoveNext_2(this.ThisPtr, out b));
			return b > 0;
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0016AFC0 File Offset: 0x00169FC0
		public uint GetMany(ref T[] items)
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
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.GetMany_3(this.ThisPtr, item, intPtr, out num));
				items = Marshaler<T>.FromAbiArray(new ValueTuple<int, IntPtr>(item, intPtr));
				result = num;
			}
			finally
			{
				Marshaler<T>.DisposeMarshalerArray(obj);
			}
			return result;
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001C17 RID: 7191 RVA: 0x0016B060 File Offset: 0x0016A060
		public T _Current
		{
			get
			{
				object[] array = new object[2];
				array[0] = this.ThisPtr;
				object[] array2 = array;
				T result;
				try
				{
					this._obj.Vftbl.get_Current_0.DynamicInvokeAbi(array2);
					result = Marshaler<T>.FromAbi(array2[1]);
				}
				finally
				{
					Marshaler<T>.DisposeAbi(array2[1]);
				}
				return result;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x0016B0C8 File Offset: 0x0016A0C8
		public bool HasCurrent
		{
			get
			{
				byte b = 0;
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.get_HasCurrent_1(this.ThisPtr, out b));
				return b > 0;
			}
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x0016B0FD File Offset: 0x0016A0FD
		public bool MoveNext()
		{
			return this._FromIterator.MoveNext();
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x0016B10A File Offset: 0x0016A10A
		public void Reset()
		{
			this._FromIterator.Reset();
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0016B117 File Offset: 0x0016A117
		public void Dispose()
		{
			this._FromIterator.Dispose();
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001C1C RID: 7196 RVA: 0x0016B124 File Offset: 0x0016A124
		public T Current
		{
			get
			{
				return this._FromIterator.Current;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001C1D RID: 7197 RVA: 0x0016B131 File Offset: 0x0016A131
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x04000E6E RID: 3694
		public static Guid PIID = IEnumerator<T>.Vftbl.PIID;

		// Token: 0x04000E6F RID: 3695
		protected readonly ObjectReference<IEnumerator<T>.Vftbl> _obj;

		// Token: 0x04000E70 RID: 3696
		private IEnumerator<T>.FromAbiHelper _FromIterator;

		// Token: 0x02000A25 RID: 2597
		public class FromAbiHelper : IEnumerator<!0>, IEnumerator, IDisposable
		{
			// Token: 0x06008510 RID: 34064 RVA: 0x00327E88 File Offset: 0x00326E88
			public FromAbiHelper(IObjectReference obj) : this(new IEnumerator<T>(obj))
			{
			}

			// Token: 0x06008511 RID: 34065 RVA: 0x00327E96 File Offset: 0x00326E96
			internal FromAbiHelper(IIterator<T> iterator)
			{
				this._iterator = iterator;
			}

			// Token: 0x17001DEB RID: 7659
			// (get) Token: 0x06008512 RID: 34066 RVA: 0x00327EAC File Offset: 0x00326EAC
			public T Current
			{
				get
				{
					if (!this.m_isInitialized)
					{
						throw new InvalidOperationException(ErrorStrings.InvalidOperation_EnumNotStarted);
					}
					if (!this.m_hadCurrent)
					{
						throw new InvalidOperationException(ErrorStrings.InvalidOperation_EnumEnded);
					}
					return this.m_current;
				}
			}

			// Token: 0x17001DEC RID: 7660
			// (get) Token: 0x06008513 RID: 34067 RVA: 0x00327EDA File Offset: 0x00326EDA
			object IEnumerator.Current
			{
				get
				{
					if (!this.m_isInitialized)
					{
						throw new InvalidOperationException(ErrorStrings.InvalidOperation_EnumNotStarted);
					}
					if (!this.m_hadCurrent)
					{
						throw new InvalidOperationException(ErrorStrings.InvalidOperation_EnumEnded);
					}
					return this.m_current;
				}
			}

			// Token: 0x06008514 RID: 34068 RVA: 0x00327F10 File Offset: 0x00326F10
			public bool MoveNext()
			{
				if (!this.m_hadCurrent)
				{
					return false;
				}
				try
				{
					if (!this.m_isInitialized)
					{
						this.m_hadCurrent = this._iterator.HasCurrent;
						this.m_isInitialized = true;
					}
					else
					{
						this.m_hadCurrent = this._iterator._MoveNext();
					}
					if (this.m_hadCurrent)
					{
						this.m_current = this._iterator._Current;
					}
				}
				catch (Exception e)
				{
					if (Marshal.GetHRForException(e) == -2147483636)
					{
						throw new InvalidOperationException(ErrorStrings.InvalidOperation_EnumFailedVersion);
					}
					throw;
				}
				return this.m_hadCurrent;
			}

			// Token: 0x06008515 RID: 34069 RVA: 0x0012F160 File Offset: 0x0012E160
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06008516 RID: 34070 RVA: 0x000F6B2C File Offset: 0x000F5B2C
			public void Dispose()
			{
			}

			// Token: 0x040040BB RID: 16571
			private readonly IIterator<T> _iterator;

			// Token: 0x040040BC RID: 16572
			private bool m_hadCurrent = true;

			// Token: 0x040040BD RID: 16573
			private T m_current;

			// Token: 0x040040BE RID: 16574
			private bool m_isInitialized;
		}

		// Token: 0x02000A26 RID: 2598
		public sealed class ToAbiHelper : IIterator<T>
		{
			// Token: 0x06008517 RID: 34071 RVA: 0x00327FA8 File Offset: 0x00326FA8
			internal ToAbiHelper(IEnumerator<T> enumerator)
			{
				this.m_enumerator = enumerator;
			}

			// Token: 0x17001DED RID: 7661
			// (get) Token: 0x06008518 RID: 34072 RVA: 0x00327FBE File Offset: 0x00326FBE
			public T _Current
			{
				get
				{
					if (this.m_firstItem)
					{
						this.m_firstItem = false;
						this._MoveNext();
					}
					if (!this.m_hasCurrent)
					{
						ExceptionHelpers.ThrowExceptionForHR(-2147483637);
					}
					return this.m_enumerator.Current;
				}
			}

			// Token: 0x17001DEE RID: 7662
			// (get) Token: 0x06008519 RID: 34073 RVA: 0x00327FF3 File Offset: 0x00326FF3
			public bool HasCurrent
			{
				get
				{
					if (this.m_firstItem)
					{
						this.m_firstItem = false;
						this._MoveNext();
					}
					return this.m_hasCurrent;
				}
			}

			// Token: 0x0600851A RID: 34074 RVA: 0x00328014 File Offset: 0x00327014
			public bool _MoveNext()
			{
				try
				{
					this.m_hasCurrent = this.m_enumerator.MoveNext();
				}
				catch (InvalidOperationException)
				{
					ExceptionHelpers.ThrowExceptionForHR(-2147483636);
				}
				return this.m_hasCurrent;
			}

			// Token: 0x0600851B RID: 34075 RVA: 0x00328058 File Offset: 0x00327058
			public uint GetMany(ref T[] items)
			{
				if (items == null)
				{
					return 0U;
				}
				int num = 0;
				while (num < items.Length && this.HasCurrent)
				{
					items[num] = this._Current;
					this._MoveNext();
					num++;
				}
				if (typeof(T) == typeof(string))
				{
					string[] array = items as string[];
					for (int i = num; i < items.Length; i++)
					{
						array[i] = string.Empty;
					}
				}
				return (uint)num;
			}

			// Token: 0x17001DEF RID: 7663
			// (get) Token: 0x0600851C RID: 34076 RVA: 0x003280D2 File Offset: 0x003270D2
			public object Current
			{
				get
				{
					return this._Current;
				}
			}

			// Token: 0x0600851D RID: 34077 RVA: 0x003280DF File Offset: 0x003270DF
			public bool MoveNext()
			{
				return this._MoveNext();
			}

			// Token: 0x040040BF RID: 16575
			private readonly IEnumerator<T> m_enumerator;

			// Token: 0x040040C0 RID: 16576
			private bool m_firstItem = true;

			// Token: 0x040040C1 RID: 16577
			private bool m_hasCurrent;
		}

		// Token: 0x02000A27 RID: 2599
		[Guid("6A79E863-4300-459A-9966-CBB660963EE1")]
		public struct Vftbl
		{
			// Token: 0x0600851E RID: 34078 RVA: 0x003280E8 File Offset: 0x003270E8
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.get_Current_0 = Marshal.GetDelegateForFunctionPointer(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)], IEnumerator<T>.Vftbl.get_Current_0_Type);
				this.get_HasCurrent_1 = Marshal.GetDelegateForFunctionPointer<_get_PropertyAsBoolean>(ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
				this.MoveNext_2 = Marshal.GetDelegateForFunctionPointer<IEnumerator_Delegates.MoveNext_2>(ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
				this.GetMany_3 = Marshal.GetDelegateForFunctionPointer<IEnumerator_Delegates.GetMany_3>(ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}

			// Token: 0x0600851F RID: 34079 RVA: 0x0032817C File Offset: 0x0032717C
			unsafe static Vftbl()
			{
				IEnumerator<T>.Vftbl.AbiToProjectionVftable = new IEnumerator<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					get_Current_0 = Delegate.CreateDelegate(IEnumerator<T>.Vftbl.get_Current_0_Type, typeof(IEnumerator<T>.Vftbl).GetMethod("Do_Abi_get_Current_0", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
					{
						Marshaler<T>.AbiType
					})),
					get_HasCurrent_1 = new _get_PropertyAsBoolean(IEnumerator<T>.Vftbl.Do_Abi_get_HasCurrent_1),
					MoveNext_2 = new IEnumerator_Delegates.MoveNext_2(IEnumerator<T>.Vftbl.Do_Abi_MoveNext_2),
					GetMany_3 = new IEnumerator_Delegates.GetMany_3(IEnumerator<T>.Vftbl.Do_Abi_GetMany_3)
				};
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IEnumerator<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(IEnumerator<T>.Vftbl.AbiToProjectionVftable.get_Current_0);
				ptr[(IntPtr)7 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<_get_PropertyAsBoolean>(IEnumerator<T>.Vftbl.AbiToProjectionVftable.get_HasCurrent_1);
				ptr[(IntPtr)8 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<IEnumerator_Delegates.MoveNext_2>(IEnumerator<T>.Vftbl.AbiToProjectionVftable.MoveNext_2);
				ptr[(IntPtr)9 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<IEnumerator_Delegates.GetMany_3>(IEnumerator<T>.Vftbl.AbiToProjectionVftable.GetMany_3);
				IEnumerator<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06008520 RID: 34080 RVA: 0x00328314 File Offset: 0x00327314
			private static IEnumerator<T>.ToAbiHelper FindAdapter(IntPtr thisPtr)
			{
				IEnumerator<T> key = ComWrappersSupport.FindObject<IEnumerator<T>>(thisPtr);
				return IEnumerator<T>.Vftbl._adapterTable.GetValue(key, (IEnumerator<T> enumerator) => new IEnumerator<T>.ToAbiHelper(enumerator));
			}

			// Token: 0x06008521 RID: 34081 RVA: 0x00328354 File Offset: 0x00327354
			private static int Do_Abi_MoveNext_2(IntPtr thisPtr, out byte __return_value__)
			{
				__return_value__ = 0;
				try
				{
					__return_value__ = (IEnumerator<T>.Vftbl.FindAdapter(thisPtr)._MoveNext() ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008522 RID: 34082 RVA: 0x003283A0 File Offset: 0x003273A0
			private static int Do_Abi_GetMany_3(IntPtr thisPtr, int __itemsSize, IntPtr items, out uint __return_value__)
			{
				__return_value__ = 0U;
				T[] arg = Marshaler<T>.FromAbiArray(new ValueTuple<int, IntPtr>(__itemsSize, items));
				try
				{
					uint many = IEnumerator<T>.Vftbl.FindAdapter(thisPtr).GetMany(ref arg);
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

			// Token: 0x06008523 RID: 34083 RVA: 0x0032840C File Offset: 0x0032740C
			private unsafe static int Do_Abi_get_Current_0<TAbi>(void* thisPtr, out TAbi __return_value__)
			{
				T arg = default(T);
				__return_value__ = default(TAbi);
				try
				{
					arg = IEnumerator<T>.Vftbl.FindAdapter(new IntPtr(thisPtr))._Current;
					__return_value__ = (TAbi)((object)Marshaler<T>.FromManaged(arg));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06008524 RID: 34084 RVA: 0x00328474 File Offset: 0x00327474
			private static int Do_Abi_get_HasCurrent_1(IntPtr thisPtr, out byte __return_value__)
			{
				__return_value__ = 0;
				try
				{
					__return_value__ = (IEnumerator<T>.Vftbl.FindAdapter(thisPtr).HasCurrent ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040C2 RID: 16578
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040C3 RID: 16579
			public Delegate get_Current_0;

			// Token: 0x040040C4 RID: 16580
			internal _get_PropertyAsBoolean get_HasCurrent_1;

			// Token: 0x040040C5 RID: 16581
			public IEnumerator_Delegates.MoveNext_2 MoveNext_2;

			// Token: 0x040040C6 RID: 16582
			public IEnumerator_Delegates.GetMany_3 GetMany_3;

			// Token: 0x040040C7 RID: 16583
			public static Guid PIID = GuidGenerator.CreateIID(typeof(IEnumerator<T>));

			// Token: 0x040040C8 RID: 16584
			private static readonly Type get_Current_0_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				Marshaler<T>.AbiType.MakeByRefType(),
				typeof(int)
			});

			// Token: 0x040040C9 RID: 16585
			private static readonly IEnumerator<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x040040CA RID: 16586
			public static readonly IntPtr AbiToProjectionVftablePtr;

			// Token: 0x040040CB RID: 16587
			private static ConditionalWeakTable<IEnumerator<T>, IEnumerator<T>.ToAbiHelper> _adapterTable = new ConditionalWeakTable<IEnumerator<T>, IEnumerator<T>.ToAbiHelper>();
		}
	}
}
