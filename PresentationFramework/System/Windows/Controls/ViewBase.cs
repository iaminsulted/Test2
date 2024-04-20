using System;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls
{
	// Token: 0x020007FF RID: 2047
	public abstract class ViewBase : DependencyObject
	{
		// Token: 0x060076FA RID: 30458 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected internal virtual void PrepareItem(ListViewItem item)
		{
		}

		// Token: 0x060076FB RID: 30459 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected internal virtual void ClearItem(ListViewItem item)
		{
		}

		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x060076FC RID: 30460 RVA: 0x002F0E9D File Offset: 0x002EFE9D
		protected internal virtual object DefaultStyleKey
		{
			get
			{
				return typeof(ListBox);
			}
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x060076FD RID: 30461 RVA: 0x002F0EA9 File Offset: 0x002EFEA9
		protected internal virtual object ItemContainerDefaultStyleKey
		{
			get
			{
				return typeof(ListBoxItem);
			}
		}

		// Token: 0x060076FE RID: 30462 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnThemeChanged()
		{
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x060076FF RID: 30463 RVA: 0x002F0EB5 File Offset: 0x002EFEB5
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06007700 RID: 30464 RVA: 0x002F0EBD File Offset: 0x002EFEBD
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext != context)
			{
				this._inheritanceContext = context;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06007701 RID: 30465 RVA: 0x002F0EDA File Offset: 0x002EFEDA
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext == context)
			{
				this._inheritanceContext = null;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06007702 RID: 30466 RVA: 0x00109403 File Offset: 0x00108403
		protected internal virtual IViewAutomationPeer GetAutomationPeer(ListView parent)
		{
			return null;
		}

		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x06007703 RID: 30467 RVA: 0x002F0EF7 File Offset: 0x002EFEF7
		// (set) Token: 0x06007704 RID: 30468 RVA: 0x002F0EFF File Offset: 0x002EFEFF
		internal bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				this._isUsed = value;
			}
		}

		// Token: 0x040038BE RID: 14526
		private DependencyObject _inheritanceContext;

		// Token: 0x040038BF RID: 14527
		private bool _isUsed;
	}
}
