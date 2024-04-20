using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x02000636 RID: 1590
	internal class BringPositionIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06004EE9 RID: 20201 RVA: 0x002433AD File Offset: 0x002423AD
		public BringPositionIntoViewCompletedEventArgs(ITextPointer position, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
		}
	}
}
