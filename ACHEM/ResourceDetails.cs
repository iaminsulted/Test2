public class ResourceDetails
{
	public UISprite Icon;

	public UILabel ResourceName;

	public UILabel ResourceDescription;

	public void Init(CombatClass combatClass)
	{
		Icon.spriteName = "Icon-Passive";
		ResourceName.text = combatClass.GetResourceString(2);
		ResourceDescription.text = "";
	}
}
