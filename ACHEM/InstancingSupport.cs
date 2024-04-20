using System.Collections.Generic;
using UnityEngine;

public class InstancingSupport : MonoBehaviour
{
	[Tooltip("The list of game objects that will be used if instancing is supported. It is recommend to wrap as many instanced game objects into one game object to cut down on the list size.")]
	public List<GameObject> instancedGameObjects;

	[Tooltip("The list of game objects that will be used if instancing is NOT supported. It is recommended to wrap as many non-instanced game objects into one game object to cut down on the list size.")]
	public List<GameObject> nonInstancedGameObjects;

	private void Awake()
	{
		if (SystemInfo.supportsInstancing)
		{
			for (int i = 0; i < instancedGameObjects.Count; i++)
			{
				instancedGameObjects[i].SetActive(value: true);
			}
			for (int j = 0; j < nonInstancedGameObjects.Count; j++)
			{
				nonInstancedGameObjects[j].SetActive(value: false);
			}
		}
		else
		{
			for (int k = 0; k < instancedGameObjects.Count; k++)
			{
				instancedGameObjects[k].SetActive(value: false);
			}
			for (int l = 0; l < nonInstancedGameObjects.Count; l++)
			{
				nonInstancedGameObjects[l].SetActive(value: true);
			}
		}
	}
}
