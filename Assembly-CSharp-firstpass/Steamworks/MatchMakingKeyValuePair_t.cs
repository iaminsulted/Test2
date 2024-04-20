using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AB RID: 427
	public struct MatchMakingKeyValuePair_t
	{
		// Token: 0x06000D0E RID: 3342 RVA: 0x00029918 File Offset: 0x00027B18
		private MatchMakingKeyValuePair_t(string strKey, string strValue)
		{
			this.m_szKey = strKey;
			this.m_szValue = strValue;
		}

		// Token: 0x04000A23 RID: 2595
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szKey;

		// Token: 0x04000A24 RID: 2596
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string m_szValue;
	}
}
