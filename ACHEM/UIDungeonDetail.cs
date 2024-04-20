using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonDetail : MonoBehaviour
{
	private enum Mode
	{
		Default,
		ChoosePrivate
	}

	private DungeonData dungeon;

	public UILabel lblTitle;

	public UILabel lblLevel;

	public UILabel lblDesc;

	public UILabel lblRequirement;

	public UILabel lblPrivate;

	public UIGrid grid;

	public UITable table;

	public GameObject itemGOprefab;

	public UIButton btnEnterPublic;

	public UIButton btnEnterPrivate;

	public UIButton btnPrivateClose;

	public UIButton btnClose;

	public UILabel lblLeftAction;

	public UILabel lblRightAction;

	private Transform container;

	private List<UIDungeonLootItem> itemGOs;

	private ObjectPool<GameObject> itemGOpool;

	private Mode mode;

	private bool visible = true;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				base.gameObject.SetActive(visible);
			}
		}
	}

	private void OnEnable()
	{
		Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnEnterPublic.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(EnterPublicClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnEnterPrivate.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(EnterPrivateClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnPrivateClose.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(ClosePrivate));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(CloseClick));
	}

	private void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnEnterPublic.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(EnterPublicClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnEnterPrivate.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(EnterPrivateClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnPrivateClose.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(ClosePrivate));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnClose.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(CloseClick));
	}

	public void Init()
	{
		container = itemGOprefab.transform.parent;
		if (itemGOs == null)
		{
			itemGOs = new List<UIDungeonLootItem>();
		}
		if (itemGOpool == null)
		{
			itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		}
		itemGOprefab.SetActive(value: false);
		lblPrivate.enabled = false;
		btnPrivateClose.gameObject.SetActive(value: false);
		mode = Mode.Default;
	}

	public void Init(DungeonData data)
	{
		dungeon = data;
		lblRequirement.text = GetRestrictionText();
		lblRequirement.gameObject.SetActive(!IsAvailable(data));
		lblDesc.text = data.map.Description;
		lblTitle.text = data.map.DisplayName;
		lblLevel.text = $"Level {data.map.MinLevel}+";
		if (data.map.IsScaling)
		{
			lblLevel.text += " (Scaled)";
		}
		if (data.map.powerRestriction > 0)
		{
			lblLevel.text += $"\nAverage Power {data.map.powerRestriction}+";
		}
		UILabel uILabel = lblLevel;
		uILabel.text = uILabel.text + "\nDifficulty: " + (data.map.IsChallenge ? "Challenge" : "Normal");
		if (data.map.MaxUsers == 1)
		{
			lblLevel.text += "\nSolo Dungeon";
		}
		else
		{
			string text = "1-" + data.map.MaxUsers;
			if (!data.map.IsDynamicScaling)
			{
				text = data.map.MaxUsers.ToString();
			}
			UILabel uILabel2 = lblLevel;
			uILabel2.text = uILabel2.text + "\nRecommended Players: " + text;
		}
		mode = Mode.Default;
		UpdateDungeonButtons();
		foreach (UIDungeonLootItem itemGO in itemGOs)
		{
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		foreach (Item item in data.items)
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIDungeonLootItem component = obj.GetComponent<UIDungeonLootItem>();
			component.Init(item);
			itemGOs.Add(component);
		}
		grid.Reposition();
		table.Reposition();
		Visible = true;
	}

	private void UpdateDungeonButtons()
	{
		switch (mode)
		{
		case Mode.Default:
			btnEnterPublic.isEnabled = dungeon.map.MaxUsers > 1;
			btnPrivateClose.gameObject.SetActive(value: false);
			lblPrivate.enabled = false;
			lblLeftAction.text = "Enter\nPublic Dungeon";
			if (dungeon.map.MaxUsers == 1)
			{
				lblRightAction.text = "Enter\nSolo Dungeon";
			}
			else if (PartyManager.IsInParty)
			{
				lblRightAction.text = "Enter Private\nParty Dungeon";
			}
			else
			{
				lblRightAction.text = "Enter\nPrivate Dungeon";
			}
			break;
		case Mode.ChoosePrivate:
			btnEnterPublic.isEnabled = true;
			btnPrivateClose.gameObject.SetActive(value: true);
			lblPrivate.enabled = true;
			lblLeftAction.text = "Enter\nScaled";
			lblRightAction.text = "Enter\nUnscaled";
			break;
		}
	}

	private void ClosePrivate(GameObject go)
	{
		mode = Mode.Default;
		UpdateDungeonButtons();
	}

	private void CloseClick(GameObject go)
	{
		Hide();
	}

	public void Hide()
	{
		Visible = false;
	}

	private bool IsAvailable(DungeonData data)
	{
		if (data.map.levelRestriction > Entities.Instance.me.Level)
		{
			return false;
		}
		if (data.map.powerRestriction > Session.MyPlayerData.AverageItemPower)
		{
			return false;
		}
		if (data.map.upgradeRestriction > -1 && !Session.MyPlayerData.CheckBitFlag("iu0", (byte)data.map.upgradeRestriction))
		{
			return false;
		}
		if (data.map.questRestriction > -1 && Session.MyPlayerData.GetQSValue(data.map.questRestriction) < data.map.questValue)
		{
			return false;
		}
		return true;
	}

	private string GetRestrictionText()
	{
		if (dungeon.map.levelRestriction > Entities.Instance.me.Level)
		{
			return "You must reach Level " + dungeon.map.levelRestriction + " to enter.";
		}
		if (dungeon.map.powerRestriction > Session.MyPlayerData.AverageItemPower)
		{
			return "Your average Power must be " + dungeon.map.powerRestriction + "+ to enter.";
		}
		if (dungeon.map.upgradeRestriction > -1 && !Session.MyPlayerData.CheckBitFlag("iu0", (byte)dungeon.map.upgradeRestriction))
		{
			if (!(dungeon.map.RequirementText == ""))
			{
				return dungeon.map.RequirementText;
			}
			return "This area requires an upgrade to join.";
		}
		if (dungeon.map.questRestriction > -1 && Session.MyPlayerData.GetQSValue(dungeon.map.questRestriction) < dungeon.map.questValue)
		{
			if (!(dungeon.map.RequirementText == ""))
			{
				return dungeon.map.RequirementText;
			}
			return "You have not completed the quest requirements for this area.";
		}
		return "";
	}

	private bool CanEnter(bool isPublic)
	{
		if (dungeon.map.levelRestriction > Entities.Instance.me.Level)
		{
			MessageBox.Show("Level Requirement", GetRestrictionText());
			return false;
		}
		if (dungeon.map.powerRestriction > Session.MyPlayerData.AverageItemPower)
		{
			MessageBox.Show("Power Requirement", GetRestrictionText());
			return false;
		}
		if (dungeon.map.upgradeRestriction > -1 && !Session.MyPlayerData.CheckBitFlag("iu0", (byte)dungeon.map.upgradeRestriction))
		{
			MessageBox.Show("Upgrade Requirement", GetRestrictionText());
			return false;
		}
		if (dungeon.map.questRestriction > -1 && Session.MyPlayerData.GetQSValue(dungeon.map.questRestriction) < dungeon.map.questValue)
		{
			MessageBox.Show("Quest Requirement", GetRestrictionText());
			return false;
		}
		if (isPublic && PartyManager.IsInParty)
		{
			if (!PartyManager.IsLeader)
			{
				MessageBox.Show("Party Join", "The party leader must join the dungeon to enter as a party.");
				return false;
			}
			if (dungeon.map.MaxUsers < PartyManager.MemberCount)
			{
				MessageBox.Show("Party Too Big", "Your party is too large for this dungeon. Max size: " + dungeon.map.MaxUsers + ".");
				return false;
			}
		}
		return true;
	}

	private void EnterPublicClick(GameObject go)
	{
		bool isPrivate = mode == Mode.ChoosePrivate;
		if (PartyManager.IsLeader)
		{
			Confirmation.Show("Join as Party", "Your party will be asked to follow you into the dungeon. Continue?", delegate(bool b)
			{
				if (b)
				{
					TryEnter(isPrivate, enterScaled: true);
				}
			});
		}
		else
		{
			TryEnter(isPrivate, enterScaled: true);
		}
	}

	private void EnterPrivateClick(GameObject go)
	{
		if ((!PartyManager.IsInParty || PartyManager.IsLeader) && mode == Mode.Default && dungeon.map.IsScaling && !dungeon.map.IsSeasonal)
		{
			mode = Mode.ChoosePrivate;
			UpdateDungeonButtons();
		}
		else
		{
			TryEnter(isPrivateDungeon: true, enterScaled: false);
		}
	}

	private void TryEnter(bool isPrivateDungeon, bool enterScaled)
	{
		if (CanEnter(isPrivateDungeon))
		{
			Game.Instance.SendAreaJoinRequest(dungeon.map.ID, isPrivateDungeon, enterScaled);
			AudioManager.Play2DSFX("sfx_engine_key");
		}
	}
}
