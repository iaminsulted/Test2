using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using MS.Internal.Documents;
using MS.Internal.PtsHost;

namespace MS.Internal
{
	// Token: 0x020000F9 RID: 249
	internal static class LayoutDump
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x001043A2 File Offset: 0x001033A2
		internal static string DumpLayoutAndVisualTreeToString(string tagName, Visual root)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 2;
			LayoutDump.DumpLayoutAndVisualTree(xmlTextWriter, tagName, root);
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
			return stringWriter.ToString();
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x001043DA File Offset: 0x001033DA
		internal static void DumpLayoutAndVisualTree(XmlTextWriter writer, string tagName, Visual root)
		{
			writer.WriteStartElement(tagName);
			LayoutDump.DumpVisual(writer, root, root);
			writer.WriteEndElement();
			writer.WriteRaw("\r\n");
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x001043FC File Offset: 0x001033FC
		internal static void DumpLayoutTreeToFile(string tagName, UIElement root, string fileName)
		{
			string value = LayoutDump.DumpLayoutTreeToString(tagName, root);
			StreamWriter streamWriter = new StreamWriter(fileName);
			streamWriter.Write(value);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00104429 File Offset: 0x00103429
		internal static string DumpLayoutTreeToString(string tagName, UIElement root)
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 2;
			LayoutDump.DumpLayoutTree(xmlTextWriter, tagName, root);
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
			return stringWriter.ToString();
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00104461 File Offset: 0x00103461
		internal static void DumpLayoutTree(XmlTextWriter writer, string tagName, UIElement root)
		{
			writer.WriteStartElement(tagName);
			LayoutDump.DumpUIElement(writer, root, root, true);
			writer.WriteEndElement();
			writer.WriteRaw("\r\n");
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00104484 File Offset: 0x00103484
		internal static void AddUIElementDumpHandler(Type type, LayoutDump.DumpCustomUIElement dumper)
		{
			LayoutDump._elementToDumpHandler.Add(type, dumper);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00104492 File Offset: 0x00103492
		internal static void AddDocumentPageDumpHandler(Type type, LayoutDump.DumpCustomDocumentPage dumper)
		{
			LayoutDump._documentPageToDumpHandler.Add(type, dumper);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x001044A0 File Offset: 0x001034A0
		internal static void DumpVisual(XmlTextWriter writer, Visual visual, Visual parent)
		{
			if (visual is UIElement)
			{
				LayoutDump.DumpUIElement(writer, (UIElement)visual, parent, false);
				return;
			}
			writer.WriteStartElement(visual.GetType().Name);
			Rect visualContentBounds = visual.VisualContentBounds;
			if (!visualContentBounds.IsEmpty)
			{
				LayoutDump.DumpRect(writer, "ContentRect", visualContentBounds);
			}
			Geometry clip = VisualTreeHelper.GetClip(visual);
			if (clip != null)
			{
				LayoutDump.DumpRect(writer, "Clip.Bounds", clip.Bounds);
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(parent);
			Point point = new Point(0.0, 0.0);
			generalTransform.TryTransform(point, out point);
			if (point.X != 0.0 || point.Y != 0.0)
			{
				LayoutDump.DumpPoint(writer, "Position", point);
			}
			LayoutDump.DumpVisualChildren(writer, "Children", visual);
			writer.WriteEndElement();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00104578 File Offset: 0x00103578
		private static void DumpUIElement(XmlTextWriter writer, UIElement element, Visual parent, bool uiElementsOnly)
		{
			writer.WriteStartElement(element.GetType().Name);
			LayoutDump.DumpSize(writer, "DesiredSize", element.DesiredSize);
			LayoutDump.DumpSize(writer, "ComputedSize", element.RenderSize);
			Geometry clip = VisualTreeHelper.GetClip(element);
			if (clip != null)
			{
				LayoutDump.DumpRect(writer, "Clip.Bounds", clip.Bounds);
			}
			GeneralTransform generalTransform = element.TransformToAncestor(parent);
			Point point = new Point(0.0, 0.0);
			generalTransform.TryTransform(point, out point);
			if (point.X != 0.0 || point.Y != 0.0)
			{
				LayoutDump.DumpPoint(writer, "Position", point);
			}
			bool flag = false;
			Type type = element.GetType();
			LayoutDump.DumpCustomUIElement dumpCustomUIElement = null;
			while (dumpCustomUIElement == null && type != null)
			{
				dumpCustomUIElement = (LayoutDump._elementToDumpHandler[type] as LayoutDump.DumpCustomUIElement);
				type = type.BaseType;
			}
			if (dumpCustomUIElement != null)
			{
				flag = dumpCustomUIElement(writer, element, uiElementsOnly);
			}
			if (!flag)
			{
				if (uiElementsOnly)
				{
					LayoutDump.DumpUIElementChildren(writer, "Children", element);
				}
				else
				{
					LayoutDump.DumpVisualChildren(writer, "Children", element);
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00104698 File Offset: 0x00103698
		internal static void DumpDocumentPage(XmlTextWriter writer, DocumentPage page, Visual parent)
		{
			writer.WriteStartElement("DocumentPage");
			writer.WriteAttributeString("Type", page.GetType().FullName);
			if (page != DocumentPage.Missing)
			{
				LayoutDump.DumpSize(writer, "Size", page.Size);
				GeneralTransform generalTransform = page.Visual.TransformToAncestor(parent);
				Point point = new Point(0.0, 0.0);
				generalTransform.TryTransform(point, out point);
				if (point.X != 0.0 || point.Y != 0.0)
				{
					LayoutDump.DumpPoint(writer, "Position", point);
				}
				Type type = page.GetType();
				LayoutDump.DumpCustomDocumentPage dumpCustomDocumentPage = null;
				while (dumpCustomDocumentPage == null && type != null)
				{
					dumpCustomDocumentPage = (LayoutDump._documentPageToDumpHandler[type] as LayoutDump.DumpCustomDocumentPage);
					type = type.BaseType;
				}
				if (dumpCustomDocumentPage != null)
				{
					dumpCustomDocumentPage(writer, page);
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00104780 File Offset: 0x00103780
		private static void DumpVisualChildren(XmlTextWriter writer, string tagName, Visual visualParent)
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount(visualParent);
			if (childrenCount > 0)
			{
				writer.WriteStartElement(tagName);
				writer.WriteAttributeString("Count", childrenCount.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < childrenCount; i++)
				{
					LayoutDump.DumpVisual(writer, visualParent.InternalGetVisualChild(i), visualParent);
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x001047D8 File Offset: 0x001037D8
		internal static void DumpUIElementChildren(XmlTextWriter writer, string tagName, Visual visualParent)
		{
			List<UIElement> list = new List<UIElement>();
			LayoutDump.GetUIElementsFromVisual(visualParent, list);
			if (list.Count > 0)
			{
				writer.WriteStartElement(tagName);
				writer.WriteAttributeString("Count", list.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < list.Count; i++)
				{
					LayoutDump.DumpUIElement(writer, list[i], visualParent, true);
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00104848 File Offset: 0x00103848
		internal static void DumpPoint(XmlTextWriter writer, string tagName, Point point)
		{
			writer.WriteStartElement(tagName);
			writer.WriteAttributeString("Left", point.X.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Top", point.Y.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x001048AC File Offset: 0x001038AC
		internal static void DumpSize(XmlTextWriter writer, string tagName, Size size)
		{
			writer.WriteStartElement(tagName);
			writer.WriteAttributeString("Width", size.Width.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Height", size.Height.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00104910 File Offset: 0x00103910
		internal static void DumpRect(XmlTextWriter writer, string tagName, Rect rect)
		{
			writer.WriteStartElement(tagName);
			writer.WriteAttributeString("Left", rect.Left.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Top", rect.Top.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Width", rect.Width.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Height", rect.Height.ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x001049BC File Offset: 0x001039BC
		internal static void GetUIElementsFromVisual(Visual visual, List<UIElement> uiElements)
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount(visual);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual visual2 = visual.InternalGetVisualChild(i);
				if (visual2 is UIElement)
				{
					uiElements.Add((UIElement)visual2);
				}
				else
				{
					LayoutDump.GetUIElementsFromVisual(visual2, uiElements);
				}
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00104A04 File Offset: 0x00103A04
		static LayoutDump()
		{
			LayoutDump.AddUIElementDumpHandler(typeof(TextBlock), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpText));
			LayoutDump.AddUIElementDumpHandler(typeof(FlowDocumentScrollViewer), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpFlowDocumentScrollViewer));
			LayoutDump.AddUIElementDumpHandler(typeof(FlowDocumentView), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpFlowDocumentView));
			LayoutDump.AddUIElementDumpHandler(typeof(DocumentPageView), new LayoutDump.DumpCustomUIElement(LayoutDump.DumpDocumentPageView));
			LayoutDump.AddDocumentPageDumpHandler(typeof(FlowDocumentPage), new LayoutDump.DumpCustomDocumentPage(LayoutDump.DumpFlowDocumentPage));
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00104AAC File Offset: 0x00103AAC
		private static bool DumpDocumentPageView(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			DocumentPageView documentPageView = element as DocumentPageView;
			if (documentPageView.DocumentPage != null)
			{
				LayoutDump.DumpDocumentPage(writer, documentPageView.DocumentPage, element);
			}
			return false;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00104AD8 File Offset: 0x00103AD8
		private static bool DumpText(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			TextBlock textBlock = element as TextBlock;
			if (textBlock.HasComplexContent)
			{
				LayoutDump.DumpTextRange(writer, textBlock.ContentStart, textBlock.ContentEnd);
			}
			else
			{
				LayoutDump.DumpTextRange(writer, textBlock.Text);
			}
			writer.WriteStartElement("Metrics");
			writer.WriteAttributeString("BaselineOffset", ((double)textBlock.GetValue(TextBlock.BaselineOffsetProperty)).ToString("F", CultureInfo.InvariantCulture));
			writer.WriteEndElement();
			if (textBlock.IsLayoutDataValid)
			{
				ReadOnlyCollection<LineResult> lineResults = textBlock.GetLineResults();
				LayoutDump.DumpLineResults(writer, lineResults, element);
			}
			return false;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00104B6C File Offset: 0x00103B6C
		private static bool DumpFlowDocumentScrollViewer(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			FlowDocumentScrollViewer flowDocumentScrollViewer = element as FlowDocumentScrollViewer;
			bool result = false;
			if (flowDocumentScrollViewer.HorizontalScrollBarVisibility == ScrollBarVisibility.Hidden && flowDocumentScrollViewer.VerticalScrollBarVisibility == ScrollBarVisibility.Hidden && flowDocumentScrollViewer.ScrollViewer != null)
			{
				FlowDocumentView flowDocumentView = flowDocumentScrollViewer.ScrollViewer.Content as FlowDocumentView;
				if (flowDocumentView != null)
				{
					LayoutDump.DumpUIElement(writer, flowDocumentView, flowDocumentScrollViewer, uiElementsOnly);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00104BC0 File Offset: 0x00103BC0
		private static bool DumpFlowDocumentView(XmlTextWriter writer, UIElement element, bool uiElementsOnly)
		{
			FlowDocumentView flowDocumentView = element as FlowDocumentView;
			IScrollInfo scrollInfo = flowDocumentView;
			if (scrollInfo.ScrollOwner != null)
			{
				Size size = new Size(scrollInfo.ExtentWidth, scrollInfo.ExtentHeight);
				if (DoubleUtil.AreClose(size, element.DesiredSize))
				{
					LayoutDump.DumpSize(writer, "Extent", size);
				}
				Point point = new Point(scrollInfo.HorizontalOffset, scrollInfo.VerticalOffset);
				if (!DoubleUtil.IsZero(point.X) || !DoubleUtil.IsZero(point.Y))
				{
					LayoutDump.DumpPoint(writer, "Offset", point);
				}
			}
			FlowDocumentPage documentPage = flowDocumentView.Document.BottomlessFormatter.DocumentPage;
			GeneralTransform generalTransform = documentPage.Visual.TransformToAncestor(flowDocumentView);
			Point point2 = new Point(0.0, 0.0);
			generalTransform.TryTransform(point2, out point2);
			if (!DoubleUtil.IsZero(point2.X) && !DoubleUtil.IsZero(point2.Y))
			{
				LayoutDump.DumpPoint(writer, "PagePosition", point2);
			}
			LayoutDump.DumpFlowDocumentPage(writer, documentPage);
			return false;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00104CB8 File Offset: 0x00103CB8
		private static void DumpFlowDocumentPage(XmlTextWriter writer, DocumentPage page)
		{
			FlowDocumentPage flowDocumentPage = page as FlowDocumentPage;
			writer.WriteStartElement("FormattedLines");
			writer.WriteAttributeString("Count", flowDocumentPage.FormattedLinesCount.ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
			TextDocumentView textDocumentView = (TextDocumentView)((IServiceProvider)flowDocumentPage).GetService(typeof(ITextView));
			if (textDocumentView.IsValid)
			{
				LayoutDump.DumpColumnResults(writer, textDocumentView.Columns, page.Visual);
			}
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00104D2C File Offset: 0x00103D2C
		private static void DumpTextRange(XmlTextWriter writer, string content)
		{
			int num = 0;
			int length = content.Length;
			writer.WriteStartElement("TextRange");
			writer.WriteAttributeString("Start", num.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Length", (length - num).ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00104D88 File Offset: 0x00103D88
		private static void DumpTextRange(XmlTextWriter writer, ITextPointer start, ITextPointer end)
		{
			int offsetToPosition = start.TextContainer.Start.GetOffsetToPosition(start);
			int offsetToPosition2 = end.TextContainer.Start.GetOffsetToPosition(end);
			writer.WriteStartElement("TextRange");
			writer.WriteAttributeString("Start", offsetToPosition.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Length", (offsetToPosition2 - offsetToPosition).ToString(CultureInfo.InvariantCulture));
			writer.WriteEndElement();
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00104DFC File Offset: 0x00103DFC
		private static void DumpLineRange(XmlTextWriter writer, int cpStart, int cpEnd, int cpContentEnd, int cpEllipses)
		{
			writer.WriteStartElement("TextRange");
			writer.WriteAttributeString("Start", cpStart.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("Length", (cpEnd - cpStart).ToString(CultureInfo.InvariantCulture));
			if (cpEnd != cpContentEnd)
			{
				writer.WriteAttributeString("HiddenLength", (cpEnd - cpContentEnd).ToString(CultureInfo.InvariantCulture));
			}
			if (cpEnd != cpEllipses)
			{
				writer.WriteAttributeString("EllipsesLength", (cpEnd - cpEllipses).ToString(CultureInfo.InvariantCulture));
			}
			writer.WriteEndElement();
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00104E8C File Offset: 0x00103E8C
		private static void DumpLineResults(XmlTextWriter writer, ReadOnlyCollection<LineResult> lines, Visual visualParent)
		{
			if (lines != null)
			{
				writer.WriteStartElement("Lines");
				writer.WriteAttributeString("Count", lines.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < lines.Count; i++)
				{
					writer.WriteStartElement("Line");
					LineResult lineResult = lines[i];
					LayoutDump.DumpRect(writer, "LayoutBox", lineResult.LayoutBox);
					LayoutDump.DumpLineRange(writer, lineResult.StartPositionCP, lineResult.EndPositionCP, lineResult.GetContentEndPositionCP(), lineResult.GetEllipsesPositionCP());
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00104F28 File Offset: 0x00103F28
		private static void DumpParagraphResults(XmlTextWriter writer, string tagName, ReadOnlyCollection<ParagraphResult> paragraphs, Visual visualParent)
		{
			if (paragraphs != null)
			{
				writer.WriteStartElement(tagName);
				writer.WriteAttributeString("Count", paragraphs.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < paragraphs.Count; i++)
				{
					ParagraphResult paragraphResult = paragraphs[i];
					if (paragraphResult is TextParagraphResult)
					{
						LayoutDump.DumpTextParagraphResult(writer, (TextParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is ContainerParagraphResult)
					{
						LayoutDump.DumpContainerParagraphResult(writer, (ContainerParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is TableParagraphResult)
					{
						LayoutDump.DumpTableParagraphResult(writer, (TableParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is FloaterParagraphResult)
					{
						LayoutDump.DumpFloaterParagraphResult(writer, (FloaterParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is UIElementParagraphResult)
					{
						LayoutDump.DumpUIElementParagraphResult(writer, (UIElementParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is FigureParagraphResult)
					{
						LayoutDump.DumpFigureParagraphResult(writer, (FigureParagraphResult)paragraphResult, visualParent);
					}
					else if (paragraphResult is SubpageParagraphResult)
					{
						LayoutDump.DumpSubpageParagraphResult(writer, (SubpageParagraphResult)paragraphResult, visualParent);
					}
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00105028 File Offset: 0x00104028
		private static void DumpTextParagraphResult(XmlTextWriter writer, TextParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("TextParagraph");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpTextRange(writer, paragraph.StartPosition, paragraph.EndPosition);
			LayoutDump.DumpLineResults(writer, paragraph.Lines, visualParent2);
			LayoutDump.DumpParagraphResults(writer, "Floaters", paragraph.Floaters, visualParent2);
			LayoutDump.DumpParagraphResults(writer, "Figures", paragraph.Figures, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x001050D0 File Offset: 0x001040D0
		private static void DumpContainerParagraphResult(XmlTextWriter writer, ContainerParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("ContainerParagraph");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpParagraphResults(writer, "Paragraphs", paragraph.Paragraphs, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00105148 File Offset: 0x00104148
		private static void DumpFloaterParagraphResult(XmlTextWriter writer, FloaterParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("Floater");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpColumnResults(writer, paragraph.Columns, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x001051BC File Offset: 0x001041BC
		private static void DumpUIElementParagraphResult(XmlTextWriter writer, UIElementParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("BlockUIContainer");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			writer.WriteEndElement();
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00105220 File Offset: 0x00104220
		private static void DumpFigureParagraphResult(XmlTextWriter writer, FigureParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("Figure");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpColumnResults(writer, paragraph.Columns, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00105294 File Offset: 0x00104294
		private static void DumpTableParagraphResult(XmlTextWriter writer, TableParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("TableParagraph");
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visual = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			ReadOnlyCollection<ParagraphResult> paragraphs = paragraph.Paragraphs;
			int childrenCount = VisualTreeHelper.GetChildrenCount(visual);
			for (int i = 0; i < childrenCount; i++)
			{
				Visual visual2 = visual.InternalGetVisualChild(i);
				int childrenCount2 = VisualTreeHelper.GetChildrenCount(visual2);
				ReadOnlyCollection<ParagraphResult> cellParagraphs = ((RowParagraphResult)paragraphs[i]).CellParagraphs;
				for (int j = 0; j < childrenCount2; j++)
				{
					Visual cellVisual = visual2.InternalGetVisualChild(j);
					LayoutDump.DumpTableCell(writer, cellParagraphs[j], cellVisual, visual);
				}
			}
			writer.WriteEndElement();
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0010533C File Offset: 0x0010433C
		private static void DumpSubpageParagraphResult(XmlTextWriter writer, SubpageParagraphResult paragraph, Visual visualParent)
		{
			writer.WriteStartElement("SubpageParagraph");
			writer.WriteStartElement("Element");
			writer.WriteAttributeString("Type", paragraph.Element.GetType().FullName);
			writer.WriteEndElement();
			LayoutDump.DumpRect(writer, "LayoutBox", paragraph.LayoutBox);
			Visual visualParent2 = LayoutDump.DumpParagraphOffset(writer, paragraph, visualParent);
			LayoutDump.DumpColumnResults(writer, paragraph.Columns, visualParent2);
			writer.WriteEndElement();
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x001053B0 File Offset: 0x001043B0
		private static void DumpColumnResults(XmlTextWriter writer, ReadOnlyCollection<ColumnResult> columns, Visual visualParent)
		{
			if (columns != null)
			{
				writer.WriteStartElement("Columns");
				writer.WriteAttributeString("Count", columns.Count.ToString(CultureInfo.InvariantCulture));
				for (int i = 0; i < columns.Count; i++)
				{
					writer.WriteStartElement("Column");
					ColumnResult columnResult = columns[i];
					LayoutDump.DumpRect(writer, "LayoutBox", columnResult.LayoutBox);
					LayoutDump.DumpTextRange(writer, columnResult.StartPosition, columnResult.EndPosition);
					LayoutDump.DumpParagraphResults(writer, "Paragraphs", columnResult.Paragraphs, visualParent);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00105454 File Offset: 0x00104454
		private static Visual DumpParagraphOffset(XmlTextWriter writer, ParagraphResult paragraph, Visual visualParent)
		{
			object value = paragraph.GetType().GetField("_paraClient", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(paragraph);
			Visual visual = (Visual)value.GetType().GetProperty("Visual", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(value, null);
			if (visualParent.IsAncestorOf(visual))
			{
				GeneralTransform generalTransform = visual.TransformToAncestor(visualParent);
				Point point = new Point(0.0, 0.0);
				generalTransform.TryTransform(point, out point);
				if (point.X != 0.0 || point.Y != 0.0)
				{
					LayoutDump.DumpPoint(writer, "Origin", point);
				}
			}
			return visual;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x001054FC File Offset: 0x001044FC
		private static void DumpTableCalculatedMetrics(XmlTextWriter writer, object element)
		{
			PropertyInfo property = typeof(Table).GetProperty("ColumnCount");
			if (property != null)
			{
				int num = (int)property.GetValue(element, null);
				writer.WriteStartElement("ColumnCount");
				writer.WriteAttributeString("Count", num.ToString(CultureInfo.InvariantCulture));
				writer.WriteEndElement();
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00105560 File Offset: 0x00104560
		private static void DumpTableCell(XmlTextWriter writer, ParagraphResult paragraph, Visual cellVisual, Visual tableVisual)
		{
			FieldInfo field = paragraph.GetType().GetField("_paraClient", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				return;
			}
			CellParaClient cellParaClient = (CellParaClient)field.GetValue(paragraph);
			TableCell cell = cellParaClient.CellParagraph.Cell;
			writer.WriteStartElement("Cell");
			Type type = cell.GetType();
			PropertyInfo property = type.GetProperty("ColumnIndex", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
			if (property != null)
			{
				writer.WriteAttributeString("ColumnIndex", ((int)property.GetValue(cell, null)).ToString(CultureInfo.InvariantCulture));
			}
			PropertyInfo property2 = type.GetProperty("RowIndex", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
			if (property2 != null)
			{
				writer.WriteAttributeString("RowIndex", ((int)property2.GetValue(cell, null)).ToString(CultureInfo.InvariantCulture));
			}
			writer.WriteAttributeString("ColumnSpan", cell.ColumnSpan.ToString(CultureInfo.InvariantCulture));
			writer.WriteAttributeString("RowSpan", cell.RowSpan.ToString(CultureInfo.InvariantCulture));
			Rect rect = cellParaClient.Rect.FromTextDpi();
			LayoutDump.DumpRect(writer, "LayoutBox", rect);
			bool flag;
			LayoutDump.DumpParagraphResults(writer, "Paragraphs", cellParaClient.GetColumnResults(out flag)[0].Paragraphs, cellParaClient.Visual);
			writer.WriteEndElement();
		}

		// Token: 0x040006DA RID: 1754
		private static Hashtable _elementToDumpHandler = new Hashtable();

		// Token: 0x040006DB RID: 1755
		private static Hashtable _documentPageToDumpHandler = new Hashtable();

		// Token: 0x020008B1 RID: 2225
		// (Invoke) Token: 0x060080D2 RID: 32978
		internal delegate bool DumpCustomUIElement(XmlTextWriter writer, UIElement element, bool uiElementsOnly);

		// Token: 0x020008B2 RID: 2226
		// (Invoke) Token: 0x060080D6 RID: 32982
		internal delegate void DumpCustomDocumentPage(XmlTextWriter writer, DocumentPage page);
	}
}
