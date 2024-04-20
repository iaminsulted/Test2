using System;

namespace Steamworks
{
	// Token: 0x020001C7 RID: 455
	[Serializable]
	public struct HAuthTicket : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
	{
		// Token: 0x06000DEB RID: 3563 RVA: 0x0002B460 File Offset: 0x00029660
		public HAuthTicket(uint value)
		{
			this.m_HAuthTicket = value;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0002B469 File Offset: 0x00029669
		public override string ToString()
		{
			return this.m_HAuthTicket.ToString();
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0002B476 File Offset: 0x00029676
		public override bool Equals(object other)
		{
			return other is HAuthTicket && this == (HAuthTicket)other;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0002B493 File Offset: 0x00029693
		public override int GetHashCode()
		{
			return this.m_HAuthTicket.GetHashCode();
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0002B4A0 File Offset: 0x000296A0
		public static bool operator ==(HAuthTicket x, HAuthTicket y)
		{
			return x.m_HAuthTicket == y.m_HAuthTicket;
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0002B4B0 File Offset: 0x000296B0
		public static bool operator !=(HAuthTicket x, HAuthTicket y)
		{
			return !(x == y);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0002B4BC File Offset: 0x000296BC
		public static explicit operator HAuthTicket(uint value)
		{
			return new HAuthTicket(value);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0002B4C4 File Offset: 0x000296C4
		public static explicit operator uint(HAuthTicket that)
		{
			return that.m_HAuthTicket;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0002B4CC File Offset: 0x000296CC
		public bool Equals(HAuthTicket other)
		{
			return this.m_HAuthTicket == other.m_HAuthTicket;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0002B4DC File Offset: 0x000296DC
		public int CompareTo(HAuthTicket other)
		{
			return this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);
		}

		// Token: 0x04000A7C RID: 2684
		public static readonly HAuthTicket Invalid = new HAuthTicket(0U);

		// Token: 0x04000A7D RID: 2685
		public uint m_HAuthTicket;
	}
}
