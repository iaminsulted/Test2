using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.Automation
{
	// Token: 0x02000106 RID: 262
	internal class TextAdaptor : ITextProvider, IDisposable
	{
		// Token: 0x06000647 RID: 1607 RVA: 0x00106444 File Offset: 0x00105444
		internal TextAdaptor(AutomationPeer textPeer, ITextContainer textContainer)
		{
			Invariant.Assert(textContainer != null, "Invalid ITextContainer");
			Invariant.Assert(textPeer is TextAutomationPeer || textPeer is ContentTextAutomationPeer, "Invalid AutomationPeer");
			this._textPeer = textPeer;
			this._textContainer = textContainer;
			this._textContainer.Changed += this.OnTextContainerChanged;
			if (this._textContainer.TextSelection != null)
			{
				this._textContainer.TextSelection.Changed += this.OnTextSelectionChanged;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x001064D1 File Offset: 0x001054D1
		public void Dispose()
		{
			if (this._textContainer != null && this._textContainer.TextSelection != null)
			{
				this._textContainer.TextSelection.Changed -= this.OnTextSelectionChanged;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0010650C File Offset: 0x0010550C
		internal Rect[] GetBoundingRectangles(ITextPointer start, ITextPointer end, bool clipToView, bool transformToScreen)
		{
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView == null)
			{
				return Array.Empty<Rect>();
			}
			ReadOnlyCollection<TextSegment> textSegments = updatedTextView.TextSegments;
			if (textSegments.Count > 0)
			{
				if (!updatedTextView.Contains(start) && start.CompareTo(textSegments[0].Start) < 0)
				{
					start = textSegments[0].Start.CreatePointer();
				}
				if (!updatedTextView.Contains(end) && end.CompareTo(textSegments[textSegments.Count - 1].End) > 0)
				{
					end = textSegments[textSegments.Count - 1].End.CreatePointer();
				}
			}
			if (!updatedTextView.Contains(start) || !updatedTextView.Contains(end))
			{
				return Array.Empty<Rect>();
			}
			TextRangeAdaptor.MoveToInsertionPosition(start, LogicalDirection.Forward);
			TextRangeAdaptor.MoveToInsertionPosition(end, LogicalDirection.Backward);
			Rect rect = Rect.Empty;
			if (clipToView)
			{
				rect = this.GetVisibleRectangle(updatedTextView);
				if (rect.IsEmpty)
				{
					return Array.Empty<Rect>();
				}
			}
			List<Rect> list = new List<Rect>();
			ITextPointer textPointer = start.CreatePointer();
			while (textPointer.CompareTo(end) < 0)
			{
				TextSegment lineRange = updatedTextView.GetLineRange(textPointer);
				if (!lineRange.IsNull)
				{
					ITextPointer startPosition = (lineRange.Start.CompareTo(start) <= 0) ? start : lineRange.Start;
					ITextPointer endPosition = (lineRange.End.CompareTo(end) >= 0) ? end : lineRange.End;
					Rect item = Rect.Empty;
					Geometry tightBoundingGeometryFromTextPositions = updatedTextView.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
					if (tightBoundingGeometryFromTextPositions != null)
					{
						item = tightBoundingGeometryFromTextPositions.Bounds;
						if (clipToView)
						{
							item.Intersect(rect);
						}
						if (!item.IsEmpty)
						{
							if (transformToScreen)
							{
								item = new Rect(this.ClientToScreen(item.TopLeft, updatedTextView.RenderScope), this.ClientToScreen(item.BottomRight, updatedTextView.RenderScope));
							}
							list.Add(item);
						}
					}
				}
				if (textPointer.MoveToLineBoundary(1) == 0)
				{
					textPointer = end;
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x001066F4 File Offset: 0x001056F4
		internal ITextView GetUpdatedTextView()
		{
			ITextView textView = this._textContainer.TextView;
			if (textView != null && !textView.IsValid)
			{
				if (!textView.Validate())
				{
					textView = null;
				}
				if (textView != null && !textView.IsValid)
				{
					textView = null;
				}
			}
			return textView;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00106730 File Offset: 0x00105730
		internal void Select(ITextPointer start, ITextPointer end)
		{
			if (this._textContainer.TextSelection != null)
			{
				this._textContainer.TextSelection.Select(start, end);
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00106754 File Offset: 0x00105754
		internal void ScrollIntoView(ITextPointer start, ITextPointer end, bool alignToTop)
		{
			Rect rect = Rect.Empty;
			foreach (Rect rect2 in this.GetBoundingRectangles(start, end, false, false))
			{
				rect.Union(rect2);
			}
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView != null && !rect.IsEmpty)
			{
				Rect visibleRectangle = this.GetVisibleRectangle(updatedTextView);
				Rect rect3 = Rect.Intersect(rect, visibleRectangle);
				if (rect3 == rect)
				{
					return;
				}
				UIElement renderScope = updatedTextView.RenderScope;
				Visual visual = renderScope;
				while (visual != null)
				{
					IScrollInfo scrollInfo = visual as IScrollInfo;
					if (scrollInfo != null)
					{
						if (visual != renderScope)
						{
							rect = renderScope.TransformToAncestor(visual).TransformBounds(rect);
						}
						if (scrollInfo.CanHorizontallyScroll)
						{
							scrollInfo.SetHorizontalOffset(alignToTop ? rect.Left : (rect.Right - scrollInfo.ViewportWidth));
						}
						if (scrollInfo.CanVerticallyScroll)
						{
							scrollInfo.SetVerticalOffset(alignToTop ? rect.Top : (rect.Bottom - scrollInfo.ViewportHeight));
							break;
						}
						break;
					}
					else
					{
						visual = (VisualTreeHelper.GetParent(visual) as Visual);
					}
				}
				FrameworkElement frameworkElement = renderScope as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.BringIntoView(rect3);
					return;
				}
			}
			else
			{
				ITextPointer textPointer = alignToTop ? start.CreatePointer() : end.CreatePointer();
				textPointer.MoveToElementEdge(alignToTop ? ElementEdge.AfterStart : ElementEdge.AfterEnd);
				FrameworkContentElement frameworkContentElement = textPointer.GetAdjacentElement(LogicalDirection.Backward) as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.BringIntoView();
				}
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x001068B7 File Offset: 0x001058B7
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			this._textPeer.RaiseAutomationEvent(AutomationEvents.TextPatternOnTextChanged);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x001068C6 File Offset: 0x001058C6
		private void OnTextSelectionChanged(object sender, EventArgs e)
		{
			this._textPeer.RaiseAutomationEvent(AutomationEvents.TextPatternOnTextSelectionChanged);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x001068D8 File Offset: 0x001058D8
		private Rect GetVisibleRectangle(ITextView textView)
		{
			Rect empty = new Rect(textView.RenderScope.RenderSize);
			Visual visual = VisualTreeHelper.GetParent(textView.RenderScope) as Visual;
			while (visual != null && empty != Rect.Empty)
			{
				if (VisualTreeHelper.GetClip(visual) != null)
				{
					GeneralTransform inverse = textView.RenderScope.TransformToAncestor(visual).Inverse;
					if (inverse != null)
					{
						Rect rect = VisualTreeHelper.GetClip(visual).Bounds;
						rect = inverse.TransformBounds(rect);
						empty.Intersect(rect);
					}
					else
					{
						empty = Rect.Empty;
					}
				}
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
			return empty;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00106968 File Offset: 0x00105968
		private Point ClientToScreen(Point point, Visual visual)
		{
			if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				return this.ObsoleteClientToScreen(point, visual);
			}
			try
			{
				point = visual.PointToScreen(point);
			}
			catch (InvalidOperationException)
			{
			}
			return point;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x001069A8 File Offset: 0x001059A8
		private Point ObsoleteClientToScreen(Point point, Visual visual)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(visual);
			if (presentationSource != null)
			{
				GeneralTransform generalTransform = visual.TransformToAncestor(presentationSource.RootVisual);
				if (generalTransform != null)
				{
					point = generalTransform.Transform(point);
				}
			}
			return PointUtil.ClientToScreen(point, presentationSource);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x001069E0 File Offset: 0x001059E0
		private Point ScreenToClient(Point point, Visual visual)
		{
			if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				return this.ObsoleteScreenToClient(point, visual);
			}
			try
			{
				point = visual.PointFromScreen(point);
			}
			catch (InvalidOperationException)
			{
			}
			return point;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00106A20 File Offset: 0x00105A20
		private Point ObsoleteScreenToClient(Point point, Visual visual)
		{
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(visual);
			point = PointUtil.ScreenToClient(point, presentationSource);
			if (presentationSource != null)
			{
				GeneralTransform generalTransform = visual.TransformToAncestor(presentationSource.RootVisual);
				if (generalTransform != null)
				{
					generalTransform = generalTransform.Inverse;
					if (generalTransform != null)
					{
						point = generalTransform.Transform(point);
					}
				}
			}
			return point;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00106A64 File Offset: 0x00105A64
		ITextRangeProvider[] ITextProvider.GetSelection()
		{
			ITextRange textSelection = this._textContainer.TextSelection;
			if (textSelection == null)
			{
				throw new InvalidOperationException(SR.Get("TextProvider_TextSelectionNotSupported"));
			}
			return new ITextRangeProvider[]
			{
				new TextRangeAdaptor(this, textSelection.Start, textSelection.End, this._textPeer)
			};
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00106AB4 File Offset: 0x00105AB4
		ITextRangeProvider[] ITextProvider.GetVisibleRanges()
		{
			ITextRangeProvider[] array = null;
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView != null)
			{
				List<TextSegment> list = new List<TextSegment>();
				if (updatedTextView is MultiPageTextView)
				{
					list.AddRange(updatedTextView.TextSegments);
				}
				else
				{
					Rect visibleRectangle = this.GetVisibleRectangle(updatedTextView);
					if (!visibleRectangle.IsEmpty)
					{
						ITextPointer textPositionFromPoint = updatedTextView.GetTextPositionFromPoint(visibleRectangle.TopLeft, true);
						ITextPointer textPositionFromPoint2 = updatedTextView.GetTextPositionFromPoint(visibleRectangle.BottomRight, true);
						list.Add(new TextSegment(textPositionFromPoint, textPositionFromPoint2, true));
					}
				}
				if (list.Count > 0)
				{
					array = new ITextRangeProvider[list.Count];
					for (int i = 0; i < list.Count; i++)
					{
						array[i] = new TextRangeAdaptor(this, list[i].Start, list[i].End, this._textPeer);
					}
				}
			}
			if (array == null)
			{
				array = new ITextRangeProvider[]
				{
					new TextRangeAdaptor(this, this._textContainer.Start, this._textContainer.Start, this._textPeer)
				};
			}
			return array;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00106BBC File Offset: 0x00105BBC
		ITextRangeProvider ITextProvider.RangeFromChild(IRawElementProviderSimple childElementProvider)
		{
			if (childElementProvider == null)
			{
				throw new ArgumentNullException("childElementProvider");
			}
			DependencyObject dependencyObject;
			if (this._textPeer is TextAutomationPeer)
			{
				dependencyObject = ((TextAutomationPeer)this._textPeer).ElementFromProvider(childElementProvider);
			}
			else
			{
				dependencyObject = ((ContentTextAutomationPeer)this._textPeer).ElementFromProvider(childElementProvider);
			}
			TextRangeAdaptor textRangeAdaptor = null;
			if (dependencyObject != null)
			{
				ITextPointer textPointer = null;
				ITextPointer textPointer2 = null;
				if (dependencyObject is TextElement)
				{
					textPointer = ((TextElement)dependencyObject).ElementStart;
					textPointer2 = ((TextElement)dependencyObject).ElementEnd;
				}
				else
				{
					DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
					if (parent is InlineUIContainer || parent is BlockUIContainer)
					{
						textPointer = ((TextElement)parent).ContentStart;
						textPointer2 = ((TextElement)parent).ContentEnd;
					}
					else
					{
						ITextPointer textPointer3 = this._textContainer.Start.CreatePointer();
						while (textPointer3.CompareTo(this._textContainer.End) < 0)
						{
							TextPointerContext pointerContext = textPointer3.GetPointerContext(LogicalDirection.Forward);
							if (pointerContext == TextPointerContext.ElementStart)
							{
								if (dependencyObject == textPointer3.GetAdjacentElement(LogicalDirection.Forward))
								{
									textPointer = textPointer3.CreatePointer(LogicalDirection.Forward);
									textPointer3.MoveToElementEdge(ElementEdge.AfterEnd);
									textPointer2 = textPointer3.CreatePointer(LogicalDirection.Backward);
									break;
								}
							}
							else if (pointerContext == TextPointerContext.EmbeddedElement && dependencyObject == textPointer3.GetAdjacentElement(LogicalDirection.Forward))
							{
								textPointer = textPointer3.CreatePointer(LogicalDirection.Forward);
								textPointer3.MoveToNextContextPosition(LogicalDirection.Forward);
								textPointer2 = textPointer3.CreatePointer(LogicalDirection.Backward);
								break;
							}
							textPointer3.MoveToNextContextPosition(LogicalDirection.Forward);
						}
					}
				}
				if (textPointer != null && textPointer2 != null)
				{
					textRangeAdaptor = new TextRangeAdaptor(this, textPointer, textPointer2, this._textPeer);
				}
			}
			if (textRangeAdaptor == null)
			{
				throw new InvalidOperationException(SR.Get("TextProvider_InvalidChildElement"));
			}
			return textRangeAdaptor;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00106D38 File Offset: 0x00105D38
		ITextRangeProvider ITextProvider.RangeFromPoint(Point location)
		{
			TextRangeAdaptor textRangeAdaptor = null;
			ITextView updatedTextView = this.GetUpdatedTextView();
			if (updatedTextView != null)
			{
				location = this.ScreenToClient(location, updatedTextView.RenderScope);
				ITextPointer textPositionFromPoint = updatedTextView.GetTextPositionFromPoint(location, true);
				if (textPositionFromPoint != null)
				{
					textRangeAdaptor = new TextRangeAdaptor(this, textPositionFromPoint, textPositionFromPoint, this._textPeer);
				}
			}
			if (textRangeAdaptor == null)
			{
				throw new ArgumentException(SR.Get("TextProvider_InvalidPoint"));
			}
			return textRangeAdaptor;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00106D8F File Offset: 0x00105D8F
		ITextRangeProvider ITextProvider.DocumentRange
		{
			get
			{
				return new TextRangeAdaptor(this, this._textContainer.Start, this._textContainer.End, this._textPeer);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00106DB3 File Offset: 0x00105DB3
		SupportedTextSelection ITextProvider.SupportedTextSelection
		{
			get
			{
				if (this._textContainer.TextSelection != null)
				{
					return SupportedTextSelection.Single;
				}
				return SupportedTextSelection.None;
			}
		}

		// Token: 0x040006FC RID: 1788
		private AutomationPeer _textPeer;

		// Token: 0x040006FD RID: 1789
		private ITextContainer _textContainer;
	}
}
