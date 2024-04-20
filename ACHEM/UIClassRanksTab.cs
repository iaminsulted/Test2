using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIClassRanksTab : MonoBehaviour, IRefreshable
{
	public UIScrollView ScrollView;

	public UIGrid RankTable;

	public GameObject ClassRankItemTemplate;

	public void Refresh(CharClass charClass)
	{
		ClassRankItemTemplate.SetActive(value: false);
		int i = 1;
		for (int childCount = RankTable.transform.childCount; i < childCount; i++)
		{
			RankTable.transform.GetChild(i).gameObject.SetActive(value: false);
			Object.Destroy(RankTable.transform.GetChild(i).gameObject);
		}
		ClassRankDetails[] array = (from x in charClass.ToCombatClass().ClassRankDetails
			orderby x.Key
			select x.Value).ToArray();
		foreach (ClassRankDetails rankDetails in array)
		{
			UIClassRankItem component = Object.Instantiate(ClassRankItemTemplate, RankTable.transform).GetComponent<UIClassRankItem>();
			component.Init(rankDetails);
			component.gameObject.SetActive(value: true);
		}
		RankTable.Reposition();
		ScrollView.ResetPosition();
	}

	public void Refresh()
	{
		RankTable.Reposition();
		ScrollView.ResetPosition();
	}
}
