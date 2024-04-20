using System.Collections.Generic;

public class ResponseSpellCooldowns : Response
{
	public List<SpellCooldown> cooldowns;

	public ResponseSpellCooldowns()
	{
	}

	public ResponseSpellCooldowns(List<SpellCooldown> cooldowns)
	{
		type = 26;
		cmd = 4;
		this.cooldowns = cooldowns;
	}
}
