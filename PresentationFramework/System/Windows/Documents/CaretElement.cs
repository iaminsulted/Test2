using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal;
using MS.Internal.Documents;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020005DE RID: 1502
	internal sealed class CaretElement : Adorner
	{
		// Token: 0x0600488A RID: 18570 RVA: 0x0022C90C File Offset: 0x0022B90C
		internal CaretElement(TextEditor textEditor, bool isBlinkEnabled) : base(textEditor.TextView.RenderScope)
		{
			Invariant.Assert(textEditor.TextView != null && textEditor.TextView.RenderScope != null, "Assert: textView != null && RenderScope != null");
			this._textEditor = textEditor;
			this._isBlinkEnabled = isBlinkEnabled;
			this._left = 0.0;
			this._top = 0.0;
			this._systemCaretWidth = SystemParameters.CaretWidth;
			this._height = 0.0;
			base.AllowDrop = false;
			this._caretElement = new CaretElement.CaretSubElement();
			this._caretElement.ClipToBounds = false;
			base.AddVisualChild(this._caretElement);
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x0600488B RID: 18571 RVA: 0x0022C9BD File Offset: 0x0022B9BD
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._caretElement != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x0022C9CA File Offset: 0x0022B9CA
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._caretElement;
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x00109403 File Offset: 0x00108403
		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			return null;
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x0022C9F0 File Offset: 0x0022B9F0
		protected override void OnRender(DrawingContext drawingContext)
		{
			if (this._selectionGeometry != null)
			{
				FrameworkElement ownerElement = this.GetOwnerElement();
				Brush brush = (Brush)ownerElement.GetValue(TextBoxBase.SelectionBrushProperty);
				if (brush == null)
				{
					return;
				}
				double opacity = (double)ownerElement.GetValue(TextBoxBase.SelectionOpacityProperty);
				drawingContext.PushOpacity(opacity);
				Pen pen = null;
				drawingContext.DrawGeometry(brush, pen, this._selectionGeometry);
				drawingContext.Pop();
			}
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x0022CA50 File Offset: 0x0022BA50
		protected override Size MeasureOverride(Size availableSize)
		{
			base.MeasureOverride(availableSize);
			this._caretElement.InvalidateVisual();
			return new Size(double.IsInfinity(availableSize.Width) ? 8.988465674311579E+307 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 8.988465674311579E+307 : availableSize.Height);
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x0022CAB8 File Offset: 0x0022BAB8
		protected override Size ArrangeOverride(Size availableSize)
		{
			if (this._pendingGeometryUpdate)
			{
				((TextSelection)this._textEditor.Selection).UpdateCaretState(CaretScrollMethod.None);
				this._pendingGeometryUpdate = false;
			}
			Point location = new Point(this._left, this._top);
			this._caretElement.Arrange(new Rect(location, availableSize));
			return availableSize;
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x0022CB10 File Offset: 0x0022BB10
		internal void Update(bool visible, Rect caretRectangle, Brush caretBrush, double opacity, bool italic, CaretScrollMethod scrollMethod, double scrollToOriginPosition)
		{
			Invariant.Assert(caretBrush != null, "Assert: caretBrush != null");
			this.EnsureAttachedToView();
			bool flag = visible && !this._showCaret;
			if (this._showCaret != visible)
			{
				base.InvalidateVisual();
				this._showCaret = visible;
			}
			this._caretBrush = caretBrush;
			this._opacity = opacity;
			double num;
			double num2;
			double num3;
			double num4;
			if (caretRectangle.IsEmpty || caretRectangle.Height <= 0.0)
			{
				num = 0.0;
				num2 = 0.0;
				num3 = 0.0;
				num4 = 0.0;
			}
			else
			{
				num = caretRectangle.X;
				num2 = caretRectangle.Y;
				num3 = caretRectangle.Height;
				num4 = SystemParameters.CaretWidth;
			}
			bool flag2 = flag || italic != this._italic;
			if (!DoubleUtil.AreClose(this._left, num))
			{
				this._left = num;
				flag2 = true;
			}
			if (!DoubleUtil.AreClose(this._top, num2))
			{
				this._top = num2;
				flag2 = true;
			}
			if (!caretRectangle.IsEmpty && this._interimWidth != caretRectangle.Width)
			{
				this._interimWidth = caretRectangle.Width;
				flag2 = true;
			}
			if (!DoubleUtil.AreClose(this._systemCaretWidth, num4))
			{
				this._systemCaretWidth = num4;
				flag2 = true;
			}
			if (!DoubleUtil.AreClose(this._height, num3))
			{
				this._height = num3;
				base.InvalidateMeasure();
			}
			if (flag2 || !double.IsNaN(scrollToOriginPosition))
			{
				this._scrolledToCurrentPositionYet = false;
				this.RefreshCaret(italic);
			}
			if (scrollMethod != CaretScrollMethod.None && !this._scrolledToCurrentPositionYet)
			{
				Rect rect = new Rect(this._left - 5.0, this._top, 10.0 + (this.IsInInterimState ? this._interimWidth : this._systemCaretWidth), this._height);
				if (!double.IsNaN(scrollToOriginPosition) && scrollToOriginPosition > 0.0)
				{
					rect.X += rect.Width;
					rect.Width = 0.0;
				}
				if (scrollMethod != CaretScrollMethod.Simple)
				{
					if (scrollMethod == CaretScrollMethod.Navigation)
					{
						this.DoNavigationalScrollToView(scrollToOriginPosition, rect);
					}
				}
				else
				{
					this.DoSimpleScrollToView(scrollToOriginPosition, rect);
				}
				this._scrolledToCurrentPositionYet = true;
			}
			this.SetBlinkAnimation(visible, flag2);
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x0022CD48 File Offset: 0x0022BD48
		private void DoSimpleScrollToView(double scrollToOriginPosition, Rect scrollRectangle)
		{
			if (!double.IsNaN(scrollToOriginPosition))
			{
				TextViewBase.BringRectIntoViewMinimally(this._textEditor.TextView, new Rect(scrollToOriginPosition, scrollRectangle.Y, scrollRectangle.Width, scrollRectangle.Height));
				scrollRectangle.X -= scrollToOriginPosition;
			}
			TextViewBase.BringRectIntoViewMinimally(this._textEditor.TextView, scrollRectangle);
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x0022CDA8 File Offset: 0x0022BDA8
		private void DoNavigationalScrollToView(double scrollToOriginPosition, Rect targetRect)
		{
			ScrollViewer scrollViewer = this._textEditor._Scroller as ScrollViewer;
			if (scrollViewer != null)
			{
				Point inPoint = new Point(targetRect.Left, targetRect.Top);
				if (this._textEditor.TextView.RenderScope.TransformToAncestor(scrollViewer).TryTransform(inPoint, out inPoint))
				{
					double viewportWidth = scrollViewer.ViewportWidth;
					double viewportHeight = scrollViewer.ViewportHeight;
					if (inPoint.Y < 0.0 || inPoint.Y + targetRect.Height > viewportHeight)
					{
						if (inPoint.Y < 0.0)
						{
							double num = Math.Abs(inPoint.Y);
							scrollViewer.ScrollToVerticalOffset(Math.Max(0.0, scrollViewer.VerticalOffset - num - viewportHeight / 4.0));
						}
						else
						{
							double num = inPoint.Y + targetRect.Height - viewportHeight;
							scrollViewer.ScrollToVerticalOffset(Math.Min(scrollViewer.ExtentHeight, scrollViewer.VerticalOffset + num + viewportHeight / 4.0));
						}
					}
					if (inPoint.X < 0.0 || inPoint.X > viewportWidth)
					{
						double num;
						if (inPoint.X < 0.0)
						{
							num = Math.Abs(inPoint.X);
							scrollViewer.ScrollToHorizontalOffset(Math.Max(0.0, scrollViewer.HorizontalOffset - num - viewportWidth / 4.0));
							return;
						}
						num = inPoint.X - viewportWidth;
						scrollViewer.ScrollToHorizontalOffset(Math.Min(scrollViewer.ExtentWidth, scrollViewer.HorizontalOffset + num + viewportWidth / 4.0));
						return;
					}
				}
			}
			else if (!this._textEditor.Selection.MovingPosition.HasValidLayout && this._textEditor.TextView != null && this._textEditor.TextView.IsValid)
			{
				this.DoSimpleScrollToView(scrollToOriginPosition, targetRect);
			}
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x0022CF98 File Offset: 0x0022BF98
		internal void UpdateSelection()
		{
			Geometry selectionGeometry = this._selectionGeometry;
			this._selectionGeometry = null;
			if (!this._textEditor.Selection.IsEmpty)
			{
				this.EnsureAttachedToView();
				List<TextSegment> textSegments = this._textEditor.Selection.TextSegments;
				for (int i = 0; i < textSegments.Count; i++)
				{
					TextSegment textSegment = textSegments[i];
					Geometry tightBoundingGeometryFromTextPositions = this._textEditor.Selection.TextView.GetTightBoundingGeometryFromTextPositions(textSegment.Start, textSegment.End);
					CaretElement.AddGeometry(ref this._selectionGeometry, tightBoundingGeometryFromTextPositions);
				}
			}
			if (this._selectionGeometry != selectionGeometry)
			{
				this.RefreshCaret(this._italic);
			}
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x0022D03C File Offset: 0x0022C03C
		internal static void AddGeometry(ref Geometry geometry, Geometry addedGeometry)
		{
			if (addedGeometry != null)
			{
				if (geometry == null)
				{
					geometry = addedGeometry;
					return;
				}
				geometry = Geometry.Combine(geometry, addedGeometry, GeometryCombineMode.Union, null, 0.0001, ToleranceType.Absolute);
			}
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x0022D060 File Offset: 0x0022C060
		internal static void ClipGeometryByViewport(ref Geometry geometry, Rect viewport)
		{
			if (geometry != null)
			{
				Geometry geometry2 = new RectangleGeometry(viewport);
				geometry = Geometry.Combine(geometry, geometry2, GeometryCombineMode.Intersect, null, 0.0001, ToleranceType.Absolute);
			}
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x0022D090 File Offset: 0x0022C090
		internal static void AddTransformToGeometry(Geometry targetGeometry, Transform transformToAdd)
		{
			if (targetGeometry != null && transformToAdd != null)
			{
				targetGeometry.Transform = ((targetGeometry.Transform == null || targetGeometry.Transform.IsIdentity) ? transformToAdd : new MatrixTransform(targetGeometry.Transform.Value * transformToAdd.Value));
			}
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x0022D0DC File Offset: 0x0022C0DC
		internal void Hide()
		{
			if (this._showCaret)
			{
				this._showCaret = false;
				base.InvalidateVisual();
				this.SetBlinking(false);
				this.Win32DestroyCaret();
			}
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x0022D100 File Offset: 0x0022C100
		internal void RefreshCaret(bool italic)
		{
			this._italic = italic;
			AdornerLayer adornerLayer = this._adornerLayer;
			if (adornerLayer != null)
			{
				Adorner[] adorners = adornerLayer.GetAdorners(base.AdornedElement);
				if (adorners != null)
				{
					for (int i = 0; i < adorners.Length; i++)
					{
						if (adorners[i] == this)
						{
							adornerLayer.Update(base.AdornedElement);
							adornerLayer.InvalidateVisual();
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x0022D155 File Offset: 0x0022C155
		internal void DetachFromView()
		{
			this.SetBlinking(false);
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
				this._adornerLayer = null;
			}
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x0022D17C File Offset: 0x0022C17C
		internal void SetBlinking(bool isBlinkEnabled)
		{
			if (isBlinkEnabled != this._isBlinkEnabled)
			{
				if (this._isBlinkEnabled && this._blinkAnimationClock != null && this._blinkAnimationClock.CurrentState == ClockState.Active)
				{
					this._blinkAnimationClock.Controller.Stop();
				}
				this._isBlinkEnabled = isBlinkEnabled;
				if (isBlinkEnabled)
				{
					this.Win32CreateCaret();
					return;
				}
				this.Win32DestroyCaret();
			}
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x0022D1D6 File Offset: 0x0022C1D6
		internal void UpdateCaretBrush(Brush caretBrush)
		{
			this._caretBrush = caretBrush;
			this._caretElement.InvalidateVisual();
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x0022D1EC File Offset: 0x0022C1EC
		internal void OnRenderCaretSubElement(DrawingContext context)
		{
			this.Win32SetCaretPos();
			if (this._showCaret)
			{
				TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
				Invariant.Assert(!this._italic || !this.IsInInterimState, "Assert !(_italic && IsInInterimState)");
				int num = 0;
				context.PushOpacity(this._opacity);
				num++;
				if (this._italic && !threadLocalStore.Bidi)
				{
					FlowDirection flowDirection = (FlowDirection)base.AdornedElement.GetValue(FrameworkElement.FlowDirectionProperty);
					context.PushTransform(new RotateTransform((double)((flowDirection == FlowDirection.RightToLeft) ? -20 : 20), 0.0, this._height));
					num++;
				}
				if (this.IsInInterimState || this._systemCaretWidth > 1.0)
				{
					context.PushOpacity(0.5);
					num++;
				}
				if (this.IsInInterimState)
				{
					context.DrawRectangle(this._caretBrush, null, new Rect(0.0, 0.0, this._interimWidth, this._height));
				}
				else
				{
					if (!this._italic || threadLocalStore.Bidi)
					{
						GuidelineSet guidelines = new GuidelineSet(new double[]
						{
							-(this._systemCaretWidth / 2.0),
							this._systemCaretWidth / 2.0
						}, null);
						context.PushGuidelineSet(guidelines);
						num++;
					}
					context.DrawRectangle(this._caretBrush, null, new Rect(-(this._systemCaretWidth / 2.0), 0.0, this._systemCaretWidth, this._height));
				}
				if (threadLocalStore.Bidi)
				{
					double num2 = 2.0;
					if ((FlowDirection)base.AdornedElement.GetValue(FrameworkElement.FlowDirectionProperty) == FlowDirection.RightToLeft)
					{
						num2 *= -1.0;
					}
					PathGeometry pathGeometry = new PathGeometry();
					PathFigure pathFigure = new PathFigure();
					pathFigure.StartPoint = new Point(0.0, 0.0);
					pathFigure.Segments.Add(new LineSegment(new Point(-num2, 0.0), true));
					pathFigure.Segments.Add(new LineSegment(new Point(0.0, this._height / 10.0), true));
					pathFigure.IsClosed = true;
					pathGeometry.Figures.Add(pathFigure);
					context.DrawGeometry(this._caretBrush, null, pathGeometry);
				}
				for (int i = 0; i < num; i++)
				{
					context.Pop();
				}
				return;
			}
			this.Win32DestroyCaret();
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x0022D47C File Offset: 0x0022C47C
		internal void OnTextViewUpdated()
		{
			this._pendingGeometryUpdate = true;
			base.InvalidateArrange();
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x0600489F RID: 18591 RVA: 0x0022D48B File Offset: 0x0022C48B
		private static CaretElement Debug_CaretElement
		{
			get
			{
				TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
				return ((ITextSelection)TextEditor._ThreadLocalStore.FocusedTextSelection).CaretElement;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x060048A0 RID: 18592 RVA: 0x0022D4A2 File Offset: 0x0022C4A2
		private static FrameworkElement Debug_RenderScope
		{
			get
			{
				return ((ITextSelection)TextEditor._ThreadLocalStore.FocusedTextSelection).TextView.RenderScope as FrameworkElement;
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x060048A1 RID: 18593 RVA: 0x0022D4BD File Offset: 0x0022C4BD
		internal Geometry SelectionGeometry
		{
			get
			{
				return this._selectionGeometry;
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x060048A2 RID: 18594 RVA: 0x0022D4C5 File Offset: 0x0022C4C5
		// (set) Token: 0x060048A3 RID: 18595 RVA: 0x0022D4CD File Offset: 0x0022C4CD
		internal bool IsSelectionActive
		{
			get
			{
				return this._isSelectionActive;
			}
			set
			{
				this._isSelectionActive = value;
			}
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x0022D4D6 File Offset: 0x0022C4D6
		private FrameworkElement GetOwnerElement()
		{
			return CaretElement.GetOwnerElement(this._textEditor.UiScope);
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x0022D4E8 File Offset: 0x0022C4E8
		internal static FrameworkElement GetOwnerElement(FrameworkElement uiScope)
		{
			if (uiScope is IFlowDocumentViewer)
			{
				for (DependencyObject dependencyObject = uiScope; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
				{
					if (dependencyObject is FlowDocumentReader)
					{
						return (FrameworkElement)dependencyObject;
					}
				}
				return null;
			}
			return uiScope;
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x0022D520 File Offset: 0x0022C520
		private void EnsureAttachedToView()
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._textEditor.TextView.RenderScope);
			if (adornerLayer == null)
			{
				if (this._adornerLayer != null)
				{
					this._adornerLayer.Remove(this);
				}
				this._adornerLayer = null;
				return;
			}
			if (this._adornerLayer == adornerLayer)
			{
				return;
			}
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
			}
			this._adornerLayer = adornerLayer;
			this._adornerLayer.Add(this, 1073741823);
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x0022D598 File Offset: 0x0022C598
		private void SetBlinkAnimation(bool visible, bool positionChanged)
		{
			if (!this._isBlinkEnabled)
			{
				return;
			}
			int num = this.Win32GetCaretBlinkTime();
			if (num > 0)
			{
				Duration duration = new Duration(TimeSpan.FromMilliseconds((double)(num * 2)));
				if (this._blinkAnimationClock == null || this._blinkAnimationClock.Timeline.Duration != duration)
				{
					DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
					doubleAnimationUsingKeyFrames.BeginTime = null;
					doubleAnimationUsingKeyFrames.RepeatBehavior = RepeatBehavior.Forever;
					doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(1.0, KeyTime.FromPercent(0.0)));
					doubleAnimationUsingKeyFrames.KeyFrames.Add(new DiscreteDoubleKeyFrame(0.0, KeyTime.FromPercent(0.5)));
					doubleAnimationUsingKeyFrames.Duration = duration;
					Timeline.SetDesiredFrameRate(doubleAnimationUsingKeyFrames, new int?(10));
					this._blinkAnimationClock = doubleAnimationUsingKeyFrames.CreateClock();
					this._blinkAnimationClock.Controller.Begin();
					this._caretElement.ApplyAnimationClock(UIElement.OpacityProperty, this._blinkAnimationClock);
				}
			}
			else if (this._blinkAnimationClock != null)
			{
				this._caretElement.ApplyAnimationClock(UIElement.OpacityProperty, null);
				this._blinkAnimationClock = null;
			}
			if (this._blinkAnimationClock != null)
			{
				if (visible && (this._blinkAnimationClock.CurrentState > ClockState.Active || positionChanged))
				{
					this._blinkAnimationClock.Controller.Begin();
					return;
				}
				if (!visible)
				{
					this._blinkAnimationClock.Controller.Stop();
				}
			}
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x0022D708 File Offset: 0x0022C708
		private void Win32CreateCaret()
		{
			if (!this._isSelectionActive)
			{
				return;
			}
			if (!this._win32Caret || this._win32Height != this._height)
			{
				IntPtr intPtr = IntPtr.Zero;
				PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this);
				if (presentationSource != null)
				{
					intPtr = (presentationSource as IWin32Window).Handle;
				}
				if (intPtr != IntPtr.Zero)
				{
					double y = presentationSource.CompositionTarget.TransformToDevice.Transform(new Point(0.0, this._height)).Y;
					NativeMethods.BitmapHandle hbitmap = UnsafeNativeMethods.CreateBitmap(1, this.ConvertToInt32(y), 1, 1, null);
					bool flag = UnsafeNativeMethods.CreateCaret(new HandleRef(null, intPtr), hbitmap, 0, 0);
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (flag)
					{
						this._win32Caret = true;
						this._win32Height = this._height;
						return;
					}
					this._win32Caret = false;
					throw new Win32Exception(lastWin32Error);
				}
			}
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x0022D7DE File Offset: 0x0022C7DE
		private void Win32DestroyCaret()
		{
			if (!this._isSelectionActive)
			{
				return;
			}
			if (this._win32Caret)
			{
				SafeNativeMethods.DestroyCaret();
				Marshal.GetLastWin32Error();
				this._win32Caret = false;
				this._win32Height = 0.0;
			}
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x0022D814 File Offset: 0x0022C814
		private void Win32SetCaretPos()
		{
			if (!this._isSelectionActive)
			{
				return;
			}
			if (!this._win32Caret)
			{
				this.Win32CreateCaret();
			}
			PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this);
			if (presentationSource != null)
			{
				Point point = new Point(0.0, 0.0);
				if (!this._caretElement.TransformToAncestor(presentationSource.RootVisual).TryTransform(point, out point))
				{
					point = new Point(0.0, 0.0);
				}
				point = presentationSource.CompositionTarget.TransformToDevice.Transform(point);
				bool flag = SafeNativeMethods.SetCaretPos(this.ConvertToInt32(point.X), this.ConvertToInt32(point.Y));
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (!flag)
				{
					this._win32Caret = false;
					this.Win32CreateCaret();
					bool flag2 = SafeNativeMethods.SetCaretPos(this.ConvertToInt32(point.X), this.ConvertToInt32(point.Y));
					lastWin32Error = Marshal.GetLastWin32Error();
					if (!flag2)
					{
						throw new Win32Exception(lastWin32Error);
					}
				}
			}
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x0022D90C File Offset: 0x0022C90C
		private int ConvertToInt32(double value)
		{
			int result;
			if (double.IsNaN(value))
			{
				result = 0;
			}
			else if (value < -2147483648.0)
			{
				result = int.MinValue;
			}
			else if (value > 2147483647.0)
			{
				result = int.MaxValue;
			}
			else
			{
				result = Convert.ToInt32(value);
			}
			return result;
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x0022D958 File Offset: 0x0022C958
		private int Win32GetCaretBlinkTime()
		{
			Invariant.Assert(this._isSelectionActive, "Blink animation should only be required for an owner with active selection.");
			int caretBlinkTime = SafeNativeMethods.GetCaretBlinkTime();
			if (caretBlinkTime == 0)
			{
				return -1;
			}
			return caretBlinkTime;
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x060048AD RID: 18605 RVA: 0x0022D981 File Offset: 0x0022C981
		private bool IsInInterimState
		{
			get
			{
				return this._interimWidth != 0.0;
			}
		}

		// Token: 0x0400260F RID: 9743
		internal const double BidiCaretIndicatorWidth = 2.0;

		// Token: 0x04002610 RID: 9744
		internal const double CaretPaddingWidth = 5.0;

		// Token: 0x04002611 RID: 9745
		private readonly TextEditor _textEditor;

		// Token: 0x04002612 RID: 9746
		private bool _showCaret;

		// Token: 0x04002613 RID: 9747
		private bool _isSelectionActive;

		// Token: 0x04002614 RID: 9748
		private AnimationClock _blinkAnimationClock;

		// Token: 0x04002615 RID: 9749
		private double _left;

		// Token: 0x04002616 RID: 9750
		private double _top;

		// Token: 0x04002617 RID: 9751
		private double _systemCaretWidth;

		// Token: 0x04002618 RID: 9752
		private double _interimWidth;

		// Token: 0x04002619 RID: 9753
		private double _height;

		// Token: 0x0400261A RID: 9754
		private double _win32Height;

		// Token: 0x0400261B RID: 9755
		private bool _isBlinkEnabled;

		// Token: 0x0400261C RID: 9756
		private Brush _caretBrush;

		// Token: 0x0400261D RID: 9757
		private double _opacity;

		// Token: 0x0400261E RID: 9758
		private AdornerLayer _adornerLayer;

		// Token: 0x0400261F RID: 9759
		private bool _italic;

		// Token: 0x04002620 RID: 9760
		private bool _win32Caret;

		// Token: 0x04002621 RID: 9761
		private const double CaretOpacity = 0.5;

		// Token: 0x04002622 RID: 9762
		private const double BidiIndicatorHeightRatio = 10.0;

		// Token: 0x04002623 RID: 9763
		private const double DefaultNarrowCaretWidth = 1.0;

		// Token: 0x04002624 RID: 9764
		private Geometry _selectionGeometry;

		// Token: 0x04002625 RID: 9765
		internal const double c_geometryCombineTolerance = 0.0001;

		// Token: 0x04002626 RID: 9766
		internal const double c_endOfParaMagicMultiplier = 0.5;

		// Token: 0x04002627 RID: 9767
		internal const int ZOrderValue = 1073741823;

		// Token: 0x04002628 RID: 9768
		private readonly CaretElement.CaretSubElement _caretElement;

		// Token: 0x04002629 RID: 9769
		private bool _pendingGeometryUpdate;

		// Token: 0x0400262A RID: 9770
		private bool _scrolledToCurrentPositionYet;

		// Token: 0x02000B2A RID: 2858
		private class CaretSubElement : UIElement
		{
			// Token: 0x06008C7F RID: 35967 RVA: 0x0033CE80 File Offset: 0x0033BE80
			internal CaretSubElement()
			{
			}

			// Token: 0x06008C80 RID: 35968 RVA: 0x00109403 File Offset: 0x00108403
			protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
			{
				return null;
			}

			// Token: 0x06008C81 RID: 35969 RVA: 0x0033CE88 File Offset: 0x0033BE88
			protected override void OnRender(DrawingContext drawingContext)
			{
				((CaretElement)this._parent).OnRenderCaretSubElement(drawingContext);
			}
		}
	}
}
