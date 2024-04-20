using System;

namespace Windows.Kinect
{
	// Token: 0x0200003B RID: 59
	public struct PointF
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000223 RID: 547 RVA: 0x000170C0 File Offset: 0x000152C0
		// (set) Token: 0x06000224 RID: 548 RVA: 0x000170C8 File Offset: 0x000152C8
		public float X { readonly get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000170D1 File Offset: 0x000152D1
		// (set) Token: 0x06000226 RID: 550 RVA: 0x000170D9 File Offset: 0x000152D9
		public float Y { readonly get; set; }

		// Token: 0x06000227 RID: 551 RVA: 0x000170E4 File Offset: 0x000152E4
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0001710E File Offset: 0x0001530E
		public override bool Equals(object obj)
		{
			return obj is PointF && this.Equals((ColorSpacePoint)obj);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00017126 File Offset: 0x00015326
		public bool Equals(ColorSpacePoint obj)
		{
			return this.X == obj.X && this.Y == obj.Y;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00017148 File Offset: 0x00015348
		public static bool operator ==(PointF a, PointF b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0001715D File Offset: 0x0001535D
		public static bool operator !=(PointF a, PointF b)
		{
			return !a.Equals(b);
		}
	}
}
