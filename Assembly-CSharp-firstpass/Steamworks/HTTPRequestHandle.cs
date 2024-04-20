using System;

namespace Steamworks
{
	// Token: 0x020001CF RID: 463
	[Serializable]
	public struct HTTPRequestHandle : IEquatable<HTTPRequestHandle>, IComparable<HTTPRequestHandle>
	{
		// Token: 0x06000E3F RID: 3647 RVA: 0x0002B90C File Offset: 0x00029B0C
		public HTTPRequestHandle(uint value)
		{
			this.m_HTTPRequestHandle = value;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0002B915 File Offset: 0x00029B15
		public override string ToString()
		{
			return this.m_HTTPRequestHandle.ToString();
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x0002B922 File Offset: 0x00029B22
		public override bool Equals(object other)
		{
			return other is HTTPRequestHandle && this == (HTTPRequestHandle)other;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0002B93F File Offset: 0x00029B3F
		public override int GetHashCode()
		{
			return this.m_HTTPRequestHandle.GetHashCode();
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0002B94C File Offset: 0x00029B4C
		public static bool operator ==(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return x.m_HTTPRequestHandle == y.m_HTTPRequestHandle;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0002B95C File Offset: 0x00029B5C
		public static bool operator !=(HTTPRequestHandle x, HTTPRequestHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0002B968 File Offset: 0x00029B68
		public static explicit operator HTTPRequestHandle(uint value)
		{
			return new HTTPRequestHandle(value);
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0002B970 File Offset: 0x00029B70
		public static explicit operator uint(HTTPRequestHandle that)
		{
			return that.m_HTTPRequestHandle;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0002B978 File Offset: 0x00029B78
		public bool Equals(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle == other.m_HTTPRequestHandle;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0002B988 File Offset: 0x00029B88
		public int CompareTo(HTTPRequestHandle other)
		{
			return this.m_HTTPRequestHandle.CompareTo(other.m_HTTPRequestHandle);
		}

		// Token: 0x04000A88 RID: 2696
		public static readonly HTTPRequestHandle Invalid = new HTTPRequestHandle(0U);

		// Token: 0x04000A89 RID: 2697
		public uint m_HTTPRequestHandle;
	}
}
