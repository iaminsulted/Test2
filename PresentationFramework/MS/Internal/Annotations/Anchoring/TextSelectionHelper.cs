using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D7 RID: 727
	internal class TextSelectionHelper
	{
		// Token: 0x06001B7A RID: 7034 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		private TextSelectionHelper()
		{
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x00169024 File Offset: 0x00168024
		public static bool MergeSelections(object anchor1, object anchor2, out object newAnchor)
		{
			TextAnchor textAnchor = anchor1 as TextAnchor;
			TextAnchor textAnchor2 = anchor2 as TextAnchor;
			if (anchor1 != null && textAnchor == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "anchor1: type = " + anchor1.GetType().ToString());
			}
			if (anchor2 != null && textAnchor2 == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "Anchor2: type = " + anchor2.GetType().ToString());
			}
			if (textAnchor == null)
			{
				newAnchor = textAnchor2;
				return newAnchor != null;
			}
			if (textAnchor2 == null)
			{
				newAnchor = textAnchor;
				return newAnchor != null;
			}
			newAnchor = TextAnchor.ExclusiveUnion(textAnchor, textAnchor2);
			return true;
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x001690B8 File Offset: 0x001680B8
		public static IList<DependencyObject> GetSelectedNodes(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			ITextPointer textPointer = null;
			ITextPointer position = null;
			IList<TextSegment> list;
			TextSelectionHelper.CheckSelection(selection, out textPointer, out position, out list);
			IList<DependencyObject> list2 = new List<DependencyObject>();
			if (textPointer.CompareTo(position) == 0)
			{
				list2.Add(((TextPointer)textPointer).Parent);
				return list2;
			}
			TextPointer textPointer2 = (TextPointer)textPointer.CreatePointer();
			while (((ITextPointer)textPointer2).CompareTo(position) < 0)
			{
				DependencyObject parent = textPointer2.Parent;
				if (!list2.Contains(parent))
				{
					list2.Add(parent);
				}
				textPointer2.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			return list2;
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00169144 File Offset: 0x00168144
		public static UIElement GetParent(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			ITextPointer pointer = null;
			ITextPointer textPointer = null;
			IList<TextSegment> list;
			TextSelectionHelper.CheckSelection(selection, out pointer, out textPointer, out list);
			return TextSelectionHelper.GetParent(pointer);
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x00169178 File Offset: 0x00168178
		public static UIElement GetParent(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			DependencyObject parent = PathNode.GetParent(pointer.TextContainer.Parent);
			FlowDocumentScrollViewer flowDocumentScrollViewer = parent as FlowDocumentScrollViewer;
			if (flowDocumentScrollViewer != null)
			{
				return (UIElement)flowDocumentScrollViewer.ScrollViewer.Content;
			}
			DocumentViewerBase documentViewerBase = parent as DocumentViewerBase;
			if (documentViewerBase != null)
			{
				int num;
				TextSelectionHelper.GetPointerPage(pointer.CreatePointer(LogicalDirection.Forward), out num);
				if (num >= 0)
				{
					foreach (DocumentPageView documentPageView in documentViewerBase.PageViews)
					{
						if (documentPageView.PageNumber == num)
						{
							Invariant.Assert(VisualTreeHelper.GetChildrenCount(documentPageView) == 1);
							return VisualTreeHelper.GetChild(documentPageView, 0) as DocumentPageHost;
						}
					}
					return null;
				}
			}
			return parent as UIElement;
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x00169250 File Offset: 0x00168250
		public static Point GetAnchorPoint(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			TextAnchor textAnchor = selection as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			return TextSelectionHelper.GetAnchorPointForPointer(textAnchor.Start.CreatePointer(LogicalDirection.Forward));
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x00169290 File Offset: 0x00168290
		public static Point GetAnchorPointForPointer(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(pointer);
			if (anchorRectangle != Rect.Empty)
			{
				return new Point(anchorRectangle.Left, anchorRectangle.Top + anchorRectangle.Height);
			}
			return new Point(0.0, 0.0);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x001692F4 File Offset: 0x001682F4
		public static Point GetPointForPointer(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(pointer);
			if (anchorRectangle != Rect.Empty)
			{
				return new Point(anchorRectangle.Left, anchorRectangle.Top + anchorRectangle.Height / 2.0);
			}
			return new Point(0.0, 0.0);
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x00169360 File Offset: 0x00168360
		public static Rect GetAnchorRectangle(ITextPointer pointer)
		{
			if (pointer == null)
			{
				throw new ArgumentNullException("pointer");
			}
			bool flag = false;
			ITextView documentPageTextView = TextSelectionHelper.GetDocumentPageTextView(pointer);
			if (pointer.CompareTo(pointer.TextContainer.End) == 0)
			{
				Point point = new Point(double.MaxValue, double.MaxValue);
				pointer = documentPageTextView.GetTextPositionFromPoint(point, true);
				flag = true;
			}
			if (documentPageTextView != null && documentPageTextView.IsValid && TextDocumentView.Contains(pointer, documentPageTextView.TextSegments))
			{
				Rect rectangleFromTextPosition = documentPageTextView.GetRectangleFromTextPosition(pointer);
				if (flag && rectangleFromTextPosition != Rect.Empty)
				{
					rectangleFromTextPosition.X += rectangleFromTextPosition.Height / 2.0;
				}
				return rectangleFromTextPosition;
			}
			return Rect.Empty;
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x00169414 File Offset: 0x00168414
		public static IDocumentPaginatorSource GetPointerPage(ITextPointer pointer, out int pageNumber)
		{
			Invariant.Assert(pointer != null, "unknown pointer");
			IDocumentPaginatorSource documentPaginatorSource = pointer.TextContainer.Parent as IDocumentPaginatorSource;
			FixedDocument fixedDocument = documentPaginatorSource as FixedDocument;
			if (fixedDocument != null)
			{
				FixedDocumentSequence fixedDocumentSequence = fixedDocument.Parent as FixedDocumentSequence;
				if (fixedDocumentSequence != null)
				{
					documentPaginatorSource = fixedDocumentSequence;
				}
			}
			Invariant.Assert(documentPaginatorSource != null);
			DynamicDocumentPaginator dynamicDocumentPaginator = documentPaginatorSource.DocumentPaginator as DynamicDocumentPaginator;
			pageNumber = ((dynamicDocumentPaginator != null) ? dynamicDocumentPaginator.GetPageNumber((ContentPosition)pointer) : -1);
			return documentPaginatorSource;
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x00169488 File Offset: 0x00168488
		internal static void CheckSelection(object selection, out ITextPointer start, out ITextPointer end, out IList<TextSegment> segments)
		{
			ITextRange textRange = selection as ITextRange;
			if (textRange != null)
			{
				start = textRange.Start;
				end = textRange.End;
				segments = textRange.TextSegments;
				return;
			}
			TextAnchor textAnchor = selection as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			start = textAnchor.Start;
			end = textAnchor.End;
			segments = textAnchor.TextSegments;
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x001694F0 File Offset: 0x001684F0
		internal static ITextView GetDocumentPageTextView(ITextPointer pointer)
		{
			DependencyObject parent = pointer.TextContainer.Parent;
			if (parent != null)
			{
				FlowDocumentScrollViewer flowDocumentScrollViewer = PathNode.GetParent(parent) as FlowDocumentScrollViewer;
				if (flowDocumentScrollViewer != null)
				{
					IServiceProvider serviceProvider = flowDocumentScrollViewer.ScrollViewer.Content as IServiceProvider;
					Invariant.Assert(serviceProvider != null, "FlowDocumentScrollViewer should be an IServiceProvider.");
					return serviceProvider.GetService(typeof(ITextView)) as ITextView;
				}
			}
			int num;
			IDocumentPaginatorSource pointerPage = TextSelectionHelper.GetPointerPage(pointer, out num);
			if (pointerPage != null && num >= 0)
			{
				IServiceProvider serviceProvider2 = pointerPage.DocumentPaginator.GetPage(num) as IServiceProvider;
				if (serviceProvider2 != null)
				{
					return serviceProvider2.GetService(typeof(ITextView)) as ITextView;
				}
			}
			return null;
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x00169590 File Offset: 0x00168590
		internal static List<ITextView> GetDocumentPageTextViews(TextSegment segment)
		{
			ITextPointer textPointer = segment.Start.CreatePointer(LogicalDirection.Forward);
			ITextPointer textPointer2 = segment.End.CreatePointer(LogicalDirection.Backward);
			DependencyObject parent = textPointer.TextContainer.Parent;
			if (parent != null)
			{
				FlowDocumentScrollViewer flowDocumentScrollViewer = PathNode.GetParent(parent) as FlowDocumentScrollViewer;
				if (flowDocumentScrollViewer != null)
				{
					IServiceProvider serviceProvider = flowDocumentScrollViewer.ScrollViewer.Content as IServiceProvider;
					Invariant.Assert(serviceProvider != null, "FlowDocumentScrollViewer should be an IServiceProvider.");
					return new List<ITextView>(1)
					{
						serviceProvider.GetService(typeof(ITextView)) as ITextView
					};
				}
			}
			int num;
			IDocumentPaginatorSource pointerPage = TextSelectionHelper.GetPointerPage(textPointer, out num);
			DynamicDocumentPaginator dynamicDocumentPaginator = pointerPage.DocumentPaginator as DynamicDocumentPaginator;
			int num2 = (dynamicDocumentPaginator != null) ? dynamicDocumentPaginator.GetPageNumber((ContentPosition)textPointer2) : -1;
			List<ITextView> result;
			if (num == -1 || num2 == -1)
			{
				result = new List<ITextView>(0);
			}
			else if (num == num2)
			{
				result = TextSelectionHelper.ProcessSinglePage(pointerPage, num);
			}
			else
			{
				result = TextSelectionHelper.ProcessMultiplePages(pointerPage, num, num2);
			}
			return result;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x00169680 File Offset: 0x00168680
		private static List<ITextView> ProcessSinglePage(IDocumentPaginatorSource idp, int pageNumber)
		{
			Invariant.Assert(idp != null, "IDocumentPaginatorSource is null");
			IServiceProvider serviceProvider = idp.DocumentPaginator.GetPage(pageNumber) as IServiceProvider;
			List<ITextView> list = null;
			if (serviceProvider != null)
			{
				list = new List<ITextView>(1);
				ITextView textView = serviceProvider.GetService(typeof(ITextView)) as ITextView;
				if (textView != null)
				{
					list.Add(textView);
				}
			}
			return list;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x001696DC File Offset: 0x001686DC
		private static List<ITextView> ProcessMultiplePages(IDocumentPaginatorSource idp, int startPageNumber, int endPageNumber)
		{
			Invariant.Assert(idp != null, "IDocumentPaginatorSource is null");
			DocumentViewerBase documentViewerBase = PathNode.GetParent(idp as DependencyObject) as DocumentViewerBase;
			Invariant.Assert(documentViewerBase != null, "DocumentViewer not found");
			if (endPageNumber < startPageNumber)
			{
				int num = endPageNumber;
				endPageNumber = startPageNumber;
				startPageNumber = num;
			}
			List<ITextView> list = null;
			if (idp != null && startPageNumber >= 0 && endPageNumber >= startPageNumber)
			{
				list = new List<ITextView>(endPageNumber - startPageNumber + 1);
				for (int i = startPageNumber; i <= endPageNumber; i++)
				{
					DocumentPageView documentPageView = AnnotationHelper.FindView(documentViewerBase, i);
					if (documentPageView != null)
					{
						IServiceProvider serviceProvider = documentPageView.DocumentPage as IServiceProvider;
						if (serviceProvider != null)
						{
							ITextView textView = serviceProvider.GetService(typeof(ITextView)) as ITextView;
							if (textView != null)
							{
								list.Add(textView);
							}
						}
					}
				}
			}
			return list;
		}
	}
}
