using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200074F RID: 1871
	[TemplatePart(Name = "PART_VisualBrushCanvas", Type = typeof(Canvas))]
	internal class DataGridColumnFloatingHeader : Control
	{
		// Token: 0x060065DB RID: 26075 RVA: 0x002AFED0 File Offset: 0x002AEED0
		static DataGridColumnFloatingHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(DataGridColumnHeader.ColumnFloatingHeaderStyleKey));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnFloatingHeader.OnWidthChanged), new CoerceValueCallback(DataGridColumnFloatingHeader.OnCoerceWidth)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnFloatingHeader.OnHeightChanged), new CoerceValueCallback(DataGridColumnFloatingHeader.OnCoerceHeight)));
		}

		// Token: 0x060065DC RID: 26076 RVA: 0x002AFF60 File Offset: 0x002AEF60
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			double num = (double)e.NewValue;
			if (dataGridColumnFloatingHeader._visualBrushCanvas != null && !DoubleUtil.IsNaN(num))
			{
				VisualBrush visualBrush = dataGridColumnFloatingHeader._visualBrushCanvas.Background as VisualBrush;
				if (visualBrush != null)
				{
					Rect viewbox = visualBrush.Viewbox;
					visualBrush.Viewbox = new Rect(viewbox.X, viewbox.Y, num - dataGridColumnFloatingHeader.GetVisualCanvasMarginX(), viewbox.Height);
				}
			}
		}

		// Token: 0x060065DD RID: 26077 RVA: 0x002AFFD4 File Offset: 0x002AEFD4
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			if (dataGridColumnFloatingHeader._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnFloatingHeader._referenceHeader.ActualWidth + dataGridColumnFloatingHeader.GetVisualCanvasMarginX();
			}
			return baseValue;
		}

		// Token: 0x060065DE RID: 26078 RVA: 0x002B0018 File Offset: 0x002AF018
		private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			double num = (double)e.NewValue;
			if (dataGridColumnFloatingHeader._visualBrushCanvas != null && !DoubleUtil.IsNaN(num))
			{
				VisualBrush visualBrush = dataGridColumnFloatingHeader._visualBrushCanvas.Background as VisualBrush;
				if (visualBrush != null)
				{
					Rect viewbox = visualBrush.Viewbox;
					visualBrush.Viewbox = new Rect(viewbox.X, viewbox.Y, viewbox.Width, num - dataGridColumnFloatingHeader.GetVisualCanvasMarginY());
				}
			}
		}

		// Token: 0x060065DF RID: 26079 RVA: 0x002B008C File Offset: 0x002AF08C
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			if (dataGridColumnFloatingHeader._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnFloatingHeader._referenceHeader.ActualHeight + dataGridColumnFloatingHeader.GetVisualCanvasMarginY();
			}
			return baseValue;
		}

		// Token: 0x060065E0 RID: 26080 RVA: 0x002B00D0 File Offset: 0x002AF0D0
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._visualBrushCanvas = (base.GetTemplateChild("PART_VisualBrushCanvas") as Canvas);
			this.UpdateVisualBrush();
		}

		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x060065E1 RID: 26081 RVA: 0x002B00F4 File Offset: 0x002AF0F4
		// (set) Token: 0x060065E2 RID: 26082 RVA: 0x002B00FC File Offset: 0x002AF0FC
		internal DataGridColumnHeader ReferenceHeader
		{
			get
			{
				return this._referenceHeader;
			}
			set
			{
				this._referenceHeader = value;
			}
		}

		// Token: 0x060065E3 RID: 26083 RVA: 0x002B0108 File Offset: 0x002AF108
		private void UpdateVisualBrush()
		{
			if (this._referenceHeader != null && this._visualBrushCanvas != null)
			{
				VisualBrush visualBrush = new VisualBrush(this._referenceHeader);
				visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
				double num = base.Width;
				if (DoubleUtil.IsNaN(num))
				{
					num = this._referenceHeader.ActualWidth;
				}
				else
				{
					num -= this.GetVisualCanvasMarginX();
				}
				double num2 = base.Height;
				if (DoubleUtil.IsNaN(num2))
				{
					num2 = this._referenceHeader.ActualHeight;
				}
				else
				{
					num2 -= this.GetVisualCanvasMarginY();
				}
				Vector offset = VisualTreeHelper.GetOffset(this._referenceHeader);
				visualBrush.Viewbox = new Rect(offset.X, offset.Y, num, num2);
				this._visualBrushCanvas.Background = visualBrush;
			}
		}

		// Token: 0x060065E4 RID: 26084 RVA: 0x002B01BD File Offset: 0x002AF1BD
		internal void ClearHeader()
		{
			this._referenceHeader = null;
			if (this._visualBrushCanvas != null)
			{
				this._visualBrushCanvas.Background = null;
			}
		}

		// Token: 0x060065E5 RID: 26085 RVA: 0x002B01DC File Offset: 0x002AF1DC
		private double GetVisualCanvasMarginX()
		{
			double num = 0.0;
			if (this._visualBrushCanvas != null)
			{
				Thickness margin = this._visualBrushCanvas.Margin;
				num += margin.Left;
				num += margin.Right;
			}
			return num;
		}

		// Token: 0x060065E6 RID: 26086 RVA: 0x002B021C File Offset: 0x002AF21C
		private double GetVisualCanvasMarginY()
		{
			double num = 0.0;
			if (this._visualBrushCanvas != null)
			{
				Thickness margin = this._visualBrushCanvas.Margin;
				num += margin.Top;
				num += margin.Bottom;
			}
			return num;
		}

		// Token: 0x04003399 RID: 13209
		private DataGridColumnHeader _referenceHeader;

		// Token: 0x0400339A RID: 13210
		private const string VisualBrushCanvasTemplateName = "PART_VisualBrushCanvas";

		// Token: 0x0400339B RID: 13211
		private Canvas _visualBrushCanvas;
	}
}
