using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MS.Internal.WindowsRuntime.ABI.System.Collections.Generic;
using MS.Internal.WindowsRuntime.Windows.Data.Text;
using WinRT;
using WinRT.Interop;

namespace MS.Internal.WindowsRuntime.ABI.Windows.Data.Text
{
	// Token: 0x02000302 RID: 770
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Guid("A5DD6357-BF2A-4C4F-A31F-29E71C6F8B35")]
	internal static class WordSegmentsTokenizingHandler
	{
		// Token: 0x06001CDA RID: 7386 RVA: 0x0016C5B8 File Offset: 0x0016B5B8
		static WordSegmentsTokenizingHandler()
		{
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(WordSegmentsTokenizingHandler), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(WordSegmentsTokenizingHandler.AbiToProjectionVftable, intPtr, false);
			WordSegmentsTokenizingHandler.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x0016C628 File Offset: 0x0016B628
		public static Delegate AbiInvokeDelegate { get; } = new WordSegmentsTokenizingHandler.Abi_Invoke(WordSegmentsTokenizingHandler.Do_Abi_Invoke);

		// Token: 0x06001CDC RID: 7388 RVA: 0x0016C62F File Offset: 0x0016B62F
		public static IObjectReference CreateMarshaler(WordSegmentsTokenizingHandler managedDelegate)
		{
			if (managedDelegate != null)
			{
				return ComWrappersSupport.CreateCCWForObject(managedDelegate).As<IDelegateVftbl>(GuidGenerator.GetIID(typeof(WordSegmentsTokenizingHandler)));
			}
			return null;
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x0016C650 File Offset: 0x0016B650
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<WordSegmentsTokenizingHandler>.GetAbi(value);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x0016C658 File Offset: 0x0016B658
		public static WordSegmentsTokenizingHandler FromAbi(IntPtr nativeDelegate)
		{
			ObjectReference<IDelegateVftbl> objectReference = ObjectReference<IDelegateVftbl>.FromAbi(nativeDelegate);
			if (objectReference != null)
			{
				return (WordSegmentsTokenizingHandler)ComWrappersSupport.TryRegisterObjectForInterface(new WordSegmentsTokenizingHandler(new WordSegmentsTokenizingHandler.NativeDelegateWrapper(objectReference).Invoke), nativeDelegate);
			}
			return null;
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x0016C68D File Offset: 0x0016B68D
		public static IntPtr FromManaged(WordSegmentsTokenizingHandler managedDelegate)
		{
			IObjectReference objectReference = WordSegmentsTokenizingHandler.CreateMarshaler(managedDelegate);
			if (objectReference == null)
			{
				return IntPtr.Zero;
			}
			return objectReference.GetRef();
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0016C6A4 File Offset: 0x0016B6A4
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<WordSegmentsTokenizingHandler>.DisposeMarshaler(value);
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0016C6AC File Offset: 0x0016B6AC
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<WordSegmentsTokenizingHandler>.DisposeAbi(abi);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x0016C6B4 File Offset: 0x0016B6B4
		private static int Do_Abi_Invoke(IntPtr thisPtr, IntPtr precedingWords, IntPtr words)
		{
			try
			{
				ComWrappersSupport.MarshalDelegateInvoke<WordSegmentsTokenizingHandler>(thisPtr, delegate(WordSegmentsTokenizingHandler invoke)
				{
					invoke(IEnumerable<WordSegment>.FromAbi(precedingWords), IEnumerable<WordSegment>.FromAbi(words));
				});
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000E7F RID: 3711
		private static readonly IDelegateVftbl AbiToProjectionVftable = new IDelegateVftbl
		{
			IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
			Invoke = Marshal.GetFunctionPointerForDelegate(WordSegmentsTokenizingHandler.AbiInvokeDelegate)
		};

		// Token: 0x04000E80 RID: 3712
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x02000A59 RID: 2649
		// (Invoke) Token: 0x060085F8 RID: 34296
		private delegate int Abi_Invoke(IntPtr thisPtr, IntPtr precedingWords, IntPtr words);

		// Token: 0x02000A5A RID: 2650
		[ObjectReferenceWrapper("_nativeDelegate")]
		private class NativeDelegateWrapper
		{
			// Token: 0x060085FB RID: 34299 RVA: 0x00329EDC File Offset: 0x00328EDC
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x060085FC RID: 34300 RVA: 0x00329EEC File Offset: 0x00328EEC
			public void Invoke(IEnumerable<WordSegment> precedingWords, IEnumerable<WordSegment> words)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				WordSegmentsTokenizingHandler.Abi_Invoke delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer<WordSegmentsTokenizingHandler.Abi_Invoke>(this._nativeDelegate.Vftbl.Invoke);
				IObjectReference objRef = null;
				IObjectReference objRef2 = null;
				try
				{
					objRef = IEnumerable<WordSegment>.CreateMarshaler(precedingWords);
					objRef2 = IEnumerable<WordSegment>.CreateMarshaler(words);
					ExceptionHelpers.ThrowExceptionForHR(delegateForFunctionPointer(thisPtr, IEnumerable<WordSegment>.GetAbi(objRef), IEnumerable<WordSegment>.GetAbi(objRef2)));
				}
				finally
				{
					IEnumerable<WordSegment>.DisposeMarshaler(objRef);
					IEnumerable<WordSegment>.DisposeMarshaler(objRef2);
				}
			}

			// Token: 0x0400412A RID: 16682
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;
		}
	}
}
