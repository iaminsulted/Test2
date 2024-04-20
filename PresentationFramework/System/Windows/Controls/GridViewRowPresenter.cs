using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200078C RID: 1932
	public class GridViewRowPresenter : GridViewRowPresenterBase
	{
		// Token: 0x06006B33 RID: 27443 RVA: 0x002C4EAC File Offset: 0x002C3EAC
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridViewRowPresenter", new object[]
			{
				base.GetType(),
				(this.Content != null) ? this.Content.ToString() : string.Empty,
				(base.Columns != null) ? base.Columns.Count : 0
			});
		}

		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x06006B34 RID: 27444 RVA: 0x002C4F0D File Offset: 0x002C3F0D
		// (set) Token: 0x06006B35 RID: 27445 RVA: 0x002C4F1A File Offset: 0x002C3F1A
		public object Content
		{
			get
			{
				return base.GetValue(GridViewRowPresenter.ContentProperty);
			}
			set
			{
				base.SetValue(GridViewRowPresenter.ContentProperty, value);
			}
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x002C4F28 File Offset: 0x002C3F28
		private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewRowPresenter gridViewRowPresenter = (GridViewRowPresenter)d;
			Type type = (e.OldValue != null) ? e.OldValue.GetType() : null;
			Type right = (e.NewValue != null) ? e.NewValue.GetType() : null;
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				gridViewRowPresenter._oldContentType = type;
				right = type;
			}
			else if (e.OldValue == BindingExpressionBase.DisconnectedItem)
			{
				type = gridViewRowPresenter._oldContentType;
			}
			if (type != right)
			{
				gridViewRowPresenter.NeedUpdateVisualTree = true;
				return;
			}
			gridViewRowPresenter.UpdateCells();
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x002C4FB4 File Offset: 0x002C3FB4
		protected override Size MeasureOverride(Size constraint)
		{
			GridViewColumnCollection columns = base.Columns;
			if (columns == null)
			{
				return default(Size);
			}
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = 0.0;
			double height = constraint.Height;
			bool flag = false;
			foreach (GridViewColumn gridViewColumn in columns)
			{
				UIElement uielement = internalChildren[gridViewColumn.ActualIndex];
				if (uielement != null)
				{
					double num3 = Math.Max(0.0, constraint.Width - num2);
					if (gridViewColumn.State == ColumnMeasureState.Init || gridViewColumn.State == ColumnMeasureState.Headered)
					{
						if (!flag)
						{
							base.EnsureDesiredWidthList();
							base.LayoutUpdated += this.OnLayoutUpdated;
							flag = true;
						}
						uielement.Measure(new Size(num3, height));
						if (this.IsOnCurrentPage)
						{
							gridViewColumn.EnsureWidth(uielement.DesiredSize.Width);
						}
						base.DesiredWidthList[gridViewColumn.ActualIndex] = gridViewColumn.DesiredWidth;
						num2 += gridViewColumn.DesiredWidth;
					}
					else if (gridViewColumn.State == ColumnMeasureState.Data)
					{
						num3 = Math.Min(num3, gridViewColumn.DesiredWidth);
						uielement.Measure(new Size(num3, height));
						num2 += gridViewColumn.DesiredWidth;
					}
					else
					{
						num3 = Math.Min(num3, gridViewColumn.Width);
						uielement.Measure(new Size(num3, height));
						num2 += gridViewColumn.Width;
					}
					num = Math.Max(num, uielement.DesiredSize.Height);
				}
			}
			this._isOnCurrentPageValid = false;
			num2 += 2.0;
			return new Size(num2, num);
		}

		// Token: 0x06006B38 RID: 27448 RVA: 0x002C5190 File Offset: 0x002C4190
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			GridViewColumnCollection columns = base.Columns;
			if (columns == null)
			{
				return arrangeSize;
			}
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = arrangeSize.Width;
			foreach (GridViewColumn gridViewColumn in columns)
			{
				UIElement uielement = internalChildren[gridViewColumn.ActualIndex];
				if (uielement != null)
				{
					double num3 = Math.Min(num2, (gridViewColumn.State == ColumnMeasureState.SpecificWidth) ? gridViewColumn.Width : gridViewColumn.DesiredWidth);
					uielement.Arrange(new Rect(num, 0.0, num3, arrangeSize.Height));
					num2 -= num3;
					num += num3;
				}
			}
			return arrangeSize;
		}

		// Token: 0x06006B39 RID: 27449 RVA: 0x002C525C File Offset: 0x002C425C
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.NeedUpdateVisualTree)
			{
				base.InternalChildren.Clear();
				GridViewColumnCollection columns = base.Columns;
				if (columns != null)
				{
					foreach (GridViewColumn column in columns.ColumnCollection)
					{
						base.InternalChildren.AddInternal(this.CreateCell(column));
					}
				}
				base.NeedUpdateVisualTree = false;
			}
			this._viewPortValid = false;
		}

		// Token: 0x06006B3A RID: 27450 RVA: 0x002C52EC File Offset: 0x002C42EC
		internal override void OnColumnPropertyChanged(GridViewColumn column, string propertyName)
		{
			if ("ActualWidth".Equals(propertyName))
			{
				return;
			}
			int actualIndex;
			if ((actualIndex = column.ActualIndex) >= 0 && actualIndex < base.InternalChildren.Count)
			{
				if (GridViewColumn.WidthProperty.Name.Equals(propertyName))
				{
					base.InvalidateMeasure();
					return;
				}
				if ("DisplayMemberBinding".Equals(propertyName))
				{
					FrameworkElement frameworkElement = base.InternalChildren[actualIndex] as FrameworkElement;
					if (frameworkElement != null)
					{
						BindingBase displayMemberBinding = column.DisplayMemberBinding;
						if (displayMemberBinding != null && frameworkElement is TextBlock)
						{
							frameworkElement.SetBinding(TextBlock.TextProperty, displayMemberBinding);
							return;
						}
						this.RenewCell(actualIndex, column);
						return;
					}
				}
				else
				{
					ContentPresenter contentPresenter = base.InternalChildren[actualIndex] as ContentPresenter;
					if (contentPresenter != null)
					{
						if (GridViewColumn.CellTemplateProperty.Name.Equals(propertyName))
						{
							DataTemplate cellTemplate;
							if ((cellTemplate = column.CellTemplate) == null)
							{
								contentPresenter.ClearValue(ContentControl.ContentTemplateProperty);
								return;
							}
							contentPresenter.ContentTemplate = cellTemplate;
							return;
						}
						else if (GridViewColumn.CellTemplateSelectorProperty.Name.Equals(propertyName))
						{
							DataTemplateSelector cellTemplateSelector;
							if ((cellTemplateSelector = column.CellTemplateSelector) == null)
							{
								contentPresenter.ClearValue(ContentControl.ContentTemplateSelectorProperty);
								return;
							}
							contentPresenter.ContentTemplateSelector = cellTemplateSelector;
						}
					}
				}
			}
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x002C5408 File Offset: 0x002C4408
		internal override void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
		{
			base.OnColumnCollectionChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Move)
			{
				base.InvalidateArrange();
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				base.InternalChildren.AddInternal(this.CreateCell((GridViewColumn)e.NewItems[0]));
				break;
			case NotifyCollectionChangedAction.Remove:
				base.InternalChildren.RemoveAt(e.ActualIndex);
				break;
			case NotifyCollectionChangedAction.Replace:
				base.InternalChildren.RemoveAt(e.ActualIndex);
				base.InternalChildren.AddInternal(this.CreateCell((GridViewColumn)e.NewItems[0]));
				break;
			case NotifyCollectionChangedAction.Reset:
				base.InternalChildren.Clear();
				break;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x170018C2 RID: 6338
		// (get) Token: 0x06006B3C RID: 27452 RVA: 0x002C54D0 File Offset: 0x002C44D0
		internal List<UIElement> ActualCells
		{
			get
			{
				List<UIElement> list = new List<UIElement>();
				GridViewColumnCollection columns = base.Columns;
				if (columns != null)
				{
					UIElementCollection internalChildren = base.InternalChildren;
					List<int> indexList = columns.IndexList;
					if (internalChildren.Count == columns.Count)
					{
						int i = 0;
						int count = columns.Count;
						while (i < count)
						{
							UIElement uielement = internalChildren[indexList[i]];
							if (uielement != null)
							{
								list.Add(uielement);
							}
							i++;
						}
					}
				}
				return list;
			}
		}

		// Token: 0x06006B3D RID: 27453 RVA: 0x002C5540 File Offset: 0x002C4540
		private void FindViewPort()
		{
			this._viewItem = (base.TemplatedParent as FrameworkElement);
			if (this._viewItem != null)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this._viewItem);
				if (itemsControl != null)
				{
					ScrollViewer scrollHost = itemsControl.ScrollHost;
					if (scrollHost != null && itemsControl.ItemsHost is VirtualizingPanel && scrollHost.CanContentScroll)
					{
						this._viewPort = (scrollHost.GetTemplateChild("PART_ScrollContentPresenter") as FrameworkElement);
						if (this._viewPort == null)
						{
							this._viewPort = scrollHost;
						}
					}
				}
			}
		}

		// Token: 0x06006B3E RID: 27454 RVA: 0x002C55BC File Offset: 0x002C45BC
		private bool CheckVisibleOnCurrentPage()
		{
			if (!this._viewPortValid)
			{
				this.FindViewPort();
			}
			bool result = true;
			if (this._viewItem != null && this._viewPort != null)
			{
				Rect container = new Rect(default(Point), this._viewPort.RenderSize);
				Rect rect = new Rect(default(Point), this._viewItem.RenderSize);
				rect = this._viewItem.TransformToAncestor(this._viewPort).TransformBounds(rect);
				result = this.CheckContains(container, rect);
			}
			return result;
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x002C5644 File Offset: 0x002C4644
		private bool CheckContains(Rect container, Rect element)
		{
			return (this.CheckIsPointBetween(container, element.Top) && this.CheckIsPointBetween(container, element.Bottom)) || this.CheckIsPointBetween(element, container.Top + 2.0) || this.CheckIsPointBetween(element, container.Bottom - 2.0);
		}

		// Token: 0x06006B40 RID: 27456 RVA: 0x002C56A5 File Offset: 0x002C46A5
		private bool CheckIsPointBetween(Rect rect, double pointY)
		{
			return DoubleUtil.LessThanOrClose(rect.Top, pointY) && DoubleUtil.LessThanOrClose(pointY, rect.Bottom);
		}

		// Token: 0x06006B41 RID: 27457 RVA: 0x002C56C8 File Offset: 0x002C46C8
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			bool flag = false;
			GridViewColumnCollection columns = base.Columns;
			if (columns != null)
			{
				foreach (GridViewColumn gridViewColumn in columns)
				{
					if (gridViewColumn.State != ColumnMeasureState.SpecificWidth)
					{
						gridViewColumn.State = ColumnMeasureState.Data;
						if (base.DesiredWidthList == null || gridViewColumn.ActualIndex >= base.DesiredWidthList.Count)
						{
							flag = true;
							break;
						}
						if (!DoubleUtil.AreClose(gridViewColumn.DesiredWidth, base.DesiredWidthList[gridViewColumn.ActualIndex]))
						{
							base.DesiredWidthList[gridViewColumn.ActualIndex] = gridViewColumn.DesiredWidth;
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				base.InvalidateMeasure();
			}
			base.LayoutUpdated -= this.OnLayoutUpdated;
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x002C579C File Offset: 0x002C479C
		private FrameworkElement CreateCell(GridViewColumn column)
		{
			BindingBase displayMemberBinding;
			FrameworkElement frameworkElement;
			if ((displayMemberBinding = column.DisplayMemberBinding) != null)
			{
				frameworkElement = new TextBlock();
				frameworkElement.DataContext = this.Content;
				frameworkElement.SetBinding(TextBlock.TextProperty, displayMemberBinding);
			}
			else
			{
				ContentPresenter contentPresenter = new ContentPresenter();
				contentPresenter.Content = this.Content;
				DataTemplate cellTemplate;
				if ((cellTemplate = column.CellTemplate) != null)
				{
					contentPresenter.ContentTemplate = cellTemplate;
				}
				DataTemplateSelector cellTemplateSelector;
				if ((cellTemplateSelector = column.CellTemplateSelector) != null)
				{
					contentPresenter.ContentTemplateSelector = cellTemplateSelector;
				}
				frameworkElement = contentPresenter;
			}
			ContentControl contentControl;
			if ((contentControl = (base.TemplatedParent as ContentControl)) != null)
			{
				frameworkElement.VerticalAlignment = contentControl.VerticalContentAlignment;
				frameworkElement.HorizontalAlignment = contentControl.HorizontalContentAlignment;
			}
			frameworkElement.Margin = GridViewRowPresenter._defalutCellMargin;
			return frameworkElement;
		}

		// Token: 0x06006B43 RID: 27459 RVA: 0x002C5841 File Offset: 0x002C4841
		private void RenewCell(int index, GridViewColumn column)
		{
			base.InternalChildren.RemoveAt(index);
			base.InternalChildren.Insert(index, this.CreateCell(column));
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x002C5864 File Offset: 0x002C4864
		private void UpdateCells()
		{
			UIElementCollection internalChildren = base.InternalChildren;
			ContentControl contentControl = base.TemplatedParent as ContentControl;
			for (int i = 0; i < internalChildren.Count; i++)
			{
				FrameworkElement frameworkElement = (FrameworkElement)internalChildren[i];
				ContentPresenter contentPresenter;
				if ((contentPresenter = (frameworkElement as ContentPresenter)) != null)
				{
					contentPresenter.Content = this.Content;
				}
				else
				{
					frameworkElement.DataContext = this.Content;
				}
				if (contentControl != null)
				{
					frameworkElement.VerticalAlignment = contentControl.VerticalContentAlignment;
					frameworkElement.HorizontalAlignment = contentControl.HorizontalContentAlignment;
				}
			}
		}

		// Token: 0x170018C3 RID: 6339
		// (get) Token: 0x06006B45 RID: 27461 RVA: 0x002C58E6 File Offset: 0x002C48E6
		private bool IsOnCurrentPage
		{
			get
			{
				if (!this._isOnCurrentPageValid)
				{
					this._isOnCurrentPage = (base.IsVisible && this.CheckVisibleOnCurrentPage());
					this._isOnCurrentPageValid = true;
				}
				return this._isOnCurrentPage;
			}
		}

		// Token: 0x04003596 RID: 13718
		public static readonly DependencyProperty ContentProperty = ContentControl.ContentProperty.AddOwner(typeof(GridViewRowPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(GridViewRowPresenter.OnContentChanged)));

		// Token: 0x04003597 RID: 13719
		private FrameworkElement _viewPort;

		// Token: 0x04003598 RID: 13720
		private FrameworkElement _viewItem;

		// Token: 0x04003599 RID: 13721
		private Type _oldContentType;

		// Token: 0x0400359A RID: 13722
		private bool _viewPortValid;

		// Token: 0x0400359B RID: 13723
		private bool _isOnCurrentPage;

		// Token: 0x0400359C RID: 13724
		private bool _isOnCurrentPageValid;

		// Token: 0x0400359D RID: 13725
		private static readonly Thickness _defalutCellMargin = new Thickness(6.0, 0.0, 6.0, 0.0);
	}
}
