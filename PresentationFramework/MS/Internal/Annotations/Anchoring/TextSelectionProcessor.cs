using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D8 RID: 728
	internal sealed class TextSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06001B8A RID: 7050 RVA: 0x001673A2 File Offset: 0x001663A2
		public override bool MergeSelections(object anchor1, object anchor2, out object newAnchor)
		{
			return TextSelectionHelper.MergeSelections(anchor1, anchor2, out newAnchor);
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x00169794 File Offset: 0x00168794
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			return TextSelectionHelper.GetSelectedNodes(selection);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0016979C File Offset: 0x0016879C
		public override UIElement GetParent(object selection)
		{
			return TextSelectionHelper.GetParent(selection);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x001697A4 File Offset: 0x001687A4
		public override Point GetAnchorPoint(object selection)
		{
			return TextSelectionHelper.GetAnchorPoint(selection);
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x001697AC File Offset: 0x001687AC
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
			IList<TextSegment> list = null;
			ITextPointer textPointer;
			ITextPointer position;
			TextSelectionHelper.CheckSelection(selection, out textPointer, out position, out list);
			if (!(textPointer is TextPointer))
			{
				throw new ArgumentException(SR.Get("WrongSelectionType"), "selection");
			}
			ITextPointer textPointer2;
			ITextPointer textPointer3;
			if (!this.GetNodesStartAndEnd(startNode, out textPointer2, out textPointer3))
			{
				return null;
			}
			if (textPointer2.CompareTo(position) > 0)
			{
				throw new ArgumentException(SR.Get("InvalidStartNodeForTextSelection"), "startNode");
			}
			if (textPointer3.CompareTo(textPointer) < 0)
			{
				throw new ArgumentException(SR.Get("InvalidStartNodeForTextSelection"), "startNode");
			}
			ContentLocatorPart contentLocatorPart = new ContentLocatorPart(TextSelectionProcessor.CharacterRangeElementName);
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				this.GetTextSegmentValues(list[i], textPointer2, textPointer3, out num, out num2);
				contentLocatorPart.NameValuePairs.Add("Segment" + i.ToString(NumberFormatInfo.InvariantInfo), num.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + num2.ToString(NumberFormatInfo.InvariantInfo));
			}
			contentLocatorPart.NameValuePairs.Add("Count", list.Count.ToString(NumberFormatInfo.InvariantInfo));
			return new List<ContentLocatorPart>(1)
			{
				contentLocatorPart
			};
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0016990C File Offset: 0x0016890C
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (TextSelectionProcessor.CharacterRangeElementName != locatorPart.PartType)
			{
				throw new ArgumentException(SR.Get("IncorrectLocatorPartType", new object[]
				{
					locatorPart.PartType.Namespace + ":" + locatorPart.PartType.Name
				}), "locatorPart");
			}
			int num = 0;
			int num2 = 0;
			string text = locatorPart.NameValuePairs["Count"];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Count"
				}));
			}
			int num3 = int.Parse(text, NumberFormatInfo.InvariantInfo);
			TextAnchor textAnchor = new TextAnchor();
			attachmentLevel = AttachmentLevel.Unresolved;
			for (int i = 0; i < num3; i++)
			{
				TextSelectionProcessor.GetLocatorPartSegmentValues(locatorPart, i, out num, out num2);
				ITextPointer textPointer;
				ITextPointer textPointer2;
				if (!this.GetNodesStartAndEnd(startNode, out textPointer, out textPointer2))
				{
					return null;
				}
				int offsetToPosition = textPointer.GetOffsetToPosition(textPointer2);
				if (num > offsetToPosition)
				{
					return null;
				}
				ITextPointer textPointer3 = textPointer.CreatePointer(num);
				ITextPointer textPointer4 = (offsetToPosition <= num2) ? textPointer2.CreatePointer() : textPointer.CreatePointer(num2);
				if (textPointer3.CompareTo(textPointer4) >= 0)
				{
					return null;
				}
				textAnchor.AddTextSegment(textPointer3, textPointer4);
			}
			if (textAnchor.IsEmpty)
			{
				throw new ArgumentException(SR.Get("IncorrectAnchorLength"), "locatorPart");
			}
			attachmentLevel = AttachmentLevel.Full;
			if (this._clamping)
			{
				ITextPointer start = textAnchor.Start;
				ITextPointer end = textAnchor.End;
				IServiceProvider serviceProvider;
				if (this._targetPage != null)
				{
					serviceProvider = this._targetPage;
				}
				else
				{
					serviceProvider = (PathNode.GetParent(start.TextContainer.Parent as FlowDocument) as IServiceProvider);
				}
				Invariant.Assert(serviceProvider != null, "No ServiceProvider found to get TextView from.");
				ITextView textView = serviceProvider.GetService(typeof(ITextView)) as ITextView;
				Invariant.Assert(textView != null, "Null TextView provided by ServiceProvider.");
				textAnchor = TextAnchor.TrimToIntersectionWith(textAnchor, textView.TextSegments);
				if (textAnchor == null)
				{
					attachmentLevel = AttachmentLevel.Unresolved;
				}
				else
				{
					if (textAnchor.Start.CompareTo(start) != 0)
					{
						attachmentLevel &= ~AttachmentLevel.StartPortion;
					}
					if (textAnchor.End.CompareTo(end) != 0)
					{
						attachmentLevel &= ~AttachmentLevel.EndPortion;
					}
				}
			}
			return textAnchor;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x00169B3B File Offset: 0x00168B3B
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TextSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x17000512 RID: 1298
		// (set) Token: 0x06001B91 RID: 7057 RVA: 0x00169B4C File Offset: 0x00168B4C
		internal bool Clamping
		{
			set
			{
				this._clamping = value;
			}
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x00169B58 File Offset: 0x00168B58
		internal static void GetMaxMinLocatorPartValues(ContentLocatorPart locatorPart, out int startOffset, out int endOffset)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			string text = locatorPart.NameValuePairs["Count"];
			if (text == null)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Count"
				}));
			}
			int num = int.Parse(text, NumberFormatInfo.InvariantInfo);
			startOffset = int.MaxValue;
			endOffset = 0;
			for (int i = 0; i < num; i++)
			{
				int num2;
				int num3;
				TextSelectionProcessor.GetLocatorPartSegmentValues(locatorPart, i, out num2, out num3);
				if (num2 < startOffset)
				{
					startOffset = num2;
				}
				if (num3 > endOffset)
				{
					endOffset = num3;
				}
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x00169BE7 File Offset: 0x00168BE7
		internal void SetTargetDocumentPageView(DocumentPageView target)
		{
			this._targetPage = target;
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x00169BF0 File Offset: 0x00168BF0
		private static void GetLocatorPartSegmentValues(ContentLocatorPart locatorPart, int segmentNumber, out int startOffset, out int endOffset)
		{
			if (segmentNumber < 0)
			{
				throw new ArgumentException("segmentNumber");
			}
			string[] array = locatorPart.NameValuePairs["Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)].Split(TextSelectionProcessor.Separator);
			if (array.Length != 2)
			{
				throw new ArgumentException(SR.Get("InvalidLocatorPart", new object[]
				{
					"Segment" + segmentNumber.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			startOffset = int.Parse(array[0], NumberFormatInfo.InvariantInfo);
			endOffset = int.Parse(array[1], NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00169C8C File Offset: 0x00168C8C
		private ITextContainer GetTextContainer(DependencyObject startNode)
		{
			ITextContainer textContainer = null;
			IServiceProvider serviceProvider = startNode as IServiceProvider;
			if (serviceProvider != null)
			{
				textContainer = (serviceProvider.GetService(typeof(ITextContainer)) as ITextContainer);
			}
			if (textContainer == null)
			{
				TextBoxBase textBoxBase = startNode as TextBoxBase;
				if (textBoxBase != null)
				{
					textContainer = textBoxBase.TextContainer;
				}
			}
			return textContainer;
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x00169CD0 File Offset: 0x00168CD0
		private bool GetNodesStartAndEnd(DependencyObject startNode, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			ITextContainer textContainer = this.GetTextContainer(startNode);
			if (textContainer != null)
			{
				start = textContainer.Start;
				end = textContainer.End;
			}
			else
			{
				TextElement textElement = startNode as TextElement;
				if (textElement == null)
				{
					return false;
				}
				start = textElement.ContentStart;
				end = textElement.ContentEnd;
			}
			return true;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x00169D20 File Offset: 0x00168D20
		private void GetTextSegmentValues(TextSegment segment, ITextPointer elementStart, ITextPointer elementEnd, out int startOffset, out int endOffset)
		{
			startOffset = 0;
			endOffset = 0;
			if (elementStart.CompareTo(segment.Start) >= 0)
			{
				startOffset = 0;
			}
			else
			{
				startOffset = elementStart.GetOffsetToPosition(segment.Start);
			}
			if (elementEnd.CompareTo(segment.End) >= 0)
			{
				endOffset = elementStart.GetOffsetToPosition(segment.End);
				return;
			}
			endOffset = elementStart.GetOffsetToPosition(elementEnd);
		}

		// Token: 0x04000E0F RID: 3599
		internal const string SegmentAttribute = "Segment";

		// Token: 0x04000E10 RID: 3600
		internal const string CountAttribute = "Count";

		// Token: 0x04000E11 RID: 3601
		internal const string IncludeOverlaps = "IncludeOverlaps";

		// Token: 0x04000E12 RID: 3602
		internal static readonly char[] Separator = new char[]
		{
			','
		};

		// Token: 0x04000E13 RID: 3603
		internal static readonly XmlQualifiedName CharacterRangeElementName = new XmlQualifiedName("CharacterRange", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04000E14 RID: 3604
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = new XmlQualifiedName[]
		{
			TextSelectionProcessor.CharacterRangeElementName
		};

		// Token: 0x04000E15 RID: 3605
		private DocumentPageView _targetPage;

		// Token: 0x04000E16 RID: 3606
		private bool _clamping = true;
	}
}
