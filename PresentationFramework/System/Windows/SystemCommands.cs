using System;
using System.Windows.Input;
using System.Windows.Interop;
using Standard;

namespace System.Windows
{
	// Token: 0x020003B4 RID: 948
	public static class SystemCommands
	{
		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x0018BF91 File Offset: 0x0018AF91
		// (set) Token: 0x060026AB RID: 9899 RVA: 0x0018BF98 File Offset: 0x0018AF98
		public static RoutedCommand CloseWindowCommand { get; private set; } = new RoutedCommand("CloseWindow", typeof(SystemCommands));

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x0018BFA0 File Offset: 0x0018AFA0
		// (set) Token: 0x060026AD RID: 9901 RVA: 0x0018BFA7 File Offset: 0x0018AFA7
		public static RoutedCommand MaximizeWindowCommand { get; private set; } = new RoutedCommand("MaximizeWindow", typeof(SystemCommands));

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x0018BFAF File Offset: 0x0018AFAF
		// (set) Token: 0x060026AF RID: 9903 RVA: 0x0018BFB6 File Offset: 0x0018AFB6
		public static RoutedCommand MinimizeWindowCommand { get; private set; } = new RoutedCommand("MinimizeWindow", typeof(SystemCommands));

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060026B0 RID: 9904 RVA: 0x0018BFBE File Offset: 0x0018AFBE
		// (set) Token: 0x060026B1 RID: 9905 RVA: 0x0018BFC5 File Offset: 0x0018AFC5
		public static RoutedCommand RestoreWindowCommand { get; private set; } = new RoutedCommand("RestoreWindow", typeof(SystemCommands));

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060026B2 RID: 9906 RVA: 0x0018BFCD File Offset: 0x0018AFCD
		// (set) Token: 0x060026B3 RID: 9907 RVA: 0x0018BFD4 File Offset: 0x0018AFD4
		public static RoutedCommand ShowSystemMenuCommand { get; private set; } = new RoutedCommand("ShowSystemMenu", typeof(SystemCommands));

		// Token: 0x060026B5 RID: 9909 RVA: 0x0018C068 File Offset: 0x0018B068
		private static void _PostSystemCommand(Window window, SC command)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((int)command), IntPtr.Zero);
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0018C0AD File Offset: 0x0018B0AD
		public static void CloseWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.CLOSE);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0018C0C5 File Offset: 0x0018B0C5
		public static void MaximizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.MAXIMIZE);
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0018C0DD File Offset: 0x0018B0DD
		public static void MinimizeWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.MINIMIZE);
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0018C0F5 File Offset: 0x0018B0F5
		public static void RestoreWindow(Window window)
		{
			Verify.IsNotNull<Window>(window, "window");
			SystemCommands._PostSystemCommand(window, SC.RESTORE);
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0018C110 File Offset: 0x0018B110
		public static void ShowSystemMenu(Window window, Point screenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			DpiScale dpi = window.GetDpi();
			SystemCommands.ShowSystemMenuPhysicalCoordinates(window, DpiHelper.LogicalPixelsToDevice(screenLocation, dpi.DpiScaleX, dpi.DpiScaleY));
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0018C14C File Offset: 0x0018B14C
		internal static void ShowSystemMenuPhysicalCoordinates(Window window, Point physicalScreenLocation)
		{
			Verify.IsNotNull<Window>(window, "window");
			IntPtr handle = new WindowInteropHelper(window).Handle;
			if (handle == IntPtr.Zero || !NativeMethods.IsWindow(handle))
			{
				return;
			}
			uint num = NativeMethods.TrackPopupMenuEx(NativeMethods.GetSystemMenu(handle, false), 256U, (int)physicalScreenLocation.X, (int)physicalScreenLocation.Y, handle, IntPtr.Zero);
			if (num != 0U)
			{
				NativeMethods.PostMessage(handle, WM.SYSCOMMAND, new IntPtr((long)((ulong)num)), IntPtr.Zero);
			}
		}
	}
}
