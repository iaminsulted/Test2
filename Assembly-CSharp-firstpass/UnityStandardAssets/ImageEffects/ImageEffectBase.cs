using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200001D RID: 29
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00012CE3 File Offset: 0x00010EE3
		protected virtual void Start()
		{
			if (!this.shader || !this.shader.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00012D06 File Offset: 0x00010F06
		protected Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new Material(this.shader);
					this.m_Material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_Material;
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00012D3A File Offset: 0x00010F3A
		protected virtual void OnDisable()
		{
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
		}

		// Token: 0x04000167 RID: 359
		public Shader shader;

		// Token: 0x04000168 RID: 360
		private Material m_Material;
	}
}
