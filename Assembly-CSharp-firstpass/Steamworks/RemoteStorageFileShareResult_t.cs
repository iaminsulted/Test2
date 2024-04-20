using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000109 RID: 265
	[CallbackIdentity(1307)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileShareResult_t
	{
		// Token: 0x04000450 RID: 1104
		public const int k_iCallback = 1307;

		// Token: 0x04000451 RID: 1105
		public EResult m_eResult;

		// Token: 0x04000452 RID: 1106
		public UGCHandle_t m_hFile;

		// Token: 0x04000453 RID: 1107
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchFilename;
	}
}
