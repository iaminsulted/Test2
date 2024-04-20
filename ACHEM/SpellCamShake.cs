public class SpellCamShake
{
	public enum Target
	{
		None,
		Caster,
		Target,
		All
	}

	public enum Style
	{
		None,
		Random,
		WeaponFollow,
		Jitter,
		Rotate
	}

	public enum Trigger
	{
		None,
		Impact,
		Cast
	}

	public Target target = Target.Caster;

	public float multiplier = 1f;

	public Trigger trigger = Trigger.Impact;

	public Style style = Style.Random;

	public float GetImpactMultiplier(CombatSolver.SpellResult spellResult, bool isAA)
	{
		if (spellResult == CombatSolver.SpellResult.Miss || spellResult == CombatSolver.SpellResult.Dodge)
		{
			return 0f;
		}
		float num = multiplier;
		if (isAA)
		{
			num /= 3f;
		}
		if (spellResult == CombatSolver.SpellResult.Crit)
		{
			num *= 2f;
		}
		return num;
	}
}
