using System;

namespace Assets.Scripts.Game;

public class EffectVersion : IComparable<EffectVersion>
{
	public int reqPassiveClassRank;

	public int upgradeID;

	public EffectTemplate effectTemplate;

	private EffectVersion()
	{
	}

	public int CompareTo(EffectVersion other)
	{
		if (reqPassiveClassRank > other.reqPassiveClassRank)
		{
			return -1;
		}
		if (reqPassiveClassRank < other.reqPassiveClassRank)
		{
			return 1;
		}
		if (upgradeID > other.upgradeID)
		{
			return 1;
		}
		if (upgradeID < other.upgradeID)
		{
			return -1;
		}
		return 0;
	}
}
