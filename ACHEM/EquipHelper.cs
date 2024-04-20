using System;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class EquipHelper : MonoBehaviour
{
	[Serializable]
	public class EquipTag
	{
		public EquipItemSlot EquipItemSlot;

		public string equipTag;

		public List<GameObject> gameObjects;
	}

	public List<EquipTag> equipTags = new List<EquipTag>();

	private void Start()
	{
	}

	private void Update()
	{
	}
}
