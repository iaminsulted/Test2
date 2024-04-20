using System;
using System.Collections.Generic;

namespace Assets.Scripts.Game;

public class SpellVersion : IComparable<SpellVersion>
{
	public List<int> reqEffectIds = new List<int>();

	public bool isSpellOverride;

	public int reqClass;

	public int reqClassRank;

	public int combo;

	public SpellTemplate spellTemplate;

	private SpellVersion()
	{
	}

	public int CompareTo(SpellVersion other)
	{
		if (reqClass > other.reqClass)
		{
			return -1;
		}
		if (reqClass < other.reqClass)
		{
			return 1;
		}
		if (reqEffectIds.Count > other.reqEffectIds.Count)
		{
			return -1;
		}
		if (reqEffectIds.Count < other.reqEffectIds.Count)
		{
			return 1;
		}
		if (reqClassRank > other.reqClassRank)
		{
			return -1;
		}
		if (reqClassRank < other.reqClassRank)
		{
			return 1;
		}
		return 0;
	}
}
