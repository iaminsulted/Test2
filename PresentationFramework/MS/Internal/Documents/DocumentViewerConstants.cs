using System;

namespace MS.Internal.Documents
{
	// Token: 0x020001C0 RID: 448
	internal static class DocumentViewerConstants
	{
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0013CDF8 File Offset: 0x0013BDF8
		public static double MinimumZoom
		{
			get
			{
				return 5.0;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0013CE03 File Offset: 0x0013BE03
		public static double MaximumZoom
		{
			get
			{
				return 5000.0;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0013CE0E File Offset: 0x0013BE0E
		public static double MinimumScale
		{
			get
			{
				return 0.05;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0013CE19 File Offset: 0x0013BE19
		public static double MinimumThumbnailsScale
		{
			get
			{
				return 0.125;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0013CE24 File Offset: 0x0013BE24
		public static double MaximumScale
		{
			get
			{
				return 50.0;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0013CE2F File Offset: 0x0013BE2F
		public static int MaximumMaxPagesAcross
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x04000A91 RID: 2705
		private const double _minimumZoom = 5.0;

		// Token: 0x04000A92 RID: 2706
		private const double _minimumThumbnailsZoom = 12.5;

		// Token: 0x04000A93 RID: 2707
		private const double _maximumZoom = 5000.0;

		// Token: 0x04000A94 RID: 2708
		private const int _maximumMaxPagesAcross = 32;
	}
}
