using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006D2 RID: 1746
	internal static class TextTreeText
	{
		// Token: 0x06005B2B RID: 23339 RVA: 0x002835B4 File Offset: 0x002825B4
		internal static void InsertText(TextTreeRootTextBlock rootTextBlock, int offset, object text)
		{
			Invariant.Assert(text is string || text is char[], "Bad text parameter!");
			int logicalOffset;
			TextTreeTextBlock textTreeTextBlock = TextTreeText.FindBlock(rootTextBlock, offset, out logicalOffset);
			int textLength = TextContainer.GetTextLength(text);
			int num = textTreeTextBlock.InsertText(logicalOffset, text, 0, textLength);
			if (num < textLength)
			{
				if (textTreeTextBlock.GapOffset < 2048)
				{
					TextTreeText.InsertTextLeft(textTreeTextBlock, text, num);
					return;
				}
				TextTreeText.InsertTextRight(textTreeTextBlock, text, num);
			}
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x00283620 File Offset: 0x00282620
		internal static void RemoveText(TextTreeRootTextBlock rootTextBlock, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			int num;
			TextTreeTextBlock textTreeTextBlock = TextTreeText.FindBlock(rootTextBlock, offset, out num);
			if (textTreeTextBlock.Count == num)
			{
				textTreeTextBlock = (TextTreeTextBlock)textTreeTextBlock.GetNextNode();
				Invariant.Assert(textTreeTextBlock != null);
				num = 0;
			}
			int num2;
			TextTreeTextBlock textTreeTextBlock2 = TextTreeText.FindBlock(rootTextBlock, offset + count, out num2);
			int num3;
			SplayTreeNode splayTreeNode;
			if (num > 0 || count < textTreeTextBlock.Count)
			{
				num3 = Math.Min(count, textTreeTextBlock.Count - num);
				textTreeTextBlock.RemoveText(num, num3);
				splayTreeNode = textTreeTextBlock.GetNextNode();
			}
			else
			{
				num3 = 0;
				splayTreeNode = textTreeTextBlock;
			}
			if (count > num3)
			{
				int num4;
				SplayTreeNode splayTreeNode2;
				if (num2 < textTreeTextBlock2.Count)
				{
					num4 = num2;
					textTreeTextBlock2.RemoveText(0, num2);
					splayTreeNode2 = textTreeTextBlock2.GetPreviousNode();
				}
				else
				{
					num4 = 0;
					splayTreeNode2 = textTreeTextBlock2;
				}
				if (num3 + num4 < count)
				{
					TextTreeText.Remove((TextTreeTextBlock)splayTreeNode, (TextTreeTextBlock)splayTreeNode2);
				}
			}
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x002836E4 File Offset: 0x002826E4
		internal static char[] CutText(TextTreeRootTextBlock rootTextBlock, int offset, int count)
		{
			char[] array = new char[count];
			TextTreeText.ReadText(rootTextBlock, offset, count, array, 0);
			TextTreeText.RemoveText(rootTextBlock, offset, count);
			return array;
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x0028370C File Offset: 0x0028270C
		internal static void ReadText(TextTreeRootTextBlock rootTextBlock, int offset, int count, char[] chars, int startIndex)
		{
			if (count > 0)
			{
				int logicalOffset;
				TextTreeTextBlock textTreeTextBlock = TextTreeText.FindBlock(rootTextBlock, offset, out logicalOffset);
				for (;;)
				{
					Invariant.Assert(textTreeTextBlock != null, "Caller asked for too much text!");
					int num = textTreeTextBlock.ReadText(logicalOffset, count, chars, startIndex);
					logicalOffset = 0;
					count -= num;
					if (count == 0)
					{
						break;
					}
					startIndex += num;
					textTreeTextBlock = (TextTreeTextBlock)textTreeTextBlock.GetNextNode();
				}
			}
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x0028375F File Offset: 0x0028275F
		internal static void InsertObject(TextTreeRootTextBlock rootTextBlock, int offset)
		{
			TextTreeText.InsertText(rootTextBlock, offset, new string(char.MaxValue, 1));
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x00283773 File Offset: 0x00282773
		internal static void InsertElementEdges(TextTreeRootTextBlock rootTextBlock, int offset, int childSymbolCount)
		{
			if (childSymbolCount == 0)
			{
				TextTreeText.InsertText(rootTextBlock, offset, new string('뻯', 2));
				return;
			}
			TextTreeText.InsertText(rootTextBlock, offset, new string('뻯', 1));
			TextTreeText.InsertText(rootTextBlock, offset + childSymbolCount + 1, new string('\0', 1));
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x002837AF File Offset: 0x002827AF
		internal static void RemoveElementEdges(TextTreeRootTextBlock rootTextBlock, int offset, int symbolCount)
		{
			Invariant.Assert(symbolCount >= 2, "Element must span at least two symbols!");
			if (symbolCount == 2)
			{
				TextTreeText.RemoveText(rootTextBlock, offset, 2);
				return;
			}
			TextTreeText.RemoveText(rootTextBlock, offset + symbolCount - 1, 1);
			TextTreeText.RemoveText(rootTextBlock, offset, 1);
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x002837E4 File Offset: 0x002827E4
		private static TextTreeTextBlock FindBlock(TextTreeRootTextBlock rootTextBlock, int offset, out int localOffset)
		{
			int num;
			TextTreeTextBlock textTreeTextBlock = (TextTreeTextBlock)rootTextBlock.ContainedNode.GetSiblingAtOffset(offset, out num);
			if (textTreeTextBlock.LeftSymbolCount == offset)
			{
				TextTreeTextBlock textTreeTextBlock2 = (TextTreeTextBlock)textTreeTextBlock.GetPreviousNode();
				if (textTreeTextBlock2 != null)
				{
					textTreeTextBlock = textTreeTextBlock2;
					num -= textTreeTextBlock.SymbolCount;
					Invariant.Assert(num >= 0);
				}
			}
			localOffset = offset - num;
			Invariant.Assert(localOffset >= 0 && localOffset <= textTreeTextBlock.Count);
			return textTreeTextBlock;
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x00283854 File Offset: 0x00282854
		private static void InsertTextLeft(TextTreeTextBlock rightBlock, object text, int textOffset)
		{
			int num = -1;
			int textLength = TextContainer.GetTextLength(text);
			if (rightBlock.GapOffset == 0)
			{
				TextTreeTextBlock textTreeTextBlock = (TextTreeTextBlock)rightBlock.GetPreviousNode();
				if (textTreeTextBlock != null)
				{
					textOffset += textTreeTextBlock.InsertText(textTreeTextBlock.Count, text, textOffset, textLength);
				}
			}
			if (textOffset < textLength)
			{
				int num2 = 1;
				TextTreeTextBlock textTreeTextBlock2 = rightBlock.SplitBlock();
				textOffset += textTreeTextBlock2.InsertText(textTreeTextBlock2.Count, text, textOffset, textLength);
				if (textOffset < textLength)
				{
					int num3 = Math.Min(rightBlock.FreeCapacity, textLength - textOffset);
					num = textLength - num3;
					rightBlock.InsertText(0, text, num, textLength);
					if (textOffset < num)
					{
						num2 += (num - textOffset + 4096 - 1) / 4096;
					}
				}
				for (int i = 1; i < num2; i++)
				{
					TextTreeTextBlock textTreeTextBlock3 = new TextTreeTextBlock(4096);
					textOffset += textTreeTextBlock3.InsertText(0, text, textOffset, num);
					textTreeTextBlock3.InsertAtNode(textTreeTextBlock2, false);
					textTreeTextBlock2 = textTreeTextBlock3;
				}
				Invariant.Assert(num2 == 1 || textOffset == num, "Not all text copied!");
			}
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x0028394C File Offset: 0x0028294C
		private static void InsertTextRight(TextTreeTextBlock leftBlock, object text, int textOffset)
		{
			int num = TextContainer.GetTextLength(text);
			if (leftBlock.GapOffset == leftBlock.Count)
			{
				TextTreeTextBlock textTreeTextBlock = (TextTreeTextBlock)leftBlock.GetNextNode();
				if (textTreeTextBlock != null)
				{
					int num2 = Math.Min(textTreeTextBlock.FreeCapacity, num - textOffset);
					textTreeTextBlock.InsertText(0, text, num - num2, num);
					num -= num2;
				}
			}
			if (textOffset < num)
			{
				int num3 = 1;
				TextTreeTextBlock textTreeTextBlock2 = leftBlock.SplitBlock();
				int num2 = Math.Min(textTreeTextBlock2.FreeCapacity, num - textOffset);
				textTreeTextBlock2.InsertText(0, text, num - num2, num);
				num -= num2;
				if (textOffset < num)
				{
					textOffset += leftBlock.InsertText(leftBlock.Count, text, textOffset, num);
					if (textOffset < num)
					{
						num3 += (num - textOffset + 4096 - 1) / 4096;
					}
				}
				for (int i = 0; i < num3 - 1; i++)
				{
					TextTreeTextBlock textTreeTextBlock3 = new TextTreeTextBlock(4096);
					textOffset += textTreeTextBlock3.InsertText(0, text, textOffset, num);
					textTreeTextBlock3.InsertAtNode(leftBlock, false);
					leftBlock = textTreeTextBlock3;
				}
				Invariant.Assert(textOffset == num, "Not all text copied!");
			}
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x00283A50 File Offset: 0x00282A50
		internal static void Remove(TextTreeTextBlock firstNode, TextTreeTextBlock lastNode)
		{
			SplayTreeNode previousNode = firstNode.GetPreviousNode();
			SplayTreeNode splayTreeNode;
			if (previousNode != null)
			{
				previousNode.Split();
				splayTreeNode = previousNode.ParentNode;
				previousNode.ParentNode = null;
			}
			else
			{
				splayTreeNode = firstNode.GetContainingNode();
			}
			SplayTreeNode rightSubTree = lastNode.Split();
			SplayTreeNode splayTreeNode2 = SplayTreeNode.Join(previousNode, rightSubTree);
			if (splayTreeNode != null)
			{
				splayTreeNode.ContainedNode = splayTreeNode2;
			}
			if (splayTreeNode2 != null)
			{
				splayTreeNode2.ParentNode = splayTreeNode;
			}
		}
	}
}
