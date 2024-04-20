using System;

namespace Steamworks
{
	// Token: 0x020001E5 RID: 485
	[Serializable]
	public struct SteamLeaderboard_t : IEquatable<SteamLeaderboard_t>, IComparable<SteamLeaderboard_t>
	{
		// Token: 0x06000F2B RID: 3883 RVA: 0x0002C634 File Offset: 0x0002A834
		public SteamLeaderboard_t(ulong value)
		{
			this.m_SteamLeaderboard = value;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0002C63D File Offset: 0x0002A83D
		public override string ToString()
		{
			return this.m_SteamLeaderboard.ToString();
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0002C64A File Offset: 0x0002A84A
		public override bool Equals(object other)
		{
			return other is SteamLeaderboard_t && this == (SteamLeaderboard_t)other;
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0002C667 File Offset: 0x0002A867
		public override int GetHashCode()
		{
			return this.m_SteamLeaderboard.GetHashCode();
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0002C674 File Offset: 0x0002A874
		public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return x.m_SteamLeaderboard == y.m_SteamLeaderboard;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0002C684 File Offset: 0x0002A884
		public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0002C690 File Offset: 0x0002A890
		public static explicit operator SteamLeaderboard_t(ulong value)
		{
			return new SteamLeaderboard_t(value);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0002C698 File Offset: 0x0002A898
		public static explicit operator ulong(SteamLeaderboard_t that)
		{
			return that.m_SteamLeaderboard;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0002C6A0 File Offset: 0x0002A8A0
		public bool Equals(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard == other.m_SteamLeaderboard;
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0002C6B0 File Offset: 0x0002A8B0
		public int CompareTo(SteamLeaderboard_t other)
		{
			return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
		}

		// Token: 0x04000AAF RID: 2735
		public ulong m_SteamLeaderboard;
	}
}
