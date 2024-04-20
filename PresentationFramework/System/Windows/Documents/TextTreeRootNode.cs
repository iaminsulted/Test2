using System;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006D0 RID: 1744
	internal class TextTreeRootNode : TextTreeNode
	{
		// Token: 0x06005AEA RID: 23274 RVA: 0x002833D9 File Offset: 0x002823D9
		internal TextTreeRootNode(TextContainer tree)
		{
			this._tree = tree;
			this._symbolCount = 2;
			this._caretUnitBoundaryCacheOffset = -1;
		}

		// Token: 0x06005AEB RID: 23275 RVA: 0x002833F6 File Offset: 0x002823F6
		internal override TextTreeNode Clone()
		{
			Invariant.Assert(false, "Unexpected call to TextTreeRootNode.Clone!");
			return null;
		}

		// Token: 0x06005AEC RID: 23276 RVA: 0x00105F35 File Offset: 0x00104F35
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.None;
		}

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x06005AED RID: 23277 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005AEE RID: 23278 RVA: 0x00283404 File Offset: 0x00282404
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set ParentNode on TextContainer root!");
			}
		}

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x06005AEF RID: 23279 RVA: 0x00283411 File Offset: 0x00282411
		// (set) Token: 0x06005AF0 RID: 23280 RVA: 0x00283419 File Offset: 0x00282419
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

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x06005AF1 RID: 23281 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AF2 RID: 23282 RVA: 0x00283427 File Offset: 0x00282427
		internal override int LeftSymbolCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root is never a sibling!");
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x06005AF3 RID: 23283 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AF4 RID: 23284 RVA: 0x00283427 File Offset: 0x00282427
		internal override int LeftCharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root is never a sibling!");
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x06005AF5 RID: 23285 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005AF6 RID: 23286 RVA: 0x00283434 File Offset: 0x00282434
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root never has sibling nodes!");
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x06005AF7 RID: 23287 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005AF8 RID: 23288 RVA: 0x00283434 File Offset: 0x00282434
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextContainer root never has sibling nodes!");
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x06005AF9 RID: 23289 RVA: 0x00283441 File Offset: 0x00282441
		// (set) Token: 0x06005AFA RID: 23290 RVA: 0x00283449 File Offset: 0x00282449
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

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x06005AFB RID: 23291 RVA: 0x00283452 File Offset: 0x00282452
		// (set) Token: 0x06005AFC RID: 23292 RVA: 0x0028345A File Offset: 0x0028245A
		internal uint PositionGeneration
		{
			get
			{
				return this._positionGeneration;
			}
			set
			{
				this._positionGeneration = value;
			}
		}

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x06005AFD RID: 23293 RVA: 0x00283463 File Offset: 0x00282463
		// (set) Token: 0x06005AFE RID: 23294 RVA: 0x0028346B File Offset: 0x0028246B
		internal uint LayoutGeneration
		{
			get
			{
				return this._layoutGeneration;
			}
			set
			{
				this._layoutGeneration = value;
				this._caretUnitBoundaryCacheOffset = -1;
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x06005AFF RID: 23295 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B00 RID: 23296 RVA: 0x0028347B File Offset: 0x0028247B
		internal override int SymbolOffsetCache
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0, "Bad SymbolOffsetCache on TextContainer root!");
			}
		}

		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x06005B01 RID: 23297 RVA: 0x0028348B File Offset: 0x0028248B
		// (set) Token: 0x06005B02 RID: 23298 RVA: 0x00283493 File Offset: 0x00282493
		internal override int SymbolCount
		{
			get
			{
				return this._symbolCount;
			}
			set
			{
				Invariant.Assert(value >= 2, "Bad _symbolCount on TextContainer root!");
				this._symbolCount = value;
			}
		}

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x06005B03 RID: 23299 RVA: 0x002834AD File Offset: 0x002824AD
		// (set) Token: 0x06005B04 RID: 23300 RVA: 0x002834B5 File Offset: 0x002824B5
		internal override int IMECharCount
		{
			get
			{
				return this._imeCharCount;
			}
			set
			{
				Invariant.Assert(value >= 0, "IMECharCount may never be negative!");
				this._imeCharCount = value;
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x06005B05 RID: 23301 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B06 RID: 23302 RVA: 0x002834CF File Offset: 0x002824CF
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(!value, "Root node BeforeStart edge can never be referenced!");
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x06005B07 RID: 23303 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B08 RID: 23304 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x06005B09 RID: 23305 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B0A RID: 23306 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x06005B0B RID: 23307 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B0C RID: 23308 RVA: 0x002834DF File Offset: 0x002824DF
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(!value, "Root node AfterEnd edge can never be referenced!");
			}
		}

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x06005B0D RID: 23309 RVA: 0x002834EF File Offset: 0x002824EF
		internal TextContainer TextContainer
		{
			get
			{
				return this._tree;
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x06005B0E RID: 23310 RVA: 0x002834F7 File Offset: 0x002824F7
		// (set) Token: 0x06005B0F RID: 23311 RVA: 0x002834FF File Offset: 0x002824FF
		internal TextTreeRootTextBlock RootTextBlock
		{
			get
			{
				return this._rootTextBlock;
			}
			set
			{
				this._rootTextBlock = value;
			}
		}

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06005B10 RID: 23312 RVA: 0x00283508 File Offset: 0x00282508
		// (set) Token: 0x06005B11 RID: 23313 RVA: 0x00283510 File Offset: 0x00282510
		internal DispatcherProcessingDisabled DispatcherProcessingDisabled
		{
			get
			{
				return this._processingDisabled;
			}
			set
			{
				this._processingDisabled = value;
			}
		}

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06005B12 RID: 23314 RVA: 0x00283519 File Offset: 0x00282519
		// (set) Token: 0x06005B13 RID: 23315 RVA: 0x00283521 File Offset: 0x00282521
		internal bool CaretUnitBoundaryCache
		{
			get
			{
				return this._caretUnitBoundaryCache;
			}
			set
			{
				this._caretUnitBoundaryCache = value;
			}
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06005B14 RID: 23316 RVA: 0x0028352A File Offset: 0x0028252A
		// (set) Token: 0x06005B15 RID: 23317 RVA: 0x00283532 File Offset: 0x00282532
		internal int CaretUnitBoundaryCacheOffset
		{
			get
			{
				return this._caretUnitBoundaryCacheOffset;
			}
			set
			{
				this._caretUnitBoundaryCacheOffset = value;
			}
		}

		// Token: 0x0400306A RID: 12394
		private readonly TextContainer _tree;

		// Token: 0x0400306B RID: 12395
		private TextTreeNode _containedNode;

		// Token: 0x0400306C RID: 12396
		private int _symbolCount;

		// Token: 0x0400306D RID: 12397
		private int _imeCharCount;

		// Token: 0x0400306E RID: 12398
		private uint _generation;

		// Token: 0x0400306F RID: 12399
		private uint _positionGeneration;

		// Token: 0x04003070 RID: 12400
		private uint _layoutGeneration;

		// Token: 0x04003071 RID: 12401
		private TextTreeRootTextBlock _rootTextBlock;

		// Token: 0x04003072 RID: 12402
		private bool _caretUnitBoundaryCache;

		// Token: 0x04003073 RID: 12403
		private int _caretUnitBoundaryCacheOffset;

		// Token: 0x04003074 RID: 12404
		private DispatcherProcessingDisabled _processingDisabled;
	}
}
