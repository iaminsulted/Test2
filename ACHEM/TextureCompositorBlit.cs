using System.Collections;
using UnityEngine;

public class TextureCompositorBlit : TextureCompositor
{
	public Material rMatSkin;

	public Material rMatArmor;

	public Material rMatAlpha;

	public Material rMatDiffuse;

	public Material rMatCMPartial;

	public void Start()
	{
	}

	public override IEnumerator FinalizeJob(CompositorRequest cr)
	{
		Debug.Log("FinalizeJob");
		yield return new WaitForEndOfFrame();
		RenderTexture renderTexture;
		RenderTexture renderTexture2 = (renderTexture = null);
		for (int i = 0; i < cr.tasks.Count; i++)
		{
			if (cr.tex == null)
			{
				continue;
			}
			renderTexture = RenderTexture.GetTemporary(cr.tex.width, cr.tex.height, 0, RenderTextureFormat.ARGB32);
			CompositorTask compositorTask = cr.tasks[i];
			if (compositorTask is CompositorTaskSkin)
			{
				CompositorTaskSkin compositorTaskSkin = (CompositorTaskSkin)compositorTask;
				Material material = Object.Instantiate(rMatSkin);
				material.SetColor("_AColor", compositorTaskSkin.ColorA);
				material.SetColor("_BColor", compositorTaskSkin.ColorB);
				Graphics.Blit(compositorTaskSkin.TextureD, renderTexture, material);
			}
			else if (compositorTask is CompositorTaskColorMask)
			{
				CompositorTaskColorMask compositorTaskColorMask = (CompositorTaskColorMask)compositorTask;
				Material material2 = Object.Instantiate(rMatArmor);
				material2.SetTexture("_ArmorTex", compositorTaskColorMask.TextureD);
				material2.SetTexture("_CMTex", compositorTaskColorMask.TextureCM);
				material2.SetColor("_RColor", compositorTaskColorMask.ColorR);
				material2.SetColor("_GColor", compositorTaskColorMask.ColorG);
				material2.SetColor("_BColor", compositorTaskColorMask.ColorB);
				material2.SetVector("_Rect", new Vector4((float)compositorTask.Rect.x / 1024f, (float)compositorTask.Rect.y / 1024f, 1024f / (float)compositorTask.Rect.width, 1024f / (float)compositorTask.Rect.height));
				Graphics.Blit(renderTexture2, renderTexture, material2);
			}
			else if (compositorTask is CompositorTaskPartialColorMask)
			{
				CompositorTaskPartialColorMask compositorTaskPartialColorMask = (CompositorTaskPartialColorMask)compositorTask;
				Material material3 = Object.Instantiate(rMatCMPartial);
				material3.SetTexture("_DTex", compositorTaskPartialColorMask.TextureD);
				material3.SetTexture("_CTex", compositorTaskPartialColorMask.TextureC);
				material3.SetTexture("_CMTex", compositorTaskPartialColorMask.TextureCM);
				material3.SetColor("_RColor", compositorTaskPartialColorMask.ColorR);
				material3.SetColor("_GColor", compositorTaskPartialColorMask.ColorG);
				material3.SetColor("_BColor", compositorTaskPartialColorMask.ColorB);
				material3.SetVector("_Rect", new Vector4((float)compositorTask.Rect.x / 1024f, (float)compositorTask.Rect.y / 1024f, 1024f / (float)compositorTask.Rect.width, 1024f / (float)compositorTask.Rect.height));
				Graphics.Blit(renderTexture2, renderTexture, material3);
			}
			else if (compositorTask is CompositorTaskDiffuse)
			{
				CompositorTaskDiffuse compositorTaskDiffuse = (CompositorTaskDiffuse)compositorTask;
				Material material4 = Object.Instantiate(rMatDiffuse);
				material4.SetTexture("_CMTex", compositorTaskDiffuse.TextureCM);
				material4.SetTexture("_DTex", compositorTaskDiffuse.TextureD);
				material4.SetVector("_Rect", new Vector4((float)compositorTask.Rect.x / 1024f, (float)compositorTask.Rect.y / 1024f, 1024f / (float)compositorTask.Rect.width, 1024f / (float)compositorTask.Rect.height));
				Graphics.Blit(renderTexture2, renderTexture, material4);
			}
			else if (compositorTask is CompositorTaskColorTint)
			{
				CompositorTaskColorTint compositorTaskColorTint = (CompositorTaskColorTint)compositorTask;
				Material material5 = Object.Instantiate(rMatAlpha);
				for (int j = 0; j < compositorTaskColorTint.Textures.Count; j++)
				{
					material5.SetColor("_Color" + j, compositorTaskColorTint.Colors[j]);
					material5.SetTexture("_BlendTex" + j, compositorTaskColorTint.Textures[j]);
					Rectangle rectangle = compositorTaskColorTint.Rectangles[j];
					material5.SetVector("_Rect" + j, new Vector4((float)rectangle.x / 1024f, (float)rectangle.y / 1024f, 1024f / (float)rectangle.width, 1024f / (float)rectangle.height));
				}
				Graphics.Blit(renderTexture2, renderTexture, material5);
			}
			else
			{
				Material material6 = Object.Instantiate(rMatAlpha);
				material6.SetTexture("_BlendTex0", compositorTask.TextureD);
				material6.SetVector("_Rect0", new Vector4((float)compositorTask.Rect.x / 1024f, (float)compositorTask.Rect.y / 1024f, 1024f / (float)compositorTask.Rect.width, 1024f / (float)compositorTask.Rect.height));
				Graphics.Blit(renderTexture2, renderTexture, material6);
			}
			if (renderTexture2 != null)
			{
				RenderTexture.ReleaseTemporary(renderTexture2);
			}
			renderTexture2 = renderTexture;
		}
		if (cr.tex != null)
		{
			if (cr.tex is RenderTexture)
			{
				RenderTexture dest = (RenderTexture)cr.tex;
				Graphics.Blit(renderTexture, dest);
			}
			else if (cr.tex is Texture2D)
			{
				Texture2D texture2D = (Texture2D)cr.tex;
				RenderTexture.active = renderTexture;
				texture2D.ReadPixels(new Rect(0f, 0f, texture2D.width, texture2D.height), 0, 0, recalculateMipMaps: false);
				texture2D.Apply();
			}
			RenderTexture.ReleaseTemporary(renderTexture);
			cr.OnComplete();
			cr.tasks.Clear();
		}
	}
}
