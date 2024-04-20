using System;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000858 RID: 2136
	public class TabPanel : Panel
	{
		// Token: 0x06007D93 RID: 32147 RVA: 0x003146B3 File Offset: 0x003136B3
		static TabPanel()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
		}

		// Token: 0x06007D94 RID: 32148 RVA: 0x003146F4 File Offset: 0x003136F4
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			Dock tabStripPlacement = this.TabStripPlacement;
			this._numRows = 1;
			this._numHeaders = 0;
			this._rowHeight = 0.0;
			if (tabStripPlacement == Dock.Top || tabStripPlacement == Dock.Bottom)
			{
				int num = 0;
				double num2 = 0.0;
				double num3 = 0.0;
				foreach (object obj in base.InternalChildren)
				{
					UIElement uielement = (UIElement)obj;
					if (uielement.Visibility != Visibility.Collapsed)
					{
						this._numHeaders++;
						uielement.Measure(constraint);
						Size desiredSizeWithoutMargin = this.GetDesiredSizeWithoutMargin(uielement);
						if (this._rowHeight < desiredSizeWithoutMargin.Height)
						{
							this._rowHeight = desiredSizeWithoutMargin.Height;
						}
						if (num2 + desiredSizeWithoutMargin.Width > constraint.Width && num > 0)
						{
							if (num3 < num2)
							{
								num3 = num2;
							}
							num2 = desiredSizeWithoutMargin.Width;
							num = 1;
							this._numRows++;
						}
						else
						{
							num2 += desiredSizeWithoutMargin.Width;
							num++;
						}
					}
				}
				if (num3 < num2)
				{
					num3 = num2;
				}
				result.Height = this._rowHeight * (double)this._numRows;
				if (double.IsInfinity(result.Width) || DoubleUtil.IsNaN(result.Width) || num3 < constraint.Width)
				{
					result.Width = num3;
				}
				else
				{
					result.Width = constraint.Width;
				}
			}
			else if (tabStripPlacement == Dock.Left || tabStripPlacement == Dock.Right)
			{
				foreach (object obj2 in base.InternalChildren)
				{
					UIElement uielement2 = (UIElement)obj2;
					if (uielement2.Visibility != Visibility.Collapsed)
					{
						this._numHeaders++;
						uielement2.Measure(constraint);
						Size desiredSizeWithoutMargin2 = this.GetDesiredSizeWithoutMargin(uielement2);
						if (result.Width < desiredSizeWithoutMargin2.Width)
						{
							result.Width = desiredSizeWithoutMargin2.Width;
						}
						result.Height += desiredSizeWithoutMargin2.Height;
					}
				}
			}
			return result;
		}

		// Token: 0x06007D95 RID: 32149 RVA: 0x00314948 File Offset: 0x00313948
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			Dock tabStripPlacement = this.TabStripPlacement;
			if (tabStripPlacement == Dock.Top || tabStripPlacement == Dock.Bottom)
			{
				this.ArrangeHorizontal(arrangeSize);
			}
			else if (tabStripPlacement == Dock.Left || tabStripPlacement == Dock.Right)
			{
				this.ArrangeVertical(arrangeSize);
			}
			return arrangeSize;
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x00109403 File Offset: 0x00108403
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			return null;
		}

		// Token: 0x06007D97 RID: 32151 RVA: 0x0031497C File Offset: 0x0031397C
		private Size GetDesiredSizeWithoutMargin(UIElement element)
		{
			Thickness thickness = (Thickness)element.GetValue(FrameworkElement.MarginProperty);
			return new Size
			{
				Height = Math.Max(0.0, element.DesiredSize.Height - thickness.Top - thickness.Bottom),
				Width = Math.Max(0.0, element.DesiredSize.Width - thickness.Left - thickness.Right)
			};
		}

		// Token: 0x06007D98 RID: 32152 RVA: 0x00314A0C File Offset: 0x00313A0C
		private double[] GetHeadersSize()
		{
			double[] array = new double[this._numHeaders];
			int num = 0;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement.Visibility != Visibility.Collapsed)
				{
					array[num] = this.GetDesiredSizeWithoutMargin(uielement).Width;
					num++;
				}
			}
			return array;
		}

		// Token: 0x06007D99 RID: 32153 RVA: 0x00314A90 File Offset: 0x00313A90
		private void ArrangeHorizontal(Size arrangeSize)
		{
			Dock tabStripPlacement = this.TabStripPlacement;
			bool flag = this._numRows > 1;
			int num = 0;
			int[] array = Array.Empty<int>();
			Vector vector = default(Vector);
			double[] headersSize = this.GetHeadersSize();
			if (flag)
			{
				array = this.CalculateHeaderDistribution(arrangeSize.Width, headersSize);
				num = this.GetActiveRow(array);
				if (tabStripPlacement == Dock.Top)
				{
					vector.Y = (double)(this._numRows - 1 - num) * this._rowHeight;
				}
				if (tabStripPlacement == Dock.Bottom && num != 0)
				{
					vector.Y = (double)(this._numRows - num) * this._rowHeight;
				}
			}
			int num2 = 0;
			int num3 = 0;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement.Visibility != Visibility.Collapsed)
				{
					Thickness thickness = (Thickness)uielement.GetValue(FrameworkElement.MarginProperty);
					double left = thickness.Left;
					double right = thickness.Right;
					double top = thickness.Top;
					double bottom = thickness.Bottom;
					object obj2 = flag && ((num3 < array.Length && array[num3] == num2) || num2 == this._numHeaders - 1);
					Size size = new Size(headersSize[num2], this._rowHeight);
					object obj3 = obj2;
					if (obj3 != null)
					{
						size.Width = arrangeSize.Width - vector.X;
					}
					uielement.Arrange(new Rect(vector.X, vector.Y, size.Width, size.Height));
					Size size2 = size;
					size2.Height = Math.Max(0.0, size2.Height - top - bottom);
					size2.Width = Math.Max(0.0, size2.Width - left - right);
					vector.X += size.Width;
					if (obj3 != null)
					{
						if ((num3 == num && tabStripPlacement == Dock.Top) || (num3 == num - 1 && tabStripPlacement == Dock.Bottom))
						{
							vector.Y = 0.0;
						}
						else
						{
							vector.Y += this._rowHeight;
						}
						vector.X = 0.0;
						num3++;
					}
					num2++;
				}
			}
		}

		// Token: 0x06007D9A RID: 32154 RVA: 0x00314CF4 File Offset: 0x00313CF4
		private void ArrangeVertical(Size arrangeSize)
		{
			double num = 0.0;
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement.Visibility != Visibility.Collapsed)
				{
					Size desiredSizeWithoutMargin = this.GetDesiredSizeWithoutMargin(uielement);
					uielement.Arrange(new Rect(0.0, num, arrangeSize.Width, desiredSizeWithoutMargin.Height));
					num += desiredSizeWithoutMargin.Height;
				}
			}
		}

		// Token: 0x06007D9B RID: 32155 RVA: 0x00314D94 File Offset: 0x00313D94
		private int GetActiveRow(int[] solution)
		{
			int num = 0;
			int num2 = 0;
			if (solution.Length != 0)
			{
				foreach (object obj in base.InternalChildren)
				{
					UIElement uielement = (UIElement)obj;
					if (uielement.Visibility != Visibility.Collapsed)
					{
						if ((bool)uielement.GetValue(Selector.IsSelectedProperty))
						{
							return num;
						}
						if (num < solution.Length && solution[num] == num2)
						{
							num++;
						}
						num2++;
					}
				}
			}
			if (this.TabStripPlacement == Dock.Top)
			{
				num = this._numRows - 1;
			}
			return num;
		}

		// Token: 0x06007D9C RID: 32156 RVA: 0x00314E3C File Offset: 0x00313E3C
		private int[] CalculateHeaderDistribution(double rowWidthLimit, double[] headerWidth)
		{
			double num = 0.0;
			int num2 = headerWidth.Length;
			int num3 = this._numRows - 1;
			double num4 = 0.0;
			int num5 = 0;
			int[] array = new int[num3];
			int[] array2 = new int[num3];
			int[] array3 = new int[this._numRows];
			double[] array4 = new double[this._numRows];
			double[] array5 = new double[this._numRows];
			double[] array6 = new double[this._numRows];
			int num6 = 0;
			double num7;
			for (int i = 0; i < num2; i++)
			{
				if (num4 + headerWidth[i] > rowWidthLimit && num5 > 0)
				{
					array4[num6] = num4;
					array3[num6] = num5;
					num7 = Math.Max(0.0, (rowWidthLimit - num4) / (double)num5);
					array5[num6] = num7;
					array[num6] = i - 1;
					if (num < num7)
					{
						num = num7;
					}
					num6++;
					num4 = headerWidth[i];
					num5 = 1;
				}
				else
				{
					num4 += headerWidth[i];
					if (headerWidth[i] != 0.0)
					{
						num5++;
					}
				}
			}
			if (num6 == 0)
			{
				return Array.Empty<int>();
			}
			array4[num6] = num4;
			array3[num6] = num5;
			num7 = (rowWidthLimit - num4) / (double)num5;
			array5[num6] = num7;
			if (num < num7)
			{
				num = num7;
			}
			array.CopyTo(array2, 0);
			array5.CopyTo(array6, 0);
			for (;;)
			{
				int num8 = 0;
				double num9 = 0.0;
				for (int j = 0; j < this._numRows; j++)
				{
					if (num9 < array5[j])
					{
						num9 = array5[j];
						num8 = j;
					}
				}
				if (num8 == 0)
				{
					break;
				}
				int num10 = num8;
				int num11 = num10 - 1;
				int num12 = array[num11];
				double num13 = headerWidth[num12];
				array4[num10] += num13;
				if (array4[num10] > rowWidthLimit)
				{
					break;
				}
				array[num11]--;
				array3[num10]++;
				array4[num11] -= num13;
				array3[num11]--;
				array5[num11] = (rowWidthLimit - array4[num11]) / (double)array3[num11];
				array5[num10] = (rowWidthLimit - array4[num10]) / (double)array3[num10];
				num9 = 0.0;
				for (int k = 0; k < this._numRows; k++)
				{
					if (num9 < array5[k])
					{
						num9 = array5[k];
					}
				}
				if (num9 < num)
				{
					num = num9;
					array.CopyTo(array2, 0);
					array5.CopyTo(array6, 0);
				}
			}
			num6 = 0;
			for (int l = 0; l < num2; l++)
			{
				headerWidth[l] += array6[num6];
				if (num6 < num3 && array2[num6] == l)
				{
					num6++;
				}
			}
			return array2;
		}

		// Token: 0x17001CF7 RID: 7415
		// (get) Token: 0x06007D9D RID: 32157 RVA: 0x003150E8 File Offset: 0x003140E8
		private Dock TabStripPlacement
		{
			get
			{
				Dock result = Dock.Top;
				TabControl tabControl = base.TemplatedParent as TabControl;
				if (tabControl != null)
				{
					result = tabControl.TabStripPlacement;
				}
				return result;
			}
		}

		// Token: 0x04003AF6 RID: 15094
		private int _numRows = 1;

		// Token: 0x04003AF7 RID: 15095
		private int _numHeaders;

		// Token: 0x04003AF8 RID: 15096
		private double _rowHeight;
	}
}
