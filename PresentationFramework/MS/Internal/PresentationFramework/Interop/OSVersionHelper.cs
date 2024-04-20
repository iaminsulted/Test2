using System;
using System.Runtime.InteropServices;

namespace MS.Internal.PresentationFramework.Interop
{
	// Token: 0x02000333 RID: 819
	internal static class OSVersionHelper
	{
		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001EB8 RID: 7864 RVA: 0x0017039C File Offset: 0x0016F39C
		// (set) Token: 0x06001EB9 RID: 7865 RVA: 0x001703A3 File Offset: 0x0016F3A3
		internal static bool IsOsWindows10RS5OrGreater { get; set; } = OSVersionHelper.IsWindows10RS5OrGreater();

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x001703AB File Offset: 0x0016F3AB
		// (set) Token: 0x06001EBB RID: 7867 RVA: 0x001703B2 File Offset: 0x0016F3B2
		internal static bool IsOsWindows10RS3OrGreater { get; set; } = OSVersionHelper.IsWindows10RS3OrGreater();

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x001703BA File Offset: 0x0016F3BA
		// (set) Token: 0x06001EBD RID: 7869 RVA: 0x001703C1 File Offset: 0x0016F3C1
		internal static bool IsOsWindows10RS2OrGreater { get; set; } = OSVersionHelper.IsWindows10RS2OrGreater();

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x001703C9 File Offset: 0x0016F3C9
		// (set) Token: 0x06001EBF RID: 7871 RVA: 0x001703D0 File Offset: 0x0016F3D0
		internal static bool IsOsWindows10RS1OrGreater { get; set; } = OSVersionHelper.IsWindows10RS1OrGreater();

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x001703D8 File Offset: 0x0016F3D8
		// (set) Token: 0x06001EC1 RID: 7873 RVA: 0x001703DF File Offset: 0x0016F3DF
		internal static bool IsOsWindows10TH2OrGreater { get; set; } = OSVersionHelper.IsWindows10TH2OrGreater();

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x001703E7 File Offset: 0x0016F3E7
		// (set) Token: 0x06001EC3 RID: 7875 RVA: 0x001703EE File Offset: 0x0016F3EE
		internal static bool IsOsWindows10TH1OrGreater { get; set; } = OSVersionHelper.IsWindows10TH1OrGreater();

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x001703F6 File Offset: 0x0016F3F6
		// (set) Token: 0x06001EC5 RID: 7877 RVA: 0x001703FD File Offset: 0x0016F3FD
		internal static bool IsOsWindows10OrGreater { get; set; } = OSVersionHelper.IsWindows10OrGreater();

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x00170405 File Offset: 0x0016F405
		// (set) Token: 0x06001EC7 RID: 7879 RVA: 0x0017040C File Offset: 0x0016F40C
		internal static bool IsOsWindows8Point1OrGreater { get; set; } = OSVersionHelper.IsWindows8Point1OrGreater();

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x00170414 File Offset: 0x0016F414
		// (set) Token: 0x06001EC9 RID: 7881 RVA: 0x0017041B File Offset: 0x0016F41B
		internal static bool IsOsWindows8OrGreater { get; set; } = OSVersionHelper.IsWindows8OrGreater();

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x00170423 File Offset: 0x0016F423
		// (set) Token: 0x06001ECB RID: 7883 RVA: 0x0017042A File Offset: 0x0016F42A
		internal static bool IsOsWindows7SP1OrGreater { get; set; } = OSVersionHelper.IsWindows7SP1OrGreater();

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001ECC RID: 7884 RVA: 0x00170432 File Offset: 0x0016F432
		// (set) Token: 0x06001ECD RID: 7885 RVA: 0x00170439 File Offset: 0x0016F439
		internal static bool IsOsWindows7OrGreater { get; set; } = OSVersionHelper.IsWindows7OrGreater();

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x00170441 File Offset: 0x0016F441
		// (set) Token: 0x06001ECF RID: 7887 RVA: 0x00170448 File Offset: 0x0016F448
		internal static bool IsOsWindowsVistaSP2OrGreater { get; set; } = OSVersionHelper.IsWindowsVistaSP2OrGreater();

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001ED0 RID: 7888 RVA: 0x00170450 File Offset: 0x0016F450
		// (set) Token: 0x06001ED1 RID: 7889 RVA: 0x00170457 File Offset: 0x0016F457
		internal static bool IsOsWindowsVistaSP1OrGreater { get; set; } = OSVersionHelper.IsWindowsVistaSP1OrGreater();

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x0017045F File Offset: 0x0016F45F
		// (set) Token: 0x06001ED3 RID: 7891 RVA: 0x00170466 File Offset: 0x0016F466
		internal static bool IsOsWindowsVistaOrGreater { get; set; } = OSVersionHelper.IsWindowsVistaOrGreater();

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001ED4 RID: 7892 RVA: 0x0017046E File Offset: 0x0016F46E
		// (set) Token: 0x06001ED5 RID: 7893 RVA: 0x00170475 File Offset: 0x0016F475
		internal static bool IsOsWindowsXPSP3OrGreater { get; set; } = OSVersionHelper.IsWindowsXPSP3OrGreater();

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001ED6 RID: 7894 RVA: 0x0017047D File Offset: 0x0016F47D
		// (set) Token: 0x06001ED7 RID: 7895 RVA: 0x00170484 File Offset: 0x0016F484
		internal static bool IsOsWindowsXPSP2OrGreater { get; set; } = OSVersionHelper.IsWindowsXPSP2OrGreater();

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001ED8 RID: 7896 RVA: 0x0017048C File Offset: 0x0016F48C
		// (set) Token: 0x06001ED9 RID: 7897 RVA: 0x00170493 File Offset: 0x0016F493
		internal static bool IsOsWindowsXPSP1OrGreater { get; set; } = OSVersionHelper.IsWindowsXPSP1OrGreater();

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x0017049B File Offset: 0x0016F49B
		// (set) Token: 0x06001EDB RID: 7899 RVA: 0x001704A2 File Offset: 0x0016F4A2
		internal static bool IsOsWindowsXPOrGreater { get; set; } = OSVersionHelper.IsWindowsXPOrGreater();

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x001704AA File Offset: 0x0016F4AA
		// (set) Token: 0x06001EDD RID: 7901 RVA: 0x001704B1 File Offset: 0x0016F4B1
		internal static bool IsOsWindowsServer { get; set; } = OSVersionHelper.IsWindowsServer();

