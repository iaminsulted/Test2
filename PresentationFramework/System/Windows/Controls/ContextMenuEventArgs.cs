using System;

namespace System.Windows.Controls
{
	// Token: 0x02000739 RID: 1849
	public sealed class ContextMenuEventArgs : RoutedEventArgs
	{
		// Token: 0x060061E0 RID: 25056 RVA: 0x0029E1E2 File Offset: 0x0029D1E2
		internal ContextMenuEventArgs(object source, bool opening) : this(source, opening, -1.0, -1.0)
		{
		}

		// Token: 0x060061E1 RID: 25057 RVA: 0x0029E1FE File Offset: 0x0029D1FE
		internal ContextMenuEventArgs(object source, bool opening, double left, double top)
		{
			this._left = left;
			this._top = top;
			base.RoutedEvent = (opening ? ContextMenuService.ContextMenuOpeningEvent : ContextMenuService.ContextMenuClosingEvent);
			base.Source = source;
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x060061E2 RID: 25058 RVA: 0x0029E231 File Offset: 0x0029D231
		public double CursorLeft
		{
			get
			{
				return this._left;
			}
		}

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x060061E3 RID: 25059 RVA: 0x0029E239 File Offset: 0x0029D239
		public double CursorTop
		{
			get
			{
				return this._top;
			}
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x060061E4 RID: 25060 RVA: 0x0029E241 File Offset: 0x0029D241
		// (set) Token: 0x060061E5 RID: 25061 RVA: 0x0029E249 File Offset: 0x0029D249
		internal DependencyObject TargetElement
		{
			get
			{
				return this._targetElement;
			}
			set
			{
				this._targetElement = value;
			}
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x0029E252 File Offset: 0x0029D252
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((ContextMenuEventHandler)genericHandler)(genericTarget, this);
		}

		// Token: 0x04003299 RID: 12953
		private double _left;

		// Token: 0x0400329A RID: 12954
		private double _top;

		// Token: 0x0400329B RID: 12955
		private DependencyObject _targetElement;
	}
}
