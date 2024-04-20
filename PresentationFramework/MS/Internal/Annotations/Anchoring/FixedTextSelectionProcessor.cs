using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D2 RID: 722
	internal class FixedTextSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06001B35 RID: 6965 RVA: 0x001673A2 File Offset: 0x001663A2
		public override bool MergeSelections(object anchor1, object anchor2, out object newAnchor)
		{
			return TextSelectionHelper.MergeSelections(anchor1, anchor2, out newAnchor);
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x001673AC File Offset: 0x001663AC
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			IEnumerable<TextSegment> enumerable = this.CheckSelection(selection);
			IList<DependencyObject> list = new List<DependencyObject>();
			foreach (TextSegment textSegment in enumerable)
			{
				int minValue = int.MinValue;
				ITextPointer pointer = textSegment.Start.CreatePointer(LogicalDirection.Forward);
				TextSelectionHelper.GetPointerPage(pointer, out minValue);
				Point pointForPointer = TextSelectionHelper.GetPointForPointer(pointer);
				if (minValue == -2147483648)
				{
					throw new ArgumentException(SR.Get("SelectionDoesNotResolveToAPage", new object[]
					{
						"start"
					}), "selection");
				}
				int minValue2 = int.MinValue;
				ITextPointer pointer2 = textSegment.End.CreatePointer(LogicalDirection.Backward);
				TextSelectionHelper.GetPointerPage(pointer2, out minValue2);
				Point pointForPointer2 = TextSelectionHelper.GetPointForPointer(pointer2);
				if (minValue2 == -2147483648)
				{
					throw new ArgumentException(SR.Get("SelectionDoesNotResolveToAPage", new object[]
					{
						"end"
					}), "selection");
				}
				int num = list.Count;
				int num2 = minValue2 - minValue;
				int i = 0;
				if (list.Count > 0 && ((FixedTextSelectionProcessor.FixedPageProxy)list[list.Count - 1]).Page == minValue)
				{
					num--;
					i++;
				}
				while (i <= num2)
				{
					list.Add(new FixedTextSelectionProcessor.FixedPageProxy(textSegment.Start.TextContainer.Parent, minValue + i));
					i++;
				}
				if (num2 == 0)
				{
					((FixedTextSelectionProcessor.FixedPageProxy)list[num]).Segments.Add(new FixedTextSelectionProcessor.PointSegment(pointForPointer, pointForPointer2));
				}
				else
				{
					((FixedTextSelectionProcessor.FixedPageProxy)list[num]).Segments.Add(new FixedTextSelectionProcessor.PointSegment(pointForPointer, FixedTextSelectionProcessor.PointSegment.NotAPoint));
					((FixedTextSelectionProcessor.FixedPageProxy)list[num + num2]).Segments.Add(new FixedTextSelectionProcessor.PointSegment(FixedTextSelectionProcessor.PointSegment.NotAPoint, pointForPointer2));
				}
			}
			return list;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00167588 File Offset: 0x00166588
		public override UIElement GetParent(object selection)
		{
			this.CheckAnchor(selection);
			return TextSelectionHelper.GetParent(selection);
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00167598 File Offset: 0x00166598
		public override Point GetAnchorPoint(object selection)
		{
			this.CheckAnchor(selection);
			return TextSelectionHelper.GetAnchorPoint(selection);
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x001675A8 File Offset: 0x001665A8
		public override IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			this.CheckSelection(selection);
			FixedTextSelectionProcessor.FixedPageProxy fixedPageProxy = startNode as FixedTextSelectionProcessor.FixedPageProxy;
			if (fixedPageProxy == null)
			{
				throw new ArgumentException(SR.Get("StartNodeMustBeFixedPageProxy"), "startNode");
			}
			ContentLocatorPart contentLocatorPart = new ContentLocatorPart(FixedTextSelectionProcessor.FixedTextElementName);
			if (fixedPageProxy.Segments.Count == 0)
			{
				contentLocatorPart.NameValuePairs.Add("Count", 1.ToString(NumberFormatInfo.InvariantInfo));
				contentLocatorPart.NameValuePairs.Add("Segment" + 0.ToString(NumberFormatInfo.InvariantInfo), ",,,");
			}
			else
			{
				contentLocatorPart.NameValuePairs.Add("Count", fixedPageProxy.Segments.Count.ToString(NumberFormatInfo.InvariantInfo));
				for (int i = 0; i < fixedPageProxy.Segments.Count; i++)
				{
					string text = "";
					if (!double.IsNaN(fixedPageProxy.Segments[i].Start.X))
					{
						text = text + fixedPageProxy.Segments[i].Start.X.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + fixedPageProxy.Segments[i].Start.Y.ToString(NumberFormatInfo.InvariantInfo);
					}
					else
					{
						text += TextSelectionProcessor.Separator[0].ToString();
					}
					text += TextSelectionProcessor.Separator[0].ToString();
					if (!double.IsNaN(fixedPageProxy.Segments[i].End.X))
					{
						text = text + fixedPageProxy.Segments[i].End.X.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + fixedPageProxy.Segments[i].End.Y.ToString(NumberFormatInfo.InvariantInfo);
					}
					else
					{
						text += TextSelectionProcessor.Separator[0].ToString();
					}
					contentLocatorPart.NameValuePairs.Add("Segment" + i.ToString(NumberFormatInfo.InvariantInfo), text);
				}
			}
			return new List<ContentLocatorPart>(1)
			{
				contentLocatorPart
			};
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0016783C File Offset: 0x0016683C
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			DocumentPage documentPage = null;
			FixedPage fixedPage = startNode as FixedPage;
			if (fixedPage != null)
			{
				documentPage = this.GetDocumentPage(fixedPage);
			}
			else
			{
				DocumentPageView documentPageView = startNode as DocumentPageView;
				if (documentPageView != null)
				{
					documentPage = (documentPageView.DocumentPage as FixedDocumentPage);
					if (documentPage == null)
					{
						documentPage = (documentPageView.DocumentPage as FixedDocumentSequenceDocumentPage);
					}
				}
			}
			if (documentPage == null)
			{
				throw new ArgumentException(SR.Get("StartNodeMustBeDocumentPageViewOrFixedPage"), "startNode");
			}
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			attachmentLevel = AttachmentLevel.Unresolved;
			ITextView textView = (ITextView)((IServiceProvider)documentPage).GetService(typeof(ITextView));
			ReadOnlyCollection<TextSegment> textSegments = textView.TextSegments;
			if (textSegments == null || textSegments.Count <= 0)
			{
				return null;
			}
			TextAnchor textAnchor = new TextAnchor();
			if (documentPage != null)
			{
				string text = locatorPart.NameValuePairs["Count"];
				if (text == null)
				{
					throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
					{
						"Count"
					}));
				}
				int num = int.Parse(text, NumberFormatInfo.InvariantInfo);
				for (int i = 0; i < num; i++)
				{
					Point point;
					Point point2;
					this.GetLocatorPartSegmentValues(locatorPart, i, out point, out point2);
					ITextPointer textPointer;
					if (double.IsNaN(point.X) || double.IsNaN(point.Y))
					{
						textPointer = FixedTextSelectionProcessor.FindStartVisibleTextPointer(documentPage);
					}
					else
					{
						textPointer = textView.GetTextPositionFromPoint(point, true);
					}
					if (textPointer != null)
					{
						ITextPointer textPointer2;
						if (double.IsNaN(point2.X) || double.IsNaN(point2.Y))
						{
							textPointer2 = FixedTextSelectionProcessor.FindEndVisibleTextPointer(documentPage);
						}
						else
						{
							textPointer2 = textView.GetTextPositionFromPoint(point2, true);
						}
						Invariant.Assert(textPointer2 != null, "end TP is null when start TP is not");
						attachmentLevel = AttachmentLevel.Full;
						textAnchor.AddTextSegment(textPointer, textPointer2);
					}
				}
			}
			if (textAnchor.TextSegments.Count > 0)
			{
				return textAnchor;
			}
			return null;
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x001679F8 File Offset: 0x001669F8
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])FixedTextSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x00167A0C File Offset: 0x00166A0C
		private DocumentPage GetDocumentPage(FixedPage page)
		{
			Invariant.Assert(page != null);
			DocumentPage result = null;
			PageContent pageContent = page.Parent as PageContent;
			if (pageContent != null)
			{
				FixedDocument fixedDocument = pageContent.Parent as FixedDocument;
				FixedDocumentSequence fixedDocumentSequence = fixedDocument.Parent as FixedDocumentSequence;
				if (fixedDocumentSequence != null)
				{
					result = fixedDocumentSequence.GetPage(fixedDocument, fixedDocument.GetIndexOfPage(page));
				}
				else
				{
					result = fixedDocument.GetPage(fixedDocument.GetIndexOfPage(page));
				}
			}
			return result;
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x00167A70 File Offset: 0x00166A70
		private IList<TextSegment> CheckSelection(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			ITextRange textRange = selection as ITextRange;
			ITextPointer start;
			IList<TextSegment> textSegments;
			if (textRange != null)
			{
				start = textRange.Start;
				textSegments = textRange.TextSegments;
			}
			else
			{
				TextAnchor textAnchor = selection as TextAnchor;
				if (textAnchor == null)
				{
					throw new ArgumentException(SR.Get("WrongSelectionType"), "selection: type=" + selection.GetType().ToString());
				}
				start = textAnchor.Start;
				textSegments = textAnchor.TextSegments;
			}
			if (!(start.TextContainer is FixedTextContainer) && !(start.TextContainer is DocumentSequenceTextContainer))
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection: type=" + selection.GetType().ToString());
			}
			return textSegments;
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x00167B28 File Offset: 0x00166B28
		private TextAnchor CheckAnchor(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			TextAnchor textAnchor = selection as TextAnchor;
			if (textAnchor == null || (!(textAnchor.Start.TextContainer is FixedTextContainer) && !(textAnchor.Start.TextContainer is DocumentSequenceTextContainer)))
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection: type=" + selection.GetType().ToString());
			}
			return textAnchor;
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x00167B98 File Offset: 0x00166B98
		private void GetLocatorPartSegmentValues(ContentLocatorPart locatorPart, int segmentNumber, out Point start, out Point end)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (FixedTextSelectionProcessor.FixedTextElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			string text = locatorPart.NameValuePairs["Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			string[] array = text.Split(TextSelectionProcessor.Separator);
			if (array.Length != 4)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			start = this.GetPoint(array[0], array[1]);
			end = this.GetPoint(array[2], array[3]);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x00167CC0 File Offset: 0x00166CC0
		private Point GetPoint(string xstr, string ystr)
		{
			Point result;
			if (xstr != null && !string.IsNullOrEmpty(xstr.Trim()) && ystr != null && !string.IsNullOrEmpty(ystr.Trim()))
			{
				double x = double.Parse(xstr, NumberFormatInfo.InvariantInfo);
				double y = double.Parse(ystr, NumberFormatInfo.InvariantInfo);
				result = new Point(x, y);
			}
			else
			{
				result = new Point(double.NaN, double.NaN);
			}
			return result;
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00167D2C File Offset: 0x00166D2C
		private static ITextPointer FindStartVisibleTextPointer(DocumentPage documentPage)
		{
			ITextPointer textPointer;
			ITextPointer position;
			if (!FixedTextSelectionProcessor.GetTextViewRange(documentPage, out textPointer, out position))
			{
				return null;
			}
			if (!textPointer.IsAtInsertionPosition && !textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
			{
				return null;
			}
			if (textPointer.CompareTo(position) > 0)
			{
				return null;
			}
			return textPointer;
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00167D68 File Offset: 0x00166D68
		private static ITextPointer FindEndVisibleTextPointer(DocumentPage documentPage)
		{
			ITextPointer textPointer;
			ITextPointer textPointer2;
			if (!FixedTextSelectionProcessor.GetTextViewRange(documentPage, out textPointer, out textPointer2))
			{
				return null;
			}
			if (!textPointer2.IsAtInsertionPosition && !textPointer2.MoveToNextInsertionPosition(LogicalDirection.Backward))
			{
				return null;
			}
			if (textPointer.CompareTo(textPointer2) > 0)
			{
				return null;
			}
			return textPointer2;
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00167DA4 File Offset: 0x00166DA4
		private static bool GetTextViewRange(DocumentPage documentPage, out ITextPointer start, out ITextPointer end)
		{
			ITextPointer textPointer;
			end = (textPointer = null);
			start = textPointer;
			Invariant.Assert(documentPage != DocumentPage.Missing);
			ITextView textView = ((IServiceProvider)documentPage).GetService(typeof(ITextView)) as ITextView;
			Invariant.Assert(textView != null, "DocumentPage didn't provide a TextView.");
			if (textView.TextSegments == null || textView.TextSegments.Count == 0)
			{
				return false;
			}
			start = textView.TextSegments[0].Start.CreatePointer(LogicalDirection.Forward);
			end = textView.TextSegments[textView.TextSegments.Count - 1].End.CreatePointer(LogicalDirection.Backward);
			return true;
		}

		// Token: 0x04000E03 RID: 3587
		private static readonly XmlQualifiedName FixedTextElementName = new XmlQualifiedName("FixedTextRange", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04000E04 RID: 3588
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			FixedTextSelectionProcessor.FixedTextElementName
		};

		// Token: 0x02000A1A RID: 2586
		internal sealed class FixedPageProxy : DependencyObject
		{
			// Token: 0x060084F1 RID: 34033 RVA: 0x00327B95 File Offset: 0x00326B95
			public FixedPageProxy(DependencyObject parent, int page)
			{
				base.SetValue(PathNode.HiddenParentProperty, parent);
				this._page = page;
			}

			// Token: 0x17001DE4 RID: 7652
			// (get) Token: 0x060084F2 RID: 34034 RVA: 0x00327BBC File Offset: 0x00326BBC
			public int Page
			{
				get
				{
					return this._page;
				}
			}

			// Token: 0x17001DE5 RID: 7653
			// (get) Token: 0x060084F3 RID: 34035 RVA: 0x00327BC4 File Offset: 0x00326BC4
			public IList<FixedTextSelectionProcessor.PointSegment> Segments
			{
				get
				{
					return this._segments;
				}
			}

			// Token: 0x040040A1 RID: 16545
			private int _page;

			// Token: 0x040040A2 RID: 16546
			private IList<FixedTextSelectionProcessor.PointSegment> _segments = new List<FixedTextSelectionProcessor.PointSegment>(1);
		}

		// Token: 0x02000A1B RID: 2587
		internal sealed class PointSegment
		{
			// Token: 0x060084F4 RID: 34036 RVA: 0x00327BCC File Offset: 0x00326BCC
			internal PointSegment(Point start, Point end)
			{
				this._start = start;
				this._end = end;
			}

			// Token: 0x17001DE6 RID: 7654
			// (get) Token: 0x060084F5 RID: 34037 RVA: 0x00327BE2 File Offset: 0x00326BE2
			public Point Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001DE7 RID: 7655
			// (get) Token: 0x060084F6 RID: 34038 RVA: 0x00327BEA File Offset: 0x00326BEA
			public Point End
			{
				get
				{
					return this._end;
				}
			}

			// Token: 0x040040A3 RID: 16547
			public static readonly Point NotAPoint = new Point(double.NaN, double.NaN);

			// Token: 0x040040A4 RID: 16548
			private Point _start;

			// Token: 0x040040A5 RID: 16549
			private Point _end;
		}
	}
}
