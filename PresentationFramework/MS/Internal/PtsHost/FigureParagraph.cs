using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200011E RID: 286
	internal sealed class FigureParagraph : BaseParagraph
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x0010AEA3 File Offset: 0x00109EA3
		internal FigureParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0010E502 File Offset: 0x0010D502
		public override void Dispose()
		{
			base.Dispose();
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0010E524 File Offset: 0x0010D524
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			base.GetParaProperties(ref fspap, false);
			fspap.idobj = -2;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0010E538 File Offset: 0x0010D538
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			FigureParaClient figureParaClient = new FigureParaClient(this);
			paraClientHandle = figureParaClient.Handle;
			if (this._mainTextSegment == null)
			{
				this._mainTextSegment = new ContainerParagraph(base.Element, base.StructuralCache);
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0010E574 File Offset: 0x0010D574
		internal void GetFigureProperties(FigureParaClient paraClient, int fInTextLine, uint fswdir, int fBottomUndefined, out int dur, out int dvr, out PTS.FSFIGUREPROPS fsfigprops, out int cPolygons, out int cVertices, out int durDistTextLeft, out int durDistTextRight, out int dvrDistTextTop, out int dvrDistTextBottom)
		{
			Invariant.Assert(base.StructuralCache.CurrentFormatContext.FinitePage);
			PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			Figure figure = (Figure)base.Element;
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Element, base.StructuralCache.TextFormatterHost.PixelsPerDip);
			durDistTextLeft = (durDistTextRight = (dvrDistTextTop = (dvrDistTextBottom = 0)));
			bool flag;
			double width = FigureHelper.CalculateFigureWidth(base.StructuralCache, figure, figure.Width, out flag);
			double d = this.LimitTotalWidthFromAnchor(width, TextDpi.FromTextDpi(mbpInfo.MarginLeft + mbpInfo.MarginRight));
			int num = Math.Max(1, TextDpi.ToTextDpi(d) - (mbpInfo.BPLeft + mbpInfo.BPRight));
			bool flag2;
			double height = FigureHelper.CalculateFigureHeight(base.StructuralCache, figure, figure.Height, out flag2);
			double d2 = this.LimitTotalHeightFromAnchor(height, TextDpi.FromTextDpi(mbpInfo.MarginTop + mbpInfo.MarginBottom));
			int num2 = Math.Max(1, TextDpi.ToTextDpi(d2) - (mbpInfo.BPTop + mbpInfo.BPBottom));
			int num3 = 1;
			PTS.FSCOLUMNINFO[] array = new PTS.FSCOLUMNINFO[num3];
			array[0].durBefore = 0;
			array[0].durWidth = num;
			PTS.FSRECT fsrect = new PTS.FSRECT(0, 0, num, num2);
			PTS.FSFMTR fsfmtr;
			IntPtr intPtr;
			IntPtr intPtr2;
			PTS.FSBBOX fsbbox;
			IntPtr zero;
			int num4;
			this.CreateSubpageFiniteHelper(base.PtsContext, IntPtr.Zero, 0, this._mainTextSegment.Handle, IntPtr.Zero, 0, 1, fswdir, num, num2, ref fsrect, num3, array, 0, out fsfmtr, out intPtr, out intPtr2, out dvr, out fsbbox, out zero, out num4);
			if (intPtr2 != IntPtr.Zero)
			{
				PTS.Validate(PTS.FsDestroySubpageBreakRecord(base.PtsContext.Context, intPtr2));
			}
			if (PTS.ToBoolean(fsbbox.fDefined))
			{
				if (fsbbox.fsrc.du < num && flag)
				{
					if (intPtr != IntPtr.Zero)
					{
						PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, intPtr), base.PtsContext);
					}
					if (zero != IntPtr.Zero)
					{
						MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
						PTS.ValidateHandle(marginCollapsingState);
						marginCollapsingState.Dispose();
						zero = IntPtr.Zero;
					}
					num = fsbbox.fsrc.du + 1;
					array[0].durWidth = num;
					PTS.FSRECT fsrect2 = new PTS.FSRECT(0, 0, num, num2);
					PTS.FSFMTR fsfmtr2;
					IntPtr intPtr3;
					this.CreateSubpageFiniteHelper(base.PtsContext, IntPtr.Zero, 0, this._mainTextSegment.Handle, IntPtr.Zero, 0, 1, fswdir, num, num2, ref fsrect2, num3, array, 0, out fsfmtr2, out intPtr, out intPtr3, out dvr, out fsbbox, out zero, out num4);
					if (intPtr3 != IntPtr.Zero)
					{
						PTS.Validate(PTS.FsDestroySubpageBreakRecord(base.PtsContext.Context, intPtr3));
					}
				}
			}
			else
			{
				num = TextDpi.ToTextDpi(TextDpi.MinWidth);
			}
			dur = num + mbpInfo.MBPLeft + mbpInfo.MBPRight;
			if (zero != IntPtr.Zero)
			{
				MarginCollapsingState marginCollapsingState2 = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState2);
				marginCollapsingState2.Dispose();
				zero = IntPtr.Zero;
			}
			dvr += mbpInfo.MBPTop + mbpInfo.MBPBottom;
			if (!flag2)
			{
				dvr = TextDpi.ToTextDpi(d2) + mbpInfo.MarginTop + mbpInfo.MarginBottom;
			}
			FigureHorizontalAnchor horizontalAnchor = figure.HorizontalAnchor;
			FigureVerticalAnchor verticalAnchor = figure.VerticalAnchor;
			fsfigprops.fskrefU = (PTS.FSKREF)(horizontalAnchor / FigureHorizontalAnchor.ContentLeft);
			fsfigprops.fskrefV = (PTS.FSKREF)(verticalAnchor / FigureVerticalAnchor.ContentTop);
			fsfigprops.fskalfU = (PTS.FSKALIGNFIG)(horizontalAnchor % FigureHorizontalAnchor.ContentLeft);
			fsfigprops.fskalfV = (PTS.FSKALIGNFIG)(verticalAnchor % FigureVerticalAnchor.ContentTop);
			if (!PTS.ToBoolean(fInTextLine))
			{
				if (fsfigprops.fskrefU == PTS.FSKREF.fskrefChar)
				{
					fsfigprops.fskrefU = PTS.FSKREF.fskrefMargin;
					fsfigprops.fskalfU = PTS.FSKALIGNFIG.fskalfMin;
				}
				if (fsfigprops.fskrefV == PTS.FSKREF.fskrefChar)
				{
					fsfigprops.fskrefV = PTS.FSKREF.fskrefMargin;
					fsfigprops.fskalfV = PTS.FSKALIGNFIG.fskalfMin;
				}
			}
			fsfigprops.fskwrap = PTS.WrapDirectionToFskwrap(figure.WrapDirection);
			fsfigprops.fNonTextPlane = 0;
			fsfigprops.fAllowOverlap = 0;
			fsfigprops.fDelayable = PTS.FromBoolean(figure.CanDelayPlacement);
			cPolygons = (cVertices = 0);
			paraClient.SubpageHandle = intPtr;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0010E994 File Offset: 0x0010D994
		internal unsafe void GetFigurePolygons(FigureParaClient paraClient, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			ccVertices = (cfspt = (fWrapThrough = 0));
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0010E9B4 File Offset: 0x0010D9B4
		internal void CalcFigurePosition(FigureParaClient paraClient, uint fswdir, ref PTS.FSRECT fsrcPage, ref PTS.FSRECT fsrcMargin, ref PTS.FSRECT fsrcTrack, ref PTS.FSRECT fsrcFigurePreliminary, int fMustPosition, int fInTextLine, out int fPushToNextTrack, out PTS.FSRECT fsrcFlow, out PTS.FSRECT fsrcOverlap, out PTS.FSBBOX fsbbox, out PTS.FSRECT fsrcSearch)
		{
			Figure figure = (Figure)base.Element;
			FigureHorizontalAnchor horizontalAnchor = figure.HorizontalAnchor;
			FigureVerticalAnchor verticalAnchor = figure.VerticalAnchor;
			fsrcSearch = this.CalculateSearchArea(horizontalAnchor, verticalAnchor, ref fsrcPage, ref fsrcMargin, ref fsrcTrack, ref fsrcFigurePreliminary);
			if (verticalAnchor == FigureVerticalAnchor.ParagraphTop && fsrcFigurePreliminary.v != fsrcMargin.v && fsrcFigurePreliminary.v + fsrcFigurePreliminary.dv > fsrcTrack.v + fsrcTrack.dv && !PTS.ToBoolean(fMustPosition))
			{
				fPushToNextTrack = 1;
			}
			else
			{
				fPushToNextTrack = 0;
			}
			fsrcFlow = fsrcFigurePreliminary;
			if (FigureHelper.IsHorizontalColumnAnchor(horizontalAnchor))
			{
				fsrcFlow.u += this.CalculateParagraphToColumnOffset(horizontalAnchor, fsrcFigurePreliminary);
			}
			fsrcFlow.u += TextDpi.ToTextDpi(figure.HorizontalOffset);
			fsrcFlow.v += TextDpi.ToTextDpi(figure.VerticalOffset);
			fsrcOverlap = fsrcFlow;
			if (!FigureHelper.IsHorizontalPageAnchor(horizontalAnchor) && horizontalAnchor != FigureHorizontalAnchor.ColumnCenter && horizontalAnchor != FigureHorizontalAnchor.ContentCenter)
			{
				int num;
				double d;
				double d2;
				double num2;
				FigureHelper.GetColumnMetrics(base.StructuralCache, out num, out d, out d2, out num2);
				int num3 = TextDpi.ToTextDpi(d);
				int num4 = TextDpi.ToTextDpi(d2);
				int num5 = num3 + num4;
				int du = (fsrcOverlap.du / num5 + 1) * num5 - num4;
				fsrcOverlap.du = du;
				if (horizontalAnchor == FigureHorizontalAnchor.ContentRight || horizontalAnchor == FigureHorizontalAnchor.ColumnRight)
				{
					fsrcOverlap.u = fsrcFlow.u + fsrcFlow.du + num4 - fsrcOverlap.du;
				}
				fsrcSearch.u = fsrcOverlap.u;
				fsrcSearch.du = fsrcOverlap.du;
			}
			fsbbox = default(PTS.FSBBOX);
			fsbbox.fDefined = 1;
			fsbbox.fsrc = fsrcFlow;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0010EB61 File Offset: 0x0010DB61
		internal override void ClearUpdateInfo()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.ClearUpdateInfo();
			}
			base.ClearUpdateInfo();
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0010EB7C File Offset: 0x0010DB7C
		internal override bool InvalidateStructure(int startPosition)
		{
			if (this._mainTextSegment != null && this._mainTextSegment.InvalidateStructure(startPosition))
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
			return this._mainTextSegment == null;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0010EBAF File Offset: 0x0010DBAF
		internal override void InvalidateFormatCache()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.InvalidateFormatCache();
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0010EBC4 File Offset: 0x0010DBC4
		internal void UpdateSegmentLastFormatPositions()
		{
			this._mainTextSegment.UpdateLastFormatPositions();
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0010EBD4 File Offset: 0x0010DBD4
		private unsafe void CreateSubpageFiniteHelper(PtsContext ptsContext, IntPtr brParaIn, int fFromPreviousPage, IntPtr nSeg, IntPtr pFtnRej, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int lWidth, int lHeight, ref PTS.FSRECT rcMargin, int cColumns, PTS.FSCOLUMNINFO[] columnInfoCollection, int fApplyColumnBalancing, out PTS.FSFMTR fsfmtr, out IntPtr pSubPage, out IntPtr brParaOut, out int dvrUsed, out PTS.FSBBOX fsBBox, out IntPtr pfsMcsClient, out int topSpace)
		{
			base.StructuralCache.CurrentFormatContext.PushNewPageData(new Size(TextDpi.FromTextDpi(lWidth), TextDpi.FromTextDpi(lHeight)), default(Thickness), false, true);
			fixed (PTS.FSCOLUMNINFO[] array = columnInfoCollection)
			{
				PTS.FSCOLUMNINFO* rgColumnInfo;
				if (columnInfoCollection == null || array.Length == 0)
				{
					rgColumnInfo = null;
				}
				else
				{
					rgColumnInfo = &array[0];
				}
				PTS.Validate(PTS.FsCreateSubpageFinite(ptsContext.Context, brParaIn, fFromPreviousPage, nSeg, pFtnRej, fEmptyOk, fSuppressTopSpace, fswdir, lWidth, lHeight, ref rcMargin, cColumns, rgColumnInfo, 0, 0, null, null, 0, null, null, 0, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA.fsksuppresshardbreakbeforefirstparaNone, out fsfmtr, out pSubPage, out brParaOut, out dvrUsed, out fsBBox, out pfsMcsClient, out topSpace), ptsContext);
			}
			base.StructuralCache.CurrentFormatContext.PopPageData();
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0010EC7C File Offset: 0x0010DC7C
		private int CalculateParagraphToColumnOffset(FigureHorizontalAnchor horizontalAnchor, PTS.FSRECT fsrcInColumn)
		{
			Invariant.Assert(FigureHelper.IsHorizontalColumnAnchor(horizontalAnchor));
			int num;
			if (horizontalAnchor == FigureHorizontalAnchor.ColumnLeft)
			{
				num = fsrcInColumn.u;
			}
			else if (horizontalAnchor == FigureHorizontalAnchor.ColumnRight)
			{
				num = fsrcInColumn.u + fsrcInColumn.du - 1;
			}
			else
			{
				num = fsrcInColumn.u + fsrcInColumn.du / 2 - 1;
			}
			int num2;
			double num3;
			double num4;
			double num5;
			FigureHelper.GetColumnMetrics(base.StructuralCache, out num2, out num3, out num4, out num5);
			Invariant.Assert(num2 > 0);
			int num6 = TextDpi.ToTextDpi(num3 + num4);
			int num7 = (num - base.StructuralCache.CurrentFormatContext.PageMarginRect.u) / num6;
			int num8 = base.StructuralCache.CurrentFormatContext.PageMarginRect.u + num7 * num6;
			int num9 = TextDpi.ToTextDpi(num3);
			int num10 = num8 - fsrcInColumn.u;
			int num11 = num8 + num9 - (fsrcInColumn.u + fsrcInColumn.du);
			if (horizontalAnchor == FigureHorizontalAnchor.ColumnLeft)
			{
				return num10;
			}
			if (horizontalAnchor == FigureHorizontalAnchor.ColumnRight)
			{
				return num11;
			}
			return (num11 + num10) / 2;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0010ED64 File Offset: 0x0010DD64
		private double LimitTotalWidthFromAnchor(double width, double elementMarginWidth)
		{
			FigureHorizontalAnchor horizontalAnchor = ((Figure)base.Element).HorizontalAnchor;
			double num;
			if (FigureHelper.IsHorizontalPageAnchor(horizontalAnchor))
			{
				num = base.StructuralCache.CurrentFormatContext.PageWidth;
			}
			else if (FigureHelper.IsHorizontalContentAnchor(horizontalAnchor))
			{
				Thickness pageMargin = base.StructuralCache.CurrentFormatContext.PageMargin;
				num = base.StructuralCache.CurrentFormatContext.PageWidth - pageMargin.Left - pageMargin.Right;
			}
			else
			{
				int num2;
				double num3;
				double num4;
				double num5;
				FigureHelper.GetColumnMetrics(base.StructuralCache, out num2, out num3, out num4, out num5);
				num = num3;
			}
			if (width + elementMarginWidth > num)
			{
				width = Math.Max(TextDpi.MinWidth, num - elementMarginWidth);
			}
			return width;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0010EE10 File Offset: 0x0010DE10
		private double LimitTotalHeightFromAnchor(double height, double elementMarginHeight)
		{
			double num;
			if (FigureHelper.IsVerticalPageAnchor(((Figure)base.Element).VerticalAnchor))
			{
				num = base.StructuralCache.CurrentFormatContext.PageHeight;
			}
			else
			{
				Thickness pageMargin = base.StructuralCache.CurrentFormatContext.PageMargin;
				num = base.StructuralCache.CurrentFormatContext.PageHeight - pageMargin.Top - pageMargin.Bottom;
			}
			if (height + elementMarginHeight > num)
			{
				height = Math.Max(TextDpi.MinWidth, num - elementMarginHeight);
			}
			return height;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0010EE9C File Offset: 0x0010DE9C
		private PTS.FSRECT CalculateSearchArea(FigureHorizontalAnchor horizAnchor, FigureVerticalAnchor vertAnchor, ref PTS.FSRECT fsrcPage, ref PTS.FSRECT fsrcMargin, ref PTS.FSRECT fsrcTrack, ref PTS.FSRECT fsrcFigurePreliminary)
		{
			PTS.FSRECT result;
			if (FigureHelper.IsHorizontalPageAnchor(horizAnchor))
			{
				result.u = fsrcPage.u;
				result.du = fsrcPage.du;
			}
			else if (FigureHelper.IsHorizontalContentAnchor(horizAnchor))
			{
				result.u = fsrcMargin.u;
				result.du = fsrcMargin.du;
			}
			else
			{
				result.u = fsrcTrack.u;
				result.du = fsrcTrack.du;
			}
			if (FigureHelper.IsVerticalPageAnchor(vertAnchor))
			{
				result.v = fsrcPage.v;
				result.dv = fsrcPage.dv;
			}
			else if (FigureHelper.IsVerticalContentAnchor(vertAnchor))
			{
				result.v = fsrcMargin.v;
				result.dv = fsrcMargin.dv;
			}
			else
			{
				result.v = fsrcFigurePreliminary.v;
				result.dv = fsrcTrack.v + fsrcTrack.dv - fsrcFigurePreliminary.v;
			}
			return result;
		}

		// Token: 0x04000782 RID: 1922
		private BaseParagraph _mainTextSegment;
	}
}
