using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006D5 RID: 1749
	internal class TextTreeTextNode : TextTreeNode
	{
		// Token: 0x06005B76 RID: 23414 RVA: 0x0028419B File Offset: 0x0028319B
		internal TextTreeTextNode()
		{
			this._symbolOffsetCache = -1;
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x002841AC File Offset: 0x002831AC
		internal override TextTreeNode Clone()
		{
			TextTreeTextNode textTreeTextNode = null;
			if (this._symbolCount > 0)
			{
				textTreeTextNode = new TextTreeTextNode();
				textTreeTextNode._symbolCount = this._symbolCount;
			}
			return textTreeTextNode;
		}

		// Token: 0x06005B78 RID: 23416 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.Text;
		}

		// Token: 0x06005B79 RID: 23417 RVA: 0x002841D8 File Offset: 0x002831D8
		internal override TextTreeNode IncrementReferenceCount(ElementEdge edge, int delta)
		{
			Invariant.Assert(delta >= 0);
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd, "Bad edge ref to TextTreeTextNode!");
			if (delta == 0)
			{
				return this;
			}
			TextTreeTextNode textTreeTextNode;
			if (this._positionRefCount > 0 && edge != this._referencedEdge)
			{
				textTreeTextNode = this.Split((edge == ElementEdge.BeforeStart) ? 0 : this._symbolCount, edge);
				textTreeTextNode._referencedEdge = edge;
				textTreeTextNode._positionRefCount += delta;
				TextTreeTextNode textTreeTextNode2;
				if (edge == ElementEdge.BeforeStart)
				{
					textTreeTextNode2 = (textTreeTextNode.GetPreviousNode() as TextTreeTextNode);
				}
				else
				{
					textTreeTextNode2 = (textTreeTextNode.GetNextNode() as TextTreeTextNode);
				}
				if (textTreeTextNode2 != null && textTreeTextNode2._positionRefCount == 0)
				{
					textTreeTextNode2.Merge();
				}
			}
			else
			{
				textTreeTextNode = this;
				this._referencedEdge = edge;
				this._positionRefCount += delta;
			}
			return textTreeTextNode;
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x00284290 File Offset: 0x00283290
		internal override void DecrementReferenceCount(ElementEdge edge)
		{
			Invariant.Assert(edge == this._referencedEdge, "Bad edge decrement!");
			this._positionRefCount--;
			Invariant.Assert(this._positionRefCount >= 0, "Bogus PositionRefCount! ");
			if (this._positionRefCount == 0)
			{
				this.Merge();
			}
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x002842E4 File Offset: 0x002832E4
		internal TextTreeTextNode Split(int localOffset, ElementEdge edge)
		{
			Invariant.Assert(this._symbolCount > 0, "Splitting a zero-width TextNode!");
			Invariant.Assert(localOffset >= 0 && localOffset <= this._symbolCount, "Bad localOffset!");
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterEnd, "Bad edge parameter!");
			TextTreeTextNode textTreeTextNode = new TextTreeTextNode();
			textTreeTextNode._generation = this._generation;
			base.Splay();
			ElementEdge edge2;
			TextTreeTextNode result;
			if (this._positionRefCount > 0 && this._referencedEdge == ElementEdge.BeforeStart)
			{
				textTreeTextNode._symbolOffsetCache = ((this._symbolOffsetCache == -1) ? -1 : (this._symbolOffsetCache + localOffset));
				textTreeTextNode._symbolCount = this._symbolCount - localOffset;
				this._symbolCount = localOffset;
				edge2 = ElementEdge.AfterEnd;
				result = ((edge == ElementEdge.BeforeStart) ? this : textTreeTextNode);
			}
			else
			{
				textTreeTextNode._symbolOffsetCache = this._symbolOffsetCache;
				textTreeTextNode._symbolCount = localOffset;
				this._symbolOffsetCache = ((this._symbolOffsetCache == -1) ? -1 : (this._symbolOffsetCache + localOffset));
				this._symbolCount -= localOffset;
				edge2 = ElementEdge.BeforeStart;
				result = ((edge == ElementEdge.BeforeStart) ? textTreeTextNode : this);
			}
			Invariant.Assert(this._symbolCount >= 0);
			Invariant.Assert(textTreeTextNode._symbolCount >= 0);
			textTreeTextNode.InsertAtNode(this, edge2);
			return result;
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x06005B7C RID: 23420 RVA: 0x0028440D File Offset: 0x0028340D
		// (set) Token: 0x06005B7D RID: 23421 RVA: 0x00284415 File Offset: 0x00283415
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return this._parentNode;
			}
			set
			{
				this._parentNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x06005B7E RID: 23422 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005B7F RID: 23423 RVA: 0x00284423 File Offset: 0x00283423
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set child on a TextTreeTextNode!");
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x06005B80 RID: 23424 RVA: 0x00284430 File Offset: 0x00283430
		// (set) Token: 0x06005B81 RID: 23425 RVA: 0x00284438 File Offset: 0x00283438
		internal override int LeftSymbolCount
		{
			get
			{
				return this._leftSymbolCount;
			}
			set
			{
				this._leftSymbolCount = value;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x06005B82 RID: 23426 RVA: 0x00284441 File Offset: 0x00283441
		// (set) Token: 0x06005B83 RID: 23427 RVA: 0x00284449 File Offset: 0x00283449
		internal override int LeftCharCount
		{
			get
			{
				return this._leftCharCount;
			}
			set
			{
				this._leftCharCount = value;
			}
		}

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x06005B84 RID: 23428 RVA: 0x00284452 File Offset: 0x00283452
		// (set) Token: 0x06005B85 RID: 23429 RVA: 0x0028445A File Offset: 0x0028345A
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return this._leftChildNode;
			}
			set
			{
				this._leftChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x00284468 File Offset: 0x00283468
		// (set) Token: 0x06005B87 RID: 23431 RVA: 0x00284470 File Offset: 0x00283470
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return this._rightChildNode;
			}
			set
			{
				this._rightChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x0028447E File Offset: 0x0028347E
		// (set) Token: 0x06005B89 RID: 23433 RVA: 0x00284486 File Offset: 0x00283486
		internal override uint Generation
		{
			get
			{
				return this._generation;
			}
			set
			{
				this._generation = value;
			}
		}

		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x06005B8A RID: 23434 RVA: 0x0028448F File Offset: 0x0028348F
		// (set) Token: 0x06005B8B RID: 23435 RVA: 0x00284497 File Offset: 0x00283497
		internal override int SymbolOffsetCache
		{
			get
			{
				return this._symbolOffsetCache;
			}
			set
			{
				this._symbolOffsetCache = value;
			}
		}

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x06005B8C RID: 23436 RVA: 0x002844A0 File Offset: 0x002834A0
		// (set) Token: 0x06005B8D RID: 23437 RVA: 0x002844A8 File Offset: 0x002834A8
		internal override int SymbolCount
		{
			get
			{
				return this._symbolCount;
			}
			set
			{
				this._symbolCount = value;
			}
		}

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x06005B8E RID: 23438 RVA: 0x002844B1 File Offset: 0x002834B1
		// (set) Token: 0x06005B8F RID: 23439 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override int IMECharCount
		{
			get
			{
				return this.SymbolCount;
			}
			set
			{
			}
		}

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x06005B90 RID: 23440 RVA: 0x002844B9 File Offset: 0x002834B9
		// (set) Token: 0x06005B91 RID: 23441 RVA: 0x002844CF File Offset: 0x002834CF
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return this._referencedEdge == ElementEdge.BeforeStart && this._positionRefCount > 0;
			}
			set
			{
				Invariant.Assert(false, "Can't set TextTreeTextNode ref counts directly!");
			}
		}

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x06005B92 RID: 23442 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B93 RID: 23443 RVA: 0x002844DC File Offset: 0x002834DC
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Text nodes don't have an AfterStart edge!");
			}
		}

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x06005B94 RID: 23444 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B95 RID: 23445 RVA: 0x002844E9 File Offset: 0x002834E9
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Text nodes don't have a BeforeEnd edge!");
			}
		}

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x06005B96 RID: 23446 RVA: 0x002844F6 File Offset: 0x002834F6
		// (set) Token: 0x06005B97 RID: 23447 RVA: 0x002844CF File Offset: 0x002834CF
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return this._referencedEdge == ElementEdge.AfterEnd && this._positionRefCount > 0;
			}
			set
			{
				Invariant.Assert(false, "Can't set TextTreeTextNode ref counts directly!");
			}
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x0028450C File Offset: 0x0028350C
		private void Merge()
		{
			Invariant.Assert(this._positionRefCount == 0, "Inappropriate Merge call!");
			TextTreeTextNode textTreeTextNode = base.GetPreviousNode() as TextTreeTextNode;
			if (textTreeTextNode != null && (textTreeTextNode._positionRefCount == 0 || textTreeTextNode._referencedEdge == ElementEdge.BeforeStart))
			{
				base.Remove();
				this._parentNode = null;
				textTreeTextNode.Splay();
				textTreeTextNode._symbolCount += this._symbolCount;
			}
			else
			{
				textTreeTextNode = this;
			}
			TextTreeTextNode textTreeTextNode2 = textTreeTextNode.GetNextNode() as TextTreeTextNode;
			if (textTreeTextNode2 != null)
			{
				if (textTreeTextNode._positionRefCount == 0 && (textTreeTextNode2._positionRefCount == 0 || textTreeTextNode2._referencedEdge == ElementEdge.AfterEnd))
				{
					textTreeTextNode.Remove();
					textTreeTextNode._parentNode = null;
					textTreeTextNode2.Splay();
					if (textTreeTextNode2._symbolOffsetCache != -1)
					{
						textTreeTextNode2._symbolOffsetCache -= textTreeTextNode._symbolCount;
					}
					textTreeTextNode2._symbolCount += textTreeTextNode._symbolCount;
					return;
				}
				if ((textTreeTextNode._positionRefCount == 0 || textTreeTextNode._referencedEdge == ElementEdge.BeforeStart) && textTreeTextNode2._positionRefCount == 0)
				{
					textTreeTextNode2.Remove();
					textTreeTextNode2._parentNode = null;
					textTreeTextNode.Splay();
					textTreeTextNode._symbolCount += textTreeTextNode2._symbolCount;
				}
			}
		}

		// Token: 0x0400308A RID: 12426
		private int _leftSymbolCount;

		// Token: 0x0400308B RID: 12427
		private int _leftCharCount;

		// Token: 0x0400308C RID: 12428
		private TextTreeNode _parentNode;

		// Token: 0x0400308D RID: 12429
		private TextTreeNode _leftChildNode;

		// Token: 0x0400308E RID: 12430
		private TextTreeNode _rightChildNode;

		// Token: 0x0400308F RID: 12431
		private uint _generation;

		// Token: 0x04003090 RID: 12432
		private int _symbolOffsetCache;

		// Token: 0x04003091 RID: 12433
		private int _symbolCount;

		// Token: 0x04003092 RID: 12434
		private int _positionRefCount;

		// Token: 0x04003093 RID: 12435
		private ElementEdge _referencedEdge;
	}
}
