using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000028 RID: 40
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
	public class SepiaTone : ImageEffectBase
	{
		// Token: 0x060001BD RID: 445 RVA: 0x00015110 File Offset: 0x00013310
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, base.material);
		}
	}
}
