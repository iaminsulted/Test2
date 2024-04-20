using System;

namespace Steamworks
{
	// Token: 0x020001CD RID: 461
	[Serializable]
	public struct HHTMLBrowser : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
	{
		// Token: 0x06000E29 RID: 3625 RVA: 0x0002B7D4 File Offset: 0x000299D4
		public HHTMLBrowser(uint value)
		{
			this.m_HHTMLBrowser = value;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0002B7DD File Offset: 0x000299DD
		public override string ToString()
		{
			return this.m_HHTMLBrowser.ToString();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0002B7EA File Offset: 0x000299EA
		public override bool Equals(object other)
		{
			return other is HHTMLBrowser && this == (HHTMLBrowser)other;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0002B807 File Offset: 0x00029A07
		public override int GetHashCode()
		{
			return this.m_HHTMLBrowser.GetHashCode();
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0002B814 File Offset: 0x00029A14
		public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
		{
			return x.m_HHTMLBrowser == y.m_HHTMLBrowser;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0002B824 File Offset: 0x00029A24
		public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y)
		{
			return !(x == y);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0002B830 File Offset: 0x00029A30
		public static explicit operator HHTMLBrowser(uint value)
		{
			return new HHTMLBrowser(value);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0002B838 File Offset: 0x00029A38
		public static explicit operator uint(HHTMLBrowser that)
		{
			return that.m_HHTMLBrowser;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0002B840 File Offset: 0x00029A40
		public bool Equals(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser == other.m_HHTMLBrowser;
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0002B850 File Offset: 0x00029A50
		public int CompareTo(HHTMLBrowser other)
		{
			return this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);
		}

		// Token: 0x04000A84 RID: 2692
		public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);

		// Token: 0x04000A85 RID: 2693
		public uint m_HHTMLBrowser;
	}
}
