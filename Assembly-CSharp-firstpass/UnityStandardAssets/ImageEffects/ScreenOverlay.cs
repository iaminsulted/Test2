using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000025 RID: 37
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Screen Overlay")]
	public class ScreenOverlay : PostEffectsBase
	{
		// Token: 0x060001AE RID: 430 RVA: 0x00014903 File Offset: 0x00012B03
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0001493C File Offset: 0x00012B3C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector4 value = new Vector4(1f, 0f, 0f, 1f);
			this.overlayMaterial.SetVector("_UV_Transform", value);
			this.overlayMaterial.SetFloat("_Intensity", this.intensity);
			this.overlayMaterial.SetTexture("_Overlay", this.texture);
			Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
		}

		// Token: 0x04000197 RID: 407
		public ScreenOverlay.OverlayBlendMode blendMode = ScreenOverlay.OverlayBlendMode.Overlay;

		// Token: 0x04000198 RID: 408
		public float intensity = 1f;

		// Token: 0x04000199 RID: 409
		public Texture2D texture;

		// Token: 0x0400019A RID: 410
		public Shader overlayShader;

		// Token: 0x0400019B RID: 411
		private Material overlayMaterial;

		// Token: 0x0200021F RID: 543
		public enum OverlayBlendMode
		{
			// Token: 0x04000C89 RID: 3209
			Additive,
			// Token: 0x04000C8A RID: 3210
			ScreenBlend,
			// Token: 0x04000C8B RID: 3211
			Multiply,
			// Token: 0x04000C8C RID: 3212
			Overlay,
			// Token: 0x04000C8D RID: 3213
			AlphaBlend
		}
	}
}
