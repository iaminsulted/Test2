using System;

namespace System.Windows.Documents
{
	// Token: 0x020005DB RID: 1499
	public class BlockCollection : TextElementCollection<Block>
	{
		// Token: 0x0600487E RID: 18558 RVA: 0x0022C7F7 File Offset: 0x0022B7F7
		internal BlockCollection(DependencyObject owner, bool isOwnerParent) : base(owner, isOwnerParent)
		{
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x0600487F RID: 18559 RVA: 0x0022C801 File Offset: 0x0022B801
		public Block FirstBlock
		{
			get
			{
				return base.FirstChild;
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06004880 RID: 18560 RVA: 0x0022C809 File Offset: 0x0022B809
		public Block LastBlock
		{
			get
			{
				return base.LastChild;
			}
		}
	}
}
