using System;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020002B3 RID: 691
	internal static class ShellUtil
	{
		// Token: 0x060019F4 RID: 6644 RVA: 0x0016290F File Offset: 0x0016190F
		public static string GetPathFromShellItem(IShellItem item)
		{
			return item.GetDisplayName((SIGDN)2147647488U);
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x0016291C File Offset: 0x0016191C
		public static string GetPathForKnownFolder(Guid knownFolder)
		{
			if (knownFolder == default(Guid))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(260);
			if (!NativeMethods2.SHGetFolderPathEx(ref knownFolder, KF_FLAG.DEFAULT, IntPtr.Zero, stringBuilder, (uint)stringBuilder.Capacity).Succeeded)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x00162970 File Offset: 0x00161970
		public static IShellItem2 GetShellItemForPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			Guid guid = new Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93");
			object obj;
			HRESULT hrLeft = NativeMethods2.SHCreateItemFromParsingName(path, null, ref guid, out obj);
			if (hrLeft == (HRESULT)Win32Error.ERROR_FILE_NOT_FOUND || hrLeft == (HRESULT)Win32Error.ERROR_PATH_NOT_FOUND)
			{
				hrLeft = HRESULT.S_OK;
				obj = null;
			}
			hrLeft.ThrowIfFailed();
			return (IShellItem2)obj;
		}
	}
}
