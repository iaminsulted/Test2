using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x02000639 RID: 1593
	internal class BringPageIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06004EF2 RID: 20210 RVA: 0x00243456 File Offset: 0x00242456
		public BringPageIntoViewCompletedEventArgs(ITextPointer position, Point suggestedOffset, int count, ITextPointer newPosition, Point newSuggestedOffset, int pagesMoved, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this._position = position;
			this._count = count;
			this._newPosition = newPosition;
			this._newSuggestedOffset = newSuggestedOffset;
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06004EF3 RID: 20211 RVA: 0x00243482 File Offset: 0x00242482
		public ITextPointer Position
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._position;
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x00243490 File Offset: 0x00242490
		public int Count
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._count;
			}
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06004EF5 RID: 20213 RVA: 0x0024349E File Offset: 0x0024249E
		public ITextPointer NewPosition
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newPosition;
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06004EF6 RID: 20214 RVA: 0x002434AC File Offset: 0x002424AC
		public Point NewSuggestedOffset
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._newSuggestedOffset;
			}
		}

		// Token: 0x0400283D RID: 10301
		private readonly ITextPointer _position;

		// Token: 0x0400283E RID: 10302
		private readonly int _count;

		// Token: 0x0400283F RID: 10303
		private readonly ITextPointer _newPosition;

		// Token: 0x04002840 RID: 10304
		private readonly Point _newSuggestedOffset;
	}
}
