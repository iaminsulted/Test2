using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000007 RID: 7
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom")]
	public class Bloom : PostEffectsBase
	{
		// Token: 0x06000106 RID: 262 RVA: 0x0000CC78 File Offset: 0x0000AE78
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.blurAndFlaresMaterial = base.CheckShaderAndCreateMaterial(this.blurAndFlaresShader, this.blurAndFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000CD04 File Offset: 0x0000AF04
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.doHdr = false;
			if (this.hdr == Bloom.HDRBloomMode.Auto)
			{
				this.doHdr = (source.format == RenderTextureFormat.ARGBHalf && base.GetComponent<Camera>().allowHDR);
			}
			else
			{
				this.doHdr = (this.hdr == Bloom.HDRBloomMode.On);
			}
			this.doHdr = (this.doHdr && this.supportHDRTextures);
			Bloom.BloomScreenBlendMode bloomScreenBlendMode = this.screenBlendMode;
			if (this.doHdr)
			{
				bloomScreenBlendMode = Bloom.BloomScreenBlendMode.Add;
			}
			RenderTextureFormat format = this.doHdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.Default;
			int width = source.width / 2;
			int height = source.height / 2;
			int width2 = source.width / 4;
			int height2 = source.height / 4;
			float num = 1f * (float)source.width / (1f * (float)source.height);
			float num2 = 0.001953125f;
			RenderTexture temporary = RenderTexture.GetTemporary(width2, height2, 0, format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, format);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.Blit(source, temporary2, this.screenBlend, 2);
				RenderTexture temporary3 = RenderTexture.GetTemporary(width2, height2, 0, format);
				Graphics.Blit(temporary2, temporary3, this.screenBlend, 2);
				Graphics.Blit(temporary3, temporary, this.screenBlend, 6);
				RenderTexture.ReleaseTemporary(temporary3);
			}
			else
			{
				Graphics.Blit(source, temporary2);
				Graphics.Blit(temporary2, temporary, this.screenBlend, 6);
			}
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture renderTexture = RenderTexture.GetTemporary(width2, height2, 0, format);
			this.BrightFilter(this.bloomThreshold * this.bloomThresholdColor, temporary, renderTexture);
			if (this.bloomBlurIterations < 1)
			{
				this.bloomBlurIterations = 1;
			}
			else if (this.bloomBlurIterations > 10)
			{
				this.bloomBlurIterations = 10;
			}
			for (int i = 0; i < this.bloomBlurIterations; i++)
			{
				float num3 = (1f + (float)i * 0.25f) * this.sepBlurSpread;
				RenderTexture temporary4 = RenderTexture.GetTemporary(width2, height2, 0, format);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, num3 * num2, 0f, 0f));
				Graphics.Blit(renderTexture, temporary4, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary4;
				temporary4 = RenderTexture.GetTemporary(width2, height2, 0, format);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num3 / num * num2, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, temporary4, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary4;
				if (this.quality > Bloom.BloomQuality.Cheap)
				{
					if (i == 0)
					{
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(renderTexture, temporary);
					}
					else
					{
						temporary.MarkRestoreExpected();
						Graphics.Blit(renderTexture, temporary, this.screenBlend, 10);
					}
				}
			}
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.SetRenderTarget(renderTexture);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(temporary, renderTexture, this.screenBlend, 6);
			}
			if (this.lensflareIntensity > Mathf.Epsilon)
			{
				RenderTexture temporary5 = RenderTexture.GetTemporary(width2, height2, 0, format);
				if (this.lensflareMode == Bloom.LensFlareStyle.Ghosting)
				{
					this.BrightFilter(this.lensflareThreshold, renderTexture, temporary5);
					if (this.quality > Bloom.BloomQuality.Cheap)
					{
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f / (1f * (float)temporary.height), 0f, 0f));
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(1.5f / (1f * (float)temporary.width), 0f, 0f, 0f));
						Graphics.SetRenderTarget(temporary5);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 4);
					}
					this.Vignette(0.975f, temporary5, temporary5);
					this.BlendFlares(temporary5, renderTexture);
				}
				else
				{
					float num4 = 1f * Mathf.Cos(this.flareRotation);
					float num5 = 1f * Mathf.Sin(this.flareRotation);
					float num6 = this.hollyStretchWidth * 1f / num * num2;
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num4, num5, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshold, 1f, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_TintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
					this.blurAndFlaresMaterial.SetFloat("_Saturation", this.lensFlareSaturation);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 2);
					temporary5.DiscardContents();
					Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 3);
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num4 * num6, num5 * num6, 0f, 0f));
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 2f);
					temporary5.DiscardContents();
					Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 4f);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 1);
					for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
					{
						num6 = this.hollyStretchWidth * 2f / num * num2;
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num6 * num4, num6 * num5, 0f, 0f));
						temporary5.DiscardContents();
						Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num6 * num4, num6 * num5, 0f, 0f));
						temporary.DiscardContents();
						Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 4);
					}
					if (this.lensflareMode == Bloom.LensFlareStyle.Anamorphic)
					{
						this.AddTo(1f, temporary, renderTexture);
					}
					else
					{
						this.Vignette(1f, temporary, temporary5);
						this.BlendFlares(temporary5, temporary);
						this.AddTo(1f, temporary, renderTexture);
					}
				}
				RenderTexture.ReleaseTemporary(temporary5);
			}
			int pass = (int)bloomScreenBlendMode;
			this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
			this.screenBlend.SetTexture("_ColorBuffer", source);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				RenderTexture temporary6 = RenderTexture.GetTemporary(width, height, 0, format);
				Graphics.Blit(renderTexture, temporary6);
				Graphics.Blit(temporary6, destination, this.screenBlend, pass);
				RenderTexture.ReleaseTemporary(temporary6);
			}
			else
			{
				Graphics.Blit(renderTexture, destination, this.screenBlend, pass);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000D466 File Offset: 0x0000B666
		private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.screenBlend.SetFloat("_Intensity", intensity_);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.screenBlend, 9);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000D490 File Offset: 0x0000B690
		private void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
		private void BrightFilter(float thresh, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", new Vector4(thresh, thresh, thresh, thresh));
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 0);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000D609 File Offset: 0x0000B809
		private void BrightFilter(Color threshColor, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", threshColor);
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 1);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000D630 File Offset: 0x0000B830
		private void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				to.MarkRestoreExpected();
				Graphics.Blit((from == to) ? null : from, to, this.screenBlend, (from == to) ? 7 : 3);
				return;
			}
			if (from != to)
			{
				Graphics.SetRenderTarget(to);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(from, to);
			}
		}

		// Token: 0x04000051 RID: 81
		public Bloom.TweakMode tweakMode;

		// Token: 0x04000052 RID: 82
		public Bloom.BloomScreenBlendMode screenBlendMode = Bloom.BloomScreenBlendMode.Add;

		// Token: 0x04000053 RID: 83
		public Bloom.HDRBloomMode hdr;

		// Token: 0x04000054 RID: 84
		private bool doHdr;

		// Token: 0x04000055 RID: 85
		public float sepBlurSpread = 2.5f;

		// Token: 0x04000056 RID: 86
		public Bloom.BloomQuality quality = Bloom.BloomQuality.High;

		// Token: 0x04000057 RID: 87
		public float bloomIntensity = 0.5f;

		// Token: 0x04000058 RID: 88
		public float bloomThreshold = 0.5f;

		// Token: 0x04000059 RID: 89
		public Color bloomThresholdColor = Color.white;

		// Token: 0x0400005A RID: 90
		public int bloomBlurIterations = 2;

		// Token: 0x0400005B RID: 91
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x0400005C RID: 92
		public float flareRotation;

		// Token: 0x0400005D RID: 93
		public Bloom.LensFlareStyle lensflareMode = Bloom.LensFlareStyle.Anamorphic;

		// Token: 0x0400005E RID: 94
		public float hollyStretchWidth = 2.5f;

		// Token: 0x0400005F RID: 95
		public float lensflareIntensity;

		// Token: 0x04000060 RID: 96
		public float lensflareThreshold = 0.3f;

		// Token: 0x04000061 RID: 97
		public float lensFlareSaturation = 0.75f;

		// Token: 0x04000062 RID: 98
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04000063 RID: 99
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x04000064 RID: 100
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04000065 RID: 101
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x04000066 RID: 102
		public Texture2D lensFlareVignetteMask;

		// Token: 0x04000067 RID: 103
		public Shader lensFlareShader;

		// Token: 0x04000068 RID: 104
		private Material lensFlareMaterial;

		// Token: 0x04000069 RID: 105
		public Shader screenBlendShader;

		// Token: 0x0400006A RID: 106
		private Material screenBlend;

		// Token: 0x0400006B RID: 107
		public Shader blurAndFlaresShader;

		// Token: 0x0400006C RID: 108
		private Material blurAndFlaresMaterial;

		// Token: 0x0400006D RID: 109
		public Shader brightPassFilterShader;

		// Token: 0x0400006E RID: 110
		private Material brightPassFilterMaterial;

		// Token: 0x0200020E RID: 526
		public enum LensFlareStyle
		{
			// Token: 0x04000C4A RID: 3146
			Ghosting,
			// Token: 0x04000C4B RID: 3147
			Anamorphic,
			// Token: 0x04000C4C RID: 3148
			Combined
		}

		// Token: 0x0200020F RID: 527
		public enum TweakMode
		{
			// Token: 0x04000C4E RID: 3150
			Basic,
			// Token: 0x04000C4F RID: 3151
			Complex
		}

		// Token: 0x02000210 RID: 528
		public enum HDRBloomMode
		{
			// Token: 0x04000C51 RID: 3153
			Auto,
			// Token: 0x04000C52 RID: 3154
			On,
			// Token: 0x04000C53 RID: 3155
			Off
		}

		// Token: 0x02000211 RID: 529
		public enum BloomScreenBlendMode
		{
			// Token: 0x04000C55 RID: 3157
			Screen,
			// Token: 0x04000C56 RID: 3158
			Add
		}

		// Token: 0x02000212 RID: 530
		public enum BloomQuality
		{
			// Token: 0x04000C58 RID: 3160
			Cheap,
			// Token: 0x04000C59 RID: 3161
			High
		}
	}
}
