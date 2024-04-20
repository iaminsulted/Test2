using System;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class TradeSkillDropDown : MonoBehaviour
{
	public UIGrid Grid;

	public List<GameObject> Contents = new List<GameObject>();

	public bool isOpen = true;

	public GameObject PlusIcon;

	public GameObject MinusIcon;

	public GameObject TradeSkillPrefab;

	public void Init()
	{
		foreach (TradeSkillType value in Enum.GetValues(typeof(TradeSkillType)))
		{
			if (value != TradeSkillType.Mining)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(TradeSkillPrefab, base.gameObject.transform.parent);
				gameObject.transform.SetSiblingIndex((int)(base.transform.GetSiblingIndex() + 1 + value));
				gameObject.GetComponent<UITradeSkill>().Init(value);
				Contents.Add(gameObject);
			}
		}
	}

	public void Load(Player player)
	{
		foreach (GameObject content in Contents)
		{
			content.GetComponent<UITradeSkill>().Load(player);
		}
		Refresh();
	}

	public void Clear()
	{
		foreach (GameObject content in Contents)
		{
			content.SetActive(value: false);
			UnityEngine.Object.Destroy(content.gameObject);
		}
		Contents.Clear();
		Grid.Reposition();
	}

	public void Refresh()
	{
		foreach (GameObject content in Contents)
		{
			content.SetActive(isOpen);
		}
		PlusIcon.gameObject.SetActive(!isOpen);
		MinusIcon.gameObject.SetActive(isOpen);
	}

	public void OnButtonClick()
	{
		isOpen = !isOpen;
		Refresh();
		Grid.Reposition();
	}
}
