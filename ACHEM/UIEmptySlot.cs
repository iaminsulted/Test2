using UnityEngine;

public class UIEmptySlot : MonoBehaviour
{
	public UILabel NameLabel;

	public UILabel InfoLabel;

	public UISprite Icon;

	public void Init(UIInventory.FilterType filterType)
	{
		NameLabel.text = "No " + filterType.ToString() + " Equipped";
		InfoLabel.text = "Make a selection";
		Icon.spriteName = "Icon-" + filterType;
	}
}
