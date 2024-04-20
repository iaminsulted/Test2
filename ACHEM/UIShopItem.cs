using Assets.Scripts.Game;

public class UIShopItem : UIItem
{
	public UISprite Lock;

	public UISprite IconEquipped;

	public UISprite IconCostume;

	public UILabel LabelCost;

	public UISprite SpriteCurrency;

	public UISprite SpriteCurrencyFg;

	public override void Init(Item item)
	{
		base.Init(item);
		Lock.gameObject.SetActive(value: false);
		LabelCost.gameObject.SetActive(value: false);
		if (!(item is InventoryItem item2))
		{
			if (item is ShopItem item3)
			{
				InitShopItem(item3);
			}
		}
		else
		{
			InitInventoryItem(item2);
		}
	}

	private void InitInventoryItem(InventoryItem item)
	{
		if (IconEquipped != null)
		{
			IconEquipped.gameObject.SetActive(item.IsStatEquip);
		}
		if (IconCostume != null)
		{
			IconCostume.gameObject.SetActive(item.IsCosmeticEquip);
			IconCostume.spriteName = ItemIcons.GetCosmeticIconBySlot(item.EquipSlot);
		}
	}

	private void InitShopItem(ShopItem item)
	{
		IconEquipped.gameObject.SetActive(value: false);
		IconCostume.gameObject.SetActive(value: false);
		if (!item.IsAvailable())
		{
			Lock.gameObject.SetActive(value: true);
			return;
		}
		LabelCost.text = item.CurrencyCost.ToString();
		SpriteCurrency.spriteName = item.CurrencyIcon;
		if (SpriteCurrencyFg != null)
		{
			SpriteCurrencyFg.gameObject.SetActive(!string.IsNullOrEmpty(item.CurrencyIconFg));
			SpriteCurrencyFg.spriteName = item.CurrencyIconFg;
		}
		LabelCost.gameObject.SetActive(value: true);
	}
}
