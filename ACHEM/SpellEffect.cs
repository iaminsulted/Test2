using Assets.Scripts.Game;

public class SpellEffect
{
	public int effectID;

	public int upgradeID;

	public bool hideDesc;

	public EffectTemplate effectT => EffectTemplates.Get(effectID, upgradeID);
}
