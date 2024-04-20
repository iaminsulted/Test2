using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x02000182 RID: 386
	internal abstract class EditingBehavior
	{
		// Token: 0x06000C8D RID: 3213 RVA: 0x00130896 File Offset: 0x0012F896
		internal EditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			if (editingCoordinator == null)
			{
				throw new ArgumentNullException("editingCoordinator");
			}
			this._inkCanvas = inkCanvas;
			this._editingCoordinator = editingCoordinator;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x001308C8 File Offset: 0x0012F8C8
		public void Activate()
		{
			this.OnActivate();
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x001308D0 File Offset: 0x0012F8D0
		public void Deactivate()
		{
			this.OnDeactivate();
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x001308D8 File Offset: 0x0012F8D8
		public void Commit(bool commit)
		{
			this.OnCommit(commit);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x001308E1 File Offset: 0x0012F8E1
		public void UpdateTransform()
		{
			if (!this.EditingCoordinator.IsTransformValid(this))
			{
				this.OnTransformChanged();
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x001308F7 File Offset: 0x0012F8F7
		public Cursor Cursor
		{
			get
			{
				if (this._cachedCursor == null || !this.EditingCoordinator.IsCursorValid(this))
				{
					this._cachedCursor = this.GetCurrentCursor();
				}
				return this._cachedCursor;
			}
		}

		// Token: 0x06000C93 RID: 3219
		protected abstract void OnActivate();

		// Token: 0x06000C94 RID: 3220
		protected abstract void OnDeactivate();

		// Token: 0x06000C95 RID: 3221
		protected abstract void OnCommit(bool commit);

		// Token: 0x06000C96 RID: 3222
		protected abstract Cursor GetCurrentCursor();

		// Token: 0x06000C97 RID: 3223 RVA: 0x00130921 File Offset: 0x0012F921
		protected void SelfDeactivate()
		{
			this.EditingCoordinator.DeactivateDynamicBehavior();
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00130930 File Offset: 0x0012F930
		protected Matrix GetElementTransformMatrix()
		{
			Transform layoutTransform = this.InkCanvas.LayoutTransform;
			Transform renderTransform = this.InkCanvas.RenderTransform;
			return layoutTransform.Value * renderTransform.Value;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnTransformChanged()
		{
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00130964 File Offset: 0x0012F964
		protected InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0013096C File Offset: 0x0012F96C
		protected EditingCoordinator EditingCoordinator
		{
			get
			{
				return this._editingCoordinator;
			}
		}

		// Token: 0x04000992 RID: 2450
		private InkCanvas _inkCanvas;

		// Token: 0x04000993 RID: 2451
		private EditingCoordinator _editingCoordinator;

		// Token: 0x04000994 RID: 2452
		private Cursor _cachedCursor;
	}
}
