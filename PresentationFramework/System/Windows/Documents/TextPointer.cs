using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006B5 RID: 1717
	public class TextPointer : ContentPosition, ITextPointer
	{
		// Token: 0x0600573B RID: 22331 RVA: 0x0026DC5E File Offset: 0x0026CC5E
		internal TextPointer(TextPointer textPointer)
		{
			if (textPointer == null)
			{
				throw new ArgumentNullException("textPointer");
			}
			this.InitializeOffset(textPointer, 0, textPointer.GetGravityInternal());
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x0026DC82 File Offset: 0x0026CC82
		internal TextPointer(TextPointer position, int offset)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			this.InitializeOffset(position, offset, position.GetGravityInternal());
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x0026DCA6 File Offset: 0x0026CCA6
		internal TextPointer(TextPointer position, LogicalDirection direction)
		{
			this.InitializeOffset(position, 0, direction);
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x0026DCB7 File Offset: 0x0026CCB7
		internal TextPointer(TextPointer position, int offset, LogicalDirection direction)
		{
			this.InitializeOffset(position, offset, direction);
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x0026DCC8 File Offset: 0x0026CCC8
		internal TextPointer(TextContainer textContainer, int offset, LogicalDirection direction)
		{
			if (offset < 1 || offset > textContainer.InternalSymbolCount - 1)
			{
				throw new ArgumentException(SR.Get("BadDistance"));
			}
			SplayTreeNode splayTreeNode;
			ElementEdge edge;
			textContainer.GetNodeAndEdgeAtOffset(offset, out splayTreeNode, out edge);
			this.Initialize(textContainer, (TextTreeNode)splayTreeNode, edge, direction, textContainer.PositionGeneration, false, false, textContainer.LayoutGeneration);
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x0026DD24 File Offset: 0x0026CD24
		internal TextPointer(TextContainer tree, TextTreeNode node, ElementEdge edge)
		{
			this.Initialize(tree, node, edge, LogicalDirection.Forward, tree.PositionGeneration, false, false, tree.LayoutGeneration);
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x0026DD50 File Offset: 0x0026CD50
		internal TextPointer(TextContainer tree, TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			this.Initialize(tree, node, edge, direction, tree.PositionGeneration, false, false, tree.LayoutGeneration);
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x0026DD7C File Offset: 0x0026CD7C
		internal TextPointer CreatePointer()
		{
			return new TextPointer(this);
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x0026DD84 File Offset: 0x0026CD84
		internal TextPointer CreatePointer(LogicalDirection gravity)
		{
			return new TextPointer(this, gravity);
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x0026DD8D File Offset: 0x0026CD8D
		public bool IsInSameDocument(TextPointer textPosition)
		{
			if (textPosition == null)
			{
				throw new ArgumentNullException("textPosition");
			}
			this._tree.EmptyDeadPositionList();
			return this.TextContainer == textPosition.TextContainer;
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x0026DDB8 File Offset: 0x0026CDB8
		public int CompareTo(TextPointer position)
		{
			this._tree.EmptyDeadPositionList();
			ValidationHelper.VerifyPosition(this._tree, position);
			this.SyncToTreeGeneration();
			position.SyncToTreeGeneration();
			int symbolOffset = this.GetSymbolOffset();
			int symbolOffset2 = position.GetSymbolOffset();
			int result;
			if (symbolOffset < symbolOffset2)
			{
				result = -1;
			}
			else if (symbolOffset > symbolOffset2)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x0026DE0C File Offset: 0x0026CE0C
		public TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (direction != LogicalDirection.Forward)
			{
				return TextPointer.GetPointerContextBackward(this._node, this.Edge);
			}
			return TextPointer.GetPointerContextForward(this._node, this.Edge);
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x0026DE5C File Offset: 0x0026CE5C
		public int GetTextRunLength(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			int num = 0;
			if (this._tree.PlainTextOnly)
			{
				Invariant.Assert(this.GetScopingNode() is TextTreeRootNode);
				if (direction == LogicalDirection.Forward)
				{
					num = this._tree.InternalSymbolCount - this.GetSymbolOffset() - 1;
				}
				else
				{
					num = this.GetSymbolOffset() - 1;
				}
			}
			else
			{
				for (TextTreeNode textTreeNode = this.GetAdjacentTextNodeSibling(direction); textTreeNode != null; textTreeNode = (((direction == LogicalDirection.Forward) ? textTreeNode.GetNextNode() : textTreeNode.GetPreviousNode()) as TextTreeTextNode))
				{
					num += textTreeNode.SymbolCount;
				}
			}
			return num;
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x0026DEFC File Offset: 0x0026CEFC
		public int GetOffsetToPosition(TextPointer position)
		{
			this._tree.EmptyDeadPositionList();
			ValidationHelper.VerifyPosition(this._tree, position);
			this.SyncToTreeGeneration();
			position.SyncToTreeGeneration();
			return position.GetSymbolOffset() - this.GetSymbolOffset();
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x0026DF2E File Offset: 0x0026CF2E
		public string GetTextInRun(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x0026DF44 File Offset: 0x0026CF44
		public int GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.SyncToTreeGeneration();
			TextTreeTextNode adjacentTextNodeSibling = this.GetAdjacentTextNodeSibling(direction);
			if (adjacentTextNodeSibling != null)
			{
				return TextPointer.GetTextInRun(this._tree, this.GetSymbolOffset(), adjacentTextNodeSibling, -1, direction, textBuffer, startIndex, count);
			}
			return 0;
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x0026DF87 File Offset: 0x0026CF87
		public DependencyObject GetAdjacentElement(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointer.GetAdjacentElement(this._node, this.Edge, direction);
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x0026DFB7 File Offset: 0x0026CFB7
		public TextPointer GetPositionAtOffset(int offset)
		{
			return this.GetPositionAtOffset(offset, this.LogicalDirection);
		}

		// Token: 0x0600574D RID: 22349 RVA: 0x0026DFC8 File Offset: 0x0026CFC8
		public TextPointer GetPositionAtOffset(int offset, LogicalDirection direction)
		{
			TextPointer textPointer = new TextPointer(this, direction);
			if (textPointer.MoveByOffset(offset) == offset)
			{
				textPointer.Freeze();
				return textPointer;
			}
			return null;
		}

		// Token: 0x0600574E RID: 22350 RVA: 0x0026DFF0 File Offset: 0x0026CFF0
		public TextPointer GetNextContextPosition(LogicalDirection direction)
		{
			return (TextPointer)((ITextPointer)this).GetNextContextPosition(direction);
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x0026DFFE File Offset: 0x0026CFFE
		public TextPointer GetInsertionPosition(LogicalDirection direction)
		{
			return (TextPointer)((ITextPointer)this).GetInsertionPosition(direction);
		}

		// Token: 0x06005750 RID: 22352 RVA: 0x0026E00C File Offset: 0x0026D00C
		internal TextPointer GetInsertionPosition()
		{
			return this.GetInsertionPosition(LogicalDirection.Forward);
		}

		// Token: 0x06005751 RID: 22353 RVA: 0x0026E015 File Offset: 0x0026D015
		public TextPointer GetNextInsertionPosition(LogicalDirection direction)
		{
			return (TextPointer)((ITextPointer)this).GetNextInsertionPosition(direction);
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x0026E024 File Offset: 0x0026D024
		public TextPointer GetLineStartPosition(int count)
		{
			int num;
			TextPointer lineStartPosition = this.GetLineStartPosition(count, out num);
			if (num == count)
			{
				return lineStartPosition;
			}
			return null;
		}

		// Token: 0x06005753 RID: 22355 RVA: 0x0026E044 File Offset: 0x0026D044
		public TextPointer GetLineStartPosition(int count, out int actualCount)
		{
			this.ValidateLayout();
			TextPointer textPointer = new TextPointer(this);
			if (this.HasValidLayout)
			{
				actualCount = textPointer.MoveToLineBoundary(count);
			}
			else
			{
				actualCount = 0;
			}
			textPointer.SetLogicalDirection(LogicalDirection.Forward);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06005754 RID: 22356 RVA: 0x0026E083 File Offset: 0x0026D083
		public Rect GetCharacterRect(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			this.ValidateLayout();
			if (!this.HasValidLayout)
			{
				return Rect.Empty;
			}
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x0026E0C0 File Offset: 0x0026D0C0
		public void InsertTextInRun(string textData)
		{
			if (textData == null)
			{
				throw new ArgumentNullException("textData");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextPointer position;
			if (TextSchema.IsInTextContent(this))
			{
				position = this;
			}
			else
			{
				position = TextRangeEditTables.EnsureInsertionPosition(this);
			}
			this._tree.BeginChange();
			try
			{
				this._tree.InsertTextInternal(position, textData);
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		// Token: 0x06005756 RID: 22358 RVA: 0x0026E138 File Offset: 0x0026D138
		public int DeleteTextInRun(int count)
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (!TextSchema.IsInTextContent(this))
			{
				return 0;
			}
			LogicalDirection direction = (count < 0) ? LogicalDirection.Backward : LogicalDirection.Forward;
			int textRunLength = this.GetTextRunLength(direction);
			if (count > 0 && count > textRunLength)
			{
				count = textRunLength;
			}
			else if (count < 0 && count < -textRunLength)
			{
				count = -textRunLength;
			}
			TextPointer textPointer = new TextPointer(this, count);
			this._tree.BeginChange();
			try
			{
				if (count > 0)
				{
					this._tree.DeleteContentInternal(this, textPointer);
				}
				else if (count < 0)
				{
					this._tree.DeleteContentInternal(textPointer, this);
				}
			}
			finally
			{
				this._tree.EndChange();
			}
			return count;
		}

		// Token: 0x06005757 RID: 22359 RVA: 0x0026E1E4 File Offset: 0x0026D1E4
		internal void InsertTextElement(TextElement textElement)
		{
			Invariant.Assert(textElement != null);
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			ValidationHelper.ValidateChild(this, textElement, "textElement");
			if (textElement.Parent != null)
			{
				throw new InvalidOperationException(SR.Get("TextPointer_CannotInsertTextElementBecauseItBelongsToAnotherTree"));
			}
			textElement.RepositionWithContent(this);
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x0026E238 File Offset: 0x0026D238
		public TextPointer InsertParagraphBreak()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (this.TextContainer.Parent != null)
			{
				Type type = this.TextContainer.Parent.GetType();
				if (!TextSchema.IsValidChildOfContainer(type, typeof(Paragraph)))
				{
					throw new InvalidOperationException(SR.Get("TextSchema_IllegalElement", new object[]
					{
						"Paragraph",
						type
					}));
				}
			}
			Inline nonMergeableInlineAncestor = this.GetNonMergeableInlineAncestor();
			if (nonMergeableInlineAncestor != null)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_CannotSplitElement", new object[]
				{
					nonMergeableInlineAncestor.GetType().Name
				}));
			}
			this._tree.BeginChange();
			TextPointer result;
			try
			{
				result = TextRangeEdit.InsertParagraphBreak(this, true);
			}
			finally
			{
				this._tree.EndChange();
			}
			return result;
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x0026E308 File Offset: 0x0026D308
		public TextPointer InsertLineBreak()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			this._tree.BeginChange();
			TextPointer result;
			try
			{
				result = TextRangeEdit.InsertLineBreak(this);
			}
			finally
			{
				this._tree.EndChange();
			}
			return result;
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x0025DD21 File Offset: 0x0025CD21
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x0600575B RID: 22363 RVA: 0x0026E358 File Offset: 0x0026D358
		public bool HasValidLayout
		{
			get
			{
				return this._tree.TextView != null && this._tree.TextView.IsValid && this._tree.TextView.Contains(this);
			}
		}

		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x0600575C RID: 22364 RVA: 0x0026E38E File Offset: 0x0026D38E
		public LogicalDirection LogicalDirection
		{
			get
			{
				return this.GetGravityInternal();
			}
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x0600575D RID: 22365 RVA: 0x0026E396 File Offset: 0x0026D396
		public DependencyObject Parent
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return this.GetLogicalTreeNode();
			}
		}

		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x0600575E RID: 22366 RVA: 0x0026E3AF File Offset: 0x0026D3AF
		public bool IsAtInsertionPosition
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x0600575F RID: 22367 RVA: 0x0026E3C8 File Offset: 0x0026D3C8
		public bool IsAtLineStartPosition
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				this.ValidateLayout();
				if (!this.HasValidLayout)
				{
					return false;
				}
				TextSegment lineRange = this._tree.TextView.GetLineRange(this);
				if (!lineRange.IsNull)
				{
					TextPointer textPointer = new TextPointer(this);
					TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
					while ((pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.ElementEnd) && TextSchema.IsFormattingType(textPointer.GetAdjacentElement(LogicalDirection.Backward).GetType()))
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
						pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
					}
					if (textPointer.CompareTo((TextPointer)lineRange.Start) <= 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x06005760 RID: 22368 RVA: 0x0026E466 File Offset: 0x0026D466
		public Paragraph Paragraph
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return this.ParentBlock as Paragraph;
			}
		}

		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x06005761 RID: 22369 RVA: 0x0026E484 File Offset: 0x0026D484
		internal Block ParagraphOrBlockUIContainer
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				Block parentBlock = this.ParentBlock;
				if (!(parentBlock is Paragraph) && !(parentBlock is BlockUIContainer))
				{
					return null;
				}
				return parentBlock;
			}
		}

		// Token: 0x17001486 RID: 5254
		// (get) Token: 0x06005762 RID: 22370 RVA: 0x0026E4BC File Offset: 0x0026D4BC
		public TextPointer DocumentStart
		{
			get
			{
				return this.TextContainer.Start;
			}
		}

		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x06005763 RID: 22371 RVA: 0x0026E4C9 File Offset: 0x0026D4C9
		public TextPointer DocumentEnd
		{
			get
			{
				return this.TextContainer.End;
			}
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x0026E4D8 File Offset: 0x0026D4D8
		internal Inline GetNonMergeableInlineAncestor()
		{
			Inline inline = this.Parent as Inline;
			while (inline != null && TextSchema.IsMergeableInline(inline.GetType()))
			{
				inline = (inline.Parent as Inline);
			}
			return inline;
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x0026E510 File Offset: 0x0026D510
		internal ListItem GetListAncestor()
		{
			TextElement textElement = this.Parent as TextElement;
			while (textElement != null && !(textElement is ListItem))
			{
				textElement = (textElement.Parent as TextElement);
			}
			return textElement as ListItem;
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x0026E548 File Offset: 0x0026D548
		internal static int GetTextInRun(TextContainer textContainer, int symbolOffset, TextTreeTextNode textNode, int nodeOffset, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}
			if (startIndex < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"startIndex"
				}));
			}
			if (startIndex > textBuffer.Length)
			{
				throw new ArgumentException(SR.Get("StartIndexExceedsBufferSize", new object[]
				{
					startIndex,
					textBuffer.Length
				}));
			}
			if (count < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"count"
				}));
			}
			if (count > textBuffer.Length - startIndex)
			{
				throw new ArgumentException(SR.Get("MaxLengthExceedsBufferSize", new object[]
				{
					count,
					textBuffer.Length,
					startIndex
				}));
			}
			Invariant.Assert(textNode != null, "textNode is expected to be non-null");
			textContainer.EmptyDeadPositionList();
			int num;
			if (nodeOffset < 0)
			{
				num = 0;
			}
			else
			{
				num = ((direction == LogicalDirection.Forward) ? nodeOffset : (textNode.SymbolCount - nodeOffset));
				symbolOffset += nodeOffset;
			}
			int num2 = 0;
			while (textNode != null)
			{
				num2 += Math.Min(count - num2, textNode.SymbolCount - num);
				num = 0;
				if (num2 == count)
				{
					break;
				}
				textNode = (((direction == LogicalDirection.Forward) ? textNode.GetNextNode() : textNode.GetPreviousNode()) as TextTreeTextNode);
			}
			if (direction == LogicalDirection.Backward)
			{
				symbolOffset -= num2;
			}
			if (num2 > 0)
			{
				TextTreeText.ReadText(textContainer.RootTextBlock, symbolOffset, num2, textBuffer, startIndex);
			}
			return num2;
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x0026E6AC File Offset: 0x0026D6AC
		internal static DependencyObject GetAdjacentElement(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			TextTreeNode adjacentNode = TextPointer.GetAdjacentNode(node, edge, direction);
			DependencyObject result;
			if (adjacentNode is TextTreeObjectNode)
			{
				result = ((TextTreeObjectNode)adjacentNode).EmbeddedElement;
			}
			else if (adjacentNode is TextTreeTextElementNode)
			{
				result = ((TextTreeTextElementNode)adjacentNode).TextElement;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x0026E6F4 File Offset: 0x0026D6F4
		internal void MoveToPosition(TextPointer textPosition)
		{
			ValidationHelper.VerifyPosition(this._tree, textPosition);
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			textPosition.SyncToTreeGeneration();
			this.MoveToNode(this._tree, textPosition.Node, textPosition.Edge);
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x0026E744 File Offset: 0x0026D744
		internal int MoveByOffset(int offset)
		{
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (offset != 0)
			{
				int symbolOffset = this.GetSymbolOffset();
				int num = symbolOffset + offset;
				if (num < 1)
				{
					if (offset > 0)
					{
						num = this._tree.InternalSymbolCount - 1;
						offset = num - symbolOffset;
					}
					else
					{
						offset += 1 - num;
						num = 1;
					}
				}
				else if (num > this._tree.InternalSymbolCount - 1)
				{
					offset -= num - (this._tree.InternalSymbolCount - 1);
					num = this._tree.InternalSymbolCount - 1;
				}
				SplayTreeNode splayTreeNode;
				ElementEdge edge;
				this._tree.GetNodeAndEdgeAtOffset(num, out splayTreeNode, out edge);
				this.MoveToNode(this._tree, (TextTreeNode)splayTreeNode, edge);
			}
			return offset;
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x0026E7F8 File Offset: 0x0026D7F8
		internal bool MoveToNextContextPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextTreeNode newNode;
			ElementEdge elementEdge;
			bool flag;
			if (direction == LogicalDirection.Forward)
			{
				flag = this.GetNextNodeAndEdge(out newNode, out elementEdge);
			}
			else
			{
				flag = this.GetPreviousNodeAndEdge(out newNode, out elementEdge);
			}
			if (flag)
			{
				this.SetNodeAndEdge(this.AdjustRefCounts(newNode, elementEdge, this._node, this.Edge), elementEdge);
				this.DebugAssertGeneration();
			}
			this.AssertState();
			return flag;
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x0026E86E File Offset: 0x0026D86E
		internal bool MoveToInsertionPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x0026E899 File Offset: 0x0026D899
		internal bool MoveToNextInsertionPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x0026E8C4 File Offset: 0x0026D8C4
		internal int MoveToLineBoundary(int count)
		{
			this.VerifyNotFrozen();
			this.ValidateLayout();
			if (!this.HasValidLayout)
			{
				return 0;
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointerBase.MoveToLineBoundary(this, this._tree.TextView, count);
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x0026E900 File Offset: 0x0026D900
		internal void InsertUIElement(UIElement uiElement)
		{
			if (uiElement == null)
			{
				throw new ArgumentNullException("uiElement");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			ValidationHelper.ValidateChild(this, uiElement, "uiElement");
			if (!((TextElement)this.Parent).IsEmpty)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_UIElementNotAllowedInThisPosition"));
			}
			this._tree.BeginChange();
			try
			{
				this._tree.InsertEmbeddedObjectInternal(this, uiElement);
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x0026E990 File Offset: 0x0026D990
		internal TextElement GetAdjacentElementFromOuterPosition(LogicalDirection direction)
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextTreeTextElementNode adjacentTextElementNodeSibling = this.GetAdjacentTextElementNodeSibling(direction);
			if (adjacentTextElementNodeSibling != null)
			{
				return adjacentTextElementNodeSibling.TextElement;
			}
			return null;
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x0026E9C4 File Offset: 0x0026D9C4
		internal void SetLogicalDirection(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			if (this.GetGravityInternal() != direction)
			{
				this.SyncToTreeGeneration();
				SplayTreeNode splayTreeNode = this._node;
				ElementEdge edge = this.Edge;
				ElementEdge elementEdge;
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = this._node.GetPreviousNode();
					if (splayTreeNode != null)
					{
						elementEdge = ElementEdge.AfterEnd;
						goto IL_110;
					}
					splayTreeNode = this._node.GetContainingNode();
					Invariant.Assert(splayTreeNode != null, "Bad tree state: newNode must be non-null (BeforeStart)");
					elementEdge = ElementEdge.AfterStart;
					goto IL_110;
				case ElementEdge.AfterStart:
					splayTreeNode = this._node.GetFirstContainedNode();
					if (splayTreeNode != null)
					{
						elementEdge = ElementEdge.BeforeStart;
						goto IL_110;
					}
					splayTreeNode = this._node;
					elementEdge = ElementEdge.BeforeEnd;
					goto IL_110;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					break;
				case ElementEdge.BeforeEnd:
					splayTreeNode = this._node.GetLastContainedNode();
					if (splayTreeNode != null)
					{
						elementEdge = ElementEdge.AfterEnd;
						goto IL_110;
					}
					splayTreeNode = this._node;
					elementEdge = ElementEdge.AfterStart;
					goto IL_110;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						splayTreeNode = this._node.GetNextNode();
						if (splayTreeNode != null)
						{
							elementEdge = ElementEdge.BeforeStart;
							goto IL_110;
						}
						splayTreeNode = this._node.GetContainingNode();
						Invariant.Assert(splayTreeNode != null, "Bad tree state: newNode must be non-null (AfterEnd)");
						elementEdge = ElementEdge.BeforeEnd;
						goto IL_110;
					}
					break;
				}
				Invariant.Assert(false, "Bad ElementEdge value");
				elementEdge = this.Edge;
				IL_110:
				this.SetNodeAndEdge(this.AdjustRefCounts((TextTreeNode)splayTreeNode, elementEdge, this._node, this.Edge), elementEdge);
				Invariant.Assert(this.GetGravityInternal() == direction, "Inconsistent position gravity");
			}
		}

		// Token: 0x17001488 RID: 5256
		// (get) Token: 0x06005771 RID: 22385 RVA: 0x0026EB14 File Offset: 0x0026DB14
		internal bool IsFrozen
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				return (this._flags & 16U) == 16U;
			}
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x0026EB2E File Offset: 0x0026DB2E
		internal void Freeze()
		{
			this._tree.EmptyDeadPositionList();
			this.SetIsFrozen();
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x0026EB41 File Offset: 0x0026DB41
		internal TextPointer GetFrozenPointer(LogicalDirection logicalDirection)
		{
			ValidationHelper.VerifyDirection(logicalDirection, "logicalDirection");
			this._tree.EmptyDeadPositionList();
			return (TextPointer)TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x0026EB65 File Offset: 0x0026DB65
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			this.SetLogicalDirection(direction);
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x0026EB6E File Offset: 0x0026DB6E
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return this.CompareTo((TextPointer)position);
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x0026EB7C File Offset: 0x0026DB7C
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			int num = this.Offset + 1;
			int internalOffset = this.TextContainer.GetInternalOffset(position);
			int result;
			if (num < internalOffset)
			{
				result = -1;
			}
			else if (num > internalOffset)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x0026EBB2 File Offset: 0x0026DBB2
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return this.GetOffsetToPosition((TextPointer)position);
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x0026EBC0 File Offset: 0x0026DBC0
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			return this.GetPointerContext(direction);
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x0026EBC9 File Offset: 0x0026DBC9
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			return this.GetTextRunLength(direction);
		}

		// Token: 0x0600577A RID: 22394 RVA: 0x00230052 File Offset: 0x0022F052
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x0600577B RID: 22395 RVA: 0x0026EBD2 File Offset: 0x0026DBD2
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return this.GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x0026EBDF File Offset: 0x0026DBDF
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return this.GetAdjacentElement(direction);
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x0026EBE8 File Offset: 0x0026DBE8
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			DependencyObject element = this.GetElement(direction);
			if (element == null)
			{
				return null;
			}
			return element.GetType();
		}

		// Token: 0x0600577E RID: 22398 RVA: 0x0026EC24 File Offset: 0x0026DC24
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			this._tree.EmptyDeadPositionList();
			ValidationHelper.VerifyPosition(this._tree, position);
			TextPointer textPointer = (TextPointer)position;
			this.SyncToTreeGeneration();
			textPointer.SyncToTreeGeneration();
			TextTreeNode scopingNode = this.GetScopingNode();
			TextTreeNode scopingNode2 = textPointer.GetScopingNode();
			return scopingNode == scopingNode2;
		}

		// Token: 0x0600577F RID: 22399 RVA: 0x002300F8 File Offset: 0x0022F0F8
		ITextPointer ITextPointer.GetNextContextPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextContextPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x00230120 File Offset: 0x0022F120
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x00230136 File Offset: 0x0022F136
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x0023014C File Offset: 0x0022F14C
		ITextPointer ITextPointer.GetNextInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextInsertionPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x0026EC6C File Offset: 0x0026DC6C
		object ITextPointer.GetValue(DependencyProperty formattingProperty)
		{
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			DependencyObject dependencyParent = this.GetDependencyParent();
			object result;
			if (dependencyParent == null)
			{
				result = DependencyProperty.UnsetValue;
			}
			else
			{
				result = dependencyParent.GetValue(formattingProperty);
			}
			return result;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x0026ECB4 File Offset: 0x0026DCB4
		object ITextPointer.ReadLocalValue(DependencyProperty formattingProperty)
		{
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextElement textElement = this.Parent as TextElement;
			if (textElement == null)
			{
				throw new InvalidOperationException(SR.Get("NoScopingElement", new object[]
				{
					"This TextPointer"
				}));
			}
			return textElement.ReadLocalValue(formattingProperty);
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x0026ED14 File Offset: 0x0026DD14
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			DependencyObject dependencyObject = this.Parent as TextElement;
			if (dependencyObject == null)
			{
				return new DependencyObject().GetLocalValueEnumerator();
			}
			return dependencyObject.GetLocalValueEnumerator();
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x0026ED52 File Offset: 0x0026DD52
		ITextPointer ITextPointer.CreatePointer()
		{
			return ((ITextPointer)this).CreatePointer(0, this.LogicalDirection);
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x0026ED61 File Offset: 0x0026DD61
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return new StaticTextPointer(this._tree, this._node, this._node.GetOffsetFromEdge(this.Edge));
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x0026ED96 File Offset: 0x0026DD96
		ITextPointer ITextPointer.CreatePointer(int offset)
		{
			return ((ITextPointer)this).CreatePointer(offset, this.LogicalDirection);
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x0023B1AC File Offset: 0x0023A1AC
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return ((ITextPointer)this).CreatePointer(0, gravity);
		}

		// Token: 0x0600578A RID: 22410 RVA: 0x0026EDA5 File Offset: 0x0026DDA5
		ITextPointer ITextPointer.CreatePointer(int offset, LogicalDirection gravity)
		{
			return new TextPointer(this, offset, gravity);
		}

		// Token: 0x0600578B RID: 22411 RVA: 0x0026EDAF File Offset: 0x0026DDAF
		void ITextPointer.Freeze()
		{
			this.Freeze();
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x0026EDB7 File Offset: 0x0026DDB7
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return this.GetFrozenPointer(logicalDirection);
		}

		// Token: 0x0600578D RID: 22413 RVA: 0x0026EDC0 File Offset: 0x0026DDC0
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			return this.MoveToNextContextPosition(direction);
		}

		// Token: 0x0600578E RID: 22414 RVA: 0x0026EDC9 File Offset: 0x0026DDC9
		int ITextPointer.MoveByOffset(int offset)
		{
			return this.MoveByOffset(offset);
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x0026EDD2 File Offset: 0x0026DDD2
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			this.MoveToPosition((TextPointer)position);
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x0026EDE0 File Offset: 0x0026DDE0
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
			this.MoveToElementEdge(edge);
		}

		// Token: 0x06005791 RID: 22417 RVA: 0x0026EDEC File Offset: 0x0026DDEC
		internal void MoveToElementEdge(ElementEdge edge)
		{
			ValidationHelper.VerifyElementEdge(edge, "edge");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextTreeNode scopingNode = this.GetScopingNode();
			TextTreeTextElementNode textTreeTextElementNode = scopingNode as TextTreeTextElementNode;
			if (textTreeTextElementNode != null)
			{
				this.MoveToNode(this._tree, textTreeTextElementNode, edge);
				return;
			}
			if (scopingNode is TextTreeRootNode)
			{
				return;
			}
			throw new InvalidOperationException(SR.Get("NoScopingElement", new object[]
			{
				"This TextNavigator"
			}));
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x0026EE61 File Offset: 0x0026DE61
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return this.MoveToLineBoundary(count);
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x0026EE6A File Offset: 0x0026DE6A
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return this.GetCharacterRect(direction);
		}

		// Token: 0x06005794 RID: 22420 RVA: 0x0026EE73 File Offset: 0x0026DE73
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return this.MoveToInsertionPosition(direction);
		}

		// Token: 0x06005795 RID: 22421 RVA: 0x0026EE7C File Offset: 0x0026DE7C
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return this.MoveToNextInsertionPosition(direction);
		}

		// Token: 0x06005796 RID: 22422 RVA: 0x0026EE85 File Offset: 0x0026DE85
		void ITextPointer.InsertTextInRun(string textData)
		{
			this.InsertTextInRun(textData);
		}

		// Token: 0x06005797 RID: 22423 RVA: 0x0026EE90 File Offset: 0x0026DE90
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			this._tree.BeginChange();
			try
			{
				TextRangeEditTables.DeleteContent(this, (TextPointer)limit);
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		// Token: 0x06005798 RID: 22424 RVA: 0x0026EED4 File Offset: 0x0026DED4
		bool ITextPointer.ValidateLayout()
		{
			return this.ValidateLayout();
		}

		// Token: 0x06005799 RID: 22425 RVA: 0x0026EEDC File Offset: 0x0026DEDC
		internal bool ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, this._tree.TextView);
		}

		// Token: 0x0600579A RID: 22426 RVA: 0x0026EEEF File Offset: 0x0026DEEF
		internal TextTreeTextNode GetAdjacentTextNodeSibling(LogicalDirection direction)
		{
			return this.GetAdjacentSiblingNode(direction) as TextTreeTextNode;
		}

		// Token: 0x0600579B RID: 22427 RVA: 0x0026EEFD File Offset: 0x0026DEFD
		internal static TextTreeTextNode GetAdjacentTextNodeSibling(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			return TextPointer.GetAdjacentSiblingNode(node, edge, direction) as TextTreeTextNode;
		}

		// Token: 0x0600579C RID: 22428 RVA: 0x0026EF0C File Offset: 0x0026DF0C
		internal TextTreeTextElementNode GetAdjacentTextElementNodeSibling(LogicalDirection direction)
		{
			return this.GetAdjacentSiblingNode(direction) as TextTreeTextElementNode;
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x0026EF1A File Offset: 0x0026DF1A
		internal TextTreeTextElementNode GetAdjacentTextElementNode(LogicalDirection direction)
		{
			return this.GetAdjacentNode(direction) as TextTreeTextElementNode;
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x0026EF28 File Offset: 0x0026DF28
		internal TextTreeNode GetAdjacentSiblingNode(LogicalDirection direction)
		{
			this.DebugAssertGeneration();
			return TextPointer.GetAdjacentSiblingNode(this._node, this.Edge, direction);
		}

		// Token: 0x0600579F RID: 22431 RVA: 0x0026EF44 File Offset: 0x0026DF44
		internal static TextTreeNode GetAdjacentSiblingNode(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			SplayTreeNode splayTreeNode;
			if (direction == LogicalDirection.Forward)
			{
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = node;
					goto IL_72;
				case ElementEdge.AfterStart:
					splayTreeNode = node.GetFirstContainedNode();
					goto IL_72;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				case ElementEdge.BeforeEnd:
					break;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						splayTreeNode = node.GetNextNode();
						goto IL_72;
					}
					break;
				}
				splayTreeNode = null;
			}
			else
			{
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = node.GetPreviousNode();
					goto IL_72;
				case ElementEdge.AfterStart:
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					break;
				case ElementEdge.BeforeEnd:
					splayTreeNode = node.GetLastContainedNode();
					goto IL_72;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						splayTreeNode = node;
						goto IL_72;
					}
					break;
				}
				splayTreeNode = null;
			}
			IL_72:
			return (TextTreeNode)splayTreeNode;
		}

		// Token: 0x060057A0 RID: 22432 RVA: 0x0026EFC9 File Offset: 0x0026DFC9
		internal int GetSymbolOffset()
		{
			this.DebugAssertGeneration();
			return TextPointer.GetSymbolOffset(this._tree, this._node, this.Edge);
		}

		// Token: 0x060057A1 RID: 22433 RVA: 0x0026EFE8 File Offset: 0x0026DFE8
		internal static int GetSymbolOffset(TextContainer tree, TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				return node.GetSymbolOffset(tree.Generation);
			case ElementEdge.AfterStart:
				return node.GetSymbolOffset(tree.Generation) + 1;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				return node.GetSymbolOffset(tree.Generation) + node.SymbolCount - 1;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					return node.GetSymbolOffset(tree.Generation) + node.SymbolCount;
				}
				break;
			}
			Invariant.Assert(false, "Unknown value for position edge");
			return 0;
		}

		// Token: 0x060057A2 RID: 22434 RVA: 0x0026F06F File Offset: 0x0026E06F
		internal DependencyObject GetLogicalTreeNode()
		{
			this.DebugAssertGeneration();
			return this.GetScopingNode().GetLogicalTreeNode();
		}

		// Token: 0x060057A3 RID: 22435 RVA: 0x0026F084 File Offset: 0x0026E084
		internal void SyncToTreeGeneration()
		{
			TextTreeFixupNode textTreeFixupNode = null;
			if (this._generation == this._tree.PositionGeneration)
			{
				return;
			}
			this.IsCaretUnitBoundaryCacheValid = false;
			SplayTreeNode splayTreeNode = this._node;
			ElementEdge elementEdge = this.Edge;
			for (;;)
			{
				SplayTreeNode splayTreeNode2 = splayTreeNode;
				SplayTreeNode splayTreeNode3 = splayTreeNode;
				SplayTreeNode parentNode;
				for (;;)
				{
					parentNode = splayTreeNode2.ParentNode;
					if (parentNode == null)
					{
						break;
					}
					textTreeFixupNode = (parentNode as TextTreeFixupNode);
					if (textTreeFixupNode != null)
					{
						break;
					}
					if (splayTreeNode2.Role == SplayTreeNodeRole.LocalRoot)
					{
						splayTreeNode3.Splay();
						splayTreeNode3 = parentNode;
					}
					splayTreeNode2 = parentNode;
				}
				if (parentNode == null)
				{
					break;
				}
				if (this.GetGravityInternal() == LogicalDirection.Forward)
				{
					if (elementEdge == ElementEdge.BeforeStart && textTreeFixupNode.FirstContainedNode != null)
					{
						splayTreeNode = textTreeFixupNode.FirstContainedNode;
						Invariant.Assert(elementEdge == ElementEdge.BeforeStart, "edge BeforeStart is expected");
					}
					else
					{
						splayTreeNode = textTreeFixupNode.NextNode;
						elementEdge = textTreeFixupNode.NextEdge;
					}
				}
				else if (elementEdge == ElementEdge.AfterEnd && textTreeFixupNode.LastContainedNode != null)
				{
					splayTreeNode = textTreeFixupNode.LastContainedNode;
					Invariant.Assert(elementEdge == ElementEdge.AfterEnd, "edge AfterEnd is expected");
				}
				else
				{
					splayTreeNode = textTreeFixupNode.PreviousNode;
					elementEdge = textTreeFixupNode.PreviousEdge;
				}
			}
			this.SetNodeAndEdge((TextTreeNode)splayTreeNode, elementEdge);
			this._generation = this._tree.PositionGeneration;
			this.AssertState();
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x0026F19A File Offset: 0x0026E19A
		internal TextTreeNode GetScopingNode()
		{
			return TextPointer.GetScopingNode(this._node, this.Edge);
		}

		// Token: 0x060057A5 RID: 22437 RVA: 0x0026F1B0 File Offset: 0x0026E1B0
		internal static TextTreeNode GetScopingNode(TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				break;
			case ElementEdge.AfterStart:
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
			case ElementEdge.BeforeEnd:
				goto IL_2A;
			default:
				if (edge != ElementEdge.AfterEnd)
				{
					goto IL_2A;
				}
				break;
			}
			return (TextTreeNode)node.GetContainingNode();
			IL_2A:
			return node;
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x0026F1EA File Offset: 0x0026E1EA
		internal void DebugAssertGeneration()
		{
			Invariant.Assert(this._generation == this._tree.PositionGeneration, "TextPointer not synchronized to tree generation!");
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x0026F209 File Offset: 0x0026E209
		internal bool GetNextNodeAndEdge(out TextTreeNode node, out ElementEdge edge)
		{
			this.DebugAssertGeneration();
			return TextPointer.GetNextNodeAndEdge(this._node, this.Edge, this._tree.PlainTextOnly, out node, out edge);
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x0026F230 File Offset: 0x0026E230
		internal static bool GetNextNodeAndEdge(TextTreeNode sourceNode, ElementEdge sourceEdge, bool plainTextOnly, out TextTreeNode node, out ElementEdge edge)
		{
			node = sourceNode;
			edge = sourceEdge;
			SplayTreeNode splayTreeNode = node;
			SplayTreeNode splayTreeNode2 = node;
			for (;;)
			{
				bool flag = false;
				bool flag2 = false;
				ElementEdge elementEdge = edge;
				switch (elementEdge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = splayTreeNode2.GetFirstContainedNode();
					if (splayTreeNode == null)
					{
						if (!(splayTreeNode2 is TextTreeTextElementNode))
						{
							flag = (splayTreeNode2 is TextTreeTextNode);
							edge = ElementEdge.BeforeEnd;
							goto IL_D3;
						}
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.BeforeEnd;
					}
					break;
				case ElementEdge.AfterStart:
					splayTreeNode = splayTreeNode2.GetFirstContainedNode();
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.AfterStart;
						}
						else
						{
							flag = (splayTreeNode is TextTreeTextNode);
							flag2 = (splayTreeNode.GetNextNode() is TextTreeTextNode);
							edge = ElementEdge.AfterEnd;
						}
					}
					else if (splayTreeNode2 is TextTreeTextElementNode)
					{
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.AfterEnd;
					}
					else
					{
						Invariant.Assert(splayTreeNode2 is TextTreeRootNode, "currentNode is expected to be TextTreeRootNode");
					}
					break;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					goto IL_13E;
				case ElementEdge.BeforeEnd:
					goto IL_D3;
				default:
				{
					if (elementEdge != ElementEdge.AfterEnd)
					{
						goto IL_13E;
					}
					SplayTreeNode nextNode = splayTreeNode2.GetNextNode();
					flag = (nextNode is TextTreeTextNode);
					splayTreeNode = nextNode;
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.AfterStart;
						}
						else
						{
							flag2 = (splayTreeNode.GetNextNode() is TextTreeTextNode);
						}
					}
					else
					{
						SplayTreeNode containingNode = splayTreeNode2.GetContainingNode();
						if (!(containingNode is TextTreeRootNode))
						{
							splayTreeNode = containingNode;
						}
					}
					break;
				}
				}
				IL_149:
				splayTreeNode2 = splayTreeNode;
				if (flag && flag2 && plainTextOnly)
				{
					break;
				}
				if (!flag || !flag2)
				{
					goto IL_19A;
				}
				continue;
				IL_D3:
				splayTreeNode = splayTreeNode2.GetNextNode();
				if (splayTreeNode != null)
				{
					flag2 = (splayTreeNode is TextTreeTextNode);
					edge = ElementEdge.BeforeStart;
					goto IL_149;
				}
				splayTreeNode = splayTreeNode2.GetContainingNode();
				goto IL_149;
				IL_13E:
				Invariant.Assert(false, "Unknown ElementEdge value");
				goto IL_149;
			}
			splayTreeNode = splayTreeNode.GetContainingNode();
			Invariant.Assert(splayTreeNode is TextTreeRootNode);
			if (edge == ElementEdge.BeforeStart)
			{
				edge = ElementEdge.BeforeEnd;
			}
			else
			{
				splayTreeNode = splayTreeNode.GetLastContainedNode();
				Invariant.Assert(splayTreeNode != null);
				Invariant.Assert(edge == ElementEdge.AfterEnd);
			}
			IL_19A:
			if (splayTreeNode != null)
			{
				node = (TextTreeNode)splayTreeNode;
			}
			return splayTreeNode != null;
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x0026F3E6 File Offset: 0x0026E3E6
		internal bool GetPreviousNodeAndEdge(out TextTreeNode node, out ElementEdge edge)
		{
			this.DebugAssertGeneration();
			return TextPointer.GetPreviousNodeAndEdge(this._node, this.Edge, this._tree.PlainTextOnly, out node, out edge);
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x0026F40C File Offset: 0x0026E40C
		internal static bool GetPreviousNodeAndEdge(TextTreeNode sourceNode, ElementEdge sourceEdge, bool plainTextOnly, out TextTreeNode node, out ElementEdge edge)
		{
			node = sourceNode;
			edge = sourceEdge;
			SplayTreeNode splayTreeNode = node;
			SplayTreeNode splayTreeNode2 = node;
			for (;;)
			{
				bool flag = false;
				bool flag2 = false;
				ElementEdge elementEdge = edge;
				switch (elementEdge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = splayTreeNode2.GetPreviousNode();
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.BeforeEnd;
						}
						else
						{
							flag = (splayTreeNode is TextTreeTextNode);
							flag2 = (flag && splayTreeNode.GetPreviousNode() is TextTreeTextNode);
						}
					}
					else
					{
						SplayTreeNode containingNode = splayTreeNode2.GetContainingNode();
						if (!(containingNode is TextTreeRootNode))
						{
							splayTreeNode = containingNode;
						}
					}
					break;
				case ElementEdge.AfterStart:
					goto IL_96;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					goto IL_153;
				case ElementEdge.BeforeEnd:
					splayTreeNode = splayTreeNode2.GetLastContainedNode();
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.BeforeEnd;
						}
						else
						{
							flag = (splayTreeNode is TextTreeTextNode);
							flag2 = (flag && splayTreeNode.GetPreviousNode() is TextTreeTextNode);
							edge = ElementEdge.BeforeStart;
						}
					}
					else if (splayTreeNode2 is TextTreeTextElementNode)
					{
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.BeforeStart;
					}
					else
					{
						Invariant.Assert(splayTreeNode2 is TextTreeRootNode, "currentNode is expected to be a TextTreeRootNode");
					}
					break;
				default:
					if (elementEdge != ElementEdge.AfterEnd)
					{
						goto IL_153;
					}
					splayTreeNode = splayTreeNode2.GetLastContainedNode();
					if (splayTreeNode == null)
					{
						if (!(splayTreeNode2 is TextTreeTextElementNode))
						{
							flag = (splayTreeNode2 is TextTreeTextNode);
							edge = ElementEdge.AfterStart;
							goto IL_96;
						}
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.AfterStart;
					}
					break;
				}
				IL_15E:
				splayTreeNode2 = splayTreeNode;
				if (flag && flag2 && plainTextOnly)
				{
					break;
				}
				if (!flag || !flag2)
				{
					goto IL_1AF;
				}
				continue;
				IL_96:
				splayTreeNode = splayTreeNode2.GetPreviousNode();
				if (splayTreeNode != null)
				{
					flag2 = (splayTreeNode is TextTreeTextNode);
					edge = ElementEdge.AfterEnd;
					goto IL_15E;
				}
				splayTreeNode = splayTreeNode2.GetContainingNode();
				goto IL_15E;
				IL_153:
				Invariant.Assert(false, "Unknown ElementEdge value");
				goto IL_15E;
			}
			splayTreeNode = splayTreeNode.GetContainingNode();
			Invariant.Assert(splayTreeNode is TextTreeRootNode);
			if (edge == ElementEdge.AfterEnd)
			{
				edge = ElementEdge.AfterStart;
			}
			else
			{
				splayTreeNode = splayTreeNode.GetFirstContainedNode();
				Invariant.Assert(splayTreeNode != null);
				Invariant.Assert(edge == ElementEdge.BeforeStart);
			}
			IL_1AF:
			if (splayTreeNode != null)
			{
				node = (TextTreeNode)splayTreeNode;
			}
			return splayTreeNode != null;
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x0026F5D8 File Offset: 0x0026E5D8
		internal static TextPointerContext GetPointerContextForward(TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				return node.GetPointerContext(LogicalDirection.Forward);
			case ElementEdge.AfterStart:
				if (node.ContainedNode != null)
				{
					return ((TextTreeNode)node.GetFirstContainedNode()).GetPointerContext(LogicalDirection.Forward);
				}
				break;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				goto IL_B7;
			case ElementEdge.BeforeEnd:
				break;
			default:
			{
				if (edge != ElementEdge.AfterEnd)
				{
					goto IL_B7;
				}
				TextTreeNode textTreeNode = (TextTreeNode)node.GetNextNode();
				if (textTreeNode != null)
				{
					return textTreeNode.GetPointerContext(LogicalDirection.Forward);
				}
				Invariant.Assert(node.GetContainingNode() != null, "Bad position!");
				return (node.GetContainingNode() is TextTreeRootNode) ? TextPointerContext.None : TextPointerContext.ElementEnd;
			}
			}
			Invariant.Assert(node.ParentNode != null || node is TextTreeRootNode, "Inconsistent node.ParentNode");
			return (node.ParentNode != null) ? TextPointerContext.ElementEnd : TextPointerContext.None;
			IL_B7:
			Invariant.Assert(false, "Unreachable code.");
			return TextPointerContext.Text;
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x0026F6AC File Offset: 0x0026E6AC
		internal static TextPointerContext GetPointerContextBackward(TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
			{
				TextTreeNode textTreeNode = (TextTreeNode)node.GetPreviousNode();
				if (textTreeNode != null)
				{
					return textTreeNode.GetPointerContext(LogicalDirection.Backward);
				}
				Invariant.Assert(node.GetContainingNode() != null, "Bad position!");
				return (node.GetContainingNode() is TextTreeRootNode) ? TextPointerContext.None : TextPointerContext.ElementStart;
			}
			case ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				goto IL_B7;
			case ElementEdge.BeforeEnd:
			{
				TextTreeNode textTreeNode2 = (TextTreeNode)node.GetLastContainedNode();
				if (textTreeNode2 != null)
				{
					return textTreeNode2.GetPointerContext(LogicalDirection.Backward);
				}
				break;
			}
			default:
				if (edge != ElementEdge.AfterEnd)
				{
					goto IL_B7;
				}
				return node.GetPointerContext(LogicalDirection.Backward);
			}
			Invariant.Assert(node.ParentNode != null || node is TextTreeRootNode, "Inconsistent node.ParentNode");
			return (node.ParentNode != null) ? TextPointerContext.ElementStart : TextPointerContext.None;
			IL_B7:
			Invariant.Assert(false, "Unknown ElementEdge value");
			return TextPointerContext.Text;
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x0026F780 File Offset: 0x0026E780
		internal void InsertInline(Inline inline)
		{
			TextPointer textPointer = this;
			if (!TextSchema.ValidateChild(textPointer, inline.GetType(), false, true))
			{
				if (textPointer.Parent == null)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_CannotInsertContentInThisPosition"));
				}
				textPointer = TextRangeEditTables.EnsureInsertionPosition(this);
				Invariant.Assert(textPointer.Parent is Run, "EnsureInsertionPosition() must return a position in text content");
				Run run = (Run)textPointer.Parent;
				if (run.IsEmpty)
				{
					run.RepositionWithContent(null);
				}
				else
				{
					textPointer = TextRangeEdit.SplitFormattingElement(textPointer, false);
				}
				Invariant.Assert(TextSchema.IsValidChild(textPointer, inline.GetType()));
			}
			inline.RepositionWithContent(textPointer);
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x0026F814 File Offset: 0x0026E814
		internal static DependencyObject GetCommonAncestor(TextPointer position1, TextPointer position2)
		{
			TextElement textElement = position1.Parent as TextElement;
			TextElement textElement2 = position2.Parent as TextElement;
			DependencyObject result;
			if (textElement == null)
			{
				result = position1.Parent;
			}
			else if (textElement2 == null)
			{
				result = position2.Parent;
			}
			else
			{
				result = TextElement.GetCommonAncestor(textElement, textElement2);
			}
			return result;
		}

		// Token: 0x17001489 RID: 5257
		// (get) Token: 0x060057AF RID: 22447 RVA: 0x0026F85C File Offset: 0x0026E85C
		Type ITextPointer.ParentType
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				DependencyObject parent = this.Parent;
				if (parent == null)
				{
					return null;
				}
				return parent.GetType();
			}
		}

		// Token: 0x1700148A RID: 5258
		// (get) Token: 0x060057B0 RID: 22448 RVA: 0x0026F88C File Offset: 0x0026E88C
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x1700148B RID: 5259
		// (get) Token: 0x060057B1 RID: 22449 RVA: 0x0026F894 File Offset: 0x0026E894
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return this.HasValidLayout;
			}
		}

		// Token: 0x1700148C RID: 5260
		// (get) Token: 0x060057B2 RID: 22450 RVA: 0x0026F89C File Offset: 0x0026E89C
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				this.ValidateLayout();
				if (!this.HasValidLayout)
				{
					return false;
				}
				if (this._layoutGeneration != this._tree.LayoutGeneration)
				{
					this.IsCaretUnitBoundaryCacheValid = false;
				}
				if (!this.IsCaretUnitBoundaryCacheValid)
				{
					this.CaretUnitBoundaryCache = this._tree.IsAtCaretUnitBoundary(this);
					this._layoutGeneration = this._tree.LayoutGeneration;
					this.IsCaretUnitBoundaryCacheValid = true;
				}
				return this.CaretUnitBoundaryCache;
			}
		}

		// Token: 0x1700148D RID: 5261
		// (get) Token: 0x060057B3 RID: 22451 RVA: 0x0026F91D File Offset: 0x0026E91D
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this.LogicalDirection;
			}
		}

		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x060057B4 RID: 22452 RVA: 0x0026F925 File Offset: 0x0026E925
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return this.IsAtInsertionPosition;
			}
		}

		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x060057B5 RID: 22453 RVA: 0x0026F92D File Offset: 0x0026E92D
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this.IsFrozen;
			}
		}

		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x060057B6 RID: 22454 RVA: 0x0026F935 File Offset: 0x0026E935
		int ITextPointer.Offset
		{
			get
			{
				return this.Offset;
			}
		}

		// Token: 0x17001491 RID: 5265
		// (get) Token: 0x060057B7 RID: 22455 RVA: 0x0026F93D File Offset: 0x0026E93D
		internal int Offset
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return this.GetSymbolOffset() - 1;
			}
		}

		// Token: 0x17001492 RID: 5266
		// (get) Token: 0x060057B8 RID: 22456 RVA: 0x0026F958 File Offset: 0x0026E958
		int ITextPointer.CharOffset
		{
			get
			{
				return this.CharOffset;
			}
		}

		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x060057B9 RID: 22457 RVA: 0x0026F960 File Offset: 0x0026E960
		internal int CharOffset
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				ElementEdge edge = this.Edge;
				int num;
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					return this._node.GetIMECharOffset();
				case ElementEdge.AfterStart:
				{
					num = this._node.GetIMECharOffset();
					TextTreeTextElementNode textTreeTextElementNode = this._node as TextTreeTextElementNode;
					if (textTreeTextElementNode != null)
					{
						return num + textTreeTextElementNode.IMELeftEdgeCharCount;
					}
					return num;
				}
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					goto IL_84;
				case ElementEdge.BeforeEnd:
					break;
				default:
					if (edge != ElementEdge.AfterEnd)
					{
						goto IL_84;
					}
					break;
				}
				return this._node.GetIMECharOffset() + this._node.IMECharCount;
				IL_84:
				Invariant.Assert(false, "Unknown value for position edge");
				num = 0;
				return num;
			}
		}

		// Token: 0x17001494 RID: 5268
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x0026F9FF File Offset: 0x0026E9FF
		internal TextContainer TextContainer
		{
			get
			{
				return this._tree;
			}
		}

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x060057BB RID: 22459 RVA: 0x0026FA07 File Offset: 0x0026EA07
		internal FrameworkElement ContainingFrameworkElement
		{
			get
			{
				return (FrameworkElement)this._tree.Parent;
			}
		}

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x060057BC RID: 22460 RVA: 0x0026FA19 File Offset: 0x0026EA19
		internal bool IsAtRowEnd
		{
			get
			{
				return TextPointerBase.IsAtRowEnd(this);
			}
		}

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x060057BD RID: 22461 RVA: 0x0026FA21 File Offset: 0x0026EA21
		internal bool HasNonMergeableInlineAncestor
		{
			get
			{
				return this.GetNonMergeableInlineAncestor() != null;
			}
		}

		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x060057BE RID: 22462 RVA: 0x0026FA2C File Offset: 0x0026EA2C
		internal bool IsAtNonMergeableInlineStart
		{
			get
			{
				return TextPointerBase.IsAtNonMergeableInlineStart(this);
			}
		}

		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x060057BF RID: 22463 RVA: 0x0026FA34 File Offset: 0x0026EA34
		internal TextTreeNode Node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x060057C0 RID: 22464 RVA: 0x0026FA3C File Offset: 0x0026EA3C
		internal ElementEdge Edge
		{
			get
			{
				return (ElementEdge)(this._flags & 15U);
			}
		}

		// Token: 0x1700149B RID: 5275
		// (get) Token: 0x060057C1 RID: 22465 RVA: 0x0026FA48 File Offset: 0x0026EA48
		internal Block ParentBlock
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				DependencyObject parent = this.Parent;
				while (parent is Inline && !(parent is AnchoredBlock))
				{
					parent = ((Inline)parent).Parent;
				}
				return parent as Block;
			}
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x0026FA94 File Offset: 0x0026EA94
		private void InitializeOffset(TextPointer position, int distance, LogicalDirection direction)
		{
			position.SyncToTreeGeneration();
			SplayTreeNode node;
			ElementEdge edge;
			bool isCaretUnitBoundaryCacheValid;
			if (distance != 0)
			{
				int num = position.GetSymbolOffset() + distance;
				if (num < 1 || num > position.TextContainer.InternalSymbolCount - 1)
				{
					throw new ArgumentException(SR.Get("BadDistance"));
				}
				position.TextContainer.GetNodeAndEdgeAtOffset(num, out node, out edge);
				isCaretUnitBoundaryCacheValid = false;
			}
			else
			{
				node = position.Node;
				edge = position.Edge;
				isCaretUnitBoundaryCacheValid = position.IsCaretUnitBoundaryCacheValid;
			}
			this.Initialize(position.TextContainer, (TextTreeNode)node, edge, direction, position.TextContainer.PositionGeneration, position.CaretUnitBoundaryCache, isCaretUnitBoundaryCacheValid, position._layoutGeneration);
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x0026FB2C File Offset: 0x0026EB2C
		private void Initialize(TextContainer tree, TextTreeNode node, ElementEdge edge, LogicalDirection gravity, uint generation, bool caretUnitBoundaryCache, bool isCaretUnitBoundaryCacheValid, uint layoutGeneration)
		{
			this._tree = tree;
			TextPointer.RepositionForGravity(ref node, ref edge, gravity);
			this.SetNodeAndEdge(node.IncrementReferenceCount(edge), edge);
			this._generation = generation;
			this.CaretUnitBoundaryCache = caretUnitBoundaryCache;
			this.IsCaretUnitBoundaryCacheValid = isCaretUnitBoundaryCacheValid;
			this._layoutGeneration = layoutGeneration;
			this.VerifyFlags();
			tree.AssertTree();
			this.AssertState();
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x0026FB8B File Offset: 0x0026EB8B
		private void VerifyNotFrozen()
		{
			if (this.IsFrozen)
			{
				throw new InvalidOperationException(SR.Get("TextPositionIsFrozen"));
			}
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x0026FBA8 File Offset: 0x0026EBA8
		private TextTreeNode AdjustRefCounts(TextTreeNode newNode, ElementEdge newNodeEdge, TextTreeNode oldNode, ElementEdge oldNodeEdge)
		{
			Invariant.Assert(oldNode.ParentNode == null || oldNode.IsChildOfNode(oldNode.ParentNode), "Trying to add ref a dead node!");
			Invariant.Assert(newNode.ParentNode == null || newNode.IsChildOfNode(newNode.ParentNode), "Trying to add ref a dead node!");
			TextTreeNode result = newNode;
			if (newNode != oldNode || newNodeEdge != oldNodeEdge)
			{
				result = newNode.IncrementReferenceCount(newNodeEdge);
				oldNode.DecrementReferenceCount(oldNodeEdge);
			}
			return result;
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x0026FC14 File Offset: 0x0026EC14
		private static void RepositionForGravity(ref TextTreeNode node, ref ElementEdge edge, LogicalDirection gravity)
		{
			SplayTreeNode splayTreeNode = node;
			ElementEdge elementEdge = edge;
			ElementEdge elementEdge2 = edge;
			switch (elementEdge2)
			{
			case ElementEdge.BeforeStart:
				if (gravity == LogicalDirection.Backward)
				{
					splayTreeNode = node.GetPreviousNode();
					elementEdge = ElementEdge.AfterEnd;
					if (splayTreeNode == null)
					{
						splayTreeNode = node.GetContainingNode();
						elementEdge = ElementEdge.AfterStart;
					}
				}
				break;
			case ElementEdge.AfterStart:
				if (gravity == LogicalDirection.Forward)
				{
					splayTreeNode = node.GetFirstContainedNode();
					elementEdge = ElementEdge.BeforeStart;
					if (splayTreeNode == null)
					{
						splayTreeNode = node;
						elementEdge = ElementEdge.BeforeEnd;
					}
				}
				break;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				if (gravity == LogicalDirection.Backward)
				{
					splayTreeNode = node.GetLastContainedNode();
					elementEdge = ElementEdge.AfterEnd;
					if (splayTreeNode == null)
					{
						splayTreeNode = node;
						elementEdge = ElementEdge.AfterStart;
					}
				}
				break;
			default:
				if (elementEdge2 == ElementEdge.AfterEnd)
				{
					if (gravity == LogicalDirection.Forward)
					{
						splayTreeNode = node.GetNextNode();
						elementEdge = ElementEdge.BeforeStart;
						if (splayTreeNode == null)
						{
							splayTreeNode = node.GetContainingNode();
							elementEdge = ElementEdge.BeforeEnd;
						}
					}
				}
				break;
			}
			node = (TextTreeNode)splayTreeNode;
			edge = elementEdge;
		}

		// Token: 0x060057C7 RID: 22471 RVA: 0x0026FCB9 File Offset: 0x0026ECB9
		private LogicalDirection GetGravityInternal()
		{
			if (this.Edge != ElementEdge.BeforeStart && this.Edge != ElementEdge.BeforeEnd)
			{
				return LogicalDirection.Backward;
			}
			return LogicalDirection.Forward;
		}

		// Token: 0x060057C8 RID: 22472 RVA: 0x0026FCD0 File Offset: 0x0026ECD0
		private DependencyObject GetDependencyParent()
		{
			this.DebugAssertGeneration();
			return this.GetScopingNode().GetDependencyParent();
		}

		// Token: 0x060057C9 RID: 22473 RVA: 0x0026FCE3 File Offset: 0x0026ECE3
		internal TextTreeNode GetAdjacentNode(LogicalDirection direction)
		{
			return TextPointer.GetAdjacentNode(this._node, this.Edge, direction);
		}

		// Token: 0x060057CA RID: 22474 RVA: 0x0026FCF8 File Offset: 0x0026ECF8
		internal static TextTreeNode GetAdjacentNode(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = TextPointer.GetAdjacentSiblingNode(node, edge, direction);
			if (textTreeNode == null)
			{
				if (edge == ElementEdge.AfterStart || edge == ElementEdge.BeforeEnd)
				{
					textTreeNode = node;
				}
				else
				{
					textTreeNode = (TextTreeNode)node.GetContainingNode();
				}
			}
			return textTreeNode;
		}

		// Token: 0x060057CB RID: 22475 RVA: 0x0026FD2A File Offset: 0x0026ED2A
		private void MoveToNode(TextContainer tree, TextTreeNode node, ElementEdge edge)
		{
			TextPointer.RepositionForGravity(ref node, ref edge, this.GetGravityInternal());
			this._tree = tree;
			this.SetNodeAndEdge(this.AdjustRefCounts(node, edge, this._node, this.Edge), edge);
			this._generation = tree.PositionGeneration;
		}

		// Token: 0x060057CC RID: 22476 RVA: 0x0026FD6C File Offset: 0x0026ED6C
		private TextElement GetElement(LogicalDirection direction)
		{
			this.DebugAssertGeneration();
			TextTreeTextElementNode adjacentTextElementNode = this.GetAdjacentTextElementNode(direction);
			if (adjacentTextElementNode != null)
			{
				return adjacentTextElementNode.TextElement;
			}
			return null;
		}

		// Token: 0x060057CD RID: 22477 RVA: 0x0026FD94 File Offset: 0x0026ED94
		private void AssertState()
		{
			if (Invariant.Strict)
			{
				Invariant.Assert(this._node != null, "Null position node!");
				if (this.GetGravityInternal() == LogicalDirection.Forward)
				{
					Invariant.Assert(this.Edge == ElementEdge.BeforeStart || this.Edge == ElementEdge.BeforeEnd, "Bad position edge/gravity pair! (1)");
				}
				else
				{
					Invariant.Assert(this.Edge == ElementEdge.AfterStart || this.Edge == ElementEdge.AfterEnd, "Bad position edge/gravity pair! (2)");
				}
				if (this._node is TextTreeRootNode)
				{
					Invariant.Assert(this.Edge != ElementEdge.BeforeStart && this.Edge != ElementEdge.AfterEnd, "Position at outer edge of root!");
				}
				else if (this._node is TextTreeTextNode || this._node is TextTreeObjectNode)
				{
					Invariant.Assert(this.Edge != ElementEdge.AfterStart && this.Edge != ElementEdge.BeforeEnd, "Position at inner leaf node edge!");
				}
				else
				{
					Invariant.Assert(this._node is TextTreeTextElementNode, "Unknown node type!");
				}
				Invariant.Assert(this._tree != null, "Position has no tree!");
			}
		}

		// Token: 0x060057CE RID: 22478 RVA: 0x0026FEA1 File Offset: 0x0026EEA1
		private void SetNodeAndEdge(TextTreeNode node, ElementEdge edge)
		{
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterStart || edge == ElementEdge.BeforeEnd || edge == ElementEdge.AfterEnd);
			this._node = node;
			this._flags = ((this._flags & 4294967280U) | (uint)edge);
			this.VerifyFlags();
			this.IsCaretUnitBoundaryCacheValid = false;
		}

		// Token: 0x060057CF RID: 22479 RVA: 0x0026FEE0 File Offset: 0x0026EEE0
		private void SetIsFrozen()
		{
			this._flags |= 16U;
			this.VerifyFlags();
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x0026FEF8 File Offset: 0x0026EEF8
		private void VerifyFlags()
		{
			ElementEdge elementEdge = (ElementEdge)(this._flags & 15U);
			Invariant.Assert(elementEdge == ElementEdge.BeforeStart || elementEdge == ElementEdge.AfterStart || elementEdge == ElementEdge.BeforeEnd || elementEdge == ElementEdge.AfterEnd);
		}

		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x060057D1 RID: 22481 RVA: 0x0026FF28 File Offset: 0x0026EF28
		// (set) Token: 0x060057D2 RID: 22482 RVA: 0x0026FF37 File Offset: 0x0026EF37
		private bool IsCaretUnitBoundaryCacheValid
		{
			get
			{
				return (this._flags & 32U) == 32U;
			}
			set
			{
				this._flags = ((this._flags & 4294967263U) | (value ? 32U : 0U));
				this.VerifyFlags();
			}
		}

		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x060057D3 RID: 22483 RVA: 0x0026FF57 File Offset: 0x0026EF57
		// (set) Token: 0x060057D4 RID: 22484 RVA: 0x0026FF66 File Offset: 0x0026EF66
		private bool CaretUnitBoundaryCache
		{
			get
			{
				return (this._flags & 64U) == 64U;
			}
			set
			{
				this._flags = ((this._flags & 4294967231U) | (value ? 64U : 0U));
				this.VerifyFlags();
			}
		}

		// Token: 0x04002FD3 RID: 12243
		private TextContainer _tree;

		// Token: 0x04002FD4 RID: 12244
		private TextTreeNode _node;

		// Token: 0x04002FD5 RID: 12245
		private uint _generation;

		// Token: 0x04002FD6 RID: 12246
		private uint _layoutGeneration;

		// Token: 0x04002FD7 RID: 12247
		private uint _flags;

		// Token: 0x02000B6B RID: 2923
		[Flags]
		private enum Flags
		{
			// Token: 0x040048D1 RID: 18641
			EdgeMask = 15,
			// Token: 0x040048D2 RID: 18642
			IsFrozen = 16,
			// Token: 0x040048D3 RID: 18643
			IsCaretUnitBoundaryCacheValid = 32,
			// Token: 0x040048D4 RID: 18644
			CaretUnitBoundaryCache = 64
		}
	}
}
