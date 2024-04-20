public class IAPlayerStateRequiredCore : IARequiredCore
{
	public byte State;

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		Player me = Entities.Instance.me;
		if (me == null)
		{
			return false;
		}
		bool flag = me.serverState == Entity.State.Dead;
		if (!flag || State != 0)
		{
			if (!flag)
			{
				return State != 0;
			}
			return false;
		}
		return true;
	}
}
