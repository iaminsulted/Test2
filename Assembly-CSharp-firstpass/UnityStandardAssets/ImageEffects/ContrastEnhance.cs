using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000014 RID: 20
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Enhance (Unsharp Mask)")]
	public class ContrastEnhance : PostEffectsBase
	{
		// Token: 0x06000141 RID: 321 RVA: 0x0000FE7C File Offset: 0x0000E07C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.contrastCompositeMaterial = base.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
			this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000FED8 File Offset: 0x0000E0D8
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0);
			Graphics.Blit(source, temporary);
			RenderTexture temporary2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
			Graphics.Blit(temporary, temporary2);
			RenderTexture.ReleaseTemporary(temporary);
			this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 1f / (float)temporary2.height, 0f, 0f));
			RenderTexture temporary3 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
			Graphics.Blit(temporary2, temporary3, this.separableBlurMaterial);
			RenderTexture.ReleaseTemporary(temporary2);
			this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 1f / (float)temporary2.width, 0f, 0f, 0f));
			temporary2 = RenderTexture.GetTemporary(width / 4, height / 4, 0);
			Graphics.Blit(temporary3, temporary2, this.separableBlurMaterial);
			RenderTexture.ReleaseTemporary(temporary3);
			this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", temporary2);
			this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
			this.contrastCompositeMaterial.SetFloat("threshold", this.threshold);
			Graphics.Blit(source, destination, this.contrastCompositeMaterial);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		// Token: 0x040000EC RID: 236
		[Range(0f, 1f)]
		public float intensity = 0.5f;

		// Token: 0x040000ED RID: 237
		[Range(0f, 0.999f)]
		public float threshold;

		// Token: 0x040000EE RID: 238
		private Material separableBlurMaterial;

		// Token: 0x040000EF RID: 239
		private Material contrastCompositeMaterial;

		// Token: 0x040000F0 RID: 240
		[Range(0f, 1f)]
		public float blurSpread = 1f;

		// Token: 0x040000F1 RID: 241
		public Shader separableBlurShader;

		// Token: 0x040000F2 RID: 242
		public Shader contrastCompositeShader;
	}
}
