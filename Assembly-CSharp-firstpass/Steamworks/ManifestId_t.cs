using System;

namespace Steamworks
{
	// Token: 0x020001DF RID: 479
	[Serializable]
	public struct ManifestId_t : IEquatable<ManifestId_t>, IComparable<ManifestId_t>
	{
		// Token: 0x06000EEA RID: 3818 RVA: 0x0002C294 File Offset: 0x0002A494
		public ManifestId_t(ulong value)
		{
			this.m_ManifestId = value;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0002C29D File Offset: 0x0002A49D
		public override string ToString()
		{
			return this.m_ManifestId.ToString();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0002C2AA File Offset: 0x0002A4AA
		public override bool Equals(object other)
		{
			return other is ManifestId_t && this == (ManifestId_t)other;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0002C2C7 File Offset: 0x0002A4C7
		public override int GetHashCode()
		{
			return this.m_ManifestId.GetHashCode();
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0002C2D4 File Offset: 0x0002A4D4
		public static bool operator ==(ManifestId_t x, ManifestId_t y)
		{
			return x.m_ManifestId == y.m_ManifestId;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0002C2E4 File Offset: 0x0002A4E4
		public static bool operator !=(ManifestId_t x, ManifestId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0002C2F0 File Offset: 0x0002A4F0
		public static explicit operator ManifestId_t(ulong value)
		{
			return new ManifestId_t(value);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0002C2F8 File Offset: 0x0002A4F8
		public static explicit operator ulong(ManifestId_t that)
		{
			return that.m_ManifestId;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0002C300 File Offset: 0x0002A500
		public bool Equals(ManifestId_t other)
		{
			return this.m_ManifestId == other.m_ManifestId;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0002C310 File Offset: 0x0002A510
		public int CompareTo(ManifestId_t other)
		{
			return this.m_ManifestId.CompareTo(other.m_ManifestId);
		}

		// Token: 0x04000AA4 RID: 2724
		public static readonly ManifestId_t Invalid = new ManifestId_t(0UL);

		// Token: 0x04000AA5 RID: 2725
		public ulong m_ManifestId;
	}
}
