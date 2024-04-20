using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007E5 RID: 2021
	[Localizability(LocalizationCategory.Text)]
	[ContentProperty("Text")]
	public class TextBox : TextBoxBase, IAddChild, ITextBoxViewHost
	{
		// Token: 0x060074DA RID: 29914 RVA: 0x002E9820 File Offset: 0x002E8820
		static TextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
			TextBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TextBox));
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(TextBox.OnMinMaxChanged);
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			FrameworkElement.MinHeightProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			FrameworkElement.MaxHeightProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			Control.FontFamilyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			Control.FontSizeProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			PropertyChangedCallback propertyChangedCallback2 = new PropertyChangedCallback(TextBox.OnTypographyChanged);
			DependencyProperty[] typographyPropertiesList = Typography.TypographyPropertiesList;
			for (int i = 0; i < typographyPropertiesList.Length; i++)
			{
				typographyPropertiesList[i].OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(propertyChangedCallback2));
			}
			TextBoxBase.HorizontalScrollBarVisibilityProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(ScrollBarVisibility.Hidden, new PropertyChangedCallback(TextBoxBase.OnScrollViewerPropertyChanged), new CoerceValueCallback(TextBox.CoerceHorizontalScrollBarVisibility)));
			ControlsTraceLogger.AddControl(TelemetryControls.TextBox);
		}

		// Token: 0x060074DB RID: 29915 RVA: 0x002E9B38 File Offset: 0x002E8B38
		public TextBox()
		{
			TextEditor.RegisterCommandHandlers(typeof(TextBox), false, false, false);
			base.InitializeTextContainer(new TextContainer(this, true)
			{
				CollectTextChanges = true
			});
			base.TextEditor.AcceptsRichContent = false;
		}

		// Token: 0x060074DC RID: 29916 RVA: 0x002E9B8A File Offset: 0x002E8B8A
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			throw new InvalidOperationException(SR.Get("TextBoxInvalidChild", new object[]
			{
				value.ToString()
			}));
		}

		// Token: 0x060074DD RID: 29917 RVA: 0x002E9BB8 File Offset: 0x002E8BB8
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			base.TextContainer.End.InsertTextInRun(text);
		}

		// Token: 0x060074DE RID: 29918 RVA: 0x002E9BDC File Offset: 0x002E8BDC
		public void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.Get("ParameterCannotBeNegative"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", SR.Get("ParameterCannotBeNegative"));
			}
			int symbolCount = base.TextContainer.SymbolCount;
			if (start > symbolCount)
			{
				start = symbolCount;
			}
			TextPointer textPointer = base.TextContainer.CreatePointerAtOffset(start, LogicalDirection.Forward);
			textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
			int offsetToPosition = textPointer.GetOffsetToPosition(base.TextContainer.End);
			if (length > offsetToPosition)
			{
				length = offsetToPosition;
			}
			TextPointer textPointer2 = new TextPointer(textPointer, length, LogicalDirection.Forward);
			textPointer2 = textPointer2.GetInsertionPosition(LogicalDirection.Forward);
			base.TextSelectionInternal.Select(textPointer, textPointer2);
		}

		// Token: 0x060074DF RID: 29919 RVA: 0x002E9C7C File Offset: 0x002E8C7C
		public void Clear()
		{
			using (base.TextSelectionInternal.DeclareChangeBlock())
			{
				base.TextContainer.DeleteContentInternal(base.TextContainer.Start, base.TextContainer.End);
				base.TextSelectionInternal.Select(base.TextContainer.Start, base.TextContainer.Start);
			}
		}

		// Token: 0x060074E0 RID: 29920 RVA: 0x002E9CF4 File Offset: 0x002E8CF4
		public int GetCharacterIndexFromPoint(Point point, bool snapToText)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			TextPointer textPositionFromPointInternal = base.GetTextPositionFromPointInternal(point, snapToText);
			if (textPositionFromPointInternal == null)
			{
				return -1;
			}
			int offset = textPositionFromPointInternal.Offset;
			if (textPositionFromPointInternal.LogicalDirection != LogicalDirection.Backward)
			{
				return offset;
			}
			return offset - 1;
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x002E9D30 File Offset: 0x002E8D30
		public int GetCharacterIndexFromLineIndex(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			if (startPositionOfLine != null)
			{
				return startPositionOfLine.Offset;
			}
			return 0;
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x002E9D74 File Offset: 0x002E8D74
		public int GetLineIndexFromCharacterIndex(int charIndex)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			int result;
			if (base.TextContainer.CreatePointerAtOffset(charIndex, LogicalDirection.Forward).ValidateLayout())
			{
				result = ((TextBoxView)base.RenderScope).GetLineIndexFromOffset(charIndex);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x002E9DD4 File Offset: 0x002E8DD4
		public int GetLineLength(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			TextPointer endPositionOfLine = this.GetEndPositionOfLine(lineIndex);
			int result;
			if (startPositionOfLine == null || endPositionOfLine == null)
			{
				result = -1;
			}
			else
			{
				result = startPositionOfLine.GetOffsetToPosition(endPositionOfLine);
			}
			return result;
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x002E9E28 File Offset: 0x002E8E28
		public int GetFirstVisibleLineIndex()
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			double lineHeight = this.GetLineHeight();
			return (int)Math.Floor(base.VerticalOffset / lineHeight + 0.0001);
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x002E9E60 File Offset: 0x002E8E60
		public int GetLastVisibleLineIndex()
		{
			if (base.RenderScope == null)
			{
				return -1;
			}
			double extentHeight = ((IScrollInfo)base.RenderScope).ExtentHeight;
			if (base.VerticalOffset + base.ViewportHeight >= extentHeight)
			{
				return this.LineCount - 1;
			}
			return (int)Math.Floor((base.VerticalOffset + base.ViewportHeight - 1.0) / this.GetLineHeight());
		}

		// Token: 0x060074E6 RID: 29926 RVA: 0x002E9EC8 File Offset: 0x002E8EC8
		public void ScrollToLine(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			Rect targetRectangle;
			if (this.GetRectangleFromTextPositionInternal(startPositionOfLine, false, out targetRectangle))
			{
				base.RenderScope.BringIntoView(targetRectangle);
			}
		}

		// Token: 0x060074E7 RID: 29927 RVA: 0x002E9F18 File Offset: 0x002E8F18
		public string GetLineText(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			if (lineIndex < 0 || lineIndex >= this.LineCount)
			{
				throw new ArgumentOutOfRangeException("lineIndex");
			}
			TextPointer startPositionOfLine = this.GetStartPositionOfLine(lineIndex);
			TextPointer endPositionOfLine = this.GetEndPositionOfLine(lineIndex);
			string result;
			if (startPositionOfLine != null && endPositionOfLine != null)
			{
				result = TextRangeBase.GetTextInternal(startPositionOfLine, endPositionOfLine);
			}
			else
			{
				result = this.Text;
			}
			return result;
		}

		// Token: 0x060074E8 RID: 29928 RVA: 0x002E9F6F File Offset: 0x002E8F6F
		public Rect GetRectFromCharacterIndex(int charIndex)
		{
			return this.GetRectFromCharacterIndex(charIndex, false);
		}

		// Token: 0x060074E9 RID: 29929 RVA: 0x002E9F7C File Offset: 0x002E8F7C
		public Rect GetRectFromCharacterIndex(int charIndex, bool trailingEdge)
		{
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			TextPointer textPointer = base.TextContainer.CreatePointerAtOffset(charIndex, LogicalDirection.Backward);
			textPointer = textPointer.GetInsertionPosition(LogicalDirection.Backward);
			if (trailingEdge && charIndex < base.TextContainer.SymbolCount)
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
				Invariant.Assert(textPointer != null);
				textPointer = textPointer.GetPositionAtOffset(0, LogicalDirection.Backward);
			}
			else
			{
				textPointer = textPointer.GetPositionAtOffset(0, LogicalDirection.Forward);
			}
			Rect result;
			this.GetRectangleFromTextPositionInternal(textPointer, true, out result);
			return result;
		}

		// Token: 0x060074EA RID: 29930 RVA: 0x002EA000 File Offset: 0x002E9000
		public SpellingError GetSpellingError(int charIndex)
		{
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			TextPointer position = base.TextContainer.CreatePointerAtOffset(charIndex, LogicalDirection.Forward);
			SpellingError spellingErrorAtPosition = base.TextEditor.GetSpellingErrorAtPosition(position, LogicalDirection.Forward);
			if (spellingErrorAtPosition == null && charIndex < base.TextContainer.SymbolCount - 1)
			{
				position = base.TextContainer.CreatePointerAtOffset(charIndex + 1, LogicalDirection.Forward);
				spellingErrorAtPosition = base.TextEditor.GetSpellingErrorAtPosition(position, LogicalDirection.Backward);
			}
			return spellingErrorAtPosition;
		}

		// Token: 0x060074EB RID: 29931 RVA: 0x002EA078 File Offset: 0x002E9078
		public int GetSpellingErrorStart(int charIndex)
		{
			SpellingError spellingError = this.GetSpellingError(charIndex);
			if (spellingError != null)
			{
				return spellingError.Start.Offset;
			}
			return -1;
		}

		// Token: 0x060074EC RID: 29932 RVA: 0x002EA0A0 File Offset: 0x002E90A0
		public int GetSpellingErrorLength(int charIndex)
		{
			SpellingError spellingError = this.GetSpellingError(charIndex);
			if (spellingError != null)
			{
				return spellingError.End.Offset - spellingError.Start.Offset;
			}
			return 0;
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x002EA0D4 File Offset: 0x002E90D4
		public int GetNextSpellingErrorCharacterIndex(int charIndex, LogicalDirection direction)
		{
			if (charIndex < 0 || charIndex > base.TextContainer.SymbolCount)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			if (base.TextContainer.SymbolCount == 0)
			{
				return -1;
			}
			ITextPointer textPointer = base.TextContainer.CreatePointerAtOffset(charIndex, direction);
			textPointer = base.TextEditor.GetNextSpellingErrorPosition(textPointer, direction);
			if (textPointer != null)
			{
				return textPointer.Offset;
			}
			return -1;
		}

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x060074EE RID: 29934 RVA: 0x002EA134 File Offset: 0x002E9134
		// (set) Token: 0x060074EF RID: 29935 RVA: 0x002EA146 File Offset: 0x002E9146
		public TextWrapping TextWrapping
		{
			get
			{
				return (TextWrapping)base.GetValue(TextBox.TextWrappingProperty);
			}
			set
			{
				base.SetValue(TextBox.TextWrappingProperty, value);
			}
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x060074F0 RID: 29936 RVA: 0x002EA159 File Offset: 0x002E9159
		// (set) Token: 0x060074F1 RID: 29937 RVA: 0x002EA16B File Offset: 0x002E916B
		[DefaultValue(1)]
		public int MinLines
		{
			get
			{
				return (int)base.GetValue(TextBox.MinLinesProperty);
			}
			set
			{
				base.SetValue(TextBox.MinLinesProperty, value);
			}
		}

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x060074F2 RID: 29938 RVA: 0x002EA17E File Offset: 0x002E917E
		// (set) Token: 0x060074F3 RID: 29939 RVA: 0x002EA190 File Offset: 0x002E9190
		[DefaultValue(2147483647)]
		public int MaxLines
		{
			get
			{
				return (int)base.GetValue(TextBox.MaxLinesProperty);
			}
			set
			{
				base.SetValue(TextBox.MaxLinesProperty, value);
			}
		}

		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x060074F4 RID: 29940 RVA: 0x002EA1A3 File Offset: 0x002E91A3
		// (set) Token: 0x060074F5 RID: 29941 RVA: 0x002EA1B5 File Offset: 0x002E91B5
		[DefaultValue("")]
		[Localizability(LocalizationCategory.Text)]
		public string Text
		{
			get
			{
				return (string)base.GetValue(TextBox.TextProperty);
			}
			set
			{
				base.SetValue(TextBox.TextProperty, value);
			}
		}

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x060074F6 RID: 29942 RVA: 0x002EA1C3 File Offset: 0x002E91C3
		// (set) Token: 0x060074F7 RID: 29943 RVA: 0x002EA1D5 File Offset: 0x002E91D5
		public CharacterCasing CharacterCasing
		{
			get
			{
				return (CharacterCasing)base.GetValue(TextBox.CharacterCasingProperty);
			}
			set
			{
				base.SetValue(TextBox.CharacterCasingProperty, value);
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x060074F8 RID: 29944 RVA: 0x002EA1E8 File Offset: 0x002E91E8
		// (set) Token: 0x060074F9 RID: 29945 RVA: 0x002EA1FA File Offset: 0x002E91FA
		[DefaultValue(0)]
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable)]
		public int MaxLength
		{
			get
			{
				return (int)base.GetValue(TextBox.MaxLengthProperty);
			}
			set
			{
				base.SetValue(TextBox.MaxLengthProperty, value);
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x060074FA RID: 29946 RVA: 0x002EA20D File Offset: 0x002E920D
		// (set) Token: 0x060074FB RID: 29947 RVA: 0x002EA21F File Offset: 0x002E921F
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(TextBox.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(TextBox.TextAlignmentProperty, value);
			}
		}

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x060074FC RID: 29948 RVA: 0x002EA232 File Offset: 0x002E9232
		// (set) Token: 0x060074FD RID: 29949 RVA: 0x002EA240 File Offset: 0x002E9240
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedText
		{
			get
			{
				return base.TextSelectionInternal.Text;
			}
			set
			{
				using (base.TextSelectionInternal.DeclareChangeBlock())
				{
					base.TextSelectionInternal.Text = value;
				}
			}
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x060074FE RID: 29950 RVA: 0x002EA284 File Offset: 0x002E9284
		// (set) Token: 0x060074FF RID: 29951 RVA: 0x002EA2A4 File Offset: 0x002E92A4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(0)]
		public int SelectionLength
		{
			get
			{
				return base.TextSelectionInternal.Start.GetOffsetToPosition(base.TextSelectionInternal.End);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get("ParameterCannotBeNegative"));
				}
				int offsetToPosition = base.TextSelectionInternal.Start.GetOffsetToPosition(base.TextContainer.End);
				if (value > offsetToPosition)
				{
					value = offsetToPosition;
				}
				TextPointer textPointer = new TextPointer(base.TextSelectionInternal.Start, value, LogicalDirection.Forward);
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
				base.TextSelectionInternal.Select(base.TextSelectionInternal.Start, textPointer);
			}
		}

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x06007500 RID: 29952 RVA: 0x002EA31F File Offset: 0x002E931F
		// (set) Token: 0x06007501 RID: 29953 RVA: 0x002EA334 File Offset: 0x002E9334
		[DefaultValue(0)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectionStart
		{
			get
			{
				return base.TextSelectionInternal.Start.Offset;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get("ParameterCannotBeNegative"));
				}
				int num = base.TextSelectionInternal.Start.GetOffsetToPosition(base.TextSelectionInternal.End);
				int symbolCount = base.TextContainer.SymbolCount;
				if (value > symbolCount)
				{
					value = symbolCount;
				}
				TextPointer textPointer = base.TextContainer.CreatePointerAtOffset(value, LogicalDirection.Forward);
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
				int offsetToPosition = textPointer.GetOffsetToPosition(base.TextContainer.End);
				if (num > offsetToPosition)
				{
					num = offsetToPosition;
				}
				TextPointer textPointer2 = new TextPointer(textPointer, num, LogicalDirection.Forward);
				textPointer2 = textPointer2.GetInsertionPosition(LogicalDirection.Forward);
				base.TextSelectionInternal.Select(textPointer, textPointer2);
			}
		}

		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x06007502 RID: 29954 RVA: 0x002EA3D9 File Offset: 0x002E93D9
		// (set) Token: 0x06007503 RID: 29955 RVA: 0x002EA3E1 File Offset: 0x002E93E1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CaretIndex
		{
			get
			{
				return this.SelectionStart;
			}
			set
			{
				this.Select(value, 0);
			}
		}

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x06007504 RID: 29956 RVA: 0x002EA3EB File Offset: 0x002E93EB
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int LineCount
		{
			get
			{
				if (base.RenderScope == null)
				{
					return -1;
				}
				return this.GetLineIndexFromCharacterIndex(base.TextContainer.SymbolCount) + 1;
			}
		}

		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x06007505 RID: 29957 RVA: 0x002EA40A File Offset: 0x002E940A
		// (set) Token: 0x06007506 RID: 29958 RVA: 0x002EA41C File Offset: 0x002E941C
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(TextBox.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(TextBox.TextDecorationsProperty, value);
			}
		}

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x06007507 RID: 29959 RVA: 0x0023D24E File Offset: 0x0023C24E
		public Typography Typography
		{
			get
			{
				return new Typography(this);
			}
		}

		// Token: 0x06007508 RID: 29960 RVA: 0x002EA42A File Offset: 0x002E942A
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TextBoxAutomationPeer(this);
		}

		// Token: 0x06007509 RID: 29961 RVA: 0x002EA434 File Offset: 0x002E9434
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (base.RenderScope != null)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Property.GetMetadata(typeof(TextBox)) as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null && (e.IsAValueChange || e.IsASubPropertyChange || e.Property == TextBox.TextAlignmentProperty))
				{
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange || e.Property == Control.HorizontalContentAlignmentProperty || e.Property == Control.VerticalContentAlignmentProperty)
					{
						((TextBoxView)base.RenderScope).Remeasure();
					}
					else if (frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender))
					{
						((TextBoxView)base.RenderScope).Rerender();
					}
					if (Speller.IsSpellerAffectingProperty(e.Property) && base.TextEditor.Speller != null)
					{
						base.TextEditor.Speller.ResetErrors();
					}
				}
			}
			TextBoxAutomationPeer textBoxAutomationPeer = UIElementAutomationPeer.FromElement(this) as TextBoxAutomationPeer;
			if (textBoxAutomationPeer != null)
			{
				if (e.Property == TextBox.TextProperty)
				{
					textBoxAutomationPeer.RaiseValuePropertyChangedEvent((string)e.OldValue, (string)e.NewValue);
				}
				if (e.Property == TextBoxBase.IsReadOnlyProperty)
				{
					textBoxAutomationPeer.RaiseIsReadOnlyPropertyChangedEvent((bool)e.OldValue, (bool)e.NewValue);
				}
			}
		}

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x0600750A RID: 29962 RVA: 0x002EA5A1 File Offset: 0x002E95A1
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new RangeContentEnumerator(base.TextContainer.Start, base.TextContainer.End);
			}
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x002EA5C0 File Offset: 0x002E95C0
		protected override Size MeasureOverride(Size constraint)
		{
			if (this.MinLines > 1 && this.MaxLines < this.MinLines)
			{
				throw new Exception(SR.Get("TextBoxMinMaxLinesMismatch"));
			}
			Size result = base.MeasureOverride(constraint);
			if (this._minmaxChanged)
			{
				if (base.ScrollViewer == null)
				{
					this.SetRenderScopeMinMaxHeight();
				}
				else
				{
					this.SetScrollViewerMinMaxHeight();
				}
				this._minmaxChanged = false;
			}
			return result;
		}

		// Token: 0x0600750C RID: 29964 RVA: 0x002EA620 File Offset: 0x002E9620
		internal void OnTextWrappingChanged()
		{
			base.CoerceValue(TextBoxBase.HorizontalScrollBarVisibilityProperty);
		}

		// Token: 0x0600750D RID: 29965 RVA: 0x002EA62D File Offset: 0x002E962D
		internal override FrameworkElement CreateRenderScope()
		{
			return new TextBoxView(this);
		}

		// Token: 0x0600750E RID: 29966 RVA: 0x002EA635 File Offset: 0x002E9635
		internal override void AttachToVisualTree()
		{
			base.AttachToVisualTree();
			if (base.RenderScope == null)
			{
				return;
			}
			this.OnTextWrappingChanged();
			this._minmaxChanged = true;
		}

		// Token: 0x0600750F RID: 29967 RVA: 0x002EA653 File Offset: 0x002E9653
		internal override string GetPlainText()
		{
			return this.Text;
		}

		// Token: 0x17001B31 RID: 6961
		// (get) Token: 0x06007510 RID: 29968 RVA: 0x002EA65B File Offset: 0x002E965B
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TextBox._dType;
			}
		}

		// Token: 0x06007511 RID: 29969 RVA: 0x002EA662 File Offset: 0x002E9662
		internal override void DoLineUp()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToVerticalOffset(base.VerticalOffset - this.GetLineHeight());
			}
		}

		// Token: 0x06007512 RID: 29970 RVA: 0x002EA684 File Offset: 0x002E9684
		internal override void DoLineDown()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToVerticalOffset(base.VerticalOffset + this.GetLineHeight());
			}
		}

		// Token: 0x06007513 RID: 29971 RVA: 0x002EA6A8 File Offset: 0x002E96A8
		internal override void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			bool flag = false;
			string text = null;
			try
			{
				this._changeEventNestingCount++;
				if (!this._isInsideTextContentChange)
				{
					this._isInsideTextContentChange = true;
					DeferredTextReference deferredTextReference = new DeferredTextReference(base.TextContainer);
					this._newTextValue = deferredTextReference;
					base.SetCurrentDeferredValue(TextBox.TextProperty, deferredTextReference);
				}
			}
			finally
			{
				this._changeEventNestingCount--;
				if (this._changeEventNestingCount == 0)
				{
					if (FrameworkCompatibilityPreferences.GetKeepTextBoxDisplaySynchronizedWithTextProperty())
					{
						text = (this._newTextValue as string);
						flag = (text != null && text != this.Text);
					}
					this._isInsideTextContentChange = false;
					this._newTextValue = DependencyProperty.UnsetValue;
				}
			}
			if (flag)
			{
				try
				{
					this._newTextValue = text;
					this._isInsideTextContentChange = true;
					this._changeEventNestingCount++;
					this.OnTextPropertyChanged(text, this.Text);
				}
				finally
				{
					this._changeEventNestingCount--;
					this._isInsideTextContentChange = false;
					this._newTextValue = DependencyProperty.UnsetValue;
				}
			}
			if (this._changeEventNestingCount == 0)
			{
				base.OnTextContainerChanged(sender, e);
			}
		}

		// Token: 0x06007514 RID: 29972 RVA: 0x002EA7C4 File Offset: 0x002E97C4
		internal void OnDeferredTextReferenceResolved(DeferredTextReference dtr, string s)
		{
			if (dtr == this._newTextValue)
			{
				this._newTextValue = s;
			}
		}

		// Token: 0x06007515 RID: 29973 RVA: 0x002EA7D6 File Offset: 0x002E97D6
		internal override void OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			base.OnScrollChanged(sender, e);
			if (e.ViewportHeightChange != 0.0)
			{
				this.SetScrollViewerMinMaxHeight();
			}
		}

		// Token: 0x06007516 RID: 29974 RVA: 0x002EA7F7 File Offset: 0x002E97F7
		internal void RaiseCourtesyTextChangedEvent()
		{
			this.OnTextChanged(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.None));
		}

		// Token: 0x17001B32 RID: 6962
		// (get) Token: 0x06007517 RID: 29975 RVA: 0x001FCA42 File Offset: 0x001FBA42
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x17001B33 RID: 6963
		// (get) Token: 0x06007518 RID: 29976 RVA: 0x002DCFC6 File Offset: 0x002DBFC6
		internal TextSelection Selection
		{
			get
			{
				return base.TextSelectionInternal;
			}
		}

		// Token: 0x17001B34 RID: 6964
		// (get) Token: 0x06007519 RID: 29977 RVA: 0x002EA80A File Offset: 0x002E980A
		internal TextPointer StartPosition
		{
			get
			{
				return base.TextContainer.Start;
			}
		}

		// Token: 0x17001B35 RID: 6965
		// (get) Token: 0x0600751A RID: 29978 RVA: 0x002EA817 File Offset: 0x002E9817
		internal TextPointer EndPosition
		{
			get
			{
				return base.TextContainer.End;
			}
		}

		// Token: 0x17001B36 RID: 6966
		// (get) Token: 0x0600751B RID: 29979 RVA: 0x002EA824 File Offset: 0x002E9824
		internal bool IsTypographyDefaultValue
		{
			get
			{
				return !this._isTypographySet;
			}
		}

		// Token: 0x17001B37 RID: 6967
		// (get) Token: 0x0600751C RID: 29980 RVA: 0x002EA82F File Offset: 0x002E982F
		ITextContainer ITextBoxViewHost.TextContainer
		{
			get
			{
				return base.TextContainer;
			}
		}

		// Token: 0x17001B38 RID: 6968
		// (get) Token: 0x0600751D RID: 29981 RVA: 0x002EA837 File Offset: 0x002E9837
		bool ITextBoxViewHost.IsTypographyDefaultValue
		{
			get
			{
				return this.IsTypographyDefaultValue;
			}
		}

		// Token: 0x0600751E RID: 29982 RVA: 0x002EA840 File Offset: 0x002E9840
		private bool GetRectangleFromTextPositionInternal(TextPointer position, bool relativeToTextBox, out Rect rect)
		{
			if (base.RenderScope == null)
			{
				rect = Rect.Empty;
				return false;
			}
			if (position.ValidateLayout())
			{
				rect = TextPointerBase.GetCharacterRect(position, position.LogicalDirection, relativeToTextBox);
			}
			else
			{
				rect = Rect.Empty;
			}
			return rect != Rect.Empty;
		}

		// Token: 0x0600751F RID: 29983 RVA: 0x002EA89C File Offset: 0x002E989C
		private TextPointer GetStartPositionOfLine(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			Point point = default(Point);
			double lineHeight = this.GetLineHeight();
			point.Y = lineHeight * (double)lineIndex + lineHeight / 2.0 - base.VerticalOffset;
			point.X = -base.HorizontalOffset;
			TextPointer textPointer;
			if (TextEditor.GetTextView(base.RenderScope).Validate(point))
			{
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetTextPositionFromPoint(point, true);
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetLineRange(textPointer).Start.CreatePointer(textPointer.LogicalDirection);
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06007520 RID: 29984 RVA: 0x002EA948 File Offset: 0x002E9948
		private TextPointer GetEndPositionOfLine(int lineIndex)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			Point point = default(Point);
			double lineHeight = this.GetLineHeight();
			point.Y = lineHeight * (double)lineIndex + lineHeight / 2.0 - base.VerticalOffset;
			point.X = 0.0;
			TextPointer textPointer;
			if (TextEditor.GetTextView(base.RenderScope).Validate(point))
			{
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetTextPositionFromPoint(point, true);
				textPointer = (TextPointer)TextEditor.GetTextView(base.RenderScope).GetLineRange(textPointer).End.CreatePointer(textPointer.LogicalDirection);
				if (TextPointerBase.IsNextToPlainLineBreak(textPointer, LogicalDirection.Forward))
				{
					textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				}
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06007521 RID: 29985 RVA: 0x002EAA08 File Offset: 0x002E9A08
		private static object CoerceHorizontalScrollBarVisibility(DependencyObject d, object value)
		{
			TextBox textBox = d as TextBox;
			if (textBox != null && (textBox.TextWrapping == TextWrapping.Wrap || textBox.TextWrapping == TextWrapping.WrapWithOverflow))
			{
				return ScrollBarVisibility.Disabled;
			}
			return value;
		}

		// Token: 0x06007522 RID: 29986 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool MaxLengthValidateValue(object value)
		{
			return (int)value >= 0;
		}

		// Token: 0x06007523 RID: 29987 RVA: 0x002EAA38 File Offset: 0x002E9A38
		private static bool CharacterCasingValidateValue(object value)
		{
			return CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper;
		}

		// Token: 0x06007524 RID: 29988 RVA: 0x0024392A File Offset: 0x0024292A
		private static bool MinLinesValidateValue(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x06007525 RID: 29989 RVA: 0x0024392A File Offset: 0x0024292A
		private static bool MaxLinesValidateValue(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x06007526 RID: 29990 RVA: 0x002EAA51 File Offset: 0x002E9A51
		private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TextBox)d)._minmaxChanged = true;
		}

		// Token: 0x06007527 RID: 29991 RVA: 0x002EAA60 File Offset: 0x002E9A60
		private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox textBox = (TextBox)d;
			if (textBox._isInsideTextContentChange && (textBox._newTextValue == DependencyProperty.UnsetValue || textBox._newTextValue is DeferredTextReference))
			{
				return;
			}
			textBox.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
		}

		// Token: 0x06007528 RID: 29992 RVA: 0x002EAAB8 File Offset: 0x002E9AB8
		private void OnTextPropertyChanged(string oldText, string newText)
		{
			bool flag = false;
			int start = 0;
			bool flag2 = false;
			if (this._isInsideTextContentChange)
			{
				if (object.Equals(this._newTextValue, newText))
				{
					return;
				}
				flag = true;
			}
			if (newText == null)
			{
				newText = string.Empty;
			}
			bool flag3 = base.HasExpression(base.LookupEntry(TextBox.TextProperty.GlobalIndex), TextBox.TextProperty);
			string oldText2 = oldText;
			if (flag)
			{
				flag2 = true;
				oldText2 = (string)this._newTextValue;
			}
			else if (flag3)
			{
				BindingExpressionBase bindingExpression = BindingOperations.GetBindingExpression(this, TextBox.TextProperty);
				flag2 = (bindingExpression != null && bindingExpression.IsInUpdate && bindingExpression.IsInTransfer);
			}
			if (flag2)
			{
				start = this.ChooseCaretIndex(this.CaretIndex, oldText2, newText);
			}
			if (flag)
			{
				this._newTextValue = newText;
			}
			this._isInsideTextContentChange = true;
			try
			{
				using (base.TextSelectionInternal.DeclareChangeBlock())
				{
					base.TextContainer.DeleteContentInternal(base.TextContainer.Start, base.TextContainer.End);
					base.TextContainer.End.InsertTextInRun(newText);
					this.Select(start, 0);
				}
			}
			finally
			{
				if (!flag)
				{
					this._isInsideTextContentChange = false;
				}
			}
			if (flag3)
			{
				UndoManager undoManager = base.TextEditor._GetUndoManager();
				if (undoManager != null && undoManager.IsEnabled)
				{
					undoManager.Clear();
				}
			}
		}

		// Token: 0x06007529 RID: 29993 RVA: 0x002EAC10 File Offset: 0x002E9C10
		private int ChooseCaretIndex(int oldIndex, string oldText, string newText)
		{
			int num = newText.IndexOf(oldText, StringComparison.Ordinal);
			if (oldText.Length > 0 && num >= 0)
			{
				return num + oldIndex;
			}
			if (oldIndex == 0)
			{
				return 0;
			}
			if (oldIndex == oldText.Length)
			{
				return newText.Length;
			}
			int num2 = 0;
			while (num2 < oldText.Length && num2 < newText.Length && oldText[num2] == newText[num2])
			{
				num2++;
			}
			int num3 = 0;
			while (num3 < oldText.Length && num3 < newText.Length && oldText[oldText.Length - 1 - num3] == newText[newText.Length - 1 - num3])
			{
				num3++;
			}
			if (2 * (num2 + num3) >= Math.Min(oldText.Length, newText.Length))
			{
				if (oldIndex <= num2)
				{
					return oldIndex;
				}
				if (oldIndex >= oldText.Length - num3)
				{
					return newText.Length - (oldText.Length - oldIndex);
				}
			}
			char value = oldText[oldIndex - 1];
			int i = newText.IndexOf(value);
			int num4 = -1;
			int num5 = 1;
			while (i >= 0)
			{
				int num6 = 1;
				num = i - 1;
				while (num >= 0 && oldIndex - (i - num) >= 0 && newText[num] == oldText[oldIndex - (i - num)])
				{
					num6++;
					num--;
				}
				num = i + 1;
				while (num < newText.Length && oldIndex + (num - i) < oldText.Length && newText[num] == oldText[oldIndex + (num - i)])
				{
					num6++;
					num++;
				}
				if (num6 > num5)
				{
					num4 = i + 1;
					num5 = num6;
				}
				i = newText.IndexOf(value, i + 1);
			}
			if (num4 >= 0)
			{
				return num4;
			}
			return newText.Length;
		}

		// Token: 0x0600752A RID: 29994 RVA: 0x002EADAF File Offset: 0x002E9DAF
		private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TextBox)
			{
				((TextBox)d).OnTextWrappingChanged();
			}
		}

		// Token: 0x0600752B RID: 29995 RVA: 0x002EADC4 File Offset: 0x002E9DC4
		private void SetScrollViewerMinMaxHeight()
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (base.ReadLocalValue(FrameworkElement.HeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MaxHeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MinHeightProperty) != DependencyProperty.UnsetValue)
			{
				base.ScrollViewer.ClearValue(FrameworkElement.MinHeightProperty);
				base.ScrollViewer.ClearValue(FrameworkElement.MaxHeightProperty);
				return;
			}
			double num = base.ScrollViewer.ActualHeight - base.ViewportHeight;
			double lineHeight = this.GetLineHeight();
			double num2 = num + lineHeight * (double)this.MinLines;
			if (this.MinLines > 1 && base.ScrollViewer.MinHeight != num2)
			{
				base.ScrollViewer.MinHeight = num2;
			}
			num2 = num + lineHeight * (double)this.MaxLines;
			if (this.MaxLines < 2147483647 && base.ScrollViewer.MaxHeight != num2)
			{
				base.ScrollViewer.MaxHeight = num2;
			}
		}

		// Token: 0x0600752C RID: 29996 RVA: 0x002EAEAC File Offset: 0x002E9EAC
		private void SetRenderScopeMinMaxHeight()
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (base.ReadLocalValue(FrameworkElement.HeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MaxHeightProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(FrameworkElement.MinHeightProperty) != DependencyProperty.UnsetValue)
			{
				base.RenderScope.ClearValue(FrameworkElement.MinHeightProperty);
				base.RenderScope.ClearValue(FrameworkElement.MaxHeightProperty);
				return;
			}
			double lineHeight = this.GetLineHeight();
			double num = lineHeight * (double)this.MinLines;
			if (this.MinLines > 1 && base.RenderScope.MinHeight != num)
			{
				base.RenderScope.MinHeight = num;
			}
			num = lineHeight * (double)this.MaxLines;
			if (this.MaxLines < 2147483647 && base.RenderScope.MaxHeight != num)
			{
				base.RenderScope.MaxHeight = num;
			}
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x002EAF7C File Offset: 0x002E9F7C
		private double GetLineHeight()
		{
			FontFamily fontFamily = (FontFamily)base.GetValue(Control.FontFamilyProperty);
			double num = (double)base.GetValue(TextElement.FontSizeProperty);
			double result;
			if (TextOptions.GetTextFormattingMode(this) == TextFormattingMode.Ideal)
			{
				result = fontFamily.LineSpacing * num;
			}
			else
			{
				result = fontFamily.GetLineSpacingForDisplayMode(num, base.GetDpi().DpiScaleY);
			}
			return result;
		}

		// Token: 0x0600752E RID: 29998 RVA: 0x002EAFD5 File Offset: 0x002E9FD5
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeText(XamlDesignerSerializationManager manager)
		{
			return manager.XmlWriter == null;
		}

		// Token: 0x0600752F RID: 29999 RVA: 0x0022A948 File Offset: 0x00229948
		private static void OnQueryScrollCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
		}

		// Token: 0x06007530 RID: 30000 RVA: 0x002EAFE0 File Offset: 0x002E9FE0
		private static object CoerceText(DependencyObject d, object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			return value;
		}

		// Token: 0x06007531 RID: 30001 RVA: 0x002EAFEC File Offset: 0x002E9FEC
		private static void OnTypographyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TextBox)d)._isTypographySet = true;
		}

		// Token: 0x04003823 RID: 14371
		public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(TextBox), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TextBox.OnTextWrappingChanged)));

		// Token: 0x04003824 RID: 14372
		public static readonly DependencyProperty MinLinesProperty = DependencyProperty.Register("MinLines", typeof(int), typeof(TextBox), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TextBox.OnMinMaxChanged)), new ValidateValueCallback(TextBox.MinLinesValidateValue));

		// Token: 0x04003825 RID: 14373
		public static readonly DependencyProperty MaxLinesProperty = DependencyProperty.Register("MaxLines", typeof(int), typeof(TextBox), new FrameworkPropertyMetadata(int.MaxValue, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TextBox.OnMinMaxChanged)), new ValidateValueCallback(TextBox.MaxLinesValidateValue));

		// Token: 0x04003826 RID: 14374
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(TextBox.OnTextPropertyChanged), new CoerceValueCallback(TextBox.CoerceText), true, UpdateSourceTrigger.LostFocus));

		// Token: 0x04003827 RID: 14375
		public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register("CharacterCasing", typeof(CharacterCasing), typeof(TextBox), new FrameworkPropertyMetadata(CharacterCasing.Normal), new ValidateValueCallback(TextBox.CharacterCasingValidateValue));

		// Token: 0x04003828 RID: 14376
		public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBox), new FrameworkPropertyMetadata(0), new ValidateValueCallback(TextBox.MaxLengthValidateValue));

		// Token: 0x04003829 RID: 14377
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextBox));

		// Token: 0x0400382A RID: 14378
		public static readonly DependencyProperty TextDecorationsProperty = Inline.TextDecorationsProperty.AddOwner(typeof(TextBox), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x0400382B RID: 14379
		private static DependencyObjectType _dType;

		// Token: 0x0400382C RID: 14380
		private bool _minmaxChanged;

		// Token: 0x0400382D RID: 14381
		private bool _isInsideTextContentChange;

		// Token: 0x0400382E RID: 14382
		private object _newTextValue = DependencyProperty.UnsetValue;

		// Token: 0x0400382F RID: 14383
		private bool _isTypographySet;

		// Token: 0x04003830 RID: 14384
		private int _changeEventNestingCount;
	}
}
