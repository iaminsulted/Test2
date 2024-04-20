using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000862 RID: 2146
	public class UniformGrid : Panel
	{
		// Token: 0x17001D3E RID: 7486
		// (get) Token: 0x06007EAE RID: 32430 RVA: 0x00319C79 File Offset: 0x00318C79
		// (set) Token: 0x06007EAF RID: 32431 RVA: 0x00319C8B File Offset: 0x00318C8B
		public int FirstColumn
		{
			get
			{
				return (int)base.GetValue(UniformGrid.FirstColumnProperty);
			}
			set
			{
				base.SetValue(UniformGrid.FirstColumnProperty, value);
			}
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool ValidateFirstColumn(object o)
		{
			return (int)o >= 0;
		}

		// Token: 0x17001D3F RID: 7487
		// (get) Token: 0x06007EB1 RID: 32433 RVA: 0x00319C9E File Offset: 0x00318C9E
		// (set) Token: 0x06007EB2 RID: 32434 RVA: 0x00319CB0 File Offset: 0x00318CB0
		public int Columns
		{
			get
			{
				return (int)base.GetValue(UniformGrid.ColumnsProperty);
			}
			set
			{
				base.SetValue(UniformGrid.ColumnsProperty, value);
			}
		}

		// Token: 0x06007EB3 RID: 32435 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool ValidateColumns(object o)
		{
			return (int)o >= 0;
		}

		// Token: 0x17001D40 RID: 7488
		// (get) Token: 0x06007EB4 RID: 32436 RVA: 0x00319CC3 File Offset: 0x00318CC3
		// (set) Token: 0x06007EB5 RID: 32437 RVA: 0x00319CD5 File Offset: 0x00318CD5
		public int Rows
		{
			get
			{
				return (int)base.GetValue(UniformGrid.RowsProperty);
			}
			set
			{
				base.SetValue(UniformGrid.RowsProperty, value);
			}
		}

		// Token: 0x06007EB6 RID: 32438 RVA: 0x002A6F55 File Offset: 0x002A5F55
		private static bool ValidateRows(object o)
		{
			return (int)o >= 0;
		}

		// Token: 0x06007EB7 RID: 32439 RVA: 0x00319CE8 File Offset: 0x00318CE8
		protected override Size MeasureOverride(Size constraint)
		{
			this.UpdateComputedValues();
			Size availableSize = new Size(constraint.Width / (double)this._columns, constraint.Height / (double)this._rows);
			double num = 0.0;
			double num2 = 0.0;
			int i = 0;
			int count = base.InternalChildren.Count;
			while (i < count)
			{
				UIElement uielement = base.InternalChildren[i];
				uielement.Measure(availableSize);
				Size desiredSize = uielement.DesiredSize;
				if (num < desiredSize.Width)
				{
					num = desiredSize.Width;
				}
				if (num2 < desiredSize.Height)
				{
					num2 = desiredSize.Height;
				}
				i++;
			}
			return new Size(num * (double)this._columns, num2 * (double)this._rows);
		}

		// Token: 0x06007EB8 RID: 32440 RVA: 0x00319DA4 File Offset: 0x00318DA4
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Rect finalRect = new Rect(0.0, 0.0, arrangeSize.Width / (double)this._columns, arrangeSize.Height / (double)this._rows);
			double width = finalRect.Width;
			double num = arrangeSize.Width - 1.0;
			finalRect.X += finalRect.Width * (double)this.FirstColumn;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				uielement.Arrange(finalRect);
				if (uielement.Visibility != Visibility.Collapsed)
				{
					finalRect.X += width;
					if (finalRect.X >= num)
					{
						finalRect.Y += finalRect.Height;
						finalRect.X = 0.0;
					}
				}
			}
			return arrangeSize;
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x00319EB4 File Offset: 0x00318EB4
		private void UpdateComputedValues()
		{
			this._columns = this.Columns;
			this._rows = this.Rows;
			if (this.FirstColumn >= this._columns)
			{
				this.FirstColumn = 0;
			}
			if (this._rows == 0 || this._columns == 0)
			{
				int num = 0;
				int i = 0;
				int count = base.InternalChildren.Count;
				while (i < count)
				{
					if (base.InternalChildren[i].Visibility != Visibility.Collapsed)
					{
						num++;
					}
					i++;
				}
				if (num == 0)
				{
					num = 1;
				}
				if (this._rows == 0)
				{
					if (this._columns > 0)
					{
						this._rows = (num + this.FirstColumn + (this._columns - 1)) / this._columns;
						return;
					}
					this._rows = (int)Math.Sqrt((double)num);
					if (this._rows * this._rows < num)
					{
						this._rows++;
					}
					this._columns = this._rows;
					return;
				}
				else if (this._columns == 0)
				{
					this._columns = (num + (this._rows - 1)) / this._rows;
				}
			}
		}

		// Token: 0x04003B4C RID: 15180
		public static readonly DependencyProperty FirstColumnProperty = DependencyProperty.Register("FirstColumn", typeof(int), typeof(UniformGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformGrid.ValidateFirstColumn));

		// Token: 0x04003B4D RID: 15181
		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(UniformGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformGrid.ValidateColumns));

		// Token: 0x04003B4E RID: 15182
		public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(UniformGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(UniformGrid.ValidateRows));

		// Token: 0x04003B4F RID: 15183
		private int _rows;

		// Token: 0x04003B50 RID: 15184
		private int _columns;
	}
}
