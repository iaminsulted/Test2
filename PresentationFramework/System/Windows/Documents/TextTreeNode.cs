using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006CD RID: 1741
	internal abstract class TextTreeNode : SplayTreeNode
	{
		// Token: 0x06005AB2 RID: 23218
		internal abstract TextTreeNode Clone();

		// Token: 0x06005AB3 RID: 23219 RVA: 0x00282FC8 File Offset: 0x00281FC8
		internal TextContainer GetTextTree()
		{
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				SplayTreeNode containingNode = splayTreeNode.GetContainingNode();
				if (containingNode == null)
				{
					break;
				}
				splayTreeNode = containingNode;
			}
			return ((TextTreeRootNode)splayTreeNode).TextContainer;
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x00282FF0 File Offset: 0x00281FF0
		internal DependencyObject GetDependencyParent()
		{
			SplayTreeNode splayTreeNode = this;
			TextTreeTextElementNode textTreeTextElementNode;
			for (;;)
			{
				textTreeTextElementNode = (splayTreeNode as TextTreeTextElementNode);
				if (textTreeTextElementNode != null)
				{
					break;
				}
				SplayTreeNode containingNode = splayTreeNode.GetContainingNode();
				if (containingNode == null)
				{
					goto Block_2;
				}
				splayTreeNode = containingNode;
			}
			DependencyObject dependencyObject = textTreeTextElementNode.TextElement;
			Invariant.Assert(dependencyObject != null, "TextElementNode has null TextElement!");
			return dependencyObject;
			Block_2:
			dependencyObject = ((TextTreeRootNode)splayTreeNode).TextContainer.Parent;
			return dependencyObject;
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x00283044 File Offset: 0x00282044
		internal DependencyObject GetLogicalTreeNode()
		{
			TextTreeObjectNode textTreeObjectNode = this as TextTreeObjectNode;
			if (textTreeObjectNode != null && textTreeObjectNode.EmbeddedElement is FrameworkElement)
			{
				return textTreeObjectNode.EmbeddedElement;
			}
			SplayTreeNode splayTreeNode = this;
			TextTreeTextElementNode textTreeTextElementNode;
			for (;;)
			{
				textTreeTextElementNode = (splayTreeNode as TextTreeTextElementNode);
				if (textTreeTextElementNode != null)
				{
					break;
				}
				SplayTreeNode containingNode = splayTreeNode.GetContainingNode();
				if (containingNode == null)
				{
					goto Block_4;
				}
				splayTreeNode = containingNode;
			}
			return textTreeTextElementNode.TextElement;
			Block_4:
			return ((TextTreeRootNode)splayTreeNode).TextContainer.Parent;
		}

		// Token: 0x06005AB6 RID: 23222
		internal abstract TextPointerContext GetPointerContext(LogicalDirection direction);

		// Token: 0x06005AB7 RID: 23223 RVA: 0x002830A9 File Offset: 0x002820A9
		internal TextTreeNode IncrementReferenceCount(ElementEdge edge)
		{
			return this.IncrementReferenceCount(edge, 1);
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x002830B3 File Offset: 0x002820B3
		internal virtual TextTreeNode IncrementReferenceCount(ElementEdge edge, bool delta)
		{
			return this.IncrementReferenceCount(edge, delta ? 1 : 0);
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x002830C4 File Offset: 0x002820C4
		internal virtual TextTreeNode IncrementReferenceCount(ElementEdge edge, int delta)
		{
			Invariant.Assert(delta >= 0);
			if (delta > 0)
			{
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					this.BeforeStartReferenceCount = true;
					return this;
				case ElementEdge.AfterStart:
					this.AfterStartReferenceCount = true;
					return this;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					break;
				case ElementEdge.BeforeEnd:
					this.BeforeEndReferenceCount = true;
					return this;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						this.AfterEndReferenceCount = true;
						return this;
					}
					break;
				}
				Invariant.Assert(false, "Bad ElementEdge value!");
			}
			return this;
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void DecrementReferenceCount(ElementEdge edge)
		{
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x0028312F File Offset: 0x0028212F
		internal void InsertAtPosition(TextPointer position)
		{
			base.InsertAtNode(position.Node, position.Edge);
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x00283143 File Offset: 0x00282143
		internal ElementEdge GetEdgeFromOffsetNoBias(int nodeOffset)
		{
			return this.GetEdgeFromOffset(nodeOffset, LogicalDirection.Forward);
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x00283150 File Offset: 0x00282150
		internal ElementEdge GetEdgeFromOffset(int nodeOffset, LogicalDirection bias)
		{
			ElementEdge result;
			if (this.SymbolCount == 0)
			{
				result = ((bias == LogicalDirection.Forward) ? ElementEdge.AfterEnd : ElementEdge.BeforeStart);
			}
			else if (nodeOffset == 0)
			{
				result = ElementEdge.BeforeStart;
			}
			else if (nodeOffset == this.SymbolCount)
			{
				result = ElementEdge.AfterEnd;
			}
			else if (nodeOffset == 1)
			{
				result = ElementEdge.AfterStart;
			}
			else
			{
				Invariant.Assert(nodeOffset == this.SymbolCount - 1);
				result = ElementEdge.BeforeEnd;
			}
			return result;
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x002831A0 File Offset: 0x002821A0
		internal int GetOffsetFromEdge(ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				return 0;
			case ElementEdge.AfterStart:
				return 1;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				return this.SymbolCount - 1;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					return this.SymbolCount;
				}
				break;
			}
			int result = 0;
			Invariant.Assert(false, "Bad ElementEdge value!");
			return result;
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06005ABF RID: 23231
		// (set) Token: 0x06005AC0 RID: 23232
		internal abstract bool BeforeStartReferenceCount { get; set; }

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x06005AC1 RID: 23233
		// (set) Token: 0x06005AC2 RID: 23234
		internal abstract bool AfterStartReferenceCount { get; set; }

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x06005AC3 RID: 23235
		// (set) Token: 0x06005AC4 RID: 23236
		internal abstract bool BeforeEndReferenceCount { get; set; }

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x06005AC5 RID: 23237
		// (set) Token: 0x06005AC6 RID: 23238
		internal abstract bool AfterEndReferenceCount { get; set; }
	}
}
