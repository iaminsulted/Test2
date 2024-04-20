using System.Collections.Generic;

public class CasterSequencedEvent : SequencedEvent
{
	public string animLabel;

	public float length;

	public List<Entity> targets;

	public float castSpeed = 1f;

	public SpellTemplate spellT;
}
