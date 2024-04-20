using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007C8 RID: 1992
	[ContentProperty("Document")]
	[Localizability(LocalizationCategory.Inherit)]
	public class RichTextBox : TextBoxBase, IAddChild
	{
		// Token: 0x06007208 RID: 29192 RVA: 0x002DC9E4 File Offset: 0x002DB9E4
		static RichTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(typeof(RichTextBox)));
			RichTextBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(RichTextBox));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(true));
			TextBoxBase.AutoWordSelectionProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(true));
			if (!FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering)
			{
				TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(0.4));
			}
			RichTextBox.HookupInheritablePropertyListeners();
			ControlsTraceLogger.AddControl(TelemetryControls.RichTextBox);
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x002DCADE File Offset: 0x002DBADE
		public RichTextBox() : this(null)
		{
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x002DCAE8 File Offset: 0x002DBAE8
		public RichTextBox(FlowDocument document)
		{
			TextEditor.RegisterCommandHandlers(typeof(RichTextBox), true, false, false);
			if (document == null)
			{
				document = new FlowDocument();
				document.Blocks.Add(new Paragraph());
				this._implicitDocument = true;
			}
			this.Document = document;
			Invariant.Assert(base.TextContainer != null);
			Invariant.Assert(base.TextEditor != null);
			Invariant.Assert(base.TextEditor.TextContainer == base.TextContainer);
		}

		// Token: 0x0600720B RID: 29195 RVA: 0x002DCB6C File Offset: 0x002DBB6C
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is FlowDocument))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(FlowDocument)
				}), "value");
			}
			if (!this._implicitDocument)
			{
				throw new ArgumentException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					base.GetType(),
					value.GetType()
				}));
			}
			this.Document = (FlowDocument)value;
		}

		// Token: 0x0600720C RID: 29196 RVA: 0x001F091A File Offset: 0x001EF91A
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x0600720D RID: 29197 RVA: 0x002DCBFC File Offset: 0x002DBBFC
		public TextPointer GetPositionFromPoint(Point point, bool snapToText)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			return base.GetTextPositionFromPointInternal(point, snapToText);
		}

		// Token: 0x0600720E RID: 29198 RVA: 0x002DCC10 File Offset: 0x002DBC10
		public SpellingError GetSpellingError(TextPointer position)
		{
			ValidationHelper.VerifyPosition(base.TextContainer, position);
			return base.TextEditor.GetSpellingErrorAtPosition(position, position.LogicalDirection);
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x002DCC30 File Offset: 0x002DBC30
		public TextRange GetSpellingErrorRange(TextPointer position)
		{
			ValidationHelper.VerifyPosition(base.TextContainer, position);
			SpellingError spellingErrorAtPosition = base.TextEditor.GetSpellingErrorAtPosition(position, position.LogicalDirection);
			if (spellingErrorAtPosition != null)
			{
				return new TextRange(spellingErrorAtPosition.Start, spellingErrorAtPosition.End);
			}
			return null;
		}

		// Token: 0x06007210 RID: 29200 RVA: 0x002DCC72 File Offset: 0x002DBC72
		public TextPointer GetNextSpellingErrorPosition(TextPointer position, LogicalDirection direction)
		{
			ValidationHelper.VerifyPosition(base.TextContainer, position);
			return (TextPointer)base.TextEditor.GetNextSpellingErrorPosition(position, direction);
		}

		// Token: 0x06007211 RID: 29201 RVA: 0x002DCC92 File Offset: 0x002DBC92
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RichTextBoxAutomationPeer(this);
		}

		// Token: 0x06007212 RID: 29202 RVA: 0x002DCC9A File Offset: 0x002DBC9A
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			FlowDocument document = this.Document;
			if (document == null)
			{
				return;
			}
			document.SetDpi(newDpiScaleInfo);
		}

		// Token: 0x06007213 RID: 29203 RVA: 0x002DCCAD File Offset: 0x002DBCAD
		protected override Size MeasureOverride(Size constraint)
		{
			if (constraint.Width == double.PositiveInfinity)
			{
				constraint.Width = base.MinWidth;
			}
			return base.MeasureOverride(constraint);
		}

		// Token: 0x06007214 RID: 29204 RVA: 0x002DCCD8 File Offset: 0x002DBCD8
		internal override FrameworkElement CreateRenderScope()
		{
			return new FlowDocumentView
			{
				Document = this.Document,
				Document = 
				{
					PagePadding = new Thickness(5.0, 0.0, 5.0, 0.0)
				},
				OverridesDefaultStyle = true
			};
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x06007215 RID: 29205 RVA: 0x002DCD31 File Offset: 0x002DBD31
		// (set) Token: 0x06007216 RID: 29206 RVA: 0x002DCD48 File Offset: 0x002DBD48
		public FlowDocument Document
		{
			get
			{
				Invariant.Assert(this._document != null);
				return this._document;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value != this._document && value.StructuralCache != null && value.StructuralCache.TextContainer != null && value.StructuralCache.TextContainer.TextSelection != null)
				{
					throw new ArgumentException(SR.Get("RichTextBox_DocumentBelongsToAnotherRichTextBoxAlready"));
				}
				if (this._document != null && base.TextSelectionInternal.ChangeBlockLevel > 0)
				{
					throw new InvalidOperationException(SR.Get("RichTextBox_CantSetDocumentInsideChangeBlock"));
				}
				if (value == this._document)
				{
					return;
				}
				bool flag = this._document == null;
				if (this._document != null)
				{
					this._document.PageSizeChanged -= this.OnPageSizeChangedHandler;
					base.RemoveLogicalChild(this._document);
					this._document.TextContainer.CollectTextChanges = false;
					this._document = null;
				}
				if (!flag)
				{
					this._implicitDocument = false;
				}
				this._document = value;
				this._document.SetDpi(base.GetDpi());
				bool renderScope = base.RenderScope != null;
				this._document.TextContainer.CollectTextChanges = true;
				base.InitializeTextContainer(this._document.TextContainer);
				this._document.PageSizeChanged += this.OnPageSizeChangedHandler;
				base.AddLogicalChild(this._document);
				if (renderScope)
				{
					this.AttachToVisualTree();
				}
				this.TransferInheritedPropertiesToFlowDocument();
				if (!flag)
				{
					base.ChangeUndoLimit(base.UndoLimit);
					base.ChangeUndoEnabled(base.IsUndoEnabled);
					Invariant.Assert(base.PendingUndoAction == UndoAction.None);
					base.PendingUndoAction = UndoAction.Clear;
					try
					{
						this.OnTextChanged(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.Clear));
					}
					finally
					{
						base.PendingUndoAction = UndoAction.None;
					}
				}
			}
		}

		// Token: 0x06007217 RID: 29207 RVA: 0x002DCEFC File Offset: 0x002DBEFC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeDocument()
		{
			Block firstBlock = this._document.Blocks.FirstBlock;
			if (this._implicitDocument && (firstBlock == null || (firstBlock == this._document.Blocks.LastBlock && firstBlock is Paragraph)))
			{
				Inline inline = (firstBlock == null) ? null : ((Paragraph)firstBlock).Inlines.FirstInline;
				if (inline == null || (inline == ((Paragraph)firstBlock).Inlines.LastInline && inline is Run && inline.ContentStart.CompareTo(inline.ContentEnd) == 0))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x06007218 RID: 29208 RVA: 0x002DCF8B File Offset: 0x002DBF8B
		// (set) Token: 0x06007219 RID: 29209 RVA: 0x002DCF9D File Offset: 0x002DBF9D
		public bool IsDocumentEnabled
		{
			get
			{
				return (bool)base.GetValue(RichTextBox.IsDocumentEnabledProperty);
			}
			set
			{
				base.SetValue(RichTextBox.IsDocumentEnabledProperty, value);
			}
		}

		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x0600721A RID: 29210 RVA: 0x002DCFAB File Offset: 0x002DBFAB
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this._document == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new SingleChildEnumerator(this._document);
			}
		}

		// Token: 0x17001A6C RID: 6764
		// (get) Token: 0x0600721B RID: 29211 RVA: 0x002DCFC6 File Offset: 0x002DBFC6
		public TextSelection Selection
		{
			get
			{
				return base.TextSelectionInternal;
			}
		}

		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x0600721C RID: 29212 RVA: 0x002DCFCE File Offset: 0x002DBFCE
		// (set) Token: 0x0600721D RID: 29213 RVA: 0x002DCFDC File Offset: 0x002DBFDC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextPointer CaretPosition
		{
			get
			{
				return this.Selection.MovingPosition;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!this.Selection.Start.IsInSameDocument(value))
				{
					throw new ArgumentException(SR.Get("RichTextBox_PointerNotInSameDocument"), "value");
				}
				this.Selection.SetCaretToPosition(value, value.LogicalDirection, true, false);
			}
		}

		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x0600721E RID: 29214 RVA: 0x002DD033 File Offset: 0x002DC033
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return RichTextBox._dType;
			}
		}

		// Token: 0x0600721F RID: 29215 RVA: 0x002DD03C File Offset: 0x002DC03C
		private static void HookupInheritablePropertyListeners()
		{
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(RichTextBox.OnFormattingPropertyChanged);
			DependencyProperty[] inheritableProperties = TextSchema.GetInheritableProperties(typeof(FlowDocument));
			for (int i = 0; i < inheritableProperties.Length; i++)
			{
				inheritableProperties[i].OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			}
			PropertyChangedCallback propertyChangedCallback2 = new PropertyChangedCallback(RichTextBox.OnBehavioralPropertyChanged);
			DependencyProperty[] behavioralProperties = TextSchema.BehavioralProperties;
			for (int j = 0; j < behavioralProperties.Length; j++)
			{
				behavioralProperties[j].OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(propertyChangedCallback2));
			}
		}

		// Token: 0x06007220 RID: 29216 RVA: 0x002DD0D0 File Offset: 0x002DC0D0
		private void TransferInheritedPropertiesToFlowDocument()
		{
			if (this._implicitDocument)
			{
				foreach (DependencyProperty dependencyProperty in TextSchema.GetInheritableProperties(typeof(FlowDocument)))
				{
					this.TransferFormattingProperty(dependencyProperty, base.GetValue(dependencyProperty));
				}
			}
			foreach (DependencyProperty dependencyProperty2 in TextSchema.BehavioralProperties)
			{
				this.TransferBehavioralProperty(dependencyProperty2, base.GetValue(dependencyProperty2));
			}
		}

		// Token: 0x06007221 RID: 29217 RVA: 0x002DD144 File Offset: 0x002DC144
		private static void OnFormattingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RichTextBox richTextBox = (RichTextBox)d;
			if (richTextBox._implicitDocument)
			{
				richTextBox.TransferFormattingProperty(e.Property, e.NewValue);
			}
		}

		// Token: 0x06007222 RID: 29218 RVA: 0x002DD174 File Offset: 0x002DC174
		private void TransferFormattingProperty(DependencyProperty property, object inheritedValue)
		{
			Invariant.Assert(this._implicitDocument, "We only supposed to do this for implicit documents");
			object value = this._document.GetValue(property);
			if (!TextSchema.ValuesAreEqual(inheritedValue, value))
			{
				this._document.ClearValue(property);
				value = this._document.GetValue(property);
				if (!TextSchema.ValuesAreEqual(inheritedValue, value))
				{
					this._document.SetValue(property, inheritedValue);
				}
			}
		}

		// Token: 0x06007223 RID: 29219 RVA: 0x002DD1D6 File Offset: 0x002DC1D6
		private static void OnBehavioralPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((RichTextBox)d).TransferBehavioralProperty(e.Property, e.NewValue);
		}

		// Token: 0x06007224 RID: 29220 RVA: 0x002DD1F1 File Offset: 0x002DC1F1
		private void TransferBehavioralProperty(DependencyProperty property, object inheritedValue)
		{
			this._document.SetValue(property, inheritedValue);
		}

		// Token: 0x06007225 RID: 29221 RVA: 0x002DD200 File Offset: 0x002DC200
		private void OnPageSizeChangedHandler(object sender, EventArgs e)
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (this.Document != null)
			{
				this.Document.TextWrapping = TextWrapping.Wrap;
			}
			base.RenderScope.ClearValue(FrameworkElement.WidthProperty);
			base.RenderScope.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
			if (base.RenderScope.HorizontalAlignment != HorizontalAlignment.Stretch)
			{
				base.RenderScope.HorizontalAlignment = HorizontalAlignment.Stretch;
			}
		}

		// Token: 0x06007226 RID: 29222 RVA: 0x002DD264 File Offset: 0x002DC264
		private static void OnIsDocumentEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RichTextBox richTextBox = (RichTextBox)d;
			if (richTextBox.Document != null)
			{
				richTextBox.Document.CoerceValue(UIElement.IsEnabledProperty);
			}
		}

		// Token: 0x04003751 RID: 14161
		public static readonly DependencyProperty IsDocumentEnabledProperty = DependencyProperty.Register("IsDocumentEnabled", typeof(bool), typeof(RichTextBox), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(RichTextBox.OnIsDocumentEnabledChanged)));

		// Token: 0x04003752 RID: 14162
		private FlowDocument _document;

		// Token: 0x04003753 RID: 14163
		private bool _implicitDocument;

		// Token: 0x04003754 RID: 14164
		private static DependencyObjectType _dType;
	}
}
