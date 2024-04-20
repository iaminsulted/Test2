using System;
using System.Windows.Controls.Primitives;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200074D RID: 1869
	internal class DataGridColumnDropSeparator : Separator
	{
		// Token: 0x060065D3 RID: 26067 RVA: 0x002AFDC8 File Offset: 0x002AEDC8
		static DataGridColumnDropSeparator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(DataGridColumnHeader.ColumnHeaderDropSeparatorStyleKey));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnDropSeparator.OnCoerceWidth)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnDropSeparator.OnCoerceHeight)));
		}

		// Token: 0x060065D4 RID: 26068 RVA: 0x002AFE3F File Offset: 0x002AEE3F
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			if (DoubleUtil.IsNaN((double)baseValue))
			{
				return 2.0;
			}
			return baseValue;
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x002AFE60 File Offset: 0x002AEE60
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnDropSeparator dataGridColumnDropSeparator = (DataGridColumnDropSeparator)d;
			if (dataGridColumnDropSeparator._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnDropSeparator._referenceHeader.ActualHeight;
			}
			return baseValue;
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x060065D6 RID: 26070 RVA: 0x002AFE9D File Offset: 0x002AEE9D
		// (set) Token: 0x060065D7 RID: 26071 RVA: 0x002AFEA5 File Offset: 0x002AEEA5
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

		// Token: 0x04003397 RID: 13207
		private DataGridColumnHeader _referenceHeader;
	}
}
