using System.Linq;
using Assets.Scripts.Game;

public class ClassRankDetails
{
	public int ClassID;

	public int ClassRank;

	public string Description;

	public CombatSpellSlot SpellSlot;

	public bool IsUnlock;

	public int RewardID;

	public int RewardType;

	public bool IsPassive => SpellSlot == CombatSpellSlot.Passive;

	public CombatClass CombatClass => Session.MyPlayerData.combatClassList.FirstOrDefault((CombatClass x) => x.ID == ClassID);

	public string Icon
	{
		get
		{
			if (RewardType == 2)
			{
				return Items.Get(RewardID).Icon;
			}
			if (RewardType == 3)
			{
				return "Icon_Badge_Rank_100";
			}
			if (IsPassive)
			{
				return "Icon-Passive";
			}
			return Spell.icon ?? "";
		}
	}

	public int SpellID
	{
		get
		{
			if (!IsPassive)
			{
				return CombatClass.Spells[SpellSlot.GetIndex()];
			}
			return CombatClass.PassiveEffectID;
		}
	}

	public SpellTemplate Spell
	{
		get
		{
			if (!IsPassive)
			{
				return SpellTemplates.Get(SpellID, null, ClassRank, ClassID, 0);
			}
			return null;
		}
	}

	public EffectTemplate Passive
	{
		get
		{
			int passiveEffectID = CombatClass.PassiveEffectID;
			if (!IsPassive)
			{
				return null;
			}
			return EffectTemplates.GetBaseEffect(passiveEffectID);
		}
	}

	public string GetDescription(bool usePastTense = false)
	{
		string text = (usePastTense ? "Unlocked " : "Unlock the ");
		text = ((RewardType == 2) ? (text + Items.Get(RewardID).Name) : ((RewardType == 3) ? (text + Badges.Get(RewardID).Title + " Title") : (IsPassive ? (text + Passive.name + " passive ability") : (IsUnlock ? (Spell.isUlt ? (text + Spell.name + " Ultimate skill") : ((Spell.ID != CombatClass.CrossSkillID) ? (text + Spell.name + " skill") : (text + Spell.name + " Cross Skill"))) : ((!usePastTense) ? (Spell.name + " - " + Description) : ("Upgraded " + Spell.name))))));
		if (usePastTense)
		{
			text += "!";
		}
		return text;
	}
}
