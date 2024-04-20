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
	// Token: 0x02000112 RID: 274
	internal class ContainerParaClient : BaseParaClient
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0010A829 File Offset: 0x00109829
		internal ContainerParaClient(ContainerParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0010A834 File Offset: 0x00109834
		protected override void OnArrange()
		{
			base.OnArrange();
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ParentFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorMargin();
			}
			this._rect.u = this._rect.u + mbpInfo.MarginLeft;
			this._rect.du = this._rect.du - (mbpInfo.MarginLeft + mbpInfo.MarginRight);
			this._rect.du = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.du);
			this._rect.dv = Math.Max(TextDpi.ToTextDpi(TextDpi.MinWidth), this._rect.dv);
			uint fswdirTrack = PTS.FlowDirectionToFswdir(this._flowDirection);
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				PtsHelper.ArrangeParaList(base.PtsContext, fssubtrackdetails.fsrc, arrayParaDesc, fswdirTrack);
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0010A960 File Offset: 0x00109960
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			IInputElement inputElement = null;
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				inputElement = PtsHelper.InputHitTestParaList(base.PtsContext, pt, ref fssubtrackdetails.fsrc, arrayParaDesc);
			}
			if (inputElement == null && this._rect.Contains(pt))
			{
				inputElement = (base.Paragraph.Element as IInputElement);
			}
			return inputElement;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0010A9EC File Offset: 0x001099EC
		internal override List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			if (base.Paragraph.Element as ContentElement == e)
			{
				this.GetRectanglesForParagraphElement(out list);
			}
			else
			{
				PTS.FSSUBTRACKDETAILS fssubtrackdetails;
				PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
				if (fssubtrackdetails.cParas != 0)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
					list = PtsHelper.GetRectanglesInParaList(base.PtsContext, e, start, length, arrayParaDesc);
				}
				else
				{
					list = new List<Rect>();
				}
			}
			Invariant.Assert(list != null);
			return list;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0010AA84 File Offset: 0x00109A84
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			this._visual.DrawBackgroundAndBorder(backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				PtsHelper.UpdateParaListVisuals(base.PtsContext, this._visual.Children, fskupdInherited, arrayParaDesc);
				return;
			}
			this._visual.Children.Clear();
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0010AB84 File Offset: 0x00109B84
		internal override void UpdateViewport(ref PTS.FSRECT viewport)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out arrayParaDesc);
				PtsHelper.UpdateViewportParaList(base.PtsContext, arrayParaDesc, ref viewport);
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0010ABE2 File Offset: 0x00109BE2
		internal override ParagraphResult CreateParagraphResult()
		{
			return new ContainerParagraphResult(this);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0010ABEC File Offset: 0x00109BEC
		internal override TextContentRange GetTextContentRange()
		{
			TextElement textElement = base.Paragraph.Element as TextElement;
			Invariant.Assert(textElement != null, "Expecting TextElement as owner of ContainerParagraph.");
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			TextContentRange textContentRange;
			if (fssubtrackdetails.cParas == 0 || (this._isFirstChunk && this._isLastChunk))
			{
				textContentRange = TextContainerHelper.GetTextContentRangeForTextElement(textElement);
			}
			else
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
				textContentRange = new TextContentRange();
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = base.Paragraph.StructuralCache.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					textContentRange.Merge(baseParaClient.GetTextContentRange());
				}
				if (this._isFirstChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.BeforeStart));
				}
				if (this._isLastChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.AfterEnd));
				}
			}
			Invariant.Assert(textContentRange != null);
			return textContentRange;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0010AD00 File Offset: 0x00109D00
		internal ReadOnlyCollection<ParagraphResult> GetChildrenParagraphResults(out bool hasTextContent)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			hasTextContent = false;
			if (fssubtrackdetails.cParas == 0)
			{
				return new ReadOnlyCollection<ParagraphResult>(new List<ParagraphResult>(0));
			}
			PTS.FSPARADESCRIPTION[] array;
			PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
			List<ParagraphResult> list = new List<ParagraphResult>(array.Length);
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
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0010ADBA File Offset: 0x00109DBA
		internal void SetChunkInfo(bool isFirstChunk, bool isLastChunk)
		{
			this._isFirstChunk = isFirstChunk;
			this._isLastChunk = isLastChunk;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0010ADCC File Offset: 0x00109DCC
		internal override int GetFirstTextLineBaseline()
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			if (fssubtrackdetails.cParas == 0)
			{
				return this._rect.v;
			}
			PTS.FSPARADESCRIPTION[] array;
			PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
			BaseParaClient baseParaClient = base.PtsContext.HandleToObject(array[0].pfsparaclient) as BaseParaClient;
			PTS.ValidateHandle(baseParaClient);
			return baseParaClient.GetFirstTextLineBaseline();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0010AE50 File Offset: 0x00109E50
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			bool flag;
			ReadOnlyCollection<ParagraphResult> childrenParagraphResults = this.GetChildrenParagraphResults(out flag);
			Invariant.Assert(childrenParagraphResults != null, "Paragraph collection is null.");
			if (childrenParagraphResults.Count > 0)
			{
				return TextDocumentView.GetTightBoundingGeometryFromTextPositionsHelper(childrenParagraphResults, startPosition, endPosition, TextDpi.FromTextDpi(this._dvrTopSpace), visibleRect);
			}
			return null;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0010AE93 File Offset: 0x00109E93
		internal override bool IsFirstChunk
		{
			get
			{
				return this._isFirstChunk;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x0010AE9B File Offset: 0x00109E9B
		internal override bool IsLastChunk
		{
			get
			{
				return this._isLastChunk;
			}
		}

		// Token: 0x04000739 RID: 1849
		private bool _isFirstChunk;

		// Token: 0x0400073A RID: 1850
		private bool _isLastChunk;
	}
}
