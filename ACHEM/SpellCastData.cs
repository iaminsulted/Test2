using System.Collections.Generic;
using Assets.Scripts.Game;
using Newtonsoft.Json;

public class SpellCastData
{
	public enum State
	{
		None,
		Charging,
		Channeling
	}

	[JsonIgnore]
	public SpellTemplate spellT;

	public int spellTemplateID;

	public int npcSpellID;

	public float chargeTime;

	public float channelTime;

	public float ts;

	public State state;

	public List<AoeLocation> aoeLocations = new List<AoeLocation>();

	public void Init(SpellTemplate spellT)
	{
		this.spellT = spellT;
		spellTemplateID = spellT.ID;
	}
}
