using System;

namespace Microsoft.Win32
{
	// Token: 0x020000E4 RID: 228
	public static class FileDialogCustomPlaces
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x000FEAD2 File Offset: 0x000FDAD2
		public static FileDialogCustomPlace RoamingApplicationData
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("3EB685DB-65F9-4CF6-A03A-E3EF65729F3D"));
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x000FEAE3 File Offset: 0x000FDAE3
		public static FileDialogCustomPlace LocalApplicationData
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("F1B32785-6FBA-4FCF-9D55-7B8E7F157091"));
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x000FEAF4 File Offset: 0x000FDAF4
		public static FileDialogCustomPlace Cookies
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("2B0F765D-C0E9-4171-908E-08A611B84FF6"));
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x000FEB05 File Offset: 0x000FDB05
		public static FileDialogCustomPlace Contacts
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("56784854-C6CB-462b-8169-88E350ACB882"));
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x000FEB16 File Offset: 0x000FDB16
		public static FileDialogCustomPlace Favorites
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("1777F761-68AD-4D8A-87BD-30B759FA33DD"));
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x000FEB27 File Offset: 0x000FDB27
		public static FileDialogCustomPlace Programs
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("A77F5D77-2E2B-44C3-A6A2-ABA601054A51"));
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000FEB38 File Offset: 0x000FDB38
		public static FileDialogCustomPlace Music
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("4BD8D571-6D19-48D3-BE97-422220080E43"));
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000FEB49 File Offset: 0x000FDB49
		public static FileDialogCustomPlace Pictures
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("33E28130-4E1E-4676-835A-98395C3BC3BB"));
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000FEB5A File Offset: 0x000FDB5A
		public static FileDialogCustomPlace SendTo
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("8983036C-27C0-404B-8F08-102D10DCFD74"));
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x000FEB6B File Offset: 0x000FDB6B
		public static FileDialogCustomPlace StartMenu
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("625B53C3-AB48-4EC1-BA1F-A1EF4146FC19"));
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000FEB7C File Offset: 0x000FDB7C
		public static FileDialogCustomPlace Startup
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("B97D20BB-F46A-4C97-BA10-5E3608430854"));
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x000FEB8D File Offset: 0x000FDB8D
		public static FileDialogCustomPlace System
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("1AC14E77-02E7-4E5D-B744-2EB1AE5198B7"));
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000FEB9E File Offset: 0x000FDB9E
		public static FileDialogCustomPlace Templates
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("A63293E8-664E-48DB-A079-DF759E0509F7"));
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x000FEBAF File Offset: 0x000FDBAF
		public static FileDialogCustomPlace Desktop
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("B4BFCC3A-DB2C-424C-B029-7FE99A87C641"));
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x000FEBC0 File Offset: 0x000FDBC0
		public static FileDialogCustomPlace Documents
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("FDD39AD0-238F-46AF-ADB4-6C85480369C7"));
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000FEBD1 File Offset: 0x000FDBD1
		public static FileDialogCustomPlace ProgramFiles
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("905E63B6-C1BF-494E-B29C-65B732D3D21A"));
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x000FEBE2 File Offset: 0x000FDBE2
		public static FileDialogCustomPlace ProgramFilesCommon
		{
			get
			{
				return new FileDialogCustomPlace(new Guid("F7F1ED05-9F6D-47A2-AAAE-29D317C6F066"));
			}
		}
	}
}
