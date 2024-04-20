using System.Collections.Generic;

public static class MachineSpellTemplates
{
	private static Dictionary<int, MachineSpellTemplate> table;

	static MachineSpellTemplates()
	{
		table = new Dictionary<int, MachineSpellTemplate>();
	}

	public static void Init(List<MachineSpellTemplate> spells)
	{
		foreach (MachineSpellTemplate spell in spells)
		{
			Add(spell);
		}
	}

	public static void Add(MachineSpellTemplate spellT)
	{
		if (!table.ContainsKey(spellT.ID))
		{
			table.Add(spellT.ID, spellT);
		}
	}

	public static MachineSpellTemplate Get(int id)
	{
		if (table.ContainsKey(id))
		{
			return table[id];
		}
		return null;
	}
}
