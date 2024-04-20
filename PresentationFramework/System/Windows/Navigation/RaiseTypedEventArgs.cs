using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005CE RID: 1486
	internal class RaiseTypedEventArgs : EventArgs
	{
		// Token: 0x060047E9 RID: 18409 RVA: 0x0022AB39 File Offset: 0x00229B39
		internal RaiseTypedEventArgs(Delegate d, object o)
		{
			this.D = d;
			this.O = o;
		}

		// Token: 0x040025ED RID: 9709
		internal Delegate D;

		// Token: 0x040025EE RID: 9710
		internal object O;
	}
}
