using System;

namespace ABI.System
{
	// Token: 0x02000094 RID: 148
	internal struct Boolean
	{
		// Token: 0x0600020C RID: 524 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public static bool CreateMarshaler(bool value)
		{
			return value;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000F93D8 File Offset: 0x000F83D8
		public static Boolean GetAbi(bool value)
		{
			return new Boolean
			{
				value = (value ? 1 : 0)
			};
		}

		// Token: 0x0600020E RID: 526 RVA: 0x000F93FD File Offset: 0x000F83FD
		public static bool FromAbi(Boolean abi)
		{
			return abi.value > 0;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000F9408 File Offset: 0x000F8408
		public unsafe static void CopyAbi(bool value, IntPtr dest)
		{
			*(byte*)dest.ToPointer() = Boolean.GetAbi(value).value;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x000F941D File Offset: 0x000F841D
		public static Boolean FromManaged(bool value)
		{
			return Boolean.GetAbi(value);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000F9425 File Offset: 0x000F8425
		public unsafe static void CopyManaged(bool arg, IntPtr dest)
		{
			*(byte*)dest.ToPointer() = Boolean.FromManaged(arg).value;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public static void DisposeMarshaler(bool m)
		{
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public static void DisposeAbi(byte abi)
		{
		}

		// Token: 0x0400057C RID: 1404
		private byte value;
	}
}
