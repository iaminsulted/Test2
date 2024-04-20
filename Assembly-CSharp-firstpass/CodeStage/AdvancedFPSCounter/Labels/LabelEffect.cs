using System;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.Labels
{
	// Token: 0x020001FB RID: 507
	internal class LabelEffect
	{
		// Token: 0x06001021 RID: 4129 RVA: 0x000347B6 File Offset: 0x000329B6
		internal LabelEffect(bool enabled, Color color, int padding) : this(enabled, color, Vector2.zero)
		{
			this.padding = padding;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x000347CC File Offset: 0x000329CC
		internal LabelEffect(bool enabled, Color color, Vector2 distance)
		{
			this.enabled = enabled;
			this.color = color;
			this.distance = distance;
		}

		// Token: 0x04000B8B RID: 2955
		public bool enabled;

		// Token: 0x04000B8C RID: 2956
		public Color color;

		// Token: 0x04000B8D RID: 2957
		public Vector2 distance;

		// Token: 0x04000B8E RID: 2958
		public int padding;
	}
}
