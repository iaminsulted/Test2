using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200000E RID: 14
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Blur")]
	public class Blur : MonoBehaviour
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000E401 File Offset: 0x0000C601
		protected Material material
		{
			get
			{
				if (Blur.m_Material == null)
				{
					Blur.m_Material = new Material(this.blurShader);
					Blur.m_Material.hideFlags = HideFlags.DontSave;
				}
				return Blur.m_Material;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000E431 File Offset: 0x0000C631
		protected void OnDisable()
		{
			if (Blur.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(Blur.m_Material);
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000E449 File Offset: 0x0000C649
		protected void Start()
		{
			if (!this.blurShader || !this.material.shader.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000E474 File Offset: 0x0000C674
		public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
		{
			float num = 0.5f + (float)iteration * this.blurSpread;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000E4E0 File Offset: 0x0000C6E0
		private void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			float num = 1f;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000E544 File Offset: 0x0000C744
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int width = source.width / 4;
			int height = source.height / 4;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
			this.DownSample4x(source, renderTexture);
			for (int i = 0; i < this.iterations; i++)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
				this.FourTapCone(renderTexture, temporary, i);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x040000A7 RID: 167
		[Range(0f, 10f)]
		public int iterations = 3;

		// Token: 0x040000A8 RID: 168
		[Range(0f, 1f)]
		public float blurSpread = 0.6f;

		// Token: 0x040000A9 RID: 169
		public Shader blurShader;

		// Token: 0x040000AA RID: 170
		private static Material m_Material;
	}
}
