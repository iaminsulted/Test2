using System;
using System.Collections.Specialized;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000842 RID: 2114
	public class ItemsChangedEventArgs : EventArgs
	{
		// Token: 0x06007BC8 RID: 31688 RVA: 0x0030C7F4 File Offset: 0x0030B7F4
		internal ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, GeneratorPosition oldPosition, int itemCount, int itemUICount)
		{
			this._action = action;
			this._position = position;
			this._oldPosition = oldPosition;
			this._itemCount = itemCount;
			this._itemUICount = itemUICount;
		}

		// Token: 0x06007BC9 RID: 31689 RVA: 0x0030C821 File Offset: 0x0030B821
		internal ItemsChangedEventArgs(NotifyCollectionChangedAction action, GeneratorPosition position, int itemCount, int itemUICount) : this(action, position, new GeneratorPosition(-1, 0), itemCount, itemUICount)
		{
		}

		// Token: 0x17001C9F RID: 7327
		// (get) Token: 0x06007BCA RID: 31690 RVA: 0x0030C835 File Offset: 0x0030B835
		public NotifyCollectionChangedAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x17001CA0 RID: 7328
		// (get) Token: 0x06007BCB RID: 31691 RVA: 0x0030C83D File Offset: 0x0030B83D
		public GeneratorPosition Position
		{
			get
			{
				return this._position;
			}
		}

		// Token: 0x17001CA1 RID: 7329
		// (get) Token: 0x06007BCC RID: 31692 RVA: 0x0030C845 File Offset: 0x0030B845
		public GeneratorPosition OldPosition
		{
			get
			{
				return this._oldPosition;
			}
		}

		// Token: 0x17001CA2 RID: 7330
		// (get) Token: 0x06007BCD RID: 31693 RVA: 0x0030C84D File Offset: 0x0030B84D
		public int ItemCount
		{
			get
			{
				return this._itemCount;
			}
		}

		// Token: 0x17001CA3 RID: 7331
		// (get) Token: 0x06007BCE RID: 31694 RVA: 0x0030C855 File Offset: 0x0030B855
		public int ItemUICount
		{
			get
			{
				return this._itemUICount;
			}
		}

		// Token: 0x04003A57 RID: 14935
		private NotifyCollectionChangedAction _action;

		// Token: 0x04003A58 RID: 14936
		private GeneratorPosition _position;

		// Token: 0x04003A59 RID: 14937
		private GeneratorPosition _oldPosition;

		// Token: 0x04003A5A RID: 14938
		private int _itemCount;

		// Token: 0x04003A5B RID: 14939
		private int _itemUICount;
	}
}
