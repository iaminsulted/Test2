using Newtonsoft.Json;

namespace Assets.Scripts.Game;

public class SpellComboState
{
	[JsonProperty]
	private int autoAttackCombo;

	[JsonProperty]
	private int spellCombo;

	[JsonProperty]
	private int lastSpellID;

	public int Get(int spellID)
	{
		SpellTemplate baseSpell = SpellTemplates.GetBaseSpell(spellID);
		if (baseSpell == null)
		{
			return 0;
		}
		if (baseSpell.isAA)
		{
			return autoAttackCombo;
		}
		if (lastSpellID == spellID)
		{
			return spellCombo;
		}
		return 0;
	}

	public void ResetCombos()
	{
		ResetAutoAttackCombo();
		ResetSpellCombo();
	}

	private void ResetAutoAttackCombo()
	{
		autoAttackCombo = 0;
	}

	private void ResetSpellCombo()
	{
		spellCombo = 0;
		lastSpellID = 0;
	}
}
