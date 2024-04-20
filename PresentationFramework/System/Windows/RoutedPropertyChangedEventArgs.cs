using System;

namespace System.Windows
{
	// Token: 0x02000396 RID: 918
	public class RoutedPropertyChangedEventArgs<T> : RoutedEventArgs
	{
		// Token: 0x06002535 RID: 9525 RVA: 0x00185D39 File Offset: 0x00184D39
		public RoutedPropertyChangedEventArgs(T oldValue, T newValue)
		{
			this._oldValue = oldValue;
			this._newValue = newValue;
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x00185D4F File Offset: 0x00184D4F
		public RoutedPropertyChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent) : this(oldValue, newValue)
		{
			base.RoutedEvent = routedEvent;
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002537 RID: 9527 RVA: 0x00185D60 File Offset: 0x00184D60
		public T OldValue
		{
			get
			{
				return this._oldValue;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002538 RID: 9528 RVA: 0x00185D68 File Offset: 0x00184D68
		public T NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x00185D70 File Offset: 0x00184D70
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((RoutedPropertyChangedEventHandler<T>)genericHandler)(genericTarget, this);
		}

		// Token: 0x0400117A RID: 4474
		private T _oldValue;

		// Token: 0x0400117B RID: 4475
		private T _newValue;
	}
}
