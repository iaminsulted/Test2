using System;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x0200025D RID: 605
	internal class HeaderedContentModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x06001778 RID: 6008 RVA: 0x0015E642 File Offset: 0x0015D642
		internal HeaderedContentModelTreeEnumerator(HeaderedContentControl headeredContentControl, object content, object header) : base(header)
		{
			this._owner = headeredContentControl;
			this._content = content;
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x0015E659 File Offset: 0x0015D659
		protected override object Current
		{
			get
			{
				if (base.Index == 1 && this._content != null)
				{
					return this._content;
				}
				return base.Current;
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x0015E67C File Offset: 0x0015D67C
		protected override bool MoveNext()
		{
			if (this._content != null)
			{
				if (base.Index == 0)
				{
					int index = base.Index;
					base.Index = index + 1;
					base.VerifyUnchanged();
					return true;
				}
				if (base.Index == 1)
				{
					int index = base.Index;
					base.Index = index + 1;
					return false;
				}
			}
			return base.MoveNext();
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600177B RID: 6011 RVA: 0x0015E6D2 File Offset: 0x0015D6D2
		protected override bool IsUnchanged
		{
			get
			{
				return base.Content == this._owner.Header && this._content == this._owner.Content;
			}
		}

		// Token: 0x04000CAA RID: 3242
		private HeaderedContentControl _owner;

		// Token: 0x04000CAB RID: 3243
		private object _content;
	}
}
