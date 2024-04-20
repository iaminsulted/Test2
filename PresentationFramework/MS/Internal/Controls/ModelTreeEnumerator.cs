using System;
using System.Collections;
using System.Windows;

namespace MS.Internal.Controls
{
	// Token: 0x0200025B RID: 603
	internal abstract class ModelTreeEnumerator : IEnumerator
	{
		// Token: 0x0600176A RID: 5994 RVA: 0x0015E563 File Offset: 0x0015D563
		internal ModelTreeEnumerator(object content)
		{
			this._content = content;
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x0015E579 File Offset: 0x0015D579
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0015E581 File Offset: 0x0015D581
		bool IEnumerator.MoveNext()
		{
			return this.MoveNext();
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0015E589 File Offset: 0x0015D589
		void IEnumerator.Reset()
		{
			this.Reset();
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x0015E591 File Offset: 0x0015D591
		protected object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600176F RID: 5999 RVA: 0x0015E599 File Offset: 0x0015D599
		// (set) Token: 0x06001770 RID: 6000 RVA: 0x0015E5A1 File Offset: 0x0015D5A1
		protected int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001771 RID: 6001 RVA: 0x0015E5AA File Offset: 0x0015D5AA
		protected virtual object Current
		{
			get
			{
				if (this._index == 0)
				{
					return this._content;
				}
				throw new InvalidOperationException(SR.Get("EnumeratorInvalidOperation"));
			}
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x0015E5CA File Offset: 0x0015D5CA
		protected virtual bool MoveNext()
		{
			if (this._index < 1)
			{
				this._index++;
				if (this._index == 0)
				{
					this.VerifyUnchanged();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0015E5F4 File Offset: 0x0015D5F4
		protected virtual void Reset()
		{
			this.VerifyUnchanged();
			this._index = -1;
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001774 RID: 6004
		protected abstract bool IsUnchanged { get; }

		// Token: 0x06001775 RID: 6005 RVA: 0x0015E603 File Offset: 0x0015D603
		protected void VerifyUnchanged()
		{
			if (!this.IsUnchanged)
			{
				throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
			}
		}

		// Token: 0x04000CA7 RID: 3239
		private int _index = -1;

		// Token: 0x04000CA8 RID: 3240
		private object _content;
	}
}
