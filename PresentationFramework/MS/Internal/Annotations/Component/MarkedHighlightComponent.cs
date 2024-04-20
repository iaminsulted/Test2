using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using MS.Internal.Annotations.Anchoring;
using MS.Utility;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002CD RID: 717
	internal sealed class MarkedHighlightComponent : Canvas, IAnnotationComponent
	{
		// Token: 0x06001AE0 RID: 6880 RVA: 0x00165C34 File Offset: 0x00164C34
		public MarkedHighlightComponent(XmlQualifiedName type, DependencyObject host)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._DPHost = ((host == null) ? this : host);
			base.ClipToBounds = false;
			this.HighlightAnchor = new HighlightComponent(1, true, type);
			base.Children.Add(this.HighlightAnchor);
			this._leftMarker = null;
			this._rightMarker = null;
			this._state = 0;
			this.SetState();
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x00165CB4 File Offset: 0x00164CB4
		public IList AttachedAnnotations
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				if (this._attachedAnnotation != null)
				{
					arrayList.Add(this._attachedAnnotation);
				}
				return arrayList;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x00165CDD File Offset: 0x00164CDD
		// (set) Token: 0x06001AE3 RID: 6883 RVA: 0x00165CE5 File Offset: 0x00164CE5
		public PresentationContext PresentationContext
		{
			get
			{
				return this._presentationContext;
			}
			set
			{
				this._presentationContext = value;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x0016545A File Offset: 0x0016445A
		// (set) Token: 0x06001AE5 RID: 6885 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public int ZOrder
		{
			get
			{
				return -1;
			}
			set
			{
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x00165CEE File Offset: 0x00164CEE
		public UIElement AnnotatedElement
		{
			get
			{
				if (this._attachedAnnotation == null)
				{
					return null;
				}
				return this._attachedAnnotation.Parent as UIElement;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x00165D0A File Offset: 0x00164D0A
		// (set) Token: 0x06001AE8 RID: 6888 RVA: 0x00165D12 File Offset: 0x00164D12
		public bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				this._isDirty = value;
				if (value)
				{
					this.UpdateGeometry();
				}
			}
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x00165D24 File Offset: 0x00164D24
		public GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (this._attachedAnnotation == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidAttachedAnnotation"));
			}
			this.HighlightAnchor.GetDesiredTransform(transform);
			return transform;
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x00165D4C File Offset: 0x00164D4C
		public void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (this._attachedAnnotation != null)
			{
				throw new ArgumentException(SR.Get("MoreThanOneAttachedAnnotation"));
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedMHBegin);
			this._attachedAnnotation = attachedAnnotation;
			if ((attachedAnnotation.AttachmentLevel & AttachmentLevel.StartPortion) != AttachmentLevel.Unresolved)
			{
				this._leftMarker = this.CreateMarker(this.GetMarkerGeometry());
			}
			if ((attachedAnnotation.AttachmentLevel & AttachmentLevel.EndPortion) != AttachmentLevel.Unresolved)
			{
				this._rightMarker = this.CreateMarker(this.GetMarkerGeometry());
			}
			this.RegisterAnchor();
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedMHEnd);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x00165DD0 File Offset: 0x00164DD0
		public void RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (attachedAnnotation != this._attachedAnnotation)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnnotation"), "attachedAnnotation");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedMHBegin);
			this.CleanUpAnchor();
			this._attachedAnnotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedMHEnd);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x00165692 File Offset: 0x00164692
		public void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			throw new NotSupportedException(SR.Get("NotSupported"));
		}

		// Token: 0x170004FF RID: 1279
		// (set) Token: 0x06001AED RID: 6893 RVA: 0x00165E30 File Offset: 0x00164E30
		public bool Focused
		{
			set
			{
				byte state = this._state;
				if (value)
				{
					this._state |= 1;
				}
				else
				{
					this._state &= 126;
				}
				if (this._state == 0 != (state == 0))
				{
					this.SetState();
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (set) Token: 0x06001AEE RID: 6894 RVA: 0x00165E7D File Offset: 0x00164E7D
		public Brush MarkerBrush
		{
			set
			{
				base.SetValue(MarkedHighlightComponent.MarkerBrushProperty, value);
			}
		}

		// Token: 0x17000501 RID: 1281
		// (set) Token: 0x06001AEF RID: 6895 RVA: 0x00165E8B File Offset: 0x00164E8B
		public double StrokeThickness
		{
			set
			{
				base.SetValue(MarkedHighlightComponent.StrokeThicknessProperty, value);
			}
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x00165E9E File Offset: 0x00164E9E
		internal void SetTabIndex(int index)
		{
			if (this._DPHost != null)
			{
				KeyboardNavigation.SetTabIndex(this._DPHost, index);
			}
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x00165EB4 File Offset: 0x00164EB4
		private void SetMarkerTransform(Path marker, ITextPointer anchor, ITextPointer baseAnchor, int xScaleFactor)
		{
			if (marker == null)
			{
				return;
			}
			GeometryGroup geometryGroup = marker.Data as GeometryGroup;
			Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(anchor);
			if (anchorRectangle == Rect.Empty)
			{
				return;
			}
			double num = anchorRectangle.Height - MarkedHighlightComponent.MarkerVerticalSpace - this._bottomTailHeight - this._topTailHeight;
			double scaleY = 0.0;
			double scaleY2 = 0.0;
			if (num > 0.0)
			{
				scaleY = num / this._bodyHeight;
				scaleY2 = 1.0;
			}
			ScaleTransform value = new ScaleTransform(1.0, scaleY);
			TranslateTransform value2 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Y + this._topTailHeight + MarkedHighlightComponent.MarkerVerticalSpace);
			TransformGroup transformGroup = new TransformGroup();
			transformGroup.Children.Add(value);
			transformGroup.Children.Add(value2);
			ScaleTransform value3 = new ScaleTransform((double)xScaleFactor, scaleY2);
			TranslateTransform value4 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Bottom - this._bottomTailHeight);
			TranslateTransform value5 = new TranslateTransform(anchorRectangle.X, anchorRectangle.Top + MarkedHighlightComponent.MarkerVerticalSpace);
			TransformGroup transformGroup2 = new TransformGroup();
			transformGroup2.Children.Add(value3);
			transformGroup2.Children.Add(value4);
			TransformGroup transformGroup3 = new TransformGroup();
			transformGroup3.Children.Add(value3);
			transformGroup3.Children.Add(value5);
			if (geometryGroup.Children[0] != null)
			{
				geometryGroup.Children[0].Transform = transformGroup3;
			}
			if (geometryGroup.Children[1] != null)
			{
				geometryGroup.Children[1].Transform = transformGroup;
			}
			if (geometryGroup.Children[2] != null)
			{
				geometryGroup.Children[2].Transform = transformGroup2;
			}
			if (baseAnchor != null)
			{
				ITextView documentPageTextView = TextSelectionHelper.GetDocumentPageTextView(baseAnchor);
				ITextView documentPageTextView2 = TextSelectionHelper.GetDocumentPageTextView(anchor);
				if (documentPageTextView != documentPageTextView2 && documentPageTextView.RenderScope != null && documentPageTextView2.RenderScope != null)
				{
					geometryGroup.Transform = (Transform)documentPageTextView2.RenderScope.TransformToVisual(documentPageTextView.RenderScope);
				}
			}
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x001660C4 File Offset: 0x001650C4
		private void SetSelected(bool selected)
		{
			byte state = this._state;
			if (selected && this._uiParent.IsFocused)
			{
				this._state |= 2;
			}
			else
			{
				this._state &= 125;
			}
			if (this._state == 0 != (state == 0))
			{
				this.SetState();
			}
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x00166120 File Offset: 0x00165120
		private void RemoveHighlightMarkers()
		{
			if (this._leftMarker != null)
			{
				base.Children.Remove(this._leftMarker);
			}
			if (this._rightMarker != null)
			{
				base.Children.Remove(this._rightMarker);
			}
			this._leftMarker = null;
			this._rightMarker = null;
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x00166170 File Offset: 0x00165170
		private void RegisterAnchor()
		{
			TextAnchor textAnchor = this._attachedAnnotation.AttachedAnchor as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnchor"));
			}
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			this.HighlightAnchor.AddAttachedAnnotation(this._attachedAnnotation);
			this.UpdateGeometry();
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.AnnotatedElement);
			if (adornerLayer == null)
			{
				throw new InvalidOperationException(SR.Get("NoPresentationContextForGivenElement", new object[]
				{
					this.AnnotatedElement
				}));
			}
			AdornerPresentationContext.HostComponent(adornerLayer, this, this.AnnotatedElement, false);
			this._selection = textContainer.TextSelection;
			if (this._selection != null)
			{
				this._uiParent = (PathNode.GetParent(textContainer.Parent) as UIElement);
				this.RegisterComponent();
				if (this._uiParent != null)
				{
					this._uiParent.GotKeyboardFocus += this.OnContainerGotFocus;
					this._uiParent.LostKeyboardFocus += this.OnContainerLostFocus;
					if (this.HighlightAnchor.IsSelected(this._selection))
					{
						this.SetSelected(true);
					}
				}
			}
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00166280 File Offset: 0x00165280
		private void CleanUpAnchor()
		{
			if (this._selection != null)
			{
				this.UnregisterComponent();
				if (this._uiParent != null)
				{
					this._uiParent.GotKeyboardFocus -= this.OnContainerGotFocus;
					this._uiParent.LostKeyboardFocus -= this.OnContainerLostFocus;
				}
			}
			this._presentationContext.RemoveFromHost(this, false);
			if (this.HighlightAnchor != null)
			{
				this.HighlightAnchor.RemoveAttachedAnnotation(this._attachedAnnotation);
				base.Children.Remove(this.HighlightAnchor);
				this.HighlightAnchor = null;
				this.RemoveHighlightMarkers();
			}
			this._attachedAnnotation = null;
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0016631C File Offset: 0x0016531C
		private void SetState()
		{
			if (this._state == 0)
			{
				if (this._highlightAnchor != null)
				{
					this._highlightAnchor.Activate(false);
				}
				this.MarkerBrush = new SolidColorBrush(MarkedHighlightComponent.DefaultMarkerColor);
				this.StrokeThickness = MarkedHighlightComponent.MarkerStrokeThickness;
				this._DPHost.SetValue(StickyNoteControl.IsActiveProperty, false);
				return;
			}
			if (this._highlightAnchor != null)
			{
				this._highlightAnchor.Activate(true);
			}
			this.MarkerBrush = new SolidColorBrush(MarkedHighlightComponent.DefaultActiveMarkerColor);
			this.StrokeThickness = MarkedHighlightComponent.ActiveMarkerStrokeThickness;
			this._DPHost.SetValue(StickyNoteControl.IsActiveProperty, true);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x001663B4 File Offset: 0x001653B4
		private Path CreateMarker(Geometry geometry)
		{
			Path path = new Path();
			path.Data = geometry;
			Binding binding = new Binding("MarkerBrushProperty");
			binding.Source = this;
			path.SetBinding(Shape.StrokeProperty, binding);
			Binding binding2 = new Binding("StrokeThicknessProperty");
			binding2.Source = this;
			path.SetBinding(Shape.StrokeThicknessProperty, binding2);
			path.StrokeEndLineCap = PenLineCap.Round;
			path.StrokeStartLineCap = PenLineCap.Round;
			base.Children.Add(path);
			return path;
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x00166428 File Offset: 0x00165428
		private void RegisterComponent()
		{
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[this._selection];
			if (componentsRegister == null)
			{
				componentsRegister = new MarkedHighlightComponent.ComponentsRegister(new EventHandler(MarkedHighlightComponent.OnSelectionChanged), new MouseEventHandler(MarkedHighlightComponent.OnMouseMove));
				MarkedHighlightComponent._documentHandlers.Add(this._selection, componentsRegister);
				this._selection.Changed += componentsRegister.SelectionHandler;
				if (this._uiParent != null)
				{
					this._uiParent.MouseMove += componentsRegister.MouseMoveHandler;
				}
			}
			componentsRegister.Add(this);
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x001664B0 File Offset: 0x001654B0
		private void UnregisterComponent()
		{
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[this._selection];
			componentsRegister.Remove(this);
			if (componentsRegister.Components.Count == 0)
			{
				MarkedHighlightComponent._documentHandlers.Remove(this._selection);
				this._selection.Changed -= componentsRegister.SelectionHandler;
				if (this._uiParent != null)
				{
					this._uiParent.MouseMove -= componentsRegister.MouseMoveHandler;
				}
			}
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x00166524 File Offset: 0x00165524
		private void UpdateGeometry()
		{
			if (this.HighlightAnchor == null || this.HighlightAnchor == null)
			{
				throw new Exception(SR.Get("UndefinedHighlightAnchor"));
			}
			TextAnchor range = ((IHighlightRange)this.HighlightAnchor).Range;
			ITextPointer textPointer = range.Start.CreatePointer(LogicalDirection.Forward);
			ITextPointer textPointer2 = range.End.CreatePointer(LogicalDirection.Backward);
			FlowDirection textFlowDirection = MarkedHighlightComponent.GetTextFlowDirection(textPointer);
			FlowDirection textFlowDirection2 = MarkedHighlightComponent.GetTextFlowDirection(textPointer2);
			this.SetMarkerTransform(this._leftMarker, textPointer, null, (textFlowDirection == FlowDirection.LeftToRight) ? 1 : -1);
			this.SetMarkerTransform(this._rightMarker, textPointer2, textPointer, (textFlowDirection2 == FlowDirection.LeftToRight) ? -1 : 1);
			this.HighlightAnchor.IsDirty = true;
			this.IsDirty = false;
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x001665C0 File Offset: 0x001655C0
		private Geometry GetMarkerGeometry()
		{
			GeometryGroup geometryGroup = new GeometryGroup();
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 1.0), new Point(1.0, 0.0)));
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 0.0), new Point(0.0, 50.0)));
			geometryGroup.Children.Add(new LineGeometry(new Point(0.0, 0.0), new Point(1.0, 1.0)));
			this._bodyHeight = geometryGroup.Children[1].Bounds.Height;
			this._topTailHeight = geometryGroup.Children[0].Bounds.Height;
			this._bottomTailHeight = geometryGroup.Children[2].Bounds.Height;
			return geometryGroup;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x001666EC File Offset: 0x001656EC
		private void CheckPosition(ITextPointer position)
		{
			bool flag = ((IHighlightRange)this._highlightAnchor).Range.Contains(position);
			bool flag2 = (bool)this._DPHost.GetValue(StickyNoteControl.IsMouseOverAnchorProperty);
			if (flag != flag2)
			{
				this._DPHost.SetValue(StickyNoteControl.IsMouseOverAnchorPropertyKey, flag);
			}
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x00166736 File Offset: 0x00165736
		private void OnContainerGotFocus(object sender, KeyboardFocusChangedEventArgs args)
		{
			if (this.HighlightAnchor != null && this.HighlightAnchor.IsSelected(this._selection))
			{
				this.SetSelected(true);
			}
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x0016675A File Offset: 0x0016575A
		private void OnContainerLostFocus(object sender, KeyboardFocusChangedEventArgs args)
		{
			this.SetSelected(false);
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x00166763 File Offset: 0x00165763
		// (set) Token: 0x06001B00 RID: 6912 RVA: 0x0016676B File Offset: 0x0016576B
		internal HighlightComponent HighlightAnchor
		{
			get
			{
				return this._highlightAnchor;
			}
			set
			{
				this._highlightAnchor = value;
				if (this._highlightAnchor != null)
				{
					this._highlightAnchor.DefaultBackground = MarkedHighlightComponent.DefaultAnchorBackground;
					this._highlightAnchor.DefaultActiveBackground = MarkedHighlightComponent.DefaultActiveAnchorBackground;
				}
			}
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0016679C File Offset: 0x0016579C
		private static FlowDirection GetTextFlowDirection(ITextPointer pointer)
		{
			Invariant.Assert(pointer != null, "Null pointer passed.");
			Invariant.Assert(pointer.IsAtInsertionPosition, "Pointer is not an insertion position");
			int num = 0;
			LogicalDirection logicalDirection = pointer.LogicalDirection;
			TextPointerContext pointerContext = pointer.GetPointerContext(logicalDirection);
			FlowDirection result;
			if ((pointerContext == TextPointerContext.ElementEnd || pointerContext == TextPointerContext.ElementStart) && !TextSchema.IsFormattingType(pointer.ParentType))
			{
				result = (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
			}
			else
			{
				Rect anchorRectangle = TextSelectionHelper.GetAnchorRectangle(pointer);
				ITextPointer textPointer = pointer.GetNextInsertionPosition(logicalDirection);
				if (textPointer != null)
				{
					textPointer = textPointer.CreatePointer((logicalDirection == LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward);
					if (logicalDirection == LogicalDirection.Forward)
					{
						if (pointerContext == TextPointerContext.ElementEnd && textPointer.GetPointerContext(textPointer.LogicalDirection) == TextPointerContext.ElementStart)
						{
							return (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
						}
					}
					else if (pointerContext == TextPointerContext.ElementStart && textPointer.GetPointerContext(textPointer.LogicalDirection) == TextPointerContext.ElementEnd)
					{
						return (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
					}
					Rect anchorRectangle2 = TextSelectionHelper.GetAnchorRectangle(textPointer);
					if (anchorRectangle2 != Rect.Empty && anchorRectangle != Rect.Empty)
					{
						num = Math.Sign(anchorRectangle2.Left - anchorRectangle.Left);
						if (logicalDirection == LogicalDirection.Backward)
						{
							num = -num;
						}
					}
				}
				if (num == 0)
				{
					result = (FlowDirection)pointer.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				else
				{
					result = ((num > 0) ? FlowDirection.LeftToRight : FlowDirection.RightToLeft);
				}
			}
			return result;
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x001668DC File Offset: 0x001658DC
		private static void OnSelectionChanged(object sender, EventArgs args)
		{
			ITextRange textRange = sender as ITextRange;
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[textRange];
			if (componentsRegister == null)
			{
				return;
			}
			List<MarkedHighlightComponent> components = componentsRegister.Components;
			bool[] array = new bool[components.Count];
			for (int i = 0; i < components.Count; i++)
			{
				array[i] = components[i].HighlightAnchor.IsSelected(textRange);
				if (!array[i])
				{
					components[i].SetSelected(false);
				}
			}
			for (int j = 0; j < components.Count; j++)
			{
				if (array[j])
				{
					components[j].SetSelected(true);
				}
			}
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00166984 File Offset: 0x00165984
		private static void OnMouseMove(object sender, MouseEventArgs args)
		{
			IServiceProvider serviceProvider = sender as IServiceProvider;
			if (serviceProvider == null)
			{
				return;
			}
			ITextView textView = (ITextView)serviceProvider.GetService(typeof(ITextView));
			if (textView == null || !textView.IsValid)
			{
				return;
			}
			Point position = Mouse.PrimaryDevice.GetPosition(textView.RenderScope);
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(position, false);
			if (textPositionFromPoint != null)
			{
				MarkedHighlightComponent.CheckAllHighlightRanges(textPositionFromPoint);
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x001669E4 File Offset: 0x001659E4
		private static void CheckAllHighlightRanges(ITextPointer pos)
		{
			ITextRange textSelection = pos.TextContainer.TextSelection;
			if (textSelection == null)
			{
				return;
			}
			MarkedHighlightComponent.ComponentsRegister componentsRegister = (MarkedHighlightComponent.ComponentsRegister)MarkedHighlightComponent._documentHandlers[textSelection];
			if (componentsRegister == null)
			{
				return;
			}
			List<MarkedHighlightComponent> components = componentsRegister.Components;
			for (int i = 0; i < components.Count; i++)
			{
				components[i].CheckPosition(pos);
			}
		}

		// Token: 0x04000DD4 RID: 3540
		public static DependencyProperty MarkerBrushProperty = DependencyProperty.Register("MarkerBrushProperty", typeof(Brush), typeof(MarkedHighlightComponent));

		// Token: 0x04000DD5 RID: 3541
		public static DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThicknessProperty", typeof(double), typeof(MarkedHighlightComponent));

		// Token: 0x04000DD6 RID: 3542
		internal static Color DefaultAnchorBackground = (Color)ColorConverter.ConvertFromString("#3380FF80");

		// Token: 0x04000DD7 RID: 3543
		internal static Color DefaultMarkerColor = (Color)ColorConverter.ConvertFromString("#FF008000");

		// Token: 0x04000DD8 RID: 3544
		internal static Color DefaultActiveAnchorBackground = (Color)ColorConverter.ConvertFromString("#3300FF00");

		// Token: 0x04000DD9 RID: 3545
		internal static Color DefaultActiveMarkerColor = (Color)ColorConverter.ConvertFromString("#FF008000");

		// Token: 0x04000DDA RID: 3546
		internal static double MarkerStrokeThickness = 1.0;

		// Token: 0x04000DDB RID: 3547
		internal static double ActiveMarkerStrokeThickness = 2.0;

		// Token: 0x04000DDC RID: 3548
		internal static double MarkerVerticalSpace = 2.0;

		// Token: 0x04000DDD RID: 3549
		private static Hashtable _documentHandlers = new Hashtable();

		// Token: 0x04000DDE RID: 3550
		private byte _state;

		// Token: 0x04000DDF RID: 3551
		private HighlightComponent _highlightAnchor;

		// Token: 0x04000DE0 RID: 3552
		private double _bodyHeight;

		// Token: 0x04000DE1 RID: 3553
		private double _bottomTailHeight;

		// Token: 0x04000DE2 RID: 3554
		private double _topTailHeight;

		// Token: 0x04000DE3 RID: 3555
		private Path _leftMarker;

		// Token: 0x04000DE4 RID: 3556
		private Path _rightMarker;

		// Token: 0x04000DE5 RID: 3557
		private DependencyObject _DPHost;

		// Token: 0x04000DE6 RID: 3558
		private const byte FocusFlag = 1;

		// Token: 0x04000DE7 RID: 3559
		private const byte FocusFlagComplement = 126;

		// Token: 0x04000DE8 RID: 3560
		private const byte SelectedFlag = 2;

		// Token: 0x04000DE9 RID: 3561
		private const byte SelectedFlagComplement = 125;

		// Token: 0x04000DEA RID: 3562
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04000DEB RID: 3563
		private PresentationContext _presentationContext;

		// Token: 0x04000DEC RID: 3564
		private bool _isDirty = true;

		// Token: 0x04000DED RID: 3565
		private ITextRange _selection;

		// Token: 0x04000DEE RID: 3566
		private UIElement _uiParent;

		// Token: 0x02000A19 RID: 2585
		private class ComponentsRegister
		{
			// Token: 0x060084EA RID: 34026 RVA: 0x003279E3 File Offset: 0x003269E3
			public ComponentsRegister(EventHandler selectionHandler, MouseEventHandler mouseMoveHandler)
			{
				this._components = new List<MarkedHighlightComponent>();
				this._selectionHandler = selectionHandler;
				this._mouseMoveHandler = mouseMoveHandler;
			}

			// Token: 0x060084EB RID: 34027 RVA: 0x00327A04 File Offset: 0x00326A04
			public void Add(MarkedHighlightComponent component)
			{
				if (this._components.Count == 0)
				{
					UIElement host = component.PresentationContext.Host;
					if (host != null)
					{
						KeyboardNavigation.SetTabNavigation(host, KeyboardNavigationMode.Local);
						KeyboardNavigation.SetControlTabNavigation(host, KeyboardNavigationMode.Local);
					}
				}
				int i = 0;
				while (i < this._components.Count && this.Compare(this._components[i], component) <= 0)
				{
					i++;
				}
				this._components.Insert(i, component);
				while (i < this._components.Count)
				{
					this._components[i].SetTabIndex(i);
					i++;
				}
			}

			// Token: 0x060084EC RID: 34028 RVA: 0x00327A9C File Offset: 0x00326A9C
			public void Remove(MarkedHighlightComponent component)
			{
				int i = 0;
				while (i < this._components.Count && this._components[i] != component)
				{
					i++;
				}
				if (i < this._components.Count)
				{
					this._components.RemoveAt(i);
					while (i < this._components.Count)
					{
						this._components[i].SetTabIndex(i);
						i++;
					}
				}
			}

			// Token: 0x17001DE1 RID: 7649
			// (get) Token: 0x060084ED RID: 34029 RVA: 0x00327B0E File Offset: 0x00326B0E
			public List<MarkedHighlightComponent> Components
			{
				get
				{
					return this._components;
				}
			}

			// Token: 0x17001DE2 RID: 7650
			// (get) Token: 0x060084EE RID: 34030 RVA: 0x00327B16 File Offset: 0x00326B16
			public EventHandler SelectionHandler
			{
				get
				{
					return this._selectionHandler;
				}
			}

			// Token: 0x17001DE3 RID: 7651
			// (get) Token: 0x060084EF RID: 34031 RVA: 0x00327B1E File Offset: 0x00326B1E
			public MouseEventHandler MouseMoveHandler
			{
				get
				{
					return this._mouseMoveHandler;
				}
			}

			// Token: 0x060084F0 RID: 34032 RVA: 0x00327B28 File Offset: 0x00326B28
			private int Compare(IAnnotationComponent first, IAnnotationComponent second)
			{
				TextAnchor textAnchor = ((IAttachedAnnotation)first.AttachedAnnotations[0]).FullyAttachedAnchor as TextAnchor;
				TextAnchor textAnchor2 = ((IAttachedAnnotation)second.AttachedAnnotations[0]).FullyAttachedAnchor as TextAnchor;
				int num = textAnchor.Start.CompareTo(textAnchor2.Start);
				if (num == 0)
				{
					num = textAnchor.End.CompareTo(textAnchor2.End);
				}
				return num;
			}

			// Token: 0x0400409E RID: 16542
			private List<MarkedHighlightComponent> _components;

			// Token: 0x0400409F RID: 16543
			private EventHandler _selectionHandler;

			// Token: 0x040040A0 RID: 16544
			private MouseEventHandler _mouseMoveHandler;
		}
	}
}
