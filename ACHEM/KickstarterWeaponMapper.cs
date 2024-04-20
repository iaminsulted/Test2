using System;
using System.Collections.Generic;
using UnityEngine;

public class KickstarterWeaponMapper : MonoBehaviour
{
	public TextAsset Csv;

	private Dictionary<int, Dictionary<string, int>> map = new Dictionary<int, Dictionary<string, int>>();

	private void Awake()
	{
		string[] array = Csv.text.Trim().Split(new string[1] { System.Environment.NewLine }, StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Trim().Split(',');
			map.Add(int.Parse(array2[0]), new Dictionary<string, int>
			{
				{
					"shop",
					int.Parse(array2[1])
				},
				{
					"weapon",
					int.Parse(array2[2])
				}
			});
		}
		GameObject gameObject = GameObject.Find("Get Your Custom Item");
		if (map.ContainsKey(Session.MyPlayerData.UserID))
		{
			GetComponent<LoadItemById>().Id = map[Session.MyPlayerData.UserID]["weapon"];
			gameObject.GetComponent<IAUserRequired>().UserID = Session.MyPlayerData.UserID;
			gameObject.GetComponent<CTAShop>().ID = map[Session.MyPlayerData.UserID]["shop"];
			return;
		}
		bool num = Session.MyPlayerData.CheckBitFlag("iu0", 8);
		bool flag = Session.MyPlayerData.CheckBitFlag("iu0", 2);
		if (num)
		{
			GetComponent<LoadItemById>().Id = 230;
		}
		else if (flag)
		{
			GetComponent<LoadItemById>().Id = 276;
		}
		else
		{
			GetComponent<LoadItemById>().Id = 230;
		}
		gameObject.transform.parent.GetComponent<NPCIATalk>().children.RemoveAt(gameObject.transform.GetSiblingIndex());
		UnityEngine.Object.Destroy(gameObject);
	}
}
