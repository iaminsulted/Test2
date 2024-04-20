using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Automation.Peers
{
	// Token: 0x0200057D RID: 1405
	public class ListViewAutomationPeer : ListBoxAutomationPeer
	{
		// Token: 0x060044F2 RID: 17650 RVA: 0x00222A49 File Offset: 0x00221A49
		public ListViewAutomationPeer(ListView owner) : base(owner)
		{
			Invariant.Assert(owner != null);
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00222A5B File Offset: 0x00221A5B
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			if (this._viewAutomationPeer != null)
			{
				return this._viewAutomationPeer.GetAutomationControlType();
			}
			return base.GetAutomationControlTypeCore();
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00222A77 File Offset: 0x00221A77
		protected override string GetClassNameCore()
		{
			return "ListView";
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x00222A80 File Offset: 0x00221A80
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (this._viewAutomationPeer != null)
			{
				object pattern = this._viewAutomationPeer.GetPattern(patternInterface);
				if (pattern != null)
				{
					return pattern;
				}
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x00222AB0 File Offset: 0x00221AB0
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this._refreshItemPeers)
			{
				this._refreshItemPeers = false;
				base.ItemPeers.Clear();
			}
			List<AutomationPeer> list = base.GetChildrenCore();
			if (this._viewAutomationPeer != null)
			{
				list = this._viewAutomationPeer.GetChildren(list);
			}
			return list;
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x00222AF4 File Offset: 0x00221AF4
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			if (this._viewAutomationPeer != null)
			{
				return this._viewAutomationPeer.CreateItemAutomationPeer(item);
			}
			return base.CreateItemAutomationPeer(item);
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x060044F8 RID: 17656 RVA: 0x00222B12 File Offset: 0x00221B12
		// (set) Token: 0x060044F9 RID: 17657 RVA: 0x00222B1A File Offset: 0x00221B1A
		protected internal IViewAutomationPeer ViewAutomationPeer
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				return this._viewAutomationPeer;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				if (this._viewAutomationPeer != value)
				{
					this._refreshItemPeers = true;
				}
				this._viewAutomationPeer = value;
			}
		}

		// Token: 0x0400253E RID: 9534
		private bool _refreshItemPeers;

		// Token: 0x0400253F RID: 9535
		private IViewAutomationPeer _viewAutomationPeer;
	}
}
