using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000021 RID: 33
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
	public class NoiseAndScratches : MonoBehaviour
	{
		// Token: 0x0600018E RID: 398 RVA: 0x000137DC File Offset: 0x000119DC
		protected void Start()
		{
			if (this.shaderRGB == null || this.shaderYUV == null)
			{
				Debug.Log("Noise shaders are not set up! Disabling noise effect.");
				base.enabled = false;
				return;
			}
			if (!this.shaderRGB.isSupported)
			{
				base.enabled = false;
				return;
			}
			if (!this.shaderYUV.isSupported)
			{
				this.rgbFallback = true;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00013840 File Offset: 0x00011A40
		protected Material material
		{
			get
			{
				if (this.m_MaterialRGB == null)
				{
					this.m_MaterialRGB = new Material(this.shaderRGB);
					this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
				}
				if (this.m_MaterialYUV == null && !this.rgbFallback)
				{
					this.m_MaterialYUV = new Material(this.shaderYUV);
					this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
				}
				if (this.rgbFallback || this.monochrome)
				{
					return this.m_MaterialRGB;
				}
				return this.m_MaterialYUV;
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000138CA File Offset: 0x00011ACA
		protected void OnDisable()
		{
			if (this.m_MaterialRGB)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialRGB);
			}
			if (this.m_MaterialYUV)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialYUV);
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000138FC File Offset: 0x00011AFC
		private void SanitizeParameters()
		{
			this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
			this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
			this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
			this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
			this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
			this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
			this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000139C8 File Offset: 0x00011BC8
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.SanitizeParameters();
			if (this.scratchTimeLeft <= 0f)
			{
				this.scratchTimeLeft = UnityEngine.Random.value * 2f / this.scratchFPS;
				this.scratchX = UnityEngine.Random.value;
				this.scratchY = UnityEngine.Random.value;
			}
			this.scratchTimeLeft -= Time.deltaTime;
			Material material = this.material;
			material.SetTexture("_GrainTex", this.grainTexture);
			material.SetTexture("_ScratchTex", this.scratchTexture);
			float num = 1f / this.grainSize;
			material.SetVector("_GrainOffsetScale", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, (float)Screen.width / (float)this.grainTexture.width * num, (float)Screen.height / (float)this.grainTexture.height * num));
			material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + UnityEngine.Random.value * this.scratchJitter, this.scratchY + UnityEngine.Random.value * this.scratchJitter, (float)Screen.width / (float)this.scratchTexture.width, (float)Screen.height / (float)this.scratchTexture.height));
			material.SetVector("_Intensity", new Vector4(UnityEngine.Random.Range(this.grainIntensityMin, this.grainIntensityMax), UnityEngine.Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x0400017F RID: 383
		public bool monochrome = true;

		// Token: 0x04000180 RID: 384
		private bool rgbFallback;

		// Token: 0x04000181 RID: 385
		[Range(0f, 5f)]
		public float grainIntensityMin = 0.1f;

		// Token: 0x04000182 RID: 386
		[Range(0f, 5f)]
		public float grainIntensityMax = 0.2f;

		// Token: 0x04000183 RID: 387
		[Range(0.1f, 50f)]
		public float grainSize = 2f;

		// Token: 0x04000184 RID: 388
		[Range(0f, 5f)]
		public float scratchIntensityMin = 0.05f;

		// Token: 0x04000185 RID: 389
		[Range(0f, 5f)]
		public float scratchIntensityMax = 0.25f;

		// Token: 0x04000186 RID: 390
		[Range(1f, 30f)]
		public float scratchFPS = 10f;

		// Token: 0x04000187 RID: 391
		[Range(0f, 1f)]
		public float scratchJitter = 0.01f;

		// Token: 0x04000188 RID: 392
		public Texture grainTexture;

		// Token: 0x04000189 RID: 393
		public Texture scratchTexture;

		// Token: 0x0400018A RID: 394
		public Shader shaderRGB;

		// Token: 0x0400018B RID: 395
		public Shader shaderYUV;

		// Token: 0x0400018C RID: 396
		private Material m_MaterialRGB;

		// Token: 0x0400018D RID: 397
		private Material m_MaterialYUV;

		// Token: 0x0400018E RID: 398
		private float scratchTimeLeft;

		// Token: 0x0400018F RID: 399
		private float scratchX;

		// Token: 0x04000190 RID: 400
		private float scratchY;
	}
}
