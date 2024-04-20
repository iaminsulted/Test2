using System;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Win32;

namespace System.Windows.Controls
{
	// Token: 0x020007BE RID: 1982
	internal sealed class PopupControlService
	{
		// Token: 0x06007139 RID: 28985 RVA: 0x002D9603 File Offset: 0x002D8603
		internal PopupControlService()
		{
			InputManager.Current.PostProcessInput += this.OnPostProcessInput;
			this._focusChangedEventHandler = new KeyboardFocusChangedEventHandler(this.OnFocusChanged);
		}

		// Token: 0x0600713A RID: 28986 RVA: 0x002D9634 File Offset: 0x002D8634
		private void OnPostProcessInput(object sender, ProcessInputEventArgs e)
		{
			if (e.StagingItem.Input.RoutedEvent == InputManager.InputReportEvent)
			{
				InputReportEventArgs inputReportEventArgs = (InputReportEventArgs)e.StagingItem.Input;
				if (!inputReportEventArgs.Handled && inputReportEventArgs.Report.Type == InputType.Mouse)
				{
					RawMouseInputReport rawMouseInputReport = (RawMouseInputReport)inputReportEventArgs.Report;
					if ((rawMouseInputReport.Actions & RawMouseActions.AbsoluteMove) == RawMouseActions.AbsoluteMove)
					{
						if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
						{
							this.RaiseToolTipClosingEvent(true);
							return;
						}
						IInputElement inputElement = Mouse.PrimaryDevice.RawDirectlyOver;
						if (inputElement != null)
						{
							Point position = Mouse.PrimaryDevice.GetPosition(inputElement);
							if (Mouse.CapturedMode != CaptureMode.None)
							{
								PresentationSource presentationSource = PresentationSource.CriticalFromVisual((DependencyObject)inputElement);
								UIElement uielement = (presentationSource != null) ? (presentationSource.RootVisual as UIElement) : null;
								if (uielement != null)
								{
									position = Mouse.PrimaryDevice.GetPosition(uielement);
									IInputElement inputElement2;
									uielement.InputHitTest(position, out inputElement2, out inputElement);
									position = Mouse.PrimaryDevice.GetPosition(inputElement);
								}
								else
								{
									inputElement = null;
								}
							}
							if (inputElement != null)
							{
								this.OnMouseMove(inputElement, position);
								return;
							}
						}
					}
					else if ((rawMouseInputReport.Actions & RawMouseActions.Deactivate) == RawMouseActions.Deactivate && this.LastMouseDirectlyOver != null)
					{
						this.LastMouseDirectlyOver = null;
						if (this.LastMouseOverWithToolTip != null)
						{
							this.RaiseToolTipClosingEvent(true);
							if (SafeNativeMethods.GetCapture() == IntPtr.Zero)
							{
								this.LastMouseOverWithToolTip = null;
								return;
							}
						}
					}
				}
			}
			else
			{
				if (e.StagingItem.Input.RoutedEvent == Keyboard.KeyDownEvent)
				{
					this.ProcessKeyDown(sender, (KeyEventArgs)e.StagingItem.Input);
					return;
				}
				if (e.StagingItem.Input.RoutedEvent == Keyboard.KeyUpEvent)
				{
					this.ProcessKeyUp(sender, (KeyEventArgs)e.StagingItem.Input);
					return;
				}
				if (e.StagingItem.Input.RoutedEvent == Mouse.MouseUpEvent)
				{
					this.ProcessMouseUp(sender, (MouseButtonEventArgs)e.StagingItem.Input);
					return;
				}
				if (e.StagingItem.Input.RoutedEvent == Mouse.MouseDownEvent)
				{
					this.RaiseToolTipClosingEvent(true);
				}
			}
		}

		// Token: 0x0600713B RID: 28987 RVA: 0x002D9837 File Offset: 0x002D8837
		private void OnMouseMove(IInputElement directlyOver, Point pt)
		{
			if (directlyOver != this.LastMouseDirectlyOver)
			{
				this.LastMouseDirectlyOver = directlyOver;
				if (directlyOver != this.LastMouseOverWithToolTip)
				{
					this.InspectElementForToolTip(directlyOver as DependencyObject, ToolTip.ToolTipTrigger.Mouse);
				}
			}
		}

