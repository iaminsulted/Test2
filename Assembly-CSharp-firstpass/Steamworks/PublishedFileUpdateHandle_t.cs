using System;

namespace Steamworks
{
	// Token: 0x020001D8 RID: 472
	[Serializable]
	public struct PublishedFileUpdateHandle_t : IEquatable<PublishedFileUpdateHandle_t>, IComparable<PublishedFileUpdateHandle_t>
	{
		// Token: 0x06000E9E RID: 3742 RVA: 0x0002BE5A File Offset: 0x0002A05A
		public PublishedFileUpdateHandle_t(ulong value)
		{
			this.m_PublishedFileUpdateHandle = value;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0002BE63 File Offset: 0x0002A063
		public override string ToString()
		{
			return this.m_PublishedFileUpdateHandle.ToString();
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0002BE70 File Offset: 0x0002A070
		public override bool Equals(object other)
		{
			return other is PublishedFileUpdateHandle_t && this == (PublishedFileUpdateHandle_t)other;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0002BE8D File Offset: 0x0002A08D
		public override int GetHashCode()
		{
			return this.m_PublishedFileUpdateHandle.GetHashCode();
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0002BE9A File Offset: 0x0002A09A
		public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return x.m_PublishedFileUpdateHandle == y.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0002BEAA File Offset: 0x0002A0AA
		public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0002BEB6 File Offset: 0x0002A0B6
		public static explicit operator PublishedFileUpdateHandle_t(ulong value)
		{
			return new PublishedFileUpdateHandle_t(value);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0002BEBE File Offset: 0x0002A0BE
		public static explicit operator ulong(PublishedFileUpdateHandle_t that)
		{
			return that.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0002BEC6 File Offset: 0x0002A0C6
		public bool Equals(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle == other.m_PublishedFileUpdateHandle;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0002BED6 File Offset: 0x0002A0D6
		public int CompareTo(PublishedFileUpdateHandle_t other)
		{
			return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
		}

		// Token: 0x04000A97 RID: 2711
		public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000A98 RID: 2712
		public ulong m_PublishedFileUpdateHandle;
	}
}
