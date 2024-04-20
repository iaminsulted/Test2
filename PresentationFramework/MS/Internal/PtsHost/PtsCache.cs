using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;
using MS.Internal.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000136 RID: 310
	internal sealed class PtsCache
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x001155A8 File Offset: 0x001145A8
		internal static PtsHost AcquireContext(PtsContext ptsContext, TextFormattingMode textFormattingMode)
		{
			PtsCache ptsCache = ptsContext.Dispatcher.PtsCache as PtsCache;
			if (ptsCache == null)
			{
				ptsCache = new PtsCache(ptsContext.Dispatcher);
				ptsContext.Dispatcher.PtsCache = ptsCache;
			}
			return ptsCache.AcquireContextCore(ptsContext, textFormattingMode);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x001155E9 File Offset: 0x001145E9
		internal static void ReleaseContext(PtsContext ptsContext)
		{
			PtsCache ptsCache = ptsContext.Dispatcher.PtsCache as PtsCache;
			Invariant.Assert(ptsCache != null, "Cannot retrieve PtsCache from PtsContext object.");
			ptsCache.ReleaseContextCore(ptsContext);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0011560F File Offset: 0x0011460F
		internal static void GetFloaterHandlerInfo(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			PtsCache ptsCache = Dispatcher.CurrentDispatcher.PtsCache as PtsCache;
			Invariant.Assert(ptsCache != null, "Cannot retrieve PtsCache from the current Dispatcher.");
			ptsCache.GetFloaterHandlerInfoCore(ptsHost, pobjectinfo);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00115635 File Offset: 0x00114635
		internal static void GetTableObjHandlerInfo(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			PtsCache ptsCache = Dispatcher.CurrentDispatcher.PtsCache as PtsCache;
			Invariant.Assert(ptsCache != null, "Cannot retrieve PtsCache from the current Dispatcher.");
			ptsCache.GetTableObjHandlerInfoCore(ptsHost, pobjectinfo);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0011565C File Offset: 0x0011465C
		internal static bool IsDisposed()
		{
			bool result = true;
			if (Dispatcher.CurrentDispatcher != null)
			{
				PtsCache ptsCache = Dispatcher.CurrentDispatcher.PtsCache as PtsCache;
				if (ptsCache != null)
				{
					result = (ptsCache._disposed == 1);
				}
			}
			return result;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00115690 File Offset: 0x00114690
		private PtsCache(Dispatcher dispatcher)
		{
			this._contextPool = new List<PtsCache.ContextDesc>(1);
			new PtsCache.PtsCacheShutDownListener(this);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x001156B8 File Offset: 0x001146B8
		~PtsCache()
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				this.DestroyPTSContexts();
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x001156F4 File Offset: 0x001146F4
		private PtsHost AcquireContextCore(PtsContext ptsContext, TextFormattingMode textFormattingMode)
		{
			int num = 0;
			while (num < this._contextPool.Count && (this._contextPool[num].InUse || this._contextPool[num].IsOptimalParagraphEnabled != ptsContext.IsOptimalParagraphEnabled))
			{
				num++;
			}
			if (num == this._contextPool.Count)
			{
				this._contextPool.Add(new PtsCache.ContextDesc());
				this._contextPool[num].IsOptimalParagraphEnabled = ptsContext.IsOptimalParagraphEnabled;
				this._contextPool[num].PtsHost = new PtsHost();
				this._contextPool[num].PtsHost.Context = this.CreatePTSContext(num, textFormattingMode);
			}
			if (this._contextPool[num].IsOptimalParagraphEnabled)
			{
				ptsContext.TextFormatter = this._contextPool[num].TextFormatter;
			}
			this._contextPool[num].InUse = true;
			this._contextPool[num].Owner = new WeakReference(ptsContext);
			return this._contextPool[num].PtsHost;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00115810 File Offset: 0x00114810
		private void ReleaseContextCore(PtsContext ptsContext)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._disposed == 0)
				{
					if (this._releaseQueue == null)
					{
						this._releaseQueue = new List<PtsContext>();
						ptsContext.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnPtsContextReleased), null);
					}
					this._releaseQueue.Add(ptsContext);
				}
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0011588C File Offset: 0x0011488C
		private void GetFloaterHandlerInfoCore(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			int num = 0;
			while (num < this._contextPool.Count && this._contextPool[num].PtsHost != ptsHost)
			{
				num++;
			}
			Invariant.Assert(num < this._contextPool.Count, "Cannot find matching PtsHost in the Context pool.");
			PTS.Validate(PTS.GetFloaterHandlerInfo(ref this._contextPool[num].FloaterInit, pobjectinfo));
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x001158F8 File Offset: 0x001148F8
		private void GetTableObjHandlerInfoCore(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			int num = 0;
			while (num < this._contextPool.Count && this._contextPool[num].PtsHost != ptsHost)
			{
				num++;
			}
			Invariant.Assert(num < this._contextPool.Count, "Cannot find matching PtsHost in the context pool.");
			PTS.Validate(PTS.GetTableObjHandlerInfo(ref this._contextPool[num].TableobjInit, pobjectinfo));
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00115963 File Offset: 0x00114963
		private void Shutdown()
		{
			GC.WaitForPendingFinalizers();
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				this.OnPtsContextReleased(false);
				this.DestroyPTSContexts();
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00115988 File Offset: 0x00114988
		private void DestroyPTSContexts()
		{
			int i = 0;
			while (i < this._contextPool.Count)
			{
				PtsContext ptsContext = this._contextPool[i].Owner.Target as PtsContext;
				if (ptsContext != null)
				{
					Invariant.Assert(this._contextPool[i].PtsHost.Context == ptsContext.Context, "PTS Context mismatch.");
					this._contextPool[i].Owner = new WeakReference(null);
					this._contextPool[i].InUse = false;
					Invariant.Assert(!ptsContext.Disposed, "PtsContext has been already disposed.");
					ptsContext.Dispose();
				}
				if (!this._contextPool[i].InUse)
				{
					Invariant.Assert(this._contextPool[i].PtsHost.Context != IntPtr.Zero, "PTS Context handle is not valid.");
					PTS.IgnoreError(PTS.DestroyDocContext(this._contextPool[i].PtsHost.Context));
					Invariant.Assert(this._contextPool[i].InstalledObjects != IntPtr.Zero, "Installed Objects handle is not valid.");
					PTS.IgnoreError(PTS.DestroyInstalledObjectsInfo(this._contextPool[i].InstalledObjects));
					if (this._contextPool[i].TextPenaltyModule != null)
					{
						this._contextPool[i].TextPenaltyModule.Dispose();
					}
					this._contextPool.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00115B14 File Offset: 0x00114B14
		private object OnPtsContextReleased(object args)
		{
			this.OnPtsContextReleased(true);
			return null;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00115B20 File Offset: 0x00114B20
		private void OnPtsContextReleased(bool cleanContextPool)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._releaseQueue != null)
				{
					foreach (PtsContext ptsContext in this._releaseQueue)
					{
						int i;
						for (i = 0; i < this._contextPool.Count; i++)
						{
							if (this._contextPool[i].PtsHost.Context == ptsContext.Context)
							{
								this._contextPool[i].Owner = new WeakReference(null);
								this._contextPool[i].InUse = false;
								break;
							}
						}
						Invariant.Assert(i < this._contextPool.Count, "PtsContext not found in the context pool.");
						Invariant.Assert(!ptsContext.Disposed, "PtsContext has been already disposed.");
						ptsContext.Dispose();
					}
					this._releaseQueue = null;
				}
			}
			if (cleanContextPool && this._contextPool.Count > 4)
			{
				int i = 4;
				while (i < this._contextPool.Count)
				{
					if (!this._contextPool[i].InUse)
					{
						Invariant.Assert(this._contextPool[i].PtsHost.Context != IntPtr.Zero, "PTS Context handle is not valid.");
						PTS.Validate(PTS.DestroyDocContext(this._contextPool[i].PtsHost.Context));
						Invariant.Assert(this._contextPool[i].InstalledObjects != IntPtr.Zero, "Installed Objects handle is not valid.");
						PTS.Validate(PTS.DestroyInstalledObjectsInfo(this._contextPool[i].InstalledObjects));
						if (this._contextPool[i].TextPenaltyModule != null)
						{
							this._contextPool[i].TextPenaltyModule.Dispose();
						}
						this._contextPool.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00115D50 File Offset: 0x00114D50
		private IntPtr CreatePTSContext(int index, TextFormattingMode textFormattingMode)
		{
			PtsHost ptsHost = this._contextPool[index].PtsHost;
			Invariant.Assert(ptsHost != null);
			IntPtr installedObjects;
			int installedObjectsCount;
			this.InitInstalledObjectsInfo(ptsHost, ref this._contextPool[index].SubtrackParaInfo, ref this._contextPool[index].SubpageParaInfo, out installedObjects, out installedObjectsCount);
			this._contextPool[index].InstalledObjects = installedObjects;
			this.InitGenericInfo(ptsHost, (IntPtr)(index + 1), installedObjects, installedObjectsCount, ref this._contextPool[index].ContextInfo);
			this.InitFloaterObjInfo(ptsHost, ref this._contextPool[index].FloaterInit);
			this.InitTableObjInfo(ptsHost, ref this._contextPool[index].TableobjInit);
			if (this._contextPool[index].IsOptimalParagraphEnabled)
			{
				TextFormatterContext textFormatterContext = new TextFormatterContext();
				TextPenaltyModule textPenaltyModule = textFormatterContext.GetTextPenaltyModule();
				IntPtr ptsPenaltyModule = textPenaltyModule.DangerousGetHandle();
				this._contextPool[index].TextPenaltyModule = textPenaltyModule;
				this._contextPool[index].ContextInfo.ptsPenaltyModule = ptsPenaltyModule;
				this._contextPool[index].TextFormatter = TextFormatter.CreateFromContext(textFormatterContext, textFormattingMode);
				GC.SuppressFinalize(this._contextPool[index].TextPenaltyModule);
			}
			IntPtr result;
			PTS.Validate(PTS.CreateDocContext(ref this._contextPool[index].ContextInfo, out result));
			return result;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00115EAC File Offset: 0x00114EAC
		private void InitGenericInfo(PtsHost ptsHost, IntPtr clientData, IntPtr installedObjects, int installedObjectsCount, ref PTS.FSCONTEXTINFO contextInfo)
		{
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			contextInfo.version = 0U;
			contextInfo.fsffi = 320U;
			contextInfo.drMinColumnBalancingStep = TextDpi.ToTextDpi(10.0);
			contextInfo.cInstalledObjects = installedObjectsCount;
			contextInfo.pInstalledObjects = installedObjects;
			contextInfo.pfsclient = clientData;
			contextInfo.pfnAssertFailed = new PTS.AssertFailed(ptsHost.AssertFailed);
			contextInfo.fscbk.cbkfig.pfnGetFigureProperties = new PTS.GetFigureProperties(ptsHost.GetFigureProperties);
			contextInfo.fscbk.cbkfig.pfnGetFigurePolygons = new PTS.GetFigurePolygons(ptsHost.GetFigurePolygons);
			contextInfo.fscbk.cbkfig.pfnCalcFigurePosition = new PTS.CalcFigurePosition(ptsHost.CalcFigurePosition);
			contextInfo.fscbk.cbkgen.pfnFSkipPage = new PTS.FSkipPage(ptsHost.FSkipPage);
			contextInfo.fscbk.cbkgen.pfnGetPageDimensions = new PTS.GetPageDimensions(ptsHost.GetPageDimensions);
			contextInfo.fscbk.cbkgen.pfnGetNextSection = new PTS.GetNextSection(ptsHost.GetNextSection);
			contextInfo.fscbk.cbkgen.pfnGetSectionProperties = new PTS.GetSectionProperties(ptsHost.GetSectionProperties);
			contextInfo.fscbk.cbkgen.pfnGetJustificationProperties = new PTS.GetJustificationProperties(ptsHost.GetJustificationProperties);
			contextInfo.fscbk.cbkgen.pfnGetMainTextSegment = new PTS.GetMainTextSegment(ptsHost.GetMainTextSegment);
			contextInfo.fscbk.cbkgen.pfnGetHeaderSegment = new PTS.GetHeaderSegment(ptsHost.GetHeaderSegment);
			contextInfo.fscbk.cbkgen.pfnGetFooterSegment = new PTS.GetFooterSegment(ptsHost.GetFooterSegment);
			contextInfo.fscbk.cbkgen.pfnUpdGetSegmentChange = new PTS.UpdGetSegmentChange(ptsHost.UpdGetSegmentChange);
			contextInfo.fscbk.cbkgen.pfnGetSectionColumnInfo = new PTS.GetSectionColumnInfo(ptsHost.GetSectionColumnInfo);
			contextInfo.fscbk.cbkgen.pfnGetSegmentDefinedColumnSpanAreaInfo = new PTS.GetSegmentDefinedColumnSpanAreaInfo(ptsHost.GetSegmentDefinedColumnSpanAreaInfo);
			contextInfo.fscbk.cbkgen.pfnGetHeightDefinedColumnSpanAreaInfo = new PTS.GetHeightDefinedColumnSpanAreaInfo(ptsHost.GetHeightDefinedColumnSpanAreaInfo);
			contextInfo.fscbk.cbkgen.pfnGetFirstPara = new PTS.GetFirstPara(ptsHost.GetFirstPara);
			contextInfo.fscbk.cbkgen.pfnGetNextPara = new PTS.GetNextPara(ptsHost.GetNextPara);
			contextInfo.fscbk.cbkgen.pfnUpdGetFirstChangeInSegment = new PTS.UpdGetFirstChangeInSegment(ptsHost.UpdGetFirstChangeInSegment);
			contextInfo.fscbk.cbkgen.pfnUpdGetParaChange = new PTS.UpdGetParaChange(ptsHost.UpdGetParaChange);
			contextInfo.fscbk.cbkgen.pfnGetParaProperties = new PTS.GetParaProperties(ptsHost.GetParaProperties);
			contextInfo.fscbk.cbkgen.pfnCreateParaclient = new PTS.CreateParaclient(ptsHost.CreateParaclient);
			contextInfo.fscbk.cbkgen.pfnTransferDisplayInfo = new PTS.TransferDisplayInfo(ptsHost.TransferDisplayInfo);
			contextInfo.fscbk.cbkgen.pfnDestroyParaclient = new PTS.DestroyParaclient(ptsHost.DestroyParaclient);
			contextInfo.fscbk.cbkgen.pfnFInterruptFormattingAfterPara = new PTS.FInterruptFormattingAfterPara(ptsHost.FInterruptFormattingAfterPara);
			contextInfo.fscbk.cbkgen.pfnGetEndnoteSeparators = new PTS.GetEndnoteSeparators(ptsHost.GetEndnoteSeparators);
			contextInfo.fscbk.cbkgen.pfnGetEndnoteSegment = new PTS.GetEndnoteSegment(ptsHost.GetEndnoteSegment);
			contextInfo.fscbk.cbkgen.pfnGetNumberEndnoteColumns = new PTS.GetNumberEndnoteColumns(ptsHost.GetNumberEndnoteColumns);
			contextInfo.fscbk.cbkgen.pfnGetEndnoteColumnInfo = new PTS.GetEndnoteColumnInfo(ptsHost.GetEndnoteColumnInfo);
			contextInfo.fscbk.cbkgen.pfnGetFootnoteSeparators = new PTS.GetFootnoteSeparators(ptsHost.GetFootnoteSeparators);
			contextInfo.fscbk.cbkgen.pfnFFootnoteBeneathText = new PTS.FFootnoteBeneathText(ptsHost.FFootnoteBeneathText);
			contextInfo.fscbk.cbkgen.pfnGetNumberFootnoteColumns = new PTS.GetNumberFootnoteColumns(ptsHost.GetNumberFootnoteColumns);
			contextInfo.fscbk.cbkgen.pfnGetFootnoteColumnInfo = new PTS.GetFootnoteColumnInfo(ptsHost.GetFootnoteColumnInfo);
			contextInfo.fscbk.cbkgen.pfnGetFootnoteSegment = new PTS.GetFootnoteSegment(ptsHost.GetFootnoteSegment);
			contextInfo.fscbk.cbkgen.pfnGetFootnotePresentationAndRejectionOrder = new PTS.GetFootnotePresentationAndRejectionOrder(ptsHost.GetFootnotePresentationAndRejectionOrder);
			contextInfo.fscbk.cbkgen.pfnFAllowFootnoteSeparation = new PTS.FAllowFootnoteSeparation(ptsHost.FAllowFootnoteSeparation);
			contextInfo.fscbk.cbkobj.pfnDuplicateMcsclient = new PTS.DuplicateMcsclient(ptsHost.DuplicateMcsclient);
			contextInfo.fscbk.cbkobj.pfnDestroyMcsclient = new PTS.DestroyMcsclient(ptsHost.DestroyMcsclient);
			contextInfo.fscbk.cbkobj.pfnFEqualMcsclient = new PTS.FEqualMcsclient(ptsHost.FEqualMcsclient);
			contextInfo.fscbk.cbkobj.pfnConvertMcsclient = new PTS.ConvertMcsclient(ptsHost.ConvertMcsclient);
			contextInfo.fscbk.cbkobj.pfnGetObjectHandlerInfo = new PTS.GetObjectHandlerInfo(ptsHost.GetObjectHandlerInfo);
			contextInfo.fscbk.cbktxt.pfnCreateParaBreakingSession = new PTS.CreateParaBreakingSession(ptsHost.CreateParaBreakingSession);
			contextInfo.fscbk.cbktxt.pfnDestroyParaBreakingSession = new PTS.DestroyParaBreakingSession(ptsHost.DestroyParaBreakingSession);
			contextInfo.fscbk.cbktxt.pfnGetTextProperties = new PTS.GetTextProperties(ptsHost.GetTextProperties);
			contextInfo.fscbk.cbktxt.pfnGetNumberFootnotes = new PTS.GetNumberFootnotes(ptsHost.GetNumberFootnotes);
			contextInfo.fscbk.cbktxt.pfnGetFootnotes = new PTS.GetFootnotes(ptsHost.GetFootnotes);
			contextInfo.fscbk.cbktxt.pfnFormatDropCap = new PTS.FormatDropCap(ptsHost.FormatDropCap);
			contextInfo.fscbk.cbktxt.pfnGetDropCapPolygons = new PTS.GetDropCapPolygons(ptsHost.GetDropCapPolygons);
			contextInfo.fscbk.cbktxt.pfnDestroyDropCap = new PTS.DestroyDropCap(ptsHost.DestroyDropCap);
			contextInfo.fscbk.cbktxt.pfnFormatBottomText = new PTS.FormatBottomText(ptsHost.FormatBottomText);
			contextInfo.fscbk.cbktxt.pfnFormatLine = new PTS.FormatLine(ptsHost.FormatLine);
			contextInfo.fscbk.cbktxt.pfnFormatLineForced = new PTS.FormatLineForced(ptsHost.FormatLineForced);
			contextInfo.fscbk.cbktxt.pfnFormatLineVariants = new PTS.FormatLineVariants(ptsHost.FormatLineVariants);
			contextInfo.fscbk.cbktxt.pfnReconstructLineVariant = new PTS.ReconstructLineVariant(ptsHost.ReconstructLineVariant);
			contextInfo.fscbk.cbktxt.pfnDestroyLine = new PTS.DestroyLine(ptsHost.DestroyLine);
			contextInfo.fscbk.cbktxt.pfnDuplicateLineBreakRecord = new PTS.DuplicateLineBreakRecord(ptsHost.DuplicateLineBreakRecord);
			contextInfo.fscbk.cbktxt.pfnDestroyLineBreakRecord = new PTS.DestroyLineBreakRecord(ptsHost.DestroyLineBreakRecord);
			contextInfo.fscbk.cbktxt.pfnSnapGridVertical = new PTS.SnapGridVertical(ptsHost.SnapGridVertical);
			contextInfo.fscbk.cbktxt.pfnGetDvrSuppressibleBottomSpace = new PTS.GetDvrSuppressibleBottomSpace(ptsHost.GetDvrSuppressibleBottomSpace);
			contextInfo.fscbk.cbktxt.pfnGetDvrAdvance = new PTS.GetDvrAdvance(ptsHost.GetDvrAdvance);
			contextInfo.fscbk.cbktxt.pfnUpdGetChangeInText = new PTS.UpdGetChangeInText(ptsHost.UpdGetChangeInText);
			contextInfo.fscbk.cbktxt.pfnUpdGetDropCapChange = new PTS.UpdGetDropCapChange(ptsHost.UpdGetDropCapChange);
			contextInfo.fscbk.cbktxt.pfnFInterruptFormattingText = new PTS.FInterruptFormattingText(ptsHost.FInterruptFormattingText);
			contextInfo.fscbk.cbktxt.pfnGetTextParaCache = new PTS.GetTextParaCache(ptsHost.GetTextParaCache);
			contextInfo.fscbk.cbktxt.pfnSetTextParaCache = new PTS.SetTextParaCache(ptsHost.SetTextParaCache);
			contextInfo.fscbk.cbktxt.pfnGetOptimalLineDcpCache = new PTS.GetOptimalLineDcpCache(ptsHost.GetOptimalLineDcpCache);
			contextInfo.fscbk.cbktxt.pfnGetNumberAttachedObjectsBeforeTextLine = new PTS.GetNumberAttachedObjectsBeforeTextLine(ptsHost.GetNumberAttachedObjectsBeforeTextLine);
			contextInfo.fscbk.cbktxt.pfnGetAttachedObjectsBeforeTextLine = new PTS.GetAttachedObjectsBeforeTextLine(ptsHost.GetAttachedObjectsBeforeTextLine);
			contextInfo.fscbk.cbktxt.pfnGetNumberAttachedObjectsInTextLine = new PTS.GetNumberAttachedObjectsInTextLine(ptsHost.GetNumberAttachedObjectsInTextLine);
			contextInfo.fscbk.cbktxt.pfnGetAttachedObjectsInTextLine = new PTS.GetAttachedObjectsInTextLine(ptsHost.GetAttachedObjectsInTextLine);
			contextInfo.fscbk.cbktxt.pfnUpdGetAttachedObjectChange = new PTS.UpdGetAttachedObjectChange(ptsHost.UpdGetAttachedObjectChange);
			contextInfo.fscbk.cbktxt.pfnGetDurFigureAnchor = new PTS.GetDurFigureAnchor(ptsHost.GetDurFigureAnchor);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0011677C File Offset: 0x0011577C
		private void InitInstalledObjectsInfo(PtsHost ptsHost, ref PTS.FSIMETHODS subtrackParaInfo, ref PTS.FSIMETHODS subpageParaInfo, out IntPtr installedObjects, out int installedObjectsCount)
		{
			subtrackParaInfo.pfnCreateContext = new PTS.ObjCreateContext(ptsHost.SubtrackCreateContext);
			subtrackParaInfo.pfnDestroyContext = new PTS.ObjDestroyContext(ptsHost.SubtrackDestroyContext);
			subtrackParaInfo.pfnFormatParaFinite = new PTS.ObjFormatParaFinite(ptsHost.SubtrackFormatParaFinite);
			subtrackParaInfo.pfnFormatParaBottomless = new PTS.ObjFormatParaBottomless(ptsHost.SubtrackFormatParaBottomless);
			subtrackParaInfo.pfnUpdateBottomlessPara = new PTS.ObjUpdateBottomlessPara(ptsHost.SubtrackUpdateBottomlessPara);
			subtrackParaInfo.pfnSynchronizeBottomlessPara = new PTS.ObjSynchronizeBottomlessPara(ptsHost.SubtrackSynchronizeBottomlessPara);
			subtrackParaInfo.pfnComparePara = new PTS.ObjComparePara(ptsHost.SubtrackComparePara);
			subtrackParaInfo.pfnClearUpdateInfoInPara = new PTS.ObjClearUpdateInfoInPara(ptsHost.SubtrackClearUpdateInfoInPara);
			subtrackParaInfo.pfnDestroyPara = new PTS.ObjDestroyPara(ptsHost.SubtrackDestroyPara);
			subtrackParaInfo.pfnDuplicateBreakRecord = new PTS.ObjDuplicateBreakRecord(ptsHost.SubtrackDuplicateBreakRecord);
			subtrackParaInfo.pfnDestroyBreakRecord = new PTS.ObjDestroyBreakRecord(ptsHost.SubtrackDestroyBreakRecord);
			subtrackParaInfo.pfnGetColumnBalancingInfo = new PTS.ObjGetColumnBalancingInfo(ptsHost.SubtrackGetColumnBalancingInfo);
			subtrackParaInfo.pfnGetNumberFootnotes = new PTS.ObjGetNumberFootnotes(ptsHost.SubtrackGetNumberFootnotes);
			subtrackParaInfo.pfnGetFootnoteInfo = new PTS.ObjGetFootnoteInfo(ptsHost.SubtrackGetFootnoteInfo);
			subtrackParaInfo.pfnGetFootnoteInfoWord = IntPtr.Zero;
			subtrackParaInfo.pfnShiftVertical = new PTS.ObjShiftVertical(ptsHost.SubtrackShiftVertical);
			subtrackParaInfo.pfnTransferDisplayInfoPara = new PTS.ObjTransferDisplayInfoPara(ptsHost.SubtrackTransferDisplayInfoPara);
			subpageParaInfo.pfnCreateContext = new PTS.ObjCreateContext(ptsHost.SubpageCreateContext);
			subpageParaInfo.pfnDestroyContext = new PTS.ObjDestroyContext(ptsHost.SubpageDestroyContext);
			subpageParaInfo.pfnFormatParaFinite = new PTS.ObjFormatParaFinite(ptsHost.SubpageFormatParaFinite);
			subpageParaInfo.pfnFormatParaBottomless = new PTS.ObjFormatParaBottomless(ptsHost.SubpageFormatParaBottomless);
			subpageParaInfo.pfnUpdateBottomlessPara = new PTS.ObjUpdateBottomlessPara(ptsHost.SubpageUpdateBottomlessPara);
			subpageParaInfo.pfnSynchronizeBottomlessPara = new PTS.ObjSynchronizeBottomlessPara(ptsHost.SubpageSynchronizeBottomlessPara);
			subpageParaInfo.pfnComparePara = new PTS.ObjComparePara(ptsHost.SubpageComparePara);
			subpageParaInfo.pfnClearUpdateInfoInPara = new PTS.ObjClearUpdateInfoInPara(ptsHost.SubpageClearUpdateInfoInPara);
			subpageParaInfo.pfnDestroyPara = new PTS.ObjDestroyPara(ptsHost.SubpageDestroyPara);
			subpageParaInfo.pfnDuplicateBreakRecord = new PTS.ObjDuplicateBreakRecord(ptsHost.SubpageDuplicateBreakRecord);
			subpageParaInfo.pfnDestroyBreakRecord = new PTS.ObjDestroyBreakRecord(ptsHost.SubpageDestroyBreakRecord);
			subpageParaInfo.pfnGetColumnBalancingInfo = new PTS.ObjGetColumnBalancingInfo(ptsHost.SubpageGetColumnBalancingInfo);
			subpageParaInfo.pfnGetNumberFootnotes = new PTS.ObjGetNumberFootnotes(ptsHost.SubpageGetNumberFootnotes);
			subpageParaInfo.pfnGetFootnoteInfo = new PTS.ObjGetFootnoteInfo(ptsHost.SubpageGetFootnoteInfo);
			subpageParaInfo.pfnShiftVertical = new PTS.ObjShiftVertical(ptsHost.SubpageShiftVertical);
			subpageParaInfo.pfnTransferDisplayInfoPara = new PTS.ObjTransferDisplayInfoPara(ptsHost.SubpageTransferDisplayInfoPara);
			PTS.Validate(PTS.CreateInstalledObjectsInfo(ref subtrackParaInfo, ref subpageParaInfo, out installedObjects, out installedObjectsCount));
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x001169E4 File Offset: 0x001159E4
		private void InitFloaterObjInfo(PtsHost ptsHost, ref PTS.FSFLOATERINIT floaterInit)
		{
			floaterInit.fsfloatercbk.pfnGetFloaterProperties = new PTS.GetFloaterProperties(ptsHost.GetFloaterProperties);
			floaterInit.fsfloatercbk.pfnFormatFloaterContentFinite = new PTS.FormatFloaterContentFinite(ptsHost.FormatFloaterContentFinite);
			floaterInit.fsfloatercbk.pfnFormatFloaterContentBottomless = new PTS.FormatFloaterContentBottomless(ptsHost.FormatFloaterContentBottomless);
			floaterInit.fsfloatercbk.pfnUpdateBottomlessFloaterContent = new PTS.UpdateBottomlessFloaterContent(ptsHost.UpdateBottomlessFloaterContent);
			floaterInit.fsfloatercbk.pfnGetFloaterPolygons = new PTS.GetFloaterPolygons(ptsHost.GetFloaterPolygons);
			floaterInit.fsfloatercbk.pfnClearUpdateInfoInFloaterContent = new PTS.ClearUpdateInfoInFloaterContent(ptsHost.ClearUpdateInfoInFloaterContent);
			floaterInit.fsfloatercbk.pfnCompareFloaterContents = new PTS.CompareFloaterContents(ptsHost.CompareFloaterContents);
			floaterInit.fsfloatercbk.pfnDestroyFloaterContent = new PTS.DestroyFloaterContent(ptsHost.DestroyFloaterContent);
			floaterInit.fsfloatercbk.pfnDuplicateFloaterContentBreakRecord = new PTS.DuplicateFloaterContentBreakRecord(ptsHost.DuplicateFloaterContentBreakRecord);
			floaterInit.fsfloatercbk.pfnDestroyFloaterContentBreakRecord = new PTS.DestroyFloaterContentBreakRecord(ptsHost.DestroyFloaterContentBreakRecord);
			floaterInit.fsfloatercbk.pfnGetFloaterContentColumnBalancingInfo = new PTS.GetFloaterContentColumnBalancingInfo(ptsHost.GetFloaterContentColumnBalancingInfo);
			floaterInit.fsfloatercbk.pfnGetFloaterContentNumberFootnotes = new PTS.GetFloaterContentNumberFootnotes(ptsHost.GetFloaterContentNumberFootnotes);
			floaterInit.fsfloatercbk.pfnGetFloaterContentFootnoteInfo = new PTS.GetFloaterContentFootnoteInfo(ptsHost.GetFloaterContentFootnoteInfo);
			floaterInit.fsfloatercbk.pfnTransferDisplayInfoInFloaterContent = new PTS.TransferDisplayInfoInFloaterContent(ptsHost.TransferDisplayInfoInFloaterContent);
			floaterInit.fsfloatercbk.pfnGetMCSClientAfterFloater = new PTS.GetMCSClientAfterFloater(ptsHost.GetMCSClientAfterFloater);
			floaterInit.fsfloatercbk.pfnGetDvrUsedForFloater = new PTS.GetDvrUsedForFloater(ptsHost.GetDvrUsedForFloater);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00116B64 File Offset: 0x00115B64
		private void InitTableObjInfo(PtsHost ptsHost, ref PTS.FSTABLEOBJINIT tableobjInit)
		{
			tableobjInit.tableobjcbk.pfnGetTableProperties = new PTS.GetTableProperties(ptsHost.GetTableProperties);
			tableobjInit.tableobjcbk.pfnAutofitTable = new PTS.AutofitTable(ptsHost.AutofitTable);
			tableobjInit.tableobjcbk.pfnUpdAutofitTable = new PTS.UpdAutofitTable(ptsHost.UpdAutofitTable);
			tableobjInit.tableobjcbk.pfnGetMCSClientAfterTable = new PTS.GetMCSClientAfterTable(ptsHost.GetMCSClientAfterTable);
			tableobjInit.tableobjcbk.pfnGetDvrUsedForFloatTable = IntPtr.Zero;
			tableobjInit.tablecbkfetch.pfnGetFirstHeaderRow = new PTS.GetFirstHeaderRow(ptsHost.GetFirstHeaderRow);
			tableobjInit.tablecbkfetch.pfnGetNextHeaderRow = new PTS.GetNextHeaderRow(ptsHost.GetNextHeaderRow);
			tableobjInit.tablecbkfetch.pfnGetFirstFooterRow = new PTS.GetFirstFooterRow(ptsHost.GetFirstFooterRow);
			tableobjInit.tablecbkfetch.pfnGetNextFooterRow = new PTS.GetNextFooterRow(ptsHost.GetNextFooterRow);
			tableobjInit.tablecbkfetch.pfnGetFirstRow = new PTS.GetFirstRow(ptsHost.GetFirstRow);
			tableobjInit.tablecbkfetch.pfnGetNextRow = new PTS.GetNextRow(ptsHost.GetNextRow);
			tableobjInit.tablecbkfetch.pfnUpdFChangeInHeaderFooter = new PTS.UpdFChangeInHeaderFooter(ptsHost.UpdFChangeInHeaderFooter);
			tableobjInit.tablecbkfetch.pfnUpdGetFirstChangeInTable = new PTS.UpdGetFirstChangeInTable(ptsHost.UpdGetFirstChangeInTable);
			tableobjInit.tablecbkfetch.pfnUpdGetRowChange = new PTS.UpdGetRowChange(ptsHost.UpdGetRowChange);
			tableobjInit.tablecbkfetch.pfnUpdGetCellChange = new PTS.UpdGetCellChange(ptsHost.UpdGetCellChange);
			tableobjInit.tablecbkfetch.pfnGetDistributionKind = new PTS.GetDistributionKind(ptsHost.GetDistributionKind);
			tableobjInit.tablecbkfetch.pfnGetRowProperties = new PTS.GetRowProperties(ptsHost.GetRowProperties);
			tableobjInit.tablecbkfetch.pfnGetCells = new PTS.GetCells(ptsHost.GetCells);
			tableobjInit.tablecbkfetch.pfnFInterruptFormattingTable = new PTS.FInterruptFormattingTable(ptsHost.FInterruptFormattingTable);
			tableobjInit.tablecbkfetch.pfnCalcHorizontalBBoxOfRow = new PTS.CalcHorizontalBBoxOfRow(ptsHost.CalcHorizontalBBoxOfRow);
			tableobjInit.tablecbkcell.pfnFormatCellFinite = new PTS.FormatCellFinite(ptsHost.FormatCellFinite);
			tableobjInit.tablecbkcell.pfnFormatCellBottomless = new PTS.FormatCellBottomless(ptsHost.FormatCellBottomless);
			tableobjInit.tablecbkcell.pfnUpdateBottomlessCell = new PTS.UpdateBottomlessCell(ptsHost.UpdateBottomlessCell);
			tableobjInit.tablecbkcell.pfnCompareCells = new PTS.CompareCells(ptsHost.CompareCells);
			tableobjInit.tablecbkcell.pfnClearUpdateInfoInCell = new PTS.ClearUpdateInfoInCell(ptsHost.ClearUpdateInfoInCell);
			tableobjInit.tablecbkcell.pfnSetCellHeight = new PTS.SetCellHeight(ptsHost.SetCellHeight);
			tableobjInit.tablecbkcell.pfnDestroyCell = new PTS.DestroyCell(ptsHost.DestroyCell);
			tableobjInit.tablecbkcell.pfnDuplicateCellBreakRecord = new PTS.DuplicateCellBreakRecord(ptsHost.DuplicateCellBreakRecord);
			tableobjInit.tablecbkcell.pfnDestroyCellBreakRecord = new PTS.DestroyCellBreakRecord(ptsHost.DestroyCellBreakRecord);
			tableobjInit.tablecbkcell.pfnGetCellNumberFootnotes = new PTS.GetCellNumberFootnotes(ptsHost.GetCellNumberFootnotes);
			tableobjInit.tablecbkcell.pfnGetCellFootnoteInfo = IntPtr.Zero;
			tableobjInit.tablecbkcell.pfnGetCellFootnoteInfoWord = IntPtr.Zero;
			tableobjInit.tablecbkcell.pfnGetCellMinColumnBalancingStep = new PTS.GetCellMinColumnBalancingStep(ptsHost.GetCellMinColumnBalancingStep);
			tableobjInit.tablecbkcell.pfnTransferDisplayInfoCell = new PTS.TransferDisplayInfoCell(ptsHost.TransferDisplayInfoCell);
		}

		// Token: 0x040007C8 RID: 1992
		private List<PtsCache.ContextDesc> _contextPool;

		// Token: 0x040007C9 RID: 1993
		private List<PtsContext> _releaseQueue;

		// Token: 0x040007CA RID: 1994
		private object _lock = new object();

		// Token: 0x040007CB RID: 1995
		private int _disposed;

		// Token: 0x020008C2 RID: 2242
		private class ContextDesc
		{
			// Token: 0x04003C37 RID: 15415
			internal PtsHost PtsHost;

			// Token: 0x04003C38 RID: 15416
			internal PTS.FSCONTEXTINFO ContextInfo;

			// Token: 0x04003C39 RID: 15417
			internal PTS.FSIMETHODS SubtrackParaInfo;

			// Token: 0x04003C3A RID: 15418
			internal PTS.FSIMETHODS SubpageParaInfo;

			// Token: 0x04003C3B RID: 15419
			internal PTS.FSFLOATERINIT FloaterInit;

			// Token: 0x04003C3C RID: 15420
			internal PTS.FSTABLEOBJINIT TableobjInit;

			// Token: 0x04003C3D RID: 15421
			internal IntPtr InstalledObjects;

			// Token: 0x04003C3E RID: 15422
			internal TextFormatter TextFormatter;

			// Token: 0x04003C3F RID: 15423
			internal TextPenaltyModule TextPenaltyModule;

			// Token: 0x04003C40 RID: 15424
			internal bool IsOptimalParagraphEnabled;

			// Token: 0x04003C41 RID: 15425
			internal WeakReference Owner;

			// Token: 0x04003C42 RID: 15426
			internal bool InUse;
		}

		// Token: 0x020008C3 RID: 2243
		private sealed class PtsCacheShutDownListener : ShutDownListener
		{
			// Token: 0x06008159 RID: 33113 RVA: 0x0032314F File Offset: 0x0032214F
			public PtsCacheShutDownListener(PtsCache target) : base(target)
			{
			}

			// Token: 0x0600815A RID: 33114 RVA: 0x00323158 File Offset: 0x00322158
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				((PtsCache)target).Shutdown();
			}
		}
	}
}
