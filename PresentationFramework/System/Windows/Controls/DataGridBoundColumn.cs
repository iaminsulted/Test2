using System;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x02000741 RID: 1857
	public abstract class DataGridBoundColumn : DataGridColumn
	{
		// Token: 0x06006432 RID: 25650 RVA: 0x002A7B38 File Offset: 0x002A6B38
		static DataGridBoundColumn()
		{
			DataGridColumn.SortMemberPathProperty.OverrideMetadata(typeof(DataGridBoundColumn), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridBoundColumn.OnCoerceSortMemberPath)));
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x002A7BD8 File Offset: 0x002A6BD8
		private static object OnCoerceSortMemberPath(DependencyObject d, object baseValue)
		{
			DataGridBoundColumn dataGridBoundColumn = (DataGridBoundColumn)d;
			string text = (string)baseValue;
			if (string.IsNullOrEmpty(text))
			{
				string pathFromBinding = DataGridHelper.GetPathFromBinding(dataGridBoundColumn.Binding as Binding);
				if (!string.IsNullOrEmpty(pathFromBinding))
				{
					text = pathFromBinding;
				}
			}
			return text;
		}

		// Token: 0x1700172A RID: 5930
		// (get) Token: 0x06006434 RID: 25652 RVA: 0x002A7C17 File Offset: 0x002A6C17
		// (set) Token: 0x06006435 RID: 25653 RVA: 0x002A7C20 File Offset: 0x002A6C20
		public virtual BindingBase Binding
		{
			get
			{
				return this._binding;
			}
			set
			{
				if (this._binding != value)
				{
					BindingBase binding = this._binding;
					this._binding = value;
					base.CoerceValue(DataGridColumn.IsReadOnlyProperty);
					base.CoerceValue(DataGridColumn.SortMemberPathProperty);
					this.OnBindingChanged(binding, this._binding);
				}
			}
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x002A7C67 File Offset: 0x002A6C67
		protected override bool OnCoerceIsReadOnly(bool baseValue)
		{
			return DataGridHelper.IsOneWay(this._binding) || base.OnCoerceIsReadOnly(baseValue);
		}

		// Token: 0x06006437 RID: 25655 RVA: 0x002A7C7F File Offset: 0x002A6C7F
		protected virtual void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("Binding");
		}

		// Token: 0x06006438 RID: 25656 RVA: 0x002A7C8C File Offset: 0x002A6C8C
		internal void ApplyBinding(DependencyObject target, DependencyProperty property)
		{
			BindingBase binding = this.Binding;
			if (binding != null)
			{
				BindingOperations.SetBinding(target, property, binding);
				return;
			}
			BindingOperations.ClearBinding(target, property);
		}

		// Token: 0x1700172B RID: 5931
		// (get) Token: 0x06006439 RID: 25657 RVA: 0x002A7CB4 File Offset: 0x002A6CB4
		// (set) Token: 0x0600643A RID: 25658 RVA: 0x002A7CC6 File Offset: 0x002A6CC6
		public Style ElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridBoundColumn.ElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridBoundColumn.ElementStyleProperty, value);
			}
		}

		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x0600643B RID: 25659 RVA: 0x002A7CD4 File Offset: 0x002A6CD4
		// (set) Token: 0x0600643C RID: 25660 RVA: 0x002A7CE6 File Offset: 0x002A6CE6
		public Style EditingElementStyle
		{
			get
			{
				return (Style)base.GetValue(DataGridBoundColumn.EditingElementStyleProperty);
			}
			set
			{
				base.SetValue(DataGridBoundColumn.EditingElementStyleProperty, value);
			}
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x002A7CF4 File Offset: 0x002A6CF4
		internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
		{
			Style style = this.PickStyle(isEditing, defaultToElementStyle);
			if (style != null)
			{
				element.Style = style;
			}
		}

		// Token: 0x0600643E RID: 25662 RVA: 0x002A7D14 File Offset: 0x002A6D14
		private Style PickStyle(bool isEditing, bool defaultToElementStyle)
		{
			Style style = isEditing ? this.EditingElementStyle : this.ElementStyle;
			if (isEditing && defaultToElementStyle && style == null)
			{
				style = this.ElementStyle;
			}
			return style;
		}

		// Token: 0x1700172D RID: 5933
		// (get) Token: 0x0600643F RID: 25663 RVA: 0x002A7D43 File Offset: 0x002A6D43
		// (set) Token: 0x06006440 RID: 25664 RVA: 0x002A7D55 File Offset: 0x002A6D55
		public override BindingBase ClipboardContentBinding
		{
			get
			{
				return base.ClipboardContentBinding ?? this.Binding;
			}
			set
			{
				base.ClipboardContentBinding = value;
			}
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x002A7D60 File Offset: 0x002A6D60
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				bool isEditing = dataGridCell.IsEditing;
				if (string.Compare(propertyName, "Binding", StringComparison.Ordinal) == 0 || (string.Compare(propertyName, "ElementStyle", StringComparison.Ordinal) == 0 && !isEditing) || (string.Compare(propertyName, "EditingElementStyle", StringComparison.Ordinal) == 0 && isEditing))
				{
					dataGridCell.BuildVisualTree();
					return;
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		// Token: 0x0400333B RID: 13115
		public static readonly DependencyProperty ElementStyleProperty = DependencyProperty.Register("ElementStyle", typeof(Style), typeof(DataGridBoundColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400333C RID: 13116
		public static readonly DependencyProperty EditingElementStyleProperty = DependencyProperty.Register("EditingElementStyle", typeof(Style), typeof(DataGridBoundColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400333D RID: 13117
		private BindingBase _binding;
	}
}
