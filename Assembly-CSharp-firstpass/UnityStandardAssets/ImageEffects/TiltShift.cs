using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200002A RID: 42
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
	internal class TiltShift : PostEffectsBase
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x000155B6 File Offset: 0x000137B6
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.tiltShiftMaterial = base.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000155EC File Offset: 0x000137EC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.tiltShiftMaterial.SetFloat("_BlurSize", (this.maxBlurSize < 0f) ? 0f : this.maxBlurSize);
			this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
			source.filterMode = FilterMode.Bilinear;
			RenderTexture renderTexture = destination;
			if ((float)this.downsample > 0f)
			{
				renderTexture = RenderTexture.GetTemporary(source.width >> this.downsample, source.height >> this.downsample, 0, source.format);
				renderTexture.filterMode = FilterMode.Bilinear;
			}
			int num = (int)this.quality;
			num *= 2;
			Graphics.Blit(source, renderTexture, this.tiltShiftMaterial, (this.mode == TiltShift.TiltShiftMode.TiltShiftMode) ? num : (num + 1));
			if (this.downsample > 0)
			{
				this.tiltShiftMaterial.SetTexture("_Blurred", renderTexture);
				Graphics.Blit(source, destination, this.tiltShiftMaterial, 6);
			}
			if (renderTexture != destination)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040001BD RID: 445
		public TiltShift.TiltShiftMode mode;

		// Token: 0x040001BE RID: 446
		public TiltShift.TiltShiftQuality quality = TiltShift.TiltShiftQuality.Normal;

		// Token: 0x040001BF RID: 447
		[Range(0f, 15f)]
		public float blurArea = 1f;

		// Token: 0x040001C0 RID: 448
		[Range(0f, 25f)]
		public float maxBlurSize = 5f;

		// Token: 0x040001C1 RID: 449
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x040001C2 RID: 450
		public Shader tiltShiftShader;

		// Token: 0x040001C3 RID: 451
		private Material tiltShiftMaterial;

		// Token: 0x02000223 RID: 547
		public enum TiltShiftMode
		{
			// Token: 0x04000C9A RID: 3226
			TiltShiftMode,
			// Token: 0x04000C9B RID: 3227
			IrisMode
		}

		// Token: 0x02000224 RID: 548
		public enum TiltShiftQuality
		{
			// Token: 0x04000C9D RID: 3229
			Preview,
			// Token: 0x04000C9E RID: 3230
			Normal,
			// Token: 0x04000C9F RID: 3231
			High
		}
	}
}
