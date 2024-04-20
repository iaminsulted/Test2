using System;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UIPartyPortrait : MonoBehaviour
{
	public const float Fade_Alpha = 0.75f;

	public UILabel lblName;

	public UILabel outOfRange;

	public UISlider hpSlider;

	public UISlider mpSlider;

	public TweenSlider hpTween;

	public TweenSlider mpTween;

	public UISprite classCircle;

	public UISprite leader;

	public UISprite ClassIcon;

	public GameObject disconnect;

	public UIWidget invisibleWidget;

	public UIGrid spellGrid;

	public GameObject spellPrefab;

	private Entity entity;

	private int memberID;

	private PlayerPartyData member;

	private ObjectPool<GameObject> spellPool;

	private List<UIActionPartySpell> partySpellBtns = new List<UIActionPartySpell>();

	private bool isHover;

	public Entity Entity
	{
		get
		{
			return entity;
		}
		private set
		{
			if (entity != value)
			{
				if (entity != null)
				{
					entity.StatUpdateEvent -= UpdateStats;
					entity.ClassUpdated -= EntityClassUpdated;
				}
				entity = value;
				if (entity != null)
				{
					entity.StatUpdateEvent += UpdateStats;
					entity.ClassUpdated += EntityClassUpdated;
					lblName.text = entity.name;
					OnMyTargetUpdated(Entities.Instance.me.Target);
					UpdateStats();
				}
			}
		}
	}

	private void OnEnable()
	{
		Entities.Instance.EntityListUpdated += OnEntityListUpdated;
		Entities.Instance.me.TargetUpdateEvent += OnMyTargetUpdated;
	}

	private void OnDisable()
	{
		Entities.Instance.EntityListUpdated -= OnEntityListUpdated;
		Entities.Instance.me.TargetUpdateEvent -= OnMyTargetUpdated;
	}

	public void Hide()
	{
		NGUITools.SetActive(base.gameObject, state: false);
	}

	private void EntityClassUpdated()
	{
		ClassIcon.spriteName = entity.ClassIcon;
		mpSlider.foregroundWidget.color = InterfaceColors.Resource.GetColor(entity);
		if (entity.resource == Entity.Resource.None)
		{
			mpSlider.value = 1f;
		}
	}

	private void OnEntityListUpdated()
	{
		Init(memberID, member);
	}

	public void Init(int ID, PlayerPartyData data)
	{
		memberID = ID;
		member = data;
		Entity = Entities.Instance.GetPlayerByName(data.name);
		if (spellPool == null)
		{
			spellPool = new ObjectPool<GameObject>(spellPrefab);
		}
		spellPrefab.SetActive(value: false);
		bool isInRange = Entity != null;
		UpdateRangeDisplay(isInRange);
		if (Entity != null)
		{
			ClassIcon.spriteName = Entity.ClassIcon;
		}
		leader.enabled = PartyManager.IsPrivate && ID == PartyManager.LeaderID;
		if (data.isDisconnected)
		{
			ShowDisconnect();
		}
		else
		{
			HideDisconnect();
		}
	}

	private void UpdateStats()
	{
		UpdateHP(entity.CalculateDisplayStat(Stat.Health), entity.CalculateDisplayStat(Stat.MaxHealth));
		UpdateRP(entity.CalculateDisplayStat(Stat.Resource), entity.CalculateDisplayStat(Stat.MaxResource));
	}

	private void UpdateHP(float hp, float hpMax)
	{
		if (hpMax > 0f && hp >= 0f)
		{
			float to = hp / hpMax;
			hpTween.tweenFactor = 0f;
			hpTween.from = hpSlider.value;
			hpTween.to = to;
			hpTween.PlayForward();
		}
	}

	private void UpdateRP(float rp, float rpMax)
	{
		if (rpMax > 0f && rp >= 0f)
		{
			float to = rp / rpMax;
			mpTween.tweenFactor = 0f;
			mpTween.from = mpSlider.value;
			mpTween.to = to;
			mpTween.PlayForward();
		}
	}

	public void OnClick()
	{
		if (entity != null)
		{
			Entities.Instance.me.Target = entity;
		}
		else
		{
			UIGame.Instance.PortraitTarget.Show(memberID, member.name);
		}
	}

	public void OnHover(bool isHover)
	{
		this.isHover = isHover;
		if (isHover)
		{
			invisibleWidget.alpha = 1f;
		}
		else if (entity == null || Entities.Instance.me.Target != entity)
		{
			invisibleWidget.alpha = 0.75f;
		}
	}

	private void OnMyTargetUpdated(Entity newTarget)
	{
		if (entity != null && entity == newTarget)
		{
			invisibleWidget.alpha = 1f;
		}
		else if (!isHover)
		{
			invisibleWidget.alpha = 0.75f;
		}
	}

	public bool IsID(int id)
	{
		return memberID == id;
	}

	public string GetMemberName()
	{
		return member.name;
	}

	private void UpdateRangeDisplay(bool isInRange)
	{
		hpSlider.gameObject.SetActive(isInRange);
		mpSlider.gameObject.SetActive(isInRange);
		outOfRange.enabled = !isInRange;
		List<SpellTemplate> castableSpells = (isInRange ? Session.MyPlayerData.MySpells.Where(Game.Instance.combat.IsSpellVisibleOnParty).ToList() : new List<SpellTemplate>());
		UpdateSpells(castableSpells);
	}

	public void ShowDisconnect()
	{
		UpdateRangeDisplay(isInRange: false);
		disconnect.SetActive(value: true);
		ClassIcon.enabled = false;
		classCircle.enabled = false;
	}

	public void HideDisconnect()
	{
		UpdateRangeDisplay(Entity != null);
		disconnect.SetActive(value: false);
		ClassIcon.enabled = true;
		classCircle.enabled = true;
	}

	public void UpdateSpells(List<SpellTemplate> castableSpells)
	{
		if (entity == null || entity.visualState == Entity.State.Dead || entity.areaID != Entities.Instance.me.areaID || entity.cellID != Entities.Instance.me.cellID || castableSpells.Count == 0)
		{
			RemoveSpellButtons(partySpellBtns);
			return;
		}
		IEnumerable<int> castableSpellIDs = castableSpells.Select((SpellTemplate spell) => spell.ID);
		List<UIActionPartySpell> spellBtns = partySpellBtns.Where((UIActionPartySpell btn) => !castableSpellIDs.Contains(btn.spellT.ID)).ToList();
		RemoveSpellButtons(spellBtns);
		UpdateSpellButtons(castableSpells);
		spellGrid.Reposition();
	}

	private void UpdateSpellButtons(List<SpellTemplate> castableSpells)
	{
		foreach (SpellTemplate spellT in castableSpells)
		{
			UIActionPartySpell uIActionPartySpell = partySpellBtns.FirstOrDefault((UIActionPartySpell btn) => btn.spellT.ID == spellT.ID);
			if (uIActionPartySpell != null)
			{
				uIActionPartySpell.Refresh();
			}
			else
			{
				AddSpellButton(spellT);
			}
		}
	}

	private void AddSpellButton(SpellTemplate spellT)
	{
		GameObject obj = spellPool.Get();
		obj.transform.SetParent(spellGrid.transform, worldPositionStays: false);
		obj.SetActive(value: true);
		UIActionPartySpell component = obj.GetComponent<UIActionPartySpell>();
		component.Init(spellT, entity);
		component.OnHoverAction = (Action<bool>)Delegate.Combine(component.OnHoverAction, new Action<bool>(OnHover));
		partySpellBtns.Add(component);
	}

	private void RemoveSpellButtons(List<UIActionPartySpell> spellBtns)
	{
		if (spellBtns != null && spellBtns.Count != 0)
		{
			for (int num = spellBtns.Count - 1; num >= 0; num--)
			{
				UIActionPartySpell uIActionPartySpell = spellBtns[num];
				uIActionPartySpell.OnHoverAction = (Action<bool>)Delegate.Remove(uIActionPartySpell.OnHoverAction, new Action<bool>(OnHover));
				spellPool.Release(spellBtns[num].gameObject);
				partySpellBtns.Remove(spellBtns[num]);
			}
		}
	}
}
