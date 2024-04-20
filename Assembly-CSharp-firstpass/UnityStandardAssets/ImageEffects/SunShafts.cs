using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000029 RID: 41
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Sun Shafts")]
	public class SunShafts : PostEffectsBase
	{
		// Token: 0x060001BF RID: 447 RVA: 0x00015128 File Offset: 0x00013328
		public override bool CheckResources()
		{
			base.CheckSupport(this.useDepthTexture);
			this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
			this.simpleClearMaterial = base.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00015188 File Offset: 0x00013388
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.useDepthTexture)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			int num = 4;
			if (this.resolution == SunShafts.SunShaftsResolution.Normal)
			{
				num = 2;
			}
			else if (this.resolution == SunShafts.SunShaftsResolution.High)
			{
				num = 1;
			}
			Vector3 vector = Vector3.one * 0.5f;
			if (this.sunTransform)
			{
				vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
			}
			else
			{
				vector = new Vector3(0.5f, 0.5f, 0f);
			}
			int width = source.width / num;
			int height = source.height / num;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			this.sunShaftsMaterial.SetVector("_SunThreshold", this.sunThreshold);
			if (!this.useDepthTexture)
			{
				RenderTextureFormat format = base.GetComponent<Camera>().allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, format);
				RenderTexture.active = temporary2;
				GL.ClearWithSkybox(false, base.GetComponent<Camera>());
				this.sunShaftsMaterial.SetTexture("_Skybox", temporary2);
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary2);
			}
			else
			{
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
			}
			base.DrawBorder(temporary, this.simpleClearMaterial);
			this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
			float num2 = this.sunShaftBlurRadius * 0.0013020834f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			for (int i = 0; i < this.radialBlurIterations; i++)
			{
				RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0);
				Graphics.Blit(temporary, temporary3, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary);
				num2 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
				temporary = RenderTexture.GetTemporary(width, height, 0);
				Graphics.Blit(temporary3, temporary, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary3);
				num2 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			}
			if (vector.z >= 0f)
			{
				this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
			}
			else
			{
				this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
			}
			this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
			Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode == SunShafts.ShaftsScreenBlendMode.Screen) ? 0 : 4);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x040001AF RID: 431
		public SunShafts.SunShaftsResolution resolution = SunShafts.SunShaftsResolution.Normal;

		// Token: 0x040001B0 RID: 432
		public SunShafts.ShaftsScreenBlendMode screenBlendMode;

		// Token: 0x040001B1 RID: 433
		public Transform sunTransform;

		// Token: 0x040001B2 RID: 434
		public int radialBlurIterations = 2;

		// Token: 0x040001B3 RID: 435
		public Color sunColor = Color.white;

		// Token: 0x040001B4 RID: 436
		public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

		// Token: 0x040001B5 RID: 437
		public float sunShaftBlurRadius = 2.5f;

		// Token: 0x040001B6 RID: 438
		public float sunShaftIntensity = 1.15f;

		// Token: 0x040001B7 RID: 439
		public float maxRadius = 0.75f;

		// Token: 0x040001B8 RID: 440
		public bool useDepthTexture = true;

		// Token: 0x040001B9 RID: 441
		public Shader sunShaftsShader;

		// Token: 0x040001BA RID: 442
		private Material sunShaftsMaterial;

		// Token: 0x040001BB RID: 443
		public Shader simpleClearShader;

		// Token: 0x040001BC RID: 444
		private Material simpleClearMaterial;

		// Token: 0x02000221 RID: 545
		public enum SunShaftsResolution
		{
			// Token: 0x04000C93 RID: 3219
			Low,
			// Token: 0x04000C94 RID: 3220
			Normal,
			// Token: 0x04000C95 RID: 3221
			High
		}

		// Token: 0x02000222 RID: 546
		public enum ShaftsScreenBlendMode
		{
			// Token: 0x04000C97 RID: 3223
			Screen,
			// Token: 0x04000C98 RID: 3224
			Add
		}
	}
}
