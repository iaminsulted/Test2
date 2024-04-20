using System;

namespace Windows.Kinect
{
	// Token: 0x02000060 RID: 96
	public struct ColorSpacePoint
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x0001F136 File Offset: 0x0001D336
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x0001F13E File Offset: 0x0001D33E
		public float X { readonly get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x0001F147 File Offset: 0x0001D347
		// (set) Token: 0x060004EB RID: 1259 RVA: 0x0001F14F File Offset: 0x0001D34F
		public float Y { readonly get; set; }

		// Token: 0x060004EC RID: 1260 RVA: 0x0001F158 File Offset: 0x0001D358
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001F182 File Offset: 0x0001D382
		public override bool Equals(object obj)
		{
			return obj is ColorSpacePoint && this.Equals((ColorSpacePoint)obj);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001F19C File Offset: 0x0001D39C
		public bool Equals(ColorSpacePoint obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001F1D7 File Offset: 0x0001D3D7
		public static bool operator ==(ColorSpacePoint a, ColorSpacePoint b)
		{
			return a.Equals(b);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001F1E1 File Offset: 0x0001D3E1
		public static bool operator !=(ColorSpacePoint a, ColorSpacePoint b)
		{
			return !a.Equals(b);
		}
	}
}
