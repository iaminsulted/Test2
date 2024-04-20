using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200002D RID: 45
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Twirl")]
	public class Twirl : ImageEffectBase
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x00015FE4 File Offset: 0x000141E4
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x040001D3 RID: 467
		public Vector2 radius = new Vector2(0.3f, 0.3f);

		// Token: 0x040001D4 RID: 468
		[Range(0f, 360f)]
		public float angle = 50f;

		// Token: 0x040001D5 RID: 469
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
