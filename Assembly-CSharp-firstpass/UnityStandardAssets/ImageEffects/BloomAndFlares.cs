using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200000C RID: 12
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/BloomAndFlares (3.5, Deprecated)")]
	public class BloomAndFlares : PostEffectsBase
	{
		// Token: 0x0600010E RID: 270 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.vignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
			this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
			this.addBrightStuffBlendOneOneMaterial = base.CheckShaderAndCreateMaterial(this.addBrightStuffOneOneShader, this.addBrightStuffBlendOneOneMaterial);
			this.hollywoodFlaresMaterial = base.CheckShaderAndCreateMaterial(this.hollywoodFlaresShader, this.hollywoodFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000D884 File Offset: 0x0000BA84
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.doHdr = false;
			if (this.hdr == HDRBloomMode.Auto)
			{
				this.doHdr = (source.format == RenderTextureFormat.ARGBHalf && base.GetComponent<Camera>().allowHDR);
			}
			else
			{
				this.doHdr = (this.hdr == HDRBloomMode.On);
			}
			this.doHdr = (this.doHdr && this.supportHDRTextures);
			BloomScreenBlendMode pass = this.screenBlendMode;
			if (this.doHdr)
			{
				pass = BloomScreenBlendMode.Add;
			}
			RenderTextureFormat format = this.doHdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.Default;
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
			RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
			RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
			float num = 1f * (float)source.width / (1f * (float)source.height);
			float num2 = 0.001953125f;
			Graphics.Blit(source, temporary, this.screenBlend, 2);
			Graphics.Blit(temporary, temporary2, this.screenBlend, 2);
			RenderTexture.ReleaseTemporary(temporary);
			this.BrightFilter(this.bloomThreshold, this.useSrcAlphaAsMask, temporary2, temporary3);
			temporary2.DiscardContents();
			if (this.bloomBlurIterations < 1)
			{
				this.bloomBlurIterations = 1;
			}
			for (int i = 0; i < this.bloomBlurIterations; i++)
			{
				float num3 = (1f + (float)i * 0.5f) * this.sepBlurSpread;
				this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, num3 * num2, 0f, 0f));
				RenderTexture renderTexture = (i == 0) ? temporary3 : temporary2;
				Graphics.Blit(renderTexture, temporary4, this.separableBlurMaterial);
				renderTexture.DiscardContents();
				this.separableBlurMaterial.SetVector("offsets", new Vector4(num3 / num * num2, 0f, 0f, 0f));
				Graphics.Blit(temporary4, temporary2, this.separableBlurMaterial);
				temporary4.DiscardContents();
			}
			if (this.lensflares)
			{
				if (this.lensflareMode == LensflareStyle34.Ghosting)
				{
					this.BrightFilter(this.lensflareThreshold, 0f, temporary2, temporary4);
					temporary2.DiscardContents();
					this.Vignette(0.975f, temporary4, temporary3);
					temporary4.DiscardContents();
					this.BlendFlares(temporary3, temporary2);
					temporary3.DiscardContents();
				}
				else
				{
					this.hollywoodFlaresMaterial.SetVector("_threshold", new Vector4(this.lensflareThreshold, 1f / (1f - this.lensflareThreshold), 0f, 0f));
					this.hollywoodFlaresMaterial.SetVector("tintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 2);
					temporary4.DiscardContents();
					Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 3);
					temporary3.DiscardContents();
					this.hollywoodFlaresMaterial.SetVector("offsets", new Vector4(this.sepBlurSpread * 1f / num * num2, 0f, 0f, 0f));
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
					temporary4.DiscardContents();
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 2f);
					Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 1);
					temporary3.DiscardContents();
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 4f);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
					temporary4.DiscardContents();
					if (this.lensflareMode == LensflareStyle34.Anamorphic)
					{
						for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
						{
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
							temporary3.DiscardContents();
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
							temporary4.DiscardContents();
						}
						this.AddTo(1f, temporary3, temporary2);
						temporary3.DiscardContents();
					}
					else
					{
						for (int k = 0; k < this.hollywoodFlareBlurIterations; k++)
						{
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
							temporary3.DiscardContents();
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
							temporary4.DiscardContents();
						}
						this.Vignette(1f, temporary3, temporary4);
						temporary3.DiscardContents();
						this.BlendFlares(temporary4, temporary3);
						temporary4.DiscardContents();
						this.AddTo(1f, temporary3, temporary2);
						temporary3.DiscardContents();
					}
				}
			}
			this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
			this.screenBlend.SetTexture("_ColorBuffer", source);
			Graphics.Blit(temporary2, destination, this.screenBlend, (int)pass);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
			RenderTexture.ReleaseTemporary(temporary4);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000DE92 File Offset: 0x0000C092
		private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.addBrightStuffBlendOneOneMaterial.SetFloat("_Intensity", intensity_);
			Graphics.Blit(from, to, this.addBrightStuffBlendOneOneMaterial);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
		private void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000E000 File Offset: 0x0000C200
		private void BrightFilter(float thresh, float useAlphaAsMask, RenderTexture from, RenderTexture to)
		{
			if (this.doHdr)
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f, 0f, 0f));
			}
			else
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f / (1f - thresh), 0f, 0f));
			}
			this.brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask);
			Graphics.Blit(from, to, this.brightPassFilterMaterial);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000E088 File Offset: 0x0000C288
		private void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				Graphics.Blit(from, to, this.screenBlend, 3);
				return;
			}
			this.vignetteMaterial.SetFloat("vignetteIntensity", amount);
			Graphics.Blit(from, to, this.vignetteMaterial);
		}

		// Token: 0x0400007D RID: 125
		public TweakMode34 tweakMode;

		// Token: 0x0400007E RID: 126
		public BloomScreenBlendMode screenBlendMode = BloomScreenBlendMode.Add;

		// Token: 0x0400007F RID: 127
		public HDRBloomMode hdr;

		// Token: 0x04000080 RID: 128
		private bool doHdr;

		// Token: 0x04000081 RID: 129
		public float sepBlurSpread = 1.5f;

		// Token: 0x04000082 RID: 130
		public float useSrcAlphaAsMask = 0.5f;

		// Token: 0x04000083 RID: 131
		public float bloomIntensity = 1f;

		// Token: 0x04000084 RID: 132
		public float bloomThreshold = 0.5f;

		// Token: 0x04000085 RID: 133
		public int bloomBlurIterations = 2;

		// Token: 0x04000086 RID: 134
		public bool lensflares;

		// Token: 0x04000087 RID: 135
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x04000088 RID: 136
		public LensflareStyle34 lensflareMode = LensflareStyle34.Anamorphic;

		// Token: 0x04000089 RID: 137
		public float hollyStretchWidth = 3.5f;

		// Token: 0x0400008A RID: 138
		public float lensflareIntensity = 1f;

		// Token: 0x0400008B RID: 139
		public float lensflareThreshold = 0.3f;

		// Token: 0x0400008C RID: 140
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x0400008D RID: 141
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x0400008E RID: 142
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x0400008F RID: 143
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x04000090 RID: 144
		public Texture2D lensFlareVignetteMask;

		// Token: 0x04000091 RID: 145
		public Shader lensFlareShader;

		// Token: 0x04000092 RID: 146
		private Material lensFlareMaterial;

		// Token: 0x04000093 RID: 147
		public Shader vignetteShader;

		// Token: 0x04000094 RID: 148
		private Material vignetteMaterial;

		// Token: 0x04000095 RID: 149
		public Shader separableBlurShader;

		// Token: 0x04000096 RID: 150
		private Material separableBlurMaterial;

		// Token: 0x04000097 RID: 151
		public Shader addBrightStuffOneOneShader;

		// Token: 0x04000098 RID: 152
		private Material addBrightStuffBlendOneOneMaterial;

		// Token: 0x04000099 RID: 153
		public Shader screenBlendShader;

		// Token: 0x0400009A RID: 154
		private Material screenBlend;

		// Token: 0x0400009B RID: 155
		public Shader hollywoodFlaresShader;

		// Token: 0x0400009C RID: 156
		private Material hollywoodFlaresMaterial;

		// Token: 0x0400009D RID: 157
		public Shader brightPassFilterShader;

		// Token: 0x0400009E RID: 158
		private Material brightPassFilterMaterial;
	}
}
