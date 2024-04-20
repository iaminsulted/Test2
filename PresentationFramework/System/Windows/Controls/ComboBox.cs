using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Win32;

namespace System.Windows.Controls
{
	// Token: 0x02000731 RID: 1841
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[TemplatePart(Name = "PART_EditableTextBox", Type = typeof(TextBox))]
	[Localizability(LocalizationCategory.ComboBox)]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ComboBoxItem))]
	public class ComboBox : Selector
	{
		// Token: 0x060060AD RID: 24749 RVA: 0x0029A3B0 File Offset: 0x002993B0
		static ComboBox()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ComboBox.CoerceToolTipIsEnabled)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(typeof(ComboBox)));
			ComboBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ComboBox));
			ItemsControl.IsTextSearchEnabledProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.LostMouseCaptureEvent, new MouseEventHandler(ComboBox.OnLostMouseCapture));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseDownEvent, new MouseButtonEventHandler(ComboBox.OnMouseButtonDown), true);
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseMoveEvent, new MouseEventHandler(ComboBox.OnMouseMove));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(ComboBox.OnPreviewMouseButtonDown));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseWheelEvent, new MouseWheelEventHandler(ComboBox.OnMouseWheel), true);
			EventManager.RegisterClassHandler(typeof(ComboBox), UIElement.GotFocusEvent, new RoutedEventHandler(ComboBox.OnGotFocus));
			EventManager.RegisterClassHandler(typeof(ComboBox), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ComboBox.OnContextMenuOpen), true);
			EventManager.RegisterClassHandler(typeof(ComboBox), ContextMenuService.ContextMenuClosingEvent, new ContextMenuEventHandler(ComboBox.OnContextMenuClose), true);
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(ComboBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(ComboBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.ComboBox);
		}

		// Token: 0x060060AE RID: 24750 RVA: 0x0029A895 File Offset: 0x00299895
		public ComboBox()
		{
			this.Initialize();
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x060060AF RID: 24751 RVA: 0x0029A8AF File Offset: 0x002998AF
		// (set) Token: 0x060060B0 RID: 24752 RVA: 0x0029A8C1 File Offset: 0x002998C1
		[Category("Layout")]
		[Bindable(true)]
		[TypeConverter(typeof(LengthConverter))]
		public double MaxDropDownHeight
		{
			get
			{
				return (double)base.GetValue(ComboBox.MaxDropDownHeightProperty);
			}
			set
			{
				base.SetValue(ComboBox.MaxDropDownHeightProperty, value);
			}
		}

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x060060B1 RID: 24753 RVA: 0x0029A8D4 File Offset: 0x002998D4
		// (set) Token: 0x060060B2 RID: 24754 RVA: 0x0029A8E6 File Offset: 0x002998E6
		[Browsable(false)]
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsDropDownOpen
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsDropDownOpenProperty);
			}
			set
			{
				base.SetValue(ComboBox.IsDropDownOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x060060B3 RID: 24755 RVA: 0x0029A8F9 File Offset: 0x002998F9
		// (set) Token: 0x060060B4 RID: 24756 RVA: 0x0029A90B File Offset: 0x0029990B
		public bool ShouldPreserveUserEnteredPrefix
		{
			get
			{
				return (bool)base.GetValue(ComboBox.ShouldPreserveUserEnteredPrefixProperty);
			}
			set
			{
				base.SetValue(ComboBox.ShouldPreserveUserEnteredPrefixProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060060B5 RID: 24757 RVA: 0x0029A920 File Offset: 0x00299920
		private static object CoerceIsDropDownOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				ComboBox comboBox = (ComboBox)d;
				if (!comboBox.IsLoaded)
				{
					comboBox.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x060060B6 RID: 24758 RVA: 0x0029A951 File Offset: 0x00299951
		private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
		{
			if (!((ComboBox)d).IsDropDownOpen)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x060060B7 RID: 24759 RVA: 0x0029A967 File Offset: 0x00299967
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x0029A97B File Offset: 0x0029997B
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(ComboBox.IsDropDownOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x0029A997 File Offset: 0x00299997
		protected virtual void OnDropDownOpened(EventArgs e)
		{
			base.RaiseClrEvent(ComboBox.DropDownOpenedKey, e);
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x0029A9A5 File Offset: 0x002999A5
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			base.RaiseClrEvent(ComboBox.DropDownClosedKey, e);
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x0029A9B4 File Offset: 0x002999B4
		private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)d;
			comboBox.HasMouseEnteredItemsHost = false;
			bool flag = (bool)e.NewValue;
			bool oldValue = !flag;
			ComboBoxAutomationPeer comboBoxAutomationPeer = UIElementAutomationPeer.FromElement(comboBox) as ComboBoxAutomationPeer;
			if (comboBoxAutomationPeer != null)
			{
				comboBoxAutomationPeer.RaiseExpandCollapseAutomationEvent(oldValue, flag);
			}
			if (flag)
			{
				Mouse.Capture(comboBox, CaptureMode.SubTree);
				if (comboBox.IsEditable && comboBox.EditableTextBoxSite != null)
				{
					comboBox.EditableTextBoxSite.SelectAll();
				}
				if (comboBox._clonedElement != null && VisualTreeHelper.GetParent(comboBox._clonedElement) == null)
				{
					comboBox.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(delegate(object arg)
					{
						ComboBox comboBox2 = (ComboBox)arg;
						comboBox2.UpdateSelectionBoxItem();
						if (comboBox2._clonedElement != null)
						{
							comboBox2._clonedElement.CoerceValue(FrameworkElement.FlowDirectionProperty);
						}
						return null;
					}), comboBox);
				}
				comboBox.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object arg)
				{
					ComboBox comboBox2 = (ComboBox)arg;
					if (comboBox2.IsItemsHostVisible)
					{
						comboBox2.NavigateToItem(comboBox2.InternalSelectedInfo, ItemsControl.ItemNavigateArgs.Empty, true);
					}
					return null;
				}), comboBox);
				comboBox.OnDropDownOpened(EventArgs.Empty);
			}
			else
			{
				if (comboBox.IsKeyboardFocusWithin)
				{
					if (comboBox.IsEditable)
					{
						if (comboBox.EditableTextBoxSite != null && !comboBox.EditableTextBoxSite.IsKeyboardFocusWithin)
						{
							comboBox.Focus();
						}
					}
					else
					{
						comboBox.Focus();
					}
				}
				comboBox.HighlightedInfo = null;
				if (comboBox.HasCapture)
				{
					Mouse.Capture(null);
				}
				if (comboBox._dropDownPopup == null)
				{
					comboBox.OnDropDownClosed(EventArgs.Empty);
				}
			}
			comboBox.CoerceValue(ComboBox.IsSelectionBoxHighlightedProperty);
			comboBox.CoerceValue(ToolTipService.IsEnabledProperty);
			comboBox.UpdateVisualState();
		}

		// Token: 0x060060BC RID: 24764 RVA: 0x0029AB18 File Offset: 0x00299B18
		private void OnPopupClosed(object source, EventArgs e)
		{
			this.OnDropDownClosed(EventArgs.Empty);
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x060060BD RID: 24765 RVA: 0x0029AB25 File Offset: 0x00299B25
		// (set) Token: 0x060060BE RID: 24766 RVA: 0x0029AB37 File Offset: 0x00299B37
		public bool IsEditable
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsEditableProperty);
			}
			set
			{
				base.SetValue(ComboBox.IsEditableProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x0029AB4A File Offset: 0x00299B4A
		private static void OnIsEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = d as ComboBox;
			comboBox.Update();
			comboBox.UpdateVisualState();
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x060060C0 RID: 24768 RVA: 0x0029AB5D File Offset: 0x00299B5D
		// (set) Token: 0x060060C1 RID: 24769 RVA: 0x0029AB6F File Offset: 0x00299B6F
		public string Text
		{
			get
			{
				return (string)base.GetValue(ComboBox.TextProperty);
			}
			set
			{
				base.SetValue(ComboBox.TextProperty, value);
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x060060C2 RID: 24770 RVA: 0x0029AB7D File Offset: 0x00299B7D
		// (set) Token: 0x060060C3 RID: 24771 RVA: 0x0029AB8F File Offset: 0x00299B8F
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(ComboBox.IsReadOnlyProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x060060C4 RID: 24772 RVA: 0x0029ABA2 File Offset: 0x00299BA2
		// (set) Token: 0x060060C5 RID: 24773 RVA: 0x0029ABAF File Offset: 0x00299BAF
		public object SelectionBoxItem
		{
			get
			{
				return base.GetValue(ComboBox.SelectionBoxItemProperty);
			}
			private set
			{
				base.SetValue(ComboBox.SelectionBoxItemPropertyKey, value);
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x060060C6 RID: 24774 RVA: 0x0029ABBD File Offset: 0x00299BBD
		// (set) Token: 0x060060C7 RID: 24775 RVA: 0x0029ABCF File Offset: 0x00299BCF
		public DataTemplate SelectionBoxItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ComboBox.SelectionBoxItemTemplateProperty);
			}
			private set
			{
				base.SetValue(ComboBox.SelectionBoxItemTemplatePropertyKey, value);
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x060060C8 RID: 24776 RVA: 0x0029ABDD File Offset: 0x00299BDD
		// (set) Token: 0x060060C9 RID: 24777 RVA: 0x0029ABEF File Offset: 0x00299BEF
		public string SelectionBoxItemStringFormat
		{
			get
			{
				return (string)base.GetValue(ComboBox.SelectionBoxItemStringFormatProperty);
			}
			private set
			{
				base.SetValue(ComboBox.SelectionBoxItemStringFormatPropertyKey, value);
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x060060CA RID: 24778 RVA: 0x0029ABFD File Offset: 0x00299BFD
		// (set) Token: 0x060060CB RID: 24779 RVA: 0x0029AC0F File Offset: 0x00299C0F
		public bool StaysOpenOnEdit
		{
			get
			{
				return (bool)base.GetValue(ComboBox.StaysOpenOnEditProperty);
			}
			set
			{
				base.SetValue(ComboBox.StaysOpenOnEditProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x060060CC RID: 24780 RVA: 0x0029AC22 File Offset: 0x00299C22
		public bool IsSelectionBoxHighlighted
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsSelectionBoxHighlightedProperty);
			}
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x0029AC34 File Offset: 0x00299C34
		private static object CoerceIsSelectionBoxHighlighted(object o, object value)
		{
			ComboBox comboBox = (ComboBox)o;
			ComboBoxItem highlightedElement;
			return (!comboBox.IsDropDownOpen && comboBox.IsKeyboardFocusWithin) || ((highlightedElement = comboBox.HighlightedElement) != null && highlightedElement.Content == comboBox._clonedElement);
		}

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x060060CE RID: 24782 RVA: 0x0029AC7B File Offset: 0x00299C7B
		// (remove) Token: 0x060060CF RID: 24783 RVA: 0x0029AC89 File Offset: 0x00299C89
		public event EventHandler DropDownOpened
		{
			add
			{
				base.EventHandlersStoreAdd(ComboBox.DropDownOpenedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(ComboBox.DropDownOpenedKey, value);
			}
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x060060D0 RID: 24784 RVA: 0x0029AC97 File Offset: 0x00299C97
		// (remove) Token: 0x060060D1 RID: 24785 RVA: 0x0029ACA5 File Offset: 0x00299CA5
		public event EventHandler DropDownClosed
		{
			add
			{
				base.EventHandlersStoreAdd(ComboBox.DropDownClosedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(ComboBox.DropDownClosedKey, value);
			}
		}

		// Token: 0x060060D2 RID: 24786 RVA: 0x0029ACB4 File Offset: 0x00299CB4
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			this.SelectedItemUpdated();
			if (this.IsDropDownOpen)
			{
				ItemsControl.ItemInfo internalSelectedInfo = base.InternalSelectedInfo;
				if (internalSelectedInfo != null)
				{
					base.NavigateToItem(internalSelectedInfo, ItemsControl.ItemNavigateArgs.Empty, false);
				}
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionPatternOnInvalidated) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				ComboBoxAutomationPeer comboBoxAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this) as ComboBoxAutomationPeer;
				if (comboBoxAutomationPeer != null)
				{
					comboBoxAutomationPeer.RaiseSelectionEvents(e);
				}
			}
		}

		// Token: 0x060060D3 RID: 24787 RVA: 0x0029AD2C File Offset: 0x00299D2C
		internal void SelectedItemUpdated()
		{
			try
			{
				this.UpdatingSelectedItem = true;
				if (!this.UpdatingText)
				{
					string primaryTextFromItem = TextSearch.GetPrimaryTextFromItem(this, base.InternalSelectedItem);
					if (this.Text != primaryTextFromItem)
					{
						base.SetCurrentValueInternal(ComboBox.TextProperty, primaryTextFromItem);
					}
				}
				this.Update();
			}
			finally
			{
				this.UpdatingSelectedItem = false;
			}
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x0029AD90 File Offset: 0x00299D90
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)d;
			ComboBoxAutomationPeer comboBoxAutomationPeer = UIElementAutomationPeer.FromElement(comboBox) as ComboBoxAutomationPeer;
			if (comboBoxAutomationPeer != null)
			{
				comboBoxAutomationPeer.RaiseValuePropertyChangedEvent((string)e.OldValue, (string)e.NewValue);
			}
			comboBox.TextUpdated((string)e.NewValue, false);
		}

		// Token: 0x060060D5 RID: 24789 RVA: 0x0029ADE2 File Offset: 0x00299DE2
		private void OnEditableTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			if (!this.IsEditable)
			{
				return;
			}
			this.TextUpdated(this.EditableTextBoxSite.Text, true);
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x0029ADFF File Offset: 0x00299DFF
		private void OnEditableTextBoxSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (!Helper.IsComposing(this.EditableTextBoxSite))
			{
				this._textBoxSelectionStart = this.EditableTextBoxSite.SelectionStart;
			}
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x0029AE20 File Offset: 0x00299E20
		private void OnEditableTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (this.IsWaitingForTextComposition && e.TextComposition.Source == this.EditableTextBoxSite && e.TextComposition.Stage == TextCompositionStage.Done)
			{
				this.IsWaitingForTextComposition = false;
				this.TextUpdated(this.EditableTextBoxSite.Text, true);
				this.EditableTextBoxSite.RaiseCourtesyTextChangedEvent();
			}
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x0029AE7C File Offset: 0x00299E7C
		private void TextUpdated(string newText, bool textBoxUpdated)
		{
			if (!this.UpdatingText && !this.UpdatingSelectedItem)
			{
				if (Helper.IsComposing(this.EditableTextBoxSite))
				{
					this.IsWaitingForTextComposition = true;
					return;
				}
				try
				{
					this.UpdatingText = true;
					if (base.IsTextSearchEnabled)
					{
						if (this._updateTextBoxOperation != null)
						{
							this._updateTextBoxOperation.Abort();
							this._updateTextBoxOperation = null;
						}
						MatchedTextInfo matchedTextInfo = TextSearch.FindMatchingPrefix(this, newText);
						int num = matchedTextInfo.MatchedItemIndex;
						if (num >= 0)
						{
							if (textBoxUpdated)
							{
								int selectionStart = this.EditableTextBoxSite.SelectionStart;
								if (selectionStart == newText.Length && selectionStart > this._textBoxSelectionStart)
								{
									string text = matchedTextInfo.MatchedText;
									if (this.ShouldPreserveUserEnteredPrefix)
									{
										text = newText + text.Substring(matchedTextInfo.MatchedPrefixLength);
									}
									UndoManager undoManager = this.EditableTextBoxSite.TextContainer.UndoManager;
									if (undoManager != null && undoManager.OpenedUnit != null && undoManager.OpenedUnit.GetType() != typeof(TextParentUndoUnit))
									{
										this._updateTextBoxOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.UpdateTextBoxCallback), new object[]
										{
											text,
											matchedTextInfo
										});
									}
									else
									{
										this.UpdateTextBox(text, matchedTextInfo);
									}
									newText = text;
								}
							}
							else
							{
								string matchedText = matchedTextInfo.MatchedText;
								if (!string.Equals(newText, matchedText, StringComparison.CurrentCulture))
								{
									num = -1;
								}
							}
						}
						if (num != base.SelectedIndex)
						{
							base.SetCurrentValueInternal(Selector.SelectedIndexProperty, num);
						}
					}
					if (textBoxUpdated)
					{
						base.SetCurrentValueInternal(ComboBox.TextProperty, newText);
					}
					else if (this.EditableTextBoxSite != null)
					{
						this.EditableTextBoxSite.Text = newText;
					}
				}
				finally
				{
					this.UpdatingText = false;
				}
			}
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x0029B038 File Offset: 0x0029A038
		private object UpdateTextBoxCallback(object arg)
		{
			this._updateTextBoxOperation = null;
			object[] array = (object[])arg;
			string matchedText = (string)array[0];
			MatchedTextInfo matchedTextInfo = (MatchedTextInfo)array[1];
			try
			{
				this.UpdatingText = true;
				this.UpdateTextBox(matchedText, matchedTextInfo);
			}
			finally
			{
				this.UpdatingText = false;
			}
			return null;
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x0029B090 File Offset: 0x0029A090
		private void UpdateTextBox(string matchedText, MatchedTextInfo matchedTextInfo)
		{
			this.EditableTextBoxSite.Text = matchedText;
			this.EditableTextBoxSite.SelectionStart = matchedText.Length - matchedTextInfo.TextExcludingPrefixLength;
			this.EditableTextBoxSite.SelectionLength = matchedTextInfo.TextExcludingPrefixLength;
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x0029B0C7 File Offset: 0x0029A0C7
		private void Update()
		{
			if (this.IsEditable)
			{
				this.UpdateEditableTextBox();
				return;
			}
			this.UpdateSelectionBoxItem();
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x0029B0E0 File Offset: 0x0029A0E0
		private void UpdateEditableTextBox()
		{
			if (!this.UpdatingText)
			{
				try
				{
					this.UpdatingText = true;
					string text = this.Text;
					if (this.EditableTextBoxSite != null && this.EditableTextBoxSite.Text != text)
					{
						this.EditableTextBoxSite.Text = text;
						this.EditableTextBoxSite.SelectAll();
					}
				}
				finally
				{
					this.UpdatingText = false;
				}
			}
		}

		// Token: 0x060060DD RID: 24797 RVA: 0x0029B150 File Offset: 0x0029A150
		private void UpdateSelectionBoxItem()
		{
			object obj = base.InternalSelectedItem;
			DataTemplate dataTemplate = base.ItemTemplate;
			string text = base.ItemStringFormat;
			ContentControl contentControl = obj as ContentControl;
			if (contentControl != null)
			{
				obj = contentControl.Content;
				dataTemplate = contentControl.ContentTemplate;
				text = contentControl.ContentStringFormat;
			}
			if (this._clonedElement != null)
			{
				this._clonedElement.LayoutUpdated -= this.CloneLayoutUpdated;
				this._clonedElement = null;
			}
			if (dataTemplate == null && base.ItemTemplateSelector == null && text == null)
			{
				DependencyObject dependencyObject = obj as DependencyObject;
				if (dependencyObject != null)
				{
					this._clonedElement = (dependencyObject as UIElement);
					if (this._clonedElement != null)
					{
						VisualBrush visualBrush = new VisualBrush(this._clonedElement);
						visualBrush.Stretch = Stretch.None;
						visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
						visualBrush.Viewbox = new Rect(this._clonedElement.RenderSize);
						visualBrush.ViewportUnits = BrushMappingMode.Absolute;
						visualBrush.Viewport = new Rect(this._clonedElement.RenderSize);
						DependencyObject parent = VisualTreeHelper.GetParent(this._clonedElement);
						FlowDirection flowDirection = (parent == null) ? FlowDirection.LeftToRight : ((FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty));
						if (base.FlowDirection != flowDirection)
						{
							visualBrush.Transform = new MatrixTransform(new Matrix(-1.0, 0.0, 0.0, 1.0, this._clonedElement.RenderSize.Width, 0.0));
						}
						Rectangle rectangle = new Rectangle();
						rectangle.Fill = visualBrush;
						rectangle.Width = this._clonedElement.RenderSize.Width;
						rectangle.Height = this._clonedElement.RenderSize.Height;
						this._clonedElement.LayoutUpdated += this.CloneLayoutUpdated;
						obj = rectangle;
						dataTemplate = null;
					}
					else
					{
						obj = ComboBox.ExtractString(dependencyObject);
						dataTemplate = ContentPresenter.StringContentTemplate;
					}
				}
			}
			if (obj == null)
			{
				obj = string.Empty;
				dataTemplate = ContentPresenter.StringContentTemplate;
			}
			this.SelectionBoxItem = obj;
			this.SelectionBoxItemTemplate = dataTemplate;
			this.SelectionBoxItemStringFormat = text;
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x0029B35C File Offset: 0x0029A35C
		private void CloneLayoutUpdated(object sender, EventArgs e)
		{
			Rectangle rectangle = (Rectangle)this.SelectionBoxItem;
			rectangle.Width = this._clonedElement.RenderSize.Width;
			rectangle.Height = this._clonedElement.RenderSize.Height;
			VisualBrush visualBrush = (VisualBrush)rectangle.Fill;
			visualBrush.Viewbox = new Rect(this._clonedElement.RenderSize);
			visualBrush.Viewport = new Rect(this._clonedElement.RenderSize);
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x0029B3DC File Offset: 0x0029A3DC
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (!Selector.GetIsSelectionActive(this))
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			else if (this.IsDropDownOpen)
			{
				VisualStateManager.GoToState(this, "FocusedDropDown", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			if (this.IsEditable)
			{
				VisualStateManager.GoToState(this, "Editable", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Uneditable", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x060060E0 RID: 24800 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x0029B48A File Offset: 0x0029A48A
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (item is Separator)
			{
				Separator.PrepareContainer(element as Control);
			}
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x0029B4A7 File Offset: 0x0029A4A7
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			base.AdjustItemInfo(e, this._highlightedInfo);
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x060060E3 RID: 24803 RVA: 0x0029B4C0 File Offset: 0x0029A4C0
		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (!e.Handled && comboBox.IsEditable && comboBox.EditableTextBoxSite != null)
			{
				if (e.OriginalSource == comboBox)
				{
					comboBox.EditableTextBoxSite.Focus();
					e.Handled = true;
					return;
				}
				if (e.OriginalSource == comboBox.EditableTextBoxSite)
				{
					comboBox.EditableTextBoxSite.SelectAll();
				}
			}
		}

		// Token: 0x060060E4 RID: 24804 RVA: 0x0029B524 File Offset: 0x0029A524
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			bool result = false;
			if (!this.IsEditable)
			{
				result = base.FocusItem(info, itemNavigateArgs);
			}
			this.HighlightedInfo = ((info.Container is ComboBoxItem) ? info : null);
			if ((this.IsEditable || !this.IsDropDownOpen) && itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				int num = info.Index;
				if (num < 0)
				{
					num = base.Items.IndexOf(info.Item);
				}
				base.SetCurrentValueInternal(Selector.SelectedIndexProperty, num);
				result = true;
			}
			return result;
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x0029B5AC File Offset: 0x0029A5AC
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (this.IsDropDownOpen && !base.IsKeyboardFocusWithin)
			{
				DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
				if (dependencyObject == null || (!this.IsContextMenuOpen && ItemsControl.ItemsControlFromItemContainer(dependencyObject) != this))
				{
					this.Close();
				}
			}
			base.CoerceValue(ComboBox.IsSelectionBoxHighlightedProperty);
		}

		// Token: 0x060060E6 RID: 24806 RVA: 0x0029B600 File Offset: 0x0029A600
		private static void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.IsKeyboardFocusWithin)
			{
				if (!comboBox.IsDropDownOpen)
				{
					if (e.Delta < 0)
					{
						comboBox.SelectNext();
					}
					else
					{
						comboBox.SelectPrev();
					}
				}
				e.Handled = true;
				return;
			}
			if (comboBox.IsDropDownOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060060E7 RID: 24807 RVA: 0x0029B652 File Offset: 0x0029A652
		private static void OnContextMenuOpen(object sender, ContextMenuEventArgs e)
		{
			((ComboBox)sender).IsContextMenuOpen = true;
		}

		// Token: 0x060060E8 RID: 24808 RVA: 0x0029B660 File Offset: 0x0029A660
		private static void OnContextMenuClose(object sender, ContextMenuEventArgs e)
		{
			((ComboBox)sender).IsContextMenuOpen = false;
		}

		// Token: 0x060060E9 RID: 24809 RVA: 0x0029B670 File Offset: 0x0029A670
		protected override void OnIsMouseCapturedChanged(DependencyPropertyChangedEventArgs e)
		{
			if (base.IsMouseCaptured)
			{
				if (this._autoScrollTimer == null)
				{
					this._autoScrollTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
					this._autoScrollTimer.Interval = ItemsControl.AutoScrollTimeout;
					this._autoScrollTimer.Tick += this.OnAutoScrollTimeout;
					this._autoScrollTimer.Start();
				}
			}
			else if (this._autoScrollTimer != null)
			{
				this._autoScrollTimer.Stop();
				this._autoScrollTimer = null;
			}
			base.OnIsMouseCapturedChanged(e);
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x060060EA RID: 24810 RVA: 0x0029B6EE File Offset: 0x0029A6EE
		protected internal override bool HasEffectiveKeyboardFocus
		{
			get
			{
				if (this.IsEditable && this.EditableTextBoxSite != null)
				{
					return this.EditableTextBoxSite.HasEffectiveKeyboardFocus;
				}
				return base.HasEffectiveKeyboardFocus;
			}
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void NotifyComboBoxItemMouseDown(ComboBoxItem comboBoxItem)
		{
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x0029B714 File Offset: 0x0029A714
		internal void NotifyComboBoxItemMouseUp(ComboBoxItem comboBoxItem)
		{
			object obj = base.ItemContainerGenerator.ItemFromContainer(comboBoxItem);
			if (obj != null)
			{
				base.SelectionChange.SelectJustThisItem(base.NewItemInfo(obj, comboBoxItem, -1), true);
			}
			this.Close();
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x0029B74C File Offset: 0x0029A74C
		internal void NotifyComboBoxItemEnter(ComboBoxItem item)
		{
			if (this.IsDropDownOpen && Mouse.Captured == this && base.DidMouseMove())
			{
				this.HighlightedInfo = base.ItemInfoFromContainer(item);
				if (!this.IsEditable && !item.IsKeyboardFocusWithin)
				{
					item.Focus();
				}
			}
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x0029B78A File Offset: 0x0029A78A
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ComboBoxItem;
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x0029B795 File Offset: 0x0029A795
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ComboBoxItem();
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x0029B79C File Offset: 0x0029A79C
		private void Initialize()
		{
			base.CanSelectMultiple = false;
		}

		// Token: 0x060060F1 RID: 24817 RVA: 0x0029B7A5 File Offset: 0x0029A7A5
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (this.IsEditable && e.OriginalSource == this.EditableTextBoxSite)
			{
				this.KeyDownHandler(e);
			}
		}

		// Token: 0x060060F2 RID: 24818 RVA: 0x0029B7C4 File Offset: 0x0029A7C4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.KeyDownHandler(e);
		}

		// Token: 0x060060F3 RID: 24819 RVA: 0x0029B7D0 File Offset: 0x0029A7D0
		private void KeyDownHandler(KeyEventArgs e)
		{
			bool flag = false;
			Key key = e.Key;
			if (key == Key.System)
			{
				key = e.SystemKey;
			}
			bool flag2 = base.FlowDirection == FlowDirection.RightToLeft;
			if (key <= Key.Down)
			{
				if (key != Key.Return)
				{
					switch (key)
					{
					case Key.Escape:
						if (this.IsDropDownOpen)
						{
							this.KeyboardCloseDropDown(false);
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Prior:
						if (this.IsItemsHostVisible)
						{
							base.NavigateByPage(this.HighlightedInfo, FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Next:
						if (this.IsItemsHostVisible)
						{
							base.NavigateByPage(this.HighlightedInfo, FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.End:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else
							{
								this.SelectLast();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Home:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else
							{
								this.SelectFirst();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Left:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Left, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else if (!flag2)
							{
								this.SelectPrev();
							}
							else
							{
								this.SelectNext();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Up:
						flag = true;
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
						{
							this.KeyboardToggleDropDown(true);
							goto IL_349;
						}
						if (this.IsItemsHostVisible)
						{
							base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							goto IL_349;
						}
						this.SelectPrev();
						goto IL_349;
					case Key.Right:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Right, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else if (!flag2)
							{
								this.SelectNext();
							}
							else
							{
								this.SelectPrev();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Down:
						flag = true;
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
						{
							this.KeyboardToggleDropDown(true);
							goto IL_349;
						}
						if (this.IsItemsHostVisible)
						{
							base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							goto IL_349;
						}
						this.SelectNext();
						goto IL_349;
					}
				}
				else
				{
					if (this.IsDropDownOpen)
					{
						this.KeyboardCloseDropDown(true);
						flag = true;
						goto IL_349;
					}
					goto IL_349;
				}
			}
			else if (key != Key.F4)
			{
				if (key == Key.Oem5)
				{
					if (Keyboard.Modifiers == ModifierKeys.Control)
					{
						base.NavigateToItem(base.InternalSelectedInfo, ItemsControl.ItemNavigateArgs.Empty, false);
						flag = true;
						goto IL_349;
					}
					goto IL_349;
				}
			}
			else
			{
				if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
				{
					this.KeyboardToggleDropDown(true);
					flag = true;
					goto IL_349;
				}
				goto IL_349;
			}
			flag = false;
			IL_349:
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x060060F4 RID: 24820 RVA: 0x0029BB30 File Offset: 0x0029AB30
		private void SelectPrev()
		{
			if (!base.Items.IsEmpty)
			{
				int internalSelectedIndex = base.InternalSelectedIndex;
				if (internalSelectedIndex > 0)
				{
					this.SelectItemHelper(internalSelectedIndex - 1, -1, -1);
				}
			}
		}

		// Token: 0x060060F5 RID: 24821 RVA: 0x0029BB60 File Offset: 0x0029AB60
		private void SelectNext()
		{
			int count = base.Items.Count;
			if (count > 0)
			{
				int internalSelectedIndex = base.InternalSelectedIndex;
				if (internalSelectedIndex < count - 1)
				{
					this.SelectItemHelper(internalSelectedIndex + 1, 1, count);
				}
			}
		}

		// Token: 0x060060F6 RID: 24822 RVA: 0x0029BB95 File Offset: 0x0029AB95
		private void SelectFirst()
		{
			this.SelectItemHelper(0, 1, base.Items.Count);
		}

		// Token: 0x060060F7 RID: 24823 RVA: 0x0029BBAA File Offset: 0x0029ABAA
		private void SelectLast()
		{
			this.SelectItemHelper(base.Items.Count - 1, -1, -1);
		}

		// Token: 0x060060F8 RID: 24824 RVA: 0x0029BBC4 File Offset: 0x0029ABC4
		private void SelectItemHelper(int startIndex, int increment, int stopIndex)
		{
			for (int num = startIndex; num != stopIndex; num += increment)
			{
				object obj = base.Items[num];
				DependencyObject dependencyObject = base.ItemContainerGenerator.ContainerFromIndex(num);
				if (this.IsSelectableHelper(obj) && this.IsSelectableHelper(dependencyObject))
				{
					base.SelectionChange.SelectJustThisItem(base.NewItemInfo(obj, dependencyObject, num), true);
					return;
				}
			}
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x0029BC20 File Offset: 0x0029AC20
		private bool IsSelectableHelper(object o)
		{
			DependencyObject dependencyObject = o as DependencyObject;
			return dependencyObject == null || (bool)dependencyObject.GetValue(UIElement.IsEnabledProperty);
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x0029BC4C File Offset: 0x0029AC4C
		private static string ExtractString(DependencyObject d)
		{
			string text = string.Empty;
			TextBlock textBlock;
			Visual reference;
			TextElement textElement;
			if ((textBlock = (d as TextBlock)) != null)
			{
				text = textBlock.Text;
			}
			else if ((reference = (d as Visual)) != null)
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
				for (int i = 0; i < childrenCount; i++)
				{
					text += ComboBox.ExtractString(VisualTreeHelper.GetChild(reference, i));
				}
			}
			else if ((textElement = (d as TextElement)) != null)
			{
				text += TextRangeBase.GetTextInternal(textElement.ContentStart, textElement.ContentEnd);
			}
			return text;
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x0029BCD0 File Offset: 0x0029ACD0
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._dropDownPopup != null)
			{
				this._dropDownPopup.Closed -= this.OnPopupClosed;
			}
			this.EditableTextBoxSite = (base.GetTemplateChild("PART_EditableTextBox") as TextBox);
			this._dropDownPopup = (base.GetTemplateChild("PART_Popup") as Popup);
			if (this.EditableTextBoxSite != null)
			{
				this.EditableTextBoxSite.TextChanged += this.OnEditableTextBoxTextChanged;
				this.EditableTextBoxSite.SelectionChanged += this.OnEditableTextBoxSelectionChanged;
				this.EditableTextBoxSite.PreviewTextInput += this.OnEditableTextBoxPreviewTextInput;
			}
			if (this._dropDownPopup != null)
			{
				this._dropDownPopup.Closed += this.OnPopupClosed;
			}
			this.Update();
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x0029BDA0 File Offset: 0x0029ADA0
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
			if (this.EditableTextBoxSite != null)
			{
				this.EditableTextBoxSite.TextChanged -= this.OnEditableTextBoxTextChanged;
				this.EditableTextBoxSite.SelectionChanged -= this.OnEditableTextBoxSelectionChanged;
				this.EditableTextBoxSite.PreviewTextInput -= this.OnEditableTextBoxPreviewTextInput;
			}
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x0029BE04 File Offset: 0x0029AE04
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (Mouse.Captured != comboBox)
			{
				if (e.OriginalSource == comboBox)
				{
					if (Mouse.Captured == null || !MenuBase.IsDescendant(comboBox, Mouse.Captured as DependencyObject))
					{
						comboBox.Close();
						return;
					}
				}
				else if (MenuBase.IsDescendant(comboBox, e.OriginalSource as DependencyObject))
				{
					if (comboBox.IsDropDownOpen && Mouse.Captured == null && SafeNativeMethods.GetCapture() == IntPtr.Zero)
					{
						Mouse.Capture(comboBox, CaptureMode.SubTree);
						e.Handled = true;
						return;
					}
				}
				else
				{
					comboBox.Close();
				}
			}
		}

		// Token: 0x060060FE RID: 24830 RVA: 0x0029BE94 File Offset: 0x0029AE94
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (!comboBox.IsContextMenuOpen && !comboBox.IsKeyboardFocusWithin)
			{
				comboBox.Focus();
			}
			e.Handled = true;
			if (Mouse.Captured == comboBox && e.OriginalSource == comboBox)
			{
				comboBox.Close();
			}
		}

		// Token: 0x060060FF RID: 24831 RVA: 0x0029BEE0 File Offset: 0x0029AEE0
		private static void OnPreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.IsEditable)
			{
				Visual visual = e.OriginalSource as Visual;
				Visual editableTextBoxSite = comboBox.EditableTextBoxSite;
				if (visual != null && editableTextBoxSite != null && editableTextBoxSite.IsAncestorOf(visual))
				{
					if (comboBox.IsDropDownOpen && !comboBox.StaysOpenOnEdit)
					{
						comboBox.Close();
						return;
					}
					if (!comboBox.IsContextMenuOpen && !comboBox.IsKeyboardFocusWithin)
					{
						comboBox.Focus();
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x0029BF53 File Offset: 0x0029AF53
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (this.HasMouseEnteredItemsHost && !this.IsMouseOverItemsHost && this.IsDropDownOpen)
			{
				this.Close();
				e.Handled = true;
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x06006101 RID: 24833 RVA: 0x0029BF84 File Offset: 0x0029AF84
		private static void OnMouseMove(object sender, MouseEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.IsDropDownOpen)
			{
				bool flag = comboBox.ItemsHost != null && comboBox.ItemsHost.IsMouseOver;
				if (flag && !comboBox.HasMouseEnteredItemsHost)
				{
					comboBox.SetInitialMousePosition();
				}
				comboBox.IsMouseOverItemsHost = flag;
				comboBox.HasMouseEnteredItemsHost = flag;
			}
			if (Mouse.LeftButton == MouseButtonState.Pressed && comboBox.HasMouseEnteredItemsHost && Mouse.Captured == comboBox)
			{
				if (Mouse.LeftButton == MouseButtonState.Pressed)
				{
					comboBox.DoAutoScroll(comboBox.HighlightedInfo);
				}
				else
				{
					comboBox.ReleaseMouseCapture();
					comboBox.ResetLastMousePosition();
				}
				e.Handled = true;
			}
		}

		// Token: 0x06006102 RID: 24834 RVA: 0x0029C01E File Offset: 0x0029B01E
		private void KeyboardToggleDropDown(bool commitSelection)
		{
			this.KeyboardToggleDropDown(!this.IsDropDownOpen, commitSelection);
		}

		// Token: 0x06006103 RID: 24835 RVA: 0x0029C030 File Offset: 0x0029B030
		private void KeyboardCloseDropDown(bool commitSelection)
		{
			this.KeyboardToggleDropDown(false, commitSelection);
		}

		// Token: 0x06006104 RID: 24836 RVA: 0x0029C03C File Offset: 0x0029B03C
		private void KeyboardToggleDropDown(bool openDropDown, bool commitSelection)
		{
			ItemsControl.ItemInfo itemInfo = null;
			if (commitSelection)
			{
				itemInfo = this.HighlightedInfo;
			}
			base.SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, BooleanBoxes.Box(openDropDown));
			if (!openDropDown && commitSelection && itemInfo != null)
			{
				base.SelectionChange.SelectJustThisItem(itemInfo, true);
			}
		}

		// Token: 0x06006105 RID: 24837 RVA: 0x0029C084 File Offset: 0x0029B084
		private void CommitSelection()
		{
			ItemsControl.ItemInfo highlightedInfo = this.HighlightedInfo;
			if (highlightedInfo != null)
			{
				base.SelectionChange.SelectJustThisItem(highlightedInfo, true);
			}
		}

		// Token: 0x06006106 RID: 24838 RVA: 0x0029C0AE File Offset: 0x0029B0AE
		private void OnAutoScrollTimeout(object sender, EventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed && this.HasMouseEnteredItemsHost)
			{
				base.DoAutoScroll(this.HighlightedInfo);
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x0029C0CC File Offset: 0x0029B0CC
		private void Close()
		{
			if (this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, false);
			}
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x0029C0E7 File Offset: 0x0029B0E7
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ComboBoxAutomationPeer(this);
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x06006109 RID: 24841 RVA: 0x0029C0EF File Offset: 0x0029B0EF
		// (set) Token: 0x0600610A RID: 24842 RVA: 0x0029C0F7 File Offset: 0x0029B0F7
		internal TextBox EditableTextBoxSite
		{
			get
			{
				return this._editableTextBoxSite;
			}
			set
			{
				this._editableTextBoxSite = value;
			}
		}

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x0600610B RID: 24843 RVA: 0x0029C100 File Offset: 0x0029B100
		private bool HasCapture
		{
			get
			{
				return Mouse.Captured == this;
			}
		}

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x0600610C RID: 24844 RVA: 0x0029C10C File Offset: 0x0029B10C
		private bool IsItemsHostVisible
		{
			get
			{
				Panel itemsHost = base.ItemsHost;
				if (itemsHost != null)
				{
					HwndSource hwndSource = PresentationSource.CriticalFromVisual(itemsHost) as HwndSource;
					if (hwndSource != null && !hwndSource.IsDisposed && hwndSource.RootVisual != null)
					{
						return hwndSource.RootVisual.IsAncestorOf(itemsHost);
					}
				}
				return false;
			}
		}

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x0600610D RID: 24845 RVA: 0x0029C150 File Offset: 0x0029B150
		// (set) Token: 0x0600610E RID: 24846 RVA: 0x0029C158 File Offset: 0x0029B158
		private ItemsControl.ItemInfo HighlightedInfo
		{
			get
			{
				return this._highlightedInfo;
			}
			set
			{
				ComboBoxItem comboBoxItem = (this._highlightedInfo != null) ? (this._highlightedInfo.Container as ComboBoxItem) : null;
				if (comboBoxItem != null)
				{
					comboBoxItem.SetIsHighlighted(false);
				}
				this._highlightedInfo = value;
				comboBoxItem = ((this._highlightedInfo != null) ? (this._highlightedInfo.Container as ComboBoxItem) : null);
				if (comboBoxItem != null)
				{
					comboBoxItem.SetIsHighlighted(true);
				}
				base.CoerceValue(ComboBox.IsSelectionBoxHighlightedProperty);
			}
		}

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x0600610F RID: 24847 RVA: 0x0029C1CF File Offset: 0x0029B1CF
		private ComboBoxItem HighlightedElement
		{
			get
			{
				if (!(this._highlightedInfo == null))
				{
					return this._highlightedInfo.Container as ComboBoxItem;
				}
				return null;
			}
		}

		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06006110 RID: 24848 RVA: 0x0029C1F1 File Offset: 0x0029B1F1
		// (set) Token: 0x06006111 RID: 24849 RVA: 0x0029C1FF File Offset: 0x0029B1FF
		private bool IsMouseOverItemsHost
		{
			get
			{
				return this._cacheValid[1];
			}
			set
			{
				this._cacheValid[1] = value;
			}
		}

		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06006112 RID: 24850 RVA: 0x0029C20E File Offset: 0x0029B20E
		// (set) Token: 0x06006113 RID: 24851 RVA: 0x0029C21C File Offset: 0x0029B21C
		private bool HasMouseEnteredItemsHost
		{
			get
			{
				return this._cacheValid[2];
			}
			set
			{
				this._cacheValid[2] = value;
			}
		}

		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x06006114 RID: 24852 RVA: 0x0029C22B File Offset: 0x0029B22B
		// (set) Token: 0x06006115 RID: 24853 RVA: 0x0029C239 File Offset: 0x0029B239
		private bool IsContextMenuOpen
		{
			get
			{
				return this._cacheValid[4];
			}
			set
			{
				this._cacheValid[4] = value;
			}
		}

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x06006116 RID: 24854 RVA: 0x0029C248 File Offset: 0x0029B248
		// (set) Token: 0x06006117 RID: 24855 RVA: 0x0029C256 File Offset: 0x0029B256
		private bool UpdatingText
		{
			get
			{
				return this._cacheValid[8];
			}
			set
			{
				this._cacheValid[8] = value;
			}
		}

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x06006118 RID: 24856 RVA: 0x0029C265 File Offset: 0x0029B265
		// (set) Token: 0x06006119 RID: 24857 RVA: 0x0029C274 File Offset: 0x0029B274
		private bool UpdatingSelectedItem
		{
			get
			{
				return this._cacheValid[16];
			}
			set
			{
				this._cacheValid[16] = value;
			}
		}

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x0600611A RID: 24858 RVA: 0x0029C284 File Offset: 0x0029B284
		// (set) Token: 0x0600611B RID: 24859 RVA: 0x0029C293 File Offset: 0x0029B293
		private bool IsWaitingForTextComposition
		{
			get
			{
				return this._cacheValid[32];
			}
			set
			{
				this._cacheValid[32] = value;
			}
		}

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x0600611C RID: 24860 RVA: 0x0029C2A3 File Offset: 0x0029B2A3
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ComboBox._dType;
			}
		}

		// Token: 0x04003242 RID: 12866
		public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(ComboBox), new FrameworkPropertyMetadata(SystemParameters.PrimaryScreenHeight / 3.0, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		// Token: 0x04003243 RID: 12867
		public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ComboBox.OnIsDropDownOpenChanged), new CoerceValueCallback(ComboBox.CoerceIsDropDownOpen)));

		// Token: 0x04003244 RID: 12868
		public static readonly DependencyProperty ShouldPreserveUserEnteredPrefixProperty = DependencyProperty.Register("ShouldPreserveUserEnteredPrefix", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003245 RID: 12869
		public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(ComboBox.OnIsEditableChanged)));

		// Token: 0x04003246 RID: 12870
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ComboBox.OnTextChanged)));

		// Token: 0x04003247 RID: 12871
		public static readonly DependencyProperty IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(typeof(ComboBox));

		// Token: 0x04003248 RID: 12872
		private static readonly DependencyPropertyKey SelectionBoxItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItem", typeof(object), typeof(ComboBox), new FrameworkPropertyMetadata(string.Empty));

		// Token: 0x04003249 RID: 12873
		public static readonly DependencyProperty SelectionBoxItemProperty = ComboBox.SelectionBoxItemPropertyKey.DependencyProperty;

		// Token: 0x0400324A RID: 12874
		private static readonly DependencyPropertyKey SelectionBoxItemTemplatePropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemTemplate", typeof(DataTemplate), typeof(ComboBox), new FrameworkPropertyMetadata(null));

		// Token: 0x0400324B RID: 12875
		public static readonly DependencyProperty SelectionBoxItemTemplateProperty = ComboBox.SelectionBoxItemTemplatePropertyKey.DependencyProperty;

		// Token: 0x0400324C RID: 12876
		private static readonly DependencyPropertyKey SelectionBoxItemStringFormatPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemStringFormat", typeof(string), typeof(ComboBox), new FrameworkPropertyMetadata(null));

		// Token: 0x0400324D RID: 12877
		public static readonly DependencyProperty SelectionBoxItemStringFormatProperty = ComboBox.SelectionBoxItemStringFormatPropertyKey.DependencyProperty;

		// Token: 0x0400324E RID: 12878
		public static readonly DependencyProperty StaysOpenOnEditProperty = DependencyProperty.Register("StaysOpenOnEdit", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x0400324F RID: 12879
		private static readonly DependencyPropertyKey IsSelectionBoxHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelectionBoxHighlighted", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, new CoerceValueCallback(ComboBox.CoerceIsSelectionBoxHighlighted)));

		// Token: 0x04003250 RID: 12880
		private static readonly DependencyProperty IsSelectionBoxHighlightedProperty = ComboBox.IsSelectionBoxHighlightedPropertyKey.DependencyProperty;

		// Token: 0x04003251 RID: 12881
		private static readonly EventPrivateKey DropDownOpenedKey = new EventPrivateKey();

		// Token: 0x04003252 RID: 12882
		private static readonly EventPrivateKey DropDownClosedKey = new EventPrivateKey();

		// Token: 0x04003253 RID: 12883
		private const string EditableTextBoxTemplateName = "PART_EditableTextBox";

		// Token: 0x04003254 RID: 12884
		private const string PopupTemplateName = "PART_Popup";

		// Token: 0x04003255 RID: 12885
		private TextBox _editableTextBoxSite;

		// Token: 0x04003256 RID: 12886
		private Popup _dropDownPopup;

		// Token: 0x04003257 RID: 12887
		private int _textBoxSelectionStart;

		// Token: 0x04003258 RID: 12888
		private BitVector32 _cacheValid = new BitVector32(0);

		// Token: 0x04003259 RID: 12889
		private ItemsControl.ItemInfo _highlightedInfo;

		// Token: 0x0400325A RID: 12890
		private DispatcherTimer _autoScrollTimer;

		// Token: 0x0400325B RID: 12891
		private UIElement _clonedElement;

		// Token: 0x0400325C RID: 12892
		private DispatcherOperation _updateTextBoxOperation;

		// Token: 0x0400325D RID: 12893
		private static DependencyObjectType _dType;

		// Token: 0x02000BC1 RID: 3009
		private enum CacheBits
		{
			// Token: 0x040049CE RID: 18894
			IsMouseOverItemsHost = 1,
			// Token: 0x040049CF RID: 18895
			HasMouseEnteredItemsHost,
			// Token: 0x040049D0 RID: 18896
			IsContextMenuOpen = 4,
			// Token: 0x040049D1 RID: 18897
			UpdatingText = 8,
			// Token: 0x040049D2 RID: 18898
			UpdatingSelectedItem = 16,
			// Token: 0x040049D3 RID: 18899
			IsWaitingForTextComposition = 32
		}
	}
}
