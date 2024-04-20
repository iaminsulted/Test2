using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x0200063F RID: 1599
	public sealed class LinkTargetCollection : CollectionBase
	{
		// Token: 0x17001254 RID: 4692
		public LinkTarget this[int index]
		{
			get
			{
				return (LinkTarget)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x00243535 File Offset: 0x00242535
		public int Add(LinkTarget value)
		{
			return ((IList)this).Add(value);
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x0024353E File Offset: 0x0024253E
		public void Remove(LinkTarget value)
		{
			((IList)this).Remove(value);
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x00243547 File Offset: 0x00242547
		public bool Contains(LinkTarget value)
		{
			return ((IList)this).Contains(value);
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x00243550 File Offset: 0x00242550
		public void CopyTo(LinkTarget[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x0024355A File Offset: 0x0024255A
		public int IndexOf(LinkTarget value)
		{
			return ((IList)this).IndexOf(value);
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x00243563 File Offset: 0x00242563
		public void Insert(int index, LinkTarget value)
		{
			((IList)this).Insert(index, value);
		}
	}
}
