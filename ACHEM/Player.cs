using System;
using System.Collections.Generic;
using Assets.Scripts.Game;
using StatCurves;
using UnityEngine;

public class Player : Entity
{
	public long iu0;

	public long iu1;

	public int DuelOpponentID = -1;

	public float respawnTime = 1f;

	public NPC interactingNPC;

	public int guildID;

	public string guildTag;

	public string guildName;

	public DateTime DisconnectTS;

	public DateTime LastLogout;

	public int LastServerID;

	public Dictionary<TradeSkillType, int> tradeSkillLevel = new Dictionary<TradeSkillType, int>();

	public bool IsInGuild => guildID > 0;

	public override string ClassIcon
	{
		get
		{
			if (base.CombatClass != null)
			{
				return base.CombatClass.Icon;
			}
			return "Ico_Warrior_64";
		}
	}

	public override bool IsInPvp
	{
		get
		{
			if (!Game.Instance.AreaData.HasPvp)
			{
				return DuelOpponentID > 0;
			}
			return true;
		}
	}

	public event Action<TradeSkillType, int> TradeSkillLevelUpdated;

	public Player()
	{
	}

	public Player(ComEntity comEntity)
		: base(comEntity)
	{
		type = Type.Player;
		iu0 = comEntity.iu0;
		iu1 = comEntity.iu1;
		DuelOpponentID = comEntity.duelOpponentID;
		CheckPvpState();
	}

	public override void ComSync(ComEntity comEntity)
	{
		base.ComSync(comEntity);
		tradeSkillLevel = comEntity.tradeSkillLevel;
		guildID = comEntity.guildID;
		guildTag = comEntity.guildTag;
		guildName = comEntity.guildName;
	}

	protected override string GetChargeAnimationBySpell(SpellTemplate spellT, SpellAction spellAction)
	{
		string elementOfList = ArtixRandom.GetElementOfList(spellAction.chargeAnimsOverride);
		if (!string.IsNullOrEmpty(elementOfList))
		{
			return elementOfList;
		}
		string elementOfList2 = ArtixRandom.GetElementOfList(spellT.chargeAnims);
		if (!string.IsNullOrEmpty(elementOfList2))
		{
			return elementOfList2;
		}
		return "2H_ChargeLong";
	}

	protected override List<string> GetSpellAnimations(SpellTemplate spellT, SpellAction spellAction)
	{
		string[] collection = ((spellAction.animsOverride.Length == 0) ? (ShouldSpellUseAltAnim(spellT) ? spellT.animsAlt : spellT.anims) : spellAction.animsOverride);
		return new List<string>(collection);
	}

	public void EnableControl()
	{
		if (entitycontroller != null)
		{
			entitycontroller.enabled = true;
			base.wrapper.GetComponent<CharacterController>().enabled = true;
			moveController.enabled = true;
		}
	}

	public void DisableControl()
	{
		InterruptKeyframeSpell();
		if (entitycontroller != null)
		{
			entitycontroller.ResetAnimation();
			entitycontroller.StopDash();
			entitycontroller.enabled = false;
			base.wrapper.GetComponent<CharacterController>().enabled = false;
			moveController.enabled = false;
		}
	}

	public void Respawn(ComEntity comEntity)
	{
		position = new Vector3(comEntity.posX, comEntity.posY, comEntity.posZ);
		rotation = Quaternion.Euler(new Vector3(0f, (int)comEntity.rotY, 0f));
		statsBaseline.SetValues(comEntity.statsBaseline);
		statsCurrent.SetValues(comEntity.statsCurrent);
		if (comEntity.resists != null)
		{
			resists.SetValues(comEntity.resists);
		}
		isPvPFlagged = comEntity.isPvPFlagged;
		base.visualState = comEntity.state;
		base.serverState = comEntity.state;
		react = comEntity.react;
		stateEmote = comEntity.stateEmote;
		statusMap = comEntity.statusMap;
		spellCastData = comEntity.spellCastData;
		IsSheathed = comEntity.IsSheathed;
		SyncServerEffects(comEntity);
		if (effects != null)
		{
			for (int num = effects.Count - 1; num >= 0; num--)
			{
				RemoveEffect(effects[num]);
			}
		}
		if (comEntity.effects != null)
		{
			for (int num2 = comEntity.effects.Count - 1; num2 >= 0; num2--)
			{
				AddEffect(new Effect(comEntity.effects[num2]));
			}
		}
		if (base.wrapper != null)
		{
			base.wrapper.transform.SetPositionAndRotation(position, rotation);
			entitycontroller.ResetAnimation();
			entitycontroller.StopDash();
			moveController.Stop();
			moveController.enabled = true;
			AudioManager.PlayCombatSFX("UI_RespawnOrTeleport", base.isMe, base.wrapper.transform);
		}
		OnStatUpdate();
		OnRespawn();
	}

