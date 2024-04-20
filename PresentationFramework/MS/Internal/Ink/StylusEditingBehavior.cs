using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000192 RID: 402
	internal abstract class StylusEditingBehavior : EditingBehavior, IStylusEditing
	{
		// Token: 0x06000D66 RID: 3430 RVA: 0x001348B3 File Offset: 0x001338B3
		internal StylusEditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0013526C File Offset: 0x0013426C
		internal void SwitchToMode(InkCanvasEditingMode mode)
		{
			this._disableInput = true;
			try
			{
				this.OnSwitchToMode(mode);
			}
			finally
			{
				this._disableInput = false;
			}
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x001352A4 File Offset: 0x001342A4
		void IStylusEditing.AddStylusPoints(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (this._disableInput)
			{
				return;
			}
			if (!base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = true;
				this.StylusInputBegin(stylusPoints, userInitiated);
				return;
			}
			this.StylusInputContinue(stylusPoints, userInitiated);
		}

		// Token: 0x06000D69 RID: 3433
		protected abstract void OnSwitchToMode(InkCanvasEditingMode mode);

		// Token: 0x06000D6A RID: 3434 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void OnActivate()
		{
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void OnDeactivate()
		{
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x001352D9 File Offset: 0x001342D9
		protected sealed override void OnCommit(bool commit)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = false;
				this.StylusInputEnd(commit);
				return;
			}
			this.OnCommitWithoutStylusInput(commit);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void StylusInputEnd(bool commit)
		{
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnCommitWithoutStylusInput(bool commit)
		{
		}

		// Token: 0x040009DE RID: 2526
		private bool _disableInput;
	}
}
