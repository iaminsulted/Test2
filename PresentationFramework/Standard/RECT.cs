using System;

namespace Standard
{
	// Token: 0x02000059 RID: 89
	internal struct RECT
	{
		// Token: 0x0600006D RID: 109 RVA: 0x000F7C5B File Offset: 0x000F6C5B
		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000F7C95 File Offset: 0x000F6C95
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000F7C9D File Offset: 0x000F6C9D
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000F7CA6 File Offset: 0x000F6CA6
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000F7CAE File Offset: 0x000F6CAE
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000F7CB7 File Offset: 0x000F6CB7
		// (set) Token: 0x06000073 RID: 115 RVA: 0x000F7CBF File Offset: 0x000F6CBF
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

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000F7CC8 File Offset: 0x000F6CC8
		// (set) Token: 0x06000075 RID: 117 RVA: 0x000F7CD0 File Offset: 0x000F6CD0
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000076 RID: 118 RVA: 0x000F7CD9 File Offset: 0x000F6CD9
		public int Width
		{
			get
			{
				return this._right - this._left;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000F7CE8 File Offset: 0x000F6CE8
		public int Height
		{
			get
			{
				return this._bottom - this._top;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000078 RID: 120 RVA: 0x000F7CF8 File Offset: 0x000F6CF8
		public POINT Position
		{
			get
			{
				return new POINT
				{
					x = this._left,
					y = this._top
				};
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000F7D28 File Offset: 0x000F6D28
		public SIZE Size
		{
			get
			{
				return new SIZE
				{
					cx = this.Width,
					cy = this.Height
				};
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000F7D58 File Offset: 0x000F6D58
		public static RECT Union(RECT rect1, RECT rect2)
		{
			return new RECT
			{
				Left = Math.Min(rect1.Left, rect2.Left),
				Top = Math.Min(rect1.Top, rect2.Top),
				Right = Math.Max(rect1.Right, rect2.Right),
				Bottom = Math.Max(rect1.Bottom, rect2.Bottom)
			};
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000F7DD8 File Offset: 0x000F6DD8
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				RECT rect = (RECT)obj;
				result = (rect._bottom == this._bottom && rect._left == this._left && rect._right == this._right && rect._top == this._top);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000F7E40 File Offset: 0x000F6E40
		public override int GetHashCode()
		{
			return (this._left << 16 | Utility.LOWORD(this._right)) ^ (this._top << 16 | Utility.LOWORD(this._bottom));
		}

		// Token: 0x04000488 RID: 1160
		private int _left;

		// Token: 0x04000489 RID: 1161
		private int _top;

		// Token: 0x0400048A RID: 1162
		private int _right;

		// Token: 0x0400048B RID: 1163
		private int _bottom;
	}
}
