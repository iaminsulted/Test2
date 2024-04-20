using UnityEngine;

public class UIClassSkill : MonoBehaviour
{
	public UILabel labelName;

	public UILabel labelDescription;

	public UISprite sprite;

	public SpellTemplate spellT;

	public void Init(SpellTemplate spellT)
	{
		labelName.text = spellT.name;
		labelDescription.text = spellT.desc;
		sprite.spriteName = spellT.Icon;
		this.spellT = spellT;
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			Tooltip.ShowAtPosition(spellT.ToolTipText, UIGame.Instance.FixedToolTipPosition);
		}
		else
		{
			Tooltip.Hide();
		}
	}
}