		// Token: 0x0600713C RID: 28988 RVA: 0x002D9860 File Offset: 0x002D8860
		private void OnFocusChanged(object sender, KeyboardFocusChangedEventArgs e)
		{
			IInputElement newFocus = e.NewFocus;
			if (newFocus != null)
			{
				this.InspectElementForToolTip(newFocus as DependencyObject, ToolTip.ToolTipTrigger.KeyboardFocus);
			}
		}

		// Token: 0x0600713D RID: 28989 RVA: 0x002D9888 File Offset: 0x002D8888
		private void ProcessMouseUp(object sender, MouseButtonEventArgs e)
		{
			this.RaiseToolTipClosingEvent(false);
			if (!e.Handled && e.ChangedButton == MouseButton.Right && e.RightButton == MouseButtonState.Released)
			{
				IInputElement rawDirectlyOver = Mouse.PrimaryDevice.RawDirectlyOver;
				if (rawDirectlyOver != null)
				{
					Point position = Mouse.PrimaryDevice.GetPosition(rawDirectlyOver);
					if (this.RaiseContextMenuOpeningEvent(rawDirectlyOver, position.X, position.Y, e.UserInitiated))
					{
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x0600713E RID: 28990 RVA: 0x002D98F4 File Offset: 0x002D88F4
		private void ProcessKeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Handled)
			{
				if (!AccessibilitySwitches.UseLegacyToolTipDisplay && e.SystemKey == Key.F10 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					e.Handled = this.OpenOrCloseToolTipViaShortcut();
					return;
				}
				if (e.SystemKey == Key.F10 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					this.RaiseContextMenuOpeningEvent(e);
				}
			}
		}

		// Token: 0x0600713F RID: 28991 RVA: 0x002D9958 File Offset: 0x002D8958
		public bool OpenOrCloseToolTipViaShortcut()
		{
			bool result = false;
			if (this._lastToolTipOpen)
			{
				this.RaiseToolTipClosingEvent(true);
				this.LastObjectWithToolTip = null;
				result = true;
			}
			else
			{
				IInputElement focusedElement = Keyboard.FocusedElement;
				if (focusedElement != null)
				{
					result = this.InspectElementForToolTip(focusedElement as DependencyObject, ToolTip.ToolTipTrigger.KeyboardShortcut);
				}
			}
			return result;
		}

		// Token: 0x06007140 RID: 28992 RVA: 0x002D9999 File Offset: 0x002D8999
		private void ProcessKeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Handled && e.Key == Key.Apps)
			{
				this.RaiseContextMenuOpeningEvent(e);
			}
		}

		// Token: 0x06007141 RID: 28993 RVA: 0x002D99B4 File Offset: 0x002D89B4
		private bool InspectElementForToolTip(DependencyObject o, ToolTip.ToolTipTrigger triggerAction)
		{
			DependencyObject lastChecked = o;
			bool flag = false;
			bool fromKeyboard = triggerAction == ToolTip.ToolTipTrigger.KeyboardFocus || triggerAction == ToolTip.ToolTipTrigger.KeyboardShortcut;
			bool result = this.LocateNearestToolTip(ref o, triggerAction, ref flag);
			if (flag)
			{
				if (o != null)
				{
					if (this.LastObjectWithToolTip != null)
					{
						this.RaiseToolTipClosingEvent(true);
						this.LastMouseOverWithToolTip = null;
					}
					this.LastChecked = lastChecked;
					this.LastObjectWithToolTip = o;
					if (!fromKeyboard)
					{
						this.LastMouseOverWithToolTip = o;
					}
					bool flag2 = !fromKeyboard && this._quickShow;
					this.ResetToolTipTimer();
					if (flag2)
					{
						this._quickShow = false;
						this.RaiseToolTipOpeningEvent(fromKeyboard);
						return result;
					}
					this.ToolTipTimer = new DispatcherTimer(DispatcherPriority.Normal);
					this.ToolTipTimer.Interval = TimeSpan.FromMilliseconds((double)ToolTipService.GetInitialShowDelay(o));
					this.ToolTipTimer.Tag = BooleanBoxes.TrueBox;
					this.ToolTipTimer.Tick += delegate(object s, EventArgs e)
					{
						this.RaiseToolTipOpeningEvent(fromKeyboard);
					};
					this.ToolTipTimer.Start();
					return result;
				}
			}
			else if (this.LastMouseOverWithToolTip == null || triggerAction != ToolTip.ToolTipTrigger.KeyboardFocus)
			{
				this.RaiseToolTipClosingEvent(true);
				if (triggerAction == ToolTip.ToolTipTrigger.Mouse)
				{
					this.LastMouseOverWithToolTip = null;
				}
				this.LastObjectWithToolTip = null;
			}
			return result;
		}

