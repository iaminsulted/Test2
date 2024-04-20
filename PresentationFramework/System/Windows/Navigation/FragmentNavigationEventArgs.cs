using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005AC RID: 1452
	public class FragmentNavigationEventArgs : EventArgs
	{
		// Token: 0x0600463D RID: 17981 RVA: 0x00225B32 File Offset: 0x00224B32
		internal FragmentNavigationEventArgs(string fragment, object Navigator)
		{
			this._fragment = fragment;
			this._navigator = Navigator;
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x00225B48 File Offset: 0x00224B48
		public string Fragment
		{
			get
			{
				return this._fragment;
			}
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x0600463F RID: 17983 RVA: 0x00225B50 File Offset: 0x00224B50
		// (set) Token: 0x06004640 RID: 17984 RVA: 0x00225B58 File Offset: 0x00224B58
		public bool Handled
		{
			get
			{
				return this._handled;
			}
			set
			{
				this._handled = value;
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06004641 RID: 17985 RVA: 0x00225B61 File Offset: 0x00224B61
		public object Navigator
		{
			get
			{
				return this._navigator;
			}
		}

		// Token: 0x04002560 RID: 9568
		private string _fragment;

		// Token: 0x04002561 RID: 9569
		private bool _handled;

		// Token: 0x04002562 RID: 9570
		private object _navigator;
	}
}
