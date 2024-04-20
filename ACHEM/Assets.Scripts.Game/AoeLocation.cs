using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Game;

public class AoeLocation
{
	public int ID;

	public int actionId;

	public int aoeId;

	public bool isAura;

	public ComVector3 position;

	public ComVector3 randomOffset;

	public float yRotation;

	public Entity.Type aoeSourceType;

	public int aoeSourceId;

	[JsonIgnore]
	public Quaternion rotation => Quaternion.Euler(new Vector3(0f, yRotation, 0f));

	public AoeLocation()
	{
	}

	public AoeLocation(int actionId, int aoeId, bool isAura, ComVector3 position, ComVector3 randomOffset, float yRotation, Entity aoeSource)
	{
		ID = -1;
		this.actionId = actionId;
		this.aoeId = aoeId;
		this.isAura = isAura;
		this.position = position;
		this.randomOffset = randomOffset;
		this.yRotation = yRotation;
		aoeSourceType = aoeSource.type;
		aoeSourceId = aoeSource.ID;
	}
}
