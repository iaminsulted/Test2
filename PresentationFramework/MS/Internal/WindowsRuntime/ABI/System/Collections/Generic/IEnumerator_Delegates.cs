using System;

namespace MS.Internal.WindowsRuntime.ABI.System.Collections.Generic
{
	// Token: 0x020002E7 RID: 743
	internal static class IEnumerator_Delegates
	{
		// Token: 0x02000A28 RID: 2600
		// (Invoke) Token: 0x06008526 RID: 34086
		public delegate int MoveNext_2(IntPtr thisPtr, out byte __return_value__);

		// Token: 0x02000A29 RID: 2601
		// (Invoke) Token: 0x0600852A RID: 34090
		public delegate int GetMany_3(IntPtr thisPtr, int __itemsSize, IntPtr items, out uint __return_value__);
	}
}
