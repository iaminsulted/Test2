using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x02000637 RID: 1591
	internal class BringPointIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06004EEA RID: 20202 RVA: 0x002433BA File Offset: 0x002423BA
		public BringPointIntoViewCompletedEventArgs(Point point, ITextPointer position, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
			this._point = point;
			this._position = position;
		}

		// Token: 0x17001249 RID: 4681
		// (get) Token: 0x06004EEB RID: 20203 RVA: 0x002433D6 File Offset: 0x002423D6
		public Point Point
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._point;
			}
		}

		// Token: 0x1700124A RID: 4682
		// (get) Token: 0x06004EEC RID: 20204 RVA: 0x002433E4 File Offset: 0x002423E4
		public ITextPointer Position
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this._position;
			}
		}

		// Token: 0x04002837 RID: 10295
		private readonly Point _point;

		// Token: 0x04002838 RID: 10296
		private readonly ITextPointer _position;
	}
}
