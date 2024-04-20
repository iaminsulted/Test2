using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000027 RID: 39
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Occlusion")]
	public class ScreenSpaceAmbientOcclusion : MonoBehaviour
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x00014CC5 File Offset: 0x00012EC5
		private static Material CreateMaterial(Shader shader)
		{
			if (!shader)
			{
				return null;
			}
			return new Material(shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00014CDF File Offset: 0x00012EDF
		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00014CF2 File Offset: 0x00012EF2
		private void OnDisable()
		{
			ScreenSpaceAmbientOcclusion.DestroyMaterial(this.m_SSAOMaterial);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00014D00 File Offset: 0x00012F00
		private void Start()
		{
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
			{
				this.m_Supported = false;
				base.enabled = false;
				return;
			}
			this.m_Supported = true;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00014D5B File Offset: 0x00012F5B
		private void OnEnable()
		{
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00014D70 File Offset: 0x00012F70
		private void CreateMaterials()
		{
			if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
			{
				this.m_SSAOMaterial = ScreenSpaceAmbientOcclusion.CreateMaterial(this.m_SSAOShader);
				this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00014DC0 File Offset: 0x00012FC0
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.m_Supported || !this.m_SSAOShader.isSupported)
			{
				base.enabled = false;
				return;
			}
			this.CreateMaterials();
			this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
			this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
			this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
			this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
			this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
			this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
			float fieldOfView = base.GetComponent<Camera>().fieldOfView;
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			float num = Mathf.Tan(fieldOfView * 0.017453292f * 0.5f) * farClipPlane;
			float x = num * base.GetComponent<Camera>().aspect;
			this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(x, num, farClipPlane));
			int num2;
			int num3;
			if (this.m_RandomTexture)
			{
				num2 = this.m_RandomTexture.width;
				num3 = this.m_RandomTexture.height;
			}
			else
			{
				num2 = 1;
				num3 = 1;
			}
			this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.width / (float)num2, (float)renderTexture.height / (float)num3, 0f));
			this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
			bool flag = this.m_Blur > 0;
			Graphics.Blit(flag ? null : source, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
			if (flag)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
				Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(renderTexture);
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
				this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
				this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
				Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary);
				renderTexture = temporary2;
			}
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040001A4 RID: 420
		[Range(0.05f, 1f)]
		public float m_Radius = 0.4f;

		// Token: 0x040001A5 RID: 421
		public ScreenSpaceAmbientOcclusion.SSAOSamples m_SampleCount = ScreenSpaceAmbientOcclusion.SSAOSamples.Medium;

		// Token: 0x040001A6 RID: 422
		[Range(0.5f, 4f)]
		public float m_OcclusionIntensity = 1.5f;

		// Token: 0x040001A7 RID: 423
		[Range(0f, 4f)]
		public int m_Blur = 2;

		// Token: 0x040001A8 RID: 424
		[Range(1f, 6f)]
		public int m_Downsampling = 2;

		// Token: 0x040001A9 RID: 425
		[Range(0.2f, 2f)]
		public float m_OcclusionAttenuation = 1f;

		// Token: 0x040001AA RID: 426
		[Range(1E-05f, 0.5f)]
		public float m_MinZ = 0.01f;

		// Token: 0x040001AB RID: 427
		public Shader m_SSAOShader;

		// Token: 0x040001AC RID: 428
		private Material m_SSAOMaterial;

		// Token: 0x040001AD RID: 429
		public Texture2D m_RandomTexture;

		// Token: 0x040001AE RID: 430
		private bool m_Supported;

		// Token: 0x02000220 RID: 544
		public enum SSAOSamples
		{
			// Token: 0x04000C8F RID: 3215
			Low,
			// Token: 0x04000C90 RID: 3216
			Medium,
			// Token: 0x04000C91 RID: 3217
			High
		}
	}
}
