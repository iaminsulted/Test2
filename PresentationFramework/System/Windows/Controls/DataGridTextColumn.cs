using System;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace System.Windows.Controls
{
	// Token: 0x0200076B RID: 1899
	public class DataGridTextColumn : DataGridBoundColumn
	{
		// Token: 0x06006716 RID: 26390 RVA: 0x002B3A10 File Offset: 0x002B2A10
		static DataGridTextColumn()
		{
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(DataGridTextColumn.DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(DataGridTextColumn.DefaultEditingElementStyle));
		}

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x06006717 RID: 26391 RVA: 0x002B3B60 File Offset: 0x002B2B60
		public static Style DefaultElementStyle
		{
			get
			{
				if (DataGridTextColumn._defaultElementStyle == null)
				{
					Style style = new Style(typeof(TextBlock));
					style.Setters.Add(new Setter(FrameworkElement.MarginProperty, new Thickness(2.0, 0.0, 2.0, 0.0)));
					style.Seal();
					DataGridTextColumn._defaultElementStyle = style;
				}
				return DataGridTextColumn._defaultElementStyle;
			}
		}

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x06006718 RID: 26392 RVA: 0x002B3BD8 File Offset: 0x002B2BD8
		public static Style DefaultEditingElementStyle
		{
			get
			{
				if (DataGridTextColumn._defaultEditingElementStyle == null)
				{
					Style style = new Style(typeof(TextBox));
					style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0.0)));
					style.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(0.0)));
					style.Seal();
					DataGridTextColumn._defaultEditingElementStyle = style;
				}
				return DataGridTextColumn._defaultEditingElementStyle;
			}
		}

		// Token: 0x06006719 RID: 26393 RVA: 0x002B3C5C File Offset: 0x002B2C5C
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			TextBlock textBlock = new TextBlock();
			this.SyncProperties(textBlock);
			base.ApplyStyle(false, false, textBlock);
			base.ApplyBinding(textBlock, TextBlock.TextProperty);
			DataGridHelper.RestoreFlowDirection(textBlock, cell);
			return textBlock;
		}

		// Token: 0x0600671A RID: 26394 RVA: 0x002B3C94 File Offset: 0x002B2C94
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			TextBox textBox = new TextBox();
			this.SyncProperties(textBox);
			base.ApplyStyle(true, false, textBox);
			base.ApplyBinding(textBox, TextBox.TextProperty);
			DataGridHelper.RestoreFlowDirection(textBox, cell);
			return textBox;
		}

		// Token: 0x0600671B RID: 26395 RVA: 0x002B3CCC File Offset: 0x002B2CCC
		private void SyncProperties(FrameworkElement e)
		{
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontFamilyProperty, DataGridTextColumn.FontFamilyProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontSizeProperty, DataGridTextColumn.FontSizeProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontStyleProperty, DataGridTextColumn.FontStyleProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontWeightProperty, DataGridTextColumn.FontWeightProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.ForegroundProperty, DataGridTextColumn.ForegroundProperty);
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x002B3D30 File Offset: 0x002B2D30
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				FrameworkElement frameworkElement = dataGridCell.Content as FrameworkElement;
				if (frameworkElement != null)
				{
					if (!(propertyName == "FontFamily"))
					{
						if (!(propertyName == "FontSize"))
						{
							if (!(propertyName == "FontStyle"))
							{
								if (!(propertyName == "FontWeight"))
								{
									if (propertyName == "Foreground")
									{
										DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.ForegroundProperty, DataGridTextColumn.ForegroundProperty);
									}
								}
								else
								{
									DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontWeightProperty, DataGridTextColumn.FontWeightProperty);
								}
							}
							else
							{
								DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontStyleProperty, DataGridTextColumn.FontStyleProperty);
							}
						}
						else
						{
							DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontSizeProperty, DataGridTextColumn.FontSizeProperty);
						}
					}
					else
					{
						DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontFamilyProperty, DataGridTextColumn.FontFamilyProperty);
					}
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x002B3E04 File Offset: 0x002B2E04
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
				string text2 = this.ConvertTextForEdit(textCompositionEventArgs.Text);
				textBox.Text = text2;
				textBox.Select(text2.Length, 0);
				return text;
			}
			if (!(editingEventArgs is MouseButtonEventArgs) || !DataGridTextColumn.PlaceCaretOnTextBox(textBox, Mouse.GetPosition(textBox)))
			{
				textBox.SelectAll();
			}
			return text;
		}

		// Token: 0x0600671E RID: 26398 RVA: 0x002B3E72 File Offset: 0x002B2E72
		private string ConvertTextForEdit(string s)
		{
			if (s == "\b")
			{
				s = string.Empty;
			}
			return s;
		}

		// Token: 0x0600671F RID: 26399 RVA: 0x002B182E File Offset: 0x002B082E
		protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			base.CancelCellEdit(editingElement, uneditedValue);
		}

		// Token: 0x06006720 RID: 26400 RVA: 0x002B184F File Offset: 0x002B084F
		protected override bool CommitCellEdit(FrameworkElement editingElement)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			return base.CommitCellEdit(editingElement);
		}

		// Token: 0x06006721 RID: 26401 RVA: 0x002B3E8C File Offset: 0x002B2E8C
		private static bool PlaceCaretOnTextBox(TextBox textBox, Point position)
		{
			int characterIndexFromPoint = textBox.GetCharacterIndexFromPoint(position, false);
			if (characterIndexFromPoint >= 0)
			{
				textBox.Select(characterIndexFromPoint, 0);
				return true;
			}
			return false;
		}

		// Token: 0x06006722 RID: 26402 RVA: 0x002B3EB4 File Offset: 0x002B2EB4
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

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x06006723 RID: 26403 RVA: 0x002B3F36 File Offset: 0x002B2F36
		// (set) Token: 0x06006724 RID: 26404 RVA: 0x002B3F48 File Offset: 0x002B2F48
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(DataGridTextColumn.FontFamilyProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontFamilyProperty, value);
			}
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x06006725 RID: 26405 RVA: 0x002B3F56 File Offset: 0x002B2F56
		// (set) Token: 0x06006726 RID: 26406 RVA: 0x002B3F68 File Offset: 0x002B2F68
		[Localizability(LocalizationCategory.None)]
		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(DataGridTextColumn.FontSizeProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontSizeProperty, value);
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x06006727 RID: 26407 RVA: 0x002B3F7B File Offset: 0x002B2F7B
		// (set) Token: 0x06006728 RID: 26408 RVA: 0x002B3F8D File Offset: 0x002B2F8D
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(DataGridTextColumn.FontStyleProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontStyleProperty, value);
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x06006729 RID: 26409 RVA: 0x002B3FA0 File Offset: 0x002B2FA0
		// (set) Token: 0x0600672A RID: 26410 RVA: 0x002B3FB2 File Offset: 0x002B2FB2
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(DataGridTextColumn.FontWeightProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontWeightProperty, value);
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x0600672B RID: 26411 RVA: 0x002B3FC5 File Offset: 0x002B2FC5
		// (set) Token: 0x0600672C RID: 26412 RVA: 0x002B3FD7 File Offset: 0x002B2FD7
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(DataGridTextColumn.ForegroundProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.ForegroundProperty, value);
			}
		}

		// Token: 0x04003427 RID: 13351
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x04003428 RID: 13352
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x04003429 RID: 13353
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400342A RID: 13354
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400342B RID: 13355
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400342C RID: 13356
		private static Style _defaultElementStyle;

		// Token: 0x0400342D RID: 13357
		private static Style _defaultEditingElementStyle;
	}
}
