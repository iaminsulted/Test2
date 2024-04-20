using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000123 RID: 291
	internal sealed class FlowDocumentPage : DocumentPage, IServiceProvider, IDisposable, IContentHost
	{
		// Token: 0x060007A6 RID: 1958 RVA: 0x00110DC9 File Offset: 0x0010FDC9
		internal FlowDocumentPage(StructuralCache structuralCache) : base(null)
		{
			this._structuralCache = structuralCache;
			this._ptsPage = new PtsPage(structuralCache.Section);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00110DEC File Offset: 0x0010FDEC
		~FlowDocumentPage()
		{
			this.Dispose(false);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00110E1C File Offset: 0x0010FE1C
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
			base.Dispose();
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00110E31 File Offset: 0x0010FE31
		public override Visual Visual
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				this.UpdateVisual();
				return base.Visual;
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00110E4C File Offset: 0x0010FE4C
		internal void FormatBottomless(Size pageSize, Thickness pageMargin)
		{
			Invariant.Assert(!this.IsDisposed);
			this._formattedLinesCount = 0;
			TextDpi.EnsureValidPageSize(ref pageSize);
			this._pageMargin = pageMargin;
			base.SetSize(pageSize);
			if (!DoubleUtil.AreClose(this._lastFormatWidth, pageSize.Width) || !DoubleUtil.AreClose(this._pageMargin.Left, pageMargin.Left) || !DoubleUtil.AreClose(this._pageMargin.Right, pageMargin.Right))
			{
				this._structuralCache.InvalidateFormatCache(false);
			}
			this._lastFormatWidth = pageSize.Width;
			using (this._structuralCache.SetDocumentFormatContext(this))
			{
				this.OnBeforeFormatPage();
				if (this._ptsPage.PrepareForBottomlessUpdate())
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, true, false);
					this._ptsPage.UpdateBottomlessPage();
				}
				else
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, false, false);
					this._ptsPage.CreateBottomlessPage();
				}
				pageSize = this._ptsPage.CalculatedSize;
				pageSize.Width += pageMargin.Left + pageMargin.Right;
				pageSize.Height += pageMargin.Top + pageMargin.Bottom;
				base.SetSize(pageSize);
				base.SetContentBox(new Rect(pageMargin.Left, pageMargin.Top, this._ptsPage.CalculatedSize.Width, this._ptsPage.CalculatedSize.Height));
				this._structuralCache.CurrentFormatContext.PopPageData();
				this.OnAfterFormatPage();
				this._structuralCache.DetectInvalidOperation();
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00111024 File Offset: 0x00110024
		internal PageBreakRecord FormatFinite(Size pageSize, Thickness pageMargin, PageBreakRecord breakRecord)
		{
			Invariant.Assert(!this.IsDisposed);
			this._formattedLinesCount = 0;
			TextDpi.EnsureValidPageSize(ref pageSize);
			TextDpi.EnsureValidPageMargin(ref pageMargin, pageSize);
			double num = PtsHelper.CalculatePageMarginAdjustment(this._structuralCache, pageSize.Width - (pageMargin.Left + pageMargin.Right));
			if (!DoubleUtil.IsZero(num))
			{
				pageMargin.Right += num - num / 100.0;
			}
			this._pageMargin = pageMargin;
			base.SetSize(pageSize);
			base.SetContentBox(new Rect(pageMargin.Left, pageMargin.Top, pageSize.Width - (pageMargin.Left + pageMargin.Right), pageSize.Height - (pageMargin.Top + pageMargin.Bottom)));
			using (this._structuralCache.SetDocumentFormatContext(this))
			{
				this.OnBeforeFormatPage();
				if (this._ptsPage.PrepareForFiniteUpdate(breakRecord))
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, true, true);
					this._ptsPage.UpdateFinitePage(breakRecord);
				}
				else
				{
					this._structuralCache.CurrentFormatContext.PushNewPageData(pageSize, this._pageMargin, false, true);
					this._ptsPage.CreateFinitePage(breakRecord);
				}
				this._structuralCache.CurrentFormatContext.PopPageData();
				this.OnAfterFormatPage();
				this._structuralCache.DetectInvalidOperation();
			}
			return this._ptsPage.BreakRecord;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x001111A8 File Offset: 0x001101A8
		internal void Arrange(Size partitionSize)
		{
			Invariant.Assert(!this.IsDisposed);
			this._partitionSize = partitionSize;
			using (this._structuralCache.SetDocumentArrangeContext(this))
			{
				this._ptsPage.ArrangePage();
				this._structuralCache.DetectInvalidOperation();
			}
			this.ValidateTextView();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00111210 File Offset: 0x00110210
		internal void ForceReformat()
		{
			Invariant.Assert(!this.IsDisposed);
			this._ptsPage.ClearUpdateInfo();
			this._structuralCache.ForceReformat = true;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00111238 File Offset: 0x00110238
		internal IInputElement InputHitTestCore(Point point)
		{
			Invariant.Assert(!this.IsDisposed);
			if (FrameworkElement.GetFrameworkParent(this._structuralCache.FormattingOwner) == null)
			{
				return null;
			}
			IInputElement inputElement = null;
			if (this.IsLayoutDataValid)
			{
				GeneralTransform generalTransform = this.PageVisual.Child.TransformToAncestor(this.PageVisual);
				generalTransform = generalTransform.Inverse;
				if (generalTransform != null)
				{
					point = generalTransform.Transform(point);
					inputElement = this._ptsPage.InputHitTest(point);
				}
			}
			if (inputElement == null)
			{
				return this._structuralCache.FormattingOwner;
			}
			return inputElement;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x001112BC File Offset: 0x001102BC
		internal ReadOnlyCollection<Rect> GetRectanglesCore(ContentElement child, bool isLimitedToTextView)
		{
			Invariant.Assert(!this.IsDisposed);
			List<Rect> list = new List<Rect>();
			if (this.IsLayoutDataValid)
			{
				TextPointer textPointer = this.FindElementPosition(child, isLimitedToTextView);
				if (textPointer != null)
				{
					int offsetToPosition = this._structuralCache.TextContainer.Start.GetOffsetToPosition(textPointer);
					int length = 1;
					if (child is TextElement)
					{
						TextPointer position = new TextPointer(((TextElement)child).ElementEnd);
						length = textPointer.GetOffsetToPosition(position);
					}
					list = this._ptsPage.GetRectangles(child, offsetToPosition, length);
				}
			}
			if (this.PageVisual != null && list.Count > 0)
			{
				List<Rect> list2 = new List<Rect>(list.Count);
				GeneralTransform generalTransform = this.PageVisual.Child.TransformToAncestor(this.PageVisual);
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(generalTransform.TransformBounds(list[i]));
				}
				list = list2;
			}
			Invariant.Assert(list != null);
			return new ReadOnlyCollection<Rect>(list);
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x001113B0 File Offset: 0x001103B0
		internal IEnumerator<IInputElement> HostedElementsCore
		{
			get
			{
				if (this.IsLayoutDataValid)
				{
					this._textView = this.GetTextView();
					Invariant.Assert(this._textView != null && ((ITextView)this._textView).TextSegments.Count > 0);
					return new HostedElements(((ITextView)this._textView).TextSegments);
				}
				return new HostedElements(new ReadOnlyCollection<TextSegment>(new List<TextSegment>(0)));
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00111418 File Offset: 0x00110418
		internal ReadOnlyCollection<ParagraphResult> FloatingElementResults
		{
			get
			{
				List<ParagraphResult> list = new List<ParagraphResult>(0);
				List<BaseParaClient> floatingElementList = this._ptsPage.PageContext.FloatingElementList;
				if (floatingElementList != null)
				{
					for (int i = 0; i < floatingElementList.Count; i++)
					{
						ParagraphResult item = floatingElementList[i].CreateParagraphResult();
						list.Add(item);
					}
				}
				return new ReadOnlyCollection<ParagraphResult>(list);
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0011146B File Offset: 0x0011046B
		internal void OnChildDesiredSizeChangedCore(UIElement child)
		{
			this._structuralCache.FormattingOwner.OnChildDesiredSizeChanged(child);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00111480 File Offset: 0x00110480
		internal ReadOnlyCollection<ColumnResult> GetColumnResults(out bool hasTextContent)
		{
			Invariant.Assert(!this.IsDisposed);
			List<ColumnResult> list = new List<ColumnResult>(0);
			hasTextContent = false;
			if (!(this._ptsPage.PageHandle == IntPtr.Zero))
			{
				PTS.FSPAGEDETAILS fspagedetails;
				PTS.Validate(PTS.FsQueryPageDetails(this.StructuralCache.PtsContext.Context, this._ptsPage.PageHandle, out fspagedetails));
				if (PTS.ToBoolean(fspagedetails.fSimple))
				{
					PTS.FSTRACKDETAILS fstrackdetails;
					PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, fspagedetails.u.simple.trackdescr.pfstrack, out fstrackdetails));
					if (fstrackdetails.cParas > 0)
					{
						list = new List<ColumnResult>(1);
						ColumnResult columnResult = new ColumnResult(this, ref fspagedetails.u.simple.trackdescr, default(Vector));
						list.Add(columnResult);
						if (columnResult.HasTextContent)
						{
							hasTextContent = true;
						}
					}
				}
				else if (fspagedetails.u.complex.cSections > 0)
				{
					PTS.FSSECTIONDESCRIPTION[] array;
					PtsHelper.SectionListFromPage(this.StructuralCache.PtsContext, this._ptsPage.PageHandle, ref fspagedetails, out array);
					PTS.FSSECTIONDETAILS fssectiondetails;
					PTS.Validate(PTS.FsQuerySectionDetails(this.StructuralCache.PtsContext.Context, array[0].pfssection, out fssectiondetails));
					if (PTS.ToBoolean(fssectiondetails.fFootnotesAsPagenotes) && fssectiondetails.u.withpagenotes.cBasicColumns > 0)
					{
						PTS.FSTRACKDESCRIPTION[] array2;
						PtsHelper.TrackListFromSection(this.StructuralCache.PtsContext, array[0].pfssection, ref fssectiondetails, out array2);
						list = new List<ColumnResult>(fssectiondetails.u.withpagenotes.cBasicColumns);
						foreach (PTS.FSTRACKDESCRIPTION fstrackdescription in array2)
						{
							if (fstrackdescription.pfstrack != IntPtr.Zero)
							{
								PTS.FSTRACKDETAILS fstrackdetails2;
								PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, fstrackdescription.pfstrack, out fstrackdetails2));
								if (fstrackdetails2.cParas > 0)
								{
									ColumnResult columnResult2 = new ColumnResult(this, ref fstrackdescription, default(Vector));
									list.Add(columnResult2);
									if (columnResult2.HasTextContent)
									{
										hasTextContent = true;
									}
								}
							}
						}
					}
				}
			}
			Invariant.Assert(list != null);
			return new ReadOnlyCollection<ColumnResult>(list);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x001116CC File Offset: 0x001106CC
		internal TextContentRange GetTextContentRangeFromColumn(IntPtr pfstrack)
		{
			Invariant.Assert(!this.IsDisposed);
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, pfstrack, out fstrackdetails));
			TextContentRange textContentRange = new TextContentRange();
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromTrack(this.StructuralCache.PtsContext, pfstrack, ref fstrackdetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = this.StructuralCache.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					textContentRange.Merge(baseParaClient.GetTextContentRange());
				}
			}
			return textContentRange;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00111770 File Offset: 0x00110770
		internal ReadOnlyCollection<ParagraphResult> GetParagraphResultsFromColumn(IntPtr pfstrack, Vector parentOffset, out bool hasTextContent)
		{
			Invariant.Assert(!this.IsDisposed);
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(this.StructuralCache.PtsContext.Context, pfstrack, out fstrackdetails));
			hasTextContent = false;
			if (fstrackdetails.cParas == 0)
			{
				return new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
			}
			PTS.FSPARADESCRIPTION[] array;
			PtsHelper.ParaListFromTrack(this.StructuralCache.PtsContext, pfstrack, ref fstrackdetails, out array);
			List<ParagraphResult> list = new List<ParagraphResult>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				BaseParaClient baseParaClient = this.StructuralCache.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				ParagraphResult paragraphResult = baseParaClient.CreateParagraphResult();
				if (paragraphResult.HasTextContent)
				{
					hasTextContent = true;
				}
				list.Add(paragraphResult);
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00111833 File Offset: 0x00110833
		internal void OnFormatLine()
		{
			Invariant.Assert(!this.IsDisposed);
			this._formattedLinesCount++;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00111851 File Offset: 0x00110851
		internal void EnsureValidVisuals()
		{
			Invariant.Assert(!this.IsDisposed);
			this.UpdateVisual();
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00111868 File Offset: 0x00110868
		internal void UpdateViewport(ref PTS.FSRECT viewport, bool drawBackground)
		{
			GeneralTransform generalTransform = this.PageVisual.Child.TransformToAncestor(this.PageVisual);
			generalTransform = generalTransform.Inverse;
			Rect rect = viewport.FromTextDpi();
			if (generalTransform != null)
			{
				rect = generalTransform.TransformBounds(rect);
			}
			if (!this.IsDisposed)
			{
				if (drawBackground)
				{
					this.PageVisual.DrawBackground((Brush)this._structuralCache.PropertyOwner.GetValue(FlowDocument.BackgroundProperty), rect);
				}
				using (this._structuralCache.SetDocumentVisualValidationContext(this))
				{
					PTS.FSRECT fsrect = new PTS.FSRECT(rect);
					this._ptsPage.UpdateViewport(ref fsrect);
					this._structuralCache.DetectInvalidOperation();
				}
				this.ValidateTextView();
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00111928 File Offset: 0x00110928
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x00111935 File Offset: 0x00110935
		internal bool UseSizingWorkaroundForTextBox
		{
			get
			{
				return this._ptsPage.UseSizingWorkaroundForTextBox;
			}
			set
			{
				this._ptsPage.UseSizingWorkaroundForTextBox = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00111943 File Offset: 0x00110943
		internal Thickness Margin
		{
			get
			{
				return this._pageMargin;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0011194B File Offset: 0x0011094B
		internal bool IsDisposed
		{
			get
			{
				return this._disposed != 0 || this._structuralCache.PtsContext.Disposed;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00111968 File Offset: 0x00110968
		internal Size ContentSize
		{
			get
			{
				Size contentSize = this._ptsPage.ContentSize;
				contentSize.Width += this._pageMargin.Left + this._pageMargin.Right;
				contentSize.Height += this._pageMargin.Top + this._pageMargin.Bottom;
				return contentSize;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x001119CC File Offset: 0x001109CC
		internal bool FinitePage
		{
			get
			{
				return this._ptsPage.FinitePage;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x001119D9 File Offset: 0x001109D9
		internal PageContext PageContext
		{
			get
			{
				return this._ptsPage.PageContext;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x001119E6 File Offset: 0x001109E6
		internal bool IncrementalUpdate
		{
			get
			{
				return this._ptsPage.IncrementalUpdate;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x001119F3 File Offset: 0x001109F3
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x001119FB File Offset: 0x001109FB
		internal int FormattedLinesCount
		{
			get
			{
				return this._formattedLinesCount;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00111A04 File Offset: 0x00110A04
		internal bool IsLayoutDataValid
		{
			get
			{
				bool result = false;
				if (!this.IsDisposed)
				{
					result = this._structuralCache.FormattingOwner.IsLayoutDataValid;
				}
				return result;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00111A2D File Offset: 0x00110A2D
		// (set) Token: 0x060007C5 RID: 1989 RVA: 0x00111A35 File Offset: 0x00110A35
		internal TextPointer DependentMax
		{
			get
			{
				return this._DependentMax;
			}
			set
			{
				if (this._DependentMax == null || (value != null && value.CompareTo(this._DependentMax) > 0))
				{
					this._DependentMax = value;
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00111A58 File Offset: 0x00110A58
		internal Rect Viewport
		{
			get
			{
				return new Rect(this.Size);
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00111A68 File Offset: 0x00110A68
		private void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				if (disposing)
				{
					if (this.PageVisual != null)
					{
						this.DestroyVisualLinks(this.PageVisual);
						this.PageVisual.Children.Clear();
						this.PageVisual.ClearDrawingContext();
					}
					if (this._ptsPage != null)
					{
						this._ptsPage.Dispose();
					}
				}
				try
				{
					if (disposing)
					{
						base.OnPageDestroyed(EventArgs.Empty);
					}
				}
				finally
				{
					this._ptsPage = null;
					this._structuralCache = null;
					this._textView = null;
					this._DependentMax = null;
				}
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00111B08 File Offset: 0x00110B08
		private void UpdateVisual()
		{
			if (this.PageVisual == null)
			{
				base.SetVisual(new PageVisual(this));
			}
			if (this._visualNeedsUpdate)
			{
				this.PageVisual.DrawBackground((Brush)this._structuralCache.PropertyOwner.GetValue(FlowDocument.BackgroundProperty), new Rect(this._partitionSize));
				ContainerVisual containerVisual = null;
				using (this._structuralCache.SetDocumentVisualValidationContext(this))
				{
					containerVisual = this._ptsPage.GetPageVisual();
					this._structuralCache.DetectInvalidOperation();
				}
				this.PageVisual.Child = containerVisual;
				FlowDirection childFD = (FlowDirection)this._structuralCache.PropertyOwner.GetValue(FlowDocument.FlowDirectionProperty);
				PtsHelper.UpdateMirroringTransform(FlowDirection.LeftToRight, childFD, containerVisual, this.Size.Width);
				using (this._structuralCache.SetDocumentVisualValidationContext(this))
				{
					this._ptsPage.ClearUpdateInfo();
					this._structuralCache.DetectInvalidOperation();
				}
				this._visualNeedsUpdate = false;
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00111C28 File Offset: 0x00110C28
		private void OnBeforeFormatPage()
		{
			if (this._visualNeedsUpdate)
			{
				this._ptsPage.ClearUpdateInfo();
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00111C3D File Offset: 0x00110C3D
		private void OnAfterFormatPage()
		{
			if (this._textView != null)
			{
				this._textView.Invalidate();
			}
			this._visualNeedsUpdate = true;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00111C5C File Offset: 0x00110C5C
		private TextPointer FindElementPosition(IInputElement e, bool isLimitedToTextView)
		{
			TextPointer textPointer = null;
			if (e is TextElement)
			{
				if ((e as TextElement).TextContainer == this._structuralCache.TextContainer)
				{
					textPointer = new TextPointer((e as TextElement).ElementStart);
				}
			}
			else
			{
				if (this._structuralCache.TextContainer.Start == null || this._structuralCache.TextContainer.End == null)
				{
					return null;
				}
				TextPointer textPointer2 = new TextPointer(this._structuralCache.TextContainer.Start);
				while (textPointer == null && ((ITextPointer)textPointer2).CompareTo(this._structuralCache.TextContainer.End) < 0)
				{
					if (textPointer2.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement)
					{
						DependencyObject adjacentElement = textPointer2.GetAdjacentElement(LogicalDirection.Forward);
						if ((adjacentElement is ContentElement || adjacentElement is UIElement) && (adjacentElement == e as ContentElement || adjacentElement == e as UIElement))
						{
							textPointer = new TextPointer(textPointer2);
						}
					}
					textPointer2.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			if (textPointer != null && isLimitedToTextView)
			{
				this._textView = this.GetTextView();
				Invariant.Assert(this._textView != null);
				for (int i = 0; i < ((ITextView)this._textView).TextSegments.Count; i++)
				{
					if (((ITextPointer)textPointer).CompareTo(((ITextView)this._textView).TextSegments[i].Start) >= 0 && ((ITextPointer)textPointer).CompareTo(((ITextView)this._textView).TextSegments[i].End) < 0)
					{
						return textPointer;
					}
				}
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00111DCC File Offset: 0x00110DCC
		private void DestroyVisualLinks(ContainerVisual visual)
		{
			VisualCollection children = visual.Children;
			if (children != null)
			{
				for (int i = 0; i < children.Count; i++)
				{
					if (children[i] is UIElementIsland)
					{
						children.RemoveAt(i);
					}
					else
					{
						Invariant.Assert(children[i] is ContainerVisual, "The children should always derive from ContainerVisual");
						this.DestroyVisualLinks((ContainerVisual)children[i]);
					}
				}
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00111E36 File Offset: 0x00110E36
		private void ValidateTextView()
		{
			if (this._textView != null)
			{
				this._textView.OnUpdated();
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00111E4B File Offset: 0x00110E4B
		private TextDocumentView GetTextView()
		{
			TextDocumentView textDocumentView = (TextDocumentView)((IServiceProvider)this).GetService(typeof(ITextView));
			Invariant.Assert(textDocumentView != null);
			return textDocumentView;
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00111E6B File Offset: 0x00110E6B
		private PageVisual PageVisual
		{
			get
			{
				return base.Visual as PageVisual;
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00111E78 File Offset: 0x00110E78
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextView))
			{
				if (this._textView == null)
				{
					this._textView = new TextDocumentView(this, this._structuralCache.TextContainer);
				}
				return this._textView;
			}
			return null;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00111ED2 File Offset: 0x00110ED2
		IInputElement IContentHost.InputHitTest(Point point)
		{
			return this.InputHitTestCore(point);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00111EDB File Offset: 0x00110EDB
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			return this.GetRectanglesCore(child, true);
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00111EE5 File Offset: 0x00110EE5
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				return this.HostedElementsCore;
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00111EED File Offset: 0x00110EED
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			this.OnChildDesiredSizeChangedCore(child);
		}

		// Token: 0x04000787 RID: 1927
		private PtsPage _ptsPage;

		// Token: 0x04000788 RID: 1928
		private StructuralCache _structuralCache;

		// Token: 0x04000789 RID: 1929
		private int _formattedLinesCount;

		// Token: 0x0400078A RID: 1930
		private TextDocumentView _textView;

		// Token: 0x0400078B RID: 1931
		private Size _partitionSize;

		// Token: 0x0400078C RID: 1932
		private Thickness _pageMargin;

		// Token: 0x0400078D RID: 1933
		private int _disposed;

		// Token: 0x0400078E RID: 1934
		private TextPointer _DependentMax;

		// Token: 0x0400078F RID: 1935
		private bool _visualNeedsUpdate;

		// Token: 0x04000790 RID: 1936
		private double _lastFormatWidth;
	}
}
