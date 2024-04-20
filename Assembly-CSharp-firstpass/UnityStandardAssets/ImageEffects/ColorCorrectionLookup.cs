using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000012 RID: 18
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)")]
	public class ColorCorrectionLookup : PostEffectsBase
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000FADD File Offset: 0x0000DCDD
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
			if (!this.isSupported || !SystemInfo.supports3DTextures)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000FB1A File Offset: 0x0000DD1A
		private void OnDisable()
		{
			if (this.material)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000FB3B File Offset: 0x0000DD3B
		private void OnDestroy()
		{
			if (this.converted3DLut)
			{
				UnityEngine.Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = null;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000FB5C File Offset: 0x0000DD5C
		public void SetIdentityLut()
		{
			int num = 16;
			Color[] array = new Color[num * num * num];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
					}
				}
			}
			if (this.converted3DLut)
			{
				UnityEngine.Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
			this.converted3DLut.SetPixels(array);
			this.converted3DLut.Apply();
			this.basedOnTempTex = "";
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000FC3F File Offset: 0x0000DE3F
		public bool ValidDimensions(Texture2D tex2d)
		{
			return tex2d && tex2d.height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000FC68 File Offset: 0x0000DE68
		public void Convert(Texture2D temp2DTex, string path)
		{
			if (!temp2DTex)
			{
				Debug.LogError("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.");
				return;
			}
			int num = temp2DTex.width * temp2DTex.height;
			num = temp2DTex.height;
			if (!this.ValidDimensions(temp2DTex))
			{
				Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
				this.basedOnTempTex = "";
				return;
			}
			Color[] pixels = temp2DTex.GetPixels();
			Color[] array = new Color[pixels.Length];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						int num2 = num - j - 1;
						array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
					}
				}
			}
			if (this.converted3DLut)
			{
				UnityEngine.Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
			this.converted3DLut.SetPixels(array);
			this.converted3DLut.Apply();
			this.basedOnTempTex = path;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000FD80 File Offset: 0x0000DF80
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || !SystemInfo.supports3DTextures)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.converted3DLut == null)
			{
				this.SetIdentityLut();
			}
			int width = this.converted3DLut.width;
			this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
			this.material.SetFloat("_Scale", (float)(width - 1) / (1f * (float)width));
			this.material.SetFloat("_Offset", 1f / (2f * (float)width));
			this.material.SetTexture("_ClutTex", this.converted3DLut);
			Graphics.Blit(source, destination, this.material, (QualitySettings.activeColorSpace == ColorSpace.Linear) ? 1 : 0);
		}

		// Token: 0x040000E7 RID: 231
		public Shader shader;

		// Token: 0x040000E8 RID: 232
		private Material material;

		// Token: 0x040000E9 RID: 233
		public Texture3D converted3DLut;

		// Token: 0x040000EA RID: 234
		public string basedOnTempTex = "";
	}
}
