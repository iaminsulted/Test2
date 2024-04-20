using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020005EA RID: 1514
	internal sealed class DocumentSequenceTextPointer : ContentPosition, ITextPointer
	{
		// Token: 0x0600494A RID: 18762 RVA: 0x0022FFF6 File Offset: 0x0022EFF6
		internal DocumentSequenceTextPointer(ChildDocumentBlock childBlock, ITextPointer childPosition)
		{
			this._childBlock = childBlock;
			this._childTp = childPosition;
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x0023000C File Offset: 0x0022F00C
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			this._childTp.SetLogicalDirection(direction);
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x0023001A File Offset: 0x0022F01A
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return DocumentSequenceTextPointer.CompareTo(this, position);
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x00230023 File Offset: 0x0022F023
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return ((ITextPointer)this).CompareTo((ITextPointer)position.Handle0);
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x00230037 File Offset: 0x0022F037
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return DocumentSequenceTextPointer.GetOffsetToPosition(this, position);
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x00230040 File Offset: 0x0022F040
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetPointerContext(this, direction);
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x00230049 File Offset: 0x0022F049
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetTextRunLength(this, direction);
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x00230052 File Offset: 0x0022F052
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x0023005B File Offset: 0x0022F05B
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return DocumentSequenceTextPointer.GetTextInRun(this, direction, textBuffer, startIndex, count);
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x00230068 File Offset: 0x0022F068
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetAdjacentElement(this, direction);
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00230071 File Offset: 0x0022F071
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.GetElementType(this, direction);
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x0023007A File Offset: 0x0022F07A
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			return DocumentSequenceTextPointer.HasEqualScope(this, position);
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x00230083 File Offset: 0x0022F083
		object ITextPointer.GetValue(DependencyProperty property)
		{
			return DocumentSequenceTextPointer.GetValue(this, property);
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x0023008C File Offset: 0x0022F08C
		object ITextPointer.ReadLocalValue(DependencyProperty property)
		{
			return DocumentSequenceTextPointer.ReadLocalValue(this, property);
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x00230095 File Offset: 0x0022F095
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			return DocumentSequenceTextPointer.GetLocalValueEnumerator(this);
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x0023009D File Offset: 0x0022F09D
		ITextPointer ITextPointer.CreatePointer()
		{
			return DocumentSequenceTextPointer.CreatePointer(this);
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x002300A5 File Offset: 0x0022F0A5
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x002300B8 File Offset: 0x0022F0B8
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, distance);
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x002300C1 File Offset: 0x0022F0C1
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, gravity);
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x002300CA File Offset: 0x0022F0CA
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(this, distance, gravity);
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x002300D4 File Offset: 0x0022F0D4
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x002300DD File Offset: 0x0022F0DD
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x002300E6 File Offset: 0x0022F0E6
		void ITextPointer.InsertTextInRun(string textData)
		{
			throw new InvalidOperationException(SR.Get("DocumentReadOnly"));
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x002300E6 File Offset: 0x0022F0E6
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			throw new InvalidOperationException(SR.Get("DocumentReadOnly"));
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x002300F8 File Offset: 0x0022F0F8
		ITextPointer ITextPointer.GetNextContextPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextContextPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x00230120 File Offset: 0x0022F120
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x00230136 File Offset: 0x0022F136
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x0023014C File Offset: 0x0022F14C
		ITextPointer ITextPointer.GetNextInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextInsertionPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00230174 File Offset: 0x0022F174
		bool ITextPointer.ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, ((ITextPointer)this).TextContainer.TextView);
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06004967 RID: 18791 RVA: 0x00230187 File Offset: 0x0022F187
		Type ITextPointer.ParentType
		{
			get
			{
				return DocumentSequenceTextPointer.GetElementType(this);
			}
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06004968 RID: 18792 RVA: 0x0023018F File Offset: 0x0022F18F
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this.AggregatedContainer;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06004969 RID: 18793 RVA: 0x00230197 File Offset: 0x0022F197
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return ((ITextPointer)this).TextContainer.TextView != null && ((ITextPointer)this).TextContainer.TextView.IsValid && ((ITextPointer)this).TextContainer.TextView.Contains(this);
			}
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x0600496A RID: 18794 RVA: 0x002301CC File Offset: 0x0022F1CC
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				Invariant.Assert(((ITextPointer)this).HasValidLayout);
				ITextView textView = ((ITextPointer)this).TextContainer.TextView;
				bool flag = textView.IsAtCaretUnitBoundary(this);
				if (!flag && ((ITextPointer)this).LogicalDirection == LogicalDirection.Backward)
				{
					ITextPointer position = ((ITextPointer)this).CreatePointer(LogicalDirection.Forward);
					flag = textView.IsAtCaretUnitBoundary(position);
				}
				return flag;
			}
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x0600496B RID: 18795 RVA: 0x00230214 File Offset: 0x0022F214
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this._childTp.LogicalDirection;
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x0600496C RID: 18796 RVA: 0x00230221 File Offset: 0x0022F221
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x0600496D RID: 18797 RVA: 0x00230229 File Offset: 0x0022F229
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x0600496E RID: 18798 RVA: 0x00230231 File Offset: 0x0022F231
		int ITextPointer.Offset
		{
			get
			{
				return TextPointerBase.GetOffset(this);
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x001056E1 File Offset: 0x001046E1
		int ITextPointer.CharOffset
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x00230239 File Offset: 0x0022F239
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.iScan(this, direction);
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x00230242 File Offset: 0x0022F242
		int ITextPointer.MoveByOffset(int offset)
		{
			if (this._isFrozen)
			{
				throw new InvalidOperationException(SR.Get("TextPositionIsFrozen"));
			}
			if (DocumentSequenceTextPointer.iScan(this, offset))
			{
				return offset;
			}
			return 0;
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x00230268 File Offset: 0x0022F268
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = this.AggregatedContainer.VerifyPosition(position);
			LogicalDirection logicalDirection = this.ChildPointer.LogicalDirection;
			this.ChildBlock = documentSequenceTextPointer.ChildBlock;
			if (this.ChildPointer.TextContainer == documentSequenceTextPointer.ChildPointer.TextContainer)
			{
				this.ChildPointer.MoveToPosition(documentSequenceTextPointer.ChildPointer);
				return;
			}
			this.ChildPointer = documentSequenceTextPointer.ChildPointer.CreatePointer();
			this.ChildPointer.SetLogicalDirection(logicalDirection);
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x002302E1 File Offset: 0x0022F2E1
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
			this.ChildPointer.MoveToElementEdge(edge);
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x002302EF File Offset: 0x0022F2EF
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return TextPointerBase.MoveToLineBoundary(this, ((ITextPointer)this).TextContainer.TextView, count, true);
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x00230304 File Offset: 0x0022F304
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x0023030D File Offset: 0x0022F30D
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00230316 File Offset: 0x0022F316
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x0023031F File Offset: 0x0022F31F
		internal DocumentSequenceTextContainer AggregatedContainer
		{
			get
			{
				return this._childBlock.AggregatedContainer;
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06004979 RID: 18809 RVA: 0x0023032C File Offset: 0x0022F32C
		// (set) Token: 0x0600497A RID: 18810 RVA: 0x00230334 File Offset: 0x0022F334
		internal ChildDocumentBlock ChildBlock
		{
			get
			{
				return this._childBlock;
			}
			set
			{
				this._childBlock = value;
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x0600497B RID: 18811 RVA: 0x0023033D File Offset: 0x0022F33D
		// (set) Token: 0x0600497C RID: 18812 RVA: 0x00230345 File Offset: 0x0022F345
		internal ITextPointer ChildPointer
		{
			get
			{
				return this._childTp;
			}
			set
			{
				this._childTp = value;
			}
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00230350 File Offset: 0x0022F350
		public static int CompareTo(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer tp = thisTp.AggregatedContainer.VerifyPosition(position);
			return DocumentSequenceTextPointer.xGapAwareCompareTo(thisTp, tp);
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00230374 File Offset: 0x0022F374
		public static int GetOffsetToPosition(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = thisTp.AggregatedContainer.VerifyPosition(position);
			int num = DocumentSequenceTextPointer.xGapAwareCompareTo(thisTp, documentSequenceTextPointer);
			if (num == 0)
			{
				return 0;
			}
			if (num <= 0)
			{
				return DocumentSequenceTextPointer.xGapAwareGetDistance(thisTp, documentSequenceTextPointer);
			}
			return -1 * DocumentSequenceTextPointer.xGapAwareGetDistance(documentSequenceTextPointer, thisTp);
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x002303B0 File Offset: 0x0022F3B0
		public static TextPointerContext GetPointerContext(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return DocumentSequenceTextPointer.xGapAwareGetSymbolType(thisTp, direction);
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x002303C4 File Offset: 0x0022F3C4
		public static int GetTextRunLength(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return thisTp.ChildPointer.GetTextRunLength(direction);
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x002303E0 File Offset: 0x0022F3E0
		public static int GetTextInRun(DocumentSequenceTextPointer thisTp, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}
			if (startIndex < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"startIndex"
				}));
			}
			if (startIndex > textBuffer.Length)
			{
				throw new ArgumentException(SR.Get("StartIndexExceedsBufferSize", new object[]
				{
					startIndex,
					textBuffer.Length
				}));
			}
			if (count < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"count"
				}));
			}
			if (count > textBuffer.Length - startIndex)
			{
				throw new ArgumentException(SR.Get("MaxLengthExceedsBufferSize", new object[]
				{
					count,
					textBuffer.Length,
					startIndex
				}));
			}
			return thisTp.ChildPointer.GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x002304C8 File Offset: 0x0022F4C8
		public static object GetAdjacentElement(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return DocumentSequenceTextPointer.xGapAwareGetEmbeddedElement(thisTp, direction);
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x002304DC File Offset: 0x0022F4DC
		public static Type GetElementType(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction).ChildPointer.GetElementType(direction);
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x002304FB File Offset: 0x0022F4FB
		public static Type GetElementType(DocumentSequenceTextPointer thisTp)
		{
			return thisTp.ChildPointer.ParentType;
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00230508 File Offset: 0x0022F508
		public static bool HasEqualScope(DocumentSequenceTextPointer thisTp, ITextPointer position)
		{
			DocumentSequenceTextPointer documentSequenceTextPointer = thisTp.AggregatedContainer.VerifyPosition(position);
			if (thisTp.ChildPointer.TextContainer == documentSequenceTextPointer.ChildPointer.TextContainer)
			{
				return thisTp.ChildPointer.HasEqualScope(documentSequenceTextPointer.ChildPointer);
			}
			return thisTp.ChildPointer.ParentType == typeof(FixedDocument) && documentSequenceTextPointer.ChildPointer.ParentType == typeof(FixedDocument);
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00230584 File Offset: 0x0022F584
		public static object GetValue(DocumentSequenceTextPointer thisTp, DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return thisTp.ChildPointer.GetValue(property);
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x002305A0 File Offset: 0x0022F5A0
		public static object ReadLocalValue(DocumentSequenceTextPointer thisTp, DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return thisTp.ChildPointer.ReadLocalValue(property);
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x002305BC File Offset: 0x0022F5BC
		public static LocalValueEnumerator GetLocalValueEnumerator(DocumentSequenceTextPointer thisTp)
		{
			return thisTp.ChildPointer.GetLocalValueEnumerator();
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x002305C9 File Offset: 0x0022F5C9
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, 0, thisTp.ChildPointer.LogicalDirection);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x002305DD File Offset: 0x0022F5DD
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, int distance)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, distance, thisTp.ChildPointer.LogicalDirection);
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x002305F1 File Offset: 0x0022F5F1
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, LogicalDirection gravity)
		{
			return DocumentSequenceTextPointer.CreatePointer(thisTp, 0, gravity);
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x002305FC File Offset: 0x0022F5FC
		public static ITextPointer CreatePointer(DocumentSequenceTextPointer thisTp, int distance, LogicalDirection gravity)
		{
			ValidationHelper.VerifyDirection(gravity, "gravity");
			DocumentSequenceTextPointer documentSequenceTextPointer = new DocumentSequenceTextPointer(thisTp.ChildBlock, thisTp.ChildPointer.CreatePointer(gravity));
			if (distance != 0 && !DocumentSequenceTextPointer.xGapAwareScan(documentSequenceTextPointer, distance))
			{
				throw new ArgumentException(SR.Get("BadDistance"), "distance");
			}
			return documentSequenceTextPointer;
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00230650 File Offset: 0x0022F650
		internal static bool iScan(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			bool flag = thisTp.ChildPointer.MoveToNextContextPosition(direction);
			if (!flag)
			{
				flag = DocumentSequenceTextPointer.xGapAwareScan(thisTp, (direction == LogicalDirection.Forward) ? 1 : -1);
			}
			return flag;
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x0023067D File Offset: 0x0022F67D
		internal static bool iScan(DocumentSequenceTextPointer thisTp, int distance)
		{
			return DocumentSequenceTextPointer.xGapAwareScan(thisTp, distance);
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00230688 File Offset: 0x0022F688
		private static DocumentSequenceTextPointer xGetClingDSTP(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			TextPointerContext pointerContext = thisTp.ChildPointer.GetPointerContext(direction);
			if (pointerContext != TextPointerContext.None)
			{
				return thisTp;
			}
			ChildDocumentBlock childDocumentBlock = thisTp.ChildBlock;
			ITextPointer textPointer = thisTp.ChildPointer;
			if (direction == LogicalDirection.Forward)
			{
				while (pointerContext == TextPointerContext.None)
				{
					if (childDocumentBlock.IsTail)
					{
						break;
					}
					childDocumentBlock = childDocumentBlock.NextBlock;
					textPointer = childDocumentBlock.ChildContainer.Start;
					pointerContext = textPointer.GetPointerContext(direction);
				}
			}
			else
			{
				while (pointerContext == TextPointerContext.None && !childDocumentBlock.IsHead)
				{
					childDocumentBlock = childDocumentBlock.PreviousBlock;
					textPointer = childDocumentBlock.ChildContainer.End;
					pointerContext = textPointer.GetPointerContext(direction);
				}
			}
			return new DocumentSequenceTextPointer(childDocumentBlock, textPointer);
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x00230710 File Offset: 0x0022F710
		private static TextPointerContext xGapAwareGetSymbolType(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction).ChildPointer.GetPointerContext(direction);
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00230724 File Offset: 0x0022F724
		private static object xGapAwareGetEmbeddedElement(DocumentSequenceTextPointer thisTp, LogicalDirection direction)
		{
			return DocumentSequenceTextPointer.xGetClingDSTP(thisTp, direction).ChildPointer.GetAdjacentElement(direction);
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00230738 File Offset: 0x0022F738
		private static int xGapAwareCompareTo(DocumentSequenceTextPointer thisTp, DocumentSequenceTextPointer tp)
		{
			if (thisTp == tp)
			{
				return 0;
			}
			ChildDocumentBlock childBlock = thisTp.ChildBlock;
			ChildDocumentBlock childBlock2 = tp.ChildBlock;
			int childBlockDistance = thisTp.AggregatedContainer.GetChildBlockDistance(childBlock, childBlock2);
			if (childBlockDistance == 0)
			{
				return thisTp.ChildPointer.CompareTo(tp.ChildPointer);
			}
			if (childBlockDistance < 0)
			{
				if (!DocumentSequenceTextPointer.xUnseparated(tp, thisTp))
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (!DocumentSequenceTextPointer.xUnseparated(thisTp, tp))
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x0023079C File Offset: 0x0022F79C
		private static bool xUnseparated(DocumentSequenceTextPointer tp1, DocumentSequenceTextPointer tp2)
		{
			if (tp1.ChildPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None || tp2.ChildPointer.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.None)
			{
				return false;
			}
			for (ChildDocumentBlock nextBlock = tp1.ChildBlock.NextBlock; nextBlock != tp2.ChildBlock; nextBlock = nextBlock.NextBlock)
			{
				if (nextBlock.ChildContainer.Start.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x002307FC File Offset: 0x0022F7FC
		private static int xGapAwareGetDistance(DocumentSequenceTextPointer tp1, DocumentSequenceTextPointer tp2)
		{
			if (tp1 == tp2)
			{
				return 0;
			}
			int num = 0;
			DocumentSequenceTextPointer documentSequenceTextPointer = new DocumentSequenceTextPointer(tp1.ChildBlock, tp1.ChildPointer);
			while (documentSequenceTextPointer.ChildBlock != tp2.ChildBlock)
			{
				num += documentSequenceTextPointer.ChildPointer.GetOffsetToPosition(documentSequenceTextPointer.ChildPointer.TextContainer.End);
				ChildDocumentBlock nextBlock = documentSequenceTextPointer.ChildBlock.NextBlock;
				documentSequenceTextPointer.ChildBlock = nextBlock;
				documentSequenceTextPointer.ChildPointer = nextBlock.ChildContainer.Start;
			}
			return num + documentSequenceTextPointer.ChildPointer.GetOffsetToPosition(tp2.ChildPointer);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x0023088C File Offset: 0x0022F88C
		private static bool xGapAwareScan(DocumentSequenceTextPointer thisTp, int distance)
		{
			ChildDocumentBlock childDocumentBlock = thisTp.ChildBlock;
			bool flag = true;
			ITextPointer textPointer = thisTp.ChildPointer;
			if (textPointer == null)
			{
				flag = false;
				textPointer = thisTp.ChildPointer.CreatePointer();
			}
			LogicalDirection logicalDirection = (distance > 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			distance = Math.Abs(distance);
			while (distance > 0)
			{
				switch (textPointer.GetPointerContext(logicalDirection))
				{
				case TextPointerContext.None:
					if ((childDocumentBlock.IsHead && logicalDirection == LogicalDirection.Backward) || (childDocumentBlock.IsTail && logicalDirection == LogicalDirection.Forward))
					{
						return false;
					}
					childDocumentBlock = ((logicalDirection == LogicalDirection.Forward) ? childDocumentBlock.NextBlock : childDocumentBlock.PreviousBlock);
					textPointer = ((logicalDirection == LogicalDirection.Forward) ? childDocumentBlock.ChildContainer.Start.CreatePointer(textPointer.LogicalDirection) : childDocumentBlock.ChildContainer.End.CreatePointer(textPointer.LogicalDirection));
					break;
				case TextPointerContext.Text:
				{
					int textRunLength = textPointer.GetTextRunLength(logicalDirection);
					int num = (textRunLength < distance) ? textRunLength : distance;
					distance -= num;
					if (logicalDirection == LogicalDirection.Backward)
					{
						num *= -1;
					}
					textPointer.MoveByOffset(num);
					break;
				}
				case TextPointerContext.EmbeddedElement:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				case TextPointerContext.ElementStart:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				case TextPointerContext.ElementEnd:
					textPointer.MoveToNextContextPosition(logicalDirection);
					distance--;
					break;
				}
			}
			thisTp.ChildBlock = childDocumentBlock;
			if (flag)
			{
				thisTp.ChildPointer = textPointer;
			}
			else
			{
				thisTp.ChildPointer = textPointer.CreatePointer();
			}
			return true;
		}

		// Token: 0x04002671 RID: 9841
		private ChildDocumentBlock _childBlock;

		// Token: 0x04002672 RID: 9842
		private ITextPointer _childTp;

		// Token: 0x04002673 RID: 9843
		private bool _isFrozen;
	}
}
