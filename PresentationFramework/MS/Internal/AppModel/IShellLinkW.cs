using System;
using System.Runtime.InteropServices;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002A4 RID: 676
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellLinkW
	{
		// Token: 0x06001949 RID: 6473
		void GetPath([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxPath, [In] [Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

		// Token: 0x0600194A RID: 6474
		IntPtr GetIDList();

		// Token: 0x0600194B RID: 6475
		void SetIDList(IntPtr pidl);

		// Token: 0x0600194C RID: 6476
		void GetDescription([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxName);

		// Token: 0x0600194D RID: 6477
		void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x0600194E RID: 6478
		void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszDir, int cchMaxPath);

		// Token: 0x0600194F RID: 6479
		void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

		// Token: 0x06001950 RID: 6480
		void GetArguments([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszArgs, int cchMaxPath);

		// Token: 0x06001951 RID: 6481
		void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

		// Token: 0x06001952 RID: 6482
		short GetHotKey();

		// Token: 0x06001953 RID: 6483
		void SetHotKey(short wHotKey);

		// Token: 0x06001954 RID: 6484
		uint GetShowCmd();

		// Token: 0x06001955 RID: 6485
		void SetShowCmd(uint iShowCmd);

		// Token: 0x06001956 RID: 6486
		void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

		// Token: 0x06001957 RID: 6487
		void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

		// Token: 0x06001958 RID: 6488
		void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

		// Token: 0x06001959 RID: 6489
		void Resolve(IntPtr hwnd, uint fFlags);

		// Token: 0x0600195A RID: 6490
		void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
	}
}
