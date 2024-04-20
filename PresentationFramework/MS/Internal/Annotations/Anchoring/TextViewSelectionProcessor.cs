using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Documents;
using System.Xml;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D9 RID: 729
	internal class TextViewSelectionProcessor : SelectionProcessor
	{
		// Token: 0x06001B9A RID: 7066 RVA: 0x00169DBF File Offset: 0x00168DBF
		public override bool MergeSelections(object selection1, object selection2, out object newSelection)
		{
			newSelection = null;
			return false;
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x00169DC5 File Offset: 0x00168DC5
		public override IList<DependencyObject> GetSelectedNodes(object selection)
		{
			this.VerifySelection(selection);
			return new DependencyObject[]
			{
				(DependencyObject)selection
			};
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x00169DDE File Offset: 0x00168DDE
		public override UIElement GetParent(object selection)
		{
			this.VerifySelection(selection);
			return (UIElement)selection;
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x00169DEE File Offset: 0x00168DEE
		public override Point GetAnchorPoint(object selection)
		{
			this.VerifySelection(selection);
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x00169E10 File Offset: 0x00168E10
		public override IList<ContentLocatorPart> GenerateLocatorParts(object selection, DependencyObject startNode)
		{
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			ITextView textView = this.VerifySelection(selection);
			List<ContentLocatorPart> list = new List<ContentLocatorPart>(1);
			int num;
			int num2;
			if (textView != null && textView.IsValid)
			{
				TextViewSelectionProcessor.GetTextViewTextRange(textView, out num, out num2);
			}
			else
			{
				num = -1;
				num2 = -1;
			}
			list.Add(new ContentLocatorPart(TextSelectionProcessor.CharacterRangeElementName)
			{
				NameValuePairs = 
				{
					{
						"Count",
						1.ToString(NumberFormatInfo.InvariantInfo)
					},
					{
						"Segment" + 0.ToString(NumberFormatInfo.InvariantInfo),
						num.ToString(NumberFormatInfo.InvariantInfo) + TextSelectionProcessor.Separator[0].ToString() + num2.ToString(NumberFormatInfo.InvariantInfo)
					},
					{
						"IncludeOverlaps",
						bool.TrueString
					}
				}
			});
			return list;
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x00169EEC File Offset: 0x00168EEC
		public override object ResolveLocatorPart(ContentLocatorPart locatorPart, DependencyObject startNode, out AttachmentLevel attachmentLevel)
		{
			if (locatorPart == null)
			{
				throw new ArgumentNullException("locatorPart");
			}
			if (startNode == null)
			{
				throw new ArgumentNullException("startNode");
			}
			attachmentLevel = AttachmentLevel.Unresolved;
			return null;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x00169F0E File Offset: 0x00168F0E
		public override XmlQualifiedName[] GetLocatorPartTypes()
		{
			return (XmlQualifiedName[])TextViewSelectionProcessor.LocatorPartTypeNames.Clone();
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x00169F20 File Offset: 0x00168F20
		internal static TextRange GetTextViewTextRange(ITextView textView, out int startOffset, out int endOffset)
		{
			startOffset = int.MinValue;
			endOffset = 0;
			TextRange result = null;
			IList<TextSegment> textSegments = textView.TextSegments;
			if (textSegments != null && textSegments.Count > 0)
			{
				ITextPointer start = textSegments[0].Start;
				ITextPointer end = textSegments[textSegments.Count - 1].End;
				startOffset = end.TextContainer.Start.GetOffsetToPosition(start);
				endOffset = end.TextContainer.Start.GetOffsetToPosition(end);
				result = new TextRange(start, end);
			}
			return result;
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x00169FA4 File Offset: 0x00168FA4
		private ITextView VerifySelection(object selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			IServiceProvider serviceProvider = selection as IServiceProvider;
			if (serviceProvider == null)
			{
				throw new ArgumentException(SR.Get("SelectionMustBeServiceProvider"), "selection");
			}
			return serviceProvider.GetService(typeof(ITextView)) as ITextView;
		}

		// Token: 0x04000E17 RID: 3607
		private static readonly XmlQualifiedName[] LocatorPartTypeNames = Array.Empty<XmlQualifiedName>();
	}
}
