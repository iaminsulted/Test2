using Assets.Scripts.Game;
using UnityEngine;

public class PassiveSkillDetails : MonoBehaviour
{
	public UISprite Icon;

	public UILabel PassiveName;

	public UILabel PassiveDescription;

	private EffectTemplate effectT;

	public void Init(int effectID)
	{
		effectT = EffectTemplates.GetBaseEffect(effectID);
		Icon.spriteName = "Icon-Passive";
		PassiveName.text = effectT.name;
		PassiveDescription.text = effectT.desc;
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			Tooltip.ShowAtPosition(effectT.ToolTipText, UIGame.Instance.FixedToolTipPosition);
		}
		else
		{
			Tooltip.Hide();
		}
	}
}
