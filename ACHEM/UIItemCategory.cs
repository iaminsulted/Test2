using UnityEngine;

public class UIItemCategory : MonoBehaviour
{
	public UISprite CategoryIcon;

	public UILabel CategoryLabel;

	public UILabel QuantityLabel;

	public UIInventory.FilterType Category;

	public void Init(UIInventory.FilterType category, int quantity)
	{
		if (quantity == 0)
		{
			CategoryLabel.color = new Color(1f, 1f, 1f, 0.2f);
			QuantityLabel.color = new Color(1f, 1f, 1f, 0.2f);
			CategoryIcon.color = new Color(1f, 1f, 1f, 0.2f);
		}
		Category = category;
		CategoryLabel.text = UIInventory.FilterLabels[Category];
		QuantityLabel.text = quantity.ToString();
	}

	private void OnItemUpdate(UIInventory.FilterType item, int quantity)
	{
		Init(item, quantity);
	}
}
