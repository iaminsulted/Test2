using StatCurves;
using UnityEngine;

namespace Assets.Scripts.UI;

public class UIClassGloryItem : UIItem
{
	public UISlider glorySlider;

	public UILabel gloryLabel;

	public UILabel gloryRank;

	public UISprite gloryXpIcon;

	public int ID;

	public CombatClass combatClass;

	public Color32 ColorOwned;

	public void Init(CombatClass combatClass)
	{
		this.combatClass = combatClass;
		Refresh();
	}

	public void Refresh()
	{
		Icon.spriteName = combatClass.Icon;
		CharClass charClass = combatClass.ToCharClass();
		if (Session.MyPlayerData.OwnsClass(combatClass.ID))
		{
			Icon.color = Color.white;
			float num;
			float num2;
			if (charClass.ClassGloryRank < 2)
			{
				num = charClass.ClassGlory;
				num2 = ClassRanks.GetGloryToRankUp(charClass.ClassGloryRank);
			}
			else
			{
				num = charClass.ClassGlory - ClassRanks.GetGloryToRankUp(charClass.ClassGloryRank - 1);
				num2 = ClassRanks.GetGloryToRankUp(charClass.ClassGloryRank) - ClassRanks.GetGloryToRankUp(charClass.ClassGloryRank - 1);
			}
			glorySlider.value = num / num2;
			gloryLabel.text = num + " / " + num2;
			gloryRank.text = charClass.ClassGloryRank.ToString();
		}
		else
		{
			Icon.color = Color.gray;
			Background.color = Color.gray;
			IconForeground.color = Color.gray;
		}
	}
}