		// Token: 0x06007142 RID: 28994 RVA: 0x002D9AD8 File Offset: 0x002D8AD8
		private bool LocateNearestToolTip(ref DependencyObject o, ToolTip.ToolTipTrigger triggerAction, ref bool showToolTip)
		{
			IInputElement inputElement = o as IInputElement;
			bool result = false;
			showToolTip = false;
			if (inputElement != null)
			{
				FindToolTipEventArgs findToolTipEventArgs = new FindToolTipEventArgs(triggerAction);
				inputElement.RaiseEvent(findToolTipEventArgs);
				result = findToolTipEventArgs.Handled;
				if (findToolTipEventArgs.TargetElement != null)
				{
					o = findToolTipEventArgs.TargetElement;
					showToolTip = true;
				}
				else if (findToolTipEventArgs.KeepCurrentActive)
				{
					o = null;
					showToolTip = true;
				}
			}
			return result;
		}

		// Token: 0x06007143 RID: 28995 RVA: 0x002D9B2E File Offset: 0x002D8B2E
		internal bool StopLookingForToolTip(DependencyObject o)
		{
			return o == this.LastChecked || o == this.LastMouseOverWithToolTip || o == this._currentToolTip || this.WithinCurrentToolTip(o);
		}

		// Token: 0x06007144 RID: 28996 RVA: 0x002D9B58 File Offset: 0x002D8B58
		private bool WithinCurrentToolTip(DependencyObject o)
		{
			if (this._currentToolTip == null)
			{
				return false;
			}
			DependencyObject dependencyObject = o as Visual;
			if (dependencyObject == null)
			{
				ContentElement contentElement = o as ContentElement;
				if (contentElement != null)
				{
					dependencyObject = PopupControlService.FindContentElementParent(contentElement);
				}
				else
				{
					dependencyObject = (o as Visual3D);
				}
			}
			return dependencyObject != null && ((dependencyObject is Visual && ((Visual)dependencyObject).IsDescendantOf(this._currentToolTip)) || (dependencyObject is Visual3D && ((Visual3D)dependencyObject).IsDescendantOf(this._currentToolTip)));
		}

		// Token: 0x06007145 RID: 28997 RVA: 0x002D9BD0 File Offset: 0x002D8BD0
		private void ResetToolTipTimer()
		{
			if (this._toolTipTimer != null)
			{
				this._toolTipTimer.Stop();
				this._toolTipTimer = null;
				this._quickShow = false;
			}
		}

		// Token: 0x06007146 RID: 28998 RVA: 0x002D9BF3 File Offset: 0x002D8BF3
		internal void OnRaiseToolTipOpeningEvent(object sender, EventArgs e)
		{
			this.RaiseToolTipOpeningEvent(false);
		}

