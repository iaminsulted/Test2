using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020000C0 RID: 192
	[Guid("00000000-0000-0000-C000-000000000046")]
	internal struct IUnknownVftbl
	{
		// Token: 0x06000322 RID: 802 RVA: 0x000FD827 File Offset: 0x000FC827
		static IUnknownVftbl()
		{
			Marshal.StructureToPtr<IUnknownVftbl>(IUnknownVftbl.AbiToProjectionVftbl, IUnknownVftbl.AbiToProjectionVftblPtr, false);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x000FD852 File Offset: 0x000FC852
		private static IUnknownVftbl GetVftbl()
		{
			return ComWrappersSupport.IUnknownVftbl;
		}

		// Token: 0x040005EE RID: 1518
		public IUnknownVftbl._QueryInterface QueryInterface;

		// Token: 0x040005EF RID: 1519
		public IUnknownVftbl._AddRef AddRef;

		// Token: 0x040005F0 RID: 1520
		public IUnknownVftbl._Release Release;

		// Token: 0x040005F1 RID: 1521
		public static readonly IUnknownVftbl AbiToProjectionVftbl = IUnknownVftbl.GetVftbl();

		// Token: 0x040005F2 RID: 1522
		public static readonly IntPtr AbiToProjectionVftblPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IUnknownVftbl>());

		// Token: 0x020008A1 RID: 2209
		// (Invoke) Token: 0x06008094 RID: 32916
		public delegate int _QueryInterface(IntPtr pThis, ref Guid iid, out IntPtr vftbl);

		// Token: 0x020008A2 RID: 2210
		// (Invoke) Token: 0x06008098 RID: 32920
		internal delegate uint _AddRef(IntPtr pThis);

		// Token: 0x020008A3 RID: 2211
		// (Invoke) Token: 0x0600809C RID: 32924
		internal delegate uint _Release(IntPtr pThis);
	}
}
