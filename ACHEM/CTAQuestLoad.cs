using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Quest Load")]
public class CTAQuestLoad : ClientTriggerAction
{
	[FormerlySerializedAs("QuestIDs")]
	public List<int> AcceptQuestIDs = new List<int>();

	[FormerlySerializedAs("QuestIDs")]
	public List<int> TurnInQuestIDs = new List<int>();

	protected override void OnExecute()
	{
		UIQuest.ShowQuests(AcceptQuestIDs, TurnInQuestIDs);
	}

	public bool AreQuestAvailable()
	{
		foreach (int item in AcceptQuestIDs.Union(TurnInQuestIDs).ToList())
		{
			Quest quest = Quests.Get(item);
			if (quest == null)
			{
				if (!Quests.HasKey(item))
				{
					return true;
				}
			}
			else if (Session.MyPlayerData.IsQuestAvailable(quest) && ((Session.MyPlayerData.IsQuestComplete(quest.ID) && TurnInQuestIDs.Contains(quest.ID)) || AcceptQuestIDs.Contains(quest.ID)))
			{
				return true;
			}
		}
		return false;
	}
}
