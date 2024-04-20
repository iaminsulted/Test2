using System;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x0200039E RID: 926
	public class StartupEventArgs : EventArgs
	{
		// Token: 0x06002566 RID: 9574 RVA: 0x00186408 File Offset: 0x00185408
		internal StartupEventArgs()
		{
			this._performDefaultAction = true;
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002567 RID: 9575 RVA: 0x00186417 File Offset: 0x00185417
		public string[] Args
		{
			get
			{
				if (this._args == null)
				{
					this._args = this.GetCmdLineArgs();
				}
				return this._args;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002568 RID: 9576 RVA: 0x00186433 File Offset: 0x00185433
		// (set) Token: 0x06002569 RID: 9577 RVA: 0x0018643B File Offset: 0x0018543B
		internal bool PerformDefaultAction
		{
			get
			{
				return this._performDefaultAction;
			}
			set
			{
				this._performDefaultAction = value;
			}
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x00186444 File Offset: 0x00185444
		private string[] GetCmdLineArgs()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			Invariant.Assert(commandLineArgs.Length >= 1);
			int num = commandLineArgs.Length - 1;
			num = ((num >= 0) ? num : 0);
			string[] array = new string[num];
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				array[i - 1] = commandLineArgs[i];
			}
			return array;
		}

		// Token: 0x0400118E RID: 4494
		private string[] _args;

		// Token: 0x0400118F RID: 4495
		private bool _performDefaultAction;
	}
}
