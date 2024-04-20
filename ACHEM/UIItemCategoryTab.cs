using UnityEngine;

public class UIItemCategoryTab : MonoBehaviour
{
	public static readonly Color32 normalColor = new Color32(0, 0, 0, byte.MaxValue);

	public static readonly Color32 specialColor = new Color32(19, 19, 29, byte.MaxValue);

	public UIButton CategoryButton;

	public UISprite CategoryIcon;

	public UISprite BackgroundSprite;

	public UISprite ActiveSprite;

	public UIInventory.FilterType Category;

	public void Init(UIInventory.FilterType selectedCategory, UIInventory.FilterType category, int quantity)
	{
		ActiveSprite.gameObject.SetActive(selectedCategory == category);
		Category = category;
		bool flag = Category == UIInventory.FilterType.Pet || Category == UIInventory.FilterType.TravelForm || Category == UIInventory.FilterType.Tokens || Category == UIInventory.FilterType.Resources;
		BackgroundSprite.color = (flag ? specialColor : normalColor);
		CategoryButton.defaultColor = ((quantity == 0) ? ((Color)new Color32(75, 75, 75, byte.MaxValue)) : Color.white);
	}
}