		// Token: 0x06001EDF RID: 7903
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS5OrGreater();

		// Token: 0x06001EE0 RID: 7904
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS3OrGreater();

		// Token: 0x06001EE1 RID: 7905
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS2OrGreater();

		// Token: 0x06001EE2 RID: 7906
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10RS1OrGreater();

		// Token: 0x06001EE3 RID: 7907
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10TH2OrGreater();

		// Token: 0x06001EE4 RID: 7908
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10TH1OrGreater();

		// Token: 0x06001EE5 RID: 7909
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows10OrGreater();

		// Token: 0x06001EE6 RID: 7910
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows8Point1OrGreater();

		// Token: 0x06001EE7 RID: 7911
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows8OrGreater();

		// Token: 0x06001EE8 RID: 7912
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows7SP1OrGreater();

		// Token: 0x06001EE9 RID: 7913
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindows7OrGreater();

		// Token: 0x06001EEA RID: 7914
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsVistaSP2OrGreater();

		// Token: 0x06001EEB RID: 7915
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsVistaSP1OrGreater();

		// Token: 0x06001EEC RID: 7916
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsVistaOrGreater();

		// Token: 0x06001EED RID: 7917
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPSP3OrGreater();

		// Token: 0x06001EEE RID: 7918
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPSP2OrGreater();

		// Token: 0x06001EEF RID: 7919
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPSP1OrGreater();

		// Token: 0x06001EF0 RID: 7920
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsXPOrGreater();

		// Token: 0x06001EF1 RID: 7921
		[DllImport("PresentationNative_cor3.dll", CallingConvention = 2)]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool IsWindowsServer();

