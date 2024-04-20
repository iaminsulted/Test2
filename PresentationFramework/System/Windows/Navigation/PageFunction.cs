using System;

namespace System.Windows.Navigation
{
	// Token: 0x020005D0 RID: 1488
	public class PageFunction<T> : PageFunctionBase
	{
		// Token: 0x060047EE RID: 18414 RVA: 0x0022AB4F File Offset: 0x00229B4F
		public PageFunction()
		{
			base.RaiseTypedEvent += this.RaiseTypedReturnEvent;
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0022AB69 File Offset: 0x00229B69
		protected virtual void OnReturn(ReturnEventArgs<T> e)
		{
			base._OnReturnUnTyped(e);
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0022AB74 File Offset: 0x00229B74
		internal void RaiseTypedReturnEvent(PageFunctionBase b, RaiseTypedEventArgs args)
		{
			Delegate d = args.D;
			object o = args.O;
			if (d != null)
			{
				ReturnEventArgs<T> e = o as ReturnEventArgs<T>;
				(d as ReturnEventHandler<T>)(this, e);
			}
		}

		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060047F1 RID: 18417 RVA: 0x0022ABA6 File Offset: 0x00229BA6
		// (remove) Token: 0x060047F2 RID: 18418 RVA: 0x0022ABAF File Offset: 0x00229BAF
		public event ReturnEventHandler<T> Return
		{
			add
			{
				base._AddEventHandler(value);
			}
			remove
			{
				base._RemoveEventHandler(value);
			}
		}
	}
}
