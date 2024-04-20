using UnityEngine;

public class ChestItemInfoDisplay : MonoBehaviour
{
	public UIItem UIItem;

	public UIItemDetails Details;

	private void Start()
	{
		UIItem.Clicked += ShowInfo;
	}

	private void ShowInfo(UIItem item)
	{
		Details.Show();
		Details.LoadInventoryItem(item.Item);
	}

	private void OnDestroy()
	{
		UIItem.Clicked -= ShowInfo;
	}
}
