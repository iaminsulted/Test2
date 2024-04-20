using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000026 RID: 38
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Obscurance")]
	internal class ScreenSpaceAmbientObscurance : PostEffectsBase
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x000149DE File Offset: 0x00012BDE
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.aoMaterial = base.CheckShaderAndCreateMaterial(this.aoShader, this.aoMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00014A14 File Offset: 0x00012C14
		private void OnDisable()
		{
			if (this.aoMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.aoMaterial);
			}
			this.aoMaterial = null;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00014A38 File Offset: 0x00012C38
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Matrix4x4 projectionMatrix = base.GetComponent<Camera>().projectionMatrix;
			Matrix4x4 inverse = projectionMatrix.inverse;
			Vector4 value = new Vector4(-2f / ((float)Screen.width * projectionMatrix[0]), -2f / ((float)Screen.height * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
			this.aoMaterial.SetVector("_ProjInfo", value);
			this.aoMaterial.SetMatrix("_ProjectionInv", inverse);
			this.aoMaterial.SetTexture("_Rand", this.rand);
			this.aoMaterial.SetFloat("_Radius", this.radius);
			this.aoMaterial.SetFloat("_Radius2", this.radius * this.radius);
			this.aoMaterial.SetFloat("_Intensity", this.intensity);
			this.aoMaterial.SetFloat("_BlurFilterDistance", this.blurFilterDistance);
			int width = source.width;
			int height = source.height;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width >> this.downsample, height >> this.downsample);
			Graphics.Blit(source, renderTexture, this.aoMaterial, 0);
			if (this.downsample > 0)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, temporary, this.aoMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.aoMaterial.SetVector("_Axis", new Vector2(1f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, temporary, this.aoMaterial, 1);
				RenderTexture.ReleaseTemporary(renderTexture);
				this.aoMaterial.SetVector("_Axis", new Vector2(0f, 1f));
				renderTexture = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(temporary, renderTexture, this.aoMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary);
			}
			this.aoMaterial.SetTexture("_AOTex", renderTexture);
			Graphics.Blit(source, destination, this.aoMaterial, 2);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0400019C RID: 412
		[Range(0f, 3f)]
		public float intensity = 0.5f;

		// Token: 0x0400019D RID: 413
		[Range(0.1f, 3f)]
		public float radius = 0.2f;

		// Token: 0x0400019E RID: 414
		[Range(0f, 3f)]
		public int blurIterations = 1;

		// Token: 0x0400019F RID: 415
		[Range(0f, 5f)]
		public float blurFilterDistance = 1.25f;

		// Token: 0x040001A0 RID: 416
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x040001A1 RID: 417
		public Texture2D rand;

		// Token: 0x040001A2 RID: 418
		public Shader aoShader;

		// Token: 0x040001A3 RID: 419
		private Material aoMaterial;
	}
}
