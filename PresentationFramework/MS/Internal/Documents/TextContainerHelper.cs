using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020001F2 RID: 498
	internal static class TextContainerHelper
	{
		// Token: 0x060011BA RID: 4538 RVA: 0x00145788 File Offset: 0x00144788
		internal static List<AutomationPeer> GetAutomationPeersFromRange(ITextPointer start, ITextPointer end, ITextPointer ownerContentStart)
		{
			List<AutomationPeer> list = new List<AutomationPeer>();
			start = start.CreatePointer();
			while (start.CompareTo(end) < 0)
			{
				bool flag = false;
				if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					object adjacentElement = start.GetAdjacentElement(LogicalDirection.Forward);
					if (adjacentElement is ContentElement)
					{
						AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)adjacentElement);
						if (automationPeer != null)
						{
							if (ownerContentStart == null || TextContainerHelper.IsImmediateAutomationChild(start, ownerContentStart))
							{
								list.Add(automationPeer);
							}
							start.MoveToNextContextPosition(LogicalDirection.Forward);
							start.MoveToElementEdge(ElementEdge.AfterEnd);
							flag = true;
						}
					}
				}
				else if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement)
				{
					object adjacentElement = start.GetAdjacentElement(LogicalDirection.Forward);
					if (adjacentElement is UIElement)
					{
						if (ownerContentStart == null || TextContainerHelper.IsImmediateAutomationChild(start, ownerContentStart))
						{
							AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)adjacentElement);
							if (automationPeer != null)
							{
								list.Add(automationPeer);
							}
							else
							{
								TextContainerHelper.iterate((Visual)adjacentElement, list);
							}
						}
					}
					else if (adjacentElement is ContentElement)
					{
						AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)adjacentElement);
						if (automationPeer != null && (ownerContentStart == null || TextContainerHelper.IsImmediateAutomationChild(start, ownerContentStart)))
						{
							list.Add(automationPeer);
						}
					}
				}
				if (!flag && !start.MoveToNextContextPosition(LogicalDirection.Forward))
				{
					break;
				}
			}
			return list;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00145894 File Offset: 0x00144894
		internal static bool IsImmediateAutomationChild(ITextPointer elementStart, ITextPointer ownerContentStart)
		{
			Invariant.Assert(elementStart.CompareTo(ownerContentStart) >= 0);
			bool result = true;
			ITextPointer textPointer = elementStart.CreatePointer();
			while (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				if (textPointer.CompareTo(ownerContentStart) <= 0)
				{
					break;
				}
				AutomationPeer automationPeer = null;
				object adjacentElement = textPointer.GetAdjacentElement(LogicalDirection.Forward);
				if (adjacentElement is UIElement)
				{
					automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)adjacentElement);
				}
				else if (adjacentElement is ContentElement)
				{
					automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)adjacentElement);
				}
				if (automationPeer != null)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00145924 File Offset: 0x00144924
		internal static AutomationPeer GetEnclosingAutomationPeer(ITextPointer start, ITextPointer end, out ITextPointer elementStart, out ITextPointer elementEnd)
		{
			List<object> list = new List<object>();
			List<ITextPointer> list2 = new List<ITextPointer>();
			ITextPointer textPointer = start.CreatePointer();
			while (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				object obj = textPointer.GetAdjacentElement(LogicalDirection.Forward);
				if (obj != null)
				{
					list.Insert(0, obj);
					list2.Insert(0, textPointer.CreatePointer(LogicalDirection.Forward));
				}
			}
			List<object> list3 = new List<object>();
			List<ITextPointer> list4 = new List<ITextPointer>();
			textPointer = end.CreatePointer();
			while (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
				object obj = textPointer.GetAdjacentElement(LogicalDirection.Backward);
				if (obj != null)
				{
					list3.Insert(0, obj);
					list4.Insert(0, textPointer.CreatePointer(LogicalDirection.Backward));
				}
			}
			AutomationPeer automationPeer = null;
			ITextPointer textPointer2;
			elementEnd = (textPointer2 = null);
			elementStart = textPointer2;
			for (int i = Math.Min(list.Count, list3.Count); i > 0; i--)
			{
				if (list[i - 1] == list3[i - 1])
				{
					object obj = list[i - 1];
					if (obj is UIElement)
					{
						automationPeer = UIElementAutomationPeer.CreatePeerForElement((UIElement)obj);
					}
					else if (obj is ContentElement)
					{
						automationPeer = ContentElementAutomationPeer.CreatePeerForElement((ContentElement)obj);
					}
					if (automationPeer != null)
					{
						elementStart = list2[i - 1];
						elementEnd = list4[i - 1];
						break;
					}
				}
			}
			return automationPeer;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00145A78 File Offset: 0x00144A78
		internal static TextContentRange GetTextContentRangeForTextElement(TextElement textElement)
		{
			ITextContainer textContainer = textElement.TextContainer;
			int elementStartOffset = textElement.ElementStartOffset;
			int elementEndOffset = textElement.ElementEndOffset;
			return new TextContentRange(elementStartOffset, elementEndOffset, textContainer);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00145AA0 File Offset: 0x00144AA0
		internal static TextContentRange GetTextContentRangeForTextElementEdge(TextElement textElement, ElementEdge edge)
		{
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd);
			ITextContainer textContainer = textElement.TextContainer;
			int cpFirst;
			int cpLast;
			if (edge == ElementEdge.AfterEnd)
			{
				cpFirst = textElement.ContentEndOffset;
				cpLast = textElement.ElementEndOffset;
			}
			else
			{
				cpFirst = textElement.ElementStartOffset;
				cpLast = textElement.ContentStartOffset;
			}
			return new TextContentRange(cpFirst, cpLast, textContainer);
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00145AF0 File Offset: 0x00144AF0
		internal static ITextPointer GetContentStart(ITextContainer textContainer, DependencyObject element)
		{
			ITextPointer result;
			if (element is TextElement)
			{
				result = ((TextElement)element).ContentStart;
			}
			else
			{
				Invariant.Assert(element is TextBlock || element is FlowDocument || element is TextBox, "Cannot retrive ContentStart position for EmbeddedObject.");
				result = textContainer.CreatePointerAtOffset(0, LogicalDirection.Forward);
			}
			return result;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x00145B44 File Offset: 0x00144B44
		internal static int GetElementLength(ITextContainer textContainer, DependencyObject element)
		{
			int symbolCount;
			if (element is TextElement)
			{
				symbolCount = ((TextElement)element).SymbolCount;
			}
			else
			{
				Invariant.Assert(element is TextBlock || element is FlowDocument || element is TextBox, "Cannot retrive length for EmbeddedObject.");
				symbolCount = textContainer.SymbolCount;
			}
			return symbolCount;
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal static int EmbeddedObjectLength
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00145B95 File Offset: 0x00144B95
		internal static ITextPointer GetTextPointerFromCP(ITextContainer textContainer, int cp, LogicalDirection direction)
		{
			return textContainer.CreatePointerAtOffset(cp, direction);
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00145B9F File Offset: 0x00144B9F
		internal static StaticTextPointer GetStaticTextPointerFromCP(ITextContainer textContainer, int cp)
		{
			return textContainer.CreateStaticPointerAtOffset(cp);
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00145BA8 File Offset: 0x00144BA8
		internal static ITextPointer GetTextPointerForEmbeddedObject(FrameworkElement embeddedObject)
		{
			TextElement textElement = embeddedObject.Parent as TextElement;
			ITextPointer result;
			if (textElement != null)
			{
				result = textElement.ContentStart;
			}
			else
			{
				Invariant.Assert(false, "Embedded object needs to have InlineUIContainer or BlockUIContainer as parent.");
				result = null;
			}
			return result;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00145BDC File Offset: 0x00144BDC
		internal static int GetCPFromElement(ITextContainer textContainer, DependencyObject element, ElementEdge edge)
		{
			TextElement textElement = element as TextElement;
			int result;
			if (textElement != null)
			{
				if (!textElement.IsInTree || textElement.TextContainer != textContainer)
				{
					result = textContainer.SymbolCount;
				}
				else
				{
					switch (edge)
					{
					case ElementEdge.BeforeStart:
						return textElement.ElementStartOffset;
					case ElementEdge.AfterStart:
						return textElement.ContentStartOffset;
					case ElementEdge.BeforeStart | ElementEdge.AfterStart:
						break;
					case ElementEdge.BeforeEnd:
						return textElement.ContentEndOffset;
					default:
						if (edge == ElementEdge.AfterEnd)
						{
							return textElement.ElementEndOffset;
						}
						break;
					}
					Invariant.Assert(false, "Unknown ElementEdge.");
					result = 0;
				}
			}
			else
			{
				Invariant.Assert(element is TextBlock || element is FlowDocument || element is TextBox, "Cannot retrive length for EmbeddedObject.");
				result = ((edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterStart) ? 0 : textContainer.SymbolCount);
			}
			return result;
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00145C9C File Offset: 0x00144C9C
		internal static int GetCchFromElement(ITextContainer textContainer, DependencyObject element)
		{
			TextElement textElement = element as TextElement;
			int symbolCount;
			if (textElement != null)
			{
				symbolCount = textElement.SymbolCount;
			}
			else
			{
				symbolCount = textContainer.SymbolCount;
			}
			return symbolCount;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00145CC4 File Offset: 0x00144CC4
		internal static int GetCPFromEmbeddedObject(UIElement embeddedObject, ElementEdge edge)
		{
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd, "Cannot retrieve CP from the content of embedded object.");
			int result = -1;
			if (embeddedObject is FrameworkElement)
			{
				FrameworkElement frameworkElement = (FrameworkElement)embeddedObject;
				if (frameworkElement.Parent is TextElement)
				{
					TextElement textElement = (TextElement)frameworkElement.Parent;
					result = ((edge == ElementEdge.BeforeStart) ? textElement.ContentStartOffset : textElement.ContentEndOffset);
				}
			}
			return result;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00145D24 File Offset: 0x00144D24
		private static void iterate(Visual parent, List<AutomationPeer> peers)
		{
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			for (int i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = parent.InternalGetVisualChild(i);
				AutomationPeer item;
				if (visual != null && visual.CheckFlagsAnd(VisualFlags.IsUIElement) && (item = UIElementAutomationPeer.CreatePeerForElement((UIElement)visual)) != null)
				{
					peers.Add(item);
				}
				else
				{
					TextContainerHelper.iterate(visual, peers);
				}
			}
		}

		// Token: 0x04000B1D RID: 2845
		internal static int ElementEdgeCharacterLength = 1;
	}
}
