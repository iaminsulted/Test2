using System;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace System.Windows.Controls
{
	// Token: 0x02000759 RID: 1881
	public class DataGridHyperlinkColumn : DataGridBoundColumn
	{
		// Token: 0x06006652 RID: 26194 RVA: 0x002B158C File Offset: 0x002B058C
		static DataGridHyperlinkColumn()
		{
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridHyperlinkColumn), new FrameworkPropertyMetadata(DataGridHyperlinkColumn.DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridHyperlinkColumn), new FrameworkPropertyMetadata(DataGridHyperlinkColumn.DefaultEditingElementStyle));
		}

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x06006653 RID: 26195 RVA: 0x002B1600 File Offset: 0x002B0600
		// (set) Token: 0x06006654 RID: 26196 RVA: 0x002B1612 File Offset: 0x002B0612
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(DataGridHyperlinkColumn.TargetNameProperty);
			}
			set
			{
				base.SetValue(DataGridHyperlinkColumn.TargetNameProperty, value);
			}
		}

		// Token: 0x17001798 RID: 6040
		// (get) Token: 0x06006655 RID: 26197 RVA: 0x002B1620 File Offset: 0x002B0620
		// (set) Token: 0x06006656 RID: 26198 RVA: 0x002B1628 File Offset: 0x002B0628
		public BindingBase ContentBinding
		{
			get
			{
				return this._contentBinding;
			}
			set
			{
				if (this._contentBinding != value)
				{
					BindingBase contentBinding = this._contentBinding;
					this._contentBinding = value;
					this.OnContentBindingChanged(contentBinding, value);
				}
			}
		}

		// Token: 0x06006657 RID: 26199 RVA: 0x002B1654 File Offset: 0x002B0654
		protected virtual void OnContentBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("ContentBinding");
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x002B1661 File Offset: 0x002B0661
		private void ApplyContentBinding(DependencyObject target, DependencyProperty property)
		{
			if (this.ContentBinding != null)
			{
				BindingOperations.SetBinding(target, property, this.ContentBinding);
				return;
			}
			if (this.Binding != null)
			{
				BindingOperations.SetBinding(target, property, this.Binding);
				return;
			}
			BindingOperations.ClearBinding(target, property);
		}

		// Token: 0x06006659 RID: 26201 RVA: 0x002B1698 File Offset: 0x002B0698
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null && !dataGridCell.IsEditing)
			{
				if (string.Compare(propertyName, "ContentBinding", StringComparison.Ordinal) == 0)
				{
					dataGridCell.BuildVisualTree();
					return;
				}
				if (string.Compare(propertyName, "TargetName", StringComparison.Ordinal) == 0)
				{
					TextBlock textBlock = dataGridCell.Content as TextBlock;
					if (textBlock != null && textBlock.Inlines.Count > 0)
					{
						Hyperlink hyperlink = textBlock.Inlines.FirstInline as Hyperlink;
						if (hyperlink != null)
						{
							hyperlink.TargetName = this.TargetName;
							return;
						}
					}
				}
			}
			else
			{
				base.RefreshCellContent(element, propertyName);
			}
		}

		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x0600665A RID: 26202 RVA: 0x002B1720 File Offset: 0x002B0720
		public static Style DefaultElementStyle
		{
			get
			{
				return DataGridTextColumn.DefaultElementStyle;
			}
		}

		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x002B1727 File Offset: 0x002B0727
		public static Style DefaultEditingElementStyle
		{
			get
			{
				return DataGridTextColumn.DefaultEditingElementStyle;
			}
		}

		// Token: 0x0600665C RID: 26204 RVA: 0x002B1730 File Offset: 0x002B0730
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			TextBlock textBlock = new TextBlock();
			Hyperlink hyperlink = new Hyperlink();
			InlineUIContainer inlineUIContainer = new InlineUIContainer();
			ContentPresenter contentPresenter = new ContentPresenter();
			textBlock.Inlines.Add(hyperlink);
			hyperlink.Inlines.Add(inlineUIContainer);
			inlineUIContainer.Child = contentPresenter;
			hyperlink.TargetName = this.TargetName;
			base.ApplyStyle(false, false, textBlock);
			base.ApplyBinding(hyperlink, Hyperlink.NavigateUriProperty);
			this.ApplyContentBinding(contentPresenter, ContentPresenter.ContentProperty);
			DataGridHelper.RestoreFlowDirection(textBlock, cell);
			return textBlock;
		}

		// Token: 0x0600665D RID: 26205 RVA: 0x002B17AC File Offset: 0x002B07AC
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			TextBox textBox = new TextBox();
			base.ApplyStyle(true, false, textBox);
			base.ApplyBinding(textBox, TextBox.TextProperty);
			DataGridHelper.RestoreFlowDirection(textBox, cell);
			return textBox;
		}

		// Token: 0x0600665E RID: 26206 RVA: 0x002B17DC File Offset: 0x002B07DC
		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			TextBox textBox = editingElement as TextBox;
			if (textBox == null)
			{
				return null;
			}
			textBox.Focus();
			string text = textBox.Text;
			TextCompositionEventArgs textCompositionEventArgs = editingEventArgs as TextCompositionEventArgs;
			if (textCompositionEventArgs != null)
			{
				string text2 = textCompositionEventArgs.Text;
				textBox.Text = text2;
				textBox.Select(text2.Length, 0);
				return text;
			}
			textBox.SelectAll();
			return text;
		}

		// Token: 0x0600665F RID: 26207 RVA: 0x002B182E File Offset: 0x002B082E
		protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			base.CancelCellEdit(editingElement, uneditedValue);
		}

		// Token: 0x06006660 RID: 26208 RVA: 0x002B184F File Offset: 0x002B084F
		protected override bool CommitCellEdit(FrameworkElement editingElement)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			return base.CommitCellEdit(editingElement);
		}

		// Token: 0x06006661 RID: 26209 RVA: 0x002B1870 File Offset: 0x002B0870
		internal override void OnInput(InputEventArgs e)
		{
			if (DataGridHelper.HasNonEscapeCharacters(e as TextCompositionEventArgs))
			{
				base.BeginEdit(e, true);
				return;
			}
			if (DataGridHelper.IsImeProcessed(e as KeyEventArgs) && base.DataGridOwner != null)
			{
				DataGridCell currentCellContainer = base.DataGridOwner.CurrentCellContainer;
				if (currentCellContainer != null && !currentCellContainer.IsEditing)
				{
					base.BeginEdit(e, false);
					base.Dispatcher.Invoke(delegate()
					{
					}, DispatcherPriority.Background);
				}
			}
		}

		// Token: 0x040033BD RID: 13245
		public static readonly DependencyProperty TargetNameProperty = Hyperlink.TargetNameProperty.AddOwner(typeof(DataGridHyperlinkColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x040033BE RID: 13246
		private BindingBase _contentBinding;
	}
}
