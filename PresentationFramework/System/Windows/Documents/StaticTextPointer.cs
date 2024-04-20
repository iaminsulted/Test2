using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200068E RID: 1678
	internal struct StaticTextPointer
	{
		// Token: 0x06005356 RID: 21334 RVA: 0x0025C27C File Offset: 0x0025B27C
		internal StaticTextPointer(ITextContainer textContainer, object handle0)
		{
			this = new StaticTextPointer(textContainer, handle0, 0);
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x0025C287 File Offset: 0x0025B287
		internal StaticTextPointer(ITextContainer textContainer, object handle0, int handle1)
		{
			this._textContainer = textContainer;
			this._generation = ((textContainer != null) ? textContainer.Generation : 0U);
			this._handle0 = handle0;
			this._handle1 = handle1;
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x0025C2B0 File Offset: 0x0025B2B0
		internal ITextPointer CreateDynamicTextPointer(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.CreateDynamicTextPointer(this, direction);
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x0025C2CA File Offset: 0x0025B2CA
		internal TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.GetPointerContext(this, direction);
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x0025C2E4 File Offset: 0x0025B2E4
		internal int GetOffsetToPosition(StaticTextPointer position)
		{
			this.AssertGeneration();
			return this._textContainer.GetOffsetToPosition(this, position);
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x0025C2FE File Offset: 0x0025B2FE
		internal int GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			this.AssertGeneration();
			return this._textContainer.GetTextInRun(this, direction, textBuffer, startIndex, count);
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x0025C31C File Offset: 0x0025B31C
		internal object GetAdjacentElement(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.GetAdjacentElement(this, direction);
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x0025C336 File Offset: 0x0025B336
		internal StaticTextPointer CreatePointer(int offset)
		{
			this.AssertGeneration();
			return this._textContainer.CreatePointer(this, offset);
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x0025C350 File Offset: 0x0025B350
		internal StaticTextPointer GetNextContextPosition(LogicalDirection direction)
		{
			this.AssertGeneration();
			return this._textContainer.GetNextContextPosition(this, direction);
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x0025C36A File Offset: 0x0025B36A
		internal int CompareTo(StaticTextPointer position)
		{
			this.AssertGeneration();
			return this._textContainer.CompareTo(this, position);
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x0025C384 File Offset: 0x0025B384
		internal int CompareTo(ITextPointer position)
		{
			this.AssertGeneration();
			return this._textContainer.CompareTo(this, position);
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x0025C39E File Offset: 0x0025B39E
		internal object GetValue(DependencyProperty formattingProperty)
		{
			this.AssertGeneration();
			return this._textContainer.GetValue(this, formattingProperty);
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x0025C3B8 File Offset: 0x0025B3B8
		internal static StaticTextPointer Min(StaticTextPointer position1, StaticTextPointer position2)
		{
			position2.AssertGeneration();
			if (position1.CompareTo(position2) > 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x0025C3CF File Offset: 0x0025B3CF
		internal static StaticTextPointer Max(StaticTextPointer position1, StaticTextPointer position2)
		{
			position2.AssertGeneration();
			if (position1.CompareTo(position2) < 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x0025C3E6 File Offset: 0x0025B3E6
		internal void AssertGeneration()
		{
			if (this._textContainer != null)
			{
				Invariant.Assert(this._generation == this._textContainer.Generation, "StaticTextPointer not synchronized to tree generation!");
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06005365 RID: 21349 RVA: 0x0025C40D File Offset: 0x0025B40D
		internal ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06005366 RID: 21350 RVA: 0x0025C415 File Offset: 0x0025B415
		internal DependencyObject Parent
		{
			get
			{
				return this._textContainer.GetParent(this);
			}
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06005367 RID: 21351 RVA: 0x0025C428 File Offset: 0x0025B428
		internal bool IsNull
		{
			get
			{
				return this._textContainer == null;
			}
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06005368 RID: 21352 RVA: 0x0025C433 File Offset: 0x0025B433
		internal object Handle0
		{
			get
			{
				return this._handle0;
			}
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06005369 RID: 21353 RVA: 0x0025C43B File Offset: 0x0025B43B
		internal int Handle1
		{
			get
			{
				return this._handle1;
			}
		}

		// Token: 0x04002ED8 RID: 11992
		internal static StaticTextPointer Null = new StaticTextPointer(null, null, 0);

		// Token: 0x04002ED9 RID: 11993
		private readonly ITextContainer _textContainer;

		// Token: 0x04002EDA RID: 11994
		private readonly uint _generation;

		// Token: 0x04002EDB RID: 11995
		private readonly object _handle0;

		// Token: 0x04002EDC RID: 11996
		private readonly int _handle1;
	}
}
