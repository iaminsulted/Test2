using System;

namespace Steamworks
{
	// Token: 0x020001D5 RID: 469
	[Serializable]
	public struct SNetListenSocket_t : IEquatable<SNetListenSocket_t>, IComparable<SNetListenSocket_t>
	{
		// Token: 0x06000E7F RID: 3711 RVA: 0x0002BC9F File Offset: 0x00029E9F
		public SNetListenSocket_t(uint value)
		{
			this.m_SNetListenSocket = value;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0002BCA8 File Offset: 0x00029EA8
		public override string ToString()
		{
			return this.m_SNetListenSocket.ToString();
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0002BCB5 File Offset: 0x00029EB5
		public override bool Equals(object other)
		{
			return other is SNetListenSocket_t && this == (SNetListenSocket_t)other;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0002BCD2 File Offset: 0x00029ED2
		public override int GetHashCode()
		{
			return this.m_SNetListenSocket.GetHashCode();
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0002BCDF File Offset: 0x00029EDF
		public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return x.m_SNetListenSocket == y.m_SNetListenSocket;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0002BCEF File Offset: 0x00029EEF
		public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0002BCFB File Offset: 0x00029EFB
		public static explicit operator SNetListenSocket_t(uint value)
		{
			return new SNetListenSocket_t(value);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0002BD03 File Offset: 0x00029F03
		public static explicit operator uint(SNetListenSocket_t that)
		{
			return that.m_SNetListenSocket;
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0002BD0B File Offset: 0x00029F0B
		public bool Equals(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket == other.m_SNetListenSocket;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0002BD1B File Offset: 0x00029F1B
		public int CompareTo(SNetListenSocket_t other)
		{
			return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
		}

		// Token: 0x04000A93 RID: 2707
		public uint m_SNetListenSocket;
	}
}
