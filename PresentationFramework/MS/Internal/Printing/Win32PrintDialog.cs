using System;
using System.Printing;
using System.Printing.Interop;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using MS.Win32;

namespace MS.Internal.Printing
{
	// Token: 0x02000155 RID: 341
	internal class Win32PrintDialog
	{
		// Token: 0x06000B3F RID: 2879 RVA: 0x0012C0A8 File Offset: 0x0012B0A8
		public Win32PrintDialog()
		{
			this._printTicket = null;
			this._printQueue = null;
			this._minPage = 1U;
			this._maxPage = 9999U;
			this._pageRangeSelection = PageRangeSelection.AllPages;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0012C0D8 File Offset: 0x0012B0D8
		internal uint ShowDialog()
		{
			uint result = 0U;
			IntPtr intPtr = IntPtr.Zero;
			if (Application.Current != null && Application.Current.MainWindow != null)
			{
				intPtr = new WindowInteropHelper(Application.Current.MainWindow).CriticalHandle;
			}
			try
			{
				if (this._printQueue == null || this._printTicket == null)
				{
					this.ProbeForPrintingSupport();
				}
				using (Win32PrintDialog.PrintDlgExMarshaler printDlgExMarshaler = new Win32PrintDialog.PrintDlgExMarshaler(intPtr, this))
				{
					printDlgExMarshaler.SyncToStruct();
					if (UnsafeNativeMethods.PrintDlgEx(printDlgExMarshaler.UnmanagedPrintDlgEx) == 0)
					{
						result = printDlgExMarshaler.SyncFromStruct();
					}
				}
			}
			catch (Exception ex)
			{
				if (!string.Equals(ex.GetType().FullName, "System.Printing.PrintingNotSupportedException", StringComparison.Ordinal))
				{
					throw;
				}
				string text = SR.Get("PrintDialogInstallPrintSupportMessageBox");
				string text2 = SR.Get("PrintDialogInstallPrintSupportCaption");
				MessageBoxOptions messageBoxOptions = (text2 != null && text2.Length > 0 && text2[0] == '‏') ? MessageBoxOptions.RtlReading : MessageBoxOptions.None;
				int type = (int)((MessageBoxOptions)64 | messageBoxOptions);
				if (intPtr == IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.GetActiveWindow();
				}
				if (UnsafeNativeMethods.MessageBox(new HandleRef(null, intPtr), text, text2, type) != 0)
				{
					result = 0U;
				}
			}
			return result;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0012C20C File Offset: 0x0012B20C
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0012C214 File Offset: 0x0012B214
		internal PrintTicket PrintTicket
		{
			get
			{
				return this._printTicket;
			}
			set
			{
				this._printTicket = value;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0012C21D File Offset: 0x0012B21D
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0012C225 File Offset: 0x0012B225
		internal PrintQueue PrintQueue
		{
			get
			{
				return this._printQueue;
			}
			set
			{
				this._printQueue = value;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0012C22E File Offset: 0x0012B22E
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0012C236 File Offset: 0x0012B236
		internal uint MinPage
		{
			get
			{
				return this._minPage;
			}
			set
			{
				this._minPage = value;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0012C23F File Offset: 0x0012B23F
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0012C247 File Offset: 0x0012B247
		internal uint MaxPage
		{
			get
			{
				return this._maxPage;
			}
			set
			{
				this._maxPage = value;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0012C250 File Offset: 0x0012B250
		// (set) Token: 0x06000B4A RID: 2890 RVA: 0x0012C258 File Offset: 0x0012B258
		internal PageRangeSelection PageRangeSelection
		{
			get
			{
				return this._pageRangeSelection;
			}
			set
			{
				this._pageRangeSelection = value;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0012C261 File Offset: 0x0012B261
		// (set) Token: 0x06000B4C RID: 2892 RVA: 0x0012C269 File Offset: 0x0012B269
		internal PageRange PageRange
		{
			get
			{
				return this._pageRange;
			}
			set
			{
				this._pageRange = value;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0012C272 File Offset: 0x0012B272
		// (set) Token: 0x06000B4E RID: 2894 RVA: 0x0012C27A File Offset: 0x0012B27A
		internal bool PageRangeEnabled
		{
			get
			{
				return this._pageRangeEnabled;
			}
			set
			{
				this._pageRangeEnabled = value;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0012C283 File Offset: 0x0012B283
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0012C28B File Offset: 0x0012B28B
		internal bool SelectedPagesEnabled
		{
			get
			{
				return this._selectedPagesEnabled;
			}
			set
			{
				this._selectedPagesEnabled = value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0012C294 File Offset: 0x0012B294
		// (set) Token: 0x06000B52 RID: 2898 RVA: 0x0012C29C File Offset: 0x0012B29C
		internal bool CurrentPageEnabled
		{
			get
			{
				return this._currentPageEnabled;
			}
			set
			{
				this._currentPageEnabled = value;
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0012C2A8 File Offset: 0x0012B2A8
		private void ProbeForPrintingSupport()
		{
			string deviceName = (this._printQueue != null) ? this._printQueue.FullName : string.Empty;
			try
			{
				using (new PrintTicketConverter(deviceName, 1))
				{
				}
			}
			catch (PrintQueueException)
			{
			}
		}

		// Token: 0x040008B3 RID: 2227
		private PrintTicket _printTicket;

		// Token: 0x040008B4 RID: 2228
		private PrintQueue _printQueue;

		// Token: 0x040008B5 RID: 2229
		private PageRangeSelection _pageRangeSelection;

		// Token: 0x040008B6 RID: 2230
		private PageRange _pageRange;

		// Token: 0x040008B7 RID: 2231
		private bool _pageRangeEnabled;

		// Token: 0x040008B8 RID: 2232
		private bool _selectedPagesEnabled;

		// Token: 0x040008B9 RID: 2233
		private bool _currentPageEnabled;

		// Token: 0x040008BA RID: 2234
		private uint _minPage;

		// Token: 0x040008BB RID: 2235
		private uint _maxPage;

		// Token: 0x040008BC RID: 2236
		private const char RightToLeftMark = '‏';

		// Token: 0x020009BC RID: 2492
		private sealed class PrintDlgExMarshaler : IDisposable
		{
			// Token: 0x060083B2 RID: 33714 RVA: 0x0032393E File Offset: 0x0032293E
			internal PrintDlgExMarshaler(IntPtr owner, Win32PrintDialog dialog)
			{
				this._ownerHandle = owner;
				this._dialog = dialog;
				this._unmanagedPrintDlgEx = IntPtr.Zero;
			}

			// Token: 0x060083B3 RID: 33715 RVA: 0x00323960 File Offset: 0x00322960
			~PrintDlgExMarshaler()
			{
				this.Dispose(true);
			}

			// Token: 0x17001DA4 RID: 7588
			// (get) Token: 0x060083B4 RID: 33716 RVA: 0x00323990 File Offset: 0x00322990
			internal IntPtr UnmanagedPrintDlgEx
			{
				get
				{
					return this._unmanagedPrintDlgEx;
				}
			}

			// Token: 0x060083B5 RID: 33717 RVA: 0x00323998 File Offset: 0x00322998
			internal uint SyncFromStruct()
			{
				if (this._unmanagedPrintDlgEx == IntPtr.Zero)
				{
					return 0U;
				}
				uint num = this.AcquireResultFromPrintDlgExStruct(this._unmanagedPrintDlgEx);
				if (num == 1U || num == 2U)
				{
					string text;
					uint num2;
					PageRange pageRange;
					IntPtr devModeHandle;
					this.ExtractPrintDataAndDevMode(this._unmanagedPrintDlgEx, out text, out num2, out pageRange, out devModeHandle);
					this._dialog.PrintQueue = this.AcquirePrintQueue(text);
					this._dialog.PrintTicket = this.AcquirePrintTicket(devModeHandle, text);
					if ((num2 & 2U) == 2U)
					{
						if (pageRange.PageFrom > pageRange.PageTo)
						{
							int pageTo = pageRange.PageTo;
							pageRange.PageTo = pageRange.PageFrom;
							pageRange.PageFrom = pageTo;
						}
						this._dialog.PageRangeSelection = PageRangeSelection.UserPages;
						this._dialog.PageRange = pageRange;
					}
					else if ((num2 & 1U) == 1U)
					{
						this._dialog.PageRangeSelection = PageRangeSelection.SelectedPages;
					}
					else if ((num2 & 4194304U) == 4194304U)
					{
						this._dialog.PageRangeSelection = PageRangeSelection.CurrentPage;
					}
					else
					{
						this._dialog.PageRangeSelection = PageRangeSelection.AllPages;
					}
				}
				return num;
			}

			// Token: 0x060083B6 RID: 33718 RVA: 0x00323A98 File Offset: 0x00322A98
			internal void SyncToStruct()
			{
				if (this._unmanagedPrintDlgEx != IntPtr.Zero)
				{
					this.FreeUnmanagedPrintDlgExStruct(this._unmanagedPrintDlgEx);
				}
				if (this._ownerHandle == IntPtr.Zero)
				{
					this._ownerHandle = UnsafeNativeMethods.GetDesktopWindow();
				}
				this._unmanagedPrintDlgEx = this.AllocateUnmanagedPrintDlgExStruct();
			}

			// Token: 0x060083B7 RID: 33719 RVA: 0x00323AEC File Offset: 0x00322AEC
			private void Dispose(bool disposing)
			{
				if (disposing && this._unmanagedPrintDlgEx != IntPtr.Zero)
				{
					this.FreeUnmanagedPrintDlgExStruct(this._unmanagedPrintDlgEx);
					this._unmanagedPrintDlgEx = IntPtr.Zero;
				}
			}

			// Token: 0x060083B8 RID: 33720 RVA: 0x00323B1C File Offset: 0x00322B1C
			private void ExtractPrintDataAndDevMode(IntPtr unmanagedBuffer, out string printerName, out uint flags, out PageRange pageRange, out IntPtr devModeHandle)
			{
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					devModeHandle = printdlgex.hDevMode;
					intPtr = printdlgex.hDevNames;
					flags = printdlgex.Flags;
					intPtr2 = printdlgex.lpPageRanges;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					devModeHandle = printdlgex2.hDevMode;
					intPtr = printdlgex2.hDevNames;
					flags = printdlgex2.Flags;
					intPtr2 = printdlgex2.lpPageRanges;
				}
				if ((flags & 2U) == 2U && intPtr2 != IntPtr.Zero)
				{
					NativeMethods.PRINTPAGERANGE printpagerange = (NativeMethods.PRINTPAGERANGE)Marshal.PtrToStructure(intPtr2, typeof(NativeMethods.PRINTPAGERANGE));
					pageRange = new PageRange((int)printpagerange.nFromPage, (int)printpagerange.nToPage);
				}
				else
				{
					pageRange = new PageRange(1);
				}
				if (intPtr != IntPtr.Zero)
				{
					IntPtr intPtr3 = IntPtr.Zero;
					try
					{
						intPtr3 = UnsafeNativeMethods.GlobalLock(intPtr);
						int offset = checked((int)((NativeMethods.DEVNAMES)Marshal.PtrToStructure(intPtr3, typeof(NativeMethods.DEVNAMES))).wDeviceOffset * Marshal.SystemDefaultCharSize);
						printerName = Marshal.PtrToStringAuto(intPtr3 + offset);
						return;
					}
					finally
					{
						if (intPtr3 != IntPtr.Zero)
						{
							UnsafeNativeMethods.GlobalUnlock(intPtr);
						}
					}
				}
				printerName = string.Empty;
			}

			// Token: 0x060083B9 RID: 33721 RVA: 0x00323C7C File Offset: 0x00322C7C
			private PrintQueue AcquirePrintQueue(string printerName)
			{
				PrintQueue printQueue = null;
				EnumeratedPrintQueueTypes[] enumerationFlag = new EnumeratedPrintQueueTypes[]
				{
					EnumeratedPrintQueueTypes.Local,
					EnumeratedPrintQueueTypes.Connections
				};
				PrintQueueIndexedProperty[] propertiesFilter = new PrintQueueIndexedProperty[]
				{
					PrintQueueIndexedProperty.Name,
					PrintQueueIndexedProperty.QueueAttributes
				};
				using (LocalPrintServer localPrintServer = new LocalPrintServer())
				{
					foreach (PrintQueue printQueue2 in localPrintServer.GetPrintQueues(propertiesFilter, enumerationFlag))
					{
						if (printerName.Equals(printQueue2.FullName, StringComparison.OrdinalIgnoreCase))
						{
							printQueue = printQueue2;
							break;
						}
					}
				}
				if (printQueue != null)
				{
					printQueue.InPartialTrust = true;
				}
				return printQueue;
			}

			// Token: 0x060083BA RID: 33722 RVA: 0x00323D28 File Offset: 0x00322D28
			private PrintTicket AcquirePrintTicket(IntPtr devModeHandle, string printQueueName)
			{
				PrintTicket result = null;
				byte[] array = null;
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = UnsafeNativeMethods.GlobalLock(devModeHandle);
					NativeMethods.DEVMODE devmode = (NativeMethods.DEVMODE)Marshal.PtrToStructure(intPtr, typeof(NativeMethods.DEVMODE));
					array = new byte[(int)(devmode.dmSize + devmode.dmDriverExtra)];
					Marshal.Copy(intPtr, array, 0, array.Length);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.GlobalUnlock(devModeHandle);
					}
				}
				using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printQueueName, PrintTicketConverter.MaxPrintSchemaVersion))
				{
					result = printTicketConverter.ConvertDevModeToPrintTicket(array);
				}
				return result;
			}

			// Token: 0x060083BB RID: 33723 RVA: 0x00323DD4 File Offset: 0x00322DD4
			private uint AcquireResultFromPrintDlgExStruct(IntPtr unmanagedBuffer)
			{
				uint dwResultAction;
				if (!this.Is64Bit())
				{
					dwResultAction = ((NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32))).dwResultAction;
				}
				else
				{
					dwResultAction = ((NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64))).dwResultAction;
				}
				return dwResultAction;
			}

			// Token: 0x060083BC RID: 33724 RVA: 0x00323E24 File Offset: 0x00322E24
			private IntPtr AllocateUnmanagedPrintDlgExStruct()
			{
				IntPtr intPtr = IntPtr.Zero;
				NativeMethods.PRINTPAGERANGE structure;
				structure.nToPage = (uint)this._dialog.PageRange.PageTo;
				structure.nFromPage = (uint)this._dialog.PageRange.PageFrom;
				uint flags = 1835008U;
				try
				{
					if (!this.Is64Bit())
					{
						NativeMethods.PRINTDLGEX32 printdlgex = new NativeMethods.PRINTDLGEX32();
						printdlgex.hwndOwner = this._ownerHandle;
						printdlgex.nMinPage = this._dialog.MinPage;
						printdlgex.nMaxPage = this._dialog.MaxPage;
						printdlgex.Flags = flags;
						if (this._dialog.SelectedPagesEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.SelectedPages)
							{
								printdlgex.Flags |= 1U;
							}
						}
						else
						{
							printdlgex.Flags |= 4U;
						}
						if (this._dialog.CurrentPageEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.CurrentPage)
							{
								printdlgex.Flags |= 4194304U;
							}
						}
						else
						{
							printdlgex.Flags |= 8388608U;
						}
						if (this._dialog.PageRangeEnabled)
						{
							printdlgex.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
							printdlgex.nMaxPageRanges = 1U;
							if (this._dialog.PageRangeSelection == PageRangeSelection.UserPages)
							{
								printdlgex.nPageRanges = 1U;
								Marshal.StructureToPtr<NativeMethods.PRINTPAGERANGE>(structure, printdlgex.lpPageRanges, false);
								printdlgex.Flags |= 2U;
							}
							else
							{
								printdlgex.nPageRanges = 0U;
							}
						}
						else
						{
							printdlgex.lpPageRanges = IntPtr.Zero;
							printdlgex.nMaxPageRanges = 0U;
							printdlgex.Flags |= 8U;
						}
						if (this._dialog.PrintQueue != null)
						{
							printdlgex.hDevNames = this.AllocateAndInitializeDevNames(this._dialog.PrintQueue.FullName);
							if (this._dialog.PrintTicket != null)
							{
								printdlgex.hDevMode = this.AllocateAndInitializeDevMode(this._dialog.PrintQueue.FullName, this._dialog.PrintTicket);
							}
						}
						intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX32)));
						Marshal.StructureToPtr<NativeMethods.PRINTDLGEX32>(printdlgex, intPtr, false);
					}
					else
					{
						NativeMethods.PRINTDLGEX64 printdlgex2 = new NativeMethods.PRINTDLGEX64();
						printdlgex2.hwndOwner = this._ownerHandle;
						printdlgex2.nMinPage = this._dialog.MinPage;
						printdlgex2.nMaxPage = this._dialog.MaxPage;
						printdlgex2.Flags = flags;
						if (this._dialog.SelectedPagesEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.SelectedPages)
							{
								printdlgex2.Flags |= 1U;
							}
						}
						else
						{
							printdlgex2.Flags |= 4U;
						}
						if (this._dialog.CurrentPageEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.CurrentPage)
							{
								printdlgex2.Flags |= 4194304U;
							}
						}
						else
						{
							printdlgex2.Flags |= 8388608U;
						}
						if (this._dialog.PageRangeEnabled)
						{
							printdlgex2.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
							printdlgex2.nMaxPageRanges = 1U;
							if (this._dialog.PageRangeSelection == PageRangeSelection.UserPages)
							{
								printdlgex2.nPageRanges = 1U;
								Marshal.StructureToPtr<NativeMethods.PRINTPAGERANGE>(structure, printdlgex2.lpPageRanges, false);
								printdlgex2.Flags |= 2U;
							}
							else
							{
								printdlgex2.nPageRanges = 0U;
							}
						}
						else
						{
							printdlgex2.lpPageRanges = IntPtr.Zero;
							printdlgex2.nMaxPageRanges = 0U;
							printdlgex2.Flags |= 8U;
						}
						if (this._dialog.PrintQueue != null)
						{
							printdlgex2.hDevNames = this.AllocateAndInitializeDevNames(this._dialog.PrintQueue.FullName);
							if (this._dialog.PrintTicket != null)
							{
								printdlgex2.hDevMode = this.AllocateAndInitializeDevMode(this._dialog.PrintQueue.FullName, this._dialog.PrintTicket);
							}
						}
						intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX64)));
						Marshal.StructureToPtr<NativeMethods.PRINTDLGEX64>(printdlgex2, intPtr, false);
					}
				}
				catch (Exception)
				{
					if (intPtr != IntPtr.Zero)
					{
						this.FreeUnmanagedPrintDlgExStruct(intPtr);
						intPtr = IntPtr.Zero;
					}
					throw;
				}
				return intPtr;
			}

			// Token: 0x060083BD RID: 33725 RVA: 0x00324258 File Offset: 0x00323258
			private void FreeUnmanagedPrintDlgExStruct(IntPtr unmanagedBuffer)
			{
				if (unmanagedBuffer == IntPtr.Zero)
				{
					return;
				}
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				IntPtr intPtr3 = IntPtr.Zero;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					intPtr = printdlgex.hDevMode;
					intPtr2 = printdlgex.hDevNames;
					intPtr3 = printdlgex.lpPageRanges;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					intPtr = printdlgex2.hDevMode;
					intPtr2 = printdlgex2.hDevNames;
					intPtr3 = printdlgex2.lpPageRanges;
				}
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr2);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr3);
				}
				Marshal.FreeHGlobal(unmanagedBuffer);
			}

