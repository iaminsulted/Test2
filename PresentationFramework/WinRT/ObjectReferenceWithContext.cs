using System;
using ABI.WinRT.Interop;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020000B5 RID: 181
	internal class ObjectReferenceWithContext<T> : ObjectReference<T>
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x000FD079 File Offset: 0x000FC079
		public ObjectReferenceWithContext(IntPtr thisPtr, IntPtr contextCallbackPtr) : base(thisPtr)
		{
			this._contextCallbackPtr = contextCallbackPtr;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000FD08C File Offset: 0x000FC08C
		protected unsafe override void Release()
		{
			ComCallData comCallData = default(ComCallData);
			IntPtr contextCallbackPtr = this._contextCallbackPtr;
			new ABI.WinRT.Interop.IContextCallback(ObjectReference<ABI.WinRT.Interop.IContextCallback.Vftbl>.Attach(ref contextCallbackPtr)).ContextCallback(delegate(ComCallData* _)
			{
				base.Release();
				return 0;
			}, &comCallData, ObjectReferenceWithContext<T>.IID_ICallbackWithNoReentrancyToApplicationSTA, 5);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000FD0D0 File Offset: 0x000FC0D0
		public override int TryAs<U>(Guid iid, out ObjectReference<U> objRef)
		{
			objRef = null;
			base.ThrowIfDisposed();
			IntPtr thisPtr;
			int num = base.VftblIUnknown.QueryInterface(base.ThisPtr, ref iid, out thisPtr);
			if (num >= 0)
			{
				using (ObjectReference<ABI.WinRT.Interop.IContextCallback.Vftbl> objectReference = ObjectReference<ABI.WinRT.Interop.IContextCallback.Vftbl>.FromAbi(this._contextCallbackPtr))
				{
					objRef = new ObjectReferenceWithContext<U>(thisPtr, objectReference.GetRef());
				}
			}
			return num;
		}

		// Token: 0x040005E2 RID: 1506
		private static readonly Guid IID_ICallbackWithNoReentrancyToApplicationSTA = Guid.Parse("0A299774-3E4E-FC42-1D9D-72CEE105CA57");

		// Token: 0x040005E3 RID: 1507
		private readonly IntPtr _contextCallbackPtr;
	}
}
