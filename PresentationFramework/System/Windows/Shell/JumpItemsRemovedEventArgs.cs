using System;
using System.Collections.Generic;

namespace System.Windows.Shell
{
	// Token: 0x020003EC RID: 1004
	public sealed class JumpItemsRemovedEventArgs : EventArgs
	{
		// Token: 0x06002B0D RID: 11021 RVA: 0x001A0E0F File Offset: 0x0019FE0F
		public JumpItemsRemovedEventArgs() : this(null)
		{
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x001A0E18 File Offset: 0x0019FE18
		public JumpItemsRemovedEventArgs(IList<JumpItem> removedItems)
		{
			if (removedItems != null)
			{
				this.RemovedItems = new List<JumpItem>(removedItems).AsReadOnly();
				return;
			}
			this.RemovedItems = new List<JumpItem>().AsReadOnly();
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06002B0F RID: 11023 RVA: 0x001A0E45 File Offset: 0x0019FE45
		// (set) Token: 0x06002B10 RID: 11024 RVA: 0x001A0E4D File Offset: 0x0019FE4D
		public IList<JumpItem> RemovedItems { get; private set; }
	}
}
