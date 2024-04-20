using System.Collections.Generic;
using Assets.Scripts.Game;

public class DamageMod
{
	public enum Type
	{
		Damage,
		Healing
	}

	public enum Direction
	{
		Outgoing,
		Incoming
	}

	public Type type;

	public CombatSolver.DamageSource source;

	public Direction direction;

	public float multiplier;

	public List<SpellRequirement> requirements = new List<SpellRequirement>();
}
