using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x020007BD RID: 1981
	internal sealed class PasswordTextPointer : ITextPointer
	{
		// Token: 0x06007107 RID: 28935 RVA: 0x002D92AE File Offset: 0x002D82AE
		internal PasswordTextPointer(PasswordTextContainer container, LogicalDirection gravity, int offset)
		{
			this._container = container;
			this._gravity = gravity;
			this._offset = offset;
			container.AddPosition(this);
		}

		// Token: 0x06007108 RID: 28936 RVA: 0x002D92D2 File Offset: 0x002D82D2
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			if (direction != this._gravity)
			{
				this.Container.RemovePosition(this);
				this._gravity = direction;
				this.Container.AddPosition(this);
			}
		}

		// Token: 0x06007109 RID: 28937 RVA: 0x002D92FC File Offset: 0x002D82FC
		int ITextPointer.CompareTo(ITextPointer position)
		{
			int offset = ((PasswordTextPointer)position)._offset;
			int result;
			if (this._offset < offset)
			{
				result = -1;
			}
			else if (this._offset > offset)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600710A RID: 28938 RVA: 0x00230023 File Offset: 0x0022F023
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			return ((ITextPointer)this).CompareTo((ITextPointer)position.Handle0);
		}

		// Token: 0x0600710B RID: 28939 RVA: 0x002D9332 File Offset: 0x002D8332
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return ((PasswordTextPointer)position)._offset - this._offset;
		}

		// Token: 0x0600710C RID: 28940 RVA: 0x002D9348 File Offset: 0x002D8348
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			TextPointerContext result;
			if ((direction == LogicalDirection.Backward && this._offset == 0) || (direction == LogicalDirection.Forward && this._offset == this._container.SymbolCount))
			{
				result = TextPointerContext.None;
			}
			else
			{
				result = TextPointerContext.Text;
			}
			return result;
		}

		// Token: 0x0600710D RID: 28941 RVA: 0x002D9380 File Offset: 0x002D8380
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			int result;
			if (direction == LogicalDirection.Forward)
			{
				result = this._container.SymbolCount - this._offset;
			}
			else
			{
				result = this._offset;
			}
			return result;
		}

		// Token: 0x0600710E RID: 28942 RVA: 0x00230052 File Offset: 0x0022F052
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x0600710F RID: 28943 RVA: 0x002D93B0 File Offset: 0x002D83B0
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			int num;
			if (direction == LogicalDirection.Forward)
			{
				num = Math.Min(count, this._container.SymbolCount - this._offset);
			}
			else
			{
				num = Math.Min(count, this._offset);
			}
			char passwordChar = this._container.PasswordChar;
			for (int i = 0; i < num; i++)
			{
				textBuffer[startIndex + i] = passwordChar;
			}
			return num;
		}

		// Token: 0x06007110 RID: 28944 RVA: 0x00109403 File Offset: 0x00108403
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06007111 RID: 28945 RVA: 0x00109403 File Offset: 0x00108403
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06007112 RID: 28946 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			return true;
		}

		// Token: 0x06007113 RID: 28947 RVA: 0x002D940A File Offset: 0x002D840A
		object ITextPointer.GetValue(DependencyProperty formattingProperty)
		{
			return this._container.PasswordBox.GetValue(formattingProperty);
		}

		// Token: 0x06007114 RID: 28948 RVA: 0x00244625 File Offset: 0x00243625
		object ITextPointer.ReadLocalValue(DependencyProperty formattingProperty)
		{
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x0024462C File Offset: 0x0024362C
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			return new DependencyObject().GetLocalValueEnumerator();
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x002D941D File Offset: 0x002D841D
		ITextPointer ITextPointer.CreatePointer()
		{
			return new PasswordTextPointer(this._container, this._gravity, this._offset);
		}

		// Token: 0x06007117 RID: 28951 RVA: 0x002300A5 File Offset: 0x0022F0A5
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			return new StaticTextPointer(((ITextPointer)this).TextContainer, ((ITextPointer)this).CreatePointer());
		}

		// Token: 0x06007118 RID: 28952 RVA: 0x002D9436 File Offset: 0x002D8436
		ITextPointer ITextPointer.CreatePointer(int distance)
		{
			return new PasswordTextPointer(this._container, this._gravity, this._offset + distance);
		}

		// Token: 0x06007119 RID: 28953 RVA: 0x002D9451 File Offset: 0x002D8451
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return new PasswordTextPointer(this._container, gravity, this._offset);
		}

		// Token: 0x0600711A RID: 28954 RVA: 0x002D9465 File Offset: 0x002D8465
		ITextPointer ITextPointer.CreatePointer(int distance, LogicalDirection gravity)
		{
			return new PasswordTextPointer(this._container, gravity, this._offset + distance);
		}

		// Token: 0x0600711B RID: 28955 RVA: 0x002D947B File Offset: 0x002D847B
		void ITextPointer.Freeze()
		{
			this._isFrozen = true;
		}

		// Token: 0x0600711C RID: 28956 RVA: 0x002300DD File Offset: 0x0022F0DD
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x002D9484 File Offset: 0x002D8484
		void ITextPointer.InsertTextInRun(string textData)
		{
			this._container.InsertText(this, textData);
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x002D9493 File Offset: 0x002D8493
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			this._container.DeleteContent(this, limit);
		}

		// Token: 0x0600711F RID: 28959 RVA: 0x002D94A4 File Offset: 0x002D84A4
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			int offset;
			if (direction == LogicalDirection.Backward)
			{
				if (this.Offset == 0)
				{
					return false;
				}
				offset = 0;
			}
			else
			{
				if (this.Offset == this.Container.SymbolCount)
				{
					return false;
				}
				offset = this.Container.SymbolCount;
			}
			this.Container.RemovePosition(this);
			this.Offset = offset;
			this.Container.AddPosition(this);
			return true;
		}

		// Token: 0x06007120 RID: 28960 RVA: 0x002D9504 File Offset: 0x002D8504
		int ITextPointer.MoveByOffset(int distance)
		{
			int num = this.Offset + distance;
			if (num >= 0)
			{
				int symbolCount = this.Container.SymbolCount;
			}
			this.Container.RemovePosition(this);
			this.Offset = num;
			this.Container.AddPosition(this);
			return distance;
		}

		// Token: 0x06007121 RID: 28961 RVA: 0x002D954C File Offset: 0x002D854C
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			this.Container.RemovePosition(this);
			this.Offset = ((PasswordTextPointer)position).Offset;
			this.Container.AddPosition(this);
		}

		// Token: 0x06007122 RID: 28962 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
		}

		// Token: 0x06007123 RID: 28963 RVA: 0x002D9577 File Offset: 0x002D8577
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return TextPointerBase.MoveToLineBoundary(this, this._container.TextView, count);
		}

		// Token: 0x06007124 RID: 28964 RVA: 0x00230304 File Offset: 0x0022F304
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		// Token: 0x06007125 RID: 28965 RVA: 0x0023030D File Offset: 0x0022F30D
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x06007126 RID: 28966 RVA: 0x00230316 File Offset: 0x0022F316
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x06007127 RID: 28967 RVA: 0x002300F8 File Offset: 0x0022F0F8
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

		// Token: 0x06007128 RID: 28968 RVA: 0x00230120 File Offset: 0x0022F120
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x0023014C File Offset: 0x0022F14C
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

		// Token: 0x0600712A RID: 28970 RVA: 0x00230136 File Offset: 0x0022F136
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x002D958B File Offset: 0x002D858B
		bool ITextPointer.ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, this._container.TextView);
		}

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x0600712C RID: 28972 RVA: 0x00109403 File Offset: 0x00108403
		Type ITextPointer.ParentType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x0600712D RID: 28973 RVA: 0x002D959E File Offset: 0x002D859E
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x0600712E RID: 28974 RVA: 0x002D95A6 File Offset: 0x002D85A6
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return this._container.TextView != null && this._container.TextView.IsValid && this._container.TextView.Contains(this);
			}
		}

		// Token: 0x17001A28 RID: 6696
		// (get) Token: 0x0600712F RID: 28975 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A29 RID: 6697
		// (get) Token: 0x06007130 RID: 28976 RVA: 0x002D95DA File Offset: 0x002D85DA
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this._gravity;
			}
		}

		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x06007131 RID: 28977 RVA: 0x00230221 File Offset: 0x0022F221
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x06007132 RID: 28978 RVA: 0x002D95E2 File Offset: 0x002D85E2
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this._isFrozen;
			}
		}

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x06007133 RID: 28979 RVA: 0x00230231 File Offset: 0x0022F231
		int ITextPointer.Offset
		{
			get
			{
				return TextPointerBase.GetOffset(this);
			}
		}

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x06007134 RID: 28980 RVA: 0x002D95EA File Offset: 0x002D85EA
		int ITextPointer.CharOffset
		{
			get
			{
				return this.Offset;
			}
		}

		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x06007135 RID: 28981 RVA: 0x002D959E File Offset: 0x002D859E
		internal PasswordTextContainer Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x06007136 RID: 28982 RVA: 0x002D95DA File Offset: 0x002D85DA
		internal LogicalDirection LogicalDirection
		{
			get
			{
				return this._gravity;
			}
		}

		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x06007137 RID: 28983 RVA: 0x002D95F2 File Offset: 0x002D85F2
		// (set) Token: 0x06007138 RID: 28984 RVA: 0x002D95FA File Offset: 0x002D85FA
		internal int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x0400370F RID: 14095
		private PasswordTextContainer _container;

		// Token: 0x04003710 RID: 14096
		private LogicalDirection _gravity;

		// Token: 0x04003711 RID: 14097
		private int _offset;

		// Token: 0x04003712 RID: 14098
		private bool _isFrozen;
	}
}
