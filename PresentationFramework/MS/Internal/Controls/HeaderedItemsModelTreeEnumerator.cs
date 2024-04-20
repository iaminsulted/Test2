using System;
using System.Collections;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x0200025E RID: 606
	internal class HeaderedItemsModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x0600177C RID: 6012 RVA: 0x0015E6FC File Offset: 0x0015D6FC
		internal HeaderedItemsModelTreeEnumerator(HeaderedItemsControl headeredItemsControl, IEnumerator items, object header) : base(header)
		{
			this._owner = headeredItemsControl;
			this._items = items;
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0015E713 File Offset: 0x0015D713
		protected override object Current
		{
			get
			{
				if (base.Index > 0)
				{
					return this._items.Current;
				}
				return base.Current;
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x0015E730 File Offset: 0x0015D730
		protected override bool MoveNext()
		{
			if (base.Index >= 0)
			{
				int index = base.Index;
				base.Index = index + 1;
				return this._items.MoveNext();
			}
			return base.MoveNext();
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x0015E768 File Offset: 0x0015D768
		protected override void Reset()
		{
			base.Reset();
			this._items.Reset();
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001780 RID: 6016 RVA: 0x0015E77B File Offset: 0x0015D77B
		protected override bool IsUnchanged
		{
			get
			{
				return base.Content == this._owner.Header;
			}
		}

		// Token: 0x04000CAC RID: 3244
		private HeaderedItemsControl _owner;

		// Token: 0x04000CAD RID: 3245
		private IEnumerator _items;
	}
}
