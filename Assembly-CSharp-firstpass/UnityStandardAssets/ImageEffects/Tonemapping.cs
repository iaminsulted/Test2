using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200002B RID: 43
	[ImageEffectAllowedInSceneView]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Tonemapping")]
	public class Tonemapping : PostEffectsBase
	{
		// Token: 0x060001C5 RID: 453 RVA: 0x00015718 File Offset: 0x00013918
		public override bool CheckResources()
		{
			base.CheckSupport(false, true);
			this.tonemapMaterial = base.CheckShaderAndCreateMaterial(this.tonemapper, this.tonemapMaterial);
			if (!this.curveTex && this.type == Tonemapping.TonemapperType.UserCurve)
			{
				this.curveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
				this.curveTex.filterMode = FilterMode.Bilinear;
				this.curveTex.wrapMode = TextureWrapMode.Clamp;
				this.curveTex.hideFlags = HideFlags.DontSave;
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000157AC File Offset: 0x000139AC
		public float UpdateCurve()
		{
			float num = 1f;
			if (this.remapCurve.keys.Length < 1)
			{
				this.remapCurve = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(2f, 1f)
				});
			}
			if (this.remapCurve != null)
			{
				if (this.remapCurve.length > 0)
				{
					num = this.remapCurve[this.remapCurve.length - 1].time;
				}
				for (float num2 = 0f; num2 <= 1f; num2 += 0.003921569f)
				{
					float num3 = this.remapCurve.Evaluate(num2 * 1f * num);
					this.curveTex.SetPixel((int)Mathf.Floor(num2 * 255f), 0, new Color(num3, num3, num3));
				}
				this.curveTex.Apply();
			}
			return 1f / num;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000158A8 File Offset: 0x00013AA8
		private void OnDisable()
		{
			if (this.rt)
			{
				UnityEngine.Object.DestroyImmediate(this.rt);
				this.rt = null;
			}
			if (this.tonemapMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.tonemapMaterial);
				this.tonemapMaterial = null;
			}
			if (this.curveTex)
			{
				UnityEngine.Object.DestroyImmediate(this.curveTex);
				this.curveTex = null;
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00015914 File Offset: 0x00013B14
		private bool CreateInternalRenderTexture()
		{
			if (this.rt)
			{
				return false;
			}
			this.rtFormat = (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf) ? RenderTextureFormat.RGHalf : RenderTextureFormat.ARGBHalf);
			this.rt = new RenderTexture(1, 1, 0, this.rtFormat);
			this.rt.hideFlags = HideFlags.DontSave;
			return true;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00015968 File Offset: 0x00013B68
		[ImageEffectTransformsToLDR]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.exposureAdjustment = ((this.exposureAdjustment < 0.001f) ? 0.001f : this.exposureAdjustment);
			if (this.type == Tonemapping.TonemapperType.UserCurve)
			{
				float value = this.UpdateCurve();
				this.tonemapMaterial.SetFloat("_RangeScale", value);
				this.tonemapMaterial.SetTexture("_Curve", this.curveTex);
				Graphics.Blit(source, destination, this.tonemapMaterial, 4);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.SimpleReinhard)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 6);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.Hable)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 5);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.Photographic)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 8);
				return;
			}
			if (this.type == Tonemapping.TonemapperType.OptimizedHejiDawson)
			{
				this.tonemapMaterial.SetFloat("_ExposureAdjustment", 0.5f * this.exposureAdjustment);
				Graphics.Blit(source, destination, this.tonemapMaterial, 7);
				return;
			}
			bool flag = this.CreateInternalRenderTexture();
			RenderTexture temporary = RenderTexture.GetTemporary((int)this.adaptiveTextureSize, (int)this.adaptiveTextureSize, 0, this.rtFormat);
			Graphics.Blit(source, temporary);
			int num = (int)Mathf.Log((float)temporary.width * 1f, 2f);
			int num2 = 2;
			RenderTexture[] array = new RenderTexture[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = RenderTexture.GetTemporary(temporary.width / num2, temporary.width / num2, 0, this.rtFormat);
				num2 *= 2;
			}
			RenderTexture source2 = array[num - 1];
			Graphics.Blit(temporary, array[0], this.tonemapMaterial, 1);
			if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
			{
				for (int j = 0; j < num - 1; j++)
				{
					Graphics.Blit(array[j], array[j + 1], this.tonemapMaterial, 9);
					source2 = array[j + 1];
				}
			}
			else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
			{
				for (int k = 0; k < num - 1; k++)
				{
					Graphics.Blit(array[k], array[k + 1]);
					source2 = array[k + 1];
				}
			}
			this.adaptionSpeed = ((this.adaptionSpeed < 0.001f) ? 0.001f : this.adaptionSpeed);
			this.tonemapMaterial.SetFloat("_AdaptionSpeed", this.adaptionSpeed);
			this.rt.MarkRestoreExpected();
			Graphics.Blit(source2, this.rt, this.tonemapMaterial, flag ? 3 : 2);
			this.middleGrey = ((this.middleGrey < 0.001f) ? 0.001f : this.middleGrey);
			this.tonemapMaterial.SetVector("_HdrParams", new Vector4(this.middleGrey, this.middleGrey, this.middleGrey, this.white * this.white));
			this.tonemapMaterial.SetTexture("_SmallTex", this.rt);
			if (this.type == Tonemapping.TonemapperType.AdaptiveReinhard)
			{
				Graphics.Blit(source, destination, this.tonemapMaterial, 0);
			}
			else if (this.type == Tonemapping.TonemapperType.AdaptiveReinhardAutoWhite)
			{
				Graphics.Blit(source, destination, this.tonemapMaterial, 10);
			}
			else
			{
				Debug.LogError("No valid adaptive tonemapper type found!");
				Graphics.Blit(source, destination);
			}
			for (int l = 0; l < num; l++)
			{
				RenderTexture.ReleaseTemporary(array[l]);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x040001C4 RID: 452
		public Tonemapping.TonemapperType type = Tonemapping.TonemapperType.Photographic;

		// Token: 0x040001C5 RID: 453
		public Tonemapping.AdaptiveTexSize adaptiveTextureSize = Tonemapping.AdaptiveTexSize.Square256;

		// Token: 0x040001C6 RID: 454
		public AnimationCurve remapCurve;

		// Token: 0x040001C7 RID: 455
		private Texture2D curveTex;

		// Token: 0x040001C8 RID: 456
		public float exposureAdjustment = 1.5f;

		// Token: 0x040001C9 RID: 457
		public float middleGrey = 0.4f;

		// Token: 0x040001CA RID: 458
		public float white = 2f;

		// Token: 0x040001CB RID: 459
		public float adaptionSpeed = 1.5f;

		// Token: 0x040001CC RID: 460
		public Shader tonemapper;

		// Token: 0x040001CD RID: 461
		public bool validRenderTextureFormat = true;

		// Token: 0x040001CE RID: 462
		private Material tonemapMaterial;

		// Token: 0x040001CF RID: 463
		private RenderTexture rt;

		// Token: 0x040001D0 RID: 464
		private RenderTextureFormat rtFormat = RenderTextureFormat.ARGBHalf;

		// Token: 0x02000225 RID: 549
		public enum TonemapperType
		{
			// Token: 0x04000CA1 RID: 3233
			SimpleReinhard,
			// Token: 0x04000CA2 RID: 3234
			UserCurve,
			// Token: 0x04000CA3 RID: 3235
			Hable,
			// Token: 0x04000CA4 RID: 3236
			Photographic,
			// Token: 0x04000CA5 RID: 3237
			OptimizedHejiDawson,
			// Token: 0x04000CA6 RID: 3238
			AdaptiveReinhard,
			// Token: 0x04000CA7 RID: 3239
			AdaptiveReinhardAutoWhite
		}

		// Token: 0x02000226 RID: 550
		public enum AdaptiveTexSize
		{
			// Token: 0x04000CA9 RID: 3241
			Square16 = 16,
			// Token: 0x04000CAA RID: 3242
			Square32 = 32,
			// Token: 0x04000CAB RID: 3243
			Square64 = 64,
			// Token: 0x04000CAC RID: 3244
			Square128 = 128,
			// Token: 0x04000CAD RID: 3245
			Square256 = 256,
			// Token: 0x04000CAE RID: 3246
			Square512 = 512,
			// Token: 0x04000CAF RID: 3247
			Square1024 = 1024
		}
	}
}
