using System;
using System.Collections;
using StatCurves;
using UnityEngine;

public class UIAugmentRerollDetail : MonoBehaviour
{
	public UIItem item;

	public InventoryItem invItem;

	public UIRerollGraphDetail rerollGraphDetail;

	private int ID;

	public GameObject oddsMenu;

	public GameObject confirmButton;

	public GameObject rerollWarningBox;

	[SerializeField]
	private Transform goalPos;

	public UILabel nameLabel;

	public UILabel powerLabel;

	public UILabel modifierLabel;

	public UILabel infusionLabel;

	public UILabel goldCostLabel;

	public UILabel resultModifierLabel;

	public UISprite iconSprite;

	public UISprite ModifierSprite;

	public UISprite resultModiferSprite;

	public ItemModifier newItemMod;

	private int goldCost;

	private bool canReroll;

	private float hpControl;

	private float attackControl;

	private float armorControl;

	private float evasionControl;

	private float critControl;

	private float hasteControl;

	public static UIAugmentRerollDetail Instance { get; private set; }

	public void init(UIItem _item, float _hpControl, float _attackControl, float _armorControl, float _evasionControl, float _critControl, float _hasteControl)
	{
		hpControl = _hpControl;
		attackControl = _attackControl;
		armorControl = _armorControl;
		evasionControl = _evasionControl;
		critControl = _critControl;
		hasteControl = _hasteControl;
		item.Init(_item.Item);
		canReroll = true;
		if (_item.Item is InventoryItem inventoryItem)
		{
			invItem = inventoryItem;
		}
		confirmButton.SetActive(value: false);
		powerLabel.text = item.Item.GetCombatPower().ToString();
		modifierLabel.text = item.modifier.name;
		infusionLabel.text = item.Item.PowerOffset + "/" + item.Item.DisplayTimesInfusable;
		goldCost = Modifiers.GetRerollCost(item.Item.Level, (int)item.Item.Rarity);
		goldCostLabel.text = goldCost.ToString();
		resultModiferSprite.gameObject.SetActive(value: false);
		resultModifierLabel.gameObject.SetActive(value: false);
		StartCoroutine(showMeTheOdds());
	}

	private void augmentRerolled(ItemModifier mod)
	{
		Session.MyPlayerData.modifierRerolled -= augmentRerolled;
		resultModiferSprite.gameObject.SetActive(value: true);
		resultModifierLabel.gameObject.SetActive(value: true);
		newItemMod = mod;
		resultModifierLabel.text = mod.name;
		CalculateDiffs();
		if (mod.rarity < ItemModifier.ModifierRarity.uncommon)
		{
			resultModiferSprite.gameObject.SetActive(value: false);
		}
		else
		{
			resultModiferSprite.spriteName = ItemModifier.getModifierIcon((int)mod.rarity);
			resultModiferSprite.gameObject.SetActive(value: true);
		}
		StartCoroutine(rollDelay());
		confirmButton.SetActive(value: true);
	}

	public void sendRerollRequestSafe()
	{
		if (newItemMod != null && canReroll && newItemMod.rarity >= ItemModifier.ModifierRarity.legendary)
		{
			rerollWarningBox.SetActive(value: true);
		}
		else
		{
			SendRerollRequest();
		}
	}

	public void SendRerollRequest()
	{
		if (Session.MyPlayerData.Gold < goldCost)
		{
			Notification.ShowText("Not enough gold!");
			Chat.Notify("Not Enough Gold", "[eb2009]");
		}
		else if (canReroll)
		{
			closeRerollWarningWindow();
			canReroll = false;
			Game.Instance.SendItemModiferRerollRequest(invItem);
			Session.MyPlayerData.modifierRerolled += augmentRerolled;
		}
	}

	public void openGraph()
	{
		ItemModifier.OpenAugmentGraph(invItem.modifier);
	}

	public void openGraphResult()
	{
		ItemModifier.OpenAugmentGraph(newItemMod);
	}

	public void closeRerollWarningWindow()
	{
		if (rerollWarningBox.activeSelf)
		{
			rerollWarningBox.SetActive(value: false);
		}
	}

	private IEnumerator showMeTheOdds()
	{
		Vector3 t = oddsMenu.transform.position;
		float speed = 8f;
		while (oddsMenu.transform.position.x >= goalPos.transform.position.x)
		{
			oddsMenu.transform.position = new Vector3(Mathf.Lerp(oddsMenu.transform.position.x, goalPos.position.x, Time.deltaTime * speed), t.y, t.z);
			yield return new WaitForSeconds(0.01f);
		}
	}

	private IEnumerator rollDelay()
	{
		yield return new WaitForSeconds(0.2f);
		canReroll = true;
	}

	public void CalculateDiffs()
	{
		ItemModifier modifier = item.modifier;
		rerollGraphDetail.DisplayStatBar(modifier.maxHealth - newItemMod.maxHealth, 1, getModifierNumericalStatDiff(Stat.MaxHealth, hpControl, modifier.maxHealth, newItemMod.maxHealth));
		rerollGraphDetail.DisplayStatBar(modifier.attack - newItemMod.attack, 2, getModifierNumericalStatDiff(Stat.Attack, attackControl, modifier.attack, newItemMod.attack));
		rerollGraphDetail.DisplayStatBar(modifier.armor - newItemMod.armor, 3, getModifierNumericalStatDiff(Stat.Armor, armorControl, modifier.armor, newItemMod.armor));
		rerollGraphDetail.DisplayStatBar(modifier.evasion - newItemMod.evasion, 4, getModifierNumericalStatDiff(Stat.Evasion, evasionControl, modifier.evasion, newItemMod.evasion));
		rerollGraphDetail.DisplayStatBar(modifier.crit - newItemMod.crit, 5, getModifierNumericalStatDiff(Stat.Crit, critControl, modifier.crit, newItemMod.crit));
		rerollGraphDetail.DisplayStatBar(modifier.haste - newItemMod.haste, 6, getModifierNumericalStatDiff(Stat.Haste, hasteControl, modifier.haste, newItemMod.haste));
	}

	public float getModifierNumericalStatDiff(Stat stat, float control, float curModStat, float newModStat)
	{
		int num = (int)ItemRarity.GetItemRarity(item.Item.Rarity).levelDiff;
		return GameCurves.GetRealItemStatValue(stat, (float)Math.Round(control, 2) + (float)Math.Round(curModStat, 2), item.Item.EquipSlot, (float)(item.Item.Level + num) + (float)item.Item.PowerOffset / 20f) - GameCurves.GetRealItemStatValue(stat, (float)Math.Round(control, 2) + (float)Math.Round(newModStat, 2), item.Item.EquipSlot, (float)(item.Item.Level + num) + (float)item.Item.PowerOffset / 20f);
	}
}
