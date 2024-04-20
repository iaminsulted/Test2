using System;
using StatCurves;
using UnityEngine;

public static class InterfaceColors
{
	public static class Names
	{
		public static readonly Color32 Staff = new Color32(byte.MaxValue, 183, 28, byte.MaxValue);

		public static readonly Color32 Mod = new Color32(248, 84, 57, byte.MaxValue);

		public static readonly Color32 Tester = new Color32(57, 130, 248, byte.MaxValue);

		public static readonly Color32 WhiteHat = new Color32(75, 248, 233, byte.MaxValue);

		public static Color32 GetColor(int accessLevel)
		{
			if (accessLevel >= 100)
			{
				return Staff;
			}
			if (accessLevel >= 60)
			{
				return Mod;
			}
			if (accessLevel >= 55)
			{
				return WhiteHat;
			}
			if (accessLevel >= 50)
			{
				return Tester;
			}
			return Color.white;
		}
	}

	public static class Chat
	{
		public static readonly Color32 White = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Dark_White = new Color32(204, 204, 204, byte.MaxValue);

		public static readonly Color32 Gray_90 = new Color32(229, 229, 229, byte.MaxValue);

		public static readonly Color32 Green = new Color32(0, 167, 0, byte.MaxValue);

		public static readonly Color32 Yellow = new Color32(byte.MaxValue, 183, 28, byte.MaxValue);

		public static readonly Color32 Red = new Color32(byte.MaxValue, 45, 45, byte.MaxValue);