		// Token: 0x06007147 RID: 28999 RVA: 0x002D9BFC File Offset: 0x002D8BFC
		private void RaiseToolTipOpeningEvent(bool fromKeyboard = false)
		{
			this.ResetToolTipTimer();
			if (this._forceCloseTimer != null)
			{
				this.OnForceClose(null, EventArgs.Empty);
			}
			DependencyObject lastObjectWithToolTip = this.LastObjectWithToolTip;
			if (lastObjectWithToolTip != null)
			{
				bool flag = true;
				IInputElement inputElement = lastObjectWithToolTip as IInputElement;
				if (inputElement != null)
				{
					ToolTipEventArgs toolTipEventArgs = new ToolTipEventArgs(true);
					inputElement.RaiseEvent(toolTipEventArgs);
					flag = !toolTipEventArgs.Handled;
				}
				if (flag)
				{
					ToolTip toolTip = ToolTipService.GetToolTip(lastObjectWithToolTip) as ToolTip;
					if (toolTip != null)
					{
						this._currentToolTip = toolTip;
						this._ownToolTip = false;
					}
					else if (this._currentToolTip == null || !this._ownToolTip)
					{
						this._currentToolTip = new ToolTip();
						this._ownToolTip = true;
						this._currentToolTip.SetValue(PopupControlService.ServiceOwnedProperty, BooleanBoxes.TrueBox);
						Binding binding = new Binding();
						binding.Path = new PropertyPath(ToolTipService.ToolTipProperty);
						binding.Mode = BindingMode.OneWay;
						binding.Source = lastObjectWithToolTip;
						this._currentToolTip.SetBinding(ContentControl.ContentProperty, binding);
					}
					if (!this._currentToolTip.StaysOpen)
					{
						throw new NotSupportedException(SR.Get("ToolTipStaysOpenFalseNotAllowed"));
					}
					this._currentToolTip.SetValue(PopupControlService.OwnerProperty, lastObjectWithToolTip);
					this._currentToolTip.Opened += new RoutedEventHandler(this.OnToolTipOpened);
					this._currentToolTip.Closed += new RoutedEventHandler(this.OnToolTipClosed);
					this._currentToolTip.FromKeyboard = fromKeyboard;
					this._currentToolTip.IsOpen = true;
					this.ToolTipTimer = new DispatcherTimer(DispatcherPriority.Normal);
					this.ToolTipTimer.Interval = TimeSpan.FromMilliseconds((double)ToolTipService.GetShowDuration(lastObjectWithToolTip));
					this.ToolTipTimer.Tick += this.OnRaiseToolTipClosingEvent;
					this.ToolTipTimer.Start();
				}
			}
		}

		// Token: 0x06007148 RID: 29000 RVA: 0x002D9DA8 File Offset: 0x002D8DA8
		internal void OnRaiseToolTipClosingEvent(object sender, EventArgs e)
		{
			this.RaiseToolTipClosingEvent(false);
		}

		// Token: 0x06007149 RID: 29001 RVA: 0x002D9DB4 File Offset: 0x002D8DB4
		private void RaiseToolTipClosingEvent(bool reset)
		{
			this.ResetToolTipTimer();
			if (reset)
			{
				this.LastChecked = null;
			}
			DependencyObject lastObjectWithToolTip = this.LastObjectWithToolTip;
			if (lastObjectWithToolTip != null && this._currentToolTip != null)
			{
				bool isOpen = this._currentToolTip.IsOpen;
				try
				{
					if (isOpen)
					{
						IInputElement inputElement = lastObjectWithToolTip as IInputElement;
						if (inputElement != null)
						{
							inputElement.RaiseEvent(new ToolTipEventArgs(false));
						}
					}
				}
				finally
				{
					if (this._currentToolTip != null)
					{
						if (isOpen)
						{
							this._currentToolTip.IsOpen = false;
							if (this._currentToolTip != null)
							{
								this._forceCloseTimer = new DispatcherTimer(DispatcherPriority.Normal);
								this._forceCloseTimer.Interval = Popup.AnimationDelayTime;
								this._forceCloseTimer.Tick += this.OnForceClose;
								this._forceCloseTimer.Tag = this._currentToolTip;
								this._forceCloseTimer.Start();
							}
							this._quickShow = true;
							this.ToolTipTimer = new DispatcherTimer(DispatcherPriority.Normal);
							this.ToolTipTimer.Interval = TimeSpan.FromMilliseconds((double)ToolTipService.GetBetweenShowDelay(lastObjectWithToolTip));
							this.ToolTipTimer.Tick += this.OnBetweenShowDelay;
							this.ToolTipTimer.Start();
						}
						else
						{
							this._currentToolTip.ClearValue(PopupControlService.OwnerProperty);
							if (this._ownToolTip)
							{
								BindingOperations.ClearBinding(this._currentToolTip, ContentControl.ContentProperty);
							}
						}
						if (this._currentToolTip != null)
						{
							this._currentToolTip.FromKeyboard = false;
							this._currentToolTip = null;
						}
					}
				}
			}
		}

