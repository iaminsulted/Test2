using System.Collections.Generic;

public class QuestObjective
{
	public int ID;

	public int QuestID;

	public QuestObjectiveType Type;

	public int Qty;

	public string Desc;

	public int MapID;

	public string MapName;

	public int CellID;

	public int Sort;

	public bool HideArrow;

	public IndicatorFX IndicatorFX;

	public List<int> RefIDs;

	public List<int> MapIDs;

	public List<string> MapNames;

	public int RefID
	{
		get
		{
			if (RefIDs.Count > 0)
			{
				return RefIDs[0];
			}
			return 0;
		}
	}

	public bool IsMapHidden => MapIDs.Contains(344);
}
