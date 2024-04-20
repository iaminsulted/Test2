using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005A RID: 90
	[StructLayout(LayoutKind.Sequential)]
	internal class RefRECT
	{
		// Token: 0x0600007D RID: 125 RVA: 0x000F7E6D File Offset: 0x000F6E6D
		public RefRECT(int left, int top, int right, int bottom)
		{
			this._left = left;
			this._top = top;
			this._right = right;
			this._bottom = bottom;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000F7E92 File Offset: 0x000F6E92
		public int Width
		{
			get
			{
				return this._right - this._left;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000F7EA1 File Offset: 0x000F6EA1
		public int Height
		{
			get
			{
				return this._bottom - this._top;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000F7EB0 File Offset: 0x000F6EB0
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000F7EB8 File Offset: 0x000F6EB8
		public int Left
		{
			get
			{
				return this._left;
			}
			set
			{
				this._left = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000F7EC1 File Offset: 0x000F6EC1
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000F7EC9 File Offset: 0x000F6EC9
		public int Right
		{
			get
			{
				return this._right;
			}
			set
			{
				this._right = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000F7ED2 File Offset: 0x000F6ED2
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000F7EDA File Offset: 0x000F6EDA
		public int Top
		{
			get
			{
				return this._top;
			}
			set
			{
				this._top = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000F7EE3 File Offset: 0x000F6EE3
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000F7EEB File Offset: 0x000F6EEB
		public int Bottom
		{
			get
			{
				return this._bottom;
			}
			set
			{
				this._bottom = value;
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000F7EF4 File Offset: 0x000F6EF4
		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}

		// Token: 0x0400048C RID: 1164
		private int _left;

		// Token: 0x0400048D RID: 1165
		private int _top;

		// Token: 0x0400048E RID: 1166
		private int _right;

		// Token: 0x0400048F RID: 1167
		private int _bottom;
	}
}
