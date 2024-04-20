using System.Collections.Generic;
using AQ3DServer.GameServer.CommClasses;
using Assets.Scripts.Game;
using UnityEngine;

public class Aura
{
	public enum Operation
	{
		Remove,
		Add
	}

	public readonly int ID;

	public readonly Entity caster;

	public readonly AuraTemplate template;

	public readonly SpellTemplate spellT;

	public readonly SpellAction spellAction;

	public readonly List<AoeLocation> aoeLocations = new List<AoeLocation>();

	private List<GameObject> allAoeFx = new List<GameObject>();

	public Aura(ComAura comAura)
	{
		if (comAura != null)
		{
			ID = comAura.auraId;
			caster = Entities.Instance.GetEntity(comAura.casterType, comAura.casterId);
			if (caster != null)
			{
				spellT = SpellTemplates.Get(comAura.spellTemplateId, caster.effects, caster.ScaledClassRank, caster.EquippedClassID, 0);
				spellAction = spellT.GetActionById(comAura.spellActionId);
				template = spellAction.aura;
				aoeLocations = comAura.aoeLocations ?? new List<AoeLocation>();
			}
		}
	}

	public void Init()
	{
		CreateFx();
	}

	private void CreateFx()
	{
		foreach (AoeLocation aoeLocation in aoeLocations)
		{
			if (aoeLocation == null)
			{
				continue;
			}
			AoeShape aoeById = spellAction.GetAoeById(aoeLocation.aoeId, aoeLocation.isAura);
			if (aoeById != null)
			{
				if (aoeById.isFixed)
				{
					CreateFixedFx(aoeLocation, aoeById);
				}
				else
				{
					CreateFollowFx(aoeLocation, aoeById);
				}
			}
		}
	}

	private void CreateFixedFx(AoeLocation aoeLocation, AoeShape aoe)
	{
		if (string.IsNullOrEmpty(aoe.aoeFX))
		{
			return;
		}
		GameObject gameObject = SpellFXContainer.mInstance.CreateFXAtPosition(aoe.aoeFX, aoeLocation.position + aoeLocation.randomOffset, aoeLocation.rotation, spellAction);
		if (!(gameObject == null))
		{
			allAoeFx.Add(gameObject);
			if (!string.IsNullOrEmpty(aoe.aoeSFX))
			{
				AudioManager.PlayCombatSFX(aoe.aoeSFX, caster.isMe, gameObject.transform);
			}
		}
	}

	private void CreateFollowFx(AoeLocation aoeLocation, AoeShape aoe)
	{
		Entity entity = Entities.Instance.GetEntity(aoeLocation.aoeSourceType, aoeLocation.aoeSourceId);
		List<GameObject> list = SpellFXContainer.mInstance.CreateEntityFX(entity, caster.isMe, aoe.aoeFX);
		if (list == null)
		{
			return;
		}
		foreach (GameObject item in list)
		{
			allAoeFx.Add(item);
			if (!string.IsNullOrEmpty(aoe.aoeSFX))
			{
				AudioManager.PlayCombatSFX(aoe.aoeSFX, caster.isMe || entity.isMe, item.transform);
			}
		}
	}

	public void Destroy()
	{
		foreach (GameObject item in allAoeFx)
		{
			if (item != null)
			{
				Object.Destroy(item);
			}
		}
	}
}
