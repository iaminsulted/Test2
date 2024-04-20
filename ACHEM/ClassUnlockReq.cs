public class ClassUnlockReq
{
	public int ClassReqID;

	public int ClassRank;

	public string Description
	{
		get
		{
			if (ClassReqID > 0)
			{
				return CombatClass.GetClassByID(ClassReqID).Name + ((ClassRank == 0) ? " Unlocked" : (" Rank " + ClassRank));
			}
			return "None";
		}
	}

	public bool PlayerMeetsReq()
	{
		if (ClassReqID > 0 && Session.MyPlayerData.GetClassRank(ClassReqID) < ClassRank)
		{
			return false;
		}
		return true;
	}
}