	public override Color32 GetNamePlateColor()
	{
		Player me = Entities.Instance.me;
		if (base.visualState == State.Dead)
		{
			return Color.gray;
		}
		if (type != Type.Player || CanAttack(me))
		{
			return GetReactionColor(me);
		}
		if (PartyManager.IsMember(ID))
		{
			return InterfaceColors.EntityReaction.Party;
		}
		if (Session.MyPlayerData.IsFriendsWith(name))
		{
			return InterfaceColors.EntityReaction.Friend;
		}
		if (!base.isMe && IsInGuild && guildID == me.guildID)
		{
			return InterfaceColors.EntityReaction.Guild;
		}
		return InterfaceColors.Names.GetColor(base.AccessLevel);
	}

	public string ScrubText(string str)
	{
		string text = str;
		if (!string.IsNullOrEmpty(text))
		{
			bool flag = baseAsset.gender == "M";
			if (text.Contains("@name"))
			{
				text = text.Replace("@name", name);
			}
			if (text.Contains("@NAME"))
			{
				text = text.Replace("@NAME", name.ToUpper());
			}
			if (text.Contains("@he"))
			{
				text = text.Replace("@he", flag ? "he" : "she");
			}
			if (text.Contains("@He"))
			{
				text = text.Replace("@He", flag ? "He" : "She");
			}
			if (text.Contains("@him"))
			{
				text = text.Replace("@him", flag ? "him" : "her");
			}
			if (text.Contains("@Him"))
			{
				text = text.Replace("@Him", flag ? "Him" : "Her");
			}
			if (text.Contains("@his"))
			{
				text = text.Replace("@his", flag ? "his" : "her");
			}
			if (text.Contains("@His"))
			{
				text = text.Replace("@His", flag ? "His" : "Her");
			}
			if (text.Contains("@hers"))
			{
				text = text.Replace("@hers", flag ? "his" : "hers");
			}
			if (text.Contains("@Hers"))
			{
				text = text.Replace("@Hers", flag ? "His" : "Hers");
			}
			if (text.Contains("@self"))
			{
				text = text.Replace("@self", flag ? "himself" : "herself");
			}
			if (text.Contains("@Self"))
			{
				text = text.Replace("@Self", flag ? "Himself" : "Herself");
			}
			if (text.Contains("@title"))
			{
				text = text.Replace("@title", TitleName);
			}
			if (text.Contains("@gold"))
			{
				text = text.Replace("@gold", Session.MyPlayerData.Gold.ToString("n0"));
			}
			if (text.Contains("@class"))
			{
				text = text.Replace("@class", base.CombatClass.Name);
			}
			if (text.Contains("@level"))
			{
				text = text.Replace("@level", base.Level.ToString());
			}
			if (text.Contains("@nextlevelxp"))
			{
				text = text.Replace("@nextlevelxp", Session.MyPlayerData.XPToLevel.ToString("n0"));
			}
			if (text.Contains("@xp"))
			{
				text = text.Replace("@xp", Session.MyPlayerData.XP.ToString("n0"));
			}
			if (text.Contains("@guild"))
			{
				text = text.Replace("@guild", Session.MyPlayerData.Guild.name);
			}
		}
		return text;
	}

	public override int GetExpectedStatAt(int level)
	{
		return Mathf.CeilToInt(GameCurves.GetStatCurve(level));
	}

