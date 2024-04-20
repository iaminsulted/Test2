using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000139 RID: 313
	internal sealed class PtsHost
	{
		// Token: 0x060008BC RID: 2236 RVA: 0x001185E6 File Offset: 0x001175E6
		internal PtsHost()
		{
			this._context = new SecurityCriticalDataForSet<IntPtr>(IntPtr.Zero);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x001185FE File Offset: 0x001175FE
		internal void EnterContext(PtsContext ptsContext)
		{
			Invariant.Assert(this._ptsContext == null);
			this._ptsContext = ptsContext;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00118615 File Offset: 0x00117615
		internal void LeaveContext(PtsContext ptsContext)
		{
			Invariant.Assert(this._ptsContext == ptsContext);
			this._ptsContext = null;
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0011862C File Offset: 0x0011762C
		private PtsContext PtsContext
		{
			get
			{
				Invariant.Assert(this._ptsContext != null);
				return this._ptsContext;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00118642 File Offset: 0x00117642
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x00118669 File Offset: 0x00117669
		internal IntPtr Context
		{
			get
			{
				Invariant.Assert(this._context.Value != IntPtr.Zero);
				return this._context.Value;
			}
			set
			{
				Invariant.Assert(this._context.Value == IntPtr.Zero);
				this._context.Value = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00118691 File Offset: 0x00117691
		internal static int ContainerParagraphId
		{
			get
			{
				return PtsHost._customParaId;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00118698 File Offset: 0x00117698
		internal static int SubpageParagraphId
		{
			get
			{
				return PtsHost.ContainerParagraphId + 1;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060008C4 RID: 2244 RVA: 0x001186A1 File Offset: 0x001176A1
		internal static int FloaterParagraphId
		{
			get
			{
				return PtsHost.SubpageParagraphId + 1;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x001186AA File Offset: 0x001176AA
		internal static int TableParagraphId
		{
			get
			{
				return PtsHost.FloaterParagraphId + 1;
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x001186B3 File Offset: 0x001176B3
		internal void AssertFailed(string arg1, string arg2, int arg3, uint arg4)
		{
			if (!PtsCache.IsDisposed())
			{
				ErrorHandler.Assert(false, ErrorHandler.PTSAssert, new object[]
				{
					arg1,
					arg2,
					arg3,
					arg4
				});
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x001186E8 File Offset: 0x001176E8
		internal int GetFigureProperties(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, int fInTextLine, uint fswdir, int fBottomUndefined, out int dur, out int dvr, out PTS.FSFIGUREPROPS fsfigprops, out int cPolygons, out int cVertices, out int durDistTextLeft, out int durDistTextRight, out int dvrDistTextTop, out int dvrDistTextBottom)
		{
			int result = 0;
			try
			{
				FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
				PTS.ValidateHandle(figureParagraph);
				FigureParaClient figureParaClient = this.PtsContext.HandleToObject(pfsparaclientFigure) as FigureParaClient;
				PTS.ValidateHandle(figureParaClient);
				figureParagraph.GetFigureProperties(figureParaClient, fInTextLine, fswdir, fBottomUndefined, out dur, out dvr, out fsfigprops, out cPolygons, out cVertices, out durDistTextLeft, out durDistTextRight, out dvrDistTextTop, out dvrDistTextBottom);
			}
			catch (Exception callbackException)
			{
				dur = (dvr = (cPolygons = (cVertices = 0)));
				fsfigprops = default(PTS.FSFIGUREPROPS);
				durDistTextLeft = (durDistTextRight = (dvrDistTextTop = (dvrDistTextBottom = 0)));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dur = (dvr = (cPolygons = (cVertices = 0)));
				fsfigprops = default(PTS.FSFIGUREPROPS);
				durDistTextLeft = (durDistTextRight = (dvrDistTextTop = (dvrDistTextBottom = 0)));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00118804 File Offset: 0x00117804
		internal unsafe int GetFigurePolygons(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			int result = 0;
			try
			{
				FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
				PTS.ValidateHandle(figureParagraph);
				FigureParaClient figureParaClient = this.PtsContext.HandleToObject(pfsparaclientFigure) as FigureParaClient;
				PTS.ValidateHandle(figureParaClient);
				figureParagraph.GetFigurePolygons(figureParaClient, fswdir, ncVertices, nfspt, rgcVertices, out ccVertices, rgfspt, out cfspt, out fWrapThrough);
			}
			catch (Exception callbackException)
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				rgfspt = null;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				rgfspt = null;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x001188D4 File Offset: 0x001178D4
		internal int CalcFigurePosition(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, ref PTS.FSRECT fsrcPage, ref PTS.FSRECT fsrcMargin, ref PTS.FSRECT fsrcTrack, ref PTS.FSRECT fsrcFigurePreliminary, int fMustPosition, int fInTextLine, out int fPushToNextTrack, out PTS.FSRECT fsrcFlow, out PTS.FSRECT fsrcOverlap, out PTS.FSBBOX fsbbox, out PTS.FSRECT fsrcSearch)
		{
			int result = 0;
			try
			{
				FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
				PTS.ValidateHandle(figureParagraph);
				FigureParaClient figureParaClient = this.PtsContext.HandleToObject(pfsparaclientFigure) as FigureParaClient;
				PTS.ValidateHandle(figureParaClient);
				figureParagraph.CalcFigurePosition(figureParaClient, fswdir, ref fsrcPage, ref fsrcMargin, ref fsrcTrack, ref fsrcFigurePreliminary, fMustPosition, fInTextLine, out fPushToNextTrack, out fsrcFlow, out fsrcOverlap, out fsbbox, out fsrcSearch);
			}
			catch (Exception callbackException)
			{
				fPushToNextTrack = 0;
				fsrcFlow = (fsrcOverlap = (fsrcSearch = default(PTS.FSRECT)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fPushToNextTrack = 0;
				fsrcFlow = (fsrcOverlap = (fsrcSearch = default(PTS.FSRECT)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x001189E8 File Offset: 0x001179E8
		internal int FSkipPage(IntPtr pfsclient, IntPtr nms, out int fSkip)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.FSkipPage(out fSkip);
			}
			catch (Exception callbackException)
			{
				fSkip = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fSkip = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00118A6C File Offset: 0x00117A6C
		internal int GetPageDimensions(IntPtr pfsclient, IntPtr nms, out uint fswdir, out int fHeaderFooterAtTopBottom, out int durPage, out int dvrPage, ref PTS.FSRECT fsrcMargin)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetPageDimensions(out fswdir, out fHeaderFooterAtTopBottom, out durPage, out dvrPage, ref fsrcMargin);
			}
			catch (Exception callbackException)
			{
				fswdir = 0U;
				fHeaderFooterAtTopBottom = (durPage = (dvrPage = 0));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fswdir = 0U;
				fHeaderFooterAtTopBottom = (durPage = (dvrPage = 0));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00118B18 File Offset: 0x00117B18
		internal int GetNextSection(IntPtr pfsclient, IntPtr nmsCur, out int fSuccess, out IntPtr nmsNext)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsCur) as Section;
				PTS.ValidateHandle(section);
				section.GetNextSection(out fSuccess, out nmsNext);
			}
			catch (Exception callbackException)
			{
				fSuccess = 0;
				nmsNext = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fSuccess = 0;
				nmsNext = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00118BB0 File Offset: 0x00117BB0
		internal int GetSectionProperties(IntPtr pfsclient, IntPtr nms, out int fNewPage, out uint fswdir, out int fApplyColumnBalancing, out int ccol, out int cSegmentDefinedColumnSpanAreas, out int cHeightDefinedColumnSpanAreas)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetSectionProperties(out fNewPage, out fswdir, out fApplyColumnBalancing, out ccol, out cSegmentDefinedColumnSpanAreas, out cHeightDefinedColumnSpanAreas);
			}
			catch (Exception callbackException)
			{
				fNewPage = (fApplyColumnBalancing = (ccol = 0));
				fswdir = 0U;
				cSegmentDefinedColumnSpanAreas = (cHeightDefinedColumnSpanAreas = 0);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fNewPage = (fApplyColumnBalancing = (ccol = 0));
				fswdir = 0U;
				cSegmentDefinedColumnSpanAreas = (cHeightDefinedColumnSpanAreas = 0);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00118C74 File Offset: 0x00117C74
		internal unsafe int GetJustificationProperties(IntPtr pfsclient, IntPtr* rgnms, int cnms, int fLastSectionNotBroken, out int fJustify, out PTS.FSKALIGNPAGE fskal, out int fCancelAtLastColumn)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(*rgnms) as Section;
				PTS.ValidateHandle(section);
				section.GetJustificationProperties(rgnms, cnms, fLastSectionNotBroken, out fJustify, out fskal, out fCancelAtLastColumn);
			}
			catch (Exception callbackException)
			{
				fJustify = (fCancelAtLastColumn = 0);
				fskal = PTS.FSKALIGNPAGE.fskalpgTop;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fJustify = (fCancelAtLastColumn = 0);
				fskal = PTS.FSKALIGNPAGE.fskalpgTop;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00118D18 File Offset: 0x00117D18
		internal int GetMainTextSegment(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmSegment)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsSection) as Section;
				PTS.ValidateHandle(section);
				section.GetMainTextSegment(out nmSegment);
			}
			catch (Exception callbackException)
			{
				nmSegment = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nmSegment = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00118DA4 File Offset: 0x00117DA4
		internal int GetHeaderSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fHeaderPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirHeader, out IntPtr nmsHeader)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetHeaderSegment(pfsbrpagePrelim, fswdir, out fHeaderPresent, out fHardMargin, out dvrMaxHeight, out dvrFromEdge, out fswdirHeader, out nmsHeader);
			}
			catch (Exception callbackException)
			{
				fHeaderPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirHeader = 0U;
				nmsHeader = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fHeaderPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirHeader = 0U;
				nmsHeader = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00118E74 File Offset: 0x00117E74
		internal int GetFooterSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fFooterPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirFooter, out IntPtr nmsFooter)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetFooterSegment(pfsbrpagePrelim, fswdir, out fFooterPresent, out fHardMargin, out dvrMaxHeight, out dvrFromEdge, out fswdirFooter, out nmsFooter);
			}
			catch (Exception callbackException)
			{
				fFooterPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirFooter = 0U;
				nmsFooter = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFooterPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirFooter = 0U;
				nmsFooter = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00118F44 File Offset: 0x00117F44
		internal int UpdGetSegmentChange(IntPtr pfsclient, IntPtr nms, out PTS.FSKCHANGE fskch)
		{
			int result = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nms) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				containerParagraph.UpdGetSegmentChange(out fskch);
			}
			catch (Exception callbackException)
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00118FC8 File Offset: 0x00117FC8
		internal unsafe int GetSectionColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncol, PTS.FSCOLUMNINFO* fscolinfo, out int ccol)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetSectionColumnInfo(fswdir, ncol, fscolinfo, out ccol);
			}
			catch (Exception callbackException)
			{
				ccol = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ccol = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00119054 File Offset: 0x00118054
		internal unsafe int GetSegmentDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, IntPtr* rgnmSeg, int* rgcColumns, out int cAreasActual)
		{
			cAreasActual = 0;
			return -10000;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00119054 File Offset: 0x00118054
		internal unsafe int GetHeightDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, int* rgdvrAreaHeight, int* rgcColumns, out int cAreasActual)
		{
			cAreasActual = 0;
			return -10000;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00119060 File Offset: 0x00118060
		internal int GetFirstPara(IntPtr pfsclient, IntPtr nms, out int fSuccessful, out IntPtr nmp)
		{
			int result = 0;
			try
			{
				ISegment segment = this.PtsContext.HandleToObject(nms) as ISegment;
				PTS.ValidateHandle(segment);
				segment.GetFirstPara(out fSuccessful, out nmp);
			}
			catch (Exception callbackException)
			{
				fSuccessful = 0;
				nmp = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fSuccessful = 0;
				nmp = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x001190F8 File Offset: 0x001180F8
		internal int GetNextPara(IntPtr pfsclient, IntPtr nms, IntPtr nmpCur, out int fFound, out IntPtr nmpNext)
		{
			int result = 0;
			try
			{
				ISegment segment = this.PtsContext.HandleToObject(nms) as ISegment;
				PTS.ValidateHandle(segment);
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmpCur) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				segment.GetNextPara(baseParagraph, out fFound, out nmpNext);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				nmpNext = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				nmpNext = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x001191AC File Offset: 0x001181AC
		internal int UpdGetFirstChangeInSegment(IntPtr pfsclient, IntPtr nms, out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange)
		{
			int result = 0;
			try
			{
				ISegment segment = this.PtsContext.HandleToObject(nms) as ISegment;
				PTS.ValidateHandle(segment);
				segment.UpdGetFirstChangeInSegment(out fFound, out fChangeFirst, out nmpBeforeChange);
			}
			catch (Exception callbackException)
			{
				fFound = (fChangeFirst = 0);
				nmpBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = (fChangeFirst = 0);
				nmpBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00119250 File Offset: 0x00118250
		internal int UpdGetParaChange(IntPtr pfsclient, IntPtr nmp, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				baseParagraph.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			}
			catch (Exception callbackException)
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x001192E0 File Offset: 0x001182E0
		internal int GetParaProperties(IntPtr pfsclient, IntPtr nmp, ref PTS.FSPAP fspap)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				baseParagraph.GetParaProperties(ref fspap);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00119360 File Offset: 0x00118360
		internal int CreateParaclient(IntPtr pfsclient, IntPtr nmp, out IntPtr pfsparaclient)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				baseParagraph.CreateParaclient(out pfsparaclient);
			}
			catch (Exception callbackException)
			{
				pfsparaclient = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsparaclient = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x001193EC File Offset: 0x001183EC
		internal int TransferDisplayInfo(IntPtr pfsclient, IntPtr pfsparaclientOld, IntPtr pfsparaclientNew)
		{
			int result = 0;
			try
			{
				BaseParaClient baseParaClient = this.PtsContext.HandleToObject(pfsparaclientOld) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				BaseParaClient baseParaClient2 = this.PtsContext.HandleToObject(pfsparaclientNew) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient2);
				baseParaClient2.TransferDisplayInfo(baseParaClient);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00119484 File Offset: 0x00118484
		internal int DestroyParaclient(IntPtr pfsclient, IntPtr pfsparaclient)
		{
			int result = 0;
			try
			{
				BaseParaClient baseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				baseParaClient.Dispose();
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00119504 File Offset: 0x00118504
		internal int FInterruptFormattingAfterPara(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int vr, out int fInterruptFormatting)
		{
			fInterruptFormatting = 0;
			return 0;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0011950C File Offset: 0x0011850C
		internal int GetEndnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsEndnoteSeparator, out IntPtr nmsEndnoteContSeparator, out IntPtr nmsEndnoteContNotice)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsSection) as Section;
				PTS.ValidateHandle(section);
				section.GetEndnoteSeparators(out nmsEndnoteSeparator, out nmsEndnoteContSeparator, out nmsEndnoteContNotice);
			}
			catch (Exception callbackException)
			{
				nmsEndnoteSeparator = (nmsEndnoteContSeparator = (nmsEndnoteContNotice = IntPtr.Zero));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nmsEndnoteSeparator = (nmsEndnoteContSeparator = (nmsEndnoteContNotice = IntPtr.Zero));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x001195B4 File Offset: 0x001185B4
		internal int GetEndnoteSegment(IntPtr pfsclient, IntPtr nmsSection, out int fEndnotesPresent, out IntPtr nmsEndnotes)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsSection) as Section;
				PTS.ValidateHandle(section);
				section.GetEndnoteSegment(out fEndnotesPresent, out nmsEndnotes);
			}
			catch (Exception callbackException)
			{
				fEndnotesPresent = 0;
				nmsEndnotes = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fEndnotesPresent = 0;
				nmsEndnotes = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0011964C File Offset: 0x0011864C
		internal int GetNumberEndnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolEndnote)
		{
			ccolEndnote = 0;
			return -10000;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00119054 File Offset: 0x00118054
		internal unsafe int GetEndnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolEndnote, PTS.FSCOLUMNINFO* fscolinfoEndnote, out int ccolEndnote)
		{
			ccolEndnote = 0;
			return -10000;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00119658 File Offset: 0x00118658
		internal int GetFootnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsFtnSeparator, out IntPtr nmsFtnContSeparator, out IntPtr nmsFtnContNotice)
		{
			nmsFtnSeparator = (nmsFtnContSeparator = (nmsFtnContNotice = IntPtr.Zero));
			return -10000;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0011964C File Offset: 0x0011864C
		internal int FFootnoteBeneathText(IntPtr pfsclient, IntPtr nms, out int fFootnoteBeneathText)
		{
			fFootnoteBeneathText = 0;
			return -10000;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0011964C File Offset: 0x0011864C
		internal int GetNumberFootnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolFootnote)
		{
			ccolFootnote = 0;
			return -10000;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00119054 File Offset: 0x00118054
		internal unsafe int GetFootnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolFootnote, PTS.FSCOLUMNINFO* fscolinfoFootnote, out int ccolFootnote)
		{
			ccolFootnote = 0;
			return -10000;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0011967D File Offset: 0x0011867D
		internal int GetFootnoteSegment(IntPtr pfsclient, IntPtr nmftn, out IntPtr nmsFootnote)
		{
			nmsFootnote = IntPtr.Zero;
			return -10000;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0011968C File Offset: 0x0011868C
		internal unsafe int GetFootnotePresentationAndRejectionOrder(IntPtr pfsclient, int cFootnotes, IntPtr* rgProposedPresentationOrder, IntPtr* rgProposedRejectionOrder, out int fProposedPresentationOrderAccepted, IntPtr* rgFinalPresentationOrder, out int fProposedRejectionOrderAccepted, IntPtr* rgFinalRejectionOrder)
		{
			fProposedPresentationOrderAccepted = (fProposedRejectionOrderAccepted = 0);
			return -10000;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0011964C File Offset: 0x0011864C
		internal int FAllowFootnoteSeparation(IntPtr pfsclient, IntPtr nmftn, out int fAllow)
		{
			fAllow = 0;
			return -10000;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x001196A8 File Offset: 0x001186A8
		internal int DuplicateMcsclient(IntPtr pfsclient, IntPtr pmcsclientIn, out IntPtr pmcsclientNew)
		{
			int result = 0;
			try
			{
				MarginCollapsingState marginCollapsingState = this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				pmcsclientNew = marginCollapsingState.Clone().Handle;
			}
			catch (Exception callbackException)
			{
				pmcsclientNew = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pmcsclientNew = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0011973C File Offset: 0x0011873C
		internal int DestroyMcsclient(IntPtr pfsclient, IntPtr pmcsclient)
		{
			int result = 0;
			try
			{
				MarginCollapsingState marginCollapsingState = this.PtsContext.HandleToObject(pmcsclient) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				marginCollapsingState.Dispose();
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x001197BC File Offset: 0x001187BC
		internal int FEqualMcsclient(IntPtr pfsclient, IntPtr pmcsclient1, IntPtr pmcsclient2, out int fEqual)
		{
			int result = 0;
			if (pmcsclient1 == IntPtr.Zero || pmcsclient2 == IntPtr.Zero)
			{
				fEqual = PTS.FromBoolean(pmcsclient1 == pmcsclient2);
			}
			else
			{
				try
				{
					MarginCollapsingState marginCollapsingState = this.PtsContext.HandleToObject(pmcsclient1) as MarginCollapsingState;
					PTS.ValidateHandle(marginCollapsingState);
					MarginCollapsingState marginCollapsingState2 = this.PtsContext.HandleToObject(pmcsclient2) as MarginCollapsingState;
					PTS.ValidateHandle(marginCollapsingState2);
					fEqual = PTS.FromBoolean(marginCollapsingState.IsEqual(marginCollapsingState2));
				}
				catch (Exception callbackException)
				{
					fEqual = 0;
					this.PtsContext.CallbackException = callbackException;
					result = -100002;
				}
				catch
				{
					fEqual = 0;
					this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
					result = -100002;
				}
			}
			return result;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00119890 File Offset: 0x00118890
		internal int ConvertMcsclient(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, uint fswdir, IntPtr pmcsclient, int fSuppressTopSpace, out int dvr)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				BaseParaClient baseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclient != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclient) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				baseParagraph.CollapseMargin(baseParaClient, marginCollapsingState, fswdir, PTS.ToBoolean(fSuppressTopSpace), out dvr);
			}
			catch (Exception callbackException)
			{
				dvr = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvr = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00119964 File Offset: 0x00118964
		internal int GetObjectHandlerInfo(IntPtr pfsclient, int idobj, IntPtr pObjectInfo)
		{
			int result = 0;
			try
			{
				if (idobj == PtsHost.FloaterParagraphId)
				{
					PtsCache.GetFloaterHandlerInfo(this, pObjectInfo);
				}
				else if (idobj == PtsHost.TableParagraphId)
				{
					PtsCache.GetTableObjHandlerInfo(this, pObjectInfo);
				}
				else
				{
					pObjectInfo = IntPtr.Zero;
				}
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x001199F0 File Offset: 0x001189F0
		internal int CreateParaBreakingSession(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int fsdcpStart, IntPtr pfsbreakreclineclient, uint fswdir, int urStartTrack, int durTrack, int urPageLeftMargin, out IntPtr ppfsparabreakingsession, out int fParagraphJustified)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				LineBreakRecord lineBreakRecord = null;
				if (pfsbreakreclineclient != IntPtr.Zero)
				{
					lineBreakRecord = (this.PtsContext.HandleToObject(pfsbreakreclineclient) as LineBreakRecord);
					PTS.ValidateHandle(lineBreakRecord);
				}
				OptimalBreakSession optimalBreakSession;
				bool condition;
				textParagraph.CreateOptimalBreakSession(textParaClient, fsdcpStart, durTrack, lineBreakRecord, out optimalBreakSession, out condition);
				fParagraphJustified = PTS.FromBoolean(condition);
				ppfsparabreakingsession = optimalBreakSession.Handle;
			}
			catch (Exception callbackException)
			{
				ppfsparabreakingsession = IntPtr.Zero;
				fParagraphJustified = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ppfsparabreakingsession = IntPtr.Zero;
				fParagraphJustified = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00119AE8 File Offset: 0x00118AE8
		internal int DestroyParaBreakingSession(IntPtr pfsclient, IntPtr pfsparabreakingsession)
		{
			int result = 0;
			OptimalBreakSession optimalBreakSession = this.PtsContext.HandleToObject(pfsparabreakingsession) as OptimalBreakSession;
			PTS.ValidateHandle(optimalBreakSession);
			optimalBreakSession.Dispose();
			return result;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x00119B08 File Offset: 0x00118B08
		internal int GetTextProperties(IntPtr pfsclient, IntPtr nmp, int iArea, ref PTS.FSTXTPROPS fstxtprops)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.GetTextProperties(iArea, ref fstxtprops);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00119B88 File Offset: 0x00118B88
		internal int GetNumberFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, out int nFootnote)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.GetNumberFootnotes(fsdcpStart, fsdcpLim, out nFootnote);
			}
			catch (Exception callbackException)
			{
				nFootnote = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nFootnote = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00119C14 File Offset: 0x00118C14
		internal unsafe int GetFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, int nFootnotes, IntPtr* rgnmftn, int* rgdcp, out int cFootnotes)
		{
			cFootnotes = 0;
			return -10000;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00119C20 File Offset: 0x00118C20
		internal int FormatDropCap(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int fSuppressTopSpace, out IntPtr pfsdropc, out int fInMargin, out int dur, out int dvr, out int cPolygons, out int cVertices, out int durText)
		{
			pfsdropc = IntPtr.Zero;
			fInMargin = (dur = (dvr = (cPolygons = (cVertices = (durText = 0)))));
			return -10000;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00119C5C File Offset: 0x00118C5C
		internal unsafe int GetDropCapPolygons(IntPtr pfsclient, IntPtr pfsdropc, IntPtr nmp, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			ccVertices = (cfspt = (fWrapThrough = 0));
			return -10000;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00119C7E File Offset: 0x00118C7E
		internal int DestroyDropCap(IntPtr pfsclient, IntPtr pfsdropc)
		{
			return -10000;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00119C88 File Offset: 0x00118C88
		internal int FormatBottomText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, IntPtr pfslineLast, int dvrLine, out IntPtr pmcsclientOut)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				Line line = this.PtsContext.HandleToObject(pfslineLast) as Line;
				if (line != null)
				{
					PTS.ValidateHandle(line);
					textParagraph.FormatBottomText(iArea, fswdir, line, dvrLine, out pmcsclientOut);
				}
				else
				{
					Invariant.Assert(this.PtsContext.HandleToObject(pfslineLast) is LineBreakpoint);
					pmcsclientOut = IntPtr.Zero;
				}
			}
			catch (Exception callbackException)
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00119D60 File Offset: 0x00118D60
		internal int FormatLine(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				textParagraph.FormatLine(textParaClient, iArea, dcp, pbrlineIn, fswdir, urStartLine, durLine, urStartTrack, durTrack, urPageLeftMargin, PTS.ToBoolean(fAllowHyphenation), PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), out pfsline, out dcpLine, out ppbrlineOut, out fForcedBroken, out fsflres, out dvrAscent, out dvrDescent, out urBBox, out durBBox, out dcpDepend, out fReformatNeighborsAsLastLine);
			}
			catch (Exception callbackException)
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00119EE8 File Offset: 0x00118EE8
		internal int FormatLineForced(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, int dvrAvailable, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				int num;
				int num2;
				textParagraph.FormatLine(textParaClient, iArea, dcp, pbrlineIn, fswdir, urStartLine, durLine, urStartTrack, durTrack, urPageLeftMargin, true, PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), out pfsline, out dcpLine, out ppbrlineOut, out num, out fsflres, out dvrAscent, out dvrDescent, out urBBox, out durBBox, out dcpDepend, out num2);
			}
			catch (Exception callbackException)
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = 0)))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = 0)))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0011A050 File Offset: 0x00119050
		internal unsafe int FormatLineVariants(IntPtr pfsclient, IntPtr pfsparabreakingsession, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, IntPtr lineVariantRestriction, int nLineVariantsAlloc, PTS.FSLINEVARIANT* rgfslinevariant, out int nLineVariantsActual, out int iLineVariantBest)
		{
			int result = 0;
			try
			{
				OptimalBreakSession optimalBreakSession = this.PtsContext.HandleToObject(pfsparabreakingsession) as OptimalBreakSession;
				PTS.ValidateHandle(optimalBreakSession);
				TextLineBreak textLineBreak = null;
				if (pbrlineIn != IntPtr.Zero)
				{
					LineBreakRecord lineBreakRecord = this.PtsContext.HandleToObject(pbrlineIn) as LineBreakRecord;
					PTS.ValidateHandle(lineBreakRecord);
					textLineBreak = lineBreakRecord.TextLineBreak;
				}
				IList<TextBreakpoint> list = optimalBreakSession.TextParagraph.FormatLineVariants(optimalBreakSession.TextParaClient, optimalBreakSession.TextParagraphCache, optimalBreakSession.OptimalTextSource, dcp, textLineBreak, fswdir, urStartLine, durLine, PTS.ToBoolean(fAllowHyphenation), PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), lineVariantRestriction, out iLineVariantBest);
				for (int i = 0; i < Math.Min(list.Count, nLineVariantsAlloc); i++)
				{
					TextBreakpoint textBreakpoint = list[i];
					LineBreakpoint lineBreakpoint = new LineBreakpoint(optimalBreakSession, textBreakpoint);
					TextLineBreak textLineBreak2 = textBreakpoint.GetTextLineBreak();
					if (textLineBreak2 != null)
					{
						LineBreakRecord lineBreakRecord2 = new LineBreakRecord(optimalBreakSession.PtsContext, textLineBreak2);
						rgfslinevariant[i].pfsbreakreclineclient = lineBreakRecord2.Handle;
					}
					else
					{
						rgfslinevariant[i].pfsbreakreclineclient = IntPtr.Zero;
					}
					int dvrAscent = TextDpi.ToTextDpi(textBreakpoint.Baseline);
					int dvrDescent = TextDpi.ToTextDpi(textBreakpoint.Height - textBreakpoint.Baseline);
					optimalBreakSession.TextParagraph.CalcLineAscentDescent(dcp, ref dvrAscent, ref dvrDescent);
					rgfslinevariant[i].pfslineclient = lineBreakpoint.Handle;
					rgfslinevariant[i].dcpLine = textBreakpoint.Length;
					rgfslinevariant[i].fForceBroken = PTS.FromBoolean(textBreakpoint.IsTruncated);
					rgfslinevariant[i].fslres = optimalBreakSession.OptimalTextSource.GetFormatResultForBreakpoint(dcp, textBreakpoint);
					rgfslinevariant[i].dvrAscent = dvrAscent;
					rgfslinevariant[i].dvrDescent = dvrDescent;
					rgfslinevariant[i].fReformatNeighborsAsLastLine = 0;
					rgfslinevariant[i].ptsLinePenaltyInfo = textBreakpoint.GetTextPenaltyResource().Value;
				}
				nLineVariantsActual = list.Count;
			}
			catch (Exception callbackException)
			{
				nLineVariantsActual = 0;
				iLineVariantBest = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nLineVariantsActual = 0;
				iLineVariantBest = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0011A304 File Offset: 0x00119304
		internal int ReconstructLineVariant(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcpStart, IntPtr pbrlineIn, int dcpLine, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				textParagraph.ReconstructLineVariant(textParaClient, iArea, dcpStart, pbrlineIn, dcpLine, fswdir, urStartLine, durLine, urStartTrack, durTrack, urPageLeftMargin, PTS.ToBoolean(fAllowHyphenation), PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), out pfsline, out dcpLine, out ppbrlineOut, out fForcedBroken, out fsflres, out dvrAscent, out dvrDescent, out urBBox, out durBBox, out dcpDepend, out fReformatNeighborsAsLastLine);
			}
			catch (Exception callbackException)
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0011A48C File Offset: 0x0011948C
		internal int DestroyLine(IntPtr pfsclient, IntPtr pfsline)
		{
			((UnmanagedHandle)this.PtsContext.HandleToObject(pfsline)).Dispose();
			return 0;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0011A4A8 File Offset: 0x001194A8
		internal int DuplicateLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn, out IntPtr pbrlineDup)
		{
			int result = 0;
			try
			{
				LineBreakRecord lineBreakRecord = this.PtsContext.HandleToObject(pbrlineIn) as LineBreakRecord;
				PTS.ValidateHandle(lineBreakRecord);
				pbrlineDup = lineBreakRecord.Clone().Handle;
			}
			catch (Exception callbackException)
			{
				pbrlineDup = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pbrlineDup = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0011A53C File Offset: 0x0011953C
		internal int DestroyLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn)
		{
			int result = 0;
			try
			{
				LineBreakRecord lineBreakRecord = this.PtsContext.HandleToObject(pbrlineIn) as LineBreakRecord;
				PTS.ValidateHandle(lineBreakRecord);
				lineBreakRecord.Dispose();
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0011A5BC File Offset: 0x001195BC
		internal int SnapGridVertical(IntPtr pfsclient, uint fswdir, int vrMargin, int vrCurrent, out int vrNew)
		{
			vrNew = 0;
			return -10000;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0011A5C8 File Offset: 0x001195C8
		internal int GetDvrSuppressibleBottomSpace(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsline, uint fswdir, out int dvrSuppressible)
		{
			int result = 0;
			try
			{
				Line line = this.PtsContext.HandleToObject(pfsline) as Line;
				if (line != null)
				{
					PTS.ValidateHandle(line);
					line.GetDvrSuppressibleBottomSpace(out dvrSuppressible);
				}
				else
				{
					dvrSuppressible = 0;
				}
			}
			catch (Exception callbackException)
			{
				dvrSuppressible = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvrSuppressible = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0011A65C File Offset: 0x0011965C
		internal int GetDvrAdvance(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, uint fswdir, out int dvr)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.GetDvrAdvance(dcp, fswdir, out dvr);
			}
			catch (Exception callbackException)
			{
				dvr = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvr = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0011A6E8 File Offset: 0x001196E8
		internal int UpdGetChangeInText(IntPtr pfsclient, IntPtr nmp, out int dcpStart, out int ddcpOld, out int ddcpNew)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.UpdGetChangeInText(out dcpStart, out ddcpOld, out ddcpNew);
			}
			catch (Exception callbackException)
			{
				dcpStart = (ddcpOld = (ddcpNew = 0));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dcpStart = (ddcpOld = (ddcpNew = 0));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0011964C File Offset: 0x0011864C
		internal int UpdGetDropCapChange(IntPtr pfsclient, IntPtr nmp, out int fChanged)
		{
			fChanged = 0;
			return -10000;
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0011A788 File Offset: 0x00119788
		internal int FInterruptFormattingText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, int vr, out int fInterruptFormatting)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				fInterruptFormatting = PTS.FromBoolean(textParagraph.InterruptFormatting(dcp, vr));
			}
			catch (Exception callbackException)
			{
				fInterruptFormatting = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fInterruptFormatting = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0011A81C File Offset: 0x0011981C
		internal int GetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, out int fFound, out int dcpPara, out int urBBox, out int durBBox, out int dvrPara, out PTS.FSKCLEAR fskclear, out IntPtr pmcsclientAfterPara, out int cLines, out int fOptimalLines, out int fOptimalLineDcpsCached, out int dvrMinLineHeight)
		{
			fFound = 0;
			dcpPara = (urBBox = (durBBox = (dvrPara = (cLines = (dvrMinLineHeight = (fOptimalLines = (fOptimalLineDcpsCached = 0)))))));
			pmcsclientAfterPara = IntPtr.Zero;
			fskclear = PTS.FSKCLEAR.fskclearNone;
			return 0;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00105F35 File Offset: 0x00104F35
		internal unsafe int SetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, int dcpPara, int urBBox, int durBBox, int dvrPara, PTS.FSKCLEAR fskclear, IntPtr pmcsclientAfterPara, int cLines, int fOptimalLines, int* rgdcpOptimalLines, int dvrMinLineHeight)
		{
			return 0;
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00119C7E File Offset: 0x00118C7E
		internal unsafe int GetOptimalLineDcpCache(IntPtr pfsclient, int cLines, int* rgdcp)
		{
			return -10000;
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0011A868 File Offset: 0x00119868
		internal int GetNumberAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, out int cAttachedObjects)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
				cAttachedObjects = textParagraph.GetAttachedObjectCount(dcpFirst, lastDcpAttachedObjectBeforeLine);
				return 0;
			}
			catch (Exception callbackException)
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0011A904 File Offset: 0x00119904
		internal unsafe int GetAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, int nAttachedObjects, IntPtr* rgnmpAttachedObject, int* rgidobj, int* rgdcpAnchor, out int cObjects, out int fEndOfParagraph)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
				List<AttachedObject> attachedObjects = textParagraph.GetAttachedObjects(dcpFirst, lastDcpAttachedObjectBeforeLine);
				for (int i = 0; i < attachedObjects.Count; i++)
				{
					if (attachedObjects[i] is FigureObject)
					{
						FigureObject figureObject = (FigureObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = figureObject.Para.Handle;
						rgdcpAnchor[i] = figureObject.Dcp;
						rgidobj[i] = -2;
					}
					else
					{
						FloaterObject floaterObject = (FloaterObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = floaterObject.Para.Handle;
						rgdcpAnchor[i] = floaterObject.Dcp;
						rgidobj[i] = PtsHost.FloaterParagraphId;
					}
				}
				cObjects = attachedObjects.Count;
				fEndOfParagraph = 0;
			}
			catch (Exception callbackException)
			{
				cObjects = 0;
				fEndOfParagraph = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cObjects = 0;
				fEndOfParagraph = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0011AA58 File Offset: 0x00119A58
		internal int GetNumberAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, out int cAttachedObjects)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				LineBase lineBase = this.PtsContext.HandleToObject(pfsline) as LineBase;
				if (lineBase == null)
				{
					LineBreakpoint lineBreakpoint = this.PtsContext.HandleToObject(pfsline) as LineBreakpoint;
					PTS.ValidateHandle(lineBreakpoint);
					lineBase = lineBreakpoint.OptimalBreakSession.OptimalTextSource;
				}
				if (lineBase.HasFigures || lineBase.HasFloaters)
				{
					int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
					cAttachedObjects = textParagraph.GetAttachedObjectCount(lastDcpAttachedObjectBeforeLine, dcpLim);
				}
				else
				{
					cAttachedObjects = 0;
				}
				return 0;
			}
			catch (Exception callbackException)
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0011AB44 File Offset: 0x00119B44
		internal unsafe int GetAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, int nAttachedObjects, IntPtr* rgnmpAttachedObject, int* rgidobj, int* rgdcpAnchor, out int cObjects)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
				List<AttachedObject> attachedObjects = textParagraph.GetAttachedObjects(lastDcpAttachedObjectBeforeLine, dcpLim);
				for (int i = 0; i < attachedObjects.Count; i++)
				{
					if (attachedObjects[i] is FigureObject)
					{
						FigureObject figureObject = (FigureObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = figureObject.Para.Handle;
						rgdcpAnchor[i] = figureObject.Dcp;
						rgidobj[i] = -2;
					}
					else
					{
						FloaterObject floaterObject = (FloaterObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = floaterObject.Para.Handle;
						rgdcpAnchor[i] = floaterObject.Dcp;
						rgidobj[i] = PtsHost.FloaterParagraphId;
					}
				}
				cObjects = attachedObjects.Count;
			}
			catch (Exception callbackException)
			{
				cObjects = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cObjects = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0011AC8C File Offset: 0x00119C8C
		internal int UpdGetAttachedObjectChange(IntPtr pfsclient, IntPtr nmp, IntPtr nmpObject, out PTS.FSKCHANGE fskchObject)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmpObject) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				int num;
				baseParagraph.UpdGetParaChange(out fskchObject, out num);
			}
			catch (Exception callbackException)
			{
				fskchObject = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskchObject = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0011AD18 File Offset: 0x00119D18
		internal int GetDurFigureAnchor(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsparaclientFigure, IntPtr pfsline, IntPtr nmpFigure, uint fswdir, IntPtr pfsfmtlinein, out int dur)
		{
			int result = 0;
			try
			{
				Line line = this.PtsContext.HandleToObject(pfsline) as Line;
				if (line != null)
				{
					PTS.ValidateHandle(line);
					FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
					PTS.ValidateHandle(figureParagraph);
					line.GetDurFigureAnchor(figureParagraph, fswdir, out dur);
				}
				else
				{
					Invariant.Assert(false);
					dur = 0;
				}
			}
			catch (Exception callbackException)
			{
				dur = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dur = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0011ADD0 File Offset: 0x00119DD0
		internal int GetFloaterProperties(IntPtr pfsclient, IntPtr nmFloater, uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				floaterBaseParagraph.GetFloaterProperties(fswdirTrack, out fsfloaterprops);
			}
			catch (Exception callbackException)
			{
				fsfloaterprops = default(PTS.FSFLOATERPROPS);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfloaterprops = default(PTS.FSFLOATERPROPS);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0011AE60 File Offset: 0x00119E60
		internal int FormatFloaterContentFinite(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsbrkFloaterContentIn, int fBreakRecordFromPreviousPage, IntPtr nmFloater, IntPtr pftnrej, int fEmptyOk, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecpara, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.FormatFloaterContentFinite(floaterBaseParaClient, pfsbrkFloaterContentIn, fBreakRecordFromPreviousPage, pftnrej, fEmptyOk, fSuppressTopSpace, fswdirTrack, fAtMaxWidth, durAvailable, dvrAvailable, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfsFloatContent, out pbrkrecpara, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
			}
			catch (Exception callbackException)
			{
				fsfmtr = default(PTS.FSFMTR);
				pfsFloatContent = (pbrkrecpara = IntPtr.Zero);
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtr = default(PTS.FSFMTR);
				pfsFloatContent = (pbrkrecpara = IntPtr.Zero);
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0011AF94 File Offset: 0x00119F94
		internal int FormatFloaterContentBottomless(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.FormatFloaterContentBottomless(floaterBaseParaClient, fSuppressTopSpace, fswdirTrack, fAtMaxWidth, durAvailable, dvrAvailable, out fsfmtrbl, out pfsFloatContent, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfsFloatContent = IntPtr.Zero;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfsFloatContent = IntPtr.Zero;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0011B09C File Offset: 0x0011A09C
		internal int UpdateBottomlessFloaterContent(IntPtr pfsFloaterContent, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.UpdateBottomlessFloaterContent(floaterBaseParaClient, fSuppressTopSpace, fswdirTrack, fAtMaxWidth, durAvailable, dvrAvailable, pfsFloaterContent, out fsfmtrbl, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0011B190 File Offset: 0x0011A190
		internal unsafe int GetFloaterPolygons(IntPtr pfsparaclient, IntPtr pfsFloaterContent, IntPtr nmFloater, uint fswdirTrack, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.GetFloaterPolygons(floaterBaseParaClient, fswdirTrack, ncVertices, nfspt, rgcVertices, out ccVertices, rgfspt, out cfspt, out fWrapThrough);
			}
			catch (Exception callbackException)
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0011B258 File Offset: 0x0011A258
		internal int ClearUpdateInfoInFloaterContent(IntPtr pfsFloaterContent)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent) && (this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient) is UIElementParaClient)
			{
				return 0;
			}
			return PTS.FsClearUpdateInfoInSubpage(this.Context, pfsFloaterContent);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0011B290 File Offset: 0x0011A290
		internal int CompareFloaterContents(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew, out PTS.FSCOMPRESULT fscmpr)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContentOld) && this.PtsContext.IsValidHandle(pfsFloaterContentNew))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContentOld) as FloaterBaseParaClient;
				FloaterBaseParaClient floaterBaseParaClient2 = this.PtsContext.HandleToObject(pfsFloaterContentNew) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient && !(floaterBaseParaClient2 is UIElementParaClient))
				{
					fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
					return 0;
				}
				if (floaterBaseParaClient2 is UIElementParaClient && !(floaterBaseParaClient is UIElementParaClient))
				{
					fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
					return 0;
				}
				if (floaterBaseParaClient is UIElementParaClient && floaterBaseParaClient2 is UIElementParaClient)
				{
					if (pfsFloaterContentOld.Equals(pfsFloaterContentNew))
					{
						fscmpr = PTS.FSCOMPRESULT.fscmprNoChange;
						return 0;
					}
					fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
					return 0;
				}
			}
			return PTS.FsCompareSubpages(this.Context, pfsFloaterContentOld, pfsFloaterContentNew, out fscmpr);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0011B33C File Offset: 0x0011A33C
		internal int DestroyFloaterContent(IntPtr pfsFloaterContent)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent) && (this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient) is UIElementParaClient)
			{
				return 0;
			}
			return PTS.FsDestroySubpage(this.Context, pfsFloaterContent);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0011B372 File Offset: 0x0011A372
		internal int DuplicateFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent, out IntPtr pfsbrkFloaterContentDup)
		{
			if (this.PtsContext.IsValidHandle(pfsbrkFloaterContent) && (this.PtsContext.HandleToObject(pfsbrkFloaterContent) as FloaterBaseParaClient) is UIElementParaClient)
			{
				Invariant.Assert(false, "Embedded UIElement should not have break record");
			}
			return PTS.FsDuplicateSubpageBreakRecord(this.Context, pfsbrkFloaterContent, out pfsbrkFloaterContentDup);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0011B3B2 File Offset: 0x0011A3B2
		internal int DestroyFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent)
		{
			if (this.PtsContext.IsValidHandle(pfsbrkFloaterContent) && (this.PtsContext.HandleToObject(pfsbrkFloaterContent) as FloaterBaseParaClient) is UIElementParaClient)
			{
				Invariant.Assert(false, "Embedded UIElement should not have break record");
			}
			return PTS.FsDestroySubpageBreakRecord(this.Context, pfsbrkFloaterContent);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0011B3F4 File Offset: 0x0011A3F4
		internal int GetFloaterContentColumnBalancingInfo(IntPtr pfsFloaterContent, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient)
				{
					if (((BlockUIContainer)floaterBaseParaClient.Paragraph.Element).Child != null)
					{
						nlines = 1;
						UIElement child = ((BlockUIContainer)floaterBaseParaClient.Paragraph.Element).Child;
						dvrSumHeight = TextDpi.ToTextDpi(child.DesiredSize.Height);
						dvrMinHeight = TextDpi.ToTextDpi(child.DesiredSize.Height);
					}
					else
					{
						nlines = 0;
						dvrSumHeight = (dvrMinHeight = 0);
					}
					return 0;
				}
			}
			uint num;
			return PTS.FsGetSubpageColumnBalancingInfo(this.Context, pfsFloaterContent, out num, out nlines, out dvrSumHeight, out dvrMinHeight);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0011B4AE File Offset: 0x0011A4AE
		internal int GetFloaterContentNumberFootnotes(IntPtr pfsFloaterContent, out int cftn)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent) && (this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient) is UIElementParaClient)
			{
				cftn = 0;
				return 0;
			}
			return PTS.FsGetNumberSubpageFootnotes(this.Context, pfsFloaterContent, out cftn);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0011B4E8 File Offset: 0x0011A4E8
		internal int GetFloaterContentFootnoteInfo(IntPtr pfsFloaterContent, uint fswdir, int nftn, int iftnFirst, ref PTS.FSFTNINFO fsftninf, out int iftnLim)
		{
			iftnLim = 0;
			return 0;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0011B4F0 File Offset: 0x0011A4F0
		internal int TransferDisplayInfoInFloaterContent(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContentOld) && this.PtsContext.IsValidHandle(pfsFloaterContentNew))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContentOld) as FloaterBaseParaClient;
				FloaterBaseParaClient floaterBaseParaClient2 = this.PtsContext.HandleToObject(pfsFloaterContentNew) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient || floaterBaseParaClient2 is UIElementParaClient)
				{
					return 0;
				}
			}
			return PTS.FsTransferDisplayInfoSubpage(this.PtsContext.Context, pfsFloaterContentOld, pfsFloaterContentNew);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0011B560 File Offset: 0x0011A560
		internal int GetMCSClientAfterFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr pmcsclientOut)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				floaterBaseParagraph.GetMCSClientAfterFloater(fswdirTrack, marginCollapsingState, out pmcsclientOut);
			}
			catch (Exception callbackException)
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0011B61C File Offset: 0x0011A61C
		internal int GetDvrUsedForFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, int dvrDisplaced, out int dvrUsed)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				floaterBaseParagraph.GetDvrUsedForFloater(fswdirTrack, marginCollapsingState, dvrDisplaced, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0011B6D4 File Offset: 0x0011A6D4
		internal int SubtrackCreateContext(IntPtr pfsclient, IntPtr pfsc, IntPtr pfscbkobj, uint ffi, int idobj, out IntPtr pfssobjc)
		{
			pfssobjc = (IntPtr)(idobj + PtsHost._objectContextOffset);
			return 0;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00105F35 File Offset: 0x00104F35
		internal int SubtrackDestroyContext(IntPtr pfssobjc)
		{
			return 0;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0011B6E8 File Offset: 0x0011A6E8
		internal int SubtrackFormatParaFinite(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr pfsobjbrk, int fBreakRecordFromPreviousPage, IntPtr nmp, int iArea, IntPtr pftnrej, IntPtr pfsgeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, int fBreakInside, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecpara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fBreakInsidePossible)
		{
			int result = 0;
			fBreakInsidePossible = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nmp) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				ContainerParaClient containerParaClient = this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient;
				PTS.ValidateHandle(containerParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				containerParagraph.FormatParaFinite(containerParaClient, pfsobjbrk, fBreakRecordFromPreviousPage, iArea, pftnrej, pfsgeom, fEmptyOk, fSuppressTopSpace, fswdir, ref fsrcToFill, marginCollapsingState, fskclearIn, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfspara, out pbrkrecpara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace);
			}
			catch (Exception callbackException)
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0011B848 File Offset: 0x0011A848
		internal int SubtrackFormatParaBottomless(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nmp) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				ContainerParaClient containerParaClient = this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient;
				PTS.ValidateHandle(containerParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				containerParagraph.FormatParaBottomless(containerParaClient, iArea, pfsgeom, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out pfspara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0011B998 File Offset: 0x0011A998
		internal int SubtrackUpdateBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nmp) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				ContainerParaClient containerParaClient = this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient;
				PTS.ValidateHandle(containerParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				containerParagraph.UpdateBottomlessPara(pfspara, containerParaClient, iArea, pfsgeom, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0011BAE4 File Offset: 0x0011AAE4
		internal int SubtrackSynchronizeBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsgeom, uint fswdir, int dvrShift)
		{
			int result = 0;
			try
			{
				PTS.ValidateHandle(this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient);
				PTS.Validate(PTS.FsSynchronizeBottomlessSubtrack(this.Context, pfspara, pfsgeom, fswdir, dvrShift), this.PtsContext);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0011BB78 File Offset: 0x0011AB78
		internal int SubtrackComparePara(IntPtr pfsparaclientOld, IntPtr pfsparaOld, IntPtr pfsparaclientNew, IntPtr pfsparaNew, uint fswdir, out PTS.FSCOMPRESULT fscmpr, out int dvrShifted)
		{
			return PTS.FsCompareSubtrack(this.Context, pfsparaOld, pfsparaNew, fswdir, out fscmpr, out dvrShifted);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0011BB8E File Offset: 0x0011AB8E
		internal int SubtrackClearUpdateInfoInPara(IntPtr pfspara)
		{
			return PTS.FsClearUpdateInfoInSubtrack(this.Context, pfspara);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0011BB9C File Offset: 0x0011AB9C
		internal int SubtrackDestroyPara(IntPtr pfspara)
		{
			return PTS.FsDestroySubtrack(this.Context, pfspara);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0011BBAA File Offset: 0x0011ABAA
		internal int SubtrackDuplicateBreakRecord(IntPtr pfssobjc, IntPtr pfsbrkrecparaOrig, out IntPtr pfsbrkrecparaDup)
		{
			return PTS.FsDuplicateSubtrackBreakRecord(this.Context, pfsbrkrecparaOrig, out pfsbrkrecparaDup);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0011BBB9 File Offset: 0x0011ABB9
		internal int SubtrackDestroyBreakRecord(IntPtr pfssobjc, IntPtr pfsobjbrk)
		{
			return PTS.FsDestroySubtrackBreakRecord(this.Context, pfsobjbrk);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0011BBC7 File Offset: 0x0011ABC7
		internal int SubtrackGetColumnBalancingInfo(IntPtr pfspara, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight)
		{
			return PTS.FsGetSubtrackColumnBalancingInfo(this.Context, pfspara, fswdir, out nlines, out dvrSumHeight, out dvrMinHeight);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0011BBDB File Offset: 0x0011ABDB
		internal int SubtrackGetNumberFootnotes(IntPtr pfspara, out int nftn)
		{
			return PTS.FsGetNumberSubtrackFootnotes(this.Context, pfspara, out nftn);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00119054 File Offset: 0x00118054
		internal unsafe int SubtrackGetFootnoteInfo(IntPtr pfspara, uint fswdir, int nftn, int iftnFirst, PTS.FSFTNINFO* pfsftninf, out int iftnLim)
		{
			iftnLim = 0;
			return -10000;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0011BBEA File Offset: 0x0011ABEA
		internal int SubtrackShiftVertical(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsshift, uint fswdir, out PTS.FSBBOX pfsbbox)
		{
			pfsbbox = default(PTS.FSBBOX);
			return 0;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0011BBF5 File Offset: 0x0011ABF5
		internal int SubtrackTransferDisplayInfoPara(IntPtr pfsparaOld, IntPtr pfsparaNew)
		{
			return PTS.FsTransferDisplayInfoSubtrack(this.Context, pfsparaOld, pfsparaNew);
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0011BC04 File Offset: 0x0011AC04
		internal int SubpageCreateContext(IntPtr pfsclient, IntPtr pfsc, IntPtr pfscbkobj, uint ffi, int idobj, out IntPtr pfssobjc)
		{
			pfssobjc = (IntPtr)(idobj + PtsHost._objectContextOffset + 1);
			return 0;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00105F35 File Offset: 0x00104F35
		internal int SubpageDestroyContext(IntPtr pfssobjc)
		{
			return 0;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0011BC1C File Offset: 0x0011AC1C
		internal int SubpageFormatParaFinite(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr pfsobjbrk, int fBreakRecordFromPreviousPage, IntPtr nmp, int iArea, IntPtr pftnrej, IntPtr pfsgeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, int fBreakInside, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecpara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fBreakInsidePossible)
		{
			int result = 0;
			fBreakInsidePossible = 0;
			try
			{
				SubpageParagraph subpageParagraph = this.PtsContext.HandleToObject(nmp) as SubpageParagraph;
				PTS.ValidateHandle(subpageParagraph);
				SubpageParaClient subpageParaClient = this.PtsContext.HandleToObject(pfsparaclient) as SubpageParaClient;
				PTS.ValidateHandle(subpageParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				subpageParagraph.FormatParaFinite(subpageParaClient, pfsobjbrk, fBreakRecordFromPreviousPage, pftnrej, fEmptyOk, fSuppressTopSpace, fswdir, ref fsrcToFill, marginCollapsingState, fskclearIn, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfspara, out pbrkrecpara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace);
			}
			catch (Exception callbackException)
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0011BD78 File Offset: 0x0011AD78
		internal int SubpageFormatParaBottomless(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				SubpageParagraph subpageParagraph = this.PtsContext.HandleToObject(nmp) as SubpageParagraph;
				PTS.ValidateHandle(subpageParagraph);
				SubpageParaClient subpageParaClient = this.PtsContext.HandleToObject(pfsparaclient) as SubpageParaClient;
				PTS.ValidateHandle(subpageParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				subpageParagraph.FormatParaBottomless(subpageParaClient, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out pfspara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0011BEC4 File Offset: 0x0011AEC4
		internal int SubpageUpdateBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				SubpageParagraph subpageParagraph = this.PtsContext.HandleToObject(nmp) as SubpageParagraph;
				PTS.ValidateHandle(subpageParagraph);
				SubpageParaClient subpageParaClient = this.PtsContext.HandleToObject(pfsparaclient) as SubpageParaClient;
				PTS.ValidateHandle(subpageParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				subpageParagraph.UpdateBottomlessPara(pfspara, subpageParaClient, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00105F35 File Offset: 0x00104F35
		internal int SubpageSynchronizeBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsgeom, uint fswdir, int dvrShift)
		{
			return 0;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0011C00C File Offset: 0x0011B00C
		internal int SubpageComparePara(IntPtr pfsparaclientOld, IntPtr pfsparaOld, IntPtr pfsparaclientNew, IntPtr pfsparaNew, uint fswdir, out PTS.FSCOMPRESULT fscmpr, out int dvrShifted)
		{
			dvrShifted = 0;
			return PTS.FsCompareSubpages(this.Context, pfsparaOld, pfsparaNew, out fscmpr);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0011C022 File Offset: 0x0011B022
		internal int SubpageClearUpdateInfoInPara(IntPtr pfspara)
		{
			return PTS.FsClearUpdateInfoInSubpage(this.Context, pfspara);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0011C030 File Offset: 0x0011B030
		internal int SubpageDestroyPara(IntPtr pfspara)
		{
			return PTS.FsDestroySubpage(this.Context, pfspara);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0011C03E File Offset: 0x0011B03E
		internal int SubpageDuplicateBreakRecord(IntPtr pfssobjc, IntPtr pfsbrkrecparaOrig, out IntPtr pfsbrkrecparaDup)
		{
			return PTS.FsDuplicateSubpageBreakRecord(this.Context, pfsbrkrecparaOrig, out pfsbrkrecparaDup);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0011C04D File Offset: 0x0011B04D
		internal int SubpageDestroyBreakRecord(IntPtr pfssobjc, IntPtr pfsobjbrk)
		{
			return PTS.FsDestroySubpageBreakRecord(this.Context, pfsobjbrk);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0011C05B File Offset: 0x0011B05B
		internal int SubpageGetColumnBalancingInfo(IntPtr pfspara, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight)
		{
			return PTS.FsGetSubpageColumnBalancingInfo(this.Context, pfspara, out fswdir, out nlines, out dvrSumHeight, out dvrMinHeight);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0011C070 File Offset: 0x0011B070
		internal int SubpageGetNumberFootnotes(IntPtr pfspara, out int nftn)
		{
			return PTS.FsGetNumberSubpageFootnotes(this.Context, pfspara, out nftn);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0011C07F File Offset: 0x0011B07F
		internal unsafe int SubpageGetFootnoteInfo(IntPtr pfspara, uint fswdir, int nftn, int iftnFirst, PTS.FSFTNINFO* pfsftninf, out int iftnLim)
		{
			return PTS.FsGetSubpageFootnoteInfo(this.Context, pfspara, nftn, iftnFirst, out fswdir, pfsftninf, out iftnLim);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0011BBEA File Offset: 0x0011ABEA
		internal int SubpageShiftVertical(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsshift, uint fswdir, out PTS.FSBBOX pfsbbox)
		{
			pfsbbox = default(PTS.FSBBOX);
			return 0;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0011C096 File Offset: 0x0011B096
		internal int SubpageTransferDisplayInfoPara(IntPtr pfsparaOld, IntPtr pfsparaNew)
		{
			return PTS.FsTransferDisplayInfoSubpage(this.Context, pfsparaOld, pfsparaNew);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0011C0A8 File Offset: 0x0011B0A8
		internal int GetTableProperties(IntPtr pfsclient, IntPtr nmTable, uint fswdirTrack, out PTS.FSTABLEOBJPROPS fstableobjprops)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetTableProperties(fswdirTrack, out fstableobjprops);
			}
			catch (Exception callbackException)
			{
				fstableobjprops = default(PTS.FSTABLEOBJPROPS);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fstableobjprops = default(PTS.FSTABLEOBJPROPS);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0011C138 File Offset: 0x0011B138
		internal int AutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth)
		{
			int result = 0;
			try
			{
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				tableParaClient.AutofitTable(fswdirTrack, durAvailableSpace, out durTableWidth);
			}
			catch (Exception callbackException)
			{
				durTableWidth = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				durTableWidth = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0011C1C4 File Offset: 0x0011B1C4
		internal int UpdAutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth, out int fNoChangeInCellWidths)
		{
			int result = 0;
			try
			{
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				tableParaClient.UpdAutofitTable(fswdirTrack, durAvailableSpace, out durTableWidth, out fNoChangeInCellWidths);
			}
			catch (Exception callbackException)
			{
				durTableWidth = 0;
				fNoChangeInCellWidths = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				durTableWidth = 0;
				fNoChangeInCellWidths = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0011C25C File Offset: 0x0011B25C
		internal int GetMCSClientAfterTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr ppmcsclientOut)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetMCSClientAfterTable(fswdirTrack, pmcsclientIn, out ppmcsclientOut);
			}
			catch (Exception callbackException)
			{
				ppmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ppmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0011C2F0 File Offset: 0x0011B2F0
		internal int GetFirstHeaderRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedHeader, out int fFound, out IntPtr pnmFirstHeaderRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetFirstHeaderRow(fRepeatedHeader, out fFound, out pnmFirstHeaderRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmFirstHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmFirstHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0011C38C File Offset: 0x0011B38C
		internal int GetNextHeaderRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmHeaderRow, int fRepeatedHeader, out int fFound, out IntPtr pnmNextHeaderRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetNextHeaderRow(fRepeatedHeader, nmHeaderRow, out fFound, out pnmNextHeaderRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmNextHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmNextHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0011C428 File Offset: 0x0011B428
		internal int GetFirstFooterRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedFooter, out int fFound, out IntPtr pnmFirstFooterRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetFirstFooterRow(fRepeatedFooter, out fFound, out pnmFirstFooterRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmFirstFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmFirstFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0011C4C4 File Offset: 0x0011B4C4
		internal int GetNextFooterRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmFooterRow, int fRepeatedFooter, out int fFound, out IntPtr pnmNextFooterRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetNextFooterRow(fRepeatedFooter, nmFooterRow, out fFound, out pnmNextFooterRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmNextFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmNextFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0011C560 File Offset: 0x0011B560
		internal int GetFirstRow(IntPtr pfsclient, IntPtr nmTable, out int fFound, out IntPtr pnmFirstRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetFirstRow(out fFound, out pnmFirstRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmFirstRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmFirstRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0011C5F8 File Offset: 0x0011B5F8
		internal int GetNextRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out int fFound, out IntPtr pnmNextRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetNextRow(nmRow, out fFound, out pnmNextRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmNextRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmNextRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0011C694 File Offset: 0x0011B694
		internal int UpdFChangeInHeaderFooter(IntPtr pfsclient, IntPtr nmTable, out int fHeaderChanged, out int fFooterChanged, out int fRepeatedHeaderChanged, out int fRepeatedFooterChanged)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.UpdFChangeInHeaderFooter(out fHeaderChanged, out fFooterChanged, out fRepeatedHeaderChanged, out fRepeatedFooterChanged);
			}
			catch (Exception callbackException)
			{
				fHeaderChanged = 0;
				fFooterChanged = 0;
				fRepeatedHeaderChanged = 0;
				fRepeatedFooterChanged = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fHeaderChanged = 0;
				fFooterChanged = 0;
				fRepeatedHeaderChanged = 0;
				fRepeatedFooterChanged = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0011C738 File Offset: 0x0011B738
		internal int UpdGetFirstChangeInTable(IntPtr pfsclient, IntPtr nmTable, out int fFound, out int fChangeFirst, out IntPtr pnmRowBeforeChange)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.UpdGetFirstChangeInTable(out fFound, out fChangeFirst, out pnmRowBeforeChange);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				fChangeFirst = 0;
				pnmRowBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				fChangeFirst = 0;
				pnmRowBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0011C7D8 File Offset: 0x0011B7D8
		internal int UpdGetRowChange(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			int result = 0;
			try
			{
				PTS.ValidateHandle(this.PtsContext.HandleToObject(nmTable) as TableParagraph);
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			}
			catch (Exception callbackException)
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0011C880 File Offset: 0x0011B880
		internal int UpdGetCellChange(IntPtr pfsclient, IntPtr nmRow, IntPtr nmCell, out int fWidthChanged, out PTS.FSKCHANGE fskchCell)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				cellParagraph.UpdGetCellChange(out fWidthChanged, out fskchCell);
			}
			catch (Exception callbackException)
			{
				fWidthChanged = 0;
				fskchCell = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fWidthChanged = 0;
				fskchCell = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0011C914 File Offset: 0x0011B914
		internal int GetDistributionKind(IntPtr pfsclient, IntPtr nmTable, uint fswdirTable, out PTS.FSKTABLEHEIGHTDISTRIBUTION tabledistr)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetDistributionKind(fswdirTable, out tabledistr);
			}
			catch (Exception callbackException)
			{
				tabledistr = PTS.FSKTABLEHEIGHTDISTRIBUTION.fskdistributeUnchanged;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				tabledistr = PTS.FSKTABLEHEIGHTDISTRIBUTION.fskdistributeUnchanged;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0011C99C File Offset: 0x0011B99C
		internal int GetRowProperties(IntPtr pfsclient, IntPtr nmRow, uint fswdirTable, out PTS.FSTABLEROWPROPS rowprops)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.GetRowProperties(fswdirTable, out rowprops);
			}
			catch (Exception callbackException)
			{
				rowprops = default(PTS.FSTABLEROWPROPS);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				rowprops = default(PTS.FSTABLEROWPROPS);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0011CA2C File Offset: 0x0011BA2C
		internal unsafe int GetCells(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, PTS.FSTABLEKCELLMERGE* rgkcellmerge)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.GetCells(cCells, rgnmCell, rgkcellmerge);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0011CAB0 File Offset: 0x0011BAB0
		internal int FInterruptFormattingTable(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmRow, int dvr, out int fInterrupt)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.FInterruptFormattingTable(dvr, out fInterrupt);
			}
			catch (Exception callbackException)
			{
				fInterrupt = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fInterrupt = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0011CB3C File Offset: 0x0011BB3C
		internal unsafe int CalcHorizontalBBoxOfRow(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, IntPtr* rgpfscell, out int urBBox, out int durBBox)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.CalcHorizontalBBoxOfRow(cCells, rgnmCell, rgpfscell, out urBBox, out durBBox);
			}
			catch (Exception callbackException)
			{
				urBBox = 0;
				durBBox = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				urBBox = 0;
				durBBox = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0011CBD4 File Offset: 0x0011BBD4
		internal int FormatCellFinite(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, IntPtr pfsFtnRejector, int fEmptyOK, uint fswdirTable, int dvrExtraHeight, int dvrAvailable, out PTS.FSFMTR pfmtr, out IntPtr ppfscell, out IntPtr pfsbrkcellOut, out int dvrUsed)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.FormatCellFinite(tableParaClient, pfsbrkcell, pfsFtnRejector, fEmptyOK, fswdirTable, dvrAvailable, out pfmtr, out ppfscell, out pfsbrkcellOut, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				pfmtr = default(PTS.FSFMTR);
				ppfscell = IntPtr.Zero;
				pfsbrkcellOut = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfmtr = default(PTS.FSFMTR);
				ppfscell = IntPtr.Zero;
				pfsbrkcellOut = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0011CCB4 File Offset: 0x0011BCB4
		internal int FormatCellBottomless(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out IntPtr ppfscell, out int dvrUsed)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.FormatCellBottomless(tableParaClient, fswdirTable, out fmtrbl, out ppfscell, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				ppfscell = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				ppfscell = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0011CD74 File Offset: 0x0011BD74
		internal int UpdateBottomlessCell(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfscell) as CellParaClient;
				PTS.ValidateHandle(cellParaClient);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.UpdateBottomlessCell(cellParaClient, tableParaClient, fswdirTable, out fmtrbl, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0011CE3C File Offset: 0x0011BE3C
		internal int CompareCells(IntPtr pfscellOld, IntPtr pfscellNew, out PTS.FSCOMPRESULT fscmpr)
		{
			fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
			return 0;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00105F35 File Offset: 0x00104F35
		internal int ClearUpdateInfoInCell(IntPtr pfscell)
		{
			return 0;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0011CE44 File Offset: 0x0011BE44
		internal int SetCellHeight(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, int fBrokenHere, uint fswdirTable, int dvrActual)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfscell) as CellParaClient;
				PTS.ValidateHandle(cellParaClient);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.SetCellHeight(cellParaClient, tableParaClient, pfsbrkcell, fBrokenHere, fswdirTable, dvrActual);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0011C03E File Offset: 0x0011B03E
		internal int DuplicateCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell, out IntPtr ppfsbrkcellDup)
		{
			return PTS.FsDuplicateSubpageBreakRecord(this.Context, pfsbrkcell, out ppfsbrkcellDup);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0011C04D File Offset: 0x0011B04D
		internal int DestroyCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell)
		{
			return PTS.FsDestroySubpageBreakRecord(this.Context, pfsbrkcell);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0011CEFC File Offset: 0x0011BEFC
		internal int DestroyCell(IntPtr pfsCell)
		{
			int result = 0;
			try
			{
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfsCell) as CellParaClient;
				if (cellParaClient != null)
				{
					cellParaClient.Dispose();
				}
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0011CF78 File Offset: 0x0011BF78
		internal int GetCellNumberFootnotes(IntPtr pfsCell, out int cFtn)
		{
			int result = 0;
			try
			{
				PTS.ValidateHandle(this.PtsContext.HandleToObject(pfsCell) as CellParaClient);
				cFtn = 0;
			}
			catch (Exception callbackException)
			{
				cFtn = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cFtn = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0011CFF8 File Offset: 0x0011BFF8
		internal int GetCellMinColumnBalancingStep(IntPtr pfscell, uint fswdir, out int dvrMinStep)
		{
			int result = 0;
			try
			{
				PTS.ValidateHandle(this.PtsContext.HandleToObject(pfscell) as CellParaClient);
				dvrMinStep = TextDpi.ToTextDpi(1.0);
			}
			catch (Exception callbackException)
			{
				dvrMinStep = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvrMinStep = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0011D088 File Offset: 0x0011C088
		internal int TransferDisplayInfoCell(IntPtr pfscellOld, IntPtr pfscellNew)
		{
			int result = 0;
			try
			{
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfscellOld) as CellParaClient;
				PTS.ValidateHandle(cellParaClient);
				CellParaClient cellParaClient2 = this.PtsContext.HandleToObject(pfscellNew) as CellParaClient;
				PTS.ValidateHandle(cellParaClient2);
				cellParaClient2.TransferDisplayInfo(cellParaClient);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x040007D6 RID: 2006
		private PtsContext _ptsContext;

		// Token: 0x040007D7 RID: 2007
		private SecurityCriticalDataForSet<IntPtr> _context;

		// Token: 0x040007D8 RID: 2008
		private static int _objectContextOffset = 10;

		// Token: 0x040007D9 RID: 2009
		private static int _customParaId = 0;
	}
}
