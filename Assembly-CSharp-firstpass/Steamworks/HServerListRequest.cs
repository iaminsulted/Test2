using System;

namespace Steamworks
{
	// Token: 0x020001D3 RID: 467
	[Serializable]
	public struct HServerListRequest : IEquatable<HServerListRequest>
	{
		// Token: 0x06000E6A RID: 3690 RVA: 0x0002BB70 File Offset: 0x00029D70
		public HServerListRequest(IntPtr value)
		{
			this.m_HServerListRequest = value;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0002BB79 File Offset: 0x00029D79
		public override string ToString()
		{
			return this.m_HServerListRequest.ToString();
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0002BB86 File Offset: 0x00029D86
		public override bool Equals(object other)
		{
			return other is HServerListRequest && this == (HServerListRequest)other;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0002BBA3 File Offset: 0x00029DA3
		public override int GetHashCode()
		{
			return this.m_HServerListRequest.GetHashCode();
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0002BBB0 File Offset: 0x00029DB0
		public static bool operator ==(HServerListRequest x, HServerListRequest y)
		{
			return x.m_HServerListRequest == y.m_HServerListRequest;
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0002BBC3 File Offset: 0x00029DC3
		public static bool operator !=(HServerListRequest x, HServerListRequest y)
		{
			return !(x == y);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0002BBCF File Offset: 0x00029DCF
		public static explicit operator HServerListRequest(IntPtr value)
		{
			return new HServerListRequest(value);
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0002BBD7 File Offset: 0x00029DD7
		public static explicit operator IntPtr(HServerListRequest that)
		{
			return that.m_HServerListRequest;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0002BBDF File Offset: 0x00029DDF
		public bool Equals(HServerListRequest other)
		{
			return this.m_HServerListRequest == other.m_HServerListRequest;
		}

		// Token: 0x04000A8F RID: 2703
		public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);

		// Token: 0x04000A90 RID: 2704
		public IntPtr m_HServerListRequest;
	}
}
