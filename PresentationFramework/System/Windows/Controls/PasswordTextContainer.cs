using System;
using System.Collections;
using System.Security;
using System.Windows.Documents;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Controls
{
	// Token: 0x020007BC RID: 1980
	internal sealed class PasswordTextContainer : ITextContainer
	{
		// Token: 0x060070CA RID: 28874 RVA: 0x002D8B40 File Offset: 0x002D7B40
		internal PasswordTextContainer(PasswordBox passwordBox)
		{
			this._passwordBox = passwordBox;
			this._password = new SecureString();
		}

		// Token: 0x060070CB RID: 28875 RVA: 0x002D8B5C File Offset: 0x002D7B5C
		internal void InsertText(ITextPointer position, string textData)
		{
			this.BeginChange();
			try
			{
				int offset = ((PasswordTextPointer)position).Offset;
				for (int i = 0; i < textData.Length; i++)
				{
					this._password.InsertAt(offset + i, textData[i]);
				}
				this.OnPasswordChange(offset, textData.Length);
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x060070CC RID: 28876 RVA: 0x002D8BC8 File Offset: 0x002D7BC8
		internal void DeleteContent(ITextPointer startPosition, ITextPointer endPosition)
		{
			this.BeginChange();
			try
			{
				int offset = ((PasswordTextPointer)startPosition).Offset;
				int offset2 = ((PasswordTextPointer)endPosition).Offset;
				for (int i = 0; i < offset2 - offset; i++)
				{
					this._password.RemoveAt(offset);
				}
				this.OnPasswordChange(offset, offset - offset2);
			}
			finally
			{
				this.EndChange();
			}
		}

		// Token: 0x060070CD RID: 28877 RVA: 0x002D8C30 File Offset: 0x002D7C30
		internal void BeginChange()
		{
			this._changeBlockLevel++;
		}

		// Token: 0x060070CE RID: 28878 RVA: 0x002D8C40 File Offset: 0x002D7C40
		internal void EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x060070CF RID: 28879 RVA: 0x002D8C4C File Offset: 0x002D7C4C
		internal void EndChange(bool skipEvents)
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

		// Token: 0x060070D0 RID: 28880 RVA: 0x002D8CB0 File Offset: 0x002D7CB0
		void ITextContainer.BeginChange()
		{
			this.BeginChange();
		}

		// Token: 0x060070D1 RID: 28881 RVA: 0x0022F68B File Offset: 0x0022E68B
		void ITextContainer.BeginChangeNoUndo()
		{
			((ITextContainer)this).BeginChange();
		}

		// Token: 0x060070D2 RID: 28882 RVA: 0x002D8C40 File Offset: 0x002D7C40
		void ITextContainer.EndChange()
		{
			this.EndChange(false);
		}

		// Token: 0x060070D3 RID: 28883 RVA: 0x002D8CB8 File Offset: 0x002D7CB8
		void ITextContainer.EndChange(bool skipEvents)
		{
			this.EndChange(skipEvents);
		}

		// Token: 0x060070D4 RID: 28884 RVA: 0x002D8CC1 File Offset: 0x002D7CC1
		ITextPointer ITextContainer.CreatePointerAtOffset(int offset, LogicalDirection direction)
		{
			return new PasswordTextPointer(this, direction, offset);
		}

		// Token: 0x060070D5 RID: 28885 RVA: 0x00145B95 File Offset: 0x00144B95
		ITextPointer ITextContainer.CreatePointerAtCharOffset(int charOffset, LogicalDirection direction)
		{
			return ((ITextContainer)this).CreatePointerAtOffset(charOffset, direction);
		}

		// Token: 0x060070D6 RID: 28886 RVA: 0x0022F70F File Offset: 0x0022E70F
		ITextPointer ITextContainer.CreateDynamicTextPointer(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).CreatePointer(direction);
		}

		// Token: 0x060070D7 RID: 28887 RVA: 0x0022F723 File Offset: 0x0022E723
		StaticTextPointer ITextContainer.CreateStaticPointerAtOffset(int offset)
		{
			return new StaticTextPointer(this, ((ITextContainer)this).CreatePointerAtOffset(offset, LogicalDirection.Forward));
		}

		// Token: 0x060070D8 RID: 28888 RVA: 0x0022F733 File Offset: 0x0022E733
		TextPointerContext ITextContainer.GetPointerContext(StaticTextPointer pointer, LogicalDirection direction)
		{
			return ((ITextPointer)pointer.Handle0).GetPointerContext(direction);
		}

		// Token: 0x060070D9 RID: 28889 RVA: 0x0022F747 File Offset: 0x0022E747
		int ITextContainer.GetOffsetToPosition(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).GetOffsetToPosition((ITextPointer)position2.Handle0);
		}

		// Token: 0x060070DA RID: 28890 RVA: 0x0022F766 File Offset: 0x0022E766
		int ITextContainer.GetTextInRun(StaticTextPointer position, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return ((ITextPointer)position.Handle0).GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x060070DB RID: 28891 RVA: 0x0022F77F File Offset: 0x0022E77F
		object ITextContainer.GetAdjacentElement(StaticTextPointer position, LogicalDirection direction)
		{
			return ((ITextPointer)position.Handle0).GetAdjacentElement(direction);
		}

		// Token: 0x060070DC RID: 28892 RVA: 0x00109403 File Offset: 0x00108403
		DependencyObject ITextContainer.GetParent(StaticTextPointer position)
		{
			return null;
		}

		// Token: 0x060070DD RID: 28893 RVA: 0x0022F793 File Offset: 0x0022E793
		StaticTextPointer ITextContainer.CreatePointer(StaticTextPointer position, int offset)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).CreatePointer(offset));
		}

		// Token: 0x060070DE RID: 28894 RVA: 0x0022F7AD File Offset: 0x0022E7AD
		StaticTextPointer ITextContainer.GetNextContextPosition(StaticTextPointer position, LogicalDirection direction)
		{
			return new StaticTextPointer(this, ((ITextPointer)position.Handle0).GetNextContextPosition(direction));
		}

		// Token: 0x060070DF RID: 28895 RVA: 0x0022F7C7 File Offset: 0x0022E7C7
		int ITextContainer.CompareTo(StaticTextPointer position1, StaticTextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo((ITextPointer)position2.Handle0);
		}

		// Token: 0x060070E0 RID: 28896 RVA: 0x0022F7E6 File Offset: 0x0022E7E6
		int ITextContainer.CompareTo(StaticTextPointer position1, ITextPointer position2)
		{
			return ((ITextPointer)position1.Handle0).CompareTo(position2);
		}

		// Token: 0x060070E1 RID: 28897 RVA: 0x0022F7FA File Offset: 0x0022E7FA
		object ITextContainer.GetValue(StaticTextPointer position, DependencyProperty formattingProperty)
		{
			return ((ITextPointer)position.Handle0).GetValue(formattingProperty);
		}

		// Token: 0x060070E2 RID: 28898 RVA: 0x002D8CCC File Offset: 0x002D7CCC
		internal void AddPosition(PasswordTextPointer position)
		{
			this.RemoveUnreferencedPositions();
			if (this._positionList == null)
			{
				this._positionList = new ArrayList();
			}
			int index = this.FindIndex(position.Offset, position.LogicalDirection);
			this._positionList.Insert(index, new WeakReference(position));
			this.DebugAssertPositionList();
		}

		// Token: 0x060070E3 RID: 28899 RVA: 0x002D8D20 File Offset: 0x002D7D20
		internal void RemovePosition(PasswordTextPointer searchPosition)
		{
			Invariant.Assert(this._positionList != null);
			int i;
			for (i = 0; i < this._positionList.Count; i++)
			{
				if (this.GetPointerAtIndex(i) == searchPosition)
				{
					this._positionList.RemoveAt(i);
					i = -1;
					break;
				}
			}
			Invariant.Assert(i == -1, "Couldn't find position to remove!");
		}

		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x060070E4 RID: 28900 RVA: 0x00105F35 File Offset: 0x00104F35
		bool ITextContainer.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x060070E5 RID: 28901 RVA: 0x002D8D79 File Offset: 0x002D7D79
		ITextPointer ITextContainer.Start
		{
			get
			{
				return this.Start;
			}
		}

		// Token: 0x17001A16 RID: 6678
		// (get) Token: 0x060070E6 RID: 28902 RVA: 0x002D8D81 File Offset: 0x002D7D81
		ITextPointer ITextContainer.End
		{
			get
			{
				return this.End;
			}
		}

		// Token: 0x17001A17 RID: 6679
		// (get) Token: 0x060070E7 RID: 28903 RVA: 0x00105F35 File Offset: 0x00104F35
		uint ITextContainer.Generation
		{
			get
			{
				return 0U;
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x060070E8 RID: 28904 RVA: 0x002D8D89 File Offset: 0x002D7D89
		Highlights ITextContainer.Highlights
		{
			get
			{
				if (this._highlights == null)
				{
					this._highlights = new Highlights(this);
				}
				return this._highlights;
			}
		}

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x060070E9 RID: 28905 RVA: 0x002D8DA5 File Offset: 0x002D7DA5
		DependencyObject ITextContainer.Parent
		{
			get
			{
				return this._passwordBox;
			}
		}

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x060070EA RID: 28906 RVA: 0x002D8DAD File Offset: 0x002D7DAD
		// (set) Token: 0x060070EB RID: 28907 RVA: 0x002D8DB5 File Offset: 0x002D7DB5
		ITextSelection ITextContainer.TextSelection
		{
			get
			{
				return this._textSelection;
			}
			set
			{
				this._textSelection = value;
			}
		}

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x060070EC RID: 28908 RVA: 0x00109403 File Offset: 0x00108403
		UndoManager ITextContainer.UndoManager
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x060070ED RID: 28909 RVA: 0x002D8DBE File Offset: 0x002D7DBE
		// (set) Token: 0x060070EE RID: 28910 RVA: 0x002D8DC6 File Offset: 0x002D7DC6
		ITextView ITextContainer.TextView
		{
			get
			{
				return this.TextView;
			}
			set
			{
				this.TextView = value;
			}
		}

		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x060070EF RID: 28911 RVA: 0x002D8DCF File Offset: 0x002D7DCF
		// (set) Token: 0x060070F0 RID: 28912 RVA: 0x002D8DD7 File Offset: 0x002D7DD7
		internal ITextView TextView
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

		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x060070F1 RID: 28913 RVA: 0x002D8DE0 File Offset: 0x002D7DE0
		int ITextContainer.SymbolCount
		{
			get
			{
				return this.SymbolCount;
			}
		}

		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x060070F2 RID: 28914 RVA: 0x002D8DE0 File Offset: 0x002D7DE0
		int ITextContainer.IMECharCount
		{
			get
			{
				return this.SymbolCount;
			}
		}

		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x060070F3 RID: 28915 RVA: 0x002D8DE8 File Offset: 0x002D7DE8
		internal ITextPointer Start
		{
			get
			{
				return new PasswordTextPointer(this, LogicalDirection.Backward, 0);
			}
		}

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x060070F4 RID: 28916 RVA: 0x002D8DF2 File Offset: 0x002D7DF2
		internal ITextPointer End
		{
			get
			{
				return new PasswordTextPointer(this, LogicalDirection.Forward, this.SymbolCount);
			}
		}

		// Token: 0x060070F5 RID: 28917 RVA: 0x002D8E01 File Offset: 0x002D7E01
		internal SecureString GetPasswordCopy()
		{
			return this._password.Copy();
		}

		// Token: 0x060070F6 RID: 28918 RVA: 0x002D8E10 File Offset: 0x002D7E10
		internal void SetPassword(SecureString value)
		{
			int symbolCount = this.SymbolCount;
			this._password.Clear();
			this.OnPasswordChange(0, -symbolCount);
			this._password = ((value == null) ? new SecureString() : value.Copy());
			this.OnPasswordChange(0, this.SymbolCount);
		}

		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x060070F7 RID: 28919 RVA: 0x002D8E5B File Offset: 0x002D7E5B
		internal int SymbolCount
		{
			get
			{
				return this._password.Length;
			}
		}

		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x060070F8 RID: 28920 RVA: 0x002D8E68 File Offset: 0x002D7E68
		internal char PasswordChar
		{
			get
			{
				return this.PasswordBox.PasswordChar;
			}
		}

		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x060070F9 RID: 28921 RVA: 0x002D8DA5 File Offset: 0x002D7DA5
		internal PasswordBox PasswordBox
		{
			get
			{
				return this._passwordBox;
			}
		}

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x060070FA RID: 28922 RVA: 0x002D8E75 File Offset: 0x002D7E75
		// (remove) Token: 0x060070FB RID: 28923 RVA: 0x002D8E8E File Offset: 0x002D7E8E
		event EventHandler Changing;

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x060070FC RID: 28924 RVA: 0x002D8EA7 File Offset: 0x002D7EA7
		// (remove) Token: 0x060070FD RID: 28925 RVA: 0x002D8EC0 File Offset: 0x002D7EC0
		event TextContainerChangeEventHandler Change;

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x060070FE RID: 28926 RVA: 0x002D8ED9 File Offset: 0x002D7ED9
		// (remove) Token: 0x060070FF RID: 28927 RVA: 0x002D8EF2 File Offset: 0x002D7EF2
		event TextContainerChangedEventHandler Changed;

		// Token: 0x06007100 RID: 28928 RVA: 0x002D8F0C File Offset: 0x002D7F0C
		private void AddChange(ITextPointer startPosition, int symbolCount, PrecursorTextChangeType precursorTextChange)
		{
			Invariant.Assert(this._changeBlockLevel > 0, "All public APIs must call BeginChange!");
			Invariant.Assert(!this._isReadOnly, "Illegal to modify PasswordTextContainer inside Change event scope!");
			if (this.Changing != null)
			{
				this.Changing(this, EventArgs.Empty);
			}
			if (this._changes == null)
			{
				this._changes = new TextContainerChangedEventArgs();
			}
			this._changes.AddChange(precursorTextChange, startPosition.Offset, symbolCount, false);
			if (this.Change != null)
			{
				Invariant.Assert(precursorTextChange == PrecursorTextChangeType.ContentAdded || precursorTextChange == PrecursorTextChangeType.ContentRemoved);
				TextChangeType textChange = (precursorTextChange == PrecursorTextChangeType.ContentAdded) ? TextChangeType.ContentAdded : TextChangeType.ContentRemoved;
				this._isReadOnly = true;
				try
				{
					this.Change(this, new TextContainerChangeEventArgs(startPosition, symbolCount, symbolCount, textChange));
				}
				finally
				{
					this._isReadOnly = false;
				}
			}
		}

		// Token: 0x06007101 RID: 28929 RVA: 0x002D8FD4 File Offset: 0x002D7FD4
		private void OnPasswordChange(int offset, int delta)
		{
			if (delta != 0)
			{
				this.UpdatePositionList(offset, delta);
				PasswordTextPointer startPosition = new PasswordTextPointer(this, LogicalDirection.Forward, offset);
				int symbolCount;
				PrecursorTextChangeType precursorTextChange;
				if (delta > 0)
				{
					symbolCount = delta;
					precursorTextChange = PrecursorTextChangeType.ContentAdded;
				}
				else
				{
					symbolCount = -delta;
					precursorTextChange = PrecursorTextChangeType.ContentRemoved;
				}
				this.AddChange(startPosition, symbolCount, precursorTextChange);
			}
		}

		// Token: 0x06007102 RID: 28930 RVA: 0x002D9010 File Offset: 0x002D8010
		private void UpdatePositionList(int offset, int delta)
		{
			if (this._positionList == null)
			{
				return;
			}
			this.RemoveUnreferencedPositions();
			int i = this.FindIndex(offset, LogicalDirection.Forward);
			if (delta < 0)
			{
				int num = -1;
				while (i < this._positionList.Count)
				{
					PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
					if (pointerAtIndex != null)
					{
						if (pointerAtIndex.Offset > offset + -delta)
						{
							break;
						}
						pointerAtIndex.Offset = offset;
						if (pointerAtIndex.LogicalDirection == LogicalDirection.Backward)
						{
							if (num >= 0)
							{
								WeakReference value = (WeakReference)this._positionList[num];
								this._positionList[num] = this._positionList[i];
								this._positionList[i] = value;
								num++;
							}
						}
						else if (num == -1)
						{
							num = i;
						}
					}
					i++;
				}
			}
			while (i < this._positionList.Count)
			{
				PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
				if (pointerAtIndex != null)
				{
					pointerAtIndex.Offset += delta;
				}
				i++;
			}
			this.DebugAssertPositionList();
		}

		// Token: 0x06007103 RID: 28931 RVA: 0x002D90F8 File Offset: 0x002D80F8
		private void RemoveUnreferencedPositions()
		{
			if (this._positionList == null)
			{
				return;
			}
			for (int i = this._positionList.Count - 1; i >= 0; i--)
			{
				if (this.GetPointerAtIndex(i) == null)
				{
					this._positionList.RemoveAt(i);
				}
			}
		}

		// Token: 0x06007104 RID: 28932 RVA: 0x002D913C File Offset: 0x002D813C
		private int FindIndex(int offset, LogicalDirection gravity)
		{
			Invariant.Assert(this._positionList != null);
			int i;
			for (i = 0; i < this._positionList.Count; i++)
			{
				PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
				if (pointerAtIndex != null && ((pointerAtIndex.Offset == offset && (pointerAtIndex.LogicalDirection == gravity || gravity == LogicalDirection.Backward)) || pointerAtIndex.Offset > offset))
				{
					break;
				}
			}
			return i;
		}

		// Token: 0x06007105 RID: 28933 RVA: 0x002D9198 File Offset: 0x002D8198
		private void DebugAssertPositionList()
		{
			if (Invariant.Strict)
			{
				int num = -1;
				LogicalDirection logicalDirection = LogicalDirection.Backward;
				for (int i = 0; i < this._positionList.Count; i++)
				{
					PasswordTextPointer pointerAtIndex = this.GetPointerAtIndex(i);
					if (pointerAtIndex != null)
					{
						Invariant.Assert(pointerAtIndex.Offset >= 0 && pointerAtIndex.Offset <= this._password.Length);
						Invariant.Assert(num <= pointerAtIndex.Offset);
						if (i > 0 && pointerAtIndex.LogicalDirection == LogicalDirection.Backward && num == pointerAtIndex.Offset)
						{
							Invariant.Assert(logicalDirection != LogicalDirection.Forward);
						}
						num = pointerAtIndex.Offset;
						logicalDirection = pointerAtIndex.LogicalDirection;
					}
				}
			}
		}

		// Token: 0x06007106 RID: 28934 RVA: 0x002D9240 File Offset: 0x002D8240
		private PasswordTextPointer GetPointerAtIndex(int index)
		{
			Invariant.Assert(this._positionList != null);
			WeakReference weakReference = (WeakReference)this._positionList[index];
			Invariant.Assert(weakReference != null);
			object target = weakReference.Target;
			if (target != null && !(target is PasswordTextPointer))
			{
				bool condition = false;
				string str = "Unexpected type: ";
				Type type = target.GetType();
				Invariant.Assert(condition, str + ((type != null) ? type.ToString() : null));
			}
			return (PasswordTextPointer)target;
		}

		// Token: 0x04003703 RID: 14083
		private readonly PasswordBox _passwordBox;

		// Token: 0x04003704 RID: 14084
		private SecureString _password;

		// Token: 0x04003705 RID: 14085
		private ArrayList _positionList;

		// Token: 0x04003706 RID: 14086
		private Highlights _highlights;

		// Token: 0x04003707 RID: 14087
		private int _changeBlockLevel;

		// Token: 0x04003708 RID: 14088
		private TextContainerChangedEventArgs _changes;

		// Token: 0x04003709 RID: 14089
		private ITextView _textview;

		// Token: 0x0400370A RID: 14090
		private bool _isReadOnly;

		// Token: 0x0400370B RID: 14091
		private EventHandler Changing;

		// Token: 0x0400370C RID: 14092
		private TextContainerChangeEventHandler Change;

		// Token: 0x0400370D RID: 14093
		private TextContainerChangedEventHandler Changed;

		// Token: 0x0400370E RID: 14094
		private ITextSelection _textSelection;
	}
}
