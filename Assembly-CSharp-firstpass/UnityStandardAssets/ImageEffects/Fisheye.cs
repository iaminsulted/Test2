using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200001A RID: 26
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Displacement/Fisheye")]
	public class Fisheye : PostEffectsBase
	{
		// Token: 0x06000171 RID: 369 RVA: 0x000126F7 File Offset: 0x000108F7
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fisheyeMaterial = base.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00012730 File Offset: 0x00010930
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 0.15625f;
			float num2 = (float)source.width * 1f / ((float)source.height * 1f);
			this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num, this.strengthY * num, this.strengthX * num2 * num, this.strengthY * num));
			Graphics.Blit(source, destination, this.fisheyeMaterial);
		}

		// Token: 0x04000158 RID: 344
		[Range(0f, 1.5f)]
		public float strengthX = 0.05f;

		// Token: 0x04000159 RID: 345
		[Range(0f, 1.5f)]
		public float strengthY = 0.05f;

		// Token: 0x0400015A RID: 346
		public Shader fishEyeShader;

		// Token: 0x0400015B RID: 347
		private Material fisheyeMaterial;
	}
}
