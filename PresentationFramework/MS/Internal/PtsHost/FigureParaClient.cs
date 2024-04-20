using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200011D RID: 285
	internal sealed class FigureParaClient : BaseParaClient
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x0010D2D1 File Offset: 0x0010C2D1
		internal FigureParaClient(FigureParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0010D2E8 File Offset: 0x0010C2E8
		public override void Dispose()
		{
			if (this.SubpageHandle != IntPtr.Zero)
			{
				PTS.Validate(PTS.FsDestroySubpage(base.PtsContext.Context, this.SubpageHandle), base.PtsContext);
				this.SubpageHandle = IntPtr.Zero;
			}
			if (this._pageContext != null)
			{
				this._pageContext.RemoveFloatingParaClient(this);
			}
			base.Dispose();
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0010D350 File Offset: 0x0010C350
		protected override void OnArrange()
		{
			base.OnArrange();
			((FigureParagraph)base.Paragraph).UpdateSegmentLastFormatPositions();
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			this._pageContext.AddFloatingParaClient(this);
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			this._contentRect.u = this._rect.u + mbpInfo.BPLeft;
			this._contentRect.du = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.du - mbpInfo.BPRight - mbpInfo.BPLeft);
			this._contentRect.v = this._rect.v + mbpInfo.BPTop;
			this._contentRect.dv = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.dv - mbpInfo.BPBottom - mbpInfo.BPTop);
			this._paddingRect.u = this._rect.u + mbpInfo.BorderLeft;
			this._paddingRect.du = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.du - mbpInfo.BorderRight - mbpInfo.BorderLeft);
			this._paddingRect.v = this._rect.v + mbpInfo.BorderTop;
			this._paddingRect.dv = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.dv - mbpInfo.BorderBottom - mbpInfo.BorderTop);
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				this._pageContextOfThisPage.PageRect = new PTS.FSRECT(fssubpagedetails.u.simple.trackdescr.fsrc);
				base.Paragraph.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, fssubpagedetails.u.simple.trackdescr.fsrc, base.Paragraph.StructuralCache.CurrentArrangeContext.FinitePage);
				PtsHelper.ArrangeTrack(base.PtsContext, ref fssubpagedetails.u.simple.trackdescr, fssubpagedetails.u.simple.fswdir);
				base.Paragraph.StructuralCache.CurrentArrangeContext.PopPageData();
				return;
			}
			this._pageContextOfThisPage.PageRect = new PTS.FSRECT(fssubpagedetails.u.complex.fsrc);
			if (fssubpagedetails.u.complex.cBasicColumns != 0)
			{
				PTS.FSTRACKDESCRIPTION[] array;
				PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					base.Paragraph.StructuralCache.CurrentArrangeContext.PushNewPageData(this._pageContextOfThisPage, array[i].fsrc, base.Paragraph.StructuralCache.CurrentArrangeContext.FinitePage);
					PtsHelper.ArrangeTrack(base.PtsContext, ref array[i], fssubpagedetails.u.complex.fswdir);
					base.Paragraph.StructuralCache.CurrentArrangeContext.PopPageData();
				}
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0010D6A8 File Offset: 0x0010C6A8
		internal override void UpdateViewport(ref PTS.FSRECT viewport)
		{
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			PTS.FSRECT fsrect = default(PTS.FSRECT);
			fsrect.u = viewport.u - this.ContentRect.u;
			fsrect.v = viewport.v - this.ContentRect.v;
			fsrect.du = viewport.du;
			fsrect.dv = viewport.dv;
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				PtsHelper.UpdateViewportTrack(base.PtsContext, ref fssubpagedetails.u.simple.trackdescr, ref fsrect);
				return;
			}
			if (fssubpagedetails.u.complex.cBasicColumns != 0)
			{
				PTS.FSTRACKDESCRIPTION[] array;
				PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
				if (array.Length != 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						PtsHelper.UpdateViewportTrack(base.PtsContext, ref array[i], ref fsrect);
					}
				}
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0010D7B0 File Offset: 0x0010C7B0
		internal void ArrangeFigure(PTS.FSRECT rcFigure, PTS.FSRECT rcHostPara, uint fswdirParent, PageContext pageContext)
		{
			this._rect = rcFigure;
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			this._rect.v = this._rect.v + mbpInfo.MarginTop;
			this._rect.dv = this._rect.dv - (mbpInfo.MarginTop + mbpInfo.MarginBottom);
			this._rect.u = this._rect.u + mbpInfo.MarginLeft;
			this._rect.du = this._rect.du - (mbpInfo.MarginLeft + mbpInfo.MarginRight);
			this._pageContext = pageContext;
			this._flowDirectionParent = PTS.FswdirToFlowDirection(fswdirParent);
			this._flowDirection = (FlowDirection)base.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty);
			this.OnArrange();
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0010D888 File Offset: 0x0010C888
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
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
				PTS.FSSUBPAGEDETAILS fssubpagedetails;
				PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
				if (base.Rect.Contains(pt))
				{
					if (this.ContentRect.Contains(pt))
					{
						pt = new PTS.FSPOINT(pt.u - this.ContentRect.u, pt.v - this.ContentRect.v);
						if (PTS.ToBoolean(fssubpagedetails.fSimple))
						{
							inputElement = PtsHelper.InputHitTestTrack(base.PtsContext, pt, ref fssubpagedetails.u.simple.trackdescr);
						}
						else if (fssubpagedetails.u.complex.cBasicColumns != 0)
						{
							PTS.FSTRACKDESCRIPTION[] array;
							PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
							int num2 = 0;
							while (num2 < array.Length && inputElement == null)
							{
								inputElement = PtsHelper.InputHitTestTrack(base.PtsContext, pt, ref array[num2]);
								num2++;
							}
						}
					}
					if (inputElement == null)
					{
						inputElement = (base.Paragraph.Element as IInputElement);
					}
				}
			}
			return inputElement;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0010D9F0 File Offset: 0x0010C9F0
		internal override List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			if (base.Paragraph.Element as ContentElement == e)
			{
				this.GetRectanglesForParagraphElement(out list);
			}
			else
			{
				PTS.FSSUBPAGEDETAILS fssubpagedetails;
				PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
				if (PTS.ToBoolean(fssubpagedetails.fSimple))
				{
					list = PtsHelper.GetRectanglesInTrack(base.PtsContext, e, start, length, ref fssubpagedetails.u.simple.trackdescr);
				}
				else if (fssubpagedetails.u.complex.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
					for (int i = 0; i < array.Length; i++)
					{
						List<Rect> rectanglesInTrack = PtsHelper.GetRectanglesInTrack(base.PtsContext, e, start, length, ref array[i]);
						Invariant.Assert(rectanglesInTrack != null);
						if (rectanglesInTrack.Count != 0)
						{
							list.AddRange(rectanglesInTrack);
						}
					}
				}
				list = PtsHelper.OffsetRectangleList(list, TextDpi.FromTextDpi(this.ContentRect.u), TextDpi.FromTextDpi(this.ContentRect.v));
			}
			Invariant.Assert(list != null);
			return list;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0010DB14 File Offset: 0x0010CB14
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			fskupdInherited = PTS.FSKUPDATE.fskupdNew;
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			this.Visual.DrawBackgroundAndBorder(backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
			if (this._visual.Children.Count != 2)
			{
				this._visual.Children.Clear();
				this._visual.Children.Add(new ContainerVisual());
				this._visual.Children.Add(new ContainerVisual());
			}
			ContainerVisual containerVisual = (ContainerVisual)this._visual.Children[0];
			ContainerVisual containerVisual2 = (ContainerVisual)this._visual.Children[1];
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				PTS.FSKUPDATE fskupdate = fssubpagedetails.u.simple.trackdescr.fsupdinf.fskupd;
				if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
				{
					fskupdate = fskupdInherited;
				}
				VisualCollection children = containerVisual.Children;
				if (fskupdate == PTS.FSKUPDATE.fskupdNew)
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				else if (children.Count == 1 && !(children[0] is ContainerVisual))
				{
					children.Clear();
					children.Add(new ContainerVisual());
				}
				ContainerVisual containerVisual3 = (ContainerVisual)children[0];
				PtsHelper.UpdateTrackVisuals(base.PtsContext, containerVisual3.Children, fskupdInherited, ref fssubpagedetails.u.simple.trackdescr);
			}
			else
			{
				bool flag = fssubpagedetails.u.complex.cBasicColumns == 0;
				if (!flag)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
					flag = (array.Length == 0);
					if (!flag)
					{
						PTS.FSKUPDATE fskupdate2 = fskupdInherited;
						ErrorHandler.Assert(fskupdate2 != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
						VisualCollection children2 = containerVisual.Children;
						if (children2.Count == 0)
						{
							children2.Add(new SectionVisual());
						}
						else if (!(children2[0] is SectionVisual))
						{
							children2.Clear();
							children2.Add(new SectionVisual());
						}
						SectionVisual sectionVisual = (SectionVisual)children2[0];
						ColumnPropertiesGroup columnProperties = new ColumnPropertiesGroup(base.Paragraph.Element);
						sectionVisual.DrawColumnRules(ref array, TextDpi.FromTextDpi(fssubpagedetails.u.complex.fsrc.v), TextDpi.FromTextDpi(fssubpagedetails.u.complex.fsrc.dv), columnProperties);
						children2 = sectionVisual.Children;
						if (fskupdate2 == PTS.FSKUPDATE.fskupdNew)
						{
							children2.Clear();
							for (int i = 0; i < array.Length; i++)
							{
								children2.Add(new ContainerVisual());
							}
						}
						ErrorHandler.Assert(children2.Count == array.Length, ErrorHandler.ColumnVisualCountMismatch);
						for (int j = 0; j < array.Length; j++)
						{
							ContainerVisual containerVisual4 = (ContainerVisual)children2[j];
							PtsHelper.UpdateTrackVisuals(base.PtsContext, containerVisual4.Children, fskupdInherited, ref array[j]);
						}
					}
				}
				if (flag)
				{
					this._visual.Children.Clear();
				}
			}
			containerVisual.Offset = new PTS.FSVECTOR(this.ContentRect.u, this.ContentRect.v).FromTextDpi();
			containerVisual2.Offset = new PTS.FSVECTOR(this.ContentRect.u, this.ContentRect.v).FromTextDpi();
			PTS.FSRECT fsrect = new PTS.FSRECT(this._paddingRect.u - this._contentRect.u, this._paddingRect.v - this._contentRect.v, this._paddingRect.du, this._paddingRect.dv);
			PtsHelper.ClipChildrenToRect(this._visual, fsrect.FromTextDpi());
			PtsHelper.UpdateFloatingElementVisuals(containerVisual2, this._pageContextOfThisPage.FloatingElementList);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0010DF59 File Offset: 0x0010CF59
		internal override ParagraphResult CreateParagraphResult()
		{
			return new FigureParagraphResult(this);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0010DF64 File Offset: 0x0010CF64
		internal override TextContentRange GetTextContentRange()
		{
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			TextContentRange textContentRange;
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				textContentRange = PtsHelper.TextContentRangeFromTrack(base.PtsContext, fssubpagedetails.u.simple.trackdescr.pfstrack);
			}
			else
			{
				textContentRange = new TextContentRange();
				if (fssubpagedetails.u.complex.cBasicColumns != 0)
				{
					PTS.FSTRACKDESCRIPTION[] array;
					PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
					Invariant.Assert(array.Length == 1);
					for (int i = 0; i < array.Length; i++)
					{
						textContentRange.Merge(PtsHelper.TextContentRangeFromTrack(base.PtsContext, array[i].pfstrack));
					}
				}
			}
			if (this.IsFirstChunk)
			{
				textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(base.Paragraph.Element as TextElement, ElementEdge.BeforeStart));
			}
			if (this.IsLastChunk)
			{
				textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(base.Paragraph.Element as TextElement, ElementEdge.AfterEnd));
			}
			return textContentRange;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0010E074 File Offset: 0x0010D074
		private ReadOnlyCollection<ParagraphResult> GetChildrenParagraphResults(out bool hasTextContent)
		{
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			hasTextContent = false;
			List<ParagraphResult> list;
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, fssubpagedetails.u.simple.trackdescr.pfstrack, out fstrackdetails));
				if (fstrackdetails.cParas == 0)
				{
					return new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
				}
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromTrack(base.PtsContext, fssubpagedetails.u.simple.trackdescr.pfstrack, ref fstrackdetails, out array);
				list = new List<ParagraphResult>(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = base.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					ParagraphResult paragraphResult = baseParaClient.CreateParagraphResult();
					if (paragraphResult.HasTextContent)
					{
						hasTextContent = true;
					}
					list.Add(paragraphResult);
				}
			}
			else
			{
				if (fssubpagedetails.u.complex.cBasicColumns == 0)
				{
					return new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
				}
				PTS.FSTRACKDESCRIPTION[] array2;
				PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array2);
				PTS.FSTRACKDETAILS fstrackdetails2;
				PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, array2[0].pfstrack, out fstrackdetails2));
				if (fstrackdetails2.cParas == 0)
				{
					return new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
				}
				PTS.FSPARADESCRIPTION[] array3;
				PtsHelper.ParaListFromTrack(base.PtsContext, array2[0].pfstrack, ref fstrackdetails2, out array3);
				list = new List<ParagraphResult>(array3.Length);
				for (int j = 0; j < array3.Length; j++)
				{
					BaseParaClient baseParaClient2 = base.PtsContext.HandleToObject(array3[j].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient2);
					ParagraphResult paragraphResult2 = baseParaClient2.CreateParagraphResult();
					if (paragraphResult2.HasTextContent)
					{
						hasTextContent = true;
					}
					list.Add(paragraphResult2);
				}
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0010E268 File Offset: 0x0010D268
		internal ReadOnlyCollection<ColumnResult> GetColumnResults(out bool hasTextContent)
		{
			List<ColumnResult> list = new List<ColumnResult>(0);
			Vector contentOffset = default(Vector);
			hasTextContent = false;
			PTS.FSSUBPAGEDETAILS fssubpagedetails;
			PTS.Validate(PTS.FsQuerySubpageDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubpagedetails));
			if (PTS.ToBoolean(fssubpagedetails.fSimple))
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, fssubpagedetails.u.simple.trackdescr.pfstrack, out fstrackdetails));
				if (fstrackdetails.cParas > 0)
				{
					list = new List<ColumnResult>(1);
					ColumnResult columnResult = new ColumnResult(this, ref fssubpagedetails.u.simple.trackdescr, contentOffset);
					list.Add(columnResult);
					if (columnResult.HasTextContent)
					{
						hasTextContent = true;
					}
				}
			}
			else if (fssubpagedetails.u.complex.cBasicColumns != 0)
			{
				PTS.FSTRACKDESCRIPTION[] array;
				PtsHelper.TrackListFromSubpage(base.PtsContext, this._paraHandle.Value, ref fssubpagedetails, out array);
				list = new List<ColumnResult>(1);
				PTS.FSTRACKDETAILS fstrackdetails2;
				PTS.Validate(PTS.FsQueryTrackDetails(base.PtsContext.Context, array[0].pfstrack, out fstrackdetails2));
				if (fstrackdetails2.cParas > 0)
				{
					ColumnResult columnResult2 = new ColumnResult(this, ref array[0], contentOffset);
					list.Add(columnResult2);
					if (columnResult2.HasTextContent)
					{
						hasTextContent = true;
					}
				}
			}
			return new ReadOnlyCollection<ColumnResult>(list);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0010E3B7 File Offset: 0x0010D3B7
		internal ReadOnlyCollection<ParagraphResult> GetParagraphResultsFromColumn(IntPtr pfstrack, Vector parentOffset, out bool hasTextContent)
		{
			return this.GetChildrenParagraphResults(out hasTextContent);
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0010E3C0 File Offset: 0x0010D3C0
		internal TextContentRange GetTextContentRangeFromColumn(IntPtr pfstrack)
		{
			return this.GetTextContentRange();
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0010E3C8 File Offset: 0x0010D3C8
		internal Geometry GetTightBoundingGeometryFromTextPositions(ReadOnlyCollection<ColumnResult> columns, ReadOnlyCollection<ParagraphResult> floatingElements, ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			Geometry result = null;
			Invariant.Assert(columns != null && columns.Count <= 1, "Columns collection is null.");
			Invariant.Assert(floatingElements != null, "Floating element collection is null.");
			ReadOnlyCollection<ParagraphResult> readOnlyCollection = (columns.Count > 0) ? columns[0].Paragraphs : new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
			if (readOnlyCollection.Count > 0 || floatingElements.Count > 0)
			{
				result = TextDocumentView.GetTightBoundingGeometryFromTextPositionsHelper(readOnlyCollection, floatingElements, startPosition, endPosition, TextDpi.FromTextDpi(this._dvrTopSpace), visibleRect);
				Rect viewport = new Rect(0.0, 0.0, TextDpi.FromTextDpi(this._contentRect.du), TextDpi.FromTextDpi(this._contentRect.dv));
				CaretElement.ClipGeometryByViewport(ref result, viewport);
			}
			return result;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0010E490 File Offset: 0x0010D490
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0010E49D File Offset: 0x0010D49D
		internal IntPtr SubpageHandle
		{
			get
			{
				return this._paraHandle.Value;
			}
			set
			{
				this._paraHandle.Value = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0010E4AB File Offset: 0x0010D4AB
		internal PTS.FSRECT ContentRect
		{
			get
			{
				return this._contentRect;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0010E4B4 File Offset: 0x0010D4B4
		internal ReadOnlyCollection<ParagraphResult> FloatingElementResults
		{
			get
			{
				List<ParagraphResult> list = new List<ParagraphResult>(0);
				List<BaseParaClient> floatingElementList = this._pageContextOfThisPage.FloatingElementList;
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

		// Token: 0x0400077F RID: 1919
		private PTS.FSRECT _contentRect;

		// Token: 0x04000780 RID: 1920
		private PTS.FSRECT _paddingRect;

		// Token: 0x04000781 RID: 1921
		private PageContext _pageContextOfThisPage = new PageContext();
	}
}
