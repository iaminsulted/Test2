using System;

namespace Steamworks
{
	// Token: 0x020001D2 RID: 466
	[Serializable]
	public struct SteamItemInstanceID_t : IEquatable<SteamItemInstanceID_t>, IComparable<SteamItemInstanceID_t>
	{
		// Token: 0x06000E5F RID: 3679 RVA: 0x0002BAD3 File Offset: 0x00029CD3
		public SteamItemInstanceID_t(ulong value)
		{
			this.m_SteamItemInstanceID = value;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0002BADC File Offset: 0x00029CDC
		public override string ToString()
		{
			return this.m_SteamItemInstanceID.ToString();
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0002BAE9 File Offset: 0x00029CE9
		public override bool Equals(object other)
		{
			return other is SteamItemInstanceID_t && this == (SteamItemInstanceID_t)other;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0002BB06 File Offset: 0x00029D06
		public override int GetHashCode()
		{
			return this.m_SteamItemInstanceID.GetHashCode();
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0002BB13 File Offset: 0x00029D13
		public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return x.m_SteamItemInstanceID == y.m_SteamItemInstanceID;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0002BB23 File Offset: 0x00029D23
		public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0002BB2F File Offset: 0x00029D2F
		public static explicit operator SteamItemInstanceID_t(ulong value)
		{
			return new SteamItemInstanceID_t(value);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0002BB37 File Offset: 0x00029D37
		public static explicit operator ulong(SteamItemInstanceID_t that)
		{
			return that.m_SteamItemInstanceID;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0002BB3F File Offset: 0x00029D3F
		public bool Equals(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID == other.m_SteamItemInstanceID;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0002BB4F File Offset: 0x00029D4F
		public int CompareTo(SteamItemInstanceID_t other)
		{
			return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
		}

		// Token: 0x04000A8D RID: 2701
		public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);

		// Token: 0x04000A8E RID: 2702
		public ulong m_SteamItemInstanceID;
	}
}
