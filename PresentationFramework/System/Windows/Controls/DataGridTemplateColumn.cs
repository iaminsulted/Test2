using System;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x0200076A RID: 1898
	public class DataGridTemplateColumn : DataGridColumn
	{
		// Token: 0x06006705 RID: 26373 RVA: 0x002B3734 File Offset: 0x002B2734
		static DataGridTemplateColumn()
		{
			DataGridColumn.CanUserSortProperty.OverrideMetadata(typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridTemplateColumn.OnCoerceTemplateColumnCanUserSort)));
			DataGridColumn.SortMemberPathProperty.OverrideMetadata(typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridTemplateColumn.OnTemplateColumnSortMemberPathChanged)));
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x002B3860 File Offset: 0x002B2860
		private static void OnTemplateColumnSortMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridTemplateColumn)d).CoerceValue(DataGridColumn.CanUserSortProperty);
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x002B3872 File Offset: 0x002B2872
		private static object OnCoerceTemplateColumnCanUserSort(DependencyObject d, object baseValue)
		{
			if (string.IsNullOrEmpty(((DataGridTemplateColumn)d).SortMemberPath))
			{
				return false;
			}
			return DataGridColumn.OnCoerceCanUserSort(d, baseValue);
		}

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x06006708 RID: 26376 RVA: 0x002B3894 File Offset: 0x002B2894
		// (set) Token: 0x06006709 RID: 26377 RVA: 0x002B38A6 File Offset: 0x002B28A6
		public DataTemplate CellTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridTemplateColumn.CellTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellTemplateProperty, value);
			}
		}

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600670A RID: 26378 RVA: 0x002B38B4 File Offset: 0x002B28B4
		// (set) Token: 0x0600670B RID: 26379 RVA: 0x002B38C6 File Offset: 0x002B28C6
		public DataTemplateSelector CellTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridTemplateColumn.CellTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellTemplateSelectorProperty, value);
			}
		}

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x0600670C RID: 26380 RVA: 0x002B38D4 File Offset: 0x002B28D4
		// (set) Token: 0x0600670D RID: 26381 RVA: 0x002B38E6 File Offset: 0x002B28E6
		public DataTemplate CellEditingTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridTemplateColumn.CellEditingTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellEditingTemplateProperty, value);
			}
		}

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x0600670E RID: 26382 RVA: 0x002B38F4 File Offset: 0x002B28F4
		// (set) Token: 0x0600670F RID: 26383 RVA: 0x002B3906 File Offset: 0x002B2906
		public DataTemplateSelector CellEditingTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x002B3914 File Offset: 0x002B2914
		private void ChooseCellTemplateAndSelector(bool isEditing, out DataTemplate template, out DataTemplateSelector templateSelector)
		{
			template = null;
			templateSelector = null;
			if (isEditing)
			{
				template = this.CellEditingTemplate;
				templateSelector = this.CellEditingTemplateSelector;
			}
			if (template == null && templateSelector == null)
			{
				template = this.CellTemplate;
				templateSelector = this.CellTemplateSelector;
			}
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x002B3948 File Offset: 0x002B2948
		private FrameworkElement LoadTemplateContent(bool isEditing, object dataItem, DataGridCell cell)
		{
			DataTemplate dataTemplate;
			DataTemplateSelector dataTemplateSelector;
			this.ChooseCellTemplateAndSelector(isEditing, out dataTemplate, out dataTemplateSelector);
			if (dataTemplate != null || dataTemplateSelector != null)
			{
				ContentPresenter contentPresenter = new ContentPresenter();
				BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, new Binding());
				contentPresenter.ContentTemplate = dataTemplate;
				contentPresenter.ContentTemplateSelector = dataTemplateSelector;
				return contentPresenter;
			}
			return null;
		}

		// Token: 0x06006712 RID: 26386 RVA: 0x002B398C File Offset: 0x002B298C
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			return this.LoadTemplateContent(false, dataItem, cell);
		}

		// Token: 0x06006713 RID: 26387 RVA: 0x002B3997 File Offset: 0x002B2997
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return this.LoadTemplateContent(true, dataItem, cell);
		}

		// Token: 0x06006714 RID: 26388 RVA: 0x002B39A4 File Offset: 0x002B29A4
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				bool isEditing = dataGridCell.IsEditing;
				if ((!isEditing && (string.Compare(propertyName, "CellTemplate", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "CellTemplateSelector", StringComparison.Ordinal) == 0)) || (isEditing && (string.Compare(propertyName, "CellEditingTemplate", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "CellEditingTemplateSelector", StringComparison.Ordinal) == 0)))
				{
					dataGridCell.BuildVisualTree();
					return;
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		// Token: 0x04003423 RID: 13347
		public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x04003424 RID: 13348
		public static readonly DependencyProperty CellTemplateSelectorProperty = DependencyProperty.Register("CellTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x04003425 RID: 13349
		public static readonly DependencyProperty CellEditingTemplateProperty = DependencyProperty.Register("CellEditingTemplate", typeof(DataTemplate), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x04003426 RID: 13350
		public static readonly DependencyProperty CellEditingTemplateSelectorProperty = DependencyProperty.Register("CellEditingTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));
	}
}
