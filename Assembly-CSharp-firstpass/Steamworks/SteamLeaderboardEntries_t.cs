using System;

namespace Steamworks
{
	// Token: 0x020001E4 RID: 484
	[Serializable]
	public struct SteamLeaderboardEntries_t : IEquatable<SteamLeaderboardEntries_t>, IComparable<SteamLeaderboardEntries_t>
	{
		// Token: 0x06000F21 RID: 3873 RVA: 0x0002C5A5 File Offset: 0x0002A7A5
		public SteamLeaderboardEntries_t(ulong value)
		{
			this.m_SteamLeaderboardEntries = value;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0002C5AE File Offset: 0x0002A7AE
		public override string ToString()
		{
			return this.m_SteamLeaderboardEntries.ToString();
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0002C5BB File Offset: 0x0002A7BB
		public override bool Equals(object other)
		{
			return other is SteamLeaderboardEntries_t && this == (SteamLeaderboardEntries_t)other;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x0002C5D8 File Offset: 0x0002A7D8
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboardEntries.GetHashCode();
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0002C5E5 File Offset: 0x0002A7E5
		public static bool operator ==(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return x.m_SteamLeaderboardEntries == y.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0002C5F5 File Offset: 0x0002A7F5
		public static bool operator !=(SteamLeaderboardEntries_t x, SteamLeaderboardEntries_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x0002C601 File Offset: 0x0002A801
		public static explicit operator SteamLeaderboardEntries_t(ulong value)
		{
			return new SteamLeaderboardEntries_t(value);
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0002C609 File Offset: 0x0002A809
		public static explicit operator ulong(SteamLeaderboardEntries_t that)
		{
			return that.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0002C611 File Offset: 0x0002A811
		public bool Equals(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries == other.m_SteamLeaderboardEntries;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0002C621 File Offset: 0x0002A821
		public int CompareTo(SteamLeaderboardEntries_t other)
		{
			return this.m_SteamLeaderboardEntries.CompareTo(other.m_SteamLeaderboardEntries);
		}

		// Token: 0x04000AAE RID: 2734
		public ulong m_SteamLeaderboardEntries;
	}
}
