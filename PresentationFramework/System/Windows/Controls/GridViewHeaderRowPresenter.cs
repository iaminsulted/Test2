using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200078B RID: 1931
	[StyleTypedProperty(Property = "ColumnHeaderContainerStyle", StyleTargetType = typeof(GridViewColumnHeader))]
	public class GridViewHeaderRowPresenter : GridViewRowPresenterBase
	{
		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x06006AF4 RID: 27380 RVA: 0x002C3580 File Offset: 0x002C2580
		// (set) Token: 0x06006AF5 RID: 27381 RVA: 0x002C3592 File Offset: 0x002C2592
		public Style ColumnHeaderContainerStyle
		{
			get
			{
				return (Style)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty, value);
			}
		}

		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x002C35A0 File Offset: 0x002C25A0
		// (set) Token: 0x06006AF7 RID: 27383 RVA: 0x002C35B2 File Offset: 0x002C25B2
		public DataTemplate ColumnHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty, value);
			}
		}

		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x06006AF8 RID: 27384 RVA: 0x002C35C0 File Offset: 0x002C25C0
		// (set) Token: 0x06006AF9 RID: 27385 RVA: 0x002C35D2 File Offset: 0x002C25D2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector ColumnHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x170018BB RID: 6331
		// (get) Token: 0x06006AFA RID: 27386 RVA: 0x002C35E0 File Offset: 0x002C25E0
		// (set) Token: 0x06006AFB RID: 27387 RVA: 0x002C35F2 File Offset: 0x002C25F2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ColumnHeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty, value);
			}
		}

		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x06006AFC RID: 27388 RVA: 0x002C3600 File Offset: 0x002C2600
		// (set) Token: 0x06006AFD RID: 27389 RVA: 0x002C3612 File Offset: 0x002C2612
		public bool AllowsColumnReorder
		{
			get
			{
				return (bool)base.GetValue(GridViewHeaderRowPresenter.AllowsColumnReorderProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.AllowsColumnReorderProperty, value);
			}
		}

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x06006AFE RID: 27390 RVA: 0x002C3620 File Offset: 0x002C2620
		// (set) Token: 0x06006AFF RID: 27391 RVA: 0x002C3632 File Offset: 0x002C2632
		public ContextMenu ColumnHeaderContextMenu
		{
			get
			{
				return (ContextMenu)base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty, value);
			}
		}

		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x06006B00 RID: 27392 RVA: 0x002C3640 File Offset: 0x002C2640
		// (set) Token: 0x06006B01 RID: 27393 RVA: 0x002C364D File Offset: 0x002C264D
		public object ColumnHeaderToolTip
		{
			get
			{
				return base.GetValue(GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty);
			}
			set
			{
				base.SetValue(GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty, value);
			}
		}

		// Token: 0x06006B02 RID: 27394 RVA: 0x002C365C File Offset: 0x002C265C
		private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewHeaderRowPresenter gridViewHeaderRowPresenter = (GridViewHeaderRowPresenter)d;
			if (e.Property == GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty || e.Property == GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty)
			{
				Helper.CheckTemplateAndTemplateSelector("GridViewHeaderRowPresenter", GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty, GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty, gridViewHeaderRowPresenter);
			}
			gridViewHeaderRowPresenter.UpdateAllHeaders(e.Property);
		}

		// Token: 0x06006B03 RID: 27395 RVA: 0x002C36B0 File Offset: 0x002C26B0
		protected override Size MeasureOverride(Size constraint)
		{
			GridViewColumnCollection columns = base.Columns;
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = 0.0;
			double height = constraint.Height;
			bool flag = false;
			if (columns != null)
			{
				for (int i = 0; i < columns.Count; i++)
				{
					UIElement uielement = internalChildren[this.GetVisualIndex(i)];
					if (uielement != null)
					{
						double num3 = Math.Max(0.0, constraint.Width - num2);
						GridViewColumn gridViewColumn = columns[i];
						if (gridViewColumn.State == ColumnMeasureState.Init)
						{
							if (!flag)
							{
								base.EnsureDesiredWidthList();
								base.LayoutUpdated += this.OnLayoutUpdated;
								flag = true;
							}
							uielement.Measure(new Size(num3, height));
							base.DesiredWidthList[gridViewColumn.ActualIndex] = gridViewColumn.EnsureWidth(uielement.DesiredSize.Width);
							num2 += gridViewColumn.DesiredWidth;
						}
						else if (gridViewColumn.State == ColumnMeasureState.Headered || gridViewColumn.State == ColumnMeasureState.Data)
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
			}
			this._paddingHeader.Measure(new Size(0.0, height));
			num = Math.Max(num, this._paddingHeader.DesiredSize.Height);
			num2 += 2.0;
			if (this._isHeaderDragging)
			{
				this._indicator.Measure(constraint);
				this._floatingHeader.Measure(constraint);
			}
			return new Size(num2, num);
		}

		// Token: 0x06006B04 RID: 27396 RVA: 0x002C38A0 File Offset: 0x002C28A0
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			GridViewColumnCollection columns = base.Columns;
			UIElementCollection internalChildren = base.InternalChildren;
			double num = 0.0;
			double num2 = arrangeSize.Width;
			this.HeadersPositionList.Clear();
			Rect rect;
			if (columns != null)
			{
				for (int i = 0; i < columns.Count; i++)
				{
					UIElement uielement = internalChildren[this.GetVisualIndex(i)];
					if (uielement != null)
					{
						GridViewColumn gridViewColumn = columns[i];
						double num3 = Math.Min(num2, (gridViewColumn.State == ColumnMeasureState.SpecificWidth) ? gridViewColumn.Width : gridViewColumn.DesiredWidth);
						rect = new Rect(num, 0.0, num3, arrangeSize.Height);
						uielement.Arrange(rect);
						this.HeadersPositionList.Add(rect);
						num2 -= num3;
						num += num3;
					}
				}
				if (this._isColumnChangedOrCreated)
				{
					for (int j = 0; j < columns.Count; j++)
					{
						(internalChildren[this.GetVisualIndex(j)] as GridViewColumnHeader).CheckWidthForPreviousHeaderGripper();
					}
					this._paddingHeader.CheckWidthForPreviousHeaderGripper();
					this._isColumnChangedOrCreated = false;
				}
			}
			rect = new Rect(num, 0.0, Math.Max(num2, 0.0), arrangeSize.Height);
			this._paddingHeader.Arrange(rect);
			this.HeadersPositionList.Add(rect);
			if (this._isHeaderDragging)
			{
				this._floatingHeader.Arrange(new Rect(new Point(this._currentPos.X - this._relativeStartPos.X, 0.0), this.HeadersPositionList[this._startColumnIndex].Size));
				Point location = this.FindPositionByIndex(this._desColumnIndex);
				this._indicator.Arrange(new Rect(location, new Size(this._indicator.DesiredSize.Width, arrangeSize.Height)));
			}
			return arrangeSize;
		}

		// Token: 0x06006B05 RID: 27397 RVA: 0x002C3A98 File Offset: 0x002C2A98
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			GridViewColumnHeader gridViewColumnHeader = e.Source as GridViewColumnHeader;
			if (gridViewColumnHeader != null && this.AllowsColumnReorder)
			{
				this.PrepareHeaderDrag(gridViewColumnHeader, e.GetPosition(this), e.GetPosition(gridViewColumnHeader), false);
				this.MakeParentItemsControlGotFocus();
			}
			e.Handled = true;
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x002C3AE6 File Offset: 0x002C2AE6
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			this._prepareDragging = false;
			if (this._isHeaderDragging)
			{
				this.FinishHeaderDrag(false);
			}
			e.Handled = true;
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x002C3B0C File Offset: 0x002C2B0C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.LeftButton == MouseButtonState.Pressed && this._prepareDragging)
			{
				this._currentPos = e.GetPosition(this);
				this._desColumnIndex = this.FindIndexByPosition(this._currentPos, true);
				if (!this._isHeaderDragging)
				{
					if (this.CheckStartHeaderDrag(this._currentPos, this._startPos))
					{
						this.StartHeaderDrag();
						base.InvalidateMeasure();
					}
				}
				else
				{
					bool flag = GridViewHeaderRowPresenter.IsMousePositionValid(this._floatingHeader, this._currentPos, 2.0);
					this._indicator.Visibility = (this._floatingHeader.Visibility = (flag ? Visibility.Visible : Visibility.Hidden));
					base.InvalidateArrange();
				}
			}
			e.Handled = true;
		}

		// Token: 0x06006B08 RID: 27400 RVA: 0x002C3BC9 File Offset: 0x002C2BC9
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (e.LeftButton == MouseButtonState.Pressed && this._isHeaderDragging)
			{
				this.FinishHeaderDrag(true);
			}
			this._prepareDragging = false;
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x002C3BF4 File Offset: 0x002C2BF4
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.NeedUpdateVisualTree)
			{
				UIElementCollection internalChildren = base.InternalChildren;
				GridViewColumnCollection columns = base.Columns;
				this.RenewEvents();
				if (internalChildren.Count == 0)
				{
					this.AddPaddingColumnHeader();
					this.AddIndicator();
					this.AddFloatingHeader(null);
				}
				else if (internalChildren.Count > 3)
				{
					int num = internalChildren.Count - 3;
					for (int i = 0; i < num; i++)
					{
						this.RemoveHeader(null, 1);
					}
				}
				this.UpdatePaddingHeader(this._paddingHeader);
				if (columns != null)
				{
					int num2 = 1;
					for (int j = columns.Count - 1; j >= 0; j--)
					{
						GridViewColumn column = columns[j];
						this.CreateAndInsertHeader(column, num2++);
					}
				}
				this.BuildHeaderLinks();
				base.NeedUpdateVisualTree = false;
				this._isColumnChangedOrCreated = true;
			}
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x002C3CC0 File Offset: 0x002C2CC0
		internal override void OnColumnPropertyChanged(GridViewColumn column, string propertyName)
		{
			if (column.ActualIndex >= 0)
			{
				GridViewColumnHeader gridViewColumnHeader = this.FindHeaderByColumn(column);
				if (gridViewColumnHeader != null)
				{
					if (GridViewColumn.WidthProperty.Name.Equals(propertyName) || "ActualWidth".Equals(propertyName))
					{
						base.InvalidateMeasure();
						return;
					}
					if (GridViewColumn.HeaderProperty.Name.Equals(propertyName))
					{
						if (!gridViewColumnHeader.IsInternalGenerated || column.Header is GridViewColumnHeader)
						{
							int index = base.InternalChildren.IndexOf(gridViewColumnHeader);
							this.RemoveHeader(gridViewColumnHeader, -1);
							this.CreateAndInsertHeader(column, index);
							this.BuildHeaderLinks();
							return;
						}
						this.UpdateHeaderContent(gridViewColumnHeader);
						return;
					}
					else
					{
						DependencyProperty columnDPFromName = GridViewHeaderRowPresenter.GetColumnDPFromName(propertyName);
						if (columnDPFromName != null)
						{
							this.UpdateHeaderProperty(gridViewColumnHeader, columnDPFromName);
						}
					}
				}
			}
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x002C3D74 File Offset: 0x002C2D74
		internal override void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
		{
			base.OnColumnCollectionChanged(e);
			UIElementCollection internalChildren = base.InternalChildren;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				int visualIndex = this.GetVisualIndex(e.NewStartingIndex);
				GridViewColumn column = (GridViewColumn)e.NewItems[0];
				this.CreateAndInsertHeader(column, visualIndex + 1);
				break;
			}
			case NotifyCollectionChangedAction.Remove:
				this.RemoveHeader(null, this.GetVisualIndex(e.OldStartingIndex));
				break;
			case NotifyCollectionChangedAction.Replace:
			{
				int visualIndex = this.GetVisualIndex(e.OldStartingIndex);
				this.RemoveHeader(null, visualIndex);
				GridViewColumn column = (GridViewColumn)e.NewItems[0];
				this.CreateAndInsertHeader(column, visualIndex);
				break;
			}
			case NotifyCollectionChangedAction.Move:
			{
				int visualIndex2 = this.GetVisualIndex(e.OldStartingIndex);
				int visualIndex3 = this.GetVisualIndex(e.NewStartingIndex);
				GridViewColumnHeader element = (GridViewColumnHeader)internalChildren[visualIndex2];
				internalChildren.RemoveAt(visualIndex2);
				internalChildren.InsertInternal(visualIndex3, element);
				break;
			}
			case NotifyCollectionChangedAction.Reset:
			{
				int count = e.ClearedColumns.Count;
				for (int i = 0; i < count; i++)
				{
					this.RemoveHeader(null, 1);
				}
				break;
			}
			}
			this.BuildHeaderLinks();
			this._isColumnChangedOrCreated = true;
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x002C3EA0 File Offset: 0x002C2EA0
		internal void MakeParentItemsControlGotFocus()
		{
			if (this._itemsControl != null && !this._itemsControl.IsKeyboardFocusWithin)
			{
				ListBox listBox = this._itemsControl as ListBox;
				if (listBox != null && listBox.LastActionItem != null)
				{
					listBox.LastActionItem.Focus();
					return;
				}
				this._itemsControl.Focus();
			}
		}

		// Token: 0x06006B0D RID: 27405 RVA: 0x002C3EF4 File Offset: 0x002C2EF4
		internal void UpdateHeaderProperty(GridViewColumnHeader header, DependencyProperty property)
		{
			DependencyProperty gvDP;
			DependencyProperty columnDP;
			DependencyProperty targetDP;
			GridViewHeaderRowPresenter.GetMatchingDPs(property, out gvDP, out columnDP, out targetDP);
			this.UpdateHeaderProperty(header, targetDP, columnDP, gvDP);
		}

		// Token: 0x06006B0E RID: 27406 RVA: 0x002C3F17 File Offset: 0x002C2F17
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GridViewHeaderRowPresenterAutomationPeer(this);
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x002C3F20 File Offset: 0x002C2F20
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
						if (gridViewColumn.State == ColumnMeasureState.Init)
						{
							gridViewColumn.State = ColumnMeasureState.Headered;
						}
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

		// Token: 0x06006B10 RID: 27408 RVA: 0x002C3FFC File Offset: 0x002C2FFC
		private int GetVisualIndex(int columnIndex)
		{
			return base.InternalChildren.Count - 3 - columnIndex;
		}

		// Token: 0x06006B11 RID: 27409 RVA: 0x002C4010 File Offset: 0x002C3010
		private void BuildHeaderLinks()
		{
			GridViewColumnHeader previousVisualHeader = null;
			if (base.Columns != null)
			{
				for (int i = 0; i < base.Columns.Count; i++)
				{
					GridViewColumnHeader gridViewColumnHeader = (GridViewColumnHeader)base.InternalChildren[this.GetVisualIndex(i)];
					gridViewColumnHeader.PreviousVisualHeader = previousVisualHeader;
					previousVisualHeader = gridViewColumnHeader;
				}
			}
			if (this._paddingHeader != null)
			{
				this._paddingHeader.PreviousVisualHeader = previousVisualHeader;
			}
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x002C4070 File Offset: 0x002C3070
		private GridViewColumnHeader CreateAndInsertHeader(GridViewColumn column, int index)
		{
			object header = column.Header;
			GridViewColumnHeader gridViewColumnHeader = header as GridViewColumnHeader;
			if (header != null)
			{
				DependencyObject dependencyObject = header as DependencyObject;
				if (dependencyObject != null)
				{
					Visual visual = dependencyObject as Visual;
					if (visual != null)
					{
						Visual visual2 = VisualTreeHelper.GetParent(visual) as Visual;
						if (visual2 != null)
						{
							if (gridViewColumnHeader != null)
							{
								GridViewHeaderRowPresenter gridViewHeaderRowPresenter = visual2 as GridViewHeaderRowPresenter;
								if (gridViewHeaderRowPresenter != null)
								{
									gridViewHeaderRowPresenter.InternalChildren.RemoveNoVerify(gridViewColumnHeader);
								}
							}
							else
							{
								GridViewColumnHeader gridViewColumnHeader2 = visual2 as GridViewColumnHeader;
								if (gridViewColumnHeader2 != null)
								{
									gridViewColumnHeader2.ClearValue(ContentControl.ContentProperty);
								}
							}
						}
					}
					DependencyObject parent = LogicalTreeHelper.GetParent(dependencyObject);
					if (parent != null)
					{
						LogicalTreeHelper.RemoveLogicalChild(parent, header);
					}
				}
			}
			if (gridViewColumnHeader == null)
			{
				gridViewColumnHeader = new GridViewColumnHeader();
				gridViewColumnHeader.IsInternalGenerated = true;
			}
			gridViewColumnHeader.SetValue(GridViewColumnHeader.ColumnPropertyKey, column);
			this.HookupItemsControlKeyboardEvent(gridViewColumnHeader);
			base.InternalChildren.InsertInternal(index, gridViewColumnHeader);
			this.UpdateHeader(gridViewColumnHeader);
			this._gvHeadersValid = false;
			return gridViewColumnHeader;
		}

		// Token: 0x06006B13 RID: 27411 RVA: 0x002C413E File Offset: 0x002C313E
		private void RemoveHeader(GridViewColumnHeader header, int index)
		{
			this._gvHeadersValid = false;
			if (header != null)
			{
				base.InternalChildren.Remove(header);
			}
			else
			{
				header = (GridViewColumnHeader)base.InternalChildren[index];
				base.InternalChildren.RemoveAt(index);
			}
			this.UnhookItemsControlKeyboardEvent(header);
		}

		// Token: 0x06006B14 RID: 27412 RVA: 0x002C4180 File Offset: 0x002C3180
		private void RenewEvents()
		{
			ScrollViewer headerSV = this._headerSV;
			this._headerSV = (base.Parent as ScrollViewer);
			if (headerSV != this._headerSV)
			{
				if (headerSV != null)
				{
					headerSV.ScrollChanged -= this.OnHeaderScrollChanged;
				}
				if (this._headerSV != null)
				{
					this._headerSV.ScrollChanged += this.OnHeaderScrollChanged;
				}
			}
			ScrollViewer mainSV = this._mainSV;
			this._mainSV = (base.TemplatedParent as ScrollViewer);
			if (mainSV != this._mainSV)
			{
				if (mainSV != null)
				{
					mainSV.ScrollChanged -= this.OnMasterScrollChanged;
				}
				if (this._mainSV != null)
				{
					this._mainSV.ScrollChanged += this.OnMasterScrollChanged;
				}
			}
			ItemsControl itemsControl = this._itemsControl;
			this._itemsControl = GridViewHeaderRowPresenter.FindItemsControlThroughTemplatedParent(this);
			if (itemsControl != this._itemsControl)
			{
				if (itemsControl != null)
				{
					itemsControl.KeyDown -= this.OnColumnHeadersPresenterKeyDown;
				}
				if (this._itemsControl != null)
				{
					this._itemsControl.KeyDown += this.OnColumnHeadersPresenterKeyDown;
				}
			}
			ListView listView = this._itemsControl as ListView;
			if (listView != null && listView.View != null && listView.View is GridView)
			{
				((GridView)listView.View).HeaderRowPresenter = this;
			}
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x002C42BC File Offset: 0x002C32BC
		private void UnhookItemsControlKeyboardEvent(GridViewColumnHeader header)
		{
			if (this._itemsControl != null)
			{
				this._itemsControl.KeyDown -= header.OnColumnHeaderKeyDown;
			}
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x002C42DD File Offset: 0x002C32DD
		private void HookupItemsControlKeyboardEvent(GridViewColumnHeader header)
		{
			if (this._itemsControl != null)
			{
				this._itemsControl.KeyDown += header.OnColumnHeaderKeyDown;
			}
		}

		// Token: 0x06006B17 RID: 27415 RVA: 0x002C42FE File Offset: 0x002C32FE
		private void OnMasterScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (this._headerSV != null && this._mainSV == e.OriginalSource)
			{
				this._headerSV.ScrollToHorizontalOffset(e.HorizontalOffset);
			}
		}

		// Token: 0x06006B18 RID: 27416 RVA: 0x002C4327 File Offset: 0x002C3327
		private void OnHeaderScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (this._mainSV != null && this._headerSV == e.OriginalSource)
			{
				this._mainSV.ScrollToHorizontalOffset(e.HorizontalOffset);
			}
		}

		// Token: 0x06006B19 RID: 27417 RVA: 0x002C4350 File Offset: 0x002C3350
		private void AddPaddingColumnHeader()
		{
			GridViewColumnHeader gridViewColumnHeader = new GridViewColumnHeader();
			gridViewColumnHeader.IsInternalGenerated = true;
			gridViewColumnHeader.SetValue(GridViewColumnHeader.RolePropertyKey, GridViewColumnHeaderRole.Padding);
			gridViewColumnHeader.Content = null;
			gridViewColumnHeader.ContentTemplate = null;
			gridViewColumnHeader.ContentTemplateSelector = null;
			gridViewColumnHeader.MinWidth = 0.0;
			gridViewColumnHeader.Padding = new Thickness(0.0);
			gridViewColumnHeader.Width = double.NaN;
			gridViewColumnHeader.HorizontalAlignment = HorizontalAlignment.Stretch;
			if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				gridViewColumnHeader.Focusable = false;
			}
			base.InternalChildren.AddInternal(gridViewColumnHeader);
			this._paddingHeader = gridViewColumnHeader;
		}

		// Token: 0x06006B1A RID: 27418 RVA: 0x002C43EC File Offset: 0x002C33EC
		private void AddIndicator()
		{
			Separator separator = new Separator();
			separator.Visibility = Visibility.Hidden;
			separator.Margin = new Thickness(0.0);
			separator.Width = 2.0;
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Border));
			frameworkElementFactory.SetValue(Border.BackgroundProperty, new SolidColorBrush(Color.FromUInt32(4278190208U)));
			ControlTemplate controlTemplate = new ControlTemplate(typeof(Separator));
			controlTemplate.VisualTree = frameworkElementFactory;
			controlTemplate.Seal();
			separator.Template = controlTemplate;
			base.InternalChildren.AddInternal(separator);
			this._indicator = separator;
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x002C448C File Offset: 0x002C348C
		private void AddFloatingHeader(GridViewColumnHeader srcHeader)
		{
			Type type = (srcHeader != null) ? srcHeader.GetType() : typeof(GridViewColumnHeader);
			GridViewColumnHeader gridViewColumnHeader;
			try
			{
				gridViewColumnHeader = (Activator.CreateInstance(type) as GridViewColumnHeader);
			}
			catch (MissingMethodException innerException)
			{
				throw new ArgumentException(SR.Get("ListView_MissingParameterlessConstructor", new object[]
				{
					type
				}), innerException);
			}
			gridViewColumnHeader.IsInternalGenerated = true;
			gridViewColumnHeader.SetValue(GridViewColumnHeader.RolePropertyKey, GridViewColumnHeaderRole.Floating);
			gridViewColumnHeader.Visibility = Visibility.Hidden;
			base.InternalChildren.AddInternal(gridViewColumnHeader);
			this._floatingHeader = gridViewColumnHeader;
		}

		// Token: 0x06006B1C RID: 27420 RVA: 0x002C451C File Offset: 0x002C351C
		private void UpdateFloatingHeader(GridViewColumnHeader srcHeader)
		{
			this._floatingHeader.Style = srcHeader.Style;
			this._floatingHeader.FloatSourceHeader = srcHeader;
			this._floatingHeader.Width = srcHeader.ActualWidth;
			this._floatingHeader.Height = srcHeader.ActualHeight;
			this._floatingHeader.SetValue(GridViewColumnHeader.ColumnPropertyKey, srcHeader.Column);
			this._floatingHeader.Visibility = Visibility.Hidden;
			this._floatingHeader.MinWidth = srcHeader.MinWidth;
			this._floatingHeader.MinHeight = srcHeader.MinHeight;
			object obj = srcHeader.ReadLocalValue(ContentControl.ContentTemplateProperty);
			if (obj != DependencyProperty.UnsetValue && obj != null)
			{
				this._floatingHeader.ContentTemplate = srcHeader.ContentTemplate;
			}
			object obj2 = srcHeader.ReadLocalValue(ContentControl.ContentTemplateSelectorProperty);
			if (obj2 != DependencyProperty.UnsetValue && obj2 != null)
			{
				this._floatingHeader.ContentTemplateSelector = srcHeader.ContentTemplateSelector;
			}
			if (!(srcHeader.Content is Visual))
			{
				this._floatingHeader.Content = srcHeader.Content;
			}
		}

		// Token: 0x06006B1D RID: 27421 RVA: 0x002C461A File Offset: 0x002C361A
		private bool CheckStartHeaderDrag(Point currentPos, Point originalPos)
		{
			return DoubleUtil.GreaterThan(Math.Abs(currentPos.X - originalPos.X), 4.0);
		}

		// Token: 0x06006B1E RID: 27422 RVA: 0x002C4640 File Offset: 0x002C3640
		private static ItemsControl FindItemsControlThroughTemplatedParent(GridViewHeaderRowPresenter presenter)
		{
			FrameworkElement frameworkElement = presenter.TemplatedParent as FrameworkElement;
			ItemsControl itemsControl = null;
			while (frameworkElement != null)
			{
				itemsControl = (frameworkElement as ItemsControl);
				if (itemsControl != null)
				{
					break;
				}
				frameworkElement = (frameworkElement.TemplatedParent as FrameworkElement);
			}
			return itemsControl;
		}

		// Token: 0x06006B1F RID: 27423 RVA: 0x002C4678 File Offset: 0x002C3678
		private void OnColumnHeadersPresenterKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && this._isHeaderDragging)
			{
				GridViewColumnHeader draggingSrcHeader = this._draggingSrcHeader;
				this.FinishHeaderDrag(true);
				this.PrepareHeaderDrag(draggingSrcHeader, this._currentPos, this._relativeStartPos, true);
				base.InvalidateArrange();
			}
		}

		// Token: 0x06006B20 RID: 27424 RVA: 0x002C46C0 File Offset: 0x002C36C0
		private GridViewColumnHeader FindHeaderByColumn(GridViewColumn column)
		{
			GridViewColumnCollection columns = base.Columns;
			UIElementCollection internalChildren = base.InternalChildren;
			if (columns != null && internalChildren.Count > columns.Count)
			{
				int num = columns.IndexOf(column);
				if (num != -1)
				{
					int visualIndex = this.GetVisualIndex(num);
					GridViewColumnHeader gridViewColumnHeader = internalChildren[visualIndex] as GridViewColumnHeader;
					if (gridViewColumnHeader.Column == column)
					{
						return gridViewColumnHeader;
					}
					for (int i = 1; i < internalChildren.Count; i++)
					{
						gridViewColumnHeader = (internalChildren[i] as GridViewColumnHeader);
						if (gridViewColumnHeader != null && gridViewColumnHeader.Column == column)
						{
							return gridViewColumnHeader;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06006B21 RID: 27425 RVA: 0x002C4754 File Offset: 0x002C3754
		private int FindIndexByPosition(Point startPos, bool findNearestColumn)
		{
			int num = -1;
			if (startPos.X < 0.0)
			{
				return 0;
			}
			int i = 0;
			while (i < this.HeadersPositionList.Count)
			{
				num++;
				Rect rect = this.HeadersPositionList[i];
				double x = rect.X;
				double num2 = x + rect.Width;
				if (DoubleUtil.GreaterThanOrClose(startPos.X, x) && DoubleUtil.LessThanOrClose(startPos.X, num2))
				{
					if (!findNearestColumn)
					{
						break;
					}
					double value = (x + num2) * 0.5;
					if (DoubleUtil.GreaterThanOrClose(startPos.X, value) && i != this.HeadersPositionList.Count - 1)
					{
						num++;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			return num;
		}

		// Token: 0x06006B22 RID: 27426 RVA: 0x002C4810 File Offset: 0x002C3810
		private Point FindPositionByIndex(int index)
		{
			return new Point(this.HeadersPositionList[index].X, 0.0);
		}

		// Token: 0x06006B23 RID: 27427 RVA: 0x002C4840 File Offset: 0x002C3840
		private void UpdateHeader(GridViewColumnHeader header)
		{
			this.UpdateHeaderContent(header);
			int i = 0;
			int num = GridViewHeaderRowPresenter.s_DPList[0].Length;
			while (i < num)
			{
				this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.s_DPList[2][i], GridViewHeaderRowPresenter.s_DPList[1][i], GridViewHeaderRowPresenter.s_DPList[0][i]);
				i++;
			}
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x002C488C File Offset: 0x002C388C
		private void UpdateHeaderContent(GridViewColumnHeader header)
		{
			if (header != null && header.IsInternalGenerated)
			{
				GridViewColumn column = header.Column;
				if (column != null)
				{
					if (column.Header == null)
					{
						header.ClearValue(ContentControl.ContentProperty);
						return;
					}
					header.Content = column.Header;
				}
			}
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x002C48CE File Offset: 0x002C38CE
		private void UpdatePaddingHeader(GridViewColumnHeader header)
		{
			this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty);
			this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty);
			this.UpdateHeaderProperty(header, GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty);
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x002C48F4 File Offset: 0x002C38F4
		private void UpdateAllHeaders(DependencyProperty dp)
		{
			DependencyProperty gvDP;
			DependencyProperty columnDP;
			DependencyProperty targetDP;
			GridViewHeaderRowPresenter.GetMatchingDPs(dp, out gvDP, out columnDP, out targetDP);
			int num;
			int num2;
			this.GetIndexRange(dp, out num, out num2);
			UIElementCollection internalChildren = base.InternalChildren;
			for (int i = num; i <= num2; i++)
			{
				GridViewColumnHeader gridViewColumnHeader = internalChildren[i] as GridViewColumnHeader;
				if (gridViewColumnHeader != null)
				{
					this.UpdateHeaderProperty(gridViewColumnHeader, targetDP, columnDP, gvDP);
				}
			}
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x002C4950 File Offset: 0x002C3950
		private void GetIndexRange(DependencyProperty dp, out int iStart, out int iEnd)
		{
			iStart = ((dp == GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty || dp == GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty || dp == GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty) ? 1 : 0);
			iEnd = base.InternalChildren.Count - 3;
		}

		// Token: 0x06006B28 RID: 27432 RVA: 0x002C4980 File Offset: 0x002C3980
		private void UpdateHeaderProperty(GridViewColumnHeader header, DependencyProperty targetDP, DependencyProperty columnDP, DependencyProperty gvDP)
		{
			if (gvDP == GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty && header.Role == GridViewColumnHeaderRole.Padding)
			{
				Style columnHeaderContainerStyle = this.ColumnHeaderContainerStyle;
				if (columnHeaderContainerStyle != null && !columnHeaderContainerStyle.TargetType.IsAssignableFrom(typeof(GridViewColumnHeader)))
				{
					header.Style = null;
					return;
				}
			}
			GridViewColumn column = header.Column;
			object obj = null;
			if (column != null && columnDP != null)
			{
				obj = column.GetValue(columnDP);
			}
			if (obj == null)
			{
				obj = base.GetValue(gvDP);
			}
			header.UpdateProperty(targetDP, obj);
		}

		// Token: 0x06006B29 RID: 27433 RVA: 0x002C49F4 File Offset: 0x002C39F4
		private void PrepareHeaderDrag(GridViewColumnHeader header, Point pos, Point relativePos, bool cancelInvoke)
		{
			if (header.Role == GridViewColumnHeaderRole.Normal)
			{
				this._prepareDragging = true;
				this._isHeaderDragging = false;
				this._draggingSrcHeader = header;
				this._startPos = pos;
				this._relativeStartPos = relativePos;
				if (!cancelInvoke)
				{
					this._startColumnIndex = this.FindIndexByPosition(this._startPos, false);
				}
			}
		}

		// Token: 0x06006B2A RID: 27434 RVA: 0x002C4A44 File Offset: 0x002C3A44
		private void StartHeaderDrag()
		{
			this._startPos = this._currentPos;
			this._isHeaderDragging = true;
			this._draggingSrcHeader.SuppressClickEvent = true;
			if (base.Columns != null)
			{
				base.Columns.BlockWrite();
			}
			base.InternalChildren.Remove(this._floatingHeader);
			this.AddFloatingHeader(this._draggingSrcHeader);
			this.UpdateFloatingHeader(this._draggingSrcHeader);
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x002C4AAC File Offset: 0x002C3AAC
		private void FinishHeaderDrag(bool isCancel)
		{
			this._prepareDragging = false;
			this._isHeaderDragging = false;
			this._draggingSrcHeader.SuppressClickEvent = false;
			this._floatingHeader.Visibility = Visibility.Hidden;
			this._floatingHeader.ResetFloatingHeaderCanvasBackground();
			this._indicator.Visibility = Visibility.Hidden;
			if (base.Columns != null)
			{
				base.Columns.UnblockWrite();
			}
			if (!isCancel)
			{
				bool flag = GridViewHeaderRowPresenter.IsMousePositionValid(this._floatingHeader, this._currentPos, 2.0);
				int newIndex = (this._startColumnIndex >= this._desColumnIndex) ? this._desColumnIndex : (this._desColumnIndex - 1);
				if (flag)
				{
					base.Columns.Move(this._startColumnIndex, newIndex);
				}
			}
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x002C4B59 File Offset: 0x002C3B59
		private static bool IsMousePositionValid(FrameworkElement floatingHeader, Point currentPos, double arrange)
		{
			return DoubleUtil.LessThanOrClose(-floatingHeader.Height * arrange, currentPos.Y) && DoubleUtil.LessThanOrClose(currentPos.Y, floatingHeader.Height * (arrange + 1.0));
		}

		// Token: 0x170018BF RID: 6335
		// (get) Token: 0x06006B2D RID: 27437 RVA: 0x002C4B94 File Offset: 0x002C3B94
		internal List<GridViewColumnHeader> ActualColumnHeaders
		{
			get
			{
				if (this._gvHeaders == null || !this._gvHeadersValid)
				{
					this._gvHeadersValid = true;
					this._gvHeaders = new List<GridViewColumnHeader>();
					if (base.Columns != null)
					{
						UIElementCollection internalChildren = base.InternalChildren;
						int i = 0;
						int count = base.Columns.Count;
						while (i < count)
						{
							GridViewColumnHeader gridViewColumnHeader = internalChildren[this.GetVisualIndex(i)] as GridViewColumnHeader;
							if (gridViewColumnHeader != null)
							{
								this._gvHeaders.Add(gridViewColumnHeader);
							}
							i++;
						}
					}
				}
				return this._gvHeaders;
			}
		}

		// Token: 0x170018C0 RID: 6336
		// (get) Token: 0x06006B2E RID: 27438 RVA: 0x002C4C12 File Offset: 0x002C3C12
		private List<Rect> HeadersPositionList
		{
			get
			{
				if (this._headersPositionList == null)
				{
					this._headersPositionList = new List<Rect>();
				}
				return this._headersPositionList;
			}
		}

		// Token: 0x06006B2F RID: 27439 RVA: 0x002C4C30 File Offset: 0x002C3C30
		private static DependencyProperty GetColumnDPFromName(string dpName)
		{
			foreach (DependencyProperty dependencyProperty in GridViewHeaderRowPresenter.s_DPList[1])
			{
				if (dependencyProperty != null && dpName.Equals(dependencyProperty.Name))
				{
					return dependencyProperty;
				}
			}
			return null;
		}

		// Token: 0x06006B30 RID: 27440 RVA: 0x002C4C6C File Offset: 0x002C3C6C
		private static void GetMatchingDPs(DependencyProperty indexDP, out DependencyProperty gvDP, out DependencyProperty columnDP, out DependencyProperty headerDP)
		{
			for (int i = 0; i < GridViewHeaderRowPresenter.s_DPList.Length; i++)
			{
				for (int j = 0; j < GridViewHeaderRowPresenter.s_DPList[i].Length; j++)
				{
					if (indexDP == GridViewHeaderRowPresenter.s_DPList[i][j])
					{
						gvDP = GridViewHeaderRowPresenter.s_DPList[0][j];
						columnDP = GridViewHeaderRowPresenter.s_DPList[1][j];
						headerDP = GridViewHeaderRowPresenter.s_DPList[2][j];
						return;
					}
				}
			}
			DependencyProperty dependencyProperty;
			headerDP = (dependencyProperty = null);
			columnDP = (dependencyProperty = dependencyProperty);
			gvDP = dependencyProperty;
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x002C4CE4 File Offset: 0x002C3CE4
		// Note: this type is marked as 'beforefieldinit'.
		static GridViewHeaderRowPresenter()
		{
			DependencyProperty[][] array = new DependencyProperty[3][];
			array[0] = new DependencyProperty[]
			{
				GridViewHeaderRowPresenter.ColumnHeaderContainerStyleProperty,
				GridViewHeaderRowPresenter.ColumnHeaderTemplateProperty,
				GridViewHeaderRowPresenter.ColumnHeaderTemplateSelectorProperty,
				GridViewHeaderRowPresenter.ColumnHeaderStringFormatProperty,
				GridViewHeaderRowPresenter.ColumnHeaderContextMenuProperty,
				GridViewHeaderRowPresenter.ColumnHeaderToolTipProperty
			};
			int num = 1;
			DependencyProperty[] array2 = new DependencyProperty[6];
			array2[0] = GridViewColumn.HeaderContainerStyleProperty;
			array2[1] = GridViewColumn.HeaderTemplateProperty;
			array2[2] = GridViewColumn.HeaderTemplateSelectorProperty;
			array2[3] = GridViewColumn.HeaderStringFormatProperty;
			array[num] = array2;
			array[2] = new DependencyProperty[]
			{
				FrameworkElement.StyleProperty,
				ContentControl.ContentTemplateProperty,
				ContentControl.ContentTemplateSelectorProperty,
				ContentControl.ContentStringFormatProperty,
				FrameworkElement.ContextMenuProperty,
				FrameworkElement.ToolTipProperty
			};
			GridViewHeaderRowPresenter.s_DPList = array;
		}

		// Token: 0x0400357B RID: 13691
		public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = GridView.ColumnHeaderContainerStyleProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		// Token: 0x0400357C RID: 13692
		public static readonly DependencyProperty ColumnHeaderTemplateProperty = GridView.ColumnHeaderTemplateProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		// Token: 0x0400357D RID: 13693
		public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = GridView.ColumnHeaderTemplateSelectorProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		// Token: 0x0400357E RID: 13694
		public static readonly DependencyProperty ColumnHeaderStringFormatProperty = GridView.ColumnHeaderStringFormatProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		// Token: 0x0400357F RID: 13695
		public static readonly DependencyProperty AllowsColumnReorderProperty = GridView.AllowsColumnReorderProperty.AddOwner(typeof(GridViewHeaderRowPresenter));

		// Token: 0x04003580 RID: 13696
		public static readonly DependencyProperty ColumnHeaderContextMenuProperty = GridView.ColumnHeaderContextMenuProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		// Token: 0x04003581 RID: 13697
		public static readonly DependencyProperty ColumnHeaderToolTipProperty = GridView.ColumnHeaderToolTipProperty.AddOwner(typeof(GridViewHeaderRowPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridViewHeaderRowPresenter.PropertyChanged)));

		// Token: 0x04003582 RID: 13698
		private bool _gvHeadersValid;

		// Token: 0x04003583 RID: 13699
		private List<GridViewColumnHeader> _gvHeaders;

		// Token: 0x04003584 RID: 13700
		private List<Rect> _headersPositionList;

		// Token: 0x04003585 RID: 13701
		private ScrollViewer _mainSV;

		// Token: 0x04003586 RID: 13702
		private ScrollViewer _headerSV;

		// Token: 0x04003587 RID: 13703
		private GridViewColumnHeader _paddingHeader;

		// Token: 0x04003588 RID: 13704
		private GridViewColumnHeader _floatingHeader;

		// Token: 0x04003589 RID: 13705
		private Separator _indicator;

		// Token: 0x0400358A RID: 13706
		private ItemsControl _itemsControl;

		// Token: 0x0400358B RID: 13707
		private GridViewColumnHeader _draggingSrcHeader;

		// Token: 0x0400358C RID: 13708
		private Point _startPos;

		// Token: 0x0400358D RID: 13709
		private Point _relativeStartPos;

		// Token: 0x0400358E RID: 13710
		private Point _currentPos;

		// Token: 0x0400358F RID: 13711
		private int _startColumnIndex;

		// Token: 0x04003590 RID: 13712
		private int _desColumnIndex;

		// Token: 0x04003591 RID: 13713
		private bool _isHeaderDragging;

		// Token: 0x04003592 RID: 13714
		private bool _isColumnChangedOrCreated;

		// Token: 0x04003593 RID: 13715
		private bool _prepareDragging;

		// Token: 0x04003594 RID: 13716
		private const double c_thresholdX = 4.0;

		// Token: 0x04003595 RID: 13717
		private static readonly DependencyProperty[][] s_DPList;
	}
}
