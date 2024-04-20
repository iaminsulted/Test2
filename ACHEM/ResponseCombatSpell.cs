using System.Collections.Generic;
using Assets.Scripts.Game;

public class ResponseCombatSpell : Response
{
	public int ID;

	public int code;

	public ResponseCombatSpellType rcst;

	public Entity.Type casterType;

	public int casterID;

	public int spellTemplateID;

	public List<int> pulseActionIds = new List<int>();

	public float gcdLength;

	public float cooldown;

	public float timeStamp;

	public SpellComboState comboState;

	public SpellCastData spellCastData;

	public List<ComEntityUpdate> entityUpdates = new List<ComEntityUpdate>();

	public string messageWarning;
}
