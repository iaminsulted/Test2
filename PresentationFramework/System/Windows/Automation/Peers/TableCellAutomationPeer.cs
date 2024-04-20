using System;
using System.Windows.Automation.Provider;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000595 RID: 1429
	public class TableCellAutomationPeer : TextElementAutomationPeer, IGridItemProvider
	{
		// Token: 0x060045A3 RID: 17827 RVA: 0x002213E3 File Offset: 0x002203E3
		public TableCellAutomationPeer(TableCell owner) : base(owner)
		{
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x002245B4 File Offset: 0x002235B4
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.GridItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x001FBDEE File Offset: 0x001FADEE
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x002245C3 File Offset: 0x002235C3
		protected override string GetLocalizedControlTypeCore()
		{
			return "cell";
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x002245CA File Offset: 0x002235CA
		protected override string GetClassNameCore()
		{
			return "TableCell";
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x00221440 File Offset: 0x00220440
		protected override bool IsControlElementCore()
		{
			if (!base.IncludeInvisibleElementsInControlView)
			{
				bool? isTextViewVisible = base.IsTextViewVisible;
				bool flag = true;
				return isTextViewVisible.GetValueOrDefault() == flag & isTextViewVisible != null;
			}
			return true;
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x002245D1 File Offset: 0x002235D1
		internal void OnColumnSpanChanged(int oldValue, int newValue)
		{
			base.RaisePropertyChangedEvent(GridItemPatternIdentifiers.ColumnSpanProperty, oldValue, newValue);
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x002245EA File Offset: 0x002235EA
		internal void OnRowSpanChanged(int oldValue, int newValue)
		{
			base.RaisePropertyChangedEvent(GridItemPatternIdentifiers.RowSpanProperty, oldValue, newValue);
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x060045AB RID: 17835 RVA: 0x00224603 File Offset: 0x00223603
		int IGridItemProvider.Row
		{
			get
			{
				return ((TableCell)base.Owner).RowIndex;
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x060045AC RID: 17836 RVA: 0x00224615 File Offset: 0x00223615
		int IGridItemProvider.Column
		{
			get
			{
				return ((TableCell)base.Owner).ColumnIndex;
			}
		}

		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x060045AD RID: 17837 RVA: 0x00224627 File Offset: 0x00223627
		int IGridItemProvider.RowSpan
		{
			get
			{
				return ((TableCell)base.Owner).RowSpan;
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x060045AE RID: 17838 RVA: 0x00224639 File Offset: 0x00223639
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				return ((TableCell)base.Owner).ColumnSpan;
			}
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x060045AF RID: 17839 RVA: 0x0022464B File Offset: 0x0022364B
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				if ((TableCell)base.Owner != null)
				{
					return base.ProviderFromPeer(ContentElementAutomationPeer.CreatePeerForElement(((TableCell)base.Owner).Table));
				}
				return null;
			}
		}
	}
}
