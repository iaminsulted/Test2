using System;

namespace ABI.System
{
	// Token: 0x02000095 RID: 149
	internal struct Char
	{
		// Token: 0x06000214 RID: 532 RVA: 0x000F93D3 File Offset: 0x000F83D3
		public static char CreateMarshaler(char value)
		{
			return value;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000F943C File Offset: 0x000F843C
		public static Char GetAbi(char value)
		{
			return new Char
			{
				value = (ushort)value
			};
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000F945A File Offset: 0x000F845A
		public static char FromAbi(Char abi)
		{
			return (char)abi.value;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000F9462 File Offset: 0x000F8462
		public unsafe static void CopyAbi(char value, IntPtr dest)
		{
			*(short*)dest.ToPointer() = (short)Char.GetAbi(value).value;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000F9477 File Offset: 0x000F8477
		public static Char FromManaged(char value)
		{
			return Char.GetAbi(value);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000F947F File Offset: 0x000F847F
		public unsafe static void CopyManaged(char arg, IntPtr dest)
		{
			*(short*)dest.ToPointer() = (short)Char.FromManaged(arg).value;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public static void DisposeMarshaler(char m)
		{
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public static void DisposeAbi(Char abi)
		{
		}

		// Token: 0x0400057D RID: 1405
		private ushort value;
	}
}
