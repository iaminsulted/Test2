using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MS.Internal.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000575 RID: 1397
	internal class RecyclableWrapper : IDisposable
	{
		// Token: 0x060044D0 RID: 17616 RVA: 0x00222894 File Offset: 0x00221894
		public RecyclableWrapper(ItemsControl itemsControl, object item)
		{
			this._itemsControl = itemsControl;
			this._container = ((IGeneratorHost)itemsControl).GetContainerForItem(item);
			this.LinkItem(item);
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x002228B7 File Offset: 0x002218B7
		public void LinkItem(object item)
		{
			this._item = item;
			ItemContainerGenerator.LinkContainerToItem(this._container, this._item);
			((IItemContainerGenerator)this._itemsControl.ItemContainerGenerator).PrepareItemContainer(this._container);
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x002228E7 File Offset: 0x002218E7
		private void UnlinkItem()
		{
			if (this._item != null)
			{
				ItemContainerGenerator.UnlinkContainerFromItem(this._container, this._item, this._itemsControl);
				this._item = null;
			}
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x0022290F File Offset: 0x0022190F
		void IDisposable.Dispose()
		{
			this.UnlinkItem();
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x060044D4 RID: 17620 RVA: 0x00222917 File Offset: 0x00221917
		public AutomationPeer Peer
		{
			get
			{
				return UIElementAutomationPeer.CreatePeerForElement((UIElement)this._container);
			}
		}

		// Token: 0x0400253B RID: 9531
		private ItemsControl _itemsControl;

		// Token: 0x0400253C RID: 9532
		private DependencyObject _container;

		// Token: 0x0400253D RID: 9533
		private object _item;
	}
}
