using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCompositorOrtho : TextureCompositor
{
	public Material rMatSkin;

	public Material rMatArmor;

	public Material rMatAlpha;

	public Material rMatDiffuse;

	public Material rMatCMPartial;

	public Camera cam;

	public GameObject rQuad;

	private static ObjectPool<GameObject> quadPool;

	public void Start()
	{
		quadPool = new ObjectPool<GameObject>(rQuad);
	}

	public override IEnumerator FinalizeJob(CompositorRequest cr)
	{
		Texture tex = cr.tex;
		if (tex != null)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < cr.tasks.Count; i++)
			{
				CompositorTask compositorTask = cr.tasks[i];
				GameObject gameObject = quadPool.Get();
				gameObject.SetActive(value: true);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = new Vector3(compositorTask.Rect.x, compositorTask.Rect.y, (float)(-i) * 0.1f);
				gameObject.transform.localScale = new Vector3(compositorTask.Rect.width, compositorTask.Rect.height, 0f);
				gameObject.layer = base.gameObject.layer;
				list.Add(gameObject);
				if (compositorTask is CompositorTaskSkin)
				{
					CompositorTaskSkin compositorTaskSkin = (CompositorTaskSkin)compositorTask;
					Material material = Object.Instantiate(rMatSkin);
					material.SetTexture("_MainTex", compositorTaskSkin.TextureD);
					material.SetColor("_AColor", compositorTaskSkin.ColorA);
					material.SetColor("_BColor", compositorTaskSkin.ColorB);
					gameObject.GetComponent<Renderer>().sharedMaterial = material;
				}
				else if (compositorTask is CompositorTaskColorMask)
				{
					CompositorTaskColorMask compositorTaskColorMask = (CompositorTaskColorMask)compositorTask;
					Material material2 = Object.Instantiate(rMatArmor);
					material2.SetTexture("_MainTex", compositorTaskColorMask.TextureD);
					material2.SetTexture("_CMTex", compositorTaskColorMask.TextureCM);
					material2.SetColor("_RColor", compositorTaskColorMask.ColorR);
					material2.SetColor("_GColor", compositorTaskColorMask.ColorG);
					material2.SetColor("_BColor", compositorTaskColorMask.ColorB);
					gameObject.GetComponent<Renderer>().sharedMaterial = material2;
				}
				else if (compositorTask is CompositorTaskPartialColorMask)
				{
					CompositorTaskPartialColorMask compositorTaskPartialColorMask = (CompositorTaskPartialColorMask)compositorTask;
					Material material3 = Object.Instantiate(rMatCMPartial);
					material3.SetTexture("_MainTex", compositorTaskPartialColorMask.TextureD);
					material3.SetTexture("_CMTex", compositorTaskPartialColorMask.TextureCM);
					material3.SetTexture("_CTex", compositorTaskPartialColorMask.TextureC);
					material3.SetColor("_RColor", compositorTaskPartialColorMask.ColorR);
					material3.SetColor("_GColor", compositorTaskPartialColorMask.ColorG);
					material3.SetColor("_BColor", compositorTaskPartialColorMask.ColorB);
					gameObject.GetComponent<Renderer>().sharedMaterial = material3;
				}
				else if (compositorTask is CompositorTaskDiffuse)
				{
					CompositorTaskDiffuse compositorTaskDiffuse = (CompositorTaskDiffuse)compositorTask;
					Material material4 = Object.Instantiate(rMatDiffuse);
					material4.SetTexture("_CMTex", compositorTaskDiffuse.TextureCM);
					material4.SetTexture("_DTex", compositorTaskDiffuse.TextureD);
					gameObject.GetComponent<Renderer>().sharedMaterial = material4;
				}
				else if (compositorTask is CompositorTaskColorTint)
				{
					CompositorTaskColorTint compositorTaskColorTint = (CompositorTaskColorTint)compositorTask;
					Material material5 = Object.Instantiate(rMatAlpha);
					material5.SetTexture("_MainTex", compositorTaskColorTint.Textures[0]);
					material5.SetColor("_Color", compositorTaskColorTint.Colors[0]);
					gameObject.GetComponent<Renderer>().sharedMaterial = material5;
					for (int j = 1; j < compositorTaskColorTint.Textures.Count; j++)
					{
						material5 = Object.Instantiate(rMatAlpha);
						Rectangle rectangle = compositorTaskColorTint.Rectangles[j];
						GameObject gameObject2 = quadPool.Get();
						gameObject2.SetActive(value: true);
						gameObject2.transform.parent = base.transform;
						gameObject2.transform.localPosition = new Vector3(rectangle.x, rectangle.y, (float)(-i) - (float)j * 0.1f);
						gameObject2.transform.localScale = new Vector3(rectangle.width, rectangle.height, 0f);
						gameObject2.layer = base.gameObject.layer;
						list.Add(gameObject2);
						material5.SetTexture("_MainTex", compositorTaskColorTint.Textures[j]);
						material5.SetColor("_Color", compositorTaskColorTint.Colors[j]);
						gameObject2.GetComponent<Renderer>().sharedMaterial = material5;
					}
				}
				else
				{
					Material material6 = Object.Instantiate(rMatAlpha);
					material6.SetTexture("_MainTex", compositorTask.TextureD);
					gameObject.GetComponent<Renderer>().sharedMaterial = material6;
				}
			}
			base.transform.localScale = Vector3.one * ((float)tex.height / 1024f);
			cam.orthographicSize = tex.height / 2;
			if (tex is RenderTexture)
			{
				cam.targetTexture = (RenderTexture)tex;
				cam.Render();
			}
			else if (tex is Texture2D)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32);
				cam.targetTexture = temporary;
				cam.Render();
				RenderTexture.active = temporary;
				((Texture2D)tex).ReadPixels(new Rect(0f, 0f, tex.width, tex.height), 0, 0, recalculateMipMaps: false);
				((Texture2D)tex).Apply();
				RenderTexture.ReleaseTemporary(temporary);
			}
			cam.targetTexture = null;
			foreach (GameObject item in list)
			{
				Object.Destroy(item.GetComponent<Renderer>().sharedMaterial);
				quadPool.Release(item);
			}
		}
		cr.OnComplete();
		cr.tasks.Clear();
		yield return null;
	}
}
