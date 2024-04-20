using System;
using System.Collections;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000783 RID: 1923
	[StyleTypedProperty(Property = "PreviewStyle", StyleTargetType = typeof(Control))]
	public class GridSplitter : Thumb
	{
		// Token: 0x06006A0B RID: 27147 RVA: 0x002C0924 File Offset: 0x002BF924
		static GridSplitter()
		{
			EventManager.RegisterClassHandler(typeof(GridSplitter), Thumb.DragStartedEvent, new DragStartedEventHandler(GridSplitter.OnDragStarted));
			EventManager.RegisterClassHandler(typeof(GridSplitter), Thumb.DragDeltaEvent, new DragDeltaEventHandler(GridSplitter.OnDragDelta));
			EventManager.RegisterClassHandler(typeof(GridSplitter), Thumb.DragCompletedEvent, new DragCompletedEventHandler(GridSplitter.OnDragCompleted));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(typeof(GridSplitter)));
			GridSplitter._dType = DependencyObjectType.FromSystemTypeInternal(typeof(GridSplitter));
			UIElement.FocusableProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			FrameworkElement.HorizontalAlignmentProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(HorizontalAlignment.Right));
			FrameworkElement.CursorProperty.OverrideMetadata(typeof(GridSplitter), new FrameworkPropertyMetadata(null, new CoerceValueCallback(GridSplitter.CoerceCursor)));
			ControlsTraceLogger.AddControl(TelemetryControls.GridSplitter);
		}

		// Token: 0x06006A0D RID: 27149 RVA: 0x002C0B98 File Offset: 0x002BFB98
		private static void UpdateCursor(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			o.CoerceValue(FrameworkElement.CursorProperty);
		}

		// Token: 0x06006A0E RID: 27150 RVA: 0x002C0BA8 File Offset: 0x002BFBA8
		private static object CoerceCursor(DependencyObject o, object value)
		{
			GridSplitter gridSplitter = (GridSplitter)o;
			bool flag;
			BaseValueSourceInternal valueSource = gridSplitter.GetValueSource(FrameworkElement.CursorProperty, null, out flag);
			if (value == null && valueSource == BaseValueSourceInternal.Default)
			{
				GridResizeDirection effectiveResizeDirection = gridSplitter.GetEffectiveResizeDirection();
				if (effectiveResizeDirection == GridResizeDirection.Columns)
				{
					return Cursors.SizeWE;
				}
				if (effectiveResizeDirection == GridResizeDirection.Rows)
				{
					return Cursors.SizeNS;
				}
			}
			return value;
		}

		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x06006A0F RID: 27151 RVA: 0x002C0BF0 File Offset: 0x002BFBF0
		// (set) Token: 0x06006A10 RID: 27152 RVA: 0x002C0C02 File Offset: 0x002BFC02
		public GridResizeDirection ResizeDirection
		{
			get
			{
				return (GridResizeDirection)base.GetValue(GridSplitter.ResizeDirectionProperty);
			}
			set
			{
				base.SetValue(GridSplitter.ResizeDirectionProperty, value);
			}
		}

		// Token: 0x06006A11 RID: 27153 RVA: 0x002C0C18 File Offset: 0x002BFC18
		private static bool IsValidResizeDirection(object o)
		{
			GridResizeDirection gridResizeDirection = (GridResizeDirection)o;
			return gridResizeDirection == GridResizeDirection.Auto || gridResizeDirection == GridResizeDirection.Columns || gridResizeDirection == GridResizeDirection.Rows;
		}

		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x06006A12 RID: 27154 RVA: 0x002C0C39 File Offset: 0x002BFC39
		// (set) Token: 0x06006A13 RID: 27155 RVA: 0x002C0C4B File Offset: 0x002BFC4B
		public GridResizeBehavior ResizeBehavior
		{
			get
			{
				return (GridResizeBehavior)base.GetValue(GridSplitter.ResizeBehaviorProperty);
			}
			set
			{
				base.SetValue(GridSplitter.ResizeBehaviorProperty, value);
			}
		}

		// Token: 0x06006A14 RID: 27156 RVA: 0x002C0C60 File Offset: 0x002BFC60
		private static bool IsValidResizeBehavior(object o)
		{
			GridResizeBehavior gridResizeBehavior = (GridResizeBehavior)o;
			return gridResizeBehavior == GridResizeBehavior.BasedOnAlignment || gridResizeBehavior == GridResizeBehavior.CurrentAndNext || gridResizeBehavior == GridResizeBehavior.PreviousAndCurrent || gridResizeBehavior == GridResizeBehavior.PreviousAndNext;
		}

		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x06006A15 RID: 27157 RVA: 0x002C0C85 File Offset: 0x002BFC85
		// (set) Token: 0x06006A16 RID: 27158 RVA: 0x002C0C97 File Offset: 0x002BFC97
		public bool ShowsPreview
		{
			get
			{
				return (bool)base.GetValue(GridSplitter.ShowsPreviewProperty);
			}
			set
			{
				base.SetValue(GridSplitter.ShowsPreviewProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17001884 RID: 6276
		// (get) Token: 0x06006A17 RID: 27159 RVA: 0x002C0CAA File Offset: 0x002BFCAA
		// (set) Token: 0x06006A18 RID: 27160 RVA: 0x002C0CBC File Offset: 0x002BFCBC
		public Style PreviewStyle
		{
			get
			{
				return (Style)base.GetValue(GridSplitter.PreviewStyleProperty);
			}
			set
			{
				base.SetValue(GridSplitter.PreviewStyleProperty, value);
			}
		}

		// Token: 0x17001885 RID: 6277
		// (get) Token: 0x06006A19 RID: 27161 RVA: 0x002C0CCA File Offset: 0x002BFCCA
		// (set) Token: 0x06006A1A RID: 27162 RVA: 0x002C0CDC File Offset: 0x002BFCDC
		public double KeyboardIncrement
		{
			get
			{
				return (double)base.GetValue(GridSplitter.KeyboardIncrementProperty);
			}
			set
			{
				base.SetValue(GridSplitter.KeyboardIncrementProperty, value);
			}
		}

		// Token: 0x06006A1B RID: 27163 RVA: 0x002C0CF0 File Offset: 0x002BFCF0
		private static bool IsValidDelta(object o)
		{
			double num = (double)o;
			return num > 0.0 && !double.IsPositiveInfinity(num);
		}

		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x06006A1C RID: 27164 RVA: 0x002C0D1B File Offset: 0x002BFD1B
		// (set) Token: 0x06006A1D RID: 27165 RVA: 0x002C0D2D File Offset: 0x002BFD2D
		public double DragIncrement
		{
			get
			{
				return (double)base.GetValue(GridSplitter.DragIncrementProperty);
			}
			set
			{
				base.SetValue(GridSplitter.DragIncrementProperty, value);
			}
		}

		// Token: 0x06006A1E RID: 27166 RVA: 0x002C0D40 File Offset: 0x002BFD40
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GridSplitterAutomationPeer(this);
		}

		// Token: 0x06006A1F RID: 27167 RVA: 0x002C0D48 File Offset: 0x002BFD48
		private GridResizeDirection GetEffectiveResizeDirection()
		{
			GridResizeDirection gridResizeDirection = this.ResizeDirection;
			if (gridResizeDirection == GridResizeDirection.Auto)
			{
				if (base.HorizontalAlignment != HorizontalAlignment.Stretch)
				{
					gridResizeDirection = GridResizeDirection.Columns;
				}
				else if (base.VerticalAlignment != VerticalAlignment.Stretch)
				{
					gridResizeDirection = GridResizeDirection.Rows;
				}
				else if (base.ActualWidth <= base.ActualHeight)
				{
					gridResizeDirection = GridResizeDirection.Columns;
				}
				else
				{
					gridResizeDirection = GridResizeDirection.Rows;
				}
			}
			return gridResizeDirection;
		}

		// Token: 0x06006A20 RID: 27168 RVA: 0x002C0D90 File Offset: 0x002BFD90
		private GridResizeBehavior GetEffectiveResizeBehavior(GridResizeDirection direction)
		{
			GridResizeBehavior gridResizeBehavior = this.ResizeBehavior;
			if (gridResizeBehavior == GridResizeBehavior.BasedOnAlignment)
			{
				if (direction == GridResizeDirection.Columns)
				{
					HorizontalAlignment horizontalAlignment = base.HorizontalAlignment;
					if (horizontalAlignment != HorizontalAlignment.Left)
					{
						if (horizontalAlignment != HorizontalAlignment.Right)
						{
							gridResizeBehavior = GridResizeBehavior.PreviousAndNext;
						}
						else
						{
							gridResizeBehavior = GridResizeBehavior.CurrentAndNext;
						}
					}
					else
					{
						gridResizeBehavior = GridResizeBehavior.PreviousAndCurrent;
					}
				}
				else
				{
					VerticalAlignment verticalAlignment = base.VerticalAlignment;
					if (verticalAlignment != VerticalAlignment.Top)
					{
						if (verticalAlignment != VerticalAlignment.Bottom)
						{
							gridResizeBehavior = GridResizeBehavior.PreviousAndNext;
						}
						else
						{
							gridResizeBehavior = GridResizeBehavior.CurrentAndNext;
						}
					}
					else
					{
						gridResizeBehavior = GridResizeBehavior.PreviousAndCurrent;
					}
				}
			}
			return gridResizeBehavior;
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x002C0DE2 File Offset: 0x002BFDE2
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			base.CoerceValue(FrameworkElement.CursorProperty);
		}

		// Token: 0x06006A22 RID: 27170 RVA: 0x002C0DF6 File Offset: 0x002BFDF6
		private void RemovePreviewAdorner()
		{
			if (this._resizeData.Adorner != null)
			{
				(VisualTreeHelper.GetParent(this._resizeData.Adorner) as AdornerLayer).Remove(this._resizeData.Adorner);
			}
		}

		// Token: 0x06006A23 RID: 27171 RVA: 0x002C0E2C File Offset: 0x002BFE2C
		private void InitializeData(bool ShowsPreview)
		{
			Grid grid = base.Parent as Grid;
			if (grid != null)
			{
				this._resizeData = new GridSplitter.ResizeData();
				this._resizeData.Grid = grid;
				this._resizeData.ShowsPreview = ShowsPreview;
				this._resizeData.ResizeDirection = this.GetEffectiveResizeDirection();
				this._resizeData.ResizeBehavior = this.GetEffectiveResizeBehavior(this._resizeData.ResizeDirection);
				this._resizeData.SplitterLength = Math.Min(base.ActualWidth, base.ActualHeight);
				if (!this.SetupDefinitionsToResize())
				{
					this._resizeData = null;
					return;
				}
				this.SetupPreview();
			}
		}

		// Token: 0x06006A24 RID: 27172 RVA: 0x002C0ED0 File Offset: 0x002BFED0
		private bool SetupDefinitionsToResize()
		{
			if ((int)base.GetValue((this._resizeData.ResizeDirection == GridResizeDirection.Columns) ? Grid.ColumnSpanProperty : Grid.RowSpanProperty) == 1)
			{
				int num = (int)base.GetValue((this._resizeData.ResizeDirection == GridResizeDirection.Columns) ? Grid.ColumnProperty : Grid.RowProperty);
				GridResizeBehavior resizeBehavior = this._resizeData.ResizeBehavior;
				int num2;
				int num3;
				if (resizeBehavior != GridResizeBehavior.CurrentAndNext)
				{
					if (resizeBehavior == GridResizeBehavior.PreviousAndCurrent)
					{
						num2 = num - 1;
						num3 = num;
					}
					else
					{
						num2 = num - 1;
						num3 = num + 1;
					}
				}
				else
				{
					num2 = num;
					num3 = num + 1;
				}
				int num4 = (this._resizeData.ResizeDirection == GridResizeDirection.Columns) ? this._resizeData.Grid.ColumnDefinitions.Count : this._resizeData.Grid.RowDefinitions.Count;
				if (num2 >= 0 && num3 < num4)
				{
					this._resizeData.SplitterIndex = num;
					this._resizeData.Definition1Index = num2;
					this._resizeData.Definition1 = GridSplitter.GetGridDefinition(this._resizeData.Grid, num2, this._resizeData.ResizeDirection);
					this._resizeData.OriginalDefinition1Length = this._resizeData.Definition1.UserSizeValueCache;
					this._resizeData.OriginalDefinition1ActualLength = this.GetActualLength(this._resizeData.Definition1);
					this._resizeData.Definition2Index = num3;
					this._resizeData.Definition2 = GridSplitter.GetGridDefinition(this._resizeData.Grid, num3, this._resizeData.ResizeDirection);
					this._resizeData.OriginalDefinition2Length = this._resizeData.Definition2.UserSizeValueCache;
					this._resizeData.OriginalDefinition2ActualLength = this.GetActualLength(this._resizeData.Definition2);
					bool flag = GridSplitter.IsStar(this._resizeData.Definition1);
					bool flag2 = GridSplitter.IsStar(this._resizeData.Definition2);
					if (flag && flag2)
					{
						this._resizeData.SplitBehavior = GridSplitter.SplitBehavior.Split;
					}
					else
					{
						this._resizeData.SplitBehavior = ((!flag) ? GridSplitter.SplitBehavior.Resize1 : GridSplitter.SplitBehavior.Resize2);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006A25 RID: 27173 RVA: 0x002C10D8 File Offset: 0x002C00D8
		private void SetupPreview()
		{
			if (this._resizeData.ShowsPreview)
			{
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._resizeData.Grid);
				if (adornerLayer == null)
				{
					return;
				}
				this._resizeData.Adorner = new GridSplitter.PreviewAdorner(this, this.PreviewStyle);
				adornerLayer.Add(this._resizeData.Adorner);
				this.GetDeltaConstraints(out this._resizeData.MinChange, out this._resizeData.MaxChange);
			}
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x002C114B File Offset: 0x002C014B
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (this._resizeData != null)
			{
				this.CancelResize();
			}
		}

		// Token: 0x06006A27 RID: 27175 RVA: 0x002C1162 File Offset: 0x002C0162
		private static void OnDragStarted(object sender, DragStartedEventArgs e)
		{
			(sender as GridSplitter).OnDragStarted(e);
		}

		// Token: 0x06006A28 RID: 27176 RVA: 0x002C1170 File Offset: 0x002C0170
		private void OnDragStarted(DragStartedEventArgs e)
		{
			this.InitializeData(this.ShowsPreview);
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x002C117E File Offset: 0x002C017E
		private static void OnDragDelta(object sender, DragDeltaEventArgs e)
		{
			(sender as GridSplitter).OnDragDelta(e);
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x002C118C File Offset: 0x002C018C
		private void OnDragDelta(DragDeltaEventArgs e)
		{
			if (this._resizeData != null)
			{
				double num = e.HorizontalChange;
				double num2 = e.VerticalChange;
				double dragIncrement = this.DragIncrement;
				num = Math.Round(num / dragIncrement) * dragIncrement;
				num2 = Math.Round(num2 / dragIncrement) * dragIncrement;
				if (this._resizeData.ShowsPreview)
				{
					if (this._resizeData.ResizeDirection == GridResizeDirection.Columns)
					{
						this._resizeData.Adorner.OffsetX = Math.Min(Math.Max(num, this._resizeData.MinChange), this._resizeData.MaxChange);
						return;
					}
					this._resizeData.Adorner.OffsetY = Math.Min(Math.Max(num2, this._resizeData.MinChange), this._resizeData.MaxChange);
					return;
				}
				else
				{
					this.MoveSplitter(num, num2);
				}
			}
		}

		// Token: 0x06006A2B RID: 27179 RVA: 0x002C1256 File Offset: 0x002C0256
		private static void OnDragCompleted(object sender, DragCompletedEventArgs e)
		{
			(sender as GridSplitter).OnDragCompleted(e);
		}

		// Token: 0x06006A2C RID: 27180 RVA: 0x002C1264 File Offset: 0x002C0264
		private void OnDragCompleted(DragCompletedEventArgs e)
		{
			if (this._resizeData != null)
			{
				if (this._resizeData.ShowsPreview)
				{
					this.MoveSplitter(this._resizeData.Adorner.OffsetX, this._resizeData.Adorner.OffsetY);
					this.RemovePreviewAdorner();
				}
				this._resizeData = null;
			}
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x002C12BC File Offset: 0x002C02BC
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key != Key.Escape)
			{
				switch (key)
				{
				case Key.Left:
					e.Handled = this.KeyboardMoveSplitter(-this.KeyboardIncrement, 0.0);
					return;
				case Key.Up:
					e.Handled = this.KeyboardMoveSplitter(0.0, -this.KeyboardIncrement);
					return;
				case Key.Right:
					e.Handled = this.KeyboardMoveSplitter(this.KeyboardIncrement, 0.0);
					return;
				case Key.Down:
					e.Handled = this.KeyboardMoveSplitter(0.0, this.KeyboardIncrement);
					break;
				default:
					return;
				}
			}
			else if (this._resizeData != null)
			{
				this.CancelResize();
				e.Handled = true;
				return;
			}
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x002C1378 File Offset: 0x002C0378
		private void CancelResize()
		{
			DependencyObject parent = base.Parent;
			if (this._resizeData.ShowsPreview)
			{
				this.RemovePreviewAdorner();
			}
			else
			{
				GridSplitter.SetDefinitionLength(this._resizeData.Definition1, this._resizeData.OriginalDefinition1Length);
				GridSplitter.SetDefinitionLength(this._resizeData.Definition2, this._resizeData.OriginalDefinition2Length);
			}
			this._resizeData = null;
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x002C13E0 File Offset: 0x002C03E0
		private static bool IsStar(DefinitionBase definition)
		{
			return definition.UserSizeValueCache.IsStar;
		}

		// Token: 0x06006A30 RID: 27184 RVA: 0x002C13FB File Offset: 0x002C03FB
		private static DefinitionBase GetGridDefinition(Grid grid, int index, GridResizeDirection direction)
		{
			if (direction != GridResizeDirection.Columns)
			{
				return grid.RowDefinitions[index];
			}
			return grid.ColumnDefinitions[index];
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x002C141C File Offset: 0x002C041C
		private double GetActualLength(DefinitionBase definition)
		{
			ColumnDefinition columnDefinition = definition as ColumnDefinition;
			if (columnDefinition != null)
			{
				return columnDefinition.ActualWidth;
			}
			return ((RowDefinition)definition).ActualHeight;
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x002C1445 File Offset: 0x002C0445
		private static void SetDefinitionLength(DefinitionBase definition, GridLength length)
		{
			definition.SetValue((definition is ColumnDefinition) ? ColumnDefinition.WidthProperty : RowDefinition.HeightProperty, length);
		}

		// Token: 0x06006A33 RID: 27187 RVA: 0x002C1468 File Offset: 0x002C0468
		private void GetDeltaConstraints(out double minDelta, out double maxDelta)
		{
			double actualLength = this.GetActualLength(this._resizeData.Definition1);
			double num = this._resizeData.Definition1.UserMinSizeValueCache;
			double userMaxSizeValueCache = this._resizeData.Definition1.UserMaxSizeValueCache;
			double actualLength2 = this.GetActualLength(this._resizeData.Definition2);
			double num2 = this._resizeData.Definition2.UserMinSizeValueCache;
			double userMaxSizeValueCache2 = this._resizeData.Definition2.UserMaxSizeValueCache;
			if (this._resizeData.SplitterIndex == this._resizeData.Definition1Index)
			{
				num = Math.Max(num, this._resizeData.SplitterLength);
			}
			else if (this._resizeData.SplitterIndex == this._resizeData.Definition2Index)
			{
				num2 = Math.Max(num2, this._resizeData.SplitterLength);
			}
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Split)
			{
				minDelta = -Math.Min(actualLength - num, userMaxSizeValueCache2 - actualLength2);
				maxDelta = Math.Min(userMaxSizeValueCache - actualLength, actualLength2 - num2);
				return;
			}
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Resize1)
			{
				minDelta = num - actualLength;
				maxDelta = userMaxSizeValueCache - actualLength;
				return;
			}
			minDelta = actualLength2 - userMaxSizeValueCache2;
			maxDelta = actualLength2 - num2;
		}

		// Token: 0x06006A34 RID: 27188 RVA: 0x002C1588 File Offset: 0x002C0588
		private void SetLengths(double definition1Pixels, double definition2Pixels)
		{
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Split)
			{
				IEnumerable enumerable2;
				if (this._resizeData.ResizeDirection != GridResizeDirection.Columns)
				{
					IEnumerable enumerable = this._resizeData.Grid.RowDefinitions;
					enumerable2 = enumerable;
				}
				else
				{
					IEnumerable enumerable = this._resizeData.Grid.ColumnDefinitions;
					enumerable2 = enumerable;
				}
				int num = 0;
				using (IEnumerator enumerator = enumerable2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DefinitionBase definition = (DefinitionBase)obj;
						if (num == this._resizeData.Definition1Index)
						{
							GridSplitter.SetDefinitionLength(definition, new GridLength(definition1Pixels, GridUnitType.Star));
						}
						else if (num == this._resizeData.Definition2Index)
						{
							GridSplitter.SetDefinitionLength(definition, new GridLength(definition2Pixels, GridUnitType.Star));
						}
						else if (GridSplitter.IsStar(definition))
						{
							GridSplitter.SetDefinitionLength(definition, new GridLength(this.GetActualLength(definition), GridUnitType.Star));
						}
						num++;
					}
					return;
				}
			}
			if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Resize1)
			{
				GridSplitter.SetDefinitionLength(this._resizeData.Definition1, new GridLength(definition1Pixels));
				return;
			}
			GridSplitter.SetDefinitionLength(this._resizeData.Definition2, new GridLength(definition2Pixels));
		}

		// Token: 0x06006A35 RID: 27189 RVA: 0x002C16B4 File Offset: 0x002C06B4
		private void MoveSplitter(double horizontalChange, double verticalChange)
		{
			DpiScale dpi = base.GetDpi();
			double num;
			if (this._resizeData.ResizeDirection == GridResizeDirection.Columns)
			{
				num = horizontalChange;
				if (base.UseLayoutRounding)
				{
					num = UIElement.RoundLayoutValue(num, dpi.DpiScaleX);
				}
			}
			else
			{
				num = verticalChange;
				if (base.UseLayoutRounding)
				{
					num = UIElement.RoundLayoutValue(num, dpi.DpiScaleY);
				}
			}
			DefinitionBase definition = this._resizeData.Definition1;
			DefinitionBase definition2 = this._resizeData.Definition2;
			if (definition != null && definition2 != null)
			{
				double actualLength = this.GetActualLength(definition);
				double actualLength2 = this.GetActualLength(definition2);
				if (this._resizeData.SplitBehavior == GridSplitter.SplitBehavior.Split && !LayoutDoubleUtil.AreClose(actualLength + actualLength2, this._resizeData.OriginalDefinition1ActualLength + this._resizeData.OriginalDefinition2ActualLength))
				{
					this.CancelResize();
					return;
				}
				double val;
				double val2;
				this.GetDeltaConstraints(out val, out val2);
				if (base.FlowDirection != this._resizeData.Grid.FlowDirection)
				{
					num = -num;
				}
				num = Math.Min(Math.Max(num, val), val2);
				double num2 = actualLength + num;
				double definition2Pixels = actualLength + actualLength2 - num2;
				this.SetLengths(num2, definition2Pixels);
			}
		}

		// Token: 0x06006A36 RID: 27190 RVA: 0x002C17C4 File Offset: 0x002C07C4
		internal bool KeyboardMoveSplitter(double horizontalChange, double verticalChange)
		{
			if (this._resizeData != null)
			{
				return false;
			}
			this.InitializeData(false);
			if (this._resizeData == null)
			{
				return false;
			}
			if (base.FlowDirection == FlowDirection.RightToLeft)
			{
				horizontalChange = -horizontalChange;
			}
			this.MoveSplitter(horizontalChange, verticalChange);
			this._resizeData = null;
			return true;
		}

		// Token: 0x17001887 RID: 6279
		// (get) Token: 0x06006A37 RID: 27191 RVA: 0x002C17FE File Offset: 0x002C07FE
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return GridSplitter._dType;
			}
		}

		// Token: 0x04003533 RID: 13619
		public static readonly DependencyProperty ResizeDirectionProperty = DependencyProperty.Register("ResizeDirection", typeof(GridResizeDirection), typeof(GridSplitter), new FrameworkPropertyMetadata(GridResizeDirection.Auto, new PropertyChangedCallback(GridSplitter.UpdateCursor)), new ValidateValueCallback(GridSplitter.IsValidResizeDirection));

		// Token: 0x04003534 RID: 13620
		public static readonly DependencyProperty ResizeBehaviorProperty = DependencyProperty.Register("ResizeBehavior", typeof(GridResizeBehavior), typeof(GridSplitter), new FrameworkPropertyMetadata(GridResizeBehavior.BasedOnAlignment), new ValidateValueCallback(GridSplitter.IsValidResizeBehavior));

		// Token: 0x04003535 RID: 13621
		public static readonly DependencyProperty ShowsPreviewProperty = DependencyProperty.Register("ShowsPreview", typeof(bool), typeof(GridSplitter), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04003536 RID: 13622
		public static readonly DependencyProperty PreviewStyleProperty = DependencyProperty.Register("PreviewStyle", typeof(Style), typeof(GridSplitter), new FrameworkPropertyMetadata(null));

		// Token: 0x04003537 RID: 13623
		public static readonly DependencyProperty KeyboardIncrementProperty = DependencyProperty.Register("KeyboardIncrement", typeof(double), typeof(GridSplitter), new FrameworkPropertyMetadata(10.0), new ValidateValueCallback(GridSplitter.IsValidDelta));

		// Token: 0x04003538 RID: 13624
		public static readonly DependencyProperty DragIncrementProperty = DependencyProperty.Register("DragIncrement", typeof(double), typeof(GridSplitter), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(GridSplitter.IsValidDelta));

		// Token: 0x04003539 RID: 13625
		private GridSplitter.ResizeData _resizeData;

		// Token: 0x0400353A RID: 13626
		private static DependencyObjectType _dType;

		// Token: 0x02000BEC RID: 3052
		private sealed class PreviewAdorner : Adorner
		{
			// Token: 0x06008FC2 RID: 36802 RVA: 0x00345464 File Offset: 0x00344464
			public PreviewAdorner(GridSplitter gridSplitter, Style previewStyle) : base(gridSplitter)
			{
				Control control = new Control();
				control.Style = previewStyle;
				control.IsEnabled = false;
				this.Translation = new TranslateTransform();
				this._decorator = new Decorator();
				this._decorator.Child = control;
				this._decorator.RenderTransform = this.Translation;
				base.AddVisualChild(this._decorator);
			}

			// Token: 0x06008FC3 RID: 36803 RVA: 0x003454CB File Offset: 0x003444CB
			protected override Visual GetVisualChild(int index)
			{
				if (index != 0)
				{
					throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
				}
				return this._decorator;
			}

			// Token: 0x17001F6D RID: 8045
			// (get) Token: 0x06008FC4 RID: 36804 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
			protected override int VisualChildrenCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x06008FC5 RID: 36805 RVA: 0x003454F4 File Offset: 0x003444F4
			protected override Size ArrangeOverride(Size finalSize)
			{
				this._decorator.Arrange(new Rect(default(Point), finalSize));
				return finalSize;
			}

			// Token: 0x17001F6E RID: 8046
			// (get) Token: 0x06008FC6 RID: 36806 RVA: 0x0034551C File Offset: 0x0034451C
			// (set) Token: 0x06008FC7 RID: 36807 RVA: 0x00345529 File Offset: 0x00344529
			public double OffsetX
			{
				get
				{
					return this.Translation.X;
				}
				set
				{
					this.Translation.X = value;
				}
			}

			// Token: 0x17001F6F RID: 8047
			// (get) Token: 0x06008FC8 RID: 36808 RVA: 0x00345537 File Offset: 0x00344537
			// (set) Token: 0x06008FC9 RID: 36809 RVA: 0x00345544 File Offset: 0x00344544
			public double OffsetY
			{
				get
				{
					return this.Translation.Y;
				}
				set
				{
					this.Translation.Y = value;
				}
			}

			// Token: 0x04004A61 RID: 19041
			private TranslateTransform Translation;

			// Token: 0x04004A62 RID: 19042
			private Decorator _decorator;
		}

		// Token: 0x02000BED RID: 3053
		private enum SplitBehavior
		{
			// Token: 0x04004A64 RID: 19044
			Split,
			// Token: 0x04004A65 RID: 19045
			Resize1,
			// Token: 0x04004A66 RID: 19046
			Resize2
		}

		// Token: 0x02000BEE RID: 3054
		private class ResizeData
		{
			// Token: 0x04004A67 RID: 19047
			public bool ShowsPreview;

			// Token: 0x04004A68 RID: 19048
			public GridSplitter.PreviewAdorner Adorner;

			// Token: 0x04004A69 RID: 19049
			public double MinChange;

			// Token: 0x04004A6A RID: 19050
			public double MaxChange;

			// Token: 0x04004A6B RID: 19051
			public Grid Grid;

			// Token: 0x04004A6C RID: 19052
			public GridResizeDirection ResizeDirection;

			// Token: 0x04004A6D RID: 19053
			public GridResizeBehavior ResizeBehavior;

			// Token: 0x04004A6E RID: 19054
			public DefinitionBase Definition1;

			// Token: 0x04004A6F RID: 19055
			public DefinitionBase Definition2;

			// Token: 0x04004A70 RID: 19056
			public GridSplitter.SplitBehavior SplitBehavior;

			// Token: 0x04004A71 RID: 19057
			public int SplitterIndex;

			// Token: 0x04004A72 RID: 19058
			public int Definition1Index;

			// Token: 0x04004A73 RID: 19059
			public int Definition2Index;

			// Token: 0x04004A74 RID: 19060
			public GridLength OriginalDefinition1Length;

			// Token: 0x04004A75 RID: 19061
			public GridLength OriginalDefinition2Length;

			// Token: 0x04004A76 RID: 19062
			public double OriginalDefinition1ActualLength;

			// Token: 0x04004A77 RID: 19063
			public double OriginalDefinition2ActualLength;

			// Token: 0x04004A78 RID: 19064
			public double SplitterLength;
		}
	}
}
