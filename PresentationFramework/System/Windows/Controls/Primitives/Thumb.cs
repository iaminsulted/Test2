using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200085A RID: 2138
	[DefaultEvent("DragDelta")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class Thumb : Control
	{
		// Token: 0x06007E1F RID: 32287 RVA: 0x003169B4 File Offset: 0x003159B4
		static Thumb()
		{
			Thumb.DragStartedEvent = EventManager.RegisterRoutedEvent("DragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(Thumb));
			Thumb.DragDeltaEvent = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(Thumb));
			Thumb.DragCompletedEvent = EventManager.RegisterRoutedEvent("DragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(Thumb));
			Thumb.IsDraggingPropertyKey = DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(Thumb), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Thumb.OnIsDraggingPropertyChanged)));
			Thumb.IsDraggingProperty = Thumb.IsDraggingPropertyKey.DependencyProperty;
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(typeof(Thumb)));
			Thumb._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Thumb));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			EventManager.RegisterClassHandler(typeof(Thumb), Mouse.LostMouseCaptureEvent, new MouseEventHandler(Thumb.OnLostMouseCapture));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x1400015E RID: 350
		// (add) Token: 0x06007E20 RID: 32288 RVA: 0x00316B34 File Offset: 0x00315B34
		// (remove) Token: 0x06007E21 RID: 32289 RVA: 0x00316B42 File Offset: 0x00315B42
		[Category("Behavior")]
		public event DragStartedEventHandler DragStarted
		{
			add
			{
				base.AddHandler(Thumb.DragStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragStartedEvent, value);
			}
		}

		// Token: 0x1400015F RID: 351
		// (add) Token: 0x06007E22 RID: 32290 RVA: 0x00316B50 File Offset: 0x00315B50
		// (remove) Token: 0x06007E23 RID: 32291 RVA: 0x00316B5E File Offset: 0x00315B5E
		[Category("Behavior")]
		public event DragDeltaEventHandler DragDelta
		{
			add
			{
				base.AddHandler(Thumb.DragDeltaEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragDeltaEvent, value);
			}
		}

		// Token: 0x14000160 RID: 352
		// (add) Token: 0x06007E24 RID: 32292 RVA: 0x00316B6C File Offset: 0x00315B6C
		// (remove) Token: 0x06007E25 RID: 32293 RVA: 0x00316B7A File Offset: 0x00315B7A
		[Category("Behavior")]
		public event DragCompletedEventHandler DragCompleted
		{
			add
			{
				base.AddHandler(Thumb.DragCompletedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragCompletedEvent, value);
			}
		}

		// Token: 0x17001D18 RID: 7448
		// (get) Token: 0x06007E26 RID: 32294 RVA: 0x00316B88 File Offset: 0x00315B88
		// (set) Token: 0x06007E27 RID: 32295 RVA: 0x00316B9A File Offset: 0x00315B9A
		[Category("Appearance")]
		[Browsable(false)]
		[Bindable(true)]
		public bool IsDragging
		{
			get
			{
				return (bool)base.GetValue(Thumb.IsDraggingProperty);
			}
			protected set
			{
				base.SetValue(Thumb.IsDraggingPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06007E28 RID: 32296 RVA: 0x00316BAD File Offset: 0x00315BAD
		private static void OnIsDraggingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Thumb thumb = (Thumb)d;
			thumb.OnDraggingChanged(e);
			thumb.UpdateVisualState();
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x00316BC4 File Offset: 0x00315BC4
		public void CancelDrag()
		{
			if (this.IsDragging)
			{
				if (base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
				base.ClearValue(Thumb.IsDraggingPropertyKey);
				base.RaiseEvent(new DragCompletedEventArgs(this._previousScreenCoordPosition.X - this._originScreenCoordPosition.X, this._previousScreenCoordPosition.Y - this._originScreenCoordPosition.Y, true));
			}
		}

		// Token: 0x06007E2A RID: 32298 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnDraggingChanged(DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06007E2B RID: 32299 RVA: 0x00316C2C File Offset: 0x00315C2C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (this.IsDragging)
			{
				VisualStateManager.GoToState(this, "Pressed", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06007E2C RID: 32300 RVA: 0x00316CB6 File Offset: 0x00315CB6
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ThumbAutomationPeer(this);
		}

		// Token: 0x06007E2D RID: 32301 RVA: 0x00316CC0 File Offset: 0x00315CC0
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!this.IsDragging)
			{
				e.Handled = true;
				base.Focus();
				base.CaptureMouse();
				base.SetValue(Thumb.IsDraggingPropertyKey, true);
				this._originThumbPoint = e.GetPosition(this);
				this._previousScreenCoordPosition = (this._originScreenCoordPosition = SafeSecurityHelper.ClientToScreen(this, this._originThumbPoint));
				bool flag = true;
				try
				{
					base.RaiseEvent(new DragStartedEventArgs(this._originThumbPoint.X, this._originThumbPoint.Y));
					flag = false;
				}
				finally
				{
					if (flag)
					{
						this.CancelDrag();
					}
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06007E2E RID: 32302 RVA: 0x00316D68 File Offset: 0x00315D68
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (base.IsMouseCaptured && this.IsDragging)
			{
				e.Handled = true;
				base.ClearValue(Thumb.IsDraggingPropertyKey);
				base.ReleaseMouseCapture();
				Point point = SafeSecurityHelper.ClientToScreen(this, e.MouseDevice.GetPosition(this));
				base.RaiseEvent(new DragCompletedEventArgs(point.X - this._originScreenCoordPosition.X, point.Y - this._originScreenCoordPosition.Y, false));
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x06007E2F RID: 32303 RVA: 0x00316DEC File Offset: 0x00315DEC
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			Thumb thumb = (Thumb)sender;
			if (Mouse.Captured != thumb)
			{
				thumb.CancelDrag();
			}
		}

		// Token: 0x06007E30 RID: 32304 RVA: 0x00316E10 File Offset: 0x00315E10
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.IsDragging)
			{
				if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
				{
					Point position = e.GetPosition(this);
					Point point = SafeSecurityHelper.ClientToScreen(this, position);
					if (point != this._previousScreenCoordPosition)
					{
						this._previousScreenCoordPosition = point;
						e.Handled = true;
						base.RaiseEvent(new DragDeltaEventArgs(position.X - this._originThumbPoint.X, position.Y - this._originThumbPoint.Y));
						return;
					}
				}
				else
				{
					if (e.MouseDevice.Captured == this)
					{
						base.ReleaseMouseCapture();
					}
					base.ClearValue(Thumb.IsDraggingPropertyKey);
					this._originThumbPoint.X = 0.0;
					this._originThumbPoint.Y = 0.0;
				}
			}
		}

		// Token: 0x17001D19 RID: 7449
		// (get) Token: 0x06007E31 RID: 32305 RVA: 0x001A5A01 File Offset: 0x001A4A01
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x17001D1A RID: 7450
		// (get) Token: 0x06007E32 RID: 32306 RVA: 0x00316EE5 File Offset: 0x00315EE5
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Thumb._dType;
			}
		}

		// Token: 0x04003B19 RID: 15129
		private static readonly DependencyPropertyKey IsDraggingPropertyKey;

		// Token: 0x04003B1A RID: 15130
		public static readonly DependencyProperty IsDraggingProperty;

		// Token: 0x04003B1B RID: 15131
		private Point _originThumbPoint;

		// Token: 0x04003B1C RID: 15132
		private Point _originScreenCoordPosition;

		// Token: 0x04003B1D RID: 15133
		private Point _previousScreenCoordPosition;

		// Token: 0x04003B1E RID: 15134
		private static DependencyObjectType _dType;
	}
}
