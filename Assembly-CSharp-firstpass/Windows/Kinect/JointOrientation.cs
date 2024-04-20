using System;

namespace Windows.Kinect
{
	// Token: 0x02000075 RID: 117
	public struct JointOrientation
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x00021947 File Offset: 0x0001FB47
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0002194F File Offset: 0x0001FB4F
		public JointType JointType { readonly get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00021958 File Offset: 0x0001FB58
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x00021960 File Offset: 0x0001FB60
		public Vector4 Orientation { readonly get; set; }

		// Token: 0x060005CB RID: 1483 RVA: 0x0002196C File Offset: 0x0001FB6C
		public override int GetHashCode()
		{
			return this.JointType.GetHashCode() ^ this.Orientation.GetHashCode();
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000219A2 File Offset: 0x0001FBA2
		public override bool Equals(object obj)
		{
			return obj is JointOrientation && this.Equals((JointOrientation)obj);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000219BC File Offset: 0x0001FBBC
		public bool Equals(JointOrientation obj)
		{
			return this.JointType.Equals(obj.JointType) && this.Orientation.Equals(obj.Orientation);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00021A02 File Offset: 0x0001FC02
		public static bool operator ==(JointOrientation a, JointOrientation b)
		{
			return a.Equals(b);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00021A0C File Offset: 0x0001FC0C
		public static bool operator !=(JointOrientation a, JointOrientation b)
		{
			return !a.Equals(b);
		}
	}
}
