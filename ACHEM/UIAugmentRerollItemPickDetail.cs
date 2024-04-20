using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIAugmentRerollItemPickDetail : MonoBehaviour
{
	public List<InventoryItem> currentItems;

	public GameObject itemListTemplate;

	public UITable inventoryTable;

	public int CurrentBank { get; protected set; }

	public void init(GameObject itemListTemp, UITable invTable)
	{
		itemListTemplate = itemListTemp;
		inventoryTable = invTable;
	}

	public void RefreshCurrentItems()
	{
		if (CurrentBank >= 0)
		{
			currentItems = Session.MyPlayerData.allItems[CurrentBank].Where((InventoryItem x) => !x.IsHidden).ToList();
			return;
		}
		currentItems.Clear();
		foreach (KeyValuePair<int, List<InventoryItem>> allItem in Session.MyPlayerData.allItems)
		{
			if (allItem.Key != 0)
			{
				currentItems.AddRange(allItem.Value.Where((InventoryItem x) => !x.IsHidden).ToList());
			}
		}
	}

	public void populateList(List<InventoryItem> validItems)
	{
		foreach (InventoryItem validItem in validItems)
		{
			UIInventoryItem component = Object.Instantiate(itemListTemplate, inventoryTable.transform).GetComponent<UIInventoryItem>();
			component.Item = validItem;
			component.Init(component.Item);
			component.IconEquipped.gameObject.SetActive(value: false);
			component.IconCostume.gameObject.SetActive(value: false);
		}
	}
}
