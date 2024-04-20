using System;

namespace System.Windows.Documents
{
	// Token: 0x020005E1 RID: 1505
	internal sealed class ChildDocumentBlock
	{
		// Token: 0x060048B0 RID: 18608 RVA: 0x0022DAD6 File Offset: 0x0022CAD6
		internal ChildDocumentBlock(DocumentSequenceTextContainer aggregatedContainer, ITextContainer childContainer)
		{
			this._aggregatedContainer = aggregatedContainer;
			this._container = childContainer;
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x0022DAEC File Offset: 0x0022CAEC
		internal ChildDocumentBlock(DocumentSequenceTextContainer aggregatedContainer, DocumentReference docRef)
		{
			this._aggregatedContainer = aggregatedContainer;
			this._docRef = docRef;
			this._SetStatus(ChildDocumentBlock.BlockStatus.UnloadedBlock);
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x0022DB09 File Offset: 0x0022CB09
		internal ChildDocumentBlock InsertNextBlock(ChildDocumentBlock newBlock)
		{
			newBlock._nextBlock = this._nextBlock;
			newBlock._previousBlock = this;
			if (this._nextBlock != null)
			{
				this._nextBlock._previousBlock = newBlock;
			}
			this._nextBlock = newBlock;
			return newBlock;
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x060048B3 RID: 18611 RVA: 0x0022DB3A File Offset: 0x0022CB3A
		internal DocumentSequenceTextContainer AggregatedContainer
		{
			get
			{
				return this._aggregatedContainer;
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x0022DB42 File Offset: 0x0022CB42
		internal ITextContainer ChildContainer
		{
			get
			{
				this._EnsureBlockLoaded();
				return this._container;
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x060048B5 RID: 18613 RVA: 0x0022DB50 File Offset: 0x0022CB50
		internal DocumentSequenceHighlightLayer ChildHighlightLayer
		{
			get
			{
				if (this._highlightLayer == null)
				{
					this._highlightLayer = new DocumentSequenceHighlightLayer(this._aggregatedContainer);
					this.ChildContainer.Highlights.AddLayer(this._highlightLayer);
				}
				return this._highlightLayer;
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x060048B6 RID: 18614 RVA: 0x0022DB87 File Offset: 0x0022CB87
		internal DocumentReference DocRef
		{
			get
			{
				return this._docRef;
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x060048B7 RID: 18615 RVA: 0x0022DB8F File Offset: 0x0022CB8F
		internal ITextPointer End
		{
			get
			{
				return this.ChildContainer.End;
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x060048B8 RID: 18616 RVA: 0x0022DB9C File Offset: 0x0022CB9C
		internal bool IsHead
		{
			get
			{
				return this._previousBlock == null;
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x060048B9 RID: 18617 RVA: 0x0022DBA7 File Offset: 0x0022CBA7
		internal bool IsTail
		{
			get
			{
				return this._nextBlock == null;
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x060048BA RID: 18618 RVA: 0x0022DBB2 File Offset: 0x0022CBB2
		internal ChildDocumentBlock PreviousBlock
		{
			get
			{
				return this._previousBlock;
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x060048BB RID: 18619 RVA: 0x0022DBBA File Offset: 0x0022CBBA
		internal ChildDocumentBlock NextBlock
		{
			get
			{
				return this._nextBlock;
			}
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0022DBC4 File Offset: 0x0022CBC4
		private void _EnsureBlockLoaded()
		{
			if (this._HasStatus(ChildDocumentBlock.BlockStatus.UnloadedBlock))
			{
				this._ClearStatus(ChildDocumentBlock.BlockStatus.UnloadedBlock);
				IServiceProvider serviceProvider = this._docRef.GetDocument(false) as IServiceProvider;
				if (serviceProvider != null)
				{
					ITextContainer textContainer = serviceProvider.GetService(typeof(ITextContainer)) as ITextContainer;
					if (textContainer != null)
					{
						this._container = textContainer;
					}
				}
				if (this._container == null)
				{
					this._container = new NullTextContainer();
				}
			}
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x0022DC29 File Offset: 0x0022CC29
		private bool _HasStatus(ChildDocumentBlock.BlockStatus flags)
		{
			return (this._status & flags) == flags;
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x0022DC36 File Offset: 0x0022CC36
		private void _SetStatus(ChildDocumentBlock.BlockStatus flags)
		{
			this._status |= flags;
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x0022DC46 File Offset: 0x0022CC46
		private void _ClearStatus(ChildDocumentBlock.BlockStatus flags)
		{
			this._status &= ~flags;
		}

		// Token: 0x04002632 RID: 9778
		private readonly DocumentSequenceTextContainer _aggregatedContainer;

		// Token: 0x04002633 RID: 9779
		private readonly DocumentReference _docRef;

		// Token: 0x04002634 RID: 9780
		private ITextContainer _container;

		// Token: 0x04002635 RID: 9781
		private DocumentSequenceHighlightLayer _highlightLayer;

		// Token: 0x04002636 RID: 9782
		private ChildDocumentBlock.BlockStatus _status;

		// Token: 0x04002637 RID: 9783
		private ChildDocumentBlock _previousBlock;

		// Token: 0x04002638 RID: 9784
		private ChildDocumentBlock _nextBlock;

		// Token: 0x02000B2B RID: 2859
		[Flags]
		internal enum BlockStatus
		{
			// Token: 0x040047EB RID: 18411
			None = 0,
			// Token: 0x040047EC RID: 18412
			UnloadedBlock = 1
		}
	}
}
