using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.Windows.Foundation.Collections;
using WinRT;

namespace MS.Internal.WindowsRuntime.ABI.System.Collections.Generic
{
	// Token: 0x020002E4 RID: 740
	[Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
	internal class IEnumerable<T> : IEnumerable<!0>, IEnumerable, IIterable<T>
	{
		// Token: 0x06001BF0 RID: 7152 RVA: 0x0016ACD6 File Offset: 0x00169CD6
		public static IObjectReference CreateMarshaler(IEnumerable<T> obj)
		{
			if (obj != null)
			{
				return ComWrappersSupport.CreateCCWForObject(obj).As<IEnumerable<T>.Vftbl>(GuidGenerator.GetIID(typeof(IEnumerable<T>)));
			}
			return null;
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000FC0E4 File Offset: 0x000FB0E4
		public static IntPtr GetAbi(IObjectReference objRef)
		{
			if (objRef == null)
			{
				return IntPtr.Zero;
			}
			return objRef.ThisPtr;
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x0016ACF7 File Offset: 0x00169CF7
		public static IEnumerable<T> FromAbi(IntPtr thisPtr)
		{
			if (!(thisPtr == IntPtr.Zero))
			{
				return new IEnumerable<T>(IEnumerable<T>.ObjRefFromAbi(thisPtr));
			}
			return null;
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x0016AD13 File Offset: 0x00169D13
		public static IntPtr FromManaged(IEnumerable<T> value)
		{
			if (value != null)
			{
				return IEnumerable<T>.CreateMarshaler(value).GetRef();
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000FC0F5 File Offset: 0x000FB0F5
		public static void DisposeMarshaler(IObjectReference objRef)
		{
			if (objRef != null)
			{
				objRef.Dispose();
			}
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x0016AD29 File Offset: 0x00169D29
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<IIterable<T>>.DisposeAbi(abi);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0016AD31 File Offset: 0x00169D31
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IEnumerable<T>));
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x0016AD44 File Offset: 0x00169D44
		public static ObjectReference<IEnumerable<T>.Vftbl> ObjRefFromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			IEnumerable<T>.Vftbl vftbl = new IEnumerable<T>.Vftbl(thisPtr);
			return ObjectReference<IEnumerable<T>.Vftbl>.FromAbi(thisPtr, vftbl.IInspectableVftbl.IUnknownVftbl, vftbl);
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x0016AD7A File Offset: 0x00169D7A
		public static implicit operator IEnumerable<T>(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IEnumerable<T>(obj);
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x0016AD87 File Offset: 0x00169D87
		public static implicit operator IEnumerable<T>(ObjectReference<IEnumerable<T>.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IEnumerable<T>(obj);
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001BFA RID: 7162 RVA: 0x0016AD94 File Offset: 0x00169D94
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001BFB RID: 7163 RVA: 0x0016AD9C File Offset: 0x00169D9C
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x0016ADA9 File Offset: 0x00169DA9
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x0016ADB6 File Offset: 0x00169DB6
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x0016ADC3 File Offset: 0x00169DC3
		public IEnumerable(IObjectReference obj) : this(obj.As<IEnumerable<T>.Vftbl>())
		{
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x0016ADD1 File Offset: 0x00169DD1
		public IEnumerable(ObjectReference<IEnumerable<T>.Vftbl> obj)
		{
			this._obj = obj;
			this._FromIterable = new IEnumerable<T>.FromAbiHelper(this);
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x0016ADEC File Offset: 0x00169DEC
		IIterator<T> IIterable<!0>.First()
		{
			IntPtr intPtr = 0;
			IIterator<T> result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(this._obj.Vftbl.First_0(this.ThisPtr, out intPtr));
				result = IEnumerator<T>.FromAbiInternal(intPtr);
			}
			finally
			{
				IEnumerator<T>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x0016AE44 File Offset: 0x00169E44
		public IEnumerator<T> GetEnumerator()
		{
			return this._FromIterable.GetEnumerator();
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0016AE51 File Offset: 0x00169E51
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000E6B RID: 3691
		public static Guid PIID = IEnumerable<T>.Vftbl.PIID;

		// Token: 0x04000E6C RID: 3692
		protected readonly ObjectReference<IEnumerable<T>.Vftbl> _obj;

		// Token: 0x04000E6D RID: 3693
		private IEnumerable<T>.FromAbiHelper _FromIterable;

		// Token: 0x02000A21 RID: 2593
		public class FromAbiHelper : IEnumerable<!0>, IEnumerable
		{
			// Token: 0x06008503 RID: 34051 RVA: 0x00327CDB File Offset: 0x00326CDB
			public FromAbiHelper(IObjectReference obj) : this(new IEnumerable<T>(obj))
			{
			}

			// Token: 0x06008504 RID: 34052 RVA: 0x00327CE9 File Offset: 0x00326CE9
			public FromAbiHelper(IEnumerable<T> iterable)
			{
				this._iterable = iterable;
			}

			// Token: 0x06008505 RID: 34053 RVA: 0x00327CF8 File Offset: 0x00326CF8
			public IEnumerator<T> GetEnumerator()
			{
				IEnumerator<T> enumerator = ((IIterable<!0>)this._iterable).First() as IEnumerator<T>;
				if (enumerator != null)
				{
					return enumerator;
				}
				throw new InvalidOperationException("Unexpected type for enumerator");
			}

			// Token: 0x06008506 RID: 34054 RVA: 0x00327D25 File Offset: 0x00326D25
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040040B4 RID: 16564
			private readonly IEnumerable<T> _iterable;
		}

		// Token: 0x02000A22 RID: 2594
		internal sealed class ToAbiHelper : IIterable<!0>
		{
			// Token: 0x06008507 RID: 34055 RVA: 0x00327D2D File Offset: 0x00326D2D
			internal ToAbiHelper(IEnumerable<T> enumerable)
			{
				this.m_enumerable = enumerable;
			}

			// Token: 0x06008508 RID: 34056 RVA: 0x00327D3C File Offset: 0x00326D3C
			public IIterator<T> First()
			{
				return new IEnumerator<T>.ToAbiHelper(this.m_enumerable.GetEnumerator());
			}

			// Token: 0x040040B5 RID: 16565
			private readonly IEnumerable<T> m_enumerable;
		}

		// Token: 0x02000A23 RID: 2595
		[Guid("FAA585EA-6214-4217-AFDA-7F46DE5869B3")]
		public struct Vftbl
		{
			// Token: 0x06008509 RID: 34057 RVA: 0x00327D50 File Offset: 0x00326D50
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.First_0 = Marshal.GetDelegateForFunctionPointer<IEnumerable_Delegates.First_0>(ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)]);
			}

			// Token: 0x0600850A RID: 34058 RVA: 0x00327D98 File Offset: 0x00326D98
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)Marshal.AllocCoTaskMem(Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<IInspectable.Vftbl>(IEnumerable<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<IEnumerable_Delegates.First_0>(IEnumerable<T>.Vftbl.AbiToProjectionVftable.First_0);
				IEnumerable<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600850B RID: 34059 RVA: 0x00327E3C File Offset: 0x00326E3C
			private static int Do_Abi_First_0(IntPtr thisPtr, out IntPtr __return_value__)
			{
				__return_value__ = 0;
				try
				{
					IEnumerable<T> enumerable = ComWrappersSupport.FindObject<IEnumerable<T>>(thisPtr);
					__return_value__ = MarshalInterface<IEnumerator<T>>.FromManaged(enumerable.GetEnumerator());
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x040040B6 RID: 16566
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x040040B7 RID: 16567
			public IEnumerable_Delegates.First_0 First_0;

			// Token: 0x040040B8 RID: 16568
			public static Guid PIID = GuidGenerator.CreateIID(typeof(IEnumerable<T>));

			// Token: 0x040040B9 RID: 16569
			private static readonly IEnumerable<T>.Vftbl AbiToProjectionVftable = new IEnumerable<T>.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				First_0 = new IEnumerable_Delegates.First_0(IEnumerable<T>.Vftbl.Do_Abi_First_0)
			};

			// Token: 0x040040BA RID: 16570
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
