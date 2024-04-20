using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200002F RID: 47
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Vortex")]
	public class Vortex : ImageEffectBase
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x000163C1 File Offset: 0x000145C1
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x040001E4 RID: 484
		public Vector2 radius = new Vector2(0.4f, 0.4f);

		// Token: 0x040001E5 RID: 485
		public float angle = 50f;

		// Token: 0x040001E6 RID: 486
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
