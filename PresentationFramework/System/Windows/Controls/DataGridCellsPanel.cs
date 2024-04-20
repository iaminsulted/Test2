using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000746 RID: 1862
	public class DataGridCellsPanel : VirtualizingPanel
	{
		// Token: 0x060064A1 RID: 25761 RVA: 0x002A9010 File Offset: 0x002A8010
		static DataGridCellsPanel()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DataGridCellsPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
		}

		// Token: 0x060064A2 RID: 25762 RVA: 0x002A9031 File Offset: 0x002A8031
		public DataGridCellsPanel()
		{
			this.IsVirtualizing = false;
			this.InRecyclingMode = false;
		}

		// Token: 0x060064A3 RID: 25763 RVA: 0x002A9054 File Offset: 0x002A8054
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			this.DetermineVirtualizationState();
			this.EnsureRealizedChildren();
			IList realizedChildren = this.RealizedChildren;
			if (this.RebuildRealizedColumnsBlockList)
			{
				size = this.DetermineRealizedColumnsBlockList(constraint);
			}
			else
			{
				size = this.GenerateAndMeasureChildrenForRealizedColumns(constraint);
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				this.DisconnectRecycledContainers();
			}
			if (!DoubleUtil.AreClose(base.DesiredSize, size) && base.MeasureDuringArrange)
			{
				this.ParentPresenter.InvalidateMeasure();
				UIElement uielement = VisualTreeHelper.GetParent(this) as UIElement;
				if (uielement != null)
				{
					uielement.InvalidateMeasure();
				}
			}
			return size;
		}

		// Token: 0x060064A4 RID: 25764 RVA: 0x002A90E4 File Offset: 0x002A80E4
		private static void MeasureChild(UIElement child, Size constraint)
		{
			IProvideDataGridColumn provideDataGridColumn = child as IProvideDataGridColumn;
			bool flag = child is DataGridColumnHeader;
			Size availableSize = new Size(double.PositiveInfinity, constraint.Height);
			double num = 0.0;
			bool flag2 = false;
			if (provideDataGridColumn != null)
			{
				DataGridColumn column = provideDataGridColumn.Column;
				DataGridLength width = column.Width;
				if (width.IsAuto || (width.IsSizeToHeader && flag) || (width.IsSizeToCells && !flag))
				{
					child.Measure(availableSize);
					num = child.DesiredSize.Width;
					flag2 = true;
				}
				availableSize.Width = column.GetConstraintWidth(flag);
			}
			if (DoubleUtil.AreClose(num, 0.0))
			{
				child.Measure(availableSize);
			}
			Size desiredSize = child.DesiredSize;
			if (provideDataGridColumn != null)
			{
				DataGridColumn column2 = provideDataGridColumn.Column;
				column2.UpdateDesiredWidthForAutoColumn(flag, DoubleUtil.AreClose(num, 0.0) ? desiredSize.Width : num);
				DataGridLength width2 = column2.Width;
				if (flag2 && !DoubleUtil.IsNaN(width2.DisplayValue) && DoubleUtil.GreaterThan(num, width2.DisplayValue))
				{
					availableSize.Width = width2.DisplayValue;
					child.Measure(availableSize);
				}
			}
		}

		// Token: 0x060064A5 RID: 25765 RVA: 0x002A920C File Offset: 0x002A820C
		private Size GenerateAndMeasureChildrenForRealizedColumns(Size constraint)
		{
			double num = 0.0;
			double num2 = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			IItemContainerGenerator itemContainerGenerator = base.ItemContainerGenerator;
			List<RealizedColumnsBlock> realizedColumnsBlockList = this.RealizedColumnsBlockList;
			this.VirtualizeChildren(realizedColumnsBlockList, itemContainerGenerator);
			if (realizedColumnsBlockList.Count > 0)
			{
				int i = 0;
				int count = realizedColumnsBlockList.Count;
				while (i < count)
				{
					RealizedColumnsBlock realizedColumnsBlock = realizedColumnsBlockList[i];
					Size size = this.GenerateChildren(itemContainerGenerator, realizedColumnsBlock.StartIndex, realizedColumnsBlock.EndIndex, constraint);
					num += size.Width;
					num2 = Math.Max(num2, size.Height);
					if (i != count - 1)
					{
						RealizedColumnsBlock realizedColumnsBlock2 = realizedColumnsBlockList[i + 1];
						num += this.GetColumnEstimatedMeasureWidthSum(realizedColumnsBlock.EndIndex + 1, realizedColumnsBlock2.StartIndex - 1, averageColumnWidth);
					}
					i++;
				}
				num += this.GetColumnEstimatedMeasureWidthSum(0, realizedColumnsBlockList[0].StartIndex - 1, averageColumnWidth);
				num += this.GetColumnEstimatedMeasureWidthSum(realizedColumnsBlockList[realizedColumnsBlockList.Count - 1].EndIndex + 1, parentDataGrid.Columns.Count - 1, averageColumnWidth);
			}
			else
			{
				num = 0.0;
			}
			return new Size(num, num2);
		}

		// Token: 0x060064A6 RID: 25766 RVA: 0x002A9354 File Offset: 0x002A8354
		private Size DetermineRealizedColumnsBlockList(Size constraint)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			Size result = default(Size);
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid == null)
			{
				return result;
			}
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			double cellsPanelHorizontalOffset = parentDataGrid.CellsPanelHorizontalOffset;
			double num = horizontalScrollOffset;
			double num2 = -cellsPanelHorizontalOffset;
			double num3 = horizontalScrollOffset - cellsPanelHorizontalOffset;
			int num4 = -1;
			int lastVisibleNonFrozenDisplayIndex = -1;
			double num5 = this.GetViewportWidth() - cellsPanelHorizontalOffset;
			double num6 = 0.0;
			if (this.IsVirtualizing && DoubleUtil.LessThan(num5, 0.0))
			{
				return result;
			}
			bool hasVisibleStarColumns = parentDataGrid.InternalColumns.HasVisibleStarColumns;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			bool flag = DoubleUtil.AreClose(averageColumnWidth, 0.0);
			bool flag2 = !this.IsVirtualizing;
			bool flag3 = flag || hasVisibleStarColumns || flag2;
			int frozenColumnCount = parentDataGrid.FrozenColumnCount;
			int num7 = -1;
			bool redeterminationNeeded = false;
			IItemContainerGenerator itemContainerGenerator = base.ItemContainerGenerator;
			IDisposable disposable = null;
			int num8 = 0;
			try
			{
				int i = 0;
				int count = parentDataGrid.Columns.Count;
				while (i < count)
				{
					DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
					if (dataGridColumn.IsVisible)
					{
						int num9 = parentDataGrid.ColumnIndexFromDisplayIndex(i);
						if (num9 != num8 || num7 != num9 - 1)
						{
							num8 = num9;
							if (disposable != null)
							{
								disposable.Dispose();
								disposable = null;
							}
						}
						num7 = num9;
						Size size;
						if (flag3)
						{
							if (this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
							{
								break;
							}
						}
						else
						{
							size = new Size(DataGridCellsPanel.GetColumnEstimatedMeasureWidth(dataGridColumn, averageColumnWidth), 0.0);
						}
						if (flag2 || hasVisibleStarColumns || DoubleUtil.LessThan(num6, num5))
						{
							if (i < frozenColumnCount)
							{
								if (!flag3 && this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
								{
									break;
								}
								list.Add(num9);
								list2.Add(i);
								num6 += size.Width;
								num += size.Width;
							}
							else if (DoubleUtil.LessThanOrClose(num2, num3))
							{
								if (DoubleUtil.LessThanOrClose(num2 + size.Width, num3))
								{
									if (flag3)
									{
										if (flag2 || hasVisibleStarColumns)
										{
											list.Add(num9);
											list2.Add(i);
										}
										else if (flag)
										{
											redeterminationNeeded = true;
										}
									}
									else if (disposable != null)
									{
										disposable.Dispose();
										disposable = null;
									}
									num2 += size.Width;
								}
								else
								{
									if (!flag3 && this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
									{
										break;
									}
									double num10 = num3 - num2;
									if (DoubleUtil.AreClose(num10, 0.0))
									{
										num2 = num + size.Width;
										num6 += size.Width;
									}
									else
									{
										double num11 = size.Width - num10;
										num2 = num + num11;
										num6 += num11;
									}
									list.Add(num9);
									list2.Add(i);
									num4 = i;
									lastVisibleNonFrozenDisplayIndex = i;
								}
							}
							else
							{
								if (!flag3 && this.GenerateChild(itemContainerGenerator, constraint, dataGridColumn, ref disposable, ref num8, out size) == null)
								{
									break;
								}
								if (num4 < 0)
								{
									num4 = i;
								}
								lastVisibleNonFrozenDisplayIndex = i;
								num2 += size.Width;
								num6 += size.Width;
								list.Add(num9);
								list2.Add(i);
							}
						}
						result.Width += size.Width;
						result.Height = Math.Max(result.Height, size.Height);
					}
					i++;
				}
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
					disposable = null;
				}
			}
			if (!hasVisibleStarColumns && !flag2)
			{
				if (this.ParentPresenter is DataGridColumnHeadersPresenter)
				{
					Size size2 = this.EnsureAtleastOneHeader(itemContainerGenerator, constraint, list, list2);
					result.Height = Math.Max(result.Height, size2.Height);
					redeterminationNeeded = true;
				}
				else
				{
					this.EnsureFocusTrail(list, list2, num4, lastVisibleNonFrozenDisplayIndex, constraint);
				}
			}
			this.UpdateRealizedBlockLists(list, list2, redeterminationNeeded);
			this.VirtualizeChildren(this.RealizedColumnsBlockList, itemContainerGenerator);
			return result;
		}

		// Token: 0x060064A7 RID: 25767 RVA: 0x002A9738 File Offset: 0x002A8738
		private void UpdateRealizedBlockLists(List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, bool redeterminationNeeded)
		{
			realizedColumnIndices.Sort();
			this.RealizedColumnsBlockList = DataGridCellsPanel.BuildRealizedColumnsBlockList(realizedColumnIndices);
			this.RealizedColumnsDisplayIndexBlockList = DataGridCellsPanel.BuildRealizedColumnsBlockList(realizedColumnDisplayIndices);
			if (!redeterminationNeeded)
			{
				this.RebuildRealizedColumnsBlockList = false;
			}
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x002A9764 File Offset: 0x002A8764
		private static List<RealizedColumnsBlock> BuildRealizedColumnsBlockList(List<int> indexList)
		{
			List<RealizedColumnsBlock> list = new List<RealizedColumnsBlock>();
			if (indexList.Count == 1)
			{
				list.Add(new RealizedColumnsBlock(indexList[0], indexList[0], 0));
			}
			else if (indexList.Count > 0)
			{
				int startIndex = indexList[0];
				int i = 1;
				int count = indexList.Count;
				while (i < count)
				{
					if (indexList[i] != indexList[i - 1] + 1)
					{
						if (list.Count == 0)
						{
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i - 1], 0));
						}
						else
						{
							RealizedColumnsBlock realizedColumnsBlock = list[list.Count - 1];
							int startIndexOffset = realizedColumnsBlock.StartIndexOffset + realizedColumnsBlock.EndIndex - realizedColumnsBlock.StartIndex + 1;
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i - 1], startIndexOffset));
						}
						startIndex = indexList[i];
					}
					if (i == count - 1)
					{
						if (list.Count == 0)
						{
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i], 0));
						}
						else
						{
							RealizedColumnsBlock realizedColumnsBlock2 = list[list.Count - 1];
							int startIndexOffset2 = realizedColumnsBlock2.StartIndexOffset + realizedColumnsBlock2.EndIndex - realizedColumnsBlock2.StartIndex + 1;
							list.Add(new RealizedColumnsBlock(startIndex, indexList[i], startIndexOffset2));
						}
					}
					i++;
				}
			}
			return list;
		}

		// Token: 0x060064A9 RID: 25769 RVA: 0x002A98B0 File Offset: 0x002A88B0
		private static GeneratorPosition IndexToGeneratorPositionForStart(IItemContainerGenerator generator, int index, out int childIndex)
		{
			GeneratorPosition result = (generator != null) ? generator.GeneratorPositionFromIndex(index) : new GeneratorPosition(-1, index + 1);
			childIndex = ((result.Offset == 0) ? result.Index : (result.Index + 1));
			return result;
		}

		// Token: 0x060064AA RID: 25770 RVA: 0x002A98F1 File Offset: 0x002A88F1
		private UIElement GenerateChild(IItemContainerGenerator generator, Size constraint, DataGridColumn column, ref IDisposable generatorState, ref int childIndex, out Size childSize)
		{
			if (generatorState == null)
			{
				generatorState = generator.StartAt(DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, childIndex, out childIndex), GeneratorDirection.Forward, true);
			}
			return this.GenerateChild(generator, constraint, column, ref childIndex, out childSize);
		}

		// Token: 0x060064AB RID: 25771 RVA: 0x002A991C File Offset: 0x002A891C
		private UIElement GenerateChild(IItemContainerGenerator generator, Size constraint, DataGridColumn column, ref int childIndex, out Size childSize)
		{
			bool newlyRealized;
			UIElement uielement = generator.GenerateNext(out newlyRealized) as UIElement;
			if (uielement == null)
			{
				childSize = default(Size);
				return null;
			}
			this.AddContainerFromGenerator(childIndex, uielement, newlyRealized);
			childIndex++;
			DataGridCellsPanel.MeasureChild(uielement, constraint);
			DataGridLength width = column.Width;
			childSize = uielement.DesiredSize;
			if (!DoubleUtil.IsNaN(width.DisplayValue))
			{
				childSize = new Size(width.DisplayValue, childSize.Height);
			}
			return uielement;
		}

		// Token: 0x060064AC RID: 25772 RVA: 0x002A999C File Offset: 0x002A899C
		private Size GenerateChildren(IItemContainerGenerator generator, int startIndex, int endIndex, Size constraint)
		{
			double num = 0.0;
			double num2 = 0.0;
			int num3;
			GeneratorPosition position = DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, startIndex, out num3);
			DataGrid parentDataGrid = this.ParentDataGrid;
			using (generator.StartAt(position, GeneratorDirection.Forward, true))
			{
				for (int i = startIndex; i <= endIndex; i++)
				{
					if (parentDataGrid.Columns[i].IsVisible)
					{
						Size size;
						if (this.GenerateChild(generator, constraint, parentDataGrid.Columns[i], ref num3, out size) == null)
						{
							return new Size(num, num2);
						}
						num += size.Width;
						num2 = Math.Max(num2, size.Height);
					}
				}
			}
			return new Size(num, num2);
		}

		// Token: 0x060064AD RID: 25773 RVA: 0x002A9A68 File Offset: 0x002A8A68
		private void AddContainerFromGenerator(int childIndex, UIElement child, bool newlyRealized)
		{
			if (!newlyRealized)
			{
				if (this.InRecyclingMode)
				{
					IList realizedChildren = this.RealizedChildren;
					if (childIndex >= realizedChildren.Count || realizedChildren[childIndex] != child)
					{
						this.InsertRecycledContainer(childIndex, child);
						child.Measure(default(Size));
						return;
					}
				}
			}
			else
			{
				this.InsertNewContainer(childIndex, child);
			}
		}

		// Token: 0x060064AE RID: 25774 RVA: 0x002A9ABA File Offset: 0x002A8ABA
		private void InsertRecycledContainer(int childIndex, UIElement container)
		{
			this.InsertContainer(childIndex, container, true);
		}

		// Token: 0x060064AF RID: 25775 RVA: 0x002A9AC5 File Offset: 0x002A8AC5
		private void InsertNewContainer(int childIndex, UIElement container)
		{
			this.InsertContainer(childIndex, container, false);
		}

		// Token: 0x060064B0 RID: 25776 RVA: 0x002A9AD0 File Offset: 0x002A8AD0
		private void InsertContainer(int childIndex, UIElement container, bool isRecycled)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			int num = 0;
			if (childIndex > 0)
			{
				num = this.ChildIndexFromRealizedIndex(childIndex - 1);
				num++;
			}
			if (!isRecycled || num >= internalChildren.Count || internalChildren[num] != container)
			{
				if (num < internalChildren.Count)
				{
					int num2 = num;
					if (isRecycled && VisualTreeHelper.GetParent(container) != null)
					{
						int num3 = internalChildren.IndexOf(container);
						base.RemoveInternalChildRange(num3, 1);
						if (num3 < num2)
						{
							num2--;
						}
						base.InsertInternalChild(num2, container);
					}
					else
					{
						base.InsertInternalChild(num2, container);
					}
				}
				else if (isRecycled && VisualTreeHelper.GetParent(container) != null)
				{
					int index = internalChildren.IndexOf(container);
					base.RemoveInternalChildRange(index, 1);
					base.AddInternalChild(container);
				}
				else
				{
					base.AddInternalChild(container);
				}
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				this._realizedChildren.Insert(childIndex, container);
			}
			base.ItemContainerGenerator.PrepareItemContainer(container);
		}

		// Token: 0x060064B1 RID: 25777 RVA: 0x002A9BA8 File Offset: 0x002A8BA8
		private int ChildIndexFromRealizedIndex(int realizedChildIndex)
		{
			if (this.IsVirtualizing && this.InRecyclingMode && realizedChildIndex < this._realizedChildren.Count)
			{
				UIElement uielement = this._realizedChildren[realizedChildIndex];
				UIElementCollection internalChildren = base.InternalChildren;
				for (int i = realizedChildIndex; i < internalChildren.Count; i++)
				{
					if (internalChildren[i] == uielement)
					{
						return i;
					}
				}
			}
			return realizedChildIndex;
		}

		// Token: 0x060064B2 RID: 25778 RVA: 0x002A9C08 File Offset: 0x002A8C08
		private static bool InBlockOrNextBlock(List<RealizedColumnsBlock> blockList, int index, ref int blockIndex, ref RealizedColumnsBlock block, out bool pastLastBlock)
		{
			pastLastBlock = false;
			bool result = true;
			if (index < block.StartIndex)
			{
				result = false;
			}
			else if (index > block.EndIndex)
			{
				if (blockIndex == blockList.Count - 1)
				{
					blockIndex++;
					pastLastBlock = true;
					result = false;
				}
				else
				{
					int num = blockIndex + 1;
					blockIndex = num;
					block = blockList[num];
					if (index < block.StartIndex || index > block.EndIndex)
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060064B3 RID: 25779 RVA: 0x002A9C78 File Offset: 0x002A8C78
		private Size EnsureAtleastOneHeader(IItemContainerGenerator generator, Size constraint, List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			int count = parentDataGrid.Columns.Count;
			Size result = default(Size);
			if (this.RealizedChildren.Count == 0 && count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					DataGridColumn dataGridColumn = parentDataGrid.Columns[i];
					if (dataGridColumn.IsVisible)
					{
						int index = i;
						using (generator.StartAt(DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, index, out index), GeneratorDirection.Forward, true))
						{
							if (this.GenerateChild(generator, constraint, dataGridColumn, ref index, out result) != null)
							{
								int num = 0;
								DataGridCellsPanel.AddToIndicesListIfNeeded(realizedColumnIndices, realizedColumnDisplayIndices, i, dataGridColumn.DisplayIndex, ref num);
								return result;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060064B4 RID: 25780 RVA: 0x002A9D38 File Offset: 0x002A8D38
		private void EnsureFocusTrail(List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, int firstVisibleNonFrozenDisplayIndex, int lastVisibleNonFrozenDisplayIndex, Size constraint)
		{
			if (firstVisibleNonFrozenDisplayIndex < 0)
			{
				return;
			}
			int frozenColumnCount = this.ParentDataGrid.FrozenColumnCount;
			int count = this.Columns.Count;
			ItemsControl parentPresenter = this.ParentPresenter;
			if (parentPresenter == null)
			{
				return;
			}
			ItemContainerGenerator itemContainerGenerator = parentPresenter.ItemContainerGenerator;
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < firstVisibleNonFrozenDisplayIndex; i++)
			{
				if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, i, ref num))
				{
					num2 = i;
					break;
				}
			}
			if (num2 < frozenColumnCount)
			{
				for (int j = frozenColumnCount; j < count; j++)
				{
					if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, j, ref num))
					{
						num2 = j;
						break;
					}
				}
			}
			for (int k = firstVisibleNonFrozenDisplayIndex - 1; k > num2; k--)
			{
				if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, k, ref num))
				{
					num2 = k;
					break;
				}
			}
			for (int l = lastVisibleNonFrozenDisplayIndex + 1; l < count; l++)
			{
				if (this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, l, ref num))
				{
					num2 = l;
					break;
				}
			}
			int num3 = count - 1;
			while (num3 > num2 && !this.GenerateChildForFocusTrail(itemContainerGenerator, realizedColumnIndices, realizedColumnDisplayIndices, constraint, num3, ref num))
			{
				num3--;
			}
		}

		// Token: 0x060064B5 RID: 25781 RVA: 0x002A9E48 File Offset: 0x002A8E48
		private bool GenerateChildForFocusTrail(ItemContainerGenerator generator, List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, Size constraint, int displayIndex, ref int displayIndexListIterator)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(displayIndex);
			if (dataGridColumn.IsVisible)
			{
				int num = parentDataGrid.ColumnIndexFromDisplayIndex(displayIndex);
				UIElement uielement = generator.ContainerFromIndex(num) as UIElement;
				if (uielement == null)
				{
					int index = num;
					using (((IItemContainerGenerator)generator).StartAt(DataGridCellsPanel.IndexToGeneratorPositionForStart(generator, index, out index), GeneratorDirection.Forward, true))
					{
						Size size;
						uielement = this.GenerateChild(generator, constraint, dataGridColumn, ref index, out size);
					}
				}
				if (uielement != null && DataGridHelper.TreeHasFocusAndTabStop(uielement))
				{
					DataGridCellsPanel.AddToIndicesListIfNeeded(realizedColumnIndices, realizedColumnDisplayIndices, num, displayIndex, ref displayIndexListIterator);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060064B6 RID: 25782 RVA: 0x002A9EE4 File Offset: 0x002A8EE4
		private static void AddToIndicesListIfNeeded(List<int> realizedColumnIndices, List<int> realizedColumnDisplayIndices, int columnIndex, int displayIndex, ref int displayIndexListIterator)
		{
			int count = realizedColumnDisplayIndices.Count;
			while (displayIndexListIterator < count)
			{
				if (realizedColumnDisplayIndices[displayIndexListIterator] == displayIndex)
				{
					return;
				}
				if (realizedColumnDisplayIndices[displayIndexListIterator] > displayIndex)
				{
					realizedColumnDisplayIndices.Insert(displayIndexListIterator, displayIndex);
					realizedColumnIndices.Add(columnIndex);
					return;
				}
				displayIndexListIterator++;
			}
			realizedColumnIndices.Add(columnIndex);
			realizedColumnDisplayIndices.Add(displayIndex);
		}

		// Token: 0x060064B7 RID: 25783 RVA: 0x002A9F44 File Offset: 0x002A8F44
		private void VirtualizeChildren(List<RealizedColumnsBlock> blockList, IItemContainerGenerator generator)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			ObservableCollection<DataGridColumn> columns = parentDataGrid.Columns;
			int count = columns.Count;
			int num = 0;
			IList realizedChildren = this.RealizedChildren;
			int num2 = realizedChildren.Count;
			if (num2 == 0)
			{
				return;
			}
			int index = 0;
			int count2 = blockList.Count;
			RealizedColumnsBlock realizedColumnsBlock = (count2 > 0) ? blockList[index] : new RealizedColumnsBlock(-1, -1, -1);
			bool flag = count2 <= 0;
			int num3 = -1;
			int num4 = 0;
			int num5 = -1;
			ItemsControl parentPresenter = this.ParentPresenter;
			DataGridCellsPresenter dataGridCellsPresenter = parentPresenter as DataGridCellsPresenter;
			DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = parentPresenter as DataGridColumnHeadersPresenter;
			for (int i = 0; i < num2; i++)
			{
				int num6 = i;
				UIElement uielement = realizedChildren[i] as UIElement;
				IProvideDataGridColumn provideDataGridColumn = uielement as IProvideDataGridColumn;
				if (provideDataGridColumn != null)
				{
					DataGridColumn column = provideDataGridColumn.Column;
					while (num < count && column != columns[num])
					{
						num++;
					}
					num6 = num++;
				}
				bool flag2 = flag || !DataGridCellsPanel.InBlockOrNextBlock(blockList, num6, ref index, ref realizedColumnsBlock, out flag);
				DataGridCell dataGridCell = uielement as DataGridCell;
				if ((dataGridCell != null && (dataGridCell.IsEditing || dataGridCell.IsKeyboardFocusWithin || dataGridCell == parentDataGrid.FocusedCell)) || (dataGridCellsPresenter != null && dataGridCellsPresenter.IsItemItsOwnContainerInternal(dataGridCellsPresenter.Items[num6])) || (dataGridColumnHeadersPresenter != null && dataGridColumnHeadersPresenter.IsItemItsOwnContainerInternal(dataGridColumnHeadersPresenter.Items[num6])))
				{
					flag2 = false;
				}
				if (!columns[num6].IsVisible)
				{
					flag2 = true;
				}
				if (flag2)
				{
					if (num3 == -1)
					{
						num3 = i;
						num4 = 1;
					}
					else if (num5 == num6 - 1)
					{
						num4++;
					}
					else
					{
						this.CleanupRange(realizedChildren, generator, num3, num4);
						num2 -= num4;
						i -= num4;
						num4 = 1;
						num3 = i;
					}
					num5 = num6;
				}
				else if (num4 > 0)
				{
					this.CleanupRange(realizedChildren, generator, num3, num4);
					num2 -= num4;
					i -= num4;
					num4 = 0;
					num3 = -1;
				}
			}
			if (num4 > 0)
			{
				this.CleanupRange(realizedChildren, generator, num3, num4);
			}
		}

		// Token: 0x060064B8 RID: 25784 RVA: 0x002AA134 File Offset: 0x002A9134
		private void CleanupRange(IList children, IItemContainerGenerator generator, int startIndex, int count)
		{
			if (count <= 0)
			{
				return;
			}
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				GeneratorPosition position = new GeneratorPosition(startIndex, 0);
				((IRecyclingItemContainerGenerator)generator).Recycle(position, count);
				this._realizedChildren.RemoveRange(startIndex, count);
				return;
			}
			base.RemoveInternalChildRange(startIndex, count);
			generator.Remove(new GeneratorPosition(startIndex, 0), count);
		}

		// Token: 0x060064B9 RID: 25785 RVA: 0x002AA198 File Offset: 0x002A9198
		private void DisconnectRecycledContainers()
		{
			int num = 0;
			UIElement uielement = (this._realizedChildren.Count > 0) ? this._realizedChildren[0] : null;
			UIElementCollection internalChildren = base.InternalChildren;
			int num2 = -1;
			int num3 = 0;
			for (int i = 0; i < internalChildren.Count; i++)
			{
				if (internalChildren[i] == uielement)
				{
					if (num3 > 0)
					{
						base.RemoveInternalChildRange(num2, num3);
						i -= num3;
						num3 = 0;
						num2 = -1;
					}
					num++;
					if (num < this._realizedChildren.Count)
					{
						uielement = this._realizedChildren[num];
					}
					else
					{
						uielement = null;
					}
				}
				else
				{
					if (num2 == -1)
					{
						num2 = i;
					}
					num3++;
				}
			}
			if (num3 > 0)
			{
				base.RemoveInternalChildRange(num2, num3);
			}
		}

		// Token: 0x060064BA RID: 25786 RVA: 0x002AA24C File Offset: 0x002A924C
		private void InitializeArrangeState(DataGridCellsPanel.ArrangeState arrangeState)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			double cellsPanelHorizontalOffset = parentDataGrid.CellsPanelHorizontalOffset;
			arrangeState.NextFrozenCellStart = horizontalScrollOffset;
			arrangeState.NextNonFrozenCellStart -= cellsPanelHorizontalOffset;
			arrangeState.ViewportStartX = horizontalScrollOffset - cellsPanelHorizontalOffset;
			arrangeState.FrozenColumnCount = parentDataGrid.FrozenColumnCount;
		}

		// Token: 0x060064BB RID: 25787 RVA: 0x002AA298 File Offset: 0x002A9298
		private void FinishArrange(DataGridCellsPanel.ArrangeState arrangeState)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				parentDataGrid.NonFrozenColumnsViewportHorizontalOffset = arrangeState.DataGridHorizontalScrollStartX;
			}
			if (arrangeState.OldClippedChild != null)
			{
				arrangeState.OldClippedChild.CoerceValue(UIElement.ClipProperty);
			}
			this._clippedChildForFrozenBehaviour = arrangeState.NewClippedChild;
			if (this._clippedChildForFrozenBehaviour != null)
			{
				this._clippedChildForFrozenBehaviour.CoerceValue(UIElement.ClipProperty);
			}
		}

		// Token: 0x060064BC RID: 25788 RVA: 0x002AA2F7 File Offset: 0x002A92F7
		private void SetDataGridCellPanelWidth(IList children, double newWidth)
		{
			if (children.Count != 0 && children[0] is DataGridColumnHeader && !DoubleUtil.AreClose(this.ParentDataGrid.CellsPanelActualWidth, newWidth))
			{
				this.ParentDataGrid.CellsPanelActualWidth = newWidth;
			}
		}

		// Token: 0x060064BD RID: 25789 RVA: 0x002AA32E File Offset: 0x002A932E
		[Conditional("DEBUG")]
		private static void Debug_VerifyRealizedIndexCountVsDisplayIndexCount(List<RealizedColumnsBlock> blockList, List<RealizedColumnsBlock> displayIndexBlockList)
		{
			RealizedColumnsBlock realizedColumnsBlock = blockList[blockList.Count - 1];
			RealizedColumnsBlock realizedColumnsBlock2 = displayIndexBlockList[displayIndexBlockList.Count - 1];
		}

		// Token: 0x060064BE RID: 25790 RVA: 0x002AA350 File Offset: 0x002A9350
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			IList realizedChildren = this.RealizedChildren;
			DataGridCellsPanel.ArrangeState arrangeState = new DataGridCellsPanel.ArrangeState();
			arrangeState.ChildHeight = arrangeSize.Height;
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				parentDataGrid.QueueInvalidateCellsPanelHorizontalOffset();
				this.SetDataGridCellPanelWidth(realizedChildren, arrangeSize.Width);
				this.InitializeArrangeState(arrangeState);
			}
			List<RealizedColumnsBlock> realizedColumnsDisplayIndexBlockList = this.RealizedColumnsDisplayIndexBlockList;
			if (realizedColumnsDisplayIndexBlockList != null && realizedColumnsDisplayIndexBlockList.Count > 0)
			{
				double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
				List<RealizedColumnsBlock> realizedColumnsBlockList = this.RealizedColumnsBlockList;
				List<int> realizedChildrenNotInBlockList = this.GetRealizedChildrenNotInBlockList(realizedColumnsBlockList, realizedChildren);
				int num = -1;
				RealizedColumnsBlock realizedColumnsBlock = realizedColumnsDisplayIndexBlockList[++num];
				bool flag = false;
				int i = 0;
				int count = parentDataGrid.Columns.Count;
				while (i < count)
				{
					bool flag2 = DataGridCellsPanel.InBlockOrNextBlock(realizedColumnsDisplayIndexBlockList, i, ref num, ref realizedColumnsBlock, out flag);
					if (flag)
					{
						break;
					}
					if (flag2)
					{
						int num2 = parentDataGrid.ColumnIndexFromDisplayIndex(i);
						RealizedColumnsBlock realizedBlockForColumn = DataGridCellsPanel.GetRealizedBlockForColumn(realizedColumnsBlockList, num2);
						int num3 = realizedBlockForColumn.StartIndexOffset + num2 - realizedBlockForColumn.StartIndex;
						if (realizedChildrenNotInBlockList != null)
						{
							int num4 = 0;
							int count2 = realizedChildrenNotInBlockList.Count;
							while (num4 < count2 && realizedChildrenNotInBlockList[num4] <= num3)
							{
								num3++;
								num4++;
							}
						}
						this.ArrangeChild(realizedChildren[num3] as UIElement, i, arrangeState);
					}
					else
					{
						DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
						if (dataGridColumn.IsVisible)
						{
							double columnEstimatedMeasureWidth = DataGridCellsPanel.GetColumnEstimatedMeasureWidth(dataGridColumn, averageColumnWidth);
							arrangeState.NextNonFrozenCellStart += columnEstimatedMeasureWidth;
						}
					}
					i++;
				}
				if (realizedChildrenNotInBlockList != null)
				{
					int j = 0;
					int count3 = realizedChildrenNotInBlockList.Count;
					while (j < count3)
					{
						(realizedChildren[realizedChildrenNotInBlockList[j]] as UIElement).Arrange(default(Rect));
						j++;
					}
				}
			}
			this.FinishArrange(arrangeState);
			return arrangeSize;
		}

		// Token: 0x060064BF RID: 25791 RVA: 0x002AA514 File Offset: 0x002A9514
		private void ArrangeChild(UIElement child, int displayIndex, DataGridCellsPanel.ArrangeState arrangeState)
		{
			IProvideDataGridColumn provideDataGridColumn = child as IProvideDataGridColumn;
			if (child == this._clippedChildForFrozenBehaviour)
			{
				arrangeState.OldClippedChild = child;
				this._clippedChildForFrozenBehaviour = null;
			}
			double num;
			if (provideDataGridColumn != null)
			{
				num = provideDataGridColumn.Column.Width.DisplayValue;
				if (DoubleUtil.IsNaN(num))
				{
					num = provideDataGridColumn.Column.ActualWidth;
				}
			}
			else
			{
				num = child.DesiredSize.Width;
			}
			Rect finalRect = new Rect(new Size(num, arrangeState.ChildHeight));
			if (displayIndex < arrangeState.FrozenColumnCount)
			{
				finalRect.X = arrangeState.NextFrozenCellStart;
				arrangeState.NextFrozenCellStart += num;
				arrangeState.DataGridHorizontalScrollStartX += num;
			}
			else if (DoubleUtil.LessThanOrClose(arrangeState.NextNonFrozenCellStart, arrangeState.ViewportStartX))
			{
				if (DoubleUtil.LessThanOrClose(arrangeState.NextNonFrozenCellStart + num, arrangeState.ViewportStartX))
				{
					finalRect.X = arrangeState.NextNonFrozenCellStart;
					arrangeState.NextNonFrozenCellStart += num;
				}
				else
				{
					double num2 = arrangeState.ViewportStartX - arrangeState.NextNonFrozenCellStart;
					if (DoubleUtil.AreClose(num2, 0.0))
					{
						finalRect.X = arrangeState.NextFrozenCellStart;
						arrangeState.NextNonFrozenCellStart = arrangeState.NextFrozenCellStart + num;
					}
					else
					{
						finalRect.X = arrangeState.NextFrozenCellStart - num2;
						double num3 = num - num2;
						arrangeState.NewClippedChild = child;
						this._childClipForFrozenBehavior.Rect = new Rect(num2, 0.0, num3, finalRect.Height);
						arrangeState.NextNonFrozenCellStart = arrangeState.NextFrozenCellStart + num3;
					}
				}
			}
			else
			{
				finalRect.X = arrangeState.NextNonFrozenCellStart;
				arrangeState.NextNonFrozenCellStart += num;
			}
			child.Arrange(finalRect);
		}

		// Token: 0x060064C0 RID: 25792 RVA: 0x002AA6D0 File Offset: 0x002A96D0
		private static RealizedColumnsBlock GetRealizedBlockForColumn(List<RealizedColumnsBlock> blockList, int columnIndex)
		{
			int i = 0;
			int count = blockList.Count;
			while (i < count)
			{
				RealizedColumnsBlock result = blockList[i];
				if (columnIndex >= result.StartIndex && columnIndex <= result.EndIndex)
				{
					return result;
				}
				i++;
			}
			return new RealizedColumnsBlock(-1, -1, -1);
		}

		// Token: 0x060064C1 RID: 25793 RVA: 0x002AA718 File Offset: 0x002A9718
		private List<int> GetRealizedChildrenNotInBlockList(List<RealizedColumnsBlock> blockList, IList children)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			RealizedColumnsBlock realizedColumnsBlock = blockList[blockList.Count - 1];
			int num = realizedColumnsBlock.StartIndexOffset + realizedColumnsBlock.EndIndex - realizedColumnsBlock.StartIndex + 1;
			if (children.Count == num)
			{
				return null;
			}
			List<int> list = new List<int>();
			if (blockList.Count == 0)
			{
				int i = 0;
				int count = children.Count;
				while (i < count)
				{
					list.Add(i);
					i++;
				}
			}
			else
			{
				int num2 = 0;
				RealizedColumnsBlock realizedColumnsBlock2 = blockList[num2++];
				int j = 0;
				int count2 = children.Count;
				while (j < count2)
				{
					IProvideDataGridColumn provideDataGridColumn = children[j] as IProvideDataGridColumn;
					int num3 = j;
					if (provideDataGridColumn != null)
					{
						num3 = parentDataGrid.Columns.IndexOf(provideDataGridColumn.Column);
					}
					if (num3 < realizedColumnsBlock2.StartIndex)
					{
						list.Add(j);
					}
					else if (num3 > realizedColumnsBlock2.EndIndex)
					{
						if (num2 >= blockList.Count)
						{
							for (int k = j; k < count2; k++)
							{
								list.Add(k);
							}
							break;
						}
						realizedColumnsBlock2 = blockList[num2++];
						if (num3 < realizedColumnsBlock2.StartIndex)
						{
							list.Add(j);
						}
					}
					j++;
				}
			}
			return list;
		}

		// Token: 0x1700174B RID: 5963
		// (get) Token: 0x060064C2 RID: 25794 RVA: 0x002AA85C File Offset: 0x002A985C
		internal bool HasCorrectRealizedColumns
		{
			get
			{
				DataGridColumnCollection dataGridColumnCollection = (DataGridColumnCollection)this.ParentDataGrid.Columns;
				this.EnsureRealizedChildren();
				IList realizedChildren = this.RealizedChildren;
				if (realizedChildren.Count == dataGridColumnCollection.Count)
				{
					return true;
				}
				List<int> displayIndexMap = dataGridColumnCollection.DisplayIndexMap;
				List<RealizedColumnsBlock> realizedColumnsBlockList = this.RealizedColumnsBlockList;
				int i = 0;
				int count = realizedChildren.Count;
				for (int j = 0; j < realizedColumnsBlockList.Count; j++)
				{
					RealizedColumnsBlock realizedColumnsBlock = realizedColumnsBlockList[j];
					for (int k = realizedColumnsBlock.StartIndex; k <= realizedColumnsBlock.EndIndex; k++)
					{
						while (i < count)
						{
							IProvideDataGridColumn provideDataGridColumn = realizedChildren[i] as IProvideDataGridColumn;
							if (provideDataGridColumn != null)
							{
								int displayIndex = provideDataGridColumn.Column.DisplayIndex;
								int num = (displayIndex < 0) ? -1 : displayIndexMap[displayIndex];
								if (k < num)
								{
									return false;
								}
								if (k == num)
								{
									break;
								}
							}
							i++;
						}
						if (i == count)
						{
							return false;
						}
						i++;
					}
				}
				return true;
			}
		}

		// Token: 0x1700174C RID: 5964
		// (get) Token: 0x060064C3 RID: 25795 RVA: 0x002AA950 File Offset: 0x002A9950
		// (set) Token: 0x060064C4 RID: 25796 RVA: 0x002AA988 File Offset: 0x002A9988
		private bool RebuildRealizedColumnsBlockList
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return true;
				}
				DataGridColumnCollection internalColumns = parentDataGrid.InternalColumns;
				if (!this.IsVirtualizing)
				{
					return internalColumns.RebuildRealizedColumnsBlockListForNonVirtualizedRows;
				}
				return internalColumns.RebuildRealizedColumnsBlockListForVirtualizedRows;
			}
			set
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					if (this.IsVirtualizing)
					{
						parentDataGrid.InternalColumns.RebuildRealizedColumnsBlockListForVirtualizedRows = value;
						return;
					}
					parentDataGrid.InternalColumns.RebuildRealizedColumnsBlockListForNonVirtualizedRows = value;
				}
			}
		}

		// Token: 0x1700174D RID: 5965
		// (get) Token: 0x060064C5 RID: 25797 RVA: 0x002AA9C0 File Offset: 0x002A99C0
		// (set) Token: 0x060064C6 RID: 25798 RVA: 0x002AA9F8 File Offset: 0x002A99F8
		private List<RealizedColumnsBlock> RealizedColumnsBlockList
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return null;
				}
				DataGridColumnCollection internalColumns = parentDataGrid.InternalColumns;
				if (!this.IsVirtualizing)
				{
					return internalColumns.RealizedColumnsBlockListForNonVirtualizedRows;
				}
				return internalColumns.RealizedColumnsBlockListForVirtualizedRows;
			}
			set
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					if (this.IsVirtualizing)
					{
						parentDataGrid.InternalColumns.RealizedColumnsBlockListForVirtualizedRows = value;
						return;
					}
					parentDataGrid.InternalColumns.RealizedColumnsBlockListForNonVirtualizedRows = value;
				}
			}
		}

		// Token: 0x1700174E RID: 5966
		// (get) Token: 0x060064C7 RID: 25799 RVA: 0x002AAA30 File Offset: 0x002A9A30
		// (set) Token: 0x060064C8 RID: 25800 RVA: 0x002AAA68 File Offset: 0x002A9A68
		private List<RealizedColumnsBlock> RealizedColumnsDisplayIndexBlockList
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return null;
				}
				DataGridColumnCollection internalColumns = parentDataGrid.InternalColumns;
				if (!this.IsVirtualizing)
				{
					return internalColumns.RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows;
				}
				return internalColumns.RealizedColumnsDisplayIndexBlockListForVirtualizedRows;
			}
			set
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					if (this.IsVirtualizing)
					{
						parentDataGrid.InternalColumns.RealizedColumnsDisplayIndexBlockListForVirtualizedRows = value;
						return;
					}
					parentDataGrid.InternalColumns.RealizedColumnsDisplayIndexBlockListForNonVirtualizedRows = value;
				}
			}
		}

		// Token: 0x060064C9 RID: 25801 RVA: 0x002AAAA0 File Offset: 0x002A9AA0
		protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
		{
			base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
			if (newIsItemsHost)
			{
				ItemsControl parentPresenter = this.ParentPresenter;
				if (parentPresenter != null)
				{
					IItemContainerGenerator itemContainerGenerator = parentPresenter.ItemContainerGenerator;
					if (itemContainerGenerator != null && itemContainerGenerator == itemContainerGenerator.GetItemContainerGeneratorForPanel(this))
					{
						DataGridCellsPresenter dataGridCellsPresenter = parentPresenter as DataGridCellsPresenter;
						if (dataGridCellsPresenter != null)
						{
							dataGridCellsPresenter.InternalItemsHost = this;
							return;
						}
						DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = parentPresenter as DataGridColumnHeadersPresenter;
						if (dataGridColumnHeadersPresenter != null)
						{
							dataGridColumnHeadersPresenter.InternalItemsHost = this;
							return;
						}
					}
				}
			}
			else
			{
				ItemsControl parentPresenter2 = this.ParentPresenter;
				if (parentPresenter2 != null)
				{
					DataGridCellsPresenter dataGridCellsPresenter2 = parentPresenter2 as DataGridCellsPresenter;
					if (dataGridCellsPresenter2 != null)
					{
						if (dataGridCellsPresenter2.InternalItemsHost == this)
						{
							dataGridCellsPresenter2.InternalItemsHost = null;
							return;
						}
					}
					else
					{
						DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter2 = parentPresenter2 as DataGridColumnHeadersPresenter;
						if (dataGridColumnHeadersPresenter2 != null && dataGridColumnHeadersPresenter2.InternalItemsHost == this)
						{
							dataGridColumnHeadersPresenter2.InternalItemsHost = null;
						}
					}
				}
			}
		}

		// Token: 0x1700174F RID: 5967
		// (get) Token: 0x060064CA RID: 25802 RVA: 0x002AAB48 File Offset: 0x002A9B48
		private DataGridRowsPresenter ParentRowsPresenter
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid == null)
				{
					return null;
				}
				if (!parentDataGrid.IsGrouping)
				{
					return parentDataGrid.InternalItemsHost as DataGridRowsPresenter;
				}
				DataGridCellsPresenter dataGridCellsPresenter = this.ParentPresenter as DataGridCellsPresenter;
				if (dataGridCellsPresenter != null)
				{
					DataGridRow dataGridRowOwner = dataGridCellsPresenter.DataGridRowOwner;
					if (dataGridRowOwner != null)
					{
						return VisualTreeHelper.GetParent(dataGridRowOwner) as DataGridRowsPresenter;
					}
				}
				return null;
			}
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x002AAB9C File Offset: 0x002A9B9C
		private void DetermineVirtualizationState()
		{
			ItemsControl parentPresenter = this.ParentPresenter;
			if (parentPresenter != null)
			{
				this.IsVirtualizing = VirtualizingPanel.GetIsVirtualizing(parentPresenter);
				this.InRecyclingMode = (VirtualizingPanel.GetVirtualizationMode(parentPresenter) == VirtualizationMode.Recycling);
			}
		}

		// Token: 0x17001750 RID: 5968
		// (get) Token: 0x060064CC RID: 25804 RVA: 0x002AABCE File Offset: 0x002A9BCE
		// (set) Token: 0x060064CD RID: 25805 RVA: 0x002AABD6 File Offset: 0x002A9BD6
		private bool IsVirtualizing { get; set; }

		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x060064CE RID: 25806 RVA: 0x002AABDF File Offset: 0x002A9BDF
		// (set) Token: 0x060064CF RID: 25807 RVA: 0x002AABE7 File Offset: 0x002A9BE7
		private bool InRecyclingMode { get; set; }

		// Token: 0x060064D0 RID: 25808 RVA: 0x002AABF0 File Offset: 0x002A9BF0
		private static double GetColumnEstimatedMeasureWidth(DataGridColumn column, double averageColumnWidth)
		{
			if (!column.IsVisible)
			{
				return 0.0;
			}
			double num = column.Width.DisplayValue;
			if (DoubleUtil.IsNaN(num))
			{
				num = Math.Max(averageColumnWidth, column.MinWidth);
				num = Math.Min(num, column.MaxWidth);
			}
			return num;
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x002AAC44 File Offset: 0x002A9C44
		private double GetColumnEstimatedMeasureWidthSum(int startIndex, int endIndex, double averageColumnWidth)
		{
			double num = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			for (int i = startIndex; i <= endIndex; i++)
			{
				num += DataGridCellsPanel.GetColumnEstimatedMeasureWidth(parentDataGrid.Columns[i], averageColumnWidth);
			}
			return num;
		}

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x060064D2 RID: 25810 RVA: 0x002AAC84 File Offset: 0x002A9C84
		private IList RealizedChildren
		{
			get
			{
				if (this.IsVirtualizing && this.InRecyclingMode)
				{
					return this._realizedChildren;
				}
				return base.InternalChildren;
			}
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x002AACA4 File Offset: 0x002A9CA4
		private void EnsureRealizedChildren()
		{
			if (this.IsVirtualizing && this.InRecyclingMode)
			{
				if (this._realizedChildren == null)
				{
					UIElementCollection internalChildren = base.InternalChildren;
					this._realizedChildren = new List<UIElement>(internalChildren.Count);
					for (int i = 0; i < internalChildren.Count; i++)
					{
						this._realizedChildren.Add(internalChildren[i]);
					}
					return;
				}
			}
			else
			{
				this._realizedChildren = null;
			}
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x002AAD0C File Offset: 0x002A9D0C
		internal double ComputeCellsPanelHorizontalOffset()
		{
			double result = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			ScrollViewer internalScrollHost = parentDataGrid.InternalScrollHost;
			if (internalScrollHost != null)
			{
				result = horizontalScrollOffset + base.TransformToAncestor(internalScrollHost).Transform(default(Point)).X;
			}
			return result;
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x002AAD5C File Offset: 0x002A9D5C
		private double GetViewportWidth()
		{
			double num = 0.0;
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null)
			{
				ScrollContentPresenter internalScrollContentPresenter = parentDataGrid.InternalScrollContentPresenter;
				if (internalScrollContentPresenter != null && !internalScrollContentPresenter.CanContentScroll)
				{
					num = internalScrollContentPresenter.ViewportWidth;
				}
				else
				{
					IScrollInfo scrollInfo = parentDataGrid.InternalItemsHost as IScrollInfo;
					if (scrollInfo != null)
					{
						num = scrollInfo.ViewportWidth;
					}
				}
			}
			DataGridRowsPresenter parentRowsPresenter = this.ParentRowsPresenter;
			if (DoubleUtil.AreClose(num, 0.0) && parentRowsPresenter != null)
			{
				Size availableSize = parentRowsPresenter.AvailableSize;
				if (!DoubleUtil.IsNaN(availableSize.Width) && !double.IsInfinity(availableSize.Width))
				{
					num = availableSize.Width;
				}
				else if (parentDataGrid.IsGrouping)
				{
					IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = DataGridHelper.FindParent<GroupItem>(parentRowsPresenter);
					if (hierarchicalVirtualizationAndScrollInfo != null)
					{
						num = hierarchicalVirtualizationAndScrollInfo.Constraints.Viewport.Width;
					}
				}
			}
			return num;
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x002AAE2C File Offset: 0x002A9E2C
		protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			base.OnItemsChanged(sender, args);
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Remove:
				this.OnItemsRemove(args);
				return;
			case NotifyCollectionChangedAction.Replace:
				this.OnItemsReplace(args);
				return;
			case NotifyCollectionChangedAction.Move:
				this.OnItemsMove(args);
				break;
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				return;
			}
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x002AAE78 File Offset: 0x002A9E78
		private void OnItemsRemove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.Position, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x002AAE78 File Offset: 0x002A9E78
		private void OnItemsReplace(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.Position, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x002AAE92 File Offset: 0x002A9E92
		private void OnItemsMove(ItemsChangedEventArgs args)
		{
			this.RemoveChildRange(args.OldPosition, args.ItemCount, args.ItemUICount);
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x002AAEAC File Offset: 0x002A9EAC
		private void RemoveChildRange(GeneratorPosition position, int itemCount, int itemUICount)
		{
			if (base.IsItemsHost)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				int num = position.Index;
				if (position.Offset > 0)
				{
					num++;
				}
				if (num < internalChildren.Count && itemUICount > 0)
				{
					base.RemoveInternalChildRange(num, itemUICount);
					if (this.IsVirtualizing && this.InRecyclingMode)
					{
						this._realizedChildren.RemoveRange(num, itemUICount);
					}
				}
			}
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x002AAF10 File Offset: 0x002A9F10
		protected override void OnClearChildren()
		{
			base.OnClearChildren();
			this._realizedChildren = null;
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x002AAF1F File Offset: 0x002A9F1F
		internal void InternalBringIndexIntoView(int index)
		{
			this.BringIndexIntoView(index);
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x002AAF28 File Offset: 0x002A9F28
		protected internal override void BringIndexIntoView(int index)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid == null)
			{
				base.BringIndexIntoView(index);
				return;
			}
			if (index < 0 || index >= parentDataGrid.Columns.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (parentDataGrid.InternalColumns.ColumnWidthsComputationPending)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action<int>(this.RetryBringIndexIntoView), index);
				return;
			}
			ScrollContentPresenter internalScrollContentPresenter = parentDataGrid.InternalScrollContentPresenter;
			IScrollInfo scrollInfo = null;
			if (internalScrollContentPresenter != null && !internalScrollContentPresenter.CanContentScroll)
			{
				scrollInfo = internalScrollContentPresenter;
			}
			else
			{
				ScrollViewer internalScrollHost = parentDataGrid.InternalScrollHost;
				if (internalScrollHost != null)
				{
					scrollInfo = internalScrollHost.ScrollInfo;
				}
			}
			if (scrollInfo == null)
			{
				base.BringIndexIntoView(index);
				return;
			}
			bool measureDirty = base.MeasureDirty;
			bool retryRequested = measureDirty;
			double num = 0.0;
			double value = parentDataGrid.HorizontalScrollOffset;
			while (!this.IsChildInView(index, out num) && !DoubleUtil.AreClose(value, num))
			{
				retryRequested = true;
				scrollInfo.SetHorizontalOffset(num);
				base.UpdateLayout();
				value = num;
			}
			if (parentDataGrid.RetryBringColumnIntoView(retryRequested))
			{
				DispatcherPriority priority = measureDirty ? DispatcherPriority.Background : DispatcherPriority.Loaded;
				base.Dispatcher.BeginInvoke(priority, new Action<int>(this.RetryBringIndexIntoView), index);
				base.InvalidateMeasure();
			}
		}

		// Token: 0x060064DE RID: 25822 RVA: 0x002AB04C File Offset: 0x002AA04C
		private void RetryBringIndexIntoView(int index)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			if (parentDataGrid != null && 0 <= index && index < parentDataGrid.Columns.Count)
			{
				this.BringIndexIntoView(index);
			}
		}

		// Token: 0x060064DF RID: 25823 RVA: 0x002AB07C File Offset: 0x002AA07C
		private bool IsChildInView(int index, out double newHorizontalOffset)
		{
			DataGrid parentDataGrid = this.ParentDataGrid;
			double horizontalScrollOffset = parentDataGrid.HorizontalScrollOffset;
			newHorizontalOffset = horizontalScrollOffset;
			double averageColumnWidth = parentDataGrid.InternalColumns.AverageColumnWidth;
			int frozenColumnCount = parentDataGrid.FrozenColumnCount;
			double cellsPanelHorizontalOffset = parentDataGrid.CellsPanelHorizontalOffset;
			double viewportWidth = this.GetViewportWidth();
			double num = horizontalScrollOffset;
			double num2 = -cellsPanelHorizontalOffset;
			double num3 = horizontalScrollOffset - cellsPanelHorizontalOffset;
			int displayIndex = this.Columns[index].DisplayIndex;
			double num4 = 0.0;
			double num5 = 0.0;
			for (int i = 0; i <= displayIndex; i++)
			{
				DataGridColumn dataGridColumn = parentDataGrid.ColumnFromDisplayIndex(i);
				if (dataGridColumn.IsVisible)
				{
					double columnEstimatedMeasureWidth = DataGridCellsPanel.GetColumnEstimatedMeasureWidth(dataGridColumn, averageColumnWidth);
					if (i < frozenColumnCount)
					{
						num4 = num;
						num5 = num4 + columnEstimatedMeasureWidth;
						num += columnEstimatedMeasureWidth;
					}
					else if (DoubleUtil.LessThanOrClose(num2, num3))
					{
						if (DoubleUtil.LessThanOrClose(num2 + columnEstimatedMeasureWidth, num3))
						{
							num4 = num2;
							num5 = num4 + columnEstimatedMeasureWidth;
							num2 += columnEstimatedMeasureWidth;
						}
						else
						{
							num4 = num;
							double num6 = num3 - num2;
							if (DoubleUtil.AreClose(num6, 0.0))
							{
								num5 = num4 + columnEstimatedMeasureWidth;
								num2 = num + columnEstimatedMeasureWidth;
							}
							else
							{
								double num7 = columnEstimatedMeasureWidth - num6;
								num5 = num4 + num7;
								num2 = num + num7;
								if (i == displayIndex)
								{
									newHorizontalOffset = horizontalScrollOffset - num6;
									return false;
								}
							}
						}
					}
					else
					{
						num4 = num2;
						num5 = num4 + columnEstimatedMeasureWidth;
						num2 += columnEstimatedMeasureWidth;
					}
				}
			}
			double num8 = num3 + viewportWidth;
			if (DoubleUtil.LessThan(num4, num3))
			{
				newHorizontalOffset = num4 + cellsPanelHorizontalOffset;
			}
			else
			{
				if (!DoubleUtil.GreaterThan(num5, num8))
				{
					return true;
				}
				double num9 = num5 - num8;
				if (displayIndex < frozenColumnCount)
				{
					num -= num5 - num4;
				}
				if (DoubleUtil.LessThan(num4 - num9, num))
				{
					num9 = num4 - num;
				}
				if (DoubleUtil.AreClose(num9, 0.0))
				{
					return true;
				}
				newHorizontalOffset = horizontalScrollOffset + num9;
			}
			return false;
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x002AB246 File Offset: 0x002AA246
		internal Geometry GetFrozenClipForChild(UIElement child)
		{
			if (child == this._clippedChildForFrozenBehaviour)
			{
				return this._childClipForFrozenBehavior;
			}
			return null;
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x060064E1 RID: 25825 RVA: 0x002AB25C File Offset: 0x002AA25C
		private ObservableCollection<DataGridColumn> Columns
		{
			get
			{
				DataGrid parentDataGrid = this.ParentDataGrid;
				if (parentDataGrid != null)
				{
					return parentDataGrid.Columns;
				}
				return null;
			}
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x060064E2 RID: 25826 RVA: 0x002AB27C File Offset: 0x002AA27C
		private DataGrid ParentDataGrid
		{
			get
			{
				if (this._parentDataGrid == null)
				{
					DataGridCellsPresenter dataGridCellsPresenter = this.ParentPresenter as DataGridCellsPresenter;
					if (dataGridCellsPresenter != null)
					{
						DataGridRow dataGridRowOwner = dataGridCellsPresenter.DataGridRowOwner;
						if (dataGridRowOwner != null)
						{
							this._parentDataGrid = dataGridRowOwner.DataGridOwner;
						}
					}
					else
					{
						DataGridColumnHeadersPresenter dataGridColumnHeadersPresenter = this.ParentPresenter as DataGridColumnHeadersPresenter;
						if (dataGridColumnHeadersPresenter != null)
						{
							this._parentDataGrid = dataGridColumnHeadersPresenter.ParentDataGrid;
						}
					}
				}
				return this._parentDataGrid;
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x060064E3 RID: 25827 RVA: 0x002AB2DC File Offset: 0x002AA2DC
		private ItemsControl ParentPresenter
		{
			get
			{
				FrameworkElement frameworkElement = base.TemplatedParent as FrameworkElement;
				if (frameworkElement != null)
				{
					return frameworkElement.TemplatedParent as ItemsControl;
				}
				return null;
			}
		}

		// Token: 0x04003356 RID: 13142
		private DataGrid _parentDataGrid;

		// Token: 0x04003357 RID: 13143
		private UIElement _clippedChildForFrozenBehaviour;

		// Token: 0x04003358 RID: 13144
		private RectangleGeometry _childClipForFrozenBehavior = new RectangleGeometry();

		// Token: 0x04003359 RID: 13145
		private List<UIElement> _realizedChildren;

		// Token: 0x02000BCB RID: 3019
		private class ArrangeState
		{
			// Token: 0x06008F62 RID: 36706 RVA: 0x00344374 File Offset: 0x00343374
			public ArrangeState()
			{
				this.FrozenColumnCount = 0;
				this.ChildHeight = 0.0;
				this.NextFrozenCellStart = 0.0;
				this.NextNonFrozenCellStart = 0.0;
				this.ViewportStartX = 0.0;
				this.DataGridHorizontalScrollStartX = 0.0;
				this.OldClippedChild = null;
				this.NewClippedChild = null;
			}

			// Token: 0x17001F58 RID: 8024
			// (get) Token: 0x06008F63 RID: 36707 RVA: 0x003443E7 File Offset: 0x003433E7
			// (set) Token: 0x06008F64 RID: 36708 RVA: 0x003443EF File Offset: 0x003433EF
			public int FrozenColumnCount { get; set; }

			// Token: 0x17001F59 RID: 8025
			// (get) Token: 0x06008F65 RID: 36709 RVA: 0x003443F8 File Offset: 0x003433F8
			// (set) Token: 0x06008F66 RID: 36710 RVA: 0x00344400 File Offset: 0x00343400
			public double ChildHeight { get; set; }

			// Token: 0x17001F5A RID: 8026
			// (get) Token: 0x06008F67 RID: 36711 RVA: 0x00344409 File Offset: 0x00343409
			// (set) Token: 0x06008F68 RID: 36712 RVA: 0x00344411 File Offset: 0x00343411
			public double NextFrozenCellStart { get; set; }

			// Token: 0x17001F5B RID: 8027
			// (get) Token: 0x06008F69 RID: 36713 RVA: 0x0034441A File Offset: 0x0034341A
			// (set) Token: 0x06008F6A RID: 36714 RVA: 0x00344422 File Offset: 0x00343422
			public double NextNonFrozenCellStart { get; set; }

			// Token: 0x17001F5C RID: 8028
			// (get) Token: 0x06008F6B RID: 36715 RVA: 0x0034442B File Offset: 0x0034342B
			// (set) Token: 0x06008F6C RID: 36716 RVA: 0x00344433 File Offset: 0x00343433
			public double ViewportStartX { get; set; }

			// Token: 0x17001F5D RID: 8029
			// (get) Token: 0x06008F6D RID: 36717 RVA: 0x0034443C File Offset: 0x0034343C
			// (set) Token: 0x06008F6E RID: 36718 RVA: 0x00344444 File Offset: 0x00343444
			public double DataGridHorizontalScrollStartX { get; set; }

			// Token: 0x17001F5E RID: 8030
			// (get) Token: 0x06008F6F RID: 36719 RVA: 0x0034444D File Offset: 0x0034344D
			// (set) Token: 0x06008F70 RID: 36720 RVA: 0x00344455 File Offset: 0x00343455
			public UIElement OldClippedChild { get; set; }

			// Token: 0x17001F5F RID: 8031
			// (get) Token: 0x06008F71 RID: 36721 RVA: 0x0034445E File Offset: 0x0034345E
			// (set) Token: 0x06008F72 RID: 36722 RVA: 0x00344466 File Offset: 0x00343466
			public UIElement NewClippedChild { get; set; }
		}
	}
}
