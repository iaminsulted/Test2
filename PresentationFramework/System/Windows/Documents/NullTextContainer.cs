using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000647 RID: 1607
	internal sealed class NullTextContainer : ITextContainer
	{
		// Token: 0x06004F6B RID: 20331 RVA: 0x002445BA File Offset: 0x002435BA
		internal NullTextContainer()
		{
			this._start = new NullTextPointer(this, LogicalDirection.Backward);
			this._end = new NullTextPointer(this, LogicalDirection.Forward);
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextContainer.BeginChange()
		{
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x0022F68B File Offset: 0x0022E68B
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x0022F693 File Offset: 0x0022E693
		void ITextContainer.EndChange()
		{
			((ITextContainer)this).EndChange(false);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextContainer.EndChange(bool skipEvents)
		{
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x0022F700 File Offset: 0x0022E700
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return ((ITextContainer)this).Start.CreatePointer(offset, direction);
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x001056E1 File Offset: 0x001046E1
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x0022F70F File Offset: 0x0022E70F
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x0022F723 File Offset: 0x0022E723
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x0022F733 File Offset: 0x0022E733
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x0022F747 File Offset: 0x0022E747
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x0022F766 File Offset: 0x0022E766
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x0022F77F File Offset: 0x0022E77F
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x00109403 File Offset: 0x00108403
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x0022F793 File Offset: 0x0022E793
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x0022F7AD File Offset: 0x0022E7AD
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x0022F7C7 File Offset: 0x0022E7C7
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x0022F7E6 File Offset: 0x0022E7E6
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x0022F7FA File Offset: 0x0022E7FA
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06004F7E RID: 20350 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06004F7F RID: 20351 RVA: 0x002445DC File Offset: 0x002435DC
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06004F80 RID: 20352 RVA: 0x002445E4 File Offset: 0x002435E4
		ITextPointer ITextContainer.End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06004F81 RID: 20353 RVA: 0x00105F35 File Offset: 0x00104F35
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06004F82 RID: 20354 RVA: 0x00109403 File Offset: 0x00108403
		Highlights ITextContainer.Highlights
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06004F83 RID: 20355 RVA: 0x00109403 File Offset: 0x00108403
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06004F84 RID: 20356 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06004F85 RID: 20357 RVA: 0x002445EC File Offset: 0x002435EC
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "NullTextContainer is never associated with a TextEditor/TextSelection!");
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06004F86 RID: 20358 RVA: 0x00109403 File Offset: 0x00108403
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06004F88 RID: 20360 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		ITextView ITextContainer.TextView
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06004F89 RID: 20361 RVA: 0x00105F35 File Offset: 0x00104F35
		int ITextContainer.SymbolCount
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06004F8A RID: 20362 RVA: 0x002445F9 File Offset: 0x002435F9
		int ITextContainer.IMECharCount
		{
			get
			{
				Invariant.Assert(false);
				return 0;
			}
		}

		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x06004F8B RID: 20363 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		// (remove) Token: 0x06004F8C RID: 20364 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public event EventHandler Changing
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x140000C4 RID: 196
		// (add) Token: 0x06004F8D RID: 20365 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		// (remove) Token: 0x06004F8E RID: 20366 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public event TextContainerChangeEventHandler Change
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x140000C5 RID: 197
		// (add) Token: 0x06004F8F RID: 20367 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		// (remove) Token: 0x06004F90 RID: 20368 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public event TextContainerChangedEventHandler Changed
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x0400285D RID: 10333
		private NullTextPointer _start;

		// Token: 0x0400285E RID: 10334
		private NullTextPointer _end;
	}
}
