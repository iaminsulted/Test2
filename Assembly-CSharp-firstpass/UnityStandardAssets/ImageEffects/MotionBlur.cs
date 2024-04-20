using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200001F RID: 31
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Motion Blur (Color Accumulation)")]
	[RequireComponent(typeof(Camera))]
	public class MotionBlur : ImageEffectBase
	{
		// Token: 0x06000182 RID: 386 RVA: 0x00012E22 File Offset: 0x00011022
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00012E2A File Offset: 0x0001102A
		protected override void OnDisable()
		{
			base.OnDisable();
			UnityEngine.Object.DestroyImmediate(this.accumTexture);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00012E40 File Offset: 0x00011040
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.accumTexture == null || this.accumTexture.width != source.width || this.accumTexture.height != source.height)
			{
				UnityEngine.Object.DestroyImmediate(this.accumTexture);
				this.accumTexture = new RenderTexture(source.width, source.height, 0);
				this.accumTexture.hideFlags = HideFlags.HideAndDontSave;
				Graphics.Blit(source, this.accumTexture);
			}
			if (this.extraBlur)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
				this.accumTexture.MarkRestoreExpected();
				Graphics.Blit(this.accumTexture, temporary);
				Graphics.Blit(temporary, this.accumTexture);
				RenderTexture.ReleaseTemporary(temporary);
			}
			this.blurAmount = Mathf.Clamp(this.blurAmount, 0f, 0.92f);
			base.material.SetTexture("_MainTex", this.accumTexture);
			base.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
			this.accumTexture.MarkRestoreExpected();
			Graphics.Blit(source, this.accumTexture, base.material);
			Graphics.Blit(this.accumTexture, destination);
		}

		// Token: 0x04000169 RID: 361
		[Range(0f, 0.92f)]
		public float blurAmount = 0.8f;

		// Token: 0x0400016A RID: 362
		public bool extraBlur;

		// Token: 0x0400016B RID: 363
		private RenderTexture accumTexture;
	}
}
