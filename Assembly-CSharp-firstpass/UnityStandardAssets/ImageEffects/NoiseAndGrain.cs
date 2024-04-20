using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000020 RID: 32
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise And Grain (Filmic)")]
	public class NoiseAndGrain : PostEffectsBase
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00012F8E File Offset: 0x0001118E
		private void Awake()
		{
			this.mesh = new Mesh();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00012F9C File Offset: 0x0001119C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.noiseMaterial = base.CheckShaderAndCreateMaterial(this.noiseShader, this.noiseMaterial);
			if (this.dx11Grain && this.supportDX11)
			{
				this.dx11NoiseMaterial = base.CheckShaderAndCreateMaterial(this.dx11NoiseShader, this.dx11NoiseMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00013008 File Offset: 0x00011208
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || null == this.noiseTexture)
			{
				Graphics.Blit(source, destination);
				if (null == this.noiseTexture)
				{
					Debug.LogWarning("Noise & Grain effect failing as noise texture is not assigned. please assign.", base.transform);
				}
				return;
			}
			this.softness = Mathf.Clamp(this.softness, 0f, 0.99f);
			if (this.dx11Grain && this.supportDX11)
			{
				this.dx11NoiseMaterial.SetFloat("_DX11NoiseTime", (float)Time.frameCount);
				this.dx11NoiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.dx11NoiseMaterial.SetVector("_NoisePerChannel", this.monochrome ? Vector3.one : this.intensities);
				this.dx11NoiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
				this.dx11NoiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
				if (this.softness > Mathf.Epsilon)
				{
					RenderTexture temporary = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary, this.dx11NoiseMaterial, this.noiseTexture, this.mesh, this.monochrome ? 3 : 2);
					this.dx11NoiseMaterial.SetTexture("_NoiseTex", temporary);
					Graphics.Blit(source, destination, this.dx11NoiseMaterial, 4);
					RenderTexture.ReleaseTemporary(temporary);
					return;
				}
				NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.dx11NoiseMaterial, this.noiseTexture, this.mesh, this.monochrome ? 1 : 0);
				return;
			}
			else
			{
				if (this.noiseTexture)
				{
					this.noiseTexture.wrapMode = TextureWrapMode.Repeat;
					this.noiseTexture.filterMode = this.filterMode;
				}
				this.noiseMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.noiseMaterial.SetVector("_NoisePerChannel", this.monochrome ? Vector3.one : this.intensities);
				this.noiseMaterial.SetVector("_NoiseTilingPerChannel", this.monochrome ? (Vector3.one * this.monochromeTiling) : this.tiling);
				this.noiseMaterial.SetVector("_MidGrey", new Vector3(this.midGrey, 1f / (1f - this.midGrey), -1f / this.midGrey));
				this.noiseMaterial.SetVector("_NoiseAmount", new Vector3(this.generalIntensity, this.blackIntensity, this.whiteIntensity) * this.intensityMultiplier);
				if (this.softness > Mathf.Epsilon)
				{
					RenderTexture temporary2 = RenderTexture.GetTemporary((int)((float)source.width * (1f - this.softness)), (int)((float)source.height * (1f - this.softness)));
					NoiseAndGrain.DrawNoiseQuadGrid(source, temporary2, this.noiseMaterial, this.noiseTexture, this.mesh, 2);
					this.noiseMaterial.SetTexture("_NoiseTex", temporary2);
					Graphics.Blit(source, destination, this.noiseMaterial, 1);
					RenderTexture.ReleaseTemporary(temporary2);
					return;
				}
				NoiseAndGrain.DrawNoiseQuadGrid(source, destination, this.noiseMaterial, this.noiseTexture, this.mesh, 0);
				return;
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000133A8 File Offset: 0x000115A8
		private static void DrawNoiseQuadGrid(RenderTexture source, RenderTexture dest, Material fxMaterial, Texture2D noise, Mesh mesh, int passNr)
		{
			RenderTexture.active = dest;
			fxMaterial.SetTexture("_MainTex", source);
			GL.PushMatrix();
			GL.LoadOrtho();
			fxMaterial.SetPass(passNr);
			NoiseAndGrain.BuildMesh(mesh, source, noise);
			Transform transform = Camera.main.transform;
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
			transform.position = position;
			transform.rotation = rotation;
			GL.PopMatrix();
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00013430 File Offset: 0x00011630
		private static void BuildMesh(Mesh mesh, RenderTexture source, Texture2D noise)
		{
			float noiseSize = (float)noise.width * 1f;
			float num = 1f * (float)source.width / NoiseAndGrain.TILE_AMOUNT;
			float num2 = 1f * (float)source.width / (1f * (float)source.height);
			float num3 = 1f / num;
			float num4 = num3 * num2;
			int num5 = (int)Mathf.Ceil(num);
			int num6 = (int)Mathf.Ceil(1f / num4);
			if (mesh.vertices.Length != num5 * num6 * 4)
			{
				Vector3[] array = new Vector3[num5 * num6 * 4];
				Vector2[] array2 = new Vector2[num5 * num6 * 4];
				int[] array3 = new int[num5 * num6 * 6];
				int num7 = 0;
				int num8 = 0;
				for (float num9 = 0f; num9 < 1f; num9 += num3)
				{
					for (float num10 = 0f; num10 < 1f; num10 += num4)
					{
						array[num7] = new Vector3(num9, num10, 0.1f);
						array[num7 + 1] = new Vector3(num9 + num3, num10, 0.1f);
						array[num7 + 2] = new Vector3(num9 + num3, num10 + num4, 0.1f);
						array[num7 + 3] = new Vector3(num9, num10 + num4, 0.1f);
						array2[num7] = new Vector2(0f, 0f);
						array2[num7 + 1] = new Vector2(1f, 0f);
						array2[num7 + 2] = new Vector2(1f, 1f);
						array2[num7 + 3] = new Vector2(0f, 1f);
						array3[num8] = num7;
						array3[num8 + 1] = num7 + 1;
						array3[num8 + 2] = num7 + 2;
						array3[num8 + 3] = num7;
						array3[num8 + 4] = num7 + 2;
						array3[num8 + 5] = num7 + 3;
						num7 += 4;
						num8 += 6;
					}
				}
				mesh.vertices = array;
				mesh.uv2 = array2;
				mesh.triangles = array3;
			}
			NoiseAndGrain.BuildMeshUV0(mesh, num5, num6, noiseSize, noise.width);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00013668 File Offset: 0x00011868
		private static void BuildMeshUV0(Mesh mesh, int width, int height, float noiseSize, int noiseWidth)
		{
			float num = noiseSize / ((float)noiseWidth * 1f);
			float num2 = 1f / noiseSize;
			Vector2[] array = new Vector2[width * height * 4];
			int num3 = 0;
			for (int i = 0; i < width * height; i++)
			{
				float num4 = UnityEngine.Random.Range(0f, noiseSize);
				float num5 = UnityEngine.Random.Range(0f, noiseSize);
				num4 = Mathf.Floor(num4) * num2;
				num5 = Mathf.Floor(num5) * num2;
				array[num3] = new Vector2(num4, num5);
				array[num3 + 1] = new Vector2(num4 + num * num2, num5);
				array[num3 + 2] = new Vector2(num4 + num * num2, num5 + num * num2);
				array[num3 + 3] = new Vector2(num4, num5 + num * num2);
				num3 += 4;
			}
			mesh.uv = array;
		}

		// Token: 0x0400016C RID: 364
		public float intensityMultiplier = 0.25f;

		// Token: 0x0400016D RID: 365
		public float generalIntensity = 0.5f;

		// Token: 0x0400016E RID: 366
		public float blackIntensity = 1f;

		// Token: 0x0400016F RID: 367
		public float whiteIntensity = 1f;

		// Token: 0x04000170 RID: 368
		public float midGrey = 0.2f;

		// Token: 0x04000171 RID: 369
		public bool dx11Grain;

		// Token: 0x04000172 RID: 370
		public float softness;

		// Token: 0x04000173 RID: 371
		public bool monochrome;

		// Token: 0x04000174 RID: 372
		public Vector3 intensities = new Vector3(1f, 1f, 1f);

		// Token: 0x04000175 RID: 373
		public Vector3 tiling = new Vector3(64f, 64f, 64f);

		// Token: 0x04000176 RID: 374
		public float monochromeTiling = 64f;

		// Token: 0x04000177 RID: 375
		public FilterMode filterMode = FilterMode.Bilinear;

		// Token: 0x04000178 RID: 376
		public Texture2D noiseTexture;

		// Token: 0x04000179 RID: 377
		public Shader noiseShader;

		// Token: 0x0400017A RID: 378
		private Material noiseMaterial;

		// Token: 0x0400017B RID: 379
		public Shader dx11NoiseShader;

		// Token: 0x0400017C RID: 380
		private Material dx11NoiseMaterial;

		// Token: 0x0400017D RID: 381
		private static float TILE_AMOUNT = 64f;

		// Token: 0x0400017E RID: 382
		private Mesh mesh;
	}
}
