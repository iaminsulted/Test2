using System.Collections.Generic;
using System.Linq;
using StatCurves;

public class CharClass
{
	public int CharID;

	public int ClassID;

	public int ClassGlory;

	public int ClassXP;

	public bool bEquip;

	public int ClassRank => ClassRanks.GetRank(ClassXP);

	public int ClassGloryRank => ClassRanks.GetGloryRank(ClassGlory);

	public CombatClass ToCombatClass()
	{
		List<CombatClass> list = Session.MyPlayerData.combatClassList.Where((CombatClass c) => c.ID == ClassID).ToList();
		if (list == null || list.Count == 0)
		{
			return null;
		}
		return list[0];
	}
}
