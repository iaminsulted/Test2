using System;

namespace Steamworks
{
	// Token: 0x020001DE RID: 478
	[Serializable]
	public struct DepotId_t : IEquatable<DepotId_t>, IComparable<DepotId_t>
	{
		// Token: 0x06000EDF RID: 3807 RVA: 0x0002C1F8 File Offset: 0x0002A3F8
		public DepotId_t(uint value)
		{
			this.m_DepotId = value;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0002C201 File Offset: 0x0002A401
		public override string ToString()
		{
			return this.m_DepotId.ToString();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0002C20E File Offset: 0x0002A40E
		public override bool Equals(object other)
		{
			return other is DepotId_t && this == (DepotId_t)other;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0002C22B File Offset: 0x0002A42B
		public override int GetHashCode()
		{
			return this.m_DepotId.GetHashCode();
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0002C238 File Offset: 0x0002A438
		public static bool operator ==(DepotId_t x, DepotId_t y)
		{
			return x.m_DepotId == y.m_DepotId;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0002C248 File Offset: 0x0002A448
		public static bool operator !=(DepotId_t x, DepotId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0002C254 File Offset: 0x0002A454
		public static explicit operator DepotId_t(uint value)
		{
			return new DepotId_t(value);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0002C25C File Offset: 0x0002A45C
		public static explicit operator uint(DepotId_t that)
		{
			return that.m_DepotId;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0002C264 File Offset: 0x0002A464
		public bool Equals(DepotId_t other)
		{
			return this.m_DepotId == other.m_DepotId;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0002C274 File Offset: 0x0002A474
		public int CompareTo(DepotId_t other)
		{
			return this.m_DepotId.CompareTo(other.m_DepotId);
		}

		// Token: 0x04000AA2 RID: 2722
		public static readonly DepotId_t Invalid = new DepotId_t(0U);

		// Token: 0x04000AA3 RID: 2723
		public uint m_DepotId;
	}
}
