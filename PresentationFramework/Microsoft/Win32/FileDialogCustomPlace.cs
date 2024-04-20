using System;

namespace Microsoft.Win32
{
	// Token: 0x020000E3 RID: 227
	public sealed class FileDialogCustomPlace
	{
		// Token: 0x060003E4 RID: 996 RVA: 0x000FEA89 File Offset: 0x000FDA89
		public FileDialogCustomPlace(Guid knownFolder)
		{
			this.KnownFolder = knownFolder;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000FEA98 File Offset: 0x000FDA98
		public FileDialogCustomPlace(string path)
		{
			this.Path = (path ?? "");
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x000FEAB0 File Offset: 0x000FDAB0
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x000FEAB8 File Offset: 0x000FDAB8
		public Guid KnownFolder { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x000FEAC1 File Offset: 0x000FDAC1
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x000FEAC9 File Offset: 0x000FDAC9
		public string Path { get; private set; }
	}
}
