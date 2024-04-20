using System.Collections.Generic;
using UnityEngine;

public class BattleonRunePicker : MonoBehaviour
{
	public Vector3[] runePositions;

	public ParticleSystemRenderer ps;

	public List<Material> mats = new List<Material>();

	public List<int> questID;

	public void PickRune()
	{
		for (int i = 0; i < questID.Count; i++)
		{
			if (Quests.Get(questID[i]) != null && Session.MyPlayerData.IsQuestComplete(questID[i]))
			{
				base.transform.position = runePositions[i];
				ps.material = mats[i];
				break;
			}
		}
	}
}
