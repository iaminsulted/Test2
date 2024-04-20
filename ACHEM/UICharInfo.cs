using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UICharInfo : UIMenuWindow
{
	public UILabel NameLabel;

	public UILabel LevelLabel;

	public UILabel XPLabel;

	public UILabel ResourceLabel;

	public UILabel HpStatLabel;

	public UILabel MpStatLabel;

	public UILabel AttackStatLabel;

	public UILabel ArmorStatLabel;

	public UILabel EvasionStatLabel;

	public UILabel CritStatLabel;

	public UILabel HasteStatLabel;

	public UILabel TotalClassRank;

	public UILabel TitleLabel;

	public UIButton btnCharPage;

	public UIGrid scrollGrid;

	public GameObject effectPrefab;

	public GameObject TitleButtonObject;

	public GameObject Pencil;

	private Transform container;

	private List<UIEffectItem> uiEffects = new List<UIEffectItem>();

	private Entity thisEntity;

	private string entityName;

	private ObjectPool<GameObject> effectPool;

	private static UICharInfo instance;

	public UILabel ClassName;

	public GameObject DescriptionObject;

	public GameObject DescriptionTextObject;

	public UILabel Description;

	public UISprite ClassIcon;

	public BuffDropDown buffDropDown;

	public GearDropDown gearDropDown;

	public GuildDropDown guildDropDown;

	public StatsDropDown statsDropDown;

	public TradeSkillDropDown tradeSkillDropDown;

	public GameObject[] levelStars;

	public Color[] totalRankColors;

	private void OnDestroy()
	{
		RemoveEntityListeners(thisEntity);
		ClearEffects();
	}

	protected override void Init()
	{
		base.Init();
		tradeSkillDropDown.Init();
	}

	public static void Show(Entity entity)
	{
		if (instance == null)
		{
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/CharInfo"), UIManager.Instance.transform).GetComponent<UICharInfo>();
			instance.Init();
		}
		if (instance.TitleButtonObject.GetComponent<UINotificationIcon>() != null)
		{
			instance.TitleButtonObject.GetComponent<UINotificationIcon>().ShouldIBeOn();
		}
		instance.Load(entity);
	}

	public static void Toggle(Entity entity)
	{
		if (instance == null)
		{
			Show(entity);
		}
		else
		{
			instance.Close();
		}
	}

	public void Load(Entity entity)
	{
		if (thisEntity == entity)
		{
			return;
		}
		if (thisEntity != null)
		{
			RemoveEntityListeners(thisEntity);
		}
		thisEntity = entity;
		entityName = entity.name;
		container = effectPrefab.transform.parent;
		effectPrefab.SetActive(value: false);
		effectPool = new ObjectPool<GameObject>(effectPrefab);
		ClearEffects();
		foreach (Effect effect in entity.effects)
		{
			AddEffect(effect);
		}
		SortEffects();
		entity.EffectAdded += EffectAdded;
		entity.StatUpdateEvent += StatsUpdated;
		UpdateStats(entity);
		btnCharPage.gameObject.SetActive(entity.type == Entity.Type.Player);
		ClassName.text = ((entity is NPC nPC) ? nPC.ElementString : GetClassIcon(entity.EquippedClassID));
		ClassIcon.spriteName = entity.ClassIcon;
		if (entity.type == Entity.Type.Player)
		{
			TotalClassRank.text = entity.TotalClassRank.ToString();
			DescriptionObject.SetActive(value: false);
			DescriptionTextObject.SetActive(value: false);
			gearDropDown.gameObject.SetActive(value: true);
			gearDropDown.Load(entity);
			tradeSkillDropDown.gameObject.SetActive(value: true);
			tradeSkillDropDown.Load(entity as Player);
			if (((Player)entity).IsInGuild)
			{
				guildDropDown.gameObject.SetActive(value: true);
				guildDropDown.Load((Player)entity);
			}
			else
			{
				guildDropDown.gameObject.SetActive(value: false);
			}
			if (entity.isMe)
			{
				ClassName.text = GetClassIcon(entity.EquippedClassID);
			}
		}
		else
		{
			if (string.IsNullOrEmpty(entity.Description))
			{
				DescriptionObject.SetActive(value: false);
				DescriptionTextObject.SetActive(value: false);
			}
			else
			{
				DescriptionObject.SetActive(value: true);
				DescriptionTextObject.SetActive(value: true);
				Description.text = entity.Description;
			}
			gearDropDown.Clear();
			gearDropDown.gameObject.SetActive(value: false);
			tradeSkillDropDown.Clear();
			tradeSkillDropDown.gameObject.SetActive(value: false);
			guildDropDown.gameObject.SetActive(value: false);
			TotalClassRank.transform.parent.gameObject.SetActive(value: false);
		}
		scrollGrid.Reposition();
	}

	public void OpenTitleWindow()
	{
		UISettings.Show(UISettings.Tab.Titles);
	}

	public void UpdateRankIcon()
	{
		int num = Session.MyPlayerData.TotalClassRank / 500;
		if (Session.MyPlayerData.TotalClassRank % 500 < 101)
		{
			num--;
		}
		int num2 = (Session.MyPlayerData.TotalClassRank - 100) / 100 % 5 + 1;
		if (Session.MyPlayerData.TotalClassRank <= 100)
		{
			num2 = 0;
		}
		if ((Session.MyPlayerData.TotalClassRank - 100) / 100 % 5 == 0 && (Session.MyPlayerData.TotalClassRank - 100) % 500 == 0)
		{
			num2 = 5;
		}
		for (int i = 0; i < 5; i++)
		{
			if (i < num2)
			{
				levelStars[i].SetActive(value: true);
				levelStars[i].GetComponent<UISprite>().color = totalRankColors[num];
			}
			else
			{
				levelStars[i].SetActive(value: false);
			}
		}
	}

	public void ViewPage()
	{
		Confirmation.OpenUrl(Main.WebAccountURL + "/Character?id=" + entityName);
	}

	private string GetClassIcon(int classID)
	{
		CombatClass classByID = CombatClass.GetClassByID(classID);
		if (classByID != null)
		{
			return classByID.Name;
		}
		return "Ico_Warrior_64";
	}

	private void UpdateStats(Entity entity)
	{
		statsDropDown.Load(entity);
		if (entity.IsInPvp && entity.CanAttack(Entities.Instance.me))
		{
			NameLabel.text = entity.CombatClass.Name;
		}
		else
		{
			NameLabel.text = entity.name;
		}
		LevelLabel.text = entity.DisplayLevel.ToString();
		if (entity.type == Entity.Type.NPC)
		{
			NPC nPC = entity as NPC;
			string text = "";
			for (int i = 0; i < nPC.elements.Count; i++)
			{
				text += nPC.elements[i];
				if (i < nPC.elements.Count - 1)
				{
					text += ", ";
				}
			}
			TotalClassRank.transform.parent.gameObject.SetActive(value: false);
			TitleButtonObject.SetActive(value: false);
			TitleLabel.text = text;
		}
		else
		{
			TotalClassRank.text = entity.TotalClassRank.ToString();
			TitleLabel.text = (string.IsNullOrEmpty(entity.TitleName) ? "[i]No Title" : entity.TitleName);
			TitleButtonObject.SetActive(value: true);
		}
		Pencil.SetActive(entity.isMe);
		MpStatLabel.text = entity.statsCurrent[Stat.MaxResource].ToString();
		ResourceLabel.text = ((entity.resource == Entity.Resource.None) ? "Max Mana" : ("Max " + entity.CombatClass.GetResourceString()));
		HpStatLabel.text = entity.CalculateDisplayStat(Stat.MaxHealth).ToString();
		AttackStatLabel.text = entity.CalculateDisplayStat(Stat.Attack).ToString();
		ArmorStatLabel.text = entity.CalculateDisplayStat(Stat.Armor).ToString();
		EvasionStatLabel.text = entity.CalculateDisplayStat(Stat.Evasion).ToString();
		CritStatLabel.text = entity.CalculateDisplayStat(Stat.Crit).ToString();
		HasteStatLabel.text = entity.CalculateDisplayStat(Stat.Haste).ToString();
	}

	private void SortEffects()
	{
		uiEffects = (from p in uiEffects
			orderby p.effect.template.isHarmful, p.effect.timestampApplied
			select p).ToList();
		int num = 0;
		foreach (UIEffectItem uiEffect in uiEffects)
		{
			uiEffect.gameObject.transform.SetSiblingIndex(num++);
			buffDropDown.Add(uiEffect.gameObject);
		}
		buffDropDown.Refresh();
		container.GetComponent<UIGrid>().Reposition();
	}

	private void AddEffect(Effect effect)
	{
		if (!effect.template.hideInList)
		{
			GameObject obj = effectPool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UICharInfoEffectItem component = obj.GetComponent<UICharInfoEffectItem>();
			component.Destroyed += OnUIEffectDestroyed;
			component.SetItem(effect, forceLarge: true);
			uiEffects.Add(component);
		}
	}

	private void ClearEffects()
	{
		for (int num = uiEffects.Count - 1; num >= 0; num--)
		{
			DestroyUIEffect(uiEffects[num]);
		}
		uiEffects.Clear();
		buffDropDown.Clear();
	}

	private void DestroyUIEffect(UIEffectItem uiEffect)
	{
		uiEffect.Destroyed -= OnUIEffectDestroyed;
		uiEffect.Clear();
		effectPool.Release(uiEffect.gameObject);
		uiEffects.Remove(uiEffect);
	}

	private void OnUIEffectDestroyed(UIEffectItem uiEffect)
	{
		DestroyUIEffect(uiEffect);
		SortEffects();
	}

	private void EffectAdded(Effect effect)
	{
		AddEffect(effect);
		SortEffects();
	}

	private void StatsUpdated()
	{
		UpdateStats(thisEntity);
	}

	private void RemoveEntityListeners(Entity entity)
	{
		entity.EffectAdded -= EffectAdded;
		entity.StatUpdateEvent -= StatsUpdated;
	}
}
