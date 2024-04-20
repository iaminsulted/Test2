using System;

namespace Steamworks
{
	// Token: 0x020001C2 RID: 450
	[Serializable]
	public struct HSteamUser : IEquatable<HSteamUser>, IComparable<HSteamUser>
	{
		// Token: 0x06000D97 RID: 3479 RVA: 0x0002AD72 File Offset: 0x00028F72
		public HSteamUser(int value)
		{
			this.m_HSteamUser = value;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0002AD7B File Offset: 0x00028F7B
		public override string ToString()
		{
			return this.m_HSteamUser.ToString();
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0002AD88 File Offset: 0x00028F88
		public override bool Equals(object other)
		{
			return other is HSteamUser && this == (HSteamUser)other;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0002ADA5 File Offset: 0x00028FA5
		public override int GetHashCode()
		{
			return this.m_HSteamUser.GetHashCode();
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002ADB2 File Offset: 0x00028FB2
		public static bool operator ==(HSteamUser x, HSteamUser y)
		{
			return x.m_HSteamUser == y.m_HSteamUser;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0002ADC2 File Offset: 0x00028FC2
		public static bool operator !=(HSteamUser x, HSteamUser y)
		{
			return !(x == y);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0002ADCE File Offset: 0x00028FCE
		public static explicit operator HSteamUser(int value)
		{
			return new HSteamUser(value);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002ADD6 File Offset: 0x00028FD6
		public static explicit operator int(HSteamUser that)
		{
			return that.m_HSteamUser;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0002ADDE File Offset: 0x00028FDE
		public bool Equals(HSteamUser other)
		{
			return this.m_HSteamUser == other.m_HSteamUser;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0002ADEE File Offset: 0x00028FEE
		public int CompareTo(HSteamUser other)
		{
			return this.m_HSteamUser.CompareTo(other.m_HSteamUser);
		}

		// Token: 0x04000A74 RID: 2676
		public int m_HSteamUser;
	}
}
