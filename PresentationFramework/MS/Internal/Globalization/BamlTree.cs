using System;
using System.Collections.Generic;

namespace MS.Internal.Globalization
{
	// Token: 0x0200019C RID: 412
	internal sealed class BamlTree
	{
		// Token: 0x06000DA7 RID: 3495 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal BamlTree()
		{
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00136B38 File Offset: 0x00135B38
		internal BamlTree(BamlTreeNode root, int size)
		{
			this._root = root;
			this._nodeList = new List<BamlTreeNode>(size);
			this.CreateInternalIndex(ref this._root, ref this._nodeList, false);
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x00136B66 File Offset: 0x00135B66
		internal BamlTreeNode Root
		{
			get
			{
				return this._root;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x00136B6E File Offset: 0x00135B6E
		internal int Size
		{
			get
			{
				return this._nodeList.Count;
			}
		}

		// Token: 0x1700023B RID: 571
		internal BamlTreeNode this[int i]
		{
			get
			{
				return this._nodeList[i];
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00136B8C File Offset: 0x00135B8C
		internal BamlTree Copy()
		{
			BamlTreeNode root = this._root;
			List<BamlTreeNode> nodeList = new List<BamlTreeNode>(this.Size);
			this.CreateInternalIndex(ref root, ref nodeList, true);
			return new BamlTree
			{
				_root = root,
				_nodeList = nodeList
			};
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00136BCA File Offset: 0x00135BCA
		internal void AddTreeNode(BamlTreeNode node)
		{
			this._nodeList.Add(node);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00136BD8 File Offset: 0x00135BD8
		private void CreateInternalIndex(ref BamlTreeNode parent, ref List<BamlTreeNode> nodeList, bool toCopy)
		{
			List<BamlTreeNode> children = parent.Children;
			if (toCopy)
			{
				parent = parent.Copy();
				if (children != null)
				{
					parent.Children = new List<BamlTreeNode>(children.Count);
				}
			}
			nodeList.Add(parent);
			if (children == null)
			{
				return;
			}
			for (int i = 0; i < children.Count; i++)
			{
				BamlTreeNode bamlTreeNode = children[i];
				this.CreateInternalIndex(ref bamlTreeNode, ref nodeList, toCopy);
				if (toCopy)
				{
					bamlTreeNode.Parent = parent;
					parent.Children.Add(bamlTreeNode);
				}
			}
		}

		// Token: 0x04000A02 RID: 2562
		private BamlTreeNode _root;

		// Token: 0x04000A03 RID: 2563
		private List<BamlTreeNode> _nodeList;
	}
}
