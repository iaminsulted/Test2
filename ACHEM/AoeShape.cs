using Newtonsoft.Json;
using StatCurves;
using UnityEngine;

public class AoeShape
{
	public enum Type
	{
		Circle,
		Cone,
		Line
	}

	public const float Default_Height = 10f;

	public int ID;

	public bool isAura;

	public Type type;

	public float xOffset;

	public float zOffset;

	public int startAngle;

	public float width;

	public float height = 10f;

	public float radius;

	public float minRadius;

	public string mainTexture;

	public bool isFixed;

	public float randomOffsetRadius;

	public string aoeFX;

	public string aoeSFX;

	[JsonIgnore]
	public Vector3 StartPosition => new Vector3(xOffset, 0f, zOffset);

	public float GetRadius(Entity caster, Entity aoeSource)
	{
		float num = radius;
		if (caster is NPC nPC)
		{
			NpcSpell currentNpcSpell = nPC.GetCurrentNpcSpell();
			if (currentNpcSpell != null)
			{
				num *= currentNpcSpell.SpellOptions.aoeRadiusMultiplier;
			}
		}
		if (caster == aoeSource)
		{
			num += caster.combatRadius;
		}
		return num * caster.statsCurrent[Stat.AoeRadiusBonus];
	}

	public float GetMinRadius(Entity caster, Entity aoeSource)
	{
		float num = minRadius;
		if (caster is NPC nPC)
		{
			NpcSpell currentNpcSpell = nPC.GetCurrentNpcSpell();
			if (currentNpcSpell != null)
			{
				num *= currentNpcSpell.SpellOptions.aoeRadiusMultiplier;
			}
		}
		if (caster == aoeSource && num > 0f)
		{
			num += caster.combatRadius;
		}
		return num;
	}

	public Vector3 GetPositionBySource(Entity caster, Entity aoeSource)
	{
		Quaternion forwardDirection = GetForwardDirection(caster, aoeSource);
		Vector3 vector = new Vector3(xOffset, 0f, zOffset);
		return aoeSource.position + forwardDirection * vector;
	}

	public Quaternion GetRotationBySource(Entity caster, Entity aoeSource)
	{
		return GetForwardDirection(caster, aoeSource) * Quaternion.Euler(0f, startAngle, 0f);
	}

	public float GetYRotation(Entity caster, Entity aoeSource)
	{
		return GetRotationBySource(caster, aoeSource).eulerAngles.y;
	}

	private static Quaternion GetForwardDirection(Entity caster, Entity aoeSource)
	{
		Vector3 forward = aoeSource.position - caster.position;
		forward.y = 0f;
		Quaternion result = caster.rotation;
		if (caster != aoeSource)
		{
			result = Quaternion.LookRotation(forward);
		}
		return result;
	}

	public BaseTelegraph GetTelegraph(Entity caster, Entity aoeSource)
	{
		BaseTelegraph baseTelegraph = null;
		float num = GetRadius(caster, aoeSource);
		float num2 = GetMinRadius(caster, aoeSource);
		return type switch
		{
			Type.Circle => new CircleTelegraph(StartPosition, startAngle, num2, num, mainTexture), 
			Type.Cone => new ConeTelegraph(StartPosition, startAngle, width, num2, num, mainTexture), 
			Type.Line => new RectTelegraph(StartPosition, startAngle, new Vector2(width, num - num2)), 
			_ => null, 
		};
	}
}
