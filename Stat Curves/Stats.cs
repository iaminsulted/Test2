using System;

namespace StatCurves
{
	// Token: 0x02000012 RID: 18
	public static class Stats
	{
		// Token: 0x0600006E RID: 110 RVA: 0x000043D4 File Offset: 0x000025D4
		public static bool IsStatPrimary(Stat stat)
		{
			return stat == Stat.MaxHealth || stat == Stat.Health || stat == Stat.Attack;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000043F4 File Offset: 0x000025F4
		public static bool DoesStatScale(Stat stat)
		{
			return stat == Stat.MaxHealth || stat == Stat.Attack || stat == Stat.Armor || stat == Stat.Evasion || stat == Stat.Crit || stat == Stat.Haste;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004424 File Offset: 0x00002624
		public static bool IsStatRounded(Stat stat)
		{
			return stat == Stat.MaxHealth || stat == Stat.Health || stat == Stat.MaxResource || stat == Stat.Resource || stat == Stat.Attack || stat == Stat.Armor || stat == Stat.Evasion || stat == Stat.Crit || stat == Stat.CritPower || stat == Stat.Haste;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004464 File Offset: 0x00002664
		public static bool IsStatBonus(Stat stat)
		{
			return stat == Stat.XPBoost || stat == Stat.ClassXPBoost || stat == Stat.GoldBoost || stat == Stat.DamageBonus || stat == Stat.DefenseBonus || stat == Stat.DodgeBonus || stat == Stat.CritBonus || stat == Stat.CastSpeedBonus || stat == Stat.AoeRadiusBonus || stat == Stat.SkillSpeedBonus || stat == Stat.AASpeedBonus || stat == Stat.CooldownSpeed;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000044B8 File Offset: 0x000026B8
		public static bool AlwaysShowPercent(Stat stat)
		{
			return stat == Stat.XPBoost || stat == Stat.ClassXPBoost || stat == Stat.GoldBoost || stat == Stat.DamageBonus || stat == Stat.DodgeBonus || stat == Stat.CritBonus || stat == Stat.CastSpeedBonus || stat == Stat.AoeRadiusBonus || stat == Stat.SkillSpeedBonus || stat == Stat.AASpeedBonus || stat == Stat.CooldownSpeed;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004504 File Offset: 0x00002704
		public static bool IsBuffWhenNegative(Stat stat)
		{
			return stat == Stat.CooldownSpeed;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000451C File Offset: 0x0000271C
		public static string GetStatString(Stat stat)
		{
			string result;
			switch (stat)
			{
			case Stat.Health:
			case Stat.Resource:
			case Stat.Attack:
			case Stat.Armor:
			case Stat.Evasion:
			case Stat.Crit:
			case Stat.Haste:
				result = stat.ToString();
				break;
			case Stat.MaxHealth:
				result = "Max Health";
				break;
			case Stat.MaxResource:
				result = "Max Resource";
				break;
			case Stat.CritPower:
				result = "Critical Damage";
				break;
			case Stat.RunSpeed:
				result = "Move Speed";
				break;
			case Stat.XPBoost:
				result = "XP";
				break;
			case Stat.GoldBoost:
				result = "Gold";
				break;
			case Stat.DamageBonus:
				result = "Damage";
				break;
			case Stat.DefenseBonus:
				result = "Protection";
				break;
			case Stat.DodgeBonus:
				result = "Dodge Chance";
				break;
			case Stat.CritBonus:
				result = "Critical Chance";
				break;
			case Stat.CastSpeedBonus:
				result = "Cast Speed";
				break;
			case Stat.UltCharge:
				result = "Ultimate Charge";
				break;
			case Stat.ClassXPBoost:
				result = "Class XP";
				break;
			case Stat.AoeRadiusBonus:
				result = "Area Spell Radius";
				break;
			case Stat.SkillSpeedBonus:
				result = "Skill Speed";
				break;
			case Stat.AASpeedBonus:
				result = "Auto-Attack Speed";
				break;
			case Stat.CooldownSpeed:
				result = "Cooldown Speed";
				break;
			default:
				throw new ArgumentOutOfRangeException("stat", stat, null);
			}
			return result;
		}
	}
}
