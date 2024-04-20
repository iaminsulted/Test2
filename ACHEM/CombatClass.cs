using System.Collections.Generic;
using System.Linq;
using StatCurves;

public class CombatClass
{
	public int ID;

	public string Name;

	public string Description;

	public string Icon;

	public List<int> Spells;

	public int CrossSkillID;

	public int ShopID;

	public string BitFlagName = "";

	public byte BitFlagIndex;

	public Entity.Resource Resource;

	public int PassiveEffectID;

	public bool HideUnavailable;

	public bool bStaff;

	public int QSIndex;

	public int QSValue;

	public string RequirementText;

	public int ClassTokenID;

	public int ClassTokenQuantity;

	public EquipItemSlot WeaponRequired;

	public bool DualWield;

	public int SortOrder;

	public int GetTokensNpcID;

	public string GetTokensNpcName;

	public int GetTokensMapID;

	public string GetTokensMapName;

	public bool IsPreOrder;

	public int SkinClassID;

	public bool XPMultiplier;

	public int ClassTier;

	public int Difficulty;

	public Dictionary<int, ClassRankDetails> ClassRankDetails = new Dictionary<int, ClassRankDetails>();

	public List<ClassUnlockReq> UnlockReqs = new List<ClassUnlockReq>();

	public List<Item> RewardItems = new List<Item>();

	public List<Badge> RewardBadges = new List<Badge>();

	public bool HasClassTokens => ClassTokenID > 0;

	public bool NeedsTokensToUnlock
	{
		get
		{
			if (HasClassTokens)
			{
				return ClassTokenQuantity > 0;
			}
			return false;
		}
	}

	public bool IsClassFree
	{
		get
		{
			if (HasClassTokens)
			{
				return ClassTokenQuantity == 0;
			}
			return false;
		}
	}

	public bool HasClassTrainer
	{
		get
		{
			if (GetTokensNpcName != null)
			{
				return GetTokensMapName != null;
			}
			return false;
		}
	}

	public bool HasToken => ClassTokenID > 0;

	public int ClassTokenCost
	{
		get
		{
			if (!IsPreOrder || !Session.MyPlayerData.CheckBitFlag("iu0", 11))
			{
				return ClassTokenQuantity;
			}
			return 0;
		}
	}

	public bool IsSkin => SkinClassID > 0;

	public bool Equipped => ToCharClass()?.bEquip ?? false;

	public CharClass ToCharClass()
	{
		List<CharClass> list = Session.MyPlayerData.charClasses.Where((CharClass c) => c.ClassID == ID).ToList();
		if (list.Count == 0)
		{
			return new CharClass
			{
				ClassID = ID,
				CharID = -1
			};
		}
		return list[0];
	}

	public static CombatClass GetClassByID(int id)
	{
		CombatClass result = null;
		foreach (CombatClass combatClass in Session.MyPlayerData.combatClassList)
		{
			if (combatClass.ID == id)
			{
				result = combatClass;
				break;
			}
		}
		return result;
	}

	public static CombatClass GetClassByTokenID(int tokenID)
	{
		CombatClass result = null;
		foreach (CombatClass combatClass in Session.MyPlayerData.combatClassList)
		{
			if (combatClass.ClassTokenID == tokenID)
			{
				result = combatClass;
				break;
			}
		}
		return result;
	}

	public string GetResourceString(int amount = 0)
	{
		switch (Resource)
		{
		case Entity.Resource.Souls:
			if (amount != 1)
			{
				return "Souls";
			}
			return "Soul";
		case Entity.Resource.Bullets:
			if (amount != 1)
			{
				return "Gold Bullets";
			}
			return "Gold Bullet";
		case Entity.Resource.ManaNoRegen:
			return "Mana";
		case Entity.Resource.BreathOfTheWind:
			return "Breath of the Wind";
		default:
			return Resource.ToString();
		}
	}
}
