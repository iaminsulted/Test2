using System;

namespace System.Windows.Documents
{
	// Token: 0x02000667 RID: 1639
	internal static class Validators
	{
		// Token: 0x06005060 RID: 20576 RVA: 0x0024D3BA File Offset: 0x0024C3BA
		internal static bool IsValidFontSize(long fs)
		{
			return fs >= 0L && fs <= 32767L;
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x0024D3CF File Offset: 0x0024C3CF
		internal static bool IsValidWidthType(long wt)
		{
			return wt >= 0L && wt <= 3L;
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0024D3E0 File Offset: 0x0024C3E0
		internal static long MakeValidShading(long s)
		{
			if (s > 10000L)
			{
				s = 10000L;
			}
			return s;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x0024D3F4 File Offset: 0x0024C3F4
		internal static long MakeValidBorderWidth(long w)
		{
			if (w < 0L)
			{
				w = 0L;
			}
			if (w > 1440L)
			{
				w = 1440L;
			}
			return w;
		}
	}
}