			// Token: 0x060083BE RID: 33726 RVA: 0x00324323 File Offset: 0x00323323
			private bool Is64Bit()
			{
				return Marshal.SizeOf<IntPtr>(IntPtr.Zero) == 8;
			}

			// Token: 0x060083BF RID: 33727 RVA: 0x00324334 File Offset: 0x00323334
			private IntPtr AllocateAndInitializeDevNames(string printerName)
			{
				IntPtr intPtr = IntPtr.Zero;
				char[] array = printerName.ToCharArray();
				intPtr = Marshal.AllocHGlobal(checked((array.Length + 3) * Marshal.SystemDefaultCharSize + Marshal.SizeOf(typeof(NativeMethods.DEVNAMES))));
				ushort num = (ushort)Marshal.SizeOf(typeof(NativeMethods.DEVNAMES));
				NativeMethods.DEVNAMES devnames;
				devnames.wDeviceOffset = (ushort)((int)num / Marshal.SystemDefaultCharSize);
				IntPtr intPtr2;
				IntPtr destination;
				checked
				{
					devnames.wDriverOffset = (ushort)((int)devnames.wDeviceOffset + array.Length + 1);
					devnames.wOutputOffset = devnames.wDriverOffset + 1;
					devnames.wDefault = 0;
					Marshal.StructureToPtr<NativeMethods.DEVNAMES>(devnames, intPtr, false);
					intPtr2 = (IntPtr)((long)intPtr + (long)(unchecked((ulong)num)));
					destination = (IntPtr)((long)intPtr2 + unchecked((long)(checked(array.Length * Marshal.SystemDefaultCharSize))));
				}
				byte[] array2 = new byte[3 * Marshal.SystemDefaultCharSize];
				Array.Clear(array2, 0, array2.Length);
				Marshal.Copy(array, 0, intPtr2, array.Length);
				Marshal.Copy(array2, 0, destination, array2.Length);
				return intPtr;
			}

			// Token: 0x060083C0 RID: 33728 RVA: 0x00324424 File Offset: 0x00323424
			private IntPtr AllocateAndInitializeDevMode(string printerName, PrintTicket printTicket)
			{
				byte[] array = null;
				using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printerName, PrintTicketConverter.MaxPrintSchemaVersion))
				{
					array = printTicketConverter.ConvertPrintTicketToDevMode(printTicket, BaseDevModeType.UserDefault);
				}
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				return intPtr;
			}

			// Token: 0x060083C1 RID: 33729 RVA: 0x0032447C File Offset: 0x0032347C
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x04003F74 RID: 16244
			private Win32PrintDialog _dialog;

			// Token: 0x04003F75 RID: 16245
			private IntPtr _unmanagedPrintDlgEx;

			// Token: 0x04003F76 RID: 16246
			private IntPtr _ownerHandle;
		}
	}
}
