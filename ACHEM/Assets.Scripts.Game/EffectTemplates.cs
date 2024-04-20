using System.Collections.Generic;

namespace Assets.Scripts.Game;

public static class EffectTemplates
{
	private static Dictionary<int, List<EffectVersion>> table;

	static EffectTemplates()
	{
		table = new Dictionary<int, List<EffectVersion>>();
	}

	public static void Init(Dictionary<int, List<EffectVersion>> effectVersions)
	{
		table = effectVersions;
	}

	public static EffectTemplate GetBaseEffect(int effectId)
	{
		return Get(effectId, 0);
	}

	public static EffectTemplate Get(int effectId, int upgradeID, int classRank = 0)
	{
		if (!table.TryGetValue(effectId, out var value))
		{
			return null;
		}
		foreach (EffectVersion item in value)
		{
			if (item.effectTemplate.type == EffectTemplate.EffectType.Passive && classRank > 0 && classRank >= item.reqPassiveClassRank)
			{
				return item.effectTemplate;
			}
			if (item.upgradeID == upgradeID)
			{
				return item.effectTemplate;
			}
		}
		return null;
	}

	public static EffectTemplate MachineGet(int effectId, int upgradeID)
	{
		if (!table.TryGetValue(effectId, out var value))
		{
			return null;
		}
		foreach (EffectVersion item in value)
		{
			if (item.upgradeID == upgradeID)
			{
				return item.effectTemplate;
			}
		}
		return null;
	}
}
