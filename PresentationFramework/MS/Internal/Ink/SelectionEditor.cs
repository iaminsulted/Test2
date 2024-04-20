using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x0200018F RID: 399
	internal class SelectionEditor : EditingBehavior
	{
		// Token: 0x06000D57 RID: 3415 RVA: 0x001348B3 File Offset: 0x001338B3
		internal SelectionEditor(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00134F38 File Offset: 0x00133F38
		internal void OnInkCanvasSelectionChanged()
		{
			Point position = Mouse.PrimaryDevice.GetPosition(base.InkCanvas.SelectionAdorner);
			this.UpdateSelectionCursor(position);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00134F64 File Offset: 0x00133F64
		protected override void OnActivate()
		{
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(this.OnAdornerMouseButtonDownEvent));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseEnterEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(this.OnAdornerMouseLeaveEvent));
			Point position = Mouse.PrimaryDevice.GetPosition(base.InkCanvas.SelectionAdorner);
			this.UpdateSelectionCursor(position);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00135014 File Offset: 0x00134014
		protected override void OnDeactivate()
		{
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseDownEvent, new MouseButtonEventHandler(this.OnAdornerMouseButtonDownEvent));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseEnterEvent, new MouseEventHandler(this.OnAdornerMouseMoveEvent));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseLeaveEvent, new MouseEventHandler(this.OnAdornerMouseLeaveEvent));
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected override void OnCommit(bool commit)
		{
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x001350A5 File Offset: 0x001340A5
		protected override Cursor GetCurrentCursor()
		{
			if (base.InkCanvas.SelectionAdorner.IsMouseOver)
			{
				return PenCursorManager.GetSelectionCursor(this._hitResult, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft);
			}
			return null;
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x001350D4 File Offset: 0x001340D4
		private void OnAdornerMouseButtonDownEvent(object sender, MouseButtonEventArgs args)
		{
			if (args.StylusDevice == null && args.LeftButton != MouseButtonState.Pressed)
			{
				return;
			}
			Point position = args.GetPosition(base.InkCanvas.SelectionAdorner);
			if (this.HitTestOnSelectionAdorner(position) != InkCanvasSelectionHitResult.None)
			{
				base.EditingCoordinator.ActivateDynamicBehavior(base.EditingCoordinator.SelectionEditingBehavior, args.Device);
				return;
			}
			base.EditingCoordinator.ActivateDynamicBehavior(base.EditingCoordinator.LassoSelectionBehavior, (args.StylusDevice != null) ? args.StylusDevice : args.Device);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00135158 File Offset: 0x00134158
		private void OnAdornerMouseMoveEvent(object sender, MouseEventArgs args)
		{
			Point position = args.GetPosition(base.InkCanvas.SelectionAdorner);
			this.UpdateSelectionCursor(position);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0013517E File Offset: 0x0013417E
		private void OnAdornerMouseLeaveEvent(object sender, MouseEventArgs args)
		{
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0013518C File Offset: 0x0013418C
		private InkCanvasSelectionHitResult HitTestOnSelectionAdorner(Point position)
		{
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = InkCanvasSelectionHitResult.None;
			if (base.InkCanvas.InkCanvasSelection.HasSelection)
			{
				inkCanvasSelectionHitResult = base.InkCanvas.SelectionAdorner.SelectionHandleHitTest(position);
				if (inkCanvasSelectionHitResult >= InkCanvasSelectionHitResult.TopLeft && inkCanvasSelectionHitResult <= InkCanvasSelectionHitResult.Left)
				{
					inkCanvasSelectionHitResult = (base.InkCanvas.ResizeEnabled ? inkCanvasSelectionHitResult : InkCanvasSelectionHitResult.None);
				}
				else if (inkCanvasSelectionHitResult == InkCanvasSelectionHitResult.Selection)
				{
					inkCanvasSelectionHitResult = (base.InkCanvas.MoveEnabled ? inkCanvasSelectionHitResult : InkCanvasSelectionHitResult.None);
				}
			}
			return inkCanvasSelectionHitResult;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x001351F4 File Offset: 0x001341F4
		private void UpdateSelectionCursor(Point hitPoint)
		{
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = this.HitTestOnSelectionAdorner(hitPoint);
			if (this._hitResult != inkCanvasSelectionHitResult)
			{
				this._hitResult = inkCanvasSelectionHitResult;
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
			}
		}

		// Token: 0x040009DD RID: 2525
		private InkCanvasSelectionHitResult _hitResult;
	}
}
