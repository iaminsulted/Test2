using System;

namespace Steamworks
{
	// Token: 0x020001D1 RID: 465
	[Serializable]
	public struct SteamItemDef_t : IEquatable<SteamItemDef_t>, IComparable<SteamItemDef_t>
	{
		// Token: 0x06000E55 RID: 3669 RVA: 0x0002BA44 File Offset: 0x00029C44
		public SteamItemDef_t(int value)
		{
			this.m_SteamItemDef = value;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0002BA4D File Offset: 0x00029C4D
		public override string ToString()
		{
			return this.m_SteamItemDef.ToString();
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0002BA5A File Offset: 0x00029C5A
		public override bool Equals(object other)
		{
			return other is SteamItemDef_t && this == (SteamItemDef_t)other;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0002BA77 File Offset: 0x00029C77
		public override int GetHashCode()
		{
			return this.m_SteamItemDef.GetHashCode();
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0002BA84 File Offset: 0x00029C84
		public static bool operator ==(SteamItemDef_t x, SteamItemDef_t y)
		{
			return x.m_SteamItemDef == y.m_SteamItemDef;
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0002BA94 File Offset: 0x00029C94
		public static bool operator !=(SteamItemDef_t x, SteamItemDef_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0002BAA0 File Offset: 0x00029CA0
		public static explicit operator SteamItemDef_t(int value)
		{
			return new SteamItemDef_t(value);
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0002BAA8 File Offset: 0x00029CA8
		public static explicit operator int(SteamItemDef_t that)
		{
			return that.m_SteamItemDef;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0002BAB0 File Offset: 0x00029CB0
		public bool Equals(SteamItemDef_t other)
		{
			return this.m_SteamItemDef == other.m_SteamItemDef;
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0002BAC0 File Offset: 0x00029CC0
		public int CompareTo(SteamItemDef_t other)
		{
			return this.m_SteamItemDef.CompareTo(other.m_SteamItemDef);
		}

		// Token: 0x04000A8C RID: 2700
		public int m_SteamItemDef;
	}
}
