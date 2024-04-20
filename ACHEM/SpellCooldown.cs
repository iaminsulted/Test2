public class SpellCooldown
{
	public int spellID;

	public float timeStamp;

	public float cooldown;

	public SpellCooldown()
	{
	}

	public SpellCooldown(int spellID, float timeStamp, float cooldown)
	{
		this.spellID = spellID;
		this.timeStamp = timeStamp;
		this.cooldown = cooldown;
	}
}
