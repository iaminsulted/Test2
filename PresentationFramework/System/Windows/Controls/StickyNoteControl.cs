using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Annotations;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Component;
using MS.Internal.Commands;
using MS.Internal.Controls.StickyNote;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Utility;

namespace System.Windows.Controls
{
	// Token: 0x02000710 RID: 1808
	[TemplatePart(Name = "PART_EraseMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_TitleThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_ResizeBottomRightThumb", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_ContentControl", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_IconButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_CopyMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_PasteMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_InkMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_SelectMenuItem", Type = typeof(MenuItem))]
	[TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
	public sealed class StickyNoteControl : Control, IAnnotationComponent
	{
		// Token: 0x06005E2A RID: 24106 RVA: 0x0028E6B0 File Offset: 0x0028D6B0
		void IAnnotationComponent.AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (this._attachedAnnotation == null)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedSNBegin);
				this.SetAnnotation(attachedAnnotation);
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedSNEnd);
				return;
			}
			throw new InvalidOperationException(SR.Get("AddAnnotationsNotImplemented"));
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x0028E704 File Offset: 0x0028D704
		void IAnnotationComponent.RemoveAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			if (attachedAnnotation == this._attachedAnnotation)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedSNBegin);
				this.GiveUpFocus();
				this.ClearAnnotation();
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedSNEnd);
				return;
			}
			throw new ArgumentException(SR.Get("InvalidValueSpecified"), "attachedAnnotation");
		}

		// Token: 0x06005E2C RID: 24108 RVA: 0x00165692 File Offset: 0x00164692
		void IAnnotationComponent.ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			throw new NotSupportedException(SR.Get("NotSupported"));
		}

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06005E2D RID: 24109 RVA: 0x0028E764 File Offset: 0x0028D764
		IList IAnnotationComponent.AttachedAnnotations
		{
			get
			{
				ArrayList arrayList = new ArrayList(1);
				if (this._attachedAnnotation != null)
				{
					arrayList.Add(this._attachedAnnotation);
				}
				return arrayList;
			}
		}

		// Token: 0x06005E2E RID: 24110 RVA: 0x0028E790 File Offset: 0x0028D790
		GeneralTransform IAnnotationComponent.GetDesiredTransform(GeneralTransform transform)
		{
			if (this._attachedAnnotation == null)
			{
				return null;
			}
			if (this.IsExpanded && base.FlowDirection == FlowDirection.RightToLeft && this._attachedAnnotation.Parent is DocumentPageHost)
			{
				this._selfMirroring = true;
			}
			else
			{
				this._selfMirroring = false;
			}
			Point anchorPoint = this._attachedAnnotation.AnchorPoint;
			if (double.IsInfinity(anchorPoint.X) || double.IsInfinity(anchorPoint.Y))
			{
				throw new InvalidOperationException(SR.Get("InvalidAnchorPosition"));
			}
			if (double.IsNaN(anchorPoint.X) || double.IsNaN(anchorPoint.Y))
			{
				return null;
			}
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			if (this._selfMirroring)
			{
				generalTransformGroup.Children.Add(new MatrixTransform(-1.0, 0.0, 0.0, 1.0, base.Width, 0.0));
			}
			generalTransformGroup.Children.Add(new TranslateTransform(anchorPoint.X, anchorPoint.Y));
			TranslateTransform translateTransform = new TranslateTransform(0.0, 0.0);
			if (this.IsExpanded)
			{
				translateTransform = this.PositionTransform.Clone();
				this._deltaX = (this._deltaY = 0.0);
				Rect pageBounds = this.PageBounds;
				Rect stickyNoteBounds = this.StickyNoteBounds;
				double num;
				double num2;
				StickyNoteControl.GetOffsets(pageBounds, stickyNoteBounds, out num, out num2);
				if (DoubleUtil.GreaterThan(Math.Abs(num), Math.Abs(this._offsetX)))
				{
					double num3 = this._offsetX - num;
					if (DoubleUtil.LessThan(num3, 0.0))
					{
						num3 = Math.Max(num3, -(stickyNoteBounds.Left - pageBounds.Left));
					}
					translateTransform.X += num3;
					this._deltaX = num3;
				}
				if (DoubleUtil.GreaterThan(Math.Abs(num2), Math.Abs(this._offsetY)))
				{
					double num4 = this._offsetY - num2;
					if (DoubleUtil.LessThan(num4, 0.0))
					{
						num4 = Math.Max(num4, -(stickyNoteBounds.Top - pageBounds.Top));
					}
					translateTransform.Y += num4;
					this._deltaY = num4;
				}
			}
			if (translateTransform != null)
			{
				generalTransformGroup.Children.Add(translateTransform);
			}
			generalTransformGroup.Children.Add(transform);
			return generalTransformGroup;
		}

		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06005E2F RID: 24111 RVA: 0x0028E9EE File Offset: 0x0028D9EE
		UIElement IAnnotationComponent.AnnotatedElement
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

		// Token: 0x170015C0 RID: 5568
		// (get) Token: 0x06005E30 RID: 24112 RVA: 0x0028EA0A File Offset: 0x0028DA0A
		// (set) Token: 0x06005E31 RID: 24113 RVA: 0x0028EA12 File Offset: 0x0028DA12
		PresentationContext IAnnotationComponent.PresentationContext
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

		// Token: 0x170015C1 RID: 5569
		// (get) Token: 0x06005E32 RID: 24114 RVA: 0x0028EA1B File Offset: 0x0028DA1B
		// (set) Token: 0x06005E33 RID: 24115 RVA: 0x0028EA23 File Offset: 0x0028DA23
		int IAnnotationComponent.ZOrder
		{
			get
			{
				return this._zOrder;
			}
			set
			{
				this._zOrder = value;
				this.UpdateAnnotationWithSNC(XmlToken.ZOrder);
			}
		}

		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x06005E34 RID: 24116 RVA: 0x0028EA37 File Offset: 0x0028DA37
		// (set) Token: 0x06005E35 RID: 24117 RVA: 0x0028EA4E File Offset: 0x0028DA4E
		bool IAnnotationComponent.IsDirty
		{
			get
			{
				return this._anchor != null && this._anchor.IsDirty;
			}
			set
			{
				if (this._anchor != null)
				{
					this._anchor.IsDirty = value;
				}
				if (value)
				{
					base.InvalidateVisual();
				}
			}
		}

		// Token: 0x170015C3 RID: 5571
		// (get) Token: 0x06005E36 RID: 24118 RVA: 0x0028EA6D File Offset: 0x0028DA6D
		// (set) Token: 0x06005E37 RID: 24119 RVA: 0x0028EA75 File Offset: 0x0028DA75
		internal TranslateTransform PositionTransform
		{
			get
			{
				return this._positionTransform;
			}
			set
			{
				Invariant.Assert(value != null, "PositionTransform cannot be null.");
				this._positionTransform = value;
				this.InvalidateTransform();
			}
		}

		// Token: 0x170015C4 RID: 5572
		// (get) Token: 0x06005E38 RID: 24120 RVA: 0x0028EA92 File Offset: 0x0028DA92
		// (set) Token: 0x06005E39 RID: 24121 RVA: 0x0028EA9A File Offset: 0x0028DA9A
		internal double XOffset
		{
			get
			{
				return this._offsetX;
			}
			set
			{
				this._offsetX = value;
			}
		}

		// Token: 0x170015C5 RID: 5573
		// (get) Token: 0x06005E3A RID: 24122 RVA: 0x0028EAA3 File Offset: 0x0028DAA3
		// (set) Token: 0x06005E3B RID: 24123 RVA: 0x0028EAAB File Offset: 0x0028DAAB
		internal double YOffset
		{
			get
			{
				return this._offsetY;
			}
			set
			{
				this._offsetY = value;
			}
		}

		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x06005E3C RID: 24124 RVA: 0x0028EAB4 File Offset: 0x0028DAB4
		internal bool FlipBothOrigins
		{
			get
			{
				return this.IsExpanded && base.FlowDirection == FlowDirection.RightToLeft && this._attachedAnnotation != null && this._attachedAnnotation.Parent is DocumentPageHost;
			}
		}

		// Token: 0x06005E3D RID: 24125 RVA: 0x0028EAE4 File Offset: 0x0028DAE4
		private void OnAuthorUpdated(object obj, AnnotationAuthorChangedEventArgs args)
		{
			if (!this.InternalLocker.IsLocked(LockHelper.LockFlag.AnnotationChanged))
			{
				this.UpdateSNCWithAnnotation(XmlToken.Author);
				this.IsDirty = true;
			}
		}

		// Token: 0x06005E3E RID: 24126 RVA: 0x0028EB08 File Offset: 0x0028DB08
		private void OnAnnotationUpdated(object obj, AnnotationResourceChangedEventArgs args)
		{
			if (!this.InternalLocker.IsLocked(LockHelper.LockFlag.AnnotationChanged))
			{
				SNCAnnotation sncAnnotation = new SNCAnnotation(args.Annotation);
				this._sncAnnotation = sncAnnotation;
				this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder);
				this.IsDirty = true;
			}
		}

		// Token: 0x06005E3F RID: 24127 RVA: 0x0028EB48 File Offset: 0x0028DB48
		private void SetAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			SNCAnnotation sncannotation = new SNCAnnotation(attachedAnnotation.Annotation);
			bool hasInkData = sncannotation.HasInkData;
			bool hasTextData = sncannotation.HasTextData;
			if (hasInkData && hasTextData)
			{
				throw new ArgumentException(SR.Get("InvalidStickyNoteAnnotation"), "attachedAnnotation");
			}
			if (hasInkData)
			{
				this._stickyNoteType = StickyNoteType.Ink;
			}
			else if (hasTextData)
			{
				this._stickyNoteType = StickyNoteType.Text;
			}
			if (this.Content != null)
			{
				this.EnsureStickyNoteType();
			}
			if (sncannotation.IsNewAnnotation)
			{
				AnnotationResource item = new AnnotationResource("Meta Data");
				attachedAnnotation.Annotation.Cargos.Add(item);
			}
			this._attachedAnnotation = attachedAnnotation;
			this._attachedAnnotation.Annotation.CargoChanged += this.OnAnnotationUpdated;
			this._attachedAnnotation.Annotation.AuthorChanged += this.OnAuthorUpdated;
			this._sncAnnotation = sncannotation;
			this._anchor.AddAttachedAnnotation(attachedAnnotation);
			this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder);
			this.IsDirty = false;
			if ((this._attachedAnnotation.AttachmentLevel & AttachmentLevel.StartPortion) == AttachmentLevel.Unresolved)
			{
				base.SetValue(UIElement.VisibilityProperty, Visibility.Collapsed);
				return;
			}
			base.RequestBringIntoView += this.OnRequestBringIntoView;
		}

		// Token: 0x06005E40 RID: 24128 RVA: 0x0028EC68 File Offset: 0x0028DC68
		private void ClearAnnotation()
		{
			this._attachedAnnotation.Annotation.CargoChanged -= this.OnAnnotationUpdated;
			this._attachedAnnotation.Annotation.AuthorChanged -= this.OnAuthorUpdated;
			this._anchor.RemoveAttachedAnnotation(this._attachedAnnotation);
			this._sncAnnotation = null;
			this._attachedAnnotation = null;
			base.RequestBringIntoView -= this.OnRequestBringIntoView;
		}

		// Token: 0x06005E41 RID: 24129 RVA: 0x0028ECE0 File Offset: 0x0028DCE0
		private void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			FrameworkElement frameworkElement = ((IAnnotationComponent)this).AnnotatedElement as FrameworkElement;
			DocumentPageHost documentPageHost = frameworkElement as DocumentPageHost;
			if (documentPageHost != null)
			{
				frameworkElement = (documentPageHost.PageVisual as FrameworkElement);
			}
			if (frameworkElement == null)
			{
				return;
			}
			IScrollInfo scrollInfo = frameworkElement as IScrollInfo;
			if (scrollInfo != null)
			{
				Rect stickyNoteBounds = this.StickyNoteBounds;
				Rect rect = new Rect(0.0, 0.0, scrollInfo.ViewportWidth, scrollInfo.ViewportHeight);
				if (stickyNoteBounds.IntersectsWith(rect))
				{
					return;
				}
			}
			Transform transform = (Transform)base.TransformToVisual(frameworkElement);
			Rect rect2 = new Rect(0.0, 0.0, base.Width, base.Height);
			rect2.Transform(transform.Value);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.DispatchBringIntoView), new object[]
			{
				frameworkElement,
				rect2
			});
		}

		// Token: 0x06005E42 RID: 24130 RVA: 0x0028EDC4 File Offset: 0x0028DDC4
		private object DispatchBringIntoView(object arg)
		{
			object[] array = (object[])arg;
			FrameworkElement frameworkElement = (FrameworkElement)array[0];
			Rect targetRectangle = (Rect)array[1];
			frameworkElement.BringIntoView(targetRectangle);
			return null;
		}

		// Token: 0x06005E43 RID: 24131 RVA: 0x0028EDF0 File Offset: 0x0028DDF0
		private void UpdateSNCWithAnnotation(XmlToken tokens)
		{
			if (this._sncAnnotation != null)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateSNCWithAnnotationBegin);
				using (new LockHelper.AutoLocker(this.InternalLocker, LockHelper.LockFlag.DataChanged))
				{
					SNCAnnotation.UpdateStickyNoteControl(tokens, this, this._sncAnnotation);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateSNCWithAnnotationEnd);
			}
		}

		// Token: 0x06005E44 RID: 24132 RVA: 0x0028EE54 File Offset: 0x0028DE54
		private void UpdateAnnotationWithSNC(XmlToken tokens)
		{
			if (this._sncAnnotation != null && !this.InternalLocker.IsLocked(LockHelper.LockFlag.DataChanged))
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateAnnotationWithSNCBegin);
				using (new LockHelper.AutoLocker(this.InternalLocker, LockHelper.LockFlag.AnnotationChanged))
				{
					SNCAnnotation.UpdateAnnotation(tokens, this, this._sncAnnotation);
				}
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.UpdateAnnotationWithSNCEnd);
			}
		}

		// Token: 0x06005E45 RID: 24133 RVA: 0x0028EEC8 File Offset: 0x0028DEC8
		private void UpdateOffsets()
		{
			if (this._attachedAnnotation != null)
			{
				Rect pageBounds = this.PageBounds;
				Rect stickyNoteBounds = this.StickyNoteBounds;
				if (!pageBounds.IsEmpty && !stickyNoteBounds.IsEmpty)
				{
					Invariant.Assert(DoubleUtil.GreaterThan(stickyNoteBounds.Right, pageBounds.Left), "Note's right is off left of page.");
					Invariant.Assert(DoubleUtil.LessThan(stickyNoteBounds.Left, pageBounds.Right), "Note's left is off right of page.");
					Invariant.Assert(DoubleUtil.GreaterThan(stickyNoteBounds.Bottom, pageBounds.Top), "Note's bottom is off top of page.");
					Invariant.Assert(DoubleUtil.LessThan(stickyNoteBounds.Top, pageBounds.Bottom), "Note's top is off bottom of page.");
					double num;
					double num2;
					StickyNoteControl.GetOffsets(pageBounds, stickyNoteBounds, out num, out num2);
					if (!DoubleUtil.AreClose(this.XOffset, num))
					{
						this.XOffset = num;
					}
					if (!DoubleUtil.AreClose(this.YOffset, num2))
					{
						this.YOffset = num2;
					}
				}
			}
		}

		// Token: 0x06005E46 RID: 24134 RVA: 0x0028EFB0 File Offset: 0x0028DFB0
		private static void GetOffsets(Rect rectPage, Rect rectStickyNote, out double offsetX, out double offsetY)
		{
			offsetX = 0.0;
			if (DoubleUtil.LessThan(rectStickyNote.Left, rectPage.Left))
			{
				offsetX = rectStickyNote.Left - rectPage.Left;
			}
			else if (DoubleUtil.GreaterThan(rectStickyNote.Right, rectPage.Right))
			{
				offsetX = rectStickyNote.Right - rectPage.Right;
			}
			offsetY = 0.0;
			if (DoubleUtil.LessThan(rectStickyNote.Top, rectPage.Top))
			{
				offsetY = rectStickyNote.Top - rectPage.Top;
				return;
			}
			if (DoubleUtil.GreaterThan(rectStickyNote.Bottom, rectPage.Bottom))
			{
				offsetY = rectStickyNote.Bottom - rectPage.Bottom;
			}
		}

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06005E47 RID: 24135 RVA: 0x0028F070 File Offset: 0x0028E070
		private Rect StickyNoteBounds
		{
			get
			{
				Rect empty = Rect.Empty;
				Point anchorPoint = this._attachedAnnotation.AnchorPoint;
				if (!double.IsNaN(anchorPoint.X) && !double.IsNaN(anchorPoint.Y) && this.PositionTransform != null)
				{
					empty = new Rect(anchorPoint.X + this.PositionTransform.X + this._deltaX, anchorPoint.Y + this.PositionTransform.Y + this._deltaY, base.Width, base.Height);
				}
				return empty;
			}
		}

		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06005E48 RID: 24136 RVA: 0x0028F0FC File Offset: 0x0028E0FC
		private Rect PageBounds
		{
			get
			{
				Rect empty = Rect.Empty;
				IScrollInfo scrollInfo = ((IAnnotationComponent)this).AnnotatedElement as IScrollInfo;
				if (scrollInfo != null)
				{
					empty = new Rect(-scrollInfo.HorizontalOffset, -scrollInfo.VerticalOffset, scrollInfo.ExtentWidth, scrollInfo.ExtentHeight);
				}
				else
				{
					UIElement annotatedElement = ((IAnnotationComponent)this).AnnotatedElement;
					if (annotatedElement != null)
					{
						Size renderSize = annotatedElement.RenderSize;
						empty = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
					}
				}
				return empty;
			}
		}

		// Token: 0x06005E49 RID: 24137 RVA: 0x0028F180 File Offset: 0x0028E180
		static StickyNoteControl()
		{
			Type typeFromHandle = typeof(StickyNoteControl);
			EventManager.RegisterClassHandler(typeFromHandle, Stylus.PreviewStylusDownEvent, new StylusDownEventHandler(StickyNoteControl._OnPreviewDeviceDown<StylusDownEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(StickyNoteControl._OnPreviewDeviceDown<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, Mouse.MouseDownEvent, new MouseButtonEventHandler(StickyNoteControl._OnDeviceDown<MouseButtonEventArgs>));
			EventManager.RegisterClassHandler(typeFromHandle, ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(StickyNoteControl._OnContextMenuOpening));
			CommandHelpers.RegisterCommandHandler(typeof(StickyNoteControl), StickyNoteControl.DeleteNoteCommand, new ExecutedRoutedEventHandler(StickyNoteControl._OnCommandExecuted), new CanExecuteRoutedEventHandler(StickyNoteControl._OnQueryCommandEnabled));
			CommandHelpers.RegisterCommandHandler(typeof(StickyNoteControl), StickyNoteControl.InkCommand, new ExecutedRoutedEventHandler(StickyNoteControl._OnCommandExecuted), new CanExecuteRoutedEventHandler(StickyNoteControl._OnQueryCommandEnabled));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new ComponentResourceKey(typeof(PresentationUIStyleResources), "StickyNoteControlStyleKey")));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			Control.IsTabStopProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false));
			Control.ForegroundProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl._UpdateInkDrawingAttributes)));
			Control.FontFamilyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontSizeProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontStretchProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontStyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
			Control.FontWeightProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(StickyNoteControl.OnFontPropertyChanged)));
		}

		// Token: 0x06005E4A RID: 24138 RVA: 0x0028F628 File Offset: 0x0028E628
		private StickyNoteControl() : this(StickyNoteType.Text)
		{
		}

		// Token: 0x06005E4B RID: 24139 RVA: 0x0028F634 File Offset: 0x0028E634
		internal StickyNoteControl(StickyNoteType type)
		{
			this._stickyNoteType = type;
			base.SetValue(StickyNoteControl.StickyNoteTypePropertyKey, type);
			this.InitStickyNoteControl();
		}

		// Token: 0x06005E4C RID: 24140 RVA: 0x0028F684 File Offset: 0x0028E684
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this.IsExpanded)
			{
				this.EnsureStickyNoteType();
			}
			this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height | XmlToken.IsExpanded | XmlToken.Author | XmlToken.Text | XmlToken.Ink | XmlToken.ZOrder);
			if (!this.IsExpanded)
			{
				Button iconButton = this.GetIconButton();
				if (iconButton != null)
				{
					iconButton.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
					return;
				}
			}
			else
			{
				Button closeButton = this.GetCloseButton();
				if (closeButton != null)
				{
					closeButton.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
				}
				Thumb titleThumb = this.GetTitleThumb();
				if (titleThumb != null)
				{
					titleThumb.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
					titleThumb.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
				}
				Thumb resizeThumb = this.GetResizeThumb();
				if (resizeThumb != null)
				{
					resizeThumb.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
					resizeThumb.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
				}
				this.SetupMenu();
			}
		}

		// Token: 0x170015C9 RID: 5577
		// (get) Token: 0x06005E4D RID: 24141 RVA: 0x0028F774 File Offset: 0x0028E774
		public string Author
		{
			get
			{
				return (string)base.GetValue(StickyNoteControl.AuthorProperty);
			}
		}

		// Token: 0x170015CA RID: 5578
		// (get) Token: 0x06005E4E RID: 24142 RVA: 0x0028F786 File Offset: 0x0028E786
		// (set) Token: 0x06005E4F RID: 24143 RVA: 0x0028F798 File Offset: 0x0028E798
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(StickyNoteControl.IsExpandedProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.IsExpandedProperty, value);
			}
		}

		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x06005E50 RID: 24144 RVA: 0x0028F7A6 File Offset: 0x0028E7A6
		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(StickyNoteControl.IsActiveProperty);
			}
		}

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x06005E51 RID: 24145 RVA: 0x0028F7B8 File Offset: 0x0028E7B8
		public bool IsMouseOverAnchor
		{
			get
			{
				return (bool)base.GetValue(StickyNoteControl.IsMouseOverAnchorProperty);
			}
		}

		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x06005E52 RID: 24146 RVA: 0x0028F7CA File Offset: 0x0028E7CA
		// (set) Token: 0x06005E53 RID: 24147 RVA: 0x0028F7DC File Offset: 0x0028E7DC
		public FontFamily CaptionFontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(StickyNoteControl.CaptionFontFamilyProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontFamilyProperty, value);
			}
		}

		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x06005E54 RID: 24148 RVA: 0x0028F7EA File Offset: 0x0028E7EA
		// (set) Token: 0x06005E55 RID: 24149 RVA: 0x0028F7FC File Offset: 0x0028E7FC
		public double CaptionFontSize
		{
			get
			{
				return (double)base.GetValue(StickyNoteControl.CaptionFontSizeProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontSizeProperty, value);
			}
		}

		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x06005E56 RID: 24150 RVA: 0x0028F80F File Offset: 0x0028E80F
		// (set) Token: 0x06005E57 RID: 24151 RVA: 0x0028F821 File Offset: 0x0028E821
		public FontStretch CaptionFontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(StickyNoteControl.CaptionFontStretchProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontStretchProperty, value);
			}
		}

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x06005E58 RID: 24152 RVA: 0x0028F834 File Offset: 0x0028E834
		// (set) Token: 0x06005E59 RID: 24153 RVA: 0x0028F846 File Offset: 0x0028E846
		public FontStyle CaptionFontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(StickyNoteControl.CaptionFontStyleProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontStyleProperty, value);
			}
		}

		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x06005E5A RID: 24154 RVA: 0x0028F859 File Offset: 0x0028E859
		// (set) Token: 0x06005E5B RID: 24155 RVA: 0x0028F86B File Offset: 0x0028E86B
		public FontWeight CaptionFontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(StickyNoteControl.CaptionFontWeightProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.CaptionFontWeightProperty, value);
			}
		}

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x06005E5C RID: 24156 RVA: 0x0028F87E File Offset: 0x0028E87E
		// (set) Token: 0x06005E5D RID: 24157 RVA: 0x0028F890 File Offset: 0x0028E890
		public double PenWidth
		{
			get
			{
				return (double)base.GetValue(StickyNoteControl.PenWidthProperty);
			}
			set
			{
				base.SetValue(StickyNoteControl.PenWidthProperty, value);
			}
		}

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x06005E5E RID: 24158 RVA: 0x0028F8A3 File Offset: 0x0028E8A3
		public StickyNoteType StickyNoteType
		{
			get
			{
				return (StickyNoteType)base.GetValue(StickyNoteControl.StickyNoteTypeProperty);
			}
		}

		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06005E5F RID: 24159 RVA: 0x0028F8B5 File Offset: 0x0028E8B5
		public IAnchorInfo AnchorInfo
		{
			get
			{
				if (this._attachedAnnotation != null)
				{
					return this._attachedAnnotation;
				}
				return null;
			}
		}

		// Token: 0x06005E60 RID: 24160 RVA: 0x0028F8C7 File Offset: 0x0028E8C7
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			this.ClearCachedControls();
		}

		// Token: 0x06005E61 RID: 24161 RVA: 0x0028F8D8 File Offset: 0x0028E8D8
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs args)
		{
			base.OnIsKeyboardFocusWithinChanged(args);
			ContextMenu contextMenu = Keyboard.FocusedElement as ContextMenu;
			if (contextMenu != null && contextMenu.PlacementTarget != null && contextMenu.PlacementTarget.IsDescendantOf(this))
			{
				return;
			}
			this._anchor.Focused = base.IsKeyboardFocusWithin;
		}

		// Token: 0x06005E62 RID: 24162 RVA: 0x0028F924 File Offset: 0x0028E924
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs args)
		{
			base.OnGotKeyboardFocus(args);
			base.ApplyTemplate();
			if (this.IsExpanded)
			{
				Invariant.Assert(this.Content != null);
				this.BringToFront();
				if (args.NewFocus == this)
				{
					UIElement innerControl = this.Content.InnerControl;
					Invariant.Assert(innerControl != null, "InnerControl is null or not a UIElement.");
					if (!innerControl.IsKeyboardFocused)
					{
						innerControl.Focus();
					}
				}
			}
		}

		// Token: 0x06005E63 RID: 24163 RVA: 0x0028F990 File Offset: 0x0028E990
		private void EnsureStickyNoteType()
		{
			UIElement contentContainer = this.GetContentContainer();
			if (this._contentControl != null)
			{
				if (this._contentControl.Type != this._stickyNoteType)
				{
					this.DisconnectContent();
					this._contentControl = StickyNoteContentControlFactory.CreateContentControl(this._stickyNoteType, contentContainer);
					this.ConnectContent();
					return;
				}
			}
			else
			{
				this._contentControl = StickyNoteContentControlFactory.CreateContentControl(this._stickyNoteType, contentContainer);
				this.ConnectContent();
			}
		}

		// Token: 0x06005E64 RID: 24164 RVA: 0x0028F9F6 File Offset: 0x0028E9F6
		private void DisconnectContent()
		{
			Invariant.Assert(this.Content != null, "Content is null.");
			this.StopListenToContentControlEvent();
			this.UnbindContentControlProperties();
			this._contentControl = null;
		}

		// Token: 0x06005E65 RID: 24165 RVA: 0x0028FA20 File Offset: 0x0028EA20
		private void ConnectContent()
		{
			Invariant.Assert(this.Content != null);
			if (this.Content.InnerControl is InkCanvas)
			{
				this.InitializeEventHandlers();
				base.SetValue(StickyNoteControl.InkEditingModeProperty, InkCanvasEditingMode.Ink);
				this.UpdateInkDrawingAttributes();
			}
			this.StartListenToContentControlEvent();
			this.BindContentControlProperties();
		}

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x06005E66 RID: 24166 RVA: 0x0028FA76 File Offset: 0x0028EA76
		internal StickyNoteContentControl Content
		{
			get
			{
				return this._contentControl;
			}
		}

		// Token: 0x06005E67 RID: 24167 RVA: 0x0028FA7E File Offset: 0x0028EA7E
		private Button GetCloseButton()
		{
			return base.GetTemplateChild("PART_CloseButton") as Button;
		}

		// Token: 0x06005E68 RID: 24168 RVA: 0x0028FA90 File Offset: 0x0028EA90
		private Button GetIconButton()
		{
			return base.GetTemplateChild("PART_IconButton") as Button;
		}

		// Token: 0x06005E69 RID: 24169 RVA: 0x0028FAA2 File Offset: 0x0028EAA2
		private Thumb GetTitleThumb()
		{
			return base.GetTemplateChild("PART_TitleThumb") as Thumb;
		}

		// Token: 0x06005E6A RID: 24170 RVA: 0x0028FAB4 File Offset: 0x0028EAB4
		private UIElement GetContentContainer()
		{
			return base.GetTemplateChild("PART_ContentControl") as UIElement;
		}

		// Token: 0x06005E6B RID: 24171 RVA: 0x0028FAC6 File Offset: 0x0028EAC6
		private Thumb GetResizeThumb()
		{
			return base.GetTemplateChild("PART_ResizeBottomRightThumb") as Thumb;
		}

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x06005E6C RID: 24172 RVA: 0x0028FAD8 File Offset: 0x0028EAD8
		// (set) Token: 0x06005E6D RID: 24173 RVA: 0x0028FAE0 File Offset: 0x0028EAE0
		private bool IsDirty
		{
			get
			{
				return this._dirty;
			}
			set
			{
				this._dirty = value;
			}
		}

		// Token: 0x06005E6E RID: 24174 RVA: 0x0028FAE9 File Offset: 0x0028EAE9
		private static void _OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((StickyNoteControl)d).OnIsExpandedChanged();
		}

		// Token: 0x06005E6F RID: 24175 RVA: 0x0028FAF8 File Offset: 0x0028EAF8
		private static void OnFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StickyNoteControl stickyNoteControl = (StickyNoteControl)d;
			if (stickyNoteControl.Content != null && stickyNoteControl.Content.Type != StickyNoteType.Ink)
			{
				FrameworkElement innerControl = stickyNoteControl.Content.InnerControl;
				if (innerControl != null)
				{
					innerControl.SetValue(e.Property, e.NewValue);
				}
			}
		}

		// Token: 0x06005E70 RID: 24176 RVA: 0x0028FB48 File Offset: 0x0028EB48
		private static void _UpdateInkDrawingAttributes(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StickyNoteControl stickyNoteControl = (StickyNoteControl)d;
			stickyNoteControl.UpdateInkDrawingAttributes();
			if (e.Property == Control.ForegroundProperty && stickyNoteControl.Content != null && stickyNoteControl.Content.Type != StickyNoteType.Ink)
			{
				FrameworkElement innerControl = stickyNoteControl.Content.InnerControl;
				if (innerControl != null)
				{
					innerControl.SetValue(Control.ForegroundProperty, e.NewValue);
				}
			}
		}

		// Token: 0x06005E71 RID: 24177 RVA: 0x0028FBA8 File Offset: 0x0028EBA8
		private void OnTextChanged(object obj, TextChangedEventArgs args)
		{
			if (!this.InternalLocker.IsLocked(LockHelper.LockFlag.DataChanged))
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationTextChangedBegin);
				try
				{
					this.AsyncUpdateAnnotation(XmlToken.Text);
					this.IsDirty = true;
				}
				finally
				{
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationTextChangedEnd);
				}
			}
		}

		// Token: 0x06005E72 RID: 24178 RVA: 0x0028FC08 File Offset: 0x0028EC08
		private static void _OnDeviceDown<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			args.Handled = true;
		}

		// Token: 0x06005E73 RID: 24179 RVA: 0x0028FC16 File Offset: 0x0028EC16
		private static void _OnContextMenuOpening(object sender, ContextMenuEventArgs args)
		{
			if (!(args.TargetElement is ScrollBar))
			{
				args.Handled = true;
			}
		}

		// Token: 0x06005E74 RID: 24180 RVA: 0x0028FC2C File Offset: 0x0028EC2C
		private static void _OnPreviewDeviceDown<TEventArgs>(object sender, TEventArgs args) where TEventArgs : InputEventArgs
		{
			StickyNoteControl stickyNoteControl = sender as StickyNoteControl;
			IInputElement inputElement = null;
			StylusDevice stylusDevice = args.Device as StylusDevice;
			if (stylusDevice != null)
			{
				inputElement = stylusDevice.Captured;
			}
			else
			{
				MouseDevice mouseDevice = args.Device as MouseDevice;
				if (mouseDevice != null)
				{
					inputElement = mouseDevice.Captured;
				}
			}
			if (stickyNoteControl != null && (inputElement == stickyNoteControl || inputElement == null))
			{
				stickyNoteControl.OnPreviewDeviceDown(sender, args);
			}
		}

		// Token: 0x06005E75 RID: 24181 RVA: 0x0028FC95 File Offset: 0x0028EC95
		private void OnInkCanvasStrokesReplacedEventHandler(object sender, InkCanvasStrokesReplacedEventArgs e)
		{
			this.StopListenToStrokesEvent(e.PreviousStrokes);
			this.StartListenToStrokesEvent(e.NewStrokes);
		}

		// Token: 0x06005E76 RID: 24182 RVA: 0x0028FCB0 File Offset: 0x0028ECB0
		private void OnInkCanvasSelectionMovingEventHandler(object sender, InkCanvasSelectionEditingEventArgs e)
		{
			Rect newRectangle = e.NewRectangle;
			if (newRectangle.X < 0.0 || newRectangle.Y < 0.0)
			{
				newRectangle.X = ((newRectangle.X < 0.0) ? 0.0 : newRectangle.X);
				newRectangle.Y = ((newRectangle.Y < 0.0) ? 0.0 : newRectangle.Y);
				e.NewRectangle = newRectangle;
			}
		}

		// Token: 0x06005E77 RID: 24183 RVA: 0x0028FD48 File Offset: 0x0028ED48
		private void OnInkCanvasSelectionResizingEventHandler(object sender, InkCanvasSelectionEditingEventArgs e)
		{
			Rect newRectangle = e.NewRectangle;
			if (newRectangle.X < 0.0 || newRectangle.Y < 0.0)
			{
				if (newRectangle.X < 0.0)
				{
					newRectangle.Width += newRectangle.X;
					newRectangle.X = 0.0;
				}
				if (newRectangle.Y < 0.0)
				{
					newRectangle.Height += newRectangle.Y;
					newRectangle.Y = 0.0;
				}
				e.NewRectangle = newRectangle;
			}
		}

		// Token: 0x06005E78 RID: 24184 RVA: 0x0028FDF8 File Offset: 0x0028EDF8
		private void OnInkStrokesChanged(object sender, StrokeCollectionChangedEventArgs args)
		{
			this.StopListenToStrokeEvent(args.Removed);
			this.StartListenToStrokeEvent(args.Added);
			if (args.Removed.Count > 0 || args.Added.Count > 0)
			{
				Invariant.Assert(this.Content != null && this.Content.InnerControl is InkCanvas);
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(this.Content.InnerControl) as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.InvalidateMeasure();
				}
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationInkChangedBegin);
			try
			{
				this.UpdateAnnotationWithSNC(XmlToken.Ink);
				this.IsDirty = true;
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AnnotationInkChangedEnd);
			}
		}

		// Token: 0x06005E79 RID: 24185 RVA: 0x0028FEB8 File Offset: 0x0028EEB8
		private void InitStickyNoteControl()
		{
			XmlQualifiedName type = (this._stickyNoteType == StickyNoteType.Text) ? StickyNoteControl.TextSchemaName : StickyNoteControl.InkSchemaName;
			this._anchor = new MarkedHighlightComponent(type, this);
			this.IsDirty = false;
			base.Loaded += this.OnLoadedEventHandler;
		}

		// Token: 0x06005E7A RID: 24186 RVA: 0x0028FF00 File Offset: 0x0028EF00
		private void InitializeEventHandlers()
		{
			this._propertyDataChangedHandler = new StickyNoteControl.StrokeChangedHandler<PropertyDataChangedEventArgs>(this);
			this._strokeDrawingAttributesReplacedHandler = new StickyNoteControl.StrokeChangedHandler<DrawingAttributesReplacedEventArgs>(this);
			this._strokePacketDataChangedHandler = new StickyNoteControl.StrokeChangedHandler<EventArgs>(this);
		}

		// Token: 0x06005E7B RID: 24187 RVA: 0x0028FF28 File Offset: 0x0028EF28
		private void OnButtonClick(object sender, RoutedEventArgs e)
		{
			bool isExpanded = this.IsExpanded;
			base.SetCurrentValueInternal(StickyNoteControl.IsExpandedProperty, BooleanBoxes.Box(!isExpanded));
		}

		// Token: 0x06005E7C RID: 24188 RVA: 0x0028FF50 File Offset: 0x0028EF50
		private void DeleteStickyNote()
		{
			Invariant.Assert(this._attachedAnnotation != null, "AttachedAnnotation is null.");
			Invariant.Assert(this._attachedAnnotation.Store != null, "AttachedAnnotation's Store is null.");
			this._attachedAnnotation.Store.DeleteAnnotation(this._attachedAnnotation.Annotation.Id);
		}

		// Token: 0x06005E7D RID: 24189 RVA: 0x0028FFAC File Offset: 0x0028EFAC
		private void OnDragCompleted(object sender, DragCompletedEventArgs args)
		{
			Thumb thumb = args.Source as Thumb;
			if (thumb == this.GetTitleThumb())
			{
				this.UpdateAnnotationWithSNC(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset);
				return;
			}
			if (thumb == this.GetResizeThumb())
			{
				this.UpdateAnnotationWithSNC(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height);
			}
		}

		// Token: 0x06005E7E RID: 24190 RVA: 0x0028FFEC File Offset: 0x0028EFEC
		private void OnDragDelta(object sender, DragDeltaEventArgs args)
		{
			Invariant.Assert(this.IsExpanded, "Dragging occurred when the StickyNoteControl was not expanded.");
			Thumb thumb = args.Source as Thumb;
			double num = args.HorizontalChange;
			if (this._selfMirroring)
			{
				num = -num;
			}
			if (thumb == this.GetTitleThumb())
			{
				this.OnTitleDragDelta(num, args.VerticalChange);
			}
			else if (thumb == this.GetResizeThumb())
			{
				this.OnResizeDragDelta(args.HorizontalChange, args.VerticalChange);
			}
			this.UpdateOffsets();
		}

		// Token: 0x06005E7F RID: 24191 RVA: 0x00290060 File Offset: 0x0028F060
		private void OnTitleDragDelta(double horizontalChange, double verticalChange)
		{
			Invariant.Assert(this.IsExpanded);
			Rect stickyNoteBounds = this.StickyNoteBounds;
			Rect pageBounds = this.PageBounds;
			double num = 45.0;
			double num2 = 20.0;
			if (this._selfMirroring)
			{
				double num3 = num2;
				num2 = num;
				num = num3;
			}
			Point point = new Point(-(stickyNoteBounds.X + stickyNoteBounds.Width - num), -stickyNoteBounds.Y);
			Point point2 = new Point(pageBounds.Width - stickyNoteBounds.X - num2, pageBounds.Height - stickyNoteBounds.Y - 20.0);
			horizontalChange = Math.Min(Math.Max(point.X, horizontalChange), point2.X);
			verticalChange = Math.Min(Math.Max(point.Y, verticalChange), point2.Y);
			TranslateTransform positionTransform = this.PositionTransform;
			this.PositionTransform = new TranslateTransform(positionTransform.X + horizontalChange + this._deltaX, positionTransform.Y + verticalChange + this._deltaY);
			this._deltaX = (this._deltaY = 0.0);
			this.IsDirty = true;
		}

		// Token: 0x06005E80 RID: 24192 RVA: 0x00290184 File Offset: 0x0028F184
		private void OnResizeDragDelta(double horizontalChange, double verticalChange)
		{
			Invariant.Assert(this.IsExpanded);
			Rect stickyNoteBounds = this.StickyNoteBounds;
			double num = stickyNoteBounds.Width + horizontalChange;
			double num2 = stickyNoteBounds.Height + verticalChange;
			if (!this._selfMirroring && stickyNoteBounds.X + num < 45.0)
			{
				num = stickyNoteBounds.Width;
			}
			double minWidth = base.MinWidth;
			double minHeight = base.MinHeight;
			if (num < minWidth)
			{
				num = minWidth;
				horizontalChange = num - base.Width;
			}
			if (num2 < minHeight)
			{
				num2 = minHeight;
			}
			base.SetCurrentValueInternal(FrameworkElement.WidthProperty, num);
			base.SetCurrentValueInternal(FrameworkElement.HeightProperty, num2);
			if (this._selfMirroring)
			{
				this.OnTitleDragDelta(-horizontalChange, 0.0);
			}
			else
			{
				this.OnTitleDragDelta(0.0, 0.0);
			}
			this.IsDirty = true;
		}

		// Token: 0x06005E81 RID: 24193 RVA: 0x00290260 File Offset: 0x0028F260
		private void OnPreviewDeviceDown(object dc, InputEventArgs args)
		{
			if (this.IsExpanded)
			{
				bool flag = false;
				if (!base.IsKeyboardFocusWithin && this.StickyNoteType == StickyNoteType.Ink)
				{
					Visual visual = args.OriginalSource as Visual;
					if (visual != null)
					{
						Invariant.Assert(this.Content.InnerControl != null, "InnerControl is null.");
						flag = visual.IsDescendantOf(this.Content.InnerControl);
					}
				}
				this.BringToFront();
				if (!this.IsActive || !base.IsKeyboardFocusWithin)
				{
					base.Focus();
				}
				if (flag)
				{
					args.Handled = true;
				}
			}
		}

		// Token: 0x06005E82 RID: 24194 RVA: 0x002902E8 File Offset: 0x0028F2E8
		private void OnLoadedEventHandler(object sender, RoutedEventArgs e)
		{
			if (this.IsExpanded)
			{
				this.UpdateSNCWithAnnotation(XmlToken.Left | XmlToken.Top | XmlToken.XOffset | XmlToken.YOffset | XmlToken.Width | XmlToken.Height);
				if (this._sncAnnotation.IsNewAnnotation)
				{
					base.Focus();
				}
			}
			base.Loaded -= this.OnLoadedEventHandler;
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x00290324 File Offset: 0x0028F324
		private void ClearCachedControls()
		{
			if (this.Content != null)
			{
				this.DisconnectContent();
			}
			Button closeButton = this.GetCloseButton();
			if (closeButton != null)
			{
				closeButton.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
			}
			Button iconButton = this.GetIconButton();
			if (iconButton != null)
			{
				iconButton.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.OnButtonClick));
			}
			Thumb titleThumb = this.GetTitleThumb();
			if (titleThumb != null)
			{
				titleThumb.RemoveHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
				titleThumb.RemoveHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
			}
			Thumb resizeThumb = this.GetResizeThumb();
			if (resizeThumb != null)
			{
				resizeThumb.RemoveHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(this.OnDragDelta));
				resizeThumb.RemoveHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.OnDragCompleted));
			}
		}

		// Token: 0x06005E84 RID: 24196 RVA: 0x002903F4 File Offset: 0x0028F3F4
		private void OnIsExpandedChanged()
		{
			this.InvalidateTransform();
			this.UpdateAnnotationWithSNC(XmlToken.IsExpanded);
			this.IsDirty = true;
			if (this.IsExpanded)
			{
				this.BringToFront();
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.AsyncTakeFocus), null);
				return;
			}
			this.GiveUpFocus();
			this.SendToBack();
		}

		// Token: 0x06005E85 RID: 24197 RVA: 0x0029044E File Offset: 0x0028F44E
		private object AsyncTakeFocus(object notUsed)
		{
			base.Focus();
			return null;
		}

		// Token: 0x06005E86 RID: 24198 RVA: 0x00290458 File Offset: 0x0028F458
		private void GiveUpFocus()
		{
			if (base.IsKeyboardFocusWithin)
			{
				bool flag = false;
				DependencyObject dependencyObject = this._attachedAnnotation.Parent;
				while (dependencyObject != null && !flag)
				{
					IInputElement inputElement = dependencyObject as IInputElement;
					if (inputElement != null)
					{
						flag = inputElement.Focus();
					}
					if (!flag)
					{
						dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject);
					}
				}
				if (!flag)
				{
					Keyboard.Focus(null);
				}
			}
		}

		// Token: 0x06005E87 RID: 24199 RVA: 0x002904AC File Offset: 0x0028F4AC
		private void BringToFront()
		{
			PresentationContext presentationContext = ((IAnnotationComponent)this).PresentationContext;
			if (presentationContext != null)
			{
				presentationContext.BringToFront(this);
			}
		}

		// Token: 0x06005E88 RID: 24200 RVA: 0x002904CC File Offset: 0x0028F4CC
		private void SendToBack()
		{
			PresentationContext presentationContext = ((IAnnotationComponent)this).PresentationContext;
			if (presentationContext != null)
			{
				presentationContext.SendToBack(this);
			}
		}

		// Token: 0x06005E89 RID: 24201 RVA: 0x002904EC File Offset: 0x0028F4EC
		private void InvalidateTransform()
		{
			PresentationContext presentationContext = ((IAnnotationComponent)this).PresentationContext;
			if (presentationContext != null)
			{
				presentationContext.InvalidateTransform(this);
			}
		}

		// Token: 0x06005E8A RID: 24202 RVA: 0x0029050A File Offset: 0x0028F50A
		private object AsyncUpdateAnnotation(object arg)
		{
			this.UpdateAnnotationWithSNC((XmlToken)arg);
			return null;
		}

		// Token: 0x06005E8B RID: 24203 RVA: 0x0029051C File Offset: 0x0028F51C
		private void BindContentControlProperties()
		{
			Invariant.Assert(this.Content != null);
			if (this.Content.Type != StickyNoteType.Ink)
			{
				FrameworkElement innerControl = this.Content.InnerControl;
				innerControl.SetValue(Control.FontFamilyProperty, base.GetValue(Control.FontFamilyProperty));
				innerControl.SetValue(Control.FontSizeProperty, base.GetValue(Control.FontSizeProperty));
				innerControl.SetValue(Control.FontStretchProperty, base.GetValue(Control.FontStretchProperty));
				innerControl.SetValue(Control.FontStyleProperty, base.GetValue(Control.FontStyleProperty));
				innerControl.SetValue(Control.FontWeightProperty, base.GetValue(Control.FontWeightProperty));
				innerControl.SetValue(Control.ForegroundProperty, base.GetValue(Control.ForegroundProperty));
				return;
			}
			MultiBinding multiBinding = new MultiBinding();
			multiBinding.Mode = BindingMode.TwoWay;
			multiBinding.Converter = new StickyNoteControl.InkEditingModeIsKeyboardFocusWithin2EditingMode();
			Binding binding = new Binding();
			binding.Mode = BindingMode.TwoWay;
			binding.Path = new PropertyPath(StickyNoteControl.InkEditingModeProperty);
			binding.Source = this;
			multiBinding.Bindings.Add(binding);
			Binding binding2 = new Binding();
			binding2.Path = new PropertyPath(UIElement.IsKeyboardFocusWithinProperty);
			binding2.Source = this;
			multiBinding.Bindings.Add(binding2);
			this.Content.InnerControl.SetBinding(InkCanvas.EditingModeProperty, multiBinding);
		}

		// Token: 0x06005E8C RID: 24204 RVA: 0x00290660 File Offset: 0x0028F660
		private void UnbindContentControlProperties()
		{
			Invariant.Assert(this.Content != null);
			FrameworkElement innerControl = this.Content.InnerControl;
			if (this.Content.Type != StickyNoteType.Ink)
			{
				innerControl.ClearValue(Control.FontFamilyProperty);
				innerControl.ClearValue(Control.FontSizeProperty);
				innerControl.ClearValue(Control.FontStretchProperty);
				innerControl.ClearValue(Control.FontStyleProperty);
				innerControl.ClearValue(Control.FontWeightProperty);
				innerControl.ClearValue(Control.ForegroundProperty);
				return;
			}
			BindingOperations.ClearBinding(innerControl, InkCanvas.EditingModeProperty);
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x002906E4 File Offset: 0x0028F6E4
		private void StartListenToContentControlEvent()
		{
			Invariant.Assert(this.Content != null);
			if (this.Content.Type == StickyNoteType.Ink)
			{
				InkCanvas inkCanvas = this.Content.InnerControl as InkCanvas;
				Invariant.Assert(inkCanvas != null, "InnerControl is not an InkCanvas for note of type Ink.");
				inkCanvas.StrokesReplaced += this.OnInkCanvasStrokesReplacedEventHandler;
				inkCanvas.SelectionMoving += this.OnInkCanvasSelectionMovingEventHandler;
				inkCanvas.SelectionResizing += this.OnInkCanvasSelectionResizingEventHandler;
				this.StartListenToStrokesEvent(inkCanvas.Strokes);
				return;
			}
			TextBoxBase textBoxBase = this.Content.InnerControl as TextBoxBase;
			Invariant.Assert(textBoxBase != null, "InnerControl is not a TextBoxBase for note of type Text.");
			textBoxBase.TextChanged += this.OnTextChanged;
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x002907A0 File Offset: 0x0028F7A0
		private void StopListenToContentControlEvent()
		{
			Invariant.Assert(this.Content != null);
			if (this.Content.Type == StickyNoteType.Ink)
			{
				InkCanvas inkCanvas = this.Content.InnerControl as InkCanvas;
				Invariant.Assert(inkCanvas != null, "InnerControl is not an InkCanvas for note of type Ink.");
				inkCanvas.StrokesReplaced -= this.OnInkCanvasStrokesReplacedEventHandler;
				inkCanvas.SelectionMoving -= this.OnInkCanvasSelectionMovingEventHandler;
				inkCanvas.SelectionResizing -= this.OnInkCanvasSelectionResizingEventHandler;
				this.StopListenToStrokesEvent(inkCanvas.Strokes);
				return;
			}
			TextBoxBase textBoxBase = this.Content.InnerControl as TextBoxBase;
			Invariant.Assert(textBoxBase != null, "InnerControl is not a TextBoxBase for note of type Text.");
			textBoxBase.TextChanged -= this.OnTextChanged;
		}

		// Token: 0x06005E8F RID: 24207 RVA: 0x0029085A File Offset: 0x0028F85A
		private void StartListenToStrokesEvent(StrokeCollection strokes)
		{
			strokes.StrokesChanged += this.OnInkStrokesChanged;
			strokes.PropertyDataChanged += this._propertyDataChangedHandler.OnStrokeChanged;
			this.StartListenToStrokeEvent(strokes);
		}

		// Token: 0x06005E90 RID: 24208 RVA: 0x0029088C File Offset: 0x0028F88C
		private void StopListenToStrokesEvent(StrokeCollection strokes)
		{
			strokes.StrokesChanged -= this.OnInkStrokesChanged;
			strokes.PropertyDataChanged -= this._propertyDataChangedHandler.OnStrokeChanged;
			this.StopListenToStrokeEvent(strokes);
		}

		// Token: 0x06005E91 RID: 24209 RVA: 0x002908C0 File Offset: 0x0028F8C0
		private void StartListenToStrokeEvent(IEnumerable<Stroke> strokes)
		{
			foreach (Stroke stroke in strokes)
			{
				stroke.DrawingAttributes.AttributeChanged += this._propertyDataChangedHandler.OnStrokeChanged;
				stroke.DrawingAttributesReplaced += this._strokeDrawingAttributesReplacedHandler.OnStrokeChanged;
				stroke.StylusPointsReplaced += new StylusPointsReplacedEventHandler(this._strokePacketDataChangedHandler.OnStrokeChanged);
				stroke.StylusPoints.Changed += this._strokePacketDataChangedHandler.OnStrokeChanged;
				stroke.PropertyDataChanged += this._propertyDataChangedHandler.OnStrokeChanged;
			}
		}

		// Token: 0x06005E92 RID: 24210 RVA: 0x00290984 File Offset: 0x0028F984
		private void StopListenToStrokeEvent(IEnumerable<Stroke> strokes)
		{
			foreach (Stroke stroke in strokes)
			{
				stroke.DrawingAttributes.AttributeChanged -= this._propertyDataChangedHandler.OnStrokeChanged;
				stroke.DrawingAttributesReplaced -= this._strokeDrawingAttributesReplacedHandler.OnStrokeChanged;
				stroke.StylusPointsReplaced -= new StylusPointsReplacedEventHandler(this._strokePacketDataChangedHandler.OnStrokeChanged);
				stroke.StylusPoints.Changed -= this._strokePacketDataChangedHandler.OnStrokeChanged;
				stroke.PropertyDataChanged -= this._propertyDataChangedHandler.OnStrokeChanged;
			}
		}

		// Token: 0x06005E93 RID: 24211 RVA: 0x00290A48 File Offset: 0x0028FA48
		private void SetupMenu()
		{
			MenuItem inkMenuItem = this.GetInkMenuItem();
			if (inkMenuItem != null)
			{
				Binding binding = new Binding("InkEditingMode");
				binding.Mode = BindingMode.OneWay;
				binding.RelativeSource = RelativeSource.TemplatedParent;
				binding.Converter = new StickyNoteControl.InkEditingModeConverter();
				binding.ConverterParameter = InkCanvasEditingMode.Ink;
				inkMenuItem.SetBinding(MenuItem.IsCheckedProperty, binding);
			}
			MenuItem selectMenuItem = this.GetSelectMenuItem();
			if (selectMenuItem != null)
			{
				Binding binding2 = new Binding("InkEditingMode");
				binding2.Mode = BindingMode.OneWay;
				binding2.RelativeSource = RelativeSource.TemplatedParent;
				binding2.Converter = new StickyNoteControl.InkEditingModeConverter();
				binding2.ConverterParameter = InkCanvasEditingMode.Select;
				selectMenuItem.SetBinding(MenuItem.IsCheckedProperty, binding2);
			}
			MenuItem eraseMenuItem = this.GetEraseMenuItem();
			if (eraseMenuItem != null)
			{
				Binding binding3 = new Binding("InkEditingMode");
				binding3.Mode = BindingMode.OneWay;
				binding3.RelativeSource = RelativeSource.TemplatedParent;
				binding3.Converter = new StickyNoteControl.InkEditingModeConverter();
				binding3.ConverterParameter = InkCanvasEditingMode.EraseByStroke;
				eraseMenuItem.SetBinding(MenuItem.IsCheckedProperty, binding3);
			}
			MenuItem copyMenuItem = this.GetCopyMenuItem();
			if (copyMenuItem != null)
			{
				copyMenuItem.CommandTarget = this.Content.InnerControl;
			}
			MenuItem pasteMenuItem = this.GetPasteMenuItem();
			if (pasteMenuItem != null)
			{
				pasteMenuItem.CommandTarget = this.Content.InnerControl;
			}
		}

		// Token: 0x06005E94 RID: 24212 RVA: 0x00290B84 File Offset: 0x0028FB84
		private static void _OnCommandExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			RoutedCommand routedCommand = (RoutedCommand)args.Command;
			StickyNoteControl stickyNoteControl = sender as StickyNoteControl;
			Invariant.Assert(stickyNoteControl != null, "Unexpected Commands");
			Invariant.Assert(routedCommand == StickyNoteControl.DeleteNoteCommand || routedCommand == StickyNoteControl.InkCommand, "Unknown Commands");
			if (routedCommand == StickyNoteControl.DeleteNoteCommand)
			{
				stickyNoteControl.DeleteStickyNote();
				return;
			}
			if (routedCommand == StickyNoteControl.InkCommand)
			{
				StickyNoteContentControl content = stickyNoteControl.Content;
				if (content == null || content.Type != StickyNoteType.Ink)
				{
					throw new InvalidOperationException(SR.Get("CannotProcessInkCommand"));
				}
				InkCanvasEditingMode inkCanvasEditingMode = (InkCanvasEditingMode)args.Parameter;
				stickyNoteControl.SetValue(StickyNoteControl.InkEditingModeProperty, inkCanvasEditingMode);
			}
		}

		// Token: 0x06005E95 RID: 24213 RVA: 0x00290C28 File Offset: 0x0028FC28
		private static void _OnQueryCommandEnabled(object sender, CanExecuteRoutedEventArgs args)
		{
			RoutedCommand routedCommand = (RoutedCommand)args.Command;
			StickyNoteControl stickyNoteControl = sender as StickyNoteControl;
			Invariant.Assert(stickyNoteControl != null, "Unexpected Commands");
			Invariant.Assert(routedCommand == StickyNoteControl.DeleteNoteCommand || routedCommand == StickyNoteControl.InkCommand, "Unknown Commands");
			if (routedCommand == StickyNoteControl.DeleteNoteCommand)
			{
				args.CanExecute = (stickyNoteControl._attachedAnnotation != null);
				return;
			}
			if (routedCommand == StickyNoteControl.InkCommand)
			{
				StickyNoteContentControl content = stickyNoteControl.Content;
				args.CanExecute = (content != null && content.Type == StickyNoteType.Ink);
				return;
			}
			Invariant.Assert(false, "Unknown command.");
		}

		// Token: 0x06005E96 RID: 24214 RVA: 0x00290CBC File Offset: 0x0028FCBC
		private void UpdateInkDrawingAttributes()
		{
			if (this.Content == null || this.Content.Type != StickyNoteType.Ink)
			{
				return;
			}
			DrawingAttributes drawingAttributes = new DrawingAttributes();
			SolidColorBrush solidColorBrush = base.Foreground as SolidColorBrush;
			if (solidColorBrush == null)
			{
				throw new ArgumentException(SR.Get("InvalidInkForeground"));
			}
			drawingAttributes.StylusTip = StylusTip.Ellipse;
			drawingAttributes.Width = this.PenWidth;
			drawingAttributes.Height = this.PenWidth;
			drawingAttributes.Color = solidColorBrush.Color;
			((InkCanvas)this.Content.InnerControl).DefaultDrawingAttributes = drawingAttributes;
		}

		// Token: 0x06005E97 RID: 24215 RVA: 0x00290D46 File Offset: 0x0028FD46
		private MenuItem GetInkMenuItem()
		{
			return base.GetTemplateChild("PART_InkMenuItem") as MenuItem;
		}

		// Token: 0x06005E98 RID: 24216 RVA: 0x00290D58 File Offset: 0x0028FD58
		private MenuItem GetSelectMenuItem()
		{
			return base.GetTemplateChild("PART_SelectMenuItem") as MenuItem;
		}

		// Token: 0x06005E99 RID: 24217 RVA: 0x00290D6A File Offset: 0x0028FD6A
		private MenuItem GetEraseMenuItem()
		{
			return base.GetTemplateChild("PART_EraseMenuItem") as MenuItem;
		}

		// Token: 0x06005E9A RID: 24218 RVA: 0x00290D7C File Offset: 0x0028FD7C
		private MenuItem GetCopyMenuItem()
		{
			return base.GetTemplateChild("PART_CopyMenuItem") as MenuItem;
		}

		// Token: 0x06005E9B RID: 24219 RVA: 0x00290D8E File Offset: 0x0028FD8E
		private MenuItem GetPasteMenuItem()
		{
			return base.GetTemplateChild("PART_PasteMenuItem") as MenuItem;
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x00290DA0 File Offset: 0x0028FDA0
		private Separator GetClipboardSeparator()
		{
			return base.GetTemplateChild("PART_ClipboardSeparator") as Separator;
		}

		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x06005E9D RID: 24221 RVA: 0x00290DB2 File Offset: 0x0028FDB2
		private LockHelper InternalLocker
		{
			get
			{
				if (this._lockHelper == null)
				{
					this._lockHelper = new LockHelper();
				}
				return this._lockHelper;
			}
		}

		// Token: 0x04003179 RID: 12665
		public static readonly XmlQualifiedName TextSchemaName = new XmlQualifiedName("TextStickyNote", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x0400317A RID: 12666
		public static readonly XmlQualifiedName InkSchemaName = new XmlQualifiedName("InkStickyNote", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x0400317B RID: 12667
		private PresentationContext _presentationContext;

		// Token: 0x0400317C RID: 12668
		private TranslateTransform _positionTransform = new TranslateTransform(0.0, 0.0);

		// Token: 0x0400317D RID: 12669
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x0400317E RID: 12670
		private SNCAnnotation _sncAnnotation;

		// Token: 0x0400317F RID: 12671
		private double _offsetX;

		// Token: 0x04003180 RID: 12672
		private double _offsetY;

		// Token: 0x04003181 RID: 12673
		private double _deltaX;

		// Token: 0x04003182 RID: 12674
		private double _deltaY;

		// Token: 0x04003183 RID: 12675
		private int _zOrder;

		// Token: 0x04003184 RID: 12676
		private bool _selfMirroring;

		// Token: 0x04003185 RID: 12677
		internal static readonly DependencyPropertyKey AuthorPropertyKey = DependencyProperty.RegisterReadOnly("Author", typeof(string), typeof(StickyNoteControl), new FrameworkPropertyMetadata(string.Empty));

		// Token: 0x04003186 RID: 12678
		public static readonly DependencyProperty AuthorProperty = StickyNoteControl.AuthorPropertyKey.DependencyProperty;

		// Token: 0x04003187 RID: 12679
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(StickyNoteControl), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(StickyNoteControl._OnIsExpandedChanged)));

		// Token: 0x04003188 RID: 12680
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.RegisterAttached("IsActive", typeof(bool), typeof(StickyNoteControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04003189 RID: 12681
		internal static readonly DependencyPropertyKey IsMouseOverAnchorPropertyKey = DependencyProperty.RegisterReadOnly("IsMouseOverAnchor", typeof(bool), typeof(StickyNoteControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x0400318A RID: 12682
		public static readonly DependencyProperty IsMouseOverAnchorProperty = StickyNoteControl.IsMouseOverAnchorPropertyKey.DependencyProperty;

		// Token: 0x0400318B RID: 12683
		public static readonly DependencyProperty CaptionFontFamilyProperty = DependencyProperty.Register("CaptionFontFamily", typeof(FontFamily), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x0400318C RID: 12684
		public static readonly DependencyProperty CaptionFontSizeProperty = DependencyProperty.Register("CaptionFontSize", typeof(double), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x0400318D RID: 12685
		public static readonly DependencyProperty CaptionFontStretchProperty = DependencyProperty.Register("CaptionFontStretch", typeof(FontStretch), typeof(StickyNoteControl), new FrameworkPropertyMetadata(FontStretches.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x0400318E RID: 12686
		public static readonly DependencyProperty CaptionFontStyleProperty = DependencyProperty.Register("CaptionFontStyle", typeof(FontStyle), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x0400318F RID: 12687
		public static readonly DependencyProperty CaptionFontWeightProperty = DependencyProperty.Register("CaptionFontWeight", typeof(FontWeight), typeof(StickyNoteControl), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04003190 RID: 12688
		public static readonly DependencyProperty PenWidthProperty = DependencyProperty.Register("PenWidth", typeof(double), typeof(StickyNoteControl), new FrameworkPropertyMetadata(new DrawingAttributes().Width, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(StickyNoteControl._UpdateInkDrawingAttributes)));

		// Token: 0x04003191 RID: 12689
		private static readonly DependencyPropertyKey StickyNoteTypePropertyKey = DependencyProperty.RegisterReadOnly("StickyNoteType", typeof(StickyNoteType), typeof(StickyNoteControl), new FrameworkPropertyMetadata(StickyNoteType.Text));

		// Token: 0x04003192 RID: 12690
		public static readonly DependencyProperty StickyNoteTypeProperty = StickyNoteControl.StickyNoteTypePropertyKey.DependencyProperty;

		// Token: 0x04003193 RID: 12691
		public static readonly RoutedCommand DeleteNoteCommand = new RoutedCommand("DeleteNote", typeof(StickyNoteControl));

		// Token: 0x04003194 RID: 12692
		public static readonly RoutedCommand InkCommand = new RoutedCommand("Ink", typeof(StickyNoteControl));

		// Token: 0x04003195 RID: 12693
		private static readonly DependencyProperty InkEditingModeProperty = DependencyProperty.Register("InkEditingMode", typeof(InkCanvasEditingMode), typeof(StickyNoteControl), new FrameworkPropertyMetadata(InkCanvasEditingMode.None));

		// Token: 0x04003196 RID: 12694
		private LockHelper _lockHelper;

		// Token: 0x04003197 RID: 12695
		private MarkedHighlightComponent _anchor;

		// Token: 0x04003198 RID: 12696
		private bool _dirty;

		// Token: 0x04003199 RID: 12697
		private StickyNoteType _stickyNoteType;

		// Token: 0x0400319A RID: 12698
		private StickyNoteContentControl _contentControl;

		// Token: 0x0400319B RID: 12699
		private StickyNoteControl.StrokeChangedHandler<PropertyDataChangedEventArgs> _propertyDataChangedHandler;

		// Token: 0x0400319C RID: 12700
		private StickyNoteControl.StrokeChangedHandler<DrawingAttributesReplacedEventArgs> _strokeDrawingAttributesReplacedHandler;

		// Token: 0x0400319D RID: 12701
		private StickyNoteControl.StrokeChangedHandler<EventArgs> _strokePacketDataChangedHandler;

		// Token: 0x02000BBA RID: 3002
		private class InkEditingModeConverter : IValueConverter
		{
			// Token: 0x06008F33 RID: 36659 RVA: 0x003438F4 File Offset: 0x003428F4
			public object Convert(object o, Type type, object parameter, CultureInfo culture)
			{
				InkCanvasEditingMode inkCanvasEditingMode = (InkCanvasEditingMode)parameter;
				if ((InkCanvasEditingMode)o == inkCanvasEditingMode)
				{
					return true;
				}
				return DependencyProperty.UnsetValue;
			}

			// Token: 0x06008F34 RID: 36660 RVA: 0x00109403 File Offset: 0x00108403
			public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
			{
				return null;
			}
		}

		// Token: 0x02000BBB RID: 3003
		private class InkEditingModeIsKeyboardFocusWithin2EditingMode : IMultiValueConverter
		{
			// Token: 0x06008F36 RID: 36662 RVA: 0x00343920 File Offset: 0x00342920
			public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
			{
				InkCanvasEditingMode inkCanvasEditingMode = (InkCanvasEditingMode)values[0];
				if ((bool)values[1])
				{
					return inkCanvasEditingMode;
				}
				return InkCanvasEditingMode.None;
			}

			// Token: 0x06008F37 RID: 36663 RVA: 0x0034394D File Offset: 0x0034294D
			public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
			{
				return new object[]
				{
					value,
					Binding.DoNothing
				};
			}
		}

		// Token: 0x02000BBC RID: 3004
		private class StrokeChangedHandler<TEventArgs>
		{
			// Token: 0x06008F39 RID: 36665 RVA: 0x00343961 File Offset: 0x00342961
			public StrokeChangedHandler(StickyNoteControl snc)
			{
				Invariant.Assert(snc != null);
				this._snc = snc;
			}

			// Token: 0x06008F3A RID: 36666 RVA: 0x00343979 File Offset: 0x00342979
			public void OnStrokeChanged(object sender, TEventArgs t)
			{
				this._snc.UpdateAnnotationWithSNC(XmlToken.Ink);
				this._snc._dirty = true;
			}

			// Token: 0x040049B9 RID: 18873
			private StickyNoteControl _snc;
		}
	}
}
