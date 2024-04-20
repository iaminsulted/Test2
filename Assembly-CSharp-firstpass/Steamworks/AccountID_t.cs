using System;

namespace Steamworks
{
	// Token: 0x020001DC RID: 476
	[Serializable]
	public struct AccountID_t : IEquatable<AccountID_t>, IComparable<AccountID_t>
	{
		// Token: 0x06000ECA RID: 3786 RVA: 0x0002C0CD File Offset: 0x0002A2CD
		public AccountID_t(uint value)
		{
			this.m_AccountID = value;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0002C0D6 File Offset: 0x0002A2D6
		public override string ToString()
		{
			return this.m_AccountID.ToString();
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0002C0E3 File Offset: 0x0002A2E3
		public override bool Equals(object other)
		{
			return other is AccountID_t && this == (AccountID_t)other;
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0002C100 File Offset: 0x0002A300
		public override int GetHashCode()
		{
			return this.m_AccountID.GetHashCode();
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0002C10D File Offset: 0x0002A30D
		public static bool operator ==(AccountID_t x, AccountID_t y)
		{
			return x.m_AccountID == y.m_AccountID;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0002C11D File Offset: 0x0002A31D
		public static bool operator !=(AccountID_t x, AccountID_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0002C129 File Offset: 0x0002A329
		public static explicit operator AccountID_t(uint value)
		{
			return new AccountID_t(value);
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0002C131 File Offset: 0x0002A331
		public static explicit operator uint(AccountID_t that)
		{
			return that.m_AccountID;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0002C139 File Offset: 0x0002A339
		public bool Equals(AccountID_t other)
		{
			return this.m_AccountID == other.m_AccountID;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0002C149 File Offset: 0x0002A349
		public int CompareTo(AccountID_t other)
		{
			return this.m_AccountID.CompareTo(other.m_AccountID);
		}

		// Token: 0x04000A9F RID: 2719
		public uint m_AccountID;
	}
}
