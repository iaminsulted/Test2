using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class NPCIA
{
	public int ID = -1;

	public string Label;

	[HideInInspector]
	public string Title;

	[HideInInspector]
	public string Subtitle;

	[FormerlySerializedAs("IsSingleUse")]
	public bool IsAutoTrigger;

	public string ImageUrl;

	[HideInInspector]
	public bool DontHideWhenLocked;

	public string RequirementsText;

	public NPCIA parent;

	public List<NPCIA> children = new List<NPCIA>();

	public List<ClientTriggerActionCore> Actions = new List<ClientTriggerActionCore>();

	public List<IARequiredCore> Requirements = new List<IARequiredCore>();

	public int SortingOrder;

	public int sagaID;

	public string ImageTitle;

	public string ImageDesc;

	public bool bStaff;

	public DateTime DateStart;

	public DateTime DateEnd;

	public int tempInstanceID;

	private bool initialized;

	private static int CurrentInstanceID;

	public abstract string Icon { get; }

	public virtual string CurrentLabel => Label;

	public virtual ApopState ApopState => ApopState.Talk;

	public virtual Quest TurnInQuest => null;

	public int QuestEndDialogID
	{
		get
		{
			if (TurnInQuest != null && TurnInQuest.EndDialogID > 0)
			{
				return TurnInQuest.EndDialogID;
			}
			return 0;
		}
	}

	public event Action RequirementUpdated;

	public static event Action<NPCIA> OnInitialize;

	public static int GetNextInstanceID()
	{
		return ++CurrentInstanceID;
	}

	public NPCIA()
	{
		tempInstanceID = GetNextInstanceID();
	}

	protected virtual void Init()
	{
		if (initialized)
		{
			return;
		}
		if (IsAutoTrigger)
		{
			IASessionDataRequiredCore iASessionDataRequiredCore = new IASessionDataRequiredCore();
			iASessionDataRequiredCore.Key = GetSessionDataID();
			iASessionDataRequiredCore.Value = false;
			Requirements.Add(iASessionDataRequiredCore);
		}
		foreach (IARequiredCore requirement in Requirements)
		{
			requirement.Updated += OnRequirementUpdated;
		}
		initialized = true;
	}

	private void OnRequirementUpdated()
	{
		if (this.RequirementUpdated != null)
		{
			this.RequirementUpdated();
		}
	}

	public bool IsRequirementMet()
	{
		foreach (IARequiredCore requirement in Requirements)
		{
			if (!requirement.IsRequirementMet(Session.MyPlayerData))
			{
				return false;
			}
		}
		return true;
	}

	public bool StaffAccessCheck()
	{
		if (bStaff)
		{
			if (bStaff && Session.MyPlayerData.AccessLevel >= 100)
			{
				return Main.Environment == Environment.Content;
			}
			return false;
		}
		return true;
	}

	public bool IsTriggerAvailable()
	{
		if (IsAutoTrigger && IsRequirementMet())
		{
			return StaffAccessCheck();
		}
		return false;
	}

	public virtual bool IsAvailable()
	{
		if (!IsAutoTrigger && IsRequirementMet())
		{
			return StaffAccessCheck();
		}
		return false;
	}

	public string GetSessionDataID()
	{
		int num = ((ID == -1) ? tempInstanceID : ID);
		return "Area:" + Game.CurrentAreaID + "Cell:" + Game.CurrentCellID + "Apop:" + num;
	}

	private void getSelfAndChildren(NPCIA apop, List<NPCIA> selfAndChildren)
	{
		selfAndChildren.Add(apop);
		if (children == null)
		{
			return;
		}
		foreach (NPCIA child in apop.children)
		{
			getSelfAndChildren(child, selfAndChildren);
		}
	}

	public List<NPCIA> GetSelfAndRelated()
	{
		List<NPCIA> list = new List<NPCIA>();
		getSelfAndChildren(this, list);
		return list;
	}

	public T FindChildRecursive<T>(int ID) where T : NPCIA
	{
		if (this.ID == ID && this is T)
		{
			return (T)this;
		}
		if (children == null || children.Count == 0)
		{
			return null;
		}
		T val = (T)children.Find((NPCIA x) => x is T && x.ID == ID);
		if (val != null)
		{
			return val;
		}
		foreach (NPCIA child in children)
		{
			val = child.FindChildRecursive<T>(ID);
			if (val != null)
			{
				return val;
			}
		}
		return null;
	}

	public bool ContainsChild(int ID)
	{
		if (children == null || children.Count == 0)
		{
			return false;
		}
		if (children.Find((NPCIA x) => x.ID == ID) != null)
		{
			return true;
		}
		foreach (NPCIA child in children)
		{
			if (child.ContainsChild(ID))
			{
				return true;
			}
		}
		return false;
	}

	public void InitializeSelfAndChildren()
	{
		Init();
		if (children == null || children.Count == 0)
		{
			return;
		}
		foreach (NPCIA child in children)
		{
			child.parent = this;
			child.InitializeSelfAndChildren();
		}
	}
}