	public override int CalculateDisplayStat(Stat stat)
	{
		switch (stat)
		{
		case Stat.MaxResource:
			return Mathf.CeilToInt(statsCurrent[stat]);
		case Stat.Resource:
			return Mathf.CeilToInt(Mathf.Clamp(statsCurrent[stat], 0f, CalculateDisplayStat(Stat.MaxResource)));
		case Stat.Health:
			return Mathf.CeilToInt((float)CalculateDisplayStat(Stat.MaxHealth) * base.HealthPercent);
		default:
		{
			float expectedStatFromLevel = GameCurves.GetExpectedStatFromLevel(base.DisplayLevel);
			float bonusStatFromLevel = GameCurves.GetBonusStatFromLevel(base.DisplayLevel);
			float num = Mathf.Ceil(GameCurves.GetExpectedStatFromLevel(base.ScaledLevel));
			AreaData areaData = Game.Instance?.AreaData;
			if (areaData == null)
			{
				return 0;
			}
			if (areaData.HasPvp)
			{
				return Mathf.RoundToInt(statsCurrent[stat] * GameCurves.GetStatCurve(base.DisplayLevel) / GameCurves.GetStatCurve(base.ScaledLevel));
			}
			float num3;
			float num4;
			if (areaData.isScaling && areaData.MinScalingLevel <= base.Level && base.Level <= areaData.MaxScalingLevel)
			{
				float num2 = (float)XP / (float)Levels.GetXPToLevel(base.Level, Session.MyPlayerData.LevelCap);
				num3 = GameCurves.GetExpectedStatFromGear((float)base.DisplayLevel + num2) / GameCurves.GetExpectedStatFromGear(base.ScaledLevel);
				num4 = statsBaseline[stat] - num;
			}
			else if (areaData.isScaling && base.Level > areaData.MaxScalingLevel)
			{
				num3 = GameCurves.GetExpectedStatFromGear(base.DisplayLevel) / GameCurves.GetExpectedStatFromGear(base.ScaledLevel);
				num4 = statsBaseline[stat] - num;
			}
			else
			{
				num3 = GameCurves.GetExpectedStatFromGear(base.DisplayLevel) / GameCurves.GetExpectedStatFromGear(base.ScaledLevel);
				num4 = statsBaseline[stat] - num - bonusStatFromLevel;
			}
			float num5 = statsCurrent[stat] - statsBaseline[stat];
			float num6 = GameCurves.GetStatCurve(base.DisplayLevel) / GameCurves.GetStatCurve(base.ScaledLevel);
			return Mathf.CeilToInt(expectedStatFromLevel + num4 * num3 + num5 * num6);
		}
		}
	}

	public override bool CanAttack(Entity target)
	{
		if (target == null || target == this || target.react == Reaction.Passive || target.react == Reaction.PassiveAggressive)
		{
			return false;
		}
		if (react == Reaction.Passive || react == Reaction.PassiveAggressive)
		{
			return false;
		}
		if (target is Player)
		{
			if (isPvPFlagged || target.isPvPFlagged)
			{
				return true;
			}
			if (DuelOpponentID > 0 && DuelOpponentID == target.ID)
			{
				return true;
			}
		}
		else if (target is NPC { summoner: not null } nPC && nPC.summoner.type == Type.Player && ((DuelOpponentID > 0 && DuelOpponentID == nPC.summoner.ID) || nPC.summoner.isPvPFlagged))
		{
			return true;
		}
		if (teamID > 0 && target.teamID > 0)
		{
			return teamID != target.teamID;
		}
		return react != target.react;
	}

	public override Color32 GetReactionColor(Entity entity)
	{
		if ((DuelOpponentID <= 0 || DuelOpponentID != entity.ID) && (!(entity is Player) || (!isPvPFlagged && !entity.isPvPFlagged)))
		{
			return base.GetReactionColor(entity);
		}
		return InterfaceColors.EntityReaction.Hostile;
	}

	public void EndNPCInteraction()
	{
		if (interactingNPC != null)
		{
			Game.Instance.SendNPCDialogueEndRequest(interactingNPC.SpawnID);
		}
	}

	public void OnTradeSkillLevelUpdated(TradeSkillType tradeSkill, int level)
	{
		if (this.TradeSkillLevelUpdated != null)
		{
			this.TradeSkillLevelUpdated(tradeSkill, level);
		}
	}

	public void SetTradeSkillLevel(TradeSkillType tradeSkill, int level)
	{
		tradeSkillLevel[tradeSkill] = level;
		OnTradeSkillLevelUpdated(tradeSkill, level);
	}

	public void SpawnBobber()
	{
		if (assetController != null && base.CastSpot != null)
		{
			(assetController as PlayerAssetController).LoadBobber(base.CastSpot);
		}
	}

	public void DespawnBobber(bool immediate)
	{
		if (assetController != null && base.CastSpot != null)
		{
			(assetController as PlayerAssetController).DestroyBobber(immediate);
		}
	}

	public void UpdateGuildIDNameTag(int guildID, string guildTag, string guildName)
	{
		this.guildID = guildID;
		this.guildTag = guildTag;
		this.guildName = guildName;
		if (base.wrapper != null)
		{
			BuildNamePlate();
		}
	}
}
