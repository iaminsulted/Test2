public class UICharInfoEffectItem : UIEffectItem
{
	public UILabel lblDesc;

	public override void SetItem(Effect newEffect, bool forceLarge = false)
	{
		base.SetItem(newEffect, forceLarge);
		string text = (newEffect.template.isHarmful ? "[FF0707]" : "[077007]");
		lblDesc.text = text + newEffect.template.name + ":[-] [000000]" + newEffect.template.desc + "[-]";
	}

	public new void OnTooltip(bool show)
	{
		if (show)
		{
			Tooltip.ShowAtPosition(effect.template.ToolTipText, UIGame.Instance.MouseToolTipPosition, UIWidget.Pivot.TopRight);
		}
		else
		{
			Tooltip.Hide();
		}
	}
}
