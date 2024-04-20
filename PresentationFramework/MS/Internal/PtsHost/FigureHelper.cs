using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200011C RID: 284
	internal static class FigureHelper
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x0010CF4F File Offset: 0x0010BF4F
		internal static bool IsVerticalPageAnchor(FigureVerticalAnchor verticalAnchor)
		{
			return verticalAnchor == FigureVerticalAnchor.PageTop || verticalAnchor == FigureVerticalAnchor.PageBottom || verticalAnchor == FigureVerticalAnchor.PageCenter;
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0010CF5E File Offset: 0x0010BF5E
		internal static bool IsVerticalContentAnchor(FigureVerticalAnchor verticalAnchor)
		{
			return verticalAnchor == FigureVerticalAnchor.ContentTop || verticalAnchor == FigureVerticalAnchor.ContentBottom || verticalAnchor == FigureVerticalAnchor.ContentCenter;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0010CF4F File Offset: 0x0010BF4F
		internal static bool IsHorizontalPageAnchor(FigureHorizontalAnchor horizontalAnchor)
		{
			return horizontalAnchor == FigureHorizontalAnchor.PageLeft || horizontalAnchor == FigureHorizontalAnchor.PageRight || horizontalAnchor == FigureHorizontalAnchor.PageCenter;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0010CF5E File Offset: 0x0010BF5E
		internal static bool IsHorizontalContentAnchor(FigureHorizontalAnchor horizontalAnchor)
		{
			return horizontalAnchor == FigureHorizontalAnchor.ContentLeft || horizontalAnchor == FigureHorizontalAnchor.ContentRight || horizontalAnchor == FigureHorizontalAnchor.ContentCenter;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0010CF6E File Offset: 0x0010BF6E
		internal static bool IsHorizontalColumnAnchor(FigureHorizontalAnchor horizontalAnchor)
		{
			return horizontalAnchor == FigureHorizontalAnchor.ColumnLeft || horizontalAnchor == FigureHorizontalAnchor.ColumnRight || horizontalAnchor == FigureHorizontalAnchor.ColumnCenter;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0010CF80 File Offset: 0x0010BF80
		internal static double CalculateFigureWidth(StructuralCache structuralCache, Figure figure, FigureLength figureLength, out bool isWidthAuto)
		{
			isWidthAuto = figureLength.IsAuto;
			FigureHorizontalAnchor horizontalAnchor = figure.HorizontalAnchor;
			double num;
			if (figureLength.IsPage || (figureLength.IsAuto && FigureHelper.IsHorizontalPageAnchor(horizontalAnchor)))
			{
				num = structuralCache.CurrentFormatContext.PageWidth * figureLength.Value;
			}
			else if (figureLength.IsAbsolute)
			{
				num = FigureHelper.CalculateFigureCommon(figureLength);
			}
			else
			{
				int num2;
				double num3;
				double num4;
				double num5;
				FigureHelper.GetColumnMetrics(structuralCache, out num2, out num3, out num4, out num5);
				if (figureLength.IsContent || (figureLength.IsAuto && FigureHelper.IsHorizontalContentAnchor(horizontalAnchor)))
				{
					num = (num3 * (double)num2 + num4 * (double)(num2 - 1)) * figureLength.Value;
				}
				else
				{
					double value = figureLength.Value;
					int num6 = (int)value;
					if ((double)num6 == value && num6 > 0)
					{
						num6--;
					}
					num = num3 * value + num4 * (double)num6;
				}
			}
			Invariant.Assert(!DoubleUtil.IsNaN(num));
			return num;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0010D060 File Offset: 0x0010C060
		internal static double CalculateFigureHeight(StructuralCache structuralCache, Figure figure, FigureLength figureLength, out bool isHeightAuto)
		{
			double num;
			if (figureLength.IsPage)
			{
				num = structuralCache.CurrentFormatContext.PageHeight * figureLength.Value;
			}
			else if (figureLength.IsContent)
			{
				Thickness pageMargin = structuralCache.CurrentFormatContext.PageMargin;
				num = (structuralCache.CurrentFormatContext.PageHeight - pageMargin.Top - pageMargin.Bottom) * figureLength.Value;
			}
			else if (figureLength.IsColumn)
			{
				int num2;
				double num3;
				double num4;
				double num5;
				FigureHelper.GetColumnMetrics(structuralCache, out num2, out num3, out num4, out num5);
				double num6 = figureLength.Value;
				if (num6 > (double)num2)
				{
					num6 = (double)num2;
				}
				int num7 = (int)num6;
				if ((double)num7 == num6 && num7 > 0)
				{
					num7--;
				}
				num = num3 * num6 + num4 * (double)num7;
			}
			else
			{
				num = FigureHelper.CalculateFigureCommon(figureLength);
			}
			if (!DoubleUtil.IsNaN(num))
			{
				if (FigureHelper.IsVerticalPageAnchor(figure.VerticalAnchor))
				{
					num = Math.Max(1.0, Math.Min(num, structuralCache.CurrentFormatContext.PageHeight));
				}
				else
				{
					Thickness pageMargin2 = structuralCache.CurrentFormatContext.PageMargin;
					num = Math.Max(1.0, Math.Min(num, structuralCache.CurrentFormatContext.PageHeight - pageMargin2.Top - pageMargin2.Bottom));
				}
				TextDpi.EnsureValidPageWidth(ref num);
				isHeightAuto = false;
			}
			else
			{
				num = structuralCache.CurrentFormatContext.PageHeight;
				isHeightAuto = true;
			}
			return num;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0010D1B0 File Offset: 0x0010C1B0
		internal static double CalculateFigureCommon(FigureLength figureLength)
		{
			double result;
			if (figureLength.IsAuto)
			{
				result = double.NaN;
			}
			else if (figureLength.IsAbsolute)
			{
				result = figureLength.Value;
			}
			else
			{
				Invariant.Assert(false, "Unknown figure length type specified.");
				result = 0.0;
			}
			return result;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0010D1FC File Offset: 0x0010C1FC
		internal static void GetColumnMetrics(StructuralCache structuralCache, out int cColumns, out double width, out double gap, out double rule)
		{
			ColumnPropertiesGroup columnPropertiesGroup = new ColumnPropertiesGroup(structuralCache.PropertyOwner);
			FontFamily pageFontFamily = (FontFamily)structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(structuralCache.PropertyOwner);
			double pageFontSize = (double)structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			Size pageSize = structuralCache.CurrentFormatContext.PageSize;
			Thickness pageMargin = structuralCache.CurrentFormatContext.PageMargin;
			double num = pageSize.Width - (pageMargin.Left + pageMargin.Right);
			cColumns = PtsHelper.CalculateColumnCount(columnPropertiesGroup, lineHeightValue, num, pageFontSize, pageFontFamily, true);
			rule = columnPropertiesGroup.ColumnRuleWidth;
			double num2;
			PtsHelper.GetColumnMetrics(columnPropertiesGroup, num, pageFontSize, pageFontFamily, true, cColumns, ref lineHeightValue, out width, out num2, out gap);
			if (columnPropertiesGroup.IsColumnWidthFlexible && columnPropertiesGroup.ColumnSpaceDistribution == ColumnSpaceDistribution.Between)
			{
				width += num2 / (double)cColumns;
			}
			width = Math.Min(width, num);
		}
	}
}
