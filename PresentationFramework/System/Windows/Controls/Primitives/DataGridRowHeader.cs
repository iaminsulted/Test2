using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200082D RID: 2093
	[TemplatePart(Name = "PART_TopHeaderGripper", Type = typeof(Thumb))]
	[TemplatePart(Name = "PART_BottomHeaderGripper", Type = typeof(Thumb))]
	public class DataGridRowHeader : ButtonBase
	{
		// Token: 0x06007A9B RID: 31387 RVA: 0x00308D80 File Offset: 0x00307D80
		static DataGridRowHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(typeof(DataGridRowHeader)));
			ContentControl.ContentProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceContent)));
			ContentControl.ContentTemplateProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceContentTemplate)));
			ContentControl.ContentTemplateSelectorProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceContentTemplateSelector)));
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceStyle)));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridRowHeader.OnNotifyPropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceWidth)));
			ButtonBase.ClickModeProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(ClickMode.Press));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(false));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(DataGridRowHeader), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x06007A9C RID: 31388 RVA: 0x0030906F File Offset: 0x0030806F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridRowHeaderAutomationPeer(this);
		}

		// Token: 0x17001C5E RID: 7262
		// (get) Token: 0x06007A9D RID: 31389 RVA: 0x00309077 File Offset: 0x00308077
		// (set) Token: 0x06007A9E RID: 31390 RVA: 0x00309089 File Offset: 0x00308089
		public Brush SeparatorBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGridRowHeader.SeparatorBrushProperty);
			}
			set
			{
				base.SetValue(DataGridRowHeader.SeparatorBrushProperty, value);
			}
		}

		// Token: 0x17001C5F RID: 7263
		// (get) Token: 0x06007A9F RID: 31391 RVA: 0x00309097 File Offset: 0x00308097
		// (set) Token: 0x06007AA0 RID: 31392 RVA: 0x003090A9 File Offset: 0x003080A9
		public Visibility SeparatorVisibility
		{
			get
			{
				return (Visibility)base.GetValue(DataGridRowHeader.SeparatorVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGridRowHeader.SeparatorVisibilityProperty, value);
			}
		}

		// Token: 0x06007AA1 RID: 31393 RVA: 0x003090BC File Offset: 0x003080BC
		protected override Size MeasureOverride(Size availableSize)
		{
			Size result = base.MeasureOverride(availableSize);
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner == null)
			{
				return result;
			}
			if (DoubleUtil.IsNaN(dataGridOwner.RowHeaderWidth) && result.Width > dataGridOwner.RowHeaderActualWidth)
			{
				dataGridOwner.RowHeaderActualWidth = result.Width;
			}
			return new Size(dataGridOwner.RowHeaderActualWidth, result.Height);
		}

		// Token: 0x06007AA2 RID: 31394 RVA: 0x00309118 File Offset: 0x00308118
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			DataGridRow parentRow = this.ParentRow;
			if (parentRow != null)
			{
				parentRow.RowHeader = this;
				this.SyncProperties();
			}
			this.HookupGripperEvents();
		}

		// Token: 0x06007AA3 RID: 31395 RVA: 0x00309148 File Offset: 0x00308148
		internal void SyncProperties()
		{
			DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
			DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.WidthProperty);
			base.CoerceValue(DataGridRowHeader.IsRowSelectedProperty);
			this.OnCanUserResizeRowsChanged();
		}

		// Token: 0x06007AA4 RID: 31396 RVA: 0x0030919D File Offset: 0x0030819D
		private static void OnNotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridRowHeader)d).NotifyPropertyChanged(d, e);
		}

		// Token: 0x06007AA5 RID: 31397 RVA: 0x003091AC File Offset: 0x003081AC
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.Property == DataGridRow.HeaderProperty || e.Property == ContentControl.ContentProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderStyleProperty || e.Property == DataGridRow.HeaderStyleProperty || e.Property == FrameworkElement.StyleProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.StyleProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderTemplateProperty || e.Property == DataGridRow.HeaderTemplateProperty || e.Property == ContentControl.ContentTemplateProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderTemplateSelectorProperty || e.Property == DataGridRow.HeaderTemplateSelectorProperty || e.Property == ContentControl.ContentTemplateSelectorProperty)
			{
				DataGridHelper.TransferProperty(this, ContentControl.ContentTemplateSelectorProperty);
				return;
			}
			if (e.Property == DataGrid.RowHeaderWidthProperty || e.Property == FrameworkElement.WidthProperty)
			{
				DataGridHelper.TransferProperty(this, FrameworkElement.WidthProperty);
				return;
			}
			if (e.Property == DataGridRow.IsSelectedProperty)
			{
				base.CoerceValue(DataGridRowHeader.IsRowSelectedProperty);
				return;
			}
			if (e.Property == DataGrid.CanUserResizeRowsProperty)
			{
				this.OnCanUserResizeRowsChanged();
				return;
			}
			if (e.Property == DataGrid.RowHeaderActualWidthProperty)
			{
				base.InvalidateMeasure();
				base.InvalidateArrange();
				UIElement uielement = base.Parent as UIElement;
				if (uielement != null)
				{
					uielement.InvalidateMeasure();
					uielement.InvalidateArrange();
					return;
				}
			}
			else if (e.Property == DataGrid.CurrentItemProperty || e.Property == DataGridRow.IsEditingProperty || e.Property == UIElement.IsMouseOverProperty || e.Property == UIElement.IsKeyboardFocusWithinProperty)
			{
				base.UpdateVisualState();
			}
		}

		// Token: 0x06007AA6 RID: 31398 RVA: 0x00309350 File Offset: 0x00308350
		private static object OnCoerceContent(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, ContentControl.ContentProperty, dataGridRowHeader.ParentRow, DataGridRow.HeaderProperty);
		}

		// Token: 0x06007AA7 RID: 31399 RVA: 0x0030937C File Offset: 0x0030837C
		private static object OnCoerceContentTemplate(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			DataGridRow parentRow = dataGridRowHeader.ParentRow;
			DataGrid grandParentObject = (parentRow != null) ? parentRow.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, ContentControl.ContentTemplateProperty, parentRow, DataGridRow.HeaderTemplateProperty, grandParentObject, DataGrid.RowHeaderTemplateProperty);
		}

		// Token: 0x06007AA8 RID: 31400 RVA: 0x003093BC File Offset: 0x003083BC
		private static object OnCoerceContentTemplateSelector(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			DataGridRow parentRow = dataGridRowHeader.ParentRow;
			DataGrid grandParentObject = (parentRow != null) ? parentRow.DataGridOwner : null;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, ContentControl.ContentTemplateSelectorProperty, parentRow, DataGridRow.HeaderTemplateSelectorProperty, grandParentObject, DataGrid.RowHeaderTemplateSelectorProperty);
		}

		// Token: 0x06007AA9 RID: 31401 RVA: 0x003093FC File Offset: 0x003083FC
		private static object OnCoerceStyle(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, FrameworkElement.StyleProperty, dataGridRowHeader.ParentRow, DataGridRow.HeaderStyleProperty, dataGridRowHeader.DataGridOwner, DataGrid.RowHeaderStyleProperty);
		}

		// Token: 0x06007AAA RID: 31402 RVA: 0x00309434 File Offset: 0x00308434
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			DataGridRowHeader dataGridRowHeader = d as DataGridRowHeader;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridRowHeader, baseValue, FrameworkElement.WidthProperty, dataGridRowHeader.DataGridOwner, DataGrid.RowHeaderWidthProperty);
		}

		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x06007AAB RID: 31403 RVA: 0x00309460 File Offset: 0x00308460
		private bool IsRowCurrent
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					DataGrid dataGridOwner = parentRow.DataGridOwner;
					if (dataGridOwner != null)
					{
						return dataGridOwner.IsCurrent(parentRow, null);
					}
				}
				return false;
			}
		}

		// Token: 0x17001C61 RID: 7265
		// (get) Token: 0x06007AAC RID: 31404 RVA: 0x0030948C File Offset: 0x0030848C
		private bool IsRowEditing
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				return parentRow != null && parentRow.IsEditing;
			}
		}

		// Token: 0x17001C62 RID: 7266
		// (get) Token: 0x06007AAD RID: 31405 RVA: 0x003094AC File Offset: 0x003084AC
		private bool IsRowMouseOver
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				return parentRow != null && parentRow.IsMouseOver;
			}
		}

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x06007AAE RID: 31406 RVA: 0x003094CC File Offset: 0x003084CC
		private bool IsDataGridKeyboardFocusWithin
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					DataGrid dataGridOwner = parentRow.DataGridOwner;
					if (dataGridOwner != null)
					{
						return dataGridOwner.IsKeyboardFocusWithin;
					}
				}
				return false;
			}
		}

		// Token: 0x06007AAF RID: 31407 RVA: 0x003094F8 File Offset: 0x003084F8
		internal override void ChangeVisualState(bool useTransitions)
		{
			byte b = 0;
			if (this.IsRowCurrent)
			{
				b += 16;
			}
			if (this.IsRowSelected || this.IsRowEditing)
			{
				b += 8;
			}
			if (this.IsRowEditing)
			{
				b += 4;
			}
			if (this.IsRowMouseOver)
			{
				b += 2;
			}
			if (this.IsDataGridKeyboardFocusWithin)
			{
				b += 1;
			}
			for (byte b2 = DataGridRowHeader._idealStateMapping[(int)b]; b2 != 255; b2 = DataGridRowHeader._fallbackStateMapping[(int)b2])
			{
				string stateName = DataGridRowHeader._stateNames[(int)b2];
				if (VisualStateManager.GoToState(this, stateName, useTransitions))
				{
					break;
				}
			}
			base.ChangeValidationVisualState(useTransitions);
		}

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x06007AB0 RID: 31408 RVA: 0x00309584 File Offset: 0x00308584
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsRowSelected
		{
			get
			{
				return (bool)base.GetValue(DataGridRowHeader.IsRowSelectedProperty);
			}
		}

		// Token: 0x06007AB1 RID: 31409 RVA: 0x00309598 File Offset: 0x00308598
		private static object OnCoerceIsRowSelected(DependencyObject d, object baseValue)
		{
			DataGridRow parentRow = ((DataGridRowHeader)d).ParentRow;
			if (parentRow != null)
			{
				return parentRow.IsSelected;
			}
			return baseValue;
		}

		// Token: 0x06007AB2 RID: 31410 RVA: 0x003095C4 File Offset: 0x003085C4
		protected override void OnClick()
		{
			base.OnClick();
			if (Mouse.Captured == this)
			{
				base.ReleaseMouseCapture();
			}
			DataGrid dataGridOwner = this.DataGridOwner;
			DataGridRow parentRow = this.ParentRow;
			if (dataGridOwner != null && parentRow != null)
			{
				dataGridOwner.HandleSelectionForRowHeaderAndDetailsInput(parentRow, true);
			}
		}

		// Token: 0x06007AB3 RID: 31411 RVA: 0x00309604 File Offset: 0x00308604
		private void HookupGripperEvents()
		{
			this.UnhookGripperEvents();
			this._topGripper = (base.GetTemplateChild("PART_TopHeaderGripper") as Thumb);
			this._bottomGripper = (base.GetTemplateChild("PART_BottomHeaderGripper") as Thumb);
			if (this._topGripper != null)
			{
				this._topGripper.DragStarted += this.OnRowHeaderGripperDragStarted;
				this._topGripper.DragDelta += this.OnRowHeaderResize;
				this._topGripper.DragCompleted += this.OnRowHeaderGripperDragCompleted;
				this._topGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetTopGripperVisibility();
			}
			if (this._bottomGripper != null)
			{
				this._bottomGripper.DragStarted += this.OnRowHeaderGripperDragStarted;
				this._bottomGripper.DragDelta += this.OnRowHeaderResize;
				this._bottomGripper.DragCompleted += this.OnRowHeaderGripperDragCompleted;
				this._bottomGripper.MouseDoubleClick += this.OnGripperDoubleClicked;
				this.SetBottomGripperVisibility();
			}
		}

		// Token: 0x06007AB4 RID: 31412 RVA: 0x00309718 File Offset: 0x00308718
		private void UnhookGripperEvents()
		{
			if (this._topGripper != null)
			{
				this._topGripper.DragStarted -= this.OnRowHeaderGripperDragStarted;
				this._topGripper.DragDelta -= this.OnRowHeaderResize;
				this._topGripper.DragCompleted -= this.OnRowHeaderGripperDragCompleted;
				this._topGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._topGripper = null;
			}
			if (this._bottomGripper != null)
			{
				this._bottomGripper.DragStarted -= this.OnRowHeaderGripperDragStarted;
				this._bottomGripper.DragDelta -= this.OnRowHeaderResize;
				this._bottomGripper.DragCompleted -= this.OnRowHeaderGripperDragCompleted;
				this._bottomGripper.MouseDoubleClick -= this.OnGripperDoubleClicked;
				this._bottomGripper = null;
			}
		}

		// Token: 0x06007AB5 RID: 31413 RVA: 0x003097FC File Offset: 0x003087FC
		private void SetTopGripperVisibility()
		{
			if (this._topGripper != null)
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				DataGridRow parentRow = this.ParentRow;
				if (dataGridOwner != null && parentRow != null && dataGridOwner.CanUserResizeRows && dataGridOwner.Items.Count > 1 && parentRow.Item != dataGridOwner.Items[0])
				{
					this._topGripper.Visibility = Visibility.Visible;
					return;
				}
				this._topGripper.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x06007AB6 RID: 31414 RVA: 0x00309868 File Offset: 0x00308868
		private void SetBottomGripperVisibility()
		{
			if (this._bottomGripper != null)
			{
				DataGrid dataGridOwner = this.DataGridOwner;
				if (dataGridOwner != null && dataGridOwner.CanUserResizeRows)
				{
					this._bottomGripper.Visibility = Visibility.Visible;
					return;
				}
				this._bottomGripper.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x06007AB7 RID: 31415 RVA: 0x003098A8 File Offset: 0x003088A8
		private DataGridRow PreviousRow
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					DataGrid dataGridOwner = parentRow.DataGridOwner;
					if (dataGridOwner != null)
					{
						int num = dataGridOwner.ItemContainerGenerator.IndexFromContainer(parentRow);
						if (num > 0)
						{
							return (DataGridRow)dataGridOwner.ItemContainerGenerator.ContainerFromIndex(num - 1);
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06007AB8 RID: 31416 RVA: 0x003098EF File Offset: 0x003088EF
		private DataGridRow RowToResize(object gripper)
		{
			if (gripper != this._bottomGripper)
			{
				return this.PreviousRow;
			}
			return this.ParentRow;
		}

		// Token: 0x06007AB9 RID: 31417 RVA: 0x00309908 File Offset: 0x00308908
		private void OnRowHeaderGripperDragStarted(object sender, DragStartedEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResizeStarted();
				e.Handled = true;
			}
		}

		// Token: 0x06007ABA RID: 31418 RVA: 0x00309930 File Offset: 0x00308930
		private void OnRowHeaderResize(object sender, DragDeltaEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResize(e.VerticalChange);
				e.Handled = true;
			}
		}

		// Token: 0x06007ABB RID: 31419 RVA: 0x0030995C File Offset: 0x0030895C
		private void OnRowHeaderGripperDragCompleted(object sender, DragCompletedEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResizeCompleted(e.Canceled);
				e.Handled = true;
			}
		}

		// Token: 0x06007ABC RID: 31420 RVA: 0x00309988 File Offset: 0x00308988
		private void OnGripperDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			DataGridRow dataGridRow = this.RowToResize(sender);
			if (dataGridRow != null)
			{
				dataGridRow.OnRowResizeReset();
				e.Handled = true;
			}
		}

		// Token: 0x06007ABD RID: 31421 RVA: 0x003099AD File Offset: 0x003089AD
		private void OnCanUserResizeRowsChanged()
		{
			this.SetTopGripperVisibility();
			this.SetBottomGripperVisibility();
		}

		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x06007ABE RID: 31422 RVA: 0x00306677 File Offset: 0x00305677
		internal DataGridRow ParentRow
		{
			get
			{
				return DataGridHelper.FindParent<DataGridRow>(this);
			}
		}

		// Token: 0x17001C67 RID: 7271
		// (get) Token: 0x06007ABF RID: 31423 RVA: 0x003099BC File Offset: 0x003089BC
		private DataGrid DataGridOwner
		{
			get
			{
				DataGridRow parentRow = this.ParentRow;
				if (parentRow != null)
				{
					return parentRow.DataGridOwner;
				}
				return null;
			}
		}

		// Token: 0x040039FC RID: 14844
		private const byte DATAGRIDROWHEADER_stateMouseOverCode = 0;

		// Token: 0x040039FD RID: 14845
		private const byte DATAGRIDROWHEADER_stateMouseOverCurrentRowCode = 1;

		// Token: 0x040039FE RID: 14846
		private const byte DATAGRIDROWHEADER_stateMouseOverEditingRowCode = 2;

		// Token: 0x040039FF RID: 14847
		private const byte DATAGRIDROWHEADER_stateMouseOverEditingRowFocusedCode = 3;

		// Token: 0x04003A00 RID: 14848
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedCode = 4;

		// Token: 0x04003A01 RID: 14849
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRowCode = 5;

		// Token: 0x04003A02 RID: 14850
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRowFocusedCode = 6;

		// Token: 0x04003A03 RID: 14851
		private const byte DATAGRIDROWHEADER_stateMouseOverSelectedFocusedCode = 7;

		// Token: 0x04003A04 RID: 14852
		private const byte DATAGRIDROWHEADER_stateNormalCode = 8;

		// Token: 0x04003A05 RID: 14853
		private const byte DATAGRIDROWHEADER_stateNormalCurrentRowCode = 9;

		// Token: 0x04003A06 RID: 14854
		private const byte DATAGRIDROWHEADER_stateNormalEditingRowCode = 10;

		// Token: 0x04003A07 RID: 14855
		private const byte DATAGRIDROWHEADER_stateNormalEditingRowFocusedCode = 11;

		// Token: 0x04003A08 RID: 14856
		private const byte DATAGRIDROWHEADER_stateSelectedCode = 12;

		// Token: 0x04003A09 RID: 14857
		private const byte DATAGRIDROWHEADER_stateSelectedCurrentRowCode = 13;

		// Token: 0x04003A0A RID: 14858
		private const byte DATAGRIDROWHEADER_stateSelectedCurrentRowFocusedCode = 14;

		// Token: 0x04003A0B RID: 14859
		private const byte DATAGRIDROWHEADER_stateSelectedFocusedCode = 15;

		// Token: 0x04003A0C RID: 14860
		private const byte DATAGRIDROWHEADER_stateNullCode = 255;

		// Token: 0x04003A0D RID: 14861
		private static byte[] _fallbackStateMapping = new byte[]
		{
			8,
			9,
			3,
			11,
			7,
			6,
			15,
			15,
			byte.MaxValue,
			8,
			11,
			14,
			15,
			14,
			9,
			8
		};

		// Token: 0x04003A0E RID: 14862
		private static byte[] _idealStateMapping = new byte[]
		{
			8,
			8,
			0,
			0,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			12,
			15,
			4,
			7,
			10,
			11,
			2,
			3,
			9,
			9,
			1,
			1,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			byte.MaxValue,
			13,
			14,
			5,
			6,
			10,
			11,
			2,
			3
		};

		// Token: 0x04003A0F RID: 14863
		private static string[] _stateNames = new string[]
		{
			"MouseOver",
			"MouseOver_CurrentRow",
			"MouseOver_Unfocused_EditingRow",
			"MouseOver_EditingRow",
			"MouseOver_Unfocused_Selected",
			"MouseOver_Unfocused_CurrentRow_Selected",
			"MouseOver_CurrentRow_Selected",
			"MouseOver_Selected",
			"Normal",
			"Normal_CurrentRow",
			"Unfocused_EditingRow",
			"Normal_EditingRow",
			"Unfocused_Selected",
			"Unfocused_CurrentRow_Selected",
			"Normal_CurrentRow_Selected",
			"Normal_Selected"
		};

		// Token: 0x04003A10 RID: 14864
		public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(null));

		// Token: 0x04003A11 RID: 14865
		public static readonly DependencyProperty SeparatorVisibilityProperty = DependencyProperty.Register("SeparatorVisibility", typeof(Visibility), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(Visibility.Visible));

		// Token: 0x04003A12 RID: 14866
		private static readonly DependencyPropertyKey IsRowSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsRowSelected", typeof(bool), typeof(DataGridRowHeader), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged), new CoerceValueCallback(DataGridRowHeader.OnCoerceIsRowSelected)));

		// Token: 0x04003A13 RID: 14867
		public static readonly DependencyProperty IsRowSelectedProperty = DataGridRowHeader.IsRowSelectedPropertyKey.DependencyProperty;

		// Token: 0x04003A14 RID: 14868
		private Thumb _topGripper;

		// Token: 0x04003A15 RID: 14869
		private Thumb _bottomGripper;

		// Token: 0x04003A16 RID: 14870
		private const string TopHeaderGripperTemplateName = "PART_TopHeaderGripper";

		// Token: 0x04003A17 RID: 14871
		private const string BottomHeaderGripperTemplateName = "PART_BottomHeaderGripper";
	}
}
