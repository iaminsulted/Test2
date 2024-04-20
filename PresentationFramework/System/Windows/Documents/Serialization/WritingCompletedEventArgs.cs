using System;
using System.ComponentModel;

namespace System.Windows.Documents.Serialization
{
	// Token: 0x020006F0 RID: 1776
	public class WritingCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06005D4D RID: 23885 RVA: 0x0028C86F File Offset: 0x0028B86F
		public WritingCompletedEventArgs(bool cancelled, object state, Exception exception) : base(exception, cancelled, state)
		{
		}
	}
}
