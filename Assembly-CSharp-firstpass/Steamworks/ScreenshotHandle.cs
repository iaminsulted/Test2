using System;

namespace Steamworks
{
	// Token: 0x020001DB RID: 475
	[Serializable]
	public struct ScreenshotHandle : IEquatable<ScreenshotHandle>, IComparable<ScreenshotHandle>
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x0002C031 File Offset: 0x0002A231
		public ScreenshotHandle(uint value)
		{
			this.m_ScreenshotHandle = value;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0002C03A File Offset: 0x0002A23A
		public override string ToString()
		{
			return this.m_ScreenshotHandle.ToString();
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0002C047 File Offset: 0x0002A247
		public override bool Equals(object other)
		{
			return other is ScreenshotHandle && this == (ScreenshotHandle)other;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0002C064 File Offset: 0x0002A264
		public override int GetHashCode()
		{
			return this.m_ScreenshotHandle.GetHashCode();
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0002C071 File Offset: 0x0002A271
		public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
		{
			return x.m_ScreenshotHandle == y.m_ScreenshotHandle;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0002C081 File Offset: 0x0002A281
		public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y)
		{
			return !(x == y);
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0002C08D File Offset: 0x0002A28D
		public static explicit operator ScreenshotHandle(uint value)
		{
			return new ScreenshotHandle(value);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0002C095 File Offset: 0x0002A295
		public static explicit operator uint(ScreenshotHandle that)
		{
			return that.m_ScreenshotHandle;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0002C09D File Offset: 0x0002A29D
		public bool Equals(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle == other.m_ScreenshotHandle;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0002C0AD File Offset: 0x0002A2AD
		public int CompareTo(ScreenshotHandle other)
		{
			return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
		}

		// Token: 0x04000A9D RID: 2717
		public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0U);

		// Token: 0x04000A9E RID: 2718
		public uint m_ScreenshotHandle;
	}
}
