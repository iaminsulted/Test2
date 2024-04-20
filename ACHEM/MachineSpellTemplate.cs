using System.Collections.Generic;

public class MachineSpellTemplate
{
	public int ID;

	public string impactFX;

	public List<SpellCamShake> camShakes = new List<SpellCamShake>
	{
		new SpellCamShake
		{
			target = SpellCamShake.Target.Caster,
			trigger = SpellCamShake.Trigger.Impact,
			multiplier = 1f,
			style = SpellCamShake.Style.Random
		}
	};

	public float damageMult = 1f;

	public bool isHarmful;
}
