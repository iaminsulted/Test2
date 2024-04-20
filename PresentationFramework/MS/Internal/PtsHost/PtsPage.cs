using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200013A RID: 314
	internal class PtsPage : IDisposable
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x0011D12F File Offset: 0x0011C12F
		internal PtsPage(Section section) : this()
		{
			this._section = section;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0011D13E File Offset: 0x0011C13E
		private PtsPage()
		{
			this._ptsPage = new SecurityCriticalDataForSet<IntPtr>(IntPtr.Zero);
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0011D164 File Offset: 0x0011C164
		~PtsPage()
		{
			this.Dispose(false);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0011D194 File Offset: 0x0011C194
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0011D1A4 File Offset: 0x0011C1A4
		internal bool PrepareForBottomlessUpdate()
		{
			bool flag = !this.IsEmpty;
			if (!this._section.CanUpdate)
			{
				flag = false;
			}
			else if (this._section.StructuralCache != null)
			{
				if (this._section.StructuralCache.ForceReformat)
				{
					flag = false;
					this._section.StructuralCache.ClearUpdateInfo(true);
				}
				else if (this._section.StructuralCache.DtrList != null && !flag)
				{
					this._section.InvalidateStructure();
					this._section.StructuralCache.ClearUpdateInfo(false);
				}
			}
			return flag;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0011D234 File Offset: 0x0011C234
		internal bool PrepareForFiniteUpdate(PageBreakRecord breakRecord)
		{
			bool flag = !this.IsEmpty;
			if (this._section.StructuralCache != null)
			{
				if (this._section.StructuralCache.ForceReformat)
				{
					flag = false;
					this._section.InvalidateStructure();
					this._section.StructuralCache.ClearUpdateInfo(this._section.StructuralCache.DestroyStructure);
				}
				else if (this._section.StructuralCache.DtrList != null)
				{
					this._section.InvalidateStructure();
					if (!flag)
					{
						this._section.StructuralCache.ClearUpdateInfo(false);
					}
				}
				else
				{
					flag = false;
					this._section.StructuralCache.ClearUpdateInfo(false);
				}
			}
			return flag;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0011D2E4 File Offset: 0x0011C2E4
		internal IInputElement InputHitTest(Point p)
		{
			IInputElement result = null;
			if (!this.IsEmpty)
			{
				PTS.FSPOINT pt = TextDpi.ToTextPoint(p);
				result = this.InputHitTestPage(pt);
			}
			return result;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0011D30C File Offset: 0x0011C30C
		internal List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> result = new List<Rect>();
			if (!this.IsEmpty)
			{
				result = this.GetRectanglesInPage(e, start, length);
			}
			return result;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0011D332 File Offset: 0x0011C332
		private static object BackgroundFormatStatic(object arg)
		{
			Invariant.Assert(arg is PtsPage);
			((PtsPage)arg).BackgroundFormat();
			return null;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0011D350 File Offset: 0x0011C350
		private void BackgroundFormat()
		{
			FlowDocument formattingOwner = this._section.StructuralCache.FormattingOwner;
			if (formattingOwner.Formatter is FlowDocumentFormatter)
			{
				this._section.StructuralCache.BackgroundFormatInfo.BackgroundFormat(formattingOwner.BottomlessFormatter, false);
			}
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0011D398 File Offset: 0x0011C398
		private void DeferFormattingToBackground()
		{
			int cpinterrupted = this._section.StructuralCache.BackgroundFormatInfo.CPInterrupted;
			int cchAllText = this._section.StructuralCache.BackgroundFormatInfo.CchAllText;
			DirtyTextRange dtr = new DirtyTextRange(cpinterrupted, cchAllText - cpinterrupted, cchAllText - cpinterrupted, false);
			this._section.StructuralCache.AddDirtyTextRange(dtr);
			this._backgroundFormatOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, PtsPage.BackgroundUpdateCallback, this);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0011D408 File Offset: 0x0011C408
		internal void CreateBottomlessPage()
		{
			this.OnBeforeFormatPage(false, false);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			PTS.FSFMTRBL fsfmtrbl;
			IntPtr value;
			int num = PTS.FsCreatePageBottomless(this.PtsContext.Context, this._section.Handle, out fsfmtrbl, out value);
			if (num != 0)
			{
				this._ptsPage.Value = IntPtr.Zero;
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			else
			{
				this._ptsPage.Value = value;
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, false);
			if (fsfmtrbl == PTS.FSFMTRBL.fmtrblInterrupted)
			{
				this.DeferFormattingToBackground();
			}
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0011D4C0 File Offset: 0x0011C4C0
		internal void UpdateBottomlessPage()
		{
			if (this.IsEmpty)
			{
				return;
			}
			this.OnBeforeFormatPage(false, true);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			PTS.FSFMTRBL fsfmtrbl;
			int num = PTS.FsUpdateBottomlessPage(this.PtsContext.Context, this._ptsPage.Value, this._section.Handle, out fsfmtrbl);
			if (num != 0)
			{
				this.DestroyPage();
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, true);
			if (fsfmtrbl == PTS.FSFMTRBL.fmtrblInterrupted)
			{
				this.DeferFormattingToBackground();
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0011D574 File Offset: 0x0011C574
		internal void CreateFinitePage(PageBreakRecord breakRecord)
		{
			this.OnBeforeFormatPage(true, false);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			IntPtr pfsBRPageStart = (breakRecord != null) ? breakRecord.BreakRecord : IntPtr.Zero;
			PTS.FSFMTR fsfmtr;
			IntPtr value;
			IntPtr zero;
			int num = PTS.FsCreatePageFinite(this.PtsContext.Context, pfsBRPageStart, this._section.Handle, out fsfmtr, out value, out zero);
			if (num != 0)
			{
				this._ptsPage.Value = IntPtr.Zero;
				zero = IntPtr.Zero;
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			else
			{
				this._ptsPage.Value = value;
			}
			if (zero != IntPtr.Zero && this._section.StructuralCache != null)
			{
				this._breakRecord = new PageBreakRecord(this.PtsContext, new SecurityCriticalDataForSet<IntPtr>(zero), (breakRecord != null) ? (breakRecord.PageNumber + 1) : 1);
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, false);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0011D680 File Offset: 0x0011C680
		internal void UpdateFinitePage(PageBreakRecord breakRecord)
		{
			if (this.IsEmpty)
			{
				return;
			}
			this.OnBeforeFormatPage(true, true);
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			IntPtr pfsBRPageStart = (breakRecord != null) ? breakRecord.BreakRecord : IntPtr.Zero;
			PTS.FSFMTR fsfmtr;
			IntPtr intPtr;
			int num = PTS.FsUpdateFinitePage(this.PtsContext.Context, this._ptsPage.Value, pfsBRPageStart, this._section.Handle, out fsfmtr, out intPtr);
			if (num != 0)
			{
				this.DestroyPage();
				PTS.ValidateAndTrace(num, this.PtsContext);
			}
			if (intPtr != IntPtr.Zero && this._section.StructuralCache != null)
			{
				this._breakRecord = new PageBreakRecord(this.PtsContext, new SecurityCriticalDataForSet<IntPtr>(intPtr), (breakRecord != null) ? (breakRecord.PageNumber + 1) : 1);
			}
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.FormatPage, this.PageContext, this.PtsContext);
			}
			this.OnAfterFormatPage(true, true);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0011D77C File Offset: 0x0011C77C
		internal void ArrangePage()
		{
			if (this.IsEmpty)
			{
				return;
			}
			this._section.UpdateSegmentLastFormatPositions();
			PTS.FSPAGEDETAILS fspagedetails;
			PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
			if (PTS.ToBoolean(fspagedetails.fSimple))
			{
				this._section.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, fspagedetails.u.simple.trackdescr.fsrc, this._finitePage);
				PtsHelper.ArrangeTrack(this.PtsContext, ref fspagedetails.u.simple.trackdescr, PTS.FlowDirectionToFswdir(this._section.StructuralCache.PageFlowDirection));
				this._section.StructuralCache.CurrentArrangeContext.PopPageData();
				return;
			}
			ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
			if (fspagedetails.u.complex.cSections != 0)
			{
				PTS.FSSECTIONDESCRIPTION[] array;
				PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					this.ArrangeSection(ref array[i]);
				}
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0011D8B0 File Offset: 0x0011C8B0
		internal void UpdateViewport(ref PTS.FSRECT viewport)
		{
			if (!this.IsEmpty)
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					PtsHelper.UpdateViewportTrack(this.PtsContext, ref fspagedetails.u.simple.trackdescr, ref viewport);
					return;
				}
				ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fspagedetails.u.complex.cSections != 0)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						this.UpdateViewportSection(ref array[i], ref viewport);
					}
				}
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0011D97A File Offset: 0x0011C97A
		internal void ClearUpdateInfo()
		{
			if (!this.IsEmpty)
			{
				PTS.Validate(PTS.FsClearUpdateInfoInPage(this.PtsContext.Context, this._ptsPage.Value), this.PtsContext);
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0011D9AC File Offset: 0x0011C9AC
		internal ContainerVisual GetPageVisual()
		{
			if (this._visual == null)
			{
				this._visual = new ContainerVisual();
			}
			if (!this.IsEmpty)
			{
				this.UpdatePageVisuals(this._calculatedSize);
			}
			else
			{
				this._visual.Children.Clear();
			}
			return this._visual;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0011D9F8 File Offset: 0x0011C9F8
		internal PageBreakRecord BreakRecord
		{
			get
			{
				return this._breakRecord;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0011DA00 File Offset: 0x0011CA00
		internal Size CalculatedSize
		{
			get
			{
				return this._calculatedSize;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0011DA08 File Offset: 0x0011CA08
		internal Size ContentSize
		{
			get
			{
				return this._contentSize;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0011DA10 File Offset: 0x0011CA10
		internal bool FinitePage
		{
			get
			{
				return this._finitePage;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0011DA18 File Offset: 0x0011CA18
		internal PageContext PageContext
		{
			get
			{
				return this._pageContextOfThisPage;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0011DA20 File Offset: 0x0011CA20
		internal bool IncrementalUpdate
		{
			get
			{
				return this._incrementalUpdate;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0011DA28 File Offset: 0x0011CA28
		internal PtsContext PtsContext
		{
			get
			{
				return this._section.PtsContext;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0011DA35 File Offset: 0x0011CA35
		internal IntPtr PageHandle
		{
			get
			{
				return this._ptsPage.Value;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0011DA42 File Offset: 0x0011CA42
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0011DA4A File Offset: 0x0011CA4A
		internal bool UseSizingWorkaroundForTextBox
		{
			get
			{
				return this._useSizingWorkaroundForTextBox;
			}
			set
			{
				this._useSizingWorkaroundForTextBox = value;
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0011DA54 File Offset: 0x0011CA54
		private void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				if (!this.IsEmpty)
				{
					this._section.PtsContext.OnPageDisposed(this._ptsPage, disposing, true);
				}
				this._ptsPage.Value = IntPtr.Zero;
				this._breakRecord = null;
				this._visual = null;
				this._backgroundFormatOperation = null;
			}
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0011DAB8 File Offset: 0x0011CAB8
		private void OnBeforeFormatPage(bool finitePage, bool incremental)
		{
			if (!incremental && !this.IsEmpty)
			{
				this.DestroyPage();
			}
			this._incrementalUpdate = incremental;
			this._finitePage = finitePage;
			this._breakRecord = null;
			this._pageContextOfThisPage.PageRect = new PTS.FSRECT(new Rect(this._section.StructuralCache.CurrentFormatContext.PageSize));
			if (this._backgroundFormatOperation != null)
			{
				this._backgroundFormatOperation.Abort();
			}
			if (!this._finitePage)
			{
				this._section.StructuralCache.BackgroundFormatInfo.UpdateBackgroundFormatInfo();
			}
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0011DB48 File Offset: 0x0011CB48
		private void OnAfterFormatPage(bool setSize, bool incremental)
		{
			if (setSize)
			{
				PTS.FSRECT rect = this.GetRect();
				PTS.FSBBOX boundingBox = this.GetBoundingBox();
				if (!this.FinitePage && PTS.ToBoolean(boundingBox.fDefined))
				{
					rect.dv = Math.Max(rect.dv, boundingBox.fsrc.dv);
				}
				this._calculatedSize.Width = Math.Max(TextDpi.MinWidth, TextDpi.FromTextDpi(rect.du));
				this._calculatedSize.Height = Math.Max(TextDpi.MinWidth, TextDpi.FromTextDpi(rect.dv));
				if (PTS.ToBoolean(boundingBox.fDefined))
				{
					this._contentSize.Width = Math.Max(Math.Max(TextDpi.FromTextDpi(boundingBox.fsrc.du), TextDpi.MinWidth), this._calculatedSize.Width);
					this._contentSize.Height = Math.Max(TextDpi.MinWidth, TextDpi.FromTextDpi(boundingBox.fsrc.dv));
					if (!this.FinitePage)
					{
						this._contentSize.Height = Math.Max(this._contentSize.Height, this._calculatedSize.Height);
					}
				}
				else
				{
					this._contentSize = this._calculatedSize;
				}
			}
			if (!this.IsEmpty && !incremental)
			{
				this.PtsContext.OnPageCreated(this._ptsPage);
			}
			if (this._section.StructuralCache != null)
			{
				this._section.StructuralCache.ClearUpdateInfo(false);
			}
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0011DCBC File Offset: 0x0011CCBC
		private PTS.FSRECT GetRect()
		{
			PTS.FSRECT result;
			if (this.IsEmpty)
			{
				result = default(PTS.FSRECT);
			}
			else
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					result = fspagedetails.u.simple.trackdescr.fsrc;
				}
				else
				{
					ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
					result = fspagedetails.u.complex.fsrcPageBody;
				}
			}
			return result;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0011DD54 File Offset: 0x0011CD54
		private PTS.FSBBOX GetBoundingBox()
		{
			PTS.FSBBOX result = default(PTS.FSBBOX);
			if (!this.IsEmpty)
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					result = fspagedetails.u.simple.trackdescr.fsbbox;
				}
				else
				{
					ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
					result = fspagedetails.u.complex.fsbboxPageBody;
				}
			}
			return result;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0011DDE8 File Offset: 0x0011CDE8
		private void ArrangeSection(ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						this._section.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, array[i].fsrc, this._finitePage);
						PtsHelper.ArrangeTrack(this.PtsContext, ref array[i], fssectiondetails.u.withpagenotes.fswdir);
						this._section.StructuralCache.CurrentArrangeContext.PopPageData();
					}
					return;
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0011DEE8 File Offset: 0x0011CEE8
		private void UpdateViewportSection(ref PTS.FSSECTIONDESCRIPTION sectionDesc, ref PTS.FSRECT viewport)
		{
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						PtsHelper.UpdateViewportTrack(this.PtsContext, ref array[i], ref viewport);
					}
					return;
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0011DF90 File Offset: 0x0011CF90
		private void UpdatePageVisuals(Size arrangeSize)
		{
			Invariant.Assert(!this.IsEmpty);
			PTS.FSPAGEDETAILS fspagedetails;
			PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
			if (fspagedetails.fskupd == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			ErrorHandler.Assert(fspagedetails.fskupd != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
			if (this._visual.Children.Count != 2)
			{
				this._visual.Children.Clear();
				this._visual.Children.Add(new ContainerVisual());
				this._visual.Children.Add(new ContainerVisual());
			}
			ContainerVisual containerVisual = (ContainerVisual)this._visual.Children[0];
			ContainerVisual visual = (ContainerVisual)this._visual.Children[1];
			if (PTS.ToBoolean(fspagedetails.fSimple))
			{
				PTS.FSKUPDATE fskupd = fspagedetails.u.simple.trackdescr.fsupdinf.fskupd;
				if (fskupd == PTS.FSKUPDATE.fskupdInherited)
				{
					fskupd = fspagedetails.fskupd;
				}
				VisualCollection children = containerVisual.Children;
				if (fskupd == PTS.FSKUPDATE.fskupdNew)
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				else if (children.Count == 1 && children[0] is SectionVisual)
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				ContainerVisual containerVisual2 = (ContainerVisual)children[0];
				PtsHelper.UpdateTrackVisuals(this.PtsContext, containerVisual2.Children, fspagedetails.fskupd, ref fspagedetails.u.simple.trackdescr);
			}
			else
			{
				ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				bool flag = fspagedetails.u.complex.cSections == 0;
				if (!flag)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
					flag = (array.Length == 0);
					if (!flag)
					{
						ErrorHandler.Assert(array.Length == 1, ErrorHandler.NotSupportedMultiSection);
						VisualCollection children = containerVisual.Children;
						if (children.Count == 0)
						{
							children.Add(new SectionVisual());
						}
						else if (!(children[0] is SectionVisual))
						{
							children.Clear();
							children.Add(new SectionVisual());
						}
						this.UpdateSectionVisuals((SectionVisual)children[0], fspagedetails.fskupd, ref array[0]);
					}
				}
				if (flag)
				{
					containerVisual.Children.Clear();
				}
			}
			PtsHelper.UpdateFloatingElementVisuals(visual, this._pageContextOfThisPage.FloatingElementList);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0011E210 File Offset: 0x0011D210
		private void UpdateSectionVisuals(SectionVisual visual, PTS.FSKUPDATE fskupdInherited, ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			PTS.FSKUPDATE fskupdate = sectionDesc.fsupdinf.fskupd;
			if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
			{
				fskupdate = fskupdInherited;
			}
			ErrorHandler.Assert(fskupdate != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
			if (fskupdate == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			bool flag;
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				flag = (fssectiondetails.u.withpagenotes.cBasicColumns == 0);
				if (!flag)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					flag = (array.Length == 0);
					if (!flag)
					{
						ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(this._section.Element);
						visual.DrawColumnRules(ref array, TextDpi.FromTextDpi(sectionDesc.fsrc.v), TextDpi.FromTextDpi(sectionDesc.fsrc.dv), columnProperties);
						VisualCollection children = visual.Children;
						if (fskupdate == PTS.FSKUPDATE.fskupdNew)
						{
							children.Clear();
							for (int i = 0; i < array.Length; i++)
							{
								children.Add(new ContainerVisual());
							}
						}
						ErrorHandler.Assert(children.Count == array.Length, ErrorHandler.ColumnVisualCountMismatch);
						for (int j = 0; j < array.Length; j++)
						{
							ContainerVisual containerVisual = (ContainerVisual)children[j];
							PtsHelper.UpdateTrackVisuals(this.PtsContext, containerVisual.Children, fskupdate, ref array[j]);
						}
					}
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
				flag = true;
			}
			if (flag)
			{
				visual.Children.Clear();
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0011E3A8 File Offset: 0x0011D3A8
		private IInputElement InputHitTestPage(PTS.FSPOINT pt)
		{
			IInputElement inputElement = null;
			if (this._pageContextOfThisPage.FloatingElementList != null)
			{
				int num = 0;
				while (num < this._pageContextOfThisPage.FloatingElementList.Count && inputElement == null)
				{
					inputElement = this._pageContextOfThisPage.FloatingElementList[num].InputHitTest(pt);
					num++;
				}
			}
			if (inputElement == null)
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					if (fspagedetails.u.simple.trackdescr.fsrc.Contains(pt))
					{
						inputElement = PtsHelper.InputHitTestTrack(this.PtsContext, pt, ref fspagedetails.u.simple.trackdescr);
					}
				}
				else
				{
					ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
					if (fspagedetails.u.complex.cSections != 0)
					{
						PTS.FSSECTIONDESCRIPTION[] array;
						PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
						int num2 = 0;
						while (num2 < array.Length && inputElement == null)
						{
							if (array[num2].fsrc.Contains(pt))
							{
								inputElement = this.InputHitTestSection(pt, ref array[num2]);
							}
							num2++;
						}
					}
				}
			}
			return inputElement;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0011E4F8 File Offset: 0x0011D4F8
		private List<Rect> GetRectanglesInPage(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			Invariant.Assert(!this.IsEmpty);
			PTS.FSPAGEDETAILS fspagedetails;
			PTS.Validate(PTS.FsQueryPageDetails(this.PtsContext.Context, this._ptsPage.Value, out fspagedetails));
			if (PTS.ToBoolean(fspagedetails.fSimple))
			{
				list = PtsHelper.GetRectanglesInTrack(this.PtsContext, e, start, length, ref fspagedetails.u.simple.trackdescr);
			}
			else
			{
				ErrorHandler.Assert(fspagedetails.u.complex.cFootnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fspagedetails.u.complex.cSections != 0)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.PtsContext, this._ptsPage.Value, ref fspagedetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						list = this.GetRectanglesInSection(e, start, length, ref array[i]);
						Invariant.Assert(list != null);
						if (list.Count != 0)
						{
							break;
						}
					}
				}
				else
				{
					list = new List<Rect>();
				}
			}
			return list;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0011E5F0 File Offset: 0x0011D5F0
		private IInputElement InputHitTestSection(PTS.FSPOINT pt, ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			IInputElement result = null;
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].fsrc.Contains(pt))
						{
							result = PtsHelper.InputHitTestTrack(this.PtsContext, pt, ref array[i]);
							break;
						}
					}
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
			return result;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0011E6B4 File Offset: 0x0011D6B4
		private List<Rect> GetRectanglesInSection(ContentElement e, int start, int length, ref PTS.FSSECTIONDESCRIPTION sectionDesc)
		{
			PTS.FSSECTIONDETAILS fssectiondetails;
			PTS.Validate(PTS.FsQuerySectionDetails(this.PtsContext.Context, sectionDesc.pfssection, out fssectiondetails));
			List<Rect> list = new List<Rect>();
			if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes))
			{
				ErrorHandler.Assert(fssectiondetails.u.withpagenotes.cEndnoteColumns == 0, ErrorHandler.NotSupportedFootnotes);
				if (fssectiondetails.u.withpagenotes.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSection(this.PtsContext, sectionDesc.pfssection, ref fssectiondetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						List<Rect> rectanglesInTrack = PtsHelper.GetRectanglesInTrack(this.PtsContext, e, start, length, ref array[i]);
						Invariant.Assert(rectanglesInTrack != null);
						if (rectanglesInTrack.Count != 0)
						{
							list.AddRange(rectanglesInTrack);
						}
					}
				}
			}
			else
			{
				ErrorHandler.Assert(false, ErrorHandler.NotSupportedCompositeColumns);
			}
			return list;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0011E788 File Offset: 0x0011D788
		private void DestroyPage()
		{
			if (this._ptsPage.Value != IntPtr.Zero)
			{
				this.PtsContext.OnPageDisposed(this._ptsPage, true, false);
				this._ptsPage.Value = IntPtr.Zero;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0011E7C4 File Offset: 0x0011D7C4
		private bool IsEmpty
		{
			get
			{
				return this._ptsPage.Value == IntPtr.Zero;
			}
		}

		// Token: 0x040007DA RID: 2010
		private static DispatcherOperationCallback BackgroundUpdateCallback = new DispatcherOperationCallback(PtsPage.BackgroundFormatStatic);

		// Token: 0x040007DB RID: 2011
		private readonly Section _section;

		// Token: 0x040007DC RID: 2012
		private PageBreakRecord _breakRecord;

		// Token: 0x040007DD RID: 2013
		private ContainerVisual _visual;

		// Token: 0x040007DE RID: 2014
		private DispatcherOperation _backgroundFormatOperation;

		// Token: 0x040007DF RID: 2015
		private Size _calculatedSize;

		// Token: 0x040007E0 RID: 2016
		private Size _contentSize;

		// Token: 0x040007E1 RID: 2017
		private PageContext _pageContextOfThisPage = new PageContext();

		// Token: 0x040007E2 RID: 2018
		private SecurityCriticalDataForSet<IntPtr> _ptsPage;

		// Token: 0x040007E3 RID: 2019
		private bool _finitePage;

		// Token: 0x040007E4 RID: 2020
		private bool _incrementalUpdate;

		// Token: 0x040007E5 RID: 2021
		internal bool _useSizingWorkaroundForTextBox;

		// Token: 0x040007E6 RID: 2022
		private int _disposed;
	}
}
