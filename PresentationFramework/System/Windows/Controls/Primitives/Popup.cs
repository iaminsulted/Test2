using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Accessibility;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000848 RID: 2120
	[DefaultProperty("Child")]
	[DefaultEvent("Opened")]
	[Localizability(LocalizationCategory.None)]
	[ContentProperty("Child")]
	public class Popup : FrameworkElement, IAddChild
	{
		// Token: 0x06007C0E RID: 31758 RVA: 0x0030D470 File Offset: 0x0030C470
		static Popup()
		{
			EventManager.RegisterClassHandler(typeof(Popup), Mouse.LostMouseCaptureEvent, new MouseEventHandler(Popup.OnLostMouseCapture));
			EventManager.RegisterClassHandler(typeof(Popup), DragDrop.DragDropStartedEvent, new RoutedEventHandler(Popup.OnDragDropStarted), true);
			EventManager.RegisterClassHandler(typeof(Popup), DragDrop.DragDropCompletedEvent, new RoutedEventHandler(Popup.OnDragDropCompleted), true);
			UIElement.VisibilityProperty.OverrideMetadata(typeof(Popup), new FrameworkPropertyMetadata(VisibilityBoxes.CollapsedBox, null, new CoerceValueCallback(Popup.CoerceVisibility)));
		}

		// Token: 0x06007C0F RID: 31759 RVA: 0x0030D866 File Offset: 0x0030C866
		private static object CoerceVisibility(DependencyObject d, object value)
		{
			return VisibilityBoxes.CollapsedBox;
		}

		// Token: 0x06007C10 RID: 31760 RVA: 0x0030D86D File Offset: 0x0030C86D
		public Popup()
		{
			this._secHelper = new Popup.PopupSecurityHelper();
		}

		// Token: 0x17001CB1 RID: 7345
		// (get) Token: 0x06007C11 RID: 31761 RVA: 0x0030D88C File Offset: 0x0030C88C
		// (set) Token: 0x06007C12 RID: 31762 RVA: 0x0030D89E File Offset: 0x0030C89E
		internal bool TreatMousePlacementAsBottom
		{
			get
			{
				return (bool)base.GetValue(Popup.TreatMousePlacementAsBottomProperty);
			}
			set
			{
				base.SetValue(Popup.TreatMousePlacementAsBottomProperty, value);
			}
		}

		// Token: 0x17001CB2 RID: 7346
		// (get) Token: 0x06007C13 RID: 31763 RVA: 0x0030D8AC File Offset: 0x0030C8AC
		// (set) Token: 0x06007C14 RID: 31764 RVA: 0x0030D8BE File Offset: 0x0030C8BE
		[Bindable(true)]
		[CustomCategory("Content")]
		public UIElement Child
		{
			get
			{
				return (UIElement)base.GetValue(Popup.ChildProperty);
			}
			set
			{
				base.SetValue(Popup.ChildProperty, value);
			}
		}

		// Token: 0x06007C15 RID: 31765 RVA: 0x0030D8CC File Offset: 0x0030C8CC
		private static void OnChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			UIElement child = (UIElement)e.OldValue;
			UIElement child2 = (UIElement)e.NewValue;
			if (popup._popupRoot.Value != null && (popup.IsOpen || popup._popupRoot.Value.Child != null))
			{
				popup._popupRoot.Value.Child = child2;
			}
			popup.RemoveLogicalChild(child);
			popup.AddLogicalChild(child2);
			popup.Reposition();
			popup.pushTextRenderingMode();
		}

		// Token: 0x06007C16 RID: 31766 RVA: 0x0030D94C File Offset: 0x0030C94C
		internal override void pushTextRenderingMode()
		{
			if (this.Child != null && DependencyPropertyHelper.GetValueSource(this.Child, TextOptions.TextRenderingModeProperty).BaseValueSource <= BaseValueSource.Inherited)
			{
				this.Child.VisualTextRenderingMode = TextOptions.GetTextRenderingMode(this);
			}
		}

		// Token: 0x06007C17 RID: 31767 RVA: 0x0030D990 File Offset: 0x0030C990
		private static void RegisterPopupWithPlacementTarget(Popup popup, UIElement placementTarget)
		{
			List<Popup> list = Popup.RegisteredPopupsField.GetValue(placementTarget);
			if (list == null)
			{
				list = new List<Popup>();
				Popup.RegisteredPopupsField.SetValue(placementTarget, list);
			}
			if (!list.Contains(popup))
			{
				list.Add(popup);
			}
		}

		// Token: 0x06007C18 RID: 31768 RVA: 0x0030D9D0 File Offset: 0x0030C9D0
		private static void UnregisterPopupFromPlacementTarget(Popup popup, UIElement placementTarget)
		{
			List<Popup> value = Popup.RegisteredPopupsField.GetValue(placementTarget);
			if (value != null)
			{
				value.Remove(popup);
				if (value.Count == 0)
				{
					Popup.RegisteredPopupsField.SetValue(placementTarget, null);
				}
			}
		}

		// Token: 0x06007C19 RID: 31769 RVA: 0x0030DA08 File Offset: 0x0030CA08
		private void UpdatePlacementTargetRegistration(UIElement oldValue, UIElement newValue)
		{
			if (oldValue != null)
			{
				Popup.UnregisterPopupFromPlacementTarget(this, oldValue);
				if (newValue == null && VisualTreeHelper.GetParent(this) == null)
				{
					TreeWalkHelper.InvalidateOnTreeChange(this, null, oldValue, false);
				}
			}
			if (newValue != null && VisualTreeHelper.GetParent(this) == null)
			{
				Popup.RegisterPopupWithPlacementTarget(this, newValue);
				if (!base.IsSelfInheritanceParent)
				{
					base.SetIsSelfInheritanceParent();
				}
				TreeWalkHelper.InvalidateOnTreeChange(this, null, newValue, true);
			}
		}

		// Token: 0x17001CB3 RID: 7347
		// (get) Token: 0x06007C1A RID: 31770 RVA: 0x0030DA5C File Offset: 0x0030CA5C
		// (set) Token: 0x06007C1B RID: 31771 RVA: 0x0030DA6E File Offset: 0x0030CA6E
		[Category("Appearance")]
		[Bindable(true)]
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(Popup.IsOpenProperty);
			}
			set
			{
				base.SetValue(Popup.IsOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06007C1C RID: 31772 RVA: 0x0030DA84 File Offset: 0x0030CA84
		private static object CoerceIsOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				Popup popup = (Popup)d;
				if (!popup.IsLoaded && VisualTreeHelper.GetParent(popup) != null)
				{
					popup.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x06007C1D RID: 31773 RVA: 0x0030DABD File Offset: 0x0030CABD
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x06007C1E RID: 31774 RVA: 0x0030DAD1 File Offset: 0x0030CAD1
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(Popup.IsOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x06007C1F RID: 31775 RVA: 0x0030DAF0 File Offset: 0x0030CAF0
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			bool flag = (popup._secHelper.IsWindowAlive() && popup._asyncDestroy == null) || popup._asyncCreate != null;
			bool flag2 = (bool)e.NewValue;
			if (flag2 != flag)
			{
				if (flag2)
				{
					if (popup._cacheValid[4])
					{
						throw new InvalidOperationException(SR.Get("PopupReopeningNotAllowed"));
					}
					popup.CancelAsyncDestroy();
					popup.CancelAsyncCreate();
					popup.CreateWindow(false);
					if (popup._secHelper.IsWindowAlive())
					{
						if (Popup.CloseOnUnloadedHandler == null)
						{
							Popup.CloseOnUnloadedHandler = new RoutedEventHandler(Popup.CloseOnUnloaded);
						}
						popup.Unloaded += Popup.CloseOnUnloadedHandler;
						return;
					}
				}
				else
				{
					popup.CancelAsyncCreate();
					if (popup._secHelper.IsWindowAlive() && popup._asyncDestroy == null)
					{
						popup.HideWindow();
						if (Popup.CloseOnUnloadedHandler != null)
						{
							popup.Unloaded -= Popup.CloseOnUnloadedHandler;
						}
					}
				}
			}
		}

		// Token: 0x06007C20 RID: 31776 RVA: 0x0030DBD2 File Offset: 0x0030CBD2
		protected virtual void OnOpened(EventArgs e)
		{
			base.RaiseClrEvent(Popup.OpenedKey, e);
		}

		// Token: 0x06007C21 RID: 31777 RVA: 0x0030DBE0 File Offset: 0x0030CBE0
		protected virtual void OnClosed(EventArgs e)
		{
			this._cacheValid[4] = true;
			try
			{
				base.RaiseClrEvent(Popup.ClosedKey, e);
			}
			finally
			{
				this._cacheValid[4] = false;
			}
		}

		// Token: 0x06007C22 RID: 31778 RVA: 0x0030DC28 File Offset: 0x0030CC28
		private static void CloseOnUnloaded(object sender, RoutedEventArgs e)
		{
			((Popup)sender).SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x17001CB4 RID: 7348
		// (get) Token: 0x06007C23 RID: 31779 RVA: 0x0030DC3F File Offset: 0x0030CC3F
		// (set) Token: 0x06007C24 RID: 31780 RVA: 0x0030DC51 File Offset: 0x0030CC51
		[Bindable(true)]
		[Category("Layout")]
		public PlacementMode Placement
		{
			get
			{
				return (PlacementMode)base.GetValue(Popup.PlacementProperty);
			}
			set
			{
				base.SetValue(Popup.PlacementProperty, value);
			}
		}

		// Token: 0x17001CB5 RID: 7349
		// (get) Token: 0x06007C25 RID: 31781 RVA: 0x0030DC64 File Offset: 0x0030CC64
		internal PlacementMode PlacementInternal
		{
			get
			{
				PlacementMode placementMode = this.Placement;
				if ((placementMode == PlacementMode.Mouse || placementMode == PlacementMode.MousePoint) && this.TreatMousePlacementAsBottom)
				{
					placementMode = PlacementMode.Bottom;
				}
				return placementMode;
			}
		}

		// Token: 0x06007C26 RID: 31782 RVA: 0x0030DC90 File Offset: 0x0030CC90
		private static void OnPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Popup)d).Reposition();
		}

		// Token: 0x06007C27 RID: 31783 RVA: 0x0030DCA0 File Offset: 0x0030CCA0
		private static bool IsValidPlacementMode(object o)
		{
			PlacementMode placementMode = (PlacementMode)o;
			return placementMode == PlacementMode.Absolute || placementMode == PlacementMode.AbsolutePoint || placementMode == PlacementMode.Bottom || placementMode == PlacementMode.Center || placementMode == PlacementMode.Mouse || placementMode == PlacementMode.MousePoint || placementMode == PlacementMode.Relative || placementMode == PlacementMode.RelativePoint || placementMode == PlacementMode.Right || placementMode == PlacementMode.Left || placementMode == PlacementMode.Top || placementMode == PlacementMode.Custom;
		}

		// Token: 0x17001CB6 RID: 7350
		// (get) Token: 0x06007C28 RID: 31784 RVA: 0x0030DCE8 File Offset: 0x0030CCE8
		// (set) Token: 0x06007C29 RID: 31785 RVA: 0x0030DCFA File Offset: 0x0030CCFA
		[Bindable(false)]
		[Category("Layout")]
		public CustomPopupPlacementCallback CustomPopupPlacementCallback
		{
			get
			{
				return (CustomPopupPlacementCallback)base.GetValue(Popup.CustomPopupPlacementCallbackProperty);
			}
			set
			{
				base.SetValue(Popup.CustomPopupPlacementCallbackProperty, value);
			}
		}

		// Token: 0x17001CB7 RID: 7351
		// (get) Token: 0x06007C2A RID: 31786 RVA: 0x0030DD08 File Offset: 0x0030CD08
		// (set) Token: 0x06007C2B RID: 31787 RVA: 0x0030DD1A File Offset: 0x0030CD1A
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpen
		{
			get
			{
				return (bool)base.GetValue(Popup.StaysOpenProperty);
			}
			set
			{
				base.SetValue(Popup.StaysOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06007C2C RID: 31788 RVA: 0x0030DD30 File Offset: 0x0030CD30
		private static void OnStaysOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			if (popup.IsOpen)
			{
				if ((bool)e.NewValue)
				{
					popup.ReleasePopupCapture();
					return;
				}
				popup.EstablishPopupCapture(false);
			}
		}

		// Token: 0x17001CB8 RID: 7352
		// (get) Token: 0x06007C2D RID: 31789 RVA: 0x0030DD68 File Offset: 0x0030CD68
		// (set) Token: 0x06007C2E RID: 31790 RVA: 0x0030DD7A File Offset: 0x0030CD7A
		[Bindable(true)]
		[Category("Layout")]
		[TypeConverter(typeof(LengthConverter))]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(Popup.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(Popup.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x06007C2F RID: 31791 RVA: 0x0030DC90 File Offset: 0x0030CC90
		private static void OnOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Popup)d).Reposition();
		}

		// Token: 0x17001CB9 RID: 7353
		// (get) Token: 0x06007C30 RID: 31792 RVA: 0x0030DD8D File Offset: 0x0030CD8D
		// (set) Token: 0x06007C31 RID: 31793 RVA: 0x0030DD9F File Offset: 0x0030CD9F
		[Bindable(true)]
		[Category("Layout")]
		[TypeConverter(typeof(LengthConverter))]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(Popup.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(Popup.VerticalOffsetProperty, value);
			}
		}

		// Token: 0x17001CBA RID: 7354
		// (get) Token: 0x06007C32 RID: 31794 RVA: 0x0030DDB2 File Offset: 0x0030CDB2
		// (set) Token: 0x06007C33 RID: 31795 RVA: 0x0030DDC4 File Offset: 0x0030CDC4
		[Bindable(true)]
		[Category("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UIElement PlacementTarget
		{
			get
			{
				return (UIElement)base.GetValue(Popup.PlacementTargetProperty);
			}
			set
			{
				base.SetValue(Popup.PlacementTargetProperty, value);
			}
		}

		// Token: 0x06007C34 RID: 31796 RVA: 0x0030DDD4 File Offset: 0x0030CDD4
		private static void OnPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			if (popup.IsOpen)
			{
				popup.UpdatePlacementTargetRegistration((UIElement)e.OldValue, (UIElement)e.NewValue);
				return;
			}
			if (e.OldValue != null)
			{
				Popup.UnregisterPopupFromPlacementTarget(popup, (UIElement)e.OldValue);
			}
		}

		// Token: 0x17001CBB RID: 7355
		// (get) Token: 0x06007C35 RID: 31797 RVA: 0x0030DE2A File Offset: 0x0030CE2A
		// (set) Token: 0x06007C36 RID: 31798 RVA: 0x0030DE3C File Offset: 0x0030CE3C
		[Category("Layout")]
		[Bindable(true)]
		public Rect PlacementRectangle
		{
			get
			{
				return (Rect)base.GetValue(Popup.PlacementRectangleProperty);
			}
			set
			{
				base.SetValue(Popup.PlacementRectangleProperty, value);
			}
		}

		// Token: 0x17001CBC RID: 7356
		// (get) Token: 0x06007C37 RID: 31799 RVA: 0x0030DE50 File Offset: 0x0030CE50
		// (set) Token: 0x06007C38 RID: 31800 RVA: 0x0030DEC3 File Offset: 0x0030CEC3
		internal bool DropOpposite
		{
			get
			{
				bool result = false;
				if (this._cacheValid[8])
				{
					result = this._cacheValid[16];
				}
				else
				{
					DependencyObject dependencyObject = this;
					Popup popup;
					for (;;)
					{
						dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
						PopupRoot popupRoot = dependencyObject as PopupRoot;
						if (popupRoot != null)
						{
							popup = (popupRoot.Parent as Popup);
							dependencyObject = popup;
							if (popup != null && popup._cacheValid[8])
							{
								break;
							}
						}
						if (dependencyObject == null)
						{
							return result;
						}
					}
					result = popup._cacheValid[16];
				}
				return result;
			}
			set
			{
				this._cacheValid[16] = value;
				this._cacheValid[8] = true;
			}
		}

		// Token: 0x06007C39 RID: 31801 RVA: 0x0030DEE0 File Offset: 0x0030CEE0
		private void ClearDropOpposite()
		{
			this._cacheValid[8] = false;
		}

		// Token: 0x17001CBD RID: 7357
		// (get) Token: 0x06007C3A RID: 31802 RVA: 0x0030DEEF File Offset: 0x0030CEEF
		// (set) Token: 0x06007C3B RID: 31803 RVA: 0x0030DF01 File Offset: 0x0030CF01
		[Bindable(true)]
		[Category("Appearance")]
		public PopupAnimation PopupAnimation
		{
			get
			{
				return (PopupAnimation)base.GetValue(Popup.PopupAnimationProperty);
			}
			set
			{
				base.SetValue(Popup.PopupAnimationProperty, value);
			}
		}

		// Token: 0x06007C3C RID: 31804 RVA: 0x0030DF14 File Offset: 0x0030CF14
		private static object CoercePopupAnimation(DependencyObject o, object value)
		{
			if (!((Popup)o).AllowsTransparency)
			{
				return PopupAnimation.None;
			}
			return value;
		}

		// Token: 0x06007C3D RID: 31805 RVA: 0x0030DF2C File Offset: 0x0030CF2C
		private static bool IsValidPopupAnimation(object o)
		{
			PopupAnimation popupAnimation = (PopupAnimation)o;
			return popupAnimation == PopupAnimation.None || popupAnimation == PopupAnimation.Fade || popupAnimation == PopupAnimation.Slide || popupAnimation == PopupAnimation.Scroll;
		}

		// Token: 0x17001CBE RID: 7358
		// (get) Token: 0x06007C3E RID: 31806 RVA: 0x0030DF51 File Offset: 0x0030CF51
		// (set) Token: 0x06007C3F RID: 31807 RVA: 0x0030DF63 File Offset: 0x0030CF63
		public bool AllowsTransparency
		{
			get
			{
				return (bool)base.GetValue(Popup.AllowsTransparencyProperty);
			}
			set
			{
				base.SetValue(Popup.AllowsTransparencyProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06007C40 RID: 31808 RVA: 0x0030DF76 File Offset: 0x0030CF76
		private static void OnAllowsTransparencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(Popup.PopupAnimationProperty);
		}

		// Token: 0x06007C41 RID: 31809 RVA: 0x0030DF83 File Offset: 0x0030CF83
		private static object CoerceAllowsTransparency(DependencyObject d, object value)
		{
			if (!((Popup)d)._secHelper.IsChildPopup)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x17001CBF RID: 7359
		// (get) Token: 0x06007C42 RID: 31810 RVA: 0x0030DF9E File Offset: 0x0030CF9E
		public bool HasDropShadow
		{
			get
			{
				return (bool)base.GetValue(Popup.HasDropShadowProperty);
			}
		}

		// Token: 0x06007C43 RID: 31811 RVA: 0x0030DFB0 File Offset: 0x0030CFB0
		private static object CoerceHasDropShadow(DependencyObject d, object value)
		{
			return BooleanBoxes.Box(SystemParameters.DropShadow && ((Popup)d).AllowsTransparency);
		}

		// Token: 0x06007C44 RID: 31812 RVA: 0x0030DFCC File Offset: 0x0030CFCC
		public static void CreateRootPopup(Popup popup, UIElement child)
		{
			Popup.CreateRootPopupInternal(popup, child, false);
		}

		// Token: 0x06007C45 RID: 31813 RVA: 0x0030DFD8 File Offset: 0x0030CFD8
		internal static void CreateRootPopupInternal(Popup popup, UIElement child, bool bindTreatMousePlacementAsBottomProperty)
		{
			if (popup == null)
			{
				throw new ArgumentNullException("popup");
			}
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			object parent;
			if ((parent = LogicalTreeHelper.GetParent(child)) != null)
			{
				throw new InvalidOperationException(SR.Get("CreateRootPopup_ChildHasLogicalParent", new object[]
				{
					child,
					parent
				}));
			}
			if ((parent = VisualTreeHelper.GetParent(child)) != null)
			{
				throw new InvalidOperationException(SR.Get("CreateRootPopup_ChildHasVisualParent", new object[]
				{
					child,
					parent
				}));
			}
			Binding binding = new Binding("PlacementTarget");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.PlacementTargetProperty, binding);
			popup.Child = child;
			binding = new Binding("VerticalOffset");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.VerticalOffsetProperty, binding);
			binding = new Binding("HorizontalOffset");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.HorizontalOffsetProperty, binding);
			binding = new Binding("PlacementRectangle");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.PlacementRectangleProperty, binding);
			binding = new Binding("Placement");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.PlacementProperty, binding);
			binding = new Binding("StaysOpen");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.StaysOpenProperty, binding);
			binding = new Binding("CustomPopupPlacementCallback");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.CustomPopupPlacementCallbackProperty, binding);
			if (bindTreatMousePlacementAsBottomProperty)
			{
				binding = new Binding("FromKeyboard");
				binding.Mode = BindingMode.OneWay;
				binding.Source = child;
				popup.SetBinding(Popup.TreatMousePlacementAsBottomProperty, binding);
			}
			binding = new Binding("IsOpen");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.IsOpenProperty, binding);
		}

		// Token: 0x06007C46 RID: 31814 RVA: 0x0030E1B4 File Offset: 0x0030D1B4
		internal static bool IsRootedInPopup(Popup parentPopup, UIElement element)
		{
			object parent = LogicalTreeHelper.GetParent(element);
			return (parent != null || VisualTreeHelper.GetParent(element) == null) && parent == parentPopup;
		}

		// Token: 0x14000156 RID: 342
		// (add) Token: 0x06007C47 RID: 31815 RVA: 0x0030E1DC File Offset: 0x0030D1DC
		// (remove) Token: 0x06007C48 RID: 31816 RVA: 0x0030E1EA File Offset: 0x0030D1EA
		public event EventHandler Opened
		{
			add
			{
				base.EventHandlersStoreAdd(Popup.OpenedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(Popup.OpenedKey, value);
			}
		}

		// Token: 0x14000157 RID: 343
		// (add) Token: 0x06007C49 RID: 31817 RVA: 0x0030E1F8 File Offset: 0x0030D1F8
		// (remove) Token: 0x06007C4A RID: 31818 RVA: 0x0030E206 File Offset: 0x0030D206
		public event EventHandler Closed
		{
			add
			{
				base.EventHandlersStoreAdd(Popup.ClosedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(Popup.ClosedKey, value);
			}
		}

		// Token: 0x06007C4B RID: 31819 RVA: 0x0030E214 File Offset: 0x0030D214
		private void FirePopupCouldClose()
		{
			if (this.PopupCouldClose != null)
			{
				this.PopupCouldClose(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000158 RID: 344
		// (add) Token: 0x06007C4C RID: 31820 RVA: 0x0030E230 File Offset: 0x0030D230
		// (remove) Token: 0x06007C4D RID: 31821 RVA: 0x0030E268 File Offset: 0x0030D268
		internal event EventHandler PopupCouldClose;

		// Token: 0x06007C4E RID: 31822 RVA: 0x0030E2A0 File Offset: 0x0030D2A0
		protected override Size MeasureOverride(Size availableSize)
		{
			return default(Size);
		}

		// Token: 0x06007C4F RID: 31823 RVA: 0x0030E2B6 File Offset: 0x0030D2B6
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			this.OnPreviewMouseButton(e);
			base.OnPreviewMouseLeftButtonDown(e);
		}

		// Token: 0x06007C50 RID: 31824 RVA: 0x0030E2C6 File Offset: 0x0030D2C6
		protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseRightButtonDown(e);
			this.OnPreviewMouseButton(e);
		}

		// Token: 0x06007C51 RID: 31825 RVA: 0x0030E2D6 File Offset: 0x0030D2D6
		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			this.OnPreviewMouseButton(e);
			base.OnPreviewMouseLeftButtonUp(e);
		}

		// Token: 0x06007C52 RID: 31826 RVA: 0x0030E2E6 File Offset: 0x0030D2E6
		protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseRightButtonUp(e);
			this.OnPreviewMouseButton(e);
		}

		// Token: 0x06007C53 RID: 31827 RVA: 0x0030E2F8 File Offset: 0x0030D2F8
		private void OnPreviewMouseButton(MouseButtonEventArgs e)
		{
			if (this._cacheValid[1] && !this.StaysOpen && !this._cacheValid[512] && this._popupRoot.Value != null && e.OriginalSource == this._popupRoot.Value && this._popupRoot.Value.InputHitTest(e.GetPosition(this._popupRoot.Value)) == null)
			{
				base.SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
			}
			if (this._cacheValid[512] && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
			{
				this._cacheValid[512] = false;
			}
		}

		// Token: 0x06007C54 RID: 31828 RVA: 0x0030E3B4 File Offset: 0x0030D3B4
		private void EstablishPopupCapture(bool isRestoringCapture = false)
		{
			if (!this._cacheValid[1] && this._popupRoot.Value != null && !this.StaysOpen)
			{
				IInputElement inputElement = Mouse.Captured;
				PopupRoot popupRoot = inputElement as PopupRoot;
				if (popupRoot != null)
				{
					if (isRestoringCapture)
					{
						if (Mouse.LeftButton != MouseButtonState.Released || Mouse.RightButton != MouseButtonState.Released)
						{
							this._cacheValid[512] = true;
						}
					}
					else
					{
						Popup.ParentPopupRootField.SetValue(this, popupRoot);
					}
					inputElement = null;
				}
				if (inputElement == null)
				{
					Mouse.Capture(this._popupRoot.Value, CaptureMode.SubTree);
					this._cacheValid[1] = true;
				}
			}
		}

		// Token: 0x06007C55 RID: 31829 RVA: 0x0030E448 File Offset: 0x0030D448
		private void ReleasePopupCapture()
		{
			if (this._cacheValid[1])
			{
				PopupRoot value = Popup.ParentPopupRootField.GetValue(this);
				Popup.ParentPopupRootField.ClearValue(this);
				if (Mouse.Captured == this._popupRoot.Value)
				{
					if (value == null)
					{
						Mouse.Capture(null);
					}
					else
					{
						Popup popup = value.Parent as Popup;
						if (popup != null)
						{
							popup.EstablishPopupCapture(true);
						}
					}
				}
				this._cacheValid[1] = false;
			}
		}

		// Token: 0x06007C56 RID: 31830 RVA: 0x0030E4BC File Offset: 0x0030D4BC
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			Popup popup = sender as Popup;
			if (!popup.StaysOpen)
			{
				PopupRoot value = popup._popupRoot.Value;
				if (e.OriginalSource != value && Mouse.Captured == null && SafeNativeMethods.GetCapture() == IntPtr.Zero)
				{
					popup.EstablishPopupCapture(false);
					e.Handled = true;
					return;
				}
				if (Mouse.Captured != value)
				{
					popup._cacheValid[1] = false;
				}
				PopupRoot popupRoot = Mouse.Captured as PopupRoot;
				Popup popup2 = (popupRoot == null) ? null : (popupRoot.Parent as Popup);
				if ((popup2 == null || value == null || value != Popup.ParentPopupRootField.GetValue(popup2)) && (Mouse.Captured == null || !MenuBase.IsDescendant(value, Mouse.Captured as DependencyObject)) && Mouse.Captured != value && !popup.IsDragDropActive)
				{
					popup.SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
				}
			}
		}

		// Token: 0x06007C57 RID: 31831 RVA: 0x0030E5AC File Offset: 0x0030D5AC
		private static void OnDragDropStarted(object sender, RoutedEventArgs e)
		{
			((Popup)sender).IsDragDropActive = true;
		}

		// Token: 0x06007C58 RID: 31832 RVA: 0x0030E5BC File Offset: 0x0030D5BC
		private static void OnDragDropCompleted(object sender, RoutedEventArgs e)
		{
			Popup popup = (Popup)sender;
			popup.IsDragDropActive = false;
			if (!popup.StaysOpen)
			{
				popup.EstablishPopupCapture(false);
			}
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x0030E5E8 File Offset: 0x0030D5E8
		void IAddChild.AddChild(object value)
		{
			UIElement uielement = value as UIElement;
			if (uielement == null && value != null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			this.Child = uielement;
		}

		// Token: 0x06007C5A RID: 31834 RVA: 0x0030E63C File Offset: 0x0030D63C
		void IAddChild.AddText(string text)
		{
			this.Child = new TextBlock
			{
				Text = text
			};
		}

		// Token: 0x06007C5B RID: 31835 RVA: 0x0030E65D File Offset: 0x0030D65D
		internal override void OnThemeChanged()
		{
			if (this._popupRoot.Value != null)
			{
				TreeWalkHelper.InvalidateOnResourcesChange(this._popupRoot.Value, null, ResourcesChangeInfo.ThemeChangeInfo);
			}
		}

		// Token: 0x06007C5C RID: 31836 RVA: 0x0030E682 File Offset: 0x0030D682
		internal override bool BlockReverseInheritance()
		{
			return base.TemplatedParent == null;
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x0030E690 File Offset: 0x0030D690
		protected internal override DependencyObject GetUIParentCore()
		{
			if (base.Parent == null)
			{
				UIElement placementTarget = this.PlacementTarget;
				if (placementTarget != null && (this.IsOpen || this._secHelper.IsWindowAlive()))
				{
					return placementTarget;
				}
			}
			return base.GetUIParentCore();
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x0030E6CC File Offset: 0x0030D6CC
		internal override bool IgnoreModelParentBuildRoute(RoutedEventArgs e)
		{
			return base.Parent == null && e.RoutedEvent != Mouse.LostMouseCaptureEvent;
		}

		// Token: 0x17001CC0 RID: 7360
		// (get) Token: 0x06007C5F RID: 31839 RVA: 0x0030E6E8 File Offset: 0x0030D6E8
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object child = this.Child;
				if (child == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new Popup.PopupModelTreeEnumerator(this, child);
			}
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x0030E70C File Offset: 0x0030D70C
		private static Visual GetRootVisual(Visual child)
		{
			DependencyObject dependencyObject = child;
			DependencyObject parent;
			while ((parent = VisualTreeHelper.GetParent(dependencyObject)) != null)
			{
				dependencyObject = parent;
			}
			return dependencyObject as Visual;
		}

		// Token: 0x06007C61 RID: 31841 RVA: 0x0030E730 File Offset: 0x0030D730
		private Visual GetTarget()
		{
			Visual visual = this.PlacementTarget;
			if (visual == null)
			{
				visual = VisualTreeHelper.GetContainingVisual2D(VisualTreeHelper.GetParent(this));
			}
			return visual;
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x0030E754 File Offset: 0x0030D754
		private void SetHitTestable(bool hitTestable)
		{
			this._popupRoot.Value.IsHitTestVisible = hitTestable;
			if (this.IsTransparent)
			{
				this._secHelper.SetHitTestable(hitTestable);
			}
		}

		// Token: 0x06007C63 RID: 31843 RVA: 0x0030E77B File Offset: 0x0030D77B
		private static object AsyncCreateWindow(object arg)
		{
			Popup popup = (Popup)arg;
			popup._asyncCreate = null;
			popup.CreateWindow(true);
			return null;
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x0030E794 File Offset: 0x0030D794
		private void CreateNewPopupRoot()
		{
			if (this._popupRoot.Value == null)
			{
				this._popupRoot.Value = new PopupRoot();
				base.AddLogicalChild(this._popupRoot.Value);
				this._popupRoot.Value.SetupLayoutBindings(this);
			}
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x0030E7E0 File Offset: 0x0030D7E0
		private void CreateWindow(bool asyncCall)
		{
			this.ClearDropOpposite();
			Visual target = this.GetTarget();
			if (target != null && Popup.PopupSecurityHelper.IsVisualPresentationSourceNull(target))
			{
				if (!asyncCall)
				{
					this._asyncCreate = base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(Popup.AsyncCreateWindow), this);
				}
				return;
			}
			if (this._positionInfo != null)
			{
				this._positionInfo.MouseRect = Rect.Empty;
				this._positionInfo.ChildSize = Size.Empty;
			}
			bool flag = !this._secHelper.IsWindowAlive();
			if (Popup.PopupInitialPlacementHelper.IsPerMonitorDpiScalingActive)
			{
				this.DestroyWindowImpl();
				this._positionInfo = null;
				flag = true;
			}
			if (flag)
			{
				this.BuildWindow(target);
				this.CreateNewPopupRoot();
			}
			UIElement child = this.Child;
			if (this._popupRoot.Value.Child != child)
			{
				this._popupRoot.Value.Child = child;
			}
			this.UpdatePlacementTargetRegistration(null, this.PlacementTarget);
			this.UpdateTransform();
			bool flag2;
			if (flag)
			{
				this.SetRootVisualToPopupRoot();
				flag2 = this._secHelper.IsWindowAlive();
				if (flag2)
				{
					this._secHelper.ForceMsaaToUiaBridge(this._popupRoot.Value);
				}
			}
			else
			{
				this.UpdatePosition();
				flag2 = this._secHelper.IsWindowAlive();
			}
			if (flag2)
			{
				this.ShowWindow();
				this.OnOpened(EventArgs.Empty);
			}
		}

		// Token: 0x06007C66 RID: 31846 RVA: 0x0030E91C File Offset: 0x0030D91C
		private void SetRootVisualToPopupRoot()
		{
			if (this.PopupAnimation != PopupAnimation.None && this.IsTransparent)
			{
				this._popupRoot.Value.Opacity = 0.0;
			}
			this._secHelper.SetWindowRootVisual(this._popupRoot.Value);
		}

		// Token: 0x06007C67 RID: 31847 RVA: 0x0030E968 File Offset: 0x0030D968
		private void BuildWindow(Visual targetVisual)
		{
			base.CoerceValue(Popup.AllowsTransparencyProperty);
			base.CoerceValue(Popup.HasDropShadowProperty);
			this.IsTransparent = this.AllowsTransparency;
			NativeMethods.POINTSTRUCT pointstruct = (this._positionInfo != null) ? new NativeMethods.POINTSTRUCT(this._positionInfo.X, this._positionInfo.Y) : Popup.PopupInitialPlacementHelper.GetPlacementOrigin(this);
			this._secHelper.BuildWindow(pointstruct.x, pointstruct.y, targetVisual, this.IsTransparent, new HwndSourceHook(this.PopupFilterMessage), new AutoResizedEventHandler(this.OnWindowResize), new HwndDpiChangedEventHandler(this.OnDpiChanged));
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x0030EA08 File Offset: 0x0030DA08
		private bool DestroyWindowImpl()
		{
			if (this._secHelper.IsWindowAlive())
			{
				this._secHelper.DestroyWindow(new HwndSourceHook(this.PopupFilterMessage), new AutoResizedEventHandler(this.OnWindowResize), new HwndDpiChangedEventHandler(this.OnDpiChanged));
				return true;
			}
			return false;
		}

		// Token: 0x06007C69 RID: 31849 RVA: 0x0030EA54 File Offset: 0x0030DA54
		private void DestroyWindow()
		{
			if (this._secHelper.IsWindowAlive() && this.DestroyWindowImpl())
			{
				this.ReleasePopupCapture();
				this.OnClosed(EventArgs.Empty);
				this.UpdatePlacementTargetRegistration(this.PlacementTarget, null);
			}
		}

		// Token: 0x06007C6A RID: 31850 RVA: 0x0030EA8C File Offset: 0x0030DA8C
		private void ShowWindow()
		{
			if (this._secHelper.IsWindowAlive())
			{
				this._popupRoot.Value.Opacity = 1.0;
				this.SetupAnimations(true);
				this.SetHitTestable(this.HitTestable || !this.IsTransparent);
				this.EstablishPopupCapture(false);
				this._secHelper.ShowWindow();
			}
		}

		// Token: 0x06007C6B RID: 31851 RVA: 0x0030EAF4 File Offset: 0x0030DAF4
		private void HideWindow()
		{
			bool flag = this.SetupAnimations(false);
			this.SetHitTestable(false);
			this.ReleasePopupCapture();
			this._asyncDestroy = new DispatcherTimer(DispatcherPriority.Input);
			this._asyncDestroy.Tick += delegate(object sender, EventArgs args)
			{
				this._asyncDestroy.Stop();
				this._asyncDestroy = null;
				this.DestroyWindow();
			};
			this._asyncDestroy.Interval = (flag ? Popup.AnimationDelayTime : TimeSpan.Zero);
			this._asyncDestroy.Start();
			if (!flag)
			{
				this._secHelper.HideWindow();
			}
		}

		// Token: 0x06007C6C RID: 31852 RVA: 0x0030EB6C File Offset: 0x0030DB6C
		private bool SetupAnimations(bool visible)
		{
			PopupAnimation popupAnimation = this.PopupAnimation;
			this._popupRoot.Value.StopAnimations();
			if (popupAnimation != PopupAnimation.None && this.IsTransparent)
			{
				if (popupAnimation == PopupAnimation.Fade)
				{
					this._popupRoot.Value.SetupFadeAnimation(Popup.AnimationDelayTime, visible);
					return true;
				}
				if (visible)
				{
					this._popupRoot.Value.SetupTranslateAnimations(popupAnimation, Popup.AnimationDelayTime, this.AnimateFromRight, this.AnimateFromBottom);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007C6D RID: 31853 RVA: 0x0030EBE9 File Offset: 0x0030DBE9
		private void CancelAsyncCreate()
		{
			if (this._asyncCreate != null)
			{
				this._asyncCreate.Abort();
				this._asyncCreate = null;
			}
		}

		// Token: 0x06007C6E RID: 31854 RVA: 0x0030EC06 File Offset: 0x0030DC06
		private void CancelAsyncDestroy()
		{
			if (this._asyncDestroy != null)
			{
				this._asyncDestroy.Stop();
				this._asyncDestroy = null;
			}
		}

		// Token: 0x06007C6F RID: 31855 RVA: 0x0030EC22 File Offset: 0x0030DC22
		internal void ForceClose()
		{
			if (this._asyncDestroy != null)
			{
				this.CancelAsyncDestroy();
				this.DestroyWindow();
			}
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x0030EC38 File Offset: 0x0030DC38
		private unsafe IntPtr PopupFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg != 28)
			{
				if (msg == 33)
				{
					handled = true;
					return new IntPtr(3);
				}
				if (msg == 70)
				{
					if (this._secHelper.IsChildPopup)
					{
						NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)lParam);
						ptr->flags |= 256;
					}
				}
			}
			else if (wParam == IntPtr.Zero)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.HandleDeactivateApp), null);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06007C71 RID: 31857 RVA: 0x0030ECB6 File Offset: 0x0030DCB6
		private object HandleDeactivateApp(object arg)
		{
			if (!this.StaysOpen)
			{
				base.SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
			}
			this.FirePopupCouldClose();
			return null;
		}

		// Token: 0x06007C72 RID: 31858 RVA: 0x0030ECD8 File Offset: 0x0030DCD8
		private void UpdateTransform()
		{
			Matrix matrix = base.LayoutTransform.Value * base.RenderTransform.Value;
			DependencyObject parent = VisualTreeHelper.GetParent(this);
			Visual visual = (parent == null) ? null : Popup.GetRootVisual(this);
			if (visual != null)
			{
				matrix = matrix * base.TransformToAncestor(visual).AffineTransform.Value * PointUtil.GetVisualTransform(visual);
			}
			if (this.IsTransparent)
			{
				if (parent != null && (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty) == FlowDirection.RightToLeft)
				{
					matrix.Scale(-1.0, 1.0);
				}
			}
			else
			{
				Vector vector = matrix.Transform(new Vector(1.0, 0.0));
				Vector vector2 = matrix.Transform(new Vector(0.0, 1.0));
				matrix = default(Matrix);
				matrix.Scale(vector.Length, vector2.Length);
			}
			this._popupRoot.Value.Transform = new MatrixTransform(matrix);
		}

		// Token: 0x06007C73 RID: 31859 RVA: 0x0030EDEC File Offset: 0x0030DDEC
		private void OnWindowResize(object sender, AutoResizedEventArgs e)
		{
			if (this._positionInfo == null)
			{
				throw new NullReferenceException(new NullReferenceException().Message, this.SavedException);
			}
			Popup.SavedExceptionField.ClearValue(this);
			if (e.Size != this._positionInfo.ChildSize)
			{
				this._positionInfo.ChildSize = e.Size;
				this.Reposition();
			}
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x0030EE51 File Offset: 0x0030DE51
		private void OnDpiChanged(object sender, HwndDpiChangedEventArgs e)
		{
			if (this.IsOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x17001CC1 RID: 7361
		// (get) Token: 0x06007C75 RID: 31861 RVA: 0x0030EE62 File Offset: 0x0030DE62
		// (set) Token: 0x06007C76 RID: 31862 RVA: 0x0030EE6F File Offset: 0x0030DE6F
		internal Exception SavedException
		{
			get
			{
				return Popup.SavedExceptionField.GetValue(this);
			}
			set
			{
				Popup.SavedExceptionField.SetValue(this, value);
			}
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x0030EE80 File Offset: 0x0030DE80
		internal void Reposition()
		{
			if (this.IsOpen && this._secHelper.IsWindowAlive())
			{
				if (base.CheckAccess())
				{
					this.UpdatePosition();
					return;
				}
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object param)
				{
					this.Reposition();
					return null;
				}), null);
			}
		}

		// Token: 0x06007C78 RID: 31864 RVA: 0x0030EECC File Offset: 0x0030DECC
		private static bool IsAbsolutePlacementMode(PlacementMode placement)
		{
			return placement == PlacementMode.Absolute || placement == PlacementMode.AbsolutePoint || placement - PlacementMode.Mouse <= 1;
		}

		// Token: 0x06007C79 RID: 31865 RVA: 0x0030EEE0 File Offset: 0x0030DEE0
		private void UpdatePosition()
		{
			if (this._popupRoot.Value == null)
			{
				return;
			}
			PlacementMode placementInternal = this.PlacementInternal;
			Point[] placementTargetInterestPoints = this.GetPlacementTargetInterestPoints(placementInternal);
			Point[] childInterestPoints = this.GetChildInterestPoints(placementInternal);
			Rect bounds = this.GetBounds(placementTargetInterestPoints);
			Rect bounds2 = this.GetBounds(childInterestPoints);
			double num = bounds2.Width * bounds2.Height;
			int num2 = -1;
			Vector offsetVector = new Vector((double)this._positionInfo.X, (double)this._positionInfo.Y);
			double num3 = -1.0;
			CustomPopupPlacement[] array = null;
			int num4;
			if (placementInternal == PlacementMode.Custom)
			{
				CustomPopupPlacementCallback customPopupPlacementCallback = this.CustomPopupPlacementCallback;
				if (customPopupPlacementCallback != null)
				{
					array = customPopupPlacementCallback(bounds2.Size, bounds.Size, new Point(this.HorizontalOffset, this.VerticalOffset));
				}
				num4 = ((array == null) ? 0 : array.Length);
				if (!this.IsOpen)
				{
					return;
				}
			}
			else
			{
				num4 = Popup.GetNumberOfCombinations(placementInternal);
			}
			Rect screenBounds;
			for (int i = 0; i < num4; i++)
			{
				bool animateFromRight = false;
				bool animateFromBottom = false;
				Vector vector;
				if (placementInternal == PlacementMode.Custom)
				{
					vector = (Vector)placementTargetInterestPoints[0] + (Vector)array[i].Point;
					PopupPrimaryAxis primaryAxis = array[i].PrimaryAxis;
				}
				else
				{
					PopupPrimaryAxis primaryAxis;
					Popup.PointCombination pointCombination = this.GetPointCombination(placementInternal, i, out primaryAxis);
					Popup.InterestPoint targetInterestPoint = pointCombination.TargetInterestPoint;
					Popup.InterestPoint childInterestPoint = pointCombination.ChildInterestPoint;
					vector = placementTargetInterestPoints[(int)targetInterestPoint] - childInterestPoints[(int)childInterestPoint];
					animateFromRight = (childInterestPoint == Popup.InterestPoint.TopRight || childInterestPoint == Popup.InterestPoint.BottomRight);
					animateFromBottom = (childInterestPoint == Popup.InterestPoint.BottomLeft || childInterestPoint == Popup.InterestPoint.BottomRight);
				}
				Rect rect = Rect.Offset(bounds2, vector);
				screenBounds = this.GetScreenBounds(bounds, placementTargetInterestPoints[0]);
				Rect rect2 = Rect.Intersect(screenBounds, rect);
				double num5 = (rect2 != Rect.Empty) ? (rect2.Width * rect2.Height) : 0.0;
				if (num5 - num3 > 0.01)
				{
					num2 = i;
					offsetVector = vector;
					num3 = num5;
					this.AnimateFromRight = animateFromRight;
					this.AnimateFromBottom = animateFromBottom;
					if (Math.Abs(num5 - num) < 0.01)
					{
						break;
					}
				}
			}
			if (num2 >= 2 && (placementInternal == PlacementMode.Right || placementInternal == PlacementMode.Left))
			{
				this.DropOpposite = !this.DropOpposite;
			}
			bounds2 = new Rect((Size)this._secHelper.GetTransformToDevice().Transform((Point)this._popupRoot.Value.RenderSize));
			bounds2.Offset(offsetVector);
			screenBounds = this.GetScreenBounds(bounds, placementTargetInterestPoints[0]);
			Rect rect3 = Rect.Intersect(screenBounds, bounds2);
			if (Math.Abs(rect3.Width - bounds2.Width) > 0.01 || Math.Abs(rect3.Height - bounds2.Height) > 0.01)
			{
				Point point = placementTargetInterestPoints[0];
				Vector vector2 = placementTargetInterestPoints[1] - point;
				vector2.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector2.Y) || Math.Abs(vector2.Y) < 0.01)
				{
					if (bounds2.Right > screenBounds.Right)
					{
						offsetVector.X = screenBounds.Right - bounds2.Width;
					}
					else if (bounds2.Left < screenBounds.Left)
					{
						offsetVector.X = screenBounds.Left;
					}
				}
				else if (this.IsTransparent && Math.Abs(vector2.X) < 0.01)
				{
					if (bounds2.Bottom > screenBounds.Bottom)
					{
						offsetVector.Y = screenBounds.Bottom - bounds2.Height;
					}
					else if (bounds2.Top < screenBounds.Top)
					{
						offsetVector.Y = screenBounds.Top;
					}
				}
				Point point2 = placementTargetInterestPoints[2];
				Vector vector3 = point - point2;
				vector3.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector3.X) || Math.Abs(vector3.X) < 0.01)
				{
					if (bounds2.Bottom > screenBounds.Bottom)
					{
						offsetVector.Y = screenBounds.Bottom - bounds2.Height;
					}
					else if (bounds2.Top < screenBounds.Top)
					{
						offsetVector.Y = screenBounds.Top;
					}
				}
				else if (this.IsTransparent && Math.Abs(vector3.Y) < 0.01)
				{
					if (bounds2.Right > screenBounds.Right)
					{
						offsetVector.X = screenBounds.Right - bounds2.Width;
					}
					else if (bounds2.Left < screenBounds.Left)
					{
						offsetVector.X = screenBounds.Left;
					}
				}
			}
			int num6 = DoubleUtil.DoubleToInt(offsetVector.X);
			int num7 = DoubleUtil.DoubleToInt(offsetVector.Y);
			if (num6 != this._positionInfo.X || num7 != this._positionInfo.Y)
			{
				this._positionInfo.X = num6;
				this._positionInfo.Y = num7;
				this._secHelper.SetPopupPos(true, num6, num7, false, 0, 0);
			}
		}

		// Token: 0x06007C7A RID: 31866 RVA: 0x0030F408 File Offset: 0x0030E408
		private void GetPopupRootLimits(out Rect targetBounds, out Rect screenBounds, out Size limitSize)
		{
			PlacementMode placementInternal = this.PlacementInternal;
			Point[] placementTargetInterestPoints = this.GetPlacementTargetInterestPoints(placementInternal);
			targetBounds = this.GetBounds(placementTargetInterestPoints);
			screenBounds = this.GetScreenBounds(targetBounds, placementTargetInterestPoints[0]);
			PopupPrimaryAxis primaryAxis = Popup.GetPrimaryAxis(placementInternal);
			limitSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			if (primaryAxis == PopupPrimaryAxis.Horizontal)
			{
				Point point = placementTargetInterestPoints[0];
				Vector vector = placementTargetInterestPoints[2] - point;
				vector.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector.X) || Math.Abs(vector.X) < 0.01)
				{
					limitSize.Height = Math.Max(0.0, Math.Max(screenBounds.Bottom - targetBounds.Bottom, targetBounds.Top - screenBounds.Top));
					return;
				}
				if (this.IsTransparent && Math.Abs(vector.Y) < 0.01)
				{
					limitSize.Width = Math.Max(0.0, Math.Max(screenBounds.Right - targetBounds.Right, targetBounds.Left - screenBounds.Left));
					return;
				}
			}
			else if (primaryAxis == PopupPrimaryAxis.Vertical)
			{
				Point point2 = placementTargetInterestPoints[0];
				Vector vector2 = placementTargetInterestPoints[1] - point2;
				vector2.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector2.X) || Math.Abs(vector2.Y) < 0.01)
				{
					limitSize.Width = Math.Max(0.0, Math.Max(screenBounds.Right - targetBounds.Right, targetBounds.Left - screenBounds.Left));
					return;
				}
				if (this.IsTransparent && Math.Abs(vector2.X) < 0.01)
				{
					limitSize.Height = Math.Max(0.0, Math.Max(screenBounds.Bottom - targetBounds.Bottom, targetBounds.Top - screenBounds.Top));
				}
			}
		}

		// Token: 0x06007C7B RID: 31867 RVA: 0x0030F628 File Offset: 0x0030E628
		private Point[] GetPlacementTargetInterestPoints(PlacementMode placement)
		{
			if (this._positionInfo == null)
			{
				this._positionInfo = new Popup.PositionInfo();
			}
			Rect rect = this.PlacementRectangle;
			UIElement uielement = this.GetTarget() as UIElement;
			Vector vector = new Vector(this.HorizontalOffset, this.VerticalOffset);
			Point[] array;
			if (uielement == null || Popup.IsAbsolutePlacementMode(placement))
			{
				if (placement == PlacementMode.Mouse || placement == PlacementMode.MousePoint)
				{
					if (this._positionInfo.MouseRect == Rect.Empty)
					{
						this._positionInfo.MouseRect = this.GetMouseRect(placement);
					}
					rect = this._positionInfo.MouseRect;
				}
				else if (rect == Rect.Empty)
				{
					rect = default(Rect);
				}
				vector = this._secHelper.GetTransformToDevice().Transform(vector);
				rect.Offset(vector);
				array = Popup.InterestPointsFromRect(rect);
			}
			else
			{
				if (rect == Rect.Empty)
				{
					if (placement != PlacementMode.Relative && placement != PlacementMode.RelativePoint)
					{
						rect = new Rect(0.0, 0.0, uielement.RenderSize.Width, uielement.RenderSize.Height);
					}
					else
					{
						rect = default(Rect);
					}
				}
				rect.Offset(vector);
				array = Popup.InterestPointsFromRect(rect);
				Visual rootVisual = Popup.GetRootVisual(uielement);
				GeneralTransform generalTransform = Popup.TransformToClient(uielement, rootVisual);
				for (int i = 0; i < 5; i++)
				{
					generalTransform.TryTransform(array[i], out array[i]);
					array[i] = this._secHelper.ClientToScreen(rootVisual, array[i]);
				}
			}
			return array;
		}

		// Token: 0x06007C7C RID: 31868 RVA: 0x0030F7B4 File Offset: 0x0030E7B4
		private static void SwapPoints(ref Point p1, ref Point p2)
		{
			Point point = p1;
			p1 = p2;
			p2 = point;
		}

		// Token: 0x06007C7D RID: 31869 RVA: 0x0030F7DC File Offset: 0x0030E7DC
		private Point[] GetChildInterestPoints(PlacementMode placement)
		{
			UIElement child = this.Child;
			if (child == null)
			{
				return Popup.InterestPointsFromRect(default(Rect));
			}
			Point[] array = Popup.InterestPointsFromRect(new Rect(default(Point), child.RenderSize));
			UIElement uielement = this.GetTarget() as UIElement;
			if (uielement != null && !Popup.IsAbsolutePlacementMode(placement) && (FlowDirection)uielement.GetValue(FrameworkElement.FlowDirectionProperty) != (FlowDirection)child.GetValue(FrameworkElement.FlowDirectionProperty))
			{
				Popup.SwapPoints(ref array[0], ref array[1]);
				Popup.SwapPoints(ref array[2], ref array[3]);
			}
			Vector animationOffset = this._popupRoot.Value.AnimationOffset;
			GeneralTransform generalTransform = Popup.TransformToClient(child, this._popupRoot.Value);
			for (int i = 0; i < 5; i++)
			{
				generalTransform.TryTransform(array[i] - animationOffset, out array[i]);
			}
			return array;
		}

		// Token: 0x06007C7E RID: 31870 RVA: 0x0030F8D0 File Offset: 0x0030E8D0
		private static Point[] InterestPointsFromRect(Rect rect)
		{
			return new Point[]
			{
				rect.TopLeft,
				rect.TopRight,
				rect.BottomLeft,
				rect.BottomRight,
				new Point(rect.Left + rect.Width / 2.0, rect.Top + rect.Height / 2.0)
			};
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x0030F959 File Offset: 0x0030E959
		private static GeneralTransform TransformToClient(Visual visual, Visual rootVisual)
		{
			return new GeneralTransformGroup
			{
				Children = 
				{
					visual.TransformToAncestor(rootVisual),
					new MatrixTransform(PointUtil.GetVisualTransform(rootVisual) * Popup.PopupSecurityHelper.GetTransformToDevice(rootVisual))
				}
			};
		}

		// Token: 0x06007C80 RID: 31872 RVA: 0x0030F994 File Offset: 0x0030E994
		private Rect GetBounds(Point[] interestPoints)
		{
			double num2;
			double num = num2 = interestPoints[0].X;
			double num4;
			double num3 = num4 = interestPoints[0].Y;
			for (int i = 1; i < interestPoints.Length; i++)
			{
				double x = interestPoints[i].X;
				double y = interestPoints[i].Y;
				if (x < num2)
				{
					num2 = x;
				}
				if (x > num)
				{
					num = x;
				}
				if (y < num4)
				{
					num4 = y;
				}
				if (y > num3)
				{
					num3 = y;
				}
			}
			return new Rect(num2, num4, num - num2, num3 - num4);
		}

		// Token: 0x06007C81 RID: 31873 RVA: 0x0030FA1C File Offset: 0x0030EA1C
		private static int GetNumberOfCombinations(PlacementMode placement)
		{
			switch (placement)
			{
			case PlacementMode.Bottom:
			case PlacementMode.Mouse:
			case PlacementMode.Top:
				return 2;
			case PlacementMode.Right:
			case PlacementMode.AbsolutePoint:
			case PlacementMode.RelativePoint:
			case PlacementMode.MousePoint:
			case PlacementMode.Left:
				return 4;
			case PlacementMode.Custom:
				return 0;
			}
			return 1;
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x0030FA68 File Offset: 0x0030EA68
		private Popup.PointCombination GetPointCombination(PlacementMode placement, int i, out PopupPrimaryAxis axis)
		{
			bool flag = SystemParameters.MenuDropAlignment;
			switch (placement)
			{
			case PlacementMode.Relative:
			case PlacementMode.AbsolutePoint:
			case PlacementMode.RelativePoint:
			case PlacementMode.MousePoint:
				axis = PopupPrimaryAxis.Horizontal;
				if (flag)
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopLeft);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomRight);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				break;
			case PlacementMode.Bottom:
			case PlacementMode.Mouse:
				axis = PopupPrimaryAxis.Horizontal;
				if (flag)
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				break;
			case PlacementMode.Center:
				axis = PopupPrimaryAxis.None;
				return new Popup.PointCombination(Popup.InterestPoint.Center, Popup.InterestPoint.Center);
			case PlacementMode.Right:
			case PlacementMode.Left:
				axis = PopupPrimaryAxis.Vertical;
				flag |= this.DropOpposite;
				if ((flag && placement == PlacementMode.Right) || (!flag && placement == PlacementMode.Left))
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.BottomRight);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.TopLeft);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.BottomLeft);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				break;
			case PlacementMode.Top:
				axis = PopupPrimaryAxis.Horizontal;
				if (flag)
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.BottomRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.TopRight);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.TopLeft);
					}
					goto IL_1AE;
				}
				break;
			}
			axis = PopupPrimaryAxis.None;
			return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopLeft);
			IL_1AE:
			return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x0030FC2A File Offset: 0x0030EC2A
		private static PopupPrimaryAxis GetPrimaryAxis(PlacementMode placement)
		{
			switch (placement)
			{
			case PlacementMode.Bottom:
			case PlacementMode.AbsolutePoint:
			case PlacementMode.RelativePoint:
			case PlacementMode.Top:
				return PopupPrimaryAxis.Horizontal;
			case PlacementMode.Right:
			case PlacementMode.Left:
				return PopupPrimaryAxis.Vertical;
			}
			return PopupPrimaryAxis.None;
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x0030FC6C File Offset: 0x0030EC6C
		internal Size RestrictSize(Size desiredSize)
		{
			Rect rect;
			Rect rect2;
			Size size;
			this.GetPopupRootLimits(out rect, out rect2, out size);
			desiredSize = (Size)this._secHelper.GetTransformToDevice().Transform((Point)desiredSize);
			desiredSize.Width = Math.Min(desiredSize.Width, rect2.Width);
			desiredSize.Width = Math.Min(desiredSize.Width, size.Width);
			double val = 0.75 * rect2.Width * rect2.Height / desiredSize.Width;
			desiredSize.Height = Math.Min(desiredSize.Height, rect2.Height);
			desiredSize.Height = Math.Min(desiredSize.Height, val);
			desiredSize.Height = Math.Min(desiredSize.Height, size.Height);
			desiredSize = (Size)this._secHelper.GetTransformFromDevice().Transform((Point)desiredSize);
			return desiredSize;
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x0030FD68 File Offset: 0x0030ED68
		private Rect GetScreenBounds(Rect boundingBox, Point p)
		{
			if (this._secHelper.IsChildPopup)
			{
				return this._secHelper.GetParentWindowRect();
			}
			NativeMethods.RECT rc = new NativeMethods.RECT(0, 0, 0, 0);
			NativeMethods.RECT rect = PointUtil.FromRect(boundingBox);
			IntPtr intPtr = SafeNativeMethods.MonitorFromRect(ref rect, 2);
			if (intPtr != IntPtr.Zero)
			{
				NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
				monitorinfoex.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFOEX));
				SafeNativeMethods.GetMonitorInfo(new HandleRef(null, intPtr), monitorinfoex);
				if ((this.Child is MenuBase || this.Child is ToolTip || base.TemplatedParent is MenuItem) && p.X >= (double)monitorinfoex.rcWork.left && p.X <= (double)monitorinfoex.rcWork.right && p.Y >= (double)monitorinfoex.rcWork.top && p.Y <= (double)monitorinfoex.rcWork.bottom)
				{
					rc = monitorinfoex.rcWork;
				}
				else
				{
					rc = monitorinfoex.rcMonitor;
				}
			}
			return PointUtil.ToRect(rc);
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x0030FE74 File Offset: 0x0030EE74
		private Rect GetMouseRect(PlacementMode placement)
		{
			NativeMethods.POINT mouseCursorPos = this._secHelper.GetMouseCursorPos(this.GetTarget());
			if (placement == PlacementMode.Mouse)
			{
				int num;
				int num2;
				int num3;
				int num4;
				Popup.GetMouseCursorSize(out num, out num2, out num3, out num4);
				return new Rect((double)mouseCursorPos.x, (double)(mouseCursorPos.y - 1), (double)Math.Max(0, num - num3), (double)Math.Max(0, num2 - num4 + 2));
			}
			return new Rect((double)mouseCursorPos.x, (double)mouseCursorPos.y, 0.0, 0.0);
		}

		// Token: 0x06007C87 RID: 31879 RVA: 0x0030FEF8 File Offset: 0x0030EEF8
		private static void GetMouseCursorSize(out int width, out int height, out int hotX, out int hotY)
		{
			width = (height = (hotX = (hotY = 0)));
			IntPtr cursor = SafeNativeMethods.GetCursor();
			if (cursor != IntPtr.Zero)
			{
				width = (height = 16);
				NativeMethods.ICONINFO iconinfo = new NativeMethods.ICONINFO();
				bool flag = true;
				try
				{
					UnsafeNativeMethods.GetIconInfo(new HandleRef(null, cursor), out iconinfo);
				}
				catch (Win32Exception)
				{
					flag = false;
				}
				if (flag)
				{
					NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
					if (UnsafeNativeMethods.GetObject(iconinfo.hbmMask.MakeHandleRef(null), Marshal.SizeOf(typeof(NativeMethods.BITMAP)), bitmap) != 0)
					{
						int num = bitmap.bmWidth * bitmap.bmHeight / 8;
						byte[] array = new byte[num * 2];
						if (UnsafeNativeMethods.GetBitmapBits(iconinfo.hbmMask.MakeHandleRef(null), array.Length, array) != 0)
						{
							bool flag2 = false;
							if (iconinfo.hbmColor.IsInvalid)
							{
								flag2 = true;
								num /= 2;
							}
							bool flag3 = true;
							int i = num;
							for (i--; i >= 0; i--)
							{
								if (array[i] != 255 || (flag2 && array[i + num] != 0))
								{
									flag3 = false;
									break;
								}
							}
							if (!flag3)
							{
								int num2 = 0;
								while (num2 < num && array[num2] == 255 && (!flag2 || array[num2 + num] == 0))
								{
									num2++;
								}
								int num3 = bitmap.bmWidth / 8;
								int num4 = i % num3 * 8;
								i /= num3;
								int num5 = num2 % num3 * 8;
								num2 /= num3;
								width = num4 - num5 + 1;
								height = i - num2 + 1;
								hotX = iconinfo.xHotspot - num5;
								hotY = iconinfo.yHotspot - num2;
							}
							else
							{
								width = bitmap.bmWidth;
								height = bitmap.bmHeight;
								hotX = iconinfo.xHotspot;
								hotY = iconinfo.yHotspot;
							}
						}
					}
					iconinfo.hbmColor.Dispose();
					iconinfo.hbmMask.Dispose();
				}
			}
		}

		// Token: 0x17001CC2 RID: 7362
		// (get) Token: 0x06007C88 RID: 31880 RVA: 0x003100E4 File Offset: 0x0030F0E4
		// (set) Token: 0x06007C89 RID: 31881 RVA: 0x003100F2 File Offset: 0x0030F0F2
		private bool IsTransparent
		{
			get
			{
				return this._cacheValid[2];
			}
			set
			{
				this._cacheValid[2] = value;
			}
		}

		// Token: 0x17001CC3 RID: 7363
		// (get) Token: 0x06007C8A RID: 31882 RVA: 0x00310101 File Offset: 0x0030F101
		// (set) Token: 0x06007C8B RID: 31883 RVA: 0x00310110 File Offset: 0x0030F110
		private bool AnimateFromRight
		{
			get
			{
				return this._cacheValid[32];
			}
			set
			{
				this._cacheValid[32] = value;
			}
		}

		// Token: 0x17001CC4 RID: 7364
		// (get) Token: 0x06007C8C RID: 31884 RVA: 0x00310120 File Offset: 0x0030F120
		// (set) Token: 0x06007C8D RID: 31885 RVA: 0x0031012F File Offset: 0x0030F12F
		private bool AnimateFromBottom
		{
			get
			{
				return this._cacheValid[64];
			}
			set
			{
				this._cacheValid[64] = value;
			}
		}

		// Token: 0x17001CC5 RID: 7365
		// (get) Token: 0x06007C8E RID: 31886 RVA: 0x0031013F File Offset: 0x0030F13F
		// (set) Token: 0x06007C8F RID: 31887 RVA: 0x00310154 File Offset: 0x0030F154
		internal bool HitTestable
		{
			get
			{
				return !this._cacheValid[128];
			}
			set
			{
				this._cacheValid[128] = !value;
			}
		}

		// Token: 0x17001CC6 RID: 7366
		// (get) Token: 0x06007C90 RID: 31888 RVA: 0x0031016A File Offset: 0x0030F16A
		// (set) Token: 0x06007C91 RID: 31889 RVA: 0x0031017C File Offset: 0x0030F17C
		private bool IsDragDropActive
		{
			get
			{
				return this._cacheValid[256];
			}
			set
			{
				this._cacheValid[256] = value;
			}
		}

		// Token: 0x17001CC7 RID: 7367
		// (get) Token: 0x06007C92 RID: 31890 RVA: 0x001A5A01 File Offset: 0x001A4A01
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x04003A71 RID: 14961
		internal static readonly DependencyProperty TreatMousePlacementAsBottomProperty = DependencyProperty.Register("TreatMousePlacementAsBottom", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(false));

		// Token: 0x04003A72 RID: 14962
		public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(Popup), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Popup.OnChildChanged)));

		// Token: 0x04003A73 RID: 14963
		internal static readonly UncommonField<List<Popup>> RegisteredPopupsField = new UncommonField<List<Popup>>();

		// Token: 0x04003A74 RID: 14964
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Popup.OnIsOpenChanged), new CoerceValueCallback(Popup.CoerceIsOpen)));

		// Token: 0x04003A75 RID: 14965
		[CommonDependencyProperty]
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(Popup), new FrameworkPropertyMetadata(PlacementMode.Bottom, new PropertyChangedCallback(Popup.OnPlacementChanged)), new ValidateValueCallback(Popup.IsValidPlacementMode));

		// Token: 0x04003A76 RID: 14966
		public static readonly DependencyProperty CustomPopupPlacementCallbackProperty = DependencyProperty.Register("CustomPopupPlacementCallback", typeof(CustomPopupPlacementCallback), typeof(Popup), new FrameworkPropertyMetadata(null));

		// Token: 0x04003A77 RID: 14967
		public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register("StaysOpen", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(Popup.OnStaysOpenChanged)));

		// Token: 0x04003A78 RID: 14968
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(Popup), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(Popup.OnOffsetChanged)));

		// Token: 0x04003A79 RID: 14969
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(Popup), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(Popup.OnOffsetChanged)));

		// Token: 0x04003A7A RID: 14970
		public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(Popup), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Popup.OnPlacementTargetChanged)));

		// Token: 0x04003A7B RID: 14971
		public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.Register("PlacementRectangle", typeof(Rect), typeof(Popup), new FrameworkPropertyMetadata(Rect.Empty, new PropertyChangedCallback(Popup.OnOffsetChanged)));

		// Token: 0x04003A7C RID: 14972
		[CommonDependencyProperty]
		public static readonly DependencyProperty PopupAnimationProperty = DependencyProperty.Register("PopupAnimation", typeof(PopupAnimation), typeof(Popup), new FrameworkPropertyMetadata(PopupAnimation.None, null, new CoerceValueCallback(Popup.CoercePopupAnimation)), new ValidateValueCallback(Popup.IsValidPopupAnimation));

		// Token: 0x04003A7D RID: 14973
		public static readonly DependencyProperty AllowsTransparencyProperty = Window.AllowsTransparencyProperty.AddOwner(typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Popup.OnAllowsTransparencyChanged), new CoerceValueCallback(Popup.CoerceAllowsTransparency)));

		// Token: 0x04003A7E RID: 14974
		private static readonly DependencyPropertyKey HasDropShadowPropertyKey = DependencyProperty.RegisterReadOnly("HasDropShadow", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, new CoerceValueCallback(Popup.CoerceHasDropShadow)));

		// Token: 0x04003A7F RID: 14975
		public static readonly DependencyProperty HasDropShadowProperty = Popup.HasDropShadowPropertyKey.DependencyProperty;

		// Token: 0x04003A80 RID: 14976
		private static readonly EventPrivateKey OpenedKey = new EventPrivateKey();

		// Token: 0x04003A81 RID: 14977
		private static readonly EventPrivateKey ClosedKey = new EventPrivateKey();

		// Token: 0x04003A83 RID: 14979
		private static readonly UncommonField<Exception> SavedExceptionField = new UncommonField<Exception>();

		// Token: 0x04003A84 RID: 14980
		internal const double Tolerance = 0.01;

		// Token: 0x04003A85 RID: 14981
		private const int AnimationDelay = 150;

		// Token: 0x04003A86 RID: 14982
		internal static TimeSpan AnimationDelayTime = new TimeSpan(0, 0, 0, 0, 150);

		// Token: 0x04003A87 RID: 14983
		internal static RoutedEventHandler CloseOnUnloadedHandler;

		// Token: 0x04003A88 RID: 14984
		private static readonly UncommonField<PopupRoot> ParentPopupRootField = new UncommonField<PopupRoot>();

		// Token: 0x04003A89 RID: 14985
		private Popup.PositionInfo _positionInfo;

		// Token: 0x04003A8A RID: 14986
		private SecurityCriticalDataForSet<PopupRoot> _popupRoot;

		// Token: 0x04003A8B RID: 14987
		private DispatcherOperation _asyncCreate;

		// Token: 0x04003A8C RID: 14988
		private DispatcherTimer _asyncDestroy;

		// Token: 0x04003A8D RID: 14989
		private Popup.PopupSecurityHelper _secHelper;

		// Token: 0x04003A8E RID: 14990
		private BitVector32 _cacheValid = new BitVector32(0);

		// Token: 0x04003A8F RID: 14991
		private const double RestrictPercentage = 0.75;

		// Token: 0x02000C4B RID: 3147
		private class PopupModelTreeEnumerator : ModelTreeEnumerator
		{
			// Token: 0x06009184 RID: 37252 RVA: 0x0034975C File Offset: 0x0034875C
			internal PopupModelTreeEnumerator(Popup popup, object child) : base(child)
			{
				this._popup = popup;
			}

			// Token: 0x17001FDD RID: 8157
			// (get) Token: 0x06009185 RID: 37253 RVA: 0x0034976C File Offset: 0x0034876C
			protected override bool IsUnchanged
			{
				get
				{
					return base.Content == this._popup.Child;
				}
			}

			// Token: 0x04004C33 RID: 19507
			private Popup _popup;
		}

		// Token: 0x02000C4C RID: 3148
		private enum InterestPoint
		{
			// Token: 0x04004C35 RID: 19509
			TopLeft,
			// Token: 0x04004C36 RID: 19510
			TopRight,
			// Token: 0x04004C37 RID: 19511
			BottomLeft,
			// Token: 0x04004C38 RID: 19512
			BottomRight,
			// Token: 0x04004C39 RID: 19513
			Center
		}

		// Token: 0x02000C4D RID: 3149
		private struct PointCombination
		{
			// Token: 0x06009186 RID: 37254 RVA: 0x00349781 File Offset: 0x00348781
			public PointCombination(Popup.InterestPoint targetInterestPoint, Popup.InterestPoint childInterestPoint)
			{
				this.TargetInterestPoint = targetInterestPoint;
				this.ChildInterestPoint = childInterestPoint;
			}

			// Token: 0x04004C3A RID: 19514
			public Popup.InterestPoint TargetInterestPoint;

			// Token: 0x04004C3B RID: 19515
			public Popup.InterestPoint ChildInterestPoint;
		}

		// Token: 0x02000C4E RID: 3150
		private class PositionInfo
		{
			// Token: 0x04004C3C RID: 19516
			public int X;

			// Token: 0x04004C3D RID: 19517
			public int Y;

			// Token: 0x04004C3E RID: 19518
			public Size ChildSize;

			// Token: 0x04004C3F RID: 19519
			public Rect MouseRect = Rect.Empty;
		}

		// Token: 0x02000C4F RID: 3151
		private enum CacheBits
		{
			// Token: 0x04004C41 RID: 19521
			CaptureEngaged = 1,
			// Token: 0x04004C42 RID: 19522
			IsTransparent,
			// Token: 0x04004C43 RID: 19523
			OnClosedHandlerReopen = 4,
			// Token: 0x04004C44 RID: 19524
			DropOppositeSet = 8,
			// Token: 0x04004C45 RID: 19525
			DropOpposite = 16,
			// Token: 0x04004C46 RID: 19526
			AnimateFromRight = 32,
			// Token: 0x04004C47 RID: 19527
			AnimateFromBottom = 64,
			// Token: 0x04004C48 RID: 19528
			HitTestable = 128,
			// Token: 0x04004C49 RID: 19529
			IsDragDropActive = 256,
			// Token: 0x04004C4A RID: 19530
			IsIgnoringMouseEvents = 512
		}

		// Token: 0x02000C50 RID: 3152
		private class PopupSecurityHelper
		{
			// Token: 0x06009188 RID: 37256 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
			internal PopupSecurityHelper()
			{
			}

			// Token: 0x17001FDE RID: 8158
			// (get) Token: 0x06009189 RID: 37257 RVA: 0x003497A4 File Offset: 0x003487A4
			internal bool IsChildPopup
			{
				get
				{
					if (!this._isChildPopupInitialized)
					{
						this._isChildPopup = false;
						this._isChildPopupInitialized = true;
					}
					return this._isChildPopup;
				}
			}

			// Token: 0x0600918A RID: 37258 RVA: 0x003497C4 File Offset: 0x003487C4
			internal bool IsWindowAlive()
			{
				if (this._window != null)
				{
					HwndSource value = this._window.Value;
					return value != null && !value.IsDisposed;
				}
				return false;
			}

			// Token: 0x0600918B RID: 37259 RVA: 0x003497F8 File Offset: 0x003487F8
			internal Point ClientToScreen(Visual rootVisual, Point clientPoint)
			{
				HwndSource hwndSource = Popup.PopupSecurityHelper.GetPresentationSource(rootVisual) as HwndSource;
				if (hwndSource != null)
				{
					return PointUtil.ToPoint(this.ClientToScreen(hwndSource, clientPoint));
				}
				return clientPoint;
			}

			// Token: 0x0600918C RID: 37260 RVA: 0x00349824 File Offset: 0x00348824
			private NativeMethods.POINT ClientToScreen(HwndSource hwnd, Point clientPt)
			{
				bool isChildPopup = this.IsChildPopup;
				HwndSource hwndSource = null;
				if (isChildPopup)
				{
					hwndSource = HwndSource.CriticalFromHwnd(this.ParentHandle);
				}
				Point pointScreen = clientPt;
				if (!isChildPopup || hwndSource != hwnd)
				{
					pointScreen = PointUtil.ClientToScreen(clientPt, hwnd);
				}
				if (isChildPopup && hwndSource != hwnd)
				{
					pointScreen = PointUtil.ScreenToClient(pointScreen, hwndSource);
				}
				return new NativeMethods.POINT((int)pointScreen.X, (int)pointScreen.Y);
			}

			// Token: 0x0600918D RID: 37261 RVA: 0x0034987C File Offset: 0x0034887C
			internal NativeMethods.POINT GetMouseCursorPos(Visual targetVisual)
			{
				if (Mouse.DirectlyOver != null)
				{
					HwndSource hwndSource = null;
					if (targetVisual != null)
					{
						hwndSource = (Popup.PopupSecurityHelper.GetPresentationSource(targetVisual) as HwndSource);
					}
					IInputElement inputElement = targetVisual as IInputElement;
					if (inputElement != null)
					{
						Point point = Mouse.GetPosition(inputElement);
						if (hwndSource != null && !hwndSource.IsDisposed)
						{
							Visual rootVisual = hwndSource.RootVisual;
							CompositionTarget compositionTarget = hwndSource.CompositionTarget;
							if (rootVisual != null && compositionTarget != null)
							{
								GeneralTransform generalTransform = targetVisual.TransformToAncestor(rootVisual);
								Matrix matrix = PointUtil.GetVisualTransform(rootVisual) * compositionTarget.TransformToDevice;
								generalTransform.TryTransform(point, out point);
								point = matrix.Transform(point);
								return this.ClientToScreen(hwndSource, point);
							}
						}
					}
				}
				NativeMethods.POINT point2 = new NativeMethods.POINT(0, 0);
				UnsafeNativeMethods.TryGetCursorPos(point2);
				return point2;
			}

			// Token: 0x0600918E RID: 37262 RVA: 0x00349918 File Offset: 0x00348918
			internal void SetPopupPos(bool position, int x, int y, bool size, int width, int height)
			{
				int num = 20;
				if (!position)
				{
					num |= 2;
				}
				if (!size)
				{
					num |= 1;
				}
				UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.Handle), new HandleRef(null, IntPtr.Zero), x, y, width, height, num);
			}

			// Token: 0x0600918F RID: 37263 RVA: 0x0034995C File Offset: 0x0034895C
			internal Rect GetParentWindowRect()
			{
				NativeMethods.RECT rc = new NativeMethods.RECT(0, 0, 0, 0);
				IntPtr parentHandle = this.ParentHandle;
				if (parentHandle != IntPtr.Zero)
				{
					SafeNativeMethods.GetClientRect(new HandleRef(null, parentHandle), ref rc);
				}
				return PointUtil.ToRect(rc);
			}

			// Token: 0x06009190 RID: 37264 RVA: 0x0034999C File Offset: 0x0034899C
			internal Matrix GetTransformToDevice()
			{
				CompositionTarget compositionTarget = this._window.Value.CompositionTarget;
				if (compositionTarget != null && !compositionTarget.IsDisposed)
				{
					return compositionTarget.TransformToDevice;
				}
				return Matrix.Identity;
			}

			// Token: 0x06009191 RID: 37265 RVA: 0x003499D4 File Offset: 0x003489D4
			internal static Matrix GetTransformToDevice(Visual targetVisual)
			{
				HwndSource hwndSource = null;
				if (targetVisual != null)
				{
					hwndSource = (Popup.PopupSecurityHelper.GetPresentationSource(targetVisual) as HwndSource);
				}
				if (hwndSource != null)
				{
					CompositionTarget compositionTarget = hwndSource.CompositionTarget;
					if (compositionTarget != null && !compositionTarget.IsDisposed)
					{
						return compositionTarget.TransformToDevice;
					}
				}
				return Matrix.Identity;
			}

			// Token: 0x06009192 RID: 37266 RVA: 0x00349A14 File Offset: 0x00348A14
			internal Matrix GetTransformFromDevice()
			{
				CompositionTarget compositionTarget = this._window.Value.CompositionTarget;
				if (compositionTarget != null && !compositionTarget.IsDisposed)
				{
					return compositionTarget.TransformFromDevice;
				}
				return Matrix.Identity;
			}

			// Token: 0x06009193 RID: 37267 RVA: 0x00349A49 File Offset: 0x00348A49
			internal void SetWindowRootVisual(Visual v)
			{
				this._window.Value.RootVisual = v;
			}

			// Token: 0x06009194 RID: 37268 RVA: 0x00349A5C File Offset: 0x00348A5C
			internal static bool IsVisualPresentationSourceNull(Visual visual)
			{
				return Popup.PopupSecurityHelper.GetPresentationSource(visual) == null;
			}

			// Token: 0x06009195 RID: 37269 RVA: 0x00349A68 File Offset: 0x00348A68
			internal void ShowWindow()
			{
				if (this.IsChildPopup)
				{
					IntPtr lastWebOCHwnd = this.GetLastWebOCHwnd();
					UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.Handle), (lastWebOCHwnd == IntPtr.Zero) ? NativeMethods.HWND_TOP : new HandleRef(null, lastWebOCHwnd), 0, 0, 0, 0, 83);
					return;
				}
				if (!FrameworkCompatibilityPreferences.GetUseSetWindowPosForTopmostWindows())
				{
					UnsafeNativeMethods.ShowWindow(new HandleRef(null, this.Handle), 8);
					return;
				}
				UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 595);
			}

			// Token: 0x06009196 RID: 37270 RVA: 0x00349AF3 File Offset: 0x00348AF3
			internal void HideWindow()
			{
				UnsafeNativeMethods.ShowWindow(new HandleRef(null, this.Handle), 0);
			}

			// Token: 0x06009197 RID: 37271 RVA: 0x00349B08 File Offset: 0x00348B08
			private IntPtr GetLastWebOCHwnd()
			{
				IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(null, this.Handle), 1);
				StringBuilder stringBuilder = new StringBuilder(260);
				while (window != IntPtr.Zero)
				{
					if (UnsafeNativeMethods.GetClassName(new HandleRef(null, window), stringBuilder, 260) == 0)
					{
						throw new Win32Exception();
					}
					if (string.Compare(stringBuilder.ToString(), "Shell Embedding", StringComparison.OrdinalIgnoreCase) == 0)
					{
						break;
					}
					window = UnsafeNativeMethods.GetWindow(new HandleRef(null, window), 3);
				}
				return window;
			}

			// Token: 0x06009198 RID: 37272 RVA: 0x00349B80 File Offset: 0x00348B80
			internal void SetHitTestable(bool hitTestable)
			{
				IntPtr handle = this.Handle;
				int num = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -20);
				int num2 = num;
				if ((num2 & 32) == 0 != hitTestable)
				{
					if (hitTestable)
					{
						num = (num2 & -33);
					}
					else
					{
						num = (num2 | 32);
					}
					UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(null, handle), -20, (IntPtr)num);
				}
			}

			// Token: 0x06009199 RID: 37273 RVA: 0x00349BD4 File Offset: 0x00348BD4
			private static Visual FindMainTreeVisual(Visual v)
			{
				DependencyObject dependencyObject = null;
				DependencyObject dependencyObject2 = v;
				while (dependencyObject2 != null)
				{
					dependencyObject = dependencyObject2;
					PopupRoot popupRoot = dependencyObject2 as PopupRoot;
					if (popupRoot != null)
					{
						dependencyObject2 = popupRoot.Parent;
						Popup popup = dependencyObject2 as Popup;
						if (popup != null)
						{
							UIElement placementTarget = popup.PlacementTarget;
							if (placementTarget != null)
							{
								dependencyObject2 = placementTarget;
							}
						}
					}
					else
					{
						dependencyObject2 = VisualTreeHelper.GetParent(dependencyObject2);
					}
				}
				return dependencyObject as Visual;
			}

			// Token: 0x0600919A RID: 37274 RVA: 0x00349C28 File Offset: 0x00348C28
			internal void BuildWindow(int x, int y, Visual placementTarget, bool transparent, HwndSourceHook hook, AutoResizedEventHandler handler, HwndDpiChangedEventHandler dpiChangedHandler)
			{
				transparent = (transparent && !this.IsChildPopup);
				Visual visual = placementTarget;
				if (this.IsChildPopup)
				{
					visual = Popup.PopupSecurityHelper.FindMainTreeVisual(placementTarget);
				}
				HwndSource hwndSource = Popup.PopupSecurityHelper.GetPresentationSource(visual) as HwndSource;
				IntPtr intPtr = IntPtr.Zero;
				if (hwndSource != null)
				{
					intPtr = Popup.PopupSecurityHelper.GetHandle(hwndSource);
				}
				int windowClassStyle = 0;
				int num = 67108864;
				int num2 = 134217856;
				if (this.IsChildPopup)
				{
					num |= 1073741824;
				}
				else
				{
					num |= int.MinValue;
					num2 |= 8;
				}
				HwndSourceParameters parameters = new HwndSourceParameters(string.Empty);
				parameters.WindowClassStyle = windowClassStyle;
				parameters.WindowStyle = num;
				parameters.ExtendedWindowStyle = num2;
				parameters.SetPosition(x, y);
				if (this.IsChildPopup)
				{
					if (intPtr != IntPtr.Zero)
					{
						parameters.ParentWindow = intPtr;
					}
				}
				else
				{
					parameters.UsesPerPixelOpacity = transparent;
					if (intPtr != IntPtr.Zero && Popup.PopupSecurityHelper.ConnectedToForegroundWindow(intPtr))
					{
						parameters.ParentWindow = intPtr;
					}
				}
				HwndSource hwndSource2 = new HwndSource(parameters);
				hwndSource2.AddHook(hook);
				this._window = new SecurityCriticalDataClass<HwndSource>(hwndSource2);
				hwndSource2.CompositionTarget.BackgroundColor = (transparent ? Colors.Transparent : Colors.Black);
				hwndSource2.AutoResized += handler;
				hwndSource2.DpiChanged += dpiChangedHandler;
			}

			// Token: 0x0600919B RID: 37275 RVA: 0x00349D6C File Offset: 0x00348D6C
			private static bool ConnectedToForegroundWindow(IntPtr window)
			{
				IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
				while (window != IntPtr.Zero)
				{
					if (window == foregroundWindow)
					{
						return true;
					}
					window = UnsafeNativeMethods.GetParent(new HandleRef(null, window));
				}
				return false;
			}

			// Token: 0x0600919C RID: 37276 RVA: 0x00349DA8 File Offset: 0x00348DA8
			private static IntPtr GetHandle(HwndSource hwnd)
			{
				if (hwnd == null)
				{
					return IntPtr.Zero;
				}
				return hwnd.CriticalHandle;
			}

			// Token: 0x0600919D RID: 37277 RVA: 0x00349DBC File Offset: 0x00348DBC
			private static IntPtr GetParentHandle(HwndSource hwnd)
			{
				if (hwnd != null)
				{
					IntPtr handle = Popup.PopupSecurityHelper.GetHandle(hwnd);
					if (handle != IntPtr.Zero)
					{
						return UnsafeNativeMethods.GetParent(new HandleRef(null, handle));
					}
				}
				return IntPtr.Zero;
			}

			// Token: 0x17001FDF RID: 8159
			// (get) Token: 0x0600919E RID: 37278 RVA: 0x00349DF2 File Offset: 0x00348DF2
			private IntPtr Handle
			{
				get
				{
					return Popup.PopupSecurityHelper.GetHandle(this._window.Value);
				}
			}

			// Token: 0x17001FE0 RID: 8160
			// (get) Token: 0x0600919F RID: 37279 RVA: 0x00349E04 File Offset: 0x00348E04
			private IntPtr ParentHandle
			{
				get
				{
					return Popup.PopupSecurityHelper.GetParentHandle(this._window.Value);
				}
			}

			// Token: 0x060091A0 RID: 37280 RVA: 0x00349E16 File Offset: 0x00348E16
			private static PresentationSource GetPresentationSource(Visual visual)
			{
				if (visual == null)
				{
					return null;
				}
				return PresentationSource.CriticalFromVisual(visual);
			}

			// Token: 0x060091A1 RID: 37281 RVA: 0x00349E24 File Offset: 0x00348E24
			internal void ForceMsaaToUiaBridge(PopupRoot popupRoot)
			{
				if (this.Handle != IntPtr.Zero && (UnsafeNativeMethods.IsWinEventHookInstalled(32773) || UnsafeNativeMethods.IsWinEventHookInstalled(32778)))
				{
					PopupRootAutomationPeer popupRootAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(popupRoot) as PopupRootAutomationPeer;
					if (popupRootAutomationPeer != null)
					{
						if (popupRootAutomationPeer.Hwnd == IntPtr.Zero)
						{
							popupRootAutomationPeer.Hwnd = this.Handle;
						}
						IRawElementProviderSimple el = popupRootAutomationPeer.ProviderFromPeer(popupRootAutomationPeer);
						IntPtr intPtr = AutomationInteropProvider.ReturnRawElementProvider(this.Handle, IntPtr.Zero, new IntPtr(-4), el);
						if (intPtr != IntPtr.Zero)
						{
							IAccessible accessible = null;
							Guid guid = new Guid("618736e0-3c3d-11cf-810c-00aa00389b71");
							if (UnsafeNativeMethods.ObjectFromLresult(intPtr, ref guid, IntPtr.Zero, ref accessible) == 0)
							{
							}
						}
					}
				}
			}

			// Token: 0x060091A2 RID: 37282 RVA: 0x00349EDC File Offset: 0x00348EDC
			internal void DestroyWindow(HwndSourceHook hook, AutoResizedEventHandler onAutoResizedEventHandler, HwndDpiChangedEventHandler onDpiChagnedEventHandler)
			{
				HwndSource value = this._window.Value;
				this._window = null;
				if (!value.IsDisposed)
				{
					value.AutoResized -= onAutoResizedEventHandler;
					value.DpiChanged -= onDpiChagnedEventHandler;
					value.RemoveHook(hook);
					value.RootVisual = null;
					value.Dispose();
				}
			}

			// Token: 0x04004C4B RID: 19531
			private bool _isChildPopup;

			// Token: 0x04004C4C RID: 19532
			private bool _isChildPopupInitialized;

			// Token: 0x04004C4D RID: 19533
			private SecurityCriticalDataClass<HwndSource> _window;

			// Token: 0x04004C4E RID: 19534
			private const string WebOCWindowClassName = "Shell Embedding";
		}

		// Token: 0x02000C51 RID: 3153
		private static class PopupInitialPlacementHelper
		{
			// Token: 0x17001FE1 RID: 8161
			// (get) Token: 0x060091A3 RID: 37283 RVA: 0x00349F28 File Offset: 0x00348F28
			internal static bool IsPerMonitorDpiScalingActive
			{
				get
				{
					if (!HwndTarget.IsPerMonitorDpiScalingEnabled)
					{
						return false;
					}
					if (HwndTarget.IsProcessPerMonitorDpiAware != null)
					{
						return HwndTarget.IsProcessPerMonitorDpiAware.Value;
					}
					return DpiUtil.GetProcessDpiAwareness(IntPtr.Zero) == NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE;
				}
			}

			// Token: 0x060091A4 RID: 37284 RVA: 0x00349F68 File Offset: 0x00348F68
			private static NativeMethods.POINTSTRUCT? GetPlacementTargetOriginInScreenCoordinates(Popup popup)
			{
				UIElement uielement = ((popup != null) ? popup.GetTarget() : null) as UIElement;
				if (uielement != null)
				{
					Visual rootVisual = Popup.GetRootVisual(uielement);
					Point clientPoint;
					if (Popup.TransformToClient(uielement, rootVisual).TryTransform(new Point(0.0, 0.0), out clientPoint))
					{
						Point point = popup._secHelper.ClientToScreen(rootVisual, clientPoint);
						return new NativeMethods.POINTSTRUCT?(new NativeMethods.POINTSTRUCT((int)point.X, (int)point.Y));
					}
				}
				return null;
			}

			// Token: 0x060091A5 RID: 37285 RVA: 0x00349FEC File Offset: 0x00348FEC
			internal static NativeMethods.POINTSTRUCT GetPlacementOrigin(Popup popup)
			{
				NativeMethods.POINTSTRUCT result = new NativeMethods.POINTSTRUCT(0, 0);
				if (Popup.PopupInitialPlacementHelper.IsPerMonitorDpiScalingActive)
				{
					NativeMethods.POINTSTRUCT? placementTargetOriginInScreenCoordinates = Popup.PopupInitialPlacementHelper.GetPlacementTargetOriginInScreenCoordinates(popup);
					if (placementTargetOriginInScreenCoordinates != null)
					{
						try
						{
							IntPtr handle = SafeNativeMethods.MonitorFromPoint(placementTargetOriginInScreenCoordinates.Value, 2);
							NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
							SafeNativeMethods.GetMonitorInfo(new HandleRef(null, handle), monitorinfoex);
							result.x = monitorinfoex.rcMonitor.left;
							result.y = monitorinfoex.rcMonitor.top;
						}
						catch (Win32Exception)
						{
						}
					}
				}
				return result;
			}
		}
	}
}
