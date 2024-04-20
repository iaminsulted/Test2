using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200009B RID: 155
	internal class WinrtModule
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000F9783 File Offset: 0x000F8783
		public static WinrtModule Instance
		{
			get
			{
				return WinrtModule._instance.Value;
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000F9790 File Offset: 0x000F8790
		public unsafe WinrtModule()
		{
			IntPtr mtaCookie;
			Marshal.ThrowExceptionForHR(Platform.CoIncrementMTAUsage(&mtaCookie));
			this._mtaCookie = mtaCookie;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000F97B8 File Offset: 0x000F87B8
		[return: TupleElementNames(new string[]
		{
			"obj",
			"hr"
		})]
		public unsafe static ValueTuple<ObjectReference<IActivationFactoryVftbl>, int> GetActivationFactory(string runtimeClassId)
		{
			WinrtModule instance = WinrtModule.Instance;
			Guid guid = typeof(IActivationFactoryVftbl).GUID;
			IntPtr intPtr;
			int num = Platform.RoGetActivationFactory(MarshalString.GetAbi(MarshalString.CreateMarshaler(runtimeClassId)), ref guid, &intPtr);
			return new ValueTuple<ObjectReference<IActivationFactoryVftbl>, int>((num == 0) ? ObjectReference<IActivationFactoryVftbl>.Attach(ref intPtr) : null, num);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000F9804 File Offset: 0x000F8804
		~WinrtModule()
		{
			Marshal.ThrowExceptionForHR(Platform.CoDecrementMTAUsage(this._mtaCookie));
		}

		// Token: 0x04000586 RID: 1414
		private readonly IntPtr _mtaCookie;

		// Token: 0x04000587 RID: 1415
		private static Lazy<WinrtModule> _instance = new Lazy<WinrtModule>();
	}
}
