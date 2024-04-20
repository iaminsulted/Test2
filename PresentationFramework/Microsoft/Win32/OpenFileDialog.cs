using System;
using System.IO;
using System.Windows;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Win32;

namespace Microsoft.Win32
{
	// Token: 0x020000E5 RID: 229
	public sealed class OpenFileDialog : FileDialog
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x000FEBF3 File Offset: 0x000FDBF3
		public OpenFileDialog()
		{
			this.Initialize();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000FEC04 File Offset: 0x000FDC04
		public Stream OpenFile()
		{
			string text = null;
			string[] fileNamesInternal = base.FileNamesInternal;
			if (fileNamesInternal.Length != 0)
			{
				text = fileNamesInternal[0];
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException(SR.Get("FileNameMustNotBeNull"));
			}
			return new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x000FEC44 File Offset: 0x000FDC44
		public Stream[] OpenFiles()
		{
			string[] fileNamesInternal = base.FileNamesInternal;
			Stream[] array = new Stream[fileNamesInternal.Length];
			for (int i = 0; i < fileNamesInternal.Length; i++)
			{
				string text = fileNamesInternal[i];
				if (string.IsNullOrEmpty(text))
				{
					throw new InvalidOperationException(SR.Get("FileNameMustNotBeNull"));
				}
				FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read);
				array[i] = fileStream;
			}
			return array;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000FEC9A File Offset: 0x000FDC9A
		public override void Reset()
		{
			base.Reset();
			this.Initialize();
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x000FECA8 File Offset: 0x000FDCA8
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x000FECB5 File Offset: 0x000FDCB5
		public bool Multiselect
		{
			get
			{
				return base.GetOption(512);
			}
			set
			{
				base.SetOption(512, value);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x000FECC3 File Offset: 0x000FDCC3
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x000FECCC File Offset: 0x000FDCCC
		public bool ReadOnlyChecked
		{
			get
			{
				return base.GetOption(1);
			}
			set
			{
				base.SetOption(1, value);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000FECD6 File Offset: 0x000FDCD6
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x000FECE2 File Offset: 0x000FDCE2
		public bool ShowReadOnly
		{
			get
			{
				return !base.GetOption(4);
			}
			set
			{
				base.SetOption(4, !value);
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000FECEF File Offset: 0x000FDCEF
		protected override void CheckPermissionsToShowDialog()
		{
			base.CheckPermissionsToShowDialog();
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x000FECF8 File Offset: 0x000FDCF8
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			bool openFileName = UnsafeNativeMethods.GetOpenFileName(ofn);
			if (!openFileName)
			{
				switch (UnsafeNativeMethods.CommDlgExtendedError())
				{
				case 12289:
					throw new InvalidOperationException(SR.Get("FileDialogSubClassFailure"));
				case 12290:
					throw new InvalidOperationException(SR.Get("FileDialogInvalidFileName", new object[]
					{
						base.SafeFileName
					}));
				case 12291:
					throw new InvalidOperationException(SR.Get("FileDialogBufferTooSmall"));
				}
			}
			return openFileName;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000FED74 File Offset: 0x000FDD74
		internal override string[] ProcessVistaFiles(IFileDialog dialog)
		{
			IFileOpenDialog fileOpenDialog = (IFileOpenDialog)dialog;
			if (this.Multiselect)
			{
				IShellItemArray results = fileOpenDialog.GetResults();
				uint count = results.GetCount();
				string[] array = new string[count];
				for (uint num = 0U; num < count; num += 1U)
				{
					IShellItem itemAt = results.GetItemAt(num);
					array[(int)num] = itemAt.GetDisplayName((SIGDN)2147647488U);
				}
				return array;
			}
			IShellItem result = fileOpenDialog.GetResult();
			return new string[]
			{
				result.GetDisplayName((SIGDN)2147647488U)
			};
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000FEDEE File Offset: 0x000FDDEE
		internal override IFileDialog CreateVistaDialog()
		{
			return (IFileDialog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")));
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x000FEE09 File Offset: 0x000FDE09
		private void Initialize()
		{
			base.SetOption(4096, true);
		}
	}
}
