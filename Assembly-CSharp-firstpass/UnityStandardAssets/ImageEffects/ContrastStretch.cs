using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000015 RID: 21
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Contrast Stretch")]
	public class ContrastStretch : MonoBehaviour
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0001004D File Offset: 0x0000E24D
		protected Material materialLum
		{
			get
			{
				if (this.m_materialLum == null)
				{
					this.m_materialLum = new Material(this.shaderLum);
					this.m_materialLum.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialLum;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00010081 File Offset: 0x0000E281
		protected Material materialReduce
		{
			get
			{
				if (this.m_materialReduce == null)
				{
					this.m_materialReduce = new Material(this.shaderReduce);
					this.m_materialReduce.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialReduce;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000100B5 File Offset: 0x0000E2B5
		protected Material materialAdapt
		{
			get
			{
				if (this.m_materialAdapt == null)
				{
					this.m_materialAdapt = new Material(this.shaderAdapt);
					this.m_materialAdapt.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialAdapt;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000100E9 File Offset: 0x0000E2E9
		protected Material materialApply
		{
			get
			{
				if (this.m_materialApply == null)
				{
					this.m_materialApply = new Material(this.shaderApply);
					this.m_materialApply.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_materialApply;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0001011D File Offset: 0x0000E31D
		private void Start()
		{
			if (!this.shaderAdapt.isSupported || !this.shaderApply.isSupported || !this.shaderLum.isSupported || !this.shaderReduce.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0001015C File Offset: 0x0000E35C
		private void OnEnable()
		{
			for (int i = 0; i < 2; i++)
			{
				if (!this.adaptRenderTex[i])
				{
					this.adaptRenderTex[i] = new RenderTexture(1, 1, 0);
					this.adaptRenderTex[i].hideFlags = HideFlags.HideAndDontSave;
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000101A4 File Offset: 0x0000E3A4
		private void OnDisable()
		{
			for (int i = 0; i < 2; i++)
			{
				UnityEngine.Object.DestroyImmediate(this.adaptRenderTex[i]);
				this.adaptRenderTex[i] = null;
			}
			if (this.m_materialLum)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialLum);
			}
			if (this.m_materialReduce)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialReduce);
			}
			if (this.m_materialAdapt)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialAdapt);
			}
			if (this.m_materialApply)
			{
				UnityEngine.Object.DestroyImmediate(this.m_materialApply);
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00010234 File Offset: 0x0000E434
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / 1, source.height / 1);
			Graphics.Blit(source, renderTexture, this.materialLum);
			while (renderTexture.width > 1 || renderTexture.height > 1)
			{
				int num = renderTexture.width / 2;
				if (num < 1)
				{
					num = 1;
				}
				int num2 = renderTexture.height / 2;
				if (num2 < 1)
				{
					num2 = 1;
				}
				RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
				Graphics.Blit(renderTexture, temporary, this.materialReduce);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.CalculateAdaptation(renderTexture);
			this.materialApply.SetTexture("_AdaptTex", this.adaptRenderTex[this.curAdaptIndex]);
			Graphics.Blit(source, destination, this.materialApply);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000102EC File Offset: 0x0000E4EC
		private void CalculateAdaptation(Texture curTexture)
		{
			int num = this.curAdaptIndex;
			this.curAdaptIndex = (this.curAdaptIndex + 1) % 2;
			float num2 = 1f - Mathf.Pow(1f - this.adaptationSpeed, 30f * Time.deltaTime);
			num2 = Mathf.Clamp(num2, 0.01f, 1f);
			this.materialAdapt.SetTexture("_CurTex", curTexture);
			this.materialAdapt.SetVector("_AdaptParams", new Vector4(num2, this.limitMinimum, this.limitMaximum, 0f));
			Graphics.SetRenderTarget(this.adaptRenderTex[this.curAdaptIndex]);
			GL.Clear(false, true, Color.black);
			Graphics.Blit(this.adaptRenderTex[num], this.adaptRenderTex[this.curAdaptIndex], this.materialAdapt);
		}

		// Token: 0x040000F3 RID: 243
		[Range(0.0001f, 1f)]
		public float adaptationSpeed = 0.02f;

		// Token: 0x040000F4 RID: 244
		[Range(0f, 1f)]
		public float limitMinimum = 0.2f;

		// Token: 0x040000F5 RID: 245
		[Range(0f, 1f)]
		public float limitMaximum = 0.6f;

		// Token: 0x040000F6 RID: 246
		private RenderTexture[] adaptRenderTex = new RenderTexture[2];

		// Token: 0x040000F7 RID: 247
		private int curAdaptIndex;

		// Token: 0x040000F8 RID: 248
		public Shader shaderLum;

		// Token: 0x040000F9 RID: 249
		private Material m_materialLum;

		// Token: 0x040000FA RID: 250
		public Shader shaderReduce;

		// Token: 0x040000FB RID: 251
		private Material m_materialReduce;

		// Token: 0x040000FC RID: 252
		public Shader shaderAdapt;

		// Token: 0x040000FD RID: 253
		private Material m_materialAdapt;

		// Token: 0x040000FE RID: 254
		public Shader shaderApply;

		// Token: 0x040000FF RID: 255
		private Material m_materialApply;
	}
}
