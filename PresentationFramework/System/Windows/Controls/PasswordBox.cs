using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007BB RID: 1979
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(FrameworkElement))]
	public sealed class PasswordBox : Control, ITextBoxViewHost
	{
		// Token: 0x06007082 RID: 28802 RVA: 0x002D7C80 File Offset: 0x002D6C80
		static PasswordBox()
		{
			PasswordBox.PasswordChangedEvent = EventManager.RegisterRoutedEvent("PasswordChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PasswordBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(typeof(PasswordBox)));
			PasswordBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(PasswordBox));
			PasswordBox.PasswordCharProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.OnPasswordCharChanged)));
			Control.PaddingProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.OnPaddingChanged)));
			NavigationService.NavigationServiceProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.OnParentNavigationServiceChanged)));
			InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(PasswordBox.ForceToFalse)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(PasswordBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(PasswordBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			TextBoxBase.SelectionTextBrushProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			TextBoxBase.CaretBrushProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			ControlsTraceLogger.AddControl(TelemetryControls.PasswordBox);
		}

		// Token: 0x06007083 RID: 28803 RVA: 0x002D7F4B File Offset: 0x002D6F4B
		public PasswordBox()
		{
			this.Initialize();
		}

		// Token: 0x06007084 RID: 28804 RVA: 0x002D7F59 File Offset: 0x002D6F59
		public void Paste()
		{
			ApplicationCommands.Paste.Execute(null, this);
		}

		// Token: 0x06007085 RID: 28805 RVA: 0x002D7F67 File Offset: 0x002D6F67
		public void SelectAll()
		{
			this.Selection.Select(this.TextContainer.Start, this.TextContainer.End);
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x002D7F8A File Offset: 0x002D6F8A
		public void Clear()
		{
			this.Password = string.Empty;
		}

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x06007087 RID: 28807 RVA: 0x002D7F98 File Offset: 0x002D6F98
		// (set) Token: 0x06007088 RID: 28808 RVA: 0x002D7FF0 File Offset: 0x002D6FF0
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public unsafe string Password
		{
			get
			{
				string result;
				using (SecureString securePassword = this.SecurePassword)
				{
					IntPtr intPtr = Marshal.SecureStringToBSTR(securePassword);
					try
					{
						result = new string((char*)((void*)intPtr));
					}
					finally
					{
						Marshal.ZeroFreeBSTR(intPtr);
					}
				}
				return result;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				using (SecureString secureString = new SecureString())
				{
					for (int i = 0; i < value.Length; i++)
					{
						secureString.AppendChar(value[i]);
					}
					this.SetSecurePassword(secureString);
				}
			}
		}

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x06007089 RID: 28809 RVA: 0x002D8050 File Offset: 0x002D7050
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SecureString SecurePassword
		{
			get
			{
				return this.TextContainer.GetPasswordCopy();
			}
		}

		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x0600708A RID: 28810 RVA: 0x002D805D File Offset: 0x002D705D
		// (set) Token: 0x0600708B RID: 28811 RVA: 0x002D806F File Offset: 0x002D706F
		public char PasswordChar
		{
			get
			{
				return (char)base.GetValue(PasswordBox.PasswordCharProperty);
			}
			set
			{
				base.SetValue(PasswordBox.PasswordCharProperty, value);
			}
		}

		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x0600708C RID: 28812 RVA: 0x002D8082 File Offset: 0x002D7082
		// (set) Token: 0x0600708D RID: 28813 RVA: 0x002D8094 File Offset: 0x002D7094
		[DefaultValue(0)]
		public int MaxLength
		{
			get
			{
				return (int)base.GetValue(PasswordBox.MaxLengthProperty);
			}
			set
			{
				base.SetValue(PasswordBox.MaxLengthProperty, value);
			}
		}

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x0600708E RID: 28814 RVA: 0x002D80A7 File Offset: 0x002D70A7
		// (set) Token: 0x0600708F RID: 28815 RVA: 0x002D80B9 File Offset: 0x002D70B9
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(PasswordBox.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(PasswordBox.SelectionBrushProperty, value);
			}
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x06007090 RID: 28816 RVA: 0x002D80C7 File Offset: 0x002D70C7
		// (set) Token: 0x06007091 RID: 28817 RVA: 0x002D80D9 File Offset: 0x002D70D9
		public Brush SelectionTextBrush
		{
			get
			{
				return (Brush)base.GetValue(PasswordBox.SelectionTextBrushProperty);
			}
			set
			{
				base.SetValue(PasswordBox.SelectionTextBrushProperty, value);
			}
		}

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x06007092 RID: 28818 RVA: 0x002D80E7 File Offset: 0x002D70E7
		// (set) Token: 0x06007093 RID: 28819 RVA: 0x002D80F9 File Offset: 0x002D70F9
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(PasswordBox.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(PasswordBox.SelectionOpacityProperty, value);
			}
		}

		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x06007094 RID: 28820 RVA: 0x002D810C File Offset: 0x002D710C
		// (set) Token: 0x06007095 RID: 28821 RVA: 0x002D811E File Offset: 0x002D711E
		public Brush CaretBrush
		{
			get
			{
				return (Brush)base.GetValue(PasswordBox.CaretBrushProperty);
			}
			set
			{
				base.SetValue(PasswordBox.CaretBrushProperty, value);
			}
		}

		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x06007096 RID: 28822 RVA: 0x002D812C File Offset: 0x002D712C
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(PasswordBox.IsSelectionActiveProperty);
			}
		}

		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x06007097 RID: 28823 RVA: 0x002D813E File Offset: 0x002D713E
		// (set) Token: 0x06007098 RID: 28824 RVA: 0x002D8150 File Offset: 0x002D7150
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(PasswordBox.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(PasswordBox.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x06007099 RID: 28825 RVA: 0x002D815E File Offset: 0x002D715E
		// (remove) Token: 0x0600709A RID: 28826 RVA: 0x002D816C File Offset: 0x002D716C
		public event RoutedEventHandler PasswordChanged
		{
			add
			{
				base.AddHandler(PasswordBox.PasswordChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(PasswordBox.PasswordChangedEvent, value);
			}
		}

		// Token: 0x0600709B RID: 28827 RVA: 0x002D817C File Offset: 0x002D717C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x0600709C RID: 28828 RVA: 0x002D821F File Offset: 0x002D721F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new PasswordBoxAutomationPeer(this);
		}

		// Token: 0x0600709D RID: 28829 RVA: 0x002D8227 File Offset: 0x002D7227
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.AttachToVisualTree();
		}

		// Token: 0x0600709E RID: 28830 RVA: 0x002D8235 File Offset: 0x002D7235
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			if (oldTemplate != null && newTemplate != null && oldTemplate.VisualTree != newTemplate.VisualTree)
			{
				this.DetachFromVisualTree();
			}
		}

		// Token: 0x0600709F RID: 28831 RVA: 0x002D825C File Offset: 0x002D725C
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (this.RenderScope != null)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Property.GetMetadata(typeof(PasswordBox)) as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null && (e.IsAValueChange || e.IsASubPropertyChange))
				{
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange || e.Property == Control.HorizontalContentAlignmentProperty || e.Property == Control.VerticalContentAlignmentProperty)
					{
						((TextBoxView)this.RenderScope).Remeasure();
						return;
					}
					if (frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender))
					{
						((TextBoxView)this.RenderScope).Rerender();
					}
				}
			}
		}

		// Token: 0x060070A0 RID: 28832 RVA: 0x002D8325 File Offset: 0x002D7325
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnKeyDown(e);
		}

		// Token: 0x060070A1 RID: 28833 RVA: 0x002D8343 File Offset: 0x002D7343
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnKeyUp(e);
		}

		// Token: 0x060070A2 RID: 28834 RVA: 0x002D8361 File Offset: 0x002D7361
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnTextInput(e);
		}

		// Token: 0x060070A3 RID: 28835 RVA: 0x002D837F File Offset: 0x002D737F
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnMouseDown(e);
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x002D839D File Offset: 0x002D739D
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnMouseMove(e);
		}

		// Token: 0x060070A5 RID: 28837 RVA: 0x002D83BB File Offset: 0x002D73BB
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnMouseUp(e);
		}

		// Token: 0x060070A6 RID: 28838 RVA: 0x002D83D9 File Offset: 0x002D73D9
		protected override void OnQueryCursor(QueryCursorEventArgs e)
		{
			base.OnQueryCursor(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnQueryCursor(e);
		}

		// Token: 0x060070A7 RID: 28839 RVA: 0x002D83F7 File Offset: 0x002D73F7
		protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			base.OnQueryContinueDrag(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnQueryContinueDrag(e);
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x002D8415 File Offset: 0x002D7415
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			base.OnGiveFeedback(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnGiveFeedback(e);
		}

		// Token: 0x060070A9 RID: 28841 RVA: 0x002D8433 File Offset: 0x002D7433
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDragEnter(e);
		}

		// Token: 0x060070AA RID: 28842 RVA: 0x002D8451 File Offset: 0x002D7451
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDragOver(e);
		}

		// Token: 0x060070AB RID: 28843 RVA: 0x002D846F File Offset: 0x002D746F
		protected override void OnDragLeave(DragEventArgs e)
		{
			base.OnDragLeave(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDragLeave(e);
		}

		// Token: 0x060070AC RID: 28844 RVA: 0x002D848D File Offset: 0x002D748D
		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDrop(e);
		}

		// Token: 0x060070AD RID: 28845 RVA: 0x002D84AB File Offset: 0x002D74AB
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnContextMenuOpening(e);
		}

		// Token: 0x060070AE RID: 28846 RVA: 0x002D84C9 File Offset: 0x002D74C9
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnGotKeyboardFocus(e);
		}

		// Token: 0x060070AF RID: 28847 RVA: 0x002D84E7 File Offset: 0x002D74E7
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnLostKeyboardFocus(e);
		}

		// Token: 0x060070B0 RID: 28848 RVA: 0x002D8505 File Offset: 0x002D7505
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnLostFocus(e);
		}

		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x060070B1 RID: 28849 RVA: 0x002D8523 File Offset: 0x002D7523
		internal PasswordTextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x060070B2 RID: 28850 RVA: 0x002D852B File Offset: 0x002D752B
		internal FrameworkElement RenderScope
		{
			get
			{
				return this._renderScope;
			}
		}

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x060070B3 RID: 28851 RVA: 0x002D8533 File Offset: 0x002D7533
		internal ScrollViewer ScrollViewer
		{
			get
			{
				if (this._scrollViewer == null && this._textEditor != null)
				{
					this._scrollViewer = (this._textEditor._Scroller as ScrollViewer);
				}
				return this._scrollViewer;
			}
		}

		// Token: 0x17001A10 RID: 6672
		// (get) Token: 0x060070B4 RID: 28852 RVA: 0x002D8561 File Offset: 0x002D7561
		ITextContainer ITextBoxViewHost.TextContainer
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x060070B5 RID: 28853 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ITextBoxViewHost.IsTypographyDefaultValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060070B6 RID: 28854 RVA: 0x002D8569 File Offset: 0x002D7569
		private void Initialize()
		{
			TextEditor.RegisterCommandHandlers(typeof(PasswordBox), false, false, false);
			this.InitializeTextContainer(new PasswordTextContainer(this));
			this._textEditor.AcceptsRichContent = false;
			this._textEditor.AcceptsTab = false;
		}

		// Token: 0x060070B7 RID: 28855 RVA: 0x002D85A4 File Offset: 0x002D75A4
		private void InitializeTextContainer(PasswordTextContainer textContainer)
		{
			Invariant.Assert(textContainer != null);
			if (this._textContainer != null)
			{
				Invariant.Assert(this._textEditor != null);
				Invariant.Assert(this._textEditor.TextContainer == this._textContainer);
				this.DetachFromVisualTree();
				this._textEditor.OnDetach();
			}
			this._textContainer = textContainer;
			((ITextContainer)this._textContainer).Changed += this.OnTextContainerChanged;
			this._textEditor = new TextEditor(this._textContainer, this, true);
		}

		// Token: 0x060070B8 RID: 28856 RVA: 0x002D862A File Offset: 0x002D762A
		private static object ForceToFalse(DependencyObject d, object value)
		{
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x060070B9 RID: 28857 RVA: 0x002D8634 File Offset: 0x002D7634
		private static void OnPasswordCharChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			if (passwordBox._renderScope != null)
			{
				passwordBox._renderScope.InvalidateMeasure();
			}
		}

		// Token: 0x060070BA RID: 28858 RVA: 0x002D865B File Offset: 0x002D765B
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			if (!e.HasContentAddedOrRemoved)
			{
				return;
			}
			base.RaiseEvent(new RoutedEventArgs(PasswordBox.PasswordChangedEvent));
		}

		// Token: 0x060070BB RID: 28859 RVA: 0x002D8678 File Offset: 0x002D7678
		private void SetRenderScopeToContentHost(TextBoxView renderScope)
		{
			this.ClearContentHost();
			this._passwordBoxContentHost = (base.GetTemplateChild("PART_ContentHost") as FrameworkElement);
			this._renderScope = renderScope;
			if (this._passwordBoxContentHost is ScrollViewer)
			{
				ScrollViewer scrollViewer = (ScrollViewer)this._passwordBoxContentHost;
				if (scrollViewer.Content != null)
				{
					throw new NotSupportedException(SR.Get("TextBoxScrollViewerMarkedAsTextBoxContentMustHaveNoContent"));
				}
				scrollViewer.Content = this._renderScope;
			}
			else if (this._passwordBoxContentHost is Decorator)
			{
				Decorator decorator = (Decorator)this._passwordBoxContentHost;
				if (decorator.Child != null)
				{
					throw new NotSupportedException(SR.Get("TextBoxDecoratorMarkedAsTextBoxContentMustHaveNoContent"));
				}
				decorator.Child = this._renderScope;
			}
			else
			{
				this._renderScope = null;
				if (this._passwordBoxContentHost != null)
				{
					this._passwordBoxContentHost = null;
					throw new NotSupportedException(SR.Get("PasswordBoxInvalidTextContainer"));
				}
			}
			this.InitializeRenderScope();
			FrameworkElement frameworkElement = this._renderScope;
			while (frameworkElement != this && frameworkElement != null)
			{
				if (frameworkElement is Border)
				{
					this._border = (Border)frameworkElement;
				}
				frameworkElement = (frameworkElement.Parent as FrameworkElement);
			}
		}

		// Token: 0x060070BC RID: 28860 RVA: 0x002D8784 File Offset: 0x002D7784
		private void ClearContentHost()
		{
			this.UninitializeRenderScope();
			if (this._passwordBoxContentHost is ScrollViewer)
			{
				((ScrollViewer)this._passwordBoxContentHost).Content = null;
			}
			else if (this._passwordBoxContentHost is Decorator)
			{
				((Decorator)this._passwordBoxContentHost).Child = null;
			}
			else
			{
				Invariant.Assert(this._passwordBoxContentHost == null, "_passwordBoxContentHost must be null here");
			}
			this._passwordBoxContentHost = null;
		}

		// Token: 0x060070BD RID: 28861 RVA: 0x002D87F4 File Offset: 0x002D77F4
		private void InitializeRenderScope()
		{
			if (this._renderScope == null)
			{
				return;
			}
			ITextView textView = TextEditor.GetTextView(this._renderScope);
			this._textEditor.TextView = textView;
			this.TextContainer.TextView = textView;
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.CanContentScroll = true;
			}
		}

		// Token: 0x060070BE RID: 28862 RVA: 0x002D8842 File Offset: 0x002D7842
		private void UninitializeRenderScope()
		{
			this._textEditor.TextView = null;
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x002D8850 File Offset: 0x002D7850
		private void ResetSelection()
		{
			this.Select(0, 0);
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x060070C0 RID: 28864 RVA: 0x002D8870 File Offset: 0x002D7870
		private void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.Get("ParameterCannotBeNegative"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", SR.Get("ParameterCannotBeNegative"));
			}
			ITextPointer textPointer = this.TextContainer.Start.CreatePointer();
			while (start-- > 0 && textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
			{
			}
			ITextPointer textPointer2 = textPointer.CreatePointer();
			while (length-- > 0 && textPointer2.MoveToNextInsertionPosition(LogicalDirection.Forward))
			{
			}
			this.Selection.Select(textPointer, textPointer2);
		}

		// Token: 0x060070C1 RID: 28865 RVA: 0x002D88F8 File Offset: 0x002D78F8
		private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			if (passwordBox.ScrollViewer != null)
			{
				object value = passwordBox.GetValue(Control.PaddingProperty);
				if (value is Thickness)
				{
					passwordBox.ScrollViewer.Padding = (Thickness)value;
					return;
				}
				passwordBox.ScrollViewer.ClearValue(Control.PaddingProperty);
			}
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x002D894C File Offset: 0x002D794C
		private static void OnParentNavigationServiceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)o;
			NavigationService navigationService = NavigationService.GetNavigationService(o);
			if (passwordBox._navigationService != null)
			{
				passwordBox._navigationService.Navigating -= passwordBox.OnNavigating;
			}
			if (navigationService != null)
			{
				navigationService.Navigating += passwordBox.OnNavigating;
				passwordBox._navigationService = navigationService;
				return;
			}
			passwordBox._navigationService = null;
		}

		// Token: 0x060070C3 RID: 28867 RVA: 0x002D7F8A File Offset: 0x002D6F8A
		private void OnNavigating(object sender, NavigatingCancelEventArgs e)
		{
			this.Password = string.Empty;
		}

		// Token: 0x060070C4 RID: 28868 RVA: 0x002D89AC File Offset: 0x002D79AC
		private void AttachToVisualTree()
		{
			this.DetachFromVisualTree();
			this.SetRenderScopeToContentHost(new TextBoxView(this));
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
				this.ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
				this.ScrollViewer.Focusable = false;
				if (this.ScrollViewer.Background == null)
				{
					this.ScrollViewer.Background = Brushes.Transparent;
				}
				PasswordBox.OnPaddingChanged(this, default(DependencyPropertyChangedEventArgs));
			}
			if (this._border != null)
			{
				this._border.Style = null;
			}
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x002D8A37 File Offset: 0x002D7A37
		private void DetachFromVisualTree()
		{
			if (this._textEditor != null)
			{
				this._textEditor.Selection.DetachFromVisualTree();
			}
			this._scrollViewer = null;
			this._border = null;
			this.ClearContentHost();
		}

		// Token: 0x060070C6 RID: 28870 RVA: 0x002D8A68 File Offset: 0x002D7A68
		private void SetSecurePassword(SecureString value)
		{
			this.TextContainer.BeginChange();
			try
			{
				this.TextContainer.SetPassword(value);
				this.ResetSelection();
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x060070C7 RID: 28871 RVA: 0x002D8AB0 File Offset: 0x002D7AB0
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			if (passwordBox.Selection != null)
			{
				CaretElement caretElement = passwordBox.Selection.CaretElement;
				if (caretElement != null)
				{
					if (e.Property == PasswordBox.CaretBrushProperty)
					{
						caretElement.UpdateCaretBrush(TextSelection.GetCaretBrush(passwordBox.Selection.TextEditor));
					}
					caretElement.InvalidateVisual();
				}
				TextBoxView textBoxView = ((passwordBox != null) ? passwordBox.RenderScope : null) as TextBoxView;
				TextBoxView textBoxView2 = textBoxView;
				if (textBoxView2 != null && ((ITextView)textBoxView2).RendersOwnSelection)
				{
					textBoxView.InvalidateArrange();
				}
			}
		}

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x060070C8 RID: 28872 RVA: 0x002D8B2C File Offset: 0x002D7B2C
		private ITextSelection Selection
		{
			get
			{
				return this._textEditor.Selection;
			}
		}

		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x060070C9 RID: 28873 RVA: 0x002D8B39 File Offset: 0x002D7B39
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return PasswordBox._dType;
			}
		}

		// Token: 0x040036EF RID: 14063
		public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.RegisterAttached("PasswordChar", typeof(char), typeof(PasswordBox), new FrameworkPropertyMetadata('*'));

		// Token: 0x040036F0 RID: 14064
		public static readonly DependencyProperty MaxLengthProperty = TextBox.MaxLengthProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x040036F1 RID: 14065
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x040036F2 RID: 14066
		public static readonly DependencyProperty SelectionTextBrushProperty = TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x040036F3 RID: 14067
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x040036F4 RID: 14068
		public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x040036F5 RID: 14069
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x040036F6 RID: 14070
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x040036F8 RID: 14072
		private TextEditor _textEditor;

		// Token: 0x040036F9 RID: 14073
		private PasswordTextContainer _textContainer;

		// Token: 0x040036FA RID: 14074
		private TextBoxView _renderScope;

		// Token: 0x040036FB RID: 14075
		private ScrollViewer _scrollViewer;

		// Token: 0x040036FC RID: 14076
		private Border _border;

		// Token: 0x040036FD RID: 14077
		private FrameworkElement _passwordBoxContentHost;

		// Token: 0x040036FE RID: 14078
		private const int _defaultWidth = 100;

		// Token: 0x040036FF RID: 14079
		private const int _defaultHeight = 20;

		// Token: 0x04003700 RID: 14080
		private const string ContentHostTemplateName = "PART_ContentHost";

		// Token: 0x04003701 RID: 14081
		private static DependencyObjectType _dType;

		// Token: 0x04003702 RID: 14082
		private NavigationService _navigationService;
	}
}
