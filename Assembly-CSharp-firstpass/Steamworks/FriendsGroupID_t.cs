using System;

namespace Steamworks
{
	// Token: 0x020001CC RID: 460
	[Serializable]
	public struct FriendsGroupID_t : IEquatable<FriendsGroupID_t>, IComparable<FriendsGroupID_t>
	{
		// Token: 0x06000E1E RID: 3614 RVA: 0x0002B738 File Offset: 0x00029938
		public FriendsGroupID_t(short value)
		{
			this.m_FriendsGroupID = value;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0002B741 File Offset: 0x00029941
		public override string ToString()
		{
			return this.m_FriendsGroupID.ToString();
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0002B74E File Offset: 0x0002994E
		public override bool Equals(object other)
		{
			return other is FriendsGroupID_t && this == (FriendsGroupID_t)other;
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0002B76B File Offset: 0x0002996B
		public override int GetHashCode()
		{
			return this.m_FriendsGroupID.GetHashCode();
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0002B778 File Offset: 0x00029978
		public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return x.m_FriendsGroupID == y.m_FriendsGroupID;
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0002B788 File Offset: 0x00029988
		public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0002B794 File Offset: 0x00029994
		public static explicit operator FriendsGroupID_t(short value)
		{
			return new FriendsGroupID_t(value);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0002B79C File Offset: 0x0002999C
		public static explicit operator short(FriendsGroupID_t that)
		{
			return that.m_FriendsGroupID;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0002B7A4 File Offset: 0x000299A4
		public bool Equals(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID == other.m_FriendsGroupID;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0002B7B4 File Offset: 0x000299B4
		public int CompareTo(FriendsGroupID_t other)
		{
			return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
		}

		// Token: 0x04000A82 RID: 2690
		public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t(-1);

		// Token: 0x04000A83 RID: 2691
		public short m_FriendsGroupID;
	}
}
