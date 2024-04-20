using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000566 RID: 1382
	public class GridViewAutomationPeer : IViewAutomationPeer, ITableProvider, IGridProvider
	{
		// Token: 0x06004427 RID: 17447 RVA: 0x0022049C File Offset: 0x0021F49C
		public GridViewAutomationPeer(GridView owner, ListView listview)
		{
			Invariant.Assert(owner != null);
			Invariant.Assert(listview != null);
			this._owner = owner;
			this._listview = listview;
			this._oldItemsCount = this._listview.Items.Count;
			this._oldColumnsCount = this._owner.Columns.Count;
			((INotifyCollectionChanged)this._owner.Columns).CollectionChanged += this.OnColumnCollectionChanged;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x001FD464 File Offset: 0x001FC464
		AutomationControlType IViewAutomationPeer.GetAutomationControlType()
		{
			return AutomationControlType.DataGrid;
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x00220518 File Offset: 0x0021F518
		object IViewAutomationPeer.GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			if (patternInterface == PatternInterface.Grid || patternInterface == PatternInterface.Table)
			{
				result = this;
			}
			return result;
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x00220534 File Offset: 0x0021F534
		List<AutomationPeer> IViewAutomationPeer.GetChildren(List<AutomationPeer> children)
		{
			if (this._owner.HeaderRowPresenter != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this._owner.HeaderRowPresenter);
				if (automationPeer != null)
				{
					if (children == null)
					{
						children = new List<AutomationPeer>();
					}
					children.Insert(0, automationPeer);
				}
			}
			return children;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00220578 File Offset: 0x0021F578
		ItemAutomationPeer IViewAutomationPeer.CreateItemAutomationPeer(object item)
		{
			ListViewAutomationPeer listviewAP = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
			return new GridViewItemAutomationPeer(item, listviewAP);
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x002205A0 File Offset: 0x0021F5A0
		void IViewAutomationPeer.ItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null)
			{
				if (this._oldItemsCount != this._listview.Items.Count)
				{
					listViewAutomationPeer.RaisePropertyChangedEvent(GridPatternIdentifiers.RowCountProperty, this._oldItemsCount, this._listview.Items.Count);
				}
				this._oldItemsCount = this._listview.Items.Count;
			}
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x0022061A File Offset: 0x0021F61A
		[MethodImpl(MethodImplOptions.NoInlining)]
		void IViewAutomationPeer.ViewDetached()
		{
			((INotifyCollectionChanged)this._owner.Columns).CollectionChanged -= this.OnColumnCollectionChanged;
		}

		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x0600442E RID: 17454 RVA: 0x00105F35 File Offset: 0x00104F35
		RowOrColumnMajor ITableProvider.RowOrColumnMajor
		{
			get
			{
				return RowOrColumnMajor.RowMajor;
			}
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x00220638 File Offset: 0x0021F638
		IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
		{
			if (this._owner.HeaderRowPresenter != null)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>(this._owner.HeaderRowPresenter.ActualColumnHeaders.Count);
				ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
				if (listViewAutomationPeer != null)
				{
					foreach (GridViewColumnHeader element in this._owner.HeaderRowPresenter.ActualColumnHeaders)
					{
						AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(element);
						if (automationPeer != null)
						{
							list.Add(ElementProxy.StaticWrap(automationPeer, listViewAutomationPeer));
						}
					}
				}
				return list.ToArray();
			}
			return new IRawElementProviderSimple[0];
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x002206F0 File Offset: 0x0021F6F0
		IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
		{
			return Array.Empty<IRawElementProviderSimple>();
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06004431 RID: 17457 RVA: 0x002206F7 File Offset: 0x0021F6F7
		int IGridProvider.ColumnCount
		{
			get
			{
				if (this._owner.HeaderRowPresenter != null)
				{
					return this._owner.HeaderRowPresenter.ActualColumnHeaders.Count;
				}
				return this._owner.Columns.Count;
			}
		}

		// Token: 0x17000F60 RID: 3936
		// (get) Token: 0x06004432 RID: 17458 RVA: 0x0022072C File Offset: 0x0021F72C
		int IGridProvider.RowCount
		{
			get
			{
				return this._listview.Items.Count;
			}
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x00220740 File Offset: 0x0021F740
		IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
		{
			if (row < 0 || row >= ((IGridProvider)this).RowCount)
			{
				throw new ArgumentOutOfRangeException("row");
			}
			if (column < 0 || column >= ((IGridProvider)this).ColumnCount)
			{
				throw new ArgumentOutOfRangeException("column");
			}
			ListViewItem listViewItem = this._listview.ItemContainerGenerator.ContainerFromIndex(row) as ListViewItem;
			if (listViewItem == null)
			{
				VirtualizingPanel virtualizingPanel = this._listview.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.BringIndexIntoView(row);
				}
				listViewItem = (this._listview.ItemContainerGenerator.ContainerFromIndex(row) as ListViewItem);
				if (listViewItem != null)
				{
					this._listview.Dispatcher.Invoke(DispatcherPriority.Loaded, new DispatcherOperationCallback((object arg) => null), null);
				}
			}
			if (listViewItem != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this._listview);
				if (automationPeer != null)
				{
					AutomationPeer automationPeer2 = UIElementAutomationPeer.FromElement(listViewItem);
					if (automationPeer2 != null)
					{
						AutomationPeer eventsSource = automationPeer2.EventsSource;
						if (eventsSource != null)
						{
							automationPeer2 = eventsSource;
						}
						List<AutomationPeer> children = automationPeer2.GetChildren();
						if (children.Count > column)
						{
							return ElementProxy.StaticWrap(children[column], automationPeer);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0022084C File Offset: 0x0021F84C
		private void OnColumnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this._oldColumnsCount != this._owner.Columns.Count)
			{
				ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this._listview) as ListViewAutomationPeer;
				Invariant.Assert(listViewAutomationPeer != null);
				if (listViewAutomationPeer != null)
				{
					listViewAutomationPeer.RaisePropertyChangedEvent(GridPatternIdentifiers.ColumnCountProperty, this._oldColumnsCount, this._owner.Columns.Count);
				}
			}
			this._oldColumnsCount = this._owner.Columns.Count;
			AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(this._listview);
			if (automationPeer != null)
			{
				List<AutomationPeer> children = automationPeer.GetChildren();
				if (children != null)
				{
					foreach (AutomationPeer automationPeer2 in children)
					{
						automationPeer2.InvalidatePeer();
					}
				}
			}
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x00220928 File Offset: 0x0021F928
		internal static Visual FindVisualByType(Visual parent, Type type)
		{
			if (parent != null)
			{
				int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
				for (int i = 0; i < internalVisualChildrenCount; i++)
				{
					Visual visual = parent.InternalGetVisualChild(i);
					if (!type.IsInstanceOfType(visual))
					{
						visual = GridViewAutomationPeer.FindVisualByType(visual, type);
					}
					if (visual != null)
					{
						return visual;
					}
				}
			}
			return null;
		}

		// Token: 0x04002525 RID: 9509
		private GridView _owner;

		// Token: 0x04002526 RID: 9510
		private ListView _listview;

		// Token: 0x04002527 RID: 9511
		private int _oldItemsCount;

		// Token: 0x04002528 RID: 9512
		private int _oldColumnsCount;
	}
}
