using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000011 RID: 17
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")]
	public class ColorCorrectionCurves : PostEffectsBase
	{
		// Token: 0x06000130 RID: 304 RVA: 0x0000F432 File Offset: 0x0000D632
		private new void Start()
		{
			base.Start();
			this.updateTexturesOnStartup = true;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000F441 File Offset: 0x0000D641
		private void Awake()
		{
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000F444 File Offset: 0x0000D644
		public override bool CheckResources()
		{
			base.CheckSupport(this.mode == ColorCorrectionCurves.ColorCorrectionMode.Advanced);
			this.ccMaterial = base.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
			this.ccDepthMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
			this.selectiveCcMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
			if (!this.rgbChannelTex)
			{
				this.rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.rgbDepthChannelTex)
			{
				this.rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.zCurveTex)
			{
				this.zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
			}
			this.rgbChannelTex.hideFlags = HideFlags.DontSave;
			this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
			this.zCurveTex.hideFlags = HideFlags.DontSave;
			this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000F56C File Offset: 0x0000D76C
		public void UpdateParameters()
		{
			this.CheckResources();
			if (this.redChannel != null && this.greenChannel != null && this.blueChannel != null)
			{
				for (float num = 0f; num <= 1f; num += 0.003921569f)
				{
					float num2 = Mathf.Clamp(this.redChannel.Evaluate(num), 0f, 1f);
					float num3 = Mathf.Clamp(this.greenChannel.Evaluate(num), 0f, 1f);
					float num4 = Mathf.Clamp(this.blueChannel.Evaluate(num), 0f, 1f);
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
					float num5 = Mathf.Clamp(this.zCurve.Evaluate(num), 0f, 1f);
					this.zCurveTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num5, num5, num5));
					num2 = Mathf.Clamp(this.depthRedChannel.Evaluate(num), 0f, 1f);
					num3 = Mathf.Clamp(this.depthGreenChannel.Evaluate(num), 0f, 1f);
					num4 = Mathf.Clamp(this.depthBlueChannel.Evaluate(num), 0f, 1f);
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
				}
				this.rgbChannelTex.Apply();
				this.rgbDepthChannelTex.Apply();
				this.zCurveTex.Apply();
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000F78F File Offset: 0x0000D98F
		private void UpdateTextures()
		{
			this.UpdateParameters();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000F798 File Offset: 0x0000D998
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.updateTexturesOnStartup)
			{
				this.UpdateParameters();
				this.updateTexturesOnStartup = false;
			}
			if (this.useDepthCorrection)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			RenderTexture renderTexture = destination;
			if (this.selectiveCc)
			{
				renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			}
			if (this.useDepthCorrection)
			{
				this.ccDepthMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccDepthMaterial.SetTexture("_ZCurve", this.zCurveTex);
				this.ccDepthMaterial.SetTexture("_RgbDepthTex", this.rgbDepthChannelTex);
				this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccDepthMaterial);
			}
			else
			{
				this.ccMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccMaterial);
			}
			if (this.selectiveCc)
			{
				this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
				this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
				Graphics.Blit(renderTexture, destination, this.selectiveCcMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040000CF RID: 207
		public AnimationCurve redChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040000D0 RID: 208
		public AnimationCurve greenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040000D1 RID: 209
		public AnimationCurve blueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040000D2 RID: 210
		public bool useDepthCorrection;

		// Token: 0x040000D3 RID: 211
		public AnimationCurve zCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040000D4 RID: 212
		public AnimationCurve depthRedChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040000D5 RID: 213
		public AnimationCurve depthGreenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040000D6 RID: 214
		public AnimationCurve depthBlueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040000D7 RID: 215
		private Material ccMaterial;

		// Token: 0x040000D8 RID: 216
		private Material ccDepthMaterial;

		// Token: 0x040000D9 RID: 217
		private Material selectiveCcMaterial;

		// Token: 0x040000DA RID: 218
		private Texture2D rgbChannelTex;

		// Token: 0x040000DB RID: 219
		private Texture2D rgbDepthChannelTex;

		// Token: 0x040000DC RID: 220
		private Texture2D zCurveTex;

		// Token: 0x040000DD RID: 221
		public float saturation = 1f;

		// Token: 0x040000DE RID: 222
		public bool selectiveCc;

		// Token: 0x040000DF RID: 223
		public Color selectiveFromColor = Color.white;

		// Token: 0x040000E0 RID: 224
		public Color selectiveToColor = Color.white;

		// Token: 0x040000E1 RID: 225
		public ColorCorrectionCurves.ColorCorrectionMode mode;

		// Token: 0x040000E2 RID: 226
		public bool updateTextures = true;

		// Token: 0x040000E3 RID: 227
		public Shader colorCorrectionCurvesShader;

		// Token: 0x040000E4 RID: 228
		public Shader simpleColorCorrectionCurvesShader;

		// Token: 0x040000E5 RID: 229
		public Shader colorCorrectionSelectiveShader;

		// Token: 0x040000E6 RID: 230
		private bool updateTexturesOnStartup = true;

		// Token: 0x02000217 RID: 535
		public enum ColorCorrectionMode
		{
			// Token: 0x04000C6A RID: 3178
			Simple,
			// Token: 0x04000C6B RID: 3179
			Advanced
		}
	}
}
