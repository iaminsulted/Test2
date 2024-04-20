using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPartyHUD : MonoBehaviour
{
	public UIGrid grid;

	public List<UIPartyPortrait> portraits;

	private void OnEnable()
	{
		InitPortraits();
		PartyManager.PartyUpdated += InitPortraits;
		Game.Instance.combat.CDTrigger += OnCDTrigger;
		Game.Instance.combat.ResetSpellCD += OnResetSpellCooldown;
		Game.Instance.combat.SpellsUpdated += OnSpellUpdated;
		Entities.Instance.EntityListUpdated += OnEntityListUpdated;
		Entities.Instance.me.StatUpdateEvent += OnMyStatsUpdated;
		Entities.Instance.me.EffectAdded += OnMyEffectAdded;
		Entities.Instance.me.EffectRemoved += OnMyEffectRemoved;
		Entities.Instance.me.ServerStateChanged += OnMyStateChanged;
		Session.MyPlayerData.ClassRankUpdated += OnMyClassRankUpdated;
		Session.MyPlayerData.ClassEquipped += OnMyClassEquipped;
		SpellTemplates.SpellsLoaded += OnSpellsLoaded;
	}

	private void OnDisable()
	{
		PartyManager.PartyUpdated -= InitPortraits;
		if (Game.Instance?.combat != null)
		{
			Game.Instance.combat.CDTrigger -= OnCDTrigger;
			Game.Instance.combat.ResetSpellCD -= OnResetSpellCooldown;
			Game.Instance.combat.SpellsUpdated -= OnSpellUpdated;
		}
		if (Entities.Instance?.me != null)
		{
			Entities.Instance.EntityListUpdated -= OnEntityListUpdated;
			Entities.Instance.me.StatUpdateEvent -= OnMyStatsUpdated;
			Entities.Instance.me.EffectAdded -= OnMyEffectAdded;
			Entities.Instance.me.EffectRemoved -= OnMyEffectRemoved;
			Entities.Instance.me.ServerStateChanged -= OnMyStateChanged;
		}
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ClassRankUpdated -= OnMyClassRankUpdated;
			Session.MyPlayerData.ClassEquipped -= OnMyClassEquipped;
		}
		SpellTemplates.SpellsLoaded -= OnSpellsLoaded;
	}

	private void InitPortraits()
	{
		if (!PartyManager.IsInParty)
		{
			foreach (UIPartyPortrait portrait in portraits)
			{
				portrait.UpdateSpells(new List<SpellTemplate>());
			}
			grid.gameObject.SetActive(value: false);
			return;
		}
		grid.gameObject.SetActive(value: true);
		List<KeyValuePair<int, PlayerPartyData>> list = PartyManager.MemberData.Where((KeyValuePair<int, PlayerPartyData> p) => p.Key != Entities.Instance.me.ID).ToList();
		for (int i = 0; i < portraits.Count; i++)
		{
			if (i >= list.Count)
			{
				portraits[i].gameObject.SetActive(value: false);
				continue;
			}
			portraits[i].gameObject.SetActive(value: true);
			portraits[i].Init(list[i].Key, list[i].Value);
		}
	}

	private void UpdatePartySpells()
	{
		if (!PartyManager.IsInParty)
		{
			return;
		}
		List<SpellTemplate> castableSpells = Session.MyPlayerData.MySpells.Where(Game.Instance.combat.IsSpellVisibleOnParty).ToList();
		foreach (UIPartyPortrait portrait in portraits)
		{
			portrait.UpdateSpells(castableSpells);
		}
	}

	private void OnCDTrigger(int spellID, float cooldown)
	{
		UpdatePartySpells();
		StopCoroutine("CooldownRoutine");
		StartCoroutine(CooldownRoutine(cooldown));
	}

	private IEnumerator CooldownRoutine(float cooldown)
	{
		yield return new WaitForSecondsRealtime(cooldown);
		UpdatePartySpells();
	}

	private void OnResetSpellCooldown(int _, float __)
	{
		UpdatePartySpells();
	}

	private void OnSpellUpdated(Dictionary<InputAction, int> _)
	{
		UpdatePartySpells();
	}

	private void OnEntityListUpdated()
	{
		UpdatePartySpells();
	}

	private void OnMyStatsUpdated()
	{
		UpdatePartySpells();
	}

	private void OnMyEffectAdded(Effect _)
	{
		UpdatePartySpells();
	}

	private void OnMyEffectRemoved(Effect _)
	{
		UpdatePartySpells();
	}

	private void OnMyStateChanged(Entity.State oldState, Entity.State newState)
	{
		UpdatePartySpells();
	}

	private void OnMyClassRankUpdated(int _, int __)
	{
		UpdatePartySpells();
	}

	private void OnMyClassEquipped(int _)
	{
		UpdatePartySpells();
	}

	private void OnSpellsLoaded()
	{
		UpdatePartySpells();
	}
}
