using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006CE RID: 1742
	internal class TextTreeObjectNode : TextTreeNode
	{
		// Token: 0x06005AC8 RID: 23240 RVA: 0x002831FD File Offset: 0x002821FD
		internal TextTreeObjectNode(DependencyObject embeddedElement)
		{
			this._embeddedElement = embeddedElement;
			this._symbolOffsetCache = -1;
		}

		// Token: 0x06005AC9 RID: 23241 RVA: 0x00283213 File Offset: 0x00282213
		internal override TextTreeNode Clone()
		{
			return new TextTreeObjectNode(this._embeddedElement);
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x0010A7E1 File Offset: 0x001097E1
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.EmbeddedElement;
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x06005ACB RID: 23243 RVA: 0x00283220 File Offset: 0x00282220
		// (set) Token: 0x06005ACC RID: 23244 RVA: 0x00283228 File Offset: 0x00282228
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

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x06005ACD RID: 23245 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005ACE RID: 23246 RVA: 0x00283236 File Offset: 0x00282236
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set contained node on a TextTreeObjectNode!");
			}
		}

		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06005ACF RID: 23247 RVA: 0x00283243 File Offset: 0x00282243
		// (set) Token: 0x06005AD0 RID: 23248 RVA: 0x0028324B File Offset: 0x0028224B
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

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06005AD1 RID: 23249 RVA: 0x00283254 File Offset: 0x00282254
		// (set) Token: 0x06005AD2 RID: 23250 RVA: 0x0028325C File Offset: 0x0028225C
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

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06005AD3 RID: 23251 RVA: 0x00283265 File Offset: 0x00282265
		// (set) Token: 0x06005AD4 RID: 23252 RVA: 0x0028326D File Offset: 0x0028226D
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

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x0028327B File Offset: 0x0028227B
		// (set) Token: 0x06005AD6 RID: 23254 RVA: 0x00283283 File Offset: 0x00282283
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

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x06005AD7 RID: 23255 RVA: 0x00283291 File Offset: 0x00282291
		// (set) Token: 0x06005AD8 RID: 23256 RVA: 0x00283299 File Offset: 0x00282299
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

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x06005AD9 RID: 23257 RVA: 0x002832A2 File Offset: 0x002822A2
		// (set) Token: 0x06005ADA RID: 23258 RVA: 0x002832AA File Offset: 0x002822AA
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

		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x06005ADB RID: 23259 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		// (set) Token: 0x06005ADC RID: 23260 RVA: 0x002832B3 File Offset: 0x002822B3
		internal override int SymbolCount
		{
			get
			{
				return 1;
			}
			set
			{
				Invariant.Assert(false, "Can't set SymbolCount on TextTreeObjectNode!");
			}
		}

		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x06005ADD RID: 23261 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		// (set) Token: 0x06005ADE RID: 23262 RVA: 0x002832C0 File Offset: 0x002822C0
		internal override int IMECharCount
		{
			get
			{
				return 1;
			}
			set
			{
				Invariant.Assert(false, "Can't set CharCount on TextTreeObjectNode!");
			}
		}

		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x06005ADF RID: 23263 RVA: 0x002832CD File Offset: 0x002822CD
		// (set) Token: 0x06005AE0 RID: 23264 RVA: 0x002832DA File Offset: 0x002822DA
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

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x06005AE1 RID: 23265 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AE2 RID: 23266 RVA: 0x002832F0 File Offset: 0x002822F0
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Object nodes don't have an AfterStart edge!");
			}
		}

		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x06005AE3 RID: 23267 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005AE4 RID: 23268 RVA: 0x002832FD File Offset: 0x002822FD
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Object nodes don't have a BeforeEnd edge!");
			}
		}

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x06005AE5 RID: 23269 RVA: 0x0028330A File Offset: 0x0028230A
		// (set) Token: 0x06005AE6 RID: 23270 RVA: 0x00283317 File Offset: 0x00282317
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

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x06005AE7 RID: 23271 RVA: 0x0028332D File Offset: 0x0028232D
		internal DependencyObject EmbeddedElement
		{
			get
			{
				return this._embeddedElement;
			}
		}

		// Token: 0x04003060 RID: 12384
		private int _leftSymbolCount;

		// Token: 0x04003061 RID: 12385
		private int _leftCharCount;

		// Token: 0x04003062 RID: 12386
		private TextTreeNode _parentNode;

		// Token: 0x04003063 RID: 12387
		private TextTreeNode _leftChildNode;

		// Token: 0x04003064 RID: 12388
		private TextTreeNode _rightChildNode;

		// Token: 0x04003065 RID: 12389
		private uint _generation;

		// Token: 0x04003066 RID: 12390
		private int _symbolOffsetCache;

		// Token: 0x04003067 RID: 12391
		private ElementEdge _edgeReferenceCounts;

		// Token: 0x04003068 RID: 12392
		private readonly DependencyObject _embeddedElement;
	}
}
