using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.FontCache;
using MS.Internal.PtsHost;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x0200061B RID: 1563
	[Localizability(LocalizationCategory.Inherit, Readability = Readability.Unreadable)]
	[ContentProperty("Blocks")]
	public class FlowDocument : FrameworkContentElement, IDocumentPaginatorSource, IServiceProvider, IAddChild
	{
		// Token: 0x06004C80 RID: 19584 RVA: 0x0023C90C File Offset: 0x0023B90C
		static FlowDocument()
		{
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(FlowDocument.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(FlowDocument._typeofThis, new FrameworkPropertyMetadata(propertyChangedCallback));
			}
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(FlowDocument._typeofThis, new FrameworkPropertyMetadata(FlowDocument._typeofThis));
			ContentElement.FocusableProperty.OverrideMetadata(FlowDocument._typeofThis, new FrameworkPropertyMetadata(true));
			ControlsTraceLogger.AddControl(TelemetryControls.FlowDocument);
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x0023CE1D File Offset: 0x0023BE1D
		public FlowDocument()
		{
			this.Initialize(null);
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x0023CE3F File Offset: 0x0023BE3F
		public FlowDocument(Block block)
		{
			this.Initialize(null);
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			this.Blocks.Add(block);
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x0023CE7B File Offset: 0x0023BE7B
		internal FlowDocument(TextContainer textContainer)
		{
			this.Initialize(textContainer);
		}

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x06004C84 RID: 19588 RVA: 0x0022BE7C File Offset: 0x0022AE7C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x06004C85 RID: 19589 RVA: 0x0023CE9D File Offset: 0x0023BE9D
		internal TextRange TextRange
		{
			get
			{
				return new TextRange(this.ContentStart, this.ContentEnd);
			}
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x06004C86 RID: 19590 RVA: 0x0023CEB0 File Offset: 0x0023BEB0
		public TextPointer ContentStart
		{
			get
			{
				return this._structuralCache.TextContainer.Start;
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x06004C87 RID: 19591 RVA: 0x0023CEC2 File Offset: 0x0023BEC2
		public TextPointer ContentEnd
		{
			get
			{
				return this._structuralCache.TextContainer.End;
			}
		}

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x06004C88 RID: 19592 RVA: 0x0023CED4 File Offset: 0x0023BED4
		// (set) Token: 0x06004C89 RID: 19593 RVA: 0x0023CEE6 File Offset: 0x0023BEE6
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(FlowDocument.FontFamilyProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontFamilyProperty, value);
			}
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x06004C8A RID: 19594 RVA: 0x0023CEF4 File Offset: 0x0023BEF4
		// (set) Token: 0x06004C8B RID: 19595 RVA: 0x0023CF06 File Offset: 0x0023BF06
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(FlowDocument.FontStyleProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontStyleProperty, value);
			}
		}

		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x06004C8C RID: 19596 RVA: 0x0023CF19 File Offset: 0x0023BF19
		// (set) Token: 0x06004C8D RID: 19597 RVA: 0x0023CF2B File Offset: 0x0023BF2B
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(FlowDocument.FontWeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontWeightProperty, value);
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x06004C8E RID: 19598 RVA: 0x0023CF3E File Offset: 0x0023BF3E
		// (set) Token: 0x06004C8F RID: 19599 RVA: 0x0023CF50 File Offset: 0x0023BF50
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(FlowDocument.FontStretchProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontStretchProperty, value);
			}
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x0023CF63 File Offset: 0x0023BF63
		// (set) Token: 0x06004C91 RID: 19601 RVA: 0x0023CF75 File Offset: 0x0023BF75
		[Localizability(LocalizationCategory.None)]
		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(FlowDocument.FontSizeProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FontSizeProperty, value);
			}
		}

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x06004C92 RID: 19602 RVA: 0x0023CF88 File Offset: 0x0023BF88
		// (set) Token: 0x06004C93 RID: 19603 RVA: 0x0023CF9A File Offset: 0x0023BF9A
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(FlowDocument.ForegroundProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ForegroundProperty, value);
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x0023CFA8 File Offset: 0x0023BFA8
		// (set) Token: 0x06004C95 RID: 19605 RVA: 0x0023CFBA File Offset: 0x0023BFBA
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(FlowDocument.BackgroundProperty);
			}
			set
			{
				base.SetValue(FlowDocument.BackgroundProperty, value);
			}
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x0023CFC8 File Offset: 0x0023BFC8
		// (set) Token: 0x06004C97 RID: 19607 RVA: 0x0023CFDA File Offset: 0x0023BFDA
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(FlowDocument.TextEffectsProperty);
			}
			set
			{
				base.SetValue(FlowDocument.TextEffectsProperty, value);
			}
		}

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x06004C98 RID: 19608 RVA: 0x0023CFE8 File Offset: 0x0023BFE8
		// (set) Token: 0x06004C99 RID: 19609 RVA: 0x0023CFFA File Offset: 0x0023BFFA
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(FlowDocument.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(FlowDocument.TextAlignmentProperty, value);
			}
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x0023D00D File Offset: 0x0023C00D
		// (set) Token: 0x06004C9B RID: 19611 RVA: 0x0023D01F File Offset: 0x0023C01F
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(FlowDocument.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(FlowDocument.FlowDirectionProperty, value);
			}
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x06004C9C RID: 19612 RVA: 0x0023D032 File Offset: 0x0023C032
		// (set) Token: 0x06004C9D RID: 19613 RVA: 0x0023D044 File Offset: 0x0023C044
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.LineHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.LineHeightProperty, value);
			}
		}

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x0023D057 File Offset: 0x0023C057
		// (set) Token: 0x06004C9F RID: 19615 RVA: 0x0023D069 File Offset: 0x0023C069
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(FlowDocument.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(FlowDocument.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x0023D07C File Offset: 0x0023C07C
		// (set) Token: 0x06004CA1 RID: 19617 RVA: 0x0023D08E File Offset: 0x0023C08E
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double ColumnWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.ColumnWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnWidthProperty, value);
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x0023D0A1 File Offset: 0x0023C0A1
		// (set) Token: 0x06004CA3 RID: 19619 RVA: 0x0023D0B3 File Offset: 0x0023C0B3
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double ColumnGap
		{
			get
			{
				return (double)base.GetValue(FlowDocument.ColumnGapProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnGapProperty, value);
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x0023D0C6 File Offset: 0x0023C0C6
		// (set) Token: 0x06004CA5 RID: 19621 RVA: 0x0023D0D8 File Offset: 0x0023C0D8
		public bool IsColumnWidthFlexible
		{
			get
			{
				return (bool)base.GetValue(FlowDocument.IsColumnWidthFlexibleProperty);
			}
			set
			{
				base.SetValue(FlowDocument.IsColumnWidthFlexibleProperty, value);
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x0023D0E6 File Offset: 0x0023C0E6
		// (set) Token: 0x06004CA7 RID: 19623 RVA: 0x0023D0F8 File Offset: 0x0023C0F8
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double ColumnRuleWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.ColumnRuleWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnRuleWidthProperty, value);
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06004CA8 RID: 19624 RVA: 0x0023D10B File Offset: 0x0023C10B
		// (set) Token: 0x06004CA9 RID: 19625 RVA: 0x0023D11D File Offset: 0x0023C11D
		public Brush ColumnRuleBrush
		{
			get
			{
				return (Brush)base.GetValue(FlowDocument.ColumnRuleBrushProperty);
			}
			set
			{
				base.SetValue(FlowDocument.ColumnRuleBrushProperty, value);
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06004CAA RID: 19626 RVA: 0x0023D12B File Offset: 0x0023C12B
		// (set) Token: 0x06004CAB RID: 19627 RVA: 0x0023D13D File Offset: 0x0023C13D
		public bool IsOptimalParagraphEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocument.IsOptimalParagraphEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocument.IsOptimalParagraphEnabledProperty, value);
			}
		}

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x06004CAC RID: 19628 RVA: 0x0023D14B File Offset: 0x0023C14B
		// (set) Token: 0x06004CAD RID: 19629 RVA: 0x0023D15D File Offset: 0x0023C15D
		[TypeConverter(typeof(LengthConverter))]
		public double PageWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.PageWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.PageWidthProperty, value);
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x06004CAE RID: 19630 RVA: 0x0023D170 File Offset: 0x0023C170
		// (set) Token: 0x06004CAF RID: 19631 RVA: 0x0023D182 File Offset: 0x0023C182
		[TypeConverter(typeof(LengthConverter))]
		public double MinPageWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MinPageWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MinPageWidthProperty, value);
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06004CB0 RID: 19632 RVA: 0x0023D195 File Offset: 0x0023C195
		// (set) Token: 0x06004CB1 RID: 19633 RVA: 0x0023D1A7 File Offset: 0x0023C1A7
		[TypeConverter(typeof(LengthConverter))]
		public double MaxPageWidth
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MaxPageWidthProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MaxPageWidthProperty, value);
			}
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x0023D1BA File Offset: 0x0023C1BA
		// (set) Token: 0x06004CB3 RID: 19635 RVA: 0x0023D1CC File Offset: 0x0023C1CC
		[TypeConverter(typeof(LengthConverter))]
		public double PageHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.PageHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.PageHeightProperty, value);
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x0023D1DF File Offset: 0x0023C1DF
		// (set) Token: 0x06004CB5 RID: 19637 RVA: 0x0023D1F1 File Offset: 0x0023C1F1
		[TypeConverter(typeof(LengthConverter))]
		public double MinPageHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MinPageHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MinPageHeightProperty, value);
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06004CB6 RID: 19638 RVA: 0x0023D204 File Offset: 0x0023C204
		// (set) Token: 0x06004CB7 RID: 19639 RVA: 0x0023D216 File Offset: 0x0023C216
		[TypeConverter(typeof(LengthConverter))]
		public double MaxPageHeight
		{
			get
			{
				return (double)base.GetValue(FlowDocument.MaxPageHeightProperty);
			}
			set
			{
				base.SetValue(FlowDocument.MaxPageHeightProperty, value);
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06004CB8 RID: 19640 RVA: 0x0023D229 File Offset: 0x0023C229
		// (set) Token: 0x06004CB9 RID: 19641 RVA: 0x0023D23B File Offset: 0x0023C23B
		public Thickness PagePadding
		{
			get
			{
				return (Thickness)base.GetValue(FlowDocument.PagePaddingProperty);
			}
			set
			{
				base.SetValue(FlowDocument.PagePaddingProperty, value);
			}
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x0023D24E File Offset: 0x0023C24E
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x06004CBB RID: 19643 RVA: 0x0023D256 File Offset: 0x0023C256
		// (set) Token: 0x06004CBC RID: 19644 RVA: 0x0023D268 File Offset: 0x0023C268
		public bool IsHyphenationEnabled
		{
			get
			{
				return (bool)base.GetValue(FlowDocument.IsHyphenationEnabledProperty);
			}
			set
			{
				base.SetValue(FlowDocument.IsHyphenationEnabledProperty, value);
			}
		}

		// Token: 0x06004CBD RID: 19645 RVA: 0x0023D278 File Offset: 0x0023C278
		public void SetDpi(DpiScale dpiInfo)
		{
			if (dpiInfo.PixelsPerDip != this._pixelsPerDip)
			{
				this._pixelsPerDip = dpiInfo.PixelsPerDip;
				if (this.StructuralCache.HasPtsContext())
				{
					this.StructuralCache.TextFormatterHost.PixelsPerDip = this._pixelsPerDip;
				}
				IFlowDocumentFormatter formatter = this._formatter;
				if (formatter == null)
				{
					return;
				}
				formatter.OnContentInvalidated(true);
			}
		}

		// Token: 0x06004CBE RID: 19646 RVA: 0x0023D2D8 File Offset: 0x0023C2D8
		protected sealed override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if ((e.IsAValueChange || e.IsASubPropertyChange) && this._structuralCache != null && this._structuralCache.IsFormattedOnce)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null)
				{
					bool flag = frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || flag || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange)
					{
						if (this._structuralCache.IsFormattingInProgress)
						{
							this._structuralCache.OnInvalidOperationDetected();
							throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
						}
						this._structuralCache.InvalidateFormatCache(!flag);
						if (this._formatter != null)
						{
							this._formatter.OnContentInvalidated(!flag);
						}
					}
				}
			}
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0022ECE4 File Offset: 0x0022DCE4
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentAutomationPeer(this);
		}

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x06004CC0 RID: 19648 RVA: 0x0023D3C3 File Offset: 0x0023C3C3
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new RangeContentEnumerator(this._structuralCache.TextContainer.Start, this._structuralCache.TextContainer.End);
			}
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x0023D3EC File Offset: 0x0023C3EC
		protected override bool IsEnabledCore
		{
			get
			{
				if (!base.IsEnabledCore)
				{
					return false;
				}
				RichTextBox richTextBox = base.Parent as RichTextBox;
				return richTextBox == null || richTextBox.IsDocumentEnabled;
			}
		}

		// Token: 0x06004CC2 RID: 19650 RVA: 0x0023D41C File Offset: 0x0023C41C
		internal ContentPosition GetObjectPosition(object element)
		{
			ITextPointer textPointer = null;
			if (element == this)
			{
				textPointer = this.ContentStart;
			}
			else if (element is TextElement)
			{
				textPointer = ((TextElement)element).ContentStart;
			}
			else if (element is FrameworkElement)
			{
				DependencyObject dependencyObject = null;
				while (element is FrameworkElement)
				{
					dependencyObject = LogicalTreeHelper.GetParent((DependencyObject)element);
					if (dependencyObject == null)
					{
						dependencyObject = VisualTreeHelper.GetParent((Visual)element);
					}
					if (!(dependencyObject is FrameworkElement))
					{
						break;
					}
					element = dependencyObject;
				}
				if (dependencyObject is BlockUIContainer || dependencyObject is InlineUIContainer)
				{
					textPointer = TextContainerHelper.GetTextPointerForEmbeddedObject((FrameworkElement)element);
				}
			}
			if (textPointer != null && textPointer.TextContainer != this._structuralCache.TextContainer)
			{
				textPointer = null;
			}
			TextPointer textPointer2 = textPointer as TextPointer;
			if (textPointer2 == null)
			{
				return ContentPosition.Missing;
			}
			return textPointer2;
		}

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0023D4D0 File Offset: 0x0023C4D0
		internal void OnChildDesiredSizeChanged(UIElement child)
		{
			if (this._structuralCache != null && this._structuralCache.IsFormattedOnce && !this._structuralCache.ForceReformat)
			{
				if (this._structuralCache.IsFormattingInProgress)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnChildDesiredSizeChangedAsync), child);
					return;
				}
				int cpfromEmbeddedObject = TextContainerHelper.GetCPFromEmbeddedObject(child, ElementEdge.BeforeStart);
				if (cpfromEmbeddedObject < 0)
				{
					return;
				}
				TextPointer textPointer = new TextPointer(this._structuralCache.TextContainer.Start);
				textPointer.MoveByOffset(cpfromEmbeddedObject);
				TextPointer textPointer2 = new TextPointer(textPointer);
				textPointer2.MoveByOffset(TextContainerHelper.EmbeddedObjectLength);
				DirtyTextRange dtr = new DirtyTextRange(cpfromEmbeddedObject, TextContainerHelper.EmbeddedObjectLength, TextContainerHelper.EmbeddedObjectLength, false);
				this._structuralCache.AddDirtyTextRange(dtr);
				if (this._formatter != null)
				{
					this._formatter.OnContentInvalidated(true, textPointer, textPointer2);
				}
			}
		}

		// Token: 0x06004CC4 RID: 19652 RVA: 0x0023D5A4 File Offset: 0x0023C5A4
		internal void InitializeForFirstFormatting()
		{
			this._structuralCache.TextContainer.Changing += this.OnTextContainerChanging;
			this._structuralCache.TextContainer.Change += this.OnTextContainerChange;
			this._structuralCache.TextContainer.Highlights.Changed += this.OnHighlightChanged;
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x0023D60C File Offset: 0x0023C60C
		internal void Uninitialize()
		{
			this._structuralCache.TextContainer.Changing -= this.OnTextContainerChanging;
			this._structuralCache.TextContainer.Change -= this.OnTextContainerChange;
			this._structuralCache.TextContainer.Highlights.Changed -= this.OnHighlightChanged;
			this._structuralCache.IsFormattedOnce = false;
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0023D680 File Offset: 0x0023C680
		internal Thickness ComputePageMargin()
		{
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this);
			Thickness pagePadding = this.PagePadding;
			if (DoubleUtil.IsNaN(pagePadding.Left))
			{
				pagePadding.Left = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Top))
			{
				pagePadding.Top = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Right))
			{
				pagePadding.Right = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Bottom))
			{
				pagePadding.Bottom = lineHeightValue;
			}
			return pagePadding;
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0023D6F4 File Offset: 0x0023C6F4
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			base.OnNewParent(newParent);
			if (newParent is RichTextBox || parent is RichTextBox)
			{
				base.CoerceValue(ContentElement.IsEnabledProperty);
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06004CC8 RID: 19656 RVA: 0x0023D72C File Offset: 0x0023C72C
		internal FlowDocumentFormatter BottomlessFormatter
		{
			get
			{
				if (this._formatter != null && !(this._formatter is FlowDocumentFormatter))
				{
					this._formatter.Suspend();
					this._formatter = null;
				}
				if (this._formatter == null)
				{
					this._formatter = new FlowDocumentFormatter(this);
				}
				return (FlowDocumentFormatter)this._formatter;
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06004CC9 RID: 19657 RVA: 0x0023D77F File Offset: 0x0023C77F
		internal StructuralCache StructuralCache
		{
			get
			{
				return this._structuralCache;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06004CCA RID: 19658 RVA: 0x0023D787 File Offset: 0x0023C787
		internal TypographyProperties TypographyPropertiesGroup
		{
			get
			{
				if (this._typographyPropertiesGroup == null)
				{
					this._typographyPropertiesGroup = TextElement.GetTypographyProperties(this);
				}
				return this._typographyPropertiesGroup;
			}
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06004CCB RID: 19659 RVA: 0x0023D7A9 File Offset: 0x0023C7A9
		// (set) Token: 0x06004CCC RID: 19660 RVA: 0x0023D7B1 File Offset: 0x0023C7B1
		internal TextWrapping TextWrapping
		{
			get
			{
				return this._textWrapping;
			}
			set
			{
				this._textWrapping = value;
			}
		}

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06004CCD RID: 19661 RVA: 0x0023D7BA File Offset: 0x0023C7BA
		internal IFlowDocumentFormatter Formatter
		{
			get
			{
				return this._formatter;
			}
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06004CCE RID: 19662 RVA: 0x0023D7C2 File Offset: 0x0023C7C2
		internal bool IsLayoutDataValid
		{
			get
			{
				return this._formatter != null && this._formatter.IsLayoutDataValid;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06004CCF RID: 19663 RVA: 0x0023D7D9 File Offset: 0x0023C7D9
		internal TextContainer TextContainer
		{
			get
			{
				return this._structuralCache.TextContainer;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x0023D7E6 File Offset: 0x0023C7E6
		// (set) Token: 0x06004CD1 RID: 19665 RVA: 0x0023D7EE File Offset: 0x0023C7EE
		internal double PixelsPerDip
		{
			get
			{
				return this._pixelsPerDip;
			}
			set
			{
				this._pixelsPerDip = value;
			}
		}

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06004CD2 RID: 19666 RVA: 0x0023D7F8 File Offset: 0x0023C7F8
		// (remove) Token: 0x06004CD3 RID: 19667 RVA: 0x0023D830 File Offset: 0x0023C830
		internal event EventHandler PageSizeChanged;

		// Token: 0x06004CD4 RID: 19668 RVA: 0x0023D865 File Offset: 0x0023C865
		private static void OnTypographyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FlowDocument)d)._typographyPropertiesGroup = null;
		}

		// Token: 0x06004CD5 RID: 19669 RVA: 0x0023D873 File Offset: 0x0023C873
		private object OnChildDesiredSizeChangedAsync(object arg)
		{
			this.OnChildDesiredSizeChanged(arg as UIElement);
			return null;
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x0023D882 File Offset: 0x0023C882
		private void Initialize(TextContainer textContainer)
		{
			if (textContainer == null)
			{
				textContainer = new TextContainer(this, false);
			}
			this._structuralCache = new StructuralCache(this, textContainer);
			if (this._formatter != null)
			{
				this._formatter.Suspend();
				this._formatter = null;
			}
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x0023D8B8 File Offset: 0x0023C8B8
		private static void OnPageMetricsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			if (flowDocument._structuralCache != null && flowDocument._structuralCache.IsFormattedOnce)
			{
				if (flowDocument._formatter != null)
				{
					flowDocument._formatter.OnContentInvalidated(true);
				}
				if (flowDocument.PageSizeChanged != null)
				{
					flowDocument.PageSizeChanged(flowDocument, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06004CD8 RID: 19672 RVA: 0x0023D90E File Offset: 0x0023C90E
		private static void OnMinPageWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.MaxPageWidthProperty);
			d.CoerceValue(FlowDocument.PageWidthProperty);
		}

		// Token: 0x06004CD9 RID: 19673 RVA: 0x0023D926 File Offset: 0x0023C926
		private static void OnMinPageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.MaxPageHeightProperty);
			d.CoerceValue(FlowDocument.PageHeightProperty);
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x0023D93E File Offset: 0x0023C93E
		private static void OnMaxPageWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.PageWidthProperty);
		}

		// Token: 0x06004CDB RID: 19675 RVA: 0x0023D94B File Offset: 0x0023C94B
		private static void OnMaxPageHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(FlowDocument.PageHeightProperty);
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x0023D958 File Offset: 0x0023C958
		private static object CoerceMaxPageWidth(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			double minPageWidth = flowDocument.MinPageWidth;
			if (num < minPageWidth)
			{
				return minPageWidth;
			}
			return value;
		}

		// Token: 0x06004CDD RID: 19677 RVA: 0x0023D984 File Offset: 0x0023C984
		private static object CoerceMaxPageHeight(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			double minPageHeight = flowDocument.MinPageHeight;
			if (num < minPageHeight)
			{
				return minPageHeight;
			}
			return value;
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x0023D9B0 File Offset: 0x0023C9B0
		private static object CoercePageWidth(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			if (!DoubleUtil.IsNaN(num))
			{
				double maxPageWidth = flowDocument.MaxPageWidth;
				if (num > maxPageWidth)
				{
					num = maxPageWidth;
				}
				double minPageWidth = flowDocument.MinPageWidth;
				if (num < minPageWidth)
				{
				}
			}
			return value;
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x0023D9F0 File Offset: 0x0023C9F0
		private static object CoercePageHeight(DependencyObject d, object value)
		{
			FlowDocument flowDocument = (FlowDocument)d;
			double num = (double)value;
			if (!DoubleUtil.IsNaN(num))
			{
				double maxPageHeight = flowDocument.MaxPageHeight;
				if (num > maxPageHeight)
				{
					num = maxPageHeight;
				}
				double minPageHeight = flowDocument.MinPageHeight;
				if (num < minPageHeight)
				{
				}
			}
			return value;
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x0023DA30 File Offset: 0x0023CA30
		private void OnHighlightChanged(object sender, HighlightChangedEventArgs args)
		{
			Invariant.Assert(args != null);
			Invariant.Assert(args.Ranges != null);
			Invariant.Assert(this._structuralCache != null && this._structuralCache.IsFormattedOnce, "Unexpected Highlights.Changed callback before first format!");
			if (this._structuralCache.IsFormattingInProgress)
			{
				this._structuralCache.OnInvalidOperationDetected();
				throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
			}
			if (args.OwnerType != typeof(SpellerHighlightLayer))
			{
				return;
			}
			if (args.Ranges.Count > 0)
			{
				if (this._formatter == null || !(this._formatter is FlowDocumentFormatter))
				{
					this._structuralCache.InvalidateFormatCache(false);
				}
				if (this._formatter != null)
				{
					for (int i = 0; i < args.Ranges.Count; i++)
					{
						TextSegment textSegment = (TextSegment)args.Ranges[i];
						this._formatter.OnContentInvalidated(false, textSegment.Start, textSegment.End);
						if (this._formatter is FlowDocumentFormatter)
						{
							DirtyTextRange dtr = new DirtyTextRange(textSegment.Start.Offset, textSegment.Start.GetOffsetToPosition(textSegment.End), textSegment.Start.GetOffsetToPosition(textSegment.End), false);
							this._structuralCache.AddDirtyTextRange(dtr);
						}
					}
				}
			}
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x0023DB8C File Offset: 0x0023CB8C
		private void OnTextContainerChanging(object sender, EventArgs args)
		{
			Invariant.Assert(sender == this._structuralCache.TextContainer, "Received text change for foreign TextContainer.");
			Invariant.Assert(this._structuralCache != null && this._structuralCache.IsFormattedOnce, "Unexpected TextContainer.Changing callback before first format!");
			if (this._structuralCache.IsFormattingInProgress)
			{
				this._structuralCache.OnInvalidOperationDetected();
				throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
			}
			this._structuralCache.IsContentChangeInProgress = true;
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x0023DC08 File Offset: 0x0023CC08
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			Invariant.Assert(args != null);
			Invariant.Assert(sender == this._structuralCache.TextContainer);
			Invariant.Assert(this._structuralCache != null && this._structuralCache.IsFormattedOnce, "Unexpected TextContainer.Change callback before first format!");
			if (args.Count == 0)
			{
				return;
			}
			try
			{
				if (this._structuralCache.IsFormattingInProgress)
				{
					this._structuralCache.OnInvalidOperationDetected();
					throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
				}
				ITextPointer end;
				if (args.TextChange != TextChangeType.ContentRemoved)
				{
					end = args.ITextPosition.CreatePointer(args.Count, LogicalDirection.Forward);
				}
				else
				{
					end = args.ITextPosition;
				}
				if (!args.AffectsRenderOnly || (this._formatter != null && this._formatter is FlowDocumentFormatter))
				{
					DirtyTextRange dtr = new DirtyTextRange(args);
					this._structuralCache.AddDirtyTextRange(dtr);
				}
				else
				{
					this._structuralCache.InvalidateFormatCache(false);
				}
				if (this._formatter != null)
				{
					this._formatter.OnContentInvalidated(!args.AffectsRenderOnly, args.ITextPosition, end);
				}
			}
			finally
			{
				this._structuralCache.IsContentChangeInProgress = false;
			}
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x0023C8C8 File Offset: 0x0023B8C8
		private static bool IsValidPageSize(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return double.IsNaN(num) || (num >= 0.0 && num <= num2);
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x0023DD28 File Offset: 0x0023CD28
		private static bool IsValidMinPageSize(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && (double.IsNegativeInfinity(num) || (num >= 0.0 && num <= num2));
		}

		// Token: 0x06004CE5 RID: 19685 RVA: 0x0023DD74 File Offset: 0x0023CD74
		private static bool IsValidMaxPageSize(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && (double.IsPositiveInfinity(num) || (num >= 0.0 && num <= num2));
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x0022C372 File Offset: 0x0022B372
		private static bool IsValidPagePadding(object o)
		{
			return Block.IsValidThickness((Thickness)o, true);
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x0023DDC0 File Offset: 0x0023CDC0
		private static bool IsValidColumnRuleWidth(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && num >= 0.0 && num <= num2;
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x0023C8C8 File Offset: 0x0023B8C8
		private static bool IsValidColumnGap(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return double.IsNaN(num) || (num >= 0.0 && num <= num2);
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x0023DE00 File Offset: 0x0023CE00
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!TextSchema.IsValidChildOfContainer(FlowDocument._typeofThis, value.GetType()))
			{
				throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					FlowDocument._typeofThis.Name,
					value.GetType().Name
				}));
			}
			if (value is TextElement && ((TextElement)value).Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					value.GetType().Name
				}));
			}
			if (value is Block)
			{
				TextContainer textContainer = this._structuralCache.TextContainer;
				((Block)value).RepositionWithContent(textContainer.End);
				return;
			}
			Invariant.Assert(false);
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x0023DEC4 File Offset: 0x0023CEC4
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType == typeof(ITextContainer))
			{
				return this._structuralCache.TextContainer;
			}
			if (serviceType == typeof(TextContainer))
			{
				return this._structuralCache.TextContainer;
			}
			return null;
		}

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x0023DF24 File Offset: 0x0023CF24
		DocumentPaginator IDocumentPaginatorSource.DocumentPaginator
		{
			get
			{
				if (this._formatter != null && !(this._formatter is FlowDocumentPaginator))
				{
					this._formatter.Suspend();
					this._formatter = null;
				}
				if (this._formatter == null)
				{
					this._formatter = new FlowDocumentPaginator(this);
				}
				return (FlowDocumentPaginator)this._formatter;
			}
		}

		// Token: 0x040027CB RID: 10187
		private static readonly Type _typeofThis = typeof(FlowDocument);

		// Token: 0x040027CC RID: 10188
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027CD RID: 10189
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027CE RID: 10190
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027CF RID: 10191
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027D0 RID: 10192
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027D1 RID: 10193
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027D2 RID: 10194
		public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(FlowDocument._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x040027D3 RID: 10195
		public static readonly DependencyProperty TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(FlowDocument._typeofThis, new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x040027D4 RID: 10196
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027D5 RID: 10197
		public static readonly DependencyProperty FlowDirectionProperty = Block.FlowDirectionProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027D6 RID: 10198
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027D7 RID: 10199
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027D8 RID: 10200
		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040027D9 RID: 10201
		public static readonly DependencyProperty ColumnGapProperty = DependencyProperty.Register("ColumnGap", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(FlowDocument.IsValidColumnGap));

		// Token: 0x040027DA RID: 10202
		public static readonly DependencyProperty IsColumnWidthFlexibleProperty = DependencyProperty.Register("IsColumnWidthFlexible", typeof(bool), FlowDocument._typeofThis, new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040027DB RID: 10203
		public static readonly DependencyProperty ColumnRuleWidthProperty = DependencyProperty.Register("ColumnRuleWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(FlowDocument.IsValidColumnRuleWidth));

		// Token: 0x040027DC RID: 10204
		public static readonly DependencyProperty ColumnRuleBrushProperty = DependencyProperty.Register("ColumnRuleBrush", typeof(Brush), FlowDocument._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x040027DD RID: 10205
		public static readonly DependencyProperty IsOptimalParagraphEnabledProperty = DependencyProperty.Register("IsOptimalParagraphEnabled", typeof(bool), FlowDocument._typeofThis, new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x040027DE RID: 10206
		public static readonly DependencyProperty PageWidthProperty = DependencyProperty.Register("PageWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnPageMetricsChanged), new CoerceValueCallback(FlowDocument.CoercePageWidth)), new ValidateValueCallback(FlowDocument.IsValidPageSize));

		// Token: 0x040027DF RID: 10207
		public static readonly DependencyProperty MinPageWidthProperty = DependencyProperty.Register("MinPageWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMinPageWidthChanged)), new ValidateValueCallback(FlowDocument.IsValidMinPageSize));

		// Token: 0x040027E0 RID: 10208
		public static readonly DependencyProperty MaxPageWidthProperty = DependencyProperty.Register("MaxPageWidth", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMaxPageWidthChanged), new CoerceValueCallback(FlowDocument.CoerceMaxPageWidth)), new ValidateValueCallback(FlowDocument.IsValidMaxPageSize));

		// Token: 0x040027E1 RID: 10209
		public static readonly DependencyProperty PageHeightProperty = DependencyProperty.Register("PageHeight", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnPageMetricsChanged), new CoerceValueCallback(FlowDocument.CoercePageHeight)), new ValidateValueCallback(FlowDocument.IsValidPageSize));

		// Token: 0x040027E2 RID: 10210
		public static readonly DependencyProperty MinPageHeightProperty = DependencyProperty.Register("MinPageHeight", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMinPageHeightChanged)), new ValidateValueCallback(FlowDocument.IsValidMinPageSize));

		// Token: 0x040027E3 RID: 10211
		public static readonly DependencyProperty MaxPageHeightProperty = DependencyProperty.Register("MaxPageHeight", typeof(double), FlowDocument._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnMaxPageHeightChanged), new CoerceValueCallback(FlowDocument.CoerceMaxPageHeight)), new ValidateValueCallback(FlowDocument.IsValidMaxPageSize));

		// Token: 0x040027E4 RID: 10212
		public static readonly DependencyProperty PagePaddingProperty = DependencyProperty.Register("PagePadding", typeof(Thickness), FlowDocument._typeofThis, new FrameworkPropertyMetadata(new Thickness(double.NaN), FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FlowDocument.OnPageMetricsChanged)), new ValidateValueCallback(FlowDocument.IsValidPagePadding));

		// Token: 0x040027E5 RID: 10213
		public static readonly DependencyProperty IsHyphenationEnabledProperty = Block.IsHyphenationEnabledProperty.AddOwner(FlowDocument._typeofThis);

		// Token: 0x040027E7 RID: 10215
		private StructuralCache _structuralCache;

		// Token: 0x040027E8 RID: 10216
		private TypographyProperties _typographyPropertiesGroup;

		// Token: 0x040027E9 RID: 10217
		private IFlowDocumentFormatter _formatter;

		// Token: 0x040027EA RID: 10218
		private TextWrapping _textWrapping = TextWrapping.Wrap;

		// Token: 0x040027EB RID: 10219
		private double _pixelsPerDip = (double)Util.PixelsPerDip;
	}
}
