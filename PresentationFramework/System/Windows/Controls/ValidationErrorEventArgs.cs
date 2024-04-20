using System;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020007FB RID: 2043
	public class ValidationErrorEventArgs : RoutedEventArgs
	{
		// Token: 0x060076E4 RID: 30436 RVA: 0x002F0D0F File Offset: 0x002EFD0F
		internal ValidationErrorEventArgs(ValidationError validationError, ValidationErrorEventAction action)
		{
			Invariant.Assert(validationError != null);
			base.RoutedEvent = Validation.ErrorEvent;
			this._validationError = validationError;
			this._action = action;
		}

		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x060076E5 RID: 30437 RVA: 0x002F0D39 File Offset: 0x002EFD39
		public ValidationError Error
		{
			get
			{
				return this._validationError;
			}
		}

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x060076E6 RID: 30438 RVA: 0x002F0D41 File Offset: 0x002EFD41
		public ValidationErrorEventAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x060076E7 RID: 30439 RVA: 0x002F0D49 File Offset: 0x002EFD49
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			((EventHandler<ValidationErrorEventArgs>)genericHandler)(genericTarget, this);
		}

		// Token: 0x040038B2 RID: 14514
		private ValidationError _validationError;

		// Token: 0x040038B3 RID: 14515
		private ValidationErrorEventAction _action;
	}
}
