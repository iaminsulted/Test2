using System;

namespace Windows.Kinect
{
	// Token: 0x02000059 RID: 89
	public struct CameraSpacePoint
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0001DDFE File Offset: 0x0001BFFE
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x0001DE06 File Offset: 0x0001C006
		public float X { readonly get; set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0001DE0F File Offset: 0x0001C00F
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x0001DE17 File Offset: 0x0001C017
		public float Y { readonly get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001DE20 File Offset: 0x0001C020
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0001DE28 File Offset: 0x0001C028
		public float Z { readonly get; set; }

		// Token: 0x0600048C RID: 1164 RVA: 0x0001DE34 File Offset: 0x0001C034
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001DE6D File Offset: 0x0001C06D
		public override bool Equals(object obj)
		{
			return obj is CameraSpacePoint && this.Equals((CameraSpacePoint)obj);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001DE88 File Offset: 0x0001C088
		public bool Equals(CameraSpacePoint obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y) && this.Z.Equals(obj.Z);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001DEDA File Offset: 0x0001C0DA
		public static bool operator ==(CameraSpacePoint a, CameraSpacePoint b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001DEE4 File Offset: 0x0001C0E4
		public static bool operator !=(CameraSpacePoint a, CameraSpacePoint b)
		{
			return !a.Equals(b);
		}
	}
}
