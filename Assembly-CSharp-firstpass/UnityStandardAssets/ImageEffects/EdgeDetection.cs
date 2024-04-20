using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000019 RID: 25
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
	public class EdgeDetection : PostEffectsBase
	{
		// Token: 0x0600016B RID: 363 RVA: 0x000124E0 File Offset: 0x000106E0
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.edgeDetectMaterial = base.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
			if (this.mode != this.oldMode)
			{
				this.SetCameraFlag();
			}
			this.oldMode = this.mode;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00012541 File Offset: 0x00010741
		private new void Start()
		{
			this.oldMode = this.mode;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00012550 File Offset: 0x00010750
		private void SetCameraFlag()
		{
			if (this.mode == EdgeDetection.EdgeDetectMode.SobelDepth || this.mode == EdgeDetection.EdgeDetectMode.SobelDepthThin)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
				return;
			}
			if (this.mode == EdgeDetection.EdgeDetectMode.TriangleDepthNormals || this.mode == EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000125A7 File Offset: 0x000107A7
		private void OnEnable()
		{
			this.SetCameraFlag();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000125B0 File Offset: 0x000107B0
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector2 vector = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
			this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
			this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
			this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
			this.edgeDetectMaterial.SetVector("_BgColor", this.edgesOnlyBgColor);
			this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
			this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
			Graphics.Blit(source, destination, this.edgeDetectMaterial, (int)this.mode);
		}

		// Token: 0x0400014D RID: 333
		public EdgeDetection.EdgeDetectMode mode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x0400014E RID: 334
		public float sensitivityDepth = 1f;

		// Token: 0x0400014F RID: 335
		public float sensitivityNormals = 1f;

		// Token: 0x04000150 RID: 336
		public float lumThreshold = 0.2f;

		// Token: 0x04000151 RID: 337
		public float edgeExp = 1f;

		// Token: 0x04000152 RID: 338
		public float sampleDist = 1f;

		// Token: 0x04000153 RID: 339
		public float edgesOnly;

		// Token: 0x04000154 RID: 340
		public Color edgesOnlyBgColor = Color.white;

		// Token: 0x04000155 RID: 341
		public Shader edgeDetectShader;

		// Token: 0x04000156 RID: 342
		private Material edgeDetectMaterial;

		// Token: 0x04000157 RID: 343
		private EdgeDetection.EdgeDetectMode oldMode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x0200021E RID: 542
		public enum EdgeDetectMode
		{
			// Token: 0x04000C83 RID: 3203
			TriangleDepthNormals,
			// Token: 0x04000C84 RID: 3204
			RobertsCrossDepthNormals,
			// Token: 0x04000C85 RID: 3205
			SobelDepth,
			// Token: 0x04000C86 RID: 3206
			SobelDepthThin,
			// Token: 0x04000C87 RID: 3207
			TriangleLuminance
		}
	}
}
