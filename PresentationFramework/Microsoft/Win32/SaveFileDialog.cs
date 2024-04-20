using System;
using System.IO;
using System.Windows;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Win32;

namespace Microsoft.Win32
{
	// Token: 0x020000E6 RID: 230
	public sealed class SaveFileDialog : FileDialog
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x000FEE17 File Offset: 0x000FDE17
		public SaveFileDialog()
		{
			this.Initialize();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000FEE25 File Offset: 0x000FDE25
		public Stream OpenFile()
		{
			string text = (base.FileNamesInternal.Length != 0) ? base.FileNamesInternal[0] : null;
			if (string.IsNullOrEmpty(text))
			{
				throw new InvalidOperationException(SR.Get("FileNameMustNotBeNull"));
			}
			return new FileStream(text, FileMode.Create, FileAccess.ReadWrite);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000FEE5A File Offset: 0x000FDE5A
		public override void Reset()
		{
			base.Reset();
			this.Initialize();
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000FEE68 File Offset: 0x000FDE68
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x000FEE75 File Offset: 0x000FDE75
		public bool CreatePrompt
		{
			get
			{
				return base.GetOption(8192);
			}
			set
			{
				base.SetOption(8192, value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x000FEE83 File Offset: 0x000FDE83
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x000FEE8C File Offset: 0x000FDE8C
		public bool OverwritePrompt
		{
			get
			{
				return base.GetOption(2);
			}
			set
			{
				base.SetOption(2, value);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000FEE98 File Offset: 0x000FDE98
		internal override bool PromptUserIfAppropriate(string fileName)
		{
			if (!base.PromptUserIfAppropriate(fileName))
			{
				return false;
			}
			bool flag = File.Exists(Path.GetFullPath(fileName));
			return (!this.CreatePrompt || flag || this.PromptFileCreate(fileName)) && (!this.OverwritePrompt || !flag || this.PromptFileOverwrite(fileName));
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000FEEE8 File Offset: 0x000FDEE8
		internal override bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn)
		{
			bool saveFileName = UnsafeNativeMethods.GetSaveFileName(ofn);
			if (!saveFileName)
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
			return saveFileName;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x000FEF64 File Offset: 0x000FDF64
		internal override string[] ProcessVistaFiles(IFileDialog dialog)
		{
			IShellItem result = dialog.GetResult();
			return new string[]
			{
				result.GetDisplayName((SIGDN)2147647488U)
			};
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000FEF8C File Offset: 0x000FDF8C
		internal override IFileDialog CreateVistaDialog()
		{
			return (IFileDialog)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("C0B4E2F3-BA21-4773-8DBA-335EC946EB8B")));
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000FEFA7 File Offset: 0x000FDFA7
		private void Initialize()
		{
			base.SetOption(2, true);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000FEFB1 File Offset: 0x000FDFB1
		private bool PromptFileCreate(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.Get("FileDialogCreatePrompt", new object[]
			{
				fileName
			}), MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000FEFD0 File Offset: 0x000FDFD0
		private bool PromptFileOverwrite(string fileName)
		{
			return base.MessageBoxWithFocusRestore(SR.Get("FileDialogOverwritePrompt", new object[]
			{
				fileName
			}), MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
		}
	}
}
