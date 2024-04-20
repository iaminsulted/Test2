using System;

public static class SpellSlotSpellExtensions
{
	public static int GetIndex(this CombatSpellSlot spellSlotSpell)
	{
		return (int)(CombatSpellIndex)Enum.Parse(typeof(CombatSpellIndex), spellSlotSpell.ToString());
	}
}
