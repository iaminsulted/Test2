using System;

namespace Steamworks
{
	// Token: 0x020001D6 RID: 470
	[Serializable]
	public struct SNetSocket_t : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
	{
		// Token: 0x06000E89 RID: 3721 RVA: 0x0002BD2E File Offset: 0x00029F2E
		public SNetSocket_t(uint value)
		{
			this.m_SNetSocket = value;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0002BD37 File Offset: 0x00029F37
		public override string ToString()
		{
			return this.m_SNetSocket.ToString();
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0002BD44 File Offset: 0x00029F44
		public override bool Equals(object other)
		{
			return other is SNetSocket_t && this == (SNetSocket_t)other;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0002BD61 File Offset: 0x00029F61
		public override int GetHashCode()
		{
			return this.m_SNetSocket.GetHashCode();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0002BD6E File Offset: 0x00029F6E
		public static bool operator ==(SNetSocket_t x, SNetSocket_t y)
		{
			return x.m_SNetSocket == y.m_SNetSocket;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0002BD7E File Offset: 0x00029F7E
		public static bool operator !=(SNetSocket_t x, SNetSocket_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0002BD8A File Offset: 0x00029F8A
		public static explicit operator SNetSocket_t(uint value)
		{
			return new SNetSocket_t(value);
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0002BD92 File Offset: 0x00029F92
		public static explicit operator uint(SNetSocket_t that)
		{
			return that.m_SNetSocket;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0002BD9A File Offset: 0x00029F9A
		public bool Equals(SNetSocket_t other)
		{
			return this.m_SNetSocket == other.m_SNetSocket;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0002BDAA File Offset: 0x00029FAA
		public int CompareTo(SNetSocket_t other)
		{
			return this.m_SNetSocket.CompareTo(other.m_SNetSocket);
		}

		// Token: 0x04000A94 RID: 2708
		public uint m_SNetSocket;
	}
}
