using System;

namespace Steamworks
{
	// Token: 0x020001C0 RID: 448
	[Serializable]
	public struct servernetadr_t
	{
		// Token: 0x06000D7B RID: 3451 RVA: 0x0002AABE File Offset: 0x00028CBE
		public void Init(uint ip, ushort usQueryPort, ushort usConnectionPort)
		{
			this.m_unIP = ip;
			this.m_usQueryPort = usQueryPort;
			this.m_usConnectionPort = usConnectionPort;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0002AAD5 File Offset: 0x00028CD5
		public ushort GetQueryPort()
		{
			return this.m_usQueryPort;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0002AADD File Offset: 0x00028CDD
		public void SetQueryPort(ushort usPort)
		{
			this.m_usQueryPort = usPort;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0002AAE6 File Offset: 0x00028CE6
		public ushort GetConnectionPort()
		{
			return this.m_usConnectionPort;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0002AAEE File Offset: 0x00028CEE
		public void SetConnectionPort(ushort usPort)
		{
			this.m_usConnectionPort = usPort;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0002AAF7 File Offset: 0x00028CF7
		public uint GetIP()
		{
			return this.m_unIP;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0002AAFF File Offset: 0x00028CFF
		public void SetIP(uint unIP)
		{
			this.m_unIP = unIP;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0002AB08 File Offset: 0x00028D08
		public string GetConnectionAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usConnectionPort);
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0002AB1B File Offset: 0x00028D1B
		public string GetQueryAddressString()
		{
			return servernetadr_t.ToString(this.m_unIP, this.m_usQueryPort);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0002AB30 File Offset: 0x00028D30
		public static string ToString(uint unIP, ushort usPort)
		{
			return string.Format("{0}.{1}.{2}.{3}:{4}", new object[]
			{
				(ulong)(unIP >> 24) & 255UL,
				(ulong)(unIP >> 16) & 255UL,
				(ulong)(unIP >> 8) & 255UL,
				(ulong)unIP & 255UL,
				usPort
			});
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002ABA2 File Offset: 0x00028DA2
		public static bool operator <(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP < y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort < y.m_usQueryPort);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0002ABD2 File Offset: 0x00028DD2
		public static bool operator >(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP > y.m_unIP || (x.m_unIP == y.m_unIP && x.m_usQueryPort > y.m_usQueryPort);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0002AC02 File Offset: 0x00028E02
		public override bool Equals(object other)
		{
			return other is servernetadr_t && this == (servernetadr_t)other;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002AC1F File Offset: 0x00028E1F
		public override int GetHashCode()
		{
			return this.m_unIP.GetHashCode() + this.m_usQueryPort.GetHashCode() + this.m_usConnectionPort.GetHashCode();
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0002AC44 File Offset: 0x00028E44
		public static bool operator ==(servernetadr_t x, servernetadr_t y)
		{
			return x.m_unIP == y.m_unIP && x.m_usQueryPort == y.m_usQueryPort && x.m_usConnectionPort == y.m_usConnectionPort;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x0002AC72 File Offset: 0x00028E72
		public static bool operator !=(servernetadr_t x, servernetadr_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0002AC7E File Offset: 0x00028E7E
		public bool Equals(servernetadr_t other)
		{
			return this.m_unIP == other.m_unIP && this.m_usQueryPort == other.m_usQueryPort && this.m_usConnectionPort == other.m_usConnectionPort;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0002ACAC File Offset: 0x00028EAC
		public int CompareTo(servernetadr_t other)
		{
			return this.m_unIP.CompareTo(other.m_unIP) + this.m_usQueryPort.CompareTo(other.m_usQueryPort) + this.m_usConnectionPort.CompareTo(other.m_usConnectionPort);
		}

		// Token: 0x04000A70 RID: 2672
		private ushort m_usConnectionPort;

		// Token: 0x04000A71 RID: 2673
		private ushort m_usQueryPort;

		// Token: 0x04000A72 RID: 2674
		private uint m_unIP;
	}
}
