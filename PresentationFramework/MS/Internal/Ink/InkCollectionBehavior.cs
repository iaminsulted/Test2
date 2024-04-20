using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x02000189 RID: 393
	internal sealed class InkCollectionBehavior : StylusEditingBehavior
	{
		// Token: 0x06000D0E RID: 3342 RVA: 0x00132AC8 File Offset: 0x00131AC8
		internal InkCollectionBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
			this._stylusPoints = null;
			this._userInitiated = false;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00132AE0 File Offset: 0x00131AE0
		internal void ResetDynamicRenderer()
		{
			this._resetDynamicRenderer = true;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00132AEC File Offset: 0x00131AEC
		protected override void OnSwitchToMode(InkCanvasEditingMode mode)
		{
			switch (mode)
			{
			case InkCanvasEditingMode.None:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				break;
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				base.InkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, base.InkCanvas));
				return;
			case InkCanvasEditingMode.Select:
			{
				StylusPointCollection stylusPointCollection = (this._stylusPoints != null) ? this._stylusPoints.Clone() : null;
				base.Commit(false);
				IStylusEditing stylusEditing = base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				if (stylusPointCollection != null && stylusEditing != null)
				{
					stylusEditing.AddStylusPoints(stylusPointCollection, false);
					return;
				}
				break;
			}
			case InkCanvasEditingMode.EraseByPoint:
			case InkCanvasEditingMode.EraseByStroke:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00132BA0 File Offset: 0x00131BA0
		protected override void OnActivate()
		{
			base.OnActivate();
			if (base.InkCanvas.InternalDynamicRenderer != null)
			{
				base.InkCanvas.InternalDynamicRenderer.Enabled = true;
				base.InkCanvas.UpdateDynamicRenderer();
			}
			this._resetDynamicRenderer = base.EditingCoordinator.StylusOrMouseIsDown;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00132BED File Offset: 0x00131BED
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			if (base.InkCanvas.InternalDynamicRenderer != null)
			{
				base.InkCanvas.InternalDynamicRenderer.Enabled = false;
				base.InkCanvas.UpdateDynamicRenderer();
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00132C1E File Offset: 0x00131C1E
		protected override Cursor GetCurrentCursor()
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				return Cursors.None;
			}
			return this.PenCursor;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00132C3C File Offset: 0x00131C3C
		protected override void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._userInitiated = false;
			if (userInitiated)
			{
				this._userInitiated = true;
			}
			this._stylusPoints = new StylusPointCollection(stylusPoints.Description, 100);
			this._stylusPoints.Add(stylusPoints);
			this._strokeDrawingAttributes = base.InkCanvas.DefaultDrawingAttributes.Clone();
			if (this._resetDynamicRenderer)
			{
				InputDevice inputDeviceForReset = base.EditingCoordinator.GetInputDeviceForReset();
				if (base.InkCanvas.InternalDynamicRenderer != null && inputDeviceForReset != null)
				{
					StylusDevice stylusDevice = inputDeviceForReset as StylusDevice;
					base.InkCanvas.InternalDynamicRenderer.Reset(stylusDevice, stylusPoints);
				}
				this._resetDynamicRenderer = false;
			}
			base.EditingCoordinator.InvalidateBehaviorCursor(this);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00132CDF File Offset: 0x00131CDF
		protected override void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (!userInitiated)
			{
				this._userInitiated = false;
			}
			this._stylusPoints.Add(stylusPoints);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00132CF8 File Offset: 0x00131CF8
		protected override void StylusInputEnd(bool commit)
		{
			try
			{
				if (commit && this._stylusPoints != null)
				{
					InkCanvasStrokeCollectedEventArgs e = new InkCanvasStrokeCollectedEventArgs(new Stroke(this._stylusPoints, this._strokeDrawingAttributes));
					base.InkCanvas.RaiseGestureOrStrokeCollected(e, this._userInitiated);
				}
			}
			finally
			{
				this._stylusPoints = null;
				this._strokeDrawingAttributes = null;
				this._userInitiated = false;
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
			}
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00132D70 File Offset: 0x00131D70
		protected override void OnTransformChanged()
		{
			this._cachedPenCursor = null;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x00132D7C File Offset: 0x00131D7C
		private Cursor PenCursor
		{
			get
			{
				if (this._cachedPenCursor == null || this._cursorDrawingAttributes != base.InkCanvas.DefaultDrawingAttributes)
				{
					Matrix matrix = base.GetElementTransformMatrix();
					DrawingAttributes drawingAttributes = base.InkCanvas.DefaultDrawingAttributes;
					if (!matrix.IsIdentity)
					{
						matrix *= drawingAttributes.StylusTipTransform;
						matrix.OffsetX = 0.0;
						matrix.OffsetY = 0.0;
						if (matrix.HasInverse)
						{
							drawingAttributes = drawingAttributes.Clone();
							drawingAttributes.StylusTipTransform = matrix;
						}
					}
					this._cursorDrawingAttributes = base.InkCanvas.DefaultDrawingAttributes.Clone();
					DpiScale dpi = base.InkCanvas.GetDpi();
					this._cachedPenCursor = PenCursorManager.GetPenCursor(drawingAttributes, false, base.InkCanvas.FlowDirection == FlowDirection.RightToLeft, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				return this._cachedPenCursor;
			}
		}

		// Token: 0x040009B5 RID: 2485
		private bool _resetDynamicRenderer;

		// Token: 0x040009B6 RID: 2486
		private StylusPointCollection _stylusPoints;

		// Token: 0x040009B7 RID: 2487
		private bool _userInitiated;

		// Token: 0x040009B8 RID: 2488
		private DrawingAttributes _strokeDrawingAttributes;

		// Token: 0x040009B9 RID: 2489
		private DrawingAttributes _cursorDrawingAttributes;

		// Token: 0x040009BA RID: 2490
		private Cursor _cachedPenCursor;
	}
}
