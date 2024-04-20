using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x0200019E RID: 414
	internal abstract class BamlTreeNode
	{
		// Token: 0x06000DB4 RID: 3508 RVA: 0x00136C56 File Offset: 0x00135C56
		internal BamlTreeNode(BamlNodeType type)
		{
			this.NodeType = type;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00136C65 File Offset: 0x00135C65
		internal void AddChild(BamlTreeNode child)
		{
			if (this._children == null)
			{
				this._children = new List<BamlTreeNode>();
			}
			this._children.Add(child);
			child.Parent = this;
		}

		// Token: 0x06000DB6 RID: 3510
		internal abstract BamlTreeNode Copy();

		// Token: 0x06000DB7 RID: 3511
		internal abstract void Serialize(BamlWriter writer);

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00136C8D File Offset: 0x00135C8D
		// (set) Token: 0x06000DB9 RID: 3513 RVA: 0x00136C95 File Offset: 0x00135C95
		internal BamlNodeType NodeType
		{
			get
			{
				return this._nodeType;
			}
			set
			{
				this._nodeType = value;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00136C9E File Offset: 0x00135C9E
		// (set) Token: 0x06000DBB RID: 3515 RVA: 0x00136CA6 File Offset: 0x00135CA6
		internal List<BamlTreeNode> Children
		{
			get
			{
				return this._children;
			}
			set
			{
				this._children = value;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00136CAF File Offset: 0x00135CAF
		// (set) Token: 0x06000DBD RID: 3517 RVA: 0x00136CB7 File Offset: 0x00135CB7
		internal BamlTreeNode Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x00136CC0 File Offset: 0x00135CC0
		// (set) Token: 0x06000DBF RID: 3519 RVA: 0x00136CCD File Offset: 0x00135CCD
		internal bool Formatted
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.ContentFormatted) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.ContentFormatted;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.ContentFormatted;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x00136CF3 File Offset: 0x00135CF3
		// (set) Token: 0x06000DC1 RID: 3521 RVA: 0x00136D00 File Offset: 0x00135D00
		internal bool Visited
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.NodeVisited) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.NodeVisited;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.NodeVisited;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00136D26 File Offset: 0x00135D26
		// (set) Token: 0x06000DC3 RID: 3523 RVA: 0x00136D33 File Offset: 0x00135D33
		internal bool Unidentifiable
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.Unidentifiable) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.Unidentifiable;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.Unidentifiable;
			}
		}

		// Token: 0x04000A04 RID: 2564
		protected BamlNodeType _nodeType;

		// Token: 0x04000A05 RID: 2565
		protected List<BamlTreeNode> _children;

		// Token: 0x04000A06 RID: 2566
		protected BamlTreeNode _parent;

		// Token: 0x04000A07 RID: 2567
		private BamlTreeNode.BamlTreeNodeState _state;

		// Token: 0x020009CC RID: 2508
		[Flags]
		private enum BamlTreeNodeState : byte
		{
			// Token: 0x04003FAD RID: 16301
			None = 0,
			// Token: 0x04003FAE RID: 16302
			ContentFormatted = 1,
			// Token: 0x04003FAF RID: 16303
			NodeVisited = 2,
			// Token: 0x04003FB0 RID: 16304
			Unidentifiable = 4
		}
	}
}
