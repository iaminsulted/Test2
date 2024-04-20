using System;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x020000AA RID: 170
	internal static class MarshalExtensions
	{
		// Token: 0x0600027D RID: 637 RVA: 0x000FAD8C File Offset: 0x000F9D8C
		public static void Dispose(this GCHandle handle)
		{
			if (handle.IsAllocated)
			{
				handle.Free();
			}
		}
	}
}
