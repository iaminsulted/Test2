using System;
using System.Collections.Generic;

public class ApopData
{
	public int ID;

	public int ParentID;

	public int Type;

	public int SortingOrder;

	public string Icon;

	public string Label;

	public bool IsAutoTrigger;

	public string Action;

	public string Requirements;

	public string NpcName;

	public string NpcTitle;

	public string ImageUrl;

	public bool DontHideWhenLocked;

	public string RequirementsText;

	public int sagaID;

	public string ImageTitle;

	public string ImageDescription;

	public bool bStaff;

	public DateTime DateStart;

	public DateTime DateEnd;

	public List<int> ChildrenIDs = new List<int>();

	public void SafeAddChildren(int id)
	{
		if (!ChildrenIDs.Contains(id))
		{
			ChildrenIDs.Add(id);
		}
	}
}
