using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200068C RID: 1676
	internal abstract class SplayTreeNode
	{
		// Token: 0x0600532B RID: 21291 RVA: 0x0025BA48 File Offset: 0x0025AA48
		internal SplayTreeNode GetSiblingAtOffset(int offset, out int nodeOffset)
		{
			SplayTreeNode splayTreeNode = this;
			nodeOffset = 0;
			int leftSymbolCount;
			for (;;)
			{
				leftSymbolCount = splayTreeNode.LeftSymbolCount;
				if (offset < nodeOffset + leftSymbolCount)
				{
					splayTreeNode = splayTreeNode.LeftChildNode;
				}
				else
				{
					int symbolCount = splayTreeNode.SymbolCount;
					if (offset <= nodeOffset + leftSymbolCount + symbolCount)
					{
						break;
					}
					nodeOffset += leftSymbolCount + symbolCount;
					splayTreeNode = splayTreeNode.RightChildNode;
				}
			}
			nodeOffset += leftSymbolCount;
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x0025BAA4 File Offset: 0x0025AAA4
		internal SplayTreeNode GetSiblingAtCharOffset(int charOffset, out int nodeCharOffset)
		{
			SplayTreeNode splayTreeNode = this;
			nodeCharOffset = 0;
			int leftCharCount;
			for (;;)
			{
				leftCharCount = splayTreeNode.LeftCharCount;
				if (charOffset < nodeCharOffset + leftCharCount)
				{
					splayTreeNode = splayTreeNode.LeftChildNode;
				}
				else if (charOffset == nodeCharOffset + leftCharCount && charOffset > 0)
				{
					splayTreeNode = splayTreeNode.LeftChildNode;
				}
				else
				{
					int imecharCount = splayTreeNode.IMECharCount;
					if (imecharCount > 0 && charOffset <= nodeCharOffset + leftCharCount + imecharCount)
					{
						break;
					}
					nodeCharOffset += leftCharCount + imecharCount;
					splayTreeNode = splayTreeNode.RightChildNode;
				}
			}
			nodeCharOffset += leftCharCount;
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x0600532D RID: 21293 RVA: 0x0025BB18 File Offset: 0x0025AB18
		internal SplayTreeNode GetFirstContainedNode()
		{
			SplayTreeNode containedNode = this.ContainedNode;
			if (containedNode == null)
			{
				return null;
			}
			return containedNode.GetMinSibling();
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x0025BB38 File Offset: 0x0025AB38
		internal SplayTreeNode GetLastContainedNode()
		{
			SplayTreeNode containedNode = this.ContainedNode;
			if (containedNode == null)
			{
				return null;
			}
			return containedNode.GetMaxSibling();
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x0025BB57 File Offset: 0x0025AB57
		internal SplayTreeNode GetContainingNode()
		{
			this.Splay();
			return this.ParentNode;
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x0025BB68 File Offset: 0x0025AB68
		internal SplayTreeNode GetPreviousNode()
		{
			SplayTreeNode splayTreeNode = this.LeftChildNode;
			if (splayTreeNode != null)
			{
				for (;;)
				{
					SplayTreeNode rightChildNode = splayTreeNode.RightChildNode;
					if (rightChildNode == null)
					{
						break;
					}
					splayTreeNode = rightChildNode;
				}
			}
			else
			{
				SplayTreeNodeRole role = this.Role;
				splayTreeNode = this.ParentNode;
				while (role != SplayTreeNodeRole.LocalRoot)
				{
					if (role == SplayTreeNodeRole.RightChild)
					{
						goto IL_41;
					}
					role = splayTreeNode.Role;
					splayTreeNode = splayTreeNode.ParentNode;
				}
				splayTreeNode = null;
			}
			IL_41:
			if (splayTreeNode != null)
			{
				splayTreeNode.Splay();
			}
			return splayTreeNode;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x0025BBC0 File Offset: 0x0025ABC0
		internal SplayTreeNode GetNextNode()
		{
			SplayTreeNode splayTreeNode = this.RightChildNode;
			if (splayTreeNode != null)
			{
				for (;;)
				{
					SplayTreeNode leftChildNode = splayTreeNode.LeftChildNode;
					if (leftChildNode == null)
					{
						break;
					}
					splayTreeNode = leftChildNode;
				}
			}
			else
			{
				SplayTreeNodeRole role = this.Role;
				splayTreeNode = this.ParentNode;
				while (role != SplayTreeNodeRole.LocalRoot)
				{
					if (role == SplayTreeNodeRole.LeftChild)
					{
						goto IL_41;
					}
					role = splayTreeNode.Role;
					splayTreeNode = splayTreeNode.ParentNode;
				}
				splayTreeNode = null;
			}
			IL_41:
			if (splayTreeNode != null)
			{
				splayTreeNode.Splay();
			}
			return splayTreeNode;
		}

		// Token: 0x06005332 RID: 21298 RVA: 0x0025BC18 File Offset: 0x0025AC18
		internal int GetSymbolOffset(uint treeGeneration)
		{
			int num = 0;
			SplayTreeNode splayTreeNode = this;
			while (splayTreeNode.Generation != treeGeneration || splayTreeNode.SymbolOffsetCache < 0)
			{
				splayTreeNode.Splay();
				num += splayTreeNode.LeftSymbolCount;
				num++;
				splayTreeNode = splayTreeNode.ParentNode;
			}
			num += splayTreeNode.SymbolOffsetCache;
			this.Generation = treeGeneration;
			this.SymbolOffsetCache = num;
			return num;
		}

		// Token: 0x06005333 RID: 21299 RVA: 0x0025BC74 File Offset: 0x0025AC74
		internal int GetIMECharOffset()
		{
			int num = 0;
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				splayTreeNode.Splay();
				num += splayTreeNode.LeftCharCount;
				splayTreeNode = splayTreeNode.ParentNode;
				if (splayTreeNode == null)
				{
					break;
				}
				TextTreeTextElementNode textTreeTextElementNode = splayTreeNode as TextTreeTextElementNode;
				if (textTreeTextElementNode != null)
				{
					num += textTreeTextElementNode.IMELeftEdgeCharCount;
				}
			}
			return num;
		}

		// Token: 0x06005334 RID: 21300 RVA: 0x0025BCB4 File Offset: 0x0025ACB4
		internal void InsertAtNode(SplayTreeNode positionNode, ElementEdge edge)
		{
			if (edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd)
			{
				this.InsertAtNode(positionNode, edge == ElementEdge.BeforeStart);
				return;
			}
			SplayTreeNode splayTreeNode;
			bool insertBefore;
			if (edge == ElementEdge.AfterStart)
			{
				splayTreeNode = positionNode.GetFirstContainedNode();
				insertBefore = true;
			}
			else
			{
				splayTreeNode = positionNode.GetLastContainedNode();
				insertBefore = false;
			}
			if (splayTreeNode == null)
			{
				positionNode.ContainedNode = this;
				this.ParentNode = positionNode;
				Invariant.Assert(this.LeftChildNode == null);
				Invariant.Assert(this.RightChildNode == null);
				Invariant.Assert(this.LeftSymbolCount == 0);
				return;
			}
			this.InsertAtNode(splayTreeNode, insertBefore);
		}

		// Token: 0x06005335 RID: 21301 RVA: 0x0025BD34 File Offset: 0x0025AD34
		internal void InsertAtNode(SplayTreeNode location, bool insertBefore)
		{
			Invariant.Assert(this.ParentNode == null, "Can't insert child node!");
			Invariant.Assert(this.LeftChildNode == null, "Can't insert node with left children!");
			Invariant.Assert(this.RightChildNode == null, "Can't insert node with right children!");
			SplayTreeNode splayTreeNode = insertBefore ? location.GetPreviousNode() : location;
			SplayTreeNode rightSubTree;
			SplayTreeNode parentNode;
			if (splayTreeNode != null)
			{
				rightSubTree = splayTreeNode.Split();
				parentNode = splayTreeNode.ParentNode;
			}
			else
			{
				rightSubTree = location;
				location.Splay();
				Invariant.Assert(location.Role == SplayTreeNodeRole.LocalRoot, "location should be local root!");
				parentNode = location.ParentNode;
			}
			SplayTreeNode.Join(this, splayTreeNode, rightSubTree);
			this.ParentNode = parentNode;
			if (parentNode != null)
			{
				parentNode.ContainedNode = this;
			}
		}

		// Token: 0x06005336 RID: 21302 RVA: 0x0025BDD8 File Offset: 0x0025ADD8
		internal void Remove()
		{
			this.Splay();
			Invariant.Assert(this.Role == SplayTreeNodeRole.LocalRoot);
			SplayTreeNode parentNode = this.ParentNode;
			SplayTreeNode leftChildNode = this.LeftChildNode;
			SplayTreeNode rightChildNode = this.RightChildNode;
			if (leftChildNode != null)
			{
				leftChildNode.ParentNode = null;
			}
			if (rightChildNode != null)
			{
				rightChildNode.ParentNode = null;
			}
			SplayTreeNode splayTreeNode = SplayTreeNode.Join(leftChildNode, rightChildNode);
			if (parentNode != null)
			{
				parentNode.ContainedNode = splayTreeNode;
			}
			if (splayTreeNode != null)
			{
				splayTreeNode.ParentNode = parentNode;
			}
			this.ParentNode = null;
			this.LeftChildNode = null;
			this.RightChildNode = null;
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x0025BE54 File Offset: 0x0025AE54
		internal static void Join(SplayTreeNode root, SplayTreeNode leftSubTree, SplayTreeNode rightSubTree)
		{
			root.LeftChildNode = leftSubTree;
			root.RightChildNode = rightSubTree;
			Invariant.Assert(root.Role == SplayTreeNodeRole.LocalRoot);
			if (leftSubTree != null)
			{
				leftSubTree.ParentNode = root;
				root.LeftSymbolCount = leftSubTree.LeftSymbolCount + leftSubTree.SymbolCount;
				root.LeftCharCount = leftSubTree.LeftCharCount + leftSubTree.IMECharCount;
			}
			else
			{
				root.LeftSymbolCount = 0;
				root.LeftCharCount = 0;
			}
			if (rightSubTree != null)
			{
				rightSubTree.ParentNode = root;
			}
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x0025BEC8 File Offset: 0x0025AEC8
		internal static SplayTreeNode Join(SplayTreeNode leftSubTree, SplayTreeNode rightSubTree)
		{
			Invariant.Assert(leftSubTree == null || leftSubTree.ParentNode == null);
			Invariant.Assert(rightSubTree == null || rightSubTree.ParentNode == null);
			SplayTreeNode splayTreeNode;
			if (leftSubTree != null)
			{
				splayTreeNode = leftSubTree.GetMaxSibling();
				splayTreeNode.Splay();
				Invariant.Assert(splayTreeNode.Role == SplayTreeNodeRole.LocalRoot);
				Invariant.Assert(splayTreeNode.RightChildNode == null);
				splayTreeNode.RightChildNode = rightSubTree;
				if (rightSubTree != null)
				{
					rightSubTree.ParentNode = splayTreeNode;
				}
			}
			else if (rightSubTree != null)
			{
				splayTreeNode = rightSubTree;
				Invariant.Assert(splayTreeNode.Role == SplayTreeNodeRole.LocalRoot);
			}
			else
			{
				splayTreeNode = null;
			}
			return splayTreeNode;
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x0025BF54 File Offset: 0x0025AF54
		internal SplayTreeNode Split()
		{
			this.Splay();
			Invariant.Assert(this.Role == SplayTreeNodeRole.LocalRoot, "location should be local root!");
			SplayTreeNode rightChildNode = this.RightChildNode;
			if (rightChildNode != null)
			{
				rightChildNode.ParentNode = null;
				this.RightChildNode = null;
			}
			return rightChildNode;
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x0025BF94 File Offset: 0x0025AF94
		internal SplayTreeNode GetMinSibling()
		{
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				SplayTreeNode leftChildNode = splayTreeNode.LeftChildNode;
				if (leftChildNode == null)
				{
					break;
				}
				splayTreeNode = leftChildNode;
			}
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x0025BFB8 File Offset: 0x0025AFB8
		internal SplayTreeNode GetMaxSibling()
		{
			SplayTreeNode splayTreeNode = this;
			for (;;)
			{
				SplayTreeNode rightChildNode = splayTreeNode.RightChildNode;
				if (rightChildNode == null)
				{
					break;
				}
				splayTreeNode = rightChildNode;
			}
			splayTreeNode.Splay();
			return splayTreeNode;
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x0025BFDC File Offset: 0x0025AFDC
		internal void Splay()
		{
			SplayTreeNodeRole role;
			SplayTreeNode parentNode;
			for (;;)
			{
				role = this.Role;
				if (role == SplayTreeNodeRole.LocalRoot)
				{
					goto IL_7F;
				}
				parentNode = this.ParentNode;
				SplayTreeNodeRole role2 = parentNode.Role;
				if (role2 == SplayTreeNodeRole.LocalRoot)
				{
					break;
				}
				SplayTreeNode parentNode2 = parentNode.ParentNode;
				if (role == role2)
				{
					if (role == SplayTreeNodeRole.LeftChild)
					{
						parentNode2.RotateRight();
						parentNode.RotateRight();
					}
					else
					{
						parentNode2.RotateLeft();
						parentNode.RotateLeft();
					}
				}
				else if (role == SplayTreeNodeRole.LeftChild)
				{
					parentNode.RotateRight();
					parentNode2.RotateLeft();
				}
				else
				{
					parentNode.RotateLeft();
					parentNode2.RotateRight();
				}
			}
			if (role == SplayTreeNodeRole.LeftChild)
			{
				parentNode.RotateRight();
			}
			else
			{
				parentNode.RotateLeft();
			}
			IL_7F:
			Invariant.Assert(this.Role == SplayTreeNodeRole.LocalRoot, "Splay didn't move node to root!");
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x0025C07B File Offset: 0x0025B07B
		internal bool IsChildOfNode(SplayTreeNode parentNode)
		{
			return parentNode.LeftChildNode == this || parentNode.RightChildNode == this || parentNode.ContainedNode == this;
		}

		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x0600533E RID: 21310
		// (set) Token: 0x0600533F RID: 21311
		internal abstract SplayTreeNode ParentNode { get; set; }

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06005340 RID: 21312
		// (set) Token: 0x06005341 RID: 21313
		internal abstract SplayTreeNode ContainedNode { get; set; }

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06005342 RID: 21314
		// (set) Token: 0x06005343 RID: 21315
		internal abstract SplayTreeNode LeftChildNode { get; set; }

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06005344 RID: 21316
		// (set) Token: 0x06005345 RID: 21317
		internal abstract SplayTreeNode RightChildNode { get; set; }

		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06005346 RID: 21318
		// (set) Token: 0x06005347 RID: 21319
		internal abstract int SymbolCount { get; set; }

		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x06005348 RID: 21320
		// (set) Token: 0x06005349 RID: 21321
		internal abstract int IMECharCount { get; set; }

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x0600534A RID: 21322
		// (set) Token: 0x0600534B RID: 21323
		internal abstract int LeftSymbolCount { get; set; }

		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x0600534C RID: 21324
		// (set) Token: 0x0600534D RID: 21325
		internal abstract int LeftCharCount { get; set; }

		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x0600534E RID: 21326
		// (set) Token: 0x0600534F RID: 21327
		internal abstract uint Generation { get; set; }

		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06005350 RID: 21328
		// (set) Token: 0x06005351 RID: 21329
		internal abstract int SymbolOffsetCache { get; set; }

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06005352 RID: 21330 RVA: 0x0025C09C File Offset: 0x0025B09C
		internal SplayTreeNodeRole Role
		{
			get
			{
				SplayTreeNode parentNode = this.ParentNode;
				SplayTreeNodeRole result;
				if (parentNode == null || parentNode.ContainedNode == this)
				{
					result = SplayTreeNodeRole.LocalRoot;
				}
				else if (parentNode.LeftChildNode == this)
				{
					result = SplayTreeNodeRole.LeftChild;
				}
				else
				{
					Invariant.Assert(parentNode.RightChildNode == this, "Node has no relation to parent!");
					result = SplayTreeNodeRole.RightChild;
				}
				return result;
			}
		}

		// Token: 0x06005353 RID: 21331 RVA: 0x0025C0E4 File Offset: 0x0025B0E4
		private void RotateLeft()
		{
			Invariant.Assert(this.RightChildNode != null, "Can't rotate left with null right child!");
			SplayTreeNode rightChildNode = this.RightChildNode;
			this.RightChildNode = rightChildNode.LeftChildNode;
			if (rightChildNode.LeftChildNode != null)
			{
				rightChildNode.LeftChildNode.ParentNode = this;
			}
			SplayTreeNode parentNode = this.ParentNode;
			rightChildNode.ParentNode = parentNode;
			if (parentNode != null)
			{
				if (parentNode.ContainedNode == this)
				{
					parentNode.ContainedNode = rightChildNode;
				}
				else if (this.Role == SplayTreeNodeRole.LeftChild)
				{
					parentNode.LeftChildNode = rightChildNode;
				}
				else
				{
					parentNode.RightChildNode = rightChildNode;
				}
			}
			rightChildNode.LeftChildNode = this;
			this.ParentNode = rightChildNode;
			SplayTreeNode rightChildNode2 = this.RightChildNode;
			rightChildNode.LeftSymbolCount += this.LeftSymbolCount + this.SymbolCount;
			rightChildNode.LeftCharCount += this.LeftCharCount + this.IMECharCount;
		}

		// Token: 0x06005354 RID: 21332 RVA: 0x0025C1B0 File Offset: 0x0025B1B0
		private void RotateRight()
		{
			Invariant.Assert(this.LeftChildNode != null, "Can't rotate right with null left child!");
			SplayTreeNode leftChildNode = this.LeftChildNode;
			this.LeftChildNode = leftChildNode.RightChildNode;
			if (leftChildNode.RightChildNode != null)
			{
				leftChildNode.RightChildNode.ParentNode = this;
			}
			SplayTreeNode parentNode = this.ParentNode;
			leftChildNode.ParentNode = parentNode;
			if (parentNode != null)
			{
				if (parentNode.ContainedNode == this)
				{
					parentNode.ContainedNode = leftChildNode;
				}
				else if (this.Role == SplayTreeNodeRole.LeftChild)
				{
					parentNode.LeftChildNode = leftChildNode;
				}
				else
				{
					parentNode.RightChildNode = leftChildNode;
				}
			}
			leftChildNode.RightChildNode = this;
			this.ParentNode = leftChildNode;
			SplayTreeNode leftChildNode2 = this.LeftChildNode;
			this.LeftSymbolCount -= leftChildNode.LeftSymbolCount + leftChildNode.SymbolCount;
			this.LeftCharCount -= leftChildNode.LeftCharCount + leftChildNode.IMECharCount;
		}
	}
}
