using System;
using System.ComponentModel;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006DB RID: 1755
	internal static class ValidationHelper
	{
		// Token: 0x06005C62 RID: 23650 RVA: 0x00286438 File Offset: 0x00285438
		internal static void VerifyPosition(ITextContainer tree, ITextPointer position)
		{
			ValidationHelper.VerifyPosition(tree, position, "position");
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x00286446 File Offset: 0x00285446
		internal static void VerifyPosition(ITextContainer container, ITextPointer position, string paramName)
		{
			if (position == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (position.TextContainer != container)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedTree", new object[]
				{
					paramName
				}));
			}
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x00286478 File Offset: 0x00285478
		internal static void VerifyPositionPair(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (startPosition == null)
			{
				throw new ArgumentNullException("startPosition");
			}
			if (endPosition == null)
			{
				throw new ArgumentNullException("endPosition");
			}
			if (startPosition.TextContainer != endPosition.TextContainer)
			{
				throw new ArgumentException(SR.Get("InDifferentTextContainers", new object[]
				{
					"startPosition",
					"endPosition"
				}));
			}
			if (startPosition.CompareTo(endPosition) > 0)
			{
				throw new ArgumentException(SR.Get("BadTextPositionOrder", new object[]
				{
					"startPosition",
					"endPosition"
				}));
			}
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x00286505 File Offset: 0x00285505
		internal static void VerifyDirection(LogicalDirection direction, string argumentName)
		{
			if (direction != LogicalDirection.Forward && direction != LogicalDirection.Backward)
			{
				throw new InvalidEnumArgumentException(argumentName, (int)direction, typeof(LogicalDirection));
			}
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x00286520 File Offset: 0x00285520
		internal static void VerifyElementEdge(ElementEdge edge, string param)
		{
			if (edge != ElementEdge.BeforeStart && edge != ElementEdge.AfterStart && edge != ElementEdge.BeforeEnd && edge != ElementEdge.AfterEnd)
			{
				throw new InvalidEnumArgumentException(param, (int)edge, typeof(ElementEdge));
			}
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x00286544 File Offset: 0x00285544
		internal static void ValidateChild(TextPointer position, object child, string paramName)
		{
			Invariant.Assert(position != null);
			if (child == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (!TextSchema.IsValidChild(position, child.GetType()))
			{
				throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					position.Parent.GetType().Name,
					child.GetType().Name
				}));
			}
			if (child is TextElement)
			{
				if (((TextElement)child).Parent != null)
				{
					throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
					{
						child.GetType().Name
					}));
				}
			}
			else
			{
				Invariant.Assert(child is UIElement);
			}
		}
	}
}
