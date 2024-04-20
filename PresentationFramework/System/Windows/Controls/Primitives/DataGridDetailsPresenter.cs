using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200082C RID: 2092
	public class DataGridDetailsPresenter : ContentPresenter
	{
		// Token: 0x06007A8A RID: 31370 RVA: 0x00308900 File Offset: 0x00307900
		static DataGridDetailsPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(typeof(DataGridDetailsPresenter)));
			ContentPresenter.ContentTemplateProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridDetailsPresenter.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridDetailsPresenter.OnCoerceContentTemplate)));
			ContentPresenter.ContentTemplateSelectorProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridDetailsPresenter.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridDetailsPresenter.OnCoerceContentTemplateSelector)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridDetailsPresenter), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
			EventManager.RegisterClassHandler(typeof(DataGridDetailsPresenter), UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(DataGridDetailsPresenter.OnAnyMouseLeftButtonDownThunk), true);
		}

		// Token: 0x06007A8C RID: 31372 RVA: 0x003089DA File Offset: 0x003079DA
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridDetailsPresenterAutomationPeer(this);
		}

		// Token: 0x06007A8D RID: 31373 RVA: 0x003089E4 File Offset: 0x003079E4
		private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
		{
			DataGridDetailsPresenter dataGridDetailsPresenter = d as DataGridDetailsPresenter;
			DataGridRow dataGridRowOwner = dataGridDetailsPresenter.DataGridRowOwner;
			DataGrid grandParentObject = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridDetailsPresenter, baseValue, ContentPresenter.ContentTemplateProperty, dataGridRowOwner, DataGridRow.DetailsTemplateProperty, grandParentObject, DataGrid.RowDetailsTemplateProperty);
		}

		// Token: 0x06007A8E RID: 31374 RVA: 0x00308A24 File Offset: 0x00307A24
		private static object OnCoerceContentTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridDetailsPresenter dataGridDetailsPresenter = d as DataGridDetailsPresenter;
			DataGridRow dataGridRowOwner = dataGridDetailsPresenter.DataGridRowOwner;
			DataGrid grandParentObject = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridDetailsPresenter, baseValue, ContentPresenter.ContentTemplateSelectorProperty, dataGridRowOwner, DataGridRow.DetailsTemplateSelectorProperty, grandParentObject, DataGrid.RowDetailsTemplateSelectorProperty);
		}

		// Token: 0x06007A8F RID: 31375 RVA: 0x00308A64 File Offset: 0x00307A64
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner != null)
			{
				dataGridRowOwner.DetailsPresenter = this;
				this.SyncProperties();
			}
		}

		// Token: 0x06007A90 RID: 31376 RVA: 0x00308A8F File Offset: 0x00307A8F
		private static void OnAnyMouseLeftButtonDownThunk(object sender, MouseButtonEventArgs e)
		{
			((DataGridDetailsPresenter)sender).OnAnyMouseLeftButtonDown(e);
		}

		// Token: 0x06007A91 RID: 31377 RVA: 0x00308AA0 File Offset: 0x00307AA0
		private void OnAnyMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!PresentationSource.UnderSamePresentationSource(new DependencyObject[]
			{
				e.OriginalSource as DependencyObject,
				this
			}))
			{
				return;
			}
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			DataGrid dataGrid = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
			if (dataGrid != null && dataGridRowOwner != null)
			{
				if (dataGrid.CurrentCell.Item != dataGridRowOwner.Item)
				{
					dataGrid.ScrollIntoView(dataGridRowOwner.Item, dataGrid.ColumnFromDisplayIndex(0));
				}
				dataGrid.HandleSelectionForRowHeaderAndDetailsInput(dataGridRowOwner, Mouse.Captured == null);
			}
		}

		// Token: 0x17001C5B RID: 7259
		// (get) Token: 0x06007A92 RID: 31378 RVA: 0x00308B1F File Offset: 0x00307B1F
		internal FrameworkElement DetailsElement
		{
			get
			{
				if (VisualTreeHelper.GetChildrenCount(this) > 0)
				{
					return VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
				}
				return null;
			}
		}

		// Token: 0x06007A93 RID: 31379 RVA: 0x00308B38 File Offset: 0x00307B38
		internal void SyncProperties()
		{
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			base.Content = ((dataGridRowOwner != null) ? dataGridRowOwner.Item : null);
			DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateProperty);
			DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateSelectorProperty);
		}

		// Token: 0x06007A94 RID: 31380 RVA: 0x00308B74 File Offset: 0x00307B74
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridDetailsPresenter)d).NotifyPropertyChanged(d, e);
		}

		// Token: 0x06007A95 RID: 31381 RVA: 0x00308B84 File Offset: 0x00307B84
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.Property == DataGrid.RowDetailsTemplateProperty || e.Property == DataGridRow.DetailsTemplateProperty || e.Property == ContentPresenter.ContentTemplateProperty)
			{
				DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateProperty);
				return;
			}
			if (e.Property == DataGrid.RowDetailsTemplateSelectorProperty || e.Property == DataGridRow.DetailsTemplateSelectorProperty || e.Property == ContentPresenter.ContentTemplateSelectorProperty)
			{
				DataGridHelper.TransferProperty(this, ContentPresenter.ContentTemplateSelectorProperty);
			}
		}

		// Token: 0x06007A96 RID: 31382 RVA: 0x00308BFC File Offset: 0x00307BFC
		protected override Size MeasureOverride(Size availableSize)
		{
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return base.MeasureOverride(availableSize);
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return base.MeasureOverride(availableSize);
			}
			if (dataGridRowOwner.DetailsPresenterDrawsGridLines && DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Size result = base.MeasureOverride(DataGridHelper.SubtractFromSize(availableSize, horizontalGridLineThickness, true));
				result.Height += horizontalGridLineThickness;
				return result;
			}
			return base.MeasureOverride(availableSize);
		}

		// Token: 0x06007A97 RID: 31383 RVA: 0x00308C6C File Offset: 0x00307C6C
		protected override Size ArrangeOverride(Size finalSize)
		{
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return base.ArrangeOverride(finalSize);
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return base.ArrangeOverride(finalSize);
			}
			if (dataGridRowOwner.DetailsPresenterDrawsGridLines && DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Size result = base.ArrangeOverride(DataGridHelper.SubtractFromSize(finalSize, horizontalGridLineThickness, true));
				result.Height += horizontalGridLineThickness;
				return result;
			}
			return base.ArrangeOverride(finalSize);
		}

		// Token: 0x06007A98 RID: 31384 RVA: 0x00308CDC File Offset: 0x00307CDC
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return;
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			if (dataGridRowOwner.DetailsPresenterDrawsGridLines && DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Rect rectangle = new Rect(new Size(base.RenderSize.Width, horizontalGridLineThickness));
				rectangle.Y = base.RenderSize.Height - horizontalGridLineThickness;
				drawingContext.DrawRectangle(dataGridOwner.HorizontalGridLinesBrush, null, rectangle);
			}
		}

		// Token: 0x17001C5C RID: 7260
		// (get) Token: 0x06007A99 RID: 31385 RVA: 0x00308D60 File Offset: 0x00307D60
		private DataGrid DataGridOwner
		{
			get
			{
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				if (dataGridRowOwner != null)
				{
					return dataGridRowOwner.DataGridOwner;
				}
				return null;
			}
		}

		// Token: 0x17001C5D RID: 7261
		// (get) Token: 0x06007A9A RID: 31386 RVA: 0x00306677 File Offset: 0x00305677
		internal DataGridRow DataGridRowOwner
		{
			get
			{
				return DataGridHelper.FindParent<DataGridRow>(this);
			}
		}
	}
}
