using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200001C RID: 28
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
	public class Grayscale : ImageEffectBase
	{
		// Token: 0x06000178 RID: 376 RVA: 0x00012CA0 File Offset: 0x00010EA0
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			base.material.SetFloat("_RampOffset", this.rampOffset);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x04000165 RID: 357
		public Texture textureRamp;

		// Token: 0x04000166 RID: 358
		[Range(-1f, 1f)]
		public float rampOffset;
	}
}
