using System;
using System.Windows.Documents;

namespace System.Windows.Navigation
{
	// Token: 0x020005D1 RID: 1489
	public class RequestNavigateEventArgs : RoutedEventArgs
	{
		// Token: 0x060047F3 RID: 18419 RVA: 0x0022ABB8 File Offset: 0x00229BB8
		protected RequestNavigateEventArgs()
		{
			base.RoutedEvent = Hyperlink.RequestNavigateEvent;
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0022ABCB File Offset: 0x00229BCB
		public RequestNavigateEventArgs(Uri uri, string target)
		{
			this._uri = uri;
			this._target = target;
			base.RoutedEvent = Hyperlink.RequestNavigateEvent;
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x060047F5 RID: 18421 RVA: 0x0022ABEC File Offset: 0x00229BEC
		public Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x060047F6 RID: 18422 RVA: 0x0022ABF4 File Offset: 0x00229BF4
		public string Target
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0022ABFC File Offset: 0x00229BFC
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			if (base.RoutedEvent == null)
			{
				throw new InvalidOperationException(SR.Get("RequestNavigateEventMustHaveRoutedEvent"));
			}
			((RequestNavigateEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x040025EF RID: 9711
		private Uri _uri;

		// Token: 0x040025F0 RID: 9712
		private string _target;
	}
}
