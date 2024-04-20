using System;

namespace System.Windows.Documents
{
	// Token: 0x0200069C RID: 1692
	internal class TextContainerChangeEventArgs : EventArgs
	{
		// Token: 0x06005532 RID: 21810 RVA: 0x00260823 File Offset: 0x0025F823
		internal TextContainerChangeEventArgs(ITextPointer textPosition, int count, int charCount, TextChangeType textChange) : this(textPosition, count, charCount, textChange, null, false)
		{
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x00260832 File Offset: 0x0025F832
		internal TextContainerChangeEventArgs(ITextPointer textPosition, int count, int charCount, TextChangeType textChange, DependencyProperty property, bool affectsRenderOnly)
		{
			this._textPosition = textPosition.GetFrozenPointer(LogicalDirection.Forward);
			this._count = count;
			this._charCount = charCount;
			this._textChange = textChange;
			this._property = property;
			this._affectsRenderOnly = affectsRenderOnly;
		}

		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x06005534 RID: 21812 RVA: 0x0026086D File Offset: 0x0025F86D
		internal ITextPointer ITextPosition
		{
			get
			{
				return this._textPosition;
			}
		}

		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x06005535 RID: 21813 RVA: 0x00260875 File Offset: 0x0025F875
		internal int IMECharCount
		{
			get
			{
				return this._charCount;
			}
		}

		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06005536 RID: 21814 RVA: 0x0026087D File Offset: 0x0025F87D
		internal bool AffectsRenderOnly
		{
			get
			{
				return this._affectsRenderOnly;
			}
		}

		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x06005537 RID: 21815 RVA: 0x00260885 File Offset: 0x0025F885
		internal int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x06005538 RID: 21816 RVA: 0x0026088D File Offset: 0x0025F88D
		internal TextChangeType TextChange
		{
			get
			{
				return this._textChange;
			}
		}

		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x06005539 RID: 21817 RVA: 0x00260895 File Offset: 0x0025F895
		internal DependencyProperty Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x04002F17 RID: 12055
		private readonly ITextPointer _textPosition;

		// Token: 0x04002F18 RID: 12056
		private readonly int _count;

		// Token: 0x04002F19 RID: 12057
		private readonly int _charCount;

		// Token: 0x04002F1A RID: 12058
		private readonly TextChangeType _textChange;

		// Token: 0x04002F1B RID: 12059
		private readonly DependencyProperty _property;

		// Token: 0x04002F1C RID: 12060
		private readonly bool _affectsRenderOnly;
	}
}