		// Token: 0x0600714A RID: 29002 RVA: 0x002D9F38 File Offset: 0x002D8F38
		private void OnToolTipOpened(object sender, EventArgs e)
		{
			((ToolTip)sender).Opened -= new RoutedEventHandler(this.OnToolTipOpened);
			this._lastToolTipOpen = true;
		}

		// Token: 0x0600714B RID: 29003 RVA: 0x002D9F58 File Offset: 0x002D8F58
		private void OnToolTipClosed(object sender, EventArgs e)
		{
			ToolTip toolTip = (ToolTip)sender;
			toolTip.Closed -= new RoutedEventHandler(this.OnToolTipClosed);
			toolTip.ClearValue(PopupControlService.OwnerProperty);
			this._lastToolTipOpen = false;
			if ((bool)toolTip.GetValue(PopupControlService.ServiceOwnedProperty))
			{
				BindingOperations.ClearBinding(toolTip, ContentControl.ContentProperty);
			}
		}

		// Token: 0x0600714C RID: 29004 RVA: 0x002D9FAD File Offset: 0x002D8FAD
		private void OnForceClose(object sender, EventArgs e)
		{
			this._forceCloseTimer.Stop();
			((ToolTip)this._forceCloseTimer.Tag).ForceClose();
			this._forceCloseTimer = null;
		}

		// Token: 0x0600714D RID: 29005 RVA: 0x002D9FD6 File Offset: 0x002D8FD6
		private void OnBetweenShowDelay(object source, EventArgs e)
		{
			this.ResetToolTipTimer();
		}

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x0600714E RID: 29006 RVA: 0x002D9FE0 File Offset: 0x002D8FE0
		// (set) Token: 0x0600714F RID: 29007 RVA: 0x002DA013 File Offset: 0x002D9013
		private IInputElement LastMouseDirectlyOver
		{
			get
			{
				if (this._lastMouseDirectlyOver != null)
				{
					IInputElement inputElement = (IInputElement)this._lastMouseDirectlyOver.Target;
					if (inputElement != null)
					{
						return inputElement;
					}
					this._lastMouseDirectlyOver = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastMouseDirectlyOver = null;
					return;
				}
				if (this._lastMouseDirectlyOver == null)
				{
					this._lastMouseDirectlyOver = new WeakReference(value);
					return;
				}
				this._lastMouseDirectlyOver.Target = value;
			}
		}

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x06007150 RID: 29008 RVA: 0x002DA044 File Offset: 0x002D9044
		// (set) Token: 0x06007151 RID: 29009 RVA: 0x002DA077 File Offset: 0x002D9077
		private DependencyObject LastMouseOverWithToolTip
		{
			get
			{
				if (this._lastMouseOverWithToolTip != null)
				{
					DependencyObject dependencyObject = (DependencyObject)this._lastMouseOverWithToolTip.Target;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._lastMouseOverWithToolTip = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastMouseOverWithToolTip = null;
					return;
				}
				if (this._lastMouseOverWithToolTip == null)
				{
					this._lastMouseOverWithToolTip = new WeakReference(value);
					return;
				}
				this._lastMouseOverWithToolTip.Target = value;
			}
		}

		// Token: 0x17001A33 RID: 6707
		// (get) Token: 0x06007152 RID: 29010 RVA: 0x002DA0A8 File Offset: 0x002D90A8
		// (set) Token: 0x06007153 RID: 29011 RVA: 0x002DA0DB File Offset: 0x002D90DB
		private DependencyObject LastObjectWithToolTip
		{
			get
			{
				if (this._lastObjectWithToolTip != null)
				{
					DependencyObject dependencyObject = (DependencyObject)this._lastObjectWithToolTip.Target;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._lastObjectWithToolTip = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastObjectWithToolTip = null;
					return;
				}
				if (this._lastObjectWithToolTip == null)
				{
					this._lastObjectWithToolTip = new WeakReference(value);
					return;
				}
				this._lastObjectWithToolTip.Target = value;
			}
		}

		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x06007154 RID: 29012 RVA: 0x002DA10C File Offset: 0x002D910C
		// (set) Token: 0x06007155 RID: 29013 RVA: 0x002DA13F File Offset: 0x002D913F
		private DependencyObject LastChecked
		{
			get
			{
				if (this._lastChecked != null)
				{
					DependencyObject dependencyObject = (DependencyObject)this._lastChecked.Target;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._lastChecked = null;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this._lastChecked = null;
					return;
				}
				if (this._lastChecked == null)
				{
					this._lastChecked = new WeakReference(value);
					return;
				}
				this._lastChecked.Target = value;
			}
		}

