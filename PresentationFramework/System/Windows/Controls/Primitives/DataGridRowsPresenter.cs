using System;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200082E RID: 2094
	public class DataGridRowsPresenter : VirtualizingStackPanel
	{
		// Token: 0x06007AC1 RID: 31425 RVA: 0x002AAF1F File Offset: 0x002A9F1F
		internal void InternalBringIndexIntoView(int index)
		{
			this.BringIndexIntoView(index);
		}

		// Token: 0x06007AC2 RID: 31426 RVA: 0x003099DC File Offset: 0x003089DC
		protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
		{
			base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
			if (newIsItemsHost)
			{
				DataGrid owner = this.Owner;
				if (owner != null)
				{
					IItemContainerGenerator itemContainerGenerator = owner.ItemContainerGenerator;
					if (itemContainerGenerator != null && itemContainerGenerator == itemContainerGenerator.GetItemContainerGeneratorForPanel(this))
					{
						owner.InternalItemsHost = this;
						return;
					}
				}
			}
			else
			{
				if (this._owner != null && this._owner.InternalItemsHost == this)
				{
					this._owner.InternalItemsHost = null;
				}
				this._owner = null;
			}
		}

		// Token: 0x06007AC3 RID: 31427 RVA: 0x00309A44 File Offset: 0x00308A44
		protected override void OnViewportSizeChanged(Size oldViewportSize, Size newViewportSize)
		{
			DataGrid owner = this.Owner;
			if (owner != null)
			{
				ScrollContentPresenter internalScrollContentPresenter = owner.InternalScrollContentPresenter;
				if (internalScrollContentPresenter == null || internalScrollContentPresenter.CanContentScroll)
				{
					owner.OnViewportSizeChanged(oldViewportSize, newViewportSize);
				}
			}
		}

		// Token: 0x06007AC4 RID: 31428 RVA: 0x00309A75 File Offset: 0x00308A75
		protected override Size MeasureOverride(Size constraint)
		{
			this._availableSize = constraint;
			return base.MeasureOverride(constraint);
		}

		// Token: 0x17001C68 RID: 7272
		// (get) Token: 0x06007AC5 RID: 31429 RVA: 0x00309A85 File Offset: 0x00308A85
		internal Size AvailableSize
		{
			get
			{
				return this._availableSize;
			}
		}

		// Token: 0x06007AC6 RID: 31430 RVA: 0x00309A8D File Offset: 0x00308A8D
		protected override void OnCleanUpVirtualizedItem(CleanUpVirtualizedItemEventArgs e)
		{
			base.OnCleanUpVirtualizedItem(e);
			if (e.UIElement != null && Validation.GetHasError(e.UIElement))
			{
				e.Cancel = true;
			}
		}

		// Token: 0x17001C69 RID: 7273
		// (get) Token: 0x06007AC7 RID: 31431 RVA: 0x00309AB2 File Offset: 0x00308AB2
		internal DataGrid Owner
		{
			get
			{
				if (this._owner == null)
				{
					this._owner = (ItemsControl.GetItemsOwner(this) as DataGrid);
				}
				return this._owner;
			}
		}

		// Token: 0x04003A18 RID: 14872
		private DataGrid _owner;

		// Token: 0x04003A19 RID: 14873
		private Size _availableSize;
	}
}
