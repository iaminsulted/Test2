using System.Collections.Generic;
using System.Linq;

public static class Quests
{
	private static Dictionary<int, Quest> map;

	private static Dictionary<int, QuestObjective> qoMap;

	static Quests()
	{
		map = new Dictionary<int, Quest>();
		qoMap = new Dictionary<int, QuestObjective>();
	}

	public static void Clear()
	{
		map = new Dictionary<int, Quest>();
		qoMap = new Dictionary<int, QuestObjective>();
	}

	public static void Add(int id, Quest quest)
	{
		if (map.ContainsKey(id))
		{
			return;
		}
		map.Add(id, quest);
		if (quest == null)
		{
			return;
		}
		foreach (QuestObjective objective in quest.Objectives)
		{
			qoMap.Add(objective.ID, objective);
		}
	}

	public static bool HasKey(int ID)
	{
		if (ID <= 0)
		{
			return true;
		}
		return map.ContainsKey(ID);
	}

	public static Quest Get(int ID)
	{
		if (map.ContainsKey(ID))
		{
			return map[ID];
		}
		return null;
	}

	public static QuestObjective GetObjective(int qoid)
	{
		if (qoMap.ContainsKey(qoid))
		{
			return qoMap[qoid];
		}
		return null;
	}

	public static List<Quest> GetSagaQuests(int sagaID)
	{
		return map.Values.Where((Quest x) => x != null && x.QSIndex > 0 && x.QSIndex == sagaID && x.IsSagaQuest).ToList();
	}
}