		// Token: 0x06007156 RID: 29014 RVA: 0x002DA170 File Offset: 0x002D9170
		private void RaiseContextMenuOpeningEvent(KeyEventArgs e)
		{
			IInputElement inputElement = e.OriginalSource as IInputElement;
			if (inputElement != null && this.RaiseContextMenuOpeningEvent(inputElement, -1.0, -1.0, e.UserInitiated))
			{
				e.Handled = true;
			}
		}

		// Token: 0x06007157 RID: 29015 RVA: 0x002DA1B4 File Offset: 0x002D91B4
		private bool RaiseContextMenuOpeningEvent(IInputElement source, double x, double y, bool userInitiated)
		{
			ContextMenuEventArgs contextMenuEventArgs = new ContextMenuEventArgs(source, true, x, y);
			DependencyObject dependencyObject = source as DependencyObject;
			if (userInitiated && dependencyObject != null)
			{
				if (InputElement.IsUIElement(dependencyObject))
				{
					((UIElement)dependencyObject).RaiseEvent(contextMenuEventArgs, userInitiated);
				}
				else if (InputElement.IsContentElement(dependencyObject))
				{
					((ContentElement)dependencyObject).RaiseEvent(contextMenuEventArgs, userInitiated);
				}
				else if (InputElement.IsUIElement3D(dependencyObject))
				{
					((UIElement3D)dependencyObject).RaiseEvent(contextMenuEventArgs, userInitiated);
				}
				else
				{
					source.RaiseEvent(contextMenuEventArgs);
				}
			}
			else
			{
				source.RaiseEvent(contextMenuEventArgs);
			}
			if (contextMenuEventArgs.Handled)
			{
				this.RaiseToolTipClosingEvent(true);
				return true;
			}
			DependencyObject targetElement = contextMenuEventArgs.TargetElement;
			if (targetElement != null && ContextMenuService.ContextMenuIsEnabled(targetElement))
			{
				ContextMenu contextMenu = ContextMenuService.GetContextMenu(targetElement) as ContextMenu;
				contextMenu.SetValue(PopupControlService.OwnerProperty, targetElement);
				contextMenu.Closed += this.OnContextMenuClosed;
				if (x == -1.0 && y == -1.0)
				{
					contextMenu.Placement = PlacementMode.Center;
				}
				else
				{
					contextMenu.Placement = PlacementMode.MousePoint;
				}
				this.RaiseToolTipClosingEvent(true);
				contextMenu.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.TrueBox);
				return true;
			}
			return false;
		}

		// Token: 0x06007158 RID: 29016 RVA: 0x002DA2C0 File Offset: 0x002D92C0
		private void OnContextMenuClosed(object source, RoutedEventArgs e)
		{
			ContextMenu contextMenu = source as ContextMenu;
			if (contextMenu != null)
			{
				contextMenu.Closed -= this.OnContextMenuClosed;
				DependencyObject dependencyObject = (DependencyObject)contextMenu.GetValue(PopupControlService.OwnerProperty);
				if (dependencyObject != null)
				{
					contextMenu.ClearValue(PopupControlService.OwnerProperty);
					UIElement target = PopupControlService.GetTarget(dependencyObject);
					if (target != null && !PopupControlService.IsPresentationSourceNull(target))
					{
						object obj;
						if (!(dependencyObject is ContentElement) && !(dependencyObject is UIElement3D))
						{
							IInputElement inputElement = target;
							obj = inputElement;
						}
						else
						{
							obj = (IInputElement)dependencyObject;
						}
						object obj2 = obj;
						ContextMenuEventArgs e2 = new ContextMenuEventArgs(obj2, false);
						((IInputElement)obj2).RaiseEvent(e2);
					}
				}
			}
		}

