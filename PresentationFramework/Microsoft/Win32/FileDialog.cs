using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Win32;

namespace Microsoft.Win32
{
	// Token: 0x020000E2 RID: 226
	public abstract class FileDialog : CommonDialog
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x000FDACD File Offset: 0x000FCACD
		protected FileDialog()
		{
			this.Initialize();
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000FDADB File Offset: 0x000FCADB
		public override void Reset()
		{
			this.Initialize();
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000FDAE3 File Offset: 0x000FCAE3
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString() + ": Title: " + this.Title + ", FileName: ");
			stringBuilder.Append(this.FileName);
			return stringBuilder.ToString();
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x000FDB17 File Offset: 0x000FCB17
		// (set) Token: 0x060003AA RID: 938 RVA: 0x000FDB24 File Offset: 0x000FCB24
		public bool AddExtension
		{
			get
			{
				return this.GetOption(int.MinValue);
			}
			set
			{
				this.SetOption(int.MinValue, value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060003AB RID: 939 RVA: 0x000FDB32 File Offset: 0x000FCB32
		// (set) Token: 0x060003AC RID: 940 RVA: 0x000FDB3F File Offset: 0x000FCB3F
		public virtual bool CheckFileExists
		{
			get
			{
				return this.GetOption(4096);
			}
			set
			{
				this.SetOption(4096, value);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060003AD RID: 941 RVA: 0x000FDB4D File Offset: 0x000FCB4D
		// (set) Token: 0x060003AE RID: 942 RVA: 0x000FDB5A File Offset: 0x000FCB5A
		public bool CheckPathExists
		{
			get
			{
				return this.GetOption(2048);
			}
			set
			{
				this.SetOption(2048, value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060003AF RID: 943 RVA: 0x000FDB68 File Offset: 0x000FCB68
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x000FDB7E File Offset: 0x000FCB7E
		public string DefaultExt
		{
			get
			{
				if (this._defaultExtension != null)
				{
					return this._defaultExtension;
				}
				return string.Empty;
			}
			set
			{
				if (value != null)
				{
					if (value.StartsWith(".", StringComparison.Ordinal))
					{
						value = value.Substring(1);
					}
					else if (value.Length == 0)
					{
						value = null;
					}
				}
				this._defaultExtension = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x000FDBAE File Offset: 0x000FCBAE
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x000FDBBE File Offset: 0x000FCBBE
		public bool DereferenceLinks
		{
			get
			{
				return !this.GetOption(1048576);
			}
			set
			{
				this.SetOption(1048576, !value);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x000FDBD0 File Offset: 0x000FCBD0
		public string SafeFileName
		{
			get
			{
				string text = Path.GetFileName(this.CriticalFileName);
				if (text == null)
				{
					text = string.Empty;
				}
				return text;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x000FDBF4 File Offset: 0x000FCBF4
		public string[] SafeFileNames
		{
			get
			{
				string[] fileNamesInternal = this.FileNamesInternal;
				string[] array = new string[fileNamesInternal.Length];
				for (int i = 0; i < fileNamesInternal.Length; i++)
				{
					array[i] = Path.GetFileName(fileNamesInternal[i]);
					if (array[i] == null)
					{
						array[i] = string.Empty;
					}
				}
				return array;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x000FDC38 File Offset: 0x000FCC38
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x000FDC40 File Offset: 0x000FCC40
		public string FileName
		{
			get
			{
				return this.CriticalFileName;
			}
			set
			{
				if (value == null)
				{
					this._fileNames = null;
					return;
				}
				this._fileNames = new string[]
				{
					value
				};
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x000FDC5D File Offset: 0x000FCC5D
		public string[] FileNames
		{
			get
			{
				return this.FileNamesInternal;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x000FDC65 File Offset: 0x000FCC65
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x000FDC7C File Offset: 0x000FCC7C
		public string Filter
		{
			get
			{
				if (this._filter != null)
				{
					return this._filter;
				}
				return string.Empty;
			}
			set
			{
				if (string.CompareOrdinal(value, this._filter) != 0)
				{
					string text = value;
					if (!string.IsNullOrEmpty(text))
					{
						if (text.Split('|', StringSplitOptions.None).Length % 2 != 0)
						{
							throw new ArgumentException(SR.Get("FileDialogInvalidFilter"));
						}
					}
					else
					{
						text = null;
					}
					this._filter = text;
				}
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000FDCC9 File Offset: 0x000FCCC9
		// (set) Token: 0x060003BB RID: 955 RVA: 0x000FDCD1 File Offset: 0x000FCCD1
		public int FilterIndex
		{
			get
			{
				return this._filterIndex;
			}
			set
			{
				this._filterIndex = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060003BC RID: 956 RVA: 0x000FDCDA File Offset: 0x000FCCDA
		// (set) Token: 0x060003BD RID: 957 RVA: 0x000FDCFA File Offset: 0x000FCCFA
		public string InitialDirectory
		{
			get
			{
				if (this._initialDirectory.Value != null)
				{
					return this._initialDirectory.Value;
				}
				return string.Empty;
			}
			set
			{
				this._initialDirectory.Value = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060003BE RID: 958 RVA: 0x000FDD08 File Offset: 0x000FCD08
		// (set) Token: 0x060003BF RID: 959 RVA: 0x000FDD11 File Offset: 0x000FCD11
		public bool RestoreDirectory
		{
			get
			{
				return this.GetOption(8);
			}
			set
			{
				this.SetOption(8, value);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x000FDD1B File Offset: 0x000FCD1B
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x000FDD3B File Offset: 0x000FCD3B
		public string Title
		{
			get
			{
				if (this._title.Value != null)
				{
					return this._title.Value;
				}
				return string.Empty;
			}
			set
			{
				this._title.Value = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000FDD49 File Offset: 0x000FCD49
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x000FDD59 File Offset: 0x000FCD59
		public bool ValidateNames
		{
			get
			{
				return !this.GetOption(256);
			}
			set
			{
				this.SetOption(256, !value);
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003C4 RID: 964 RVA: 0x000FDD6C File Offset: 0x000FCD6C
		// (remove) Token: 0x060003C5 RID: 965 RVA: 0x000FDDA4 File Offset: 0x000FCDA4
		public event CancelEventHandler FileOk;

		// Token: 0x060003C6 RID: 966 RVA: 0x000FDDDC File Offset: 0x000FCDDC
		protected override IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			IntPtr result = IntPtr.Zero;
			if (msg == 78)
			{
				this._hwndFileDialog = UnsafeNativeMethods.GetParent(new HandleRef(this, hwnd));
				NativeMethods.OFNOTIFY ofnotify = (NativeMethods.OFNOTIFY)UnsafeNativeMethods.PtrToStructure(lParam, typeof(NativeMethods.OFNOTIFY));
				switch (ofnotify.hdr_code)
				{
				case -606:
					if (this._ignoreSecondFileOkNotification)
					{
						if (this._fileOkNotificationCount != 0)
						{
							this._ignoreSecondFileOkNotification = false;
							UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, hwnd), 0, NativeMethods.InvalidIntPtr);
							result = NativeMethods.InvalidIntPtr;
							break;
						}
						this._fileOkNotificationCount = 1;
					}
					if (!this.DoFileOk(ofnotify.lpOFN))
					{
						UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(this, hwnd), 0, NativeMethods.InvalidIntPtr);
						result = NativeMethods.InvalidIntPtr;
					}
					break;
				case -604:
					this._ignoreSecondFileOkNotification = true;
					this._fileOkNotificationCount = 0;
					break;
				case -602:
				{
					NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(ofnotify.lpOFN, typeof(NativeMethods.OPENFILENAME_I));
					int num = (int)UnsafeNativeMethods.UnsafeSendMessage(this._hwndFileDialog, (WindowMessage)1124, IntPtr.Zero, IntPtr.Zero);
					if (num > openfilename_I.nMaxFile)
					{
						int num2 = num + 2048;
						NativeMethods.CharBuffer charBuffer = NativeMethods.CharBuffer.CreateBuffer(num2);
						IntPtr lpstrFile = charBuffer.AllocCoTaskMem();
						Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
						openfilename_I.lpstrFile = lpstrFile;
						openfilename_I.nMaxFile = num2;
						this._charBuffer = charBuffer;
						Marshal.StructureToPtr<NativeMethods.OPENFILENAME_I>(openfilename_I, ofnotify.lpOFN, true);
						Marshal.StructureToPtr<NativeMethods.OFNOTIFY>(ofnotify, lParam, true);
					}
					break;
				}
				case -601:
					base.MoveToScreenCenter(new HandleRef(this, this._hwndFileDialog));
					break;
				}
			}
			else
			{
				result = base.HookProc(hwnd, msg, wParam, lParam);
			}
			return result;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000FDF8A File Offset: 0x000FCF8A
		protected void OnFileOk(CancelEventArgs e)
		{
			if (this.FileOk != null)
			{
				this.FileOk(this, e);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000FDFA1 File Offset: 0x000FCFA1
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			if (this.UseVistaDialog)
			{
				return this.RunVistaDialog(hwndOwner);
			}
			return this.RunLegacyDialog(hwndOwner);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000FDFBC File Offset: 0x000FCFBC
		private bool RunLegacyDialog(IntPtr hwndOwner)
		{
			NativeMethods.WndProc lpfnHook = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.OPENFILENAME_I openfilename_I = new NativeMethods.OPENFILENAME_I();
			bool result;
			try
			{
				this._charBuffer = NativeMethods.CharBuffer.CreateBuffer(8192);
				if (this._fileNames != null)
				{
					this._charBuffer.PutString(this._fileNames[0]);
				}
				openfilename_I.lStructSize = Marshal.SizeOf(typeof(NativeMethods.OPENFILENAME_I));
				openfilename_I.hwndOwner = hwndOwner;
				openfilename_I.hInstance = IntPtr.Zero;
				openfilename_I.lpstrFilter = FileDialog.MakeFilterString(this._filter, this.DereferenceLinks);
				openfilename_I.nFilterIndex = this._filterIndex;
				openfilename_I.lpstrFile = this._charBuffer.AllocCoTaskMem();
				openfilename_I.nMaxFile = this._charBuffer.Length;
				openfilename_I.lpstrInitialDir = this._initialDirectory.Value;
				openfilename_I.lpstrTitle = this._title.Value;
				openfilename_I.Flags = (this.Options | 8912928);
				openfilename_I.lpfnHook = lpfnHook;
				openfilename_I.FlagsEx = 16777216;
				if (this._defaultExtension != null && this.AddExtension)
				{
					openfilename_I.lpstrDefExt = this._defaultExtension;
				}
				result = this.RunFileDialog(openfilename_I);
			}
			finally
			{
				this._charBuffer = null;
				if (openfilename_I.lpstrFile != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
				}
			}
			return result;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000FE124 File Offset: 0x000FD124
		internal bool GetOption(int option)
		{
			return (this._dialogOptions.Value & option) != 0;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000FE136 File Offset: 0x000FD136
		internal void SetOption(int option, bool value)
		{
			if (value)
			{
				this._dialogOptions.Value = (this._dialogOptions.Value | option);
				return;
			}
			this._dialogOptions.Value = (this._dialogOptions.Value & ~option);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000FE164 File Offset: 0x000FD164
		internal bool MessageBoxWithFocusRestore(string message, MessageBoxButton buttons, MessageBoxImage image)
		{
			bool result = false;
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			try
			{
				result = (MessageBox.Show(message, this.DialogCaption, buttons, image, MessageBoxResult.OK, MessageBoxOptions.None) == MessageBoxResult.Yes);
			}
			finally
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(this, focus));
			}
			return result;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000FE1B0 File Offset: 0x000FD1B0
		internal virtual bool PromptUserIfAppropriate(string fileName)
		{
			bool flag = true;
			if (this.GetOption(4096))
			{
				try
				{
					flag = File.Exists(Path.GetFullPath(fileName));
				}
				catch (PathTooLongException)
				{
					flag = false;
				}
				if (!flag)
				{
					this.PromptFileNotFound(fileName);
				}
			}
			return flag;
		}

		// Token: 0x060003CE RID: 974
		internal abstract bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000FE1FC File Offset: 0x000FD1FC
		internal string[] FileNamesInternal
		{
			get
			{
				if (this._fileNames == null)
				{
					return Array.Empty<string>();
				}
				return (string[])this._fileNames.Clone();
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000FE21C File Offset: 0x000FD21C
		private bool DoFileOk(IntPtr lpOFN)
		{
			NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(lpOFN, typeof(NativeMethods.OPENFILENAME_I));
			int value = this._dialogOptions.Value;
			int filterIndex = this._filterIndex;
			string[] fileNames = this._fileNames;
			bool flag = false;
			try
			{
				this._dialogOptions.Value = ((this._dialogOptions.Value & -2) | (openfilename_I.Flags & 1));
				this._filterIndex = openfilename_I.nFilterIndex;
				this._charBuffer.PutCoTaskMem(openfilename_I.lpstrFile);
				if (!this.GetOption(512))
				{
					this._fileNames = new string[]
					{
						this._charBuffer.GetString()
					};
				}
				else
				{
					this._fileNames = FileDialog.GetMultiselectFiles(this._charBuffer);
				}
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					this.OnFileOk(cancelEventArgs);
					flag = !cancelEventArgs.Cancel;
				}
			}
			finally
			{
				if (!flag)
				{
					this._dialogOptions.Value = value;
					this._filterIndex = filterIndex;
					this._fileNames = fileNames;
				}
			}
			return flag;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000FE32C File Offset: 0x000FD32C
		private static string[] GetMultiselectFiles(NativeMethods.CharBuffer charBuffer)
		{
			string text = charBuffer.GetString();
			string text2 = charBuffer.GetString();
			if (text2.Length == 0)
			{
				return new string[]
				{
					text
				};
			}
			if (!text.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
			{
				text += Path.DirectorySeparatorChar.ToString();
			}
			List<string> list = new List<string>();
			do
			{
				bool flag = text2[0] == Path.DirectorySeparatorChar && text2[1] == Path.DirectorySeparatorChar;
				bool flag2 = text2.Length > 3 && text2[1] == Path.VolumeSeparatorChar && text2[2] == Path.DirectorySeparatorChar;
				if (!flag && !flag2)
				{
					text2 = text + text2;
				}
				list.Add(text2);
				text2 = charBuffer.GetString();
			}
			while (!string.IsNullOrEmpty(text2));
			return list.ToArray();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000FE3F4 File Offset: 0x000FD3F4
		private void Initialize()
		{
			this._dialogOptions.Value = 0;
			this.SetOption(4, true);
			this.SetOption(2048, true);
			this.SetOption(int.MinValue, true);
			this._title.Value = null;
			this._initialDirectory.Value = null;
			this._defaultExtension = null;
			this._fileNames = null;
			this._filter = null;
			this._filterIndex = 1;
			this._ignoreSecondFileOkNotification = false;
			this._fileOkNotificationCount = 0;
			this.CustomPlaces = new List<FileDialogCustomPlace>();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000FE47C File Offset: 0x000FD47C
		private static string MakeFilterString(string s, bool dereferenceLinks)
		{
			if (string.IsNullOrEmpty(s))
			{
				if (!dereferenceLinks || Environment.OSVersion.Version.Major < 5)
				{
					return null;
				}
				s = " |*.*";
			}
			StringBuilder stringBuilder = new StringBuilder(s);
			stringBuilder.Replace('|', '\0');
			stringBuilder.Append('\0');
			stringBuilder.Append('\0');
			return stringBuilder.ToString();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000FE4D8 File Offset: 0x000FD4D8
		private bool ProcessFileNames()
		{
			if (!this.GetOption(256))
			{
				string[] filterExtensions = this.GetFilterExtensions();
				for (int i = 0; i < this._fileNames.Length; i++)
				{
					string text = this._fileNames[i];
					if (this.AddExtension && !Path.HasExtension(text))
					{
						for (int j = 0; j < filterExtensions.Length; j++)
						{
							Invariant.Assert(!filterExtensions[j].StartsWith(".", StringComparison.Ordinal), "FileDialog.GetFilterExtensions should not return things starting with '.'");
							string extension = Path.GetExtension(text);
							Invariant.Assert(extension.Length == 0 || extension.StartsWith(".", StringComparison.Ordinal), "Path.GetExtension should return something that starts with '.'");
							StringBuilder stringBuilder = new StringBuilder(text.Substring(0, text.Length - extension.Length));
							if (filterExtensions[j].IndexOfAny(new char[]
							{
								'*',
								'?'
							}) == -1)
							{
								stringBuilder.Append(".");
								stringBuilder.Append(filterExtensions[j]);
							}
							if (!this.GetOption(4096) || File.Exists(stringBuilder.ToString()))
							{
								text = stringBuilder.ToString();
								break;
							}
						}
						this._fileNames[i] = text;
					}
					if (!this.PromptUserIfAppropriate(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000FE617 File Offset: 0x000FD617
		private void PromptFileNotFound(string fileName)
		{
			this.MessageBoxWithFocusRestore(SR.Get("FileDialogFileNotFound", new object[]
			{
				fileName
			}), MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x000FE637 File Offset: 0x000FD637
		private string CriticalFileName
		{
			get
			{
				if (this._fileNames == null)
				{
					return string.Empty;
				}
				if (this._fileNames[0].Length > 0)
				{
					return this._fileNames[0];
				}
				return string.Empty;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x000FE668 File Offset: 0x000FD668
		private string DialogCaption
		{
			get
			{
				if (!UnsafeNativeMethods.IsWindow(new HandleRef(this, this._hwndFileDialog)))
				{
					return string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder(UnsafeNativeMethods.GetWindowTextLength(new HandleRef(this, this._hwndFileDialog)) + 1);
				UnsafeNativeMethods.GetWindowText(new HandleRef(this, this._hwndFileDialog), stringBuilder, stringBuilder.Capacity);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000FE6C8 File Offset: 0x000FD6C8
		private string[] GetFilterExtensions()
		{
			string filter = this._filter;
			List<string> list = new List<string>();
			if (this._defaultExtension != null)
			{
				list.Add(this._defaultExtension);
			}
			if (filter != null)
			{
				string[] array = filter.Split(new char[]
				{
					'|'
				}, StringSplitOptions.RemoveEmptyEntries);
				int num = this._filterIndex * 2 - 1;
				if (num >= array.Length)
				{
					throw new InvalidOperationException(SR.Get("FileDialogInvalidFilterIndex"));
				}
				if (this._filterIndex > 0)
				{
					foreach (string text in array[num].Split(';', StringSplitOptions.None))
					{
						int num2 = text.LastIndexOf('.');
						if (num2 >= 0)
						{
							list.Add(text.Substring(num2 + 1, text.Length - (num2 + 1)));
						}
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x000FE793 File Offset: 0x000FD793
		protected int Options
		{
			get
			{
				return this._dialogOptions.Value & 1051405;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060003DA RID: 986 RVA: 0x000FE7A6 File Offset: 0x000FD7A6
		// (set) Token: 0x060003DB RID: 987 RVA: 0x000FE7AE File Offset: 0x000FD7AE
		public IList<FileDialogCustomPlace> CustomPlaces { get; set; }

		// Token: 0x060003DC RID: 988
		internal abstract IFileDialog CreateVistaDialog();

		// Token: 0x060003DD RID: 989
		internal abstract string[] ProcessVistaFiles(IFileDialog dialog);

		// Token: 0x060003DE RID: 990 RVA: 0x000FE7B8 File Offset: 0x000FD7B8
		internal virtual void PrepareVistaDialog(IFileDialog dialog)
		{
			dialog.SetDefaultExtension(this.DefaultExt);
			dialog.SetFileName(this.CriticalFileName);
			if (!string.IsNullOrEmpty(this.InitialDirectory))
			{
				IShellItem shellItemForPath = ShellUtil.GetShellItemForPath(this.InitialDirectory);
				if (shellItemForPath != null)
				{
					dialog.SetDefaultFolder(shellItemForPath);
					dialog.SetFolder(shellItemForPath);
				}
			}
			dialog.SetTitle(this.Title);
			FOS options = (FOS)((this.Options & 1063690) | 536870912 | 64);
			dialog.SetOptions(options);
			COMDLG_FILTERSPEC[] filterItems = FileDialog.GetFilterItems(this.Filter);
			if (filterItems.Length != 0)
			{
				dialog.SetFileTypes((uint)filterItems.Length, filterItems);
				dialog.SetFileTypeIndex((uint)this.FilterIndex);
			}
			IList<FileDialogCustomPlace> customPlaces = this.CustomPlaces;
			if (customPlaces != null && customPlaces.Count != 0)
			{
				foreach (FileDialogCustomPlace customPlace in customPlaces)
				{
					IShellItem shellItem = FileDialog.ResolveCustomPlace(customPlace);
					if (shellItem != null)
					{
						try
						{
							dialog.AddPlace(shellItem, FDAP.BOTTOM);
						}
						catch (ArgumentException)
						{
						}
					}
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060003DF RID: 991 RVA: 0x000FE8C8 File Offset: 0x000FD8C8
		private bool UseVistaDialog
		{
			get
			{
				return Environment.OSVersion.Version.Major >= 6;
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000FE8E0 File Offset: 0x000FD8E0
		private bool RunVistaDialog(IntPtr hwndOwner)
		{
			IFileDialog fileDialog = this.CreateVistaDialog();
			this.PrepareVistaDialog(fileDialog);
			bool succeeded;
			using (new FileDialog.VistaDialogEvents(fileDialog, new FileDialog.VistaDialogEvents.OnOkCallback(this.HandleVistaFileOk)))
			{
				succeeded = fileDialog.Show(hwndOwner).Succeeded;
			}
			return succeeded;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000FE93C File Offset: 0x000FD93C
		private bool HandleVistaFileOk(IFileDialog dialog)
		{
			((UnsafeNativeMethods.IOleWindow)dialog).GetWindow(out this._hwndFileDialog);
			int value = this._dialogOptions.Value;
			int filterIndex = this._filterIndex;
			string[] fileNames = this._fileNames;
			bool flag = false;
			try
			{
				uint fileTypeIndex = dialog.GetFileTypeIndex();
				this._filterIndex = (int)fileTypeIndex;
				this._fileNames = this.ProcessVistaFiles(dialog);
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					this.OnFileOk(cancelEventArgs);
					flag = !cancelEventArgs.Cancel;
				}
			}
			finally
			{
				if (!flag)
				{
					this._fileNames = fileNames;
					this._dialogOptions.Value = value;
					this._filterIndex = filterIndex;
				}
				else if ((this.Options & 4) != 0)
				{
					this._dialogOptions.Value = (this._dialogOptions.Value & -2);
				}
			}
			return flag;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000FEA08 File Offset: 0x000FDA08
		private static COMDLG_FILTERSPEC[] GetFilterItems(string filter)
		{
			List<COMDLG_FILTERSPEC> list = new List<COMDLG_FILTERSPEC>();
			if (!string.IsNullOrEmpty(filter))
			{
				string[] array = filter.Split('|', StringSplitOptions.None);
				if (array.Length % 2 == 0)
				{
					for (int i = 1; i < array.Length; i += 2)
					{
						list.Add(new COMDLG_FILTERSPEC
						{
							pszName = array[i - 1],
							pszSpec = array[i]
						});
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000FEA6D File Offset: 0x000FDA6D
		private static IShellItem ResolveCustomPlace(FileDialogCustomPlace customPlace)
		{
			return ShellUtil.GetShellItemForPath(ShellUtil.GetPathForKnownFolder(customPlace.KnownFolder) ?? customPlace.Path);
		}

		// Token: 0x040005FC RID: 1532
		private const FOS c_VistaFileDialogMask = FOS.OVERWRITEPROMPT | FOS.NOCHANGEDIR | FOS.NOVALIDATE | FOS.ALLOWMULTISELECT | FOS.PATHMUSTEXIST | FOS.FILEMUSTEXIST | FOS.CREATEPROMPT | FOS.NODEREFERENCELINKS;

		// Token: 0x040005FD RID: 1533
		private SecurityCriticalDataForSet<int> _dialogOptions;

		// Token: 0x040005FE RID: 1534
		private bool _ignoreSecondFileOkNotification;

		// Token: 0x040005FF RID: 1535
		private int _fileOkNotificationCount;

		// Token: 0x04000600 RID: 1536
		private SecurityCriticalDataForSet<string> _title;

		// Token: 0x04000601 RID: 1537
		private SecurityCriticalDataForSet<string> _initialDirectory;

		// Token: 0x04000602 RID: 1538
		private string _defaultExtension;

		// Token: 0x04000603 RID: 1539
		private string _filter;

		// Token: 0x04000604 RID: 1540
		private int _filterIndex;

		// Token: 0x04000605 RID: 1541
		private NativeMethods.CharBuffer _charBuffer;

		// Token: 0x04000606 RID: 1542
		private IntPtr _hwndFileDialog;

		// Token: 0x04000607 RID: 1543
		private string[] _fileNames;

		// Token: 0x04000608 RID: 1544
		private const int FILEBUFSIZE = 8192;

		// Token: 0x04000609 RID: 1545
		private const int OPTION_ADDEXTENSION = -2147483648;

		// Token: 0x020008A5 RID: 2213
		private sealed class VistaDialogEvents : IFileDialogEvents, IDisposable
		{
			// Token: 0x060080A3 RID: 32931 RVA: 0x0032235E File Offset: 0x0032135E
			public VistaDialogEvents(IFileDialog dialog, FileDialog.VistaDialogEvents.OnOkCallback okCallback)
			{
				this._dialog = dialog;
				this._eventCookie = dialog.Advise(this);
				this._okCallback = okCallback;
			}

			// Token: 0x060080A4 RID: 32932 RVA: 0x00322381 File Offset: 0x00321381
			HRESULT IFileDialogEvents.OnFileOk(IFileDialog pfd)
			{
				if (!this._okCallback(pfd))
				{
					return HRESULT.S_FALSE;
				}
				return HRESULT.S_OK;
			}

			// Token: 0x060080A5 RID: 32933 RVA: 0x0032239C File Offset: 0x0032139C
			HRESULT IFileDialogEvents.OnFolderChanging(IFileDialog pfd, IShellItem psiFolder)
			{
				return HRESULT.E_NOTIMPL;
			}

			// Token: 0x060080A6 RID: 32934 RVA: 0x003223A3 File Offset: 0x003213A3
			HRESULT IFileDialogEvents.OnFolderChange(IFileDialog pfd)
			{
				return HRESULT.S_OK;
			}

			// Token: 0x060080A7 RID: 32935 RVA: 0x003223A3 File Offset: 0x003213A3
			HRESULT IFileDialogEvents.OnSelectionChange(IFileDialog pfd)
			{
				return HRESULT.S_OK;
			}

			// Token: 0x060080A8 RID: 32936 RVA: 0x003223AA File Offset: 0x003213AA
			HRESULT IFileDialogEvents.OnShareViolation(IFileDialog pfd, IShellItem psi, out FDESVR pResponse)
			{
				pResponse = FDESVR.DEFAULT;
				return HRESULT.S_OK;
			}

			// Token: 0x060080A9 RID: 32937 RVA: 0x003223A3 File Offset: 0x003213A3
			HRESULT IFileDialogEvents.OnTypeChange(IFileDialog pfd)
			{
				return HRESULT.S_OK;
			}

			// Token: 0x060080AA RID: 32938 RVA: 0x003223AA File Offset: 0x003213AA
			HRESULT IFileDialogEvents.OnOverwrite(IFileDialog pfd, IShellItem psi, out FDEOR pResponse)
			{
				pResponse = FDEOR.DEFAULT;
				return HRESULT.S_OK;
			}

			// Token: 0x060080AB RID: 32939 RVA: 0x003223B4 File Offset: 0x003213B4
			void IDisposable.Dispose()
			{
				this._dialog.Unadvise(this._eventCookie);
			}

			// Token: 0x04003C02 RID: 15362
			private IFileDialog _dialog;

			// Token: 0x04003C03 RID: 15363
			private FileDialog.VistaDialogEvents.OnOkCallback _okCallback;

			// Token: 0x04003C04 RID: 15364
			private uint _eventCookie;

			// Token: 0x02000C73 RID: 3187
			// (Invoke) Token: 0x06009205 RID: 37381
			public delegate bool OnOkCallback(IFileDialog dialog);
		}
	}
}
