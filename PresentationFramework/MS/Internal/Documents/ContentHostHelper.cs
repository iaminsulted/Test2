using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020001B9 RID: 441
	internal static class ContentHostHelper
	{
		// Token: 0x06000E7B RID: 3707 RVA: 0x0013922C File Offset: 0x0013822C
		internal static IContentHost FindContentHost(ContentElement contentElement)
		{
			IContentHost result = null;
			if (contentElement == null)
			{
				return null;
			}
			if (contentElement is TextElement)
			{
				TextContainer textContainer = ((TextElement)contentElement).TextContainer;
				DependencyObject parent = textContainer.Parent;
				if (parent is IContentHost)
				{
					result = (IContentHost)parent;
				}
				else if (parent is FlowDocument)
				{
					result = ContentHostHelper.GetICHFromFlowDocument((TextElement)contentElement, (FlowDocument)parent);
				}
				else if (textContainer.TextView != null && textContainer.TextView.RenderScope is IContentHost)
				{
					result = (IContentHost)textContainer.TextView.RenderScope;
				}
			}
			return result;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x001392B4 File Offset: 0x001382B4
		private static IContentHost GetICHFromFlowDocument(TextElement contentElement, FlowDocument flowDocument)
		{
			IContentHost result = null;
			ITextView textView = flowDocument.StructuralCache.TextContainer.TextView;
			if (textView != null)
			{
				if (textView.RenderScope is FlowDocumentView)
				{
					if (VisualTreeHelper.GetChildrenCount(textView.RenderScope) > 0)
					{
						result = (VisualTreeHelper.GetChild(textView.RenderScope, 0) as IContentHost);
					}
				}
				else if (textView.RenderScope is FrameworkElement)
				{
					List<DocumentPageView> list = new List<DocumentPageView>();
					ContentHostHelper.FindDocumentPageViews(textView.RenderScope, list);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].DocumentPage is FlowDocumentPage)
						{
							textView = (ITextView)((IServiceProvider)list[i].DocumentPage).GetService(typeof(ITextView));
							if (textView != null && textView.IsValid && (textView.Contains(contentElement.ContentStart) || textView.Contains(contentElement.ContentEnd)))
							{
								result = (list[i].DocumentPage.Visual as IContentHost);
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x001393C0 File Offset: 0x001383C0
		private static void FindDocumentPageViews(Visual root, List<DocumentPageView> pageViews)
		{
			Invariant.Assert(root != null);
			Invariant.Assert(pageViews != null);
			if (root is DocumentPageView)
			{
				pageViews.Add((DocumentPageView)root);
				return;
			}
			int internalVisualChildrenCount = root.InternalVisualChildrenCount;
			for (int i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = root.InternalGetVisualChild(i);
				FrameworkElement frameworkElement = visual as FrameworkElement;
				if (frameworkElement != null)
				{
					if (frameworkElement.TemplatedParent != null)
					{
						if (frameworkElement is DocumentPageView)
						{
							pageViews.Add(frameworkElement as DocumentPageView);
						}
						else
						{
							ContentHostHelper.FindDocumentPageViews(frameworkElement, pageViews);
						}
					}
				}
				else
				{
					ContentHostHelper.FindDocumentPageViews(visual, pageViews);
				}
			}
		}
	}
}