		// Token: 0x06007159 RID: 29017 RVA: 0x002DA345 File Offset: 0x002D9345
		private static bool IsPresentationSourceNull(DependencyObject uie)
		{
			return PresentationSource.CriticalFromVisual(uie) == null;
		}

		// Token: 0x0600715A RID: 29018 RVA: 0x002DA350 File Offset: 0x002D9350
		internal static DependencyObject FindParent(DependencyObject o)
		{
			DependencyObject dependencyObject = o as Visual;
			if (dependencyObject == null)
			{
				dependencyObject = (o as Visual3D);
			}
			ContentElement contentElement = (dependencyObject == null) ? (o as ContentElement) : null;
			if (contentElement != null)
			{
				o = ContentOperations.GetParent(contentElement);
				if (o != null)
				{
					return o;
				}
				FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					return frameworkContentElement.Parent;
				}
			}
			else if (dependencyObject != null)
			{
				return VisualTreeHelper.GetParent(dependencyObject);
			}
			return null;
		}

		// Token: 0x0600715B RID: 29019 RVA: 0x002DA3A8 File Offset: 0x002D93A8
		internal static DependencyObject FindContentElementParent(ContentElement ce)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = ce;
			while (dependencyObject2 != null)
			{
				dependencyObject = (dependencyObject2 as Visual);
				if (dependencyObject != null)
				{
					break;
				}
				dependencyObject = (dependencyObject2 as Visual3D);
				if (dependencyObject != null)
				{
					break;
				}
				ce = (dependencyObject2 as ContentElement);
				if (ce == null)
				{
					break;
				}
				dependencyObject2 = ContentOperations.GetParent(ce);
				if (dependencyObject2 == null)
				{
					FrameworkContentElement frameworkContentElement = ce as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						dependencyObject2 = frameworkContentElement.Parent;
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x0600715C RID: 29020 RVA: 0x002DA3FC File Offset: 0x002D93FC
		internal static bool IsElementEnabled(DependencyObject o)
		{
			bool result = true;
			UIElement uielement = o as UIElement;
			ContentElement contentElement = (uielement == null) ? (o as ContentElement) : null;
			UIElement3D uielement3D = (uielement == null && contentElement == null) ? (o as UIElement3D) : null;
			if (uielement != null)
			{
				result = uielement.IsEnabled;
			}
			else if (contentElement != null)
			{
				result = contentElement.IsEnabled;
			}
			else if (uielement3D != null)
			{
				result = uielement3D.IsEnabled;
			}
			return result;
		}

		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x0600715D RID: 29021 RVA: 0x002DA452 File Offset: 0x002D9452
		internal static PopupControlService Current
		{
			get
			{
				return FrameworkElement.PopupControlService;
			}
		}

		// Token: 0x17001A36 RID: 6710
		// (get) Token: 0x0600715E RID: 29022 RVA: 0x002DA459 File Offset: 0x002D9459
		internal ToolTip CurrentToolTip
		{
			get
			{
				return this._currentToolTip;
			}
		}

		// Token: 0x17001A37 RID: 6711
		// (get) Token: 0x0600715F RID: 29023 RVA: 0x002DA461 File Offset: 0x002D9461
		// (set) Token: 0x06007160 RID: 29024 RVA: 0x002DA469 File Offset: 0x002D9469
		private DispatcherTimer ToolTipTimer
		{
			get
			{
				return this._toolTipTimer;
			}
			set
			{
				this.ResetToolTipTimer();
				this._toolTipTimer = value;
			}
		}

		// Token: 0x06007161 RID: 29025 RVA: 0x002DA478 File Offset: 0x002D9478
		private static UIElement GetTarget(DependencyObject o)
		{
			UIElement uielement = o as UIElement;
			if (uielement == null)
			{
				ContentElement contentElement = o as ContentElement;
				if (contentElement != null)
				{
					DependencyObject dependencyObject = PopupControlService.FindContentElementParent(contentElement);
					uielement = (dependencyObject as UIElement);
					if (uielement == null)
					{
						UIElement3D uielement3D = dependencyObject as UIElement3D;
						if (uielement3D != null)
						{
							uielement = UIElementHelper.GetContainingUIElement2D(uielement3D);
						}
					}
				}
				else
				{
					UIElement3D uielement3D2 = o as UIElement3D;
					if (uielement3D2 != null)
					{
						uielement = UIElementHelper.GetContainingUIElement2D(uielement3D2);
					}
				}
			}
			return uielement;
		}

		// Token: 0x06007162 RID: 29026 RVA: 0x002DA4D4 File Offset: 0x002D94D4
		private static void OnOwnerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (o is ContextMenu)
			{
				o.CoerceValue(ContextMenu.HorizontalOffsetProperty);
				o.CoerceValue(ContextMenu.VerticalOffsetProperty);
				o.CoerceValue(ContextMenu.PlacementTargetProperty);
				o.CoerceValue(ContextMenu.PlacementRectangleProperty);
				o.CoerceValue(ContextMenu.PlacementProperty);
				o.CoerceValue(ContextMenu.HasDropShadowProperty);
				return;
			}
			if (o is ToolTip)
			{
				o.CoerceValue(ToolTip.HorizontalOffsetProperty);
				o.CoerceValue(ToolTip.VerticalOffsetProperty);
				o.CoerceValue(ToolTip.PlacementTargetProperty);
				o.CoerceValue(ToolTip.PlacementRectangleProperty);
				o.CoerceValue(ToolTip.PlacementProperty);
				o.CoerceValue(ToolTip.HasDropShadowProperty);
			}
		}

		// Token: 0x06007163 RID: 29027 RVA: 0x002DA578 File Offset: 0x002D9578
		internal static object CoerceProperty(DependencyObject o, object value, DependencyProperty dp)
		{
			DependencyObject dependencyObject = (DependencyObject)o.GetValue(PopupControlService.OwnerProperty);
			if (dependencyObject != null)
			{
				bool flag;
				if (dependencyObject.GetValueSource(dp, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					return dependencyObject.GetValue(dp);
				}
				if (dp == ToolTip.PlacementTargetProperty || dp == ContextMenu.PlacementTargetProperty)
				{
					UIElement target = PopupControlService.GetTarget(dependencyObject);
					if (target != null)
					{
						return target;
					}
				}
			}
			return value;
		}

		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x06007164 RID: 29028 RVA: 0x002DA5D2 File Offset: 0x002D95D2
		internal KeyboardFocusChangedEventHandler FocusChangedEventHandler
		{
			get
			{
				return this._focusChangedEventHandler;
			}
		}

		// Token: 0x04003713 RID: 14099
		internal static readonly RoutedEvent ContextMenuOpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PopupControlService));

		// Token: 0x04003714 RID: 14100
		internal static readonly RoutedEvent ContextMenuClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PopupControlService));

		// Token: 0x04003715 RID: 14101
		internal static readonly DependencyProperty ServiceOwnedProperty = DependencyProperty.RegisterAttached("ServiceOwned", typeof(bool), typeof(PopupControlService), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003716 RID: 14102
		internal static readonly DependencyProperty OwnerProperty = DependencyProperty.RegisterAttached("Owner", typeof(DependencyObject), typeof(PopupControlService), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PopupControlService.OnOwnerChanged)));

		// Token: 0x04003717 RID: 14103
		private DispatcherTimer _toolTipTimer;

		// Token: 0x04003718 RID: 14104
		private bool _quickShow;

		// Token: 0x04003719 RID: 14105
		private WeakReference _lastMouseDirectlyOver;

		// Token: 0x0400371A RID: 14106
		private WeakReference _lastMouseOverWithToolTip;

		// Token: 0x0400371B RID: 14107
		private WeakReference _lastObjectWithToolTip;

		// Token: 0x0400371C RID: 14108
		private WeakReference _lastChecked;

		// Token: 0x0400371D RID: 14109
		private bool _lastToolTipOpen;

		// Token: 0x0400371E RID: 14110
		private ToolTip _currentToolTip;

		// Token: 0x0400371F RID: 14111
		private DispatcherTimer _forceCloseTimer;

		// Token: 0x04003720 RID: 14112
		private bool _ownToolTip;

		// Token: 0x04003721 RID: 14113
		private KeyboardFocusChangedEventHandler _focusChangedEventHandler;
	}
}
