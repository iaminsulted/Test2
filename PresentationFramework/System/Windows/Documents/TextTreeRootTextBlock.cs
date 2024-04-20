using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006D1 RID: 1745
	internal class TextTreeRootTextBlock : SplayTreeNode
	{
		// Token: 0x06005B16 RID: 23318 RVA: 0x0028353B File Offset: 0x0028253B
		internal TextTreeRootTextBlock()
		{
			new TextTreeTextBlock(2).InsertAtNode(this, ElementEdge.AfterStart);
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x06005B17 RID: 23319 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005B18 RID: 23320 RVA: 0x00283550 File Offset: 0x00282550
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set ParentNode on TextBlock root!");
			}
		}

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x06005B19 RID: 23321 RVA: 0x0028355D File Offset: 0x0028255D
		// (set) Token: 0x06005B1A RID: 23322 RVA: 0x00283565 File Offset: 0x00282565
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return this._containedNode;
			}
			set
			{
				this._containedNode = (TextTreeTextBlock)value;
			}
		}

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x06005B1B RID: 23323 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B1C RID: 23324 RVA: 0x00283427 File Offset: 0x00282427
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

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x06005B1D RID: 23325 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B1E RID: 23326 RVA: 0x00283573 File Offset: 0x00282573
		internal override int LeftCharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0);
			}
		}

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x06005B1F RID: 23327 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005B20 RID: 23328 RVA: 0x0028357E File Offset: 0x0028257E
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextBlock root never has sibling nodes!");
			}
		}

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x06005B21 RID: 23329 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005B22 RID: 23330 RVA: 0x0028357E File Offset: 0x0028257E
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "TextBlock root never has sibling nodes!");
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x06005B23 RID: 23331 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B24 RID: 23332 RVA: 0x0028358B File Offset: 0x0028258B
		internal override uint Generation
		{
			get
			{
				return 0U;
			}
			set
			{
				Invariant.Assert(false, "TextTreeRootTextBlock does not track Generation!");
			}
		}

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06005B25 RID: 23333 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B26 RID: 23334 RVA: 0x00283598 File Offset: 0x00282598
		internal override int SymbolOffsetCache
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(false, "TextTreeRootTextBlock does not track SymbolOffsetCache!");
			}
		}

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x06005B27 RID: 23335 RVA: 0x0016545A File Offset: 0x0016445A
		// (set) Token: 0x06005B28 RID: 23336 RVA: 0x002835A5 File Offset: 0x002825A5
		internal override int SymbolCount
		{
			get
			{
				return -1;
			}
			set
			{
				Invariant.Assert(false, "TextTreeRootTextBlock does not track symbol count!");
			}
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x06005B29 RID: 23337 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B2A RID: 23338 RVA: 0x00283573 File Offset: 0x00282573
		internal override int IMECharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0);
			}
		}

		// Token: 0x04003075 RID: 12405
		private TextTreeTextBlock _containedNode;
	}
}
