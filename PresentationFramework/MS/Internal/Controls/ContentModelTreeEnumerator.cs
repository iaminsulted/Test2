using System;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x0200025C RID: 604
	internal class ContentModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x06001776 RID: 6006 RVA: 0x0015E61D File Offset: 0x0015D61D
		internal ContentModelTreeEnumerator(ContentControl contentControl, object content) : base(content)
		{
			this._owner = contentControl;
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x0015E62D File Offset: 0x0015D62D
		protected override bool IsUnchanged
		{
			get
			{
				return base.Content == this._owner.Content;
			}
		}

		// Token: 0x04000CA9 RID: 3241
		private ContentControl _owner;
	}
}
