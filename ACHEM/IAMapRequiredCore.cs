public class IAMapRequiredCore : IARequiredCore
{
	public int MapID;

	public bool Not;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = Game.Instance.AreaData.id == MapID;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}
}
