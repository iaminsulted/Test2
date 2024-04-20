public class RequestCrossSkillEquip : Request
{
	public int SpellID;

	public RequestCrossSkillEquip()
	{
		type = 26;
		cmd = 3;
	}

	public RequestCrossSkillEquip(int spellID)
	{
		type = 26;
		cmd = 3;
		SpellID = spellID;
	}
}
