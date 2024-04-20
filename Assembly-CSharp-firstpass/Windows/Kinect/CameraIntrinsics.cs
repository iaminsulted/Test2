using System;

namespace Windows.Kinect
{
	// Token: 0x02000039 RID: 57
	public struct CameraIntrinsics
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00016D65 File Offset: 0x00014F65
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00016D6D File Offset: 0x00014F6D
		public float FocalLengthX { readonly get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00016D76 File Offset: 0x00014F76
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00016D7E File Offset: 0x00014F7E
		public float FocalLengthY { readonly get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00016D87 File Offset: 0x00014F87
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00016D8F File Offset: 0x00014F8F
		public float PrincipalPointX { readonly get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00016D98 File Offset: 0x00014F98
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00016DA0 File Offset: 0x00014FA0
		public float PrincipalPointY { readonly get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00016DA9 File Offset: 0x00014FA9
		// (set) Token: 0x06000209 RID: 521 RVA: 0x00016DB1 File Offset: 0x00014FB1
		public float RadialDistortionSecondOrder { readonly get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00016DBA File Offset: 0x00014FBA
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00016DC2 File Offset: 0x00014FC2
		public float RadialDistortionFourthOrder { readonly get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00016DCB File Offset: 0x00014FCB
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00016DD3 File Offset: 0x00014FD3
		public float RadialDistortionSixthOrder { readonly get; set; }

		// Token: 0x0600020E RID: 526 RVA: 0x00016DDC File Offset: 0x00014FDC
		public override int GetHashCode()
		{
			return this.FocalLengthX.GetHashCode() ^ this.FocalLengthY.GetHashCode() ^ this.PrincipalPointX.GetHashCode() ^ this.PrincipalPointY.GetHashCode() ^ this.RadialDistortionSecondOrder.GetHashCode() ^ this.RadialDistortionFourthOrder.GetHashCode() ^ this.RadialDistortionSixthOrder.GetHashCode();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00016E51 File Offset: 0x00015051
		public override bool Equals(object obj)
		{
			return obj is CameraIntrinsics && this.Equals((CameraIntrinsics)obj);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00016E6C File Offset: 0x0001506C
		public bool Equals(CameraIntrinsics obj)
		{
			return this.FocalLengthX.Equals(obj.FocalLengthX) && this.FocalLengthY.Equals(obj.FocalLengthY) && this.PrincipalPointX.Equals(obj.PrincipalPointX) && this.PrincipalPointY.Equals(obj.PrincipalPointY) && this.RadialDistortionSecondOrder.Equals(obj.RadialDistortionSecondOrder) && this.RadialDistortionFourthOrder.Equals(obj.RadialDistortionFourthOrder) && this.RadialDistortionSixthOrder.Equals(obj.RadialDistortionSixthOrder);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00016F1D File Offset: 0x0001511D
		public static bool operator ==(CameraIntrinsics a, CameraIntrinsics b)
		{
			return a.Equals(b);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00016F27 File Offset: 0x00015127
		public static bool operator !=(CameraIntrinsics a, CameraIntrinsics b)
		{
			return !a.Equals(b);
		}
	}
}
