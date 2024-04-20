using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000142 RID: 322
	internal sealed class Section : UnmanagedHandle
	{
		// Token: 0x060009B1 RID: 2481 RVA: 0x0011EF9F File Offset: 0x0011DF9F
		internal Section(StructuralCache structuralCache) : base(structuralCache.PtsContext)
		{
			this._structuralCache = structuralCache;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0011EFB4 File Offset: 0x0011DFB4
		public override void Dispose()
		{
			this.DestroyStructure();
			base.Dispose();
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0011EFC2 File Offset: 0x0011DFC2
		internal void FSkipPage(out int fSkip)
		{
			fSkip = 0;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0011EFC8 File Offset: 0x0011DFC8
		internal void GetPageDimensions(out uint fswdir, out int fHeaderFooterAtTopBottom, out int durPage, out int dvrPage, ref PTS.FSRECT fsrcMargin)
		{
			Size pageSize = this._structuralCache.CurrentFormatContext.PageSize;
			durPage = TextDpi.ToTextDpi(pageSize.Width);
			dvrPage = TextDpi.ToTextDpi(pageSize.Height);
			Thickness pageMargin = this._structuralCache.CurrentFormatContext.PageMargin;
			TextDpi.EnsureValidPageMargin(ref pageMargin, pageSize);
			fsrcMargin.u = TextDpi.ToTextDpi(pageMargin.Left);
			fsrcMargin.v = TextDpi.ToTextDpi(pageMargin.Top);
			fsrcMargin.du = durPage - TextDpi.ToTextDpi(pageMargin.Left + pageMargin.Right);
			fsrcMargin.dv = dvrPage - TextDpi.ToTextDpi(pageMargin.Top + pageMargin.Bottom);
			this.StructuralCache.PageFlowDirection = (FlowDirection)this._structuralCache.PropertyOwner.GetValue(FrameworkElement.FlowDirectionProperty);
			fswdir = PTS.FlowDirectionToFswdir(this.StructuralCache.PageFlowDirection);
			fHeaderFooterAtTopBottom = 0;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0011F0B9 File Offset: 0x0011E0B9
		internal unsafe void GetJustificationProperties(IntPtr* rgnms, int cnms, int fLastSectionNotBroken, out int fJustify, out PTS.FSKALIGNPAGE fskal, out int fCancelAtLastColumn)
		{
			fJustify = 0;
			fCancelAtLastColumn = 0;
			fskal = PTS.FSKALIGNPAGE.fskalpgTop;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0011F0C7 File Offset: 0x0011E0C7
		internal void GetNextSection(out int fSuccess, out IntPtr nmsNext)
		{
			fSuccess = 0;
			nmsNext = IntPtr.Zero;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0011F0D4 File Offset: 0x0011E0D4
		internal void GetSectionProperties(out int fNewPage, out uint fswdir, out int fApplyColumnBalancing, out int ccol, out int cSegmentDefinedColumnSpanAreas, out int cHeightDefinedColumnSpanAreas)
		{
			ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this.Element);
			Size pageSize = this._structuralCache.CurrentFormatContext.PageSize;
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this.Element);
			Thickness pageMargin = this._structuralCache.CurrentFormatContext.PageMargin;
			double pageFontSize = (double)this._structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			FontFamily pageFontFamily = (FontFamily)this._structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			bool finitePage = this._structuralCache.CurrentFormatContext.FinitePage;
			fNewPage = 0;
			fswdir = PTS.FlowDirectionToFswdir((FlowDirection)this._structuralCache.PropertyOwner.GetValue(FrameworkElement.FlowDirectionProperty));
			fApplyColumnBalancing = 0;
			ccol = PtsHelper.CalculateColumnCount(columnProperties, lineHeightValue, pageSize.Width - (pageMargin.Left + pageMargin.Right), pageFontSize, pageFontFamily, finitePage);
			cSegmentDefinedColumnSpanAreas = 0;
			cHeightDefinedColumnSpanAreas = 0;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0011F1BB File Offset: 0x0011E1BB
		internal void GetMainTextSegment(out IntPtr nmSegment)
		{
			if (this._mainTextSegment == null)
			{
				this._mainTextSegment = new ContainerParagraph(this.Element, this._structuralCache);
			}
			nmSegment = this._mainTextSegment.Handle;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0011F1EC File Offset: 0x0011E1EC
		internal void GetHeaderSegment(IntPtr pfsbrpagePrelim, uint fswdir, out int fHeaderPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirHeader, out IntPtr nmsHeader)
		{
			fHeaderPresent = 0;
			fHardMargin = 0;
			dvrMaxHeight = (dvrFromEdge = 0);
			fswdirHeader = fswdir;
			nmsHeader = IntPtr.Zero;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0011F1EC File Offset: 0x0011E1EC
		internal void GetFooterSegment(IntPtr pfsbrpagePrelim, uint fswdir, out int fFooterPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirFooter, out IntPtr nmsFooter)
		{
			fFooterPresent = 0;
			fHardMargin = 0;
			dvrMaxHeight = (dvrFromEdge = 0);
			fswdirFooter = fswdir;
			nmsFooter = IntPtr.Zero;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0011F218 File Offset: 0x0011E218
		internal unsafe void GetSectionColumnInfo(uint fswdir, int ncol, PTS.FSCOLUMNINFO* pfscolinfo, out int ccol)
		{
			ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this.Element);
			Size pageSize = this._structuralCache.CurrentFormatContext.PageSize;
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this.Element);
			Thickness pageMargin = this._structuralCache.CurrentFormatContext.PageMargin;
			double pageFontSize = (double)this._structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
			FontFamily pageFontFamily = (FontFamily)this._structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
			bool finitePage = this._structuralCache.CurrentFormatContext.FinitePage;
			ccol = ncol;
			PtsHelper.GetColumnsInfo(columnProperties, lineHeightValue, pageSize.Width - (pageMargin.Left + pageMargin.Right), pageFontSize, pageFontFamily, ncol, pfscolinfo, finitePage);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0011F0C7 File Offset: 0x0011E0C7
		internal void GetEndnoteSegment(out int fEndnotesPresent, out IntPtr nmsEndnotes)
		{
			fEndnotesPresent = 0;
			nmsEndnotes = IntPtr.Zero;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0011F2CF File Offset: 0x0011E2CF
		internal void GetEndnoteSeparators(out IntPtr nmsEndnoteSeparator, out IntPtr nmsEndnoteContSeparator, out IntPtr nmsEndnoteContNotice)
		{
			nmsEndnoteSeparator = IntPtr.Zero;
			nmsEndnoteContSeparator = IntPtr.Zero;
			nmsEndnoteContNotice = IntPtr.Zero;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0011F2E6 File Offset: 0x0011E2E6
		internal void InvalidateFormatCache()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.InvalidateFormatCache();
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0011F2FB File Offset: 0x0011E2FB
		internal void ClearUpdateInfo()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.ClearUpdateInfo();
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0011F310 File Offset: 0x0011E310
		internal void InvalidateStructure()
		{
			if (this._mainTextSegment != null)
			{
				DtrList dtrList = this._structuralCache.DtrList;
				if (dtrList != null)
				{
					this._mainTextSegment.InvalidateStructure(dtrList[0].StartIndex);
				}
			}
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0011F34F File Offset: 0x0011E34F
		internal void DestroyStructure()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.Dispose();
				this._mainTextSegment = null;
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0011F36B File Offset: 0x0011E36B
		internal void UpdateSegmentLastFormatPositions()
		{
			if (this._mainTextSegment != null)
			{
				this._mainTextSegment.UpdateLastFormatPositions();
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060009C3 RID: 2499 RVA: 0x0011F380 File Offset: 0x0011E380
		internal bool CanUpdate
		{
			get
			{
				return this._mainTextSegment != null;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0011F38B File Offset: 0x0011E38B
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x0011F393 File Offset: 0x0011E393
		internal DependencyObject Element
		{
			get
			{
				return this._structuralCache.PropertyOwner;
			}
		}

		// Token: 0x040007F3 RID: 2035
		private BaseParagraph _mainTextSegment;

		// Token: 0x040007F4 RID: 2036
		private readonly StructuralCache _structuralCache;
	}
}
