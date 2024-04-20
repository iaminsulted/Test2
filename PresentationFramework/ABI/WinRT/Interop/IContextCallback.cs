using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000093 RID: 147
	[Guid("000001da-0000-0000-C000-000000000046")]
	internal class IContextCallback : IContextCallback
	{
		// Token: 0x06000203 RID: 515 RVA: 0x000F9340 File Offset: 0x000F8340
		public static ObjectReference<IContextCallback.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IContextCallback.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000F9348 File Offset: 0x000F8348
		public static implicit operator IContextCallback(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IContextCallback(obj);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000F9355 File Offset: 0x000F8355
		public static implicit operator IContextCallback(ObjectReference<IContextCallback.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IContextCallback(obj);
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000F9362 File Offset: 0x000F8362
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000F936F File Offset: 0x000F836F
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000F937C File Offset: 0x000F837C
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000F9389 File Offset: 0x000F8389
		public IContextCallback(IObjectReference obj) : this(obj.As<IContextCallback.Vftbl>())
		{
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000F9397 File Offset: 0x000F8397
		public IContextCallback(ObjectReference<IContextCallback.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000F93A6 File Offset: 0x000F83A6
		public unsafe void ContextCallback(PFNCONTEXTCALL pfnCallback, ComCallData* pParam, Guid riid, int iMethod)
		{
			Marshal.ThrowExceptionForHR(this._obj.Vftbl.ContextCallback_4(this.ThisPtr, pfnCallback, pParam, ref riid, iMethod, IntPtr.Zero));
		}

		// Token: 0x0400057B RID: 1403
		protected readonly ObjectReference<IContextCallback.Vftbl> _obj;

		// Token: 0x02000881 RID: 2177
		[Guid("000001da-0000-0000-C000-000000000046")]
		internal struct Vftbl
		{
			// Token: 0x04003BCE RID: 15310
			private IUnknownVftbl IUnknownVftbl;

			// Token: 0x04003BCF RID: 15311
			public IContextCallback.Vftbl._ContextCallback ContextCallback_4;

			// Token: 0x02000C6E RID: 3182
			// (Invoke) Token: 0x060091F5 RID: 37365
			public unsafe delegate int _ContextCallback(IntPtr pThis, PFNCONTEXTCALL pfnCallback, ComCallData* pData, ref Guid riid, int iMethod, IntPtr pUnk);
		}

		// Token: 0x02000882 RID: 2178
		private struct ContextCallData
		{
			// Token: 0x04003BD0 RID: 15312
			public IntPtr delegateHandle;

			// Token: 0x04003BD1 RID: 15313
			public unsafe ComCallData* userData;
		}
	}
}
