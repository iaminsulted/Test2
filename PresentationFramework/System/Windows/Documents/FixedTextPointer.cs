using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000618 RID: 1560
	internal class FixedTextPointer : ContentPosition, ITextPointer
	{
		// Token: 0x06004C1F RID: 19487 RVA: 0x0023AF0D File Offset: 0x00239F0D
		internal FixedTextPointer(bool mutable, LogicalDirection gravity, FlowPosition flow)
		{
			this._isFrozen = !mutable;
			this._gravity = gravity;
			this._flowPosition = flow;
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x0023AF30 File Offset: 0x00239F30
		internal int CompareTo(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			return this._flowPosition.CompareTo(fixedTextPointer.FlowPosition);
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x00230023 File Offset: 0x0022F023
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return ((ITextPointer)this).CompareTo((ITextPointer)position.Handle0);
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x0023AF5B File Offset: 0x00239F5B
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return this.CompareTo(position);
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x0023AF64 File Offset: 0x00239F64
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			return this._flowPosition.GetDistance(fixedTextPointer.FlowPosition);
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x0023AF8F File Offset: 0x00239F8F
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return this._flowPosition.GetPointerContext(direction);
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x0023AFA8 File Offset: 0x00239FA8
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			if (this._flowPosition.GetPointerContext(direction) != TextPointerContext.Text)
			{
				return 0;
			}
			return this._flowPosition.GetTextRunLength(direction);
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x00230052 File Offset: 0x0022F052
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x0023AFD4 File Offset: 0x00239FD4
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}
			if (count < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"count"
				}));
			}
			if (this._flowPosition.GetPointerContext(direction) != TextPointerContext.Text)
			{
				return 0;
			}
			return this._flowPosition.GetTextInRun(direction, count, textBuffer, startIndex);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x0023B040 File Offset: 0x0023A040
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			TextPointerContext pointerContext = this._flowPosition.GetPointerContext(direction);
			if (pointerContext != TextPointerContext.EmbeddedElement && pointerContext != TextPointerContext.ElementStart && pointerContext != TextPointerContext.ElementEnd)
			{
				return null;
			}
			return this._flowPosition.GetAdjacentElement(direction);
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x0023B080 File Offset: 0x0023A080
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			TextPointerContext pointerContext = this._flowPosition.GetPointerContext(direction);
			if (pointerContext != TextPointerContext.ElementStart && pointerContext != TextPointerContext.ElementEnd)
			{
				return null;
			}
			FixedElement element = this._flowPosition.GetElement(direction);
			if (!element.IsTextElement)
			{
				return null;
			}
			return element.Type;
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x0023B0CC File Offset: 0x0023A0CC
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			FixedElement scopingElement2 = fixedTextPointer.FlowPosition.GetScopingElement();
			return scopingElement == scopingElement2;
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x0023B100 File Offset: 0x0023A100
		object ITextPointer.GetValue(DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			return this._flowPosition.GetScopingElement().GetValue(property);
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x0023B121 File Offset: 0x0023A121
		object ITextPointer.ReadLocalValue(DependencyProperty property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			if (!scopingElement.IsTextElement)
			{
				throw new InvalidOperationException(SR.Get("NoElementObject"));
			}
			return scopingElement.ReadLocalValue(property);
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x0023B15C File Offset: 0x0023A15C
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			if (!scopingElement.IsTextElement)
			{
				return new DependencyObject().GetLocalValueEnumerator();
			}
			return scopingElement.GetLocalValueEnumerator();
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x0023B18E File Offset: 0x0023A18E
		ITextPointer ITextPointer.CreatePointer()
		{
			return ((ITextPointer)this).CreatePointer(0, ((ITextPointer)this).LogicalDirection);
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x002300A5 File Offset: 0x0022F0A5
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x0023B19D File Offset: 0x0023A19D
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return ((ITextPointer)this).CreatePointer(distance, ((ITextPointer)this).LogicalDirection);
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x0023B1AC File Offset: 0x0023A1AC
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return ((ITextPointer)this).CreatePointer(0, gravity);
		}

		// Token: 0x06004C32 RID: 19506 RVA: 0x0023B1B8 File Offset: 0x0023A1B8
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			ValidationHelper.VerifyDirection(gravity, "gravity");
			FlowPosition flowPosition = (FlowPosition)this._flowPosition.Clone();
			if (!flowPosition.Move(distance))
			{
				throw new ArgumentException(SR.Get("BadDistance"), "distance");
			}
			return new FixedTextPointer(true, gravity, flowPosition);
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x0023B207 File Offset: 0x0023A207
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x002300DD File Offset: 0x0022F0DD
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x002300F8 File Offset: 0x0022F0F8
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

		// Token: 0x06004C36 RID: 19510 RVA: 0x00230120 File Offset: 0x0022F120
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x00230136 File Offset: 0x0022F136
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x0023014C File Offset: 0x0022F14C
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

		// Token: 0x06004C39 RID: 19513 RVA: 0x0023B210 File Offset: 0x0023A210
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			this.LogicalDirection = direction;
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x0023B219 File Offset: 0x0023A219
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return this._flowPosition.Move(direction);
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x0023B232 File Offset: 0x0023A232
		int ITextPointer.MoveByOffset(int offset)
		{
			if (this._isFrozen)
			{
				throw new InvalidOperationException(SR.Get("TextPositionIsFrozen"));
			}
			if (!this._flowPosition.Move(offset))
			{
				throw new ArgumentException(SR.Get("BadDistance"), "offset");
			}
			return offset;
		}

		// Token: 0x06004C3C RID: 19516 RVA: 0x0023B270 File Offset: 0x0023A270
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			FixedTextPointer fixedTextPointer = this.FixedTextContainer.VerifyPosition(position);
			this._flowPosition.MoveTo(fixedTextPointer.FlowPosition);
		}

		// Token: 0x06004C3D RID: 19517 RVA: 0x0023B29C File Offset: 0x0023A29C
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
			ValidationHelper.VerifyElementEdge(edge, "edge");
			FixedElement scopingElement = this._flowPosition.GetScopingElement();
			if (!scopingElement.IsTextElement)
			{
				throw new InvalidOperationException(SR.Get("NoElementObject"));
			}
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				this._flowPosition = (FlowPosition)scopingElement.Start.FlowPosition.Clone();
				this._flowPosition.Move(-1);
				return;
			case ElementEdge.AfterStart:
				this._flowPosition = (FlowPosition)scopingElement.Start.FlowPosition.Clone();
				return;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				this._flowPosition = (FlowPosition)scopingElement.End.FlowPosition.Clone();
				return;
			default:
				if (edge != ElementEdge.AfterEnd)
				{
					return;
				}
				this._flowPosition = (FlowPosition)scopingElement.End.FlowPosition.Clone();
				this._flowPosition.Move(1);
				break;
			}
		}

		// Token: 0x06004C3E RID: 19518 RVA: 0x002302EF File Offset: 0x0022F2EF
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return TextPointerBase.MoveToLineBoundary(this, ((ITextPointer)this).TextContainer.TextView, count, true);
		}

		// Token: 0x06004C3F RID: 19519 RVA: 0x00230304 File Offset: 0x0022F304
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x06004C40 RID: 19520 RVA: 0x0023030D File Offset: 0x0022F30D
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06004C41 RID: 19521 RVA: 0x00230316 File Offset: 0x0022F316
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x06004C42 RID: 19522 RVA: 0x0023B37E File Offset: 0x0023A37E
		void ITextPointer.InsertTextInRun(string textData)
		{
			if (textData == null)
			{
				throw new ArgumentNullException("textData");
			}
			throw new InvalidOperationException(SR.Get("FixedDocumentReadonly"));
		}

		// Token: 0x06004C43 RID: 19523 RVA: 0x0023B39D File Offset: 0x0023A39D
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			throw new InvalidOperationException(SR.Get("FixedDocumentReadonly"));
		}

		// Token: 0x06004C44 RID: 19524 RVA: 0x00230174 File Offset: 0x0022F174
		bool ITextPointer.ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, ((ITextPointer)this).TextContainer.TextView);
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06004C45 RID: 19525 RVA: 0x0023B3B0 File Offset: 0x0023A3B0
		Type ITextPointer.ParentType
		{
			get
			{
				FixedElement scopingElement = this._flowPosition.GetScopingElement();
				if (!scopingElement.IsTextElement)
				{
					return ((ITextContainer)this._flowPosition.TextContainer).Parent.GetType();
				}
				return scopingElement.Type;
			}
		}

		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06004C46 RID: 19526 RVA: 0x0023B3ED File Offset: 0x0023A3ED
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this.FixedTextContainer;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06004C47 RID: 19527 RVA: 0x00230197 File Offset: 0x0022F197
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return ((ITextPointer)this).TextContainer.TextView != null && ((ITextPointer)this).TextContainer.TextView.IsValid && ((ITextPointer)this).TextContainer.TextView.Contains(this);
			}
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x06004C48 RID: 19528 RVA: 0x0023B3F8 File Offset: 0x0023A3F8
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				Invariant.Assert(((ITextPointer)this).HasValidLayout);
				ITextView textView = ((ITextPointer)this).TextContainer.TextView;
				bool flag = textView.IsAtCaretUnitBoundary(this);
				if (!flag && this.LogicalDirection == LogicalDirection.Backward)
				{
					ITextPointer position = ((ITextPointer)this).CreatePointer(LogicalDirection.Forward);
					flag = textView.IsAtCaretUnitBoundary(position);
				}
				return flag;
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x06004C49 RID: 19529 RVA: 0x0023B440 File Offset: 0x0023A440
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this.LogicalDirection;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06004C4A RID: 19530 RVA: 0x00230221 File Offset: 0x0022F221
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06004C4B RID: 19531 RVA: 0x0023B448 File Offset: 0x0023A448
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x00230231 File Offset: 0x0022F231
		int ITextPointer.Offset
		{
			get
			{
				return TextPointerBase.GetOffset(this);
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x001056E1 File Offset: 0x001046E1
		int ITextPointer.CharOffset
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x0023B450 File Offset: 0x0023A450
		internal FlowPosition FlowPosition
		{
			get
			{
				return this._flowPosition;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06004C4F RID: 19535 RVA: 0x0023B458 File Offset: 0x0023A458
		internal FixedTextContainer FixedTextContainer
		{
			get
			{
				return this._flowPosition.TextContainer;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06004C50 RID: 19536 RVA: 0x0023B465 File Offset: 0x0023A465
		// (set) Token: 0x06004C51 RID: 19537 RVA: 0x0023B46D File Offset: 0x0023A46D
		internal LogicalDirection LogicalDirection
		{
			get
			{
				return this._gravity;
			}
			set
			{
				ValidationHelper.VerifyDirection(value, "value");
				this._flowPosition = this._flowPosition.GetClingPosition(value);
				this._gravity = value;
			}
		}

		// Token: 0x040027C1 RID: 10177
		private LogicalDirection _gravity;

		// Token: 0x040027C2 RID: 10178
		private FlowPosition _flowPosition;

		// Token: 0x040027C3 RID: 10179
		private bool _isFrozen;
	}
}
