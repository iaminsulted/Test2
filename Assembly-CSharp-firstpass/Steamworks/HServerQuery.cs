using System;

namespace Steamworks
{
	// Token: 0x020001D4 RID: 468
	[Serializable]
	public struct HServerQuery : IEquatable<HServerQuery>, IComparable<HServerQuery>
	{
		// Token: 0x06000E74 RID: 3700 RVA: 0x0002BC03 File Offset: 0x00029E03
		public HServerQuery(int value)
		{
			this.m_HServerQuery = value;
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0002BC0C File Offset: 0x00029E0C
		public override string ToString()
		{
			return this.m_HServerQuery.ToString();
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0002BC19 File Offset: 0x00029E19
		public override bool Equals(object other)
		{
			return other is HServerQuery && this == (HServerQuery)other;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0002BC36 File Offset: 0x00029E36
		public override int GetHashCode()
		{
			return this.m_HServerQuery.GetHashCode();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0002BC43 File Offset: 0x00029E43
		public static bool operator ==(HServerQuery x, HServerQuery y)
		{
			return x.m_HServerQuery == y.m_HServerQuery;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0002BC53 File Offset: 0x00029E53
		public static bool operator !=(HServerQuery x, HServerQuery y)
		{
			return !(x == y);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0002BC5F File Offset: 0x00029E5F
		public static explicit operator HServerQuery(int value)
		{
			return new HServerQuery(value);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0002BC67 File Offset: 0x00029E67
		public static explicit operator int(HServerQuery that)
		{
			return that.m_HServerQuery;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0002BC6F File Offset: 0x00029E6F
		public bool Equals(HServerQuery other)
		{
			return this.m_HServerQuery == other.m_HServerQuery;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0002BC7F File Offset: 0x00029E7F
		public int CompareTo(HServerQuery other)
		{
			return this.m_HServerQuery.CompareTo(other.m_HServerQuery);
		}

		// Token: 0x04000A91 RID: 2705
		public static readonly HServerQuery Invalid = new HServerQuery(-1);

		// Token: 0x04000A92 RID: 2706
		public int m_HServerQuery;
	}
}
