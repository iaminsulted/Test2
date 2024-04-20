using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x02000185 RID: 389
	internal sealed class EraserBehavior : StylusEditingBehavior
	{
		// Token: 0x06000CD7 RID: 3287 RVA: 0x0013170C File Offset: 0x0013070C
		internal EraserBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00131718 File Offset: 0x00130718
		protected override void OnSwitchToMode(InkCanvasEditingMode mode)
		{
			switch (mode)
			{
			case InkCanvasEditingMode.None:
				base.Commit(true);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				break;
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				base.Commit(true);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			case InkCanvasEditingMode.Select:
			{
				StylusPointCollection stylusPointCollection = (this._stylusPoints != null) ? this._stylusPoints.Clone() : null;
				base.Commit(true);
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
				base.Commit(true);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x001317C4 File Offset: 0x001307C4
		protected override void OnActivate()
		{
			base.OnActivate();
			InkCanvasEditingMode activeEditingMode = base.EditingCoordinator.ActiveEditingMode;
			if (this._cachedEraseMode != activeEditingMode)
			{
				this._cachedEraseMode = activeEditingMode;
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
				return;
			}
			if (activeEditingMode == InkCanvasEditingMode.EraseByPoint)
			{
				bool flag = this._cachedStylusShape != null;
				if (flag && (this._cachedStylusShape.Width != base.InkCanvas.EraserShape.Width || this._cachedStylusShape.Height != base.InkCanvas.EraserShape.Height || this._cachedStylusShape.Rotation != base.InkCanvas.EraserShape.Rotation || this._cachedStylusShape.GetType() != base.InkCanvas.EraserShape.GetType()))
				{
					this.ResetCachedPointEraserCursor();
					flag = false;
				}
				if (!flag)
				{
					base.EditingCoordinator.InvalidateBehaviorCursor(this);
				}
			}
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x001318A8 File Offset: 0x001308A8
		protected override void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._incrementalStrokeHitTester = base.InkCanvas.Strokes.GetIncrementalStrokeHitTester(base.InkCanvas.EraserShape);
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				this._incrementalStrokeHitTester.StrokeHit += this.OnPointEraseResultChanged;
			}
			else
			{
				this._incrementalStrokeHitTester.StrokeHit += this.OnStrokeEraseResultChanged;
			}
			this._stylusPoints = new StylusPointCollection(stylusPoints.Description, 100);
			this._stylusPoints.Add(stylusPoints);
			this._incrementalStrokeHitTester.AddPoints(stylusPoints);
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				base.EditingCoordinator.InvalidateBehaviorCursor(this);
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0013194F File Offset: 0x0013094F
		protected override void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._stylusPoints.Add(stylusPoints);
			this._incrementalStrokeHitTester.AddPoints(stylusPoints);
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0013196C File Offset: 0x0013096C
		protected override void StylusInputEnd(bool commit)
		{
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				this._incrementalStrokeHitTester.StrokeHit -= this.OnPointEraseResultChanged;
			}
			else
			{
				this._incrementalStrokeHitTester.StrokeHit -= this.OnStrokeEraseResultChanged;
			}
			this._stylusPoints = null;
			this._incrementalStrokeHitTester.EndHitTesting();
			this._incrementalStrokeHitTester = null;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x001319CC File Offset: 0x001309CC
		protected override Cursor GetCurrentCursor()
		{
			if (InkCanvasEditingMode.EraseByPoint == this._cachedEraseMode)
			{
				if (this._cachedPointEraserCursor == null)
				{
					this._cachedStylusShape = base.InkCanvas.EraserShape;
					Matrix tranform = base.GetElementTransformMatrix();
					if (!tranform.IsIdentity)
					{
						if (tranform.HasInverse)
						{
							tranform.OffsetX = 0.0;
							tranform.OffsetY = 0.0;
						}
						else
						{
							tranform = Matrix.Identity;
						}
					}
					DpiScale dpi = base.InkCanvas.GetDpi();
					this._cachedPointEraserCursor = PenCursorManager.GetPointEraserCursor(this._cachedStylusShape, tranform, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				return this._cachedPointEraserCursor;
			}
			return PenCursorManager.GetStrokeEraserCursor();
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00131A77 File Offset: 0x00130A77
		protected override void OnTransformChanged()
		{
			this.ResetCachedPointEraserCursor();
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00131A7F File Offset: 0x00130A7F
		private void ResetCachedPointEraserCursor()
		{
			this._cachedPointEraserCursor = null;
			this._cachedStylusShape = null;
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00131A90 File Offset: 0x00130A90
		private void OnStrokeEraseResultChanged(object sender, StrokeHitEventArgs e)
		{
			bool flag = false;
			try
			{
				InkCanvasStrokeErasingEventArgs inkCanvasStrokeErasingEventArgs = new InkCanvasStrokeErasingEventArgs(e.HitStroke);
				base.InkCanvas.RaiseStrokeErasing(inkCanvasStrokeErasingEventArgs);
				if (!inkCanvasStrokeErasingEventArgs.Cancel)
				{
					base.InkCanvas.Strokes.Remove(e.HitStroke);
					base.InkCanvas.RaiseInkErased();
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					base.Commit(false);
				}
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x00131B00 File Offset: 0x00130B00
		private void OnPointEraseResultChanged(object sender, StrokeHitEventArgs e)
		{
			bool flag = false;
			try
			{
				InkCanvasStrokeErasingEventArgs inkCanvasStrokeErasingEventArgs = new InkCanvasStrokeErasingEventArgs(e.HitStroke);
				base.InkCanvas.RaiseStrokeErasing(inkCanvasStrokeErasingEventArgs);
				if (!inkCanvasStrokeErasingEventArgs.Cancel)
				{
					StrokeCollection pointEraseResults = e.GetPointEraseResults();
					StrokeCollection strokeCollection = new StrokeCollection();
					strokeCollection.Add(e.HitStroke);
					try
					{
						if (pointEraseResults.Count > 0)
						{
							base.InkCanvas.Strokes.Replace(strokeCollection, pointEraseResults);
						}
						else
						{
							base.InkCanvas.Strokes.Remove(strokeCollection);
						}
					}
					catch (ArgumentException ex)
					{
						if (!ex.Data.Contains("System.Windows.Ink.StrokeCollection"))
						{
							throw;
						}
					}
					base.InkCanvas.RaiseInkErased();
				}
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					base.Commit(false);
				}
			}
		}

		// Token: 0x040009A5 RID: 2469
		private InkCanvasEditingMode _cachedEraseMode;

		// Token: 0x040009A6 RID: 2470
		private IncrementalStrokeHitTester _incrementalStrokeHitTester;

		// Token: 0x040009A7 RID: 2471
		private Cursor _cachedPointEraserCursor;

		// Token: 0x040009A8 RID: 2472
		private StylusShape _cachedStylusShape;

		// Token: 0x040009A9 RID: 2473
		private StylusPointCollection _stylusPoints;
	}
}
