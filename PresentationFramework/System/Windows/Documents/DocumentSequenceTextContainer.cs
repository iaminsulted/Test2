using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x020005E9 RID: 1513
	internal sealed class DocumentSequenceTextContainer : ITextContainer
	{
		// Token: 0x0600491A RID: 18714 RVA: 0x0022F661 File Offset: 0x0022E661
		internal DocumentSequenceTextContainer(DependencyObject parent)
		{
			this._parent = (FixedDocumentSequence)parent;
			this._Initialize();
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x0022F67B File Offset: 0x0022E67B
		void ITextContainer.BeginChange()
		{
			this._changeBlockLevel++;
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x0022F68B File Offset: 0x0022E68B
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x0022F693 File Offset: 0x0022E693
		void ITextContainer.EndChange()
		{
			((ITextContainer)this).EndChange(false);
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x0022F69C File Offset: 0x0022E69C
		void ITextContainer.EndChange(bool skipEvents)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "Unmatched EndChange call!");
			this._changeBlockLevel--;
			if (this._changeBlockLevel == 0 && this._changes != null)
			{
				TextContainerChangedEventArgs changes = this._changes;
				this._changes = null;
				if (this.Changed != null && !skipEvents)
				{
					this.Changed(this, changes);
				}
			}
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x0022F700 File Offset: 0x0022E700
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return ((ITextContainer)this).Start.CreatePointer(offset, direction);
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x001056E1 File Offset: 0x001046E1
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x0022F70F File Offset: 0x0022E70F
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x0022F723 File Offset: 0x0022E723
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x0022F733 File Offset: 0x0022E733
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x0022F747 File Offset: 0x0022E747
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x0022F766 File Offset: 0x0022E766
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0022F77F File Offset: 0x0022E77F
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x00109403 File Offset: 0x00108403
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x0022F793 File Offset: 0x0022E793
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x0022F7AD File Offset: 0x0022E7AD
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x0022F7C7 File Offset: 0x0022E7C7
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x0022F7E6 File Offset: 0x0022E7E6
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x0022F7FA File Offset: 0x0022E7FA
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x0600492D RID: 18733 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x0600492E RID: 18734 RVA: 0x0022F80E File Offset: 0x0022E80E
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x0600492F RID: 18735 RVA: 0x0022F816 File Offset: 0x0022E816
		ITextPointer ITextContainer.End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06004930 RID: 18736 RVA: 0x00105F35 File Offset: 0x00104F35
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06004931 RID: 18737 RVA: 0x0022F81E File Offset: 0x0022E81E
		Highlights ITextContainer.Highlights
		{
			get
			{
				return this.Highlights;
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06004932 RID: 18738 RVA: 0x0022F826 File Offset: 0x0022E826
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06004933 RID: 18739 RVA: 0x0022F82E File Offset: 0x0022E82E
		// (set) Token: 0x06004934 RID: 18740 RVA: 0x0022F836 File Offset: 0x0022E836
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return this.TextSelection;
			}
			set
			{
				this._textSelection = value;
			}
		}

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06004935 RID: 18741 RVA: 0x00109403 File Offset: 0x00108403
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06004936 RID: 18742 RVA: 0x0022F83F File Offset: 0x0022E83F
		// (set) Token: 0x06004937 RID: 18743 RVA: 0x0022F847 File Offset: 0x0022E847
		ITextView ITextContainer.TextView
		{
			get
			{
				return this._textview;
			}
			set
			{
				this._textview = value;
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x0022F850 File Offset: 0x0022E850
		int ITextContainer.SymbolCount
		{
			get
			{
				return ((ITextContainer)this).Start.GetOffsetToPosition(((ITextContainer)this).End);
			}
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06004939 RID: 18745 RVA: 0x001056E1 File Offset: 0x001046E1
		int ITextContainer.IMECharCount
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x0600493A RID: 18746 RVA: 0x0022F864 File Offset: 0x0022E864
		// (remove) Token: 0x0600493B RID: 18747 RVA: 0x0022F89C File Offset: 0x0022E89C
		public event EventHandler Changing;

		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x0600493C RID: 18748 RVA: 0x0022F8D4 File Offset: 0x0022E8D4
		// (remove) Token: 0x0600493D RID: 18749 RVA: 0x0022F90C File Offset: 0x0022E90C
		public event TextContainerChangeEventHandler Change;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x0600493E RID: 18750 RVA: 0x0022F944 File Offset: 0x0022E944
		// (remove) Token: 0x0600493F RID: 18751 RVA: 0x0022F97C File Offset: 0x0022E97C
		public event TextContainerChangedEventHandler Changed;

		// Token: 0x06004940 RID: 18752 RVA: 0x0022F9B4 File Offset: 0x0022E9B4
		internal DocumentSequenceTextPointer VerifyPosition(ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			if (position.TextContainer != this)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedContainer", new object[]
				{
					"position"
				}));
			}
			DocumentSequenceTextPointer documentSequenceTextPointer = position as DocumentSequenceTextPointer;
			if (documentSequenceTextPointer == null)
			{
				throw new ArgumentException(SR.Get("BadFixedTextPosition", new object[]
				{
					"position"
				}));
			}
			return documentSequenceTextPointer;
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x0022FA20 File Offset: 0x0022EA20
		internal DocumentSequenceTextPointer MapChildPositionToParent(ITextPointer tp)
		{
			for (ChildDocumentBlock childDocumentBlock = this._doclistHead; childDocumentBlock != null; childDocumentBlock = childDocumentBlock.NextBlock)
			{
				if (childDocumentBlock.ChildContainer == tp.TextContainer)
				{
					return new DocumentSequenceTextPointer(childDocumentBlock, tp);
				}
			}
			return null;
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x0022FA58 File Offset: 0x0022EA58
		internal ChildDocumentBlock FindChildBlock(DocumentReference docRef)
		{
			for (ChildDocumentBlock nextBlock = this._doclistHead.NextBlock; nextBlock != null; nextBlock = nextBlock.NextBlock)
			{
				if (nextBlock.DocRef == docRef)
				{
					return nextBlock;
				}
			}
			return null;
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x0022FA8C File Offset: 0x0022EA8C
		internal int GetChildBlockDistance(ChildDocumentBlock block1, ChildDocumentBlock block2)
		{
			if (block1 == block2)
			{
				return 0;
			}
			int num = 0;
			for (ChildDocumentBlock childDocumentBlock = block1; childDocumentBlock != null; childDocumentBlock = childDocumentBlock.NextBlock)
			{
				if (childDocumentBlock == block2)
				{
					return num;
				}
				num++;
			}
			num = 0;
			for (ChildDocumentBlock childDocumentBlock = block1; childDocumentBlock != null; childDocumentBlock = childDocumentBlock.PreviousBlock)
			{
				if (childDocumentBlock == block2)
				{
					return num;
				}
				num--;
			}
			return 0;
		}

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06004944 RID: 18756 RVA: 0x0022FAD4 File Offset: 0x0022EAD4
		internal Highlights Highlights
		{
			get
			{
				if (this._highlights == null)
				{
					this._highlights = new DocumentSequenceTextContainer.DocumentSequenceHighlights(this);
				}
				return this._highlights;
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06004945 RID: 18757 RVA: 0x0022FAF0 File Offset: 0x0022EAF0
		internal ITextSelection TextSelection
		{
			get
			{
				return this._textSelection;
			}
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x0022FAF8 File Offset: 0x0022EAF8
		private void _Initialize()
		{
			this._doclistHead = new ChildDocumentBlock(this, new NullTextContainer());
			this._doclistTail = new ChildDocumentBlock(this, new NullTextContainer());
			this._doclistHead.InsertNextBlock(this._doclistTail);
			ChildDocumentBlock childDocumentBlock = this._doclistHead;
			foreach (DocumentReference docRef in this._parent.References)
			{
				childDocumentBlock.InsertNextBlock(new ChildDocumentBlock(this, docRef));
				childDocumentBlock = childDocumentBlock.NextBlock;
			}
			if (this._parent.References.Count != 0)
			{
				this._start = new DocumentSequenceTextPointer(this._doclistHead.NextBlock, this._doclistHead.NextBlock.ChildContainer.Start);
				this._end = new DocumentSequenceTextPointer(this._doclistTail.PreviousBlock, this._doclistTail.PreviousBlock.ChildContainer.End);
			}
			else
			{
				this._start = new DocumentSequenceTextPointer(this._doclistHead, this._doclistHead.ChildContainer.Start);
				this._end = new DocumentSequenceTextPointer(this._doclistTail, this._doclistTail.ChildContainer.End);
			}
			this._parent.References.CollectionChanged += this._OnContentChanged;
			this.Highlights.Changed += this._OnHighlightChanged;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x0022FC74 File Offset: 0x0022EC74
		private void AddChange(ITextPointer startPosition, int symbolCount, PrecursorTextChangeType precursorTextChange)
		{
			Invariant.Assert(!this._isReadOnly, "Illegal to modify DocumentSequenceTextContainer inside Change event scope!");
			((ITextContainer)this).BeginChange();
			try
			{
				if (this.Changing != null)
				{
					this.Changing(this, EventArgs.Empty);
				}
				if (this._changes == null)
				{
					this._changes = new TextContainerChangedEventArgs();
				}
				this._changes.AddChange(precursorTextChange, DocumentSequenceTextPointer.GetOffsetToPosition(this._start, startPosition), symbolCount, false);
				if (this.Change != null)
				{
					Invariant.Assert(precursorTextChange == PrecursorTextChangeType.ContentAdded || precursorTextChange == PrecursorTextChangeType.ContentRemoved);
					TextChangeType textChange = (precursorTextChange == PrecursorTextChangeType.ContentAdded) ? TextChangeType.ContentAdded : TextChangeType.ContentRemoved;
					this._isReadOnly = true;
					try
					{
						this.Change(this, new TextContainerChangeEventArgs(startPosition, symbolCount, -1, textChange));
					}
					finally
					{
						this._isReadOnly = false;
					}
				}
			}
			finally
			{
				((ITextContainer)this).EndChange();
			}
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x0022FD4C File Offset: 0x0022ED4C
		private void _OnContentChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.Action != NotifyCollectionChangedAction.Add)
			{
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			if (args.NewItems.Count != 1)
			{
				throw new NotSupportedException(SR.Get("RangeActionsNotSupported"));
			}
			object obj = args.NewItems[0];
			if (args.NewStartingIndex != this._parent.References.Count - 1)
			{
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					args.Action
				}));
			}
			ChildDocumentBlock childDocumentBlock = new ChildDocumentBlock(this, (DocumentReference)obj);
			ChildDocumentBlock previousBlock = this._doclistTail.PreviousBlock;
			previousBlock.InsertNextBlock(childDocumentBlock);
			DocumentSequenceTextPointer startPosition = new DocumentSequenceTextPointer(previousBlock, previousBlock.End);
			this._end = new DocumentSequenceTextPointer(childDocumentBlock, childDocumentBlock.ChildContainer.End);
			if (childDocumentBlock.NextBlock == this._doclistTail && childDocumentBlock.PreviousBlock == this._doclistHead)
			{
				this._start = new DocumentSequenceTextPointer(childDocumentBlock, childDocumentBlock.ChildContainer.Start);
			}
			ITextContainer childContainer = childDocumentBlock.ChildContainer;
			int symbolCount = 1;
			this.AddChange(startPosition, symbolCount, PrecursorTextChangeType.ContentAdded);
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x0022FE7C File Offset: 0x0022EE7C
		private void _OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			int i = 0;
			DocumentSequenceTextPointer documentSequenceTextPointer = null;
			ChildDocumentBlock childDocumentBlock = null;
			List<TextSegment> list = new List<TextSegment>(4);
			while (i < args.Ranges.Count)
			{
				TextSegment textSegment = (TextSegment)args.Ranges[i];
				DocumentSequenceTextPointer documentSequenceTextPointer2 = (DocumentSequenceTextPointer)textSegment.End;
				if (documentSequenceTextPointer == null)
				{
					documentSequenceTextPointer = (DocumentSequenceTextPointer)textSegment.Start;
				}
				ChildDocumentBlock childDocumentBlock2 = childDocumentBlock;
				childDocumentBlock = documentSequenceTextPointer.ChildBlock;
				if (childDocumentBlock2 != null && childDocumentBlock != childDocumentBlock2 && !(childDocumentBlock2.ChildContainer is NullTextContainer) && list.Count != 0)
				{
					childDocumentBlock2.ChildHighlightLayer.RaiseHighlightChangedEvent(new ReadOnlyCollection<TextSegment>(list));
					list.Clear();
				}
				ITextPointer childPointer = documentSequenceTextPointer.ChildPointer;
				if (documentSequenceTextPointer2.ChildBlock != childDocumentBlock)
				{
					ITextPointer textPointer = documentSequenceTextPointer.ChildPointer.TextContainer.End;
					if (childPointer.CompareTo(textPointer) != 0)
					{
						list.Add(new TextSegment(childPointer, textPointer));
					}
					if (!(childDocumentBlock.ChildContainer is NullTextContainer) && list.Count != 0)
					{
						childDocumentBlock.ChildHighlightLayer.RaiseHighlightChangedEvent(new ReadOnlyCollection<TextSegment>(list));
					}
					childDocumentBlock = childDocumentBlock.NextBlock;
					documentSequenceTextPointer = new DocumentSequenceTextPointer(childDocumentBlock, childDocumentBlock.ChildContainer.Start);
					list.Clear();
				}
				else
				{
					ITextPointer textPointer = documentSequenceTextPointer2.ChildPointer;
					if (childPointer.CompareTo(textPointer) != 0)
					{
						list.Add(new TextSegment(childPointer, textPointer));
					}
					i++;
					documentSequenceTextPointer = null;
				}
			}
			if (list.Count > 0 && childDocumentBlock != null && !(childDocumentBlock.ChildContainer is NullTextContainer))
			{
				childDocumentBlock.ChildHighlightLayer.RaiseHighlightChangedEvent(new ReadOnlyCollection<TextSegment>(list));
			}
		}

		// Token: 0x04002666 RID: 9830
		private readonly FixedDocumentSequence _parent;

		// Token: 0x04002667 RID: 9831
		private DocumentSequenceTextPointer _start;

		// Token: 0x04002668 RID: 9832
		private DocumentSequenceTextPointer _end;

		// Token: 0x04002669 RID: 9833
		private ChildDocumentBlock _doclistHead;

		// Token: 0x0400266A RID: 9834
		private ChildDocumentBlock _doclistTail;

		// Token: 0x0400266B RID: 9835
		private ITextSelection _textSelection;

		// Token: 0x0400266C RID: 9836
		private Highlights _highlights;

		// Token: 0x0400266D RID: 9837
		private int _changeBlockLevel;

		// Token: 0x0400266E RID: 9838
		private TextContainerChangedEventArgs _changes;

		// Token: 0x0400266F RID: 9839
		private ITextView _textview;

		// Token: 0x04002670 RID: 9840
		private bool _isReadOnly;

		// Token: 0x02000B31 RID: 2865
		private sealed class DocumentSequenceHighlights : Highlights
		{
			// Token: 0x06008C98 RID: 35992 RVA: 0x0033D1D0 File Offset: 0x0033C1D0
			internal DocumentSequenceHighlights(DocumentSequenceTextContainer textContainer) : base(textContainer)
			{
			}

			// Token: 0x06008C99 RID: 35993 RVA: 0x0033D1DC File Offset: 0x0033C1DC
			internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction, Type highlightLayerOwnerType)
			{
				StaticTextPointer textPosition2;
				if (this.EnsureParentPosition(textPosition, direction, out textPosition2))
				{
					return base.GetHighlightValue(textPosition2, direction, highlightLayerOwnerType);
				}
				return DependencyProperty.UnsetValue;
			}

			// Token: 0x06008C9A RID: 35994 RVA: 0x0033D204 File Offset: 0x0033C204
			internal override bool IsContentHighlighted(StaticTextPointer textPosition, LogicalDirection direction)
			{
				StaticTextPointer textPosition2;
				return this.EnsureParentPosition(textPosition, direction, out textPosition2) && base.IsContentHighlighted(textPosition2, direction);
			}

			// Token: 0x06008C9B RID: 35995 RVA: 0x0033D228 File Offset: 0x0033C228
			internal override StaticTextPointer GetNextHighlightChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
			{
				StaticTextPointer staticTextPointer = StaticTextPointer.Null;
				StaticTextPointer textPosition2;
				if (this.EnsureParentPosition(textPosition, direction, out textPosition2))
				{
					staticTextPointer = base.GetNextHighlightChangePosition(textPosition2, direction);
					if (textPosition.TextContainer.Highlights != this)
					{
						staticTextPointer = this.GetStaticPositionInChildContainer(staticTextPointer, direction, textPosition);
					}
				}
				return staticTextPointer;
			}

			// Token: 0x06008C9C RID: 35996 RVA: 0x0033D26C File Offset: 0x0033C26C
			internal override StaticTextPointer GetNextPropertyChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
			{
				StaticTextPointer staticTextPointer = StaticTextPointer.Null;
				StaticTextPointer textPosition2;
				if (this.EnsureParentPosition(textPosition, direction, out textPosition2))
				{
					staticTextPointer = base.GetNextPropertyChangePosition(textPosition2, direction);
					if (textPosition.TextContainer.Highlights != this)
					{
						staticTextPointer = this.GetStaticPositionInChildContainer(staticTextPointer, direction, textPosition);
					}
				}
				return staticTextPointer;
			}

			// Token: 0x06008C9D RID: 35997 RVA: 0x0033D2B0 File Offset: 0x0033C2B0
			private bool EnsureParentPosition(StaticTextPointer textPosition, LogicalDirection direction, out StaticTextPointer parentPosition)
			{
				parentPosition = textPosition;
				if (textPosition.TextContainer.Highlights != this)
				{
					if (textPosition.GetPointerContext(direction) == TextPointerContext.None)
					{
						return false;
					}
					ITextPointer tp = textPosition.CreateDynamicTextPointer(LogicalDirection.Forward);
					ITextPointer textPointer = ((DocumentSequenceTextContainer)base.TextContainer).MapChildPositionToParent(tp);
					parentPosition = textPointer.CreateStaticPointer();
				}
				return true;
			}

			// Token: 0x06008C9E RID: 35998 RVA: 0x0033D308 File Offset: 0x0033C308
			private StaticTextPointer GetStaticPositionInChildContainer(StaticTextPointer textPosition, LogicalDirection direction, StaticTextPointer originalPosition)
			{
				StaticTextPointer result = StaticTextPointer.Null;
				if (!textPosition.IsNull)
				{
					ITextPointer textPointer = (textPosition.CreateDynamicTextPointer(LogicalDirection.Forward) as DocumentSequenceTextPointer).ChildPointer;
					if (textPointer.TextContainer != originalPosition.TextContainer)
					{
						if (this.IsContentHighlighted(originalPosition, direction))
						{
							textPointer = ((direction == LogicalDirection.Forward) ? originalPosition.TextContainer.End : originalPosition.TextContainer.Start);
							result = textPointer.CreateStaticPointer();
						}
						else
						{
							result = StaticTextPointer.Null;
						}
					}
					else
					{
						result = textPointer.CreateStaticPointer();
					}
				}
				return result;
			}
		}
	}
}
