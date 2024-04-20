using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000599 RID: 1433
	public class TextElementAutomationPeer : ContentTextAutomationPeer
	{
		// Token: 0x060045C4 RID: 17860 RVA: 0x00224834 File Offset: 0x00223834
		public TextElementAutomationPeer(TextElement owner) : base(owner)
		{
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x00224840 File Offset: 0x00223840
		protected override List<AutomationPeer> GetChildrenCore()
		{
			TextElement textElement = (TextElement)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(textElement.ContentStart, textElement.ContentEnd, null);
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x0022486C File Offset: 0x0022386C
		protected override Rect GetBoundingRectangleCore()
		{
			TextElement textElement = (TextElement)base.Owner;
			ITextView textView = textElement.TextContainer.TextView;
			if (textView == null || !textView.IsValid)
			{
				return Rect.Empty;
			}
			Geometry tightBoundingGeometryFromTextPositions = textView.GetTightBoundingGeometryFromTextPositions(textElement.ContentStart, textElement.ContentEnd);
			if (tightBoundingGeometryFromTextPositions == null)
			{
				return Rect.Empty;
			}
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(textView.RenderScope);
			if (presentationSource == null)
			{
				return Rect.Empty;
			}
			HwndSource hwndSource = presentationSource as HwndSource;
			if (hwndSource == null)
			{
				return Rect.Empty;
			}
			return PointUtil.ClientToScreen(PointUtil.RootToClient(PointUtil.ElementToRoot(tightBoundingGeometryFromTextPositions.Bounds, textView.RenderScope, presentationSource), presentationSource), hwndSource);
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x00224904 File Offset: 0x00223904
		protected override Point GetClickablePointCore()
		{
			Point result = default(Point);
			TextElement textElement = (TextElement)base.Owner;
			ITextView textView = textElement.TextContainer.TextView;
			if (textView == null || !textView.IsValid || (!textView.Contains(textElement.ContentStart) && !textView.Contains(textElement.ContentEnd)))
			{
				return result;
			}
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(textView.RenderScope);
			if (presentationSource == null)
			{
				return result;
			}
			HwndSource hwndSource = presentationSource as HwndSource;
			if (hwndSource == null)
			{
				return result;
			}
			TextPointer textPointer = textElement.ContentStart.GetNextInsertionPosition(LogicalDirection.Forward);
			if (textPointer == null || textPointer.CompareTo(textElement.ContentEnd) > 0)
			{
				textPointer = textElement.ContentEnd;
			}
			Rect rect = PointUtil.ClientToScreen(PointUtil.RootToClient(PointUtil.ElementToRoot(this.CalculateVisibleRect(textView, textElement, textElement.ContentStart, textPointer), textView.RenderScope, presentationSource), presentationSource), hwndSource);
			result = new Point(rect.Left + rect.Width * 0.5, rect.Top + rect.Height * 0.5);
			return result;
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x00224A08 File Offset: 0x00223A08
		protected override bool IsOffscreenCore()
		{
			IsOffscreenBehavior isOffscreenBehavior = AutomationProperties.GetIsOffscreenBehavior(base.Owner);
			if (isOffscreenBehavior == IsOffscreenBehavior.Onscreen)
			{
				return false;
			}
			if (isOffscreenBehavior != IsOffscreenBehavior.Offscreen)
			{
				TextElement textElement = (TextElement)base.Owner;
				ITextView textView = textElement.TextContainer.TextView;
				return textView == null || !textView.IsValid || (!textView.Contains(textElement.ContentStart) && !textView.Contains(textElement.ContentEnd)) || this.CalculateVisibleRect(textView, textElement, textElement.ContentStart, textElement.ContentEnd) == Rect.Empty;
			}
			return true;
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x00224A94 File Offset: 0x00223A94
		private Rect CalculateVisibleRect(ITextView textView, TextElement textElement, TextPointer startPointer, TextPointer endPointer)
		{
			Geometry tightBoundingGeometryFromTextPositions = textView.GetTightBoundingGeometryFromTextPositions(startPointer, endPointer);
			Rect rect = (tightBoundingGeometryFromTextPositions != null) ? tightBoundingGeometryFromTextPositions.Bounds : Rect.Empty;
			Visual visual = textView.RenderScope;
			while (visual != null && rect != Rect.Empty)
			{
				if (VisualTreeHelper.GetClip(visual) != null)
				{
					GeneralTransform inverse = textView.RenderScope.TransformToAncestor(visual).Inverse;
					if (inverse == null)
					{
						return Rect.Empty;
					}
					Rect rect2 = VisualTreeHelper.GetClip(visual).Bounds;
					rect2 = inverse.TransformBounds(rect2);
					rect.Intersect(rect2);
				}
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
			return rect;
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x00224B28 File Offset: 0x00223B28
		internal override List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end)
		{
			base.GetChildren();
			TextElement textElement = (TextElement)base.Owner;
			return TextContainerHelper.GetAutomationPeersFromRange(start, end, textElement.ContentStart);
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x060045CB RID: 17867 RVA: 0x00224B58 File Offset: 0x00223B58
		internal bool? IsTextViewVisible
		{
			get
			{
				TextElement textElement = (TextElement)base.Owner;
				object obj;
				if (textElement == null)
				{
					obj = null;
				}
				else
				{
					TextContainer textContainer = textElement.TextContainer;
					obj = ((textContainer != null) ? textContainer.TextView : null);
				}
				object obj2 = obj;
				UIElement uielement = (obj2 != null) ? ((ITextView)obj2).RenderScope : null;
				if (uielement == null)
				{
					return null;
				}
				return new bool?(uielement.IsVisible);
			}
		}
	}
}
