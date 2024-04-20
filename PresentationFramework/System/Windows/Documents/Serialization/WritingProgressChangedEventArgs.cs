using System;
using System.ComponentModel;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006F1 RID: 1777
	public class WritingProgressChangedEventArgs : ProgressChangedEventArgs
	{
		// Token: 0x06005D4E RID: 23886 RVA: 0x0028C87A File Offset: 0x0028B87A
		public WritingProgressChangedEventArgs(WritingProgressChangeLevel writingLevel, int number, int progressPercentage, object state) : base(progressPercentage, state)
		{
			this._number = number;
			this._writingLevel = writingLevel;
		}

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x06005D4F RID: 23887 RVA: 0x0028C893 File Offset: 0x0028B893
		public int Number
		{
			get
			{
				return this._number;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x06005D50 RID: 23888 RVA: 0x0028C89B File Offset: 0x0028B89B
		public WritingProgressChangeLevel WritingLevel
		{
			get
			{
				return this._writingLevel;
			}
		}

		// Token: 0x04003158 RID: 12632
		private int _number;

		// Token: 0x04003159 RID: 12633
		private WritingProgressChangeLevel _writingLevel;
	}
}
