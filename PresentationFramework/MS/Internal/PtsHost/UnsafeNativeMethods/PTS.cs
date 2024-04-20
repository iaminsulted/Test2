using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows;
using MS.Internal.Text;

namespace MS.Internal.PtsHost.UnsafeNativeMethods
{
	// Token: 0x02000152 RID: 338
	internal static class PTS
	{
		// Token: 0x06000AEF RID: 2799 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal static void IgnoreError(int fserr)
		{
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0012BE6C File Offset: 0x0012AE6C
		internal static void Validate(int fserr)
		{
			if (fserr != 0)
			{
				PTS.Error(fserr, null);
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0012BE78 File Offset: 0x0012AE78
		internal static void Validate(int fserr, PtsContext ptsContext)
		{
			if (fserr != 0)
			{
				PTS.Error(fserr, ptsContext);
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0012BE84 File Offset: 0x0012AE84
		private static void Error(int fserr, PtsContext ptsContext)
		{
			if (fserr <= -112)
			{
				if (fserr != -100002)
				{
					if (fserr != -112)
					{
						goto IL_78;
					}
				}
				else
				{
					if (ptsContext != null)
					{
						PTS.SecondaryException ex = new PTS.SecondaryException(ptsContext.CallbackException);
						ptsContext.CallbackException = null;
						throw ex;
					}
					throw new Exception(SR.Get("PTSError", new object[]
					{
						fserr
					}));
				}
			}
			else if (fserr != -100)
			{
				if (fserr == -2)
				{
					throw new OutOfMemoryException();
				}
				goto IL_78;
			}
			throw new PTS.PtsException(SR.Get("FormatRestrictionsExceeded", new object[]
			{
				fserr
			}));
			IL_78:
			throw new PTS.PtsException(SR.Get("PTSError", new object[]
			{
				fserr
			}));
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0012BF27 File Offset: 0x0012AF27
		internal static void ValidateAndTrace(int fserr, PtsContext ptsContext)
		{
			if (fserr != 0)
			{
				PTS.ErrorTrace(fserr, ptsContext);
			}
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0012BF34 File Offset: 0x0012AF34
		private static void ErrorTrace(int fserr, PtsContext ptsContext)
		{
			if (fserr == -2)
			{
				throw new OutOfMemoryException();
			}
			if (ptsContext == null)
			{
				throw new Exception(SR.Get("PTSError", new object[]
				{
					fserr
				}));
			}
			Exception innermostException = PTS.GetInnermostException(ptsContext);
			if (innermostException != null && !(innermostException is PTS.SecondaryException) && !(innermostException is PTS.PtsException))
			{
				PTS.SecondaryException ex = new PTS.SecondaryException(innermostException);
				ptsContext.CallbackException = null;
				throw ex;
			}
			string p = (innermostException == null) ? string.Empty : innermostException.Message;
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.PageFormattingError, ptsContext, p);
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.PageFormattingError, ptsContext, p);
				return;
			}
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0012BFD4 File Offset: 0x0012AFD4
		private static Exception GetInnermostException(PtsContext ptsContext)
		{
			Invariant.Assert(ptsContext != null);
			Exception ex = ptsContext.CallbackException;
			Exception ex2 = ex;
			while (ex2 != null)
			{
				ex2 = ex.InnerException;
				if (ex2 != null)
				{
					ex = ex2;
				}
				if (!(ex is PTS.PtsException) && !(ex is PTS.SecondaryException))
				{
					break;
				}
			}
			return ex;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0012C015 File Offset: 0x0012B015
		internal static void ValidateHandle(object handle)
		{
			if (handle == null)
			{
				PTS.InvalidHandle();
			}
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0012C01F File Offset: 0x0012B01F
		private static void InvalidHandle()
		{
			throw new Exception(SR.Get("PTSInvalidHandle"));
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0012C030 File Offset: 0x0012B030
		internal static int FromBoolean(bool condition)
		{
			if (!condition)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0012C038 File Offset: 0x0012B038
		internal static bool ToBoolean(int flag)
		{
			return flag != 0;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000F93D3 File Offset: 0x000F83D3
		internal static PTS.FSKWRAP WrapDirectionToFskwrap(WrapDirection wrapDirection)
		{
			return (PTS.FSKWRAP)wrapDirection;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0012C040 File Offset: 0x0012B040
		internal static PTS.FSKCLEAR WrapDirectionToFskclear(WrapDirection wrapDirection)
		{
			PTS.FSKCLEAR result;
			switch (wrapDirection)
			{
			case WrapDirection.None:
				result = PTS.FSKCLEAR.fskclearNone;
				break;
			case WrapDirection.Left:
				result = PTS.FSKCLEAR.fskclearLeft;
				break;
			case WrapDirection.Right:
				result = PTS.FSKCLEAR.fskclearRight;
				break;
			case WrapDirection.Both:
				result = PTS.FSKCLEAR.fskclearBoth;
				break;
			default:
				result = PTS.FSKCLEAR.fskclearNone;
				break;
			}
			return result;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0012C078 File Offset: 0x0012B078
		internal static FlowDirection FswdirToFlowDirection(uint fswdir)
		{
			FlowDirection result;
			if (fswdir == 4U)
			{
				result = FlowDirection.RightToLeft;
			}
			else
			{
				result = FlowDirection.LeftToRight;
			}
			return result;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0012C090 File Offset: 0x0012B090
		internal static uint FlowDirectionToFswdir(FlowDirection fd)
		{
			uint result;
			if (fd == FlowDirection.RightToLeft)
			{
				result = 4U;
			}
			else
			{
				result = 0U;
			}
			return result;
		}

		// Token: 0x06000AFE RID: 2814
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int GetFloaterHandlerInfo([In] ref PTS.FSFLOATERINIT pfsfloaterinit, IntPtr pFloaterObjectInfo);

		// Token: 0x06000AFF RID: 2815
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int GetTableObjHandlerInfo([In] ref PTS.FSTABLEOBJINIT pfstableobjinit, IntPtr pTableObjectInfo);

		// Token: 0x06000B00 RID: 2816
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int CreateInstalledObjectsInfo([In] ref PTS.FSIMETHODS fssubtrackparamethods, ref PTS.FSIMETHODS fssubpageparamethods, out IntPtr pInstalledObjects, out int cInstalledObjects);

		// Token: 0x06000B01 RID: 2817
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int DestroyInstalledObjectsInfo(IntPtr pInstalledObjects);

		// Token: 0x06000B02 RID: 2818
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int CreateDocContext([In] ref PTS.FSCONTEXTINFO fscontextinfo, out IntPtr pfscontext);

		// Token: 0x06000B03 RID: 2819
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int DestroyDocContext(IntPtr pfscontext);

		// Token: 0x06000B04 RID: 2820
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsCreatePageFinite(IntPtr pfscontext, IntPtr pfsBRPageStart, IntPtr fsnmSectStart, out PTS.FSFMTR pfsfmtrOut, out IntPtr ppfsPageOut, out IntPtr ppfsBRPageOut);

		// Token: 0x06000B05 RID: 2821
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsUpdateFinitePage(IntPtr pfscontext, IntPtr pfspage, IntPtr pfsBRPageStart, IntPtr fsnmSectStart, out PTS.FSFMTR pfsfmtrOut, out IntPtr ppfsBRPageOut);

		// Token: 0x06000B06 RID: 2822
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsCreatePageBottomless(IntPtr pfscontext, IntPtr fsnmsect, out PTS.FSFMTRBL pfsfmtrbl, out IntPtr ppfspage);

		// Token: 0x06000B07 RID: 2823
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsUpdateBottomlessPage(IntPtr pfscontext, IntPtr pfspage, IntPtr fsnmsect, out PTS.FSFMTRBL pfsfmtrbl);

		// Token: 0x06000B08 RID: 2824
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsClearUpdateInfoInPage(IntPtr pfscontext, IntPtr pfspage);

		// Token: 0x06000B09 RID: 2825
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDestroyPage(IntPtr pfscontext, IntPtr pfspage);

		// Token: 0x06000B0A RID: 2826
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDestroyPageBreakRecord(IntPtr pfscontext, IntPtr pfsbreakrec);

		// Token: 0x06000B0B RID: 2827
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsCreateSubpageFinite(IntPtr pfsContext, IntPtr pBRSubPageStart, int fFromPreviousPage, IntPtr nSeg, IntPtr pFtnRej, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int lWidth, int lHeight, ref PTS.FSRECT rcMargin, int cColumns, PTS.FSCOLUMNINFO* rgColumnInfo, int fApplyColumnBalancing, int cSegmentAreas, IntPtr* rgnSegmentForArea, int* rgSpanForSegmentArea, int cHeightAreas, int* rgHeightForArea, int* rgSpanForHeightArea, int fAllowOverhangBottom, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pSubPage, out IntPtr pBRSubPageOut, out int dvrUsed, out PTS.FSBBOX fsBBox, out IntPtr pfsMcsClient, out int topSpace);

		// Token: 0x06000B0C RID: 2828
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsCreateSubpageBottomless(IntPtr pfsContext, IntPtr nSeg, int fSuppressTopSpace, uint fswdir, int lWidth, int urMargin, int durMargin, int vrMargin, int cColumns, PTS.FSCOLUMNINFO* rgColumnInfo, int cSegmentAreas, IntPtr* rgnSegmentForArea, int* rgSpanForSegmentArea, int cHeightAreas, int* rgHeightForArea, int* rgSpanForHeightArea, int fINterrruptible, out PTS.FSFMTRBL pfsfmtr, out IntPtr ppSubPage, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr pfsMcsClient, out int pTopSpace, out int fPageBecomesUninterruptible);

		// Token: 0x06000B0D RID: 2829
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsUpdateBottomlessSubpage(IntPtr pfsContext, IntPtr pfsSubpage, IntPtr nmSeg, int fSuppressTopSpace, uint fswdir, int lWidth, int urMargin, int durMargin, int vrMargin, int cColumns, PTS.FSCOLUMNINFO* rgColumnInfo, int cSegmentAreas, IntPtr* rgnSegmentForArea, int* rgSpanForSegmentArea, int cHeightAreas, int* rgHeightForArea, int* rgSpanForHeightArea, int fINterrruptible, out PTS.FSFMTRBL pfsfmtr, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr pfsMcsClient, out int pTopSpace, out int fPageBecomesUninterruptible);

		// Token: 0x06000B0E RID: 2830
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsCompareSubpages(IntPtr pfsContext, IntPtr pfsSubpageOld, IntPtr pfsSubpageNew, out PTS.FSCOMPRESULT fsCompResult);

		// Token: 0x06000B0F RID: 2831
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsClearUpdateInfoInSubpage(IntPtr pfscontext, IntPtr pSubpage);

		// Token: 0x06000B10 RID: 2832
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDestroySubpage(IntPtr pfsContext, IntPtr pSubpage);

		// Token: 0x06000B11 RID: 2833
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDuplicateSubpageBreakRecord(IntPtr pfsContext, IntPtr pBreakRecSubPageIn, out IntPtr ppBreakRecSubPageOut);

		// Token: 0x06000B12 RID: 2834
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDestroySubpageBreakRecord(IntPtr pfscontext, IntPtr pfsbreakrec);

		// Token: 0x06000B13 RID: 2835
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsGetSubpageColumnBalancingInfo(IntPtr pfsContext, IntPtr pSubpage, out uint fswdir, out int lLineNumber, out int lLineHeights, out int lMinimumLineHeight);

		// Token: 0x06000B14 RID: 2836
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsGetNumberSubpageFootnotes(IntPtr pfsContext, IntPtr pSubpage, out int cFootnotes);

		// Token: 0x06000B15 RID: 2837
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsGetSubpageFootnoteInfo(IntPtr pfsContext, IntPtr pSubpage, int cArraySize, int indexStart, out uint fswdir, PTS.FSFTNINFO* rgFootnoteInfo, out int indexLim);

		// Token: 0x06000B16 RID: 2838
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsTransferDisplayInfoSubpage(IntPtr pfsContext, IntPtr pSubpageOld, IntPtr pfsSubpageNew);

		// Token: 0x06000B17 RID: 2839
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsFormatSubtrackFinite(IntPtr pfsContext, IntPtr pfsBRSubtackIn, int fFromPreviousPage, IntPtr fsnmSegment, int iArea, IntPtr pfsFtnRej, IntPtr pfsGeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, [In] ref PTS.FSRECT fsRectToFill, IntPtr pfsMcsClientIn, PTS.FSKCLEAR fsKClearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstpara, out PTS.FSFMTR pfsfmtr, out IntPtr ppfsSubtrack, out IntPtr pfsBRSubtrackOut, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr ppfsMcsClientOut, out PTS.FSKCLEAR pfsKClearOut, out int pTopSpace);

		// Token: 0x06000B18 RID: 2840
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsFormatSubtrackBottomless(IntPtr pfsContext, IntPtr fsnmSegment, int iArea, IntPtr pfsGeom, int fSuppressTopSpace, uint fswdir, int ur, int dur, int vr, IntPtr pfsMcsClientIn, PTS.FSKCLEAR fsKClearIn, int fCanBeInterruptedIn, out PTS.FSFMTRBL pfsfmtrbl, out IntPtr ppfsSubtrack, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr ppfsMcsClientOut, out PTS.FSKCLEAR pfsKClearOut, out int pTopSpace, out int pfCanBeInterruptedOut);

		// Token: 0x06000B19 RID: 2841
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsUpdateBottomlessSubtrack(IntPtr pfsContext, IntPtr pfsSubtrack, IntPtr fsnmSegment, int iArea, IntPtr pfsGeom, int fSuppressTopSpace, uint fswdir, int ur, int dur, int vr, IntPtr pfsMcsClientIn, PTS.FSKCLEAR fsKClearIn, int fCanBeInterruptedIn, out PTS.FSFMTRBL pfsfmtrbl, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr ppfsMcsClientOut, out PTS.FSKCLEAR pfsKClearOut, out int pTopSpace, out int pfCanBeInterruptedOut);

		// Token: 0x06000B1A RID: 2842
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsSynchronizeBottomlessSubtrack(IntPtr pfsContext, IntPtr pfsSubtrack, IntPtr pfsGeom, uint fswdir, int vrShift);

		// Token: 0x06000B1B RID: 2843
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsCompareSubtrack(IntPtr pfsContext, IntPtr pfsSubtrackOld, IntPtr pfsSubtrackNew, uint fswdir, out PTS.FSCOMPRESULT fsCompResult, out int dvrShifted);

		// Token: 0x06000B1C RID: 2844
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsClearUpdateInfoInSubtrack(IntPtr pfsContext, IntPtr pfsSubtrack);

		// Token: 0x06000B1D RID: 2845
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDestroySubtrack(IntPtr pfsContext, IntPtr pfsSubtrack);

		// Token: 0x06000B1E RID: 2846
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDuplicateSubtrackBreakRecord(IntPtr pfsContext, IntPtr pfsBRSubtrackIn, out IntPtr ppfsBRSubtrackOut);

		// Token: 0x06000B1F RID: 2847
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsDestroySubtrackBreakRecord(IntPtr pfscontext, IntPtr pfsbreakrec);

		// Token: 0x06000B20 RID: 2848
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsGetSubtrackColumnBalancingInfo(IntPtr pfscontext, IntPtr pfsSubtrack, uint fswdir, out int lLineNumber, out int lLineHeights, out int lMinimumLineHeight);

		// Token: 0x06000B21 RID: 2849
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsGetNumberSubtrackFootnotes(IntPtr pfscontext, IntPtr pfsSubtrack, out int cFootnotes);

		// Token: 0x06000B22 RID: 2850
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsTransferDisplayInfoSubtrack(IntPtr pfscontext, IntPtr pfsSubtrackOld, IntPtr pfsSubtrackNew);

		// Token: 0x06000B23 RID: 2851
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryFloaterDetails(IntPtr pfsContext, IntPtr pfsfloater, out PTS.FSFLOATERDETAILS fsfloaterdetails);

		// Token: 0x06000B24 RID: 2852
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryPageDetails(IntPtr pfsContext, IntPtr pPage, out PTS.FSPAGEDETAILS pPageDetails);

		// Token: 0x06000B25 RID: 2853
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryPageSectionList(IntPtr pfsContext, IntPtr pPage, int cArraySize, PTS.FSSECTIONDESCRIPTION* rgSectionDescription, out int cActualSize);

		// Token: 0x06000B26 RID: 2854
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQuerySectionDetails(IntPtr pfsContext, IntPtr pSection, out PTS.FSSECTIONDETAILS pSectionDetails);

		// Token: 0x06000B27 RID: 2855
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQuerySectionBasicColumnList(IntPtr pfsContext, IntPtr pSection, int cArraySize, PTS.FSTRACKDESCRIPTION* rgColumnDescription, out int cActualSize);

		// Token: 0x06000B28 RID: 2856
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryTrackDetails(IntPtr pfsContext, IntPtr pTrack, out PTS.FSTRACKDETAILS pTrackDetails);

		// Token: 0x06000B29 RID: 2857
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryTrackParaList(IntPtr pfsContext, IntPtr pTrack, int cParas, PTS.FSPARADESCRIPTION* rgParaDesc, out int cParaDesc);

		// Token: 0x06000B2A RID: 2858
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQuerySubpageDetails(IntPtr pfsContext, IntPtr pSubPage, out PTS.FSSUBPAGEDETAILS pSubPageDetails);

		// Token: 0x06000B2B RID: 2859
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQuerySubpageBasicColumnList(IntPtr pfsContext, IntPtr pSubPage, int cArraySize, PTS.FSTRACKDESCRIPTION* rgColumnDescription, out int cActualSize);

		// Token: 0x06000B2C RID: 2860
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQuerySubtrackDetails(IntPtr pfsContext, IntPtr pSubTrack, out PTS.FSSUBTRACKDETAILS pSubTrackDetails);

		// Token: 0x06000B2D RID: 2861
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQuerySubtrackParaList(IntPtr pfsContext, IntPtr pSubTrack, int cParas, PTS.FSPARADESCRIPTION* rgParaDesc, out int cParaDesc);

		// Token: 0x06000B2E RID: 2862
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryTextDetails(IntPtr pfsContext, IntPtr pPara, out PTS.FSTEXTDETAILS pTextDetails);

		// Token: 0x06000B2F RID: 2863
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryLineListSingle(IntPtr pfsContext, IntPtr pPara, int cLines, PTS.FSLINEDESCRIPTIONSINGLE* rgLineDesc, out int cLineDesc);

		// Token: 0x06000B30 RID: 2864
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryLineListComposite(IntPtr pfsContext, IntPtr pPara, int cElements, PTS.FSLINEDESCRIPTIONCOMPOSITE* rgLineDescription, out int cLineElements);

		// Token: 0x06000B31 RID: 2865
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryLineCompositeElementList(IntPtr pfsContext, IntPtr pLine, int cElements, PTS.FSLINEELEMENT* rgLineElement, out int cLineElements);

		// Token: 0x06000B32 RID: 2866
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryAttachedObjectList(IntPtr pfsContext, IntPtr pPara, int cAttachedObject, PTS.FSATTACHEDOBJECTDESCRIPTION* rgAttachedObjects, out int cAttachedObjectDesc);

		// Token: 0x06000B33 RID: 2867
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryFigureObjectDetails(IntPtr pfsContext, IntPtr pPara, out PTS.FSFIGUREDETAILS fsFigureDetails);

		// Token: 0x06000B34 RID: 2868
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryTableObjDetails(IntPtr pfscontext, IntPtr pfstableobj, out PTS.FSTABLEOBJDETAILS pfstableobjdetails);

		// Token: 0x06000B35 RID: 2869
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryTableObjTableProperDetails(IntPtr pfscontext, IntPtr pfstableProper, out PTS.FSTABLEDETAILS pfstabledetailsProper);

		// Token: 0x06000B36 RID: 2870
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryTableObjRowList(IntPtr pfscontext, IntPtr pfstableProper, int cRows, PTS.FSTABLEROWDESCRIPTION* rgtablerowdescr, out int pcRowsActual);

		// Token: 0x06000B37 RID: 2871
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsQueryTableObjRowDetails(IntPtr pfscontext, IntPtr pfstablerow, out PTS.FSTABLEROWDETAILS ptableorowdetails);

		// Token: 0x06000B38 RID: 2872
		[DllImport("PresentationNative_cor3.dll")]
		internal unsafe static extern int FsQueryTableObjCellList(IntPtr pfscontext, IntPtr pfstablerow, int cCells, PTS.FSKUPDATE* rgfskupd, IntPtr* rgpfscell, PTS.FSTABLEKCELLMERGE* rgkcellmerge, out int pcCellsActual);

		// Token: 0x06000B39 RID: 2873
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsTransformRectangle(uint fswdirIn, ref PTS.FSRECT rectPage, ref PTS.FSRECT rectTransform, uint fswdirOut, out PTS.FSRECT rectOut);

		// Token: 0x06000B3A RID: 2874
		[DllImport("PresentationNative_cor3.dll")]
		internal static extern int FsTransformBbox(uint fswdirIn, ref PTS.FSRECT rectPage, ref PTS.FSBBOX bboxTransform, uint fswdirOut, out PTS.FSBBOX bboxOut);

		// Token: 0x04000831 RID: 2097
		internal const int True = 1;

		// Token: 0x04000832 RID: 2098
		internal const int False = 0;

		// Token: 0x04000833 RID: 2099
		internal const int dvBottomUndefined = 2147483647;

		// Token: 0x04000834 RID: 2100
		internal const int MaxFontSize = 160000;

		// Token: 0x04000835 RID: 2101
		internal const int MaxPageSize = 3500000;

		// Token: 0x04000836 RID: 2102
		internal const int fsffiWordFlowTextFinite = 1;

		// Token: 0x04000837 RID: 2103
		internal const int fsffiWordClashFootnotesWithText = 2;

		// Token: 0x04000838 RID: 2104
		internal const int fsffiWordNewSectionAboveFootnotes = 4;

		// Token: 0x04000839 RID: 2105
		internal const int fsffiWordStopAfterFirstCollision = 8;

		// Token: 0x0400083A RID: 2106
		internal const int fsffiUseTextParaCache = 16;

		// Token: 0x0400083B RID: 2107
		internal const int fsffiKeepClientLines = 32;

		// Token: 0x0400083C RID: 2108
		internal const int fsffiUseTextQuickLoop = 64;

		// Token: 0x0400083D RID: 2109
		internal const int fsffiAvalonDisableOptimalInChains = 256;

		// Token: 0x0400083E RID: 2110
		internal const int fsffiWordAdjustTrackWidthsForFigureInWebView = 512;

		// Token: 0x0400083F RID: 2111
		internal const int fsidobjText = -1;

		// Token: 0x04000840 RID: 2112
		internal const int fsidobjFigure = -2;

		// Token: 0x04000841 RID: 2113
		internal const int fswdirDefault = 0;

		// Token: 0x04000842 RID: 2114
		internal const int fswdirES = 0;

		// Token: 0x04000843 RID: 2115
		internal const int fswdirEN = 1;

		// Token: 0x04000844 RID: 2116
		internal const int fswdirSE = 2;

		// Token: 0x04000845 RID: 2117
		internal const int fswdirSW = 3;

		// Token: 0x04000846 RID: 2118
		internal const int fswdirWS = 4;

		// Token: 0x04000847 RID: 2119
		internal const int fswdirWN = 5;

		// Token: 0x04000848 RID: 2120
		internal const int fswdirNE = 6;

		// Token: 0x04000849 RID: 2121
		internal const int fswdirNW = 7;

		// Token: 0x0400084A RID: 2122
		internal const int fUDirection = 4;

		// Token: 0x0400084B RID: 2123
		internal const int fVDirection = 1;

		// Token: 0x0400084C RID: 2124
		internal const int fUVertical = 2;

		// Token: 0x0400084D RID: 2125
		internal const int fserrNone = 0;

		// Token: 0x0400084E RID: 2126
		internal const int fserrOutOfMemory = -2;

		// Token: 0x0400084F RID: 2127
		internal const int fserrNotImplemented = -10000;

		// Token: 0x04000850 RID: 2128
		internal const int fserrCallbackException = -100002;

		// Token: 0x04000851 RID: 2129
		internal const int tserrNone = 0;

		// Token: 0x04000852 RID: 2130
		internal const int tserrInvalidParameter = -1;

		// Token: 0x04000853 RID: 2131
		internal const int tserrOutOfMemory = -2;

		// Token: 0x04000854 RID: 2132
		internal const int tserrNullOutputParameter = -3;

		// Token: 0x04000855 RID: 2133
		internal const int tserrInvalidLsContext = -4;

		// Token: 0x04000856 RID: 2134
		internal const int tserrInvalidLine = -5;

		// Token: 0x04000857 RID: 2135
		internal const int tserrInvalidDnode = -6;

		// Token: 0x04000858 RID: 2136
		internal const int tserrInvalidDeviceResolution = -7;

		// Token: 0x04000859 RID: 2137
		internal const int tserrInvalidRun = -8;

		// Token: 0x0400085A RID: 2138
		internal const int tserrMismatchLineContext = -9;

		// Token: 0x0400085B RID: 2139
		internal const int tserrContextInUse = -10;

		// Token: 0x0400085C RID: 2140
		internal const int tserrDuplicateSpecialCharacter = -11;

		// Token: 0x0400085D RID: 2141
		internal const int tserrInvalidAutonumRun = -12;

		// Token: 0x0400085E RID: 2142
		internal const int tserrFormattingFunctionDisabled = -13;

		// Token: 0x0400085F RID: 2143
		internal const int tserrUnfinishedDnode = -14;

		// Token: 0x04000860 RID: 2144
		internal const int tserrInvalidDnodeType = -15;

		// Token: 0x04000861 RID: 2145
		internal const int tserrInvalidPenDnode = -16;

		// Token: 0x04000862 RID: 2146
		internal const int tserrInvalidNonPenDnode = -17;

		// Token: 0x04000863 RID: 2147
		internal const int tserrInvalidBaselinePenDnode = -18;

		// Token: 0x04000864 RID: 2148
		internal const int tserrInvalidFormatterResult = -19;

		// Token: 0x04000865 RID: 2149
		internal const int tserrInvalidObjectIdFetched = -20;

		// Token: 0x04000866 RID: 2150
		internal const int tserrInvalidDcpFetched = -21;

		// Token: 0x04000867 RID: 2151
		internal const int tserrInvalidCpContentFetched = -22;

		// Token: 0x04000868 RID: 2152
		internal const int tserrInvalidBookmarkType = -23;

		// Token: 0x04000869 RID: 2153
		internal const int tserrSetDocDisabled = -24;

		// Token: 0x0400086A RID: 2154
		internal const int tserrFiniFunctionDisabled = -25;

		// Token: 0x0400086B RID: 2155
		internal const int tserrCurrentDnodeIsNotTab = -26;

		// Token: 0x0400086C RID: 2156
		internal const int tserrPendingTabIsNotResolved = -27;

		// Token: 0x0400086D RID: 2157
		internal const int tserrWrongFiniFunction = -28;

		// Token: 0x0400086E RID: 2158
		internal const int tserrInvalidBreakingClass = -29;

		// Token: 0x0400086F RID: 2159
		internal const int tserrBreakingTableNotSet = -30;

		// Token: 0x04000870 RID: 2160
		internal const int tserrInvalidModWidthClass = -31;

		// Token: 0x04000871 RID: 2161
		internal const int tserrModWidthPairsNotSet = -32;

		// Token: 0x04000872 RID: 2162
		internal const int tserrWrongTruncationPoint = -33;

		// Token: 0x04000873 RID: 2163
		internal const int tserrWrongBreak = -34;

		// Token: 0x04000874 RID: 2164
		internal const int tserrDupInvalid = -35;

		// Token: 0x04000875 RID: 2165
		internal const int tserrRubyInvalidVersion = -36;

		// Token: 0x04000876 RID: 2166
		internal const int tserrTatenakayokoInvalidVersion = -37;

		// Token: 0x04000877 RID: 2167
		internal const int tserrWarichuInvalidVersion = -38;

		// Token: 0x04000878 RID: 2168
		internal const int tserrWarichuInvalidData = -39;

		// Token: 0x04000879 RID: 2169
		internal const int tserrCreateSublineDisabled = -40;

		// Token: 0x0400087A RID: 2170
		internal const int tserrCurrentSublineDoesNotExist = -41;

		// Token: 0x0400087B RID: 2171
		internal const int tserrCpOutsideSubline = -42;

		// Token: 0x0400087C RID: 2172
		internal const int tserrHihInvalidVersion = -43;

		// Token: 0x0400087D RID: 2173
		internal const int tserrInsufficientQueryDepth = -44;

		// Token: 0x0400087E RID: 2174
		internal const int tserrInvalidBreakRecord = -45;

		// Token: 0x0400087F RID: 2175
		internal const int tserrInvalidPap = -46;

		// Token: 0x04000880 RID: 2176
		internal const int tserrContradictoryQueryInput = -47;

		// Token: 0x04000881 RID: 2177
		internal const int tserrLineIsNotActive = -48;

		// Token: 0x04000882 RID: 2178
		internal const int tserrTooLongParagraph = -49;

		// Token: 0x04000883 RID: 2179
		internal const int tserrTooManyCharsToGlyph = -50;

		// Token: 0x04000884 RID: 2180
		internal const int tserrWrongHyphenationPosition = -51;

		// Token: 0x04000885 RID: 2181
		internal const int tserrTooManyPriorities = -52;

		// Token: 0x04000886 RID: 2182
		internal const int tserrWrongGivenCp = -53;

		// Token: 0x04000887 RID: 2183
		internal const int tserrWrongCpFirstForGetBreaks = -54;

		// Token: 0x04000888 RID: 2184
		internal const int tserrWrongJustTypeForGetBreaks = -55;

		// Token: 0x04000889 RID: 2185
		internal const int tserrWrongJustTypeForCreateLineGivenCp = -56;

		// Token: 0x0400088A RID: 2186
		internal const int tserrTooLongGlyphContext = -57;

		// Token: 0x0400088B RID: 2187
		internal const int tserrInvalidCharToGlyphMapping = -58;

		// Token: 0x0400088C RID: 2188
		internal const int tserrInvalidMathUsage = -59;

		// Token: 0x0400088D RID: 2189
		internal const int tserrInconsistentChp = -60;

		// Token: 0x0400088E RID: 2190
		internal const int tserrStoppedInSubline = -61;

		// Token: 0x0400088F RID: 2191
		internal const int tserrPenPositionCouldNotBeUsed = -62;

		// Token: 0x04000890 RID: 2192
		internal const int tserrDebugFlagsInShip = -63;

		// Token: 0x04000891 RID: 2193
		internal const int tserrInvalidOrderTabs = -64;

		// Token: 0x04000892 RID: 2194
		internal const int tserrSystemRestrictionsExceeded = -100;

		// Token: 0x04000893 RID: 2195
		internal const int tserrInvalidPtsContext = -103;

		// Token: 0x04000894 RID: 2196
		internal const int tserrInvalidClientOutput = -104;

		// Token: 0x04000895 RID: 2197
		internal const int tserrInvalidObjectOutput = -105;

		// Token: 0x04000896 RID: 2198
		internal const int tserrInvalidGeometry = -106;

		// Token: 0x04000897 RID: 2199
		internal const int tserrInvalidFootnoteRejector = -107;

		// Token: 0x04000898 RID: 2200
		internal const int tserrInvalidFootnoteInfo = -108;

		// Token: 0x04000899 RID: 2201
		internal const int tserrOutputArrayTooSmall = -110;

		// Token: 0x0400089A RID: 2202
		internal const int tserrWordNotSupportedInBottomless = -111;

		// Token: 0x0400089B RID: 2203
		internal const int tserrPageTooLong = -112;

		// Token: 0x0400089C RID: 2204
		internal const int tserrInvalidQuery = -113;

		// Token: 0x0400089D RID: 2205
		internal const int tserrWrongWritingDirection = -114;

		// Token: 0x0400089E RID: 2206
		internal const int tserrPageNotClearedForUpdate = -115;

		// Token: 0x0400089F RID: 2207
		internal const int tserrInternalError = -1000;

		// Token: 0x040008A0 RID: 2208
		internal const int tserrNotImplemented = -10000;

		// Token: 0x040008A1 RID: 2209
		internal const int tserrClientAbort = -100000;

		// Token: 0x040008A2 RID: 2210
		internal const int tserrPageSizeMismatch = -100001;

		// Token: 0x040008A3 RID: 2211
		internal const int tserrCallbackException = -100002;

		// Token: 0x040008A4 RID: 2212
		internal const int fsfdbgCheckVariantsConsistency = 1;

		// Token: 0x020008CB RID: 2251
		[Serializable]
		private class SecondaryException : Exception
		{
			// Token: 0x06008177 RID: 33143 RVA: 0x003234F6 File Offset: 0x003224F6
			internal SecondaryException(Exception e) : base(null, e)
			{
			}

			// Token: 0x06008178 RID: 33144 RVA: 0x002DBDDD File Offset: 0x002DADDD
			protected SecondaryException(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}

			// Token: 0x17001DA0 RID: 7584
			// (get) Token: 0x06008179 RID: 33145 RVA: 0x00323500 File Offset: 0x00322500
			// (set) Token: 0x0600817A RID: 33146 RVA: 0x0032350D File Offset: 0x0032250D
			public override string HelpLink
			{
				get
				{
					return base.InnerException.HelpLink;
				}
				set
				{
					base.InnerException.HelpLink = value;
				}
			}

			// Token: 0x17001DA1 RID: 7585
			// (get) Token: 0x0600817B RID: 33147 RVA: 0x0032351B File Offset: 0x0032251B
			public override string Message
			{
				get
				{
					return base.InnerException.Message;
				}
			}

			// Token: 0x17001DA2 RID: 7586
			// (get) Token: 0x0600817C RID: 33148 RVA: 0x00323528 File Offset: 0x00322528
			// (set) Token: 0x0600817D RID: 33149 RVA: 0x00323535 File Offset: 0x00322535
			public override string Source
			{
				get
				{
					return base.InnerException.Source;
				}
				set
				{
					base.InnerException.Source = value;
				}
			}

			// Token: 0x17001DA3 RID: 7587
			// (get) Token: 0x0600817E RID: 33150 RVA: 0x00323543 File Offset: 0x00322543
			public override string StackTrace
			{
				get
				{
					return base.InnerException.StackTrace;
				}
			}
		}

		// Token: 0x020008CC RID: 2252
		private class PtsException : Exception
		{
			// Token: 0x0600817F RID: 33151 RVA: 0x0028DE95 File Offset: 0x0028CE95
			internal PtsException(string s) : base(s)
			{
			}
		}

		// Token: 0x020008CD RID: 2253
		internal struct Restrictions
		{
			// Token: 0x04003C50 RID: 15440
			internal const int tsduRestriction = 1073741823;

			// Token: 0x04003C51 RID: 15441
			internal const int tsdvRestriction = 1073741823;

			// Token: 0x04003C52 RID: 15442
			internal const int tscColumnRestriction = 1000;

			// Token: 0x04003C53 RID: 15443
			internal const int tscSegmentAreaRestriction = 1000;

			// Token: 0x04003C54 RID: 15444
			internal const int tscHeightAreaRestriction = 1000;

			// Token: 0x04003C55 RID: 15445
			internal const int tscTableColumnsRestriction = 1000;

			// Token: 0x04003C56 RID: 15446
			internal const int tscFootnotesRestriction = 1000;

			// Token: 0x04003C57 RID: 15447
			internal const int tscAttachedObjectsRestriction = 100000;

			// Token: 0x04003C58 RID: 15448
			internal const int tscLineInParaRestriction = 1000000;

			// Token: 0x04003C59 RID: 15449
			internal const int tscVerticesRestriction = 10000;

			// Token: 0x04003C5A RID: 15450
			internal const int tscPolygonsRestriction = 200;

			// Token: 0x04003C5B RID: 15451
			internal const int tscSeparatorsRestriction = 1000;

			// Token: 0x04003C5C RID: 15452
			internal const int tscMatrixColumnsRestriction = 1000;

			// Token: 0x04003C5D RID: 15453
			internal const int tscMatrixRowsRestriction = 10000;

			// Token: 0x04003C5E RID: 15454
			internal const int tscEquationsRestriction = 10000;

			// Token: 0x04003C5F RID: 15455
			internal const int tsduFontParameterRestriction = 50000000;

			// Token: 0x04003C60 RID: 15456
			internal const int tsdvFontParameterRestriction = 50000000;

			// Token: 0x04003C61 RID: 15457
			internal const int tscBreakingClassesRestriction = 200;

			// Token: 0x04003C62 RID: 15458
			internal const int tscBreakingUnitsRestriction = 200;

			// Token: 0x04003C63 RID: 15459
			internal const int tscModWidthClassesRestriction = 200;

			// Token: 0x04003C64 RID: 15460
			internal const int tscPairActionsRestriction = 200;

			// Token: 0x04003C65 RID: 15461
			internal const int tscPriorityActionsRestriction = 200;

			// Token: 0x04003C66 RID: 15462
			internal const int tscExpansionUnitsRestriction = 200;

			// Token: 0x04003C67 RID: 15463
			internal const int tscCharacterRestriction = 268435455;

			// Token: 0x04003C68 RID: 15464
			internal const int tscInstalledHandlersRestriction = 200;

			// Token: 0x04003C69 RID: 15465
			internal const int tscJustPriorityLimRestriction = 20;
		}

		// Token: 0x020008CE RID: 2254
		internal struct FSCBK
		{
			// Token: 0x04003C6A RID: 15466
			internal PTS.FSCBKGEN cbkgen;

			// Token: 0x04003C6B RID: 15467
			internal PTS.FSCBKTXT cbktxt;

			// Token: 0x04003C6C RID: 15468
			internal PTS.FSCBKOBJ cbkobj;

			// Token: 0x04003C6D RID: 15469
			internal PTS.FSCBKFIG cbkfig;

			// Token: 0x04003C6E RID: 15470
			internal PTS.FSCBKWRD cbkwrd;
		}

		// Token: 0x020008CF RID: 2255
		internal struct FSCBKFIG
		{
			// Token: 0x04003C6F RID: 15471
			internal PTS.GetFigureProperties pfnGetFigureProperties;

			// Token: 0x04003C70 RID: 15472
			internal PTS.GetFigurePolygons pfnGetFigurePolygons;

			// Token: 0x04003C71 RID: 15473
			internal PTS.CalcFigurePosition pfnCalcFigurePosition;
		}

		// Token: 0x020008D0 RID: 2256
		internal enum FSKREF
		{
			// Token: 0x04003C73 RID: 15475
			fskrefPage,
			// Token: 0x04003C74 RID: 15476
			fskrefMargin,
			// Token: 0x04003C75 RID: 15477
			fskrefParagraph,
			// Token: 0x04003C76 RID: 15478
			fskrefChar,
			// Token: 0x04003C77 RID: 15479
			fskrefOutOfMinMargin,
			// Token: 0x04003C78 RID: 15480
			fskrefOutOfMaxMargin
		}

		// Token: 0x020008D1 RID: 2257
		internal enum FSKALIGNFIG
		{
			// Token: 0x04003C7A RID: 15482
			fskalfMin,
			// Token: 0x04003C7B RID: 15483
			fskalfCenter,
			// Token: 0x04003C7C RID: 15484
			fskalfMax
		}

		// Token: 0x020008D2 RID: 2258
		internal struct FSFIGUREPROPS
		{
			// Token: 0x04003C7D RID: 15485
			internal PTS.FSKREF fskrefU;

			// Token: 0x04003C7E RID: 15486
			internal PTS.FSKREF fskrefV;

			// Token: 0x04003C7F RID: 15487
			internal PTS.FSKALIGNFIG fskalfU;

			// Token: 0x04003C80 RID: 15488
			internal PTS.FSKALIGNFIG fskalfV;

			// Token: 0x04003C81 RID: 15489
			internal PTS.FSKWRAP fskwrap;

			// Token: 0x04003C82 RID: 15490
			internal int fNonTextPlane;

			// Token: 0x04003C83 RID: 15491
			internal int fAllowOverlap;

			// Token: 0x04003C84 RID: 15492
			internal int fDelayable;
		}

		// Token: 0x020008D3 RID: 2259
		internal struct FSCBKGEN
		{
			// Token: 0x04003C85 RID: 15493
			internal PTS.FSkipPage pfnFSkipPage;

			// Token: 0x04003C86 RID: 15494
			internal PTS.GetPageDimensions pfnGetPageDimensions;

			// Token: 0x04003C87 RID: 15495
			internal PTS.GetNextSection pfnGetNextSection;

			// Token: 0x04003C88 RID: 15496
			internal PTS.GetSectionProperties pfnGetSectionProperties;

			// Token: 0x04003C89 RID: 15497
			internal PTS.GetJustificationProperties pfnGetJustificationProperties;

			// Token: 0x04003C8A RID: 15498
			internal PTS.GetMainTextSegment pfnGetMainTextSegment;

			// Token: 0x04003C8B RID: 15499
			internal PTS.GetHeaderSegment pfnGetHeaderSegment;

			// Token: 0x04003C8C RID: 15500
			internal PTS.GetFooterSegment pfnGetFooterSegment;

			// Token: 0x04003C8D RID: 15501
			internal PTS.UpdGetSegmentChange pfnUpdGetSegmentChange;

			// Token: 0x04003C8E RID: 15502
			internal PTS.GetSectionColumnInfo pfnGetSectionColumnInfo;

			// Token: 0x04003C8F RID: 15503
			internal PTS.GetSegmentDefinedColumnSpanAreaInfo pfnGetSegmentDefinedColumnSpanAreaInfo;

			// Token: 0x04003C90 RID: 15504
			internal PTS.GetHeightDefinedColumnSpanAreaInfo pfnGetHeightDefinedColumnSpanAreaInfo;

			// Token: 0x04003C91 RID: 15505
			internal PTS.GetFirstPara pfnGetFirstPara;

			// Token: 0x04003C92 RID: 15506
			internal PTS.GetNextPara pfnGetNextPara;

			// Token: 0x04003C93 RID: 15507
			internal PTS.UpdGetFirstChangeInSegment pfnUpdGetFirstChangeInSegment;

			// Token: 0x04003C94 RID: 15508
			internal PTS.UpdGetParaChange pfnUpdGetParaChange;

			// Token: 0x04003C95 RID: 15509
			internal PTS.GetParaProperties pfnGetParaProperties;

			// Token: 0x04003C96 RID: 15510
			internal PTS.CreateParaclient pfnCreateParaclient;

			// Token: 0x04003C97 RID: 15511
			internal PTS.TransferDisplayInfo pfnTransferDisplayInfo;

			// Token: 0x04003C98 RID: 15512
			internal PTS.DestroyParaclient pfnDestroyParaclient;

			// Token: 0x04003C99 RID: 15513
			internal PTS.FInterruptFormattingAfterPara pfnFInterruptFormattingAfterPara;

			// Token: 0x04003C9A RID: 15514
			internal PTS.GetEndnoteSeparators pfnGetEndnoteSeparators;

			// Token: 0x04003C9B RID: 15515
			internal PTS.GetEndnoteSegment pfnGetEndnoteSegment;

			// Token: 0x04003C9C RID: 15516
			internal PTS.GetNumberEndnoteColumns pfnGetNumberEndnoteColumns;

			// Token: 0x04003C9D RID: 15517
			internal PTS.GetEndnoteColumnInfo pfnGetEndnoteColumnInfo;

			// Token: 0x04003C9E RID: 15518
			internal PTS.GetFootnoteSeparators pfnGetFootnoteSeparators;

			// Token: 0x04003C9F RID: 15519
			internal PTS.FFootnoteBeneathText pfnFFootnoteBeneathText;

			// Token: 0x04003CA0 RID: 15520
			internal PTS.GetNumberFootnoteColumns pfnGetNumberFootnoteColumns;

			// Token: 0x04003CA1 RID: 15521
			internal PTS.GetFootnoteColumnInfo pfnGetFootnoteColumnInfo;

			// Token: 0x04003CA2 RID: 15522
			internal PTS.GetFootnoteSegment pfnGetFootnoteSegment;

			// Token: 0x04003CA3 RID: 15523
			internal PTS.GetFootnotePresentationAndRejectionOrder pfnGetFootnotePresentationAndRejectionOrder;

			// Token: 0x04003CA4 RID: 15524
			internal PTS.FAllowFootnoteSeparation pfnFAllowFootnoteSeparation;
		}

		// Token: 0x020008D4 RID: 2260
		internal struct FSPAP
		{
			// Token: 0x04003CA5 RID: 15525
			internal int idobj;

			// Token: 0x04003CA6 RID: 15526
			internal int fKeepWithNext;

			// Token: 0x04003CA7 RID: 15527
			internal int fBreakPageBefore;

			// Token: 0x04003CA8 RID: 15528
			internal int fBreakColumnBefore;
		}

		// Token: 0x020008D5 RID: 2261
		internal struct FSCBKOBJ
		{
			// Token: 0x04003CA9 RID: 15529
			internal IntPtr pfnNewPtr;

			// Token: 0x04003CAA RID: 15530
			internal IntPtr pfnDisposePtr;

			// Token: 0x04003CAB RID: 15531
			internal IntPtr pfnReallocPtr;

			// Token: 0x04003CAC RID: 15532
			internal PTS.DuplicateMcsclient pfnDuplicateMcsclient;

			// Token: 0x04003CAD RID: 15533
			internal PTS.DestroyMcsclient pfnDestroyMcsclient;

			// Token: 0x04003CAE RID: 15534
			internal PTS.FEqualMcsclient pfnFEqualMcsclient;

			// Token: 0x04003CAF RID: 15535
			internal PTS.ConvertMcsclient pfnConvertMcsclient;

			// Token: 0x04003CB0 RID: 15536
			internal PTS.GetObjectHandlerInfo pfnGetObjectHandlerInfo;
		}

		// Token: 0x020008D6 RID: 2262
		internal struct FSCBKTXT
		{
			// Token: 0x04003CB1 RID: 15537
			internal PTS.CreateParaBreakingSession pfnCreateParaBreakingSession;

			// Token: 0x04003CB2 RID: 15538
			internal PTS.DestroyParaBreakingSession pfnDestroyParaBreakingSession;

			// Token: 0x04003CB3 RID: 15539
			internal PTS.GetTextProperties pfnGetTextProperties;

			// Token: 0x04003CB4 RID: 15540
			internal PTS.GetNumberFootnotes pfnGetNumberFootnotes;

			// Token: 0x04003CB5 RID: 15541
			internal PTS.GetFootnotes pfnGetFootnotes;

			// Token: 0x04003CB6 RID: 15542
			internal PTS.FormatDropCap pfnFormatDropCap;

			// Token: 0x04003CB7 RID: 15543
			internal PTS.GetDropCapPolygons pfnGetDropCapPolygons;

			// Token: 0x04003CB8 RID: 15544
			internal PTS.DestroyDropCap pfnDestroyDropCap;

			// Token: 0x04003CB9 RID: 15545
			internal PTS.FormatBottomText pfnFormatBottomText;

			// Token: 0x04003CBA RID: 15546
			internal PTS.FormatLine pfnFormatLine;

			// Token: 0x04003CBB RID: 15547
			internal PTS.FormatLineForced pfnFormatLineForced;

			// Token: 0x04003CBC RID: 15548
			internal PTS.FormatLineVariants pfnFormatLineVariants;

			// Token: 0x04003CBD RID: 15549
			internal PTS.ReconstructLineVariant pfnReconstructLineVariant;

			// Token: 0x04003CBE RID: 15550
			internal PTS.DestroyLine pfnDestroyLine;

			// Token: 0x04003CBF RID: 15551
			internal PTS.DuplicateLineBreakRecord pfnDuplicateLineBreakRecord;

			// Token: 0x04003CC0 RID: 15552
			internal PTS.DestroyLineBreakRecord pfnDestroyLineBreakRecord;

			// Token: 0x04003CC1 RID: 15553
			internal PTS.SnapGridVertical pfnSnapGridVertical;

			// Token: 0x04003CC2 RID: 15554
			internal PTS.GetDvrSuppressibleBottomSpace pfnGetDvrSuppressibleBottomSpace;

			// Token: 0x04003CC3 RID: 15555
			internal PTS.GetDvrAdvance pfnGetDvrAdvance;

			// Token: 0x04003CC4 RID: 15556
			internal PTS.UpdGetChangeInText pfnUpdGetChangeInText;

			// Token: 0x04003CC5 RID: 15557
			internal PTS.UpdGetDropCapChange pfnUpdGetDropCapChange;

			// Token: 0x04003CC6 RID: 15558
			internal PTS.FInterruptFormattingText pfnFInterruptFormattingText;

			// Token: 0x04003CC7 RID: 15559
			internal PTS.GetTextParaCache pfnGetTextParaCache;

			// Token: 0x04003CC8 RID: 15560
			internal PTS.SetTextParaCache pfnSetTextParaCache;

			// Token: 0x04003CC9 RID: 15561
			internal PTS.GetOptimalLineDcpCache pfnGetOptimalLineDcpCache;

			// Token: 0x04003CCA RID: 15562
			internal PTS.GetNumberAttachedObjectsBeforeTextLine pfnGetNumberAttachedObjectsBeforeTextLine;

			// Token: 0x04003CCB RID: 15563
			internal PTS.GetAttachedObjectsBeforeTextLine pfnGetAttachedObjectsBeforeTextLine;

			// Token: 0x04003CCC RID: 15564
			internal PTS.GetNumberAttachedObjectsInTextLine pfnGetNumberAttachedObjectsInTextLine;

			// Token: 0x04003CCD RID: 15565
			internal PTS.GetAttachedObjectsInTextLine pfnGetAttachedObjectsInTextLine;

			// Token: 0x04003CCE RID: 15566
			internal PTS.UpdGetAttachedObjectChange pfnUpdGetAttachedObjectChange;

			// Token: 0x04003CCF RID: 15567
			internal PTS.GetDurFigureAnchor pfnGetDurFigureAnchor;
		}

		// Token: 0x020008D7 RID: 2263
		internal struct FSLINEVARIANT
		{
			// Token: 0x04003CD0 RID: 15568
			internal IntPtr pfsbreakreclineclient;

			// Token: 0x04003CD1 RID: 15569
			internal IntPtr pfslineclient;

			// Token: 0x04003CD2 RID: 15570
			internal int dcpLine;

			// Token: 0x04003CD3 RID: 15571
			internal int fForceBroken;

			// Token: 0x04003CD4 RID: 15572
			internal PTS.FSFLRES fslres;

			// Token: 0x04003CD5 RID: 15573
			internal int dvrAscent;

			// Token: 0x04003CD6 RID: 15574
			internal int dvrDescent;

			// Token: 0x04003CD7 RID: 15575
			internal int fReformatNeighborsAsLastLine;

			// Token: 0x04003CD8 RID: 15576
			internal IntPtr ptsLinePenaltyInfo;
		}

		// Token: 0x020008D8 RID: 2264
		internal struct FSTXTPROPS
		{
			// Token: 0x04003CD9 RID: 15577
			internal uint fswdir;

			// Token: 0x04003CDA RID: 15578
			internal int dcpStartContent;

			// Token: 0x04003CDB RID: 15579
			internal int fKeepTogether;

			// Token: 0x04003CDC RID: 15580
			internal int fDropCap;

			// Token: 0x04003CDD RID: 15581
			internal int cMinLinesAfterBreak;

			// Token: 0x04003CDE RID: 15582
			internal int cMinLinesBeforeBreak;

			// Token: 0x04003CDF RID: 15583
			internal int fVerticalGrid;

			// Token: 0x04003CE0 RID: 15584
			internal int fOptimizeParagraph;

			// Token: 0x04003CE1 RID: 15585
			internal int fAvoidHyphenationAtTrackBottom;

			// Token: 0x04003CE2 RID: 15586
			internal int fAvoidHyphenationOnLastChainElement;

			// Token: 0x04003CE3 RID: 15587
			internal int cMaxConsecutiveHyphens;
		}

		// Token: 0x020008D9 RID: 2265
		internal enum FSKFMTLINE
		{
			// Token: 0x04003CE5 RID: 15589
			fskfmtlineNormal,
			// Token: 0x04003CE6 RID: 15590
			fskfmtlineOptimal,
			// Token: 0x04003CE7 RID: 15591
			fskfmtlineForced,
			// Token: 0x04003CE8 RID: 15592
			fskfmtlineWord
		}

		// Token: 0x020008DA RID: 2266
		internal struct FSFMTLINEIN
		{
			// Token: 0x04003CE9 RID: 15593
			internal PTS.FSKFMTLINE fskfmtline;

			// Token: 0x04003CEA RID: 15594
			internal IntPtr nmp;

			// Token: 0x04003CEB RID: 15595
			private int iArea;

			// Token: 0x04003CEC RID: 15596
			private int dcpStartLine;

			// Token: 0x04003CED RID: 15597
			private IntPtr pbrLineIn;

			// Token: 0x04003CEE RID: 15598
			private int urStartLine;

			// Token: 0x04003CEF RID: 15599
			private int durLine;

			// Token: 0x04003CF0 RID: 15600
			private int urStartTrack;

			// Token: 0x04003CF1 RID: 15601
			private int durTrack;

			// Token: 0x04003CF2 RID: 15602
			private int urPageLeftMargin;

			// Token: 0x04003CF3 RID: 15603
			private int fAllowHyphenation;

			// Token: 0x04003CF4 RID: 15604
			private int fClearOnleft;

			// Token: 0x04003CF5 RID: 15605
			private int fClearOnRight;

			// Token: 0x04003CF6 RID: 15606
			private int fTreatAsFirstInPara;

			// Token: 0x04003CF7 RID: 15607
			private int fTreatAsLastInPara;

			// Token: 0x04003CF8 RID: 15608
			private int fSuppressTopSpace;

			// Token: 0x04003CF9 RID: 15609
			private int dcpLine;

			// Token: 0x04003CFA RID: 15610
			private int dvrAvailable;

			// Token: 0x04003CFB RID: 15611
			private int fChain;

			// Token: 0x04003CFC RID: 15612
			private int vrStartLine;

			// Token: 0x04003CFD RID: 15613
			private int urStartLr;

			// Token: 0x04003CFE RID: 15614
			private int durLr;

			// Token: 0x04003CFF RID: 15615
			private int fHitByPolygon;

			// Token: 0x04003D00 RID: 15616
			private int fClearLeftLrWord;

			// Token: 0x04003D01 RID: 15617
			private int fClearRightLrWord;
		}

		// Token: 0x020008DB RID: 2267
		internal struct FSCBKWRD
		{
			// Token: 0x04003D02 RID: 15618
			internal IntPtr pfnGetSectionHorizMargins;

			// Token: 0x04003D03 RID: 15619
			internal IntPtr pfnFPerformColumnBalancing;

			// Token: 0x04003D04 RID: 15620
			internal IntPtr pfnCalculateColumnBalancingApproximateHeight;

			// Token: 0x04003D05 RID: 15621
			internal IntPtr pfnCalculateColumnBalancingStep;

			// Token: 0x04003D06 RID: 15622
			internal IntPtr pfnGetColumnSectionBreak;

			// Token: 0x04003D07 RID: 15623
			internal IntPtr pfnFSuppressKeepWithNextAtTopOfPage;

			// Token: 0x04003D08 RID: 15624
			internal IntPtr pfnFSuppressKeepTogetherAtTopOfPage;

			// Token: 0x04003D09 RID: 15625
			internal IntPtr pfnFAllowSpaceAfterOverhang;

			// Token: 0x04003D0A RID: 15626
			internal IntPtr pfnFormatLineWord;

			// Token: 0x04003D0B RID: 15627
			internal IntPtr pfnGetSuppressedTopSpace;

			// Token: 0x04003D0C RID: 15628
			internal IntPtr pfnChangeSplatLineHeight;

			// Token: 0x04003D0D RID: 15629
			internal IntPtr pfnGetDvrAdvanceWord;

			// Token: 0x04003D0E RID: 15630
			internal IntPtr pfnGetMinDvrAdvance;

			// Token: 0x04003D0F RID: 15631
			internal IntPtr pfnGetDurTooNarrowForFigure;

			// Token: 0x04003D10 RID: 15632
			internal IntPtr pfnResolveOverlap;

			// Token: 0x04003D11 RID: 15633
			internal IntPtr pfnGetOffsetForFlowAroundAndBBox;

			// Token: 0x04003D12 RID: 15634
			internal IntPtr pfnGetClientGeometryHandle;

			// Token: 0x04003D13 RID: 15635
			internal IntPtr pfnDuplicateClientGeometryHandle;

			// Token: 0x04003D14 RID: 15636
			internal IntPtr pfnDestroyClientGeometryHandle;

			// Token: 0x04003D15 RID: 15637
			internal IntPtr pfnObstacleAddNotification;

			// Token: 0x04003D16 RID: 15638
			internal IntPtr pfnGetFigureObstaclesForRestart;

			// Token: 0x04003D17 RID: 15639
			internal IntPtr pfnRepositionFigure;

			// Token: 0x04003D18 RID: 15640
			internal IntPtr pfnFStopBeforeLr;

			// Token: 0x04003D19 RID: 15641
			internal IntPtr pfnFStopBeforeLine;

			// Token: 0x04003D1A RID: 15642
			internal IntPtr pfnFIgnoreCollision;

			// Token: 0x04003D1B RID: 15643
			internal IntPtr pfnGetNumberOfLinesForColumnBalancing;

			// Token: 0x04003D1C RID: 15644
			internal IntPtr pfnFCancelPageBreakBefore;

			// Token: 0x04003D1D RID: 15645
			internal IntPtr pfnChangeVrTopLineForFigure;

			// Token: 0x04003D1E RID: 15646
			internal IntPtr pfnFApplyWidowOrphanControlInFootnoteResolution;
		}

		// Token: 0x020008DC RID: 2268
		internal struct FSCOLUMNINFO
		{
			// Token: 0x04003D1F RID: 15647
			internal int durBefore;

			// Token: 0x04003D20 RID: 15648
			internal int durWidth;
		}

		// Token: 0x020008DD RID: 2269
		internal enum FSCOMPRESULT
		{
			// Token: 0x04003D22 RID: 15650
			fscmprNoChange,
			// Token: 0x04003D23 RID: 15651
			fscmprChangeInside,
			// Token: 0x04003D24 RID: 15652
			fscmprShifted
		}

		// Token: 0x020008DE RID: 2270
		internal struct FSCONTEXTINFO
		{
			// Token: 0x04003D25 RID: 15653
			internal uint version;

			// Token: 0x04003D26 RID: 15654
			internal uint fsffi;

			// Token: 0x04003D27 RID: 15655
			internal int drMinColumnBalancingStep;

			// Token: 0x04003D28 RID: 15656
			internal int cInstalledObjects;

			// Token: 0x04003D29 RID: 15657
			internal IntPtr pInstalledObjects;

			// Token: 0x04003D2A RID: 15658
			internal IntPtr pfsclient;

			// Token: 0x04003D2B RID: 15659
			internal IntPtr ptsPenaltyModule;

			// Token: 0x04003D2C RID: 15660
			internal PTS.FSCBK fscbk;

			// Token: 0x04003D2D RID: 15661
			internal PTS.AssertFailed pfnAssertFailed;
		}

		// Token: 0x020008DF RID: 2271
		internal struct FSRECT
		{
			// Token: 0x06008180 RID: 33152 RVA: 0x00323550 File Offset: 0x00322550
			internal FSRECT(int inU, int inV, int inDU, int inDV)
			{
				this.u = inU;
				this.v = inV;
				this.du = inDU;
				this.dv = inDV;
			}

			// Token: 0x06008181 RID: 33153 RVA: 0x0032356F File Offset: 0x0032256F
			internal FSRECT(PTS.FSRECT rect)
			{
				this.u = rect.u;
				this.v = rect.v;
				this.du = rect.du;
				this.dv = rect.dv;
			}

			// Token: 0x06008182 RID: 33154 RVA: 0x003235A4 File Offset: 0x003225A4
			internal FSRECT(Rect rect)
			{
				if (!rect.IsEmpty)
				{
					this.u = TextDpi.ToTextDpi(rect.Left);
					this.v = TextDpi.ToTextDpi(rect.Top);
					this.du = TextDpi.ToTextDpi(rect.Width);
					this.dv = TextDpi.ToTextDpi(rect.Height);
					return;
				}
				this.u = (this.v = (this.du = (this.dv = 0)));
			}

			// Token: 0x06008183 RID: 33155 RVA: 0x00323625 File Offset: 0x00322625
			public static bool operator ==(PTS.FSRECT rect1, PTS.FSRECT rect2)
			{
				return rect1.u == rect2.u && rect1.v == rect2.v && rect1.du == rect2.du && rect1.dv == rect2.dv;
			}

			// Token: 0x06008184 RID: 33156 RVA: 0x00323661 File Offset: 0x00322661
			public static bool operator !=(PTS.FSRECT rect1, PTS.FSRECT rect2)
			{
				return !(rect1 == rect2);
			}

			// Token: 0x06008185 RID: 33157 RVA: 0x0032366D File Offset: 0x0032266D
			public override bool Equals(object o)
			{
				return o is PTS.FSRECT && (PTS.FSRECT)o == this;
			}

			// Token: 0x06008186 RID: 33158 RVA: 0x0032368A File Offset: 0x0032268A
			public override int GetHashCode()
			{
				return this.u.GetHashCode() ^ this.v.GetHashCode() ^ this.du.GetHashCode() ^ this.dv.GetHashCode();
			}

			// Token: 0x06008187 RID: 33159 RVA: 0x0016EE88 File Offset: 0x0016DE88
			internal Rect FromTextDpi()
			{
				return new Rect(TextDpi.FromTextDpi(this.u), TextDpi.FromTextDpi(this.v), TextDpi.FromTextDpi(this.du), TextDpi.FromTextDpi(this.dv));
			}

			// Token: 0x06008188 RID: 33160 RVA: 0x003236BC File Offset: 0x003226BC
			internal bool Contains(PTS.FSPOINT point)
			{
				return point.u >= this.u && point.u <= this.u + this.du && point.v >= this.v && point.v <= this.v + this.dv;
			}

			// Token: 0x04003D2E RID: 15662
			internal int u;

			// Token: 0x04003D2F RID: 15663
			internal int v;

			// Token: 0x04003D30 RID: 15664
			internal int du;

			// Token: 0x04003D31 RID: 15665
			internal int dv;
		}

		// Token: 0x020008E0 RID: 2272
		internal struct FSPOINT
		{
			// Token: 0x06008189 RID: 33161 RVA: 0x00323714 File Offset: 0x00322714
			internal FSPOINT(int inU, int inV)
			{
				this.u = inU;
				this.v = inV;
			}

			// Token: 0x0600818A RID: 33162 RVA: 0x00323724 File Offset: 0x00322724
			public static bool operator ==(PTS.FSPOINT point1, PTS.FSPOINT point2)
			{
				return point1.u == point2.u && point1.v == point2.v;
			}

			// Token: 0x0600818B RID: 33163 RVA: 0x00323744 File Offset: 0x00322744
			public static bool operator !=(PTS.FSPOINT point1, PTS.FSPOINT point2)
			{
				return !(point1 == point2);
			}

			// Token: 0x0600818C RID: 33164 RVA: 0x00323750 File Offset: 0x00322750
			public override bool Equals(object o)
			{
				return o is PTS.FSPOINT && (PTS.FSPOINT)o == this;
			}

			// Token: 0x0600818D RID: 33165 RVA: 0x0032376D File Offset: 0x0032276D
			public override int GetHashCode()
			{
				return this.u.GetHashCode() ^ this.v.GetHashCode();
			}

			// Token: 0x04003D32 RID: 15666
			internal int u;

			// Token: 0x04003D33 RID: 15667
			internal int v;
		}

		// Token: 0x020008E1 RID: 2273
		internal struct FSVECTOR
		{
			// Token: 0x0600818E RID: 33166 RVA: 0x00323786 File Offset: 0x00322786
			internal FSVECTOR(int inDU, int inDV)
			{
				this.du = inDU;
				this.dv = inDV;
			}

			// Token: 0x0600818F RID: 33167 RVA: 0x00323796 File Offset: 0x00322796
			public static bool operator ==(PTS.FSVECTOR vector1, PTS.FSVECTOR vector2)
			{
				return vector1.du == vector2.du && vector1.dv == vector2.dv;
			}

			// Token: 0x06008190 RID: 33168 RVA: 0x003237B6 File Offset: 0x003227B6
			public static bool operator !=(PTS.FSVECTOR vector1, PTS.FSVECTOR vector2)
			{
				return !(vector1 == vector2);
			}

			// Token: 0x06008191 RID: 33169 RVA: 0x003237C2 File Offset: 0x003227C2
			public override bool Equals(object o)
			{
				return o is PTS.FSVECTOR && (PTS.FSVECTOR)o == this;
			}

			// Token: 0x06008192 RID: 33170 RVA: 0x003237DF File Offset: 0x003227DF
			public override int GetHashCode()
			{
				return this.du.GetHashCode() ^ this.dv.GetHashCode();
			}

			// Token: 0x06008193 RID: 33171 RVA: 0x003237F8 File Offset: 0x003227F8
			internal Vector FromTextDpi()
			{
				return new Vector(TextDpi.FromTextDpi(this.du), TextDpi.FromTextDpi(this.dv));
			}

			// Token: 0x04003D34 RID: 15668
			internal int du;

			// Token: 0x04003D35 RID: 15669
			internal int dv;
		}

		// Token: 0x020008E2 RID: 2274
		internal struct FSBBOX
		{
			// Token: 0x04003D36 RID: 15670
			internal int fDefined;

			// Token: 0x04003D37 RID: 15671
			internal PTS.FSRECT fsrc;
		}

		// Token: 0x020008E3 RID: 2275
		internal struct FSFIGOBSTINFO
		{
			// Token: 0x04003D38 RID: 15672
			internal IntPtr nmpFigure;

			// Token: 0x04003D39 RID: 15673
			internal PTS.FSFLOWAROUND flow;

			// Token: 0x04003D3A RID: 15674
			internal PTS.FSPOLYGONINFO polyginfo;

			// Token: 0x04003D3B RID: 15675
			internal PTS.FSOVERLAP overlap;

			// Token: 0x04003D3C RID: 15676
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003D3D RID: 15677
			internal PTS.FSPOINT fsptPosPreliminary;

			// Token: 0x04003D3E RID: 15678
			internal int fNonTextPlane;

			// Token: 0x04003D3F RID: 15679
			internal int fUTextRelative;

			// Token: 0x04003D40 RID: 15680
			internal int fVTextRelative;
		}

		// Token: 0x020008E4 RID: 2276
		internal struct FSFIGOBSTRESTARTINFO
		{
			// Token: 0x04003D41 RID: 15681
			internal IntPtr nmpFigure;

			// Token: 0x04003D42 RID: 15682
			internal int fReached;

			// Token: 0x04003D43 RID: 15683
			internal int fNonTextPlane;
		}

		// Token: 0x020008E5 RID: 2277
		internal struct FSFLOATERCBK
		{
			// Token: 0x04003D44 RID: 15684
			internal PTS.GetFloaterProperties pfnGetFloaterProperties;

			// Token: 0x04003D45 RID: 15685
			internal PTS.FormatFloaterContentFinite pfnFormatFloaterContentFinite;

			// Token: 0x04003D46 RID: 15686
			internal PTS.FormatFloaterContentBottomless pfnFormatFloaterContentBottomless;

			// Token: 0x04003D47 RID: 15687
			internal PTS.UpdateBottomlessFloaterContent pfnUpdateBottomlessFloaterContent;

			// Token: 0x04003D48 RID: 15688
			internal PTS.GetFloaterPolygons pfnGetFloaterPolygons;

			// Token: 0x04003D49 RID: 15689
			internal PTS.ClearUpdateInfoInFloaterContent pfnClearUpdateInfoInFloaterContent;

			// Token: 0x04003D4A RID: 15690
			internal PTS.CompareFloaterContents pfnCompareFloaterContents;

			// Token: 0x04003D4B RID: 15691
			internal PTS.DestroyFloaterContent pfnDestroyFloaterContent;

			// Token: 0x04003D4C RID: 15692
			internal PTS.DuplicateFloaterContentBreakRecord pfnDuplicateFloaterContentBreakRecord;

			// Token: 0x04003D4D RID: 15693
			internal PTS.DestroyFloaterContentBreakRecord pfnDestroyFloaterContentBreakRecord;

			// Token: 0x04003D4E RID: 15694
			internal PTS.GetFloaterContentColumnBalancingInfo pfnGetFloaterContentColumnBalancingInfo;

			// Token: 0x04003D4F RID: 15695
			internal PTS.GetFloaterContentNumberFootnotes pfnGetFloaterContentNumberFootnotes;

			// Token: 0x04003D50 RID: 15696
			internal PTS.GetFloaterContentFootnoteInfo pfnGetFloaterContentFootnoteInfo;

			// Token: 0x04003D51 RID: 15697
			internal PTS.TransferDisplayInfoInFloaterContent pfnTransferDisplayInfoInFloaterContent;

			// Token: 0x04003D52 RID: 15698
			internal PTS.GetMCSClientAfterFloater pfnGetMCSClientAfterFloater;

			// Token: 0x04003D53 RID: 15699
			internal PTS.GetDvrUsedForFloater pfnGetDvrUsedForFloater;
		}

		// Token: 0x020008E6 RID: 2278
		internal enum FSKFLOATALIGNMENT
		{
			// Token: 0x04003D55 RID: 15701
			fskfloatalignMin,
			// Token: 0x04003D56 RID: 15702
			fskfloatalignCenter,
			// Token: 0x04003D57 RID: 15703
			fskfloatalignMax
		}

		// Token: 0x020008E7 RID: 2279
		internal struct FSFLOATERPROPS
		{
			// Token: 0x04003D58 RID: 15704
			internal PTS.FSKCLEAR fskclear;

			// Token: 0x04003D59 RID: 15705
			internal PTS.FSKFLOATALIGNMENT fskfloatalignment;

			// Token: 0x04003D5A RID: 15706
			internal int fFloat;

			// Token: 0x04003D5B RID: 15707
			internal PTS.FSKWRAP fskwr;

			// Token: 0x04003D5C RID: 15708
			internal int fDelayNoProgress;

			// Token: 0x04003D5D RID: 15709
			internal int durDistTextLeft;

			// Token: 0x04003D5E RID: 15710
			internal int durDistTextRight;

			// Token: 0x04003D5F RID: 15711
			internal int dvrDistTextTop;

			// Token: 0x04003D60 RID: 15712
			internal int dvrDistTextBottom;
		}

		// Token: 0x020008E8 RID: 2280
		internal struct FSFLOATERINIT
		{
			// Token: 0x04003D61 RID: 15713
			internal PTS.FSFLOATERCBK fsfloatercbk;
		}

		// Token: 0x020008E9 RID: 2281
		internal struct FSFLOATERDETAILS
		{
			// Token: 0x04003D62 RID: 15714
			internal PTS.FSKUPDATE fskupdContent;

			// Token: 0x04003D63 RID: 15715
			internal IntPtr fsnmFloater;

			// Token: 0x04003D64 RID: 15716
			internal PTS.FSRECT fsrcFloater;

			// Token: 0x04003D65 RID: 15717
			internal IntPtr pfsFloaterContent;
		}

		// Token: 0x020008EA RID: 2282
		internal enum FSFLRES
		{
			// Token: 0x04003D67 RID: 15719
			fsflrOutOfSpace,
			// Token: 0x04003D68 RID: 15720
			fsflrOutOfSpaceHyphenated,
			// Token: 0x04003D69 RID: 15721
			fsflrEndOfParagraph,
			// Token: 0x04003D6A RID: 15722
			fsflrEndOfParagraphClearLeft,
			// Token: 0x04003D6B RID: 15723
			fsflrEndOfParagraphClearRight,
			// Token: 0x04003D6C RID: 15724
			fsflrEndOfParagraphClearBoth,
			// Token: 0x04003D6D RID: 15725
			fsflrPageBreak,
			// Token: 0x04003D6E RID: 15726
			fsflrColumnBreak,
			// Token: 0x04003D6F RID: 15727
			fsflrSoftBreak,
			// Token: 0x04003D70 RID: 15728
			fsflrSoftBreakClearLeft,
			// Token: 0x04003D71 RID: 15729
			fsflrSoftBreakClearRight,
			// Token: 0x04003D72 RID: 15730
			fsflrSoftBreakClearBoth,
			// Token: 0x04003D73 RID: 15731
			fsflrNoProgressClear
		}

		// Token: 0x020008EB RID: 2283
		internal struct FSFLTOBSTINFO
		{
			// Token: 0x04003D74 RID: 15732
			internal PTS.FSFLOWAROUND flow;

			// Token: 0x04003D75 RID: 15733
			internal PTS.FSPOLYGONINFO polyginfo;

			// Token: 0x04003D76 RID: 15734
			internal int fSuppressAutoClear;
		}

		// Token: 0x020008EC RID: 2284
		internal enum FSFMTRKSTOP
		{
			// Token: 0x04003D78 RID: 15736
			fmtrGoalReached,
			// Token: 0x04003D79 RID: 15737
			fmtrBrokenOutOfSpace,
			// Token: 0x04003D7A RID: 15738
			fmtrBrokenPageBreak,
			// Token: 0x04003D7B RID: 15739
			fmtrBrokenColumnBreak,
			// Token: 0x04003D7C RID: 15740
			fmtrBrokenPageBreakBeforePara,
			// Token: 0x04003D7D RID: 15741
			fmtrBrokenColumnBreakBeforePara,
			// Token: 0x04003D7E RID: 15742
			fmtrBrokenPageBreakBeforeSection,
			// Token: 0x04003D7F RID: 15743
			fmtrBrokenDelayable,
			// Token: 0x04003D80 RID: 15744
			fmtrNoProgressOutOfSpace,
			// Token: 0x04003D81 RID: 15745
			fmtrNoProgressPageBreak,
			// Token: 0x04003D82 RID: 15746
			fmtrNoProgressPageBreakBeforePara,
			// Token: 0x04003D83 RID: 15747
			fmtrNoProgressColumnBreakBeforePara,
			// Token: 0x04003D84 RID: 15748
			fmtrNoProgressPageBreakBeforeSection,
			// Token: 0x04003D85 RID: 15749
			fmtrNoProgressPageSkipped,
			// Token: 0x04003D86 RID: 15750
			fmtrNoProgressDelayable,
			// Token: 0x04003D87 RID: 15751
			fmtrCollision
		}

		// Token: 0x020008ED RID: 2285
		internal struct FSFMTR
		{
			// Token: 0x04003D88 RID: 15752
			internal PTS.FSFMTRKSTOP kstop;

			// Token: 0x04003D89 RID: 15753
			internal int fContainsItemThatStoppedBeforeFootnote;

			// Token: 0x04003D8A RID: 15754
			internal int fForcedProgress;
		}

		// Token: 0x020008EE RID: 2286
		internal enum FSFMTRBL
		{
			// Token: 0x04003D8C RID: 15756
			fmtrblGoalReached,
			// Token: 0x04003D8D RID: 15757
			fmtrblCollision,
			// Token: 0x04003D8E RID: 15758
			fmtrblInterrupted
		}

		// Token: 0x020008EF RID: 2287
		internal struct FSFTNINFO
		{
			// Token: 0x04003D8F RID: 15759
			internal IntPtr nmftn;

			// Token: 0x04003D90 RID: 15760
			internal int vrAccept;

			// Token: 0x04003D91 RID: 15761
			internal int vrReject;
		}

		// Token: 0x020008F0 RID: 2288
		internal struct FSINTERVAL
		{
			// Token: 0x04003D92 RID: 15762
			internal int ur;

			// Token: 0x04003D93 RID: 15763
			internal int dur;
		}

		// Token: 0x020008F1 RID: 2289
		internal struct FSFILLINFO
		{
			// Token: 0x04003D94 RID: 15764
			internal PTS.FSRECT fsrc;

			// Token: 0x04003D95 RID: 15765
			internal int fLastInPara;
		}

		// Token: 0x020008F2 RID: 2290
		internal struct FSEMPTYSPACE
		{
			// Token: 0x04003D96 RID: 15766
			internal int ur;

			// Token: 0x04003D97 RID: 15767
			internal int dur;

			// Token: 0x04003D98 RID: 15768
			internal int fPolygonInside;
		}

		// Token: 0x020008F3 RID: 2291
		internal enum FSHYPHENQUALITY
		{
			// Token: 0x04003D9A RID: 15770
			fshqExcellent,
			// Token: 0x04003D9B RID: 15771
			fshqGood,
			// Token: 0x04003D9C RID: 15772
			fshqFair,
			// Token: 0x04003D9D RID: 15773
			fshqPoor,
			// Token: 0x04003D9E RID: 15774
			fshqBad
		}

		// Token: 0x020008F4 RID: 2292
		internal struct FSIMETHODS
		{
			// Token: 0x04003D9F RID: 15775
			internal PTS.ObjCreateContext pfnCreateContext;

			// Token: 0x04003DA0 RID: 15776
			internal PTS.ObjDestroyContext pfnDestroyContext;

			// Token: 0x04003DA1 RID: 15777
			internal PTS.ObjFormatParaFinite pfnFormatParaFinite;

			// Token: 0x04003DA2 RID: 15778
			internal PTS.ObjFormatParaBottomless pfnFormatParaBottomless;

			// Token: 0x04003DA3 RID: 15779
			internal PTS.ObjUpdateBottomlessPara pfnUpdateBottomlessPara;

			// Token: 0x04003DA4 RID: 15780
			internal PTS.ObjSynchronizeBottomlessPara pfnSynchronizeBottomlessPara;

			// Token: 0x04003DA5 RID: 15781
			internal PTS.ObjComparePara pfnComparePara;

			// Token: 0x04003DA6 RID: 15782
			internal PTS.ObjClearUpdateInfoInPara pfnClearUpdateInfoInPara;

			// Token: 0x04003DA7 RID: 15783
			internal PTS.ObjDestroyPara pfnDestroyPara;

			// Token: 0x04003DA8 RID: 15784
			internal PTS.ObjDuplicateBreakRecord pfnDuplicateBreakRecord;

			// Token: 0x04003DA9 RID: 15785
			internal PTS.ObjDestroyBreakRecord pfnDestroyBreakRecord;

			// Token: 0x04003DAA RID: 15786
			internal PTS.ObjGetColumnBalancingInfo pfnGetColumnBalancingInfo;

			// Token: 0x04003DAB RID: 15787
			internal PTS.ObjGetNumberFootnotes pfnGetNumberFootnotes;

			// Token: 0x04003DAC RID: 15788
			internal PTS.ObjGetFootnoteInfo pfnGetFootnoteInfo;

			// Token: 0x04003DAD RID: 15789
			internal IntPtr pfnGetFootnoteInfoWord;

			// Token: 0x04003DAE RID: 15790
			internal PTS.ObjShiftVertical pfnShiftVertical;

			// Token: 0x04003DAF RID: 15791
			internal PTS.ObjTransferDisplayInfoPara pfnTransferDisplayInfoPara;
		}

		// Token: 0x020008F5 RID: 2293
		internal enum FSKALIGNPAGE
		{
			// Token: 0x04003DB1 RID: 15793
			fskalpgTop,
			// Token: 0x04003DB2 RID: 15794
			fskalpgCenter,
			// Token: 0x04003DB3 RID: 15795
			fskalpgBottom
		}

		// Token: 0x020008F6 RID: 2294
		internal enum FSKCHANGE
		{
			// Token: 0x04003DB5 RID: 15797
			fskchNone,
			// Token: 0x04003DB6 RID: 15798
			fskchNew,
			// Token: 0x04003DB7 RID: 15799
			fskchInside
		}

		// Token: 0x020008F7 RID: 2295
		internal enum FSKCLEAR
		{
			// Token: 0x04003DB9 RID: 15801
			fskclearNone,
			// Token: 0x04003DBA RID: 15802
			fskclearLeft,
			// Token: 0x04003DBB RID: 15803
			fskclearRight,
			// Token: 0x04003DBC RID: 15804
			fskclearBoth
		}

		// Token: 0x020008F8 RID: 2296
		internal enum FSKWRAP
		{
			// Token: 0x04003DBE RID: 15806
			fskwrNone,
			// Token: 0x04003DBF RID: 15807
			fskwrLeft,
			// Token: 0x04003DC0 RID: 15808
			fskwrRight,
			// Token: 0x04003DC1 RID: 15809
			fskwrBoth,
			// Token: 0x04003DC2 RID: 15810
			fskwrLargest
		}

		// Token: 0x020008F9 RID: 2297
		internal enum FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA
		{
			// Token: 0x04003DC4 RID: 15812
			fsksuppresshardbreakbeforefirstparaNone,
			// Token: 0x04003DC5 RID: 15813
			fsksuppresshardbreakbeforefirstparaColumn,
			// Token: 0x04003DC6 RID: 15814
			fsksuppresshardbreakbeforefirstparaPageAndColumn
		}

		// Token: 0x020008FA RID: 2298
		internal struct FSFLOWAROUND
		{
			// Token: 0x04003DC7 RID: 15815
			internal PTS.FSRECT fsrcBounding;

			// Token: 0x04003DC8 RID: 15816
			internal PTS.FSKWRAP fskwr;

			// Token: 0x04003DC9 RID: 15817
			internal int durTooNarrow;

			// Token: 0x04003DCA RID: 15818
			internal int durDistTextLeft;

			// Token: 0x04003DCB RID: 15819
			internal int durDistTextRight;

			// Token: 0x04003DCC RID: 15820
			internal int dvrDistTextTop;

			// Token: 0x04003DCD RID: 15821
			internal int dvrDistTextBottom;
		}

		// Token: 0x020008FB RID: 2299
		internal struct FSPOLYGONINFO
		{
			// Token: 0x04003DCE RID: 15822
			internal int cPolygons;

			// Token: 0x04003DCF RID: 15823
			internal unsafe int* rgcVertices;

			// Token: 0x04003DD0 RID: 15824
			internal int cfspt;

			// Token: 0x04003DD1 RID: 15825
			internal unsafe PTS.FSPOINT* rgfspt;

			// Token: 0x04003DD2 RID: 15826
			internal int fWrapThrough;
		}

		// Token: 0x020008FC RID: 2300
		internal struct FSOVERLAP
		{
			// Token: 0x04003DD3 RID: 15827
			internal PTS.FSRECT fsrc;

			// Token: 0x04003DD4 RID: 15828
			internal int fAllowOverlap;
		}

		// Token: 0x020008FD RID: 2301
		internal struct FSFIGUREDETAILS
		{
			// Token: 0x04003DD5 RID: 15829
			internal PTS.FSRECT fsrcFlowAround;

			// Token: 0x04003DD6 RID: 15830
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003DD7 RID: 15831
			internal PTS.FSPOINT fsptPosPreliminary;

			// Token: 0x04003DD8 RID: 15832
			internal int fDelayed;
		}

		// Token: 0x020008FE RID: 2302
		internal struct FSLINEELEMENT
		{
			// Token: 0x04003DD9 RID: 15833
			internal IntPtr pfslineclient;

			// Token: 0x04003DDA RID: 15834
			internal int dcpFirst;

			// Token: 0x04003DDB RID: 15835
			internal IntPtr pfsbreakreclineclient;

			// Token: 0x04003DDC RID: 15836
			internal int dcpLim;

			// Token: 0x04003DDD RID: 15837
			internal int urStart;

			// Token: 0x04003DDE RID: 15838
			internal int dur;

			// Token: 0x04003DDF RID: 15839
			internal int fAllowHyphenation;

			// Token: 0x04003DE0 RID: 15840
			internal int urBBox;

			// Token: 0x04003DE1 RID: 15841
			internal int durBBox;

			// Token: 0x04003DE2 RID: 15842
			internal int urLrWord;

			// Token: 0x04003DE3 RID: 15843
			internal int durLrWord;

			// Token: 0x04003DE4 RID: 15844
			internal int dvrAscent;

			// Token: 0x04003DE5 RID: 15845
			internal int dvrDescent;

			// Token: 0x04003DE6 RID: 15846
			internal int fClearOnLeft;

			// Token: 0x04003DE7 RID: 15847
			internal int fClearOnRight;

			// Token: 0x04003DE8 RID: 15848
			internal int fHitByPolygon;

			// Token: 0x04003DE9 RID: 15849
			internal int fForceBroken;

			// Token: 0x04003DEA RID: 15850
			internal int fClearLeftLrWord;

			// Token: 0x04003DEB RID: 15851
			internal int fClearRightLrWord;
		}

		// Token: 0x020008FF RID: 2303
		internal struct FSLINEDESCRIPTIONCOMPOSITE
		{
			// Token: 0x04003DEC RID: 15852
			internal IntPtr pline;

			// Token: 0x04003DED RID: 15853
			internal int cElements;

			// Token: 0x04003DEE RID: 15854
			internal int vrStart;

			// Token: 0x04003DEF RID: 15855
			internal int dvrAscent;

			// Token: 0x04003DF0 RID: 15856
			internal int dvrDescent;

			// Token: 0x04003DF1 RID: 15857
			internal int fTreatedAsFirst;

			// Token: 0x04003DF2 RID: 15858
			internal int fTreatedAsLast;

			// Token: 0x04003DF3 RID: 15859
			internal int dvrAvailableForcedLine;

			// Token: 0x04003DF4 RID: 15860
			internal int fUsedWordFormatLineInChain;

			// Token: 0x04003DF5 RID: 15861
			internal int fFirstLineInWordLr;
		}

		// Token: 0x02000900 RID: 2304
		internal struct FSLINEDESCRIPTIONSINGLE
		{
			// Token: 0x04003DF6 RID: 15862
			internal IntPtr pfslineclient;

			// Token: 0x04003DF7 RID: 15863
			internal IntPtr pfsbreakreclineclient;

			// Token: 0x04003DF8 RID: 15864
			internal int dcpFirst;

			// Token: 0x04003DF9 RID: 15865
			internal int dcpLim;

			// Token: 0x04003DFA RID: 15866
			internal int urStart;

			// Token: 0x04003DFB RID: 15867
			internal int dur;

			// Token: 0x04003DFC RID: 15868
			internal int fAllowHyphenation;

			// Token: 0x04003DFD RID: 15869
			internal int urBBox;

			// Token: 0x04003DFE RID: 15870
			internal int durBBox;

			// Token: 0x04003DFF RID: 15871
			internal int vrStart;

			// Token: 0x04003E00 RID: 15872
			internal int dvrAscent;

			// Token: 0x04003E01 RID: 15873
			internal int dvrDescent;

			// Token: 0x04003E02 RID: 15874
			internal int fClearOnLeft;

			// Token: 0x04003E03 RID: 15875
			internal int fClearOnRight;

			// Token: 0x04003E04 RID: 15876
			internal int fTreatedAsFirst;

			// Token: 0x04003E05 RID: 15877
			internal int fForceBroken;
		}

		// Token: 0x02000901 RID: 2305
		internal struct FSATTACHEDOBJECTDESCRIPTION
		{
			// Token: 0x04003E06 RID: 15878
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E07 RID: 15879
			internal IntPtr pfspara;

			// Token: 0x04003E08 RID: 15880
			internal IntPtr pfsparaclient;

			// Token: 0x04003E09 RID: 15881
			internal IntPtr nmp;

			// Token: 0x04003E0A RID: 15882
			internal int idobj;

			// Token: 0x04003E0B RID: 15883
			internal int vrStart;

			// Token: 0x04003E0C RID: 15884
			internal int dvrUsed;

			// Token: 0x04003E0D RID: 15885
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E0E RID: 15886
			internal int dvrTopSpace;
		}

		// Token: 0x02000902 RID: 2306
		internal struct FSDROPCAPDETAILS
		{
			// Token: 0x04003E0F RID: 15887
			internal PTS.FSRECT fsrcDropCap;

			// Token: 0x04003E10 RID: 15888
			internal int fSuppressDropCapTopSpacing;

			// Token: 0x04003E11 RID: 15889
			internal IntPtr pdcclient;
		}

		// Token: 0x02000903 RID: 2307
		internal enum FSKTEXTLINES
		{
			// Token: 0x04003E13 RID: 15891
			fsklinesNormal,
			// Token: 0x04003E14 RID: 15892
			fsklinesOptimal,
			// Token: 0x04003E15 RID: 15893
			fsklinesForced,
			// Token: 0x04003E16 RID: 15894
			fsklinesWord
		}

		// Token: 0x02000904 RID: 2308
		internal struct FSTEXTDETAILSFULL
		{
			// Token: 0x04003E17 RID: 15895
			internal uint fswdir;

			// Token: 0x04003E18 RID: 15896
			internal PTS.FSKTEXTLINES fsklines;

			// Token: 0x04003E19 RID: 15897
			internal int fLinesComposite;

			// Token: 0x04003E1A RID: 15898
			internal int cLines;

			// Token: 0x04003E1B RID: 15899
			internal int cAttachedObjects;

			// Token: 0x04003E1C RID: 15900
			internal int dcpFirst;

			// Token: 0x04003E1D RID: 15901
			internal int dcpLim;

			// Token: 0x04003E1E RID: 15902
			internal int fDropCapPresent;

			// Token: 0x04003E1F RID: 15903
			internal PTS.FSUPDATEINFO fsupdinfDropCap;

			// Token: 0x04003E20 RID: 15904
			internal PTS.FSDROPCAPDETAILS dcdetails;

			// Token: 0x04003E21 RID: 15905
			internal int fSuppressTopLineSpacing;

			// Token: 0x04003E22 RID: 15906
			internal int fUpdateInfoForLinesPresent;

			// Token: 0x04003E23 RID: 15907
			internal int cLinesBeforeChange;

			// Token: 0x04003E24 RID: 15908
			internal int dvrShiftBeforeChange;

			// Token: 0x04003E25 RID: 15909
			internal int cLinesChanged;

			// Token: 0x04003E26 RID: 15910
			internal int dcLinesChanged;

			// Token: 0x04003E27 RID: 15911
			internal int dvrShiftAfterChange;

			// Token: 0x04003E28 RID: 15912
			internal int ddcpAfterChange;
		}

		// Token: 0x02000905 RID: 2309
		internal struct FSTEXTDETAILSCACHED
		{
			// Token: 0x04003E29 RID: 15913
			internal uint fswdir;

			// Token: 0x04003E2A RID: 15914
			internal PTS.FSKTEXTLINES fsklines;

			// Token: 0x04003E2B RID: 15915
			internal PTS.FSRECT fsrcPara;

			// Token: 0x04003E2C RID: 15916
			internal int fSuppressTopLineSpacing;

			// Token: 0x04003E2D RID: 15917
			internal int dcpFirst;

			// Token: 0x04003E2E RID: 15918
			internal int dcpLim;

			// Token: 0x04003E2F RID: 15919
			internal int cLines;

			// Token: 0x04003E30 RID: 15920
			internal int fClearOnLeft;

			// Token: 0x04003E31 RID: 15921
			internal int fClearOnRight;

			// Token: 0x04003E32 RID: 15922
			internal int fOptimalLineDcpsCached;
		}

		// Token: 0x02000906 RID: 2310
		internal enum FSKTEXTDETAILS
		{
			// Token: 0x04003E34 RID: 15924
			fsktdCached,
			// Token: 0x04003E35 RID: 15925
			fsktdFull
		}

		// Token: 0x02000907 RID: 2311
		internal struct FSTEXTDETAILS
		{
			// Token: 0x04003E36 RID: 15926
			internal PTS.FSKTEXTDETAILS fsktd;

			// Token: 0x04003E37 RID: 15927
			internal PTS.FSTEXTDETAILS.nested_u u;

			// Token: 0x02000C7A RID: 3194
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004C81 RID: 19585
				[FieldOffset(0)]
				internal PTS.FSTEXTDETAILSFULL full;

				// Token: 0x04004C82 RID: 19586
				[FieldOffset(0)]
				internal PTS.FSTEXTDETAILSCACHED cached;
			}
		}

		// Token: 0x02000908 RID: 2312
		internal struct FSPARADESCRIPTION
		{
			// Token: 0x04003E38 RID: 15928
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E39 RID: 15929
			internal IntPtr pfspara;

			// Token: 0x04003E3A RID: 15930
			internal IntPtr pfsparaclient;

			// Token: 0x04003E3B RID: 15931
			internal IntPtr nmp;

			// Token: 0x04003E3C RID: 15932
			internal int idobj;

			// Token: 0x04003E3D RID: 15933
			internal int dvrUsed;

			// Token: 0x04003E3E RID: 15934
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E3F RID: 15935
			internal int dvrTopSpace;
		}

		// Token: 0x02000909 RID: 2313
		internal struct FSTRACKDETAILS
		{
			// Token: 0x04003E40 RID: 15936
			internal int cParas;
		}

		// Token: 0x0200090A RID: 2314
		internal struct FSTRACKDESCRIPTION
		{
			// Token: 0x04003E41 RID: 15937
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E42 RID: 15938
			internal IntPtr nms;

			// Token: 0x04003E43 RID: 15939
			internal PTS.FSRECT fsrc;

			// Token: 0x04003E44 RID: 15940
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E45 RID: 15941
			internal int fTrackRelativeToRect;

			// Token: 0x04003E46 RID: 15942
			internal IntPtr pfstrack;
		}

		// Token: 0x0200090B RID: 2315
		internal struct FSSUBTRACKDETAILS
		{
			// Token: 0x04003E47 RID: 15943
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E48 RID: 15944
			internal IntPtr nms;

			// Token: 0x04003E49 RID: 15945
			internal PTS.FSRECT fsrc;

			// Token: 0x04003E4A RID: 15946
			internal int cParas;
		}

		// Token: 0x0200090C RID: 2316
		internal struct FSSUBPAGEDETAILSCOMPLEX
		{
			// Token: 0x04003E4B RID: 15947
			internal IntPtr nms;

			// Token: 0x04003E4C RID: 15948
			internal uint fswdir;

			// Token: 0x04003E4D RID: 15949
			internal PTS.FSRECT fsrc;

			// Token: 0x04003E4E RID: 15950
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E4F RID: 15951
			internal int cBasicColumns;

			// Token: 0x04003E50 RID: 15952
			internal int cSegmentDefinedColumnSpanAreas;

			// Token: 0x04003E51 RID: 15953
			internal int cHeightDefinedColumnSpanAreas;
		}

		// Token: 0x0200090D RID: 2317
		internal struct FSSUBPAGEDETAILSSIMPLE
		{
			// Token: 0x04003E52 RID: 15954
			internal uint fswdir;

			// Token: 0x04003E53 RID: 15955
			internal PTS.FSTRACKDESCRIPTION trackdescr;
		}

		// Token: 0x0200090E RID: 2318
		internal struct FSSUBPAGEDETAILS
		{
			// Token: 0x04003E54 RID: 15956
			internal int fSimple;

			// Token: 0x04003E55 RID: 15957
			internal PTS.FSSUBPAGEDETAILS.nested_u u;

			// Token: 0x02000C7B RID: 3195
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004C83 RID: 19587
				[FieldOffset(0)]
				internal PTS.FSSUBPAGEDETAILSSIMPLE simple;

				// Token: 0x04004C84 RID: 19588
				[FieldOffset(0)]
				internal PTS.FSSUBPAGEDETAILSCOMPLEX complex;
			}
		}

		// Token: 0x0200090F RID: 2319
		internal struct FSCOMPOSITECOLUMNDETAILS
		{
			// Token: 0x04003E56 RID: 15958
			internal PTS.FSTRACKDESCRIPTION trackdescrMainText;

			// Token: 0x04003E57 RID: 15959
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteSeparator;

			// Token: 0x04003E58 RID: 15960
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationSeparator;

			// Token: 0x04003E59 RID: 15961
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationNotice;

			// Token: 0x04003E5A RID: 15962
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteSeparator;

			// Token: 0x04003E5B RID: 15963
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationSeparator;

			// Token: 0x04003E5C RID: 15964
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationNotice;

			// Token: 0x04003E5D RID: 15965
			internal int cFootnotes;

			// Token: 0x04003E5E RID: 15966
			internal PTS.FSRECT fsrcFootnotes;

			// Token: 0x04003E5F RID: 15967
			internal PTS.FSBBOX fsbboxFootnotes;

			// Token: 0x04003E60 RID: 15968
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnote;
		}

		// Token: 0x02000910 RID: 2320
		internal struct FSENDNOTECOLUMNDETAILS
		{
			// Token: 0x04003E61 RID: 15969
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationSeparator;

			// Token: 0x04003E62 RID: 15970
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationNotice;

			// Token: 0x04003E63 RID: 15971
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnote;
		}

		// Token: 0x02000911 RID: 2321
		internal struct FSCOMPOSITECOLUMNDESCRIPTION
		{
			// Token: 0x04003E64 RID: 15972
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E65 RID: 15973
			internal PTS.FSRECT fsrc;

			// Token: 0x04003E66 RID: 15974
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E67 RID: 15975
			internal IntPtr pfscompcol;
		}

		// Token: 0x02000912 RID: 2322
		internal struct FSENDNOTECOLUMNDESCRIPTION
		{
			// Token: 0x04003E68 RID: 15976
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E69 RID: 15977
			internal PTS.FSRECT fsrc;

			// Token: 0x04003E6A RID: 15978
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E6B RID: 15979
			internal IntPtr pfsendnotecol;
		}

		// Token: 0x02000913 RID: 2323
		internal struct FSSECTIONDETAILSWITHPAGENOTES
		{
			// Token: 0x04003E6C RID: 15980
			internal uint fswdir;

			// Token: 0x04003E6D RID: 15981
			internal int fColumnBalancingApplied;

			// Token: 0x04003E6E RID: 15982
			internal PTS.FSRECT fsrcSectionBody;

			// Token: 0x04003E6F RID: 15983
			internal PTS.FSBBOX fsbboxSectionBody;

			// Token: 0x04003E70 RID: 15984
			internal int cBasicColumns;

			// Token: 0x04003E71 RID: 15985
			internal int cSegmentDefinedColumnSpanAreas;

			// Token: 0x04003E72 RID: 15986
			internal int cHeightDefinedColumnSpanAreas;

			// Token: 0x04003E73 RID: 15987
			internal PTS.FSRECT fsrcEndnote;

			// Token: 0x04003E74 RID: 15988
			internal PTS.FSBBOX fsbboxEndnote;

			// Token: 0x04003E75 RID: 15989
			internal int cEndnoteColumns;

			// Token: 0x04003E76 RID: 15990
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteSeparator;
		}

		// Token: 0x02000914 RID: 2324
		internal struct FSSECTIONDETAILSWITHCOLNOTES
		{
			// Token: 0x04003E77 RID: 15991
			internal uint fswdir;

			// Token: 0x04003E78 RID: 15992
			internal int fColumnBalancingApplied;

			// Token: 0x04003E79 RID: 15993
			internal int cCompositeColumns;
		}

		// Token: 0x02000915 RID: 2325
		internal struct FSSECTIONDETAILS
		{
			// Token: 0x04003E7A RID: 15994
			internal int fFootnotesAsPagenotes;

			// Token: 0x04003E7B RID: 15995
			internal PTS.FSSECTIONDETAILS.nested_u u;

			// Token: 0x02000C7C RID: 3196
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004C85 RID: 19589
				[FieldOffset(0)]
				internal PTS.FSSECTIONDETAILSWITHPAGENOTES withpagenotes;

				// Token: 0x04004C86 RID: 19590
				[FieldOffset(0)]
				internal PTS.FSSECTIONDETAILSWITHCOLNOTES withcolumnnotes;
			}
		}

		// Token: 0x02000916 RID: 2326
		internal struct FSSECTIONDESCRIPTION
		{
			// Token: 0x04003E7C RID: 15996
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E7D RID: 15997
			internal IntPtr nms;

			// Token: 0x04003E7E RID: 15998
			internal PTS.FSRECT fsrc;

			// Token: 0x04003E7F RID: 15999
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E80 RID: 16000
			internal int fOtherSectionInside;

			// Token: 0x04003E81 RID: 16001
			internal int dvrUsedTop;

			// Token: 0x04003E82 RID: 16002
			internal int dvrUsedBottom;

			// Token: 0x04003E83 RID: 16003
			internal IntPtr pfssection;
		}

		// Token: 0x02000917 RID: 2327
		internal struct FSFOOTNOTECOLUMNDETAILS
		{
			// Token: 0x04003E84 RID: 16004
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationSeparator;

			// Token: 0x04003E85 RID: 16005
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationNotice;

			// Token: 0x04003E86 RID: 16006
			internal int cTracks;
		}

		// Token: 0x02000918 RID: 2328
		internal struct FSFOOTNOTECOLUMNDESCRIPTION
		{
			// Token: 0x04003E87 RID: 16007
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003E88 RID: 16008
			internal PTS.FSRECT fsrc;

			// Token: 0x04003E89 RID: 16009
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04003E8A RID: 16010
			internal IntPtr pfsfootnotecol;
		}

		// Token: 0x02000919 RID: 2329
		internal struct FSPAGEDETAILSCOMPLEX
		{
			// Token: 0x04003E8B RID: 16011
			internal int fTopBottomHeaderFooter;

			// Token: 0x04003E8C RID: 16012
			internal uint fswdirHeader;

			// Token: 0x04003E8D RID: 16013
			internal PTS.FSTRACKDESCRIPTION trackdescrHeader;

			// Token: 0x04003E8E RID: 16014
			internal uint fswdirFooter;

			// Token: 0x04003E8F RID: 16015
			internal PTS.FSTRACKDESCRIPTION trackdescrFooter;

			// Token: 0x04003E90 RID: 16016
			internal int fJustified;

			// Token: 0x04003E91 RID: 16017
			internal PTS.FSKALIGNPAGE fskalpg;

			// Token: 0x04003E92 RID: 16018
			internal uint fswdirPageProper;

			// Token: 0x04003E93 RID: 16019
			internal PTS.FSUPDATEINFO fsupdinfPageBody;

			// Token: 0x04003E94 RID: 16020
			internal PTS.FSRECT fsrcPageBody;

			// Token: 0x04003E95 RID: 16021
			internal PTS.FSRECT fsrcPageMarginActual;

			// Token: 0x04003E96 RID: 16022
			internal PTS.FSBBOX fsbboxPageBody;

			// Token: 0x04003E97 RID: 16023
			internal int cSections;

			// Token: 0x04003E98 RID: 16024
			internal PTS.FSRECT fsrcFootnote;

			// Token: 0x04003E99 RID: 16025
			internal PTS.FSBBOX fsbboxFootnote;

			// Token: 0x04003E9A RID: 16026
			internal int cFootnoteColumns;

			// Token: 0x04003E9B RID: 16027
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteSeparator;
		}

		// Token: 0x0200091A RID: 2330
		internal struct FSPAGEDETAILSSIMPLE
		{
			// Token: 0x04003E9C RID: 16028
			internal PTS.FSTRACKDESCRIPTION trackdescr;
		}

		// Token: 0x0200091B RID: 2331
		internal struct FSPAGEDETAILS
		{
			// Token: 0x04003E9D RID: 16029
			internal PTS.FSKUPDATE fskupd;

			// Token: 0x04003E9E RID: 16030
			internal int fSimple;

			// Token: 0x04003E9F RID: 16031
			internal PTS.FSPAGEDETAILS.nested_u u;

			// Token: 0x02000C7D RID: 3197
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004C87 RID: 19591
				[FieldOffset(0)]
				internal PTS.FSPAGEDETAILSSIMPLE simple;

				// Token: 0x04004C88 RID: 19592
				[FieldOffset(0)]
				internal PTS.FSPAGEDETAILSCOMPLEX complex;
			}
		}

		// Token: 0x0200091C RID: 2332
		internal struct FSTABLECBKFETCH
		{
			// Token: 0x04003EA0 RID: 16032
			internal PTS.GetFirstHeaderRow pfnGetFirstHeaderRow;

			// Token: 0x04003EA1 RID: 16033
			internal PTS.GetNextHeaderRow pfnGetNextHeaderRow;

			// Token: 0x04003EA2 RID: 16034
			internal PTS.GetFirstFooterRow pfnGetFirstFooterRow;

			// Token: 0x04003EA3 RID: 16035
			internal PTS.GetNextFooterRow pfnGetNextFooterRow;

			// Token: 0x04003EA4 RID: 16036
			internal PTS.GetFirstRow pfnGetFirstRow;

			// Token: 0x04003EA5 RID: 16037
			internal PTS.GetNextRow pfnGetNextRow;

			// Token: 0x04003EA6 RID: 16038
			internal PTS.UpdFChangeInHeaderFooter pfnUpdFChangeInHeaderFooter;

			// Token: 0x04003EA7 RID: 16039
			internal PTS.UpdGetFirstChangeInTable pfnUpdGetFirstChangeInTable;

			// Token: 0x04003EA8 RID: 16040
			internal PTS.UpdGetRowChange pfnUpdGetRowChange;

			// Token: 0x04003EA9 RID: 16041
			internal PTS.UpdGetCellChange pfnUpdGetCellChange;

			// Token: 0x04003EAA RID: 16042
			internal PTS.GetDistributionKind pfnGetDistributionKind;

			// Token: 0x04003EAB RID: 16043
			internal PTS.GetRowProperties pfnGetRowProperties;

			// Token: 0x04003EAC RID: 16044
			internal PTS.GetCells pfnGetCells;

			// Token: 0x04003EAD RID: 16045
			internal PTS.FInterruptFormattingTable pfnFInterruptFormattingTable;

			// Token: 0x04003EAE RID: 16046
			internal PTS.CalcHorizontalBBoxOfRow pfnCalcHorizontalBBoxOfRow;
		}

		// Token: 0x0200091D RID: 2333
		internal struct FSTABLECBKCELL
		{
			// Token: 0x04003EAF RID: 16047
			internal PTS.FormatCellFinite pfnFormatCellFinite;

			// Token: 0x04003EB0 RID: 16048
			internal PTS.FormatCellBottomless pfnFormatCellBottomless;

			// Token: 0x04003EB1 RID: 16049
			internal PTS.UpdateBottomlessCell pfnUpdateBottomlessCell;

			// Token: 0x04003EB2 RID: 16050
			internal PTS.CompareCells pfnCompareCells;

			// Token: 0x04003EB3 RID: 16051
			internal PTS.ClearUpdateInfoInCell pfnClearUpdateInfoInCell;

			// Token: 0x04003EB4 RID: 16052
			internal PTS.SetCellHeight pfnSetCellHeight;

			// Token: 0x04003EB5 RID: 16053
			internal PTS.DestroyCell pfnDestroyCell;

			// Token: 0x04003EB6 RID: 16054
			internal PTS.DuplicateCellBreakRecord pfnDuplicateCellBreakRecord;

			// Token: 0x04003EB7 RID: 16055
			internal PTS.DestroyCellBreakRecord pfnDestroyCellBreakRecord;

			// Token: 0x04003EB8 RID: 16056
			internal PTS.GetCellNumberFootnotes pfnGetCellNumberFootnotes;

			// Token: 0x04003EB9 RID: 16057
			internal IntPtr pfnGetCellFootnoteInfo;

			// Token: 0x04003EBA RID: 16058
			internal IntPtr pfnGetCellFootnoteInfoWord;

			// Token: 0x04003EBB RID: 16059
			internal PTS.GetCellMinColumnBalancingStep pfnGetCellMinColumnBalancingStep;

			// Token: 0x04003EBC RID: 16060
			internal PTS.TransferDisplayInfoCell pfnTransferDisplayInfoCell;
		}

		// Token: 0x0200091E RID: 2334
		internal enum FSKTABLEHEIGHTDISTRIBUTION
		{
			// Token: 0x04003EBE RID: 16062
			fskdistributeUnchanged,
			// Token: 0x04003EBF RID: 16063
			fskdistributeEqually,
			// Token: 0x04003EC0 RID: 16064
			fskdistributeProportionally
		}

		// Token: 0x0200091F RID: 2335
		internal enum FSKROWHEIGHTRESTRICTION
		{
			// Token: 0x04003EC2 RID: 16066
			fskrowheightNatural,
			// Token: 0x04003EC3 RID: 16067
			fskrowheightAtLeast,
			// Token: 0x04003EC4 RID: 16068
			fskrowheightAtMostNoBreak,
			// Token: 0x04003EC5 RID: 16069
			fskrowheightExactNoBreak
		}

		// Token: 0x02000920 RID: 2336
		internal enum FSKROWBREAKRESTRICTION
		{
			// Token: 0x04003EC7 RID: 16071
			fskrowbreakAnywhere,
			// Token: 0x04003EC8 RID: 16072
			fskrowbreakNoBreakInside,
			// Token: 0x04003EC9 RID: 16073
			fskrowbreakNoBreakInsideAfter
		}

		// Token: 0x02000921 RID: 2337
		internal struct FSTABLEROWPROPS
		{
			// Token: 0x04003ECA RID: 16074
			internal PTS.FSKROWBREAKRESTRICTION fskrowbreak;

			// Token: 0x04003ECB RID: 16075
			internal PTS.FSKROWHEIGHTRESTRICTION fskrowheight;

			// Token: 0x04003ECC RID: 16076
			internal int dvrRowHeightRestriction;

			// Token: 0x04003ECD RID: 16077
			internal int fBBoxOverflowsBottom;

			// Token: 0x04003ECE RID: 16078
			internal int dvrAboveRow;

			// Token: 0x04003ECF RID: 16079
			internal int dvrBelowRow;

			// Token: 0x04003ED0 RID: 16080
			internal int dvrAboveTopRow;

			// Token: 0x04003ED1 RID: 16081
			internal int dvrBelowBottomRow;

			// Token: 0x04003ED2 RID: 16082
			internal int dvrAboveRowBreak;

			// Token: 0x04003ED3 RID: 16083
			internal int dvrBelowRowBreak;

			// Token: 0x04003ED4 RID: 16084
			internal int cCells;
		}

		// Token: 0x02000922 RID: 2338
		internal enum FSTABLEKCELLMERGE
		{
			// Token: 0x04003ED6 RID: 16086
			fskcellmergeNo,
			// Token: 0x04003ED7 RID: 16087
			fskcellmergeFirst,
			// Token: 0x04003ED8 RID: 16088
			fskcellmergeMiddle,
			// Token: 0x04003ED9 RID: 16089
			fskcellmergeLast
		}

		// Token: 0x02000923 RID: 2339
		internal enum FSKTABLEOBJALIGNMENT
		{
			// Token: 0x04003EDB RID: 16091
			fsktableobjAlignLeft,
			// Token: 0x04003EDC RID: 16092
			fsktableobjAlignRight,
			// Token: 0x04003EDD RID: 16093
			fsktableobjAlignCenter
		}

		// Token: 0x02000924 RID: 2340
		internal struct FSTABLEOBJPROPS
		{
			// Token: 0x04003EDE RID: 16094
			internal PTS.FSKCLEAR fskclear;

			// Token: 0x04003EDF RID: 16095
			internal PTS.FSKTABLEOBJALIGNMENT ktablealignment;

			// Token: 0x04003EE0 RID: 16096
			internal int fFloat;

			// Token: 0x04003EE1 RID: 16097
			internal PTS.FSKWRAP fskwr;

			// Token: 0x04003EE2 RID: 16098
			internal int fDelayNoProgress;

			// Token: 0x04003EE3 RID: 16099
			internal int dvrCaptionTop;

			// Token: 0x04003EE4 RID: 16100
			internal int dvrCaptionBottom;

			// Token: 0x04003EE5 RID: 16101
			internal int durCaptionLeft;

			// Token: 0x04003EE6 RID: 16102
			internal int durCaptionRight;

			// Token: 0x04003EE7 RID: 16103
			internal uint fswdirTable;
		}

		// Token: 0x02000925 RID: 2341
		internal struct FSTABLEOBJCBK
		{
			// Token: 0x04003EE8 RID: 16104
			internal PTS.GetTableProperties pfnGetTableProperties;

			// Token: 0x04003EE9 RID: 16105
			internal PTS.AutofitTable pfnAutofitTable;

			// Token: 0x04003EEA RID: 16106
			internal PTS.UpdAutofitTable pfnUpdAutofitTable;

			// Token: 0x04003EEB RID: 16107
			internal PTS.GetMCSClientAfterTable pfnGetMCSClientAfterTable;

			// Token: 0x04003EEC RID: 16108
			internal IntPtr pfnGetDvrUsedForFloatTable;
		}

		// Token: 0x02000926 RID: 2342
		internal struct FSTABLECBKFETCHWORD
		{
			// Token: 0x04003EED RID: 16109
			internal IntPtr pfnGetTablePropertiesWord;

			// Token: 0x04003EEE RID: 16110
			internal IntPtr pfnGetRowPropertiesWord;

			// Token: 0x04003EEF RID: 16111
			internal IntPtr pfnGetRowWidthWord;

			// Token: 0x04003EF0 RID: 16112
			internal IntPtr pfnGetNumberFiguresForTableRow;

			// Token: 0x04003EF1 RID: 16113
			internal IntPtr pfnGetFiguresForTableRow;

			// Token: 0x04003EF2 RID: 16114
			internal IntPtr pfnFStopBeforeTableRowLr;

			// Token: 0x04003EF3 RID: 16115
			internal IntPtr pfnFIgnoreCollisionForTableRow;

			// Token: 0x04003EF4 RID: 16116
			internal IntPtr pfnChangeRowHeightRestriction;

			// Token: 0x04003EF5 RID: 16117
			internal IntPtr pfnNotifyRowPosition;

			// Token: 0x04003EF6 RID: 16118
			internal IntPtr pfnNotifyRowBorderAbove;

			// Token: 0x04003EF7 RID: 16119
			internal IntPtr pfnNotifyTableBreakRec;
		}

		// Token: 0x02000927 RID: 2343
		internal struct FSTABLEOBJINIT
		{
			// Token: 0x04003EF8 RID: 16120
			internal PTS.FSTABLEOBJCBK tableobjcbk;

			// Token: 0x04003EF9 RID: 16121
			internal PTS.FSTABLECBKFETCH tablecbkfetch;

			// Token: 0x04003EFA RID: 16122
			internal PTS.FSTABLECBKCELL tablecbkcell;

			// Token: 0x04003EFB RID: 16123
			internal PTS.FSTABLECBKFETCHWORD tablecbkfetchword;
		}

		// Token: 0x02000928 RID: 2344
		internal struct FSTABLEOBJDETAILS
		{
			// Token: 0x04003EFC RID: 16124
			internal IntPtr fsnmTable;

			// Token: 0x04003EFD RID: 16125
			internal PTS.FSRECT fsrcTableObj;

			// Token: 0x04003EFE RID: 16126
			internal int dvrTopCaption;

			// Token: 0x04003EFF RID: 16127
			internal int dvrBottomCaption;

			// Token: 0x04003F00 RID: 16128
			internal int durLeftCaption;

			// Token: 0x04003F01 RID: 16129
			internal int durRightCaption;

			// Token: 0x04003F02 RID: 16130
			internal uint fswdirTable;

			// Token: 0x04003F03 RID: 16131
			internal PTS.FSKUPDATE fskupdTableProper;

			// Token: 0x04003F04 RID: 16132
			internal IntPtr pfstableProper;
		}

		// Token: 0x02000929 RID: 2345
		internal struct FSTABLEDETAILS
		{
			// Token: 0x04003F05 RID: 16133
			internal int dvrTable;

			// Token: 0x04003F06 RID: 16134
			internal int cRows;
		}

		// Token: 0x0200092A RID: 2346
		internal struct FSTABLEROWDESCRIPTION
		{
			// Token: 0x04003F07 RID: 16135
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04003F08 RID: 16136
			internal IntPtr fsnmRow;

			// Token: 0x04003F09 RID: 16137
			internal IntPtr pfstablerow;

			// Token: 0x04003F0A RID: 16138
			internal int fRowInSeparateRect;

			// Token: 0x04003F0B RID: 16139
			internal PTS.FSTABLEROWDESCRIPTION.nested_u u;

			// Token: 0x02000C7E RID: 3198
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004C89 RID: 19593
				[FieldOffset(0)]
				internal PTS.FSRECT fsrcRow;

				// Token: 0x04004C8A RID: 19594
				[FieldOffset(0)]
				internal int dvrRow;
			}
		}

		// Token: 0x0200092B RID: 2347
		internal enum FSKTABLEROWBOUNDARY
		{
			// Token: 0x04003F0D RID: 16141
			fsktablerowboundaryOuter,
			// Token: 0x04003F0E RID: 16142
			fsktablerowboundaryInner,
			// Token: 0x04003F0F RID: 16143
			fsktablerowboundaryBreak
		}

		// Token: 0x0200092C RID: 2348
		internal struct FSTABLEROWDETAILS
		{
			// Token: 0x04003F10 RID: 16144
			internal PTS.FSKTABLEROWBOUNDARY fskboundaryAbove;

			// Token: 0x04003F11 RID: 16145
			internal int dvrAbove;

			// Token: 0x04003F12 RID: 16146
			internal PTS.FSKTABLEROWBOUNDARY fskboundaryBelow;

			// Token: 0x04003F13 RID: 16147
			internal int dvrBelow;

			// Token: 0x04003F14 RID: 16148
			internal int cCells;

			// Token: 0x04003F15 RID: 16149
			internal int fForcedRow;
		}

		// Token: 0x0200092D RID: 2349
		internal struct FSTABLESRVCONTEXT
		{
			// Token: 0x04003F16 RID: 16150
			internal IntPtr pfscontext;

			// Token: 0x04003F17 RID: 16151
			internal IntPtr pfsclient;

			// Token: 0x04003F18 RID: 16152
			internal PTS.FSCBKOBJ cbkobj;

			// Token: 0x04003F19 RID: 16153
			internal PTS.FSTABLECBKFETCH tablecbkfetch;

			// Token: 0x04003F1A RID: 16154
			internal PTS.FSTABLECBKCELL tablecbkcell;

			// Token: 0x04003F1B RID: 16155
			internal uint fsffi;
		}

		// Token: 0x0200092E RID: 2350
		internal enum FSKUPDATE
		{
			// Token: 0x04003F1D RID: 16157
			fskupdInherited,
			// Token: 0x04003F1E RID: 16158
			fskupdNoChange,
			// Token: 0x04003F1F RID: 16159
			fskupdNew,
			// Token: 0x04003F20 RID: 16160
			fskupdChangeInside,
			// Token: 0x04003F21 RID: 16161
			fskupdShifted
		}

		// Token: 0x0200092F RID: 2351
		internal struct FSUPDATEINFO
		{
			// Token: 0x04003F22 RID: 16162
			public PTS.FSKUPDATE fskupd;

			// Token: 0x04003F23 RID: 16163
			public int dvrShifted;
		}

		// Token: 0x02000930 RID: 2352
		// (Invoke) Token: 0x06008195 RID: 33173
		internal delegate void AssertFailed(string arg1, string arg2, int arg3, uint arg4);

		// Token: 0x02000931 RID: 2353
		// (Invoke) Token: 0x06008199 RID: 33177
		internal delegate int GetFigureProperties(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, int fInTextLine, uint fswdir, int fBottomUndefined, out int dur, out int dvr, out PTS.FSFIGUREPROPS fsfigprops, out int cPolygons, out int cVertices, out int durDistTextLeft, out int durDistTextRight, out int dvrDistTextTop, out int dvrDistTextBottom);

		// Token: 0x02000932 RID: 2354
		// (Invoke) Token: 0x0600819D RID: 33181
		internal unsafe delegate int GetFigurePolygons(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough);

		// Token: 0x02000933 RID: 2355
		// (Invoke) Token: 0x060081A1 RID: 33185
		internal delegate int CalcFigurePosition(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, ref PTS.FSRECT fsrcPage, ref PTS.FSRECT fsrcMargin, ref PTS.FSRECT fsrcTrack, ref PTS.FSRECT fsrcFigurePreliminary, int fMustPosition, int fInTextLine, out int fPushToNextTrack, out PTS.FSRECT fsrcFlow, out PTS.FSRECT fsrcOverlap, out PTS.FSBBOX fsbbox, out PTS.FSRECT fsrcSearch);

		// Token: 0x02000934 RID: 2356
		// (Invoke) Token: 0x060081A5 RID: 33189
		internal delegate int FSkipPage(IntPtr pfsclient, IntPtr nms, out int fSkip);

		// Token: 0x02000935 RID: 2357
		// (Invoke) Token: 0x060081A9 RID: 33193
		internal delegate int GetPageDimensions(IntPtr pfsclient, IntPtr nms, out uint fswdir, out int fHeaderFooterAtTopBottom, out int durPage, out int dvrPage, ref PTS.FSRECT fsrcMargin);

		// Token: 0x02000936 RID: 2358
		// (Invoke) Token: 0x060081AD RID: 33197
		internal delegate int GetNextSection(IntPtr pfsclient, IntPtr nmsCur, out int fSuccess, out IntPtr nmsNext);

		// Token: 0x02000937 RID: 2359
		// (Invoke) Token: 0x060081B1 RID: 33201
		internal delegate int GetSectionProperties(IntPtr pfsclient, IntPtr nms, out int fNewPage, out uint fswdir, out int fApplyColumnBalancing, out int ccol, out int cSegmentDefinedColumnSpanAreas, out int cHeightDefinedColumnSpanAreas);

		// Token: 0x02000938 RID: 2360
		// (Invoke) Token: 0x060081B5 RID: 33205
		internal unsafe delegate int GetJustificationProperties(IntPtr pfsclient, IntPtr* rgnms, int cnms, int fLastSectionNotBroken, out int fJustify, out PTS.FSKALIGNPAGE fskal, out int fCancelAtLastColumn);

		// Token: 0x02000939 RID: 2361
		// (Invoke) Token: 0x060081B9 RID: 33209
		internal delegate int GetMainTextSegment(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmSegment);

		// Token: 0x0200093A RID: 2362
		// (Invoke) Token: 0x060081BD RID: 33213
		internal delegate int GetHeaderSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fHeaderPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirHeader, out IntPtr nmsHeader);

		// Token: 0x0200093B RID: 2363
		// (Invoke) Token: 0x060081C1 RID: 33217
		internal delegate int GetFooterSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fFooterPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirFooter, out IntPtr nmsFooter);

		// Token: 0x0200093C RID: 2364
		// (Invoke) Token: 0x060081C5 RID: 33221
		internal delegate int UpdGetSegmentChange(IntPtr pfsclient, IntPtr nms, out PTS.FSKCHANGE fskch);

		// Token: 0x0200093D RID: 2365
		// (Invoke) Token: 0x060081C9 RID: 33225
		internal unsafe delegate int GetSectionColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncol, PTS.FSCOLUMNINFO* fscolinfo, out int ccol);

		// Token: 0x0200093E RID: 2366
		// (Invoke) Token: 0x060081CD RID: 33229
		internal unsafe delegate int GetSegmentDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, IntPtr* rgnmSeg, int* rgcColumns, out int cAreasActual);

		// Token: 0x0200093F RID: 2367
		// (Invoke) Token: 0x060081D1 RID: 33233
		internal unsafe delegate int GetHeightDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, int* rgdvrAreaHeight, int* rgcColumns, out int cAreasActual);

		// Token: 0x02000940 RID: 2368
		// (Invoke) Token: 0x060081D5 RID: 33237
		internal delegate int GetFirstPara(IntPtr pfsclient, IntPtr nms, out int fSuccessful, out IntPtr nmp);

		// Token: 0x02000941 RID: 2369
		// (Invoke) Token: 0x060081D9 RID: 33241
		internal delegate int GetNextPara(IntPtr pfsclient, IntPtr nms, IntPtr nmpCur, out int fFound, out IntPtr nmpNext);

		// Token: 0x02000942 RID: 2370
		// (Invoke) Token: 0x060081DD RID: 33245
		internal delegate int UpdGetFirstChangeInSegment(IntPtr pfsclient, IntPtr nms, out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange);

		// Token: 0x02000943 RID: 2371
		// (Invoke) Token: 0x060081E1 RID: 33249
		internal delegate int UpdGetParaChange(IntPtr pfsclient, IntPtr nmp, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges);

		// Token: 0x02000944 RID: 2372
		// (Invoke) Token: 0x060081E5 RID: 33253
		internal delegate int GetParaProperties(IntPtr pfsclient, IntPtr nmp, ref PTS.FSPAP fspap);

		// Token: 0x02000945 RID: 2373
		// (Invoke) Token: 0x060081E9 RID: 33257
		internal delegate int CreateParaclient(IntPtr pfsclient, IntPtr nmp, out IntPtr pfsparaclient);

		// Token: 0x02000946 RID: 2374
		// (Invoke) Token: 0x060081ED RID: 33261
		internal delegate int TransferDisplayInfo(IntPtr pfsclient, IntPtr pfsparaclientOld, IntPtr pfsparaclientNew);

		// Token: 0x02000947 RID: 2375
		// (Invoke) Token: 0x060081F1 RID: 33265
		internal delegate int DestroyParaclient(IntPtr pfsclient, IntPtr pfsparaclient);

		// Token: 0x02000948 RID: 2376
		// (Invoke) Token: 0x060081F5 RID: 33269
		internal delegate int FInterruptFormattingAfterPara(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int vr, out int fInterruptFormatting);

		// Token: 0x02000949 RID: 2377
		// (Invoke) Token: 0x060081F9 RID: 33273
		internal delegate int GetEndnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsEndnoteSeparator, out IntPtr nmEndnoteContSeparator, out IntPtr nmsEndnoteContNotice);

		// Token: 0x0200094A RID: 2378
		// (Invoke) Token: 0x060081FD RID: 33277
		internal delegate int GetEndnoteSegment(IntPtr pfsclient, IntPtr nmsSection, out int fEndnotesPresent, out IntPtr nmsEndnotes);

		// Token: 0x0200094B RID: 2379
		// (Invoke) Token: 0x06008201 RID: 33281
		internal delegate int GetNumberEndnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolEndnote);

		// Token: 0x0200094C RID: 2380
		// (Invoke) Token: 0x06008205 RID: 33285
		internal unsafe delegate int GetEndnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolEndnote, PTS.FSCOLUMNINFO* fscolinfoEndnote, out int ccolEndnote);