		public static readonly Color32 Pink = new Color32(byte.MaxValue, 102, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Party = new Color32(95, 233, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Party_Dark = new Color32(0, 214, 248, byte.MaxValue);

		public static readonly Color32 Magenta = new Color32(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Dark_Whisper_Pink = new Color32(253, 53, 230, byte.MaxValue);

		public static readonly Color32 Whisper_Pink = new Color32(253, 87, 234, byte.MaxValue);

		public static readonly Color32 Dark_Teal = new Color32(71, 134, 118, byte.MaxValue);

		public static readonly Color32 Light_Blue = new Color32(182, 217, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Guild = new Color32(13, 181, 70, byte.MaxValue);

		public static readonly Color32 Guild_Dark = new Color32(9, 150, 57, byte.MaxValue);
	}

	public static class Popups
	{
		public static readonly Color32 Default_Top = new Color32(byte.MaxValue, 50, 75, byte.MaxValue);

		public static readonly Color32 Default_Bot = new Color32(byte.MaxValue, 12, 120, byte.MaxValue);

		public static readonly Color32 HP_Gain_Top = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

		public static readonly Color32 HP_Gain_Bot = new Color32(0, 43, 0, byte.MaxValue);

		public static readonly Color32 MP_Gain_Top = new Color32(0, 171, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 MP_Gain_Bot = new Color32(34, 34, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 My_HP_Loss_Top = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		public static readonly Color32 My_HP_Loss_Bot = new Color32(171, 0, 0, byte.MaxValue);

		public static readonly Color32 Other_HP_Loss_Top = new Color32(byte.MaxValue, byte.MaxValue, 175, byte.MaxValue);

		public static readonly Color32 Other_HP_Loss_Bot = new Color32(200, 175, 75, byte.MaxValue);

		public static readonly Color32 Other_HP_Loss_AA_Top = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Other_HP_Loss_AA_Bot = new Color32(174, 174, 174, byte.MaxValue);

		public static readonly Color32 HP_Gain_Crit_Top = new Color32(0, byte.MaxValue, 0, byte.MaxValue);

		public static readonly Color32 HP_Gain_Crit_Bot = new Color32(0, 43, 0, byte.MaxValue);

		public static readonly Color32 MP_Gain_Crit_Top = new Color32(0, 171, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 MP_Gain_Crit_Bot = new Color32(34, 34, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 My_HP_Loss_Crit_Top = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		public static readonly Color32 My_HP_Loss_Crit_Bot = new Color32(171, 0, 0, byte.MaxValue);

		public static readonly Color32 My_Normal_Text_Top = new Color32(byte.MaxValue, 251, 122, byte.MaxValue);

		public static readonly Color32 My_Normal_Text_Bot = new Color32(188, 131, 10, byte.MaxValue);

		public static readonly Color32 Other_Normal_Text_Top = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Other_Normal_Text_Bot = new Color32(174, 174, 174, byte.MaxValue);

		public static readonly Color32 My_Dots_Top = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);

		public static readonly Color32 My_Dots_Bot = new Color32(171, 0, 0, byte.MaxValue);

		public static readonly Color32 Other_Dots_Top = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Other_Dots_Bot = new Color32(174, 174, 174, byte.MaxValue);

		public static readonly Color32 My_Message_Top = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 My_Message_Bot = new Color32(174, 174, 174, byte.MaxValue);

		public static readonly Color32 Message_Top = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Message_Bot = new Color32(174, 174, 174, byte.MaxValue);

		public static readonly Color32 Other_Message_Top = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Other_Message_Bot = new Color32(174, 174, 174, byte.MaxValue);

		public static readonly Color32 Effect_Fades_Top = new Color32(180, 180, 180, byte.MaxValue);

		public static readonly Color32 Effect_Fades_Bot = new Color32(115, 115, 115, byte.MaxValue);

		public static readonly Color32 Gold_Top = new Color32(byte.MaxValue, 145, 0, byte.MaxValue);

		public static readonly Color32 Gold_Bot = new Color32(byte.MaxValue, 145, 0, byte.MaxValue);

		public static readonly Color32 XP_Top = new Color32(byte.MaxValue, 195, 0, byte.MaxValue);

		public static readonly Color32 XP_Bot = new Color32(byte.MaxValue, 195, 0, byte.MaxValue);

		public static readonly Color32 Trade_Skill_XP_Top = new Color32(0, 200, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Trade_Skill_XP_Bot = new Color32(0, 200, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Class_XP_Top = new Color32(200, 0, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Class_XP_Bot = new Color32(200, 0, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Level_Up_Top = new Color32(byte.MaxValue, 142, 0, byte.MaxValue);

		public static readonly Color32 Level_Up_Bot = new Color32(174, 97, 0, byte.MaxValue);

		public static readonly Color32 Multihit_Top = new Color32(byte.MaxValue, 183, 28, byte.MaxValue);

		public static readonly Color32 Multihit_Bot = new Color32(188, 131, 10, byte.MaxValue);
	}

	public static class EntityReaction
	{
		public static readonly Color32 Hostile = new Color32(242, 25, 20, byte.MaxValue);

		public static readonly Color32 Neutral = new Color32(byte.MaxValue, 113, 0, byte.MaxValue);

		public static readonly Color32 Friendly = new Color32(36, 165, 36, byte.MaxValue);

		public static readonly Color32 Inactive = new Color32(77, 161, 184, byte.MaxValue);

		public static readonly Color32 PvpAlly = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Party = new Color32(95, 233, byte.MaxValue, byte.MaxValue);

		public static readonly Color32 Friend = new Color32(byte.MaxValue, 221, 162, byte.MaxValue);

		public static readonly Color32 Guild = new Color32(46, 178, 53, byte.MaxValue);
	}

	public static class CastBar
	{
		public static readonly Color32 Charge = new Color32(byte.MaxValue, 195, 0, byte.MaxValue);

		public static readonly Color32 Channel = new Color32(147, 0, 227, byte.MaxValue);

		public static readonly Color32 Interrupt = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
	}

	public static class ActionButton
	{
		public static readonly Color Default_Color = new Color(1f, 1f, 1f);

		public static readonly Color Unable_To_Cast = new Color(0.4f, 0.4f, 0.4f, 1f);
	}

	public static class Portrait
	{
		public static readonly Color32 Gold = new Color32(byte.MaxValue, 204, 96, byte.MaxValue);

		public static readonly Color32 White = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}

	public static class Stats
	{
		public static readonly Color32 Red = new Color32(200, 0, 0, byte.MaxValue);

		public static readonly Color32 Green = new Color32(30, 100, 0, byte.MaxValue);
	}

	public static class Telegraphs
	{
		public static readonly Color32 Hostile = new Color(1f, 0f, 0f, 0.85f);

		public static readonly Color32 Neutral = new Color(0f, 0.46f, 0.93f, 0.85f);

		public static readonly Color32 Friendly = new Color(0f, 0.49f, 0.01f, 0.85f);

		public static readonly Color32 HostileFriendly = new Color(0.65f, 0.91f, 0.01f, 0.75f);

		public static readonly Color32 Error = new Color(1f, 0.4f, 1f, 1f);

		public static Color32 GetColor(bool isActionHarmful, bool canCasterAttackMe, bool isCasterMe)
		{
			if (isActionHarmful && canCasterAttackMe)
			{
				return Hostile;
			}
			if (!isActionHarmful && !canCasterAttackMe)
			{
				return Friendly;
			}
			if (isActionHarmful && isCasterMe)
			{
				return Neutral;
			}
			if (isActionHarmful && !canCasterAttackMe)
			{
				return Neutral;
			}
			if (!isActionHarmful && canCasterAttackMe)
			{
				return HostileFriendly;
			}
			return Error;
		}
	}

	public static class Buttons
	{
		public static Color32 Selected = new Color32(33, 33, 33, byte.MaxValue);

		public static Color32 Default = new Color32(9, 9, 9, byte.MaxValue);
	}

	public static class Resource
	{
		public static Color32 None = new Color32(100, 100, 100, byte.MaxValue);

		public static Color32 ResourceLow = new Color32(150, 150, 150, byte.MaxValue);

		public static Color32 Mana = new Color32(30, 76, byte.MaxValue, byte.MaxValue);

		public static Color32 ManaText = new Color32(75, 121, byte.MaxValue, byte.MaxValue);

		public static Color32 Spirit = new Color32(byte.MaxValue, 170, 0, byte.MaxValue);

		public static Color32 SpiritText = new Color32(168, 121, 0, byte.MaxValue);

		public static Color32 Chi = new Color32(182, 133, 13, byte.MaxValue);

		public static Color32 Determination = new Color32(239, 146, 23, byte.MaxValue);

		public static Color32 Bullets = new Color32(239, 120, 28, byte.MaxValue);

		public static Color32 Souls = new Color32(168, 84, 183, byte.MaxValue);

		public static Color32 Focus = new Color32(17, 122, 40, byte.MaxValue);

		public static Color32 Fury = new Color32(194, 103, 12, byte.MaxValue);

		public static Color32 BreathOfTheWind = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		public static Color32 GetTextColor(Entity.Resource resource)
		{
			switch (resource)
			{
			case Entity.Resource.Mana:
			case Entity.Resource.ManaNoRegen:
				return ManaText;
			case Entity.Resource.Spirit:
				return SpiritText;
			case Entity.Resource.Determination:
				return Determination;
			case Entity.Resource.Bullets:
				return Bullets;
			case Entity.Resource.Souls:
				return Souls;
			case Entity.Resource.Chi:
				return Chi;
			case Entity.Resource.Focus:
				return Focus;
			case Entity.Resource.Fury:
				return Fury;
			case Entity.Resource.BreathOfTheWind:
				return BreathOfTheWind;
			default:
				return None;
			}
		}

		public static Color32 GetColor(Entity entity)
		{
			if (entity == null)
			{
				return None;
			}
			float num = entity.statsCurrent[Stat.Resource];
			switch (entity.resource)
			{
			case Entity.Resource.Mana:
			case Entity.Resource.ManaNoRegen:
				return Mana;
			case Entity.Resource.Spirit:
				return Spirit;
			case Entity.Resource.Determination:
				if (!(num >= 50f))
				{
					return ResourceLow;
				}
				return Determination;
			case Entity.Resource.Bullets:
				return Bullets;
			case Entity.Resource.Souls:
				if (!(num >= 40f))
				{
					return ResourceLow;
				}
				return Souls;
			case Entity.Resource.Chi:
				return Chi;
			case Entity.Resource.Focus:
				return Focus;
			case Entity.Resource.Fury:
				if (!(num >= 100f))
				{
					return ResourceLow;
				}
				return Fury;
			default:
				return None;
			}
		}
	}

	public static class SpellText
	{
		public const string Name_Color = "[892800]";

		public const string Description_Color = "[000000]";

		public const string Property_Name_Color = "[892800]";

		public const string Property_Color = "[D73D00]";

		public const string Attack_Color = "[D73D00]";

		public const string Debuff_Color = "[FF0707]";

		public const string Helpful_Color = "[077007]";
	}

	public static string ToHex(this Color32 color)
	{
		return BitConverter.ToString(new byte[3] { color.r, color.g, color.b }).Replace("-", string.Empty);
	}

	public static string ToBBCode(this Color32 color)
	{
		return "[" + color.ToHex() + "]";
	}
}
