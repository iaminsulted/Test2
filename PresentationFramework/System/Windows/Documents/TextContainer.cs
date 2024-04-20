using System;
using System.Windows.Data;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000699 RID: 1689
	internal class TextContainer : ITextContainer
	{
		// Token: 0x060054A2 RID: 21666 RVA: 0x0025DD0A File Offset: 0x0025CD0A
		internal TextContainer(DependencyObject parent, bool plainTextOnly)
		{
			this._parent = parent;
			this.SetFlags(plainTextOnly, TextContainer.Flags.PlainTextOnly);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x0025DD21 File Offset: 0x0025CD21
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x0025DD29 File Offset: 0x0025CD29
		internal void EnableUndo(FrameworkElement uiScope)
		{
			Invariant.Assert(this._undoManager == null, SR.Get("TextContainer_UndoManagerCreatedMoreThanOnce"));
			this._undoManager = new UndoManager();
			UndoManager.AttachUndoManager(uiScope, this._undoManager);
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x0025DD5A File Offset: 0x0025CD5A
		internal void DisableUndo(FrameworkElement uiScope)
		{
			Invariant.Assert(this._undoManager != null, "UndoManager not created.");
			Invariant.Assert(this._undoManager == UndoManager.GetUndoManager(uiScope));
			UndoManager.DetachUndoManager(uiScope);
			this._undoManager = null;
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x0025DD90 File Offset: 0x0025CD90
		internal void SetValue(TextPointer position, DependencyProperty property, object value)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			this.EmptyDeadPositionList();
			this.ValidateSetValue(position);
			this.BeginChange();
			try
			{
				TextElement textElement = position.Parent as TextElement;
				Invariant.Assert(textElement != null);
				textElement.SetValue(property, value);
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x0025DE00 File Offset: 0x0025CE00
		internal void SetValues(TextPointer position, LocalValueEnumerator values)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			this.EmptyDeadPositionList();
			this.ValidateSetValue(position);
			this.BeginChange();
			try
			{
				TextElement textElement = position.Parent as TextElement;
				Invariant.Assert(textElement != null);
				values.Reset();
				while (values.MoveNext())
				{
					LocalValueEntry localValueEntry = values.Current;
					if (!(localValueEntry.Property.Name == "CachedSource") && !localValueEntry.Property.ReadOnly && localValueEntry.Property != Run.TextProperty)
					{
						BindingExpressionBase bindingExpressionBase = localValueEntry.Value as BindingExpressionBase;
						if (bindingExpressionBase != null)
						{
							textElement.SetValue(localValueEntry.Property, bindingExpressionBase.Value);
						}
						else
						{
							textElement.SetValue(localValueEntry.Property, localValueEntry.Value);
						}
					}
				}
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x0025DEE4 File Offset: 0x0025CEE4
		internal void BeginChange()
		{
			this.BeginChange(true);
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x0025DEED File Offset: 0x0025CEED
		internal void BeginChangeNoUndo()
		{
			this.BeginChange(false);
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x0025DEF6 File Offset: 0x0025CEF6
		internal void EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x0025DF00 File Offset: 0x0025CF00
		internal void EndChange(bool skipEvents)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "Unmatched EndChange call!");
			this._changeBlockLevel--;
			if (this._changeBlockLevel == 0)
			{
				try
				{
					this._rootNode.DispatcherProcessingDisabled.Dispose();
					if (this._changes != null)
					{
						TextContainerChangedEventArgs changes = this._changes;
						this._changes = null;
						if (this.ChangedHandler != null && !skipEvents)
						{
							this.ChangedHandler(this, changes);
						}
					}
				}
				finally
				{
					if (this._changeBlockUndoRecord != null)
					{
						try
						{
							this._changeBlockUndoRecord.OnEndChange();
						}
						finally
						{
							this._changeBlockUndoRecord = null;
						}
					}
				}
			}
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x0025DFB4 File Offset: 0x0025CFB4
		void ITextContainer.BeginChange()
		{
			this.BeginChange();
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x0025DFBC File Offset: 0x0025CFBC
		void ITextContainer.BeginChangeNoUndo()
		{
			this.BeginChangeNoUndo();
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x0025DEF6 File Offset: 0x0025CEF6
		void ITextContainer.EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x0025DFC4 File Offset: 0x0025CFC4
		void ITextContainer.EndChange(bool skipEvents)
		{
			this.EndChange(skipEvents);
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x0025DFCD File Offset: 0x0025CFCD
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return this.CreatePointerAtOffset(offset, direction);
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x0025DFD7 File Offset: 0x0025CFD7
		internal TextPointer CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			this.EmptyDeadPositionList();
			this.DemandCreatePositionState();
			return new TextPointer(this, offset + 1, direction);
		}

		// Token: 0x060054B2 RID: 21682 RVA: 0x0025DFEF File Offset: 0x0025CFEF
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			return this.CreatePointerAtCharOffset(charOffset, direction);
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x0025DFFC File Offset: 0x0025CFFC
		internal TextPointer CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			this.EmptyDeadPositionList();
			this.DemandCreatePositionState();
			TextTreeNode textTreeNode;
			ElementEdge edge;
			this.GetNodeAndEdgeAtCharOffset(charOffset, out textTreeNode, out edge);
			if (textTreeNode != null)
			{
				return new TextPointer(this, textTreeNode, edge, direction);
			}
			return null;
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x0025E02E File Offset: 0x0025D02E
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return this.CreatePointerAtOffset(this.GetInternalOffset(position) - 1, direction);
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x0025E040 File Offset: 0x0025D040
		internal StaticTextPointer CreateStaticPointerAtOffset(int offset)
		{
			this.EmptyDeadPositionList();
			this.DemandCreatePositionState();
			SplayTreeNode splayTreeNode;
			ElementEdge elementEdge;
			this.GetNodeAndEdgeAtOffset(offset + 1, false, out splayTreeNode, out elementEdge);
			int handle = offset + 1 - splayTreeNode.GetSymbolOffset(this.Generation);
			return new StaticTextPointer(this, splayTreeNode, handle);
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x0025E080 File Offset: 0x0025D080
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return this.CreateStaticPointerAtOffset(offset);
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x0025E08C File Offset: 0x0025D08C
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = (TextTreeNode)pointer.Handle0;
			int handle = pointer.Handle1;
			TextPointerContext result;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				result = TextPointerContext.Text;
			}
			else if (direction == LogicalDirection.Forward)
			{
				ElementEdge edgeFromOffset = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Forward);
				result = TextPointer.GetPointerContextForward(textTreeNode, edgeFromOffset);
			}
			else
			{
				ElementEdge edgeFromOffset = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Backward);
				result = TextPointer.GetPointerContextBackward(textTreeNode, edgeFromOffset);
			}
			return result;
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x0025E0F0 File Offset: 0x0025D0F0
		internal int GetInternalOffset(StaticTextPointer position)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			int result;
			if (textTreeNode is TextTreeTextNode)
			{
				result = textTreeNode.GetSymbolOffset(this.Generation) + handle;
			}
			else
			{
				result = TextPointer.GetSymbolOffset(this, textTreeNode, textTreeNode.GetEdgeFromOffsetNoBias(handle));
			}
			return result;
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x0025E13B File Offset: 0x0025D13B
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return this.GetInternalOffset(position2) - this.GetInternalOffset(position1);
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x0025E14C File Offset: 0x0025D14C
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int num = position.Handle1;
			TextTreeTextNode textTreeTextNode = textTreeNode as TextTreeTextNode;
			if (textTreeTextNode == null || num == 0 || num == textTreeNode.SymbolCount)
			{
				textTreeTextNode = TextPointer.GetAdjacentTextNodeSibling(textTreeNode, textTreeNode.GetEdgeFromOffsetNoBias(num), direction);
				num = -1;
			}
			if (textTreeTextNode != null)
			{
				return TextPointer.GetTextInRun(this, textTreeTextNode.GetSymbolOffset(this.Generation), textTreeTextNode, num, direction, textBuffer, startIndex, count);
			}
			return 0;
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x0025E1B4 File Offset: 0x0025D1B4
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			DependencyObject result;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				result = null;
			}
			else
			{
				result = TextPointer.GetAdjacentElement(textTreeNode, textTreeNode.GetEdgeFromOffset(handle, direction), direction);
			}
			return result;
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x0025E200 File Offset: 0x0025D200
		private TextTreeNode GetScopingNode(StaticTextPointer position)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			TextTreeNode result;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				result = textTreeNode;
			}
			else
			{
				result = TextPointer.GetScopingNode(textTreeNode, textTreeNode.GetEdgeFromOffsetNoBias(handle));
			}
			return result;
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x0025E24A File Offset: 0x0025D24A
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return this.GetScopingNode(position).GetLogicalTreeNode();
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x0025E258 File Offset: 0x0025D258
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			int num = this.GetInternalOffset(position) - 1;
			return ((ITextContainer)this).CreateStaticPointerAtOffset(num + offset);
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x0025E278 File Offset: 0x0025D278
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = (TextTreeNode)position.Handle0;
			int handle = position.Handle1;
			ElementEdge elementEdge;
			bool flag;
			if (textTreeNode is TextTreeTextNode && handle > 0 && handle < textTreeNode.SymbolCount)
			{
				if (this.PlainTextOnly)
				{
					textTreeNode = (TextTreeNode)textTreeNode.GetContainingNode();
					elementEdge = ((direction == LogicalDirection.Backward) ? ElementEdge.AfterStart : ElementEdge.BeforeEnd);
				}
				else
				{
					for (;;)
					{
						TextTreeTextNode textTreeTextNode = ((direction == LogicalDirection.Forward) ? textTreeNode.GetNextNode() : textTreeNode.GetPreviousNode()) as TextTreeTextNode;
						if (textTreeTextNode == null)
						{
							break;
						}
						textTreeNode = textTreeTextNode;
					}
					elementEdge = ((direction == LogicalDirection.Backward) ? ElementEdge.BeforeStart : ElementEdge.AfterEnd);
				}
				flag = true;
			}
			else if (direction == LogicalDirection.Forward)
			{
				elementEdge = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Forward);
				flag = TextPointer.GetNextNodeAndEdge(textTreeNode, elementEdge, this.PlainTextOnly, out textTreeNode, out elementEdge);
			}
			else
			{
				elementEdge = textTreeNode.GetEdgeFromOffset(handle, LogicalDirection.Backward);
				flag = TextPointer.GetPreviousNodeAndEdge(textTreeNode, elementEdge, this.PlainTextOnly, out textTreeNode, out elementEdge);
			}
			StaticTextPointer @null;
			if (flag)
			{
				@null = new StaticTextPointer(this, textTreeNode, textTreeNode.GetOffsetFromEdge(elementEdge));
			}
			else
			{
				@null = StaticTextPointer.Null;
			}
			return @null;
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x0025E358 File Offset: 0x0025D358
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			int internalOffset = this.GetInternalOffset(position1);
			int internalOffset2 = this.GetInternalOffset(position2);
			int result;
			if (internalOffset < internalOffset2)
			{
				result = -1;
			}
			else if (internalOffset > internalOffset2)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x0025E388 File Offset: 0x0025D388
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			int internalOffset = this.GetInternalOffset(position1);
			int num = position2.Offset + 1;
			int result;
			if (internalOffset < num)
			{
				result = -1;
			}
			else if (internalOffset > num)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x0025E3BC File Offset: 0x0025D3BC
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			DependencyObject dependencyParent = this.GetScopingNode(position).GetDependencyParent();
			if (dependencyParent != null)
			{
				return dependencyParent.GetValue(formattingProperty);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x0025E3E8 File Offset: 0x0025D3E8
		internal void BeforeAddChange()
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			if (this.HasListeners)
			{
				if (this.ChangingHandler != null)
				{
					this.ChangingHandler(this, EventArgs.Empty);
				}
				if (this._changes == null)
				{
					this._changes = new TextContainerChangedEventArgs();
				}
			}
		}

		// Token: 0x060054C4 RID: 21700 RVA: 0x0025E43C File Offset: 0x0025D43C
		internal void AddChange(TextPointer startPosition, int symbolCount, int charCount, PrecursorTextChangeType textChange)
		{
			this.AddChange(startPosition, symbolCount, charCount, textChange, null, false);
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x0025E44C File Offset: 0x0025D44C
		internal void AddChange(TextPointer startPosition, int symbolCount, int charCount, PrecursorTextChangeType textChange, DependencyProperty property, bool affectsRenderOnly)
		{
			Invariant.Assert(textChange != PrecursorTextChangeType.ElementAdded && textChange != PrecursorTextChangeType.ElementExtracted, "Need second TextPointer for ElementAdded/Extracted operations!");
			this.AddChange(startPosition, null, symbolCount, 0, charCount, textChange, property, affectsRenderOnly);
		}

		// Token: 0x060054C6 RID: 21702 RVA: 0x0025E484 File Offset: 0x0025D484
		internal void AddChange(TextPointer startPosition, TextPointer endPosition, int symbolCount, int leftEdgeCharCount, int childCharCount, PrecursorTextChangeType textChange, DependencyProperty property, bool affectsRenderOnly)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			Invariant.Assert(!this.CheckFlags(TextContainer.Flags.ReadOnly) || textChange == PrecursorTextChangeType.PropertyModified, "Illegal to modify TextContainer structure inside Change event scope!");
			if (this.HasListeners)
			{
				if (this._changes == null)
				{
					this._changes = new TextContainerChangedEventArgs();
				}
				Invariant.Assert(this._changes != null, "Missing call to BeforeAddChange!");
				this._changes.AddChange(textChange, startPosition.Offset, symbolCount, this.CollectTextChanges);
				if (this.ChangeHandler != null)
				{
					this.FireChangeEvent(startPosition, endPosition, symbolCount, leftEdgeCharCount, childCharCount, textChange, property, affectsRenderOnly);
				}
			}
		}

		// Token: 0x060054C7 RID: 21703 RVA: 0x0025E522 File Offset: 0x0025D522
		internal void AddLocalValueChange()
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			this._changes.SetLocalPropertyValueChanged();
		}

		// Token: 0x060054C8 RID: 21704 RVA: 0x0025E544 File Offset: 0x0025D544
		internal void InsertTextInternal(TextPointer position, object text)
		{
			Invariant.Assert(text is string || text is char[], "Unexpected type for 'text' parameter!");
			int textLength = TextContainer.GetTextLength(text);
			if (textLength == 0)
			{
				return;
			}
			this.DemandCreateText();
			position.SyncToTreeGeneration();
			if (Invariant.Strict && position.Node.SymbolCount == 0)
			{
				Invariant.Assert(position.Node is TextTreeTextNode);
				Invariant.Assert((position.Edge == ElementEdge.AfterEnd && position.Node.GetPreviousNode() is TextTreeTextNode && position.Node.GetPreviousNode().SymbolCount > 0) || (position.Edge == ElementEdge.BeforeStart && position.Node.GetNextNode() is TextTreeTextNode && position.Node.GetNextNode().SymbolCount > 0));
			}
			this.BeforeAddChange();
			TextPointer startPosition = this.HasListeners ? new TextPointer(position, LogicalDirection.Backward) : null;
			LogicalDirection logicalDirection;
			if (position.Edge == ElementEdge.BeforeStart || position.Edge == ElementEdge.BeforeEnd)
			{
				logicalDirection = LogicalDirection.Backward;
			}
			else
			{
				logicalDirection = LogicalDirection.Forward;
			}
			TextTreeTextNode textTreeTextNode = position.GetAdjacentTextNodeSibling(logicalDirection);
			if (textTreeTextNode != null && ((logicalDirection == LogicalDirection.Backward && textTreeTextNode.AfterEndReferenceCount) || (logicalDirection == LogicalDirection.Forward && textTreeTextNode.BeforeStartReferenceCount)))
			{
				textTreeTextNode = null;
			}
			SplayTreeNode containingNode;
			if (textTreeTextNode == null)
			{
				textTreeTextNode = new TextTreeTextNode();
				textTreeTextNode.InsertAtPosition(position);
				containingNode = textTreeTextNode.GetContainingNode();
			}
			else
			{
				textTreeTextNode.Splay();
				containingNode = textTreeTextNode.ParentNode;
			}
			textTreeTextNode.SymbolCount += textLength;
			this.UpdateContainerSymbolCount(containingNode, textLength, textLength);
			int symbolOffset = textTreeTextNode.GetSymbolOffset(this.Generation);
			TextTreeText.InsertText(this._rootNode.RootTextBlock, symbolOffset, text);
			TextTreeUndo.CreateInsertUndoUnit(this, symbolOffset, textLength);
			this.NextGeneration(false);
			this.AddChange(startPosition, textLength, textLength, PrecursorTextChangeType.ContentAdded);
			TextElement textElement = position.Parent as TextElement;
			if (textElement != null)
			{
				textElement.OnTextUpdated();
			}
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x0025E708 File Offset: 0x0025D708
		internal void InsertElementInternal(TextPointer startPosition, TextPointer endPosition, TextElement element)
		{
			Invariant.Assert(!this.PlainTextOnly);
			Invariant.Assert(startPosition.TextContainer == this);
			Invariant.Assert(endPosition.TextContainer == this);
			this.DemandCreateText();
			startPosition.SyncToTreeGeneration();
			endPosition.SyncToTreeGeneration();
			bool flag = startPosition.CompareTo(endPosition) != 0;
			this.BeforeAddChange();
			TextContainer.ExtractChangeEventArgs extractChangeEventArgs;
			char[] array;
			TextTreeTextElementNode textTreeTextElementNode;
			int num;
			bool flag4;
			if (element.TextElementNode != null)
			{
				bool flag2 = this == element.TextContainer;
				if (!flag2)
				{
					element.TextContainer.BeginChange();
				}
				bool flag3 = true;
				try
				{
					array = element.TextContainer.ExtractElementInternal(element, true, out extractChangeEventArgs);
					flag3 = false;
				}
				finally
				{
					if (flag3 && !flag2)
					{
						element.TextContainer.EndChange();
					}
				}
				textTreeTextElementNode = element.TextElementNode;
				num = extractChangeEventArgs.ChildIMECharCount;
				if (flag2)
				{
					startPosition.SyncToTreeGeneration();
					endPosition.SyncToTreeGeneration();
					extractChangeEventArgs.AddChange();
					extractChangeEventArgs = null;
				}
				flag4 = false;
			}
			else
			{
				array = null;
				textTreeTextElementNode = new TextTreeTextElementNode();
				num = 0;
				flag4 = true;
				extractChangeEventArgs = null;
			}
			DependencyObject logicalTreeNode = startPosition.GetLogicalTreeNode();
			TextElementCollectionHelper.MarkDirty(logicalTreeNode);
			if (flag4)
			{
				textTreeTextElementNode.TextElement = element;
				element.TextElementNode = textTreeTextElementNode;
			}
			TextTreeTextElementNode textTreeTextElementNode2 = null;
			int num2 = 0;
			if (flag)
			{
				textTreeTextElementNode2 = startPosition.GetAdjacentTextElementNodeSibling(LogicalDirection.Forward);
				if (textTreeTextElementNode2 != null)
				{
					num2 = -textTreeTextElementNode2.IMELeftEdgeCharCount;
					textTreeTextElementNode2.IMECharCount += num2;
				}
			}
			int num3 = this.InsertElementToSiblingTree(startPosition, endPosition, textTreeTextElementNode);
			num += textTreeTextElementNode.IMELeftEdgeCharCount;
			TextTreeTextElementNode textTreeTextElementNode3 = null;
			int num4 = 0;
			if (element.IsFirstIMEVisibleSibling && !flag)
			{
				textTreeTextElementNode3 = (TextTreeTextElementNode)textTreeTextElementNode.GetNextNode();
				if (textTreeTextElementNode3 != null)
				{
					num4 = textTreeTextElementNode3.IMELeftEdgeCharCount;
					textTreeTextElementNode3.IMECharCount += num4;
				}
			}
			this.UpdateContainerSymbolCount(textTreeTextElementNode.GetContainingNode(), (array == null) ? 2 : array.Length, num + num4 + num2);
			int symbolOffset = textTreeTextElementNode.GetSymbolOffset(this.Generation);
			if (flag4)
			{
				TextTreeText.InsertElementEdges(this._rootNode.RootTextBlock, symbolOffset, num3);
			}
			else
			{
				TextTreeText.InsertText(this._rootNode.RootTextBlock, symbolOffset, array);
			}
			this.NextGeneration(false);
			TextTreeUndo.CreateInsertElementUndoUnit(this, symbolOffset, array != null);
			if (extractChangeEventArgs != null)
			{
				extractChangeEventArgs.AddChange();
				extractChangeEventArgs.TextContainer.EndChange();
			}
			if (this.HasListeners)
			{
				TextPointer startPosition2 = new TextPointer(this, textTreeTextElementNode, ElementEdge.BeforeStart);
				if (num3 == 0 || array != null)
				{
					this.AddChange(startPosition2, (array == null) ? 2 : array.Length, num, PrecursorTextChangeType.ContentAdded);
				}
				else
				{
					TextPointer endPosition2 = new TextPointer(this, textTreeTextElementNode, ElementEdge.BeforeEnd);
					this.AddChange(startPosition2, endPosition2, textTreeTextElementNode.SymbolCount, textTreeTextElementNode.IMELeftEdgeCharCount, textTreeTextElementNode.IMECharCount - textTreeTextElementNode.IMELeftEdgeCharCount, PrecursorTextChangeType.ElementAdded, null, false);
				}
				if (num4 != 0)
				{
					this.RaiseEventForFormerFirstIMEVisibleNode(textTreeTextElementNode3);
				}
				if (num2 != 0)
				{
					this.RaiseEventForNewFirstIMEVisibleNode(textTreeTextElementNode2);
				}
			}
			element.BeforeLogicalTreeChange();
			try
			{
				LogicalTreeHelper.AddLogicalChild(logicalTreeNode, element);
			}
			finally
			{
				element.AfterLogicalTreeChange();
			}
			if (flag4)
			{
				this.ReparentLogicalChildren(textTreeTextElementNode, textTreeTextElementNode.TextElement, logicalTreeNode);
			}
			if (flag)
			{
				element.OnTextUpdated();
			}
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x0025E9E0 File Offset: 0x0025D9E0
		internal void InsertEmbeddedObjectInternal(TextPointer position, DependencyObject embeddedObject)
		{
			Invariant.Assert(!this.PlainTextOnly);
			this.DemandCreateText();
			position.SyncToTreeGeneration();
			this.BeforeAddChange();
			DependencyObject logicalTreeNode = position.GetLogicalTreeNode();
			TextTreeNode textTreeNode = new TextTreeObjectNode(embeddedObject);
			textTreeNode.InsertAtPosition(position);
			this.UpdateContainerSymbolCount(textTreeNode.GetContainingNode(), textTreeNode.SymbolCount, textTreeNode.IMECharCount);
			int symbolOffset = textTreeNode.GetSymbolOffset(this.Generation);
			TextTreeText.InsertObject(this._rootNode.RootTextBlock, symbolOffset);
			this.NextGeneration(false);
			TextTreeUndo.CreateInsertUndoUnit(this, symbolOffset, 1);
			LogicalTreeHelper.AddLogicalChild(logicalTreeNode, embeddedObject);
			if (this.HasListeners)
			{
				TextPointer startPosition = new TextPointer(this, textTreeNode, ElementEdge.BeforeStart);
				this.AddChange(startPosition, 1, 1, PrecursorTextChangeType.ContentAdded);
			}
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x0025EA88 File Offset: 0x0025DA88
		internal void DeleteContentInternal(TextPointer startPosition, TextPointer endPosition)
		{
			startPosition.SyncToTreeGeneration();
			endPosition.SyncToTreeGeneration();
			if (startPosition.CompareTo(endPosition) == 0)
			{
				return;
			}
			this.BeforeAddChange();
			TextTreeUndoUnit textTreeUndoUnit = TextTreeUndo.CreateDeleteContentUndoUnit(this, startPosition, endPosition);
			TextTreeNode scopingNode = startPosition.GetScopingNode();
			TextElementCollectionHelper.MarkDirty(scopingNode.GetLogicalTreeNode());
			int num = 0;
			TextTreeTextElementNode nextIMEVisibleNode = this.GetNextIMEVisibleNode(startPosition, endPosition);
			if (nextIMEVisibleNode != null)
			{
				num = -nextIMEVisibleNode.IMELeftEdgeCharCount;
				nextIMEVisibleNode.IMECharCount += num;
			}
			int num3;
			int num2 = this.CutTopLevelLogicalNodes(scopingNode, startPosition, endPosition, out num3);
			int num4;
			num2 += this.DeleteContentFromSiblingTree(scopingNode, startPosition, endPosition, num != 0, out num4);
			num3 += num4;
			Invariant.Assert(num2 > 0);
			if (textTreeUndoUnit != null)
			{
				textTreeUndoUnit.SetTreeHashCode();
			}
			TextPointer startPosition2 = new TextPointer(startPosition, LogicalDirection.Forward);
			this.AddChange(startPosition2, num2, num3, PrecursorTextChangeType.ContentRemoved);
			if (num != 0)
			{
				this.RaiseEventForNewFirstIMEVisibleNode(nextIMEVisibleNode);
			}
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x0025EB4D File Offset: 0x0025DB4D
		internal void GetNodeAndEdgeAtOffset(int offset, out SplayTreeNode node, out ElementEdge edge)
		{
			this.GetNodeAndEdgeAtOffset(offset, true, out node, out edge);
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x0025EB5C File Offset: 0x0025DB5C
		internal void GetNodeAndEdgeAtOffset(int offset, bool splitNode, out SplayTreeNode node, out ElementEdge edge)
		{
			Invariant.Assert(offset >= 1 && offset <= this.InternalSymbolCount - 1, "Bogus symbol offset!");
			bool flag = false;
			node = this._rootNode;
			int num = 0;
			for (;;)
			{
				Invariant.Assert(node.Generation != this._rootNode.Generation || node.SymbolOffsetCache == -1 || node.SymbolOffsetCache == num, "Bad node offset cache!");
				node.Generation = this._rootNode.Generation;
				node.SymbolOffsetCache = num;
				if (offset == num)
				{
					break;
				}
				if (node is TextTreeRootNode || node is TextTreeTextElementNode)
				{
					if (offset == num + 1)
					{
						goto Block_6;
					}
					if (offset == num + node.SymbolCount - 1)
					{
						goto Block_7;
					}
				}
				if (offset == num + node.SymbolCount)
				{
					goto Block_8;
				}
				if (node.ContainedNode == null)
				{
					goto Block_9;
				}
				node = node.ContainedNode;
				num++;
				int num2;
				node = node.GetSiblingAtOffset(offset - num, out num2);
				num += num2;
			}
			edge = ElementEdge.BeforeStart;
			flag = true;
			goto IL_126;
			Block_6:
			edge = ElementEdge.AfterStart;
			goto IL_126;
			Block_7:
			edge = ElementEdge.BeforeEnd;
			goto IL_126;
			Block_8:
			edge = ElementEdge.AfterEnd;
			flag = true;
			goto IL_126;
			Block_9:
			Invariant.Assert(node is TextTreeTextNode);
			if (splitNode)
			{
				node = ((TextTreeTextNode)node).Split(offset - num, ElementEdge.AfterEnd);
			}
			edge = ElementEdge.BeforeStart;
			IL_126:
			if (flag)
			{
				node = this.AdjustForZeroWidthNode(node, edge);
			}
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x0025ECA0 File Offset: 0x0025DCA0
		internal void GetNodeAndEdgeAtCharOffset(int charOffset, out TextTreeNode node, out ElementEdge edge)
		{
			Invariant.Assert(charOffset >= 0 && charOffset <= this.IMECharCount, "Bogus char offset!");
			if (this.IMECharCount == 0)
			{
				node = null;
				edge = ElementEdge.BeforeStart;
				return;
			}
			bool flag = false;
			node = this._rootNode;
			int num = 0;
			for (;;)
			{
				int num2 = 0;
				TextTreeTextElementNode textTreeTextElementNode = node as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					num2 = textTreeTextElementNode.IMELeftEdgeCharCount;
					if (num2 > 0)
					{
						if (charOffset == num)
						{
							break;
						}
						if (charOffset == num + num2)
						{
							goto Block_6;
						}
					}
				}
				else if (node is TextTreeTextNode || node is TextTreeObjectNode)
				{
					if (charOffset == num)
					{
						goto Block_8;
					}
					if (charOffset == num + node.IMECharCount)
					{
						goto Block_9;
					}
				}
				if (node.ContainedNode == null)
				{
					goto Block_10;
				}
				node = (TextTreeNode)node.ContainedNode;
				num += num2;
				int num3;
				node = (TextTreeNode)node.GetSiblingAtCharOffset(charOffset - num, out num3);
				num += num3;
			}
			edge = ElementEdge.BeforeStart;
			goto IL_FA;
			Block_6:
			edge = ElementEdge.AfterStart;
			goto IL_FA;
			Block_8:
			edge = ElementEdge.BeforeStart;
			flag = true;
			goto IL_FA;
			Block_9:
			edge = ElementEdge.AfterEnd;
			flag = true;
			goto IL_FA;
			Block_10:
			Invariant.Assert(node is TextTreeTextNode);
			node = ((TextTreeTextNode)node).Split(charOffset - num, ElementEdge.AfterEnd);
			edge = ElementEdge.BeforeStart;
			IL_FA:
			if (flag)
			{
				node = (TextTreeNode)this.AdjustForZeroWidthNode(node, edge);
			}
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void EmptyDeadPositionList()
		{
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x0025EDBC File Offset: 0x0025DDBC
		internal static int GetTextLength(object text)
		{
			Invariant.Assert(text is string || text is char[], "Bad text parameter!");
			string text2 = text as string;
			int result;
			if (text2 != null)
			{
				result = text2.Length;
			}
			else
			{
				result = ((char[])text).Length;
			}
			return result;
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void AssertTree()
		{
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x0025EE04 File Offset: 0x0025DE04
		internal int GetContentHashCode()
		{
			return this.InternalSymbolCount;
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x0025EE0C File Offset: 0x0025DE0C
		internal void NextLayoutGeneration()
		{
			TextTreeRootNode rootNode = this._rootNode;
			uint layoutGeneration = rootNode.LayoutGeneration;
			rootNode.LayoutGeneration = layoutGeneration + 1U;
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x0025EE30 File Offset: 0x0025DE30
		internal void ExtractElementInternal(TextElement element)
		{
			TextContainer.ExtractChangeEventArgs extractChangeEventArgs;
			this.ExtractElementInternal(element, false, out extractChangeEventArgs);
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x0025EE48 File Offset: 0x0025DE48
		internal bool IsAtCaretUnitBoundary(TextPointer position)
		{
			position.DebugAssertGeneration();
			Invariant.Assert(position.HasValidLayout);
			if (this._rootNode.CaretUnitBoundaryCacheOffset != position.GetSymbolOffset())
			{
				this._rootNode.CaretUnitBoundaryCacheOffset = position.GetSymbolOffset();
				this._rootNode.CaretUnitBoundaryCache = this._textview.IsAtCaretUnitBoundary(position);
				if (!this._rootNode.CaretUnitBoundaryCache && position.LogicalDirection == LogicalDirection.Backward)
				{
					TextPointer positionAtOffset = position.GetPositionAtOffset(0, LogicalDirection.Forward);
					this._rootNode.CaretUnitBoundaryCache = this._textview.IsAtCaretUnitBoundary(positionAtOffset);
				}
			}
			return this._rootNode.CaretUnitBoundaryCache;
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x060054D6 RID: 21718 RVA: 0x0025EEE1 File Offset: 0x0025DEE1
		internal TextPointer Start
		{
			get
			{
				this.EmptyDeadPositionList();
				this.DemandCreatePositionState();
				TextPointer textPointer = new TextPointer(this, this._rootNode, ElementEdge.AfterStart, LogicalDirection.Backward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x060054D7 RID: 21719 RVA: 0x0025EF03 File Offset: 0x0025DF03
		internal TextPointer End
		{
			get
			{
				this.EmptyDeadPositionList();
				this.DemandCreatePositionState();
				TextPointer textPointer = new TextPointer(this, this._rootNode, ElementEdge.BeforeEnd, LogicalDirection.Forward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x060054D8 RID: 21720 RVA: 0x0025EF25 File Offset: 0x0025DF25
		internal DependencyObject Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x060054D9 RID: 21721 RVA: 0x0025EF2D File Offset: 0x0025DF2D
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return this.CheckFlags(TextContainer.Flags.ReadOnly);
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x060054DA RID: 21722 RVA: 0x0025EF36 File Offset: 0x0025DF36
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this.Start;
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x060054DB RID: 21723 RVA: 0x0025EF3E File Offset: 0x0025DF3E
		ITextPointer ITextContainer.End
		{
			get
			{
				return this.End;
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x060054DC RID: 21724 RVA: 0x0025EF46 File Offset: 0x0025DF46
		uint ITextContainer.Generation
		{
			get
			{
				return this.Generation;
			}
		}

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x060054DD RID: 21725 RVA: 0x0025EF4E File Offset: 0x0025DF4E
		Highlights ITextContainer.Highlights
		{
			get
			{
				return this.Highlights;
			}
		}

		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x060054DE RID: 21726 RVA: 0x0025EF56 File Offset: 0x0025DF56
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this.Parent;
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x060054DF RID: 21727 RVA: 0x0025EF5E File Offset: 0x0025DF5E
		// (set) Token: 0x060054E0 RID: 21728 RVA: 0x0025EF66 File Offset: 0x0025DF66
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return this.TextSelection;
			}
			set
			{
				this._textSelection = value;
			}
		}

		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x060054E1 RID: 21729 RVA: 0x0025EF6F File Offset: 0x0025DF6F
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return this.UndoManager;
			}
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x060054E2 RID: 21730 RVA: 0x0025EF77 File Offset: 0x0025DF77
		// (set) Token: 0x060054E3 RID: 21731 RVA: 0x0025EF7F File Offset: 0x0025DF7F
		ITextView ITextContainer.TextView
		{
			get
			{
				return this.TextView;
			}
			set
			{
				this.TextView = value;
			}
		}

		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x060054E4 RID: 21732 RVA: 0x0025EF88 File Offset: 0x0025DF88
		// (set) Token: 0x060054E5 RID: 21733 RVA: 0x0025EF90 File Offset: 0x0025DF90
		internal ITextView TextView
		{
			get
			{
				return this._textview;
			}
			set
			{
				this._textview = value;
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x060054E6 RID: 21734 RVA: 0x0025EF99 File Offset: 0x0025DF99
		int ITextContainer.SymbolCount
		{
			get
			{
				return this.SymbolCount;
			}
		}

		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x060054E7 RID: 21735 RVA: 0x0025EFA1 File Offset: 0x0025DFA1
		internal int SymbolCount
		{
			get
			{
				return this.InternalSymbolCount - 2;
			}
		}

		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x060054E8 RID: 21736 RVA: 0x0025EFAB File Offset: 0x0025DFAB
		internal int InternalSymbolCount
		{
			get
			{
				if (this._rootNode != null)
				{
					return this._rootNode.SymbolCount;
				}
				return 2;
			}
		}

		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x0025EFC2 File Offset: 0x0025DFC2
		internal int IMECharCount
		{
			get
			{
				if (this._rootNode != null)
				{
					return this._rootNode.IMECharCount;
				}
				return 0;
			}
		}

		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x060054EA RID: 21738 RVA: 0x0025EFD9 File Offset: 0x0025DFD9
		int ITextContainer.IMECharCount
		{
			get
			{
				return this.IMECharCount;
			}
		}

		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x060054EB RID: 21739 RVA: 0x0025EFE1 File Offset: 0x0025DFE1
		internal TextTreeRootTextBlock RootTextBlock
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for TextBlocks before root node create!");
				return this._rootNode.RootTextBlock;
			}
		}

		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x060054EC RID: 21740 RVA: 0x0025F001 File Offset: 0x0025E001
		internal uint Generation
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for Generation before root node create!");
				return this._rootNode.Generation;
			}
		}

		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x060054ED RID: 21741 RVA: 0x0025F021 File Offset: 0x0025E021
		internal uint PositionGeneration
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for PositionGeneration before root node create!");
				return this._rootNode.PositionGeneration;
			}
		}

		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x060054EE RID: 21742 RVA: 0x0025F041 File Offset: 0x0025E041
		internal uint LayoutGeneration
		{
			get
			{
				Invariant.Assert(this._rootNode != null, "Asking for LayoutGeneration before root node create!");
				return this._rootNode.LayoutGeneration;
			}
		}

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x060054EF RID: 21743 RVA: 0x0025F061 File Offset: 0x0025E061
		internal Highlights Highlights
		{
			get
			{
				if (this._highlights == null)
				{
					this._highlights = new Highlights(this);
				}
				return this._highlights;
			}
		}

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x060054F0 RID: 21744 RVA: 0x0025F07D File Offset: 0x0025E07D
		internal TextTreeRootNode RootNode
		{
			get
			{
				return this._rootNode;
			}
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x060054F1 RID: 21745 RVA: 0x0025F085 File Offset: 0x0025E085
		internal TextTreeNode FirstContainedNode
		{
			get
			{
				if (this._rootNode != null)
				{
					return (TextTreeNode)this._rootNode.GetFirstContainedNode();
				}
				return null;
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x060054F2 RID: 21746 RVA: 0x0025F0A1 File Offset: 0x0025E0A1
		internal TextTreeNode LastContainedNode
		{
			get
			{
				if (this._rootNode != null)
				{
					return (TextTreeNode)this._rootNode.GetLastContainedNode();
				}
				return null;
			}
		}

		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x060054F3 RID: 21747 RVA: 0x0025F0BD File Offset: 0x0025E0BD
		internal UndoManager UndoManager
		{
			get
			{
				return this._undoManager;
			}
		}

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x060054F4 RID: 21748 RVA: 0x0025F0C5 File Offset: 0x0025E0C5
		internal ITextSelection TextSelection
		{
			get
			{
				return this._textSelection;
			}
		}

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x060054F5 RID: 21749 RVA: 0x0025F0CD File Offset: 0x0025E0CD
		internal bool HasListeners
		{
			get
			{
				return this.ChangingHandler != null || this.ChangeHandler != null || this.ChangedHandler != null;
			}
		}

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x060054F6 RID: 21750 RVA: 0x0025F0EA File Offset: 0x0025E0EA
		internal bool PlainTextOnly
		{
			get
			{
				return this.CheckFlags(TextContainer.Flags.PlainTextOnly);
			}
		}

		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x060054F7 RID: 21751 RVA: 0x0025F0F3 File Offset: 0x0025E0F3
		// (set) Token: 0x060054F8 RID: 21752 RVA: 0x0025F0FC File Offset: 0x0025E0FC
		internal bool CollectTextChanges
		{
			get
			{
				return this.CheckFlags(TextContainer.Flags.CollectTextChanges);
			}
			set
			{
				this.SetFlags(value, TextContainer.Flags.CollectTextChanges);
			}
		}

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x060054F9 RID: 21753 RVA: 0x0025F106 File Offset: 0x0025E106
		// (remove) Token: 0x060054FA RID: 21754 RVA: 0x0025F10F File Offset: 0x0025E10F
		event EventHandler ITextContainer.Changing
		{
			add
			{
				this.Changing += value;
			}
			remove
			{
				this.Changing -= value;
			}
		}

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x060054FB RID: 21755 RVA: 0x0025F118 File Offset: 0x0025E118
		// (remove) Token: 0x060054FC RID: 21756 RVA: 0x0025F121 File Offset: 0x0025E121
		event TextContainerChangeEventHandler ITextContainer.Change
		{
			add
			{
				this.Change += value;
			}
			remove
			{
				this.Change -= value;
			}
		}

		// Token: 0x140000CB RID: 203
		// (add) Token: 0x060054FD RID: 21757 RVA: 0x0025F12A File Offset: 0x0025E12A
		// (remove) Token: 0x060054FE RID: 21758 RVA: 0x0025F133 File Offset: 0x0025E133
		event TextContainerChangedEventHandler ITextContainer.Changed
		{
			add
			{
				this.Changed += value;
			}
			remove
			{
				this.Changed -= value;
			}
		}

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x060054FF RID: 21759 RVA: 0x0025F13C File Offset: 0x0025E13C
		// (remove) Token: 0x06005500 RID: 21760 RVA: 0x0025F155 File Offset: 0x0025E155
		internal event EventHandler Changing
		{
			add
			{
				this.ChangingHandler = (EventHandler)Delegate.Combine(this.ChangingHandler, value);
			}
			remove
			{
				this.ChangingHandler = (EventHandler)Delegate.Remove(this.ChangingHandler, value);
			}
		}

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06005501 RID: 21761 RVA: 0x0025F16E File Offset: 0x0025E16E
		// (remove) Token: 0x06005502 RID: 21762 RVA: 0x0025F187 File Offset: 0x0025E187
		internal event TextContainerChangeEventHandler Change
		{
			add
			{
				this.ChangeHandler = (TextContainerChangeEventHandler)Delegate.Combine(this.ChangeHandler, value);
			}
			remove
			{
				this.ChangeHandler = (TextContainerChangeEventHandler)Delegate.Remove(this.ChangeHandler, value);
			}
		}

		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06005503 RID: 21763 RVA: 0x0025F1A0 File Offset: 0x0025E1A0
		// (remove) Token: 0x06005504 RID: 21764 RVA: 0x0025F1B9 File Offset: 0x0025E1B9
		internal event TextContainerChangedEventHandler Changed
		{
			add
			{
				this.ChangedHandler = (TextContainerChangedEventHandler)Delegate.Combine(this.ChangedHandler, value);
			}
			remove
			{
				this.ChangedHandler = (TextContainerChangedEventHandler)Delegate.Remove(this.ChangedHandler, value);
			}
		}

		// Token: 0x06005505 RID: 21765 RVA: 0x0025F1D2 File Offset: 0x0025E1D2
		private void ReparentLogicalChildren(SplayTreeNode containerNode, DependencyObject newParentLogicalNode, DependencyObject oldParentLogicalNode)
		{
			this.ReparentLogicalChildren(containerNode.GetFirstContainedNode(), null, newParentLogicalNode, oldParentLogicalNode);
		}

		// Token: 0x06005506 RID: 21766 RVA: 0x0025F1E4 File Offset: 0x0025E1E4
		private void ReparentLogicalChildren(SplayTreeNode firstChildNode, SplayTreeNode lastChildNode, DependencyObject newParentLogicalNode, DependencyObject oldParentLogicalNode)
		{
			Invariant.Assert(newParentLogicalNode != null || oldParentLogicalNode != null, "Both new and old parents should not be null");
			for (SplayTreeNode splayTreeNode = firstChildNode; splayTreeNode != null; splayTreeNode = splayTreeNode.GetNextNode())
			{
				DependencyObject dependencyObject = null;
				TextTreeTextElementNode textTreeTextElementNode = splayTreeNode as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					dependencyObject = textTreeTextElementNode.TextElement;
				}
				else
				{
					TextTreeObjectNode textTreeObjectNode = splayTreeNode as TextTreeObjectNode;
					if (textTreeObjectNode != null)
					{
						dependencyObject = textTreeObjectNode.EmbeddedElement;
					}
				}
				TextElement textElement = dependencyObject as TextElement;
				if (textElement != null)
				{
					textElement.BeforeLogicalTreeChange();
				}
				try
				{
					if (oldParentLogicalNode != null)
					{
						LogicalTreeHelper.RemoveLogicalChild(oldParentLogicalNode, dependencyObject);
					}
					if (newParentLogicalNode != null)
					{
						LogicalTreeHelper.AddLogicalChild(newParentLogicalNode, dependencyObject);
					}
				}
				finally
				{
					if (textElement != null)
					{
						textElement.AfterLogicalTreeChange();
					}
				}
				if (splayTreeNode == lastChildNode)
				{
					break;
				}
			}
		}

		// Token: 0x06005507 RID: 21767 RVA: 0x0025F288 File Offset: 0x0025E288
		private SplayTreeNode AdjustForZeroWidthNode(SplayTreeNode node, ElementEdge edge)
		{
			TextTreeTextNode textTreeTextNode = node as TextTreeTextNode;
			if (textTreeTextNode == null)
			{
				Invariant.Assert(node.SymbolCount > 0, "Only TextTreeTextNodes may have zero symbol counts!");
				return node;
			}
			if (textTreeTextNode.SymbolCount == 0)
			{
				SplayTreeNode nextNode = textTreeTextNode.GetNextNode();
				if (nextNode != null)
				{
					if (Invariant.Strict && nextNode.SymbolCount == 0)
					{
						Invariant.Assert(nextNode is TextTreeTextNode);
						Invariant.Assert(!textTreeTextNode.BeforeStartReferenceCount);
						Invariant.Assert(!((TextTreeTextNode)nextNode).AfterEndReferenceCount);
						Invariant.Assert(textTreeTextNode.GetPreviousNode() == null || textTreeTextNode.GetPreviousNode().SymbolCount > 0, "Found three consecutive zero-width text nodes! (1)");
						Invariant.Assert(nextNode.GetNextNode() == null || nextNode.GetNextNode().SymbolCount > 0, "Found three consecutive zero-width text nodes! (2)");
					}
					if (!textTreeTextNode.BeforeStartReferenceCount)
					{
						node = nextNode;
					}
				}
			}
			else if (edge == ElementEdge.BeforeStart)
			{
				if (textTreeTextNode.AfterEndReferenceCount)
				{
					SplayTreeNode previousNode = textTreeTextNode.GetPreviousNode();
					if (previousNode != null && previousNode.SymbolCount == 0 && !((TextTreeNode)previousNode).AfterEndReferenceCount)
					{
						Invariant.Assert(previousNode is TextTreeTextNode);
						node = previousNode;
					}
				}
			}
			else if (textTreeTextNode.BeforeStartReferenceCount)
			{
				SplayTreeNode nextNode = textTreeTextNode.GetNextNode();
				if (nextNode != null && nextNode.SymbolCount == 0 && !((TextTreeNode)nextNode).BeforeStartReferenceCount)
				{
					Invariant.Assert(nextNode is TextTreeTextNode);
					node = nextNode;
				}
			}
			return node;
		}

		// Token: 0x06005508 RID: 21768 RVA: 0x0025F3D4 File Offset: 0x0025E3D4
		private int InsertElementToSiblingTree(TextPointer startPosition, TextPointer endPosition, TextTreeTextElementNode elementNode)
		{
			int num = 0;
			int num2 = 0;
			if (startPosition.CompareTo(endPosition) == 0)
			{
				int num3 = elementNode.IMECharCount - elementNode.IMELeftEdgeCharCount;
				elementNode.InsertAtPosition(startPosition);
				if (elementNode.ContainedNode != null)
				{
					num = elementNode.SymbolCount - 2;
					num2 = num3;
				}
			}
			else
			{
				num = this.InsertElementToSiblingTreeComplex(startPosition, endPosition, elementNode, out num2);
			}
			elementNode.SymbolCount = num + 2;
			elementNode.IMECharCount = num2 + elementNode.IMELeftEdgeCharCount;
			return num;
		}

		// Token: 0x06005509 RID: 21769 RVA: 0x0025F43C File Offset: 0x0025E43C
		private int InsertElementToSiblingTreeComplex(TextPointer startPosition, TextPointer endPosition, TextTreeTextElementNode elementNode, out int childCharCount)
		{
			SplayTreeNode scopingNode = startPosition.GetScopingNode();
			SplayTreeNode leftSubTree;
			SplayTreeNode splayTreeNode;
			SplayTreeNode rightSubTree;
			int result = this.CutContent(startPosition, endPosition, out childCharCount, out leftSubTree, out splayTreeNode, out rightSubTree);
			SplayTreeNode.Join(elementNode, leftSubTree, rightSubTree);
			elementNode.ContainedNode = splayTreeNode;
			splayTreeNode.ParentNode = elementNode;
			scopingNode.ContainedNode = elementNode;
			elementNode.ParentNode = scopingNode;
			return result;
		}

		// Token: 0x0600550A RID: 21770 RVA: 0x0025F484 File Offset: 0x0025E484
		private int DeleteContentFromSiblingTree(SplayTreeNode containingNode, TextPointer startPosition, TextPointer endPosition, bool newFirstIMEVisibleNode, out int charCount)
		{
			if (startPosition.CompareTo(endPosition) == 0)
			{
				if (newFirstIMEVisibleNode)
				{
					this.UpdateContainerSymbolCount(containingNode, 0, -1);
				}
				charCount = 0;
				return 0;
			}
			int symbolOffset = startPosition.GetSymbolOffset();
			SplayTreeNode splayTreeNode;
			SplayTreeNode splayTreeNode2;
			SplayTreeNode splayTreeNode3;
			int num = this.CutContent(startPosition, endPosition, out charCount, out splayTreeNode, out splayTreeNode2, out splayTreeNode3);
			if (splayTreeNode2 != null)
			{
				TextTreeNode previousNode;
				ElementEdge previousEdge;
				if (splayTreeNode != null)
				{
					previousNode = (TextTreeNode)splayTreeNode.GetMaxSibling();
					previousEdge = ElementEdge.AfterEnd;
				}
				else
				{
					previousNode = (TextTreeNode)containingNode;
					previousEdge = ElementEdge.AfterStart;
				}
				TextTreeNode nextNode;
				ElementEdge nextEdge;
				if (splayTreeNode3 != null)
				{
					nextNode = (TextTreeNode)splayTreeNode3.GetMinSibling();
					nextEdge = ElementEdge.BeforeStart;
				}
				else
				{
					nextNode = (TextTreeNode)containingNode;
					nextEdge = ElementEdge.BeforeEnd;
				}
				this.AdjustRefCountsForContentDelete(ref previousNode, previousEdge, ref nextNode, nextEdge, (TextTreeNode)splayTreeNode2);
				if (splayTreeNode != null)
				{
					splayTreeNode.Splay();
				}
				if (splayTreeNode3 != null)
				{
					splayTreeNode3.Splay();
				}
				splayTreeNode2.Splay();
				Invariant.Assert(splayTreeNode2.ParentNode == null, "Assigning fixup node to parented child!");
				splayTreeNode2.ParentNode = new TextTreeFixupNode(previousNode, previousEdge, nextNode, nextEdge);
			}
			SplayTreeNode splayTreeNode4 = SplayTreeNode.Join(splayTreeNode, splayTreeNode3);
			containingNode.ContainedNode = splayTreeNode4;
			if (splayTreeNode4 != null)
			{
				splayTreeNode4.ParentNode = containingNode;
			}
			if (num > 0)
			{
				int num2 = 0;
				if (newFirstIMEVisibleNode)
				{
					num2 = -1;
				}
				this.UpdateContainerSymbolCount(containingNode, -num, -charCount + num2);
				TextTreeText.RemoveText(this._rootNode.RootTextBlock, symbolOffset, num);
				this.NextGeneration(true);
				Invariant.Assert(startPosition.Parent == endPosition.Parent);
				TextElement textElement = startPosition.Parent as TextElement;
				if (textElement != null)
				{
					textElement.OnTextUpdated();
				}
			}
			return num;
		}

		// Token: 0x0600550B RID: 21771 RVA: 0x0025F5E0 File Offset: 0x0025E5E0
		private int CutTopLevelLogicalNodes(TextTreeNode containingNode, TextPointer startPosition, TextPointer endPosition, out int charCount)
		{
			Invariant.Assert(startPosition.GetScopingNode() == endPosition.GetScopingNode(), "startPosition/endPosition not in same sibling tree!");
			SplayTreeNode splayTreeNode = startPosition.GetAdjacentSiblingNode(LogicalDirection.Forward);
			SplayTreeNode adjacentSiblingNode = endPosition.GetAdjacentSiblingNode(LogicalDirection.Forward);
			int num = 0;
			charCount = 0;
			DependencyObject logicalTreeNode = containingNode.GetLogicalTreeNode();
			while (splayTreeNode != adjacentSiblingNode)
			{
				object child = null;
				SplayTreeNode nextNode = splayTreeNode.GetNextNode();
				TextTreeTextElementNode textTreeTextElementNode = splayTreeNode as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					int imecharCount = textTreeTextElementNode.IMECharCount;
					char[] array = TextTreeText.CutText(this._rootNode.RootTextBlock, textTreeTextElementNode.GetSymbolOffset(this.Generation), textTreeTextElementNode.SymbolCount);
					this.ExtractElementFromSiblingTree(containingNode, textTreeTextElementNode, true);
					Invariant.Assert(textTreeTextElementNode.TextElement.TextElementNode != textTreeTextElementNode);
					textTreeTextElementNode = textTreeTextElementNode.TextElement.TextElementNode;
					this.UpdateContainerSymbolCount(containingNode, -textTreeTextElementNode.SymbolCount, -imecharCount);
					this.NextGeneration(true);
					TextContainer textContainer = new TextContainer(null, false);
					TextPointer start = textContainer.Start;
					textContainer.InsertElementToSiblingTree(start, start, textTreeTextElementNode);
					Invariant.Assert(array.Length == textTreeTextElementNode.SymbolCount);
					textContainer.UpdateContainerSymbolCount(textTreeTextElementNode.GetContainingNode(), textTreeTextElementNode.SymbolCount, textTreeTextElementNode.IMECharCount);
					textContainer.DemandCreateText();
					TextTreeText.InsertText(textContainer.RootTextBlock, 1, array);
					textContainer.NextGeneration(false);
					child = textTreeTextElementNode.TextElement;
					num += textTreeTextElementNode.SymbolCount;
					charCount += imecharCount;
				}
				else
				{
					TextTreeObjectNode textTreeObjectNode = splayTreeNode as TextTreeObjectNode;
					if (textTreeObjectNode != null)
					{
						child = textTreeObjectNode.EmbeddedElement;
					}
				}
				LogicalTreeHelper.RemoveLogicalChild(logicalTreeNode, child);
				splayTreeNode = nextNode;
			}
			if (num > 0)
			{
				startPosition.SyncToTreeGeneration();
				endPosition.SyncToTreeGeneration();
			}
			return num;
		}

		// Token: 0x0600550C RID: 21772 RVA: 0x0025F768 File Offset: 0x0025E768
		private void AdjustRefCountsForContentDelete(ref TextTreeNode previousNode, ElementEdge previousEdge, ref TextTreeNode nextNode, ElementEdge nextEdge, TextTreeNode middleSubTree)
		{
			bool delta = false;
			bool delta2 = false;
			this.GetReferenceCounts((TextTreeNode)middleSubTree.GetMinSibling(), ref delta, ref delta2);
			previousNode = previousNode.IncrementReferenceCount(previousEdge, delta2);
			nextNode = nextNode.IncrementReferenceCount(nextEdge, delta);
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x0025F7A8 File Offset: 0x0025E7A8
		private void GetReferenceCounts(TextTreeNode node, ref bool leftEdgeReferenceCount, ref bool rightEdgeReferenceCount)
		{
			do
			{
				leftEdgeReferenceCount |= (node.BeforeStartReferenceCount || node.BeforeEndReferenceCount);
				rightEdgeReferenceCount |= (node.AfterStartReferenceCount || node.AfterEndReferenceCount);
				if (node.ContainedNode != null)
				{
					this.GetReferenceCounts((TextTreeNode)node.ContainedNode.GetMinSibling(), ref leftEdgeReferenceCount, ref rightEdgeReferenceCount);
				}
				node = (TextTreeNode)node.GetNextNode();
			}
			while (node != null);
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x0025F814 File Offset: 0x0025E814
		private void AdjustRefCountsForShallowDelete(ref TextTreeNode previousNode, ElementEdge previousEdge, ref TextTreeNode nextNode, ElementEdge nextEdge, ref TextTreeNode firstContainedNode, ref TextTreeNode lastContainedNode, TextTreeTextElementNode extractedElementNode)
		{
			previousNode = previousNode.IncrementReferenceCount(previousEdge, extractedElementNode.AfterStartReferenceCount);
			nextNode = nextNode.IncrementReferenceCount(nextEdge, extractedElementNode.BeforeEndReferenceCount);
			if (firstContainedNode != null)
			{
				firstContainedNode = firstContainedNode.IncrementReferenceCount(ElementEdge.BeforeStart, extractedElementNode.BeforeStartReferenceCount);
			}
			else
			{
				nextNode = nextNode.IncrementReferenceCount(nextEdge, extractedElementNode.BeforeStartReferenceCount);
			}
			if (lastContainedNode != null)
			{
				lastContainedNode = lastContainedNode.IncrementReferenceCount(ElementEdge.AfterEnd, extractedElementNode.AfterEndReferenceCount);
				return;
			}
			previousNode = previousNode.IncrementReferenceCount(previousEdge, extractedElementNode.AfterEndReferenceCount);
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x0025F89C File Offset: 0x0025E89C
		private int CutContent(TextPointer startPosition, TextPointer endPosition, out int charCount, out SplayTreeNode leftSubTree, out SplayTreeNode middleSubTree, out SplayTreeNode rightSubTree)
		{
			Invariant.Assert(startPosition.GetScopingNode() == endPosition.GetScopingNode(), "startPosition/endPosition not in same sibling tree!");
			Invariant.Assert(startPosition.CompareTo(endPosition) != 0, "CutContent doesn't expect empty span!");
			ElementEdge edge = startPosition.Edge;
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				leftSubTree = startPosition.Node.GetPreviousNode();
				goto IL_81;
			case ElementEdge.AfterStart:
				leftSubTree = null;
				goto IL_81;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
			case ElementEdge.BeforeEnd:
				break;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					leftSubTree = startPosition.Node;
					goto IL_81;
				}
				break;
			}
			Invariant.Assert(false, "Unexpected edge!");
			leftSubTree = null;
			IL_81:
			edge = endPosition.Edge;
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				rightSubTree = endPosition.Node;
				goto IL_D6;
			case ElementEdge.AfterStart:
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				rightSubTree = null;
				goto IL_D6;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					rightSubTree = endPosition.Node.GetNextNode();
					goto IL_D6;
				}
				break;
			}
			Invariant.Assert(false, "Unexpected edge! (2)");
			rightSubTree = null;
			IL_D6:
			if (rightSubTree == null)
			{
				if (leftSubTree == null)
				{
					middleSubTree = startPosition.GetScopingNode().ContainedNode;
				}
				else
				{
					middleSubTree = leftSubTree.GetNextNode();
				}
			}
			else
			{
				middleSubTree = rightSubTree.GetPreviousNode();
				if (middleSubTree == leftSubTree)
				{
					middleSubTree = null;
				}
			}
			if (leftSubTree != null)
			{
				leftSubTree.Split();
				Invariant.Assert(leftSubTree.Role == SplayTreeNodeRole.LocalRoot);
				leftSubTree.ParentNode.ContainedNode = null;
				leftSubTree.ParentNode = null;
			}
			int num = 0;
			charCount = 0;
			if (middleSubTree != null)
			{
				if (rightSubTree != null)
				{
					middleSubTree.Split();
				}
				else
				{
					middleSubTree.Splay();
				}
				Invariant.Assert(middleSubTree.Role == SplayTreeNodeRole.LocalRoot, "middleSubTree is not a local root!");
				if (middleSubTree.ParentNode != null)
				{
					middleSubTree.ParentNode.ContainedNode = null;
					middleSubTree.ParentNode = null;
				}
				for (SplayTreeNode splayTreeNode = middleSubTree; splayTreeNode != null; splayTreeNode = splayTreeNode.RightChildNode)
				{
					num += splayTreeNode.LeftSymbolCount + splayTreeNode.SymbolCount;
					charCount += splayTreeNode.LeftCharCount + splayTreeNode.IMECharCount;
				}
			}
			if (rightSubTree != null)
			{
				rightSubTree.Splay();
			}
			Invariant.Assert(leftSubTree == null || leftSubTree.Role == SplayTreeNodeRole.LocalRoot);
			Invariant.Assert(middleSubTree == null || middleSubTree.Role == SplayTreeNodeRole.LocalRoot);
			Invariant.Assert(rightSubTree == null || rightSubTree.Role == SplayTreeNodeRole.LocalRoot);
			return num;
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x0025FAD8 File Offset: 0x0025EAD8
		private char[] ExtractElementInternal(TextElement element, bool deep, out TextContainer.ExtractChangeEventArgs extractChangeEventArgs)
		{
			this.BeforeAddChange();
			SplayTreeNode splayTreeNode = null;
			SplayTreeNode lastChildNode = null;
			extractChangeEventArgs = null;
			char[] result = null;
			TextTreeTextElementNode textElementNode = element.TextElementNode;
			SplayTreeNode containingNode = textElementNode.GetContainingNode();
			bool flag = textElementNode.ContainedNode == null;
			TextPointer textPointer = new TextPointer(this, textElementNode, ElementEdge.BeforeStart, LogicalDirection.Backward);
			TextPointer textPointer2 = null;
			if (!flag)
			{
				textPointer2 = new TextPointer(this, textElementNode, ElementEdge.AfterEnd, LogicalDirection.Backward);
			}
			int symbolOffset = textElementNode.GetSymbolOffset(this.Generation);
			DependencyObject logicalTreeNode = ((TextTreeNode)containingNode).GetLogicalTreeNode();
			TextElementCollectionHelper.MarkDirty(logicalTreeNode);
			element.BeforeLogicalTreeChange();
			try
			{
				LogicalTreeHelper.RemoveLogicalChild(logicalTreeNode, element);
			}
			finally
			{
				element.AfterLogicalTreeChange();
			}
			TextTreeUndoUnit textTreeUndoUnit;
			if (deep && !flag)
			{
				textTreeUndoUnit = TextTreeUndo.CreateDeleteContentUndoUnit(this, textPointer, textPointer2);
			}
			else
			{
				textTreeUndoUnit = TextTreeUndo.CreateExtractElementUndoUnit(this, textElementNode);
			}
			if (!deep && !flag)
			{
				splayTreeNode = textElementNode.GetFirstContainedNode();
				lastChildNode = textElementNode.GetLastContainedNode();
			}
			int imecharCount = textElementNode.IMECharCount;
			int imeleftEdgeCharCount = textElementNode.IMELeftEdgeCharCount;
			int num = 0;
			TextTreeTextElementNode textTreeTextElementNode = null;
			if ((deep || flag) && element.IsFirstIMEVisibleSibling)
			{
				textTreeTextElementNode = (TextTreeTextElementNode)textElementNode.GetNextNode();
				if (textTreeTextElementNode != null)
				{
					num = -textTreeTextElementNode.IMELeftEdgeCharCount;
					textTreeTextElementNode.IMECharCount += num;
				}
			}
			this.ExtractElementFromSiblingTree(containingNode, textElementNode, deep);
			int num2 = 0;
			TextTreeTextElementNode textTreeTextElementNode2 = splayTreeNode as TextTreeTextElementNode;
			if (textTreeTextElementNode2 != null)
			{
				num2 = textTreeTextElementNode2.IMELeftEdgeCharCount;
				textTreeTextElementNode2.IMECharCount += num2;
			}
			if (!deep)
			{
				element.TextElementNode = null;
				TextTreeText.RemoveElementEdges(this._rootNode.RootTextBlock, symbolOffset, textElementNode.SymbolCount);
			}
			else
			{
				result = TextTreeText.CutText(this._rootNode.RootTextBlock, symbolOffset, textElementNode.SymbolCount);
			}
			if (deep)
			{
				this.UpdateContainerSymbolCount(containingNode, -textElementNode.SymbolCount, -imecharCount + num + num2);
			}
			else
			{
				this.UpdateContainerSymbolCount(containingNode, -2, -imeleftEdgeCharCount + num + num2);
			}
			this.NextGeneration(true);
			if (textTreeUndoUnit != null)
			{
				textTreeUndoUnit.SetTreeHashCode();
			}
			if (deep)
			{
				extractChangeEventArgs = new TextContainer.ExtractChangeEventArgs(this, textPointer, textElementNode, (num == 0) ? null : textTreeTextElementNode, (num2 == 0) ? null : textTreeTextElementNode2, imecharCount, imecharCount - imeleftEdgeCharCount);
			}
			else if (flag)
			{
				this.AddChange(textPointer, 2, imecharCount, PrecursorTextChangeType.ContentRemoved);
			}
			else
			{
				this.AddChange(textPointer, textPointer2, textElementNode.SymbolCount, imeleftEdgeCharCount, imecharCount - imeleftEdgeCharCount, PrecursorTextChangeType.ElementExtracted, null, false);
			}
			if (extractChangeEventArgs == null)
			{
				if (num != 0)
				{
					this.RaiseEventForNewFirstIMEVisibleNode(textTreeTextElementNode);
				}
				if (num2 != 0)
				{
					this.RaiseEventForFormerFirstIMEVisibleNode(textTreeTextElementNode2);
				}
			}
			if (!deep && !flag)
			{
				this.ReparentLogicalChildren(splayTreeNode, lastChildNode, logicalTreeNode, element);
			}
			if (element.TextElementNode != null)
			{
				element.TextElementNode.IMECharCount -= imeleftEdgeCharCount;
			}
			return result;
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x0025FD44 File Offset: 0x0025ED44
		private void ExtractElementFromSiblingTree(SplayTreeNode containingNode, TextTreeTextElementNode elementNode, bool deep)
		{
			TextTreeNode textTreeNode = (TextTreeNode)elementNode.GetPreviousNode();
			ElementEdge previousEdge = ElementEdge.AfterEnd;
			if (textTreeNode == null)
			{
				textTreeNode = (TextTreeNode)containingNode;
				previousEdge = ElementEdge.AfterStart;
			}
			TextTreeNode textTreeNode2 = (TextTreeNode)elementNode.GetNextNode();
			ElementEdge nextEdge = ElementEdge.BeforeStart;
			if (textTreeNode2 == null)
			{
				textTreeNode2 = (TextTreeNode)containingNode;
				nextEdge = ElementEdge.BeforeEnd;
			}
			elementNode.Remove();
			Invariant.Assert(elementNode.Role == SplayTreeNodeRole.LocalRoot);
			if (deep)
			{
				this.AdjustRefCountsForContentDelete(ref textTreeNode, previousEdge, ref textTreeNode2, nextEdge, elementNode);
				elementNode.ParentNode = new TextTreeFixupNode(textTreeNode, previousEdge, textTreeNode2, nextEdge);
				this.DeepCopy(elementNode);
				return;
			}
			SplayTreeNode containedNode = elementNode.ContainedNode;
			elementNode.ContainedNode = null;
			TextTreeNode firstContainedNode;
			TextTreeNode lastContainedNode;
			if (containedNode != null)
			{
				containedNode.ParentNode = null;
				firstContainedNode = (TextTreeNode)containedNode.GetMinSibling();
				lastContainedNode = (TextTreeNode)containedNode.GetMaxSibling();
			}
			else
			{
				firstContainedNode = null;
				lastContainedNode = null;
			}
			this.AdjustRefCountsForShallowDelete(ref textTreeNode, previousEdge, ref textTreeNode2, nextEdge, ref firstContainedNode, ref lastContainedNode, elementNode);
			elementNode.ParentNode = new TextTreeFixupNode(textTreeNode, previousEdge, textTreeNode2, nextEdge, firstContainedNode, lastContainedNode);
			if (containedNode != null)
			{
				containedNode.Splay();
				SplayTreeNode splayTreeNode = containedNode;
				if (textTreeNode != containingNode)
				{
					textTreeNode.Split();
					Invariant.Assert(textTreeNode.Role == SplayTreeNodeRole.LocalRoot);
					Invariant.Assert(textTreeNode.RightChildNode == null);
					SplayTreeNode minSibling = containedNode.GetMinSibling();
					minSibling.Splay();
					textTreeNode.RightChildNode = minSibling;
					minSibling.ParentNode = textTreeNode;
					splayTreeNode = textTreeNode;
				}
				if (textTreeNode2 != containingNode)
				{
					textTreeNode2.Splay();
					Invariant.Assert(textTreeNode2.Role == SplayTreeNodeRole.LocalRoot);
					Invariant.Assert(textTreeNode2.LeftChildNode == null);
					SplayTreeNode maxSibling = containedNode.GetMaxSibling();
					maxSibling.Splay();
					textTreeNode2.LeftChildNode = maxSibling;
					textTreeNode2.LeftSymbolCount += maxSibling.LeftSymbolCount + maxSibling.SymbolCount;
					textTreeNode2.LeftCharCount += maxSibling.LeftCharCount + maxSibling.IMECharCount;
					maxSibling.ParentNode = textTreeNode2;
					splayTreeNode = textTreeNode2;
				}
				containingNode.ContainedNode = splayTreeNode;
				if (splayTreeNode != null)
				{
					splayTreeNode.ParentNode = containingNode;
				}
			}
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x0025FF18 File Offset: 0x0025EF18
		private TextTreeTextElementNode DeepCopy(TextTreeTextElementNode elementNode)
		{
			TextTreeTextElementNode textTreeTextElementNode = (TextTreeTextElementNode)elementNode.Clone();
			elementNode.TextElement.TextElementNode = textTreeTextElementNode;
			if (elementNode.ContainedNode != null)
			{
				textTreeTextElementNode.ContainedNode = this.DeepCopyContainedNodes((TextTreeNode)elementNode.ContainedNode.GetMinSibling());
				textTreeTextElementNode.ContainedNode.ParentNode = textTreeTextElementNode;
			}
			return textTreeTextElementNode;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x0025FF70 File Offset: 0x0025EF70
		private TextTreeNode DeepCopyContainedNodes(TextTreeNode node)
		{
			TextTreeNode result = null;
			TextTreeNode textTreeNode = null;
			do
			{
				TextTreeTextElementNode textTreeTextElementNode = node as TextTreeTextElementNode;
				TextTreeNode textTreeNode2;
				if (textTreeTextElementNode != null)
				{
					textTreeNode2 = this.DeepCopy(textTreeTextElementNode);
				}
				else
				{
					textTreeNode2 = node.Clone();
				}
				Invariant.Assert(textTreeNode2 != null || (node is TextTreeTextNode && node.SymbolCount == 0));
				if (textTreeNode2 != null)
				{
					textTreeNode2.ParentNode = textTreeNode;
					if (textTreeNode != null)
					{
						textTreeNode.RightChildNode = textTreeNode2;
					}
					else
					{
						Invariant.Assert(textTreeNode2.Role == SplayTreeNodeRole.LocalRoot);
						result = textTreeNode2;
					}
					textTreeNode = textTreeNode2;
				}
				node = (TextTreeNode)node.GetNextNode();
			}
			while (node != null);
			return result;
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x0025FFF4 File Offset: 0x0025EFF4
		private void DemandCreatePositionState()
		{
			if (this._rootNode == null)
			{
				this._rootNode = new TextTreeRootNode(this);
			}
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x0026000C File Offset: 0x0025F00C
		private void DemandCreateText()
		{
			Invariant.Assert(this._rootNode != null, "Unexpected DemandCreateText call before position allocation.");
			if (this._rootNode.RootTextBlock == null)
			{
				this._rootNode.RootTextBlock = new TextTreeRootTextBlock();
				TextTreeText.InsertElementEdges(this._rootNode.RootTextBlock, 0, 0);
			}
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x0026005B File Offset: 0x0025F05B
		private void UpdateContainerSymbolCount(SplayTreeNode containingNode, int symbolCount, int charCount)
		{
			do
			{
				containingNode.Splay();
				containingNode.SymbolCount += symbolCount;
				containingNode.IMECharCount += charCount;
				containingNode = containingNode.ParentNode;
			}
			while (containingNode != null);
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x0026008C File Offset: 0x0025F08C
		private void NextGeneration(bool deletedContent)
		{
			this.AssertTree();
			this.AssertTreeAndTextSize();
			TextTreeRootNode rootNode = this._rootNode;
			uint num = rootNode.Generation;
			rootNode.Generation = num + 1U;
			if (deletedContent)
			{
				TextTreeRootNode rootNode2 = this._rootNode;
				num = rootNode2.PositionGeneration;
				rootNode2.PositionGeneration = num + 1U;
			}
			this.NextLayoutGeneration();
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x002600D8 File Offset: 0x0025F0D8
		private DependencyProperty[] LocalValueEnumeratorToArray(LocalValueEnumerator valuesEnumerator)
		{
			DependencyProperty[] array = new DependencyProperty[valuesEnumerator.Count];
			int num = 0;
			valuesEnumerator.Reset();
			while (valuesEnumerator.MoveNext())
			{
				DependencyProperty[] array2 = array;
				int num2 = num++;
				LocalValueEntry localValueEntry = valuesEnumerator.Current;
				array2[num2] = localValueEntry.Property;
			}
			return array;
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x00260120 File Offset: 0x0025F120
		private void ValidateSetValue(TextPointer position)
		{
			if (position.TextContainer != this)
			{
				throw new InvalidOperationException(SR.Get("NotInThisTree", new object[]
				{
					"position"
				}));
			}
			position.SyncToTreeGeneration();
			if (!(position.Parent is TextElement))
			{
				throw new InvalidOperationException(SR.Get("NoElement"));
			}
		}

		// Token: 0x0600551A RID: 21786 RVA: 0x00260178 File Offset: 0x0025F178
		private void AssertTreeAndTextSize()
		{
			if (Invariant.Strict && this._rootNode.RootTextBlock != null)
			{
				int num = 0;
				for (TextTreeTextBlock textTreeTextBlock = (TextTreeTextBlock)this._rootNode.RootTextBlock.ContainedNode.GetMinSibling(); textTreeTextBlock != null; textTreeTextBlock = (TextTreeTextBlock)textTreeTextBlock.GetNextNode())
				{
					Invariant.Assert(textTreeTextBlock.Count > 0, "Empty TextBlock!");
					num += textTreeTextBlock.Count;
				}
				Invariant.Assert(num == this.InternalSymbolCount, "TextContainer.SymbolCount does not match TextTreeText size!");
			}
		}

		// Token: 0x0600551B RID: 21787 RVA: 0x002601F8 File Offset: 0x0025F1F8
		private void BeginChange(bool undo)
		{
			if (undo && this._changeBlockUndoRecord == null && this._changeBlockLevel == 0)
			{
				Invariant.Assert(this._changeBlockLevel == 0);
				this._changeBlockUndoRecord = new ChangeBlockUndoRecord(this, string.Empty);
			}
			if (this._changeBlockLevel == 0)
			{
				this.DemandCreatePositionState();
				if (this.Dispatcher != null)
				{
					this._rootNode.DispatcherProcessingDisabled = this.Dispatcher.DisableProcessing();
				}
			}
			this._changeBlockLevel++;
		}

		// Token: 0x0600551C RID: 21788 RVA: 0x00260274 File Offset: 0x0025F274
		private void FireChangeEvent(TextPointer startPosition, TextPointer endPosition, int symbolCount, int leftEdgeCharCount, int childCharCount, PrecursorTextChangeType precursorTextChange, DependencyProperty property, bool affectsRenderOnly)
		{
			Invariant.Assert(this.ChangeHandler != null);
			this.SetFlags(true, TextContainer.Flags.ReadOnly);
			try
			{
				if (precursorTextChange == PrecursorTextChangeType.ElementAdded)
				{
					Invariant.Assert(symbolCount > 2, "ElementAdded must span at least two element edges and one content symbol!");
					TextContainerChangeEventArgs e = new TextContainerChangeEventArgs(startPosition, 1, leftEdgeCharCount, TextChangeType.ContentAdded);
					TextContainerChangeEventArgs e2 = new TextContainerChangeEventArgs(endPosition, 1, 0, TextChangeType.ContentAdded);
					this.ChangeHandler(this, e);
					this.ChangeHandler(this, e2);
				}
				else if (precursorTextChange == PrecursorTextChangeType.ElementExtracted)
				{
					Invariant.Assert(symbolCount > 2, "ElementExtracted must span at least two element edges and one content symbol!");
					TextContainerChangeEventArgs e3 = new TextContainerChangeEventArgs(startPosition, 1, leftEdgeCharCount, TextChangeType.ContentRemoved);
					TextContainerChangeEventArgs e4 = new TextContainerChangeEventArgs(endPosition, 1, 0, TextChangeType.ContentRemoved);
					this.ChangeHandler(this, e3);
					this.ChangeHandler(this, e4);
				}
				else
				{
					TextContainerChangeEventArgs e5 = new TextContainerChangeEventArgs(startPosition, symbolCount, leftEdgeCharCount + childCharCount, this.ConvertSimplePrecursorChangeToTextChange(precursorTextChange), property, affectsRenderOnly);
					this.ChangeHandler(this, e5);
				}
			}
			finally
			{
				this.SetFlags(false, TextContainer.Flags.ReadOnly);
			}
		}

		// Token: 0x0600551D RID: 21789 RVA: 0x00260364 File Offset: 0x0025F364
		private TextChangeType ConvertSimplePrecursorChangeToTextChange(PrecursorTextChangeType precursorTextChange)
		{
			Invariant.Assert(precursorTextChange != PrecursorTextChangeType.ElementAdded && precursorTextChange != PrecursorTextChangeType.ElementExtracted);
			return (TextChangeType)precursorTextChange;
		}

		// Token: 0x0600551E RID: 21790 RVA: 0x0026037C File Offset: 0x0025F37C
		private TextTreeTextElementNode GetNextIMEVisibleNode(TextPointer startPosition, TextPointer endPosition)
		{
			TextTreeTextElementNode result = null;
			TextElement textElement = startPosition.GetAdjacentElement(LogicalDirection.Forward) as TextElement;
			if (textElement != null && textElement.IsFirstIMEVisibleSibling)
			{
				result = (TextTreeTextElementNode)endPosition.GetAdjacentSiblingNode(LogicalDirection.Forward);
			}
			return result;
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x002603B4 File Offset: 0x0025F3B4
		private void RaiseEventForFormerFirstIMEVisibleNode(TextTreeNode node)
		{
			TextPointer startPosition = new TextPointer(this, node, ElementEdge.BeforeStart);
			this.AddChange(startPosition, 0, 1, PrecursorTextChangeType.ContentAdded);
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x002603D4 File Offset: 0x0025F3D4
		private void RaiseEventForNewFirstIMEVisibleNode(TextTreeNode node)
		{
			TextPointer startPosition = new TextPointer(this, node, ElementEdge.BeforeStart);
			this.AddChange(startPosition, 0, 1, PrecursorTextChangeType.ContentRemoved);
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x002603F4 File Offset: 0x0025F3F4
		private void SetFlags(bool value, TextContainer.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x00260412 File Offset: 0x0025F412
		private bool CheckFlags(TextContainer.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x06005523 RID: 21795 RVA: 0x0026041F File Offset: 0x0025F41F
		private Dispatcher Dispatcher
		{
			get
			{
				if (this.Parent == null)
				{
					return null;
				}
				return this.Parent.Dispatcher;
			}
		}

		// Token: 0x04002F07 RID: 12039
		private readonly DependencyObject _parent;

		// Token: 0x04002F08 RID: 12040
		private TextTreeRootNode _rootNode;

		// Token: 0x04002F09 RID: 12041
		private Highlights _highlights;

		// Token: 0x04002F0A RID: 12042
		private int _changeBlockLevel;

		// Token: 0x04002F0B RID: 12043
		private TextContainerChangedEventArgs _changes;

		// Token: 0x04002F0C RID: 12044
		private ITextView _textview;

		// Token: 0x04002F0D RID: 12045
		private UndoManager _undoManager;

		// Token: 0x04002F0E RID: 12046
		private ITextSelection _textSelection;

		// Token: 0x04002F0F RID: 12047
		private ChangeBlockUndoRecord _changeBlockUndoRecord;

		// Token: 0x04002F10 RID: 12048
		private EventHandler ChangingHandler;

		// Token: 0x04002F11 RID: 12049
		private TextContainerChangeEventHandler ChangeHandler;

		// Token: 0x04002F12 RID: 12050
		private TextContainerChangedEventHandler ChangedHandler;

		// Token: 0x04002F13 RID: 12051
		private TextContainer.Flags _flags;

		// Token: 0x02000B5E RID: 2910
		private class ExtractChangeEventArgs
		{
			// Token: 0x06008DBF RID: 36287 RVA: 0x0033EEB8 File Offset: 0x0033DEB8
			internal ExtractChangeEventArgs(TextContainer textTree, TextPointer startPosition, TextTreeTextElementNode node, TextTreeTextElementNode newFirstIMEVisibleNode, TextTreeTextElementNode formerFirstIMEVisibleNode, int charCount, int childCharCount)
			{
				this._textTree = textTree;
				this._startPosition = startPosition;
				this._symbolCount = node.SymbolCount;
				this._charCount = charCount;
				this._childCharCount = childCharCount;
				this._newFirstIMEVisibleNode = newFirstIMEVisibleNode;
				this._formerFirstIMEVisibleNode = formerFirstIMEVisibleNode;
			}

			// Token: 0x06008DC0 RID: 36288 RVA: 0x0033EF08 File Offset: 0x0033DF08
			internal void AddChange()
			{
				this._textTree.AddChange(this._startPosition, this._symbolCount, this._charCount, PrecursorTextChangeType.ContentRemoved);
				if (this._newFirstIMEVisibleNode != null)
				{
					this._textTree.RaiseEventForNewFirstIMEVisibleNode(this._newFirstIMEVisibleNode);
				}
				if (this._formerFirstIMEVisibleNode != null)
				{
					this._textTree.RaiseEventForFormerFirstIMEVisibleNode(this._formerFirstIMEVisibleNode);
				}
			}

			// Token: 0x17001EFA RID: 7930
			// (get) Token: 0x06008DC1 RID: 36289 RVA: 0x0033EF65 File Offset: 0x0033DF65
			internal TextContainer TextContainer
			{
				get
				{
					return this._textTree;
				}
			}

			// Token: 0x17001EFB RID: 7931
			// (get) Token: 0x06008DC2 RID: 36290 RVA: 0x0033EF6D File Offset: 0x0033DF6D
			internal int ChildIMECharCount
			{
				get
				{
					return this._childCharCount;
				}
			}

			// Token: 0x040048B1 RID: 18609
			private readonly TextContainer _textTree;

			// Token: 0x040048B2 RID: 18610
			private readonly TextPointer _startPosition;

			// Token: 0x040048B3 RID: 18611
			private readonly int _symbolCount;

			// Token: 0x040048B4 RID: 18612
			private readonly int _charCount;

			// Token: 0x040048B5 RID: 18613
			private readonly int _childCharCount;

			// Token: 0x040048B6 RID: 18614
			private readonly TextTreeTextElementNode _newFirstIMEVisibleNode;

			// Token: 0x040048B7 RID: 18615
			private readonly TextTreeTextElementNode _formerFirstIMEVisibleNode;
		}

		// Token: 0x02000B5F RID: 2911
		[Flags]
		private enum Flags
		{
			// Token: 0x040048B9 RID: 18617
			ReadOnly = 1,
			// Token: 0x040048BA RID: 18618
			PlainTextOnly = 2,
			// Token: 0x040048BB RID: 18619
			CollectTextChanges = 4
		}
	}
}
