using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000648 RID: 1608
	internal sealed class NullTextPointer : ITextPointer
	{
		// Token: 0x06004F91 RID: 20369 RVA: 0x00244602 File Offset: 0x00243602
		internal NullTextPointer(NullTextContainer container, LogicalDirection gravity)
		{
			this._container = container;
			this._gravity = gravity;
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return 0;
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return 0;
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return 0;
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x00105F35 File Offset: 0x00104F35
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.None;
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			return 0;
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x00230052 File Offset: 0x0022F052
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return 0;
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x00109403 File Offset: 0x00108403
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00109403 File Offset: 0x00108403
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			return true;
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x00244618 File Offset: 0x00243618
		object ITextPointer.GetValue(DependencyProperty property)
		{
			return property.DefaultMetadata.DefaultValue;
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x00244625 File Offset: 0x00243625
		object ITextPointer.ReadLocalValue(DependencyProperty property)
		{
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x0024462C File Offset: 0x0024362C
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			return new DependencyObject().GetLocalValueEnumerator();
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00244638 File Offset: 0x00243638
		ITextPointer ITextPointer.CreatePointer()
		{
			return ((ITextPointer)this).CreatePointer(0, this._gravity);
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x002300A5 File Offset: 0x0022F0A5
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x00244647 File Offset: 0x00243647
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return ((ITextPointer)this).CreatePointer(distance, this._gravity);
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x0023B1AC File Offset: 0x0023A1AC
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return ((ITextPointer)this).CreatePointer(0, gravity);
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x00244656 File Offset: 0x00243656
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			return new NullTextPointer(this._container, gravity);
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x00244664 File Offset: 0x00243664
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x002300DD File Offset: 0x0022F0DD
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x0024466D File Offset: 0x0024366D
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "gravity");
			this._gravity = direction;
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			return false;
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.MoveByOffset(int distance)
		{
			return 0;
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return 0;
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x00244684 File Offset: 0x00243684
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return default(Rect);
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x0023030D File Offset: 0x0022F30D
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x00230316 File Offset: 0x0022F316
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextPointer.InsertTextInRun(string textData)
		{
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x002300F8 File Offset: 0x0022F0F8
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

		// Token: 0x06004FB2 RID: 20402 RVA: 0x00230120 File Offset: 0x0022F120
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x00230136 File Offset: 0x0022F136
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x0023014C File Offset: 0x0022F14C
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

		// Token: 0x06004FB5 RID: 20405 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITextPointer.ValidateLayout()
		{
			return false;
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x0024469A File Offset: 0x0024369A
		Type ITextPointer.ParentType
		{
			get
			{
				return typeof(FixedDocument);
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06004FB7 RID: 20407 RVA: 0x002446A6 File Offset: 0x002436A6
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06004FB8 RID: 20408 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x002446AE File Offset: 0x002436AE
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				Invariant.Assert(false, "NullTextPointer never has valid layout!");
				return false;
			}
		}

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x002446BC File Offset: 0x002436BC
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this._gravity;
			}
		}

		// Token: 0x1700127C RID: 4732
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x00230221 File Offset: 0x0022F221
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06004FBC RID: 20412 RVA: 0x002446C4 File Offset: 0x002436C4
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06004FBD RID: 20413 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextPointer.Offset
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700127F RID: 4735
		// (get) Token: 0x06004FBE RID: 20414 RVA: 0x001056E1 File Offset: 0x001046E1
		int ITextPointer.CharOffset
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0400285F RID: 10335
		private LogicalDirection _gravity;

		// Token: 0x04002860 RID: 10336
		private NullTextContainer _container;

		// Token: 0x04002861 RID: 10337
		private bool _isFrozen;
	}
}