		// Token: 0x06001EF2 RID: 7922 RVA: 0x00170588 File Offset: 0x0016F588
		internal static bool IsOsVersionOrGreater(OperatingSystemVersion osVer)
		{
			switch (osVer)
			{
			case OperatingSystemVersion.WindowsXPSP2:
				return OSVersionHelper.IsOsWindowsXPSP2OrGreater;
			case OperatingSystemVersion.WindowsXPSP3:
				return OSVersionHelper.IsOsWindowsXPSP3OrGreater;
			case OperatingSystemVersion.WindowsVista:
				return OSVersionHelper.IsOsWindowsVistaOrGreater;
			case OperatingSystemVersion.WindowsVistaSP1:
				return OSVersionHelper.IsOsWindowsVistaSP1OrGreater;
			case OperatingSystemVersion.WindowsVistaSP2:
				return OSVersionHelper.IsOsWindowsVistaSP2OrGreater;
			case OperatingSystemVersion.Windows7:
				return OSVersionHelper.IsOsWindows7OrGreater;
			case OperatingSystemVersion.Windows7SP1:
				return OSVersionHelper.IsOsWindows7SP1OrGreater;
			case OperatingSystemVersion.Windows8:
				return OSVersionHelper.IsOsWindows8OrGreater;
			case OperatingSystemVersion.Windows8Point1:
				return OSVersionHelper.IsOsWindows8Point1OrGreater;
			case OperatingSystemVersion.Windows10:
				return OSVersionHelper.IsOsWindows10OrGreater;
			case OperatingSystemVersion.Windows10TH2:
				return OSVersionHelper.IsOsWindows10TH2OrGreater;
			case OperatingSystemVersion.Windows10RS1:
				return OSVersionHelper.IsOsWindows10RS1OrGreater;
			case OperatingSystemVersion.Windows10RS2:
				return OSVersionHelper.IsOsWindows10RS2OrGreater;
			case OperatingSystemVersion.Windows10RS3:
				return OSVersionHelper.IsOsWindows10RS3OrGreater;
			case OperatingSystemVersion.Windows10RS5:
				return OSVersionHelper.IsOsWindows10RS5OrGreater;
			default:
				throw new ArgumentException(string.Format("{0} is not a valid OS!", osVer.ToString()), "osVer");
			}
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x00170654 File Offset: 0x0016F654
		internal static OperatingSystemVersion GetOsVersion()
		{
			if (OSVersionHelper.IsOsWindows10RS5OrGreater)
			{
				return OperatingSystemVersion.Windows10RS5;
			}
			if (OSVersionHelper.IsOsWindows10RS3OrGreater)
			{
				return OperatingSystemVersion.Windows10RS3;
			}
			if (OSVersionHelper.IsOsWindows10RS2OrGreater)
			{
				return OperatingSystemVersion.Windows10RS2;
			}
			if (OSVersionHelper.IsOsWindows10RS1OrGreater)
			{
				return OperatingSystemVersion.Windows10RS1;
			}
			if (OSVersionHelper.IsOsWindows10TH2OrGreater)
			{
				return OperatingSystemVersion.Windows10TH2;
			}
			if (OSVersionHelper.IsOsWindows10OrGreater)
			{
				return OperatingSystemVersion.Windows10;
			}
			if (OSVersionHelper.IsOsWindows8Point1OrGreater)
			{
				return OperatingSystemVersion.Windows8Point1;
			}
			if (OSVersionHelper.IsOsWindows8OrGreater)
			{
				return OperatingSystemVersion.Windows8;
			}
			if (OSVersionHelper.IsOsWindows7SP1OrGreater)
			{
				return OperatingSystemVersion.Windows7SP1;
			}
			if (OSVersionHelper.IsOsWindows7OrGreater)
			{
				return OperatingSystemVersion.Windows7;
			}
			if (OSVersionHelper.IsOsWindowsVistaSP2OrGreater)
			{
				return OperatingSystemVersion.WindowsVistaSP2;
			}
			if (OSVersionHelper.IsOsWindowsVistaSP1OrGreater)
			{
				return OperatingSystemVersion.WindowsVistaSP1;
			}
			if (OSVersionHelper.IsOsWindowsVistaOrGreater)
			{
				return OperatingSystemVersion.WindowsVista;
			}
			if (OSVersionHelper.IsOsWindowsXPSP3OrGreater)
			{
				return OperatingSystemVersion.WindowsXPSP3;
			}
			if (OSVersionHelper.IsOsWindowsXPSP2OrGreater)
			{
				return OperatingSystemVersion.WindowsXPSP2;
			}
			throw new Exception("OSVersionHelper.GetOsVersion Could not detect OS!");
		}
	}
}
