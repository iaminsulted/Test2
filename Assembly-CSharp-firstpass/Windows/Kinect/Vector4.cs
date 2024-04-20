using System;

namespace Windows.Kinect
{
	// Token: 0x02000084 RID: 132
	public struct Vector4
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0002373D File Offset: 0x0002193D
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x00023745 File Offset: 0x00021945
		public float X { readonly get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0002374E File Offset: 0x0002194E
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00023756 File Offset: 0x00021956
		public float Y { readonly get; set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0002375F File Offset: 0x0002195F
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x00023767 File Offset: 0x00021967
		public float Z { readonly get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00023770 File Offset: 0x00021970
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x00023778 File Offset: 0x00021978
		public float W { readonly get; set; }

		// Token: 0x06000662 RID: 1634 RVA: 0x00023784 File Offset: 0x00021984
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.W.GetHashCode();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000237CC File Offset: 0x000219CC
		public override bool Equals(object obj)
		{
			return obj is Vector4 && this.Equals((Vector4)obj);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000237E4 File Offset: 0x000219E4
		public bool Equals(Vector4 obj)
		{
			return this.X.Equals(obj.X) && this.Y.Equals(obj.Y) && this.Z.Equals(obj.Z) && this.W.Equals(obj.W);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002384D File Offset: 0x00021A4D
		public static bool operator ==(Vector4 a, Vector4 b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00023857 File Offset: 0x00021A57
		public static bool operator !=(Vector4 a, Vector4 b)
		{
			return !a.Equals(b);
		}
	}
}
