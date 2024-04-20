using System.Collections.Generic;
using Assets.Scripts.Game;

namespace AQ3DServer.GameServer.CommClasses;

public class ComAura
{
	public int auraId;

	public int spellTemplateId;

	public int spellActionId;

	public int casterId;

	public Entity.Type casterType;

	public List<AoeLocation> aoeLocations;
}
