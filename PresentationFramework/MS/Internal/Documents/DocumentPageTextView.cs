using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001BE RID: 446
	internal class DocumentPageTextView : TextViewBase
	{
		// Token: 0x06000F16 RID: 3862 RVA: 0x0013C4FC File Offset: 0x0013B4FC
		internal DocumentPageTextView(DocumentPageView owner, ITextContainer textContainer)
		{
			Invariant.Assert(owner != null && textContainer != null);
			this._owner = owner;
			this._page = owner.DocumentPageInternal;
			this._textContainer = textContainer;
			if (this._page is IServiceProvider)
			{
				this._pageTextView = (((IServiceProvider)this._page).GetService(typeof(ITextView)) as ITextView);
			}
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated += this.HandlePageTextViewUpdated;
			}
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0013C58C File Offset: 0x0013B58C
		internal DocumentPageTextView(FlowDocumentView owner, ITextContainer textContainer)
		{
			Invariant.Assert(owner != null && textContainer != null);
			this._owner = owner;
			this._page = owner.DocumentPage;
			this._textContainer = textContainer;
			if (this._page is IServiceProvider)
			{
				this._pageTextView = (((IServiceProvider)this._page).GetService(typeof(ITextView)) as ITextView);
			}
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated += this.HandlePageTextViewUpdated;
			}
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0013C619 File Offset: 0x0013B619
		internal override ITextPointer GetTextPositionFromPoint(Point point, bool snapToText)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return null;
			}
			point = this.TransformToDescendant(point);
			return this._pageTextView.GetTextPositionFromPoint(point, snapToText);
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0013C654 File Offset: 0x0013B654
		internal override Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			transform = Transform.Identity;
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return Rect.Empty;
			}
			Transform transform2;
			Rect rawRectangleFromTextPosition = this._pageTextView.GetRawRectangleFromTextPosition(position, out transform2);
			Invariant.Assert(transform2 != null);
			Transform transformToAncestor = this.GetTransformToAncestor();
			transform = this.GetAggregateTransform(transform2, transformToAncestor);
			return rawRectangleFromTextPosition;
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0013C6B8 File Offset: 0x0013B6B8
		internal override Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			Geometry geometry = null;
			if (!this.IsPageMissing)
			{
				geometry = this._pageTextView.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
				if (geometry != null)
				{
					Transform affineTransform = this.GetTransformToAncestor().AffineTransform;
					CaretElement.AddTransformToGeometry(geometry, affineTransform);
				}
			}
			return geometry;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0013C70C File Offset: 0x0013B70C
		internal override ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				newSuggestedX = suggestedX;
				linesMoved = 0;
				return position;
			}
			suggestedX = this.TransformToDescendant(new Point(suggestedX, 0.0)).X;
			ITextPointer positionAtNextLine = this._pageTextView.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
			newSuggestedX = this.TransformToAncestor(new Point(newSuggestedX, 0.0)).X;
			return positionAtNextLine;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0013C798 File Offset: 0x0013B798
		internal override ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			ITextPointer textPointer = position;
			newSuggestedOffset = suggestedOffset;
			Point point = suggestedOffset;
			pagesMoved = 0;
			if (count == 0)
			{
				return textPointer;
			}
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return position;
			}
			point.Y = this.GetYOffsetAtNextPage(point.Y, count, out pagesMoved);
			if (pagesMoved != 0)
			{
				point = this.TransformToDescendant(point);
				textPointer = this._pageTextView.GetTextPositionFromPoint(point, true);
				Invariant.Assert(textPointer != null);
				Rect rectangleFromTextPosition = this._pageTextView.GetRectangleFromTextPosition(position);
				newSuggestedOffset = this.TransformToAncestor(new Point(rectangleFromTextPosition.X, rectangleFromTextPosition.Y));
			}
			return textPointer;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0013C845 File Offset: 0x0013B845
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return !this.IsPageMissing && this._pageTextView.IsAtCaretUnitBoundary(position);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0013C875 File Offset: 0x0013B875
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return null;
			}
			return this._pageTextView.GetNextCaretUnitPosition(position, direction);
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0013C8A6 File Offset: 0x0013B8A6
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return null;
			}
			return this._pageTextView.GetBackspaceCaretUnitPosition(position);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0013C8D6 File Offset: 0x0013B8D6
		internal override TextSegment GetLineRange(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return TextSegment.Null;
			}
			return this._pageTextView.GetLineRange(position);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0013C90A File Offset: 0x0013B90A
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			if (this.IsPageMissing)
			{
				return new ReadOnlyCollection<GlyphRun>(new List<GlyphRun>());
			}
			return this._pageTextView.GetGlyphRuns(start, end);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0013C944 File Offset: 0x0013B944
		internal override bool Contains(ITextPointer position)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return !this.IsPageMissing && this._pageTextView.Contains(position);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0013C974 File Offset: 0x0013B974
		internal void OnPageConnected()
		{
			this.OnPageDisconnected();
			if (this._owner is DocumentPageView)
			{
				this._page = ((DocumentPageView)this._owner).DocumentPageInternal;
			}
			else if (this._owner is FlowDocumentView)
			{
				this._page = ((FlowDocumentView)this._owner).DocumentPage;
			}
			if (this._page is IServiceProvider)
			{
				this._pageTextView = (((IServiceProvider)this._page).GetService(typeof(ITextView)) as ITextView);
			}
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated += this.HandlePageTextViewUpdated;
			}
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0013CA33 File Offset: 0x0013BA33
		internal void OnPageDisconnected()
		{
			if (this._pageTextView != null)
			{
				this._pageTextView.Updated -= this.HandlePageTextViewUpdated;
			}
			this._pageTextView = null;
			this._page = null;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0013CA62 File Offset: 0x0013BA62
		internal void OnTransformChanged()
		{
			if (this.IsValid)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0013CA77 File Offset: 0x0013BA77
		internal override bool Validate()
		{
			if (!this._owner.IsMeasureValid || !this._owner.IsArrangeValid)
			{
				this._owner.UpdateLayout();
			}
			return this._pageTextView != null && this._pageTextView.Validate();
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0013CAB4 File Offset: 0x0013BAB4
		internal override bool Validate(ITextPointer position)
		{
			FlowDocumentView flowDocumentView = this._owner as FlowDocumentView;
			bool result;
			if (flowDocumentView == null || flowDocumentView.Document == null)
			{
				result = base.Validate(position);
			}
			else
			{
				if (this.Validate())
				{
					BackgroundFormatInfo backgroundFormatInfo = flowDocumentView.Document.StructuralCache.BackgroundFormatInfo;
					FlowDocumentFormatter bottomlessFormatter = flowDocumentView.Document.BottomlessFormatter;
					int num = -1;
					while (this.IsValid && !this.Contains(position))
					{
						backgroundFormatInfo.BackgroundFormat(bottomlessFormatter, true);
						this._owner.UpdateLayout();
						if (backgroundFormatInfo.CPInterrupted <= num)
						{
							break;
						}
						num = backgroundFormatInfo.CPInterrupted;
					}
				}
				result = (this.IsValid && this.Contains(position));
			}
			return result;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0013CB58 File Offset: 0x0013BB58
		internal override void ThrottleBackgroundTasksForUserInput()
		{
			FlowDocumentView flowDocumentView = this._owner as FlowDocumentView;
			if (flowDocumentView != null && flowDocumentView.Document != null)
			{
				flowDocumentView.Document.StructuralCache.ThrottleBackgroundFormatting();
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0013CB8C File Offset: 0x0013BB8C
		internal override UIElement RenderScope
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0013CB94 File Offset: 0x0013BB94
		internal override ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0013CB9C File Offset: 0x0013BB9C
		internal override bool IsValid
		{
			get
			{
				return this._owner.IsMeasureValid && this._owner.IsArrangeValid && this._page != null && (this.IsPageMissing || (this._pageTextView != null && this._pageTextView.IsValid));
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0013CBEC File Offset: 0x0013BBEC
		internal override bool RendersOwnSelection
		{
			get
			{
				return this._pageTextView != null && this._pageTextView.RendersOwnSelection;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0013CC03 File Offset: 0x0013BC03
		internal override ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				if (!this.IsValid || this.IsPageMissing)
				{
					return new ReadOnlyCollection<TextSegment>(new List<TextSegment>());
				}
				return this._pageTextView.TextSegments;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0013CC2B File Offset: 0x0013BC2B
		internal DocumentPageView DocumentPageView
		{
			get
			{
				return this._owner as DocumentPageView;
			}
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0013CC38 File Offset: 0x0013BC38
		private void HandlePageTextViewUpdated(object sender, EventArgs e)
		{
			Invariant.Assert(this._pageTextView != null);
			if (sender == this._pageTextView)
			{
				this.OnUpdated(EventArgs.Empty);
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0013CC5C File Offset: 0x0013BC5C
		private Transform GetTransformToAncestor()
		{
			Invariant.Assert(this.IsValid && !this.IsPageMissing);
			Transform transform = this._page.Visual.TransformToAncestor(this._owner) as Transform;
			if (transform == null)
			{
				transform = Transform.Identity;
			}
			return transform;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0013CCA8 File Offset: 0x0013BCA8
		private Point TransformToAncestor(Point point)
		{
			Invariant.Assert(this.IsValid && !this.IsPageMissing);
			GeneralTransform generalTransform = this._page.Visual.TransformToAncestor(this._owner);
			if (generalTransform != null)
			{
				point = generalTransform.Transform(point);
			}
			return point;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0013CCF4 File Offset: 0x0013BCF4
		private Point TransformToDescendant(Point point)
		{
			Invariant.Assert(this.IsValid && !this.IsPageMissing);
			GeneralTransform generalTransform = this._page.Visual.TransformToAncestor(this._owner);
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

		// Token: 0x06000F33 RID: 3891 RVA: 0x0013CD48 File Offset: 0x0013BD48
		private double GetYOffsetAtNextPage(double offset, int count, out int pagesMoved)
		{
			double num = offset;
			pagesMoved = 0;
			if (this._owner is IScrollInfo && ((IScrollInfo)this._owner).ScrollOwner != null)
			{
				IScrollInfo scrollInfo = (IScrollInfo)this._owner;
				double viewportHeight = scrollInfo.ViewportHeight;
				double extentHeight = scrollInfo.ExtentHeight;
				if (count > 0)
				{
					while (pagesMoved < count)
					{
						if (!DoubleUtil.LessThanOrClose(offset + viewportHeight, extentHeight))
						{
							break;
						}
						num += viewportHeight;
						pagesMoved++;
					}
				}
				else
				{
					while (Math.Abs(pagesMoved) < Math.Abs(count) && DoubleUtil.GreaterThanOrClose(offset - viewportHeight, 0.0))
					{
						num -= viewportHeight;
						pagesMoved--;
					}
				}
			}
			return num;
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0013CDE0 File Offset: 0x0013BDE0
		private bool IsPageMissing
		{
			get
			{
				return this._page == DocumentPage.Missing;
			}
		}

		// Token: 0x04000A8D RID: 2701
		private readonly UIElement _owner;

		// Token: 0x04000A8E RID: 2702
		private readonly ITextContainer _textContainer;

		// Token: 0x04000A8F RID: 2703
		private DocumentPage _page;

		// Token: 0x04000A90 RID: 2704
		private ITextView _pageTextView;
	}
}
