using System;
using System.Collections.Generic;

namespace System.Windows.Shell
{
	// Token: 0x020003EB RID: 1003
	public sealed class JumpItemsRejectedEventArgs : EventArgs
	{
		// Token: 0x06002B07 RID: 11015 RVA: 0x001A0D5A File Offset: 0x0019FD5A
		public JumpItemsRejectedEventArgs() : this(null, null)
		{
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x001A0D64 File Offset: 0x0019FD64
		public JumpItemsRejectedEventArgs(IList<JumpItem> rejectedItems, IList<JumpItemRejectionReason> reasons)
		{
			if ((rejectedItems == null && reasons != null) || (reasons == null && rejectedItems != null) || (rejectedItems != null && reasons != null && rejectedItems.Count != reasons.Count))
			{
				throw new ArgumentException(SR.Get("JumpItemsRejectedEventArgs_CountMismatch"));
			}
			if (rejectedItems != null)
			{
				this.RejectedItems = new List<JumpItem>(rejectedItems).AsReadOnly();
				this.RejectionReasons = new List<JumpItemRejectionReason>(reasons).AsReadOnly();
				return;
			}
			this.RejectedItems = new List<JumpItem>().AsReadOnly();
			this.RejectionReasons = new List<JumpItemRejectionReason>().AsReadOnly();
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06002B09 RID: 11017 RVA: 0x001A0DED File Offset: 0x0019FDED
		// (set) Token: 0x06002B0A RID: 11018 RVA: 0x001A0DF5 File Offset: 0x0019FDF5
		public IList<JumpItem> RejectedItems { get; private set; }

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06002B0B RID: 11019 RVA: 0x001A0DFE File Offset: 0x0019FDFE
		// (set) Token: 0x06002B0C RID: 11020 RVA: 0x001A0E06 File Offset: 0x0019FE06
		public IList<JumpItemRejectionReason> RejectionReasons { get; private set; }
	}
}
