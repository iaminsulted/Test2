using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200012C RID: 300
	internal sealed class ListParagraph : ContainerParagraph
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x0011368E File Offset: 0x0011268E
		internal ListParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00114058 File Offset: 0x00113058
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			ListParaClient listParaClient = new ListParaClient(this);
			paraClientHandle = listParaClient.Handle;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00114074 File Offset: 0x00113074
		protected override BaseParagraph GetParagraph(ITextPointer textPointer, bool fEmptyOk)
		{
			Invariant.Assert(textPointer is TextPointer);
			BaseParagraph baseParagraph = null;
			while (baseParagraph == null)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart)
				{
					TextElement adjacentElementFromOuterPosition = ((TextPointer)textPointer).GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					if (adjacentElementFromOuterPosition is ListItem)
					{
						baseParagraph = new ListItemParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
						break;
					}
					if (adjacentElementFromOuterPosition is List)
					{
						baseParagraph = new ListParagraph(adjacentElementFromOuterPosition, base.StructuralCache);
						break;
					}
					if (((TextPointer)textPointer).IsFrozen)
					{
						textPointer = textPointer.CreatePointer();
					}
					textPointer.MoveToPosition(adjacentElementFromOuterPosition.ElementEnd);
				}
				else if (pointerContext == TextPointerContext.ElementEnd)
				{
					if (base.Element == ((TextPointer)textPointer).Parent)
					{
						break;
					}
					if (((TextPointer)textPointer).IsFrozen)
					{
						textPointer = textPointer.CreatePointer();
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				else
				{
					if (((TextPointer)textPointer).IsFrozen)
					{
						textPointer = textPointer.CreatePointer();
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			if (baseParagraph != null)
			{
				base.StructuralCache.CurrentFormatContext.DependentMax = (TextPointer)textPointer;
			}
			return baseParagraph;
		}
	}
}
