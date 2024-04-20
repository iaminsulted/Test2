using Assets.Scripts.Game;

public class SpellActionDelayEvent : SequencedEvent
{
	public KeyframeSpellData frameData;

	public SpellAction spellAction;

	public Entity.ImpactSource impactSource;
}
