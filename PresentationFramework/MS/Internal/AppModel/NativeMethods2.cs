using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002B4 RID: 692
	internal static class NativeMethods2
	{
		// Token: 0x060019F7 RID: 6647
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void SHAddToRecentDocsString(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		// Token: 0x060019F8 RID: 6648
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void SHAddToRecentDocs_ShellLink(SHARD uFlags, IShellLinkW pv);

		// Token: 0x060019F9 RID: 6649 RVA: 0x001629D8 File Offset: 0x001619D8
		internal static void SHAddToRecentDocs(string path)
		{
			NativeMethods2.SHAddToRecentDocsString(SHARD.PATHW, path);
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x001629E1 File Offset: 0x001619E1
		internal static void SHAddToRecentDocs(IShellLinkW shellLink)
		{
			NativeMethods2.SHAddToRecentDocs_ShellLink(SHARD.LINK, shellLink);
		}

		// Token: 0x060019FB RID: 6651
		[DllImport("shell32.dll")]
		internal static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x060019FC RID: 6652
		[DllImport("shell32.dll")]
		internal static extern HRESULT SHGetFolderPathEx([In] ref Guid rfid, KF_FLAG dwFlags, [In] [Optional] IntPtr hToken, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszPath, uint cchPath);

		// Token: 0x060019FD RID: 6653
		[DllImport("shell32.dll", PreserveSig = false)]
		internal static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		// Token: 0x060019FE RID: 6654
		[DllImport("shell32.dll")]
		internal static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
	}
}
