using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000846 RID: 2118
	public abstract class MultiSelector : Selector
	{
		// Token: 0x17001CAE RID: 7342
		// (get) Token: 0x06007C05 RID: 31749 RVA: 0x0030D407 File Offset: 0x0030C407
		// (set) Token: 0x06007C06 RID: 31750 RVA: 0x0030D40F File Offset: 0x0030C40F
		protected bool CanSelectMultipleItems
		{
			get
			{
				return base.CanSelectMultiple;
			}
			set
			{
				base.CanSelectMultiple = value;
			}
		}

		// Token: 0x17001CAF RID: 7343
		// (get) Token: 0x06007C07 RID: 31751 RVA: 0x002D2109 File Offset: 0x002D1109
		[Category("Appearance")]
		[Bindable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList SelectedItems
		{
			get
			{
				return base.SelectedItemsImpl;
			}
		}

		// Token: 0x06007C08 RID: 31752 RVA: 0x0030D418 File Offset: 0x0030C418
		protected void BeginUpdateSelectedItems()
		{
			((SelectedItemCollection)this.SelectedItems).BeginUpdateSelectedItems();
		}

		// Token: 0x06007C09 RID: 31753 RVA: 0x0030D42A File Offset: 0x0030C42A
		protected void EndUpdateSelectedItems()
		{
			((SelectedItemCollection)this.SelectedItems).EndUpdateSelectedItems();
		}

		// Token: 0x17001CB0 RID: 7344
		// (get) Token: 0x06007C0A RID: 31754 RVA: 0x0030D43C File Offset: 0x0030C43C
		protected bool IsUpdatingSelectedItems
		{
			get
			{
				return ((SelectedItemCollection)this.SelectedItems).IsUpdatingSelectedItems;
			}
		}

		// Token: 0x06007C0B RID: 31755 RVA: 0x0030D44E File Offset: 0x0030C44E
		public void SelectAll()
		{
			if (this.CanSelectMultipleItems)
			{
				this.SelectAllImpl();
				return;
			}
			throw new NotSupportedException(SR.Get("MultiSelectorSelectAll"));
		}

		// Token: 0x06007C0C RID: 31756 RVA: 0x002D2056 File Offset: 0x002D1056
		public void UnselectAll()
		{
			this.UnselectAllImpl();
		}
	}
}
