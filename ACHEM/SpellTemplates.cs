using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game;

public static class SpellTemplates
{
	private static Dictionary<int, List<SpellVersion>> table;

	private static List<SpellTemplate> basePvpSpells;

	public static event Action SpellsLoaded;

	static SpellTemplates()
	{
		basePvpSpells = new List<SpellTemplate>();
		table = new Dictionary<int, List<SpellVersion>>();
	}

	public static void Init(Dictionary<int, List<SpellVersion>> spellVersions)
	{
		table = spellVersions;
		CachePvpActions();
		SpellTemplates.SpellsLoaded?.Invoke();
	}

	private static void CachePvpActions()
	{
		basePvpSpells.Clear();
		foreach (KeyValuePair<int, List<SpellVersion>> item in table)
		{
			SpellVersion spellVersion = item.Value.FirstOrDefault();
			if (spellVersion != null && spellVersion.spellTemplate.isPvpAction)
			{
				basePvpSpells.Add(GetBaseSpell(item.Key));
			}
		}
	}

	public static List<SpellTemplate> GetPvpActions()
	{
		return basePvpSpells;
	}

	public static SpellTemplate GetBaseSpell(int spellId)
	{
		return Get(spellId, null, 0, 0, 0);
	}

	public static List<SpellTemplate> GetOverrideSpells(int spellId, int classRank, int classId, int combo)
	{
		List<SpellTemplate> list = new List<SpellTemplate>();
		foreach (int overrideEffectID in GetOverrideEffectIDs(spellId, classRank))
		{
			List<int> effectIds = new List<int> { overrideEffectID };
			SpellTemplate spellFromEffectIDs = GetSpellFromEffectIDs(spellId, effectIds, classRank, classId, combo);
			if (spellFromEffectIDs != null)
			{
				list.Add(spellFromEffectIDs);
			}
		}
		return list;
	}

	public static List<int> GetOverrideEffectIDs(int spellId, int classRank)
	{
		if (!table.TryGetValue(spellId, out var value))
		{
			return new List<int>();
		}
		return value.Where((SpellVersion v) => v.reqClassRank <= classRank && v.isSpellOverride && v.reqEffectIds.Count > 0).SelectMany((SpellVersion v) => v.reqEffectIds).Distinct()
			.ToList();
	}

	public static SpellTemplate Get(int spellId, List<Effect> effects, int classRank, int classId, int combo)
	{
		List<int> effectIds = new List<int>();
		if (effects != null && effects.Count > 0)
		{
			effectIds = effects.Select((Effect e) => e.template.ID).ToList();
		}
		return GetSpellFromEffectIDs(spellId, effectIds, classRank, classId, combo);
	}

	private static SpellTemplate GetSpellFromEffectIDs(int spellId, List<int> effectIds, int classRank, int classId, int combo)
	{
		if (!table.TryGetValue(spellId, out var value))
		{
			return null;
		}
		foreach (SpellVersion item in value)
		{
			bool num = item.reqEffectIds.All(effectIds.Contains);
			bool flag = classRank >= item.reqClassRank;
			bool flag2 = item.reqClass == 0 || classId == item.reqClass;
			bool flag3 = item.combo == 0 || item.combo == combo;
			if (num && flag && flag2 && flag3)
			{
				return item.spellTemplate;
			}
		}
		return null;
	}
}
