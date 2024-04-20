using System;

namespace System.Windows.Controls
{
	// Token: 0x020007E6 RID: 2022
	public class TextChange
	{
		// Token: 0x06007532 RID: 30002 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		internal TextChange()
		{
		}

		// Token: 0x17001B39 RID: 6969
		// (get) Token: 0x06007533 RID: 30003 RVA: 0x002EAFFA File Offset: 0x002E9FFA
		// (set) Token: 0x06007534 RID: 30004 RVA: 0x002EB002 File Offset: 0x002EA002
		public int Offset
		{
			get
			{
				return this._offset;
			}
			internal set
			{
				this._offset = value;
			}
		}

		// Token: 0x17001B3A RID: 6970
		// (get) Token: 0x06007535 RID: 30005 RVA: 0x002EB00B File Offset: 0x002EA00B
		// (set) Token: 0x06007536 RID: 30006 RVA: 0x002EB013 File Offset: 0x002EA013
		public int AddedLength
		{
			get
			{
				return this._addedLength;
			}
			internal set
			{
				this._addedLength = value;
			}
		}

		// Token: 0x17001B3B RID: 6971
		// (get) Token: 0x06007537 RID: 30007 RVA: 0x002EB01C File Offset: 0x002EA01C
		// (set) Token: 0x06007538 RID: 30008 RVA: 0x002EB024 File Offset: 0x002EA024
		public int RemovedLength
		{
			get
			{
				return this._removedLength;
			}
			internal set
			{
				this._removedLength = value;
			}
		}

		// Token: 0x04003831 RID: 14385
		private int _offset;

		// Token: 0x04003832 RID: 14386
		private int _addedLength;

		// Token: 0x04003833 RID: 14387
		private int _removedLength;
	}
}
