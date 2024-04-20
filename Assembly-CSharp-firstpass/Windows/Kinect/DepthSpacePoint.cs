using System;

namespace Windows.Kinect
{
	// Token: 0x02000066 RID: 102
	public struct DepthSpacePoint
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00020362 File Offset: 0x0001E562
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0002036A File Offset: 0x0001E56A
		public float X { readonly get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00020373 File Offset: 0x0001E573
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x0002037B File Offset: 0x0001E57B
		public float Y { readonly get; set; }

		// Token: 0x06000546 RID: 1350 RVA: 0x00020384 File Offset: 0x0001E584
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000203AE File Offset: 0x0001E5AE
		public override bool Equals(object obj)
		{
			return obj is DepthSpacePoint && this.Equals((DepthSpacePoint)obj);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000203C8 File Offset: 0x0001E5C8
		public bool Equals(DepthSpacePoint obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00020403 File Offset: 0x0001E603
		public static bool operator ==(DepthSpacePoint a, DepthSpacePoint b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0002040D File Offset: 0x0001E60D
		public static bool operator !=(DepthSpacePoint a, DepthSpacePoint b)
		{
			return !a.Equals(b);
		}
	}
}
