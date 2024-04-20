using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200000D RID: 13
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
	public class BloomOptimized : PostEffectsBase
	{
		// Token: 0x06000115 RID: 277 RVA: 0x0000E1E0 File Offset: 0x0000C3E0
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fastBloomMaterial = base.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000E216 File Offset: 0x0000C416
		private void OnDisable()
		{
			if (this.fastBloomMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.fastBloomMaterial);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000E230 File Offset: 0x0000C430
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = (this.resolution == BloomOptimized.Resolution.Low) ? 4 : 2;
			float num2 = (this.resolution == BloomOptimized.Resolution.Low) ? 0.5f : 1f;
			this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
			source.filterMode = FilterMode.Bilinear;
			int width = source.width / num;
			int height = source.height / num;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.fastBloomMaterial, 1);
			int num3 = (this.blurType == BloomOptimized.BlurType.Standard) ? 0 : 2;
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2 + (float)i * 1f, 0f, this.threshold, this.intensity));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 2 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 3 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.fastBloomMaterial.SetTexture("_Bloom", renderTexture);
			Graphics.Blit(source, destination, this.fastBloomMaterial, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0400009F RID: 159
		[Range(0f, 1.5f)]
		public float threshold = 0.25f;

		// Token: 0x040000A0 RID: 160
		[Range(0f, 2.5f)]
		public float intensity = 0.75f;

		// Token: 0x040000A1 RID: 161
		[Range(0.25f, 5.5f)]
		public float blurSize = 1f;

		// Token: 0x040000A2 RID: 162
		private BloomOptimized.Resolution resolution;

		// Token: 0x040000A3 RID: 163
		[Range(1f, 4f)]
		public int blurIterations = 1;

		// Token: 0x040000A4 RID: 164
		public BloomOptimized.BlurType blurType;

		// Token: 0x040000A5 RID: 165
		public Shader fastBloomShader;

		// Token: 0x040000A6 RID: 166
		private Material fastBloomMaterial;

		// Token: 0x02000213 RID: 531
		public enum Resolution
		{
			// Token: 0x04000C5B RID: 3163
			Low,
			// Token: 0x04000C5C RID: 3164
			High
		}

		// Token: 0x02000214 RID: 532
		public enum BlurType
		{
			// Token: 0x04000C5E RID: 3166
			Standard,
			// Token: 0x04000C5F RID: 3167
			Sgx
		}
	}
}
