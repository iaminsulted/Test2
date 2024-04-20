using System.Collections.Generic;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseQuestMeta : Response
{
	public Dictionary<int, int> QuestObjectives;

	public Dictionary<int, QuestString> QuestStrings;

	public Dictionary<int, int> QuestChains;
}
