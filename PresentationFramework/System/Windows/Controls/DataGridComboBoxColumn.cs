using System;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace System.Windows.Controls
{
	// Token: 0x02000752 RID: 1874
	public class DataGridComboBoxColumn : DataGridColumn
	{
		// Token: 0x060065F9 RID: 26105 RVA: 0x002B04F4 File Offset: 0x002AF4F4
		static DataGridComboBoxColumn()
		{
			DataGridColumn.SortMemberPathProperty.OverrideMetadata(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridComboBoxColumn.OnCoerceSortMemberPath)));
		}

		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x060065FA RID: 26106 RVA: 0x002B05F6 File Offset: 0x002AF5F6
		public static ComponentResourceKey TextBlockComboBoxStyleKey
		{
			get
			{
				return SystemResourceKey.DataGridComboBoxColumnTextBlockComboBoxStyleKey;
			}
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x002B0600 File Offset: 0x002AF600
		private static object OnCoerceSortMemberPath(DependencyObject d, object baseValue)
		{
			DataGridComboBoxColumn dataGridComboBoxColumn = (DataGridComboBoxColumn)d;
			string text = (string)baseValue;
			if (string.IsNullOrEmpty(text))
			{
				string pathFromBinding = DataGridHelper.GetPathFromBinding(dataGridComboBoxColumn.EffectiveBinding as Binding);
				if (!string.IsNullOrEmpty(pathFromBinding))
				{
					text = pathFromBinding;
				}
			}
			return text;
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x060065FC RID: 26108 RVA: 0x002B063F File Offset: 0x002AF63F
		private BindingBase EffectiveBinding
		{
			get
			{
				if (this.SelectedItemBinding != null)
				{
					return this.SelectedItemBinding;
				}
				if (this.SelectedValueBinding != null)
				{
					return this.SelectedValueBinding;
				}
				return this.TextBinding;
			}
		}

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x060065FD RID: 26109 RVA: 0x002B0665 File Offset: 0x002AF665
		// (set) Token: 0x060065FE RID: 26110 RVA: 0x002B0670 File Offset: 0x002AF670
		public virtual BindingBase SelectedValueBinding
		{
			get
			{
				return this._selectedValueBinding;
			}
			set
			{
				if (this._selectedValueBinding != value)
				{
					BindingBase selectedValueBinding = this._selectedValueBinding;
					this._selectedValueBinding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnSelectedValueBindingChanged(selectedValueBinding, this._selectedValueBinding);
				}
			}
		}

		// Token: 0x060065FF RID: 26111 RVA: 0x002B06B7 File Offset: 0x002AF6B7
		protected override bool OnCoerceIsReadOnly(bool baseValue)
		{
			return DataGridHelper.IsOneWay(this.EffectiveBinding) || base.OnCoerceIsReadOnly(baseValue);
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x06006600 RID: 26112 RVA: 0x002B06CF File Offset: 0x002AF6CF
		// (set) Token: 0x06006601 RID: 26113 RVA: 0x002B06D8 File Offset: 0x002AF6D8
		public virtual BindingBase SelectedItemBinding
		{
			get
			{
				return this._selectedItemBinding;
			}
			set
			{
				if (this._selectedItemBinding != value)
				{
					BindingBase selectedItemBinding = this._selectedItemBinding;
					this._selectedItemBinding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnSelectedItemBindingChanged(selectedItemBinding, this._selectedItemBinding);
				}
			}
		}

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x06006602 RID: 26114 RVA: 0x002B071F File Offset: 0x002AF71F
		// (set) Token: 0x06006603 RID: 26115 RVA: 0x002B0728 File Offset: 0x002AF728
		public virtual BindingBase TextBinding
		{
			get
			{
				return this._textBinding;
			}
			set
			{
				if (this._textBinding != value)
				{
					BindingBase textBinding = this._textBinding;
					this._textBinding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnTextBindingChanged(textBinding, this._textBinding);
				}
			}
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x002B076F File Offset: 0x002AF76F
		protected virtual void OnSelectedValueBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("SelectedValueBinding");
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x002B077C File Offset: 0x002AF77C
		protected virtual void OnSelectedItemBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("SelectedItemBinding");
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x002B0789 File Offset: 0x002AF789
		protected virtual void OnTextBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("TextBinding");
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x06006607 RID: 26119 RVA: 0x002B0798 File Offset: 0x002AF798
		public static Style DefaultElementStyle
		{
			get
			{
				if (DataGridComboBoxColumn._defaultElementStyle == null)
				{
					Style style = new Style(typeof(ComboBox));
					style.Setters.Add(new Setter(Selector.IsSynchronizedWithCurrentItemProperty, false));
					style.Seal();
					DataGridComboBoxColumn._defaultElementStyle = style;
				}
				return DataGridComboBoxColumn._defaultElementStyle;
			}
		}

		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x06006608 RID: 26120 RVA: 0x002B07E6 File Offset: 0x002AF7E6
		public static Style DefaultEditingElementStyle
		{
			get
			{
				return DataGridComboBoxColumn.DefaultElementStyle;
			}
		}

		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x06006609 RID: 26121 RVA: 0x002B07ED File Offset: 0x002AF7ED
		// (set) Token: 0x0600660A RID: 26122 RVA: 0x002B07FF File Offset: 0x002AF7FF
		public Style ElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridComboBoxColumn.ElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.ElementStyleProperty, value);
			}
		}

		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x0600660B RID: 26123 RVA: 0x002B080D File Offset: 0x002AF80D
		// (set) Token: 0x0600660C RID: 26124 RVA: 0x002B081F File Offset: 0x002AF81F
		public Style EditingElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridComboBoxColumn.EditingElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.EditingElementStyleProperty, value);
			}
		}

		// Token: 0x0600660D RID: 26125 RVA: 0x002B0830 File Offset: 0x002AF830
		private void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
		{
			Style style = this.PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x002B0850 File Offset: 0x002AF850
		internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkContentElement element)
		{
			Style style = this.PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		// Token: 0x0600660F RID: 26127 RVA: 0x002B0870 File Offset: 0x002AF870
		private Style PickStyle(bool isEditing, bool defaultToElementStyle)
		{
			Style style = isEditing ? this.EditingElementStyle : this.ElementStyle;
			if (isEditing && defaultToElementStyle && style == null)
			{
				style = this.ElementStyle;
			}
			return style;
		}

		// Token: 0x06006610 RID: 26128 RVA: 0x002B089F File Offset: 0x002AF89F
		private static void ApplyBinding(BindingBase binding, DependencyObject target, DependencyProperty property)
		{
			if (binding != null)
			{
				BindingOperations.SetBinding(target, property, binding);
				return;
			}
			BindingOperations.ClearBinding(target, property);
		}

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x06006611 RID: 26129 RVA: 0x002B08B5 File Offset: 0x002AF8B5
		// (set) Token: 0x06006612 RID: 26130 RVA: 0x002A7D55 File Offset: 0x002A6D55
		public override BindingBase ClipboardContentBinding
		{
			get
			{
				return base.ClipboardContentBinding ?? this.EffectiveBinding;
			}
			set
			{
				base.ClipboardContentBinding = value;
			}
		}

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x06006613 RID: 26131 RVA: 0x002B08C7 File Offset: 0x002AF8C7
		// (set) Token: 0x06006614 RID: 26132 RVA: 0x002B08D9 File Offset: 0x002AF8D9
		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)base.GetValue(DataGridComboBoxColumn.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.ItemsSourceProperty, value);
			}
		}

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x06006615 RID: 26133 RVA: 0x002B08E7 File Offset: 0x002AF8E7
		// (set) Token: 0x06006616 RID: 26134 RVA: 0x002B08F9 File Offset: 0x002AF8F9
		public string DisplayMemberPath
		{
			get
			{
				return (string)base.GetValue(DataGridComboBoxColumn.DisplayMemberPathProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.DisplayMemberPathProperty, value);
			}
		}

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x06006617 RID: 26135 RVA: 0x002B0907 File Offset: 0x002AF907
		// (set) Token: 0x06006618 RID: 26136 RVA: 0x002B0919 File Offset: 0x002AF919
		public string SelectedValuePath
		{
			get
			{
				return (string)base.GetValue(DataGridComboBoxColumn.SelectedValuePathProperty);
			}
			set
			{
				base.SetValue(DataGridComboBoxColumn.SelectedValuePathProperty, value);
			}
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x002B0928 File Offset: 0x002AF928
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell == null)
			{
				base.RefreshCellContent(element, propertyName);
				return;
			}
			bool isEditing = dataGridCell.IsEditing;
			if ((string.Compare(propertyName, "ElementStyle", StringComparison.Ordinal) == 0 && !isEditing) || (string.Compare(propertyName, "EditingElementStyle", StringComparison.Ordinal) == 0 && isEditing))
			{
				dataGridCell.BuildVisualTree();
				return;
			}
			ComboBox comboBox = dataGridCell.Content as ComboBox;
			if (propertyName == "SelectedItemBinding")
			{
				DataGridComboBoxColumn.ApplyBinding(this.SelectedItemBinding, comboBox, Selector.SelectedItemProperty);
				return;
			}
			if (propertyName == "SelectedValueBinding")
			{
				DataGridComboBoxColumn.ApplyBinding(this.SelectedValueBinding, comboBox, Selector.SelectedValueProperty);
				return;
			}
			if (propertyName == "TextBinding")
			{
				DataGridComboBoxColumn.ApplyBinding(this.TextBinding, comboBox, ComboBox.TextProperty);
				return;
			}
			if (propertyName == "SelectedValuePath")
			{
				DataGridHelper.SyncColumnProperty(this, comboBox, Selector.SelectedValuePathProperty, DataGridComboBoxColumn.SelectedValuePathProperty);
				return;
			}
			if (propertyName == "DisplayMemberPath")
			{
				DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.DisplayMemberPathProperty, DataGridComboBoxColumn.DisplayMemberPathProperty);
				return;
			}
			if (!(propertyName == "ItemsSource"))
			{
				base.RefreshCellContent(element, propertyName);
				return;
			}
			DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.ItemsSourceProperty, DataGridComboBoxColumn.ItemsSourceProperty);
		}

		// Token: 0x0600661A RID: 26138 RVA: 0x002B0A4D File Offset: 0x002AFA4D
		private object GetComboBoxSelectionValue(ComboBox comboBox)
		{
			if (this.SelectedItemBinding != null)
			{
				return comboBox.SelectedItem;
			}
			if (this.SelectedValueBinding != null)
			{
				return comboBox.SelectedValue;
			}
			return comboBox.Text;
		}

		// Token: 0x0600661B RID: 26139 RVA: 0x002B0A74 File Offset: 0x002AFA74
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			DataGridComboBoxColumn.TextBlockComboBox textBlockComboBox = new DataGridComboBoxColumn.TextBlockComboBox();
			this.ApplyStyle(false, false, textBlockComboBox);
			this.ApplyColumnProperties(textBlockComboBox);
			DataGridHelper.RestoreFlowDirection(textBlockComboBox, cell);
			return textBlockComboBox;
		}

		// Token: 0x0600661C RID: 26140 RVA: 0x002B0AA0 File Offset: 0x002AFAA0
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			ComboBox comboBox = new ComboBox();
			this.ApplyStyle(true, false, comboBox);
			this.ApplyColumnProperties(comboBox);
			DataGridHelper.RestoreFlowDirection(comboBox, cell);
			return comboBox;
		}

		// Token: 0x0600661D RID: 26141 RVA: 0x002B0ACC File Offset: 0x002AFACC
		private void ApplyColumnProperties(ComboBox comboBox)
		{
			DataGridComboBoxColumn.ApplyBinding(this.SelectedItemBinding, comboBox, Selector.SelectedItemProperty);
			DataGridComboBoxColumn.ApplyBinding(this.SelectedValueBinding, comboBox, Selector.SelectedValueProperty);
			DataGridComboBoxColumn.ApplyBinding(this.TextBinding, comboBox, ComboBox.TextProperty);
			DataGridHelper.SyncColumnProperty(this, comboBox, Selector.SelectedValuePathProperty, DataGridComboBoxColumn.SelectedValuePathProperty);
			DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.DisplayMemberPathProperty, DataGridComboBoxColumn.DisplayMemberPathProperty);
			DataGridHelper.SyncColumnProperty(this, comboBox, ItemsControl.ItemsSourceProperty, DataGridComboBoxColumn.ItemsSourceProperty);
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x002B0B40 File Offset: 0x002AFB40
		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			ComboBox comboBox = editingElement as ComboBox;
			if (comboBox != null)
			{
				comboBox.Focus();
				object comboBoxSelectionValue = this.GetComboBoxSelectionValue(comboBox);
				if (DataGridComboBoxColumn.IsComboBoxOpeningInputEvent(editingEventArgs))
				{
					comboBox.IsDropDownOpen = true;
				}
				return comboBoxSelectionValue;
			}
			return null;
		}

		// Token: 0x0600661F RID: 26143 RVA: 0x002B0B78 File Offset: 0x002AFB78
		protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			ComboBox comboBox = editingElement as ComboBox;
			if (comboBox != null && comboBox.EditableTextBoxSite != null)
			{
				DataGridHelper.CacheFlowDirection(comboBox.EditableTextBoxSite, comboBox.Parent as DataGridCell);
				DataGridHelper.CacheFlowDirection(comboBox, comboBox.Parent as DataGridCell);
			}
			base.CancelCellEdit(editingElement, uneditedValue);
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x002B0BC8 File Offset: 0x002AFBC8
		protected override bool CommitCellEdit(FrameworkElement editingElement)
		{
			ComboBox comboBox = editingElement as ComboBox;
			if (comboBox != null && comboBox.EditableTextBoxSite != null)
			{
				DataGridHelper.CacheFlowDirection(comboBox.EditableTextBoxSite, comboBox.Parent as DataGridCell);
				DataGridHelper.CacheFlowDirection(comboBox, comboBox.Parent as DataGridCell);
			}
			return base.CommitCellEdit(editingElement);
		}

		// Token: 0x06006621 RID: 26145 RVA: 0x002B0C15 File Offset: 0x002AFC15
		internal override void OnInput(InputEventArgs e)
		{
			if (DataGridComboBoxColumn.IsComboBoxOpeningInputEvent(e))
			{
				base.BeginEdit(e, true);
			}
		}

		// Token: 0x06006622 RID: 26146 RVA: 0x002B0C28 File Offset: 0x002AFC28
		private static bool IsComboBoxOpeningInputEvent(RoutedEventArgs e)
		{
			KeyEventArgs keyEventArgs = e as KeyEventArgs;
			if (keyEventArgs != null && keyEventArgs.RoutedEvent == Keyboard.KeyDownEvent && (keyEventArgs.KeyStates & KeyStates.Down) == KeyStates.Down)
			{
				bool flag = (keyEventArgs.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
				Key key = keyEventArgs.Key;
				if (key == Key.System)
				{
					key = keyEventArgs.SystemKey;
				}
				return (key == Key.F4 && !flag) || ((key == Key.Up || key == Key.Down) && flag);
			}
			return false;
		}

		// Token: 0x040033A1 RID: 13217
		public static readonly DependencyProperty ElementStyleProperty = DataGridBoundColumn.ElementStyleProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(DataGridComboBoxColumn.DefaultElementStyle));

		// Token: 0x040033A2 RID: 13218
		public static readonly DependencyProperty EditingElementStyleProperty = DataGridBoundColumn.EditingElementStyleProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(DataGridComboBoxColumn.DefaultEditingElementStyle));

		// Token: 0x040033A3 RID: 13219
		public static readonly DependencyProperty ItemsSourceProperty = ItemsControl.ItemsSourceProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x040033A4 RID: 13220
		public static readonly DependencyProperty DisplayMemberPathProperty = ItemsControl.DisplayMemberPathProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x040033A5 RID: 13221
		public static readonly DependencyProperty SelectedValuePathProperty = Selector.SelectedValuePathProperty.AddOwner(typeof(DataGridComboBoxColumn), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x040033A6 RID: 13222
		private static Style _defaultElementStyle;

		// Token: 0x040033A7 RID: 13223
		private BindingBase _selectedValueBinding;

		// Token: 0x040033A8 RID: 13224
		private BindingBase _selectedItemBinding;

		// Token: 0x040033A9 RID: 13225
		private BindingBase _textBinding;

		// Token: 0x02000BCD RID: 3021
		internal class TextBlockComboBox : ComboBox
		{
			// Token: 0x06008F7B RID: 36731 RVA: 0x00344596 File Offset: 0x00343596
			static TextBlockComboBox()
			{
				FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridComboBoxColumn.TextBlockComboBox), new FrameworkPropertyMetadata(DataGridComboBoxColumn.TextBlockComboBoxStyleKey));
				KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(DataGridComboBoxColumn.TextBlockComboBox), new FrameworkPropertyMetadata(false));
			}
		}
	}
}
