using UnityEngine;

public class UIGuildShopItem : UIItem
{
	public enum GuildPowerCategory
	{
		Armor,
		Xp,
		Gold,
		Attack
	}

	public UILabel ItemNameLabel;

	public UILabel BuffListLabel;

	public UILabel CostLabel;

	public UILabel LevelRequiredLabel;

	public Item item;

	public UIGuild uiGuild;

	public GameObject BuyButtonObject;

	public UIButton BuyButton;

	public UILabel BuyLabel;

	public GameObject GoldIcon;

	public UISprite BuyButtonSprite;

	public GameObject Lock;

	public GameObject ArmorBoostIcon;

	public GameObject XpBoostIcon;

	public GameObject GoldBoostIcon;

	public GameObject AttackBoostIcon;

	private GuildPowerCategory guildPowerCategory;

	public void Init(Item item, int currentLevel, int levelRequired, GuildPowerCategory guildPowerCategory)
	{
		this.item = item;
		ItemNameLabel.text = item.Name;
		CostLabel.text = item.Cost.ToString();
		BuffListLabel.text = item.GetDescriptionForGuildPower().Trim();
		ArmorBoostIcon.SetActive(value: false);
		XpBoostIcon.SetActive(value: false);
		GoldBoostIcon.SetActive(value: false);
		AttackBoostIcon.SetActive(value: false);
		this.guildPowerCategory = guildPowerCategory;
		switch (guildPowerCategory)
		{
		case GuildPowerCategory.Armor:
			ArmorBoostIcon.SetActive(value: true);
			break;
		case GuildPowerCategory.Xp:
			XpBoostIcon.SetActive(value: true);
			break;
		case GuildPowerCategory.Gold:
			GoldBoostIcon.SetActive(value: true);
			break;
		case GuildPowerCategory.Attack:
			AttackBoostIcon.SetActive(value: true);
			break;
		}
		if (currentLevel < levelRequired)
		{
			BuyButtonSprite.color = Color.black;
			BuyButton.enabled = false;
			BuyLabel.color = Color.gray;
			LevelRequiredLabel.gameObject.SetActive(value: true);
			Lock.SetActive(value: true);
			LevelRequiredLabel.text = "LVL " + levelRequired;
			ItemNameLabel.color = new Color(0.1f, 0.1f, 0.1f, 1f);
		}
	}

	public void BuyButtonClicked()
	{
		UIGuild.ClickedItemPowerCategory = guildPowerCategory;
		uiGuild.ShowBuyConfirmation(item);
	}
}
