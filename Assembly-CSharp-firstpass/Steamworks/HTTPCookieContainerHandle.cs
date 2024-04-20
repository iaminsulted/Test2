using System;

namespace Steamworks
{
	// Token: 0x020001CE RID: 462
	[Serializable]
	public struct HTTPCookieContainerHandle : IEquatable<HTTPCookieContainerHandle>, IComparable<HTTPCookieContainerHandle>
	{
		// Token: 0x06000E34 RID: 3636 RVA: 0x0002B870 File Offset: 0x00029A70
		public HTTPCookieContainerHandle(uint value)
		{
			this.m_HTTPCookieContainerHandle = value;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0002B879 File Offset: 0x00029A79
		public override string ToString()
		{
			return this.m_HTTPCookieContainerHandle.ToString();
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0002B886 File Offset: 0x00029A86
		public override bool Equals(object other)
		{
			return other is HTTPCookieContainerHandle && this == (HTTPCookieContainerHandle)other;
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0002B8A3 File Offset: 0x00029AA3
		public override int GetHashCode()
		{
			return this.m_HTTPCookieContainerHandle.GetHashCode();
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0002B8B0 File Offset: 0x00029AB0
		public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return x.m_HTTPCookieContainerHandle == y.m_HTTPCookieContainerHandle;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0002B8C0 File Offset: 0x00029AC0
		public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0002B8CC File Offset: 0x00029ACC
		public static explicit operator HTTPCookieContainerHandle(uint value)
		{
			return new HTTPCookieContainerHandle(value);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0002B8D4 File Offset: 0x00029AD4
		public static explicit operator uint(HTTPCookieContainerHandle that)
		{
			return that.m_HTTPCookieContainerHandle;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0002B8DC File Offset: 0x00029ADC
		public bool Equals(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle == other.m_HTTPCookieContainerHandle;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0002B8EC File Offset: 0x00029AEC
		public int CompareTo(HTTPCookieContainerHandle other)
		{
			return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
		}

		// Token: 0x04000A86 RID: 2694
		public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);

		// Token: 0x04000A87 RID: 2695
		public uint m_HTTPCookieContainerHandle;
	}
}
