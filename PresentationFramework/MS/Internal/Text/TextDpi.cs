using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.Text
{
	// Token: 0x02000326 RID: 806
	internal static class TextDpi
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x0016ED54 File Offset: 0x0016DD54
		internal static double MinWidth
		{
			get
			{
				return 0.0033333333333333335;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x0016ED5F File Offset: 0x0016DD5F
		internal static double MaxWidth
		{
			get
			{
				return 3579139.4066666667;
			}
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x0016ED6C File Offset: 0x0016DD6C
		internal static int ToTextDpi(double d)
		{
			if (DoubleUtil.IsZero(d))
			{
				return 0;
			}
			if (d > 0.0)
			{
				if (d > 3579139.4066666667)
				{
					d = 3579139.4066666667;
				}
				else if (d < 0.0033333333333333335)
				{
					d = 0.0033333333333333335;
				}
			}
			else if (d < -3579139.4066666667)
			{
				d = -3579139.4066666667;
			}
			else if (d > -0.0033333333333333335)
			{
				d = -0.0033333333333333335;
			}
			return (int)Math.Round(d * 300.0);
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x0016EE02 File Offset: 0x0016DE02
		internal static double FromTextDpi(int i)
		{
			return (double)i / 300.0;
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x0016EE10 File Offset: 0x0016DE10
		internal static PTS.FSPOINT ToTextPoint(Point point)
		{
			return new PTS.FSPOINT
			{
				u = TextDpi.ToTextDpi(point.X),
				v = TextDpi.ToTextDpi(point.Y)
			};
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x0016EE4C File Offset: 0x0016DE4C
		internal static PTS.FSVECTOR ToTextSize(Size size)
		{
			return new PTS.FSVECTOR
			{
				du = TextDpi.ToTextDpi(size.Width),
				dv = TextDpi.ToTextDpi(size.Height)
			};
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x0016EE88 File Offset: 0x0016DE88
		internal static Rect FromTextRect(PTS.FSRECT fsrect)
		{
			return new Rect(TextDpi.FromTextDpi(fsrect.u), TextDpi.FromTextDpi(fsrect.v), TextDpi.FromTextDpi(fsrect.du), TextDpi.FromTextDpi(fsrect.dv));
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x0016EEBB File Offset: 0x0016DEBB
		internal static void EnsureValidLineOffset(ref double offset)
		{
			if (offset > 3579139.4066666667)
			{
				offset = 3579139.4066666667;
				return;
			}
			if (offset < -3579139.4066666667)
			{
				offset = -3579139.4066666667;
			}
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0016EEEE File Offset: 0x0016DEEE
		internal static void SnapToTextDpi(ref Size size)
		{
			size = new Size(TextDpi.FromTextDpi(TextDpi.ToTextDpi(size.Width)), TextDpi.FromTextDpi(TextDpi.ToTextDpi(size.Height)));
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x0016EF1B File Offset: 0x0016DF1B
		internal static void EnsureValidLineWidth(ref double width)
		{
			if (width > 3579139.4066666667)
			{
				width = 3579139.4066666667;
				return;
			}
			if (width < 0.0033333333333333335)
			{
				width = 0.0033333333333333335;
			}
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x0016EF50 File Offset: 0x0016DF50
		internal static void EnsureValidLineWidth(ref Size size)
		{
			if (size.Width > 3579139.4066666667)
			{
				size.Width = 3579139.4066666667;
				return;
			}
			if (size.Width < 0.0033333333333333335)
			{
				size.Width = 0.0033333333333333335;
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0016EF9E File Offset: 0x0016DF9E
		internal static void EnsureValidLineWidth(ref int width)
		{
			if (width > 1073741822)
			{
				width = 1073741822;
				return;
			}
			if (width < 1)
			{
				width = 1;
			}
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0016EFBC File Offset: 0x0016DFBC
		internal static void EnsureValidPageSize(ref Size size)
		{
			if (size.Width > 3579139.4066666667)
			{
				size.Width = 3579139.4066666667;
			}
			else if (size.Width < 0.0033333333333333335)
			{
				size.Width = 0.0033333333333333335;
			}
			if (size.Height > 3579139.4066666667)
			{
				size.Height = 3579139.4066666667;
				return;
			}
			if (size.Height < 0.0033333333333333335)
			{
				size.Height = 0.0033333333333333335;
			}
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0016EF1B File Offset: 0x0016DF1B
		internal static void EnsureValidPageWidth(ref double width)
		{
			if (width > 3579139.4066666667)
			{
				width = 3579139.4066666667;
				return;
			}
			if (width < 0.0033333333333333335)
			{
				width = 0.0033333333333333335;
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0016F04C File Offset: 0x0016E04C
		internal static void EnsureValidPageMargin(ref Thickness pageMargin, Size pageSize)
		{
			if (pageMargin.Left >= pageSize.Width)
			{
				pageMargin.Right = 0.0;
			}
			if (pageMargin.Left + pageMargin.Right >= pageSize.Width)
			{
				pageMargin.Right = Math.Max(0.0, pageSize.Width - pageMargin.Left - 0.0033333333333333335);
				if (pageMargin.Left + pageMargin.Right >= pageSize.Width)
				{
					pageMargin.Left = pageSize.Width - 0.0033333333333333335;
				}
			}
			if (pageMargin.Top >= pageSize.Height)
			{
				pageMargin.Bottom = 0.0;
			}
			if (pageMargin.Top + pageMargin.Bottom >= pageSize.Height)
			{
				pageMargin.Bottom = Math.Max(0.0, pageSize.Height - pageMargin.Top - 0.0033333333333333335);
				if (pageMargin.Top + pageMargin.Bottom >= pageSize.Height)
				{
					pageMargin.Top = pageSize.Height - 0.0033333333333333335;
				}
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0016F174 File Offset: 0x0016E174
		internal static void EnsureValidObjSize(ref Size size)
		{
			if (size.Width > 1193046.4688888888)
			{
				size.Width = 1193046.4688888888;
			}
			if (size.Height > 1193046.4688888888)
			{
				size.Height = 1193046.4688888888;
			}
		}

		// Token: 0x04000EE4 RID: 3812
		private const double _scale = 300.0;

		// Token: 0x04000EE5 RID: 3813
		private const int _maxSizeInt = 1073741822;

		// Token: 0x04000EE6 RID: 3814
		private const double _maxSize = 3579139.4066666667;

		// Token: 0x04000EE7 RID: 3815
		private const int _minSizeInt = 1;

		// Token: 0x04000EE8 RID: 3816
		private const double _minSize = 0.0033333333333333335;

		// Token: 0x04000EE9 RID: 3817
		private const double _maxObjSize = 1193046.4688888888;
	}
}
