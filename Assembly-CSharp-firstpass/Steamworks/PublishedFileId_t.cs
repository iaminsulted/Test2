using System;

namespace Steamworks
{
	// Token: 0x020001D7 RID: 471
	[Serializable]
	public struct PublishedFileId_t : IEquatable<PublishedFileId_t>, IComparable<PublishedFileId_t>
	{
		// Token: 0x06000E93 RID: 3731 RVA: 0x0002BDBD File Offset: 0x00029FBD
		public PublishedFileId_t(ulong value)
		{
			this.m_PublishedFileId = value;
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0002BDC6 File Offset: 0x00029FC6
		public override string ToString()
		{
			return this.m_PublishedFileId.ToString();
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0002BDD3 File Offset: 0x00029FD3
		public override bool Equals(object other)
		{
			return other is PublishedFileId_t && this == (PublishedFileId_t)other;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0002BDF0 File Offset: 0x00029FF0
		public override int GetHashCode()
		{
			return this.m_PublishedFileId.GetHashCode();
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0002BDFD File Offset: 0x00029FFD
		public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
		{
			return x.m_PublishedFileId == y.m_PublishedFileId;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0002BE0D File Offset: 0x0002A00D
		public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0002BE19 File Offset: 0x0002A019
		public static explicit operator PublishedFileId_t(ulong value)
		{
			return new PublishedFileId_t(value);
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0002BE21 File Offset: 0x0002A021
		public static explicit operator ulong(PublishedFileId_t that)
		{
			return that.m_PublishedFileId;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0002BE29 File Offset: 0x0002A029
		public bool Equals(PublishedFileId_t other)
		{
			return this.m_PublishedFileId == other.m_PublishedFileId;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0002BE39 File Offset: 0x0002A039
		public int CompareTo(PublishedFileId_t other)
		{
			return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
		}

		// Token: 0x04000A95 RID: 2709
		public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);

		// Token: 0x04000A96 RID: 2710
		public ulong m_PublishedFileId;
	}
}
