using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Controls;
using MS.Internal.Ink;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x02000796 RID: 1942
	[ContentProperty("Children")]
	public class InkCanvas : FrameworkElement, IAddChild
	{
		// Token: 0x06006BEF RID: 27631 RVA: 0x002C7364 File Offset: 0x002C6364
		static InkCanvas()
		{
			InkCanvas.StrokeCollectedEvent = EventManager.RegisterRoutedEvent("StrokeCollected", RoutingStrategy.Bubble, typeof(InkCanvasStrokeCollectedEventHandler), typeof(InkCanvas));
			InkCanvas.GestureEvent = EventManager.RegisterRoutedEvent("Gesture", RoutingStrategy.Bubble, typeof(InkCanvasGestureEventHandler), typeof(InkCanvas));
			InkCanvas.ActiveEditingModeChangedEvent = EventManager.RegisterRoutedEvent("ActiveEditingModeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.EditingModeChangedEvent = EventManager.RegisterRoutedEvent("EditingModeChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.EditingModeInvertedChangedEvent = EventManager.RegisterRoutedEvent("EditingModeInvertedChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.StrokeErasedEvent = EventManager.RegisterRoutedEvent("StrokeErased", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InkCanvas));
			InkCanvas.DeselectCommand = new RoutedCommand("Deselect", typeof(InkCanvas));
			Type typeFromHandle = typeof(InkCanvas);
			EventManager.RegisterClassHandler(typeFromHandle, Stylus.StylusDownEvent, new StylusDownEventHandler(InkCanvas._OnDeviceDown<StylusDownEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.MouseDownEvent, new MouseButtonEventHandler(InkCanvas._OnDeviceDown<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Stylus.StylusUpEvent, new StylusEventHandler(InkCanvas._OnDeviceUp<StylusEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.MouseUpEvent, new MouseButtonEventHandler(InkCanvas._OnDeviceUp<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.QueryCursorEvent, new QueryCursorEventHandler(InkCanvas._OnQueryCursor), true);
			InkCanvas._RegisterClipboardHandlers();
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Delete, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled));
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.SelectAll, Key.A, ModifierKeys.Control, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled));
			CommandHelpers.RegisterCommandHandler(typeFromHandle, InkCanvas.DeselectCommand, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled), KeyGesture.CreateFromResourceStrings("Esc", "InkCanvasDeselectKeyDisplayString"));
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			UIElement.FocusableProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			Style style = new Style(typeFromHandle);
			style.Setters.Add(new Setter(InkCanvas.BackgroundProperty, new DynamicResourceExtension(SystemColors.WindowBrushKey)));
			style.Setters.Add(new Setter(Stylus.IsFlicksEnabledProperty, false));
			style.Setters.Add(new Setter(Stylus.IsTapFeedbackEnabledProperty, false));
			style.Setters.Add(new Setter(Stylus.IsTouchFeedbackEnabledProperty, false));
			Trigger trigger = new Trigger();
			trigger.Property = FrameworkElement.WidthProperty;
			trigger.Value = double.NaN;
			Setter setter = new Setter();
			setter.Property = FrameworkElement.MinWidthProperty;
			setter.Value = 350.0;
			trigger.Setters.Add(setter);
			style.Triggers.Add(trigger);
			trigger = new Trigger();
			trigger.Property = FrameworkElement.HeightProperty;
			trigger.Value = double.NaN;
			setter = new Setter();
			setter.Property = FrameworkElement.MinHeightProperty;
			setter.Value = 250.0;
			trigger.Setters.Add(setter);
			style.Triggers.Add(trigger);
			style.Seal();
			FrameworkElement.StyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(style));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(typeof(InkCanvas)));
			FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null));
		}

		// Token: 0x06006BF0 RID: 27632 RVA: 0x002C79A2 File Offset: 0x002C69A2
		public InkCanvas()
		{
			this.Initialize();
		}

		// Token: 0x06006BF1 RID: 27633 RVA: 0x002C79B0 File Offset: 0x002C69B0
		private void Initialize()
		{
			this._dynamicRenderer = new DynamicRenderer();
			this._dynamicRenderer.Enabled = false;
			base.StylusPlugIns.Add(this._dynamicRenderer);
			this._editingCoordinator = new EditingCoordinator(this);
			this._editingCoordinator.UpdateActiveEditingState();
			this.DefaultDrawingAttributes.AttributeChanged += this.DefaultDrawingAttributes_Changed;
			this.InitializeInkObject();
			this._rtiHighContrastCallback = new InkCanvas.RTIHighContrastCallback(this);
			HighContrastHelper.RegisterHighContrastCallback(this._rtiHighContrastCallback);
			if (SystemParameters.HighContrast)
			{
				this._rtiHighContrastCallback.TurnHighContrastOn(SystemColors.WindowTextColor);
			}
		}

		// Token: 0x06006BF2 RID: 27634 RVA: 0x002C7A47 File Offset: 0x002C6A47
		private void InitializeInkObject()
		{
			this.UpdateDynamicRenderer();
			this._defaultStylusPointDescription = new StylusPointDescription();
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x002C7A5A File Offset: 0x002C6A5A
		protected override Size MeasureOverride(Size availableSize)
		{
			if (this._localAdornerDecorator == null)
			{
				base.ApplyTemplate();
			}
			this._localAdornerDecorator.Measure(availableSize);
			return this._localAdornerDecorator.DesiredSize;
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x002C7A82 File Offset: 0x002C6A82
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (this._localAdornerDecorator == null)
			{
				base.ApplyTemplate();
			}
			this._localAdornerDecorator.Arrange(new Rect(arrangeSize));
			return arrangeSize;
		}

		// Token: 0x06006BF5 RID: 27637 RVA: 0x002C7AA8 File Offset: 0x002C6AA8
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParams)
		{
			base.VerifyAccess();
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(hitTestParams.HitPoint))
			{
				return new PointHitTestResult(this, hitTestParams.HitPoint);
			}
			return null;
		}

		// Token: 0x06006BF6 RID: 27638 RVA: 0x002C7AF0 File Offset: 0x002C6AF0
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.IsAValueChange || e.IsASubPropertyChange)
			{
				if (e.Property == UIElement.RenderTransformProperty || e.Property == FrameworkElement.LayoutTransformProperty)
				{
					this.EditingCoordinator.InvalidateTransform();
					Transform transform = e.NewValue as Transform;
					if (transform != null && !transform.HasAnimatedProperties)
					{
						TransformGroup transformGroup = transform as TransformGroup;
						if (transformGroup != null)
						{
							Stack<Transform> stack = new Stack<Transform>();
							stack.Push(transform);
							while (stack.Count > 0)
							{
								transform = stack.Pop();
								if (transform.HasAnimatedProperties)
								{
									return;
								}
								transformGroup = (transform as TransformGroup);
								if (transformGroup != null)
								{
									for (int i = 0; i < transformGroup.Children.Count; i++)
									{
										stack.Push(transformGroup.Children[i]);
									}
								}
							}
						}
						this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
						this.EditingCoordinator.UpdatePointEraserCursor();
					}
				}
				if (e.Property == FrameworkElement.FlowDirectionProperty)
				{
					this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
				}
			}
		}

		// Token: 0x06006BF7 RID: 27639 RVA: 0x002C7C0C File Offset: 0x002C6C0C
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (this._localAdornerDecorator == null)
			{
				this._localAdornerDecorator = new AdornerDecorator();
				InkPresenter inkPresenter = this.InkPresenter;
				base.AddVisualChild(this._localAdornerDecorator);
				this._localAdornerDecorator.Child = inkPresenter;
				inkPresenter.Child = this.InnerCanvas;
				this._localAdornerDecorator.AdornerLayer.Add(this.SelectionAdorner);
			}
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x06006BF8 RID: 27640 RVA: 0x002C7C73 File Offset: 0x002C6C73
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._localAdornerDecorator != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06006BF9 RID: 27641 RVA: 0x002C7C80 File Offset: 0x002C6C80
		protected override Visual GetVisualChild(int index)
		{
			if (this._localAdornerDecorator == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._localAdornerDecorator;
		}

		// Token: 0x06006BFA RID: 27642 RVA: 0x002C7CAE File Offset: 0x002C6CAE
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new InkCanvasAutomationPeer(this);
		}

		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x06006BFB RID: 27643 RVA: 0x002C7CB6 File Offset: 0x002C6CB6
		// (set) Token: 0x06006BFC RID: 27644 RVA: 0x002C7CC8 File Offset: 0x002C6CC8
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(InkCanvas.BackgroundProperty);
			}
			set
			{
				base.SetValue(InkCanvas.BackgroundProperty, value);
			}
		}

		// Token: 0x06006BFD RID: 27645 RVA: 0x002C7CD6 File Offset: 0x002C6CD6
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetTop(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.TopProperty);
		}

		// Token: 0x06006BFE RID: 27646 RVA: 0x002C7CF6 File Offset: 0x002C6CF6
		public static void SetTop(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.TopProperty, length);
		}

		// Token: 0x06006BFF RID: 27647 RVA: 0x002C7D17 File Offset: 0x002C6D17
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetBottom(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.BottomProperty);
		}

		// Token: 0x06006C00 RID: 27648 RVA: 0x002C7D37 File Offset: 0x002C6D37
		public static void SetBottom(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.BottomProperty, length);
		}

		// Token: 0x06006C01 RID: 27649 RVA: 0x002C7D58 File Offset: 0x002C6D58
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetLeft(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.LeftProperty);
		}

		// Token: 0x06006C02 RID: 27650 RVA: 0x002C7D78 File Offset: 0x002C6D78
		public static void SetLeft(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.LeftProperty, length);
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x002C7D99 File Offset: 0x002C6D99
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		[AttachedPropertyBrowsableForChildren]
		public static double GetRight(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(InkCanvas.RightProperty);
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x002C7DB9 File Offset: 0x002C6DB9
		public static void SetRight(UIElement element, double length)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(InkCanvas.RightProperty, length);
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x002C7DDC File Offset: 0x002C6DDC
		private static void OnPositioningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				InkCanvasInnerCanvas inkCanvasInnerCanvas = VisualTreeHelper.GetParent(uielement) as InkCanvasInnerCanvas;
				if (inkCanvasInnerCanvas != null)
				{
					if (e.Property == InkCanvas.LeftProperty || e.Property == InkCanvas.TopProperty)
					{
						inkCanvasInnerCanvas.InvalidateMeasure();
						return;
					}
					inkCanvasInnerCanvas.InvalidateArrange();
				}
			}
		}

		// Token: 0x170018F6 RID: 6390
		// (get) Token: 0x06006C06 RID: 27654 RVA: 0x002C7E2B File Offset: 0x002C6E2B
		// (set) Token: 0x06006C07 RID: 27655 RVA: 0x002C7E3D File Offset: 0x002C6E3D
		public StrokeCollection Strokes
		{
			get
			{
				return (StrokeCollection)base.GetValue(InkCanvas.StrokesProperty);
			}
			set
			{
				base.SetValue(InkCanvas.StrokesProperty, value);
			}
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x002C7E4C File Offset: 0x002C6E4C
		private static void OnStrokesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			InkCanvas inkCanvas = (InkCanvas)d;
			StrokeCollection strokeCollection = (StrokeCollection)e.OldValue;
			StrokeCollection strokeCollection2 = (StrokeCollection)e.NewValue;
			if (strokeCollection != strokeCollection2)
			{
				inkCanvas.CoreChangeSelection(new StrokeCollection(), inkCanvas.InkCanvasSelection.SelectedElements, false);
				inkCanvas.InitializeInkObject();
				InkCanvasStrokesReplacedEventArgs e2 = new InkCanvasStrokesReplacedEventArgs(strokeCollection2, strokeCollection);
				inkCanvas.OnStrokesReplaced(e2);
			}
		}

		// Token: 0x170018F7 RID: 6391
		// (get) Token: 0x06006C09 RID: 27657 RVA: 0x002C7EAC File Offset: 0x002C6EAC
		internal InkCanvasSelectionAdorner SelectionAdorner
		{
			get
			{
				if (this._selectionAdorner == null)
				{
					this._selectionAdorner = new InkCanvasSelectionAdorner(this.InnerCanvas);
					Binding binding = new Binding();
					binding.Path = new PropertyPath(InkCanvas.ActiveEditingModeProperty);
					binding.Mode = BindingMode.OneWay;
					binding.Source = this;
					binding.Converter = new InkCanvas.ActiveEditingMode2VisibilityConverter();
					this._selectionAdorner.SetBinding(UIElement.VisibilityProperty, binding);
				}
				return this._selectionAdorner;
			}
		}

		// Token: 0x170018F8 RID: 6392
		// (get) Token: 0x06006C0A RID: 27658 RVA: 0x002C7F19 File Offset: 0x002C6F19
		internal InkCanvasFeedbackAdorner FeedbackAdorner
		{
			get
			{
				base.VerifyAccess();
				if (this._feedbackAdorner == null)
				{
					this._feedbackAdorner = new InkCanvasFeedbackAdorner(this);
				}
				return this._feedbackAdorner;
			}
		}

		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x06006C0B RID: 27659 RVA: 0x002C7F3B File Offset: 0x002C6F3B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsGestureRecognizerAvailable
		{
			get
			{
				return this.GestureRecognizer.IsRecognizerAvailable;
			}
		}

		// Token: 0x170018FA RID: 6394
		// (get) Token: 0x06006C0C RID: 27660 RVA: 0x002C7F48 File Offset: 0x002C6F48
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public UIElementCollection Children
		{
			get
			{
				return this.InnerCanvas.Children;
			}
		}

		// Token: 0x170018FB RID: 6395
		// (get) Token: 0x06006C0D RID: 27661 RVA: 0x002C7F55 File Offset: 0x002C6F55
		// (set) Token: 0x06006C0E RID: 27662 RVA: 0x002C7F67 File Offset: 0x002C6F67
		public DrawingAttributes DefaultDrawingAttributes
		{
			get
			{
				return (DrawingAttributes)base.GetValue(InkCanvas.DefaultDrawingAttributesProperty);
			}
			set
			{
				base.SetValue(InkCanvas.DefaultDrawingAttributesProperty, value);
			}
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x002C7F78 File Offset: 0x002C6F78
		private static void OnDefaultDrawingAttributesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			InkCanvas inkCanvas = (InkCanvas)d;
			DrawingAttributes drawingAttributes = (DrawingAttributes)e.OldValue;
			DrawingAttributes drawingAttributes2 = (DrawingAttributes)e.NewValue;
			inkCanvas.UpdateDynamicRenderer(drawingAttributes2);
			if (drawingAttributes != drawingAttributes2)
			{
				drawingAttributes.AttributeChanged -= inkCanvas.DefaultDrawingAttributes_Changed;
				DrawingAttributesReplacedEventArgs e2 = new DrawingAttributesReplacedEventArgs(drawingAttributes2, drawingAttributes);
				drawingAttributes2.AttributeChanged += inkCanvas.DefaultDrawingAttributes_Changed;
				inkCanvas.RaiseDefaultDrawingAttributeReplaced(e2);
			}
		}

		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x06006C10 RID: 27664 RVA: 0x002C7FE4 File Offset: 0x002C6FE4
		// (set) Token: 0x06006C11 RID: 27665 RVA: 0x002C8018 File Offset: 0x002C7018
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StylusShape EraserShape
		{
			get
			{
				base.VerifyAccess();
				if (this._eraserShape == null)
				{
					this._eraserShape = new RectangleStylusShape(8.0, 8.0);
				}
				return this._eraserShape;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				StylusShape eraserShape = this.EraserShape;
				this._eraserShape = value;
				if (eraserShape.Width != this._eraserShape.Width || eraserShape.Height != this._eraserShape.Height || eraserShape.Rotation != this._eraserShape.Rotation || eraserShape.GetType() != this._eraserShape.GetType())
				{
					this.EditingCoordinator.UpdatePointEraserCursor();
				}
			}
		}

		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x06006C12 RID: 27666 RVA: 0x002C80A3 File Offset: 0x002C70A3
		public InkCanvasEditingMode ActiveEditingMode
		{
			get
			{
				return (InkCanvasEditingMode)base.GetValue(InkCanvas.ActiveEditingModeProperty);
			}
		}

		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x06006C13 RID: 27667 RVA: 0x002C80B5 File Offset: 0x002C70B5
		// (set) Token: 0x06006C14 RID: 27668 RVA: 0x002C80C7 File Offset: 0x002C70C7
		public InkCanvasEditingMode EditingMode
		{
			get
			{
				return (InkCanvasEditingMode)base.GetValue(InkCanvas.EditingModeProperty);
			}
			set
			{
				base.SetValue(InkCanvas.EditingModeProperty, value);
			}
		}

		// Token: 0x06006C15 RID: 27669 RVA: 0x002C80DA File Offset: 0x002C70DA
		private static void OnEditingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InkCanvas)d).RaiseEditingModeChanged(new RoutedEventArgs(InkCanvas.EditingModeChangedEvent, d));
		}

		// Token: 0x170018FF RID: 6399
		// (get) Token: 0x06006C16 RID: 27670 RVA: 0x002C80F2 File Offset: 0x002C70F2
		// (set) Token: 0x06006C17 RID: 27671 RVA: 0x002C8104 File Offset: 0x002C7104
		public InkCanvasEditingMode EditingModeInverted
		{
			get
			{
				return (InkCanvasEditingMode)base.GetValue(InkCanvas.EditingModeInvertedProperty);
			}
			set
			{
				base.SetValue(InkCanvas.EditingModeInvertedProperty, value);
			}
		}

		// Token: 0x06006C18 RID: 27672 RVA: 0x002C8117 File Offset: 0x002C7117
		private static void OnEditingModeInvertedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InkCanvas)d).RaiseEditingModeInvertedChanged(new RoutedEventArgs(InkCanvas.EditingModeInvertedChangedEvent, d));
		}

		// Token: 0x06006C19 RID: 27673 RVA: 0x002C812F File Offset: 0x002C712F
		private static bool ValidateEditingMode(object value)
		{
			return EditingModeHelper.IsDefined((InkCanvasEditingMode)value);
		}

		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x06006C1A RID: 27674 RVA: 0x002C813C File Offset: 0x002C713C
		// (set) Token: 0x06006C1B RID: 27675 RVA: 0x002C814A File Offset: 0x002C714A
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool UseCustomCursor
		{
			get
			{
				base.VerifyAccess();
				return this._useCustomCursor;
			}
			set
			{
				base.VerifyAccess();
				if (this._useCustomCursor != value)
				{
					this._useCustomCursor = value;
					this.UpdateCursor();
				}
			}
		}

		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x06006C1C RID: 27676 RVA: 0x002C8168 File Offset: 0x002C7168
		// (set) Token: 0x06006C1D RID: 27677 RVA: 0x002C817B File Offset: 0x002C717B
		public bool MoveEnabled
		{
			get
			{
				base.VerifyAccess();
				return this._editingCoordinator.MoveEnabled;
			}
			set
			{
				base.VerifyAccess();
				if (this._editingCoordinator.MoveEnabled != value)
				{
					this._editingCoordinator.MoveEnabled = value;
				}
			}
		}

		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x06006C1E RID: 27678 RVA: 0x002C819D File Offset: 0x002C719D
		// (set) Token: 0x06006C1F RID: 27679 RVA: 0x002C81B0 File Offset: 0x002C71B0
		public bool ResizeEnabled
		{
			get
			{
				base.VerifyAccess();
				return this._editingCoordinator.ResizeEnabled;
			}
			set
			{
				base.VerifyAccess();
				if (this._editingCoordinator.ResizeEnabled != value)
				{
					this._editingCoordinator.ResizeEnabled = value;
				}
			}
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x06006C20 RID: 27680 RVA: 0x002C81D2 File Offset: 0x002C71D2
		// (set) Token: 0x06006C21 RID: 27681 RVA: 0x002C81E0 File Offset: 0x002C71E0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StylusPointDescription DefaultStylusPointDescription
		{
			get
			{
				base.VerifyAccess();
				return this._defaultStylusPointDescription;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._defaultStylusPointDescription = value;
			}
		}

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x06006C22 RID: 27682 RVA: 0x002C81FD File Offset: 0x002C71FD
		// (set) Token: 0x06006C23 RID: 27683 RVA: 0x002C8210 File Offset: 0x002C7210
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable<InkCanvasClipboardFormat> PreferredPasteFormats
		{
			get
			{
				base.VerifyAccess();
				return this.ClipboardProcessor.PreferredFormats;
			}
			set
			{
				base.VerifyAccess();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.ClipboardProcessor.PreferredFormats = value;
			}
		}

		// Token: 0x14000120 RID: 288
		// (add) Token: 0x06006C24 RID: 27684 RVA: 0x002C8232 File Offset: 0x002C7232
		// (remove) Token: 0x06006C25 RID: 27685 RVA: 0x002C8240 File Offset: 0x002C7240
		[Category("Behavior")]
		public event InkCanvasStrokeCollectedEventHandler StrokeCollected
		{
			add
			{
				base.AddHandler(InkCanvas.StrokeCollectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.StrokeCollectedEvent, value);
			}
		}

		// Token: 0x06006C26 RID: 27686 RVA: 0x002C824E File Offset: 0x002C724E
		protected virtual void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06006C27 RID: 27687 RVA: 0x002C8268 File Offset: 0x002C7268
		internal void RaiseGestureOrStrokeCollected(InkCanvasStrokeCollectedEventArgs e, bool userInitiated)
		{
			bool flag = true;
			try
			{
				if (userInitiated && (this.ActiveEditingMode == InkCanvasEditingMode.InkAndGesture || this.ActiveEditingMode == InkCanvasEditingMode.GestureOnly) && this.GestureRecognizer.IsRecognizerAvailable)
				{
					StrokeCollection strokeCollection = new StrokeCollection();
					strokeCollection.Add(e.Stroke);
					ReadOnlyCollection<GestureRecognitionResult> readOnlyCollection = this.GestureRecognizer.CriticalRecognize(strokeCollection);
					if (readOnlyCollection.Count > 0)
					{
						InkCanvasGestureEventArgs inkCanvasGestureEventArgs = new InkCanvasGestureEventArgs(strokeCollection, readOnlyCollection);
						if (readOnlyCollection[0].ApplicationGesture == ApplicationGesture.NoGesture)
						{
							inkCanvasGestureEventArgs.Cancel = true;
						}
						else
						{
							inkCanvasGestureEventArgs.Cancel = false;
						}
						this.OnGesture(inkCanvasGestureEventArgs);
						if (!inkCanvasGestureEventArgs.Cancel)
						{
							flag = false;
							return;
						}
					}
				}
				flag = false;
				if (this.ActiveEditingMode == InkCanvasEditingMode.Ink || this.ActiveEditingMode == InkCanvasEditingMode.InkAndGesture)
				{
					this.Strokes.Add(e.Stroke);
					this.OnStrokeCollected(e);
				}
			}
			finally
			{
				if (flag)
				{
					this.Strokes.Add(e.Stroke);
				}
			}
		}

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x06006C28 RID: 27688 RVA: 0x002C8358 File Offset: 0x002C7358
		// (remove) Token: 0x06006C29 RID: 27689 RVA: 0x002C8366 File Offset: 0x002C7366
		[Category("Behavior")]
		public event InkCanvasGestureEventHandler Gesture
		{
			add
			{
				base.AddHandler(InkCanvas.GestureEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.GestureEvent, value);
			}
		}

		// Token: 0x06006C2A RID: 27690 RVA: 0x002C824E File Offset: 0x002C724E
		protected virtual void OnGesture(InkCanvasGestureEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x14000122 RID: 290
		// (add) Token: 0x06006C2B RID: 27691 RVA: 0x002C8374 File Offset: 0x002C7374
		// (remove) Token: 0x06006C2C RID: 27692 RVA: 0x002C83AC File Offset: 0x002C73AC
		public event InkCanvasStrokesReplacedEventHandler StrokesReplaced;

		// Token: 0x06006C2D RID: 27693 RVA: 0x002C83E1 File Offset: 0x002C73E1
		protected virtual void OnStrokesReplaced(InkCanvasStrokesReplacedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.StrokesReplaced != null)
			{
				this.StrokesReplaced(this, e);
			}
		}

		// Token: 0x14000123 RID: 291
		// (add) Token: 0x06006C2E RID: 27694 RVA: 0x002C8408 File Offset: 0x002C7408
		// (remove) Token: 0x06006C2F RID: 27695 RVA: 0x002C8440 File Offset: 0x002C7440
		public event DrawingAttributesReplacedEventHandler DefaultDrawingAttributesReplaced;

		// Token: 0x06006C30 RID: 27696 RVA: 0x002C8475 File Offset: 0x002C7475
		protected virtual void OnDefaultDrawingAttributesReplaced(DrawingAttributesReplacedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.DefaultDrawingAttributesReplaced != null)
			{
				this.DefaultDrawingAttributesReplaced(this, e);
			}
		}

		// Token: 0x06006C31 RID: 27697 RVA: 0x002C849A File Offset: 0x002C749A
		private void RaiseDefaultDrawingAttributeReplaced(DrawingAttributesReplacedEventArgs e)
		{
			this.OnDefaultDrawingAttributesReplaced(e);
			this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
		}

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x06006C32 RID: 27698 RVA: 0x002C84B9 File Offset: 0x002C74B9
		// (remove) Token: 0x06006C33 RID: 27699 RVA: 0x002C84C7 File Offset: 0x002C74C7
		[Category("Behavior")]
		public event RoutedEventHandler ActiveEditingModeChanged
		{
			add
			{
				base.AddHandler(InkCanvas.ActiveEditingModeChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.ActiveEditingModeChangedEvent, value);
			}
		}

		// Token: 0x06006C34 RID: 27700 RVA: 0x002C824E File Offset: 0x002C724E
		protected virtual void OnActiveEditingModeChanged(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06006C35 RID: 27701 RVA: 0x002C84D5 File Offset: 0x002C74D5
		internal void RaiseActiveEditingModeChanged(RoutedEventArgs e)
		{
			if (this.ActiveEditingMode != this._editingCoordinator.ActiveEditingMode)
			{
				base.SetValue(InkCanvas.ActiveEditingModePropertyKey, this._editingCoordinator.ActiveEditingMode);
				this.OnActiveEditingModeChanged(e);
			}
		}

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x06006C36 RID: 27702 RVA: 0x002C850C File Offset: 0x002C750C
		// (remove) Token: 0x06006C37 RID: 27703 RVA: 0x002C851A File Offset: 0x002C751A
		[Category("Behavior")]
		public event RoutedEventHandler EditingModeChanged
		{
			add
			{
				base.AddHandler(InkCanvas.EditingModeChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.EditingModeChangedEvent, value);
			}
		}

		// Token: 0x06006C38 RID: 27704 RVA: 0x002C824E File Offset: 0x002C724E
		protected virtual void OnEditingModeChanged(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06006C39 RID: 27705 RVA: 0x002C8528 File Offset: 0x002C7528
		private void RaiseEditingModeChanged(RoutedEventArgs e)
		{
			this._editingCoordinator.UpdateEditingState(false);
			this.OnEditingModeChanged(e);
		}

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x06006C3A RID: 27706 RVA: 0x002C853D File Offset: 0x002C753D
		// (remove) Token: 0x06006C3B RID: 27707 RVA: 0x002C854B File Offset: 0x002C754B
		[Category("Behavior")]
		public event RoutedEventHandler EditingModeInvertedChanged
		{
			add
			{
				base.AddHandler(InkCanvas.EditingModeInvertedChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.EditingModeInvertedChangedEvent, value);
			}
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x002C824E File Offset: 0x002C724E
		protected virtual void OnEditingModeInvertedChanged(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x002C8559 File Offset: 0x002C7559
		private void RaiseEditingModeInvertedChanged(RoutedEventArgs e)
		{
			this._editingCoordinator.UpdateEditingState(true);
			this.OnEditingModeInvertedChanged(e);
		}

		// Token: 0x14000127 RID: 295
		// (add) Token: 0x06006C3E RID: 27710 RVA: 0x002C8570 File Offset: 0x002C7570
		// (remove) Token: 0x06006C3F RID: 27711 RVA: 0x002C85A8 File Offset: 0x002C75A8
		public event InkCanvasSelectionEditingEventHandler SelectionMoving;

		// Token: 0x06006C40 RID: 27712 RVA: 0x002C85DD File Offset: 0x002C75DD
		protected virtual void OnSelectionMoving(InkCanvasSelectionEditingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionMoving != null)
			{
				this.SelectionMoving(this, e);
			}
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x002C8602 File Offset: 0x002C7602
		internal void RaiseSelectionMoving(InkCanvasSelectionEditingEventArgs e)
		{
			this.OnSelectionMoving(e);
		}

		// Token: 0x14000128 RID: 296
		// (add) Token: 0x06006C42 RID: 27714 RVA: 0x002C860C File Offset: 0x002C760C
		// (remove) Token: 0x06006C43 RID: 27715 RVA: 0x002C8644 File Offset: 0x002C7644
		public event EventHandler SelectionMoved;

		// Token: 0x06006C44 RID: 27716 RVA: 0x002C8679 File Offset: 0x002C7679
		protected virtual void OnSelectionMoved(EventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionMoved != null)
			{
				this.SelectionMoved(this, e);
			}
		}

		// Token: 0x06006C45 RID: 27717 RVA: 0x002C869E File Offset: 0x002C769E
		internal void RaiseSelectionMoved(EventArgs e)
		{
			this.OnSelectionMoved(e);
			this.EditingCoordinator.SelectionEditor.OnInkCanvasSelectionChanged();
		}

		// Token: 0x14000129 RID: 297
		// (add) Token: 0x06006C46 RID: 27718 RVA: 0x002C86B8 File Offset: 0x002C76B8
		// (remove) Token: 0x06006C47 RID: 27719 RVA: 0x002C86F0 File Offset: 0x002C76F0
		public event InkCanvasStrokeErasingEventHandler StrokeErasing;

		// Token: 0x06006C48 RID: 27720 RVA: 0x002C8725 File Offset: 0x002C7725
		protected virtual void OnStrokeErasing(InkCanvasStrokeErasingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.StrokeErasing != null)
			{
				this.StrokeErasing(this, e);
			}
		}

		// Token: 0x06006C49 RID: 27721 RVA: 0x002C874A File Offset: 0x002C774A
		internal void RaiseStrokeErasing(InkCanvasStrokeErasingEventArgs e)
		{
			this.OnStrokeErasing(e);
		}

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x06006C4A RID: 27722 RVA: 0x002C8753 File Offset: 0x002C7753
		// (remove) Token: 0x06006C4B RID: 27723 RVA: 0x002C8761 File Offset: 0x002C7761
		[Category("Behavior")]
		public event RoutedEventHandler StrokeErased
		{
			add
			{
				base.AddHandler(InkCanvas.StrokeErasedEvent, value);
			}
			remove
			{
				base.RemoveHandler(InkCanvas.StrokeErasedEvent, value);
			}
		}

		// Token: 0x06006C4C RID: 27724 RVA: 0x002C824E File Offset: 0x002C724E
		protected virtual void OnStrokeErased(RoutedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			base.RaiseEvent(e);
		}

		// Token: 0x06006C4D RID: 27725 RVA: 0x002C876F File Offset: 0x002C776F
		internal void RaiseInkErased()
		{
			this.OnStrokeErased(new RoutedEventArgs(InkCanvas.StrokeErasedEvent, this));
		}

		// Token: 0x1400012B RID: 299
		// (add) Token: 0x06006C4E RID: 27726 RVA: 0x002C8784 File Offset: 0x002C7784
		// (remove) Token: 0x06006C4F RID: 27727 RVA: 0x002C87BC File Offset: 0x002C77BC
		public event InkCanvasSelectionEditingEventHandler SelectionResizing;

		// Token: 0x06006C50 RID: 27728 RVA: 0x002C87F1 File Offset: 0x002C77F1
		protected virtual void OnSelectionResizing(InkCanvasSelectionEditingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionResizing != null)
			{
				this.SelectionResizing(this, e);
			}
		}

		// Token: 0x06006C51 RID: 27729 RVA: 0x002C8816 File Offset: 0x002C7816
		internal void RaiseSelectionResizing(InkCanvasSelectionEditingEventArgs e)
		{
			this.OnSelectionResizing(e);
		}

		// Token: 0x1400012C RID: 300
		// (add) Token: 0x06006C52 RID: 27730 RVA: 0x002C8820 File Offset: 0x002C7820
		// (remove) Token: 0x06006C53 RID: 27731 RVA: 0x002C8858 File Offset: 0x002C7858
		public event EventHandler SelectionResized;

		// Token: 0x06006C54 RID: 27732 RVA: 0x002C888D File Offset: 0x002C788D
		protected virtual void OnSelectionResized(EventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionResized != null)
			{
				this.SelectionResized(this, e);
			}
		}

		// Token: 0x06006C55 RID: 27733 RVA: 0x002C88B2 File Offset: 0x002C78B2
		internal void RaiseSelectionResized(EventArgs e)
		{
			this.OnSelectionResized(e);
			this.EditingCoordinator.SelectionEditor.OnInkCanvasSelectionChanged();
		}

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x06006C56 RID: 27734 RVA: 0x002C88CC File Offset: 0x002C78CC
		// (remove) Token: 0x06006C57 RID: 27735 RVA: 0x002C8904 File Offset: 0x002C7904
		public event InkCanvasSelectionChangingEventHandler SelectionChanging;

		// Token: 0x06006C58 RID: 27736 RVA: 0x002C8939 File Offset: 0x002C7939
		protected virtual void OnSelectionChanging(InkCanvasSelectionChangingEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionChanging != null)
			{
				this.SelectionChanging(this, e);
			}
		}

		// Token: 0x06006C59 RID: 27737 RVA: 0x002C895E File Offset: 0x002C795E
		private void RaiseSelectionChanging(InkCanvasSelectionChangingEventArgs e)
		{
			this.OnSelectionChanging(e);
		}

		// Token: 0x1400012E RID: 302
		// (add) Token: 0x06006C5A RID: 27738 RVA: 0x002C8968 File Offset: 0x002C7968
		// (remove) Token: 0x06006C5B RID: 27739 RVA: 0x002C89A0 File Offset: 0x002C79A0
		public event EventHandler SelectionChanged;

		// Token: 0x06006C5C RID: 27740 RVA: 0x002C89D5 File Offset: 0x002C79D5
		protected virtual void OnSelectionChanged(EventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(this, e);
			}
		}

		// Token: 0x06006C5D RID: 27741 RVA: 0x002C89FA File Offset: 0x002C79FA
		internal void RaiseSelectionChanged(EventArgs e)
		{
			this.OnSelectionChanged(e);
			this.EditingCoordinator.SelectionEditor.OnInkCanvasSelectionChanged();
		}

		// Token: 0x06006C5E RID: 27742 RVA: 0x002C8A13 File Offset: 0x002C7A13
		internal void RaiseOnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			this.OnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		// Token: 0x06006C5F RID: 27743 RVA: 0x002C8A1D File Offset: 0x002C7A1D
		public ReadOnlyCollection<ApplicationGesture> GetEnabledGestures()
		{
			return new ReadOnlyCollection<ApplicationGesture>(this.GestureRecognizer.GetEnabledGestures());
		}

		// Token: 0x06006C60 RID: 27744 RVA: 0x002C8A2F File Offset: 0x002C7A2F
		public void SetEnabledGestures(IEnumerable<ApplicationGesture> applicationGestures)
		{
			this.GestureRecognizer.SetEnabledGestures(applicationGestures);
		}

		// Token: 0x06006C61 RID: 27745 RVA: 0x002C8A3D File Offset: 0x002C7A3D
		public Rect GetSelectionBounds()
		{
			base.VerifyAccess();
			return this.InkCanvasSelection.SelectionBounds;
		}

		// Token: 0x06006C62 RID: 27746 RVA: 0x002C8A50 File Offset: 0x002C7A50
		public ReadOnlyCollection<UIElement> GetSelectedElements()
		{
			base.VerifyAccess();
			return this.InkCanvasSelection.SelectedElements;
		}

		// Token: 0x06006C63 RID: 27747 RVA: 0x002C8A63 File Offset: 0x002C7A63
		public StrokeCollection GetSelectedStrokes()
		{
			base.VerifyAccess();
			return new StrokeCollection
			{
				this.InkCanvasSelection.SelectedStrokes
			};
		}

		// Token: 0x06006C64 RID: 27748 RVA: 0x002C8A81 File Offset: 0x002C7A81
		public void Select(StrokeCollection selectedStrokes)
		{
			this.Select(selectedStrokes, null);
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x002C8A8B File Offset: 0x002C7A8B
		public void Select(IEnumerable<UIElement> selectedElements)
		{
			this.Select(null, selectedElements);
		}

		// Token: 0x06006C66 RID: 27750 RVA: 0x002C8A98 File Offset: 0x002C7A98
		public void Select(StrokeCollection selectedStrokes, IEnumerable<UIElement> selectedElements)
		{
			base.VerifyAccess();
			if (this.EnsureActiveEditingMode(InkCanvasEditingMode.Select))
			{
				UIElement[] elements = this.ValidateSelectedElements(selectedElements);
				StrokeCollection strokes = this.ValidateSelectedStrokes(selectedStrokes);
				this.ChangeInkCanvasSelection(strokes, elements);
			}
		}

		// Token: 0x06006C67 RID: 27751 RVA: 0x002C8ACC File Offset: 0x002C7ACC
		public InkCanvasSelectionHitResult HitTestSelection(Point point)
		{
			base.VerifyAccess();
			if (this._localAdornerDecorator == null)
			{
				base.ApplyTemplate();
			}
			return this.InkCanvasSelection.HitTestSelection(point);
		}

		// Token: 0x06006C68 RID: 27752 RVA: 0x002C8AEF File Offset: 0x002C7AEF
		public void CopySelection()
		{
			base.VerifyAccess();
			this.PrivateCopySelection();
		}

		// Token: 0x06006C69 RID: 27753 RVA: 0x002C8B00 File Offset: 0x002C7B00
		public void CutSelection()
		{
			base.VerifyAccess();
			InkCanvasClipboardDataFormats inkCanvasClipboardDataFormats = this.PrivateCopySelection();
			if (inkCanvasClipboardDataFormats != InkCanvasClipboardDataFormats.None)
			{
				this.DeleteCurrentSelection((inkCanvasClipboardDataFormats & (InkCanvasClipboardDataFormats.XAML | InkCanvasClipboardDataFormats.ISF)) > InkCanvasClipboardDataFormats.None, (inkCanvasClipboardDataFormats & InkCanvasClipboardDataFormats.XAML) > InkCanvasClipboardDataFormats.None);
			}
		}

		// Token: 0x06006C6A RID: 27754 RVA: 0x002C8B2F File Offset: 0x002C7B2F
		public void Paste()
		{
			this.Paste(new Point(0.0, 0.0));
		}

		// Token: 0x06006C6B RID: 27755 RVA: 0x002C8B50 File Offset: 0x002C7B50
		public void Paste(Point point)
		{
			base.VerifyAccess();
			if (DoubleUtil.IsNaN(point.X) || DoubleUtil.IsNaN(point.Y) || double.IsInfinity(point.X) || double.IsInfinity(point.Y))
			{
				throw new ArgumentException(SR.Get("InvalidPoint"), "point");
			}
			if (!this._editingCoordinator.UserIsEditing)
			{
				IDataObject dataObject = null;
				try
				{
					dataObject = Clipboard.GetDataObject();
				}
				catch (ExternalException)
				{
					return;
				}
				if (dataObject != null)
				{
					this.PasteFromDataObject(dataObject, point);
				}
			}
		}

		// Token: 0x06006C6C RID: 27756 RVA: 0x002C8BE8 File Offset: 0x002C7BE8
		public bool CanPaste()
		{
			base.VerifyAccess();
			return !this._editingCoordinator.UserIsEditing && this.PrivateCanPaste();
		}

		// Token: 0x06006C6D RID: 27757 RVA: 0x002C8C05 File Offset: 0x002C7C05
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			((IAddChild)this.InnerCanvas).AddChild(value);
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x002C8C21 File Offset: 0x002C7C21
		void IAddChild.AddText(string textData)
		{
			((IAddChild)this.InnerCanvas).AddText(textData);
		}

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x06006C6F RID: 27759 RVA: 0x002C8C2F File Offset: 0x002C7C2F
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return this.InnerCanvas.PrivateLogicalChildren;
			}
		}

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x06006C70 RID: 27760 RVA: 0x002C8C3C File Offset: 0x002C7C3C
		// (set) Token: 0x06006C71 RID: 27761 RVA: 0x002C8C4C File Offset: 0x002C7C4C
		protected DynamicRenderer DynamicRenderer
		{
			get
			{
				base.VerifyAccess();
				return this.InternalDynamicRenderer;
			}
			set
			{
				base.VerifyAccess();
				if (value != this._dynamicRenderer)
				{
					int num = -1;
					if (this._dynamicRenderer != null)
					{
						num = base.StylusPlugIns.IndexOf(this._dynamicRenderer);
						if (-1 != num)
						{
							base.StylusPlugIns.RemoveAt(num);
						}
						if (this.InkPresenter.ContainsAttachedVisual(this._dynamicRenderer.RootVisual))
						{
							this.InkPresenter.DetachVisuals(this._dynamicRenderer.RootVisual);
						}
					}
					this._dynamicRenderer = value;
					if (this._dynamicRenderer != null)
					{
						if (!base.StylusPlugIns.Contains(this._dynamicRenderer))
						{
							if (-1 != num)
							{
								base.StylusPlugIns.Insert(num, this._dynamicRenderer);
							}
							else
							{
								base.StylusPlugIns.Add(this._dynamicRenderer);
							}
						}
						this._dynamicRenderer.DrawingAttributes = this.DefaultDrawingAttributes;
						if (!this.InkPresenter.ContainsAttachedVisual(this._dynamicRenderer.RootVisual) && this._dynamicRenderer.Enabled && this._dynamicRenderer.RootVisual != null)
						{
							this.InkPresenter.AttachVisuals(this._dynamicRenderer.RootVisual, this.DefaultDrawingAttributes);
						}
					}
				}
			}
		}

		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x06006C72 RID: 27762 RVA: 0x002C8D74 File Offset: 0x002C7D74
		protected InkPresenter InkPresenter
		{
			get
			{
				base.VerifyAccess();
				if (this._inkPresenter == null)
				{
					this._inkPresenter = new InkPresenter();
					Binding binding = new Binding();
					binding.Path = new PropertyPath(InkCanvas.StrokesProperty);
					binding.Mode = BindingMode.OneWay;
					binding.Source = this;
					this._inkPresenter.SetBinding(InkPresenter.StrokesProperty, binding);
				}
				return this._inkPresenter;
			}
		}

		// Token: 0x06006C73 RID: 27763 RVA: 0x002C8DD6 File Offset: 0x002C7DD6
		private bool UserInitiatedCanPaste()
		{
			return this.PrivateCanPaste();
		}

		// Token: 0x06006C74 RID: 27764 RVA: 0x002C8DE0 File Offset: 0x002C7DE0
		private bool PrivateCanPaste()
		{
			bool result = false;
			IDataObject dataObject = null;
			try
			{
				dataObject = Clipboard.GetDataObject();
			}
			catch (ExternalException)
			{
				return false;
			}
			if (dataObject != null)
			{
				result = this.ClipboardProcessor.CheckDataFormats(dataObject);
			}
			return result;
		}

		// Token: 0x06006C75 RID: 27765 RVA: 0x002C8E24 File Offset: 0x002C7E24
		internal void PasteFromDataObject(IDataObject dataObj, Point point)
		{
			this.ClearSelection(false);
			StrokeCollection strokeCollection = new StrokeCollection();
			List<UIElement> list = new List<UIElement>();
			if (!this.ClipboardProcessor.PasteData(dataObj, ref strokeCollection, ref list))
			{
				return;
			}
			if (strokeCollection.Count == 0 && list.Count == 0)
			{
				return;
			}
			UIElementCollection children = this.Children;
			foreach (UIElement element in list)
			{
				children.Add(element);
			}
			if (strokeCollection != null)
			{
				this.Strokes.Add(strokeCollection);
			}
			try
			{
				this.CoreChangeSelection(strokeCollection, list.ToArray(), this.EditingMode == InkCanvasEditingMode.Select);
			}
			finally
			{
				Rect selectionBounds = this.GetSelectionBounds();
				this.InkCanvasSelection.CommitChanges(Rect.Offset(selectionBounds, -selectionBounds.Left + point.X, -selectionBounds.Top + point.Y), false);
				if (this.EditingMode != InkCanvasEditingMode.Select)
				{
					this.ClearSelection(false);
				}
			}
		}

		// Token: 0x06006C76 RID: 27766 RVA: 0x002C8F34 File Offset: 0x002C7F34
		private InkCanvasClipboardDataFormats CopyToDataObject()
		{
			DataObject dataObject = new DataObject();
			InkCanvasClipboardDataFormats inkCanvasClipboardDataFormats = this.ClipboardProcessor.CopySelectedData(dataObject);
			if (inkCanvasClipboardDataFormats != InkCanvasClipboardDataFormats.None)
			{
				Clipboard.SetDataObject(dataObject, true);
			}
			return inkCanvasClipboardDataFormats;
		}

		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x06006C77 RID: 27767 RVA: 0x002C8F5D File Offset: 0x002C7F5D
		internal EditingCoordinator EditingCoordinator
		{
			get
			{
				return this._editingCoordinator;
			}
		}

		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x06006C78 RID: 27768 RVA: 0x002C8F65 File Offset: 0x002C7F65
		internal DynamicRenderer InternalDynamicRenderer
		{
			get
			{
				return this._dynamicRenderer;
			}
		}

		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x06006C79 RID: 27769 RVA: 0x002C8F70 File Offset: 0x002C7F70
		internal InkCanvasInnerCanvas InnerCanvas
		{
			get
			{
				if (this._innerCanvas == null)
				{
					this._innerCanvas = new InkCanvasInnerCanvas(this);
					Binding binding = new Binding();
					binding.Path = new PropertyPath(InkCanvas.BackgroundProperty);
					binding.Mode = BindingMode.OneWay;
					binding.Source = this;
					this._innerCanvas.SetBinding(Panel.BackgroundProperty, binding);
				}
				return this._innerCanvas;
			}
		}

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x06006C7A RID: 27770 RVA: 0x002C8FCD File Offset: 0x002C7FCD
		internal InkCanvasSelection InkCanvasSelection
		{
			get
			{
				if (this._selection == null)
				{
					this._selection = new InkCanvasSelection(this);
				}
				return this._selection;
			}
		}

		// Token: 0x06006C7B RID: 27771 RVA: 0x002C8FE9 File Offset: 0x002C7FE9
		internal void BeginDynamicSelection(Visual visual)
		{
			this._dynamicallySelectedStrokes = new StrokeCollection();
			this.InkPresenter.AttachVisuals(visual, new DrawingAttributes());
		}

		// Token: 0x06006C7C RID: 27772 RVA: 0x002C9008 File Offset: 0x002C8008
		internal void UpdateDynamicSelection(StrokeCollection strokesToDynamicallySelect, StrokeCollection strokesToDynamicallyUnselect)
		{
			if (strokesToDynamicallySelect != null)
			{
				foreach (Stroke stroke in strokesToDynamicallySelect)
				{
					this._dynamicallySelectedStrokes.Add(stroke);
					stroke.IsSelected = true;
				}
			}
			if (strokesToDynamicallyUnselect != null)
			{
				foreach (Stroke stroke2 in strokesToDynamicallyUnselect)
				{
					this._dynamicallySelectedStrokes.Remove(stroke2);
					stroke2.IsSelected = false;
				}
			}
		}

		// Token: 0x06006C7D RID: 27773 RVA: 0x002C90A8 File Offset: 0x002C80A8
		internal StrokeCollection EndDynamicSelection(Visual visual)
		{
			this.InkPresenter.DetachVisuals(visual);
			StrokeCollection dynamicallySelectedStrokes = this._dynamicallySelectedStrokes;
			this._dynamicallySelectedStrokes = null;
			return dynamicallySelectedStrokes;
		}

		// Token: 0x06006C7E RID: 27774 RVA: 0x002C90C3 File Offset: 0x002C80C3
		internal bool ClearSelectionRaiseSelectionChanging()
		{
			if (!this.InkCanvasSelection.HasSelection)
			{
				return true;
			}
			this.ChangeInkCanvasSelection(new StrokeCollection(), new UIElement[0]);
			return !this.InkCanvasSelection.HasSelection;
		}

		// Token: 0x06006C7F RID: 27775 RVA: 0x002C90F3 File Offset: 0x002C80F3
		internal void ClearSelection(bool raiseSelectionChangedEvent)
		{
			if (this.InkCanvasSelection.HasSelection)
			{
				this.CoreChangeSelection(new StrokeCollection(), new UIElement[0], raiseSelectionChangedEvent);
			}
		}

		// Token: 0x06006C80 RID: 27776 RVA: 0x002C9114 File Offset: 0x002C8114
		internal void ChangeInkCanvasSelection(StrokeCollection strokes, UIElement[] elements)
		{
			bool flag;
			bool flag2;
			this.InkCanvasSelection.SelectionIsDifferentThanCurrent(strokes, out flag, elements, out flag2);
			if (flag || flag2)
			{
				InkCanvasSelectionChangingEventArgs inkCanvasSelectionChangingEventArgs = new InkCanvasSelectionChangingEventArgs(strokes, elements);
				StrokeCollection strokeCollection = strokes;
				UIElement[] validElements = elements;
				this.RaiseSelectionChanging(inkCanvasSelectionChangingEventArgs);
				if (!inkCanvasSelectionChangingEventArgs.Cancel)
				{
					if (inkCanvasSelectionChangingEventArgs.StrokesChanged)
					{
						strokeCollection = this.ValidateSelectedStrokes(inkCanvasSelectionChangingEventArgs.GetSelectedStrokes());
						int count = strokes.Count;
						for (int i = 0; i < count; i++)
						{
							if (!strokeCollection.Contains(strokes[i]))
							{
								strokes[i].IsSelected = false;
							}
						}
					}
					if (inkCanvasSelectionChangingEventArgs.ElementsChanged)
					{
						validElements = this.ValidateSelectedElements(inkCanvasSelectionChangingEventArgs.GetSelectedElements());
					}
					this.CoreChangeSelection(strokeCollection, validElements, true);
					return;
				}
				StrokeCollection selectedStrokes = this.InkCanvasSelection.SelectedStrokes;
				int count2 = strokes.Count;
				for (int j = 0; j < count2; j++)
				{
					if (!selectedStrokes.Contains(strokes[j]))
					{
						strokes[j].IsSelected = false;
					}
				}
			}
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x002C9208 File Offset: 0x002C8208
		private void CoreChangeSelection(StrokeCollection validStrokes, IList<UIElement> validElements, bool raiseSelectionChanged)
		{
			this.InkCanvasSelection.Select(validStrokes, validElements, raiseSelectionChanged);
		}

		// Token: 0x06006C82 RID: 27778 RVA: 0x002C9218 File Offset: 0x002C8218
		internal static StrokeCollection GetValidStrokes(StrokeCollection subset, StrokeCollection superset)
		{
			StrokeCollection strokeCollection = new StrokeCollection();
			int count = subset.Count;
			if (count == 0)
			{
				return strokeCollection;
			}
			for (int i = 0; i < count; i++)
			{
				Stroke item = subset[i];
				if (superset.Contains(item))
				{
					strokeCollection.Add(item);
				}
			}
			return strokeCollection;
		}

		// Token: 0x06006C83 RID: 27779 RVA: 0x002C925C File Offset: 0x002C825C
		private static void _RegisterClipboardHandlers()
		{
			Type typeFromHandle = typeof(InkCanvas);
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Cut, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled), KeyGesture.CreateFromResourceStrings("Shift+Delete", "KeyShiftDeleteDisplayString"));
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Copy, new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted), new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled), KeyGesture.CreateFromResourceStrings("Ctrl+Insert", "KeyCtrlInsertDisplayString"));
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(InkCanvas._OnCommandExecuted);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(InkCanvas._OnQueryCommandEnabled);
			InputGesture inputGesture = KeyGesture.CreateFromResourceStrings("Shift+Insert", SR.Get("KeyShiftInsertDisplayString"));
			CommandHelpers.RegisterCommandHandler(typeFromHandle, ApplicationCommands.Paste, executedRoutedEventHandler, canExecuteRoutedEventHandler, inputGesture);
		}

		// Token: 0x06006C84 RID: 27780 RVA: 0x002C9313 File Offset: 0x002C8313
		private StrokeCollection ValidateSelectedStrokes(StrokeCollection strokes)
		{
			if (strokes == null)
			{
				return new StrokeCollection();
			}
			return InkCanvas.GetValidStrokes(strokes, this.Strokes);
		}

		// Token: 0x06006C85 RID: 27781 RVA: 0x002C932C File Offset: 0x002C832C
		private UIElement[] ValidateSelectedElements(IEnumerable<UIElement> selectedElements)
		{
			if (selectedElements == null)
			{
				return new UIElement[0];
			}
			List<UIElement> list = new List<UIElement>();
			foreach (UIElement uielement in selectedElements)
			{
				if (!list.Contains(uielement) && this.InkCanvasIsAncestorOf(uielement))
				{
					list.Add(uielement);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06006C86 RID: 27782 RVA: 0x002C939C File Offset: 0x002C839C
		private bool InkCanvasIsAncestorOf(UIElement element)
		{
			return this != element && base.IsAncestorOf(element);
		}

		// Token: 0x06006C87 RID: 27783 RVA: 0x002C93AE File Offset: 0x002C83AE
		private void DefaultDrawingAttributes_Changed(object sender, PropertyDataChangedEventArgs args)
		{
			base.InvalidateSubProperty(InkCanvas.DefaultDrawingAttributesProperty);
			this.UpdateDynamicRenderer();
			this._editingCoordinator.InvalidateBehaviorCursor(this._editingCoordinator.InkCollectionBehavior);
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x002C93D7 File Offset: 0x002C83D7
		internal void UpdateDynamicRenderer()
		{
			this.UpdateDynamicRenderer(this.DefaultDrawingAttributes);
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x002C93E8 File Offset: 0x002C83E8
		private void UpdateDynamicRenderer(DrawingAttributes newDrawingAttributes)
		{
			base.ApplyTemplate();
			if (this.DynamicRenderer != null)
			{
				this.DynamicRenderer.DrawingAttributes = newDrawingAttributes;
				if (!this.InkPresenter.AttachedVisualIsPositionedCorrectly(this.DynamicRenderer.RootVisual, newDrawingAttributes))
				{
					if (this.InkPresenter.ContainsAttachedVisual(this.DynamicRenderer.RootVisual))
					{
						this.InkPresenter.DetachVisuals(this.DynamicRenderer.RootVisual);
					}
					if (this.DynamicRenderer.Enabled && this.DynamicRenderer.RootVisual != null)
					{
						this.InkPresenter.AttachVisuals(this.DynamicRenderer.RootVisual, newDrawingAttributes);
					}
				}
			}
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x002C948C File Offset: 0x002C848C
		private bool EnsureActiveEditingMode(InkCanvasEditingMode newEditingMode)
		{
			bool result = true;
			if (this.ActiveEditingMode != newEditingMode)
			{
				if (this.EditingCoordinator.IsStylusInverted)
				{
					this.EditingModeInverted = newEditingMode;
				}
				else
				{
					this.EditingMode = newEditingMode;
				}
				result = (this.ActiveEditingMode == newEditingMode);
			}
			return result;
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x06006C8B RID: 27787 RVA: 0x002C94CC File Offset: 0x002C84CC
		private ClipboardProcessor ClipboardProcessor
		{
			get
			{
				if (this._clipboardProcessor == null)
				{
					this._clipboardProcessor = new ClipboardProcessor(this);
				}
				return this._clipboardProcessor;
			}
		}

		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x06006C8C RID: 27788 RVA: 0x002C94E8 File Offset: 0x002C84E8
		private GestureRecognizer GestureRecognizer
		{
			get
			{
				if (this._gestureRecognizer == null)
				{
					this._gestureRecognizer = new GestureRecognizer();
				}
				return this._gestureRecognizer;
			}
		}

		// Token: 0x06006C8D RID: 27789 RVA: 0x002C9504 File Offset: 0x002C8504
		private void DeleteCurrentSelection(bool removeSelectedStrokes, bool removeSelectedElements)
		{
			StrokeCollection selectedStrokes = this.GetSelectedStrokes();
			IList<UIElement> selectedElements = this.GetSelectedElements();
			StrokeCollection validStrokes = removeSelectedStrokes ? new StrokeCollection() : selectedStrokes;
			IList<UIElement> validElements;
			if (!removeSelectedElements)
			{
				validElements = selectedElements;
			}
			else
			{
				IList<UIElement> list = new List<UIElement>();
				validElements = list;
			}
			this.CoreChangeSelection(validStrokes, validElements, true);
			if (removeSelectedStrokes && selectedStrokes != null && selectedStrokes.Count != 0)
			{
				this.Strokes.Remove(selectedStrokes);
			}
			if (removeSelectedElements)
			{
				UIElementCollection children = this.Children;
				foreach (UIElement element in selectedElements)
				{
					children.Remove(element);
				}
			}
		}

		// Token: 0x06006C8E RID: 27790 RVA: 0x002C95A4 File Offset: 0x002C85A4
		private static void _OnCommandExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			ICommand command = args.Command;
			InkCanvas inkCanvas = sender as InkCanvas;
			if (inkCanvas.IsEnabled && !inkCanvas.EditingCoordinator.UserIsEditing)
			{
				if (command == ApplicationCommands.Delete)
				{
					inkCanvas.DeleteCurrentSelection(true, true);
					return;
				}
				if (command == ApplicationCommands.Cut)
				{
					inkCanvas.CutSelection();
					return;
				}
				if (command == ApplicationCommands.Copy)
				{
					inkCanvas.CopySelection();
					return;
				}
				if (command == ApplicationCommands.SelectAll)
				{
					if (inkCanvas.ActiveEditingMode == InkCanvasEditingMode.Select)
					{
						IEnumerable<UIElement> selectedElements = null;
						UIElementCollection children = inkCanvas.Children;
						if (children.Count > 0)
						{
							UIElement[] array = new UIElement[children.Count];
							for (int i = 0; i < children.Count; i++)
							{
								array[i] = children[i];
							}
							selectedElements = array;
						}
						inkCanvas.Select(inkCanvas.Strokes, selectedElements);
						return;
					}
				}
				else
				{
					if (command == ApplicationCommands.Paste)
					{
						try
						{
							inkCanvas.Paste();
							return;
						}
						catch (COMException)
						{
							return;
						}
						catch (XamlParseException)
						{
							return;
						}
						catch (ArgumentException)
						{
							return;
						}
					}
					if (command == InkCanvas.DeselectCommand)
					{
						inkCanvas.ClearSelectionRaiseSelectionChanging();
					}
				}
			}
		}

		// Token: 0x06006C8F RID: 27791 RVA: 0x002C96BC File Offset: 0x002C86BC
		private static void _OnQueryCommandEnabled(object sender, CanExecuteRoutedEventArgs args)
		{
			RoutedCommand routedCommand = (RoutedCommand)args.Command;
			InkCanvas inkCanvas = sender as InkCanvas;
			if (inkCanvas.IsEnabled && !inkCanvas.EditingCoordinator.UserIsEditing)
			{
				if (routedCommand == ApplicationCommands.Delete || routedCommand == ApplicationCommands.Cut || routedCommand == ApplicationCommands.Copy || routedCommand == InkCanvas.DeselectCommand)
				{
					args.CanExecute = inkCanvas.InkCanvasSelection.HasSelection;
				}
				else
				{
					if (routedCommand == ApplicationCommands.Paste)
					{
						try
						{
							args.CanExecute = (args.UserInitiated ? inkCanvas.UserInitiatedCanPaste() : inkCanvas.CanPaste());
							goto IL_D3;
						}
						catch (COMException)
						{
							args.CanExecute = false;
							goto IL_D3;
						}
					}
					if (routedCommand == ApplicationCommands.SelectAll)
					{
						args.CanExecute = (inkCanvas.ActiveEditingMode == InkCanvasEditingMode.Select && (inkCanvas.Strokes.Count > 0 || inkCanvas.Children.Count > 0));
					}
				}
			}
			else
			{
				args.CanExecute = false;
			}
			IL_D3:
			if (routedCommand == ApplicationCommands.Cut || routedCommand == ApplicationCommands.Copy || routedCommand == ApplicationCommands.Paste)
			{
				args.Handled = true;
			}
		}

		// Token: 0x06006C90 RID: 27792 RVA: 0x002C97CC File Offset: 0x002C87CC
		private InkCanvasClipboardDataFormats PrivateCopySelection()
		{
			InkCanvasClipboardDataFormats result = InkCanvasClipboardDataFormats.None;
			if (this.InkCanvasSelection.HasSelection && !this._editingCoordinator.UserIsEditing)
			{
				result = this.CopyToDataObject();
			}
			return result;
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x002C97FD File Offset: 0x002C87FD
		private static void _OnDeviceDown<TEventArgs>(object sender, TEventArgs e) where TEventArgs : InputEventArgs
		{
			((InkCanvas)sender).EditingCoordinator.OnInkCanvasDeviceDown(sender, e);
		}

		// Token: 0x06006C92 RID: 27794 RVA: 0x002C9816 File Offset: 0x002C8816
		private static void _OnDeviceUp<TEventArgs>(object sender, TEventArgs e) where TEventArgs : InputEventArgs
		{
			((InkCanvas)sender).EditingCoordinator.OnInkCanvasDeviceUp(sender, e);
		}

		// Token: 0x06006C93 RID: 27795 RVA: 0x002C9830 File Offset: 0x002C8830
		private static void _OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			InkCanvas inkCanvas = (InkCanvas)sender;
			if (inkCanvas.UseCustomCursor)
			{
				return;
			}
			if (!e.Handled || inkCanvas.ForceCursor)
			{
				Cursor activeBehaviorCursor = inkCanvas.EditingCoordinator.GetActiveBehaviorCursor();
				if (activeBehaviorCursor != null)
				{
					e.Cursor = activeBehaviorCursor;
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006C94 RID: 27796 RVA: 0x002C987A File Offset: 0x002C887A
		internal void UpdateCursor()
		{
			if (base.IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x040035CD RID: 13773
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(InkCanvas), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x040035CE RID: 13774
		public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		// Token: 0x040035CF RID: 13775
		public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		// Token: 0x040035D0 RID: 13776
		public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		// Token: 0x040035D1 RID: 13777
		public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right", typeof(double), typeof(InkCanvas), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(InkCanvas.OnPositioningChanged)), new ValidateValueCallback(Shape.IsDoubleFiniteOrNaN));

		// Token: 0x040035D2 RID: 13778
		public static readonly DependencyProperty StrokesProperty = InkPresenter.StrokesProperty.AddOwner(typeof(InkCanvas), new FrameworkPropertyMetadata(new StrokeCollectionDefaultValueFactory(), new PropertyChangedCallback(InkCanvas.OnStrokesChanged)));

		// Token: 0x040035D3 RID: 13779
		public static readonly DependencyProperty DefaultDrawingAttributesProperty = DependencyProperty.Register("DefaultDrawingAttributes", typeof(DrawingAttributes), typeof(InkCanvas), new FrameworkPropertyMetadata(new DrawingAttributesDefaultValueFactory(), new PropertyChangedCallback(InkCanvas.OnDefaultDrawingAttributesChanged)), (object value) => value != null);

		// Token: 0x040035D4 RID: 13780
		internal static readonly DependencyPropertyKey ActiveEditingModePropertyKey = DependencyProperty.RegisterReadOnly("ActiveEditingMode", typeof(InkCanvasEditingMode), typeof(InkCanvas), new FrameworkPropertyMetadata(InkCanvasEditingMode.Ink));

		// Token: 0x040035D5 RID: 13781
		public static readonly DependencyProperty ActiveEditingModeProperty = InkCanvas.ActiveEditingModePropertyKey.DependencyProperty;

		// Token: 0x040035D6 RID: 13782
		public static readonly DependencyProperty EditingModeProperty = DependencyProperty.Register("EditingMode", typeof(InkCanvasEditingMode), typeof(InkCanvas), new FrameworkPropertyMetadata(InkCanvasEditingMode.Ink, new PropertyChangedCallback(InkCanvas.OnEditingModeChanged)), new ValidateValueCallback(InkCanvas.ValidateEditingMode));

		// Token: 0x040035D7 RID: 13783
		public static readonly DependencyProperty EditingModeInvertedProperty = DependencyProperty.Register("EditingModeInverted", typeof(InkCanvasEditingMode), typeof(InkCanvas), new FrameworkPropertyMetadata(InkCanvasEditingMode.EraseByStroke, new PropertyChangedCallback(InkCanvas.OnEditingModeInvertedChanged)), new ValidateValueCallback(InkCanvas.ValidateEditingMode));

		// Token: 0x040035E7 RID: 13799
		internal static readonly RoutedCommand DeselectCommand;

		// Token: 0x040035E8 RID: 13800
		private InkCanvasSelection _selection;

		// Token: 0x040035E9 RID: 13801
		private InkCanvasSelectionAdorner _selectionAdorner;

		// Token: 0x040035EA RID: 13802
		private InkCanvasFeedbackAdorner _feedbackAdorner;

		// Token: 0x040035EB RID: 13803
		private InkCanvasInnerCanvas _innerCanvas;

		// Token: 0x040035EC RID: 13804
		private AdornerDecorator _localAdornerDecorator;

		// Token: 0x040035ED RID: 13805
		private StrokeCollection _dynamicallySelectedStrokes;

		// Token: 0x040035EE RID: 13806
		private EditingCoordinator _editingCoordinator;

		// Token: 0x040035EF RID: 13807
		private StylusPointDescription _defaultStylusPointDescription;

		// Token: 0x040035F0 RID: 13808
		private StylusShape _eraserShape;

		// Token: 0x040035F1 RID: 13809
		private bool _useCustomCursor;

		// Token: 0x040035F2 RID: 13810
		private InkPresenter _inkPresenter;

		// Token: 0x040035F3 RID: 13811
		private DynamicRenderer _dynamicRenderer;

		// Token: 0x040035F4 RID: 13812
		private ClipboardProcessor _clipboardProcessor;

		// Token: 0x040035F5 RID: 13813
		private GestureRecognizer _gestureRecognizer;

		// Token: 0x040035F6 RID: 13814
		private InkCanvas.RTIHighContrastCallback _rtiHighContrastCallback;

		// Token: 0x040035F7 RID: 13815
		private const double c_pasteDefaultLocation = 0.0;

		// Token: 0x040035F8 RID: 13816
		private const string InkCanvasDeselectKey = "Esc";

		// Token: 0x040035F9 RID: 13817
		private const string KeyCtrlInsert = "Ctrl+Insert";

		// Token: 0x040035FA RID: 13818
		private const string KeyShiftInsert = "Shift+Insert";

		// Token: 0x040035FB RID: 13819
		private const string KeyShiftDelete = "Shift+Delete";

		// Token: 0x02000BF5 RID: 3061
		private class RTIHighContrastCallback : HighContrastCallback
		{
			// Token: 0x06008FE7 RID: 36839 RVA: 0x00345821 File Offset: 0x00344821
			internal RTIHighContrastCallback(InkCanvas inkCanvas)
			{
				this._thisInkCanvas = inkCanvas;
			}

			// Token: 0x06008FE8 RID: 36840 RVA: 0x00345830 File Offset: 0x00344830
			private RTIHighContrastCallback()
			{
			}

			// Token: 0x06008FE9 RID: 36841 RVA: 0x00345838 File Offset: 0x00344838
			internal override void TurnHighContrastOn(Color highContrastColor)
			{
				DrawingAttributes drawingAttributes = this._thisInkCanvas.DefaultDrawingAttributes.Clone();
				drawingAttributes.Color = highContrastColor;
				this._thisInkCanvas.UpdateDynamicRenderer(drawingAttributes);
			}

			// Token: 0x06008FEA RID: 36842 RVA: 0x00345869 File Offset: 0x00344869
			internal override void TurnHighContrastOff()
			{
				this._thisInkCanvas.UpdateDynamicRenderer(this._thisInkCanvas.DefaultDrawingAttributes);
			}

			// Token: 0x17001F73 RID: 8051
			// (get) Token: 0x06008FEB RID: 36843 RVA: 0x00345881 File Offset: 0x00344881
			internal override Dispatcher Dispatcher
			{
				get
				{
					return this._thisInkCanvas.Dispatcher;
				}
			}

			// Token: 0x04004A92 RID: 19090
			private InkCanvas _thisInkCanvas;
		}

		// Token: 0x02000BF6 RID: 3062
		private class ActiveEditingMode2VisibilityConverter : IValueConverter
		{
			// Token: 0x06008FEC RID: 36844 RVA: 0x0034588E File Offset: 0x0034488E
			public object Convert(object o, Type type, object parameter, CultureInfo culture)
			{
				if ((InkCanvasEditingMode)o != InkCanvasEditingMode.None)
				{
					return Visibility.Visible;
				}
				return Visibility.Collapsed;
			}

			// Token: 0x06008FED RID: 36845 RVA: 0x00109403 File Offset: 0x00108403
			public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
			{
				return null;
			}
		}
	}
}
