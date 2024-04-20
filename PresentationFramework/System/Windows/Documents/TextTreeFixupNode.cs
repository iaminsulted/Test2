using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006CA RID: 1738
	internal class TextTreeFixupNode : TextTreeNode
	{
		// Token: 0x06005A88 RID: 23176 RVA: 0x00282E23 File Offset: 0x00281E23
		internal TextTreeFixupNode(TextTreeNode previousNode, ElementEdge previousEdge, TextTreeNode nextNode, ElementEdge nextEdge) : this(previousNode, previousEdge, nextNode, nextEdge, null, null)
		{
		}

		// Token: 0x06005A89 RID: 23177 RVA: 0x00282E32 File Offset: 0x00281E32
		internal TextTreeFixupNode(TextTreeNode previousNode, ElementEdge previousEdge, TextTreeNode nextNode, ElementEdge nextEdge, TextTreeNode firstContainedNode, TextTreeNode lastContainedNode)
		{
			this._previousNode = previousNode;
			this._previousEdge = previousEdge;
			this._nextNode = nextNode;
			this._nextEdge = nextEdge;
			this._firstContainedNode = firstContainedNode;
			this._lastContainedNode = lastContainedNode;
		}

		// Token: 0x06005A8A RID: 23178 RVA: 0x00282E67 File Offset: 0x00281E67
		internal override TextTreeNode Clone()
		{
			Invariant.Assert(false, "Unexpected call to TextTreeFixupNode.Clone!");
			return null;
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x00282E75 File Offset: 0x00281E75
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			Invariant.Assert(false, "Unexpected call to TextTreeFixupNode.GetPointerContext!");
			return TextPointerContext.None;
		}

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x06005A8C RID: 23180 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005A8D RID: 23181 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x06005A8E RID: 23182 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005A8F RID: 23183 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06005A90 RID: 23184 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005A91 RID: 23185 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override int LeftSymbolCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x06005A92 RID: 23186 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005A93 RID: 23187 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override int LeftCharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x06005A94 RID: 23188 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005A95 RID: 23189 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x06005A96 RID: 23190 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005A97 RID: 23191 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x06005A98 RID: 23192 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005A99 RID: 23193 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override uint Generation
		{
			get
			{
				return 0U;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x06005A9A RID: 23194 RVA: 0x0016545A File Offset: 0x0016445A
		// (set) Token: 0x06005A9B RID: 23195 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override int SymbolOffsetCache
		{
			get
			{
				return -1;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x06005A9C RID: 23196 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005A9D RID: 23197 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override int SymbolCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x06005A9E RID: 23198 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005A9F RID: 23199 RVA: 0x00282E83 File Offset: 0x00281E83
		internal override int IMECharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "FixupNode");
			}
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AA1 RID: 23201 RVA: 0x00282E90 File Offset: 0x00281E90
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x06005AA2 RID: 23202 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AA3 RID: 23203 RVA: 0x00282E90 File Offset: 0x00281E90
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x06005AA4 RID: 23204 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AA5 RID: 23205 RVA: 0x00282E90 File Offset: 0x00281E90
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x06005AA6 RID: 23206 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AA7 RID: 23207 RVA: 0x00282E90 File Offset: 0x00281E90
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "TextTreeFixupNode should never have a position reference!");
			}
		}

		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x00282E9D File Offset: 0x00281E9D
		internal TextTreeNode PreviousNode
		{
			get
			{
				return this._previousNode;
			}
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x00282EA5 File Offset: 0x00281EA5
		internal ElementEdge PreviousEdge
		{
			get
			{
				return this._previousEdge;
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x06005AAA RID: 23210 RVA: 0x00282EAD File Offset: 0x00281EAD
		internal TextTreeNode NextNode
		{
			get
			{
				return this._nextNode;
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x06005AAB RID: 23211 RVA: 0x00282EB5 File Offset: 0x00281EB5
		internal ElementEdge NextEdge
		{
			get
			{
				return this._nextEdge;
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x06005AAC RID: 23212 RVA: 0x00282EBD File Offset: 0x00281EBD
		internal TextTreeNode FirstContainedNode
		{
			get
			{
				return this._firstContainedNode;
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x06005AAD RID: 23213 RVA: 0x00282EC5 File Offset: 0x00281EC5
		internal TextTreeNode LastContainedNode
		{
			get
			{
				return this._lastContainedNode;
			}
		}

		// Token: 0x04003058 RID: 12376
		private readonly TextTreeNode _previousNode;

		// Token: 0x04003059 RID: 12377
		private readonly ElementEdge _previousEdge;

		// Token: 0x0400305A RID: 12378
		private readonly TextTreeNode _nextNode;

		// Token: 0x0400305B RID: 12379
		private readonly ElementEdge _nextEdge;

		// Token: 0x0400305C RID: 12380
		private readonly TextTreeNode _firstContainedNode;

		// Token: 0x0400305D RID: 12381
		private readonly TextTreeNode _lastContainedNode;
	}
}
