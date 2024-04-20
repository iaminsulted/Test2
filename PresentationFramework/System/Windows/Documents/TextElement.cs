using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.PresentationFramework;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x020006AD RID: 1709
	public abstract class TextElement : FrameworkContentElement, IAddChild
	{
		// Token: 0x06005699 RID: 22169 RVA: 0x0026A904 File Offset: 0x00269904
		static TextElement()
		{
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(TextElement.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(propertyChangedCallback));
			}
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x0026AAFC File Offset: 0x00269AFC
		internal TextElement()
		{
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x0026AB10 File Offset: 0x00269B10
		internal void Reposition(TextPointer start, TextPointer end)
		{
			if (start != null)
			{
				ValidationHelper.VerifyPositionPair(start, end);
			}
			else if (end != null)
			{
				throw new ArgumentException(SR.Get("TextElement_UnmatchedEndPointer"));
			}
			if (start != null)
			{
				SplayTreeNode splayTreeNode = start.GetScopingNode();
				SplayTreeNode splayTreeNode2 = end.GetScopingNode();
				if (splayTreeNode == this._textElementNode)
				{
					splayTreeNode = this._textElementNode.GetContainingNode();
				}
				if (splayTreeNode2 == this._textElementNode)
				{
					splayTreeNode2 = this._textElementNode.GetContainingNode();
				}
				if (splayTreeNode != splayTreeNode2)
				{
					throw new ArgumentException(SR.Get("InDifferentScope", new object[]
					{
						"start",
						"end"
					}));
				}
			}
			if (this.IsInTree)
			{
				TextContainer textContainer = this.EnsureTextContainer();
				if (start == null)
				{
					textContainer.BeginChange();
					try
					{
						textContainer.ExtractElementInternal(this);
						return;
					}
					finally
					{
						textContainer.EndChange();
					}
				}
				if (textContainer == start.TextContainer)
				{
					textContainer.BeginChange();
					try
					{
						textContainer.ExtractElementInternal(this);
						textContainer.InsertElementInternal(start, end, this);
						return;
					}
					finally
					{
						textContainer.EndChange();
					}
				}
				textContainer.BeginChange();
				try
				{
					textContainer.ExtractElementInternal(this);
				}
				finally
				{
					textContainer.EndChange();
				}
				start.TextContainer.BeginChange();
				try
				{
					start.TextContainer.InsertElementInternal(start, end, this);
					return;
				}
				finally
				{
					start.TextContainer.EndChange();
				}
			}
			if (start != null)
			{
				start.TextContainer.BeginChange();
				try
				{
					start.TextContainer.InsertElementInternal(start, end, this);
				}
				finally
				{
					start.TextContainer.EndChange();
				}
			}
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x0026ACA0 File Offset: 0x00269CA0
		internal void RepositionWithContent(TextPointer textPosition)
		{
			TextContainer textContainer;
			if (textPosition == null)
			{
				if (!this.IsInTree)
				{
					return;
				}
				textContainer = this.EnsureTextContainer();
				textContainer.BeginChange();
				try
				{
					textContainer.DeleteContentInternal(this.ElementStart, this.ElementEnd);
					return;
				}
				finally
				{
					textContainer.EndChange();
				}
			}
			textContainer = textPosition.TextContainer;
			textContainer.BeginChange();
			try
			{
				textContainer.InsertElementInternal(textPosition, textPosition, this);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x0600569D RID: 22173 RVA: 0x0026AD1C File Offset: 0x00269D1C
		internal TextRange TextRange
		{
			get
			{
				base.VerifyAccess();
				TextContainer tree = this.EnsureTextContainer();
				TextPointer textPointer = new TextPointer(tree, this._textElementNode, ElementEdge.AfterStart, LogicalDirection.Backward);
				textPointer.Freeze();
				TextPointer textPointer2 = new TextPointer(tree, this._textElementNode, ElementEdge.BeforeEnd, LogicalDirection.Forward);
				textPointer2.Freeze();
				return new TextRange(textPointer, textPointer2);
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x0600569E RID: 22174 RVA: 0x0026AD65 File Offset: 0x00269D65
		public TextPointer ElementStart
		{
			get
			{
				TextPointer textPointer = new TextPointer(this.EnsureTextContainer(), this._textElementNode, ElementEdge.BeforeStart, LogicalDirection.Forward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x0600569F RID: 22175 RVA: 0x0026AD80 File Offset: 0x00269D80
		internal StaticTextPointer StaticElementStart
		{
			get
			{
				return new StaticTextPointer(this.EnsureTextContainer(), this._textElementNode, 0);
			}
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x060056A0 RID: 22176 RVA: 0x0026AD94 File Offset: 0x00269D94
		public TextPointer ContentStart
		{
			get
			{
				TextPointer textPointer = new TextPointer(this.EnsureTextContainer(), this._textElementNode, ElementEdge.AfterStart, LogicalDirection.Backward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x060056A1 RID: 22177 RVA: 0x0026ADAF File Offset: 0x00269DAF
		internal StaticTextPointer StaticContentStart
		{
			get
			{
				return new StaticTextPointer(this.EnsureTextContainer(), this._textElementNode, 1);
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x060056A2 RID: 22178 RVA: 0x0026ADC3 File Offset: 0x00269DC3
		public TextPointer ContentEnd
		{
			get
			{
				TextPointer textPointer = new TextPointer(this.EnsureTextContainer(), this._textElementNode, ElementEdge.BeforeEnd, LogicalDirection.Forward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x060056A3 RID: 22179 RVA: 0x0026ADDE File Offset: 0x00269DDE
		internal StaticTextPointer StaticContentEnd
		{
			get
			{
				return new StaticTextPointer(this.EnsureTextContainer(), this._textElementNode, this._textElementNode.SymbolCount - 1);
			}
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x0026ADFE File Offset: 0x00269DFE
		internal bool Contains(TextPointer position)
		{
			ValidationHelper.VerifyPosition(this.EnsureTextContainer(), position);
			return this.ContentStart.CompareTo(position) <= 0 && this.ContentEnd.CompareTo(position) >= 0;
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x060056A5 RID: 22181 RVA: 0x0026AE2F File Offset: 0x00269E2F
		public TextPointer ElementEnd
		{
			get
			{
				TextPointer textPointer = new TextPointer(this.EnsureTextContainer(), this._textElementNode, ElementEdge.AfterEnd, LogicalDirection.Backward);
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x060056A6 RID: 22182 RVA: 0x0026AE4A File Offset: 0x00269E4A
		internal StaticTextPointer StaticElementEnd
		{
			get
			{
				return new StaticTextPointer(this.EnsureTextContainer(), this._textElementNode, this._textElementNode.SymbolCount);
			}
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x0026AE68 File Offset: 0x00269E68
		// (set) Token: 0x060056A8 RID: 22184 RVA: 0x0026AE7A File Offset: 0x00269E7A
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(TextElement.FontFamilyProperty);
			}
			set
			{
				base.SetValue(TextElement.FontFamilyProperty, value);
			}
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x0026AE88 File Offset: 0x00269E88
		public static void SetFontFamily(DependencyObject element, FontFamily value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontFamilyProperty, value);
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x0026AEA4 File Offset: 0x00269EA4
		public static FontFamily GetFontFamily(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontFamily)element.GetValue(TextElement.FontFamilyProperty);
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x060056AB RID: 22187 RVA: 0x0026AEC4 File Offset: 0x00269EC4
		// (set) Token: 0x060056AC RID: 22188 RVA: 0x0026AED6 File Offset: 0x00269ED6
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(TextElement.FontStyleProperty);
			}
			set
			{
				base.SetValue(TextElement.FontStyleProperty, value);
			}
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x0026AEE9 File Offset: 0x00269EE9
		public static void SetFontStyle(DependencyObject element, FontStyle value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontStyleProperty, value);
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x0026AF0A File Offset: 0x00269F0A
		public static FontStyle GetFontStyle(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStyle)element.GetValue(TextElement.FontStyleProperty);
		}

		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x060056AF RID: 22191 RVA: 0x0026AF2A File Offset: 0x00269F2A
		// (set) Token: 0x060056B0 RID: 22192 RVA: 0x0026AF3C File Offset: 0x00269F3C
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(TextElement.FontWeightProperty);
			}
			set
			{
				base.SetValue(TextElement.FontWeightProperty, value);
			}
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x0026AF4F File Offset: 0x00269F4F
		public static void SetFontWeight(DependencyObject element, FontWeight value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontWeightProperty, value);
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x0026AF70 File Offset: 0x00269F70
		public static FontWeight GetFontWeight(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontWeight)element.GetValue(TextElement.FontWeightProperty);
		}

		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x060056B3 RID: 22195 RVA: 0x0026AF90 File Offset: 0x00269F90
		// (set) Token: 0x060056B4 RID: 22196 RVA: 0x0026AFA2 File Offset: 0x00269FA2
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(TextElement.FontStretchProperty);
			}
			set
			{
				base.SetValue(TextElement.FontStretchProperty, value);
			}
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x0026AFB5 File Offset: 0x00269FB5
		public static void SetFontStretch(DependencyObject element, FontStretch value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontStretchProperty, value);
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x0026AFD6 File Offset: 0x00269FD6
		public static FontStretch GetFontStretch(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FontStretch)element.GetValue(TextElement.FontStretchProperty);
		}

		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x060056B7 RID: 22199 RVA: 0x0026AFF6 File Offset: 0x00269FF6
		// (set) Token: 0x060056B8 RID: 22200 RVA: 0x0026B008 File Offset: 0x0026A008
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(TextElement.FontSizeProperty);
			}
			set
			{
				base.SetValue(TextElement.FontSizeProperty, value);
			}
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x0026B01B File Offset: 0x0026A01B
		public static void SetFontSize(DependencyObject element, double value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.FontSizeProperty, value);
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x0026B03C File Offset: 0x0026A03C
		[TypeConverter(typeof(FontSizeConverter))]
		public static double GetFontSize(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (double)element.GetValue(TextElement.FontSizeProperty);
		}

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x060056BB RID: 22203 RVA: 0x0026B05C File Offset: 0x0026A05C
		// (set) Token: 0x060056BC RID: 22204 RVA: 0x0026B06E File Offset: 0x0026A06E
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(TextElement.ForegroundProperty);
			}
			set
			{
				base.SetValue(TextElement.ForegroundProperty, value);
			}
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x0026B07C File Offset: 0x0026A07C
		public static void SetForeground(DependencyObject element, Brush value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(TextElement.ForegroundProperty, value);
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x0026B098 File Offset: 0x0026A098
		public static Brush GetForeground(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Brush)element.GetValue(TextElement.ForegroundProperty);
		}

		// Token: 0x17001457 RID: 5207
		// (get) Token: 0x060056BF RID: 22207 RVA: 0x0026B0B8 File Offset: 0x0026A0B8
		// (set) Token: 0x060056C0 RID: 22208 RVA: 0x0026B0CA File Offset: 0x0026A0CA
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(TextElement.BackgroundProperty);
			}
			set
			{
				base.SetValue(TextElement.BackgroundProperty, value);
			}
		}

		// Token: 0x17001458 RID: 5208
		// (get) Token: 0x060056C1 RID: 22209 RVA: 0x0026B0D8 File Offset: 0x0026A0D8
		// (set) Token: 0x060056C2 RID: 22210 RVA: 0x0026B0EA File Offset: 0x0026A0EA
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(TextElement.TextEffectsProperty);
			}
			set
			{
				base.SetValue(TextElement.TextEffectsProperty, value);
			}
		}

		// Token: 0x17001459 RID: 5209
		// (get) Token: 0x060056C3 RID: 22211 RVA: 0x0023D24E File Offset: 0x0023C24E
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x0026B0F8 File Offset: 0x0026A0F8
		void IAddChild.AddChild(object value)
		{
			value.GetType();
			TextElement textElement = value as TextElement;
			if (textElement != null)
			{
				TextSchema.ValidateChild(this, textElement, true, true);
				this.Append(textElement);
				return;
			}
			UIElement uielement = value as UIElement;
			if (uielement == null)
			{
				throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					base.GetType().Name,
					value.GetType().Name
				}));
			}
			InlineUIContainer inlineUIContainer = this as InlineUIContainer;
			if (inlineUIContainer != null)
			{
				if (inlineUIContainer.Child != null)
				{
					throw new ArgumentException(SR.Get("TextSchema_ThisInlineUIContainerHasAChildUIElementAlready", new object[]
					{
						base.GetType().Name,
						((InlineUIContainer)this).Child.GetType().Name,
						value.GetType().Name
					}));
				}
				inlineUIContainer.Child = uielement;
				return;
			}
			else
			{
				BlockUIContainer blockUIContainer = this as BlockUIContainer;
				if (blockUIContainer != null)
				{
					if (blockUIContainer.Child != null)
					{
						throw new ArgumentException(SR.Get("TextSchema_ThisBlockUIContainerHasAChildUIElementAlready", new object[]
						{
							base.GetType().Name,
							((BlockUIContainer)this).Child.GetType().Name,
							value.GetType().Name
						}));
					}
					blockUIContainer.Child = uielement;
					return;
				}
				else
				{
					if (TextSchema.IsValidChild(this, typeof(InlineUIContainer)))
					{
						InlineUIContainer inlineUIContainer2 = Inline.CreateImplicitInlineUIContainer(this);
						this.Append(inlineUIContainer2);
						inlineUIContainer2.Child = uielement;
						return;
					}
					throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
					{
						base.GetType().Name,
						value.GetType().Name
					}));
				}
			}
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x0026B28C File Offset: 0x0026A28C
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (TextSchema.IsValidChild(this, typeof(string)))
			{
				this.Append(text);
				return;
			}
			if (TextSchema.IsValidChild(this, typeof(Run)))
			{
				Run run = Inline.CreateImplicitRun(this);
				this.Append(run);
				run.Text = text;
				return;
			}
			if (text.Trim().Length > 0)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_TextIsNotAllowed", new object[]
				{
					base.GetType().Name
				}));
			}
		}

		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x060056C6 RID: 22214 RVA: 0x0026B31B File Offset: 0x0026A31B
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (!this.IsEmpty)
				{
					return new RangeContentEnumerator(this.ContentStart, this.ContentEnd);
				}
				return new RangeContentEnumerator(null, null);
			}
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x0026B340 File Offset: 0x0026A340
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			bool flag = e.NewValueSource == BaseValueSourceInternal.Local || e.OldValueSource == BaseValueSourceInternal.Local;
			if ((flag || e.IsAValueChange || e.IsASubPropertyChange) && this.IsInTree)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null)
				{
					bool flag2 = frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange;
					bool flag3 = frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
					if (flag2 || flag3)
					{
						TextContainer textContainer = this.EnsureTextContainer();
						textContainer.BeginChange();
						try
						{
							if (flag)
							{
								TextTreeUndo.CreatePropertyUndoUnit(this, e);
							}
							if (e.IsAValueChange || e.IsASubPropertyChange)
							{
								this.NotifyTypographicPropertyChanged(flag2, flag, e.Property);
							}
						}
						finally
						{
							textContainer.EndChange();
						}
					}
				}
			}
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x0026B440 File Offset: 0x0026A440
		internal void NotifyTypographicPropertyChanged(bool affectsMeasureOrArrange, bool localValueChanged, DependencyProperty property)
		{
			if (!this.IsInTree)
			{
				return;
			}
			TextContainer textContainer = this.EnsureTextContainer();
			textContainer.NextLayoutGeneration();
			if (textContainer.HasListeners)
			{
				TextPointer textPointer = new TextPointer(textContainer, this._textElementNode, ElementEdge.BeforeStart, LogicalDirection.Forward);
				textPointer.Freeze();
				textContainer.BeginChange();
				try
				{
					textContainer.BeforeAddChange();
					if (localValueChanged)
					{
						textContainer.AddLocalValueChange();
					}
					textContainer.AddChange(textPointer, this._textElementNode.SymbolCount, this._textElementNode.IMECharCount, PrecursorTextChangeType.PropertyModified, property, !affectsMeasureOrArrange);
				}
				finally
				{
					textContainer.EndChange();
				}
			}
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x0026B4D4 File Offset: 0x0026A4D4
		internal static TypographyProperties GetTypographyProperties(DependencyObject element)
		{
			TypographyProperties typographyProperties = new TypographyProperties();
			typographyProperties.SetStandardLigatures((bool)element.GetValue(Typography.StandardLigaturesProperty));
			typographyProperties.SetContextualLigatures((bool)element.GetValue(Typography.ContextualLigaturesProperty));
			typographyProperties.SetDiscretionaryLigatures((bool)element.GetValue(Typography.DiscretionaryLigaturesProperty));
			typographyProperties.SetHistoricalLigatures((bool)element.GetValue(Typography.HistoricalLigaturesProperty));
			typographyProperties.SetAnnotationAlternates((int)element.GetValue(Typography.AnnotationAlternatesProperty));
			typographyProperties.SetContextualAlternates((bool)element.GetValue(Typography.ContextualAlternatesProperty));
			typographyProperties.SetHistoricalForms((bool)element.GetValue(Typography.HistoricalFormsProperty));
			typographyProperties.SetKerning((bool)element.GetValue(Typography.KerningProperty));
			typographyProperties.SetCapitalSpacing((bool)element.GetValue(Typography.CapitalSpacingProperty));
			typographyProperties.SetCaseSensitiveForms((bool)element.GetValue(Typography.CaseSensitiveFormsProperty));
			typographyProperties.SetStylisticSet1((bool)element.GetValue(Typography.StylisticSet1Property));
			typographyProperties.SetStylisticSet2((bool)element.GetValue(Typography.StylisticSet2Property));
			typographyProperties.SetStylisticSet3((bool)element.GetValue(Typography.StylisticSet3Property));
			typographyProperties.SetStylisticSet4((bool)element.GetValue(Typography.StylisticSet4Property));
			typographyProperties.SetStylisticSet5((bool)element.GetValue(Typography.StylisticSet5Property));
			typographyProperties.SetStylisticSet6((bool)element.GetValue(Typography.StylisticSet6Property));
			typographyProperties.SetStylisticSet7((bool)element.GetValue(Typography.StylisticSet7Property));
			typographyProperties.SetStylisticSet8((bool)element.GetValue(Typography.StylisticSet8Property));
			typographyProperties.SetStylisticSet9((bool)element.GetValue(Typography.StylisticSet9Property));
			typographyProperties.SetStylisticSet10((bool)element.GetValue(Typography.StylisticSet10Property));
			typographyProperties.SetStylisticSet11((bool)element.GetValue(Typography.StylisticSet11Property));
			typographyProperties.SetStylisticSet12((bool)element.GetValue(Typography.StylisticSet12Property));
			typographyProperties.SetStylisticSet13((bool)element.GetValue(Typography.StylisticSet13Property));
			typographyProperties.SetStylisticSet14((bool)element.GetValue(Typography.StylisticSet14Property));
			typographyProperties.SetStylisticSet15((bool)element.GetValue(Typography.StylisticSet15Property));
			typographyProperties.SetStylisticSet16((bool)element.GetValue(Typography.StylisticSet16Property));
			typographyProperties.SetStylisticSet17((bool)element.GetValue(Typography.StylisticSet17Property));
			typographyProperties.SetStylisticSet18((bool)element.GetValue(Typography.StylisticSet18Property));
			typographyProperties.SetStylisticSet19((bool)element.GetValue(Typography.StylisticSet19Property));
			typographyProperties.SetStylisticSet20((bool)element.GetValue(Typography.StylisticSet20Property));
			typographyProperties.SetFraction((FontFraction)element.GetValue(Typography.FractionProperty));
			typographyProperties.SetSlashedZero((bool)element.GetValue(Typography.SlashedZeroProperty));
			typographyProperties.SetMathematicalGreek((bool)element.GetValue(Typography.MathematicalGreekProperty));
			typographyProperties.SetEastAsianExpertForms((bool)element.GetValue(Typography.EastAsianExpertFormsProperty));
			typographyProperties.SetVariants((FontVariants)element.GetValue(Typography.VariantsProperty));
			typographyProperties.SetCapitals((FontCapitals)element.GetValue(Typography.CapitalsProperty));
			typographyProperties.SetNumeralStyle((FontNumeralStyle)element.GetValue(Typography.NumeralStyleProperty));
			typographyProperties.SetNumeralAlignment((FontNumeralAlignment)element.GetValue(Typography.NumeralAlignmentProperty));
			typographyProperties.SetEastAsianWidths((FontEastAsianWidths)element.GetValue(Typography.EastAsianWidthsProperty));
			typographyProperties.SetEastAsianLanguage((FontEastAsianLanguage)element.GetValue(Typography.EastAsianLanguageProperty));
			typographyProperties.SetStandardSwashes((int)element.GetValue(Typography.StandardSwashesProperty));
			typographyProperties.SetContextualSwashes((int)element.GetValue(Typography.ContextualSwashesProperty));
			typographyProperties.SetStylisticAlternates((int)element.GetValue(Typography.StylisticAlternatesProperty));
			return typographyProperties;
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x0026B898 File Offset: 0x0026A898
		internal void DeepEndInit()
		{
			if (!base.IsInitialized)
			{
				if (!this.IsEmpty)
				{
					IEnumerator logicalChildren = this.LogicalChildren;
					while (logicalChildren.MoveNext())
					{
						object obj = logicalChildren.Current;
						TextElement textElement = obj as TextElement;
						if (textElement != null)
						{
							textElement.DeepEndInit();
						}
					}
				}
				this.EndInit();
				Invariant.Assert(base.IsInitialized);
			}
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x0026B8EC File Offset: 0x0026A8EC
		internal static TextElement GetCommonAncestor(TextElement element1, TextElement element2)
		{
			if (element1 != element2)
			{
				int i = 0;
				int j = 0;
				TextElement textElement = element1;
				while (textElement.Parent is TextElement)
				{
					i++;
					textElement = (TextElement)textElement.Parent;
				}
				textElement = element2;
				while (textElement.Parent is TextElement)
				{
					j++;
					textElement = (TextElement)textElement.Parent;
				}
				while (i > j)
				{
					if (element1 == element2)
					{
						break;
					}
					element1 = (TextElement)element1.Parent;
					i--;
				}
				while (j > i)
				{
					if (element1 == element2)
					{
						break;
					}
					element2 = (TextElement)element2.Parent;
					j--;
				}
				while (element1 != element2)
				{
					element1 = (element1.Parent as TextElement);
					element2 = (element2.Parent as TextElement);
				}
			}
			Invariant.Assert(element1 == element2);
			return element1;
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnTextUpdated()
		{
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void BeforeLogicalTreeChange()
		{
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void AfterLogicalTreeChange()
		{
		}

		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x060056CF RID: 22223 RVA: 0x0026B9A6 File Offset: 0x0026A9A6
		internal TextContainer TextContainer
		{
			get
			{
				return this.EnsureTextContainer();
			}
		}

		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x060056D0 RID: 22224 RVA: 0x0026B9AE File Offset: 0x0026A9AE
		internal bool IsEmpty
		{
			get
			{
				return this._textElementNode == null || this._textElementNode.ContainedNode == null;
			}
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x060056D1 RID: 22225 RVA: 0x0026B9C8 File Offset: 0x0026A9C8
		internal bool IsInTree
		{
			get
			{
				return this._textElementNode != null;
			}
		}

		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x060056D2 RID: 22226 RVA: 0x0026B9D3 File Offset: 0x0026A9D3
		internal int ElementStartOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation) - 1;
			}
		}

		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x060056D3 RID: 22227 RVA: 0x0026B9FD File Offset: 0x0026A9FD
		internal int ContentStartOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation);
			}
		}

		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x060056D4 RID: 22228 RVA: 0x0026BA25 File Offset: 0x0026AA25
		internal int ContentEndOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation) + this._textElementNode.SymbolCount - 2;
			}
		}

		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x060056D5 RID: 22229 RVA: 0x0026BA5B File Offset: 0x0026AA5B
		internal int ElementEndOffset
		{
			get
			{
				Invariant.Assert(this.IsInTree, "TextElement is not in any TextContainer, caller should ensure this.");
				return this._textElementNode.GetSymbolOffset(this.EnsureTextContainer().Generation) + this._textElementNode.SymbolCount - 1;
			}
		}

		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x060056D6 RID: 22230 RVA: 0x0026BA91 File Offset: 0x0026AA91
		internal int SymbolCount
		{
			get
			{
				if (!this.IsInTree)
				{
					return 2;
				}
				return this._textElementNode.SymbolCount;
			}
		}

		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x060056D7 RID: 22231 RVA: 0x0026BAA8 File Offset: 0x0026AAA8
		// (set) Token: 0x060056D8 RID: 22232 RVA: 0x0026BAB0 File Offset: 0x0026AAB0
		internal TextTreeTextElementNode TextElementNode
		{
			get
			{
				return this._textElementNode;
			}
			set
			{
				this._textElementNode = value;
			}
		}

		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x060056D9 RID: 22233 RVA: 0x0026BAB9 File Offset: 0x0026AAB9
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

		// Token: 0x060056DA RID: 22234 RVA: 0x0026BADB File Offset: 0x0026AADB
		private static void OnTypographyChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			((TextElement)element)._typographyPropertiesGroup = null;
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x060056DB RID: 22235 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IsIMEStructuralElement
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x060056DC RID: 22236 RVA: 0x0026BAEC File Offset: 0x0026AAEC
		internal int IMELeftEdgeCharCount
		{
			get
			{
				int result = 0;
				if (this.IsIMEStructuralElement)
				{
					if (!this.IsInTree)
					{
						result = -1;
					}
					else
					{
						result = (this.TextElementNode.IsFirstSibling ? 0 : 1);
					}
				}
				return result;
			}
		}

		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x060056DD RID: 22237 RVA: 0x0026BB24 File Offset: 0x0026AB24
		internal virtual bool IsFirstIMEVisibleSibling
		{
			get
			{
				bool result = false;
				if (this.IsIMEStructuralElement)
				{
					result = (this.TextElementNode == null || this.TextElementNode.IsFirstSibling);
				}
				return result;
			}
		}

		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x060056DE RID: 22238 RVA: 0x0026BB54 File Offset: 0x0026AB54
		internal TextElement NextElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetNextNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x060056DF RID: 22239 RVA: 0x0026BB88 File Offset: 0x0026AB88
		internal TextElement PreviousElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetPreviousNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x060056E0 RID: 22240 RVA: 0x0026BBBC File Offset: 0x0026ABBC
		internal TextElement FirstChildElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetFirstContainedNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x060056E1 RID: 22241 RVA: 0x0026BBF0 File Offset: 0x0026ABF0
		internal TextElement LastChildElement
		{
			get
			{
				if (!this.IsInTree)
				{
					return null;
				}
				TextTreeTextElementNode textTreeTextElementNode = this._textElementNode.GetLastContainedNode() as TextTreeTextElementNode;
				if (textTreeTextElementNode == null)
				{
					return null;
				}
				return textTreeTextElementNode.TextElement;
			}
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x0026BC24 File Offset: 0x0026AC24
		private void Append(string textData)
		{
			if (textData == null)
			{
				throw new ArgumentNullException("textData");
			}
			TextContainer textContainer = this.EnsureTextContainer();
			textContainer.BeginChange();
			try
			{
				textContainer.InsertTextInternal(new TextPointer(textContainer, this._textElementNode, ElementEdge.BeforeEnd), textData);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x0026BC7C File Offset: 0x0026AC7C
		private void Append(TextElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			TextContainer textContainer = this.EnsureTextContainer();
			textContainer.BeginChange();
			try
			{
				TextPointer textPointer = new TextPointer(textContainer, this._textElementNode, ElementEdge.BeforeEnd);
				textContainer.InsertElementInternal(textPointer, textPointer, element);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x0026BCD4 File Offset: 0x0026ACD4
		private TextContainer EnsureTextContainer()
		{
			TextContainer textContainer;
			if (this.IsInTree)
			{
				textContainer = this._textElementNode.GetTextTree();
				textContainer.EmptyDeadPositionList();
			}
			else
			{
				textContainer = new TextContainer(null, false);
				TextPointer start = textContainer.Start;
				textContainer.BeginChange();
				try
				{
					textContainer.InsertElementInternal(start, start, this);
				}
				finally
				{
					textContainer.EndChange();
				}
				Invariant.Assert(this.IsInTree);
			}
			return textContainer;
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x0026BD40 File Offset: 0x0026AD40
		private static bool IsValidFontFamily(object o)
		{
			return o is FontFamily;
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x0026BD4C File Offset: 0x0026AD4C
		private static bool IsValidFontSize(object value)
		{
			double num = (double)value;
			double minWidth = TextDpi.MinWidth;
			double num2 = (double)Math.Min(1000000, 160000);
			return !double.IsNaN(num) && num >= minWidth && num <= num2;
		}

		// Token: 0x04002FA3 RID: 12195
		internal static readonly UncommonField<TextElement> ContainerTextElementField = new UncommonField<TextElement>();

		// Token: 0x04002FA4 RID: 12196
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.RegisterAttached("FontFamily", typeof(FontFamily), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(TextElement.IsValidFontFamily));

		// Token: 0x04002FA5 RID: 12197
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStyleProperty = DependencyProperty.RegisterAttached("FontStyle", typeof(FontStyle), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04002FA6 RID: 12198
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontWeightProperty = DependencyProperty.RegisterAttached("FontWeight", typeof(FontWeight), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04002FA7 RID: 12199
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStretchProperty = DependencyProperty.RegisterAttached("FontStretch", typeof(FontStretch), typeof(TextElement), new FrameworkPropertyMetadata(FontStretches.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04002FA8 RID: 12200
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontSizeProperty = DependencyProperty.RegisterAttached("FontSize", typeof(double), typeof(TextElement), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(TextElement.IsValidFontSize));

		// Token: 0x04002FA9 RID: 12201
		[CommonDependencyProperty]
		public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(TextElement), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

		// Token: 0x04002FAA RID: 12202
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(TextElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002FAB RID: 12203
		public static readonly DependencyProperty TextEffectsProperty = DependencyProperty.Register("TextEffects", typeof(TextEffectCollection), typeof(TextElement), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x04002FAC RID: 12204
		private TextTreeTextElementNode _textElementNode;

		// Token: 0x04002FAD RID: 12205
		private TypographyProperties _typographyPropertiesGroup = Typography.Default;
	}
}
