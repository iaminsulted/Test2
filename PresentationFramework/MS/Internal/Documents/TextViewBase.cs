using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020001F5 RID: 501
	internal abstract class TextViewBase : ITextView
	{
		// Token: 0x06001230 RID: 4656
		internal abstract ITextPointer GetTextPositionFromPoint(Point point, bool snapToText);

		// Token: 0x06001231 RID: 4657 RVA: 0x0014A394 File Offset: 0x00149394
		internal virtual Rect GetRectangleFromTextPosition(ITextPointer position)
		{
			Transform transform;
			Rect rect = this.GetRawRectangleFromTextPosition(position, out transform);
			Invariant.Assert(transform != null);
			if (rect != Rect.Empty)
			{
				rect = transform.TransformBounds(rect);
			}
			return rect;
		}

		// Token: 0x06001232 RID: 4658
		internal abstract Rect GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform);

		// Token: 0x06001233 RID: 4659
		internal abstract Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition);

		// Token: 0x06001234 RID: 4660
		internal abstract ITextPointer GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved);

		// Token: 0x06001235 RID: 4661 RVA: 0x0014A3CA File Offset: 0x001493CA
		internal virtual ITextPointer GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			newSuggestedOffset = suggestedOffset;
			pagesMoved = 0;
			return position;
		}

		// Token: 0x06001236 RID: 4662
		internal abstract bool IsAtCaretUnitBoundary(ITextPointer position);

		// Token: 0x06001237 RID: 4663
		internal abstract ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction);

		// Token: 0x06001238 RID: 4664
		internal abstract ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position);

		// Token: 0x06001239 RID: 4665
		internal abstract TextSegment GetLineRange(ITextPointer position);

		// Token: 0x0600123A RID: 4666 RVA: 0x0014A3D9 File Offset: 0x001493D9
		internal virtual ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			return new ReadOnlyCollection<GlyphRun>(new List<GlyphRun>());
		}

		// Token: 0x0600123B RID: 4667
		internal abstract bool Contains(ITextPointer position);

		// Token: 0x0600123C RID: 4668 RVA: 0x0014A400 File Offset: 0x00149400
		internal static void BringRectIntoViewMinimally(ITextView textView, Rect rect)
		{
			IScrollInfo scrollInfo = textView.RenderScope as IScrollInfo;
			if (scrollInfo != null)
			{
				Rect rect2 = new Rect(scrollInfo.HorizontalOffset, scrollInfo.VerticalOffset, scrollInfo.ViewportWidth, scrollInfo.ViewportHeight);
				rect.X += rect2.X;
				rect.Y += rect2.Y;
				double num = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect2.Left, rect2.Right, rect.Left, rect.Right);
				double num2 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect2.Top, rect2.Bottom, rect.Top, rect.Bottom);
				scrollInfo.SetHorizontalOffset(num);
				scrollInfo.SetVerticalOffset(num2);
				FrameworkElement frameworkElement = FrameworkElement.GetFrameworkParent(textView.RenderScope) as FrameworkElement;
				if (frameworkElement != null)
				{
					if (scrollInfo.ViewportWidth > 0.0)
					{
						rect.X -= num;
					}
					if (scrollInfo.ViewportHeight > 0.0)
					{
						rect.Y -= num2;
					}
					frameworkElement.BringIntoView(rect);
					return;
				}
			}
			else
			{
				((FrameworkElement)textView.RenderScope).BringIntoView(rect);
			}
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0014A52A File Offset: 0x0014952A
		internal virtual void BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			this.OnBringPositionIntoViewCompleted(new BringPositionIntoViewCompletedEventArgs(position, this.Contains(position), null, false, userState));
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0014A55C File Offset: 0x0014955C
		internal virtual void BringPointIntoViewAsync(Point point, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			ITextPointer textPositionFromPoint = this.GetTextPositionFromPoint(point, true);
			this.OnBringPointIntoViewCompleted(new BringPointIntoViewCompletedEventArgs(point, textPositionFromPoint, textPositionFromPoint != null, null, false, userState));
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0014A5A0 File Offset: 0x001495A0
		internal virtual void BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			double newSuggestedX;
			int num;
			ITextPointer positionAtNextLine = this.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out num);
			this.OnBringLineIntoViewCompleted(new BringLineIntoViewCompletedEventArgs(position, suggestedX, count, positionAtNextLine, newSuggestedX, num, num == count, null, false, userState));
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0014A5EC File Offset: 0x001495EC
		internal virtual void BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			if (!this.IsValid)
			{
				throw new InvalidOperationException(SR.Get("TextViewInvalidLayout"));
			}
			Point newSuggestedOffset;
			int num;
			ITextPointer positionAtNextPage = this.GetPositionAtNextPage(position, suggestedOffset, count, out newSuggestedOffset, out num);
			this.OnBringPageIntoViewCompleted(new BringPageIntoViewCompletedEventArgs(position, suggestedOffset, count, positionAtNextPage, newSuggestedOffset, num, num == count, null, false, userState));
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void CancelAsync(object userState)
		{
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x001463B5 File Offset: 0x001453B5
		internal virtual bool Validate()
		{
			return this.IsValid;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0014A638 File Offset: 0x00149638
		internal virtual bool Validate(Point point)
		{
			return this.Validate();
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0014A640 File Offset: 0x00149640
		internal virtual bool Validate(ITextPointer position)
		{
			this.Validate();
			return this.IsValid && this.Contains(position);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void ThrottleBackgroundTasksForUserInput()
		{
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001246 RID: 4678
		internal abstract UIElement RenderScope { get; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001247 RID: 4679
		internal abstract ITextContainer TextContainer { get; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001248 RID: 4680
		internal abstract bool IsValid { get; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool RendersOwnSelection
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x0600124A RID: 4682
		internal abstract ReadOnlyCollection<TextSegment> TextSegments { get; }

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600124B RID: 4683 RVA: 0x0014A65C File Offset: 0x0014965C
		// (remove) Token: 0x0600124C RID: 4684 RVA: 0x0014A694 File Offset: 0x00149694
		public event BringPositionIntoViewCompletedEventHandler BringPositionIntoViewCompleted;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600124D RID: 4685 RVA: 0x0014A6CC File Offset: 0x001496CC
		// (remove) Token: 0x0600124E RID: 4686 RVA: 0x0014A704 File Offset: 0x00149704
		public event BringPointIntoViewCompletedEventHandler BringPointIntoViewCompleted;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600124F RID: 4687 RVA: 0x0014A73C File Offset: 0x0014973C
		// (remove) Token: 0x06001250 RID: 4688 RVA: 0x0014A774 File Offset: 0x00149774
		public event BringLineIntoViewCompletedEventHandler BringLineIntoViewCompleted;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06001251 RID: 4689 RVA: 0x0014A7AC File Offset: 0x001497AC
		// (remove) Token: 0x06001252 RID: 4690 RVA: 0x0014A7E4 File Offset: 0x001497E4
		public event BringPageIntoViewCompletedEventHandler BringPageIntoViewCompleted;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06001253 RID: 4691 RVA: 0x0014A81C File Offset: 0x0014981C
		// (remove) Token: 0x06001254 RID: 4692 RVA: 0x0014A854 File Offset: 0x00149854
		public event EventHandler Updated;

		// Token: 0x06001255 RID: 4693 RVA: 0x0014A889 File Offset: 0x00149889
		protected virtual void OnBringPositionIntoViewCompleted(BringPositionIntoViewCompletedEventArgs e)
		{
			if (this.BringPositionIntoViewCompleted != null)
			{
				this.BringPositionIntoViewCompleted(this, e);
			}
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0014A8A0 File Offset: 0x001498A0
		protected virtual void OnBringPointIntoViewCompleted(BringPointIntoViewCompletedEventArgs e)
		{
			if (this.BringPointIntoViewCompleted != null)
			{
				this.BringPointIntoViewCompleted(this, e);
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x0014A8B7 File Offset: 0x001498B7
		protected virtual void OnBringLineIntoViewCompleted(BringLineIntoViewCompletedEventArgs e)
		{
			if (this.BringLineIntoViewCompleted != null)
			{
				this.BringLineIntoViewCompleted(this, e);
			}
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x0014A8CE File Offset: 0x001498CE
		protected virtual void OnBringPageIntoViewCompleted(BringPageIntoViewCompletedEventArgs e)
		{
			if (this.BringPageIntoViewCompleted != null)
			{
				this.BringPageIntoViewCompleted(this, e);
			}
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0014A8E5 File Offset: 0x001498E5
		protected virtual void OnUpdated(EventArgs e)
		{
			if (this.Updated != null)
			{
				this.Updated(this, e);
			}
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x0014A8FC File Offset: 0x001498FC
		protected virtual Transform GetAggregateTransform(Transform firstTransform, Transform secondTransform)
		{
			Invariant.Assert(firstTransform != null);
			Invariant.Assert(secondTransform != null);
			if (firstTransform.IsIdentity)
			{
				return secondTransform;
			}
			if (secondTransform.IsIdentity)
			{
				return firstTransform;
			}
			return new MatrixTransform(firstTransform.Value * secondTransform.Value);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0014A93A File Offset: 0x0014993A
		ITextPointer ITextView.GetTextPositionFromPoint(Point point, bool snapToText)
		{
			return this.GetTextPositionFromPoint(point, snapToText);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0014A944 File Offset: 0x00149944
		Rect ITextView.GetRectangleFromTextPosition(ITextPointer position)
		{
			return this.GetRectangleFromTextPosition(position);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0014A94D File Offset: 0x0014994D
		Rect ITextView.GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			return this.GetRawRectangleFromTextPosition(position, out transform);
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0014A957 File Offset: 0x00149957
		Geometry ITextView.GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			return this.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x0014A961 File Offset: 0x00149961
		ITextPointer ITextView.GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			return this.GetPositionAtNextLine(position, suggestedX, count, out newSuggestedX, out linesMoved);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x0014A970 File Offset: 0x00149970
		ITextPointer ITextView.GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			return this.GetPositionAtNextPage(position, suggestedOffset, count, out newSuggestedOffset, out pagesMoved);
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x0014A97F File Offset: 0x0014997F
		bool ITextView.IsAtCaretUnitBoundary(ITextPointer position)
		{
			return this.IsAtCaretUnitBoundary(position);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0014A988 File Offset: 0x00149988
		ITextPointer ITextView.GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return this.GetNextCaretUnitPosition(position, direction);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0014A992 File Offset: 0x00149992
		ITextPointer ITextView.GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return this.GetBackspaceCaretUnitPosition(position);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0014A99B File Offset: 0x0014999B
		TextSegment ITextView.GetLineRange(ITextPointer position)
		{
			return this.GetLineRange(position);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0014A9A4 File Offset: 0x001499A4
		ReadOnlyCollection<GlyphRun> ITextView.GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			return this.GetGlyphRuns(start, end);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0014A9AE File Offset: 0x001499AE
		bool ITextView.Contains(ITextPointer position)
		{
			return this.Contains(position);
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0014A9B7 File Offset: 0x001499B7
		void ITextView.BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			this.BringPositionIntoViewAsync(position, userState);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0014A9C1 File Offset: 0x001499C1
		void ITextView.BringPointIntoViewAsync(Point point, object userState)
		{
			this.BringPointIntoViewAsync(point, userState);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0014A9CB File Offset: 0x001499CB
		void ITextView.BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			this.BringLineIntoViewAsync(position, suggestedX, count, userState);
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0014A9D8 File Offset: 0x001499D8
		void ITextView.BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			this.BringPageIntoViewAsync(position, suggestedOffset, count, userState);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0014A9E5 File Offset: 0x001499E5
		void ITextView.CancelAsync(object userState)
		{
			this.CancelAsync(userState);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0014A638 File Offset: 0x00149638
		bool ITextView.Validate()
		{
			return this.Validate();
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0014A9EE File Offset: 0x001499EE
		bool ITextView.Validate(Point point)
		{
			return this.Validate(point);
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0014A9F7 File Offset: 0x001499F7
		bool ITextView.Validate(ITextPointer position)
		{
			return this.Validate(position);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0014AA00 File Offset: 0x00149A00
		void ITextView.ThrottleBackgroundTasksForUserInput()
		{
			this.ThrottleBackgroundTasksForUserInput();
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001270 RID: 4720 RVA: 0x0014AA08 File Offset: 0x00149A08
		UIElement ITextView.RenderScope
		{
			get
			{
				return this.RenderScope;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001271 RID: 4721 RVA: 0x0014AA10 File Offset: 0x00149A10
		ITextContainer ITextView.TextContainer
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x001463B5 File Offset: 0x001453B5
		bool ITextView.IsValid
		{
			get
			{
				return this.IsValid;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x0014AA18 File Offset: 0x00149A18
		bool ITextView.RendersOwnSelection
		{
			get
			{
				return this.RendersOwnSelection;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x0014AA20 File Offset: 0x00149A20
		ReadOnlyCollection<TextSegment> ITextView.TextSegments
		{
			get
			{
				return this.TextSegments;
			}
		}
	}
}
