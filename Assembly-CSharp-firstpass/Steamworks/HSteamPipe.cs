using System;

namespace Steamworks
{
	// Token: 0x020001C1 RID: 449
	[Serializable]
	public struct HSteamPipe : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
	{
		// Token: 0x06000D8D RID: 3469 RVA: 0x0002ACE3 File Offset: 0x00028EE3
		public HSteamPipe(int value)
		{
			this.m_HSteamPipe = value;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0002ACEC File Offset: 0x00028EEC
		public override string ToString()
		{
			return this.m_HSteamPipe.ToString();
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0002ACF9 File Offset: 0x00028EF9
		public override bool Equals(object other)
		{
			return other is HSteamPipe && this == (HSteamPipe)other;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0002AD16 File Offset: 0x00028F16
		public override int GetHashCode()
		{
			return this.m_HSteamPipe.GetHashCode();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0002AD23 File Offset: 0x00028F23
		public static bool operator ==(HSteamPipe x, HSteamPipe y)
		{
			return x.m_HSteamPipe == y.m_HSteamPipe;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0002AD33 File Offset: 0x00028F33
		public static bool operator !=(HSteamPipe x, HSteamPipe y)
		{
			return !(x == y);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0002AD3F File Offset: 0x00028F3F
		public static explicit operator HSteamPipe(int value)
		{
			return new HSteamPipe(value);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002AD47 File Offset: 0x00028F47
		public static explicit operator int(HSteamPipe that)
		{
			return that.m_HSteamPipe;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0002AD4F File Offset: 0x00028F4F
		public bool Equals(HSteamPipe other)
		{
			return this.m_HSteamPipe == other.m_HSteamPipe;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0002AD5F File Offset: 0x00028F5F
		public int CompareTo(HSteamPipe other)
		{
			return this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);
		}

		// Token: 0x04000A73 RID: 2675
		public int m_HSteamPipe;
	}
}
