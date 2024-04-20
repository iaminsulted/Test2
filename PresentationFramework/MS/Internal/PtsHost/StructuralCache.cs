using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000145 RID: 325
	internal sealed class StructuralCache
	{
		// Token: 0x060009CB RID: 2507 RVA: 0x0011F5DC File Offset: 0x0011E5DC
		internal StructuralCache(FlowDocument owner, TextContainer textContainer)
		{
			Invariant.Assert(owner != null);
			Invariant.Assert(textContainer != null);
			Invariant.Assert(textContainer.Parent != null);
			this._owner = owner;
			this._textContainer = textContainer;
			this._backgroundFormatInfo = new BackgroundFormatInfo(this);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0011F62C File Offset: 0x0011E62C
		~StructuralCache()
		{
			if (this._ptsContext != null)
			{
				PtsCache.ReleaseContext(this._ptsContext);
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0011F668 File Offset: 0x0011E668
		internal IDisposable SetDocumentFormatContext(FlowDocumentPage currentPage)
		{
			if (!this.CheckFlags(StructuralCache.Flags.FormattedOnce))
			{
				this.SetFlags(true, StructuralCache.Flags.FormattedOnce);
				this._owner.InitializeForFirstFormatting();
			}
			return new StructuralCache.DocumentFormatContext(this, currentPage);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0011F68D File Offset: 0x0011E68D
		internal IDisposable SetDocumentArrangeContext(FlowDocumentPage currentPage)
		{
			return new StructuralCache.DocumentArrangeContext(this, currentPage);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0011F696 File Offset: 0x0011E696
		internal IDisposable SetDocumentVisualValidationContext(FlowDocumentPage currentPage)
		{
			return new StructuralCache.DocumentVisualValidationContext(this, currentPage);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0011F69F File Offset: 0x0011E69F
		internal void DetectInvalidOperation()
		{
			if (this._illegalTreeChangeDetected)
			{
				throw new InvalidOperationException(SR.Get("IllegalTreeChangeDetectedPostAction"));
			}
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0011F6B9 File Offset: 0x0011E6B9
		internal void OnInvalidOperationDetected()
		{
			if (this._currentPage != null)
			{
				this._illegalTreeChangeDetected = true;
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0011F6CA File Offset: 0x0011E6CA
		internal void InvalidateFormatCache(bool destroyStructure)
		{
			if (this._section != null)
			{
				this._section.InvalidateFormatCache();
				this._destroyStructure = (this._destroyStructure || destroyStructure);
				this._forceReformat = true;
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0011F6F4 File Offset: 0x0011E6F4
		internal void AddDirtyTextRange(DirtyTextRange dtr)
		{
			if (this._dtrs == null)
			{
				this._dtrs = new DtrList();
			}
			this._dtrs.Merge(dtr);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0011F715 File Offset: 0x0011E715
		internal DtrList DtrsFromRange(int dcpNew, int cchOld)
		{
			if (this._dtrs == null)
			{
				return null;
			}
			return this._dtrs.DtrsFromRange(dcpNew, cchOld);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0011F730 File Offset: 0x0011E730
		internal void ClearUpdateInfo(bool destroyStructureCache)
		{
			this._dtrs = null;
			this._forceReformat = false;
			this._destroyStructure = false;
			if (this._section != null && !this._ptsContext.Disposed)
			{
				if (destroyStructureCache)
				{
					this._section.DestroyStructure();
				}
				this._section.ClearUpdateInfo();
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0011F780 File Offset: 0x0011E780
		internal void ThrottleBackgroundFormatting()
		{
			this._backgroundFormatInfo.ThrottleBackgroundFormatting();
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0011F78D File Offset: 0x0011E78D
		internal bool HasPtsContext()
		{
			return this._ptsContext != null;
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0011F798 File Offset: 0x0011E798
		internal DependencyObject PropertyOwner
		{
			get
			{
				return this._textContainer.Parent;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0011F7A5 File Offset: 0x0011E7A5
		internal FlowDocument FormattingOwner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0011F7AD File Offset: 0x0011E7AD
		internal Section Section
		{
			get
			{
				this.EnsurePtsContext();
				return this._section;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0011F7BB File Offset: 0x0011E7BB
		internal NaturalLanguageHyphenator Hyphenator
		{
			get
			{
				this.EnsureHyphenator();
				return this._hyphenator;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0011F7C9 File Offset: 0x0011E7C9
		internal PtsContext PtsContext
		{
			get
			{
				this.EnsurePtsContext();
				return this._ptsContext;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0011F7D7 File Offset: 0x0011E7D7
		internal StructuralCache.DocumentFormatContext CurrentFormatContext
		{
			get
			{
				return this._documentFormatContext;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x0011F7DF File Offset: 0x0011E7DF
		internal StructuralCache.DocumentArrangeContext CurrentArrangeContext
		{
			get
			{
				return this._documentArrangeContext;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060009DF RID: 2527 RVA: 0x0011F7E7 File Offset: 0x0011E7E7
		internal TextFormatterHost TextFormatterHost
		{
			get
			{
				this.EnsurePtsContext();
				return this._textFormatterHost;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0011F7F5 File Offset: 0x0011E7F5
		internal TextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0011F7FD File Offset: 0x0011E7FD
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x0011F805 File Offset: 0x0011E805
		internal FlowDirection PageFlowDirection
		{
			get
			{
				return this._pageFlowDirection;
			}
			set
			{
				this._pageFlowDirection = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0011F80E File Offset: 0x0011E80E
		// (set) Token: 0x060009E4 RID: 2532 RVA: 0x0011F816 File Offset: 0x0011E816
		internal bool ForceReformat
		{
			get
			{
				return this._forceReformat;
			}
			set
			{
				this._forceReformat = value;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0011F81F File Offset: 0x0011E81F
		internal bool DestroyStructure
		{
			get
			{
				return this._destroyStructure;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0011F827 File Offset: 0x0011E827
		internal DtrList DtrList
		{
			get
			{
				return this._dtrs;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0011F82F File Offset: 0x0011E82F
		internal bool IsDeferredVisualCreationSupported
		{
			get
			{
				return this._currentPage != null && !this._currentPage.FinitePage;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0011F849 File Offset: 0x0011E849
		internal BackgroundFormatInfo BackgroundFormatInfo
		{
			get
			{
				return this._backgroundFormatInfo;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0011F851 File Offset: 0x0011E851
		internal bool IsOptimalParagraphEnabled
		{
			get
			{
				return this.PtsContext.IsOptimalParagraphEnabled && (bool)this.PropertyOwner.GetValue(FlowDocument.IsOptimalParagraphEnabledProperty);
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0011F877 File Offset: 0x0011E877
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0011F880 File Offset: 0x0011E880
		internal bool IsFormattingInProgress
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.FormattingInProgress);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.FormattingInProgress);
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0011F88A File Offset: 0x0011E88A
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0011F893 File Offset: 0x0011E893
		internal bool IsContentChangeInProgress
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.ContentChangeInProgress);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.ContentChangeInProgress);
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0011F89D File Offset: 0x0011E89D
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x0011F8A6 File Offset: 0x0011E8A6
		internal bool IsFormattedOnce
		{
			get
			{
				return this.CheckFlags(StructuralCache.Flags.FormattedOnce);
			}
			set
			{
				this.SetFlags(value, StructuralCache.Flags.FormattedOnce);
			}
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0011F8B0 File Offset: 0x0011E8B0
		private void EnsureHyphenator()
		{
			if (this._hyphenator == null)
			{
				this._hyphenator = new NaturalLanguageHyphenator();
			}
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0011F8C8 File Offset: 0x0011E8C8
		private void EnsurePtsContext()
		{
			if (this._ptsContext == null)
			{
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode(this.PropertyOwner);
				this._ptsContext = new PtsContext(true, textFormattingMode);
				this._textFormatterHost = new TextFormatterHost(this._ptsContext.TextFormatter, textFormattingMode, this._owner.PixelsPerDip);
				this._section = new Section(this);
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0011F924 File Offset: 0x0011E924
		private void SetFlags(bool value, StructuralCache.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0011F942 File Offset: 0x0011E942
		private bool CheckFlags(StructuralCache.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x040007F7 RID: 2039
		private readonly FlowDocument _owner;

		// Token: 0x040007F8 RID: 2040
		private PtsContext _ptsContext;

		// Token: 0x040007F9 RID: 2041
		private Section _section;

		// Token: 0x040007FA RID: 2042
		private TextContainer _textContainer;

		// Token: 0x040007FB RID: 2043
		private TextFormatterHost _textFormatterHost;

		// Token: 0x040007FC RID: 2044
		private FlowDocumentPage _currentPage;

		// Token: 0x040007FD RID: 2045
		private StructuralCache.DocumentFormatContext _documentFormatContext;

		// Token: 0x040007FE RID: 2046
		private StructuralCache.DocumentArrangeContext _documentArrangeContext;

		// Token: 0x040007FF RID: 2047
		private DtrList _dtrs;

		// Token: 0x04000800 RID: 2048
		private bool _illegalTreeChangeDetected;

		// Token: 0x04000801 RID: 2049
		private bool _forceReformat;

		// Token: 0x04000802 RID: 2050
		private bool _destroyStructure;

		// Token: 0x04000803 RID: 2051
		private BackgroundFormatInfo _backgroundFormatInfo;

		// Token: 0x04000804 RID: 2052
		private FlowDirection _pageFlowDirection;

		// Token: 0x04000805 RID: 2053
		private NaturalLanguageHyphenator _hyphenator;

		// Token: 0x04000806 RID: 2054
		private StructuralCache.Flags _flags;

		// Token: 0x020008C5 RID: 2245
		[Flags]
		private enum Flags
		{
			// Token: 0x04003C46 RID: 15430
			FormattedOnce = 1,
			// Token: 0x04003C47 RID: 15431
			ContentChangeInProgress = 2,
			// Token: 0x04003C48 RID: 15432
			FormattingInProgress = 8
		}

		// Token: 0x020008C6 RID: 2246
		internal abstract class DocumentOperationContext
		{
			// Token: 0x0600815C RID: 33116 RVA: 0x0032317C File Offset: 0x0032217C
			internal DocumentOperationContext(StructuralCache owner, FlowDocumentPage page)
			{
				Invariant.Assert(owner != null, "Invalid owner object.");
				Invariant.Assert(page != null, "Invalid page object.");
				Invariant.Assert(owner._currentPage == null, "Page formatting reentrancy detected. Trying to create second _DocumentPageContext for the same StructuralCache.");
				this._owner = owner;
				this._owner._currentPage = page;
				this._owner._illegalTreeChangeDetected = false;
				owner.PtsContext.Enter();
			}

			// Token: 0x0600815D RID: 33117 RVA: 0x003231E8 File Offset: 0x003221E8
			protected void Dispose()
			{
				Invariant.Assert(this._owner._currentPage != null, "DocumentPageContext is already disposed.");
				try
				{
					this._owner.PtsContext.Leave();
				}
				finally
				{
					this._owner._currentPage = null;
				}
			}

			// Token: 0x17001D92 RID: 7570
			// (get) Token: 0x0600815E RID: 33118 RVA: 0x0032323C File Offset: 0x0032223C
			internal Size DocumentPageSize
			{
				get
				{
					return this._owner._currentPage.Size;
				}
			}

			// Token: 0x17001D93 RID: 7571
			// (get) Token: 0x0600815F RID: 33119 RVA: 0x0032324E File Offset: 0x0032224E
			internal Thickness DocumentPageMargin
			{
				get
				{
					return this._owner._currentPage.Margin;
				}
			}

			// Token: 0x04003C49 RID: 15433
			protected readonly StructuralCache _owner;
		}

		// Token: 0x020008C7 RID: 2247
		internal class DocumentFormatContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008160 RID: 33120 RVA: 0x00323260 File Offset: 0x00322260
			internal DocumentFormatContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
				this._owner._documentFormatContext = this;
			}

			// Token: 0x06008161 RID: 33121 RVA: 0x00323281 File Offset: 0x00322281
			void IDisposable.Dispose()
			{
				this._owner._documentFormatContext = null;
				base.Dispose();
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008162 RID: 33122 RVA: 0x0032329B File Offset: 0x0032229B
			internal void OnFormatLine()
			{
				this._owner._currentPage.OnFormatLine();
			}

			// Token: 0x06008163 RID: 33123 RVA: 0x003232B0 File Offset: 0x003222B0
			internal void PushNewPageData(Size pageSize, Thickness pageMargin, bool incrementalUpdate, bool finitePage)
			{
				this._documentFormatInfoStack.Push(this._currentFormatInfo);
				this._currentFormatInfo.PageSize = pageSize;
				this._currentFormatInfo.PageMargin = pageMargin;
				this._currentFormatInfo.IncrementalUpdate = incrementalUpdate;
				this._currentFormatInfo.FinitePage = finitePage;
			}

			// Token: 0x06008164 RID: 33124 RVA: 0x003232FF File Offset: 0x003222FF
			internal void PopPageData()
			{
				this._currentFormatInfo = this._documentFormatInfoStack.Pop();
			}

			// Token: 0x17001D94 RID: 7572
			// (get) Token: 0x06008165 RID: 33125 RVA: 0x00323312 File Offset: 0x00322312
			internal double PageHeight
			{
				get
				{
					return this._currentFormatInfo.PageSize.Height;
				}
			}

			// Token: 0x17001D95 RID: 7573
			// (get) Token: 0x06008166 RID: 33126 RVA: 0x00323324 File Offset: 0x00322324
			internal double PageWidth
			{
				get
				{
					return this._currentFormatInfo.PageSize.Width;
				}
			}

			// Token: 0x17001D96 RID: 7574
			// (get) Token: 0x06008167 RID: 33127 RVA: 0x00323336 File Offset: 0x00322336
			internal Size PageSize
			{
				get
				{
					return this._currentFormatInfo.PageSize;
				}
			}

			// Token: 0x17001D97 RID: 7575
			// (get) Token: 0x06008168 RID: 33128 RVA: 0x00323343 File Offset: 0x00322343
			internal Thickness PageMargin
			{
				get
				{
					return this._currentFormatInfo.PageMargin;
				}
			}

			// Token: 0x17001D98 RID: 7576
			// (get) Token: 0x06008169 RID: 33129 RVA: 0x00323350 File Offset: 0x00322350
			internal bool IncrementalUpdate
			{
				get
				{
					return this._currentFormatInfo.IncrementalUpdate;
				}
			}

			// Token: 0x17001D99 RID: 7577
			// (get) Token: 0x0600816A RID: 33130 RVA: 0x0032335D File Offset: 0x0032235D
			internal bool FinitePage
			{
				get
				{
					return this._currentFormatInfo.FinitePage;
				}
			}

			// Token: 0x17001D9A RID: 7578
			// (get) Token: 0x0600816B RID: 33131 RVA: 0x0032336A File Offset: 0x0032236A
			internal PTS.FSRECT PageRect
			{
				get
				{
					return new PTS.FSRECT(new Rect(0.0, 0.0, this.PageWidth, this.PageHeight));
				}
			}

			// Token: 0x17001D9B RID: 7579
			// (get) Token: 0x0600816C RID: 33132 RVA: 0x00323394 File Offset: 0x00322394
			internal PTS.FSRECT PageMarginRect
			{
				get
				{
					return new PTS.FSRECT(new Rect(this.PageMargin.Left, this.PageMargin.Top, this.PageSize.Width - this.PageMargin.Left - this.PageMargin.Right, this.PageSize.Height - this.PageMargin.Top - this.PageMargin.Bottom));
				}
			}

			// Token: 0x17001D9C RID: 7580
			// (set) Token: 0x0600816D RID: 33133 RVA: 0x0032341F File Offset: 0x0032241F
			internal TextPointer DependentMax
			{
				set
				{
					this._owner._currentPage.DependentMax = value;
				}
			}

			// Token: 0x04003C4A RID: 15434
			private StructuralCache.DocumentFormatContext.DocumentFormatInfo _currentFormatInfo;

			// Token: 0x04003C4B RID: 15435
			private Stack<StructuralCache.DocumentFormatContext.DocumentFormatInfo> _documentFormatInfoStack = new Stack<StructuralCache.DocumentFormatContext.DocumentFormatInfo>();

			// Token: 0x02000C78 RID: 3192
			private struct DocumentFormatInfo
			{
				// Token: 0x04004C7A RID: 19578
				internal Size PageSize;

				// Token: 0x04004C7B RID: 19579
				internal Thickness PageMargin;

				// Token: 0x04004C7C RID: 19580
				internal bool IncrementalUpdate;

				// Token: 0x04004C7D RID: 19581
				internal bool FinitePage;
			}
		}

		// Token: 0x020008C8 RID: 2248
		internal class DocumentArrangeContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x0600816E RID: 33134 RVA: 0x00323432 File Offset: 0x00322432
			internal DocumentArrangeContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
				this._owner._documentArrangeContext = this;
			}

			// Token: 0x0600816F RID: 33135 RVA: 0x00323453 File Offset: 0x00322453
			internal void PushNewPageData(PageContext pageContext, PTS.FSRECT columnRect, bool finitePage)
			{
				this._documentArrangeInfoStack.Push(this._currentArrangeInfo);
				this._currentArrangeInfo.PageContext = pageContext;
				this._currentArrangeInfo.ColumnRect = columnRect;
				this._currentArrangeInfo.FinitePage = finitePage;
			}

			// Token: 0x06008170 RID: 33136 RVA: 0x0032348A File Offset: 0x0032248A
			internal void PopPageData()
			{
				this._currentArrangeInfo = this._documentArrangeInfoStack.Pop();
			}

			// Token: 0x06008171 RID: 33137 RVA: 0x0032349D File Offset: 0x0032249D
			void IDisposable.Dispose()
			{
				GC.SuppressFinalize(this);
				this._owner._documentArrangeContext = null;
				base.Dispose();
			}

			// Token: 0x17001D9D RID: 7581
			// (get) Token: 0x06008172 RID: 33138 RVA: 0x003234B7 File Offset: 0x003224B7
			internal PageContext PageContext
			{
				get
				{
					return this._currentArrangeInfo.PageContext;
				}
			}

			// Token: 0x17001D9E RID: 7582
			// (get) Token: 0x06008173 RID: 33139 RVA: 0x003234C4 File Offset: 0x003224C4
			internal PTS.FSRECT ColumnRect
			{
				get
				{
					return this._currentArrangeInfo.ColumnRect;
				}
			}

			// Token: 0x17001D9F RID: 7583
			// (get) Token: 0x06008174 RID: 33140 RVA: 0x003234D1 File Offset: 0x003224D1
			internal bool FinitePage
			{
				get
				{
					return this._currentArrangeInfo.FinitePage;
				}
			}

			// Token: 0x04003C4C RID: 15436
			private StructuralCache.DocumentArrangeContext.DocumentArrangeInfo _currentArrangeInfo;

			// Token: 0x04003C4D RID: 15437
			private Stack<StructuralCache.DocumentArrangeContext.DocumentArrangeInfo> _documentArrangeInfoStack = new Stack<StructuralCache.DocumentArrangeContext.DocumentArrangeInfo>();

			// Token: 0x02000C79 RID: 3193
			private struct DocumentArrangeInfo
			{
				// Token: 0x04004C7E RID: 19582
				internal PageContext PageContext;

				// Token: 0x04004C7F RID: 19583
				internal PTS.FSRECT ColumnRect;

				// Token: 0x04004C80 RID: 19584
				internal bool FinitePage;
			}
		}

		// Token: 0x020008C9 RID: 2249
		internal class DocumentVisualValidationContext : StructuralCache.DocumentOperationContext, IDisposable
		{
			// Token: 0x06008175 RID: 33141 RVA: 0x003234DE File Offset: 0x003224DE
			internal DocumentVisualValidationContext(StructuralCache owner, FlowDocumentPage page) : base(owner, page)
			{
			}

			// Token: 0x06008176 RID: 33142 RVA: 0x003234E8 File Offset: 0x003224E8
			void IDisposable.Dispose()
			{
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}
	}
}
