using System.Collections.Generic;
using UnityEngine;

public class OCCam : MonoBehaviour
{
	public GameObject Full;

	public GameObject Armor;

	public Dictionary<TextureCompositor.Region, GameObject> RegionMap;

	private void Awake()
	{
		RegionMap = new Dictionary<TextureCompositor.Region, GameObject>();
		RegionMap[TextureCompositor.Region.Full] = Full;
		RegionMap[TextureCompositor.Region.Armor] = Armor;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
