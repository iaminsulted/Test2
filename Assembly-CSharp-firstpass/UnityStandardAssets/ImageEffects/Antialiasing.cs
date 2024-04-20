using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000006 RID: 6
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Antialiasing")]
	public class Antialiasing : PostEffectsBase
	{
		// Token: 0x06000102 RID: 258 RVA: 0x0000C8DC File Offset: 0x0000AADC
		public Material CurrentAAMaterial()
		{
			Material result;
			switch (this.mode)
			{
			case AAMode.FXAA2:
				result = this.materialFXAAII;
				break;
			case AAMode.FXAA3Console:
				result = this.materialFXAAIII;
				break;
			case AAMode.FXAA1PresetA:
				result = this.materialFXAAPreset2;
				break;
			case AAMode.FXAA1PresetB:
				result = this.materialFXAAPreset3;
				break;
			case AAMode.NFAA:
				result = this.nfaa;
				break;
			case AAMode.SSAA:
				result = this.ssaa;
				break;
			case AAMode.DLAA:
				result = this.dlaa;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000C958 File Offset: 0x0000AB58
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.materialFXAAPreset2 = base.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
			this.materialFXAAPreset3 = base.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
			this.materialFXAAII = base.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
			this.materialFXAAIII = base.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
			this.nfaa = base.CreateMaterial(this.nfaaShader, this.nfaa);
			this.ssaa = base.CreateMaterial(this.ssaaShader, this.ssaa);
			this.dlaa = base.CreateMaterial(this.dlaaShader, this.dlaa);
			if (!this.ssaaShader.isSupported)
			{
				base.NotSupported();
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000CA34 File Offset: 0x0000AC34
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.mode == AAMode.FXAA3Console && this.materialFXAAIII != null)
			{
				this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
				this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
				this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
				Graphics.Blit(source, destination, this.materialFXAAIII);
				return;
			}
			if (this.mode == AAMode.FXAA1PresetB && this.materialFXAAPreset3 != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAPreset3);
				return;
			}
			if (this.mode == AAMode.FXAA1PresetA && this.materialFXAAPreset2 != null)
			{
				source.anisoLevel = 4;
				Graphics.Blit(source, destination, this.materialFXAAPreset2);
				source.anisoLevel = 0;
				return;
			}
			if (this.mode == AAMode.FXAA2 && this.materialFXAAII != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAII);
				return;
			}
			if (this.mode == AAMode.SSAA && this.ssaa != null)
			{
				Graphics.Blit(source, destination, this.ssaa);
				return;
			}
			if (this.mode == AAMode.DLAA && this.dlaa != null)
			{
				source.anisoLevel = 0;
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(source, temporary, this.dlaa, 0);
				Graphics.Blit(temporary, destination, this.dlaa, this.dlaaSharp ? 2 : 1);
				RenderTexture.ReleaseTemporary(temporary);
				return;
			}
			if (this.mode == AAMode.NFAA && this.nfaa != null)
			{
				source.anisoLevel = 0;
				this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
				this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
				Graphics.Blit(source, destination, this.nfaa, this.showGeneratedNormals ? 1 : 0);
				return;
			}
			Graphics.Blit(source, destination);
		}

		// Token: 0x0400003B RID: 59
		public AAMode mode = AAMode.FXAA3Console;

		// Token: 0x0400003C RID: 60
		public bool showGeneratedNormals;

		// Token: 0x0400003D RID: 61
		public float offsetScale = 0.2f;

		// Token: 0x0400003E RID: 62
		public float blurRadius = 18f;

		// Token: 0x0400003F RID: 63
		public float edgeThresholdMin = 0.05f;

		// Token: 0x04000040 RID: 64
		public float edgeThreshold = 0.2f;

		// Token: 0x04000041 RID: 65
		public float edgeSharpness = 4f;

		// Token: 0x04000042 RID: 66
		public bool dlaaSharp;

		// Token: 0x04000043 RID: 67
		public Shader ssaaShader;

		// Token: 0x04000044 RID: 68
		private Material ssaa;

		// Token: 0x04000045 RID: 69
		public Shader dlaaShader;

		// Token: 0x04000046 RID: 70
		private Material dlaa;

		// Token: 0x04000047 RID: 71
		public Shader nfaaShader;

		// Token: 0x04000048 RID: 72
		private Material nfaa;

		// Token: 0x04000049 RID: 73
		public Shader shaderFXAAPreset2;

		// Token: 0x0400004A RID: 74
		private Material materialFXAAPreset2;

		// Token: 0x0400004B RID: 75
		public Shader shaderFXAAPreset3;

		// Token: 0x0400004C RID: 76
		private Material materialFXAAPreset3;

		// Token: 0x0400004D RID: 77
		public Shader shaderFXAAII;

		// Token: 0x0400004E RID: 78
		private Material materialFXAAII;

		// Token: 0x0400004F RID: 79
		public Shader shaderFXAAIII;

		// Token: 0x04000050 RID: 80
		private Material materialFXAAIII;
	}
}
