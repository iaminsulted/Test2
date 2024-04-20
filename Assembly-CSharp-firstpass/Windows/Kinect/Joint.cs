using System;

namespace Windows.Kinect
{
	// Token: 0x02000074 RID: 116
	public struct Joint
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00021830 File Offset: 0x0001FA30
		// (set) Token: 0x060005BD RID: 1469 RVA: 0x00021838 File Offset: 0x0001FA38
		public JointType JointType { readonly get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00021841 File Offset: 0x0001FA41
		// (set) Token: 0x060005BF RID: 1471 RVA: 0x00021849 File Offset: 0x0001FA49
		public CameraSpacePoint Position { readonly get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00021852 File Offset: 0x0001FA52
		// (set) Token: 0x060005C1 RID: 1473 RVA: 0x0002185A File Offset: 0x0001FA5A
		public TrackingState TrackingState { readonly get; set; }

		// Token: 0x060005C2 RID: 1474 RVA: 0x00021864 File Offset: 0x0001FA64
		public override int GetHashCode()
		{
			return this.JointType.GetHashCode() ^ this.Position.GetHashCode() ^ this.TrackingState.GetHashCode();
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000218AF File Offset: 0x0001FAAF
		public override bool Equals(object obj)
		{
			return obj is Joint && this.Equals((Joint)obj);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000218C8 File Offset: 0x0001FAC8
		public bool Equals(Joint obj)
		{
			return this.JointType.Equals(obj.JointType) && this.Position.Equals(obj.Position) && this.TrackingState.Equals(obj.TrackingState);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00021930 File Offset: 0x0001FB30
		public static bool operator ==(Joint a, Joint b)
		{
			return a.Equals(b);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0002193A File Offset: 0x0001FB3A
		public static bool operator !=(Joint a, Joint b)
		{
			return !a.Equals(b);
		}
	}
}
