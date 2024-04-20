public class IAUserRequiredCore : IARequiredCore
{
	public int UserID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = UserID == myPlayerData.UserID;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}
}
