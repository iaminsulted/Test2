using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020001D4 RID: 468
	internal class MultiPageTextView : TextViewBase
	{
		// Token: 0x0600107C RID: 4220 RVA: 0x0013FA41 File Offset: 0x0013EA41
		internal MultiPageTextView(DocumentViewerBase viewer, UIElement renderScope, ITextContainer textContainer)
		{
			this._viewer = viewer;
			this._renderScope = renderScope;
			this._textContainer = textContainer;
			this._pageTextViews = new List<DocumentPageTextView>();
			this.OnPagesUpdatedCore();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0013FA6F File Offset: 0x0013EA6F
		protected override void OnUpdated(EventArgs e)
		{
			base.OnUpdated(e);
			if (this.IsValid)
			{
				this.OnUpdatedWorker(null);
				return;
			}
			this._renderScope.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnUpdatedWorker), EventArgs.Empty);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0013FAB0 File Offset: 0x0013EAB0
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			ITextPointer result = null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPoint = this.GetTextViewFromPoint(point, false);
			if (textViewFromPoint != null)
			{
				point = this.TransformToDescendant(textViewFromPoint.RenderScope, point);
				result = textViewFromPoint.GetTextPositionFromPoint(point, snapToText);
			}
			return result;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0013FAFC File Offset: 0x0013EAFC
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			Rect result = Rect.Empty;
			transform = Transform.Identity;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				Transform firstTransform;
				result = textViewFromPosition.GetRawRectangleFromTextPosition(position, out firstTransform);
				Transform transformToAncestor = this.GetTransformToAncestor(textViewFromPosition.RenderScope);
				transform = this.GetAggregateTransform(firstTransform, transformToAncestor);
			}
			return result;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0013FB5C File Offset: 0x0013EB5C
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			Geometry result = null;
			int i = 0;
			int count = this._pageTextViews.Count;
			while (i < count)
			{
				ReadOnlyCollection<TextSegment> textSegments = this._pageTextViews[i].TextSegments;
				for (int j = 0; j < textSegments.Count; j++)
				{
					TextSegment textSegment = textSegments[j];
					ITextPointer textPointer = (startPosition.CompareTo(textSegment.Start) > 0) ? startPosition : textSegment.Start;
					ITextPointer textPointer2 = (endPosition.CompareTo(textSegment.End) < 0) ? endPosition : textSegment.End;
					if (textPointer.CompareTo(textPointer2) < 0)
					{
						Geometry tightBoundingGeometryFromTextPositions = this._pageTextViews[i].GetTightBoundingGeometryFromTextPositions(textPointer, textPointer2);
						if (tightBoundingGeometryFromTextPositions != null)
						{
							Transform affineTransform = this._pageTextViews[i].RenderScope.TransformToAncestor(this._renderScope).AffineTransform;
							CaretElement.AddTransformToGeometry(tightBoundingGeometryFromTextPositions, affineTransform);
							CaretElement.AddGeometry(ref result, tightBoundingGeometryFromTextPositions);
						}
					}
				}
				i++;
			}
			return result;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0013FC70 File Offset: 0x0013EC70
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			int num;
			return this.GetPositionAtNextLineCore(position, suggestedX, count, out newSuggestedX, out linesMoved, out num);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0013FCA4 File Offset: 0x0013ECA4
		internal override ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			int num;
			return this.GetPositionAtNextPageCore(position, suggestedOffset, count, out newSuggestedOffset, out pagesMoved, out num);
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0013FCD8 File Offset: 0x0013ECD8
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			bool result = false;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.IsAtCaretUnitBoundary(position);
			}
			return result;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0013FD14 File Offset: 0x0013ED14
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			ITextPointer result = null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.GetNextCaretUnitPosition(position, direction);
			}
			return result;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0013FD50 File Offset: 0x0013ED50
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			ITextPointer result = null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.GetBackspaceCaretUnitPosition(position);
			}
			return result;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0013FD8C File Offset: 0x0013ED8C
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			TextSegment result = TextSegment.Null;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				result = textViewFromPosition.GetLineRange(position);
			}
			return result;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0013FDCB File Offset: 0x0013EDCB
		internal override bool Contains(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return this.GetTextViewFromPosition(position) != null;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0013FDF0 File Offset: 0x0013EDF0
		internal override void BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(position, false, null, false, userState));
			}
			MultiPageTextView.BringPositionIntoViewRequest bringPositionIntoViewRequest = new MultiPageTextView.BringPositionIntoViewRequest(position, userState);
			this._pendingRequest = bringPositionIntoViewRequest;
			if (this.GetTextViewFromPosition(position) != null)
			{
				bringPositionIntoViewRequest.Succeeded = true;
				this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				return;
			}
			if (!(position is ContentPosition))
			{
				this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				return;
			}
			DynamicDocumentPaginator dynamicDocumentPaginator = this._viewer.Document.DocumentPaginator as DynamicDocumentPaginator;
			if (dynamicDocumentPaginator == null)
			{
				this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				return;
			}
			int pageNumber = dynamicDocumentPaginator.GetPageNumber((ContentPosition)position) + 1;
			if (this._viewer.CanGoToPage(pageNumber))
			{
				this._viewer.GoToPage(pageNumber);
				return;
			}
			this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0013FEB8 File Offset: 0x0013EEB8
		internal override void BringPointIntoViewAsync(Point point, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(point, null, false, null, false, userState));
				return;
			}
			MultiPageTextView.BringPointIntoViewRequest bringPointIntoViewRequest = new MultiPageTextView.BringPointIntoViewRequest(point, userState);
			this._pendingRequest = bringPointIntoViewRequest;
			DocumentPageTextView textViewFromPoint = this.GetTextViewFromPoint(point, false);
			if (textViewFromPoint != null)
			{
				point = this.TransformToDescendant(textViewFromPoint.RenderScope, point);
				ITextPointer textPositionFromPoint = textViewFromPoint.GetTextPositionFromPoint(point, true);
				bringPointIntoViewRequest.Position = textPositionFromPoint;
				this.OnBringPointIntoViewCompleted(bringPointIntoViewRequest);
				return;
			}
			this._renderScope.TransformToAncestor(this._viewer).TryTransform(point, out point);
			bool flag = false;
			if (this._viewer is FlowDocumentPageViewer)
			{
				flag = ((FlowDocumentPageViewer)this._viewer).BringPointIntoView(point);
			}
			else if (this._viewer is DocumentViewer)
			{
				flag = ((DocumentViewer)this._viewer).BringPointIntoView(point);
			}
			else if (DoubleUtil.LessThan(point.X, 0.0))
			{
				if (this._viewer.CanGoToPreviousPage)
				{
					this._viewer.PreviousPage();
					flag = true;
				}
			}
			else if (DoubleUtil.GreaterThan(point.X, this._viewer.RenderSize.Width))
			{
				if (this._viewer.CanGoToNextPage)
				{
					this._viewer.NextPage();
					flag = true;
				}
			}
			else if (DoubleUtil.LessThan(point.Y, 0.0))
			{
				if (this._viewer.CanGoToPreviousPage)
				{
					this._viewer.PreviousPage();
					flag = true;
				}
			}
			else if (DoubleUtil.GreaterThan(point.Y, this._viewer.RenderSize.Height) && this._viewer.CanGoToNextPage)
			{
				this._viewer.NextPage();
				flag = true;
			}
			if (!flag)
			{
				this.OnBringPointIntoViewCompleted(bringPointIntoViewRequest);
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00140090 File Offset: 0x0013F090
		internal override void BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(position, suggestedX, count, position, suggestedX, 0, false, null, false, userState));
				return;
			}
			this._pendingRequest = new MultiPageTextView.BringLineIntoViewRequest(position, suggestedX, count, userState);
			this.BringLineIntoViewCore((MultiPageTextView.BringLineIntoViewRequest)this._pendingRequest);
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x001400F8 File Offset: 0x0013F0F8
		internal override void BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this._pendingRequest != null)
			{
				this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(position, suggestedOffset, count, position, suggestedOffset, 0, false, null, false, userState));
				return;
			}
			this._pendingRequest = new MultiPageTextView.BringPageIntoViewRequest(position, suggestedOffset, count, userState);
			this.BringPageIntoViewCore((MultiPageTextView.BringPageIntoViewRequest)this._pendingRequest);
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00140160 File Offset: 0x0013F160
		internal override void CancelAsync(object userState)
		{
			if (this._pendingRequest != null)
			{
				if (this._pendingRequest is MultiPageTextView.BringLineIntoViewRequest)
				{
					MultiPageTextView.BringLineIntoViewRequest bringLineIntoViewRequest = (MultiPageTextView.BringLineIntoViewRequest)this._pendingRequest;
					this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(bringLineIntoViewRequest.Position, bringLineIntoViewRequest.SuggestedX, bringLineIntoViewRequest.Count, bringLineIntoViewRequest.NewPosition, bringLineIntoViewRequest.NewSuggestedX, bringLineIntoViewRequest.Count - bringLineIntoViewRequest.NewCount, false, null, true, bringLineIntoViewRequest.UserState));
				}
				else if (this._pendingRequest is MultiPageTextView.BringPageIntoViewRequest)
				{
					MultiPageTextView.BringPageIntoViewRequest bringPageIntoViewRequest = (MultiPageTextView.BringPageIntoViewRequest)this._pendingRequest;
					this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(bringPageIntoViewRequest.Position, bringPageIntoViewRequest.SuggestedOffset, bringPageIntoViewRequest.Count, bringPageIntoViewRequest.NewPosition, bringPageIntoViewRequest.NewSuggestedOffset, bringPageIntoViewRequest.Count - bringPageIntoViewRequest.NewCount, false, null, true, bringPageIntoViewRequest.UserState));
				}
				else if (this._pendingRequest is MultiPageTextView.BringPointIntoViewRequest)
				{
					MultiPageTextView.BringPointIntoViewRequest bringPointIntoViewRequest = (MultiPageTextView.BringPointIntoViewRequest)this._pendingRequest;
					this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(bringPointIntoViewRequest.Point, bringPointIntoViewRequest.Position, false, null, true, bringPointIntoViewRequest.UserState));
				}
				else if (this._pendingRequest is MultiPageTextView.BringPositionIntoViewRequest)
				{
					MultiPageTextView.BringPositionIntoViewRequest bringPositionIntoViewRequest = (MultiPageTextView.BringPositionIntoViewRequest)this._pendingRequest;
					this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(bringPositionIntoViewRequest.Position, false, null, true, bringPositionIntoViewRequest.UserState));
				}
				this._pendingRequest = null;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x001402A4 File Offset: 0x0013F2A4
		internal void OnPagesUpdated()
		{
			this.OnPagesUpdatedCore();
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0013CA62 File Offset: 0x0013BA62
		internal void OnPageLayoutChanged()
		{
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x001402BF File Offset: 0x0013F2BF
		internal ITextView GetPageTextViewFromPosition(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return this.GetTextViewFromPosition(position);
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x001402E0 File Offset: 0x0013F2E0
		internal override UIElement RenderScope
		{
			get
			{
				return this._renderScope;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001091 RID: 4241 RVA: 0x001402E8 File Offset: 0x0013F2E8
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x001402F0 File Offset: 0x0013F2F0
		internal override bool IsValid
		{
			get
			{
				bool result = false;
				if (this._pageTextViews != null)
				{
					result = true;
					for (int i = 0; i < this._pageTextViews.Count; i++)
					{
						if (!this._pageTextViews[i].IsValid)
						{
							result = false;
							break;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x00140337 File Offset: 0x0013F337
		internal override bool RendersOwnSelection
		{
			get
			{
				return this._pageTextViews != null && this._pageTextViews.Count > 0 && this._pageTextViews[0].RendersOwnSelection;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00140364 File Offset: 0x0013F364
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				List<TextSegment> list = new List<TextSegment>();
				if (this.IsValid)
				{
					for (int i = 0; i < this._pageTextViews.Count; i++)
					{
						list.AddRange(this._pageTextViews[i].TextSegments);
					}
				}
				return new ReadOnlyCollection<TextSegment>(list);
			}
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x001403B4 File Offset: 0x0013F3B4
		private void OnPagesUpdatedCore()
		{
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				this._pageTextViews[i].Updated -= this.HandlePageTextViewUpdated;
			}
			this._pageTextViews.Clear();
			ReadOnlyCollection<DocumentPageView> pageViews = this._viewer.PageViews;
			if (pageViews != null)
			{
				for (int i = 0; i < pageViews.Count; i++)
				{
					DocumentPageTextView documentPageTextView = ((IServiceProvider)pageViews[i]).GetService(typeof(ITextView)) as DocumentPageTextView;
					if (documentPageTextView != null)
					{
						this._pageTextViews.Add(documentPageTextView);
						documentPageTextView.Updated += this.HandlePageTextViewUpdated;
					}
				}
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0014045C File Offset: 0x0013F45C
		private void HandlePageTextViewUpdated(object sender, EventArgs e)
		{
			this.OnUpdated(EventArgs.Empty);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0014046C File Offset: 0x0013F46C
		private void BringLineIntoViewCore(MultiPageTextView.BringLineIntoViewRequest request)
		{
			double newSuggestedX;
			int num;
			int num2;
			ITextPointer positionAtNextLineCore = this.GetPositionAtNextLineCore(request.NewPosition, request.NewSuggestedX, request.NewCount, out newSuggestedX, out num, out num2);
			Invariant.Assert(Math.Abs(request.NewCount) >= Math.Abs(num));
			request.NewPosition = positionAtNextLineCore;
			request.NewSuggestedX = newSuggestedX;
			request.NewCount -= num;
			request.NewPageNumber = num2;
			if (request.NewCount == 0)
			{
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
			if (positionAtNextLineCore is DocumentSequenceTextPointer || positionAtNextLineCore is FixedTextPointer)
			{
				if (this._viewer.CanGoToPage(num2 + 1))
				{
					this._viewer.GoToPage(num2 + 1);
					return;
				}
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
			else if (request.NewCount > 0)
			{
				if (this._viewer.CanGoToNextPage)
				{
					this._viewer.NextPage();
					return;
				}
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
			else
			{
				if (this._viewer.CanGoToPreviousPage)
				{
					this._viewer.PreviousPage();
					return;
				}
				this.OnBringLineIntoViewCompleted(request);
				return;
			}
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00140568 File Offset: 0x0013F568
		private void BringPageIntoViewCore(MultiPageTextView.BringPageIntoViewRequest request)
		{
			Point newSuggestedOffset;
			int num;
			int num2;
			ITextPointer positionAtNextPageCore = this.GetPositionAtNextPageCore(request.NewPosition, request.NewSuggestedOffset, request.NewCount, out newSuggestedOffset, out num, out num2);
			Invariant.Assert(Math.Abs(request.NewCount) >= Math.Abs(num));
			request.NewPosition = positionAtNextPageCore;
			request.NewSuggestedOffset = newSuggestedOffset;
			request.NewCount -= num;
			if (request.NewCount == 0 || num2 == -1)
			{
				this.OnBringPageIntoViewCompleted(request);
				return;
			}
			num2 += ((request.NewCount > 0) ? 1 : -1);
			if (this._viewer.CanGoToPage(num2 + 1))
			{
				request.NewPageNumber = num2;
				this._viewer.GoToPage(num2 + 1);
				return;
			}
			this.OnBringPageIntoViewCompleted(request);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0014061C File Offset: 0x0013F61C
		private ITextPointer GetPositionAtNextLineCore(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved, out int pageNumber)
		{
			DocumentPageTextView documentPageTextView = this.GetTextViewFromPosition(position);
			ITextPointer result;
			if (documentPageTextView != null)
			{
				int num = count;
				suggestedX = this.TransformToDescendant(documentPageTextView.RenderScope, new Point(suggestedX, 0.0)).X;
				result = documentPageTextView.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
				pageNumber = ((DocumentPageView)documentPageTextView.RenderScope).PageNumber;
				newSuggestedX = this.TransformToAncestor(documentPageTextView.RenderScope, new Point(newSuggestedX, 0.0)).X;
				while (num != linesMoved)
				{
					int num2 = 0;
					count = num - linesMoved;
					pageNumber += ((count > 0) ? 1 : -1);
					documentPageTextView = this.GetTextViewFromPageNumber(pageNumber);
					if (documentPageTextView == null)
					{
						break;
					}
					ReadOnlyCollection<TextSegment> textSegments = documentPageTextView.TextSegments;
					int num3 = count;
					if (count > 0)
					{
						position = documentPageTextView.GetTextPositionFromPoint(new Point(suggestedX, 0.0), true);
						if (position != null)
						{
							count--;
							linesMoved++;
						}
					}
					else
					{
						position = documentPageTextView.GetTextPositionFromPoint(new Point(suggestedX, documentPageTextView.RenderScope.RenderSize.Height), true);
						if (position != null)
						{
							count++;
							linesMoved--;
						}
					}
					if (position != null)
					{
						if (count == 0)
						{
							result = this.GetPositionAtPageBoundary(num3 > 0, documentPageTextView, position, suggestedX);
							newSuggestedX = suggestedX;
						}
						else
						{
							result = documentPageTextView.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out num2);
							linesMoved += num2;
						}
						newSuggestedX = this.TransformToAncestor(documentPageTextView.RenderScope, new Point(newSuggestedX, 0.0)).X;
					}
				}
			}
			else
			{
				result = position;
				linesMoved = 0;
				newSuggestedX = suggestedX;
				pageNumber = -1;
			}
			return result;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x001407B4 File Offset: 0x0013F7B4
		private ITextPointer GetPositionAtNextPageCore(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved, out int pageNumber)
		{
			ITextPointer textPointer = position;
			pagesMoved = 0;
			newSuggestedOffset = suggestedOffset;
			pageNumber = -1;
			DocumentPageTextView textViewFromPosition = this.GetTextViewFromPosition(position);
			if (textViewFromPosition != null)
			{
				int pageNumber2 = ((DocumentPageView)textViewFromPosition.RenderScope).PageNumber;
				DocumentPageTextView textViewForNextPage = this.GetTextViewForNextPage(pageNumber2, count, out pageNumber);
				pagesMoved = pageNumber - pageNumber2;
				Invariant.Assert(Math.Abs(pagesMoved) <= Math.Abs(count));
				if (pageNumber != pageNumber2 && textViewForNextPage != null)
				{
					Point point = this.TransformToDescendant(textViewFromPosition.RenderScope, suggestedOffset);
					textPointer = textViewForNextPage.GetTextPositionFromPoint(point, true);
					if (textPointer != null)
					{
						Rect rectangleFromTextPosition = textViewForNextPage.GetRectangleFromTextPosition(textPointer);
						point = this.TransformToAncestor(textViewFromPosition.RenderScope, new Point(rectangleFromTextPosition.X, rectangleFromTextPosition.Y));
						newSuggestedOffset = point;
					}
					else
					{
						textPointer = position;
						pagesMoved = 0;
						pageNumber = pageNumber2;
					}
				}
				else
				{
					pagesMoved = 0;
					pageNumber = pageNumber2;
				}
			}
			return textPointer;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0014088C File Offset: 0x0013F88C
		private ITextPointer GetPositionAtPageBoundary(bool pageTop, ITextView pageTextView, ITextPointer position, double suggestedX)
		{
			ITextPointer textPointer;
			if (pageTop)
			{
				double suggestedX2;
				int num;
				textPointer = pageTextView.GetPositionAtNextLine(position, suggestedX, 1, out suggestedX2, out num);
				if (num == 1)
				{
					textPointer = pageTextView.GetPositionAtNextLine(textPointer, suggestedX2, -1, out suggestedX2, out num);
				}
				else
				{
					textPointer = position;
				}
			}
			else
			{
				double suggestedX2;
				int num;
				textPointer = pageTextView.GetPositionAtNextLine(position, suggestedX, -1, out suggestedX2, out num);
				if (num == -1)
				{
					textPointer = pageTextView.GetPositionAtNextLine(textPointer, suggestedX2, 1, out suggestedX2, out num);
				}
				else
				{
					textPointer = position;
				}
			}
			return textPointer;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x001408EC File Offset: 0x0013F8EC
		private DocumentPageTextView GetTextViewFromPoint(Point point, bool snap)
		{
			DocumentPageTextView documentPageTextView = null;
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this.TransformToAncestor(this._pageTextViews[i].RenderScope, new Rect(this._pageTextViews[i].RenderScope.RenderSize)).Contains(point))
				{
					documentPageTextView = this._pageTextViews[i];
					break;
				}
			}
			if (documentPageTextView == null && snap)
			{
				double[] array = new double[this._pageTextViews.Count];
				for (int i = 0; i < this._pageTextViews.Count; i++)
				{
					Rect rect = this.TransformToAncestor(this._pageTextViews[i].RenderScope, new Rect(this._pageTextViews[i].RenderScope.RenderSize));
					double x;
					if (point.X >= rect.Left && point.X <= rect.Right)
					{
						x = 0.0;
					}
					else
					{
						x = Math.Min(Math.Abs(point.X - rect.Left), Math.Abs(point.X - rect.Right));
					}
					double x2;
					if (point.Y >= rect.Top && point.Y <= rect.Bottom)
					{
						x2 = 0.0;
					}
					else
					{
						x2 = Math.Min(Math.Abs(point.Y - rect.Top), Math.Abs(point.Y - rect.Bottom));
					}
					array[i] = Math.Sqrt(Math.Pow(x, 2.0) + Math.Pow(x2, 2.0));
				}
				double num = double.MaxValue;
				for (int i = 0; i < array.Length; i++)
				{
					if (num > array[i])
					{
						num = array[i];
						documentPageTextView = this._pageTextViews[i];
					}
				}
			}
			return documentPageTextView;
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x00140ADC File Offset: 0x0013FADC
		private DocumentPageTextView GetTextViewFromPosition(ITextPointer position)
		{
			DocumentPageTextView result = null;
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this._pageTextViews[i].Contains(position))
				{
					result = this._pageTextViews[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x00140B28 File Offset: 0x0013FB28
		private DocumentPageTextView GetTextViewFromPageNumber(int pageNumber)
		{
			DocumentPageTextView result = null;
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this._pageTextViews[i].DocumentPageView.PageNumber == pageNumber)
				{
					result = this._pageTextViews[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00140B78 File Offset: 0x0013FB78
		private DocumentPageTextView GetTextViewForNextPage(int pageNumber, int count, out int newPageNumber)
		{
			Invariant.Assert(count != 0);
			newPageNumber = pageNumber + count;
			int num = newPageNumber;
			DocumentPageTextView documentPageTextView = null;
			int num2 = Math.Abs(count);
			for (int i = 0; i < this._pageTextViews.Count; i++)
			{
				if (this._pageTextViews[i].DocumentPageView.PageNumber == newPageNumber)
				{
					documentPageTextView = this._pageTextViews[i];
					num = newPageNumber;
					break;
				}
				int pageNumber2 = this._pageTextViews[i].DocumentPageView.PageNumber;
				if (count > 0 && pageNumber2 > pageNumber)
				{
					int num3 = pageNumber2 - pageNumber;
					if (num3 < num2)
					{
						num2 = num3;
						documentPageTextView = this._pageTextViews[i];
						num = pageNumber2;
					}
				}
				else if (count < 0 && pageNumber2 < pageNumber)
				{
					int num4 = Math.Abs(pageNumber2 - pageNumber);
					if (num4 < num2)
					{
						num2 = num4;
						documentPageTextView = this._pageTextViews[i];
						num = pageNumber2;
					}
				}
			}
			if (documentPageTextView != null)
			{
				newPageNumber = num;
			}
			else
			{
				newPageNumber = pageNumber;
				documentPageTextView = this.GetTextViewFromPageNumber(pageNumber);
			}
			Invariant.Assert(newPageNumber >= 0);
			return documentPageTextView;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00140C78 File Offset: 0x0013FC78
		private Transform GetTransformToAncestor(Visual innerScope)
		{
			Transform transform = innerScope.TransformToAncestor(this._renderScope) as Transform;
			if (transform == null)
			{
				transform = Transform.Identity;
			}
			return transform;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x00140CA4 File Offset: 0x0013FCA4
		private Rect TransformToAncestor(Visual innerScope, Rect rect)
		{
			if (rect != Rect.Empty)
			{
				GeneralTransform generalTransform = innerScope.TransformToAncestor(this._renderScope);
				if (generalTransform != null)
				{
					rect = generalTransform.TransformBounds(rect);
				}
			}
			return rect;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00140CD8 File Offset: 0x0013FCD8
		private Point TransformToAncestor(Visual innerScope, Point point)
		{
			GeneralTransform generalTransform = innerScope.TransformToAncestor(this._renderScope);
			if (generalTransform != null)
			{
				point = generalTransform.Transform(point);
			}
			return point;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x00140D00 File Offset: 0x0013FD00
		private Point TransformToDescendant(Visual innerScope, Point point)
		{
			GeneralTransform generalTransform = innerScope.TransformToAncestor(this._renderScope);
			if (generalTransform != null)
			{
				generalTransform = generalTransform.Inverse;
				if (generalTransform != null)
				{
					point = generalTransform.Transform(point);
				}
			}
			return point;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x00140D31 File Offset: 0x0013FD31
		private void OnBringPositionIntoViewCompleted(MultiPageTextView.BringPositionIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(request.Position, request.Succeeded, null, false, request.UserState));
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00140D59 File Offset: 0x0013FD59
		private void OnBringPointIntoViewCompleted(MultiPageTextView.BringPointIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(request.Point, request.Position, request.Position != null, null, false, request.UserState));
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x00140D8C File Offset: 0x0013FD8C
		private void OnBringLineIntoViewCompleted(MultiPageTextView.BringLineIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(request.Position, request.SuggestedX, request.Count, request.NewPosition, request.NewSuggestedX, request.Count - request.NewCount, request.NewCount == 0, null, false, request.UserState));
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x00140DE8 File Offset: 0x0013FDE8
		private void OnBringPageIntoViewCompleted(MultiPageTextView.BringPageIntoViewRequest request)
		{
			this._pendingRequest = null;
			this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(request.Position, request.SuggestedOffset, request.Count, request.NewPosition, request.NewSuggestedOffset, request.Count - request.NewCount, request.NewCount == 0, null, false, request.UserState));
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x00140E44 File Offset: 0x0013FE44
		private object OnUpdatedWorker(object o)
		{
			if (this.IsValid && this._pendingRequest != null)
			{
				if (this._pendingRequest is MultiPageTextView.BringLineIntoViewRequest)
				{
					MultiPageTextView.BringLineIntoViewRequest bringLineIntoViewRequest = (MultiPageTextView.BringLineIntoViewRequest)this._pendingRequest;
					ITextView textView = this.GetTextViewFromPageNumber(bringLineIntoViewRequest.NewPageNumber);
					if (textView != null)
					{
						double x = this.TransformToDescendant(textView.RenderScope, new Point(bringLineIntoViewRequest.NewSuggestedX, 0.0)).X;
						ITextPointer textPositionFromPoint;
						if (bringLineIntoViewRequest.Count > 0)
						{
							textPositionFromPoint = textView.GetTextPositionFromPoint(new Point(-1.0, -1.0), true);
							if (textPositionFromPoint != null)
							{
								bringLineIntoViewRequest.NewCount--;
							}
						}
						else
						{
							textPositionFromPoint = textView.GetTextPositionFromPoint((Point)textView.RenderScope.RenderSize, true);
							if (textPositionFromPoint != null)
							{
								bringLineIntoViewRequest.NewCount++;
							}
						}
						if (textPositionFromPoint == null)
						{
							if (bringLineIntoViewRequest.NewPosition == null)
							{
								bringLineIntoViewRequest.NewPosition = bringLineIntoViewRequest.Position;
								bringLineIntoViewRequest.NewCount = bringLineIntoViewRequest.Count;
							}
							this.OnBringLineIntoViewCompleted(bringLineIntoViewRequest);
						}
						else if (bringLineIntoViewRequest.NewCount != 0)
						{
							bringLineIntoViewRequest.NewPosition = textPositionFromPoint;
							this.BringLineIntoViewCore(bringLineIntoViewRequest);
						}
						else
						{
							bringLineIntoViewRequest.NewPosition = this.GetPositionAtPageBoundary(bringLineIntoViewRequest.Count > 0, textView, textPositionFromPoint, bringLineIntoViewRequest.NewSuggestedX);
							this.OnBringLineIntoViewCompleted(bringLineIntoViewRequest);
						}
					}
					else if (this.IsPageNumberOutOfRange(bringLineIntoViewRequest.NewPageNumber))
					{
						this.OnBringLineIntoViewCompleted(bringLineIntoViewRequest);
					}
				}
				else if (this._pendingRequest is MultiPageTextView.BringPageIntoViewRequest)
				{
					MultiPageTextView.BringPageIntoViewRequest bringPageIntoViewRequest = (MultiPageTextView.BringPageIntoViewRequest)this._pendingRequest;
					ITextView textView = this.GetTextViewFromPageNumber(bringPageIntoViewRequest.NewPageNumber);
					if (textView != null)
					{
						Point point = this.TransformToDescendant(textView.RenderScope, bringPageIntoViewRequest.NewSuggestedOffset);
						Point point2 = point;
						Invariant.Assert(bringPageIntoViewRequest.NewCount != 0);
						ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(point2, true);
						if (textPositionFromPoint != null)
						{
							bringPageIntoViewRequest.NewCount = ((bringPageIntoViewRequest.Count > 0) ? (bringPageIntoViewRequest.NewCount - 1) : (bringPageIntoViewRequest.NewCount + 1));
						}
						if (textPositionFromPoint == null)
						{
							if (bringPageIntoViewRequest.NewPosition == null)
							{
								bringPageIntoViewRequest.NewPosition = bringPageIntoViewRequest.Position;
								bringPageIntoViewRequest.NewCount = bringPageIntoViewRequest.Count;
							}
							this.OnBringPageIntoViewCompleted(bringPageIntoViewRequest);
						}
						else if (bringPageIntoViewRequest.NewCount != 0)
						{
							bringPageIntoViewRequest.NewPosition = textPositionFromPoint;
							this.BringPageIntoViewCore(bringPageIntoViewRequest);
						}
						else
						{
							bringPageIntoViewRequest.NewPosition = textPositionFromPoint;
							this.OnBringPageIntoViewCompleted(bringPageIntoViewRequest);
						}
					}
					else if (this.IsPageNumberOutOfRange(bringPageIntoViewRequest.NewPageNumber))
					{
						this.OnBringPageIntoViewCompleted(bringPageIntoViewRequest);
					}
				}
				else if (this._pendingRequest is MultiPageTextView.BringPointIntoViewRequest)
				{
					MultiPageTextView.BringPointIntoViewRequest bringPointIntoViewRequest = (MultiPageTextView.BringPointIntoViewRequest)this._pendingRequest;
					ITextView textView = this.GetTextViewFromPoint(bringPointIntoViewRequest.Point, true);
					if (textView != null)
					{
						Point point = this.TransformToDescendant(textView.RenderScope, bringPointIntoViewRequest.Point);
						bringPointIntoViewRequest.Position = textView.GetTextPositionFromPoint(point, true);
					}
					this.OnBringPointIntoViewCompleted(bringPointIntoViewRequest);
				}
				else if (this._pendingRequest is MultiPageTextView.BringPositionIntoViewRequest)
				{
					MultiPageTextView.BringPositionIntoViewRequest bringPositionIntoViewRequest = (MultiPageTextView.BringPositionIntoViewRequest)this._pendingRequest;
					bringPositionIntoViewRequest.Succeeded = bringPositionIntoViewRequest.Position.HasValidLayout;
					this.OnBringPositionIntoViewCompleted(bringPositionIntoViewRequest);
				}
			}
			return null;
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x00141154 File Offset: 0x00140154
		private bool IsPageNumberOutOfRange(int pageNumber)
		{
			if (pageNumber < 0)
			{
				return true;
			}
			IDocumentPaginatorSource document = this._viewer.Document;
			if (document == null)
			{
				return true;
			}
			DocumentPaginator documentPaginator = document.DocumentPaginator;
			return documentPaginator == null || (documentPaginator.IsPageCountValid && pageNumber >= documentPaginator.PageCount);
		}

		// Token: 0x04000ABF RID: 2751
		private readonly DocumentViewerBase _viewer;

		// Token: 0x04000AC0 RID: 2752
		private readonly UIElement _renderScope;

		// Token: 0x04000AC1 RID: 2753
		private readonly ITextContainer _textContainer;

		// Token: 0x04000AC2 RID: 2754
		private List<DocumentPageTextView> _pageTextViews;

		// Token: 0x04000AC3 RID: 2755
		private MultiPageTextView.BringIntoViewRequest _pendingRequest;

		// Token: 0x020009DF RID: 2527
		private class BringIntoViewRequest
		{
			// Token: 0x0600841D RID: 33821 RVA: 0x003250A7 File Offset: 0x003240A7
			internal BringIntoViewRequest(object userState)
			{
				this.UserState = userState;
			}

			// Token: 0x04003FE0 RID: 16352
			internal readonly object UserState;
		}

		// Token: 0x020009E0 RID: 2528
		private class BringPositionIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x0600841E RID: 33822 RVA: 0x003250B6 File Offset: 0x003240B6
			internal BringPositionIntoViewRequest(ITextPointer position, object userState) : base(userState)
			{
				this.Position = position;
				this.Succeeded = false;
			}

			// Token: 0x04003FE1 RID: 16353
			internal readonly ITextPointer Position;

			// Token: 0x04003FE2 RID: 16354
			internal bool Succeeded;
		}

		// Token: 0x020009E1 RID: 2529
		private class BringPointIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x0600841F RID: 33823 RVA: 0x003250CD File Offset: 0x003240CD
			internal BringPointIntoViewRequest(Point point, object userState) : base(userState)
			{
				this.Point = point;
				this.Position = null;
			}

			// Token: 0x04003FE3 RID: 16355
			internal readonly Point Point;

			// Token: 0x04003FE4 RID: 16356
			internal ITextPointer Position;
		}

		// Token: 0x020009E2 RID: 2530
		private class BringLineIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x06008420 RID: 33824 RVA: 0x003250E4 File Offset: 0x003240E4
			internal BringLineIntoViewRequest(ITextPointer position, double suggestedX, int count, object userState) : base(userState)
			{
				this.Position = position;
				this.SuggestedX = suggestedX;
				this.Count = count;
				this.NewPosition = position;
				this.NewSuggestedX = suggestedX;
				this.NewCount = count;
			}

			// Token: 0x04003FE5 RID: 16357
			internal readonly ITextPointer Position;

			// Token: 0x04003FE6 RID: 16358
			internal readonly double SuggestedX;

			// Token: 0x04003FE7 RID: 16359
			internal readonly int Count;

			// Token: 0x04003FE8 RID: 16360
			internal ITextPointer NewPosition;

			// Token: 0x04003FE9 RID: 16361
			internal double NewSuggestedX;

			// Token: 0x04003FEA RID: 16362
			internal int NewCount;

			// Token: 0x04003FEB RID: 16363
			internal int NewPageNumber;
		}

		// Token: 0x020009E3 RID: 2531
		private class BringPageIntoViewRequest : MultiPageTextView.BringIntoViewRequest
		{
			// Token: 0x06008421 RID: 33825 RVA: 0x00325118 File Offset: 0x00324118
			internal BringPageIntoViewRequest(ITextPointer position, Point suggestedOffset, int count, object userState) : base(userState)
			{
				this.Position = position;
				this.SuggestedOffset = suggestedOffset;
				this.Count = count;
				this.NewPosition = position;
				this.NewSuggestedOffset = suggestedOffset;
				this.NewCount = count;
			}

			// Token: 0x04003FEC RID: 16364
			internal readonly ITextPointer Position;

			// Token: 0x04003FED RID: 16365
			internal readonly Point SuggestedOffset;

			// Token: 0x04003FEE RID: 16366
			internal readonly int Count;

			// Token: 0x04003FEF RID: 16367
			internal ITextPointer NewPosition;

			// Token: 0x04003FF0 RID: 16368
			internal Point NewSuggestedOffset;

			// Token: 0x04003FF1 RID: 16369
			internal int NewCount;

			// Token: 0x04003FF2 RID: 16370
			internal int NewPageNumber;
		}
	}
}
