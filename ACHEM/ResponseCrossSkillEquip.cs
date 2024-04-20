public class ResponseCrossSkillEquip : Response
{
	public int SpellID;

	public ResponseCrossSkillEquip()
	{
		type = 26;
		cmd = 3;
	}

	public ResponseCrossSkillEquip(int spellID, float cooldown)
	{
		type = 26;
		cmd = 3;
		SpellID = spellID;
	}
}
