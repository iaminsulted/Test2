using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200000F RID: 15
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class BlurOptimized : PostEffectsBase
	{
		// Token: 0x06000120 RID: 288 RVA: 0x0000E5C9 File Offset: 0x0000C7C9
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000E5FF File Offset: 0x0000C7FF
		public void OnDisable()
		{
			if (this.blurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.blurMaterial);
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000E61C File Offset: 0x0000C81C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 1f / (1f * (float)(1 << this.downsample));
			this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
			source.filterMode = FilterMode.Bilinear;
			int width = source.width >> this.downsample;
			int height = source.height >> this.downsample;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
			int num2 = (this.blurType == BlurOptimized.BlurType.StandardGauss) ? 0 : 2;
			for (int i = 0; i < this.blurIterations; i++)
			{
				float num3 = (float)i * 1f;
				this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num + num3, -this.blurSize * num - num3, 0f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num2);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num2);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040000AB RID: 171
		[Range(0f, 2f)]
		public int downsample = 1;

		// Token: 0x040000AC RID: 172
		[Range(0f, 10f)]
		public float blurSize = 3f;

		// Token: 0x040000AD RID: 173
		[Range(1f, 4f)]
		public int blurIterations = 2;

		// Token: 0x040000AE RID: 174
		public BlurOptimized.BlurType blurType;

		// Token: 0x040000AF RID: 175
		public Shader blurShader;

		// Token: 0x040000B0 RID: 176
		private Material blurMaterial;

		// Token: 0x02000215 RID: 533
		public enum BlurType
		{
			// Token: 0x04000C61 RID: 3169
			StandardGauss,
			// Token: 0x04000C62 RID: 3170
			SgxGauss
		}
	}
}
