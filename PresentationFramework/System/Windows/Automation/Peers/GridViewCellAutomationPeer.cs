using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000567 RID: 1383
	public class GridViewCellAutomationPeer : FrameworkElementAutomationPeer, ITableItemProvider, IGridItemProvider
	{
		// Token: 0x06004436 RID: 17462 RVA: 0x0022096A File Offset: 0x0021F96A
		internal GridViewCellAutomationPeer(ContentPresenter owner, ListViewAutomationPeer parent) : base(owner)
		{
			Invariant.Assert(parent != null);
			this._listviewAP = parent;
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x0022096A File Offset: 0x0021F96A
		internal GridViewCellAutomationPeer(TextBlock owner, ListViewAutomationPeer parent) : base(owner)
		{
			Invariant.Assert(parent != null);
			this._listviewAP = parent;
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x0021BF20 File Offset: 0x0021AF20
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x00220983 File Offset: 0x0021F983
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			if (base.Owner is TextBlock)
			{
				return AutomationControlType.Text;
			}
			return AutomationControlType.Custom;
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x00220997 File Offset: 0x0021F997
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.GridItem || patternInterface == PatternInterface.TableItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x002209AC File Offset: 0x0021F9AC
		protected override bool IsControlElementCore()
		{
			bool includeInvisibleElementsInControlView = base.IncludeInvisibleElementsInControlView;
			if (base.Owner is TextBlock)
			{
				return includeInvisibleElementsInControlView || base.Owner.IsVisible;
			}
			List<AutomationPeer> childrenAutomationPeer = this.GetChildrenAutomationPeer(base.Owner, includeInvisibleElementsInControlView);
			return childrenAutomationPeer != null && childrenAutomationPeer.Count >= 1;
		}

		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x0600443C RID: 17468 RVA: 0x002209FD File Offset: 0x0021F9FD
		// (set) Token: 0x0600443D RID: 17469 RVA: 0x00220A05 File Offset: 0x0021FA05
		internal int Column
		{
			get
			{
				return this._column;
			}
			set
			{
				this._column = value;
			}
		}

		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x0600443E RID: 17470 RVA: 0x00220A0E File Offset: 0x0021FA0E
		// (set) Token: 0x0600443F RID: 17471 RVA: 0x00220A16 File Offset: 0x0021FA16
		internal int Row
		{
			get
			{
				return this._row;
			}
			set
			{
				this._row = value;
			}
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x002206F0 File Offset: 0x0021F6F0
		IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
		{
			return Array.Empty<IRawElementProviderSimple>();
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x00220A20 File Offset: 0x0021FA20
		IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
		{
			ListView listView = this._listviewAP.Owner as ListView;
			if (listView != null && listView.View is GridView)
			{
				GridView gridView = listView.View as GridView;
				if (gridView.HeaderRowPresenter != null && gridView.HeaderRowPresenter.ActualColumnHeaders.Count > this.Column)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(gridView.HeaderRowPresenter.ActualColumnHeaders[this.Column]);
					if (automationPeer != null)
					{
						return new IRawElementProviderSimple[]
						{
							base.ProviderFromPeer(automationPeer)
						};
					}
				}
			}
			return Array.Empty<IRawElementProviderSimple>();
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06004442 RID: 17474 RVA: 0x00220AAF File Offset: 0x0021FAAF
		int IGridItemProvider.Row
		{
			get
			{
				return this.Row;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x06004443 RID: 17475 RVA: 0x00220AB7 File Offset: 0x0021FAB7
		int IGridItemProvider.Column
		{
			get
			{
				return this.Column;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x06004444 RID: 17476 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int IGridItemProvider.RowSpan
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06004445 RID: 17477 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06004446 RID: 17478 RVA: 0x00220ABF File Offset: 0x0021FABF
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				return base.ProviderFromPeer(this._listviewAP);
			}
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x00220AD0 File Offset: 0x0021FAD0
		private List<AutomationPeer> GetChildrenAutomationPeer(Visual parent, bool includeInvisibleItems)
		{
			Invariant.Assert(parent != null);
			List<AutomationPeer> children = null;
			GridViewCellAutomationPeer.iterate(parent, includeInvisibleItems, delegate(AutomationPeer peer)
			{
				if (children == null)
				{
					children = new List<AutomationPeer>();
				}
				children.Add(peer);
				return false;
			});
			return children;
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x00220B10 File Offset: 0x0021FB10
		private static bool iterate(Visual parent, bool includeInvisibleItems, GridViewCellAutomationPeer.IteratorCallback callback)
		{
			bool flag = false;
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			int num = 0;
			while (num < internalVisualChildrenCount && !flag)
			{
				Visual visual = parent.InternalGetVisualChild(num);
				AutomationPeer peer;
				if (visual != null && visual.CheckFlagsAnd(VisualFlags.IsUIElement) && (includeInvisibleItems || ((UIElement)visual).IsVisible) && (peer = UIElementAutomationPeer.CreatePeerForElement((UIElement)visual)) != null)
				{
					flag = callback(peer);
				}
				else
				{
					flag = GridViewCellAutomationPeer.iterate(visual, includeInvisibleItems, callback);
				}
				num++;
			}
			return flag;
		}

		// Token: 0x04002529 RID: 9513
		private ListViewAutomationPeer _listviewAP;

		// Token: 0x0400252A RID: 9514
		private int _column;

		// Token: 0x0400252B RID: 9515
		private int _row;

		// Token: 0x02000B1D RID: 2845
		// (Invoke) Token: 0x06008C58 RID: 35928
		private delegate bool IteratorCallback(AutomationPeer peer);
	}
}
