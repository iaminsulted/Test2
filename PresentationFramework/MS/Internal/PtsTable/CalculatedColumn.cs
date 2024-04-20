using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsTable
{
	// Token: 0x02000108 RID: 264
	internal struct CalculatedColumn
	{
		// Token: 0x06000681 RID: 1665 RVA: 0x00108CF4 File Offset: 0x00107CF4
		internal void ValidateAuto(double durMinWidth, double durMaxWidth)
		{
			this._durMinWidth = durMinWidth;
			this._durMaxWidth = durMaxWidth;
			this.SetFlags(true, CalculatedColumn.Flags.ValidAutofit);
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x00108D0C File Offset: 0x00107D0C
		internal int PtsWidthChanged
		{
			get
			{
				return PTS.FromBoolean(!this.CheckFlags(CalculatedColumn.Flags.ValidWidth));
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00108D1D File Offset: 0x00107D1D
		internal double DurMinWidth
		{
			get
			{
				return this._durMinWidth;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x00108D25 File Offset: 0x00107D25
		internal double DurMaxWidth
		{
			get
			{
				return this._durMaxWidth;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000685 RID: 1669 RVA: 0x00108D2D File Offset: 0x00107D2D
		// (set) Token: 0x06000686 RID: 1670 RVA: 0x00108D35 File Offset: 0x00107D35
		internal GridLength UserWidth
		{
			get
			{
				return this._userWidth;
			}
			set
			{
				if (this._userWidth != value)
				{
					this.SetFlags(false, CalculatedColumn.Flags.ValidAutofit);
				}
				this._userWidth = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00108D54 File Offset: 0x00107D54
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00108D5C File Offset: 0x00107D5C
		internal double DurWidth
		{
			get
			{
				return this._durWidth;
			}
			set
			{
				if (!DoubleUtil.AreClose(this._durWidth, value))
				{
					this.SetFlags(false, CalculatedColumn.Flags.ValidWidth);
				}
				this._durWidth = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00108D7B File Offset: 0x00107D7B
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x00108D83 File Offset: 0x00107D83
		internal double UrOffset
		{
			get
			{
				return this._urOffset;
			}
			set
			{
				this._urOffset = value;
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00108D8C File Offset: 0x00107D8C
		private void SetFlags(bool value, CalculatedColumn.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00108DAA File Offset: 0x00107DAA
		private bool CheckFlags(CalculatedColumn.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x04000704 RID: 1796
		private GridLength _userWidth;

		// Token: 0x04000705 RID: 1797
		private double _durWidth;

		// Token: 0x04000706 RID: 1798
		private double _durMinWidth;

		// Token: 0x04000707 RID: 1799
		private double _durMaxWidth;

		// Token: 0x04000708 RID: 1800
		private double _urOffset;

		// Token: 0x04000709 RID: 1801
		private CalculatedColumn.Flags _flags;

		// Token: 0x020008BE RID: 2238
		[Flags]
		private enum Flags
		{
			// Token: 0x04003C28 RID: 15400
			ValidWidth = 1,
			// Token: 0x04003C29 RID: 15401
			ValidAutofit = 2
		}
	}
}
