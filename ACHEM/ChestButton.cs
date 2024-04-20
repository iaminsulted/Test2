using UnityEngine;

public class ChestButton : MonoBehaviour
{
	public UILabel ChestName;

	public UISprite ChestSprite;

	public UILabel ChestQty;

	public GameObject Highlight;

	public Item ItemChest;

	public int Qty;

	public void Selected(bool isSelected)
	{
		Highlight.SetActive(isSelected);
	}

	public void SetQty(int qty)
	{
		Qty = qty;
		ChestQty.text = Qty.ToString();
	}

	public void Init(Item item)
	{
		ItemChest = item;
		ChestName.text = "[" + item.RarityColor + "]" + item.Name.ToUpper() + "[-]";
		ChestSprite.spriteName = item.ChestSprite;
	}
}
