using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Game;
using StatCurves;

public class EffectTemplate
{
	public enum Uniqueness
	{
		Once,
		PerUser
	}

	public enum EffectType
	{
		Combat,
		User,
		Proc,
		Passive,
		Map,
		Event,
		Perma,
		PermaOnline,
		Session,
		Pvp,
		Guild
	}

	public enum EffectGroup
	{
		None,
		Food,
		Visual,
		GuildXP,
		GuildGold,
		GuildAtk,
		GuildDef
	}

	public enum ShaderFxType
	{
		None,
		Ghost
	}

	public const string Debuff_Color = "[FF0707]";

	public const string Helpful_Color = "[077007]";

	public int ID;

	public string name;

	public string desc = "";

	public string icon;

	public float duration = -1f;

	public List<EffectUpgrade> effectUpgrades = new List<EffectUpgrade>();

	public EffectType type;

	public EffectGroup effectGroup;

	public bool isHarmful;

	public Hashtable shaderFX;

	public string particleFX;

	public string stackFX;

	public int spellHighlightID = -1;

	public CombatSolver.DamageModel damageModel;

	public float damageMult;

	public bool showDamageToCaster;

	public bool applyImmediately;

	public bool hideIcon;

	public bool hideText;

	public bool hideInList;

	public Entity.StatusType status;

	public DamageMod damageMod;

	public List<StatMod> statMods = new List<StatMod>();

	public int maxStack = 1;

	public bool isTravelForm;

	public bool randomizeBones;

	public float entityScaleFactor;

	public bool isWeaponSheathed;

	public EffectDurationEnd durationEnd;

	public bool onlyAffectsHealth
	{
		get
		{
			if (statMods.Count != 0)
			{
				return statMods.All((StatMod mod) => mod.stat == Stat.Health);
			}
			return true;
		}
	}

	public string ToolTipText => GetDescription();

	public int GetUpgradeID()
	{
		return effectUpgrades.FirstOrDefault((EffectUpgrade u) => u.reqPassiveClassRank == 0)?.ID ?? 0;
	}

	public string GetDescription()
	{
		return string.Concat(string.Concat(string.Concat("" + (isHarmful ? "[FF0707]" : "[077007]"), name, "[-]"), "\n[000000]", desc, "[-]"), GetBaseEffectInfo(Entities.Instance.me.ScaledLevel));
	}

	public string GetBaseEffectInfo(int level)
	{
		string powerDescription = GetPowerDescription();
		powerDescription += GetStatModDescriptions(level);
		powerDescription += GetDamageModDescription();
		if (duration > 0f)
		{
			powerDescription = powerDescription + "\n[892800]Duration: [-][D73D00]" + ArtixString.FormatDuration(duration) + "[-]";
			if (type == EffectType.Perma)
			{
				powerDescription += "[D73D00](Continues offline)[-]";
			}
			else if (type == EffectType.PermaOnline)
			{
				powerDescription += "[D73D00](Paused when offline)[-]";
			}
		}
		if (maxStack > 1)
		{
			powerDescription = powerDescription + "\n[892800]Max Stacks: [-][D73D00]" + maxStack + "[-]";
		}
		return powerDescription;
	}

	private string GetStatModDescriptions(int level)
	{
		string text = "";
		foreach (StatMod item in statMods.Where((StatMod mod) => !mod.hideDesc))
		{
			if (item.percent == 0f && item.flat == 0f)
			{
				continue;
			}
			text += "\n";
			if (item.percent != 0f)
			{
				float num = item.percent * 100f;
				string text2 = "%";
				if (item.type == StatMod.ModType.Expected)
				{
					num = (int)((float)Entities.Instance.me.GetExpectedStatAt(level) * item.percent);
					text2 = "";
				}
				string text3;
				if (Stats.IsBuffWhenNegative(item.stat))
				{
					text3 = ((item.percent < 0f) ? "[077007]+" : "[FF0707]");
					num *= -1f;
				}
				else
				{
					text3 = ((item.percent > 0f) ? "[077007]+" : "[FF0707]");
				}
				text = text + text3 + num + text2;
			}
			if (item.flat != 0f)
			{
				float num2 = (Stats.IsStatBonus(item.stat) ? (item.flat * 100f) : item.flat);
				string text4;
				if (Stats.IsBuffWhenNegative(item.stat))
				{
					text4 = ((item.flat < 0f) ? "[077007]+" : "[FF0707]");
					num2 *= -1f;
				}
				else
				{
					text4 = ((item.flat > 0f) ? "[077007]+" : "[FF0707]");
				}
				string text5 = (Stats.AlwaysShowPercent(item.stat) ? "%" : "");
				text = text + text4 + num2 + text5;
			}
			text = text + " " + Stats.GetStatString(item.stat) + "[-]";
		}
		return text;
	}

	private string GetDamageModDescription()
	{
		if (damageMod == null)
		{
			return "";
		}
		if (damageMod.requirements != null && damageMod.requirements.Count > 0)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder("\n");
		if (damageMod.multiplier >= 0f)
		{
			stringBuilder.Append("[077007]");
			stringBuilder.Append('+');
		}
		else
		{
			stringBuilder.Append("[FF0707]");
		}
		stringBuilder.Append(damageMod.multiplier * 100f);
		stringBuilder.Append("% ");
		stringBuilder.Append(damageMod.direction);
		stringBuilder.Append(' ');
		if (damageMod.source != 0 && damageMod.source != CombatSolver.DamageSource.Spell)
		{
			stringBuilder.Append(damageMod.source);
			stringBuilder.Append(' ');
		}
		stringBuilder.Append(damageMod.type);
		stringBuilder.Append("[-]");
		return stringBuilder.ToString();
	}

	private string GetPowerDescription()
	{
		string text = "";
		if (damageMult > 0f && damageModel == CombatSolver.DamageModel.DoT)
		{
			text = text + "\n[892800]Attack Power: [-][D73D00]" + damageMult * 100f + "%[-]";
		}
		else if (damageMult > 0f && damageModel == CombatSolver.DamageModel.HoT)
		{
			text = text + "\n[892800]Heal Power: [-][077007]" + damageMult * 100f + "%[-]";
		}
		return text;
	}
}