		// Token: 0x0200094D RID: 2381
		// (Invoke) Token: 0x06008209 RID: 33289
		internal delegate int GetFootnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsFtnSeparator, out IntPtr nmsFtnContSeparator, out IntPtr nmsFtnContNotice);

		// Token: 0x0200094E RID: 2382
		// (Invoke) Token: 0x0600820D RID: 33293
		internal delegate int FFootnoteBeneathText(IntPtr pfsclient, IntPtr nms, out int fFootnoteBeneathText);

		// Token: 0x0200094F RID: 2383
		// (Invoke) Token: 0x06008211 RID: 33297
		internal delegate int GetNumberFootnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolFootnote);

		// Token: 0x02000950 RID: 2384
		// (Invoke) Token: 0x06008215 RID: 33301
		internal unsafe delegate int GetFootnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolFootnote, PTS.FSCOLUMNINFO* fscolinfoFootnote, out int ccolFootnote);

		// Token: 0x02000951 RID: 2385
		// (Invoke) Token: 0x06008219 RID: 33305
		internal delegate int GetFootnoteSegment(IntPtr pfsclient, IntPtr nmftn, out IntPtr nmsFootnote);

		// Token: 0x02000952 RID: 2386
		// (Invoke) Token: 0x0600821D RID: 33309
		internal unsafe delegate int GetFootnotePresentationAndRejectionOrder(IntPtr pfsclient, int cFootnotes, IntPtr* rgProposedPresentationOrder, IntPtr* rgProposedRejectionOrder, out int fProposedPresentationOrderAccepted, IntPtr* rgFinalPresentationOrder, out int fProposedRejectionOrderAccepted, IntPtr* rgFinalRejectionOrder);

		// Token: 0x02000953 RID: 2387
		// (Invoke) Token: 0x06008221 RID: 33313
		internal delegate int FAllowFootnoteSeparation(IntPtr pfsclient, IntPtr nmftn, out int fAllow);

		// Token: 0x02000954 RID: 2388
		// (Invoke) Token: 0x06008225 RID: 33317
		internal delegate int DuplicateMcsclient(IntPtr pfsclient, IntPtr pmcsclientIn, out IntPtr pmcsclientNew);

		// Token: 0x02000955 RID: 2389
		// (Invoke) Token: 0x06008229 RID: 33321
		internal delegate int DestroyMcsclient(IntPtr pfsclient, IntPtr pmcsclient);

		// Token: 0x02000956 RID: 2390
		// (Invoke) Token: 0x0600822D RID: 33325
		internal delegate int FEqualMcsclient(IntPtr pfsclient, IntPtr pmcsclient1, IntPtr pmcsclient2, out int fEqual);

		// Token: 0x02000957 RID: 2391
		// (Invoke) Token: 0x06008231 RID: 33329
		internal delegate int ConvertMcsclient(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, uint fswdir, IntPtr pmcsclient, int fSuppressTopSpace, out int dvr);

		// Token: 0x02000958 RID: 2392
		// (Invoke) Token: 0x06008235 RID: 33333
		internal delegate int GetObjectHandlerInfo(IntPtr pfsclient, int idobj, IntPtr pobjectinfo);

		// Token: 0x02000959 RID: 2393
		// (Invoke) Token: 0x06008239 RID: 33337
		internal delegate int CreateParaBreakingSession(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int fsdcpStart, IntPtr pfsbreakreclineclient, uint fswdir, int urStartTrack, int durTrack, int urPageLeftMargin, out IntPtr ppfsparabreakingsession, out int fParagraphJustified);

		// Token: 0x0200095A RID: 2394
		// (Invoke) Token: 0x0600823D RID: 33341
		internal delegate int DestroyParaBreakingSession(IntPtr pfsclient, IntPtr pfsparabreakingsession);

		// Token: 0x0200095B RID: 2395
		// (Invoke) Token: 0x06008241 RID: 33345
		internal delegate int GetTextProperties(IntPtr pfsclient, IntPtr nmp, int iArea, ref PTS.FSTXTPROPS fstxtprops);

		// Token: 0x0200095C RID: 2396
		// (Invoke) Token: 0x06008245 RID: 33349
		internal delegate int GetNumberFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, out int nFootnote);

		// Token: 0x0200095D RID: 2397
		// (Invoke) Token: 0x06008249 RID: 33353
		internal unsafe delegate int GetFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, int nFootnotes, IntPtr* rgnmftn, int* rgdcp, out int cFootnotes);

		// Token: 0x0200095E RID: 2398
		// (Invoke) Token: 0x0600824D RID: 33357
		internal delegate int FormatDropCap(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int fSuppressTopSpace, out IntPtr pfsdropc, out int fInMargin, out int dur, out int dvr, out int cPolygons, out int cVertices, out int durText);

		// Token: 0x0200095F RID: 2399
		// (Invoke) Token: 0x06008251 RID: 33361
		internal unsafe delegate int GetDropCapPolygons(IntPtr pfsclient, IntPtr pfsdropc, IntPtr nmp, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough);

		// Token: 0x02000960 RID: 2400
		// (Invoke) Token: 0x06008255 RID: 33365
		internal delegate int DestroyDropCap(IntPtr pfsclient, IntPtr pfsdropc);

		// Token: 0x02000961 RID: 2401
		// (Invoke) Token: 0x06008259 RID: 33369
		internal delegate int FormatBottomText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, IntPtr pfslineLast, int dvrLine, out IntPtr pmcsclientOut);

		// Token: 0x02000962 RID: 2402
		// (Invoke) Token: 0x0600825D RID: 33373
		internal delegate int FormatLine(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine);

		// Token: 0x02000963 RID: 2403
		// (Invoke) Token: 0x06008261 RID: 33377
		internal delegate int FormatLineForced(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, int dvrAvailable, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend);

		// Token: 0x02000964 RID: 2404
		// (Invoke) Token: 0x06008265 RID: 33381
		internal unsafe delegate int FormatLineVariants(IntPtr pfsclient, IntPtr pfsparabreakingsession, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, IntPtr lineVariantRestriction, int nLineVariantsAlloc, PTS.FSLINEVARIANT* rgfslinevariant, out int nLineVariantsActual, out int iLineVariantBest);

		// Token: 0x02000965 RID: 2405
		// (Invoke) Token: 0x06008269 RID: 33385
		internal delegate int ReconstructLineVariant(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcpStart, IntPtr pbrlineIn, int dcpLine, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine);

		// Token: 0x02000966 RID: 2406
		// (Invoke) Token: 0x0600826D RID: 33389
		internal delegate int DestroyLine(IntPtr pfsclient, IntPtr pfsline);

		// Token: 0x02000967 RID: 2407
		// (Invoke) Token: 0x06008271 RID: 33393
		internal delegate int DuplicateLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn, out IntPtr pbrlineDup);

		// Token: 0x02000968 RID: 2408
		// (Invoke) Token: 0x06008275 RID: 33397
		internal delegate int DestroyLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn);

		// Token: 0x02000969 RID: 2409
		// (Invoke) Token: 0x06008279 RID: 33401
		internal delegate int SnapGridVertical(IntPtr pfsclient, uint fswdir, int vrMargin, int vrCurrent, out int vrNew);

		// Token: 0x0200096A RID: 2410
		// (Invoke) Token: 0x0600827D RID: 33405
		internal delegate int GetDvrSuppressibleBottomSpace(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsline, uint fswdir, out int dvrSuppressible);

		// Token: 0x0200096B RID: 2411
		// (Invoke) Token: 0x06008281 RID: 33409
		internal delegate int GetDvrAdvance(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, uint fswdir, out int dvr);

		// Token: 0x0200096C RID: 2412
		// (Invoke) Token: 0x06008285 RID: 33413
		internal delegate int UpdGetChangeInText(IntPtr pfsclient, IntPtr nmp, out int dcpStart, out int ddcpOld, out int ddcpNew);

		// Token: 0x0200096D RID: 2413
		// (Invoke) Token: 0x06008289 RID: 33417
		internal delegate int UpdGetDropCapChange(IntPtr pfsclient, IntPtr nmp, out int fChanged);

		// Token: 0x0200096E RID: 2414
		// (Invoke) Token: 0x0600828D RID: 33421
		internal delegate int FInterruptFormattingText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, int vr, out int fInterruptFormatting);

		// Token: 0x0200096F RID: 2415
		// (Invoke) Token: 0x06008291 RID: 33425
		internal delegate int GetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, out int fFound, out int dcpPara, out int urBBox, out int durBBox, out int dvrPara, out PTS.FSKCLEAR fskclear, out IntPtr pmcsclientAfterPara, out int cLines, out int fOptimalLines, out int fOptimalLineDcpsCached, out int dvrMinLineHeight);

		// Token: 0x02000970 RID: 2416
		// (Invoke) Token: 0x06008295 RID: 33429
		internal unsafe delegate int SetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, int dcpPara, int urBBox, int durBBox, int dvrPara, PTS.FSKCLEAR fskclear, IntPtr pmcsclientAfterPara, int cLines, int fOptimalLines, int* rgdcpOptimalLines, int dvrMinLineHeight);

		// Token: 0x02000971 RID: 2417
		// (Invoke) Token: 0x06008299 RID: 33433
		internal unsafe delegate int GetOptimalLineDcpCache(IntPtr pfsclient, int cLines, int* rgdcp);

		// Token: 0x02000972 RID: 2418
		// (Invoke) Token: 0x0600829D RID: 33437
		internal delegate int GetNumberAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, out int cAttachedObjects);

		// Token: 0x02000973 RID: 2419
		// (Invoke) Token: 0x060082A1 RID: 33441
		internal unsafe delegate int GetAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, int nAttachedObjects, IntPtr* rgnmpObjects, int* rgidobj, int* rgdcpAnchor, out int cObjects, out int fEndOfParagraph);

		// Token: 0x02000974 RID: 2420
		// (Invoke) Token: 0x060082A5 RID: 33445
		internal delegate int GetNumberAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, out int cAttachedObjects);

		// Token: 0x02000975 RID: 2421
		// (Invoke) Token: 0x060082A9 RID: 33449
		internal unsafe delegate int GetAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, int nAttachedObjects, IntPtr* rgnmpObjects, int* rgidobj, int* rgdcpAnchor, out int cObjects);

		// Token: 0x02000976 RID: 2422
		// (Invoke) Token: 0x060082AD RID: 33453
		internal delegate int UpdGetAttachedObjectChange(IntPtr pfsclient, IntPtr nmp, IntPtr nmpAttachedObject, out PTS.FSKCHANGE fskchObject);

		// Token: 0x02000977 RID: 2423
		// (Invoke) Token: 0x060082B1 RID: 33457
		internal delegate int GetDurFigureAnchor(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsparaclientFigure, IntPtr pfsline, IntPtr nmpFigure, uint fswdir, IntPtr pfsFmtLineIn, out int dur);

		// Token: 0x02000978 RID: 2424
		// (Invoke) Token: 0x060082B5 RID: 33461
		internal delegate int GetFloaterProperties(IntPtr pfsclient, IntPtr nmFloater, uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops);

		// Token: 0x02000979 RID: 2425
		// (Invoke) Token: 0x060082B9 RID: 33465
		internal delegate int FormatFloaterContentFinite(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsbrkFloaterContentIn, int fBreakRecordFromPreviousPage, IntPtr nmFloater, IntPtr pftnrej, int fEmptyOk, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsbrkFloatContentOut, out IntPtr pbrkrecpara, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x0200097A RID: 2426
		// (Invoke) Token: 0x060082BD RID: 33469
		internal delegate int FormatFloaterContentBottomless(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsbrkFloatContentOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x0200097B RID: 2427
		// (Invoke) Token: 0x060082C1 RID: 33473
		internal delegate int UpdateBottomlessFloaterContent(IntPtr pfsFloaterContent, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x0200097C RID: 2428
		// (Invoke) Token: 0x060082C5 RID: 33477
		internal unsafe delegate int GetFloaterPolygons(IntPtr pfsparaclient, IntPtr pfsFloaterContent, IntPtr nmFloater, uint fswdirTrack, int ncVertices, int nfspt, int* rgcVertices, out int cVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough);

		// Token: 0x0200097D RID: 2429
		// (Invoke) Token: 0x060082C9 RID: 33481
		internal delegate int ClearUpdateInfoInFloaterContent(IntPtr pfsFloaterContent);

		// Token: 0x0200097E RID: 2430
		// (Invoke) Token: 0x060082CD RID: 33485
		internal delegate int CompareFloaterContents(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew, out PTS.FSCOMPRESULT fscmpr);

		// Token: 0x0200097F RID: 2431
		// (Invoke) Token: 0x060082D1 RID: 33489
		internal delegate int DestroyFloaterContent(IntPtr pfsFloaterContent);

		// Token: 0x02000980 RID: 2432
		// (Invoke) Token: 0x060082D5 RID: 33493
		internal delegate int DuplicateFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent, out IntPtr pfsbrkFloaterContentDup);

		// Token: 0x02000981 RID: 2433
		// (Invoke) Token: 0x060082D9 RID: 33497
		internal delegate int DestroyFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent);

		// Token: 0x02000982 RID: 2434
		// (Invoke) Token: 0x060082DD RID: 33501
		internal delegate int GetFloaterContentColumnBalancingInfo(IntPtr pfsFloaterContent, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight);

		// Token: 0x02000983 RID: 2435
		// (Invoke) Token: 0x060082E1 RID: 33505
		internal delegate int GetFloaterContentNumberFootnotes(IntPtr pfsFloaterContent, out int cftn);

		// Token: 0x02000984 RID: 2436
		// (Invoke) Token: 0x060082E5 RID: 33509
		internal delegate int GetFloaterContentFootnoteInfo(IntPtr pfsFloaterContent, uint fswdir, int nftn, int iftnFirst, ref PTS.FSFTNINFO fsftninf, out int iftnLim);

		// Token: 0x02000985 RID: 2437
		// (Invoke) Token: 0x060082E9 RID: 33513
		internal delegate int TransferDisplayInfoInFloaterContent(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew);

		// Token: 0x02000986 RID: 2438
		// (Invoke) Token: 0x060082ED RID: 33517
		internal delegate int GetMCSClientAfterFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr pmcsclientOut);

		// Token: 0x02000987 RID: 2439
		// (Invoke) Token: 0x060082F1 RID: 33521
		internal delegate int GetDvrUsedForFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, int dvrDisplaced, out int dvrUsed);

		// Token: 0x02000988 RID: 2440
		// (Invoke) Token: 0x060082F5 RID: 33525
		internal delegate int ObjCreateContext(IntPtr pfsclient, IntPtr pfsc, IntPtr pfscbkobj, uint ffi, int idobj, out IntPtr pfssobjc);

		// Token: 0x02000989 RID: 2441
		// (Invoke) Token: 0x060082F9 RID: 33529
		internal delegate int ObjDestroyContext(IntPtr pfssobjc);

		// Token: 0x0200098A RID: 2442
		// (Invoke) Token: 0x060082FD RID: 33533
		internal delegate int ObjFormatParaFinite(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr pfsobjbrk, int fBreakRecordFromPreviousPage, IntPtr nmp, int iArea, IntPtr pftnrej, IntPtr pfsgeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, int fBreakInside, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecpara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fBreakInsidePossible);

		// Token: 0x0200098B RID: 2443
		// (Invoke) Token: 0x06008301 RID: 33537
		internal delegate int ObjFormatParaBottomless(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable);

		// Token: 0x0200098C RID: 2444
		// (Invoke) Token: 0x06008305 RID: 33541
		internal delegate int ObjUpdateBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable);

		// Token: 0x0200098D RID: 2445
		// (Invoke) Token: 0x06008309 RID: 33545
		internal delegate int ObjSynchronizeBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsgeom, uint fswdir, int dvrShift);

		// Token: 0x0200098E RID: 2446
		// (Invoke) Token: 0x0600830D RID: 33549
		internal delegate int ObjComparePara(IntPtr pfsparaclientOld, IntPtr pfsparaOld, IntPtr pfsparaclientNew, IntPtr pfsparaNew, uint fswdir, out PTS.FSCOMPRESULT fscmpr, out int dvrShifted);

		// Token: 0x0200098F RID: 2447
		// (Invoke) Token: 0x06008311 RID: 33553
		internal delegate int ObjClearUpdateInfoInPara(IntPtr pfspara);

		// Token: 0x02000990 RID: 2448
		// (Invoke) Token: 0x06008315 RID: 33557
		internal delegate int ObjDestroyPara(IntPtr pfspara);

		// Token: 0x02000991 RID: 2449
		// (Invoke) Token: 0x06008319 RID: 33561
		internal delegate int ObjDuplicateBreakRecord(IntPtr pfssobjc, IntPtr pfsbrkrecparaOrig, out IntPtr pfsbrkrecparaDup);

		// Token: 0x02000992 RID: 2450
		// (Invoke) Token: 0x0600831D RID: 33565
		internal delegate int ObjDestroyBreakRecord(IntPtr pfssobjc, IntPtr pfsobjbrk);

		// Token: 0x02000993 RID: 2451
		// (Invoke) Token: 0x06008321 RID: 33569
		internal delegate int ObjGetColumnBalancingInfo(IntPtr pfspara, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight);

		// Token: 0x02000994 RID: 2452
		// (Invoke) Token: 0x06008325 RID: 33573
		internal delegate int ObjGetNumberFootnotes(IntPtr pfspara, out int nftn);

		// Token: 0x02000995 RID: 2453
		// (Invoke) Token: 0x06008329 RID: 33577
		internal unsafe delegate int ObjGetFootnoteInfo(IntPtr pfspara, uint fswdir, int nftn, int iftnFirst, PTS.FSFTNINFO* pfsftninf, out int iftnLim);

		// Token: 0x02000996 RID: 2454
		// (Invoke) Token: 0x0600832D RID: 33581
		internal delegate int ObjShiftVertical(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsshift, uint fswdir, out PTS.FSBBOX fsbbox);

		// Token: 0x02000997 RID: 2455
		// (Invoke) Token: 0x06008331 RID: 33585
		internal delegate int ObjTransferDisplayInfoPara(IntPtr pfsparaOld, IntPtr pfsparaNew);

		// Token: 0x02000998 RID: 2456
		// (Invoke) Token: 0x06008335 RID: 33589
		internal delegate int GetTableProperties(IntPtr pfsclient, IntPtr nmTable, uint fswdirTrack, out PTS.FSTABLEOBJPROPS fstableobjprops);

		// Token: 0x02000999 RID: 2457
		// (Invoke) Token: 0x06008339 RID: 33593
		internal delegate int AutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth);

		// Token: 0x0200099A RID: 2458
		// (Invoke) Token: 0x0600833D RID: 33597
		internal delegate int UpdAutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth, out int fNoChangeInCellWidths);

		// Token: 0x0200099B RID: 2459
		// (Invoke) Token: 0x06008341 RID: 33601
		internal delegate int GetMCSClientAfterTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr ppmcsclientOut);

		// Token: 0x0200099C RID: 2460
		// (Invoke) Token: 0x06008345 RID: 33605
		internal delegate int GetFirstHeaderRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedHeader, out int fFound, out IntPtr pnmFirstHeaderRow);

		// Token: 0x0200099D RID: 2461
		// (Invoke) Token: 0x06008349 RID: 33609
		internal delegate int GetNextHeaderRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmHeaderRow, int fRepeatedHeader, out int fFound, out IntPtr pnmNextHeaderRow);

		// Token: 0x0200099E RID: 2462
		// (Invoke) Token: 0x0600834D RID: 33613
		internal delegate int GetFirstFooterRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedFooter, out int fFound, out IntPtr pnmFirstFooterRow);

		// Token: 0x0200099F RID: 2463
		// (Invoke) Token: 0x06008351 RID: 33617
		internal delegate int GetNextFooterRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmFooterRow, int fRepeatedFooter, out int fFound, out IntPtr pnmNextFooterRow);

		// Token: 0x020009A0 RID: 2464
		// (Invoke) Token: 0x06008355 RID: 33621
		internal delegate int GetFirstRow(IntPtr pfsclient, IntPtr nmTable, out int fFound, out IntPtr pnmFirstRow);

		// Token: 0x020009A1 RID: 2465
		// (Invoke) Token: 0x06008359 RID: 33625
		internal delegate int GetNextRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out int fFound, out IntPtr pnmNextRow);

		// Token: 0x020009A2 RID: 2466
		// (Invoke) Token: 0x0600835D RID: 33629
		internal delegate int UpdFChangeInHeaderFooter(IntPtr pfsclient, IntPtr nmTable, out int fHeaderChanged, out int fFooterChanged, out int fRepeatedHeaderChanged, out int fRepeatedFooterChanged);

		// Token: 0x020009A3 RID: 2467
		// (Invoke) Token: 0x06008361 RID: 33633
		internal delegate int UpdGetFirstChangeInTable(IntPtr pfsclient, IntPtr nmTable, out int fFound, out int fChangeFirst, out IntPtr pnmRowBeforeChange);

		// Token: 0x020009A4 RID: 2468
		// (Invoke) Token: 0x06008365 RID: 33637
		internal delegate int UpdGetRowChange(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges);

		// Token: 0x020009A5 RID: 2469
		// (Invoke) Token: 0x06008369 RID: 33641
		internal delegate int UpdGetCellChange(IntPtr pfsclient, IntPtr nmRow, IntPtr nmCell, out int fWidthChanged, out PTS.FSKCHANGE fskchCell);

		// Token: 0x020009A6 RID: 2470
		// (Invoke) Token: 0x0600836D RID: 33645
		internal delegate int GetDistributionKind(IntPtr pfsclient, IntPtr nmTable, uint fswdirTable, out PTS.FSKTABLEHEIGHTDISTRIBUTION tabledistr);

		// Token: 0x020009A7 RID: 2471
		// (Invoke) Token: 0x06008371 RID: 33649
		internal delegate int GetRowProperties(IntPtr pfsclient, IntPtr nmRow, uint fswdirTable, out PTS.FSTABLEROWPROPS rowprops);

		// Token: 0x020009A8 RID: 2472
		// (Invoke) Token: 0x06008375 RID: 33653
		internal unsafe delegate int GetCells(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, PTS.FSTABLEKCELLMERGE* rgkcellmerge);

		// Token: 0x020009A9 RID: 2473
		// (Invoke) Token: 0x06008379 RID: 33657
		internal delegate int FInterruptFormattingTable(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmRow, int dvr, out int fInterrupt);

		// Token: 0x020009AA RID: 2474
		// (Invoke) Token: 0x0600837D RID: 33661
		internal unsafe delegate int CalcHorizontalBBoxOfRow(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, IntPtr* rgpfscell, out int urBBox, out int durBBox);

		// Token: 0x020009AB RID: 2475
		// (Invoke) Token: 0x06008381 RID: 33665
		internal delegate int FormatCellFinite(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, IntPtr pfsFtnRejector, int fEmptyOK, uint fswdirTable, int dvrExtraHeight, int dvrAvailable, out PTS.FSFMTR pfmtr, out IntPtr ppfscell, out IntPtr pfsbrkcellOut, out int dvrUsed);

		// Token: 0x020009AC RID: 2476
		// (Invoke) Token: 0x06008385 RID: 33669
		internal delegate int FormatCellBottomless(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out IntPtr ppfscell, out int dvrUsed);

		// Token: 0x020009AD RID: 2477
		// (Invoke) Token: 0x06008389 RID: 33673
		internal delegate int UpdateBottomlessCell(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out int dvrUsed);

		// Token: 0x020009AE RID: 2478
		// (Invoke) Token: 0x0600838D RID: 33677
		internal delegate int CompareCells(IntPtr pfscellOld, IntPtr pfscellNew, out PTS.FSCOMPRESULT pfscmpr);

		// Token: 0x020009AF RID: 2479
		// (Invoke) Token: 0x06008391 RID: 33681
		internal delegate int ClearUpdateInfoInCell(IntPtr pfscell);

		// Token: 0x020009B0 RID: 2480
		// (Invoke) Token: 0x06008395 RID: 33685
		internal delegate int SetCellHeight(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, int fBrokenHere, uint fswdirTable, int dvrActual);

		// Token: 0x020009B1 RID: 2481
		// (Invoke) Token: 0x06008399 RID: 33689
		internal delegate int DestroyCell(IntPtr pfsCell);

		// Token: 0x020009B2 RID: 2482
		// (Invoke) Token: 0x0600839D RID: 33693
		internal delegate int DuplicateCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell, out IntPtr ppfsbrkcellDup);

		// Token: 0x020009B3 RID: 2483
		// (Invoke) Token: 0x060083A1 RID: 33697
		internal delegate int DestroyCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell);

		// Token: 0x020009B4 RID: 2484
		// (Invoke) Token: 0x060083A5 RID: 33701
		internal delegate int GetCellNumberFootnotes(IntPtr pfscell, out int cFtn);

		// Token: 0x020009B5 RID: 2485
		// (Invoke) Token: 0x060083A9 RID: 33705
		internal delegate int GetCellMinColumnBalancingStep(IntPtr pfscell, uint fswdir, out int pdvrMinStep);

		// Token: 0x020009B6 RID: 2486
		// (Invoke) Token: 0x060083AD RID: 33709
		internal delegate int TransferDisplayInfoCell(IntPtr pfscellOld, IntPtr pfscellNew);
	}
}
