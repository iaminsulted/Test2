using System;
using System.Windows;

namespace MS.Internal.PresentationFramework
{
	// Token: 0x02000332 RID: 818
	internal static class AnimatedTypeHelpers
	{
		// Token: 0x06001EAA RID: 7850 RVA: 0x00170105 File Offset: 0x0016F105
		private static double InterpolateDouble(double from, double to, double progress)
		{
			return from + (to - from) * progress;
		}

		// Token: 0x06001EAB RID: 7851 RVA: 0x00170110 File Offset: 0x0016F110
		internal static Thickness InterpolateThickness(Thickness from, Thickness to, double progress)
		{
			return new Thickness(AnimatedTypeHelpers.InterpolateDouble(from.Left, to.Left, progress), AnimatedTypeHelpers.InterpolateDouble(from.Top, to.Top, progress), AnimatedTypeHelpers.InterpolateDouble(from.Right, to.Right, progress), AnimatedTypeHelpers.InterpolateDouble(from.Bottom, to.Bottom, progress));
		}

		// Token: 0x06001EAC RID: 7852 RVA: 0x00170172 File Offset: 0x0016F172
		private static double AddDouble(double value1, double value2)
		{
			return value1 + value2;
		}

		// Token: 0x06001EAD RID: 7853 RVA: 0x00170178 File Offset: 0x0016F178
		internal static Thickness AddThickness(Thickness value1, Thickness value2)
		{
			return new Thickness(AnimatedTypeHelpers.AddDouble(value1.Left, value2.Left), AnimatedTypeHelpers.AddDouble(value1.Top, value2.Top), AnimatedTypeHelpers.AddDouble(value1.Right, value2.Right), AnimatedTypeHelpers.AddDouble(value1.Bottom, value2.Bottom));
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x001701D8 File Offset: 0x0016F1D8
		internal static Thickness SubtractThickness(Thickness value1, Thickness value2)
		{
			return new Thickness(value1.Left - value2.Left, value1.Top - value2.Top, value1.Right - value2.Right, value1.Bottom - value2.Bottom);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x00170226 File Offset: 0x0016F226
		private static double GetSegmentLengthDouble(double from, double to)
		{
			return Math.Abs(to - from);
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x00170230 File Offset: 0x0016F230
		internal static double GetSegmentLengthThickness(Thickness from, Thickness to)
		{
			return Math.Sqrt(Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Left, to.Left), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Top, to.Top), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Right, to.Right), 2.0) + Math.Pow(AnimatedTypeHelpers.GetSegmentLengthDouble(from.Bottom, to.Bottom), 2.0));
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x001702C9 File Offset: 0x0016F2C9
		private static double ScaleDouble(double value, double factor)
		{
			return value * factor;
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x001702CE File Offset: 0x0016F2CE
		internal static Thickness ScaleThickness(Thickness value, double factor)
		{
			return new Thickness(AnimatedTypeHelpers.ScaleDouble(value.Left, factor), AnimatedTypeHelpers.ScaleDouble(value.Top, factor), AnimatedTypeHelpers.ScaleDouble(value.Right, factor), AnimatedTypeHelpers.ScaleDouble(value.Bottom, factor));
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x00170309 File Offset: 0x0016F309
		private static bool IsValidAnimationValueDouble(double value)
		{
			return !AnimatedTypeHelpers.IsInvalidDouble(value);
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x00170316 File Offset: 0x0016F316
		internal static bool IsValidAnimationValueThickness(Thickness value)
		{
			return AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Left) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Top) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Right) || AnimatedTypeHelpers.IsValidAnimationValueDouble(value.Bottom);
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x0016E9C3 File Offset: 0x0016D9C3
		private static double GetZeroValueDouble(double baseValue)
		{
			return 0.0;
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x00170353 File Offset: 0x0016F353
		internal static Thickness GetZeroValueThickness(Thickness baseValue)
		{
			return new Thickness(AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Left), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Top), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Right), AnimatedTypeHelpers.GetZeroValueDouble(baseValue.Bottom));
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x0017038A File Offset: 0x0016F38A
		private static bool IsInvalidDouble(double value)
		{
			return double.IsInfinity(value) || DoubleUtil.IsNaN(value);
		}
	}
}
