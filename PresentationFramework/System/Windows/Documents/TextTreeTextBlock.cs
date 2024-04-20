using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020006D3 RID: 1747
	internal class TextTreeTextBlock : SplayTreeNode
	{
		// Token: 0x06005B36 RID: 23350 RVA: 0x00283AA8 File Offset: 0x00282AA8
		internal TextTreeTextBlock(int size)
		{
			Invariant.Assert(size > 0);
			Invariant.Assert(size <= 4096);
			this._text = new char[size];
			this._gapSize = size;
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x00283ADC File Offset: 0x00282ADC
		internal int InsertText(int logicalOffset, object text, int textStartIndex, int textEndIndex)
		{
			Invariant.Assert(text is string || text is char[], "Bad text parameter!");
			Invariant.Assert(textStartIndex <= textEndIndex, "Bad start/end index!");
			base.Splay();
			int num = textEndIndex - textStartIndex;
			if (this._text.Length < 4096 && num > this._gapSize)
			{
				char[] array = new char[Math.Min(this.Count + num, 4096)];
				Array.Copy(this._text, 0, array, 0, this._gapOffset);
				int num2 = this._text.Length - (this._gapOffset + this._gapSize);
				Array.Copy(this._text, this._gapOffset + this._gapSize, array, array.Length - num2, num2);
				this._gapSize += array.Length - this._text.Length;
				this._text = array;
			}
			if (logicalOffset != this._gapOffset)
			{
				this.MoveGap(logicalOffset);
			}
			num = Math.Min(num, this._gapSize);
			string text2 = text as string;
			if (text2 != null)
			{
				text2.CopyTo(textStartIndex, this._text, logicalOffset, num);
			}
			else
			{
				Array.Copy((char[])text, textStartIndex, this._text, logicalOffset, num);
			}
			this._gapOffset += num;
			this._gapSize -= num;
			return num;
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x00283C30 File Offset: 0x00282C30
		internal TextTreeTextBlock SplitBlock()
		{
			Invariant.Assert(this._gapSize == 0, "Splitting non-full block!");
			Invariant.Assert(this._text.Length == 4096, "Splitting non-max sized block!");
			TextTreeTextBlock textTreeTextBlock = new TextTreeTextBlock(4096);
			bool insertBefore;
			if (this._gapOffset < 2048)
			{
				Array.Copy(this._text, 0, textTreeTextBlock._text, 0, this._gapOffset);
				textTreeTextBlock._gapOffset = this._gapOffset;
				textTreeTextBlock._gapSize = 4096 - this._gapOffset;
				this._gapSize += this._gapOffset;
				this._gapOffset = 0;
				insertBefore = true;
			}
			else
			{
				Array.Copy(this._text, this._gapOffset, textTreeTextBlock._text, this._gapOffset, 4096 - this._gapOffset);
				Invariant.Assert(textTreeTextBlock._gapOffset == 0);
				textTreeTextBlock._gapSize = this._gapOffset;
				this._gapSize = 4096 - this._gapOffset;
				insertBefore = false;
			}
			textTreeTextBlock.InsertAtNode(this, insertBefore);
			return textTreeTextBlock;
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x00283D38 File Offset: 0x00282D38
		internal void RemoveText(int logicalOffset, int count)
		{
			Invariant.Assert(logicalOffset >= 0);
			Invariant.Assert(count >= 0);
			Invariant.Assert(logicalOffset + count <= this.Count, "Removing too much text!");
			int num = count;
			int count2 = this.Count;
			base.Splay();
			if (logicalOffset < this._gapOffset)
			{
				if (logicalOffset + count < this._gapOffset)
				{
					this.MoveGap(logicalOffset + count);
				}
				int num2 = (logicalOffset + count == this._gapOffset) ? count : (this._gapOffset - logicalOffset);
				this._gapOffset -= num2;
				this._gapSize += num2;
				logicalOffset = this._gapOffset;
				count -= num2;
			}
			logicalOffset += this._gapSize;
			if (logicalOffset > this._gapOffset + this._gapSize)
			{
				this.MoveGap(logicalOffset - this._gapSize);
			}
			this._gapSize += count;
			Invariant.Assert(this._gapOffset + this._gapSize <= this._text.Length);
			Invariant.Assert(count2 == this.Count + num);
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x00283E44 File Offset: 0x00282E44
		internal int ReadText(int logicalOffset, int count, char[] chars, int charsStartIndex)
		{
			int num = count;
			if (logicalOffset < this._gapOffset)
			{
				int num2 = Math.Min(count, this._gapOffset - logicalOffset);
				Array.Copy(this._text, logicalOffset, chars, charsStartIndex, num2);
				count -= num2;
				charsStartIndex += num2;
				logicalOffset = this._gapOffset;
			}
			if (count > 0)
			{
				logicalOffset += this._gapSize;
				int num2 = Math.Min(count, this._text.Length - logicalOffset);
				Array.Copy(this._text, logicalOffset, chars, charsStartIndex, num2);
				count -= num2;
			}
			return num - count;
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x06005B3B RID: 23355 RVA: 0x00283EC3 File Offset: 0x00282EC3
		// (set) Token: 0x06005B3C RID: 23356 RVA: 0x00283ECB File Offset: 0x00282ECB
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return this._parentNode;
			}
			set
			{
				this._parentNode = value;
			}
		}

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06005B3D RID: 23357 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x06005B3E RID: 23358 RVA: 0x00283ED4 File Offset: 0x00282ED4
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set ContainedNode on a TextTreeTextBlock!");
			}
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x00283EE1 File Offset: 0x00282EE1
		// (set) Token: 0x06005B40 RID: 23360 RVA: 0x00283EE9 File Offset: 0x00282EE9
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

		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x06005B41 RID: 23361 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B42 RID: 23362 RVA: 0x00283573 File Offset: 0x00282573
		internal override int LeftCharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0);
			}
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x06005B43 RID: 23363 RVA: 0x00283EF2 File Offset: 0x00282EF2
		// (set) Token: 0x06005B44 RID: 23364 RVA: 0x00283EFA File Offset: 0x00282EFA
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return this._leftChildNode;
			}
			set
			{
				this._leftChildNode = (TextTreeTextBlock)value;
			}
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x06005B45 RID: 23365 RVA: 0x00283F08 File Offset: 0x00282F08
		// (set) Token: 0x06005B46 RID: 23366 RVA: 0x00283F10 File Offset: 0x00282F10
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return this._rightChildNode;
			}
			set
			{
				this._rightChildNode = (TextTreeTextBlock)value;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x06005B47 RID: 23367 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B48 RID: 23368 RVA: 0x00283F1E File Offset: 0x00282F1E
		internal override uint Generation
		{
			get
			{
				return 0U;
			}
			set
			{
				Invariant.Assert(false, "TextTreeTextBlock does not track Generation!");
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x06005B49 RID: 23369 RVA: 0x0016545A File Offset: 0x0016445A
		// (set) Token: 0x06005B4A RID: 23370 RVA: 0x00283F2B File Offset: 0x00282F2B
		internal override int SymbolOffsetCache
		{
			get
			{
				return -1;
			}
			set
			{
				Invariant.Assert(false, "TextTreeTextBlock does not track SymbolOffsetCache!");
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06005B4B RID: 23371 RVA: 0x00283F38 File Offset: 0x00282F38
		// (set) Token: 0x06005B4C RID: 23372 RVA: 0x00283F40 File Offset: 0x00282F40
		internal override int SymbolCount
		{
			get
			{
				return this.Count;
			}
			set
			{
				Invariant.Assert(false, "Can't set SymbolCount on TextTreeTextBlock!");
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06005B4D RID: 23373 RVA: 0x00105F35 File Offset: 0x00104F35
		// (set) Token: 0x06005B4E RID: 23374 RVA: 0x00283573 File Offset: 0x00282573
		internal override int IMECharCount
		{
			get
			{
				return 0;
			}
			set
			{
				Invariant.Assert(value == 0);
			}
		}

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06005B4F RID: 23375 RVA: 0x00283F4D File Offset: 0x00282F4D
		internal int Count
		{
			get
			{
				return this._text.Length - this._gapSize;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x06005B50 RID: 23376 RVA: 0x00283F5E File Offset: 0x00282F5E
		internal int FreeCapacity
		{
			get
			{
				return this._gapSize;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x06005B51 RID: 23377 RVA: 0x00283F66 File Offset: 0x00282F66
		internal int GapOffset
		{
			get
			{
				return this._gapOffset;
			}
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x00283F70 File Offset: 0x00282F70
		private void MoveGap(int offset)
		{
			int sourceIndex;
			int destinationIndex;
			int length;
			if (offset < this._gapOffset)
			{
				sourceIndex = offset;
				destinationIndex = offset + this._gapSize;
				length = this._gapOffset - offset;
			}
			else
			{
				sourceIndex = this._gapOffset + this._gapSize;
				destinationIndex = this._gapOffset;
				length = offset - this._gapOffset;
			}
			Array.Copy(this._text, sourceIndex, this._text, destinationIndex, length);
			this._gapOffset = offset;
		}

		// Token: 0x04003076 RID: 12406
		private int _leftSymbolCount;

		// Token: 0x04003077 RID: 12407
		private SplayTreeNode _parentNode;

		// Token: 0x04003078 RID: 12408
		private TextTreeTextBlock _leftChildNode;

		// Token: 0x04003079 RID: 12409
		private TextTreeTextBlock _rightChildNode;

		// Token: 0x0400307A RID: 12410
		private char[] _text;

		// Token: 0x0400307B RID: 12411
		private int _gapOffset;

		// Token: 0x0400307C RID: 12412
		private int _gapSize;

		// Token: 0x0400307D RID: 12413
		internal const int MaxBlockSize = 4096;
	}
}
