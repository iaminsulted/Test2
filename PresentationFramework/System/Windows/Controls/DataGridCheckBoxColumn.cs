using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace System.Windows.Controls
{
	// Token: 0x02000747 RID: 1863
	public class DataGridCheckBoxColumn : DataGridBoundColumn
	{
		// Token: 0x060064E4 RID: 25828 RVA: 0x002AB308 File Offset: 0x002AA308
		static DataGridCheckBoxColumn()
		{
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridCheckBoxColumn), new FrameworkPropertyMetadata(DataGridCheckBoxColumn.DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridCheckBoxColumn), new FrameworkPropertyMetadata(DataGridCheckBoxColumn.DefaultEditingElementStyle));
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x060064E5 RID: 25829 RVA: 0x002AB384 File Offset: 0x002AA384
		public static Style DefaultElementStyle
		{
			get
			{
				if (DataGridCheckBoxColumn._defaultElementStyle == null)
				{
					Style style = new Style(typeof(CheckBox));
					style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
					style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
					style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
					style.Seal();
					DataGridCheckBoxColumn._defaultElementStyle = style;
				}
				return DataGridCheckBoxColumn._defaultElementStyle;
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x060064E6 RID: 25830 RVA: 0x002AB428 File Offset: 0x002AA428
		public static Style DefaultEditingElementStyle
		{
			get
			{
				if (DataGridCheckBoxColumn._defaultEditingElementStyle == null)
				{
					Style style = new Style(typeof(CheckBox));
					style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
					style.Seal();
					DataGridCheckBoxColumn._defaultEditingElementStyle = style;
				}
				return DataGridCheckBoxColumn._defaultEditingElementStyle;
			}
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x002AB491 File Offset: 0x002AA491
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			return this.GenerateCheckBox(false, cell);
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x002AB49B File Offset: 0x002AA49B
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return this.GenerateCheckBox(true, cell);
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x002AB4A8 File Offset: 0x002AA4A8
		private CheckBox GenerateCheckBox(bool isEditing, DataGridCell cell)
		{
			CheckBox checkBox = (cell != null) ? (cell.Content as CheckBox) : null;
			if (checkBox == null)
			{
				checkBox = new CheckBox();
			}
			checkBox.IsThreeState = this.IsThreeState;
			base.ApplyStyle(isEditing, true, checkBox);
			base.ApplyBinding(checkBox, ToggleButton.IsCheckedProperty);
			return checkBox;
		}

		// Token: 0x060064EA RID: 25834 RVA: 0x002AB4F4 File Offset: 0x002AA4F4
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null && string.Compare(propertyName, "IsThreeState", StringComparison.Ordinal) == 0)
			{
				CheckBox checkBox = dataGridCell.Content as CheckBox;
				if (checkBox != null)
				{
					checkBox.IsThreeState = this.IsThreeState;
					return;
				}
			}
			else
			{
				base.RefreshCellContent(element, propertyName);
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x060064EB RID: 25835 RVA: 0x002AB53D File Offset: 0x002AA53D
		// (set) Token: 0x060064EC RID: 25836 RVA: 0x002AB54F File Offset: 0x002AA54F
		public bool IsThreeState
		{
			get
			{
				return (bool)base.GetValue(DataGridCheckBoxColumn.IsThreeStateProperty);
			}
			set
			{
				base.SetValue(DataGridCheckBoxColumn.IsThreeStateProperty, value);
			}
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x002AB560 File Offset: 0x002AA560
		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			CheckBox checkBox = editingElement as CheckBox;
			if (checkBox != null)
			{
				checkBox.Focus();
				bool? isChecked = checkBox.IsChecked;
				if ((DataGridCheckBoxColumn.IsMouseLeftButtonDown(editingEventArgs) && DataGridCheckBoxColumn.IsMouseOver(checkBox, editingEventArgs)) || DataGridCheckBoxColumn.IsSpaceKeyDown(editingEventArgs))
				{
					ToggleButton toggleButton = checkBox;
					bool? flag = isChecked;
					bool flag2 = true;
					toggleButton.IsChecked = new bool?(!(flag.GetValueOrDefault() == flag2 & flag != null));
				}
				return isChecked;
			}
			return false;
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x002AB5CF File Offset: 0x002AA5CF
		internal override void OnInput(InputEventArgs e)
		{
			if (DataGridCheckBoxColumn.IsSpaceKeyDown(e))
			{
				base.BeginEdit(e, true);
			}
		}

		// Token: 0x060064EF RID: 25839 RVA: 0x002AB5E4 File Offset: 0x002AA5E4
		private static bool IsMouseLeftButtonDown(RoutedEventArgs e)
		{
			MouseButtonEventArgs mouseButtonEventArgs = e as MouseButtonEventArgs;
			return mouseButtonEventArgs != null && mouseButtonEventArgs.ChangedButton == MouseButton.Left && mouseButtonEventArgs.ButtonState == MouseButtonState.Pressed;
		}

		// Token: 0x060064F0 RID: 25840 RVA: 0x002AB60E File Offset: 0x002AA60E
		private static bool IsMouseOver(CheckBox checkBox, RoutedEventArgs e)
		{
			return checkBox.InputHitTest(((MouseButtonEventArgs)e).GetPosition(checkBox)) != null;
		}

		// Token: 0x060064F1 RID: 25841 RVA: 0x002AB628 File Offset: 0x002AA628
		private static bool IsSpaceKeyDown(RoutedEventArgs e)
		{
			KeyEventArgs keyEventArgs = e as KeyEventArgs;
			return keyEventArgs != null && keyEventArgs.RoutedEvent == Keyboard.KeyDownEvent && (keyEventArgs.KeyStates & KeyStates.Down) == KeyStates.Down && keyEventArgs.Key == Key.Space;
		}

		// Token: 0x0400335A RID: 13146
		public static readonly DependencyProperty IsThreeStateProperty = ToggleButton.IsThreeStateProperty.AddOwner(typeof(DataGridCheckBoxColumn), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400335B RID: 13147
		private static Style _defaultElementStyle;

		// Token: 0x0400335C RID: 13148
		private static Style _defaultEditingElementStyle;
	}
}
