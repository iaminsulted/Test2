using Assets.Scripts.Game;
using UnityEngine;

public class UIInventoryItem : UIItem
{
	public static Color32 checkGreen = new Color32(0, 118, 0, byte.MaxValue);

	public static Color32 checkGray = new Color32(118, 118, 118, byte.MaxValue);

	public UISprite IconEquipped;

	public UISprite IconCostume;

	public UISprite IconNew;

	public UIEmptySlot EmptySlot;

	public GameObject InvItemParent;

	public UIInventory.FilterType FilterType { get; private set; }

	public override void Init(Item item)
	{
		base.Init(item);
		if (item is InventoryItem inventoryItem)
		{
			ShowIcon(inventoryItem);
			ShowNotif(inventoryItem);
			ShowModifier(inventoryItem);
		}
		else
		{
			if (IconEquipped != null)
			{
				IconEquipped.gameObject.SetActive(value: false);
			}
			if (IconCostume != null)
			{
				IconCostume.gameObject.SetActive(value: false);
			}
			if (IconNew != null)
			{
				IconNew.gameObject.SetActive(value: false);
			}
		}
		if (EmptySlot != null)
		{
			InvItemParent.SetActive(value: true);
			EmptySlot.gameObject.SetActive(value: false);
		}
	}

	public void InitAsEquipSlot(UIInventory.FilterType filterType)
	{
		Item = null;
		FilterType = filterType;
		EmptySlot.Init(filterType);
		EmptySlot.gameObject.SetActive(value: true);
		InvItemParent.SetActive(value: false);
	}

	private void ShowIcon(InventoryItem inventoryItem)
	{
		if (IconEquipped != null)
		{
			IconEquipped.color = checkGreen;
			IconEquipped.gameObject.SetActive(inventoryItem.IsStatEquip);
		}
		if (IconCostume != null)
		{
			IconCostume.gameObject.SetActive(inventoryItem.IsCosmeticEquip);
			IconCostume.spriteName = ItemIcons.GetCosmeticIconBySlot(inventoryItem.EquipSlot);
		}
	}

	private void ShowNotif(InventoryItem inventoryItem)
	{
		if (IconNew != null)
		{
			IconNew.gameObject.SetActive(inventoryItem.IsNew);
		}
	}

	private void ShowModifier(InventoryItem inventoryItem)
	{
		if (inventoryItem.modifier != null && inventoryItem.IsEquipType && !inventoryItem.IsCosmetic && inventoryItem.modifier.rarity >= ItemModifier.ModifierRarity.uncommon)
		{
			hasModifier = true;
			modifierRarity = (int)inventoryItem.modifier.rarity;
		}
		if (!hasModifier || !inventoryItem.HasStats)
		{
			ModifierSprite.gameObject.SetActive(value: false);
			ModifierLabel.gameObject.SetActive(value: false);
			InfoLabel.gameObject.SetActive(value: true);
			InfoLabel.text = inventoryItem.GetTagline();
		}
		else
		{
			ModifierLabel.text = inventoryItem.GetTagline();
			ModifierSprite.spriteName = UIItem.setModifierIcon(modifierRarity);
			ModifierSprite.gameObject.SetActive(value: true);
			ModifierLabel.gameObject.SetActive(value: true);
			InfoLabel.gameObject.SetActive(value: false);
		}
	}
}
