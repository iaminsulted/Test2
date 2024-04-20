using System;

namespace Steamworks
{
	// Token: 0x020001D0 RID: 464
	[Serializable]
	public struct SteamInventoryResult_t : IEquatable<SteamInventoryResult_t>, IComparable<SteamInventoryResult_t>
	{
		// Token: 0x06000E4A RID: 3658 RVA: 0x0002B9A8 File Offset: 0x00029BA8
		public SteamInventoryResult_t(int value)
		{
			this.m_SteamInventoryResult = value;
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0002B9B1 File Offset: 0x00029BB1
		public override string ToString()
		{
			return this.m_SteamInventoryResult.ToString();
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0002B9BE File Offset: 0x00029BBE
		public override bool Equals(object other)
		{
			return other is SteamInventoryResult_t && this == (SteamInventoryResult_t)other;
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x0002B9DB File Offset: 0x00029BDB
		public override int GetHashCode()
		{
			return this.m_SteamInventoryResult.GetHashCode();
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0002B9E8 File Offset: 0x00029BE8
		public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0002B9F8 File Offset: 0x00029BF8
		public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0002BA04 File Offset: 0x00029C04
		public static explicit operator SteamInventoryResult_t(int value)
		{
			return new SteamInventoryResult_t(value);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0002BA0C File Offset: 0x00029C0C
		public static explicit operator int(SteamInventoryResult_t that)
		{
			return that.m_SteamInventoryResult;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0002BA14 File Offset: 0x00029C14
		public bool Equals(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0002BA24 File Offset: 0x00029C24
		public int CompareTo(SteamInventoryResult_t other)
		{
			return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
		}

		// Token: 0x04000A8A RID: 2698
		public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);

		// Token: 0x04000A8B RID: 2699
		public int m_SteamInventoryResult;
	}
}
