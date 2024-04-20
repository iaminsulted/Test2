using System;

namespace System.Windows.Shell
{
	// Token: 0x020003EF RID: 1007
	public class JumpTask : JumpItem
	{
		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06002B37 RID: 11063 RVA: 0x001A1FCA File Offset: 0x001A0FCA
		// (set) Token: 0x06002B38 RID: 11064 RVA: 0x001A1FD2 File Offset: 0x001A0FD2
		public string Title { get; set; }

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06002B39 RID: 11065 RVA: 0x001A1FDB File Offset: 0x001A0FDB
		// (set) Token: 0x06002B3A RID: 11066 RVA: 0x001A1FE3 File Offset: 0x001A0FE3
		public string Description { get; set; }

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06002B3B RID: 11067 RVA: 0x001A1FEC File Offset: 0x001A0FEC
		// (set) Token: 0x06002B3C RID: 11068 RVA: 0x001A1FF4 File Offset: 0x001A0FF4
		public string ApplicationPath { get; set; }

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06002B3D RID: 11069 RVA: 0x001A1FFD File Offset: 0x001A0FFD
		// (set) Token: 0x06002B3E RID: 11070 RVA: 0x001A2005 File Offset: 0x001A1005
		public string Arguments { get; set; }

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06002B3F RID: 11071 RVA: 0x001A200E File Offset: 0x001A100E
		// (set) Token: 0x06002B40 RID: 11072 RVA: 0x001A2016 File Offset: 0x001A1016
		public string WorkingDirectory { get; set; }

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06002B41 RID: 11073 RVA: 0x001A201F File Offset: 0x001A101F
		// (set) Token: 0x06002B42 RID: 11074 RVA: 0x001A2027 File Offset: 0x001A1027
		public string IconResourcePath { get; set; }

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06002B43 RID: 11075 RVA: 0x001A2030 File Offset: 0x001A1030
		// (set) Token: 0x06002B44 RID: 11076 RVA: 0x001A2038 File Offset: 0x001A1038
		public int IconResourceIndex { get; set; }
	}
}
