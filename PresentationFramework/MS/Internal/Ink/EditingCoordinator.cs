using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000183 RID: 387
	internal class EditingCoordinator
	{
		// Token: 0x06000C9C RID: 3228 RVA: 0x00130974 File Offset: 0x0012F974
		internal EditingCoordinator(InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._activationStack = new Stack<EditingBehavior>(2);
			this._inkCanvas.AddHandler(Stylus.StylusInRangeEvent, new StylusEventHandler(this.OnInkCanvasStylusInAirOrInRangeMove));
			this._inkCanvas.AddHandler(Stylus.StylusInAirMoveEvent, new StylusEventHandler(this.OnInkCanvasStylusInAirOrInRangeMove));
			this._inkCanvas.AddHandler(Stylus.StylusOutOfRangeEvent, new StylusEventHandler(this.OnInkCanvasStylusOutOfRange));
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00130A0C File Offset: 0x0012FA0C
		internal void ActivateDynamicBehavior(EditingBehavior dynamicBehavior, InputDevice inputDevice)
		{
			this.PushEditingBehavior(dynamicBehavior);
			if (dynamicBehavior == this.LassoSelectionBehavior)
			{
				bool flag = false;
				try
				{
					this.InitializeCapture(inputDevice, (IStylusEditing)dynamicBehavior, true, false);
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.ActiveEditingBehavior.Commit(false);
						this.ReleaseCapture(true);
					}
				}
			}
			this._inkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, this._inkCanvas));
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00130A80 File Offset: 0x0012FA80
		internal void DeactivateDynamicBehavior()
		{
			this.PopEditingBehavior();
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00130A88 File Offset: 0x0012FA88
		internal void UpdateActiveEditingState()
		{
			this.UpdateEditingState(this._stylusIsInverted);
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00130A98 File Offset: 0x0012FA98
		internal void UpdateEditingState(bool inverted)
		{
			if (inverted != this._stylusIsInverted)
			{
				return;
			}
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			EditingBehavior behavior = this.GetBehavior(this.ActiveEditingMode);
			if (this.UserIsEditing)
			{
				if (this.IsInMidStroke)
				{
					((StylusEditingBehavior)activeEditingBehavior).SwitchToMode(this.ActiveEditingMode);
				}
				else
				{
					if (activeEditingBehavior == this.SelectionEditingBehavior)
					{
						activeEditingBehavior.Commit(true);
					}
					this.ChangeEditingBehavior(behavior);
				}
			}
			else if (this.IsInMidStroke)
			{
				((StylusEditingBehavior)activeEditingBehavior).SwitchToMode(this.ActiveEditingMode);
			}
			else
			{
				this.ChangeEditingBehavior(behavior);
			}
			this._inkCanvas.UpdateCursor();
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00130B2C File Offset: 0x0012FB2C
		internal void UpdatePointEraserCursor()
		{
			if (this.ActiveEditingMode == InkCanvasEditingMode.EraseByPoint)
			{
				this.InvalidateBehaviorCursor(this.EraserBehavior);
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00130B43 File Offset: 0x0012FB43
		internal void InvalidateTransform()
		{
			this.SetTransformValid(this.InkCollectionBehavior, false);
			this.SetTransformValid(this.EraserBehavior, false);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00130B5F File Offset: 0x0012FB5F
		internal void InvalidateBehaviorCursor(EditingBehavior behavior)
		{
			this.SetCursorValid(behavior, false);
			if (!this.GetTransformValid(behavior))
			{
				behavior.UpdateTransform();
				this.SetTransformValid(behavior, true);
			}
			if (behavior == this.ActiveEditingBehavior)
			{
				this._inkCanvas.UpdateCursor();
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00130B94 File Offset: 0x0012FB94
		internal bool IsCursorValid(EditingBehavior behavior)
		{
			return this.GetCursorValid(behavior);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00130B9D File Offset: 0x0012FB9D
		internal bool IsTransformValid(EditingBehavior behavior)
		{
			return this.GetTransformValid(behavior);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00130BA8 File Offset: 0x0012FBA8
		internal IStylusEditing ChangeStylusEditingMode(StylusEditingBehavior sourceBehavior, InkCanvasEditingMode newMode)
		{
			if (this.IsInMidStroke && ((sourceBehavior != this.LassoSelectionBehavior && sourceBehavior == this.ActiveEditingBehavior) || (sourceBehavior == this.LassoSelectionBehavior && this.SelectionEditor == this.ActiveEditingBehavior)))
			{
				this.PopEditingBehavior();
				EditingBehavior behavior = this.GetBehavior(this.ActiveEditingMode);
				if (behavior != null)
				{
					this.PushEditingBehavior(behavior);
					if (newMode == InkCanvasEditingMode.Select && behavior == this.SelectionEditor)
					{
						this.PushEditingBehavior(this.LassoSelectionBehavior);
					}
				}
				else
				{
					this.ReleaseCapture(true);
				}
				this._inkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, this._inkCanvas));
				return this.ActiveEditingBehavior as IStylusEditing;
			}
			return null;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		internal void DebugCheckActiveBehavior(EditingBehavior behavior)
		{
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		private void DebugCheckDynamicBehavior(EditingBehavior behavior)
		{
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		[Conditional("DEBUG")]
		private void DebugCheckNonDynamicBehavior(EditingBehavior behavior)
		{
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00130C50 File Offset: 0x0012FC50
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x00130C58 File Offset: 0x0012FC58
		internal bool MoveEnabled
		{
			get
			{
				return this._moveEnabled;
			}
			set
			{
				this._moveEnabled = value;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00130C61 File Offset: 0x0012FC61
		// (set) Token: 0x06000CAD RID: 3245 RVA: 0x00130C69 File Offset: 0x0012FC69
		internal bool UserIsEditing
		{
			get
			{
				return this._userIsEditing;
			}
			set
			{
				this._userIsEditing = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00130C74 File Offset: 0x0012FC74
		internal bool StylusOrMouseIsDown
		{
			get
			{
				bool flag = false;
				StylusDevice currentStylusDevice = Stylus.CurrentStylusDevice;
				if (currentStylusDevice != null && this._inkCanvas.IsStylusOver && !currentStylusDevice.InAir)
				{
					flag = true;
				}
				bool flag2 = this._inkCanvas.IsMouseOver && Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed;
				return flag || flag2;
			}
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00130CC9 File Offset: 0x0012FCC9
		internal InputDevice GetInputDeviceForReset()
		{
			if (this._capturedStylus != null && !this._capturedStylus.InAir)
			{
				return this._capturedStylus;
			}
			if (this._capturedMouse != null && this._capturedMouse.LeftButton == MouseButtonState.Pressed)
			{
				return this._capturedMouse;
			}
			return null;
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00130D05 File Offset: 0x0012FD05
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x00130D0D File Offset: 0x0012FD0D
		internal bool ResizeEnabled
		{
			get
			{
				return this._resizeEnabled;
			}
			set
			{
				this._resizeEnabled = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00130D16 File Offset: 0x0012FD16
		internal LassoSelectionBehavior LassoSelectionBehavior
		{
			get
			{
				if (this._lassoSelectionBehavior == null)
				{
					this._lassoSelectionBehavior = new LassoSelectionBehavior(this, this._inkCanvas);
				}
				return this._lassoSelectionBehavior;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00130D38 File Offset: 0x0012FD38
		internal SelectionEditingBehavior SelectionEditingBehavior
		{
			get
			{
				if (this._selectionEditingBehavior == null)
				{
					this._selectionEditingBehavior = new SelectionEditingBehavior(this, this._inkCanvas);
				}
				return this._selectionEditingBehavior;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00130D5A File Offset: 0x0012FD5A
		internal InkCanvasEditingMode ActiveEditingMode
		{
			get
			{
				if (this._stylusIsInverted)
				{
					return this._inkCanvas.EditingModeInverted;
				}
				return this._inkCanvas.EditingMode;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00130D7B File Offset: 0x0012FD7B
		internal SelectionEditor SelectionEditor
		{
			get
			{
				if (this._selectionEditor == null)
				{
					this._selectionEditor = new SelectionEditor(this, this._inkCanvas);
				}
				return this._selectionEditor;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00130D9D File Offset: 0x0012FD9D
		internal bool IsInMidStroke
		{
			get
			{
				return this._capturedStylus != null || this._capturedMouse != null;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00130DB2 File Offset: 0x0012FDB2
		internal bool IsStylusInverted
		{
			get
			{
				return this._stylusIsInverted;
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00130DBC File Offset: 0x0012FDBC
		private EditingBehavior GetBehavior(InkCanvasEditingMode editingMode)
		{
			EditingBehavior result;
			switch (editingMode)
			{
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				result = this.InkCollectionBehavior;
				break;
			case InkCanvasEditingMode.Select:
				result = this.SelectionEditor;
				break;
			case InkCanvasEditingMode.EraseByPoint:
			case InkCanvasEditingMode.EraseByStroke:
				result = this.EraserBehavior;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x00130E0C File Offset: 0x0012FE0C
		private void PushEditingBehavior(EditingBehavior newEditingBehavior)
		{
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			if (activeEditingBehavior != null)
			{
				activeEditingBehavior.Deactivate();
			}
			this._activationStack.Push(newEditingBehavior);
			newEditingBehavior.Activate();
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x00130E3C File Offset: 0x0012FE3C
		private void PopEditingBehavior()
		{
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			if (activeEditingBehavior != null)
			{
				activeEditingBehavior.Deactivate();
				this._activationStack.Pop();
				activeEditingBehavior = this.ActiveEditingBehavior;
				if (this.ActiveEditingBehavior != null)
				{
					activeEditingBehavior.Activate();
				}
			}
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x00130E7C File Offset: 0x0012FE7C
		private void OnInkCanvasStylusInAirOrInRangeMove(object sender, StylusEventArgs args)
		{
			if (this._capturedMouse != null)
			{
				if (this.ActiveEditingBehavior == this.InkCollectionBehavior && this._inkCanvas.InternalDynamicRenderer != null)
				{
					this._inkCanvas.InternalDynamicRenderer.Enabled = false;
					this._inkCanvas.InternalDynamicRenderer.Enabled = true;
				}
				this.ActiveEditingBehavior.Commit(true);
				this.ReleaseCapture(true);
			}
			this.UpdateInvertedState(args.StylusDevice, args.Inverted);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00130EF4 File Offset: 0x0012FEF4
		private void OnInkCanvasStylusOutOfRange(object sender, StylusEventArgs args)
		{
			this.UpdateInvertedState(args.StylusDevice, false);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00130F04 File Offset: 0x0012FF04
		internal void OnInkCanvasDeviceDown(object sender, InputEventArgs args)
		{
			MouseButtonEventArgs mouseButtonEventArgs = args as MouseButtonEventArgs;
			bool resetDynamicRenderer = false;
			if (mouseButtonEventArgs != null)
			{
				if (this._inkCanvas.Focus() && this.ActiveEditingMode != InkCanvasEditingMode.None)
				{
					mouseButtonEventArgs.Handled = true;
				}
				if (mouseButtonEventArgs.ChangedButton != MouseButton.Left)
				{
					return;
				}
				if (mouseButtonEventArgs.StylusDevice != null)
				{
					this.UpdateInvertedState(mouseButtonEventArgs.StylusDevice, mouseButtonEventArgs.StylusDevice.Inverted);
				}
			}
			else
			{
				StylusEventArgs stylusEventArgs = args as StylusEventArgs;
				this.UpdateInvertedState(stylusEventArgs.StylusDevice, stylusEventArgs.Inverted);
			}
			IStylusEditing stylusEditing = this.ActiveEditingBehavior as IStylusEditing;
			if (!this.IsInMidStroke && stylusEditing != null)
			{
				bool flag = false;
				try
				{
					InputDevice inputDevice;
					if (mouseButtonEventArgs != null && mouseButtonEventArgs.StylusDevice != null)
					{
						inputDevice = mouseButtonEventArgs.StylusDevice;
						resetDynamicRenderer = true;
					}
					else
					{
						inputDevice = args.Device;
					}
					this.InitializeCapture(inputDevice, stylusEditing, args.UserInitiated, resetDynamicRenderer);
					flag = true;
				}
				finally
				{
					if (!flag)
					{
						this.ActiveEditingBehavior.Commit(false);
						this.ReleaseCapture(this.IsInMidStroke);
					}
				}
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00130FFC File Offset: 0x0012FFFC
		private void OnInkCanvasDeviceMove<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			if (this.IsInputDeviceCaptured(args.Device))
			{
				IStylusEditing stylusEditing = this.ActiveEditingBehavior as IStylusEditing;
				if (stylusEditing != null)
				{
					StylusPointCollection stylusPoints;
					if (this._capturedStylus != null)
					{
						stylusPoints = this._capturedStylus.GetStylusPoints(this._inkCanvas, this._commonDescription);
					}
					else
					{
						MouseEventArgs mouseEventArgs = args as MouseEventArgs;
						if (mouseEventArgs != null && mouseEventArgs.StylusDevice != null)
						{
							return;
						}
						stylusPoints = new StylusPointCollection(new Point[]
						{
							this._capturedMouse.GetPosition(this._inkCanvas)
						});
					}
					bool flag = false;
					try
					{
						stylusEditing.AddStylusPoints(stylusPoints, args.UserInitiated);
						flag = true;
					}
					finally
					{
						if (!flag)
						{
							this.ActiveEditingBehavior.Commit(false);
							this.ReleaseCapture(true);
						}
					}
				}
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x001310D0 File Offset: 0x001300D0
		internal void OnInkCanvasDeviceUp(object sender, InputEventArgs args)
		{
			MouseButtonEventArgs mouseButtonEventArgs = args as MouseButtonEventArgs;
			StylusDevice stylusDevice = null;
			if (mouseButtonEventArgs != null)
			{
				stylusDevice = mouseButtonEventArgs.StylusDevice;
			}
			if (this.IsInputDeviceCaptured(args.Device) || (stylusDevice != null && this.IsInputDeviceCaptured(stylusDevice)))
			{
				if (this._capturedMouse != null && mouseButtonEventArgs != null && mouseButtonEventArgs.ChangedButton != MouseButton.Left)
				{
					return;
				}
				try
				{
					if (this.ActiveEditingBehavior != null)
					{
						this.ActiveEditingBehavior.Commit(true);
					}
				}
				finally
				{
					this.ReleaseCapture(true);
				}
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00131150 File Offset: 0x00130150
		private void OnInkCanvasLostDeviceCapture<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			if (this.UserIsEditing)
			{
				this.ReleaseCapture(false);
				if (this.ActiveEditingBehavior == this.InkCollectionBehavior && this._inkCanvas.InternalDynamicRenderer != null)
				{
					this._inkCanvas.InternalDynamicRenderer.Enabled = false;
					this._inkCanvas.InternalDynamicRenderer.Enabled = true;
				}
				this.ActiveEditingBehavior.Commit(true);
			}
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x001311B8 File Offset: 0x001301B8
		private void InitializeCapture(InputDevice inputDevice, IStylusEditing stylusEditingBehavior, bool userInitiated, bool resetDynamicRenderer)
		{
			this._capturedStylus = (inputDevice as StylusDevice);
			this._capturedMouse = (inputDevice as MouseDevice);
			if (this._capturedStylus != null)
			{
				StylusPointCollection stylusPoints = this._capturedStylus.GetStylusPoints(this._inkCanvas);
				this._commonDescription = StylusPointDescription.GetCommonDescription(this._inkCanvas.DefaultStylusPointDescription, stylusPoints.Description);
				StylusPointCollection stylusPoints2 = stylusPoints.Reformat(this._commonDescription);
				if (resetDynamicRenderer)
				{
					InkCollectionBehavior inkCollectionBehavior = stylusEditingBehavior as InkCollectionBehavior;
					if (inkCollectionBehavior != null)
					{
						inkCollectionBehavior.ResetDynamicRenderer();
					}
				}
				stylusEditingBehavior.AddStylusPoints(stylusPoints2, userInitiated);
				this._inkCanvas.CaptureStylus();
				if (this._inkCanvas.IsStylusCaptured && this.ActiveEditingMode != InkCanvasEditingMode.None)
				{
					this._inkCanvas.AddHandler(Stylus.StylusMoveEvent, new StylusEventHandler(this.OnInkCanvasDeviceMove<StylusEventArgs>));
					this._inkCanvas.AddHandler(UIElement.LostStylusCaptureEvent, new StylusEventHandler(this.OnInkCanvasLostDeviceCapture<StylusEventArgs>));
					return;
				}
				this._capturedStylus = null;
				return;
			}
			else
			{
				this._commonDescription = null;
				StylusPointCollection stylusPoints2 = new StylusPointCollection(new Point[]
				{
					this._capturedMouse.GetPosition(this._inkCanvas)
				});
				stylusEditingBehavior.AddStylusPoints(stylusPoints2, userInitiated);
				this._inkCanvas.CaptureMouse();
				if (this._inkCanvas.IsMouseCaptured && this.ActiveEditingMode != InkCanvasEditingMode.None)
				{
					this._inkCanvas.AddHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnInkCanvasDeviceMove<MouseEventArgs>));
					this._inkCanvas.AddHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnInkCanvasLostDeviceCapture<MouseEventArgs>));
					return;
				}
				this._capturedMouse = null;
				return;
			}
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00131338 File Offset: 0x00130338
		private void ReleaseCapture(bool releaseDevice)
		{
			if (this._capturedStylus != null)
			{
				this._commonDescription = null;
				this._inkCanvas.RemoveHandler(Stylus.StylusMoveEvent, new StylusEventHandler(this.OnInkCanvasDeviceMove<StylusEventArgs>));
				this._inkCanvas.RemoveHandler(UIElement.LostStylusCaptureEvent, new StylusEventHandler(this.OnInkCanvasLostDeviceCapture<StylusEventArgs>));
				this._capturedStylus = null;
				if (releaseDevice)
				{
					this._inkCanvas.ReleaseStylusCapture();
					return;
				}
			}
			else if (this._capturedMouse != null)
			{
				this._inkCanvas.RemoveHandler(Mouse.MouseMoveEvent, new MouseEventHandler(this.OnInkCanvasDeviceMove<MouseEventArgs>));
				this._inkCanvas.RemoveHandler(UIElement.LostMouseCaptureEvent, new MouseEventHandler(this.OnInkCanvasLostDeviceCapture<MouseEventArgs>));
				this._capturedMouse = null;
				if (releaseDevice)
				{
					this._inkCanvas.ReleaseMouseCapture();
				}
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x001313F7 File Offset: 0x001303F7
		private bool IsInputDeviceCaptured(InputDevice inputDevice)
		{
			return (inputDevice == this._capturedStylus && ((StylusDevice)inputDevice).Captured == this._inkCanvas) || (inputDevice == this._capturedMouse && ((MouseDevice)inputDevice).Captured == this._inkCanvas);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00131438 File Offset: 0x00130438
		internal Cursor GetActiveBehaviorCursor()
		{
			EditingBehavior activeEditingBehavior = this.ActiveEditingBehavior;
			if (activeEditingBehavior == null)
			{
				return Cursors.Arrow;
			}
			Cursor cursor = activeEditingBehavior.Cursor;
			if (!this.GetCursorValid(activeEditingBehavior))
			{
				this.SetCursorValid(activeEditingBehavior, true);
			}
			return cursor;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0013146C File Offset: 0x0013046C
		private bool GetCursorValid(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag behaviorCursorFlag = this.GetBehaviorCursorFlag(behavior);
			return this.GetBitFlag(behaviorCursorFlag);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00131488 File Offset: 0x00130488
		private void SetCursorValid(EditingBehavior behavior, bool isValid)
		{
			EditingCoordinator.BehaviorValidFlag behaviorCursorFlag = this.GetBehaviorCursorFlag(behavior);
			this.SetBitFlag(behaviorCursorFlag, isValid);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x001314A8 File Offset: 0x001304A8
		private bool GetTransformValid(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag behaviorTransformFlag = this.GetBehaviorTransformFlag(behavior);
			return this.GetBitFlag(behaviorTransformFlag);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x001314C4 File Offset: 0x001304C4
		private void SetTransformValid(EditingBehavior behavior, bool isValid)
		{
			EditingCoordinator.BehaviorValidFlag behaviorTransformFlag = this.GetBehaviorTransformFlag(behavior);
			this.SetBitFlag(behaviorTransformFlag, isValid);
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x001314E1 File Offset: 0x001304E1
		private bool GetBitFlag(EditingCoordinator.BehaviorValidFlag flag)
		{
			return (this._behaviorValidFlag & flag) > (EditingCoordinator.BehaviorValidFlag)0;
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x001314EE File Offset: 0x001304EE
		private void SetBitFlag(EditingCoordinator.BehaviorValidFlag flag, bool value)
		{
			if (value)
			{
				this._behaviorValidFlag |= flag;
				return;
			}
			this._behaviorValidFlag &= ~flag;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00131514 File Offset: 0x00130514
		private EditingCoordinator.BehaviorValidFlag GetBehaviorCursorFlag(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag result = (EditingCoordinator.BehaviorValidFlag)0;
			if (behavior == this.InkCollectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.InkCollectionBehaviorCursorValid;
			}
			else if (behavior == this.EraserBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.EraserBehaviorCursorValid;
			}
			else if (behavior == this.LassoSelectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.LassoSelectionBehaviorCursorValid;
			}
			else if (behavior == this.SelectionEditingBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditingBehaviorCursorValid;
			}
			else if (behavior == this.SelectionEditor)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditorCursorValid;
			}
			return result;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00131564 File Offset: 0x00130564
		private EditingCoordinator.BehaviorValidFlag GetBehaviorTransformFlag(EditingBehavior behavior)
		{
			EditingCoordinator.BehaviorValidFlag result = (EditingCoordinator.BehaviorValidFlag)0;
			if (behavior == this.InkCollectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.InkCollectionBehaviorTransformValid;
			}
			else if (behavior == this.EraserBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.EraserBehaviorTransformValid;
			}
			else if (behavior == this.LassoSelectionBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.LassoSelectionBehaviorTransformValid;
			}
			else if (behavior == this.SelectionEditingBehavior)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditingBehaviorTransformValid;
			}
			else if (behavior == this.SelectionEditor)
			{
				result = EditingCoordinator.BehaviorValidFlag.SelectionEditorTransformValid;
			}
			return result;
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x001315C4 File Offset: 0x001305C4
		private void ChangeEditingBehavior(EditingBehavior newBehavior)
		{
			try
			{
				this._inkCanvas.ClearSelection(true);
			}
			finally
			{
				if (this.ActiveEditingBehavior != null)
				{
					this.PopEditingBehavior();
				}
				if (newBehavior != null)
				{
					this.PushEditingBehavior(newBehavior);
				}
				this._inkCanvas.RaiseActiveEditingModeChanged(new RoutedEventArgs(InkCanvas.ActiveEditingModeChangedEvent, this._inkCanvas));
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00131624 File Offset: 0x00130624
		private bool UpdateInvertedState(StylusDevice stylusDevice, bool stylusIsInverted)
		{
			if ((!this.IsInMidStroke || (this.IsInMidStroke && this.IsInputDeviceCaptured(stylusDevice))) && stylusIsInverted != this._stylusIsInverted)
			{
				this._stylusIsInverted = stylusIsInverted;
				this.UpdateActiveEditingState();
				return true;
			}
			return false;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00131658 File Offset: 0x00130658
		private EditingBehavior ActiveEditingBehavior
		{
			get
			{
				EditingBehavior result = null;
				if (this._activationStack.Count > 0)
				{
					result = this._activationStack.Peek();
				}
				return result;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00131682 File Offset: 0x00130682
		internal InkCollectionBehavior InkCollectionBehavior
		{
			get
			{
				if (this._inkCollectionBehavior == null)
				{
					this._inkCollectionBehavior = new InkCollectionBehavior(this, this._inkCanvas);
				}
				return this._inkCollectionBehavior;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x001316A4 File Offset: 0x001306A4
		private EraserBehavior EraserBehavior
		{
			get
			{
				if (this._eraserBehavior == null)
				{
					this._eraserBehavior = new EraserBehavior(this, this._inkCanvas);
				}
				return this._eraserBehavior;
			}
		}

		// Token: 0x04000995 RID: 2453
		private InkCanvas _inkCanvas;

		// Token: 0x04000996 RID: 2454
		private Stack<EditingBehavior> _activationStack;

		// Token: 0x04000997 RID: 2455
		private InkCollectionBehavior _inkCollectionBehavior;

		// Token: 0x04000998 RID: 2456
		private EraserBehavior _eraserBehavior;

		// Token: 0x04000999 RID: 2457
		private LassoSelectionBehavior _lassoSelectionBehavior;

		// Token: 0x0400099A RID: 2458
		private SelectionEditingBehavior _selectionEditingBehavior;

		// Token: 0x0400099B RID: 2459
		private SelectionEditor _selectionEditor;

		// Token: 0x0400099C RID: 2460
		private bool _moveEnabled = true;

		// Token: 0x0400099D RID: 2461
		private bool _resizeEnabled = true;

		// Token: 0x0400099E RID: 2462
		private bool _userIsEditing;

		// Token: 0x0400099F RID: 2463
		private bool _stylusIsInverted;

		// Token: 0x040009A0 RID: 2464
		private StylusPointDescription _commonDescription;

		// Token: 0x040009A1 RID: 2465
		private StylusDevice _capturedStylus;

		// Token: 0x040009A2 RID: 2466
		private MouseDevice _capturedMouse;

		// Token: 0x040009A3 RID: 2467
		private EditingCoordinator.BehaviorValidFlag _behaviorValidFlag;

		// Token: 0x020009C5 RID: 2501
		[Flags]
		private enum BehaviorValidFlag
		{
			// Token: 0x04003F8F RID: 16271
			InkCollectionBehaviorCursorValid = 1,
			// Token: 0x04003F90 RID: 16272
			EraserBehaviorCursorValid = 2,
			// Token: 0x04003F91 RID: 16273
			LassoSelectionBehaviorCursorValid = 4,
			// Token: 0x04003F92 RID: 16274
			SelectionEditingBehaviorCursorValid = 8,
			// Token: 0x04003F93 RID: 16275
			SelectionEditorCursorValid = 16,
			// Token: 0x04003F94 RID: 16276
			InkCollectionBehaviorTransformValid = 32,
			// Token: 0x04003F95 RID: 16277
			EraserBehaviorTransformValid = 64,
			// Token: 0x04003F96 RID: 16278
			LassoSelectionBehaviorTransformValid = 128,
			// Token: 0x04003F97 RID: 16279
			SelectionEditingBehaviorTransformValid = 256,
			// Token: 0x04003F98 RID: 16280
			SelectionEditorTransformValid = 512
		}
	}
}
