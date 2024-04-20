using StatCurves;
using UnityEngine;

public class UITradeSkill : MonoBehaviour
{
	public UILabel level;

	public UILabel skill;

	public UILabel xp;

	public UISlider xpBar;

	public UISprite icon;

	private TradeSkillType type;

	public void Init(TradeSkillType type)
	{
		this.type = type;
		switch (type)
		{
		case TradeSkillType.Fishing:
			skill.text = "Fishing";
			icon.spriteName = "Icon-FishingRod";
			break;
		case TradeSkillType.Mining:
			skill.text = "Mining (Coming Soon)";
			icon.spriteName = "Icon-FishingRod";
			break;
		}
	}

	public void Load(Player player)
	{
		if (player.isMe)
		{
			int num = Session.MyPlayerData.tradeSkillXP[type];
			int num2 = Session.MyPlayerData.tradeSkillXPToLevel[type];
			xp.text = num + " / " + num2;
			xpBar.value = (float)num / (float)num2;
			xp.gameObject.SetActive(value: true);
		}
		else
		{
			xpBar.value = 0f;
			xp.gameObject.SetActive(value: false);
		}
		if (player.tradeSkillLevel[type] >= 50)
		{
			xpBar.value = 1f;
			xp.gameObject.SetActive(value: false);
		}
		level.text = player.tradeSkillLevel[type].ToString();
	}
}
