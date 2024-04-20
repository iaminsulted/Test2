using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000552 RID: 1362
	public sealed class DataGridColumnHeadersPresenterAutomationPeer : ItemsControlAutomationPeer, IItemContainerProvider
	{
		// Token: 0x06004358 RID: 17240 RVA: 0x0021DF60 File Offset: 0x0021CF60
		public DataGridColumnHeadersPresenterAutomationPeer(DataGridColumnHeadersPresenter owner) : base(owner)
		{
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x001FC072 File Offset: 0x001FB072
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Header;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0021DF6C File Offset: 0x0021CF6C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = null;
			ItemPeersStorage<ItemAutomationPeer> itemPeers = base.ItemPeers;
			base.ItemPeers = new ItemPeersStorage<ItemAutomationPeer>();
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			DataGrid owningDataGrid = this.OwningDataGrid;
			if (owningDataGrid != null && owningDataGrid.Columns.Count > 0)
			{
				Panel itemsHost = itemsControl.ItemsHost;
				IList list2;
				if (this.IsVirtualized)
				{
					if (itemsHost == null)
					{
						return null;
					}
					list2 = itemsHost.Children;
				}
				else
				{
					list2 = this.OwningDataGrid.Columns;
				}
				list = new List<AutomationPeer>(list2.Count);
				foreach (object obj in list2)
				{
					DataGridColumn dataGridColumn;
					if (obj is DataGridColumnHeader)
					{
						dataGridColumn = ((DataGridColumnHeader)obj).Column;
					}
					else
					{
						dataGridColumn = (obj as DataGridColumn);
					}
					ItemAutomationPeer itemAutomationPeer = itemPeers[dataGridColumn];
					if (itemAutomationPeer == null)
					{
						itemAutomationPeer = base.GetPeerFromWeakRefStorage(dataGridColumn);
						if (itemAutomationPeer != null)
						{
							itemAutomationPeer.AncestorsInvalid = false;
							itemAutomationPeer.ChildrenValid = false;
						}
					}
					object o = (dataGridColumn == null) ? null : dataGridColumn.Header;
					if (itemAutomationPeer == null || !ItemsControl.EqualsEx(itemAutomationPeer.Item, o))
					{
						itemAutomationPeer = this.CreateItemAutomationPeer(dataGridColumn);
					}
					if (itemAutomationPeer != null)
					{
						AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
						if (wrapperPeer != null)
						{
							wrapperPeer.EventsSource = itemAutomationPeer;
						}
					}
					if (itemAutomationPeer != null && base.ItemPeers[dataGridColumn] == null)
					{
						list.Add(itemAutomationPeer);
						base.ItemPeers[dataGridColumn] = itemAutomationPeer;
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0021E100 File Offset: 0x0021D100
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfter, int propertyId, object value)
		{
			base.ResetChildrenCache();
			if (propertyId != 0 && !this.IsPropertySupportedByControlForFindItem(propertyId))
			{
				throw new ArgumentException(SR.Get("PropertyNotSupported"));
			}
			bool flag = (ItemsControl)base.Owner != null;
			IList list = null;
			if (flag)
			{
				list = this.OwningDataGrid.Columns;
			}
			if (list != null && list.Count > 0)
			{
				DataGridColumnHeaderItemAutomationPeer dataGridColumnHeaderItemAutomationPeer = null;
				if (startAfter != null)
				{
					dataGridColumnHeaderItemAutomationPeer = (base.PeerFromProvider(startAfter) as DataGridColumnHeaderItemAutomationPeer);
					if (dataGridColumnHeaderItemAutomationPeer == null)
					{
						return null;
					}
				}
				int num = 0;
				if (dataGridColumnHeaderItemAutomationPeer != null)
				{
					if (dataGridColumnHeaderItemAutomationPeer.Item == null)
					{
						throw new InvalidOperationException(SR.Get("InavalidStartItem"));
					}
					num = list.IndexOf(dataGridColumnHeaderItemAutomationPeer.Column) + 1;
					if (num == 0 || num == list.Count)
					{
						return null;
					}
				}
				if (propertyId == 0)
				{
					for (int i = num; i < list.Count; i++)
					{
						if (list.IndexOf(list[i]) == i)
						{
							return base.ProviderFromPeer(this.FindOrCreateItemAutomationPeer(list[i]));
						}
					}
				}
				object obj = null;
				for (int j = num; j < list.Count; j++)
				{
					ItemAutomationPeer itemAutomationPeer = this.FindOrCreateItemAutomationPeer(list[j]);
					if (itemAutomationPeer != null)
					{
						try
						{
							obj = this.GetSupportedPropertyValue(itemAutomationPeer, propertyId);
						}
						catch (Exception ex)
						{
							if (ex is ElementNotAvailableException)
							{
								goto IL_15D;
							}
						}
						if (value == null || obj == null)
						{
							if (obj == null && value == null && list.IndexOf(list[j]) == j)
							{
								return base.ProviderFromPeer(itemAutomationPeer);
							}
						}
						else if (value.Equals(obj) && list.IndexOf(list[j]) == j)
						{
							return base.ProviderFromPeer(itemAutomationPeer);
						}
					}
					IL_15D:;
				}
			}
			return null;
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x00105F35 File Offset: 0x00104F35
		protected override bool IsContentElementCore()
		{
			return false;
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0021E290 File Offset: 0x0021D290
		protected override ItemAutomationPeer CreateItemAutomationPeer(object column)
		{
			DataGridColumn dataGridColumn = column as DataGridColumn;
			if (column != null)
			{
				return new DataGridColumnHeaderItemAutomationPeer(dataGridColumn.Header, dataGridColumn, this);
			}
			return null;
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x0600435F RID: 17247 RVA: 0x0021E2B6 File Offset: 0x0021D2B6
		private DataGrid OwningDataGrid
		{
			get
			{
				return ((DataGridColumnHeadersPresenter)base.Owner).ParentDataGrid;
			}
		}
	}
}
