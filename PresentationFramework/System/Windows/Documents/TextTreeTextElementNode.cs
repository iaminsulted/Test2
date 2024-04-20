using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006D4 RID: 1748
	internal class TextTreeTextElementNode : TextTreeNode
	{
		// Token: 0x06005B53 RID: 23379 RVA: 0x00283FD5 File Offset: 0x00282FD5
		internal TextTreeTextElementNode()
		{
			this._symbolOffsetCache = -1;
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x00283FE4 File Offset: 0x00282FE4
		internal override TextTreeNode Clone()
		{
			return new TextTreeTextElementNode
			{
				_symbolCount = this._symbolCount,
				_imeCharCount = this._imeCharCount,
				_textElement = this._textElement
			};
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x0028400F File Offset: 0x0028300F
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			if (direction != LogicalDirection.Forward)
			{
				return TextPointerContext.ElementEnd;
			}
			return TextPointerContext.ElementStart;
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x06005B56 RID: 23382 RVA: 0x00284018 File Offset: 0x00283018
		// (set) Token: 0x06005B57 RID: 23383 RVA: 0x00284020 File Offset: 0x00283020
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

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x06005B58 RID: 23384 RVA: 0x0028402E File Offset: 0x0028302E
		// (set) Token: 0x06005B59 RID: 23385 RVA: 0x00284036 File Offset: 0x00283036
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return this._containedNode;
			}
			set
			{
				this._containedNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06005B5A RID: 23386 RVA: 0x00284044 File Offset: 0x00283044
		// (set) Token: 0x06005B5B RID: 23387 RVA: 0x0028404C File Offset: 0x0028304C
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

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x00284055 File Offset: 0x00283055
		// (set) Token: 0x06005B5D RID: 23389 RVA: 0x0028405D File Offset: 0x0028305D
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

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x00284066 File Offset: 0x00283066
		// (set) Token: 0x06005B5F RID: 23391 RVA: 0x0028406E File Offset: 0x0028306E
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

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x0028407C File Offset: 0x0028307C
		// (set) Token: 0x06005B61 RID: 23393 RVA: 0x00284084 File Offset: 0x00283084
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

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x00284092 File Offset: 0x00283092
		// (set) Token: 0x06005B63 RID: 23395 RVA: 0x0028409A File Offset: 0x0028309A
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

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x06005B64 RID: 23396 RVA: 0x002840A3 File Offset: 0x002830A3
		// (set) Token: 0x06005B65 RID: 23397 RVA: 0x002840AB File Offset: 0x002830AB
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

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x06005B66 RID: 23398 RVA: 0x002840B4 File Offset: 0x002830B4
		// (set) Token: 0x06005B67 RID: 23399 RVA: 0x002840BC File Offset: 0x002830BC
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

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x06005B68 RID: 23400 RVA: 0x002840C5 File Offset: 0x002830C5
		// (set) Token: 0x06005B69 RID: 23401 RVA: 0x002840CD File Offset: 0x002830CD
		internal override int IMECharCount
		{
			get
			{
				return this._imeCharCount;
			}
			set
			{
				this._imeCharCount = value;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x06005B6A RID: 23402 RVA: 0x002840D6 File Offset: 0x002830D6
		// (set) Token: 0x06005B6B RID: 23403 RVA: 0x002840E3 File Offset: 0x002830E3
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.BeforeStart) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.BeforeStart;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06005B6C RID: 23404 RVA: 0x002840F9 File Offset: 0x002830F9
		// (set) Token: 0x06005B6D RID: 23405 RVA: 0x00284106 File Offset: 0x00283106
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.AfterStart) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.AfterStart;
			}
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06005B6E RID: 23406 RVA: 0x0028411C File Offset: 0x0028311C
		// (set) Token: 0x06005B6F RID: 23407 RVA: 0x00284129 File Offset: 0x00283129
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.BeforeEnd) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.BeforeEnd;
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06005B70 RID: 23408 RVA: 0x0028413F File Offset: 0x0028313F
		// (set) Token: 0x06005B71 RID: 23409 RVA: 0x0028414C File Offset: 0x0028314C
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.AfterEnd) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.AfterEnd;
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x06005B72 RID: 23410 RVA: 0x00284162 File Offset: 0x00283162
		// (set) Token: 0x06005B73 RID: 23411 RVA: 0x0028416A File Offset: 0x0028316A
		internal TextElement TextElement
		{
			get
			{
				return this._textElement;
			}
			set
			{
				this._textElement = value;
			}
		}

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x06005B74 RID: 23412 RVA: 0x00284173 File Offset: 0x00283173
		internal int IMELeftEdgeCharCount
		{
			get
			{
				if (this._textElement != null)
				{
					return this._textElement.IMELeftEdgeCharCount;
				}
				return -1;
			}
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x06005B75 RID: 23413 RVA: 0x0028418A File Offset: 0x0028318A
		internal bool IsFirstSibling
		{
			get
			{
				base.Splay();
				return this._leftChildNode == null;
			}
		}

		// Token: 0x0400307E RID: 12414
		private int _leftSymbolCount;

		// Token: 0x0400307F RID: 12415
		private int _leftCharCount;

		// Token: 0x04003080 RID: 12416
		private TextTreeNode _parentNode;

		// Token: 0x04003081 RID: 12417
		private TextTreeNode _leftChildNode;

		// Token: 0x04003082 RID: 12418
		private TextTreeNode _rightChildNode;

		// Token: 0x04003083 RID: 12419
		private TextTreeNode _containedNode;

		// Token: 0x04003084 RID: 12420
		private uint _generation;

		// Token: 0x04003085 RID: 12421
		private int _symbolOffsetCache;

		// Token: 0x04003086 RID: 12422
		private int _symbolCount;

		// Token: 0x04003087 RID: 12423
		private int _imeCharCount;

		// Token: 0x04003088 RID: 12424
		private TextElement _textElement;

		// Token: 0x04003089 RID: 12425
		private ElementEdge _edgeReferenceCounts;
	}
}
