using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.PtsHost;
using MS.Internal.Text;
using Standard;

namespace System.Windows.Controls
{
	// Token: 0x02000714 RID: 1812
	internal class TextBoxView : FrameworkElement, ITextView, IScrollInfo, IServiceProvider
	{
		// Token: 0x06005EBC RID: 24252 RVA: 0x0029157C File Offset: 0x0029057C
		static TextBoxView()
		{
			FrameworkElement.MarginProperty.OverrideMetadata(typeof(TextBoxView), new FrameworkPropertyMetadata(new Thickness(2.0, 0.0, 2.0, 0.0)));
		}

		// Token: 0x06005EBD RID: 24253 RVA: 0x002915D0 File Offset: 0x002905D0
		internal TextBoxView(ITextBoxViewHost host)
		{
			Invariant.Assert(host is Control);
			this._host = host;
		}

		// Token: 0x06005EBE RID: 24254 RVA: 0x002915F0 File Offset: 0x002905F0
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == typeof(ITextView))
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06005EBF RID: 24255 RVA: 0x00291614 File Offset: 0x00290614
		void IScrollInfo.LineUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineUp(this);
			}
		}

		// Token: 0x06005EC0 RID: 24256 RVA: 0x0029162A File Offset: 0x0029062A
		void IScrollInfo.LineDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineDown(this);
			}
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x00291640 File Offset: 0x00290640
		void IScrollInfo.LineLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineLeft(this);
			}
		}

		// Token: 0x06005EC2 RID: 24258 RVA: 0x00291656 File Offset: 0x00290656
		void IScrollInfo.LineRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineRight(this);
			}
		}

		// Token: 0x06005EC3 RID: 24259 RVA: 0x0029166C File Offset: 0x0029066C
		void IScrollInfo.PageUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageUp(this);
			}
		}

		// Token: 0x06005EC4 RID: 24260 RVA: 0x00291682 File Offset: 0x00290682
		void IScrollInfo.PageDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageDown(this);
			}
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x00291698 File Offset: 0x00290698
		void IScrollInfo.PageLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageLeft(this);
			}
		}

		// Token: 0x06005EC6 RID: 24262 RVA: 0x002916AE File Offset: 0x002906AE
		void IScrollInfo.PageRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageRight(this);
			}
		}

		// Token: 0x06005EC7 RID: 24263 RVA: 0x002916C4 File Offset: 0x002906C4
		void IScrollInfo.MouseWheelUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelUp(this);
			}
		}

		// Token: 0x06005EC8 RID: 24264 RVA: 0x002916DA File Offset: 0x002906DA
		void IScrollInfo.MouseWheelDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelDown(this);
			}
		}

		// Token: 0x06005EC9 RID: 24265 RVA: 0x002916F0 File Offset: 0x002906F0
		void IScrollInfo.MouseWheelLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelLeft(this);
			}
		}

		// Token: 0x06005ECA RID: 24266 RVA: 0x00291706 File Offset: 0x00290706
		void IScrollInfo.MouseWheelRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelRight(this);
			}
		}

		// Token: 0x06005ECB RID: 24267 RVA: 0x0029171C File Offset: 0x0029071C
		void IScrollInfo.SetHorizontalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetHorizontalOffset(this, offset);
			}
		}

		// Token: 0x06005ECC RID: 24268 RVA: 0x00291733 File Offset: 0x00290733
		void IScrollInfo.SetVerticalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetVerticalOffset(this, offset);
			}
		}

		// Token: 0x06005ECD RID: 24269 RVA: 0x0029174A File Offset: 0x0029074A
		Rect IScrollInfo.MakeVisible(Visual visual, Rect rectangle)
		{
			if (this._scrollData == null)
			{
				rectangle = Rect.Empty;
			}
			else
			{
				rectangle = this._scrollData.MakeVisible(this, visual, rectangle);
			}
			return rectangle;
		}

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x06005ECE RID: 24270 RVA: 0x0029176E File Offset: 0x0029076E
		// (set) Token: 0x06005ECF RID: 24271 RVA: 0x00291785 File Offset: 0x00290785
		bool IScrollInfo.CanVerticallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanVerticallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanVerticallyScroll = value;
				}
			}
		}

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x06005ED0 RID: 24272 RVA: 0x0029179B File Offset: 0x0029079B
		// (set) Token: 0x06005ED1 RID: 24273 RVA: 0x002917B2 File Offset: 0x002907B2
		bool IScrollInfo.CanHorizontallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanHorizontallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanHorizontallyScroll = value;
				}
			}
		}

		// Token: 0x170015E5 RID: 5605
		// (get) Token: 0x06005ED2 RID: 24274 RVA: 0x002917C8 File Offset: 0x002907C8
		double IScrollInfo.ExtentWidth
		{
			get
			{
				double num = 0.0;
				if (this._scrollData != null)
				{
					num = this._scrollData.ExtentWidth;
					if (base.UseLayoutRounding)
					{
						num = UIElement.RoundLayoutValue(num, base.GetDpi().DpiScaleX);
					}
				}
				return num;
			}
		}

		// Token: 0x170015E6 RID: 5606
		// (get) Token: 0x06005ED3 RID: 24275 RVA: 0x00291814 File Offset: 0x00290814
		double IScrollInfo.ExtentHeight
		{
			get
			{
				double num = 0.0;
				if (this._scrollData != null)
				{
					num = this._scrollData.ExtentHeight;
					if (base.UseLayoutRounding)
					{
						num = UIElement.RoundLayoutValue(num, base.GetDpi().DpiScaleY);
					}
				}
				return num;
			}
		}

		// Token: 0x170015E7 RID: 5607
		// (get) Token: 0x06005ED4 RID: 24276 RVA: 0x0029185D File Offset: 0x0029085D
		double IScrollInfo.ViewportWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportWidth;
			}
		}

		// Token: 0x170015E8 RID: 5608
		// (get) Token: 0x06005ED5 RID: 24277 RVA: 0x0029187C File Offset: 0x0029087C
		double IScrollInfo.ViewportHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportHeight;
			}
		}

		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x06005ED6 RID: 24278 RVA: 0x0029189B File Offset: 0x0029089B
		double IScrollInfo.HorizontalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.HorizontalOffset;
			}
		}

		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x06005ED7 RID: 24279 RVA: 0x002918BA File Offset: 0x002908BA
		double IScrollInfo.VerticalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.VerticalOffset;
			}
		}

		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x06005ED8 RID: 24280 RVA: 0x002918D9 File Offset: 0x002908D9
		// (set) Token: 0x06005ED9 RID: 24281 RVA: 0x002918F0 File Offset: 0x002908F0
		ScrollViewer IScrollInfo.ScrollOwner
		{
			get
			{
				if (this._scrollData == null)
				{
					return null;
				}
				return this._scrollData.ScrollOwner;
			}
			set
			{
				if (this._scrollData == null)
				{
					this._scrollData = new ScrollData();
				}
				this._scrollData.SetScrollOwner(this, value);
			}
		}

		// Token: 0x06005EDA RID: 24282 RVA: 0x00291914 File Offset: 0x00290914
		protected override Size MeasureOverride(Size constraint)
		{
			this.EnsureTextContainerListeners();
			if (this._lineMetrics == null)
			{
				this._lineMetrics = new List<TextBoxView.LineRecord>(1);
			}
			this._cache = null;
			this.EnsureCache();
			LineProperties lineProperties = this._cache.LineProperties;
			bool flag = !DoubleUtil.AreClose(constraint.Width, this._previousConstraint.Width);
			if (flag && lineProperties.TextAlignment != TextAlignment.Left)
			{
				this._viewportLineVisuals = null;
			}
			bool flag2 = flag && lineProperties.TextWrapping != TextWrapping.NoWrap;
			Size size;
			if (this._lineMetrics.Count == 0 || flag2)
			{
				this._dirtyList = null;
			}
			else if (this._dirtyList == null && !this.IsBackgroundLayoutPending)
			{
				size = this._contentSize;
				goto IL_176;
			}
			if (this._dirtyList != null && this._lineMetrics.Count == 1 && this._lineMetrics[0].EndOffset == 0)
			{
				this._lineMetrics.Clear();
				this._viewportLineVisuals = null;
				this._dirtyList = null;
			}
			Size size2 = constraint;
			TextDpi.EnsureValidLineWidth(ref size2);
			if (this._dirtyList == null)
			{
				if (flag2)
				{
					this._lineMetrics.Clear();
					this._viewportLineVisuals = null;
				}
				size = this.FullMeasureTick(size2.Width, lineProperties);
			}
			else
			{
				size = this.IncrementalMeasure(size2.Width, lineProperties);
			}
			Invariant.Assert(this._lineMetrics.Count >= 1);
			this._dirtyList = null;
			double width = this._contentSize.Width;
			this._contentSize = size;
			if (width != size.Width && lineProperties.TextAlignment != TextAlignment.Left)
			{
				this.Rerender();
			}
			IL_176:
			if (this._scrollData != null)
			{
				size.Width = Math.Min(constraint.Width, size.Width);
				size.Height = Math.Min(constraint.Height, size.Height);
			}
			this._previousConstraint = constraint;
			return size;
		}

		// Token: 0x06005EDB RID: 24283 RVA: 0x00291ADB File Offset: 0x00290ADB
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (this._lineMetrics != null && this._lineMetrics.Count != 0)
			{
				this.EnsureCache();
				this.ArrangeScrollData(arrangeSize);
				this.ArrangeVisuals(arrangeSize);
				this._cache = null;
				this.FireTextViewUpdatedEvent();
			}
			return arrangeSize;
		}

		// Token: 0x06005EDC RID: 24284 RVA: 0x00291B14 File Offset: 0x00290B14
		protected override void OnRender(DrawingContext context)
		{
			context.DrawRectangle(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), null, new Rect(0.0, 0.0, base.RenderSize.Width, base.RenderSize.Height));
		}

		// Token: 0x06005EDD RID: 24285 RVA: 0x00291B69 File Offset: 0x00290B69
		protected override Visual GetVisualChild(int index)
		{
			if (index >= this.VisualChildrenCount)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._visualChildren[index];
		}

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06005EDE RID: 24286 RVA: 0x00291B8B File Offset: 0x00290B8B
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._visualChildren != null)
				{
					return this._visualChildren.Count;
				}
				return 0;
			}
		}

		// Token: 0x06005EDF RID: 24287 RVA: 0x00291BA4 File Offset: 0x00290BA4
		ITextPointer ITextView.GetTextPositionFromPoint(Point point, bool snapToText)
		{
			Invariant.Assert(this.IsLayoutValid);
			point = this.TransformToDocumentSpace(point);
			int lineIndexFromPoint = this.GetLineIndexFromPoint(point, snapToText);
			ITextPointer textPointer;
			if (lineIndexFromPoint == -1)
			{
				textPointer = null;
			}
			else
			{
				textPointer = this.GetTextPositionFromDistance(lineIndexFromPoint, point.X);
				textPointer.Freeze();
			}
			return textPointer;
		}

		// Token: 0x06005EE0 RID: 24288 RVA: 0x00291BEC File Offset: 0x00290BEC
		Rect ITextView.GetRectangleFromTextPosition(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			int num = position.Offset;
			if (num > 0 && position.LogicalDirection == LogicalDirection.Backward)
			{
				num--;
			}
			int lineIndexFromOffset = this.GetLineIndexFromOffset(num);
			LineProperties lineProperties;
			FlowDirection flowDirection;
			Rect boundsFromTextPosition;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromOffset, out lineProperties))
			{
				boundsFromTextPosition = formattedLine.GetBoundsFromTextPosition(num, out flowDirection);
			}
			if (!boundsFromTextPosition.IsEmpty)
			{
				boundsFromTextPosition.Y += (double)lineIndexFromOffset * this._lineHeight;
				if (lineProperties.FlowDirection != flowDirection)
				{
					if (position.LogicalDirection == LogicalDirection.Forward || position.Offset == 0)
					{
						boundsFromTextPosition.X = boundsFromTextPosition.Right;
					}
				}
				else if (position.LogicalDirection == LogicalDirection.Backward && position.Offset > 0)
				{
					boundsFromTextPosition.X = boundsFromTextPosition.Right;
				}
				boundsFromTextPosition.Width = 0.0;
			}
			return this.TransformToVisualSpace(boundsFromTextPosition);
		}

		// Token: 0x06005EE1 RID: 24289 RVA: 0x00291CE4 File Offset: 0x00290CE4
		Rect ITextView.GetRawRectangleFromTextPosition(ITextPointer position, out Transform transform)
		{
			transform = Transform.Identity;
			return ((ITextView)this).GetRectangleFromTextPosition(position);
		}

		// Token: 0x06005EE2 RID: 24290 RVA: 0x00291CF4 File Offset: 0x00290CF4
		Geometry ITextView.GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			Invariant.Assert(this.IsLayoutValid);
			Geometry result = null;
			double num = ((Control)this._host).FontSize * 0.5;
			int num2 = Math.Min(this._lineMetrics[this._lineMetrics.Count - 1].EndOffset, startPosition.Offset);
			int num3 = Math.Min(this._lineMetrics[this._lineMetrics.Count - 1].EndOffset, endPosition.Offset);
			int num4;
			int num5;
			this.GetVisibleLines(out num4, out num5);
			num4 = Math.Max(num4, this.GetLineIndexFromOffset(num2, LogicalDirection.Forward));
			num5 = Math.Min(num5, this.GetLineIndexFromOffset(num3, LogicalDirection.Backward));
			if (num4 > num5)
			{
				return null;
			}
			bool flag = this._lineMetrics[num4].Offset < num2 || this._lineMetrics[num4].EndOffset > num3;
			bool flag2 = this._lineMetrics[num5].Offset < num2 || this._lineMetrics[num5].EndOffset > num3;
			TextAlignment calculatedTextAlignment = this.CalculatedTextAlignment;
			int i = num4;
			if (flag)
			{
				this.GetTightBoundingGeometryFromLineIndex(i, num2, num3, calculatedTextAlignment, num, ref result);
				i++;
			}
			if (num4 <= num5 && !flag2)
			{
				num5++;
			}
			while (i < num5)
			{
				double contentOffset = this.GetContentOffset(this._lineMetrics[i].Width, calculatedTextAlignment);
				Rect rect = new Rect(contentOffset, (double)i * this._lineHeight, this._lineMetrics[i].Width, this._lineHeight);
				if (TextPointerBase.IsNextToPlainLineBreak(this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[i].EndOffset, LogicalDirection.Backward), LogicalDirection.Backward))
				{
					rect.Width += num;
				}
				rect = this.TransformToVisualSpace(rect);
				CaretElement.AddGeometry(ref result, new RectangleGeometry(rect));
				i++;
			}
			if (i == num5 && flag2)
			{
				this.GetTightBoundingGeometryFromLineIndex(i, num2, num3, calculatedTextAlignment, num, ref result);
			}
			return result;
		}

		// Token: 0x06005EE3 RID: 24291 RVA: 0x00291F0C File Offset: 0x00290F0C
		ITextPointer ITextView.GetPositionAtNextLine(ITextPointer position, double suggestedX, int count, out double newSuggestedX, out int linesMoved)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			newSuggestedX = suggestedX;
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			int num = Math.Max(0, Math.Min(this._lineMetrics.Count - 1, lineIndexFromPosition + count));
			linesMoved = num - lineIndexFromPosition;
			ITextPointer textPointer;
			if (linesMoved == 0)
			{
				textPointer = position.GetFrozenPointer(position.LogicalDirection);
			}
			else if (DoubleUtil.IsNaN(suggestedX))
			{
				textPointer = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[lineIndexFromPosition + linesMoved].Offset, LogicalDirection.Forward);
			}
			else
			{
				suggestedX -= this.GetTextAlignmentCorrection(this.CalculatedTextAlignment, this.GetWrappingWidth(base.RenderSize.Width));
				textPointer = this.GetTextPositionFromDistance(num, suggestedX);
			}
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06005EE4 RID: 24292 RVA: 0x00291FD7 File Offset: 0x00290FD7
		ITextPointer ITextView.GetPositionAtNextPage(ITextPointer position, Point suggestedOffset, int count, out Point newSuggestedOffset, out int pagesMoved)
		{
			Invariant.Assert(false);
			newSuggestedOffset = default(Point);
			pagesMoved = 0;
			return null;
		}

		// Token: 0x06005EE5 RID: 24293 RVA: 0x00291FEC File Offset: 0x00290FEC
		bool ITextView.IsAtCaretUnitBoundary(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			bool result = false;
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			CharacterHit charHit = default(CharacterHit);
			if (position.LogicalDirection == LogicalDirection.Forward)
			{
				charHit = new CharacterHit(position.Offset, 0);
			}
			else if (position.LogicalDirection == LogicalDirection.Backward)
			{
				if (position.Offset <= this._lineMetrics[lineIndexFromPosition].Offset)
				{
					return false;
				}
				charHit = new CharacterHit(position.Offset - 1, 1);
			}
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromPosition))
			{
				result = formattedLine.IsAtCaretCharacterHit(charHit);
			}
			return result;
		}

		// Token: 0x06005EE6 RID: 24294 RVA: 0x002920A0 File Offset: 0x002910A0
		ITextPointer ITextView.GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			if (position.Offset == 0 && direction == LogicalDirection.Backward)
			{
				return position.GetFrozenPointer(LogicalDirection.Forward);
			}
			if (position.Offset == this._host.TextContainer.SymbolCount && direction == LogicalDirection.Forward)
			{
				return position.GetFrozenPointer(LogicalDirection.Backward);
			}
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			CharacterHit index = new CharacterHit(position.Offset, 0);
			CharacterHit characterHit;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromPosition))
			{
				if (direction == LogicalDirection.Forward)
				{
					characterHit = formattedLine.GetNextCaretCharacterHit(index);
				}
				else
				{
					characterHit = formattedLine.GetPreviousCaretCharacterHit(index);
				}
			}
			LogicalDirection direction2;
			if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == this._lineMetrics[lineIndexFromPosition].EndOffset && direction == LogicalDirection.Forward)
			{
				if (lineIndexFromPosition == this._lineMetrics.Count - 1)
				{
					direction2 = LogicalDirection.Backward;
				}
				else
				{
					direction2 = LogicalDirection.Forward;
				}
			}
			else if (characterHit.FirstCharacterIndex + characterHit.TrailingLength == this._lineMetrics[lineIndexFromPosition].Offset && direction == LogicalDirection.Backward)
			{
				if (lineIndexFromPosition == 0)
				{
					direction2 = LogicalDirection.Forward;
				}
				else
				{
					direction2 = LogicalDirection.Backward;
				}
			}
			else
			{
				direction2 = ((characterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			ITextPointer textPointer = this._host.TextContainer.CreatePointerAtOffset(characterHit.FirstCharacterIndex + characterHit.TrailingLength, direction2);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06005EE7 RID: 24295 RVA: 0x002921F4 File Offset: 0x002911F4
		ITextPointer ITextView.GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			if (position.Offset == 0)
			{
				return position.GetFrozenPointer(LogicalDirection.Forward);
			}
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position, LogicalDirection.Backward);
			CharacterHit index = new CharacterHit(position.Offset, 0);
			CharacterHit backspaceCaretCharacterHit;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndexFromPosition))
			{
				backspaceCaretCharacterHit = formattedLine.GetBackspaceCaretCharacterHit(index);
			}
			LogicalDirection direction;
			if (backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength == this._lineMetrics[lineIndexFromPosition].Offset)
			{
				if (lineIndexFromPosition == 0)
				{
					direction = LogicalDirection.Forward;
				}
				else
				{
					direction = LogicalDirection.Backward;
				}
			}
			else
			{
				direction = ((backspaceCaretCharacterHit.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			ITextPointer textPointer = this._host.TextContainer.CreatePointerAtOffset(backspaceCaretCharacterHit.FirstCharacterIndex + backspaceCaretCharacterHit.TrailingLength, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06005EE8 RID: 24296 RVA: 0x002922D4 File Offset: 0x002912D4
		TextSegment ITextView.GetLineRange(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			Invariant.Assert(this.Contains(position));
			int lineIndexFromPosition = this.GetLineIndexFromPosition(position);
			ITextPointer startPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[lineIndexFromPosition].Offset, LogicalDirection.Forward);
			ITextPointer endPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[lineIndexFromPosition].Offset + this._lineMetrics[lineIndexFromPosition].ContentLength, LogicalDirection.Forward);
			return new TextSegment(startPosition, endPosition, true);
		}

		// Token: 0x06005EE9 RID: 24297 RVA: 0x0029235E File Offset: 0x0029135E
		ReadOnlyCollection<GlyphRun> ITextView.GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(false);
			return null;
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x00292367 File Offset: 0x00291367
		bool ITextView.Contains(ITextPointer position)
		{
			return this.Contains(position);
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		void ITextView.BringPositionIntoViewAsync(ITextPointer position, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06005EEC RID: 24300 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		void ITextView.BringPointIntoViewAsync(Point point, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06005EED RID: 24301 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		void ITextView.BringLineIntoViewAsync(ITextPointer position, double suggestedX, int count, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06005EEE RID: 24302 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		void ITextView.BringPageIntoViewAsync(ITextPointer position, Point suggestedOffset, int count, object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06005EEF RID: 24303 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		void ITextView.CancelAsync(object userState)
		{
			Invariant.Assert(false);
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x00292370 File Offset: 0x00291370
		bool ITextView.Validate()
		{
			base.UpdateLayout();
			return this.IsLayoutValid;
		}

		// Token: 0x06005EF1 RID: 24305 RVA: 0x0029237E File Offset: 0x0029137E
		bool ITextView.Validate(Point point)
		{
			return ((ITextView)this).Validate();
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x00292388 File Offset: 0x00291388
		bool ITextView.Validate(ITextPointer position)
		{
			if (position.TextContainer != this._host.TextContainer)
			{
				return false;
			}
			if (!this.IsLayoutValid)
			{
				base.UpdateLayout();
				if (!this.IsLayoutValid)
				{
					return false;
				}
			}
			int num = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
			while (!this.Contains(position))
			{
				base.InvalidateMeasure();
				base.UpdateLayout();
				if (!this.IsLayoutValid)
				{
					break;
				}
				int endOffset = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
				if (num >= endOffset)
				{
					break;
				}
				num = endOffset;
			}
			return this.IsLayoutValid && this.Contains(position);
		}

		// Token: 0x06005EF3 RID: 24307 RVA: 0x00292434 File Offset: 0x00291434
		void ITextView.ThrottleBackgroundTasksForUserInput()
		{
			if (this._throttleBackgroundTimer == null)
			{
				this._throttleBackgroundTimer = new DispatcherTimer(DispatcherPriority.Background);
				this._throttleBackgroundTimer.Interval = new TimeSpan(0, 0, 2);
				this._throttleBackgroundTimer.Tick += this.OnThrottleBackgroundTimeout;
			}
			else
			{
				this._throttleBackgroundTimer.Stop();
			}
			this._throttleBackgroundTimer.Start();
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x00292497 File Offset: 0x00291497
		internal void Remeasure()
		{
			if (this._lineMetrics != null)
			{
				this._lineMetrics.Clear();
				this._viewportLineVisuals = null;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x06005EF5 RID: 24309 RVA: 0x002924B9 File Offset: 0x002914B9
		internal void Rerender()
		{
			this._viewportLineVisuals = null;
			base.InvalidateArrange();
		}

		// Token: 0x06005EF6 RID: 24310 RVA: 0x002924C8 File Offset: 0x002914C8
		internal int GetLineIndexFromOffset(int offset)
		{
			int num = 0;
			int num2 = this._lineMetrics.Count;
			Invariant.Assert(this._lineMetrics.Count >= 1);
			int num3;
			TextBoxView.LineRecord lineRecord;
			for (;;)
			{
				Invariant.Assert(num < num2, "Couldn't find offset!");
				num3 = num + (num2 - num) / 2;
				lineRecord = this._lineMetrics[num3];
				if (offset < lineRecord.Offset)
				{
					num2 = num3;
				}
				else
				{
					if (offset <= lineRecord.EndOffset)
					{
						break;
					}
					num = num3 + 1;
				}
			}
			if (offset == lineRecord.EndOffset && num3 < this._lineMetrics.Count - 1)
			{
				num3++;
			}
			return num3;
		}

		// Token: 0x06005EF7 RID: 24311 RVA: 0x00292558 File Offset: 0x00291558
		internal void RemoveTextContainerListeners()
		{
			if (!this.CheckFlags(TextBoxView.Flags.TextContainerListenersInitialized))
			{
				return;
			}
			this._host.TextContainer.Changing -= this.OnTextContainerChanging;
			this._host.TextContainer.Change -= this.OnTextContainerChange;
			this._host.TextContainer.Highlights.Changed -= this.OnHighlightChanged;
			this.SetFlags(false, TextBoxView.Flags.TextContainerListenersInitialized);
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x06005EF8 RID: 24312 RVA: 0x002925D0 File Offset: 0x002915D0
		internal ITextBoxViewHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x170015EE RID: 5614
		// (get) Token: 0x06005EF9 RID: 24313 RVA: 0x000F93D3 File Offset: 0x000F83D3
		UIElement ITextView.RenderScope
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170015EF RID: 5615
		// (get) Token: 0x06005EFA RID: 24314 RVA: 0x002925D8 File Offset: 0x002915D8
		ITextContainer ITextView.TextContainer
		{
			get
			{
				return this._host.TextContainer;
			}
		}

		// Token: 0x170015F0 RID: 5616
		// (get) Token: 0x06005EFB RID: 24315 RVA: 0x002925E5 File Offset: 0x002915E5
		bool ITextView.IsValid
		{
			get
			{
				return this.IsLayoutValid;
			}
		}

		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x06005EFC RID: 24316 RVA: 0x002925ED File Offset: 0x002915ED
		bool ITextView.RendersOwnSelection
		{
			get
			{
				return !FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering;
			}
		}

		// Token: 0x170015F2 RID: 5618
		// (get) Token: 0x06005EFD RID: 24317 RVA: 0x002925F8 File Offset: 0x002915F8
		ReadOnlyCollection<TextSegment> ITextView.TextSegments
		{
			get
			{
				List<TextSegment> list = new List<TextSegment>(1);
				if (this._lineMetrics != null)
				{
					ITextPointer startPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[0].Offset, LogicalDirection.Backward);
					ITextPointer endPosition = this._host.TextContainer.CreatePointerAtOffset(this._lineMetrics[this._lineMetrics.Count - 1].EndOffset, LogicalDirection.Forward);
					list.Add(new TextSegment(startPosition, endPosition, true));
				}
				return new ReadOnlyCollection<TextSegment>(list);
			}
		}

		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06005EFE RID: 24318 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		// (remove) Token: 0x06005EFF RID: 24319 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		event BringPositionIntoViewCompletedEventHandler ITextView.BringPositionIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x140000DA RID: 218
		// (add) Token: 0x06005F00 RID: 24320 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		// (remove) Token: 0x06005F01 RID: 24321 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		event BringPointIntoViewCompletedEventHandler ITextView.BringPointIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x140000DB RID: 219
		// (add) Token: 0x06005F02 RID: 24322 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		// (remove) Token: 0x06005F03 RID: 24323 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		event BringLineIntoViewCompletedEventHandler ITextView.BringLineIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x140000DC RID: 220
		// (add) Token: 0x06005F04 RID: 24324 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		// (remove) Token: 0x06005F05 RID: 24325 RVA: 0x0011E8AA File Offset: 0x0011D8AA
		event BringPageIntoViewCompletedEventHandler ITextView.BringPageIntoViewCompleted
		{
			add
			{
				Invariant.Assert(false);
			}
			remove
			{
				Invariant.Assert(false);
			}
		}

		// Token: 0x140000DD RID: 221
		// (add) Token: 0x06005F06 RID: 24326 RVA: 0x0029267A File Offset: 0x0029167A
		// (remove) Token: 0x06005F07 RID: 24327 RVA: 0x00292693 File Offset: 0x00291693
		event EventHandler Updated;

		// Token: 0x06005F08 RID: 24328 RVA: 0x002926AC File Offset: 0x002916AC
		private void EnsureTextContainerListeners()
		{
			if (this.CheckFlags(TextBoxView.Flags.TextContainerListenersInitialized))
			{
				return;
			}
			this._host.TextContainer.Changing += this.OnTextContainerChanging;
			this._host.TextContainer.Change += this.OnTextContainerChange;
			this._host.TextContainer.Highlights.Changed += this.OnHighlightChanged;
			this.SetFlags(true, TextBoxView.Flags.TextContainerListenersInitialized);
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x00292724 File Offset: 0x00291724
		private void EnsureCache()
		{
			if (this._cache == null)
			{
				this._cache = new TextBoxView.TextCache(this);
			}
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x0029273C File Offset: 0x0029173C
		private LineProperties GetLineProperties()
		{
			TextProperties defaultTextProperties = new TextProperties((Control)this._host, this._host.IsTypographyDefaultValue);
			return new LineProperties((Control)this._host, (Control)this._host, defaultTextProperties, null, this.CalculatedTextAlignment);
		}

		// Token: 0x06005F0B RID: 24331 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private void OnTextContainerChanging(object sender, EventArgs args)
		{
		}

		// Token: 0x06005F0C RID: 24332 RVA: 0x00292788 File Offset: 0x00291788
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			if (args.Count == 0)
			{
				return;
			}
			if (this._dirtyList == null)
			{
				this._dirtyList = new DtrList();
			}
			DirtyTextRange dtr = new DirtyTextRange(args);
			this._dirtyList.Merge(dtr);
			base.InvalidateMeasure();
		}

		// Token: 0x06005F0D RID: 24333 RVA: 0x002927CC File Offset: 0x002917CC
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			if (args.OwnerType != typeof(SpellerHighlightLayer) && (!((ITextView)this).RendersOwnSelection || args.OwnerType != typeof(TextSelection)))
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (this._dirtyList == null)
			{
				this._dirtyList = new DtrList();
			}
			DtrList dtrList = new DtrList();
			foreach (object obj in args.Ranges)
			{
				TextSegment textSegment = (TextSegment)obj;
				int num = textSegment.End.Offset - textSegment.Start.Offset;
				DirtyTextRange dtr = new DirtyTextRange(textSegment.Start.Offset, num, num, true);
				dtrList.Merge(dtr);
			}
			DirtyTextRange mergedRange = dtrList.GetMergedRange();
			if (args.OwnerType == typeof(TextSelection))
			{
				this.HandleTextSelectionHighlightChange(mergedRange, ref flag2, ref flag);
			}
			else if (args.OwnerType == typeof(SpellerHighlightLayer))
			{
				this._dirtyList.Merge(mergedRange);
				flag = true;
			}
			if (flag)
			{
				base.InvalidateMeasure();
				return;
			}
			if (flag2)
			{
				base.InvalidateArrange();
			}
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x00292918 File Offset: 0x00291918
		private void HandleTextSelectionHighlightChange(DirtyTextRange currentSelectionRange, ref bool arrangeNeeded, ref bool measureNeeded)
		{
			if (this._lineMetrics.Count == 0)
			{
				measureNeeded = true;
				return;
			}
			if (this._dirtyList.Length > 0 && this._dirtyList.DtrsFromRange(currentSelectionRange.StartIndex, currentSelectionRange.PositionsAdded) != null)
			{
				this._dirtyList.Merge(currentSelectionRange);
				measureNeeded = true;
				return;
			}
			int[] array = new int[]
			{
				currentSelectionRange.StartIndex,
				currentSelectionRange.StartIndex + currentSelectionRange.PositionsAdded
			};
			using (TextBoxLine textBoxLine = new TextBoxLine(this))
			{
				DependencyObject element = (Control)this._host;
				LineProperties lineProperties = this.GetLineProperties();
				TextFormatter formatter = TextFormatter.FromCurrentDispatcher(TextOptions.GetTextFormattingMode(element));
				double wrappingWidth = this.GetWrappingWidth(base.RenderSize.Width);
				double wrappingWidth2 = this.GetWrappingWidth(this._previousConstraint.Width);
				foreach (int offset in array)
				{
					int lineIndexFromOffset = this.GetLineIndexFromOffset(offset);
					TextBoxView.LineRecord lineRecord = this._lineMetrics[lineIndexFromOffset];
					textBoxLine.Format(lineRecord.Offset, wrappingWidth2, wrappingWidth, lineProperties, new TextRunCache(), formatter);
					if (lineRecord.Length != textBoxLine.Length)
					{
						measureNeeded = true;
						this._dirtyList.Merge(new DirtyTextRange(lineRecord.Offset, lineRecord.Length, lineRecord.Length, true));
					}
				}
			}
			if (!measureNeeded)
			{
				DirtyTextRange? selectionRenderRange = this.GetSelectionRenderRange(currentSelectionRange);
				if (selectionRenderRange != null)
				{
					this._dirtyList.Merge(selectionRenderRange.Value);
					arrangeNeeded = true;
					this.SetFlags(true, TextBoxView.Flags.ArrangePendingFromHighlightLayer);
					return;
				}
				if (this._dirtyList.Length == 0)
				{
					this._dirtyList = null;
				}
			}
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x00292ACC File Offset: 0x00291ACC
		private void SetFlags(bool value, TextBoxView.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x00292AEA File Offset: 0x00291AEA
		private bool CheckFlags(TextBoxView.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x00292AF7 File Offset: 0x00291AF7
		private void FireTextViewUpdatedEvent()
		{
			if (this.UpdatedEvent != null)
			{
				this.UpdatedEvent(this, EventArgs.Empty);
			}
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x00292B14 File Offset: 0x00291B14
		private int GetLineIndexFromPoint(Point point, bool snapToText)
		{
			Invariant.Assert(this._lineMetrics.Count >= 1);
			if (point.Y < 0.0)
			{
				if (!snapToText)
				{
					return -1;
				}
				return 0;
			}
			else if (point.Y >= this._lineHeight * (double)this._lineMetrics.Count)
			{
				if (!snapToText)
				{
					return -1;
				}
				return this._lineMetrics.Count - 1;
			}
			else
			{
				int num = -1;
				int i = 0;
				int num2 = this._lineMetrics.Count;
				while (i < num2)
				{
					num = i + (num2 - i) / 2;
					TextBoxView.LineRecord lineRecord = this._lineMetrics[num];
					double num3 = this._lineHeight * (double)num;
					if (point.Y < num3)
					{
						num2 = num;
					}
					else if (point.Y >= num3 + this._lineHeight)
					{
						i = num + 1;
					}
					else
					{
						if (!snapToText && (point.X < 0.0 || point.X >= lineRecord.Width))
						{
							num = -1;
							break;
						}
						break;
					}
				}
				if (i >= num2)
				{
					return -1;
				}
				return num;
			}
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x00292C0B File Offset: 0x00291C0B
		private int GetLineIndexFromPosition(ITextPointer position)
		{
			return this.GetLineIndexFromOffset(position.Offset, position.LogicalDirection);
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x00292C1F File Offset: 0x00291C1F
		private int GetLineIndexFromPosition(ITextPointer position, LogicalDirection direction)
		{
			return this.GetLineIndexFromOffset(position.Offset, direction);
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x00292C2E File Offset: 0x00291C2E
		private int GetLineIndexFromOffset(int offset, LogicalDirection direction)
		{
			if (offset > 0 && direction == LogicalDirection.Backward)
			{
				offset--;
			}
			return this.GetLineIndexFromOffset(offset);
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x00292C44 File Offset: 0x00291C44
		private TextBoxLine GetFormattedLine(int lineIndex)
		{
			LineProperties lineProperties;
			return this.GetFormattedLine(lineIndex, out lineProperties);
		}

		// Token: 0x06005F17 RID: 24343 RVA: 0x00292C5C File Offset: 0x00291C5C
		private TextBoxLine GetFormattedLine(int lineIndex, out LineProperties lineProperties)
		{
			TextBoxLine textBoxLine = new TextBoxLine(this);
			TextBoxView.LineRecord lineRecord = this._lineMetrics[lineIndex];
			lineProperties = this.GetLineProperties();
			TextFormatter formatter = TextFormatter.FromCurrentDispatcher(TextOptions.GetTextFormattingMode((Control)this._host));
			double wrappingWidth = this.GetWrappingWidth(base.RenderSize.Width);
			double wrappingWidth2 = this.GetWrappingWidth(this._previousConstraint.Width);
			textBoxLine.Format(lineRecord.Offset, wrappingWidth2, wrappingWidth, lineProperties, new TextRunCache(), formatter);
			Invariant.Assert(lineRecord.Length == textBoxLine.Length, "Line is out of sync with metrics!");
			return textBoxLine;
		}

		// Token: 0x06005F18 RID: 24344 RVA: 0x00292CF4 File Offset: 0x00291CF4
		private ITextPointer GetTextPositionFromDistance(int lineIndex, double x)
		{
			LineProperties lineProperties;
			CharacterHit textPositionFromDistance;
			LogicalDirection direction;
			using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndex, out lineProperties))
			{
				textPositionFromDistance = formattedLine.GetTextPositionFromDistance(x);
				direction = ((textPositionFromDistance.TrailingLength > 0) ? LogicalDirection.Backward : LogicalDirection.Forward);
			}
			return this._host.TextContainer.CreatePointerAtOffset(textPositionFromDistance.FirstCharacterIndex + textPositionFromDistance.TrailingLength, direction);
		}

		// Token: 0x06005F19 RID: 24345 RVA: 0x00292D60 File Offset: 0x00291D60
		private void ArrangeScrollData(Size arrangeSize)
		{
			if (this._scrollData == null)
			{
				return;
			}
			bool flag = false;
			if (!DoubleUtil.AreClose(this._scrollData.Viewport, arrangeSize))
			{
				this._scrollData.Viewport = arrangeSize;
				flag = true;
			}
			if (!DoubleUtil.AreClose(this._scrollData.Extent, this._contentSize))
			{
				this._scrollData.Extent = this._contentSize;
				flag = true;
			}
			Vector vector = new Vector(Math.Max(0.0, Math.Min(this._scrollData.ExtentWidth - this._scrollData.ViewportWidth, this._scrollData.HorizontalOffset)), Math.Max(0.0, Math.Min(this._scrollData.ExtentHeight - this._scrollData.ViewportHeight, this._scrollData.VerticalOffset)));
			if (!DoubleUtil.AreClose(vector, this._scrollData.Offset))
			{
				this._scrollData.Offset = vector;
				flag = true;
			}
			if (flag && this._scrollData.ScrollOwner != null)
			{
				this._scrollData.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x00292E78 File Offset: 0x00291E78
		private void ArrangeVisuals(Size arrangeSize)
		{
			Invariant.Assert(this.CheckFlags(TextBoxView.Flags.ArrangePendingFromHighlightLayer) || this._dirtyList == null);
			this.SetFlags(false, TextBoxView.Flags.ArrangePendingFromHighlightLayer);
			if (this._dirtyList != null)
			{
				this.InvalidateDirtyVisuals();
				this._dirtyList = null;
			}
			if (this._visualChildren == null)
			{
				this._visualChildren = new List<TextBoxLineDrawingVisual>(1);
			}
			this.EnsureCache();
			LineProperties lineProperties = this._cache.LineProperties;
			TextBoxLine textBoxLine = new TextBoxLine(this);
			int num;
			int num2;
			this.GetVisibleLines(out num, out num2);
			this.SetViewportLines(num, num2);
			double wrappingWidth = this.GetWrappingWidth(arrangeSize.Width);
			double num3 = this.GetTextAlignmentCorrection(lineProperties.TextAlignment, wrappingWidth);
			double num4 = this.VerticalAlignmentOffset;
			if (this._scrollData != null)
			{
				num3 -= this._scrollData.HorizontalOffset;
				num4 -= this._scrollData.VerticalOffset;
			}
			this.DetachDiscardedVisualChildren();
			double wrappingWidth2 = this.GetWrappingWidth(this._previousConstraint.Width);
			double endOfParaGlyphWidth = ((Control)this._host).FontSize * 0.5;
			bool flag = ((ITextView)this).RendersOwnSelection && ((bool)((Control)this._host).GetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty) || (bool)((Control)this._host).GetValue(TextBoxBase.IsSelectionActiveProperty));
			for (int i = num; i <= num2; i++)
			{
				TextBoxLineDrawingVisual textBoxLineDrawingVisual = this.GetLineVisual(i);
				if (textBoxLineDrawingVisual == null)
				{
					TextBoxView.LineRecord lineRecord = this._lineMetrics[i];
					using (textBoxLine)
					{
						textBoxLine.Format(lineRecord.Offset, wrappingWidth2, wrappingWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
						if (!this.IsBackgroundLayoutPending)
						{
							Invariant.Assert(lineRecord.Length == textBoxLine.Length, "Line is out of sync with metrics!");
						}
						Geometry selectionGeometry = null;
						if (flag)
						{
							ITextSelection textSelection = this._host.TextContainer.TextSelection;
							if (!textSelection.IsEmpty)
							{
								this.GetTightBoundingGeometryFromLineIndexForSelection(textBoxLine, i, textSelection.Start.CharOffset, textSelection.End.CharOffset, this.CalculatedTextAlignment, endOfParaGlyphWidth, ref selectionGeometry);
							}
						}
						textBoxLineDrawingVisual = textBoxLine.CreateVisual(selectionGeometry);
					}
					this.SetLineVisual(i, textBoxLineDrawingVisual);
					this.AttachVisualChild(textBoxLineDrawingVisual);
				}
				textBoxLineDrawingVisual.Offset = new Vector(num3, num4 + (double)i * this._lineHeight);
			}
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x002930EC File Offset: 0x002920EC
		private void InvalidateDirtyVisuals()
		{
			for (int i = 0; i < this._dirtyList.Length; i++)
			{
				DirtyTextRange dirtyTextRange = this._dirtyList[i];
				Invariant.Assert(dirtyTextRange.FromHighlightLayer);
				Invariant.Assert(dirtyTextRange.PositionsAdded == dirtyTextRange.PositionsRemoved);
				int lineIndexFromOffset = this.GetLineIndexFromOffset(dirtyTextRange.StartIndex, LogicalDirection.Forward);
				int offset = Math.Min(dirtyTextRange.StartIndex + dirtyTextRange.PositionsAdded, this._host.TextContainer.SymbolCount);
				int lineIndexFromOffset2 = this.GetLineIndexFromOffset(offset, LogicalDirection.Backward);
				for (int j = lineIndexFromOffset; j <= lineIndexFromOffset2; j++)
				{
					this.ClearLineVisual(j);
				}
			}
		}

		// Token: 0x06005F1C RID: 24348 RVA: 0x00293198 File Offset: 0x00292198
		private void DetachDiscardedVisualChildren()
		{
			int num = this._visualChildren.Count - 1;
			for (int i = this._visualChildren.Count - 1; i >= 0; i--)
			{
				if (this._visualChildren[i] == null || this._visualChildren[i].DiscardOnArrange)
				{
					base.RemoveVisualChild(this._visualChildren[i]);
					if (i < num)
					{
						this._visualChildren[i] = this._visualChildren[num];
					}
					num--;
				}
			}
			if (num < this._visualChildren.Count - 1)
			{
				this._visualChildren.RemoveRange(num + 1, this._visualChildren.Count - num - 1);
			}
		}

		// Token: 0x06005F1D RID: 24349 RVA: 0x0029324B File Offset: 0x0029224B
		private void AttachVisualChild(TextBoxLineDrawingVisual lineVisual)
		{
			lineVisual._parentIndex = this._visualChildren.Count;
			base.AddVisualChild(lineVisual);
			this._visualChildren.Add(lineVisual);
		}

		// Token: 0x06005F1E RID: 24350 RVA: 0x00293274 File Offset: 0x00292274
		private void ClearVisualChildren()
		{
			for (int i = 0; i < this._visualChildren.Count; i++)
			{
				base.RemoveVisualChild(this._visualChildren[i]);
			}
			this._visualChildren.Clear();
		}

		// Token: 0x06005F1F RID: 24351 RVA: 0x002932B4 File Offset: 0x002922B4
		private Point TransformToDocumentSpace(Point point)
		{
			if (this._scrollData != null)
			{
				point = new Point(point.X + this._scrollData.HorizontalOffset, point.Y + this._scrollData.VerticalOffset);
			}
			point.X -= this.GetTextAlignmentCorrection(this.CalculatedTextAlignment, this.GetWrappingWidth(base.RenderSize.Width));
			point.Y -= this.VerticalAlignmentOffset;
			return point;
		}

		// Token: 0x06005F20 RID: 24352 RVA: 0x0029333C File Offset: 0x0029233C
		private Rect TransformToVisualSpace(Rect rect)
		{
			if (this._scrollData != null)
			{
				rect.X -= this._scrollData.HorizontalOffset;
				rect.Y -= this._scrollData.VerticalOffset;
			}
			rect.X += this.GetTextAlignmentCorrection(this.CalculatedTextAlignment, this.GetWrappingWidth(base.RenderSize.Width));
			rect.Y += this.VerticalAlignmentOffset;
			return rect;
		}

		// Token: 0x06005F21 RID: 24353 RVA: 0x002933C8 File Offset: 0x002923C8
		private void GetTightBoundingGeometryFromLineIndex(int lineIndex, int unclippedStartOffset, int unclippedEndOffset, TextAlignment alignment, double endOfParaGlyphWidth, ref Geometry geometry)
		{
			int num = Math.Max(this._lineMetrics[lineIndex].Offset, unclippedStartOffset);
			int num2 = Math.Min(this._lineMetrics[lineIndex].EndOffset, unclippedEndOffset);
			if (num == num2)
			{
				if (unclippedStartOffset != this._lineMetrics[lineIndex].EndOffset)
				{
					Invariant.Assert(num2 == this._lineMetrics[lineIndex].Offset || num2 == this._lineMetrics[lineIndex].Offset + this._lineMetrics[lineIndex].ContentLength);
					return;
				}
				if (TextPointerBase.IsNextToPlainLineBreak(this._host.TextContainer.CreatePointerAtOffset(unclippedStartOffset, LogicalDirection.Backward), LogicalDirection.Backward))
				{
					Rect rect = new Rect(0.0, (double)lineIndex * this._lineHeight, endOfParaGlyphWidth, this._lineHeight);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect));
					return;
				}
			}
			else
			{
				IList<Rect> rangeBounds;
				using (TextBoxLine formattedLine = this.GetFormattedLine(lineIndex))
				{
					rangeBounds = formattedLine.GetRangeBounds(num, num2 - num, 0.0, (double)lineIndex * this._lineHeight);
				}
				for (int i = 0; i < rangeBounds.Count; i++)
				{
					Rect rect2 = this.TransformToVisualSpace(rangeBounds[i]);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect2));
				}
				if (unclippedEndOffset >= this._lineMetrics[lineIndex].EndOffset && TextPointerBase.IsNextToPlainLineBreak(this._host.TextContainer.CreatePointerAtOffset(num2, LogicalDirection.Backward), LogicalDirection.Backward))
				{
					double contentOffset = this.GetContentOffset(this._lineMetrics[lineIndex].Width, alignment);
					Rect rect3 = new Rect(contentOffset + this._lineMetrics[lineIndex].Width, (double)lineIndex * this._lineHeight, endOfParaGlyphWidth, this._lineHeight);
					rect3 = this.TransformToVisualSpace(rect3);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect3));
				}
			}
		}

		// Token: 0x06005F22 RID: 24354 RVA: 0x002935B4 File Offset: 0x002925B4
		private void GetTightBoundingGeometryFromLineIndexForSelection(TextBoxLine line, int lineIndex, int unclippedStartOffset, int unclippedEndOffset, TextAlignment alignment, double endOfParaGlyphWidth, ref Geometry geometry)
		{
			int offset = this._lineMetrics[lineIndex].Offset;
			int endOffset = this._lineMetrics[lineIndex].EndOffset;
			if (offset > unclippedEndOffset || endOffset <= unclippedStartOffset)
			{
				return;
			}
			int num = Math.Max(offset, unclippedStartOffset);
			int num2 = Math.Min(endOffset, unclippedEndOffset);
			if (num == num2)
			{
				if (unclippedStartOffset != this._lineMetrics[lineIndex].EndOffset)
				{
					Invariant.Assert(num2 == this._lineMetrics[lineIndex].Offset || num2 == this._lineMetrics[lineIndex].Offset + this._lineMetrics[lineIndex].ContentLength);
					return;
				}
				if (TextPointerBase.IsNextToPlainLineBreak(this._host.TextContainer.CreatePointerAtOffset(unclippedStartOffset, LogicalDirection.Backward), LogicalDirection.Backward))
				{
					Rect rect = new Rect(0.0, 0.0, endOfParaGlyphWidth, this._lineHeight);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect));
					return;
				}
			}
			else
			{
				IList<Rect> rangeBounds = line.GetRangeBounds(num, num2 - num, 0.0, 0.0);
				for (int i = 0; i < rangeBounds.Count; i++)
				{
					Rect rect2 = rangeBounds[i];
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect2));
				}
				if (unclippedEndOffset >= this._lineMetrics[lineIndex].EndOffset && TextPointerBase.IsNextToPlainLineBreak(this._host.TextContainer.CreatePointerAtOffset(num2, LogicalDirection.Backward), LogicalDirection.Backward))
				{
					double contentOffset = this.GetContentOffset(this._lineMetrics[lineIndex].Width, alignment);
					Rect rect3 = new Rect(contentOffset + this._lineMetrics[lineIndex].Width, 0.0, endOfParaGlyphWidth, this._lineHeight);
					CaretElement.AddGeometry(ref geometry, new RectangleGeometry(rect3));
				}
			}
		}

		// Token: 0x06005F23 RID: 24355 RVA: 0x00293780 File Offset: 0x00292780
		private void GetVisibleLines(out int firstLineIndex, out int lastLineIndex)
		{
			Rect viewport = this.Viewport;
			if (!viewport.IsEmpty)
			{
				firstLineIndex = (int)(viewport.Y / this._lineHeight);
				lastLineIndex = (int)Math.Ceiling((viewport.Y + viewport.Height) / this._lineHeight) - 1;
				firstLineIndex = Math.Max(0, Math.Min(firstLineIndex, this._lineMetrics.Count - 1));
				lastLineIndex = Math.Max(0, Math.Min(lastLineIndex, this._lineMetrics.Count - 1));
				return;
			}
			firstLineIndex = 0;
			lastLineIndex = this._lineMetrics.Count - 1;
		}

		// Token: 0x06005F24 RID: 24356 RVA: 0x0029381C File Offset: 0x0029281C
		private Size FullMeasureTick(double constraintWidth, LineProperties lineProperties)
		{
			TextBoxLine textBoxLine = new TextBoxLine(this);
			Size result;
			int num;
			if (this._lineMetrics.Count == 0)
			{
				result = default(Size);
				num = 0;
			}
			else
			{
				result = this._contentSize;
				num = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
			}
			DateTime t;
			if ((ScrollBarVisibility)((Control)this._host).GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty) == ScrollBarVisibility.Auto)
			{
				t = DateTime.MaxValue;
			}
			else
			{
				t = DateTime.Now.AddMilliseconds(200.0);
			}
			bool endOfParagraph;
			do
			{
				using (textBoxLine)
				{
					textBoxLine.Format(num, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
					this._lineHeight = lineProperties.CalcLineAdvance(textBoxLine.Height);
					this._lineMetrics.Add(new TextBoxView.LineRecord(num, textBoxLine));
					result.Width = Math.Max(result.Width, textBoxLine.Width);
					result.Height += this._lineHeight;
					num += textBoxLine.Length;
					endOfParagraph = textBoxLine.EndOfParagraph;
				}
			}
			while (!endOfParagraph && DateTime.Now < t);
			if (!endOfParagraph)
			{
				this.SetFlags(true, TextBoxView.Flags.BackgroundLayoutPending);
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnBackgroundMeasure), null);
			}
			else
			{
				this.SetFlags(false, TextBoxView.Flags.BackgroundLayoutPending);
			}
			return result;
		}

		// Token: 0x06005F25 RID: 24357 RVA: 0x00293994 File Offset: 0x00292994
		private object OnBackgroundMeasure(object o)
		{
			if (this._throttleBackgroundTimer == null)
			{
				base.InvalidateMeasure();
			}
			return null;
		}

		// Token: 0x06005F26 RID: 24358 RVA: 0x002939A8 File Offset: 0x002929A8
		private Size IncrementalMeasure(double constraintWidth, LineProperties lineProperties)
		{
			Invariant.Assert(this._dirtyList != null);
			Invariant.Assert(this._dirtyList.Length > 0);
			Size contentSize = this._contentSize;
			DirtyTextRange range = this._dirtyList[0];
			if (range.StartIndex > this._lineMetrics[this._lineMetrics.Count - 1].EndOffset)
			{
				Invariant.Assert(this.IsBackgroundLayoutPending);
				return contentSize;
			}
			int startIndex = range.StartIndex;
			int num = range.PositionsAdded;
			int num2 = range.PositionsRemoved;
			for (int i = 1; i < this._dirtyList.Length; i++)
			{
				range = this._dirtyList[i];
				if (range.StartIndex > this._lineMetrics[this._lineMetrics.Count - 1].EndOffset)
				{
					Invariant.Assert(this.IsBackgroundLayoutPending);
					break;
				}
				int num3 = range.StartIndex - startIndex;
				num += num3 + range.PositionsAdded;
				num2 += num3 + range.PositionsRemoved;
				startIndex = range.StartIndex;
			}
			range = new DirtyTextRange(this._dirtyList[0].StartIndex, num, num2, false);
			if (range.PositionsAdded >= range.PositionsRemoved)
			{
				this.IncrementalMeasureLinesAfterInsert(constraintWidth, lineProperties, range, ref contentSize);
			}
			else if (range.PositionsAdded < range.PositionsRemoved)
			{
				this.IncrementalMeasureLinesAfterDelete(constraintWidth, lineProperties, range, ref contentSize);
			}
			return contentSize;
		}

		// Token: 0x06005F27 RID: 24359 RVA: 0x00293B1C File Offset: 0x00292B1C
		private void IncrementalMeasureLinesAfterInsert(double constraintWidth, LineProperties lineProperties, DirtyTextRange range, ref Size desiredSize)
		{
			int num = range.PositionsAdded - range.PositionsRemoved;
			Invariant.Assert(num >= 0);
			int num2 = this.GetLineIndexFromOffset(range.StartIndex, LogicalDirection.Forward);
			if (num > 0)
			{
				for (int i = num2 + 1; i < this._lineMetrics.Count; i++)
				{
					this._lineMetrics[i].Offset += num;
				}
			}
			TextBoxLine textBoxLine = new TextBoxLine(this);
			bool flag = false;
			int num3;
			if (num2 > 0)
			{
				this.FormatFirstIncrementalLine(num2 - 1, constraintWidth, lineProperties, textBoxLine, out num3, out flag);
			}
			else
			{
				num3 = this._lineMetrics[num2].Offset;
			}
			if (!flag)
			{
				using (textBoxLine)
				{
					textBoxLine.Format(num3, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
					this._lineMetrics[num2] = new TextBoxView.LineRecord(num3, textBoxLine);
					num3 += textBoxLine.Length;
					flag = textBoxLine.EndOfParagraph;
				}
				this.ClearLineVisual(num2);
				num2++;
			}
			this.SyncLineMetrics(range, constraintWidth, lineProperties, textBoxLine, flag, num2, num3);
			desiredSize = this.BruteForceCalculateDesiredSize();
		}

		// Token: 0x06005F28 RID: 24360 RVA: 0x00293C50 File Offset: 0x00292C50
		private void IncrementalMeasureLinesAfterDelete(double constraintWidth, LineProperties lineProperties, DirtyTextRange range, ref Size desiredSize)
		{
			int num = range.PositionsAdded - range.PositionsRemoved;
			Invariant.Assert(num < 0);
			int lineIndexFromOffset = this.GetLineIndexFromOffset(range.StartIndex);
			int num2 = range.StartIndex + -num - 1;
			if (num2 > this._lineMetrics[this._lineMetrics.Count - 1].EndOffset)
			{
				Invariant.Assert(this.IsBackgroundLayoutPending);
				num2 = this._lineMetrics[this._lineMetrics.Count - 1].EndOffset;
				if (range.StartIndex == num2)
				{
					return;
				}
			}
			int lineIndexFromOffset2 = this.GetLineIndexFromOffset(num2);
			for (int i = lineIndexFromOffset2 + 1; i < this._lineMetrics.Count; i++)
			{
				this._lineMetrics[i].Offset += num;
			}
			TextBoxLine textBoxLine = new TextBoxLine(this);
			int num3 = lineIndexFromOffset;
			int num4;
			bool flag;
			if (num3 > 0)
			{
				this.FormatFirstIncrementalLine(num3 - 1, constraintWidth, lineProperties, textBoxLine, out num4, out flag);
			}
			else
			{
				num4 = this._lineMetrics[num3].Offset;
				flag = false;
			}
			if (!flag && (range.StartIndex > num4 || range.StartIndex + -num < this._lineMetrics[num3].EndOffset))
			{
				using (textBoxLine)
				{
					textBoxLine.Format(num4, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
					this._lineMetrics[num3] = new TextBoxView.LineRecord(num4, textBoxLine);
					num4 += textBoxLine.Length;
					flag = textBoxLine.EndOfParagraph;
				}
				this.ClearLineVisual(num3);
				num3++;
			}
			this._lineMetrics.RemoveRange(num3, lineIndexFromOffset2 - num3 + 1);
			this.RemoveLineVisualRange(num3, lineIndexFromOffset2 - num3 + 1);
			this.SyncLineMetrics(range, constraintWidth, lineProperties, textBoxLine, flag, num3, num4);
			desiredSize = this.BruteForceCalculateDesiredSize();
		}

		// Token: 0x06005F29 RID: 24361 RVA: 0x00293E4C File Offset: 0x00292E4C
		private void FormatFirstIncrementalLine(int lineIndex, double constraintWidth, LineProperties lineProperties, TextBoxLine line, out int lineOffset, out bool endOfParagraph)
		{
			int endOffset = this._lineMetrics[lineIndex].EndOffset;
			lineOffset = this._lineMetrics[lineIndex].Offset;
			try
			{
				line.Format(lineOffset, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
				this._lineMetrics[lineIndex] = new TextBoxView.LineRecord(lineOffset, line);
				lineOffset += line.Length;
				endOfParagraph = line.EndOfParagraph;
			}
			finally
			{
				if (line != null)
				{
					((IDisposable)line).Dispose();
				}
			}
			if (endOffset != this._lineMetrics[lineIndex].EndOffset)
			{
				this.ClearLineVisual(lineIndex);
			}
		}

		// Token: 0x06005F2A RID: 24362 RVA: 0x00293F0C File Offset: 0x00292F0C
		private void SyncLineMetrics(DirtyTextRange range, double constraintWidth, LineProperties lineProperties, TextBoxLine line, bool endOfParagraph, int lineIndex, int lineOffset)
		{
			bool flag = range.PositionsAdded == 0 || range.PositionsRemoved == 0;
			int num = range.StartIndex + Math.Max(range.PositionsAdded, range.PositionsRemoved);
			while (!endOfParagraph && (lineIndex == this._lineMetrics.Count || !flag || lineOffset != this._lineMetrics[lineIndex].Offset))
			{
				if (lineIndex < this._lineMetrics.Count && lineOffset >= this._lineMetrics[lineIndex].EndOffset)
				{
					this._lineMetrics.RemoveAt(lineIndex);
					this.RemoveLineVisualRange(lineIndex, 1);
				}
				else
				{
					try
					{
						line.Format(lineOffset, constraintWidth, constraintWidth, lineProperties, this._cache.TextRunCache, this._cache.TextFormatter);
						TextBoxView.LineRecord lineRecord = new TextBoxView.LineRecord(lineOffset, line);
						if (lineIndex == this._lineMetrics.Count || lineOffset + line.Length <= this._lineMetrics[lineIndex].Offset)
						{
							this._lineMetrics.Insert(lineIndex, lineRecord);
							this.AddLineVisualPlaceholder(lineIndex);
						}
						else
						{
							Invariant.Assert(lineOffset < this._lineMetrics[lineIndex].EndOffset);
							TextBoxView.LineRecord lineRecord2 = this._lineMetrics[lineIndex];
							if (range.FromHighlightLayer && lineRecord2.Offset > num && lineRecord2.ContentLength == lineRecord.ContentLength && lineRecord2.EndOffset == lineRecord.EndOffset && lineRecord2.Length == lineRecord.Length && lineRecord2.Offset == lineRecord.Offset && DoubleUtilities.AreClose(lineRecord2.Width, lineRecord.Width))
							{
								break;
							}
							this._lineMetrics[lineIndex] = lineRecord;
							this.ClearLineVisual(lineIndex);
							flag |= (num <= lineRecord.EndOffset && line.HasLineBreak);
						}
						lineIndex++;
						lineOffset += line.Length;
						endOfParagraph = line.EndOfParagraph;
					}
					finally
					{
						if (line != null)
						{
							((IDisposable)line).Dispose();
						}
					}
				}
			}
			if (endOfParagraph && lineIndex < this._lineMetrics.Count)
			{
				int count = this._lineMetrics.Count - lineIndex;
				this._lineMetrics.RemoveRange(lineIndex, count);
				this.RemoveLineVisualRange(lineIndex, count);
			}
		}

		// Token: 0x06005F2B RID: 24363 RVA: 0x0029417C File Offset: 0x0029317C
		private Size BruteForceCalculateDesiredSize()
		{
			Size result = default(Size);
			for (int i = 0; i < this._lineMetrics.Count; i++)
			{
				result.Width = Math.Max(result.Width, this._lineMetrics[i].Width);
			}
			result.Height = (double)this._lineMetrics.Count * this._lineHeight;
			return result;
		}

		// Token: 0x06005F2C RID: 24364 RVA: 0x002941E8 File Offset: 0x002931E8
		private void SetViewportLines(int firstLineIndex, int lastLineIndex)
		{
			List<TextBoxLineDrawingVisual> viewportLineVisuals = this._viewportLineVisuals;
			int viewportLineVisualsIndex = this._viewportLineVisualsIndex;
			this._viewportLineVisuals = null;
			this._viewportLineVisualsIndex = -1;
			int num = lastLineIndex - firstLineIndex + 1;
			if (num <= 1)
			{
				this.ClearVisualChildren();
				return;
			}
			this._viewportLineVisuals = new List<TextBoxLineDrawingVisual>(num);
			this._viewportLineVisuals.AddRange(new TextBoxLineDrawingVisual[num]);
			this._viewportLineVisualsIndex = firstLineIndex;
			if (viewportLineVisuals == null)
			{
				this.ClearVisualChildren();
				return;
			}
			int num2 = viewportLineVisualsIndex + viewportLineVisuals.Count - 1;
			if (viewportLineVisualsIndex <= lastLineIndex && num2 >= firstLineIndex)
			{
				int num3 = Math.Max(viewportLineVisualsIndex, firstLineIndex);
				int num4 = Math.Min(num2, firstLineIndex + num - 1) - num3 + 1;
				for (int i = 0; i < num4; i++)
				{
					this._viewportLineVisuals[num3 - this._viewportLineVisualsIndex + i] = viewportLineVisuals[num3 - viewportLineVisualsIndex + i];
				}
				for (int j = 0; j < num3 - viewportLineVisualsIndex; j++)
				{
					if (viewportLineVisuals[j] != null)
					{
						viewportLineVisuals[j].DiscardOnArrange = true;
					}
				}
				for (int k = num3 - viewportLineVisualsIndex + num4; k < viewportLineVisuals.Count; k++)
				{
					if (viewportLineVisuals[k] != null)
					{
						viewportLineVisuals[k].DiscardOnArrange = true;
					}
				}
				return;
			}
			this.ClearVisualChildren();
		}

		// Token: 0x06005F2D RID: 24365 RVA: 0x00294320 File Offset: 0x00293320
		private TextBoxLineDrawingVisual GetLineVisual(int lineIndex)
		{
			TextBoxLineDrawingVisual result = null;
			if (this._viewportLineVisuals != null)
			{
				result = this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex];
			}
			return result;
		}

		// Token: 0x06005F2E RID: 24366 RVA: 0x0029434C File Offset: 0x0029334C
		private void SetLineVisual(int lineIndex, TextBoxLineDrawingVisual lineVisual)
		{
			if (this._viewportLineVisuals != null)
			{
				this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex] = lineVisual;
			}
		}

		// Token: 0x06005F2F RID: 24367 RVA: 0x0029436A File Offset: 0x0029336A
		private void AddLineVisualPlaceholder(int lineIndex)
		{
			if (this._viewportLineVisuals != null && lineIndex >= this._viewportLineVisualsIndex && lineIndex < this._viewportLineVisualsIndex + this._viewportLineVisuals.Count)
			{
				this._viewportLineVisuals.Insert(lineIndex - this._viewportLineVisualsIndex, null);
			}
		}

		// Token: 0x06005F30 RID: 24368 RVA: 0x002943A8 File Offset: 0x002933A8
		private void ClearLineVisual(int lineIndex)
		{
			if (this._viewportLineVisuals != null && lineIndex >= this._viewportLineVisualsIndex && lineIndex < this._viewportLineVisualsIndex + this._viewportLineVisuals.Count && this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex] != null)
			{
				this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex].DiscardOnArrange = true;
				this._viewportLineVisuals[lineIndex - this._viewportLineVisualsIndex] = null;
			}
		}

		// Token: 0x06005F31 RID: 24369 RVA: 0x00294420 File Offset: 0x00293420
		private void RemoveLineVisualRange(int lineIndex, int count)
		{
			if (this._viewportLineVisuals != null)
			{
				if (lineIndex < this._viewportLineVisualsIndex)
				{
					count -= this._viewportLineVisualsIndex - lineIndex;
					count = Math.Max(0, count);
					lineIndex = this._viewportLineVisualsIndex;
				}
				if (lineIndex < this._viewportLineVisualsIndex + this._viewportLineVisuals.Count)
				{
					int num = lineIndex - this._viewportLineVisualsIndex;
					count = Math.Min(count, this._viewportLineVisuals.Count - num);
					for (int i = 0; i < count; i++)
					{
						if (this._viewportLineVisuals[num + i] != null)
						{
							this._viewportLineVisuals[num + i].DiscardOnArrange = true;
						}
					}
					this._viewportLineVisuals.RemoveRange(num, count);
				}
			}
		}

		// Token: 0x06005F32 RID: 24370 RVA: 0x002944CE File Offset: 0x002934CE
		private void OnThrottleBackgroundTimeout(object sender, EventArgs e)
		{
			this._throttleBackgroundTimer.Stop();
			this._throttleBackgroundTimer = null;
			if (this.IsBackgroundLayoutPending)
			{
				this.OnBackgroundMeasure(null);
			}
		}

		// Token: 0x06005F33 RID: 24371 RVA: 0x002944F4 File Offset: 0x002934F4
		private double GetContentOffset(double lineWidth, TextAlignment aligment)
		{
			double wrappingWidth = this.GetWrappingWidth(base.RenderSize.Width);
			double result;
			if (aligment != TextAlignment.Right)
			{
				if (aligment != TextAlignment.Center)
				{
					result = 0.0;
				}
				else
				{
					result = (wrappingWidth - lineWidth) / 2.0;
				}
			}
			else
			{
				result = wrappingWidth - lineWidth;
			}
			return result;
		}

		// Token: 0x06005F34 RID: 24372 RVA: 0x00294544 File Offset: 0x00293544
		private TextAlignment HorizontalAlignmentToTextAlignment(HorizontalAlignment horizontalAlignment)
		{
			TextAlignment result;
			switch (horizontalAlignment)
			{
			default:
				result = TextAlignment.Left;
				break;
			case HorizontalAlignment.Center:
				result = TextAlignment.Center;
				break;
			case HorizontalAlignment.Right:
				result = TextAlignment.Right;
				break;
			case HorizontalAlignment.Stretch:
				result = TextAlignment.Justify;
				break;
			}
			return result;
		}

		// Token: 0x06005F35 RID: 24373 RVA: 0x00294578 File Offset: 0x00293578
		private bool Contains(ITextPointer position)
		{
			Invariant.Assert(this.IsLayoutValid);
			return position.TextContainer == this._host.TextContainer && this._lineMetrics != null && this._lineMetrics[this._lineMetrics.Count - 1].EndOffset >= position.Offset;
		}

		// Token: 0x06005F36 RID: 24374 RVA: 0x002945D5 File Offset: 0x002935D5
		private double GetWrappingWidth(double width)
		{
			if (width < this._contentSize.Width)
			{
				width = this._contentSize.Width;
			}
			if (width > this._previousConstraint.Width)
			{
				width = this._previousConstraint.Width;
			}
			TextDpi.EnsureValidLineWidth(ref width);
			return width;
		}

		// Token: 0x06005F37 RID: 24375 RVA: 0x00294618 File Offset: 0x00293618
		private double GetTextAlignmentCorrection(TextAlignment textAlignment, double width)
		{
			double result = 0.0;
			if (textAlignment != TextAlignment.Left && this._contentSize.Width > width)
			{
				result = -this.GetContentOffset(this._contentSize.Width, textAlignment);
			}
			return result;
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x00294658 File Offset: 0x00293658
		private DirtyTextRange? GetSelectionRenderRange(DirtyTextRange selectionRange)
		{
			DirtyTextRange? result = null;
			int index;
			int index2;
			this.GetVisibleLines(out index, out index2);
			int startIndex = selectionRange.StartIndex;
			int num = selectionRange.StartIndex + selectionRange.PositionsAdded;
			int offset = this._lineMetrics[index].Offset;
			int endOffset = this._lineMetrics[index2].EndOffset;
			if (endOffset >= startIndex && offset <= num)
			{
				int num2 = Math.Max(offset, startIndex);
				int num3 = Math.Min(endOffset, num) - num2;
				result = new DirtyTextRange?(new DirtyTextRange(num2, num3, num3, true));
			}
			return result;
		}

		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x06005F39 RID: 24377 RVA: 0x002946ED File Offset: 0x002936ED
		private bool IsLayoutValid
		{
			get
			{
				return base.IsMeasureValid && base.IsArrangeValid;
			}
		}

		// Token: 0x170015F4 RID: 5620
		// (get) Token: 0x06005F3A RID: 24378 RVA: 0x00294700 File Offset: 0x00293700
		private Rect Viewport
		{
			get
			{
				if (this._scrollData != null)
				{
					return new Rect(this._scrollData.HorizontalOffset, this._scrollData.VerticalOffset, this._scrollData.ViewportWidth, this._scrollData.ViewportHeight);
				}
				return Rect.Empty;
			}
		}

		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x06005F3B RID: 24379 RVA: 0x0029474C File Offset: 0x0029374C
		private bool IsBackgroundLayoutPending
		{
			get
			{
				return this.CheckFlags(TextBoxView.Flags.BackgroundLayoutPending);
			}
		}

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x06005F3C RID: 24380 RVA: 0x00294758 File Offset: 0x00293758
		private double VerticalAlignmentOffset
		{
			get
			{
				double result;
				switch (((Control)this._host).VerticalContentAlignment)
				{
				default:
					result = 0.0;
					break;
				case VerticalAlignment.Center:
					result = this.VerticalPadding / 2.0;
					break;
				case VerticalAlignment.Bottom:
					result = this.VerticalPadding;
					break;
				}
				return result;
			}
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x06005F3D RID: 24381 RVA: 0x002947B4 File Offset: 0x002937B4
		private TextAlignment CalculatedTextAlignment
		{
			get
			{
				Control control = (Control)this._host;
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(control, TextBox.TextAlignmentProperty).BaseValueSource;
				BaseValueSource baseValueSource2 = DependencyPropertyHelper.GetValueSource(control, Control.HorizontalContentAlignmentProperty).BaseValueSource;
				if (baseValueSource == BaseValueSource.Local)
				{
					return (TextAlignment)control.GetValue(TextBox.TextAlignmentProperty);
				}
				if (baseValueSource2 == BaseValueSource.Local)
				{
					object value = control.GetValue(Control.HorizontalContentAlignmentProperty);
					return this.HorizontalAlignmentToTextAlignment((HorizontalAlignment)value);
				}
				if (baseValueSource == BaseValueSource.Default && baseValueSource2 != BaseValueSource.Default)
				{
					object value = control.GetValue(Control.HorizontalContentAlignmentProperty);
					return this.HorizontalAlignmentToTextAlignment((HorizontalAlignment)value);
				}
				return (TextAlignment)control.GetValue(TextBox.TextAlignmentProperty);
			}
		}

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x06005F3E RID: 24382 RVA: 0x00294860 File Offset: 0x00293860
		private double VerticalPadding
		{
			get
			{
				Rect viewport = this.Viewport;
				double result;
				if (viewport.IsEmpty)
				{
					result = 0.0;
				}
				else
				{
					result = Math.Max(0.0, viewport.Height - this._contentSize.Height);
				}
				return result;
			}
		}

		// Token: 0x040031A6 RID: 12710
		private readonly ITextBoxViewHost _host;

		// Token: 0x040031A7 RID: 12711
		private Size _contentSize;

		// Token: 0x040031A8 RID: 12712
		private Size _previousConstraint;

		// Token: 0x040031A9 RID: 12713
		private TextBoxView.TextCache _cache;

		// Token: 0x040031AA RID: 12714
		private double _lineHeight;

		// Token: 0x040031AB RID: 12715
		private List<TextBoxLineDrawingVisual> _visualChildren;

		// Token: 0x040031AC RID: 12716
		private List<TextBoxView.LineRecord> _lineMetrics;

		// Token: 0x040031AD RID: 12717
		private List<TextBoxLineDrawingVisual> _viewportLineVisuals;

		// Token: 0x040031AE RID: 12718
		private int _viewportLineVisualsIndex;

		// Token: 0x040031AF RID: 12719
		private ScrollData _scrollData;

		// Token: 0x040031B0 RID: 12720
		private DtrList _dirtyList;

		// Token: 0x040031B1 RID: 12721
		private DispatcherTimer _throttleBackgroundTimer;

		// Token: 0x040031B2 RID: 12722
		private TextBoxView.Flags _flags;

		// Token: 0x040031B3 RID: 12723
		private EventHandler UpdatedEvent;

		// Token: 0x040031B4 RID: 12724
		private const uint _maxMeasureTimeMs = 200U;

		// Token: 0x040031B5 RID: 12725
		private const int _throttleBackgroundSeconds = 2;

		// Token: 0x02000BBD RID: 3005
		[Flags]
		private enum Flags
		{
			// Token: 0x040049BB RID: 18875
			TextContainerListenersInitialized = 1,
			// Token: 0x040049BC RID: 18876
			BackgroundLayoutPending = 2,
			// Token: 0x040049BD RID: 18877
			ArrangePendingFromHighlightLayer = 4
		}

		// Token: 0x02000BBE RID: 3006
		private class TextCache
		{
			// Token: 0x06008F3B RID: 36667 RVA: 0x00343998 File Offset: 0x00342998
			internal TextCache(TextBoxView owner)
			{
				this._lineProperties = owner.GetLineProperties();
				this._textRunCache = new TextRunCache();
				TextFormattingMode textFormattingMode = TextOptions.GetTextFormattingMode((Control)owner.Host);
				this._textFormatter = TextFormatter.FromCurrentDispatcher(textFormattingMode);
			}

			// Token: 0x17001F4F RID: 8015
			// (get) Token: 0x06008F3C RID: 36668 RVA: 0x003439DF File Offset: 0x003429DF
			internal LineProperties LineProperties
			{
				get
				{
					return this._lineProperties;
				}
			}

			// Token: 0x17001F50 RID: 8016
			// (get) Token: 0x06008F3D RID: 36669 RVA: 0x003439E7 File Offset: 0x003429E7
			internal TextRunCache TextRunCache
			{
				get
				{
					return this._textRunCache;
				}
			}

			// Token: 0x17001F51 RID: 8017
			// (get) Token: 0x06008F3E RID: 36670 RVA: 0x003439EF File Offset: 0x003429EF
			internal TextFormatter TextFormatter
			{
				get
				{
					return this._textFormatter;
				}
			}

			// Token: 0x040049BE RID: 18878
			private readonly LineProperties _lineProperties;

			// Token: 0x040049BF RID: 18879
			private readonly TextRunCache _textRunCache;

			// Token: 0x040049C0 RID: 18880
			private TextFormatter _textFormatter;
		}

		// Token: 0x02000BBF RID: 3007
		private class LineRecord
		{
			// Token: 0x06008F3F RID: 36671 RVA: 0x003439F7 File Offset: 0x003429F7
			internal LineRecord(int offset, TextBoxLine line)
			{
				this._offset = offset;
				this._length = line.Length;
				this._contentLength = line.ContentLength;
				this._width = line.Width;
			}

			// Token: 0x17001F52 RID: 8018
			// (get) Token: 0x06008F40 RID: 36672 RVA: 0x00343A2A File Offset: 0x00342A2A
			// (set) Token: 0x06008F41 RID: 36673 RVA: 0x00343A32 File Offset: 0x00342A32
			internal int Offset
			{
				get
				{
					return this._offset;
				}
				set
				{
					this._offset = value;
				}
			}

			// Token: 0x17001F53 RID: 8019
			// (get) Token: 0x06008F42 RID: 36674 RVA: 0x00343A3B File Offset: 0x00342A3B
			internal int Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x17001F54 RID: 8020
			// (get) Token: 0x06008F43 RID: 36675 RVA: 0x00343A43 File Offset: 0x00342A43
			internal int ContentLength
			{
				get
				{
					return this._contentLength;
				}
			}

			// Token: 0x17001F55 RID: 8021
			// (get) Token: 0x06008F44 RID: 36676 RVA: 0x00343A4B File Offset: 0x00342A4B
			internal double Width
			{
				get
				{
					return this._width;
				}
			}

			// Token: 0x17001F56 RID: 8022
			// (get) Token: 0x06008F45 RID: 36677 RVA: 0x00343A53 File Offset: 0x00342A53
			internal int EndOffset
			{
				get
				{
					return this._offset + this._length;
				}
			}

			// Token: 0x040049C1 RID: 18881
			private int _offset;

			// Token: 0x040049C2 RID: 18882
			private readonly int _length;

			// Token: 0x040049C3 RID: 18883
			private readonly int _contentLength;

			// Token: 0x040049C4 RID: 18884
			private readonly double _width;
		}
	}
}
