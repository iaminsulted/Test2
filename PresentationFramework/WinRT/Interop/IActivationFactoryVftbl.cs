using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020000DF RID: 223
	[Guid("00000035-0000-0000-C000-000000000046")]
	internal struct IActivationFactoryVftbl
	{
		// Token: 0x040005F3 RID: 1523
		public IInspectable.Vftbl IInspectableVftbl;

		// Token: 0x040005F4 RID: 1524
		public IActivationFactoryVftbl._ActivateInstance ActivateInstance;

		// Token: 0x020008A4 RID: 2212
		// (Invoke) Token: 0x060080A0 RID: 32928
		public delegate int _ActivateInstance(IntPtr pThis, out IntPtr instance);
	}
}
