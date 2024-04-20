using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x02000638 RID: 1592
	internal class BringLineIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06004EED RID: 20205 RVA: 0x002433F2 File Offset: 0x002423F2
		public BringLineIntoViewCompletedEventArgs(ITextPointer position, double suggestedX, int count, ITextPointer newPosition, double newSuggestedX, int linesMoved, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this._position = position;
			this._count = count;
			this._newPosition = newPosition;
			this._newSuggestedX = newSuggestedX;
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06004EEE RID: 20206 RVA: 0x0024341E File Offset: 0x0024241E
		public ITextPointer Position
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._position;
			}
		}

		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06004EEF RID: 20207 RVA: 0x0024342C File Offset: 0x0024242C
		public int Count
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._count;
			}
		}

		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06004EF0 RID: 20208 RVA: 0x0024343A File Offset: 0x0024243A
		public ITextPointer NewPosition
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newPosition;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x00243448 File Offset: 0x00242448
		public double NewSuggestedX
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newSuggestedX;
			}
		}

		// Token: 0x04002839 RID: 10297
		private readonly ITextPointer _position;

		// Token: 0x0400283A RID: 10298
		private readonly int _count;

		// Token: 0x0400283B RID: 10299
		private readonly ITextPointer _newPosition;

		// Token: 0x0400283C RID: 10300
		private readonly double _newSuggestedX;
	}
}
