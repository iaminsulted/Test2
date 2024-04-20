using UnityEngine;

public class UICharClassesUnlockSlider : MonoBehaviour
{
	public UISprite SliderFill;

	public UISprite SliderBackground;

	public UISprite TokenIcon;

	public UILabel TokenLabel;

	public UILabel ProgressLabel;

	public void Refresh(CharClass charClass)
	{
		CombatClass combatClass = charClass.ToCombatClass();
		int classTokenID = combatClass.ClassTokenID;
		int inventoryItemCount = Session.MyPlayerData.GetInventoryItemCount(classTokenID);
		int classTokenCost = combatClass.ClassTokenCost;
		float num = (float)inventoryItemCount / (float)classTokenCost;
		if (num > 1f)
		{
			num = 1f;
		}
		float num2 = (float)SliderBackground.width * num;
		SliderFill.width = (int)num2;
		TokenIcon.spriteName = combatClass.Icon;
		TokenLabel.text = charClass.ToCombatClass().Name + " Tokens";
		ProgressLabel.text = inventoryItemCount + "/" + classTokenCost;
	}
}
