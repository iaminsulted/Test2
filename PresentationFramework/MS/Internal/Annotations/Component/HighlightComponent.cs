using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;
using MS.Utility;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020002CA RID: 714
	internal class HighlightComponent : Canvas, IAnnotationComponent, IHighlightRange
	{
		// Token: 0x06001AA9 RID: 6825 RVA: 0x00165348 File Offset: 0x00164348
		public HighlightComponent()
		{
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x001653A0 File Offset: 0x001643A0
		public HighlightComponent(int priority, bool highlightContent, XmlQualifiedName type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._priority = priority;
			this._type = type;
			this._highlightContent = highlightContent;
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x00165420 File Offset: 0x00164420
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

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001AAC RID: 6828 RVA: 0x00165449 File Offset: 0x00164449
		// (set) Token: 0x06001AAD RID: 6829 RVA: 0x00165451 File Offset: 0x00164451
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

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001AAE RID: 6830 RVA: 0x0016545A File Offset: 0x0016445A
		// (set) Token: 0x06001AAF RID: 6831 RVA: 0x000F6B2C File Offset: 0x000F5B2C
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

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x0016545D File Offset: 0x0016445D
		public static XmlQualifiedName TypeName
		{
			get
			{
				return HighlightComponent._name;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x00165464 File Offset: 0x00164464
		// (set) Token: 0x06001AB2 RID: 6834 RVA: 0x0016546C File Offset: 0x0016446C
		public Color DefaultBackground
		{
			get
			{
				return this._defaultBackroundColor;
			}
			set
			{
				this._defaultBackroundColor = value;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x00165475 File Offset: 0x00164475
		// (set) Token: 0x06001AB4 RID: 6836 RVA: 0x0016547D File Offset: 0x0016447D
		public Color DefaultActiveBackground
		{
			get
			{
				return this._defaultActiveBackgroundColor;
			}
			set
			{
				this._defaultActiveBackgroundColor = value;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (set) Token: 0x06001AB5 RID: 6837 RVA: 0x00165486 File Offset: 0x00164486
		public Brush HighlightBrush
		{
			set
			{
				base.SetValue(HighlightComponent.HighlightBrushProperty, value);
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001AB6 RID: 6838 RVA: 0x00165494 File Offset: 0x00164494
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

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x001654B0 File Offset: 0x001644B0
		// (set) Token: 0x06001AB8 RID: 6840 RVA: 0x001654B8 File Offset: 0x001644B8
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
					this.InvalidateChildren();
				}
			}
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x001136C4 File Offset: 0x001126C4
		public GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			return transform;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x001654CC File Offset: 0x001644CC
		public void AddAttachedAnnotation(IAttachedAnnotation attachedAnnotation)
		{
			if (this._attachedAnnotation != null)
			{
				throw new ArgumentException(SR.Get("MoreThanOneAttachedAnnotation"));
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedHighlightBegin);
			ITextContainer textContainer = this.CheckInputData(attachedAnnotation);
			TextAnchor range = attachedAnnotation.AttachedAnchor as TextAnchor;
			this.GetColors(attachedAnnotation.Annotation, out this._background, out this._selectedBackground);
			this._range = range;
			Invariant.Assert(textContainer.Highlights != null, "textContainer.Highlights is null");
			AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
			if (annotationHighlightLayer == null)
			{
				annotationHighlightLayer = new AnnotationHighlightLayer();
				textContainer.Highlights.AddLayer(annotationHighlightLayer);
			}
			this._attachedAnnotation = attachedAnnotation;
			this._attachedAnnotation.Annotation.CargoChanged += this.OnAnnotationUpdated;
			annotationHighlightLayer.AddRange(this);
			this.HighlightBrush = new SolidColorBrush(this._background);
			base.IsHitTestVisible = false;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.AddAttachedHighlightEnd);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x001655C4 File Offset: 0x001645C4
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
			Invariant.Assert(this._range != null, "null highlight range");
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedHighlightBegin);
			ITextContainer textContainer = this.CheckInputData(attachedAnnotation);
			Invariant.Assert(textContainer.Highlights != null, "textContainer.Highlights is null");
			AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
			Invariant.Assert(annotationHighlightLayer != null, "AnnotationHighlightLayer is not initialized");
			this._attachedAnnotation.Annotation.CargoChanged -= this.OnAnnotationUpdated;
			annotationHighlightLayer.RemoveRange(this);
			this._attachedAnnotation = null;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.RemoveAttachedHighlightEnd);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00165692 File Offset: 0x00164692
		public void ModifyAttachedAnnotation(IAttachedAnnotation attachedAnnotation, object previousAttachedAnchor, AttachmentLevel previousAttachmentLevel)
		{
			throw new NotSupportedException(SR.Get("NotSupported"));
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x001656A4 File Offset: 0x001646A4
		public void Activate(bool active)
		{
			if (this._active == active)
			{
				return;
			}
			if (this._attachedAnnotation == null)
			{
				throw new InvalidOperationException(SR.Get("NoAttachedAnnotationToModify"));
			}
			TextAnchor textAnchor = this._attachedAnnotation.AttachedAnchor as TextAnchor;
			Invariant.Assert(textAnchor != null, "AttachedAnchor is not a text anchor");
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			Invariant.Assert(textContainer != null, "TextAnchor does not belong to a TextContainer");
			AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
			Invariant.Assert(annotationHighlightLayer != null, "AnnotationHighlightLayer is not initialized");
			annotationHighlightLayer.ActivateRange(this, active);
			this._active = active;
			if (active)
			{
				this.HighlightBrush = new SolidColorBrush(this._selectedBackground);
				return;
			}
			this.HighlightBrush = new SolidColorBrush(this._background);
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00165764 File Offset: 0x00164764
		void IHighlightRange.AddChild(Shape child)
		{
			base.Children.Add(child);
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x00165773 File Offset: 0x00164773
		void IHighlightRange.RemoveChild(Shape child)
		{
			base.Children.Remove(child);
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x00165781 File Offset: 0x00164781
		Color IHighlightRange.Background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x00165789 File Offset: 0x00164789
		Color IHighlightRange.SelectedBackground
		{
			get
			{
				return this._selectedBackground;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x00165791 File Offset: 0x00164791
		TextAnchor IHighlightRange.Range
		{
			get
			{
				return this._range;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x00165799 File Offset: 0x00164799
		int IHighlightRange.Priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x001657A1 File Offset: 0x001647A1
		bool IHighlightRange.HighlightContent
		{
			get
			{
				return this._highlightContent;
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x001657AC File Offset: 0x001647AC
		internal bool IsSelected(ITextRange selection)
		{
			if (selection == null)
			{
				throw new ArgumentNullException("selection");
			}
			Invariant.Assert(this._attachedAnnotation != null, "No _attachedAnnotation");
			TextAnchor textAnchor = this._attachedAnnotation.FullyAttachedAnchor as TextAnchor;
			return textAnchor != null && textAnchor.IsOverlapping(selection.TextSegments);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x001657FC File Offset: 0x001647FC
		internal static void GetCargoColors(Annotation annot, ref Color? backgroundColor, ref Color? activeBackgroundColor)
		{
			Invariant.Assert(annot != null, "annotation is null");
			ICollection<AnnotationResource> cargos = annot.Cargos;
			if (cargos != null)
			{
				foreach (AnnotationResource annotationResource in cargos)
				{
					if (annotationResource.Name == "Highlight")
					{
						foreach (object obj in ((IEnumerable)annotationResource.Contents))
						{
							XmlElement xmlElement = (XmlElement)obj;
							if (xmlElement.LocalName == "Colors" && xmlElement.NamespaceURI == "http://schemas.microsoft.com/windows/annotations/2003/11/base")
							{
								if (xmlElement.Attributes["Background"] != null)
								{
									backgroundColor = new Color?(HighlightComponent.GetColor(xmlElement.Attributes["Background"].Value));
								}
								if (xmlElement.Attributes["ActiveBackground"] != null)
								{
									activeBackgroundColor = new Color?(HighlightComponent.GetColor(xmlElement.Attributes["ActiveBackground"].Value));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x00165974 File Offset: 0x00164974
		private ITextContainer CheckInputData(IAttachedAnnotation attachedAnnotation)
		{
			if (attachedAnnotation == null)
			{
				throw new ArgumentNullException("attachedAnnotation");
			}
			TextAnchor textAnchor = attachedAnnotation.AttachedAnchor as TextAnchor;
			if (textAnchor == null)
			{
				throw new ArgumentException(SR.Get("InvalidAttachedAnchor"), "attachedAnnotation");
			}
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			Invariant.Assert(textContainer != null, "TextAnchor does not belong to a TextContainer");
			if (attachedAnnotation.Annotation == null)
			{
				throw new ArgumentException(SR.Get("AnnotationIsNull"), "attachedAnnotation");
			}
			if (!this._type.Equals(attachedAnnotation.Annotation.AnnotationType))
			{
				throw new ArgumentException(SR.Get("NotHighlightAnnotationType", new object[]
				{
					attachedAnnotation.Annotation.AnnotationType.ToString()
				}), "attachedAnnotation");
			}
			return textContainer;
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00165A2F File Offset: 0x00164A2F
		private static Color GetColor(string color)
		{
			return (Color)ColorConverter.ConvertFromString(color);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x00165A3C File Offset: 0x00164A3C
		private void GetColors(Annotation annot, out Color backgroundColor, out Color activeBackgroundColor)
		{
			Color? color = new Color?(this._defaultBackroundColor);
			Color? color2 = new Color?(this._defaultActiveBackgroundColor);
			HighlightComponent.GetCargoColors(annot, ref color, ref color2);
			backgroundColor = color.Value;
			activeBackgroundColor = color2.Value;
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00165A88 File Offset: 0x00164A88
		private void OnAnnotationUpdated(object sender, AnnotationResourceChangedEventArgs args)
		{
			Invariant.Assert(this._attachedAnnotation != null && this._attachedAnnotation.Annotation == args.Annotation, "_attachedAnnotation is different than the input one");
			Invariant.Assert(this._range != null, "The highlight range is null");
			TextAnchor textAnchor = this._attachedAnnotation.AttachedAnchor as TextAnchor;
			Invariant.Assert(textAnchor != null, "wrong anchor type of the saved attached annotation");
			ITextContainer textContainer = textAnchor.Start.TextContainer;
			Invariant.Assert(textContainer != null, "TextAnchor does not belong to a TextContainer");
			Color color;
			Color color2;
			this.GetColors(args.Annotation, out color, out color2);
			if (!this._background.Equals(color) || !this._selectedBackground.Equals(color2))
			{
				Invariant.Assert(textContainer.Highlights != null, "textContainer.Highlights is null");
				AnnotationHighlightLayer annotationHighlightLayer = textContainer.Highlights.GetLayer(typeof(HighlightComponent)) as AnnotationHighlightLayer;
				if (annotationHighlightLayer == null)
				{
					throw new InvalidDataException(SR.Get("MissingAnnotationHighlightLayer"));
				}
				this._background = color;
				this._selectedBackground = color2;
				annotationHighlightLayer.ModifiedRange(this);
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00165B88 File Offset: 0x00164B88
		private void InvalidateChildren()
		{
			foreach (object obj in base.Children)
			{
				Shape shape = ((Visual)obj) as Shape;
				Invariant.Assert(shape != null, "HighlightComponent has non-Shape children.");
				shape.InvalidateMeasure();
			}
			this.IsDirty = false;
		}

		// Token: 0x04000DC2 RID: 3522
		public static DependencyProperty HighlightBrushProperty = DependencyProperty.Register("HighlightBrushProperty", typeof(Brush), typeof(HighlightComponent));

		// Token: 0x04000DC3 RID: 3523
		public const string HighlightResourceName = "Highlight";

		// Token: 0x04000DC4 RID: 3524
		public const string ColorsContentName = "Colors";

		// Token: 0x04000DC5 RID: 3525
		public const string BackgroundAttributeName = "Background";

		// Token: 0x04000DC6 RID: 3526
		public const string ActiveBackgroundAttributeName = "ActiveBackground";

		// Token: 0x04000DC7 RID: 3527
		private Color _background;

		// Token: 0x04000DC8 RID: 3528
		private Color _selectedBackground;

		// Token: 0x04000DC9 RID: 3529
		private TextAnchor _range;

		// Token: 0x04000DCA RID: 3530
		private IAttachedAnnotation _attachedAnnotation;

		// Token: 0x04000DCB RID: 3531
		private PresentationContext _presentationContext;

		// Token: 0x04000DCC RID: 3532
		private static readonly XmlQualifiedName _name = new XmlQualifiedName("Highlight", "http://schemas.microsoft.com/windows/annotations/2003/11/base");

		// Token: 0x04000DCD RID: 3533
		private XmlQualifiedName _type = HighlightComponent._name;

		// Token: 0x04000DCE RID: 3534
		private int _priority;

		// Token: 0x04000DCF RID: 3535
		private bool _highlightContent = true;

		// Token: 0x04000DD0 RID: 3536
		private bool _active;

		// Token: 0x04000DD1 RID: 3537
		private bool _isDirty = true;

		// Token: 0x04000DD2 RID: 3538
		private Color _defaultBackroundColor = (Color)ColorConverter.ConvertFromString("#33FFFF00");

		// Token: 0x04000DD3 RID: 3539
		private Color _defaultActiveBackgroundColor = (Color)ColorConverter.ConvertFromString("#339ACD32");
	}
}
