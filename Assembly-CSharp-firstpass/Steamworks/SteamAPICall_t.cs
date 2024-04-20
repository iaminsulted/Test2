using System;

namespace Steamworks
{
	// Token: 0x020001E0 RID: 480
	[Serializable]
	public struct SteamAPICall_t : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x0002C331 File Offset: 0x0002A531
		public SteamAPICall_t(ulong value)
		{
			this.m_SteamAPICall = value;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0002C33A File Offset: 0x0002A53A
		public override string ToString()
		{
			return this.m_SteamAPICall.ToString();
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0002C347 File Offset: 0x0002A547
		public override bool Equals(object other)
		{
			return other is SteamAPICall_t && this == (SteamAPICall_t)other;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0002C364 File Offset: 0x0002A564
		public override int GetHashCode()
		{
			return this.m_SteamAPICall.GetHashCode();
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0002C371 File Offset: 0x0002A571
		public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y)
		{
			return x.m_SteamAPICall == y.m_SteamAPICall;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0002C381 File Offset: 0x0002A581
		public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0002C38D File Offset: 0x0002A58D
		public static explicit operator SteamAPICall_t(ulong value)
		{
			return new SteamAPICall_t(value);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0002C395 File Offset: 0x0002A595
		public static explicit operator ulong(SteamAPICall_t that)
		{
			return that.m_SteamAPICall;
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0002C39D File Offset: 0x0002A59D
		public bool Equals(SteamAPICall_t other)
		{
			return this.m_SteamAPICall == other.m_SteamAPICall;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0002C3AD File Offset: 0x0002A5AD
		public int CompareTo(SteamAPICall_t other)
		{
			return this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
		}

		// Token: 0x04000AA6 RID: 2726
		public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);

		// Token: 0x04000AA7 RID: 2727
		public ulong m_SteamAPICall;
	}
}
