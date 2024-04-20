using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x0200018E RID: 398
	internal sealed class SelectionEditingBehavior : EditingBehavior
	{
		// Token: 0x06000D47 RID: 3399 RVA: 0x001348B3 File Offset: 0x001338B3
		internal SelectionEditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x001348C0 File Offset: 0x001338C0
		protected override void OnActivate()
		{
			this._actionStarted = false;
			this.InitializeCapture();
			MouseDevice primaryDevice = Mouse.PrimaryDevice;
			this._hitResult = base.InkCanvas.SelectionAdorner.SelectionHandleHitTest(primaryDevice.GetPosition(base.InkCanvas.SelectionAdorner));
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
			this._selectionRect = base.InkCanvas.GetSelectionBounds();
			this._previousLocation = primaryDevice.GetPosition(base.InkCanvas.SelectionAdorner);
			this._previousRect = this._selectionRect;
			base.InkCanvas.InkCanvasSelection.StartFeedbackAdorner(this._selectionRect, this._hitResult);
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(this.OnMouseUp));
			base.InkCanvas.SelectionAdorner.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnMouseMove));
			base.InkCanvas.SelectionAdorner.AddHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnLostMouseCapture));
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x001349C8 File Offset: 0x001339C8
		protected override void OnDeactivate()
		{
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseUpEvent, new MouseButtonEventHandler(this.OnMouseUp));
			base.InkCanvas.SelectionAdorner.RemoveHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnMouseMove));
			base.InkCanvas.SelectionAdorner.RemoveHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnLostMouseCapture));
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00134A38 File Offset: 0x00133A38
		protected override void OnCommit(bool commit)
		{
			this.ReleaseCapture(true, commit);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00134A42 File Offset: 0x00133A42
		protected override Cursor GetCurrentCursor()
		{
			return PenCursorManager.GetSelectionCursor(this._hitResult, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00134A60 File Offset: 0x00133A60
		private void OnMouseMove(object sender, MouseEventArgs args)
		{
			Point position = args.GetPosition(base.InkCanvas.SelectionAdorner);
			if (!DoubleUtil.AreClose(position.X, this._previousLocation.X) || !DoubleUtil.AreClose(position.Y, this._previousLocation.Y))
			{
				if (!this._actionStarted)
				{
					this._actionStarted = true;
				}
				Rect rect = this.ChangeFeedbackRectangle(position);
				base.InkCanvas.InkCanvasSelection.UpdateFeedbackAdorner(rect);
				this._previousRect = rect;
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00134AE0 File Offset: 0x00133AE0
		private void OnMouseUp(object sender, MouseButtonEventArgs args)
		{
			if (this._actionStarted)
			{
				this._previousRect = this.ChangeFeedbackRectangle(args.GetPosition(base.InkCanvas.SelectionAdorner));
			}
			base.Commit(true);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00134B0E File Offset: 0x00133B0E
		private void OnLostMouseCapture(object sender, MouseEventArgs args)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				this.ReleaseCapture(false, true);
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00134B28 File Offset: 0x00133B28
		private Rect ChangeFeedbackRectangle(Point newPoint)
		{
			if ((this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.Left) && newPoint.X > this._selectionRect.Right - 16.0)
			{
				newPoint.X = this._selectionRect.Right - 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.Right) && newPoint.X < this._selectionRect.Left + 16.0)
			{
				newPoint.X = this._selectionRect.Left + 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.Top) && newPoint.Y > this._selectionRect.Bottom - 16.0)
			{
				newPoint.Y = this._selectionRect.Bottom - 16.0;
			}
			if ((this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.Bottom) && newPoint.Y < this._selectionRect.Top + 16.0)
			{
				newPoint.Y = this._selectionRect.Top + 16.0;
			}
			Rect result = this.CalculateRect(newPoint.X - this._previousLocation.X, newPoint.Y - this._previousLocation.Y);
			if (this._hitResult == InkCanvasSelectionHitResult.BottomRight || this._hitResult == InkCanvasSelectionHitResult.BottomLeft || this._hitResult == InkCanvasSelectionHitResult.TopRight || this._hitResult == InkCanvasSelectionHitResult.TopLeft || this._hitResult == InkCanvasSelectionHitResult.Selection)
			{
				this._previousLocation.X = newPoint.X;
				this._previousLocation.Y = newPoint.Y;
				return result;
			}
			if (this._hitResult == InkCanvasSelectionHitResult.Left || this._hitResult == InkCanvasSelectionHitResult.Right)
			{
				this._previousLocation.X = newPoint.X;
				return result;
			}
			if (this._hitResult == InkCanvasSelectionHitResult.Top || this._hitResult == InkCanvasSelectionHitResult.Bottom)
			{
				this._previousLocation.Y = newPoint.Y;
			}
			return result;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00134D54 File Offset: 0x00133D54
		private Rect CalculateRect(double x, double y)
		{
			Rect rect = this._previousRect;
			switch (this._hitResult)
			{
			case InkCanvasSelectionHitResult.TopLeft:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				break;
			case InkCanvasSelectionHitResult.Top:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				break;
			case InkCanvasSelectionHitResult.TopRight:
				rect = SelectionEditingBehavior.ExtendSelectionTop(rect, y);
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				break;
			case InkCanvasSelectionHitResult.Right:
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				break;
			case InkCanvasSelectionHitResult.BottomRight:
				rect = SelectionEditingBehavior.ExtendSelectionRight(rect, x);
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.Bottom:
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.BottomLeft:
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				rect = SelectionEditingBehavior.ExtendSelectionBottom(rect, y);
				break;
			case InkCanvasSelectionHitResult.Left:
				rect = SelectionEditingBehavior.ExtendSelectionLeft(rect, x);
				break;
			case InkCanvasSelectionHitResult.Selection:
				rect.Offset(x, y);
				break;
			}
			return rect;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00134E18 File Offset: 0x00133E18
		private static Rect ExtendSelectionLeft(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.X += extendBy;
			result.Width -= extendBy;
			return result;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00134E48 File Offset: 0x00133E48
		private static Rect ExtendSelectionTop(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Y += extendBy;
			result.Height -= extendBy;
			return result;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00134E78 File Offset: 0x00133E78
		private static Rect ExtendSelectionRight(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Width += extendBy;
			return result;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00134E98 File Offset: 0x00133E98
		private static Rect ExtendSelectionBottom(Rect rect, double extendBy)
		{
			Rect result = rect;
			result.Height += extendBy;
			return result;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00134EB7 File Offset: 0x00133EB7
		private void InitializeCapture()
		{
			base.EditingCoordinator.UserIsEditing = true;
			base.InkCanvas.SelectionAdorner.CaptureMouse();
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00134ED8 File Offset: 0x00133ED8
		private void ReleaseCapture(bool releaseDevice, bool commit)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = false;
				if (releaseDevice)
				{
					base.InkCanvas.SelectionAdorner.ReleaseMouseCapture();
				}
				base.SelfDeactivate();
				base.InkCanvas.InkCanvasSelection.EndFeedbackAdorner(commit ? this._previousRect : this._selectionRect);
			}
		}

		// Token: 0x040009D7 RID: 2519
		private const double MinimumHeightWidthSize = 16.0;

		// Token: 0x040009D8 RID: 2520
		private Point _previousLocation;

		// Token: 0x040009D9 RID: 2521
		private Rect _previousRect;

		// Token: 0x040009DA RID: 2522
		private Rect _selectionRect;

		// Token: 0x040009DB RID: 2523
		private InkCanvasSelectionHitResult _hitResult;

		// Token: 0x040009DC RID: 2524
		private bool _actionStarted;
	}
}
