using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020006D6 RID: 1750
	internal static class TextTreeUndo
	{
		// Token: 0x06005B99 RID: 23449 RVA: 0x00284624 File Offset: 0x00283624
		internal static void CreateInsertUndoUnit(TextContainer tree, int symbolOffset, int symbolCount)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return;
			}
			orClearUndoManager.Add(new TextTreeInsertUndoUnit(tree, symbolOffset, symbolCount));
		}

		// Token: 0x06005B9A RID: 23450 RVA: 0x0028464C File Offset: 0x0028364C
		internal static void CreateInsertElementUndoUnit(TextContainer tree, int symbolOffset, bool deep)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return;
			}
			orClearUndoManager.Add(new TextTreeInsertElementUndoUnit(tree, symbolOffset, deep));
		}

		// Token: 0x06005B9B RID: 23451 RVA: 0x00284674 File Offset: 0x00283674
		internal static void CreatePropertyUndoUnit(TextElement element, DependencyPropertyChangedEventArgs e)
		{
			TextContainer textContainer = element.TextContainer;
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(textContainer);
			if (orClearUndoManager == null)
			{
				return;
			}
			PropertyRecord propertyRecord = default(PropertyRecord);
			propertyRecord.Property = e.Property;
			propertyRecord.Value = ((e.OldValueSource == BaseValueSourceInternal.Local) ? e.OldValue : DependencyProperty.UnsetValue);
			orClearUndoManager.Add(new TextTreePropertyUndoUnit(textContainer, element.TextElementNode.GetSymbolOffset(textContainer.Generation) + 1, propertyRecord));
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x002846EC File Offset: 0x002836EC
		internal static TextTreeDeleteContentUndoUnit CreateDeleteContentUndoUnit(TextContainer tree, TextPointer start, TextPointer end)
		{
			if (start.CompareTo(end) == 0)
			{
				return null;
			}
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return null;
			}
			TextTreeDeleteContentUndoUnit textTreeDeleteContentUndoUnit = new TextTreeDeleteContentUndoUnit(tree, start, end);
			orClearUndoManager.Add(textTreeDeleteContentUndoUnit);
			return textTreeDeleteContentUndoUnit;
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x00284724 File Offset: 0x00283724
		internal static TextTreeExtractElementUndoUnit CreateExtractElementUndoUnit(TextContainer tree, TextTreeTextElementNode elementNode)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return null;
			}
			TextTreeExtractElementUndoUnit textTreeExtractElementUndoUnit = new TextTreeExtractElementUndoUnit(tree, elementNode);
			orClearUndoManager.Add(textTreeExtractElementUndoUnit);
			return textTreeExtractElementUndoUnit;
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x00284750 File Offset: 0x00283750
		internal static UndoManager GetOrClearUndoManager(ITextContainer textContainer)
		{
			UndoManager undoManager = textContainer.UndoManager;
			if (undoManager == null)
			{
				return null;
			}
			if (!undoManager.IsEnabled)
			{
				return null;
			}
			if (undoManager.OpenedUnit == null)
			{
				undoManager.Clear();
				return null;
			}
			return undoManager;
		}
	}
}
