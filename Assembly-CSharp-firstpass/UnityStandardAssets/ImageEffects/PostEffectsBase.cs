using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000022 RID: 34
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsBase : MonoBehaviour
	{
		// Token: 0x06000194 RID: 404 RVA: 0x00013BAC File Offset: 0x00011DAC
		protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				base.enabled = false;
				return null;
			}
			if (s.isSupported && m2Create && m2Create.shader == s)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				this.NotSupported();
				Debug.Log(string.Concat(new string[]
				{
					"The shader ",
					s.ToString(),
					" on effect ",
					this.ToString(),
					" is not supported on this platform!"
				}));
				return null;
			}
			m2Create = new Material(s);
			this.createdMaterials.Add(m2Create);
			m2Create.hideFlags = HideFlags.DontSave;
			return m2Create;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00013C68 File Offset: 0x00011E68
		protected Material CreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				Debug.Log("Missing shader in " + this.ToString());
				return null;
			}
			if (m2Create && m2Create.shader == s && s.isSupported)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				return null;
			}
			m2Create = new Material(s);
			this.createdMaterials.Add(m2Create);
			m2Create.hideFlags = HideFlags.DontSave;
			return m2Create;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00013CDB File Offset: 0x00011EDB
		private void OnEnable()
		{
			this.isSupported = true;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00013CE4 File Offset: 0x00011EE4
		private void OnDestroy()
		{
			this.RemoveCreatedMaterials();
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00013CEC File Offset: 0x00011EEC
		private void RemoveCreatedMaterials()
		{
			while (this.createdMaterials.Count > 0)
			{
				UnityEngine.Object obj = this.createdMaterials[0];
				this.createdMaterials.RemoveAt(0);
				UnityEngine.Object.Destroy(obj);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00013D1B File Offset: 0x00011F1B
		protected bool CheckSupport()
		{
			return this.CheckSupport(false);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00013D24 File Offset: 0x00011F24
		public virtual bool CheckResources()
		{
			Debug.LogWarning("CheckResources () for " + this.ToString() + " should be overwritten.");
			return this.isSupported;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00013D46 File Offset: 0x00011F46
		protected void Start()
		{
			this.CheckResources();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00013D50 File Offset: 0x00011F50
		protected bool CheckSupport(bool needDepth)
		{
			this.isSupported = true;
			this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
			this.supportDX11 = (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders);
			if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			{
				this.NotSupported();
				return false;
			}
			if (needDepth)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			return true;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00013DB1 File Offset: 0x00011FB1
		protected bool CheckSupport(bool needDepth, bool needHdr)
		{
			if (!this.CheckSupport(needDepth))
			{
				return false;
			}
			if (needHdr && !this.supportHDRTextures)
			{
				this.NotSupported();
				return false;
			}
			return true;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00013DD2 File Offset: 0x00011FD2
		public bool Dx11Support()
		{
			return this.supportDX11;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00013DDA File Offset: 0x00011FDA
		protected void ReportAutoDisable()
		{
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00013DF8 File Offset: 0x00011FF8
		private bool CheckShader(Shader s)
		{
			Debug.Log(string.Concat(new string[]
			{
				"The shader ",
				s.ToString(),
				" on effect ",
				this.ToString(),
				" is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package."
			}));
			if (!s.isSupported)
			{
				this.NotSupported();
				return false;
			}
			return false;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00013E50 File Offset: 0x00012050
		protected void NotSupported()
		{
			base.enabled = false;
			this.isSupported = false;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00013E60 File Offset: 0x00012060
		protected void DrawBorder(RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				float y;
				float y2;
				if (flag)
				{
					y = 1f;
					y2 = 0f;
				}
				else
				{
					y = 0f;
					y2 = 1f;
				}
				float x = 0f;
				float x2 = 0f + 1f / ((float)dest.width * 1f);
				float y3 = 0f;
				float y4 = 1f;
				GL.Begin(7);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x, y4, 0.1f);
				float x3 = 1f - 1f / ((float)dest.width * 1f);
				x2 = 1f;
				y3 = 0f;
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x3, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x3, y4, 0.1f);
				float x4 = 0f;
				x2 = 1f;
				y3 = 0f;
				y4 = 0f + 1f / ((float)dest.height * 1f);
				GL.TexCoord2(0f, y);
				GL.Vertex3(x4, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x4, y4, 0.1f);
				float x5 = 0f;
				x2 = 1f;
				y3 = 1f - 1f / ((float)dest.height * 1f);
				y4 = 1f;
				GL.TexCoord2(0f, y);
				GL.Vertex3(x5, y3, 0.1f);
				GL.TexCoord2(1f, y);
				GL.Vertex3(x2, y3, 0.1f);
				GL.TexCoord2(1f, y2);
				GL.Vertex3(x2, y4, 0.1f);
				GL.TexCoord2(0f, y2);
				GL.Vertex3(x5, y4, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x04000191 RID: 401
		protected bool supportHDRTextures = true;

		// Token: 0x04000192 RID: 402
		protected bool supportDX11;

		// Token: 0x04000193 RID: 403
		protected bool isSupported = true;

		// Token: 0x04000194 RID: 404
		private List<Material> createdMaterials = new List<Material>();
	}
}
