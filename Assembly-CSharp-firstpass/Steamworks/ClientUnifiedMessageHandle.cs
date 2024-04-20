using System;

namespace Steamworks
{
	// Token: 0x020001E3 RID: 483
	[Serializable]
	public struct ClientUnifiedMessageHandle : IEquatable<ClientUnifiedMessageHandle>, IComparable<ClientUnifiedMessageHandle>
	{
		// Token: 0x06000F16 RID: 3862 RVA: 0x0002C508 File Offset: 0x0002A708
		public ClientUnifiedMessageHandle(ulong value)
		{
			this.m_ClientUnifiedMessageHandle = value;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0002C511 File Offset: 0x0002A711
		public override string ToString()
		{
			return this.m_ClientUnifiedMessageHandle.ToString();
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0002C51E File Offset: 0x0002A71E
		public override bool Equals(object other)
		{
			return other is ClientUnifiedMessageHandle && this == (ClientUnifiedMessageHandle)other;
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0002C53B File Offset: 0x0002A73B
		public override int GetHashCode()
		{
			return this.m_ClientUnifiedMessageHandle.GetHashCode();
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0002C548 File Offset: 0x0002A748
		public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
		{
			return x.m_ClientUnifiedMessageHandle == y.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0002C558 File Offset: 0x0002A758
		public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0002C564 File Offset: 0x0002A764
		public static explicit operator ClientUnifiedMessageHandle(ulong value)
		{
			return new ClientUnifiedMessageHandle(value);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0002C56C File Offset: 0x0002A76C
		public static explicit operator ulong(ClientUnifiedMessageHandle that)
		{
			return that.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0002C574 File Offset: 0x0002A774
		public bool Equals(ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle == other.m_ClientUnifiedMessageHandle;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0002C584 File Offset: 0x0002A784
		public int CompareTo(ClientUnifiedMessageHandle other)
		{
			return this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);
		}

		// Token: 0x04000AAC RID: 2732
		public static readonly ClientUnifiedMessageHandle Invalid = new ClientUnifiedMessageHandle(0UL);

		// Token: 0x04000AAD RID: 2733
		public ulong m_ClientUnifiedMessageHandle;
	}
}
