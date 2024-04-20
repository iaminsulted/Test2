using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000013 RID: 19
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
	public class ColorCorrectionRamp : ImageEffectBase
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000FE4D File Offset: 0x0000E04D
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x040000EB RID: 235
		public Texture textureRamp;
	}
}
