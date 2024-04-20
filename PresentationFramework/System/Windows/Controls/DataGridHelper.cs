using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000758 RID: 1880
	internal static class DataGridHelper
	{
		// Token: 0x06006627 RID: 26151 RVA: 0x002B0D24 File Offset: 0x002AFD24
		public static Size SubtractFromSize(Size size, double thickness, bool height)
		{
			if (height)
			{
				return new Size(size.Width, Math.Max(0.0, size.Height - thickness));
			}
			return new Size(Math.Max(0.0, size.Width - thickness), size.Height);
		}

		// Token: 0x06006628 RID: 26152 RVA: 0x002B0D7C File Offset: 0x002AFD7C
		public static bool IsGridLineVisible(DataGrid dataGrid, bool isHorizontal)
		{
			if (dataGrid != null)
			{
				switch (dataGrid.GridLinesVisibility)
				{
				case DataGridGridLinesVisibility.All:
					return true;
				case DataGridGridLinesVisibility.Horizontal:
					return isHorizontal;
				case DataGridGridLinesVisibility.None:
					return false;
				case DataGridGridLinesVisibility.Vertical:
					return !isHorizontal;
				}
			}
			return false;
		}

		// Token: 0x06006629 RID: 26153 RVA: 0x002B0DB7 File Offset: 0x002AFDB7
		public static bool ShouldNotifyCells(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.Cells);
		}

		// Token: 0x0600662A RID: 26154 RVA: 0x002B0DC0 File Offset: 0x002AFDC0
		public static bool ShouldNotifyCellsPresenter(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.CellsPresenter);
		}

		// Token: 0x0600662B RID: 26155 RVA: 0x002B0DC9 File Offset: 0x002AFDC9
		public static bool ShouldNotifyColumns(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.Columns);
		}

		// Token: 0x0600662C RID: 26156 RVA: 0x002B0DD2 File Offset: 0x002AFDD2
		public static bool ShouldNotifyColumnHeaders(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x0600662D RID: 26157 RVA: 0x002B0DDC File Offset: 0x002AFDDC
		public static bool ShouldNotifyColumnHeadersPresenter(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		// Token: 0x0600662E RID: 26158 RVA: 0x002B0DE6 File Offset: 0x002AFDE6
		public static bool ShouldNotifyColumnCollection(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.ColumnCollection);
		}

		// Token: 0x0600662F RID: 26159 RVA: 0x002B0DEF File Offset: 0x002AFDEF
		public static bool ShouldNotifyDataGrid(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.DataGrid);
		}

		// Token: 0x06006630 RID: 26160 RVA: 0x002B0DF9 File Offset: 0x002AFDF9
		public static bool ShouldNotifyDetailsPresenter(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.DetailsPresenter);
		}

		// Token: 0x06006631 RID: 26161 RVA: 0x002B0E06 File Offset: 0x002AFE06
		public static bool ShouldRefreshCellContent(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.RefreshCellContent);
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x002B0E13 File Offset: 0x002AFE13
		public static bool ShouldNotifyRowHeaders(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.RowHeaders);
		}

		// Token: 0x06006633 RID: 26163 RVA: 0x002B0E20 File Offset: 0x002AFE20
		public static bool ShouldNotifyRows(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.Rows);
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x002B0E30 File Offset: 0x002AFE30
		public static bool ShouldNotifyRowSubtree(DataGridNotificationTarget target)
		{
			DataGridNotificationTarget value = DataGridNotificationTarget.Cells | DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.RefreshCellContent | DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows;
			return DataGridHelper.TestTarget(target, value);
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x000F87A7 File Offset: 0x000F77A7
		private static bool TestTarget(DataGridNotificationTarget target, DataGridNotificationTarget value)
		{
			return (target & value) > DataGridNotificationTarget.None;
		}

		// Token: 0x06006636 RID: 26166 RVA: 0x002B0E4C File Offset: 0x002AFE4C
		public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
		{
			for (FrameworkElement frameworkElement = element.TemplatedParent as FrameworkElement; frameworkElement != null; frameworkElement = (frameworkElement.TemplatedParent as FrameworkElement))
			{
				T t = frameworkElement as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x002B0E98 File Offset: 0x002AFE98
		public static T FindVisualParent<T>(UIElement element) where T : UIElement
		{
			for (UIElement uielement = element; uielement != null; uielement = (VisualTreeHelper.GetParent(uielement) as UIElement))
			{
				T t = uielement as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x002B0ED8 File Offset: 0x002AFED8
		public static bool TreeHasFocusAndTabStop(DependencyObject element)
		{
			if (element == null)
			{
				return false;
			}
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				if (uielement.Focusable && KeyboardNavigation.GetIsTabStop(uielement))
				{
					return true;
				}
			}
			else
			{
				ContentElement contentElement = element as ContentElement;
				if (contentElement != null && contentElement.Focusable && KeyboardNavigation.GetIsTabStop(contentElement))
				{
					return true;
				}
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childrenCount; i++)
			{
				if (DataGridHelper.TreeHasFocusAndTabStop(VisualTreeHelper.GetChild(element, i)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006639 RID: 26169 RVA: 0x002B0F48 File Offset: 0x002AFF48
		public static void OnColumnWidthChanged(IProvideDataGridColumn cell, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = (UIElement)cell;
			DataGridColumn column = cell.Column;
			bool flag = cell is DataGridColumnHeader;
			if (column != null)
			{
				DataGridLength width = column.Width;
				if (width.IsAuto || (!flag && width.IsSizeToCells) || (flag && width.IsSizeToHeader))
				{
					DataGridLength dataGridLength = (DataGridLength)e.OldValue;
					double num;
					if (dataGridLength.UnitType != width.UnitType)
					{
						double constraintWidth = column.GetConstraintWidth(flag);
						if (!DoubleUtil.AreClose(uielement.DesiredSize.Width, constraintWidth))
						{
							uielement.InvalidateMeasure();
							uielement.Measure(new Size(constraintWidth, double.PositiveInfinity));
						}
						num = uielement.DesiredSize.Width;
					}
					else
					{
						num = dataGridLength.DesiredValue;
					}
					if (DoubleUtil.IsNaN(width.DesiredValue) || DoubleUtil.LessThan(width.DesiredValue, num))
					{
						column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, num, width.DisplayValue));
					}
				}
			}
		}

		// Token: 0x0600663A RID: 26170 RVA: 0x002B1064 File Offset: 0x002B0064
		public static Geometry GetFrozenClipForCell(IProvideDataGridColumn cell)
		{
			DataGridCellsPanel parentPanelForCell = DataGridHelper.GetParentPanelForCell(cell);
			if (parentPanelForCell != null)
			{
				return parentPanelForCell.GetFrozenClipForChild((UIElement)cell);
			}
			return null;
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x002B1089 File Offset: 0x002B0089
		public static DataGridCellsPanel GetParentPanelForCell(IProvideDataGridColumn cell)
		{
			return VisualTreeHelper.GetParent((UIElement)cell) as DataGridCellsPanel;
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x002B109C File Offset: 0x002B009C
		public static double GetParentCellsPanelHorizontalOffset(IProvideDataGridColumn cell)
		{
			DataGridCellsPanel parentPanelForCell = DataGridHelper.GetParentPanelForCell(cell);
			if (parentPanelForCell != null)
			{
				return parentPanelForCell.ComputeCellsPanelHorizontalOffset();
			}
			return 0.0;
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x002B10C4 File Offset: 0x002B00C4
		public static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
		}

		// Token: 0x0600663E RID: 26174 RVA: 0x002B10E3 File Offset: 0x002B00E3
		public static object GetCoercedTransferPropertyValue(DependencyObject baseObject, object baseValue, DependencyProperty baseProperty, DependencyObject parentObject, DependencyProperty parentProperty)
		{
			return DataGridHelper.GetCoercedTransferPropertyValue(baseObject, baseValue, baseProperty, parentObject, parentProperty, null, null);
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x002B10F4 File Offset: 0x002B00F4
		public static object GetCoercedTransferPropertyValue(DependencyObject baseObject, object baseValue, DependencyProperty baseProperty, DependencyObject parentObject, DependencyProperty parentProperty, DependencyObject grandParentObject, DependencyProperty grandParentProperty)
		{
			object result = baseValue;
			if (DataGridHelper.IsPropertyTransferEnabled(baseObject, baseProperty))
			{
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(baseObject, baseProperty).BaseValueSource;
				if (parentObject != null)
				{
					ValueSource valueSource = DependencyPropertyHelper.GetValueSource(parentObject, parentProperty);
					if (valueSource.BaseValueSource > baseValueSource)
					{
						result = parentObject.GetValue(parentProperty);
						baseValueSource = valueSource.BaseValueSource;
					}
				}
				if (grandParentObject != null)
				{
					ValueSource valueSource2 = DependencyPropertyHelper.GetValueSource(grandParentObject, grandParentProperty);
					if (valueSource2.BaseValueSource > baseValueSource)
					{
						result = grandParentObject.GetValue(grandParentProperty);
						baseValueSource = valueSource2.BaseValueSource;
					}
				}
			}
			return result;
		}

		// Token: 0x06006640 RID: 26176 RVA: 0x002B116F File Offset: 0x002B016F
		public static void TransferProperty(DependencyObject d, DependencyProperty p)
		{
			Dictionary<DependencyProperty, bool> propertyTransferEnabledMapForObject = DataGridHelper.GetPropertyTransferEnabledMapForObject(d);
			propertyTransferEnabledMapForObject[p] = true;
			d.CoerceValue(p);
			propertyTransferEnabledMapForObject[p] = false;
		}

		// Token: 0x06006641 RID: 26177 RVA: 0x002B1190 File Offset: 0x002B0190
		private static Dictionary<DependencyProperty, bool> GetPropertyTransferEnabledMapForObject(DependencyObject d)
		{
			Dictionary<DependencyProperty, bool> dictionary;
			if (!DataGridHelper._propertyTransferEnabledMap.TryGetValue(d, out dictionary))
			{
				dictionary = new Dictionary<DependencyProperty, bool>();
				DataGridHelper._propertyTransferEnabledMap.Add(d, dictionary);
			}
			return dictionary;
		}

		// Token: 0x06006642 RID: 26178 RVA: 0x002B11C0 File Offset: 0x002B01C0
		internal static bool IsPropertyTransferEnabled(DependencyObject d, DependencyProperty p)
		{
			Dictionary<DependencyProperty, bool> dictionary;
			bool flag;
			return DataGridHelper._propertyTransferEnabledMap.TryGetValue(d, out dictionary) && dictionary.TryGetValue(p, out flag) && flag;
		}

		// Token: 0x06006643 RID: 26179 RVA: 0x002B11EC File Offset: 0x002B01EC
		internal static bool IsOneWay(BindingBase bindingBase)
		{
			if (bindingBase == null)
			{
				return false;
			}
			Binding binding = bindingBase as Binding;
			if (binding != null)
			{
				return binding.Mode == BindingMode.OneWay;
			}
			MultiBinding multiBinding = bindingBase as MultiBinding;
			if (multiBinding != null)
			{
				return multiBinding.Mode == BindingMode.OneWay;
			}
			PriorityBinding priorityBinding = bindingBase as PriorityBinding;
			if (priorityBinding != null)
			{
				Collection<BindingBase> bindings = priorityBinding.Bindings;
				int count = bindings.Count;
				for (int i = 0; i < count; i++)
				{
					if (DataGridHelper.IsOneWay(bindings[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x002B1262 File Offset: 0x002B0262
		internal static BindingExpression GetBindingExpression(FrameworkElement element, DependencyProperty dp)
		{
			if (element != null)
			{
				return element.GetBindingExpression(dp);
			}
			return null;
		}

		// Token: 0x06006645 RID: 26181 RVA: 0x002B1270 File Offset: 0x002B0270
		internal static bool ValidateWithoutUpdate(FrameworkElement element)
		{
			bool flag = true;
			BindingGroup bindingGroup = element.BindingGroup;
			DataGridCell dataGridCell = (element != null) ? (element.Parent as DataGridCell) : null;
			if (bindingGroup != null && dataGridCell != null)
			{
				Collection<BindingExpressionBase> bindingExpressions = bindingGroup.BindingExpressions;
				BindingExpressionBase[] array = new BindingExpressionBase[bindingExpressions.Count];
				bindingExpressions.CopyTo(array, 0);
				foreach (BindingExpressionBase bindingExpressionBase in array)
				{
					if (DataGridHelper.BindingExpressionBelongsToElement<DataGridCell>(bindingExpressionBase, dataGridCell))
					{
						flag = (bindingExpressionBase.ValidateWithoutUpdate() && flag);
					}
				}
			}
			return flag;
		}

		// Token: 0x06006646 RID: 26182 RVA: 0x002B12E4 File Offset: 0x002B02E4
		internal static bool BindingExpressionBelongsToElement<T>(BindingExpressionBase beb, T element) where T : FrameworkElement
		{
			DependencyObject targetElement = beb.TargetElement;
			if (targetElement != null)
			{
				DependencyObject dependencyObject = DataGridHelper.FindContextElement(beb);
				if (dependencyObject == null)
				{
					dependencyObject = targetElement;
				}
				if (dependencyObject is Visual || dependencyObject is Visual3D)
				{
					return VisualTreeHelper.IsAncestorOf(element, dependencyObject, typeof(T));
				}
			}
			return false;
		}

		// Token: 0x06006647 RID: 26183 RVA: 0x002B1330 File Offset: 0x002B0330
		private static DependencyObject FindContextElement(BindingExpressionBase beb)
		{
			BindingExpression bindingExpression;
			if ((bindingExpression = (beb as BindingExpression)) != null)
			{
				return bindingExpression.ContextElement;
			}
			ReadOnlyCollection<BindingExpressionBase> readOnlyCollection = null;
			MultiBindingExpression multiBindingExpression;
			PriorityBindingExpression priorityBindingExpression;
			if ((multiBindingExpression = (beb as MultiBindingExpression)) != null)
			{
				readOnlyCollection = multiBindingExpression.BindingExpressions;
			}
			else if ((priorityBindingExpression = (beb as PriorityBindingExpression)) != null)
			{
				readOnlyCollection = priorityBindingExpression.BindingExpressions;
			}
			if (readOnlyCollection != null)
			{
				foreach (BindingExpressionBase beb2 in readOnlyCollection)
				{
					DependencyObject dependencyObject = DataGridHelper.FindContextElement(beb2);
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
				}
			}
			return null;
		}

		// Token: 0x06006648 RID: 26184 RVA: 0x002B13C4 File Offset: 0x002B03C4
		internal static void CacheFlowDirection(FrameworkElement element, DataGridCell cell)
		{
			if (element != null && cell != null)
			{
				object obj = element.ReadLocalValue(FrameworkElement.FlowDirectionProperty);
				if (obj != DependencyProperty.UnsetValue)
				{
					cell.SetValue(DataGridHelper.FlowDirectionCacheProperty, obj);
				}
			}
		}

		// Token: 0x06006649 RID: 26185 RVA: 0x002B13F8 File Offset: 0x002B03F8
		internal static void RestoreFlowDirection(FrameworkElement element, DataGridCell cell)
		{
			if (element != null && cell != null)
			{
				object obj = cell.ReadLocalValue(DataGridHelper.FlowDirectionCacheProperty);
				if (obj != DependencyProperty.UnsetValue)
				{
					element.SetValue(FrameworkElement.FlowDirectionProperty, obj);
				}
			}
		}

		// Token: 0x0600664A RID: 26186 RVA: 0x002B142C File Offset: 0x002B042C
		internal static void UpdateTarget(FrameworkElement element)
		{
			BindingGroup bindingGroup = element.BindingGroup;
			DataGridCell dataGridCell = (element != null) ? (element.Parent as DataGridCell) : null;
			if (bindingGroup != null && dataGridCell != null)
			{
				Collection<BindingExpressionBase> bindingExpressions = bindingGroup.BindingExpressions;
				BindingExpressionBase[] array = new BindingExpressionBase[bindingExpressions.Count];
				bindingExpressions.CopyTo(array, 0);
				foreach (BindingExpressionBase bindingExpressionBase in array)
				{
					DependencyObject targetElement = bindingExpressionBase.TargetElement;
					if (targetElement != null && VisualTreeHelper.IsAncestorOf(dataGridCell, targetElement, typeof(DataGridCell)))
					{
						bindingExpressionBase.UpdateTarget();
					}
				}
			}
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x002B14AC File Offset: 0x002B04AC
		internal static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
		{
			if (DataGridHelper.IsDefaultValue(column, columnProperty))
			{
				content.ClearValue(contentProperty);
				return;
			}
			content.SetValue(contentProperty, column.GetValue(columnProperty));
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x002B14CD File Offset: 0x002B04CD
		internal static string GetPathFromBinding(Binding binding)
		{
			if (binding != null)
			{
				if (!string.IsNullOrEmpty(binding.XPath))
				{
					return binding.XPath;
				}
				if (binding.Path != null)
				{
					return binding.Path.Path;
				}
			}
			return null;
		}

		// Token: 0x0600664D RID: 26189 RVA: 0x002A5DC0 File Offset: 0x002A4DC0
		public static bool AreRowHeadersVisible(DataGridHeadersVisibility headersVisibility)
		{
			return (headersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row;
		}

		// Token: 0x0600664E RID: 26190 RVA: 0x002B14FB File Offset: 0x002B04FB
		public static double CoerceToMinMax(double value, double minValue, double maxValue)
		{
			value = Math.Max(value, minValue);
			value = Math.Min(value, maxValue);
			return value;
		}

		// Token: 0x0600664F RID: 26191 RVA: 0x002B1510 File Offset: 0x002B0510
		public static bool HasNonEscapeCharacters(TextCompositionEventArgs textArgs)
		{
			if (textArgs != null)
			{
				string text = textArgs.Text;
				int i = 0;
				int length = text.Length;
				while (i < length)
				{
					if (text[i] != '\u001b')
					{
						return true;
					}
					i++;
				}
			}
			return false;
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x002B1548 File Offset: 0x002B0548
		public static bool IsImeProcessed(KeyEventArgs keyArgs)
		{
			return keyArgs != null && keyArgs.Key == Key.ImeProcessed;
		}

		// Token: 0x040033BA RID: 13242
		private static ConditionalWeakTable<DependencyObject, Dictionary<DependencyProperty, bool>> _propertyTransferEnabledMap = new ConditionalWeakTable<DependencyObject, Dictionary<DependencyProperty, bool>>();

		// Token: 0x040033BB RID: 13243
		private static readonly DependencyProperty FlowDirectionCacheProperty = DependencyProperty.Register("FlowDirectionCache", typeof(FlowDirection), typeof(DataGridHelper));

		// Token: 0x040033BC RID: 13244
		private const char _escapeChar = '\u001b';
	}
}
