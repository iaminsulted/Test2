using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000018 RID: 24
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
	public class DepthOfFieldDeprecated : PostEffectsBase
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00011480 File Offset: 0x0000F680
		private void CreateMaterials()
		{
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00011504 File Offset: 0x0000F704
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000115A3 File Offset: 0x0000F7A3
		private void OnDisable()
		{
			Quads.Cleanup();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000115AA File Offset: 0x0000F7AA
		private void OnEnable()
		{
			this._camera = base.GetComponent<Camera>();
			this._camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000115CC File Offset: 0x0000F7CC
		private float FocalDistance01(float worldDist)
		{
			return this._camera.WorldToViewportPoint((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00011638 File Offset: 0x0000F838
		private int GetDividerBasedOnQuality()
		{
			int result = 1;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Medium)
			{
				result = 2;
			}
			else if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00011660 File Offset: 0x0000F860
		private int GetLowResolutionDividerBasedOnQuality(int baseDivider)
		{
			int num = baseDivider;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.High)
			{
				num *= 2;
			}
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0001168C File Offset: 0x0000F88C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.smoothness < 0.1f)
			{
				this.smoothness = 0.1f;
			}
			this.bokeh = (this.bokeh && this.bokehSupport);
			float num = this.bokeh ? DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR : 1f;
			bool flag = this.quality > DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;
			float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
			if (this.simpleTweakMode)
			{
				this.focalDistance01 = (this.objectFocus ? (this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane) : this.FocalDistance01(this.focalPoint));
				this.focalStartCurve = this.focalDistance01 * this.smoothness;
				this.focalEndCurve = this.focalStartCurve;
				flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
			}
			else
			{
				if (this.objectFocus)
				{
					Vector3 vector = this._camera.WorldToViewportPoint(this.objectFocus.position);
					vector.z /= this._camera.farClipPlane;
					this.focalDistance01 = vector.z;
				}
				else
				{
					this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
				}
				this.focalStartCurve = this.focalZStartCurve;
				this.focalEndCurve = this.focalZEndCurve;
				flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
			}
			this.widthOverHeight = 1f * (float)source.width / (1f * (float)source.height);
			this.oneOverBaseSize = 0.001953125f;
			this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
			this.dofMaterial.SetVector("_CurveParams", new Vector4(this.simpleTweakMode ? (1f / this.focalStartCurve) : this.focalStartCurve, this.simpleTweakMode ? (1f / this.focalEndCurve) : this.focalEndCurve, num2 * 0.5f, this.focalDistance01));
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), 0f, 0f));
			int dividerBasedOnQuality = this.GetDividerBasedOnQuality();
			int lowResolutionDividerBasedOnQuality = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality);
			this.AllocateTextures(flag, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality);
			Graphics.Blit(source, source, this.dofMaterial, 3);
			this.Downsample(source, this.mediumRezWorkTexture);
			this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 4, this.maxBlurSpread);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0f));
				Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
				Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num);
			}
			else
			{
				this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
			}
			this.dofBlurMaterial.SetTexture("_TapLow", this.lowRezWorkTexture);
			this.dofBlurMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(null, this.finalDefocus, this.dofBlurMaterial, 3);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
			}
			this.dofMaterial.SetTexture("_TapLowBackground", this.finalDefocus);
			this.dofMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(source, flag ? this.foregroundTexture : destination, this.dofMaterial, this.visualize ? 2 : 0);
			if (flag)
			{
				Graphics.Blit(this.foregroundTexture, source, this.dofMaterial, 5);
				this.Downsample(source, this.mediumRezWorkTexture);
				this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 2, this.maxBlurSpread);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0f, 0f));
					Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
					Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
					this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num);
				}
				else
				{
					this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
				}
				Graphics.Blit(this.lowRezWorkTexture, this.finalDefocus);
				this.dofMaterial.SetTexture("_TapLowForeground", this.finalDefocus);
				Graphics.Blit(source, destination, this.dofMaterial, this.visualize ? 1 : 4);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
				}
			}
			this.ReleaseTextures();
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00011C4C File Offset: 0x0000FE4C
		private void Blur(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00011D8C File Offset: 0x0000FF8C
		private void BlurFg(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			this.dofBlurMaterial.SetTexture("_TapHigh", from);
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00011EE0 File Offset: 0x000100E0
		private void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
		{
			this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(from, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(to, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00011FFC File Offset: 0x000101FC
		private void Downsample(RenderTexture from, RenderTexture to)
		{
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)to.width), 1f / (1f * (float)to.height), 0f, 0f));
			Graphics.Blit(from, to, this.dofMaterial, DepthOfFieldDeprecated.SMOOTH_DOWNSAMPLE_PASS);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00012060 File Offset: 0x00010260
		private void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
		{
			if (this.bokehMaterial)
			{
				Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
				RenderTexture.active = tempTex;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
				GL.PushMatrix();
				GL.LoadIdentity();
				bokehInfo.filterMode = FilterMode.Point;
				float num = (float)bokehInfo.width * 1f / ((float)bokehInfo.height * 1f);
				float num2 = 2f / (1f * (float)bokehInfo.width);
				num2 += this.bokehScale * this.maxBlurSpread * DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
				this.bokehMaterial.SetTexture("_Source", bokehInfo);
				this.bokehMaterial.SetTexture("_MainTex", this.bokehTexture);
				this.bokehMaterial.SetVector("_ArScale", new Vector4(num2, num2 * num, 0.5f, 0.5f * num));
				this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
				this.bokehMaterial.SetPass(0);
				foreach (Mesh mesh in meshes)
				{
					if (mesh)
					{
						Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
					}
				}
				GL.PopMatrix();
				Graphics.Blit(tempTex, finalTarget, this.dofMaterial, 8);
				bokehInfo.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000121C4 File Offset: 0x000103C4
		private void ReleaseTextures()
		{
			if (this.foregroundTexture)
			{
				RenderTexture.ReleaseTemporary(this.foregroundTexture);
			}
			if (this.finalDefocus)
			{
				RenderTexture.ReleaseTemporary(this.finalDefocus);
			}
			if (this.mediumRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
			}
			if (this.lowRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
			}
			if (this.bokehSource)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource);
			}
			if (this.bokehSource2)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource2);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00012264 File Offset: 0x00010464
		private void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
		{
			this.foregroundTexture = null;
			if (blurForeground)
			{
				this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
			}
			this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
			this.bokehSource = null;
			this.bokehSource2 = null;
			if (this.bokeh)
			{
				this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource.filterMode = FilterMode.Bilinear;
				this.bokehSource2.filterMode = FilterMode.Bilinear;
				RenderTexture.active = this.bokehSource2;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
			}
			source.filterMode = FilterMode.Bilinear;
			this.finalDefocus.filterMode = FilterMode.Bilinear;
			this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
			this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
			if (this.foregroundTexture)
			{
				this.foregroundTexture.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x04000122 RID: 290
		private static int SMOOTH_DOWNSAMPLE_PASS = 6;

		// Token: 0x04000123 RID: 291
		private static float BOKEH_EXTRA_BLUR = 2f;

		// Token: 0x04000124 RID: 292
		public DepthOfFieldDeprecated.Dof34QualitySetting quality = DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;

		// Token: 0x04000125 RID: 293
		public DepthOfFieldDeprecated.DofResolution resolution = DepthOfFieldDeprecated.DofResolution.Low;

		// Token: 0x04000126 RID: 294
		public bool simpleTweakMode = true;

		// Token: 0x04000127 RID: 295
		public float focalPoint = 1f;

		// Token: 0x04000128 RID: 296
		public float smoothness = 0.5f;

		// Token: 0x04000129 RID: 297
		public float focalZDistance;

		// Token: 0x0400012A RID: 298
		public float focalZStartCurve = 1f;

		// Token: 0x0400012B RID: 299
		public float focalZEndCurve = 1f;

		// Token: 0x0400012C RID: 300
		private float focalStartCurve = 2f;

		// Token: 0x0400012D RID: 301
		private float focalEndCurve = 2f;

		// Token: 0x0400012E RID: 302
		private float focalDistance01 = 0.1f;

		// Token: 0x0400012F RID: 303
		public Transform objectFocus;

		// Token: 0x04000130 RID: 304
		public float focalSize;

		// Token: 0x04000131 RID: 305
		public DepthOfFieldDeprecated.DofBlurriness bluriness = DepthOfFieldDeprecated.DofBlurriness.High;

		// Token: 0x04000132 RID: 306
		public float maxBlurSpread = 1.75f;

		// Token: 0x04000133 RID: 307
		public float foregroundBlurExtrude = 1.15f;

		// Token: 0x04000134 RID: 308
		public Shader dofBlurShader;

		// Token: 0x04000135 RID: 309
		private Material dofBlurMaterial;

		// Token: 0x04000136 RID: 310
		public Shader dofShader;

		// Token: 0x04000137 RID: 311
		private Material dofMaterial;

		// Token: 0x04000138 RID: 312
		public bool visualize;

		// Token: 0x04000139 RID: 313
		public DepthOfFieldDeprecated.BokehDestination bokehDestination = DepthOfFieldDeprecated.BokehDestination.Background;

		// Token: 0x0400013A RID: 314
		private float widthOverHeight = 1.25f;

		// Token: 0x0400013B RID: 315
		private float oneOverBaseSize = 0.001953125f;

		// Token: 0x0400013C RID: 316
		public bool bokeh;

		// Token: 0x0400013D RID: 317
		public bool bokehSupport = true;

		// Token: 0x0400013E RID: 318
		public Shader bokehShader;

		// Token: 0x0400013F RID: 319
		public Texture2D bokehTexture;

		// Token: 0x04000140 RID: 320
		public float bokehScale = 2.4f;

		// Token: 0x04000141 RID: 321
		public float bokehIntensity = 0.15f;

		// Token: 0x04000142 RID: 322
		public float bokehThresholdContrast = 0.1f;

		// Token: 0x04000143 RID: 323
		public float bokehThresholdLuminance = 0.55f;

		// Token: 0x04000144 RID: 324
		public int bokehDownsample = 1;

		// Token: 0x04000145 RID: 325
		private Material bokehMaterial;

		// Token: 0x04000146 RID: 326
		private Camera _camera;

		// Token: 0x04000147 RID: 327
		private RenderTexture foregroundTexture;

		// Token: 0x04000148 RID: 328
		private RenderTexture mediumRezWorkTexture;

		// Token: 0x04000149 RID: 329
		private RenderTexture finalDefocus;

		// Token: 0x0400014A RID: 330
		private RenderTexture lowRezWorkTexture;

		// Token: 0x0400014B RID: 331
		private RenderTexture bokehSource;

		// Token: 0x0400014C RID: 332
		private RenderTexture bokehSource2;

		// Token: 0x0200021A RID: 538
		public enum Dof34QualitySetting
		{
			// Token: 0x04000C74 RID: 3188
			OnlyBackground = 1,
			// Token: 0x04000C75 RID: 3189
			BackgroundAndForeground
		}

		// Token: 0x0200021B RID: 539
		public enum DofResolution
		{
			// Token: 0x04000C77 RID: 3191
			High = 2,
			// Token: 0x04000C78 RID: 3192
			Medium,
			// Token: 0x04000C79 RID: 3193
			Low
		}

		// Token: 0x0200021C RID: 540
		public enum DofBlurriness
		{
			// Token: 0x04000C7B RID: 3195
			Low = 1,
			// Token: 0x04000C7C RID: 3196
			High,
			// Token: 0x04000C7D RID: 3197
			VeryHigh = 4
		}

		// Token: 0x0200021D RID: 541
		public enum BokehDestination
		{
			// Token: 0x04000C7F RID: 3199
			Background = 1,
			// Token: 0x04000C80 RID: 3200
			Foreground,
			// Token: 0x04000C81 RID: 3201
			BackgroundAndForeground
		}
	}
}
