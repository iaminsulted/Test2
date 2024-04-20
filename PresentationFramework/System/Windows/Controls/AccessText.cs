using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000716 RID: 1814
	[ContentProperty("Text")]
	public class AccessText : FrameworkElement, IAddChild
	{
		// Token: 0x06005F46 RID: 24390 RVA: 0x00294908 File Offset: 0x00293908
		void IAddChild.AddChild(object value)
		{
			((IAddChild)this.TextBlock).AddChild(value);
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x00294916 File Offset: 0x00293916
		void IAddChild.AddText(string text)
		{
			((IAddChild)this.TextBlock).AddText(text);
		}

		// Token: 0x170015FE RID: 5630
		// (get) Token: 0x06005F48 RID: 24392 RVA: 0x00294924 File Offset: 0x00293924
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new RangeContentEnumerator(this.TextContainer.Start, this.TextContainer.End);
			}
		}

		// Token: 0x170015FF RID: 5631
		// (get) Token: 0x06005F4A RID: 24394 RVA: 0x00294941 File Offset: 0x00293941
		public char AccessKey
		{
			get
			{
				if (this._accessKey == null || this._accessKey.Text.Length <= 0)
				{
					return '\0';
				}
				return this._accessKey.Text[0];
			}
		}

		// Token: 0x17001600 RID: 5632
		// (get) Token: 0x06005F4B RID: 24395 RVA: 0x00294971 File Offset: 0x00293971
		// (set) Token: 0x06005F4C RID: 24396 RVA: 0x00294983 File Offset: 0x00293983
		[DefaultValue("")]
		public string Text
		{
			get
			{
				return (string)base.GetValue(AccessText.TextProperty);
			}
			set
			{
				base.SetValue(AccessText.TextProperty, value);
			}
		}

		// Token: 0x17001601 RID: 5633
		// (get) Token: 0x06005F4D RID: 24397 RVA: 0x00294991 File Offset: 0x00293991
		// (set) Token: 0x06005F4E RID: 24398 RVA: 0x002949A3 File Offset: 0x002939A3
		[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(AccessText.FontFamilyProperty);
			}
			set
			{
				base.SetValue(AccessText.FontFamilyProperty, value);
			}
		}

		// Token: 0x17001602 RID: 5634
		// (get) Token: 0x06005F4F RID: 24399 RVA: 0x002949B1 File Offset: 0x002939B1
		// (set) Token: 0x06005F50 RID: 24400 RVA: 0x002949C3 File Offset: 0x002939C3
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(AccessText.FontStyleProperty);
			}
			set
			{
				base.SetValue(AccessText.FontStyleProperty, value);
			}
		}

		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x06005F51 RID: 24401 RVA: 0x002949D6 File Offset: 0x002939D6
		// (set) Token: 0x06005F52 RID: 24402 RVA: 0x002949E8 File Offset: 0x002939E8
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(AccessText.FontWeightProperty);
			}
			set
			{
				base.SetValue(AccessText.FontWeightProperty, value);
			}
		}

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06005F53 RID: 24403 RVA: 0x002949FB File Offset: 0x002939FB
		// (set) Token: 0x06005F54 RID: 24404 RVA: 0x00294A0D File Offset: 0x00293A0D
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(AccessText.FontStretchProperty);
			}
			set
			{
				base.SetValue(AccessText.FontStretchProperty, value);
			}
		}

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x06005F55 RID: 24405 RVA: 0x00294A20 File Offset: 0x00293A20
		// (set) Token: 0x06005F56 RID: 24406 RVA: 0x00294A32 File Offset: 0x00293A32
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(AccessText.FontSizeProperty);
			}
			set
			{
				base.SetValue(AccessText.FontSizeProperty, value);
			}
		}

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x06005F57 RID: 24407 RVA: 0x00294A45 File Offset: 0x00293A45
		// (set) Token: 0x06005F58 RID: 24408 RVA: 0x00294A57 File Offset: 0x00293A57
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(AccessText.ForegroundProperty);
			}
			set
			{
				base.SetValue(AccessText.ForegroundProperty, value);
			}
		}

		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x06005F59 RID: 24409 RVA: 0x00294A65 File Offset: 0x00293A65
		// (set) Token: 0x06005F5A RID: 24410 RVA: 0x00294A77 File Offset: 0x00293A77
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(AccessText.BackgroundProperty);
			}
			set
			{
				base.SetValue(AccessText.BackgroundProperty, value);
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x06005F5B RID: 24411 RVA: 0x00294A85 File Offset: 0x00293A85
		// (set) Token: 0x06005F5C RID: 24412 RVA: 0x00294A97 File Offset: 0x00293A97
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(AccessText.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(AccessText.TextDecorationsProperty, value);
			}
		}

		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x06005F5D RID: 24413 RVA: 0x00294AA5 File Offset: 0x00293AA5
		// (set) Token: 0x06005F5E RID: 24414 RVA: 0x00294AB7 File Offset: 0x00293AB7
		public TextEffectCollection TextEffects
		{
			get
			{
				return (TextEffectCollection)base.GetValue(AccessText.TextEffectsProperty);
			}
			set
			{
				base.SetValue(AccessText.TextEffectsProperty, value);
			}
		}

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x06005F5F RID: 24415 RVA: 0x00294AC5 File Offset: 0x00293AC5
		// (set) Token: 0x06005F60 RID: 24416 RVA: 0x00294AD7 File Offset: 0x00293AD7
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(AccessText.LineHeightProperty);
			}
			set
			{
				base.SetValue(AccessText.LineHeightProperty, value);
			}
		}

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x06005F61 RID: 24417 RVA: 0x00294AEA File Offset: 0x00293AEA
		// (set) Token: 0x06005F62 RID: 24418 RVA: 0x00294AFC File Offset: 0x00293AFC
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(AccessText.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(AccessText.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x06005F63 RID: 24419 RVA: 0x00294B0F File Offset: 0x00293B0F
		// (set) Token: 0x06005F64 RID: 24420 RVA: 0x00294B21 File Offset: 0x00293B21
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(AccessText.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(AccessText.TextAlignmentProperty, value);
			}
		}

		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x06005F65 RID: 24421 RVA: 0x00294B34 File Offset: 0x00293B34
		// (set) Token: 0x06005F66 RID: 24422 RVA: 0x00294B46 File Offset: 0x00293B46
		public TextTrimming TextTrimming
		{
			get
			{
				return (TextTrimming)base.GetValue(AccessText.TextTrimmingProperty);
			}
			set
			{
				base.SetValue(AccessText.TextTrimmingProperty, value);
			}
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x06005F67 RID: 24423 RVA: 0x00294B59 File Offset: 0x00293B59
		// (set) Token: 0x06005F68 RID: 24424 RVA: 0x00294B6B File Offset: 0x00293B6B
		public TextWrapping TextWrapping
		{
			get
			{
				return (TextWrapping)base.GetValue(AccessText.TextWrappingProperty);
			}
			set
			{
				base.SetValue(AccessText.TextWrappingProperty, value);
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x06005F69 RID: 24425 RVA: 0x00294B7E File Offset: 0x00293B7E
		// (set) Token: 0x06005F6A RID: 24426 RVA: 0x00294B90 File Offset: 0x00293B90
		public double BaselineOffset
		{
			get
			{
				return (double)base.GetValue(AccessText.BaselineOffsetProperty);
			}
			set
			{
				base.SetValue(AccessText.BaselineOffsetProperty, value);
			}
		}

		// Token: 0x06005F6B RID: 24427 RVA: 0x00294BA3 File Offset: 0x00293BA3
		protected sealed override Size MeasureOverride(Size constraint)
		{
			this.TextBlock.Measure(constraint);
			return this.TextBlock.DesiredSize;
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x00294BBC File Offset: 0x00293BBC
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			this.TextBlock.Arrange(new Rect(arrangeSize));
			return arrangeSize;
		}

		// Token: 0x06005F6D RID: 24429 RVA: 0x00294BD0 File Offset: 0x00293BD0
		internal static bool HasCustomSerialization(object o)
		{
			Run run = o as Run;
			return run != null && AccessText.HasCustomSerializationStorage.GetValue(run);
		}

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x06005F6E RID: 24430 RVA: 0x00294BF4 File Offset: 0x00293BF4
		internal TextBlock TextBlock
		{
			get
			{
				if (this._textBlock == null)
				{
					this.CreateTextBlock();
				}
				return this._textBlock;
			}
		}

		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x06005F6F RID: 24431 RVA: 0x00294C0A File Offset: 0x00293C0A
		internal static char AccessKeyMarker
		{
			get
			{
				return '_';
			}
		}

		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x06005F70 RID: 24432 RVA: 0x00294C0E File Offset: 0x00293C0E
		private TextContainer TextContainer
		{
			get
			{
				if (this._textContainer == null)
				{
					this.CreateTextBlock();
				}
				return this._textContainer;
			}
		}

		// Token: 0x06005F71 RID: 24433 RVA: 0x00294C24 File Offset: 0x00293C24
		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AccessText)d).TextBlock.SetValue(e.Property, e.NewValue);
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x00294C44 File Offset: 0x00293C44
		private void CreateTextBlock()
		{
			this._textContainer = new TextContainer(this, false);
			this._textBlock = new TextBlock();
			base.AddVisualChild(this._textBlock);
			this._textBlock.IsContentPresenterContainer = true;
			this._textBlock.SetTextContainer(this._textContainer);
			this.InitializeTextContainerListener();
		}

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x06005F73 RID: 24435 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06005F74 RID: 24436 RVA: 0x00294C98 File Offset: 0x00293C98
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this.TextBlock;
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x00294CC0 File Offset: 0x00293CC0
		internal static void SerializeCustom(XmlWriter xmlWriter, object o)
		{
			Run run = o as Run;
			if (run != null)
			{
				xmlWriter.WriteString(AccessText.AccessKeyMarker.ToString() + run.Text);
			}
		}

		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x06005F76 RID: 24438 RVA: 0x00294CF8 File Offset: 0x00293CF8
		private static Style AccessKeyStyle
		{
			get
			{
				if (AccessText._accessKeyStyle == null)
				{
					Style style = new Style(typeof(Run));
					Trigger trigger = new Trigger();
					trigger.Property = KeyboardNavigation.ShowKeyboardCuesProperty;
					trigger.Value = true;
					trigger.Setters.Add(new Setter(AccessText.TextDecorationsProperty, System.Windows.TextDecorations.Underline));
					style.Triggers.Add(trigger);
					style.Seal();
					AccessText._accessKeyStyle = style;
				}
				return AccessText._accessKeyStyle;
			}
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x00294D70 File Offset: 0x00293D70
		private void UpdateAccessKey()
		{
			TextPointer textPointer = new TextPointer(this.TextContainer.Start);
			while (!this._accessKeyLocated && textPointer.CompareTo(this.TextContainer.End) < 0)
			{
				if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
				{
					string textInRun = textPointer.GetTextInRun(LogicalDirection.Forward);
					int num = AccessText.FindAccessKeyMarker(textInRun);
					if (num != -1 && num < textInRun.Length - 1)
					{
						string nextTextElement = StringInfo.GetNextTextElement(textInRun, num + 1);
						TextPointer positionAtOffset = textPointer.GetPositionAtOffset(num + 1 + nextTextElement.Length);
						this._accessKey = new Run(nextTextElement);
						this._accessKey.Style = AccessText.AccessKeyStyle;
						this.RegisterAccessKey();
						AccessText.HasCustomSerializationStorage.SetValue(this._accessKey, true);
						this._accessKeyLocated = true;
						this.UninitializeTextContainerListener();
						this.TextContainer.BeginChange();
						try
						{
							TextPointer textPointer2 = new TextPointer(textPointer, num);
							TextRangeEdit.DeleteInlineContent(textPointer2, positionAtOffset);
							this._accessKey.RepositionWithContent(textPointer2);
						}
						finally
						{
							this.TextContainer.EndChange();
							this.InitializeTextContainerListener();
						}
					}
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			textPointer = new TextPointer(this.TextContainer.Start);
			string text = AccessText.AccessKeyMarker.ToString();
			string oldValue = text + text;
			while (textPointer.CompareTo(this.TextContainer.End) < 0)
			{
				if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
				{
					string textInRun2 = textPointer.GetTextInRun(LogicalDirection.Forward);
					string text2 = textInRun2.Replace(oldValue, text);
					if (textInRun2 != text2)
					{
						TextPointer start = new TextPointer(textPointer, 0);
						TextPointer textPointer3 = new TextPointer(textPointer, textInRun2.Length);
						this.UninitializeTextContainerListener();
						this.TextContainer.BeginChange();
						try
						{
							textPointer3.InsertTextInRun(text2);
							TextRangeEdit.DeleteInlineContent(start, textPointer3);
						}
						finally
						{
							this.TextContainer.EndChange();
							this.InitializeTextContainerListener();
						}
					}
				}
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x00294F70 File Offset: 0x00293F70
		private static int FindAccessKeyMarker(string text)
		{
			int length = text.Length;
			int num;
			for (int i = 0; i < length; i = num + 2)
			{
				num = text.IndexOf(AccessText.AccessKeyMarker, i);
				if (num == -1)
				{
					return -1;
				}
				if (num + 1 < length && text[num + 1] != AccessText.AccessKeyMarker)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x00294FBC File Offset: 0x00293FBC
		internal static string RemoveAccessKeyMarker(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = AccessText.AccessKeyMarker.ToString();
				string oldValue = text2 + text2;
				int num = AccessText.FindAccessKeyMarker(text);
				if (num >= 0 && num < text.Length - 1)
				{
					text = text.Remove(num, 1);
				}
				text = text.Replace(oldValue, text2);
			}
			return text;
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x00295014 File Offset: 0x00294014
		private void RegisterAccessKey()
		{
			if (this._currentlyRegistered != null)
			{
				AccessKeyManager.Unregister(this._currentlyRegistered, this);
				this._currentlyRegistered = null;
			}
			string text = this._accessKey.Text;
			if (!string.IsNullOrEmpty(text))
			{
				AccessKeyManager.Register(text, this);
				this._currentlyRegistered = text;
			}
		}

		// Token: 0x06005F7B RID: 24443 RVA: 0x0029505E File Offset: 0x0029405E
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((AccessText)d).UpdateText((string)e.NewValue);
		}

		// Token: 0x06005F7C RID: 24444 RVA: 0x00295078 File Offset: 0x00294078
		private void UpdateText(string text)
		{
			if (text == null)
			{
				text = string.Empty;
			}
			this._accessKeyLocated = false;
			this._accessKey = null;
			this.TextContainer.BeginChange();
			try
			{
				this.TextContainer.DeleteContentInternal(this.TextContainer.Start, this.TextContainer.End);
				Run run = Inline.CreateImplicitRun(this);
				this.TextContainer.End.InsertTextElement(run);
				run.Text = text;
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x06005F7D RID: 24445 RVA: 0x00295108 File Offset: 0x00294108
		private void InitializeTextContainerListener()
		{
			this.TextContainer.Changed += this.OnTextContainerChanged;
		}

		// Token: 0x06005F7E RID: 24446 RVA: 0x00295121 File Offset: 0x00294121
		private void UninitializeTextContainerListener()
		{
			this.TextContainer.Changed -= this.OnTextContainerChanged;
		}

		// Token: 0x06005F7F RID: 24447 RVA: 0x0029513A File Offset: 0x0029413A
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs args)
		{
			if (args.HasContentAddedOrRemoved)
			{
				this.UpdateAccessKey();
			}
		}

		// Token: 0x040031BB RID: 12731
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AccessText), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnTextChanged)));

		// Token: 0x040031BC RID: 12732
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031BD RID: 12733
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031BE RID: 12734
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031BF RID: 12735
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031C0 RID: 12736
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031C1 RID: 12737
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031C2 RID: 12738
		public static readonly DependencyProperty BackgroundProperty = TextElement.BackgroundProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		// Token: 0x040031C3 RID: 12739
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		// Token: 0x040031C4 RID: 12740
		public static readonly DependencyProperty TextEffectsProperty = TextElement.TextEffectsProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextEffectCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		// Token: 0x040031C5 RID: 12741
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031C6 RID: 12742
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031C7 RID: 12743
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(AccessText));

		// Token: 0x040031C8 RID: 12744
		public static readonly DependencyProperty TextTrimmingProperty = TextBlock.TextTrimmingProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		// Token: 0x040031C9 RID: 12745
		public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		// Token: 0x040031CA RID: 12746
		public static readonly DependencyProperty BaselineOffsetProperty = TextBlock.BaselineOffsetProperty.AddOwner(typeof(AccessText), new FrameworkPropertyMetadata(new PropertyChangedCallback(AccessText.OnPropertyChanged)));

		// Token: 0x040031CB RID: 12747
		private TextContainer _textContainer;

		// Token: 0x040031CC RID: 12748
		private TextBlock _textBlock;

		// Token: 0x040031CD RID: 12749
		private Run _accessKey;

		// Token: 0x040031CE RID: 12750
		private bool _accessKeyLocated;

		// Token: 0x040031CF RID: 12751
		private const char _accessKeyMarker = '_';

		// Token: 0x040031D0 RID: 12752
		private static Style _accessKeyStyle;

		// Token: 0x040031D1 RID: 12753
		private string _currentlyRegistered;

		// Token: 0x040031D2 RID: 12754
		private static readonly UncommonField<bool> HasCustomSerializationStorage = new UncommonField<bool>();
	}
}
