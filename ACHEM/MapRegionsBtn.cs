using System.Collections;
using UnityEngine;

public class MapRegionsBtn : MonoBehaviour
{
	public MapController myMap;

	public GameObject myWordMapBg;

	public GameObject myRegionPrefab;

	public GameObject myRegion;

	public bool hasMaterial = true;

	public float duration = 1f;

	private Renderer rend;

	private void Start()
	{
		if (hasMaterial)
		{
			rend = GetComponent<Renderer>();
			rend.sharedMaterial.renderQueue = 3300;
		}
		if (myMap == null)
		{
			myMap = GetComponentInParent<MapController>();
		}
	}

	private void OnHover(bool isOver)
	{
		if (hasMaterial)
		{
			if (isOver)
			{
				StartCoroutine(Fade());
				return;
			}
			StopAllCoroutines();
			Color color = rend.material.color;
			color.a = 0f;
			rend.material.color = color;
		}
	}

	private IEnumerator Fade()
	{
		while ((double)rend.material.color.a < 0.7)
		{
			Color color = rend.material.color;
			color.a += 0.2f;
			rend.material.color = color;
			yield return null;
		}
	}

	private IEnumerator FadeOut()
	{
		while (rend.material.color.a > 0f)
		{
			Color color = rend.material.color;
			color.a -= 0.02f;
			rend.material.color = color;
			yield return null;
		}
	}

	public void OnClick()
	{
		if (hasMaterial)
		{
			Color color = rend.material.color;
			color.a = 0f;
			rend.material.color = color;
		}
		if (base.name == "WorldMapBtn" || base.name == "WorldMapExit")
		{
			if (base.name == "WorldMapExit")
			{
				base.transform.parent.GetComponent<MapController>().Close();
			}
			myWordMapBg.SetActive(value: true);
			if ((bool)base.transform.parent.GetComponent<MapController>().currentRegion)
			{
				Object.Destroy(base.transform.parent.GetComponent<MapController>().currentRegion);
			}
			else
			{
				base.transform.parent.GetComponent<MapController>().Close();
			}
		}
		else
		{
			myMap.LoadRegion(myRegionPrefab);
		}
	}
}
